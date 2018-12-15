using AutoFixture;
using CL.FormulaHelper;
using NUnit.Framework;
using MeasureFormula.SharedCode;
using System;
using System.Linq;
using MeasureFormula.Common_Code;

namespace MeasureFormula.Tests.SharedCodeTest
{
    public class DateHelpersTests : MeasureFormulaTestsBase
    {
        private static int FiscalYearEnd;
        private const double StandardBestConditionScore = 10d;
        private DateTime ArbitraryInServiceDate;

        [SetUp]
        public void TestSetup()
        {
            FormulaBase.FiscalYearEndMonth = 6;
            FiscalYearEnd = 1 + fixture.Create<int>() % 12;
            var negativeMonthOffset = - (fixture.Create<int>() % 60 + 1);
            ArbitraryInServiceDate = FormulaBase.GetCalendarDateTime(ArbitraryStartYear, 1);
            ArbitraryInServiceDate = ArbitraryInServiceDate.AddMonths(negativeMonthOffset);
        }

        [Test]
        public void FindLastInServiceCalendarDate_NullCondition_ReturnsNull()
        {
            var output = DateHelpers.FindLastInServiceMonthOffset(null, ArbitraryStartYear, StandardBestConditionScore);
            Assert.That(output, Is.Null);
        }

        [Test]
        public void FindLastInServiceCalendarDate_NoBestConditions_ReturnsNull()
        {
            var conditionOutput = new double?[5];
            var output = DateHelpers.FindLastInServiceMonthOffset(conditionOutput, ArbitraryStartYear, StandardBestConditionScore);
            Assert.That(output, Is.Null);
        }

        [Test]
        public void FindLastInServiceCalendarDate_ValidInputs_ReturnsCorrectValues()
        {
            var conditionOutput = new double?[] {0, 0, 0, StandardBestConditionScore, 0, StandardBestConditionScore, 0};
            const int monthOffsetForBestCondition = 5;

            var output = DateHelpers.FindLastInServiceMonthOffset(conditionOutput, ArbitraryStartYear, StandardBestConditionScore);
            Assert.That(output, Is.EqualTo(monthOffsetForBestCondition));
        }
        
        [Test]
        public void LastInServiceMonthOffset_NormalMatchingCondition_ReturnsMatchingMonth()
        {
            double demoConditionScore = CustomerConstants.BestConditionScore;
            var sampleConditionScores = new double?[] {null, 9.5, 5.6, null, 6, demoConditionScore, 7.5, demoConditionScore, 8.7, 7.6, 6};
            
            var result = DateHelpers.LastInServiceMonthOffset(sampleConditionScores, ArbitraryInServiceDate, ArbitraryStartYear, demoConditionScore);
            var expectedResult = 7;
            Assert.That(result, Is.EqualTo(expectedResult));
        }
        
        [Test]
        public void LastInServiceMonthOffset_NoConditionScore_ReturnsInServiceDateOffset()
        {
            double demoConditionScore = CustomerConstants.BestConditionScore;
            
            var result = DateHelpers.LastInServiceMonthOffset(null, ArbitraryInServiceDate, ArbitraryStartYear, demoConditionScore);
            var expectedResult = FormulaBase.ConvertDateTimeToOffset(ArbitraryInServiceDate, ArbitraryStartYear);
            Assert.That(result, Is.EqualTo(expectedResult));
        }
        
        [Test]
        public void LastInServiceMonthOffset_NoMatchingConditionScore_ReturnsInServiceDateOffset()
        {
            double demoConditionScore = CustomerConstants.BestConditionScore;
            var sampleConditionScores = Enumerable.Repeat( (double?) (CustomerConstants.BestConditionScore + CustomerConstants.WorstConditionScore) / 2, 10).ToArray();
            
            var result = DateHelpers.LastInServiceMonthOffset(sampleConditionScores, ArbitraryInServiceDate, ArbitraryStartYear, demoConditionScore);
            var expectedResult = FormulaBase.ConvertDateTimeToOffset(ArbitraryInServiceDate, ArbitraryStartYear);
            Assert.That(result, Is.EqualTo(expectedResult));
        }
        
        [Test]
        public void LastInServiceMonthOffset_NoConditionScoreAndNoInServiceDate_ReturnsNull()
        {
            double demoConditionScore = CustomerConstants.BestConditionScore;
            var result = DateHelpers.LastInServiceMonthOffset(null, null, ArbitraryStartYear, demoConditionScore);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void LastInServiceMonthOffset_NoMatchingConditionAndNoInServiceDate_ReturnsNull()
        {
            double demoConditionScore = CustomerConstants.BestConditionScore;
            var sampleConditionScores = new double?[] {null, 9.5, 5.6, null, 6, 7.5, 8.7, 7.6, 6};
            
            var result = DateHelpers.LastInServiceMonthOffset(sampleConditionScores, null, ArbitraryStartYear, demoConditionScore);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void CalendarYearMonthToFiscalDate_NoMatchingConditionAndNoServiceDate_ReturnsNull()
        {
            double demoConditionScore = CustomerConstants.BestConditionScore;
            var sampleConditionScores = new double?[] {null, 9.5, 5.6, null, 6, 7.5, 8.7, 7.6, 6};
            
            var result = DateHelpers.LastInServiceMonthOffset(sampleConditionScores, null, ArbitraryStartYear, demoConditionScore);
            Assert.That(result, Is.Null);
        }
    }
}