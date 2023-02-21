namespace Cl9Backup.CLI.Domain.Entities
{
    public class Login
    {
        public Login() { }
        public Login(string nome, string email, string senha)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
        }
        public int Id { get; set; }
        public string Nome { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Senha { get; set; } = default!;

        public override string ToString() => $"{Id} - {Nome} - {Email}";
    }
}
