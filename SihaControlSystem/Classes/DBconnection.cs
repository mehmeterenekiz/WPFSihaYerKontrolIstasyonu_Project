using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SihaControlSystem.Classes
{
    class DBconnection
    {
        public static string DBaddress = @"Data Source=" + Environment.CurrentDirectory + "\\DB\\kitap.db;Version=3;New=False;Compress=True;Read Only=False;";

        public static string ConnectionStatus;
        public static void ConnectionTest()
        {
            using (SQLiteConnection conn = new SQLiteConnection(DBaddress))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    try
                    {
                        conn.Open();
                        ConnectionStatus = "Connection is successful";

                    }
                    catch (Exception)
                    {
                        ConnectionStatus = "Connection failed";
                    }
                }
                else
                {
                    ConnectionStatus = "Connection is successful";
                }
            }
        }
    }
}
