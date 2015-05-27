using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CareerCenter.Pages
{
    public partial class EditJob : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Job lo_Job = new Job();
            string ls_Status = "Unknown";

            if (!IsPostBack)
            {
                try
                {

                    //Populate the employer list
                    DataSet lo_Employer = new DataSet();
                    DataHandler.GetDatasetFromQuery(ref lo_Employer, "Select * from employer");
                    ListItem lo_DDL;
                    for (int li_loop = 0;li_loop< lo_Employer.Tables[0].Rows.Count; li_loop++)
                    {
                        lo_DDL = new ListItem();
                        lo_DDL.Text = lo_Employer.Tables[0].Rows[li_loop]["employer_company_name"].ToString() + "(" + lo_Employer.Tables[0].Rows[li_loop]["employer_contact_name"].ToString() + ")";
                        lo_DDL.Value = lo_Employer.Tables[0].Rows[li_loop]["employer_id"].ToString();
                        ddlEmployer.Items.Add(lo_DDL);
                    }



                    int li_Job = int.Parse(Request.QueryString["ID"].ToString());
                    lo_Job.Retrieve(li_Job);

                    txtID.Text = lo_Job.Job_ID.ToString().PadLeft(6, '0');
                    txtTitle.Text = lo_Job.Job_Title;
                    ddlEmployer.SelectedValue = lo_Job.Employer_ID.ToString();
                    txtCity.Text = lo_Job.Job_City;
                    txtTerritory.Text = lo_Job.Job_Territory;
                    txtPostalCode.Text = lo_Job.Job_Postal_Code;
                    txtActiveDate.Text = lo_Job.Job_Active_Date.ToString("MM/dd/yyyy");
                    txtInactiveDate.Text = lo_Job.Job_Inactive_Date.ToString("MM/dd/yyyy");
                    chkAvailable.Checked = lo_Job.Job_Available;
                    divDescription.InnerHtml = lo_Job.Job_Description;
                    ls_Status = "Loaded";
                }
                catch (Exception ex)
                {
                    DataHandler.HandleError(ex);
                }

                if (ls_Status != "Loaded")
                {
                    //Either there was an error or no ID was passed in.  Either way, we will assume a new record
                    txtID.Text = "New";
                }

            }
            else
            {
                //The user sent stuff back to us.  Attempt to save it
                if (txtID.Text.ToLower() != "new")
                {
                    int li_Job = int.Parse(txtID.Text);
                    lo_Job.Retrieve(li_Job);
                }

                lo_Job.Job_Title = txtTitle.Text;
                lo_Job.Job_Description = hfDescription.Value;
                lo_Job.Employer_ID = int.Parse(ddlEmployer.SelectedValue.ToString());
                lo_Job.Job_City = txtCity.Text;
                lo_Job.Job_Territory = txtTerritory.Text;
                lo_Job.Job_Postal_Code = txtPostalCode.Text;
                lo_Job.Job_Active_Date = DateTime.Parse(txtActiveDate.Text);
                lo_Job.Job_Inactive_Date = DateTime.Parse(txtInactiveDate.Text);
                lo_Job.Job_Available = chkAvailable.Checked;
                lo_Job.Job_Country = "United States of America"; //Change this if international functionality is added.

                //Update or insert?
                if (txtID.Text.ToLower() != "new")
                {
                    lo_Job.Update();
                }
                else
                {
                    lo_Job.Create();
                }

                Response.Redirect(@"\pages\editjob.aspx?id=" + lo_Job.Job_ID.ToString());

            }

        }
    }
}