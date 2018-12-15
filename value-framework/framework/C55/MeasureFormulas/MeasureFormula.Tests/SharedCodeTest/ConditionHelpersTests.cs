using System;
using System.Linq;
using CL.FormulaHelper.DTOs;
using NUnit.Framework;
using MeasureFormula.SharedCode;
using AutoFixture;
using CL.FormulaHelper;
using MeasureFormula.Common_Code;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.BusinessContinuityConsequenceBase;

namespace MeasureFormula.Tests.SharedCodeTest
{
    class ConditionHelpersTests : MeasureFormulaTestsBase
    {
        private readonly XYCurveDTO linear6YearsConditionCurve = ConditionHelpers.ConstructConditionDecayCurve(6);
        private readonly XYCurveDTO linear6MonthsConditionCurve = ConditionHelpers.ConstructConditionDecayCurve(0.5);
        private const double LocalTolerance = 0.0001;

        private const double BestConditionScore = 10.0;
        private readonly ConditionScoreAnswerDto[] emptyConditionScoreAnswerDto = new ConditionScoreAnswerDto[0];
        private DateTime InServiceDate;
        private int InServiceDateOffset;

        private readonly XYCurveDTO emptyCurve = new XYCurveDTO {Points = null};

        private void PrintArrayWithAtMostFourPlaces(double?[] someDoubles)
        {
            var formattedDoubles = someDoubles.Select(x => x == null ? "null" : string.Format("{0:0.####}", x)); //$"{x:0.####}"
            Console.WriteLine(string.Join(", ", formattedDoubles));
            Console.WriteLine();
        }
        
        [SetUp]
        public void FixtureSetup()
        {
            InServiceDate = new DateTime(ArbitraryStartYear - 10, 5, 15);
            InServiceDateOffset = FormulaBase.ConvertDateTimeToOffset(InServiceDate, ArbitraryStartYear);
        }        

        // UpdateDto Tests are located here because they need a real DTO to run against.

        [Test]
        public void UpdateDto_CanSetValueToNull()
        {
            var nullProductivity = fixture.Create<baseClass.TimeInvariantInputDTO>();
            TestHelpers.DataPrep.UpdateDto(nullProductivity, "SystemEmployeeProductivityValue", null);
            
            Assert.That(nullProductivity.SystemEmployeeProductivityValue, Is.Null);
        }
        
        [Test]
        public void UpdateDto_CanChangeValue()
        {
            var productivityDto = fixture.Create<baseClass.TimeInvariantInputDTO>();

            var resultingValue = fixture.Create<int>();
            TestHelpers.DataPrep.UpdateDto(productivityDto, "SystemEmployeeProductivityValue", resultingValue);
            
            Assert.That(productivityDto.SystemEmployeeProductivityValue, Is.EqualTo(resultingValue));
        }

        [Test]
        public void UpdateDto_WhenBothTargetAndExistingValuesAreNull_WillDoNothing()
        {
            TestHelpers.DataPrep.SetConstructorParameter<Int32?>(fixture,"p_SystemEmployeeProductivityValue", null);
            var productivityDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            Int32? targetValue = null;
            TestHelpers.DataPrep.UpdateDto(productivityDto, "SystemEmployeeProductivityValue", targetValue);

            Assert.That(productivityDto.SystemEmployeeProductivityValue, Is.EqualTo(targetValue));
        }

        [Test]
        public void SetConstructorParameter_CalledTwice_WillUseSecondParameter()
        {
            var firstValue = 5;
            var secondValue = 10;
            TestHelpers.DataPrep.SetConstructorParameter<Int32?>(fixture, "p_SystemEmployeeProductivityValue", firstValue);
            TestHelpers.DataPrep.SetConstructorParameter<Int32?>(fixture, "p_SystemEmployeeProductivityValue", secondValue);
            var productivityDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            Assert.That(productivityDto.SystemEmployeeProductivityValue, Is.EqualTo(secondValue));
        }

