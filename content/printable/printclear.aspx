<%@ Page Language="C#" AutoEventWireup="true" CodeFile="printclear.aspx.cs" Inherits="content_printable_printclear" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <link rel="stylesheet" href="../../vendors/bootstrap/dist/css/bootstrap.min.css">
    <link rel="stylesheet" href="../../vendors/font-awesome/css/font-awesome.min.css">
    <link rel="stylesheet" href="../../dist/css/base.css">
    <link rel="stylesheet" href="../../dist/css/custom.css" />
    <link rel="stylesheet" href="../../dist/css/skins/_all-skins.min.css">
     <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,600,700,300italic,400italic,600italic">
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <!-- Main content -->
    <section class="invoice">
      <!-- title row -->
   
        <%
            string g = "select left(convert(varchar,getdate(),101),10)Date_view,  case when left(convert(varchar,DATEADD(MONTH,1,a.date_accomplished),101),10) is null then '-' else left(convert(varchar,DATEADD(MONTH,1,a.date_accomplished),101),10) end separationdate, case when left(CONVERT(varchar,a.date_accomplished,101),10) is null then '-' else left(CONVERT(varchar,a.date_accomplished,101),10) end date_accom,left(CONVERT(varchar,b.datehired,101),10)datehired,c.position,b.idnumber,b.lastname+', '+b.firstname+' '+b.middlename e_name,a.id,a.status,a.empid from Texitclearance a left join memployee b on a.empid=b.id left join mposition c on b.positionid=c.id where a.resignid=" + Request.QueryString["key"] + "";
            System.Data.DataTable dtempdet = dbhelper.getdata(g); %>

      <div class="row">
        <div class="col-xs-12">
          <h2 class="page-header">
            <i class="fa fa-globe"></i> MeshNetworks Inc.
            <small class="pull-right">Date: <% Response.Write(dtempdet.Rows[0]["Date_view"].ToString()); %></small>
          </h2>
        </div>
        <!-- /.col -->
      </div>
      <!-- info row -->
      <div class="row invoice-info">

    

        <div class="col-sm-4 invoice-col">

         <b>Id Number: </b><% Response.Write(dtempdet.Rows[0]["idnumber"].ToString()); %><br>
         <b>Name: </b><% Response.Write(dtempdet.Rows[0]["e_name"].ToString()); %><br>
         <b>Date Hired: </b><% Response.Write(dtempdet.Rows[0]["position"].ToString()); %><br>
      
        </div>
        <!-- /.col -->
        <div class="col-sm-4 invoice-col">
        <%--  To
          <address>
            <strong>John Doe</strong><br>
            795 Folsom Ave, Suite 600<br>
            San Francisco, CA 94107<br>
            Phone: (555) 539-1037<br>
            Email: john.doe@example.com
          </address>--%>
        </div>
        <!-- /.col -->
        <div class="col-sm-4 invoice-col">
         <b>Position: </b> <% Response.Write(dtempdet.Rows[0]["position"].ToString()); %><br>
         <b>Date accomplished: </b><% Response.Write(dtempdet.Rows[0]["date_accom"].ToString()); %><br>
         <b>Separation Date: </b><% Response.Write(dtempdet.Rows[0]["separationdate"].ToString()); %><br>
        </div>
        <!-- /.col -->
      </div>
      <!-- /.row -->
        <p class="text-muted well well-sm no-shadow" style="margin-top: 10px;">
            Reason for separation: <b>RESIGNATION</b> <br />
            *Final separation pay, if there’s any, will be available not more than 30 days after clearance is forwarded for computation. 
        </p>
        <p class="text-muted well well-sm no-shadow" style="margin-top: 10px;">
            <b>PROPERTY HAND-OVER</b> <br />
            This is to certify that the above-mentioned employee has properly handed over company-issued properties, which are issued to him for use in performance of his duties with the company.
        </p>
      <!-- Table row -->
      <div class="row">
        <div class="col-xs-12 table-responsive">
          <table class="table table-striped">
            <thead>
            <tr>
              <th>Category</th>
              <th>Product Description</th>
              <th>Qty</th>
              <th>Um</th>
              <th>Status</th>
            </tr>
            </thead>
            <tbody>
            <%
               
                
                string query = "select a.id,b.Description cat,a.qty,a.serialno, b.id,b.um,  " +
                                "case when a.serialno > 0 then convert(varchar,c.serial)+' - '+c.description  else b.description  end description ,case when a.status is null then 'Deployed' else a.status end status " +
                                "from asset_assign a  " +
                                "left join  asset_cat b on a.categoryid=b.id  " +
                                "left join asset_inventory c on CONVERT(int,a.serialno)=c.id  " +
                                "where a.action is null and a.empid=" +  dtempdet.Rows[0]["empid"].ToString() + "";
                System.Data.DataTable dtasset = dbhelper.getdata(query); 
                foreach (System.Data.DataRow dr in dtasset.Rows)
                {%>
            <tr>
              <td><% Response.Write(dr["cat"].ToString()); %></td>
              <td><% Response.Write(dr["description"].ToString()); %></td>
              <td><% Response.Write(dr["qty"].ToString()); %></td>
              <td><% Response.Write(dr["um"].ToString()); %></td>
              <td><% Response.Write(dr["status"].ToString()); %></td>
            </tr>
            <%} %>
            <%if (dtasset.Rows.Count == 0)
              {%>
              <tr >
              <td colspan="5">Not Applicable.......</td>
             </tr>
            <%} %>
            </tbody>
          </table>
        </div>
        <!-- /.col -->
      </div>
      <!-- /.row -->

      <p class="text-muted well well-sm no-shadow" style="margin-top: 10px;">
            <b>SIGNATORIES</b> <br />
            This is to certify that the above–named employee is free from any responsibilities, liabilities, monetary or otherwise, from <b>MeshNetworks, Inc.</b> as of the stipulated date as validated by the affixed signatures.
        </p>

          <!-- Table row -->
      <div class="row">
        <div class="col-xs-12 table-responsive">
          <table class="table table-striped">
            <thead>
            <tr>
              <th>Business Unit / Department</th>
              <th>Clearing Officer’s Name</th>
              <th>Status</th>
            </tr>
            </thead>
            <tbody>
            <%
                System.Data.DataTable dtsign = dbhelper.getdata(getdata.getlistapproverstatus(dtempdet.Rows[0]["empid"].ToString(), dtempdet.Rows[0]["id"].ToString()));
               foreach (System.Data.DataRow drsign in dtsign.Rows)
               {
                %>
                <tr>
                  <td><%Response.Write(drsign["position"]); %></td>
                  <td><%Response.Write(drsign["e_name"]); %></td>
                  <td><%Response.Write(drsign["sstatus"]); %></td>
                </tr>
            <%} %>
            </tbody>
          </table>
        </div>
        <!-- /.col -->
      </div>
      <!-- /.row -->

        <p class="text-muted well well-sm no-shadow" style="margin-top: 10px;">
            <b>AUTHORIZATION TO DEDUCT</b> <br />
            I hereby authorize <b>MeshNetworks, Inc.</b>. to deduct  from my last pay, all accountabilities due to the company or its officers as enumerated in this Exit Clearance.  
        </p>


      <div class="row">
        <!-- accepted payments column -->
        <div class="col-xs-6">
            <p>Employee Information:</p>
            <p>Contact Number: </p>
            <p>Address:</p>
        <%--  <img src="../../dist/img/credit/visa.png" alt="Visa">
          <img src="../../dist/img/credit/mastercard.png" alt="Mastercard">
          <img src="../../dist/img/credit/american-express.png" alt="American Express">
          <img src="../../dist/img/credit/paypal2.png" alt="Paypal">--%>

         
        </div>
        <!-- /.col -->
        <div class="col-xs-6">
          <p>Conformé:</p>

          <div class="table-responsive">
           <%-- <table class="table">
              <tr>
                <th style="width:50%">Subtotal:</th>
                <td>$250.30</td>
              </tr>
              <tr>
                <th>Tax (9.3%)</th>
                <td>$10.34</td>
              </tr>
              <tr>
                <th>Shipping:</th>
                <td>$5.80</td>
              </tr>
              <tr>
                <th>Total:</th>
                <td>$265.24</td>
              </tr>
            </table>--%>
          </div>
        </div>
        <!-- /.col -->
      </div>
      <!-- /.row -->


         

          <% if (dtempdet.Rows[0]["status"].ToString() == "Approved")
             {%>
            <p class="text-muted well well-sm no-shadow" style="margin-top: 10px;">
           This is to certify that the above-named employee is free from any responsibilities, liabilities, monetary or otherwise, from Shared Services - HRas of the stipulated date as validated by the affixed signatures.       
          </p>
          
          <%}
             else
             { %>
              <p class="text-muted well well-sm no-shadow bg-red" style="margin-top: 10px;">
           <b>Note:</b> <br />
           This clearance form is not yet cleared.     
          </p>
          <%} %>
    </section>
    <!-- /.content -->
    <div class="clearfix"></div>
  </div>
<div class="clearfix"></div>
    </div>

     <!-- jQuery 3 -->
    <script src="vendors/jquery/dist/jquery.min.js"></script>
    <!-- Bootstrap 3.3.7 -->
    <script src="vendors/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- SlimScroll -->
    <script src="vendors/jquery-slimscroll/jquery.slimscroll.min.js"></script>
    <!-- FastClick -->
    <script src="vendors/fastclick/lib/fastclick.js"></script>
    <!-- AdminLTE App -->
    <script src="dist/js/adminlte.min.js"></script>
    <script src="dist/js/active.js"></script>
    </form>
</body>
</html>
