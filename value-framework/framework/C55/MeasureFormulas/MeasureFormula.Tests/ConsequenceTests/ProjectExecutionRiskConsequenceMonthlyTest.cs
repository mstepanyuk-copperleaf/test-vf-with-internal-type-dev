using System;
using System.Collections.Generic;
using CL.FormulaHelper.DTOs;
using AutoFixture;
using MeasureFormula.SharedCode;
using MeasureFormula.TestHelpers;
using NUnit.Framework;

using baseClass = MeasureFormulas.Generated_Formula_Base_Classes.ProjectExecutionRiskConsequenceMonthlyBase;
using formulaClass = CustomerFormulaCode.ProjectExecutionRiskConsequenceMonthly;

namespace MeasureFormula.Tests
{
    public class ProjectExecutionRiskConsequenceMonthlyTest : MeasureFormulaTestsBase
    {
        private formulaClass Formulas;
        private int Months;
        private baseClass.TimeInvariantInputDTO TimeInvariantData; 
        private baseClass.TimeVariantInputDTO[] TimeVariantData;

        private CustomFieldListItemDTO InitialEffortScheduleRealistic;
        private CustomFieldListItemDTO WellDefinedProjectScope;
        private DistributionByAccountTypeDTO InvestmentSpendByAccountType;
        private double[] SpendArrayInMonths;
        private double[] SpendArrayInYears;
        private CustomFieldListItemDTO LiningImplementationPartners;
        private string MethodologyTrackRecord;
        private string PMPreviousExperience;
        private CustomFieldListItemDTO ProvenImplementationMethodology;
        private CustomFieldListItemDTO SkillsToManageTheProject;
        private CustomFieldListItemDTO TeamRightInternalSkillset;
        private CustomFieldListItemDTO TypeOfContract;
        private CustomFieldListItemDTO VendorIndustryExperience;
        private CustomFieldListItemDTO VendorTechnicalExperience;
        
        [SetUp]
        public void FixtureSetup()
        {
            Formulas = new formulaClass();
            InitialEffortScheduleRealistic = fixture.Build<CustomFieldListItemDTO>().Create();
            WellDefinedProjectScope = fixture.Build<CustomFieldListItemDTO>().Create();
            SpendArrayInYears = new[] {0.3, 0.2, 0.5};
            SpendArrayInMonths = CreateSpendArray(SpendArrayInYears);
            Months = Math.Max(fixture.Create<int>() % SpendArrayInMonths.Length, 1);
            
            InvestmentSpendByAccountType = DataPrep.MakeDistributionDTO("Capital", 0, SpendArrayInMonths);
            LiningImplementationPartners = fixture.Build<CustomFieldListItemDTO>().Create();
            MethodologyTrackRecord = fixture.Create<string>();
            PMPreviousExperience = fixture.Create<string>();
            ProvenImplementationMethodology = fixture.Build<CustomFieldListItemDTO>().Create();
            SkillsToManageTheProject = fixture.Build<CustomFieldListItemDTO>().Create();
            TeamRightInternalSkillset = fixture.Build<CustomFieldListItemDTO>().Create();
            TypeOfContract = fixture.Build<CustomFieldListItemDTO>().Create();
            VendorIndustryExperience = fixture.Build<CustomFieldListItemDTO>().Create();
            VendorTechnicalExperience = fixture.Build<CustomFieldListItemDTO>().Create();
            
            InitializeTimeInvariantTestData(InitialEffortScheduleRealistic,
                                            WellDefinedProjectScope,
                                            InvestmentSpendByAccountType,
                                            LiningImplementationPartners,
                                            MethodologyTrackRecord,
                                            PMPreviousExperience,
                                            ProvenImplementationMethodology,
                                            SkillsToManageTheProject,
                                            TeamRightInternalSkillset,
                                            TypeOfContract,
                                            VendorIndustryExperience,
                                            VendorTechnicalExperience);
            TimeInvariantData = fixture.Create<baseClass.TimeInvariantInputDTO>();
            TimeVariantData = DataPrep.BuildTimeVariantData<baseClass.TimeVariantInputDTO>(fixture, ArbitraryStartYear);
        }
        
