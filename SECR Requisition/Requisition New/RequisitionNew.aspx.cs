using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Requisition_New_RequisitionNew : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["Ginie"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        // NCCS - 891

        if (!IsPostBack)
        {
            ReqDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            ddServiceName_Bind_Dropdown();
            ddUOM_Bind_Dropdown();

            //Session["UserId"] = "10021"; // amit user
            //Session["UserId"] = "10031"; // krunal user
            //Session["UserRole"] = "Tech"; // user role

        }
    }


    //=========================={ Paging & Alert }==========================
    private void alert(string mssg)
    {
        // alert pop - up with only message
        string message = mssg;
        string script = $"alert('{message}');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "messageScript", script, true);
    }




    //=========================={ Dropdown Bind }==========================
    private void ddServiceName_Bind_Dropdown()
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sqlxx = $@"select * 
                            from ServMaster891 as serv
                            Inner Join PriceList891 as price ON serv.RefID = price.SerName";

            string sql = $@"select * 
                            from ServMaster891 as serv
                            Inner Join PriceList891 as price ON serv.RefID = price.RefID";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.ExecuteNonQuery();

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            con.Close();

            ddServiceName.DataSource = dt;
            ddServiceName.DataTextField = "ServName";
            ddServiceName.DataValueField = "RefID";
            ddServiceName.DataBind();
            ddServiceName.Items.Insert(0, new ListItem("------- Select Service Name -------", "0"));
        }
    }

    private void ddUOM_Bind_Dropdown()
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = "select * from UOMs891";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.ExecuteNonQuery();

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            con.Close();

            ddUOM.DataSource = dt;
            ddUOM.DataTextField = "umName";
            ddUOM.DataValueField = "UmId";
            ddUOM.DataBind();
            ddUOM.Items.Insert(0, new ListItem("------- Select UOM -------", "0"));
        }
    }





    //=========================={ Fetch Reference Numbers }==========================
    private string GetRequisitionReferenceNumber(SqlConnection con, SqlTransaction transaction)
    {
        string nextRefNo = "1000001";

        string sql = "SELECT ISNULL(MAX(CAST(RefNo AS INT)), 1000000) + 1 AS NextRefNo FROM Requisition1891";
        SqlCommand cmd = new SqlCommand(sql, con, transaction);

        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        ad.Fill(dt);

        if (dt.Rows.Count > 0) return dt.Rows[0]["NextRefNo"].ToString();
        return nextRefNo;
    }

    private string GetItemRefNo(SqlConnection con, SqlTransaction transaction)
    {
        string nextRefNo = "1000001";

        string sql = "SELECT ISNULL(MAX(CAST(RefNo AS INT)), 1000000) + 1 AS NextRefNo FROM Requisition2891";
        SqlCommand cmd = new SqlCommand(sql, con, transaction);

        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        ad.Fill(dt);

        if (dt.Rows.Count > 0) return dt.Rows[0]["NextRefNo"].ToString();
        return nextRefNo;
    }

    private string GetDocumentRefNo(SqlConnection con, SqlTransaction transaction)
    {
        string nextRefNo = "1000001";

        string sql = "SELECT ISNULL(MAX(CAST(RefNo AS INT)), 1000000) + 1 AS NextRefNo FROM BillDocUpload891";
        SqlCommand cmd = new SqlCommand(sql, con, transaction);

        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        ad.Fill(dt);

        if (dt.Rows.Count > 0) return dt.Rows[0]["NextRefNo"].ToString();
        return nextRefNo;
    }




    //=========================={ Fetch Data }==========================
    private DataTable GetLoggedInUserDetails(SqlConnection con, SqlTransaction transaction)
    {
        string userRole = Session["UserRole"].ToString();
        string userID = Session["UserID"].ToString();

        string sql = "select * from UserMaster891 where UserID = @UserID";
        SqlCommand cmd = new SqlCommand(sql, con, transaction);
        cmd.Parameters.AddWithValue("@UserID", userID);
        cmd.ExecuteNonQuery();

        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        ad.Fill(dt);

        return dt;
    }

    private DataTable GetServicePriceDetails(string serviceCode, SqlConnection con, SqlTransaction transaction)
    {
        string sql = "select top 1 * from PriceList891 where RefID = @RefID order by SaveOn desc";
        SqlCommand cmd = new SqlCommand(sql, con, transaction);
        cmd.Parameters.AddWithValue("@RefID", serviceCode);
        cmd.ExecuteNonQuery();

        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        ad.Fill(dt);

        return dt;
    }





    //=========================={ Sweet Alert JS }==========================
    private void getSweetAlertSuccessOnly()
    {
        string title = "Saved!";
        string message = "Record saved successfully!";
        string icon = "success";
        string confirmButtonText = "OK";

        string sweetAlertScript =
            $@"<script>
                Swal.fire({{ 
                    title: '{title}', 
                    text: '{message}', 
                    icon: '{icon}', 
                    confirmButtonText: '{confirmButtonText}' 
                }});
            </script>";
        ClientScript.RegisterStartupScript(this.GetType(), "sweetAlert", sweetAlertScript, false);
    }

    // sweet alert - success redirect
    private void getSweetAlertSuccessRedirect(string redirectUrl)
    {
        string title = "Saved!";
        string message = "Record saved successfully!";
        string icon = "success";
        string confirmButtonText = "OK";
        string allowOutsideClick = "false";

        string sweetAlertScript =
            $@"<script>
                Swal.fire({{ 
                    title: '{title}', 
                    text: '{message}', 
                    icon: '{icon}', 
                    confirmButtonText: '{confirmButtonText}',
                    allowOutsideClick: {allowOutsideClick}
                }}).then((result) => {{
                    if (result.isConfirmed) {{
                        window.location.href = '{redirectUrl}';
                    }}
                }});
            </script>";
        ClientScript.RegisterStartupScript(this.GetType(), "sweetAlert", sweetAlertScript, false);
    }

    // sweet alert - success redirect block
    private void getSweetAlertSuccessRedirectMandatory(string titles, string mssg, string redirectUrl)
    {
        string title = titles;
        string message = mssg;
        string icon = "success";
        string confirmButtonText = "OK";
        string allowOutsideClick = "false"; // Prevent closing on outside click

        string sweetAlertScript =
        $@"<script>
            Swal.fire({{ 
                title: '{title}', 
                text: '{message}', 
                icon: '{icon}', 
                confirmButtonText: '{confirmButtonText}', 
                allowOutsideClick: {allowOutsideClick}
            }}).then((result) => {{
                if (result.isConfirmed) {{
                    window.location.href = '{redirectUrl}';
                }}
            }});
        </script>";
        ClientScript.RegisterStartupScript(this.GetType(), "sweetAlert", sweetAlertScript, false);
    }

    // sweet alert - error only block
    private void getSweetAlertErrorMandatory(string titles, string mssg)
    {
        string title = titles;
        string message = mssg;
        string icon = "error";
        string confirmButtonText = "OK";
        string allowOutsideClick = "false"; // Prevent closing on outside click

        string sweetAlertScript =
        $@"<script>
            Swal.fire({{ 
                title: '{title}', 
                text: '{message}', 
                icon: '{icon}', 
                confirmButtonText: '{confirmButtonText}', 
                allowOutsideClick: {allowOutsideClick}
            }});
        </script>";
        ClientScript.RegisterStartupScript(this.GetType(), "sweetAlert", sweetAlertScript, false);
        //ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "sweetAlert", sweetAlertScript, false);
    }

    // html no redirect
    private void getSweetHTML(string titles, string mssg)
    {
        string title = titles;
        string message = mssg;
        string icon = "info";
        string confirmButtonText = "OK";
        string allowOutsideClick = "false"; // Prevent closing on outside click

        // Create a placeholder textarea for user input
        string sweetAlertScript = $@"
            <script>
                Swal.fire({{
                    title: '{title}',
                    html: '{message}',
                    icon: '{icon}',
                    confirmButtonText: '{confirmButtonText}',
                    allowOutsideClick: {allowOutsideClick}
                }})
            </script>";

        // Register the script
        ClientScript.RegisterStartupScript(this.GetType(), "sweetAlertWithTextarea", sweetAlertScript, false);
    }

    // html with redirect
    private void getSweetHTMLRedirect(string titles, string mssg, string redirectUrl)
    {
        string title = titles;
        string message = mssg;
        string icon = "info";
        string confirmButtonText = "OK";
        string allowOutsideClick = "false"; // Prevent closing on outside click

        // Create a placeholder textarea for user input
        string sweetAlertScript = $@"
            <script>
                Swal.fire({{
                    title: '{title}',
                    html: '{message}',
                    icon: '{icon}',
                    confirmButtonText: '{confirmButtonText}',
                    allowOutsideClick: {allowOutsideClick}
                }}).then((result) => {{
                    if (result.isConfirmed) {{
                        window.location.href = '{redirectUrl}';
                    }}
                }});
            </script>";

        // Register the script
        ClientScript.RegisterStartupScript(this.GetType(), "sweetAlertWithTextarea", sweetAlertScript, false);
    }


    //=========================={ GridView RowDeleting }==========================

    protected void Grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView gridView = (GridView)sender;

        // item gridview
        if (gridView.ID == "itemGrid")
        {
            int rowIndex = e.RowIndex;

            DataTable dt = ViewState["ItemDetails_VS"] as DataTable;

            if (dt != null && dt.Rows.Count > rowIndex)
            {
                dt.Rows.RemoveAt(rowIndex);

                ViewState["ItemDetails_VS"] = dt;
                Session["ItemDetails"] = dt;

                itemGrid.DataSource = dt;
                itemGrid.DataBind();

                // re-calculating total amount n assigning back to textbox
                //double? totalBillAmount = dt.AsEnumerable().Sum(row => row["Amount"] is DBNull ? (double?)null : Convert.ToDouble(row["Amount"])) ?? 0.0;
                //txtBillAmount.Text = totalBillAmount.HasValue ? totalBillAmount.Value.ToString("N2") : "0.00";
            }
        }

        // document gridview
        //if (gridView.ID == "GridDocument")
        //{
        //    int rowIndex = e.RowIndex;

        //    DataTable dt = ViewState["DocDetails_VS"] as DataTable;

        //    if (dt != null && dt.Rows.Count > rowIndex)
        //    {
        //        dt.Rows.RemoveAt(rowIndex);

        //        ViewState["DocDetails_VS"] = dt;
        //        Session["DocUploadDT"] = dt;

        //        GridDocument.DataSource = dt;
        //        GridDocument.DataBind();
        //    }
        //}
    }




    //----------============={ Item Add Button Event }=============----------
    protected void btnItemInsert_Click(object sender, EventArgs e)
    {
        InsertItemEntry();
    }

    private void InsertItemEntry()
    {
        string serviceName = ddServiceName.SelectedValue ?? "";
        string nameCellLine = CellLineName.Text.ToString();
        string uom = ddUOM.SelectedValue;
        string quantity = Quantity.Text.ToString();
        string commentsIfAny = CommentsIfAny.Text.ToString() ?? "";

        DataTable dt = ViewState["ItemDetails_VS"] as DataTable ?? createItemDatatable();

        AddRowToItemDataTable(dt, serviceName, nameCellLine, uom, quantity, commentsIfAny);

        if (dt.Rows.Count > 0)
        {
            itemDiv.Visible = true;

            itemGrid.DataSource = dt;
            itemGrid.DataBind();

            ViewState["ItemDetails_VS"] = dt;
            Session["ItemDetails"] = dt;

            ddServiceName.SelectedIndex = 0;
            ddUOM.SelectedIndex = 0;
            CellLineName.Text = string.Empty;
            Quantity.Text = string.Empty;
            CommentsIfAny.Text = string.Empty;
        }
    }

    private DataTable createItemDatatable()
    {
        DataTable dt = new DataTable();

        // service name
        DataColumn ServiceName = new DataColumn("ServiceName", typeof(string));
        dt.Columns.Add(ServiceName);

        // cell name
        DataColumn NmeCell = new DataColumn("NmeCell", typeof(string));
        dt.Columns.Add(NmeCell);

        // uom
        DataColumn UOM = new DataColumn("UOM", typeof(string));
        dt.Columns.Add(UOM);

        // quantity
        DataColumn Quty = new DataColumn("Quty", typeof(string));
        dt.Columns.Add(Quty);

        // comments
        DataColumn Comment = new DataColumn("Comment", typeof(string));
        dt.Columns.Add(Comment);

        return dt;
    }

    private void AddRowToItemDataTable(DataTable dt, string serviceName, string nameCellLine, string uom, string quantity, string commentsIfAny)
    {
        // Create a new row
        DataRow row = dt.NewRow();

        // Set values for the new row
        //row["ServiceName"] = serviceName;

        if (serviceName == "0") row["ServiceName"] = "";
        else row["ServiceName"] = serviceName;

        row["NmeCell"] = nameCellLine;
        row["UOM"] = uom;
        row["Quty"] = quantity;
        row["Comment"] = commentsIfAny;

        // Add the new row to the DataTable
        dt.Rows.Add(row);
    }




    //----------============={ Submit Button Event }=============----------
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Requisition.aspx");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (itemGrid.Rows.Count > 0)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    string requisitionRefNo = GetRequisitionReferenceNumber(con, transaction);

                    ReqNo.Text = requisitionRefNo;
                    Session["Requisition_RefNo"] = requisitionRefNo;

                    // checking for customer profile details
                    //bool isCustomerDataPresent = CheckCustomerDetailsComplete(con, transaction);

                    if (true)
                    {
                        // inserting bill head
                        InsertRequisitionHeader(requisitionRefNo, con, transaction);

                        // inserting item details from grid
                        InsertReqDetails(requisitionRefNo, con, transaction);

                        if (transaction != null) transaction.Commit();

                        btnSubmit.Enabled = false;

                        string userID = Session["UserID"].ToString();

                        //getSweetAlertSuccessRedirectMandatory("Saved Successfully", $"Your Requisition No: {requisitionRefNo} Saved Successfully", "Requisition.aspx");
                        string documentLink = $@"http://101.53.144.92/nccs/Ginie/Render/Declaration?userID={userID}";
                        //string redirect = $@"http://101.53.144.92/nccs/Ginie/External?Url=../Portal/Document/RequisitionDocs.aspx";

                        
                        string redirect = $@"http://101.53.144.92/nccs/Ginie/External?Url=../Portal/Document/RequisitionDocs.aspx";

                        string html = $@"Your Requisition No: {requisitionRefNo} Saved Successfully. <br/> <br/>";
                        html += $@"Kindly Click On <a href=""{documentLink}"" target=""_blank"">Download Document</a>, <br/> <br/>";
                        html += $@"In Order To Re-Upload It In ""<b>Confirm Your Order With Document</b>"" Section To Confirm Your Order!";

                        getSweetHTMLRedirect("Saved Successfully", html, redirect);
                    }
                    else
                    {
                        getSweetAlertErrorMandatory("No Service Found", "Please Addd Minimum One Service To Proceed Further");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
                finally
                {
                    con.Close();
                    transaction.Dispose();
                }
            }
        }
        else
        {
            getSweetAlertErrorMandatory("No Service Found", "Please Addd Minimum One Service To Proceed Further");
        }
    }


    private void InsertRequisitionHeader(string requisitionRefNo, SqlConnection con, SqlTransaction transaction)
    {
        // saveby: userID
        string userID = Session["UserID"].ToString();

        // requisition date
        DateTime requisitionDate = DateTime.Parse(ReqDate.Text);

        // fetching other details
        DataTable userDT = GetLoggedInUserDetails(con, transaction);
        string orgType = userDT.Rows[0]["OrgType"].ToString();
        string taxApplied = "";

        // Academic     - 1000001
        // Non-Academic - 1000002

        if (orgType == "1000001") taxApplied = "NO";
        else taxApplied = "YES";


        string sql = $@"INSERT INTO Requisition1891 (RefNo, ReqNo, ReqDte, TaxApplied, SaveBy) 
                        VALUES (@RefNo, @ReqNo, @ReqDte, @TaxApplied, @SaveBy)";
        SqlCommand cmd = new SqlCommand(sql, con, transaction);
        cmd.Parameters.AddWithValue("@RefNo", requisitionRefNo);
        cmd.Parameters.AddWithValue("@ReqNo", requisitionRefNo);
        cmd.Parameters.AddWithValue("@ReqDte", requisitionDate);
        cmd.Parameters.AddWithValue("@TaxApplied", taxApplied);
        cmd.Parameters.AddWithValue("@SaveBy", userID);
        cmd.ExecuteNonQuery();

        //SqlDataAdapter ad = new SqlDataAdapter(cmd);
        //DataTable dt = new DataTable();
        //ad.Fill(dt);
    }

    private void InsertReqDetails(string requisitionRefNo, SqlConnection con, SqlTransaction transaction)
    {
        string userID = Session["UserID"].ToString();

        DataTable itemsDT = (DataTable)Session["ItemDetails"];


        foreach (DataRow row in itemsDT.Rows)
        {
            string reqNo = GetItemRefNo(con, transaction); // new requisition ref no

            // item items
            string serviceCode = row["ServiceName"].ToString() ?? string.Empty;
            string cellEnlistName = row["NmeCell"].ToString();
            string uom = row["UOM"].ToString();
            string qty = row["Quty"].ToString();
            string comments = row["Comment"].ToString() ?? string.Empty;


            // fetching other details
            DataTable userDT = GetLoggedInUserDetails(con, transaction);
            DataTable servicePriceDT = GetServicePriceDetails(serviceCode, con, transaction);

            string orgType = userDT.Rows[0]["OrgType"].ToString();

            string taxApplied = "";
            double servicePrice = 0.00;


            // Academic     - 1000001
            // Non-Academic - 1000002

            if (orgType == "1000001") taxApplied = "NO";
            else taxApplied = "YES";

            if (orgType == "1000001") servicePrice = Convert.ToDouble(servicePriceDT.Rows[0]["AcadPrice"]);
            else servicePrice = Convert.ToDouble(servicePriceDT.Rows[0]["NonAcadPrice"]);

            // initial total service price == 0
            double subItemPrice = 0.00;

            // inserting bill item details
            string sql = $@"INSERT INTO Requisition2891 
                            (RefNo, BillRefNo, ServiceName, NmeCell, UOM, Quty, Comment, TaxApplied, OrgType, ServicePrice, SubItemPrice, SaveBy) 
                            VALUES 
                            (@RefNo, @BillRefNo, @ServiceName, @NmeCell, @UOM, @Quty, @Comment, @TaxApplied, @OrgType, @ServicePrice, @SubItemPrice, @SaveBy)";

            SqlCommand cmd = new SqlCommand(sql, con, transaction);
            cmd.Parameters.AddWithValue("@RefNo", reqNo);
            cmd.Parameters.AddWithValue("@BillRefNo", requisitionRefNo);
            cmd.Parameters.AddWithValue("@ServiceName", serviceCode);
            cmd.Parameters.AddWithValue("@NmeCell", cellEnlistName);
            cmd.Parameters.AddWithValue("@UOM", uom);
            cmd.Parameters.AddWithValue("@Quty", qty);
            cmd.Parameters.AddWithValue("@Comment", comments);
            cmd.Parameters.AddWithValue("@TaxApplied", taxApplied);
            cmd.Parameters.AddWithValue("@OrgType", orgType);
            cmd.Parameters.AddWithValue("@SubItemPrice", subItemPrice);
            cmd.Parameters.AddWithValue("@ServicePrice", servicePrice);
            cmd.Parameters.AddWithValue("@SaveBy", userID);
            cmd.ExecuteNonQuery();
        }

    }




}