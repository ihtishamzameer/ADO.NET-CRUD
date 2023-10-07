using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Data.SqlClient;

namespace WebApplication1.Controllers
{
    public class EmployeeController : Controller
    {
        //const is connection string here
        private const string constr = "Data Source=DESKTOP-MOFBMVJ;Initial Catalog=ihtisahm;Integrated Security=True";
        // GET: Employee
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Employee e)
        {
            using(SqlConnection con=new SqlConnection(constr))//this bloack is used to check for primary key voilation 
            {
                string pidchck = "select * from Employee where Eid=@eid";
                SqlCommand cmd = new SqlCommand(pidchck, con);
                cmd.Parameters.AddWithValue("@eid", e.Eid);
                con.Open();
                SqlDataReader sdr= cmd.ExecuteReader();
                if(!sdr.HasRows)//if primary key not voilated then add the data into db
                {
                    con.Close();
                    string q = "insert into Employee values('" + e.Eid + "','" + e.Ename + "','" + e.Eage + "','" + e.Ecity + "','" + e.Esalary + "','" + e.Edep + "')";
                    SqlCommand cmd1 = new SqlCommand(q, con);
                    // cmd.Parameters.AddWithValue("@Id", id); // Set parameter value

                    con.Open();
                    cmd1.ExecuteNonQuery();
                    ViewBag.greeting = "data succesfully inserted";
                }
                else//otherwise don't add the data into db and show an error msg
                {
                    ViewBag.greeting = "Primary key voilated";
                }

            }
            
            return View();
        }
        public ActionResult AllEmployee()
        {
            List<Employee> list = new List<Employee>();
            Employee e = new Employee();
            using(SqlConnection con= new SqlConnection(constr))
            {
                string q = "select * from Employee";
                SqlCommand cmd = new SqlCommand(q, con);
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while(sdr.Read())
                {
                    e.Eid =Convert.ToInt32(sdr[0]);
                    e.Ename = sdr[1].ToString();
                    e.Eage = Convert.ToInt32(sdr[2]);
                    e.Ecity = sdr[3].ToString();
                    e.Esalary = Convert.ToInt32(sdr[4]);
                    e.Edep = sdr[5].ToString();
                    list.Add(e);
                    e = new Employee();
                }
                sdr.Close();
            }
            return View(list);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Employee e = new Employee();
            using (SqlConnection con = new SqlConnection(constr))
            {
                string q = "select * from Employee where EId = @id";
                SqlCommand cmd = new SqlCommand(q,con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
               using( SqlDataReader sdr = cmd.ExecuteReader())
               {
                    while(sdr.Read())
                    {
                        e.Eid = Convert.ToInt32(sdr[0]);
                        e.Ename = sdr[1].ToString();
                        e.Eage = Convert.ToInt32(sdr[2]);
                        e.Ecity = sdr[3].ToString();
                        e.Esalary = Convert.ToInt32(sdr[4]);
                        e.Edep = sdr[5].ToString();
                    }
               }
            }
                return View(e);
        }
        public ActionResult Edit(Employee e)
        {
            string q = "update Employee set Eid=@id,EName=@name,EAGE=@age,ECity=@city,ESalary=@sal,EDepart=@dep where Eid=@id";
            using(SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@id", e.Eid);
                    cmd.Parameters.AddWithValue("@name", e.Ename);
                    cmd.Parameters.AddWithValue("@city", e.Ecity);
                    cmd.Parameters.AddWithValue("@dep", e.Edep);
                    cmd.Parameters.AddWithValue("@sal", e.Esalary);
                    cmd.Parameters.AddWithValue("@age", e.Eage);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ViewBag.updatemessage = "Data Updated Succesfully--!";
                }

            }

            //return RedirectToAction("AllEmployee");
            return View();
        }
        public ActionResult Details(int id)
        {
            string q = "select * from Employee where EId=@id";
            Employee e = new Employee();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using(SqlCommand cmd = new SqlCommand(q,con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while(sdr.Read())
                    {
                        e.Eid = Convert.ToInt32(sdr[0]);
                        e.Ename = sdr[1].ToString();
                        e.Eage=Convert.ToInt32(sdr[2]);
                        e.Ecity = sdr[3].ToString();
                        e.Esalary = Convert.ToInt32(sdr[4]);
                        e.Edep = sdr[5].ToString();
                    }
                }

            }
                return View(e);
        }
        public ActionResult Delete(int id)
        {
            string q = "DELETE FROM Employee WHERE Eid = @Id"; // Use parameterized query
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.Parameters.AddWithValue("@Id", id); // Set parameter value

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("AllEmployee");
        }
    }
}