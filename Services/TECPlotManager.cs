using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

namespace Independent_Reader_GUI.Services
{
    /// <summary>
    /// Handles all TEC Plotting needs
    /// </summary>
    internal class TECPlotManager
    {
        /// <summary>
        /// Create a PlotModel provided data
        /// </summary>
        /// <param name="data">Data to be plotted</param>
        /// <param name="title">Title of the plot</param>
        /// <param name="yAxisTitle">Y axis title</param>
        /// <returns></returns>
        public PlotModel CreatePlotModel(List<Tuple<DateTime, double>> data, string title, string yAxisTitle)
        {
            // Create the PlotModel
            PlotModel plotModel = new PlotModel { Title = title };
            // Configure the X-axis
            plotModel.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Angle = 90,
                StringFormat = "HH:mm:ss",
                Title = "Time",
            });
            // Configure the Y-axis
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = yAxisTitle
            });
            // Add a legend and configure it
            Legend legend = new Legend
            {
                LegendPosition = LegendPosition.TopRight,
                LegendPlacement = LegendPlacement.Outside,
                LegendOrientation = LegendOrientation.Horizontal,
            };
            plotModel.Legends.Add(legend);
            // Add the data to the PlotModel
            LineSeries series = new LineSeries();
            for (int i = 0; i < data.Count; i++)
            {
                double date = DateTimeAxis.ToDouble(data[i].Item1);
                series.Points.Add(new DataPoint(date, data[i].Item2));
            }
            plotModel.Series.Add(series);
            return plotModel;
        }

        /// <summary>
        /// Create a PlotModel provided two sets of data
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <param name="title"></param>
        /// <param name="yAxisTitle"></param>
        /// <returns></returns>
        public PlotModel CreatePlotModel(List<Tuple<DateTime, double>> data1, List<Tuple<DateTime, double>> data2, string title, string yAxisTitle, string data1Title, string data2Title)
        {
            // Create the PlotModel
            PlotModel plotModel = new PlotModel { Title = title };
            // Configure the X-axis
            plotModel.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Angle = 90,
                StringFormat = "HH:mm:ss",
                Title = "Time",
            });
            // Configure the Y-axis
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = yAxisTitle
            });
            // Add a legend and configure it
            Legend legend = new Legend
            {
                LegendPosition = LegendPosition.TopRight,
                LegendPlacement = LegendPlacement.Outside,
                LegendOrientation = LegendOrientation.Horizontal,
            };
            plotModel.Legends.Add(legend);
            // Add the data to the PlotModel
            LineSeries series1 = new LineSeries
            {
                Title = data1Title,
                Color = OxyColor.FromRgb(255, 0, 0),
            };
            LineSeries series2 = new LineSeries
            {
                Title = data2Title,
                Color = OxyColor.FromRgb(0, 0, 255),
            };
            for (int i = 0; i < data1.Count; i++)
            {
                double date1 = DateTimeAxis.ToDouble(data1[i].Item1);
                series1.Points.Add(new DataPoint(date1, data1[i].Item2));
            }
            plotModel.Series.Add(series1);
            for (int i = 0; i < data2.Count; i++)
            {
                double date2 = DateTimeAxis.ToDouble(data2[i].Item1);
                series2.Points.Add(new DataPoint(date2, data2[i].Item2));
            }
            plotModel.Series.Add(series2);
            return plotModel;
        }

        /// <summary>
        /// Export a PlotModel to a PNG with a specified Width and Height
        /// </summary>
        /// <param name="plotModel">PlotModel to be exported</param>
        /// <param name="pngFileName">PNG to be created</param>
        /// <param name="width">Width of the image to be saved</param>
        /// <param name="height">Height of the image to be saved</param>
        public void ExportPlotModelToPNG(PlotModel plotModel, string pngFileName, int width, int height)
        {
            PngExporter pngExporter = new PngExporter
            {
                Width = width,
                Height = height
            };
            pngExporter.ExportToFile(plotModel, pngFileName);
        }
    }
}
