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
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
#endregion

namespace RevitAreaReinforcement
{
    public class MyRebarType
    {
        public RebarBarType bartype;
        public bool isValid = false;

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
                isValid = true;
            }
        }

        public MyRebarType(Document Doc, double BarDiameter, double BarClass, bool AsCommonLength)
        {
            Debug.WriteLine("Try to create MyRebarType, d=" + BarDiameter + " class=" + BarClass);
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
                isValid = true;
                Debug.WriteLine("Type found: " + bartype.Name);
                break;
            }
            if (!isValid)
            {
                string errmsg = "Не удалось получить тип стержня d" + (BarDiameter * 304.8).ToString("F0") + " класс " + BarClass.ToString("F0");
                Debug.WriteLine(errmsg);
                throw new Exception(errmsg);
            }
        }
    }
}
