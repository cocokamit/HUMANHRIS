<%@ Page Language="C#" AutoEventWireup="true" CodeFile="changeMWE.aspx.cs" Inherits="content_hr_changeMWE" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ID="head" ContentPlaceHolderID="head" Runat="Server">


    <!--SELECT-->
    <link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />
    <script src="script/auto/myJScript.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
    <link href="script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
    <script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>

    
    <style type="text/css">
        .table-input input[type=text] {border:none; background: transparent}
        .table-input select {border:none; background: transparent; -moz-appearance: none; -webkit-appearance:none;}
        .datetimepicker { position:absolute; z-index:9999}
        .dataTables_filter i { border:1px solid red;    margin-left: -25px;    color: #d2d6de;    margin-top: -25px;    position: absolute; }
        .alert {padding:10px; margin-top:10px !important}
        .hidden{ display:none;}
       
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $.noConflict();
            $(".auto").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "content/hr/changeMWE.aspx/GetEmployee",
                        data: "{'term':'" + $(".auto").val() + "'}",
                        dataType: "json",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split('~')[1],
                                    val: item.split('~')[0]
                                }
                            }))
                        },
                        error: function (result) {
                            alert(result.responseText);
                        }
                    });
                },
                select: function (e, i) {
                    index = $(".auto").parent().parent().index();
                    $("#lbl_bals").val(i.item.val);
                }
            });
        });
    </script>
    <script type="text/javascript" language="javascript">
    function JqueryAjaxCall(id,rows,empid) {
            $.ajax({
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                url: "content/hr/changeMWE.aspx/cancel",
                data: "{'id':'" + id + "','empid':'" + empid + "'}",
                dataType: 'json',
                success: function (data) 
                {
                    onSuccess(data,rows);
                    document.getElementById("tbl").deleteRow(rows);
                },
                error: function (data, success, error) 
                {
                    alert("Error : " + error);
                }
            });
            return false;
        }
        function onSuccess(data, rows) 
        {
            if (data.d[1] == 0)
            $(tbl).find('tbody').append("<tr> " +
                        "<td colspan='6'> " +
                            "<div  class='alert alert-empty'> " +
                                "<i class='fa fa-info-circle'></i> " +
                                "<span>No record found</span> " +
                            "</div> " +
                        "</td> " +
                    "</tr>");
            alert(data.d[0]);
        }
    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="content" Runat="Server">
 <div class="page-title">
    <div class="title_left hd-tl">
        <h3>Set up New Payroll Rate</h3>
    </div>   
    <div class="title_right">
        <ul>
            <li><a href="dashboard?user_id=<% Response.Write(Session["user_id"]); %>"><i class="fa fa-dashboard"></i> Dashboard</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li><a href="adddtrlogs?user_id=<% Response.Write(Session["user_id"]); %>"> DTR</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>Attendance</li>
        </ul>
    </div>
</div>
<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
           <div class="pull-right">
               <div class="form-group">
                    <asp:FileUpload ID="FileUpload"  runat="server" />
               </div>
           </div>
           <table id="tbl" class="table table-striped table-bordered nowrap ">
                <thead>
                    <tr>
                        <td><asp:TextBox ID="txt_emp_name" runat="server" ClientIDMode="Static" CssClass="form-control auto"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="dll_type_rate" runat="server" CssClass="form-control">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                                <asp:ListItem Value="1">Monthly Paid</asp:ListItem>
                                <asp:ListItem Value="2">Daily Paid</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td><asp:TextBox ID="txt_effectivity" runat="server" CssClass="form-control orley"></asp:TextBox></td>
                        <td><asp:TextBox ID="txt_rate" runat="server" CssClass="form-control" AutoComplete="off" onkeyup="decimalinput(this)"></asp:TextBox></td>
                        <td><asp:TextBox ID="txt_remarks" runat="server" CssClass="form-control"></asp:TextBox></td>
                        <td><asp:Button ID="Button3" runat="server"  OnClick="save_cpr" Text="ADD" CssClass="btn btn-primary btn-sm"/></td>
                        </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Employee</td>
                        <td>Payroll Type</td>
                        <td>Effectivity Date</td>
                        <td>New Rate</td>
                        <td>Remarks</td>
                        <td>Action</td>
                    </tr>
                    <% var cpr = from a in datacontext.Context.GetTable<app_trn>()
                        join b in datacontext.Context.GetTable<app_trn_salaryinc>() on a.id equals b.app_trn_id
                        join c in datacontext.Context.GetTable<MEmployee>() on a.empid equals c.Id
                        where a.action==null
                        select new
                        { 
                            id=a.id,
                            employee = c.LastName + ", " + c.FirstName + " " + c.MiddleName,
                            pt = b.paytypeid==1?"Monthly Paid":"Daily Paid",
                            ed = b.effective_date.Value.Month + "/" + b.effective_date.Value.Day + "/" + b.effective_date.Value.Year,
                            nr = b.mr == 0 ? b.dr : b.mr,
                            remarks = a.remarks,
                            empid=a.empid
                        };
                        %>
                        <% int rows = 2;
                        foreach (var data_list in cpr.ToList())
                        {%>
                            <tr>
                                <td><%=data_list.employee %></td>
                                <td><%=data_list.pt%></td>
                                <td><%=data_list.ed%></td>
                                <td><%=data_list.nr%></td>
                                <td><%=data_list.remarks%></td>
                                <td>
                                    <a  onclick="return JqueryAjaxCall(<%=data_list.id %>,<%= rows %>,<%=data_list.empid %>);" ><i class="glyphicon glyphicon-remove-sign"></i></a>
                                </td>
                            </tr>
                        <%rows++;
                        } %>
                        <% if (cpr.Count() == 0)
                        {%>
                        <tr>
                            <td colspan="6">
                                <div id="Div1" runat="server" class="alert alert-empty">
                                    <i class="fa fa-info-circle"></i>
                                    <span>No record found</span>
                                </div>
                            </td>
                        </tr>
                        <%}%>
                </tbody>
            </table>
        </div>
    </div>
</div>
<asp:HiddenField ID="lbl_bals" ClientIDMode="Static" runat="server" />
</asp:Content>
<asp:Content ID="footer" ContentPlaceHolderID="footer" Runat="Server">
  <!--Datepicker-->
    <script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
       <script type="text/javascript">
           jQuery.noConflict();
           (function ($) {
               $(function () {
                   $(".orley").datepicker();
               });
           })(jQuery);
        </script>
</asp:Content>