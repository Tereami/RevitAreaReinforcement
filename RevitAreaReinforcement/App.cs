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
using Autodesk.Revit.UI;

namespace RevitAreaReinforcement
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class App : IExternalApplication
    {
        public static string assemblyPath = "";
        public static string assemblyFolder = "";
        public static string localFolder = "";
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
                "RevitAreaReinforcement.CommandCreateAreaRebar")
                ) as PushButton;

            PushButton btn2 = panel1.AddItem(new PushButtonData(
                "AreaFix",
                "Ремонт",
                assemblyPath,
                "RevitAreaReinforcement.CommandRestoreRebarArea")
                ) as PushButton;


            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public static void ActivateConfigFolder()
        {
            if (string.IsNullOrEmpty(App.assemblyFolder))
            {
                App.assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                App.assemblyFolder = System.IO.Path.GetDirectoryName(App.assemblyPath);
            }

            string programdataPath = 
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
            string rbspath = System.IO.Path.Combine(programdataPath, "RibbonBimStarter");
            if (!System.IO.Directory.Exists(rbspath))
                System.IO.Directory.CreateDirectory(rbspath);
            string solutionName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            localFolder =
                System.IO.Path.Combine(rbspath, solutionName);
            if (!System.IO.Directory.Exists(localFolder))
                System.IO.Directory.CreateDirectory(localFolder);

        }

    }
}
