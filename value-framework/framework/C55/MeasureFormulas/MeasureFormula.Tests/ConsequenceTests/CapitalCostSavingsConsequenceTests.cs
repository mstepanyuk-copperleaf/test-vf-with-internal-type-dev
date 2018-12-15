using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using CL.FormulaHelper.DTOs;
using MeasureFormula.Common_Code;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using NUnit.Framework;
using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.CapitalCostSavingsConsequenceBase;
using formulaClass = CustomerFormulaCode.CapitalCostSavingsConsequence;
 
namespace MeasureFormula.Tests
{
    [TestFixture]
    public class CapitalCostSavingsConsequenceTests : MeasureFormulaTestsBase
    {      
        private readonly formulaClass _formulas = new formulaClass();
        private baseClass.TimeInvariantInputDTO _timeInvariantInput;
        private IReadOnlyList<baseClass.TimeVariantInputDTO> _timeVariantInput;
        private const int AnnualCapitalValue = 100000;
        private const int AnnualCapitalAdditionalValue = 50000;
        
        [SetUp]
        public void FixtureSetup()     
        {
            var financialBenefitTypeDropdown = new CustomFieldListItemDTO {Value = (int) FinancialBenefitType.CostSavings};
            DataPrep.SetConstructorParameter(fixture, "p_FinancialBenefitType", financialBenefitTypeDropdown);
            DataPrep.SetConstructorParameter(fixture, "p_AnnualCapital", AnnualCapitalValue);
            DataPrep.SetConstructorParameter(fixture, "p_AnnualCapitalAdd", AnnualCapitalAdditionalValue);
            
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

            var expectedOutput = Enumerable.Repeat((double?)(AnnualCapitalValue - AnnualCapitalAdditionalValue), ArbitraryMonths).ToArray();
            
            Assert.That(output, Is.EqualTo(expectedOutput).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void GetUnits_WhenFinancialBenefitTypeIncorrect_ReturnsZeros()
        {
            var financialBenefitTypeDropdown = new CustomFieldListItemDTO {Value = (int) FinancialBenefitType.RevenueIncrease};
            DataPrep.UpdateDto(_timeInvariantInput, "FinancialBenefitType", financialBenefitTypeDropdown);
            
            var output = _formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, _timeInvariantInput, _timeVariantInput);

            var expectedOutput = Enumerable.Repeat(0d, ArbitraryMonths).ToArray();
            
            Assert.That(output, Is.EqualTo(expectedOutput).Within(CommonConstants.DoubleDifferenceTolerance));
        }
    }
}