using System;
using System.IO;
using DropNet;

namespace PhotoContest.Web.Helpers
{
    public class Dropbox
    {
        private static DropNetClient client;

        static Dropbox()
        {
            client = new DropNetClient(AppKeys.DropboxApiKey, AppKeys.DropboxApiSecret, AppKeys.DropboxAccessTDropboxAccessToken);
        }


        //internal static string Upload(string username, string fileName, Stream fileStream)
        //{
        //    string fullFileName = "" + username + "_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + fileName;
        //    client.UploadFile("/" + AppKeys.DropboxFolder + "/", fullFileName, fileStream);
        //    return fullFileName;
        //}

        internal static string Upload(string username, string fileName, Stream fileStream, string subFolder)
        {
            string fullFileName = "" + username + "_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + fileName;
            client.UploadFile("/" + AppKeys.DropboxFolder + "/" + subFolder + "/", fullFileName, fileStream);
            return fullFileName;
        }


        //internal static string Download(string path)
        //{
        //    return client.GetMedia("/" + AppKeys.DropboxFolder + "/" + path).Url;
        //}

        internal static void Delete(string path)
        {
            client.Delete("/" + AppKeys.DropboxFolder + "/" + path);
        }

        internal static void DeleteThumbnail(string path)
        {
            client.Delete("/" + AppKeys.DropboxFolder + "/Thumbnails/" + path);
        }

        internal static string Download(string path, string subFolder)
        {
            string fullPath = "/" + AppKeys.DropboxFolder;

            if (subFolder != null)
            {
                fullPath += "/" + subFolder;
            }

            fullPath += "/" + path;

            return client.GetMedia(fullPath).Url;
        }
    }
}