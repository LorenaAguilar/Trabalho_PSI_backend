﻿using Microsoft.AspNetCore.Identity;
using SSG_API.Data;
using SSG_API.Domain;
using SSG_API.Security;
using System;
using System.Linq;

namespace SSG_API.Business
{
    public class PrestadorService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        
        public PrestadorService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public object GetByEmail(string email)
        {
            return from p in _applicationDbContext.Prestadores
                   where p.User.NormalizedEmail == email.Trim().ToUpper()
                   select new { 
                       p.Id,
                       p.Biografia,
                       p.User.Email,
                       p.User.NomeCompleto,
                       p.User.Telefone,
                       p.User.LinkFoto,
                       p.User.Endereco,
                       p.User.Avaliacao,
                       p.User.Cpf
                   };
        }

        public object GetById(Guid id)
        {
            return from p in _applicationDbContext.Prestadores
                   where p.Id == id
                   select new { 
                       p.Id,
                       p.Biografia,
                       p.User.Email,
                       p.User.NomeCompleto,
                       p.User.Telefone,
                       p.User.LinkFoto,
                       p.User.Endereco,
                       p.User.Avaliacao,
                       p.User.Cpf
                   };
        }

        public object GetByUserName(string userName)
        {
            return GetByEmail(userName);
        }
    }
}
