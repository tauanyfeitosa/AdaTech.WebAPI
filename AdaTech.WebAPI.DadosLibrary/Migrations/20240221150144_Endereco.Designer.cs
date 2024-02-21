﻿// <auto-generated />
using System;
using AdaTech.WebAPI.DadosLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AdaTech.WebAPI.DadosLibrary.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240221150144_Endereco")]
    partial class Endereco
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("AdaTech.WebAPI.DadosLibrary.DTO.Objects.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("EnderecoId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Sobrenome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EnderecoId");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("AdaTech.WebAPI.DadosLibrary.DTO.Objects.DevolucaoTroca", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DataDevolucaoTroca")
                        .HasColumnType("TEXT");

                    b.Property<string>("Motivo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Tipo")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VendaId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("VendaId");

                    b.ToTable("DevolucoesTrocas");
                });

            modelBuilder.Entity("AdaTech.WebAPI.DadosLibrary.DTO.Objects.Endereco", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bairro")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CEP")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Rua")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Enderecos");
                });

            modelBuilder.Entity("AdaTech.WebAPI.DadosLibrary.DTO.Objects.ItemVenda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Preco")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantidade")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VendaId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProdutoId");

                    b.HasIndex("VendaId");

                    b.ToTable("ItensVenda");
                });

            modelBuilder.Entity("AdaTech.WebAPI.DadosLibrary.DTO.Objects.Produto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Preco")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantidade")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Produtos");
                });

            modelBuilder.Entity("AdaTech.WebAPI.DadosLibrary.DTO.Objects.Venda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClienteId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DataVenda")
                        .HasColumnType("TEXT");

                    b.Property<int>("StatusVenda")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.ToTable("Vendas");
                });

            modelBuilder.Entity("AdaTech.WebAPI.DadosLibrary.DTO.Relacional.ItemDevolucaoTroca", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DevolucaoTrocaId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantidade")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DevolucaoTrocaId");

                    b.HasIndex("ProdutoId");

                    b.ToTable("ItensDevolucaoTroca");
                });

            modelBuilder.Entity("AdaTech.WebAPI.DadosLibrary.DTO.Objects.Cliente", b =>
                {
                    b.HasOne("AdaTech.WebAPI.DadosLibrary.DTO.Objects.Endereco", "Endereco")
                        .WithMany()
                        .HasForeignKey("EnderecoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Endereco");
                });

            modelBuilder.Entity("AdaTech.WebAPI.DadosLibrary.DTO.Objects.DevolucaoTroca", b =>
                {
                    b.HasOne("AdaTech.WebAPI.DadosLibrary.DTO.Objects.Venda", "Venda")
                        .WithMany()
                        .HasForeignKey("VendaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Venda");
                });

            modelBuilder.Entity("AdaTech.WebAPI.DadosLibrary.DTO.Objects.ItemVenda", b =>
                {
                    b.HasOne("AdaTech.WebAPI.DadosLibrary.DTO.Objects.Produto", "Produto")
                        .WithMany()
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AdaTech.WebAPI.DadosLibrary.DTO.Objects.Venda", "Venda")
                        .WithMany("ItensVendas")
                        .HasForeignKey("VendaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Produto");

                    b.Navigation("Venda");
                });

            modelBuilder.Entity("AdaTech.WebAPI.DadosLibrary.DTO.Objects.Venda", b =>
                {
                    b.HasOne("AdaTech.WebAPI.DadosLibrary.DTO.Objects.Cliente", "Cliente")
                        .WithMany("Vendas")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("AdaTech.WebAPI.DadosLibrary.DTO.Relacional.ItemDevolucaoTroca", b =>
                {
                    b.HasOne("AdaTech.WebAPI.DadosLibrary.DTO.Objects.DevolucaoTroca", "DevolucaoTroca")
                        .WithMany("Itens")
                        .HasForeignKey("DevolucaoTrocaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AdaTech.WebAPI.DadosLibrary.DTO.Objects.Produto", "Produto")
                        .WithMany()
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DevolucaoTroca");

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("AdaTech.WebAPI.DadosLibrary.DTO.Objects.Cliente", b =>
                {
                    b.Navigation("Vendas");
                });

            modelBuilder.Entity("AdaTech.WebAPI.DadosLibrary.DTO.Objects.DevolucaoTroca", b =>
                {
                    b.Navigation("Itens");
                });

            modelBuilder.Entity("AdaTech.WebAPI.DadosLibrary.DTO.Objects.Venda", b =>
                {
                    b.Navigation("ItensVendas");
                });
#pragma warning restore 612, 618
        }
    }
}
