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
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using CylindricalFace = Autodesk.Revit.DB.CylindricalFace;
using Edge = Autodesk.Revit.DB.Edge;
using PlanarFace = Autodesk.Revit.DB.PlanarFace;
using XYZ = Autodesk.Revit.DB.XYZ;
#endregion

namespace RevitAreaReinforcement
{
    public static class Util
    {
        const double _eps = 1.0e-9;

        static public bool IsZero(double a)
        {
            return _eps > Math.Abs(a);
        }

        static public bool IsHorizontal(XYZ v)
        {
            return IsZero(v.Z);
        }

        static public bool IsVertical(XYZ v)
        {
            return IsZero(v.X) && IsZero(v.Y);
        }

        static public bool IsHorizontal(Edge e)
        {
            XYZ p = e.Evaluate(0);
            XYZ q = e.Evaluate(1);
            return IsHorizontal(q - p);
        }

        static public bool IsHorizontal(PlanarFace f)
        {
            return IsVertical(f.FaceNormal);
        }

        static public bool IsVertical(PlanarFace f)
        {
            return IsHorizontal(f.FaceNormal);
        }

        static public bool IsVertical(CylindricalFace f)
        {
            return IsVertical(f.Axis);
        }


        static public string RealString(double a)
        {
            return a.ToString("0.##");
        }


        static public string PointString(XYZ p)
        {
            return string.Format("({0},{1},{2})",
              RealString(p.X), RealString(p.Y),
              RealString(p.Z));
        }


        public static string ProfileDebugInfo(List<Curve> profile)
        {
            string msg = "Curves count: " + profile.Count.ToString();

            for (int i = 0; i < profile.Count; i++)
            {
                Curve c = profile[i];
                msg += CurveDebugInfo(c);
            }

            return msg;
        }

        public static string CurveDebugInfo(Curve c)
        {
            List<string> info = new List<string>()
            {
                $"Curve length: {c.Length.InchesToStringMillimeters()}",
                 GetPointDebugInfo(c.GetEndPoint(0)),
                 GetPointDebugInfo(c.GetEndPoint(1)),
            };
            string msg = string.Join(Environment.NewLine, info);
            return msg;
        }

        public static string GetPointDebugInfo(XYZ point)
        {
            string msg = "X: " + point.X.InchesToStringMillimeters();
            msg += "\t Y: " + point.Y.InchesToStringMillimeters();
            msg += "\t Z: " + point.Z.InchesToStringMillimeters();
            return msg;
        }

        public static string InchesToStringMillimeters(this double inches)
        {
            double mm = inches.InchesToMillimeters();
            string text = mm.ToString("0.#");
            return text;
        }

        public static double ParseToInches(this string millimeters)
        {
            if (double.TryParse(millimeters, out double result))
                return result / 304.8;
            else
                return 0;
        }

        public static double InchesToMillimeters(this double inches)
        {
            double mm = Math.Round(inches * 304.8, 3);
            return mm;
        }

        public static double MillimetersToInches(this double millimeters)
        {
            double inches = millimeters / 304.8;
            return inches;
        }
    }
}
