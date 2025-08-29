namespace HangFireJobsExample_Youtube.Domain
{

    public class Usuario
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Email { get; set; }
        public virtual string SenhaHash { get; set; }
        
        public virtual IList<ContatoInput> Contatos { get; set; } = new List<ContatoInput>();
    }
}
