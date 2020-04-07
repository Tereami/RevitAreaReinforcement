using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace RevitAreaReinforcement
{
    public class MyRebarType
    {
        public RebarBarType bartype;

        public MyRebarType(RebarBarType BarType)
        {
            bartype = BarType;
        }

        public override string ToString()
        {
            return bartype.Name;
        }

        public MyRebarType (Document doc, string typeName)
        {
            List<RebarBarType> bartypes = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .OfClass(typeof(RebarBarType))
                .Cast<RebarBarType>()
                .ToList();

            List<RebarBarType> mytypes = bartypes
                .Where(i => i.Name == typeName)
                .ToList();

            if (mytypes.Count == 0)
            {
                bartype = bartypes.First();
            }
            else
            {
                bartype = mytypes.First();
            }
        }

        public MyRebarType(Document Doc, double BarDiameter, double BarClass, bool AsCommonLength)
        {
            List<RebarBarType> bartypes = new FilteredElementCollector(Doc)
                .WhereElementIsElementType()
                .OfClass(typeof(RebarBarType))
                .Cast<RebarBarType>()
                .ToList();

            foreach(RebarBarType rbt in bartypes)
            {
                double diam = rbt.BarDiameter;
                if (Math.Abs(diam - BarDiameter) > 0.00001) continue;

                Parameter classParam = rbt.LookupParameter("Арм.КлассЧисло");
                if (classParam == null || !classParam.HasValue) continue;

                double cls = classParam.AsDouble();
                if (Math.Abs(cls - BarClass) > 0.00001) continue;

                Parameter commonLengthParam = rbt.LookupParameter("Рзм.ПогМетрыВкл");
                if (commonLengthParam == null || !commonLengthParam.HasValue) continue;
                bool commonLengthOn = commonLengthParam.AsInteger() == 1;
                if (commonLengthOn != AsCommonLength) continue;

                bartype = rbt;
                break;
            }

        }
    }
}
