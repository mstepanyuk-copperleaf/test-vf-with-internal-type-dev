using System.Linq;
using System;
using CL.FormulaHelper.DTOs;

namespace MeasureFormula.Common_Code
{
    public static class ResourceSupply
    {
        /// <summary>
        /// Returns a value in resource units for the resource code specified in CustomerConstants.cs
        /// This value is not converted to Zynos as it is expected that the dollar value will be captured through other forecast value measures.
        /// Intended primarily for use as a constraint or reporting metric.
        /// </summary>
        
        public static double?[] GetSpendForResourceType(int months, DistributionByResourceDTO listOfDistributionByResourceSupplyDto, string resourceType)
        {
            if (listOfDistributionByResourceSupplyDto == null) return null;
            
            var result = new double?[months];
			
            //Find the resource DTO in the list of DTOs that corresponds to the resource code that we are interested in
            var resourceSupplyValue = listOfDistributionByResourceSupplyDto.ResourceValues
                .FirstOrDefault( x => x.ResourceCode.Equals(resourceType, StringComparison.OrdinalIgnoreCase));
			
            if(resourceSupplyValue != null && resourceSupplyValue.SpendValues != null ){
                foreach(var kvp in resourceSupplyValue.SpendValues){
                    if (kvp.Key >= 0 && kvp.Key < months)
                    {
                        result[kvp.Key] = kvp.Value.UnitValue;
                    }
                }
            }
            else
            {
                return null;
            }
            return result;
        }
    }
}