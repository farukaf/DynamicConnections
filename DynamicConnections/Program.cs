using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DynamicConnections
{
    public static class Program
    {
        static void Main(string[] args)
        {

        }

        public static int CreateClientTable()
        {
            Context dynCon = new Context();
            return dynCon.ExecuteQuery(@"
                Create Table Client
                (
                    ID INT IDENTITY(1, 1) PRIMARY KEY,
                    Name VARCHAR(200),
                    Doc VARCHAR(40),
                    Phone VARCHAR(40)
                )");

        }
        private static void ManageUnmanagedCommand()
        {
            ServerContext dynCon = new ServerContext();
            dynCon.SqlConnection.Open();
            using (var transaction = dynCon.SqlConnection.BeginTransaction())
            {
                try
                {

                    dynCon.ExecuteUnManagedQuery("Teste");

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    transaction.Rollback();
                }
                finally
                {
                    if (dynCon.SqlConnection.State == System.Data.ConnectionState.Open)
                    {
                        dynCon.SqlConnection.Close();
                    }
                }
            }
        }

        public static int CreateDatabase()
        {
            ServerContext dynCon = new ServerContext();
            return dynCon.ExecuteQuery(@"
                CREATE DATABASE DynamicConnectionTest;");

        }

        public static int DropClientTable()
        {
            Context dynCon = new Context();
            return dynCon.ExecuteQuery("DROP TABLE Client");
        }
        public static List<Client> SelectAllClientTable()
        {
            Context dynCon = new Context();
            var list = dynCon.DynamicRead<Client>("SELECT * FROM Client");
            return list;
        }

        /// <summary>
        /// Randomly fill the Table
        /// </summary>
        /// <param name="n"></param>
        public static int FillClientTable(int n)
        {
            string[] nameLst = new string[] {
                "Faruk","Daiana","Lillian",
                "Oliver","Jake","Noah","James",
                "Jack","Connor","Liam","John",
                "Harry","Callum","Mason","Robert",
                "Jacob","Michael","Charlie","Kyle",
                "Thomas","Joe","Ethan","David",
                "George","Reece","Richard",
                "Oscar","Rhys","Alexander","Joseph",
                "Victoria","Susan","James","Charles",
                "William","Damian","Daniel",
                "Amelia","Margaret","Emma","Mary",
                "Olivia","Samantha","Patricia",
                "Isla","Bethany","Sophia","Jennifer",
                "Emily","Isabella","Elizabeth",
                "Poppy","Joanne","Linda","Sarah",
                "Ava","Megan","Mia","Barbara",
                "Jessica","Lauren","Abigail",
                "Lily","Michelle","Madison",
                "Sophie","Tracy","Charlotte"
            };

            string[] familyList = new string[] {
                "Feres","Smith","Murphy","Singh",
                "Jones","O'Kelly","Johnson","Wilson",
                "Williams","O'Sullivan","Lam","Li",
                "Brown","Walsh","Martin","Taylor",
                "Davies","O'Brien","Miller","Roy",
                "Byrne","Davis","Tremblay","Morton",
                "Evans","O'Ryan","Garcia","Lee",
                "Thomas","O'Connor","Rodriguez",
                "Roberts","O'Neill","Anderson",
                "Gelbero","Gagnon","White","Wang"

            };
            int ret = 0;
            for (int i = 0; i < n; i++)
            {
                Client cli = new Client();
                Helper.Randomize(nameLst);
                Helper.Randomize(familyList);
                cli.Name = nameLst[0] + " " + familyList[0] + " " + familyList[1];
                cli.Doc = Helper.RandomInt(1000, 99999999).ToString("000000000");
                cli.Phone = "555-" + Helper.RandomInt(11, 9999).ToString("0000");

                string qry = @"INSERT INTO Client (Name,Doc,Phone) VALUES(@NAME, @DOC, @PHONE)";

                List<SqlParameter> paramLst = new List<SqlParameter>();
                paramLst.Add(new SqlParameter("NAME", cli.Name));
                paramLst.Add(new SqlParameter("DOC", cli.Doc));
                paramLst.Add(new SqlParameter("PHONE", cli.Phone));

                Context dynCon = new Context();
                ret += dynCon.ExecuteQuery(qry, paramLst);

            }
            return ret;
        }

    }

    public class Client
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Doc { get; set; }
        public string Phone { get; set; }
    }

}