        private void InitializeTimeInvariantTestData(
            CustomFieldListItemDTO effortScheduleRealistic,
            CustomFieldListItemDTO definedProjectScope,
            DistributionByAccountTypeDTO investmentSpendAccount,
            CustomFieldListItemDTO implementationPartners,
            string methodTrackRecord,
            string previousExperience,
            CustomFieldListItemDTO implementationMethodology,
            CustomFieldListItemDTO skillsManagementProject,
            CustomFieldListItemDTO teamSkillset,
            CustomFieldListItemDTO contracts,
            CustomFieldListItemDTO vendorIndustExperience,
            CustomFieldListItemDTO vendorTechExperience)
        {
            DataPrep.SetConstructorParameter(fixture, "p_AreInitialEffortAndScheduleRealistic", effortScheduleRealistic);
            DataPrep.SetConstructorParameter(fixture, "p_HowWellDefinedWillProjectScopeBe", definedProjectScope);
            DataPrep.SetConstructorParameter(fixture, "p_InvestmentSpendByAccountType", investmentSpendAccount);
            DataPrep.SetConstructorParameter(fixture, "p_LiningUpRightImplementationPartners", implementationPartners);
            DataPrep.SetConstructorParameter(fixture, "p_MethodologyAndTrackRecord", methodTrackRecord);
            DataPrep.SetConstructorParameter(fixture, "p_PMsPreviousExperience", previousExperience);
            DataPrep.SetConstructorParameter(fixture, "p_ProvenImplementationMethodology", implementationMethodology);
            DataPrep.SetConstructorParameter(fixture, "p_SkillsToManageTheProject", skillsManagementProject);
            DataPrep.SetConstructorParameter(fixture, "p_TeamsRightInternalSkillset", teamSkillset);
            DataPrep.SetConstructorParameter(fixture, "p_TypeOfContract", contracts);
            DataPrep.SetConstructorParameter(fixture, "p_VendorIndustryExperience", vendorIndustExperience);
            DataPrep.SetConstructorParameter(fixture, "p_VendorTechnicalExperience", vendorTechExperience);
        }

        private double[] CreateSpendArray(double[] annualSpendAmount)
        {
            double[] monthlySpendArray = new double[annualSpendAmount.Length * CommonConstants.MonthsPerYearInt];
            for (int i = 0; i < annualSpendAmount.Length; i++)
            {
                double monthlySpending = annualSpendAmount[i] / CommonConstants.MonthsPerYear;
                for (int j = 0; j < CommonConstants.MonthsPerYear; j++)
                {
                    monthlySpendArray[i * CommonConstants.MonthsPerYearInt + j] = monthlySpending;
                }
            }
            return monthlySpendArray;
        }
        
        [Test]
        public void GetUnits_NullTests()
        {
            Func<object, object, double?[]> getUnitsCall = (x, y) => Formulas.GetUnits(ArbitraryStartYear, Months, (baseClass.TimeInvariantInputDTO) x, (IReadOnlyList<baseClass.TimeVariantInputDTO>) y);
            var nullCheck = new NullablePropertyCheck();
            nullCheck.RunNullTestsIncludingCustomFields(TimeInvariantData, TimeVariantData, getUnitsCall);
        }
        
