using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using System.ComponentModel;
using Pawelsberg.Trainer.Model;

namespace Pawelsberg.Trainer.Model.Map
{
    public class Workout
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Source { get; set; }
        public Uri Link { get; set; }
        public string Type { get; set; }
        public List<WorkoutPoint> Points { get; set; }
        public List<WorkoutInterval> KmIntervals { get; set; }
        public List<WorkoutInterval> FastestDistanceIntervals { get; set; }
        public List<WorkoutInterval> LongestDurationIntervals { get; set; }
        [Browsable(false)]
        public double Lat
        {
            get { return Points.OrderBy(pt => pt.Lat).First().Lat; }
        }
        [Browsable(false)]
        public double LatSize
        {
            get { return Points.OrderByDescending(pt => pt.Lat).First().Lat - Lat; }
        }
        [Browsable(false)]
        public double Lon
        {
            get { return Points.OrderBy(pt => pt.Long).First().Long; }
        }
        [Browsable(false)]
        public double LonSize
        {
            get { return Points.OrderByDescending(pt => pt.Long).First().Long - Lon; }
        }

        public TimeSpan Duration { get { return Points[Points.Count - 1].Time.Subtract(Points[0].Time); } }
        public double Distance { get { return Points[Points.Count - 1].Distance - Points[0].Distance; } }
        public double Speed
        {
            get { return Distance / Duration.TotalHours; }
        } // in km per hour

        public Workout(XmlReader reader)
        {
            reader.ReadStartElement("gpx");
            if (reader.IsStartElement() && reader.Name == "metadata")
                reader.Skip();
            reader.ReadStartElement("trk");
            reader.ReadStartElement("name");
            Name = reader.ReadContentAsString();
            reader.ReadEndElement(); // name
            if (reader.IsStartElement() && reader.Name == "cmt")
            {
                reader.ReadStartElement("cmt");
                Comment = reader.ReadContentAsString();
                reader.ReadEndElement(); // cmt
            }
            if (reader.IsStartElement() && reader.Name == "src")
            {
                reader.ReadStartElement("src");
                Source = reader.ReadContentAsString();
                reader.ReadEndElement(); // src
            }
            if (reader.IsStartElement() && reader.Name == "link")
            {
                Link = new Uri(reader.GetAttribute("href"));
                reader.Skip(); // link
            }
            if (reader.IsStartElement() && reader.Name == "type")
            {
                reader.ReadStartElement("type");
                Type = reader.ReadContentAsString();
                reader.ReadEndElement(); // type
            }

            reader.ReadStartElement("trkseg");


            Points = new List<WorkoutPoint>();
            while (reader.IsStartElement("trkpt"))
            {
                WorkoutPoint workoutPoint = new WorkoutPoint(reader);
                if (Points.Any())
                    workoutPoint.CalcSpeed = workoutPoint.SpeedCalculated(Points[Points.Count - 1]);
                Points.Add(workoutPoint);
            }
            reader.ReadEndElement(); // trkseg
            reader.ReadEndElement(); // trk
            reader.ReadEndElement(); // gpx

            KmIntervals = new List<WorkoutInterval>();
            FastestDistanceIntervals = new List<WorkoutInterval>();
            LongestDurationIntervals = new List<WorkoutInterval>();
        }
        public void RecalcPointsDistances()
        {
            double distance = 0d;
            for (int i = 1; i < Points.Count; i++)
            {
                distance += Points[i].DistanceTo(Points[i - 1]);
                Points[i].Distance = distance;
            }
        }

        public void GausianFilterCalcSpeed()
        {
            Dictionary<int, double> gausianFilter = new Dictionary<int, double>
            {
                {-5,0.01d},
                {-4,0.02d},
                {-3,0.04d},
                {-2,0.10d},
                {-1,0.20d},
                { 0,0.30d},
                { 1,0.20d},
                { 2,0.10d},
                { 3,0.04d},
                { 4,0.02d},
                { 5,0.01d}
            };

            List<double> filteredSpeeds = new List<double>();
            foreach (WorkoutPoint workoutPoint in Points)
            {
                int wpIndex = Points.IndexOf(workoutPoint);
                double multiplerSum = 0d;
                double resultSpeed = 0d;
                foreach (KeyValuePair<int, double> filterItem in gausianFilter)
                {
                    int fiIndex = wpIndex + filterItem.Key;
                    if (fiIndex >= 0 && fiIndex < Points.Count)
                    {
                        multiplerSum += filterItem.Value;
                        resultSpeed += Points[fiIndex].CalcSpeed * filterItem.Value;
                    }
                }
                filteredSpeeds.Add(resultSpeed / multiplerSum);
            }

            foreach (WorkoutPoint workoutPoint in Points)
            {
                int wpIndex = Points.IndexOf(workoutPoint);
                workoutPoint.CalcSpeed = filteredSpeeds[wpIndex];
            }
        }

