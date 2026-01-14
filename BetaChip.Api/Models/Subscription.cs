using Postgrest.Attributes;
using Postgrest.Models;

namespace BetaChip.Api.Models
{
    // Supabase의 "subscriptions" 테이블과 연결
    [Table("subscriptions")]
    public class Subscription : BaseModel
    {
        [PrimaryKey("id", false)]
        public string Id { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("tier")]
        public string Tier { get; set; } = "free";

        [Column("hwid")]
        public string? Hwid { get; set; }

        [Column("expires_at")]
        public DateTime? ExpiresAt { get; set; }
    }
}