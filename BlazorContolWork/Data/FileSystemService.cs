using Microsoft.AspNetCore.Components.Forms;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Diagnostics;

namespace BlazorContolWork.Data
{
    static public class FileSystemService
    {
        static public void Initialization()
        {
            var client = new MongoClient();
            var database = client.GetDatabase("ImageDB");
            var gridFS = new GridFSBucket(database);
        }

        static public async Task UploadImageToDb(string path, string fileName, User user)
        {
            var client = new MongoClient();
            var database = client.GetDatabase("ImageDB");
            var gridFS = new GridFSBucket(database);

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                gridFS.UploadFromStream(fileName, fs);
            }
            //File.Delete(path);
            await SearchByNameReplaceUser(fileName, user);
        }

        private static async Task SearchByNameReplaceUser(string fileName, User user)
        {
            var client = new MongoClient();
            var database = client.GetDatabase("ImageDB");
            var gridFS = new GridFSBucket(database);

            var filter = Builders<GridFSFileInfo>.Filter.Eq<string>(info => info.Filename, fileName);
            var fileInfos = (await gridFS.FindAsync(filter)).FirstOrDefault();
            user._idPhoto = fileInfos.Id;
            MongoExamples.ReplaceById(user._id, user);
        }
        
        public static string GetNameInDb(User user)
        {
            var client = new MongoClient();
            var database = client.GetDatabase("ImageDB");
            var gridFS = new GridFSBucket(database);

            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", user._idPhoto);
            string fileNames = string.Empty;

            using (var cursor = gridFS.Find(filter))
            {
                fileNames = cursor.FirstOrDefault().Filename;
            }
            return fileNames;
        }

        static public void DownloadToLocal(string webRootPath, User user)
        {
            var client = new MongoClient();
            var database = client.GetDatabase("ImageDB");
            var gridFS = new GridFSBucket(database);

            using (Stream fs = new FileStream($"{webRootPath}\\Images\\{GetNameInDb(user)}", FileMode.Create))
            {
                try
                {
                    gridFS.DownloadToStream(user._idPhoto, fs);
                }
                catch { }
            }
            
        }
    }
}
