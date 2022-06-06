<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetUp.aspx.cs" Inherits="content_Admin_SetUp" 
 MasterPageFile="~/content/site.master" %>


<asp:Content ID="head" ContentPlaceHolderID="head" runat="Server"> 
   
    <link href="vendors/select2/dist/css/select2.css" rel="stylesheet" type="text/css" />
    <link href="style/css/tablechkbx.css" rel="stylesheet" type="text/css" />
  
<script type="text/javascript">
    function autosave(evs) {
        var row = evs.parentNode.parentNode.parentNode;
        var leaveider = $("#txtdate", row).text();
        var listLength = evs.options.length;
        var allstring = "";
        for (var i = 0; i < listLength; i++) {
            if (evs.options[i].selected) {
                allstring += evs.options[i].value + ",";
            }
        }
        console.log(row);
        console.log(allstring);
        console.log(leaveider);
        $.ajax({
            type: "POST",
            url: "content/Admin/Setup.aspx/saveauto",
            contentType: "application/json;",
            data: JSON.stringify({ statusreqid: allstring, leaveid: leaveider }),
            dataType: "json",
            success: function(r) {

                console.log("success");

            },
            error: function(r) {
                console.log("error: " + r);
            }
        });

    }

</script>


       <style>
        textarea
        {
            resize: none;
        }
   
        .row
        {
            margin-bottom:20px;
            
            }
    </style>
<%--<link rel="stylesheet" href="style/multiple/docs/css/prettify.min.css" type="text/css">
    <script type="text/javascript" src="style/multiple/docs/js/bootstrap-3.3.2.min.js"></script>
    <script type="text/javascript" src="style/multiple/jquery.min.js"></script>
    <script type="text/javascript" src="style/multiple/bootstrap-multiselect.js"></script>
    <script type="text/javascript" src="style/multiple/bootstrap.min.js"></script>
    <link rel="Stylesheet" type="text/css" href="style/multiple/bootstrap-multiselect.css" />
   
       <script type="text/javascript">
           $(function () {
               $("[id$=liststatus]").multiselect({

                   includeSelectAllOption: true
               });
           });
        </script>--%>
