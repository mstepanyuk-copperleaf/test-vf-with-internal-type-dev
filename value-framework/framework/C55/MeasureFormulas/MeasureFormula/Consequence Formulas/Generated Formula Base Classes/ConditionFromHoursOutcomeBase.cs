// GENERATED CODE - DO NOT EDIT !!!
using System.Collections.Generic;
using CL.FormulaHelper;
using CL.FormulaHelper.Attributes;
using CL.FormulaHelper.DTOs;
using System.Runtime.Serialization;

namespace MeasureFormulas.Generated_Formula_Base_Classes
{
    [FormulaBase]
    public abstract class ConditionFromHoursOutcomeBase : FormulaConsequenceBase
    {
        [DataContract]
        public class TimeInvariantInputDTO
        {
            public TimeInvariantInputDTO(
                CL.FormulaHelper.DTOs.AlternativeMilestoneSetDTO p_AlternativeMilestones,
                CL.FormulaHelper.DTOs.XYCurveDTO p_AssetConditionDecayCurve,
                System.Int32? p_AssetExpectedAnnualOperatingHours,
                System.Boolean p_AssetImpact,
                System.Int32? p_AssetOperatingHours,
                System.DateTime? p_AssetOpHrsDate,
                CL.FormulaHelper.DTOs.DistributionByAccountTypeDTO p_InvestmentSpendByAccountType,
                System.String p_SystemAlternativeMilestoneISDCode)
            {
                AlternativeMilestones = p_AlternativeMilestones;
                AssetConditionDecayCurve = p_AssetConditionDecayCurve;
                AssetExpectedAnnualOperatingHours = p_AssetExpectedAnnualOperatingHours;
                AssetImpact = p_AssetImpact;
                AssetOperatingHours = p_AssetOperatingHours;
                AssetOpHrsDate = p_AssetOpHrsDate;
                InvestmentSpendByAccountType = p_InvestmentSpendByAccountType;
                SystemAlternativeMilestoneISDCode = p_SystemAlternativeMilestoneISDCode;
            }
            
            [CoreFieldInput(FormulaCoreFieldInputType.AlternativeMilestones)]
            [DataMember]
            public CL.FormulaHelper.DTOs.AlternativeMilestoneSetDTO AlternativeMilestones  { get; private set; }
            
            [CoreFieldInput(FormulaCoreFieldInputType.AssetConditionDecayCurve)]
            [DataMember]
            public CL.FormulaHelper.DTOs.XYCurveDTO AssetConditionDecayCurve  { get; private set; }
            
            [CustomFieldInput("ExpectedAnnualOperatingHours", FormulaInputAssociatedEntity.Asset)]
            [DataMember]
            public System.Int32? AssetExpectedAnnualOperatingHours  { get; private set; }
            
            [PromptInput("AssetImpact")]
            [DataMember]
            public System.Boolean AssetImpact  { get; private set; }
            
            [CustomFieldInput("OperatingHours", FormulaInputAssociatedEntity.Asset)]
            [DataMember]
            public System.Int32? AssetOperatingHours  { get; private set; }
            
            [CustomFieldInput("OpHrsDate", FormulaInputAssociatedEntity.Asset)]
            [DataMember]
            public System.DateTime? AssetOpHrsDate  { get; private set; }
            
            [CoreFieldInput(FormulaCoreFieldInputType.InvestmentSpendByAccountType)]
            [DataMember]
            public CL.FormulaHelper.DTOs.DistributionByAccountTypeDTO InvestmentSpendByAccountType  { get; private set; }
            
            [CustomFieldInput("AlternativeMilestoneISDCode", FormulaInputAssociatedEntity.System)]
            [DataMember]
            public System.String SystemAlternativeMilestoneISDCode  { get; private set; }
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
