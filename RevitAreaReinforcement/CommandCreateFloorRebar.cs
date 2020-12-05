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
#endregion

namespace RevitAreaReinforcement
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CommandCreateFloorRebar : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            App.ActivateConfigFolder();
            Document doc = commandData.Application.ActiveUIDocument.Document;
            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            List<Floor> floors = new List<Floor>();
            foreach (ElementId id in sel.GetElementIds())
            {
                Element elem = doc.GetElement(id);
                if (elem is Floor) floors.Add(elem as Floor);
            }
            if (floors.Count == 0)
            {
                message = "Выберите плиты для армирования";
                return Result.Failed;
            }

            ElementId areaTypeId = SupportDocumentGetter.GetDefaultArea(doc).Id;
            RebarCoverType zeroCover = SupportDocumentGetter.GetRebarCoverType(doc, 0);

            List<string> rebarTypes = SupportDocumentGetter.GetRebarTypes(doc);

            if (floors.Count == 0)
            {
                message = "Нет подходящих перекрытий для армирования";
                return Result.Failed;
            }

            foreach(Floor fl in floors)
            {
                Parameter floorIsStructuralParam = fl.get_Parameter(BuiltInParameter.FLOOR_PARAM_IS_STRUCTURAL);
                if(floorIsStructuralParam != null)
                {
                    if(floorIsStructuralParam.AsInteger() != 1)
                    {
                        elements.Insert(fl);
                    }
                }
            }
            if(elements.Size > 0)
            {
                message = "Найдены не несущие плиты, армирование не будет выполнено";
                return Result.Failed;
            }

            string floorPath = System.IO.Path.Combine(App.localFolder, "floor.xml");
            RebarInfoFloor rif = null;
            XmlSerializer serializer = new XmlSerializer(typeof(RebarInfoFloor));

            if (System.IO.File.Exists(floorPath))
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(floorPath))
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

            List<string> rebarMessages = new List<string>();

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Армирование плит");

                foreach (Floor floor in floors)
                {
                    List<string> curRebarMessages = RebarWorkerFloor.Generate(doc, floor, rif, areaTypeId);
                    rebarMessages.AddRange(curRebarMessages);
                }
                t.Commit();
            }

            if(rebarMessages.Count > 0)
            {
                foreach(string msg in rebarMessages)
                {
                    message += msg + System.Environment.NewLine;
                }
                return Result.Failed;
            }


            return Result.Succeeded;
        }
    }
}
