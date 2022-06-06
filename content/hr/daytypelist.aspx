<%@ Page Language="C#" AutoEventWireup="true" CodeFile="daytypelist.aspx.cs" Inherits="content_hr_daytypelist" MaintainScrollPositionOnPostback="true"
    MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">

    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />  
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
    <script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
    <script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>
    <link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />
    <link href="style/css/tablechkbx.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        jQuery.noConflict();
        (function ($) {
            $(function () {
                $(".datee").datepicker({ changeMonth: true,
                    yearRange: "-100:+0",
                    changeYear: true
                });
            });
        })(jQuery);

        function Confirm() {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure to cancel this transaction?"))
            { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
        }

        var mykey = 'AIzaSyBIxpuK5AVUcEz0U7Yq-hY3GRBFSKM8YSI';
        var calendarid = 'cocok.meshnetworks@gmail.com';
        //        url: encodeURI('https://www.googleapis.com/calendar/v3/calendars/' + calendarid + '/events?key=' + mykey),
        var calendarurl = 'https://www.googleapis.com/calendar/v3/calendars/en.philippines%23holiday%40group.v.calendar.google.com/events?key=' + mykey;

        $(document).ready(function (e) {
            var currentTime = new Date();
            var year = currentTime.getFullYear();

            console.log(year.toString());
            $.ajax({
                type: "POST",
                url: "content/hr/daytypelist.aspx/getUpdate",
                data: JSON.stringify({
                    term: mykey
                }),
                contentType: "application/json;",
                dataType: "json",
                success: function (r) {
                    var UPdate = r.d;
                    console.log(UPdate);

                    console.log("aaa");
                    if (UPdate != year) {

                        //-------------------------------------------------------------------------------------
                        console.log("bbb");
                        $.getJSON(calendarurl)
                                    .success(function (data) {
                                        console.log(data);
                                        var tblarray = new Array();
                                        for (item in data.items) {
                                            var items = {};
                                            var getyearn = new Date(data.items[item].start.date);
                                            if (getyearn.getFullYear() == year) {
                                                items.summary = data.items[item].summary;
                                                items.startdate = data.items[item].start.date;
                                                items.enddate = data.items[item].end.date;
                                                tblarray.push(items);
                                            }

                                        } for (item in data.items) {
                                            var items = {};
                                            var getyearn = new Date(data.items[item].start.date);
                                         
                                            if (getyearn.getFullYear() == year - 1) {
                                                items.summary = data.items[item].summary;
                                                items.startdate = data.items[item].start.date;
                                                items.enddate = data.items[item].end.date;
                                                tblarray.push(items);
                                            } 
                                        }
                                        $.ajax({
                                            type: "POST",
                                            url: "content/hr/daytypelist.aspx/SaveHoliday",
                                            data: JSON.stringify({
                                                term: tblarray
                                            }),
                                            contentType: "application/json;",
                                            dataType: "json",
                                            success: function (r) {
                                                if (r.d == "Mdaytype") {
                                                    window.location = r.d;
                                                }
                                                else if (r.d == "Failed to load.") {
                                                    alert(r.d + "");
                                                }

                                            },
                                            failure: function (r) {
                                                console.log("failure: " + r);
                                            },
                                            error: function (r) {
                                                console.log("error: " + r);
                                            }
                                        });

                                    })
                                            .error(function (error) {
                                                $("#output").html("An error occurred.");
                                            })
                    }
                    //--------------------------------------------------------------------------------------
                },
                failure: function (r) {
                    console.log("failure: " + r);
                },
                error: function (r) {
                    alert("error: " + r.responseText);
                }
            });

        });


    </script>
</asp:Content>
<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
    <div class="page-title">
        <div class="title_left hd-tl">
            <h4>
                Day Type</h4>
        </div>
        <div class="title_right">
            <ul>
                <li><a href="#"><i class="fa fa-gear"></i>Sytem Configurtion </a></li>
                <li><i class="fa fa-angle-right"></i></li>
                <li>Day Type Set Up</li>
            </ul>
        </div>
    </div>
    <div class="clearfix">
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="x_panel">
                <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" OnRowDataBound="gg_ta_ani_brad"
                    CssClass="table table-striped table-bordered">
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                        <asp:BoundField DataField="DayType" HeaderText="Day Type" />
                        <asp:BoundField DataField="WorkingDays" HeaderText="On Working Days" />
                        <asp:BoundField DataField="RestdayDays" HeaderText="On Rest Days" />
                        <asp:BoundField DataField="OTWorkingDays" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                        <%--HeaderText="On OT Working Days"--%>
                        <asp:BoundField DataField="OTRestdayDays" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                        <%--HeaderText="On OT Rest Days"--%>
                        <asp:BoundField DataField="NightWorkingDays" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"
                            HeaderText="On Night Working Days" />
                        <asp:BoundField DataField="NightRestdayDays" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"
                            HeaderText="On Night Rest Days" />
                        <%-- <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk_view" runat="server" Text="view" OnClick="click_edit_daytype"><i class="fa fa-sliders"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="40px" />
                        </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <h4>
                Holidays </h4>
        </div>
        <div class="col-md-6">
            <asp:LinkButton ID="btn_compose" runat="server" Text="Add +" OnClick="Compose_click" CssClass="btn btn-primary pull-right"
                AutoPostBack="True"></asp:LinkButton>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-12">
            <div class="x_panel" style="overflow: auto;">
                <asp:GridView ID="grid_holidays" runat="server" AutoGenerateColumns="false" OnRowDataBound="gg_ta_ani_brad"
                    CssClass="table table-striped table-bordered">
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                        <asp:BoundField DataField="type" HeaderText="Type" />
                        <asp:BoundField DataField="name" HeaderText="Name" />
                        <asp:BoundField DataField="dow" HeaderText="" />
                        <asp:BoundField DataField="startdate" HeaderText="Date (MM/DD/YYYY)" />
                        <%--<asp:BoundField DataField="enddate" HeaderText="End Date (MM/DD/YYYY)" />--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk_holiday" runat="server" Text="view" OnClick="editHoliday"><i class="fa fa-sliders"></i></asp:LinkButton>
                                <asp:LinkButton ID="lnk_deletehol" runat="server" Text="view" ClientIDMode="Static" OnClick="editHoliday" OnClientClick="return confirm('Are you sure you want to remove this holiday?');"><i class="fa fa-trash"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <%---------------------------------------------------------modal for editing holiday----------------------------------------------%>
    <div id="modal" runat="server" class="modal fade in">
        <div class="modal-dialog" style="width: 950px;">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="click_close" class="close"
                        aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 class="modal-title">
                        Edit Holiday</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel ID="Panel1" runat="server">
                        <div class="x_panel">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:TextBox ID="lb_holidate" runat="server" CssClass="form-control"  ClientIDMode="Static"></asp:TextBox>
                             
                                </div>
                            </div>
                             <br />
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Start Date</label>
                                       <asp:TextBox ID="txt_startdate" runat="server" CssClass="form-control datee"  ClientIDMode="Static"></asp:TextBox>
                                </div>
                                <div class="col-md-6 none">
                                    <label>End Date</label>
                                        <asp:TextBox ID="txt_enddate" runat="server" CssClass="form-control datee " ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Type</label>
                                    <asp:DropDownList ID="ddl_datetype" runat="server" CssClass="form-control" ClientIDMode="Static">
                                        <asp:ListItem Value="1">Working Day</asp:ListItem>
                                        <asp:ListItem Value="5">Double Holiday</asp:ListItem>
                                        <asp:ListItem Value="3">Special Holiday</asp:ListItem>
                                        <asp:ListItem Value="4">Regular Holiday</asp:ListItem>
                                        <asp:ListItem Value="6">Company Holiday</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                
                            </div>
                             <br/>
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Location</label>
                                   </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:ListBox ID="listbranch" AutoComplete="off" SelectionMode="Multiple" runat="server"></asp:ListBox>
                                   
                                 <asp:Label ID="Label1" runat="server" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <hr />
                             <div class="row">
                                <div class="col-md-12">
                                        <asp:Button ID="btn_Save" runat="server" OnClick="Save_Click" OnClientClick="return confirm('Are you sure you want to save?')" Text="Save" CssClass="btn btn-success btn-rounded mb-4 pull-right" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>

      <%---------------------------------------------------------modal for creating holiday----------------------------------------------%>
    <div id="modalcreate" runat="server" class="modal fade in">
        <div class="modal-dialog" style="width: 950px;">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:LinkButton ID="LinkButton2" runat="server" OnClick="click_close" class="close"
                        aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 class="modal-title">
                        Create Holiday</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel ID="Panel2" runat="server">
                        <div class="x_panel">
                            <div class="row">
                                <div class="col-md-12">
                                    <label>Name</label>
                                    <asp:TextBox ID="txt_holname" runat="server" CssClass="form-control" ClientIDMod="Static"></asp:TextBox>
                                    <asp:Label ID="lb_error" runat="server" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                             <br />
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Start Date</label>
                                       <asp:TextBox ID="txt_holsdate" runat="server" CssClass="form-control datee" ClientIDMode="Static"></asp:TextBox>
                                </div>
                                <div class="col-md-6 none">
                                    <label>End Date</label>
                                        <asp:TextBox ID="txt_holedate" runat="server" CssClass="form-control datee" ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Type</label>
                                    <asp:DropDownList ID="ddl_holtype" runat="server" CssClass="form-control" ClientIDMode="Static">
                                        <asp:ListItem Value="1">Working Day</asp:ListItem>
                                        <asp:ListItem Value="5">Double Holiday</asp:ListItem>
                                        <asp:ListItem Value="3">Special Holiday</asp:ListItem>
                                        <asp:ListItem Value="4">Regular Holiday</asp:ListItem>
                                        <asp:ListItem Value="6">Company Holiday</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                
                            </div>
                            <br/>
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Location</label>
                       
                                   </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:ListBox ID="lstFruits" AutoComplete="off" SelectionMode="Multiple" runat="server"></asp:ListBox>
                                   
                                 <asp:Label ID="lb_checker" runat="server" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <hr />
                             <div class="row">
                                <div class="col-md-12">
                                        <asp:Button ID="btn_create" runat="server" OnClick="Create_Click" OnClientClick="return confirm('Are you sure you want to save?')" Text="Save" CssClass="btn btn-success btn-rounded mb-4 pull-right" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    <asp:TextBox ID="TextBox1" runat="server" class="hide"></asp:TextBox>
    <asp:HiddenField ID="key" runat="server" />
    <asp:HiddenField ID="pg" runat="server" />
</asp:Content>
<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
   <link rel="stylesheet" href="style/multiple/docs/css/prettify.min.css" type="text/css">
    <script type="text/javascript" src="style/multiple/docs/js/bootstrap-3.3.2.min.js"></script>
    <script type="text/javascript" src="style/multiple/jquery.min.js"></script>
    <script type="text/javascript" src="style/multiple/bootstrap-multiselect.js"></script>
    <script type="text/javascript" src="style/multiple/bootstrap.min.js"></script>
    <link rel="Stylesheet" type="text/css" href="style/multiple/bootstrap-multiselect.css" />

   <script type="text/javascript">
       $(function () {
           $('[id*=lstFruits]').multiselect({

               includeSelectAllOption: true
           });

           $('[id*=listbranch]').multiselect({

               includeSelectAllOption: true
           });
       });
    </script>
 
</asp:Content>
