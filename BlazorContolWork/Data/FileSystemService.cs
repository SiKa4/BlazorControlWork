using MongoDB.Driver.GridFS;
using MongoDB.Driver;

namespace BlazorContolWork.Data
{
    static public class FileSystemService
    {
        static public async Task UploadFileToDb(string path, string fileName)
        {
            var client = new MongoClient();
            var database = client.GetDatabase("FileDB");
            var gridFS = new GridFSBucket(database);

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                gridFS.UploadFromStream(fileName, fs);
            }
            File.Delete(path);
        }
    }
}
