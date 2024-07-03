using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

public partial class Backend_MainIndex : PageBase
{
    protected DALClass clsDAL = new DALClass();

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckUser();

        //Service1Client SC = new Service1Client();
        //Label1.Text = SC.GetBatchNO(DateTime.Now);
    }
}