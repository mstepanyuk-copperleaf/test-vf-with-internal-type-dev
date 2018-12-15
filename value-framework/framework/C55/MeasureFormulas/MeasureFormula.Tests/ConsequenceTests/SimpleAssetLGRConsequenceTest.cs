using System;
using System.Collections.Generic;
using AutoFixture;
using CL.FormulaHelper.DTOs;
using MeasureFormula.TestHelpers;
using NUnit.Framework;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.SimpleAssetLGRConsequenceBase;
using formulaClass = CustomerFormulaCode.SimpleAssetLGRConsequence;

namespace MeasureFormula.Tests
{
    public class SimpleAssetLGRConsequenceTest : MeasureFormulaTestsBase
    {
        private formulaClass Formulas;
        
        private baseClass.TimeInvariantInputDTO TimeInvariantData; 
        private baseClass.TimeVariantInputDTO[] TimeVariantData;
        
        private ConsequenceGroupDTO AssetFinancialRiskConsequenceDto;
        private readonly bool? AssetContributesToLostGeneration = true;
        private readonly bool? AssetIsSpareAvailable = true;
        private double? AssetTypeDowntimeWeeksWithoutSpare;
        private double? AssetTypeDowntimeWeeksWithSpare;
        private StrategyAlternativeDTO StrategyAlternativeDto;
        
        [SetUp]
        public void FixtureSetup()
        {
            Formulas = new formulaClass();
            
            AssetTypeDowntimeWeeksWithoutSpare = fixture.Create<double>();
            AssetTypeDowntimeWeeksWithSpare = fixture.Create<double>();
            AssetFinancialRiskConsequenceDto = CreateTestAssetGenerationDto();
            StrategyAlternativeDto = CreateTestStrategyAlternativeDto();
            
            InitializeTimeInvariantTestData(AssetContributesToLostGeneration,
                                            AssetFinancialRiskConsequenceDto,
                                            AssetIsSpareAvailable,
                                            AssetTypeDowntimeWeeksWithoutSpare,
                                            AssetTypeDowntimeWeeksWithSpare,
                                            StrategyAlternativeDto);
            TimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            TimeVariantData = DataPrep.BuildTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, ArbitraryStartYear);
        }
        
        private void InitializeTimeInvariantTestData(bool? contributesToLostGeneration,
                                                     ConsequenceGroupDTO financialRiskConsequenceDto,
                                                     bool? spareAvailable,
                                                     double? downtimeWeeksWithoutSpare,
                                                     double? downtimeWeeksWithSpare,
                                                     StrategyAlternativeDTO strategyAlternative)
        {
            DataPrep.SetConstructorParameter(fixture, "p_AssetContributesToLostGeneration", contributesToLostGeneration);
            DataPrep.SetConstructorParameter(fixture, "p_AssetGenerationGroup", financialRiskConsequenceDto);
            DataPrep.SetConstructorParameter(fixture, "p_AssetIsSpareAvailable", spareAvailable);
            DataPrep.SetConstructorParameter(fixture, "p_AssetTypeDowntimeWeeksWithoutSpare", downtimeWeeksWithoutSpare);
            DataPrep.SetConstructorParameter(fixture, "p_AssetTypeDowntimeWeeksWithSpare", downtimeWeeksWithSpare);
            DataPrep.SetConstructorParameter(fixture, "p_StrategyAlternative", strategyAlternative);
        }

        private StrategyAlternativeDTO CreateTestStrategyAlternativeDto()
        {
            var avoidedCo2TimeSeries =
                DataPrep.CreateRandomTimeSeriesDto(TimeSeriesDTO.TimeSeriesOffsetType.AbsoluteCalendarYearly,
                                                   20 + fixture.Create<int>() % 10,
                                                   ArbitraryStartYear + fixture.Create<int>() % 10);
            var defaultEnergyTimeSeries =
                DataPrep.CreateRandomTimeSeriesDto(TimeSeriesDTO.TimeSeriesOffsetType.AbsoluteCalendarYearly,
                                                   20 + fixture.Create<int>() % 10,
                                                   ArbitraryStartYear + fixture.Create<int>() % 10);
            return new StrategyAlternativeDTO{AvoidedCo2TimeSeries = avoidedCo2TimeSeries, DefaultEnergyTimeSeries = defaultEnergyTimeSeries};
        }

