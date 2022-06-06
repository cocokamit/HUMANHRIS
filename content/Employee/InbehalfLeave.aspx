<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InbehalfLeave.aspx.cs" Inherits="content_Employee_InbehalfLeave"
    MasterPageFile="~/content/site.master" %>

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
                                                x.options[j].setAttribute('disabled','disabled');
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
    <section class="content-header">
    <h1>Leave Application</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li><a href="KOISK_LEAVE">Leave</a></li>
    <li class="active">Leave Application</li>
    </ol>
</section>
    <section class="content">
    <div class="row">
        <div class="col-lg-4">
         <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">Leave Credits<asp:Label ID="lblempname" runat="server" Text="EMpsNmae"></asp:Label></h3>
            </div>
            <div class="box-body">
                 <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" EmptyDataText="No Leave Credits"  CssClass="table table-striped table-bordered no-margin"><%--OnRowDataBound="datatbound"--%>
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
          <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">Application</h3>
                 <%--<input type="button" id="btndelegates" value="i" class="fa fa-info-circle mb-4 pull-right bg-primary" style="  border-radius: 50%;border: 0; "  data-toggle="modal" data-target="#modalForm" />--%>
                <asp:Button ID="btndelagates" runat="server" OnClick="modalclick" CssClass="fa fa-info-circle mb-4 pull-right bg-red" Text="i" style="border-radius: 50%;border: 0;" />
            </div>
            <div class="box-body">
                <div class="form-group radio  no-margin">
                    <asp:RadioButtonList ID="rb_range" runat="server" OnSelectedIndexChanged="select_nod" AutoPostBack="true">
                        <asp:ListItem Value="1">Whole Day</asp:ListItem>
                        <asp:ListItem Value="0.5">Half Day</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="form-group group">
                    <div id="startin" class="radio" style=" display:none;" runat="server">
                        <label style="margin-left:-20px">Expected Leave Designation<asp:Label ID="lbl_eiotd" CssClass="text-danger" runat="server"></asp:Label></label>
                        <asp:RadioButtonList ID="radiobtnlist" runat="server">
                             <asp:ListItem Value="1">1st Half (8:30 am to 1:15 pm (4.45 hours))</asp:ListItem>
                             <asp:ListItem Value="2">2nd Half  (1:15 pm to 6:00 pm (4.45 hours))</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div class="form-group">
                    <label>Leave Type</label>
                    <asp:DropDownList ID="ddl_leave" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="row">
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
                            <%--<asp:Label ID="lb_errorto" runat="server" ForeColor="Red" ClientIDMode="Static" ></asp:Label>--%>
                        </div>
                    </div>
                </div>
                
                <div class="form-group none">
                    <label>Delegate To <asp:Label ID="lbl_delegate" CssClass="text-danger" runat="server" Text=""></asp:Label></label>
                    <asp:TextBox ID="txt_delegate" runat="server" AutoComplete="off" CssClass="form-control"  Visible="false"></asp:TextBox>
                    <asp:DropDownList ID="ddlOIC" runat="server" CssClass="select2" Visible="false" ClientIDMode="Static" ></asp:DropDownList>
                    <asp:DropDownList ID="DropDownList1" runat="server" style="display:none;" ClientIDMode="Static" ></asp:DropDownList>
                </div>
                <div class="form-group no-margin">
                    <label>Reason <asp:Label ID="lbl_remarks" CssClass="text-danger" runat="server" Text=""></asp:Label></label>
                    <asp:TextBox ID="txt_lineremarks" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="box box-footer pad">
                <asp:Button ID="Button2" runat="server" OnClick="click_veirfy" Text="Verify" CssClass="btn btn-primary" />
                 <asp:Label ID="lbl_error_msg" CssClass="text-danger" ForeColor="Red" runat="server" Text=""></asp:Label>
                <asp:Label ID="lbl_err" runat="server" ForeColor="Red" ></asp:Label>
            </div>
          </div>
        </div>
        <div class="col-lg-8">
          <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">Leave Transaction</h3>
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
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="can" runat="server" ToolTip="Delete" CausesValidation="false" OnClick="delete_tran" ImageUrl="~/style/img/delete.png" />
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
            <div class="box box-footer pad">
                <asp:Button ID="Button1" runat="server" OnClick="click_save" Text="Save" CssClass="btn btn-primary"/>
            </div>
          </div>
        </div>
        
    </div>

     <div id="modal" runat="server" class="modal fade in">
        <div class="modal-dialog" style="width: 500px;">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="click_close" class="close"
                        aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 class="modal-title">
                        People assigned you as their delegate :</h4>

                </div>
                <div class="modal-body">
                    <asp:Panel ID="Panel1" runat="server">
                        <div class="x_panel">
                            <div class="row">
				                <div class="col-md-12">
				                        <div id="alert" runat="server" class="alert alert-default">
                    <i class="fa fa-info-circle"></i>
                    <span>No record found</span>
                </div>
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

</section>
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
