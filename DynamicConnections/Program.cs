using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DynamicConnections
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //FillClientTable(100);
                DynamicConnection dynCon = new DynamicConnection();
                var list = dynCon.DynamicRead<Client>("select * from client");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        /// <summary>
        /// Randomly fill the Table
        /// </summary>
        /// <param name="n"></param>
        public static void FillClientTable(int n)
        {
            string[] nameLst = new string[] {
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
                "Poppy","Joanne","Linda",
                "Ava","Megan","Mia","Barbara",
                "Jessica","Lauren","Abigail",
                "Lily","Michelle","Madison",
                "Sophie","Tracy","Charlotte","Sarah"
            };

            string[] familyList = new string[] {
                "Smith","Murphy","Li",
                "Jones","O'Kelly","Johnson","Wilson",
                "Williams","O'Sullivan","Lam",
                "Brown","Walsh","Martin","Taylor","Gelbero",
                "Davies","O'Brien","Miller","Roy",
                "Byrne","Davis","Tremblay","Morton","Singh",
                "Evans","O'Ryan","Garcia","Lee","White","Wang",
                "Thomas","O'Connor","Rodriguez","Gagnon",
                "Roberts","O'Neill","Anderson"
            };

            for (int i = 0; i < n; i++)
            {
                Client cli = new Client();
                Helper.Randomize(nameLst);
                Helper.Randomize(familyList);
                cli.Name = nameLst[0] + " " + familyList[0] + " " + familyList[1];
                cli.Doc = Helper.RandomInt(333300, 999999).ToString();
                cli.Phone = "555-" + Helper.RandomInt(1111, 9999).ToString();

                string qry = @"INSERT INTO Client (Name,Doc,Phone) VALUES(@NAME, @DOC, @PHONE)";

                List<SqlParameter> paramLst = new List<SqlParameter>();
                paramLst.Add(new SqlParameter("NAME", cli.Name));
                paramLst.Add(new SqlParameter("DOC", cli.Doc));
                paramLst.Add(new SqlParameter("PHONE", cli.Phone));

                DynamicConnection dynCon = new DynamicConnection();
                dynCon.ExecuteQuery(qry, paramLst);

            }
        }

    }

    /*
    Create Table Client
    (
        ID INT IDENTITY(1,1) PRIMARY KEY,
        Name VARCHAR(200),
        Doc VARCHAR(40),
        Phone VARCHAR(40)
    )
    */
    public class Client
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Doc { get; set; }
        public string Phone { get; set; }
    }

}
