using System;
using System.Linq;
using AutoFixture;
using CL.FormulaHelper;
using CL.FormulaHelper.DTOs;
using NUnit.Framework;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using LogisticCurve = MeasureFormula.SharedCode.ProbabilityHelpers.LogisticConditionToAnnualProbabilityOfFailure;
using MeasureFormula.Common_Code;

namespace MeasureFormula.Tests.SharedCodeTest
{
    [TestFixture]
    public class ProbabilityHelpersTests : MeasureFormulaTestsBase
    {
        private XYCurveDTO ArbitraryCurve;
        private double?[] ArbitraryBaselineConditions;  

        private readonly XYCurveDTO SimpleCurve = new XYCurveDTO
        {
            Points = new[] { new CurvePointDTO { X = 0.0, Y = 0.9 }, new CurvePointDTO { X = 10.0, Y = 0.1 }, }
        };
        private readonly int FiscalYearEndMonth = 4;

        // someday we'll need unit test helpers ....

        [SetUp]
        public void InitializeFixture()
        {
             ArbitraryCurve = fixture.Create<XYCurveDTO>();
             ArbitraryBaselineConditions = fixture.CreateMany<double?>().ToArray();  
        }
    
        [Test]
        public void LogisticConditionToAnnualProbabilityOfFailureMatchesOldValues()
        {
            var defaultCurve =
                (new ProbabilityHelpers.LogisticConditionToAnnualProbabilityOfFailure(CustomerConstants.BestConditionScore,
                                                                                      CustomerConstants.WorstConditionScore)).
                ConstructConditionToAnnualProbabilityOfFailureCurve();

            // correct values taken from AssetPOF-DegradationModel.xlsx
            var yValues = new []
            {
                0.79820, 0.50352, 0.31740, 0.19998, 0.12597, 0.07933, 0.04995, 0.03145, 0.01980, 0.01247, 0.00785
            };
            const int numIntegerConditions = 11;
            var expectedCurvePoints = Enumerable.Range(0, numIntegerConditions).Select((x, index) => new CurvePointDTO { X = x, Y = yValues[index] }).ToArray();

            Assert.That(defaultCurve.Points, Is.EqualTo(expectedCurvePoints).Using(new PointEqualityComparer(pointTolerance: 0.001)));
        }

        [Test]
        public void LogisticConditionToAnnualProbabilityOfFailure_WhenConditionScoreDifferenceIsNotIntegral_HasLastPointAtHighestConditionScore()
        {
            var worstConditionScore = 1;
            var bestConditionScore = 9.5;
            var curve = (new LogisticCurve(bestConditionScore, worstConditionScore)).ConstructConditionToAnnualProbabilityOfFailureCurve();
            Assert.That(curve.Points.Last().X, Is.EqualTo(bestConditionScore));
            Assert.That(curve.Points.First().X, Is.EqualTo(worstConditionScore));
        }

        [Test]
        public void LogisticConditionToAnnualProbabilityOfFailure_WhenConditionScoresAreReversedNonIntegral_HasExtremePointsAtExtremeConditionScores()
        {
            var worstConditionScore = 9.5;
            var bestConditionScore = 1.1;
            var curve = (new LogisticCurve(bestConditionScore, worstConditionScore)).ConstructConditionToAnnualProbabilityOfFailureCurve();
            Assert.That(curve.Points.Last().X, Is.EqualTo(worstConditionScore).Within(PointEqualityComparer.DoubleComparisonTolerance));
            Assert.That(curve.Points.First().X, Is.EqualTo(bestConditionScore).Within(PointEqualityComparer.DoubleComparisonTolerance));
        }

