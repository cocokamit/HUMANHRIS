<%@ Page Title="" Language="C#" MasterPageFile="~/content/site.master" AutoEventWireup="true" CodeFile="attinout.aspx.cs" Inherits="content_Manager_attinout" %>
<%@ Import Namespace="System.Data" %>

<asp:Content ID="head" ContentPlaceHolderID="head" Runat="Server">
    <!-- bootstrap datepicker -->
    <link href="vendors/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css" rel="stylesheet" type="text/css" />

    <!--SELECT-->
    <link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .dataTables_filter {width:100% !important}
        .dataTables_filter label { display:inherit; float:right !important}
        .dataTables_filter input {border-color:#d2d6de !imortant; width:300px !important; border-radius:3px; padding:17px 10px !important; box-shadow:none !important;}
        .dataTables_filter i {margin-left:-25px; color:#d2d6de; margin-top:-25px; position:absolute}
        
        @-moz-document url-prefix() {
                .dataTables_filter i { margin-top:11px !important}
            }
        .table-bordered {margin-top:5px !important}
        .table-bordered > thead > tr > td, .table-bordered > thead > tr > th {border-bottom-width:1px !important}
        .dataTables_paginate {margin-top:-20px}
        .dataTables_paginate a {background: #fff !important;}
        .dataTables_paginate .active a { padding:6.5px 10px !important; background-color: #337ab7 !important;border-color: #337ab7; !important}
        
        .pagination { display: inherit; float:right}
        .form-group { margin-bottom:10px !important}
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_dtr">
<section class="content-header">
    <h1>Employee In & Out</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li><a href="javascript:void(0)"> Report</a></li>
    <li class="active">In & Out</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-md-3">
            <div class="box box-primary">
                <div class="box-body">
                    <div class="form-group">
                        <label>From</label>
                        <asp:TextBox ID="tbFrom" runat="server" CssClass="datepicker form-control"  autocomplete="off"></asp:TextBox>
                    </div>
                    <div class="form-group no-margin">
                        <label>To</label>
                        <asp:TextBox ID="tbTo" runat="server" CssClass="datepicker form-control"  autocomplete="off"></asp:TextBox>
                    </div>
                    <div id="pnlSection" runat="server" class="form-group" style="margin:10px 0 0 0 !important">
                        <label>Section</label>
                        <asp:DropDownList ID="ddl_section" ClientIDMode="Static" CssClass="select2" runat="server"></asp:DropDownList>
                    </div>
                </div>
                <div class="box-footer">
                    <asp:Button ID="b_go" runat="server" Text="GO" OnClick="btn_go" CssClass="btn btn-primary" style="margin:0 0 10px 10px"/>
                </div>
            </div>
        </div>
        <div class="col-xs-9">
          <div class="box box-primary">
            <div class="box-body">
                <div id="alert" runat="server" class="alert alert-default alert-dismissible">
                    <i class="icon fa fa-info-circle"></i> No record found
                 </div>
                 <asp:Panel ID="pnl_grid" runat="server">
                <table id="example1" class="table table-bordered table-hover dataTable no-margin">
                    <thead>
                        <tr>
                            <th>ID Number</th>
                            <th>Name</th>
                            <th>Date</th>
                            <th>Check Type</th>
                        </tr>
                    </thead>
                    <tbody>
                    <% DataTable dt = (DataTable)ViewState["data"]; %>
                    <% foreach (DataRow row in dt.Rows) { %>
                    <tr>
                        <td><%=row["idnumber"]%></td>
                        <td><%=row["e_name"]%></td>
                        <td><%=row["Date_Time"]%></td>
                        <td><%=row["checktype"]%></td>
                    </tr>
                    <% } %>
               
                    </tbody>
                    </table>
                </asp:Panel>

           
            </div>
          </div>
        </div>
    </div>
</section>

<div id="modal_ta" runat="server" runat="server" class="modal fade in">
    <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="LinkButton1" runat="server" class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h4 class="modal-title">Time Adjustment</h4>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label>Date</label>
                    <asp:Label ID="lbl_date" CssClass="form-control" runat="server" ></asp:Label>
                </div>

                <div class="form-group">
                    <label>Log In</label>
                    <div class="row">
                        <div class="col-lg-6">
                             <asp:TextBox ID="txt_in1_date"  runat="server"   type="Date" CssClass="form-control"></asp:TextBox> 
                        </div>
                        <div class="col-lg-6">
                            <asp:TextBox ID="txt_in1_time" runat="server" Text="18:30" type="Time" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                </div>

                <div class="form-group">
                    <label>Time Out 2</label>
                    <div class="row">
                        <div class="col-lg-6">
                            <asp:TextBox ID="txt_out2_date"  runat="server" type="Date" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-lg-6">
                            <asp:TextBox ID="txt_out2_time" runat="server" type="Time" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>

                
                <div id="brek" runat="server">
                    <div class="form-group">
                        <label>Time Out 1</label>
                        <asp:TextBox ID="txt_out1_date"  runat="server" type="Date" CssClass="form-control"></asp:TextBox>
                        <asp:TextBox ID="txt_out1_time" runat="server" type="Time" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Time In 2</label>
                        <asp:TextBox ID="txt_in2_date"  runat="server" type="Date" CssClass="form-control"></asp:TextBox>
                        <asp:TextBox ID="txt_in2_time" runat="server" type="Time" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                
                <div class="form-group">
                    <label>Reason</label>
                    <asp:DropDownList ID="txt_reason" runat="server" DataSourceID="sql_income" DataTextField="manual_type" DataValueField="Id" ClientIDMode="Static" AppendDataBoundItems="true" >
                    <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="sql_income" runat="server" 
                    ProviderName="System.Data.SqlClient"  
                    SelectCommand="select * from time_adjustment">
                    </asp:SqlDataSource>
                </div>
                <div class="form-group no-margin">
                    <label>Notes</label>
                    <asp:TextBox ID="txt_remarks" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
                    <asp:Label ID="lbl_remarks" style=" color:Red;" runat="server" Text=""></asp:Label>
                </div>
            </div>
            <div class="box-footer pad">
                <asp:Button ID="btn_add" runat="server" Text="ADD" CssClass="btn btn-primary"/>
                <asp:Label ID="lbl_errmsg" runat="server" ForeColor="Red" ></asp:Label>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdn_scid" runat="server" />
    <asp:HiddenField ID="nightshift" runat="server" />
     <asp:HiddenField ID="hdn_rd" runat="server" />
</div>
<div id="modal_ot" runat="server" runat="server" class="modal fade in">
    <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="LinkButton2" runat="server"  class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h4 class="modal-title">Overtime Request</h4>
            </div>
            <div class="modal-body">
              <div class="form-group">
                     <label>OT Reg. Hours</label>
                     <asp:Label ID="hdn_reg_othrs" runat="server" ForeColor="Red" CssClass="form-control"></asp:Label>
                  
                </div>
                  <div class="form-group">
                  <label>OT Night Hours</label>
                    <asp:Label ID="hdn_night_othrs" runat="server" ForeColor="Red" CssClass="form-control"></asp:Label>
                  </div>
                <div class="form-group">
                    <label>Date :</label>
                    <asp:Label ID="lbl_ddate" style=" color:Red;" runat="server" CssClass="form-control"></asp:Label>
                </div>
                <div class="form-group">
                    <label>Time Out</label>
                    <div class="row">
                        <div class="col-lg-6">
                            <asp:TextBox ID="TextBox1"  runat="server" Enabled="false" type="Date" CssClass="form-control"></asp:TextBox><%--Text="2018-06-19"--%>
                        </div>
                        <div class="col-lg-6">
                            <asp:TextBox ID="TextBox2" runat="server" Enabled="false" type="Time" CssClass="form-control"></asp:TextBox><%--Text="18:30" --%>
                        </div>
                    </div>
                </div>
                <div class="form-group no-margin">
                    <label>Note</label>
                     <asp:TextBox ID="txt_otremarks" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="box-footer pad">
                <asp:Button ID="Button2" runat="server" Text="Process"  CssClass="btn btn-primary"/>
                <asp:Label ID="lbl_err_ot" runat="server" ForeColor="Red" ></asp:Label>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdn_setupout" runat="server" />
    <asp:HiddenField ID="hdn_actout" runat="server" />
</div>
<div id="modal_os" runat="server" runat="server" class="modal fade in">
    <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="LinkButton3" runat="server"  class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h4 class="modal-title">Offset Request<asp:label ID="hdn_alocated_bal" style=" display:none;" runat="server"/></asp:label></h4>
            </div>
            <div class="modal-body">
                <asp:GridView ID="grid_temp_ot" AutoGenerateColumns="false"   CssClass="table table-striped table-bordered no-margin" runat="server">
                    <Columns>
                        <asp:BoundField DataField="date" HeaderText="Date"/>
                        <asp:BoundField DataField="excess" HeaderText="Excess Hours"/>
                       <%-- <asp:BoundField DataField="excess" HeaderText="Balance"/>--%>
                     
                        <asp:TemplateField>
                            <ItemTemplate> 
                                <asp:CheckBox ID="chk_" runat="server"  AutoPostBack="true" />                  
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div class="form-group">
                     <label>Total Excess Hours</label>
                    <asp:Label ID="lbl_teh" runat="server" ForeColor="Red" CssClass="form-control"></asp:Label>
                </div>
                <div class="form-group">
                    <label>Date</label>
                    <asp:TextBox ID="txt_off_date" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Time Out</label>
                    <asp:TextBox ID="txt_off_out" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Offset Hrs</label>
                    <asp:TextBox ID="txt_off_hrs" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group" style=" display:none">
                    <label>Date Offset</label>
                    <asp:DropDownList ID="ddl_doffset" runat="server"></asp:DropDownList>
                </div>
                <div class="form-group">
                    <label>Reason</label><asp:Label ID="lbl_off_reason" style=" color:Red;" runat="server"></asp:Label>
                    <asp:TextBox ID="txt_off_reason" TextMode="MultiLine" style=" resize:none;" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="lbl_off_err" runat="server" style=" display:none;" ForeColor="Red" CssClass="form-control"></asp:Label>
                </div>
            </div>
            <div class="box-footer pad">
                <asp:Button ID="Button3" runat="server"  Text="Proccess" Visible="false"  CssClass="btn btn-primary"/>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="HiddenField2" runat="server" />
    <asp:HiddenField ID="hdn_total_tempot" runat="server" />
      <asp:HiddenField ID="hdn_max_offset" runat="server"/>
</div>
<div id="modal_cws" runat="server" runat="server" class="modal fade in">
    <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="LinkButton4" runat="server"  class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h4 class="modal-title">Change Shift</h4>
            </div>
            <div class="modal-body">
             <div class="form-group">
                <label>Date</label>
                <asp:TextBox ID="txt_date" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Shift Code</label>
                <asp:DropDownList ID="ddl_shiftcode" runat="server" CssClass="form-control" >
                </asp:DropDownList>
            </div>
            <div class="form-group no-margin">
                <label>Remarks</label>
                <asp:TextBox ID="txt_r" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
   
            </div>
            <div class="box-footer pad">
                <asp:Button ID="Button4" runat="server"  Text="Save" CssClass="btn btn-primary"/>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="key" runat="server" />
    <asp:HiddenField ID="dd" runat="server" />
</div>
<div id="WV" runat="server" runat="server" class="modal fade in">
    <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="LinkButton5" runat="server"  class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h4 class="modal-title">Work Verification</h4>
            </div>
            <div class="modal-body">
            <div class="form-group">
               <asp:RadioButtonList ID="rbl_class" Enabled="false" runat="server"  AutoPostBack="true">
               <asp:ListItem Value="RD">Rest Day</asp:ListItem>
               <asp:ListItem Value="HD">Holliday</asp:ListItem>
               </asp:RadioButtonList>
            </div>
            <div class="form-group">
                <label>Date</label>
                <asp:Label ID="Label1" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_otd" CssClass="txt_f form-control" Enabled="false" AutoComplete="False" runat="server"></asp:TextBox>
            </div>
            <div id="sc" runat="server" class="form-group" style=" display:none">
                <label>Shift Code</label>
                <asp:Label ID="lbl_sc" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:dropdownlist ID="ddl_sc" CssClass="datee form-control" AutoComplete="False" runat="server"></asp:dropdownlist>
            </div>
            <div class="form-group">
                <label>Reason</label>
                <asp:Label ID="lbl_reason" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_lineremarks" TextMode="MultiLine" CssClass="nobel form-control" AutoComplete="False" runat="server"></asp:TextBox>
            </div>
            </div>
            <div class="box-footer pad">
                 <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="btn btn-primary"/>
            </div>
        </div>
    </div>






    <asp:HiddenField ID="HiddenField3" runat="server" />
    <asp:HiddenField ID="HiddenField4" runat="server" />
    <asp:HiddenField ID="HiddenField5" runat="server" />
</div>
<div id="hdrd_offset" runat="server"  class="modal fade in">
     <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="LinkButton6" runat="server" class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h4 class="modal-title">RD/HD Offsetting</h4>
            </div>
            <div class="modal-body">
            <div class="form-group">
               <asp:RadioButtonList ID="rbl_rdhd" Enabled="false" runat="server">
               <asp:ListItem Value="RD">Rest Day</asp:ListItem>
               <asp:ListItem Value="HD">Holliday</asp:ListItem>
               </asp:RadioButtonList>
            </div>
            <div class="form-group">
                <label>Date From</label>
                <asp:Label ID="Label2" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_d_from" CssClass="txt_f form-control" Enabled="false" AutoComplete="False" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Date To</label>
                <asp:Label ID="Label5" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_d_to" CssClass="txt_f form-control" AutoComplete="False" runat="server"></asp:TextBox>
            </div>

            <div class="form-group">
                <label>Number of HRS offset</label>
                <asp:Label ID="Label3" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_noho" CssClass="form-control" AutoComplete="False" runat="server" Enabled="false"></asp:TextBox>
            </div>
           
            <div class="form-group">
                <label>Reason</label>
                <asp:Label ID="Label4" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_reason_offsethdrd" TextMode="MultiLine" CssClass="nobel form-control" AutoComplete="False" runat="server"></asp:TextBox>
            </div>
            </div>
            <div class="box-footer pad">
                 <asp:Button ID="Button5" runat="server" Text="Save" CssClass="btn btn-primary"/>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="HiddenField6" runat="server" />
    <asp:HiddenField ID="HiddenField7" runat="server" />
</div>



<asp:HiddenField ID="payrange" runat="server" />
</asp:Content>

<asp:Content ID="footer" ContentPlaceHolderID="footer" Runat="Server">
<!--Select-->
<script type="text/javascript" src="vendors/select2/dist/js/select2.full.min.js"></script>
<script type="text/javascript">
    (function ($) {
        $('.select2').select2()
    })(jQuery);
</script>

<!-- bootstrap datepicker -->
<script type="text/javascript" src="vendors/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js"></script>
<script>
    $(function () {
        //Date picker
        $('.datepicker').datepicker({
            autoclose: true
        })
    })
</script>

<!-- DataTables -->
<script src="vendors/datatables.net/js/jquery.dataTables.js"></script>
<script src="vendors/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>
<script>
    $(function () {
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

