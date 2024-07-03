using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class WebControls_PageControl : System.Web.UI.UserControl
{
    public delegate void DataBindDelegate();
    public event DataBindDelegate DataReBind;

    protected void Page_Load(object sender, EventArgs e)
    {
        ClientScriptTools.AddIntNumericTextScript(this.txtPageJumper);
        this.RefreshControl();
    }

    public virtual string Width
    {
        get 
        {
            if (this.ViewState["Width"] == null)
                this.ViewState["Width"] = "100%";

            this.ControlContainer.Width = this.ViewState["Width"].ToString();

            return this.ControlContainer.Width; 
        }
        set 
        {
            this.ViewState["Width"] = value;
            this.ControlContainer.Width = value; 
        }
    }

    public virtual int PageIndex
    {
        get 
        {
            if (this.ViewState["PageIndex"] == null)
                this.ViewState["PageIndex"] = "0";

            return int.Parse(this.ViewState["PageIndex"].ToString()); 
        }
        set
        {
            if (value >= 0 && value < this.PageCount)
                this.ViewState["PageIndex"] = value.ToString();
            else
            {
                if (value < 0)
                    this.ViewState["PageIndex"] = "0";
                
                if (value >= this.PageCount)
                {
                    if (this.PageCount > 0)
                        this.ViewState["PageIndex"] = (this.PageCount - 1).ToString();
                    else
                        this.ViewState["PageIndex"] = "0";
                }
            }

            this.RefreshControl();
        }
    }

    public virtual int RowCount
    {
        get 
        {
            if (this.ViewState["RowCount"] == null)
                this.ViewState["RowCount"] = "0";

            return int.Parse(this.ViewState["RowCount"].ToString()); 
        }
        set 
        {
            this.ViewState["RowCount"] = value.ToString();

            if (value <= 0)
                this.ViewState["RowCount"] = "0";

            if (this.PageIndex >= this.PageCount)
            {
                if (this.PageCount > 0)
                    this.PageIndex = this.PageCount - 1;
                else
                    this.PageIndex = 0;
            }

            this.RefreshControl();
        }
    }

    public virtual int PageCount
    {
        get { return (int)Math.Ceiling((double)this.RowCount / (int)this.PageSize); }
    }
    
    public virtual int PageSize
    {
        get
        {
            if (this.ViewState["PageSize"] == null)
                this.ViewState["PageSize"] = SiteSettings.PageSize.ToString();

            return int.Parse(this.ViewState["PageSize"].ToString());
        }
        set
        {
            this.ViewState["PageSize"] = value;
        }
    }

    public virtual string Message
    {
        get { return this.MessageBar1.Text; }
        set 
        {
            this.MessageBar1.Text = value;
            this.RefreshControl();
        }
    }

    protected virtual void RefreshControl()
    {
        this.MessageBar1.Visible = (this.MessageBar1.Text.Length > 0);

        this.lblRowCount.Text = this.RowCount.ToString();

        this.btnFirst.Enabled = (this.PageIndex > 0);
        this.btnPrior.Enabled = (this.PageIndex > 0);
        this.btnNext.Enabled = (this.PageIndex < this.PageCount - 1);
        this.btnLast.Enabled = (this.PageIndex < this.PageCount - 1);
        this.txtPageJumper.Enabled = (this.PageCount > 1);
        this.btnGoto.Enabled = (this.PageCount > 1);

        if (this.Page as PageBase != null)
        {
            this.lblRowCount.Text = String.Format("每頁{0}筆（共{1}筆）", this.PageSize, this.RowCount);
            this.lblPageCount.Text = String.Format("第{0}頁（共{1}頁）", (this.RowCount > 0) ? this.PageIndex + 1 : 0, this.PageCount);
        }
        else
        {
            this.lblRowCount.Text = String.Format("每頁{0}筆（共{1}筆）", this.PageSize, this.RowCount);
            this.lblPageCount.Text = String.Format("第{0}頁（共{1}頁）", (this.RowCount > 0) ? this.PageIndex + 1 : 0, this.PageCount);
        }
    }

    protected void btnPageJumper_Click(object sender, EventArgs e)
    {
        if (txtPageJumper.Text != "")
        {
            try
            {
                this.PageIndex = int.Parse(this.txtPageJumper.Text.Trim()) - 1;

                this.RefreshControl();

                if (this.DataReBind != null)
                    this.DataReBind();
            }
            finally
            {
                this.txtPageJumper.Text = "";
            }
        }
    }

    protected void btnFirst_Click(object sender, EventArgs e)
    {
        this.PageIndex = 0;

        this.RefreshControl();

        if (this.DataReBind != null)
            this.DataReBind();
    }

    protected void btnPrior_Click(object sender, EventArgs e)
    {
        --this.PageIndex;

        this.RefreshControl();

        if (this.DataReBind != null)
            this.DataReBind();
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        ++this.PageIndex;

        this.RefreshControl();

        if (this.DataReBind != null)
            this.DataReBind();
    }

    protected void btnLast_Click(object sender, EventArgs e)
    {
        this.PageIndex = this.PageCount - 1;

        this.RefreshControl();

        if (this.DataReBind != null)
            this.DataReBind();
    }
}
