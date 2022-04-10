using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Pawelsberg.Trainer.Model.Map
{
    public class MapChart
    {
        private Image _image;
        private MapTilesDirectory _mapTilesDirectory;
        private PointF? _chartStartTilePos;
        private PointF? _chartStopTilePos;
        private double _lat;
        private double _latSize;
        private double _lon;
        private double _lonSize;
        private int _zoom;

        public MapChart(double lat, double latSize, double lon, double lonSize, int zoom)
        {
            _mapTilesDirectory = new MapTilesDirectory(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                , $"{nameof(Pawelsberg)},{nameof(Trainer)}"
                , "MapCache"));
            _lat = lat;
            _latSize = latSize;
            _lon = lon;
            _lonSize = lonSize;
            _zoom = zoom;
        }

        public double Lat { get { return _lat; } }
        public double LatSize { get { return _latSize; } }
        public double Lon { get { return _lon; } }
        public double LonSize { get { return _lonSize; } }
        public int Zoom { get { return _zoom; } }
        public PointF ChartStartTilePos
        {
            get
            {
                if (_chartStartTilePos == null)
                    _chartStartTilePos = WorldToTilePos(Lon, Lat + LatSize, Zoom);
                return _chartStartTilePos.Value;
            }
        }
        public PointF ChartStopTilePos
        {
            get
            {
                if (_chartStopTilePos == null)
                    _chartStopTilePos = WorldToTilePos(Lon + LonSize, Lat, Zoom);
                return _chartStopTilePos.Value;
            }
        }
        public Image Image
        {
            get
            {
                if (_image == null)
                    _image = GetImage();
                return _image;
            }
        }

        public Bitmap GetEmptyBitmap()
        {
            Bitmap bitmap = new Bitmap((int)((ChartStopTilePos.X - ChartStartTilePos.X) * 256 + 1), (int)((ChartStopTilePos.Y - ChartStartTilePos.Y) * 256 + 1));
            return bitmap;
        }
        private Image GetImage()
        {
            Bitmap bitmap = new Bitmap((int)((ChartStopTilePos.X - ChartStartTilePos.X) * 256 + 1), (int)((ChartStopTilePos.Y - ChartStartTilePos.Y) * 256 + 1));
            Graphics graphics = Graphics.FromImage(bitmap);
            for (int x = (int)ChartStartTilePos.X; x <= (int)ChartStopTilePos.X; x++)
                for (int y = (int)ChartStartTilePos.Y; y <= (int)ChartStopTilePos.Y; y++)
                {
                    Image tile = _mapTilesDirectory.GetTile(Zoom, x, y).Image;
                    int tx = -(int)(ChartStartTilePos.X * 256d) + x * 256;
                    int ty = -(int)(ChartStartTilePos.Y * 256d) + y * 256;
                    graphics.DrawImage(tile, tx, ty);
                }
            return bitmap;
        }

        // TODO use double precision float
        public PointF WorldToImagePos(double lon, double lat)
        {
            PointF tilePos = WorldToTilePos(lon, lat, Zoom);
            PointF relativeTilePos = PointF.Subtract(tilePos, new SizeF(ChartStartTilePos));
            PointF imagePos = new PointF(relativeTilePos.X * 256f, relativeTilePos.Y * 256f);
            return imagePos;
        }
        public static PointF WorldToTilePos(double lon, double lat, int zoom)
        {
            PointF p = new Point();
            p.X = (float)((lon + 180.0) / 360.0 * (1 << zoom));
            p.Y = (float)((1.0 - Math.Log(Math.Tan(lat * Math.PI / 180.0) +
                1.0 / Math.Cos(lat * Math.PI / 180.0)) / Math.PI) / 2.0 * (1 << zoom));

            return p;
        }
        public static PointF TileToWorldPos(double tile_x, double tile_y, int zoom)
        {
            PointF p = new Point();
            double n = Math.PI - 2.0 * Math.PI * tile_y / Math.Pow(2.0, zoom);

            p.X = (float)(tile_x / Math.Pow(2.0, zoom) * 360.0 - 180.0);
            p.Y = (float)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)));

            return p;
        }
    }
}
