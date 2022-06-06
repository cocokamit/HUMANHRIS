<%@ Page Title="" Language="C#" MasterPageFile="~/content/MasterPageNew.master" AutoEventWireup="true" CodeFile="attinout.aspx.cs" Inherits="content_hr_attinout" %>

<asp:Content ID="head" ContentPlaceHolderID="head" Runat="Server">
<!-- bootstrap datepicker -->
<link href="vendors/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css" rel="stylesheet" type="text/css" />

<!--SELECT-->
<link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="content" Runat="Server">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Attendance</h3>
    </div>  
</div>
<div class="clearfix"></div>
<div class="row">
   <div class="col-md-3">
        <div class="x_panel">
             <div class="form-group">
                <label>Date From</label>
                <asp:TextBox ID="tb_from" runat="server" CssClass="datepicker form-control"  autocomplete="off"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Date To</label>
                <asp:TextBox ID="tb_to" runat="server" CssClass="datepicker form-control" autocomplete="off"  ></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Department</label>
                <asp:DropDownList ID="ddl_dept" ClientIDMode="Static" CssClass="form-control select2" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="OnChangeDept" >
                </asp:DropDownList>
                <div class="clearfix"></div>
            </div>
            <div id="pnlSection" runat="server" class="form-group">
                <label>Section</label>
                <asp:DropDownList ID="ddl_section" ClientIDMode="Static" CssClass="form-control select2" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="OnChangeSection">
                </asp:DropDownList>
                <div class="clearfix"></div>
            </div>
            <hr style=" margin:10px 0" />
            <asp:Button D="b_go" runat="server" OnClick="clickSearch" Text="Search" CssClass="btn btn-primary "/>
        </div>
    </div>
    <div class="col-md-9">
        <div class="x_panel ">
            <div id="alert" runat="server" class="alert alert-empty" style="margin:0 !important">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
            </div>
            <asp:GridView ID="grid_item" runat="server" ClientIDMode="Static" BorderColor="Turquoise" OnRowDataBound="rowbound" AutoGenerateColumns="False" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:TemplateField HeaderText="No." HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"> 
                        <ItemTemplate>
                           <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="employee" HeaderText="Employee" />
                    <asp:BoundField DataField="date" HeaderText="Date" HeaderStyle-Width="80"/>
                    <asp:BoundField DataField="ShiftCode" HeaderText="ShiftCode" />
                    <asp:BoundField DataField="daytype" HeaderText="Day Type" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="dm" HeaderText="Day Multiplier" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <asp:BoundField DataField="timein1" HeaderText="LOG IN" />
                    <asp:BoundField DataField="timeout1" HeaderText="BREAK IN"  HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"  />
                    <asp:BoundField DataField="timein2" HeaderText="BREAK OUT"  HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                    <asp:BoundField DataField="timeout2" HeaderText="LOG OUT" />
                    <asp:BoundField DataField="olw" HeaderText="LEAVE"/>
                    <asp:BoundField DataField="setupfinalout" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
</asp:Content>

<asp:Content ID="footer" ContentPlaceHolderID="footer" Runat="Server">
<!-- bootstrap datepicker -->
<script type="text/javascript" src="vendors/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js"></script>
<script>
    $(function () {
        //Date picker
        $('.datepicker').datepicker({
            autoclose: true
        })
    })
</script>

<!--Select-->
<script type="text/javascript" src="vendors/select2/dist/js/select2.full.min.js"></script>
<script type="text/javascript">
    (function ($) {
        $('.select2').select2()
    })(jQuery);
</script>

</asp:Content>

