using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;

namespace RevitAreaReinforcement
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class App : IExternalApplication
    {
        public static string assemblyPath = "";
        public static string assemblyFolder = "";
        public Result OnStartup(UIControlledApplication application)
        {
            assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            assemblyFolder = System.IO.Path.GetDirectoryName(assemblyPath);

            string tabName = "Weandrevit";
            try { application.CreateRibbonTab(tabName); } catch { }
            RibbonPanel panel1 = application.CreateRibbonPanel(tabName, "Стены");
            PushButton btn = panel1.AddItem(new PushButtonData(
                "AreaRebar",
                "Фоновая",
                assemblyPath,
                "WallReinforcement.CommandCreateAreaRebar")
                ) as PushButton;

            PushButton btn2 = panel1.AddItem(new PushButtonData(
                "AreaFix",
                "Ремонт",
                assemblyPath,
                "WallReinforcement.CommandRestoreRebarArea")
                ) as PushButton;


            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

    }
}
