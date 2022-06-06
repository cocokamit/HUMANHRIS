<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ttos.aspx.cs" Inherits="content_hr_ttos" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .vp{margin:0 20px}
        .x_panel, .x_title {margin-bottom:0}
       .panel_toolbox {min-width:0}
       .dropdown-menu{min-width:10px}
       .vh{ visibility:hidden}
       .input-group-btn input[type=submit] {border:1px solid #efefef;border-left:none;  border-top-right-radius:40%; border-bottom-right-radius:40%}
       .table-bordered {margin-top:0}
    </style>
    <script src="script/auto/myJScript.js" type="text/javascript"></script>
    <script type="text/javascript" src="style/js/googleapis_jquery.min.js"></script>
<script src="style/js/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script type="text/javascript">
    var $jj = jQuery.noConflict();
    $jj(document).ready(function () {
        $jj("select").searchable({
            maxListSize: 10000, // if list size are less than maxListSize, show them all
            maxMultiMatch: 10000, // how many matching entries should be displayed
            exactMatch: false, // Exact matching on search
            wildcards: true, // Support for wildcard characters (*, ?)
            ignoreCase: true, // Ignore case sensitivity
            latency: 200, // how many millis to wait until starting search
            warnMultiMatch: 'top {0} matches ...',
            warnNoMatch: 'no matches ...',
            zIndex: 'auto'
        });
    });
    </script>
    <script type="text/javascript">
      
        function Confirm() {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure to cancel this transaction?"))
            { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
        } 
    </script>

    <!-- Datatables -->
    <link href="vendors/datatables.net-bs/css/dataTables.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-buttons-bs/css/buttons.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-fixedheader-bs/css/fixedHeader.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-responsive-bs/css/responsive.bootstrap.min.css" rel="stylesheet">
    <link href="vendors/datatables.net-scroller-bs/css/scroller.bootstrap.min.css" rel="stylesheet">
</asp:Content>

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="">
    <div class="page-title">
        <div class="title_left hd-tl">
            <h3>Annualization</h3>
        </div>
        <div class="title_right">
            <asp:Button ID="Button2" runat="server" Text="ADD" OnClick="processannualization"  CssClass="btn btn-primary" />
        </div>
     </div>  
        <div class="clearfix"></div>
        <div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head">
            <table>
                <tr>
                    <td style=" border:none; padding:5px;"><asp:DropDownList ID="ddl_year_search" runat="server"  Height="20px" ></asp:DropDownList></td>
                    <td style=" border:none;  padding:5px; display:none;"><asp:DropDownList ID="ddl_emp_search" runat="server"  Height="20px" ></asp:DropDownList></td>
<%--                    <td style=" border:none;  padding:5px;">
                        <asp:DropDownList ID="ddl_schedule" runat="server"  Height="20px" >
                                        <asp:ListItem>7.1</asp:ListItem>
                                        <asp:ListItem>7.3</asp:ListItem>
                                        <asp:ListItem>7.4</asp:ListItem>
                        </asp:DropDownList>
                    </td>--%>
                    <td style=" border:none;  padding:5px;">
                         <asp:DropDownList ID="ddl_company" runat="server"  Height="20px">
                         </asp:DropDownList>
                    </td>
                     <td style=" border:none;  padding:5px;">
                        <asp:DropDownList ID="ddl_class" runat="server"  Height="20px" >
                                        <asp:ListItem Value="1">No previous employer</asp:ListItem>
                                        <asp:ListItem Value="2">With Previous employer</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td><asp:Button ID="Button6" runat="server" Text="Process" OnClick="process" CssClass="btn btn-primary"/></td>
                   
                </tr>
            </table>
      <%--      <table>
            <tr>
             <td style=" padding:5px;">7.1 - Resigned</td>
                    <td style=" padding:5px; display:none;">7.2 - Exempted to withholding</td>
                    <td style=" padding:5px;">7.3 - No previous employer</td>
                    <td style=" padding:5px;">7.4 - With Previous employer</td>
                    <td style=" padding:5px; display:none;">7.5 - Minimum wages</td>
            </tr>
            </table>--%>
            </div>
            <asp:GridView ID="grid_alpha_trn" runat="server" AutoGenerateColumns="false"  CssClass="table table-striped table-bordered">
                                    <Columns>
                                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                        <asp:BoundField DataField="empid" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                        <asp:BoundField DataField="emp_name" HeaderText="Employee Name"/>
                                        <asp:BoundField DataField="yr" HeaderText="Year"/>
                                            <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                    <asp:LinkButton ID="pdf" runat="server" CausesValidation="false" OnClick="PDF" Text="PDF"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                            </asp:GridView>
        </div>
    </div>
</div>
</div>
<div id="ttospo" visible="false" runat="server" class="Overlay"></div>
<div id="ttosppp" runat="server" visible="false" class="PopUpPanel pop-a">
<asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" OnClick="close"  runat="server"/>
<div class="row">
    <div class="col-md-3">
       <div class="x_panel">
       <ul class="input-form">
            <li><asp:RadioButtonList ID="txt_pressentorprev" runat="server" OnTextChanged="click_class" AutoPostBack="true">
            <asp:ListItem Value="1">No previous employer</asp:ListItem>
            <asp:ListItem Value="2">With Previous employer</asp:ListItem>
            </asp:RadioButtonList></li>
            <li>Year</li>
            <li><asp:DropDownList ID="ddl_year" runat="server" Width="200px" Height="20px"></asp:DropDownList></li>
            <li>Employee List</li>
            <li><asp:DropDownList ID="ddl_emplist" runat="server" Width="200px" Height="20px"></asp:DropDownList></li>
            <li><br /></li>
            <li>
            <asp:Button ID="Button1" runat="server" Text="Verify" OnClick="processsss" CssClass="btn btn-primary"/>
            <%if (grid_view.Rows.Count > 0)
              { %>
            <asp:Button ID="Button4" runat="server" Text="Process" OnClick="process_annualization" CssClass="btn btn-primary"/>
            <%} %>
            </li>
            <li><asp:Label ID="lbl_error_msg" CssClass="text-danger" ForeColor="Red" runat="server"></asp:Label></li>
        </ul>
                  
    </div>
    </div>
    <div class="col-md-9">
    <div class="x_panel">
    <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" OnRowDataBound="datarowbound" CssClass="table table-striped table-bordered">
                                    <Columns>
                                        <asp:BoundField DataField="empid" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                        <asp:BoundField DataField="rdo_code" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                        <asp:BoundField DataField="DateResigned" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                        <asp:BoundField DataField="civilstatus" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                        <asp:BoundField DataField="action" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                        <asp:BoundField DataField="dateofbirth" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                        <asp:BoundField DataField="present_comp_company" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                        <asp:BoundField DataField="present_comp_add" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                        <asp:BoundField DataField="present_comp_tin" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                        <asp:BoundField DataField="autorized_agent_for_BIR"  ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                        <asp:TemplateField HeaderText="Employee Information">
                                            <ItemTemplate>
                                                <ul class="input-form">
                                                    <li><asp:Label ID="lbl_empname" style=" font-size:16px; font-weight:bold; color:Blue;" runat="server" Text='<%# Eval("empname") %>'></asp:Label> </li>
                                                    <li>(<asp:Label ID="lbl_position" runat="server" Text='<%# Eval("position") %>'></asp:Label>)</li>
                                                    <li style="font-weight:bold;">T.I.N:</li>
                                                    <li><asp:Label ID="lbl_tin" runat="server" Text='<%# Eval("tin") %>' ></asp:Label></li>
                                                    <li style="font-weight:bold;">Date Hired:</li>
                                                    <li><asp:Label ID="lbl_dh" runat="server" Text='<%# Eval("datehired") %>'></asp:Label></li>
                                                    <% if (txt_pressentorprev.SelectedValue == "2")
                                                    {%>
                                                    <li style="font-weight:bold;">Previous Employer:</li>
                                                    <li><asp:textbox ID="lbl_prevemp" runat="server" ></asp:textbox></li>
                                                    <li style="font-weight:bold;">Previous Employer T.I.N:</li>
                                                    <li><asp:textbox ID="lbl_preveemptin" runat="server" onkeyup="intinput(this)" ></asp:textbox></li>
                                                    <li style="font-weight:bold;">Previous Employer Address:</li>
                                                    <li><asp:textbox ID="txt_prev_address" runat="server"  ></asp:textbox></li>
                                                    <%} %>
                                                    <li style="font-weight:bold;">Gross Income:</li>
                                                    <li><asp:Label ID="lbl_gi" runat="server" Text='<%# Eval("gross_income") %>'></asp:Label></li>
                                                </ul>
                                            </ItemTemplate>
                                       </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Non - Taxable">
                                            <ItemTemplate>
                                             <ul class="input-form"    >
                                                 <li>13th Month/Others :</li>
                                                 <li><asp:Label ID="lbl_nt_thirteenmonth" runat="server" Text='<%# Eval("thirteenmonthnontax") %>'></asp:Label></li>
                                                 <li>SSS/HDMF/PHIC/Dues:</li>
                                                 <li><asp:Label ID="lbl_nt_govtdeduction" runat="server" Text='<%# Eval("govt_union") %>'></asp:Label></li>
                                                 <li>SALARIES & FORMS:</li>
                                                 <li><asp:Label ID="lbl_nt_salariesandforms" runat="server" Text='<%# Eval("non_taxable_allowance") %>'></asp:Label></li>
                                                 <li>COMPENSITION:</li>
                                                 <li><asp:Label ID="lbl_nt_compensition" runat="server" Text='<%# Eval("nontax_compensation") %>'></asp:Label></li>
                                             </ul>
                                            </ItemTemplate>
                                       </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Taxable">
                                            <ItemTemplate>
                                             <ul class="input-form" >
                                                 <li>Basic Salary :</li>
                                                 <li><asp:Label ID="lbl_t_bs" runat="server" Text='<%# Eval("BasicPay") %>'></asp:Label></li>
                                                 <li>13th Month/Others :</li>
                                                 <li><asp:Label ID="lbl_t_thirteenmonth" runat="server" Text='<%# Eval("thirteenmonthtaxable") %>'></asp:Label></li>
                                                 <li>SALARIES & FORMS:</li>
                                                 <li><asp:Label ID="lbl_t_salariesandforms" runat="server" Text='<%# Eval("taxable_allowance") %>'></asp:Label></li>
                                                 <li>COMPENSITION:</li>
                                                 <li><asp:Label ID="lbl_t_compensition" runat="server" Text='<%# Eval("taxable_compensation") %>'></asp:Label></li>
                                             </ul>
                                           </ItemTemplate>
                                       </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Summary">
                                            <ItemTemplate>
                                             <ul class="input-form"  >
                                                 <li>Present Taxable Compensation Income:</li>
                                                 <li><asp:Label ID="lbl_summary_gci" runat="server" Text='<%# Eval("taxable_compensation") %>'></asp:Label></li>
                                                 <% if (txt_pressentorprev.SelectedValue == "2")
                                                    {%>
                                                 <li>Previous Employer Taxable Compensation Income:</li>
                                                 <li><asp:textbox ID="lbl_prev_emptci" runat="server" onkeyup="decimalinput(this)"></asp:textbox></li>
                                                    <%} %>
                                                 <li>Premium/health:</li>
                                                 <li><asp:textbox ID="lbl_premiumhealth" runat="server"  onkeyup="decimalinput(this)" Text="0.00"></asp:textbox></li>
                                                 <li>Present Employeer Tax Withheld</li>
                                                 <li><asp:Label ID="lbl_present_taxwithheld" runat="server" Text='<%# Eval("tax_due") %>'></asp:Label></li>
                                                 <% if (txt_pressentorprev.SelectedValue == "2")
                                                    {%>
                                                 <li>Previous Employeer Tax Withheld</li>
                                                 <li><asp:Textbox ID="lbl_prev_taxwithheld" onkeyup="decimalinput(this)" runat="server"></asp:Textbox></li>
                                                 <%} %>
                                             </ul>
            </ItemTemplate>
         </asp:TemplateField>
         <asp:BoundField DataField="companyid" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
         <asp:BoundField DataField="taxcodeidf" />
         <asp:BoundField DataField="taxcodef" />
         <asp:BoundField DataField="tax_wh_jan_nov" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
      </Columns>
   </asp:GridView>
</div>
</div>
</div>


</div>

<asp:HiddenField ID="TextBox1" runat="server" />
<asp:HiddenField ID="trn_det_id" runat="server" />
<asp:HiddenField ID="hf_view" runat="server" />
<asp:HiddenField ID="hf_id" runat="server" />
</asp:Content>