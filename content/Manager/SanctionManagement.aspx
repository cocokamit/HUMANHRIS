<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SanctionManagement.aspx.cs" Inherits="content_Manager_SanctionManagement" MasterPageFile="~/content/site.master"%>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_dtr">
    <!--SELECT-->
    <link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />

    <script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        jQuery.noConflict();
        (function ($) {
            $(function () {
                $(".txt_f").datepicker();
                $(".txt_t").datepicker();
            });
        })(jQuery);
    </script>
    <style type="text/css">
        input[type="date"].form-control,input[type="time"].form-control { line-height:normal !Important}
        .select2 { min-width:200px}
        .table th{font-size:12px !important}
        .table { font-size:13x !important}
        .table a  { font-size:12px !important}
        .modal { position:fixed !important}
    </style>

</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_dtr">
<section class="content-header">
    <h1>SANCTION</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">DTR</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            <div class="box-header">
                <div class="input-group">
                    <asp:DropDownList ID="drop_emp" OnSelectedIndexChanged="changedisplay" ClientIDMode="Static" AutoPostBack="true"  CssClass="select pull-left" style="margin-right:5px" Width="200px" runat="server" ></asp:DropDownList>
                    <button type="button" class="btn btn-default" data-toggle="modal" data-target="#modal-sanction">Sanction</button>
                </div>
            </div>
            <div class="box-body" style="padding-top:0 !important">  
                <asp:GridView ID="grid_sanctions" OnRowDataBound="tawesie" runat="server" EmptyDataText="No data found!" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="incidentdate" HeaderText="Date" />
                        <asp:BoundField DataField="sanctioncode" HeaderText="Sanction" />
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
                        <asp:TemplateField HeaderText="Action" ShowHeader="False" >
                            <ItemTemplate>
                                <asp:LinkButton ID="lbupload" ToolTip="Verify" runat="server" Text="uploadirfile" Visible="true" OnClick="irfilesupload"><i class="fa fa-paperclip"></i></asp:LinkButton>
                                <asp:linkbutton ID ="lbdonwloadir" ToolTip="Download" CommandName='<%# Eval("id") %>' Visible="false" runat="server" OnClick="downloadirfiles"><i class="fa fa-download"></i></asp:linkbutton>
                                <asp:LinkButton ID ="trash" ToolTip="Delete" runat="server" CausesValidation="false" OnClick="deletesanctions"><i class="fa fa-trash"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="userid" HeaderText="Issued" />
                    </Columns>
                </asp:GridView>
            </div>
          </div>
        </div>
    </div>
</section>
                            <asp:Label ID="l_msg" runat="server" CssClass="block" style="position:inherit"></asp:Label>
<div id="panelOverlay" runat="server" visible="false" class="Overlay"></div>
<div id="suspendpanel" runat="server" visible="false" class="PopUpPanel modal-medium">
    <asp:ImageButton ID="imgclose" ImageUrl="~/style/img/closeb.png" OnClick="closeupload" runat="server"/>
</div>
<div class="modal fade" id="modal-sanction">
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
    <asp:HiddenField ID="HiddenField6" runat="server" />
    <asp:HiddenField ID="HiddenField7" runat="server" />
    <asp:HiddenField ID="trn_det_id" runat="server" />
    <asp:HiddenField ID="sanctionid" runat="server" />
</div>
<asp:HiddenField ID="payrange" runat="server" />
</asp:Content>

<asp:Content ID="footer" ContentPlaceHolderID="footer" runat="server">
<!--Select-->
<script type="text/javascript" src="vendors/select2/dist/js/select2.full.min.js"></script>
<script type="text/javascript">
    (function ($) {
        $('.select2').select2()
    })(jQuery);
</script>
</asp:Content>