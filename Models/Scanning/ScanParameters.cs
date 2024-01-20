using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Independent_Reader_GUI.Models.Consumables;

namespace Independent_Reader_GUI.Models.Scanning
{
    internal class ScanParameters
    {
        public string ExperimentName;
        public string HeaterLetter;
        public int x0;
        public int y0;
        public int z0;
        public int FOVdX;
        public int SampledX;
        public int dY;
        public double RotationalOffset;
        public bool ImageBrightField;
        public int BrightFieldIntensity;
        public int BrightFieldExposure;
        public bool ImageCy5;
        public int Cy5Intensity;
        public int Cy5Exposure;
        public bool ImageFAM;
        public int FAMIntensity;
        public int FAMExposure;
        public bool ImageHEX;
        public int HEXIntensity;
        public int HEXExposure;
        public bool ImageAtto;
        public int AttoIntensity;
        public int AttoExposure;
        public bool ImageAlexa;
        public int AlexaIntensity;
        public int AlexaExposure;
        public bool ImageCy5p5;
        public int Cy5p5Intensity;
        public int Cy5p5Exposure;
        public Samples SampleNames = new Samples();
        public Assays AssayNames = new Assays();

        public ScanParameters(string experimentName,
            string heaterLetter,
            int x0, int y0, int z0, int fOVdX, int sampledX, int dY, double rotationalOffset,
            bool imageBrightField, int brightFieldIntensity, int brightFieldExposure,
            bool imageCy5, int cy5Intensity, int cy5Exposure,
            bool imageFAM, int famIntensity, int famExposure,
            bool imageHEX, int hexIntensity, int hexExposure,
            bool imageAtto, int attoIntensity, int attoExposure,
            bool imageAlexa, int alexaIntensity, int alexaExposure,
            bool imageCy5p5, int cy5p5Intensity, int cy5p5Exposure,
            Samples sampleNames, Assays assayNames)
        {
            ExperimentName = experimentName;
            HeaterLetter = heaterLetter;
            this.x0 = x0; this.y0 = y0; this.z0 = z0;
            FOVdX = fOVdX; SampledX = sampledX; this.dY = dY; RotationalOffset = rotationalOffset;
            ImageBrightField = imageBrightField; BrightFieldIntensity = brightFieldIntensity; BrightFieldExposure = brightFieldExposure;
            ImageCy5 = imageCy5; Cy5Intensity = cy5Intensity; Cy5Exposure = cy5Exposure;
            ImageFAM = imageFAM; FAMIntensity = famIntensity; FAMExposure = famExposure;
            ImageHEX = imageHEX; HEXIntensity = hexIntensity; HEXExposure = hexExposure;
            ImageAtto = imageAtto; AttoIntensity = attoIntensity; AttoExposure = attoExposure;
            ImageAlexa = imageAlexa; AlexaIntensity = alexaIntensity; AlexaExposure = alexaExposure;
            ImageCy5p5 = imageCy5p5; Cy5p5Intensity = cy5p5Intensity; Cy5p5Exposure = cy5p5Exposure;
            SampleNames = sampleNames;
            AssayNames = assayNames;
        }
    }
}
