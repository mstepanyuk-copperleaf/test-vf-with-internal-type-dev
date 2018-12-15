
#### Calculation Details

**Lost Generation Consequence**

The Lost Generation Consequence uses the Generation Group (Generation Loss and Energy Price), Is Spare Available, Downtime with Spare, and Downtime without Spare associated with the asset or asset type. In the case of analytics, it also uses the Strategy Alternative (Energy Price and Avoided CO2 Price) 

*Note: only the default Strategy Alternative is available in an Investment context.*

> Proportion of Annual Downtime:
>
> IF Is Spare Available
>
>> Proportion of Annual Downtime = Downtime with Spare / 52
>
> ELSE 
>
>> Proportion of Annual Downtime = Downtime without Spare / 52

> Weighted Annual Value (MWh) =
>
> Proportion of Annual Downtime * Generation Loss  (from Generation Group)

Energy Price is taken from the Generation Group, if it exists in the Generation Group. Otherwise, it is taken from the Strategy Alternative.

Avoided CO2 Price is taken from the Strategy Alternative, if it exists (otherwise it is set to 0).

> **Lost Generation Consequence =**
>
> Weighted Annual Value (MWh) * (Energy Price + Avoided CO2 Price)

**Lost Generation Probability of Event**

Lost Generation Risk (Legacy) is calculated using the Lost Generation Consequence as described above. The likelihood calculation follows similar logic to Lost Generation Probability of Event, but the calculation is modified to provide a result which is equivalent to the legacy Asset Analytics.

**Lost Generation Risk**

> Lost Generation Risk = Lost Generation Probability of Event * Lost Generation Consequence

The value for the Lost Generation Risk measure is the Baseline Risk minus the Oucome Risk

> Lost Generation Risk Legacy (Value) = Baseline Lost Generation Risk - Outcome Lost Generation Risk