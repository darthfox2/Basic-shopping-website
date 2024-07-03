using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

public partial class Backend_LoginErr : System.Web.UI.Page
{
    protected DALClass clsDAL = new DALClass();
    private string strReturnPage = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            strReturnPage = Request.QueryString["ReturnPage"].ToString();
            ViewState["ReturnPage"] = strReturnPage;

        }
        else
        {
            strReturnPage = (string)ViewState["ReturnPage"];
            ViewState["ReturnPage"] = strReturnPage;

        }
    }
    protected void LinkButtonRetLogin_Click(object sender, EventArgs e)
    {
        Response.Redirect("./Login.aspx?ReturnPage=" + strReturnPage, true);
    }
}
