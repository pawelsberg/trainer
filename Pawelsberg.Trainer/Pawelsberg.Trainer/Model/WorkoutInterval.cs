using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Pawelsberg.Trainer.Model.Map;

namespace Pawelsberg.Trainer.Model
{
    public class WorkoutInterval
    {
        public List<WorkoutPoint> Points { get; set; }
        public double Distance
        {
            get
            {
                return Points[Points.Count - 1].Distance - Points[0].Distance;
            }
        }
        public TimeSpan Duration
        {
            get
            {
                return Points[Points.Count - 1].Time.Subtract(Points[0].Time);
            }

        }
        public double Speed
        {
            get { return Distance / Duration.TotalHours; }
        } // in km per hour
        private WorkoutInterval()
        {
            Points = new List<WorkoutPoint>();
        }

        public static WorkoutInterval CreateTimeInterval(DateTime start, TimeSpan duration, Workout workout)
        {
            WorkoutInterval retWorkoutInterval = new WorkoutInterval();

            // Clasify existing workout points
            foreach (WorkoutPoint workoutPoint in workout.Points)
                if (workoutPoint.Time.CompareTo(start) >= 0 && workoutPoint.Time.CompareTo(start.Add(duration)) <= 0)
                    retWorkoutInterval.Points.Add(workoutPoint);

            // boundary conditions
            // calculate first point:
            if (retWorkoutInterval.Points.Count > 0 && workout.Points.IndexOf(retWorkoutInterval.Points[0]) > 0 && retWorkoutInterval.Points[0].Time.CompareTo(start) > 0)
            {
                // need to create new begin point - interpolate
                WorkoutPoint prevPoint = workout.Points[workout.Points.IndexOf(retWorkoutInterval.Points[0]) - 1];
                WorkoutPoint nextPoint = retWorkoutInterval.Points[0];
                TimeSpan prevTimeSpan = start.Subtract(prevPoint.Time);
                TimeSpan nextTimeSpan = nextPoint.Time.Subtract(start);
                double lat = prevPoint.Lat + (nextPoint.Lat - prevPoint.Lat) * prevTimeSpan.TotalSeconds / (prevTimeSpan.TotalSeconds + nextTimeSpan.TotalSeconds);
                double lon = prevPoint.Long + (nextPoint.Long - prevPoint.Long) * prevTimeSpan.TotalSeconds / (prevTimeSpan.TotalSeconds + nextTimeSpan.TotalSeconds);
                double ele = prevPoint.Ele + (nextPoint.Ele - prevPoint.Ele) * prevTimeSpan.TotalSeconds / (prevTimeSpan.TotalSeconds + nextTimeSpan.TotalSeconds);
                WorkoutPoint startPoint = new WorkoutPoint(lat, lon, ele, start);
                startPoint.Distance = startPoint.DistanceTo(prevPoint) + prevPoint.Distance;
                startPoint.UseCalcSpeed = true;
                startPoint.CalcSpeed = startPoint.SpeedCalculated(prevPoint);
                retWorkoutInterval.Points.Insert(0, startPoint);
            }

            // calculate last point:
            if (retWorkoutInterval.Points.Count > 0 && workout.Points.IndexOf(retWorkoutInterval.Points[retWorkoutInterval.Points.Count - 1]) < workout.Points.Count - 1 && retWorkoutInterval.Points[retWorkoutInterval.Points.Count - 1].Time.CompareTo(start.Add(duration)) < 0)
            {
                // need to create new end point - interpolate
                WorkoutPoint prevPoint = retWorkoutInterval.Points[retWorkoutInterval.Points.Count - 1];
                WorkoutPoint nextPoint = workout.Points[workout.Points.IndexOf(prevPoint) + 1];
                TimeSpan prevTimeSpan = start.Add(duration).Subtract(prevPoint.Time);
                TimeSpan nextTimeSpan = nextPoint.Time.Subtract(start.Add(duration));
                double lat = prevPoint.Lat + (nextPoint.Lat - prevPoint.Lat) * prevTimeSpan.TotalSeconds / (prevTimeSpan.TotalSeconds + nextTimeSpan.TotalSeconds);
                double lon = prevPoint.Long + (nextPoint.Long - prevPoint.Long) * prevTimeSpan.TotalSeconds / (prevTimeSpan.TotalSeconds + nextTimeSpan.TotalSeconds);
                double ele = prevPoint.Ele + (nextPoint.Ele - prevPoint.Ele) * prevTimeSpan.TotalSeconds / (prevTimeSpan.TotalSeconds + nextTimeSpan.TotalSeconds);
                WorkoutPoint stopPoint = new WorkoutPoint(lat, lon, ele, start.Add(duration));
                stopPoint.Distance = stopPoint.DistanceTo(prevPoint) + prevPoint.Distance;
                stopPoint.UseCalcSpeed = true;
                stopPoint.CalcSpeed = stopPoint.SpeedCalculated(prevPoint);
                retWorkoutInterval.Points.Add(stopPoint);
            }


            return retWorkoutInterval;
        }
        public static WorkoutInterval CreateDistanceInterval(double start, double distance, Workout workout)
        {
            WorkoutInterval retWorkoutInterval = new WorkoutInterval();

            // Clasify existing workout points
            foreach (WorkoutPoint workoutPoint in workout.Points)
                if (workoutPoint.Distance >= start && workoutPoint.Distance <= start + distance)
                    retWorkoutInterval.Points.Add(workoutPoint);

            // boundary conditions
            // calculate first point:
            if (retWorkoutInterval.Points.Count > 0 && workout.Points.IndexOf(retWorkoutInterval.Points[0]) > 0 && retWorkoutInterval.Points[0].Distance > start)
            {
                // need to create new begin point - interpolate
                WorkoutPoint prevPoint = workout.Points[workout.Points.IndexOf(retWorkoutInterval.Points[0]) - 1];
                WorkoutPoint nextPoint = retWorkoutInterval.Points[0];
                double distanceToPrev = start - prevPoint.Distance;
                double distanceToNext = nextPoint.Distance - start;
                DateTime startTime = prevPoint.Time.Add(TimeSpan.FromSeconds(nextPoint.Time.Subtract(prevPoint.Time).TotalSeconds * distanceToPrev / (distanceToPrev + distanceToNext)));
                double lat = prevPoint.Lat + (nextPoint.Lat - prevPoint.Lat) * distanceToPrev / (distanceToPrev + distanceToNext);
                double lon = prevPoint.Long + (nextPoint.Long - prevPoint.Long) * distanceToPrev / (distanceToPrev + distanceToNext);
                double ele = prevPoint.Ele + (nextPoint.Ele - prevPoint.Ele) * distanceToPrev / (distanceToPrev + distanceToNext);
                WorkoutPoint startPoint = new WorkoutPoint(lat, lon, ele, startTime);
                startPoint.Distance = startPoint.DistanceTo(prevPoint) + prevPoint.Distance;
                startPoint.UseCalcSpeed = true;
                startPoint.CalcSpeed = startPoint.SpeedCalculated(prevPoint);
                retWorkoutInterval.Points.Insert(0, startPoint);
            }

            // calculate last point:
            if (retWorkoutInterval.Points.Count > 0 && workout.Points.IndexOf(retWorkoutInterval.Points[retWorkoutInterval.Points.Count - 1]) < workout.Points.Count - 1 && retWorkoutInterval.Points[retWorkoutInterval.Points.Count - 1].Distance < start + distance)
            {
                // need to create new end point - interpolate
                WorkoutPoint prevPoint = retWorkoutInterval.Points[retWorkoutInterval.Points.Count - 1];
                WorkoutPoint nextPoint = workout.Points[workout.Points.IndexOf(prevPoint) + 1];
                double distanceToPrev = start + distance - prevPoint.Distance;
                double distanceToNext = nextPoint.Distance - start - distance;
                DateTime startTime = prevPoint.Time.Add(TimeSpan.FromSeconds(nextPoint.Time.Subtract(prevPoint.Time).TotalSeconds * distanceToPrev / (distanceToPrev + distanceToNext)));
                double lat = prevPoint.Lat + (nextPoint.Lat - prevPoint.Lat) * distanceToPrev / (distanceToPrev + distanceToNext);
                double lon = prevPoint.Long + (nextPoint.Long - prevPoint.Long) * distanceToPrev / (distanceToPrev + distanceToNext);
                double ele = prevPoint.Ele + (nextPoint.Ele - prevPoint.Ele) * distanceToPrev / (distanceToPrev + distanceToNext);
                WorkoutPoint stopPoint = new WorkoutPoint(lat, lon, ele, startTime);
                stopPoint.Distance = stopPoint.DistanceTo(prevPoint) + prevPoint.Distance;
                stopPoint.UseCalcSpeed = true;
                stopPoint.CalcSpeed = stopPoint.SpeedCalculated(prevPoint);
                retWorkoutInterval.Points.Add(stopPoint);
            }


            return retWorkoutInterval;
        }
        public static WorkoutInterval CreateFastestDistanceInterval(double distance, Workout workout)
        {
            WorkoutInterval fastestInterval = null;
            TimeSpan smallestDuration = TimeSpan.MaxValue;
            foreach (WorkoutPoint point in workout.Points)
            {
                WorkoutInterval tempInterval = CreateDistanceInterval(point.Distance, distance, workout);
                if (Math.Abs(tempInterval.Distance - distance) < 0.0001d && tempInterval.Duration.CompareTo(smallestDuration) < 0)
                {
                    fastestInterval = tempInterval;
                    smallestDuration = tempInterval.Duration;
                }
                tempInterval = CreateDistanceInterval(point.Distance - distance, distance, workout);
                if (Math.Abs(tempInterval.Distance - distance) < 0.0001d && tempInterval.Duration.CompareTo(smallestDuration) < 0)
                {
                    fastestInterval = tempInterval;
                    smallestDuration = tempInterval.Duration;
                }
            }
            return fastestInterval;
        }
        public static WorkoutInterval CreateLongestTimeInterval(TimeSpan duration, Workout workout)
        {
            WorkoutInterval longestInterval = null;
            double longestDistance = 0;
            foreach (WorkoutPoint point in workout.Points)
            {
                WorkoutInterval tempInterval = CreateTimeInterval(point.Time, duration, workout);
                if (Math.Abs(tempInterval.Duration.TotalSeconds - duration.TotalSeconds) < 0.01d && tempInterval.Distance > longestDistance)
                {
                    longestInterval = tempInterval;
                    longestDistance = tempInterval.Distance;
                }
                tempInterval = CreateTimeInterval(point.Time.Subtract(duration), duration, workout);
                if (Math.Abs(tempInterval.Duration.TotalSeconds - duration.TotalSeconds) < 0.01d && tempInterval.Distance > longestDistance)
                {
                    longestInterval = tempInterval;
                    longestDistance = tempInterval.Distance;
                }
            }
            return longestInterval;
        }

        public void DrawInterval(Image image, MapChart mapChart, Color color)
        {
            Graphics graphics = Graphics.FromImage(image);
            PointF[] points = new PointF[Points.Count];

            for (int i = 0; i < Points.Count; i++)
                points[i] = mapChart.WorldToImagePos(Points[i].Long, Points[i].Lat);

            Pen pen = new Pen(color, 3f);
            graphics.DrawLines(pen, points);
        }

    }
}
