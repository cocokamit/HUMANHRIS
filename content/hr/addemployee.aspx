<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addemployee.aspx.cs" Inherits="content_hr_addemployee" MasterPageFile="~/content/MasterPageNew.master" %>
<%@ Import Namespace="System.Data" %>
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
    .Grid{table-layout:fixed;width:100%;}
    .Grid .Shorter{overflow: hidden;text-overflow: ellipsis;white-space:pre-wrap;}   
    ul.bar_tabs {padding-left:0px}
    .tab-content .page-header {margin:20px 0 20px !important}
    .nav-tabs-custom > .nav-tabs > li {    border-top: 1px solid transparent;} 
    .nav-tabs-custom > .nav-tabs > li a {padding:10px 14px !important}
    .nav-tabs-custom > .nav-tabs > li.active {border-top-color: #eee !important;}
    .checkbox {margin-left:20px}
    .nav-tabs-custom {margin-bottom:10px}
    .profile_left {margin-top:10px}
    .select2-container--default .select2-selection--single { border-radius:0px !important}
    .select2-container .select2-selection--single { height:34px !important}
    .select2-container .select2-selection--single .select2-selection__rendered { padding-left:0px !important}
    .select2-container--default .select2-selection--single .select2-selection__rendered { padding-top:2px !important}
    .alert i { padding-right:0px !important}
</style>

<link rel="stylesheet" href="dist/css/base.css">
<link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />
<link href="script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
<link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
<script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
<script src="script/auto/myJScript.js" type="text/javascript"></script>
<script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>
<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
<script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
<script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
<script type="text/javascript" src="style/js/googleapis_jquery.min.js"></script>
<script src="vendors/tab/modernizr.custom.js"></script>
<script type="text/javascript">
    $(window).load(function() {
        setTimeout(function() { $('.alert').fadeOut('slow') }, 5000);

        base64 = $("#dataURLInto").val();
        if (base64 != "")
            $("#dataURLView").html('<img class="img-responsive avatar-view"  src="' + base64 + '" title="Profile" style=" width:100%">');

        var tab = $("#tab_index").val();
        console.log("tab : " + tab);

        var tb = document.getElementsByClassName('allower');
        tb[tab].click();
    });

    function SinglePage(rid, uid) {
        var pro = true;
        $.ajax({
            type: "POST",
            url: "content/hr/addemployee.aspx/readUpdate",
            data: "{rid:" + rid + ",uid:" + uid + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(data) {
                var ru = document.getElementsByClassName("readupdate");

                if (data.d) {
                    for (var i = 0; i < ru.length; i++) {
                        ru[i].setAttribute("style", "display:block;");
                    }

                }
                else {
                    for (var i = 0; i < ru.length; i++) {
                        ru[i].setAttribute("style", "display:none;");
                    }
                }


                if (rid == "1901" && data.d == false)
                    pro = false;

                if (rid == "1902" && data.d == false)
                    pro = false;
                if (rid == "1912" && data.d == false)
                    pro = false;


                console.log("aaa");

                if (data.d == true) {

                    if (rid == "1902" || rid == "1901" || rid == "1912") {
                        $("#btn_submit").show();
                    } else {
                        $("#btn_submit").hide();
                        console.log("bbb");
                    }
                }
                //                else
                //                    $("#btn_submit").hide();



            },
            error: function(err, result) {
                alert(err);
            }
        });



    }

    function nerve() {
        alert("test");
        tab = $("#tab_index").val();
        var tb = document.getElementsByClassName('allower');
        tb[tab].click();
        console.log("ccc");
    }

    $(document).ready(function() {

        $.noConflict();
        $(".auto").click(function() {
            $("#hf_auto").val($(this).attr("id"));
            console.log($(this).attr("id"));
        });

        $(".auto").autocomplete({
            source: function(request, response) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "content/hr/addemployee.aspx/GetEmployee",
                    data: "{'term':'" + $(".auto").val() + "','sender':'" + $("#hf_auto").val() + "'}",
                    dataType: "json",
                    success: function(data) {
                        response($.map(data.d, function(item) {
                            return {
                                label: item.split('-')[1],
                                val: item.split('-')[0]
                            }
                        }))
                    },
                    error: function(result) {
                        alert(result.responseText);
                    }
                });
            },
            select: function(e, i) {
                index = $(".auto").parent().parent().index();
                $("#lbl_bals").val(i.item.val);
            }
        });

        /**ADMIN ACCESS**/
        var value = $("#empid").val();

        $(".allower").click(function(e) {
            $.ajax({
                type: "POST",
                url: "content/hr/addemployee.aspx/readUpdate",
                data: "{rid:" + $(this).val() + ",uid:" + value + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(data) {
                    var ru = document.getElementsByClassName("readupdate");
                    if (data.d) {
                        for (var i = 0; i < ru.length; i++) {
                            ru[i].setAttribute("style", "display:none;");
                        }
                    }
                    else {
                        for (var i = 0; i < ru.length; i++) {
                            ru[i].setAttribute("style", "display:block;");
                        }
                    }
                    console.log(data.d);
                },
                error: function(err, result) {
                    alert(err);
                }
            });

            var d = $(this).val();
            console.log(d);
            if (d == "1902" || d == "1901" || d == "1912") {
                $("#btn_submit").show();
                document.getElementById("btn_submit").style.display = "show";

                console.log("ddd");
            } else {
                $("#btn_submit").hide();
                console.log("eee");
            }

        });

        /**END**/

    });
</script>

<script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to cancel this transaction?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    } 
</script>

</asp:Content>

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<asp:HiddenField ID="hf_action" runat="server" />
<div class="page-title">
    <div class="title_left">
        <h3><asp:Label ID="l_pagetitle" runat="server" ></asp:Label></h3>
    </div>   
    <div class="title_right">
       <ul>
        <li><a href="masterfile"><i class="fa fa-users"></i> Master File</a></li>
        <li><i class="fa fa-angle-right"></i></li>
        <li><asp:Label ID="l_page" runat="server" ></asp:Label></li>
       </ul>
    </div>
