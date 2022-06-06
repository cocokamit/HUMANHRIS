<%@ Page Language="C#" AutoEventWireup="true" CodeFile="demographic.aspx.cs" Inherits="content_report_demographic" MasterPageFile="~/content/MasterPageNew.master" %>
<%@ Import Namespace="System.Data" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <title>Cebu LandMasters Demographics</title>
    <!-- DataTables -->
    <link href="vendors/datatables.net-bs/css/custom.css" rel="stylesheet" type="text/css" />
    <link href="vendors/datatables.net-bs/css/dataTables.bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="vendors/datatables.net-fixedColumns/css/fixedColumns.bootstrap.min.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        hr{margin:0px 0 8px 0 !important}
        .alert {margin-top:0 !important;  margin-bottom:0 !important}
        .x-head {margin-bottom:8px} 
        .filter .table { margin-bottom:0 !important}
        .filter .table,.filter .table td{border:none; padding:0 !important}
        .filter .table td label {margin-left:5px}
    </style>
</asp:Content>

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Demographic</h3>
    </div>   
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-3">
         <div class="x_panel filter" >
            <div class="form-group">
                <label>Payroll Group</label>
                <asp:DropDownList ID="ddlPayrollGroup" runat="server"></asp:DropDownList>
            </div>
         <div style=" overflow:auto; height:70vh">
             <asp:CheckBox ID="cball" runat="server" Text="&nbsp;Select All" OnCheckedChanged="checkallfields" AutoPostBack="true" />
            <asp:GridView ID="gvFields" runat="server" AutoGenerateColumns="false" ShowHeader="false" CssClass="table">
                <Columns>
                    <asp:BoundField DataField="rid" HeaderText="Report ID" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="title" HeaderText="field" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="description" HeaderText="escription" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="cbField" runat="server" Text='<%# Eval("description")%>'  />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
            <hr />
            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="ClickGo" CssClass="btn btn-primary" />
         </div>
    </div>
    <div class="col-md-9">
    <div class="x_panel">
        <div id="alert" runat="server" class="alert alert-default no-margin">
            <i class="fa fa-info-circle"></i>
            <span>No record found</span>
        </div> 

        <% if(ViewState["data"] != null) {%>
        <table id="tbl" class="table table-striped table-bordered nowrap ">
            <thead>
                <tr>
                    <% DataTable dtColumns = (DataTable)ViewState["data"]; %>
                    <% foreach (DataColumn row in dtColumns.Columns) { %>
                    <td><%=row.ToString().Replace("_"," ") %></td>
                    <% } %>
                </tr>
                </thead>
                <tbody>
                 <% DataTable dtRows = (DataTable)ViewState["data"]; %>
                 <% foreach (DataRow row in dtRows.Rows){%>  
                 <tr>
                 <% foreach (DataColumn col in dtColumns.Columns) { %>
                      <td><%=row[col]%></td>
                  <% } %>
                  </tr>
                  <% } %>
            </tbody>
        </table>
        <% } %>

 
       



