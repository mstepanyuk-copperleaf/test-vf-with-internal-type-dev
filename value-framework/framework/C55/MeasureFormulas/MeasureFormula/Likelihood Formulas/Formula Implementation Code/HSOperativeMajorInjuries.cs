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
    public class HSOperativeMajorInjuries : HSOperativeMajorInjuriesBase
    {
        public override double?[] GetLikelihoodValues(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            if (timeInvariantData.ProbabilityOfOperativeMajorInjury == null){
    			
    			return null;
    		}else{
    			
    		var AssetFailProb =  InterpolatePropagate<TimeVariantInputDTO>(timeVariantData, startFiscalYear, months, (x => (x.ProbAssetFailureEvent/(12.0*100.0))));
    		int NoOfOperatives = timeInvariantData.NumberOfOperativesAffected ?? 0;
    		double MajorInjuryProb = timeInvariantData.ProbabilityOfOperativeMajorInjury ?? 0;
    		
    		var OperativeMajorInjury = NoOfOperatives * MajorInjuryProb/100.0;
    		
    		var Array1 = ArrayHelper.MultiplyStreamOfValuesByConstant(AssetFailProb, OperativeMajorInjury);
    		
    		return Array1;
        }
    }
}
}