using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Paradigmi.Classi
{
    [Table("booking", Schema = "dbo")]
    public class Booking
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("IdRisorsa")]
        public int idRisorsa { get; set; }

        [ForeignKey("IdUtente")]
        public int idUtente { get; set; }

        [Required]
        public DateTime inizio { get; set; }

        [Required]
        public DateTime fine { get; set; }

        [JsonIgnore]
        public Risorsa risorsa { get; set; }
        public Utente utente { get; set; }

        public Booking() { }
        public Booking(DateTime _inizio, DateTime _fine, Risorsa _risorsa = null, Utente _utente = null)
        {
            inizio = _inizio;
            fine = _fine;
            risorsa = _risorsa;
            utente = _utente;
        }



    }
}
