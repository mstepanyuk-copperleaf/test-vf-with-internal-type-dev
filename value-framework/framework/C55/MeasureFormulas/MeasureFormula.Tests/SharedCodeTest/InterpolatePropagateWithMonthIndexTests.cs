using System;
using System.Linq;
using CL.FormulaHelper;
using CL.FormulaHelper.DTOs;
using NUnit.Framework;

using static MeasureFormula.SharedCode.HelperFunctions;

namespace MeasureFormula.Tests.SharedCodeTest
{
    public class SingleValueTimeVariantInputDTO : ITimeVariantInputDTO
    {
        public SingleValueTimeVariantInputDTO( double p_PeriodValue, TimePeriodDTO p_TimePeriod)
        {
            PeriodValue = p_PeriodValue;
            TimePeriod = p_TimePeriod;
        }

        public readonly double PeriodValue;
        public TimePeriodDTO TimePeriod { get; }

        public double? ValueAtIndex(int monthIndex)
        {
            return InterpolatePropagateWithMonthIndexTests.FunctionReturnsIndexPlusValue(this, monthIndex);
        }

        public double? ValueAtIgnoringIndex(int monthIndex)
        {
            return InterpolatePropagateWithMonthIndexTests.FunctionReturnsValueOnly(this, monthIndex);
        }
    }
    
    [TestFixture]
    public class InterpolatePropagateWithMonthIndexTests : MeasureFormulaTestsBase
    {
        private const int FiveYearsInMonths = 60;
        public static readonly Func<SingleValueTimeVariantInputDTO, int, double?> FunctionReturnsIndexPlusValue = (data, i) => data.PeriodValue + i;
        public static readonly Func<SingleValueTimeVariantInputDTO, int, double?> FunctionReturnsValueOnly = (data, i) => data.PeriodValue;
        public static readonly Func<SingleValueTimeVariantInputDTO, double?> FunctionOfValueOnly = data => data.PeriodValue;
        
        private SingleValueTimeVariantInputDTO StartYearOneYearDuration;
        private SingleValueTimeVariantInputDTO StartYearZeroDuration;
        private SingleValueTimeVariantInputDTO StartYearNullDuration;
                
        private SingleValueTimeVariantInputDTO SixMonthsBeforeStartYearNullDuration;
        private SingleValueTimeVariantInputDTO SixMonthsAfterStartYearNullDuration;
        
        private SingleValueTimeVariantInputDTO SecondYearOneYearDuration;
        private SingleValueTimeVariantInputDTO SecondYearTenYearDuration;
        private SingleValueTimeVariantInputDTO SecondYearZeroDuration;
        private SingleValueTimeVariantInputDTO SecondYearNullDuration;
        
        private SingleValueTimeVariantInputDTO ThirdYearOneYearDuration;
        
        /*
         * Enumeration of test cases
         * At onset can have Earlier, Later or Aligned with start, normal (happy path) case is Aligned with start
         * In Transition can have Gap, Aligned via Duration, Aligned via null, Aligned via 0
         * At end can have Earlier,  Aligned via Duration, Aligned via null, Aligned via 0, normal (happy path) case is Aligned via null
         * Onset testing can be done with single element (having 2 elements will not affect how the initial was handled)
         * Transition testing can be done with 2 elements, use Aligned at start and Aligned via null at end
         * End testing must do both single and 2 element cases, using Aligned at start and when 2 elements aligned via null
         * For each test case check both
         *     for a function that does not depend on Index, agrees with Interpolate Propagate and that 
         *     for function that does depend on index matches expected values constructed manually
         */
        
