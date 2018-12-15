
#### Calculation Details

The expected annual **O&M Budget Savings** for the asset is calculated based on its condition and the configured level thresholds and expected spend for the asset type.  The O&M spend is classified as Low Cost (high Condition value), Medium Cost (medium Condition values) or High Cost (low Condition values).

The logic for this is as follows:

> IF Condition >= OMAAnnualCostMediumConditionUpperBound
>
>> OMAAnnualCostLow
>
> ELSE IF OMAAnnualCostMediumConditionUpperBound > Condition > = OMAAnnualCostHighConditionUpperBound
>
>> OMAAnnualCostMedium
>
> ELSE
>
>> OMAAnnualCostHigh
