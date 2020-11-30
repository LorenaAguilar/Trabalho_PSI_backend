namespace SSG_API.Models
{
    public class SignUpUserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string NomeCompleto { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public string LinkFoto { get; set; }
        public string Biografia { get; set; }
        public string Tipo { get { return Tipo.Trim().ToUpper(); } set { Tipo = value; } }
        public string Cpf { get; set; }
        //public string Avaliacao { get; set; }

    }
}
