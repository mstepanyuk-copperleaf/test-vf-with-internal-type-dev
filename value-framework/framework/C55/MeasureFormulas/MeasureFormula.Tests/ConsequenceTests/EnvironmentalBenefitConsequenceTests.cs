using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using NUnit.Framework;
using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.EnvironmentalBenefitConsequenceBase;
using formulaClass = CustomerFormulaCode.EnvironmentalBenefitConsequence;
 
namespace MeasureFormula.Tests
{
    [TestFixture]
    public class EnvironmentalBenefitConsequenceTests : MeasureFormulaTestsBase
    {      
        private readonly formulaClass _formulas = new formulaClass();
        private baseClass.TimeInvariantInputDTO _timeInvariantInput;
        private IReadOnlyList<baseClass.TimeVariantInputDTO> _timeVariantInput;
        private const int CO2Reduction = 50;
        private const int EnergySavings = 100;
        private const double SF6Reduction = 10d;
        private const double EnergySavingsDollarsPerMwh = 30d;
        private const double GHGValue = 20d;
        
        [SetUp]
        public void FixtureSetup()     
        {
            DataPrep.SetConstructorParameter(fixture, "p_CO2Reduction", CO2Reduction);
            DataPrep.SetConstructorParameter(fixture, "p_EnergySavings", EnergySavings);
            DataPrep.SetConstructorParameter(fixture, "p_SF6Reduction", SF6Reduction);
            DataPrep.SetConstructorParameter(fixture, "p_SystemEnergySavingsValueDollarsPerMWh", EnergySavingsDollarsPerMwh);
            DataPrep.SetConstructorParameter(fixture, "p_SystemGHGValue", GHGValue);
            
            _timeInvariantInput = fixture.Create<baseClass.TimeInvariantInputDTO>();
            _timeVariantInput = new[] {fixture.Create<baseClass.TimeVariantInputDTO>()};
        }
    
        [Test]
        public void NullTests()
        {
            Func<object, object, double?[]> getUnitsCall =
                (x, y) => _formulas.GetUnits(ArbitraryStartYear,
                                             ArbitraryMonths,
                                             (baseClass.TimeInvariantInputDTO) x,
                                             (IReadOnlyList<baseClass.TimeVariantInputDTO>) y);
    
            var nullCheck = new NullablePropertyCheck();
            Assert.DoesNotThrow(() =>
            {
                nullCheck.RunNullTestsIncludingCustomFields(_timeInvariantInput, _timeVariantInput, getUnitsCall);
            });
        }
        
        [Test]
        public void GetUnits_WhenPassedInValidInputs_ComputesCorrectly()
        {
            var output = _formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, _timeInvariantInput, _timeVariantInput);

            const double expectedOutputMonthlyValue = CO2Reduction * GHGValue
                                                      + SF6Reduction * GHGValue * CommonConstants.TonnesCO2PerKgSF6
                                                      + EnergySavings * EnergySavingsDollarsPerMwh;
            var expectedOutput = Enumerable.Repeat(expectedOutputMonthlyValue, ArbitraryMonths).ToArray();
            
            Assert.That(output, Is.EqualTo(expectedOutput).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void GetUnits_WhenGhgValueIsNull_ComputesCorrectly()
        {
            DataPrep.SetPropertyToNull(_timeInvariantInput, "SystemGHGValue");
            var output = _formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, _timeInvariantInput, _timeVariantInput);

            const double expectedOutputMonthlyValue = EnergySavings * EnergySavingsDollarsPerMwh;
            var expectedOutput = Enumerable.Repeat(expectedOutputMonthlyValue, ArbitraryMonths).ToArray();
            
            Assert.That(output, Is.EqualTo(expectedOutput).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void GetUnits_WhenEnergyValueIsNull_ComputesCorrectly()
        {
            DataPrep.SetPropertyToNull(_timeInvariantInput, "SystemEnergySavingsValueDollarsPerMWh");
            var output = _formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, _timeInvariantInput, _timeVariantInput);

            const double expectedOutputMonthlyValue = CO2Reduction * GHGValue
                                                      + SF6Reduction * GHGValue * CommonConstants.TonnesCO2PerKgSF6;
            var expectedOutput = Enumerable.Repeat(expectedOutputMonthlyValue, ArbitraryMonths).ToArray();
            
            Assert.That(output, Is.EqualTo(expectedOutput).Within(CommonConstants.DoubleDifferenceTolerance));
        }
    }
}