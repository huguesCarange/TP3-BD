using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.domain.Entities
{
    public class Contrat : Entity
    {
        [ForeignKey("IdGroupe")]
        public Groupe Groupe { get; set; }
        [Required]
        public int IdGroupe { get; set; }
        [ForeignKey("IdClient")]
        public Client Client { get; set; }
        [Required]
        public int IdClient { get; set; }
        [Required]
        public DateTime DateContrat { get; set; }
        [Required]
        public DateTime DatePresentation { get; set; }
        [Required]
        public DateTime HeureDebut { get; set; }
        [Required]
        public DateTime HeureFin { get; set; }
        public int Depot { get; set; }
        public int Cachet { get; set; }
        [Required]
        public int MontantFinal { get; set; }

    }
}
