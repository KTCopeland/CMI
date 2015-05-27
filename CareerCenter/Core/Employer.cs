using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;

namespace CareerCenter
{
    public class Employer
    {

        #region Instance Variables

        int ii_Employer_ID = DataHandler.IntNull;
        string is_Employer_Company_Name = DataHandler.StringNull;
        string is_Employer_Contact_Name = DataHandler.StringNull;
        string is_Employer_Contact_Title = DataHandler.StringNull;
        string is_Employer_Contact_Email = DataHandler.StringNull;
        string is_Employer_Contact_Phone = DataHandler.StringNull;
        bool ib_Employer_Available = false; //Set initial state to false

        string is_DBProc = "usp_employer";
        string is_DBTable = "employer";

        #endregion

        #region Properties

        public int Employer_ID
        {
            get
            {
                return ii_Employer_ID;
            }
        }

        public string Employer_Company_Name
        {
            get
            {
                return is_Employer_Company_Name;
            }
            set
            {
                if (value.Length > 100)
                {
                    is_Employer_Company_Name = value.Substring(0, 100);
                }
                else
                {
                    is_Employer_Company_Name = value;
                }
            }
        }

        public string Employer_Contact_Name
        {
            get
            {
                return is_Employer_Contact_Name;
            }
            set
            {
                if (value.Length > 100)
                {
                    is_Employer_Contact_Name = value.Substring(0, 100);
                }
                else
                {
                    is_Employer_Contact_Name = value;
                }
            }
        }

        public string Employer_Contact_Title
        {
            get
            {
                return is_Employer_Contact_Title;
            }
            set
            {
                if (value.Length > 100)
                {
                    is_Employer_Contact_Title = value.Substring(0, 100);
                }
                else
                {
                    is_Employer_Contact_Title = value;
                }
            }
        }

        public string Employer_Contact_Email
        {
            get
            {
                return is_Employer_Contact_Email;
            }
            set
            {
                if (value.Length > 150)
                {
                    is_Employer_Contact_Email = value.Substring(0, 150);
                }
                else
                {
                    is_Employer_Contact_Email = value;
                }
            }
        }

        public string Employer_Contact_Phone
        {
            get
            {
                return is_Employer_Contact_Phone;
            }
            set
            {
                if (value.Length > 50)
                {
                    is_Employer_Contact_Phone = value.Substring(0, 50);
                }
                else
                {
                    is_Employer_Contact_Phone = value;
                }
            }
        }

        public bool Employer_Available
        {
            get
            {
                return ib_Employer_Available;
            }
            set
            {
                ib_Employer_Available = value;
            }
        }

        #endregion

        #region Methods

        public bool Create()
        {
            bool lb_Return = false;
            try
            {
                SqlCommand lo_Command = new SqlCommand();
                if (DataHandler.InitializeCommandFromProcedure(ref lo_Command, is_DBProc))
                {
                    int li_ID;
                    int li_Result_ID;
                    string ls_Result_Msg;


                    DataHandler.SetVal(ref lo_Command, "@Action", "INSERT");
                    DataHandler.SetVal(ref lo_Command, "@employer_id", DataHandler.IntNull);
                    DataHandler.SetVal(ref lo_Command, "@employer_company_name", is_Employer_Company_Name);
                    DataHandler.SetVal(ref lo_Command, "@employer_contact_name", is_Employer_Contact_Name);
                    DataHandler.SetVal(ref lo_Command, "@employer_contact_title", is_Employer_Contact_Title);
                    DataHandler.SetVal(ref lo_Command, "@employer_contact_email", is_Employer_Contact_Email);
                    DataHandler.SetVal(ref lo_Command, "@employer_contact_phone", is_Employer_Contact_Phone);
                    DataHandler.SetVal(ref lo_Command, "@employer_available", ib_Employer_Available);

                    lo_Command.ExecuteNonQuery();

                    li_ID = int.Parse(lo_Command.Parameters["@employer_id"].Value.ToString());
                    li_Result_ID = int.Parse(lo_Command.Parameters["@Result_ID"].Value.ToString());
                    ls_Result_Msg = lo_Command.Parameters["@Result_Msg"].Value.ToString();

                    lo_Command.Dispose();

                    if (li_Result_ID < 0)
                    {
                        DataHandler.HandleError("Employer.Create()", ls_Result_Msg, "Employer = " + this.Employer_Company_Name);
                    }
                    else
                    {
                        ii_Employer_ID = li_ID;

                        //Employer Submission Response
                        if (DataHandler.GetSetting("employer_response_enabled").ToUpper() == "TRUE")
                        {
                            string[] ls_Name = this.Employer_Contact_Name.Split(' ');
                            string ls_Body = DataHandler.GetSetting("employer_response_text").Replace("{NAME}", ls_Name[0]).Replace("{EMAIL_HEADER}", DataHandler.GetSetting("email_header"));
                            DataHandler.SendEmail(this.Employer_Contact_Email, "Thank you for your interest", ls_Body);
                        }

                        //Employer Submission Notification
                        if (DataHandler.GetSetting("employer_notification_enabled").ToUpper() == "TRUE")
                        {
                            string[] ls_Name = this.Employer_Contact_Name.Split(' ');
                            string ls_Body = DataHandler.GetSetting("employer_notification_text").Replace("{NAME}", ls_Name[0]).Replace("{EMAIL_HEADER}", DataHandler.GetSetting("email_header"));

                            StringBuilder lo_CompanyDetail = new StringBuilder();
                            lo_CompanyDetail.Append("<hr/>Company: " + this.Employer_Company_Name + "<br/>");
                            lo_CompanyDetail.Append("Contact: " + this.Employer_Contact_Name + ", " + this.Employer_Contact_Title + "<br/>");
                            lo_CompanyDetail.Append("Contact Info: " + this.Employer_Contact_Email + " | " + this.Employer_Contact_Phone + "<br/>");

                            ls_Body = ls_Body.Replace("{EMPLOYER_DETAIL}", lo_CompanyDetail.ToString());
                            DataHandler.SendEmail(DataHandler.GetSetting("employer_notification_recipients"), "New Employer Submission", ls_Body);
                        }
                        lb_Return = true;
                    }
                }
                else
                {
                    DataHandler.HandleError("Employer.Create()", "Unable to create command object", "Employer = " + this.Employer_Company_Name);
                }
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
            }

            return lb_Return;
        }

