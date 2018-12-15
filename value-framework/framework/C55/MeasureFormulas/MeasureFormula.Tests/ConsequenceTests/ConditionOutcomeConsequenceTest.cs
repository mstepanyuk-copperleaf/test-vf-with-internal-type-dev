using System;
using System.Collections.Generic;
using System.Linq;
using CL.FormulaHelper.DTOs;
using AutoFixture;
using CL.FormulaHelper;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using NUnit.Framework;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.ConditionOutcomeConsequenceBase;
using formulaClass = CustomerFormulaCode.ConditionOutcomeConsequence;

namespace MeasureFormula.Tests
{
    public class ConditionOutcomeConsequenceTest : MeasureFormulaTestsBase
    {
        private formulaClass Formulas;
        private baseClass.TimeInvariantInputDTO TimeInvariantData;
        private baseClass.TimeVariantInputDTO[] SingleTimeVariantData;
        private baseClass.TimeVariantInputDTO[] MultiTimeVariantData;
        
        private XYCurveDTO AssetConditionDecayCurve;
        private bool AssetImpactedByAlternative = true;
        private DateTime AssetInServiceDate;
        private double OutcomeConditionScore;
        private int SurveyDate;
        private double FirstConditionScore;
        private double SecondConditionScore;
        private double ThirdConditionScore;

        [SetUp]
        public void FixtureSetup()
        {
            Formulas = new formulaClass();
            
            AssetConditionDecayCurve = fixture.Build<XYCurveDTO>().With(x => x.Points, SimpleDepreciationCurve()).Create();
            AssetInServiceDate = new DateTime(ArbitraryStartYear, 1, 1).AddMonths( - fixture.Create<int>() % 60);
            InitializeTimeInvariantTestData(AssetConditionDecayCurve, AssetImpactedByAlternative, AssetInServiceDate);
            TimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            MultiTimeVariantData = DataPrep.BuildTimeVariantData<baseClass.TimeVariantInputDTO>(fixture);
            FirstConditionScore = 3.0 + fixture.Create<double>() % 7;
            SecondConditionScore = 3.0 + fixture.Create<double>() % 7;
            ThirdConditionScore = 3.0 + fixture.Create<double>() % 7;
            DataPrep.UpdateDto(MultiTimeVariantData[0], "OutcomeConditionScore", FirstConditionScore);
            DataPrep.UpdateDto(MultiTimeVariantData[1], "OutcomeConditionScore", SecondConditionScore);
            DataPrep.UpdateDto(MultiTimeVariantData[2], "OutcomeConditionScore", ThirdConditionScore);
            
            OutcomeConditionScore = fixture.Create<double>() % 1000 / 100;
            DataPrep.SetConstructorParameter(fixture, "p_OutcomeConditionScore", OutcomeConditionScore);
            SurveyDate = AssetInServiceDate.Year + fixture.Create<int>() % 5;
            SingleTimeVariantData = BuildSimpleSingleTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, 1, SurveyDate + fixture.Create<int>() % 10 );
        }

        private void InitializeTimeInvariantTestData(XYCurveDTO conditionDecayCurve, Boolean impactedByAlternative, DateTime? inServiceDate)
        {
            DataPrep.SetConstructorParameter(fixture, "p_AssetConditionDecayCurve", conditionDecayCurve);
            DataPrep.SetConstructorParameter(fixture, "p_AssetImpactedByAlternative", impactedByAlternative);
            DataPrep.SetConstructorParameter(fixture, "p_AssetInServiceDate", inServiceDate);
        }

        private CurvePointDTO[] SimpleDepreciationCurve()
        {
            CurvePointDTO[] generatedCurvePointDto = new CurvePointDTO[3];
            generatedCurvePointDto[0] = new CurvePointDTO{CurveId = fixture.Create<long>(),X = 1,Y = 9.7};
            generatedCurvePointDto[1] = new CurvePointDTO{CurveId = fixture.Create<long>(),X = 2,Y = 5.2};
            generatedCurvePointDto[2] = new CurvePointDTO{CurveId = fixture.Create<long>(),X = 3,Y = 2.4};
            return generatedCurvePointDto;
        }
        
