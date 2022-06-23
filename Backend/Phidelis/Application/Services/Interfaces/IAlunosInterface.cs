using Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IAlunosService
    {
        void Add<Alunos>(Alunos entity);
        void AddRange(List<Alunos> entity);
        void Update<Alunos>(Alunos entity);
        void Delete<Alunos>(Alunos entity);

        void DeleteAll();

        Task<bool> SaveChangeAsync();

        Task<Alunos[]> GetAll(string filter);

        Task<Alunos> GetAlunoById(long id);

        Task<List<Alunos>> GenerateAluno(int quantidade);


        string AlunoValidate(Alunos model);
    }
}