</asp:Content>
<asp:Content ID="content1" runat="server" ContentPlaceHolderID="content">
    <section class="content-header">
    <div class="page-title">
        <div class="title_left hd-tl">
            <h3>SetUp Settings</h3>
        </div>
    </div>
    </section>
    <section class="content">
       <div class="x_panel" style="margin-bottom:3000px;">
      
      <div class="row">
        <div class="col-md-6">
            <label>Leave Type</label>
            <asp:DropDownList runat="server" ID="ddlLeave" CssClass="form-control" ClientIDMode="Static" >
                <asp:ListItem Value="1" Text="Yearly"></asp:ListItem>
                <asp:ListItem Value="2" Text="Monthly"></asp:ListItem>
            </asp:DropDownList>
        </div>
       </div>

       <div class="row" id="ytype" runat="server" clientidmode="Static">
        <div class="col-md-6">
            <label>Yearly Type</label>
            <asp:DropDownList runat="server" ID="ddlYearly" CssClass="form-control" ClientIDMode="Static" >
                <asp:ListItem Value="3" Text="Manual"></asp:ListItem>
                <asp:ListItem Value="4" Text="Evolving"></asp:ListItem>
                <asp:ListItem Value="5" Text="Tenurity"></asp:ListItem>
            </asp:DropDownList>
        </div>
       </div>    
       
       <div class="row" id="lct" runat="server" clientidmode="Static">
        <div class="col-md-6">
            <label>Leave Consumption Type</label>
            <asp:DropDownList runat="server" ID="ddlconsump" CssClass="form-control" ClientIDMode="Static" >
                <asp:ListItem Value="1" Text="Anytime"></asp:ListItem>
                <asp:ListItem Value="2" Text="Timespan"></asp:ListItem>
            </asp:DropDownList>
        </div>
       </div>   
          
       <div class="row" id="lcrd" runat="server" clientidmode="Static">
        <div class="col-md-6">
            <label>Leave credits reset date :</label>
            <asp:TextBox runat="server" ID="txt_leaveRes" CssClass="form-control datee" ClientIDMode="Static"></asp:TextBox> 
        </div>
       </div>

        <div class="row" id="leex" runat="server" clientidmode="Static">
        <div class="col-md-6">
            <div class="row"> 
                <div class="col-md-6">
                     <label>Leave credits extension :</label>
                     <asp:TextBox runat="server" ID="txt_leaveext" CssClass="form-control datee" ClientIDMode="Static"></asp:TextBox>      
                </div>
                <div class="col-md-6" id="dce" runat="server" clientidmode="Static">
                    <label>Extension default credits :</label>
                     <asp:TextBox runat="server" ID="txt_dce" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>     
                </div>
            </div>
           
        </div>
       </div>

       <div class="row" id="Div1" runat="server" clientidmode="Static">

       <div class="col-md-6">
            <div class="row"> 
                <div class="col-md-12">
                     <label>Extended Leave Type : </label> 
                      <asp:ListBox ID="listlt" SelectionMode="Multiple" AutoComplete="off" runat="server" ></asp:ListBox>  
                </div>
            </div>
           
        </div>
        <div class="col-md-6">
            <div class="row"> 
                <div class="col-md-12">
                     <label>Employee Status Leave Credits Acquisition Requirement :</label>
                      <asp:ListBox ID="liststatus" AutoComplete="off" SelectionMode="Multiple" runat="server" ></asp:ListBox>  
                </div>
            </div>
           
        </div>
        
       </div>

            <div class="row" id="Div2" runat="server" clientidmode="Static">

       <div class="col-md-6">
            <div class="row"> 
                <div class="col-md-12">
                 
                        <%------------------------------------------------------------------------------------------------------------%>


            <%--<div class="row"> 
                <div class="col-md-12">
                     <label>Employee Status Leave Credits Acquisition Requirement </label>
                      <asp:ListBox ID="liststatus" AutoComplete="off" SelectionMode="Multiple" runat="server" ></asp:ListBox>  
                </div>
            </div>--%>
            <div id="alert" runat="server" class="alert alert-default">
                    <i class="fa fa-info-circle"></i>
                    <span>No record found</span>
                </div>
                 <asp:GridView ID="grid_forms" runat="server" OnRowDataBound="ordb" AutoGenerateColumns="false" CssClass="table table-striped table-bordered" ClientIDMode="Static">
                    <Columns> 
                    <asp:TemplateField HeaderText="Date" >
                                <ItemTemplate> 
                                     <asp:Label runat="server" ID="txtdate" Text='<%# Bind("Id") %>'></asp:Label>
                                </ItemTemplate>
                    </asp:TemplateField>  
                                     
                        <asp:BoundField DataField="Leave" HeaderText="Leave"/>
                        <asp:BoundField DataField="LeaveType" HeaderText="Leave Type"/>
                        <asp:BoundField DataField="converttocash" HeaderText="Convert to cash"/>
                        <asp:TemplateField  HeaderText="Action">
                            <ItemTemplate>
                                 <asp:ListBox ID="txt_cond2" SelectionMode="Multiple" runat="server" onchange="autosave(this)"></asp:ListBox>  
                             
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:BoundField DataField="StatusReq" HeaderStyle-CssClass="none" ItemStyle-CssClass="none"/>
                        
                    </Columns>
                </asp:GridView>
                
                </div>
            </div>
           
        </div>
        <div class="col-md-6">
            <div class="row"> 
                <div class="col-md-12">
                     <label>Division :</label>
                      <asp:ListBox ID="listdivision" AutoComplete="off" SelectionMode="Multiple" runat="server" ></asp:ListBox>  
                </div>
            </div>
        </div>
       </div>


       <hr />
       <%------------------------------------------------------------------------------------------------------------%>
       <div class="row" id="vlv" runat="server" clientidmode="Static">
            <div class="col-md-6">
                <label>
                    Date Credation :</label>
                <table id="tblItems" class="table" style="width: 100%" cellpadding="0" cellspacing="0">
                    <thead>
                        <tr>
                            <th>
                                Date
                            </th>
                            <th>
                                Total Credits
                            </th>
                            <th>
                                Leave
                            </th>
                            
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td>
                                <asp:TextBox ID="txt_date" AutoComplete="off" runat="server" class="form-control datee"
                                    Style="width: 100px;" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_credits" AutoComplete="off" runat="server"
                                    class="form-control" Style="width: 200px;"  ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_ln" AutoComplete="off" runat="server"
                                    class="form-control" Style="width: 200px;"  ClientIDMode="Static"></asp:TextBox>
                               <%-- <asp:DropDownList ID="txt_ln" AutoComplete="off" runat="server"
                                    class="form-control" Style="width: 200px;"  ClientIDMode="Static"></asp:DropDownList>--%>
                            </td>
                         
                            <td>
                                <input type="button" id="btnAdd" value="ADD" class="btn btn-success btn-rounded mb-4" />
                            </td>
                        </tr>
                        
                    </tfoot>
                </table>
                <br />
            </div>
        </div>

       <%------------------------------------------------------------------------------------------------------------%>

        <hr />
             <div class="row">
                <div class="col-md-12">
                    <asp:Button ID="btnSave" runat="server" class="btn btn-success btn-rounded mb-4 pull-right" Text="Save" OnClick="onSetupSave" ClientIDMode="Static"  />
                </div>
            </div>
       </div>
     </section>
     <asp:HiddenField runat="server" ID="hfleavear" Value="" ClientIDMode="Static"/>
