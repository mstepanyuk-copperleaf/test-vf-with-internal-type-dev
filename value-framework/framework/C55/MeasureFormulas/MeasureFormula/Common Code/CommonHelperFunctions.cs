using System.Linq;
using System;
using CL.FormulaHelper.DTOs;
using CL.FormulaHelper;

namespace MeasureFormula.Common_Code
{
    public static class CommonHelperFunctions
    {
        public static int? GetAlternativeMilestoneOffset(int startFiscalYear, AlternativeMilestoneSetDTO milestoneDto, string milestoneCode)
        {
            if (milestoneDto == null || string.IsNullOrEmpty(milestoneCode)) return null;

            var milestone = milestoneDto.Milestones.FirstOrDefault(x => x.Code.Equals(milestoneCode));

            //if milestone itself is null, do nothing
            if (milestone == null) return null; 

            //use actual complete date if it exists, otherwise use forecast complete date
            var milestoneDate = milestone.ActualCompleteDate ?? milestone.ForecastCompleteDate;

            //if both actual complete date and forecast complete date are null, do nothing
            if (!milestoneDate.HasValue) return null; 
            
            return FormulaBase.ConvertDateTimeToOffset(milestoneDate.Value, startFiscalYear);
        }
        
        //The following function returns the condition that corresponds to the equivalent operating hours in the vector eohInput.  Since the vector of EOH in eohInput
        //may not add up to the number of hours in a month (or may indeed exceed the number of hours in a month) this function has to calculate the condition for the
        //fiscal (or calendar month) that should be returned.
        public static double?[] ComputeConditionFromEoh(double?[] eohInput, XYCurveDTO conditionCurve, int startFiscalYear, int months)
        {
            var conditionVector = new double?[months];
            var condIndex = 0;          //Index into conditionm vector (assumes that the X,Y points are ordered in ascending age!

            if (conditionCurve == null || conditionCurve.Points.Length < 1)
            {
                return null;
            }

            for (var i = 0; i < months; i++)
            {
                if (eohInput[i].HasValue)
                {
                    var hourTotal = eohInput[i].Value; //Total hours accumulated from EOH vector
                    //Check that the hours accumulated are less than or equal to the next point in the condition curve.  If not increment the index into the
                    //condition vector but ernsure that the index condIndex remains less than the total length of the condition vector.
                    if (hourTotal >= conditionCurve.Points[condIndex].X &&
                        condIndex < conditionCurve.Points.Length - 1)
                    {
                        condIndex++;
                    }

                    //Compute the slope of the condition curve between the point condIndex and condIndex-1.
                    var slope = (conditionCurve.Points[condIndex].Y - conditionCurve.Points[condIndex - 1].Y) /
                                (conditionCurve.Points[condIndex].X - conditionCurve.Points[condIndex - 1].X);
                    //Now compute the value of the condition for the month i...
                    var temp = conditionCurve.Points[condIndex - 1].Y + slope * hourTotal;
                    conditionVector[i] = temp < 0 ? 0 : temp > 10 ? 10 : temp;
                }
            }

            return conditionVector;
        }
        
        public static int? GetImpactOffset(int? milestoneOffset, DistributionByAccountTypeDTO investmentSpendByAccountType)
        {
            return GetImpactOffsetWithAccountType(milestoneOffset, investmentSpendByAccountType, accountType: null);
        }
        
        public static int? GetImpactOffsetWithAccountType(int? milestoneOffset, DistributionByAccountTypeDTO investmentSpendByAccountType, string accountType)
        {
            //if milestone offset exists, just use that
            if (milestoneOffset.HasValue) return milestoneOffset;

            var impactOffset = string.IsNullOrEmpty(accountType) ?
                                   FormulaBase.FindEndOfSpendMonth(investmentSpendByAccountType) :
                                   FormulaBase.FindEndOfSpendMonthForAccountType(investmentSpendByAccountType, accountType);

            //in the case of no spend, we can't do anything else
            if (!impactOffset.HasValue) return null;

            //impact will be in first month after end of spend
            return ++impactOffset;
        }
        
        public static double?[] scaleValues(int startFiscalYear, int months, double?[] inputValues, TimeSeriesDTO timeSeries)
        {
            if (inputValues == null || timeSeries == null)
            {
                return null;
            }

            var result = new double?[months];
            for (int i = 0; i < months; i++)
            {
                if (inputValues[i] == null)
                    continue;

                result[i] = timeSeries.GetMonthlyValue(startFiscalYear, i) * inputValues[i];
            }
            return result;
        }

    }
}