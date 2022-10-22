using BlazorContolWork.Data;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlazorContolWork.Data
{
    public static class MongoExamples
    {
        static MongoClient client = new MongoClient();
        static IMongoDatabase database = client.GetDatabase("DBUser");
        static IMongoCollection<User> collection = database.GetCollection<User>("UserReg");

        public static void AddToDB(User user)
        {
            collection.InsertOne(user);
        }

        private static void UpdateBase()
        {
            collection = database.GetCollection<User>("UserReg");
        }

        public static User FindLogPass(string log, string pass)
        {
            UpdateBase();
            var one = collection.Find(x => x.Login == log && x.Password == pass).FirstOrDefault();
            return one;
        }

        public static User FindLog(string log)
        {
            UpdateBase();
            var one = collection.Find(x => x.Login == log).FirstOrDefault();
            return one;
        }

        public static User FindId(ObjectId _id)
        {
            UpdateBase();
            var one = collection.Find(x => x._id == _id).FirstOrDefault();
            return one;
        }


        public static List<User> FindAll()
        {
            UpdateBase();
            var list = collection.Find(x => true).ToList();
            return list;
        }

        public static void ReplaceById(ObjectId _id, User newUser)
        {
            collection.ReplaceOne(x => x._id == _id, newUser);
        }
    }
}
