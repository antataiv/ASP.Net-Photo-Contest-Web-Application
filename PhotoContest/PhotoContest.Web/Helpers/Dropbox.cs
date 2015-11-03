//using System;
//using System.IO;
//using DropNet;

//namespace PhotoContest.Web.Helpers
//{
//    public class Dropbox
//    {
//        private static DropNetClient client;

//        static Dropbox()
//        {
//            client = new DropNetClient(AppKeys.Storage_Account_Name, AppKeys.DropboxApiSecret, AppKeys.PrimaryAccessKey);
//        }


//        //internal static string Upload(string username, string fileName, Stream fileStream)
//        //{
//        //    string fullFileName = "" + username + "_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + fileName;
//        //    client.UploadFile("/" + AppKeys.DropboxFolder + "/", fullFileName, fileStream);
//        //    return fullFileName;
//        //}

//        internal static string Upload(string username, string fileName, Stream fileStream, string subFolder)
//        {
//            string fullFileName = "" + username + "_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + fileName;
//            client.UploadFile("/" + AppKeys.dd + "/" + subFolder + "/", fullFileName, fileStream);
//            return fullFileName;
//        }


//        //internal static string Download(string path)
//        //{
//        //    return client.GetMedia("/" + AppKeys.DropboxFolder + "/" + path).Url;
//        //}

//        internal static void Delete(string path)
//        {
//            client.Delete("/" + AppKeys.dd + "/" + path);
//        }

//        internal static void DeleteThumbnail(string path)
//        {
//            client.Delete("/" + AppKeys.dd + "/Thumbnails/" + path);
//        }

//        internal static string Download(string path, string subFolder)
//        {
//            string fullPath = "/" + AppKeys.dd;

//            if (subFolder != null)
//            {
//                fullPath += "/" + subFolder;
//            }

//            fullPath += "/" + path;

//            return client.GetMedia(fullPath).Url;
//        }
//    }
//}