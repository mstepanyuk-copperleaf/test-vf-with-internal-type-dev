using System;
using System.Collections.Generic;
using CL.FormulaHelper.DTOs;
using AutoFixture;
using MeasureFormula.TestHelpers;
using NUnit.Framework;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.SimpleAssetSafetyProbabilityOutcomeBase;
using formulaClass = CustomerFormulaCode.SimpleAssetSafetyProbabilityOutcome;

namespace MeasureFormula.Tests
{
    public class SimpleAssetSafetyProbabilityOutcomeTest : MeasureFormulaTestsBase
    {
        private formulaClass Formulas;
        private baseClass.TimeInvariantInputDTO TimeInvariantData;
        private baseClass.TimeVariantInputDTO[] TimeVariantData;
        private int SampleMonths = 3;
        
        private XYCurveDTO AssetConditionDecayCurve;
        private double? AssetSafetyRiskFactor;
        private readonly double?[] AssetConditionScore = new double?[3];

        [SetUp]
        public void FixtureSetup()
        {
            Formulas = new formulaClass();
            
            AssetConditionDecayCurve = fixture.Build<XYCurveDTO>().With(x => x.Points, SimpleDepreciationCurve()).Create();
            AssetSafetyRiskFactor = fixture.Create<double>() % 1000 / 100;
            AssetConditionScore[0] = 3.0 + fixture.Create<double>() % 7;
            AssetConditionScore[1] = 3.0 + fixture.Create<double>() % 7;
            AssetConditionScore[2] = 3.0 + fixture.Create<double>() % 7;
            
            InitializeTimeInvariantTestData(AssetSafetyRiskFactor, AssetConditionScore, AssetConditionDecayCurve);
            TimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            TimeVariantData = DataPrep.BuildTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, ArbitraryStartYear);
        }

        private void InitializeTimeInvariantTestData(
            double? safetyRiskFactor,
            double?[] conditionScore,
            XYCurveDTO conditionToFailureCurve)
        {
            DataPrep.SetConstructorParameter(fixture, "p_AssetTypeSafetyRiskExposureFactor", safetyRiskFactor);
            DataPrep.SetConstructorParameter(fixture, "p_ConditionScore_Condition_ConsqUnitOutput", conditionScore);
            DataPrep.SetConstructorParameter(fixture, "p_ConditionToFailureCurve", conditionToFailureCurve);
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
        public void GetLikelihoodValues_NullTest_ReturnsNull()
        {
            Func<object, object, double?[]> getUnitsCall = (x, y) =>
                Formulas.GetLikelihoodValues(ArbitraryStartYear,
                                             SampleMonths,
                                             (baseClass.TimeInvariantInputDTO) x,
                                             (IReadOnlyList<baseClass.TimeVariantInputDTO>) y);
            var nullCheck = new NullablePropertyCheck();
            nullCheck.RunNullTestsIncludingCustomFields(TimeInvariantData, TimeVariantData, getUnitsCall);
        }
        
        [Test]
        public void GetLikelihoodValues_NullConditionCurve_ReturnsNull()
        {
            InitializeTimeInvariantTestData(AssetSafetyRiskFactor, AssetConditionScore, null);
            var nullConditionDecayCurve = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var result = Formulas.GetLikelihoodValues(ArbitraryStartYear,
                                                      SampleMonths,
                                                      nullConditionDecayCurve,
                                                      TimeVariantData);
            Assert.That(result, Is.Null);
        }
    }
}