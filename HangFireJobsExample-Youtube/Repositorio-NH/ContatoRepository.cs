using HangFireJobsExample_Youtube.Domain;
using NHibernate;

namespace HangFireJobsExample_Youtube.Repositorio_NH
{
    public class ContatoRepository
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
    }
}
