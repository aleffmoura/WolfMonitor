namespace Totten.Solutions.WolfMonitor.Register.Commands
{
    public class UserCreateCommand
    {
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Language { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
