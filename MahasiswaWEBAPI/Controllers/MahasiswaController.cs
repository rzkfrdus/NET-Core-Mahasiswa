using MahasiswaWEBAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MahasiswaWEBAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class MahasiswaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public MahasiswaController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet] // menampilkan seluruh data mhs
        [Route("mahasiswa")]
        public JsonResult Get()
        {
            string query = @"select * from mhs";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("mahasiswaConn");
            MySqlDataReader myReader;
            using (MySqlConnection myconn = new MySqlConnection(sqlDataSource))
            {
                myconn.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myconn))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myconn.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPut] // update data mhs
        [Route("mahasiswa")]
        public JsonResult Put(Mahasiswa mhs)
        {
            string query = @"update mhs set Nama=@Nama, Fakultas=@Fakultas, Jurusan=@Jurusan, PhotoFile=@PhotoFile
                            where Mhs=@Mhs";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("mahasiswaConn");
            MySqlDataReader myReader;
            using (MySqlConnection myconn = new MySqlConnection(sqlDataSource))
            {
                myconn.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myconn))
                {
                    myCommand.Parameters.AddWithValue("@Mhs", mhs.Mhs);
                    myCommand.Parameters.AddWithValue("@Nama", mhs.Nama);
                    myCommand.Parameters.AddWithValue("@Fakultas", mhs.Fakultas);
                    myCommand.Parameters.AddWithValue("@Jurusan", mhs.Jurusan);
                    myCommand.Parameters.AddWithValue("@Photofile", mhs.Photofile);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myconn.Close();
                }
            }
            return new JsonResult("Berhasil Update Data");
        }

        [HttpPost] // menambahkan data mhs
        [Route("mahasiswa")]
        public JsonResult Post(Mahasiswa mhs)
        {
            string query = @"insert into mhs (Nama,Fakultas,Jurusan,Photofile) values (@Nama,@Fakultas,@Jurusan,@Photofile)";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("mahasiswaConn");
            MySqlDataReader myReader;
            using (MySqlConnection myconn = new MySqlConnection(sqlDataSource))
            {
                myconn.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myconn))
                {
                    myCommand.Parameters.AddWithValue("@Nama", mhs.Nama);
                    myCommand.Parameters.AddWithValue("@Fakultas", mhs.Fakultas);
                    myCommand.Parameters.AddWithValue("@Jurusan", mhs.Jurusan);
                    myCommand.Parameters.AddWithValue("@Photofile", mhs.Photofile);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myconn.Close();
                }
            }
            return new JsonResult("Berhasil Menambahkan Data Mahasiswa");
        }

        [HttpDelete] // hapus data mhs berdasarkan npm(id)
        [Route("mahasiswa/{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from mhs where Mhs=@Mhs;";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("mahasiswaConn");
            MySqlDataReader myReader;
            using (MySqlConnection myconn = new MySqlConnection(sqlDataSource))
            {
                myconn.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myconn))
                {
                    myCommand.Parameters.AddWithValue("@Mhs", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myconn.Close();
                }
            }
            return new JsonResult("Berhasil Delete Data Mahasiswa");
        }
     
        [HttpPost]
        [Route("mahasiswa/savepicture")]
        public JsonResult SavePic()
        {
            try
            {
                var httpRequest = Request.Form;
                var postFile = httpRequest.Files[0];
                string filename = postFile.FileName;
                var phisicalPath = _env.ContentRootPath + "/Photofiles/" + filename;

                using(var stream = new FileStream(phisicalPath, FileMode.Create))
                {
                    postFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch
            {
                return new JsonResult("Default.jpg");
            }
        }
    }
}
