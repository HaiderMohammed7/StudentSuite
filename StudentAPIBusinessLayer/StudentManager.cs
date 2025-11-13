using Microsoft.Extensions.Configuration;
using StudentAPIBusinessLayer.DTOs;
using StudentDataAccessLayer;
using StudentDataAccessLayer.Entitys;

namespace StudentAPIBusinessLayer
{
    public class StudentManager
    {
        private readonly StudentData _data;

        public StudentManager(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("StudentDB");
            _data = new StudentData(connectionString);
        }

        public List<StudentDTO> GetAllStudents()
        {
            var Students = _data.GetAllStudents();

            return Students.Select(s => new StudentDTO
            {
                Id = s.Id,
                Name = s.Name,
                Age = s.Age,
                Grade = s.Grade,
            }).ToList();
        }
        public List<StudentDTO> GetPassedStudents()
        {
            var Students = _data.GetPassedStudents();

            return Students.Select(s => new StudentDTO
            {
                Id = s.Id,
                Name = s.Name,
                Age = s.Age,
                Grade = s.Grade,
            }).ToList();
        }
        public double GetAverageGrade()
        {
            return _data.GetAverageGrade();
        }
        public StudentDTO Find(int studentID)
        {
            var student = _data.GetStudentById(studentID);

            if (student == null)
                return null;

            return new StudentDTO
            {
                Id = student.Id,
                Name = student.Name,
                Age = student.Age,
                Grade = student.Grade
            };
        }

        public int AddNewStudent(StudentDTO dto)
        {
            var student = new StudentEntity
            {
                Name = dto.Name,
                Age = dto.Age,
                Grade = dto.Grade,
            };

            return _data.AddNewStudent(student);
        }

        public bool UpdateStudent(StudentDTO dto)
        {
            var student = new StudentEntity
            {
                Id = dto.Id,
                Name = dto.Name,
                Age = dto.Age,
                Grade = dto.Grade
            };

            return _data.UpdateStudent(student);
        }

        public bool DeleteStudent(int id)
        {
            return _data.DeleteStudent(id);
        }

    }
}