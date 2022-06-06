<%@ Page Language="C#" AutoEventWireup="true" CodeFile="underTime.aspx.cs" Inherits="content_Employee_underTime" MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_coop_list">
    <script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>   
    <script type="text/javascript" src="script/myJScript.js"></script>
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
   <script type="text/javascript">
       jQuery.noConflict();
       (function($) {
           $(function() {
               $(".datee").datepicker({ changeMonth: true,
                   yearRange: "-100:+0",
                   changeYear: true
               });
           });
       })(jQuery);
    </script>
    <link href="style/csstimepicker/timepicki.css" rel="stylesheet"/>
    <style type="text/css">
    @media only screen and (max-width: 1200px) {
		input[type=time] {
				margin-top:13px !important
		}
    }
    </style>
</asp:Content>


<asp:Content ContentPlaceHolderID="content" ID="content_udertime" runat="server">

<section class="content-header">
    <h1>Undertime Application</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li><a href="KIOSK_undertime"> Undertime</a></li>
    <li class="active">Undertime Application</li>
    </ol>
</section>
<section class="content">
   
    <div class="box box-primary">
    <div class="box-body">
        <div class="form-group none">
        <asp:Label ID="lbl_err" style=" color:Red;" runat="server"></asp:Label>
        <asp:RadioButtonList ID="rbl_class" runat="server" CssClass="radio">
                    <asp:ListItem Selected>Personal Undertime</asp:ListItem>
                    <asp:ListItem>Authorized Undertime</asp:ListItem>
                    </asp:RadioButtonList>
        </div>
    <div class="form-group">
        <label>Date</label>
        <asp:Label ID="lbl_date" style=" color:Red;" runat="server" Text=""></asp:Label>
        <asp:TextBox ID="txt_date" CssClass="datee form-control" AutoComplete="False" runat="server"></asp:TextBox>
    </div>
    <div class="form-group">
        <label>Time Out</label>
                   
        <asp:Label ID="lbl_time" style=" color:Red;" runat="server" Text=""></asp:Label>
        <div class="row">
            <div class="col-lg-6">
                <asp:TextBox ID="txt_date11" CssClass="datee form-control"   runat="server"></asp:TextBox>
            </div>
            <div class="col-lg-6">
                <asp:TextBox ID="txt_time"  Text="18:30" type="Time" CssClass="form-control" style="line-height:normal" runat="server"></asp:TextBox>
            </div>
        </div>

    </div>
    <div class="form-group no-margin">
        <label>Reason</label>
            <asp:Label ID="lbl_reason" style=" color:Red;" runat="server" Text=""></asp:Label>
        <asp:TextBox ID="txt_reason" TextMode="MultiLine" CssClass="form-control" AutoComplete="False" runat="server"></asp:TextBox>
    </div>

    <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="table">
        <Columns>
            <asp:BoundField DataField="id"/>
            <asp:BoundField DataField="date_filed" HeaderText="Date Filed" DataFormatString="{0:MM/dd/yyyy}"/>
            <asp:BoundField DataField="time" HeaderText="Time"/>
            <asp:BoundField DataField="reason" HeaderText="Reason"/>
            <asp:BoundField DataField="status" HeaderText="Status"/>  

                <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="lnk_can" ForeColor="Red" OnClick="view" Text="cancel" runat="server"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    </div>
    <div class="box-footer pad">
        <asp:Button ID="btn_save" runat="server" OnClick="addunder" Text="Save" CssClass="btn btn-primary"/>
    </div>
    </div>
     
</section>

<asp:HiddenField ID="id" runat="server" />
<div id="Div1" runat="server" visible="false" class="Overlay2"></div>
<div id="Div2" runat="server" visible="false" class="PopUpPanel3">
<asp:LinkButton ID="LinkButton1" runat="server" OnClick="cpop" class="close" ><img src="style/img/closeb.png" /></asp:LinkButton>
    <div class="fileuploadDiv">
           <ul class="ul">
                   <li>Reason</li>
                   <li><asp:TextBox ID="txt_re" TextMode="MultiLine" runat="server"></asp:TextBox></li>
                  <li><asp:Button ID="Button1" runat="server" OnClick="cancel" Text="Save" /></li>
            </ul>
    </div>
</div>
    <script src="script/jtimepicker/jquery.min.js" type="text/javascript"></script>
    <script src="script/jtimepicker/timepicki.js" type="text/javascript"></script>
    <script type="text/javascript">

        $('.nobel').timepicki();
       
    </script>
    <script src="script/jtimepicker/bootstrap.min.js" type="text/javascript"></script>

</asp:Content>
