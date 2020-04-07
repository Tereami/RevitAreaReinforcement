using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Structure;

namespace RevitAreaReinforcement
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CommandCreateAreaRebar : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            if (string.IsNullOrEmpty(App.assemblyFolder))
            {
                App.assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                App.assemblyFolder = System.IO.Path.GetDirectoryName(App.assemblyPath);
            }

            string programdataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string localFolder = System.IO.Path.Combine(programdataPath, "RibbonBimStarter", "WallReinforcement");
            string wallPath = System.IO.Path.Combine(localFolder, "wall.json");
            string floorPath = System.IO.Path.Combine(localFolder, "floor.json");
            if(!System.IO.File.Exists(wallPath))
            {
                string appWallPath = System.IO.Path.Combine(App.assemblyFolder, "wall.json");
                System.IO.File.Copy(appWallPath, wallPath);
            }
            if (!System.IO.File.Exists(floorPath))
            {
                string appFloorPath = System.IO.Path.Combine(App.assemblyFolder, "floor.json");
                System.IO.File.Copy(appFloorPath, floorPath);
            }




            Document doc = commandData.Application.ActiveUIDocument.Document;

            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            List<ElementId> selIds = sel.GetElementIds().ToList();
            if (selIds.Count == 0)
            {
                TaskDialog.Show("Error", "Выберите стены или плиты");
                return Result.Failed;
            }

            List<Wall> walls = new List<Wall>();
            List<Floor> floors = new List<Floor>();
            foreach (ElementId id in selIds)
            {
                Element elem = doc.GetElement(id);

                if (elem is Wall) walls.Add(elem as Wall);
                if (elem is Floor) floors.Add(elem as Floor);
            }
            if (walls.Count == 0 && floors.Count == 0)
            {
                message = "Выберите стены или плиты";
                return Result.Failed;
            }
            if (walls.Count != 0 && floors.Count != 0)
            {
                message = "Одновременно можно армировать только стены или плиты";
                return Result.Failed;
            }

            ElementId areaTypeId = SupportDocumentGetter.GetDefaultArea(doc).Id;
            RebarCoverType zeroCover = SupportDocumentGetter.GetRebarCoverType(doc, 0);

            List<string> rebarTypes = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .OfClass(typeof(RebarBarType))
                .Cast<RebarBarType>()
                .Select(i => i.Name)
                .OrderBy(i => i)
                .ToList();
            List<string> rebarTypes2 = rebarTypes.ToList();


            bool wallsHaveRebarInfo = SupportDocumentGetter.CheckWallsHaveRebarInfo(walls);
            if (walls.Count > 0)
            {
                
                string wallDeserialize = System.IO.File.ReadAllText(wallPath);
                RebarInfoWall riw = Newtonsoft.Json.JsonConvert.DeserializeObject<RebarInfoWall>(wallDeserialize);

                if (wallsHaveRebarInfo)
                {
                    TaskDialog.Show("Внимание!", "Армирование будет выполнено по данным, указанным в стенах, без вывода диалогового окна.");
                }
                else
                {
                    DialogWindowWall form = new DialogWindowWall(riw, rebarTypes, rebarTypes2);
                    form.ShowDialog();
                    if (form.DialogResult != System.Windows.Forms.DialogResult.OK) return Result.Cancelled;

                    wallDeserialize = Newtonsoft.Json.JsonConvert.SerializeObject(riw, Newtonsoft.Json.Formatting.Indented);
                    System.IO.File.WriteAllText(wallPath, wallDeserialize);
                }

                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Армирование стен");

                    foreach (Wall wall in walls)
                    {
                        if (wallsHaveRebarInfo)
                        {
                            RebarInfoWall newRiw = new RebarInfoWall(doc, wall);
                            newRiw.topOffset = riw.topOffset;
                            newRiw.bottomOffset = riw.bottomOffset;
                            newRiw.backOffset = riw.backOffset;
                            newRiw.horizontalAddInterval = riw.horizontalAddInterval;
                            newRiw.verticalSectionText = riw.verticalSectionText;
                            newRiw.horizontalSectionText = riw.horizontalSectionText;

                            RebarWorkerWall.GenerateRebar(doc, wall, newRiw, zeroCover, areaTypeId);
                        }
                        else
                        {
                            RebarWorkerWall.GenerateRebar(doc, wall, riw, zeroCover, areaTypeId);
                        }

                        
                    }
                    t.Commit();
                }
                
            }


            if (floors.Count > 0)
            {
                
                string floorDeserialize = System.IO.File.ReadAllText(floorPath);
                RebarInfoFloor rif = Newtonsoft.Json.JsonConvert.DeserializeObject<RebarInfoFloor>(floorDeserialize);

                DialogWindowFloor form = new DialogWindowFloor(rif, rebarTypes);
                form.ShowDialog();
                if (form.DialogResult != System.Windows.Forms.DialogResult.OK) return Result.Cancelled;

                floorDeserialize = Newtonsoft.Json.JsonConvert.SerializeObject(rif);
                System.IO.File.WriteAllText(floorPath, floorDeserialize);

                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Армирование плит");

                    foreach (Floor floor in floors)
                    {
                        RebarWorkerFloor.Generate(doc, floor, rif, areaTypeId);
                    }
                    t.Commit();
                }
            }
            return Result.Succeeded;
        }
    }
}
