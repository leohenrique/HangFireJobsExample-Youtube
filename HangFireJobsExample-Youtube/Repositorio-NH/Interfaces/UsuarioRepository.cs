using HangFireJobsExample_Youtube.Domain;

namespace HangFireJobsExample_Youtube.Repositorio_NH.Interfaces
{
    public interface IUsuarioRepository
    {
        public Usuario? ObterPorEmail(string email);

        public Task<List<Usuario>> ListarTodosAsync();

        public Task<Usuario?> Salvar(Usuario usuario);

        public Task Remove(long id);


    }

}
