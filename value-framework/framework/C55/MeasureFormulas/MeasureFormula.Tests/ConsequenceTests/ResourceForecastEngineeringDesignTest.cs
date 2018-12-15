using System;
using System.Collections.Generic;
using AutoFixture;
using CL.FormulaHelper.DTOs;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using NUnit.Framework;
using MeasureFormula.Common_Code;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.ResourceForecastEngineeringDesignBase;
using formulaClass = CustomerFormulaCode.ResourceForecastEngineeringDesign;

namespace MeasureFormula.Tests
{
    public class ResourceForecastEngineeringDesignTest : MeasureFormulaTestsBase
    {
        private formulaClass Formulas;
        private baseClass.TimeInvariantInputDTO TimeInvariantData; 
        private baseClass.TimeVariantInputDTO[] TimeVariantData;
        
        private DistributionByResourceDTO InvestmentSpendByResource;
        private int RandomIndexNumber;
        private ResourceSpendDTO ResourceSupplySpend;
        
        [SetUp]
        public void FixtureSetup()
        {
            Formulas = new formulaClass();
            RandomIndexNumber = fixture.Create<int>() % 3;
            InvestmentSpendByResource = fixture.Create<DistributionByResourceDTO>();
            
            ResourceSupplySpend = fixture.Create<ResourceSpendDTO>();
            ResourceSupplySpend.SpendValues = new Dictionary<int, ResourceSpendDTO.Values>();
            ResourceSupplySpend.AddToSpendValues(0, 100, 1000);
            ResourceSupplySpend.AddToSpendValues(12, 200, 2000);
            ResourceSupplySpend.AddToSpendValues(24, 300, 3000);
            ResourceSupplySpend.AddToSpendValues(99, 300, 3000);
            ResourceSupplySpend.ResourceCode = CoreConstants.ENG_DESIGN;
            InvestmentSpendByResource.ResourceValues[RandomIndexNumber] = ResourceSupplySpend;
            DataPrep.SetConstructorParameter(fixture, "p_InvestmentSpendByResource", InvestmentSpendByResource);
            TimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            TimeVariantData = DataPrep.BuildTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, ArbitraryStartYear);
        }
        
        private double?[] CalculateCorrectValueArray(Dictionary<int, ResourceSpendDTO.Values> dictionary, int maxMonths)
        {
            double?[] resultArray = new double?[maxMonths];
            foreach (KeyValuePair<int, ResourceSpendDTO.Values> entry in dictionary)
            {
                if (entry.Key >= 0 && entry.Key < maxMonths)
                {
                    resultArray[entry.Key] = entry.Value.UnitValue;
                }
            }
            return resultArray;
        }
        
        [Test]
        public void GetUnits_NullTests()
        {
            Func<object, object, double?[]> getUnitsCall = (x, y) => Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, (baseClass.TimeInvariantInputDTO) x, (IReadOnlyList<baseClass.TimeVariantInputDTO>) y);
            var nullCheck = new NullablePropertyCheck();
            nullCheck.RunNullTestsIncludingCustomFields(TimeInvariantData, TimeVariantData, getUnitsCall);
        }
        
        [Test]
        public void NullInvestmentSpendByResource_GetUnits_ReturnsNull()
        {
            var nullInvestmentSpendDto = new baseClass.TimeInvariantInputDTO(null);
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullInvestmentSpendDto, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void CorrectValue_GetUnits_ReturnsCalc()
        {
            var expectedResult = CalculateCorrectValueArray(ResourceSupplySpend.SpendValues, ArbitraryMonths);
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, TimeInvariantData, TimeVariantData);
            Assert.That(results, Is.EqualTo(expectedResult).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void ResourceCodeNotFound_GetUnits_ReturnsNull()
        {
            var randomTimeInvariantData = TimeInvariantData;
            randomTimeInvariantData.InvestmentSpendByResource.ResourceValues[RandomIndexNumber].ResourceCode = fixture.Create<string>();
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, randomTimeInvariantData, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void NullSpendValue_GetUnits_ReturnsNull()
        {
            var nullSpendValueDto = TimeInvariantData;
            nullSpendValueDto.InvestmentSpendByResource.ResourceValues[RandomIndexNumber].SpendValues = null;
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, nullSpendValueDto, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void SpendValueOutOfRange_GetUnits_ReturnsNull()
        {
            var spendValueOutOfRangeDto = fixture.Create<ResourceSpendDTO>();
            spendValueOutOfRangeDto.SpendValues = new Dictionary<int, ResourceSpendDTO.Values>();
            spendValueOutOfRangeDto.AddToSpendValues(-50, 100, 1000);
            spendValueOutOfRangeDto.AddToSpendValues(9000, 200, 2000);
            spendValueOutOfRangeDto.ResourceCode = CoreConstants.ENG_DESIGN;
            var demoResourceSupplyDto = new DistributionByResourceDTO();
            demoResourceSupplyDto.ResourceValues.Add(spendValueOutOfRangeDto);
            DataPrep.SetConstructorParameter(fixture, "p_InvestmentSpendByResource", demoResourceSupplyDto);
            var spendValueOutOfRangeTimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, ArbitraryMonths, spendValueOutOfRangeTimeInvariantData, TimeVariantData);

            var expectedResults = new double?[ArbitraryMonths];
            Assert.That(results, Is.EqualTo(expectedResults).Within(CommonConstants.DoubleDifferenceTolerance));
        }
    }
}