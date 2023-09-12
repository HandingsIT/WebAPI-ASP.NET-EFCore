using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

public enum UserType
{
    Student = 1,
    Professor,
    Admin,
    Developer,
}

namespace WebAPISever.Models
{
    [Index(nameof(Account), IsUnique = true)]

    public class User
    {
        [Key]
        [NotNull]
        [Column(TypeName = "BIGINT")]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? Account { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? Password { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string? Name { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? Mail { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        public DateTime? CreateTime { get; set; }
        public DateTime? LogInTime { get; set; }
        public DateTime? LogOutTime { get; set; }
    }
}
