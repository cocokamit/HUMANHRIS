<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="employee_position.aspx.cs"
    Inherits="content_Admin_employee_position" MasterPageFile="~/content/MasterPageNew.master"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="Server">
    <link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />
    <link href="style/css/tablechkbx.css" rel="stylesheet" type="text/css" />
    <script src="http://code.jquery.com/jquery-1.11.0.min.js"></script>
    
    <link href="script/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
    <script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>
    <script src="script/auto/myJScript.js" type="text/javascript"></script>
    
    
    <link href="style/css/tablechkbx.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        //----------------------------color-change when checkbox is clicked------------------------------------------
        //-----------------------but unnecessary-------
        //            function checkBoxList1OnCheck(lis tControlRef) 
        //            {
        //                var inputItemArray = listControlRef.getElementsByTagName('input');
        //                for (var i = 0; i < inputItemArray.length; i++) 
        //                {
        //                    var inputItem = inputItemArray[i];
        //                    if (inputItem.checked) {
        //                        if(confirm())
        //                        {
        //                            
        //                        }
        //                    }
        //                    else {

        //                    }
        //                }
        //             }
        function OpenConfirmDialog() {
            var name = $("[id$=hfaccname]").val();
            var num = "";
            var array1 = "";
            var array2 = "";
            $('[id$=chkMemberList] input:checked').each(function () {
                num += $(this).val() + ",";
            });
            $('[id$=chkappr] input:checked').each(function () {
                num += $(this).val() + ",";
            });
            $('[id$=chksched] input:checked').each(function () {
                num += $(this).val() + ",";
            });
            array1 = num.split(",");
            array2 = $("[id$=hftemplist]").val().split(",");

            console.log(name);
            console.log(array1);
            console.log(array2);

            if ($("[id$=hfrole]").val() == "Approver and Scheduler") {
                console.log($("[id$=hfrole]").val());
                if (array1.length > array2.length) {
                    if (confirm('Are you sure you want to save your updates?')) {
                        if (confirm('It seems that you have added a new member, please assign a correct role of (' + name + ') for the new member(s) \n\n Press "YES" if as "Approver", else "Cancel" if as "Scheduler"') == true) {
                            $("[id$=hfchk]").val("YES");
                            console.log($("[id$=hfchk]").val());
                        }
                        else {
                            $("[id$=hfchk]").val("NO");
                            console.log($("[id$=hfchk]").val());
                        } 
                    }
                }
            }
            document.getElementById("btnsavem").click();
        }


        function changerole() {
            var value = $("[id$=ddl_employee]").val();
            console.log("dsd");
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "content/Admin/employee_position.aspx/getrolevalue",
                data: JSON.stringify({ i: value }),
                dataType: "json",
                success: function(data) {
                    $("[id$=ddlRole2]").val(data.d);
                    console.log(data.d);
                },
                error: function(result) {
                    alert(result.responseText);
                }
            });
        }

    </script>
    <style type="text/css">
        .fc-basic-view
        {
            border: 1px solid #ddd;
            border-top: none;
        }
        div .box
        {
            border: 0px;
            background: #FFFFFF;
            height: 100%;
        }
        
        textarea
        {
            resize: none;
        }
    </style>
    
    <script type="text/javascript">
          $(document).ready(function () {
              $.noConflict();
              $(".auto").autocomplete({
                  source: function (request, response) {
                      $.ajax({
                          type: "POST",
                          contentType: "application/json; charset=utf-8",
                          url: "content/report/leave_credit_report.aspx/GetEmployee",
                          data: "{'term':'" + $(".auto").val() + "'}",
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
                      index = $(".auto").parent().parent().index();
                      $("#lbl_bals").val(i.item.val);
                  }
              });
          });
</script>


</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="content" runat="Server">
    <asp:HiddenField ID="lbl_bals" ClientIDMode="Static" Value="0" runat="server" />
    <asp:HiddenField ID="hfchk" Value="" runat="server" />
    <asp:HiddenField ID="hfaccname" Value="" runat="server" />
    <asp:HiddenField ID="hftemplist" Value="" runat="server" />
    <asp:HiddenField ID="hfrole" Value="" runat="server" />
    <div class="page-title">
        <div class="title_left hd-tl">
            <h3>
                Access Rights</h3>
        </div>
    </div>
    <div class="clearfix">
    </div>
    <div id="Countss" runat="server" class="row">
    </div>
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="x_panel">
            <div class="x-head">
                <asp:LinkButton runat="server" class="pull-right" Style="padding: 8px 8px 0" OnClick="refresh"><i class="fa fa-refresh">&nbsp;&nbsp;Refresh</i>
                </asp:LinkButton>
                <div class="input-group" style="width: 300px">
                    <asp:TextBox ID="tb_search" placeholder="Employee Name" Width="250px" Height="33px" CssClass="auto" runat="server"></asp:TextBox> 
                    <span class="input-group-btn">
                        <asp:Button ID="b_search" runat="server" CssClass="btn btn-primary" placeholder="Search"
                            Text="GO" OnClick="click_search" />
                    </span>
                </div>
            </div>
            <div id="alert" runat="server" class="alert alert-default alert-dismissible">
                <h5>
                    <i class="icon fa fa-info-circle"></i>No record found</h5>
            </div>
            <asp:GridView ID="grid_view" AllowPaging="True" PageSize="10" OnPageIndexChanging="grid_view_PageIndexChanging"
                CssClass="table table-bordered table-hover dataTable" runat="server" AutoGenerateColumns="false">
                <PagerStyle CssClass="pagination-ys" />
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="id" HeaderStyle-CssClass="none" ItemStyle-CssClass="none" />
                    <asp:BoundField DataField="acc_id" HeaderText="acc_id" HeaderStyle-CssClass="none"
                        ItemStyle-CssClass="none" />
                    <asp:BoundField DataField="emp_id" HeaderText="ID" />
                    <asp:BoundField DataField="name" HeaderText="Employee" />
                    <asp:BoundField DataField="acc_name" HeaderText="Role" />
                    <asp:TemplateField HeaderText="Members Under">
                        <ItemTemplate>
                            <asp:Label ID="LinkButton3" runat="server" Text='<%# Eval("underM") %>'></asp:Label>
                            <asp:LinkButton ID="lnk_members" runat="server" CssClass="Float right" Text="<i class='fa fa-eye'></i>"
                                OnClick="click_repledit"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="username" HeaderText="Username" HeaderStyle-CssClass="none"
                        ItemStyle-CssClass="none" />
                    <asp:BoundField DataField="password" HeaderText="Password" HeaderStyle-CssClass="none"
                        ItemStyle-CssClass="none" />
                    <asp:TemplateField HeaderText="Action (Employee's role)">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk_edit" runat="server" OnClick="click_repledit" CommandName='<%# Eval("name") %>'
                                Text="Edit"></asp:LinkButton>
                            <asp:LinkButton ID="link_replace" runat="server" OnClick="click_repledit" CommandName='<%# Eval("name") %>'
                                Text="Replace"></asp:LinkButton>
                            <asp:LinkButton ID="link_admin" runat="server" OnClick="click_repledit" CommandName='<%# Eval("name") %>'
                                Text="Access"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <%//-----------------------------------------------------Modal for Viewing members------------------------------------------------- %>
    <div id="modalM" runat="server" class="modal fade in">
        <div class="modal-dialog" style="width: 950px;">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:LinkButton ID="LinkButton4" runat="server" OnClick="click_close" class="close"
                        aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 class="modal-title">
                        All members</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-4">
                            <div class="row">
                                <div class="col-md-12">
                                    <label>
                                        Scheduler (</label><asp:Label runat="server" ID="lblsched" Text="0"></asp:Label><span>)</span>
                                    <br />
                                    <asp:CheckBoxList ID="chksched" runat="server" Style="max-height:500px; overflow:auto;" CssClass="checkbox checkbox-inline "
                                        RepeatLayout="table" RepeatDirection="vertical">
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <label>
                                        Approver (</label><asp:Label runat="server" ID="lblapp" Text="0"></asp:Label><span>)</span>
                                    <br />
                                    <asp:CheckBoxList ID="chkappr" runat="server" CssClass="checkbox checkbox-inline"
                                        RepeatLayout="table" RepeatDirection="vertical" Style="max-height:500px; overflow:auto;">
                                    </asp:CheckBoxList>
                                </div>
                            </div>

                            <div id="approvers" runat="server"> </div>
                        </div>
                        <div class="col-md-8">
                            <asp:Panel ID="Panel2" runat="server">
                                <div class="form-group">
                                    <div class="x-head">
                                        <asp:LinkButton runat="server" class="pull-right" Style="padding: 8px 8px 0" OnClick="refresh2"><i class="fa fa-eye"> Show All</i>
                                        </asp:LinkButton>
                                        <div class="input-group" style="width: 300px">
                                            <asp:TextBox ID="tb_searchM" runat="server" class="form-control" AutoPostBack="true"
                                                placeholder="Search" OnTextChanged="click_searchM"></asp:TextBox>
                                            <span class="input-group-btn">
                                                <asp:Button ID="b_searchM" runat="server" CssClass="btn btn-primary" placeholder="Search"
                                                    Text="GO" OnClick="click_searchM" />
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group no-margin">
                                    <label>
                                        Choose employees you want to be part of the group.</label>
                                    <span class="input-group-btn">
                                        <asp:CheckBox ID="checkallnot" runat="server" CssClass="btn btn-primary" OnCheckedChanged="click_checkallnot"
                                            AutoPostBack="true" Text="&nbsp;&nbsp;All" Style="margin-bottom: 10px;" />
                                    </span>
                                    <asp:Panel ID="Panel3" runat="server">
                                        <div style="overflow-y: scroll; width: auto; max-height: 400px;">
                                            <asp:CheckBoxList ID="chkMemberList" runat="server" CssClass="checkbox checkbox-inline"
                                                RepeatLayout="table" RepeatColumns="2" RepeatDirection="vertical">
                                            </asp:CheckBoxList>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <input type="button" id="LinkButton5" runat="server" value="Update" onclick="OpenConfirmDialog()"
                        class="btn btn-primary" />
                    <asp:LinkButton ID="btnsavem" runat="server" Text="Save" OnClick="click_saveM" ClientIDMode="Static"
                        CssClass="btn btn-primary none" />
                </div>
            </div>
        </div>
    </div>
    <%//-----------------------------------------------------Modal for editing------------------------------------------------- %>
    <div id="modal" runat="server" class="modal fade in">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="click_close" class="close"
                        aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 class="modal-title">
                        Employee Rights</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel ID="p_details" runat="server">
                        <div class="form-group">
                            <label>
                                Employee</label>
                            <asp:Label ID="l_user" ForeColor="Red" runat="server" Text=""></asp:Label>
                            <asp:Label ID="l_employee" runat="server" CssClass="form-control" autocomplete="off"></asp:Label>
                            <asp:DropDownList ID="ddl_user" CssClass="select2" runat="server" Visible="false">
                                <asp:ListItem Value="" disabled Selected hidden></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label>
                                Role
                            </label>
                            <asp:Label ID="lRoute" ForeColor="Red" runat="server"></asp:Label>
                            <asp:DropDownList ID="ddlRole" CssClass="select2" runat="server">
                            </asp:DropDownList>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="form-group">
                            <label>
                                Username</label>
                            <asp:Label ID="l_username" ForeColor="Red" runat="server" Text=""></asp:Label>
                            <asp:TextBox ID="tb_username" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="form-group no-margin">
                            <label>
                                Password</label>
                            <asp:Label ID="l_password" ForeColor="Red" runat="server" Text=""></asp:Label>
                            <asp:TextBox ID="tb_password" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                        </div>
                    </asp:Panel>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="Button3" runat="server" Text="Save" OnClick="click_save" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>
    <%//-----------------------------------------------------Modal for position replacement------------------------------------------------- %>
    <div id="replaceModal" runat="server" class="modal fade in">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:LinkButton ID="LinkButton2" runat="server" OnClick="click_close" class="close"
                        aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 class="modal-title">
                        Employee Position replacement</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel ID="Panel1" runat="server">
                        <div class="form-group">
                            <label>
                                The role...
                            </label>
                            <asp:Label ID="Label1" ForeColor="Red" runat="server"></asp:Label>
                            <asp:Label ID="l_role" runat="server" CssClass="form-control" autocomplete="off"></asp:Label>
                        </div>
                        <div class="form-group">
                            <label>
                                ...will be replaced by <span style="color: red">*</span>
                            </label>
                            <asp:Label ID="Label2" ForeColor="Red" runat="server"></asp:Label>
                            <<asp:DropDownList ID="ddl_employee" ClientIDMode="Static" CssClass="select2" runat="server" onchange="changerole()">
                            </asp:DropDownList>
                            <br />
                        </div>
                        <div class="form-group">
                            <label>
                                Employee</label>
                            <asp:Label ID="l_user2" ForeColor="Red" runat="server" Text=""></asp:Label>
                            <asp:Label ID="l_employee2" runat="server" CssClass="form-control" autocomplete="off"></asp:Label>
                        </div>
                        <div class="form-group">
                            <label>
                                New Role <span style="color: red">*</span>
                            </label>
                            <asp:Label ID="lRoute2" ForeColor="Red" runat="server"></asp:Label>
                            <asp:DropDownList ID="ddlRole2" CssClass="select2" runat="server">
                            </asp:DropDownList>
                            <div class="clearfix">
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="Button1" runat="server" Text="Save" OnClick="click_saveRep" CssClass="btn btn-primary">
                    </asp:Button>
                </div>
            </div>
        </div>
    </div>
    <%------------------------------------------------------------Modal for Admin Access---------------------------------------------------------%>
    <div id="accessModal" runat="server" class="modal fade in">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:LinkButton ID="LinkButton6" runat="server" OnClick="click_close" class="close"
                        aria-hidden="true"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 class="modal-title">
                        Admin Access</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel ID="Panel4" runat="server">
                        <div class="form-group no-margin">
                            <label>
                                Routes</label>
                            <asp:Label ID="Label3" ForeColor="Red" runat="server" Text=""></asp:Label>
                            <asp:GridView ID="gv_route" CssClass="table table-bordered table-hover dataTable no-margin"
                                OnRowDataBound="gv_routeBound" ShowHeader="false" EmptyDataText="No route available"
                                runat="server" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                                    <asp:BoundField DataField="cnt" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cb" runat="server" Visible='<%# Eval("url").ToString()!="-" && Eval("name").ToString()!="Master File"%>'
                                                Enabled='<%# Eval("url").ToString()!="-" && Eval("name").ToString()!="Master File"%>'
                                                Checked='<%# Eval("url").ToString()!="-" && Eval("name").ToString()!="Master File"%>' />
                                            <asp:Label ID="l_lable" runat="server" Text='<%# Eval("name") %>' CssClass="font-weight-bold"></asp:Label>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("description") %>' CssClass="text-muted pull-right"></asp:Label>
                                            <asp:GridView ID="gvOrders" runat="server" CssClass="table table-bordered table-hover dataTable no-margin"
                                                AutoGenerateColumns="false" OnRowDataBound="gv_orderBound">
                                                <Columns>
                                                    <asp:BoundField DataField="id" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                                                    <asp:BoundField DataField="cnt" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <label style="width: 100%;">
                                                                <asp:CheckBox ID="CheckBox1" runat="server" />
                                                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                                                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("description") %>' CssClass="text-muted pull-right"></asp:Label>
                                                            </label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Read Only">
                                                        <ItemTemplate>
                                                            <label style="width: 100%;">
                                                                <asp:RadioButton ID="read" runat="server" GroupName="checkers" />
                                                            </label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Read & Update">
                                                        <ItemTemplate>
                                                            <label style="width: 100%;">
                                                                <asp:RadioButton ID="update" runat="server" GroupName="checkers" />
                                                            </label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="read" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                                                    <asp:BoundField DataField="update" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                                                    <asp:BoundField DataField="default" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                                                </Columns>
                                            </asp:GridView>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="name" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="Button2" runat="server" Text="Save" OnClick="click_saveAccess" CssClass="btn btn-primary">
                    </asp:Button>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hf_id" runat="server" />
    <asp:HiddenField ID="hf_employee" runat="server" />
    <asp:HiddenField ID="hf_action" runat="server" />
    <asp:HiddenField ID="hf_account" runat="server" />
    <asp:HiddenField ID="hf_cbvalues" runat="server" />
</asp:Content>
