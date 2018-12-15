
#### Calculation Details

**Financial Risk Consequence**

> IF FinancialRiskConsequence HAS VALUE
>
>> FinancialRiskConsequence
>
> ELSE
>
>> Value Measure Asset Type – Financial Risk Failure Severity

**Financial Probability of Event**

> IF FinancialRiskExposureFactor HAS VALUE
>
>> FinancialRiskExposureFactor * Calculated based on (Condition, Value Measure Asset Type – Financial Risk Condition to Failure Curve)
>
> ELSE
>
>> FinancialRiskExposureFactor * Calculated based on (Condition, Value Measure Asset Type – Financial Risk Condition to Failure Curve)

**Financial Risk**

> **Financial Risk** = Financial Probability of Event * Financial Consequence
