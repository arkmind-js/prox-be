using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using sample1.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace sample1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerController : ControllerBase
    {
            //  --- JS Change ----

            private readonly IConfiguration _configuration;
            private readonly IWebHostEnvironment _env;

        public DealerController(IConfiguration configuration,IWebHostEnvironment env)
            {
                _configuration = configuration;
                _env = env;
            }


            [HttpGet]
            public JsonResult Get()
            {
                string query = @"
                            select dealer_id, dealer_name,department,
                            convert(varchar(10),DateOfJoining,120) as DateOfJoining,PhotoFileName
                            from
                            dbo.Dealer
                            ";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("NewAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }

                return new JsonResult(table);

            }

            [HttpPost]
            public JsonResult Post(Dealer dl)
            {
                string query = @"
                            insert into dbo.Dealer
                            (dealer_name,department,DateOfJoining,PhotoFileName)
                            values (@dealer_name,@department,@DateOfJoining,@PhotoFileName)
                            ";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("NewAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@dealer_name", dl.DealerName);
                        myCommand.Parameters.AddWithValue("@department", dl.Department);
                        myCommand.Parameters.AddWithValue("@DateOfJoining", dl.DateOfJoining);
                        myCommand.Parameters.AddWithValue("@PhotoFileName", dl.PhotoFileName);

                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }

                return new JsonResult("Added Successfully");

            }

            [HttpPut]
            public JsonResult Put(Dealer dl)
            {
                string query = @"
                            update dbo.Dealer
                            set dealer_name = @dealer_name, department = @department,
                            DateOfJoining = @DateOfJoining, PhotoFileName = @PhotoFileName
                            where dealer_id = @dealer_id
                            ";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("NewAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                    myCommand.Parameters.AddWithValue("@dealer_id", dl.DealerId);
                    myCommand.Parameters.AddWithValue("@dealer_name", dl.DealerName);
                    myCommand.Parameters.AddWithValue("@department", dl.Department);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", dl.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", dl.PhotoFileName);

                    myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }

                return new JsonResult("Updated Successfully");

            }
            [HttpDelete("{id}")]
            public JsonResult Delete(int id)
            {
                string query = @"
                            delete from dbo.Dealer
                            where dealer_id = @dealer_id
                            ";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("NewAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@dealer_id", id);

                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }

                return new JsonResult("Deleted successfully");

            }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream=new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(filename);
            }
            catch(Exception)
            {

                return new JsonResult("empty.png");
            }
        }


        }
}
