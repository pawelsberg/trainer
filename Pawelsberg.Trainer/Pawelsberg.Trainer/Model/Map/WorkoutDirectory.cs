using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace Pawelsberg.Trainer.Model.Map
{
    public class WorkoutDirectory
    {
        public string Folder { get; set; }
        public List<Workout> Workouts { get; set; }
        public WorkoutDirectory(string folder)
        {
            Folder = folder;
            Workouts = new List<Workout>();
        }
        public void Load()
        {
            foreach (string gpxFileName in Directory.EnumerateFiles(Folder, "*.gpx"))
            {
                XmlReader reader = XmlReader.Create(gpxFileName);

                Workout workout = Workout.CreateFromGpx(reader);
                reader.Close();
                Workouts.Add(workout);
            }

            foreach (string tcxFileName in Directory.EnumerateFiles(Folder, "*.tcx"))
            {
                XmlReader reader = XmlReader.Create(tcxFileName);

                Workout workout = Workout.CreateFromTcx(reader);
                reader.Close();
                Workouts.Add(workout);
            }

        }


    }
}
