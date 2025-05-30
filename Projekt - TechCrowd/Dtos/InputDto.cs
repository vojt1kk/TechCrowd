using System.Text.Json.Serialization;

namespace Projekt___TechCrowd.Dtos
{
    public class InputDto
    {
        [JsonPropertyName("Title")]
        public string Title { get; set; }
        [JsonPropertyName("Content")]
        public string Content { get; set; }
        [JsonPropertyName("Author")]
        public string Author { get; set; }
        [JsonPropertyName("Genre")]
        public string Genre { get; set; }
        [JsonPropertyName("Date")]
        public DateTime Date { get; set; }
    }
}
