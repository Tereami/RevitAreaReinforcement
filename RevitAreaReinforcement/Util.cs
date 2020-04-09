#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-NonCommercial-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в некоммерческих целях,
при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2020, все права защищены.
This code is listed under the Creative Commons Attribution-NonCommercial-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially,
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

        const string _caption = "The Building Coder";

        static public void InfoMsg(string msg)
        {
            Debug.WriteLine(msg);
            System.Windows.Forms.MessageBox.Show(msg,
              _caption,
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

        //public static Element SelectSingleElement(UIDocument uidoc, string description)
        //{
        //    Selection sel = uidoc.Selection;
        //    Element e = null;
        //    sel.SetElementIds(null);
        //    if (sel.PickObject(ObjectType.Element, "Выберите один элемент")
        //    {
        //        ElementSetIterator elemSetItr
        //          = sel.Elements.ForwardIterator();
        //        elemSetItr.MoveNext();
        //        e = elemSetItr.Current as Element;
        //    }
        //    return e;
        //}

        //public static bool GetSelectedElementsOrAll(
        //  List<Element> a,
        //  Document doc,
        //  Type t)
        //{
        //    Selection sel = doc.Selection;
        //    if (0 < sel.Elements.Size)
        //    {
        //        foreach (Element e in sel.Elements)
        //        {
        //            if (e.GetType().IsSubclassOf(t))
        //            {
        //                a.Add(e);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        doc.get_Elements(t, a);
        //    }
        //    return 0 < a.Count;
        //}
    }
}
