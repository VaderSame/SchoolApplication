using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StudentLibrary
{
    public class State
    {

        [Key]
        public int StateId { get; set; }
        public string StateName { get; set; }

        [NotMapped]
        public virtual ICollection<City> City { get; set; }
    }
}
