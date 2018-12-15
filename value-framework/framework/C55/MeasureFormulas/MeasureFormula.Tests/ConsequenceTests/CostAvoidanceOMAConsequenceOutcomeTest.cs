using System;
using System.Collections.Generic;
using CL.FormulaHelper.DTOs;
using AutoFixture;
using CL.FormulaHelper;
using MeasureFormula.TestHelpers;
using NUnit.Framework;
using MeasureFormula.SharedCode;
using MeasureFormula.Common_Code;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.CostAvoidanceOMAConsequenceOutcomeBase;
using formulaClass = CustomerFormulaCode.CostAvoidanceOMAConsequenceOutcome;

namespace MeasureFormula.Tests
{
    public class CostAvoidanceOMAConsequenceOutcomeTest : MeasureFormulaTestsBase
    {
        private formulaClass Formulas;
        private int Months;
        private baseClass.TimeInvariantInputDTO TimeInvariantData;
        private baseClass.TimeVariantInputDTO[] TimeVariantData;

        private CustomFieldListItemDTO AssetAccountType;
        private int CostIncurredAfterInvestment;
        private double HoursIncurredAfterInvestment;
        private TimePeriodDTO ProjectTimePeriod;

        [SetUp]
        public void FixtureSetup()
        {
            Formulas = new formulaClass();
            
            AssetAccountType = fixture.Create<CustomFieldListItemDTO>();
            DataPrep.UpdateDto(AssetAccountType, "Value", CoreConstants.OMAAcctID);
            DataPrep.SetConstructorParameter(fixture, "p_AccountType", AssetAccountType);
            TimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();

            CostIncurredAfterInvestment = fixture.Create<int>() % 10000;
            HoursIncurredAfterInvestment = fixture.Create<double>() % 10000;
            ProjectTimePeriod = fixture.Create<TimePeriodDTO>();
            InitializeTimeVariantTestData(CostIncurredAfterInvestment, HoursIncurredAfterInvestment, ProjectTimePeriod);
            TimeVariantData = DataPrep.BuildTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, ArbitraryStartYear);
            
            Months = FormulaBase.ConvertDateTimeToOffset(TimeVariantData[2].TimePeriod.StartTime, ArbitraryStartYear) + fixture.Create<int>() % 60;
        }

        private void InitializeTimeVariantTestData(int incurredInvestmentCost, double incurredInvestmentHours, TimePeriodDTO timePeriod)
        {
            DataPrep.SetConstructorParameter(fixture, "p_CostsIncurredAfterThisInvestmentIsUndertaken", incurredInvestmentCost);
            DataPrep.SetConstructorParameter(fixture, "p_HoursIncurredIfThisInvestmentIsUndertaken", incurredInvestmentHours);
            DataPrep.SetConstructorParameter(fixture, "p_TimePeriod", timePeriod);
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
            var wrongAccountNumber = CoreConstants.OMAAcctID + 1;
            var wrongAccountOMAID = fixture.Create<CustomFieldListItemDTO>();
            DataPrep.UpdateDto(wrongAccountOMAID, "Value", wrongAccountNumber);
            DataPrep.SetConstructorParameter(fixture, "p_AccountType", wrongAccountOMAID);
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