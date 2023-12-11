using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class ImagingSetupData
    {
        public string ImageBefore { get; set; } = "Yes";
        public string ImageDuringFOV { get; set; } = "No";
        public string ImageDuringFrequencyFOV { get; set; } = "NA";
        public string ImageDuringAssay { get; set; } = "No";
        public string ImageDuringFrequencyAssay { get; set; } = "NA";
        public string ImageDuringSample { get; set; } = "Yes";
        public string ImageDuringFrequencySample { get; set; } = "1 minute";
        public string ImageAfter { get; set; } = "Yes";
        public string S3BucketPath { get; set; } = "/home/ir/";
        public string LocalPath { get; set; } = "NA";
        public string ImageInCy5 { get; set; } = "Yes";
        public string ImageInFAM { get; set; } = "Yes";
        public string ImageInHEX { get; set; } = "Yes";
        public string ImageInAtto { get; set; } = "No";
        public string ImageInAlexa { get; set; } = "No";
        public string ImageInCy5p5 { get; set; } = "No";
        public string ExposureCy5 { get; set; } = "300000";
        public string ExposureFAM { get; set; } = "600000";
        public string ExposureHEX { get; set; } = "500000";
        public string ExposureAtto { get; set; } = "NA";
        public string ExposureAlexa { get; set; } = "NA";
        public string ExposureCy5p5 { get; set; } = "NA";
    }
}