        [Test]
        public void ReplaceNullWithSubsequentAdjacentValueAllNullsTest()
        {
            var allNulls = Enumerable.Repeat((double?) null, 4).ToArray();
            var replacedValues = ConditionHelpers.ReplaceNullWithSubsequentAdjacentValue(
                monthlyConditions: allNulls
            );
            Assert.That(replacedValues, Is.EqualTo(allNulls));
        }

        [Test]
        public void ReplaceNullWithSubsequentAdjacentValueTest()
        {
            var linear3MonthsExpectedLifeConditionCurve = ConditionHelpers.ConstructConditionDecayCurve(0.25);

            var months = Enumerable.Range(0, 8);
            var monthlyConditions = months.Select(x => (double?)linear3MonthsExpectedLifeConditionCurve.YFromX(x / 12.0)).ToArray();

            var genericMonthlyConditionDecay = new[] { (double?)null, null, null }.Concat(monthlyConditions).ToArray();
            PrintArrayWithAtMostFourPlaces(genericMonthlyConditionDecay);

            // ISDOffset < 0
            Console.WriteLine("ISD Offset < 0");
            var replacedValues = ConditionHelpers.ReplaceNullWithSubsequentAdjacentValue(
                monthlyConditions: genericMonthlyConditionDecay
            );
            var expected = new double?[] { 10, 10, 10, 10, 7.6667, 5.3333, 3, 0.6667, 0, 0, 0 };
            Assert.That(replacedValues, Is.EqualTo(expected).Within(LocalTolerance));

            // 0 < ISDOffset < mC.Length
            Console.WriteLine("0 < ISDOffset << mC.Length");
            replacedValues = ConditionHelpers.ReplaceNullWithSubsequentAdjacentValue(
                monthlyConditions: genericMonthlyConditionDecay
            );
            Assert.That(replacedValues, Is.EqualTo(expected).Within(LocalTolerance));

            // 0 << ISDOffset < mC.Length
            Console.WriteLine("0 << ISDOffset < mC.Length");
            replacedValues = ConditionHelpers.ReplaceNullWithSubsequentAdjacentValue(
                monthlyConditions: genericMonthlyConditionDecay
            );
            PrintArrayWithAtMostFourPlaces(replacedValues);
            Assert.That(replacedValues, Is.EqualTo(expected).Within(LocalTolerance));

            // mC.Length < ISDOffset
            Console.WriteLine("mC.Length < ISDOffset");
            replacedValues = ConditionHelpers.ReplaceNullWithSubsequentAdjacentValue(
                monthlyConditions: genericMonthlyConditionDecay
            );
            PrintArrayWithAtMostFourPlaces(replacedValues);
            Assert.That(replacedValues, Is.EqualTo(expected).Within(LocalTolerance));
        }

        [Test]
        public void ApplyConditionOverride_WhenConditionCurveIsEmpty_ReturnsNull()
        {
            var months = Enumerable.Range(0, 12);
            var monthlyConditions = months.Select(x => (double?) linear6MonthsConditionCurve.YFromX(x/12.0)).ToArray();
            
            var genericMonthlyConditionDecay = new[] { (double?)null, null }.Concat(monthlyConditions).ToArray() ;

            var emptyCurveDecay = ConditionHelpers.ApplyConditionOverride(
                                                                          overrideOffsetMonth: -2 - monthlyConditions.Length,
                                                                          ageInYearsToConditionCurve: emptyCurve,
                                                                          overrideCondition: 8,
                                                                          monthlyConditions: genericMonthlyConditionDecay);
            Assert.That(emptyCurveDecay, Is.Null);        
        }

