using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Http;
using StudentLibrary;

namespace StudentLibrary
{
    public class StudentTables
    {
        [Key]
        public int StudentId { get; set; }
        public string StudentFirstName { get; set; } = null!;
        public string StudentLastName { get; set; } = null!;
        public string StudentMiddleName { get; set; } = null!;
        public string ParentFirstName { get; set; } = null!;
        public string ParentLastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string Address { get; set; } = null!;
        public string ParentNumber { get; set; } 

        [NotMapped]
        public string StateName { get; set; }

        [NotMapped]
        public string CityName { get; set; }
    }

    public class MasterData
    {
        public List<City> City { get; set; }
        public List<State> State { get; set; }


    }
}


