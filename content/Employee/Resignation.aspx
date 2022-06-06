<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Resignation.aspx.cs" Inherits="content_Employee_Resignation" MasterPageFile="~/content/site.master" %>
<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_coop_list">
<style type="text/css">

.box-header > .box-tools {
    position: absolute;
    right: 2px !important;
    top: 2px !important;
}
</style>

<script type="text/javascript">
    function bindRecordToEdit(id, type) {
            $.ajax({
                type: "POST",
                url: "content/employee/Resignation.aspx/bindRecordToEdit",
                data: "{id:'" + id + "',type:'" + type + "'}",
                contentType: "application/json; charset=utf-8",
                datatype: "jsondata",
                async: "true",
                success: function (response) {
                    var msg = response.d;
                    if (type == "Deleted") {
                        if (msg > 0) {
                            alert("Data updated successfully")
                            window.location.href = "kiosk_res";
                        }
                    }
                    if (type == "View") {
                        $('#divData').html(msg);
                        $("#divData").slideDown("slow");
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
    <h1>Resignation List</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Resignation List</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
          <div class="box-header">
              <h3 class="box-title">
              <asp:Button ID="btn_add" runat="server" Text="Compose" OnClick="compose" href="kiosk_comp_res" CssClass="btn btn-primary" /></h3>
            </div>
            <div class="box-body table-responsive no-pad-top">
                <table class="table table-bordered">
                <%System.Data.DataTable dt = dbhelper.getdata(getdata.resignationlist(Session["emp_id"].ToString()));
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
                  <%if (i == 0)
                     {%>
                        <tr>
                            <th>Date</th>
                            <th>Status</th>
                            <th>Action</th>
                        </tr>
                    <%}%>
                  <tr>
                  <td><% Response.Write(dr["datee"].ToString()); %></td>
                  <td><span  class="<% Response.Write(statclass); %>"><% Response.Write(dr["status"].ToString()); %></span><% Response.Write(dr["e_name"].ToString()); %></td>
                  <td>
                  <%if (dr["status"].ToString() == "Pending")
                    {%>
                  <a class="glyphicon glyphicon-remove-sign" onclick="bindRecordToEdit(<%Response.Write(dr["id"].ToString());%>,'Deleted')">Cancel</a>
                  <%}%>
                   <a class="glyphicon glyphicon-check"  onclick="bindRecordToEdit(<%Response.Write(dr["id"].ToString());%>,'View')" data-toggle="modal" data-target="#modal-default">View</a> <%--data-toggle="modal" data-target="#modal-default"--%>
                   <%if (dr["status"].ToString() == "Approved")
                   {%>
                       <a class="glyphicon glyphicon-folder-open"  href="cleardet?key=<%Response.Write(dr["id"].ToString()); %>" target="_blank">Clearance</a>
                  <%}%>
                  </td>
                </tr>
                <% i++;}%>
              </table>
              <%if (dt.Rows.Count == 0)
               { %>
              <div id="content_div_msg" class="alert alert-default">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
              </div>
              <%}%>
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
          <div id="divData"></div>
        </div>
    </div>
    </div>
</div>
<asp:HiddenField ID="id" runat="server" />
<asp:HiddenField ID="TextBox1" runat="server" />
</asp:Content>

