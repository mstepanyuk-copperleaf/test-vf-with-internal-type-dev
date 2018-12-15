
#### Value Measure Calculation Details

The value of the **Data Availability at Point of Care Benefits** is calculated as:

> Data Availability at Point of Care Benefit = 
>
> (**NumberHealthCareClientsData** * (
> **PercentMinorInjuriesAvoidedData** * *MinorInjuryCost* + **PercentMajorInjuriesAvoidedData** * *MajorInjuryCost* + 
> **PercentDisabilityAvoidedData** * *PermanentDisabilityCost* + 
> **PercentFatalityAvoidedData** * *FatalityCost*)/ 100)
> *(**ProjectRoleProbability** * **AchievingBenefitProbability**/ 100)

Where:
- *Italic* values are system configurable fields, and **bold** values are questionnaire inputs.
- The criticality impact level is aligned with public safety risk matrix:
    - Minor injury or illness = $10K per person
    - Major illness / hospitalization = $500K
    - Permanent disability = $3M
    - Fatality = $10M.…
     
    *Note that these values are placeholders that should be reviewed prior to a full implementation of the value framework by OPS*

- It is computed in dollars then calibrated to the Value Measure by applying the conversion factor of 0.001 since all other Value Measures are normalized to $1,000.