</div>
<div class="clearfix"></div> 
<div class="row">

    <div id="pgridattainment" runat="server" visible="false" class="Overlay"></div>
    <div id="pgridattainment1" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton3" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li>
            <asp:dropdownlist id="ddllevel" runat="server">
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
            <li><asp:DropDownList ID="ddlupillness" runat="server"></asp:DropDownList></li><hr />
            <li><asp:DropDownList ID="ddluphost" runat="server"></asp:DropDownList></li><hr />
            <li><asp:TextBox ID="txtuphys" Placeholder="Physician" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="Button22" OnClick="updatemmedrecs" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>

    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x_content">
                <div class="col-md-3 col-sm-3 col-xs-12 no-padding">
                    <div class="profile_img img-hover">
                        <div id="dataURLView">
                            <img class="img-responsive avatar-view" src="files/profile/<%= profile.Value %>.png" onerror="this.onerror=null;this.src='files/profile/0.png'" title="Profile" style=" width:100%" />
                        </div>
                        <div class="middle">
                            <div class="docs-toolbar">
                                <label for="inputImage" title="Upload image file">
                                    <input class="hide" id="inputImage" name="file" type="file" accept="image/*">
                                    <i class="fa fa-camera text-white"></i>
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
                    
                    <% if (ViewState["action"].ToString() == "New")
                       { %>
                        <div class="box-header with-border margin-r-5">
                        </div>
                        <div class="box-left">
                            <asp:Button ID="btnupload" Visible="false" runat="server" Text="Mass Upload" CssClass="btn btn-primary" OnClick="massupload"/>
                        </div>
                    <% } %>
                    
                    
                </div>
                    
                <div class="col-md-9 col-sm-9 col-xs-12">
                    <div class="nav-tabs-custom">
                        <% string id = Session["role"].ToString() == "Admin" ? "0" : Session["emp_id"].ToString(); %>
                        <asp:TextBox ID="tab_index" runat="server" ClientIDMode="Static" Text="0"  CssClass="none" ></asp:TextBox>
                        <asp:TextBox ID="tbPage" runat="server" ClientIDMode="Static" Text="1901"  CssClass="none" ></asp:TextBox>
                        <input type="text" id="empid" style="display:none !important" value="<%=id %>"/>
                        <ul class="nav nav-tabs">
                         <% 
                           /**here oh Import Namespace="System.Data"**/
                           DataTable dt=dbhelper.getdata("Select route_id from nobel_userRight where user_id="+id);
                           DataRow[] dr = dt.Select("route_id=1901");
                           if (dr.Count() > 0)
                           {
                               if (dr[0]["route_id"].ToString() == "1901")
                               { %>
                           <li class="allower" value="1901" ><a href="#tab-profile" data-toggle="tab" aria-expanded="true">Profile</a></li>
                        <% }
                           }
                              
                           dr = dt.Select("route_id=1902");
                           if (dr.Count() > 0)
                           {
                               if (dr[0]["route_id"].ToString() == "1902")
                               { %>
                          <li class="allower" value="1902"><a href="#tab-payroll" data-toggle="tab" aria-expanded="false">Payroll</a></li>
                          <%}
                           }
                           
                           dr = dt.Select("route_id=1912");
                           if (dr.Count() > 0)
                           {
                               if (dr[0]["route_id"].ToString() == "1912")
                               { %>
                          <li class="allower" value="1912"><a href="#tab-compensation" data-toggle="tab" aria-expanded="false">Compensation</a></li>
                          <%}
                           }
                              
                           dr = dt.Select("route_id=1903");
                           if (dr.Count() > 0)
                           {
                               if (dr[0]["route_id"].ToString() == "1903")
                               { %>
                          <li class="allower" value="1903"><a href="#tab-requirment" data-toggle="tab" aria-expanded="false">Requirements</a></li>
                          <%}
                           }
                              
                           dr = dt.Select("route_id=1904");
                           if (dr.Count() > 0)
                           {
                               if (dr[0]["route_id"].ToString() == "1904")
                               { %>
                          <li class="allower" value="1904"><a href="#tab-suction" data-toggle="tab" aria-expanded="false">Disciplinary Management</a></li>
                          <%}
                           }
                              
                           dr = dt.Select("route_id=1905");
                           if (dr.Count() > 0)
                           {
                               if (dr[0]["route_id"].ToString() == "1905")
                               { %>
                          <li class="allower" value="1905"><a href="#tab-asset" data-toggle="tab" aria-expanded="false">Asset Management</a></li>
                          <%}
                           }
                              
                           dr = dt.Select("route_id=1906");
                           if (dr.Count() > 0)
                           {
                               if (dr[0]["route_id"].ToString() == "1906")
                               { %>
                          <li class="allower" value="1906"><a href="#tab-reportto" data-toggle="tab" aria-expanded="false" style='<%= ViewState["approver"].ToString() == "0" ? "color:red" : "" %>'>Reports To</a></li>
                          <%}
                           }
                              
                              
                           dr = dt.Select("route_id=1907");
                           if (dr.Count() > 0)
                           {
                               if (dr[0]["route_id"].ToString() == "1907")
                               { %>
                          <li class="allower" value="1907"><a href="#tab-leave" data-toggle="tab" aria-expanded="false" style='<%= ViewState["leave-credit"].ToString() == "0" ? "color:red" : "" %>'>Leave Credits</a></li>
                          <%}
                           } %>

                         <%-- <li style="display:none !important"  class=""><a href="">HMO</a></li>--%>
                          <%
                              dr = dt.Select("route_id=1908");
                              if (dr.Count() > 0)
                              {
                                  if (dr[0]["route_id"].ToString() == "1908")
                                  { %>
                          <li class="allower" value="1908"><a href="#tab-appraisal" data-toggle="tab" aria-expanded="false">Appraisal</a></li>
                          <%}
                              }
                              
                              dr = dt.Select("route_id=1909");
                              if (dr.Count() > 0)
                              {
                                  if (dr[0]["route_id"].ToString() == "1909")
                                  { %>
                          <li class="allower" value="1909"><a href="#tab-audit" data-toggle="tab" aria-expanded="false">Audit Trail</a></li>
                          <%}
                              }
                              
                              dr = dt.Select("route_id=1910");
                              if (dr.Count() > 0)
                              {
                                  if (dr[0]["route_id"].ToString() == "1910")
                                  { %>
                          <li class="allower" value="1910"><a href="#tab-status" data-toggle="tab" aria-expanded="false">Status</a></li>
                          <%}
                              }
                              
                              dr = dt.Select("route_id=1911");
                              if (dr.Count() > 0)
                              {
                                  if (dr[0]["route_id"].ToString() == "1911")
                                  { %>
                          <li class="allower" value="1911"><a href="#tab-note" data-toggle="tab" aria-expanded="false">Note</a></li>
                            <%}
                              }%>
                         <%--DEFAULT --%>
                          <li class="hide"><a href="#tab-profile" data-toggle="tab" aria-expanded="true">Profile</a></li>
                          <li class="hide"><a href="#tab-payroll" data-toggle="tab" aria-expanded="false">Payroll</a></li>
                          <li class="hide"><a href="#tab-compensation" data-toggle="tab" aria-expanded="false">Compensation</a></li>
                          <li class="hide"><a href="#tab-requirment" data-toggle="tab" aria-expanded="false">Requirements</a></li>
                          <li class="hide"><a href="#tab-suction" data-toggle="tab" aria-expanded="false">Discipline Management</a></li>
                          <li class="hide"><a href="#tab-asset" data-toggle="tab" aria-expanded="false">Asset</a></li>
                          <li class="hide"><a href="#tab-reportto" data-toggle="tab" aria-expanded="false" style='<%= ViewState["approver"].ToString() == "0" ? "color:red" : "" %>'>Report to</a></li>
                          <li class="hide"><a href="#tab-leave" data-toggle="tab" aria-expanded="false" style='<%= ViewState["leave-credit"].ToString() == "0" ? "color:red" : "" %>'>Leave Credit</a></li>
                          <li style="display:none !important"  class=""><a href="#tab-hmo" data-toggle="tab" aria-expanded="false">HMO</a></li>
                          <li class="hide"><a href="#tab-appraisal" data-toggle="tab" aria-expanded="false">Appraisal</a></li>
                          <li class="hide"><a href="#tab-audit" data-toggle="tab" aria-expanded="false">Audit Trail</a></li>
                          <li class="hide"><a href="#tab-status" data-toggle="tab" aria-expanded="false">Status</a></li>
                          <li class="hide"><a href="#tab-note" data-toggle="tab" aria-expanded="false">Note</a></li>
                          <%--END DEFAULT --%>
                        </ul>
                        <div class="tab-content">
                          <div id="alert" runat="server" visible="false" class="alert alert-danger alert-dismissible">
                            <h4><i class="icon fa fa-info-circle"></i> Alert!</h4>
                           <asp:Label ID="l_msg" runat="server" CssClass="block" style="position:inherit"></asp:Label>
                          </div>
                          <% 
                              dr = dt.Select("route_id=1901");
                              if (dr.Count() > 0)
                              {
                                  if (dr[0]["route_id"].ToString() == "1901")
                                  { 
                          %>
                
                

                          <div class="tab-pane" id="tab-profile">
                            <h4 class="page-header">Employee's Identity</h4>
                            <div class="row">
                                <div class="col-lg-2">
                                    <div class="form-group">
                                      <label>ID Number <span class="text-red">*</span></label>
                                      <asp:TextBox ID="txt_idnumber" AutoComplete="off" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="form-group">
                                        <label>SAP Number <span class="text-red">*</span></label>
                                        <asp:TextBox ID="txt_sapnumber" AutoComplete="off" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label>Company</label>
                                        <span class="text-red">*</span>
                                        <asp:dropdownlist ID="ddl_company" AutoComplete="off" runat="server" ></asp:dropdownlist>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label>Location</label>
                                        <asp:dropdownlist ID="ddl_branch" AutoComplete="off" runat="server" DataTextField="Department" CssClass="minimal form-control" ></asp:dropdownlist>
                                    </div>
                                </div>
                                <div class="col-lg-4 none">
                                    <div class="form-group">
                                        <label>Outlet </label>
                                        <asp:dropdownlist ID="ddl_store" runat="server" AutoComplete="off" CssClass="form-control" >
                                        </asp:dropdownlist>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label>Department <span class="text-red">*</span></label>
                                        <asp:dropdownlist ID="ddl_department" AutoComplete="off" runat="server" DataTextField="Department" DataValueField="id" AppendDataBoundItems="true"  CssClass="form-control select2"></asp:dropdownlist>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label>Section</label>
                                        <asp:dropdownlist ID="ddl_section" AutoComplete="off" DataTextField="secode" DataValueField="sectionid" runat="server" CssClass="select2"></asp:dropdownlist>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label>Internal Order <span class="text-red">*</span></label>
                                        <asp:DropDownList ID="ddl_internalorder" AutoComplete="off" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                             </div>
                            <div class="row">
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label>Position</label>
                                        <asp:dropdownlist ID="ddl_position" AutoComplete="off" runat="server" CssClass="select2"></asp:dropdownlist>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label>Band/Level <span class="text-red">*</span></label>
                                        <asp:dropdownlist ID="ddl_divission" AutoComplete="off" runat="server" CssClass="form-control"></asp:dropdownlist>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label>Payroll Group <span class="text-red">*</span></label>
                                        <asp:dropdownlist ID="ddl_pg" AutoComplete="off" runat="server"></asp:dropdownlist>
                                    </div>
                                </div>
                             </div>
                            <div class="row">
                                <div class="col-lg-2">
                                    <div class="form-group">
                                        <label>Role <span class="text-red">*</span></label>
                                        <asp:dropdownlist ID="ddl_lvel" AutoComplete="off" runat="server"></asp:dropdownlist>
                                    </div>
                                </div>
								<div class="col-lg-2">
                                    <div class="form-group">
                                        <label>Level </label>
                                        <asp:dropdownlist ID="ddl_divission2" AutoComplete="off" runat="server"></asp:dropdownlist>
                                    </div>
                                </div>
                                <div class="col-lg-4 none">
                                    <div class="form-group">
                                        <label>SBU Date</label>
                                        <asp:TextBox ID="txt_sbudate"  runat="server" CssClass="form-control datee" ></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label>Date Hired <span class="text-red">*</span></label>
                                        <asp:TextBox ID="txt_datehired"  runat="server" CssClass="form-control datee" ></asp:TextBox>
                                    </div>
                                </div>
                                 <div class="col-lg-4">
                                    <div class="form-group">
                                        <label>Status <span class="text-red">*</span></label>
                                        <asp:dropdownlist ID="ddl_status" AutoComplete="off" runat="server" CssClass="form-control"></asp:dropdownlist>
                                    </div>
                                </div>
                             </div>
                            <h4 class="page-header">Personal Information</h4>
                            <div class="row">
								 <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>Last Name</label>
                                        <span class="text-red">*</span>
                                        <asp:TextBox ID="txt_lname" AutoComplete="off" runat="server" ClientIDMode="Static"  CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>First Name</label>
                                        <span class="text-red">*</span>
                                        <asp:TextBox ID="txt_fname" AutoComplete="off" runat="server" ClientIDMode="Static" onclick="hh()" CssClass="form-control" ></asp:TextBox>
                                    </div>
                                </div>
                               <div class="col-lg-3">
                                    <div class="form-group">
                                    <label>Extension Name</label>
                                    <asp:TextBox ID="txt_exname" AutoComplete="off" runat="server" ClientIDMode="Static"  CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                    <label>Middle Name</label>
                                    <asp:TextBox ID="txt_mname" AutoComplete="off" runat="server" ClientIDMode="Static"  CssClass="form-control"></asp:TextBox>
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
                                        <label>Date of Birth <span class="text-red">*</span></label>
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
                                        <asp:dropdownlist ID="ddl_cs" AutoComplete="off" runat="server">
                                        </asp:dropdownlist>
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
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label>Personal Email</label>
                                        <asp:TextBox ID="txt_pemail" AutoComplete="off" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-3 none">
                                    <div class="form-group">
                                        <label>Health Card Expiration</label>
                                        <asp:TextBox ID="txt_health" AutoComplete="off" runat="server" CssClass="form-control datee"></asp:TextBox>
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
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbeditor" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="openattainement" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                                            <asp:LinkButton ID="cancel" runat="server" CommandName='<%# Eval("Id") %>' ToolTip="Delete" OnClick="delete_edhistory" OnClientClick="Confirm()"><i class="fa fa-trash"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="80px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <button type="button" class="btn btn-default" data-toggle="modal" data-target="#modal-attainment">Add</button>

                            <h4 class="page-header">Job History</h4>
                            <asp:GridView ID="grid_jobhistory1" runat="server" EmptyDataText="No Data Found!"   AutoGenerateColumns="False"  CssClass="table table-striped table-bordered">
                                <Columns>
                                    <%--<asp:TemplateField ItemStyle-Width="15px"> 
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1%>.
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:BoundField DataField="id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone" />
                                    <asp:BoundField DataField="position" HeaderText="Position"  />
                                    <asp:BoundField DataField="company" HeaderText="Company"  />
                                    <asp:BoundField DataField="froms" HeaderText="Date From"  />
                                    <asp:BoundField DataField="tos" HeaderText="Date To"  />
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbeditor" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="openjobhist" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                                            <asp:LinkButton ID="cancel" runat="server" CommandName='<%# Eval("Id") %>' ToolTip="Delete" OnClientClick="Confirm()" OnClick="delete_jobhistory"><i class="fa fa-trash"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="80px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <button type="button" class="btn btn-default readupdate" data-toggle="modal" data-target="#modal-jobhistory">Add</button>
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
                                            <asp:LinkButton ID="cancel" runat="server" CommandName='<%# Eval("Id") %>' ToolTip="Delete" OnClientClick="Confirm()" OnClick="deleteseminarsattended"><i class="fa fa-trash"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="130px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <button type="button" class="btn btn-default readupdate" data-toggle="modal" data-target="#modal-seminar">Add</button>

                            <h4 class="page-header">Training Attended</h4>
                             <asp:GridView ID="grid_preparatory" runat="server" EmptyDataText="No Data Found!" AutoGenerateColumns="False" CssClass="table table-striped table-bordered">
                              <Columns>
                                <asp:BoundField DataField="Id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone" />
                                <asp:TemplateField HeaderText="License Number">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnprep" runat="server" CommandName='<%# Eval("Id") %>' ToolTip="List" Text='<%# Eval("LicenseNum") %>' OnClick="opendownloadprep"></asp:LinkButton>
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
                                <asp:TemplateField HeaderText="Action"  ItemStyle-CssClass="readupdate" HeaderStyle-CssClass="readupdate" >
                                 <ItemTemplate>
                                    <asp:LinkButton ID="lbeditor" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="opentraining" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                                    <asp:LinkButton ID="cancel" runat="server" CommandName='<%# Eval("Id") %>' ToolTip="Delete" OnClientClick="Confirm()" OnClick="cancelprep"><i class="fa fa-trash"></i></asp:LinkButton>
                                 </ItemTemplate>
                                 <ItemStyle Width="100px" />
                                </asp:TemplateField>
                              </Columns>
                            </asp:GridView>
                            <button type="button" class="btn btn-default readupdate" data-toggle="modal" data-target="#modal-preparatory">Add</button>

                            <h4 class="page-header">Employee Special Skills</h4>
                            <asp:GridView ID="grid_skill1" runat="server" EmptyDataText="No Data Found!"  AutoGenerateColumns="False"  CssClass="table table-striped table-bordered">
                                <Columns>
                                        <asp:BoundField DataField="ID" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                        <asp:BoundField DataField="empid" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                        <asp:BoundField DataField="skill" HeaderText="Skill" />
                                        <asp:TemplateField ShowHeader="False">
                                          <ItemTemplate>
                                            <asp:LinkButton ID="lbeditor" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="openskills" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkbtndel" runat="server" CommandName='<%# Eval("Id") %>' Text="Remove" OnClientClick="Confirm()"  ClientIDMode="Static" OnClick="deleteskill" style=" margin:3px;" ><i class="fa fa-trash"></i></asp:LinkButton>
                                          </ItemTemplate>
                                        <ItemStyle Width="80px" />
                                        </asp:TemplateField>
                                </Columns>
                                </asp:GridView>
                            <button type="button" class="btn btn-default readupdate" data-toggle="modal" data-target="#modal-skill">Add</button>

                            <h4 class="page-header">Emergency Contact</h4>
                                <asp:GridView ID="grid_emergencycontact" runat="server" EmptyDataText="No Data Found!"  AutoGenerateColumns="False"  CssClass="table table-striped table-bordered">
                                  <Columns>
                                        <asp:BoundField DataField="Id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                        <asp:BoundField DataField="EmpId" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                        <asp:BoundField DataField="Name" HeaderText="Name" />
                                        <asp:BoundField DataField="Address" HeaderText="Address" />
                                        <asp:BoundField DataField="Contact" HeaderText="Contact Number" />
                                       <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbeditor" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="openemcontact" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkbtndel" runat="server" CommandName='<%# Eval("Id") %>' Text="Remove" OnClientClick="Confirm()"  ClientIDMode="Static" OnClick="deleteContact" style=" margin:3px;" ><i class="fa fa-trash"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="80px" />
                                        </asp:TemplateField>
                                  </Columns>
                                </asp:GridView>
                            <button type="button" class="btn btn-default readupdate" data-toggle="modal" data-target="#modal-emergency">Add</button>

                            <h4 class="page-header">Medical History</h4>
                            <asp:GridView ID="gridmedrics" OnRowDataBound="medrecscell" EmptyDataText="No Data Found!" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
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
                            <button type="button" class="btn btn-default readupdate" data-toggle="modal" data-target="#modal-medical">Add</button>

                            <h4 class="page-header">Set Up</h4>
                                <div class="form-group none">
                                    <asp:CheckBox ID="chk_mwe" runat="server" Text="Minimum Wage Earner" CssClass="checkbox"></asp:CheckBox>
                                </div>
                                <div class="form-group none">
                                    <asp:CheckBox ID="chk_allowoffset" runat="server" Text="Overtime Offsets"  CssClass="checkbox"></asp:CheckBox>
                                </div>
                                <div class="form-group none">
                                    <asp:CheckBox ID="chk_allowhdoffset" runat="server" Text="Holiday Offsets"  CssClass="checkbox"></asp:CheckBox>
                                </div>
                                <div class="form-group none">
                                    <asp:CheckBox ID="chk_allow_sc" runat="server" Text="Service Charge"  CssClass="checkbox"></asp:CheckBox>
                                </div>
                                <div class="form-group">
                                <asp:CheckBox ID="chk_imw" runat="server" Text="MWEs" CssClass="checkbox"/>
                                <asp:CheckBox ID="chk_l" runat="server" style="display:none;" Text="Leave With Pay"/>
                                </div>
                                <div class="form-group">
                                <asp:CheckBox ID="chk_allowot" runat="server" Text="Allow OT" CssClass="checkbox"/>
                                </div>
                                <div class="form-group">
                                <asp:CheckBox ID="chk_nonpunch" runat="server" Text="None Punching" CssClass="checkbox"/>
                                </div>
                                <div class="form-group">
                                <asp:CheckBox ID="chk_otmeal" runat="server" Text="Overtime Meal Allowance" CssClass="checkbox"/>
                                </div>
                                <div class="form-group">
                                <asp:CheckBox ID="chk_access" runat="server" Text="Allow Access" CssClass="checkbox"/>
                                </div>
                                <div class="form-group">
                                <asp:CheckBox ID="chk_nhs" runat="server" Text="New Hired" CssClass="checkbox"/>
                                </div>
                                <div class="form-group none">
                                <asp:CheckBox ID="chk_cola" runat="server" Text="ECOLA" CssClass="checkbox"/>
                                </div>
                                <div class="form-group none">
                                     <label>Mode of Payment</label>
                                     <asp:DropDownList ID="ddl_mop" runat="server">
                                        <asp:ListItem>Select</asp:ListItem>
                                     <asp:ListItem>CHEQUE</asp:ListItem>
                                     <asp:ListItem>BANK</asp:ListItem>
                                     </asp:DropDownList>
                                </div>
                                 <div class="form-group none">
                                     <label>Allow Surge Pay</label>
                                       <asp:TextBox ID="txt_surge_effective_date" placeholder="Effective Date"  runat="server" CssClass="form-control datee" ></asp:TextBox>
                                    
                                </div>
                                <%if (Request.QueryString["tp"].ToString() == "dd")
                                  { %>
                                <div class="form-group">
                                    <label>Attachment</label>
                                    <asp:FileUpload ID="FileUpload1"  runat="server" multiple/>
                                </div>
                                <%} %>


                          </div>
                          <%      }
                              } 
                          %>
                         



                          
                          <!--1902**************************************************-->
                          <% 
                              dr = dt.Select("route_id=1902");
                              if (dr.Count() > 0)
                              {
                                  if (dr[0]["route_id"].ToString() == "1902")
                                  { 
                          %>
                        <div class="tab-pane" id="tab-payroll">
                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="form-group">
                                        <label>Shift Code <span class="text-red">*</span></label>
                                        <asp:dropdownlist ID="ddl_shiftcode" AutoComplete="off" runat="server" style=" width:100% !important"></asp:dropdownlist>
                                    </div>
                                    <div class="form-group">
                                        <label>Tax Code <span class="text-red">*</span></label>
                                        <asp:dropdownlist ID="ddl_taxcode" AutoComplete="off" runat="server"></asp:dropdownlist>
                                    </div>
                                    <div class="form-group none">
                                        <label>GL Account <span class="text-red">*</span></label>
                                        <asp:dropdownlist ID="ddl_gl" AutoComplete="off" runat="server"></asp:dropdownlist>
                                    </div>
                                    <div class="form-group">
                                        <label>ATM</label>
                                        <asp:TextBox ID="txt_atm" AutoComplete="off" runat="server" CssClass="form-control" onkeyup="intinput(this)"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label>SSS Add-on </label>
                                        <asp:TextBox ID="txt_sssaddon" AutoComplete="off" runat="server" CssClass="form-control" onkeyup="decimalinput(this)"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label>HDMF Add-on </label>
                                        <asp:TextBox ID="txt_hdmfaddon" AutoComplete="off" runat="server" CssClass="form-control" onkeyup="decimalinput(this)"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-6">
                                    <div class="form-group">
                                        <label>SSS </label>
                                        <asp:TextBox ID="txt_sssno" AutoComplete="off" runat="server" CssClass="form-control" onkeyup="intinput(this)"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label>HDMF </label>
                                        <asp:TextBox ID="txt_hdmfno" AutoComplete="off" runat="server" CssClass="form-control" onkeyup="intinput(this)"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label>PHIC </label>
                                        <asp:TextBox ID="txt_phicno" AutoComplete="off" runat="server" CssClass="form-control" onkeyup="intinput(this)"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label>TIN </label>
                                        <asp:TextBox ID="txt_tin" AutoComplete="off" runat="server" CssClass="form-control" onkeyup="intinput(this)" oncopy="return false;" onpaste="return false;" oncut="return false;"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label>GSIS </label>
                                        <asp:TextBox ID="txt_gsisno" AutoComplete="off" runat="server" CssClass="form-control" onkeyup="intinput(this)"></asp:TextBox>
                                    </div>   
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="form-group none">
                                        <label>Meal</label>
                                        <asp:TextBox ID="txt_meal_allow" runat="server" CssClass="form-control" onkeyup="decimalinput(this);" AutoComplete="off"  ClientIDMode="Static" oncopy="return false;" onpaste="return false;" oncut="return false;"></asp:TextBox>
                                    </div>
                                </div>                
                            </div>
                        </div>
                          <%      }
                              } 
                          %>

                          <!--1912****************************************-->
                          <% 
                              dr = dt.Select("route_id=1912");
                              if (dr.Count() > 0)
                              {
                                  if (dr[0]["route_id"].ToString() == "1912")
                                  { 
                          %>
                        <div class="tab-pane" id="tab-compensation">
                            <div class="row">
                                <div class="col-lg-6">
                                    <div class="form-group">
                                        <label>Payroll Setup <span class="text-red">*</span></label>
                                        <asp:dropdownlist ID="ddl_payrolltype" AutoComplete="off" OnTextChanged="click_paytype" ClientIDMode="Static" runat="server"></asp:dropdownlist>
                                    </div>
                                    <div class="form-group">
                                        <label>Fix No of Days (in a year) <span class="text-red">*</span></label>
                                        <asp:dropdownlist ID="txt_fnod" AutoComplete="off" runat="server" ClientIDMode="Static">
                                            <asp:ListItem Value="313">313</asp:ListItem>
                                            <asp:ListItem Value="365">365</asp:ListItem>
                                            <asp:ListItem Value="312">312</asp:ListItem>
                                             <asp:ListItem Value="264">264</asp:ListItem>
                                        </asp:dropdownlist>
                                    </div>
                                    <div class="form-group">
                                            <label>Payroll Type <span class="text-red">*</span></label>
                                            <asp:dropdownlist ID="ddl_taxtable" AutoComplete="off" onchange="addcompute()" ClientIDMode="Static" runat="server">
                                                <asp:ListItem>Semi-Monthly</asp:ListItem>
                                            </asp:dropdownlist>
                                    </div>
                                    <div class="form-group">
                                        <label>Fix No of Hours <span class="text-red">*</span></label>
                                        <asp:TextBox ID="txt_fnoh" Text="8" AutoComplete="off" runat="server" ClientIDMode="Static" oncopy="return false;" onpaste="return false;" oncut="return false;"  onkeyup="decimalinput(this);addcompute();" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="form-group none">
                                        <label>Gross Monthly Rate <span class="text-red">*</span></label>
                                        <asp:TextBox ID="tb_gmr" AutoComplete="off" runat="server" oncopy="return false;" onpaste="return false;" oncut="return false;"  CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="form-group none">
                                       <label>Non Taxable Income<span class="text-red">*</span></label>
                                       <asp:TextBox ID="tb_ntaca" AutoComplete="off" runat="server"  oncopy="return false;" onpaste="return false;" oncut="return false;"  CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="form-group none">
                                        <label>Absent Daily Rate</label>
                                        <asp:TextBox ID="txt_adr" runat="server" CssClass="form-control" AutoComplete="off" onfocus="this.blur()"></asp:TextBox> 
                                    </div>
                                    <div class="form-group none">
                                      <label>Non Tax Income</label>
                                        <asp:TextBox ID="txt_nti" runat="server" CssClass="form-control"  AutoComplete="off" ClientIDMode="Static"></asp:TextBox>
                                      </div>
                                </div>
                                <div class="col-lg-4">
                                     <div class="form-group none">
                                        <label>Night Hourly Rate</label>
                                        <asp:TextBox ID="txt_nhr" runat="server" CssClass="form-control"  AutoComplete="off" onfocus="this.blur()"></asp:TextBox> 
                                    </div>
                                    <div class="form-group none">
                                        <label>Overtime Hourly Rate</label>
                                        <asp:TextBox ID="txt_ohr" runat="server" CssClass="form-control" AutoComplete="off" onfocus="this.blur()"></asp:TextBox> 
                                    </div>
                                    <div class="form-group none">
                                        <label>Overtime Night Hourly Rate</label>
                                        <asp:TextBox ID="txt_onhr" runat="server" CssClass="form-control" AutoComplete="off" onfocus="this.blur()"></asp:TextBox> 
                                    </div>
                                    <div class="form-group none">
                                        <label>Tardy Hourly Rate</label> 
                                        <asp:TextBox ID="txt_thr" runat="server" CssClass="form-control" onfocus="this.blur()" AutoComplete="off" ></asp:TextBox> 
                                    </div>
                                </div>
                                <div class="col-lg-6">
                                    <div class="form-group">
                                        <label>Monthly Rate<span class="text-red">*</span></label>
                                        <asp:TextBox ID="txt_mr" AutoComplete="off" runat="server" ClientIDMode="Static" oncopy="return false;" onpaste="return false;" oncut="return false;"  onkeyup="decimalinput(this);addcompute();" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="form-group none">
                                       <label>Non Taxable Night Shift Differential (reflect ni cya sa iya rate)<span class="text-red">*</span></label>
                                       <asp:TextBox ID="tb_ntnsd" AutoComplete="off" runat="server"  oncopy="return false;" onpaste="return false;" oncut="return false;"   CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="form-group none">
                                       <label>Non Taxable Allowance(reflect ni cya sa iya rate)<span class="text-red">*</span></label>
                                       <asp:TextBox ID="tb_nta" AutoComplete="off" runat="server"  oncopy="return false;" onpaste="return false;" oncut="return false;"   CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label>SEMI - MONTHLY INCOME <span class="text-red">*</span></label>
                                        <asp:TextBox ID="txt_pr" AutoComplete="off" runat="server" ClientIDMode="Static" onfocus="this.blur()"  oncopy="return false;" onpaste="return false;" oncut="return false;"  onkeyup="decimalinput(this);" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label>Daily Rate <span class="text-red">*</span></label>
                                        <asp:TextBox ID="txt_dr" runat="server"  AutoComplete="off" CssClass="form-control" onclick="this.select();" oncopy="return false;" onpaste="return false;" oncut="return false;"  ClientIDMode="Static" onkeyup="decimalinput(this);addcompute();"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                         <label>Hourly Rate <span class="text-red">*</span></label>
                                         <asp:TextBox ID="txt_hr" runat="server" AutoComplete="off" CssClass="form-control" onfocus="this.blur()" ClientIDMode="Static" oncopy="return false;" onpaste="return false;" oncut="return false;"  ></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                          </div>
                            <%      }
                              } 
                          %>


                          <!--1903**************************************************-->
                          <% 
                              dr = dt.Select("route_id=1903");
                              if (dr.Count() > 0)
                              {
                                  if (dr[0]["route_id"].ToString() == "1903")
                                  { 
                          %>
                          <div class="tab-pane" id="tab-requirment">
                              <asp:GridView ID="grid_reqlistcheck" EmptyDataText="No Data Found!" OnRowDataBound="kanakanaas" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                                    <Columns>
                                        <asp:BoundField DataField="cnt" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone" />
                                        <asp:TemplateField ItemStyle-Width="10px">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbrequirements" runat="server" OnCheckedChanged="selectchange" />
                                            <asp:LinkButton ID="lbviewreq" runat="server" CommandName='<%# Eval("id") %>' ToolTip="List" Text='<%# Eval("description") %>' OnClick="viewdetails"></asp:LinkButton>
                                        
                                        </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <button type="button" class="btn btn-default readupdate" data-toggle="modal" data-target="#modal-requirement">Add</button>
                                <div class="modal fade" id="modal-requirement">
                                    <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span></button>
                                        <h4 class="modal-title">Add Requirement</h4>
                                        </div>
                                        <div class="modal-body">
                                            <div class="form-group">
                                                <label>REQUIREMENTS</label> <asp:Label ID="lbl_emp" runat="server" ForeColor="Red"></asp:Label>
                                                <div id="divemp" runat="server">
                                                    <asp:DropDownList ID="ddl_reqlist" runat="server" >
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label>Description</label> <asp:Label ID="lbl_desc" runat="server" ForeColor="Red"></asp:Label>
                                                <asp:TextBox ID="txt_desc" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <ul class="input-form">
                                                <li>Source File<asp:Label ID="lbl_sf" runat="server" ForeColor="Red"></asp:Label></li>
                                                <li><asp:FileUpload ID="fuempreq" multiple runat="server" /></li>
                                            </ul>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button ID="Button3" runat="server" OnClick="clickupload" Text="Save" CssClass="btn btn-primary" />
                                        </div>
                                    </div>
                                    </div>
                                </div>

                          </div>
                           <%      }
                              } 
                          %>
                          <!--1904**************************************************-->
                          <% 
                              dr = dt.Select("route_id=1904");
                              if (dr.Count() > 0)
                              {
                                  if (dr[0]["route_id"].ToString() == "1904")
                                  { 
                          %>
                          <div class="tab-pane" id="tab-suction">
                            <asp:GridView ID="grid_sanctions" OnRowDataBound="tawesie" runat="server" EmptyDataText="No Data Found!" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                                <Columns>
                                    <asp:BoundField DataField="id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                    <asp:BoundField DataField="incidentdate" HeaderText="Date" />
                                    <asp:TemplateField HeaderText="Root Cause">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbirlistfiles" runat="server" CommandName='<%# Eval("id") %>' ToolTip="List" Text='<%# Eval("rootcause") %>' OnClick="incidentreports"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="remarks" HeaderText="Remarks" />
                                    <asp:BoundField DataField="nodays" HeaderText="Day/s" />
                                    <asp:TemplateField  HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="itemstatus" runat="server" OnClick="chngestatus" Text='<%# bind("status")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" ShowHeader="False" ItemStyle-CssClass="readupdate" HeaderStyle-CssClass="readupdate">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbupload" ToolTip="Verify" runat="server" Text="uploadirfile" Visible="true" OnClick="irfilesupload"><i class="fa fa-paperclip"></i></asp:LinkButton>
                                            <asp:linkbutton ID ="lbdonwloadir" ToolTip="Download" CommandName='<%# Eval("id") %>' Visible="false" runat="server" OnClick="downloadirfiles"><i class="fa fa-download"></i></asp:linkbutton>
                                            <asp:LinkButton ID ="trash" ToolTip="Delete" runat="server" CausesValidation="false" OnClick="deletesanctions"><i class="fa fa-trash"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
				    <asp:BoundField DataField="userid" HeaderText="Issued" />
                                </Columns>
                            </asp:GridView>
                            <button type="button" class="btn btn-default readupdate" data-toggle="modal" data-target="#modal-sunction">Add</button>
                            <div class="modal fade" id="modal-sunction">
                                <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span></button>
                                    <h4 class="modal-title">Add Sanction</h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-group">
                                            <label>Incident Date</label>
                                            <asp:TextBox ID="txt_incidentdate" runat="server" CssClass="form-control datee"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label>Incident About</label>
                                            <asp:TextBox ID="txt_incidentabout" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>

                                        <asp:ScriptManager ID="ScriptManager1"  runat="server" />
                                        <asp:UpdatePanel ID="uPnl_sanction" runat="server">
                                            <ContentTemplate>
                
                                            <div class="form-group">
                                                <label>Sanction</label>
                                                <asp:dropdownlist ID="ddlsanctioncodes" runat="server" AutoComplete="off" AutoPostBack="true" OnSelectedIndexChanged="ddlvalue_SelectedIndexChanged" >
                                                    <asp:ListItem ></asp:ListItem>
                                                    <asp:ListItem Value="1">Counselling</asp:ListItem>
                                                    <asp:ListItem Value="2">Verbal Reprimand</asp:ListItem>
                                                    <asp:ListItem Value="3">Written Warning</asp:ListItem>
                                                    <asp:ListItem Value="4">Final Warning</asp:ListItem>
                                                    <asp:ListItem Value="5">1 Day Suspension</asp:ListItem>
                                                    <asp:ListItem Value="6">3 Days Suspension</asp:ListItem>
                                                    <asp:ListItem Value="7">7 Days Suspension</asp:ListItem>
                                                    <asp:ListItem Value="8">15 Days Suspension</asp:ListItem>
                                                    <asp:ListItem Value="9">30 Days Preventive Suspension</asp:ListItem>
                                                    <asp:ListItem Value="10">Dismissal</asp:ListItem>
                                                </asp:dropdownlist>
                                            </div>
                                            <asp:Panel ID="pnl_SacnOption" runat="server" Visible="false">
                                                <div class="form-group">
                                                    <label>Suspension Date</label>
                                                    <asp:Label ID="lblsusdate" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>

                                                    <div class="row">
                                                        <div class="col-lg-10">
                                                            <asp:TextBox ID="txtsuspend" runat="server" CssClass="form-control" type="Date" AutoComplete="Off"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-2">
                                                            <asp:Button ID="btnsusdate" runat="server" OnClick="addsusdate" Text="ADD" CssClass="btn btn-primary btn-block" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <asp:GridView ID="susdate" AutoGenerateColumns="false" runat="server" OnRowDeleting="RowDeleting" ShowHeader="false" CssClass="table table-striped table-bordered">
                                                    <Columns>
                                                        <asp:BoundField DataField="suspendate" HeaderText="Suspension Date" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lb_del" runat="server" CssClass="fa fa-minus-circle" CommandName="Delete"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

            

                                        <div class="form-group">
                                            <label>Remarks</label>
                                            <asp:TextBox ID="txt_incidentremarks" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnaddsanctions" runat="server" OnClick="addsanctions" Text="ADD" CssClass="btn btn-primary" />
                                    </div>
                                </div>
                                </div>
                            </div>
                          </div>
                          <%      }
                              } 
                          %>
                          <!--1905**************************************************-->
                          <% 
                              dr = dt.Select("route_id=1905");
                              if (dr.Count() > 0)
                              {
                                  if (dr[0]["route_id"].ToString() == "1905")
                                  { 
                          %>
                          <div class="tab-pane" id="tab-asset">
                              <asp:GridView ID="grid_asset" runat="server" EmptyDataText="No Data Found!" AutoGenerateColumns="False" CssClass="table table-striped table-bordered">
                                <Columns>
                                    <asp:BoundField DataField="id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone" />
                                    <asp:BoundField DataField="cat" HeaderText="Category" />
                                    <asp:BoundField DataField="name" HeaderText="Description" />
                                    <asp:BoundField DataField="qty" HeaderText="QTY" />
                                      <asp:BoundField DataField="um" HeaderText="UM" />
                                    <asp:TemplateField  HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="return_item" runat="server" CommandName='<%# Eval("serialno") %>' ToolTip="Return" OnClick="return_asset_assign"><i class="fa fa-forward"></i></asp:LinkButton>
                                            <asp:LinkButton ID="can" runat="server" CommandName='<%# Eval("serialno") %>' ToolTip="Cancel" OnClick="delete_asset_assign"><i class="fa fa-trash   "></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="5px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <button type="button" class="btn btn-default readupdate" data-toggle="modal" data-target="#modal-asset">Add</button>
                            <div class="modal fade" id="modal-asset" >
                                <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span></button>
                                    <h4 class="modal-title">Assign Asset</h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-group">
                                            <label>Category</label>
                                            <asp:dropdownlist ID="ddl_cat" runat="server" AutoPostBack="true" OnTextChanged="select_category"></asp:dropdownlist>
                                        </div>
                                        <div class="form-group">
                                            <label>Serial</label>
                                            <asp:dropdownlist ID="txt_ser" runat="server" OnTextChanged="select_category"></asp:dropdownlist>
                                        </div>
                                        <div class="form-group no-margin">
                                            <label>Quantity</label>
                                            <asp:TextBox ID="txt_quantity" ClientIDMode="Static" onkeyup="decimalinput(this);" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="Button4" runat="server" OnClick="asset_assign" Text="ADD" CssClass="btn btn-primary" />
                                    </div>
                                </div>
                                </div>
                            </div>
                          </div>
                           <%      }
                              } 
                          %>
                          <!--1906**************************************************-->
                          <% 
                              dr = dt.Select("route_id=1906");
                              if (dr.Count() > 0)
                              {
                                  if (dr[0]["route_id"].ToString() == "1906")
                                  { 
                          %>
                          <div class="tab-pane" id="tab-reportto">
                            <asp:GridView ID="gridreport" runat="server" EmptyDataText="No Data Found!"  AutoGenerateColumns="False" CssClass="table table-striped table-bordered">
                                <Columns>
                                    <asp:BoundField DataField="ID" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                    <asp:BoundField DataField="herarchy" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                    <asp:BoundField DataField="empID" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                    <asp:BoundField DataField="seqs" HeaderText="No." ItemStyle-Width="10px"/>
                                    <asp:BoundField DataField="idnumber" HeaderText="ID Number" />
                                    <asp:BoundField DataField="employee" HeaderText="Employee" />
                                    <asp:BoundField DataField="role" HeaderText="Role" />
                                    <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="readupdate" HeaderStyle-CssClass="readupdate">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkUp" CommandArgument = "up" runat="server" Text="&#x25B2;" OnClick="ChangePreference" />
                                            <asp:LinkButton ID="lnkDown" CommandArgument = "down" runat="server" Text="&#x25BC;" OnClick="ChangePreference" />
                                            <asp:ImageButton ID="can" Visible="false" runat="server" CausesValidation="false" OnClick="delete_rt" ImageUrl="~/style/img/delete.png" style="margin-left:5px" />
                                        </ItemTemplate>
                                        <ItemStyle Width="105" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <button type="button" class="btn btn-default readupdate" data-toggle="modal" data-target="#modal-reportto">Add</button>
                            <div class="modal fade" id="modal-reportto" >
                                <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span></button>
                                    <h4 class="modal-title">Set Reports To</h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="form-group">
                                            <asp:CheckBox ID="chck_scheduler" runat="server" Text="Scheduler" CssClass="checkbox"></asp:CheckBox>
                                        </div>
                                        <div class="form-group">
                                            <label>Employee</label>
                                            <asp:TextBox ID="txt_reportto" runat="server" ClientIDMode="Static" CssClass="form-control auto"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="Button6" runat="server" OnClick="rt" Text="ADD" CssClass="btn btn-primary" />
                                    </div>
                                </div>
                                </div>
                            </div>
                          </div>
                           <%      }
                              } 
                          %>
                          <!--1907**************************************************-->
                          <% 
                              dr = dt.Select("route_id=1907");
                              if (dr.Count() > 0)
                              {
                                  if (dr[0]["route_id"].ToString() == "1907")
                                  { 
                          %>
                          <div class="tab-pane" id="tab-leave">
                          <asp:GridView ID="grid_leavecredits" runat="server" EmptyDataText="No Data Found!"  AutoGenerateColumns="False"  CssClass="table table-striped table-bordered">
                                <Columns>
                                    <asp:BoundField DataField="id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone" />
                                    <asp:BoundField DataField="leave" HeaderText="Leave Type"  />
                                    <asp:BoundField DataField="credit" HeaderText="Total Credits"  />
                                    <asp:BoundField DataField="Balance" HeaderText="Balance"  />
                                    <asp:BoundField DataField="convertocash" HeaderText="Convert to Cash"  />
                                    <asp:BoundField DataField="yyyyear" HeaderText="Year"  />
                                    <asp:TemplateField ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone" >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbEdit" runat="server" CssClass="fa fa-trash" OnClick="delete_lc" Font-Size="12px"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="5px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                             <div class="modal-footer">
                                <asp:Button ID="btn_addleave" runat="server" Text="View" OnClick="adminsideleave" CssClass="btn btn-primary" />
                            </div>
                          <button type="button" class="btn btn-default readupdate" data-toggle="modal" data-target="#modal-leave">Add</button>
                        <div class="modal fade" id="modal-leave" >
                            <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">Leave Credit Setup</h4>
                                </div>
                                <div class="modal-body">
                                    <div class="form-group">
                                        <label>Leave Type</label>
                                        <asp:DropDownList ID="ddl_leave" runat="server"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label>Allowed Number of Days</label>
                                        <asp:TextBox ID="txt_noofcredits" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label>Year</label>
                                        <asp:DropDownList ID="ddl_yyyy" runat="server"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="chk_tocash" runat="server" Text="Convert to Cash" />
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="Button11" runat="server" OnClick="lc" Text="Submit" CssClass="btn btn-primary" />
                                </div>
                                <asp:DropDownList ID="ddl_renew" runat="server" CssClass="none">
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem>Yearly</asp:ListItem>
                                    <asp:ListItem>Monthly</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            </div>
                        </div>
                          </div>
                           <%      }
                              } 
                          %>
                          <!--NOT INCLUDED**************************************************-->
                          <% 
                              dr = dt.Select("route_id=1900");
                              if (dr.Count() > 0)
                              {
                                  if (dr[0]["route_id"].ToString() == "1900")
                                  { 
                          %>
                          <div class="tab-pane" id="tab-hmo">
                          <asp:GridView ID="grid_hmo" runat="server" EmptyDataText="No Data Found!"  AutoGenerateColumns="False" CssClass="table table-striped table-bordered">
                                <Columns>
                                     <asp:BoundField DataField="ID" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                     <asp:TemplateField HeaderText="First Name" ItemStyle-CssClass="Shorter" HeaderStyle-CssClass="Shorter">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%#bind ("fname") %>' ToolTip='<%# Eval("fname") %>'></asp:Label>
                                        </ItemTemplate>
                                     </asp:TemplateField>

                                      <asp:TemplateField HeaderText="Last Name" ItemStyle-CssClass="Shorter" HeaderStyle-CssClass="Shorter">
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%#bind ("lname") %>' ToolTip='<%# Eval("lname") %>'></asp:Label>
                                        </ItemTemplate>
                                     </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Middle Name" ItemStyle-CssClass="Shorter" HeaderStyle-CssClass="Shorter">
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%#bind ("mname") %>' ToolTip='<%# Eval("mname") %>'></asp:Label>
                                        </ItemTemplate>
                                     </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Member Type" ItemStyle-CssClass="Shorter" HeaderStyle-CssClass="Shorter">
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" runat="server" Text='<%#bind ("membertype") %>' ToolTip='<%# Eval("membertype") %>'></asp:Label>
                                        </ItemTemplate>
                                     </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Relationship to Principal" ItemStyle-CssClass="Shorter" HeaderStyle-CssClass="Shorter">
                                        <ItemTemplate>
                                            <asp:Label ID="Label5" runat="server" Text='<%#bind ("reltoprincipal") %>' ToolTip='<%# Eval("reltoprincipal") %>'></asp:Label>
                                        </ItemTemplate>
                                     </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Insurance Name" ItemStyle-CssClass="Shorter" HeaderStyle-CssClass="Shorter">
                                        <ItemTemplate>
                                            <asp:Label ID="Label6" runat="server" Text='<%#bind ("insurance") %>' ToolTip='<%# Eval("insurance") %>'></asp:Label>
                                        </ItemTemplate>
                                     </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Room Category" ItemStyle-CssClass="Shorter" HeaderStyle-CssClass="Shorter">
                                        <ItemTemplate>
                                            <asp:Label ID="Label7" runat="server" Text='<%#bind ("roomcategory") %>' ToolTip='<%# Eval("roomcategory") %>'></asp:Label>
                                        </ItemTemplate>
                                     </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Date From" ItemStyle-CssClass="Shorter" HeaderStyle-CssClass="Shorter">
                                        <ItemTemplate>
                                            <asp:Label ID="Label8" runat="server" Text='<%#bind ("coverage_date_from1") %>' ToolTip='<%# Eval("coverage_date_from1") %>'></asp:Label>
                                        </ItemTemplate>
                                     </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Date To" ItemStyle-CssClass="Shorter" HeaderStyle-CssClass="Shorter">
                                        <ItemTemplate>
                                            <asp:Label ID="Label9" runat="server" Text='<%#bind ("coverage_date_to1") %>' ToolTip='<%# Eval("coverage_date_to1") %>'></asp:Label>
                                        </ItemTemplate>
                                     </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Limit Amount" ItemStyle-CssClass="Shorter" HeaderStyle-CssClass="Shorter">
                                        <ItemTemplate>
                                            <asp:Label ID="Label10" runat="server" Text='<%#bind ("total_limit_amt") %>' ToolTip='<%# Eval("total_limit_amt") %>'></asp:Label>
                                        </ItemTemplate>
                                     </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Premium Amount" ItemStyle-CssClass="Shorter" HeaderStyle-CssClass="Shorter">
                                        <ItemTemplate>
                                            <asp:Label ID="Label11" runat="server" Text='<%#bind ("total_limit_premium") %>' ToolTip='<%# Eval("total_limit_premium") %>'></asp:Label>
                                        </ItemTemplate>
                                     </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Payable" ItemStyle-CssClass="Shorter" HeaderStyle-CssClass="Shorter">
                                        <ItemTemplate>
                                            <asp:Label ID="Label12" runat="server" Text='<%#bind ("payable") %>' ToolTip='<%# Eval("payable") %>'></asp:Label>
                                        </ItemTemplate>
                                     </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Amortization" ItemStyle-CssClass="Shorter" HeaderStyle-CssClass="Shorter">
                                        <ItemTemplate>
                                            <asp:Label ID="Label13" runat="server" Text='<%#bind ("amortization") %>' ToolTip='<%# Eval("amortization") %>'></asp:Label>
                                        </ItemTemplate>
                                     </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Remarks" ItemStyle-CssClass="Shorter" HeaderStyle-CssClass="Shorter">
                                        <ItemTemplate>
                                            <asp:Label ID="Label14" runat="server" Text='<%#bind ("remarks") %>' ToolTip='<%# Eval("remarks") %>'></asp:Label>
                                        </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="can" runat="server" CausesValidation="false" OnClick="delete_hmo" ImageUrl="~/style/img/delete.png" />
                                        </ItemTemplate>
                                        <ItemStyle Width="5px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                          <button type="button" class="btn btn-default readupdate" data-toggle="modal" data-target="#modal-hmo">Add</button>
                          <button type="button" class="btn btn-default pull-right none" data-toggle="modal" data-target="#modal-leave">New Dependent</button>
                          </div>
                           <%      }
                              } 
                          %>
                          <!--1908**************************************************-->
                          <% 
                              dr = dt.Select("route_id=1908");
                              if (dr.Count() > 0)
                              {
                                  if (dr[0]["route_id"].ToString() == "1908")
                                  { 
                          %>
                          <div class="tab-pane" id="tab-appraisal">
                            <h4 class="page-header">COMPENSATION</h4>
                            <asp:GridView ID="grid_compensation" runat="server" EmptyDataText="No Data Found!"  AutoGenerateColumns="False" CssClass="table table-striped table-bordered">
                                <Columns>
                                    <asp:BoundField DataField="id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                    <asp:BoundField DataField="app_trn_id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                    <asp:BoundField DataField="effective_date" HeaderText="Effective Date"/>
                                    <asp:BoundField DataField="payrolltype" HeaderText="Payroll Type" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                    <asp:BoundField DataField="fnod" HeaderText="Fix no of Days" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                    <asp:BoundField DataField="fnoh" HeaderText="Fix no of Hours" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                    <asp:BoundField DataField="mr" HeaderText="Monthly Rate"/>
                                    <asp:BoundField DataField="pr" HeaderText="Payroll Rate"/>
                                    <asp:BoundField DataField="dr" HeaderText="Daily Rate"/>
                                     <asp:BoundField DataField="hr" HeaderText="Houly Rate"/>
                         
                                </Columns>
                            </asp:GridView>
                            <h4 class="page-header">PROMOTION</h4>
                             <asp:GridView ID="grid_promotion" runat="server" EmptyDataText="No Data Found!"  AutoGenerateColumns="False" CssClass="table table-striped table-bordered">
                                <Columns>
                                    <asp:BoundField DataField="id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                    <asp:BoundField DataField="app_trn_id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                    <asp:BoundField DataField="effective_date" HeaderText="Effective Date"/>
                                    <asp:BoundField DataField="Position" HeaderText="Position"/>
                                </Columns>
                            </asp:GridView>
                            <h4 class="page-header">REGULARIZATION</h4>
                             <asp:GridView ID="grid_regularization" runat="server" EmptyDataText="No Data Found!"  AutoGenerateColumns="False" CssClass="table table-striped table-bordered">
                                <Columns>
                                      <asp:BoundField DataField="id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                    <asp:BoundField DataField="app_trn_id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                    <asp:BoundField DataField="effective_date" HeaderText="Effective Date"/>
                                    <asp:BoundField DataField="status" HeaderText="Status"/>
                                </Columns>
                            </asp:GridView>
                        </div>
                         <%      }
                              } 
                          %>
                          <!--1909**************************************************-->
                          <% 
                              dr = dt.Select("route_id=1909");
                              if (dr.Count() > 0)
                              {
                                  if (dr[0]["route_id"].ToString() == "1909")
                                  { 
                          %>
                          <div class="tab-pane" id="tab-audit">
                            <asp:GridView ID="gridaudit" AllowPaging="true" OnPageIndexChanging="auditPagination" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered" >
                                <Columns>
                                    <asp:BoundField DataField="Id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                    <asp:BoundField DataField="UserId" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                    <asp:BoundField DataField="Sysdate" HeaderText="Date" />
                                    <asp:BoundField DataField="FullName" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                    <asp:BoundField DataField="Action" HeaderText="Action" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                                    <asp:BoundField DataField="Transact" HeaderText="Transaction" ItemStyle-CssClass="none" HeaderStyle-CssClass="none" />
                                    <asp:BoundField DataField="Subject" HeaderText="Subject Action" />
                                    <asp:BoundField DataField="Particular" HeaderText="Particular" />
                                    <asp:BoundField DataField="AlterF" HeaderText="From" />
                                    <asp:BoundField DataField="AlterT" HeaderText="To" />
                                </Columns>
                            </asp:GridView>
                          </div>
                           <%      }
                              } 
                          %>
                          <!--1910**************************************************-->
                          <% 
                              dr = dt.Select("route_id=1910");
                              if (dr.Count() > 0)
                              {
                                  if (dr[0]["route_id"].ToString() == "1910")
                                  { 
                          %>
                          <div class="tab-pane" id="tab-status">
                            <asp:GridView ID="grid_status_details" runat="server" EmptyDataText="No Data Found!"  AutoGenerateColumns="False" CssClass="table table-striped table-bordered">
                                <Columns>
                                        <asp:BoundField DataField="empstatid" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                        <asp:BoundField DataField="date_change" HeaderText="Date Change"/>
                                        <asp:BoundField DataField="status" HeaderText="Status"/>
                                        <asp:BoundField DataField="effectivedate" HeaderText="Effective Date"/>
                                        <asp:BoundField DataField="notes" HeaderText="Notes"/>
                                </Columns>
                            </asp:GridView>
                            <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-default readupdate" Text="Add" OnClick="addStatus" />
                            
                           

                            </div>
                          <%      }
                              } 
                          %>
                          <!--1911**************************************************-->
                          <% 
                              dr = dt.Select("route_id=1911");
                              if (dr.Count() > 0)
                              {
                                  if (dr[0]["route_id"].ToString() == "1911")
                                  { 
                          %>
                          <div class="tab-pane" id="tab-note">
                            <div id="alertNOte" runat="server" visible="false" class="alert alert-danger alert-dismissible no-margin" style="margin-bottom:10px !important">
                                <i class="icon fa fa-info-circle"></i> Required field.
                            </div>
                            <div class="form-group readupdate">
                                <asp:TextBox ID="tbNote" AutoComplete="off" TextMode="MultiLine" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="clickNote" CssClass="btn btn-primary pull-right readupdate"/>
                              <div class="clearfix">
                              </div>
                                 <asp:GridView ID="gvNote" AllowPaging="true" OnPageIndexChanging="notePagination" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered" >
                                    <Columns>
                                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                                        <asp:BoundField DataField="tdate" HeaderText="Date" HeaderStyle-Width="150" />
                                        <asp:BoundField DataField="note" HeaderText="Note" />
                                        <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="readupdate" HeaderStyle-CssClass="readupdate">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbEdit" runat="server" CssClass="fa fa-pencil" OnClick="clickEditNote" Font-Size="12px"></asp:LinkButton>
                                                <asp:LinkButton ID="lbDelete" runat="server" CssClass="fa fa-minus-circle no-margin" data-id='<%# Eval("id") %>' data-transaction="note" data-toggle="modal" data-target="#modal-danger" Font-Size="12px"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="70px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                          </div>
                          <%      }
                              } 
                          %> 
                        </div>
                    </div>
                    <asp:Button ID="btn_submit" runat="server" OnClick="submit" CssClass="btn btn-primary" ClientIDMode="Static" Text="Create" />
                    <script src="vendors/tab/NewcbpFWTabs.js"></script>
                    <script>
                        jQuery.noConflict();
                        (function () {
                            [ ].slice.call(document.querySelectorAll('.nav-tabs-custom')).forEach(function (el) {new CBPFWTabs(el);});
                        })(jQuery);
                    </script>
                  </div>
            </div>
        </div>
    </div>