        [SetUp]
        public void FixtureSetup()
        {
            var startYearAsDate = FormulaBase.GetCalendarDateTime(ArbitraryStartYear, 1);
            StartYearOneYearDuration = new SingleValueTimeVariantInputDTO(100.0, new TimePeriodDTO {DurationInMonths = 12, StartTime = startYearAsDate});
            StartYearZeroDuration = new SingleValueTimeVariantInputDTO(1000.0, new TimePeriodDTO {DurationInMonths = 0, StartTime = startYearAsDate});
            StartYearNullDuration = new SingleValueTimeVariantInputDTO(10000.0, new TimePeriodDTO {DurationInMonths = null, StartTime = startYearAsDate});
            
            var startSecondYearAsDate = startYearAsDate.AddYears(1);
            SecondYearOneYearDuration = new SingleValueTimeVariantInputDTO(100.0, new TimePeriodDTO {DurationInMonths = 12, StartTime = startSecondYearAsDate});
            SecondYearTenYearDuration = new SingleValueTimeVariantInputDTO(100.0, new TimePeriodDTO {DurationInMonths = 12 * 10, StartTime = startSecondYearAsDate});
            SecondYearZeroDuration = new SingleValueTimeVariantInputDTO(1000.0, new TimePeriodDTO {DurationInMonths = 0, StartTime = startSecondYearAsDate});
            SecondYearNullDuration = new SingleValueTimeVariantInputDTO(10000.0, new TimePeriodDTO {DurationInMonths = null, StartTime = startSecondYearAsDate});
            
            var startThirdYearAsDate = startYearAsDate.AddYears(2);
            ThirdYearOneYearDuration = new SingleValueTimeVariantInputDTO(300.0, new TimePeriodDTO {DurationInMonths = 12, StartTime = startThirdYearAsDate});

            var sixMonthsBeforeStart = FormulaBase.GetCalendarDateTime(ArbitraryStartYear - 1, 7);
            SixMonthsBeforeStartYearNullDuration =
                new SingleValueTimeVariantInputDTO(-50.0, new TimePeriodDTO {DurationInMonths = null, StartTime = sixMonthsBeforeStart});
            
            var sixMonthsAfterStart = FormulaBase.GetCalendarDateTime(ArbitraryStartYear, 7);
            SixMonthsAfterStartYearNullDuration =
                new SingleValueTimeVariantInputDTO(-80.0, new TimePeriodDTO {DurationInMonths = null, StartTime = sixMonthsAfterStart});


        }
        
        [Test]
        public void InterpolatePropagateWithMonthIndex_EmptyTimeVarying_ReturnsNull()
        {
            var emptyTimeVarying = new SingleValueTimeVariantInputDTO[0];

            var indexIndepentResult =
                InterpolatePropagateWithMonthIndex(emptyTimeVarying, ArbitraryStartYear, FiveYearsInMonths, FunctionReturnsValueOnly);
            var interpolatePropagateResult = FormulaBase.InterpolatePropagate(emptyTimeVarying, ArbitraryStartYear, FiveYearsInMonths, FunctionOfValueOnly);
            Assert.That(indexIndepentResult, Is.EqualTo(interpolatePropagateResult));

            var indexDependentResult =
                InterpolatePropagateWithMonthIndex(emptyTimeVarying, ArbitraryStartYear, FiveYearsInMonths, FunctionReturnsIndexPlusValue);

            Assert.That(indexDependentResult, Is.Null);
        }

        [Test]
        public void InterpolatePropagateWithMonthIndex_PeriodsCompletelyBeforeResultArray_ReturnsNull()
        {
            var tenYearsBeforeStart = FormulaBase.GetCalendarDateTime(ArbitraryStartYear - 10, 1);
            var tenYearsBeforeStartOneYearDuration =
                new SingleValueTimeVariantInputDTO(-1200, new TimePeriodDTO {DurationInMonths = 12, StartTime = tenYearsBeforeStart});
            var singlePeriodStartsAndEndsEarly = new[] {tenYearsBeforeStartOneYearDuration};
            
            var indexlessComparisonResults = BuildInterpolatePropagateComparisons(singlePeriodStartsAndEndsEarly);
            Assert.That(indexlessComparisonResults.Item1, Is.EqualTo(indexlessComparisonResults.Item2));
            
            var indexDependentResult = InterpolatePropagateWithMonthIndex(singlePeriodStartsAndEndsEarly, ArbitraryStartYear, FiveYearsInMonths, FunctionReturnsIndexPlusValue);
            Assert.That(indexDependentResult, Is.Null);           
        }
        
