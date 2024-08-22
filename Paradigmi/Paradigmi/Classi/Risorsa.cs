using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Paradigmi.Classi
{
    [Table("Risorse")]
    public class Risorsa
    {
        [Key]
        private int id { get; set; }

        [Required]
        private string nome { get; set; }

        [Required]
        private string tipoRisorsa { get; set; }

        private ICollection<Booking> booking;


        public Risorsa() { }
        public Risorsa(String nome, String tipo) 
        {
            SetNome(nome);
            SetTipoRisorsa(tipo);

        }
        public int GetId()
        {
            return id;
        }

        public void SetId(int _id)
        {
            if (_id <= 0)
            {
                throw new ArgumentException("Id deve essere un numero positivo.");
            }
            id = _id;
        }

  
        public string GetNome()
        {
            return nome;
        }

        public void SetNome(string _nome)
        {
            if (string.IsNullOrEmpty(_nome))
            {
                throw new ArgumentException("Nome non puo' essere nullo o vuoto.");
            }
            nome = _nome;
        }

        public string GetTipoRisorsa()
        {
            return tipoRisorsa;
        }

        public void SetTipoRisorsa(string _tipo)
        {
            if (string.IsNullOrEmpty(_tipo))
            {
                throw new ArgumentException("TipoRisorsa non puo' essere nullo o vuoto.");
            }
            tipoRisorsa = _tipo;
        }

        public ICollection<Booking> GetBooking() 
        {
            return booking;
        }
        public void SetBooking(ICollection<Booking> _booking) 
        {
            booking = _booking;
        }

    }
}
