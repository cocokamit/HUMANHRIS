<%@ Page Language="C#" AutoEventWireup="true" CodeFile="verify_clearance.aspx.cs" Inherits="content_hr_verify_clearance" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_coop_list">
<style type="text/css">
    .no-margin ,.alert-empty {margin:0 !important}
</style>
<script type="text/javascript">
    function bindRecordToEdit(id, type) {
        $.ajax({
            type: "POST",
            url: "content/hr/verify_clearance.aspx/bindRecordToEdit",
            data: "{id:'" + id + "',type:'" + type + "'}",
            contentType: "application/json; charset=utf-8",
            datatype: "jsondata",
            async: "true",
            success: function (response) {
                var msg = response.d;
                if (type == "View") {
                    $('#divData').html(msg);
                    $("#divData").slideDown("slow");
                }
                else {
                    if (msg > 0) {
                        alert("Data updated successfully")
                        window.location.href = "reslistver";
                    }
                }

            },
            error: function (response) {
                alert(response.status + ' ' + response.statusText);
            }
        });
    }
</script>
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server" ID="conten_leaveList">
<div class="page-title">
    <div class="title_left">
        <h3>Clearance</h3>
    </div>  
    <div class="title_right">
       <ul>
        <li><a href="dashboard"><i class="fa fa fa-dashboard"></i> Dashboard</a></li>
        <li><i class="fa fa-angle-right"></i></li>
        <li><a href="dashboard">Verification</a></li>
        <li><i class="fa fa-angle-right"></i></li>
        <li>Clearance</li>
       </ul>
    </div>       
</div>
 <div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
    <div class="x_panel">
        <div class="x_content">
            <table class="table table-bordered no-margin">
                <% 
                    
                   System.Data.DataTable dt = dbhelper.getdata("select a.id,a.nextheirarchy,a.nextapproverid, LEFT(CONVERT(varchar,a.date,101),10)datee,a.status,b.LastName+', '+b.FirstName+' '+b.MiddleName e_name from texitclearance a left join memployee b on a.empid=b.Id where a.action is null and a.status='Verification' ");
                   int i = 0;
                   foreach (System.Data.DataRow dr in dt.Rows)
                   {
                       string statclass = null;
                       if (dr["status"].ToString() == "Pending")
                           statclass = "label label-warning";
                       if (dr["status"].ToString() == "Verification")
                           statclass = "label label-primary";
                       if (dr["status"].ToString() == "Approved")
                           statclass = "label label-success";
                       if (dr["status"].ToString() == "Disapproved")
                           statclass = "label label-danger";
                  %>
                  <% if (i == 0)
                     {%>
                        <tr>
                            <th>Date</th>
                            <th>Employee</th>
                            <th>Status</th>
                            <th>Action</th>
                        </tr>
                    <%} %>
                  <tr>
                  <td><% Response.Write(dr["datee"].ToString()); %></td>
                    <td><% Response.Write(dr["e_name"].ToString()); %></td>
                  <td><span  class="<% Response.Write(statclass); %>"><% Response.Write(dr["status"].ToString()); %></span></td>
                   <td>
                  <%if (dr["status"].ToString() == "Verification")
                    {%>
                      <a class="glyphicon glyphicon-thumbs-up" title="Approved" onclick="bindRecordToEdit(<%Response.Write(dr["id"].ToString());%>,'Approved')"></a>
                      <a class="glyphicon glyphicon-remove-sign" title="Disapproved" onclick="bindRecordToEdit(<%Response.Write(dr["id"].ToString());%>,'Deleted')"></a>
                     
                  <%} %>
                   <a class="glyphicon glyphicon-check" title="View" onclick="bindRecordToEdit(<%Response.Write(dr["id"].ToString());%>,'View')" data-toggle="modal" data-target="#modal-default"></a>
                  </td>
                </tr>
                <% i++;} %>
              </table>
              <%if (dt.Rows.Count == 0)
                { %>
              <div id="content_div_msg" class="alert alert-empty">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
              </div>
              <%} %>
        </div>
    </div>
    </div>
</div>

<div class="modal fade in" id="modal-default">
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title">Resignation Letter</h4>
        </div>
        <div class="modal-body">
          <div class="form-group">
                  <div id="divData"></div>
          </div>
        </div>
    </div>
    </div>
</div>
<asp:HiddenField ID="id" runat="server" />
<asp:HiddenField ID="TextBox1" runat="server" />
</asp:Content>
