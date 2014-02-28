using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Utility
{
    public class Utility
    {
        public static void ExecSqlScript(string query)
        {
            using (var conn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                var sqlBatch = string.Empty;
                var cmd = new SqlCommand(string.Empty, conn);
                conn.Open();
                query += "\nGO";   // make sure last batch is executed.

                var startLineNumber = 1;
                var endLineNumber = 0;

                try
                {
                    foreach (var line in query.Split(new string[2] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (line.ToUpperInvariant().Trim() == "GO")
                        {
                            if (!string.IsNullOrEmpty(sqlBatch.Trim()))
                            {
                                Console.WriteLine("Executing:\n" + sqlBatch);
                                cmd.CommandText = sqlBatch;
                                cmd.ExecuteNonQuery();
                                sqlBatch = string.Empty;
                            }

                            endLineNumber++;
                            startLineNumber = endLineNumber;
                        }
                        else
                        {
                            sqlBatch += line + "\n";
                            endLineNumber++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Execute query script lines( {0},{1} ) Error: {2}", endLineNumber, startLineNumber, sqlBatch), ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

    }
}
