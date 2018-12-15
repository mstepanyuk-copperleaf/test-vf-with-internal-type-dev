using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;
using MeasureFormula.Common_Code;
using MeasureFormula.SharedCode;

namespace CustomerFormulaCode
{
    [Formula]
    public class ComplaintRisk : ComplaintRiskBase
    {
        public override double?[] GetUnits(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
    		double SystemCostOfComplaint = timeInvariantData.SystemCostOfComplaint ?? 0;

	        return null;
	        // TODO - add this measure
//    		double?[] NoOfComplaints = timeInvariantData.Complaints_ExpectedNumberOfComplaints_LikelihoodUnitOutput;
    		
  //  		double CostOfComplaint = timeInvariantData.CostOfComplaint ?? SystemCostOfComplaint;
    		
  //  		return ArrayHelper.MultiplyStreamOfValuesByConstant(NoOfComplaints, CostOfComplaint);
    		    		
        }
        
        public override double?[] GetZynos(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData,
            IReadOnlyList<TimeVariantInputDTO> timeVariantData,
            double?[] unitOutput)
        {
    		return unitOutput;
        }
    }
}
