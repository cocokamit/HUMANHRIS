<%@ Page Language="C#" AutoEventWireup="true" CodeFile="clearancedetails.aspx.cs" Inherits="content_hr_clearancedetails" MasterPageFile="~/content/site.master" %>

<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_rdl">
<section class="content-header">
    <h1></h1>
    <ol class="breadcrumb">
    <li><a href="employee-dashboard"><i class="fa fa-dashboard"></i> Dashboard</a></li>
    <li class="active">Resignation Form</li>
    </ol>
</section>
<section class="content">
<!-- Main content -->
    <section class="invoice">
      <!-- title row -->
      <div class="row">
        <div class="col-xs-12">
          <h2 class="page-header">
            <i class="fa fa-globe"></i> MeshNetworks Inc.
            <small class="pull-right">Date: 2/10/2014</small>
          </h2>
        </div>
        <!-- /.col -->
      </div>
      <!-- info row -->
      <div class="row invoice-info">

      


        <div class="col-sm-4 invoice-col">

         <b>Id Number:</b> 4F3S8J<br>
         <b>Name:</b>Nobel Barro Caquelala<br>
         <b>Date Hired:</b>09/10/2018<br>
      
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

         <b>Department:</b> 4F3S8J<br>
         <b>Date accomplished:</b>Nobel Barro Caquelala<br>
         <b>Separation Date:</b>09/10/2018<br>

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
                                "where a.action is null and a.status is null and a.empid=" + Session["emp_id"].ToString() + "";
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
               System.Data.DataTable dtsign = dbhelper.getdata(getdata.getlistapproverstatus(Session["emp_id"].ToString(),Request.QueryString["key"].ToString()));
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


          <p class="text-muted well well-sm no-shadow" style="margin-top: 10px;">
           This is to certify that the above-named employee is free from any responsibilities, liabilities, monetary or otherwise, from Shared Services - HRas of the stipulated date as validated by the affixed signatures.        </p>

      <!-- this row will not appear when printing -->
      <div class="row no-print">
        <div class="col-xs-12">
          <a href="invoice-print.html" target="_blank" class="btn btn-default"><i class="fa fa-print"></i> Print</a>
          <button type="button" class="btn btn-success pull-right"><i class="fa fa-credit-card"></i> Submit Payment
          </button>
          <button type="button" class="btn btn-primary pull-right" style="margin-right: 5px;">
            <i class="fa fa-download"></i> Generate PDF
          </button>
        </div>
      </div>
    </section>
    <!-- /.content -->
    <div class="clearfix"></div>
  </div>
</section>
<div class="clearfix"></div>
</asp:Content>
