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
using Autodesk.Revit.UI;
#endregion

namespace RevitAreaReinforcement
{
    public static class RebarWorkerWall
    {
        public static void GenerateRebar(Document doc, Wall wall, RebarInfoWall wri, RebarCoverType zeroCover, ElementId areaTypeId)
        {
            wall.get_Parameter(BuiltInParameter.CLEAR_COVER_OTHER).Set(zeroCover.Id);

            Solid sol = SupportGeometry.GetSolidFromElement(wall);
            List<Face> vertFaces = SupportGeometry.GetVerticalFaces(sol);
            Face mainFace = SupportGeometry.GetLargeFace(vertFaces);
            PlanarFace pface = mainFace as PlanarFace;

            List<Curve> wallOutlineDraft = SupportGeometry.GetFaceOuterBoundary(pface);

            //удаляю совпадающие линии
            List<Curve> wallOutline = SupportGeometry.CleanLoop(wallOutlineDraft);



            //определяю отступы для защитных слоев
            RebarCoverType coverFront = doc.GetElement(wall.get_Parameter(BuiltInParameter.CLEAR_COVER_EXTERIOR).AsElementId()) as RebarCoverType;
            RebarCoverType coverBack = doc.GetElement(wall.get_Parameter(BuiltInParameter.CLEAR_COVER_INTERIOR).AsElementId()) as RebarCoverType;

            if (coverFront == null) coverFront = coverBack;
            if (coverBack == null) coverBack = coverFront;

            double userDefineCover = wri.rebarCover;

            MyRebarType verticalRebarType = new MyRebarType(doc, wri.verticalRebarTypeName);
            MyRebarType horizontalRebarType = new MyRebarType(doc, wri.horizontalRebarTypeName);

            double offsetHorizontalExterior = userDefineCover - coverFront.CoverDistance;
            double offsetHorizontalInterior = userDefineCover - coverBack.CoverDistance;
            double offsetVerticalExterior = offsetHorizontalExterior + horizontalRebarType.bartype.BarDiameter;
            double offsetVerticalInterior = offsetHorizontalInterior + horizontalRebarType.bartype.BarDiameter;





            if (wri.generateVertical)
            {
                //определяю контур
                List<Curve> curvesVertical = SupportGeometry.MoveLine(wallOutline, wri.verticalFreeLength, SupportGeometry.LineSide.Top);
                CurveUtils.SortCurvesContiguous(doc.Application.Create, curvesVertical, true);

                if (wri.verticalOffset < 0.0001)
                {
                    AreaReinforcement arVertical = Generate(doc, wall, curvesVertical, false, true, true, offsetVerticalInterior, offsetVerticalExterior, wri.verticalRebarInterval, areaTypeId, verticalRebarType.bartype, wri.verticalSectionText);
                }
                else
                {
                    AreaReinforcement arVertical1 = Generate(doc, wall, curvesVertical, false, true, false, offsetVerticalInterior, offsetVerticalExterior, wri.verticalRebarInterval, areaTypeId, verticalRebarType.bartype, wri.verticalSectionText);

                    List<Curve> curves2 = SupportGeometry.MoveLine(curvesVertical, wri.verticalOffset, SupportGeometry.LineSide.Top);
                    curves2 = SupportGeometry.MoveLine(curves2, wri.verticalOffset, SupportGeometry.LineSide.Bottom);
                    AreaReinforcement arVertical2 = Generate(doc, wall, curves2, false, false, true, offsetVerticalInterior, offsetVerticalExterior, wri.verticalRebarInterval, areaTypeId, verticalRebarType.bartype, wri.verticalSectionText);
                }
            }

            if (wri.generateHorizontal)
            {
                //определяю контур
                double horizontalTopOffset = 0.5 * horizontalRebarType.bartype.BarDiameter - wri.topOffset;
                double horizintalBottomOffset = wri.bottomOffset - 0.5 * horizontalRebarType.bartype.BarDiameter;
                List<Curve> curvesHorizontal = SupportGeometry.MoveLine(wallOutline, horizontalTopOffset, SupportGeometry.LineSide.Top);
                curvesHorizontal = SupportGeometry.MoveLine(curvesHorizontal, horizintalBottomOffset, SupportGeometry.LineSide.Bottom);
               

                List<List<Curve>> curvesArray = new List<List<Curve>>();

                if (!wri.horizontalAddInterval)
                {
                    //AreaReinforcement arHorizontal = Generate(doc, wall, curvesHorizontal, true, offsetHorizontalInterior, offsetHorizontalExterior, wri.horizontalRebarInterval, areaTypeId, horizontalRebarType.bartype, wri.horizontalSectionText);
                    curvesArray.Add(curvesHorizontal);
                }
                else
                {
                    double heigth = SupportGeometry.GetZoneHeigth(curvesHorizontal);
                    double heigthByAxis = heigth - horizontalRebarType.bartype.BarDiameter;
                    double countCheckAsDouble1 = heigthByAxis / wri.horizontalRebarInterval;
                    double countCheckAsDouble2 = Math.Round(countCheckAsDouble1, 2);
                    double countCheckAsDouble3 = Math.Truncate(countCheckAsDouble2);
                    int countCheck = (int)countCheckAsDouble3;
                    double addIntervalByAxis = heigthByAxis - countCheck * wri.horizontalRebarInterval;
                    if (addIntervalByAxis < horizontalRebarType.bartype.BarDiameter) //доборный шаг не требуется
                    {
                        //AreaReinforcement arHorizontal = Generate(doc, wall, curvesHorizontal, true, offsetHorizontalInterior, offsetHorizontalExterior, wri.horizontalRebarInterval, areaTypeId, horizontalRebarType.bartype, wri.horizontalSectionText);
                        curvesArray.Add(curvesHorizontal);
                    }
                    else
                    {
                        int count = countCheck - 1;
                        double heigthClean = count * wri.horizontalRebarInterval;
                        double offsetMain = heigth - heigthClean - horizontalRebarType.bartype.BarDiameter;
                        List<Curve> profileMain = SupportGeometry.MoveLine(curvesHorizontal, -offsetMain, SupportGeometry.LineSide.Top);
                        curvesArray.Add(profileMain);


                        //double offsetAdd = heigthClean + wri.horizontalRebarInterval;
                        //List<Curve> profileAdd = SupportGeometry.MoveLine(curvesHorizontal, offsetAdd, SupportGeometry.LineSide.Bottom);

                        double heigthAdd = heigth - heigthClean - wri.horizontalRebarInterval;

                        List<List<Curve>> profilesAdd = SupportGeometry.CopyTopLines(curvesHorizontal, heigthAdd);
                        curvesArray.AddRange(profilesAdd);
                    }
                }

                foreach (List<Curve> profile in curvesArray)
                {
                    AreaReinforcement ar = Generate(doc, wall, profile, true, true, true, offsetHorizontalInterior, offsetHorizontalExterior, wri.horizontalRebarInterval, areaTypeId, horizontalRebarType.bartype, wri.horizontalSectionText);
                }



                //AreaReinforcement arHorizontal = AreaReinforcement
                //    .Create(doc, wall, curvesHorizontal, new XYZ(0, 0, 1), areaTypeId, horizontalRebarType.bartype.Id, ElementId.InvalidElementId);
                //arHorizontal.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BACK_DIR_1).Set(0);
                //arHorizontal.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_FRONT_DIR_1).Set(0);
                //arHorizontal.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ADDL_INTERIOR_OFFSET).Set(offsetHorizontalInterior);
                //arHorizontal.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ADDL_EXTERIOR_OFFSET).Set(offsetHorizontalExterior);
                //arHorizontal.get_Parameter(BuiltInParameter.REBAR_SYSTEM_SPACING_TOP_DIR_2_GENERIC).Set(wri.horizontalRebarInterval);
                //arHorizontal.get_Parameter(BuiltInParameter.REBAR_SYSTEM_SPACING_BOTTOM_DIR_2_GENERIC).Set(wri.horizontalRebarInterval);
                //arHorizontal.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).Set(wri.horizontalSectionText);
            }
        }


