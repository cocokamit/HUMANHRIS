<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dtrmanagement.aspx.cs" Inherits="content_hr_dtrmanagement" MasterPageFile="~/content/MasterPageNew.master"%>


<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<style type="text/css">
     hr{margin:5px 0 10px}
     input[type=text]{padding:6.5px; float:left;margin-right:5px}
</style>
<script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to cancel this transaction?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    } 
</script>
<link href="script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
    <script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>
    <script src="script/auto/myJScript.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $.noConflict();
        $(".auto").autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "content/hr/dtrmanagement.aspx/GetEmployee",
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

</asp:Content>

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<section class="content-header">
<div class="page-title">
    <div class="title_left pull-left">
        <h3>Daily Time Record</h3>
    </div>   
    <div class="title_right">
       <ul>
        <li><a href="adddtrlogs?user_id=<% Response.Write(Session["user_id"]); %>"><i class="fa fa-clipboard"></i> DTR</a></li>
        <li><i class="fa fa-angle-right"></i></li>
        <li>Daily Time Record</li>
       </ul>
    </div>
</div>
</section>
<section class="content"> 
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head">
                <asp:DropDownList ID="ddl_pg" runat="server" CssClass="minimal"  style="padding:7.5px"></asp:DropDownList>
                <asp:TextBox ID="txt_emp" runat="server" placeholder="Employee Name" CssClass="auto"></asp:TextBox>
                <asp:TextBox ID="txt_f" CssClass="datee" runat="server" placeholder="From"></asp:TextBox>
                <asp:TextBox ID="txt_t" CssClass="datee" runat="server" placeholder="To"></asp:TextBox>
                <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Export to Excel"  OnClick = "generatereport"  CssClass="right add"><i class="fa fa-file-excel-o"></i></asp:LinkButton>
                <asp:Button ID="Button3"  runat="server" OnClick="search"  Text="Search" CssClass="btn btn-primary" />
             </div>
            <asp:Label ID="lbl_err" runat="server" ForeColor="Red" Font-Size="13px"></asp:Label>
            <asp:GridView ID="griddtr" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="IdNumber" HeaderText="ID Number"/>
                    <asp:BoundField DataField="FullName" HeaderText="Employee Name"/>
                    <asp:BoundField DataField="Department" HeaderText="Department"/>
                    <asp:BoundField DataField="Biotime" HeaderText="BIO IN and OUT"/>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
 </section>
 <div class="hide"> 
  <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox> 
</div>
</asp:Content>