        private ConsequenceGroupDTO CreateTestAssetGenerationDto()
        {
            var stringName = fixture.Create<string>();
            var priceValues =
                DataPrep.CreateRandomTimeSeriesDto(TimeSeriesDTO.TimeSeriesOffsetType.AbsoluteCalendarYearly,
                                                   20 + fixture.Create<int>() % 10,
                                                   ArbitraryStartYear + fixture.Create<int>() % 10);
            var loss = fixture.Create<double>() % 1000;
            var unitCapacity = fixture.Create<double>() % 1000;
            return new ConsequenceGroupDTO() {Name =  stringName, PriceValues = priceValues, Loss = new List<double>{loss}, UnitCapacity = unitCapacity };
        }
        
        [Test]
        public void GetUnits_NullTests()
        {
            Func<object, object, double?[]> getUnitsCall = (x, y) =>
                Formulas.GetUnits(ArbitraryStartYear,
                                  ArbitraryMonths,
                                  (baseClass.TimeInvariantInputDTO) x,
                                  (IReadOnlyList<baseClass.TimeVariantInputDTO>) y);
            var nullCheck = new NullablePropertyCheck();
            nullCheck.RunNullTestsIncludingCustomFields(TimeInvariantData, TimeVariantData, getUnitsCall);
        }

        [Test]
        public void FalseAssetContributesToLostGeneration_GetUnits_ReturnsNull()
        {
            InitializeTimeInvariantTestData(false,
                                            AssetFinancialRiskConsequenceDto,
                                            AssetIsSpareAvailable,
                                            AssetTypeDowntimeWeeksWithoutSpare,
                                            AssetTypeDowntimeWeeksWithSpare,
                                            StrategyAlternativeDto);
            var falseLostGenTimeInvariantInputDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, falseLostGenTimeInvariantInputDto, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void AssetGenerationGroupNull_GetUnits_ReturnsNull()
        {
            InitializeTimeInvariantTestData(AssetContributesToLostGeneration,
                                            null,
                                            AssetIsSpareAvailable,
                                            AssetTypeDowntimeWeeksWithoutSpare,
                                            AssetTypeDowntimeWeeksWithSpare,
                                            StrategyAlternativeDto);
            var assetGenerationGroupNullTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, assetGenerationGroupNullTimeInvariantData, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void GenerationGroupLossZeroList_GetUnits_ReturnsNull()
        {
            var nullAssetFinancialRiskConsequence = AssetFinancialRiskConsequenceDto;
            nullAssetFinancialRiskConsequence.Loss = new List<double>();
            InitializeTimeInvariantTestData(AssetContributesToLostGeneration,
                                            nullAssetFinancialRiskConsequence,
                                            AssetIsSpareAvailable,
                                            AssetTypeDowntimeWeeksWithoutSpare,
                                            AssetTypeDowntimeWeeksWithSpare,
                                            StrategyAlternativeDto);
            var assetGenerationGroupNullTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, assetGenerationGroupNullTimeInvariantData, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void GenerationGroupLossNull_GetUnits_ReturnsNull()
        {
            var nullAssetFinancialRiskConsequence = AssetFinancialRiskConsequenceDto;
            nullAssetFinancialRiskConsequence.Loss = null;
            InitializeTimeInvariantTestData(AssetContributesToLostGeneration,
                                            nullAssetFinancialRiskConsequence,
                                            AssetIsSpareAvailable,
                                            AssetTypeDowntimeWeeksWithoutSpare,
                                            AssetTypeDowntimeWeeksWithSpare,
                                            StrategyAlternativeDto);
            var assetGenerationGroupNullTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, assetGenerationGroupNullTimeInvariantData, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void SpareAvailableDowntimeIsZero_GetUnits_ReturnsNull()
        {
            double? downtimeWeeksWithSpare = 0;
            InitializeTimeInvariantTestData(AssetContributesToLostGeneration,
                                            AssetFinancialRiskConsequenceDto,
                                            true,
                                            AssetTypeDowntimeWeeksWithoutSpare,
                                            downtimeWeeksWithSpare,
                                            StrategyAlternativeDto);
            var assetGenerationGroupNullTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, assetGenerationGroupNullTimeInvariantData, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void SpareNotAvailableDowntimeIsZero_GetUnits_ReturnsNull()
        {
            double? downtimeWeeksWithoutSpare = 0;
            InitializeTimeInvariantTestData(AssetContributesToLostGeneration,
                                            AssetFinancialRiskConsequenceDto,
                                            false,
                                            downtimeWeeksWithoutSpare,
                                            AssetTypeDowntimeWeeksWithSpare,
                                            StrategyAlternativeDto);
            var assetGenerationGroupNullTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, assetGenerationGroupNullTimeInvariantData, TimeVariantData);
            Assert.That(results, Is.Null);
        }
    }
}