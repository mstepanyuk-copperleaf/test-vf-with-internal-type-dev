using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using MeasureFormula.Common_Code;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using NUnit.Framework;
using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.CustomerServiceConsequenceBase;
using formulaClass = CustomerFormulaCode.CustomerServiceConsequence;
 
namespace MeasureFormula.Tests
{
    [TestFixture]
    public class CustomerServiceConsequenceTests : MeasureFormulaTestsBase
    {      
        private readonly formulaClass _formulas = new formulaClass();
        private baseClass.TimeInvariantInputDTO _timeInvariantInput;
        private IReadOnlyList<baseClass.TimeVariantInputDTO> _timeVariantInput;
        private const int AverageTotalHandlingTimePerCall = 20;
        private const int CallsPerHour = 2;
        private const int CsrCost = 30;
        private const int CustTimeCost = 100;
        private const int AgentTimeSaved = 10;
        private const int LowEffortResolution = 5;
        private const int SelfServiceResolution = 50;
        
        [SetUp]
        public void FixtureSetup()     
        {
            DataPrep.SetConstructorParameter(fixture, "p_SystemAverageTotalHandlingTimePerCall", AverageTotalHandlingTimePerCall);
            DataPrep.SetConstructorParameter(fixture, "p_SystemCallsPerYear", CallsPerHour);
            DataPrep.SetConstructorParameter(fixture, "p_SystemCSRCost", CsrCost);
            DataPrep.SetConstructorParameter(fixture, "p_SystemCustTimeCost", CustTimeCost);
            DataPrep.SetConstructorParameter(fixture, "p_AgentTimeSaved", AgentTimeSaved);
            DataPrep.SetConstructorParameter(fixture, "p_LowEffortResolutions", LowEffortResolution);
            DataPrep.SetConstructorParameter(fixture, "p_SelfServiceResolutions", SelfServiceResolution);
            
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

            const double expectedOutputMonthlyValue = AgentTimeSaved * CoreConstants.AgentTimeCostPerHour
                                                      + LowEffortResolution * CoreConstants.SavingsPerLowEffortResolution
                                                      + SelfServiceResolution * CoreConstants.SavingsPerSelfServiceResoultion;
            var expectedOutput = Enumerable.Repeat(expectedOutputMonthlyValue, ArbitraryMonths).ToArray();
            
            Assert.That(output, Is.EqualTo(expectedOutput).Within(CommonConstants.DoubleDifferenceTolerance));
        }
    }
}