        public void RecalcIntervals()
        {
            for (double km = 0d; km < Distance; km += 1d)
                KmIntervals.Add(WorkoutInterval.CreateDistanceInterval(km, 1d, this));
        }
        public void RecalcBestIntervals()
        {
            FastestDistanceIntervals = new List<WorkoutInterval>();
            List<double> distances = new List<double>() { 1d, 2d, 5d, 10d, 21.0975d, 30d, 42.195d };
            foreach (double distance in distances)
            {
                WorkoutInterval workoutInterval = WorkoutInterval.CreateFastestDistanceInterval(distance, this);
                if (workoutInterval != null)
                    FastestDistanceIntervals.Add(workoutInterval);
            }

            LongestDurationIntervals = new List<WorkoutInterval>();
            List<TimeSpan> durations = new List<TimeSpan>() { TimeSpan.FromMinutes(12d), TimeSpan.FromHours(1d) };
            foreach (TimeSpan duration in durations)
            {
                WorkoutInterval workoutInterval = WorkoutInterval.CreateLongestTimeInterval(duration, this);
                if (workoutInterval != null)
                    LongestDurationIntervals.Add(workoutInterval);
            }



        }
        public void Draw(Image image, MapChart mapChart, Color color)
        {
            Graphics graphics = Graphics.FromImage(image);
            PointF[] points = new PointF[Points.Count];
            for (int i = 0; i < Points.Count; i++)
                points[i] = mapChart.WorldToImagePos(Points[i].Long, Points[i].Lat);

            Pen pen = new Pen(color, 3f);
            graphics.DrawLines(pen, points);
        }
        public void DrawInterval(Image image, MapChart mapChart, int kmInterval, Color color)
        {
            KmIntervals[kmInterval].DrawInterval(image, mapChart, color);
        }

        public string GetHtmlDocument()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table>");
            foreach (WorkoutInterval workoutInterval in FastestDistanceIntervals)
            {
                sb.Append("<tr>");
                sb.AppendFormat("<td><a href=\"showfastest?{0}\">Fastest {0:F4}km</a></td><td>{1:hh\\:mm\\:ss}</td><td>{2:F2}km/h</td>", workoutInterval.Distance, workoutInterval.Duration, workoutInterval.Speed);
                sb.Append("</tr>");
            }
            foreach (WorkoutInterval workoutInterval in LongestDurationIntervals)
            {
                sb.Append("<tr>");
                sb.AppendFormat("<td><a href=\"showlongest?{1}\">Gratest distance in {0:hh\\:mm\\:ss}</a></td><td>{1:F4}km</td><td>{2:F2}km/h</td>", workoutInterval.Duration, workoutInterval.Distance, workoutInterval.Speed);
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }

        internal void DrawPoint(Image image, MapChart mapChart, int p, Color color)
        {
            Points[p].DrawPoint(image, mapChart, color);
        }

        internal WorkoutPoint GetPointFromDraw(Point point, MapChart mapChart)
        {
            using (Bitmap emptyBitmap = mapChart.GetEmptyBitmap())
            {
                for (int color = 1; color <= Points.Count; color++)
                    Points[color - 1].DrawPoint(emptyBitmap, mapChart, Color.FromArgb(255, Color.FromArgb(color)));

                Color measuredColor = emptyBitmap.GetPixel(point.X, point.Y);
                int measuredIndex = (measuredColor.ToArgb() & 0x0FFFFFF) - 1;
                if (measuredIndex >= 0)
                    return Points[measuredIndex];
                else
                    return null;
            }
        }
    }
}
