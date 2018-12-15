
#### Calculation Details

The value of the **Cost Savings - O&M Budget Savings** is calculated as the following if the Account Type is OPEX:

> Value = Saved Costs + Saved Hours * LabourRate

Where LabourRate is a constant in the formula code, which may be different based on the account type (currently it is set to $110/hour for both CAPEX and OPEX account types).

The benefit value generated for the measure annually is equal to:

> Benefit Value = Outcome (No Baseline)

**If** the account type is CAPEX, the value of O&M Budget Savings is equal to $0.

This Value Measure is calculated in dollars. It is intended for use in a Value Function and is converted to Value Units.