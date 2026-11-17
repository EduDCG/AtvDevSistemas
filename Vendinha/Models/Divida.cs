using System.ComponentModel.DataAnnotations;

namespace Vendinha.Models;

public class Divida
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public float Valor  { get; set; }
    
    public bool Paga { get; set; } = false;
    
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime DataPagamento;
    
    [Required]
    public string CPF { get; set; } = string.Empty;

}