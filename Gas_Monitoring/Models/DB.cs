using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Gas_Monitoring.Models
{
    public class DB
    {
        private string address;
        private int port;
        private string id;
        private string pw;
        private string schema;
        private string strConn;
        private MySqlConnection conn;

        public string Address { get => address; set => address = value; }
        public int Port { get => port; set => port = value; }
        public string Id { get => id; set => id = value; }
        public string Pw { get => pw; set => pw = value; }
        public string StrConn { get => strConn; set => strConn = value; }
        public string Schema { get => schema; set => schema = value; }
        public MySqlConnection Conn { get => conn; set => conn = value; }

        public DB()
        {
        }

        public DB(string address, int port, string id, string pw)
        {
            Address = address;
            Port = port;
            Id = id;
            Pw = pw;
            StrConn = "Server=" + Address + ";User ID=" + Id + ";Password=" + Pw + ";Database=" + Schema + ";Min Pool Size=10;";
        }
        public bool DBConn()
        {
            //Address = "192.168.0.157";
            //port = 3306;
            //id = "root";
            //pw = "ubimicro";

            //Address = "192.168.2.2";
            //Port = 3306;
            //Id = "root";
            //Pw = "ubimicro";

            Address = "127.0.0.1";
            Port = 3307;
            Id = "root";
            Pw = "offset01";

            //Address = "106.252.240.216";
            //Port = 23306;
            //Id = "root";
            //Pw = "ubimicro";

            Schema = "gas";
            StrConn = "Server=" + Address + ";Port=" + Port + ";User ID=" + Id + ";Password=" + Pw + ";Database=" + Schema + ";Min Pool Size=10;";
            try
            {
                Conn = new MySqlConnection(strConn);
                Conn.Open();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("DB Connect Fail");
                return false;
            }
        }
        public MySqlDataReader select(string sql)
        {
            var cmd = new MySqlCommand(sql, Conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            return rdr;
        }

        public void update(string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, Conn);
            cmd.ExecuteNonQuery();
        }


    }
}
