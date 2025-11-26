using System;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace JungholmInstruments.Models
{
    [Table("profiles")]
    public class Profile : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("first_name")]
        public string? FirstName { get; set; }

        [Column("last_name")]
        public string? LastName { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("role")]
        public string? Role { get; set; }

        [Column("avatar_url")]
        public string? AvatarUrl { get; set; }
    }
}

