using System.Text.Json.Serialization;

namespace LoginSystem.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }
    }
}
