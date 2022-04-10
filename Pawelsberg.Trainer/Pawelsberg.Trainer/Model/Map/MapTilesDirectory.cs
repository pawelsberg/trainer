using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Pawelsberg.Trainer.Model.Map
{
    public class MapTilesDirectory
    {
        public string Folder { get; set; }
        public List<MapTile> Tiles { get; set; }
        public MapTilesDirectory(string folder)
        {
            Folder = folder;
            Tiles = new List<MapTile>();
        }

        public MapTile GetTile(int zoom, int x, int y)
        {
            MapTile retTile = Tiles.FirstOrDefault(mt => mt.Zoom == zoom && mt.X == x && mt.Y == y);
            if (retTile == null)
                retTile = new MapTile(this, zoom, x, y);
            return retTile;
        }
    }
}
