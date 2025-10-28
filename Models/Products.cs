using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;

namespace MagazijnApp.Models;

public class Products
{
    [SQLite.PrimaryKey, SQLite.AutoIncrement]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string? ProductName { get; set; }
    
    [Required]
    [Range(0, 100)]
    public int Quantity { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string? Location { get; set; }
}