</div>

 <div class="modal " id="modal_status" runat="server">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="lblCloseStatus" runat="server" CssClass="close" OnClick="closeModalStatus">&times;</asp:LinkButton>
            <h4 class="modal-title">Change Status</h4>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label>Status <span style=" color:Red;">*</span></label>
                    <asp:DropDownList ID="ddl_status_emp"   runat="server">
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label>Effective Date <span style=" color:Red;">*</span></label>
                    <asp:TextBox ID="txt_effective_date" runat="server" CssClass="form-control datee" AutoComplete="off" ></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>Notes <span style=" color:Red;">*</span></label>
                    <asp:TextBox ID="txt_notes_status" runat="server" TextMode="MultiLine" style=" resize:none;" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group no-margin">
                    <asp:FileUpload ID="file_to_be_attached" runat="server" AllowMultiple="true" />
                </div>
            </div>
            <div class="modal-footer">
                    <asp:Label ID="lbl_err" runat="server"  ForeColor="Red" CssClass="pull-left"></asp:Label>
                <asp:Button ID="Button5" runat="server" OnClick="manual_change_status" Text="Submit" CssClass="btn btn-default" />
            </div>
        </div>
    </div>
</div>  

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
            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
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
             </div>
             <div class="form-group">
                <label>Address</label>
                <asp:TextBox ID="txtaddressemergency" runat="server" CssClass="form-control"></asp:TextBox>
             </div>
             <div class="form-group">
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
                <asp:TextBox ID="txt_hostcont" runat="server" CssClass="form-control"></asp:TextBox>
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


