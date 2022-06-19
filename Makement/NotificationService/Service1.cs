using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace NotificationService
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        private object obj = new object();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                WriteToFile("Service is started at " + DateTime.Now);
                timer.Elapsed += new ElapsedEventHandler((s, e) => { Tick(); });
                timer.Interval = 30 * 60 * 1000;
                timer.Start();
                WriteToFile("Service is inited at " + DateTime.Now);
            }
            catch (Exception ex)
            {
                WriteToFile("Error start : " + ex.Message + " at " + DateTime.Now);
                throw ex;
            }
        }

        private void Tick()
        {
            WriteToFile("Service is worked at " + DateTime.Now);
            try
            {
                if (17 < DateTime.Now.Hour)
                {
                    var connectionString = ConfigurationManager.AppSettings["connectionString"];
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand("Select Email, Text FROM Tasks Inner join AspNetUsers on Tasks.UserId = AspNetUsers.Id Where AspNetUsers.IsDeleted = 0 AND Tasks.IsDeleted = 0 AND Tasks.Status = 1", connection);
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            var email = reader.GetString(0);

                            var subject = "Complete!!!";
                            var text = "Active task: " + reader.GetString(1);
                            
                            SqlCommand addCommand = new SqlCommand($"INSERT INTO EmailMessages (Email, Text, Subject, Type) VALUES('{email}', @text, @subject, 1); ", connection);
                            addCommand.Parameters.AddWithValue("@text", text);
                            addCommand.Parameters.AddWithValue("@subject", subject);
                            addCommand.ExecuteNonQuery();

                            WriteToFile("Sent " + DateTime.Now);
                        }

                        connection.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                WriteToFile($"Exception {ex.Message} at {ex.Message}");
            }
        }

        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
            timer.Stop();
        }

        public static void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
