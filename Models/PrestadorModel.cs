using System;

namespace SSG_API.Models
{
    public class PrestadorModel
    {
        public Guid ApplicationUserID { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public string Cpf { get; set; }
        public string Biografia { get; set; }

    }
}
