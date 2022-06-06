<%@ Page Language="C#" AutoEventWireup="true" CodeFile="travel.aspx.cs" Inherits="content_Employee_travel" MasterPageFile="~/content/site.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
 
    <script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
     <link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />
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
    <script type="text/javascript" src="style/plugins/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="style/plugins/tinymce/tinymce.min.js"></script>
    <script src="script/auto/myJScript.js" type="text/javascript"></script>
    <style type="text/css">
                   
        @media only screen and (max-width: 853px) 
        {
            
             .select2-selection { width:200px !important}
        }
    </style>
 </asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_travel">
<section class="content-header">
    <h1>Travel Application</h1>
    <ol class="breadcrumb">
        <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
        <li class="active">Travel Application</li>
    </ol>
</section>
<section class="content">
        <div class="box box-primary">
            <div class="box-body">
                <div class="form-group">
                    <label>Purpose</label>
                    <asp:Label ID="lbl_purpose" style=" color:Red;" runat="server" Text=""></asp:Label>
                    <asp:TextBox ID="txt_purpose" CssClass="form-control" runat="server"></asp:TextBox>
                 </div>
                 <div class="row">
                    <div class="col-lg-6">
                        <div class="form-group">
                            <label>Start Date</label>
                           <asp:Label ID="lbl_sdate" style=" color:Red;" runat="server" Text=""></asp:Label>
                            <asp:TextBox ID="txt_sdate" CssClass="datee form-control" runat="server"></asp:TextBox>
                         </div>
                    </div>
                    <div class="col-lg-6">
                        <div class="form-group">
                            <label>End Date</label>
                           <asp:Label ID="lbl_edate" style=" color:Red;" runat="server" Text=""></asp:Label>
                            <asp:TextBox ID="txt_edate" CssClass="datee form-control" runat="server"></asp:TextBox>
                         </div>
                    </div>
                 </div>

             <div class="form-group">
                <label>Expected Budget</label>
                <asp:Label ID="lbl_expected" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_expected" CssClass="form-control" Text="0.00" onkeyup="intinput(this)" runat="server"></asp:TextBox>
             </div>

             <div class="form-group">
                <label>Actual Budget</label>
                <asp:Label ID="lbl_actual" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_actual" CssClass="form-control" Text="0.00" onkeyup="intinput(this)" runat="server"></asp:TextBox>
             </div>

             <div id="Div1" class="form-group" runat="server" visible="false" >
                <label>Description</label>
                <asp:Label ID="lbl_description" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_description" TextMode="MultiLine" CssClass="form-control"  runat="server"></asp:TextBox>
             </div>

              <div id="Div2" class="form-group" runat="server" visible="false">
                <label>Description</label>
                <asp:Label ID="lbl_notes" style=" color:Red;" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txt_notes" TextMode="MultiLine" CssClass="form-control"  runat="server"></asp:TextBox>
             </div>
             <div class="table-responsive">
             <asp:GridView ID="grid_item" runat="server" ShowFooter="True" onrowdeleting="grid_item_RowDeleting" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-form no-margin">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="15px" HeaderText="No."> 
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("RowNumber") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>             
                            <asp:ImageButton ID="btn" runat="server" ImageUrl="~/style/img/add.png" OnClick="ButtonAdd_Click"  />
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Place of Visit">
                        <ItemTemplate>
                        <asp:Label ID="lbl_date" runat="server" CssClass="na" Text=""></asp:Label>
                        <asp:Label ID="lbl_date_desp"  runat="server" Text=""></asp:Label>
                        <asp:TextBox ID="txt_date" autocomplete="off" CssClass="form-control"  runat="server" style=" min-width:200px"></asp:TextBox> 
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Travel Mode" >
                    <ItemTemplate>

                    <asp:Label ID="lbl_time_in" runat="server" CssClass="na" Text=""></asp:Label>
                    <asp:Label ID="lbl_time_in_desp"  runat="server" Text=""></asp:Label>
                    <asp:DropDownList ID="txt_time_in" runat="server" CssClass="select2" Width="100%">
                        <asp:ListItem> </asp:ListItem>
                        <asp:ListItem>By Car</asp:ListItem>
                        <asp:ListItem>By Train</asp:ListItem>
                        <asp:ListItem>By Bus</asp:ListItem>
                        <asp:ListItem>By Plane</asp:ListItem>
                        <asp:ListItem>By Taxi</asp:ListItem>
                        <asp:ListItem>By Rental Car</asp:ListItem>
                        <asp:ListItem>By Motor Bike</asp:ListItem>
                        <asp:ListItem>By Grab</asp:ListItem>
                        <asp:ListItem>By Uber</asp:ListItem>
                        <asp:ListItem>By Lyft</asp:ListItem>
                    </asp:DropDownList>
                    </ItemTemplate>
                    </asp:TemplateField>

