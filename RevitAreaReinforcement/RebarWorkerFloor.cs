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
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
#endregion

namespace RevitAreaReinforcement
{
    public static class RebarWorkerFloor
    {
        
        public static List<string> Generate(Document doc, Floor floor, RebarInfoFloor rif, ElementId areaTypeId)
        {
            List<string> messages = new List<string>();
            MyRebarType mrt = new MyRebarType(doc, rif.rebarTypeName);
            if(mrt.isValid == false)
            {
                messages.Add("Не удалось получить тип стержня " + rif.rebarTypeName);
            }
            double interval = rif.interval;
            double topCoverUser = rif.topCover;
            double bottomCoverUser = rif.bottomCover;

            RebarCoverType coverTop = doc.GetElement(floor.get_Parameter(BuiltInParameter.CLEAR_COVER_TOP).AsElementId()) as RebarCoverType;
            RebarCoverType coverBottom = doc.GetElement(floor.get_Parameter(BuiltInParameter.CLEAR_COVER_BOTTOM).AsElementId()) as RebarCoverType;

            if (coverTop == null) coverTop = coverBottom;
            if (coverBottom == null) coverBottom = coverTop;

            double topCoverDir1 = topCoverUser - coverTop.CoverDistance;
            double topCoverDir2 = topCoverDir1 + mrt.bartype.BarDiameter;
            if(rif.turnTopBars)
            {
                topCoverDir1 += mrt.bartype.BarDiameter;
                topCoverDir2 -= mrt.bartype.BarDiameter;
            }

            double bottomCoverDir1 = bottomCoverUser - coverBottom.CoverDistance;
            double bottomCoverDir2 = bottomCoverDir1 + mrt.bartype.BarDiameter;
            if(rif.turnBottomBars)
            {
                bottomCoverDir1 += mrt.bartype.BarDiameter;
                bottomCoverDir2 -= mrt.bartype.BarDiameter;
            }


            List<Curve> curves = SupportGeometry.GetFloorOuterBoundary(floor);

            XYZ direction = new XYZ(1, 0, 0);

            if (rif.useDirection)
            {
                double angle = floor.SpanDirectionAngle;
                Transform rotateTransform = Transform.CreateRotationAtPoint(new XYZ(0, 0, 1), angle, new XYZ(0, 0, 0));
                Line horizontal = Line.CreateBound(new XYZ(0, 0, 0), new XYZ(1, 0, 0));
                Curve rotatedCurve = horizontal.CreateTransformed(rotateTransform);
                direction = rotatedCurve.GetEndPoint(1);
            }


            AreaReinforcement arTopX = AreaReinforcement
                .Create(doc, floor, curves, direction, areaTypeId, mrt.bartype.Id, ElementId.InvalidElementId);
            arTopX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_1_GENERIC).Set(1);
            arTopX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_2_GENERIC).Set(0);
            arTopX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_1_GENERIC).Set(0);
            arTopX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_2_GENERIC).Set(0);
            arTopX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_SPACING_TOP_DIR_1_GENERIC).Set(interval);
            arTopX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ADDL_TOP_OFFSET).Set(topCoverDir1);
            arTopX.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).Set("верх X фон");

            AreaReinforcement arTopY = AreaReinforcement
                .Create(doc, floor, curves, direction, areaTypeId, mrt.bartype.Id, ElementId.InvalidElementId);
            arTopY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_1_GENERIC).Set(0);
            arTopY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_2_GENERIC).Set(1);
            arTopY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_1_GENERIC).Set(0);
            arTopY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_2_GENERIC).Set(0);
            arTopY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_SPACING_TOP_DIR_2_GENERIC).Set(interval);
            arTopY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ADDL_TOP_OFFSET).Set(topCoverDir2);
            arTopY.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).Set("верх Y фон");

            AreaReinforcement arBottomX = AreaReinforcement
                .Create(doc, floor, curves, direction, areaTypeId, mrt.bartype.Id, ElementId.InvalidElementId);
            arBottomX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_1_GENERIC).Set(0);
            arBottomX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_2_GENERIC).Set(0);
            arBottomX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_1_GENERIC).Set(1);
            arBottomX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_2_GENERIC).Set(0);
            arBottomX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_SPACING_BOTTOM_DIR_1_GENERIC).Set(interval);
            arBottomX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ADDL_BOTTOM_OFFSET).Set(bottomCoverDir1);
            arBottomX.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).Set("низ X фон");

            AreaReinforcement arBottomY = AreaReinforcement
                .Create(doc, floor, curves, direction, areaTypeId, mrt.bartype.Id, ElementId.InvalidElementId);
            arBottomY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_1_GENERIC).Set(0);
            arBottomY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_2_GENERIC).Set(0);
            arBottomY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_1_GENERIC).Set(0);
            arBottomY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_2_GENERIC).Set(1);
            arBottomY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_SPACING_BOTTOM_DIR_2_GENERIC).Set(interval);
            arBottomY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ADDL_BOTTOM_OFFSET).Set(bottomCoverDir2);
            arBottomY.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).Set("низ Y фон");

            return messages;
        }
    }
}
