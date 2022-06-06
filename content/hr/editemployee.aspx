<%@ Page Language="C#" AutoEventWireup="true" CodeFile="editemployee.aspx.cs" Inherits="content_hr_editemployee" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        li { list-style:none}
        h5{ margin:15px 0 0; font-weight:bold; color: #0e7ea8}
        hr { margin:8px 0 0}
        .alert { margin: 85px auto 5px;}
        .col-xs-3 { min-width:220px}
        .title_right {text-align:right;}
        .emp-identity { margin-bottom:30px}
        .emp-identity1 input[type=text]{padding:8px; width:100%; margin-bottom:10px}
        .emp-identity1 select {padding:7px; width:100%;margin-bottom:10px}
        .emp-identity1 input[type=checkbox] {margin-right:5px !important;}
        .bei{margin-left:25px}
        .hiddencol { display: none; }
        .jobhistory input[type=text]{padding:7px;  float:left; margin-right:5px}
        .jobhistory input[type=submit]{ float:left; margin-top:-5px}
        .jobhistory select {float:left;padding:5.5px; margin-right:5px}
        .skills input[type=text] {border:1px solid #bab7b7}
        .emp-hd {background:#2093be; margin:5px 0 10px}
        .emp-hd img { float:left}
        .emp-hd span { margin-left:10px; color:#fff}
        .emp-hd h1 { font-size:30px; margin-top:15px}
        .img-circle {border: 1px solid #E6E9ED;padding: 2px; width:55px; margin:10px 0}
        .input-form label{padding:0 5px }
        .edit {padding-top:7px}
        .edit input[type=text] { padding:8px}
        .edit input[type=submit] { padding:7px 10px}
        .PopUpPanel { width:400px; margin-left:-200px; left:50%}
    </style>
    <script type="text/javascript">
        function click_tab(oi) {
            document.getElementById("tb_tab").value = oi;
            if (oi < 4)
                document.getElementById("Button1").style.display = "block";
            else
                document.getElementById("Button1").style.display = "none";
        }
        function Confirm() {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure to cancel this transaction?"))
            { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
        }
        function myFunction() {

            $("#content_div_msg").fadeOut(5000)
        }
    </script>
 <script src="script/auto/myJScript.js" type="text/javascript"></script>
<link href="script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
<script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>

<script type="text/javascript">
    $(document).ready(function () {
        $.noConflict();
        $(".auto").autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "content/hr/editemployee.aspx/GetEmployee",
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
                $("#content_grid_report_lbl_bal_" + index).val(i.item.val);
            }
        });
    });

    
</script>
</asp:Content>
<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left">
        <h3>Employee</h3>
    </div>  
    <div class="title_right">
       <ul>
        <li><a href="MEmployee?user_id=<% Response.Write(Request.QueryString["user_id"].ToString()); %>"><i class="fa fa-file-text"></i> Master File</a></li>
        <li><i class="fa fa-angle-right"></i></li>
        <li>Employee</li>
       </ul>
    </div>       
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x_content">
                <div class="col-xs-12 emp-hd">
                    <img src="files/profile/<% Response.Write(profile.Value); %>.png" alt="" class="img-circle">
                    <h1><asp:Label ID="lb_name" runat="server" Font-Bold="true"></asp:Label>  </h1>
                    <asp:Label ID="l_position" runat="server" Text="Senior Programmer" Font-Size="12px" style=" margin-top:-10px; float:left;"></asp:Label>     
                </div>
                <div class="col-xs-3">
                    <ul class="nav nav-tabs tabs-left">
                        <li class="<% Response.Write(tb_tab.Text == "1" ? "active" : ""); %>"><a href="#identity" data-toggle="tab" onclick="click_tab(1)">Employee's Identity</a></li>
                        <li class="<% Response.Write(tb_tab.Text == "2" ? "active" : ""); %>"><a href="#personal" data-toggle="tab" onclick="click_tab(2)">Personal Info</a></li>
                        <li class="<% Response.Write(tb_tab.Text == "3" ? "active" : ""); %>""><a href="#messages" data-toggle="tab"  onclick="click_tab(3)">Payroll Info</a></li>
                        <li class="<% Response.Write(tb_tab.Text == "4" ? "active" : ""); %> " onclick="click_tab(4)"><a href="#skill" data-toggle="tab">Special Skills</a></li>
                        <li class=" <% Response.Write(tb_tab.Text == "5" ? "active" : ""); %> " onclick="click_tab(5)"><a href="#jobhistory" data-toggle="tab">Job History</a></li>
                        <li class=" <% Response.Write(tb_tab.Text == "6" ? "active" : ""); %> " onclick="click_tab(6)"><a href="#educational" data-toggle="tab">Educational Attainment</a></li>
                        <li class=" <% Response.Write(tb_tab.Text == "7" ? "active" : ""); %> " onclick="click_tab(7)"><a href="#asset" data-toggle="tab">Asset</a></li>
                        <li class=" <% Response.Write(tb_tab.Text == "8" ? "active" : ""); %> " onclick="click_tab(8)"><a href="#fmember" data-toggle="tab">Family Member</a></li>
                   <%--     <li class=" <% Response.Write(tb_tab.Text == "9" ? "active" : ""); %> " onclick="click_tab(9)"><a href="#bankdetails" data-toggle="tab">Bank Details</a></li>--%>
                         <li class=" <% Response.Write(tb_tab.Text == "10" ? "active" : ""); %> " onclick="click_tab(10)"><a href="#report" data-toggle="tab">Report To</a></li>
                        <li><asp:LinkButton ID="LinkButton2" OnClick="change_status"  runat="server">Status</asp:LinkButton></li>
                        <li><asp:LinkButton ID="LinkButton3" OnClick="quit_claim"  runat="server">Quit Claim Request</asp:LinkButton></li>
                        <li><asp:LinkButton ID="LinkButton4" OnClick="releasing"  runat="server">Quit Claim Releasing</asp:LinkButton></li>
                        <li><asp:LinkButton ID="LinkButton5" OnClick="gen2316"  runat="server" >Generate 2316</asp:LinkButton></li>
                       
                    </ul>
                </div>
                <div class="col-xs-9">
                    <div class="tab-content">
                    <div class="tab-pane<% Response.Write(tb_tab.Text == "1" ? " active" : ""); %>" id="identity">
                        <h5>Basic Information</h5>
                        <hr />
                        <ul class="input-form">
                            <li>ID Number</li>
                            <li><asp:TextBox ID="txt_idnumber" AutoComplete="off" runat="server"  onkeyup="intinput(this)"></asp:TextBox></li>
                            <li>Company</li>
                            <li><asp:dropdownlist ID="ddl_company" AutoComplete="off" runat="server" OnSelectedIndexChanged="click_company" AutoPostBack="true" ></asp:dropdownlist></li>
                            <li>Branch</li>
                            <li><asp:dropdownlist ID="ddl_branch" AutoComplete="off" runat="server" ></asp:dropdownlist></li>
                            <li>Department</li>
                            <li><asp:dropdownlist ID="ddl_department" AutoComplete="off" runat="server" ></asp:dropdownlist></li>
                            <li>Division</li>
                            <li><asp:dropdownlist ID="ddl_divission" AutoComplete="off" runat="server" ></asp:dropdownlist></li>
                            <li>Payroll Group</li>
                            <li><asp:dropdownlist ID="ddl_pg" runat="server" AutoComplete="off" ></asp:dropdownlist></li>
                            <li>Position</li>
                            <li><asp:dropdownlist ID="ddl_position" AutoComplete="off" runat="server" ></asp:dropdownlist></li>
                            <li>Report To</li>
                            <li><asp:dropdownlist ID="ddl_reportto" runat="server" AutoComplete="off" ></asp:dropdownlist></li>
                            <li>Status</li>
                            <li>
                                <asp:dropdownlist ID="ddl_status" Enabled="false" runat="server"> 
                                    <asp:ListItem Value="0">None</asp:ListItem>
                                    <asp:ListItem Value="1">Probationary</asp:ListItem>
                                    <asp:ListItem Value="2">Regular</asp:ListItem>
                                    <asp:ListItem Value="3">Contractual</asp:ListItem>
                                    <asp:ListItem Value="4">Resigned</asp:ListItem>
                                    <asp:ListItem Value="5">AWOL</asp:ListItem>
                                    <asp:ListItem Value="6">Terminated</asp:ListItem>
                                </asp:dropdownlist>
                            </li>
                            <li>Date Hired</li>
                            <li><asp:TextBox ID="txt_datehired"  runat="server" data-inputmask="'alias': 'date'"  ></asp:TextBox></li>
                            <li>Date Resigned</li>
                            <li><asp:TextBox ID="txt_dateresigned" runat="server" Enabled="false" data-inputmask="'alias': 'date'" ></asp:TextBox></li>
                        </ul>
                
                     
                       
                    </div>
                    <div class="tab-pane<% Response.Write(tb_tab.Text == "2" ? " active" : ""); %>" id="personal">
                        <h5>Personal</h5>
                        <hr />
                        <ul class="input-form">
                            <li>Last Name</li>
                            <li><asp:TextBox ID="txt_lname" AutoComplete="off" runat="server" ClientIDMode="Static" onclick="hh()" ></asp:TextBox></li>
                            <li>First Name</li>
                            <li><asp:TextBox ID="txt_fname" AutoComplete="off" runat="server"  ></asp:TextBox></li>
                            <li>Middle Name</li>
                            <li><asp:TextBox ID="txt_mname" AutoComplete="off" runat="server" ></asp:TextBox></li>
                            <li>Extension Name</li>
                            <li><asp:TextBox ID="txt_exname" AutoComplete="off" runat="server" ></asp:TextBox></li>
                            <li>Date of Birth</li>
                            <li><asp:TextBox ID="txt_dob" runat="server" data-inputmask="'alias': 'date'"></asp:TextBox></li>
                            <li>Place of Birth</li>
                            <li><asp:TextBox ID="txt_pob" AutoComplete="off" runat="server" ></asp:TextBox></li>
                            <li>Birth Zip Code</li>
                            <li><asp:dropdownlist ID="ddl_bzc" AutoComplete="off" runat="server" ></asp:dropdownlist></li>
                            <li>Sex</li>
                            <li><asp:dropdownlist ID="ddl_sex" AutoComplete="off" runat="server"><asp:ListItem>Male</asp:ListItem><asp:ListItem>Female</asp:ListItem></asp:dropdownlist></li>
                            <li>Civil Status</li>
                            <li><asp:dropdownlist ID="ddl_cs" AutoComplete="off" runat="server"><asp:ListItem>Single</asp:ListItem><asp:ListItem>Married</asp:ListItem><asp:ListItem>Widow</asp:ListItem></asp:dropdownlist></li>
                            <li>Citizenship</li>
                            <li><asp:dropdownlist ID="ddl_citizenship" AutoComplete="off" runat="server" ></asp:dropdownlist></li>
                            <li>Religion</li>
                            <li><asp:dropdownlist ID="ddl_relegion" AutoComplete="off" runat="server" ></asp:dropdownlist></li>
                            <li>Height</li>
                            <li><asp:TextBox ID="txt_height" AutoComplete="off" runat="server"></asp:TextBox></li>
                            <li>Weight</li>
                            <li><asp:TextBox ID="txt_weight" AutoComplete="off" runat="server"></asp:TextBox></li>
                            <li>Blood Type</li>
                            <li><asp:dropdownlist ID="ddl_bloodtype" AutoComplete="off" runat="server" ></asp:dropdownlist></li>
                            
                        </ul>
                        <h5>Address</h5>
                        <hr />
                        <ul class="emp-identity input-form">
                            <li>Present Address</li>
                            <li><asp:TextBox ID="txt_presentaddres" AutoComplete="off" runat="server" ></asp:TextBox></li>
                            <li>Permanent Address</li>
                            <li><asp:TextBox ID="txt_permanentadress" AutoComplete="off"   runat="server"></asp:TextBox></li>
                            <li>Zip Code</li>
                            <li><asp:dropdownlist ID="ddl_zipcode" AutoComplete="off" runat="server" ></asp:dropdownlist></li>
                        </ul>
                        
                        <h5>Contact</h5>
                        <hr />
                        <ul class="emp-identity input-form">
                            <li>Phone Number</li>
                            <li><asp:TextBox ID="txt_pnumber" AutoComplete="off" runat="server"></asp:TextBox></li>
                            <li>Cellphone Number</li>
                            <li><asp:TextBox ID="txt_cnumber" AutoComplete="off" runat="server"></asp:TextBox></li>
                            <li>Email Address</li>
                            <li><asp:TextBox ID="txt_email" AutoComplete="off" runat="server"></asp:TextBox></li>
                        </ul>
                    </div>
                    <div class="tab-pane<% Response.Write(tb_tab.Text == "3" ? " active" : ""); %>" id="messages">
                        <h5>Government Deductions</h5>
                        <hr />
                        <ul class="input-form">
                            <li>GSIS</li>
                            <li><asp:TextBox ID="txt_gsisno" AutoComplete="off" runat="server" onkeyup="intinput(this)"></asp:TextBox></li>
                            <li>SSS</li>
                            <li><asp:TextBox ID="txt_sssno" AutoComplete="off" runat="server" onkeyup="intinput(this)"></asp:TextBox></li>
                            <li>HDMF</li>
                            <li><asp:TextBox ID="txt_hdmfno" AutoComplete="off" runat="server" onkeyup="intinput(this)"></asp:TextBox></li>
                            <li>PHIC</li>
                            <li><asp:TextBox ID="txt_phicno" AutoComplete="off" runat="server" onkeyup="intinput(this)"></asp:TextBox></li>
                            <li>TIN</li>
                            <li><asp:TextBox ID="txt_tin" AutoComplete="off" runat="server" onkeyup="intinput(this)"></asp:TextBox></li>
                            <li>ATM</li>
                            <li><asp:TextBox ID="txt_atm" AutoComplete="off" runat="server" onkeyup="intinput(this)"></asp:TextBox></li>
                            <li>Tax Code</li>
                            <li><asp:dropdownlist ID="ddl_taxcode" AutoComplete="off" runat="server" ></asp:dropdownlist></li>
                            <li>GL Account</li>
                            <li><asp:dropdownlist ID="ddl_gl" runat="server" AutoComplete="off" ></asp:dropdownlist></li>
                            <li>Shift Code</li>
                            <li><asp:dropdownlist ID="ddl_shiftcode" runat="server"    AutoComplete="off" ><asp:ListItem></asp:ListItem></asp:dropdownlist></li>
                            <li>Tax Table</li>
                            <li><asp:dropdownlist ID="ddl_taxtable" AutoComplete="off" ClientIDMode="Static" onchange="addcompute()"  runat="server"><asp:ListItem></asp:ListItem><asp:ListItem>Daily</asp:ListItem><asp:ListItem>Weekly</asp:ListItem><asp:ListItem>Semi-Monthly</asp:ListItem><asp:ListItem>Monthly</asp:ListItem></asp:dropdownlist></li>
                            <li>Payroll Type</li>
                            <li>
                                <asp:dropdownlist ID="ddl_payrolltype" runat="server" ClientIDMode="Static" onchange="addcompute()" AutoComplete="off" Width="50%"></asp:dropdownlist>
                                <asp:CheckBox ID="chk_imw" runat="server" Text="Minimum Wage"/>
                                <asp:CheckBox ID="chk_l" runat="server" Text="Leave With Pay"/> 
                            </li>
                            <li>Fix No of Days</li>
                            <li><asp:TextBox ID="txt_fnod" runat="server" AutoComplete="off" onfocus="this.blur()" ClientIDMode="Static" onkeyup="decimalinput(this);addcompute();"></asp:TextBox></li>
                            <li>Fix No of Hours</li>
                            <li><asp:TextBox ID="txt_fnoh" runat="server" AutoComplete="off"  ClientIDMode="Static"></asp:TextBox></li>
                            <li>Monthly Rate</li>
                            <li><asp:TextBox ID="txt_mr" runat="server" AutoComplete="off" onfocus="this.blur()" ClientIDMode="Static" onkeyup="decimalinput(this);addcompute();"></asp:TextBox></li>
                            <li>Payroll Rate</li>
                            <li><asp:TextBox ID="txt_pr" runat="server"  onfocus="this.blur()"  AutoComplete="off" ClientIDMode="Static" onkeyup="decimalinput(this)"></asp:TextBox></li>
                            <li>Daily Rate</li>
                            <li><asp:TextBox ID="txt_dr" runat="server"  AutoComplete="off" onclick="this.select();" ClientIDMode="Static" onkeyup="decimalinput(this);addcompute();"></asp:TextBox></li>
                            <li>Absent Daily Rate</li>
                            <li><asp:TextBox ID="txt_adr" runat="server"  AutoComplete="off" onfocus="this.blur()" ClientIDMode="Static" onkeyup="decimalinput(this)"></asp:TextBox></li>
                            <li>Hourly Rate</li>
                            <li><asp:TextBox ID="txt_hr" runat="server"  AutoComplete="off" onfocus="this.blur()"  ClientIDMode="Static" onkeyup="decimalinput(this)"></asp:TextBox></li>
                            <li>Night Hourly Rate</li>
                            <li><asp:TextBox ID="txt_nhr" runat="server" AutoComplete="off" onfocus="this.blur()" ClientIDMode="Static" onkeyup="decimalinput(this)"></asp:TextBox></li>
                            <li>Overtime Hourly Rate</li>
                            <li><asp:TextBox ID="txt_ohr" runat="server"  AutoComplete="off" onfocus="this.blur()" ClientIDMode="Static" onkeyup="decimalinput(this)"></asp:TextBox></li>
                            <li>Overtime Night Hourly Rate</li>
                            <li><asp:TextBox ID="txt_onhr" runat="server"  AutoComplete="off" onfocus="this.blur()" ClientIDMode="Static" onkeyup="decimalinput(this)"></asp:TextBox></li>
                            <li>Tardy Hourly Rate</li>
                            <li><asp:TextBox ID="txt_thr" runat="server" onfocus="this.blur()" AutoComplete="off" ClientIDMode="Static" onkeyup="decimalinput(this)"></asp:TextBox></li>
                        </ul>
                    </div>
                    <div class="tab-pane skills edit<% Response.Write(tb_tab.Text == "4" ? " active" : ""); %>" id="skill">
                        <asp:TextBox ID="txt_skill" runat="server"></asp:TextBox>
                        <asp:Button ID="Button2" runat="server" Text="ADD" OnClick="addskill" CssClass="btn btn-primary" />
                        <hr />
                        <asp:GridView ID="grid_skill" runat="server" AutoGenerateColumns="False" EmptyDataText="No record found" CssClass="table table-striped table-bordered">
                            <Columns>
                                  <asp:BoundField DataField="skill" HeaderText="Skill"/>
                                  <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="can" runat="server" OnClick="cancelskill" CommandName='<%# Eval("id") %>'  ImageUrl="~/style/img/delete.png" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10px" />
                                </asp:TemplateField>
                            </Columns>            
                        </asp:GridView>
                    </div>
                    <div class="tab-pane jobhistory<% Response.Write(tb_tab.Text == "5" ? " active" : ""); %>" id="jobhistory">
                        <asp:GridView ID="grid_jobhistory" runat="server" EmptyDataText="No record found" AutoGenerateColumns="False" CssClass="table table-striped table-bordered">
                            <Columns>
                                <asp:BoundField DataField="company" HeaderText="Company"/>
                                <asp:BoundField DataField="position" HeaderText="Position"/>
                                <asp:BoundField DataField="datef" HeaderText="From"/>
                                <asp:BoundField DataField="datet" HeaderText="To"/>
                                  <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="can" runat="server" OnClick="canceljobhistory" CommandName='<%# Eval("id") %>' ImageUrl="~/style/img/delete.png" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10px"/>
                                </asp:TemplateField>
                            </Columns>            
                        </asp:GridView>
                        <asp:Button ID="Button8" runat="server" Text="ADD" OnClick="click_add" CommandName="jobhistory" CssClass="btn btn-primary"/>
                    </div>
                    <div class="tab-pane jobhistory<% Response.Write(tb_tab.Text == "6" ? "active" : ""); %>" id="educational">
                        <asp:GridView ID="grid_educ" runat="server" EmptyDataText="No record found"  AutoGenerateColumns="False"  CssClass="table table-striped table-bordered">
                            <Columns>
                                <asp:BoundField DataField="class" HeaderText="Attainment"/>
                                <asp:BoundField DataField="school" HeaderText="School"/>
                                <asp:BoundField DataField="address" HeaderText="Address"/>
                                <asp:BoundField DataField="yearf" HeaderText="From"/>
                                <asp:BoundField DataField="yeart" HeaderText="To" />
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="can" runat="server" OnClick="canceleduc" CommandName='<%# Eval("id") %>' ImageUrl="~/style/img/delete.png" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10px" />
                                </asp:TemplateField>
                            </Columns>            
                        </asp:GridView>
                        <asp:Button ID="Button9" runat="server" Text="ADD" OnClick="click_add"  CommandName="educational" CssClass="btn btn-primary"/>
                    </div>
                    <div class="tab-pane jobhistory<% Response.Write(tb_tab.Text == "7" ? "active" : ""); %>" id="asset">
                        <asp:GridView ID="grid_asset" runat="server" EmptyDataText="No record found" OnRowDataBound="rowboundasset"  AutoGenerateColumns="False" CssClass="table table-striped table-bordered">
                            <Columns>
                                <asp:BoundField DataField="id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                 <asp:BoundField DataField="invid" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"/>
                                <asp:BoundField DataField="cat" HeaderText="Category"/>
                                <asp:BoundField DataField="type" HeaderText="Type"/>
                                <asp:BoundField DataField="serial" HeaderText="Serial"/>
                                <asp:BoundField DataField="description" HeaderText="Description"/>
                                  <asp:BoundField DataField="status" HeaderText="Status"/>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                      <asp:ImageButton ID="can" runat="server" OnClick="cancelasset" OnClientClick="Confirm()" CommandName='<%# Eval("id") %>' ImageUrl="~/style/img/delete.png" />
                                       <asp:LinkButton ID="LinkButton1" runat="server" OnClick="clickreturn" Text="return"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="10px" />
                                </asp:TemplateField>   
                            </Columns>            
                        </asp:GridView>
                        <asp:Button ID="Button10" runat="server" Text="ADD" OnClick="click_add"  CommandName="asset" CssClass="btn btn-primary"/>
                    </div>
                    <div class="tab-pane jobhistory<% Response.Write(tb_tab.Text == "8" ? "active" : ""); %>" id="fmember">
                        <asp:GridView ID="grid_fmember" runat="server" EmptyDataText="No record found" AutoGenerateColumns="False" CssClass="table table-striped table-bordered">
                            <Columns>
                                <asp:BoundField DataField="firstname" HeaderText="First Name"/>
                                <asp:BoundField DataField="lastname" HeaderText="Last Name"/>
                                <asp:BoundField DataField="middlename" HeaderText="Middle"/>
                                <asp:BoundField DataField="extensionname" HeaderText="Extension Name"/>
                                <asp:BoundField DataField="relation" HeaderText="Relation"/>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="can" runat="server" OnClick="cancelfmember" CommandName='<%# Eval("id") %>' ImageUrl="~/style/img/delete.png" />
                                    </ItemTemplate>
                                  <ItemStyle Width="10px" />
                               </asp:TemplateField>   
                            </Columns>            
                        </asp:GridView>
                        <asp:Button ID="Button11" runat="server" Text="ADD" OnClick="click_add" CommandName="fmember" CssClass="btn btn-primary"/>
                    </div>
                    <div class="tab-pane jobhistory<% Response.Write(tb_tab.Text == "9" ? "active" : ""); %>" id="bankdetails">
                        <asp:GridView ID="grid_bankdetails" runat="server"  EmptyDataText="No record found"  AutoGenerateColumns="False" CssClass="table table-striped table-bordered">
                            <Columns>
                                <asp:BoundField DataField="accno" HeaderText="Account Number"/>
                                <asp:BoundField DataField="accname" HeaderText="Account Name"/>
                                <asp:BoundField DataField="location" HeaderText="Location/Branch"/>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="can" runat="server" OnClick="cancelbdetails" CommandName='<%# Eval("id") %>' ImageUrl="~/style/img/delete.png" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10px" />
                                </asp:TemplateField>   
                            </Columns>            
                        </asp:GridView>
                        <asp:Button ID="Button12" runat="server" Text="ADD" OnClick="click_add"  CommandName="bankdetails" CssClass="btn btn-primary"/>
                    </div>

                    <div class="tab-pane jobhistory<% Response.Write(tb_tab.Text == "10" ? "active" : ""); %>" id="report">
                        <asp:GridView ID="grid_reporting" runat="server"  EmptyDataText="No record found"  AutoGenerateColumns="False" CssClass="table table-striped table-bordered" DataKeyNames="herarchy"
                            HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White">
                            <Columns>
                              
                                <asp:BoundField DataField="id" HeaderText=""/>
                                <asp:BoundField DataField="emp_id" HeaderText=""/>
                                <asp:BoundField DataField="fullname" HeaderText="Full Name"/>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="can" runat="server" OnClick="cancelreport"  CommandName='<%# Eval("id") %>' ImageUrl="~/style/img/delete.png" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10px" />
                                </asp:TemplateField>  
                                
                                 <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkUp" CssClass="button" CommandArgument = "up" runat="server" Text="&#x25B2;" OnClick="ChangePreference" />
                                        <asp:LinkButton ID="lnkDown" CssClass="button" CommandArgument = "down" runat="server" Text="&#x25BC;" OnClick="ChangePreference" />
                                    </ItemTemplate>
                                </asp:TemplateField> 
                            </Columns>            
                        </asp:GridView>
                        <asp:Button ID="Button14" runat="server" Text="ADD" OnClick="click_add"  CommandName="ReportTo" CssClass="btn btn-primary"/>
                    </div>
                    </div>
              </div>
                <div class="clearfix"></div>
                <hr style="margin:0"/>
                <asp:Button ID="Button1" runat="server" Text="Update" ClientIDMode="Static" onclick="click_update_employee" CssClass="btn btn-primary right" style=" margin:10px"/>
                <asp:Label ID="l_msg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
            </div>
        </div>
    </div>
</div>
<div id="panelOverlay" runat="server" visible="false" class="Overlay"></div>
<div id="panelPopUpPanel" runat="server" visible="false" class="PopUpPanel skills">
    <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="close" runat="server"/>
    <h2>Job History</h2>
    <hr />
    <ul class="input-form">
        <li>Company</li>
        <li><asp:TextBox ID="txt_company" runat="server"></asp:TextBox></li>
        <li>Position</li>
        <li><asp:TextBox ID="txt_position" runat="server"></asp:TextBox></li>
        <li>From</li>
        <li><asp:TextBox ID="txt_datef" data-inputmask="'alias': 'date'" runat="server"></asp:TextBox></li>
        <li>To</li>
        <li><asp:TextBox ID="txt_datet" data-inputmask="'alias': 'date'" runat="server"></asp:TextBox></li>
    </ul>
    <hr />
    <asp:Button ID="Button3" runat="server" Text="ADD" OnClick="addjobhistory"  CssClass="btn btn-primary"/>
</div>

<div id="panelAttainment" runat="server" visible="false" class="PopUpPanel skills">
    <asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" OnClick="close" runat="server"/>
     <h2>Educational Attainment</h2>
    <hr />
    <ul class="input-form">
        <li>Attainment</li>
        <li>
            <asp:DropDownList ID="ddl_cat" runat="server">
                <asp:ListItem>Elementary</asp:ListItem>
                <asp:ListItem>HighSchool</asp:ListItem>
                <asp:ListItem>College</asp:ListItem>
                <asp:ListItem>Vocational</asp:ListItem>
            </asp:DropDownList>
        </li>
        <li>School</li>
        <li><asp:TextBox ID="txt_school" runat="server"></asp:TextBox></li>
        <li>Address</li>
        <li><asp:TextBox ID="txt_address"  runat="server"></asp:TextBox></li>
        <li>From</li>
        <li><asp:TextBox ID="txt_yearf" runat="server" data-inputmask="'alias': 'y'"></asp:TextBox></li>
        <li>To</li>
        <li><asp:TextBox ID="txt_yeart" runat="server" data-inputmask="'alias': 'y'"></asp:TextBox></li>
    </ul>
    <hr />
    <asp:Button ID="Button4" runat="server" Text="ADD" OnClick="addeduc" CssClass="btn btn-primary"/>
</div>
<div id="panelAsset" runat="server" visible="false" class="PopUpPanel skills">
    <asp:ImageButton ID="ImageButton2" ImageUrl="~/style/img/closeb.png" OnClick="close" runat="server"/>
    <h2>Asset</h2>
    <hr />
    <ul class="input-form">
        <li>Serial</li>
        <li><asp:DropDownList ID="ddl_serial" OnTextChanged="clickserial" AutoPostBack="true" runat="server"><asp:ListItem>Return</asp:ListItem><asp:ListItem>Lost</asp:ListItem></asp:DropDownList></li><%--<asp:TextBox ID="txt_serial" runat="server" placeholder="Serial"></asp:TextBox>--%>
       <%-- <li>Asset</li>
        <li><asp:TextBox ID="txt_asset"  runat="server" placeholder="Asset"></asp:TextBox></li>--%>
        <li>Description</li>
        <li><asp:TextBox ID="txt_description" runat="server"  placeholder="Description"></asp:TextBox></li>
        <li>Remarks</li>
        <li><asp:TextBox ID="txt_remarks" runat="server" placeholder="txt_remarks"></asp:TextBox></li>
       <%-- <li>Qty</li>
        <li><asp:TextBox ID="txt_qty" runat="server" placeholder="Qty"></asp:TextBox></li>--%>
    </ul>
    <hr />
    <asp:Button ID="Button5" runat="server" Text="ADD" OnClick="addasset" CssClass="btn btn-primary"/>
</div>

<div id="divstat" runat="server" visible="false" class="PopUpPanel skills">
    <asp:ImageButton ID="ImageButton5" ImageUrl="~/style/img/closeb.png" OnClick="close" runat="server"/>

    <ul class="input-form">
        <li>Status</li>
        <li><asp:DropDownList ID="ddl_statassign" OnSelectedIndexChanged="clickchangestat" style=" width:100%;"  AutoPostBack="true" runat="server"><asp:ListItem></asp:ListItem><asp:ListItem>Return</asp:ListItem><asp:ListItem>Lost</asp:ListItem></asp:DropDownList></li><%--<asp:TextBox ID="txt_serial" runat="server" placeholder="Serial"></asp:TextBox>--%>
        <li>Remarks</li>
        <li><asp:DropDownList ID="ddl_remarks" runat="server" style=" width:100%;"></asp:DropDownList>
    
    </ul>
    <hr />
    <asp:Button ID="Button13" runat="server" Text="Update"  OnClick="updatechangestat"  CssClass="btn btn-primary"/>
</div>

<div id="panelFamily" runat="server" visible="false" class="PopUpPanel skills">
    <asp:ImageButton ID="ImageButton3" ImageUrl="~/style/img/closeb.png" OnClick="close" runat="server"/>
    <h2>Family Member</h2>
    <hr />
    <ul class="input-form">
        <li>First Name</li>
        <li><asp:TextBox ID="txt_fname1" runat="server"></asp:TextBox></li>
        <li>Last Name</li>
        <li><asp:TextBox ID="txt_lname1"  runat="server"></asp:TextBox></li>
        <li></li>
        <li>Middle Name<asp:TextBox ID="txt_mname1" runat="server"></asp:TextBox></li>
        <li>Extension Name</li>
        <li><asp:TextBox ID="txt_exname1" runat="server"></asp:TextBox></li>
        <li>Relation</li>
        <li><asp:TextBox ID="txt_relation" runat="server"></asp:TextBox></li>
    </ul>
    <hr />
    <asp:Button ID="Button6" runat="server" Text="ADD" OnClick="addfmember" CssClass="btn btn-primary"/>
</div>

<div id="panelBank" runat="server" visible="false" class="PopUpPanel skills">
    <asp:ImageButton ID="ImageButton4" ImageUrl="~/style/img/closeb.png" OnClick="close" runat="server"/>
    <h2>Bank Details</h2>
    <hr />
    <ul class="input-form">
        <li>Account Number</li>
        <li><asp:TextBox ID="txt_accno" runat="server"></asp:TextBox></li>
        <li>Account Name</li>
        <li><asp:TextBox ID="txt_accname"  runat="server"></asp:TextBox></li>
        <li>Location</li>
        <li><asp:TextBox ID="txt_location" runat="server"></asp:TextBox></li>
    </ul>
    <hr />
    <asp:Button ID="Button7" runat="server" Text="ADD" OnClick="addbdetails" CssClass="btn btn-primary" />
</div>

<div id="panelReport" runat="server" visible="false" class="PopUpPanel skills">
    <asp:ImageButton ID="ImageButton6" ImageUrl="~/style/img/closeb.png" OnClick="close" runat="server"/>
    <h2>Report To</h2>

    <asp:GridView ID="grid_report" runat="server" ShowFooter="True" onrowdeleting="grid_report_RowDeleting" ShowHeader="false"  AutoGenerateColumns="False" CssClass="table table-striped table-bordered" >
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="15px"> 
                                    <ItemTemplate>
                                       <%# Container.DataItemIndex + 1%>.
                                    </ItemTemplate>
                                    <FooterTemplate >             
                                        <asp:ImageButton ID="btn" runat="server"  ImageUrl="~/style/img/add.png"  OnClick="ButtonAdd_Click_report" />
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_report" runat="server" ForeColor="Red" Font-Size="10px" ></asp:Label>
                                        <asp:Label ID="lbl_report_desp"  runat="server" Text=""></asp:Label> 
                                        <asp:Textbox ID="ddl_reportto" CssClass="auto" runat="server" Text=""></asp:Textbox> 
                                        <asp:Textbox ID="lbl_bal" style="visibility:hidden" runat="server" Text=""></asp:Textbox> 
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="can" runat="server" CausesValidation="false" CommandName="Delete" ImageUrl="~/style/img/delete.png" />
                                    </ItemTemplate>
                                    <ItemStyle Width="120px" />
                                </asp:TemplateField>
                            </Columns>            
  </asp:GridView>

    <hr />
    <asp:Button ID="Button15" runat="server" OnClick="btn_report" Text="ADD" CssClass="btn btn-primary" />
</div>
<div id="div_status" runat="server" visible="false" class="PopUpPanel skills">
    <asp:ImageButton ID="ImageButton7" ImageUrl="~/style/img/closeb.png" OnClick="close" runat="server"/>
        <asp:Label ID="lbl_header" runat="server"></asp:Label>
        <hr />
     <table style=" width:100%;">
      <tr>
      <th>Status</th>
      </tr>
                <tr>
                    
                    <td>  
                        <asp:dropdownlist ID="ddl_statusfinal"  runat="server" > 
                        <asp:ListItem Value="0">None</asp:ListItem>
                        <asp:ListItem Value="1">Probationary</asp:ListItem>
                        <asp:ListItem Value="2">Regular</asp:ListItem>
                        <asp:ListItem Value="3">Contractual</asp:ListItem>
                        <asp:ListItem Value="4">Resigned</asp:ListItem>
                        <asp:ListItem Value="5">AWOL</asp:ListItem>
                        <asp:ListItem Value="6">Terminated</asp:ListItem>
                        </asp:dropdownlist> <%--OnSelectedIndexChanged="select_status" AutoPostBack="true"--%>
                    </td>
                   
                </tr>

                 <tr>
                    <th><asp:Label ID="span_id" runat="server"  Visible="false" Text="Effective Date"></asp:Label></th><%--<asp:Label ID="span_id" runat="server" Visible="false" Text=""></asp:Label>--%>
                </tr>
                <tr>
                     <td>
                         <asp:TextBox ID="txt_effdate" Visible="false" data-inputmask="'alias': 'date'" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <th>Attachment</th><%--<asp:Label ID="span_id" runat="server" Visible="false" Text=""></asp:Label>--%>
                </tr>
                <tr>
                     <td>
                       <asp:FileUpload ID="FileUpload1"  runat="server" multiple/>
                    </td>
                </tr>
                <tr>
                   <th>Notes</th>
                </tr>
                <tr>
                <td> <asp:TextBox ID="txt_notes" runat="server" style=" resize:none; width:100%" TextMode="MultiLine"></asp:TextBox>
                </td>
                </tr>
            </table>
        <hr />
        <asp:Button ID="btn_proc_16" runat="server" Text="Proccess" OnClick="process" CssClass="btn btn-primary" />
        <asp:Label ID="lbl_errmsg" style=" color:Red;" runat="server"></asp:Label>
        <asp:HiddenField ID="hdn_proc" runat="server" />
</div>
<div id="div_releasing" runat="server" visible="false" class="PopUpPanel skills">
    <asp:ImageButton ID="ImageButton8" ImageUrl="~/style/img/closeb.png" OnClick="close" runat="server"/>
       <h2>Releasing</h2>
        <hr />
     <table style=" width:100%;">
      <tr>
      <th>Attachment</th>
      </tr>
                <tr>
                     <td>
                       <asp:FileUpload ID="FileUpload2"  runat="server" multiple/>
                    </td>
                </tr>
                <tr>
                   <th>Notes</th>
                </tr>
                <tr>
                <td> <asp:TextBox ID="txt_notes_released" runat="server" style=" resize:none; width:100%" TextMode="MultiLine"></asp:TextBox>
                </td>
                </tr>
            </table>
        <hr />
        <asp:Button ID="btn_proc_17" runat="server" Text="Proccess" OnClick="releasedqc"  CssClass="btn btn-primary" />
        <asp:Label ID="lbl_rel_errmsg" style=" color:Red;" runat="server"></asp:Label>
        <asp:HiddenField ID="HiddenField1" runat="server" />
</div>
<div id="div_gen2316" runat="server" visible="false" class="PopUpPanel skills">
    <asp:ImageButton ID="ImageButton9" ImageUrl="~/style/img/closeb.png" OnClick="close" runat="server"/>
       <h2>Generate 2316</h2>
        <hr />
         <ul>
            <li>Year</li>
            <li>
                <asp:TextBox ID="txt_yyyy" runat="server" ClientIDMode="Static" onkeyup="intinput(this)"></asp:TextBox>
            </li>
         </ul>
        <hr />
        <asp:Button ID="Button18" runat="server" Text="Proccess" OnClick="process2316"  CssClass="btn btn-primary" />
        <asp:Label ID="Label1" style=" color:Red;" runat="server"></asp:Label>
        <asp:HiddenField ID="HiddenField2" runat="server" />
</div>
<div style=" visibility:hidden;">
<asp:TextBox ID="tb_tab" runat="server" ClientIDMode="Static" Text="1"></asp:TextBox>
<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
<asp:TextBox ID="txt_sssaddon" AutoComplete="off" runat="server" onkeyup="decimalinput(this)"></asp:TextBox>
<asp:dropdownlist ID="ddl_hdmftype" AutoComplete="off" runat="server"><asp:ListItem>Value</asp:ListItem><asp:ListItem>Percentage</asp:ListItem></asp:dropdownlist>
<asp:CheckBox ID="chk_sssgs" AutoComplete="off" runat="server"  />
<asp:TextBox ID="txt_hdmfaddon" AutoComplete="off" runat="server" onkeyup="decimalinput(this)"></asp:TextBox>
</div>
<asp:HiddenField ID="id" runat="server" />
<asp:HiddenField ID="chngstatid" runat="server" />
<asp:HiddenField ID="chngremarksid" runat="server" />
<asp:HiddenField ID="profile" runat="server" />
<asp:HiddenField ID="reqid" runat="server" />
</asp:Content>
<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
<script src="vendors/jquery.inputmask/dist/min/jquery.inputmask.bundle.min.js"></script>
<script type='text/javascript' src="script/inputmasking/4lenghtshiftdosjquery-1.11.0.js"></script>
<script type='text/javascript'>
    $.noConflict();
    $(window).load(function () {
        $(":input").inputmask();
        $("#phone").inputmask({ "mask": "(999) 999-9999" });
    });
</script>
</asp:Content>
