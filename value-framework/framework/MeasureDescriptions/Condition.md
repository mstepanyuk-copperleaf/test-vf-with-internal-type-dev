
#### Calculation Details

**Condition - Baseline**

> Baseline Condition is calculated from the In-Service date forward based on the Condition Decay Curve.  If a ConditionScore is specified, this is applied at the selected date and then the degradation curve is re-applied from that point forward.

**Condition - Outcome**

> If AssetImpact is set to false, no outcome condition is calculated.

> If an OutcomeConditionScore is specified, this value is applied at the selected date and then the degradation curve is calculated from that point forward. Otherwise, a condition score of 10 at the end of investment spend is assumed and the degradation curve is calculated from that point based on the Condition Decay Curve.  

