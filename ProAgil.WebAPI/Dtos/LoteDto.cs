using System;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.WebAPI.Dtos
{
    public class LoteDto
    {
        private string _dataInicio = ""; 
        private string _dataFim = ""; 
        public int ID { get; set; }
        [Required (ErrorMessage ="O campo {0} é obrigatório")]
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string DataInicio { 
            get { return this._dataInicio; } 
            set { this._dataInicio = Convert.ToDateTime(value).ToString("dd/MM/yyyy hh:mm") ; }
        }
        public string DataFim { 
            get { return this._dataFim; } 
            set { this._dataFim = Convert.ToDateTime(value).ToString("dd/MM/yyyy hh:mm") ; }
        }        
//        public DateTime ?DataInicio { get; set; }
//        public DateTime ?DataFim { get; set; }
        [Range(5, 120000, ErrorMessage ="A quantidade de pessoas deve estar entre 2 e 120.000")]        
        public int Quantidade { get; set; }
    }
}