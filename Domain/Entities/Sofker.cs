using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class Sofker
    {
        public int SofkerTypeId { get; set; }
        public string SofkerId { get; set; }
        public string? SofkerName { get; set; }
        public string? SofkerAddress { get; set; }
        public bool? SofkerActive { get; set; }
        public int? SofkerClient { get; set; }

        public virtual Client? SofkerClientNavigation { get; set; }
        public virtual IdentificationType? SofkerType { get; set; }
    }
}
