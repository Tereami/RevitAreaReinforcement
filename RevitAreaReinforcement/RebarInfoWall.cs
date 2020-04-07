using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace RevitAreaReinforcement
{
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
