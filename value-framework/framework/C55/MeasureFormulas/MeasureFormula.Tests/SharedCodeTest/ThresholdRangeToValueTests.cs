using System.Linq;
using NUnit.Framework;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;

namespace MeasureFormula.Tests
{
    [TestFixture]
    public class ThresholdRangeToValueTests
    {
        private readonly double[] HealthIndexBandingThresholds = {4.0, 5.5, 6.6, 8.0, 15.0};
        private readonly double[] HealthIndexScores = {1.0, 2.0, 3.0, 4.0, 5.0};
        private readonly double DefaultValue = -1000.0;      

        [Test]
        public void WhenNumberIsInARange_ValueAt_ReturnsAssociatedValue()
        {
            var tRV =  new ThresholdRangeToValue(HealthIndexBandingThresholds, HealthIndexScores, ThresholdRangeToValue.RightBoundaryType.RightBoundaryOpen, DefaultValue);
            for (var index = 0; index < HealthIndexBandingThresholds.Length; index++)
            {
                Assert.That(tRV.ValueAt(HealthIndexBandingThresholds[index ] - 1.0),
                            Is.EqualTo(HealthIndexScores[index]).Within(PointEqualityComparer.DoubleComparisonTolerance));
            }
        }

        [Test]
        public void WhenNumberOnBoundaryOfRightOpen_ValueAt_ReturnsNextValue()
        {
            var rangeToValue = new ThresholdRangeToValue(HealthIndexBandingThresholds,
                                                         HealthIndexScores,
                                                         ThresholdRangeToValue.RightBoundaryType.RightBoundaryOpen,
                                                         DefaultValue);
            
            for (var index = 0; index < HealthIndexBandingThresholds.Length - 1; index++)
            {
                Assert.That(rangeToValue.ValueAt(HealthIndexBandingThresholds[index]),
                            Is.EqualTo(HealthIndexScores[index + 1]).Within(PointEqualityComparer.DoubleComparisonTolerance));
            }
        }
        
        [Test]
        public void WhenNumberOnBoundaryOfRightClosed_ValueAt_ReturnsEarlierValue()
        {
            var rangeToValue = new ThresholdRangeToValue(HealthIndexBandingThresholds,
                                                         HealthIndexScores,
                                                         ThresholdRangeToValue.RightBoundaryType.RightBoundaryClosed,
                                                         DefaultValue);
            
            for (var index = 0; index < HealthIndexBandingThresholds.Length - 1; index++)
            {
                Assert.That(rangeToValue.ValueAt(HealthIndexBandingThresholds[index]),
                            Is.EqualTo(HealthIndexScores[index]).Within(PointEqualityComparer.DoubleComparisonTolerance));
            }
        }        

        [Test]
        public void WhenNumberIsBiggerThanLastThreshold_ValueAt_ReturnsDefaultValue()
        {
            var tRV =  new ThresholdRangeToValue(HealthIndexBandingThresholds, HealthIndexScores, ThresholdRangeToValue.RightBoundaryType.RightBoundaryOpen, DefaultValue);

            Assert.That(tRV.ValueAt(HealthIndexBandingThresholds.Last() + 1.0),
                        Is.EqualTo(DefaultValue).Within(PointEqualityComparer.DoubleComparisonTolerance));
        }

        [Test]
        public void WhenArrayLengthsDontMatch_Constructor_ThrowsException()
        {
            var shorterArray = new[] {1.0};
            Assert.That(() => new ThresholdRangeToValue(HealthIndexBandingThresholds,
                                                        shorterArray,
                                                        ThresholdRangeToValue.RightBoundaryType.RightBoundaryOpen,
                                                        DefaultValue),
                        Throws.ArgumentException);
        }

        [Test]
        public void WhenThresholdArrayIsNotAscending_Constructor_ThrowsException()
        {
            var nonAscendingArray = new[] {1.0, 2.0, 3.0, 3.0};
            Assert.That(() => new ThresholdRangeToValue(nonAscendingArray,
                                                        HealthIndexScores,
                                                        ThresholdRangeToValue.RightBoundaryType.RightBoundaryOpen,
                                                        DefaultValue),
                        Throws.ArgumentException);
        }
    }
}
