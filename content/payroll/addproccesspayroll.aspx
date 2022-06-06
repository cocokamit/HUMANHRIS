<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addproccesspayroll.aspx.cs" Inherits="content_payroll_addproccesspayroll" MasterPageFile="~/content/MasterPageNew.master" %>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
     <style type="text/css"> 

        .pre li { margin-left:-40px;}
        .pre select { width:300px}
        textarea{ width:37.7%; margin-bottom:0; height:92px}
        textarea:focus,textarea:active{ border:none}
        .left { padding-right:10px}
        .nobel {position: fixed; overflow:hidden; top: 42%;left: 50%;margin-left:-70px; z-index: 2001; padding: 20px;      }
        .GridViewStyle{font-family:Verdana;font-size:11px;background-color: White;}
         .GridViewHeaderStyle{font-family:Verdana;font-size:xx-small;height:30px; text-align:center; padding : 4px 2px;color: #fff;background: #333 url(Images/grid-header.png) repeat-x top;border-left: solid 1px #525252;font-size: 0.9em;text-align: center;text-transform: uppercase;font-weight: bold; }


    </style>
   
</asp:Content>

<asp:Content ID="content" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left">
        <h3>Process Payroll</h3>
    </div>   
    <div class="title_right">
       <ul>
        <li><a href="procpay?user_id=<% Response.Write(Session["user_id"]); %>"><i class="fa fa-bug"></i> Payroll</a></li>
        <li><i class="fa fa-angle-right"></i></li>
        <li>Process Data</li>
       </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div id="xp"  class="x_panel">
             <div class="row">
             <div class="col-md-3">
                 <div class="form-group">
                    <label>Payroll Group</label>
                    <asp:DropDownList ID="ddl_pg" OnSelectedIndexChanged="verify_det" AutoPostBack="true" CssClass="form-control" runat="server"></asp:DropDownList>
                 </div>
             </div>
             <div class="col-md-3">
                 <div class="form-group">
                     <label>DTR</label>
                     <asp:DropDownList ID="ddl_dtr" Enabled="false" runat="server" ></asp:DropDownList>
                 </div>
             </div>
             <div class="col-md-3">
                 <div class="form-group">
                    <label>Other Income</label>
                    <asp:DropDownList ID="ddl_otherincome" Enabled="false" runat="server" ></asp:DropDownList>
                 </div>
             </div>
             <div class="col-md-3">
                 <div class="form-group">
                    <label>Other Deduction</label>
                    <asp:DropDownList ID="ddl_otherdeduction" Enabled="false" runat="server" ></asp:DropDownList>
                 </div>
             </div>
            </div> 
            <div class="row">
            <div class="col-md-3">
                <div class="form-group">
                    <label>Payroll Date</label>
                    <asp:TextBox ID="txt_PD" runat="server" type="date" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-3">
                <div class="form-group">
                    <label>Payroll Period</label>
                    <label style=" float:right; font-size:9px;">From</label>
                    <asp:TextBox ID="txt_pp_from" CssClass="form-control" type="date" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-3">
                <div class="form-group">
                <label style="visibility:hidden;">TO</label>
                <label style=" float:right; font-size:9px;">To</label>
                <asp:TextBox ID="txt_pp_to" CssClass="form-control" type="date" runat="server"></asp:TextBox>
                </div>
            </div>
            </div>
            
            <div class="clearfix"></div>
             <ul class="pre">
                <li>Remarks</li>
                <li><asp:TextBox ID="txt_remarks" TextMode="MultiLine" style=" resize:none; width:100%;" runat="server"></asp:TextBox></li>
             </ul>
 

                <asp:Button ID="Button1" runat="server" Text="Compute" Visible="false" OnClick="click_compute" CssClass="btn btn-primary load-nobel"/>
            
                <asp:Button ID="Button3" runat="server" Text="Mandatory" Visible="false" OnClick="click_computehdmfphic" CssClass="btn btn-primary load-nobel" />
                <asp:Button ID="Button2" runat="server" Text="Process Payroll" Visible="false" OnClick="click_proccess_dtr" CssClass="btn btn-primary load-nobel"/>
                <asp:Button ID="Button4" runat="server" Text="Refresh" Visible="false"  OnClick="refresh" CssClass="btn btn-primary load-nobel"/>
         
            
            
             
           <%-- <div id="container" style="overflow: scroll; overflow-x: hidden;">
            </div>--%>
             <div id="HeaderDiv"></div>
             <div id="DataDiv" style="overflow: auto; border: 0px solid #286090; width: 100%; height: 300px; margin-top:1px; " onscroll="Onscrollfnction();">

           <asp:GridView ID="grid_item" runat="server"  AutoGenerateColumns="False" CssClass="table table-striped table-bordered">
            <HeaderStyle CssClass="HeaderFreez" BackColor="#EBF3FF" Font-Size="13px" Font-Names="Times New Roman, Times, serif"
                 BorderColor="Black" BorderStyle="Ridge" BorderWidth="2px" />
            <Columns>
             <asp:TemplateField HeaderText="Employee" >
                <ItemTemplate> 
                    <asp:linkbutton ID="lnk_employee" OnClick="recom" runat="server" style=" font-size:9px;"    Text='<%#bind("employee")%>'></asp:linkbutton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="paytype" HeaderText="Payroll Type" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
            <asp:BoundField DataField="taxcode" HeaderText="Tax Code"  ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
            <asp:BoundField DataField="taxtable" HeaderText="Tax Table" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
            <asp:BoundField DataField="basicpay" HeaderText="Basic Pay" DataFormatString="{0:n2}"/>
            <asp:BoundField DataField="lateamt" HeaderText="Late Amt."/>
            <asp:BoundField DataField="undertimeamt" HeaderText="UT Amt."/>
            <asp:BoundField DataField="absentamt" HeaderText="Absent Amt."/>
            <asp:BoundField DataField="regularpay" HeaderText="Reg.  Pay"/>
            <asp:BoundField DataField="nightpay" HeaderText="Night Pay"/>
            <asp:BoundField DataField="otamt" HeaderText="OT Pay"/>
            <asp:BoundField DataField="restdaypay" HeaderText="RD Pay"/>
            <asp:BoundField DataField="leavew/pay" HeaderText="Leave Pay"/>
            <asp:BoundField DataField="hollidaypay" HeaderText="Prem. Pay"/>
            <asp:BoundField DataField="otherincometaxable" HeaderText="TI"/>
            <asp:BoundField DataField="grossincome" HeaderText="GROSS"/>
            <asp:BoundField DataField="ssscontribution" HeaderText="SSS"/>
            <asp:BoundField DataField="phiccontribution" HeaderText="PHIC."/>
            <asp:BoundField DataField="mdmfcontribution" HeaderText="HDMF"/>
            <asp:BoundField DataField="withholdingtax" HeaderText="WHT"/>
            <asp:BoundField DataField="otherdeduction" HeaderText="OD"/>
            <asp:BoundField DataField="otherincomenontax" HeaderText="NTI"/>
            <asp:BoundField DataField="netincome" HeaderText="NET"/>
             <asp:BoundField DataField="empid"  ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
             <asp:BoundField DataField="row"  ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
            </Columns>
            </asp:GridView>
            </div>
        </div>
    </div>
</div>
<div id="panelOverlay" visible="false" runat="server" class="Overlay"></div>
<div id="panelPopUpPanel" runat="server" visible="false" class="PopUpPanel pop-a">
<asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="close"  runat="server"/>
    <ul class="input-form">
      
        <li style=" font-size:medium; font-weight:bold;">EMPLOYEE <span style=" float:right; color:Red;"><asp:Label ID="lbl_empppp" runat="server" Text="Label"></asp:Label></span></li>
        <li>
            <ul>
                <li>SSS</li>
                <li><asp:TextBox ID="txt_ssssss" runat="server"  autocomplete="off" onclick="this.select();" ClientIDMode="Static" onkeyup="decimalinput(this);"></asp:TextBox></li>
                <li>PHIC</li>
                <li><asp:TextBox ID="txt_pppppp" runat="server"  autocomplete="off" onclick="this.select();" ClientIDMode="Static" onkeyup="decimalinput(this);"></asp:TextBox></li>
                <li>HDMF</li>
                <li><asp:TextBox ID="txt_hhhhhh" runat="server"  autocomplete="off" onclick="this.select();" ClientIDMode="Static" onkeyup="decimalinput(this);"></asp:TextBox></li>
            </ul>
        </li>
        <li style=" font-size:medium; font-weight:bold;">EMPLOYER</li>
         <li>
            <ul>
                <li>SSS</li>
                <li><asp:TextBox ID="txt_ssssss_er" runat="server"  AutoComplete="off" onclick="this.select();" ClientIDMode="Static" onkeyup="decimalinput(this);"></asp:TextBox></li>
                <li>SSS - EC</li>
                <li><asp:TextBox ID="txt_ssssss_er_ec" runat="server"  autocomplete="off" onclick="this.select();" ClientIDMode="Static" onkeyup="decimalinput(this);"></asp:TextBox></li>
                <li>PHIC</li>
                <li><asp:TextBox ID="txt_pppppp_er" runat="server"  autocomplete="off" onclick="this.select();" ClientIDMode="Static" onkeyup="decimalinput(this);"></asp:TextBox></li>
                <li>HDMF</li>
                <li><asp:TextBox ID="txt_hhhhhh_er" runat="server"  autocomplete="off" onclick="this.select();" ClientIDMode="Static" onkeyup="decimalinput(this);"></asp:TextBox></li>
            </ul>
        </li>
        <li><hr /></li>
        <li><asp:Button ID="Button5" runat="server" OnClick="save_recom" Text="Save" CssClass="btn btn-primary" /></li>
    </ul>
    <asp:HiddenField ID="hdn_gross" runat="server" />
    <asp:HiddenField ID="hdn_grossdummy" runat="server" />
    <asp:HiddenField ID="hdn_od" runat="server" />
    <asp:HiddenField ID="hdn_nti" runat="server" />
    <asp:HiddenField ID="hdn_row" runat="server" />
</div>
<script src="vendors/jquery.inputmask/dist/min/jquery.inputmask.bundle.min.js"></script>
<script type='text/javascript' src="script/inputmasking/4lenghtshiftdosjquery-1.11.0.js"></script>
<script type='text/javascript'>
    $(window).load(function () {
        $(":input").inputmask();
    });

    $(".load-nobel").click(function () {
        $("#load").removeClass("hide");
        $("#loader").removeClass("hide");
    });
</script>
    <div id="load" class="Overlay hide"></div>
        <div id="loader"  class="nobel hide">
            <img src="style/images/loading.gif" alt="loading" />
        </div>
    <asp:HiddenField ID="eec" runat="server"/>
        <asp:HiddenField ID="hdn_gds" runat="server"/>
</asp:Content>
