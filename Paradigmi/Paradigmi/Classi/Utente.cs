using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Paradigmi.Classi
{
    [Table("Utenti")]
    public class Utente
    {
        [Key]
        private int id;

        [Required]
        [EmailAddress]
        private string email;

        [Required]
        private string nome;

        [Required]
        private string cognome;

        [Required]
        private string password;

        private ICollection<Booking> bookings;


        public int getId() { return this.id; }
        public void setId(int _id) { if (_id >= 0) this.id = _id; }

        public string getEmail() { return this.email; }
        public void setEmail(string _email) { if (!String.IsNullOrEmpty(_email)) this.email = _email; }

        public string getNome() { return this.nome; }
        public void setNome(string _nome) { if (!String.IsNullOrEmpty(_nome)) this.nome = _nome; }

        public string getcognome() { return this.cognome; }
        public void setcognome(string _cognome) { if (!String.IsNullOrEmpty(_cognome)) this.cognome = _cognome; }

        public string getPassword() { return this.password; }
        public void setPassword(string _password) { if (!String.IsNullOrEmpty(_password)) this.password = _password; }

        public ICollection<Booking> getBookings() 
        {
            return bookings;
        }

        public Utente() 
        {
           
        }
        public Utente(String _nome, String _cognome, String _email, String _password) 
        {
            setNome(_nome);
            setcognome(_cognome);
            setEmail(_email);
            setPassword(_password);
        }
    }
}
