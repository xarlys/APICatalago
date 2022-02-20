using System.Text.Json;

namespace APICatalago
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public string? Trace { get; set; }
        public override string ToString()
        {
            string jsonString = JsonSerializer.Serialize(this);
            return jsonString;
        }
    }
}
