using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using CL.FormulaHelper;
using CL.FormulaHelper.DTOs;

namespace MeasureFormula.TestHelpers
{
    public static class DataPrep
    {
        private const int DayIndexGreaterThanMonthCount = 15;
        public enum ZynosTestType
        {
            NoValidZynosConversion,
            DollarToZynosConversion,
            ZynosOutputMatchesGetUnits
        }

        public static double MakeArbitraryValidNonZeroProbability(Fixture fixture)
        {
            return MakeArbitraryPercentageBetween1And100(fixture) / 100.0;
        }

        public static double MakeArbitraryPercentageBetween1And100(Fixture fixture)
        {
            return 1 + fixture.Create<int>() % 100;
        }

        public static double?[] BuildPartiallyPopulatedArray(int size, int startIndex, double? value)
        {
            var arr = new double?[size];

            var indexFirstNonNullValue = Math.Max(0, startIndex);
            for (var i = indexFirstNonNullValue; i < size; i++)
            {
                arr[i] = value;
            }

            return arr;
        }
        
        public static DistributionByAccountTypeDTO MakeDistributionWithSpendAtOffset(int endSpendOffset, double spendAmount)
        {
            var spendDictionary = new Dictionary<int, double> {{endSpendOffset, spendAmount}};
            var spendDto = new AccountTypeSpendDTO(1, "ignoredName", true, 1, spendDictionary);
            var spendDtoList = new List<AccountTypeSpendDTO> {spendDto};
            return new DistributionByAccountTypeDTO {AccountSpendValues = spendDtoList};
        }
        
        public static DistributionByAccountTypeDTO MakeDistributionDTO(string spendAccountName, int startSpendOffset, double[] spendAmount)
        {
            var spendDictionary = new Dictionary<int, double>();
            for(var index = 0; index < spendAmount.Length; index++)
            {
                spendDictionary.Add(index + startSpendOffset, spendAmount[index]);
            }
            var spendDto = new AccountTypeSpendDTO(1, spendAccountName, true, 1, spendDictionary);
            var spendDtoList = new List<AccountTypeSpendDTO> {spendDto};
            return new DistributionByAccountTypeDTO {AccountSpendValues = spendDtoList};
        }
        
        public static TimeSeriesDTO CreateConstantTimeSeries(double value)
        {
            return new TimeSeriesDTO
            {
                OffsetType = TimeSeriesDTO.TimeSeriesOffsetType.AbsoluteFiscalYearly,
                BaseYear = 2018,    // because time series extrapolate in both directions, for a constant time series, the base year does not matter
                ValuesDoubleArray = new  [] {value}
            };
        }
        
        public static TimeSeriesDTO CreateRelativeRandomTimeSeriesDto(TimeSeriesDTO.TimeSeriesOffsetType timeSeriesType, int numberOfValues)
        {
            if (timeSeriesType == TimeSeriesDTO.TimeSeriesOffsetType.AbsoluteCalendarYearly ||
                timeSeriesType == TimeSeriesDTO.TimeSeriesOffsetType.AbsoluteCalendarMonthly ||
                timeSeriesType == TimeSeriesDTO.TimeSeriesOffsetType.AbsoluteFiscalYearly)
            {
                throw new InvalidEnumArgumentException();
            }

            return CreateRandomTimeSeriesDto(timeSeriesType, numberOfValues, baseYear:null);
        }

        public static TimeSeriesDTO CreateRandomTimeSeriesDto(TimeSeriesDTO.TimeSeriesOffsetType timeSeriesType, int numberOfValues, int? baseYear)
        {
            var randomGenerator = new Random();

            var valuesArray = new double[numberOfValues].Select(x => randomGenerator.NextDouble()).ToArray();

            var timeseries = new TimeSeriesDTO()
            {
                OffsetType = timeSeriesType,
                BaseYear = baseYear,
                ValuesDoubleArray = valuesArray
            };

            return timeseries;
        }

        public static void SetConstructorParameter<T>(Fixture fixture, string propertyName, T value)
        {
            var valueType = value == null ? typeof(T) : value.GetType();
            fixture.Customizations.Insert(0, new FilteringSpecimenBuilder(new FixedBuilder(value),
                new ParameterSpecification(valueType, propertyName)));
        }
        
