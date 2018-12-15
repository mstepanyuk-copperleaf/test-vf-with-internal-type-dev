using System;
using System.Collections.Generic;
using AutoFixture;
using CL.FormulaHelper.DTOs;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using NUnit.Framework;
using MeasureFormula.Common_Code;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.SimpleAssetReplacementCostLegacyMonthlyBase;
using formulaClass = CustomerFormulaCode.SimpleAssetReplacementCostLegacyMonthly;
    
namespace MeasureFormula.Tests
{
    [TestFixture]
    public class SimpleAssetReplacementCostLegacyMonthlyTests : MeasureFormulaTestsBase
    {
        private formulaClass Formulas;
        
        private decimal AssetReplacementCost;
        private double AssetTypeCostVariationFactor;
        private baseClass.TimeInvariantInputDTO TimeInvariantData; 
        private baseClass.TimeVariantInputDTO[] TimeVariantData;
        private XYCurveDTO AssetDataCurve;

        private double?[] conditionTimeline;
        
        [SetUp]
        public void FixtureSetup()
        {
            Formulas = new formulaClass();
            
            AssetDataCurve = fixture.Build<XYCurveDTO>().With(x => x.Points, CurvePointCreation(fixture.Create<int>())).Create();
            AssetReplacementCost = fixture.Create<decimal>() % 100000;
            AssetTypeCostVariationFactor = fixture.Create<double>() % 100 / 100;
            conditionTimeline = CreateConditionScoreArray(ArbitraryMonths, 9, 0.3, 3);
            InitializeTimeInvariantTestData(AssetReplacementCost, AssetDataCurve, AssetTypeCostVariationFactor, conditionTimeline);
            TimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            TimeVariantData = DataPrep.BuildTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, ArbitraryStartYear);
        }
        
        private CurvePointDTO[] CurvePointCreation(int numCurvePoints)
        {
            var generatedCurvePointDto = new CurvePointDTO[numCurvePoints];
            for (var i = 0; i < generatedCurvePointDto.Length; i++)
            {
                generatedCurvePointDto[i] = new CurvePointDTO
                {
                    CurveId = fixture.Create<long>(),
                    X = (double) i + 1,
                    Y = (double) 1 / numCurvePoints
                };
            }
            return generatedCurvePointDto;
        }
        
        private void InitializeTimeInvariantTestData(decimal? replacementCost, XYCurveDTO spendProfileCurve, double costVariationFactor, double?[] conditionArray)
        {
            DataPrep.SetConstructorParameter(fixture, "p_AssetReplacementCost", replacementCost);
            DataPrep.SetConstructorParameter(fixture, "p_AssetTypeAnnualSpendProfileCurve", spendProfileCurve);
            DataPrep.SetConstructorParameter(fixture, "p_AssetTypeCostVariationFactor", costVariationFactor);
            DataPrep.SetConstructorParameter(fixture, "p_ConditionScore_Condition_ConsqUnitOutput", conditionArray);
        }

        private double?[] CreateConditionScoreArray(int numberOfMonths, double initialCondition, double degradation, int numberOfReplacements = 1)
        {
            double?[] conditionArray = new double?[numberOfMonths];
            for (int i = 0; i < numberOfMonths; i++)
            {
                conditionArray[i] = initialCondition;
                initialCondition -= degradation;
                if (initialCondition <= CustomerConstants.WorstConditionScore && numberOfReplacements >= 1)
                {
                    initialCondition = CustomerConstants.BestConditionScore;
                    numberOfReplacements--;
                }
                if (initialCondition <= CustomerConstants.WorstConditionScore && numberOfReplacements == 0)
                {
                    initialCondition = CustomerConstants.WorstConditionScore;
                }
            }
            return conditionArray;
        }
        
