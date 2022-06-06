<%@ Page Language="C#" AutoEventWireup="true" CodeFile="proccesspayroll.aspx.cs" Inherits="content_payroll_proccesspayroll" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<style type="text/css">
     .loader {position: fixed; overflow:hidden; top: 42%;left: 50%;margin-left:-70px; z-index: 2001; padding: 20px;}
    .table-bordered td a {
        font-size: 12px;
        padding: 0 5px 0 5px;
        text-align:center;
    }
</style>
<script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to cancel this transaction?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    }
     
</script>
<script type="text/javascript">
    function Save() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure you want to save this transaction?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    }
     
</script>

<link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />

<link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>

</asp:Content>

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Payment and Deduction</h3>
    </div>   
</div>
<div class="clearfix"></div>
<div class="row">
    <div id="p_status" runat="server" visible="false" class="Overlay"></div>
    <div id="p_status2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton16" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Field Selection</b>
            <br /><hr />
            <div class="form-group" style=" margin-right:10px;">
                <asp:RadioButtonList ID="rb_range" runat="server">
                    <asp:ListItem Value="Draft" Selected>Draft</asp:ListItem>
                    <asp:ListItem Value="Posted">Post</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <li><hr /></li>
            <asp:Button ID="btnsave" OnClientClick="Save()" OnClick="click_post" runat="server" Text="Save" CssClass="btn btn-primary"/>
    </div>
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
             <ul id="tab-list" class="nav nav-tabs" role="tablist">
                <li class="active"><a href="#payroll" data-toggle="tab" aria-expanded="true">Payroll</a></li>
                <li class=""><a href="#tmp" data-toggle="tab" aria-expanded="false">13th Month Pay</a></li>
                <li class=""><a href="#fmp" data-toggle="tab" aria-expanded="false">14th Month Pay</a></li>
                <li class=""><a href="#Bonus" data-toggle="tab" aria-expanded="false">Bonus</a></li>
             </ul>
             <div id="tab-content" class="tab-content">
                <div class="tab-pane active" id="payroll">
                    <div class="x-head hidden">
                        <asp:DropDownList ID="ddl_pg" runat="server" CssClass="minimal"></asp:DropDownList>
                        <asp:TextBox ID="txt_f" cssclass="datee" runat="server" placeholder="From"></asp:TextBox>
                        <asp:TextBox ID="txt_t" cssclass="datee" runat="server" placeholder="To"></asp:TextBox>
                        <asp:Button ID="Button1"  runat="server"  OnClick="click_search"  Text="Search" CssClass="btn btn-primary"/>
                    </div>
                    <asp:LinkButton ID="Button2" runat="server" OnClick="click_add_dtr" CssClass="right add"><i class="fa fa-plus-circle"></i></asp:LinkButton>
                    <asp:Label ID="lbl_err" runat="server" ForeColor="Red" Font-Size="13px"></asp:Label>
                    <asp:GridView ID="grid_paylist" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                      <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="PayrollDate" HeaderText="Payroll Date" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="remss" HeaderText="Payroll Date"/>
                        <asp:BoundField DataField="pg" HeaderText="Payroll Group"/>
                        <asp:BoundField DataField="datedtrrange" HeaderText="Payroll Range"/>
                          <asp:TemplateField HeaderText="Status" >
                            <ItemTemplate>
                                <%--<asp:LinkButton ID="LinkButton1" runat="server" CssClass="remove_href"  OnClick="click_post" style=" font-size:10px; border:1px; border-radius:5px; padding:3px;"  OnClientClick='<%# String.Format("javascript:return Confirmposting(\"{0}\")", Eval("status_1").ToString()) %>' Text='<%#Eval("status_1")%>' ></asp:LinkButton>--%>
                                <asp:LinkButton ID="lnk_posted" runat="server" CssClass="remove_href"  OnClick="selection" style=" font-size:10px; border:1px; border-radius:5px; padding:3px;" Text='<%#Eval("status_1")%>' ></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="180px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" >
                            <ItemTemplate>
                                <a href="payrolldetails?&payid=<%# function.Encrypt(Eval("id").ToString(), true)%>" title="Details" target="_new" style=" font-size:14px" ><i class="fa fa-list"></i></a>
                                <a href="printablepayslip?&payid=<%# function.Encrypt(Eval("id").ToString(), true)%>&b=batch" target="_new" title="Print Slip" style="font-size:15px"><i class="fa fa-print"></i></a>
                                <a href="payworksheet?&payid=<%# function.Encrypt(Eval("id").ToString(), true)%>" title="Payroll Register Detailed" target="_new" style=" font-size:14px" ><i class="fa fa-book"></i></a>
                                <a href="PayregRptViewer?&payid=<%# function.Encrypt(Eval("id").ToString(), true)%>" title="Payroll Register Summary" target="_new" style=" font-size:14px" ><i class="fa fa-line-chart"></i></a>
                                <a href="Payrollnetsummary?&payid=<%# function.Encrypt(Eval("id").ToString(), true)%>" title="Payroll Net Summary" target="_new" style=" font-size:14px" ><i class="fa fa-bank"></i></a>
                                <a href="par?&payid=<%# function.Encrypt(Eval("id").ToString(), true)%>" title="Payslip Acknowledgement Receipt" target="_new" style=" font-size:14px" ><i class="fa fa-file-text-o"></i></a>
                                <%--<a href="requestbudget?&payid=<%# function.Encrypt(Eval("id").ToString(), true)%>" title="Request Budget" target="_new" style=" font-size:14px" ><i class="fa fa-line-chart"></i></a>--%>
                                <%--<asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Bank Text File" OnClick="generatetexfile" Font-Size="14px"><i class="fa fa-file-text-o"></i></asp:LinkButton>--%>
                                <asp:LinkButton ID="LinkButton2" runat="server" ToolTip="Cancel Payroll"  Text="can" OnClick="click_cancel" OnClientClick='<%# String.Format("javascript:return Confirm(\"{0}\")", Eval("status_1").ToString()) %>' Font-Size="16px" ><i class="fa fa-trash" style="padding-right:0 "></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="250px" />
                        </asp:TemplateField>
                      </Columns>
                    </asp:GridView>
                </div>
                <div class="tab-pane" id="tmp">
                <a data-toggle="modal" data-target="#modal-tmp" onclick="click_add_new('tmp')" Class="right add"><i class="fa fa-plus-circle"></i></a>
                <table id="tmp" Class="table table-striped table-bordered">
                    <thead>
                        <tr>
                            <th style=" display:none;"></th>
                            <th>Trn. Date</th>
                            <th>Group</th>
                            <th>Range</th>
                            <th>Status</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                       <tr>
                            <td colspan="6">No Data Found!</td>
                       </tr>
                    </tbody>
                    <tfoot></tfoot>
                </table>
                </div>
                <div class="tab-pane" id="fmp">
                  <a data-toggle="modal" data-target="#modal-tmp" onclick="click_add_new('fmp')" Class="right add"><i class="fa fa-plus-circle"></i></a>
                  <table id="fmp" Class="table table-striped table-bordered">
                    <thead>
                        <tr>
                            <th style=" display:none;"></th>
                            <th>Trn. Date</th>
                            <th>Group</th>
                            <th>Range</th>
                            <th>Status</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                       <tr>
                            <td colspan="6">No Data Found!</td>
                       </tr>
                    </tbody>
                    <tfoot></tfoot>
                </table>
                </div>
                <div class="tab-pane" id="Bonus">
                <a data-toggle="modal" data-target="#modal-tmp" onclick="click_add_new('bonus')" Class="right add"><i class="fa fa-plus-circle"></i></a>
                 <table id="bonus" Class="table table-striped table-bordered">
                    <thead>
                        <tr>
                            <th style=" display:none;"></th>
                            <th>Trn. Date</th>
                            <th>Group</th>
                            <th>Range</th>
                            <th>Status</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                       <tr>
                            <td colspan="6">No Data Found!</td>
                       </tr>
                    </tbody>
                    <tfoot></tfoot>
                </table>
              </div>
            </div>
        </div>
    </div>
