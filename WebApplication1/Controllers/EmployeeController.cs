using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EmployeeController : ApiController
    {

        public HttpResponseMessage Get()
        {
            string query = @"Select EmployeeID,EmployeeName,Department,convert(varchar(10),DateofJoining,120) DateofJoining,
                               PhotoFileName From dbo.Employee";

            DataTable table = new DataTable();

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }

            return Request.CreateResponse(HttpStatusCode.OK, table);

        }

        public string Post(Employee emp)
        {
            try
            {
                string query = @"Insert into dbo.Employee values 
                                ('" + emp.EmployeeName + @"'
                                 ,'" + emp.Department + @"'
                                 ,'" + emp.DateofJoining + @"'
                                 ,'" + emp.PhotoFileName + @"'
                                )
                                 ";

                DataTable table = new DataTable();

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return "Added Successfully!";
            }

            catch (Exception)
            {
                return "field to Add!";
            }
        }

        public string Put(Employee emp)
        {
            try
            {
                string query = @"Update dbo.Employee set
                                EmployeeName= '" + emp.EmployeeName + @"'
                                ,Department= '" + emp.Department + @"'
                                ,DateofJoining= '" + emp.DateofJoining + @"'
                                ,PhotoFileName= '" + emp.PhotoFileName + @"'
                                    Where EmployeeID= " + emp.EmployeeID + @"";

                DataTable table = new DataTable();

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return "Updated Successfully!";
            }

            catch (Exception)
            {
                return "field to Upate!";
            }
        }


        public string Delete(int id)
        {
            try
            {
                string query = @"Delete From dbo.Employee 
                                    Where EmployeeID= " + id + @"";

                DataTable table = new DataTable();

                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return "Deleted Successfully!";
            }

            catch (Exception)
            {
                return "field to Delete!";
            }
        }

        [Route("api/Employee/GetAllDepatrtementNames")]
        [HttpGet]
        public HttpResponseMessage GetAllDepatrtementNames()
        {
            string query = @"Select DepartmentName From dbo.Department";

            DataTable table = new DataTable();

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeAppDB"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }

            return Request.CreateResponse(HttpStatusCode.OK, table);
        }

        [Route("api/Employee/SaveFile")]
       public string SaveFile()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var phisycalPath = HttpContext.Current.Server.MapPath("~/Photos/" + fileName);
                postedFile.SaveAs(phisycalPath);

                return fileName;
            }
            catch (Exception)
            {
                return "annonymos.png";
            }
      
        }
    }
}
