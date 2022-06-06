<%@ Page Language="C#" AutoEventWireup="true" CodeFile="manual_login.aspx.cs" Inherits="content_Employee_manual_login" MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_ot_list">
    <script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        jQuery.noConflict();
        (function($) {
            $(function() {
                $(".datee").datepicker({ changeMonth: true,
                    yearRange: "-100:+0",
                    changeYear: true
                });
            });
        })(jQuery);
    </script>
    <link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>
</asp:Content>


<asp:Content ContentPlaceHolderID="content" ID="content_manual" runat="server">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Time Adjustment Application</h3>
    </div>  
     <div class="title_right">
        <ul>
            <li><a href="KOISK_MANUAL?user_id=<% Response.Write(Request.QueryString["user_id"].ToString()); %>"><i class="fa fa-clock-o"></i>  Time Adjustment</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>Time Adjustment Application</li>
        </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
            <div class="x-head">
                <asp:DropDownList ID="ddl_dtrfile" runat="server" CssClass="minimal"></asp:DropDownList> 
                <asp:Button  ID="btn_go" runat="server" OnClick="dtr_go" Text="Load"  CssClass="btn btn-primary" />
                <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                    <Columns>
                        <asp:BoundField DataField="date" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy}"/>
                        <asp:BoundField DataField="Date_Time_In" HeaderText="Time In" />
                        <asp:BoundField DataField="Date_Time_Out" HeaderText="Time Out"  />
                    </Columns>
                </asp:GridView>
                <br />
                
                <table>
                        <tr>
                            <td>Date Time In1</td>
                            <td>:</td>
                            <td><asp:TextBox ID="txt_date_in1" placeholder="date" OnTextChanged="verify_date" AutoPostBack="true" CssClass="datee" autocomplete="off"  runat="server"></asp:TextBox> <asp:Label ID="lbl_date_in" runat="server"  style="color:Red;" Text=""></asp:Label></td>
                            <td><asp:TextBox ID="txt_time_in1" placeholder="time" runat="server" CssClass="nobel"></asp:TextBox> <asp:Label ID="lbl_time_in" runat="server"  style="color:Red;" Text=""></asp:Label></td>
                        </tr>
                        <div id="div_1" visible="false" runat="server">
                        <tr>
                            <td>Date Time Out1</td>
                            <td>:</td>
                            <td><asp:TextBox ID="txt_date_out1" placeholder="date" CssClass="datee" autocomplete="off"  runat="server"></asp:TextBox> <asp:Label ID="lbl_date_out" runat="server"  style="color:Red;" Text=""></asp:Label></td>
                            <td><asp:TextBox ID="txt_time_out1" placeholder="time" runat="server" CssClass="nobel"></asp:TextBox> <asp:Label ID="lbl_time_out" runat="server"  style="color:Red;" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Date Time In2</td>
                            <td>:</td>
                            <td> <asp:TextBox ID="txt_date_in2" placeholder="date" CssClass="datee" autocomplete="off"  runat="server"></asp:TextBox> <asp:Label ID="Label3" runat="server"  style="color:Red;" Text=""></asp:Label></td>
                            <td> <asp:TextBox ID="txt_time_in2" placeholder="time" runat="server" CssClass="nobel"></asp:TextBox> <asp:Label ID="lbl_time_in2" runat="server"  style="color:Red;" Text=""></asp:Label></td>
                        </tr>
                        </div>
                        <tr>
                            <td>Date Time Out2</td>
                            <td>:</td>
                            <td> <asp:TextBox ID="txt_date_out2" placeholder="date" CssClass="datee" autocomplete="off"  runat="server"></asp:TextBox> <asp:Label ID="lbl_date_out2" runat="server"  style="color:Red;" Text=""></asp:Label></td>
                            <td> <asp:TextBox ID="txt_time_out2" placeholder="time" runat="server" CssClass="nobel" ></asp:TextBox> <asp:Label ID="lbl_time_out2" runat="server"  style="color:Red;" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Reason</td>
                            <td>:</td>
                            <td>
                                <asp:DropDownList ID="txt_reason" runat="server" CssClass="minimal" DataSourceID="sql_income" DataTextField="manual_type" DataValueField="Id" ClientIDMode="Static" AppendDataBoundItems="true" >
                                <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="sql_income" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>"  SelectCommand="select * from time_adjustment">
                                </asp:SqlDataSource>
                                <asp:Label ID="lbl_reason" runat="server" style="color:Red;" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Remarks</td>
                            <td>:</td>
                            <td> <asp:TextBox ID="txt_remarks" TextMode="MultiLine" runat="server"></asp:TextBox> <asp:Label ID="lbl_remarks" runat="server" style="color:Red;" Text=""></asp:Label></td>
                        </tr>
                    </table>
                <hr />
                <asp:Button ID="Button2" runat="server" OnClick="btn_save_Click" Text="SAVE" CssClass="btn btn-primary" />
            </div>
        </div>
    </div>
</div>





    <asp:HiddenField ID="user_id" runat="server" />







<script src="script/jtimepicker/timepicki.js" type="text/javascript"></script>
<script type="text/javascript">
    jQuery.noConflict();
    (function ($) {
    $('.nobel').timepicki();
    })(jQuery);
</script>
<script src="script/jtimepicker/bootstrap.min.js" type="text/javascript"></script>
</asp:Content>

