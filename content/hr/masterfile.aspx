<%@ Page Title="" Language="C#" MasterPageFile="~/content/MasterPageNew.master" AutoEventWireup="true" CodeFile="masterfile.aspx.cs" Inherits="content_hr_masterfile" %>
<%@ Import Namespace="System.Data" %>

<asp:Content ID="head" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .minimal { position:absolute; z-index:5}
        .table {border-bottom:2px solid #dddddd}
        .table thead tr th { line-height:35px}
        .table tbody tr td { line-height:40px}
        .page-title {margin-bottom:39px}
        .dataTables_filter {width:100% !important}
        .dataTables_filter input {border-color:#d2d6de !imortant; width:300px !important; padding:17px 10px !important; box-shadow:none !important;}
        .dataTables_filter i {margin-left:-25px; color:#d2d6de; margin-top:-25px; position:absolute}
        @-moz-document url-prefix() {
                .dataTables_filter i { margin-top:11px !important}
            }
        .dataTables_paginate {margin-top:-20px}
        .dataTables_paginate a {background: #fff !important;}
        .dataTables_paginate .active a { padding:6.5px 10px !important; background-color: #337ab7 !important;border-color: #337ab7; !important}
    </style>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="content" Runat="Server">
<div class="page-title">
    <div class="title_left">
        <h3>Master File</h3>
    </div>   
    
    <div class="title_right">

      <%
          string id = Session["role"].ToString() == "Admin" ? "0" : Session["emp_id"].ToString(); %>
          <input type="text" id="empid" style="display:none !important" value="<%=id%>"/>
                <% // here oh Import Namespace="System.Data"
                    DataTable dts=dbhelper.getdata("Select route_id from nobel_userRight where user_id="+id);
                    DataRow[] dr = dts.Select("route_id=1900");
                    if(dr.Count()>0)
                    if (dr[0]["route_id"].ToString() == "1900")
                    { %>
                    <li class="allower" value="1900" ><a href="addemployee?user_id=<%= Request.QueryString["user_id"] %>&app_id=0&tp=dd" Class="btn btn-primary">ADD</a></li>
                    <%}
                    %>
    </div>
     
</div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x_content">
                <asp:DropDownList ID="ddl_payroll_group" runat="server" AutoPostBack="true" CssClass="minimal" OnSelectedIndexChanged="OnChange_PayrollGroup"></asp:DropDownList>
		        <asp:DropDownList ID="ddl_department_group" Visible="false" runat="server" AutoPostBack="true" CssClass="minimal" OnSelectedIndexChanged="OnChange_DepartmentGroup" style="margin-left:180px"></asp:DropDownList>
                <table id="example1" class="table table-condensed">
                <thead>
                    <tr>
                      <th>ID Number</th>
                      <th>Name</th>
                      <th>Email</th>
                      <th>Phone</th>
                      <th>Position</th>
                      <th>Status</th>
                      <%= ddl_payroll_group.SelectedItem.ToString() == "Resigned" ? " <th>Effectivity</th>" : ""%>

                    </tr>
                </thead>
              <tbody>
              <% DataTable dt = (DataTable)ViewState["data"]; %>
              <% foreach (DataRow row in dt.Rows) { %>
              <tr>
                  <td>
                    <i class="fa fa-warning text-danger <%= row["setup"].ToString() == "Incomplete" ? "" : "none" %>" style="position:absolute; font-size:10px; margin-left:-4px; margin-top:5px"></i>
                    <img src="files/profile/<%=row["profile"]%>-thumb.png" alt="" onerror="this.onerror=null;this.src='files/profile/0.png'" class="img-circle" width="25px" style="margin-right:6px">
                    <a href="employee?user_id=<%= function.Encrypt(row["id"].ToString(), true) %>&app_id=<%=row["id"]%>&tp=ed"><%=row["IdNumber"]%></a>
                  </td>
                  <td class="text-capitalize"><%=row["FullName"]%></td>
                  <td><%=row["EmailAddress"]%></td>
                  <td><%=row["PhoneNumber"]%></td>
                  <td><%=row["position"]%></td>
                  <td><%= row["status"] %></td>
                  <%= ddl_payroll_group.SelectedItem.ToString() == "Resigned" ? "<td>" + row["resigneffectivity"] + "</td>" : ""%> 
              </tr>
              <% } %>
               
                </tbody>
              </table>
   
            </div>
        </div>
    </div>
</div>
 
</asp:Content>
<asp:Content ID="footer" ContentPlaceHolderID="footer" Runat="Server">
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
</script>
 
</asp:Content>

