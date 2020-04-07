using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;

namespace RevitAreaReinforcement
{
    public static class RebarWorkerFloor
    {
        public static void Generate(Document doc, Floor floor, RebarInfoFloor rif, ElementId areaTypeId)
        {
            MyRebarType mrt = new MyRebarType(doc, rif.rebarTypeName);
            double interval = rif.interval;
            double topCoverUser = rif.topCover;
            double bottomCoverUser = rif.bottomCover;

            RebarCoverType coverTop = doc.GetElement(floor.get_Parameter(BuiltInParameter.CLEAR_COVER_TOP).AsElementId()) as RebarCoverType;
            RebarCoverType coverBottom = doc.GetElement(floor.get_Parameter(BuiltInParameter.CLEAR_COVER_BOTTOM).AsElementId()) as RebarCoverType;

            if (coverTop == null) coverTop = coverBottom;
            if (coverBottom == null) coverBottom = coverTop;

            double topCover = topCoverUser - coverTop.CoverDistance;
            double bottomCover = bottomCoverUser - coverBottom.CoverDistance;

            List<Curve> curves = SupportGeometry.GetFloorOuterBoundary(floor);

            AreaReinforcement arTopX = AreaReinforcement
                .Create(doc, floor, curves, new XYZ(1, 0, 0), areaTypeId, mrt.bartype.Id, ElementId.InvalidElementId);
            arTopX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_1_GENERIC).Set(1);
            arTopX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_2_GENERIC).Set(0);
            arTopX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_1_GENERIC).Set(0);
            arTopX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_2_GENERIC).Set(0);
            arTopX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_SPACING_TOP_DIR_1_GENERIC).Set(interval);
            arTopX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ADDL_TOP_OFFSET).Set(topCover);
            arTopX.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).Set("верх X фон");

            AreaReinforcement arTopY = AreaReinforcement
                .Create(doc, floor, curves, new XYZ(1, 0, 0), areaTypeId, mrt.bartype.Id, ElementId.InvalidElementId);
            arTopY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_1_GENERIC).Set(0);
            arTopY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_2_GENERIC).Set(1);
            arTopY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_1_GENERIC).Set(0);
            arTopY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_2_GENERIC).Set(0);
            arTopY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_SPACING_TOP_DIR_2_GENERIC).Set(interval);
            arTopY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ADDL_TOP_OFFSET).Set(topCover + mrt.bartype.BarDiameter);
            arTopY.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).Set("верх Y фон");

            AreaReinforcement arBottomX = AreaReinforcement
                .Create(doc, floor, curves, new XYZ(1, 0, 0), areaTypeId, mrt.bartype.Id, ElementId.InvalidElementId);
            arBottomX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_1_GENERIC).Set(0);
            arBottomX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_2_GENERIC).Set(0);
            arBottomX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_1_GENERIC).Set(1);
            arBottomX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_2_GENERIC).Set(0);
            arBottomX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_SPACING_BOTTOM_DIR_1_GENERIC).Set(interval);
            arBottomX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ADDL_BOTTOM_OFFSET).Set(bottomCover + mrt.bartype.BarDiameter);
            arBottomX.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).Set("низ X фон");

            AreaReinforcement arBottomY = AreaReinforcement
                .Create(doc, floor, curves, new XYZ(1, 0, 0), areaTypeId, mrt.bartype.Id, ElementId.InvalidElementId);
            arBottomY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_1_GENERIC).Set(0);
            arBottomY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_2_GENERIC).Set(0);
            arBottomY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_1_GENERIC).Set(0);
            arBottomY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_2_GENERIC).Set(1);
            arBottomY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_SPACING_BOTTOM_DIR_2_GENERIC).Set(interval);
            arBottomY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ADDL_BOTTOM_OFFSET).Set(bottomCover);
            arBottomY.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).Set("низ Y фон");
        }
    }
}
