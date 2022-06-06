<%@ Page Language="C#" AutoEventWireup="true" CodeFile="approve_leave.aspx.cs" Inherits="content_Manager_approve_leave" MasterPageFile="~/content/site.master" %>


<asp:Content ContentPlaceHolderID="head" ID="head_approve_leave" runat="server">
    <script type="text/javascript">
        function Confirm() 
        {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure you want to cancel this Application?"))
            { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
        }
        function Confirmapp() {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure you want to Approved this Application?"))
            { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
        }
</script>
 

</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_approve_leave">

<section class="content-header">
    <h1>Leave Approval</h1>
    <ol class="breadcrumb">
    <li><a href="dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Leave Approval</li>
    </ol>
</section>

<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            <div class="box-body table-responsive">
                <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false"  EmptyDataText="No record found"  CssClass="table table-striped table-bordered no-margin">
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="sysdate" HeaderText="Date Filed"/>
                        <asp:BoundField DataField="leavefrom" HeaderText="Leave From"/>
                        <asp:BoundField DataField="leaveto" HeaderText="Leave To"/>
                        <asp:BoundField DataField="Fullname" HeaderText="Employee Name"/>
                        <asp:BoundField DataField="Leave" HeaderText="Leave Type"/>
                        <asp:BoundField DataField="delegate" HeaderText="Delegate"/>
                        <asp:BoundField DataField="remarks" HeaderText="Reason"/>
                           <asp:BoundField DataField="nxt_id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnk_approve" OnClientClick="Confirmapp()"  OnClick="approve" ToolTip="Approved"> <i class="fa fa-thumbs-o-up"></i></asp:LinkButton>
                                     <asp:LinkButton runat="server" ID="lnk_view"  OnClientClick="Confirm()" OnClick="view" ToolTip="Cancel"><i class="fa fa-trash"></i></asp:LinkButton>
                                      <asp:LinkButton runat="server" ID="LinkButton1" OnClick="view_img" ToolTip="Attachment"><i class="fa fa-paperclip"></i></asp:LinkButton>
                                   
                                </ItemTemplate>
                                <ItemStyle  Width="90px"/>
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
    <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
       <div class="box-body table-responsive no-pad-top">
        <div class="form-group">
            <label>Reason</label>
            <asp:TextBox ID="txt_reason" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
    </div>
    <asp:Button ID="btn_save" runat="server" OnClick="delete3" Text="Save" CssClass="btn btn-primary" />
</div>

<div id="Div3" runat="server" visible="false" class="Overlay"></div>
<div id="Div4" runat="server" visible="false" class="PopUpPanel">
      <asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
    <div class="form-group">
        <label> <asp:Label ID="lbl_msg" ForeColor="Red" runat="server" Text="Label"></asp:Label></label>
        <asp:GridView ID="grid_img" runat="server" AutoGenerateColumns ="False"  EmptyDataText="No record found"  CssClass="table table-bordered table-hover dataTable" >
            <Columns>
                <asp:BoundField DataField="id" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <img width="150px" src='files/leave/<%# Eval("awts") %>' alt="Alternate Text" style=" margin:5px" />
                    </ItemTemplate>
                </asp:TemplateField>       
            </Columns>
        </asp:GridView>
    </div>
</div>

<asp:HiddenField ID="key" runat="server" />
<asp:TextBox ID="TextBox1" CssClass="hide" runat="server"></asp:TextBox>
<asp:HiddenField ID="idd" runat="server" />
</asp:Content>

