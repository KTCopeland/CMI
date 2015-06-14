using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.IO;
using System.Linq;
using System.Text;

namespace CareerCenter
{
    public static class DataHandler
    {

        #region Instance Variables
        //This is ok for test.  Move to config file before migration.
        static string is_ConnectionString = "Data Source=184.168.194.70;Initial Catalog=cmi;User ID=ExperisGCS;Password=Exper1$GCS;MultipleActiveResultSets=True";
        static SqlConnection io_Connection = new SqlConnection();
        #endregion

        #region Site Settings
        static Setting io_Setting = new Setting();

        public static bool RefreshSettings()
        {
            return io_Setting.Refresh();
        }

        public static string GetSetting(string as_Key)
        {
            return io_Setting.GetSetting(as_Key);
        }

        #endregion

        #region Email

        static MailHandler io_Mail = new MailHandler();

        public static bool SendEmail(string as_Recipient, string as_Subject, string as_Body )
        {
            return io_Mail.SendEmail(as_Recipient, as_Subject, as_Body);
        }
            
        #endregion

        #region Database Access

        /// <summary>
        /// Establishes (if necessary) and passes back the Application's primary database connection
        /// </summary>
        /// <returns>True if connection is valid</returns>
        static public bool GetConnection()
        {
            return GetConnection(ref io_Connection);
        }

        /// <summary>
        /// Establishes (if necessary) and passes back the database connection that was provided
        /// </summary>
        /// <param name="ao_Conn"></param>
        /// <returns>True if connection is valid</returns>
        static public bool GetConnection(ref SqlConnection ao_Conn)
        {
            bool lb_Return = false;

            try
            {
                if (ao_Conn == null)
                {
                    ao_Conn = new SqlConnection(is_ConnectionString);
                    ao_Conn.Open();
                }
                else if (ao_Conn.State == ConnectionState.Open)
                {
                    //The object is already valid, just pass it back
                    lb_Return = true;
                }
                else
                {
                    //Object is no longer valid, try to correct
                    //ls_Conn = GetConnectionString();
                    ao_Conn = new SqlConnection(is_ConnectionString);
                    ao_Conn.Open();
                }

                //Test again before sending back
                if (ao_Conn.State == ConnectionState.Open)
                {
                    lb_Return = true;
                }
                else
                {
                    lb_Return = false;
                }

            }

            catch (Exception ex)
            {
                //Send to handler and return failure
                HandleError(ex);
                lb_Return = false;
            }

            return lb_Return;

        }

        /// <summary>
        /// Builds a dataset using the Application's primary database connection and passes it back
        /// </summary>
        /// <param name="ao_Dataset"></param>
        /// <param name="as_Query"></param>
        /// <returns>True if dataset is valid and has rows</returns>
        static public bool GetDatasetFromQuery(ref System.Data.DataSet ao_Dataset, string as_Query)
        {
            return GetDatasetFromQuery(ref io_Connection, ref ao_Dataset, as_Query);
        }

        /// <summary>
        /// Builds a dataset using the provided database connection and passes it back
        /// </summary>
        /// <param name="ao_Conn"></param>
        /// <param name="ao_DataSet"></param>
        /// <param name="as_Query"></param>
        /// <returns>True if dataset is valid and has rows</returns>
        static public bool GetDatasetFromQuery(ref SqlConnection ao_Conn, ref System.Data.DataSet ao_DataSet, string as_Query)
        {
            bool lb_Return = false;

            try
            {
                if (GetConnection(ref ao_Conn))
                {
                    //DataSet lo_DataSet = new DataSet();
                    if (ao_DataSet == null)
                    {
                        ao_DataSet = new DataSet();
                    }
                    SqlCommand lo_Command = new SqlCommand(as_Query, (SqlConnection)ao_Conn);
                    SqlDataAdapter lo_Adapter = new SqlDataAdapter();
                    lo_Adapter.SelectCommand = lo_Command;
                    lo_Adapter.Fill(ao_DataSet);
                    if (ao_DataSet.Tables[0].Rows.Count > 0)
                    {
                        //ao_DataSet = lo_DataSet;
                        lb_Return = true;
                    }
                    else
                    {
                        lb_Return = false;
                    }

                    lo_Adapter.Dispose();
                    lo_Command.Dispose();

                }
            }
            catch (Exception ex)
            {
                //Send to handler and return failure
                HandleError(ex);
                lb_Return = false;
            }

            return lb_Return;
        }

