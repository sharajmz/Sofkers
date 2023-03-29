using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class Client
    {
        public Client()
        {
            Sofkers = new HashSet<Sofker>();
        }

        public int ClientId { get; set; }
        public string? ClientName { get; set; }

        public virtual ICollection<Sofker> Sofkers { get; set; }
    }
}
