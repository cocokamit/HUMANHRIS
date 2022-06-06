<%@ Page Language="C#" AutoEventWireup="true" CodeFile="change_shift.aspx.cs" Inherits="content_Employee_change_shift" MasterPageFile="~/content/site.master" %>


<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        jQuery.noConflict();
        (function ($) {
            $(function () {
                $(".datee").datepicker();
            });
        })(jQuery);
 
    </script>
<style type="text/css">
    .x-head th {font-size:10px; line-height:25px; text-transform:uppercase; color: #444}
    .x-head td { padding:2px}
    .x_panel textarea {border:1px solid #eee;width:100%}
</style>
</asp:Content>
<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<section class="content-header">
    <h1>Change Shift Application</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Change Shift Application</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            <div class="box-body table-responsive">
                <asp:GridView ID="grid_view" runat="server" EmptyDataText="No record found"  AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                        <asp:BoundField DataField="emp_id"  ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                        <asp:BoundField DataField="Date" HeaderText="DATE"/>
                        <asp:BoundField DataField="shiftcode" HeaderText="Shiftcode"/>
                        <asp:BoundField DataField="shiftcode_id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk_view" runat="server" OnClick="view" CssClass="fa fa-check-circle"></asp:LinkButton>
                               <%-- <asp:LinkButton ID="LinkButton2" runat="server" OnClick="delete_row_datatable"   Text="del" ><i class="fa fa-close"></i></asp:LinkButton>--%>
                            </ItemTemplate>
                            <ItemStyle Width="10px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
          </div>
        </div>
    </div>
</section>


 
<div class="clearfix"></div>
<div class="row">
     <div class="col-md-9">
        <div class="x_panel">
             
          
        </div>
    </div>
   
</div>

<div id="Div1" runat="server" visible="false" class="Overlay"></div>
<div id="Div2" runat="server" visible="false" class="PopUpPanel">
    <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
    <div class="form-group">
        <label>Date</label>
        <asp:TextBox ID="txt_date" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
    </div>
    <div class="form-group">
        <label>Shift Code</label>
        <asp:DropDownList ID="ddl_shiftcode" runat="server" CssClass="form-control" >
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <label>Remarks</label>
        <asp:TextBox ID="txt_remarks" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
    </div>
    <div class="box-footer">
        <asp:Button ID="btn_save" runat="server" OnClick="click_save_changeshift" Text="Save" CssClass="btn btn-primary" />
    </div>
</div>
    <asp:HiddenField ID="key" runat="server" />
    <asp:HiddenField ID="shift_id" runat="server" />
    <asp:HiddenField ID="pg" runat="server" />
</asp:Content>
