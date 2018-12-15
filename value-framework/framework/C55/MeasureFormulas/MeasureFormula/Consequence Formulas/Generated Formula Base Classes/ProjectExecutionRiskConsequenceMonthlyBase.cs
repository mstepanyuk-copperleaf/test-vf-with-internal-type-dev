// GENERATED CODE - DO NOT EDIT !!!
using System.Collections.Generic;
using CL.FormulaHelper;
using CL.FormulaHelper.Attributes;
using CL.FormulaHelper.DTOs;
using System.Runtime.Serialization;

namespace MeasureFormulas.Generated_Formula_Base_Classes
{
    [FormulaBase]
    public abstract class ProjectExecutionRiskConsequenceMonthlyBase : FormulaConsequenceBase
    {
        [DataContract]
        public class TimeInvariantInputDTO
        {
            public TimeInvariantInputDTO(
                CL.FormulaHelper.DTOs.CustomFieldListItemDTO p_AreInitialEffortAndScheduleRealistic,
                CL.FormulaHelper.DTOs.CustomFieldListItemDTO p_HowWellDefinedWillProjectScopeBe,
                CL.FormulaHelper.DTOs.DistributionByAccountTypeDTO p_InvestmentSpendByAccountType,
                CL.FormulaHelper.DTOs.CustomFieldListItemDTO p_LiningUpRightImplementationPartners,
                System.String p_MethodologyAndTrackRecord,
                System.String p_PMsPreviousExperience,
                CL.FormulaHelper.DTOs.CustomFieldListItemDTO p_ProvenImplementationMethodology,
                CL.FormulaHelper.DTOs.CustomFieldListItemDTO p_SkillsToManageTheProject,
                CL.FormulaHelper.DTOs.CustomFieldListItemDTO p_TeamsRightInternalSkillset,
                CL.FormulaHelper.DTOs.CustomFieldListItemDTO p_TypeOfContract,
                CL.FormulaHelper.DTOs.CustomFieldListItemDTO p_VendorIndustryExperience,
                CL.FormulaHelper.DTOs.CustomFieldListItemDTO p_VendorTechnicalExperience)
            {
                AreInitialEffortAndScheduleRealistic = p_AreInitialEffortAndScheduleRealistic;
                HowWellDefinedWillProjectScopeBe = p_HowWellDefinedWillProjectScopeBe;
                InvestmentSpendByAccountType = p_InvestmentSpendByAccountType;
                LiningUpRightImplementationPartners = p_LiningUpRightImplementationPartners;
                MethodologyAndTrackRecord = p_MethodologyAndTrackRecord;
                PMsPreviousExperience = p_PMsPreviousExperience;
                ProvenImplementationMethodology = p_ProvenImplementationMethodology;
                SkillsToManageTheProject = p_SkillsToManageTheProject;
                TeamsRightInternalSkillset = p_TeamsRightInternalSkillset;
                TypeOfContract = p_TypeOfContract;
                VendorIndustryExperience = p_VendorIndustryExperience;
                VendorTechnicalExperience = p_VendorTechnicalExperience;
            }
            
            [PromptInput("AreInitialEffortAndScheduleRealistic")]
            [DataMember]
            public CL.FormulaHelper.DTOs.CustomFieldListItemDTO AreInitialEffortAndScheduleRealistic  { get; private set; }
            
            [PromptInput("HowWellDefinedWillProjectScopeBe")]
            [DataMember]
            public CL.FormulaHelper.DTOs.CustomFieldListItemDTO HowWellDefinedWillProjectScopeBe  { get; private set; }
            
            [CoreFieldInput(FormulaCoreFieldInputType.InvestmentSpendByAccountType)]
            [DataMember]
            public CL.FormulaHelper.DTOs.DistributionByAccountTypeDTO InvestmentSpendByAccountType  { get; private set; }
            
            [PromptInput("LiningUpRightImplementationPartners")]
            [DataMember]
            public CL.FormulaHelper.DTOs.CustomFieldListItemDTO LiningUpRightImplementationPartners  { get; private set; }
            
            [PromptInput("MethodologyAndTrackRecord")]
            [DataMember]
            public System.String MethodologyAndTrackRecord  { get; private set; }
            
            [PromptInput("PMsPreviousExperience")]
            [DataMember]
            public System.String PMsPreviousExperience  { get; private set; }
            
