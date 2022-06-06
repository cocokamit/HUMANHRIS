<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OTMeal.aspx.cs" Inherits="content_Employee_OTMeal" MasterPageFile="~/content/site.master"%>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_coop_list">
    <script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to cancel this application?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    } 
</script>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" ID="content_ot_list" runat="server">
<section class="content-header">
    <h1>Overtime Meal</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">OT Meal</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            <div class="box-body table-responsive">
                 <div id="alert" runat="server" class="alert alert-default alert-dismissible">
                    <i class="icon fa fa-info-circle"></i> No record found
                 </div>
               <asp:GridView ID="grid_view" runat="server" AllowPaging="true" PageSize="10" OnPageIndexChanging="gridview_PageIndexChanging" OnRowDataBound="rowbound" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="Id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"/>
                    <asp:BoundField DataField="sysdate" HeaderText="Date Filed" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="Date" HeaderText="Date Overtime" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="OvertimeHoursIn" HeaderText="Time In"/>
                    <asp:BoundField DataField="OvertimeHoursOut" HeaderText="Time Out"/>
                    <asp:BoundField DataField="OvertimeHours" HeaderText="Filed OT Hours"/>
                    <asp:BoundField DataField="Remarks" HeaderText="Reson"/>
                    <asp:BoundField DataField="status" HeaderText="Status"/>  
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_can" OnClick="view" runat="server" ToolTip="Cancel" CssClass="fa fa-trash-o"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="40px" />
                    </asp:TemplateField>
                 </Columns>
                </asp:GridView>  
            </div>
          </div>
        </div>
    </div>
</section>

<div id="Div1" runat="server" visible="false" class="Overlay"></div>
<div id="Div2" runat="server" visible="false" class="PopUpPanel">
    <asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
       <div class="box-body table-responsive no-pad-top">
            <div class="form-group">
                <label>Reason</label>
                <asp:Label ID="lbl_reason" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_reason" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
    <asp:Button ID="btn_save" runat="server" OnClick="cancel" Text="Save" CssClass="btn btn-primary"/>
</div>
<asp:HiddenField ID="id" runat="server" />
<div style=" visibility:hidden;">
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
</div>
</asp:Content>

