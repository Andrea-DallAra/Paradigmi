using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Paradigmi.Classi
{
    [Table("utente", Schema = "dbo")]
    public class Utente
    {
        [Key]
        public int id { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        public string nome { get; set; }

        [Required]
        public string cognome { get; set; }

        [Required]
        public string password{ get; set; }

         public ICollection<Booking> bookings { get; set; }
     

        public Utente() 
        {    
        }
        public Utente(String _nome, String _cognome, String _email, String _password) 
        {
            nome = _nome;
            cognome = _cognome;
            email = _email;
            password = _password;
        }
    }
}
