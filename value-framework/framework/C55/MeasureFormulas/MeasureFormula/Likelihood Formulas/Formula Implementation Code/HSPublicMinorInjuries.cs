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
    public class HSPublicMinorInjuries : HSPublicMinorInjuriesBase
    {
        public override double?[] GetLikelihoodValues(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
            if (timeInvariantData.ProbabilityOfMemberOfPublicMinorInjury == null){
    			
    			return null;
    		}else{
    			
    		var AssetFailProb =  InterpolatePropagate<TimeVariantInputDTO>(timeVariantData, startFiscalYear, months, (x => (x.ProbAssetFailureEvent/(12.0*100.0))));
    		int NoOfPublic = timeInvariantData.NumberOfMembersOfPublicAffected ?? 0;
    		double MinorInjuryProb = timeInvariantData.ProbabilityOfMemberOfPublicMinorInjury ?? 0;
    		
    		var PublicMinorInjury = NoOfPublic * MinorInjuryProb/100.0;
    		
    		var Array1 = ArrayHelper.MultiplyStreamOfValuesByConstant(AssetFailProb, PublicMinorInjury);
    		
    		return Array1;
        }
    }
}
}