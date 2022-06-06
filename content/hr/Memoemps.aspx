<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Memoemps.aspx.cs" Inherits="content_hr_Memoemps" MasterPageFile="~/content/site.master"%>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
   <script type="text/javascript">
       jQuery.noConflict();
       (function ($) {
           $(function () {
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
 </asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_memo">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Compose Memo</h3>
    </div>  
    <div class="title_right">
        <ul>
            <li><a href="MemoList"><i class="fa fa-info-circle"></i>  Memo</a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>Create Memo</li>
        </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
            <ul class="input-form">
                <li>Date</li>
                <li><asp:TextBox ID="txt_date" CssClass="datee" runat="server"></asp:TextBox></li>
                <li>Subject</li>
                <li><asp:TextBox ID="txt_subject" runat="server"></asp:TextBox></li>
                <li>From</li>
                <li><asp:TextBox ID="txt_memo_from" runat="server"></asp:TextBox></li>
                <li>To</li>
                <li>
                    <div class="left" style=" width:calc(100% - 70px)">
                        <div id="all" runat="server" >
                            <asp:DropDownList ID="ddl_dtrfile" runat="server" CssClass="minimal"></asp:DropDownList>
                        </div>
                        <div id="employee" runat="server" visible="false" >
                            <asp:TextBox ID="txt_from" ClientIDMode="Static" runat="server" placeholder="From"></asp:TextBox>
                        </div>
                        <div id="payrollgroup" runat="server" class="search_result" visible="false">
                            <div class="styled-select">
                                <asp:DropDownList ID="ddl_payroll_group" runat="server"  CssClass="minimal" AutoPostBack="true" ></asp:DropDownList>
                            </div>
                        </div>
                        <div id="branch" runat="server" class="search_result" visible="false">
                            <div class="styled-select">
                                <asp:DropDownList ID="drop_branch" runat="server"  CssClass="minimal" AutoPostBack="true" ></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <asp:LinkButton ID="img_filter" runat="server" OnClick="click_filter" CssClass="right btn btn-link" style=" font-size:20px; margin:3px 8px"><i class="fa fa-send (alias)"></i></asp:LinkButton>
                    <div class="clearfix"></div>
                </li>
                <li>Memo Description</li>
                <li><asp:TextBox ID="txt_description" ClientIDMode="Static" TextMode="MultiLine" runat="server"></asp:TextBox></li>

                <li>Attach File</li>
                <li> <asp:FileUpload runat="server" ID="file_upload" Multiple="Multiple" accept=".xlsx,.xls,image/*,.doc, .docx,.ppt, .pptx,.txt,.pdf" /> </li>

                <li class="none">Other Information</li>
                <li class="none"><asp:TextBox ID="txt_notes" TextMode="MultiLine" runat="server"></asp:TextBox></li>
                <li><hr /></li>
                <li><asp:Button ID="btn_save" runat="server" Text="Save" onclick="btn_save_Click" CssClass="btn btn-primary" /></li>
            </ul>
        </div>
    </div>
</div>
 

<div id="Div1" runat="server" visible="false" class="Overlay"></div>
<div id="Div2" runat="server" visible="false" class="PopUpPanel modal-medium">
    <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
    <asp:Label ID="lbl_class" ForeColor="Red" runat="server" Text=""></asp:Label> 
    <ul class="input-form">     
        <li> 
            <asp:RadioButtonList ID="memo_to" runat="server">
                <asp:ListItem Value="1">Payroll Group</asp:ListItem>
                <asp:ListItem Value="2">Employee</asp:ListItem>
                <asp:ListItem Value="3">Branch</asp:ListItem>
                
            </asp:RadioButtonList>
       </li>
        <li><hr /></li>
        <li><asp:Button ID="Button2" runat="server" Text="Save" OnClick="click_save" CssClass="btn btn-primary" /></li>  
    </ul>
</div>


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