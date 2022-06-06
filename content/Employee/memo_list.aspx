<%@ Page Language="C#" AutoEventWireup="true" CodeFile="memo_list.aspx.cs" Inherits="content_Employee_memo_list" MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_memo_list">
    <style>
        .marlon { width:99%}
        .marlon i {color:#649fec;float:left;margin:3px 10px 0 0}
        .sub-marlon { float:left; width:calc(100% - 30px)}
        .sub-marlon span { display:block}
        .ann-date { float:right; margin-top:11px; color:#444; font-size:10px}
        .tbl { width:100%}
        .tbl,  .tbl tr,  .tbl th , .tbl td {border:none}
        .tbl tr {border-bottom:1px solid #eee;}
        .tbl tr:last-child {border:none}
        .tbl td {padding:10px 0}
        .tbl td span { line-height:20px}
        .editor {border:none}
        
        .no-border, .no-border td,.no-border th, .no-border tr { border:none}

    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_memo_list">
<section class="content-header">
    <h1>Announcement</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Announcement </li>
    </ol>
</section>
<%--<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            <div class="box-body table-responsive">
                <div id="alert" runat="server" class="alert alert-default">
                    <i class="fa fa-info-circle"></i>
                    <span>No record found</span>
                </div>
            </div>
          </div>
        </div>
    </div>
</section>--%>


<section class="content">
<div class="row">
    <div class="col-md-12">
        <div class="box box-primary">
            <div class="box-body">
            <div id="div_msg" runat="server" class="alert alert-default">
                    <i class="fa fa-info-circle"></i>
                    <span>No record found</span>
                </div>
        <asp:Panel ID="panel_display" runat="server">
            <asp:GridView ID="grid_view" runat="server" AllowPaging="true" PageSize="10" OnPageIndexChanging="gridview_PageIndexChanging" ShowHeader="false" AutoGenerateColumns="false" CssClass="tbl" onrowdatabound="gv_bound">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="rd" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:TemplateField> 
                        <ItemTemplate>
                          <asp:LinkButton ID="s" OnClick="view" Text="cancel" runat="server"> 
                            <div class="marlon">
                                <asp:Label ID="Label3" runat="server"><i class="fa fa-circle"></i></asp:Label>
                                <div class="sub-marlon">
                                    <asp:Label ID="Label2" runat="server" Text=<%# Eval("memo_date") %> CssClass="ann-date" ></asp:Label>
                                    <asp:Label ID="lbl_from" runat="server" Text='<%# Eval("memo_from") %>' Font-Bold="true" Font-Size="13px" style=" text-transform: uppercase"></asp:Label>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("memo_subject") %>'></asp:Label>
                                </div>
                            </div>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:Panel>
        <asp:Panel ID="panel_read" runat="server" >
                <div class="row">
                <div class="col-sm-12 mail_view">
                <div class="inbox-body">
                    <div class="mail_heading row">
                    <div class="col-md-8">
                        <div class="btn-group">
                            <asp:LinkButton ID="lb_back" OnClick="click_back" runat="server" data-placement="top" data-toggle="tooltip" data-original-title="Back" class="btn btn-sm btn-default"><i class="fa fa-arrow-circle-left"></i></asp:LinkButton>
                            <button class="btn btn-sm btn-default" type="button" data-placement="top" data-toggle="tooltip" data-original-title="Print"><i class="fa fa-print"></i></button>
                            <button class="btn btn-sm btn-default" type="button" data-placement="top" data-toggle="tooltip" data-original-title="Trash"><i class="fa fa-trash-o"></i></button>
                        </div>
                    </div>
                    <div class="col-md-4 text-right">
                        <p class="date"><asp:Label ID="l_date" runat="server"></asp:Label></p>
                    </div>
                    <div class="col-md-12">
                        <h4><asp:Label ID="l_subject" runat="server"></asp:Label></h4>
                        <table>
                            <tr>
                                <td style=" width:50px">From</td>
                                <td><asp:Label ID="l_from" runat="server" Text="Management" ></asp:Label></td>
                            </tr>
                            <tr>
                                <td>To</td>
                                <td><asp:Label ID="l_to" runat="server" Text=""></asp:Label></td>
                            </tr>
                        </table>
                        <hr />
                    </div>
                    </div>
                    <div id="viewer" runat="server" class="view-mail">
                    </div>

                    <br />
                    <br />
                    <br />
                    <asp:Label ID="lbl_attach" runat="server" Text=""></asp:Label>
                     <asp:GridView ID="grid_img" runat="server" AutoGenerateColumns ="False" CssClass="no-border" >
                        <Columns>
                            <asp:BoundField DataField="id" ItemStyle-Font-Size="0" HeaderStyle-Width="0" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <linkbutton></linkbutton>
                                    <asp:LinkButton runat="server" ID="lnk_btn" Text='<%# Eval("filename") %>' ></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnk_download" ForeColor="Red" OnClick="downloadreqfiles" Text="Download"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                   

                </div>
                
            </div>
        </asp:Panel>
            </div>
        </div>
    </div>
</div>
</section>
<div id="Div1" runat="server" visible="false" class="Overlay"></div>
<div id="Div2" runat="server" visible="false" class="PopUpPanel modal-cover">
    <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
    <div class="editor">
        <asp:Label ID="lbl_desc" runat="server"></asp:Label>
    </div>
</div>
<asp:HiddenField ID="key" runat="server" />
<asp:HiddenField ID="hf_line" runat="server" />
</asp:Content>


