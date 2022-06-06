<%@ Page Language="C#" AutoEventWireup="true" CodeFile="transotherincome.aspx.cs" Inherits="content_payroll_transotherincome" MasterPageFile="~/content/MasterPageNew.master" %>

<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
   
        li { list-style:none}
        input[type=date]{ line-height:normal !important}
        .emp-identity{padding:0;float:left; width:48%;}
        .emp-identity input[type=text]{padding:8px; width:100%; margin-bottom:10px}
        .emp-identity select {padding:7px; width:100%;margin-bottom:10px}
        .emp-identity input[type=checkbox] {margin-right:5px !important;}
        .popup{padding:0;float:left; width:100%;}
        .popup input[type=text]{padding:8px; width:100%; margin-bottom:10px}
        .popup select {padding:7px; width:100%;margin-bottom:10px}
        .popup input[type=checkbox] {margin-right:5px !important;}
        .bei{margin-left:25px}
        .table > tbody > tr > td { vertical-align: middle !important}
        .table input[type=text]{ width:100%; padding:5px; border:1px solid #eee}
        .dnone{ display:none;}
        .Grid {
        table-layout:fixed; 
        width:100%; 
        }
        .Grid .Shorter {
        overflow: hidden; 
        text-overflow: ellipsis;    
      
        white-space:pre-wrap;       
        }
        
        ul.bar_tabs {padding-left:0px}
       .tab-content .page-header {margin:20px 0 20px !important}
       .nav-tabs-custom > .nav-tabs > li {    border-top: 1px solid transparent;} 
       .nav-tabs-custom > .nav-tabs > li.active {border-top-color: #eee !important;}
       .checkbox {margin-left:20px}
    .nav-tabs-custom {margin-bottom:10px}
    .profile_left {margin-top:10px}
    .panel_toolbox > li > a {
        padding: 5px;
        color: #2B6384;
        font-size: 12px;
   
    }
    .x_title {
    padding: 1px 5px 6px;
    border-bottom: 1px solid #f0e6e6;
   }
   .col-lg-1, .col-lg-10, .col-lg-11, .col-lg-12, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-55, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-sm-1, .col-sm-10, .col-sm-11, .col-sm-12, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-xs-1, .col-xs-10, .col-xs-11, .col-xs-12, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9 {
    position: relative;
    min-height: 1px;
    float: left;
    padding-right: 0px;
    padding-left: 2px;
   }
   
   .pull-left {
     padding: 5px;
}
div.box {
    height: 100%;
    padding: 5px;
    overflow: auto;
    border: 1px solid #8080FF;
    border-top-color: rgb(128, 128, 255);
    background-color: #E5E5FF;
}


.x_panel {
    border-top: 3px solid #348017 !important;
}
.modal-dialog {
    width: 95%;
    margin: 30px auto;
}
.label { line-height: 0; font-size:12px;}
.disable{
   cursor: not-allowed;
   pointer-events: none;
}
</style>
<!--FREEZE GRID-->
<link href="script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
<script src="script/auto/myJScript.js" type="text/javascript"></script>
<script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>
<script type="text/javascript" src="style/js/googleapis_jquery.min.js"></script>
 <!-- SweetAlert2 -->
<link rel="stylesheet" href="vendors/sweetalert2-theme-bootstrap-4/bootstrap-4.min.css">
<!-- Toastr -->
<link rel="stylesheet" href="vendors/plugins/toastr/toastr.min.css">
<script type="text/javascript">
    $(document).ready(function () {
        $.noConflict();
        $(".auto").autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "content/payroll/transotherincome.aspx/GetEmployee",
                    data: "{'term':'" + $(".auto").val() + "','sender':'" + $("#hf_auto").val() + "'}",
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
</asp:Content>
<asp:Content ID="content_addpayroll" runat="server" ContentPlaceHolderID="content">
<asp:HiddenField ID="hf_action" runat="server" />
<div class="page-title">
    <div class="title_left">
         <h3>Other Income</h3>
    </div>   
    <div class="title_right">
    <button type="button" class="btn btn-success" data-toggle="modal" data-target="#modal-default">New</button>
       
    </div>
</div>
<div class="clearfix"></div> 
<div class="row">
<div class="col-md-12 col-sm-12 col-xs-12">
 <p></p>
        <div class="x_panel">
          <div class="nav-tabs-custom">
            <ul id="tab-list" class="nav nav-tabs" role="tablist">
              <li class="active"><a href="#activity" data-toggle="tab" aria-expanded="true">Approved Income</a></li>
              <li class=""><a href="#Draft" data-toggle="tab" aria-expanded="false">Draft</a></li>
              <%--<li class=""><a href="#history" data-toggle="tab" aria-expanded="false">History</a></li>--%>
            </ul>
            <div id="tab-content" class="tab-content">
              <div class="tab-pane active" id="activity">
                <% 
                    var OI_transaction_approved =from a in datacontext.Context.GetTable<TPayrollOtherIncome>()
                                        join b in datacontext.Context.GetTable<MPayrollGroup>() on a.PayrollGroupId equals b.Id
                                        join c in datacontext.Context.GetTable<TDTR>() on a.PeriodId equals c.Id
                                        where a.status1 == "Approved" && a.action == null orderby a.id descending
                                        select new {
                                            transid=a.id,sysdate=a.EntryDate,a.status1,
                                            remarks=a.remarks,pg=b.PayrollGroup,
                                            datefom = c.DateStart,
                                            dateto = c.DateEnd,
                                            payrollid = a.payroll_id.Value.ToString().Length == null ? "" : a.payroll_id.Value.ToString()
                                        };

                    var ff_approved = OI_transaction_approved.AsEnumerable()
                                        .Select((a, index) => new
                                        {
                                            a.transid,
                                            a.pg,
                                            a.datefom,
                                            a.dateto,
                                            a.remarks,
                                            a.sysdate,
                                            a.status1,
                                            SequenceNo = index + 1,
                                            a.payrollid
                                        });
                         %>
                         <%if (ff_approved.Count() > 0)
                           {%>
                 <table id="lbl_approved" class="table table-bordered nowrap ">
                    <thead>
                        <tr>
                            <td style="display:none;">No</td>
                            <td>No</td>
                            <td>Entry Date</td>
                            <td>Payroll Group</td>
                            <td>Payroll Range</td>
                            <td>Remarks</td>
                            <td>Status</td>
                             <td>Action</td>
                        </tr>
                    </thead>
                    <tbody>
                   
                    <% foreach (var datalist in ff_approved)
                       {
                           string disabled = datalist.payrollid.ToString().Length> 0 ? "btn btn-sm btn-danger disable" : "btn btn-sm btn-danger"; %>
                        <tr>
                            <td style="display:none;"><%=datalist.transid%></td>
                            <td><%=datalist.SequenceNo%></td>
                            <td><%=datalist.sysdate%></td>
                            <td><%=datalist.pg%></td>
                            <td><%= string.Format("{0:MM/dd/yyyy}", datalist.datefom).ToString() + " - " + string.Format("{0:MM/dd/yyyy}", datalist.dateto).ToString()%></td>
                            <td><%=datalist.remarks%></td>
                            <td><%=datalist.status1%></td>
                            <td><button id="Button1" onclick="backtodraft(this)" class="<%=disabled %>">Back to Draft</button></td>
                        </tr>
                        <%} %>
                    </tbody>
                </table>
                <%}
                           else
                           {%>
                           <div id="Div1" class="alert alert-default no-margin">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
            </div>
                <%} %>
              </div>
              <!-- /.tab-pane -->
              <div class="tab-pane" id="Draft">
               <% 
                         
                         var OI_transaction =(from a in datacontext.Context.GetTable<TPayrollOtherIncome>()
                                             join b in datacontext.Context.GetTable<MPayrollGroup>() on a.PayrollGroupId equals b.Id
                                             join c in datacontext.Context.GetTable<TDTR>() on a.PeriodId equals c.Id
                                              where a.status1=="Draft" && a.action==null
                                             select new {
                                                 transid=a.id,sysdate=a.EntryDate,a.status1,
                                                 remarks=a.remarks,pg=b.PayrollGroup,
                                                 datefom = c.DateStart,
                                                 dateto = c.DateEnd,
                                                 payrollid = a.payroll_id.Value.ToString().Length == null ? "" : a.payroll_id.Value.ToString()
                                             
                                             });
     
                                            var ff = OI_transaction.AsEnumerable()
                                                .Select((a, index) => new
                                                {
                                                    a.transid,
                                                    a.pg,
                                                    a.datefom,
                                                    a.dateto,
                                                    a.remarks,
                                                    a.sysdate,
                                                    a.status1,
                                                    SequenceNo = index + 1,
                                                    a.payrollid
                                                });
                                        %>
                                        <%if (ff.Count() > 0)
                                        {%>
                                        <table id="tbl_draft" class="table table-bordered nowrap ">
                                            <thead>
                                                <tr>
                                                    <td style=" display:none;">No</td>
                                                    <td>No</td>
                                                    <td>Entry Date</td>
                                                    <td>Payroll Group</td>
                                                    <td>Payroll Range</td>
                                                    <td>Remarks</td>
                                                    <td>Status</td>
                                                    <td>Action</td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                           <%foreach (var datalist in ff)
                                              {%>
                                                <tr>
                                                    <td style="display:none;"><%=datalist.transid%></td>
                                                    <td style="display:none;"><%=datalist.SequenceNo%></td>
                                                    <td><%=datalist.SequenceNo%></td>
                                                    <td><%=datalist.sysdate%></td>
                                                    <td><%=datalist.pg%></td>
                                                    <td><%= string.Format("{0:MM/dd/yyyy}", datalist.datefom).ToString() + " - " + string.Format("{0:MM/dd/yyyy}", datalist.dateto).ToString()%></td>
                                                    <td><%=datalist.remarks%></td>
                                                    <td><%=datalist.status1%></td>
                                                    <td>
                                                        <button id="errorr"  data-toggle="modal" data-target="#modal-error"  type="button" onclick="click_approved(<%= datalist.transid %>)" class="btn btn-primary">Approve</button>
                                                        <button id="btn-add-tab" type="button" class="btn btn-sm btn-default btn-add-tab " onclick="click_approved(<%= datalist.transid %>)">Details</button>
                                                        <button id="Button2" type="button" class="btn btn-sm btn-danger" onclick="click_cancelled(this)">Remove</button>
                                                    </td>
                                                </tr>
                                              <%}%>
                                             </tbody>
                                            </table>
                                            <%}
                                            else
                                            { %>
                                                <div id="content_alert" class="alert alert-default no-margin">
                                                    <i class="fa fa-info-circle"></i>
                                                    <span>No record found</span>
                                                </div>
                                            <%}%>
              </div>
              <!-- /.tab-pane -->
             
               <div class="tab-pane" id="trn_det">
                <div class="row">
                <div class="col-sm-4">
                    <div class="form-group">
                        <label>Payroll Group:</label>
                        <asp:Label ID="lbl_pg" ClientIDMode="Static" style=" color:Blue;" runat="server" ></asp:Label>
                    </div>
                     <div class="form-group">
                        <label>Payroll Range:</label>
                        <asp:Label ID="lbl_pr" runat="server" style=" color:Blue;" ClientIDMode="Static" ></asp:Label>
                    </div>
                </div>
                <div class="col-sm-4">
                 <div class="form-group">
                        <label>Entry Date:</label>
                        <asp:Label ID="lbl_entry" runat="server" style=" color:Blue;" ClientIDMode="Static" ></asp:Label>
                    </div>
                     <div class="form-group">
                        <label>Remarks:</label>
                        <asp:Label ID="lbl_rem_disp" runat="server" style=" color:Blue;" ClientIDMode="Static" ></asp:Label>
                    </div>
                </div>
                 <div class="col-sm-2">
                 <div class="form-group">
                         <asp:FileUpload ID="file_data" ClientIDMode="Static"  runat="server" />
                         
                    </div>
                    
                </div>
                 <div class="col-sm-2">
                     <div class="form-group">
                        <button type="button" onclick="UploadProcess()"  id="upload_files" class=" btn btn-default btn-sm">Load Excel</button>
                     </div>
                 </div>
               
                <div class="col-sm-12">
                    <table id="tbl_det" class="table table-bordered nowrap ">
                        <thead>
                            <tr>
                                <td><span style=" color:Red;">*</span>Enployee</td>
                                <td><span style=" color:Red;">*</span>Income Type</td>
                                <td>Allowed Income</td>
                                <td><span style=" color:Red;">*</span>Work Hours</td>
                                <td><span style=" color:Red;">*</span>Non Taxable</td>
                                <td><span style=" color:Red;">*</span>Taxable</td>
                                <td>Action</td>
                            </tr>
                            <tr>
                                <td><asp:TextBox ID="txt_searchemp_his"  required="required" runat="server"  ClientIDMode="Static"  Placeholder="Search Employee" class="auto " ></asp:TextBox></td>
                                <td><asp:DropDownList ID="ddl_income_type" required="required" ClientIDMode="Static" runat="server" class="form-control " > </asp:DropDownList></td>
                                <td><input type="text" id="allowed_income_his" disabled class="form-control"/></td>
                                <td><input type="text" id="work_hrs_his" AutoComplete="off" onkeyup="decimalinput(this);input_work_hrs('history')" required class="form-control "/></td>
                                <td><input type="text" id="nontax_his" AutoComplete="off" onkeyup="decimalinput(this)" required class="form-control ntt"/></td>
                                <td><input type="text" id="taxable_his" AutoComplete="off" onkeyup="decimalinput(this)" required class="form-control tax"/></td>
                                <td>
                                     <a  onclick="add_to_table('his')"><i class="fa fa-plus"></i></a>  
                                     <a  onclick="delete_rows('his')"><i class="fa fa-trash"></i></a>
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    
                </div>
              </div> 
              </div>
              <!-- /.tab-pane -->
             
            </div>
            <!-- /.tab-content -->
          </div>
          <!-- /.nav-tabs-custom -->
        </div>
        </div>
</div>
<!--Modal-->
<div class="modal" id="modal-default">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">×</span></button>
                  <h4 class="modal-title">New Transaction</h4>
                  <button type="button" style="display:none;" id="btn_success" class="btn btn-success swalDefaultSuccess" data-dismiss="modal">
                  Launch Success Toast
                </button>
              </div>
              <div class="modal-body">
              <div class="row">
                <asp:DropDownList ID="ddl_payroll_group" ClientIDMode="Static" onchange="selectpg()" class="form-control input-sm pull-left" style=" width:30%; height:33px;" runat="server"></asp:DropDownList>
                <asp:DropDownList ID="ddl_pg"  runat="server" ClientIDMode="Static"  class="form-control input-sm pull-left" style=" width:30%; height:33px;" ></asp:DropDownList>
                <button type="button" onclick="verify_data()" class="btn btn-primary">Verify</button>
                <div class="col-sm-12">
                    <table id="tbl" class="table table-bordered nowrap ">
                        <thead>
                            <tr>
                                <td><span style=" color:Red;">*</span>Employee</td>
                                <td><span style=" color:Red;">*</span>Income Type</td>
                                <td>Allowed Income</td>
                                <td><span style=" color:Red;">*</span>Work Hours</td>
                                <td><span style=" color:Red;">*</span>Non Taxable</td>
                                <td><span style=" color:Red;">*</span>Taxable</td>
                                <td>Action</td>
                            </tr>
                            <tr>
                                <td><asp:TextBox ID="txt_searchemp"  required="required" runat="server"  ClientIDMode="Static"  Placeholder="Search Employee" class="form-control has-feedback-left auto " ></asp:TextBox></td>
                                <td><asp:DropDownList ID="ddl_type" required="required" ClientIDMode="Static" runat="server" class="form-control " > </asp:DropDownList></td>
                                <td><input type="text" id="allowed_income" disabled class="form-control"/></td>
                                <td><input type="text" id="work_hrs" AutoComplete="off" onkeyup="decimalinput(this);input_work_hrs('new')" required class="form-control "/></td>
                                <td><input type="text" id="nontax" AutoComplete="off" onkeyup="decimalinput(this)" required class="form-control ntt"/></td>
                                <td><input type="text" id="taxable" AutoComplete="off" onkeyup="decimalinput(this)" required class="form-control tax"/></td>
                                <td>
                                     <a  onclick="add_to_table('new')"><i class="fa fa-plus"></i></a>  
                                     <a  onclick="delete_rows('new')"><i class="fa fa-trash"></i></a>
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                    <div class="form-group">
                    <textarea id="compose-textarea" class="form-control" style="height: 100px; resize:none;"></textarea>
              </div>
                </div>
              </div> 
              </div>
              <div class="modal-footer">
                <button type="button" onclick="save_data()" class="btn btn-primary">Save as Draft</button>
              </div>
            </div>
          </div>
        </div>
<div class="modal" id="modal-error">
    <div class="modal-dialog" style=" width:30%;">
        <div class="modal-content">
          <div class="modal-body">
            <span>Are you sure you want to approved this transaction?</span>
          </div>
          <div class="modal-footer">
          <button type="button" id="err_close" class="btn btn-danger" data-dismiss="modal">Close</button>
          <button type="button"  class="btn btn-primary" onclick="approve()">OK</button>
          </div>
        </div>  
    </div>
</div>    



<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="pg" runat="server" />
<asp:HiddenField ID="lbl_bals" ClientIDMode="Static" runat="server" />
<asp:HiddenField ID="hdn_dtr_tardy" runat="server" />
<asp:HiddenField ID="hdn_dtr_absent" runat="server" />
<asp:HiddenField ID="TextBox1" ClientIDMode="Static" runat="server" />
<asp:HiddenField ID="txt_trn_id" ClientIDMode="Static" runat="server" />
<asp:HiddenField ID="hf_auto" runat="server" ClientIDMode="Static" />
<script src="script/auto/myJScript.js" type="text/javascript"></script>
<script src="vendors/tab/NewcbpFWTabs.js"></script>

<script>
    jQuery.noConflict();
    (function () {
        [ ].slice.call(document.querySelectorAll('.nav-tabs-custom')).forEach(function (el) { new CBPFWTabs(el); });
    })(jQuery);
    </script>
    <!-- SweetAlert2 -->
<script src="vendors/sweetalert2/sweetalert2.min.js"></script>
<!-- Toastr -->
<script src="vendors/toastr/toastr.min.js"></script>
<script type="text/javascript">
  $(function() {
    const Toast = Swal.mixin({
      toast: true,
      position: 'top-end',
      showConfirmButton: false,
      timer: 4000
    });
    $('.swalDefaultSuccess').click(function() {
        Toast.fire({
        type: 'success',
        title: '<span style=" font-size:34px;">Successfully Save</span>'
      })
      });
  });

</script>

<!--create dynamic tab-->
<script type="text/javascript">
    var button = '<button class="close" type="button" title="Remove this page">×</button>';

    $(document).ready(function () {
        $('.btn-add-tab').click(function () {
            var trnid = $('[id$=txt_trn_id]').val();
            $('#tab-list').append($('<li><a id="trn_det_' + trnid + '" href="#trn_det"  role="tab" data-toggle="tab"><span>Transacation Details</span><input type="text" id="trnid_' + trnid + '" value="' + trnid + '" style=" display:none;"/>  <button class="close" type="button" title="Remove this page"><i class="fa fa-close"></i></button></a></li>'));
            view_det(trnid);
        });
        $('#tab-list').on('click', '.close', function () {
            $(this).parents('li').remove();

        });
    });




    function backtodraft(el) {
        var current = $(el).closest("tr");
        var trnid = current.find("td:eq(0)").text();
        var param = { trnid: trnid };
        console.log(trnid);
        $.ajax({
            type: "Post",
            contentType: "application/json; charset=utf-8",
            url: "content/payroll/transotherincome.aspx/backtodraft",
            data: JSON.stringify(param),
            datatype: "JSON",
            success: function (rtn) {
                if (rtn.d == "Success") {
                    alert('Successfully Saved!');
                    window.location.href = "transotherincome";
                }
            },
            error: function (rtn) {
            }
        })
    };
    function click_cancelled(el) {
        var current = $(el).closest("tr");
        var trnid = current.find("td:eq(0)").text();
        var param = { trnid: trnid };
        console.log(trnid);
        $.ajax({
            type: "Post",
            contentType: "application/json; charset=utf-8",
            url: "content/payroll/transotherincome.aspx/cancelled_trn",
            data: JSON.stringify(param),
            datatype: "JSON",
            success: function(rtn) {
                if (rtn.d == "Success") {
                    alert('Successfully Removed!');
                    window.location.href = "transotherincome";
                }
            },
            error: function(rtn) {
            }
        })
    };
</script>
</asp:Content>
<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
</asp:Content>