        static public bool InitializeCommandFromProcedure(ref SqlCommand ao_SqlCommand, string as_DBProcedure)
        {
            return InitializeCommandFromProcedure(ref ao_SqlCommand, ref io_Connection, as_DBProcedure);
        }

        static public bool InitializeCommandFromProcedure(ref SqlCommand ao_SqlCommand, ref SqlConnection ao_Conn, string as_DBProcedure)
        {
            bool lb_Return;

            try
            {
                if (!GetConnection(ref ao_Conn))
                {
                    return false;
                }

                if (ao_SqlCommand == null)
                {
                    ao_SqlCommand = new SqlCommand();
                }
                ao_SqlCommand.Connection = ao_Conn;
                ao_SqlCommand.CommandType = CommandType.StoredProcedure;
                ao_SqlCommand.CommandText = as_DBProcedure;
                SqlCommandBuilder.DeriveParameters((SqlCommand)ao_SqlCommand);
                lb_Return = true;

            }
            catch (Exception ex)
            {
                //Send to handler and return failure
                HandleError(ex);
                lb_Return = false;
            }

            return lb_Return;
        }

        static public bool InitializeCommandFromQuery(ref SqlCommand ao_SqlCommand, string as_Query)
        {
            return InitializeCommandFromQuery(ref ao_SqlCommand, ref io_Connection, as_Query);
        }

        static public bool InitializeCommandFromQuery(ref SqlCommand ao_SqlCommand, ref SqlConnection ao_Conn, string as_Query)
        {
            bool lb_Return;

            try
            {
                if (ao_Conn.State != ConnectionState.Open)
                {
                    return false;
                }
                if (ao_SqlCommand == null)
                {
                    ao_SqlCommand = new SqlCommand();
                }
                ao_SqlCommand.Connection = ao_Conn;
                ao_SqlCommand.CommandType = CommandType.Text;
                ao_SqlCommand.CommandText = as_Query;

                lb_Return = true;

            }
            catch (Exception ex)
            {
                //Send to handler and return failure
                HandleError(ex);
                lb_Return = false;
            }

            return lb_Return;
        }

        #endregion

        #region Error Handling

        static public bool HandleError(Exception ex)
        {
            bool lb_Return = false;

            try
            {
                HandleError(ex.Source, ex.Message, ex.StackTrace);
                lb_Return = true;
            }
            catch
            {
                //Not much else we can do if we end up here.  Return false.
                lb_Return = false;
            }

            return lb_Return;
        }

        static public bool HandleError(string as_Source, string as_Message, string as_Trace)
        {
            bool lb_Return = false;

            try
            {
                //{KTC} Add code to write error to the database if possible.
                //DisplayMessage(ex.StackTrace, ex.Message);
                lb_Return = true;
            }
            catch
            {
                //Not much else we can do if we end up here.  Return false.
                lb_Return = false;
            }

            return lb_Return;

        }

        #endregion

        #region Data Conversions
        //I have dealt with database Nulls so many times.  You'd think the geniuses at MS would come up with a better way.
        //Anyway, it has to be handled by convention.  Instance variables are placed here for easy reference.
        public static int IntNull = -999;
        public static string StringNull = "#NULL#";
        public static DateTime DateTimeNull = DateTime.Parse("01/01/0001 00:00:00");

        public static bool SetVal(ref SqlCommand ao_Command, string as_Parm, string as_Data)
        {
            bool lb_Return = false;

            try
            {
                if (as_Data == StringNull)
                {
                    ao_Command.Parameters[as_Parm].Value = DBNull.Value;
                }
                else
                {
                    ao_Command.Parameters[as_Parm].Value = as_Data;
                }
                lb_Return = true;
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
                lb_Return = false;
            }

            return lb_Return;
        }

        public static bool SetVal(ref SqlCommand ao_Command, string as_Parm, int ai_Data)
        {
            bool lb_Return = false;

            try
            {
                if (ai_Data == IntNull)
                {
                    ao_Command.Parameters[as_Parm].Value = DBNull.Value;
                }
                else
                {
                    ao_Command.Parameters[as_Parm].Value = ai_Data;
                }
                lb_Return = true;
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
                lb_Return = false;
            }

            return lb_Return;
        }

