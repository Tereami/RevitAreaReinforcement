using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System.Xml.Serialization;

namespace RevitAreaReinforcement
{
    [Serializable]
    public class RebarInfoFloor
    {
        public string rebarTypeName;

        public double interval;
        public double topCover;
        public double bottomCover;


        public RebarInfoFloor()
        {

        }


        public static RebarInfoFloor GetDefault(Document doc)
        {
            RebarBarType bartype = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .OfClass(typeof(RebarBarType))
                .Cast<RebarBarType>()
                .First();
            string bartypename = bartype.Name;

            RebarInfoFloor info = new RebarInfoFloor
            {
                rebarTypeName = bartypename,
                interval = 0.65616797900262469,
                topCover = 0.098425196850393692,
                bottomCover = 0.13123359580052493
            };


            return info;
        }
    }
}
