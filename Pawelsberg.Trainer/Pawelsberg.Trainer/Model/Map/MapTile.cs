using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Drawing;

namespace Pawelsberg.Trainer.Model.Map
{
    public class MapTile
    {
        private Image _image;
        private MapTilesDirectory _mapTilesDirectory;
        private string TileLocalPath
        {
            get { return string.Format(@"{0}\{1}.png", TileLocalDirectory, Y); }
        }
        private string TileLocalDirectory
        {
            get { return string.Format(@"{0}\{1}\{2}", _mapTilesDirectory.Folder, Zoom, X); }
        }
        private string TileRemotePath
        {
            get
            {
                return string.Format("https://tile.thunderforest.com/cycle/{0}/{1}/{2}.png?apikey=97d9e9e1b3ba47f9a4cd23a4bb261e71", Zoom, X, Y);
            }
        }

        public int Zoom { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public Image Image
        {
            get
            {
                if (_image != null)
                    return _image;
                if (CacheExist())
                    LoadFromCache();
                else
                    Download();

                return _image;
            }
        }
        public MapTile(MapTilesDirectory mapTilesDirectory, int zoom, int x, int y)
        {
            _mapTilesDirectory = mapTilesDirectory;
            Zoom = zoom;
            X = x;
            Y = y;
        }
        public void Download()
        {
            // prepare the web page we will be asking for
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(TileRemotePath);
            request.UserAgent = "Trainer - Pablons sport trainer application for private use only";
            // execute the request
            HttpWebResponse response = (HttpWebResponse)
            request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {

                // we will read data via the response stream
                Stream resStream = response.GetResponseStream();
                _image = Image.FromStream(resStream);
                SaveToCache();
            }
        }
        public void LoadFromCache()
        {
            _image = Image.FromFile(TileLocalPath);
        }
        public void SaveToCache()
        {
            Directory.CreateDirectory(TileLocalDirectory);
            _image.Save(TileLocalPath);
        }
        public bool CacheExist()
        {
            return File.Exists(TileLocalPath);
        }
    }
}
