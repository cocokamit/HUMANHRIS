<%@ Page Language="C#" AutoEventWireup="true" CodeFile="travel_list.aspx.cs" Inherits="content_Employee_travel_list" MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_travel_list">
<style type="text/css">
    .aspNetDisabled  { color: #8a8a8a !important}
</style>
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_travellist">
<section class="content-header">
    <h1>Official Business Trip</h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Official Business Trip</li>
    </ol>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
          <div class="box box-primary">
            <div class="box-header">
                <div class="input-group input-group-sm" style="width: 300px;">
                    <asp:Button ID="btn_add" runat="server" OnClick="addtravel" Text="ADD" CssClass="btn btn-primary" />
                </div>
            </div>
            <div class="box-body no-pad-top">
                <asp:GridView ID="grid_view" runat="server"  AutoGenerateColumns="false" OnRowDataBound="rowbound" OnPageIndexChanging="gridview_PageIndexChanging" AllowPaging="true" PageSize="10" CssClass="table table-striped table-bordered no-margin">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="date_input" HeaderText="Date Filed" DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="purpose" HeaderText="Purpose" />
                    <asp:BoundField DataField="travel_start" HeaderText="Start"  DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="travel_end" HeaderText="End"  DataFormatString="{0:MM/dd/yyyy}"/>
                    <asp:BoundField DataField="stat" HeaderText="Status"/>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="lnk_view" OnClick="view" Text="view"><i class="fa fa-info-circle"></i></asp:LinkButton>
                            <asp:LinkButton runat="server" ID="lbDelete" OnClick="deleteOB" Text="view"><i class="fa fa-minus-circle"></i></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="80px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="notes" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                </Columns>
            </asp:GridView>
            <div id="div_msg" runat="server" class="alert alert-default">
                <i class="fa fa-info-circle"></i>
                <span>No record found</span>
            </div>
            </div>
          </div>
        </div>
    </div>
</section>

<div id="modal_delete" runat="server" class="modal fade in">
    <div class="modal-dialog">
    <div class="modal-content">
        <div class="modal-header">
        <asp:LinkButton ID="lb_close_delete" runat="server" OnClick="cpop" CssClass="close">&times;</asp:LinkButton>
        <h4 class="modal-title">Cancellation Request</h4>
        </div>
        <div class="modal-body">
            <div class="form-group no-margin">
                <label>Reason</label>
                <asp:TextBox ID="tbCancelRemarks" TextMode="MultiLine" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="box-footer pad">
            <asp:Button ID="btn_save" runat="server" OnClick="DeleteTransaction" Text="Cancel" CssClass="btn btn-primary"/>
        </div>
    </div>
    </div>
</div>

<div id="Div1" runat="server" visible="false" class="Overlay"></div>
<div id="Div2" runat="server" visible="false" class="PopUpPanel modal-large">
<asp:ImageButton ID="closest" ImageUrl="~/style/img/closeb.png" OnClick="cpop" runat="server"/>

    <div id="pnlAlert" runat="server" visible="false" class="alert alert-danger" style="padding:10px; margin-bottom:10px">
        <i class="fa fa-info-circle"></i>
        CANCELED TRANSACTION 
        <br />
        <p><asp:Label ID="lblNotes" runat="server"></asp:Label></p>
    </div>

 <div class="box-body table-responsive no-pad-top">
     <div class="form-group" runat="server" id="meals">
                <label>Meals</label>
                <asp:Label ID="lbl_meals"  runat="server" Text=""></asp:Label>
      </div>

      <div class="form-group" runat="server" id="transportation">
                <label>Transportation</label>
                <asp:Label ID="lbl_trans"  runat="server" Text=""></asp:Label>
      </div>

       <div class="form-group" runat="server" id="accommodation">
                <label>Accommodation</label>
                <asp:Label ID="lbl_accom"  runat="server" Text=""></asp:Label>
      </div>

      <div class="form-group" runat="server" id="other">
                <label>Other Expense</label>
                <asp:Label ID="lbl_other"  runat="server" Text=""></asp:Label>
      </div>
      <div class="form-group" runat="server" id="total">
                <label>Total Cash Approved</label>
                <asp:Label ID="lbl_total"  runat="server" Text=""></asp:Label>
      </div>

 
     <label>Dates</label>
        <asp:GridView ID="gvDates" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-form no-margin">
            <Columns>
                <asp:BoundField DataField="obIn" HeaderText="Start" />
                <asp:BoundField DataField="obOut" HeaderText="End" />
            </Columns>            
        </asp:GridView>

     <br />
        <label>Travel Destination</label>
        <asp:GridView ID="grid_destinations" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-hover dataTable">
            <Columns>
                <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                <asp:BoundField DataField="place" HeaderText="Place"/>
                <asp:BoundField DataField="travel_mode" HeaderText="Travel Mode"/>
                <asp:BoundField DataField="arrange_type" HeaderText="Arrangement Type"/>
            </Columns>
        </asp:GridView>
 
 </div>

</div>
<asp:HiddenField ID="key" runat="server" />
    <asp:HiddenField ID="id" runat="server" />
</asp:Content>

