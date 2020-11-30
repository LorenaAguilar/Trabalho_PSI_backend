using SSG_API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSG_API.Models
{
    public class OrdemDeServicoModel
    {
        public Guid? Id { get; set; }
        public Guid Prestador { get; set; }
        public Guid Contratante { get; set; }
        public Guid ServicoPrestado { get; set; }
        public DateTime? Data { get; set; }
        public double? Preco { get; set; }
        public string Endereco { get; set; }
        public string Resumo { get; set; }
        public int? FormaPagamento { get; set; }
        public int? Status { get; set; }
    }
}
