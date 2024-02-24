<%@ Page Language="C#" UnobtrusiveValidationMode="None" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="Requisition.aspx.cs" Inherits="Requisition_Update_Requisition" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Requisition Update</title>

    <!-- Boottrap CSS -->
    <link href="../assests/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../assests/css/bootstrap1.min.css" rel="stylesheet" />

    <!-- Bootstrap JS -->
    <script src="../assests/js/bootstrap.bundle.min.js"></script>
    <%--<script src="../assests/js/bootstrap1.min.js"></script>--%>

    <!-- Popper.js -->
    <script src="../assests/js/popper.min.js"></script>
    <script src="../assests/js/popper1.min.js"></script>

    <!-- jQuery -->
    <script src="../assests/js/jquery-3.6.0.min.js"></script>
    <script src="../assests/js/jquery.min.js"></script>
    <script src="../assests/js/jquery-3.3.1.slim.min.js"></script>

    <!-- Select2 library CSS and JS -->
    <link href="../assests/select2/select2.min.css" rel="stylesheet" />
    <script src="../assests/select2/select2.min.js"></script>

    <!-- Sweet Alert CSS and JS -->
    <link href="../assests/sweertalert/sweetalert2.min.css" rel="stylesheet" />
    <script src="../assests/sweertalert/sweetalert2.all.min.js"></script>

    <!-- Sumo Select CSS and JS -->
    <link href="../assests/sumoselect/sumoselect.min.css" rel="stylesheet" />
    <script src="../assests/sumoselect/jquery.sumoselect.min.js"></script>

   

    <script src="requisition.js"></script>
    <link rel="stylesheet" href="requisition.css" />