<asp:GridView ID="grid_view" runat="server" OnPageIndexChanging="OnPageIndexChanging" AutoGenerateColumns="false" CssClass="GridViewStyle">
<HeaderStyle  CssClass="GridViewHeaderStyle" />
<Columns>
                       
    <asp:BoundField DataField="IdNumber" HeaderText="Id Number"/>
    <asp:BoundField DataField="lastname" HeaderText="Last Name"/>
    <asp:BoundField DataField="firstname" HeaderText="First Name"/>
    <asp:BoundField DataField="middlename" HeaderText="Middle Name"/>
    <asp:BoundField DataField="extensionname" HeaderText="Extension Name"/>
    <asp:BoundField DataField="pre_address" HeaderText="Pressent Address"/>
    <asp:BoundField DataField="permanentaddress" HeaderText="Permanent Adress"/>
    <asp:BoundField DataField="phonenumber" HeaderText="Phone Number"/>
    <asp:BoundField DataField="cellphonenumber" HeaderText="Cellphone Number"/>
    <asp:BoundField DataField="emailaddress" HeaderText="Email Address"/>
    <asp:BoundField DataField="placeofbirth"  HeaderText="Place of Birth"/>
    <asp:BoundField DataField="zipcode" HeaderText="Zipcode"/>
    <asp:BoundField DataField="bloodtype" HeaderText="Blood Type"/>
    <asp:BoundField DataField="sex" HeaderText="Sex"/>
    <asp:BoundField DataField="civilstatus" HeaderText="Civil Status"/>
    <asp:BoundField DataField="citizenship" HeaderText="Citizenship"/>
    <asp:BoundField DataField="religion" HeaderText="Religion"/>
    <asp:BoundField DataField="height" HeaderText="Hieght"/>
    <asp:BoundField DataField="weight" HeaderText="Weight"/>
    <asp:BoundField DataField="gsisnumber" HeaderText="GSIS"/>
    <asp:BoundField DataField="sssnumber" HeaderText="SSS Number"/>
    <asp:BoundField DataField="hdmfnumber" HeaderText="HDMF Number"/>
    <asp:BoundField DataField="phicnumber" HeaderText="Phil. Health Number"/>
    <asp:BoundField DataField="tin" HeaderText="TIN Number"/> 
    <asp:BoundField DataField="datehired" HeaderText="Date Hired"/>
    <asp:BoundField DataField="dateregularization" HeaderText="Date of Regularization"/>
    <asp:BoundField DataField="Date_of_Resignation" HeaderText="Date of Resignation"/>
    <asp:BoundField DataField="taxcode" HeaderText="Tax Code"/>
    <asp:BoundField DataField="company" HeaderText="Company"/>
    <asp:BoundField DataField="department" HeaderText="Department"/>
    <asp:BoundField DataField="division" HeaderText="Divission"/>
    <asp:BoundField DataField="position" HeaderText="Position"/>
    <asp:BoundField DataField="payrollgroup" HeaderText="Payroll Group"/>
    <asp:BoundField DataField="Account" HeaderText="GL Account"/>
    <asp:BoundField DataField="payrolltype" HeaderText="Payroll Type"/>
    <asp:BoundField DataField="shiftcode" HeaderText="Shift Code"/>
    <asp:BoundField DataField="fixnumberofdays" HeaderText="Fix No. of Days"/>
    <asp:BoundField DataField="fixnumberofhours" HeaderText="Fix No. of Days"/>
    <asp:BoundField DataField="monthlyrate" HeaderText="Monthly Rate"/>
    <asp:BoundField DataField="payrollrate" HeaderText="Payroll Rate"/>
    <asp:BoundField DataField="dailyrate" HeaderText="Daily Rate"/>
    <asp:BoundField DataField="absentdailyrate" HeaderText="Absent Daily Rate"/>
    <asp:BoundField DataField="hourlyrate" HeaderText="Hourly Rate"/>
</Columns>
</asp:GridView>
         
        </div>

</div>
 
<asp:HiddenField ID="hf_view" runat="server" />
<asp:HiddenField ID="hf_id" runat="server" />
</asp:Content>

<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
<!-- DataTables -->
<script type="text/javascript" src="vendors/datatables.net/js/jquery.dataTables.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.6.0/js/buttons.flash.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.6.0/js/dataTables.buttons.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.6.0/js/buttons.html5.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/buttons/1.6.0/js/buttons.print.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/fixedcolumns/3.2.6/js/dataTables.fixedColumns.min.js"></script>

<script type="text/javascript">
    $(function () {
        $('#tbl').DataTable({
            'lengthChange': false,
            'searching': true,
            'ordering': true,
            'info': true,
            dom: 'Bfrtip',
            buttons: ['excel', 'print'], //'copy', 'csv', 'pdf',
            scrollY: $(window).height() - 240,
            scrollX: true,
            scrollCollapse: true,
            paging: false,
            fixedColumns: {
                leftColumns: 4
            }
        })
    })
</script>
</asp:Content>



