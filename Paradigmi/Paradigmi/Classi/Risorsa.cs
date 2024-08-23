using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Paradigmi.Classi
{
    [Table("risorse", Schema = "dbo")]
    public class Risorsa
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string nome { get; set; }

        [Required]
        public string tipoRisorsa { get; set; }

        public ICollection<Booking> booking { get; set; }


        public Risorsa() { }
        public Risorsa(String _nome, String _tipo) 
        {
            nome = _nome;
            tipoRisorsa = _tipo;

        }
       

    }
}
