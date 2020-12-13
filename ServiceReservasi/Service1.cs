using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServiceReservasi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        string constring = "Data Source=LAPTOP-L35VG8L3;Initial Catalog=WCFReservasi;Persist Security Info=True;User ID=sa; Password=ashari@123";
        SqlConnection connection;
        SqlCommand com; //untuk mengoneksi database ke visual studio

        public List<DataRegister> DataRegist()
        {
            List<DataRegister> list = new List<DataRegister>();
            try
            {
                string sql = "select ID_login, Username, Password, Kategori from Login";
                connection = new SqlConnection(constring);
                com = new SqlCommand(sql, connection);
                connection.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DataRegister data = new DataRegister();
                    data.id = reader.GetInt32(0);
                    data.username = reader.GetString(1);
                    data.password = reader.GetString(2);
                    data.kategori = reader.GetString(3);
                    list.Add(data);
                }
                connection.Close();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }

        public string deletePemesanan(string IDPemesanan)
        {
            string a = "gagal";
            try
            {
                string sql = "delete from dbo.Pemesanan where ID_reservasi = '" + IDPemesanan + "'";
                connection = new SqlConnection(constring); //fungsi konek ke database
                com = new SqlCommand(sql, connection);
                connection.Open();
                com.ExecuteNonQuery();
                connection.Close();
                a = "sukses";
            }
            catch (Exception es)
            {
                Console.WriteLine(es);
            }
            return a;
        }

        public string DeleteRegister(string username)
        {
            try
            {
                int id = 0;
                string sql = "select ID_login from dbo.Login where Username = '" + username + "'";
                connection = new SqlConnection(constring);
                com = new SqlCommand(sql, connection);
                connection.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                }
                connection.Close();
                string sql2 = "delete from Login where ID_login" + id + "";
                connection = new SqlConnection(constring);
                com = new SqlCommand(sql2, connection);
                connection.Open();
                com.ExecuteNonQuery();
                connection.Close();

                return "sukses";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public List<DetailLokasi> DetailLokasi()
        {
            List<DetailLokasi> LokasiFull = new List<DetailLokasi>(); //proses untuk mendeclare nama list yang telah dibuat dengan nama baru
            try
            {
                string sql = "select ID_lokasi, Nama_lokasi, Deskripsi_full, Kouta from dbo.Lokasi"; //Declare query
                connection = new SqlConnection(constring); //fungsi koneks ke database
                com = new SqlCommand(sql, connection); //proses execute query
                connection.Open(); //membuka koneksi
                SqlDataReader reader = com.ExecuteReader(); //menampilkan data query
                while (reader.Read())
                {
                    //nama class
                    DetailLokasi data = new DetailLokasi(); //deklarasi data, mengambil 1persatu dari database

                    //bentuk array
                    data.IDLokasi = reader.GetString(0); //0 itu index, ada dikolom berapa di string sql diatas
                    data.NamaLokasi = reader.GetString(1);
                    data.DeskripsiFull = reader.GetString(2);
                    data.Kouta = reader.GetInt32(3);
                    LokasiFull.Add(data); //mengumpulkan data yang awalnya array
                }
                connection.Close(); //menutup akses ke database
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return LokasiFull;
        }

        public string editPemesanan(string IDPemesanan, string NamaCustomer, string No_telpon)
        {
            string a = "gagal";
            try
            {
                string sql = "update dbo.Pemesanan set Nama_customer = '" + NamaCustomer + "', Nama_telpon = '" + No_telpon + "'" +
                    "where ID_reservasi = '" + IDPemesanan + "'";
                connection = new SqlConnection(constring); //fungsi konek database
                com = new SqlCommand(sql, connection);
                connection.Open();
                com.ExecuteNonQuery();
                connection.Close();

                a = "sukses";
            }
            catch (Exception es)
            {
                Console.WriteLine(es);
            }
            return a;
        }

        public string GetData(int value)
        {
            throw new NotImplementedException();
        }


        public string Login(string username, string password)
        {
            string kategori = "";
            string sql = "select Kategori from Login where Username='" + username + "' and Password='" + password + "'";
            connection = new SqlConnection(constring);
            com = new SqlCommand(sql, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                kategori = reader.GetString(0);
            }
            return kategori;
        }

        public string pemesanan(string IDPemesanan, string NamaCustomer, string NoTelepon, int JumlahPemesanan, string IDLokasi)
        {
            string a = "gagal";
            try
            {
                string sql = "insert into dbo.Pemesanan values ('" + IDPemesanan + "','" + NamaCustomer + "','" + NoTelepon + "', " + JumlahPemesanan + ",'" + IDLokasi + "')";
                connection = new SqlConnection(constring); //fungsi konek ke database
                com = new SqlCommand(sql, connection);
                connection.Open();
                com.ExecuteNonQuery();
                connection.Close();

                string sql2 = "update dbo.Lokasi set Kouta = Kouta - " + JumlahPemesanan + " where ID_lokasi = '" + IDLokasi + "'";
                com = new SqlCommand(sql2, connection); //untuk konek ke database
                connection.Open();
                com.ExecuteNonQuery();
                connection.Close();
                a = "sukses";
            }
            catch (Exception es)
            {
                Console.WriteLine(es);
            }
            return a;
        }

        public List<Pemesanan> Pemesanan()
        {
            List<Pemesanan> pemesanans = new List<Pemesanan>();
            try
            {
                string sql = "select ID_reservasi, Nama_customer, Nama_telpon," +
                    "Jumlah_pemesanan, Nama_Lokasi from dbo.Pemesanan p join dbo.Lokasi l on p.ID_lokasi = l.ID_lokasi";
                connection = new SqlConnection(constring); // fungsi konek ke database
                com = new SqlCommand(sql, connection); //proses execute query
                connection.Open(); //membuka koneksi
                SqlDataReader reader = com.ExecuteReader(); //menampilkan data query
                while (reader.Read())
                {
                    //Nama class
                    Pemesanan data = new Pemesanan(); //Deklarasi data, mengambil 1persatu dari database
                    //bentuk array
                    data.IDPemesanan = reader.GetString(0);
                    data.NamaCustomer = reader.GetString(1);
                    data.NoTelepon = reader.GetString(2);
                    data.JumlahPemesanan = reader.GetInt32(3);
                    data.Lokasi = reader.GetString(4);
                    pemesanans.Add(data); //mengumpulkan data yang awalnya dari array
                }
                connection.Close(); //menutup akses ke database
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return pemesanans;
        }

        public string Register(string username, string password, string kategori)
        {
            try
            {
                string sql = "insert into Login values('" + username + "', '" + password + "', '" + kategori + "')";
                connection = new SqlConnection(constring);
                com = new SqlCommand(sql, connection);
                connection.Open();
                com.ExecuteNonQuery();
                connection.Close();

                return "sukses";

            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public List<CekLokasi> ReviewLokasi()
        {
            throw new NotImplementedException();
        }

        public string UpdateRegister(string username, string password, string kategori, int id)
        {
            try
            {
                string sql2 = "update Login SET Username='" + username + "', Password='" + password + "', Kategori='" + kategori + "'," +
                    "where ID_login = " + id + "";
                connection = new SqlConnection(constring);
                com = new SqlCommand(sql2, connection);
                connection.Open();
                com.ExecuteNonQuery();
                connection.Close();

                return "sukses";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }
}