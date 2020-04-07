using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitAreaReinforcement
{
    public class SearchLineResult
    {
        public Line Line;
        public int EndpointNumber;
        public SearchLineResult(Line line, int endpointNumber)
        {
            Line = line;
            EndpointNumber = endpointNumber;
        }
    }
}
