using System;
using System.Linq;
using AutoFixture;
using CL.FormulaHelper;
using NUnit.Framework;
using CL.FormulaHelper.DTOs;
using CustomerFormulaCode;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.Common_Code;

namespace MeasureFormula.Tests
{
    [TestFixture]
    public class AdditionalCostsOMAConsequenceTests
    {
		private static readonly Fixture fixture = new Fixture();

        private readonly double operationalLabourRate = fixture.Create<double>();
        private readonly long additionalCosts = fixture.Create<long>();
        private readonly double additionalHours = fixture.Create<double>();

        [Test]
        public void WhenAccountTypeIsNotOMAReturnsNull()
        {
            var arbitraryStartFiscalYear = 2060 + fixture.Create<int>() % 40;
            var arbitraryMonths = fixture.Create<int>() % 100;

            var notOMAAccount = fixture.Create<CustomFieldListItemDTO>();
            notOMAAccount.Value = CoreConstants.CapitalAcctID;
            ParameterBuilder.SetConstructorParameter(fixture, Parameter.AccountType, notOMAAccount);
            var timeInvariantDataWithNonOMAAccountType = fixture.Create<AdditionalCostsOMAConsequenceBase.TimeInvariantInputDTO>();

            var timeVariantData = fixture.CreateMany<AdditionalCostsOMAConsequenceBase.TimeVariantInputDTO>().ToArray();

            var results = new AdditionalCostsOMAConsequence().GetUnits(arbitraryStartFiscalYear,
                arbitraryMonths,
                timeInvariantDataWithNonOMAAccountType,
                timeVariantData);
            Assert.That(results, Is.Null);
        }

        [Test]
        public void ValuesInTimeVariantDataAreCorrectlyHandedToFormula()
        {
            FormulaBase.FiscalYearEndMonth = 5;
            var arbitraryStartFiscalYear = 2020 + fixture.Create<int>() % 40;

            var monthOffsetFromFiscalYearEnd = 3;
            var periods = TestHelpers.DataPrep.BuildContiguousTimePeriodsStartYearAndOffset(fixture, 3, arbitraryStartFiscalYear, monthOffsetFromFiscalYearEnd);

            var GeneratePositiveDouble = new Func<double>(() => Math.Abs(fixture.Create<double>()));
            var costs = periods.Select(x => (long) GeneratePositiveDouble()).ToArray();
            var hours = periods.Select(x => GeneratePositiveDouble()).ToArray();
            var multiPeriodTimeVariantData = periods.Select(
                (x, i) => new AdditionalCostsOMAConsequenceBase.TimeVariantInputDTO(costs[i], hours[i], x)).ToArray();

            var numMonthsToOutput = 1 + monthOffsetFromFiscalYearEnd + periods.Select(x => x.DurationInMonths ?? 1).Sum();
            
            var omaAccount = fixture.Create<CustomFieldListItemDTO>();
            omaAccount.Value = CoreConstants.OMAAcctID;
            ParameterBuilder.SetConstructorParameter(fixture, Parameter.AccountType, omaAccount);
            var timeInvariantDataWithOmaAccountType = fixture.Create<AdditionalCostsOMAConsequenceBase.TimeInvariantInputDTO>();

            var results = new AdditionalCostsOMAConsequence().GetUnits(arbitraryStartFiscalYear,
                numMonthsToOutput,
                timeInvariantDataWithOmaAccountType,
                multiPeriodTimeVariantData);

            Assert.That(results, Is.Not.Null);

            var indexFirstNonNullValue = monthOffsetFromFiscalYearEnd;
            Assert.That(results[indexFirstNonNullValue - 1], Is.Null);

            Assert.That(results[indexFirstNonNullValue],
                Is.EqualTo(
                        new OperationalExpenseCalculator(CoreConstants.OPEXLabourHour).
                            AnnualAdditionalCostsFor(costs[0], hours[0])).
                    Within(TestHelpers.PointEqualityComparer.DoubleComparisonTolerance));

            Assert.That(results.Last(),
                Is.EqualTo(new OperationalExpenseCalculator(CoreConstants.OPEXLabourHour).
                        AnnualAdditionalCostsFor(costs.Last(), hours.Last())).
                    Within(TestHelpers.PointEqualityComparer.DoubleComparisonTolerance));
        }


        [Test]
        public void AllParametersUsedTest()
        {         
            var neitherMultiplicativeNorAdditiveIdentity = 2;

            var operationExpenseCalculator = new OperationalExpenseCalculator(operationalLabourRate);
            var differentRateCostsCalculator =
                new OperationalExpenseCalculator(operationalLabourRate + neitherMultiplicativeNorAdditiveIdentity);

            Assert.That(operationExpenseCalculator.AnnualAdditionalCostsFor(additionalCosts, additionalHours),
				Is.Not.EqualTo(differentRateCostsCalculator.AnnualAdditionalCostsFor(additionalCosts, additionalHours)));

			Assert.That(operationExpenseCalculator.AnnualAdditionalCostsFor(additionalCosts, additionalHours),
			    Is.Not.EqualTo(operationExpenseCalculator.AnnualAdditionalCostsFor(additionalCosts + neitherMultiplicativeNorAdditiveIdentity, additionalHours)));

            Assert.That(operationExpenseCalculator.AnnualAdditionalCostsFor(additionalCosts, additionalHours),
                Is.Not.EqualTo(operationExpenseCalculator.AnnualAdditionalCostsFor(additionalCosts, additionalHours + neitherMultiplicativeNorAdditiveIdentity)));
        }

		[Test]
        public void CanHandleZerosTest()
        {
            Assert.That(new OperationalExpenseCalculator(0).AnnualAdditionalCostsFor(0, 0.0), Is.EqualTo(0.0));
        }

        [Test]
        public void ProducesNonNegativeValuesTest()
        {
            var positiveOperationalLabourRate = Math.Abs(fixture.Create<double>());
            var positiveAdditionalCosts = Math.Abs(fixture.Create<long>());
            var positiveAdditionalHours = Math.Abs(fixture.Create<double>());

            var operationExpenseCalculator = new OperationalExpenseCalculator(positiveOperationalLabourRate);

            Assert.That(operationExpenseCalculator.AnnualAdditionalCostsFor(positiveAdditionalCosts, positiveAdditionalHours),
                Is.AtLeast(0.0));
        }

        [Test]
        public void ProducesCorrectNumberTest()
        {
			var operationExpenseCalculator = new OperationalExpenseCalculator(operationalLabourRate);

            var expectedValue = additionalCosts + additionalHours * operationalLabourRate;

            Assert.That(operationExpenseCalculator.AnnualAdditionalCostsFor(additionalCosts, additionalHours),
                Is.EqualTo(expectedValue).Within(TestHelpers.PointEqualityComparer.DoubleComparisonTolerance));
        }

        [Test]
        public void ValidDataConvertsToZynosCorrectly()
        {         
            MeasureFormulaInvoker.RunGetZynosTest(typeof(BusinessContinuityConsequence), 
                                                  DataPrep.ZynosTestType.DollarToZynosConversion, 
                                                  CustomerConstants.DollarToZynoConversionFactor,
                                                  Assert.AreEqual);
        }
    }
}
