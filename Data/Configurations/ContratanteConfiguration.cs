﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json.Bson;
using SSG_API.Domain;

namespace SSG_API.Data
{
    public class ContratanteConfiguration : IEntityTypeConfiguration<Contratante>
    {
        public void Configure(EntityTypeBuilder<Contratante> builder)
        {
            builder.ToTable("Contratante");
            //builder.HasKey(p => p.UserID);
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.User);
            //builder.Property(p => p.Password);
            //builder.Property(p => p.PasswordHash);
            //builder.Property(p => p.NomeCompleto);
            //builder.Property(p => p.Endereco);
            //builder.Property(p => p.Telefone);
            //builder.Property(p => p.LinkFoto);

            //builder.HasOne(p => p.PrestadorDeservico);
            //builder.HasMany(p => p.OrdemDeServico);
        }
    }
}
