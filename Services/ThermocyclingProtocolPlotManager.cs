using Independent_Reader_GUI.Models;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

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
        private double yBufferStepAnnotation = 10.0;
        private int nAnnotationsPerStep = 5;
        private int nSeriesPointsPerStep = 2;

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

        private void Reset()
        {
            x = 0; // Reset where the sections start
            stepCount = 0; // Reset the number of steps
            sections = new List<Tuple<int, int>>(); // Reinitalize the sections
            series = new LineSeries(); // Reinitialize the series
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

        public void ClearPlot()
        {
            // Clear out the series, annotations, and axes
            plotModel.Series.Clear();
            plotModel.Annotations.Clear();
            plotModel.Axes.Clear();
            plotModel.ResetAllAxes();
            UpdatePlot();
            Reset();
        }

        /// <summary>
        /// Add a protocol step to the plot
        /// </summary>
        /// <param name="step">ThermocyclingProtocolStep instance to be added to the plot</param>
        public void AddStep(ThermocyclingProtocolStep step)
        {
            // Add the step to the protocol steps
            steps.Add(step);
            // Setup the section
            var section = Tuple.Create(x, x + dx);
            sections.Add(section);
            if (step.TypeName.Equals(ThermocyclingProtocolStepType.Set) || step.TypeName.Equals(ThermocyclingProtocolStepType.Hold))
            {
                // Setup data for plotting
                series.Points.Add(new DataPoint(section.Item1, step.Temperature));
                series.Points.Add(new DataPoint(section.Item2, step.Temperature));
                // Add annotations for plotting
                var tempAnnotation = new TextAnnotation { TextPosition = new DataPoint(x + dx / 2, step.Temperature), Text = step.Temperature.ToString() + "\u00B0" + step.TemperatureUnits[0].ToString() };
                var stepAnnotation = new TextAnnotation {
                    TextPosition = new DataPoint(x + dx / 2, step.Temperature + yBufferStepAnnotation),
                    Text = "Step: " + (step.Index + 1).ToString(),
                    TextColor = OxyColors.IndianRed,
                    FontWeight = FontWeights.Bold,
                    StrokeThickness = 0 // remove the border around the annotation
                };
                plotModel.Annotations.Add(tempAnnotation);
                plotModel.Annotations.Add(stepAnnotation);
                if (step.TypeName.Equals(ThermocyclingProtocolStepType.Set))
                {
                    var timeAnnotation = new TextAnnotation { TextPosition = new DataPoint(x + dx / 2, step.Temperature - yBufferTimeAnnotation), Text = step.Time.ToString() + " " + step.TimeUnits[0].ToString() };
                    plotModel.Annotations.Add(timeAnnotation);
                }
                else
                {
                    var timeAnnotation = new TextAnnotation { TextPosition = new DataPoint(x + dx / 2, step.Temperature - yBufferTimeAnnotation), Text = "\u221E" };
                    plotModel.Annotations.Add(timeAnnotation);
                }                
            }
            else if (step.TypeName.Equals(ThermocyclingProtocolStepType.GoTo))
            {
                // Setup data for plotting
                series.Points.Add(new DataPoint(section.Item1, 0));
                series.Points.Add(new DataPoint(section.Item2, 0));
                // Add annotations for plotting
                var stepNumberAnnotation = new TextAnnotation { TextPosition = new DataPoint(x + dx / 2, 0), Text = "GoTo Step " + (step.StepNumber+1).ToString() };
                var stepAnnotation = new TextAnnotation
                {
                    TextPosition = new DataPoint(x + dx / 2, 0 + yBufferStepAnnotation),
                    Text = "Step: " + (step.Index + 1).ToString(),
                    TextColor = OxyColors.IndianRed,
                    FontWeight = FontWeights.Bold,
                    StrokeThickness = 0
                };
                plotModel.Annotations.Add(stepNumberAnnotation);
                plotModel.Annotations.Add(stepAnnotation);
                var cycleCountAnnotation = new TextAnnotation { TextPosition = new DataPoint(x + dx / 2, 0 - yBufferTimeAnnotation), Text = "Cycles: " + step.CycleCount.ToString() };
                plotModel.Annotations.Add(cycleCountAnnotation);
            }
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

        public void PlotProtocol(ThermocyclingProtocol protocol)
        {
            if (protocol != null)
            {
                foreach (ThermocyclingProtocolStep step in protocol.Steps)
                {
                    this.AddStep(step);
                }
            }
            UpdatePlot();
        }

        private Tuple<Annotation, Annotation, Annotation, Annotation, Annotation> GetStepAnnotations(int stepIndex)
        {
            int annotationIndex = -1;
            for (int i = 0; i < plotModel.Annotations.Count; i = i + nAnnotationsPerStep)
            {
                if (i == stepIndex - 1)
                {
                    annotationIndex = i;
                    break;
                }
            }
            var tempAnnotation = plotModel.Annotations[annotationIndex];
            var stepAnnotation = plotModel.Annotations[annotationIndex + 1];
            var timeAnnotation = plotModel.Annotations[annotationIndex + 2];
            var sectionStartLine = plotModel.Annotations[annotationIndex + 3];
            var sectionEndLine = plotModel.Annotations[annotationIndex + 4];
            var annotations = Tuple.Create(tempAnnotation, stepAnnotation, timeAnnotation, sectionStartLine, sectionEndLine);
            return annotations;
        }

        public int GetStepAnnotationsStartIndex(int stepIndex)
        {
            int annotationIndex = nAnnotationsPerStep * (stepIndex);
            return annotationIndex;
        }

        public int GetSeriesStartIndex(int stepIndex)
        {
            int seriesIndex = nSeriesPointsPerStep * (stepIndex);
            return seriesIndex;
        }

        public void EditStep(int stepIndex, double stepTemperature, double stepTime, string stepTimeUnits)
        {
            // TODO: This method edits the wrong step (minus 1 for step index) and does not change the text annotations or time
            // Modify the steps list 
            ThermocyclingProtocolStep step = steps[stepIndex];
            string stepTypeName = step.TypeName;
            ThermocyclingProtocolSetStep modifiedStep = new ThermocyclingProtocolSetStep(stepTemperature, stepTime);
            steps[stepIndex] = modifiedStep;
            // Modifiy the plot (tempAnnotation, timeAnnotation, and series)
            int stepAnnotationsStartIndex = GetStepAnnotationsStartIndex(stepIndex);
            if (plotModel.Annotations[stepAnnotationsStartIndex] is TextAnnotation tempAnnotation)
            {
                tempAnnotation.Text = stepTemperature.ToString() + "\u00B0C";
                tempAnnotation.TextPosition = new DataPoint(dx * stepIndex + dx / 2, stepTemperature);
            }
            if (plotModel.Annotations[stepAnnotationsStartIndex+2] is TextAnnotation timeAnnotation)
            {
                timeAnnotation.Text = stepTime.ToString() + $" {stepTimeUnits[0].ToString().ToLower()}";
                timeAnnotation.TextPosition = new DataPoint(dx * stepIndex + dx / 2, stepTemperature-yBufferTimeAnnotation);
            }
            int seriesStartIndex = GetSeriesStartIndex(stepIndex);
            var xStart = series.Points[seriesStartIndex].X;
            var xEnd = series.Points[seriesStartIndex + 1].X;
            series.Points[seriesStartIndex] = new DataPoint(xStart, stepTemperature);
            series.Points[seriesStartIndex + 1] = new DataPoint(xEnd, stepTemperature);
            // Update the plot and axes
            SetYAxisLimits();
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
            var yPosition = Axis.InverseTransform(args.Position, plotModel.Axes[0], plotModel.Axes[1]).Y;
            if (xPosition > sections.Last().Item1 + dx)
            {
                return;
            }
            if (xPosition < 0)
            {
                return;
            }
            if (!(yPosition >= -10 && yPosition <= 108))
            {
                return;
            }
            // Determine which section was clicked
            selectedSection = sections.FirstOrDefault(s => xPosition >= s.Item1 && xPosition <= s.Item2);
            if (selectedSection != null)
            {
                // Highlight the selected section and update the plot
                HighlightSelectedSection();
                // FIXME: get this section of code to work without the message box
                MessageBox.Show("GUI crashes without this popup");
                //UpdatePlot();
            }
        }

        private void HighlightSelectedSection()
        {
            var yAxis = plotModel.Axes.FirstOrDefault(a => a.Position == AxisPosition.Left);
            if (yAxis != null)
            {
                List<DataPoint> points = new List<DataPoint>
                {
                    new DataPoint(selectedSection.Item1, -15),
                    new DataPoint(selectedSection.Item1, yAxis.ActualMaximum),
                    new DataPoint(selectedSection.Item2, yAxis.ActualMaximum),
                    new DataPoint(selectedSection.Item2, -15)
                };
                List<DataPoint> points2 = new List<DataPoint>
                {
                    new DataPoint(selectedSection.Item1, -15),
                    new DataPoint(selectedSection.Item2, -15)
                };

                if (selectedSection != null)
                {
                    var areaSeries = new AreaSeries { Color = OxyColor.FromAColor(50, OxyColors.Blue) };
                    // Look to clear old highlighted sections
                    AreaSeries? seriesToRemove = null;
                    foreach (var series in plotModel.Series)
                    {
                        if (series.GetType() == typeof(AreaSeries))
                        {
                            seriesToRemove = series as AreaSeries;
                        }
                    }
                    if (seriesToRemove != null)
                    {
                        plotModel.Series.Remove(seriesToRemove);
                    }
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
