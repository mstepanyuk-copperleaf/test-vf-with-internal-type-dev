using System;
using System.Collections.Generic;
using CL.FormulaHelper.DTOs;
using AutoFixture;
using CL.FormulaHelper;
using MeasureFormula.TestHelpers;
using NUnit.Framework;
using MeasureFormula.Common_Code;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.CostSavingsOMAConsequenceBase;
using formulaClass = CustomerFormulaCode.CostSavingsOMAConsequence;

namespace MeasureFormula.Tests
{
    public class CostSavingsOMAConsequenceTest : MeasureFormulaTestsBase
    {
        private formulaClass Formulas;
        private int Months;
        private baseClass.TimeInvariantInputDTO TimeInvariantData;
        private baseClass.TimeVariantInputDTO[] TimeVariantData;
        private CustomFieldListItemDTO AssetAccountType;

        [SetUp]
        public void FixtureSetup()
        {
            Formulas = new formulaClass();
            
            AssetAccountType = fixture.Create<CustomFieldListItemDTO>();
            DataPrep.UpdateDto(AssetAccountType, "Value", CoreConstants.OMAAcctID);
            DataPrep.SetConstructorParameter(fixture, "p_AccountType", AssetAccountType);
            TimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            TimeVariantData = DataPrep.BuildTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, ArbitraryStartYear);
            CorrectSavedValueInTimeVariantDTO(TimeVariantData[0], fixture.Create<int>() % 10000, fixture.Create<int>() % 10000);
            CorrectSavedValueInTimeVariantDTO(TimeVariantData[1], fixture.Create<int>() % 10000, fixture.Create<int>() % 10000);
            CorrectSavedValueInTimeVariantDTO(TimeVariantData[2], fixture.Create<int>() % 10000, fixture.Create<int>() % 10000);
            Months = FormulaBase.ConvertDateTimeToOffset(TimeVariantData[2].TimePeriod.StartTime, ArbitraryStartYear) + fixture.Create<int>() % 60;
        }
        
        private void CorrectSavedValueInTimeVariantDTO(object timeVariantData, int costSaved, double hoursSaved)
        {
            DataPrep.UpdateDto(timeVariantData, "CostsSaved", costSaved);
            DataPrep.UpdateDto(timeVariantData, "HoursSaved", hoursSaved);
        }
        
        [Test]
        public void GetUnits_NullTests()
        {
            Func<object, object, double?[]> getUnitsCall = (x, y) => Formulas.GetUnits(ArbitraryStartYear, Months, (baseClass.TimeInvariantInputDTO) x, (IReadOnlyList<baseClass.TimeVariantInputDTO>) y);
            var nullCheck = new NullablePropertyCheck();
            nullCheck.RunNullTestsIncludingCustomFields(TimeInvariantData, TimeVariantData, getUnitsCall);
        }
        
        [Test]
        public void NullAccountType_GetUnits_ReturnsNull()
        {
            var nullAccountTypeDto = new baseClass.TimeInvariantInputDTO(null);
            var results = Formulas.GetUnits(ArbitraryStartYear, Months, nullAccountTypeDto, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void AccountNotOMAAcctID_GetUnits_ReturnsNull()
        {
            var randomAccountNumber = CoreConstants.OMAAcctID + 1;
            var wrongAccountType = fixture.Create<CustomFieldListItemDTO>();
            DataPrep.UpdateDto(wrongAccountType, "Value", randomAccountNumber);
            DataPrep.SetConstructorParameter(fixture, "p_AccountType", wrongAccountType);
            var wrongAccountTypeDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, Months, wrongAccountTypeDto, TimeVariantData);
            
            Assert.That(results, Is.Null);
        }

        [Test]
        public void ValuesWithinRange_GetUnits_ReturnsCalc()
        {
            var result = Formulas.GetUnits(ArbitraryStartYear, Months, TimeInvariantData, TimeVariantData);
            Assert.That(result, !Is.Null);
        }
    }
}