<%@ Page Language="C#" AutoEventWireup="true" CodeFile="payrollsummary.aspx.cs" Inherits="content_payroll_payrollsummary" %>
<%@ Import Namespace="System.Data" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title></title>
<style type="text/css">
.div{width:100%; height:auto; border:none; float:left; padding:1px; }
*{ font-size:12PX; font-style:normal; font-weight:normal;}
.none { display:none}
.hiddencol { display: none; }
</style>
<script src="script/auto/myJScript.js" type="text/javascript"></script>
<%--<link rel="stylesheet" type="text/css" href="../../style/fixedheader/component.css" />--%>
<%--<link rel="stylesheet" href="../../dist/plugins/input-mask/jquery.inputmask.js"/>
<link rel="stylesheet" href="../../dist/jquery/dist/jquery.min.js"/>--%>
<link rel="stylesheet" href="../../vendors/bootstrap/dist/css/bootstrap.min.css"/>

<link rel="stylesheet" href="../../vendors/font-awesome/css/font-awesome.min.css">
<link rel="stylesheet" href="../../dist/css/base.css">
<link rel="stylesheet" href="../../dist/css/custom.css" />
<link rel="stylesheet" href="../../dist/css/skins/_all-skins.min.css">
<link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic">
<style type="text/css">
.sticky-wrap .sticky-intersect th { background-color:#437cab}
.sticky-intersect th , .sticky-col tbody tr th {border:1px solid #fff !important} 
.modal { position:fixed;}
.modal-dialog {
    width: 100%;
    margin: 0px auto;
}
.modal-header {
    padding: 5px;
    border-bottom: 1px solid #e5e5e5;
        border-bottom-color: rgb(229, 229, 229);
}
.h4, h4 {
    font-size: 8px;
    font-weight:bold;
}
hr {
    margin-top: 5px;
    margin-bottom: 5px;
    border: 0;
        border-top-color: currentcolor;
        border-top-style: none;
        border-top-width: 0px;
    border-top: 1px solid #eee;
}
</style>
    <link href="../../style/fixheadergrid/css/web.css" rel="stylesheet" />
    <script type="text/javascript" src="style/fixheadergrid/js/gridviewscroll.js"></script>
    <script type="text/javascript">
        var gridViewScroll = null;
        console.log(window.innerWidth);
        window.onload = function () {
            gridViewScroll = new GridViewScroll({
                elementID: "grid_view",
                width: window.innerWidth,
                height: 500,
                freezeColumn: true,
               // freezeFooter: true,
                freezeColumnCssClass: "GridViewScrollItemFreeze",
                freezeFooterCssClass: "GridViewScrollFooterFreeze",
                freezeHeaderRowCount: 2,
                freezeColumnCount: 2,
                onscroll: function (scrollTop, scrollLeft) {
                    console.log(scrollTop + " - " + scrollLeft);
                }
            });
            gridViewScroll.enhance();

            //redirect exact row
            var id = document.getElementById("hdn_selected_id").value;
            var fid = '#' + id;
            $('.test').attr('href', fid);
            window.location.href = $('.test').attr('href');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <h5>DTR Summary</h5>
    <hr />
    <asp:TextBox ID="txt_search" runat="server" style=" display:none;"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" OnClick="search" Text="Search" style=" display:none;"/><%--OnClick="search"--%>
    <a id="click" class="test"></a>
    <%--class="table table-striped table-bordered"--%>
    <asp:GridView ID="grid_view" runat="server"  HeaderStyle-CssClass="GridViewScrollHeader" RowStyle-CssClass="GridViewScrollItem" OnRowDataBound="OnRowDataBoundgrid1" style=" width:100%"   AutoGenerateColumns="false"  >
            <Columns>
                 <asp:TemplateField>
                          <ItemTemplate>
                            <a <%# Eval("empid")%>&b=<%# Eval("dtrid")%>&ds=<%# Eval("ds")%>&de=<%# Eval("de")%>"  class="fa fa-list-ul text-sm"></a>
                          </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:BoundField DataField="IdNumber" HeaderText="ID Number"/>
                        <asp:TemplateField HeaderText="Employee">
                          <ItemTemplate>
                            <a id="<%# Eval("empid")%>"><%# Eval("e_name")%></a>
                          </ItemTemplate>
                        </asp:TemplateField>
                       <%--<asp:BoundField DataField="e_name" HeaderText="Employee" ItemStyle-Width="10px"/>--%>
                       <%--<asp:BoundField DataField="total_reg_render" HeaderText="TEST"/>--%>


                        <asp:BoundField DataField="total_reg_render" HeaderText="Reg. hrs"/>
                        <asp:BoundField DataField="Absent_Day" HeaderText="Absent Day" />
                        <asp:BoundField DataField="Late_Hours" HeaderText="Late hrs" />
                        <asp:BoundField DataField="Undertime_Hours" HeaderText="UT hrs"/>
                        <asp:BoundField DataField="LWOP_Hours" HeaderText="LWOP hrs"/>
                        <asp:BoundField DataField="LWP_Hours" HeaderText="LWP hrs"/>
                        <asp:BoundField DataField="Regular_HRS" HeaderText="Reg. hrs"/>
                        <asp:BoundField DataField="Night_HRS" HeaderText="Night hrs"/>
                        <asp:BoundField DataField="Regular_OT_HRS" HeaderText="Reg. OT hrs"/>
                        <asp:BoundField DataField="Night_OT_HRS" HeaderText="Night OT hrs"/>
                        <asp:BoundField DataField="RD_HRS" HeaderText="RD hrs"/>
                        <asp:BoundField DataField="RD_Night_HRS" HeaderText="RD Night hrs"/>
                        <asp:BoundField DataField="RD_Regular_OT_HRS" HeaderText="RD OT hrs"/>
                        <asp:BoundField DataField="RD_Night_OT_HRS" HeaderText="RD Night OT hrs"/>
                        <asp:BoundField DataField="SH_HRS" HeaderText="SH hrs"/>
                        <asp:BoundField DataField="SH_Night_HRS" HeaderText="SH Night hrs"/>
                        <asp:BoundField DataField="SH_OT_HRS" HeaderText="SH OT hrs"/>
                        <asp:BoundField DataField="SH_Night_OT_HRS" HeaderText="SH Night OT hrs"/>
                        <asp:BoundField DataField="SH_RD_HRS" HeaderText="SH RD hrs"/>
                        <asp:BoundField DataField="SH_RD_Night_HRS" HeaderText="SH RD Night hrs"/>
                        <asp:BoundField DataField="SH_RD_OT_HRS" HeaderText="SH RD OT hrs"/>
                        <asp:BoundField DataField="SH_RD_Night_OT_HRS" HeaderText="SH RD Night OT hrs"/>
                        <asp:BoundField DataField="Legal_HRS" HeaderText="Legal hrs"/>
                        <asp:BoundField DataField="Legal_Night_HRS" HeaderText="Legal Night hrs"/>
                        <asp:BoundField DataField="Legal_OT_HRS" HeaderText="Legal OT hrs"/>
                        <asp:BoundField DataField="Legal_Night_OT_HRS" HeaderText="Legal Night OT hrs"/>
                        <asp:BoundField DataField="Legal_RD_HRS" HeaderText="Legal RD hrs"/>
                        <asp:BoundField DataField="Legal_RD_Night_HRS" HeaderText="Legal RD Night hrs"/>
                        <asp:BoundField DataField="Legal_RD_OT_HRS" HeaderText="Legal-RD-OT hrs"/>
                        <asp:BoundField DataField="Legal_RD_Night_OT_HRS" HeaderText="Legal RD Night OT hrs"/>
                        <asp:BoundField DataField="Double_HRS" HeaderText="Double hrs"/>
                        <asp:BoundField DataField="Double_Night_HRS" HeaderText="Double Night hrs"/>
                        <asp:BoundField DataField="Double_OT_HRS" HeaderText="Double OT hrs"/>
                        <asp:BoundField DataField="Double_Night_OT_HRS" HeaderText="Double Night OT hrs"/>
                        <asp:BoundField DataField="Double_RD_HRS" HeaderText="Double RD hrs"/>
                        <asp:BoundField DataField="Double_RD_Night_HRS" HeaderText="Double RD Night hrs"/>
                        <asp:BoundField DataField="Double_OT_HRS" HeaderText="Double OT hrs"/>
                        <asp:BoundField DataField="Double_RD_Night_OT_HRS" HeaderText="Double RD Night OT hrs"/>
                        <asp:BoundField DataField="otmealallwance" HeaderText="OT Meal"/>
                        <asp:BoundField DataField="offsetHRS" HeaderText="Offset hrs" Visible="false"/>
                        <asp:BoundField DataField="empid" Visible="false" />
                        <asp:BoundField DataField="departmentid" Visible="false"/>
                        <asp:BoundField DataField="row" Visible="false" />
                </Columns>
    </asp:GridView>
    <hr />
     <asp:Button ID="btnExport" runat="server" Text="Export To Excel"  CssClass="btn btn-primary" OnClick = "ExportToExcel"  />
    <asp:HiddenField ID="dtrid" runat="server" />
    <asp:HiddenField ID="pgid" runat="server" />
    <asp:HiddenField ID="ds" runat="server" />
    <asp:HiddenField ID="de" runat="server" />
    <asp:HiddenField ID="hdn_selected_rowindex" runat="server" />
    <asp:HiddenField ID="hdn_selected_id" runat="server" />
   <!-- ./wrapper -->
    <!-- jQuery 3 -->
    <script src="vendors/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap 3.3.7 -->
    <script src="vendors/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- AdminLTE App -->
    <script src="dist/js/adminlte.min.js"></script>
   
<script type="text/javascript" src="style/googleApi/nestedgrid.js"></script>
<script type="text/javascript">
    $("[src*=plus]").live("click", function () {
        $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
        $(this).attr("src", "images/minus.png");
    });
    $("[src*=minus]").live("click", function () {
        $(this).attr("src", "images/plus.png");
        $(this).closest("tr").next().remove();
    });
</script>


<script src="dist/bootstrap/dist/js/bootstrap.min.js"></script>
<script src="dist/jquery/dist/jquery.min.js"></script>
<script src="dist/plugins/input-mask/jquery.inputmask.js"></script>
<script src="dist/plugins/input-mask/jquery.inputmask.date.extensions.js"></script>
<script src="dist/plugins/input-mask/jquery.inputmask.extensions.js"></script>
<script type="text/javascript" src="vendors/select2/dist/js/select2.full.min.js"></script>

<!-- Page script -->
<script>
    $(function () {
        //Initialize Select2 Elements
        
        $('.select2').select2()

        //Datemask dd/mm/yyyy
        $('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' })
        //Datemask2 mm/dd/yyyy
        $('#datemask2').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' })
        alert('test');
        //Money Euro
        $('[data-mask]').inputmask()

        //Date range picker
        $('#reservation').daterangepicker()
        //Date range picker with time picker
        $('#reservationtime').daterangepicker({ timePicker: true, timePickerIncrement: 30, format: 'MM/DD/YYYY h:mm A' })
        //Date range as a button
        $('#daterange-btn').daterangepicker(
      {
          ranges: {
              'Today': [moment(), moment()],
              'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
              'Last 7 Days': [moment().subtract(6, 'days'), moment()],
              'Last 30 Days': [moment().subtract(29, 'days'), moment()],
              'This Month': [moment().startOf('month'), moment().endOf('month')],
              'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
          },
          startDate: moment().subtract(29, 'days'),
          endDate: moment()
      },
      function (start, end) {
          $('#daterange-btn span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'))
      }
    )

        //Date picker
        $('#datepicker').datepicker({
            autoclose: true
        })

        //iCheck for checkbox and radio inputs
        $('input[type="checkbox"].minimal, input[type="radio"].minimal').iCheck({
            checkboxClass: 'icheckbox_minimal-blue',
            radioClass: 'iradio_minimal-blue'
        })
        //Red color scheme for iCheck
        $('input[type="checkbox"].minimal-red, input[type="radio"].minimal-red').iCheck({
            checkboxClass: 'icheckbox_minimal-red',
            radioClass: 'iradio_minimal-red'
        })
        //Flat red color scheme for iCheck
        $('input[type="checkbox"].flat-red, input[type="radio"].flat-red').iCheck({
            checkboxClass: 'icheckbox_flat-green',
            radioClass: 'iradio_flat-green'
        })

        //Colorpicker
        $('.my-colorpicker1').colorpicker()
        //color picker with addon
        $('.my-colorpicker2').colorpicker()

        //Timepicker
        $('.timepicker').timepicker({
       
            showInputs: false
        })
    })
</script>
    </form>
</body>
</html>