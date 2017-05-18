using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.domain.Entities
{
    public class Artiste : Entity
    {
        [Required]
        public string Nom { get; set; }
        [Required]
        public string Prenom { get; set; }
        public string Telephone { get; set; }
        [Required]
        //unique
        public string NAS { get; set; }
        public string NomDeScene { get; set; }
        public ICollection<Membre> Membres { get; set; }
    }
}
