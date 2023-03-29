using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class IdentificationType
    {
        public IdentificationType()
        {
            Sofkers = new HashSet<Sofker>();
        }

        [Key]
        public int IdentificationId { get; set; }
        public string Identification { get; set; }

        public virtual ICollection<Sofker> Sofkers { get; set; }
    }
}
