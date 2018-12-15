using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MeasureFormula.TestHelpers
{
    public class NullablePropertyCheck
    {
        private bool CheckForCustomFieldNullability;

        readonly List<ResultDto> TestResults = new List<ResultDto>();

        // Many formulas assume that composite objects (typically Custom Field Lists) are not null based on
        // their definition in the Measure Prompt Var tab of the loader. This checker doesn't have access to that
        // information, so it would blindly iterate through all composite objects, causing false positive failures.
        // Use the RunNullTests launcher appropriate to whether your formulas address all nullable objects.
        public void RunNullTestsIncludingCustomFields(object timeInvariantDto, IEnumerable timeVariantData, Func<object, object, double?[]> getUnitsFunc)
        {
            CheckForCustomFieldNullability = true;
            RunNullTests(timeInvariantDto, timeVariantData, getUnitsFunc);
        }
        public void RunNullTestsExcludingCustomFields(object timeInvariantDto, IEnumerable timeVariantData, Func<object, object, double?[]> getUnitsFunc)
        {
            CheckForCustomFieldNullability = false;
            RunNullTests(timeInvariantDto, timeVariantData, getUnitsFunc);
        }

        private void RunNullTests(object timeInvariantDto, IEnumerable timeVariantData, Func<object, object, double?[]> getUnitsFunc)
        {
            Func<object, double?[]> getUnitsWithoutVariantArgument = x => getUnitsFunc(timeInvariantDto, x);
            IterateThroughVariantProperties(timeVariantData, getUnitsWithoutVariantArgument);

            Func<object, double?[]> getUnitsWithoutInvariantArgument = x => getUnitsFunc(x, timeVariantData);
            IterateThroughTimeInvariantProperties(timeInvariantDto, getUnitsWithoutInvariantArgument);
            OutputResults();
        }

        void IterateThroughVariantProperties(IEnumerable timeVariantDto, Func<object, double?[]> getUnitsFunc)
        {
            var timeVariantDtoArray = timeVariantDto.Cast<object>().ToArray();

            for (int period = 0; period < timeVariantDtoArray.Length; period++)
            {
                Type dtoType = timeVariantDtoArray[period].GetType();
                foreach (var propertyInfo in dtoType.GetProperties())
                {
                    Type propertyType = propertyInfo.PropertyType;

                    if (CanPropertyBeNulled(propertyType))
                    {
                        var testResult = DescribeResultDto(propertyInfo, timeVariantDtoArray[period], period);
                        var backupPropertyValue = NullThisProperty(timeVariantDtoArray[period], propertyInfo.Name);

                        RunGetUnitTest(timeVariantDto, testResult, getUnitsFunc);

                        RestorePropertyValue(timeVariantDtoArray[period], propertyInfo.Name, backupPropertyValue);
                    }
                }
            }
        }

        ResultDto DescribeResultDto(PropertyInfo propertyInfo, object dto, int? period = null)
        {
            char[] trimCharactersToRemove = { ']' };
            var dtoTrimmed = dto.ToString().Split('+').Last().Trim(trimCharactersToRemove);

            var dtoDisplayName = IsDtoTimeVariant(period) 
                                        ? string.Format("{0}\tPeriod: {1}", dtoTrimmed, period) 
                                        : dto.ToString().Split('+').Last().Trim(trimCharactersToRemove);

            var testResult = new ResultDto() { Property = propertyInfo.Name, Dto = dtoDisplayName };
            return testResult;
        }

        bool IsDtoTimeVariant(int? period)
        {
            if (period != null)
                return true;
            return false;
        }

        bool CanPropertyBeNulled(Type propertyType)
        {
            var propertyCanBeNulled = propertyType.IsGenericType &&
                propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            if (CheckForCustomFieldNullability)
                propertyCanBeNulled = propertyCanBeNulled ||
                                      propertyType.IsClass && propertyType.Name != "TimePeriodDTO";
            return propertyCanBeNulled;
        }

        object NullThisProperty(object dto, string property)
        {
            var backupPropertyValue = GetPropertyValue(dto, property);
            DataPrep.SetPropertyToNull(dto, property);
            return backupPropertyValue;
        }

        object GetPropertyValue(object dto, string propertyName)
        {
            var property = dto.GetType().GetProperty(propertyName);
            var foundProperty = property != null;
            if (!foundProperty)
            {
                var message = string.Format("Could not get {0} property for type: {1}", propertyName, dto.GetType().FullName);
                throw new InvalidOperationException(message);
            }

            var currentValue = property.GetValue(dto);
            return currentValue;
        }

        void RestorePropertyValue(object dto, string property, object value)
        {
            DataPrep.UpdateDto(dto, property, value);
        }

        void IterateThroughTimeInvariantProperties(object dto, Func<object, double?[]> getUnitsFunc)
        {
            Type dtoType = dto.GetType();

            foreach (var propertyInfo in dtoType.GetProperties())
            {
                Type propertyType = propertyInfo.PropertyType;

                if (CanPropertyBeNulled(propertyType))
                {
                    var testResult = DescribeResultDto(propertyInfo, dto);
                    var backupPropertyValue = NullThisProperty(dto, propertyInfo.Name);

                    RunGetUnitTest(dto, testResult, getUnitsFunc);

                    RestorePropertyValue(dto, propertyInfo.Name, backupPropertyValue);
                }
            }
        }

        void RunGetUnitTest(object dto, ResultDto testResult, Func<object, double?[]> getUnitsFunc)
        {
            try
            {
                getUnitsFunc(dto);
                testResult.TestPassed = true;
            }
            catch (Exception ex)
            {
                testResult.TestPassed = false;
                testResult.FailureMessage = "Exception thrown " + ex.Message;
            }
            TestResults.Add(testResult);
        }

        private void OutputResults()
        {
            string outputMessage = "";
            bool anyFailures = false;
            foreach (var testResult in TestResults)
            {
                outputMessage += string.Format("{0}...\n", testResult.Dto);
                outputMessage +=
                    string.Format("Property: {0}\t{1}", testResult.Property, testResult.TestPassed ? "Passed" : "Failed");
                if (!testResult.TestPassed)
                {
                    outputMessage += string.Format("\tException: {0}\n", testResult.FailureMessage);
                    anyFailures = true;
                }
                else outputMessage += "\n";
            }

            if (anyFailures) throw new MeasureFormulaTestException("Not all properties settable to null\n" + outputMessage);
        }

        class ResultDto
        {
            public string Dto;
            public string Property;
            public bool TestPassed;
            public string FailureMessage;
        }
    }
}
