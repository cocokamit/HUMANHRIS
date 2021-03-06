<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WithoutApprovers.aspx.cs" EnableEventValidation="false" Inherits="content_report_WithoutApprovers" MasterPageFile="~/content/MasterPageNew.master"%>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_attReport">
<style type="text/css">
.GridViewStyle{font-family:Verdana;font-size:12px;background-color: White; text-align:center;}
.GridViewHeaderStyle{font-family:Verdana;font-size:12px; text-align:center; padding : 4px 2px;color: #fff;background: #333 url(Images/grid-header.png) repeat-x top;border-left: solid 1px #525252;font-size: 0.9em;text-align: center;text-transform: uppercase;font-weight: bold; }
.rdl tr { float:left; padding:4px;}
</style>

<link href="script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
<script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>

<script language="javascript" type="text/javascript">

    function CreateGridHeader(DataDiv, content_grid_view, HeaderDiv) {

        var DataDivObj = document.getElementById(DataDiv);
        var DataGridObj = document.getElementById(content_grid_view);
        var HeaderDivObj = document.getElementById(HeaderDiv);

        //********* Creating new table which contains the header row ***********
        var HeadertableObj = HeaderDivObj.appendChild(document.createElement('table'));

        DataDivObj.style.paddingTop = '0px';
        var DataDivWidth = DataDivObj.clientWidth;
        DataDivObj.style.width = '5000px';

        //********** Setting the style of Header Div as per the Data Div ************
        HeaderDivObj.className = DataDivObj.className;
        HeaderDivObj.style.cssText = DataDivObj.style.cssText;
        //**** Making the Header Div scrollable. *****
        HeaderDivObj.style.overflow = 'auto';
        //*** Hiding the horizontal scroll bar of Header Div ****
        HeaderDivObj.style.overflowX = 'hidden';
        //**** Hiding the vertical scroll bar of Header Div **** 
        HeaderDivObj.style.overflowY = 'hidden';
        HeaderDivObj.style.height = '30px'; //DataGridObj.rows[0].clientHeight + 'px';
        HeaderDivObj.style.margintop = '30px';

        //**** Removing any border between Header Div and Data Div ****
        HeaderDivObj.style.borderBottomWidth = '0px';

        //********** Setting the style of Header Table as per the GridView ************
        HeadertableObj.className = DataGridObj.className;
        //**** Setting the Headertable css text as per the GridView css text 
        HeadertableObj.style.cssText = DataGridObj.style.cssText;
        HeadertableObj.border = '1px';
        HeadertableObj.rules = 'all';
        HeadertableObj.cellPadding = DataGridObj.cellPadding;
        HeadertableObj.cellSpacing = DataGridObj.cellSpacing;

        //********** Creating the new header row **********
        var Row = HeadertableObj.insertRow(0);
        Row.className = DataGridObj.rows[0].className;
        Row.style.cssText = DataGridObj.rows[0].style.cssText;
        Row.style.fontWeight = '8px';


        //******** This loop will create each header cell *********
        for (var iCntr = 0; iCntr < DataGridObj.rows[0].cells.length; iCntr++) {
            var spanTag = Row.appendChild(document.createElement('td'));
            spanTag.innerHTML = DataGridObj.rows[0].cells[iCntr].innerHTML;
            var width = 0;
            //****** Setting the width of Header Cell **********
            if (spanTag.clientWidth > DataGridObj.rows[1].cells[iCntr].clientWidth) {
                width = spanTag.clientWidth;
            }
            else {
                width = DataGridObj.rows[1].cells[iCntr].clientWidth;
            }
            if (iCntr <= DataGridObj.rows[0].cells.length - 2) {
                spanTag.style.width = width + 'px';
            }
            else {
                spanTag.style.width = width + 20 + 'px';
            }
            DataGridObj.rows[1].cells[iCntr].style.width = width + 'px';
        }
        var tableWidth = DataGridObj.clientWidth;
        //********* Hidding the original header of GridView *******
        DataGridObj.rows[0].style.display = 'none';
        //********* Setting the same width of all the componets **********
        HeaderDivObj.style.width = DataDivWidth + 'px';
        DataDivObj.style.width = DataDivWidth + 'px';
        DataGridObj.style.width = tableWidth + 'px';
        HeadertableObj.style.width = tableWidth + 20 + 'px';
        return false;
    }

    function Onscrollfnction() {
        var div = document.getElementById('DataDiv');
        var div2 = document.getElementById('HeaderDiv');
        //****** Scrolling HeaderDiv along with DataDiv ******
        div2.scrollLeft = div.scrollLeft;
        return false;
    }
    
</script>
<div class="page-title">
    <div class="title_left">
        <h3>Approvers Check Report</h3>
    </div>
</div>
<div class="title_right">
    <div class="col-md-5 col-sm-5 col-xs-12 form-group pull-right top_search">
        <div class="input-group">
            <div class="x-head"></div>
        </div>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12  col-sm-12 col-xs-12">
    <div class="x_panel">
        <asp:TextBox ID="ddl_emp" placeholder="Employee Name" Width="250px" Height="33px" CssClass="auto" runat="server"></asp:TextBox>   
        <asp:Button ID="btn_sear" runat="server" Text="Search" OnClick="btn_search" CssClass="btn btn-primary" />
        <div id="exp_report" runat="server" visible="false">
            <asp:LinkButton ID="Button3" runat="server" ToolTip="EXCEL" OnClick="ExportToExcel"  CssClass="right add"><i class="fa fa-file-excel-o"></i></asp:LinkButton>
            <asp:Label ID="lbl_filter_info" runat="server" style=" font-size:9px; font-weight:bold;"></asp:Label>
        </div>
        <div id="HeaderDiv"></div>
        <asp:GridView ID="grid_view" runat="server"  AutoGenerateColumns="False" CssClass="table table-striped table-bordered no-margin">
            <Columns>
                <asp:BoundField DataField="Id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                <asp:BoundField DataField="emp_id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                <asp:BoundField DataField="FullName" HeaderText="Employee Name" />
                <asp:BoundField DataField="Department" HeaderText="Department"/>
                <asp:BoundField DataField="Position" HeaderText="Position"/>
                <asp:BoundField DataField="PayrollGroup" HeaderText="PayrollGroup"/>
                <asp:TemplateField HeaderText="Add Approver" >
                  <ItemTemplate>
                    <asp:LinkButton ID="lnk_view" CommandName='<%# Eval("Id") %>' ForeColor="Red" data-toggle="modal" data-target="#modal-reportto" runat="server"><i class="fa fa-edit"></i></asp:LinkButton>
                  </ItemTemplate>
                <ItemStyle  Width="110px"/>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    </div>
</div>

<div class="modal fade" id="modal-reportto" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title">Set Approver</h4>
        </div>
        <div class="modal-body">
            <div class="form-group">
                <label>Employee</label>
                <asp:TextBox ID="txt_reportto" runat="server" ClientIDMode="Static" CssClass="form-control auto"></asp:TextBox>
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button ID="Button6" runat="server" OnClick="addreportrt" Text="ADD" CssClass="btn btn-primary" />
        </div>
    </div>
    </div>
</div>

<asp:HiddenField ID="lbl_bals" ClientIDMode="Static" runat="server" />
<script type="text/javascript">
    $(document).ready(function () {
        $.noConflict();
        $(".auto").autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "content/report/leave_credit_report.aspx/GetEmployee",
                    data: "{'term':'" + $(".auto").val() + "'}",
                    dataType: "json",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                label: item.split('-')[1],
                                val: item.split('-')[0]
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
</asp:Content>
