<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Employee201.aspx.cs" Inherits="content_Employee_Employee201" MasterPageFile="~/content/site.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        li { list-style:none}
        input[type=date]{ line-height:normal !important}
        .emp-identity{padding:0;float:left; width:48%;}
        .emp-identity input[type=text]{padding:8px; width:100%; margin-bottom:10px}
        .emp-identity select {padding:7px; width:100%;margin-bottom:10px}
        .emp-identity input[type=checkbox] {margin-right:5px !important;}
        .popup{padding:0;float:left; width:100%;}
        .popup input[type=text]{padding:8px; width:100%; margin-bottom:10px}
        .popup select {padding:7px; width:100%;margin-bottom:10px}
        .popup input[type=checkbox] {margin-right:5px !important;}
        .bei{margin-left:25px}
        .table > tbody > tr > td { vertical-align: middle !important}
        .table input[type=text]{ width:100%; padding:5px; border:1px solid #eee}
        .dnone{ display:none;}
        .Grid {
        table-layout:fixed; 
        width:100%; 
        }
        .Grid .Shorter {
        overflow: hidden; 
        text-overflow: ellipsis;    
      
        white-space:pre-wrap;       
        }
        
        ul.bar_tabs {padding-left:0px}
       .tab-content .page-header {margin:20px 0 20px !important}
       .nav-tabs-custom > .nav-tabs > li {    border-top: 1px solid transparent;} 
       .nav-tabs-custom > .nav-tabs > li a {padding:10px 14px !important}
       .nav-tabs-custom > .nav-tabs > li.active {border-top-color: #eee !important;}
       .checkbox {margin-left:20px}
.nav-tabs-custom {margin-bottom:10px}
.profile_left {margin-top:10px}

    .modal { position:fixed !important}
    .form-control[disabled], .form-control[readonly], fieldset[disabled] .form-control { background-color: #fff !important}
    </style>

<script src="script/auto/myJScript.js" type="text/javascript"></script>
<link href="script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
<script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>
<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
<script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
<script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
<link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
<script type="text/javascript" src="style/js/googleapis_jquery.min.js"></script>
<script src="style/js/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
<script src="vendors/tab/modernizr.custom.js"></script>
<script type="text/javascript">
    $(window).load(function () {
        setTimeout(function () { $('.alert').fadeOut('slow') }, 5000);

        base64 = $("#dataURLInto").val();
        if (base64 != "")
            $("#dataURLView").html('<img class="img-responsive avatar-view"  src="' + base64 + '" title="Profile" style=" width:100%">');

        console.log($("#tab_index").val());

        tab = $("#tab_index").val();
        if (tab > 2)
            $("#btn_submit").hide();
        else
            $("#btn_submit").show();
    });

    $(document).ready(function () {
        $.noConflict();
        $(".auto").click(function () {
            AutoNobel($(this).attr("id"));
        });

        function AutoNobel(orly) {
            console.log(orly);
            $(".auto").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "content/hr/addemployee.aspx/GetEmployee",
                        data: "{'term':'" + $(".auto").val() + "','sender':'" + orly + "'}",
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
        }
    });
</script>
<script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to cancel this transaction?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    } 
</script>
<link rel="stylesheet" href="dist/css/base.css">
</asp:Content>


<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<section class="content-header">
    <h1>My Profile</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">My Profile</li>
    </ol>
</section>
<section class="content">
    <div class="row">

    <div id="pgridattainment" runat="server" visible="false" class="Overlay"></div>
    <div id="pgridattainment1" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton3" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li>
            <asp:dropdownlist id="ddllevel" runat="server" CssClass="form-control">
                <asp:ListItem>Elementary</asp:ListItem>
                <asp:ListItem>High School</asp:ListItem>
                <asp:ListItem>College</asp:ListItem>
                <asp:ListItem>Vocational</asp:ListItem>
            </asp:dropdownlist><hr />
            </li>
            <li><asp:TextBox ID="txtupschool" Placeholder="School" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtupaddress" Placeholder="School Address" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtupfrom" Placeholder="Year From" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtupto" Placeholder="Year To" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="Button13" OnClick="updatemattainment" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgridjobhist" runat="server" visible="false" class="Overlay"></div>
    <div id="pgridjobhist1" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton4" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txtupposition" Placeholder="Job Position" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtupcompany" Placeholder="Company" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtupfromyear" Placeholder="From" runat="server" CssClass="form-control datee"></asp:TextBox></li>
            <li><asp:TextBox ID="txtuptoyear" Placeholder="To" runat="server" CssClass="form-control datee"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="Button17" OnClick="updatemjobhist" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgridseminar" runat="server" visible="false" class="Overlay"></div>
    <div id="pgridseminar1" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton5" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txtupseminaratt" Placeholder="Seminart Attended" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtupheld" Placeholder="Held At" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtupconducted" Placeholder="Date Conducted" runat="server" CssClass="form-control datee"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="Button19" OnClick="updatemseminar" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgridtraining" runat="server" visible="false" class="Overlay"></div>
    <div id="pgridtraining1" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton6" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txtuplicense" Placeholder="License Number" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtupval" Placeholder="Validity Date" runat="server" CssClass="form-control datee"></asp:TextBox></li>
            <li><asp:TextBox ID="txtuploc" Placeholder="Location" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtupamo" Placeholder="Amount" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtupduf" Placeholder="Duration From" runat="server" CssClass="form-control datee"></asp:TextBox></li>
            <li><asp:TextBox ID="txtupdut" Placeholder="Duration To" runat="server" CssClass="form-control datee"></asp:TextBox></li>
            <li><asp:TextBox ID="txtuppro" Placeholder="Provider" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtupab" Placeholder="About" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtupres" Placeholder="Resources" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="Button20" OnClick="updatemtraining" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgridskills" runat="server" visible="false" class="Overlay"></div>
    <div id="pgridskills2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton2" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txtupskill" Placeholder="Special Skills" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="btnupdate" OnClick="updatemskills" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgridcontact" runat="server" visible="false" class="Overlay"></div>
    <div id="pgridcontact1" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton10" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txtupname" Placeholder="Contact Name" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtupconadd" Placeholder="Address" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtupconnum" Placeholder="Contact Number" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="Button21" OnClick="updatemcontact" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgridmedrec" runat="server" visible="false" class="Overlay"></div>
    <div id="pgridmedrec1" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton11" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txtupdate" Placeholder="Contact Name" runat="server" CssClass="form-control datee"></asp:TextBox></li>
            <li><asp:DropDownList ID="ddlupillness" CssClass="form-control" runat="server"></asp:DropDownList></li>
            <li><asp:DropDownList ID="ddluphost" CssClass="form-control" runat="server"></asp:DropDownList></li>
            <li><asp:TextBox ID="txtuphys" Placeholder="Physician" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="Button22" OnClick="updatemmedrecs" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
        <div class="col-lg-12">
          <div class="box box-primary">
            <div class="box-body">
                <div class="row">
                <div class="col-md-3">
                     <div class="profile_img img-hover">
                        <div id="dataURLView">
                            <img class="img-responsive avatar-view" src="files/profile/<%= profile.Value %>.png" onerror="this.onerror=null;this.src='files/profile/0.png'" title="Profile" style=" width:100%" />
                        </div>
                        <div class="middle" style="position:absolute; margin-top:-20px; margin-left:5px">
                            <div class="docs-toolbar">
                                <label for="inputImage" title="Upload image file">
                                    <input class="hide" id="inputImage" name="file" type="file" accept="image/*">
                                    <i class="fa fa-camera text-white" style="color:#fff !important"></i>
                                </label>
                            </div>
                        </div>
                    </div>
                    <% if(ViewState["action"].ToString() == "Edit") { %>
                    <div class="box-header with-border margin-r-5">
                        <div class="text-center">
                            <h4 class="text-bold"><%= ViewState["name"]%></h4>
                            <h6 class="card-subtitle"><%= ViewState["position"]%></h6>
                        </div>
                    </div>
                    <div class="box-body">
                        <strong><i class="fa fa-user margin-r-5"></i> ID Number</strong>
                        <p class="text-muted"><%= ViewState["id"]%></p>
                        <hr>
                        <strong><i class="fa fa-phone margin-r-5"></i> Phone</strong>
                        <p class="text-muted"><%= ViewState["phone"].ToString().Length>0?ViewState["phone"].ToString():"N/A"%></p>
                        <hr>
                        <strong><i class="fa fa-envelope-square margin-r-5" ></i> Email address</strong>
                        <p class="text-muted"><%= ViewState["email"].ToString().Length>0?ViewState["email"].ToString():"N/A"%></p>
                        <hr>
                        <strong><i class="fa fa-map-marker margin-r-5"></i> Address</strong>
                        <p class="text-muted"><%= ViewState["address"].ToString().Length>0?ViewState["address"].ToString():"N/A"%></p>
                    </div>
                    <% } %>
                </div>
                    <div class="col-md-9">
                        <div class="tab-content">
                            <div id="alert" runat="server" visible="false" class="alert alert-danger alert-dismissible">
                                <h4><i class="icon fa fa-info-circle"></i> Alert!</h4>
                                <asp:Label ID="l_msg" runat="server" CssClass="block" style="position:inherit"></asp:Label>
                            </div>
                            <div class="view">
                            <h4 class="page-header">Employee's Identity</h4>
                            <div class="row">
                                <div class="col-lg-2">
                                    <div class="form-group">
                                      <label>ID Number </label>
                                      <asp:TextBox ID="txt_idnumber" AutoComplete="off" runat="server" class="form-control disabled"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="form-group">
                                        <label>SAP Number</label>
                                        <asp:TextBox ID="txt_sapnumber"  runat="server" CssClass="form-control" ></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label>Company</label>
                                        <asp:dropdownlist ID="ddl_company" AutoComplete="off" runat="server" CssClass="form-control" OnSelectedIndexChanged="click_company" AutoPostBack="true"></asp:dropdownlist>
                                    </div>
                                </div>
                                <div class="col-lg-4 none">
                                    <div class="form-group">
                                        <label>Outlet</label>
                                        <asp:dropdownlist ID="ddl_store" runat="server" AutoComplete="off" CssClass="form-control" ></asp:dropdownlist>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label>Location</label>
                                        <asp:dropdownlist ID="ddl_branch" AutoComplete="off" runat="server"  CssClass="form-control" ></asp:dropdownlist>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-2">
                                    <div class="form-group">
                                        <label>Band/Level</label>
                                        <asp:dropdownlist ID="ddl_divission" AutoComplete="off" runat="server" CssClass="form-control"></asp:dropdownlist>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="form-group">
                                        <label>Level</label>
                                        <asp:dropdownlist ID="ddl_divission2" AutoComplete="off" runat="server" CssClass="form-control"></asp:dropdownlist>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label>Role</label>
                                        <asp:dropdownlist ID="ddl_lvel" AutoComplete="off" runat="server" CssClass="form-control"></asp:dropdownlist>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label>Payroll Group</label>
                                        <asp:dropdownlist ID="ddl_pg" CssClass="form-control" AutoComplete="off" runat="server"></asp:dropdownlist>
                                    </div>
                                </div>
                             </div>
                            <div class="row">
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label>Department</label>
                                        <asp:dropdownlist ID="ddl_department" AutoComplete="off" runat="server" CssClass="form-control"></asp:dropdownlist>
                                    </div>
                                </div>
                                 <div class="col-lg-4">
                                    <div class="form-group">
                                        <label>Section</label>
                                        <asp:dropdownlist ID="ddl_section" AutoComplete="off" runat="server" CssClass="form-control"></asp:dropdownlist>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label>Position</label>
                                        <asp:dropdownlist ID="ddl_position" CssClass="form-control" AutoComplete="off" runat="server"></asp:dropdownlist>
                                    </div>
                                </div>
                             </div>
                            <div class="row">
                                <div class="col-lg-2">
                                    <div class="form-group">
                                        <label>Current Rate </label>
                                        <asp:TextBox ID="txt_cmr" AutoComplete="off" runat="server" ClientIDMode="Static" oncopy="return false;" onpaste="return false;" oncut="return false;"  onkeyup="decimalinput(this);addcompute();" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="form-group">
                                        <label>Previous Rate </label>
                                        <asp:TextBox ID="txt_pmr" AutoComplete="off" runat="server" ClientIDMode="Static" oncopy="return false;" onpaste="return false;" oncut="return false;"  onkeyup="decimalinput(this);addcompute();" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label>Change Rate Date Effectivity</label>
                                        <asp:TextBox ID="txt_chde"  runat="server" CssClass="form-control datee" ></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-4 none">
                                    <div class="form-group">
                                        <label>SBU Date</label>
                                        <asp:TextBox ID="txt_sbudate"  runat="server" CssClass="form-control datee" ></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="form-group">
                                        <label>Date Hired </label>
                                        <asp:TextBox ID="txt_datehired"  runat="server" CssClass="form-control datee" ></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="form-group">
                                        <label>Status </label>
                                        <asp:dropdownlist ID="ddl_status" AutoComplete="off" runat="server" CssClass="form-control"></asp:dropdownlist>
                                    </div>
                                 </div>
                             </div>
                             <div class="row">
                             </div>
                            </div>

                            <h4 class="page-header">Personal Information</h4>
                            <div class="row">
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>First Name <span class="text-red"></span></label>
                                        <asp:TextBox ID="txt_fname" AutoComplete="off" runat="server" ClientIDMode="Static" onclick="hh()" CssClass="form-control" ></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>Last Name <span class="text-red"></span></label>
                                        <asp:TextBox ID="txt_lname" AutoComplete="off" runat="server" ClientIDMode="Static"  CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                    <label>Middle Name <span class="text-red"></span></label>
                                    <asp:TextBox ID="txt_mname" AutoComplete="off" runat="server" ClientIDMode="Static"  CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                    <label>Extension Name <span class="text-red"></span></label>
                                    <asp:TextBox ID="txt_exname" AutoComplete="off" runat="server" ClientIDMode="Static"  CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-2">
                                    <div class="form-group">
                                        <label>Gender</label>
                                        <asp:dropdownlist ID="ddl_sex" AutoComplete="off" runat="server">
                                            <asp:ListItem Value="Male">Male</asp:ListItem>
                                            <asp:ListItem Value="Female">Female</asp:ListItem>
                                        </asp:dropdownlist>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="form-group">
                                        <label>Date of Birth</label>
                                        <asp:TextBox ID="txt_dob" AutoComplete="off" runat="server" CssClass="form-control datee"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-6">
                                    <div class="form-group">
                                        <label>Place of Birth</label>
                                        <asp:TextBox ID="txt_pob" AutoComplete="off" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                     <div class="form-group">
                                    <label>Birth Zip Code</label>
                                    <asp:dropdownlist ID="ddl_bzc" AutoComplete="off" runat="server" ></asp:dropdownlist>
                                    </div>
                                </div>
                                 <div class="col-lg-12">
                                     <div class="form-group">
                                    <label>Permanent Address</label>
                                    <asp:TextBox ID="txt_permanentadress" AutoComplete="off"   runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-9">
                                    <div class="form-group">
                                        <label>Present Address</label>
                                        <asp:TextBox ID="txt_presentaddres" AutoComplete="off" runat="server" CssClass="form-control"></asp:TextBox>
                                     </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>Zip Code</label>
                                        <asp:dropdownlist ID="ddl_zipcode" AutoComplete="off" runat="server" ></asp:dropdownlist>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                               <div class="col-lg-4">
                                    <div class="form-group">
                                    <label>Cellphone Number</label>
                                    <asp:TextBox ID="txt_cnumber" AutoComplete="off" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label>Phone Number</label>
                                        <asp:TextBox ID="txt_pnumber" AutoComplete="off" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                               <div class="col-lg-4">
                                    <div class="form-group">
                                        <label>Email Address</label>
                                        <asp:TextBox ID="txt_email" AutoComplete="off" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                   
                                </div>
                             </div>
                             <div class="row">
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>Civil Status</label>
                                        <asp:dropdownlist ID="ddl_cs" AutoComplete="off" runat="server"></asp:dropdownlist>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>Number of Children</label>
                                        <asp:TextBox ID="txt_noc" AutoComplete="off" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>Citizenship</label>
                                        <asp:dropdownlist ID="ddl_citizenship" AutoComplete="off" runat="server"></asp:dropdownlist>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>Religion</label>
                                        <asp:dropdownlist ID="ddl_relegion" AutoComplete="off" runat="server"></asp:dropdownlist>
                                    </div>
                                </div>
                              </div>
                              <div class="row">
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>Height</label>
                                        <asp:TextBox ID="txt_height" AutoComplete="off" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>Weight</label>
                                        <asp:TextBox ID="txt_weight" AutoComplete="off" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>Blood Type</label>
                                        <asp:dropdownlist ID="ddl_bloodtype" AutoComplete="off" runat="server"></asp:dropdownlist>
                                    </div>
                                </div>
                                <div class="col-lg-3 none">
                                    <div class="form-group">
                                        <label>Health Card Expiration</label>
                                        <asp:TextBox ID="txt_health" Enabled="false" AutoComplete="off" runat="server" CssClass="form-control datee"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>Personal Email</label>
                                        <asp:TextBox ID="txt_pemail" AutoComplete="off" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <h4 class="page-header">Educational Attainment</h4>
                            <asp:GridView ID="grid_educhistory1" runat="server" EmptyDataText="No Data Found!" AutoGenerateColumns="False"  CssClass="table table-striped table-bordered">
                                <Columns>
                                    <asp:BoundField DataField="id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone" />
                                    <asp:BoundField DataField="level" HeaderText="Level"  />
                                    <asp:BoundField DataField="school" HeaderText="School"  />
                                    <asp:BoundField DataField="address" HeaderText="Address"  />
                                    <asp:BoundField DataField="yearf" HeaderText="From"  />
                                    <asp:BoundField DataField="yeart" HeaderText="To"  />
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbeditor" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="openattainement" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                                            <asp:LinkButton ID="can" ToolTip="Delete" runat="server" CausesValidation="false" OnClientClick="Confirm()" OnClick="delete_edhistory" CommandName='<%# Eval("id") %>' CssClass="readupdate"><i class="fa fa-trash"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <button type="button" class="btn btn-default" data-toggle="modal" data-target="#modal-attainment">Add</button>

                            <h4 class="page-header">Job History</h4>
                            <asp:GridView ID="grid_jobhistory1" runat="server" EmptyDataText="No Data Found!"  AutoGenerateColumns="False"  CssClass="table table-striped table-bordered">
                                <Columns>
                                      <asp:BoundField DataField="id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone" />
                                    <asp:BoundField DataField="position" HeaderText="Position"  />
                                    <asp:BoundField DataField="company" HeaderText="Company"  />
                                    <asp:BoundField DataField="froms" HeaderText="Date From"  />
                                    <asp:BoundField DataField="tos" HeaderText="Date To"  />
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbeditor" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="openjobhist" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                                            <asp:LinkButton ID="cancel" runat="server" CommandName='<%# Eval("id") %>' ToolTip="Delete" OnClientClick="Confirm()" OnClick="delete_jobhistory"><i class="fa fa-trash"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <button type="button" class="btn btn-default" data-toggle="modal" data-target="#modal-jobhistory">Add</button>

                            <h4 class="page-header">Seminars Attended</h4>
                            <asp:GridView ID="grid_seminarsattended1" runat="server" EmptyDataText="No Data Found!" OnRowDataBound="saDatabound" AutoGenerateColumns="False" CssClass="table table-striped table-bordered">
                                <Columns>
                                    <asp:BoundField DataField="id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                    <asp:BoundField DataField="fileid" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                    <asp:BoundField DataField="seminarsattended" HeaderText="Seminar's Attended"/>
                                    <asp:BoundField DataField="seminarsheld" HeaderText="Held At"/>
                                    <asp:BoundField DataField="dateconducted" HeaderText="Date Conducted"/>
                                    <asp:TemplateField  HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbeditor" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="openseminar" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                                            <asp:linkbutton ID="lbdownloadsem" CommandName='<%#Eval("id") %>' runat="server" Visible="false" ToolTip="Download Certificate" OnClick="downloadseminarsfile"><i class="fa fa-download"></i></asp:linkbutton>
                                            <asp:LinkButton ID="cancel" runat="server" CommandName='<%# Eval("id") %>' ToolTip="Delete" OnClientClick="Confirm()" OnClick="deleteseminarsattended"><i class="fa fa-trash"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="130px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                             <button type="button" class="btn btn-default" data-toggle="modal" data-target="#modal-seminar">Add</button>

                            <h4 class="page-header">Employee Special Skills</h4>
                            <asp:GridView ID="grid_skill1" runat="server" EmptyDataText="No Data Found!"  AutoGenerateColumns="False"  CssClass="table table-striped table-bordered">
                                <Columns>
                                        <asp:BoundField DataField="ID" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                        <asp:BoundField DataField="empid" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                        <asp:BoundField DataField="skill" HeaderText="Skills"/>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbeditor" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="openskills" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                                                <asp:LinkButton ID="cancel" runat="server" CommandName='<%# Eval("Id") %>' ToolTip="Delete" OnClientClick="Confirm()" OnClick="deleteskill"><i class="fa fa-trash"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <button type="button" class="btn btn-default" data-toggle="modal" data-target="#modal-skill">Add</button>

                            <h4 class="page-header">Emergency Contact</h4>
                                <asp:GridView ID="grid_emergencycontact" runat="server" EmptyDataText="No Data Found!"  AutoGenerateColumns="False"  CssClass="table table-striped table-bordered">
                                  <Columns>
                                        <asp:BoundField DataField="Id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                        <asp:BoundField DataField="EmpId" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                        <asp:BoundField DataField="Name" HeaderText="Name" />
                                        <asp:BoundField DataField="Address" HeaderText="Address" />
                                        <asp:BoundField DataField="Contact" HeaderText="Contact Number" />
                                       <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbeditor" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="openemcontact" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                                                <asp:LinkButton ID="lnkbtndel" runat="server" CommandName='<%# Eval("Id") %>' Text="Remove" OnClientClick="Confirm()"  ClientIDMode="Static" OnClick="deleteContact" style=" margin:3px;" ><i class="fa fa-trash"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                  </Columns>
                                </asp:GridView>
                           <button type="button" class="btn btn-default" data-toggle="modal" data-target="#modal-emergency">Add</button>

                           <h4 class="page-header">Training Attended</h4>
                             <asp:GridView ID="grid_preparatory" runat="server" EmptyDataText="No Data Found!" AutoGenerateColumns="False" CssClass="table table-striped table-bordered">
                              <Columns>
                                <asp:BoundField DataField="Id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone" />
                                <asp:TemplateField HeaderText="License Number">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnprep" runat="server" CommandName='<%# Eval("Id") %>' ToolTip="List" Text='<%# Eval("LicenseNum") %>' OnClick="opendownloadprep" ></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ValidUntil" HeaderText="Validity" />
                                <asp:BoundField DataField="Location" HeaderText="Location" />
                                <asp:BoundField DataField="Cost" HeaderText="Amount Cost" />
                                <asp:BoundField DataField="DF" HeaderText="Duration From" />
                                <asp:BoundField DataField="DT" HeaderText="Duration To" />
                                <asp:BoundField DataField="Provider" HeaderText="Provider" />
                                <asp:BoundField DataField="About" HeaderText="About" />
                                <asp:BoundField DataField="Resources" HeaderText="Resources" />
                                <asp:TemplateField HeaderText="Action">
                                 <ItemTemplate>
                                    <asp:LinkButton ID="lbeditor" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="opentraining" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                                    <asp:LinkButton ID="cancel" runat="server" CommandName='<%# Eval("Id") %>' ToolTip="Delete" OnClientClick="Confirm()" OnClick="cancelprep"><i class="fa fa-trash"></i></asp:LinkButton>
                                 </ItemTemplate>
                                 <ItemStyle Width="120px" />
                                </asp:TemplateField>
                              </Columns>
                            </asp:GridView>
                            <button type="button" class="btn btn-default" data-toggle="modal" data-target="#modal-preparatory">Add</button>

                            <h4 class="page-header">Medical History</h4>
                            <asp:GridView ID="gridmedrics" EmptyDataText="No Data Found!" OnRowDataBound="medrecscell" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                            <Columns>
                                <asp:BoundField DataField="Id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone" />
                                <asp:BoundField DataField="meddate" HeaderText="Date"/>
                                 <asp:BoundField DataField="illness" HeaderText="Illness"/>
                                <asp:BoundField DataField="hospital" HeaderText="Hospital"/>
                                <asp:BoundField DataField="doctor" HeaderText="Physician"/>
                                <asp:BoundField DataField="findings" HeaderText="Findings"/>
                                <asp:BoundField DataField="condition" HeaderText="Admitted"/>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate >
                                        <asp:LinkButton ID="lbeditor" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="openemmedrec" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lbdwnmedrecs" runat="server" Visible="false" CommandName='<%# Eval("id") %>' OnClick="dwnmedrecs" ToolTip="Download" style="margin:3px;" ><i class="fa fa-download"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lbaction" runat="server" CommandName='<%# Eval("id") %>' OnClick="rmvmedrecord" ToolTip="Remove" OnClientClick="Confirm()"  ClientIDMode="Static" style="margin:3px;" ><i class="fa fa-trash"></i></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="125px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <button type="button" class="btn btn-default" data-toggle="modal" data-target="#modal-medical">Add</button>
                        <hr />
                        <asp:Button ID="btn_submit" runat="server" OnClick="submit" CssClass="btn btn-primary" ClientIDMode="Static" Text="Next" />


                          </div>
                    </div>
                    <script src="vendors/tab/NewcbpFWTabs.js"></script>
                    <script>
                        jQuery.noConflict();
                        (function () {
                            [ ].slice.call(document.querySelectorAll('.nav-tabs-custom')).forEach(function (el) { new CBPFWTabs(el); });
                        })(jQuery);
                    </script>
                    </div>
                </div>
            </div>
          </div>
        </div>
    </div>
</section>


<asp:HiddenField ID="hf_action" runat="server" />
 
<div class="clearfix"></div> 

<div class="modal fade in" id="prepdownload" runat="server">
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <div align="right">
                <asp:LinkButton ID="LinkButton1" OnClick="closingremarks" runat="server"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>
            </div>
        <h4 class="modal-title">Certificates</h4>
        </div>
        <div class="modal-body">
        <asp:GridView ID="grid_prepdownloadables" runat="server" EmptyDataText="No Data Found!"  AutoGenerateColumns="false" CssClass="table table-striped table-bordered" >
            <Columns>
                <asp:BoundField DataField="filename" HeaderText="File Name"/>
                <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkbtndownloadpreps" runat="server" ToolTip="Download" CommandName='<%# Eval("Id") %>' Text="download" OnClick="downloadprepdocs"><i class="fa fa-download"></i></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle Width="75px" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </div>
    </div>
    </div>
</div>

<div class="modal fade in" id="modal-profile" >
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button id="close-modalprofile" type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title">Change Profile</h4>
            </div>
            <div class="modal-body">
                <div class="img-container"><img src="vendors/crop image/img/picture-1.jpg"></div>
                <div class="row docs-data-url none">
                <textarea class="form-control" id="dataURLInto" runat="server" rows="8" clientidmode="Static"></textarea>
                </div>
            </div>
            <div class="modal-footer">
                <button class="btn btn-primary" id="getDataURL2" data-toggle="tooltip" type="button" title="$().cropper(&quot;getDataURL&quot;, &quot;image/jpeg&quot;)">Change Profile</button>
            </div>
        </div>
    </div>
</div>

<div class="modal fade in" id="modal-attainment" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title">Add Educational Attainment</h4>
        </div>
        <div class="modal-body">
        <div class="form-group">
            <label>Level</label>
            <asp:dropdownlist id="ddl_level" runat="server">
                <asp:ListItem>Elementary</asp:ListItem>
                <asp:ListItem>High School</asp:ListItem>
                <asp:ListItem>College</asp:ListItem>
                <asp:ListItem>Vocational</asp:ListItem>
            </asp:dropdownlist>
        </div>
        <div class="form-group">
            <label>School</label>
            <asp:Textbox ID="txt_school"  runat="server" CssClass="form-control"></asp:Textbox>
        </div>
        <div class="form-group">
            <label>Address</label>
            <asp:Textbox ID="txt_address"  runat="server" CssClass="form-control"></asp:Textbox>
        </div>
        <div class="form-group">
            <label>From</label>
            <asp:dropdownlist id="txt_yearf" runat="server" ></asp:dropdownlist>
        </div>
        <div class="form-group">
        <label>To</label>
        <asp:dropdownlist id="txt_yeart" runat="server"></asp:dropdownlist>
        </div>
        </div>
        <div class="modal-footer">
        <asp:Button ID="Button7" runat="server" Text="ADD" OnClick="edhistory" CssClass="btn btn-primary" />
        </div>
    </div>
    </div>
</div>

<div class="modal fade" id="modal-jobhistory" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title">Add Job History</h4>
        </div>
        <div class="modal-body">
            <div class="form-group">
                <label>Company</label>
                <asp:TextBox ID="txt_company" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Date from</label>
                <div class="row">
                    <div class="col-lg-6">
                        <div class="form-group">
                            <asp:dropdownlist id="txt_month" runat="server">
                                <asp:ListItem>Month</asp:ListItem>
                                <asp:ListItem>01</asp:ListItem>
                                <asp:ListItem>02</asp:ListItem>
                                <asp:ListItem>03</asp:ListItem>
                                <asp:ListItem>04</asp:ListItem>
                                <asp:ListItem>05</asp:ListItem>
                                <asp:ListItem>06</asp:ListItem>
                                <asp:ListItem>07</asp:ListItem>
                                <asp:ListItem>08</asp:ListItem>
                                <asp:ListItem>09</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                            </asp:dropdownlist>
                        </div>
                    </div>
                    <div class="col-lg-6">
                        <div class="form-group">
                            <asp:dropdownlist id="txt_year" runat="server">
                            </asp:dropdownlist>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label>Date To</label>
                <div class="row">
                    <div class="col-lg-6">
                        <div class="form-group">
                            <asp:dropdownlist id="txt_datetomonth" runat="server">
                                <asp:ListItem>Month</asp:ListItem>
                                <asp:ListItem>01</asp:ListItem>
                                <asp:ListItem>02</asp:ListItem>
                                <asp:ListItem>03</asp:ListItem>
                                <asp:ListItem>04</asp:ListItem>
                                <asp:ListItem>05</asp:ListItem>
                                <asp:ListItem>06</asp:ListItem>
                                <asp:ListItem>07</asp:ListItem>
                                <asp:ListItem>08</asp:ListItem>
                                <asp:ListItem>09</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                            </asp:dropdownlist>
                        </div>
                    </div>
                    <div class="col-lg-6">
                        <div class="form-group">
                            <asp:dropdownlist id="txt_datetoyear" runat="server">
                            </asp:dropdownlist>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group no-margin">
                <label>Position</label>
                <asp:TextBox ID="txt_position" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button ID="Button1" runat="server" OnClick="add_jobhistory" Text="ADD" CssClass="btn btn-primary" />
        </div>
    </div>
    </div>
</div>

<div class="modal fade in" id="modal-seminar" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title">Add Seminar</h4>
        </div>
        <div class="modal-body">
            <div class="form-group">
                <label>Name</label>
                <asp:TextBox ID="txt_seminar" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Held</label>
                <asp:TextBox ID="txt_heldat" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Date</label>
                <asp:TextBox ID="txt_dateseminars" runat="server" CssClass="form-control datee"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:FileUpload ID="fuseminarsphoto" accept=".png,.jpg,.jpeg,.gif,.pdf,.tif,.bmp" runat="server"/>
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnseminarattended" runat="server" OnClick="addseminarattended" Text="ADD" CssClass="btn btn-primary" />
        </div>
    </div>
    </div>
</div>

<div class="modal fade in" id="modal-preparatory" >
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title">ADD TRAINING</h4>
            </div>
            <div class="modal-body">
                <div class="form-group">
                  <div class="col-lg-9">
                      <div class="form-group">
                        <label>License Number</label>
                        <asp:TextBox ID="txt_license" runat="server" CssClass="form-control"></asp:TextBox>
                     </div>
                  </div>
                  <div class="col-lg-3">
                    <div class="form-group">
                     <label>Valid Until</label>
                    <asp:TextBox ID="txt_validity" runat="server" CssClass="form-control datee"></asp:TextBox>
                    </div>
                  </div>
                </div>
                <div class="form-group">
                  <div class="col-lg-9">
                    <div class="form-group">
                    <label>Location</label>
                    <asp:TextBox ID="txt_location" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                  </div>
                  <div class="col-lg-3">
                    <div class="form-group">
                    <label>Cost</label>
                    <asp:TextBox ID="txt_cost" runat="server" CssClass="form-control" onkeyup="decimalinput(this)"></asp:TextBox>
                    </div>
                  </div>
                </div>
                <div class="form-group">
                  <div class="col-lg-5">
                  <div class="form-group">
                    <label>Duration From</label>
                    <asp:TextBox ID="txt_durationfrom" runat="server" CssClass="form-control datee"></asp:TextBox>
                    </div>
                  </div>
                  <div class="col-lg-5">
                    <div class="form-group">
                    <label>Duration To</label>
                    <asp:TextBox ID="txt_durationto" runat="server" AutoPostBack="true" OnTextChanged="calculatedate" CssClass="form-control datee"></asp:TextBox>
                    </div>
                  </div>
                  <div class="col-lg-2">
                    <div class="form-group">
                    <label>Day/s</label>
                    <asp:TextBox ID="txt_days" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                  </div>
                </div>
                <div class="form-group">
                  <div class="col-lg-12">
                  <div class="form-group">
                    <label>Provider</label>
                    <asp:TextBox ID="txt_provider" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                  </div>
                </div>
                <div class="form-group">
                  <div class="col-lg-12">
                    <div class="form-group">
                    <label>Abouts</label>
                    <asp:TextBox ID="txt_about" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                  </div>
                </div>
                <div class="form-group">
                  <div class="col-lg-12">
                  <div class="form-group">
                    <label>Resources</label>
                    <asp:TextBox ID="txt_resources" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                  </div>
                </div>
                <div class="form-group">
                  <div class="col-lg-12">
                    <asp:FileUpload ID="fupreparatory" runat="server"/>
                  </div>
                </div>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btntrining" OnClick="preparatoryentry" runat="server" Text="Add" CssClass="btn btn-default" />
            </div>
        </div>
    </div>
</div>

<div class="modal fade in" id="modal-skill" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title">Add skill</h4>
        </div>
        <div class="modal-body">
                <div class="form-group">
                <label>Name</label>
                <asp:TextBox ID="txt_skill" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button ID="Button2" runat="server" OnClick="addss" Text="ADD" CssClass="btn btn-primary" />
        </div>
    </div>
    </div>
</div>

<div class="modal fade in" id="modal-emergency" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title">Add Emergency Contact</h4>
        </div>
        <div class="modal-body">
             <div class="form-group">
                <label>Name</label>
                <asp:TextBox ID="txtnameemergency" runat="server" CssClass="form-control"></asp:TextBox>
                <label>Address</label>
                <asp:TextBox ID="txtaddressemergency" runat="server" CssClass="form-control"></asp:TextBox>
                <label>Contact Number/s</label>
                <asp:TextBox ID="txtcontactemergency" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button ID="Button12" runat="server" OnClick="addemergencycontact" Text="ADD" CssClass="btn btn-primary" />
        </div>
    </div>
    </div>
</div>

<div class="modal fade in" id="modal-medical" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title">Add Medical History</h4>
        </div>
        <div class="modal-body">
            <div class="form-group">
                <label>Hospital</label> <asp:LinkButton ID="lnkhost" runat="server" ToolTip="Add" CommandName='<%# Eval("Id") %>' Text="Add" data-toggle="modal" data-target="#modal-medical-hospital"><i class="fa fa-plus"></i></asp:LinkButton>
                <asp:DropDownList ID="ddl_medical" runat="server" ></asp:DropDownList>
            </div>
            <div class="form-group">
                <label>Illness</label> <asp:LinkButton ID="lnkill" runat="server" ToolTip="Add" CommandName='<%# Eval("Id") %>' Text="Add" data-toggle="modal" data-target="#modal-medical-illness"><i class="fa fa-plus"></i></asp:LinkButton>
                <asp:DropDownList ID="ddl_illness" runat="server" ></asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:TextBox ID="txt_meddate" Placeholder="Date" runat="server" CssClass="form-control datee"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:TextBox ID="txt_medphysician" Placeholder="Physician" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                 <asp:Dropdownlist ID="ddlcondition" runat="server" Width="120px">
                    <asp:ListItem Value="">Admitted</asp:ListItem><asp:ListItem Value="0">Yes</asp:ListItem><asp:ListItem Value="1">No</asp:ListItem>
                 </asp:Dropdownlist>
            </div>
            <div class="form-group">
                 <asp:Dropdownlist ID="ddlfindings" runat="server" Width="120px">
                    <asp:ListItem Value="">Findings</asp:ListItem><asp:ListItem Value="0">Postive</asp:ListItem><asp:ListItem Value="1">Negative</asp:ListItem>
                 </asp:Dropdownlist>
            </div>
            <div class="form-group">
                <asp:TextBox ID="txt_mednote" Placeholder="Note..." runat="server" TextMode="MultiLine" CssClass="form-control datee"></asp:TextBox>
            </div>
            <asp:FileUpload ID="fuhostdoc" multiple runat="server" />
        </div>
        <div class="modal-footer">
            <asp:Button ID="Button14" runat="server" Text="ADD" OnClick="savemedrec" CssClass="btn btn-primary" />
        </div>
    </div>
    </div>
</div>

<div class="modal fade in" id="modal-medical-hospital" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title">Hospital Details</h4>
        </div>
        <div class="modal-body">
            <div class="form-group">
                <label>Hospital</label> <asp:Label ID="Label15" runat="server" ForeColor="Red"></asp:Label>
                <asp:TextBox ID="txthostdesc" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Address</label>
                <asp:TextBox ID="txthostadd" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Contact</label>
                <asp:TextBox ID="txthostcont" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button ID="Button15" runat="server" Text="ADD"  OnClick="savehost" CssClass="btn btn-primary" />
        </div>
    </div>
    </div>
</div>

<div class="modal fade in" id="modal-medical-illness" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
            <h4 class="modal-title">Illness Details</h4>
        </div>
        <div class="modal-body">
            <div class="form-group">
                <label>Illness</label> <asp:Label ID="Label16" runat="server" ForeColor="Red"></asp:Label>
                <asp:TextBox ID="txtillness" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button ID="Button16" runat="server" Text="ADD"  OnClick="addillness" CssClass="btn btn-primary" />
        </div>
    </div>
    </div>
</div>


<asp:LinkButton ID="Button8" runat="server" OnClick="refresh" Text="Refresh" CssClass="none"></asp:LinkButton>
<div class="row none">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x_content">
                <div class="col-xs-3">
                    <ul class="nav nav-tabs tabs-left">
                        <li class="" onclick="click_tab(1)"><a href="#identity" data-toggle="tab">Employee's Identity</a></li>
                        <li class="" onclick="click_tab(2)"><a href="#personal" data-toggle="tab">Personal Info</a></li>
                        <li class="" onclick="click_tab(3)"><a href="#messages" data-toggle="tab">Payroll Info</a></li>
                        <li class="" onclick="click_tab(4)"><a href="#skill" data-toggle="tab">Special Skills</a></li>
                        <li class="" onclick="click_tab(16)"><a href="#empsrequirements" data-toggle="tab">Requirement/s</a></li>
                        <li class="" onclick="click_tab(14)"><a href="#seminaratteded" data-toggle="tab">Seminar's Attended</a></li>
                        <li class="" onclick="click_tab(15)"><a href="#empssanctions" data-toggle="tab">Sanctions/s</a></li>
                        <li class="" onclick="click_tab(5)"><a href="#jobhistory" data-toggle="tab">Job History</a></li>
                        <li class="" onclick="click_tab(6)"><a href="#educational" data-toggle="tab">Attainment</a></li>
                        <li class="" onclick="click_tab(7)"><a href="#asset" data-toggle="tab">Asset</a></li>
                        <li class="" onclick="click_tab(8)"><a href="#fmember" data-toggle="tab">Dependent</a></li>
                        <li class="" onclick="click_tab(10)"><a href="#report" data-toggle="tab">Report To</a></li>
                        <li class="" onclick="click_tab(11)"><a href="#div_lc" data-toggle="tab">Leave Credits</a></li>
                        <li class="" onclick="click_tab(12)"><a href="#div_HMO" data-toggle="tab">HMO</a></li>
                        <li><asp:LinkButton ID="LinkButton2" OnClick="change_status"  runat="server">Status</asp:LinkButton></li>
                       <% if (Session["emp_id"].ToString() == "ed")
                        {%>
                       <%--   <li><asp:LinkButton ID="LinkButton2" OnClick="change_status"  runat="server">Status</asp:LinkButton></li>--%>
                       <%--   <li><asp:LinkButton ID="LinkButton3" OnClick="quit_claim"  runat="server">Quit Claim Request</asp:LinkButton></li>
                        <li><asp:LinkButton ID="LinkButton4" OnClick="releasing"  runat="server">Quit Claim Releasing</asp:LinkButton></li>
                        <li><asp:LinkButton ID="LinkButton5" OnClick="gen2316"  runat="server" >Generate 2316</asp:LinkButton></li>--%>
                       <%}%>
                    </ul>
                </div>
                <div class="col-xs-9" >
                    <div class="tab-content">
                     <div class="tab-pane" id="empsrequirements">
                    <div class="col-md-9">
                        <div class="clearfix"></div><br />
                    <asp:GridView ID="grid_forms" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                        <Columns>
                            <asp:BoundField DataField="class" HeaderText="Class"/>
                            <asp:BoundField DataField="description" HeaderText="Description"/>
                            <asp:TemplateField>
                            <ItemTemplate>
                            <asp:LinkButton ID="lnk_view" runat="server" OnClientClick="Confirm()" OnClick="clickcancelforms"  CommandName='<%# Eval("id") %>' Text="cancel" style=" margin:3px;" ></asp:LinkButton>
                            <asp:LinkButton ID="lnk_viewforms" runat="server" CommandName='<%# Eval("id") %>' Text="download" OnClick="downloadformfiles" style=" margin:3px;" ></asp:LinkButton>
                            </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    
                    <div id="div_nolaman" runat="server" class="alert alert-empty">
                            <i class="fa fa-info-circle"></i>
                            <span>No record found</span>
                    </div>
                    </div>
                    <div class="col-md-3">
                        
                    </div>
                     <asp:Label ID="lbl_class" runat="server" ForeColor="Red"></asp:Label>
                    <div id="Div1" visible="false" runat="server" class="Overlay"></div>
                    <div id="panelPopUpPanel" runat="server" visible="false" class="PopUpPanel pop-a">
                        <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
                       
                         <div id="div_msg" runat="server" class="alert alert-empty">
                            <i class="fa fa-info-circle"></i>
                            <span>No record found</span>
                         </div>
                    </div>
                    </div>
                    <div class="tab-pane" id="identity" >
                        <asp:Button ID="btn_identity" runat="server" Text="Next" OnClick="next" CssClass="btn btn-primary" />
                    </div>
                    <div class="tab-pane" id="personal">
                        <asp:Button ID="btn_personal" runat="server" Text="Next" OnClick="next1" CssClass="btn btn-primary" />
                    </div>
                    <div class="tab-pane" id="messages">
                        
                       
                        <div class="clearfix"></div>
                          <asp:Button ID="btn_payroll" runat="server" Text="Next" OnClick="next2" CssClass="btn btn-primary" />
                          <asp:HiddenField ID="hdn_minimumwage" ClientIDMode="Static"  runat="server" />
                    </div>
                    <div class="tab-pane" id="skill">
                   
                    </div>
                    <div class="tab-pane"id="seminaratteded">
                   
                    
                    </div>

                    <div class="tab-pane"id="empssanctions">
                   
                    </div>
                    <div id="overlay" runat="server" visible="false" class="Overlay"></div>
                    <div id="invetPanel" runat="server" visible="false" class="PopUpPanel modal-medium">
                        <asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" OnClick="closeupload" runat="server"/>
                         <ul class="input-form">
                            
                         </ul>
                    </div>
                    <div id="suspendpanel" runat="server" visible="false" class="PopUpPanel modal-medium">
                        <asp:ImageButton ID="imgclose" ImageUrl="~/style/img/closeb.png" OnClick="closeupload" runat="server"/>
                       
                    </div>

                    <div class="tab-pane" id="jobhistory">
                   
                    <div class="emp-identity bei">
                    
                    </div>
                    <div class="clearfix"></div>
                    </div>
                    <div class="tab-pane" id="educational">
                    
               
                    <div class="clearfix"></div>
                    </div>

                    <div class="tab-pane" id="asset">
                    
                    <div class="emp-identity bei">
                    
                    </div>

                    <div class="clearfix"></div>
                    </div>
                    <div class="tab-pane" id="fmember">
                    <ul class="emp-identity">
                    <li>First Name</li>
                    <li><asp:Textbox ID="txt_fmem_firstname" runat="server" Text=""></asp:Textbox> </li>
                    <li>Last Name</li>
                    <li><asp:Textbox ID="txt_fmem_lname" runat="server" Text=""></asp:Textbox></li>
                    <li>Middle Name</li>
                    <li><asp:Textbox ID="txt_fmem_mname" runat="server" Text=""></asp:Textbox></li>
                    <li>Extension Name</li>
                    <li><asp:Textbox ID="txt_fmem_ename" runat="server" Text=""></asp:Textbox></li>
                    <li>Relation</li>
                    <li><asp:Textbox ID="txt_fmem_relation" runat="server" Text=""></asp:Textbox></li>
                    <li><asp:Button ID="Button9" runat="server" Text="PROCESS" OnClick="fmember" CssClass="btn btn-primary" /></li>
                    </ul>
                    <div class="emp-identity bei">
                    <asp:GridView ID="grid_fmember1" runat="server" EmptyDataText="No Data Found!" AutoGenerateColumns="False" CssClass="table table-striped table-bordered">
                        <Columns>
                            <asp:BoundField DataField="id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone" />
                            <asp:BoundField DataField="firstname" HeaderText="First Name" />
                            <asp:BoundField DataField="lastname" HeaderText="Last Name" />
                            <asp:BoundField DataField="middlename" HeaderText="Middle Name" />
                            <asp:BoundField DataField="extensionname" HeaderText="Extension Name" />
                            <asp:BoundField DataField="relation" HeaderText="Relation" />
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="can" runat="server" CausesValidation="false" OnClick="delete_fmember" ImageUrl="~/style/img/delete.png" />
                                </ItemTemplate>
                                <ItemStyle Width="5px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>
                    <div class="clearfix"></div>
                    </div>
                    <div class="tab-pane" id="report"> 
                    
                        
                    </div>
                    <div class="tab-pane" id="div_lc">
                
                    <div class="emp-identity bei">
                    
                    </div>
                    <div class="clearfix"></div>
                    </div>
                    <div class="tab-pane" id="div_HMO">
                    
            
                    <div style=" width:100%">
                    
                     </div>
                    </div>
                 </div>
                </div>
                <div class="clearfix"></div>
                <hr />
            
              
            </div>
        </div>
    </div>
</div>

<div id="panelOverlay" runat="server" visible="false" class="Overlay"></div>
<div id="div_status" runat="server" visible="false" class="PopUpPanel skills">
    <asp:ImageButton ID="ImageButton7" ImageUrl="~/style/img/closeb.png" OnClick="close" runat="server"/>
        <ul class="popup">
        <li><asp:Label ID="lbl_header" runat="server"></asp:Label></li>
        <li><hr /></li>
        <li>Status</li>
        <li><asp:dropdownlist ID="ddl_statusfinal"  runat="server" ></asp:dropdownlist> </li>
        <li><asp:Label ID="span_id" runat="server"  Visible="false" Text="Effective Date"></asp:Label></li>
        <li><asp:TextBox ID="txt_effdate" Visible="false" cssclass="datee" runat="server"></asp:TextBox></li>
        <li>Attachment</li>
        <li><asp:FileUpload ID="FileUpload2"  runat="server" multiple/></li>
        <li>Notes</li>
        <li><asp:TextBox ID="txt_notes" runat="server" style=" resize:none; width:100%" TextMode="MultiLine"></asp:TextBox></li>
        <li><hr /></li>
        <li><asp:Button ID="btn_proc_16" runat="server" Text="Proccess" OnClick="process" CssClass="btn btn-primary" /></li>
        <li><asp:Label ID="lbl_errmsg" style=" color:Red;" runat="server"></asp:Label></li>
        <li><asp:HiddenField ID="hdn_proc" runat="server" /></li>
        </ul>
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
                       <asp:FileUpload ID="FileUpload3"  runat="server" multiple/>
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

<asp:HiddenField ID="hdn_empid" runat="server" />
<asp:HiddenField ID="lbl_bals" ClientIDMode="Static"  runat="server" />
<asp:HiddenField ID="id" runat="server" />
<asp:HiddenField ID="chngstatid" runat="server" />
<asp:HiddenField ID="chngremarksid" runat="server" />
<asp:HiddenField ID="profile" runat="server" Value="0"/>
<asp:HiddenField ID="reqid" runat="server" />
<asp:HiddenField ID="sancsionterm" runat="server" />
<asp:HiddenField ID="sanctionid" runat="server" />
<asp:HiddenField ID="nodayssus" runat="server" />
<asp:HiddenField ID="trn_det_id" runat="server" />
<asp:HiddenField ID="hfEmp" runat="server" />
<asp:HiddenField ID="medical" runat="server" />
<asp:HiddenField ID="seriesid" runat="server" />

    <div style=" visibility:hidden;">

    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <asp:CheckBox ID="chk_sssgs" AutoComplete="off" runat="server"  />
    <asp:dropdownlist ID="ddl_hdmftype" AutoComplete="off" runat="server"><asp:ListItem>Value</asp:ListItem><asp:ListItem>Percentage</asp:ListItem></asp:dropdownlist>

</div>
</asp:Content>

<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
<script src="vendors/jquery.inputmask/dist/min/jquery.inputmask.bundle.min.js"></script>
<script type='text/javascript' src="script/inputmasking/4lenghtshiftdosjquery-1.11.0.js"></script>
<script type='text/javascript'>
    $('.view').find('input, textarea, button, select').attr('disabled', 'disabled');
    $.noConflict();
    $(window).load(function () {
        $(":input").inputmask();
        $("#phone").inputmask({ "mask": "(999) 999-9999" });
    });
</script>

<%--Date picker--%>
<script type="text/javascript">
    $.noConflict();
    $('.datee').datepicker()
</script>

<%--CROPPER--%>
<script src="vendors/crop image/js/jquery-1.12.4.min.js"></script>
<script src="vendors/crop image/js/cropper.min.js"></script>
<script src="vendors/crop image/js/main.js"></script>
<link href="vendors/crop image/css/cropper.min.css" rel="stylesheet">
<link href="vendors/crop image/css/main.css" rel="stylesheet">
</asp:Content>
