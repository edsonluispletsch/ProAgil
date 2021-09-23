using System;

namespace ProAgil.Domain
{
    public class Lote
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int Quantidade { get; set; }
        public int EventoID { get; set; }
        public Evento Evento { get; }
    }
}