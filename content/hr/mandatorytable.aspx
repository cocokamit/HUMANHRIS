<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mandatorytable.aspx.cs" Inherits="content_hr_mandatorytable" MasterPageFile="~/content/MasterPageNew.master" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="head">
</asp:Content>

<asp:Content ID="mandatorytables" runat="server" ContentPlaceHolderID="content">
<div class="page-title">
    <div class="title_left hd-tl">
        <h3>Mandatory Table</h3>
    </div>   
    <div class="title_right">
        <ul>
            <li><a href="#"><i class="fa fa-gear"></i>  Sytem Configurtion </a></li>
            <li><i class="fa fa-angle-right"></i></li>
            <li>Mandatory Table</li>
        </ul>
    </div>
</div>
<div class="clearfix"></div>
<div class="row">
    
    <div id="pgrid_sss" runat="server" visible="false" class="Overlay"></div>
    <div id="pgrid_sss2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton2" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txtstart" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtend" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txter" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtee" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txterec" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txteeec" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtmpf" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtermpf" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txteempf" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="btnsssss" OnClick="editssss" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgrid_hdmf" runat="server" visible="false" class="Overlay"></div>
    <div id="pgrid_hdmf2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton1" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txthdmfstat" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txthdmfend" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txthdmfemyr" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txthdmfemye" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txthdmfemyrval" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtdhmfemyeval" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="btnhdmfffs" OnClick="edithdmff" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgrid_phic" runat="server" visible="false" class="Overlay"></div>
    <div id="pgrid_phic2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton3" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txtpstart" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtpend" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtpsb" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtppec" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtppec2" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtppercent" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="btnppphic" OnClick="editphiccs" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgrid_train" runat="server" visible="false" class="Overlay"></div>
    <div id="pgrid_train2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton5" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txttraintamount" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txttrainttax" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txttraintpercent" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="Button4" OnClick="edittrainlaw" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div id="pgrid_taxcode" runat="server" visible="false" class="Overlay"></div>
    <div id="pgrid_taxcode2" runat="server" visible="false" class="PopUpPanel pop-a">
        <asp:ImageButton ID="ImageButton4" ImageUrl="~/style/img/closeb.png" OnClick="closepopup"  runat="server"/>
        <b>Details</b>
        <ul class="input-form">
            <li><asp:TextBox ID="txttaxcode" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtexcemt" runat="server"></asp:TextBox></li>
            <li><asp:TextBox ID="txtdepend" runat="server"></asp:TextBox></li>
            <li><hr /></li> 
            <li><asp:Button ID="Button3" OnClick="edittcode" runat="server" Text="Edit" CssClass="btn btn-primary"/></li>
        </ul>
    </div>
    <div class="col-md-9">
        <div class="x_panel">
            <div class="title_right">
                <asp:DropDownList ID="dl_choice" runat="server" AutoPostBack="true" OnSelectedIndexChanged="change_choice" CssClass="minimal">
                    <asp:ListItem>SSS</asp:ListItem>
                    <asp:ListItem>HDMF</asp:ListItem>
                    <asp:ListItem>PHIC</asp:ListItem>
                    <asp:ListItem>Train Law</asp:ListItem>
                    <asp:ListItem>Tax Code</asp:ListItem>
                </asp:DropDownList>
            </div>
            <asp:GridView ID="grid_sss" runat="server" AutoGenerateColumns="false"  CssClass="table table-striped table-bordered">
              <Columns>
                <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                <asp:BoundField DataField="AmountStart" HeaderText="START"/>
                <asp:BoundField DataField="AmountEnd" HeaderText="END"/>
                <asp:BoundField DataField="EmployerContribution" HeaderText="ER"/>
                <asp:BoundField DataField="EmployeeContribution" HeaderText="EE"/>
                <asp:BoundField DataField="EmployerEC" HeaderText="ER-EC"/>
                <asp:BoundField DataField="EmployeeEC" HeaderText="EE-EC"/>
                <asp:BoundField DataField="mpf" HeaderText="MPF"/>
                <asp:BoundField DataField="er_mpf" HeaderText="ER-MPF"/>
                <asp:BoundField DataField="ee_mpf" HeaderText="EE-MPF"/>
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate >
                        <asp:LinkButton ID="lb_showedit" runat="server" CommandName='<%# Eval("id") %>' Text="Showedit" ToolTip="Edit" OnClick="showsss" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle Width="70px" />
                </asp:TemplateField>
              </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_hdmf" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
              <Columns>
                <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                <asp:BoundField DataField="AmountStart" HeaderText="START"/>
                <asp:BoundField DataField="AmountEnd" HeaderText="END"/>
                <asp:BoundField DataField="EmployerPercentage" HeaderText="Employer %"/>
                <asp:BoundField DataField="EmployeePercentage" HeaderText="Employee %"/>
                <asp:BoundField DataField="EmployerValue" HeaderText="Employer Value"/>
                <asp:BoundField DataField="EmployeeValue" HeaderText="Employee Value"/>
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate >
                        <asp:LinkButton ID="lb_showedit" runat="server" CommandName='<%# Eval("id") %>' Text="Showedit" ToolTip="Edit" OnClick="showhdmf" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle Width="70px" />
                </asp:TemplateField>
              </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_phic" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
              <Columns>
                <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                <asp:BoundField DataField="AmountStart" HeaderText="START"/>
                <asp:BoundField DataField="AmountEnd" HeaderText="END"/>
                <asp:BoundField DataField="SalaryBracket" HeaderText="Salary Bracket"/>
                <asp:BoundField DataField="EmployerContribution" HeaderText="Employer Contribution"/>
                <asp:BoundField DataField="EmployeeContribution" HeaderText="Employee Contribution"/>
                <asp:BoundField DataField="percentage" HeaderText="Percentage"/>
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate >
                        <asp:LinkButton ID="lb_showedit" runat="server" CommandName='<%# Eval("id") %>' Text="Showedit" ToolTip="Edit" OnClick="showphicc" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle Width="70px" />
                </asp:TemplateField>
              </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_semi_monthly_wht" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
              <Columns>
                <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                <asp:BoundField DataField="TaxCode" HeaderText="Tax Code"/>
                <asp:BoundField DataField="TaxCodeValue" HeaderText="Exemption"/>
                <asp:BoundField DataField="Amount" HeaderText="Amount"/>
                <asp:BoundField DataField="Tax" HeaderText="Tax"/>
                <asp:BoundField DataField="Percentage" HeaderText="Excess Percentage"/>
              </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_monthly_wht" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
              <Columns>
                <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                <asp:BoundField DataField="TaxCode" HeaderText="Tax Code"/>
                <asp:BoundField DataField="TaxCodeValue" HeaderText="Exemption"/>
                <asp:BoundField DataField="Amount" HeaderText="Amount"/>
                <asp:BoundField DataField="Tax" HeaderText="Tax"/>
                <asp:BoundField DataField="Percentage" HeaderText="Excess Percentage"/>
              </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_yearly_wht" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
              <Columns>
                <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                <asp:BoundField DataField="AmountStart" HeaderText="START"/>
                <asp:BoundField DataField="AmountEnd" HeaderText="END"/>
                <asp:BoundField DataField="Percentage" HeaderText="Percentage"/>
                <asp:BoundField DataField="AdditionalAmount" HeaderText="Additional Amount"/>
              </Columns>
            </asp:GridView>
             <asp:GridView ID="gridtaxtable" runat="server" AutoGenerateColumns="false" OnRowDataBound="rowboundtaxtable" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="taxtable" HeaderText="Tax Table"/>
                    <asp:TemplateField>
                        <ItemTemplate>
                                 <asp:GridView ID="grid_train" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                                    <Columns>
                                        <asp:BoundField DataField="id" ItemStyle-CssClass="dnone" HeaderStyle-CssClass="dnone"/>
                                        <asp:BoundField DataField="amount" HeaderText="Amount"/>
                                        <asp:BoundField DataField="tax" HeaderText="Tax"/>
                                        <asp:BoundField DataField="Percentage" HeaderText="Percentage"/>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate >
                                            <asp:LinkButton ID="lb_showedit" runat="server" CommandName='<%# Eval("id") %>' Text="Showedit" OnClick="showsatrainlaw" ToolTip="Edit" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="70px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:GridView ID="yearly" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                                    <Columns>
                                        <asp:BoundField DataField="amountstart" HeaderText="Amount Start"/>
                                        <asp:BoundField DataField="amountend" HeaderText="Amount End"/>
                                        <asp:BoundField DataField="additionalamount" HeaderText="Additional Amount"/>
                                        <asp:BoundField DataField="percentage" HeaderText="Percentage"/>
                                    </Columns>
                                </asp:GridView>
                        </ItemTemplate>
                    </asp:TemplateField>
                 <%--   <asp:BoundField DataField="amount" HeaderText="Amount"/>
                    <asp:BoundField DataField="tax" HeaderText="Tax"/>
                    <asp:BoundField DataField="Percentage" HeaderText="Percentage"/>--%>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="grid_taxcode" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered">
                <Columns>
                    <asp:BoundField DataField="id" ItemStyle-CssClass="none" HeaderStyle-CssClass="none"/>
                    <asp:BoundField DataField="TaxCode" HeaderText="Tax Code"/>
                    <asp:BoundField DataField="ExemptionAmount" HeaderText="Exemption"/>
                    <asp:BoundField DataField="DependentAmount" HeaderText="Dependent"/>
                     <asp:TemplateField HeaderText="Action">
                            <ItemTemplate >
                            <asp:LinkButton ID="lb_showedit" runat="server" CommandName='<%# Eval("id") %>' Text="Showedit" ToolTip="Edit" OnClick="showtaxtable" style=" margin:3px;" ><i class="fa fa-pencil"></i></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="70px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="col-md-3">
        <div class="x_panel">
            <asp:Panel ID="p_sss" runat="server">
                <ul class="input-form">
                    <li><asp:FileUpload ID="fu_sss" runat="server" /></li>
                    <%--<asp:LinkButton ID="lnk_excel" runat="server" ToolTip="EXCEL" OnClick="template_sss" CssClass="glyphicon glyphicon-circle-arrow-lef" Font-Size="20px" style=" margin:3px"><i class="fa fa-file-excel-o"></i></asp:LinkButton>--%>
                    <hr />
                    <li>START</li>
                    <li><asp:TextBox ID="txt_start" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>END</li>
                    <li><asp:TextBox ID="txt_end" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>ER</li>
                    <li><asp:TextBox ID="txt_er" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>EE</li>
                    <li><asp:TextBox ID="txt_ee" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>ER-EC</li>
                    <li><asp:TextBox ID="txt_erec" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>EE-EC</li>
                    <li><asp:TextBox ID="txt_eeec" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>MPF</li>
                    <li><asp:TextBox ID="txt_mpf" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>ER-MPF</li>
                    <li><asp:TextBox ID="txt_ermpf" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>EE-MPF</li>
                    <li><asp:TextBox ID="txt_eempf" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li><hr /></li>
                    <li><asp:Button ID="btn_sss"  runat="server" OnClick="click_add_sss" Text="Save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_hdmf" runat="server">
                <ul class="input-form">
                    <li>START</li>
                    <li><asp:TextBox ID="txt_hdmf_start" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>END</li>
                    <li><asp:TextBox ID="txt_hdmf_end" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>EMPLOYEER %</li>
                    <li><asp:TextBox ID="txt_hdmf_er" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>EMPLOYEE %</li>
                    <li><asp:TextBox ID="txt_hdmf_ee" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>EMPLOYEER VALUE</li>
                    <li><asp:TextBox ID="txt_hdmf_erv" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>EMPLOYEE VALUE</li>
                    <li><asp:TextBox ID="txt_hdmf_eev" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li><hr /></li>
                    <li><asp:Button ID="btn_hdmf"  runat="server" OnClick="click_add_hdmf" Text="save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_phic" runat="server">
                <ul class="input-form">
                    <li>START</li>
                    <li><asp:TextBox ID="txt_phic_start" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>END</li>
                    <li><asp:TextBox ID="txt_phic_end" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>SALARY BRACKET</li>
                    <li><asp:TextBox ID="txt_phic_salarybracket" AutoComplete="off" onkeyup="intinput(this)" runat="server"></asp:TextBox></li>
                    <li>EMPLOYEER CONTRIBUTION</li>
                    <li><asp:TextBox ID="txt_phic_employercontribution" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>EMPLOYEE CONTRIBUTION</li>
                    <li><asp:TextBox ID="txt_phic_employeecontribution" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li><hr /></li>
                    <li><asp:Button ID="btn_phic"  runat="server" OnClick="click_add_phic" Text="save"  CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_semi_monthly_wht" runat="server">
                <ul class="input-form">
                    <li>TAX CODE</li>
                    <li><asp:dropdownlist ID="ddl_semi_monthly_wht_taxcode"  DataTextField="TaxCode" DataValueField="Id" DataSourceID="sql_taxcode" runat="server" CssClass="minimal"></asp:dropdownlist></li>
                    <li>EXEMPTION</li>
                    <li><asp:TextBox ID="txt_semi_monthly_wht_exemption" AutoComplete="off" onkeyup="decimalinput(liis)" runat="server"></asp:TextBox></li>
                    <li>AMOUNT</li>
                    <li><asp:TextBox ID="txt_semi_monthly_wht_amount" AutoComplete="off" onkeyup="decimalinput(liis)" runat="server"></asp:TextBox></li>
                    <li>TAX</li>
                    <li><asp:TextBox ID="txt_semi_monthly_wht_tax" AutoComplete="off" onkeyup="decimalinput(liis)" runat="server"></asp:TextBox></li>
                    <li>EXCESS PERCENTAGE</li>
                    <li><asp:TextBox ID="txt_semi_monthly_wht_excesspercentage" AutoComplete="off" onkeyup="decimalinput(liis)" runat="server"></asp:TextBox></li>
                    <li><hr /></li>
                    <li><asp:Button ID="Button8"  runat="server" OnClick="click_add_semiwht" Text="save" CssClass="btn btn-primary" /></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_tax_train" runat="server">
                <ul class="input-form">
                    <li>Tax Table</li>
                    <li><asp:dropdownlist ID="ddl_taxtable" AutoComplete="off" onkeyup="decimalinput(this)" OnSelectedIndexChanged="selecttaxtable" AutoPostBack="true"  runat="server" CssClass="minimal">
                    <asp:ListItem Value="1">Daily</asp:ListItem>
                    <asp:ListItem Value="2">Weekly</asp:ListItem>
                    <asp:ListItem Value="3">Semi-Monthly</asp:ListItem>
                    <asp:ListItem Value="4">Monthly</asp:ListItem>
                    <asp:ListItem Value="6">Yearly</asp:ListItem>
                    </asp:dropdownlist></li>
                 </ul>
               <div id="ttttt" runat="server">
                   <ul class="input-form">
                        <li>AMOUNT</li>
                        <li><asp:TextBox ID="txt_tt_amt" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                        <li>TAX</li>
                        <li><asp:TextBox ID="txt_tt_tax" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                        <li>EXCESS PERCENTAGE</li>
                        <li><asp:TextBox ID="txt_tt_excess" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                        <li><hr/></li>
                        <li><asp:Button ID="Button1"  runat="server" OnClick="click_train_law" Text="save" CssClass="btn btn-primary"/></li>
                    </ul>
                </div>
                <div id="ttt_year" visible="false" runat="server">
                    <ul class="input-form">
                            <li>Amount Start</li>
                            <li><asp:TextBox ID="txt_amtstart" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                             <li>Amount End</li>
                            <li><asp:TextBox ID="txt_amtend" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                            <li>Additional Amount</li>
                            <li><asp:TextBox ID="txt_amtadd" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                            <li>Rate</li>
                            <li><asp:TextBox ID="txt_rate" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                            <li><hr/></li>
                            <li><asp:Button ID="Button2"  runat="server" OnClick="click_train_law_yearly"  Text="save" CssClass="btn btn-primary"/></li>
                     </ul>
                </div>
            </asp:Panel>
            <asp:Panel ID="p_monthly_wht" runat="server">
                <ul class="input-form">
                    <li>TAX CODE</li>
                    <li><asp:dropdownlist ID="ddl_monthly_wht_taxcode" AutoComplete="off" onkeyup="decimalinput(this)" DataTextField="TaxCode" DataValueField="Id" DataSourceID="sql_taxcode" runat="server" CssClass="minimal"></asp:dropdownlist></li>
                    <li>EXEMPTION</li>
                    <li><asp:TextBox ID="txt_monthly_wht_exemption" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>AMOUNT</li>
                    <li><asp:TextBox ID="txt_monthly_wht_amount" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>TAX</li>
                    <li><asp:TextBox ID="txt_monthly_wht_tax" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>EXCESS PERCENTAGE</li>
                    <li><asp:TextBox ID="txt_monthly_wht_percentage" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li><hr /></li>
                    <li><asp:Button ID="Button9"  runat="server" OnClick="click_add_monthlywht" Text="save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_yearly_wht" runat="server">
                <ul class="input-form">
                    <li>START</li>
                    <li><asp:TextBox ID="txt_yearly_wht_start" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>END</li>
                    <li><asp:TextBox ID="txt_yearly_wht_end" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>PERCENTAGE</li>
                    <li><asp:TextBox ID="txt_yearly_wht_percentage" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>ADDITIONAL Amount</li>
                    <li><asp:TextBox ID="txt_yearly_wht_addamount" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li><hr /></li>
                    <li><asp:Button ID="Button10"  runat="server" OnClick="click_add_yearlywht" Text="save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
            <asp:Panel ID="p_taxcode" runat="server">
                <ul class="input-form">
                    <li>TAX CODE</li>
                    <li><asp:TextBox ID="txt_tax_code_taxcode" AutoComplete="off" runat="server"></asp:TextBox></li>
                    <li>EXEMPTION</li>
                    <li><asp:TextBox ID="txt_tax_code_exemption" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li>DEPENDENTS</li>
                    <li><asp:TextBox ID="txt_tax_code_dependent" AutoComplete="off" onkeyup="decimalinput(this)" runat="server"></asp:TextBox></li>
                    <li><hr /></li>
                    <li><asp:Button ID="Button5"  runat="server" OnClick="click_add_taxcode" Text="save" CssClass="btn btn-primary"/></li>
                </ul>
            </asp:Panel>
       </div>
    </div>
</div>

<asp:HiddenField ID="seriesid" runat="server" />
<script src="script/auto/myJScript.js" type="text/javascript"></script>
<asp:SqlDataSource ID="sql_company" runat="server" ConnectionString="<%= Config.connection() %>" ProviderName="System.Data.SqlClient"  SelectCommand="select * from MCompany order by id desc"></asp:SqlDataSource>
<asp:SqlDataSource ID="sql_taxcode" runat="server" ConnectionString="<%= Config.connection() %>" ProviderName="System.Data.SqlClient"  SelectCommand="select * from MTaxcode order by id desc"></asp:SqlDataSource>
</asp:Content>
