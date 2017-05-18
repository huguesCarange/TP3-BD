using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.domain.Entities
{
    public class Facture : Entity
    {
        [Required]
        public DateTime DateProduction { get; set; }
        public DateTime DatePaiement { get; set; }
        public string ModePaiement { get; set; }
        [ForeignKey("IdContrat")]
        public Contrat Contrat { get; set; }
        [Required]
        public int IdContrat { get; set; }
    }
}
