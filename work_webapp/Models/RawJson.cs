using System;

namespace work_webapp.Models
{
    public class RawJson
    {
        public int Id { get; set; }
        public string SourceFile { get; set; } = string.Empty;
        public string JsonContent { get; set; } = string.Empty;
        public DateTime ImportedAt { get; set; } = DateTime.UtcNow;
    }
}
