using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using NUnit.Framework;
using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.EmployeeProductivityBenefitConsequenceBase;
using formulaClass = CustomerFormulaCode.EmployeeProductivityBenefitConsequence;
 
namespace MeasureFormula.Tests
{
    [TestFixture]
    public class EmployeeProductivityBenefitConsequenceTests : MeasureFormulaTestsBase
    {      
        private readonly formulaClass _formulas = new formulaClass();
        private baseClass.TimeInvariantInputDTO _timeInvariantInput;
        private IReadOnlyList<baseClass.TimeVariantInputDTO> _timeVariantInput;
        private const double ManagementEmployeeCostPerHour = 100;
        private const double OfficeEmployeeCostPerHour = 70;
        private const double FieldEmployeeCostPerHour = 70;
        private const double ProbabilityOfRepurposing = 0.2;
        private const int ManagementHoursAdditional = 10;
        private const int ManagementHoursSaved = 50;
        private const int NumManagementEmployees = 40;
        private const int OfficeHoursAdditional = 5;
        private const int OfficeHoursSaved = 50;
        private const int NumOfficeEmployees = 30;
        private const int FieldHoursAdditional = 5;
        private const int FieldHoursSaved = 50;
        private const int NumFieldEmployees = 30;
        
        [SetUp]
        public void FixtureSetup()     
        {
            DataPrep.SetConstructorParameter(fixture, "p_SystemManagerCostPerHour", ManagementEmployeeCostPerHour);
            DataPrep.SetConstructorParameter(fixture, "p_SystemOfficeEmployeeCostPerHour", OfficeEmployeeCostPerHour);
            DataPrep.SetConstructorParameter(fixture, "p_SystemFieldEmployeeCostPerHour", FieldEmployeeCostPerHour);
            DataPrep.SetConstructorParameter(fixture, "p_SystemProbabilityOfRepurposing", ProbabilityOfRepurposing);
            
            DataPrep.SetConstructorParameter(fixture, "p_ManagerHoursAdditional", ManagementHoursAdditional);
            DataPrep.SetConstructorParameter(fixture, "p_ManagerHoursSaved", ManagementHoursSaved);
            DataPrep.SetConstructorParameter(fixture, "p_NoManagerEmployees", NumManagementEmployees);
            
            DataPrep.SetConstructorParameter(fixture, "p_OfficeHoursAdditional", OfficeHoursAdditional);
            DataPrep.SetConstructorParameter(fixture, "p_OfficeHoursSaved", OfficeHoursSaved);
            DataPrep.SetConstructorParameter(fixture, "p_NoOfficeEmployees", NumOfficeEmployees);
            
            DataPrep.SetConstructorParameter(fixture, "p_FieldHoursAdditional", FieldHoursAdditional);
            DataPrep.SetConstructorParameter(fixture, "p_FieldHoursSaved", FieldHoursSaved);
            DataPrep.SetConstructorParameter(fixture, "p_NoFieldEmployees", NumFieldEmployees);
            
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

            const double expectedOutputMonthlyValue = (NumManagementEmployees * ManagementHoursSaved * ManagementEmployeeCostPerHour
                                                       + NumOfficeEmployees * OfficeHoursSaved * OfficeEmployeeCostPerHour
                                                       + NumFieldEmployees * FieldHoursSaved * FieldEmployeeCostPerHour)
                                                      * ProbabilityOfRepurposing
                                                      - (NumManagementEmployees * ManagementHoursAdditional * ManagementEmployeeCostPerHour
                                                         + NumOfficeEmployees * OfficeHoursAdditional * OfficeEmployeeCostPerHour
                                                         + NumFieldEmployees * FieldHoursAdditional * FieldEmployeeCostPerHour);
            var expectedOutput = Enumerable.Repeat(expectedOutputMonthlyValue, ArbitraryMonths).ToArray();
            
            Assert.That(output, Is.EqualTo(expectedOutput).Within(CommonConstants.DoubleDifferenceTolerance));
        }
    }
}