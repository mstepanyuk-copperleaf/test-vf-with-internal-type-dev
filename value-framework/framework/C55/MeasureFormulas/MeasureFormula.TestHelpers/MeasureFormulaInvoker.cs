using System;
using System.Collections;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.Kernel;

namespace MeasureFormula.TestHelpers
{
    public static class MeasureFormulaInvoker
    {
        public static double?[] InvokeGetUnits(
            ISpecimenBuilder fixture,
            int startFiscalYear,
            int size,
            Type timeInvariantInputDtoType,
            Type timeVariantInputDtoType,
            Type consequenceClassType)
        {
            var consequenceClass = Activator.CreateInstance(consequenceClassType);
            var timeInvariantData = new SpecimenContext(fixture).Resolve(timeInvariantInputDtoType);
            var timeVariantData = new SpecimenContext(fixture).Resolve(timeVariantInputDtoType);

            var genericListType = typeof(List<>).MakeGenericType(timeVariantInputDtoType);
            var timeVariantList = (IList)Activator.CreateInstance(genericListType);
            timeVariantList.Add(timeVariantData);

            var method = consequenceClass.GetType().GetMethod("GetUnits");
            object[] args = { startFiscalYear, size, timeInvariantData, timeVariantList };
            return method == null ? null : (double?[])method.Invoke(consequenceClass, args);
        }

        public static void RunGetZynosTest(Type consequenceClassType, DataPrep.ZynosTestType testType, double dollarToZynoConversionFactor, Action<object, object> resultChecker)
        {
            var fixture = new Fixture();
            const int maxNumMonths = 600;
            var returnArraySize = fixture.Create<int>() % maxNumMonths;
            var startMonth = fixture.Create<int>() % returnArraySize;
            var arrayValue = fixture.Create<double>();
            var startFiscalYear = 2017 + fixture.Create<int>() % 50;

            var getUnitsValues = DataPrep.BuildPartiallyPopulatedArray(returnArraySize, startMonth, arrayValue);
            var results = InvokeGetZynos(startFiscalYear, returnArraySize, consequenceClassType, getUnitsValues);

            double?[] expectedResults;

            switch (testType)
            {
                case DataPrep.ZynosTestType.ZynosOutputMatchesGetUnits:
                    expectedResults = getUnitsValues;
                    break;
                case DataPrep.ZynosTestType.DollarToZynosConversion:
                    expectedResults = DataPrep.BuildPartiallyPopulatedArray(returnArraySize, startMonth,
                        arrayValue * dollarToZynoConversionFactor);
                    break;
                case DataPrep.ZynosTestType.NoValidZynosConversion:
                    expectedResults = null;
                    break;
                default:
                    throw new MeasureFormulaTestException("Invalid ZynosTestType");
            }

            resultChecker(results, expectedResults);
        }
        
        public static double?[] InvokeGetZynos(
            int startFiscalYear,
            int size,
            Type consequenceClassType,
            double?[] unitOutput)
        {
            var consequenceClass = Activator.CreateInstance(consequenceClassType);
            var method = consequenceClass.GetType().GetMethod("GetZynos");
            object[] args = { startFiscalYear, size, null, null, unitOutput };
            return method == null ? null : (double?[])method.Invoke(consequenceClass, args);
        }
        
        public static double?[] InvokeGetZynos(
            ISpecimenBuilder fixture,
            int startFiscalYear,
            int size,
            Type timeInvariantInputDtoType,
            Type timeVariantInputDtoType,
            Type consequenceClassType,
            double?[] unitOutput)
        {
            var consequenceClass = Activator.CreateInstance(consequenceClassType);
            var timeInvariantData = new SpecimenContext(fixture).Resolve(timeInvariantInputDtoType);
            var timeVariantData = new SpecimenContext(fixture).Resolve(timeVariantInputDtoType);

            var genericListType = typeof(List<>).MakeGenericType(timeVariantInputDtoType);
            var timeVariantList = (IList)Activator.CreateInstance(genericListType);
            timeVariantList.Add(timeVariantData);

            var method = consequenceClass.GetType().GetMethod("GetZynos");
            object[] args = { startFiscalYear, size, timeInvariantData, timeVariantList, unitOutput };
            return method == null ? null : (double?[])method.Invoke(consequenceClass, args);
        }
        
        public static double?[] InvokeGetLikelihoodValues(
            ISpecimenBuilder fixture,
            int startFiscalYear,
            int size,
            Type timeInvariantInputDtoType,
            Type timeVariantInputDtoType,
            Type consequenceClassType)
        {
            var consequenceClass = Activator.CreateInstance(consequenceClassType);
            var timeInvariantData = new SpecimenContext(fixture).Resolve(timeInvariantInputDtoType);
            var timeVariantData = new SpecimenContext(fixture).Resolve(timeVariantInputDtoType);

            var genericListType = typeof(List<>).MakeGenericType(timeVariantInputDtoType);
            var timeVariantList = (IList)Activator.CreateInstance(genericListType);
            timeVariantList.Add(timeVariantData);

            var method = consequenceClass.GetType().GetMethod("GetLikelihoodValues");
            object[] args = { startFiscalYear, size, timeInvariantData, timeVariantList };
            return method == null ? null : (double?[])method.Invoke(consequenceClass, args);
        }
    }
}
