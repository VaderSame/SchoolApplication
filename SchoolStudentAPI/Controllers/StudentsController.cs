using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentWebAPI;
using StudentLibrary;
using SchoolStudentApi.Models;

namespace SchoolStudentApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class StudentsController : Controller
    {
        private readonly StudentDB_SemalContext _context;

        public StudentsController(StudentDB_SemalContext context)
        {
            _context = context;

        }
        public IActionResult FetchAllStudent(SchoolStudentApiModel studentValue)
        {
            try
            {  
                List<SchoolStudentApiModel> item = new List<SchoolStudentApiModel>();
                int startPage = Convert.ToInt32(studentValue.StartPage);
                int pageLength = Convert.ToInt32(studentValue.PageLength);

                var student = (from s in _context.StudentTables
                               join city in _context.City on s.CityId equals city.CityId
                               join state in _context.State on s.StateId equals state.StateId
                               where (string.IsNullOrEmpty(studentValue.StudentFirstName) || s.StudentFirstName.ToLower().Contains(studentValue.StudentFirstName.ToLower().Trim())) &&
                                              (studentValue.CityId == 0 || s.CityId == studentValue.CityId) &&
                                              (studentValue.StateId == 0 || s.StateId == studentValue.StateId)
                               select new SchoolStudentApiModel
                               {
                                   StudentFirstName = s.StudentFirstName,
                                   StudentLastName = s.StudentLastName,
                                   StudentMiddleName = s.StudentMiddleName,
                                   ParentLastName = s.ParentLastName,
                                   ParentFirstName = s.ParentFirstName,
                                   ParentNumber = s.ParentNumber,
                                   StudentId = s.StudentId,
                                   CityName = city.CityName,
                                   CityId = city.CityId,
                                   StateId = state.StateId,
                                   StateName = state.StateName,
                                   Address = s.Address,
                                   Email = s.Email,
                               });

                string sorting = studentValue.SortOrder + studentValue.SortColumn;
                switch (sorting)
                {
                    case ("desc1"):
                        student = student.OrderByDescending(s => s.StudentFirstName);
                        break;
                    case ("asc1"):
                        student = student.OrderBy(s => s.StudentFirstName);
                        break;
                    case ("desc2"):
                        student = student.OrderBy(s => s.StudentMiddleName);
                        break;
                    case ("asc2"):
                        student = student.OrderByDescending(s => s.StudentMiddleName);
                        break;
                    case ("desc3"):
                        student = student.OrderByDescending(s => s.StudentLastName);
                        break;
                    case ("asc3"):
                        student = student.OrderBy(s => s.StudentLastName);
                        break;
                    case ("desc4"):
                        student = student.OrderBy(s => s.ParentFirstName);
                        break;
                    case ("asc4"):
                        student = student.OrderByDescending(s => s.ParentFirstName);
                        break;
                    case ("desc5"):
                        student = student.OrderByDescending(s => s.ParentLastName);
                        break;
                    case ("asc5"):
                        student = student.OrderBy(s => s.ParentLastName);
                        break;
                    case ("asc6"):
                        student = student.OrderBy(s => s.CityName);
                        break;
                    case ("desc6"):
                        student = student.OrderByDescending(s => s.CityName);
                        break;
                    case ("asc7"):
                        student = student.OrderBy(s => s.StateName);
                        break;
                    case ("desc7"):
                        student = student.OrderByDescending(s => s.StateName);
                        break;
                    case ("asc8"):
                        student = student.OrderBy(s => s.ParentNumber);
                        break;
                    case ("desc8"):
                        student = student.OrderByDescending(s => s.ParentNumber);
                        break;
                    case ("asc9"):
                        student = student.OrderBy(s => s.Email);
                        break;
                    case ("desc9"):
                        student = student.OrderByDescending(s => s.Email);
                        break;
                    case ("asc10"):
                        student = student.OrderBy(s => s.Address);
                        break;
                    case ("desc10"):
                        student = student.OrderByDescending(s => s.Address);
                        break;
                    default:
                        student = student.OrderBy(s => s.StudentFirstName);
                        break;
                }
                int totalRecords = student.Count();
                var studentList = student.Skip(startPage).Take(pageLength).ToList();
                studentList.ForEach(y =>
                {
                    y.RecordsTotal = totalRecords;
                });
                return Ok(studentList);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        private bool StudentExistsWithId(int id)
        {
            return _context.StudentTables.Any(e => e.StudentId == id);
        }
        [HttpGet]
        public List<StudentTables> GetStudents()
        {
            return _context.StudentTables.ToList();
        }

        [HttpGet("{id}")]
        public StudentTables GetStudents(int id)
        {

            StudentTables p = new StudentTables();
            p = _context.StudentTables.FirstOrDefault(x => x.StudentId == id);
            if (p == null)
                throw new Exception("not found");
            return p;
        }

        [HttpPost]
        public IActionResult Post([FromBody] StudentTables student)
        {
            _context.StudentTables.Add(student);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetStudents), new { id = student.StudentId }, student);
        }

        private bool StudentsExists(int id)
        {
            return _context.StudentTables.Any(e => e.StudentId == id);
        }

        [HttpPut("{id}")]
        [Route("PutStudent")]
        public async Task<IActionResult> PutStudent(int id, [FromBody] StudentLibrary.StudentTables product)
        {
            if (id != product.StudentId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExistsWithId(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Route("Delete")]
        public async Task Delete(int id)
        {
            try
            {
                var StudentToDelete = await _context.StudentTables.FindAsync(id);
                _context.StudentTables.Remove(StudentToDelete);
                await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [HttpGet]
        // fetching the States are coming
        public List<State> GetState(string StateName)
        {
            return (from c in this._context.State
                    where c.StateName.StartsWith(StateName) || string.IsNullOrEmpty(StateName)
                    select c).ToList();
        }


        [HttpGet]
        // fetching the cities are coming with refrence of state id
        public List<City> GetCities(int StateId)
        {
            List<City> listCity = new List<City>();
            var getCitylist = (from City in _context.City
                               where City.StateId == StateId
                               select new City
                               {
                                   CityId = City.CityId,
                                   CityName = City.CityName,
                                   StateId = City.StateId

                               }).ToList();

            listCity = getCitylist;
            return listCity;
        }
    }

}
