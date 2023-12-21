using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Independent_Reader_GUI.Models
{
    internal class APIResponse
    {
        [JsonPropertyName("_sid")]
        public int? SubmoduleID { get; set; }
        [JsonPropertyName("_mid")]
        public int? ModuleID { get; set; }
        [JsonPropertyName("_duration_us")]
        public int? DurationInMicroSeconds { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; } = string.Empty;
        [JsonPropertyName("response")]
        public string? Response { get; set; } = string.Empty;
    }
}
