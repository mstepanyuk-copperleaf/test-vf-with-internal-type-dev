using System;
using System.Collections.Generic;
using AutoFixture;
using MeasureFormula.TestHelpers;
using NUnit.Framework;
using System.Linq;
using MeasureFormula.SharedCode;
using MeasureFormula.Common_Code;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.SimpleAssetOMSavingsConsequenceMonthlyBase;
using formulaClass = CustomerFormulaCode.SimpleAssetOMSavingsConsequenceMonthly;

namespace MeasureFormula.Tests
{
    public class SimpleAssetOMSavingsConsequenceMonthlyTest : MeasureFormulaTestsBase
    {
        private formulaClass Formulas;
        private baseClass.TimeInvariantInputDTO TimeInvariantData;
        private baseClass.TimeVariantInputDTO[] TimeVariantData;

        private int? AssetOMAAnualCostHigh;
        private double? AssetOMAAnnualCostHighConditionUpperBound;
        private int? AssetOMAAnnualCostLow;
        private int? AssetOMAAnnualCostMedium;
        private double? AssetOMAAnnualCostMediumConditionUpperBound;
        private double?[] AssetConditionScoreConsqUnitOutput;
        
        [SetUp]
        public void FixtureSetup()
        {
            Formulas = new formulaClass();
            
            AssetOMAAnualCostHigh = fixture.Create<int?>() % 10000;
            AssetOMAAnnualCostHighConditionUpperBound = fixture.Create<double?>() % 10000;
            AssetOMAAnnualCostLow = fixture.Create<int?>() % 10000;
            AssetOMAAnnualCostMedium = fixture.Create<int?>() % 10000;
            AssetOMAAnnualCostMediumConditionUpperBound = fixture.Create<double?>() % 10000;
            AssetConditionScoreConsqUnitOutput = CreateConditionScoreArray(ArbitraryMonths,7, 0.3, 3);

            InitializeTimeInvariantTestData(AssetOMAAnualCostHigh,
                                            AssetOMAAnnualCostHighConditionUpperBound,
                                            AssetOMAAnnualCostLow,
                                            AssetOMAAnnualCostMedium,
                                            AssetOMAAnnualCostMediumConditionUpperBound,
                                            AssetConditionScoreConsqUnitOutput);
            TimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            TimeVariantData = DataPrep.BuildTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, ArbitraryStartYear);
        }
        
