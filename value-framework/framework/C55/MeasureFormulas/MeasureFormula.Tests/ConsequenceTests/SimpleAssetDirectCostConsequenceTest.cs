using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using NUnit.Framework;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.SimpleAssetDirectCostConsequenceBase;
using formulaClass = CustomerFormulaCode.SimpleAssetDirectCostConsequence;

namespace MeasureFormula.Tests
{
    public class SimpleAssetDirectCostConsequenceTest : MeasureFormulaTestsBase
    {
        private formulaClass Formulas;
        
        private decimal AssetReplacementCost;
        private double AssetTypeCostVariationFactor;
        private double AssetExtraDamageFactor;
        
        private baseClass.TimeInvariantInputDTO TimeInvariantData; 
        private baseClass.TimeVariantInputDTO[] TimeVariantData;
        
        [SetUp]
        public void FixtureSetup()
        {
            Formulas = new formulaClass();
            
            AssetReplacementCost = fixture.Create<decimal>() % 10000;
            AssetTypeCostVariationFactor = fixture.Create<double>() % 100 / 100;
            AssetExtraDamageFactor = fixture.Create<double>() % 100 / 100;
            InitializeTimeInvariantTestData(AssetReplacementCost, AssetTypeCostVariationFactor, AssetExtraDamageFactor);
            TimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            TimeVariantData = DataPrep.BuildTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, ArbitraryStartYear);
        }
        
        private void InitializeTimeInvariantTestData(decimal? replacementCost, double? costVariationFactor, double? extraDamageFactor)
        {
            DataPrep.SetConstructorParameter(fixture, "p_AssetReplacementCost", replacementCost);
            DataPrep.SetConstructorParameter(fixture, "p_AssetTypeCostVariationFactor", costVariationFactor);
            DataPrep.SetConstructorParameter(fixture, "p_AssetTypeExtraDamageFactor", extraDamageFactor);
        }
        
        [Test]
        public void GetUnits_NullTests()
        {
            Func<object, object, double?[]> getUnitsCall = (x, y) => Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, (baseClass.TimeInvariantInputDTO) x, (IReadOnlyList<baseClass.TimeVariantInputDTO>) y);
            var nullCheck = new NullablePropertyCheck();
            Assert.DoesNotThrow(() =>
            {
                nullCheck.RunNullTestsIncludingCustomFields(TimeInvariantData, TimeVariantData, getUnitsCall);
            });
        }

        [Test]
        public void AssetReplacementCostNull_GetUnits_ReturnsNull()
        {
            InitializeTimeInvariantTestData(null, AssetTypeCostVariationFactor, AssetExtraDamageFactor);
            var nullAssetCostTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullAssetCostTimeInvariantData, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void AssetVariationFactorNull_GetUnits_ReturnsCalculations()
        {
            InitializeTimeInvariantTestData(AssetReplacementCost, null, AssetExtraDamageFactor);
            var nullAssetCostTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullAssetCostTimeInvariantData, TimeVariantData);
            
            double calculatedValue = (double) AssetReplacementCost * (1 + AssetExtraDamageFactor);
            var expectedResults = Enumerable.Repeat(calculatedValue, ArbitraryMonths).ToArray();
            Assert.That(expectedResults, Is.EqualTo(results).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void AssetExtraDamageFactorNull_GetUnits_ReturnsCalculations()
        {
            InitializeTimeInvariantTestData(AssetReplacementCost, AssetTypeCostVariationFactor, null);
            var nullAssetCostTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullAssetCostTimeInvariantData, TimeVariantData);
            
            var calculatedValue = (double) AssetReplacementCost * (1 + AssetTypeCostVariationFactor);
            var expectedResults = Enumerable.Repeat(calculatedValue, ArbitraryMonths).ToArray();
            Assert.That(expectedResults, Is.EqualTo(results).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void CorrectValues_GetUnits_ReturnsCalculations()
        {
            InitializeTimeInvariantTestData(AssetReplacementCost, AssetTypeCostVariationFactor, AssetExtraDamageFactor);
            var nullAssetCostTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullAssetCostTimeInvariantData, TimeVariantData);
            
            var calculatedValue = (double) AssetReplacementCost * (1 + AssetTypeCostVariationFactor) * (1 + AssetExtraDamageFactor);
            var expectedResults = Enumerable.Repeat(calculatedValue, ArbitraryMonths).ToArray();
            Assert.That(expectedResults, Is.EqualTo(results).Within(CommonConstants.DoubleDifferenceTolerance));
        }
    }
}