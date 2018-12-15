using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.Common_Code;
using MeasureFormula.SharedCode;

namespace CustomerFormulaCode
{
    [Formula]
    public class OMCostAvoidanceConsequence : OMCostAvoidanceConsequenceBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
	        var financialBenefitType = HelperFunctions.GetCustomFieldValueAsInt(timeInvariantData.FinancialBenefitType);
            
	        return InterpolatePropagate<TimeVariantInputDTO>(timeVariantData, startFiscalYear, months,
		        x => financialBenefitType == (int) FinancialBenefitType.CostAvoidance ? x.AnnualOM: 0);
        }
        
        public override double?[] GetZynos(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData,
            IReadOnlyList<TimeVariantInputDTO> timeVariantData,
            double?[] unitOutput)
        {
	        return ConvertUnitsToZynos(unitOutput, CustomerConstants.DollarToZynoConversionFactor);
        }
    }
}
