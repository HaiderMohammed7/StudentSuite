using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudentAPIBusinessLayer;
using StudentAPIBusinessLayer.DTOs;

namespace StudentAPI.Controllers
{
    [Route("api/Students")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly StudentManager _manager;

        public StudentAPIController(IConfiguration configuration)
        {
            _manager = new StudentManager(configuration);
        }



        [HttpGet("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetAllStudents()
        {
            List<StudentDTO> studentsList = _manager.GetAllStudents();
            if (studentsList.Count == 0)
            {
                return NotFound("No Students Found!");
            }
            return Ok(studentsList);
        }



        [HttpGet("Passed", Name = "GetPassedStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetPassedStudents()
        {
            List<StudentDTO> studentsList = _manager.GetPassedStudents();
            if (studentsList.Count == 0)
            {
                return NotFound("No Students Found!");
            }
            return Ok(studentsList);
        }



        [HttpGet("AverageGrade", Name = "GetAverageGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<double> GetAverageGrade()
        {
            var averageGrade = _manager.GetAverageGrade();
            if (averageGrade < 0)
                return NotFound("Invalid!");

            return Ok(averageGrade);
        }



        [HttpGet("{StudentId}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentDTO> GetStudentById(int StudentId)
        {
            if (StudentId < 1)
            {
                return BadRequest($"Not accepted Id : {StudentId}");
            }

            var student = _manager.Find(StudentId);
            if (student == null)
            {
                return NotFound($"Student with id {StudentId} not found.");
            }
            return Ok(student);
        }



        [HttpPost(Name = "AddStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> AddNewStudent(StudentDTO newStudent)
        {
            if (newStudent == null || string.IsNullOrEmpty(newStudent.Name) || newStudent.Age <= 0)
                return BadRequest("Invalid student data");

            int newId = _manager.AddNewStudent(newStudent);

            if (newId == -1)
                return StatusCode(500, "Failed to add student");

            newStudent.Id = newId;

            return CreatedAtRoute("GetStudentById", new { studentId = newId }, newStudent);
        }




        [HttpDelete("{StudentId}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteStudent(int StudentId)
        {
            if (StudentId < 1)
            {
                return BadRequest($"Not accepted Id : {StudentId}");
            }

            bool success = _manager.DeleteStudent(StudentId);

            if (!success)
                return NotFound($"StudentId with Id : {StudentId} not Found !");

            return Ok($"Student with Id : {StudentId} has been deleted.");
        }



        [HttpPut("{StudentId}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateStudent(int StudentId, StudentDTO updatedStudent)
        {
            if (StudentId < 1 || updatedStudent == null || string.IsNullOrEmpty(updatedStudent.Name) || updatedStudent.Age < 0 || updatedStudent.Grade < 0)
            {
                return BadRequest("Invalid Student data");
            }

            updatedStudent.Id = StudentId;

            bool success = _manager.UpdateStudent(updatedStudent);

            if (!success)
                return NotFound($"Student with ID {StudentId} not found.");

            return Ok(updatedStudent);
        }



        [HttpPost("UploadImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No File uploaded");

            string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            using (var FileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(FileStream);
            }

            string imageUrl = $"{Request.Scheme}://{Request.Host}/Images/{uniqueFileName}";

            return Ok(new { FileName = uniqueFileName, Url = imageUrl });
        }



        [HttpGet("GetImage/{imageName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetImage(string imageName)
        {
            string imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", imageName);

            if(!System.IO.File.Exists(imagepath))
                return NotFound("Image Not Found");

            var imageBytes = System.IO.File.ReadAllBytes(imagepath);
            var contentType = GetContentType(imagepath);

            return File(imageBytes, contentType);
        }
        private string GetContentType(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }


        // Version 1.1.0 - Added minor update comment
        // login to the database
    }
}