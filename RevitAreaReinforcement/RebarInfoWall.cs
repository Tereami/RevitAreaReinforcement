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
        public bool generateVertical;
        public string verticalSectionText;
        public bool generateHorizontal;
        public string horizontalSectionText;

        public string verticalRebarTypeName;
        public double verticalRebarInterval;
        public string horizontalRebarTypeName;
        public double horizontalRebarInterval;
        public bool horizontalAddInterval;

        public double verticalFreeLength;
        public double horizontalFreeLength;

        public double backOffset;
        public double bottomOffset;
        public double topOffset;
        public double verticalOffset;

        public double rebarCover;


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
            double verticalDiameter = GetParameter("Арм.ВертДиаметр", wall).AsDouble();
            double verticalClass = GetParameter("Арм.ВертКласс", wall).AsDouble();
            if (verticalDiameter == 0 || verticalClass == 0)
            {
                generateVertical = false;
                verticalRebarTypeName = null;
            }
            else
            {
                generateVertical = true;
                verticalRebarTypeName = new MyRebarType(doc, verticalDiameter, verticalClass, false).bartype.Name;
            }

            double horizontalDiameter = GetParameter("Арм.ГоризДиаметр", wall).AsDouble();
            double horizontalClass = GetParameter("Арм.ГоризКласс", wall).AsDouble();
            if (horizontalDiameter == 0 || horizontalClass == 0)
            {
                generateHorizontal = false;
                horizontalRebarTypeName = null;
            }
            else
            {
                generateHorizontal = true;
                horizontalRebarTypeName = new MyRebarType(doc, horizontalDiameter, horizontalClass, true).bartype.Name;
            }


            verticalRebarInterval = GetParameter("Арм.ВертШаг", wall).AsDouble();
            horizontalRebarInterval = GetParameter("Арм.ГоризШаг", wall).AsDouble();

            verticalFreeLength = GetParameter("Арм.ДлинаВыпуска", wall).AsDouble();

            rebarCover = GetParameter("Арм.ЗащитныйСлой", wall).AsDouble();
        }


        public static RebarInfoWall GetDefault(Document doc)
        {
            RebarBarType bartype = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .OfClass(typeof(RebarBarType))
                .Cast<RebarBarType>()
                .First();
            string bartypename = bartype.Name;

            RebarInfoWall info = new RebarInfoWall
            {
                generateVertical = true,
                verticalSectionText = "Осн. верт.",
                generateHorizontal = true,
                horizontalSectionText = "Осн. гор.",
                verticalRebarTypeName = bartypename,
                verticalRebarInterval = 0.65616797900262469,
                horizontalRebarTypeName = bartypename,
                horizontalRebarInterval = 0.65616797900262469,
                horizontalAddInterval = false,
                verticalFreeLength = 2.1325459317585302,
                horizontalFreeLength = 0.98425196850393692,
                backOffset = 0.16404199475065617,
                bottomOffset = 0.16404199475065617,
                topOffset = 0.16404199475065617,
                verticalOffset = 0,
                rebarCover = 0.082020997375328086
            };



            return info;
        }


        private Parameter GetParameter(string paramName, Element elem)
        {
            Parameter param = elem.LookupParameter(paramName);
            if (param == null || !param.HasValue) throw new Exception("В элементе " + elem.Id + " нет параметра " + paramName);
            return param;

        }

        private bool GetBool(string paramName, Element elem)
        {
            Parameter param = GetParameter(paramName, elem);
            int val = param.AsInteger();
            if (val == 0) return false;
            return true;
        }
    }
}
