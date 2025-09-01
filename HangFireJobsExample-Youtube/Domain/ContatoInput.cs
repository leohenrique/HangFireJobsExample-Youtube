namespace HangFireJobsExample_Youtube.Domain
{
    public class ContatoInput
    {
        public virtual string Nome { get; set; }
        public virtual string Telefone { get; set; }

    }

    public class ContatoAgrupado
    {
        public virtual string Nome { get; set; }

        public virtual IList<Contato> Contatos { get; set; } 
    }
}
