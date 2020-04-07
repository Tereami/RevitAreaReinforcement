using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

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

            if (types.Count == 0) return null;

            return types.First();
        }

        public static AreaReinforcementType GetDefaultArea(Document doc)
        {
            AreaReinforcementType ar = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .OfClass(typeof(AreaReinforcementType))
                .Cast<AreaReinforcementType>()
                .First();
            return ar;
        }

        public static bool CheckWallsHaveRebarInfo(List<Wall> walls)
        {
            foreach(Wall wall in walls)
            {
                Parameter param = wall.LookupParameter("Арм.ГоризДиаметр");
                if (param == null || !param.HasValue) return false;
            }
            return true;
        }
    }
}
