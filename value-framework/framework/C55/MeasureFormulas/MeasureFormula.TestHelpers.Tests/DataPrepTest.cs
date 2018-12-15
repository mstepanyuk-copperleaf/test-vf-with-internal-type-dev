using System.Linq;
using AutoFixture;
using CL.FormulaHelper;
using MeasureFormula.SharedCode;
using MeasureFormulas.Generated_Formula_Base_Classes;
using NUnit.Framework;

namespace MeasureFormula.TestHelpers.Tests
{
    public class DataPrepTest
    {   
        private readonly Fixture _fixture = new Fixture();

        [Test]
        public void DataPrep_MakeDistributionDTO_ReturnsDistributionDTO()
        {
            var accountName = _fixture.Create<string>();
            var offsetMonth = _fixture.Create<int>();
            var spendAmountArray = _fixture.CreateMany<double>(_fixture.Create<int>()).ToArray();
            var demoDistributionDto = DataPrep.MakeDistributionDTO(accountName, offsetMonth, spendAmountArray);
            var correctIndex = new int[spendAmountArray.Length].Select((val, index) => index+offsetMonth).ToArray();
            
            Assert.That(demoDistributionDto.AccountSpendValues[0].SpendValues.Keys.ToArray(), Is.EqualTo(correctIndex).Within(CommonConstants.DoubleDifferenceTolerance));
            Assert.That(demoDistributionDto.AccountSpendValues[0].SpendValues.Keys.First(), Is.EqualTo(offsetMonth).Within(CommonConstants.DoubleDifferenceTolerance));
            Assert.That(demoDistributionDto.AccountSpendValues[0].AccountTypeName, Is.EqualTo(accountName));
            Assert.That(demoDistributionDto.AccountSpendValues[0].SpendValues.Values.ToArray(), Is.EqualTo(spendAmountArray).Within(CommonConstants.DoubleDifferenceTolerance));
        }

        [Test]
        public void CreateConstantTimeSeries_HasSameValueAtAnyYear()
        {
            var theValue = _fixture.Create<double>();
            var constantTimeSeries = DataPrep.CreateConstantTimeSeries(theValue);

            var valueAtEarlyYear = constantTimeSeries.GetMonthlyValue(fiscalYear: 1900, monthOffset: 0);
            Assert.That(valueAtEarlyYear, Is.EqualTo(theValue).Within(PointEqualityComparer.DoubleComparisonTolerance));
            
            var valueAtLateYear = constantTimeSeries.GetMonthlyValue(fiscalYear: 2300, monthOffset: 0);
            Assert.That(valueAtLateYear, Is.EqualTo(theValue).Within(PointEqualityComparer.DoubleComparisonTolerance));
            
        }

        [Test]
        public void BuildTimeVariantData_StartYearProvided_ReturnsTimeVariantDataBeginningWithin8YearsOfStartYear()
        {
            FormulaBase.FiscalYearEndMonth = 12;
            const int startYear = 2000;
            var timeVariantData =
                DataPrep.BuildTimeVariantData<DummyConsequenceBase.TimeVariantInputDTO>(_fixture, startYear);
            Assert.That(timeVariantData[0].TimePeriod.StartTime.Year - startYear < 8);
        }
    }
}
