using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using Pawelsberg.Trainer.Model;
using Pawelsberg.Trainer.Model.Map;

namespace Pawelsberg.Trainer
{
    public partial class MainForm : Form
    {
        WorkoutDirectory _workoutDirectory;
        Workout _workout;
        MapChart _mapChart;
        int _zoom;
        public MainForm()
        {
            InitializeComponent();
            _zoom = 15;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _workoutDirectory = new WorkoutDirectory(GetWorkoutDirectory());
            Text = $"Trainer ({_workoutDirectory.Folder})";
            _workoutDirectory.Load();
            //XmlReader reader = XmlReader.Create(@"C:\Users\Pablon\Documents\2013-05-18_rolki3.gpx");
            //XmlReader reader = XmlReader.Create(@"C:\Users\Pablon\Documents\moje\gps\2015-06-13_saunderton.gpx");
            //XmlReader reader = XmlReader.Create(@"C:\Users\Pablon\Documents\moje\gps\2014-08-13 bieg.gpx");
            //XmlReader reader = XmlReader.Create(@"C:\Users\Pablon\Documents\2013_05_27 17_09_endomondo_eksport.gpx");
            LoadWorkoutDirectory();

        }

        private DialogResult ShowSelectWorkoutDirectory()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Please select folder with your GPS workout files (*.gpx, *.tcx)";
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                Properties.Settings.Default.WorkoutDirectory = folderBrowserDialog.SelectedPath;
                Properties.Settings.Default.Save();
                Text = $"Trainer ({folderBrowserDialog.SelectedPath})";
            }
            return result;
        }

        private string GetWorkoutDirectory()
        {
            if (!Directory.Exists(Properties.Settings.Default.WorkoutDirectory))
            {
                DialogResult result = ShowSelectWorkoutDirectory();
                if (result != DialogResult.OK)
                    Application.Exit();
            }
            if (!Directory.Exists(Properties.Settings.Default.WorkoutDirectory))
                Application.Exit();
            return Properties.Settings.Default.WorkoutDirectory;
        }

        private void LoadWorkoutDirectory()
        {
            List<DateTime> dates = new List<DateTime>();
            listView2.Items.Clear();
            foreach (Workout workout in _workoutDirectory.Workouts)
            {
                ListViewItem listViewItem = new ListViewItem(workout.Name);
                listViewItem.Tag = workout;
                listView2.Items.Add(listViewItem);

                
                DateTime start = workout.Points[0].Time;
                DateTime end = workout.Points[workout.Points.Count - 1].Time;

                for (DateTime current = start; current < end; current = current.AddDays(1d))
                    if (!dates.Contains(current.Date))
                        dates.Add(current.Date);

            }

            monthCalendar1.BoldedDates = dates.ToArray();
        }

        private void LoadWorkout(Workout workout)
        {
            _workout = workout;
            _mapChart = new MapChart(_workout.Lat, _workout.LatSize, _workout.Lon, _workout.LonSize, _zoom);
            mapPictureBox.Image = (Image)_mapChart.Image.Clone();
            _workout.Draw(mapPictureBox.Image, _mapChart, Color.Blue);
            propertyGrid1.SelectedObject = _workout;
            webBrowser1.DocumentText = _workout.GetHtmlDocument();
            chart1.Series.Clear();

        }


        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == graphDistanceMenuItem)
            {
                _workout.GausianFilterCalcSpeed();
                chart1.Series.Clear();
                chart1.Series.Add(_workout.Name + " Speed");
                chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;

                if (speedUnitsMenuItem.Text == "[km/h]")
                    foreach (WorkoutPoint point in _workout.Points)
                        chart1.Series[0].Points.Add(new DataPoint(point.Distance, point.CalcSpeed * 3600d / 1000d) { Tag = point });
                else
                    foreach (WorkoutPoint point in _workout.Points)
                        chart1.Series[0].Points.Add(new DataPoint(point.Distance, 1000d / 60d / point.CalcSpeed){Tag = point});

                chart1.Series.Add(_workout.Name + " Alt");
                chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series[1].YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
                foreach (WorkoutPoint point in _workout.Points)
                    chart1.Series[1].Points.AddXY(point.Distance, point.Ele);

                chart1.ChartAreas[0].RecalculateAxesScale();
                if (speedUnitsMenuItem.Text == "[km/h]")
                    SetYAxisSpeedInKmPerHour(chart1.ChartAreas[0].AxisY);
                else
                    SetYAxisSpeedInMinPerKm(chart1.ChartAreas[0].AxisY);

                SetXAxisDistanceInKm( chart1.ChartAreas[0].AxisX);
                SetYAxisAltInM(chart1.ChartAreas[0].AxisY2);
            }
            if (e.ClickedItem == graphDurationMenuItem)
            {
                _workout.GausianFilterCalcSpeed();
                chart1.Series.Clear();
                chart1.Series.Add(_workout.Name + " Speed");
                chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;

                if (speedUnitsMenuItem.Text == "[km/h]")
                    foreach (WorkoutPoint point in _workout.Points)
                        chart1.Series[0].Points[chart1.Series[0].Points.AddXY(new DateTime().Add(point.Time.Subtract(_workout.Points[0].Time)), point.CalcSpeed * 3600d / 1000d)].Tag=point;
                else
                    foreach (WorkoutPoint point in _workout.Points)
                        chart1.Series[0].Points[chart1.Series[0].Points.AddXY(new DateTime().Add(point.Time.Subtract(_workout.Points[0].Time)), 1000d / 60d / point.CalcSpeed)].Tag = point;

                chart1.Series.Add(_workout.Name + " Alt");
                chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Series[1].YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
                foreach (WorkoutPoint point in _workout.Points)
                    chart1.Series[1].Points.AddXY(new DateTime().Add(point.Time.Subtract(_workout.Points[0].Time)), point.Ele);

                chart1.ChartAreas[0].RecalculateAxesScale();
                if (speedUnitsMenuItem.Text == "[km/h]")
                    SetYAxisSpeedInKmPerHour(chart1.ChartAreas[0].AxisY);
                else
                    SetYAxisSpeedInMinPerKm(chart1.ChartAreas[0].AxisY);


                SetXAxisDurationInHms(chart1.ChartAreas[0].AxisX);
                SetYAxisAltInM(chart1.ChartAreas[0].AxisY2);
            }

            if (e.ClickedItem == intervalsMenuItem)
            {
                chart1.Series.Clear();
                chart1.Series.Add(_workout.Name + " Time");
                chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                if (speedUnitsMenuItem.Text == "[km/h]")
                    foreach (WorkoutInterval interval in _workout.KmIntervals)
                        chart1.Series[0].Points.AddXY(interval.Speed.ToString("F2"), interval.Speed);
                else
                    foreach (WorkoutInterval interval in _workout.KmIntervals)
                        chart1.Series[0].Points.AddXY(interval.Duration.ToString(@"mm\:ss"), interval.Duration.TotalMinutes);

                chart1.ChartAreas[0].RecalculateAxesScale();
                if (speedUnitsMenuItem.Text == "[km/h]")
                    SetYAxisSpeedInKmPerHour(chart1.ChartAreas[0].AxisY);
                else
                    SetYAxisSpeedInMinPerKm(chart1.ChartAreas[0].AxisY);

                SetXAxisIntervalInKm(chart1.ChartAreas[0].AxisX);

            }
            if (e.ClickedItem == speedUnitsMenuItem)
            {
                if (speedUnitsMenuItem.Text == "[min/km]")
                    speedUnitsMenuItem.Text = "[km/h]";
                else
                    speedUnitsMenuItem.Text = "[min/km]";
            }
            if (e.ClickedItem == mapZoomMenuItem)
            {
                _zoom++;
                _mapChart = new MapChart(_workout.Lat, _workout.LatSize, _workout.Lon, _workout.LonSize, _zoom);
                mapPictureBox.Image = (Image)_mapChart.Image.Clone();
                _workout.Draw(mapPictureBox.Image, _mapChart, Color.Blue);
            }
            if (e.ClickedItem == mapUnZoomMenuItem)
            {
                _zoom--;
                _mapChart = new MapChart(_workout.Lat, _workout.LatSize, _workout.Lon, _workout.LonSize, _zoom);
                mapPictureBox.Image = (Image)_mapChart.Image.Clone();
                _workout.Draw(mapPictureBox.Image, _mapChart, Color.Blue);
            }
            if (e.ClickedItem == openFolderMenuItem)
            {
                DialogResult result = ShowSelectWorkoutDirectory();
                if (result != DialogResult.OK)
                    Application.Exit();
                if (!Directory.Exists(Properties.Settings.Default.WorkoutDirectory))
                    Application.Exit();
                _workoutDirectory = new WorkoutDirectory(Properties.Settings.Default.WorkoutDirectory);
                _workoutDirectory.Load();
                LoadWorkoutDirectory();
            }
        }

        private void SetXAxisIntervalInKm(Axis axis)
        {

            axis.LabelStyle.Interval = 1d;
            axis.LabelStyle.IntervalType = DateTimeIntervalType.Number;
            axis.LabelStyle.IntervalOffset = 0d;
            axis.LabelStyle.Format = "F0";
            axis.MajorGrid.Interval = 1d;
            axis.MajorGrid.IntervalType = DateTimeIntervalType.Number;
            axis.MajorGrid.IntervalOffset = 0d;
            axis.MajorGrid.IntervalType = DateTimeIntervalType.NotSet;
            axis.MajorTickMark.Interval = 1d;
            axis.MajorTickMark.IntervalType = DateTimeIntervalType.Number;
            axis.MajorTickMark.IntervalOffset = 0d;
            axis.MajorTickMark.IntervalType = DateTimeIntervalType.NotSet;
            axis.Title = "Km interval";
        }

        private void SetXAxisDurationInHms(Axis axis)
        {
            axis.LabelStyle.Interval = 10d;
            axis.LabelStyle.IntervalType = DateTimeIntervalType.Minutes;
            axis.LabelStyle.Format = "HH\\:mm\\:ss";
            axis.MajorGrid.Interval = 10d;
            axis.MajorGrid.IntervalType = DateTimeIntervalType.Minutes;
            axis.MajorTickMark.Interval = 10d;
            axis.MajorTickMark.IntervalType = DateTimeIntervalType.Minutes;
            axis.Title = "Duration [hh:mm:ss]";
        }

        private void SetYAxisAltInM(Axis axis)
        {
            axis.Title = "Alt [m]";
        }
        private void SetYAxisSpeedInKmPerHour(Axis axis)
        {
            axis.Title = "Speed [km/h]";
            axis.LabelStyle.Interval = 5d;
            axis.LabelStyle.IntervalType = DateTimeIntervalType.NotSet;
            axis.LabelStyle.IntervalOffset = 0d;
            axis.LabelStyle.Format = "F0";
            axis.IsStartedFromZero = true;
            axis.MajorGrid.Interval = 5d;
            axis.MajorGrid.IntervalOffset = 0d;
            axis.MajorGrid.IntervalType = DateTimeIntervalType.NotSet;
            axis.MajorTickMark.Interval = 5d;
            axis.MajorTickMark.IntervalOffset = 0d;
            axis.MajorTickMark.IntervalType = DateTimeIntervalType.NotSet;
        }
        private void SetYAxisSpeedInMinPerKm(Axis axis)
        {
            axis.Minimum = 0d;
            axis.Title = "Speed [min/km]";
            axis.LabelStyle.Interval = 1d;
            axis.LabelStyle.IntervalType = DateTimeIntervalType.NotSet;
            axis.LabelStyle.IntervalOffset = 0d;
            axis.LabelStyle.Format = "F0";
            axis.IsStartedFromZero = true;
            axis.MajorGrid.Interval = 1d;
            axis.MajorGrid.IntervalType = DateTimeIntervalType.NotSet;
            axis.MajorGrid.IntervalOffset = 0d;
            axis.MajorTickMark.Interval = 1d;
            axis.MajorTickMark.IntervalType = DateTimeIntervalType.NotSet;
            axis.MajorTickMark.IntervalOffset = 0d;
        }
        private void SetXAxisDistanceInKm(Axis axis)
        {
            axis.LabelStyle.Interval = 1d;
            axis.LabelStyle.IntervalType = DateTimeIntervalType.Number;
            axis.LabelStyle.IntervalOffset = 0d;
            axis.LabelStyle.Format = "F0";
            axis.MajorGrid.Interval = 1d;
            axis.MajorGrid.IntervalOffset = 0d;
            axis.MajorGrid.IntervalType = DateTimeIntervalType.NotSet;
            axis.MajorTickMark.Interval = 1d;
            axis.MajorTickMark.IntervalOffset = 0d;
            axis.MajorTickMark.IntervalType = DateTimeIntervalType.NotSet;
            axis.Title = "Distance [km]";
        }


        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            HitTestResult control = chart1.HitTest(e.X, e.Y);
            if (control.ChartElementType == ChartElementType.DataPoint)
            { }
            if (control.ChartElementType == ChartElementType.DataPoint && chart1.ChartAreas[0].AxisY.Title == "Speed [min/km]"
                && chart1.Series.IndexOf(control.Series) == 0)
            {
                TimeSpan timeSpan = new TimeSpan((long)((double)(control.Object as DataPoint).YValues[0] * 10000000 * 60));
                valueMenuItem.Text = timeSpan.ToString();
            }
            if (control.ChartElementType == ChartElementType.DataPoint && chart1.ChartAreas[0].AxisY.Title == "Speed [km/h]"
                && chart1.Series.IndexOf(control.Series) == 0)
            {
                double speed = ((double)(control.Object as DataPoint).YValues[0]);
                valueMenuItem.Text = speed.ToString("F2");
            }
            if (control.ChartElementType == ChartElementType.DataPoint && chart1.Series.Count == 1)
            {
                mapPictureBox.Image = (Image)_mapChart.Image.Clone();
                _workout.Draw(mapPictureBox.Image, _mapChart, Color.Blue);
                _workout.DrawInterval(mapPictureBox.Image, _mapChart, control.PointIndex, Color.Red);
                mapPictureBox.Refresh();
            }
            if (control.ChartElementType == ChartElementType.DataPoint && chart1.Series.Count == 2)
            {
                mapPictureBox.Image = (Image)_mapChart.Image.Clone();
                _workout.Draw(mapPictureBox.Image, _mapChart, Color.Blue);
                _workout.DrawPoint(mapPictureBox.Image, _mapChart, control.PointIndex, Color.Red);
                foreach (DataPoint dataPoint in chart1.Series[0].Points)
                    dataPoint.Color = Color.Blue;
                (control.Object as DataPoint).Color = Color.Yellow;
                mapPictureBox.Refresh();
            }

        }

        // Drag and drop - move map
        private Point? _mouseDownPoint;
        private void mapPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDownPoint = e.Location;
        }

        private void mapPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDownPoint = null;
        }

        private void mapPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDownPoint.HasValue && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Point movement = Point.Subtract(_mouseDownPoint.Value, new Size(e.Location));
                //Point prevAutoScrollPosition = panel1.AutoScrollPosition;
                panel1.AutoScrollPosition = Point.Add(Point.Subtract(Point.Empty,new Size(panel1.AutoScrollPosition)), new Size(movement));
                //Debugger.Log(0, string.Empty, string.Format("PrevAutoScrollPosition = {2} Movement = {0} AutoScrollPosition = {1}\n", movement,panel1.AutoScrollPosition,prevAutoScrollPosition));
                //panel1.AutoScrollPosition = movement;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.None && false)
            {
                mapPictureBox.Image = (Image)_mapChart.Image.Clone();
                _workout.Draw(mapPictureBox.Image, _mapChart, Color.Blue);
                WorkoutPoint workoutPoint = _workout.GetPointFromDraw(e.Location, _mapChart);
                if (workoutPoint != null)
                {
                    _workout.DrawPoint(mapPictureBox.Image, _mapChart, _workout.Points.IndexOf(workoutPoint), Color.Red);
                    foreach (DataPoint dataPoint in chart1.Series[0].Points)
                        if (dataPoint.Tag == workoutPoint)
                            dataPoint.Color = Color.Yellow;
                        else
                            dataPoint.Color = Color.Blue;
                    mapPictureBox.Refresh();
                }

            }
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.LocalPath == "blank")
                return;
            e.Cancel = true;
            if (e.Url.LocalPath == "showfastest")
            {
                double distance = double.Parse(e.Url.Query.Split('?')[1]);
            foreach (WorkoutInterval workoutInterval in _workout.FastestDistanceIntervals)
            {
                if (Math.Abs(workoutInterval.Distance - distance)<0.0001d)
                {
                    mapPictureBox.Image = (Image)_mapChart.Image.Clone();
                    _workout.Draw(mapPictureBox.Image, _mapChart, Color.Blue);
                    workoutInterval.DrawInterval(mapPictureBox.Image, _mapChart, Color.Orange);
                    mapPictureBox.Refresh();
                    if (chart1.Series.Count > 0)
                        foreach (DataPoint dataPoint in chart1.Series[0].Points)
                            if (workoutInterval.Points.Contains(dataPoint.Tag as WorkoutPoint))
                                dataPoint.Color = Color.Yellow;
                            else
                                dataPoint.Color = Color.Blue;
                }
            }
            }
            if (e.Url.LocalPath == "showlongest")
            {
                double distance = double.Parse(e.Url.Query.Split('?')[1]);
                foreach (WorkoutInterval workoutInterval in _workout.LongestDurationIntervals)
                {
                    if (Math.Abs(workoutInterval.Distance - distance) < 0.0001d)
                    {
                        mapPictureBox.Image = (Image)_mapChart.Image.Clone();
                        _workout.Draw(mapPictureBox.Image, _mapChart, Color.Blue);
                        workoutInterval.DrawInterval(mapPictureBox.Image, _mapChart, Color.Orange);
                        mapPictureBox.Refresh();
                        if (chart1.Series.Count > 0)
                            foreach (DataPoint dataPoint in chart1.Series[0].Points)
                                if (workoutInterval.Points.Contains(dataPoint.Tag as WorkoutPoint))
                                    dataPoint.Color = Color.Yellow;
                                else
                                    dataPoint.Color = Color.Blue;

                    }
                }
            }
        }

        private void listView2_ItemActivate(object sender, EventArgs e)
        {
            ListView listView = (ListView)sender;
            if (listView.SelectedItems.Count == 1)
            {
                Workout workout = (Workout)listView.SelectedItems[0].Tag;
                workout.RecalcPointsDistances();
                workout.RecalcIntervals();
                workout.RecalcBestIntervals();
                LoadWorkout(workout);
            }
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            listView1.Items.Clear();
            foreach (Workout workout in _workoutDirectory.Workouts)
            {
                DateTime selected = e.Start;

                DateTime start = workout.Points[0].Time;
                DateTime end = workout.Points[workout.Points.Count - 1].Time;

                if (start.Date.CompareTo(selected) != 1 && end.Date.AddDays(1d).CompareTo(selected) != -1)
                {
                    ListViewItem listViewItem = new ListViewItem(workout.Name);
                    listViewItem.Tag = workout;
                    listView1.Items.Add(listViewItem);

                }
            }

        }

    }
}
