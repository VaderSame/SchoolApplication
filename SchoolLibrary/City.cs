using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace StudentLibrary
{
    public class City
    {
        [Key]
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int? StateId { get; set; }

        [NotMapped]
        public virtual State State { get; set; }
        [NotMapped]
        public virtual ICollection<StudentTables> StudentTables { get; set; }
    }
}
