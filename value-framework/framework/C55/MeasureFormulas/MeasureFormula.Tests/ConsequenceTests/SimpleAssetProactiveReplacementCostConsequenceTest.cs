using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using NUnit.Framework;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.SimpleAssetProactiveReplacementCostConsequenceBase;
using formulaClass = CustomerFormulaCode.SimpleAssetProactiveReplacementCostConsequence;

namespace MeasureFormula.Tests
{
    public class SimpleAssetProactiveReplacementCostConsequenceTest : MeasureFormulaTestsBase
    {
        private formulaClass Formulas;
        
        private baseClass.TimeInvariantInputDTO TimeInvariantData; 
        private baseClass.TimeVariantInputDTO[] TimeVariantData;
        private decimal? AssetReplacementCost;
        
        [SetUp]
        public void FixtureSetup()
        {
            Formulas = new formulaClass();
            
            AssetReplacementCost = fixture.Build<decimal?>().Create() % 10000;
            DataPrep.SetConstructorParameter(fixture, "p_AssetReplacementCost", AssetReplacementCost);
            TimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            TimeVariantData = DataPrep.BuildTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, ArbitraryStartYear);
        }
        
        [Test]
        public void GetUnits_NullTests()
        {
            Func<object, object, double?[]> getUnitsCall = (x, y) => Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, (baseClass.TimeInvariantInputDTO) x, (IReadOnlyList<baseClass.TimeVariantInputDTO>) y);
            var nullCheck = new NullablePropertyCheck();
            nullCheck.RunNullTestsIncludingCustomFields(TimeInvariantData, TimeVariantData, getUnitsCall);
        }
        
        [Test]
        public void GetUnits_NullAssetReplacementCost_ReturnsNull()
        {
            var nullAssetReplacementCostTimeInvariantData = new baseClass.TimeInvariantInputDTO(null);
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullAssetReplacementCostTimeInvariantData, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void GetUnits_CalculationCheck_ReturnsCalculations()
        {
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, TimeInvariantData, TimeVariantData);
            var expectedResult = Enumerable.Repeat( (double) (TimeInvariantData.AssetReplacementCost ?? 0), ArbitraryMonths);
            Assert.That(results, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
    }
}