        private static T[] BuildSimpleSingleTimeVariantData<T>(Fixture fixture, int periods, int startYear)
        {
            var startYearInThePast = startYear;
            const int monthOffsetFromFiscalYearEnd = 3;
            var timePeriodsStartingBeforeToday = DataPrep.BuildContiguousTimePeriodsStartYearAndOffset(fixture, periods, startYearInThePast, monthOffsetFromFiscalYearEnd);
            fixture.Customizations.Insert(0, new TimePeriodExtractor(timePeriodsStartingBeforeToday));
            var generatedTimeVariantDto = fixture.CreateMany<T>(periods).ToArray();
            return generatedTimeVariantDto;
        }
        
        [Test]
        public void NullTests_GetUnits_ReturnsNull()
        {
            Func<object, object, double?[]> getUnitsCall = (x, y) => Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, (baseClass.TimeInvariantInputDTO) x, (IReadOnlyList<baseClass.TimeVariantInputDTO>) y);
            var nullCheck = new NullablePropertyCheck();
            nullCheck.RunNullTestsIncludingCustomFields(TimeInvariantData, MultiTimeVariantData, getUnitsCall);
        }
        
        [Test]
        public void NullAssetImpactedByAlternative_GetUnits_ReturnsNull()
        {
            InitializeTimeInvariantTestData(AssetConditionDecayCurve, false, AssetInServiceDate);
            var nullImpactByAlternativeDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullImpactByAlternativeDto, MultiTimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void NullConditionCurveAndInServiceDateBeforeStartYear_GetUnits_ReturnsCalc()
        {
            var timelineStartYear = SingleTimeVariantData[0].TimePeriod.StartTime.Year + 2;
            InitializeTimeInvariantTestData(null, AssetImpactedByAlternative, AssetInServiceDate);
            var nullConditionCurveDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(timelineStartYear, ArbitraryMonths, nullConditionCurveDto, SingleTimeVariantData);
            
            var expectedResults = Enumerable.Repeat((double?)SingleTimeVariantData[0].OutcomeConditionScore, ArbitraryMonths).ToArray();
            Assert.That(results, Is.EqualTo(expectedResults).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void NullConditionCurveAndInServiceDateAfterStartYear_GetUnits_ReturnsCalc()
        {
            var timelineStartYear = SingleTimeVariantData[0].TimePeriod.StartTime.Year - 2;
            InitializeTimeInvariantTestData(null, AssetImpactedByAlternative, AssetInServiceDate);
            var nullConditionCurveDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            var monthOfSurvey = FormulaBase.ConvertDateTimeToOffset(SingleTimeVariantData[0].TimePeriod.StartTime, timelineStartYear);
            var randomNumber = fixture.Create<int>() % 60 + 1;
            var maxMonth = Math.Max(ArbitraryMonths, monthOfSurvey + 1) + randomNumber;
            
            var results = Formulas.GetUnits(timelineStartYear, maxMonth, nullConditionCurveDto, SingleTimeVariantData);
            
            var expectedResultsNullList = new double?[monthOfSurvey].ToList();
            var expectedResultsCalcList = Enumerable.Repeat((double?)SingleTimeVariantData[0].OutcomeConditionScore, maxMonth - monthOfSurvey).ToList();
            expectedResultsNullList.AddRange(expectedResultsCalcList);
            Assert.That(results, Is.EqualTo(expectedResultsNullList.ToArray()).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void InServiceDateBeforeStartYearAndSurveyDateAfterStartYear_GetUnits_ReturnsCalc()
        {
            //Test for if (inServiceDate.HasValue && annualConditionDecayCurve != null) when the InServiceDate is before StartFiscalYear
            var conditionScore = 9.1;
            DataPrep.SetConstructorParameter(fixture, "p_OutcomeConditionScore", conditionScore);
            var conditionSurveyDate = AssetInServiceDate.Year + 2 + fixture.Create<int>() % 5;
            var simpleSingleTimeVariantData = BuildSimpleSingleTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, 1, conditionSurveyDate );
            var startYear = conditionSurveyDate - 2;
            var monthOfSurvey = FormulaBase.ConvertDateTimeToOffset(simpleSingleTimeVariantData[0].TimePeriod.StartTime, startYear);
            
            var results = Formulas.GetUnits(startYear, Math.Max(ArbitraryMonths, monthOfSurvey + 1), TimeInvariantData, simpleSingleTimeVariantData);
            Assert.That(results[monthOfSurvey], Is.EqualTo(conditionScore).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void InServiceDateBeforeStartYearAndSurveyDateBeforeStartYear_GetUnits_ReturnsCalc()
        {
            //Test for if (inServiceDate.HasValue && annualConditionDecayCurve != null) when the InServiceDate is before StartFiscalYear
            var conditionScore = 9.1;
            DataPrep.SetConstructorParameter(fixture, "p_OutcomeConditionScore", conditionScore);
            var conditionSurveyDate = AssetInServiceDate.Year + fixture.Create<int>() % 5;
            var simpleSingleTimeVariantData = BuildSimpleSingleTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, 1, conditionSurveyDate );
            var startYear = conditionSurveyDate + 2;
            
            var results = Formulas.GetUnits(startYear, ArbitraryMonths, TimeInvariantData, simpleSingleTimeVariantData);
            
            var expectedResult = TimeInvariantData.AssetConditionDecayCurve.Points[2].Y;
            Assert.That(results[ArbitraryMonths - 1], Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void InServiceDateAfterThanStartYear_GetUnits_ReturnsCalc()
        {
            //Test for if (inServiceDate.HasValue && annualConditionDecayCurve != null) when the InServiceDate is after StartFiscalYear
            var startYear = AssetInServiceDate.Year - 2;
            var conditionScore = 9.1;
            DataPrep.SetConstructorParameter(fixture, "p_OutcomeConditionScore", conditionScore);
            var conditionSurveyDate = AssetInServiceDate.Year + 2 + fixture.Create<int>() % 5;
            var simpleSingleTimeVariantData = BuildSimpleSingleTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, 1, conditionSurveyDate );
            var monthOfSurvey = FormulaBase.ConvertDateTimeToOffset(simpleSingleTimeVariantData[0].TimePeriod.StartTime, startYear);
            var maxMonthRange = Math.Max(ArbitraryMonths, monthOfSurvey) + 1;
            
            var results = Formulas.GetUnits(startYear, maxMonthRange, TimeInvariantData, simpleSingleTimeVariantData);
            
            Assert.That(results[monthOfSurvey], Is.EqualTo(conditionScore).Within(CommonConstants.DoubleDifferenceTolerance));
            var monthBeforeInServiceDate = FormulaBase.ConvertDateTimeToOffset(AssetInServiceDate, startYear);
            Assert.That(results[monthBeforeInServiceDate - 1], Is.Null);
        }
        
        [Test]
        public void NullInServiceDateAndZeroTimeVariantDTOCount_GetUnits_ReturnsNull()
        {
            //Test SingleTimeVariantData count == 0, if (SingleTimeVariantData.Count == 0 && !inServiceDate.HasValue)
            InitializeTimeInvariantTestData(AssetConditionDecayCurve, AssetImpactedByAlternative, null);
            var nullInServiceDateDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            var zeroCountSingleTimeVariantData = BuildSimpleSingleTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, 0, SurveyDate);
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullInServiceDateDto, zeroCountSingleTimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void NullInServiceDateAndOneTimeVariantDTOCount_GetUnits_ReturnsCalc()
        {
            //Test SingleTimeVariantData count > 0, if (SingleTimeVariantData.Count > 0)
            var startyear = SingleTimeVariantData[0].TimePeriod.StartTime.Year - 1 - fixture.Create<int>() % 5;
            InitializeTimeInvariantTestData(AssetConditionDecayCurve, AssetImpactedByAlternative, null);
            var nullInServiceDateDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            var monthOfSurvey = FormulaBase.ConvertDateTimeToOffset(SingleTimeVariantData[0].TimePeriod.StartTime, startyear);
            var maxMonths = Math.Max(ArbitraryMonths, monthOfSurvey) + 1;
            var results = Formulas.GetUnits(startyear, maxMonths, nullInServiceDateDto, SingleTimeVariantData);
            
            var monthlyValueDto = SingleTimeVariantData.Select<ITimeVariantInputDTO, MonthValueDTO>(a => new MonthValueDTO(FormulaBase.ConvertDateTimeToOffset(a.TimePeriod.StartTime, startyear), a.TimePeriod.DurationInMonths, SingleTimeVariantData[0].OutcomeConditionScore)).ToList();
            var expectedResults = FormulaBase.InterpolateCurve(monthlyValueDto, maxMonths, TimeInvariantData.AssetConditionDecayCurve);
            Assert.That(results, Is.EqualTo(expectedResults).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void NullCurveAndNullInServiceDateAndZeroTimeVariantDTOCount_GetUnits_ReturnsNull()
        {
            InitializeTimeInvariantTestData(null, AssetImpactedByAlternative, null);
            var nullCurveNullInServiceDateDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            var zeroCountSingleTimeVariantData = BuildSimpleSingleTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, 0, SurveyDate);
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullCurveNullInServiceDateDto, zeroCountSingleTimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void NullCurveAndNullInServiceDateAndOneTimeVariantDTOCount_GetUnits_ReturnsCalc()
        {
            var startyear = SingleTimeVariantData[0].TimePeriod.StartTime.Year - 1 - fixture.Create<int>() % 5;
            InitializeTimeInvariantTestData(null, AssetImpactedByAlternative, null);
            var nullInServiceDateDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            var monthOfSurvey = FormulaBase.ConvertDateTimeToOffset(SingleTimeVariantData[0].TimePeriod.StartTime, startyear);
            var maxMonths = Math.Max(ArbitraryMonths, monthOfSurvey) + 1;
            
            var results = Formulas.GetUnits(startyear, maxMonths, nullInServiceDateDto, SingleTimeVariantData);
            
            var expectedResults = new double?[monthOfSurvey].ToList();
            var expectedResultsCalcList = Enumerable.Repeat((double?)SingleTimeVariantData[0].OutcomeConditionScore, maxMonths - monthOfSurvey).ToList();
            expectedResults.AddRange(expectedResultsCalcList);
            Assert.That(results, Is.EqualTo(expectedResults).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void TimeVariantZeroCountWithInServuceDate_GetUnits_ReturnsCalc()
        {
            if (TimeInvariantData.AssetInServiceDate != null)
            {
                var offset = FormulaBase.ConvertDateTimeToOffset(TimeInvariantData.AssetInServiceDate.Value, ArbitraryStartYear);
                var val1 = int.MaxValue;
                var expectedResults = new double?[ArbitraryMonths];
                if (val1 > 0 && offset < val1)
                {
                    int num = Math.Min(val1, ArbitraryMonths) - 1;
                    for (int index = Math.Max(offset, 0); index <= num; ++index)
                    {
                        expectedResults[index] = TimeInvariantData.AssetConditionDecayCurve.YFromX((index - offset) / CommonConstants.MonthsPerYear);
                    }
                }
                var zeroCountSingleTimeVariantData = BuildSimpleSingleTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, 0, SurveyDate);
                
                var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, TimeInvariantData, zeroCountSingleTimeVariantData);
                Assert.That(results, Is.EqualTo(expectedResults).Within(CommonConstants.DoubleDifferenceTolerance));
            }
        }
        
        [Test]
        public void ZeroCountTimeVariantAndNullCurve_GetUnits_ReturnsNull()
        {
            InitializeTimeInvariantTestData(null, AssetImpactedByAlternative, null);
            var nullCurveNullInServiceDateDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            var zeroCountSingleTimeVariantData = BuildSimpleSingleTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, 0, SurveyDate);
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullCurveNullInServiceDateDto, zeroCountSingleTimeVariantData);
            Assert.That(results, Is.Null);
        }

        [Test]
        public void threeTimeVariantAndNullInServiceAndNullCurve_GetUnits_ReturnCalc()
        {
            var startYears = MultiTimeVariantData[0].TimePeriod.StartTime.Year - 2;
            InitializeTimeInvariantTestData(null, AssetImpactedByAlternative, null);
            var nullCurveNullInServiceDateDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            var totalMonthsOfSurvey = FormulaBase.ConvertDateTimeToOffset(MultiTimeVariantData[2].TimePeriod.StartTime, startYears);
            var maxMonths = Math.Max(ArbitraryMonths, totalMonthsOfSurvey) + 1;
            
            var results = Formulas.GetUnits(startYears, maxMonths, nullCurveNullInServiceDateDto, MultiTimeVariantData);
            
            var beforeSurvey0 = FormulaBase.ConvertDateTimeToOffset(MultiTimeVariantData[0].TimePeriod.StartTime, startYears);
            var betweenSurvey0And1 =  FormulaBase.ConvertDateTimeToOffset(MultiTimeVariantData[1].TimePeriod.StartTime, startYears) - beforeSurvey0;
            var betweenSurvey1And2 =  FormulaBase.ConvertDateTimeToOffset(MultiTimeVariantData[2].TimePeriod.StartTime, startYears) - beforeSurvey0 - betweenSurvey0And1;
            var expectedResults = new double?[beforeSurvey0].ToList();
            var expectedResultsCalcList0And1 = Enumerable.Repeat((double?)MultiTimeVariantData[0].OutcomeConditionScore, betweenSurvey0And1).ToList();
            var expectedResultsCalcList1And2 = Enumerable.Repeat((double?)MultiTimeVariantData[1].OutcomeConditionScore, betweenSurvey1And2).ToList();
            var remainingMonths = maxMonths - beforeSurvey0 - betweenSurvey0And1 - betweenSurvey1And2;
            var expectedResultsCalcList2 = Enumerable.Repeat((double?)MultiTimeVariantData[2].OutcomeConditionScore, remainingMonths).ToList();
            expectedResults.AddRange(expectedResultsCalcList0And1);
            expectedResults.AddRange(expectedResultsCalcList1And2);
            expectedResults.AddRange(expectedResultsCalcList2);
            
            Assert.That(results, Is.EqualTo(expectedResults.ToArray()).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void threeTimeVariantAndNullInService_GetUnits_ReturnCalc()
        {
            var startYears = MultiTimeVariantData[0].TimePeriod.StartTime.Year - 2;
            InitializeTimeInvariantTestData(AssetConditionDecayCurve, AssetImpactedByAlternative, null);
            var nullInServiceDateDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            var totalMonthsOfSurvey = FormulaBase.ConvertDateTimeToOffset(MultiTimeVariantData[2].TimePeriod.StartTime, startYears);
            var maxMonths = Math.Max(ArbitraryMonths, totalMonthsOfSurvey) + 1;
            
            var results = Formulas.GetUnits(startYears, maxMonths, nullInServiceDateDto, MultiTimeVariantData);
            
            var beforeSurvey0 = FormulaBase.ConvertDateTimeToOffset(MultiTimeVariantData[0].TimePeriod.StartTime, startYears);
            var betweenSurvey0And1 =  FormulaBase.ConvertDateTimeToOffset(MultiTimeVariantData[1].TimePeriod.StartTime, startYears);
            var betweenSurvey1And2 =  FormulaBase.ConvertDateTimeToOffset(MultiTimeVariantData[2].TimePeriod.StartTime, startYears);
            
            Assert.That(results[beforeSurvey0 - 1], Is.Null);
            Assert.That(results[beforeSurvey0], Is.EqualTo(MultiTimeVariantData[0].OutcomeConditionScore).Within(CommonConstants.DoubleDifferenceTolerance));
            Assert.That(results[betweenSurvey0And1], Is.EqualTo(MultiTimeVariantData[1].OutcomeConditionScore).Within(CommonConstants.DoubleDifferenceTolerance));
            Assert.That(results[betweenSurvey1And2], Is.EqualTo(MultiTimeVariantData[2].OutcomeConditionScore).Within(CommonConstants.DoubleDifferenceTolerance));
        }
    }
}