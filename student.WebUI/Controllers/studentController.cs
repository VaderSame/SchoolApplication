using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System;
using System.Collections.Generic;
using StudentLibrary;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using StudentWebAPI;
using Microsoft.AspNetCore.Authorization;

namespace student.WebUI.Controllers
{
    [Authorize]
    public class studentController : Controller
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        public Uri baseAddress = new Uri("https://localhost:44377/api/Students");
        HttpClient client;

        public studentController(IWebHostEnvironment webHostEnvironment)
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
            hostingEnvironment = webHostEnvironment;
        }

        [Authorize(Roles = "Admin,Teacher,Student")]
        public async Task<IActionResult> Index()
        {
            StudentLibrary.StudentViewModel student = new StudentLibrary.StudentViewModel();
            ViewBag.message = TempData["message"];
            ViewBag.States = GetState(string.Empty);
            return View(student);
        }

        [Authorize(Roles = "Admin,Teacher,Student")]
        [HttpPost]
        public async Task<IActionResult> FetchAllStudent(SchoolStudentApiModel student)
        {
            student.PageDraw = Request.Form["draw"];
            student.StartPage = Request.Form["start"];
            student.PageLength = Request.Form["length"];
            student.SortColumn = Request.Form["order[0][column]"][0];
            student.SortOrder = Request.Form["order[0][dir]"];
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");
                string apiUrl = "https://localhost:44377/api/Students";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(apiUrl);
                using (var response = await client.PostAsync(client.BaseAddress + "/FetchAllStudent", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var studentData = JsonConvert.DeserializeObject<List<SchoolStudentApiModel>>(apiResponse);
                        var studentFirst = studentData.FirstOrDefault();
                        var studentRecords = studentFirst == null ? 0 : studentFirst.RecordsTotal;
                        var jsonData = new { draw = student.PageDraw, recordsFiltered = studentRecords, recordsTotal = studentRecords, data = studentData };
                        return Ok(jsonData);
                    }
                }
                return BadRequest();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        [Authorize(Roles = "Admin,Teacher,Student")]
        [HttpPost]
        public async Task<IActionResult> Search(StudentViewModel student)
        {
            student.PageDraw = Request.Form["draw"];
            student.StartPage = Request.Form["start"];
            student.PageLength = Request.Form["length"];
            student.SortColumn = Request.Form["order[0][column]"][0];
            student.SortOrder = Request.Form["order[0][dir]"];

            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");

                string apiUrl = "https://localhost:44377/api/Students";
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(apiUrl);
                using (var response = await client.PostAsync(client.BaseAddress + "/FetchAllStudent", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var studentData = JsonConvert.DeserializeObject<List<StudentViewModel>>(apiResponse);

                        var studentFirst = studentData.FirstOrDefault();
                        var studentRecords = studentFirst == null ? 0 : studentFirst.RecordsTotal;
                        var jsonData = new { draw = student.PageDraw, recordsFiltered = studentRecords, recordsTotal = studentRecords, data = studentData };
                        return Ok(jsonData);
                    }
                }
                return BadRequest();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //To Get State values in the dropdown
        public static List<StudentLibrary.State> GetState(string StateName)
        {
            List<State> state = new List<State>();

            string apiUrl = "https://localhost:44377/api/Students";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(apiUrl + string.Format("/GetState?StateName={0}", StateName)).Result;
            
            if (response.IsSuccessStatusCode)
            {
                state = JsonConvert.DeserializeObject<List<State>>(response.Content.ReadAsStringAsync().Result);
            }
            return state;
        }
        //To Get City values in the dropdown (JSON) for edit 
        public async Task<JsonResult> GetCities(int StateId)
        {
            try
            {
                List<City> city = new List<City>();

                HttpResponseMessage response = client.GetAsync(baseAddress + string.Format("/GetCities?StateId={0}", StateId)).Result;
                if (response.IsSuccessStatusCode)
                {
                    city = JsonConvert.DeserializeObject<List<StudentLibrary.City>>(response.Content.ReadAsStringAsync().Result);
                }
                return Json(city);
            }
            catch (HttpRequestException exception)
            {
                throw exception;
            }
        }

        //To Get City values in the dropdown (List) for create
        public static List<StudentLibrary.City> GetCit(int StateId)
        {
            try
            {
                List<City> city = new List<City>();
                string apiUrl = "https://localhost:44377/api/Students";
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(apiUrl + string.Format("/GetCities?StateId={0}", StateId)).Result;
                if (response.IsSuccessStatusCode)
                {
                    city = JsonConvert.DeserializeObject<List<StudentLibrary.City>>(response.Content.ReadAsStringAsync().Result);
                }
                return city;
            }
            catch (HttpRequestException exception)
            {
                throw exception;
            }
        }



        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Create()
        {
            ViewBag.title = "Add Student";
            ViewBag.button = "Submit";
            ViewBag.action = "Create";
            StudentViewModel studentViewModel = new StudentViewModel();
            ViewBag.States = GetState(string.Empty);

            return View(studentViewModel);
        }

        private string ProcessUploadedFile(StudentViewModel model)
        {
            string uniqueFileName = null;
            return uniqueFileName;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] StudentViewModel model)
        {
            StudentTables student = new StudentTables();
            student.StudentFirstName = model.StudentFirstName;
            student.StudentMiddleName = model.StudentMiddleName;
            student.StudentLastName = model.StudentLastName;
            student.Address = model.Address;
            student.StateId = model.StateId;
            student.CityId = model.CityId;
            student.ParentNumber = model.ParentNumber;
            student.ParentFirstName = model.ParentFirstName;
            student.ParentLastName = model.ParentLastName;
            student.Email = model.Email;

            ViewBag.messages = "AddData";

            string data = JsonConvert.SerializeObject(student);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
           
           
            HttpResponseMessage Response = client.PostAsync(client.BaseAddress + "/Post", content).Result;
            TempData["message"] = "Added Student Successfully";
            if (Response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            StudentViewModel model = new StudentViewModel();

            ViewBag.title = "Edit Student";
            ViewBag.button = "Update";
            ViewBag.action = "Edit";
            ViewBag.States = GetState(string.Empty);
            HttpResponseMessage Response = client.GetAsync(client.BaseAddress + "/GetStudents/" + id).Result;
            if (Response.IsSuccessStatusCode)
            {
                string data = Response.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<StudentViewModel>(data);
            }
            ViewBag.Cityy = model.StateId > 0 ? GetCit(model.StateId) : null;
            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StudentViewModel model)
        {
            StudentTables student = new StudentTables();

            student.StudentId = model.StudentId;
            student.StudentFirstName = model.StudentFirstName;
            student.StudentMiddleName = model.StudentMiddleName;
            student.StudentLastName = model.StudentLastName;
            student.Address = model.Address;
            student.StateId = model.StateId;
            student.CityId = model.CityId;
            student.ParentNumber = model.ParentNumber;
            student.ParentFirstName = model.ParentFirstName;
            student.ParentLastName = model.ParentLastName;
            student.Email = model.Email;
            StringContent content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");
            
            HttpResponseMessage Response = client.PutAsync(client.BaseAddress + "/PutStudent/" + model.StudentId, content).Result;
            TempData["message"] = "Updated Student Successfully";
            if (Response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            TempData["message"] = "Updated Student Successfully";
            return RedirectToAction("Index");
        }

        // Delete the students records 
        [Authorize(Roles = "Teacher")]
        public async Task<bool> Delete(int id)
        {
            try
            {   
                HttpResponseMessage Response = client.DeleteAsync(client.BaseAddress + "/Delete/" + id).Result;

                Response.EnsureSuccessStatusCode();
                TempData["message"] = "Student Deleted Successfully";
                if (Response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

    }
}