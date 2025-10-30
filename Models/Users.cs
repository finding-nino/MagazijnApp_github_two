using SQLite;
using System.ComponentModel.DataAnnotations;
namespace MagazijnApp.Models
{
    public class Users
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Username must be numbers only (student number)")]
        [System.ComponentModel.DataAnnotations.MaxLength(20)]
        public string UserName { get; set; } = string.Empty;
        [System.ComponentModel.DataAnnotations.MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.MaxLength(128)]
        public string Salt { get; set; } = string.Empty;

    }
}


//[RegularExpression(@"^\d+$") - Enforces numbers only (^ = start, \d+ = one or more digits, $ = end)