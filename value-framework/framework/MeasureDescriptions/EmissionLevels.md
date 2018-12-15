
#### Calculation Details

The value of a change in **Emission Levels** is calculated as follows:

> Emission Levels = 
>
> Emission Reduction * CO2 Offset Cost per Ton * Benefit Probability /100

Where:

> Emission Reduction = 
>
> CO2Reduction + CO2Baseline - CO2Outcome

and,

> CO2Baseline =
>
> UnitCapacity * Capacity Factor * 8760 * CO2Rate

> CO2Outcome =
>
> IF HeatRateChange IS NOT 0 THEN 
>
>> UnitCapacity * CapacityFactor * 8760 * CO2Rate * (1+HeatRateChange/100) 
>
> ELSE UnitCapacity * CapacityFactor * 8760 * CO2Rate

This Value Measure is calculated in USD then converted to value units. 