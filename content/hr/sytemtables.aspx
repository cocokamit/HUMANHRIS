<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sytemtables.aspx.cs" Inherits="content_hr_mandatorytables" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">

<style type="text/css">
    .PopUpPanel { position:absolute;background-color: #fff;   top:25%;left:35%;z-index:2001; padding:20px;min-width:200px;max-width:800px;-moz-box-shadow:2px 2px 3px #000000;-webkit-box-shadow:2px 2px 5px #000000;box-shadow:2px 2px 5px #000000;border-radius:1px;-moz-border-radiux:5px;-webkit-border-radiux:5px;}
    .PopUpPanel3{  width:300px;left:60%;}
    .close{ margin:-10px;float:right;}
    .Overlay2 {  position:fixed; top:0px; bottom:0px; left:0px; right:0px; overflow:hidden; padding:0; margin:0; background-color:#000; filter:alpha(opacity=50); opacity:0.5; z-index:1000;}
    .PopUpPanel2 { position:absolute;background-color: #fff;   top:25%;left:35%;z-index:2001; padding:20px;min-width:200px;max-width:600px;-moz-box-shadow:2px 2px 3px #000000;-webkit-box-shadow:2px 2px 5px #000000;box-shadow:2px 2px 5px #000000;border-radius:5px;-moz-border-radiux:5px;-webkit-border-radiux:5px;}
    .PopUpPanel3 {   border-radius:4px; position: fixed; z-index: 1002;  width: 600px; top: 28%;left: 50%; margin: -180px 0 0 -300px; background-color: #fff; }
    .input-form-span { margin-left:10px;border:1px solid #eee; padding:5px; margin-bottom:10px; }
</style>
<script type="text/javascript">
    function Confirm() {
        var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
        if (confirm("Are you sure to cancel this transaction?"))
        { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
    } 
</script>
</asp:Content>

<asp:Content ID="mandatorytables" runat="server" ContentPlaceHolderID="content">
 <div class="page-title">
    <div class="title_left hd-tl">
        <h3>Table Set Up</h3>
    </div>   
    <div class="title_right">
        <ul>
            <li><a href="#"><i class="fa fa-gear"></i>Sytem Configurtion </a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>Table Set Up</li>
        </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div id="pgrid_civil" runat="server" visible="false" class="Overlay"></div>
    <div id="pgrid_civil2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton17" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txt_civilupdate" Placeholder="Civil Status" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="Button15" OnClick="updatecivil" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgrid_role" runat="server" visible="false" class="Overlay"></div>
    <div id="pgrid_role2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton16" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txt_roleupdate" Placeholder="Role" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="Button6" OnClick="updaterole" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgrid_level" runat="server" visible="false" class="Overlay"></div>
    <div id="pgrid_level2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton15" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txt_levelupdate" Placeholder="Level" runat="server"></asp:TextBox></li>
            <li><asp:CheckBox ID="chckallowot" runat="server" />Allow OT Meal</li>
            <li><hr /></li> 
            <li><asp:Button ID="btnlevelupdate" OnClick="updatelevel" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgrid_payg" runat="server" visible="false" class="Overlay"></div>
    <div id="pgrid_payg2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton2" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txtpgrp" Placeholder="Payroll Group" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:CheckBox ID="chckdisp_btn" runat="server" />Enable</li>
            <li><asp:Button ID="btnpgrp" OnClick="editpgrp" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgrid_department" runat="server" visible="false" class="Overlay"></div>
    <div id="pgrid_department2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton3" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txtdepta" Placeholder="Department" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="btndepts" OnClick="editdepartme" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgrid_position" runat="server" visible="false" class="Overlay"></div>
    <div id="pgrid_position2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton4" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txtpost" Placeholder="Position" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="btnpst" OnClick="editposits" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgrid_coa" runat="server" visible="false" class="Overlay"></div>
    <div id="pgrid_coa2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton5" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txtcoas" Placeholder="Code" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtpsition" Placeholder="Position" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="btnscox" OnClick="editcoaxs" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="panelOverlay" runat="server" visible="false" class="Overlay"></div>
    <div id="panelPopUpPanel" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txtupdatereqs" Placeholder="Requirements" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="btnupreqs" OnClick="editreqname" runat="server" Text="Update" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgrid_relegion" runat="server" visible="false" class="Overlay"></div>
    <div id="pgrid_relegion2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton6" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txtrelegion" Placeholder="Relegion" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="btnrele" OnClick="editreleg" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgrid_citizenship" runat="server" visible="false" class="Overlay"></div>
    <div id="pgrid_citizenship2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton7" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txtcit" Placeholder="Citizenship" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="btncits" OnClick="editcitising" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgrid_zipcode" runat="server" visible="false" class="Overlay"></div>
    <div id="pgrid_zipcode2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton8" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txtzipcode" Placeholder="ZipCode" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtziploc" Placeholder="Location" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtziparea" Placeholder="Area" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="Button4" OnClick="editziping" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgrid_division" runat="server" visible="false" class="Overlay"></div>
    <div id="pgrid_division2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton11" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txtbandlevel" Placeholder="Company" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="btndiv" OnClick="editdivisionings" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgrid_company" runat="server" visible="false" class="Overlay"></div>
    <div id="pgrid_company2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton9" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txtcomp" Placeholder="Company" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtcomadd" Placeholder="Address" runat="server"></asp:TextBox></li>
            <li><asp:DropDownList ID="ddlcompzip"  runat="server" CssClass="minimal"></asp:DropDownList></li>
            <li><asp:TextBox ID="txtcsss" Placeholder="SSS" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtcphic" Placeholder="PHIC" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtchdmf" Placeholder="HDMF" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtctin" Placeholder="TIN" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="btncomps" OnClick="editcompings" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="plocation" runat="server" visible="false" class="Overlay"></div>
    <div id="plocation2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton14" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txtlocaiton" Placeholder="Location" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="Button7" OnClick="editlocing" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgrid_branch" runat="server" visible="false" class="Overlay"></div>
    <div id="pgrid_branch2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton10" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li>Department</li>
            <li><asp:DropDownList ID="ddl_deptsec"  runat="server" CssClass="minimal"></asp:DropDownList></li>
            <li>Department Code</li>
            <li><asp:TextBox ID="txtsecs" Placeholder="Department Code" runat="server"></asp:TextBox></li>
            <li>Description</li>
            <li><asp:TextBox ID="txtdecs" Placeholder="Description" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="btnbrnch" OnClick="editbrang" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgrid_internalorder" runat="server" visible="false" class="Overlay"></div>
    <div id="pgrid_internalorder2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton13" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li>Deparment</li> 
            <li><asp:DropDownList ID="ddl_compint"  runat="server" CssClass="minimal"></asp:DropDownList></li>
            <li>Internal Order</li> 
            <li><asp:TextBox ID="txt_internal" Placeholder="Description" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="btninternal" OnClick="editinternalorder" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgrid_store" runat="server" visible="false" class="Overlay"></div>
    <div id="pgrid_store2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton12" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txtedstore" Placeholder="Section" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="Button5" OnClick="editstores" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>

    <div class="col-md-9">
        <div class="x_panel">
            <div class="title_right">
                <asp:DropDownList ID="dl_choice" runat="server" AutoPostBack="true" OnSelectedIndexChanged="change_choice"><%--CssClass="minimal"--%>
                    <asp:ListItem>Payroll Group</asp:ListItem>
                    <asp:ListItem>Civil Status</asp:ListItem>
                   <%-- <asp:ListItem>Payroll Type</asp:ListItem>--%>
                    <asp:ListItem>Department</asp:ListItem>
                    <asp:ListItem>Department Code</asp:ListItem>
                    <asp:ListItem>Position</asp:ListItem>
                    <asp:ListItem>Internal Order</asp:ListItem>
                    <asp:ListItem>COA</asp:ListItem>
                    <asp:ListItem>Requirement/s</asp:ListItem>
                    <asp:ListItem>Religion</asp:ListItem>
                    <asp:ListItem>Citizenship</asp:ListItem>
                    <asp:ListItem>Zip Code</asp:ListItem>
                    <asp:ListItem>Band</asp:ListItem>
                    <asp:ListItem>Blood Type</asp:ListItem>
                    <asp:ListItem>Company</asp:ListItem>
                    <asp:ListItem>Location</asp:ListItem>
                    <asp:ListItem>Leave</asp:ListItem>
                    <%--<asp:ListItem>Outlet</asp:ListItem>--%>
                    <asp:ListItem>Level</asp:ListItem>
                    <asp:ListItem>Role</asp:ListItem>
                </asp:DropDownList>
            </div>

            <asp:GridView ID="grid_civil" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                  <asp:BoundField DataField="Id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                  <asp:BoundField DataField="CivilStatus" HeaderText="Civil Status"/>
                  <asp:TemplateField HeaderText="Action">
                            <ItemTemplate >
                            <asp:LinkButton ID="lb_pgrouphide" runat="server" CommandName='<%# Eval("Id") %>' Text="Remove" ToolTip="Edit" OnClick="showpcivil" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_role" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                  <asp:BoundField DataField="Id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                  <asp:BoundField DataField="Level" HeaderText="Role"/>
                  <asp:TemplateField HeaderText="Action">
                            <ItemTemplate >
                            <asp:LinkButton ID="lb_pgrouphide" runat="server" CommandName='<%# Eval("Id") %>' Text="Remove" ToolTip="Edit" OnClick="showprole" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_level" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                  <asp:BoundField DataField="Id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                  <asp:BoundField DataField="Division2" HeaderText="Level"/>
                  <asp:BoundField DataField="AllowOtMeal" HeaderText="Allow OT Meal"/>
                  <asp:TemplateField HeaderText="Action">
                            <ItemTemplate >
                            <asp:LinkButton ID="lb_pgrouphide" runat="server" CommandName='<%# Eval("Id") %>' Text="Remove" ToolTip="Edit" OnClick="showplevel" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_payg" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="PayrollGroup" HeaderText="Description"/>
                    <asp:BoundField DataField="status" HeaderText="Status"/>
                    <asp:TemplateField HeaderText="Action">
                            <ItemTemplate >
                            <asp:LinkButton ID="lb_pgrouphide" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="hidepgroup" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_paytype" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="PayrollType" HeaderText="Payroll Type"/>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_department" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="Department" HeaderText="Department"/>
                    <asp:TemplateField HeaderText="Action">
                            <ItemTemplate >
                            <asp:LinkButton ID="lb_depthide" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="hidedept" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_position" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="Position" HeaderText="Position"/>
                    <asp:TemplateField HeaderText="Action">
                            <ItemTemplate >
                            <asp:LinkButton ID="lb_positionhide" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="hidepposition" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_coa" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="accountcode" HeaderText="Code"/>
                    <asp:BoundField DataField="account" HeaderText="Position"/>
                    <asp:TemplateField HeaderText="Action">
                            <ItemTemplate >
                            <asp:LinkButton ID="lb_coahide" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="hidepcoa" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_requirements" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="No." ItemStyle-Width="10px"/>
                    <asp:BoundField DataField="description" HeaderText="Requirement/s List"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbcancelreqs" runat="server" OnClick="goidetrow" Text="cancel" ToolTip="Edit"><i class="fa fa-pencil"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="65px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_relegion" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="Religion" HeaderText="Religion"/>
                    <asp:TemplateField HeaderText="Action">
                            <ItemTemplate >
                            <asp:LinkButton ID="lb_relegionhide" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="hideprelegion" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_citizenship" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="Citizenship" HeaderText="Citizenship"/>
                    <asp:TemplateField HeaderText="Action">
                            <ItemTemplate >
                            <asp:LinkButton ID="lb_citizenhide" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="hidepcitizen" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_zipcode" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="ZipCode" HeaderText="ZipCode"/>
                    <asp:BoundField DataField="Location" HeaderText="Location"/>
                    <asp:BoundField DataField="Area" HeaderText="Area"/>
                     <asp:TemplateField HeaderText="Action">
                            <ItemTemplate >
                            <asp:LinkButton ID="lb_ziphide" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="hideziping" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_division" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="Division" HeaderText="Band"/>
                    <asp:TemplateField HeaderText="Action">
                            <ItemTemplate >
                            <asp:LinkButton ID="lb_divisionhide" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="hidedivisions" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_bloodtype" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="bloodtype" HeaderText="Blood Type"/>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_company" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="company" HeaderText="Company"/>
                    <asp:BoundField DataField="address" HeaderText="Address"/>
                    <asp:BoundField DataField="fuck" HeaderText="ZipCode"/>
                    <asp:BoundField DataField="sssnumber" HeaderText="SSS#"/>
                    <asp:BoundField DataField="phicnumber" HeaderText="PHIC#"/>
                    <asp:BoundField DataField="hdmfnumber" HeaderText="HDMF#"/>
                    <asp:BoundField DataField="tin" HeaderText="T.I.N"/>
                    <asp:TemplateField HeaderText="Action">
                            <ItemTemplate >
                            <asp:LinkButton ID="lb_comphide" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="hidecomps" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_branch" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="company" HeaderText="Company"/>
                    <asp:BoundField DataField="branch" HeaderText="Location"/>
                    <asp:TemplateField HeaderText="Action">
                            <ItemTemplate >
                            <asp:LinkButton ID="lb_locationhide" runat="server" CommandName='<%# Eval("id") %>' Text="Remove" ToolTip="Edit" OnClick="hidelocation" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_section" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="sectionid" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="departmentarea" HeaderText="Department"/>
                    <asp:BoundField DataField="seccode" HeaderText="Department Code"/>
                    <asp:BoundField DataField="sec_desc" HeaderText="Description"/>
                    <asp:TemplateField HeaderText="Action">
                            <ItemTemplate >
                            <asp:LinkButton ID="lb_brnchhide" runat="server" CommandName='<%# Eval("sectionid") %>' Text="Remove" ToolTip="Edit" OnClick="hidebranch" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
           
            <asp:GridView ID="grid_internalorder" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="Id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="Department" HeaderText="Department"/>
                    <asp:BoundField DataField="InternalOrder" HeaderText="Internal Order"/>
                    <asp:TemplateField HeaderText="Action">
                            <ItemTemplate >
                            <asp:LinkButton ID="lb_iohide" runat="server" CommandName='<%# Eval("Id") %>' Text="Remove" ToolTip="Edit" OnClick="hideinternalorder" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
           
            <asp:GridView ID="grid_insuarnce" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="insure_name" HeaderText="Insurance"/>
                </Columns>
            </asp:GridView>

            <asp:GridView ID="grid_leve" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="LeaveType" HeaderText="Leave Type"/>
                    <asp:BoundField DataField="Leave" HeaderText="Description"/>
                    <asp:BoundField DataField="yearlytotal" HeaderText="Yearly Total"/>
                    <asp:BoundField DataField="converttocash" HeaderText="Convert To Cash"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_edit" OnClick="add" runat="server" Tooltip="Edit"><i class="fa fa-pencil"></i></asp:LinkButton>
                            <asp:LinkButton ID="lnk_delete" OnClientClick="Confirm()" Visible="false" OnClick="delete_leave" runat="server" Tooltip="Delete"><i class="fa fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:GridView ID="grid_store" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" HeaderStyle-CssClass="none" ControlStyle-CssClass="none" ItemStyle-CssClass="none" />
                    <asp:BoundField DataField="store" HeaderText="Outlet"/>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_delete" OnClick="showupstore" runat="server" Tooltip="Edit"><i class=" fa fa-pencil"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
           
        </div>
    </div>
    <div class="col-md-3">
        <div class="x_panel">
            <asp:Panel ID="p_payg" runat="server">
                <ul class="input-form">
                    <li>Payroll Group</li>
                    <li><asp:TextBox ID="txt_payrollgroup" runat="server"></asp:TextBox></li>
                    <li><hr /></li> 
                    <li><asp:Button ID="btn_save_payrollgroup" OnClick="save_payrollgroup" runat="server" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_paytype" runat="server">
                <ul class="input-form">
                     <li>Payroll Type</li>
                    <li><asp:TextBox ID="txt_payrolltype" runat="server"></asp:TextBox></li>
                    <li><hr /></li> 
                    <li><asp:Button ID="btn_save_payrolltype" OnClick="save_payrolltype" runat="server" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_department" runat="server">
                <ul class="input-form">
                    <li>Department</li>
                    <li><asp:TextBox ID="txt_department" runat="server"></asp:TextBox></li>
                    <li><hr /></li> 
                    <li><asp:Button ID="btn_department" OnClick="save_department" runat="server" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_position" runat="server">
                <ul class="input-form">
                    <li>Position</li>
                    <li><asp:TextBox ID="txt_position" runat="server"></asp:TextBox></li>
                    <li><hr /></li> 
                    <li><asp:Button ID="Button8" OnClick="save_position" runat="server" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_coa" runat="server">
                <ul class="input-form">
                    <li>Account Code</li>
                    <li><asp:TextBox ID="txt_coacode" runat="server"></asp:TextBox></li>
                    <li>Account</li>
                    <li><asp:TextBox ID="txt_account" runat="server"></asp:TextBox></li>
                    <li><hr /></li> 
                    <li><asp:Button ID="Button9" OnClick="save_coa" runat="server" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_relegion" runat="server">
                <ul class="input-form">
                    <li>Religion</li>
                    <li><asp:TextBox ID="txt_religion" runat="server"></asp:TextBox></li>
                    <li><hr /></li> 
                    <li><asp:Button ID="Button10" OnClick="save_religion" runat="server" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_citizenship" runat="server">
                <ul class="input-form">
                    <li>Citizenship</li>
                    <li><asp:TextBox ID="txt_citizenship" runat="server"></asp:TextBox></li>
                    <li><hr /></li> 
                    <li><asp:Button ID="Button11" OnClick="save_citizenship" runat="server" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_zipcode" runat="server">
                <ul class="input-form">
                    <li>Zip Code</li>
                    <li><asp:TextBox ID="txt_zipcode" runat="server"></asp:TextBox></li>
                    <li>Location</li>
                    <li><asp:TextBox ID="txt_location" runat="server"></asp:TextBox></li>
                    <li>Area</li>
                    <li><asp:TextBox ID="txt_Area" runat="server"></asp:TextBox></li>
                    <li><hr /></li> 
                    <li><asp:Button ID="Button12" OnClick="save_zipcode" runat="server" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_division" runat="server">
                <ul class="input-form">
                    <li>Band Level</li>
                    <li><asp:TextBox ID="txt_division" runat="server"></asp:TextBox></li>
                    <li><hr /></li> 
                    <li><asp:Button ID="Button13" OnClick="save_divission" runat="server" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_bloodtype" runat="server">
                <ul class="input-form">
                    <li>Blood Type</li>
                    <li><asp:TextBox ID="txt_bloodtype" runat="server"></asp:TextBox></li>
                    <li><hr /></li> 
                    <li><asp:Button ID="Button14" OnClick="save_bloodtype" runat="server" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_company" runat="server">
                <ul class="input-form">
                    <li>Company</li>
                    <li><asp:TextBox ID="txt_company" runat="server"></asp:TextBox></li>
                    <li>Address</li>
                    <li><asp:TextBox ID="txt_address" runat="server"></asp:TextBox></li>
                    <li>SSS#</li>
                    <li><asp:TextBox ID="txt_sssnumber" runat="server"></asp:TextBox></li>
                    <li>PHIC#</li>
                    <li><asp:TextBox ID="txt_phicnumber" runat="server"></asp:TextBox></li>
                    <li>HDMF#</li>
                    <li><asp:TextBox ID="txt_hdmfnumber" runat="server"></asp:TextBox></li>
                    <li>T.I.N</li>
                    <li><asp:TextBox ID="txt_tin" runat="server"></asp:TextBox></li>
                    <li>ZipCode</li>
                    <li><asp:DropDownList ID="ddl_zipcomp"  runat="server" CssClass="minimal"></asp:DropDownList></li>
                    <li class="none">Over Break Roles</li>
                    <li class="none">
                        <asp:DropDownList ID="ddl_obr" runat="server">
                            <asp:ListItem Value="1">Default</asp:ListItem>
                            <asp:ListItem Value="2">No Deduction</asp:ListItem>
                        </asp:DropDownList>
                    </li>
                    <li class="none">OT Roles</li>
                    <li class="none">
                        <asp:DropDownList ID="ddl_otroles" runat="server">
                        <asp:ListItem Value="1"> >=1 Hour / Default</asp:ListItem>
                        <asp:ListItem Value="2"> >=1 Hour / Fixed Per Hour</asp:ListItem>
                        </asp:DropDownList>
                    </li>
                     <li class="none">Gov't Deduction Roles</li>
                    <li class="none"><asp:DropDownList ID="ddl_gdr" runat="server">
                    <asp:ListItem Value="1">Alternate</asp:ListItem>
                    <asp:ListItem Value="2">Default</asp:ListItem>
                    </asp:DropDownList>
                    </li>
                    <li><hr /></li> 
                    <li><asp:Button ID="Button17" OnClick="save_company" runat="server" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_branch" runat="server">
                <ul class="input-form">
                    <li>Company</li>
                    <li><asp:DropDownList ID="ddl_com"  runat="server" CssClass="minimal"></asp:DropDownList></li>
                    <li>Location</li>
                    <li><asp:TextBox ID="txt_branch" runat="server"></asp:TextBox></li>
                    <li><hr /></li> 
                    <li><asp:Button ID="Button18" OnClick="save_branch" runat="server" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_section" runat="server">
                <ul class="input-form">
                    <li>Department</li>
                    <li><asp:DropDownList ID="ddl_dept"  runat="server" CssClass="minimal"></asp:DropDownList></li>
                    <li>Department Code</li>
                    <li><asp:TextBox ID="txt_section" runat="server"></asp:TextBox></li>
                     <li>Description</li>
                    <li><asp:TextBox ID="txt_description" runat="server"></asp:TextBox></li>
                    <li><hr /></li> 
                    <li><asp:Button ID="Button1" OnClick="save_section" runat="server" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_internalorder" runat="server">
                <ul class="input-form">
                    <li>Department</li>
                    <li><asp:DropDownList ID="ddl_departinternal"  runat="server" CssClass="minimal"></asp:DropDownList></li>
                    <li>Internal Order</li>
                    <li><asp:TextBox ID="txt_internalorder" runat="server"></asp:TextBox></li>
                    <li><hr /></li> 
                    <li><asp:Button ID="btnint" OnClick="save_internal" runat="server" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_insurance" Style=" display:none;" runat="server">
                <ul class="input-form">
                    <li>Insurance Name</li>
                    <li><asp:TextBox ID="txt_insurance" runat="server"></asp:TextBox></li>
                    <li><hr /></li> 
                    <li><asp:Button ID="Button2" OnClick="save_insurance" runat="server" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>

           <asp:Panel ID="p_leave" runat="server">
               <div class="form-group">
                <li>Leave Type</li>
                <asp:TextBox ID="txt_leavetype" runat="server" CssClass="form-control"></asp:TextBox>
               </div>
               <div class="form-group">
                <li>Leave Description</li>
                <asp:TextBox ID="txt_leavedescription" runat="server" CssClass="form-control"></asp:TextBox>
               </div>
               <div class="form-group">
                <li>Total Leave</li>
                <asp:TextBox ID="txt_leavetotal" runat="server" CssClass="form-control"></asp:TextBox>
               </div>
               <div class="form-group">
                <asp:CheckBox ID="check_leave" runat="server" /> <label> Convert to Cash</label>
               </div>
               <div class="form-group">
                <asp:CheckBox ID="cb_attachment" runat="server" /> <label> Required Attachment</label>
               </div>
               <hr />
                <ul class="input-form">
                    <li></li> 
                    <li><asp:Button ID="btn_save" OnClick="save_leave" runat="server" Text="Save" CssClass="btn btn-primary no-margin"/></li>
                </ul>
              
           </asp:Panel>


           <asp:Panel ID="p_role" runat="server">
                <ul class="input-form">
                    <li>Role</li>
                    <li><asp:TextBox ID="txt_role" runat="server"></asp:TextBox></li>
                    <li><hr /></li> 
                    <li><asp:Button ID="btnrole" OnClick="save_role" runat="server" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
           <asp:Panel ID="p_level" runat="server">
                <ul class="input-form">
                    <li>Level</li>
                    <li><asp:TextBox ID="txt_level" runat="server"></asp:TextBox></li>
                    <li><hr /></li> 
                    <li><asp:Button ID="btnlevel" OnClick="save_level" runat="server" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_civil" runat="server">
                <ul class="input-form">
                    <li>Civil Status</li>
                    <li><asp:TextBox ID="txt_civil" runat="server"></asp:TextBox></li>
                    <li><hr /></li> 
                    <li><asp:Button ID="Button16" OnClick="save_civil" runat="server" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>

            <asp:Panel ID="p_store" runat="server">
                <ul class="input-form">
                    <li>Outlet Name</li>
                    <li><asp:TextBox ID="txt_store" runat="server"></asp:TextBox></li>
                    <li><hr /></li> 
                    <li><asp:Button ID="Button3" OnClick="save_store" runat="server" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
              <asp:Panel ID="p_requirements" runat="server">
                <ul class="input-form">
                    <li>Requirements</li>
                    <li><asp:TextBox ID="txtprequirements" AutoCompleteType="Disabled" runat="server"></asp:TextBox></li>
                    <li><hr /></li> 
                    <li><asp:Button ID="btnreqs" OnClick="save_requirements" runat="server" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>

        </div> 
    </div>
</div>

<div id="Div1" runat="server" visible="false" class="Overlay2"></div>
<div id="pAdd" runat="server" visible="false" class="PopUpPanel">
    <asp:ImageButton ID="ImageButton18" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
    <ul class="input-form">
        <li>Leave Type</li>
        <li>
            <asp:TextBox ID="txt_lt" AutoComplete="off" runat="server"></asp:TextBox>
            <asp:Label ID="lbl_lt" style=" color:Red;" runat="server" Text=""></asp:Label>
        </li>
        <li>Description</li>
        <li>
            <asp:TextBox ID="txt_desc" AutoComplete="off" runat="server"></asp:TextBox>
            <asp:Label ID="lbl_desc" style=" color:Red;" runat="server" Text=""></asp:Label>
        </li>
        <li>Yearly Credits</li>
        <li>
            <asp:TextBox ID="txt_yt" AutoComplete="off" ClientIDMode="Static" onkeyup="intinput(this);"  runat="server"></asp:TextBox>
            <asp:Label ID="lbl_yt" style=" color:Red;" runat="server" Text=""></asp:Label>
        </li>
        <li><asp:CheckBox ID="chk_convertable" runat="server" Text="Convertable to Cash!" /></li>
        <li><asp:CheckBox ID="cb_reqAttachment" runat="server" Text="Required Attachment" /></li>
        <li><hr /></li>
        <li><asp:Button ID="btn_add" runat="server" OnClick="update_leave" Text="Save" CssClass="btn btn-primary"/></li>
    </ul>
</div>
<asp:TextBox ID="TextBox1" style=" display: none;" runat="server"></asp:TextBox>
<asp:HiddenField ID="leave_id" runat="server" />
<asp:HiddenField ID="store_id" runat="server" />
<asp:HiddenField ID="seriesid" runat="server" />
<asp:SqlDataSource ID="sql_company" runat="server" 
ConnectionString="<%=  Config.connection() %>" 
ProviderName="System.Data.SqlClient"  
SelectCommand="select * from MCompany order by id desc"></asp:SqlDataSource>
</asp:Content>
