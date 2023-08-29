using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Helpers
{
    public static class ArticleAdventureFolderManager
    {
        private static string _serverPath;
        private static string _serverUrl;


        private static string _staticFolder = "wwwroot\\";

        private static string _imagelFiles = "images";

        public static void InitializeServerFolderManager(string serverPath,string serverUrl)
        {
            _serverPath = serverPath;
            _serverUrl = serverUrl;
            CreateImageFilesFolderIfNotExists();
        }

        private static void CreateImageFilesFolderIfNotExists()
        {
            if (!Directory.Exists(Path.Combine(_serverPath, _imagelFiles)))
            {
                Directory.CreateDirectory(Path.Combine(_serverPath, _imagelFiles));
            }
        }

        public static string GetFilesFolderPath() => Path.Combine(_serverPath, _staticFolder);

        //public static string GetStaticFolder() => _staticFolder;
        public static string GetStaticServerUrlImageFolder() => Path.Combine(_serverUrl, _imagelFiles);

        public static string GetStaticImageFolder() => _imagelFiles;
    }
}
