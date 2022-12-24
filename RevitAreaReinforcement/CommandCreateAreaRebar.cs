#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2020, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2020, all rigths reserved.*/
#endregion
#region Usings
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Structure;
using System.Xml.Serialization;
using System.IO;
using System.Data;
#endregion

namespace RevitAreaReinforcement
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CommandCreateAreaRebar : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Debug.Listeners.Clear();
            Debug.Listeners.Add(new RbsLogger.Logger("WallAreaRebar"));
            Debug.WriteLine("Wall reinforcement start");

            App.ActivateConfigFolder();

            Document doc = commandData.Application.ActiveUIDocument.Document;

            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            List<Wall> walls = new List<Wall>();
            

            foreach (ElementId id in sel.GetElementIds())
            {
                Element elem = doc.GetElement(id);

                if (elem is Wall) walls.Add(elem as Wall);
            }
            if (walls.Count == 0)
            {
                message = "Предварительно выберите стены для армирования";
                return Result.Failed;
            }

            Debug.WriteLine("Selected walls count: " + walls.Count.ToString());
            foreach (Wall w in walls)
            {
                Parameter isStructural = w.get_Parameter(BuiltInParameter.WALL_STRUCTURAL_SIGNIFICANT);
                if (isStructural == null) continue;
                if (isStructural.AsInteger() != 1)
                {
                    elements.Insert(w);
                }
            }
            if(elements.Size > 0)
            {
                message = "Найдены не несущие стены, армирование не может быть выполнено";
                Debug.WriteLine("Non-structural walls were found");
                return Result.Failed;
            }

            ElementId areaTypeId = SupportDocumentGetter.GetDefaultArea(doc).Id;
            RebarCoverType zeroCover = SupportDocumentGetter.GetRebarCoverType(doc, 0);
            List<string> rebarTypes = SupportDocumentGetter.GetRebarTypes(doc);
            List<string> rebarTypes2 = rebarTypes.ToList();
            bool wallsHaveRebarInfo = SupportDocumentGetter.CheckWallsHaveRebarInfo(walls);


            RebarInfoWall riw = new RebarInfoWall(); //RebarInfoWall.GetDefault(doc);
            string wallPath = System.IO.Path.Combine(App.localFolder, "wall.xml");
            Debug.WriteLine("Try to deserialize xml: " + wallPath);
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
                        Debug.WriteLine("Deserialize fault!");
                    }
                }
            }

            if (wallsHaveRebarInfo)
            {
                //TaskDialog.Show("Внимание!", "Армирование будет выполнено по данным, указанным в стенах, без вывода диалогового окна.");
                Debug.WriteLine("DialogWindow for auto-reinforcement");
                DialogWindowWallAuto dialogWallAuto = new DialogWindowWallAuto(riw);
                dialogWallAuto.ShowDialog();
                if (dialogWallAuto.DialogResult != System.Windows.Forms.DialogResult.OK) return Result.Cancelled;
                riw = dialogWallAuto.rebarInfo;
                Debug.WriteLine("RebarInfo created");
            }
            else
            {
                Debug.WriteLine("Dialog window for manual-reinforcement");
                DialogWindowWall form = new DialogWindowWall(riw, rebarTypes, rebarTypes2);
                form.ShowDialog();
                if (form.DialogResult != System.Windows.Forms.DialogResult.OK) return Result.Cancelled;
            }

            Debug.Write("Delete xml file and rewrite: " + wallPath);
            if (File.Exists(wallPath)) File.Delete(wallPath);
            using (FileStream writer = new FileStream(wallPath, FileMode.OpenOrCreate))
            {
                serializer.Serialize(writer, riw);
            }
            Debug.WriteLine("... xml success!");


            using (Transaction t = new Transaction(doc))
            {
                t.Start("Армирование стен");
                Debug.WriteLine("Start transaction");

                foreach (Wall wall in walls)
                {
                    if (wallsHaveRebarInfo)
                    {
                        Debug.WriteLine("Get rebar info from wall");
                        RebarInfoWall infoFromWall = new RebarInfoWall(doc, wall);
                        riw.rebarCover = infoFromWall.rebarCover;
                        riw.verticalFreeLength = infoFromWall.verticalFreeLength;
                        riw.verticalRebarTypeName = infoFromWall.verticalRebarTypeName;
                        riw.verticalRebarInterval = infoFromWall.verticalRebarInterval;
                        riw.horizontalRebarTypeName = infoFromWall.horizontalRebarTypeName;
                        riw.horizontalRebarInterval = infoFromWall.horizontalRebarInterval;
                    }
                    Debug.WriteLine("Start wall reinforcement");
                    RebarWorkerWall.GenerateRebar(doc, wall, riw, zeroCover, areaTypeId);
                }
                t.Commit();
                Debug.WriteLine("Finish transaction");
            }
            return Result.Succeeded;
        }
    }
}
