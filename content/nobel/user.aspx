<%@ Page Language="C#" AutoEventWireup="true" CodeFile="user.aspx.cs" Inherits="content_su_user" MasterPageFile="~/content/site.master"%>
<%@ Import Namespace="System.Data" %>

 <asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
   <link rel="stylesheet" href="vendors/datatables.net-bs/css/dataTables.bootstrap.min.css">
    <link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
        .checkbox input[type="checkbox"], .checkbox-inline input[type="checkbox"], .radio input[type="radio"], .radio-inline input[type="radio"] { margin-left:-20px !important}
        .checkbox { margin:0 !important}
        .dataTables_filter {width:100% !important}
        .dataTables_filter input {border-radius:3px; border-color:#d2d6de !imortant; width:300px !important; padding:16px 10px !important; box-shadow:none !important;}
        .dataTables_filter i {margin-left:-23px; color:#d2d6de; margin-top:9px; position:absolute}
     </style>
</asp:Content>

<asp:Content ID="content" runat="server" ContentPlaceHolderID="content">
<section class="content-header">
    <h1>User</h1>
    <ol class="breadcrumb">
    <li><a href="dashboard"><i class="fa fa-dashboard"></i> Home</a></li>
    <li class="active">User</li>
    </ol>
</section>
<section class="content">
<div class="row">
    <div class="col-xs-12">
        <div class="box box-primary">
        <div class="box-body table-responsive">
            <asp:LinkButton ID="lb_add" runat="server" OnClick="click_add"  CssClass="btn btn-primary" style=" position:absolute">Add</asp:LinkButton>
              <table id="example1" class="table table-condensed">
                <thead>
                    <tr>
                      <th>ID Number</th>
                      <th>Name</th>
                      <th>Account</th>
                      <th>Username</th>
                      <th>Password</th>
                    </tr>
                </thead>
              <tbody>
              <% DataTable dt = (DataTable)ViewState["sys_user"]; %>
              <% foreach (DataRow row in dt.Rows) { %>
              <tr>
                  <td><%=row["id"]%></td>
                  <td>
                    <a href="javascript:void(0)" data-id='<%=row["id"]%>' class="data-edit"><%=row["IdNumber"]%></a>
                  </td>
                  <td class="text-capitalize"><%=row["name"]%></td>
                  <td><%=row["acc_name"]%></td>
                  <td><%=row["username"]%></td>
                  <td><%=row["password"]%></td>
              </tr>
              <% } %>
               
                </tbody>
              </table>
            
        </div>
        </div>
    </div>
</div>
</section>

<div id="modal" runat="server" class="modal fade in">
    <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="click_close" class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h4 class="modal-title"><%= hf_action.Value == "0" ? "Create User" : "Edit User" %></h4>
            </div>
            <div class="modal-body">
              <asp:Panel ID="p_details" runat="server">
              <div class="form-group">
                    <label>Account</label> <asp:Label ID="l_account" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <asp:DropDownList ID="ddl_account" runat="server">
                        <asp:ListItem Selected disabled></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label>User</label> <asp:Label ID="l_user" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <asp:DropDownList ID="ddl_user" CssClass="select2" runat="server">
                        <asp:ListItem Value="" disabled selected hidden></asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                
                <div class="form-group">
                    <label>Username</label> <asp:Label ID="l_username" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <asp:TextBox ID="tb_username" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Password</label> <asp:Label ID="l_password" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <asp:TextBox ID="tb_password" runat="server" CssClass="form-control"  autocomplete="off"></asp:TextBox>
                </div>
              
                <div class="form-group">
                    <label>Administrator Privilege</label> 

                    <div class="checkbox" style=" position:absolute;">
                        <label style=" margin:6px 10px;">
                            <asp:CheckBox ID="cb_admin" runat="server" OnCheckedChanged="OnCheckedChanged" AutoPostBack="true" /> ALLOW  
                            
                        </label>

                        <div  id="divider" runat="server" visible="false" style="border-right:1px solid #d2d6de; position:absolute; height:18px; margin-top:-24px; margin-left:80px">
                            </div>
                    </div>
                    <asp:TextBox ID="tb_Adpass" CssClass="form-control" runat="server" Enabled="false" style="padding-left:90px"></asp:TextBox>
                </div>
                
              </asp:Panel>
              <asp:Panel ID="p_routes" runat="server" Visible="false" style=" margin-top:15px">
                <div class="form-group no-margin">
                    <label>Routes</label> <asp:Label ID="Label2" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <asp:LinkButton ID="lb_addroute" runat="server" OnClick="click_addroutes" CssClass="fa fa-pencil pull-right"></asp:LinkButton>
                    <asp:GridView ID="gv_right" CssClass="table table-bordered table-hover dataTable no-margin" EmptyDataText="No route found" OnRowDataBound="gv_rightsBound" runat="server" ShowHeader="false" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="route_id" HeaderStyle-CssClass="" ItemStyle-CssClass=""/>
                            <asp:BoundField DataField="cnt" HeaderStyle-CssClass="" ItemStyle-CssClass=""/>
                            <asp:BoundField DataField="nobelroute_id" HeaderStyle-CssClass="" ItemStyle-CssClass=""/>
                            <asp:TemplateField HeaderText="Account">
                                <ItemTemplate>
                                    <asp:Label ID="l_lable" runat="server" Text='<%# Eval("route") %>' ></asp:Label>
                                    <small style=" padding-left:5px">(<asp:Label ID="Label1" runat="server" Text='<%# Eval("description") %>' ></asp:Label>)</small>

                                    <asp:Panel ID="p_tree" runat="server">

          <div class="box no-border collapsed-box no-margin no-box-shadow">
          

            <div class="box-tools pull-right" style=" position:absolute; right:0; margin-top:-25px">
                <button type="button" class="btn btn-box-tool" data-widget="collapse" ><i class="fa fa-plus"></i>
                </button>
              </div>
            <div class="box-body">
              <asp:GridView ID="gv_treechild" runat="server" OnRowDataBound="gv_treechildBound" CssClass="table table-bordered table-hover dataTable no-margin" ShowHeader="false" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="ID" HeaderStyle-CssClass="" ItemStyle-CssClass=""/>
                    <asp:BoundField DataField="status" HeaderText="status" HeaderStyle-CssClass="" ItemStyle-CssClass=""/>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="l_lable" runat="server" Text='<%# Eval("name") %>' ></asp:Label>
                            <asp:LinkButton ID="lb_delete" runat="server" OnClick="click_TreeChild" CssClass="button fa fa-times-circle text-red pull-right"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
              </asp:GridView>
            </div>
          </div>

<%--
          <div class="box no-border collapsed-box no-margin no-box-shadow" >
            <button type="button" class="btn btn-box-tool pull-right" data-widget="collapse" style=" position:absolute; right:0; margin-top:-25px" ><i class="fa fa-plus"></i>
                </button>
            <div class="box-body">
              <asp:GridView ID="gv_treechild" runat="server"  CssClass="table table-bordered table-hover dataTable no-margin" ShowHeader="false" AutoGenerateColumns="true"></asp:GridView>
            </div>
          </div>--%>
      
      </asp:Panel>


                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkUp" CssClass="fa fa-chevron-circle-up" CommandArgument = "up"  OnClick="ChangePreference" runat="server"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkDown" CssClass="button fa fa-chevron-circle-down" CommandArgument = "down"  OnClick="ChangePreference" runat="server"></asp:LinkButton>
                                    <asp:LinkButton ID="lb_delete" runat="server" OnClick="deleterights" CssClass="button fa fa-times-circle"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle Width="90"/>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                </asp:Panel>

                <asp:Panel ID="p_sys_routes" runat="server" Visible="false" style=" margin-top:0px">
                    <div class="form-group no-margin">
                    <label>Routes Collection</label> <asp:Label ID="Label3" ForeColor="Red" runat="server" Text=""></asp:Label>
                    <asp:LinkButton ID="lb_back" runat="server" OnClick="click_addback" CssClass="fa fa-arrow-circle-left pull-right"> <span class="lable">Back</span></asp:LinkButton>
                    <asp:GridView ID="gv_route" CssClass="table table-bordered table-hover dataTable no-margin" OnRowDataBound="gv_routeBound" EmptyDataText="No route available" runat="server" ShowHeader="false" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="id" HeaderStyle-CssClass="" ItemStyle-CssClass=""/>
                            <asp:BoundField DataField="treeview" HeaderText="treeview" HeaderStyle-CssClass="" ItemStyle-CssClass=""/>
                            <asp:BoundField DataField="pick" HeaderText="pick" HeaderStyle-CssClass="" ItemStyle-CssClass=""/>
                            <asp:BoundField DataField="exist"  HeaderText="exist" HeaderStyle-CssClass="" ItemStyle-CssClass=""/>
                            <asp:TemplateField HeaderText="Account">
                                <ItemTemplate>
                                    <div class="checkbox">
                                        <label>
                                            <asp:CheckBox ID="cb" runat="server" />
                                            <asp:Label ID="l_lable" runat="server" Text='<%# Eval("name") %>' ></asp:Label>
                                            (<asp:Label ID="Label1" runat="server" Text='<%# Eval("description") %>' ></asp:Label>)
                                        </label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>
                </asp:Panel>
            </div>
            <div class="modal-footer">
                <asp:Button ID="Button3" runat="server" Text="Save" OnClick="click_save" CssClass="btn btn-primary" />
            </div>
        </div>
    </div>
</div>

<asp:HiddenField ID="hf_id" runat="server" />
<asp:HiddenField ID="hf_employee" runat="server" />
<asp:HiddenField ID="hf_action" runat="server" />
<asp:HiddenField ID="hf_account" runat="server" />
<asp:Button ID="btn_edit" runat="server" OnClick="click_edit" CssClass="hide" />

<script type="text/javascript">
    function Confirm(msg) {
        var confirm_value = document.getElementById("<%= hf_action.ClientID %>")
        if (confirm("Are you sure to delete examination?"))
            confirm_value.value = "2";
        else
            confirm_value.value = "";
    }
</script>
</asp:Content>

<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
<script type="text/javascript" src="vendors/select2/dist/js/select2.full.min.js"></script>
<script type="text/javascript">
    (function ($) {
        $('.select2').select2()
    })(jQuery);
</script>

<!-- DataTables -->
<script src="vendors/datatables.net/js/jquery.dataTables.js"></script>
<script src="vendors/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>
<script>
    $(function () {
        $('#example2').DataTable()
        $('#example1').DataTable({
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            'paging': true,
            'lengthChange': false,
            'searching': true,
            'ordering': true,
            'info': true,
            'autoWidth': false
        })
    })

    $(".data-edit").click(function () {
        $("#<%= hf_id.ClientID %>").val($(this).attr("data-id"));
        $("#<%= btn_edit.ClientID %>").click();
    })
</script>
</asp:Content>
 

 