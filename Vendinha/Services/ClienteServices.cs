using Vendinha.Contexts;
using Vendinha.Models;
using System.ComponentModel.DataAnnotations;

namespace Vendinha.Services;

public class ClienteService
{
    private VendinhaDbContext context;

    public ClienteService(VendinhaDbContext context)
    {
        this.context = context;
    }


    public ClienteService() : this(new VendinhaDbContext(ConnectionString))
    {
    }
    
    private const string ConnectionString = 
        "Server=172.16.103.22;Port=5432;Database=escola_rodrigo;" +
        "User Id=postgres;Password=postgres;";

    public List<Cliente> Consultar(int page = 1)
    {
        int pageSize = 20;  // limita 20 resultados
        int skip = (page - 1) * pageSize;
        
        var resultado = context.Clientes
            .OrderBy(a => a.Nome)
            .Skip(skip)
            .Take(pageSize)
            .ToList();

        return resultado;
    }
    public List<Cliente> Consultar(string pesquisa, int page = 1)
    { 
        bool FiltrarCliente(Cliente a)
        {
            return a.Nome.Contains(pesquisa);
        }
        var clientes = Consultar();
        int pageSize = 20;
        int skip = (page - 1) * pageSize;

        var resultado = context.Clientes
            .Where((cliente) => cliente.Nome.Contains(pesquisa) || 
                cliente.Email.Contains(pesquisa) || 
                cliente.CPF == pesquisa
            )
            .OrderBy(x => x.Nome)
            .Skip(skip)
            .Take(pageSize);

        var resultado2 = from cliente in clientes
            where cliente.Nome.Contains(pesquisa)
            orderby cliente.Nome
            select cliente;

            return resultado.ToList();
        }

        public bool Criar(Cliente cliente,
            out List<ValidationResult> erros)
        {
            if (!Validar(cliente, out erros))
            {
                return false;
            }

            cliente.DataNascimento = cliente.DataNascimento.Date.ToUniversalTime();
            context.Add(cliente);  // INSERT
            context.SaveChanges(); // Commit
            return true;
        }

        public bool Validar(Cliente aluno,
            out List<ValidationResult> erros)
        {
            var vcontext = new ValidationContext(aluno);
            erros = new List<ValidationResult>();
            var valida = Validator.TryValidateObject(aluno, vcontext, erros, true);

            var existente = context.Clientes.Any(a => a.CPF == aluno.CPF);

            if (existente)
            {
                erros.Add(new ValidationResult($"O código {aluno.CPF} já existe",
                    ["CPF"]));
                valida = false;
            }

            return valida;
    }
}
    