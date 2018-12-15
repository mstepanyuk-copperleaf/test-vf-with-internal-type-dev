// GENERATED CODE - DO NOT EDIT !!!
using System.Collections.Generic;
using CL.FormulaHelper;
using CL.FormulaHelper.Attributes;
using CL.FormulaHelper.DTOs;
using System.Runtime.Serialization;

namespace MeasureFormulas.Generated_Formula_Base_Classes
{
    [FormulaBase]
    public abstract class ProductWorkplaceBenBenefitConsequenceBase : FormulaConsequenceBase
    {
        [DataContract]
        public class TimeInvariantInputDTO
        {
            public TimeInvariantInputDTO()
            {
            }
        }
        
        [DataContract]
        public class TimeVariantInputDTO : ITimeVariantInputDTO
        {
            public TimeVariantInputDTO(
                System.Double p_NumberOfCandidatesAttracted,
                System.Double p_NumberOfEmployeesAffected,
                System.Double p_NumberOfEmployeesAtRiskOfLeaving,
                CL.FormulaHelper.DTOs.TimePeriodDTO p_TimePeriod,
                CL.FormulaHelper.DTOs.CustomFieldListItemDTO p_WorkplaceImpactOnAttractiveness,
                CL.FormulaHelper.DTOs.CustomFieldListItemDTO p_WorkplaceImpactOnProductivity)
            {
                NumberOfCandidatesAttracted = p_NumberOfCandidatesAttracted;
                NumberOfEmployeesAffected = p_NumberOfEmployeesAffected;
                NumberOfEmployeesAtRiskOfLeaving = p_NumberOfEmployeesAtRiskOfLeaving;
                TimePeriod = p_TimePeriod;
                WorkplaceImpactOnAttractiveness = p_WorkplaceImpactOnAttractiveness;
                WorkplaceImpactOnProductivity = p_WorkplaceImpactOnProductivity;
            }
            
            [PromptInput("NumberOfCandidatesAttracted")]
            [DataMember]
            public System.Double NumberOfCandidatesAttracted  { get; private set; }
            
            [PromptInput("NumberOfEmployeesAffected")]
            [DataMember]
            public System.Double NumberOfEmployeesAffected  { get; private set; }
            
            [PromptInput("NumberOfEmployeesAtRiskOfLeaving")]
            [DataMember]
            public System.Double NumberOfEmployeesAtRiskOfLeaving  { get; private set; }
            
            [CoreFieldInput(FormulaCoreFieldInputType.TimePeriod)]
            [DataMember]
            public CL.FormulaHelper.DTOs.TimePeriodDTO TimePeriod  { get; private set; }
            
            [PromptInput("WorkplaceImpactOnAttractiveness")]
            [DataMember]
            public CL.FormulaHelper.DTOs.CustomFieldListItemDTO WorkplaceImpactOnAttractiveness  { get; private set; }
            
            [PromptInput("WorkplaceImpactOnProductivity")]
            [DataMember]
            public CL.FormulaHelper.DTOs.CustomFieldListItemDTO WorkplaceImpactOnProductivity  { get; private set; }
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
