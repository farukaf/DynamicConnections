using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;

namespace FarukDynamicConnection
{
    public abstract class DynamicConnection
    {
        public DynamicConnection()
        {

        }

        /// <summary>
        /// Connection String
        /// </summary>
        public abstract string Conn { get; }

        private SqlConnection _SqlConnection { get; set; }
        /// <summary>
        /// SqlConnection Object
        /// </summary>
        public SqlConnection SqlConnection
        {
            get
            {
                if (_SqlConnection == null)
                {
                    _SqlConnection = new SqlConnection(Conn);
                    SqlConnection.InfoMessage += OnInfoMessageGenerated;
                }
                return _SqlConnection;
            }
        }
        private void OnInfoMessageGenerated(object sender, SqlInfoMessageEventArgs e)
        {
            Messages.Add(e.Message);
        }

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
        /// You may want to execute and manage the commands yourself
        /// Execute the Query without Opening/closing the connection
        /// Remember: With great powers. Greater the damage... don't fix me
        /// </summary>
        /// <param name="sqlCommand">Command to be executed</param>
        /// <param name="parameters">List of sql parameters</param>
        /// <returns>returns the number of changed rows</returns>
        public int ExecuteUnManagedCommand(SqlCommand sqlCommand, List<SqlParameter> parameters = null)
        {
            if (parameters != null && parameters.Count > 0)
            {
                foreach (var param in parameters)
                {
                    sqlCommand.Parameters.Add(param);
                }
            }

            return sqlCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// You may want to execute and manage the commands yourself
        /// Execute the Query without Opening/closing the connection
        /// Remember: With great powers. Greater the damage... don't fix me
        /// </summary>
        /// <param name="query">Query to be executed</param>
        /// <param name="parameters">List of sql parameters</param>
        /// <returns>returns the number of changed rows</returns>
        public int ExecuteUnManagedQuery(string query, List<SqlParameter> parameters = null)
        {
            using (SqlCommand sqlCommand = new SqlCommand(query, SqlConnection))
            {
               return ExecuteUnManagedCommand(sqlCommand, parameters);
            }
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
        /// <param name="parameters">List of sql parameters</param>
        /// <returns>Object(T) List from query result</returns>
        public List<T> DynamicRead<T>(string query, List<SqlParameter> parameters = null)
        {
            return JsonConvert.DeserializeObject<List<T>>(JsonDynamicRead(query, parameters));
        }

        /// <summary>
        /// Read the result after sending query to DB
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters">List of sql parameters</param>
        /// <returns>Returns a json string from the list</returns>
        public string JsonDynamicRead(string query, List<SqlParameter> parameters = null)
        {
            return JsonConvert.SerializeObject(DynamicRead(query, parameters));
        }

        /// <summary>
        /// Execute the Query
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Returns the number of changed rows</returns>
        public int ExecuteQuery(string query, List<SqlParameter> parameters = null)
        {
            return ExecuteQuery(new List<string>() { query }, parameters);
        }

        public List<string> _Messages { get; set; }
        public List<string> Messages
        {
            get
            {
                if (_Messages == null)
                {
                    _Messages = new List<string>();
                }
                return _Messages;
            }
            set
            {
                _Messages = value;
            }
        }

        /// <summary>
        /// Execute the Querys
        /// </summary>
        /// <param name="querys"></param>
        /// <param name="parameters">List of sql parameters</param>
        /// <returns>returns the number of changed rows</returns>
        public int ExecuteQuery(List<string> querys, List<SqlParameter> parameters = null)
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