<div class="modal fade in" id="modal-jobhistory" >
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

<div class="modal fade in" id="irsanctionlist" runat="server">
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <div align="right">
                <asp:LinkButton ID="lnkclose" OnClick="closingremarks" runat="server"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>
            </div>
        <h4 class="modal-title">Incident Reports File</h4>
        </div>
        <div class="modal-body">
        <asp:GridView ID="incidentfiles" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered" >
            <Columns>
                <asp:BoundField DataField="filename" HeaderText="File Name"/>
                <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:LinkButton ID="lnk_downloadirpiece" runat="server" ToolTip="Download" CommandName='<%# Eval("id") %>' Text="download" OnClick="downlaodirfilesbypiece"><i class="fa fa-download"></i></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle Width="75px" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </div>

    </div>
    </div>
</div>

<div class="modal fade in" id="modal_requirementlist" clientidmode="Static" runat="server">
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <div align="right">
                <asp:LinkButton ID="lnkbtnclose" OnClick="closingremarks" runat="server"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>
            </div>
        <h4 class="modal-title">Requirements List</h4>
        </div>
        <div class="modal-body">
        <asp:GridView ID="grid_det" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
            <Columns>
                <asp:BoundField DataField="id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                <asp:BoundField DataField="filename" HeaderText="File Name"/>
                <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:LinkButton ID="lnk_download" runat="server" ToolTip="Download" CommandName='<%# Eval("id") %>' Text="download" OnClick="download"><i class="fa fa-download"></i></asp:LinkButton>
                    <asp:LinkButton ID="lnk_can" runat="server" ToolTip="Remove" CommandName='<%# Eval("id") %>' OnClick="candetails"  OnClientClick="Confirm()"  ClientIDMode="Static"><i class="fa fa-trash"></i></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle Width="75px" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
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

