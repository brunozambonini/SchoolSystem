using Application.Models;
using Refit;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IGeradorCPF
    {
        [Get("/cpf?quantity=1")]
        Task<CPFGeneratorResponse> GetCPFAsync();
    }
}
