using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class ImagingSetupData
    {
        private object[] useOptions = new object[] { "Yes", "No" };
        public int X0 { get; set; } = 0;
        public int Y0 { get; set; } = 0;
        public int Z0 { get; set; } = 0;
        public int FOVdX { get; set; } = 0;
        public int dY { get; set; } = 0;
        public int RotationalOffset { get; set; } = 0;
        public DataGridViewComboBoxCell UseAutofocus = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell ImageBefore = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell ImageDuringFOV = new DataGridViewComboBoxCell();
        public string ImageDuringFrequencyFOV = "NA";
        public DataGridViewComboBoxCell ImageDuringAssay = new DataGridViewComboBoxCell();
        public string ImageDuringFrequencyAssay = "NA";
        public DataGridViewComboBoxCell ImageDuringSample = new DataGridViewComboBoxCell();
        public string ImageDuringFrequencySample { get; set; } = "1 minute";
        public DataGridViewComboBoxCell ImageAfter = new DataGridViewComboBoxCell();
        public string S3BucketPath { get; set; } = "/home/ir/";
        public string LocalPath { get; set; } = "NA";
        public DataGridViewComboBoxCell ImageInCy5 = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell ImageInFAM = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell ImageInHEX = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell ImageInAtto = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell ImageInAlexa = new DataGridViewComboBoxCell();
        public DataGridViewComboBoxCell ImageInCy5p5 = new DataGridViewComboBoxCell();
        public string ExposureCy5 { get; set; } = "300000";
        public string ExposureFAM { get; set; } = "600000";
        public string ExposureHEX { get; set; } = "500000";
        public string ExposureAtto { get; set; } = "NA";
        public string ExposureAlexa { get; set; } = "NA";
        public string ExposureCy5p5 { get; set; } = "NA";
        public int IntensityCy5 { get; set; } = 20;
        public int IntensityFAM { get; set; } = 20;
        public int IntensityHEX { get; set; } = 20;
        public int IntensityAtto { get; set; } = 20;
        public int IntensityAlexa { get; set; } = 20;
        public int IntensityCy5p5 { get; set; } = 20;

        public ImagingSetupData()
        {
            UseAutofocus.Items.AddRange(useOptions);
            UseAutofocus.Value = UseAutofocus.Items[0];
            ImageBefore.Items.AddRange(useOptions);
            ImageBefore.Value = ImageBefore.Items[0];
            ImageDuringFOV.Items.AddRange(useOptions);
            ImageDuringFOV.Value = ImageDuringFOV.Items[1];
            ImageDuringSample.Items.AddRange(useOptions);
            ImageDuringSample.Value = ImageDuringSample.Items[0];
            ImageDuringAssay.Items.AddRange(useOptions);
            ImageDuringAssay.Value = ImageDuringAssay.Items[1];
            ImageAfter.Items.AddRange(useOptions);
            ImageAfter.Value = ImageAfter.Items[0];
            ImageInCy5.Items.AddRange(useOptions);
            ImageInCy5.Value = ImageInCy5.Items[1];
            ImageInFAM.Items.AddRange(useOptions);
            ImageInFAM.Value = ImageInFAM.Items[0];
            ImageInHEX.Items.AddRange(useOptions);
            ImageInHEX.Value = ImageInHEX.Items[0];
            ImageInAtto.Items.AddRange(useOptions);
            ImageInAtto.Value = ImageInAtto.Items[0];
            ImageInAlexa.Items.AddRange(useOptions);
            ImageInAlexa.Value = ImageInAlexa.Items[0];
            ImageInCy5p5.Items.AddRange(useOptions);
            ImageInCy5p5.Value = ImageInCy5p5.Items[0];
        }
    }
}