        private void InitializeTimeInvariantTestData(int? OMAAnnualCostHigh,
                                                     double? OMAAnnualCostHighConditionUpperBound,
                                                     int? OMAAnnualCostLow,
                                                     int? OMAAnnualCostMedium,
                                                     double? OMAAnnualCostMediumConditionUpperBound,
                                                     double?[] conditionScoreConsqUnitOutput)
        {
            DataPrep.SetConstructorParameter(fixture, "p_AssetTypeOMAAnnualCostHigh", OMAAnnualCostHigh);
            DataPrep.SetConstructorParameter(fixture, "p_AssetTypeOMAAnnualCostHighConditionUpperBound", OMAAnnualCostHighConditionUpperBound);
            DataPrep.SetConstructorParameter(fixture, "p_AssetTypeOMAAnnualCostLow", OMAAnnualCostLow);
            DataPrep.SetConstructorParameter(fixture, "p_AssetTypeOMAAnnualCostMedium", OMAAnnualCostMedium);
            DataPrep.SetConstructorParameter(fixture, "p_AssetTypeOMAAnnualCostMediumConditionUpperBound", OMAAnnualCostMediumConditionUpperBound);
            DataPrep.SetConstructorParameter(fixture, "p_ConditionScore_Condition_ConsqUnitOutput", conditionScoreConsqUnitOutput);
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
        public void NullOMAAnnualCostHigh_GetUnits_ReturnsNull()
        {
            InitializeTimeInvariantTestData(null,
                                            AssetOMAAnnualCostHighConditionUpperBound,
                                            AssetOMAAnnualCostLow,
                                            AssetOMAAnnualCostMedium,
                                            AssetOMAAnnualCostMediumConditionUpperBound,
                                            AssetConditionScoreConsqUnitOutput);
            var nullOMAAnnualCostHighTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullOMAAnnualCostHighTimeInvariantData, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void NullOMAAnnualCostHighConditionUpperBoundTimeInvariantData_GetUnits_ReturnsNull()
        {
            InitializeTimeInvariantTestData(AssetOMAAnualCostHigh,
                                            null,
                                            AssetOMAAnnualCostLow,
                                            AssetOMAAnnualCostMedium,
                                            AssetOMAAnnualCostMediumConditionUpperBound,
                                            AssetConditionScoreConsqUnitOutput);
            var nullOMAAnnualCostHighConditionUpperBoundTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullOMAAnnualCostHighConditionUpperBoundTimeInvariantData, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void NullOMAAnnualCostLow_GetUnits_ReturnsNull()
        {
            InitializeTimeInvariantTestData(AssetOMAAnualCostHigh,
                                            AssetOMAAnnualCostHighConditionUpperBound,
                                            null,
                                            AssetOMAAnnualCostMedium,
                                            AssetOMAAnnualCostMediumConditionUpperBound,
                                            AssetConditionScoreConsqUnitOutput);
            var nullOMAAnnualCostLowTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullOMAAnnualCostLowTimeInvariantData, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void NullOMAAnnualCostMedium_GetUnits_ReturnsNull()
        {
            InitializeTimeInvariantTestData(AssetOMAAnualCostHigh,
                                            AssetOMAAnnualCostHighConditionUpperBound,
                                            AssetOMAAnnualCostLow,
                                            null,
                                            AssetOMAAnnualCostMediumConditionUpperBound,
                                            AssetConditionScoreConsqUnitOutput);
            var nullOMAAnnualCostMediumTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullOMAAnnualCostMediumTimeInvariantData, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void NullOMAAnnualCostMediumConditionUpperBound_GetUnits_ReturnsNull()
        {
            InitializeTimeInvariantTestData(AssetOMAAnualCostHigh,
                                            AssetOMAAnnualCostHighConditionUpperBound,
                                            AssetOMAAnnualCostLow,
                                            AssetOMAAnnualCostMedium,
                                            null,
                                            AssetConditionScoreConsqUnitOutput);
            var nullOMAAnnualCostMediumConditionUpperBoundTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullOMAAnnualCostMediumConditionUpperBoundTimeInvariantData, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void NullConditionScore_GetUnits_ReturnsNull()
        {
            InitializeTimeInvariantTestData(AssetOMAAnualCostHigh,
                                            AssetOMAAnnualCostHighConditionUpperBound,
                                            AssetOMAAnnualCostLow,
                                            AssetOMAAnnualCostMedium,
                                            AssetOMAAnnualCostMediumConditionUpperBound,
                                            null);
            var nullConditionScoreTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullConditionScoreTimeInvariantData, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void NullConditionScoreArrayContents_GetUnits_ReturnsNull()
        {
            double?[] nullConditionScoreArrayContents = new double?[ArbitraryMonths];
            InitializeTimeInvariantTestData(AssetOMAAnualCostHigh,
                                            AssetOMAAnnualCostHighConditionUpperBound,
                                            AssetOMAAnnualCostLow,
                                            AssetOMAAnnualCostMedium,
                                            AssetOMAAnnualCostMediumConditionUpperBound,
                                            nullConditionScoreArrayContents);
            var nullConditionScoreContentsTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullConditionScoreContentsTimeInvariantData, TimeVariantData);
            Assert.That(results, Is.EqualTo(nullConditionScoreArrayContents));
        }
        
        [Test]
        public void ConditionGreaterThanUpperBound_GetUnits_ReturnsCostLowArray()
        {
            double?[] conditionEqualstoMediumConditionUpperBound =
                Enumerable.Repeat(AssetOMAAnnualCostMediumConditionUpperBound, ArbitraryMonths).ToArray();
            InitializeTimeInvariantTestData(AssetOMAAnualCostHigh,
                                            AssetOMAAnnualCostHighConditionUpperBound,
                                            AssetOMAAnnualCostLow,
                                            AssetOMAAnnualCostMedium,
                                            AssetOMAAnnualCostMediumConditionUpperBound,
                                            conditionEqualstoMediumConditionUpperBound);
            var conditionUpperBoundTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, conditionUpperBoundTimeInvariantData, TimeVariantData);
            
            var expectedResults = Enumerable.Repeat(AssetOMAAnnualCostLow / CommonConstants.MonthsPerYear, ArbitraryMonths).ToArray();
            Assert.That(results, Is.EqualTo(expectedResults).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void ConditionBetweenBounds_GetUnits_ReturnsCostLowArray()
        {
            double? middleConditionTestValue = 8;
            double? costHighConditionUpperBound = 4;
            double? costMediumConditionUpperBound = 9;
            double?[] conditionBetweenBounds = Enumerable.Repeat(middleConditionTestValue, ArbitraryMonths).ToArray();
            InitializeTimeInvariantTestData(AssetOMAAnualCostHigh,
                                            costHighConditionUpperBound,
                                            AssetOMAAnnualCostLow,
                                            AssetOMAAnnualCostMedium,
                                            costMediumConditionUpperBound,
                                            conditionBetweenBounds);
            var conditionBetweenBoundsTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, conditionBetweenBoundsTimeInvariantData, TimeVariantData);
            
            var expectedResults = Enumerable.Repeat(AssetOMAAnnualCostMedium / CommonConstants.MonthsPerYear, ArbitraryMonths).ToArray();
            Assert.That(results, Is.EqualTo(expectedResults).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void ConditionLessThanCostHighConditionUpperBound_GetUnits_ReturnsCostLowArray()
        {
            double? lowConditionTestValue = 3;
            double? costHighConditionUpperBound = 4;
            double? costMediumConditionUpperBound = 9;
            double?[] conditionLessThanCostHighConditionUpperBound = Enumerable.Repeat(lowConditionTestValue, ArbitraryMonths).ToArray();
            InitializeTimeInvariantTestData(AssetOMAAnualCostHigh,
                                            costHighConditionUpperBound,
                                            AssetOMAAnnualCostLow,
                                            AssetOMAAnnualCostMedium,
                                            costMediumConditionUpperBound,
                                            conditionLessThanCostHighConditionUpperBound);
            var conditionLowerThanBoundsTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, conditionLowerThanBoundsTimeInvariantData, TimeVariantData);
            
            var expectedResults = Enumerable.Repeat(AssetOMAAnualCostHigh / CommonConstants.MonthsPerYear, ArbitraryMonths).ToArray();
            Assert.That(results, Is.EqualTo(expectedResults).Within(CommonConstants.DoubleDifferenceTolerance));
        }
    }
}