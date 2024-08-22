using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paradigmi.Classi
{
    [Table("Booking")]
    public class Booking
    {
        [Key]
        private int id;

        [ForeignKey("IdRisorsa")]
        private int idRisorsa;

        [ForeignKey("IdUtente")]
        private int idUtente;

        [Required]
        private DateTime inizio;

        [Required]
        private DateTime fine;

        private Risorsa risorsa;
        private Utente utente;

        public Booking() { }
        public Booking(DateTime inizio, DateTime fine) 
        {
            SetInizio(inizio);
            SetFine(fine);
        }
        public int GetId()
        {
            return id;
        }

        public void SetId(int _id)
        {
            if (_id <= 0)
            {
                throw new ArgumentException("l'id deve essere positivo.");
            }
            id = _id;
        }

        public int GetIdRisorsa()
        {
            return idRisorsa;
        }

        public void SetIdRisorsa(int _idRisorsa)
        {
            if (_idRisorsa <= 0)
            {
                throw new ArgumentException("IdRisorsa deve essere positivo.");
            }
            idRisorsa = _idRisorsa;
        }

        public int GetIdUtente()
        {
            return idUtente;
        }

        public void SetIdUtente(int _idUtente)
        {
            if (_idUtente <= 0)
            {
                throw new ArgumentException("IdUtente deve essere positivo.");
            }
            idUtente = _idUtente;
        }

        public DateTime GetInizio()
        {
            return inizio;
        }

        public void SetInizio(DateTime _inizio)
        {
            inizio = _inizio;
        }

        public DateTime GetFine()
        {
            return fine;
        }

        public void SetFine(DateTime _fine)
        {

            if (_fine < inizio)
            {
                throw new ArgumentException("La data di fine non puo' essere precedente a quella di inizio.");
            }
            fine = _fine;
        }

        public Risorsa GetRisorsa()
        {
            return risorsa;
        }

        public void SetRisorsa(Risorsa _risorsa)
        {
            if (_risorsa == null)
            {
                throw new ArgumentNullException("Risorsa non puo' essere null.");
            }
            risorsa = _risorsa;
        }

        public Utente GetUtente()
        {
            return utente;
        }

        public void SetUtente(Utente _utente)
        {
            if (_utente == null)
            {
                throw new ArgumentNullException("Utente non puo' essere null.");
            }
            utente = _utente;
        }
    }
}
