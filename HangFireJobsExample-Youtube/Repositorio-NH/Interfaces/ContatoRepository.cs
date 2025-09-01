using HangFireJobsExample_Youtube.Domain;

namespace HangFireJobsExample_Youtube.Repositorio_NH.Interfaces
{
    public interface IContatoRepository
    {

        public Task<Contato?> Salvar(Contato contato);
    }
}
