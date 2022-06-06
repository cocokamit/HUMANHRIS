<%@ Page Language="C#" AutoEventWireup="true" CodeFile="employee_schedule.aspx.cs" Inherits="content_Scheduler_employee_schedule" MasterPageFile="~/content/site.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .aspNetDisabled { color:#b2afaf}
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_emp_schedule">
<section class="content-header">
    <h1>Change Shift</h1>
    <ol class="breadcrumb">
    <li><a href="#"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Change Shift</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            <div class="box-body table-responsive">
                <div id="alert" runat="server" class="alert alert-default">
                    <i class="fa fa-info-circle"></i>
                    <span>No record found</span>
                </div>
                    <asp:GridView ID="grid_view" runat="server" PageSize="10" AllowPaging="true" OnPageIndexChanging="gridview_PageIndexChanging" OnRowDataBound="rowbound" AutoGenerateColumns="false" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="date" HeaderText="File Date" DataFormatString="{0:MM/dd/yyyy}"/>
                      
                    <asp:TemplateField HeaderText="Shift From">
                                <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("shiftcodeFrom")%>'
                                tooltip='<%# Eval("aa") %>' ></asp:Label>
                                </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Shift To">
                                <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("shiftcodeTo")%>'
                                tooltip='<%# Eval("bb") %>' ></asp:Label>
                                </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="status" HeaderText="Status"/>  
                     <asp:BoundField DataField="shiftcode_id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" OnClick="view" runat="server" CssClass="fa fa-info-circle border-right"></asp:LinkButton>
                            <asp:LinkButton ID="lnk_edit" OnClick="view1" runat="server" CssClass="fa fa-pencil border-right"></asp:LinkButton>
                            <asp:LinkButton ID="lnkcan" OnClick="cancel"   Text="cancel" runat="server"><i class="fa fa-trash  no-padding-right"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="120px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
          </div>
        </div>
    </div>
</section>

<div id="modal" runat="server" runat="server" class="modal fade in">
    <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="cpop" class="close" aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h4 class="modal-title">Time Adjustment</h4>
            </div>
            <div class="modal-body">
            </div>
        </div>
    </div>
</div>

<div id="Div1" runat="server" visible="false" class="Overlay"></div>
<div id="Div2" runat="server" visible="false" class="PopUpPanel">
    <asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
    <ul class="input-form">
        <li>Date</li>
        <li><asp:TextBox ID="txt_date" Enabled="false" runat="server"></asp:TextBox></li>
        <li>Shift Code</li>
        <li><asp:DropDownList ID="ddl_shiftcode" runat="server" CssClass="form-control"></asp:DropDownList></li>
        <li style="margin-top:5px">Remarks</li>
        <li><asp:TextBox ID="txt_remarks" runat="server" TextMode="MultiLine" CssClass="form-control" placeholder="remarks"></asp:TextBox></li>
        <li><hr /></li>
        <li><asp:Button ID="btn_save" runat="server" OnClick="click_save_changeshift" Text="Save" CssClass="btn btn-primary" /></li>
    </ul>
</div>



<div id="Div3" runat="server" visible="false" class="Overlay"></div>
<div id="Div4" runat="server" visible="false" class="PopUpPanel">
    <asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>
     <ul class="input-form">
       

        <li>Approver</li>
        <li>
            <asp:GridView ID="grid_approver" runat="server" AutoGenerateColumns="false" EmptyDataText="No record found" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="date" HeaderText="Date Approve" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="Fullname" HeaderText="Approver Name"/>
                    <asp:BoundField DataField="remarks" HeaderText="Remarks"/>
                    <asp:BoundField DataField="status" HeaderText="Status"/>
                </Columns>
            </asp:GridView>
       </li>

    </ul>
</div>

<asp:HiddenField ID="key" runat="server" />

</asp:Content>

<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer"></asp:Content>
