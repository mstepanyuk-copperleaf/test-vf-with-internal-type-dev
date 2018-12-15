using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class FireOrExplosionLikelihood : FireOrExplosionLikelihoodBase
    {
        public override double?[] GetLikelihoodValues(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
    		
    		double?[] failureProb = InterpolatePropagate<TimeVariantInputDTO>(timeVariantData,
    		                            startFiscalYear, months, (x => x.FailureProbability));
    			
    		double?[] explosionOrFireProb = InterpolatePropagate<TimeVariantInputDTO>(timeVariantData,
                                        startFiscalYear, months, (x => x.GenAssetProbFailureCreatesHazard));

    		double?[] dangerZoneProb = InterpolatePropagate<TimeVariantInputDTO>(timeVariantData,
                                        startFiscalYear, months, (x => x.GenAssetProbPersonInDangerZone));

    		double?[] injuryProb = InterpolatePropagate<TimeVariantInputDTO>(timeVariantData,
                                        startFiscalYear, months, (x => x.GenAssetProbPersonInjured));

    		if (failureProb == null ||
    		    explosionOrFireProb == null ||
    		    dangerZoneProb == null ||
    		    injuryProb == null)
    		{
    			return null;
    		}

            double?[] result = new double?[months];

            for (int i = 0; i < months; i++){
            	result[i] = failureProb[i] * explosionOrFireProb[i] * dangerZoneProb[i] * injuryProb[i]
            		/ (1.0e8 * 12.0);
            }

            return result;
        }
    }
}
