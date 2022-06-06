<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dtrlist.aspx.cs" Inherits="content_payroll_dtrlist" %>
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
    padding: 2px;
    border-bottom: 1px solid #e5e5e5;
        border-bottom-color: rgb(229, 229, 229);
}
.h4, h4 {
    font-size: 8px;
    font-weight:bold;
}
</style>
 

</head>
<body>
    <form id="form1" runat="server">
<div class="modal-dialog">
                            <div class="modal-content">
                               <div class="modal-header">
                                    <div style="float:left; width:100%;" >
                                        <asp:LinkButton ID="LinkButton2" class="close" runat="server" OnClick="close"><span aria-hidden="true" style=" font-size:18px; font-weight:bold;">×</span></asp:LinkButton>
                                     <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                      </button>
                                        <span style=" float:left; font-size:12px; color:Red; font-weight:bold; font-weight:bold;"><asp:Label ID="lbl_emp_name" runat="server" style=" float:left; font-size:12px; color:Red; font-weight:bold; font-weight:bold;" ></asp:Label></span>
                                        <span style=" float:left; font-size:12px;  color:Green; font-weight:bold;"> (<%=ds.Value%> - <%=de.Value %>)</span>
                                    </div>
                                </div>
                                <div class="modal-body">
                                <%--class="table table-striped table-bordered"--%>
                                <%--HeaderStyle-CssClass="GridViewScrollHeader" RowStyle-CssClass="GridViewScrollItem"--%>
                                <asp:GridView ID="gvDTR" runat="server" AutoGenerateColumns="false"    OnRowDataBound="OnRowDataBoundgrid2" style=" width:100%;"  >
                                    <Columns>
                                    
                                         <asp:BoundField  DataField="shiftcode" HeaderText="Date" />
                                         <asp:BoundField  DataField="date" HeaderText="Date" />
                                          <asp:TemplateField HeaderText="Time In">  
                                            <ItemTemplate>
                                               <asp:Label ID="lbl_date_in" Visible="false" Text='<%# Eval("datein1")%>'  runat="server" ></asp:Label>
                                                <asp:Label ID="lbl_rown" Visible="false" Text='<%# Eval("RowN")%>'  runat="server" ></asp:Label>

                                                           <asp:TextBox ID="txt_date_in"  onchange="testing(this)"  type="date" width="104px" runat="server"></asp:TextBox>
                                                           <asp:TextBox ID="txt_time_in" onchange="testing(this)" runat="server" Text='<%# Eval("timein1")%>' width="104px" type="time"  placeholder="Time"></asp:TextBox>

                                           </ItemTemplate>  
                                           </asp:TemplateField> 
                                           <asp:TemplateField HeaderText="Time Out">  
                                            <ItemTemplate>
                                             <asp:Label ID="lbl_date_out" Visible="false" Text='<%# Eval("dateout2")%>'  runat="server" ></asp:Label>
                      
                                             <asp:TextBox ID="txt_date_out" onchange="testing(this)" type="date"  runat="server"  width="104px"  placeholder="Date"></asp:TextBox>
                                             <asp:TextBox ID="txt_time_out" onchange="testing(this)" runat="server" type="time" width="104px" Text='<%# Eval("timeout2")%>' placeholder="Time"></asp:TextBox>
                                             <asp:CheckBox ID="chk_premium" runat="server" Text="W/Prem." />
                                             <asp:CheckBox ID="chk_rd" runat="server"  Text="RD" />
                                             <asp:Label ID="lbl_rd" Visible="false" Text='<%# Eval("restday")%>' runat="server" ></asp:Label>
                                             <asp:Label ID="lbl_premium" Visible="false" Text='<%# Eval("withpremium")%>' runat="server" ></asp:Label>
                                            </ItemTemplate>  
                                        </asp:TemplateField> 
                                    
                                        <asp:TemplateField HeaderText="Leave">  
                                             <ItemTemplate>
                                             <asp:Label ID="lbl_leave"  Text='<%# Eval("onleave")%>'  runat="server" Enabled="false" ></asp:Label>
                                             </ItemTemplate>  
                                        </asp:TemplateField> 
                                          <asp:TemplateField HeaderText="Absent">  
                                             <ItemTemplate>
                                             <asp:Label ID="lbl_absent"  Text='<%# Eval("absent")%>'  runat="server" Enabled="false" ></asp:Label>
                                             </ItemTemplate>  
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="Reg. Hrs">  
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_reghours"  Text='<%# Eval("regularhours")%>'  runat="server" Enabled="false" ></asp:Label>
                                            </ItemTemplate>  
                                        </asp:TemplateField> 
                                           <asp:TemplateField HeaderText="Offset Hrs">  
                                            <ItemTemplate>
                                            <asp:textbox ID="lbl_offset_hrs" onchange="testing(this)"  runat="server" Width="30px" Text='<%# Eval("totaloffsethrs")%>' AutoComplete="off" onclick="this.select();" onkeyup="decimalinput(this)"></asp:textbox>
                                            <asp:TextBox ID="txt_date_offset" style=" display:none;"  type="date" runat="server"></asp:TextBox>
                                      
                                            </ItemTemplate>  
                                        </asp:TemplateField> 
                                           <asp:TemplateField HeaderText="Night Hrs">  
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_nighthours" Text='<%# Eval("nighthours")%>'  runat="server" Enabled="false" ></asp:Label>
                                            </ItemTemplate>  
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="OT Hrs">  
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_reg_ot" runat="server" Text='<%# Eval("overtimehours")%>' Enabled="false" AutoComplete="off" onclick="this.select();" onkeyup="decimalinput(this)" Width="30px" ></asp:TextBox>
                                            </ItemTemplate>  
                                        </asp:TemplateField> 
                                            <asp:TemplateField HeaderText="OT Night Hrs">  
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_night_ot" runat="server" Text='<%# Eval("overtimenighthours")%>' Enabled="false" AutoComplete="off" onclick="this.select();" onkeyup="decimalinput(this)" Width="30px" ></asp:TextBox>
                                            </ItemTemplate>  
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="Late Hrs">  
                                            <ItemTemplate>
                                            <asp:Label ID="lbl_latehrs"  Text='<%# Eval("tardylatehours")%>'  runat="server"  Enabled="false"></asp:Label>
                                            </ItemTemplate>  
                                        </asp:TemplateField> 
                                           <asp:TemplateField HeaderText="UT Hrs">  
                                            <ItemTemplate>
                                            <asp:Label ID="lbl_uthrs"  Text='<%# Eval("tardyundertimehours")%>'   runat="server" Enabled="false"></asp:Label>
                                            </ItemTemplate>  
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderText="Remarks"  Visible="false">  
                                            <ItemTemplate>
                                             <asp:TextBox ID="txt_remarks" TextMode="MultiLine" Enabled="false" Text='<%# Eval("remarks")%>' Width="250px" Height="20px" runat="server"></asp:TextBox>
                                            </ItemTemplate>  
                                        </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">  
                                            <ItemTemplate>
                                              <asp:Label ID="lbl_status" runat="server" Text='<%# Eval("daystatus")%>'></asp:Label>
                                            </ItemTemplate>  
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="hohay"  Visible="false" >  
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" Width="10px"  OnClick="recompute"><i class="fa fa-repeat text-sm"></i></asp:LinkButton>
                                                <asp:LinkButton ID="LinkButton3" runat="server" Width="10px" OnClick="submit" Enabled="false" ForeColor="Gray"><i class="fa fa-save text-sm"></i></asp:LinkButton>
                                            </ItemTemplate>  
                                        </asp:TemplateField> 
                                        <asp:TemplateField ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">  
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("id")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <%-- <asp:BoundField  DataField="shiftcodeid"/>--%>
                                         <asp:TemplateField ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">  
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_shiftcodeid" runat="server" Text='<%# Eval("shiftcodeid")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                 </asp:GridView>
                              </div>
                           </div>
                        </div>
    <asp:HiddenField ID="dtrid" runat="server" />
    <asp:HiddenField ID="pgid" runat="server" />
    <asp:HiddenField ID="ds" runat="server" />
    <asp:HiddenField ID="de" runat="server" />
    <asp:HiddenField ID="hdn_selected_rowindex" runat="server" />
    <asp:HiddenField ID="hdn_row" runat="server" />
    <asp:HiddenField ID="hdn_row_after_saving" runat="server" />
    <asp:HiddenField ID="hdn_pg" runat="server" />
    <asp:HiddenField ID="hdn_ds" runat="server" />
    <asp:HiddenField ID="hdn_de" runat="server" />
    <asp:HiddenField ID="hdn_dtr_id" runat="server" />
    
    
    <asp:SqlDataSource ID="sql_income" runat="server" 
												 
												ProviderName="System.Data.SqlClient"  
												SelectCommand="select * from mshiftcode"></asp:SqlDataSource>
												
   <!-- ./wrapper -->
    <!-- jQuery 3 -->
    <script src="vendors/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap 3.3.7 -->
    <script src="vendors/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- AdminLTE App -->
    <script src="dist/js/adminlte.min.js"></script>
   


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