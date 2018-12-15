using System;
using System.Collections.Generic;
using CL.FormulaHelper.DTOs;
using AutoFixture;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using NUnit.Framework;
using System.Linq;
using MeasureFormula.Common_Code;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.SimpleAssetConditionalProbabilityOutcomeAABase;
using formulaClass = CustomerFormulaCode.SimpleAssetConditionalProbabilityOutcomeAA;

namespace MeasureFormula.Tests
{
    public class SimpleAssetConditionalProbabilityOutcomeAATest : MeasureFormulaTestsBase
    {
        private readonly Fixture conditionFixture = new Fixture();
        
        private formulaClass Formulas;
        private baseClass.TimeInvariantInputDTO TimeInvariantData;
        private baseClass.TimeVariantInputDTO[] TimeVariantData;
        
        private int SurveyDate;
        private XYCurveDTO AssetConditionDecayCurve;
        private double?[] AssetConditionScore;
        
        [SetUp]
        public void FixtureSetup()
        {
            conditionFixture.Customizations.Add(new RandomNumericSequenceGenerator(CustomerConstants.WorstConditionScore
                                                                                   + 1,
                                                                                   CustomerConstants.
                                                                                       BestConditionScore));
            Formulas = new formulaClass();
            
            AssetConditionDecayCurve = fixture.Build<XYCurveDTO>().With(x => x.Points, SimpleDepreciationCurve()).Create();
            AssetConditionScore = conditionFixture.CreateMany<double?>(ArbitraryMonths).ToArray();
            
            InitializeTimeInvariantTestData(AssetConditionScore, AssetConditionDecayCurve);
            TimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            SurveyDate = ArbitraryStartYear + 1;
            TimeVariantData = DataPrep.BuildTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, SurveyDate );
        }
        
         private void InitializeTimeInvariantTestData(double?[] conditionScore, XYCurveDTO failureCurve)
        {
            DataPrep.SetConstructorParameter(fixture, "p_ConditionScore_Condition_ConsqUnitOutput_B", conditionScore);
            DataPrep.SetConstructorParameter(fixture, "p_ConditionToFailureCurve", failureCurve);
        }
        
         private CurvePointDTO[] SimpleDepreciationCurve()
        {
            CurvePointDTO[] generatedCurvePointDto = new CurvePointDTO[3];
            generatedCurvePointDto[0] = new CurvePointDTO{CurveId = fixture.Create<long>(),X = 1,Y = 10.0};
            generatedCurvePointDto[1] = new CurvePointDTO{CurveId = fixture.Create<long>(),X = 2,Y = 7.2};
            generatedCurvePointDto[2] = new CurvePointDTO{CurveId = fixture.Create<long>(),X = 3,Y = 2.4};
            return generatedCurvePointDto;
        }
        
        private double?[] createConditionArray(int numberOfMonths)
        {
            var decayCondition = new double?[numberOfMonths];
            
            var initialCondition = CustomerConstants.BestConditionScore;
            for (int i = 0; i < decayCondition.Length ; i++)
            {
                initialCondition--;
                
                if (initialCondition < CustomerConstants.WorstConditionScore)
                {
                    initialCondition = CustomerConstants.BestConditionScore;
                }
                
                decayCondition[i] = initialCondition;
            }
            return decayCondition;
        }
        
        [Test]
        public void NullTests_GetUnits_ReturnsNull()
        {
            Func<object, object, double?[]> getUnitsCall = (x, y) =>
                Formulas.GetLikelihoodValues(ArbitraryStartYear,
                                             ArbitraryMonths,
                                             (baseClass.TimeInvariantInputDTO) x,
                                             (IReadOnlyList<baseClass.TimeVariantInputDTO>) y);
            
            var nullCheck = new NullablePropertyCheck();
            nullCheck.RunNullTestsIncludingCustomFields(TimeInvariantData, TimeVariantData, getUnitsCall);
        }
        
        [Test]
        public void GetLikelihoodValues_NullConditionArray_ReturnsNull()
        {
            InitializeTimeInvariantTestData(null, AssetConditionDecayCurve);
            var nullConditionArrayDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var result =
                Formulas.GetLikelihoodValues(ArbitraryStartYear,
                                             ArbitraryMonths,
                                             nullConditionArrayDto,
                                             TimeVariantData);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void GetLikelihoodValues_NullConditionCurve_ReturnsNull()
        {
            InitializeTimeInvariantTestData(AssetConditionScore, null);
            var nullConditionDecayCurveDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var result = Formulas.GetLikelihoodValues(ArbitraryStartYear,
                                                      ArbitraryMonths,
                                                      nullConditionDecayCurveDto,
                                                      TimeVariantData);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void GetLikelihoodValues_NullTimeVariantData_ReturnsNull()
        {
            var newTimeVariantDataDto = new baseClass.TimeVariantInputDTO[0];
            
            var result = Formulas.GetLikelihoodValues(ArbitraryStartYear,
                                                      ArbitraryMonths,
                                                      TimeInvariantData,
                                                      newTimeVariantDataDto);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public void GetLikelihoodValues_ChangingMatchingConditionScore_ReturnsCalc()
        {
            var decayCondition = createConditionArray(ArbitraryMonths);
            InitializeTimeInvariantTestData(decayCondition, AssetConditionDecayCurve);
            var matchingConditionTimeInvariantDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var result = Formulas.GetLikelihoodValues(ArbitraryStartYear,
                                                      ArbitraryMonths,
                                                      matchingConditionTimeInvariantDto,
                                                      TimeVariantData);
            
            var expectedResults = ProbabilityHelpers.LegacyAAOutcomeProbabilities(ArbitraryStartYear,
                                                                                  TimeVariantData[0].TimePeriod.StartTime,
                                                                                  decayCondition,
                                                                                  CustomerConstants.BestConditionScore,
                                                                                  AssetConditionDecayCurve);
            Assert.That(result, Is.EqualTo(expectedResults).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void GetLikelihoodValues_ConstantConditionScore_ReturnsZeros()
        {
            var constantCondition = Enumerable.Repeat((double?) 7.3, ArbitraryMonths).ToArray();
            InitializeTimeInvariantTestData(constantCondition, AssetConditionDecayCurve);
            var matchingConditionTimeInvariantDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var result = Formulas.GetLikelihoodValues(ArbitraryStartYear,
                                                      ArbitraryMonths,
                                                      matchingConditionTimeInvariantDto,
                                                      TimeVariantData);

            var expectedResults = Enumerable.Repeat(0, ArbitraryMonths).ToArray();
            Assert.That(result, Is.EqualTo(expectedResults).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
    }
}