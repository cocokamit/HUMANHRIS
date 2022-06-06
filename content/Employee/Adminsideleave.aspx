<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Adminsideleave.aspx.cs" Inherits="content_Employee_Adminsideleave" MasterPageFile="~/content/MasterPageNew.master"%>
<%@ Import Namespace="System.Data" %>
<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_leave">
    <script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
    <script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">

        (function ($) {
            $(function () {
                var unavailableDates = $("[id$=hfdelegates]").val().split(',');
                console.log($("[id$=hfdelegates]").val());
                function unavailable(date) {
                    dmy = date.getDate() + "-" + (date.getMonth() + 1) + "-" + date.getFullYear();
                    if ($.inArray(dmy, unavailableDates) == -1) {
                        return [true, ""];
                    } else {
                        return [false, "", "Unavailable"];
                    }
                }
                $(".datee").datepicker({ changeMonth: true,
                    yearRange: "-1:+1",
                    changeYear: true,
                    beforeShowDay: unavailable,
                    onSelect: function () {
                        console.log($(this).val());
                        if ($("[id$=txt_from]").val() != "" && $("[id$=txt_to]").val() != "") {
                            var tfrom = $("[id$=txt_from]").val();
                            var tto = $("[id$=txt_to]").val();
                            var empid = $("[id$=user_id]").val();

                            $.ajax({
                                type: "POST",
                                contentType: "application/json;",
                                url: "content/Employee/InbehalfLeave.aspx/GetInfo",
                                data: JSON.stringify({ from: tfrom, to: tto, id: empid }),
                                dataType: "json",
                                success: function (data) {
                                    var array = data.d.toString().split(',');
                                    console.log(array);
                                    var ddl = $("[id$=ddlOIC]");
                                    var x = document.getElementById("ddlOIC");
                                    var y = document.getElementById("DropDownList1");
                                    console.log(x.length);
                                    console.log(y.length);
                                    x.options.length = 1;
                                    var ylength = y.length;

                                    console.log(ylength);

                                    for (var l = 1; l < y.length; l++) {
                                        console.log(l);
                                        x.options[l] = new Option(y.options[l].label, y.options[l].value);
                                    }

                                    for (var i = 0; i < array.length - 1; i++) {
                                        console.log(array.length);
                                        for (var j = 1; j < ylength; j++) {
                                            if (x.options[j].value == array[i]) {
                                                x.options[j].setAttribute('disabled', 'disabled');
                                                console.log(x.options[j]);
                                            }
                                        }
                                    }
                                },
                                error: function (result) {
                                    alert(result.responseText);
                                }
                            });
                        }
                        if ($("[id$=txt_to]").val() != "") {

                            $("[id$=lb_errorto]").text("");
                        }

                        if ($("[id$=txt_from]").val() != "") {

                            $("[id$=lb_errorfrom]").text("");
                        }
                    }
                });
            });
        })(jQuery);

    </script>
    <!--SELECT-->
