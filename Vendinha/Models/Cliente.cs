using System.ComponentModel.DataAnnotations;

namespace Vendinha.Models;

public class Cliente
{
    
    [Key]
    [Required, StringLength(11)]
    public String CPF { get; set; }
    
    [Required]
    public String Nome { get; set; }
    
    [Required]
    public DateTime DataNascimento { get; set; }
    
    [EmailAddress]
    public String Email
    {
        get;
        set
        {
            this.Email = value.ToLower().Trim();
        }
    }

    public int Idade
    {
        get
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - DataNascimento.Year;
            var hojeAnoNascimento = hoje.AddYears(-idade);
            if (DataNascimento > hojeAnoNascimento)
            {
                idade--;
            }
            return idade;

        }
    }

    public List<Divida> Dividas { get; set; } = new();
}