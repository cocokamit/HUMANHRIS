<%@ Page Language="C#" AutoEventWireup="true" CodeFile="leave_form.aspx.cs" Inherits="content_printable_leave_form" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HRIS | Leave Form</title>
    <style type="text/css">
         *{margin:0; padding:0}
         p{ text-align:justify; line-height:30px}
        .hd { text-transform:uppercase; text-align:center; font-size:30px }
        .content {font-family:"Lucida Sans Unicode", "Lucida Grande", sans-serif; width:900px; margin:50px auto}
        .box {width:48%; float:left; text-align:center; margin-bottom:50px}
        .box span { display:block}
        .box-fot {text-align:center; width:100%}
        .box-pd { padding-left:90px}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="content">
        
        <div class="hd">Leave Application Form</div>


          


        <p>
        Date: <asp:Label ID="lbl_date" style="color:Red;" runat="server" Text="Label"></asp:Label><br />
        Reason for leave: <asp:Label ID="lbl_reason" style="color:Red;" runat="server" Text="Label"></asp:Label><br />
        I, <asp:Label ID="lbl_name" runat="server" style="color:Red;" Text="Label"></asp:Label> of <asp:Label ID="lbl_dept" runat="server" style="color:Red;" Text="Label"></asp:Label> department request for  <asp:Label ID="lbl_l" runat="server" style="color:Red;" Text="Label"></asp:Label> from  <asp:Label ID="lbl_from" runat="server" style="color:Red;" Text="Label"></asp:Label> to <asp:Label ID="lbl_to" runat="server" style="color:Red;" Text="Label"></asp:Label>.
        </p>
        <br />
        <p>I hereby delegated my duties and responsibilities to <asp:Label ID="lbl_delegate" runat="server" style="color:Red;" Text="Label"></asp:Label> for the duration of my leave of absence. I understand that
        should i fail to report back to work within 48 hours after the time requested above, i should be charged with Absence Without Official
        Leave (AWOL) and shall not be paid for the extra days incurred, which will greatly affect my performance rating.</p>
         <br />
        <p>Below certifies that i have obtained permission from immediate supervisors.</p>
        <br />
         <br />
        <div class="box">
            <span>CONFORME:__________________________________</span>  
            <span class="box-pd">Applicant</span>
        </div>

         <div class="box">
             <span>NOTED BY: __________________________________</span>
             <span class="box-pd">Supervisor/Manager</span>
         </div>

          <div class="box">
              <span class="box-pd">__________________________________  </span>
              <span class="box-pd">Delegated to</span>
          </div>

        <div class="box">
            <span>VERIFIED BY: __________________________________</span>
            <span class="box-pd"> Admin / HR</span>
        </div>
        <div class="box box-fot" style=" display:none">
            <span>APPROVED BY:  <b>Juan Dela Cruz</b></span>
            <span>__________________________________</span>
            <span>President</span>
        </div>
    </div>
    <asp:HiddenField ID="id" runat="server" />
    </form>
</body>
</html>
