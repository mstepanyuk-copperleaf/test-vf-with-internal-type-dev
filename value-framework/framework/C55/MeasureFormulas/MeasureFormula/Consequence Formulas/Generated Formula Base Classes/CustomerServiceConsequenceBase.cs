// GENERATED CODE - DO NOT EDIT !!!
using System.Collections.Generic;
using CL.FormulaHelper;
using CL.FormulaHelper.Attributes;
using CL.FormulaHelper.DTOs;
using System.Runtime.Serialization;

namespace MeasureFormulas.Generated_Formula_Base_Classes
{
    [FormulaBase]
    public abstract class CustomerServiceConsequenceBase : FormulaConsequenceBase
    {
        [DataContract]
        public class TimeInvariantInputDTO
        {
            public TimeInvariantInputDTO(
                System.Int32? p_SystemAverageTotalHandlingTimePerCall,
                System.Int32? p_SystemCallsPerYear,
                System.Int32? p_SystemCSRCost,
                System.Int32? p_SystemCustTimeCost)
            {
                SystemAverageTotalHandlingTimePerCall = p_SystemAverageTotalHandlingTimePerCall;
                SystemCallsPerYear = p_SystemCallsPerYear;
                SystemCSRCost = p_SystemCSRCost;
                SystemCustTimeCost = p_SystemCustTimeCost;
            }
            
            [CustomFieldInput("AverageTotalHandlingTimePerCall", FormulaInputAssociatedEntity.System)]
            [DataMember]
            public System.Int32? SystemAverageTotalHandlingTimePerCall  { get; private set; }
            
            [CustomFieldInput("CallsPerYear", FormulaInputAssociatedEntity.System)]
            [DataMember]
            public System.Int32? SystemCallsPerYear  { get; private set; }
            
            [CustomFieldInput("CSRCost", FormulaInputAssociatedEntity.System)]
            [DataMember]
            public System.Int32? SystemCSRCost  { get; private set; }
            
            [CustomFieldInput("CustTimeCost", FormulaInputAssociatedEntity.System)]
            [DataMember]
            public System.Int32? SystemCustTimeCost  { get; private set; }
        }
        
        [DataContract]
        public class TimeVariantInputDTO : ITimeVariantInputDTO
        {
            public TimeVariantInputDTO(
                System.Int32 p_AgentTimeSaved,
                System.Int32 p_LowEffortResolutions,
                System.Int32 p_SelfServiceResolutions,
                CL.FormulaHelper.DTOs.TimePeriodDTO p_TimePeriod)
            {
                AgentTimeSaved = p_AgentTimeSaved;
                LowEffortResolutions = p_LowEffortResolutions;
                SelfServiceResolutions = p_SelfServiceResolutions;
                TimePeriod = p_TimePeriod;
            }
            
            [PromptInput("AgentTimeSaved")]
            [DataMember]
            public System.Int32 AgentTimeSaved  { get; private set; }
            
            [PromptInput("LowEffortResolutions")]
            [DataMember]
            public System.Int32 LowEffortResolutions  { get; private set; }
            
            [PromptInput("SelfServiceResolutions")]
            [DataMember]
            public System.Int32 SelfServiceResolutions  { get; private set; }
            
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
