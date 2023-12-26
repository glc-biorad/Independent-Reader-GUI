# Independent Reader GUI

## Overview
Independent Reader GUI is a tool for utilizing the full functionality of the Independent Reader Module. It allows user to view all running submodule metrological data and connective status, control submodules, create/edit dPCR thermocycling protocols on 4 individual thermocycling heaters, run protocols on these heaters, image dPCR cartridges, perform full workflow runs (image, thermocycle, image, etc.), and set default settings/configurations for the instrument.

## To-Do
### Home Tab
- Check motor positions every N seconds to update the Home tab (use MotorManagers class)
- At Form loading check if the LEDs are connected (setup LED queries and commands via the BRADx-API code repo)
- Check the Actual Temperature of the object and sink for all TECs using the TECsManager class every N seconds
- At Form loading load in the TECs DataGridView values of interest
### Run Tab
- Update X0, Y0, Z0, FOV dX, and dY based on the Cartridge, Elastomer, Bergquist, and Glass Offset. If this combonation is not found in the ScanningData XML file, add it on the onset of the run
- Update Bergquist and Elastomer Thickness TextBox values based on changes with the Elastomer and Bergquist ComboBox selections
- Block the user if they attempt a run without an experiment name or any samples/assays set
- Warn the user if they attempt a run without contact surface, no data save locations, and no imaging before, during, or after the run.
- Update the time calculation used to predict the end time based on the number of channels used, assays, and samples
- Complete the Run button functionality
  - Warn or block the user
  - Generate the prerun report
  - Image before if necessary
  - Start thermocycling protocol on the chosen heater
  - Image during if necessary
  - Image after if necessary
  - Stitch images if necessary
  - Update the prerun report with images
  - Move data to the Image Analysis pipeline
  - Move data to an S3 bucket or Sharepoint or a local path
  - Email the user the prerun report and an update on their run
  - Start the image analysis pipeline
  - Generate the postrun report
  - Email the user the postrun report
### Control Tab
- Change the Home column of the Control tab's Motor DataGridView to a ComboBox so that users can set it to "Homed" or "Not Homed" when the GUI is opened, then update the Home tab
- Link the ComboBox States and Intensity TextBox in the LEDs DataGridView with the BRADx-API
- 

## Technology Used
- C#
- .NET Framework 6.0
- Window Forms
- OxyPlot.WindowsForms 2.1.2 for Charting
- Bonsai.Spinnaker 0.7.1 for using the Spinnaker SDK (FLIR Camera)
- Desktop Developer for C++ on Visual Studio
- iText7 7.25 for PDF report generation

## Getting Started
### Prerequists
- Visual Studio 2022
- .NET Framework 6.0
### Installation
1. Clone the repo
2. Spinnaker SDK did not work unless Desktop Development for C++ was included from the Visual Studio Installer. I attempted this from a hint given by ChatGPT-4 "The Spinnaker SDK may depend on the Visual C++ Redistributable for Visual Studio 2015 (as suggested by the 'v140' in the DLL name). "
3. Open the solution in Visual Studio and build the project

## Usage

## Features

## License

## Contact
- Author: G. Lopez-Candales
- Email: gabriel_lopez-candales@bio-rad.com

## Acknowledgments
- [OxyPlot](https://github.com/oxyplot/oxyplot)
- [Bonsai.Spinnaker](https://github.com/bonsai-rx/spinnaker)
- [iText7](https://github.com/itext/itext7-dotnet)
