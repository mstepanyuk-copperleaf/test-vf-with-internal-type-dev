using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using NUnit.Framework;
using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.BenefitLikelihoodBase;
using formulaClass = CustomerFormulaCode.BenefitLikelihood;
 
namespace MeasureFormula.Tests
{
    [TestFixture]
    public class BenefitLikelihoodTests : MeasureFormulaTestsBase 
    {        
        private readonly formulaClass _formulas = new formulaClass();
        private baseClass.TimeInvariantInputDTO _timeInvariantInput;
        private IReadOnlyList<baseClass.TimeVariantInputDTO> _timeVariantInput;
        private const int BenefitLikelihood = 50;
        
        [SetUp]
        public void FixtureSetup()     
        {
            DataPrep.SetConstructorParameter(fixture, "p_BenefitProbability", BenefitLikelihood);
            
            _timeInvariantInput = fixture.Create<baseClass.TimeInvariantInputDTO>();
            _timeVariantInput = new[] {fixture.Create<baseClass.TimeVariantInputDTO>()};
        }
    
        [Test]
        public void NullTests()
        {
            Func<object, object, double?[]> getLikelihoodCall = 
                (x, y) => _formulas.GetLikelihoodValues(ArbitraryStartYear, ArbitraryMonths, 
                (baseClass.TimeInvariantInputDTO) x, (IReadOnlyList<baseClass.TimeVariantInputDTO>) y);
    
            var nullCheck = new NullablePropertyCheck();
            Assert.DoesNotThrow(() =>
            {
                nullCheck.RunNullTestsIncludingCustomFields(_timeInvariantInput, _timeVariantInput, getLikelihoodCall);
            });
        }
        
        [Test]
        public void GetLikelihoodValues_WhenPassedInValidInputs_ComputesCorrectly()
        {
            var output = _formulas.GetLikelihoodValues(ArbitraryStartYear, ArbitraryMonths, _timeInvariantInput, 
                _timeVariantInput);

            const double expectedOutputMonthlyValue =
                BenefitLikelihood / CommonConstants.PercentPerProbabilityOne / CommonConstants.MonthsPerYear;
            var expectedOutput = Enumerable.Repeat(expectedOutputMonthlyValue, ArbitraryMonths).ToArray();
            
            Assert.That(output, Is.EqualTo(expectedOutput).Within(CommonConstants.DoubleDifferenceTolerance));
        }
    }
}