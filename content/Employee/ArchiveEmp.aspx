<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ArchiveEmp.aspx.cs" Inherits="content_Employee_ArchiveEmp"
    MasterPageFile="~/content/site.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <!-- Bootstrap -->
    <script src="vendors/jquery/dist/jquery.min.js"></script> 
 <%--   <script src="vendors/bootstrap/dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap3-dialog/1.34.7/js/bootstrap-dialog.min.js"></script> 
--%>
    <script type="text/javascript">
        $(document).ready(function () {

        });
        function viewer(el) {
            $.ajax({
                type: "POST",
                url: "content/Employee/ArchiveEmp.aspx/viewers",
                data: "{id:" + el.getAttribute('value') + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var view = "Administrator, \n";
                    if (data.d.length > 0) {
                        for (var i = 0; i < data.d.length; i++) {
                            view += data.d[i] + ', \n';
                        }
                        alert("\nPeople who can view this document: \n\n" + view+"\n\n");
                        //                        BootstrapDialog.show({
                        //                            title: 'People who can view this document:',
                        //                            message:view
                        //                        });
                    }
                    else {
                        alert("\nNo other people can view this document.")
                        //                        BootstrapDialog.show({
                        //                            title: 'No other people can view this document.'
                        //                        });
                    }

                },
                error: function (err, result) {
                    alert(err);
                }
            });
        }

       
    </script>
    
     <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
     <script type="text/javascript">
         $("[class*=plus]").live("click", function() {
             $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
             $(this).attr("class", "fa fa-minus");
         });
         $("[class*=minus]").live("click", function() {
             $(this).attr("class", "fa fa-plus");
             $(this).closest("tr").next().remove();
         });
</script>
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server" ID="content2">
<section class="content-header">
    <div class="page-title">
        <div class="title_left hd-tl">
            <h3>
               Policies And Templates</h3>
        </div>
    </div>
    <div class="clearfix">
    </div>
</section>
<section class="content">
    <div class="row">
        <div class="col-xs-12">
            <div class="box box-primary">
            <div class="box-body table-responsive">
                <div id="div_msg" runat="server" class="alert alert-default">
                    <i class="fa fa-info-circle"></i>
                    <span>No record found</span>
                </div>
               <%-- <asp:GridView ID="grid_req" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                  <Columns>
                    <asp:BoundField DataField="filename" HeaderText="File Name" />
                    <asp:BoundField DataField="category" HeaderText="Category" />
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_download" runat="server" CommandName='<%# Eval("id") %>' Text="download" OnClick="download"><i class="fa fa-download"></i></asp:LinkButton>
                            <text ID="lnk_view" Value='<%# Eval("id") %>' OnClick="viewer(this)"><i class="fa fa-users"></i></texts>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                    </asp:TemplateField>
                  </Columns>
                </asp:GridView>--%>
                
                
                <asp:GridView ID="gvCustomers" AllowPaging="true" OnPageIndexChanging="pageindexing" PageSize="10" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered"
                    DataKeyNames="category" OnRowDataBound="OnRowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <i class="fa fa-plus" style="cursor: pointer;"></i>
                                <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                    <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false" ShowHeader="false"  CssClass="table table-striped table-bordered table-responsive">
                                        <Columns>
                                            <asp:BoundField ItemStyle-Width="150px" DataField="filename" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnk_download" runat="server" CommandName='<%# Eval("id") %>' Text="download" OnClick="download"><i class="fa fa-download"></i></asp:LinkButton>
                                                    <text id="lnk_view" value='<%# Eval("id") %>' onclick="viewer(this)"><i class="fa fa-users"></i></texts>
                                                </ItemTemplate>
                                                <ItemStyle Width="100px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField  DataField="category" HeaderText="Category" ItemStyle-CssClass="info-box-number" />
                        <asp:BoundField  DataField="counts" HeaderText="#" />
                        <asp:BoundField  DataField="status" HeaderText="Status" />
                    </Columns>
                </asp:GridView>

                
            </div>
            </div>
        </div>
    </div>
    <asp:Label ID="lbl_class" runat="server" ForeColor="Red"></asp:Label>
    <div class="hide">
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    </div>
    <asp:HiddenField ID="trn_det_id" runat="server" />
    <asp:HiddenField ID="key" runat="server" />
    <asp:HiddenField ID="pg" runat="server" />
    <asp:HiddenField ID="hfEmp" runat="server" />
    
</section>
</asp:Content>
