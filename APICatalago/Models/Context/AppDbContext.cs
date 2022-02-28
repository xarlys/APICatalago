﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Models.Context
{
    public class AppDbContext: IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

        public DbSet<Categoria> Categorias; //=> Set<Categoria>();
        public DbSet<Produto> Produtos => Set<Produto>();

        //public DbSet<Categoria> Categorias { get; set; }
        //public DbSet<Produto> Produtos { get; set; }

    }
}
