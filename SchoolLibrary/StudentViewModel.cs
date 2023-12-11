using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using StudentLibrary;

namespace StudentLibrary
{
    public class StudentViewModel
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        [MaxLength(15)]
        [Display(Name = "First Name")]
        public string StudentFirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Middle Name")]
        [MaxLength(15)]
        [Display(Name = "MiddleName")]
        public string StudentMiddleName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Last Name")]
        [MaxLength(15)]
        [Display(Name = "Last Name")]
        public string StudentLastName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Parent last Name")]
        [MaxLength(15)]
        [Display(Name = " ParentLast Name")]
        public string ParentLastName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Parent First Name")]
        [MaxLength(15)]
        [Display(Name = "ParentFirst Name")]
        public string ParentFirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Address")]
        [MaxLength(100)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please Select Your State")]
        [Display(Name = "State")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "Please Select Your City")]
        [Display(Name = "City")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "Please Enter Your Parent Phone Number")]

        
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")]
        [MaxLength(10)]
        public string ParentNumber { get; set; }

        [Required(ErrorMessage = "Please enter email address")]
        [Display(Name = "Email Address")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage = "Invalid email format")]
        [EmailAddress]
        public string Email { get; set; }
        public List<State> StateItem { get; set; }
        public List<City> CityItem { get; set; }

        [Required(ErrorMessage = "Please select Atleast one hobby")]

        public string StateName { get; set; }
        public string CityName { get; set; }
        public string StartPage { get; set; }
        public string PageLength { get; set; }
        public string PageDraw { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public int RecordsTotal { get; set; }
    }
}