</div>
<!--Modal-->
<div class="modal" id="modal-tmp">
          <div class="modal-dialog" style="width:90%;">
            <div class="modal-content">
              <div class="modal-header">
              
                <button id="close_thirteen" type="button" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">×</span></button>
                  <span id="label_" style="display:none;"></span>
                  <h4 class="modal-title"><span id="label"></span></h4>
                  <button type="button" style="display:none;" id="btn_success" class="btn btn-success swalDefaultSuccess" data-dismiss="modal">
                  Launch Success Toast
                </button>
              </div>
              <div class="modal-body">
              <div class="x-head">
                <asp:DropDownList ID="ddl_pg_tmp" runat="server" CssClass="minimal"></asp:DropDownList>
                <asp:DropDownList ID="ddl_yyyy_tmp" runat="server" CssClass="minimal"></asp:DropDownList>
                
                <asp:DropDownList ID="ddl_bonus_percentage" runat="server" CssClass="minimal" style="display:none;" >
                <asp:ListItem Value="0">None</asp:ListItem>
                <asp:ListItem Value="25">25%</asp:ListItem>
                <asp:ListItem Value="50">50%</asp:ListItem>
                <asp:ListItem Value="75">75%</asp:ListItem>
                <asp:ListItem Value="100">100%</asp:ListItem>
                </asp:DropDownList>
                
                <button type="button" onclick="click_compute()" Class="btn btn-primary">Compute</button>
                <button id="save_data" onclick="save_data_thirteen()" type="button" style=" display:none;" Class="btn btn-default">Process</button>
              </div>
              <div class="row">
                 <table id="example1" class="table table-striped table-bordered nowrap">
                    <thead>
                       <tr>
                            <td style=" display:none;"></td>
                            <td>Id Number</td>
                            <td>Employee Name</td>
                            <td>Date Hired</td>
                            <td>Monthly Rate</td>
                            <td><span id="lbl_header"></span></td>
                       </tr>
                    </thead>
                    <tbody>
                    <tr>
                        <td colspan="5">
                         No Data Found!
                        </td>
                    </tr>
                    </tbody>
                    
                </table>
              </div> 

              </div>
             
            </div>
          </div>
        </div>