            [PromptInput("ProvenImplementationMethodology")]
            [DataMember]
            public CL.FormulaHelper.DTOs.CustomFieldListItemDTO ProvenImplementationMethodology  { get; private set; }
            
            [PromptInput("SkillsToManageTheProject")]
            [DataMember]
            public CL.FormulaHelper.DTOs.CustomFieldListItemDTO SkillsToManageTheProject  { get; private set; }
            
            [PromptInput("TeamsRightInternalSkillset")]
            [DataMember]
            public CL.FormulaHelper.DTOs.CustomFieldListItemDTO TeamsRightInternalSkillset  { get; private set; }
            
            [PromptInput("TypeOfContract")]
            [DataMember]
            public CL.FormulaHelper.DTOs.CustomFieldListItemDTO TypeOfContract  { get; private set; }
            
            [PromptInput("VendorIndustryExperience")]
            [DataMember]
            public CL.FormulaHelper.DTOs.CustomFieldListItemDTO VendorIndustryExperience  { get; private set; }
            
            [PromptInput("VendorTechnicalExperience")]
            [DataMember]
            public CL.FormulaHelper.DTOs.CustomFieldListItemDTO VendorTechnicalExperience  { get; private set; }
        }
        
        [DataContract]
        public class TimeVariantInputDTO : ITimeVariantInputDTO
        {
            public TimeVariantInputDTO(
                CL.FormulaHelper.DTOs.TimePeriodDTO p_TimePeriod)
            {
                TimePeriod = p_TimePeriod;
            }
            
            [CoreFieldInput(FormulaCoreFieldInputType.TimePeriod)]
            [DataMember]
            public CL.FormulaHelper.DTOs.TimePeriodDTO TimePeriod  { get; private set; }
        }
        
        public abstract double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData);
            
        public abstract double?[] GetZynos(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData,
            IReadOnlyList<TimeVariantInputDTO> timeVariantData,
            double?[] unitOutput);
        
        ///
        /// Class to enable formula debugging
        ///
        [DataContract]
        public class FormulaParams : IFormulaParams
        {
            public FormulaParams(CL.FormulaHelper.MeasureOutputType measureOutputType,
                string measureName,
                long alternativeId,
                string formulaImplClassName,
                bool isProbabilityFormula,
                int fiscalYearEndMonth,
                int startFiscalYear,
                int months,
                TimeInvariantInputDTO timeInvariantData,
                IReadOnlyList<TimeVariantInputDTO> timeVariantData,
                double?[] unitOutput,
                double?[] formulaOutput,
                string exceptionMessage)
            {
                MeasureOutputType = measureOutputType;
                MeasureName = measureName;
                AlternativeId = alternativeId;
                FormulaImplClassName = formulaImplClassName;
                IsProbabilityFormula = isProbabilityFormula;
                FiscalYearEndMonth = fiscalYearEndMonth;
                StartFiscalYear = startFiscalYear;
                Months = months;
                TimeInvariantData = timeInvariantData;
                TimeVariantData = timeVariantData;
                UnitOutput = unitOutput;
                FormulaOutput = formulaOutput;
                ExceptionMessage = exceptionMessage;
            }
            [DataMember(Order = 0)]
            public CL.FormulaHelper.MeasureOutputType MeasureOutputType { get; set; }
            [DataMember(Order = 1)]
            public string MeasureName { get; set; }
            [DataMember(Order = 2)]
            public long AlternativeId { get; set; }
            [DataMember(Order = 3)]
            public string FormulaImplClassName { get; set; }
            [DataMember(Order = 4)]
            public bool IsProbabilityFormula { get; set; }
            [DataMember(Order = 5)]
            public int FiscalYearEndMonth { get; set; }
            [DataMember(Order = 6)]
            public int StartFiscalYear { get; set; }
            [DataMember(Order = 7)]
            public int Months { get; set; }
            [DataMember(Order = 8)]
            public TimeInvariantInputDTO TimeInvariantData { get; set; }
            [DataMember(Order = 9)]
            public IReadOnlyList<TimeVariantInputDTO> TimeVariantData { get; set; }
            [DataMember(Order = 10)]
            public double?[] UnitOutput { get; set; }
            [DataMember(Order = 11)]
            public double?[] FormulaOutput { get; set; }
            [DataMember(Order = 12)]
            public string ExceptionMessage { get; set; }
        }
    }
}
// GENERATED CODE - DO NOT EDIT !!!
