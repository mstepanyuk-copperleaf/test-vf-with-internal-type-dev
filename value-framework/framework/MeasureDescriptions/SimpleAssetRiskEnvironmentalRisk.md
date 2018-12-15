
#### Calculation Details

**Environmental Consequence**

> IF EnvironmentalRiskConsequence HAS VALUE
>
>> EnvironmentalRiskConsequence
>
> ELSE
>
>> Value Measure Asset Type – Environmental Risk Failure Severity

**Environmental Probability of Event**

> IF EnvironmentalRiskExposureFactor HAS VALUE
>
>> EnvironmentalRiskExposureFactor * Calculated based on (Condition, Value Measure Asset Type – Affordability Risk Condition to Failure Curve)
>
> ELSE
>
>> EnvironmentalRiskExposureFactor * Calculated based on (Condition, Value Measure Asset Type – Affordability Risk Condition to Failure Curve)

**Environmental Risk**

> **Environemntal Risk** = Environmental Probability of Event * Environmental Consequence