<link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ContentPlaceHolderID="content" ID="content_leave" runat="server">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Leave Application</h3>
    </div>  
    <div class="title_right">
        <ul>
            <li><a href="masterfile"><i class="fa fa-users"></i>Master File</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li class="active">Leave Application</li>
        </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-4">
        <div class="x_panel">
            <div class="box-header with-border">
                <h4 class="box-title">Leave Credits <asp:Label ID="lblempname" runat="server" Text=" EMpsNmae"></asp:Label></h4>
            </div>
            <div class="box-body">
                    <asp:GridView ID="grid_view" runat="server" EmptyDataText="No Data Found!" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin"><%--OnRowDataBound="datatbound"--%>
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="Leave" HeaderText="Leave"/>
                        <asp:BoundField DataField="Credit" HeaderText="Credits"/>
                        <asp:BoundField DataField="Balance" HeaderText="Balance"/>
                        <asp:BoundField DataField="yyyyear" HeaderText="Year"/>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div class="x_panel">
            <div class="box-header with-border">
                <h4 class="box-title">Application</h4>
                <asp:Button ID="btndelagates" runat="server" OnClick="modalclick" CssClass="fa fa-info-circle mb-4 pull-right bg-red" Text="i" style="border-radius: 50%;border: 0;" />
            </div>
            <div class="box-body">
                <div class="form-group" style="opacity:70%;">
                    <asp:RadioButtonList ID="rb_range" runat="server" OnSelectedIndexChanged="select_nod" AutoPostBack="true">
                        <asp:ListItem Value="1">Whole Day</asp:ListItem>
                        <asp:ListItem Value="0.5">Half Day</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="form-group" style="opacity:70%;">
                    <div id="startin"  style=" display:none;" runat="server">
                        <label>Expected Leave Designation<asp:Label ID="lbl_eiotd" CssClass="text-danger" runat="server"></asp:Label></label>
                        <asp:RadioButtonList ID="radiobtnlist" runat="server">
                             <asp:ListItem Value="1">1st Half (8:30 am to 1:15 pm (4.45 hours))</asp:ListItem>
                             <asp:ListItem Value="2">2nd Half  (1:15 pm to 6:00 pm (4.45 hours))</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="form-group" style="opacity:70%;">
                    <label>Leave Type</label>
                    <asp:DropDownList ID="ddl_leave" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="row" style="opacity:70%;">
                    <div class="col-lg-6">
                        <div class="form-group">
                            <label>From<asp:Label ID="lbl_from" CssClass="text-danger" runat="server" Text=""></asp:Label></label>
                            <asp:TextBox ID="txt_from" runat="server" AutoComplete="off" CssClass="datee form-control" ClientIDMode="Static"></asp:TextBox>
                            <asp:Label ID="lb_errorfrom" runat="server" ForeColor="Red" ClientIDMode="Static" ></asp:Label>
                        </div>
                    </div>
                    <div class="col-lg-6">
                        <div class="form-group">
                            <label>To <asp:Label ID="lbl_to" CssClass="text-danger" runat="server" Text=""></asp:Label></label>
                            <asp:TextBox ID="txt_to" runat="server" AutoComplete="off" CssClass="datee form-control" ></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-group none">
                    <label>Delegate To <asp:Label ID="lbl_delegate" CssClass="text-danger" runat="server" Text=""></asp:Label></label>
                    <asp:TextBox ID="txt_delegate" runat="server" AutoComplete="off" CssClass="form-control"  Visible="false"></asp:TextBox>
                    <asp:DropDownList ID="ddlOIC" runat="server" CssClass="select2" Visible="false" ClientIDMode="Static" ></asp:DropDownList>
                    <asp:DropDownList ID="DropDownList1" runat="server" style="display:none;" ClientIDMode="Static" ></asp:DropDownList>
                </div>
                <div class="form-group no-margin" style="opacity:70%;">
                    <label>Reason <asp:Label ID="lbl_remarks" CssClass="text-danger" runat="server" Text=""></asp:Label></label>
                    <asp:TextBox ID="txt_lineremarks" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            
            <asp:Button ID="Button2" runat="server" OnClick="click_veirfy" Text="Verify" CssClass="btn btn-primary pull-left"/>&nbsp
            <div>
                <asp:Label ID="lbl_error_msg" CssClass="text-danger" ForeColor="Red" runat="server" Text=""></asp:Label>
                <asp:Label ID="lbl_err" runat="server" ForeColor="Red" ></asp:Label>
            </div>
        </div>
    </div>

    <div class="col-md-8">
        <div class="x_panel">
            <div id="pnlstransaction" runat="server" class="box-header" style="padding-bottom:0">
            <div class="box-body">
                <asp:GridView ID="gvLeave" runat="server" AutoGenerateColumns="false"  OnRowDataBound="rowboundLeave" EmptyDataText="No Data Found!" CssClass="table table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="id"  ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                    <asp:BoundField DataField="date" HeaderText="Filed" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="Leave" HeaderText="Leave Type"/>
                    <asp:BoundField DataField="remarks" HeaderText="Remarks"/>
                    <asp:BoundField DataField="status" HeaderText="Status" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" CssClass="label"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_view" OnClick="view1" CssClass="glyphicon glyphicon-info-sign" ToolTip="Details" runat="server"></asp:LinkButton>
                            <asp:LinkButton ID="lnk_reprint" OnClick="reprint" style=" display:none;" ToolTip="Print" CssClass="glyphicon glyphicon-print" runat="server"></asp:LinkButton>
                            <asp:LinkButton ID="lnk_can" OnClick="cancelleave" CommandName='<%# Eval("id") %>' ToolTip="Cancel" OnClientClick="Confirm()" CssClass="glyphicon glyphicon-remove-sign no-padding-right" runat="server"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle  Width="75px"/>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false"  OnRowDataBound="rowbound" EmptyDataText="No record found" CssClass="table table-bordered no-margin hidden">
                <Columns>
                    <asp:BoundField DataField="trnid"  ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                    <asp:BoundField DataField="trn_date" HeaderText="Filed" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="Leave" HeaderText="Type"/>
                    <asp:BoundField DataField="delegate" HeaderText="Delegate"/>
                    <asp:BoundField DataField="remarks" HeaderText="Remarks"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_view" OnClick="view1" CssClass="glyphicon glyphicon-info-sign" ToolTip="Details" runat="server"></asp:LinkButton>
                            <asp:LinkButton ID="lnk_reprint" OnClick="reprint" style=" display:none;" ToolTip="Print" CssClass="glyphicon glyphicon-print" runat="server"></asp:LinkButton>
                            <asp:LinkButton ID="lnk_can" OnClick="cancelall" ToolTip="Cancel" OnClientClick="Confirm()" CssClass="glyphicon glyphicon-remove-sign no-padding-right" runat="server"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle  Width="75px"/>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
            </div>

            <div id="pnlsverifying" runat="server" class="box-header" style="padding-bottom:0">
              <div class="box-header with-border">
                <h4 class="box-title">Leave Transaction</h4>
              </div>
              <div class="box-body">
                <asp:GridView ID="grid_leave" runat="server" ClientIDMode="Static"  EmptyDataText="No record found" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                    <Columns>
                        <asp:BoundField DataField="cnt" HeaderText="No."/>
                        <asp:BoundField DataField="leave" HeaderText="Leave"/>
                        <asp:BoundField DataField="date" HeaderText="Date"/>
                        <asp:BoundField DataField="noh" HeaderText="No of Days"/>
                        <asp:BoundField DataField="pay" HeaderText="Is with pay"/>
                        <asp:BoundField DataField="expectedin" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="can" runat="server" ToolTip="Remove" CausesValidation="false" OnClick="delete_tran" ><i class="fa fa-trash"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="5px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div class="form-group no-margin">
                    <label>Attach File</label>
                    <asp:FileUpload ID="file_img" CssClass="" runat="server" />
                </div>
              </div>
              <asp:Button ID="Button1" runat="server" OnClick="click_save" Text="Save" CssClass="btn btn-primary pull-left"/>
            </div>
        </div>
    </div>
