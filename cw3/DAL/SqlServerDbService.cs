using cw3.DTOs.Requests;
using cw3.DTOs.Responses;
using cw3.Models;
using System;
using System.Data.SqlClient;


namespace cw3.DAL
{
    public class SqlServerDbService : IStudentsDbService
    {

        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest request)
        {
            var st = new Student();
            st.IndexNumber = request.IndexNumber;
            st.FirstName = request.FirstName;
            st.LastName = request.LastName;
            st.Birthdate = request.Birthdate;
            var response = new EnrollStudentResponse();
            bool success;
            //int success;
            DateTime date = DateTime.Today;
            const string ConString = "Data Source=db-mssql;Initial Catalog=s19391;Integrated Security=True";

            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;

                con.Open();
                var tsn = con.BeginTransaction();

                com.CommandText = "SELECT IdStudy FROM studies WHERE name = @studies";
                com.Parameters.AddWithValue("studies", request.Studies);
                com.Transaction = tsn;
                var dr = com.ExecuteScalar();

                if (dr == null)
                {
                    tsn.Rollback();
                    success = false;
                }

                int idStudy = (int)dr;
                int idEnroll;
                int idEnrollMax;
                com.CommandText = "SELECT MAX(IdEnrollment) FROM Enrollment";
                com.Transaction = tsn;
                dr = com.ExecuteScalar();
                idEnrollMax = (int)dr;

                com.CommandText = "SELECT IdEnrollment FROM Enrollment WHERE Semester = 1 AND IdStudy = @idstudy";
                com.Parameters.AddWithValue("idstudy", idStudy);
                com.Transaction = tsn;
                dr = com.ExecuteScalar();

                if (dr == null)
                {
                    idEnroll = idEnrollMax + 1;
                    DateTime start = DateTime.Today;

                    com.CommandText = "INSERT INTO Enrollment(IdEnrollment, Semester, IdStudy, StartDate) VALUES (@idenroll, 1, @idstudy, @date)";
                    com.Parameters.AddWithValue("idenroll", idEnroll);
                    com.Parameters.AddWithValue("date", start);
                    com.Transaction = tsn;
                    com.ExecuteNonQuery();
                }
                else
                {
                    idEnroll = (int)dr;
                }

                com.CommandText = "SELECT lastname FROM student WHERE IndexNumber = @index";
                com.Parameters.AddWithValue("index", st.IndexNumber);
                com.Transaction = tsn;
                dr = com.ExecuteScalar();


                if (dr != null)
                {
                    tsn.Rollback();
                    success = false;
                }

                com.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) VALUES(@index, @fn, @ln, @birth, @idenroll2)";
                com.Parameters.AddWithValue("fn", st.FirstName);
                com.Parameters.AddWithValue("ln", st.LastName);
                com.Parameters.AddWithValue("birth", st.Birthdate);
                com.Parameters.AddWithValue("idenroll2", idEnroll);
                com.Transaction = tsn;
                com.ExecuteNonQuery();

                response.IndexNumber = st.IndexNumber;
                response.IdEnrollment = idEnroll;
                response.Semester = 1;
                response.Studies = request.Studies;
                response.StartDate = date;

                tsn.Commit();
                success = true;

            }
            if (success)
            {
                return response;
            }
            else
            {
                return null;
            }
     }

        public PromoteStudentsResponse PromoteStudents(PromoteStudentsRequest request)
        {

            var response = new PromoteStudentsResponse();
            DateTime date = DateTime.Today;
            int success;
            const string ConString = "Data Source=db-mssql;Initial Catalog=s19391;Integrated Security=True";

            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;

                con.Open();
                var tsn = con.BeginTransaction();

                com.CommandText = "EXECUTE PromoteStudents @study, @semester";
                com.Parameters.AddWithValue("study", request.Studies);
                com.Parameters.AddWithValue("semester", request.Semester);
                com.Transaction = tsn;
                success = com.ExecuteNonQuery();

                var nextSemestr = request.Semester + 1;
                com.CommandText = "SELECT IdEnrollment FROM enrollment WHERE semester = @nextSemester AND idstudy = (SELECT idstudy FROM studies WHERE name = @study)";
                com.Parameters.AddWithValue("nextSemester", nextSemestr);
                com.Transaction = tsn;
                var dr = com.ExecuteScalar();

                response.IdEnrollment =(int) dr;

                com.CommandText = "SELECT StartDate FROM enrollment WHERE semester = @nextSemester AND idstudy = (SELECT idstudy FROM studies WHERE name = @study)";
                com.Transaction = tsn;
                dr = com.ExecuteScalar();
                response.StartDate = (DateTime) dr;

                tsn.Commit();

                response.Studies = request.Studies;
                response.Semester = request.Semester + 1;

                
                 if (success>0)
                {
                    return response;
                }
                else
                {
                    return null;
                }
                
             }
        }

        public Student GetStudent(string IndexNumer)
        {
            const string ConString = "Data Source=db-mssql;Initial Catalog=s19391;Integrated Security=True";
            var stud = new Student();

            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;

                con.Open();

                com.CommandText = "SELECT * FROM Student WHERE IndexNumber = @index";
                com.Parameters.AddWithValue("index", IndexNumer);
                var dr = com.ExecuteReader();

                if (!dr.Read())
                {
                    return null;
                }
                else
                {
                    while (dr.Read())
                    {
                        stud.IndexNumber = dr["IndexNumber"].ToString();
                        stud.FirstName = dr["FirstName"].ToString();
                        stud.LastName = dr["LastName"].ToString();
                        stud.Birthdate = (DateTime)dr["BirthDate"];
                    }

                    return stud;
                }
                
            }
        }

         public LoginResponse Login(LoginRequest request)
        {
            const string ConString = "Data Source=db-mssql;Initial Catalog=s19391;Integrated Security=True";
            string Login = request.Login;
            string Password = request.Password;
            LoginResponse response = new LoginResponse();

            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();

                com.CommandText = "SELECT * FROM student WHERE IndexNumber = @index;";
                com.Parameters.AddWithValue("index", request.Login);
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    Login = dr["IndexNumber"].ToString();
                    Password = dr["Password"].ToString();
                }
            }

            if (request.Login == Login && request.Password == Password)
            {
                response.Login = Login;
                response.Password = Password;

                return response;
            }
            else
            {
                return null;
            }          
        }
     }
}
