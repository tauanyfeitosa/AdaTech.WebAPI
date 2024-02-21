﻿using AdaTech.WebAPI.DadosLibrary.DTO.Objects;

namespace AdaTech.WebAPI.DadosLibrary.DTO.Relacional
{
    public class ItemDevolucaoTroca
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int DevolucaoTrocaId { get; set; }
        public DevolucaoTroca DevolucaoTroca { get; set; }
        public int Quantidade { get; set; }
    }
}