
#### Calculation Details

The **Explosion Risk** measure is calculated using the standard approach for risk measures. It is calculated as follows:

> Risk = Consequence * Probability

**Consequence**

The consequence is set to the value of **MajorConsequence**. This value can be configured in C55.

**Probability**

The probability of the failure or event is determined through the user questionnaire.

**Explosion Risk Value**

> Explosion Risk =
>
> (FailureProbability/100)*(ExplosionFireProbability/100)*(DangerZoneProbability/100)*(InjuryProbability/100)* MajorConsequence

The value of Safety Risk is determined through the mitigated risk:

> Explosion Risk Value = Baseline Risk - Outcome Risk

