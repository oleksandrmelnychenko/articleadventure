using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Helpers
{
    public static class ArticleAdventureFolderManager
    {
        private static string _serverPathFolder;
        private static string _serverUrl;
        private static string _clientUrl;
        public static string stripePublicKey;


        private static string _staticFolder = "wwwroot\\";

        private static string _imagelFiles = "images";

       
        public static void InitializeServerFolderManager(WebApplicationBuilder builder)
        {
            stripePublicKey = builder.Configuration.GetValue<string>("StripeSettings:PubKey");
            _serverUrl = builder.Configuration.GetValue<string>("ServerSettings:ServerUrl");
            _serverPathFolder = builder.Environment.ContentRootPath;
            _clientUrl = builder.Configuration.GetValue<string>("ServerSettings:ClientUrl");
            CreateImageFilesFolderIfNotExists();
        }

        private static void CreateImageFilesFolderIfNotExists()
        {
            if (!Directory.Exists(Path.Combine(_serverPathFolder, _imagelFiles)))
            {
                Directory.CreateDirectory(Path.Combine(_serverPathFolder, _imagelFiles));
            }
        }
        public static string GetServerUrl() => _serverUrl;
        public static string GetServerPathFolder() => _serverPathFolder;
        public static string GetClientPath() => _clientUrl;
        public static string GetFilesFolderPath() => Path.Combine(_serverPathFolder, _staticFolder);

        //public static string GetStaticFolder() => _staticFolder;
        public static string GetStaticServerUrlImageFolder() => Path.Combine(_serverPathFolder, _imagelFiles);

        public static string GetStaticImageFolder() => _imagelFiles;
    }
}
