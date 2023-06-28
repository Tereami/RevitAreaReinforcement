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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
#endregion

namespace RevitAreaReinforcement
{
    public static class RebarWorkerWall
    {
        public static Guid slabHeightParamGuid { get { return new Guid("b543b893-732a-40a9-9355-a489d3ce3a66"); } }


        public static List<string> GenerateRebar(Document doc, Wall wall, RebarInfoWall wri, RebarCoverType zeroCover, ElementId areaTypeId)
        {
            Debug.WriteLine($"Start reinforcement for wall: {wall.GetElementId()}");
            List<string> messages = new List<string>();


            wall.get_Parameter(BuiltInParameter.CLEAR_COVER_OTHER).Set(zeroCover.Id);
            Debug.WriteLine("Set zero rebar cover for other faces");

            Solid sol = SupportGeometry.GetSolidFromElement(wall);
            List<Face> vertFaces = SupportGeometry.GetVerticalFaces(sol);
            Face mainFace = SupportGeometry.GetLargeFace(vertFaces);
            PlanarFace pface = mainFace as PlanarFace;
            Debug.WriteLine("Vertical planar face was found");

            List<Curve> wallOutlineDraft = SupportGeometry.GetFaceOuterBoundary(pface);
            Debug.WriteLine("Outline draft curves found: " + wallOutlineDraft.Count.ToString());

            //удаляю совпадающие линии
            List<Curve> wallOutline = SupportGeometry.CleanLoop(wallOutlineDraft);
            Debug.WriteLine("Outline clean curves found: " + wallOutline.Count.ToString());


            //определяю отступы для защитных слоев
            RebarCoverType coverFront = doc.GetElement(wall.get_Parameter(BuiltInParameter.CLEAR_COVER_EXTERIOR).AsElementId()) as RebarCoverType;
            RebarCoverType coverBack = doc.GetElement(wall.get_Parameter(BuiltInParameter.CLEAR_COVER_INTERIOR).AsElementId()) as RebarCoverType;

            if (coverFront == null) coverFront = coverBack;
            if (coverBack == null) coverBack = coverFront;

            double userDefineCover = wri.rebarCover;


            MyRebarType verticalRebarType = new MyRebarType(doc, wri.verticalRebarTypeName);
            if (verticalRebarType.isValid == false)
            {
                messages.Add(MyStrings.ErrorFailedToGetRebarType + wri.verticalRebarTypeName);
                Debug.WriteLine("Failed to get vertical rebartype: " + wri.verticalRebarTypeName);
            }
            MyRebarType horizontalRebarType = new MyRebarType(doc, wri.horizontalRebarTypeName);
            if (horizontalRebarType.isValid == false)
            {
                messages.Add(MyStrings.ErrorFailedToGetRebarType + wri.horizontalRebarTypeName);
                Debug.WriteLine("Failed to get horizontal rebartype: " + wri.horizontalRebarTypeName);
            }

#if R2017 || R2018 || R2019 || R2020 || R2021
            double vertDiam = verticalRebarType.bartype.BarDiameter;
            double horizDiam = horizontalRebarType.bartype.BarDiameter;
#else
            double vertDiam = verticalRebarType.bartype.BarNominalDiameter;
            double horizDiam = horizontalRebarType.bartype.BarNominalDiameter;
#endif

            double offsetVerticalExterior = userDefineCover - coverFront.CoverDistance - 0.5 * vertDiam;
            double offsetVerticalInterior = userDefineCover - coverBack.CoverDistance - 0.5 * vertDiam;
            double offsetHorizontalExterior = offsetVerticalExterior - horizDiam;
            double offsetHorizontalInterior = offsetVerticalInterior - horizDiam;

            double freeLengthFromFloorTop = 0;

            if (wri.generateVertical)
            {
                Debug.WriteLine("Start creating vertical rebar");
                Parameter paramFloorThickinessParam = wall.get_Parameter(slabHeightParamGuid);

                if (wri.autoVerticalFreeLength)
                {
                    Debug.WriteLine("Try to auto-calculate vertical free length");
                    if (paramFloorThickinessParam != null && paramFloorThickinessParam.HasValue)
                    {
                        double floorThickness = paramFloorThickinessParam.AsDouble();
                        freeLengthFromFloorTop = ConcreteUtils.getRebarFreeLength(verticalRebarType.bartype, wall,
                            wri.verticalFreeLengthRound, wri.verticalAsymmOffset, wri.verticalRebarStretched);
                        wri.verticalFreeLength = floorThickness + freeLengthFromFloorTop;
                        Debug.WriteLine("Vertical free length = " + wri.verticalFreeLength);
                    }
                    else
                    {
                        Debug.WriteLine("Unable to auto-calculate vertical free length");
                        throw new Exception(MyStrings.ErrorNoSlabHeightParam + wall.GetElementId().ToString());
                    }
                }


                List<Curve> curvesVertical = SupportGeometry.MoveLine(wallOutline, wri.verticalFreeLength, SupportGeometry.LineSide.Top);
                Debug.WriteLine("Curves for vertical loop: " + curvesVertical.Count.ToString());

                if (wri.useUnification)
                {
                    Debug.WriteLine("Try to unificate vertical length");
                    double verticalZoneHeight = SupportGeometry.GetZoneHeigth(curvesVertical);
                    double unificateLength = wri.getNearestLength(verticalZoneHeight);
                    double moveToUnificate = unificateLength - verticalZoneHeight;
                    if (moveToUnificate > 0.005)
                    {
                        List<Curve> curvesVerticalUnificate = SupportGeometry.MoveLine(curvesVertical, moveToUnificate, SupportGeometry.LineSide.Top);
                        curvesVertical = curvesVerticalUnificate;
                    }
                }

                /*LocationCurve wallLocCurve = wall.Location as LocationCurve;
                Line wallCurve = wallLocCurve.Curve as Line;
                if (wallCurve == null) throw new Exception("Curved wall!");*/

                double sideOffset = wri.backOffset - 0.5 * vertDiam;

                curvesVertical = SupportGeometry.MoveLine(curvesVertical, sideOffset, SupportGeometry.LineSide.Left);
                curvesVertical = SupportGeometry.MoveLine(curvesVertical, sideOffset, SupportGeometry.LineSide.Right);

                CurveUtils.SortCurvesContiguous(doc.Application.Create, curvesVertical, true);
                Debug.WriteLine("Contiguous curves sort");

                if (wri.verticalAsymmOffset)
                {
                    Debug.WriteLine("Generate vertical rebar area with offset");
                    AreaReinforcement arVertical1 = Generate(doc, wall, curvesVertical, false, true, false, offsetVerticalInterior, offsetVerticalExterior, wri.verticalRebarInterval, areaTypeId, verticalRebarType.bartype, wri.verticalSectionText);

                    double asymmVerticalOffset =
                        wri.verticalFreeLengthRound * Math.Ceiling((freeLengthFromFloorTop * 1.3) / wri.verticalFreeLengthRound);

                    List<Curve> curves2 = SupportGeometry.MoveLine(curvesVertical, asymmVerticalOffset, SupportGeometry.LineSide.Top);
                    curves2 = SupportGeometry.MoveLine(curves2, asymmVerticalOffset, SupportGeometry.LineSide.Bottom);
                    AreaReinforcement arVertical2 = Generate(doc, wall, curves2, false, false, true, offsetVerticalInterior, offsetVerticalExterior, wri.verticalRebarInterval, areaTypeId, verticalRebarType.bartype, wri.verticalSectionText);
                }
                else
                {
                    Debug.WriteLine("Generate vertical rebar area without offset");
                    AreaReinforcement arVertical = Generate(doc, wall, curvesVertical, false, true, true, offsetVerticalInterior, offsetVerticalExterior, wri.verticalRebarInterval, areaTypeId, verticalRebarType.bartype, wri.verticalSectionText);
                }
            }

            if (wri.generateHorizontal)
            {
                Debug.WriteLine("Start creating horizontal rebar");
                //определяю контур
                double horizontalTopOffset = 0.5 * horizDiam - wri.topOffset;
                double horizintalBottomOffset = wri.bottomOffset - 0.5 * horizDiam;
                List<Curve> curvesHorizontal = SupportGeometry.MoveLine(wallOutline, horizontalTopOffset, SupportGeometry.LineSide.Top);
                curvesHorizontal = SupportGeometry.MoveLine(curvesHorizontal, horizintalBottomOffset, SupportGeometry.LineSide.Bottom);

                double sideOffset = wri.horizontalFreeLength * -1;
                curvesHorizontal = SupportGeometry.MoveLine(curvesHorizontal, sideOffset, SupportGeometry.LineSide.Left);
                curvesHorizontal = SupportGeometry.MoveLine(curvesHorizontal, sideOffset, SupportGeometry.LineSide.Right);
                double horizZoneHeight = SupportGeometry.GetZoneHeigth(curvesHorizontal);

                List<AreaRebarInfo> curvesBase = new List<AreaRebarInfo>();

                if (wri.horizontalIntervalIncreasedTopOrBottom && (wri.horizontalHeightIncreaseIntervalBottom > 0 || wri.horizontalHeightIncreaseIntervalTop > 0))
                {
                    if (wri.horizontalHeightIncreaseIntervalBottom > 0 && wri.horizontalHeightIncreaseIntervalTop == 0)
                    {
                        curvesBase = IncreasedInterval(wri, curvesHorizontal, horizZoneHeight, horizDiam, SupportGeometry.LineSide.Bottom, wri.horizontalHeightIncreaseIntervalBottom);
                    }
                    else if (wri.horizontalHeightIncreaseIntervalTop > 0 && wri.horizontalHeightIncreaseIntervalBottom == 0)
                    {
                        curvesBase = IncreasedInterval(wri, curvesHorizontal, horizZoneHeight, horizDiam, SupportGeometry.LineSide.Top, wri.horizontalHeightIncreaseIntervalTop);
                    }
                    else if (wri.horizontalHeightIncreaseIntervalTop > 0 && wri.horizontalHeightIncreaseIntervalBottom > 0)
                    {
                        double horizRebarInterval = wri.horizontalRebarInterval;
                        double horizRebarHalfInterval = horizRebarInterval / 2;

                        double fullIncreasedHeightBottom = wri.horizontalHeightIncreaseIntervalBottom + horizDiam;
                        List<AreaRebarInfo> curvesBottomIncreased = AddIncreasedHorizIntervalProfiles(curvesHorizontal, fullIncreasedHeightBottom, horizRebarHalfInterval, SupportGeometry.LineSide.Bottom);
                        curvesBase.AddRange(curvesBottomIncreased);

                        double fullIncreasedHeightTop = wri.horizontalHeightIncreaseIntervalTop + horizDiam;
                        List<AreaRebarInfo> curvesTopIncreased = AddIncreasedHorizIntervalProfiles(curvesHorizontal, fullIncreasedHeightTop, horizRebarHalfInterval, SupportGeometry.LineSide.Top);
                        curvesBase.AddRange(curvesTopIncreased);

                        double mainProfileBottomOffset = wri.horizontalHeightIncreaseIntervalBottom + horizRebarInterval;
                        List<Curve> profileMain = SupportGeometry.MoveLine(curvesHorizontal, mainProfileBottomOffset, SupportGeometry.LineSide.Bottom);
                        double mainProfileHeightAfterBottomCut = horizZoneHeight - mainProfileBottomOffset;

                        double mainProfileRoundedHeight = GetRoundedMainProfileHeight(mainProfileHeightAfterBottomCut, fullIncreasedHeightTop, horizDiam, horizRebarInterval);
                        double mainProfileTopOffset = -1 * (mainProfileHeightAfterBottomCut - mainProfileRoundedHeight);
                        profileMain = SupportGeometry.MoveLine(profileMain, mainProfileTopOffset, SupportGeometry.LineSide.Top);
                        curvesBase.Add(new AreaRebarInfo(profileMain, horizRebarInterval));
                    }
                }
                else if (wri.horizontalAddInterval)
                {
                    Debug.WriteLine("Create with additional offset");

                    double horizRebarInterval = wri.horizontalRebarInterval;


                    double heigthByAxis = horizZoneHeight - horizDiam;
                    int countCheck = GetIntervalsCount(heigthByAxis, horizRebarInterval);
                    double addIntervalByAxis = heigthByAxis - countCheck * horizRebarInterval;
                    if (addIntervalByAxis < horizDiam) //доборный шаг не требуется
                    {
                        Debug.WriteLine("Additional offset not needed");
                        curvesBase.Add(new AreaRebarInfo(curvesHorizontal, horizRebarInterval));
                    }
                    else
                    {
                        Debug.WriteLine("Additional offset = " + (addIntervalByAxis * 304.8).ToString("F3"));
                        int count = countCheck - 1;

                        if (addIntervalByAxis < 50 / 304.8) //доборный шаг менее 50мм - увеличиваю отступ сверху
                        {
                            count--;
                        }

                        double heigthClean = count * horizRebarInterval;
                        double offsetMain = horizZoneHeight - heigthClean - horizDiam;
                        List<Curve> profileMain = SupportGeometry.MoveLine(curvesHorizontal, -offsetMain, SupportGeometry.LineSide.Top);

                        curvesBase.Add(new AreaRebarInfo(profileMain, horizRebarInterval));

                        double heigthAdd = horizZoneHeight - heigthClean - horizRebarInterval;

                        List<List<Curve>> profilesAdd = SupportGeometry.CopyTopOrBottomLines(curvesHorizontal, heigthAdd, SupportGeometry.LineSide.Top);
                        foreach (var prof in profilesAdd)
                            curvesBase.Add(new AreaRebarInfo(prof, horizRebarInterval));
                    }
                }

                else
                {
                    Debug.WriteLine("Create only one zone for horizontal rebars");
                    curvesBase.Add(new AreaRebarInfo(curvesHorizontal, wri.horizontalRebarInterval));
                }


                Debug.WriteLine("Loops for horizontal rebar: " + curvesBase.Count.ToString());

                foreach (var profileInfo in curvesBase)
                {
                    double interval = profileInfo.interval;
                    List<Curve> curves = profileInfo.curves;
                    AreaReinforcement ar = Generate(doc, wall, curves, true, true, true, offsetHorizontalInterior, offsetHorizontalExterior, interval, areaTypeId, horizontalRebarType.bartype, wri.horizontalSectionText);
                }
            }
            return messages;
        }


