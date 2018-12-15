using System;
using System.Collections.Generic;
using AutoFixture;
using CL.FormulaHelper;
using NUnit.Framework;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.BusinessContinuityConsequenceBase;
using formulaClass = CustomerFormulaCode.BusinessContinuityConsequence;

namespace MeasureFormula.Tests
{
    [TestFixture]
    public class BusinessContinuityConsequenceTests : MeasureFormulaTestsBase
    {
        private static readonly formulaClass Formulas = new formulaClass();
        private baseClass.TimeVariantInputDTO[] TimeVariantDTO;

        [SetUp]
        public void TestSetup()
        {
            FormulaBase.FiscalYearEndMonth = 6;
            TimeVariantDTO = fixture.Create<baseClass.TimeVariantInputDTO[]>();
        }

        [Test]
        public void GetUnits_WhenEmployeeProductivityIsNull_ReturnsNull()
        {
            var nullProductivity = fixture.Create<baseClass.TimeInvariantInputDTO>();
            TestHelpers.DataPrep.UpdateDto(nullProductivity, "SystemEmployeeProductivityValue", null);
            
            var result = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullProductivity, TimeVariantDTO);
            
            Assert.That(result, Is.Null);
        }

        [Test]
        public void RunNullTests()
        {
            var nullCheck=new TestHelpers.NullablePropertyCheck();

            var timeInvariantDTO = fixture.Create<baseClass.TimeInvariantInputDTO>();
            Func< object , object, double? []> getUnitsCall =
                (x, y) => Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, (baseClass.TimeInvariantInputDTO) x, (IReadOnlyList <baseClass.TimeVariantInputDTO>) y);

            nullCheck.RunNullTestsExcludingCustomFields(timeInvariantDTO, TimeVariantDTO, getUnitsCall);
        }
    }
    

}