        public static bool SetVal(ref SqlCommand ao_Command, string as_Parm, bool ab_Data)
        {
            bool lb_Return = false;

            try
            {
                if (ab_Data)
                {
                    ao_Command.Parameters[as_Parm].Value = 1;
                }
                else
                {
                    ao_Command.Parameters[as_Parm].Value = 0;
                }
                lb_Return = true;
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
                lb_Return = false;
            }

            return lb_Return;
        }

        public static bool SetVal(ref SqlCommand ao_Command, string as_Parm, DateTime adt_Data)
        {
            bool lb_Return = false;

            try
            {
                if (adt_Data == DateTimeNull)
                {
                    ao_Command.Parameters[as_Parm].Value = DBNull.Value;
                }
                else
                {
                    ao_Command.Parameters[as_Parm].Value = adt_Data;
                }
                lb_Return = true;
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
                lb_Return = false;
            }

            return lb_Return;
        }

        public static bool GetVal(ref string as_Parm, object ao_Data)
        {
            bool lb_Return = false;

            try
            {
                if (ao_Data != DBNull.Value)
                {
                    as_Parm = ao_Data.ToString();
                }
                else
                {
                    as_Parm = StringNull;
                }
                lb_Return = true;
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
                lb_Return = false;
            }

            return lb_Return;
        }

        public static bool GetVal(ref int ai_Parm, object ao_Data)
        {
            bool lb_Return = false;

            try
            {
                if (ao_Data != DBNull.Value)
                {
                    ai_Parm = int.Parse(ao_Data.ToString());
                }
                else
                {
                    ai_Parm = IntNull;
                }
                lb_Return = true;
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
                lb_Return = false;
            }

            return lb_Return;
        }

        public static bool GetVal(ref bool ab_Parm, object ao_Data)
        {
            //Note: Can't configure a convention for bool (bit).  
            //If it's true, it's true.  Otherwise it's false even if it was DBNull.
            //Database schema should enforce that all bit columns are not null just to be safe.
            bool lb_Return = false;

            try
            {
                if (ao_Data != DBNull.Value)
                {
                    if (ao_Data.ToString() == "1" || ao_Data.ToString().ToUpper() == "TRUE")
                    {
                        ab_Parm = true;
                    }
                    else
                    {
                        ab_Parm = false;
                    }
                }
                else
                {
                    ab_Parm = false;
                }

                lb_Return = true;

            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
                lb_Return = false;
            }

            return lb_Return;
        }

        public static bool GetVal(ref DateTime adt_Parm, object ao_Data)
        {
            bool lb_Return = false;

            try
            {
                if (ao_Data != DBNull.Value)
                {
                    adt_Parm = DateTime.Parse(ao_Data.ToString());
                }
                else
                {
                    adt_Parm = DateTimeNull;
                }
                lb_Return = true;
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
                lb_Return = false;
            }

            return lb_Return;
        }

        #endregion      
 
        #region Export Data

        public static string Export(ref DataTable ao_Table)
        {
            string ls_Return = ""; //This will be the filename that will be returned if all goes well.  Otherwise, return ""
            string ls_FileName = ""; //This will be the local filename
            StringBuilder sb = new StringBuilder();

            try
            {
                IEnumerable<string> columnNames = ao_Table.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName);
                sb.AppendLine(string.Join(",", columnNames));

                foreach (DataRow row in ao_Table.Rows)
                {
                    IEnumerable<string> fields = row.ItemArray.Select(field =>
                      string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                    sb.AppendLine(string.Join(",", fields));
                }

                //Generate File Name....
                ls_FileName = ao_Table.TableName + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + "_" + DateTime.Now.Millisecond.ToString() + ".csv";

                File.WriteAllText(HttpContext.Current.Server.MapPath(@"~\report\") + ls_FileName, sb.ToString());
                ls_Return = @"http://app.contentmarketinginstitute.careers/report/" + ls_FileName;

            }
            catch (Exception ex)
            {
                HandleError(ex);
                ls_Return = ""; //Something went wrong, so unfortunately, there is no file to share :(
            }
            return ls_Return;
        }

        #endregion

    }
}