</div>

<div id="modal" runat="server" class="modal fade in" >
<div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
            <asp:LinkButton ID="lb_close" runat="server" OnClick="cpop" CssClass="close">&times;</asp:LinkButton>
            <h4 class="modal-title"><asp:Label ID="lbl_lt" runat="server"></asp:Label></h4>
        </div>
        <div class="modal-body">
           <asp:GridView ID="grid_pay" runat="server" AutoGenerateColumns="false" OnRowDataBound="rowboundgrid_pay" EmptyDataText="No record found" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="date" HeaderText="Date Leave" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="WithPay" HeaderText="With Pay"/>
                    <asp:BoundField DataField="nod" HeaderText="No. of Days"/>
                    <asp:BoundField DataField="designation" HeaderText="Designation"/>
                    <asp:BoundField DataField="status" HeaderText="Status"  ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:TemplateField HeaderText="Action"  ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"  >
                        <ItemTemplate>
                            <%--<asp:LinkButton ID="lnk_can" OnClick="view" ToolTip="Cancel" CssClass="glyphicon glyphicon-remove-sign" runat="server"></asp:LinkButton>--%>
                        </ItemTemplate>
                        <ItemStyle  Width="10px"/>
                    </asp:TemplateField>
                    <asp:BoundField DataField="l_id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                    <asp:BoundField DataField="request_id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                </Columns>
            </asp:GridView>
            <asp:Panel ID="pnlApproverHistory" runat="server">
            <div class="modal-header">
                <h5 class="modal-title">Approver History</h5>
            </div>
            <asp:GridView ID="grid_history" runat="server" AutoGenerateColumns="false"  EmptyDataText="No History"  CssClass="table table-striped table-bordered no-margin hidden">
                <Columns>
                    <asp:BoundField DataField="sysdate" HeaderText="Date Processed" ItemStyle-Width="50px" />
                    <asp:BoundField DataField="approver" HeaderText="Approver" ItemStyle-Width="50px"/>
                    <asp:BoundField DataField="remarks" HeaderText="Remarks" ItemStyle-Width="150px"/>
                    <asp:BoundField DataField="status" HeaderText="Status" ItemStyle-Width="50px"/>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_approver" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered ">
                <Columns>
                    <asp:BoundField DataField="date" HeaderText="Date Approved" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="approver" HeaderText="Approver"/>
                    <asp:BoundField DataField="status"  HeaderText="Status"/>
                </Columns>
            </asp:GridView>
            </asp:Panel>
        </div>
    </div>
