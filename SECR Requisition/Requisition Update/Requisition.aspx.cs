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

public partial class Requisition_Update_Requisition : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["Ginie"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Search_DD_RequistionNo();
            ddServiceName_Bind_Dropdown();
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



    //=========================={ Binding Search Dropdowns }==========================
    private void Search_DD_RequistionNo()
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = "select * from Requisition1891 order by ReqNo desc";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.ExecuteNonQuery();

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            con.Close();

            ddScRequisitionNo.DataSource = dt;
            ddScRequisitionNo.DataTextField = "ReqNo";
            ddScRequisitionNo.DataValueField = "ReqNo";
            ddScRequisitionNo.DataBind();
            ddScRequisitionNo.Items.Insert(0, new ListItem("------Select Requisition No------", "0"));
        }
    }
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




    //=========================={ Search Button Event }==========================
    protected void btnNewBill_Click(object sender, EventArgs e)
    {
        Response.Redirect("RequisitionNew.aspx");
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGridView();
    }

    private void BindGridView()
    {
        searchGridDiv.Visible = true;

        // dropdown values
        string reqNo = ddScRequisitionNo.SelectedValue;

        DateTime fromDate;
        DateTime toDate;

        if (!DateTime.TryParse(ScFromDate.Text, out fromDate)) { fromDate = SqlDateTime.MinValue.Value; }
        if (!DateTime.TryParse(ScToDate.Text, out toDate)) { toDate = SqlDateTime.MaxValue.Value; }

        // DTs
        DataTable reqDT = GetRequisitionDT(reqNo);

        // dt values
        string requisitionNo = (reqDT.Rows.Count > 0) ? reqDT.Rows[0]["ReqNo"].ToString() : string.Empty;

        DataTable searchResultDT = SearchRecords(requisitionNo, fromDate, toDate);

        // binding the search grid
        gridSearch.DataSource = searchResultDT;
        gridSearch.DataBind();

        Session["PaginationDataSource"] = searchResultDT;
    }

    public DataTable SearchRecords(string requisitionNo, DateTime fromDate, DateTime toDate)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string sql = "SELECT * FROM Requisition1891 WHERE 1=1";

            if (!string.IsNullOrEmpty(requisitionNo))
            {
                sql += " AND ReqNo = @ReqNo";
            }

            if (fromDate != null)
            {
                sql += " AND ReqDte >= @FromDate";
            }

            if (toDate != null)
            {
                sql += " AND ReqDte <= @ToDate";
            }

            sql += " ORDER BY RefNo DESC";




            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                if (!string.IsNullOrEmpty(requisitionNo))
                {
                    command.Parameters.AddWithValue("@ReqNo", requisitionNo);
                }

                if (fromDate != null)
                {
                    command.Parameters.AddWithValue("@FromDate", fromDate);
                }

                if (toDate != null)
                {
                    command.Parameters.AddWithValue("@ToDate", toDate);
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
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

            FillRequisitionDetails(rowId);

            FillItemDetails(rowId);

            // binding doc gridview
            FillBillDocUpload(rowId);
        }
    }

    private void FillRequisitionDetails(int requisitionRefNo)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = "select* from Requisition1891 where RefNo = @RefNo";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@RefNo", requisitionRefNo);
            cmd.ExecuteNonQuery();

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            con.Close();


            txtReqNo.Text = dt.Rows[0]["ReqNo"].ToString();

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

    private void FillBillDocUpload(int requisitionRefNo)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = "select * from BillDocUpload891 where BillRefNo = @BillRefNo";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@BillRefNo", requisitionRefNo.ToString());
            cmd.ExecuteNonQuery();

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            con.Close();

            docGrid.Visible = true;

            GridDocument.DataSource = dt;
            GridDocument.DataBind();

            ViewState["DocDetailsDataTable"] = dt;
            Session["DocUploadDT"] = dt;
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
        string quantity = Quantity.Text.ToString();
        string commentsIfAny = CommentsIfAny.Text.ToString() ?? "";

        DataTable dt = ViewState["ReqDetailsVS"] as DataTable ?? createDatatable();

        AddRowToDataTable(dt, BillReferenceNo, serviceName, nameCellLine, quantity, commentsIfAny);

        ViewState["ReqDetailsVS"] = dt;
        Session["ReqDetails"] = dt;

        if (dt.Rows.Count > 0)
        {
            itemDiv.Visible = true;

            itemGrid.DataSource = dt;
            itemGrid.DataBind();

            ddServiceName.SelectedIndex = 0;
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

        // quantity
        DataColumn Quty = new DataColumn("Quty", typeof(string));
        dt.Columns.Add(Quty);

        // comments
        DataColumn Comment = new DataColumn("Comment", typeof(string));
        dt.Columns.Add(Comment);

        return dt;
    }

    private void AddRowToDataTable(DataTable dt, string serviceName, string BillReferenceNo, string nameCellLine, string quantity, string commentsIfAny)
    {
        // Create a new row
        DataRow row = dt.NewRow();

        // Set values for the new row
        row["BillRefNo"] = BillReferenceNo;
        row["ServiceName"] = serviceName;
        row["NmeCell"] = nameCellLine;
        row["Quty"] = quantity;
        row["Comment"] = commentsIfAny;

        // Add the new row to the DataTable
        dt.Rows.Add(row);
    }





    //----------============={ Upload New Documents }=============----------
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
            DataTable dt = ViewState["DocDetailsDataTable"] as DataTable ?? CreateDocDetailsDataTable();

            // filling document details datatable
            AddRowToDocDetailsDataTable(dt, onlyFileNameWithExtn, filePath);

            // Save DataTable to ViewState
            ViewState["DocDetailsDataTable"] = dt;
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





    //=========================={ Submit Button Click Event }==========================
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Requisition.aspx");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (GridDocument.Rows.Count > 0)
        {
            string reqReferenceNo = Session["ReqReferenceNo"].ToString();

            // updating bill head
            //UpdateBillHeader();

            // updating item details
            UpdateBillItemDetails(reqReferenceNo);

            // updating bill doc uploads
            UpdateBillDocDetails(reqReferenceNo);

            getSweetAlertSuccessRedirectMandatory("Updated!", "Updated successfully", "Requisition.aspx");
        }
        else
        {
            getSweetAlertErrorMandatory("Error!", "Update failed, please try again");
        }
    }


    // header - not in use yet
    private void UpdateBillHeader()
    {
        string billRefno = Session["ReqReferenceNo"].ToString();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = $@"UPDATE Requisition1891 SET BillAmt = @BillAmt WHERE RefNo = @RefNo";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@RefNo", billRefno);
            cmd.ExecuteNonQuery();

            //SqlDataAdapter ad = new SqlDataAdapter(cmd);
            //DataTable dt = new DataTable();
            //ad.Fill(dt);

            con.Close();
        }
    }



    // requisition item details
    private void UpdateBillItemDetails(string reqReferenceNo)
    {
        string reqlRefno = reqReferenceNo;

        DataTable billItemsDT = (DataTable)Session["ReqDetails"]; // requisition 2 details

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();

            foreach (GridViewRow row in itemGrid.Rows)
            {
                int rowIndex = row.RowIndex;

                string serviceName = billItemsDT.Rows[rowIndex]["ServiceName"].ToString();
                string cellEnlistName = billItemsDT.Rows[rowIndex]["NmeCell"].ToString();
                string qty = billItemsDT.Rows[rowIndex]["Quty"].ToString();
                string comments = billItemsDT.Rows[rowIndex]["Comment"].ToString();

                string itemRefNo = billItemsDT.Rows[rowIndex]["RefNo"].ToString();



                bool isItemExists = IsItemExists(itemRefNo);

                if (isItemExists) // true - update
                {
                    
                }
                else // false - insert
                {
                    // getting new ref id for item
                    string itemRefNoNew = GetItemRefNo().ToString();

                    string sql = $@"INSERT INTO Requisition2891 
                                    (RefNo, BillRefNo, ServiceName, NmeCell, Quty, Comment) 
                                    VALUES 
                                    (@RefNo, @BillRefNo, @ServiceName, @NmeCell, @Quty, @Comment)";

                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@RefNo", itemRefNoNew);
                    cmd.Parameters.AddWithValue("@BillRefNo", reqlRefno);
                    cmd.Parameters.AddWithValue("@ServiceName", serviceName);
                    cmd.Parameters.AddWithValue("@NmeCell", cellEnlistName);
                    cmd.Parameters.AddWithValue("@Quty", qty);
                    cmd.Parameters.AddWithValue("@Comment", comments);
                    cmd.ExecuteNonQuery();
                }
            }

            con.Close();
        }

    }

    private bool IsItemExists(string itemRefNo)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = "select * from Requisition2891 where RefNo = @RefNo";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@RefNo", itemRefNo);
            cmd.ExecuteNonQuery();

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);

            con.Close();

            if (dt.Rows.Count > 0) return true;
            else return false;
        }
    }

    private int GetItemRefNo()
    {
        string nextRefID = "1000001";

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = "SELECT ISNULL(MAX(CAST(RefNo AS INT)), 10000) + 1 AS NextRefID FROM Requisition2891";
            SqlCommand cmd = new SqlCommand(sql, con);

            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value) { nextRefID = result.ToString(); }
            return Convert.ToInt32(nextRefID);
        }
    }




    // document details
    private void UpdateBillDocDetails(string reqReferenceNo)
    {
        string reqRefno = reqReferenceNo;

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

                string existingDocRefNo = documentsDT.Rows[rowIndex]["RefNo"].ToString();

                bool isDocExist = checkForDocuUploadedExist(existingDocRefNo);

                if (isDocExist)
                {

                }
                else
                {
                    string newDocRefNo = getDocRefINo().ToString(); // new doc RefID

                    string sql = $@"INSERT INTO BillDocUpload891 
                                    (RefNo, BillRefNo, DocName, DocPath) 
                                    values 
                                    (@RefNo, @BillRefNo, @DocName, @DocPath)";

                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@RefNo", newDocRefNo);
                    cmd.Parameters.AddWithValue("@BillRefNo", reqRefno);
                    cmd.Parameters.AddWithValue("@DocName", docName);
                    cmd.Parameters.AddWithValue("@DocPath", docPath);
                    cmd.ExecuteNonQuery();
                }
            }

            con.Close();
        }
    }

    private bool checkForDocuUploadedExist(string existingDocRefNo)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = "SELECT * FROM BillDocUpload891 WHERE RefNo=@RefNo";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@RefNo", existingDocRefNo);
            cmd.ExecuteNonQuery();

            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0) return true;
            else return false;
        }
    }

    private int getDocRefINo()
    {
        string nextRefID = "1000001";

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = "SELECT ISNULL(MAX(CAST(RefNo AS INT)), 10000) + 1 AS NextRefID FROM BillDocUpload891";
            SqlCommand cmd = new SqlCommand(sql, con);

            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value) { nextRefID = result.ToString(); }
            return Convert.ToInt32(nextRefID);
        }
    }



}