        [Test]
        public void ApplyConditionOverrideTest()
        {
            XYCurveDTO ageInYearsToConditionCurve = linear6MonthsConditionCurve;
            var months = Enumerable.Range(0, 12);
            var monthlyConditions = months.Select(x => (double?) linear6MonthsConditionCurve.YFromX(x/12.0)).ToArray();

            var genericMonthlyConditionDecay = new[] { (double?)null, null }.Concat(monthlyConditions).ToArray() ;
            PrintArrayWithAtMostFourPlaces(genericMonthlyConditionDecay);

            // expected Value taken from initial behaviour of extracted function
            // previous behaviour expectedWhenOverrideOffsetIsLessThanNegativeMonthliesLength = new double?[] { 0, null, 10, 8.8333, 7.6667, 6.5, 5.3333, 4.1667, 3, 1.8333, 0.6667, 0, 0, 0 };

            var expectedWhenOverrideOffsetIsLessThanNegativeMonthliesLength = Enumerable.Repeat((double?) 0.0, genericMonthlyConditionDecay.Length).ToArray();
            var expectedWhenOverrideOffsetIsMuchLessThanZero = new double?[] { 7.6667, 6.5, 5.3333, 4.1667, 3, 1.8333, 0.6667, 0, 0, 0, 0, 0, 0, 0 };
            // old behaviour new double?[] { 5.3333, 4.1667, 3, 1.8333, 0.6667, 0, 0, 0, 0, 0, 0.6667, 0, 0, 0 };
            var expectedWhenOverrideOffsetIsLessThanZero = new double?[] { 5.3333, 4.1667, 3, 1.8333, 0.6667, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var expectedWhenOverrideOffsetIsLessThanMonthliesLength = new double?[] { null, null, 10, 8.8333, 10, 8.8333, 7.6667, 6.5, 5.3333, 4.1667, 3, 1.8333, 0.6667, 0 };
            var expectedWhenOverrideOffsetIsGreatherThanMonthliesLength = new double?[] { null, null, 10, 8.8333, 7.6667, 6.5, 5.3333, 4.1667, 3, 1.8333, 0.6667, 0, 0, 0 };
            var expectedValues = new[]
            {
                expectedWhenOverrideOffsetIsLessThanNegativeMonthliesLength,
                expectedWhenOverrideOffsetIsMuchLessThanZero,
                expectedWhenOverrideOffsetIsLessThanZero,
                expectedWhenOverrideOffsetIsLessThanMonthliesLength,
                expectedWhenOverrideOffsetIsGreatherThanMonthliesLength
            };

            TestAllOverrideOffsets(genericMonthlyConditionDecay, monthlyConditions, CustomerConstants.BestConditionScore, ageInYearsToConditionCurve, expectedValues);

            //expectedWhenOverrideOffsetIsLessThanNegativeMonthliesLength = new double?[] { 0, 0, 10, 8.8333, 7.6667, 6.5, 5.3333, 4.1667, 3, 1.8333, 0.6667, 0, 0, 0 };
            expectedWhenOverrideOffsetIsMuchLessThanZero = new double?[] { 2.6667, 1.5, 0.3333, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            // old behaviour new double?[] { 5.3333, 4.1667, 3, 1.8333, 0.6667, 0, 0, 0, 0, 0, 0.6667, 0, 0, 0 };
            expectedWhenOverrideOffsetIsLessThanZero = new double?[] { 0.3333, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            expectedWhenOverrideOffsetIsLessThanMonthliesLength = new double?[] { null, null, 10, 8.8333, 5, 3.8333, 2.6667, 1.5, 0.3333, 0, 0, 0, 0, 0 };
            //expectedWhenOverrideOffsetIsGreatherThanMonthliesLength = new double?[] { null, null, 10, 8.8333, 7.6667, 6.5, 5.3333, 4.1667, 3, 1.8333, 0.6667, 0, 0, 0 };
            expectedValues = new[]
            {
                expectedWhenOverrideOffsetIsLessThanNegativeMonthliesLength,
                expectedWhenOverrideOffsetIsMuchLessThanZero,
                expectedWhenOverrideOffsetIsLessThanZero,
                expectedWhenOverrideOffsetIsLessThanMonthliesLength,
                expectedWhenOverrideOffsetIsGreatherThanMonthliesLength
            };
            TestAllOverrideOffsets(genericMonthlyConditionDecay, monthlyConditions, 5.0, ageInYearsToConditionCurve, expectedValues);

            expectedWhenOverrideOffsetIsMuchLessThanZero = new double?[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            // old behaviour was { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.6667, 0, 0, 0 };
            expectedWhenOverrideOffsetIsLessThanZero = new double?[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            expectedWhenOverrideOffsetIsLessThanMonthliesLength = new double?[] { null, null, 10, 8.8333, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            expectedValues = new[]
            {
                expectedWhenOverrideOffsetIsLessThanNegativeMonthliesLength,
                expectedWhenOverrideOffsetIsMuchLessThanZero,
                expectedWhenOverrideOffsetIsLessThanZero,
                expectedWhenOverrideOffsetIsLessThanMonthliesLength,
                expectedWhenOverrideOffsetIsGreatherThanMonthliesLength
            };
            TestAllOverrideOffsets(genericMonthlyConditionDecay, monthlyConditions, CustomerConstants.WorstConditionScore, ageInYearsToConditionCurve, expectedValues);
        }

        private void TestAllOverrideOffsets(
            double?[] genericMonthlyConditionDecay,
            double?[] monthlyConditions,
            double testOverrideCondition,
            XYCurveDTO ageInYearsToConditionCurve,
            double?[][] expectedValues)
        {
            Console.WriteLine("Using override Condition of: " + testOverrideCondition);

            // oOM < -monthlyConditions.Length
            var transformedMonthlyConditionDecay = ConditionHelpers.ApplyConditionOverride(
                overrideOffsetMonth: -2 - monthlyConditions.Length,
                ageInYearsToConditionCurve: ageInYearsToConditionCurve,
                overrideCondition: testOverrideCondition,
                monthlyConditions: genericMonthlyConditionDecay);
            Assert.That(transformedMonthlyConditionDecay, Is.EqualTo(expectedValues[0]).Within(LocalTolerance));

            // - monthlyConditions.Length < oOM << 0
            transformedMonthlyConditionDecay = ConditionHelpers.ApplyConditionOverride(
                overrideOffsetMonth: -2,
                ageInYearsToConditionCurve: ageInYearsToConditionCurve,
                overrideCondition: testOverrideCondition,
                monthlyConditions: genericMonthlyConditionDecay);
            Assert.That(transformedMonthlyConditionDecay, Is.EqualTo(expectedValues[1]).Within(LocalTolerance));

            // - monthlyConditions.Length << oOM < 0
            transformedMonthlyConditionDecay = ConditionHelpers.ApplyConditionOverride(
                overrideOffsetMonth: 8 - monthlyConditions.Length,
                ageInYearsToConditionCurve: ageInYearsToConditionCurve,
                overrideCondition: testOverrideCondition,
                monthlyConditions: genericMonthlyConditionDecay);
            Assert.That(transformedMonthlyConditionDecay, Is.EqualTo(expectedValues[2]).Within(LocalTolerance));

            // 0 < oOM < mC.Length
            transformedMonthlyConditionDecay = ConditionHelpers.ApplyConditionOverride(
                overrideOffsetMonth: 4,
                ageInYearsToConditionCurve: ageInYearsToConditionCurve,
                overrideCondition: testOverrideCondition,
                monthlyConditions: genericMonthlyConditionDecay);
            Assert.That(transformedMonthlyConditionDecay, Is.EqualTo(expectedValues[3]).Within(LocalTolerance));

            // mC.Length < oOm
            transformedMonthlyConditionDecay = ConditionHelpers.ApplyConditionOverride(
                overrideOffsetMonth: monthlyConditions.Length + 2,
                ageInYearsToConditionCurve: ageInYearsToConditionCurve,
                overrideCondition: testOverrideCondition,
                monthlyConditions: genericMonthlyConditionDecay);
            Assert.That(transformedMonthlyConditionDecay, Is.EqualTo(expectedValues[4]).Within(LocalTolerance));

        }

        [Test]
        public void ComputeConditionsFromInServiceDate_WhenCurveIsEmpty_ReturnsNull()
        {
            var monthlyConditions = ConditionHelpers.ComputeConditionsFromInServiceDate(
                                                                                        months: 6,
                                                                                        inServiceDateOffsetMonth: -8,
                                                                                        ageInYearsToConditionCurve: emptyCurve
                                                                                       );
            Assert.That(monthlyConditions, Is.Null);
        }

        [Test]
        public void MonthlyConditionsFromInServiceDateTest()
        {
            // isd < -months < 0 < months < curvelength
            var monthlyConditions = ConditionHelpers.ComputeConditionsFromInServiceDate(
                months: 6,
                inServiceDateOffsetMonth: -8,
                ageInYearsToConditionCurve: linear6YearsConditionCurve
            );
            Assert.That(monthlyConditions, Is.Not.Null);
            var expected = new [] {(double?) 9.2222, 9.125, 9.0278, 8.9306, 8.8333, 8.7361};
            Assert.That(monthlyConditions, Is.EqualTo(expected).Within(LocalTolerance));

            // -months < isd < 0 < months < curvelength
            monthlyConditions = ConditionHelpers.ComputeConditionsFromInServiceDate(
                months: 6,
                inServiceDateOffsetMonth: -2,
                ageInYearsToConditionCurve: linear6YearsConditionCurve
            );
            Assert.That(monthlyConditions, Is.Not.Null);
            expected = new[] { (double?)9.8056, 9.7083, 9.6111, 9.5139, 9.4167, 9.3194 };
            Assert.That(monthlyConditions, Is.EqualTo(expected).Within(LocalTolerance));

            // -months < isd = 0 < months < curvelength
            monthlyConditions = ConditionHelpers.ComputeConditionsFromInServiceDate(
                months: 6,
                inServiceDateOffsetMonth: 0,
                ageInYearsToConditionCurve: linear6YearsConditionCurve
            );
            Assert.That(monthlyConditions, Is.Not.Null);
            expected = new[] { (double?)10, 9.9028, 9.8056, 9.7083, 9.6111, 9.5139 };
            Assert.That(monthlyConditions, Is.EqualTo(expected).Within(LocalTolerance));

            // 0 < isd < months < curveLength
            monthlyConditions = ConditionHelpers.ComputeConditionsFromInServiceDate(
                months: 6,
                inServiceDateOffsetMonth: 2,
                ageInYearsToConditionCurve: linear6YearsConditionCurve
            );
            Assert.That(monthlyConditions, Is.Not.Null);
            expected = new[] { (double?) null, null, 10, 9.9028, 9.8056, 9.7083 };
            Assert.That(monthlyConditions, Is.EqualTo(expected).Within(LocalTolerance));

            // 0 < months = isd
            monthlyConditions = ConditionHelpers.ComputeConditionsFromInServiceDate(
                months: 6,
                inServiceDateOffsetMonth: 6,
                ageInYearsToConditionCurve: linear6YearsConditionCurve
            );
            Assert.That(monthlyConditions, Is.Not.Null);
            expected = new[] { (double?) null, null, null, null, null, null };
            Assert.That(monthlyConditions, Is.EqualTo(expected).Within(LocalTolerance));

            // 0 < months < isd
            Console.WriteLine(" 0 < months < isd < curveLength");
            monthlyConditions = ConditionHelpers.ComputeConditionsFromInServiceDate(
                months: 6,
                inServiceDateOffsetMonth: 8,
                ageInYearsToConditionCurve: linear6YearsConditionCurve
            );
            Assert.That(monthlyConditions, Is.Not.Null);
            expected = new[] { (double?)null, null, null, null, null, null };
            Assert.That(monthlyConditions, Is.EqualTo(expected).Within(LocalTolerance));

            //isd < -months < -curvelength < 0 <  curvelength < months
            monthlyConditions = ConditionHelpers.ComputeConditionsFromInServiceDate(
                months: 12,
                inServiceDateOffsetMonth: -14,
                ageInYearsToConditionCurve: linear6MonthsConditionCurve
            );
            expected = new[] { (double?)0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Assert.That(monthlyConditions, Is.EqualTo(expected).Within(LocalTolerance));

            //-months < isd < -curvelength  <  0 <  curvelength < months
            monthlyConditions = ConditionHelpers.ComputeConditionsFromInServiceDate(
                months: 12,
                inServiceDateOffsetMonth: -10,
                ageInYearsToConditionCurve: linear6MonthsConditionCurve
            );
            expected = new[] { (double?)0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Assert.That(monthlyConditions, Is.EqualTo(expected).Within(LocalTolerance));

            //-months < -curvelength < isd <  0 <  curvelength < months
            monthlyConditions = ConditionHelpers.ComputeConditionsFromInServiceDate(
                months: 12,
                inServiceDateOffsetMonth: -4,
                ageInYearsToConditionCurve: linear6MonthsConditionCurve
            );
            expected = new[] { (double?)5.3333, 4.1667, 3, 1.8333, 0.6667, 0, 0, 0, 0, 0, 0, 0 };
            Assert.That(monthlyConditions, Is.EqualTo(expected).Within(LocalTolerance));

            // 0 < isd < curvelength < months
            monthlyConditions = ConditionHelpers.ComputeConditionsFromInServiceDate(
                months: 12,
                inServiceDateOffsetMonth: 4,
                ageInYearsToConditionCurve: linear6MonthsConditionCurve
            );
            expected = new[] { (double?)null, null, null, null, 10, 8.8333, 7.6667, 6.5, 5.3333, 4.1667, 3, 1.8333 };
            Assert.That(monthlyConditions, Is.EqualTo(expected).Within(LocalTolerance));

            // 0 <  curvelength < isd < months
            monthlyConditions = ConditionHelpers.ComputeConditionsFromInServiceDate(
                months: 12,
                inServiceDateOffsetMonth: 9,
                ageInYearsToConditionCurve: linear6MonthsConditionCurve
            );
            expected = new[] { (double?) null, null, null, null, null, null, null, null, null, 10, 8.8333, 7.6667 };
            Assert.That(monthlyConditions, Is.EqualTo(expected).Within(LocalTolerance));

            // 0 <  curvelength < months < isd
            monthlyConditions = ConditionHelpers.ComputeConditionsFromInServiceDate(
                months: 12,
                inServiceDateOffsetMonth:  14,
                ageInYearsToConditionCurve: linear6MonthsConditionCurve
            );
            expected = new[] { (double?)null, null, null, null, null, null, null, null, null, null, null, null };
            Assert.That(monthlyConditions, Is.EqualTo(expected).Within(LocalTolerance));
        }

        [Test]
        public void LastInServiceOffset_ConditionScoreInRange_ReturnCalc()
        {
            var sampleConditionScore = fixture.Create<ConditionScoreAnswerDto[]>();
            sampleConditionScore[0].ConditionScore = 9.2;
            sampleConditionScore[1].ConditionScore = 5.8;
            sampleConditionScore[2].ConditionScore = 9.2;
            var inServiceDate = new DateTime(ArbitraryStartYear, 1, 1).AddMonths( - fixture.Create<int>() % 60);
            var demoConditionScore = 9.2;
            
            var result = ConditionHelpers.LastInServiceOffset(sampleConditionScore, inServiceDate, ArbitraryStartYear, demoConditionScore);
            var expectedResult = FormulaBase.ConvertDateTimeToOffset(sampleConditionScore[2].Date, ArbitraryStartYear);
            Assert.That(result, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void LastInServiceOffset_NoMatchingCondition_ReturnInServiceDateMonth()
        {
            var samepleConditionScore = fixture.Create<ConditionScoreAnswerDto[]>();
            samepleConditionScore[0].ConditionScore = 9.2;
            samepleConditionScore[1].ConditionScore = 5.8;
            samepleConditionScore[2].ConditionScore = 9.2;
            var inServiceDate = new DateTime(ArbitraryStartYear, 1, 1).AddMonths( - fixture.Create<int>() % 60);
            var demoConditionScore = 10;
            
            var result = ConditionHelpers.LastInServiceOffset(samepleConditionScore, inServiceDate, ArbitraryStartYear, demoConditionScore);
            var expectedResult = FormulaBase.ConvertDateTimeToOffset(inServiceDate, ArbitraryStartYear);
            Assert.That(result, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void LastInServiceOffset_NullInServiceDateAndNoMatchingCondition_ReturnNull()
        {
            var samepleConditionScore = fixture.Create<ConditionScoreAnswerDto[]>();
            samepleConditionScore[0].ConditionScore = 9.2;
            samepleConditionScore[1].ConditionScore = 5.8;
            samepleConditionScore[2].ConditionScore = 9.2;
            var demoConditionScore = 10;
            
            var result = ConditionHelpers.LastInServiceOffset(samepleConditionScore, null, ArbitraryStartYear, demoConditionScore);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void FindLastInServiceCalendarDate_ConditionScoreInRange_ReturnsCalc()
        {
            var sampleConditionData = new double?[] {null, 2, 3, null, 4, 10, 9, 10, 6};
            double demoConditionScore = 10;

            var result = ConditionHelpers.FindLastInServiceCalendarDate(sampleConditionData, ArbitraryStartYear, demoConditionScore);
            
            var whereConditionEqualsBest = sampleConditionData.Select((value, index) => value.Equals(demoConditionScore)? index : (int?) null).Where(x => x != null).ToArray().Last();
            if (whereConditionEqualsBest != null)
            {
                var expectedResult = ConditionHelpers.ConvertMonthOffsetToCalendarDateTime(ArbitraryStartYear, (int) whereConditionEqualsBest);
                Assert.That(result, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
            }
        }
        
        [Test]
        public void FindLastInServiceCalendarDate_NoMatchingCondition_ReturnsNull()
        {
            var sampleConditionData = new double?[] {null, 2, 3, null, 4, 10, 9, 10, 6};
            double demoConditionScore = 1.0;
            
            var result = ConditionHelpers.FindLastInServiceCalendarDate(sampleConditionData, ArbitraryStartYear, demoConditionScore);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void LastInServiceMonthOffset_ConditionScoreInRange_ReturnsCalc()
        {
            var sampleConditionData = new double?[] {null, 2, 3, null, 4, 10, 9, 10, 6};
            var inServiceDate = new DateTime(ArbitraryStartYear, 1, 1).AddMonths( - fixture.Create<int>() % 60);
            double demoConditionScore = 10;
            
            var result = ConditionHelpers.LastInServiceMonthOffset(sampleConditionData, inServiceDate, ArbitraryStartYear, demoConditionScore);
            var expectedResult = sampleConditionData.Select((value, index) => value.Equals(demoConditionScore)? index : (int?) null).Where(x => x != null).ToArray().Last() ;
            Assert.That(result, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void LastInServiceMonthOffset_NoMatchingCondition_ReturnsInServiceDateMonth()
        {
            var sampleConditionData = new double?[] {null, 2, 3, null, 4, 8, 9, 7, 6};
            var inServiceDate = new DateTime(ArbitraryStartYear, 1, 1).AddMonths( - fixture.Create<int>() % 60);
            double demoConditionScore = 10;
            
            var result = ConditionHelpers.LastInServiceMonthOffset(sampleConditionData, inServiceDate, ArbitraryStartYear, demoConditionScore);
            var expectedResult = FormulaBase.ConvertDateTimeToOffset(inServiceDate, ArbitraryStartYear);
            Assert.That(result, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void LastInServiceMonthOffset_NoMatchingConditionAndNoInServiceDate_ReturnsNull()
        {
            var sampleConditionData = new double?[] {null, 2, 3, null, 4, 8, 9, 7, 6};
            double demoConditionScore = 10;
            
            var result = ConditionHelpers.LastInServiceMonthOffset(sampleConditionData, null, ArbitraryStartYear, demoConditionScore);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void IsFirstReplacement_ConditionScoreInRange_ReturnsFalse()
        {
            var sampleConditionData = new double?[] {null, 2, 3, null, 4, 10, 9, 10, 6};
            var demoConditionScore = 10;
            
            var result = ConditionHelpers.IsFirstReplacement(sampleConditionData, demoConditionScore);
            Assert.That(result, Is.False);
        }
        
        [Test]
        public void IsFirstReplacement_NoMatchingCondition_ReturnsTrue()
        {
            var sampleConditionData = new double?[] {null, 2, 3, null, 4, 9, 9, 5.3, 9.9};
            var demoConditionScore = 10;
            
            var result = ConditionHelpers.IsFirstReplacement(sampleConditionData, demoConditionScore);
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void IsFirstReplacement_ConditionNull_ReturnsTrue()
        {
            var sampleConditionData = new double?[10];
            var demoConditionScore = 10;
            
            var result = ConditionHelpers.IsFirstReplacement(sampleConditionData, demoConditionScore);
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void WhenConditionScoresNullAndNoInServiceDate_LastInServiceOffset_ReturnsNull()
        {
            var result = ConditionHelpers.LastInServiceOffset(null, InServiceDate, ArbitraryStartYear, BestConditionScore);
            
            Assert.That(result, Is.EqualTo(InServiceDateOffset));
        }
        
        [Test]
        public void WhenConditionScoresNullAndHasInServiceDate_LastInServiceOffset_ReturnsInServiceDateOffset()
        {
            var result = ConditionHelpers.LastInServiceOffset(null, null, ArbitraryStartYear, BestConditionScore);
            
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void WhenConditionScoresEmptyAndNoInServiceDate_LastInServiceOffset_ReturnsNull()
        {
            var result = ConditionHelpers.LastInServiceOffset(emptyConditionScoreAnswerDto, null, ArbitraryStartYear, BestConditionScore);
            
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void WhenConditionScoresEmptyAndHasInServiceDate_LastInServiceOffset_ReturnsInServiceDateOffset()
        {
            var result = ConditionHelpers.LastInServiceOffset(emptyConditionScoreAnswerDto, InServiceDate, ArbitraryStartYear, BestConditionScore);
            
            Assert.That(result, Is.EqualTo(InServiceDateOffset));
        }

        [Test]
        public void WhenTwoConditionsScores_LastInserviceOffset_ReturnsLastOffset()
        {
            var secondDate = new DateTime(ArbitraryStartYear + 10, 1, 15);
            ConditionScoreAnswerDto[] twoConditionScoresAnswerDto = 
            {
                new ConditionScoreAnswerDto {Date = new DateTime(ArbitraryStartYear + 5, 1, 15), ConditionScore = BestConditionScore},
                new ConditionScoreAnswerDto {Date = secondDate, ConditionScore = BestConditionScore}
            };
            
            var result = ConditionHelpers.LastInServiceOffset(twoConditionScoresAnswerDto, InServiceDate, ArbitraryStartYear, BestConditionScore);

            var secondDateOffset = FormulaBase.ConvertDateTimeToOffset(secondDate, ArbitraryStartYear);
            Assert.That(result, Is.EqualTo(secondDateOffset));
        }
        
        [Test]
        public void WhenTwoConditionsScoresLastOneNotBest_LastInserviceOffset_ReturnsFirstOffset()
        {
            var firstDate = new DateTime(ArbitraryStartYear + 5, 1, 15);
            ConditionScoreAnswerDto[] twoConditionScoresAnswerDto = 
            {
                new ConditionScoreAnswerDto {Date = firstDate, ConditionScore = BestConditionScore},
                new ConditionScoreAnswerDto {Date = new DateTime(ArbitraryStartYear + 5, 1, 15), ConditionScore = BestConditionScore/2.0}
            };
            
            var result = ConditionHelpers.LastInServiceOffset(twoConditionScoresAnswerDto, InServiceDate, ArbitraryStartYear, BestConditionScore);

            var firstDateOffset = FormulaBase.ConvertDateTimeToOffset(firstDate, ArbitraryStartYear);
            Assert.That(result, Is.EqualTo(firstDateOffset));
        }
        
        [Test]
        public void WhenTwoConditionsScoresNoneBest_LastInserviceOffset_ReturnsInServiceDateOffset()
        {
            var firstDate = new DateTime(ArbitraryStartYear + 5, 1, 15);
            ConditionScoreAnswerDto[] twoConditionScoresAnswerDto = 
            {
                new ConditionScoreAnswerDto {Date = firstDate, ConditionScore = BestConditionScore/2.0},
                new ConditionScoreAnswerDto {Date = new DateTime(ArbitraryStartYear + 5, 1, 15), ConditionScore = BestConditionScore/2.0}
            };
            
            var result = ConditionHelpers.LastInServiceOffset(twoConditionScoresAnswerDto, InServiceDate, ArbitraryStartYear, BestConditionScore);

            Assert.That(result, Is.EqualTo(InServiceDateOffset));
        }        
    }
}
