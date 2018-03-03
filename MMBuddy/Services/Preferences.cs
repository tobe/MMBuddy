using MMBuddy.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace MMBuddy.Services
{
    /// <summary>
    /// Saves and restores runes (preferences) from a JSON file
    /// </summary>
    public class Preferences
    {
        private string _path;

        private List<RunePage> _data;
        public List<RunePage> Data
        {
            get
            {
                return this._data;
            }
            private set { }
        }


        /// <summary>
        /// Loads the preferences from a file if it exists, if it does not, creates a blank JSON file.
        /// </summary>
        public Preferences()
        {
            try
            {
                this._path = Directory.GetCurrentDirectory();
            }
            catch (UnauthorizedAccessException)
            {
                this._path = Path.GetTempPath();
            }

            this._path += "\\MMBuddy.json";

            if(!File.Exists(this._path))
            {
                // Create a blank file if it does not exist
#pragma warning disable CS0642
                using (System.IO.File.Create(this._path)) ;
#pragma warning restore CS0642
            }else
            {
                // File exists, so load it
                var rawData = File.ReadAllText(this._path);

                // And decode
                this._data = JsonConvert.DeserializeObject<List<RunePage>>(rawData);
            }
        }

        public bool Update(List<RunePage> NewData)
        {
            var rawData = JsonConvert.SerializeObject(NewData);

            try
            {
                File.WriteAllText(this._path, rawData);
            }catch
            {
                return false;
            }

            // Update class data if it's going to be used later on
            this._data = JsonConvert.DeserializeObject<List<RunePage>>(rawData);

            return true;
        }
    }
}
