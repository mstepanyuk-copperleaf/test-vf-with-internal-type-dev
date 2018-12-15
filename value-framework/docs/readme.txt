The BASE VF can be loaded as follows:

1. Load the XY curves manually (this is not part of the Value Framework per se, but it is a pre-requisite):
- spreadsheet: BaseLibrary_PreLoader.xlsx
- use the Controller sheet (Universal Loader)

2. Clear the Value Framework, from the Maintenance Tools page
- this is required for this release of the framework
- this will remove any existing questionnaire answers

3. Load the Value Framework using:
- spreadsheet: BaseLibrary_FrameworkImporter.xlsx
- use the Controller sheet (Universal Loader)

4. If desired, load data for testing:
- spreadsheet: BaseLibrary_AncillaryData.xlsx
- edit the Controller sheet to remove the IGNORE for whichever data is required, e.g.:
   - Value Function and Value Function Value Measure
   - Value Measure Asset Type
   - System Config Field Values
- then use the Controller sheet (Universal Loader)
- NOTE: if this is a brand new "empty" database, you may also need to add Custom Rate Types via the C55 UI.
