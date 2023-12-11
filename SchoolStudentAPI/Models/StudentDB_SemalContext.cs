using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using StudentLibrary;
using SchoolStudentApi;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SchoolStudentApi.Models
{
    public partial class StudentDB_SemalContext : IdentityDbContext
    {
       

        public StudentDB_SemalContext()
        {
        }

        public StudentDB_SemalContext(DbContextOptions<StudentDB_SemalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> City { get; set; } = null!;

        public virtual DbSet<StaffTable> StaffTables { get; set; } = null!;
        public virtual DbSet<State> State { get; set; } = null!;

        public virtual DbSet<StudentTables> StudentTables { get; set; } = null!;





    }
}