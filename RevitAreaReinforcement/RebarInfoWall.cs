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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System.Xml.Serialization;
#endregion

namespace RevitAreaReinforcement
{
    [Serializable]
    public class RebarInfoWall
    {

        public bool generateVertical = true;
        public string verticalSectionText = "Осн. верт.";
        public bool generateHorizontal = true;
        public string horizontalSectionText = "Осн. гор.";

        public string verticalRebarTypeName = "12 A240";
        public double verticalRebarInterval = 0.65616797900262469;
        public string horizontalRebarTypeName = "12 A240";
        public double horizontalRebarInterval = 0.65616797900262469;
        public bool horizontalAddInterval = true;

        public double verticalFreeLength = 2.1325459317585302;
        public bool autoVerticalFreeLength = false;
        public double horizontalFreeLength = 0;

        public double backOffset = 0;
        public double bottomOffset = 0.16404199475065617;
        public double topOffset = 0.16404199475065617;
        public double verticalOffset = 0;

        public double rebarCover = 0.082020997375328086;

        public bool useUnification = false;
        public List<double> lengthsUnification;


        /// <summary>
        /// Создание настроек армирования по-умолчанию
        /// </summary>
        public RebarInfoWall()
        {
        }

        /// <summary>
        /// Создание настроек армирования на основе информации из стены
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="wall"></param>
        public RebarInfoWall(Document doc, Wall wall)
        {
            Debug.WriteLine("Start get info from wall id" + wall.Id.IntegerValue.ToString());
            double verticalDiameter = GetParameter("Арм.ВертДиаметр", wall).AsDouble();
            double verticalClass = GetParameter("Арм.ВертКласс", wall).AsDouble();
            if (verticalDiameter == 0 || verticalClass == 0)
            {
                generateVertical = false;
                verticalRebarTypeName = null;
                Debug.WriteLine("Vertical rebar will not be created");
            }
            else
            {
                Debug.WriteLine("Vertical rebar will be created");
                generateVertical = true;
                MyRebarType newrebtype = new MyRebarType(doc, verticalDiameter, verticalClass, false);
                verticalRebarTypeName = newrebtype.bartype.Name;
                Debug.WriteLine("Vertical rebar typename: " + verticalRebarTypeName);
            }

            double horizontalDiameter = GetParameter("Арм.ГоризДиаметр", wall).AsDouble();
            double horizontalClass = GetParameter("Арм.ГоризКласс", wall).AsDouble();
            if (horizontalDiameter == 0 || horizontalClass == 0)
            {
                generateHorizontal = false;
                horizontalRebarTypeName = null;
                Debug.WriteLine("Horizontal rebar will not be created");
            }
            else
            {
                Debug.WriteLine("Horizontal rebar will be created");
                generateHorizontal = true;
                horizontalRebarTypeName = new MyRebarType(doc, horizontalDiameter, horizontalClass, true).bartype.Name;
                Debug.WriteLine("Horizontal rebar typename: " + horizontalRebarTypeName);
            }


            verticalRebarInterval = GetParameter("Арм.ВертШаг", wall).AsDouble();
            horizontalRebarInterval = GetParameter("Арм.ГоризШаг", wall).AsDouble();
            Debug.WriteLine("Rebar interval: " + verticalRebarInterval.ToString("F3") + "x" + horizontalRebarInterval.ToString("F3"));

            try
            {
                verticalFreeLength = GetParameter("Арм.ДлинаВыпуска", wall).AsDouble();
                Debug.WriteLine("Vertical free length from parameter. L=" + verticalFreeLength.ToString("F3"));
            }
            catch
            {
                verticalFreeLength = 0;
                Debug.WriteLine("Vertical free length = 0");
            }

            rebarCover = GetParameter("Арм.ЗащитныйСлой", wall).AsDouble();
            Debug.WriteLine("Rebar corver =" + rebarCover.ToString("F3"));
        }


        public static RebarInfoWall GetDefault(Document doc)
        {
            Debug.WriteLine("Create default rebar info");
            List<RebarBarType> bartypes = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .OfClass(typeof(RebarBarType))
                .Cast<RebarBarType>()
                .ToList();

            Debug.WriteLine("RebarBarTypes found: " + bartypes.Count.ToString());

            RebarBarType bartype = bartypes.First();
            string bartypename = bartype.Name;
            Debug.WriteLine("Selected bartype: " + bartypename);

            RebarInfoWall info = new RebarInfoWall();
            info.horizontalRebarTypeName = bartypename;
            info.verticalRebarTypeName = bartypename;
            info.lengthsUnification = new List<double> { 38.38582677165354, 25.59055118110236, 19.19291338582677, 12.79527559055118, 9.596456692913386, 7.677165354330709 };

            Debug.WriteLine("RebarInfo was created");
            return info;
        }


        private Parameter GetParameter(string paramName, Element elem)
        {
            Parameter param = elem.LookupParameter(paramName);
            if (param == null || !param.HasValue)
            {
                Debug.WriteLine("Unable to get parameter " + paramName + " from element id" + elem.Id.IntegerValue.ToString());
                throw new Exception("В элементе " + elem.Id + " нет параметра " + paramName);
            }
            return param;
        }

        public double getNearestLength(double l)
        {
            double distance = 99999;
            double result = 99999;
            foreach (double u in lengthsUnification)
            {
                double curDist = u - l;
                if (curDist >= 0 && curDist < distance)
                {
                    distance = curDist;
                    result = u;
                }
            }
            return result;
        }
    }
}
