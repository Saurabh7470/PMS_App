using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using Microsoft.Data.SqlClient;

namespace Aon_PMS.Server.Services
{
    public class SqlService
    {
        private IConfiguration _configuration;

        public SqlService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<T>? getDatas<T>(string sp, SqlParameter[] param) where T : class
        {
            using (SqlConnection conn = new SqlConnection(_configuration?.GetConnectionString("db")?.ToString()))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sp;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    param.ToList().ForEach(a =>
                    {
                        cmd.Parameters.Add(a);
                    });
                    var jsonResult = new StringBuilder();
                    var reader = cmd.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        jsonResult.Append("[]");
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            jsonResult.Append(reader.GetValue(0).ToString());
                        }
                    }
                    List<T> data = new List<T>();
                    try
                    {
                        data = JsonConvert.DeserializeObject<List<T>>(jsonResult.ToString());
                    }
                    catch { }
                    return data;
                }
            }
        }

        public T? getData<T>(string sp, SqlParameter[] param) where T : class, new()
        {
            using (SqlConnection conn = new SqlConnection(_configuration?.GetConnectionString("db")?.ToString()))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sp;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    param.ToList().ForEach(a =>
                    {
                        cmd.Parameters.Add(a);
                    });
                    var jsonResult = new StringBuilder();
                    var reader = cmd.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        jsonResult.Append("[]");
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            jsonResult.Append(reader.GetValue(0).ToString());
                        }
                    }
                    T data = new();
                    try
                    {
                        data = JsonConvert.DeserializeObject<T>(jsonResult.ToString());
                    }
                    catch { }
                    return data;
                }
            }
        }

        public void postData(string sp, SqlParameter[] param)
        {
            using (SqlConnection conn = new SqlConnection(_configuration?.GetConnectionString("db")?.ToString()))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sp;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    param.ToList().ForEach(a =>
                    {
                        cmd.Parameters.Add(a);
                    });
                    var jsonResult = new StringBuilder();
                   // var jsonresult1 = jsonResult.Replace('n', ' ');
                    var reader = cmd.ExecuteReader();
                }
            }
        }

        public DataTable getDataAsDataTable(string sp, SqlParameter[] param)
        {
            DataTable dt = new();
            using (SqlConnection conn = new SqlConnection(_configuration?.GetConnectionString("db")?.ToString()))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sp;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    param.ToList().ForEach(a =>
                    {
                        cmd.Parameters.Add(a);
                    });
                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }

        public DataSet getDataAsDataSet(string sp, SqlParameter[] param)
        {
            DataSet dt = new();
            using (SqlConnection conn = new SqlConnection(_configuration?.GetConnectionString("db")?.ToString()))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sp;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    param.ToList().ForEach(a =>
                    {
                        cmd.Parameters.Add(a);
                    });
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        //internal ActionResult<IEnumerable<T>?> getDatas<T>(string v, Microsoft.Data.SqlClient.SqlParameter[] @params)
        //{
        //    throw new NotImplementedException();
        //}
    }
}