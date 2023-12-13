using Independent_Reader_GUI.Models;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Services
{
    internal class ThermocyclingProtocolPlotManager
    {
        private PlotModel plotModel;
        private LineSeries series;
        private List<Tuple<int, int>> sections;
        private List<ThermocyclingProtocolStep> steps = new List<ThermocyclingProtocolStep>();
        private Tuple<int, int> selectedSection = null;
        private int x = 0; // where a new step will be added along the x-axis
        private int dx = 10; // increment between steps on the x-axis
        private int stepCount = 0;
        private double yBufferTimeAnnotation = 10.0;

        /// <summary>
        /// Initialization of the Thermocycling Protocol Plot Manager Object
        /// </summary>
        public ThermocyclingProtocolPlotManager()
        {
            // Initialize the plot model, series for protocol data, and sections of the protocol
            plotModel = new PlotModel { Title = "Thermocycling Protocol" };
            sections = new List<Tuple<int, int>>();
            series = new LineSeries();
            plotModel.Series.Add(series);

            // Initialize the plot axes
            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = double.NaN, // auto-scaling
                Maximum = double.NaN, // auto-scaling
                IsZoomEnabled = false,
                IsPanEnabled = false
            };
            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = double.NaN, // auto-scaling
                Maximum = double.NaN, // auto-scaling
                IsZoomEnabled = false,
                IsPanEnabled = false
            };
            plotModel.Axes.Add(xAxis);
            plotModel.Axes.Add(yAxis);            
        }

        /// <summary>
        /// Calculate the min and max y values including a buffer to take into account annotations
        /// </summary>
        /// <param name="annotationBuffer">Buffer to take into account the text annotations</param>
        /// <returns></returns>
        private Tuple<double, double> CalculateMaxMinYValueIncludingAnnotations(double annotationBuffer = 15.0)
        {
            double maxYValue = 0.0;
            double minYValue = 500.0;
            // Calculate the Max and Min Y Value based on the current highest temperature in the protocol
            foreach (var point in series.Points)
            {
                if (point.Y > maxYValue)
                {
                    maxYValue = point.Y;
                }
                if (point.Y < minYValue)
                {
                    minYValue = point.Y;
                }
            }
            Tuple<double, double> yValueMaxMin = new Tuple<double, double>(maxYValue+annotationBuffer, minYValue-annotationBuffer);
            return yValueMaxMin;
        }

        /// <summary>
        /// Set the Y axis min and max limits
        /// </summary>
        private void SetYAxisLimits()
        {
            var yAxis = plotModel.Axes.FirstOrDefault(a => a.Position == AxisPosition.Left);
            if (yAxis != null)
            {
                Tuple<double, double> maxMinYValue = CalculateMaxMinYValueIncludingAnnotations();
                yAxis.Minimum = maxMinYValue.Item2;
                yAxis.Maximum = maxMinYValue.Item1;
            }
        }

        /// <summary>
        /// Get the plot model necessary for the OxyPlot Plot View widget.
        /// </summary>
        /// <returns></returns>
        public PlotModel GetPlotModel()
        {
            return plotModel;
        }

        /// <summary>
        /// Add a step to the Thermocycling Protocol Plot View
        /// </summary>
        /// <param name="stepTemperature">Temperature to be set at this step</param>
        /// <param name="stepTypeName">Name of the step type</param>
        public void AddStep(double stepTemperature, double stepTime, string stepTypeName)
        {
            // Add this step to the list
            ThermocyclingProtocolStep step = new ThermocyclingProtocolStep(stepTemperature, stepTime, stepTypeName);
            steps.Add(step);
            // Setup the section
            var section = Tuple.Create(x, x + dx);
            sections.Add(section);
            // Setup data for plotting
            series.Points.Add(new DataPoint(section.Item1, stepTemperature));
            series.Points.Add(new DataPoint(section.Item2, stepTemperature));
            // Add annotations for plotting
            var tempAnnotation = new TextAnnotation { TextPosition = new DataPoint(x + dx / 2, stepTemperature), Text = stepTemperature.ToString() + "\u00B0C" };
            var stepAnnotation = new TextAnnotation { TextPosition = new DataPoint(x + dx / 2, stepTemperature), Text = stepTemperature.ToString() + "\u00B0C" };
            var timeAnnotation = new TextAnnotation { TextPosition = new DataPoint(x + dx / 2, stepTemperature-yBufferTimeAnnotation), Text = stepTime.ToString() + " s"};
            plotModel.Annotations.Add(tempAnnotation);
            plotModel.Annotations.Add(stepAnnotation);
            plotModel.Annotations.Add(timeAnnotation);
            var sectionStartLine = new LineAnnotation { X = section.Item1, Type = LineAnnotationType.Vertical, Color = OxyColors.Blue };
            var sectionEndLine = new LineAnnotation { X = section.Item2, Type = LineAnnotationType.Vertical, Color = OxyColors.Blue };
            plotModel.Annotations.Add(sectionStartLine);
            plotModel.Annotations.Add(sectionEndLine);
            // Increase the step count and x
            stepCount++;
            x = x + dx;
            // Change the max and min Y value for the y-axis if necessary
            SetYAxisLimits();
            // Update the plot
            UpdatePlot();
        }

        public int StepCount
        {
            get { return stepCount; }
        }

        public List<ThermocyclingProtocolStep> Steps
        {
            get { return steps; }
        }

        public ThermocyclingProtocolStep GetStep(int stepNumber)
        {
            return steps[stepNumber-1];
        }

        /// <summary>
        /// Mouse down event handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void HandleMouseDown(object sender, OxyMouseDownEventArgs args)
        {
            // Get the x position of the mouse within the plot view
            var xPosition = Axis.InverseTransform(args.Position, plotModel.Axes[0], plotModel.Axes[1]).X;
            // Determine which section was clicked
            selectedSection = sections.FirstOrDefault(s => xPosition >= s.Item1 && xPosition <= s.Item2);
            if (selectedSection != null)
            {
                // Highlight the selected section and update the plot
                HighlightSelectedSection();
                UpdatePlot();
            }
        }

        private void HighlightSelectedSection()
        {           
            var yAxis = plotModel.Axes.FirstOrDefault(a => a.Position == AxisPosition.Left);
            if (yAxis != null)
            {
                List<DataPoint> points = new List<DataPoint>
            {
                new DataPoint(selectedSection.Item1, 0),
                new DataPoint(selectedSection.Item1, yAxis.ActualMaximum),
                new DataPoint(selectedSection.Item2, yAxis.ActualMaximum),
                new DataPoint(selectedSection.Item2, 0)
            };
                List<DataPoint> points2 = new List<DataPoint>
            {
                new DataPoint(selectedSection.Item1, 0),
                new DataPoint(selectedSection.Item2, 0)
            };
                // Clear previous highlights

                if (selectedSection != null)
                {
                    var areaSeries = new AreaSeries { Color = OxyColor.FromAColor(50, OxyColors.Blue) };
                    areaSeries.Points.Clear(); // clear the read-only points property
                    areaSeries.Points2.Clear(); // clear the read-only points2 property
                    areaSeries.Points.AddRange(points);
                    areaSeries.Points2.AddRange(points2);
                    plotModel.Series.Add(areaSeries);
                    UpdatePlot();
                }
            }
        }

        /// <summary>
        /// Update the plot model data
        /// </summary>
        public void UpdatePlot()
        {
            plotModel.InvalidatePlot(true);
        }
    }
}
