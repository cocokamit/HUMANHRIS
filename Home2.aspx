<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home2.aspx.cs" Inherits="Home2"
    MasterPageFile="~/content/MasterPageNew.master" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        </script>
    <style type="text/css">
        .rowr
        {
            display: -ms-flexbox;
            display: flex;
            -ms-flex-wrap: wrap;
            flex-wrap: wrap;
            margin: 0 -15px 0 -5px;
        }
        
        .columnr
        {
            -ms-flex: 33%;
            flex: 33%;
            max-width: 33%;
            padding: 0 6px;
        }
        
        .columnr img
        {
            margin-top: 15px;
            vertical-align: middle;
            width: 100%;
            transition: transform .2s;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.7), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
        }
        
        .columnr img:hover
        {
              -ms-transform: scale(1.05); /* IE 9 */
  -webkit-transform: scale(1.05); /* Safari 3-8 */
  transform: scale(1.05); 
        }
        
        @media screen and (max-width: 800px)
        {
            .columnr
            {
                -ms-flex: 50%;
                flex: 50%;
                max-width: 50%;
            }
        }
        
        @media screen and (max-width: 600px)
        {
            .columnr
            {
                -ms-flex: 100%;
                flex: 100%;
                max-width: 100%;
            }
        }
        
    body
    {
        }

    </style>
</asp:Content>
<asp:Content ID="content1" runat="server" ContentPlaceHolderID="content">
   
    <section class="content">  
    <div class="page-title">
        <div class="title_left hd-tl">
            <h3>Home</h3>
               
        </div>
          <div class="title_right">
       <ul>
        <li><a href="homepage"><i class="fa fa fa-dashboard"></i> Home</a></li>
       </ul>
    </div> 
    </div>
     <div class="row">
                <div class="col-md-12" style="overflow:hidden; max-height:300px;">
        <asp:LinkButton ID="banner" runat="server" Text="" CssClass="btn pull-right" style="color:Red;" OnClick="onmodal"><i class="fa fa-edit"></i></asp:LinkButton>
    
                   <asp:Image ID="homebanner" runat="server" CssClass="image img-responsive" ClientIDMode="Static" ImageUrl="~/style/images/emptyimage.png"  />
                </div>
              </div>
        
              <br />
              
    <div> 
         <asp:LinkButton ID="posts" runat="server" Text="" CssClass="btn" style="color:Red;" AutoPostBack="True" OnClick="onmodal"><i class="fa fa-edit"></i></asp:LinkButton>
             
            <div class="rowr" runat="server" id="divannouncement"> 

            </div>
            </div>
            <hr />
            
            <hr />
            <br />
     </section>
    <%------------------------------------------------------modal saving-------------------------------------------------------%>
    <div id="modal" runat="server" class="modal fade in">
        <div class="modal-dialog" style="width: 600px;">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="click_close" class="close"
                        aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 class="modal-title">
                    </h4>
                </div>
                <div class="modal-body">
                    <asp:Panel ID="Panel1" runat="server">
                        <div class="x_panel">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:FileUpload ID="FileUpload1" accept=".png,.jpeg,.jpg" runat="server" />
                                </div>
                            </div>
                           
                            <br />
                          
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <label runat="server" id="lb3">
                                        URL:</label>
                                    <asp:TextBox ID="txt_url" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btn_save" CssClass="btn btn-success pull-right" runat="server" OnClick="announce"
                                        ClientIDMode="Static" Text="Save" AutoPostBack="true" />
                                </div>
                            </div>
                            <br />
                            <div class="row" id="divgrid" runat="server">
                                <div class="col-md-12">
                                    <div id="alert" runat="server" class="alert alert-default">
                                        <i class="fa fa-info-circle"></i><span>No record found</span>
                                    </div>
                                    <div style="height:200px; overflow-y:auto;">
                                        <asp:GridView ID="grid_forms" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered"
                                            ClientIDMode="AutoID">
                                            <Columns>
                                                <asp:BoundField DataField="id" HeaderText="ID" HeaderStyle-CssClass="none" ItemStyle-CssClass="none" />
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="viewbtn" runat="server" OnClick="deleteer" CssClass="fa fa-trash"
                                                            AutoPostBack="True"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="dates" HeaderText="Date" />
                                                <asp:BoundField DataField="url" ItemStyle-Width="150px" HeaderText="Description" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
</asp:Content>
