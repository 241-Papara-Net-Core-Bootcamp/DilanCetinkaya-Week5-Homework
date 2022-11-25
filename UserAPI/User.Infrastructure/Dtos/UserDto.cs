using System.Text.Json.Serialization;

namespace User.Infrastructure.Dtos
{
    public class UserDto
    {
        [JsonPropertyName("userId")]
        public int UserId { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}
