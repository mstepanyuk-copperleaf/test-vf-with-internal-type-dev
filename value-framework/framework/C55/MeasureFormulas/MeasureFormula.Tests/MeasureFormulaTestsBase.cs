using AutoFixture;
using CL.FormulaHelper;
using NUnit.Framework;
using CL.FormulaHelper.DTOs;
using MeasureFormula.TestHelpers;

namespace MeasureFormula.Tests
{
    public class MeasureFormulaTestsBase
    {
        protected Fixture fixture;

        protected int ArbitraryStartYear;
        protected int ArbitraryMonths;

        protected TimePeriodDTO PeriodFromBeginningForAllTime;
        
        [SetUp]
        public void RunBeforeEachTest()
        {
            // Match what the customer uses
            FormulaBase.FiscalYearEndMonth = 12;
            fixture = new Fixture();
            
            ArbitraryStartYear = 1900 + fixture.Create<int>() % 100;
            ArbitraryMonths = 24 + fixture.Create<int>() % 20;
            
            PeriodFromBeginningForAllTime = new TimePeriodDTO {StartTime = FormulaBase.GetCalendarDateTime(ArbitraryStartYear, 1), DurationInMonths = null};
            DataPrep.SetConstructorParameter(fixture, "p_TimePeriod", PeriodFromBeginningForAllTime);
        }
    }
}