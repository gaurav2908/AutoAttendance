using AutoAttendance.Models;
using ClosedXML.Excel;
using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AutoAttendance.Controllers
{
    public class StudentController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Student
        public ActionResult Index(string className, string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                IEnumerable<Student> students = null;
                if(className == null)
                {
                    className = ViewBag.className;
                }
                else
                {
                    ViewBag.className = className;
                }
                if (className == null)
                {
                    ViewBag.Message = "Since no class is selected, displaying all class students!!";
                    //ViewBag.className = className;
                    students = db.Students.Where(student => (student.ApplicationUserId == User.Identity.Name));
                }
                else
                {
                    students = db.Students.Where(student => (student.ApplicationUserId == User.Identity.Name && student.ClassName == className));
                }

                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.idSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";

                switch (sortOrder)
                {
                    case "name_desc":
                        students = students.OrderByDescending(s => s.Name);
                        break;
                    case "id_desc":
                        students = students.OrderBy(s => s.Id);
                        break;
                    default:
                        students = students.OrderBy(s => s.Name);
                        break;
                }

                return View(students.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Student/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Student/Create
        public ActionResult Create(string ClassName)
        {
            if (User.Identity.IsAuthenticated)
            {
                if(ClassName != null)
                {
                    ViewBag.className = ClassName;
                }

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Student/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "ClassName,Id,Name,ApplicationUserId")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    student.ApplicationUserId = User.Identity.Name;

                    object[] queryParamsStudent = new object[3];
                    queryParamsStudent[0] = User.Identity.Name;
                    queryParamsStudent[1] = student.ClassName;
                    queryParamsStudent[2] = student.Id;
                    Student studentExisting = db.Students.Find(queryParamsStudent);

                    if (studentExisting != null)
                    {
                        ViewBag.Message = "This Student already exist in same class!!";
                        return View(student);
                    }
                    else
                    {
                        db.Students.Add(student);
                        db.SaveChanges();
                    }

                    object[] queryParams = new object[2];
                    queryParams[0] = User.Identity.Name;
                    queryParams[1] = student.ClassName;
                    
                    ClassName className = db.ClassNames.Find(queryParams);
                    if(className == null)
                    {
                        ClassName newClass = new ClassName();
                        newClass.ApplicationUserId = User.Identity.Name;
                        newClass.CName = student.ClassName;

                        db.ClassNames.Add(newClass);
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index", new { className = student.ClassName });
                    
                }
                return View(student);
            }
            catch
            {
                return View(student);
            }
        }

        // GET: Student/Edit/5
        public ActionResult Edit(string id, string className)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            object[] queryParams = new object[3];
            queryParams[0] = User.Identity.Name;
            queryParams[1] = className;
            queryParams[2] = id;
            Student student = db.Students.Find(queryParams);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ClassName,Id,Name")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.ApplicationUserId = User.Identity.Name;
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { className = student.ClassName });
            }
            return View(student);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(string id, string className)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            object[] queryParams = new object[3];
            queryParams[0] = User.Identity.Name;
            queryParams[1] = className;
            queryParams[2] = id;

            Student student = db.Students.Find(queryParams);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string className)
        {
            object[] queryParams = new object[3];
            queryParams[0] = User.Identity.Name;
            queryParams[1] = className;
            queryParams[2] = id;

            Student student = db.Students.Find(queryParams);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index", new { className = student.ClassName });
        }

        // GET: Student
        public ActionResult Attendance(string className, string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                
                IEnumerable<Student> students = null;
                if (className == null)
                {
                    className = ViewBag.className;
                }
                else
                {
                    ViewBag.className = className;
                }
                if (className == null)
                {
                    ViewBag.Message = "Since no class is selected, displaying all class students!!";
                    students = db.Students.Where(student => (student.ApplicationUserId == User.Identity.Name)).ToList();
                }
                else
                {
                    students = db.Students.Where(student => (student.ApplicationUserId == User.Identity.Name && student.ClassName == className)).ToList();
                }

                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.idSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";

                switch (sortOrder)
                {
                    case "name_desc":
                        students = students.OrderByDescending(s => s.Name);
                        break;
                    case "id_desc":
                        students = students.OrderBy(s => s.Id);
                        break;
                    default:
                        students = students.OrderBy(s => s.Name);
                        break;
                }
                
                return View(students.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Attendance(Student students, string[] Present, string submitButton, string speech)
        {

            GridView gridview = new GridView();

            //List<Student> studentList = db.Students.Where(student => student.ApplicationUserId == User.Identity.Name && student.ClassName == students.ClassName).ToList();

            var studentList = (from d in db.Students
                               where d.ApplicationUserId == User.Identity.Name && d.ClassName == students.ClassName
                               select new
                               {
                                   Id = d.Id,
                                   Name = d.Name,
                                   Present = d.present
                               }).ToList();

            List<StudentExcel> studentExcel = new List<StudentExcel>();

            foreach (var student in studentList)
            {
                StudentExcel stud = new StudentExcel();
                stud.Id = student.Id;
                stud.Name = student.Name;
                int pos = -2;
                if (Present != null)
                {
                    pos = Array.IndexOf(Present, student.Id);
                }
                if (pos > -1)
                {
                    stud.Present = "P";
                }
                else
                {
                    stud.Present = "A";
                }
                studentExcel.Add(stud);
            }

            DateTime today = DateTime.Today;
            string s_today = today.ToString("MM-dd-yyyy");
            string fileName = "Attendance_" + students.ClassName + "_" + s_today;

            switch (submitButton)
            {
                case "Download":

                    XLWorkbook wb = new XLWorkbook();
                    var ws = wb.Worksheets.Add(fileName);
                    ws.Cell(2, 1).InsertTable(studentExcel);
                    ws.Columns().AdjustToContents();
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xlsx", fileName.Replace(" ", "_")));

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        wb.SaveAs(memoryStream);
                        memoryStream.WriteTo(Response.OutputStream);
                        memoryStream.Close();
                    }

                    Response.End();
                    ViewBag.ActionMessage = "File Downloaded Successfully";
                    break;

                case "Email":
                    string from = "autoattendanceapp@gmail.com";
                    string to = User.Identity.Name;
                    string subject = "Attendance - class " + students.ClassName;
                    string body = "Please find the attandance sheet for class " + students.ClassName + " for date " + s_today + ".";
                        /////////////////////

                        DataTable table = new DataTable();
                        using (var reader = ObjectReader.Create(studentExcel))
                        {
                            table.Load(reader);
                        }

                        //Set DataTable Name which will be the name of Excel Sheet.
                        table.TableName = "Attendance";

                        //Create a New Workbook.
                        using (XLWorkbook wb2 = new XLWorkbook())
                        {
                            //Add the DataTable as Excel Worksheet.
                            wb2.Worksheets.Add(table);

                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                //Save the Excel Workbook to MemoryStream.
                                wb2.SaveAs(memoryStream);

                                //Convert MemoryStream to Byte array.
                                byte[] bytes = memoryStream.ToArray();
                                memoryStream.Close();

                                //Send Email with Excel attachment.
                                using (MailMessage mm = new MailMessage(from, to))
                                {
                                    mm.Subject = subject;
                                    mm.Body = body;

                                    //Add Byte array as Attachment.
                                    mm.Attachments.Add(new Attachment(new MemoryStream(bytes), fileName + ".xlsx"));
                                    mm.IsBodyHtml = true;
                                    SmtpClient smtp = new SmtpClient();
                                    smtp.Host = "smtp.gmail.com";
                                    smtp.EnableSsl = true;
                                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();
                                    credentials.UserName = from;
                                    credentials.Password = "String@2908";
                                    smtp.UseDefaultCredentials = true;
                                    smtp.Credentials = credentials;
                                    smtp.Port = 587;
                                    smtp.Send(mm);
                                }
                            }
                        }

                    ViewBag.className = students.ClassName;
                    ViewBag.ActionMessage = "Email Sent";
                    break;
            }

            //List<Student> studentListPost = db.Students.Where(student => student.ApplicationUserId == User.Identity.Name && student.ClassName == students.ClassName).ToList();
            //foreach (var student in studentListPost)
            //{
            //    //StudentExcel stud = new StudentExcel();
            //    //stud.Id = student.Id;
            //    //stud.Name = student.Name;
            //    int pos = -2;
            //    if (Present != null)
            //    {
            //        pos = Array.IndexOf(Present, student.Id);
            //    }
            //    if (pos > -1)
            //    {
            //        student.present = student.Id;
            //    }
            //    else
            //    {
            //        student.present = "";
            //    }
            //}

            return View(db.Students.Where(student => (student.ApplicationUserId == User.Identity.Name && student.ClassName == students.ClassName)).ToList());

        }

    }
}
