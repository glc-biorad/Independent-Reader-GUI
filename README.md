# Independent Reader GUI

## Overview
Independent Reader GUI is a tool for utilizing the full functionality of the Independent Reader Module. It allows user to view all running submodule metrological data and submodule status, control submodules, create/edit dPCR thermocycling protocols on 4 individual thermocycling TEC heaters, run protocols on these heaters simultaniously or interwoven, image dPCR cartridges (before, during, and/or after dPCR), perform full workflow runs (image, thermocycle, image, etc.), set default settings/configurations for the instrument, and update/create data entries for new cartridges, elastomer, bergquists, and/or cartridge scanning parameters. The intent is to provide a single use application for all possible needs for all types of users (admin, FSE, scientist, lab tech, etc). 

## To-Do (Future Updates and Features)
### Assay Protocol Editor
- Add all types of Assay Protocol Actions
  - Mix
    - Vertical
    - Horizontal
    - Standing
  - Move
    - Absolute
    - Relative
  - Transfer
  - Delay
  - Pipette
    - Aspirate
    - Dispense
    - Detection (LLD)
  - Pool
  - Tips
    - Eject
    - Pickup
- Add Action Button
- Edit Action Button
- Copy, Cut and Paste Functionality
- Remove Action Button
### Coordinate Ediotr
- Define coordinates using XML
### Home Tab
- Add current camera exposure to the Camera DataGridView
- Add a DataGridView to show relay value states 
- Figure out a way to determine if the unit has been homed or not yet
- Add a column to the Motor DataGridView to show the IO (Moving, Homing, Idle, Error)
- Recheck the version for motors if the loaded value doesnt have the form v#.#.#
### Run Tab
- Update X0, Y0, Z0, FOV dX, and dY based on the Cartridge, Elastomer, Bergquist, and Glass Offset. If this combonation is not found in the ScanningData XML file, add it on the onset of the run
- Update Bergquist and Elastomer Thickness TextBox values based on changes with the Elastomer and Bergquist ComboBox selections
- Block the user if they attempt a run without an experiment name or any samples/assays set
- Warn the user if they attempt a run without contact surface, no data save locations, and no imaging before, during, or after the run.
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
- Add current camera exposure to the Camera DataGridView and enable users to change it
- Add a DataGridView to show relay value states and allow users to turn them on and off (add a check status to API so we can warn user if the mechanical switch is pulled)
- Figure out a way to determine if the unit has been homed or not yet
- Add a column to the Motor DataGridView to show the IO (Moving, Homing, Idle, Error)
- Recheck the version for motors if the loaded value doesnt have the form v#.#.#
- Change the Home column of the Control tab's Motor DataGridView to a ComboBox so that users can set it to "Homed" or "Not Homed" when the GUI is opened, then update the Home tab
- Link the ComboBox States and Intensity TextBox in the LEDs DataGridView with the BRADx-API
### Imaging Tab
- Add kill scanning functionality
- Add handlers for when the cartridge, heater, glass offset, elastomer, and or bergquist change for the scanning parameters
- Update the LEDs DataGridView for scanning
### Settings Tab
- Add setting DataGridViews for each submodule
### Configure Tab
- Fix update fucntionality to the scanning parameters
- Better handle changes to the scanning parameters 

## Technology Used
- C#
- .NET Framework 6.0
- Window Forms
- OxyPlot.WindowsForms 2.1.2 for Charting
- Desktop Developer for C++ on Visual Studio
- iText7 7.25 for PDF report generation
- SSH.NET for uploading via SSH
- AForge 2.2.5 for Sharpening Images

## Getting Started
### Prerequists
- Visual Studio 2022
- .NET Framework 6.0
- Spinnaker SDK (version 3.1.0.79)
  - Go to the Spinnaker Software Download page on the [Spinnaker Teledyne FLIR website](https://www.flir.com/products/spinnaker-sdk/?vertical=machine+vision&segment=iis)
  - Click the download button (will need to sign in or create an account)
  - Look for version 3.1.0.79 and download it for Windows
  - Unzip it
  - Use the Wizard to set up the SDK for Application Development
### Installation
1. Clone the repo
2. Open the solution in Visual Studio and build the project

## Usage
### Start Up
#### Instrument 
1. Ensure the instrument is powered.
2. Connect the TEC RS485-to-USB into the computer (allows communication with the TECs)
3. Connect the FLIR Camera USB into the computer (allows for communication with the Reader Camera)
4. Connect the Chassis Board USB into the computer (allows for communication with the Chassis Board Bus Controller)
#### API
1. Start the [API Server](https://github.com/glc-biorad/BRADx-API)
   -  Clone the BRADx-API repo (C:\Users\u112958\source\repos\)
   -  Navigate to the BRADx-API directory ($ cd BRADx-API\BRADx-API)
   -  Execute the start_server.py script (starts the FastAPI local server and allows access to the SwaggerUI at 127.0.0.1:800\docs)
#### Independent Reader GUI Application 
1. Open the GUI
### Functionality
#### Home Tab
##### Overview
This tab lays out all submodule attributes, these include connectivity statuses, submodule IO, current positions for motors, firmware versions loaded on the submodules, etc. Everything on this tab is ReadOnly with respect to the user. The purpose of this tab is to show the user the current status for all submodules in a single page with clear indication if a submodule is disconnected.
##### Features
- Motors
  - Status (Connected or Not Connected)
  - Version (Firmware Version loaded onto the motor board)
  - IO (Moving, Idle, Homing, etc.)
  - Current Position (μS)
  - Set Speed (μS/s)
  - Homed (Yes or No)
- TECs
  - Status (Connected or Not Connected)
  - Version (Firmware Version loaded onto the motor board)
  - IO (Ramping Up, Ramping Down, Idle, Error, etc.)
  - Actual Object Temperature (°C)
  - Target Object Temperature (°C)
  - Actual Sink Temperature (°C)
  - Fan Speed (rpm)
- Camera
  - Status (Connected or Not Connected)
  - IO (Streaming, Imaging, Idle, etc.)
- LEDs
#### Run Tab
#### Control Tab
#### Thermocycling Tab
#### Imaging Tab
#### Settings Tab
#### Consumables Tab
#### Metrology Tab

## Features

## Troubleshooting
### Spinnaker SDK BadImage Error
- Spinnaker SDK did not work unless Desktop Development for C++ was included from the Visual Studio Installer. I attempted this from a hint given by ChatGPT-4 "The Spinnaker SDK may depend on the Visual C++ Redistributable for Visual Studio 2015 (as suggested by the 'v140' in the DLL name). "
- If installing the Desktop Development for C++ via the Visual Studio Installer does not work download the [Visual C++ Redistributable for Visual Studio 2022 for x64](https://download.visualstudio.microsoft.com/download/pr/571ad766-28d1-4028-9063-0fa32401e78f/5D3D8C6779750F92F3726C70E92F0F8BF92D3AE2ABD43BA28C6306466DE8A144/VC_redist.x64.exe). If this link does not work, just search for Visual C++ Redistributable for Visual Studio 2022.


## License

## Contact
- Author: G. Lopez-Candales
- Email: gabriel_lopez-candales@bio-rad.com

## Acknowledgments
- [OxyPlot](https://github.com/oxyplot/oxyplot)
- [Bonsai.Spinnaker](https://github.com/bonsai-rx/spinnaker)
- [iText7](https://github.com/itext/itext7-dotnet)
- [SSH.NET](https://github.com/sshnet/SSH.NET)
- [AForge](https://github.com/cureos/aforge)