        [Test]
        public void GetUnits_NullTests()
        {
            Func<object, object, double?[]> getUnitsCall = (x, y) => Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, (baseClass.TimeInvariantInputDTO) x, (IReadOnlyList<baseClass.TimeVariantInputDTO>) y);
            var nullCheck = new NullablePropertyCheck();
            nullCheck.RunNullTestsIncludingCustomFields(TimeInvariantData, TimeVariantData, getUnitsCall);
        }

        [Test]
        public void NullSpendCurveTest_GetUnits_ReturnsNull()
        {
            var timeIvariantDataNullCurve = new baseClass.TimeInvariantInputDTO(fixture.Create<decimal>(), null, fixture.Create<int>(), conditionTimeline);
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, timeIvariantDataNullCurve, TimeVariantData );
            Assert.That(results, Is.Null);
        }

        [Test]
        public void TimeInvariant0YearCurve_GetUnits_ReturnsNull()
        {
            var timeInvariantDataNoYears = TimeInvariantData;
            foreach (var pointDto in timeInvariantDataNoYears.AssetTypeAnnualSpendProfileCurve.Points)
            {
                pointDto.X = 0;
            }

            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, timeInvariantDataNoYears, TimeVariantData);
            Assert.That(results, Is.Null);
        }

        [Test]
        public void ConditionTimelineFullOfNull_GetUnits_ReturnsZeros()
        {
            double?[] emptyConditionArray = new double?[ArbitraryMonths];
            InitializeTimeInvariantTestData(AssetReplacementCost, AssetDataCurve, AssetTypeCostVariationFactor, emptyConditionArray);
            var timeInvariantDataNullCondition = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, timeInvariantDataNullCondition, TimeVariantData);
            
            var expectedResults = new double?[ArbitraryMonths];
            Assert.That(results, Is.EqualTo(expectedResults));
        }
        
        [Test]
        public void AssetValueTesting_GetUnits_ReturnsCorrect()
        {
            decimal replacementCost = 3000;
            int totalConditionTimeline = 120;
            var simpleCurve = fixture.Build<XYCurveDTO>().With(x => x.Points, CurvePointCreation(4)).Create();
            simpleCurve.Points[0].Y = 0.3;
            simpleCurve.Points[1].Y = 0.4;
            simpleCurve.Points[2].Y = 0.2;
            simpleCurve.Points[3].Y = 0.1;
            var simpleConditionTimeline = CreateConditionScoreArray(totalConditionTimeline, 9, 0.3, 3);
            InitializeTimeInvariantTestData(replacementCost, simpleCurve, 1, simpleConditionTimeline);
            
            var simpleTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            var results = Formulas.GetUnits(ArbitraryStartYear, totalConditionTimeline, simpleTimeInvariantData, TimeVariantData);
            
            //Calculating expected value
            var replacementCostCalcValue = ((float) replacementCost * 1) * (1 + 1);
            var proportion = simpleTimeInvariantData.AssetTypeAnnualSpendProfileCurve.Points[0].Y;
            var monthlySpend = proportion * replacementCostCalcValue / CommonConstants.MonthsPerYearInt;
            
            if (results != null)
            {
                //Check that the resulting array matches the predetermined array at certain spots (calculation done in excel)
                Assert.That(results[0], Is.EqualTo(monthlySpend).Within(CommonConstants.DoubleDifferenceTolerance));
                Assert.That(results[10], Is.EqualTo(200).Within(CommonConstants.DoubleDifferenceTolerance));
                Assert.That(results[20], Is.EqualTo(100).Within(CommonConstants.DoubleDifferenceTolerance));
                Assert.That(results[31], Is.EqualTo(200).Within(CommonConstants.DoubleDifferenceTolerance));
            }
            else
            {
                Console.WriteLine("Result is returning a null");
                Assert.Fail();
            }
        }

        [Test]
        public void NullReplacementCost_GetUnits_ReturnsNull()
        {
            InitializeTimeInvariantTestData(null, AssetDataCurve, AssetTypeCostVariationFactor, conditionTimeline);
            var nullReplacementCostTimeInvariantDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullReplacementCostTimeInvariantDto, TimeVariantData);
            Assert.That(results, Is.Null);
        }
    }
}