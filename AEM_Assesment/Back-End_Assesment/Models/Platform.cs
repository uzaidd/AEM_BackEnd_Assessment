using System;
using System.Collections.Generic;

namespace Back_End_Assesment.Models
{
    public partial class Platform
    {
        public Platform()
        {
            Well = new HashSet<Well>();
        }

        public int Id { get; set; }
        public string? UniqueName { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Well> Well { get; set; }
    }
}
