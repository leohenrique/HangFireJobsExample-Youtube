using HangFireJobsExample_Youtube.Domain;
using HangFireJobsExample_Youtube.Repositorio_NH.Interfaces;
using NHibernate;
using NHibernate.Linq;

namespace HangFireJobsExample_Youtube.Repositorio_NH
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly NHibernate.ISession _session;

        public UsuarioRepository(NHibernate.ISession session)
        {
            _session = session;
        }

        public Usuario? ObterPorEmail(string email)
        {
            return _session.Query<Usuario>().FirstOrDefault(u => u.Email == email);
        }

        public async Task<List<Usuario>> ListarTodosAsync()
        {
            return await _session.Query<Usuario>().ToListAsync();
        }

        public async Task<Usuario?> Salvar(Usuario usuario)
        {
            ITransaction transaction = null;
            try
            {
                transaction = _session.BeginTransaction();
                await _session.SaveOrUpdateAsync(usuario);
                await transaction.CommitAsync();
                return usuario;
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

        public async Task Remove(long id)
        {
            ITransaction transaction = null;
            try
            {
                transaction = _session.BeginTransaction();
                var usuario = await _session.GetAsync<Usuario>(id);
                if (usuario != null)
                {

                    await _session.DeleteAsync(usuario);
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                // Logar o erro
                Console.WriteLine($"Erro ao remover usuário: {ex.Message}");
                transaction?.RollbackAsync();
            }
            finally
            {
                transaction?.Dispose();
            }
        }
    }
}
