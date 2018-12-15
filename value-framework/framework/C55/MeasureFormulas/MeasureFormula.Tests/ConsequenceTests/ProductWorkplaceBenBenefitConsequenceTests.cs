using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using CL.FormulaHelper.DTOs;
using MeasureFormula.Common_Code;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using NUnit.Framework;
using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.ProductWorkplaceBenBenefitConsequenceBase;
using formulaClass = CustomerFormulaCode.ProductWorkplaceBenBenefitConsequence;
 
namespace MeasureFormula.Tests
{
    [TestFixture]
    public class ProductWorkplaceBenBenefitConsequenceTests : MeasureFormulaTestsBase
    {      
        private readonly formulaClass _formulas = new formulaClass();
        private baseClass.TimeInvariantInputDTO _timeInvariantInput;
        private IReadOnlyList<baseClass.TimeVariantInputDTO> _timeVariantInput;
        private const double NumCandidatesAttracted = 50d;
        private const double NumEmployeesAffected = 100d;
        private const double NumEmployeesAtRiskOfLeaving = 20d;
        private const double WorkplaceImpactOnAttractivenessValue = 0.8d;
        private const double WorkplaceImpactOnProductivityValue = 0.8d;
        
        [SetUp]
        public void FixtureSetup()     
        {
            DataPrep.SetConstructorParameter(fixture, "p_NumberOfCandidatesAttracted", NumCandidatesAttracted);
            DataPrep.SetConstructorParameter(fixture, "p_NumberOfEmployeesAffected", NumEmployeesAffected);
            DataPrep.SetConstructorParameter(fixture, "p_NumberOfEmployeesAtRiskOfLeaving", NumEmployeesAtRiskOfLeaving);
            var attractivenessDropdown = new CustomFieldListItemDTO {Value = WorkplaceImpactOnAttractivenessValue};
            DataPrep.SetConstructorParameter(fixture, "p_WorkplaceImpactOnAttractiveness", attractivenessDropdown);
            var productivityDropdown = new CustomFieldListItemDTO {Value = WorkplaceImpactOnProductivityValue};
            DataPrep.SetConstructorParameter(fixture, "p_WorkplaceImpactOnProductivity", productivityDropdown);
            
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

            const double expectedOutputMonthlyValue =
                NumCandidatesAttracted * WorkplaceImpactOnAttractivenessValue * CoreConstants.ValuePerCandidateAttracted
                + NumEmployeesAffected * WorkplaceImpactOnProductivityValue * CoreConstants.EmployeeCostPerYear
                + NumEmployeesAtRiskOfLeaving * WorkplaceImpactOnProductivityValue * CoreConstants.EmployeeCostToReplace * 0.1d;
            var expectedOutput = Enumerable.Repeat(expectedOutputMonthlyValue, ArbitraryMonths).ToArray();
            
            Assert.That(output, Is.EqualTo(expectedOutput).Within(CommonConstants.DoubleDifferenceTolerance));
        }
    }
}