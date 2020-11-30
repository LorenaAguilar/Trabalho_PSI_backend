using System;
using System.Globalization;

namespace SSG_API.Domain
{
    public class OrdemDeServico
    {
        public Guid Id { get; set; }
        public Prestador Prestador { get; set; }
        public Contratante Contratante { get; set; }
        public ServicoPrestado ServicoPrestado { get; set; }

        private DateTime _data;
        public DateTime DataPrestacao
        {
            get => _data;
            set => _data = value;
        }

        private double _preco;
        public double Preco
        {
            get => _preco;
            set => _preco = value;
        }
        
        private string _endereco;
        public string Endereco
        {
            get => _endereco;
            set => _endereco = value?.Trim();
        }

        private string _resumo;
        public string Resumo
        {
            get => _resumo;
            set => _resumo = value?.Trim();
        }

        private int _status;
        public int Status
        {
            /*
             * status: 
             * 0 aguardando confirmação.
             * 1 em aberto.
             * 2 finalizado.
             * 3 cancelado.
             */

            get => _status;
            set => _status = (_status > 1) ? throw new Exception("O status não pode ser alterado...") : value;
        }

        private int _formaPagamento;
        public int FormaPagamento
        {
            /*
             * forma_pagamento:
             * 0 dinheiro
             * 1 débito
             * 2 crédito
             */

            get => _formaPagamento;
            set => _formaPagamento = value;
        }
    }
}