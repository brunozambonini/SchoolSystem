using Application.Models;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IGeradorNomeService
    {
        [Get("/nomes/{quantidade}")]
        Task<List<string>> GetNamesAsync(int quantidade);
    }
}
