using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlTypes;
using System.Xml.Linq;
using System.Activities.Expressions;

public partial class Requisition_Update_Requisition : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["Ginie"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Session["UserId"] = "10021"; // amit user
            //Session["UserId"] = "10058"; // jayesh patil user
            //Session["UserRole"] = "Tech"; // user role

            BindUserRequisitions();

            //Search_DD_RequistionNo();
            //ddServiceName_Bind_Dropdown();

            ddServiceName_Bind_Dropdown();
            ddUOM_Bind_Dropdown();
        }
    }


    //=========================={ Paging & Alert }==========================
    protected void gridSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //binding GridView to PageIndex object
        gridSearch.PageIndex = e.NewPageIndex;

        DataTable pagination = (DataTable)Session["PaginationDataSource"];

        gridSearch.DataSource = pagination;
        gridSearch.DataBind();
    }

    private void alert(string mssg)
    {
        // alert pop - up with only message
        string message = mssg;
        string script = $"alert('{message}');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "messageScript", script, true);
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
    }





    //=========================={ Dropdown Bind }==========================
    private void ddServiceName_Bind_Dropdown()
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            //string sql = "select * from ServMaster891";

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






    //=========================={ Fetch Datatable }==========================
    private DataTable GetRequisitionDT(string ReqNo)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = "select * from Requisition1891 where ReqNo = @ReqNo";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@ReqNo", ReqNo);
            cmd.ExecuteNonQuery();

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            con.Close();

            return dt;
        }
    }

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




    //=========================={ Search Button Event }==========================
    protected void btnNewBill_Click(object sender, EventArgs e)
    {
        Response.Redirect("RequisitionNew.aspx");
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        //BindGridView();
    }



    private void BindUserRequisitions()
    {
        string userID = Session["UserID"].ToString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            string sql = $@"SELECT distinct req1.RefNo, req1.reqno, req1.ReqDte, org.OrgTyp, um.InstiName, 
                            (select count(*) from Requisition2891 as req2 where req2.BillRefNo = req1.RefNo) as Req2Count,
                            case when (select count(*) from billdocupload891 as doc where doc.BillRefNo = req1.RefNo) > 0
                            then 'Document Uploaded' else 'Document Not Uploaded' end as DocStatus
                            FROM Requisition1891 as req1 
                            INNER JOIN UserMaster891 as um ON req1.SaveBy = um.UserID 
                            INNER JOIN OrgType891 as org ON um.OrgType = org.RefID 
                            left join billdocupload891 as doc on doc.BillRefNo = req1.RefNo  
                            WHERE req1.SaveBy = @SaveBy Order By req1.RefNo desc";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@SaveBy", userID);
            cmd.ExecuteNonQuery();

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            con.Close();

            gridSearch.DataSource = dt;
            gridSearch.DataBind();

            Session["PaginationDataSource"] = dt;

            searchGridDiv.Visible = true;
            //divTopSearch.Visible = false;
            //UpdateDiv.Visible = true;
        }
    }





    //=========================={ Update - Fill Details }==========================
    protected void gridSearch_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "lnkView")
        {
            int rowId = Convert.ToInt32(e.CommandArgument);
            Session["ReqReferenceNo"] = rowId;

            searchGridDiv.Visible = false;
            divTopSearch.Visible = false;
            UpdateDiv.Visible = true;

            // check for req1 is done in update status or not
            CheckForRequisition1ForUpdateStatus(rowId.ToString());

            FillRequisitionDetails(rowId);

            FillItemDetails(rowId);
        }
    }

    private void CheckForRequisition1ForUpdateStatus(string req1RefNo)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            string sql = $@"select * 
                            from ReqReceived891 as rec 
                            inner join Requisition2891 as req2 on req2.RefNo = rec.Req2RefNo 
                            inner join Requisition1891 as req1 on req2.BillRefNo = req1.RefNo
                            where req1.RefNo = @RefNo";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@RefNo", req1RefNo);
            cmd.ExecuteNonQuery();

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                // insert item UI hidden
                itemInsertDiv.Visible = false;

                getSweetAlertSuccessRedirectMandatory("Availability Status Done", $"The Availability Status Of Requisition: {req1RefNo} has Been Done. Hence It Is Only In View Mode", "#");
            }
        }
    }

    private void FillRequisitionDetails(int requisitionRefNo)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            string sql = $@"select * 
                            from Requisition1891 as req1 
                            INNER JOIN UserMaster891 as um ON req1.SaveBy = um.UserID 
                            INNER JOIN OrgType891 as org ON um.OrgType = org.RefID 
                            where req1.RefNo = @RefNo";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@RefNo", requisitionRefNo);
            cmd.ExecuteNonQuery();

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            con.Close();


            txtReqNo.Text = dt.Rows[0]["ReqNo"].ToString();
            OrgType.Text = dt.Rows[0]["OrgTyp"].ToString();
            InstituteName.Text = dt.Rows[0]["InstiName"].ToString();

            DateTime reqDate = DateTime.Parse(dt.Rows[0]["ReqDte"].ToString());
            dtReqDate.Text = reqDate.ToString("yyyy-MM-dd");
        }
    }

    private void FillItemDetails(int requisitionRefNo)
    {
        itemDiv.Visible = true;

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = "select * from Requisition2891 where BillRefNo = @BillRefNo";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@BillRefNo", requisitionRefNo.ToString());
            cmd.ExecuteNonQuery();

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            con.Close();

            itemGrid.DataSource = dt;
            itemGrid.DataBind();

            ViewState["ReqDetailsVS"] = dt;
            Session["ReqDetails"] = dt;
        }
    }




    //=========================={ Item Save Button Click Event }==========================
    protected void btnItemInsert_Click(object sender, EventArgs e)
    {
        // inserting bill details
        insertBillDetails();
    }

    private void insertBillDetails()
    {
        string BillReferenceNo = Session["ReqReferenceNo"].ToString();

        string serviceName = ddServiceName.SelectedValue ?? "";
        string nameCellLine = CellLineName.Text.ToString();
        string uom = ddUOM.SelectedValue;
        string quantity = Quantity.Text.ToString();
        string commentsIfAny = CommentsIfAny.Text.ToString() ?? "";

        DataTable dt = ViewState["ReqDetailsVS"] as DataTable ?? createDatatable();

        AddRowToDataTable(dt, BillReferenceNo, serviceName, nameCellLine, uom, quantity, commentsIfAny);

        ViewState["ReqDetailsVS"] = dt;
        Session["ReqDetails"] = dt;

        if (dt.Rows.Count > 0)
        {
            itemDiv.Visible = true;

            itemGrid.DataSource = dt;
            itemGrid.DataBind();

            ddServiceName.SelectedIndex = 0;
            ddUOM.SelectedIndex = 0;
            CellLineName.Text = string.Empty;
            Quantity.Text = string.Empty;
            CommentsIfAny.Text = string.Empty;
        }
    }

    private DataTable createDatatable()
    {
        DataTable dt = new DataTable();

        // reference no
        DataColumn BillRefNo = new DataColumn("BillRefNo", typeof(string));
        dt.Columns.Add(BillRefNo);

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

    private void AddRowToDataTable(DataTable dt, string serviceName, string BillReferenceNo, string nameCellLine, string uom, string quantity, string commentsIfAny)
    {
        // Create a new row
        DataRow row = dt.NewRow();

        // Set values for the new row
        row["BillRefNo"] = BillReferenceNo;
        row["ServiceName"] = serviceName;
        row["NmeCell"] = nameCellLine;
        row["UOM"] = uom;
        row["Quty"] = quantity;
        row["Comment"] = commentsIfAny;

        // Add the new row to the DataTable
        dt.Rows.Add(row);
    }





    //=========================={ Submit Button Click Event }==========================
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
                    string reqReferenceNo = Session["ReqReferenceNo"].ToString();

                    // updating bill head
                    //UpdateBillHeader();

                    // updating item details
                    UpdateBillItemDetails(reqReferenceNo, con, transaction);

                    if (transaction != null) transaction.Commit();

                    getSweetAlertSuccessRedirectMandatory("Updated!", "Updated successfully", "Requisition.aspx");
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




    // requisition item details
    private void UpdateBillItemDetails(string reqReferenceNo, SqlConnection con, SqlTransaction transaction)
    {
        string userID = Session["UserID"].ToString();

        string reqlRefno = reqReferenceNo;

        DataTable billItemsDT = (DataTable)Session["ReqDetails"]; // requisition 2 details

        foreach (GridViewRow row in itemGrid.Rows)
        {
            int rowIndex = row.RowIndex;

            string serviceCode = billItemsDT.Rows[rowIndex]["ServiceName"].ToString();
            string cellEnlistName = billItemsDT.Rows[rowIndex]["NmeCell"].ToString();
            string uom = billItemsDT.Rows[rowIndex]["UOM"].ToString();
            string qty = billItemsDT.Rows[rowIndex]["Quty"].ToString();
            string comments = billItemsDT.Rows[rowIndex]["Comment"].ToString();

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

            string itemRefNo = billItemsDT.Rows[rowIndex]["RefNo"].ToString();
            bool isItemExists = IsItemExists(itemRefNo, con, transaction);

            if (isItemExists) // true - update
            {

            }
            else // false - insert
            {
                // getting new ref id for item
                string itemRefNoNew = GetItemRefNo(con, transaction);

                string sql = $@"INSERT INTO Requisition2891 
                                    (RefNo, BillRefNo, ServiceName, NmeCell, UOM, Quty, Comment, SubItemPrice, TaxApplied, OrgType, ServicePrice, SaveBy) 
                                    VALUES 
                                    (@RefNo, @BillRefNo, @ServiceName, @NmeCell, @UOM, @Quty, @Comment, @SubItemPrice, @TaxApplied, @OrgType, @ServicePrice, @SaveBy)";

                SqlCommand cmd = new SqlCommand(sql, con, transaction);
                cmd.Parameters.AddWithValue("@RefNo", itemRefNoNew);
                cmd.Parameters.AddWithValue("@BillRefNo", reqlRefno);
                cmd.Parameters.AddWithValue("@ServiceName", serviceCode);
                cmd.Parameters.AddWithValue("@NmeCell", cellEnlistName);
                cmd.Parameters.AddWithValue("@UOM", uom);
                cmd.Parameters.AddWithValue("@Quty", qty);
                cmd.Parameters.AddWithValue("@Comment", comments);
                cmd.Parameters.AddWithValue("@SubItemPrice", subItemPrice);
                cmd.Parameters.AddWithValue("@TaxApplied", taxApplied);
                cmd.Parameters.AddWithValue("@OrgType", orgType);
                cmd.Parameters.AddWithValue("@ServicePrice", servicePrice);
                cmd.Parameters.AddWithValue("@SaveBy", userID);
                cmd.ExecuteNonQuery();
            }
        }
    }

    private bool IsItemExists(string itemRefNo, SqlConnection con, SqlTransaction transaction)
    {
        string sql = "select * from Requisition2891 where RefNo = @RefNo";

        SqlCommand cmd = new SqlCommand(sql, con, transaction);
        cmd.Parameters.AddWithValue("@RefNo", itemRefNo);
        cmd.ExecuteNonQuery();

        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        ad.Fill(dt);

        if (dt.Rows.Count > 0) return true;
        else return false;
    }

    private string GetItemRefNo(SqlConnection con, SqlTransaction transaction)
    {
        string nextRefNo = "1000001";

        string sql = "SELECT ISNULL(MAX(CAST(RefNo AS INT)), 10000) + 1 AS NextRefNo FROM Requisition2891";
        SqlCommand cmd = new SqlCommand(sql, con, transaction);

        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        ad.Fill(dt);

        if (dt.Rows.Count > 0) return dt.Rows[0]["NextRefNo"].ToString();
        else return nextRefNo;
    }


}