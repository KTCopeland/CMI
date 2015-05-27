using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CareerCenter.Controls
{
    public partial class EmployerControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            divEntry.Visible = true;
            divSubmitted.Visible = false;
        }

        protected void btnContactMe_Click(object sender, EventArgs e)
        {
            Employer employer = new Employer();

            employer.Employer_Available = true;
            employer.Employer_Contact_Name = txtName.Text;
            employer.Employer_Company_Name = txtCompany.Text;
            employer.Employer_Contact_Email = txtEmail.Text;
            employer.Employer_Contact_Phone = txtPhone.Text;
            employer.Employer_Contact_Title = txtJobTitle.Text;

            var result = false;
            result = employer.Create();

            divEntry.Visible = false;
            divSubmitted.Visible = true;

        }
    }
}