<div class="modal fade in" id="modal-hmo" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title">ADD HMO</h4>
        </div>
        <div class="modal-body">
            <div class="form-group">
                <label>Member Type</label>
                <asp:DropDownList ID="ddl_mt"  OnTextChanged="get_emp" AutoPostBack="true"  runat="server">
                     <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Principal</asp:ListItem>
                        <asp:ListItem>Dependent</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <label>First Name</label>
                <asp:TextBox ID="txt_ins_fname" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Last Name</label>
                <asp:TextBox ID="txt_ins_lname" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Middle Name</label>
                <asp:TextBox ID="txt_ins_mname" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
             <div class="form-group">
                <label>Relationship to Principal</label>
                <asp:TextBox ID="txt_rtp" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Insurance Name</label>
                <asp:TextBox ID="txt_insurance" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Insurance Name</label>
                <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Room Category</label>
                <asp:TextBox ID="txt_rc" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Contract No.</label>
                <asp:TextBox ID="txt_cn" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Coverage Date From</label>
                <asp:TextBox ID="txt_cdf" runat="server" CssClass="form-control datee"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Coverage Date To</label>
                <asp:TextBox ID="txt_cdt" runat="server" CssClass="form-control datee"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Limit Amount</label>
                <asp:TextBox ID="txt_la" runat="server" AutoComplete="off" ClientIDMode="Static" CssClass="form-control" onkeyup="decimalinput(this)"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Premuim Amount</label>
                <asp:TextBox ID="txt_pa" runat="server" AutoComplete="off" ClientIDMode="Static" CssClass="form-control" onkeyup="decimalinput(this)"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>Remarks</label>
                <asp:TextBox ID="txt_remarks" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:CheckBox ID="chk_ins_payable" runat="server" Text="Payable"/>
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button ID="Button10" runat="server" OnClick="hmo" Text="Submit" CssClass="btn btn-default" />
        </div>
       
    </div>
    </div>