        private Tuple<double?[], double?[]> BuildInterpolatePropagateComparisons(SingleValueTimeVariantInputDTO[] timeVaryingDto)
        {
            var indexIndepentResult = InterpolatePropagateWithMonthIndex(timeVaryingDto, ArbitraryStartYear, FiveYearsInMonths, FunctionReturnsValueOnly);
            var interpolatePropagateResult = FormulaBase.InterpolatePropagate(timeVaryingDto, ArbitraryStartYear, FiveYearsInMonths, FunctionOfValueOnly);
            
            return new Tuple<double?[], double?[]>(indexIndepentResult, interpolatePropagateResult);
        }        

        [Test]
        public void InterpolatePropagateWithMonthIndex_FirstPeriodEndsBeforeStartFiscalYear_ReturnsPortionOfSecondPeriodOnly()
        {
            var tenYearsBeforeStart = FormulaBase.GetCalendarDateTime(ArbitraryStartYear - 10, 1);
            var tenYearsBeforeStartNullDuration =
                new SingleValueTimeVariantInputDTO(-1200, new TimePeriodDTO {DurationInMonths = null, StartTime = tenYearsBeforeStart});
            var firstPeriodCompleteBeforeStart = new[] {tenYearsBeforeStartNullDuration, SixMonthsBeforeStartYearNullDuration};
            
            var indexlessComparisonResults = BuildInterpolatePropagateComparisons(firstPeriodCompleteBeforeStart);
            Assert.That(indexlessComparisonResults.Item1, Is.EqualTo(indexlessComparisonResults.Item2));
            
            var indexDependentResult = InterpolatePropagateWithMonthIndex(firstPeriodCompleteBeforeStart, ArbitraryStartYear, FiveYearsInMonths, FunctionReturnsIndexPlusValue);
            var expected = Enumerable.Range(0, FiveYearsInMonths).Select(i => SixMonthsBeforeStartYearNullDuration.ValueAtIndex(i)).ToArray();            
            Assert.That(indexDependentResult, Is.EqualTo(expected));           
        }
        
        [Test]
        public void InterpolatePropagateWithMonthIndex_LastPeriodStartsAfterEndOfResultArray_ReturnsNull()
        {
            var tenYearsAfterEnd = FormulaBase.GetCalendarDateTime(ArbitraryStartYear + 10, 1).AddMonths(FiveYearsInMonths);
            var tenYearsAfterEndOneYearDuration =
                new SingleValueTimeVariantInputDTO(12000, new TimePeriodDTO {DurationInMonths = 12, StartTime = tenYearsAfterEnd});
            var lastPeriodStartsAfterResultsArray = new[] {StartYearNullDuration, tenYearsAfterEndOneYearDuration};
            
            var indexlessComparisonResults = BuildInterpolatePropagateComparisons(lastPeriodStartsAfterResultsArray);
            Assert.That(indexlessComparisonResults.Item1, Is.EqualTo(indexlessComparisonResults.Item2));
            
            var indexDependentResult = InterpolatePropagateWithMonthIndex(lastPeriodStartsAfterResultsArray, ArbitraryStartYear, FiveYearsInMonths, FunctionReturnsIndexPlusValue);
            var expected = Enumerable.Range(0, FiveYearsInMonths).Select(i => StartYearNullDuration.ValueAtIndex(i)).ToArray();            
            Assert.That(indexDependentResult, Is.EqualTo(expected));
        }  
        
        [Test]
        public void InterpolatePropagateWithMonthIndex_PeriodsCompletelyAfterResultArray_ReturnsNull()
        {
            var tenYearsAfterEnd = FormulaBase.GetCalendarDateTime(ArbitraryStartYear + 10, 1).AddMonths(FiveYearsInMonths);
            var tenYearsAfterEndOneYearDuration =
                new SingleValueTimeVariantInputDTO(12000, new TimePeriodDTO {DurationInMonths = 12, StartTime = tenYearsAfterEnd});
            var singlePeriodStartsAfterResultsArray = new[] {tenYearsAfterEndOneYearDuration};
            
            var indexlessComparisonResults = BuildInterpolatePropagateComparisons(singlePeriodStartsAfterResultsArray);
            Assert.That(indexlessComparisonResults.Item1, Is.EqualTo(indexlessComparisonResults.Item2));
            
            var indexDependentResult = InterpolatePropagateWithMonthIndex(singlePeriodStartsAfterResultsArray, ArbitraryStartYear, FiveYearsInMonths, FunctionReturnsIndexPlusValue);
            Assert.That(indexDependentResult, Is.Null);   
        }        

