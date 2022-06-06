<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ot.aspx.cs" Inherits="content_Employee_ot" MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_ot_list">
<script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">


        jQuery.noConflict();
        (function ($) {
            $(function () {
                  $(".datee").datepicker();
            });
        })(jQuery);
 
    </script>


    <link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>

</asp:Content>


<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_ot">


<%--<ul>

<li>Date</li>
<li><asp:TextBox ID="txt_otd" runat="server" AutoComplete="off" ClientIDMode="Static" ></asp:TextBox></li>

<li>Time In</li>
<li>
     
      <asp:DropDownList ID="ddl_hrs_in" runat="server"  Width="48px" style=" margin-left:2px">
            <asp:ListItem value="" disabled selected>HR</asp:ListItem>
      </asp:DropDownList>

      <asp:DropDownList ID="ddl_minute_in" runat="server" Width="48px" style=" margin-left:2px">
        <asp:ListItem value="" disabled selected>MM</asp:ListItem>
      </asp:DropDownList> 

    <asp:DropDownList ID="ddl_am_pm_in" runat="server" Width="49px" style=" margin-left:2px">
        <asp:ListItem Value="AM">AM</asp:ListItem>
        <asp:ListItem Value="PM">PM</asp:ListItem>
    </asp:DropDownList>
</li>

<li>Time Out</li>
<li>
     
     <asp:DropDownList ID="ddl_hrs_out" runat="server"  Width="48px" style=" margin-left:2px">
            <asp:ListItem value="" disabled selected>HR</asp:ListItem>
     </asp:DropDownList>

      <asp:DropDownList ID="ddl_minute_out" runat="server" Width="48px" style=" margin-left:2px">
        <asp:ListItem value="" disabled selected>MM</asp:ListItem>
      </asp:DropDownList> 

    <asp:DropDownList ID="ddl_am_pm_out" runat="server" Width="49px" style=" margin-left:2px">
        <asp:ListItem Value="AM">AM</asp:ListItem>
        <asp:ListItem Value="PM">PM</asp:ListItem>
    </asp:DropDownList>
  
    
</li>

<li>OT HRS</li>
<li><asp:TextBox ID="txt_OT" runat="server" onkeyup="intinput(this)" MaxLength="1"></asp:TextBox></li>
<li>OT Night HRS</li>
<li><asp:TextBox ID="txt_night_ot" runat="server" onkeyup="intinput(this)" MaxLength="1"></asp:TextBox></li>
<li>Remarks</li>
<li><asp:TextBox ID="txt_lineremarks" runat="server" TextMode="MultiLine"></asp:TextBox></li>
<li></li>
</ul>
--%>

<asp:GridView ID="grid_item" runat="server" ShowFooter="True" onrowdeleting="grid_item_RowDeleting" AutoGenerateColumns="False" CssClass="tb" GridLines="Vertical">
    <Columns>
         <asp:TemplateField ItemStyle-Width="15px"> 
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# Bind("RowNumber") %>'></asp:Label>
            </ItemTemplate>
            <FooterTemplate >             
                <asp:ImageButton ID="btn" runat="server" ImageUrl="~/style/img/add.png" OnClick="ButtonAdd_Click"  />
            </FooterTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Date">
            <ItemTemplate>
             <asp:Label ID="lbl_date" runat="server" CssClass="na" Text=""></asp:Label>
               <asp:Label ID="lbl_date_desp"  runat="server" Text=""></asp:Label>
              <asp:TextBox ID="txt_date" Width="320px" CssClass="datee" autocomplete="off"  runat="server"></asp:TextBox> 
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Time In" ItemStyle-Width="180px">
            <ItemTemplate>
            
               <asp:Label ID="lbl_time_in" runat="server" CssClass="na" Text=""></asp:Label>
               <asp:Label ID="lbl_time_in_desp"  runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_time_in" ClientIDMode="Static" CssClass="nobel"  runat="server"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Time Out" >
            <ItemTemplate>
                <asp:Label ID="lbl_time_out" runat="server" CssClass="na" Text=""></asp:Label>
               <asp:Label ID="lbl_time_out_desp"  runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_time_out" ClientIDMode="Static" CssClass="nobel"  runat="server"></asp:TextBox>
              
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="OT Hrs">
            <ItemTemplate>
             <asp:Label ID="lbl_hrs" runat="server" CssClass="na" Text=""></asp:Label>
               <asp:Label ID="lbl_hrs_desp"  runat="server" Text=""></asp:Label>
              <asp:TextBox ID="txt_hrs" Width="320px" ClientIDMode="Static" autocomplete="off"  runat="server"></asp:TextBox> 
            </ItemTemplate>
        </asp:TemplateField>

         <asp:TemplateField HeaderText="OT Night Hrs">
            <ItemTemplate>
             <asp:Label ID="lbl_hrs_n" runat="server" CssClass="na" Text=""></asp:Label>
               <asp:Label ID="lbl_hrs_n_desp"  runat="server" Text=""></asp:Label>
              <asp:TextBox ID="txt_hrs_n" Width="320px" ClientIDMode="Static" autocomplete="off"  runat="server"></asp:TextBox> 
            </ItemTemplate>
        </asp:TemplateField>

          <asp:TemplateField HeaderText="Reason">
            <ItemTemplate>
             <asp:Label ID="lbl_reason" runat="server" CssClass="na" Text=""></asp:Label>
               <asp:Label ID="lbl_reason_desp"  runat="server" Text=""></asp:Label>
              <asp:TextBox ID="txt_reason" Width="320px" ClientIDMode="Static" autocomplete="off"  runat="server"></asp:TextBox> 
            </ItemTemplate>
        </asp:TemplateField>


        <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                  <asp:ImageButton ID="can" runat="server" CausesValidation="false" CommandName="Delete" ImageUrl="~/style/img/delete.png" />
            </ItemTemplate>
        </asp:TemplateField>

    </Columns>            
</asp:GridView>
<asp:Button ID="Button2" runat="server" OnClick="btn_save_Click" Text="SAVE" />

    <script src="script/jtimepicker/jquery.min.js" type="text/javascript"></script>
    <script src="script/jtimepicker/timepicki.js" type="text/javascript"></script>
    <script type="text/javascript">
        $('.nobel').timepicki();
       
    </script>
    <script src="script/jtimepicker/bootstrap.min.js" type="text/javascript"></script>
</asp:Content>