</div>
 
<div class="modal fade in" id="modal_SancAttachment" runat="server" >
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <asp:LinkButton ID="lb_CloseSancAttach" runat="server" OnClick="close_sacnAttachment" CssClass="close"> <span aria-hidden="true">&times;</span></asp:LinkButton>
        <h4 class="modal-title">Sanction Attachment</h4>
        </div>
        <div class="modal-body">
            <div class="form-group">
                <asp:FileUpload ID="fuincidentreport" runat="server"  /> 
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnfiles" runat="server" OnClick="uploadirfiles" Text="Upload" CssClass="btn btn-primary" />
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
                        <li class="" onclick="click_tab(17)"><a href="#compensation" data-toggle="tab">Compensation</a></li>
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
                       <% if (Request.QueryString["tp"].ToString() == "ed")
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
                        <%--OnClick="next"--%>
                        <asp:Button ID="btn_identity" runat="server" Text="Nextsssssssssssss"  CssClass="btn btn-primary" />
                    </div>
                    <div class="tab-pane" id="personal">
                        <asp:Button ID="btn_personal" runat="server" Text="Next" OnClick="next1" CssClass="btn btn-primary" />
                    </div>
                    <div class="tab-pane" id="messages">
                        
                       
                        <div class="clearfix"></div>
                          <asp:Button ID="btn_payroll" runat="server" Text="Next" OnClick="next2" CssClass="btn btn-primary" />
                          <asp:HiddenField ID="hdn_minimumwage" ClientIDMode="Static"  runat="server" />
                    </div>
                <div class="tab-pane" id="compensation">
                        <asp:Button ID="btncompentsation" runat="server" Text="Next" OnClick="next3" CssClass="btn btn-primary" />
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
        
