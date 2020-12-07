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
using System.Diagnostics;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using CylindricalFace = Autodesk.Revit.DB.CylindricalFace;
using Edge = Autodesk.Revit.DB.Edge;
using PlanarFace = Autodesk.Revit.DB.PlanarFace;
using Transform = Autodesk.Revit.DB.Transform;
using XYZ = Autodesk.Revit.DB.XYZ;
#endregion

namespace RevitAreaReinforcement
{
    class Util
    {
        #region Geometrical Comparison
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
        #endregion // Geometrical Comparison

        #region Formatting
        /// <summary>
        /// Return an English plural suffix 's' or 
        /// nothing for the given number of items.
        /// </summary>
        public static string PluralSuffix(int n)
        {
            return 1 == n ? "" : "s";
        }

        public static string DotOrColon(int n)
        {
            return 1 < n ? ":" : ".";
        }

        static public string RealString(double a)
        {
            return a.ToString("0.##");
        }

        static public string AngleString(double a)
        {
            return RealString(a * 180 / Math.PI) + " degrees";
        }

        static public string PointString(XYZ p)
        {
            return string.Format("({0},{1},{2})",
              RealString(p.X), RealString(p.Y),
              RealString(p.Z));
        }

        static public string TransformString(Transform t)
        {
            return string.Format("({0},{1},{2},{3})", PointString(t.Origin),
              PointString(t.BasisX), PointString(t.BasisY), PointString(t.BasisZ));
        }
        #endregion // Formatting

        static public void InfoMsg(string msg)
        {
            Debug.WriteLine(msg);
            System.Windows.Forms.MessageBox.Show(msg,
              "AreaReinforcement",
              System.Windows.Forms.MessageBoxButtons.OK,
              System.Windows.Forms.MessageBoxIcon.Information);
        }

        public static string ElementDescription(Element e)
        {
            // for a wall, the element name equals the 
            // wall type name, which is equivalent to the 
            // family name ...
            FamilyInstance fi = e as FamilyInstance;
            string fn = (null == fi)
              ? string.Empty
              : fi.Symbol.Family.Name + " ";

            string cn = (null == e.Category)
              ? e.GetType().Name
              : e.Category.Name;

            return string.Format("{0} {1}<{2} {3}>",
              cn, fn, e.Id.IntegerValue, e.Name);
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
            double length = c.Length;
            string msg = Environment.NewLine + "Curve length: " + (length * 304.8).ToString("F1") ;
            msg += Environment.NewLine + GetPointDegunInfo(c.GetEndPoint(0));
            msg += Environment.NewLine + GetPointDegunInfo(c.GetEndPoint(1));
            msg += Environment.NewLine;
            return msg;
        }

        public static string GetPointDegunInfo(XYZ point)
        {
            string msg = "X: " + (point.X * 304.8).ToString();
            msg += "\t Y: " + (point.Y * 304.8).ToString();
            msg += "\t Z: " + (point.Z * 304.8).ToString();
            return msg;
        }
    }
}
