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

            foreach(Wall w in walls)
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
                return Result.Failed;
            }

            ElementId areaTypeId = SupportDocumentGetter.GetDefaultArea(doc).Id;
            RebarCoverType zeroCover = SupportDocumentGetter.GetRebarCoverType(doc, 0);
            List<string> rebarTypes = SupportDocumentGetter.GetRebarTypes(doc);
            List<string> rebarTypes2 = rebarTypes.ToList();
            bool wallsHaveRebarInfo = SupportDocumentGetter.CheckWallsHaveRebarInfo(walls);


            RebarInfoWall riw = RebarInfoWall.GetDefault(doc);
            string wallPath = System.IO.Path.Combine(App.localFolder, "wall.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(RebarInfoWall));

            if (System.IO.File.Exists(wallPath))
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(wallPath))
                {
                    try
                    {
                        riw = (RebarInfoWall)serializer.Deserialize(reader);
                    }
                    catch { }
                }
            }

            if (wallsHaveRebarInfo)
            {
                //TaskDialog.Show("Внимание!", "Армирование будет выполнено по данным, указанным в стенах, без вывода диалогового окна.");
                DialogWindowWallAuto dialogWallAuto = new DialogWindowWallAuto(riw);
                dialogWallAuto.ShowDialog();
                if (dialogWallAuto.DialogResult != System.Windows.Forms.DialogResult.OK) return Result.Cancelled;
                riw = dialogWallAuto.rebarInfo;
            }
            else
            {
                DialogWindowWall form = new DialogWindowWall(riw, rebarTypes, rebarTypes2);
                form.ShowDialog();
                if (form.DialogResult != System.Windows.Forms.DialogResult.OK) return Result.Cancelled;
            }

            if (File.Exists(wallPath)) File.Delete(wallPath);
            using (FileStream writer = new FileStream(wallPath, FileMode.OpenOrCreate))
            {
                serializer.Serialize(writer, riw);
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
                        newRiw.verticalOffset = riw.verticalOffset;

                        newRiw.useUnification = riw.useUnification;
                        newRiw.lengthsUnification = riw.lengthsUnification;
                        newRiw.autoVerticalFreeLength = riw.autoVerticalFreeLength;

                        RebarWorkerWall.GenerateRebar(doc, wall, newRiw, zeroCover, areaTypeId);
                    }
                    else
                    {
                        RebarWorkerWall.GenerateRebar(doc, wall, riw, zeroCover, areaTypeId);
                    }
                }
                t.Commit();
            }
            return Result.Succeeded;
        }
    }
}
