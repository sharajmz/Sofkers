using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class SofkerStadistic
    {
        public string Identification { get; set; }
        public string Name { get; set; }
        public DateTime ChangesDatetime { get; set; }
        public bool? IsSofkerActive { get; set; }
        public string SofkerClient { get; set; }
    }
}
