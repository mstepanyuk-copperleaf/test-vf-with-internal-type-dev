using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using CL.FormulaHelper.DTOs;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using NUnit.Framework;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.SimpleAssetFinancialConsequenceBase;
using formulaClass = CustomerFormulaCode.SimpleAssetFinancialConsequence;

namespace MeasureFormula.Tests
{
    public class SimpleAssetFinancialConsequenceTest : MeasureFormulaTestsBase
    {
        private formulaClass Formulas;
        
        private baseClass.TimeInvariantInputDTO TimeInvariantData;
        private baseClass.TimeVariantInputDTO[] TimeVariantData;
        private NumericValueRangeDTO AssetFinancialRiskConsequenceDto;
        private NumericValueRangeDTO FailureSeverityDto;
        
        [SetUp]
        public void FixtureSetup()
        {
            Formulas = new formulaClass();
            
            AssetFinancialRiskConsequenceDto = fixture.Build<NumericValueRangeDTO>().Create();
            FailureSeverityDto = fixture.Build<NumericValueRangeDTO>().Create();
            InitializeTimeInvariantTestData(AssetFinancialRiskConsequenceDto, FailureSeverityDto);
            TimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            TimeVariantData = DataPrep.BuildTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, ArbitraryStartYear);
        }
        
        private void InitializeTimeInvariantTestData(NumericValueRangeDTO assetFinancialRiskConsequence, NumericValueRangeDTO failureSeverity)
        {
            DataPrep.SetConstructorParameter(fixture, "p_AssetFinancialRiskConsequence", assetFinancialRiskConsequence);
            DataPrep.SetConstructorParameter(fixture, "p_FailureSeverity", failureSeverity);
        }
        
        [Test]
        public void GetUnits_NullTests()
        {
            Func<object, object, double?[]> getUnitsCall = (x, y) => Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, (baseClass.TimeInvariantInputDTO) x, (IReadOnlyList<baseClass.TimeVariantInputDTO>) y);
            var nullCheck = new NullablePropertyCheck();
            nullCheck.RunNullTestsIncludingCustomFields(TimeInvariantData, TimeVariantData, getUnitsCall);
        }

        [Test]
        public void AssetFinancialRiskConsequence_GetUnits_ReturnsCalculations()
        {
            InitializeTimeInvariantTestData(null, FailureSeverityDto);
            var nullAssetFinancialRiskConsequenceTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullAssetFinancialRiskConsequenceTimeInvariantData, TimeVariantData);
            
            var expectedResults = Enumerable.Repeat(FailureSeverityDto.AvgValue, ArbitraryMonths).ToArray();
            Assert.That(expectedResults, Is.EqualTo(results).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void FailureSeverityNull_GetUnits_ReturnsCalculations()
        {
            InitializeTimeInvariantTestData(AssetFinancialRiskConsequenceDto, null);
            var nullAssetFailureSeverityTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullAssetFailureSeverityTimeInvariantData, TimeVariantData);
            
            var expectedResults = Enumerable.Repeat(AssetFinancialRiskConsequenceDto.AvgValue, ArbitraryMonths).ToArray();
            Assert.That(expectedResults, Is.EqualTo(results).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void BothAssetRiskConsequenceAndFailureSeverityNull_GetUnits_ReturnsNull()
        {
            InitializeTimeInvariantTestData(null, null);
            var nullAssetFailureSeverityTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullAssetFailureSeverityTimeInvariantData, TimeVariantData);
            Assert.That(results, Is.Null);
        }
    }
}