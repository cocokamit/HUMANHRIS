<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tk.aspx.cs" Inherits="content_hr_tk" MasterPageFile="~/content/MasterPageNew.master"%>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to approved this transaction?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    } 
</script>
<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
<script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
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
</asp:Content>

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>DTR MONITORING</h3>
    </div>   
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
        <div class="box-header">
                <div class="input-group input-group-sm">
                    <asp:DropDownList ID="ddl_dept" runat="server"></asp:DropDownList>
                    <asp:TextBox ID="txt_from" placeholder="From" CssClass="txt_f form-control" style="float:left; width:150px; margin-right:5px" ClientIDMode="Static" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txt_to" ClientIDMode="Static" CssClass="txt_f form-control" style="float:left; width:150px;margin-right:5px"  placeholder="To" runat="server" ></asp:TextBox>
                    <asp:Button ID="Button1" runat="server" OnClick="btn_go"  Text="search" CssClass="btn btn-primary"/>
                </div>
            </div>
            <asp:GridView ID="grid_item" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                
                    <asp:TemplateField HeaderText="No." HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"> 
                        <ItemTemplate>
                           <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="employee" HeaderText="Employee" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                    <asp:BoundField DataField="ShiftCode" HeaderText="Shift Code"/>
                    <asp:BoundField DataField="Date" HeaderText="Date" />
                    <asp:BoundField DataField="daytype" HeaderText="Day Type" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="dm" HeaderText="Day Multiplier" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="timein1" HeaderText="In 1" />
                    <asp:BoundField DataField="timeout1" HeaderText="Out 1" />
                    <asp:BoundField DataField="timein2" HeaderText="In 2" />
                    <asp:BoundField DataField="timeout2" HeaderText="Out 2" />
                    <asp:BoundField DataField="olw" HeaderText="leave Wholeday" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="aw" HeaderText="Absent Wholeday"  HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                    <asp:BoundField DataField="olh" HeaderText="On Leave Halfday" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="ah" HeaderText="Absent Halfday"  HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                    <asp:BoundField DataField="reg_hr" HeaderText="REGULAR HRS" />
                    <asp:BoundField DataField="night" HeaderText="NIGHT HRS" />
                    <asp:BoundField DataField="offsethrs" HeaderText="OFFSET HRS" />
                    <asp:BoundField DataField="ot" HeaderText="OT HRS" />
                    <asp:BoundField DataField="otn" HeaderText="OTN HRS" />
                    <asp:BoundField DataField="totalhrs" HeaderText="Total HRS" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="late" HeaderText="LATE HRS" />
                    <asp:BoundField DataField="ut" HeaderText="UT HRS" />
                    <asp:BoundField DataField="nethours" HeaderText="Net HRS" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                    <asp:BoundField DataField="shiftcodeid" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                       <asp:TemplateField HeaderText="STATUS" >
                        <ItemTemplate> 
                        </ItemTemplate>
                        <ItemStyle  Width="50px"/>
                    </asp:TemplateField>
                 
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>



<div class="hide"> 
<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>   
</div>


<asp:HiddenField ID="idd" runat="server" />

<asp:HiddenField ID="key" runat="server" />
</asp:Content>
