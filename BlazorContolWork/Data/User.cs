using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlazorContolWork.Data
{
    public class User
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId _id;

        [BsonIgnoreIfDefault]
        public ObjectId _idPhoto;

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public User(string name, string surName, string email, string password, string login)
        {
            Name = name;
            Surname = surName;
            Password = password;
            Email = email;
            Login = login;
        }
    }
}
