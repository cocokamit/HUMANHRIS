<%@ Page Language="C#" AutoEventWireup="true" CodeFile="leave.aspx.cs" Inherits="content_Employee_leave" MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_leave">
    <script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        jQuery.noConflict();
        (function($) {
            $(function() {
                $(".datee").datepicker({ changeMonth: true,
                    yearRange: "-1:+1",
                    changeYear: true
                });
            });
        })(jQuery);
    </script>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" ID="content_leave" runat="server">
<section class="content-header">
    <h1>Leave Application</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li><a href="KOISK_LEAVE">Leave</a></li>
    <li class="active">Leave Application</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-lg-4">
         <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">Leave Credits</h3>
            </div>
            <div class="box-body">
                 <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" EmptyDataText="No Leave Credits"  CssClass="table table-striped table-bordered no-margin"><%--OnRowDataBound="datatbound"--%>
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="Leave" HeaderText="Leave"/>
                        <asp:BoundField DataField="Credit" HeaderText="Credits"/>
                        <asp:BoundField DataField="Balance" HeaderText="Balance"/>
                        <asp:BoundField DataField="yyyyear" HeaderText="Year"/>
                    </Columns>
                </asp:GridView>
            </div>
          </div>
          <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">Application</h3>
            </div>
            <div class="box-body">
                <div class="form-group radio  no-margin">
                    <asp:RadioButtonList ID="rb_range" runat="server" OnSelectedIndexChanged="select_nod" AutoPostBack="true">
                        <asp:ListItem Value="1" Selected>Whole Day</asp:ListItem>
                        <asp:ListItem Value="0.5">Half Day</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="form-group group">
                    <div id="startin" class="radio" style=" display:none;" runat="server">
                        <label style="margin-left:-20px">Expected Leave Designation<asp:Label ID="lbl_eiotd" CssClass="text-danger" runat="server"></asp:Label></label>
                        <asp:RadioButtonList ID="radiobtnlist" runat="server">
                             <asp:ListItem Value="1">1st Half (8:30 am to 1:15 pm (4.45 hours))</asp:ListItem>
                             <asp:ListItem Value="2">2nd Half  (1:15 pm to 6:00 pm (4.45 hours))</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="form-group">
                    <label>Leave Type</label>
                    <asp:DropDownList ID="ddl_leave" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="row">
                    <div class="col-lg-6">
                        <div class="form-group">
                            <label>From<asp:Label ID="lbl_from" CssClass="text-danger" runat="server" Text=""></asp:Label></label>
                            <asp:TextBox ID="txt_from" runat="server" AutoComplete="off" CssClass="datee form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-6">
                        <div class="form-group">
                            <label>To <asp:Label ID="lbl_to" CssClass="text-danger" runat="server" Text=""></asp:Label></label>
                            <asp:TextBox ID="txt_to" runat="server" AutoComplete="off" CssClass="datee form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                
                <div class="form-group none">
                    <label>Delegate To <asp:Label ID="lbl_delegate" CssClass="text-danger" runat="server" Text=""></asp:Label></label>
                    <asp:TextBox ID="txt_delegate" runat="server" AutoComplete="off" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group no-margin">
                    <label>Reason <asp:Label ID="lbl_remarks" CssClass="text-danger" runat="server" Text=""></asp:Label></label>
                    <asp:TextBox ID="txt_lineremarks" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="box box-footer pad">
                <asp:Button ID="Button2" runat="server" OnClick="click_veirfy" Text="Verify" CssClass="btn btn-primary" />
                 <asp:Label ID="lbl_error_msg" CssClass="text-danger" ForeColor="Red" runat="server" Text=""></asp:Label>
                <asp:Label ID="lbl_err" runat="server" ForeColor="Red" ></asp:Label>
            </div>
          </div>
        </div>
        <div class="col-lg-8">
          <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">Leave Transaction</h3>
            </div>
            <div class="box-body">
                <asp:GridView ID="grid_leave"  runat="server" ClientIDMode="Static" OnRowDataBound="tawesie" EmptyDataText="No record found" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                    <Columns>
                        <asp:BoundField DataField="cnt" HeaderText="No."/>
                        <asp:BoundField DataField="leave" HeaderText="Leave"/>
                        <asp:BoundField DataField="date" HeaderText="Date"/>
                        <asp:BoundField DataField="noh" HeaderText="No of Days"/>
                        <asp:BoundField DataField="pay" HeaderText="Is with pay"/>
                        <asp:BoundField DataField="expectedin" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="can" runat="server" ToolTip="Delete" CausesValidation="false" OnClick="delete_tran" ImageUrl="~/style/img/delete.png" />
                            </ItemTemplate>
                            <ItemStyle Width="5px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div class="form-group no-margin">
                    <label>Attach File</label>
                    <asp:FileUpload ID="file_img" CssClass="" runat="server" />
                </div>
            </div>
            <div class="box box-footer pad">
                <asp:Button ID="Button1" runat="server" OnClick="click_save" Text="Save" CssClass="btn btn-primary"/>
            </div>
          </div>
        </div>
        
    </div>
</section>
<asp:HiddenField ID="user_id" runat="server" />
<asp:HiddenField ID="query" runat="server" />
</asp:Content>
