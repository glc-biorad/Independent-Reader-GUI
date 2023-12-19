# Independent Reader GUI

## Overview
Independent Reader GUI is a tool for utilizing the full functionality of the Independent Reader Module. It allows user to view all running submodule metrological data and connective status, control submodules, create/edit dPCR thermocycling protocols on 4 individual thermocycling heaters, run protocols on these heaters, image dPCR cartridges, perform full workflow runs (image, thermocycle, image, etc.), and set default settings/configurations for the instrument.

## Technology Used
- C#
- .NET Framework 6.0
- Window Forms
- OxyPlot.WindowsForms 2.1.2 for Charting
- Bonsai.Spinnaker 0.7.1
- Desktop Developer for C++ on Visual Studio
- iText7 8.0.2
- BouncyCastle.NetCore 2.2.1

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
