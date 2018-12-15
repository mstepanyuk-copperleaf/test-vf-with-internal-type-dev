using CL.FormulaHelper.DTOs;
using MeasureFormula.SharedCode;

namespace MeasureFormula.Common_Code
{
    public static class LikelihoodProbability
    {
        public static double?[] GetLikelihoodValues(int months, XYCurveDTO conditionToFailureCurve, double?[] conditionScoreConsqUnitOutput, double? riskExposureFactor)
        {
            var exposureFactor = riskExposureFactor ?? CommonConstants.ExposureFactorFull;

            if (conditionToFailureCurve.Points == null || conditionScoreConsqUnitOutput == null) return null;

            var result = ProbabilityHelpers.ConditionalProbabilities(conditionScoreConsqUnitOutput, conditionToFailureCurve);
    		
            return HelperFunctions.ScaleValues(months, result, exposureFactor);
        }
    }
}