</asp:Content>
<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">
    
   <script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>     
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />

    <script type="text/javascript">
        jQuery.noConflict();
        (function ($) {
            $(function () {
                $(".datee").datepicker({
                    changeMonth: true,
                    showButtonPanel: true,
                    dateFormat: 'dd MM'
                });
            });
        })(jQuery);
    </script>
       <script type="text/javascript">
           $(document).ready(function () {
               $("#ddlLeave").on("change", function (e) {
                   if ($("#" + $(this).attr("id") + "").val() == 1) {
                       $("#lcrd").show();
                       $("#ytype").show();
                       $("#leex").show();
                       $("#lct").show();
                       $("#Div1").show();
                   }
                   else {
                       $("#lcrd").hide();
                       $("#ytype").hide();
                       $("#leex").hide();
                       $("#vlv").hide();
                       $("#lct").hide();
                       $("#Div1").hide();
                   }
               });


               $("#ddlYearly").on("change", function (e) {
                   if ($("#" + $(this).attr("id") + "").val() == 4) {
                       $("#vlv").show();
                   }
                   else {
                       $("#vlv").hide();
                   }
               });

               $("#btnAdd").on("click", function () {
                   console.log("1");
                   clickersz();
               });

               $.ajax({
                   type: "POST",
                   url: "content/Admin/Setup.aspx/getLeaveAdd",
                   contentType: "application/json;",
                   dataType: "json",
                   success: function (r) {
                       var additionals = r.d.split(",");
                       var txtdate = $("#txt_date");
                       var txtcred = $("#txt_credits");
                       var txtln = $("#txt_ln");
                       for (var i = 0; i < additionals.length - 1; i++) {
                           txtdate.val(additionals[i]);
                           txtcred.val(additionals[i + 1]);
                           txtln.val(additionals[i + 2]);
                           i = i + 2;
                           clickersz();
                       }
                       console.log("success");

                   },
                   error: function (r) {
                       console.log("error: " + r);
                   }
               });

           });

           function Remove(button) {
               var row = $(button).closest("TR");
               var name = $("TD", row).eq(3).html();
               if (confirm("Do you want to delete this row?")) {
                   var table = $("#tblItems")[0];
                   table.deleteRow(row[0].rowIndex);
               }
               leaveloader();
           };

           function leaveloader() {
               $("[id$=hfleavear]").val("");

               var tblarray = "";

               $("#tblItems TBODY TR").each(function () {
                   var row = $(this);
                   tblarray += row.find("TD").eq(0).find("input").eq(0).val() + "," + row.find("TD").eq(1).find("input").eq(0).val() + "," + row.find("TD").eq(2).find("input").eq(0).val() + ",";
               });

               $("[id$=hfleavear]").val(tblarray);

               console.log(tblarray);
           }

           function clickersz() {
               $("[id$=hfleavear]").val("");
               var txtdate = $("#txt_date");
               var txtcred = $("#txt_credits");
               var txtln = $("#txt_ln");

               var tBody = $("#tblItems > TBODY")[0];

               //Add Row.
               var row = tBody.insertRow(-1);

               var cell = $(row.insertCell(-1));
               cell.html("<input text='text' class='datee' disabled='true' style='width:100px;border-color:transparent;border-style:none;' value='" + txtdate.val() + "' />");

               cell = $(row.insertCell(-1));
               cell.html("<input text='text' disabled='true' style='border-color:transparent;outline:none;border-style:none;' value='" + txtcred.val() + "' />");

               cell = $(row.insertCell(-1));
               cell.html("<input text='text' disabled='true' style='border-color:transparent;outline:none;border-style:none;' value='" + txtln.val() + "' />");

               //Add Button cell.
               cell = $(row.insertCell(-1));
               var btnRemove = $("<input />");
               btnRemove.attr("type", "button");
               btnRemove.attr("onclick", "Remove(this);");
               btnRemove.val("Remove");
               cell.append(btnRemove);

               //Clear the TextBoxes.
               txtdate.val("");
               txtcred.val("");
               txtln.val("");

               leaveloader();
           }

   </script>


 <script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
    <script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>
   <script type="text/javascript">
       $(document).ready(function () {


           $("#txt_ln").autocomplete({
               source: function (request, response) {
                   $.ajax({
                       type: "POST",
                       contentType: "application/json; charset=utf-8",
                       url: "content/Admin/SetUp.aspx/Getleavetypes",
                       data: "{'term':'" + $("#txt_ln").val() + "'}",
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
                   $("[id$=txt_ln]").val(i.item.val);
               }
           });

       });
   </script>
      <link rel="stylesheet" href="style/multiple/docs/css/prettify.min.css" type="text/css">
    <script type="text/javascript" src="style/multiple/docs/js/bootstrap-3.3.2.min.js"></script>
    <script type="text/javascript" src="style/multiple/jquery.min.js"></script>
    <script type="text/javascript" src="style/multiple/bootstrap-multiselect.js"></script>
    <script type="text/javascript" src="style/multiple/bootstrap.min.js"></script>
    <link rel="Stylesheet" type="text/css" href="style/multiple/bootstrap-multiselect.css" />

   <script type="text/javascript">
       $(function () {
           $('[id*=listlt]').multiselect({

               includeSelectAllOption: true
           });

           $('[id*=liststatus]').multiselect({

               includeSelectAllOption: true
           });
           $('[id*=listdivision]').multiselect({

               includeSelectAllOption: true
           });
           $('[id*=txt_cond2]').multiselect({

               includeSelectAllOption: true
           });
       });
    </script>
    <style>
    .ui-datepicker-year{
    display:none;
}
     .checkbox input[type="checkbox"]
        { position:relative;
            }
    </style>
</asp:Content>