<div class="modal" id="modal-tmp-details">
          <div class="modal-dialog" style="width:90%;">
            <div class="modal-content">
              <div class="modal-header">
              
                <button id="Button3" type="button" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">×</span></button>
                  <h4 class="modal-title">Details</h4>
              </div>
              <div class="modal-body">
              <section class="invoice">
              <!-- info row -->
              <div class="row invoice-info">
                <div class="col-sm-4 invoice-col">
                  <address>
                  <span>Payroll Group:</span><strong><span id="pg"></span></strong><br>
                    <span>Year:</span><span id="yyyy"></span><br>
                    <span>Date Input:</span><span  id="di"></span><br>
                  
                  </address>
                </div>
              </div>
              <!-- /.row -->

      <!-- Table row -->
      <div class="row">
        <div class="col-xs-12 table-responsive">
             <table id="tmp_det" class="table table-striped table-bordered nowrap">
                    <thead>
                       <tr>
                            <td style=" display:none;"></td>
                            <td style=" display:none;"></td>
                            <td>Id Number</td>
                            <td>Employee Name</td>
                            <td>Date Hired</td>
                            <td>Monthly Rate</td>
                            <td>13th Month Pay</td>
                               <td>Taxable</td>
                            <td>Non-Taxable</td>
                            <td>Action</td>
                       </tr>
                    </thead>
                    <tbody>
                    <tr>
                        <td colspan="5">
                         No Data Found!
                        </td>
                    </tr>
                    </tbody>
                    
                </table>
        </div>
        <!-- /.col -->
      </div>
      <!-- /.row -->

  

    </section>
              <div class="row">
                 
              
              </div> 
             </div>
            </div>
          </div>
        </div>
