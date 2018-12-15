using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using CL.FormulaHelper.DTOs;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using NUnit.Framework;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.SimpleAssetSafetyConsequenceBase;
using formulaClass = CustomerFormulaCode.SimpleAssetSafetyConsequence;

namespace MeasureFormula.Tests
{
    public class SimpleAssetSafetyConsequenceTest : MeasureFormulaTestsBase
    {
        private formulaClass Formulas;
        
        private baseClass.TimeInvariantInputDTO TimeInvariantData; 
        private baseClass.TimeVariantInputDTO[] TimeVariantData;

        private NumericValueRangeDTO AssetSafetyRiskConsequence;
        private NumericValueRangeDTO FailureSeverity;
        
        [SetUp]
        public void FixtureSetup()
        {
            Formulas = new formulaClass();
            
            AssetSafetyRiskConsequence = fixture.Build<NumericValueRangeDTO>().Create();
            FailureSeverity = fixture.Build<NumericValueRangeDTO>().Create();
            InitializeTimeInvariantTestData(AssetSafetyRiskConsequence, FailureSeverity);
            TimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            TimeVariantData = DataPrep.BuildTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, ArbitraryStartYear);
        }
        
        private void InitializeTimeInvariantTestData(NumericValueRangeDTO safetyRiskConsequence, NumericValueRangeDTO failureSeverityFactor)
        {
            DataPrep.SetConstructorParameter(fixture, "p_AssetSafetyRiskConsequence", safetyRiskConsequence);
            DataPrep.SetConstructorParameter(fixture, "p_FailureSeverity", failureSeverityFactor);
        }
        
        [Test]
        public void GetUnits_NullTests()
        {
            Func<object, object, double?[]> getUnitsCall = (x, y) => Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, (baseClass.TimeInvariantInputDTO) x, (IReadOnlyList<baseClass.TimeVariantInputDTO>) y);
            var nullCheck = new NullablePropertyCheck();
            nullCheck.RunNullTestsIncludingCustomFields(TimeInvariantData, TimeVariantData, getUnitsCall);
        }
        
        [Test]
        public void NullInputs_GetUnits_ReturnsNull()
        {
            InitializeTimeInvariantTestData(null, null);
            var nullInputDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullInputDto, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void NullFailureSeverity_GetUnits_ReturnsCalculations()
        {
            InitializeTimeInvariantTestData(AssetSafetyRiskConsequence, null);
            var nullFailureSeverityDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullFailureSeverityDto, TimeVariantData);
            
            var expectedResult = Enumerable.Repeat(nullFailureSeverityDto.AssetSafetyRiskConsequence.AvgValue, ArbitraryMonths).ToArray();
            Assert.That(results, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void NullSafetyRisk_GetUnits_ReturnsCalculations()
        {
            InitializeTimeInvariantTestData(null, FailureSeverity);
            var nullSafetyRiskDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullSafetyRiskDto, TimeVariantData);
            
            var expectedResult = Enumerable.Repeat(nullSafetyRiskDto.FailureSeverity.AvgValue, ArbitraryMonths).ToArray();
            Assert.That(results, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
    }
}