</div>
</div>

<div id="modal1" runat="server" class="modal fade in">
<div class="modal-dialog" style="width: 500px;">
    <div class="modal-content">
      <div class="modal-header">
        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="click_close" class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
        <h4 class="modal-title"> People assigned you as their delegate :</h4>
      </div>
      <div class="modal-body">
        <asp:Panel ID="Panel1" runat="server">
            <div class="x_panel">
            <div class="row">
                <div class="col-md-12">
                    <div id="alert" runat="server" class="alert alert-default"><i class="fa fa-info-circle"></i> <span>No record found</span></div>
                    <asp:GridView ID="grid_forms" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered" ClientIDMode="AutoID">
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="ID" HeaderStyle-CssClass="none" ItemStyle-CssClass="none"/>                     
                                <asp:BoundField DataField="fullname" HeaderText="Name"/>
                            <asp:BoundField DataField="tDateStart" HeaderText="From"/>
                            <asp:BoundField DataField="tDateEnd" HeaderText="To"/>
                        </Columns>
                    </asp:GridView> 
                </div> 
            </div>                  	
            </div>
        </asp:Panel>
      </div>
    </div>
</div>
</div>

<asp:HiddenField ID="id" runat="server" />
<asp:HiddenField ID="seriesid" runat="server" />
<asp:HiddenField ID="user_id" runat="server" />
<asp:HiddenField ID="query" runat="server" />
<asp:HiddenField ID="hfdelegates" runat="server" />
</asp:Content>
<asp:Content ID="footer" ContentPlaceHolderID="footer" runat="server">
    <!--Select-->
    <script type="text/javascript" src="vendors/select2/dist/js/select2.full.min.js"></script>
    <script type="text/javascript">
        (function ($) {
            $('.select2').select2();


            $("#ddlOIC").on("change keydown paste input", function () {
                if ($("[id$=txt_from]").val() != "" && $("[id$=txt_to]").val() != "") {

                    console.log("asdbasjhbfhrkrejhthbfdjkhfkdhfgekiufgrifuhverlileighlsdigbfdbgmew,fjdsnvjoewjfoienfkdshglsdfhiwuhgbskdbvzxbcksdfslifuhweo");
                    console.log($(this).val());
                }
                else {
                    console.log("nonononono");
                    if ($("[id$=txt_from]").val() == "" && $("[id$=txt_to]").val() == "") {

                        $("[id$=lb_errorfrom]").text("This is a required field.");
                        $("[id$=lb_errorto]").text("This is a required field.");
                    }
                    else if ($("[id$=txt_from]").val() != "" && $("[id$=txt_to]").val() == "") {

                        $("[id$=lb_errorto]").text("This is a required field.");
                    }
                    else if ($("[id$=txt_from]").val() == "" && $("[id$=txt_to]").val() != "") {
                        $("[id$=lb_errorfrom]").text("This is a required field.");
                    }

                    console.log($(this).val());
                }
            });
        })(jQuery);

    </script>
</asp:Content>
