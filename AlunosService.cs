using AlunosApi.Context;
using AlunosApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AlunosApi.Services
{
    public class AlunosService : IAlunoService
    {
        private readonly AppDbContext _context;

        public AlunosService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Aluno>> GetAlunos()
        {
            return await _context.Alunos.ToListAsync();
        }


        public async Task<IEnumerable<Aluno>> GetAlunosByNome(string nome)
        {
            IEnumerable<Aluno> alunos;
            if (!string.IsNullOrEmpty(nome))
            {
                alunos = await _context.Alunos.Where(n => n.Nome.Contains(nome)).ToListAsync();
            }
            else
            {
                alunos = await GetAlunos();
            }
            return alunos;
        }

        public async Task<Aluno> GetAluno(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            return aluno;
        }
                
        public async Task CreateAluno(Aluno aluno)
        {
            _context.Alunos.Add(aluno); // inclui no contexto
            await _context.SaveChangesAsync();// persiste banco de dados
        }

        public async Task UpdateAluno(Aluno aluno)
        {
            _context.Entry(aluno).State = EntityState.Modified; // apenas informo que a entidade foi modificada
            await _context.SaveChangesAsync();// persiste banco de dados
        }

        public async Task DeleteAluno(Aluno aluno)
        {
            _context.Alunos.Remove(aluno); // inclui no contexto
            await _context.SaveChangesAsync();// persiste banco de dados
        }

        

       

       

        
    }
}
