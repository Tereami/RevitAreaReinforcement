﻿#region License
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
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
#endregion

namespace RevitAreaReinforcement
{
    public static class SupportDocumentGetter
    {
        public static RebarCoverType GetRebarCoverType(Document doc, double coverDistance)
        {
            List<RebarCoverType> types = new FilteredElementCollector(doc)
                .OfClass(typeof(RebarCoverType))
                .Cast<RebarCoverType>()
                .Where(i => i.CoverDistance == coverDistance)
                .ToList();

            Trace.WriteLine("RebarCoverTypes found: " + types.Count.ToString());

            if (types.Count == 0)
            {
                string msg = MyStrings.ErrorNoRebarCover + (coverDistance * 304.8).ToString("F2");
                Trace.WriteLine(msg);
                TaskDialog.Show("ERROR", msg);
                throw new Exception(msg);
            }

            return types.First();
        }

        public static AreaReinforcementType GetDefaultArea(Document doc)
        {
            List<AreaReinforcementType> areaTypes = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .OfClass(typeof(AreaReinforcementType))
                .Cast<AreaReinforcementType>()
                .ToList();

            Trace.WriteLine("Area reinforcement types found: " + areaTypes.Count.ToString());
            if (areaTypes.Count == 0)
            {
                string msg = MyStrings.ErrorNoAreaType;
                Trace.WriteLine(msg);
                TaskDialog.Show("ERROR", msg);
                throw new Exception(msg);
            }

            AreaReinforcementType ar = areaTypes.First();

            return ar;
        }

        public static bool CheckWallsHaveRebarInfo(List<Wall> walls)
        {
            foreach(Wall wall in walls)
            {
                Parameter param = wall.get_Parameter(RebarInfoWall.horizRebarDiameterParamGuid);
                if (param == null || !param.HasValue) return false;
                double diam = param.AsDouble();
                if (diam < 0.001) return false; //если указан 0 то тоже пропускаем
            }
            Trace.WriteLine("Walls have info for auto-reinforcement");
            return true;
        }

        public static List<string> GetRebarTypes(Document doc)
        {
            List<string> rebarTypes = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .OfClass(typeof(RebarBarType))
                .Cast<RebarBarType>()
                .Select(i => i.Name)
                .OrderBy(i => i)
                .ToList();
            Trace.WriteLine("RebarBarTypes found: " + rebarTypes.Count.ToString());
            return rebarTypes;
        }
    }
}
