<%@ Page Language="C#" AutoEventWireup="true" CodeFile="memo_list.aspx.cs" Inherits="content_hr_memo_list" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
<style>

 .no-border, .no-border td,.no-border th, .no-border tr { border:none}
</style>

</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_memo_list">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Memo</h3>
    </div>  
    <div class="title_right">
        <asp:LinkButton ID="lb_add" runat="server" Text="Compose" CssClass="btn btn-primary" OnClick="click_add"></asp:LinkButton>
    </div>
</div>
<div class="clearfix"></div>
 

<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
            <asp:Panel ID="panel_display" runat="server">
                <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false"  CssClass="table table-striped table-bordered">
                    <Columns>
                        <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:BoundField DataField="date_input" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Created"/>
                        <asp:BoundField DataField="memo_date" HeaderText="Memo Date" DataFormatString="{0:MM/dd/yyyy}"/>
                        <asp:BoundField DataField="memo_subject" HeaderText="Subject"/>
                        <asp:BoundField DataField="memo_from" HeaderText="From"/>
                        <asp:BoundField DataField="memo_description" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton Text="recipient" ID="lnk_view" OnClick="recipient" runat="server"><i class="fa fa-sliders"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="40px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div id="div_msg" runat="server" class="alert alert-empty">
                    <i class="fa fa-info-circle"></i>
                    <span>No record found</span>
                </div>
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
                                    <td><asp:Label ID="l_to" runat="server" Text="Riesol Amoroto, Marlon Abacahan, Nobel Caquilala"></asp:Label></td>
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
                     <asp:GridView ID="grid_img" runat="server" AutoGenerateColumns="False" CssClass="no-border" >
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

<div id="Div1" runat="server" visible="false" class="Overla"></div>
<div id="Div2" runat="server" visible="false" class="PopUpPane">
    <asp:LinkButton ID="lnk_close" runat="server"  class="close" OnClick="cpop" ><img src="style/images/closeb.png" alt="close" /></asp:LinkButton>

    <asp:Label ID="lbl_class" ForeColor="Red" runat="server" Text=""></asp:Label> 
    <asp:GridView ID="grid_rep" runat="server" AutoGenerateColumns="false"  CssClass="table">
        <Columns>
            <asp:BoundField DataField="Fullname" HeaderText="Full Name"/>
            <asp:BoundField DataField="stat" HeaderText="Status"/>
        </Columns>
    </asp:GridView>
</div>

 <asp:HiddenField ID="key" runat="server" />
</asp:Content>
