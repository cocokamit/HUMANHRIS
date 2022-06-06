<%@ Page Language="C#" AutoEventWireup="true" CodeFile="exit.aspx.cs" Inherits="content_hr_exit" MasterPageFile="~/content/MasterPageNew.master" %>


<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .search { margin:10px}
        .tts, .tts tr, .tts td, .tts th { border:none }
        .tts {border:1px solid #fff; width:100%}
        .tts a { color: #1D5C9A; font-size:13px; line-height:30px}
        .oik { margin:10px;}
        .Overla {  position:fixed; top:0px; bottom:0px; left:0px; right:0px; overflow:hidden; padding:0; margin:0; background-color:#000; filter:alpha(opacity=50); opacity:0.5; z-index:1000;}
        .PopUpPane {  padding:10px;  border-radius:4px; position: absolute; z-index: 1002;   width: 300px; top: 30%;left: 50%; margin-left: -130px; background-color: #fff; }
        .close { margin:-20px}
        .ull {list-style:none; margin-bottom:5px}
        .ull li { display:inline; }
        .ull li input[type]{ padding:5px}
        hr { margin:5px 0 5px 0}
        .styled-select { margin:5px 0 0 0; width:330px}
        .styled-select select { width:350px}
        .date-range { margin:5px 0 0 0}
        .date-range input[type=text] { background:none; width:125px}
    </style>
 


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

    <script type="text/javascript" src="style/plugins/jquery/jquery.min.js"></script>
<script type="text/javascript" src="style/plugins/tinymce/tinymce.min.js"></script>
 </asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_resignation" >

<h3>Exit Form</h3>

<table>

    <tr>
         <td>Exit Date: </td>
         <td>
             <asp:TextBox ID="txt_date" CssClass="datee" runat="server"></asp:TextBox>
         </td>
    </tr>

    <tr>
         <td>Employee: </td>
         <td>
         <asp:DropDownList ID="drop_emp" runat="server"></asp:DropDownList>
         </td>
    </tr>

     <tr>
         <td>Type of Exit </td>
         <td>
         <asp:DropDownList ID="drop_type" runat="server">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem>Resignation</asp:ListItem>
                <asp:ListItem>Retirement</asp:ListItem>
                <asp:ListItem>End of Contract</asp:ListItem>
                <asp:ListItem>End of Project</asp:ListItem>
                <asp:ListItem>Dismissal</asp:ListItem>
                <asp:ListItem>Layoff</asp:ListItem>
                <asp:ListItem>Termination by Mutual Agreement</asp:ListItem>
                <asp:ListItem>Forced Resignation</asp:ListItem>
                <asp:ListItem>End of Temporary Appointment</asp:ListItem>
                <asp:ListItem>Death</asp:ListItem>
                <asp:ListItem>Abandonment</asp:ListItem>
                <asp:ListItem>Transfer</asp:ListItem>
         </asp:DropDownList>
         </td>
    </tr>
    <tr>
         <td>Conducted Exit Interview: </td>
         <td>
         <asp:DropDownList ID="dro_con" runat="server">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem>Yes</asp:ListItem>
                <asp:ListItem>No</asp:ListItem>
         </asp:DropDownList>
           
         </td>
    </tr>

    <tr>
         <td>Exit Reason</td>
         <td>
             <asp:TextBox ID="txt_description" ClientIDMode="Static" TextMode="MultiLine" runat="server"></asp:TextBox>
         </td>
    </tr>

    <tr>
         <td>Other Information</td>
         <td>
             <asp:TextBox ID="txt_notes" TextMode="MultiLine" runat="server"></asp:TextBox>
         </td>
     </tr>
    


</table>
    <asp:Button ID="btn_save" runat="server" OnClick="save_resignation" Text="Save" />


<%--
<div id="Div1" runat="server" visible="false" class="Overla"></div>
<div id="Div2" runat="server" visible="false" class="PopUpPane">
    <asp:LinkButton ID="lnk_close" runat="server"  class="close" OnClick="cpop" ><img src="style/images/closeb.png" alt="close" /></asp:LinkButton>

    <asp:Label ID="lbl_class" ForeColor="Red" runat="server" Text=""></asp:Label> 
    <ul class="ul">     
        <li> <asp:RadioButtonList ID="memo_to" runat="server">
         
            <asp:ListItem Value="1">Payroll Group</asp:ListItem>
            <asp:ListItem Value="2">Employee</asp:ListItem>
             <asp:ListItem Value="3">Branch</asp:ListItem>
            <asp:ListItem Value="4">ALL</asp:ListItem>
        </asp:RadioButtonList> </li>



        <li><hr /></li>
        <li><asp:Button ID="Button2" runat="server" Text="Save" OnClick="click_save" CssClass="button" /></li>  
    </ul>
</div>--%>


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

     <asp:HiddenField ID="key" runat="server" />

</asp:Content>


