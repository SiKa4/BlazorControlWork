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

        static public void UploadImageToDb(string path, string fileName, User user)
        {
            var client = new MongoClient();
            var database = client.GetDatabase("ImageDB");
            var gridFS = new GridFSBucket(database);

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                gridFS.UploadFromStream(fileName, fs);
            }
            File.Delete(path);
            SearchByNameReplaceUser(fileName, user);
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


        static public void DownloadToLocal(string webRootPath, User user)
        {
            var client = new MongoClient();
            var database = client.GetDatabase("ImageDB");
            var gridFS = new GridFSBucket(database);

            using (Stream fs = new FileStream($"{webRootPath}\\Images\\photo.jpeg", FileMode.Create))
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
