using System.Collections.Generic;
using CL.FormulaHelper.Attributes;
using System.Linq;
using System;
using CL.FormulaHelper;
using MeasureFormulas.Generated_Formula_Base_Classes;

namespace CustomerFormulaCode
{
    [Formula]
    public class IdentityFormulaScaledEndOfSpendLikelihood : IdentityFormulaScaledEndOfSpendLikelihoodBase
    {
        public override double?[] GetLikelihoodValues(int startFiscalYear, int months,
            TimeInvariantInputDTO timeInvariantData, IReadOnlyList<TimeVariantInputDTO> timeVariantData)
        {
    		
            var result = new double?[months];
			var value = 1.0;

			FillWithValueAfterEndOfSpend(months, timeInvariantData.InvestmentSpendByAccountType, ref result, value);

			return result;    		
    		
        }
    }
}
