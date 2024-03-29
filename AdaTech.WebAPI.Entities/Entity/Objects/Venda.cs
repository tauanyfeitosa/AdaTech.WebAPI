﻿using AdaTech.WebAPI.Entities.Entity.Enums;

namespace AdaTech.WebAPI.Entities.Entity.Objects
{
    public class Venda
    {
        public int Id { get; set; }
        public IEnumerable<ItemVenda> ItensVendas { get; set; } = new List<ItemVenda>();
        public DateTime DataVenda { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public StatusVenda StatusVenda { get; set; } = StatusVenda.Efetuado;
        public decimal ValorTotal { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
