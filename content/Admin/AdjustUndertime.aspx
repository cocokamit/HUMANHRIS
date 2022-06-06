<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdjustUndertime.aspx.cs" Inherits="content_Admin_AdjustUndertime"  MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">

    <script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
    <script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.getElementById("<%= TextBox1.ClientID %>")
            if (confirm("Are you sure to cancel this transaction?"))
            { confirm_value.value = "Yes"; } else { confirm_value.value = "No"; }
        }


        function AddUT(eval,shiftname) {
            $.ajax({
                type: "POST",
                contentType: "application/json;",
                url: "content/Admin/AdjustUndertime.aspx/SetUT",
                data: JSON.stringify({ Id:eval }),
                dataType: "json",
                success: function (data) {
                    if (data.d.toString() != "") {
                        console.log("lad");

                        $("[id$=Label1]").val(shiftname);

                        var r = data.d.split("~");

                        var table = document.getElementById("Table1").getElementsByTagName('tbody')[0];
                        $('#Table1>tbody').empty();


                            for (var i = 0; i < r.length - 1; i++) {
                                var r3 = r[i].split(",");
                                var row = table.insertRow(-1);
                                row.insertCell(0).innerHTML = r3[0];
                                row.insertCell(1).innerHTML = r3[1];
                                row.insertCell(2).innerHTML = r3[2];
                                row.insertCell(3).innerHTML = r3[3];
                                row.insertCell(4).innerHTML = r3[4];
                                row.insertCell(5).innerHTML = r3[5];
                                row.insertCell(6).innerHTML = r3[6];
                                row.insertCell(7).innerHTML = '<input type="number" onkeypress="return isNumberKey(event);" class="form-control" value="'+r3[7]+'" /> ';
                            }

                            $("#Div1").show();
                    }
                },
                error: function (result) {
                    alert(result.responseText);

                    location.reload();
                }
            });
        }


        function closediv1() {
            console.log("asdasda");

            var tBody = $("#Table1 > TBODY")[0];
            var new_tbody = document.createElement('tbody');
            tBody.parentNode.replaceChild(new_tbody, tBody);

            $("#Div1").hide();
        }
        function isNumberKey(evt) {
            console.log(evt);
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function saveUT() {
            var tabel = "";
            $("#Table1>tbody").find('tr').each(function () {
                $(this).find('td').each(function () {
                    if ($(this).find('input').length != 0) {
                        tabel += $(this).find('input').eq(0).val() + "~" + $(this).parent().find('td').eq(0).html()+"`";
                    }
                });
            });
            console.log(tabel);


            $.ajax({
                type: "POST",
                contentType: "application/json;",
                url: "content/Admin/AdjustUndertime.aspx/saveUThr",
                data: JSON.stringify({ tables: tabel }),
                dataType: "json",
                success: function (data) {
                    alert("Data successfully saved!");

                },
                error: function (result) {
                    alert(result.responseText);
                }
            });

        }
    </script>
</asp:Content>

<asp:Content ID="emp_add" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Adjust Undertime</h3>
    </div>  
    <div class="title_right">
        <ul>
            <li><a href="#"><i class="fa fa-gear"></i>  DTR </a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>Adjust Undertime</li>
        </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    <div class="col-md-12">
        <div class="x_panel">
             <asp:GridView ID="grid_view" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="shiftcode" HeaderText="Shift Code"/>
                    <asp:BoundField DataField="remarks" HeaderText="Remarks"/>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href="javascript:void(0);" onclick="AddUT(<%# Eval("id")%>,'<%# Eval("shiftcode") %>');"><i class="fa fa-pencil"></i></a>
                       </ItemTemplate>
                        <ItemStyle Width="72px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>

<div id="Div1" class="modal fade in">
    <div class="modal-dialog modal-lg">  
        <div class="modal-content">
            <div class="modal-header">
                        <input type="button" id="Button6" onclick="closediv1()" class="close" aria-hidden="true" value="&times;" />
                <h4 class="modal-title"><asp:label ID="Label1" ClientIDMode="Static" runat="server"/></h4>
            </div>
            <div class="modal-body">
                   <table id="Table1" class="table" style="width: 100%" >
                        <thead>
                            <tr>
                                <th >
                                    Id
                                </th>
                                <th>
                                    RestDay
                                </th>
                                <th>
                                    TimeIn1
                                </th>
                                <th>
                                    TimeOut1
                                </th>
                                <th>
                                    TimeIn2
                                </th>
                                <th>
                                    TimeOut2
                                </th>
                                <th>
                                    NumberOfHours
                                </th>
                                <th style="width:10%">
                                    UT Min.
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                        <tfoot>      
                        </tfoot>
                    </table>
                </div>
                <div class="modal-footer">
                    <input type="button" class="btn btn-success" value="SAVE" onclick="saveUT()" /> 
                </div>
        </div>
    </div>
</div>


<asp:TextBox ID="TextBox1" runat="server" class="hide"></asp:TextBox> 
<asp:HiddenField ID="key" runat="server" />
</asp:Content>

