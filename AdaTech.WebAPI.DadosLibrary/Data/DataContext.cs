﻿using AdaTech.WebAPI.Entities.Entity.Objects;
using AdaTech.WebAPI.Entities.Entity.Relacional;
using Microsoft.EntityFrameworkCore;

namespace AdaTech.WebAPI.DadosLibrary.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ItemVenda> ItensVenda { get; set; }
        public DbSet<DevolucaoTroca> DevolucoesTrocas { get; set; }
        public DbSet<ItemDevolucaoTroca> ItensDevolucaoTroca { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Venda>()
                .HasOne(v => v.Cliente)
                .WithMany(c => c.Vendas)
                .HasForeignKey(v => v.ClienteId);

            modelBuilder.Entity<ItemVenda>()
                .HasOne(iv => iv.Produto)
                .WithMany()
                .HasForeignKey(iv => iv.ProdutoId);

            modelBuilder.Entity<DevolucaoTroca>()
                .HasMany(dt => dt.Itens)
                .WithOne(idt => idt.DevolucaoTroca)
                .HasForeignKey(idt => idt.DevolucaoTrocaId);

            modelBuilder.Entity<ItemDevolucaoTroca>()
                .HasOne(idt => idt.Produto)
                .WithMany()
                .HasForeignKey(idt => idt.ProdutoId);

            modelBuilder.Entity<ItemDevolucaoTroca>()
                .HasOne(idt => idt.DevolucaoTroca)
                .WithMany(dt => dt.Itens)
                .HasForeignKey(idt => idt.DevolucaoTrocaId);

            modelBuilder.Entity<ItemVenda>()
                .HasOne(iv => iv.Produto)
                .WithMany()
                .HasForeignKey(iv => iv.ProdutoId);

            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Endereco)
                .WithMany()
                .HasForeignKey(c => c.EnderecoId);

            modelBuilder.Entity<ItemVenda>()
                .HasOne(iv => iv.Venda)
                .WithMany(v => v.ItensVendas)
                .HasForeignKey(iv => iv.VendaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
