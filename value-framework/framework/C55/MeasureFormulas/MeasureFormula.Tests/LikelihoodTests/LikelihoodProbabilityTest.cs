using System.Linq;
using CL.FormulaHelper.DTOs;
using AutoFixture;
using MeasureFormula.SharedCode;
using NUnit.Framework;
using MeasureFormula.Common_Code;

namespace MeasureFormula.Tests
{
    public class LikelihoodProbabilityTest : MeasureFormulaTestsBase
    {
        private int SampleMonths = 3;
        
        private XYCurveDTO AssetConditionDecayCurve;
        private double? AssetFinancialRiskFactor;
        private readonly double?[] AssetConditionScore = new double?[3];
        
        [SetUp]
        public void FixtureSetup()
        {
            AssetConditionDecayCurve = fixture.Build<XYCurveDTO>().With(x => x.Points, SimpleDepreciationCurve()).Create();
            AssetFinancialRiskFactor = fixture.Create<double>() % 1000 / 100;
            AssetConditionScore[0] = 3.0 + fixture.Create<double>() % 7;
            AssetConditionScore[1] = 3.0 + fixture.Create<double>() % 7;
            AssetConditionScore[2] = 3.0 + fixture.Create<double>() % 7;
        }
        
         private CurvePointDTO[] SimpleDepreciationCurve()
        {
            CurvePointDTO[] generatedCurvePointDto = new CurvePointDTO[3];
            generatedCurvePointDto[0] = new CurvePointDTO{CurveId = fixture.Create<long>(),X = 1,Y = 9.7};
            generatedCurvePointDto[1] = new CurvePointDTO{CurveId = fixture.Create<long>(),X = 2,Y = 5.2};
            generatedCurvePointDto[2] = new CurvePointDTO{CurveId = fixture.Create<long>(),X = 3,Y = 2.4};
            return generatedCurvePointDto;
        }
        
        [Test]
        public void GetLikelihoodValues_NormalConditions_ReturnsCalc()
        {
            var result = LikelihoodProbability.GetLikelihoodValues(SampleMonths,
                                                                   AssetConditionDecayCurve,
                                                                   AssetConditionScore,
                                                                   AssetFinancialRiskFactor);
            
            var intermediateCalculations =
                ProbabilityHelpers.ConditionalProbabilities(AssetConditionScore, AssetConditionDecayCurve);
            var multiplyArray = Enumerable.Repeat(AssetFinancialRiskFactor, SampleMonths).ToArray();
            var expectedResult = ArrayHelper.MultiplyArrays(intermediateCalculations, multiplyArray);
            Assert.That(result, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void GetLikelihoodValues_NullConditionArray_ReturnsNull()
        {
            var result =
                LikelihoodProbability.GetLikelihoodValues(SampleMonths,
                                                          AssetConditionDecayCurve,
                                                          null,
                                                          AssetFinancialRiskFactor);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void GetLikelihoodValues_NullPointsConditionCurve_ReturnsNull()
        {
            var nullPointsCurve = new XYCurveDTO();
            
            var result =
                LikelihoodProbability.GetLikelihoodValues(SampleMonths,
                                                          nullPointsCurve,
                                                          AssetConditionScore,
                                                          AssetFinancialRiskFactor);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void GetLikelihoodValues_NullAssetFinancialRiskFactor_ReturnsCalculationsWithExposureFactorFull()
        {
            var result =
                LikelihoodProbability.GetLikelihoodValues(SampleMonths,
                                                          AssetConditionDecayCurve,
                                                          AssetConditionScore,
                                                          null);
            
            var expectedResult = LikelihoodProbability.GetLikelihoodValues(SampleMonths,
                                                                           AssetConditionDecayCurve,
                                                                           AssetConditionScore,
                                                                           CommonConstants.ExposureFactorFull);
            
            Assert.That(result, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
    }
} 