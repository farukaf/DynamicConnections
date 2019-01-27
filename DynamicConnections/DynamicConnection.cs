using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;

namespace DynamicConnections
{
    public class DynamicConnection
    {
        public DynamicConnection()
        {
            SqlConnection = new SqlConnection(this.Conn);
        }

        /// <summary>
        /// Throw a connection string
        /// </summary>
        /// <param name="connection"></param>
        public DynamicConnection(string connection)
        {
            SqlConnection = new SqlConnection(connection);
        }

        /// <summary>
        /// Connection String
        /// </summary>
        public string Conn { get { return "Data Source=LEEEEEROY;Initial Catalog=Teste;Integrated Security=True"; } }

        /// <summary>
        /// SqlConnection Object
        /// </summary>
        public SqlConnection SqlConnection { get; set; }

        public List<dynamic> DynamicRead(string query, List<SqlParameter> parameters)
        {
            List<dynamic> lst = new List<dynamic>();

            using (SqlCommand SqlCommand = new SqlCommand(query, SqlConnection))
            {
                SqlConnection.Open();

                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var param in parameters)
                    {
                        SqlCommand.Parameters.Add(param);
                    }
                }

                using (var reader = SqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lst.Add(DynamicRowFromReader(reader));
                    }
                }
            }
            SqlConnection.Close();

            return lst;
        }

        /// <summary>
        /// Read the result after sending query to DB
        /// </summary>
        /// <param name="query"></param>
        /// <returns> Dynamic List from query result/returns>
        public List<dynamic> DynamicRead(string query)
        {
            return DynamicRead(query, null);
        }

        /// <summary>
        /// Read the result after sending query to DB
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns>Object(T) List from query result</returns>
        public List<T> DynamicRead<T>(string query)
        {
            return JsonConvert.DeserializeObject<List<T>>(JsonDynamicRead(query));
        }

        /// <summary>
        /// Read the result after sending query to DB
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="parameters">List of sql parameters</param>
        /// <returns>Object(T) List from query result</returns>
        public List<T> DynamicRead<T>(string query, List<SqlParameter> parameters)
        {
            return JsonConvert.DeserializeObject<List<T>>(JsonDynamicRead(query, parameters));
        }

        /// <summary>
        /// Read the result after sending query to DB
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Returns a json string from the list</returns>
        public string JsonDynamicRead(string query)
        {
            return JsonConvert.SerializeObject(DynamicRead(query));
        }

        /// <summary>
        /// Read the result after sending query to DB
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters">List of sql parameters</param>
        /// <returns>Returns a json string from the list</returns>
        public string JsonDynamicRead(string query, List<SqlParameter> parameters)
        {
            return JsonConvert.SerializeObject(DynamicRead(query, parameters));
        }

        /// <summary>
        /// Execute the Query
        /// </summary>
        /// <param name="query"></param>
        /// <returns>returns the number of changed rows</returns>
        public int ExecuteQuery(string query)
        {
            return ExecuteQuery(new List<string>() { query });
        }

        /// <summary>
        /// Execute the Querys
        /// </summary>
        /// <param name="query"></param>
        /// <returns>returns the number of changed rows</returns>
        public int ExecuteQuery(List<string> querys)
        {
            return ExecuteQuery(querys, null);
        }

        /// <summary>
        /// Execute the Querys
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters">List of sql parameters</param>
        /// <returns>returns the number of changed rows</returns>
        public int ExecuteQuery(string query, List<SqlParameter> parameters)
        {
            return ExecuteQuery(new List<string>() { query }, parameters);
        }

        /// <summary>
        /// Execute the Querys
        /// </summary>
        /// <param name="querys"></param>
        /// <param name="parameters">List of sql parameters</param>
        /// <returns>returns the number of changed rows</returns>
        public int ExecuteQuery(List<string> querys, List<SqlParameter> parameters)
        {
            int c = 0;
            SqlConnection.Open();
            foreach (var query in querys)
            {
                using (SqlCommand SqlCommand = new SqlCommand(query, SqlConnection))
                {
                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (var param in parameters)
                        {
                            SqlCommand.Parameters.Add(param);
                        }
                    }

                    c += SqlCommand.ExecuteNonQuery();
                }
            }
            SqlConnection.Close();
            return c;

        }

        /// <summary>
        /// Returns a dynamic variable from a SqlDataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private dynamic DynamicRowFromReader(SqlDataReader reader)
        {
            var ret = new ExpandoObject() as IDictionary<string, object>;

            for (var i = 0; i < reader.FieldCount; i++)
                ret.Add(reader.GetName(i), reader[i]);

            return ret;
        }



    }
}
