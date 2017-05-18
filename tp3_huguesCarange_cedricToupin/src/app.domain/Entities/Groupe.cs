using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.domain.Entities
{
    public class Groupe : Entity
    {
        public string Nom { get; set; }
        [Required]
        public int Cachet { get; set; }
        public ICollection<Membre> Membres { get; set; }
        public ICollection<Contrat> Contrats { get; set; }
    }
}
