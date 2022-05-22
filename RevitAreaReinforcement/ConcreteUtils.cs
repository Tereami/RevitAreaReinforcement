using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace RevitAreaReinforcement
{
    public static class ConcreteUtils
    {
        public static double getRebarFreeLength(RebarBarType barType, Element elem, double round)
        {
            int rebarClass = GetRebarClass(barType);
            double Rs = GetRebarRsByClass(rebarClass);
            double n1 = rebarClass > 240 ? 2.5 : 2;
#if R2017 || R2018 || R2019 || R2020 || R2021
            double barDiameter = barType.BarDiameter;
#else
            double barDiameter = barType.BarNominalDiameter;
#endif
            double n2 = (barDiameter * 304.8) > 33 ? 0.9 : 1;

            int concreteClass = GetConcreteClass(elem);
            double Rbt = GetConcreteRbtByClass(concreteClass);


            double alpha = GetCoeffAlpha(false, true);

            double length = (alpha * Rs * barDiameter) / (n1 * n2 * Rbt * 4);

            double lengthRound = round * Math.Round(length / round);
            return lengthRound;
        }

        public static int GetRebarClass(RebarBarType barType)
        {
            Parameter rebarClassParam = barType.LookupParameter("Арм.КлассЧисло");
            if (rebarClassParam == null || !rebarClassParam.HasValue)
            {
                throw new Exception("Нет параметра Арм.КлассЧисло в элементе " + barType.Id.IntegerValue.ToString());
            }

            int rebarClass = (int)rebarClassParam.AsDouble();
            return rebarClass;
        }

        public static int GetConcreteClass(Element elem)
        {
            Document doc = elem.Document;
            Element elemType = doc.GetElement(elem.GetTypeId());
            Parameter materialParam = elemType.get_Parameter(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM);
            if (materialParam == null || !materialParam.HasValue)
            {
                throw new Exception("No Structural Material in element " + elem.Id.IntegerValue.ToString());
            }
            Material mat = doc.GetElement(materialParam.AsElementId()) as Material;
            if (mat == null) throw new Exception("Unable to get Material from element " + elem.Id.IntegerValue.ToString());
            Parameter materialCodeParam = mat.LookupParameter("Мтрл.КодМатериала");
            if (materialCodeParam == null || !materialCodeParam.HasValue)
            {
                throw new Exception("Нет параметра Мтрл.КодМатериала в материале " + mat.Name);
            }
            int materialCode = materialCodeParam.AsInteger() / 100000;
            int concreteClass = materialCode % 100;

            return concreteClass;
        }
        public static double GetConcreteRbtByClass(int clas)
        {
            switch (clas)
            {
                case 10: return 0.5;
                case 15: return 0.75;
                case 20: return 0.9;
                case 25: return 1.05;
                case 30: return 1.15;
                case 35: return 1.3;
                case 40: return 1.4;
                case 45: return 1.5;
                case 50: return 1.6;
                case 55: return 1.7;
                case 60: return 1.8;
                default:
                    break;
            }
            throw new Exception("UNKNOWN CONCRETE CLASS " + clas.ToString());
        }

        public static double GetRebarRsByClass(int clas)
        {
            switch (clas)
            {
                case 240: return 215;
                case 300: return 270;
                case 400: return 355;
                case 500: return 435;
                default:
                    break;
            }
            throw new Exception("UNKNOWN REBAR CLASS " + clas.ToString());
        }

        public static double GetCoeffAlpha(bool anchorOrOverlap, bool downOrUp)
        {
            if (anchorOrOverlap)
            {
                if (downOrUp)
                {
                    return 0.75;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (downOrUp)
                {
                    return 0.9;
                }
                else
                {
                    return 1.2;
                }
            }
        }

        public static double GetCoeffN1(int rebarClass)
        {
            if (rebarClass > 240)
            {
                return 2.5;
            }
            else
            {
                return 1.5;
            }
        }
    }
}