        [Test]
        public void InterpolatePropagateWithMonthIndex_EarlyStartOneYearDuration_StartsAtIndex0HalfYearData()
        {
            var sixMonthsBeforeStart = FormulaBase.GetCalendarDateTime(ArbitraryStartYear - 1, 7);
            var sixMonthsBeforeStartYearOneYearDuration =
                new SingleValueTimeVariantInputDTO(-50.0, new TimePeriodDTO {DurationInMonths = 12, StartTime = sixMonthsBeforeStart});
            var singlePeriodStartsEarly = new[] {sixMonthsBeforeStartYearOneYearDuration};

            var indexlessComparisonResults = BuildInterpolatePropagateComparisons(singlePeriodStartsEarly);
            Assert.That(indexlessComparisonResults.Item1, Is.EqualTo(indexlessComparisonResults.Item2));
            
            var indexDependentResult = InterpolatePropagateWithMonthIndex(singlePeriodStartsEarly, ArbitraryStartYear, FiveYearsInMonths, FunctionReturnsIndexPlusValue);
            var expected = Enumerable.Range(0, 6).Select(i => sixMonthsBeforeStartYearOneYearDuration.ValueAtIndex(i)).
                Concat(Enumerable.Repeat((double?) null, FiveYearsInMonths - 6)).ToArray();            
            Assert.That(indexDependentResult, Is.EqualTo(expected));
        }
        
        [Test]
        public void InterpolatePropagateWithMonthIndex_EarlyStartNullDuration_StartsAtIndex0()
        {
            var singlePeriodStartsEarly = new[] {SixMonthsBeforeStartYearNullDuration};

            var indexlessComparisonResults = BuildInterpolatePropagateComparisons(singlePeriodStartsEarly);
            Assert.That(indexlessComparisonResults.Item1, Is.EqualTo(indexlessComparisonResults.Item2));
            
            var indexDependentResult = InterpolatePropagateWithMonthIndex(singlePeriodStartsEarly, ArbitraryStartYear, FiveYearsInMonths, FunctionReturnsIndexPlusValue);
            var expected = Enumerable.Range(0, FiveYearsInMonths).Select(i => SixMonthsBeforeStartYearNullDuration.ValueAtIndex(i)).ToArray();            
            Assert.That(indexDependentResult, Is.EqualTo(expected));
        }

        [Test]
        public void InterpolatePropagateWithMonthIndex_AlignedWithStart_StartsAtIndex0()
        {
            var singlePeriodStartOnStartFiscalYear = new[] {StartYearNullDuration};
            
            var indexlessComparisonResults = BuildInterpolatePropagateComparisons(singlePeriodStartOnStartFiscalYear);
            Assert.That(indexlessComparisonResults.Item1, Is.EqualTo(indexlessComparisonResults.Item2));
            
            var indexDependentResult = InterpolatePropagateWithMonthIndex(singlePeriodStartOnStartFiscalYear, ArbitraryStartYear, FiveYearsInMonths, FunctionReturnsIndexPlusValue);
            var expected = Enumerable.Range(0, FiveYearsInMonths).Select(i => StartYearNullDuration.ValueAtIndex(i)).ToArray();            
            Assert.That(indexDependentResult, Is.EqualTo(expected));            
        }
        