<asp:TemplateField HeaderText="Arrangement Type" >
<ItemTemplate>
<asp:Label ID="lbl_time_out" runat="server" CssClass="na" Text=""></asp:Label>
<asp:Label ID="lbl_time_out_desp"  runat="server" Text=""></asp:Label>
<asp:DropDownList ID="txt_time_out" runat="server"  CssClass="select2"  Width="100%">
<asp:ListItem> </asp:ListItem>
<asp:ListItem>Personal Arrangement</asp:ListItem>
<asp:ListItem>Hotel</asp:ListItem>
<asp:ListItem>Guest House</asp:ListItem>
<asp:ListItem>Motel</asp:ListItem>
<asp:ListItem>AirBnB</asp:ListItem>
</asp:DropDownList>

</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Reason" Visible="false">
<ItemTemplate>
<asp:Label ID="lbl_reason" runat="server" CssClass="na" Text=""></asp:Label>
<asp:Label ID="lbl_reason_desp"  runat="server" Text=""></asp:Label>
<asp:DropDownList ID="txt_reason" runat="server" DataSourceID="sql_income" DataTextField="manual_type" DataValueField="Id" ClientIDMode="Static" AppendDataBoundItems="true" >
<asp:ListItem></asp:ListItem>
</asp:DropDownList>
<asp:SqlDataSource ID="sql_income" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>"  SelectCommand="select * from time_adjustment">
</asp:SqlDataSource>
</ItemTemplate>
</asp:TemplateField>


<asp:TemplateField ShowHeader="False">
<ItemTemplate>
<asp:ImageButton ID="can" runat="server" CausesValidation="false" CommandName="Delete" ImageUrl="~/style/img/delete.png" />
</ItemTemplate>
<ItemStyle Width="10px" />
</asp:TemplateField>

</Columns>            
</asp:GridView>
            </div>
            <asp:Button ID="btn_save" runat="server" Text="Save" onclick="btn_save_Click" CssClass="btn btn-primary"/>

            </div>
        </div>
</section>

<script type="text/javascript">
    $(document).ready(function () {
        TinyMCEStart('#txt_description', null);
    });

    function TinyMCEStart(elem, mode) {
        var plugins = [];
        if (mode == 'extreme') {
            plugins = [""]
        }
        tinymce.init({ selector: elem,
            theme: "modern",
            plugins: plugins,
            //content_css: "css/style.css",
            toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | print preview media fullpage | forecolor backcolor emoticons",
            style_formats: [
			{ title: 'Header 2', block: 'h2', classes: 'page-header' },
			{ title: 'Header 3', block: 'h3', classes: 'page-header' },
			{ title: 'Header 4', block: 'h4', classes: 'page-header' },
			{ title: 'Header 5', block: 'h5', classes: 'page-header' },
			{ title: 'Header 6', block: 'h6', classes: 'page-header' },
			{ title: 'Bold text', inline: 'b' },
			{ title: 'Red text', inline: 'span', styles: { color: '#ff0000'} },
			{ title: 'Red header', block: 'h1', styles: { color: '#ff0000'} },
			{ title: 'Example 1', inline: 'span', classes: 'example1' },
			{ title: 'Example 2', inline: 'span', classes: 'example2' },
			{ title: 'Table styles' },
			{ title: 'Table row 1', selector: 'tr', classes: 'tablerow1' }
		]
        });
    }
</script>

<script type="text/javascript" src="vendors/select2/dist/js/select2.full.min.js"></script>
<script type="text/javascript">
    (function ($) {
        $('.select2').select2()
    })(jQuery);
</script>

   <asp:HiddenField ID="key" runat="server" />
</asp:Content>


