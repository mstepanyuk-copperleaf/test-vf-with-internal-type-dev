using System;
using System.Linq;
using AutoFixture;
using MeasureFormula.SharedCode;
using NUnit.Framework;
using MeasureFormula.TestHelpers;


namespace MeasureFormula.Tests.SharedCodeTest
{
    public class ArrayHelperTests
    {
        private static readonly Fixture fixture = new Fixture();
        private static readonly double value1 = fixture.Create<double>();
        private static readonly double value2 = fixture.Create<double>();
        
        [Test]
        public void WhenArraysAreSameLength_MultiplyArrays_MultipliesEntries()
        {
            var firstArray = new[] {(double?)value1, value1};
            var secondArray = new[] {(double?) value2, value2};

            var multipliedArray = ArrayHelper.MultiplyArrays(firstArray, secondArray);

            var expectedArray = new[] {(double?) value1 * value2, value1 * value2};
            Assert.That(multipliedArray, Is.EqualTo(expectedArray).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void WhenArraysAreSameLength_SumArrays_AddsEntries()
        {
            var firstArray = new[] {(double?)value1, value1};
            var secondArray = new[] {(double?) value2, value2};
            var arrayPair = new[] {firstArray, secondArray};

            var summedArray = ArrayHelper.SumArrays(arrayPair);

            var expectedArray = new[] {(double?) value1 + value2, value1 + value2};
            Assert.That(summedArray, Is.EqualTo(expectedArray).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void WhenNullEntry_MultiplyArrays_ProducesNullInCorrespondingEntry()
        {
            var firstArray = new[] {(double?)value1, value1};
            var secondArray = new[] {(double?) value2, null};

            var multipliedArray = ArrayHelper.MultiplyArrays(firstArray, secondArray);

            var expectedArray = new[] {(double?) value1 * value2, null};
            Assert.That(multipliedArray, Is.EqualTo(expectedArray).Within(CommonConstants.DoubleDifferenceTolerance)); 
            
            var reverseMultipliedArray = ArrayHelper.MultiplyArrays(secondArray, firstArray);
            Assert.That(reverseMultipliedArray, Is.EqualTo(expectedArray).Within(CommonConstants.DoubleDifferenceTolerance)); 
        }
        
        [Test]
        public void WhenNullEntry_SumArray_TreatsItAsZero()
        {
            var firstArray = new[] {(double?)value1, value1};
            var secondArray = new[] {(double?) value2, null};
            var arrayPair = new[] {firstArray, secondArray};

            var summedArray = ArrayHelper.SumArrays(arrayPair);

            var expectedArray = new[] {(double?) value1 + value2, value1};
            Assert.That(summedArray, Is.EqualTo(expectedArray).Within(CommonConstants.DoubleDifferenceTolerance)); 
            
            var reversedArrayPair = new[] {secondArray, firstArray};
            var reverseSummedArray = ArrayHelper.SumArrays(reversedArrayPair);
            Assert.That(reverseSummedArray, Is.EqualTo(expectedArray).Within(CommonConstants.DoubleDifferenceTolerance)); 
        }
        
        // handles arrays of different lengths
        [Test]
        public void WhenArraysAreDifferentLengths_MultiplyArrays_ReturnsNull()
        {
            var firstArray = new[] {(double?)value1, value1};
            var secondArray = new[] {(double?) value2};

            var multipliedArray = ArrayHelper.MultiplyArrays(firstArray, secondArray);

            Assert.That(multipliedArray, Is.Null); 
            
            var reverseMultipliedArray = ArrayHelper.MultiplyArrays(secondArray, firstArray);
            Assert.That(reverseMultipliedArray, Is.Null); 
        }

        [Test]
        public void WhenArraysAreDifferentLengths_SumArrays_ReturnsNull()
        {
            var firstArray = new[] {(double?)value1, value1};
            var secondArray = new[] {(double?) value2};
            var arrayPair = new[] {firstArray, secondArray};

            var summedArray = ArrayHelper.SumArrays(arrayPair);

            Assert.That(summedArray, Is.Null); 
            
            var reversedArrayPair = new[] {secondArray, firstArray};
            var reverseSummedArray = ArrayHelper.SumArrays(reversedArrayPair);
            Assert.That(reverseSummedArray, Is.Null); 
        }
        
        // handles arrays with null values
        [Test]
        public void MultiplyArrayByTimeSeries_ArrayContainsNullValues_MultipliesArrayWithTImeSeries()
        {
            var firstArray = new double?[] {1,null,34};
            var constantValue = 2;
            var timeArray = DataPrep.CreateConstantTimeSeries(constantValue);
            var startFiscalYear = 2018;
           
            var multipliedArray = ArrayHelper.MultiplyArrayByTimeSeries(firstArray, timeArray , startFiscalYear );

            var expectedArray = firstArray.Select(x => x * constantValue).ToArray();
            Assert.That(multipliedArray, Is.EqualTo(expectedArray).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        // handles different start years
        [Test]
        public void MultiplyArrayByTimeSeries_HandlesDifferentFY_MultipliesArrayByTimeSeries()
        {
            var firstArray = new double?[] {1,null,34};
            var constantValue = 2;
            var timeArray = DataPrep.CreateConstantTimeSeries(constantValue);
            Random randomYear = new Random();
            int FiscalYear = randomYear.Next(1900,2025);
           
            var multipliedArray = ArrayHelper.MultiplyArrayByTimeSeries(firstArray, timeArray , FiscalYear );

            var expectedArray = firstArray.Select(x => x * constantValue).ToArray();
            Assert.That(multipliedArray, Is.EqualTo(expectedArray).Within(CommonConstants.DoubleDifferenceTolerance));
        }

        // handles null constants
        [Test]
        public void MultipyStreamOfValuesByConstant_WhenConstantIsNull_ReturnsNull()
        {
            var firstArray = new[] {(double?)value1};
            double? nullConstant =  null;

            var multipliedArray = ArrayHelper.MultiplyStreamOfValuesByConstant(firstArray, nullConstant);

            Assert.That(multipliedArray, Is.Null); 
        }
        
        // handles array containing null entries
        [Test]
        public void MultipyStreamOfValuesByConstant_WhenArrayContainsNull_ReturnsArrayWithNull()
        {
            var firstArray = new double?[] {4, null, 9};
            var constantMultiplier = 5;

            var multipliedArray = ArrayHelper.MultiplyStreamOfValuesByConstant(firstArray, constantMultiplier);
            
            var expectedArray = firstArray.Select(x => x * constantMultiplier).ToArray();
            Assert.That(multipliedArray, Is.EqualTo(expectedArray).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
    }
}
