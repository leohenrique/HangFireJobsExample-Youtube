namespace HangFireJobsExample_Youtube.Domain
{
    public class Contato
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Telefone { get; set; }
        public virtual Usuario Usuario { get; set; }

    }
}
