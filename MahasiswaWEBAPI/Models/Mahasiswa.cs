using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MahasiswaWEBAPI.Models
{
    public class Mahasiswa
    {
        public int Npm { get; set; }
        public string Nama { get; set; }
        public string Fakultas { get; set; }
        public string Jurusan { get; set; }
        public string Photofile { get; set; }
    }
}
