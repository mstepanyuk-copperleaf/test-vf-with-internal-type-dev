using System;
using System.Linq;
using AutoFixture;
using CL.FormulaHelper;
using CL.FormulaHelper.DTOs;
using NUnit.Framework;

using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;

namespace MeasureFormula.Tests
{
    public class HelperFunctionsTests : MeasureFormulaTestsBase
    {
        private TimeSeriesDTO CreateRandomTimeSeriesDto(TimeSeriesDTO.TimeSeriesOffsetType timeSeriesType, int numberOfValues)
        {
            var randomGenerator = new Random();
            var valuesArray = new double[numberOfValues].Select(x => randomGenerator.NextDouble()).ToArray();

            var timeseries = new TimeSeriesDTO()
            {
                OffsetType = timeSeriesType,
                BaseYear = ArbitraryStartYear,
                BaseMonth = fixture.Create<int>() % 12 + 1,
                ValuesDoubleArray = valuesArray,
                Name = fixture.Create<string>()
            };
            return timeseries;
        }
        
        [Test]
        public void GetMonthlyValue_ForAbsoluteTimeSeriesWithInvocationYearMatchesCreateYear_ComputesSameValueAsRelativeTimeSeries()
        {
            var startFiscalYear = 1990 + fixture.Create<int>() % 100;
            var numValuesToGenerate = 10;
            var absoluteTimeSeries =
                DataPrep.CreateRandomTimeSeriesDto(TimeSeriesDTO.TimeSeriesOffsetType.AbsoluteFiscalYearly,
                                                                  numValuesToGenerate,
                                                                  startFiscalYear);
            var midPointAbsolute = HelperFunctions.GetMonthlyValue(absoluteTimeSeries, startFiscalYear, numValuesToGenerate / 2);
            var relativeTimeSeries = absoluteTimeSeries;
            relativeTimeSeries.OffsetType = TimeSeriesDTO.TimeSeriesOffsetType.RelativeYearly;
            var midPontRelative = HelperFunctions.GetMonthlyValue(relativeTimeSeries, startFiscalYear, numValuesToGenerate / 2);
            
            Assert.That(midPointAbsolute, Is.EqualTo(midPontRelative));
        }

        [Test]
        public void validStringsToDoubleOthersToZero_HandlesInvalidStrings()
        {
            Assert.That(HelperFunctions.ValidStringsToDoubleOthersToZero(null), Is.EqualTo(0.0).Within(PointEqualityComparer.DoubleComparisonTolerance));
            Assert.That(HelperFunctions.ValidStringsToDoubleOthersToZero(""), Is.EqualTo(0.0).Within(PointEqualityComparer.DoubleComparisonTolerance));
            Assert.That(HelperFunctions.ValidStringsToDoubleOthersToZero("a"), Is.EqualTo(0.0).Within(PointEqualityComparer.DoubleComparisonTolerance));
        }
        
        [Test]
        public void CalendarYearMonthToFiscalDateTest()
        {
            var startFiscalYear = fixture.Create<int>();
            var initialDateTime = new DateTime(startFiscalYear, 1, 1);
            for (var fiscalYearEnd = 1; fiscalYearEnd <= 12; fiscalYearEnd++)
            {
                FormulaBase.FiscalYearEndMonth = fiscalYearEnd;
                for (var monthOffset = 0; monthOffset < 12; monthOffset++)
                {
                    var testDateTime = initialDateTime.AddMonths(monthOffset);
                    int computedFiscalYear;
                    int computedFiscalPeriod;
                    DateHelpers.CalendarYearMonthToFiscalDate(
                        testDateTime,
                        fiscalYearEnd,
                        out computedFiscalYear,
                        out computedFiscalPeriod);

                    var roundtripDateTime = FormulaBase.GetCalendarDateTime(computedFiscalYear, computedFiscalPeriod);

                    Assert.That(roundtripDateTime, Is.EqualTo(testDateTime));
                }
            }

            for (var fiscalPeriod = 1; fiscalPeriod <= 12; fiscalPeriod++)
            {
                for (var fiscalYearEnd = 1; fiscalYearEnd <= 12; fiscalYearEnd++)
                {
                    FormulaBase.FiscalYearEndMonth = fiscalYearEnd;
                    var computedCalendarDateTime = FormulaBase.GetCalendarDateTime(startFiscalYear, fiscalPeriod);

                    int roundTrippedFiscalYear;
                    int roundTrippedFiscalPeriod;
                    DateHelpers.CalendarYearMonthToFiscalDate(
                        computedCalendarDateTime,
                        fiscalYearEnd,
                        out roundTrippedFiscalYear,
                        out roundTrippedFiscalPeriod);

                    Assert.That(roundTrippedFiscalPeriod, Is.EqualTo(fiscalPeriod));
                    Assert.That(roundTrippedFiscalYear, Is.EqualTo( startFiscalYear));
                }
            }
        }
        
