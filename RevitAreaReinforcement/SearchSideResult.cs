using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitAreaReinforcement
{
    public class SearchSideResult
    {
        public List<Line> sideLines;
        public XYZ outVector;
    }
}
