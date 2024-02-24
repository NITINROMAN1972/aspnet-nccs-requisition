<%@ Page Language="C#" UnobtrusiveValidationMode="None" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="RequisitionNew.aspx.cs" Inherits="Requisition_New_RequisitionNew" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Requisition New</title>

    <!-- Boottrap CSS -->
    <link href="../assests/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../assests/css/bootstrap1.min.css" rel="stylesheet" />

    <!-- Bootstrap JS -->
    <script src="../assests/js/bootstrap.bundle.min.js"></script>
    <script src="../assests/js/bootstrap1.min.js"></script>

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

    <script src="requisition-new.js"></script>
    <link rel="stylesheet" href="requisition-new.css" />

</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"></asp:ScriptManager>


        <!-- Heading -->
        <div class="col-md-11 mx-auto fw-normal fs-3 fw-medium ps-0 pb-2 text-body-secondary mb-3">
            <asp:Literal Text="Requisition Entry" runat="server"></asp:Literal>
        </div>

        <!-- UI Starts -->
        <div class="card col-md-11 mx-auto mt-1 py-2 shadow-sm rounded-3">
            <div class="card-body">

                <!-- Heading Billing Entry -->
                <div class="fw-normal fs-5 fw-medium border-bottom pb-2 text-body-secondary mb-4">
                    <asp:Literal Text="Requisition Details" runat="server"></asp:Literal>
                </div>

                <!-- 1st row -->
                <div class="row mb-2">
                    <div class="col-md-6 align-self-end">
                        <div class="mb-1 text-body-tertiary fw-semibold fs-6">
                            <asp:Literal ID="Literal10" Text="Bill Number" runat="server">Requisition Number</asp:Literal>
                        </div>
                        <asp:TextBox runat="server" ID="ReqNo" ReadOnly="true" type="text" Text="System Generted" CssClass="form-control border border-secondary-subtle bg-light rounded-1 fs-6 fw-light py-1"></asp:TextBox>
                    </div>

                    <div class="col-md-6 align-self-end">
                        <div class="mb-1 text-body-tertiary fw-semibold fs-6">
                            <asp:Literal ID="Literal12" Text="Bill Date" runat="server">Requisition Date<em style="color: red">*</em></asp:Literal>
                            <div>
                                <asp:RequiredFieldValidator ValidationGroup="finalSubmit" ID="rr1" ControlToValidate="ReqDate" CssClass="invalid-feedback" InitialValue="" runat="server" ErrorMessage="(Please select the date)" SetFocusOnError="True" Display="Dynamic" ToolTip="Required"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <asp:TextBox runat="server" ID="ReqDate" type="date" CssClass="form-control border border-secondary-subtle bg-light rounded-1 fs-6 fw-light py-1"></asp:TextBox>
                    </div>

                </div>

            </div>
        </div>
        <!-- UI Ends -->

        <!-- Below Panel UI -->
        <div class="card col-md-11 mx-auto mt-5 rounded-3">
            <div class="card-body">

                <!-- Heading Bottom Line -  Cell Details -->
                <div class="fw-normal fs-5 fw-medium border-bottom pb-2 mb-3 text-body-secondary">
                    <asp:Literal Text="Service Details" runat="server"></asp:Literal>
                </div>

                <!-- Insert panel Starts -->
                <div class="px-0">

                    <!-- 1st Row -->
                    <div class="row mb-2">

                        <!-- Service Name DD -->
                        <div class="col-md-6 align-self-end">
                            <div class="mb-1 text-body-tertiary fw-semibold fs-6">
                                <asp:Literal ID="Literal7" Text="" runat="server">Service Name (optional)</asp:Literal>
                                <div>
                                    <asp:RequiredFieldValidator ID="rr9" ControlToValidate="ddServiceName" ValidationGroup="ItemSave" CssClass="invalid-feedback" InitialValue="0" runat="server" ErrorMessage="(Please select service name)" SetFocusOnError="True" Display="Dynamic" ToolTip="Required"></asp:RequiredFieldValidator>
                                </div>
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


                <!-- Item GridView -->
                <div id="itemDiv" runat="server" visible="false" class="mt-3 mb-3">
                    <asp:GridView ShowHeaderWhenEmpty="true" ID="itemGrid" runat="server" AutoGenerateColumns="false" OnRowDeleting="Grid_RowDeleting"
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

                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Container.DataItemIndex %>'>
                                        <asp:Image runat="server" ImageUrl="~/portal/assests/img/modern-cross-fill.svg" AlternateText="Edit" style="width: 28px; height: 28px;"/>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>

                    <%--<hr class="border border-secondary-subtle" />--%>
                </div>
                <!-- Item GridView Ends -->

                <!-- Heading Document -->
                <div class="border-top border-bottom border-secondary-subtle py-2 mt-4">
                    <div class="fw-normal fs-5 fw-medium text-body-secondary">
                        <asp:Literal Text="Document Upload" runat="server"></asp:Literal>
                    </div>
                </div>

                <!-- Documents Upload -->
                <div class="row">
                    <div class="col-md-6">
                        <div class="mt-4 input-group has-validation">
                            <asp:FileUpload ID="fileDoc" runat="server" CssClass="form-control" aria-describedby="inputGroupPrepend" />
                            <asp:Button ID="btnDocUpload" OnClick="btnDocUpload_Click" runat="server" Text="Upload +" AutoPost="true" CssClass="btn btn-custom btn-outline-secondary" />
                        </div>
                        <h6 class="pt-3 fw-lighter fs-6 text-secondary-subtle">User can upload Requisition and other Documents !</h6>
                    </div>
                    <div class="col-md-6"></div>
                </div>
                <!-- Documents Upload Ends -->


                <!-- Document Grid Starts -->
                <div id="docGrid" class="mt-5" runat="server" visible="false">
                    <asp:GridView ShowHeaderWhenEmpty="true" ID="GridDocument" EnableViewState="true" runat="server" AutoGenerateColumns="false" OnRowDeleting="Grid_RowDeleting"
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

                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Container.DataItemIndex %>'>
                                        <asp:Image runat="server" ImageUrl="~/portal/assests/img/modern-cross-fill.svg" AlternateText="Edit" style="width: 28px; height: 28px;"/>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="100px" />
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
                            <asp:Button ID="btnSubmit" Enabled="true" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="btn btn-custom text-white shadow mb-5" />
                        </div>
                    </div>
                </div>



            </div>
        </div>
        <!-- Below Panel UI Ends -->


        <asp:GridView ShowHeaderWhenEmpty="true" ID="GridDyanmic" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped table-hover text-start">
            <HeaderStyle CssClass="align-middle" />
            <Columns>
                <asp:TemplateField ControlStyle-CssClass="col-xs-1" HeaderText="Sr.No">
                    <ItemTemplate>
                        <asp:HiddenField ID="id" runat="server" Value="id" />
                        <span>
                            <%#Container.DataItemIndex + 1%>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </form>
</body>
</html>
