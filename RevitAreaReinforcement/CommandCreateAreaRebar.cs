#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-NonCommercial-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в некоммерческих целях,
при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2020, все права защищены.
This code is listed under the Creative Commons Attribution-NonCommercial-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2020, all rigths reserved.*/
#endregion
#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Structure;
using System.Xml.Serialization;
using System.IO;
#endregion

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
            string rbspath = Path.Combine(programdataPath, "RibbonBimStarter");
            if (!Directory.Exists(rbspath)) Directory.CreateDirectory(rbspath);
            string solutionName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string localFolder = Path.Combine(rbspath, solutionName);
            if (!Directory.Exists(localFolder)) Directory.CreateDirectory(localFolder);

            

            




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
                RebarInfoWall riw = null;

                if (wallsHaveRebarInfo)
                {
                    TaskDialog.Show("Внимание!", "Армирование будет выполнено по данным, указанным в стенах, без вывода диалогового окна.");
                }
                else
                {
                    string wallPath = System.IO.Path.Combine(localFolder, "wall.xml");
                    XmlSerializer serializer = new XmlSerializer(typeof(RebarInfoWall));

                    if (System.IO.File.Exists(wallPath))
                    {
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(wallPath))
                        {
                            try
                            {
                                riw = (RebarInfoWall)serializer.Deserialize(reader);
                            }
                            catch
                            {
                                riw = RebarInfoWall.GetDefault(doc);
                            }
                            
                            if (riw == null)
                            {
                                throw new Exception("Не удалось сериализовать: " + wallPath);
                            }
                        }
                    }
                    else
                    {
                        riw = RebarInfoWall.GetDefault(doc);
                    }

                    DialogWindowWall form = new DialogWindowWall(riw, rebarTypes, rebarTypes2);
                    form.ShowDialog();
                    if (form.DialogResult != System.Windows.Forms.DialogResult.OK) return Result.Cancelled;

                    if (File.Exists(wallPath)) File.Delete(wallPath);
                    using (FileStream writer = new FileStream(wallPath, FileMode.OpenOrCreate))
                    {
                        serializer.Serialize(writer, form.wri);
                    }
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
                string floorPath = System.IO.Path.Combine(localFolder, "floor.xml");
                RebarInfoFloor rif = null;
                XmlSerializer serializer = new XmlSerializer(typeof(RebarInfoFloor));

                if (System.IO.File.Exists(floorPath))
                {
                    using(System.IO.StreamReader reader = new System.IO.StreamReader(floorPath))
                        {
                        try
                        {
                            rif = (RebarInfoFloor)serializer.Deserialize(reader);
                        }
                        catch
                        {
                            rif = RebarInfoFloor.GetDefault(doc);
                        }
                        if (rif == null)
                        {
                            throw new Exception("Не удалось сериализовать: " + floorPath);
                        }
                    }
                }
                else
                {
                    rif = RebarInfoFloor.GetDefault(doc);
                }

                


                DialogWindowFloor form = new DialogWindowFloor(rif, rebarTypes);
                form.ShowDialog();
                if (form.DialogResult != System.Windows.Forms.DialogResult.OK) return Result.Cancelled;

                if (File.Exists(floorPath)) File.Delete(floorPath);
                using (FileStream writer = new FileStream(floorPath, FileMode.OpenOrCreate))
                {
                    serializer.Serialize(writer, form.rif);
                }

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