</div>

<div class="modal modal-danger fade in" id="modal-danger">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">×</span></button>
            <h4 class="modal-title">Delete Note</h4>
            </div>
            <div class="modal-body">
            <p>Continue deleting note?</p>
            </div>
            <div class="modal-footer">
                <asp:LinkButton ID="lbDeleteAction" runat="server" Text="OK" OnClick="clickDelete" class="btn btn-outline"></asp:LinkButton>
            </div>
        </div>
    </div>
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
<asp:HiddenField ID="hf_auto" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hfAction" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hfTransaction" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hfTID" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="seriesid" runat="server" />

<div style=" visibility:hidden;">
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <asp:CheckBox ID="chk_sssgs" AutoComplete="off" runat="server"  />
    <asp:dropdownlist ID="ddl_hdmftype" AutoComplete="off" runat="server"><asp:ListItem>Value</asp:ListItem><asp:ListItem>Percentage</asp:ListItem></asp:dropdownlist>
</div>
</asp:Content>

<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
<%--Date picker--%>
<script type="text/javascript">
    $.noConflict();
    $('.datee').datepicker({ changeMonth: true, changeYear: true,   yearRange: "-100:+0" })
</script>

<%--CROPPER--%>
<script src="vendors/crop image/js/jquery-1.12.4.min.js"></script>
<script src="vendors/crop image/js/cropper.min.js"></script>
<script src="vendors/crop image/js/main.js"></script>
<link href="vendors/crop image/css/cropper.min.css" rel="stylesheet">
<link href="vendors/crop image/css/main.css" rel="stylesheet">

<!--Select-->
<script type="text/javascript" src="vendors/select2/dist/js/select2.full.min.js"></script>
<script type="text/javascript">
    (function ($) {
        $('.select2').select2();
    })(jQuery);
</script>

    <!--Delete-->
    <script type="text/javascript">
        $("[data-toggle='modal']").click(function () {
            $("#hfTransaction").val($(this).attr("data-transaction"));
            $("#hfTID").val($(this).attr("data-id"));
        })
    </script>

</asp:Content>
