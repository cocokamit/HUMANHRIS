<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addPayrollOtherIncome.aspx.cs" Inherits="content_hr_addPayrollOtherIncome" MasterPageFile="~/content/MasterPageNew.master" %>
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
   .btn {
    display: inline-block;
    padding: 2px 6px;
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

</style>
<!--FREEZE GRID-->
 <link href="style/fixheadergrid/css/web.css" rel="stylesheet" />
<script src="script/auto/myJScript.js" type="text/javascript"></script>
<link href="script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
<script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>
<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
<script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
<script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
<link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
<script type="text/javascript" src="style/js/googleapis_jquery.min.js"></script>
<script src="style/js/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script src="vendors/tab/modernizr.custom.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $.noConflict();
        $(".auto").autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "content/hr/addPayrollOtherIncome.aspx/GetEmployee",
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
<link rel="stylesheet" href="dist/css/base.css">

</asp:Content>
<asp:Content ID="content_addpayroll" runat="server" ContentPlaceHolderID="content">

<asp:HiddenField ID="hf_action" runat="server" />
<div class="page-title">
    <div class="title_left">
        <h3><asp:Label ID="l_pagetitle" runat="server" ></asp:Label></h3>
    </div>   
    <div class="title_right">

       <ul>
        <li><a href="transotherincome"><i class="fa fa-users"></i>Other Income</a></li>
        <li><i class="fa fa-angle-right"></i></li>
        <li><asp:Label ID="l_page" runat="server" ></asp:Label></li>
       </ul>
    </div>
</div>
<div class="clearfix"></div> 



           
<div class="col-md-9">

      
        
                
        <div class="col-md-12" >
            <div class="box box-primary">
            <div class="box-header with-border">
              <h3 class="box-title">Other Income</h3>

              <div class="box-tools pull-right">
                <div class="has-feedback">
                 <asp:DropDownList ID="ddl_payroll_group" AutoPostBack="true" OnTextChanged="click_pgpg"  runat="server"></asp:DropDownList>
                            <asp:DropDownList ID="ddl_pg"  runat="server"></asp:DropDownList>
                            <asp:Button ID="btn_verify" runat="server" Enabled="false" OnClick="click_pg" Text="Verify" CssClass="btn btn-primary" />
                </div>
              </div>
              <!-- /.box-tools -->
            </div>
            <!-- /.box-header -->
              
                    <div class="box-body no-padding">
                      <div class="mailbox-controls">
                        <div class="box-tools pull-left">
                            
                        </div>
                       
                      </div>    
                    </div>
            <div class="table-responsive mailbox-messages">
            <asp:GridView ID="grid_view" runat="server" ClientIDMode="Static"  AutoGenerateColumns="false" HeaderStyle-CssClass="GridViewScrollHeader" RowStyle-CssClass="GridViewScrollItem"  style=" width:100%; scrollbar-width: none;">
                                        <Columns>
                                            <asp:BoundField DataField="empname" HeaderText="Employee Name"/>
                                            <asp:BoundField DataField="otherincome" HeaderText="Other Income"/>
                                            <asp:BoundField DataField="amount" HeaderText="Allowed Income"/>
                                            <asp:BoundField DataField="total_hours_worked" HeaderText="Worked Hrs"/>
                                            <asp:TemplateField HeaderText="Non-Taxable">
                                                <ItemTemplate>
                                                   <asp:TextBox ID="txt_amt" runat="server" AutoComplete="off" ClientIDMode="Static" onkeyup="decimalinput(this);" Text='<%#bind ("amt_to_bepaid")%>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:TemplateField HeaderText="Taxable">
                                                <ItemTemplate>
                                                   <asp:TextBox ID="txt_amttotax" runat="server" AutoComplete="off"  ClientIDMode="Static" onkeyup="decimalinput(this);" Text='<%#bind ("amt_to_be_tax")%>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                                 <asp:BoundField DataField="schedule" Visible="true" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                            <asp:BoundField DataField="Taxable" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                            <asp:BoundField DataField="empid" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                            <asp:BoundField DataField="incomeid" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                            <asp:BoundField DataField="type" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                            <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                            <asp:ImageButton ID="can" runat="server" CausesValidation="false" OnClick="delete" ImageUrl="~/style/img/delete.png" />
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                      </asp:GridView>
                                      </div>
            </div>
        </div>
        <div class="col-md-12 none" >
            <div class="x_panel"> 
            <div class="x_title">
                    <h2>Payroll Adjustment</h2>
                        <ul class="nav navbar-right panel_toolbox">
                          <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                          </li>
                        </ul>
                    <div class="clearfix"></div>
            </div>
            <div class="x_content">
            <asp:GridView ID="grid_pa" runat="server" EmptyDataText="No Data Found" EmptyDataRowStyle-ForeColor="Red" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                                    <Columns>
                                        <asp:BoundField DataField="emp_id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                        <asp:BoundField DataField="pa_id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                        <asp:BoundField DataField="date" HeaderText="Date"/>
                                        <asp:BoundField DataField="emp_name" HeaderText="Employee Name"/>
                                        <asp:BoundField DataField="class" HeaderText="Class"/>
                                        <asp:TemplateField HeaderText="Total Amt.">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_amt" runat="server" AutoComplete="off" ClientIDMode="Static" onkeyup="decimalinput(this);" Text='<%#bind ("total_amt")%>'></asp:TextBox>
                                        </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                </div>
           </div>
        </div>  

        <div class="col-md-12 none">
         <div class="x_panel">
                  <div class="x_title">
                        <h2>Service Charge</h2>
                        <ul class="nav navbar-right panel_toolbox">
                          <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                          </li>
                        </ul>
                  <div class="clearfix"></div>
                  </div>
                  <div class="x_content">
                    <div class="row">
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddl_sc_range" runat="server" class="form-control"> </asp:DropDownList>                                
                                    </div>
                               </div>
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <asp:Button ID="Button1" runat="server" OnClick="click_sc" CssClass="btn btn-primary" Text="Process"  />                              
                                    </div>
                               </div>
                    </div>
                  <ul class="input-form">
                   
                    <li></li>
                    <li><asp:GridView ID="grid_sc" runat="server" EmptyDataText="No Data Found" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                    <Columns>
                    <asp:BoundField DataField="empid" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
               
                    <asp:BoundField DataField="e_name" HeaderText="Employee Name"/>
                    <asp:BoundField DataField="tnod" HeaderText="No. of days"/>
                    <asp:BoundField DataField="amount" HeaderText="Amount"/>
                    </Columns>
                    </asp:GridView></li>
                  </ul>
                  </div>

                </div>
        </div>
        <div class="col-md-12">
        </div>
</div>
<div class="col-md-3">
  <div class="col-md-12" >
        <div class="x_panel">
                <div class="form-group">
                    <label>Search Employee</label>
                    <asp:TextBox ID="txt_searchemp"  runat="server"  ClientIDMode="Static"  Placeholder="Search Employee" class="form-control has-feedback-left auto"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Amount</label>
                    <asp:TextBox ID="txt_addinc_amt"  runat="server" AutoComplete="off" class="form-control has-feedback-left" placeholder="Amount"  ClientIDMode="Static" onkeyup="decimalinput(this);"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Type</label>
                    <asp:DropDownList ID="ddl_type" runat="server" class="form-control"> </asp:DropDownList>                                
                </div>
                <div class="form-group">
                    <label>Remarks</label>
                    <asp:TextBox ID="txt_addinc_remarks" TextMode="MultiLine" placeholder="Remarks" class="form-control" style=" resize:none; height:34px" runat="server"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Batch Upload</label>
                    <asp:FileUpload ID="file_data" runat="server" />
                </div>
                <div class="form-group">
                    <label>  <asp:Label ID="lbl_err_msg" runat="server" ForeColor="Red"></asp:Label></label>
                    <asp:Button ID="Button2" runat="server" OnClick="insertdatatableaddincome"   Text="Add" CssClass="btn btn-primary" />
                </div>     
        </div>
  </div>
  <div class="col-md-12" >
        <div class="x_panel">
                <div class="form-group">
                    <label>Remarks<asp:Label ID="lbl_remarks" runat="server"  ForeColor="Red"></asp:Label></label>
                    <asp:TextBox ID="txt_remarks" TextMode="MultiLine" class="form-control " style=" resize:none;" runat="server"></asp:TextBox><%--has-feedback-left auto--%>
                </div>
                <div class="form-group">
                    <asp:Button ID="btn_save" runat="server" Text="Submit" OnClick="btn_save_Click" CssClass="btn btn-primary" /> 
                </div>
        </div>
  </div>
</div>
<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="pg" runat="server" />
<asp:HiddenField ID="lbl_bals" ClientIDMode="Static" runat="server" />
<asp:HiddenField ID="hdn_dtr_tardy" runat="server" />
<asp:HiddenField ID="hdn_dtr_absent" runat="server" />

<script src="vendors/tab/NewcbpFWTabs.js"></script>
<script>
    jQuery.noConflict();
    (function () {
        [ ].slice.call(document.querySelectorAll('.nav-tabs-custom')).forEach(function (el) { new CBPFWTabs(el); });
    })(jQuery);
</script>
</asp:Content>
<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
    <script type="text/javascript" src="style/fixheadergrid/js/gridviewscroll.js"></script>
    <script type="text/javascript">
       
        var gridViewScrolll = null; 
            window.onload = function () {
            gridViewScrolll = new GridViewScroll({
                elementID: "grid_view",
                width: window.Width,
                height: 500,
                freezeColumn: true,
                // freezeFooter: true,
                freezeColumnCssClass: "GridViewScrollItemFreeze",
                freezeFooterCssClass: "GridViewScrollFooterFreeze",
                freezeHeaderRowCount: 1,
                freezeColumnCount: 2,
                onscroll: function (scrollTop, scrollLeft) {
                    console.log(scrollTop + " - " + scrollLeft);
                }
            });
            gridViewScrolll.enhance();

          
            //redirect exact row
            var id = document.getElementById("hdn_selected_id").value;
            var fid = '#' + id;
            $('.test').attr('href', fid);
            window.location.href = $('.test').attr('href');
        }
    </script>
   
</asp:Content>



