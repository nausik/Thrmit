using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace OW_Test
{
    class DB
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        private string monitors_table = "monitors";

        public DB(string con_server, string con_database, string con_uid, string con_password)
        {
            server = con_server;
            database = con_database;
            uid = con_uid;
            password = con_password;
            Initialize();
        }

        private void Initialize()
        {
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                Debug.WriteLine("Connection successful!");

                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Debug.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Debug.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                connection.Close();

                Debug.WriteLine("Connection closed!");
                return true;
            }
            catch (MySqlException ex)
            {
                Debug.Write(ex);
                return false;
            }
        }

        public void testQuery()
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                string qry = "LOAD DATA LOCAL INFILE '/test.txt' INTO TABLE" + monitors_table + "LINES TERMINATED BY '\r\n'";
                cmd.CommandText = qry;
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public bool uploadMonitors(Tree tree)
        {
            string query_start = "INSERT IGNORE INTO " + monitors_table + " (OWD_ID) VALUES ";
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;

            if (tree.monitorsCount() > 100)
            {
                int i = 0;
                while (i < tree.monitorsCount())
                {
                    string query = query_start;
                    query += tree.getMonitorsAsString(100, i);
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    i += 100;
                }

            }

            else
            {
                string query = query_start;
                query += tree.getMonitorsAsString(0);
                Debug.Write(query);
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }



            try
            {
                return true;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }
    }
}
