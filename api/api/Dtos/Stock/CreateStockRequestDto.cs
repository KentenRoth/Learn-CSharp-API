using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Stock;

public class CreateStockRequestDto
{
    [Required]
    [MaxLength(10, ErrorMessage = "Symbol must be 10 characters or less")]
    public string Symbol { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(15, ErrorMessage = "Company must be 15 characters or less")]
    public string CompanyName { get; set; } = string.Empty;
    
    [Required]
    [Range(1, 100000000000)]
    public decimal Price { get; set; }
    
    [Required]
    [Range(0.001, 100)]
    public decimal LastDiv { get; set; }
    
    [Required]
    [MaxLength(50, ErrorMessage = "Industry must be 50 characters or less")]
    public string Industry { get; set; } = string.Empty;
    
    [Required]
    [Range(1, 100000000000)]
    public long MarketCap { get; set; }
}