        public bool Retrieve(int ai_Employer)
        {
            bool lb_Return = false;
            string ls_SQL = "Select * from " + is_DBTable + " where employer_id = " + ai_Employer.ToString();
            DataSet lo_Data = new DataSet();

            try
            {
                if (DataHandler.GetDatasetFromQuery(ref lo_Data, ls_SQL))
                {
                    this.ii_Employer_ID = ai_Employer;
                    DataHandler.GetVal(ref this.is_Employer_Company_Name, lo_Data.Tables[0].Rows[0]["employer_company_name"]);
                    DataHandler.GetVal(ref this.is_Employer_Contact_Name, lo_Data.Tables[0].Rows[0]["employer_contact_name"]);
                    DataHandler.GetVal(ref this.is_Employer_Contact_Title, lo_Data.Tables[0].Rows[0]["employer_contact_title"]);
                    DataHandler.GetVal(ref this.is_Employer_Contact_Email, lo_Data.Tables[0].Rows[0]["employer_contact_email"]);
                    DataHandler.GetVal(ref this.is_Employer_Contact_Phone, lo_Data.Tables[0].Rows[0]["employer_contact_phone"]);
                    DataHandler.GetVal(ref this.ib_Employer_Available, lo_Data.Tables[0].Rows[0]["employer_available"]);
                    lo_Data.Dispose();
                    //Made it to the end without an error  Should be good to go
                    lb_Return = true;
                }
                else
                {
                    DataHandler.HandleError("Employer.Retrieve()", "Retrieve failed", "GetDatasetFromQuery(" + ai_Employer.ToString() + ")");
                }
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
            }

            return lb_Return;
        }

        public bool Update()
        {
            bool lb_Return = false;
            try
            {
                SqlCommand lo_Command = new SqlCommand();
                if (DataHandler.InitializeCommandFromProcedure(ref lo_Command, is_DBProc))
                {
                    int li_Result_ID;
                    string ls_Result_Msg;

                    DataHandler.SetVal(ref lo_Command, "@Action", "UPDATE");
                    DataHandler.SetVal(ref lo_Command, "@employer_id", ii_Employer_ID);
                    DataHandler.SetVal(ref lo_Command, "@employer_company_name", is_Employer_Company_Name);
                    DataHandler.SetVal(ref lo_Command, "@employer_contact_name", is_Employer_Contact_Name);
                    DataHandler.SetVal(ref lo_Command, "@employer_contact_title", is_Employer_Contact_Title);
                    DataHandler.SetVal(ref lo_Command, "@employer_contact_email", is_Employer_Contact_Email);
                    DataHandler.SetVal(ref lo_Command, "@employer_contact_phone", is_Employer_Contact_Phone);
                    DataHandler.SetVal(ref lo_Command, "@employer_available", ib_Employer_Available);

                    lo_Command.ExecuteNonQuery();

                    li_Result_ID = int.Parse(lo_Command.Parameters["@Result_ID"].Value.ToString());
                    ls_Result_Msg = lo_Command.Parameters["@Result_Msg"].Value.ToString();

                    lo_Command.Dispose();

                    if (li_Result_ID < 0)
                    {
                        DataHandler.HandleError("Candidate.Update()", ls_Result_Msg, "Candidate_ID = " + this.ii_Employer_ID.ToString());
                    }
                    else
                    {
                        lb_Return = true;
                    }
                }
                else
                {
                    DataHandler.HandleError("Candidate.Update()", "Unable to create command object", "Candidate_ID = " + this.ii_Employer_ID.ToString());
                }
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
            }

            return lb_Return;
        }

        public bool Delete()
        {
            bool lb_Return = false;
            try
            {
                SqlCommand lo_Command = new SqlCommand();
                if (DataHandler.InitializeCommandFromProcedure(ref lo_Command, is_DBProc))
                {
                    int li_Result_ID;
                    string ls_Result_Msg;

                    lo_Command.Parameters["@Action"].Value = "DELETE";
                    lo_Command.Parameters["@employer_id"].Value = this.ii_Employer_ID;

                    lo_Command.ExecuteNonQuery();

                    li_Result_ID = int.Parse(lo_Command.Parameters["@Result_ID"].Value.ToString());
                    ls_Result_Msg = lo_Command.Parameters["@Result_Msg"].Value.ToString();

                    lo_Command.Dispose();

                    if (li_Result_ID < 0)
                    {
                        DataHandler.HandleError("Employer.Delete()", ls_Result_Msg, "Employer_ID = " + this.ii_Employer_ID.ToString());
                    }
                    else
                    {
                        lb_Return = true;
                    }
                }
                else
                {
                    DataHandler.HandleError("Employer.Delete()", "Unable to create command object", "Employer_ID = " + this.ii_Employer_ID.ToString());
                    lb_Return = false;
                }
            }
            catch (Exception ex)
            {
                DataHandler.HandleError(ex);
            }

            return lb_Return;
        }


        #endregion

    }
}