        public static void UpdateDto(object dto, string propertyName, object targetValue)
        {
            var property = dto.GetType().GetProperty(propertyName);
            var canChangeProperty = property != null;
            if (!canChangeProperty)
            {
                var message = string.Format("Could not get {0} property for type: {1}", propertyName, dto.GetType().FullName);
                throw new InvalidOperationException(message);
            }
            
            var currentValue = property.GetValue(dto);
            if (targetValue == null && currentValue == null)
            {
                return;
            }

            var valueType = currentValue == null ? targetValue.GetType() : currentValue.GetType();
            var desiredValue = targetValue == null ? null : Convert.ChangeType(targetValue, valueType);
            property.SetValue(dto, desiredValue);
        }

        public static void SetPropertyToNull<T>(T dto, string property)
        {
            UpdateDto(dto, property, null);
        }

        /// <summary>
        /// Builds a consistent array of TimePeriodDTOs.  If startYear is set to the same start year passed to GetUnits then this
        /// data will produce nulls until the specified offsetInMonthArray
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="repeatCount">Number of TimePeriodDTOs to build.  Last one will have null as its Duration</param>
        /// <param name="startYear"></param>
        /// <param name="offsetInMonthArray">0 based, will correspond to an offset in the months array if startYear matches
        /// the one used in the call to GetUnits</param>
        public static TimePeriodDTO[] BuildContiguousTimePeriodsStartYearAndOffset(Fixture fixture, int repeatCount,
            int startYear, int offsetInMonthArray)
        {
            var startTime = FormulaBase.GetCalendarDateTime(startYear, offsetInMonthArray + 1).AddDays(DayIndexGreaterThanMonthCount);
            return BuildContiguousTimePeriodsFromDateTime(fixture, repeatCount, startTime);
        }

        public static TimePeriodDTO[] BuildContiguousTimePeriodsFromStartYearAndMonthIndex(int repeatCount, int startYear,
            int startMonth, Fixture fixture)
        {
            var startTime = new DateTime(startYear, startMonth, DayIndexGreaterThanMonthCount);
            return BuildContiguousTimePeriodsFromDateTime(fixture, repeatCount, startTime);
        }

        public static TimePeriodDTO[] BuildContiguousTimePeriodsFromDateTime(Fixture fixture, int repeatCount, DateTime startTime)
        { 
            var result = new List<TimePeriodDTO>();

            const int maxNumMonthsInPeriod = 600;
            for (var index = 0; index < repeatCount; index++)
            {
                var isLastPeriod = index == repeatCount - 1;
                var nextDurationInMonths = 1 + fixture.Create<int>() % maxNumMonthsInPeriod;
                var nextDto = new TimePeriodDTO
                {
                    DurationInMonths = isLastPeriod ? (int?) null : nextDurationInMonths,
                    StartTime = startTime
                };
                result.Add(nextDto);
                startTime = startTime.AddMonths(nextDurationInMonths);
            }
            return result.ToArray();
        }

        public static T[] BuildTimeVariantData<T>(Fixture fixture)
        {
            return BuildTimeVariantData<T>(fixture, 2010);
        }
        
        public static T[] BuildTimeVariantData<T>(Fixture fixture, int earliestStartYear)
        {
            const int periods = 3;
            var randomStartYearWithin8YearsOfEarliestStartYear = earliestStartYear + fixture.Create<int>() % 8;

            const int monthOffsetFromFiscalYearEnd = 3;
            var timePeriodsStartingBeforeToday = BuildContiguousTimePeriodsStartYearAndOffset(fixture,
                                                                                              periods,
                                                                                              randomStartYearWithin8YearsOfEarliestStartYear,
                                                                                              monthOffsetFromFiscalYearEnd);
            fixture.Customizations.Insert(0, new TimePeriodExtractor(timePeriodsStartingBeforeToday));

            var generatedTimeVariantDto = fixture.CreateMany<T>(periods).ToArray();
            return generatedTimeVariantDto;
        }
        
        public static object BuildTimeInvariantData(ISpecimenBuilder fixture, Type timeInvariantInputDtoType)
        {
            var fixtureContext = new SpecimenContext(fixture);

            return fixtureContext.Resolve(timeInvariantInputDtoType);
        }
        
        public static NumericValueRangeDictDTO BuildConsequenceLevelsWithSpecifiedLengthAndValue(Fixture fixture, int length, double value)
        {
            var dictionary = new Dictionary<long, NumericValueRangeDTO>();
            for (var key = 1; key <= length; key++)
            {
                var numericValueRangeDto = fixture.Build<NumericValueRangeDTO>()
                    .With(x => x.AvgValue, value + length - key)
                    .With(x => x.Code, key.ToString())
                    .Create();
                dictionary.Add(key, numericValueRangeDto);
            }

            return fixture.Build<NumericValueRangeDictDTO>()
                .With(x => x.Dict, dictionary)
                .Create();
        }
    }
}
