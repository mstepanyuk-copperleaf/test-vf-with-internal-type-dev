
#### Calculation Details

**Safety Consequence**

> IF Safety Risk Consequence HAS VALUE
>
>> Safety Risk Consequence     
>
> ELSE
>
>> Safety Risk Failure Severity (from Value Measure Asset Type)

## Safety Probability of Event

> IF Safety Risk Exposure Factor HAS VALUE
>
>> Safety Risk Exposure Factor * Calculated based on Condition, Safety Risk Condition to Failure Curve (from Value Measure Asset Type)
>
> ELSE
>
>> Calculated based on Condition, Safety Risk Condition to Failure Curve (from Value Measure Asset Type)

## Total Safety Risk

> **Total Safety Risk** = Safety Consequence * Safety Probability of Event
