using System;

namespace SSG_API.Models
{
    public class FacebookSignInModel
    {
        public string AccessToken { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public string Biografia { get; set; }
        public string Tipo { get { return Tipo.Trim().ToUpper(); } set { Tipo = value; } }
        public string Cpf { get; set; }
        public Guid UnidadeDeCobranca { get; set; }
        public Guid LocalDeAtendimento { get; set; }
        public Guid Servico { get; set; }
        public double Preco { get; set; }
    }
}