<div class="modal" id="modal-fmp-details">
          <div class="modal-dialog" style="width:90%;">
            <div class="modal-content">
              <div class="modal-header">
              
                <button id="Button4" type="button" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">×</span></button>
                  <h4 class="modal-title">Details</h4>
              </div>
              <div class="modal-body">
              <section class="invoice">
              <!-- info row -->
              <div class="row invoice-info">
                <div class="col-sm-4 invoice-col">
                  <address>
                  <span>Payroll Group:</span><strong><span id="fmp_pg"></span></strong><br>
                    <span>Year:</span><span id="fmp_yyyy"></span><br>
                    <span>Date Input:</span><span  id="fmp_DI"></span><br>
                  
                  </address>
                </div>
              </div>
              <!-- /.row -->

      <!-- Table row -->
      <div class="row">
        <div class="col-xs-12 table-responsive">
             <table id="fmp_details" class="table table-striped table-bordered nowrap">
                    <thead>
                       <tr>
                            <td style=" display:none;"></td>
                            <td style=" display:none;"></td>
                            <td>Id Number</td>
                            <td>Employee Name</td>
                            <td>Date Hired</td>
                            <td>Monthly Rate</td>
                            <td>13th Month Pay</td>
                               <td>Taxable</td>
                            <td>Non-Taxable</td>
                            <td>Status</td>
                       </tr>
                    </thead>
                    <tbody>
                    <tr>
                        <td colspan="5">
                         No Data Found!
                        </td>
                    </tr>
                    </tbody>
                    
                </table>
        </div>
        <!-- /.col -->
      </div>
      <!-- /.row -->

  

    </section>
              <div class="row">
                 
              
              </div> 
             </div>
            </div>
          </div>
        </div>
<div class="modal" id="modal-bonus-details">
          <div class="modal-dialog" style="width:90%;">
            <div class="modal-content">
              <div class="modal-header">
              
                <button id="Button5" type="button" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">×</span></button>
                  <h4 class="modal-title">Details</h4>
              </div>
              <div class="modal-body">
              <section class="invoice">
              <!-- info row -->
              <div class="row invoice-info">
                <div class="col-sm-4 invoice-col">
                  <address>
                  <span>Payroll Group:</span><strong><span id="bonus_pg"></span></strong><br>
                    <span>Year:</span><span id="bonus_yyyy"></span><br>
                    <span>Date Input:</span><span  id="bonus_DI"></span><br>
                  
                  </address>
                </div>
              </div>
              <!-- /.row -->

      <!-- Table row -->
      <div class="row">
        <div class="col-xs-12 table-responsive">
             <table id="bonus_details" class="table table-striped table-bordered nowrap">
                    <thead>
                       <tr>
                            <td style=" display:none;"></td>
                            <td style=" display:none;"></td>
                            <td>Id Number</td>
                            <td>Employee Name</td>
                            <td>Date Hired</td>
                            <td>Monthly Rate</td>
                            <td>13th Month Pay</td>
                            <td>Taxable</td>
                            <td>Non-Taxable</td>
                            <td>Status</td>
                       </tr>
                    </thead>
                    <tbody>
                    <tr>
                        <td colspan="5">
                         No Data Found!
                        </td>
                    </tr>
                    </tbody>
                    
                </table>
        </div>
        <!-- /.col -->
      </div>
      <!-- /.row -->

  

    </section>
              <div class="row">
                 
              
              </div> 
             </div>
            </div>
          </div>
        </div>

<!--ERROR-->
<div class="modal" id="modal-error">
            <div class="modal-dialog" style=" width:30%;">
                <div class="modal-content">
                  <div class="modal-body">
                    <span>Are you sure you want to <span style=" color:Red; font-weight:bold;">Cancelled</span> this transaction?</span>
                  </div>
                  <div class="modal-footer">
                  <button type="button" id="err_close" class="btn btn-danger" data-dismiss="modal">Close</button>
                  <button type="button"  class="btn btn-primary" onclick="cotinue_cancelled()">OK</button>
                  </div>
                </div>  
            </div>
        </div>    