</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"></asp:ScriptManager>


        <!-- top Searching div Starts -->
        <div id="divTopSearch" runat="server" visible="true">
            <div class="col-md-11 mx-auto">

                <!-- Heading Start -->
                <div class="justify-content-end d-flex px-0 mb-0 mt-4">
                    <div class="col-md-6 px-0">
                        <div class="fw-semibold fs-3 text-dark">
                            <asp:Literal ID="Literal14" Text="Requisition Details" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="col-md-6 text-end px-0">
                        <div class="fw-semibold fs-5">
                            <asp:Button ID="btnNewBill" runat="server" Text="New Requisition +" OnClick="btnNewBill_Click" CssClass="btn btn-custom text-white shadow" />
                        </div>
                    </div>
                </div>

                <!-- Control UI Starts -->
                <div class="card mt-2 shadow-sm">
                    <div class="card-body">

                        <!-- Req No -->
                        <div class="row mb-2">
                            <div class="col-md-4 align-self-end">
                                <div class="mb-1 text-body-tertiary fw-semibold fs-6">
                                    <asp:Literal ID="Literal15" Text="" runat="server">Requisition Number</asp:Literal>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddScRequisitionNo" runat="server" AutoPostBack="true" class="form-control is-invalid" CssClass=""></asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            <!-- From Date -->
                            <div class="col-md-4 align-self-end">
                                <div class="mb-1 text-body-tertiary fw-semibold fs-6">
                                    <asp:Literal ID="Literal22" Text="" runat="server">From Date</asp:Literal>
                                </div>
                                <asp:TextBox runat="server" ID="ScFromDate" type="date" CssClass="form-control border border-secondary-subtle bg-light rounded-1 fs-6 fw-light py-1"></asp:TextBox>
                            </div>

                            <!-- To Date -->
                            <div class="col-md-4 align-self-end">
                                <div class="mb-1 text-body-tertiary fw-semibold fs-6">
                                    <asp:Literal ID="Literal23" Text="" runat="server">To Date</asp:Literal>
                                </div>
                                <asp:TextBox runat="server" ID="ScToDate" type="date" CssClass="form-control border border-secondary-subtle bg-light rounded-1 fs-6 fw-light py-1"></asp:TextBox>
                            </div>
                        </div>

                        <!-- Search Button -->
                        <div class="row mb-2 mt-4">
                            <div class="col-md-10"></div>
                            <div class="col-md-2">
                                <div class="text-end">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="col-md-10 btn btn-custom text-white col-md-2 shadow" />
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
                <!-- Control UI Ends -->


                <!-- Search Grid Starts -->
                <div id="searchGridDiv" visible="false" runat="server" class="mt-5">
                    <asp:GridView ShowHeaderWhenEmpty="true" ID="gridSearch" runat="server" AutoGenerateColumns="false" OnRowCommand="gridSearch_RowCommand" AllowPaging="true" PageSize="10"
                        CssClass="datatable table table-bordered border border-1 border-dark-subtle table-hover text-center grid-custom" OnPageIndexChanging="gridSearch_PageIndexChanging" PagerStyle-CssClass="gridview-pager">
                        <HeaderStyle CssClass="" />
                        <Columns>
                            <asp:TemplateField ControlStyle-CssClass="col-md-1" HeaderText="Sr.No">
                                <ItemTemplate>
                                    <asp:HiddenField ID="id" runat="server" Value="id" />
                                    <span>
                                        <%#Container.DataItemIndex + 1%>
                                    </span>
                                </ItemTemplate>
                                <ItemStyle CssClass="col-md-1" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ReqNo" HeaderText="Requisition Number" ItemStyle-CssClass="col-xs-3 align-middle text-start fw-light" />
                            <asp:BoundField DataField="ReqDte" HeaderText="Requisition Date" ItemStyle-CssClass="col-xs-3 align-middle text-start fw-light" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnedit" CommandArgument='<%# Eval("RefNo") %>' CommandName="lnkView" ToolTip="Edit" CssClass="shadow-sm">
                                        <asp:Image runat="server" ImageUrl="~/portal/assests/img/pencil-square.svg" AlternateText="Edit" style="width: 16px; height: 16px;"/>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <!-- Search Grid Ends -->


            </div>
        </div>
        <!-- top Searching div Ends -->





        <!-- Update Div Starts -->
        <div id="UpdateDiv" runat="server" visible="false">


            <!-- Heading -->
            <div class="col-md-11 mx-auto fw-normal fs-3 fw-medium ps-0 pb-2 text-body-secondary mb-3">
                <asp:Literal Text="Requisition Update" runat="server"></asp:Literal>
            </div>


            <!-- Bill Header UI Starts-->
            <div class="card col-md-11 mx-auto mt-2 py-2 shadow-sm rounded-3">
                <div class="card-body">

                    <!-- Heading -->
                    <div class="fw-normal fs-5 fw-medium text-body-secondary border-bottom">
                        <asp:Literal Text="Requisition Details" runat="server"></asp:Literal>
                    </div>

                    <!-- 1st row -->
                    <div class="row mb-2 mt-3">
                        <div class="col-md-6 align-self-end">
                            <div class="mb-1 text-body-tertiary fw-semibold fs-6">
                                <asp:Literal ID="Literal10" Text="" runat="server">Requisition Number</asp:Literal>
                            </div>
                            <asp:TextBox runat="server" ID="txtReqNo" type="text" ReadOnly="true" CssClass="form-control border border-secondary-subtle bg-light rounded-1 fs-6 fw-light py-1"></asp:TextBox>
                        </div>
                        <div class="col-md-6 align-self-end">
                            <div class="mb-1 text-body-tertiary fw-semibold fs-6">
                                <asp:Literal ID="Literal12" Text="" runat="server">Requisition Date</asp:Literal>
                            </div>
                            <asp:TextBox runat="server" ID="dtReqDate" type="date" ReadOnly="true" CssClass="form-control border border-secondary-subtle bg-light rounded-1 fs-6 fw-light py-1"></asp:TextBox>
                        </div>
                    </div>

                </div>
            </div>
            <!-- Bill Header UI Ends-->


            <!-- Item UI Starts -->
            <div class="card col-md-11 mx-auto mt-5 rounded-3">
                <div class="card-body">


                    <!-- Heading -->
                    <div class="fw-normal fs-5 fw-medium text-body-secondary border-bottom">
                        <asp:Literal Text="Service Details" runat="server"></asp:Literal>
                    </div>

                    <!-- Insert panel Starts -->
                    <div class="px-0 mt-3">

                        <!-- 1st Row -->
                        <div class="row mb-2">

                            <!-- Service Name DD -->
                            <div class="col-md-6 align-self-end">
                                <div class="mb-1 text-body-tertiary fw-semibold fs-6">
                                    <asp:Literal ID="Literal7" Text="" runat="server">Service Name (optional)</asp:Literal>
                                </div>
                                <asp:DropDownList ID="ddServiceName" runat="server" AutoPostBack="false" class="form-control is-invalid" CssClass=""></asp:DropDownList>
                            </div>

                            <!-- Service Name Manual -->
                            <div class="col-md-3 align-self-end">
                                <div class="mb-1 text-body-tertiary fw-semibold fs-6">
                                    <asp:Literal ID="Literal9" Text="Bill Number" runat="server">Name Of Cell Line<em style="color: red">*</em></asp:Literal>
                                    <div>
                                        <asp:RequiredFieldValidator ID="rr2" ControlToValidate="CellLineName" ValidationGroup="ItemSave" CssClass="invalid-feedback" InitialValue="" runat="server" ErrorMessage="(Please insert cell name)" SetFocusOnError="True" Display="Dynamic" ToolTip="Required"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <asp:TextBox runat="server" ID="CellLineName" type="text" CssClass="form-control border border-secondary-subtle bg-light rounded-1 fs-6 fw-light py-1"></asp:TextBox>
                            </div>

                            <!-- Quantity -->
                            <div class="col-md-3 align-self-end">
                                <div class="mb-1 text-body-tertiary fw-semibold fs-6">
                                    <asp:Literal ID="Literal11" Text="" runat="server">Quantity<em style="color: red">*</em></asp:Literal>
                                    <div>
                                        <asp:RequiredFieldValidator ID="rr3" ControlToValidate="Quantity" ValidationGroup="ItemSave" CssClass="invalid-feedback" InitialValue="" runat="server" ErrorMessage="(Please insert quantity)" SetFocusOnError="True" Display="Dynamic" ToolTip="Required"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <asp:TextBox runat="server" ID="Quantity" type="text" CssClass="form-control border border-secondary-subtle bg-light rounded-1 fs-6 fw-light py-1"></asp:TextBox>
                            </div>

                        </div>
                        <!-- 1st Row Ends -->

                        <!-- 2nd Row Starts -->
                        <div class="row mb-2">

                            <!-- Comments -->
                            <div class="col-md-9 align-self-end">
                                <div class="mb-1 text-body-tertiary fw-semibold fs-6">
                                    <asp:Literal ID="Literal5" Text="" runat="server">Comments (if any)</asp:Literal>
                                </div>
                                <asp:TextBox ID="CommentsIfAny" runat="server" type="text" CssClass="form-control border border-secondary-subtle bg-light rounded-1 fs-6 fw-light py-1"></asp:TextBox>
                            </div>

                            <!-- Add Button + -->
                            <div class="col-md-3 align-self-end text-end">
                                <div class="pb-0 mb-0">
                                    <asp:Button ID="btnItemInsert" runat="server" Text="Add +" OnClick="btnItemInsert_Click" ValidationGroup="ItemSave" CssClass="btn btn-success text-white shadow mb-5 col-md-7 button-position" />
                                </div>
                            </div>

                        </div>
                        <!-- 2nd Row Ends -->

                    </div>
                    <!-- Insert panel Ends -->

                    <!-- Item GridView Starts -->
                    <div id="itemDiv" runat="server" visible="false" class="mt-3 mb-3">
                        <asp:GridView ShowHeaderWhenEmpty="true" ID="itemGrid" runat="server" AutoGenerateColumns="false"
                            CssClass="table table-bordered  border border-1 border-dark-subtle text-center grid-custom mb-3">
                            <HeaderStyle CssClass="align-middle" />
                            <Columns>

                                <asp:TemplateField ControlStyle-CssClass="col-md-1" HeaderText="Sr.No">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="id" runat="server" Value="id" />
                                        <span>
                                            <%#Container.DataItemIndex + 1%>
                                        </span>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="col-md-1" />
                                    <ItemStyle Font-Size="15px" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="ServiceName" HeaderText="Service Name" ItemStyle-Font-Size="15px" ItemStyle-CssClass="align-middle text-start fw-light" />
                                <asp:BoundField DataField="NmeCell" HeaderText="Cell Name" ItemStyle-Font-Size="15px" ItemStyle-CssClass="align-middle text-start fw-light" />
                                <asp:BoundField DataField="Quty" HeaderText="Quantity" ItemStyle-Font-Size="15px" ItemStyle-CssClass="align-middle text-start fw-light" />
                                <asp:BoundField DataField="Comment" HeaderText="Comments (if any)" ItemStyle-Font-Size="15px" ItemStyle-CssClass="align-middle text-start fw-light" />
                            </Columns>
                        </asp:GridView>

                        <hr class="border border-secondary-subtle" />
                    </div>
                    <!-- Item GridView Ends -->

                    <!-- Heading Document -->
                    <div class="border-top border-bottom border-secondary-subtle py-2 mt-4">
                        <div class="fw-normal fs-5 fw-medium text-body-secondary">
                            <asp:Literal Text="Document Upload" runat="server"></asp:Literal>
                        </div>
                    </div>

                    <!-- Documents Upload Button Starts -->
                    <div class="row">
                        <div class="col-md-6">
                            <div class="mt-4 input-group has-validation">
                                <asp:FileUpload ID="fileDoc" runat="server" CssClass="form-control" aria-describedby="inputGroupPrepend" />
                                <asp:Button ID="btnDocUpload" OnClick="btnDocUpload_Click" runat="server" Text="Upload  +" AutoPost="true" CssClass="btn btn-custom btn-outline-secondary" />
                            </div>
                            <h6 class="pt-3 fw-lighter fs-6 text-secondary-subtle">User can upload multiple documents using upload button !</h6>
                        </div>
                        <div class="col-md-6"></div>
                    </div>
                    <!-- Documents Upload Button Ends -->


                    <!-- Document Grid Starts -->
                    <div id="docGrid" class="mt-5" runat="server" visible="false">
                        <asp:GridView ShowHeaderWhenEmpty="true" ID="GridDocument" EnableViewState="true" runat="server" AutoGenerateColumns="false"
                            CssClass="table table-bordered border border-light-subtle text-start mt-3 grid-custom">
                            <HeaderStyle CssClass="align-middle fw-light fs-6" />
                            <Columns>
                                <asp:TemplateField ControlStyle-CssClass="col-md-1" HeaderText="Sr.No">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="id" runat="server" Value="id" />
                                        <span>
                                            <%#Container.DataItemIndex + 1%>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="DocName" HeaderText="File Name" ReadOnly="true" ItemStyle-Font-Size="15px" ItemStyle-CssClass="align-middle text-start fw-light" />

                                <asp:TemplateField HeaderText="View Document" ItemStyle-Font-Size="15px" ItemStyle-CssClass="align-middle text-start fw-light">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="DocPath" runat="server" Text="View Uploaded Document" NavigateUrl='<%# Eval("DocPath") %>' Target="_blank" CssClass="text-decoration-none"></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>
                    <!-- Document Grid Ends -->

                    <!-- Submit Button -->
                    <div class="">
                        <div class="row mt-5 mb-2">
                            <div class="col-md-6 text-start">
                                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" CssClass="btn btn-custom text-white shadow mb-5" />
                            </div>
                            <div class="col-md-6 text-end">
                                <asp:Button ID="btnSubmit" Enabled="true" runat="server" Text="Submit" OnClick="btnSubmit_Click" ValidationGroup="finalSubmit" CssClass="btn btn-custom text-white shadow mb-5" />
                            </div>
                        </div>
                    </div>


                </div>
            </div>
            <!-- Item UI Ends -->




        </div>
        <!-- Update Div Ends -->



    </form>
</body>
</html>