        [Test]
        public void LegacyAAOutcomeProbabilitiesTest()
        {
            FormulaBase.FiscalYearEndMonth = FiscalYearEndMonth;

            var interventionStartTime = new DateTime(ArbitraryStartYear - 1, FiscalYearEndMonth + 3, 5);

            var expectedNullFromMissingCurve =
                ProbabilityHelpers.LegacyAAOutcomeProbabilities(ArbitraryStartYear,
                                                                interventionStartTime,
                                                                ArbitraryBaselineConditions,
                                                                CustomerConstants.BestConditionScore,
                                                                null);
            Assert.That(expectedNullFromMissingCurve, Is.Null);

            var expectedNullFromMissingConditions =
                ProbabilityHelpers.LegacyAAOutcomeProbabilities(ArbitraryStartYear,
                                                                interventionStartTime,
                                                                null,
                                                                CustomerConstants.BestConditionScore,
                                                                ArbitraryCurve);
            Assert.That(expectedNullFromMissingConditions, Is.Null);


            var monthlyBaselineConditionScores = new double?[] { CustomerConstants.BestConditionScore, 5.0, 5.0, 5.0 };
            var simpleProbs =
                ProbabilityHelpers.LegacyAAOutcomeProbabilities(ArbitraryStartYear,
                                                                interventionStartTime,
                                                                monthlyBaselineConditionScores,
                                                                CustomerConstants.BestConditionScore,
                                                                SimpleCurve);
            Assert.That(simpleProbs, Is.Not.Null);

            var mintMonthlyFailureProbability =
                HelperFunctions.GetMonthlyProbabilityValue(SimpleCurve.YFromX(CustomerConstants.BestConditionScore));
            var expectedSecondValue = HelperFunctions.GetMonthlyProbabilityValue(SimpleCurve.YFromX(monthlyBaselineConditionScores[2] ?? 0.0))
                                      - mintMonthlyFailureProbability;
            var expectedProbs = new[] { 0.0, 0.0, expectedSecondValue, expectedSecondValue };

            Assert.That(expectedProbs, Is.EqualTo(simpleProbs).Within(PointEqualityComparer.DoubleComparisonTolerance));

            var interventionStartTimeInPast = new DateTime(ArbitraryStartYear - 3, 1, 5);
            var probabilitiesWhenStartTimeInPast =
                ProbabilityHelpers.LegacyAAOutcomeProbabilities(ArbitraryStartYear,
                                                                interventionStartTimeInPast,
                                                                monthlyBaselineConditionScores,
                                                                CustomerConstants.BestConditionScore,
                                                                SimpleCurve);
            Assert.That(probabilitiesWhenStartTimeInPast, Is.Not.Null);
            var baselineProbabilitiesWhenStartTimeInPast =
                ProbabilityHelpers.LegacyAABaselineProbabilities(monthlyBaselineConditionScores, CustomerConstants.BestConditionScore, SimpleCurve);
            Assert.That(baselineProbabilitiesWhenStartTimeInPast, Is.EqualTo(probabilitiesWhenStartTimeInPast).Within(PointEqualityComparer.DoubleComparisonTolerance));
        }

        [Test]
        public void LegacyAABaselineProbabilitiesTest()
        {
            var expectedNullFromMissingCurve =
                ProbabilityHelpers.LegacyAABaselineProbabilities(ArbitraryBaselineConditions, CustomerConstants.BestConditionScore,null);
            Assert.That(expectedNullFromMissingCurve, Is.Null);

            var expectedNullFromMissingConditions = ProbabilityHelpers.LegacyAABaselineProbabilities(null, CustomerConstants.BestConditionScore, ArbitraryCurve);
            Assert.That(expectedNullFromMissingConditions, Is.Null);

            var monthlyBaselineConditionScores = new double?[] { CustomerConstants.BestConditionScore, 5.0};

            var simpleMonthlyProbabilities =
                ProbabilityHelpers.LegacyAABaselineProbabilities( monthlyBaselineConditionScores, CustomerConstants.BestConditionScore, SimpleCurve);
            Assert.That(simpleMonthlyProbabilities, Is.Not.Null);

            var mintMonthlyFailureProbability =
                HelperFunctions.GetMonthlyProbabilityValue(SimpleCurve.YFromX(CustomerConstants.BestConditionScore));
            var expectedSecondValue = HelperFunctions.GetMonthlyProbabilityValue(SimpleCurve.YFromX(monthlyBaselineConditionScores[1] ?? 0.0))
                                      - mintMonthlyFailureProbability;
            var expectedMonthlyProbabilities = new [] {0.0, expectedSecondValue};

            Assert.That(expectedMonthlyProbabilities,Is.EqualTo(simpleMonthlyProbabilities).Within(PointEqualityComparer.DoubleComparisonTolerance));
        }
        
        [Test]
        public void ConditionalProbabilities_NullMonthlyConditionScore_ReturnsNull()
        {
            var result = ProbabilityHelpers.ConditionalProbabilities(null, SimpleCurve);
            Assert.That(result,Is.Null);
        }
        
        [Test]
        public void ConditionalProbabilities_NullFailureCurve_ReturnsNull()
        {
            var result = ProbabilityHelpers.ConditionalProbabilities(ArbitraryBaselineConditions, null);
            Assert.That(result,Is.Null);
        }
    }
}
