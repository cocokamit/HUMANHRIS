    <%@ Page Language="C#" AutoEventWireup="true" CodeFile="archive.aspx.cs" Inherits="content_hr_archive"
    MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
    <link href="script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
    <script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>
    <style type="text/css">
        *{font-size: 12px;}.hide{ display: none !important;}
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $.noConflict();
            $("#ddl_emp").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "content/hr/archive.aspx/GetEmployee",
                        data: "{'term':'" + $("#ddl_emp").val() + "'}",
                        dataType: "json",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split('-')[1],
                                    val: item.split('-')[0]
                                }
                            }))
                        },
                        error: function (result) {
                            alert(result.responseText);
                        }
                    });
                },
                select: function (e, i) {
                    $("[id$=hfEmp]").val(i.item.val);

                    var temp2 = $("[id$=Label1]").val();
                    $("[id$=Label1]").val(temp2 + "" + i.item.val + ";");

                    var txt = document.createElement("Button");
                    txt.innerText = i.item.label;
                    txt.id = i.item.val;
                    txt.className = "btn btn-info";
                    txt.style = "margin: 0 0 3px 3px;";
                    txt.onclick = function (e) {
                        this.parentNode.removeChild(this);
                        var res = $("[id$=Label1]").val();
                        $("[id$=Label1]").val(res.replace(this.id + ";", ""));
                    };
                    $(txt).appendTo("#namelist");

                    $("[id$=ddl_emp]").val("");
                    console.log($("[id$=Label1]").val());
                    return false;
                }
            });

            $("[id$=chkall]").on("change keydown paste input", function () {
                if ($(this).is(':checked')) {
                    $("[id$=divemp]").hide();
                    $("[id$=Label1]").val('All');
                    var myNode = document.getElementById("namelist");
                    myNode.innerHTML = '';
                }
                else {
                    $("[id$=divemp]").show();
                    $("[id$=Label1]").val('');
                }

                console.log($("[id$=Label1]").val());
            });
        });
        function viewer(el) {
            $.ajax({
                type: "POST",
                url: "content/hr/archive.aspx/viewers",
                data: "{id:" + el.getAttribute('value') + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var view = "Administrator, \n";
                    if (data.d.length > 0) {
                        for (var i = 0; i < data.d.length; i++) {
                            view += data.d[i] + ', \n';
                        }
                        alert("\nPeople who can view this document: \n\n" + view + "\n\n");
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
</asp:Content>
<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
    <div class="page-title">
        <div class="title_left hd-tl">
            <h3>
                Archive</h3>
        </div>
    </div>
    <div class="clearfix">
    </div>
    <%//--------------------------------------------Compose button-----------------------------------------------// %>
    <div class="row">
        <div class="col-md-3">
            <a href="" class="btn btn-primary btn-rounded mb-4" data-toggle="modal" data-target="#modalForm">
                ADD +</a>
        </div>
    </div>
    <div class="row">
        <br />
    </div>
    <%//--------------------------------------------Modal for composing-----------------------------------------------// %>
    <div class="col-md-3">
        <div class="modal fade" id="modalForm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content x_panel">
                    <div class="modal-header text-center">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <ul class="input-form">
                        <li>Choose Folder
                            <asp:DropDownList ID="ddl_class2" runat="server" AutoPostBack="false" CssClass="minimal left">
                            </asp:DropDownList>
                            &nbsp; </li>
                        <li>
                      
                        <li>
                        
                            Choose people/department who can view this file.
                            <div id="divemp" visible="true" runat="server">
                                <asp:TextBox ID="ddl_emp" runat="server" ClientIDMode="Static"></asp:TextBox>
                            </div>
                            <div>
                              <asp:CheckBox runat="server" CssClass="checkbox-inline" ID="chkall" Text="All" Value="All" />
                            </div>
                        </li>
                        <li>
                                <div id="namelist" >
                                </div>
                            <%--<asp:TextBox ID="txt_multi" Visible="false" TextMode="MultiLine" runat="server"></asp:TextBox>
                            &nbsp; </li>--%>
                        <li>Source File<asp:Label ID="lbl_sf" runat="server" ForeColor="Red"></asp:Label></li>
                        <li>
                            <asp:FileUpload ID="FileUpload1" multiple="multiple" runat="server" /></li>
                        <li>Description<asp:Label ID="lbl_desc" runat="server" ForeColor="Red"></asp:Label></li>
                        <li>
                            <asp:TextBox ID="txt_desc" TextMode="MultiLine" runat="server"></asp:TextBox></li>
                        <li>
                            <hr />
                        </li>
                        <li>
                            <asp:Button ID="Button1" runat="server" OnClick="clickupload" Text="Save" CssClass="btn btn-primary" /></li>
                            <%--<asp:TextBox ID="Label1" runat="server" ForeColor="Red"></asp:TextBox>--%>
                            
                            <asp:HiddenField ID="Label1" runat="server"  />
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <%//---------------------------------------Create New Folder-------------------------------------------// %>
    <div class="col-md-3">
        <div class="modal fade" id="modalCreateNewFolder" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content x_panel">
                    <div class="modal-header text-center">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <ul class="input-form">
                        <li>New Folder Name</li>
                        <li>
                            <asp:TextBox ID="txt_Nfolder" runat="server" ClientIDMode="Static" Style="text-transform: uppercase"></asp:TextBox></li>
                        <li>
                            <asp:Button ID="btn_Nfolder" runat="server" OnClick="clickaddfolder" Text="ADD" CssClass="btn btn-primary" /></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>



    <div class="row">
        <%//---------------------------------------------Archives---------------------------------------------// %>
        <div class="col-md-9">
            <div class="x_panel">
                <div class="title_left hd-tl">
                    <h2>Contents : <asp:Label ID="lb_folname" runat="server"></asp:Label></h2>
                </div>
                <div class="clearfix">
                </div>
                <br />
                <asp:GridView ID="grid_req" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                    <Columns>
                        <asp:BoundField DataField="filename" HeaderText="File Name" />
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk_download" runat="server" CommandName='<%# Eval("id") %>'
                                    Text="download" OnClick="download"><i class="fa fa-download"></i></asp:LinkButton>
                                <asp:LinkButton ID="lnk_can" runat="server" OnClientClick="return confirm('Are you sure you want to remove this file?')" OnClick="candetails"
                                    CommandName='<%# Eval("id") %>' Text="cancel"><i class="fa fa-trash"></i></asp:LinkButton>
                                       <text ID="lnk_view" runat="server"
                                    Value='<%# Eval("id") %>' onclick="viewer(this)"><i class="fa fa-users"></i></text>
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div id="div_msg" runat="server" class="alert alert-empty">
                    <i class="fa fa-info-circle"></i><span>No record found</span>
                </div>
            </div>
        </div>
        <%//---------------------------------------------Folders----------------------------------------------// %>
        <div class="col-md-3">
            <div class="x_panel">
                <div class="title_left hd-tl">
                    <h4>
                        Folders </h4>
                </div>
                </br>
                <div id="Folders" runat="server" class="list-group" style="vertical-align: middle;">
                </div>
            </div>
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

</asp:Content>
