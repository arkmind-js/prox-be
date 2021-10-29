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
    public class ProductController : ControllerBase
    {
            //  --- JS Change ----

            private readonly IConfiguration _configuration;
            private readonly IWebHostEnvironment _env;

            public ProductController(IConfiguration configuration, IWebHostEnvironment env)
            {
                _configuration = configuration;
                _env = env;
            }


            [HttpGet]
            public JsonResult Get()
            {
                string query = @"
                            select productid, productname, productdesc,productstatus
                            from dbo.ProductMain
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
        public JsonResult Post( Product pr)
            {
                string query = @"
                            insert into dbo.ProductMain
                            (productname, productdesc,productstatus)
                            values (@productname, @productdesc,@productstatus)
                            ";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("NewAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@productname", pr.ProductName);
                        myCommand.Parameters.AddWithValue("@productdesc", pr.ProductDesc);
                        myCommand.Parameters.AddWithValue("@productstatus", pr.ProductStatus);

                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }

                return new JsonResult("Added Successfully");

            }

            [HttpPut]
            public JsonResult Put(Product pr)
            {
                string query = @"
                            update dbo.ProductMain
                            set  = productname = @productname, productdesc = @productdesc
                            , productstatus = @productstatus
                            where productid = @productid
                            ";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("NewAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                    myCommand.Parameters.AddWithValue("@productid", pr.ProductId);
                    myCommand.Parameters.AddWithValue("@productname", pr.ProductName);
                    myCommand.Parameters.AddWithValue("@productdesc", pr.ProductDesc);
                    myCommand.Parameters.AddWithValue("@productstatus", pr.ProductStatus);

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
                            delete from dbo.ProductMain
                            where productid = @productid
                            ";

                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("NewAppCon");
                SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@productid", id);

                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }

                return new JsonResult("Deleted successfully");

            }



        }
    }
