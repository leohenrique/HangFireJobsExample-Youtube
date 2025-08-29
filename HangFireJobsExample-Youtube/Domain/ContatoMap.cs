using FluentNHibernate.Mapping;

namespace HangFireJobsExample_Youtube.Domain
{
    public class ContatoMap : ClassMap<Contato>
    {
        public ContatoMap()
        {
            Id(x => x.Id);
            Map(x => x.Nome);
            Map(x => x.Telefone);
            References(x => x.Usuario)
                .Column("IdUsuario");
        }
    }
}