<div class="modal" id="modal-fmp-error">
            <div class="modal-dialog" style=" width:30%;">
                <div class="modal-content">
                  <div class="modal-body">
                    <span>Are you sure you want to <span style=" color:Red; font-weight:bold;">Cancelled</span> this transaction?</span>
                  </div>
                  <div class="modal-footer">
                  <button type="button" id="err_fmp_close" class="btn btn-danger" data-dismiss="modal">Close</button>
                  <button type="button"  class="btn btn-primary" onclick="cotinue_fmp_cancelled()">OK</button>
                  </div>
                </div>  
            </div>
        </div>   
<div class="modal" id="modal-bonus-error">
            <div class="modal-dialog" style=" width:30%;">
                <div class="modal-content">
                  <div class="modal-body">
                    <span>Are you sure you want to <span style=" color:Red; font-weight:bold;">Cancelled</span> this transaction?</span>
                  </div>
                  <div class="modal-footer">
                  <button type="button" id="err_bonus_close" class="btn btn-danger" data-dismiss="modal">Close</button>
                  <button type="button"  class="btn btn-primary" onclick="cotinue_bonus_cancelled()">OK</button>
                  </div>
                </div>  
            </div>
        </div>   
<div class="hide"> 
  <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox> 
</div>
<asp:HiddenField ID="hdn_class" runat="server" />
<div id="load" class="Overlay hide"></div>
<div id="loader"  class="loader hide">
    <img src="style/images/loading.gif" alt="loading" />
</div>
<asp:HiddenField ID="seriesid" runat="server" />
<asp:HiddenField ID="hdn_trnid" runat="server" />
<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
<script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
<script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
<script src="script/auto/myJScript.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $("#modal-tmp").on("hidden.bs.modal", function () {
            $('[id$=ddl_bonus_percentage]').attr('style', 'display:none;');
        });
        get_tmp();
        get_fmp();
        get_bonus();
    });
    function click_add_new(clasification) {
        var tbl = $('[id$=example1]').find("tbody");
        tbl.empty();
        tbl.append("<tr><td colspan='5'>No Data Found!</td></tr>");
        if (clasification == "tmp") {
            $('[id$=label_]').text('Thirteen');
            $('[id$=label]').text('Process Thirteen Month Pay');
            $('[id$=lbl_header]').text('Thirteen Month Pay');
            
        }

        if (clasification == "fmp") {
            $('[id$=label_]').text('Fourteen');
            $('[id$=label]').text('Process Fourteen Month Pay');
            $('[id$=lbl_header]').text('Fourteen Month Pay');
        }

        if (clasification == "bonus") {
            console.log('sod nako bai');
            $('[id$=ddl_bonus_percentage]').attr('style', 'visibility:inherit;');
            $('[id$=label_]').text('Bonus');
            $('[id$=label]').text('Process Bonus');
            $('[id$=lbl_header]').text('Bonus');

        }
        
    };
    function click_posting_thirteen(el) {
        var current = $(el).closest("tr");
        var trnid = current.find("td:eq(0)").text();
        var param = { trnid: trnid };
        console.log($(el).text());
        if ($(el).text() != "Posted")
        {
            if (confirm("Are you sure to Post this transaction?")) {
                $.ajax({
                    type: "Post",
                    contentType: "application/json; charset=utf-8",
                    url: "content/payroll/proccesspayroll.aspx/Post_tmp",
                    data: JSON.stringify(param),
                    datatype: "json",
                    success: function (rtn) {
                        if (rtn.d == "Success") {
                            get_tmp();
                            get_fmp();
                            get_bonus();
                            alert("Successfully Posted!");
                        }
                    },
                    Error: function (rtn) {
                    }
                })
            }
        }
        else
        {
            console.log('no');
        }
    };
    
</script>
</asp:Content>