        [Test]
        public void CurveIsEmpty_NullXYCurve_ReturnsTrue()
        {
            var result = HelperFunctions.CurveIsEmpty(null);
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void CurveIsEmpty_NoPointsInXYCurve_ReturnsTrue()
        {
            var sampleXYCurve = fixture.Create<XYCurveDTO>();
            sampleXYCurve.Points = null;
            var result = HelperFunctions.CurveIsEmpty(sampleXYCurve);
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void CurveIsEmpty_NoPointsCount_ReturnsTrue()
        {
            var sampleXYCurve = fixture.Create<XYCurveDTO>();
            sampleXYCurve.Points = new CurvePointDTO[0];
            var result = HelperFunctions.CurveIsEmpty(sampleXYCurve);
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void CurveIsEmpty_NormalConditions_ReturnsFalse()
        {
            var sampleXYCurve = fixture.Create<XYCurveDTO>();
            var result = HelperFunctions.CurveIsEmpty(sampleXYCurve);
            Assert.That(result, Is.False);
        }
        
        [Test]
        public void IsRelativeTimeSeries_RelativeMonthlyType_ReturnsTrue()
        {
            var timeSeries = CreateRandomTimeSeriesDto(TimeSeriesDTO.TimeSeriesOffsetType.RelativeMonthly, fixture.Create<int>());
            var result = HelperFunctions.IsRelativeTimeSeries(timeSeries);
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void IsRelativeTimeSeries_RelativeYearlyType_ReturnsTrue()
        {
            var timeSeries = CreateRandomTimeSeriesDto(TimeSeriesDTO.TimeSeriesOffsetType.RelativeYearly, fixture.Create<int>());
            var result = HelperFunctions.IsRelativeTimeSeries(timeSeries);
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void IsRelativeTimeSeries_NotRelativeType_ReturnsFalse()
        {
            var timeSeries = CreateRandomTimeSeriesDto(TimeSeriesDTO.TimeSeriesOffsetType.AbsoluteCalendarMonthly, fixture.Create<int>());
            var result = HelperFunctions.IsRelativeTimeSeries(timeSeries);
            Assert.That(result, Is.False);
        }

        [Test]
        public void GetMonthlyProbabilityValueFromAnnualProbability_NullAnnualProbabilities_ReturnsNull()
        {
            var result = HelperFunctions.GetMonthlyProbabilityValueFromAnnualProbability(null);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void GetMonthlyProbabilityValueFromAnnualProbability_NormalConditions_ReturnsCalc()
        {
            var sampleAnnualProbabilities = new double?[] {0.1, 0.2, 0.3, 0.4, 0.5, null};
            var result = HelperFunctions.GetMonthlyProbabilityValueFromAnnualProbability(sampleAnnualProbabilities);
            
            var expectedResult = new double?[] {0.008741611, 0.01842347, 0.02928553, 0.041675471, 0.056125687, 0};
            Assert.That(result, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void GetMonthlyProbabilityValueFromAnnualProbability_NegativeProbabilityOutOfRange_ReturnsNull()
        {
            var sampleAnnualProbabilities = new double?[] {0.1, -0.2, 0.3, -0.4, 0.5};
            var result = HelperFunctions.GetMonthlyProbabilityValueFromAnnualProbability(sampleAnnualProbabilities);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void GetMonthlyProbabilityValueFromAnnualProbability_PositiveProbabilityOutOfRange_ReturnsNull()
        {
            var sampleAnnualProbabilities = new double?[] {0.1, 1.2, 0.3, 0.4, 0.5};
            var result = HelperFunctions.GetMonthlyProbabilityValueFromAnnualProbability(sampleAnnualProbabilities);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void SimpleGetMonthlyProbabilityValue_NullAnnualFreq_ReturnsNull()
        {
            var result = HelperFunctions.GetMonthlyProbabilityValue(null);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void SimpleGetMonthlyProbabilityValue_NormalConditions_ReturnsCalcs()
        {
            var annualFreq = fixture.Create<double?>();
            var result = HelperFunctions.GetMonthlyProbabilityValue(annualFreq);
            Assert.That(result, Is.EqualTo(annualFreq / CommonConstants.MonthsPerYear).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void GetMonthlyProbabilityValue_NullAnnualFreq_ReturnsNull()
        {
            var result = HelperFunctions.GetMonthlyProbability( null);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void GetMonthlyProbabilityValue_NormalConditions_ReturnsCalc()
        {
            var sampleAnnualProbabilities = new double?[] {null, 120, 240, 360, 480};
            var result = HelperFunctions.GetMonthlyProbability(sampleAnnualProbabilities);

            var expectedResult = new double?[] {null, 10, 20, 30, 40};
            Assert.That(result, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void GetMonthlyProbability_NullAnnualFreq_ReturnsNull()
        {
            var result = HelperFunctions.GetMonthlyProbability(null);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void GetMonthlyProbability_NormalConditions_ReturnsCalc()
        {
            var sampleAnnualProbabilities = new double?[] {null, 120, 240, 360, 480};
            var result = HelperFunctions.GetMonthlyProbability(sampleAnnualProbabilities);

            var expectedResult = new double?[] {null, 10, 20, 30, 40};
            Assert.That(result, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void ScaleValues_NormalConditions_ReturnsCalc()
        {
            var monthlyInputValue = new double?[] {null, 10, 20, 30, 40};
            double? factorValue = 1.5;
            var result = HelperFunctions.ScaleValues(monthlyInputValue.Length, monthlyInputValue, factorValue);

            var multiplyArray = Enumerable.Repeat(factorValue, monthlyInputValue.Length).ToArray();
            var expectedResult = ArrayHelper.MultiplyArrays(monthlyInputValue, multiplyArray);
            Assert.That(result, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void ScaleValues_NoInputValue_ReturnsNull()
        {
            var monthLength = fixture.Create<int>() % 24 + 1;
            double factorValue = 1.5;
            var result = HelperFunctions.ScaleValues(monthLength, null, factorValue);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void ScaleValues_NoFactorValue_ReturnsZeroes()
        {
            var monthlyInputValue = new double?[] {99, 10, 20, 30, 40};
            var result = HelperFunctions.ScaleValues(monthlyInputValue.Length, monthlyInputValue, null);

            var expectedResult = Enumerable.Repeat( (double?) 0, monthlyInputValue.Length).ToArray();
            Assert.That(result, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        
        [Test]
        public void ScaleEntireArrayValues_NormalConditions_ReturnsCalc()
        {
            var monthlyInputValue = new double?[] {null, 10, 20, 30, 40};
            double? factorValue = 1.5;
            var result = HelperFunctions.ScaleEntireArrayValues(monthlyInputValue, factorValue);

            var multiplyArray = Enumerable.Repeat(factorValue, monthlyInputValue.Length).ToArray();
            var expectedResult = ArrayHelper.MultiplyArrays(monthlyInputValue, multiplyArray);
            Assert.That(result, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void ScaleEntireArrayValues_NoInputValue_ReturnsNull()
        {
            double factorValue = 1.5;
            var result = HelperFunctions.ScaleEntireArrayValues(null, factorValue);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void ScaleEntireArrayValues_NoFactorValue_ReturnsZeroes()
        {
            var monthlyInputValue = new double?[] {99, 10, 20, 30, 40};
            var result = HelperFunctions.ScaleEntireArrayValues(monthlyInputValue, null);

            var expectedResult = Enumerable.Repeat( (double?) 0, monthlyInputValue.Length).ToArray();
            Assert.That(result, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        
        [Test]
        public void IsFirstReplacement_MatchingCondition_ReturnsFalse()
        {
            var monthlyConditions = new double?[] {null, 8.2, 10, 9.8, 10, 2.5, 6.4, 8.7};
            double sampleCondition = 10;
            
            var result = HelperFunctions.IsFirstReplacement(monthlyConditions, sampleCondition);
            Assert.That(result, Is.False);
        }
        
        [Test]
        public void IsFirstReplacement_NoMatchingCondition_ReturnsTrue()
        {
            var monthlyConditions = new double?[] {null, 8.2, null, 9.8, 6.1, 2.5, 6.4, 8.7};
            double sampleCondition = 10;
            
            var result = HelperFunctions.IsFirstReplacement(monthlyConditions, sampleCondition);
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void IsFirstReplacement_NullMonthlyCondition_ReturnsTrue()
        {
            double sampleCondition = 10;
            var result = HelperFunctions.IsFirstReplacement(null, sampleCondition);
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void GetCustomFieldValue_NormalCondition_ReturnsValue()
        {
            var customeFieldItem = fixture.Create<CustomFieldListItemDTO>();
            var result = HelperFunctions.GetCustomFieldValue(customeFieldItem);
            Assert.That(result, Is.EqualTo(customeFieldItem.Value).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void GetCustomFieldValue_NullCustomField_ReturnsNull()
        {
            var result = HelperFunctions.GetCustomFieldValue(null);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void GetCustomFieldValueAsInt_NormalCondition_ReturnsValue()
        {
            var customeFieldItem = fixture.Create<CustomFieldListItemDTO>();
            var result = HelperFunctions.GetCustomFieldValueAsInt(customeFieldItem);
            Assert.That(result, Is.EqualTo(customeFieldItem.ValueAsInteger).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void GetCustomFieldValueAsInt_NullCustomField_ReturnsNull()
        {
            var result = HelperFunctions.GetCustomFieldValueAsInt(null);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void CalculateMeasureValue_NormalCondition_ReturnsCalc()
        {
            var monthlyConsequences = new double?[] {null,   9, null, 10,     7};
            var likelihoodArray     = new double?[] {null, 0.1,     1,  1, null};
            var numberOfMonths = monthlyConsequences.Length;
            var result = HelperFunctions.CalculateMeasureValue(numberOfMonths, monthlyConsequences, likelihoodArray);

            var expectedResult = new double?[] {null, 0.9, null, 10, null};
            Assert.That(result, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void CalculateMeasureValue_NullMonthlyConsequences_ReturnsNull()
        {
            var likelihoodArray = new double?[] {null, 9.1, 4, null, 10, 7.8, 8.9, 9.1, 4};
            var numberOfMonths = likelihoodArray.Length;
            var result = HelperFunctions.CalculateMeasureValue(numberOfMonths, null, likelihoodArray);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void CalculateMeasureValue_NullLikelyhood_ReturnsNull()
        {
            var monthlyConsequences = new double?[] {null, 9.1, 4, null, 10, 7.8, 8.9, 9.1, 4};
            var numberOfMonths = monthlyConsequences.Length;
            var result = HelperFunctions.CalculateMeasureValue(numberOfMonths, monthlyConsequences, null);
            Assert.That(result, Is.Null);
        }
    }
}
