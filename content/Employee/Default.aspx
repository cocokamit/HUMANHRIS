<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="content_Employee_Default" MasterPageFile="~/content/site.master"%>
<%@ Import Namespace="System.Data" %>
<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_leave">
    <script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
    <script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
    <script>
        jQuery.noConflict();
        (function ($) {
            $(function () {
                $(".datee").datepicker({ changeMonth: true,
                    yearRange: "-0:+1",
                    changeYear: true
                });
            });
        })(jQuery);
    </script>
    <style type="text/css">
       .fc-basic-view { border:1px solid #ddd; border-top:none}
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" ID="content_leave" runat="server">
<section class="content-header">
    <h1>Dashboard</h1>
    <ol class="breadcrumb">
    <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
    <li class="active">Dashboard</li>
    </ol>
</section>
<section class="content">
<div class="row">
    <div class="col-md-3 col-sm-6 col-xs-12">
        <div class="info-box">
        <span class="info-box-icon bg-aqua"><i class="fa fa-clock-o"></i></span>
        <div class="info-box-content">
            <asp:Label ID="l_today" runat="server" CssClass="info-box-text"></asp:Label>
            <span class="info-box-number" style="font-size:25px"><asp:Label ID="lbl_time" runat="server"></asp:Label> <small><asp:Label ID="lbl_ampm" runat="server"></asp:Label></small></span>
            <a href="javascript:void(0)"  class="text-green">Log In</a>
        </div>
        </div>
    </div>
    <!-- /.col -->
    <div class="col-md-3 col-sm-6 col-xs-12">
        <div class="info-box">
        <span class="info-box-icon bg-yellow"><i class="glyphicon glyphicon-list-alt"></i></span>
        <div class="info-box-content">
            <span class="info-box-text">REQUEST</span>
            <asp:Label ID="l_request" runat="server" CssClass="info-box-number"></asp:Label>
            <a href="javascript:void(0)"  data-toggle="modal" data-target="#modal-default">More</a>
        </div>
        </div>
    </div>

    <!-- fix for small devices only -->
    <div class="clearfix visible-sm-block"></div>

    <div class="col-md-3 col-sm-6 col-xs-12">
        <div class="info-box">
        <span class="info-box-icon bg-green"><i class="glyphicon glyphicon-briefcase"></i></span>

        <div class="info-box-content">
            <span class="info-box-text">Leave Credit</span>
            <span class="info-box-number"><asp:Label ID="l_vlc" runat="server"></asp:Label> <i class="font-light text-primary"> Vacation</i></span>
            <span class="info-box-number"><asp:Label ID="l_sl" runat="server"></asp:Label> <i class="font-light text-danger"> Sick</i></span>
        </div>
        <!-- /.info-box-content -->
        </div>
        <!-- /.info-box -->
    </div>
    <!-- /.col -->
    <div class="col-md-3 col-sm-6 col-xs-12">
        <div class="info-box">
        <span class="info-box-icon bg-red"><i class="glyphicon glyphicon-flag"></i></span>
        <div class="info-box-content">
            <span class="info-box-text">Holidays</span>
            <asp:Label ID="l_holiday" runat="server" CssClass="info-box-number"></asp:Label>
            <asp:LinkButton ID="lb_upcoming" runat="server" Text="Upcoming"></asp:LinkButton>
        </div>
        </div>
    </div>
    </div>

  <div class="row">
       
          <!-- /.box -->
            <div class="col-md-6">
              <div class="box box-warning direct-chat direct-chat-warning">
                <div class="box-header with-border">
                            <i class="fa fa-bullhorn"></i>
                           <h3 class="box-title">Announcement</h3>
                </div>
                <div class="box-body">
                  <div class="direct-chat-messages">
                    <% DataTable announcement = getdata.announcement(Session["emp_id"].ToString()); %>
                    <% if (announcement.Rows.Count == 0){ %>
                    <label class="text-info pad"><i class="fa fa-info-circle"></i> No record found</label>
                    <% } else {%>
                     <ul class="products-list product-list-in-box">
                    <% foreach (System.Data.DataRow row in announcement.Rows){ %>
                        <li class="item">
                          <div class="product-info no-margin">
                            <a href="KIOSK_memo?user_id=<%= Request.QueryString["user_id"] %>&not=<%= row["id"].ToString() %>" class="product-title text-capitalize"><%= row["memo_from"]%>
                              <span class="label <%= row["rd"].ToString() == "0" ? "label-primary" : "label-default" %> pull-right"><%= row["memo_date"].ToString()%></span>
                              <span class="product-description">
                                  <%= row["memo_subject"].ToString() %>
                              </span>
                            </a>
                          </div>
                        </li>
                    <% } %>
                    </ul>
                    <% } %>
                  </div>
                </div>
                <div class="box-footer">
             <%--   <a href="KIOSK_memo" class="uppercase">View all</a>--%>
                </div>
              </div>
            </div>
         <!-- /.box -->
            <div class="col-md-6">
              <div class="box box-warning direct-chat direct-chat-warning">
                <div class="box-header with-border">
                <h3 class="box-title"><%Response.Write(DateTime.Now.ToString("MMMM") + " Celebrant"); %></h3>
                </div>
                <div class="box-body">
                  <div class="direct-chat-messages">
                  <% 
                      string bggg = "";
                      foreach (DataRow drgetcel in getdata.Celebrant().Rows)
                      {
                        bggg = drgetcel["eventss"].ToString().Contains("Happy") ? "text-red" : ""; %>
                        <div class="direct-chat-msg">
                            <div class="direct-chat-info clearfix">
                                <span class="direct-chat-name pull-left"><%Response.Write(drgetcel["e_name"].ToString()); %> <b><%Response.Write(" - "+drgetcel["department"].ToString()); %></b></span>
                                <span class="direct-chat-timestamp pull-right"><%Response.Write(drgetcel["eventdate"].ToString()); %></span>
                            </div>
                            <img class="direct-chat-img " src="files/profile/0.png" alt="message user image ">
                            <div class="direct-chat-text <% Response.Write(bggg); %>" >
                                <% Response.Write(drgetcel["eventss"].ToString()); %>
                            </div>
                        </div>
                    <%} %>
                  </div>
                </div>
                <div class="box-footer">
                </div>
              </div>
            </div>
    <div class="col-md-12">
        <div class="box box-primary">
        <div class="box-header with-border">
            <div class="row">
            <div class="col-md-6">
                <div class="row">
                    <div class="col-md-12">
                    <i class="fa fa-calendar-o"></i>
                    <h3 class="box-title">Schedule</h3>
                    </div>
                </div>
                <div class="row">
                    <%--<div class="col-md-12">
                    <label class="btn btn-primary pull-left" style="background-color:#3a87ad;">Schedule</label><label class="pull-left">&nbsp;</label>
                    <label class="btn btn-primary pull-left" style="background-color:#00a65a;">Holiday</label><label class="pull-left">&nbsp;</label>
                    <label class="btn btn-primary pull-left" style="background-color:#f39c12;">Announcement</label><label class="pull-left">&nbsp;</label>
                    <label class="btn btn-primary pull-left" style="background-color:#00c0ef;">Note</label><label class="pull-left">&nbsp;</label>
                    <label class="btn btn-primary pull-left" style="background-color:#800080;">Leave</label><label class="pull-left">&nbsp;</label>
                    </div>--%>
                </div>
            </div>
            <div class="col-md-6">
              <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-primary btn-rounded mb-4 pull-right" OnClick="delviewclick">
                <i class="fa fa-trash"></i></asp:LinkButton>
             <a href="" class="btn btn-primary btn-rounded mb-4 pull-right" data-toggle="modal" data-target="#myModal">
                <i class="fa fa-paste"></i></a>
            </div>    
        </div>
        </div>
        <div class="box-body">
          <div id="calendar">
          </div>
        </div>
        </div>
    </div>
<div id="myModal" class="modal fade" role="dialog" style="position:fixed;">
  <div class="modal-dialog"  >

    <!-- Modal content-->
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Add Note</h4>
      </div>
      <div class="modal-body">
             <ul class="input-form">
             <li>Date</li>
                        <li>
                          <asp:TextBox ID="txt_datefirst" runat="server" ClientIDMode="Static" CssClass=" form-control datee"></asp:TextBox></li>
             <li>Expiration Date</li>           
                        <li>
                           <asp:TextBox ID="txt_dateexp" runat="server" ClientIDMode="Static" CssClass=" form-control datee"></asp:TextBox></li>
             <li>Decription</li>
                        <li>
                            <asp:TextBox ID="txt_description" runat="server" ClientIDMode="Static" CssClass="form-control" ></asp:TextBox></li>
                    </ul>
      </div>
      <div class="modal-footer">
        <asp:Button ID="btn_Save" CssClass="btn btn-success" runat="server" Text="Save" OnClick="Save_Click" />
      </div>
    </div>

  </div>
</div>
      

      <div id="modal" runat="server" class="modal fade in">
        <div class="modal-dialog" style="">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:LinkButton ID="LinkButton2" runat="server" OnClick="click_close" class="close"
                        aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 class="modal-title">
                        Delete Note</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel ID="Panel1" runat="server">
                        <div class="x_panel" style=" overflow:auto; max-width:600px;">
                      <div id="alert" runat="server" class="alert alert-default">
                    <i class="fa fa-info-circle"></i>
                    <span>No record found</span>
                </div>
                <div>
                 <asp:GridView ID="grid_forms" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered" ClientIDMode="AutoID">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="ID" HeaderStyle-CssClass="none" ItemStyle-CssClass="none"/>  
                         <asp:TemplateField  HeaderText="Action">
                            <ItemTemplate>
                                   <asp:LinkButton ID="viewbtn" runat="server" OnClick="deleteer" CssClass="fa fa-trash" AutoPostBack="True"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>                   
                        <asp:BoundField DataField="Date" HeaderText="Date"/>
                        <asp:BoundField DataField="Description" ItemStyle-Width="150px" HeaderText="Description" />
                       
                    </Columns>
                </asp:GridView>    
                </div>           	
			</div>
                    </asp:Panel>
                </div>
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
        <h4 class="modal-title">Request Application</h4>
        </div>
        <div class="modal-body">
           <ul class="todo-list">
                <li>
                <a href="KOISK_OT">
                  <span class="handle"><i class="fa fa-clock-o"></i></span>
                  <span class="text">Over Time</span>
                  <asp:Label ID="l_ot" runat="server" CssClass="label label-danger"></asp:Label> 
                </a>
                </li>
                <li>
                 <a href="KIOSK_undertime">
                  <span class="handle"><i class="fa fa-cut"></i></span>
                  <span class="text">Undertime</span>
                  <asp:Label ID="l_undertime" runat="server" CssClass="label label-default"></asp:Label>
                 </a> 
                </li>
                <li>
                 <a href="KOISK_LEAVE">
                  <span class="handle"><i class="fa fa-calendar-o"></i></span>
                  <span class="text">Leave</span>
                  <asp:Label ID="l_leave" runat="server" CssClass="label label-warning"></asp:Label>
                 </a> 
                </li>
                <li>
                 <a href="KOISK_MANUAL">
                  <span class="handle"><i class="fa fa-history"></i></span>
                  <span class="text">Time Adjustment</span>
                  <asp:Label ID="l_ta" runat="server" CssClass="label label-success" Text="1"></asp:Label>
                  </a> 
                </li>
                <li>
                 <a href="KIOSK_travel">
                  <span class="handle"><i class="fa fa-suitcase"></i></span>
                  <span class="text">Official Business Trip</span>
                  <asp:Label ID="l_obt" runat="server" CssClass="label label-primary" Text="1"></asp:Label>
                  </a> 
                </li>
                <li>
                 <a href="KOISK_Restday">
                  <span class="handle"><i class="fa fa-share-square-o"></i></span>
                  <span class="text">Work Verification</span>
                  <asp:Label ID="l_wvs" runat="server" CssClass="label label-info" Text="0"></asp:Label>
                  </a> 
                </li>
                <li>
                 <a href="esl">
                  <span class="handle"><i class="fa fa-sign-out"></i></span>
                  <span class="text">Change Shift</span>
                  <asp:Label ID="l_csht" runat="server" CssClass="label label-default" Text="0"></asp:Label>
                  </a> 
                </li>
                <li>
                <a href="otmeal">
                  <span class="handle"><i class="fa fa-bitbucket"></i></span>
                  <span class="text">Meal Allowance</span>
                  <asp:Label ID="l_mla" runat="server" CssClass="label label-success"></asp:Label> 
                </a>
                </li>
              </ul>
        </div>
    </div>
    </div>
</div>
<asp:HiddenField ID="hf_id" runat="server"/>

<asp:HiddenField ID="hf_empid" runat="server"/>
</asp:Content>
 <asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
<link rel='stylesheet prefetch' href="vendors/calendar/fullcalendar.min.css"'>
<script src="vendors/calendar/jquery.min.js"></script>
<script src="vendors/calendar/moment.min.js"></script>
<script src="vendors/calendar/fullcalendar.min.js"></script>
    <script type="text/javascript">

         $(document).ready(function () {
             $('#calendar').fullCalendar({
                 header: {
                     left: 'prev,next today',
                     center: 'title',
                     right: 'month,basicWeek,basicDay'
                 },
                 <%= "defaultDate: '"+today+"'," %>
                 navLinks: true, // can click day/week names to navigate views
                 editable: true,
                 eventLimit: true, // allow "more" link when too many events
                 events: [<%= events %>],
                 editable: false,
                 droppable: false
                 
             });

             
               var empid=$("[id$=hf_empid]").val();
             
             console.log("c");
                       $.ajax({
                        type: "POST",
                        url: "content/Employee/default.aspx/getLeaveUpdate",
                        data: JSON.stringify({
                            id: empid
                        }),
                        contentType: "application/json;",
                        dataType: "json",
                        success: function (r) {

                            console.log("success");

                        },
                        error: function (r) {
                            console.log("error: " + r);
                        }
                    });
            
         });
     </script>
 </asp:Content>