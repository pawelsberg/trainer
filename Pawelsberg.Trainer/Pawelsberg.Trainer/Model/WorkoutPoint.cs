using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;
using System.Drawing;

namespace Pawelsberg.Trainer.Model
{
    public class WorkoutPoint
    {
        public double Lat { get; set; } // in deg
        public double Long { get; set; } // in deg
        public double Ele { get; set; } // in meters
        public DateTime Time { get; set; }
        public string Desc { get; set; }
        public double Speed
        {
            get { return UseCalcSpeed ? CalcSpeed : GpsSpeed; }
        } // in meters per secound
        public double GpsSpeed { get; set; } // in meters per secound
        public double CalcSpeed { get; set; } // in meters per secound
        public bool UseCalcSpeed { get; set; }
        public double Distance { get; set; } // in km
        public WorkoutPoint(XmlReader reader)
        {
            Lat = double.Parse(reader.GetAttribute("lat"), CultureInfo.GetCultureInfo("en-GB"));
            Long = double.Parse(reader.GetAttribute("lon"), CultureInfo.GetCultureInfo("en-GB"));
            reader.ReadStartElement("trkpt");
            reader.ReadStartElement("ele");
            Ele = double.Parse(reader.ReadContentAsString(), CultureInfo.GetCultureInfo("en-GB"));
            reader.ReadEndElement(); // ele

            reader.ReadStartElement("time");
            Time = DateTime.Parse(reader.ReadContentAsString(), CultureInfo.GetCultureInfo("en-GB"));
            reader.ReadEndElement(); // time


            if (reader.IsStartElement() && reader.Name == "desc")
            {
                reader.ReadStartElement("desc");
                Desc = reader.ReadContentAsString();
                reader.ReadEndElement(); // desc
            }

            if (reader.IsStartElement() && reader.Name == "speed")
            {
                reader.ReadStartElement("speed");
                GpsSpeed = double.Parse(reader.ReadContentAsString(), CultureInfo.GetCultureInfo("en-GB"));
                reader.ReadEndElement(); // speed
            }
            else
                UseCalcSpeed = true;

            if (reader.IsStartElement() && reader.Name == "extensions")
                reader.Skip();
            reader.ReadEndElement(); // trkpt

        }
        public WorkoutPoint(double lat, double lon, double ele, DateTime time)
        {
            Lat = lat;
            Long = lon;
            Ele = ele;
            Time = time;
        }
        public double DistanceTo(WorkoutPoint otherPoint)
        {
            // http://www.movable-type.co.uk/scripts/latlong.html
            double retDistance;
            double R = 6371; // km
            double sinLat = Math.Sin(Lat / 360 * 2 * Math.PI);
            double sinOtherLat = Math.Sin(otherPoint.Lat / 360 * 2 * Math.PI);
            double cosLat = Math.Cos(Lat / 360 * 2 * Math.PI);
            double cosOtherLat = Math.Cos(otherPoint.Lat / 360 * 2 * Math.PI);
            double cosOtherLongMinusLong = Math.Cos(otherPoint.Long / 360 * 2 * Math.PI - Long / 360 * 2 * Math.PI);
            double cos = sinLat * sinOtherLat + cosLat * cosOtherLat * cosOtherLongMinusLong;
            cos = Math.Min(1d, cos);
            cos = Math.Max(-1d, cos);
            retDistance = Math.Acos(cos) * R;

            return retDistance;
        }
        public double SpeedCalculated(WorkoutPoint otherPoint) // in meters per secound
        {
            double horizontalDist = DistanceTo(otherPoint) * 1000d;
            double eleDist = Ele - otherPoint.Ele;
            return Math.Sqrt(eleDist * eleDist + horizontalDist * horizontalDist) / Time.Subtract(otherPoint.Time).TotalSeconds;
        }

        public void DrawPoint(Image image, Map.MapChart mapChart, Color color)
        {
            Graphics graphics = Graphics.FromImage(image);
            PointF point = mapChart.WorldToImagePos(Long, Lat);

            Pen pen = new Pen(color, 3f);
            graphics.DrawEllipse(pen, point.X - 1, point.Y - 1, 2, 2);
        }
    }
}
