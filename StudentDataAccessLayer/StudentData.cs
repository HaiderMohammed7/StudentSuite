using Microsoft.Data.SqlClient;
using StudentDataAccessLayer.Entitys;
using System.Data;

namespace StudentDataAccessLayer
{
    public class StudentData
    {
        private readonly string _connectionString;

        public StudentData(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<StudentEntity> GetAllStudents()
        {
            var StudentsList = new List<StudentEntity>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using(SqlCommand cmd = new SqlCommand("GetAllStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var student = new StudentEntity
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Age = reader.GetInt32(reader.GetOrdinal("Age")),
                                Grade = reader.GetInt32(reader.GetOrdinal("Grade"))
                            };

                            StudentsList.Add(student);
                        }
                    }
                }
            }
            return StudentsList;
        }
        public List<StudentEntity> GetPassedStudents()
        {
            var StudentsList = new List<StudentEntity>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetPassedStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var student = new StudentEntity
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Age = reader.GetInt32(reader.GetOrdinal("Age")),
                                Grade = reader.GetInt32(reader.GetOrdinal("Grade"))
                            };

                            StudentsList.Add(student);
                        }
                    }
                }
            }
            return StudentsList;
        }
        public double GetAverageGrade()
        {
            double averageGrade = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAverageGrade", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    object result = cmd.ExecuteScalar();
                    if(result != DBNull.Value)
                    {
                        averageGrade = Convert.ToDouble(result);
                    }
                    else
                        averageGrade = 0;
                }
            }
            return averageGrade;
        }
        public StudentEntity GetStudentById(int studentId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetStudentById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", studentId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var student = new StudentEntity
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Age = reader.GetInt32(reader.GetOrdinal("Age")),
                                Grade = reader.GetInt32(reader.GetOrdinal("Grade"))
                            };
                            return student;
                        }
                        else
                            return null;
                    }
                }
            }
        }

        public int AddNewStudent(StudentEntity student)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("AddStudent", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Age", student.Age);
                cmd.Parameters.AddWithValue("@Grade", student.Grade);

                var outputIdParam = new SqlParameter("@NewStudentId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputIdParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                return (int)outputIdParam.Value;
            }
            catch
            {
                return -1;
            }
        }

        public bool UpdateStudent(StudentEntity student)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("UpdateStudent", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", student.Id);
                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Age", student.Age);
                cmd.Parameters.AddWithValue("@Grade", student.Grade);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteStudent(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("DeleteStudent", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine($"Rows Deleted: {rowsAffected}");
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete Error: {ex.Message}");
                return false;
            }
        }

    }
}