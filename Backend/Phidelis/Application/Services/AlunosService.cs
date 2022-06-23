﻿using Application.Context;
using Application.Models;
using Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AlunosService : IAlunosService
    {
        private readonly PhidelisContext _context;

        const string geradorNomeAPI = "https://gerador-nomes.herokuapp.com";
        const string geradorCPFAPI = "https://2devs.com.br/v1/";

        public AlunosService(PhidelisContext context)
        {
            _context = context;
        }

        public void Add<Alunos>(Alunos entity)
        {
            _context.Add(entity);
        }

        public void AddRange(List<Alunos> entity)
        {
            _context.AddRange(entity);
        }

        public void Delete<Alunos>(Alunos entity)
        {
            _context.Remove(entity);
        }
        public void Update<Alunos>(Alunos entity)
        {
            _context.Update(entity);
        }

        public async Task<bool> SaveChangeAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Alunos[]> GetAllPaginated(string term, int page, int pageSize)
        {
            IQueryable<Alunos> query = _context.Alunos;

            query = query.AsNoTracking()
                    .Where(x => x.Nome.Contains(term));

            int qtdAlunos = query.Count();
            int qunatidadePaginas = Convert.ToInt32(Math.Ceiling(qtdAlunos * 1M / pageSize));

            if (page > qunatidadePaginas)
            {
                return null;
            }

            // Pula uma qunatidade de registros de acordo com a pagina
            // Se for a primeira página, pega os primeiros registros, se for a segunda, pula as primeiras que foi pego na primeira página...
            query = query.Skip(pageSize * (page - 1)).Take(pageSize);
            return await query.ToArrayAsync();
        }

        public async Task<Alunos[]> GetAll(string filter)
        {
            IQueryable<Alunos> query = _context.Alunos;

            if(!string.IsNullOrWhiteSpace(filter))
            {
                query = query.AsNoTracking()
                    .Where(x => x.Nome.ToLower().Contains(filter.ToLower()));
            }

            return await query.ToArrayAsync();
        }

        public async Task<Alunos> GetAlunoById(long id)
        {
            IQueryable<Alunos> query = _context.Alunos;

            query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async void DeleteAll()
        {
            var students = await _context.Alunos.ToArrayAsync();

            _context.Alunos.RemoveRange(students);

        }

        public string AlunoValidate(Alunos model)
        {
            string erros = "";

            if (string.IsNullOrWhiteSpace(model.Cpf) || model.Cpf.Length != 11)
                erros = "Insira um CPF válido. ";
            if (string.IsNullOrWhiteSpace(model.Matricula))
                erros += "Insira a matrícula. ";
            if (string.IsNullOrWhiteSpace(model.Nome))
                erros += "Insira um nome.";

            return erros;
        }

        public async Task<List<Alunos>> GenerateAluno(int quantidade)
        {
            // gera lista de nomes aleatórios
            var geradorNomeClient = await RestService.For<IGeradorNomeService>(geradorNomeAPI).GetNamesAsync(quantidade);

            Random numAleatorio = new Random();

            List<Alunos> alunos = new List<Alunos>();

            for (int i = 0; i < quantidade; i++)
            {
                // gera um cpf aleatório
                var geradorCPFClient = await RestService.For<IGeradorCPF>(geradorCPFAPI).GetCPFAsync();

                // formata cpf para apenas números
                var cpf = geradorCPFClient.Data.FirstOrDefault() != null ? geradorCPFClient.Data.FirstOrDefault() : "00000000000";
                cpf = cpf.Replace("-", "");
                cpf = cpf.Replace(".", "");

                // cria aluno
                var aluno = new Alunos
                {
                    Id = 0,
                    Nome = geradorNomeClient[i],
                    Cpf = cpf,
                    Matricula = numAleatorio.Next(1047483647, 2147483647).ToString() // gera matricula
                };

                alunos.Add(aluno);
            }

            return alunos;
        }
    }
}
