<%@ Page Language="C#" AutoEventWireup="true" CodeFile="exitdoc.aspx.cs" Inherits="content_Manager_exitdoc" MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_coop_list">
<style type="text/css">

.box-header > .box-tools {
    position: absolute;
    right: 2px !important;
    top: 2px !important;
}
</style>
<script type="text/javascript">
    function bindRecordToEdit(id, current_appid, type, nxtherarchy) {
        $.ajax({
            type: "POST",
            url: "content/Manager/exitdoc.aspx/bindRecordToEdit",
            data: "{id:'" + id + "',current_appid:'" + current_appid + "',type:'" + type + "',nxtherarchy:'" + nxtherarchy + "'}",
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
                        window.location.href = "clear";
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
<section class="content-header">
    <h1>Clearance List</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Clearance List</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
          <div class="box-header">
              <h3 class="box-title">
           <%--   <asp:Button ID="btn_add" runat="server" Text="Compose" OnClick="compose" href="kiosk_comp_res" CssClass="btn btn-primary" />--%></h3>
            </div>
            <div class="box-body table-responsive no-pad-top">
                <table class="table table-bordered">
                <% 
                    
                   System.Data.DataTable dt = dbhelper.getdata("select a.id,a.nextheirarchy,a.nextapproverid, LEFT(CONVERT(varchar,a.date,101),10)datee,a.status,b.LastName+', '+b.FirstName+' '+b.MiddleName e_name,a.resignid from texitclearance a left join memployee b on a.empid=b.Id where a.action is null and a.status='Pending' and a.nextapproverid=" + Session["emp_id"].ToString() + " "); 
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
                  <%if (dr["status"].ToString() == "Pending")
                    {%>
                      <a class="glyphicon glyphicon-thumbs-up" title="Approved" onclick="bindRecordToEdit(<%Response.Write(dr["id"].ToString());%>,<%Response.Write(dr["nextapproverid"].ToString());%>,'Approved',<%Response.Write(dr["nextheirarchy"].ToString());%>)"></a>
                      <a class="glyphicon glyphicon-remove-sign" title="Disapproved" onclick="bindRecordToEdit(<%Response.Write(dr["id"].ToString());%>,<%Response.Write(dr["nextapproverid"].ToString());%>,'Deleted',<%Response.Write(dr["nextheirarchy"].ToString());%>)"></a>
                  <%} %>
                   <a class="glyphicon glyphicon-folder-open"  href="cleardet?key=<%Response.Write(dr["resignid"].ToString()); %>" target="_blank">Clearance</a>
                                     </td>
                </tr>
                <% i++;} %>
              </table>
              <%if (dt.Rows.Count == 0)
                { %>
              <div id="content_div_msg" class="alert alert-default">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
              </div>
              <%} %>
            </div>
          </div>
        </div>
    </div>
</section>


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