        [Test]
        public void InterpolatePropagateWithMonthIndex_LateStart_StartsAtStartTime()
        {
            var singlePeriodStartsAfterStartFiscalYear = new[] {SixMonthsAfterStartYearNullDuration};
            
            var indexlessComparisonResults = BuildInterpolatePropagateComparisons(singlePeriodStartsAfterStartFiscalYear);
            Assert.That(indexlessComparisonResults.Item1, Is.EqualTo(indexlessComparisonResults.Item2));
            
            var indexDependentResult = InterpolatePropagateWithMonthIndex(singlePeriodStartsAfterStartFiscalYear, ArbitraryStartYear, FiveYearsInMonths, FunctionReturnsIndexPlusValue);
            var expected = Enumerable.Repeat((double?) null, 6).
                Concat(Enumerable.Range(6, FiveYearsInMonths - 6).Select(i => SixMonthsAfterStartYearNullDuration.ValueAtIndex(i))).
                ToArray();            
            Assert.That(indexDependentResult, Is.EqualTo(expected));            
        }
        
        [Test]
        public void InterpolatePropagateWithMonthIndex_PeriodsWithGap_SkipsGap()
        {
            var withGaps = new[] {StartYearOneYearDuration, ThirdYearOneYearDuration};
            
            var indexlessComparisonResults = BuildInterpolatePropagateComparisons(withGaps);
            Assert.That(indexlessComparisonResults.Item1, Is.EqualTo(indexlessComparisonResults.Item2));
            
            var indexDependentResult = InterpolatePropagateWithMonthIndex(withGaps, ArbitraryStartYear, FiveYearsInMonths, FunctionReturnsIndexPlusValue);
            var expected = Enumerable.Range(0, 12).Select(i => StartYearOneYearDuration.ValueAtIndex(i)).
                Concat(Enumerable.Repeat((double?)null, 12)).
                Concat(Enumerable.Range(24, 12).Select(i => ThirdYearOneYearDuration.ValueAtIndex(i))).
                Concat(Enumerable.Repeat((double?)null, FiveYearsInMonths - 36)).
                ToArray();            
            Assert.That(indexDependentResult, Is.EqualTo(expected)); 
        }        
        
        [Test]
        public void InterpolatePropagateWithMonthIndex_NullDurationFollowedByAnotherItem_FillsGap()
        {
            var withGaps = new[] {StartYearNullDuration, ThirdYearOneYearDuration};
            
            var indexlessComparisonResults = BuildInterpolatePropagateComparisons(withGaps);
            Assert.That(indexlessComparisonResults.Item1, Is.EqualTo(indexlessComparisonResults.Item2));
            
            var indexDependentResult = InterpolatePropagateWithMonthIndex(withGaps, ArbitraryStartYear, FiveYearsInMonths, FunctionReturnsIndexPlusValue);
            var expected = Enumerable.Range(0, 24).Select(i => StartYearNullDuration.ValueAtIndex(i)).
                Concat(Enumerable.Range(24, 12).Select(i => ThirdYearOneYearDuration.ValueAtIndex(i))).
                Concat(Enumerable.Repeat((double?)null, FiveYearsInMonths - 36)).
                ToArray();            
            Assert.That(indexDependentResult, Is.EqualTo(expected));         }
        
        [Test]
        public void InterpolatePropagateWithMonthIndex_ZeroDurationFollowedByAnotherItem_FillsGap()
        {
            var nullDurationAtEnd = new[] {StartYearZeroDuration, ThirdYearOneYearDuration};
            
            var indexlessComparisonResults = BuildInterpolatePropagateComparisons(nullDurationAtEnd);
            Assert.That(indexlessComparisonResults.Item1, Is.EqualTo(indexlessComparisonResults.Item2));
            
            var indexDependentResult = InterpolatePropagateWithMonthIndex(nullDurationAtEnd, ArbitraryStartYear, FiveYearsInMonths, FunctionReturnsIndexPlusValue);
            var expected = Enumerable.Range(0, 24).Select(i => StartYearZeroDuration.ValueAtIndex(i)).
                Concat(Enumerable.Range(24, 12).Select(i => ThirdYearOneYearDuration.ValueAtIndex(i))).
                Concat(Enumerable.Repeat((double?)null, FiveYearsInMonths - 36)).
                ToArray();            
            Assert.That(indexDependentResult, Is.EqualTo(expected)); 
        }
        
