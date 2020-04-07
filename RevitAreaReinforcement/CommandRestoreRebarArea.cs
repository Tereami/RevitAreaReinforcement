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
    class CommandRestoreRebarArea : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            List<ElementId> selids = sel.GetElementIds().ToList();

            List<AreaReinforcement> ars = new List<AreaReinforcement>();
            foreach(ElementId id in selids)
            {
                AreaReinforcement ar = doc.GetElement(id) as AreaReinforcement;
                if (ar != null) ars.Add(ar);
            }

            if (ars.Count == 0)
            {
                message += "Выберите арматуру по площади";
                return Result.Failed;
            }

            bool checkFile = System.IO.File.Exists(@"C:\revitarea\ids.txt");
            if(checkFile)
            {
                System.IO.File.Delete(@"C:\revitarea\ids.txt");
                System.IO.Directory.Delete(@"C:\revitarea");
            }

            System.IO.Directory.CreateDirectory(@"C:\revitarea");
            System.IO.StreamWriter sw = System.IO.File.CreateText(@"C:\revitarea\ids.txt");

            foreach (AreaReinforcement ar in ars)
            {
                string line = ar.Id.IntegerValue.ToString() + ":";
                List<ElementId> curveIds = ar.GetBoundaryCurveIds().ToList();
                foreach (ElementId id in curveIds)
                {
                    line = line + id.IntegerValue.ToString() + ",";
                }
                sw.WriteLine(line);
            }
            sw.Close();

            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string assemblyFolder = System.IO.Path.GetDirectoryName(assemblyPath);

            string scriptPath = System.IO.Path.Combine(assemblyFolder, "RestoreAreaRebar.exe");

            //нужно сбросить выделение, если есть выбранные элементы
            commandData.Application.ActiveUIDocument.Selection.SetElementIds(new List<ElementId>());

            //запускаю супер-програмку
            System.Diagnostics.Process.Start(scriptPath);

            

            return Result.Succeeded;
        }
    }
}
