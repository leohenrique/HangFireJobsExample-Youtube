using FluentNHibernate.Mapping;

namespace HangFireJobsExample_Youtube.Domain
{
    public class UsuarioMap : ClassMap<Usuario>
    {
        public UsuarioMap()
        {
            Table("Usuario");
            Id(x => x.Id); //.GeneratedBy.GuidComb();
            Map(x => x.Nome);
            Map(x => x.Email);
            Map(x => x.SenhaHash);
            //HasMany(x => x.Contatos)
            //    .Cascade.All()
            //    .Inverse();
        }
    }
}
