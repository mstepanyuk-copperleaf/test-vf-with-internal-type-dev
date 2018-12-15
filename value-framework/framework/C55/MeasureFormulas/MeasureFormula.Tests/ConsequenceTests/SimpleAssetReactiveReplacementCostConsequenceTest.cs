using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using NUnit.Framework;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.SimpleAssetReactiveReplacementCostConsequenceBase;
using formulaClass = CustomerFormulaCode.SimpleAssetReactiveReplacementCostConsequence;

namespace MeasureFormula.Tests
{
    public class SimpleAssetReactiveReplacementCostConsequenceTest : MeasureFormulaTestsBase
    {
        private formulaClass Formulas;
        private baseClass.TimeInvariantInputDTO TimeInvariantData; 
        private baseClass.TimeVariantInputDTO[] TimeVariantData;

        private decimal? AssetReplacementCost;
        private double? AssetTypeExtraDamageFactor;
        
        [SetUp]
        public void FixtureSetup()
        {
            Formulas = new formulaClass();
            
            AssetReplacementCost = fixture.Build<decimal?>().Create() % 10000;
            AssetTypeExtraDamageFactor = fixture.Build<double?>().Create() % 100 / 100;
            InitializeTimeInvariantTestData(AssetReplacementCost, AssetTypeExtraDamageFactor);
            TimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            TimeVariantData = DataPrep.BuildTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, ArbitraryStartYear);
        }
        
        private void InitializeTimeInvariantTestData(Decimal? replacementCost, double? typeExtraDamageFactor)
        {
            DataPrep.SetConstructorParameter(fixture, "p_AssetReplacementCost", replacementCost);
            DataPrep.SetConstructorParameter(fixture, "p_AssetTypeExtraDamageFactor", typeExtraDamageFactor);
        }
        
        [Test]
        public void GetUnits_NullTests()
        {
            Func<object, object, double?[]> getUnitsCall = (x, y) => Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, (baseClass.TimeInvariantInputDTO) x, (IReadOnlyList<baseClass.TimeVariantInputDTO>) y);
            var nullCheck = new NullablePropertyCheck();
            nullCheck.RunNullTestsIncludingCustomFields(TimeInvariantData, TimeVariantData, getUnitsCall);
        }
        
        [Test]
        public void NullAssetReplacementCost_GetUnits_ReturnsNull()
        {
            InitializeTimeInvariantTestData(null, AssetTypeExtraDamageFactor);
            var nullAssetReplacementCostTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullAssetReplacementCostTimeInvariantData, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void NullExtraDamageFactor_GetUnits_ReturnsCalculations()
        {
            InitializeTimeInvariantTestData(AssetReplacementCost, null);
            var nullExtraDamageTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullExtraDamageTimeInvariantData, TimeVariantData);
            
            var expectedResult = Enumerable.Repeat(nullExtraDamageTimeInvariantData.AssetReplacementCost, ArbitraryMonths);
            Assert.That(results, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void NullInputs_GetUnits_ReturnsNull()
        {
            InitializeTimeInvariantTestData(null, null);
            var nullInputsTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullInputsTimeInvariantData, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void CalculationCheck_GetUnits_ReturnsCalculations()
        {
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, TimeInvariantData, TimeVariantData);
            
            var expectedResult = Enumerable.Repeat( (double) (TimeInvariantData.AssetReplacementCost ?? 0) * (1 + (TimeInvariantData.AssetTypeExtraDamageFactor ?? 0)), ArbitraryMonths);
            Assert.That(results, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
    }
}