using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Test_Test : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["Ginie"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // SECR = 751
            // NCCS = 891
            // MPKV = ?

            // get all table columns
            //DataTable dtResp = GetResponsibilitiesData();
            //DynamicGridView(dtResp);

            // get current loggedin user role
            string userRole = Session["UserRole"].ToString();
            string userID = Session["UserId"].ToString();

            alert($"userRole : {userRole}");

            // get all avaiable sessions
            //DataTable dtSessions = GetSessionsData();
            //DynamicGridView_Session(dtSessions);
        }
    }

    private void alert(string mssg)
    {
        // alert pop - up with only message
        string message = mssg;
        string script = $"alert('{message}');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "messageScript", script, true);
    }




    private DataTable GetResponsibilitiesData()
    {
        string userID = Session["UserId"].ToString();


        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = "select * from Requisition1891 where SaveBy = @SaveBy order by ReqNo desc";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@SaveBy", userID);
            cmd.ExecuteNonQuery();

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            con.Close();

            return dt;
        }
    }

    protected void DynamicGridView(DataTable dtResp)
    {
        if (dtResp.Rows.Count > 0)
        {
            // turning OFF column auto generation
            GridDyanmic.AutoGenerateColumns = true;

            // assigning data source to GridView
            GridDyanmic.DataSource = dtResp;
            GridDyanmic.DataBind();

            // Clear existing columns
            GridDyanmic.Columns.Clear();

            // Dynamically creating BoundFields or Columns using from the data source
            foreach (DataColumn col in dtResp.Columns)
            {
                BoundField boundField = new BoundField();
                boundField.DataField = col.ColumnName;
                boundField.HeaderText = col.ColumnName;
                GridDyanmic.Columns.Add(boundField);
            }

            // turning ON column auto generation
            GridDyanmic.AutoGenerateColumns = false;
        }
    }






    private DataTable GetSessionsData()
    {
        DataTable dtSessions = new DataTable();
        dtSessions.Columns.Add("SessionName");
        dtSessions.Columns.Add("SessionValue");

        // Loop through all sessions and add them to the DataTable
        foreach (string sessionName in Session.Contents)
        {
            DataRow row = dtSessions.NewRow();
            row["SessionName"] = sessionName;
            row["SessionValue"] = Session[sessionName].ToString();
            dtSessions.Rows.Add(row);
        }

        return dtSessions;
    }

    protected void DynamicGridView_Session(DataTable dtSessions)
    {
        if (dtSessions.Rows.Count > 0)
        {
            // turning OFF column auto generation
            GridDyanmic.AutoGenerateColumns = true;

            // assigning data source to GridView
            GridDyanmic.DataSource = dtSessions;
            GridDyanmic.DataBind();

            // Clear existing columns
            GridDyanmic.Columns.Clear();

            // Dynamically creating BoundFields or Columns using from the data source
            foreach (DataColumn col in dtSessions.Columns)
            {
                BoundField boundField = new BoundField();
                boundField.DataField = col.ColumnName;
                boundField.HeaderText = col.ColumnName;
                GridDyanmic.Columns.Add(boundField);
            }

            // turning ON column auto generation
            GridDyanmic.AutoGenerateColumns = false;
        }
    }
}