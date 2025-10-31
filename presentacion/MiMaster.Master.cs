using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace presentacion
{
    public partial class MiMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Oculto NavBar en LogIn.aspx
            string currentPage = System.IO.Path.GetFileName(Request.Path);

            if(currentPage.Equals("LogIn.aspx", StringComparison.OrdinalIgnoreCase))
            {
                pnlNavbar.Visible = false;
            }
        }
    }
}