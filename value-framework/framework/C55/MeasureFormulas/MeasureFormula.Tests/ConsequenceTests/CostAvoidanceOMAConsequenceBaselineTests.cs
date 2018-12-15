using System;
using System.Collections.Generic;
using AutoFixture;
using CL.FormulaHelper.DTOs;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using NUnit.Framework;
using MeasureFormula.Common_Code;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.CostAvoidanceOMAConsequenceBaselineBase;
using formulaClass = CustomerFormulaCode.CostAvoidanceOMAConsequenceBaseline;
namespace MeasureFormula.Tests
{
    [TestFixture]
    public class CostAvoidanceOMAConsequenceBaselineTests : MeasureFormulaTestsBase
    {
        private formulaClass Formulas;

        [SetUp]
        public void FixtureSetup()
        {
            Formulas = new formulaClass();
        }

        [Test]
        public void NullTests()
        {
            Func<object, object, double?[]> getUnitsCall = (x, y) => Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths,
                                                                                       (baseClass.TimeInvariantInputDTO) x, 
                                                                                       (IReadOnlyList<baseClass.TimeVariantInputDTO>) y);

            var correctAccount = fixture.Build<CustomFieldListItemDTO>().With(x=>x.Value, CoreConstants.OMAAcctID).Create();
            DataPrep.SetConstructorParameter(fixture, "p_AccountType", correctAccount);
            
            var nullCheck = new NullablePropertyCheck();
            nullCheck.RunNullTestsIncludingCustomFields(fixture.Create<baseClass.TimeInvariantInputDTO>(), new [] {fixture.Create<baseClass.TimeVariantInputDTO>()}, getUnitsCall);
        }
    }
}
