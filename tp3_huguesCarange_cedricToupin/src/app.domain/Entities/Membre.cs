using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.domain.Entities
{
    public class Membre
    {   
        [Required]
        public int IdArtiste{ get; set; }
        [Required]
        public int IdGroupe { get; set; }
        public Artiste Artiste { get; set; }
        public Groupe Groupe { get; set; }
    }
}
