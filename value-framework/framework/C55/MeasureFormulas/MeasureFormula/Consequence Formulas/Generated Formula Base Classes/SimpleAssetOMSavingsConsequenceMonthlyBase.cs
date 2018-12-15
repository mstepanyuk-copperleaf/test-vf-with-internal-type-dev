// GENERATED CODE - DO NOT EDIT !!!
using System.Collections.Generic;
using CL.FormulaHelper;
using CL.FormulaHelper.Attributes;
using CL.FormulaHelper.DTOs;
using System.Runtime.Serialization;

namespace MeasureFormulas.Generated_Formula_Base_Classes
{
    [FormulaBase]
    public abstract class SimpleAssetOMSavingsConsequenceMonthlyBase : FormulaConsequenceBase
    {
        [DataContract]
        public class TimeInvariantInputDTO
        {
            public TimeInvariantInputDTO(
                System.Int32? p_AssetTypeOMAAnnualCostHigh,
                System.Double? p_AssetTypeOMAAnnualCostHighConditionUpperBound,
                System.Int32? p_AssetTypeOMAAnnualCostLow,
                System.Int32? p_AssetTypeOMAAnnualCostMedium,
                System.Double? p_AssetTypeOMAAnnualCostMediumConditionUpperBound,
                System.Double?[] p_ConditionScore_Condition_ConsqUnitOutput)
            {
                AssetTypeOMAAnnualCostHigh = p_AssetTypeOMAAnnualCostHigh;
                AssetTypeOMAAnnualCostHighConditionUpperBound = p_AssetTypeOMAAnnualCostHighConditionUpperBound;
                AssetTypeOMAAnnualCostLow = p_AssetTypeOMAAnnualCostLow;
                AssetTypeOMAAnnualCostMedium = p_AssetTypeOMAAnnualCostMedium;
                AssetTypeOMAAnnualCostMediumConditionUpperBound = p_AssetTypeOMAAnnualCostMediumConditionUpperBound;
                ConditionScore_Condition_ConsqUnitOutput = p_ConditionScore_Condition_ConsqUnitOutput;
            }
            
            [CustomFieldInput("OMAAnnualCostHigh", FormulaInputAssociatedEntity.AssetType)]
            [DataMember]
            public System.Int32? AssetTypeOMAAnnualCostHigh  { get; private set; }
            
            [CustomFieldInput("OMAAnnualCostHighConditionUpperBound", FormulaInputAssociatedEntity.AssetType)]
            [DataMember]
            public System.Double? AssetTypeOMAAnnualCostHighConditionUpperBound  { get; private set; }
            
            [CustomFieldInput("OMAAnnualCostLow", FormulaInputAssociatedEntity.AssetType)]
            [DataMember]
            public System.Int32? AssetTypeOMAAnnualCostLow  { get; private set; }
            
            [CustomFieldInput("OMAAnnualCostMedium", FormulaInputAssociatedEntity.AssetType)]
            [DataMember]
            public System.Int32? AssetTypeOMAAnnualCostMedium  { get; private set; }
            
            [CustomFieldInput("OMAAnnualCostMediumConditionUpperBound", FormulaInputAssociatedEntity.AssetType)]
            [DataMember]
            public System.Double? AssetTypeOMAAnnualCostMediumConditionUpperBound  { get; private set; }
            
            [MeasureInput("ConditionScore", "Condition", MeasureOutputType.ConsqUnitOutput, false)]
            [DataMember]
            public System.Double?[] ConditionScore_Condition_ConsqUnitOutput  { get; private set; }
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
