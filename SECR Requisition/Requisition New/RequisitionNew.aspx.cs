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




    //=========================={ Paging & Alert }==========================
    private void ddServiceName_Bind_Dropdown()
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = "select * from ServMaster891";
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





    //=========================={ Fetch Reference Numbers }==========================
    private int GetRequisitionReferenceNumber()
    {
        string nextRefID = "1000001";

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = "SELECT ISNULL(MAX(CAST(RefNo AS INT)), 1000000) + 1 AS NextRefID FROM Requisition1891";
            SqlCommand cmd = new SqlCommand(sql, con);

            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value) { nextRefID = result.ToString(); }
            return Convert.ToInt32(nextRefID);
        }
    }

    private int GetItemRefNo()
    {
        string nextRefID = "1000001";

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = "SELECT ISNULL(MAX(CAST(RefNo AS INT)), 1000000) + 1 AS NextRefID FROM Requisition2891";
            SqlCommand cmd = new SqlCommand(sql, con);

            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value) { nextRefID = result.ToString(); }
            return Convert.ToInt32(nextRefID);
        }
    }

    private int GetDocumentRefNo()
    {
        string nextRefID = "1000001";

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = "SELECT ISNULL(MAX(CAST(RefNo AS INT)), 1000000) + 1 AS NextRefID FROM BillDocUpload891";
            SqlCommand cmd = new SqlCommand(sql, con);

            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value) { nextRefID = result.ToString(); }
            return Convert.ToInt32(nextRefID);
        }
    }




    //=========================={ Fetch Data }==========================
    private DataTable GetLoggedInUserDetails()
    {
        string userRole = Session["UserRole"].ToString();
        string userID = Session["UserID"].ToString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = "select * from UserMaster891 where UserID = @UserID";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.ExecuteNonQuery();

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            con.Close();

            return dt;
        }
    }

    private DataTable GetServicePriceDetails(string serviceCode)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = "select top 1 * from PriceList891 where SerName = @SerName order by Date desc";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@SerName", serviceCode);
            cmd.ExecuteNonQuery();

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            con.Close();

            return dt;
        }
    }





    //=========================={ Sweet Alert JS }==========================
    // sweet alert - success only
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
        if (gridView.ID == "GridDocument")
        {
            int rowIndex = e.RowIndex;

            DataTable dt = ViewState["DocDetails_VS"] as DataTable;

            if (dt != null && dt.Rows.Count > rowIndex)
            {
                dt.Rows.RemoveAt(rowIndex);

                ViewState["DocDetails_VS"] = dt;
                Session["DocUploadDT"] = dt;

                GridDocument.DataSource = dt;
                GridDocument.DataBind();
            }
        }
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
        string quantity = Quantity.Text.ToString();
        string commentsIfAny = CommentsIfAny.Text.ToString() ?? "";

        DataTable dt = ViewState["ItemDetails_VS"] as DataTable ?? createItemDatatable();

        AddRowToItemDataTable(dt, serviceName, nameCellLine, quantity, commentsIfAny);

        if (dt.Rows.Count > 0)
        {
            itemDiv.Visible = true;

            itemGrid.DataSource = dt;
            itemGrid.DataBind();

            ViewState["ItemDetails_VS"] = dt;
            Session["ItemDetails"] = dt;

            ddServiceName.SelectedIndex = 0;
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

        // quantity
        DataColumn Quty = new DataColumn("Quty", typeof(string));
        dt.Columns.Add(Quty);

        // comments
        DataColumn Comment = new DataColumn("Comment", typeof(string));
        dt.Columns.Add(Comment);

        return dt;
    }

    private void AddRowToItemDataTable(DataTable dt, string serviceName, string nameCellLine, string quantity, string commentsIfAny)
    {
        // Create a new row
        DataRow row = dt.NewRow();

        // Set values for the new row
        //row["ServiceName"] = serviceName;

        if (serviceName == "0") row["ServiceName"] = "";
        else row["ServiceName"] = serviceName;

        row["NmeCell"] = nameCellLine;
        row["Quty"] = quantity;
        row["Comment"] = commentsIfAny;

        // Add the new row to the DataTable
        dt.Rows.Add(row);
    }


    protected void DynamicGridView(DataTable dtResp)
    {
        if (dtResp.Rows.Count > 0)
        {
            // Clear existing columns
            GridDyanmic.Columns.Clear();

            // turning OFF column auto generation
            GridDyanmic.AutoGenerateColumns = true;

            // assigning data source to GridView
            GridDyanmic.DataSource = dtResp;
            GridDyanmic.DataBind();

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





    //----------============={ Upload Documents }=============----------
    protected void btnDocUpload_Click(object sender, EventArgs e)
    {
        // setting the file size in web.config file (web.config should not be read only)
        //settingHttpRuntimeForFileSize();

        if (fileDoc.HasFile)
        {
            string FileExtension = System.IO.Path.GetExtension(fileDoc.FileName);

            if (FileExtension == ".xlsx" || FileExtension == ".xls")
            {

            }

            // file name
            string onlyFileNameWithExtn = fileDoc.FileName.ToString();

            // getting unique file name
            string strFileName = GenerateUniqueId(onlyFileNameWithExtn);

            // saving and getting file path
            string filePath = getServerFilePath(strFileName);

            // Retrieve DataTable from ViewState or create a new one
            DataTable dt = ViewState["DocDetails_VS"] as DataTable ?? CreateDocDetailsDataTable();

            // filling document details datatable
            AddRowToDocDetailsDataTable(dt, onlyFileNameWithExtn, filePath);

            // Save DataTable to ViewState
            ViewState["DocDetails_VS"] = dt;
            Session["DocUploadDT"] = dt;

            if (dt.Rows.Count > 0)
            {
                docGrid.Visible = true;

                // binding document details gridview
                GridDocument.DataSource = dt;
                GridDocument.DataBind();
            }
        }
    }

    private string GenerateUniqueId(string strFileName)
    {
        long timestamp = DateTime.Now.Ticks;
        //string guid = Guid.NewGuid().ToString("N"); //N to remove hypen "-" from GUIDs
        string guid = Guid.NewGuid().ToString();
        string uniqueID = timestamp + "_" + guid + "_" + strFileName;
        return uniqueID;
    }

    private string getServerFilePath(string strFileName)
    {
        string orgFilePath = Server.MapPath("~/Portal/Public/" + strFileName);

        // saving file
        fileDoc.SaveAs(orgFilePath);

        //string filePath = Server.MapPath("~/Portal/Public/" + strFileName);
        //file:///C:/HostingSpaces/PAWAN/cdsmis.in/wwwroot/Pms2/Portal/Public/638399011215544557_926f9320-275e-49ad-8f59-32ecb304a9f1_EMB%20Recording.pdf

        // replacing server-specific path with the desired URL
        string baseUrl = "http://101.53.144.92/nccs/Ginie/External?url=..";
        string relativePath = orgFilePath.Replace(Server.MapPath("~/Portal/Public/"), "Portal/Public/");

        // Full URL for the hyperlink
        string fullUrl = $"{baseUrl}/{relativePath}";

        return fullUrl;
    }

    private DataTable CreateDocDetailsDataTable()
    {
        DataTable dt = new DataTable();

        // file name
        DataColumn DocName = new DataColumn("DocName", typeof(string));
        dt.Columns.Add(DocName);

        // Doc uploaded path
        DataColumn DocPath = new DataColumn("DocPath", typeof(string));
        dt.Columns.Add(DocPath);

        return dt;
    }

    private void AddRowToDocDetailsDataTable(DataTable dt, string onlyFileNameWithExtn, string filePath)
    {
        // Create a new row
        DataRow row = dt.NewRow();

        // Set values for the new row
        row["DocName"] = onlyFileNameWithExtn;
        row["DocPath"] = filePath;

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
            if (GridDocument.Rows.Count > 0)
            {
                string requisitionRefNo = GetRequisitionReferenceNumber().ToString();

                ReqNo.Text = requisitionRefNo.ToString();
                Session["Requisition_RefNo"] = requisitionRefNo.ToString();

                // inserting bill head
                bool isRequisitionHeaderSaved = InsertRequisitionHeader(requisitionRefNo);

                if (isRequisitionHeaderSaved)
                {
                    // inserting item details from grid
                    InsertReqDetails(requisitionRefNo);

                    // inserting documents
                    InsertBillDocument(requisitionRefNo);

                    btnSubmit.Enabled = false;

                    getSweetAlertSuccessRedirectMandatory("Saved Successfully", $"Your Requisition No: {requisitionRefNo} Saved Successfully", "Requisition.aspx");
                }
                else
                {
                    getSweetAlertErrorMandatory("Some went wrong!", "Requisition did not saved");
                }
            }
            else
            {
                getSweetAlertErrorMandatory("No Documents Found", "please add minimum one document");
            }
        }
        else
        {
            getSweetAlertErrorMandatory("No Service Found", "please minim one services");
        }
    }

    private bool InsertRequisitionHeader(string requisitionRefNo)
    {
        string reqRefNo = requisitionRefNo;
        DateTime requisitionDate = DateTime.Parse(ReqDate.Text);

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = $@"INSERT INTO Requisition1891 (RefNo, ReqNo, ReqDte) 
                            VALUES (@RefNo, @ReqNo, @ReqDte)";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@RefNo", reqRefNo);
            cmd.Parameters.AddWithValue("@ReqNo", reqRefNo);
            cmd.Parameters.AddWithValue("@ReqDte", requisitionDate.ToString("yyyy-MM-dd"));
            int rows = cmd.ExecuteNonQuery();

            //SqlDataAdapter ad = new SqlDataAdapter(cmd);
            //DataTable dt = new DataTable();
            //ad.Fill(dt);
            con.Close();

            if (rows > 0) return true;
            else return false;
        }
    }

    private void InsertReqDetails(string requisitionRefNo)
    {
        DataTable itemsDT = (DataTable)Session["ItemDetails"];

        DynamicGridView(itemsDT);

        string ReqRefNo = requisitionRefNo;

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            foreach (DataRow row in itemsDT.Rows)
            {
                string reqNo = GetItemRefNo().ToString(); // new requisition ref no

                // item items
                string serviceCode = row["ServiceName"].ToString() ?? string.Empty;
                string cellEnlistName = row["NmeCell"].ToString();
                string qty = row["Quty"].ToString();
                string comments = row["Comment"].ToString() ?? string.Empty;


                // fetching other details
                DataTable userDT = GetLoggedInUserDetails();
                DataTable servicePriceDT = GetServicePriceDetails(serviceCode);

                string orgType = userDT.Rows[0]["OrgType"].ToString();

                string taxApplied = "";
                double servicePrice = 0.00;


                // Academic     - 1000001
                // Non-Academic - 1000002

                if (orgType == "1000001") taxApplied = "NO";
                else taxApplied = "YES";

                if (orgType == "1000001") servicePrice = Convert.ToDouble(servicePriceDT.Rows[0]["AcadPrice"]);
                else servicePrice = Convert.ToDouble(servicePriceDT.Rows[0]["NonAcadPrice"]);

                // inserting bill item details
                string sql = $@"INSERT INTO Requisition2891 
                            (RefNo, BillRefNo, ServiceName, NmeCell, Quty, Comment, TaxApplied, OrgType, ServicePrice) 
                            VALUES 
                            (@RefNo, @BillRefNo, @ServiceName, @NmeCell, @Quty, @Comment, @TaxApplied, @OrgType, @ServicePrice)";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@RefNo", reqNo);
                cmd.Parameters.AddWithValue("@BillRefNo", ReqRefNo);
                cmd.Parameters.AddWithValue("@ServiceName", serviceCode);
                cmd.Parameters.AddWithValue("@NmeCell", cellEnlistName);
                cmd.Parameters.AddWithValue("@Quty", qty);
                cmd.Parameters.AddWithValue("@Comment", comments);
                cmd.Parameters.AddWithValue("@TaxApplied", taxApplied);
                cmd.Parameters.AddWithValue("@OrgType", orgType);
                cmd.Parameters.AddWithValue("@ServicePrice", servicePrice);
                int execute = cmd.ExecuteNonQuery();
            }

            con.Close();
        }

    }

    private void InsertBillDocument(string requisitionRefNo)
    {
        string reqRefNo = requisitionRefNo;

        DataTable documentsDT = (DataTable)Session["DocUploadDT"];

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            foreach (GridViewRow row in GridDocument.Rows)
            {
                int rowIndex = row.RowIndex;

                string docName = documentsDT.Rows[rowIndex]["DocName"].ToString();

                HyperLink hypDocPath = (HyperLink)row.FindControl("DocPath");
                string docPath = hypDocPath.NavigateUrl;

                // getting new doc ref id
                int docRefNo = GetDocumentRefNo();

                bool isDocExist = checkForDocuUploadedExist(docRefNo.ToString());

                if (isDocExist)
                {
                    //string sql = $@"UPDATE DocUpload874 SET DocName=@DocName, DocPath=@DocPath WHERE RefID=@RefID";

                    //SqlCommand cmd = new SqlCommand(sql, con);
                    //cmd.Parameters.AddWithValue("@DocName", docName);
                    //cmd.Parameters.AddWithValue("@DocPath", docPath);
                    //cmd.Parameters.AddWithValue("@RefID", );
                    //cmd.ExecuteNonQuery();
                }
                else
                {
                    string sql = $@"INSERT INTO BillDocUpload891
                                    (RefNo, BillRefNo, DocName, DocPath) 
                                    values (@RefNo, @BillRefNo, @DocName, @DocPath)";

                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@RefNo", docRefNo);
                    cmd.Parameters.AddWithValue("@BillRefNo", reqRefNo);
                    cmd.Parameters.AddWithValue("@DocName", docName);
                    cmd.Parameters.AddWithValue("@DocPath", docPath);
                    cmd.ExecuteNonQuery();
                }
            }

            con.Close();
        }
    }

    private bool checkForDocuUploadedExist(string docRefNo)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = "SELECT * FROM BillDocUpload891 WHERE RefNo=@RefNo";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@RefNo", docRefNo);
            cmd.ExecuteNonQuery();

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0) return true;
            else return false;
        }
    }
}