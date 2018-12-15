// GENERATED CODE - DO NOT EDIT !!!
using System.Collections.Generic;
using CL.FormulaHelper;
using CL.FormulaHelper.Attributes;
using CL.FormulaHelper.DTOs;
using System.Runtime.Serialization;

namespace MeasureFormulas.Generated_Formula_Base_Classes
{
    [FormulaBase]
    public abstract class EmployeeProductivityBenefitConsequenceBase : FormulaConsequenceBase
    {
        [DataContract]
        public class TimeInvariantInputDTO
        {
            public TimeInvariantInputDTO(
                System.Double? p_SystemFieldEmployeeCostPerHour,
                System.Double? p_SystemManagerCostPerHour,
                System.Double? p_SystemOfficeEmployeeCostPerHour,
                System.Double? p_SystemProbabilityOfRepurposing)
            {
                SystemFieldEmployeeCostPerHour = p_SystemFieldEmployeeCostPerHour;
                SystemManagerCostPerHour = p_SystemManagerCostPerHour;
                SystemOfficeEmployeeCostPerHour = p_SystemOfficeEmployeeCostPerHour;
                SystemProbabilityOfRepurposing = p_SystemProbabilityOfRepurposing;
            }
            
            [CustomFieldInput("FieldEmployeeCostPerHour", FormulaInputAssociatedEntity.System)]
            [DataMember]
            public System.Double? SystemFieldEmployeeCostPerHour  { get; private set; }
            
            [CustomFieldInput("ManagerCostPerHour", FormulaInputAssociatedEntity.System)]
            [DataMember]
            public System.Double? SystemManagerCostPerHour  { get; private set; }
            
            [CustomFieldInput("OfficeEmployeeCostPerHour", FormulaInputAssociatedEntity.System)]
            [DataMember]
            public System.Double? SystemOfficeEmployeeCostPerHour  { get; private set; }
            
            [CustomFieldInput("ProbabilityOfRepurposing", FormulaInputAssociatedEntity.System)]
            [DataMember]
            public System.Double? SystemProbabilityOfRepurposing  { get; private set; }
        }
        
        [DataContract]
        public class TimeVariantInputDTO : ITimeVariantInputDTO
        {
            public TimeVariantInputDTO(
                System.Int32 p_FieldHoursAdditional,
                System.Int32 p_FieldHoursSaved,
                System.Int32 p_ManagerHoursAdditional,
                System.Int32 p_ManagerHoursSaved,
                System.Int32 p_NoFieldEmployees,
                System.Int32 p_NoManagerEmployees,
                System.Int32 p_NoOfficeEmployees,
                System.Int32 p_OfficeHoursAdditional,
                System.Int32 p_OfficeHoursSaved,
                CL.FormulaHelper.DTOs.TimePeriodDTO p_TimePeriod)
            {
                FieldHoursAdditional = p_FieldHoursAdditional;
                FieldHoursSaved = p_FieldHoursSaved;
                ManagerHoursAdditional = p_ManagerHoursAdditional;
                ManagerHoursSaved = p_ManagerHoursSaved;
                NoFieldEmployees = p_NoFieldEmployees;
                NoManagerEmployees = p_NoManagerEmployees;
                NoOfficeEmployees = p_NoOfficeEmployees;
                OfficeHoursAdditional = p_OfficeHoursAdditional;
                OfficeHoursSaved = p_OfficeHoursSaved;
                TimePeriod = p_TimePeriod;
            }
            
            [PromptInput("FieldHoursAdditional")]
            [DataMember]
            public System.Int32 FieldHoursAdditional  { get; private set; }
            
            [PromptInput("FieldHoursSaved")]
            [DataMember]
            public System.Int32 FieldHoursSaved  { get; private set; }
            
            [PromptInput("ManagerHoursAdditional")]
            [DataMember]
            public System.Int32 ManagerHoursAdditional  { get; private set; }
            
            [PromptInput("ManagerHoursSaved")]
            [DataMember]
            public System.Int32 ManagerHoursSaved  { get; private set; }
            
            [PromptInput("NoFieldEmployees")]
            [DataMember]
            public System.Int32 NoFieldEmployees  { get; private set; }
            
            [PromptInput("NoManagerEmployees")]
            [DataMember]
            public System.Int32 NoManagerEmployees  { get; private set; }
            
            [PromptInput("NoOfficeEmployees")]
            [DataMember]
            public System.Int32 NoOfficeEmployees  { get; private set; }
            
            [PromptInput("OfficeHoursAdditional")]
            [DataMember]
            public System.Int32 OfficeHoursAdditional  { get; private set; }
            
            [PromptInput("OfficeHoursSaved")]
            [DataMember]
            public System.Int32 OfficeHoursSaved  { get; private set; }
            
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
