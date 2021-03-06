﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MySql.Data.MySqlClient;

using System.IO;
using System.Drawing;
using System.Drawing.Printing;

namespace PenjualanPembelian
{
    public class Pelanggan
    {
        private int kodePelanggan;
        private string nama;
        private string alamat;
        private string telepon;

        #region CONSTRUCTORS
        public Pelanggan()
        {
            this.KodePelanggan = 0;
            this.Nama = "";
            this.Alamat = "";
            this.Telepon = "";
        }

        public Pelanggan(int pKode, string pNama, string pAlamat, string pTelepon)
        {
            this.KodePelanggan = pKode;
            this.Nama = pNama;
            this.Alamat = pAlamat;
            this.Telepon = pTelepon;
        }
        #endregion

        #region PROPERTIES
        public int KodePelanggan
        {
            get { return kodePelanggan; }
            set { kodePelanggan = value; }
        }

        public string Nama
        {
            get { return nama; }
            set { nama = value; }
        }

        public string Alamat
        {
            get { return alamat; }
            set { alamat = value; }
        }

        public string Telepon
        {
            get { return telepon; }
            set { telepon = value; }
        }
        #endregion

        #region METHODS
        public static string GenerateCode(out string pHasilId)
        {
            string sql = "SELECT COUNT(*) FROM pelanggan";
            pHasilId = "";

            try
            {
                MySqlDataReader hasilData = Koneksi.JalankanPerintahQuery(sql);

                if (hasilData.Read() == true)
                {
                    if (hasilData.GetValue(0).ToString() != "")
                    {
                        int idTerbaru = int.Parse(hasilData.GetValue(0).ToString()) + 1;

                        pHasilId = idTerbaru.ToString();
                    }
                    else
                    {
                        // Jika tidak ditemukan data dengan kategori tertentu maka kode terbaru = "J1"
                        pHasilId = "1";
                    }
                }
                return "1";
            }
            catch (Exception exc)
            {
                return exc.Message;
            }
        }

        public static string TambahData(Pelanggan pel)
        {
            string sql = "INSERT INTO pelanggan (KodePelanggan, Nama, Alamat, Telepon) VALUES ('" + pel.KodePelanggan + "', '" + pel.Nama.Replace("'", "\\'") + "', '" + pel.Alamat + "', '" + pel.Telepon + "')";

            try
            {
                Koneksi.JalankanPerintahDML(sql);
                return "1";
            }
            catch (MySqlException exc)
            {
                return exc.Message + ". Perintah SQL: " + sql;
            }
        }

        public static string BacaData(string kriteria, string nilaiKriteria, List<Pelanggan> listHasilData)
        {
            string sql = "";

            // JIka tidak ada kriteria yang diisikan
            if (kriteria == "")
            {
                sql = "SELECT * FROM pelanggan";
            }
            else
            {
                sql = "SELECT * FROM pelanggan WHERE " + kriteria + " LIKE '%" + nilaiKriteria + "%'";
            }

            try
            {
                MySqlDataReader hasilData = Koneksi.JalankanPerintahQuery(sql);

                while (hasilData.Read() == true) // selama masih ada data atau selama masih bisa membaca data
                {
                    // Baca hasil dari MySqlDataReader dan simpan di objek
                    Pelanggan pel = new Pelanggan();
                    pel.KodePelanggan = int.Parse(hasilData.GetValue(0).ToString());
                    pel.Nama = hasilData.GetValue(1).ToString();
                    pel.Alamat = hasilData.GetValue(2).ToString();
                    pel.Telepon = hasilData.GetValue(3).ToString();

                    // Simpan ke list
                    listHasilData.Add(pel);
                }

                return "1";
            }
            catch (MySqlException exc)
            {
                return exc.Message + ". Perintah sql : " + sql;
            }
        }

        public static string UbahData(Pelanggan pPelanggan)
        {
            string sql = "UPDATE pelanggan SET Nama = '" + pPelanggan.Nama.Replace("'", "\\'") +
                         "', Alamat = '" + pPelanggan.Alamat +
                         "', Telepon = '" + pPelanggan.Telepon +
                         "' WHERE KodePegawai = '" + pPelanggan.KodePelanggan + "'";
        
            try
            {
                Koneksi.JalankanPerintahDML(sql);
                return "1";
            }
            catch (MySqlException exc)
            {
                return exc.Message + ". Perintah sql : " + sql;
            }
        }

        public static string HapusData(Pelanggan pPelanggan)
        {
            string sql = "DELETE FROM pelanggan WHERE KodePelanggan = '" + pPelanggan.KodePelanggan + "'";

            try
            {
                Koneksi.JalankanPerintahDML(sql);
                return "1";
            }
            catch (MySqlException exc)
            {
                return exc.Message + ". Perintah sql : " + sql;
            }
        }

        public static string CetakNota(string pKriteria, string pNilaiKriteria, string pNamaFile)
        {
            try
            {
                List<Pelanggan> listPelanggan = new List<Pelanggan>();

                // baca data nota tertentu yang akan dicetak
                string hasilBaca = Pelanggan.BacaData(pKriteria, pNilaiKriteria, listPelanggan);

                // simpan dulu isi nota yang akan ditampilkan ke objek file (StreamWrite)
                StreamWriter file = new StreamWriter(pNamaFile);

                // tampilkan info perusahaan
                file.WriteLine("Laporan Per Tanggal : " + DateTime.Now.ToShortDateString());
                file.WriteLine("");
                file.WriteLine("TOKO MAJU MAKMUR UNTUNG SELALU");
                file.WriteLine("Jl. Raya Kalirungkut Surabaya");
                file.WriteLine("Telp. (031) - 12345678");
                file.WriteLine("**".PadRight(50, '*'));
                file.WriteLine("");

                for (int i = 0; i < listPelanggan.Count; i++)
                {
                    // tampilkan header nota
                    file.WriteLine("Kode Pelanggan : " + listPelanggan[i].KodePelanggan);
                    file.WriteLine("Nama : " + listPelanggan[i].Nama);
                    file.WriteLine("Alamat : " + listPelanggan[i].Alamat);
                    file.WriteLine("Telepon : " + listPelanggan[i].Telepon);
                    file.WriteLine("=".PadRight(50, '='));
                }
                file.Close();

                // cetak ke printer
                Cetak c = new Cetak(pNamaFile, "Courier New", 9, 10, 10, 10, 10);
                c.CetakKePrinter("tulisan");
                return "1";
            }
            catch (MySqlException exc)
            {
                return exc.Message;
            }
        }
        #endregion
    }
}

