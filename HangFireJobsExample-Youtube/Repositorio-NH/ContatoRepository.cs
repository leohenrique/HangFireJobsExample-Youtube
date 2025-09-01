using HangFireJobsExample_Youtube.Domain;
using HangFireJobsExample_Youtube.Repositorio_NH.Interfaces;
using NHibernate;
using NHibernate.Linq;

namespace HangFireJobsExample_Youtube.Repositorio_NH
{
    public class ContatoRepository : IContatoRepository
    {
        private readonly NHibernate.ISession _session;

        public ContatoRepository(NHibernate.ISession session)
        {
            _session = session;
        }

        public async Task<Contato?> Salvar(Contato contato)
        {
            ITransaction transaction = null;
            try
            {
                transaction = _session.BeginTransaction();
                await _session.SaveOrUpdateAsync(contato);
                await transaction.CommitAsync();
                return contato;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await transaction?.RollbackAsync();
            }
            finally
            {
                transaction?.Dispose();
            }
            return null;
        }

        public async Task<List<int>> BuscarListaIds()
        {
            return await _session.Query<Contato>()
                .Select(c => c.Id)
                .ToListAsync();
        }

        public async Task<ContatoInput> BuscarDetalhePorId(int id)
        {
            var listaIN = new[] { id, id + 1 };

           var result = await _session.Query<Contato>()
                .Where(c => listaIN.Contains(c.Id))
                .Select(c => new ContatoInput { 
                    Nome = c.Nome,
                    Telefone = c.Telefone
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<ContatoAgrupado>> BuscarAgrupadoPorNomeContato()
        {
            
            var result = await _session.Query<Contato>()                 
                 .GroupBy(c => c.Nome)
                 .Select(c => new ContatoAgrupado
                 {
                     Nome = c.Key,
                     Contatos = c.ToList()
                 })
                 .ToListAsync();

            return result;
        }
    }
}
