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

namespace sample1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewAppController : ControllerBase
    {
        //  --- JS Change ----

        private readonly IConfiguration _configuration;
        public NewAppController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select new_id, new_name,new_age,new_state,new_product,new_datetime from
                            dbo.NewApp
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("NewAppCon");
            SqlDataReader myReader;
            using(SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand=new SqlCommand(query,myCon))
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
        public JsonResult Post(Newapp nap)
        {
            string query = @"
                            insert into dbo.NewApp
                            (new_name,new_age,new_state,new_product)
                            values (@new_name,@new_age,@new_state,@new_product)
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("NewAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@new_name", nap.NewappName);
                    myCommand.Parameters.AddWithValue("@new_age", nap.NewappAge);
                    myCommand.Parameters.AddWithValue("@new_state", nap.NewappState);
                    myCommand.Parameters.AddWithValue("@new_product", nap.NewappProduct);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully");

        }

        [HttpPut]
        public JsonResult Put(Newapp nap)
        {
            string query = @"
                            update dbo.NewApp
                            set new_name = @new_name, new_age = @new_age,
                            new_state = @new_state, new_product = @new_product
                            where new_id = @new_id
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("NewAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@new_id", nap.NewappId);
                    myCommand.Parameters.AddWithValue("@new_name", nap.NewappName);
                    myCommand.Parameters.AddWithValue("@new_age", nap.NewappAge);
                    myCommand.Parameters.AddWithValue("@new_state", nap.NewappState);
                    myCommand.Parameters.AddWithValue("@new_product", nap.NewappProduct);

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
                            delete from dbo.NewApp
                            where new_id = @new_id
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("NewAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@new_id", id);
                   
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