        private static AreaReinforcement Generate(Document doc, Wall wall, List<Curve> profile, bool isHorizontal, bool createInterior, bool createExterior, double offsetInt, double offsetExt, double interval, ElementId areaId, RebarBarType barType, string partition)
        {
            CurveUtils.SortCurvesContiguous(doc.Application.Create, profile, true);
            AreaReinforcement arein = AreaReinforcement.Create(doc, wall, profile, new XYZ(0, 0, 1), areaId, barType.Id, ElementId.InvalidElementId);



            if (isHorizontal)
            {
                arein.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BACK_DIR_1).Set(0);
                arein.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_FRONT_DIR_1).Set(0);
                arein.get_Parameter(BuiltInParameter.REBAR_SYSTEM_SPACING_TOP_DIR_2_GENERIC).Set(interval);
                arein.get_Parameter(BuiltInParameter.REBAR_SYSTEM_SPACING_BOTTOM_DIR_2_GENERIC).Set(interval);

                if(!createInterior)
                    arein.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BACK_DIR_2).Set(0);

                if(!createExterior)
                    arein.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_FRONT_DIR_2).Set(0);
            }
            else
            {
                arein.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_FRONT_DIR_2).Set(0);
                arein.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BACK_DIR_2).Set(0);
                arein.get_Parameter(BuiltInParameter.REBAR_SYSTEM_SPACING_TOP_DIR_1_GENERIC).Set(interval);
                arein.get_Parameter(BuiltInParameter.REBAR_SYSTEM_SPACING_BOTTOM_DIR_1_GENERIC).Set(interval);

                if (!createInterior)
                    arein.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BACK_DIR_1).Set(0);

                if (!createExterior)
                    arein.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_FRONT_DIR_1).Set(0);

            }
            arein.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ADDL_INTERIOR_OFFSET).Set(offsetInt);
            arein.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ADDL_EXTERIOR_OFFSET).Set(offsetExt);
            arein.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).Set(partition);

            return arein;
        }
    }
}
