using System;
using StudentLibrary;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentWebAPI
{
    public class SchoolStudentApiModel
    {
        public int StudentId { get; set; }
        public int[] hobbieslist { get; set; }
        public int Id { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string StudentMiddleName { get; set; }
        public string ParentFirstName { get; set; }
        public string ParentLastName { get; set; }
        public string Gender { get; set; }
        [NotMapped]
        public string HobbiesId { get; set; }
        public string Address { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string StateName { get; set; }
        public string ParentNumber { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ImageData { get; set; }
        public string CityName { get; set; }
        public virtual City City { get; set; }
        public virtual State State { get; set; }

        public string StartPage { get; set; }
        public string PageLength { get; set; }
        public string PageDraw { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public int RecordsTotal { get; set; }
    }
}
