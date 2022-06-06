<%@ Page Language="C#" AutoEventWireup="true" CodeFile="verifyTravel.aspx.cs" Inherits="content_hr_verifyTravel" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_travel_list">
        <style type="text/css">
        .div{width:100%; height:auto; border:none; float:left; padding:1px; }
        *{ font-size:12PX; font-style:normal; font-weight:normal;}
        #nav,#nav1 
        {
         background: gray url(style/images/nav_bkg.jpg) repeat-x 0% 0%; float: left; list-style: none; margin: 0; padding: 0; width: 100%; 
        }
        #nav,#nav1 li 
        {
         float: left; font-size: 11px; margin: 0; padding: 1; text-transform: uppercase; 
        }
        #nav1 li a 
        {
         border-bottom: none; border-right: 1px solid #392c2b; color: #d8cbca; float: left; letter-spacing: 1px; padding: 0px 8px; text-decoration: none; 
        }
        #nav li a {
         border-bottom: none; border-right: 1px solid #392c2b; color: #d8cbca; float: left; letter-spacing: 1px; padding: 10px 16px; text-decoration: none; 
        }
        #nav li a:hover {
         background: #433433 url(style/images/search_bkg.jpg) repeat-x 0% 0%; text-decoration: none; 
        }
        .click{background: #433433 url(style/images/search_bkg.jpg) repeat-x 0% 0%; text-decoration: none; }
  
        select {

            width: 200px;
            padding: 6px 20px;
            margin: 8px 0;
            display: inline-block;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box;
        }
        .btn input[type=submit] 
        {
            width: 100%;
            background-color: #4CAF50;
            color: white;
            padding: 9px 30px;
            margin: 8px 0;
            border: none;
            border-radius: 4px;
            cursor: pointer;
        }
        .btn input[type=submit]:hover 
        {
            background-color: #45a049;
        }
        .Grid { background-color: #fff; margin: 5px 0 10px 0; border: solid 1px #525252; border-collapse:collapse; font-family:Calibri; color: #474747; width: 100%; font-size: 12px; text-align: center;}
        .Grid td {padding: 2px; border: solid 1px #c1c1c1; }
        .Grid th {padding : 4px 2px;color: #fff;background: #333 url(Images/grid-header.png) repeat-x top;border-left: solid 1px #525252;font-size: 0.9em;text-align: center;text-transform: uppercase;font-weight: bold; }
        .Grid .alt {background: #fcfcfc url(Images/grid-alt.png) repeat-x top; }
        .Grid .pgr {background: #363670 url(Images/grid-pgr.png) repeat-x top; }
        .Grid .pgr table { margin: 3px 0; }
        .Grid .pgr td { border-width: 0; padding: 0 6px; border-left: solid 1px #666; font-weight: bold; color: #fff; line-height: 12px; }  
        .Grid .pgr a { color: Gray; text-decoration: none; }
        .Grid .pgr a:hover{ color: #000; text-decoration: none; }
        .PopUpPanel { position:absolute;background-color: #fff;   top:25%;left:35%;z-index:2001; padding:20px;min-width:200px;max-width:800px;-moz-box-shadow:2px 2px 3px #000000;-webkit-box-shadow:2px 2px 5px #000000;box-shadow:2px 2px 5px #000000;border-radius:1px;-moz-border-radiux:5px;-webkit-border-radiux:5px;}
        .PopUpPanel3{ top:30%; width:350px;left:60%; height:700px}
        .close{ margin:-10px;float:right;}

        .Overlay2 {  position:fixed; top:0px; bottom:0px; left:0px; right:0px; overflow:hidden; padding:0; margin:0; background-color:#000; filter:alpha(opacity=50); opacity:0.5; z-index:1000;}
        .PopUpPanel2 { position:absolute;background-color: #fff;   top:25%;left:35%;z-index:2001; padding:20px;min-width:200px;max-width:600px;-moz-box-shadow:2px 2px 3px #000000;-webkit-box-shadow:2px 2px 5px #000000;box-shadow:2px 2px 5px #000000;border-radius:5px;-moz-border-radiux:5px;-webkit-border-radiux:5px;}
        .PopUpPanel3 {   border-radius:4px; position: fixed; z-index: 1002;  width: 600px; top: 50%;left: 50%; margin: -180px 0 0 -300px; background-color: #fff; }
        </style>

<script src="script/auto/myJScript.js" type="text/javascript"></script>
<script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to this job?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }

    }

</script>

<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
<script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>   
<script type="text/javascript" src="script/myJScript.js"></script>
<script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
<link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
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
</script>
    <link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>
    <style type="text/css">
        .radio { margin-left: 20px}
</style>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="verifyTravel">
<div class="page-title">
    <div class="title_left">
        <h3>Verify Travel</h3>
    </div>
</div>
<div class="clearfix"></div>
    <div class="row">
        <div class="col-md-12  col-sm-12 col-xs-12">
            <div class="x_panel">
                <div class="x-head">
                    <asp:TextBox ID="txt_from" placeholder="From" CssClass="datee" style="float:left; width:150px; margin-right:5px" ClientIDMode="Static" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txt_to" ClientIDMode="Static" CssClass="datee" style="float:left; width:150px;margin-right:5px"  placeholder="To" runat="server" ></asp:TextBox>
                    <asp:Button ID="btn_search" runat="server" OnClick="search" Text="search" CssClass="btn btn-primary"/>
                </div>
            
                <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" onrowdatabound="GridView_RowDataBound"  CssClass="table table-striped table-bordered table-input">
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="date_input" HeaderText="Date Filed" DataFormatString="{0:MM/dd/yyyy}"/>
                        <asp:BoundField DataField="Fullname" HeaderText="Employee Name"/>
                        <asp:BoundField DataField="purpose" HeaderText="Purpose"/>
                        <asp:BoundField DataField="travel_start" HeaderText="Travel Start"  DataFormatString="{0:MM/dd/yyyy}"/>
                        <asp:BoundField DataField="travel_end" HeaderText="Travel End"  DataFormatString="{0:MM/dd/yyyy}"/>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                 <asp:LinkButton runat="server" ID="lnk_approve" Visible="false" OnClientClick="Confirm()" OnClick="approve" CssClass="fa fa-thumbs-up"> </asp:LinkButton>
                                 <asp:LinkButton runat="server" ID="lnk_view" OnClick="view" CssClass="fa fa-sliders"></asp:LinkButton>
                                 <asp:LinkButton runat="server" OnClick="view1" ID="lnk_cancel"  CssClass="fa fa-trash-o" ToolTip="Cancel"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="105px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <div id="alert" runat="server" class="alert alert-empty" visible="false">
                    <i class="fa fa-info-circle"></i>
                    <span>No record found</span>
                </div>
            </div>
        </div>
    </div>

<div id="Div1" runat="server" visible="false" class="Overlay"></div>
<div id="Div2" runat="server" visible="false" class="nobel-modal">
    <div class="modal-header">
        <asp:LinkButton ID="lb_close" runat="server" OnClick="cpop"  CssClass="close">&times;</asp:LinkButton>
        <h4 class="modal-title"><asp:Label ID="l_name" runat="server"></asp:Label></h4>
    </div>
    <div class="modal-body">
         <ul class="input-form">

                     <li runat="server" id="meals">
                         Meals: <asp:Textbox ID="txt_meals" AutoComplete="off" runat="server"  onkeyup="intinput(this)" ></asp:Textbox>
                    </li>

                    <li runat="server" id="transportation">
                         Transportation: <asp:Textbox ID="txt_trans" AutoComplete="off" runat="server"  onkeyup="intinput(this)" ></asp:Textbox>
                    </li>
                    <li runat="server" id="accommodation">
                        Accommodation: <asp:Textbox ID="txt_acom" AutoComplete="off" runat="server"  onkeyup="intinput(this)" ></asp:Textbox>
                    </li>

                    <li runat="server" id="other">
                         Other Expense: <asp:Textbox ID="txt_other" AutoComplete="off" runat="server"  onkeyup="intinput(this)" ></asp:Textbox>
                    </li>
                    <li runat="server" id="total">
                        Total Cash Approved : <asp:Textbox ID="txt_cash" AutoComplete="off" runat="server"  onkeyup="intinput(this)" ></asp:Textbox>
                    </li>

                    <li>
                       Expected Budget : <asp:Label ID="lbl_ex" runat="server" Text=""></asp:Label>
                   </li>
                   <li>
                       Actual Budget : <asp:Label ID="lbl_act" runat="server" Text=""></asp:Label>
                   </li>
                  
                   <li>
                       Description : <asp:Label ID="txt_reason" runat="server" Text="Label"></asp:Label>
                   </li>


                   <li>Travel Destinations</li>
                   <li>
                        <asp:GridView ID="grid_destinations" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                            <Columns>
                                <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                <asp:BoundField DataField="place" HeaderText="Place"/>
                                <asp:BoundField DataField="travel_mode" HeaderText="Travel Mode"/>
                                <asp:BoundField DataField="arrange_type" HeaderText="Arrangement Type"/>
                            </Columns>
                        </asp:GridView>
                   </li>
              
            </ul>
    </div>
    <div class="modal-footer">
        <asp:LinkButton ID="lnk_button" Visible="false" runat="server" onclick="lnk_button_Click" CssClass="btn pull-left">Re-Print</asp:LinkButton>
        <asp:Button ID="btn_save" runat="server" Text="Save" onclick="btn_save_Click" CssClass="btn btn-primary" />
    </div>
</div>

<div id="Div3" runat="server" visible="false" class="Overlay"></div>
<div id="Div4" runat="server" visible="false" class="nobel-modal">
    <div class="modal-header">
        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="cpop"  CssClass="close">&times;</asp:LinkButton>
        <h4 class="modal-title">Delete Transaction</h4>
    </div>
    <div class="modal-body">
        <ul class="input-form">
            <li><asp:TextBox ID="txt_reason1" TextMode="MultiLine" runat="server" placeholder="Remarks"></asp:TextBox></li>
        </ul>
        <asp:Button ID="btn_save1" runat="server" OnClick="delete3" Text="Save" CssClass="btn btn-primary" />
    </div>
</div>

<asp:HiddenField ID="key" runat="server" />
<asp:TextBox ID="TextBox1" style="visibility:hidden;" runat="server"></asp:TextBox>
<asp:HiddenField ID="idd" runat="server" />
</asp:Content>