        [Test]
        public void NoCapitalAccount_GetUnits_ReturnsNull()
        {
            var noCapitalAccountTimeInvariantData = TimeInvariantData;
            noCapitalAccountTimeInvariantData.InvestmentSpendByAccountType.AccountSpendValues[0].AccountTypeName = fixture.Create<string>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, Months, noCapitalAccountTimeInvariantData, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void NullInitialEffortScheduleRealistic_GetUnits_ReturnsCalculations()
        {
            int randomIndexCheck = fixture.Create<int>() % Months;
            InitializeTimeInvariantTestData(null,
                                            WellDefinedProjectScope,
                                            InvestmentSpendByAccountType,
                                            LiningImplementationPartners,
                                            MethodologyTrackRecord,
                                            PMPreviousExperience,
                                            ProvenImplementationMethodology,
                                            SkillsToManageTheProject,
                                            TeamRightInternalSkillset,
                                            TypeOfContract,
                                            VendorIndustryExperience,
                                            VendorTechnicalExperience);
            var nullEffortScheduleDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, Months, nullEffortScheduleDto, TimeVariantData);
            
            double calculationFactor = WellDefinedProjectScope.Value
                                       + LiningImplementationPartners.Value
                                       + TeamRightInternalSkillset.Value
                                       + SkillsToManageTheProject.Value
                                       + TypeOfContract.Value
                                       + ProvenImplementationMethodology.Value
                                       + VendorIndustryExperience.Value
                                       + VendorTechnicalExperience.Value;
            var expectedResults = nullEffortScheduleDto.InvestmentSpendByAccountType.AccountSpendValues[0].SpendValues[randomIndexCheck] * calculationFactor;
            Assert.That(results[randomIndexCheck], Is.EqualTo(expectedResults).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void NullWellDefinedProjectScope_GetUnits_ReturnsCalculations()
        {
            int randomIndexCheck = fixture.Create<int>() % Months;
            InitializeTimeInvariantTestData(InitialEffortScheduleRealistic,
                                            null,
                                            InvestmentSpendByAccountType,
                                            LiningImplementationPartners,
                                            MethodologyTrackRecord,
                                            PMPreviousExperience,
                                            ProvenImplementationMethodology,
                                            SkillsToManageTheProject,
                                            TeamRightInternalSkillset,
                                            TypeOfContract,
                                            VendorIndustryExperience,
                                            VendorTechnicalExperience);
            var nullWellDefinedProjectDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, Months, nullWellDefinedProjectDto, TimeVariantData);
            
            double calculationFactor = LiningImplementationPartners.Value
                                       + TeamRightInternalSkillset.Value
                                       + SkillsToManageTheProject.Value
                                       + TypeOfContract.Value
                                       + ProvenImplementationMethodology.Value
                                       + InitialEffortScheduleRealistic.Value
                                       + VendorIndustryExperience.Value
                                       + VendorTechnicalExperience.Value;
            var expectedResults = nullWellDefinedProjectDto.InvestmentSpendByAccountType.AccountSpendValues[0].SpendValues[randomIndexCheck] * calculationFactor;
            Assert.That(results[randomIndexCheck], Is.EqualTo(expectedResults).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void NullInvestmentSpendByAccountType_GetUnits_ReturnsNull()
        {
            InitializeTimeInvariantTestData(InitialEffortScheduleRealistic,
                                            WellDefinedProjectScope,
                                            null,
                                            LiningImplementationPartners,
                                            MethodologyTrackRecord,
                                            PMPreviousExperience,
                                            ProvenImplementationMethodology,
                                            SkillsToManageTheProject,
                                            TeamRightInternalSkillset,
                                            TypeOfContract,
                                            VendorIndustryExperience,
                                            VendorTechnicalExperience);
            var nullInvestmentSpendByAccountTypeDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, Months, nullInvestmentSpendByAccountTypeDto, TimeVariantData);
            Assert.That(results, Is.Null);
        }
        
        [Test]
        public void NullLiningImplementationPartners_GetUnits_ReturnsCalculations()
        {
            int randomIndexCheck = fixture.Create<int>() % Months;
            InitializeTimeInvariantTestData(InitialEffortScheduleRealistic,
                                            WellDefinedProjectScope,
                                            InvestmentSpendByAccountType,
                                            null,
                                            MethodologyTrackRecord,
                                            PMPreviousExperience,
                                            ProvenImplementationMethodology,
                                            SkillsToManageTheProject,
                                            TeamRightInternalSkillset,
                                            TypeOfContract,
                                            VendorIndustryExperience,
                                            VendorTechnicalExperience);
            var nullLiningImplementationPartnersDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, Months, nullLiningImplementationPartnersDto, TimeVariantData);
            
            double calculationFactor = WellDefinedProjectScope.Value
                                       + TeamRightInternalSkillset.Value
                                       + SkillsToManageTheProject.Value
                                       + TypeOfContract.Value
                                       + ProvenImplementationMethodology.Value
                                       + InitialEffortScheduleRealistic.Value
                                       + VendorIndustryExperience.Value
                                       + VendorTechnicalExperience.Value;
            var expectedResults = nullLiningImplementationPartnersDto.InvestmentSpendByAccountType.AccountSpendValues[0].SpendValues[randomIndexCheck] * calculationFactor;
            Assert.That(results[randomIndexCheck], Is.EqualTo(expectedResults).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void NullImplementationMethodology_GetUnits_ReturnsCalculations()
        {
            int randomIndexCheck = fixture.Create<int>() % Months;
            InitializeTimeInvariantTestData(InitialEffortScheduleRealistic,
                                            WellDefinedProjectScope,
                                            InvestmentSpendByAccountType,
                                            LiningImplementationPartners,
                                            MethodologyTrackRecord,
                                            PMPreviousExperience,
                                            null,
                                            SkillsToManageTheProject,
                                            TeamRightInternalSkillset,
                                            TypeOfContract,
                                            VendorIndustryExperience,
                                            VendorTechnicalExperience);
            var nullImplementationMethodologyDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, Months, nullImplementationMethodologyDto, TimeVariantData);
            
            double calculationFactor = WellDefinedProjectScope.Value
                                       + LiningImplementationPartners.Value
                                       + TeamRightInternalSkillset.Value
                                       + SkillsToManageTheProject.Value
                                       + TypeOfContract.Value
                                       + InitialEffortScheduleRealistic.Value
                                       + VendorIndustryExperience.Value
                                       + VendorTechnicalExperience.Value;
            var expectedResults = nullImplementationMethodologyDto.InvestmentSpendByAccountType.AccountSpendValues[0].SpendValues[randomIndexCheck] * calculationFactor;
            Assert.That(results[randomIndexCheck], Is.EqualTo(expectedResults).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void NullSkillsToManageTheProject_GetUnits_ReturnsCalculations()
        {
            int randomIndexCheck = fixture.Create<int>() % Months;
            InitializeTimeInvariantTestData(InitialEffortScheduleRealistic,
                                            WellDefinedProjectScope,
                                            InvestmentSpendByAccountType,
                                            LiningImplementationPartners,
                                            MethodologyTrackRecord,
                                            PMPreviousExperience,
                                            ProvenImplementationMethodology,
                                            null,
                                            TeamRightInternalSkillset,
                                            TypeOfContract,
                                            VendorIndustryExperience,
                                            VendorTechnicalExperience);
            var nullSkillsToManageTheProjectDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, Months, nullSkillsToManageTheProjectDto, TimeVariantData);
            
            double calculationFactor = WellDefinedProjectScope.Value
                                       + LiningImplementationPartners.Value
                                       + TeamRightInternalSkillset.Value
                                       + TypeOfContract.Value
                                       + ProvenImplementationMethodology.Value
                                       + InitialEffortScheduleRealistic.Value
                                       + VendorIndustryExperience.Value
                                       + VendorTechnicalExperience.Value;
            var expectedResults = nullSkillsToManageTheProjectDto.InvestmentSpendByAccountType.AccountSpendValues[0].SpendValues[randomIndexCheck] * calculationFactor;
            Assert.That(results[randomIndexCheck], Is.EqualTo(expectedResults).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void NulTeamRightInternalSkillsetGetUnits_ReturnsCalculations()
        {
            int randomIndexCheck = fixture.Create<int>() % Months;
            InitializeTimeInvariantTestData(InitialEffortScheduleRealistic,
                                            WellDefinedProjectScope,
                                            InvestmentSpendByAccountType,
                                            LiningImplementationPartners,
                                            MethodologyTrackRecord,
                                            PMPreviousExperience,
                                            ProvenImplementationMethodology,
                                            SkillsToManageTheProject,
                                            null,
                                            TypeOfContract,
                                            VendorIndustryExperience,
                                            VendorTechnicalExperience);
            var nullTeamRightInternalSkillsetDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, Months, nullTeamRightInternalSkillsetDto, TimeVariantData);
            
            double calculationFactor = WellDefinedProjectScope.Value
                                       + LiningImplementationPartners.Value
                                       + SkillsToManageTheProject.Value
                                       + TypeOfContract.Value
                                       + ProvenImplementationMethodology.Value
                                       + InitialEffortScheduleRealistic.Value
                                       + VendorIndustryExperience.Value
                                       + VendorTechnicalExperience.Value;
            var expectedResults = nullTeamRightInternalSkillsetDto.InvestmentSpendByAccountType.AccountSpendValues[0].SpendValues[randomIndexCheck] * calculationFactor;
            Assert.That(results[randomIndexCheck], Is.EqualTo(expectedResults).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void NullTypeOfContract_GetUnits_ReturnsCalculations()
        {
            int randomIndexCheck = fixture.Create<int>() % Months;
            InitializeTimeInvariantTestData(InitialEffortScheduleRealistic,
                                            WellDefinedProjectScope,
                                            InvestmentSpendByAccountType,
                                            LiningImplementationPartners,
                                            MethodologyTrackRecord,
                                            PMPreviousExperience,
                                            ProvenImplementationMethodology,
                                            SkillsToManageTheProject,
                                            TeamRightInternalSkillset,
                                            null,
                                            VendorIndustryExperience,
                                            VendorTechnicalExperience);
            var nullTypeOfContractDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, Months, nullTypeOfContractDto, TimeVariantData);
            
            double calculationFactor = WellDefinedProjectScope.Value
                                       + LiningImplementationPartners.Value
                                       + TeamRightInternalSkillset.Value
                                       + SkillsToManageTheProject.Value
                                       + ProvenImplementationMethodology.Value
                                       + InitialEffortScheduleRealistic.Value
                                       + VendorIndustryExperience.Value
                                       + VendorTechnicalExperience.Value;
            var expectedResults = nullTypeOfContractDto.InvestmentSpendByAccountType.AccountSpendValues[0].SpendValues[randomIndexCheck] * calculationFactor;
            Assert.That(results[randomIndexCheck], Is.EqualTo(expectedResults).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        
        [Test]
        public void NullVendorIndustryExperience_GetUnits_ReturnsCalculations()
        {
            int randomIndexCheck = fixture.Create<int>() % Months;
            InitializeTimeInvariantTestData(InitialEffortScheduleRealistic,
                                            WellDefinedProjectScope,
                                            InvestmentSpendByAccountType,
                                            LiningImplementationPartners,
                                            MethodologyTrackRecord,
                                            PMPreviousExperience,
                                            ProvenImplementationMethodology,
                                            SkillsToManageTheProject,
                                            TeamRightInternalSkillset,
                                            TypeOfContract,
                                            null,
                                            VendorTechnicalExperience);
            var nullVendorIndustryExperienceDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, Months, nullVendorIndustryExperienceDto, TimeVariantData);
            
            double calculationFactor = WellDefinedProjectScope.Value
                                       + LiningImplementationPartners.Value
                                       + TeamRightInternalSkillset.Value
                                       + SkillsToManageTheProject.Value
                                       + TypeOfContract.Value
                                       + ProvenImplementationMethodology.Value
                                       + InitialEffortScheduleRealistic.Value
                                       + VendorTechnicalExperience.Value;
            var expectedResults = nullVendorIndustryExperienceDto.InvestmentSpendByAccountType.AccountSpendValues[0].SpendValues[randomIndexCheck] * calculationFactor;
            Assert.That(results[randomIndexCheck], Is.EqualTo(expectedResults).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void NullVendorTechnicalExperience_GetUnits_ReturnsCalculations()
        {
            int randomIndexCheck = fixture.Create<int>() % Months;
            InitializeTimeInvariantTestData(InitialEffortScheduleRealistic,
                                            WellDefinedProjectScope,
                                            InvestmentSpendByAccountType,
                                            LiningImplementationPartners,
                                            MethodologyTrackRecord,
                                            PMPreviousExperience,
                                            ProvenImplementationMethodology,
                                            SkillsToManageTheProject,
                                            TeamRightInternalSkillset,
                                            TypeOfContract,
                                            VendorIndustryExperience,
                                            null);
            var nullVendorTechnicalExperienceDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, Months, nullVendorTechnicalExperienceDto, TimeVariantData);
            
            double calculationFactor = WellDefinedProjectScope.Value
                                       + LiningImplementationPartners.Value
                                       + TeamRightInternalSkillset.Value
                                       + SkillsToManageTheProject.Value
                                       + TypeOfContract.Value
                                       + ProvenImplementationMethodology.Value
                                       + InitialEffortScheduleRealistic.Value
                                       + VendorIndustryExperience.Value;
            var expectedResults = nullVendorTechnicalExperienceDto.InvestmentSpendByAccountType.AccountSpendValues[0].SpendValues[randomIndexCheck] * calculationFactor;
            Assert.That(results[randomIndexCheck], Is.EqualTo(expectedResults).Within(CommonConstants.DoubleDifferenceTolerance));
        }
        
        [Test]
        public void NullFactors_GetUnits_ReturnsNull()
        {
            InitializeTimeInvariantTestData(null,
                                            null,
                                            InvestmentSpendByAccountType,
                                            null,
                                            MethodologyTrackRecord,
                                            PMPreviousExperience,
                                            null,
                                            null,
                                            null,
                                            null,
                                            null,
                                            null);
            var nullFactorsDto = fixture.Create<baseClass.TimeInvariantInputDTO>();
            
            var results = Formulas.GetUnits(ArbitraryStartYear, Months, nullFactorsDto, TimeVariantData);
            Assert.That(results, Is.Null);
        }
    }
}