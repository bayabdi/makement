using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace EmailSenderService
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        private object obj = new object();
        private bool IsWork { get; set; }

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                WriteToFile("Service is started at " + DateTime.Now);
                
                timer.Interval = 60 * 1000;
                timer.Elapsed += new ElapsedEventHandler((o, e) => { Tick(); });
                timer.Start();
            }
            catch (Exception ex)
            {
                WriteToFile("Error start : " + ex.Message + " at " + DateTime.Now);
                throw ex;
            }
        }

        private bool Send(string email, string body, string subject, int type)
        {
            try
            {
                string fromEmail;
                string fromPassword;

                if (type == 0)
                {
                    fromEmail = ConfigurationManager.AppSettings["RequestEmail"];
                    fromPassword = ConfigurationManager.AppSettings["RequestPassword"];
                }
                else
                {
                    fromEmail = ConfigurationManager.AppSettings["NotificationEmail"];
                    fromPassword = ConfigurationManager.AppSettings["NotificationPassword"];
                }

                using (var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromEmail, fromPassword),
                    EnableSsl = true,
                })
                {

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true,
                    };

                    WriteToFile($"Sending to {email}");

                    mailMessage.To.Add(email);

                    smtpClient.Send(mailMessage);
                    WriteToFile($"Sent to {email} at {DateTime.Now}");

                    Thread.Sleep(5 * 1000);
                }

                return true;
            }
            catch(Exception ex)
            {
                WriteToFile($"Error {ex.InnerException} at  {DateTime.Now}");
                return false;
            }
        }

        private void Tick()
        {

            WriteToFile("Begin at " + DateTime.Now);
            
            lock(obj)
            {
                try
                {
                    WriteToFile("Do at " + DateTime.Now);

                    var connectionString = ConfigurationManager.AppSettings["connectionString"];
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand("SELECT * FROM EmailMessages", connection);
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            var message = new
                            {
                                Id = reader.GetInt32(0),
                                Email = reader.GetString(1),
                                Text = reader.GetString(2),
                                Subject = reader.GetString(3),
                                Type = reader.GetInt32(4)
                            };
                            if (Send(message.Email, message.Text, message.Subject, message.Type))
                            {
                                SqlCommand removeCommand = new SqlCommand($"Delete FROM EmailMessages Where Id = {message.Id}", connection);
                                removeCommand.ExecuteNonQuery();
                            }
                        }

                        connection.Close();
                    }
                }
                catch(Exception ex)
                {
                    WriteToFile("Error start : " + ex.Message + " at " + DateTime.Now);
                    throw ex;
                }
            }

            WriteToFile("End at " + DateTime.Now);
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

        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
            timer.Stop();
        }
    }
}
