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
using System.Diagnostics;
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
            Debug.Listeners.Clear();
            Debug.Listeners.Add(new RbsLogger.Logger("FloorAreaRebar"));
            Debug.WriteLine("Floor reinforcement start");
            App.ActivateConfigFolder();
            Document doc = commandData.Application.ActiveUIDocument.Document;
            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            List<Floor> floors = new List<Floor>();
            foreach (ElementId id in sel.GetElementIds())
            {
                Element elem = doc.GetElement(id);
                if (elem is Floor) floors.Add(elem as Floor);
            }
            Debug.WriteLine("Selected floors: " + floors.Count);
            if (floors.Count == 0)
            {
                message = MyStrings.MessageSelectFloors;
                return Result.Failed;
            }

            ElementId areaTypeId = SupportDocumentGetter.GetDefaultArea(doc).Id;
            Debug.WriteLine($"AreaTypeId: {areaTypeId}");
            RebarCoverType zeroCover = SupportDocumentGetter.GetRebarCoverType(doc, 0);
            Debug.WriteLine($"Zero cover type id: {zeroCover.Id}");

            List<string> rebarTypes = SupportDocumentGetter.GetRebarTypes(doc);
            Debug.WriteLine("Rebar types: " + rebarTypes.Count);

            if (floors.Count == 0)
            {
                message = MyStrings.MessageNoCorrectFloors;
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
            Debug.WriteLine("Structural floors: " + (floors.Count - elements.Size));
            if(elements.Size > 0)
            {
                message = MyStrings.MessageNoStructuralFloors;
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
                    catch(Exception ex)
                    {
                        Debug.WriteLine("Failed deserialize, create new one: " + ex.Message);
                        rif = RebarInfoFloor.GetDefault(doc);
                    }
                    if (rif == null)
                    {
                        Debug.WriteLine("Deserialize error: " + floorPath);
                        throw new Exception("Serialize failed: " + floorPath);
                    }
                }
            }
            else
            {
                Debug.WriteLine("No xml file, create new one");
                rif = RebarInfoFloor.GetDefault(doc);
            }

            DialogWindowFloor form = new DialogWindowFloor(rif, rebarTypes);
            form.ShowDialog();
            if (form.DialogResult != System.Windows.Forms.DialogResult.OK)
            {
                Debug.WriteLine("Cancelled by user");
                return Result.Cancelled;
            }

            if (File.Exists(floorPath))
            {
                Debug.WriteLine("File is deleted: " + floorPath);
                File.Delete(floorPath);
            }
            using (FileStream writer = new FileStream(floorPath, FileMode.OpenOrCreate))
            {
                serializer.Serialize(writer, form.rif);
                Debug.WriteLine("New file is created: " + floorPath);
            }

            List<string> rebarMessages = new List<string>();

            using (Transaction t = new Transaction(doc))
            {
                t.Start(MyStrings.TransactionFloorReinforcement);

                foreach (Floor floor in floors)
                {
                    Debug.WriteLine($"Current reinforcement floor: {floor.Id}");
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
                Debug.WriteLine("Errors: " + message);
                return Result.Failed;
            }
            Debug.WriteLine("All done");
            return Result.Succeeded;
        }
    }
}
