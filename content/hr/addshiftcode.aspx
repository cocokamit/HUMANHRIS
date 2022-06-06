<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addshiftcode.aspx.cs" Inherits="content_hr_newshiftcode" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <script src="script/auto/myJScript.js" type="text/javascript"></script>
    <style type="text/css">
        th
        {
          font-weight:400;
        }
        .dp
        { 
          display:none;
        }
    </style>
</asp:Content>

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>New Shift</h3>
    </div>   
    <div class="title_right">
        <ul>
            <li><a href="#"><i class="fa fa-gear"></i> System Configuration</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li><a href="Mshiftcode"></i> Shift Code</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>New Shift</li>
        </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-9">
        <div class="x_panel">
            <ul class="input-form"> 
           
                <li>Shift Code <asp:Label ID="lbl_shiftcode" runat="server" ></asp:Label></li>
                <li><asp:TextBox ID="txt_shiftcode" CssClass="input" MaxLength="50" runat="server"></asp:TextBox></li>
                <li>Remarks <asp:Label ID="lbl_remarks" runat="server"></asp:Label></li>
                <li><asp:TextBox ID="txt_remarks" TextMode="MultiLine"  runat="server"></asp:TextBox></li>
            </ul>
            <asp:GridView ID="grid_view" runat="server" OnRowDataBound="grid_viewrowbound" AutoGenerateColumns="false" EmptyDataText="No record found" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="col_1" HeaderText="Day"/>
                    <asp:BoundField DataField="col_rd" HeaderText="RD"/>
                    <asp:BoundField DataField="col_2" HeaderText="Time In 1"/>
                    <asp:BoundField DataField="col_3" HeaderText="Time Out 1" />
                    <asp:BoundField DataField="col_4" HeaderText="Time In 2"/>
                    <asp:BoundField DataField="col_5" HeaderText="Time Out 2"/>
                    <asp:BoundField DataField="col_6" HeaderText="Total No. of HRS." />
                    <asp:BoundField DataField="col_7" HeaderText="Flex HRS."/>
                    <asp:BoundField DataField="col_8" HeaderText="Grace MIN."/>
                    <asp:BoundField DataField="col_9" HeaderText="Total Night Hrs."/>
                    <asp:BoundField DataField="col_10" HeaderText="Night Shift" ItemStyle-CssClass="dp" HeaderStyle-CssClass="dp"/>
                    <asp:BoundField DataField="col_11" HeaderText="Night Break Hrs." ItemStyle-CssClass="dp" HeaderStyle-CssClass="dp"/>
                    <asp:BoundField DataField="col_12" HeaderText="Paid BT"/> 
                    <asp:BoundField DataField="col_13" HeaderText="Mandatory to punch when BT"/> 
                      <asp:BoundField DataField="flexbreak" HeaderText="Flexible Break"/> 
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton2" runat="server"  OnClick="delete" ToolTip="Delete"><i class="fa fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="40px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <hr />
            <asp:Button ID="btn_save" runat="server" OnClick="click_add_shiftcodes" Visible="false" Text="SAVE" CssClass="btn btn-primary" />
        </div>
    </div>
    <div class="col-md-3">
        <div class="x_panel">
            <ul class="input-form">
            <li><asp:CheckBox ID="chk_sched" runat="server" OnCheckedChanged="change" AutoPostBack="true" Text=" Fix Schedule" /></li>
            <div id="fix" runat="server" style=" display:none;">
                <li>Day</li>
                <li>
                    <asp:DropDownList ID="ddl_day" runat="server" CssClass="minimal">
                        <asp:ListItem>Monday</asp:ListItem>
                        <asp:ListItem>Tuesday</asp:ListItem>
                        <asp:ListItem>Wednesday</asp:ListItem>
                        <asp:ListItem>Thursday</asp:ListItem>
                        <asp:ListItem>Friday</asp:ListItem>
                        <asp:ListItem>Saturday</asp:ListItem>
                        <asp:ListItem>Sunday</asp:ListItem>
                    </asp:DropDownList>
                </li>
                </div>
                <li>
                    <table>
                        <tr>
                            <th>Time In</th>
                            <th>&nbsp</th>
                            <th>Time Out</th>
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="txt_timein1" type="Time" runat="server" ></asp:TextBox></td>
                            <td>&nbsp</td>
                            <td><asp:TextBox ID="txt_timeout2" type="Time"  runat="server" ></asp:TextBox></td>
                        </tr>
                    </table>
                  </li>
                  <li><asp:CheckBox ID="chk_flexbreak" runat="server" OnCheckedChanged="click_break_flex" AutoPostBack="true" Text="Flex Break"/>
                  </li>
                   <li>
                    <table id="break_id" runat="server">
                       <tr>
                   
                       </tr>
                        <tr>
                             <th>Out For Break</th>
                             <th>&nbsp</th>
                             <th>In From Break</th>
                        </tr>
                        <tr>
                            <td><asp:TextBox ID="txt_timeout1" type="Time"    ClientIDMode="Static" AutoPostBack="true" OnTextChanged="click_bi" runat="server"  ></asp:TextBox></td>
                            <td>&nbsp</td>
                            <td><asp:TextBox ID="txt_timein2" type="Time"   ClientIDMode="Static" AutoPostBack="true" OnTextChanged="click_bi"  runat="server" ></asp:TextBox></td>
                        </tr>
                     
                      <%--<tr>
                            <td><asp:CheckBox ID="chk_break_setup" runat="server" Text="Mandatory to punch when break." /></td>
                        </tr>--%>
                    </table>
                </li>
              <%--  <li><asp:CheckBox ID="chk_break_setup" runat="server" Text="Mandatory to punch when break." /></li>--%>
                <li><asp:CheckBox ID="chk_ibwp1" runat="server" OnCheckedChanged="click_pb" AutoPostBack="true" Text="Paid Break"/></li>
                <div id="nomb" runat="server" >
                <li>No. of Min. Break</li>
                <li><asp:TextBox ID="txt_nohb" AutoComplete="off" runat="server" min="0"  type="number" ClientIDMode="Static" onkeyup="decimalinput(this);"></asp:TextBox></li>
                </div>
                <li>Flex Min.</li>
                <li><asp:TextBox ID="txt_fh" AutoComplete="off" runat="server" min="0" type="number" ClientIDMode="Static" onkeyup="decimalinput(this);"></asp:TextBox></li>
                <li>Grace Min</li> 
                <li><asp:TextBox ID="txt_gm" AutoComplete="off" runat="server" min="0"  type="number" ClientIDMode="Static" onkeyup="decimalinput(this);"></asp:TextBox></li>
                <li><asp:CheckBox ID="chk_rd" runat="server" Text="Rest Day" /></li>
                <li><hr /></li>
                <li><asp:Button ID="btn_add" runat="server" OnClick="add_into_datatable" Text="ADD" CssClass="btn btn-primary"/></li>
                <li><asp:Label ID="lbl_err" runat="server" ForeColor="Red"></asp:Label></li>
            </ul>
        </div>
    </div>
</div>
<asp:TextBox ID="txt_nh" AutoComplete="off" runat="server" ClientIDMode="Static"  onkeyup="intinput(this)" CssClass="hidden"></asp:TextBox>
<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="pg" runat="server" />

<%--<script src="script/jtimepicker/jquery.min.js"></script>
<script src="script/jtimepicker/timepicki.js"></script>
<script>
    $('#timepicker1').timepicki();
    $('#txt_timein1').timepicki();
    $('#txt_timeout1').timepicki();
    $('#txt_timein2').timepicki();
    $('#txt_timeout2').timepicki();
</script>
<script src="script/jtimepicker/bootstrap.min.js"></script>--%>
</asp:Content>

<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
    <script src="vendors/moment/min/moment.min.js"></script>
    <script src="vendors/bootstrap-daterangepicker/daterangepicker.js"></script>
    <script src="vendors/bootstrap-datetimepicker/build/js/bootstrap-datetimepicker.min.js"></script>
    <script>
        $('.timepicker').datetimepicker({
            format: 'hh:mm'
        });
    </script>
</asp:Content>