        private static AreaReinforcement Generate(Document doc, Wall wall, List<Curve> profile, bool isHorizontal, bool createInterior, bool createExterior, double offsetInt, double offsetExt, double interval, ElementId areaId, RebarBarType barType, string partition)
        {
            Debug.WriteLine("Start generating area rebar. Profile debug info: ");
            Debug.WriteLine(Util.ProfileDebugInfo(profile));
            CurveUtils.SortCurvesContiguous(doc.Application.Create, profile, true);
            AreaReinforcement arein = AreaReinforcement.Create(doc, wall, profile, new XYZ(0, 0, 1), areaId, barType.Id, ElementId.InvalidElementId);

            if (isHorizontal)
            {
                arein.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BACK_DIR_1).Set(0);
                arein.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_FRONT_DIR_1).Set(0);
                arein.get_Parameter(BuiltInParameter.REBAR_SYSTEM_SPACING_TOP_DIR_2_GENERIC).Set(interval);
                arein.get_Parameter(BuiltInParameter.REBAR_SYSTEM_SPACING_BOTTOM_DIR_2_GENERIC).Set(interval);

                if (!createInterior)
                    arein.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BACK_DIR_2).Set(0);

                if (!createExterior)
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

            Debug.WriteLine("Rebar area created");
            return arein;
        }

        private static int GetIntervalsCount(double length, double interval)
        {
            double countCheckAsDouble1 = length / interval;
            double countCheckAsDouble2 = Math.Round(countCheckAsDouble1, 2);
            double countCheckAsDouble3 = Math.Truncate(countCheckAsDouble2);
            int countCheck = (int)countCheckAsDouble3;
            return countCheck;
        }


        private static List<AreaRebarInfo> IncreasedInterval(RebarInfoWall wri, List<Curve> curvesHorizontal, double horizZoneHeight, double horizDiam, SupportGeometry.LineSide side, double increasedHeight)
        {
            double horizRebarInterval = wri.horizontalRebarInterval;
            double horizRebarHalfInterval = horizRebarInterval / 2;

            double fullIncreasedHeight = increasedHeight + horizDiam;
            List<AreaRebarInfo> curvesBase = AddIncreasedHorizIntervalProfiles(curvesHorizontal, fullIncreasedHeight, horizRebarHalfInterval, side);

            double mainProfileRoundedHeight = GetRoundedMainProfileHeight(horizZoneHeight, fullIncreasedHeight, horizDiam, horizRebarInterval);
            double mainProfileOffset = horizZoneHeight - mainProfileRoundedHeight;

            //List<Curve> profileMain = curvesHorizontal.ToList();
            if (side == SupportGeometry.LineSide.Top)
                mainProfileOffset *= -1;

            List<Curve> profileMain = SupportGeometry.MoveLine(curvesHorizontal, mainProfileOffset, side);
            curvesBase.Add(new AreaRebarInfo(profileMain, horizRebarInterval));

            return curvesBase;
        }

        private static List<AreaRebarInfo> AddIncreasedHorizIntervalProfiles(List<Curve> curvesHorizontal, double fullIncreasedHeight, double increasedInterval, SupportGeometry.LineSide side)
        {
            List<List<Curve>> profilesAddTop =
                            SupportGeometry.CopyTopOrBottomLines(curvesHorizontal, fullIncreasedHeight, side);
            List<AreaRebarInfo> curvesBase = new List<AreaRebarInfo>();
            foreach (var prof in profilesAddTop)
                curvesBase.Add(new AreaRebarInfo(prof, increasedInterval));
            return curvesBase;
        }

        private static double GetRoundedMainProfileHeight(double horizZoneHeight, double fullIncreasedHeight, double horizDiam, double horizRebarInterval)
        {
            double mainProfileZoneHeightNotRound = horizZoneHeight - fullIncreasedHeight - horizDiam;
            int mainRebarsCount = GetIntervalsCount(mainProfileZoneHeightNotRound, horizRebarInterval);
            double mainProfileZoneHeightRound = horizRebarInterval * mainRebarsCount + horizDiam;
            return mainProfileZoneHeightRound;
        }
    }
}