        [Test]
        public void InterpolatePropagateWithMonthIndex_EarlyEnd_OnlyFillsTillEndOfPeriod()
        {
            var earlyEnd = new[] {StartYearOneYearDuration, SecondYearOneYearDuration};
            
            var indexlessComparisonResults = BuildInterpolatePropagateComparisons(earlyEnd);
            Assert.That(indexlessComparisonResults.Item1, Is.EqualTo(indexlessComparisonResults.Item2));
            
            var indexDependentResult = InterpolatePropagateWithMonthIndex(earlyEnd, ArbitraryStartYear, FiveYearsInMonths, FunctionReturnsIndexPlusValue);
            var expected = Enumerable.Range(0, 12).Select(i => StartYearOneYearDuration.ValueAtIndex(i)).
                Concat(Enumerable.Range(12, 12).Select(i => SecondYearOneYearDuration.ValueAtIndex(i))).
                Concat(Enumerable.Repeat((double?)null, FiveYearsInMonths - 24)).
                ToArray();            
            Assert.That(indexDependentResult, Is.EqualTo(expected));
        }
        
        [Test]
        public void InterpolatePropagateWithMonthIndex_LateEnd_OnlyFillsTillEndOfArray()
        {
            var lateEnd = new[] {StartYearOneYearDuration, SecondYearTenYearDuration};
            
            var indexlessComparisonResults = BuildInterpolatePropagateComparisons(lateEnd);
            Assert.That(indexlessComparisonResults.Item1, Is.EqualTo(indexlessComparisonResults.Item2));
            
            var indexDependentResult = InterpolatePropagateWithMonthIndex(lateEnd, ArbitraryStartYear, FiveYearsInMonths, FunctionReturnsIndexPlusValue);
            var expected = Enumerable.Range(0, 12).Select(i => StartYearOneYearDuration.ValueAtIndex(i)).
                Concat(Enumerable.Range(12, FiveYearsInMonths - 12).Select(i => SecondYearTenYearDuration.ValueAtIndex(i))).
                ToArray();            
            Assert.That(indexDependentResult, Is.EqualTo(expected));
        }        
        
        [Test]
        public void InterpolatePropagateWithMonthIndex_NullDurationAtEnd_FillsTillEnd()
        {
            var nullDurationAtEnd = new[] {StartYearOneYearDuration, SecondYearNullDuration};
            
            var indexlessComparisonResults = BuildInterpolatePropagateComparisons(nullDurationAtEnd);
            Assert.That(indexlessComparisonResults.Item1, Is.EqualTo(indexlessComparisonResults.Item2));
            
            var indexDependentResult = InterpolatePropagateWithMonthIndex(nullDurationAtEnd, ArbitraryStartYear, FiveYearsInMonths, FunctionReturnsIndexPlusValue);
            var expected = Enumerable.Range(0, 12).Select(i => StartYearOneYearDuration.ValueAtIndex(i)).
                Concat(Enumerable.Range(12, FiveYearsInMonths - 12).Select(i => SecondYearNullDuration.ValueAtIndex(i))).
                ToArray();            
            Assert.That(indexDependentResult, Is.EqualTo(expected));
        }
        
        [Test]
        public void InterpolatePropagateWithMonthIndex_ZeroDurationAtEnd_FillsTillEnd()
        {
            var zeroDurationAtEnd = new[] {StartYearOneYearDuration, SecondYearZeroDuration};
            
            var indexlessComparisonResults = BuildInterpolatePropagateComparisons(zeroDurationAtEnd);
            Assert.That(indexlessComparisonResults.Item1, Is.EqualTo(indexlessComparisonResults.Item2));
                       
            var indexDependentResult = InterpolatePropagateWithMonthIndex(zeroDurationAtEnd, ArbitraryStartYear, FiveYearsInMonths, FunctionReturnsIndexPlusValue);
            var expected = Enumerable.Range(0, 12).Select(i => StartYearOneYearDuration.ValueAtIndex(i)).
                Concat(Enumerable.Range(12, FiveYearsInMonths - 12).Select(i => SecondYearZeroDuration.ValueAtIndex(i))).
                ToArray();             
            Assert.That(indexDependentResult, Is.EqualTo(expected));
        }        
    }
}
