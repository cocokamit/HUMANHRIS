<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home"
    MasterPageFile="~/content/site.master" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="Server">

    <link rel="stylesheet" type="text/css" href="style/css/nobel.css" />

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
        
    </style>
</asp:Content>
<asp:Content ID="content1" runat="server" ContentPlaceHolderID="content">
    <section class="content-header">
    <div class="page-title">
        <div class="title_left hd-tl">
            <h3>Home</h3>
               
        </div>
           <div class="title_right">
       <ul>
        <li><a href="homepage2"><i class="fa fa fa-dashboard"></i> Home</a></li>
       </ul>
    </div> 
    </div>

    </section>
    <section class="content">    
     <div class="row">
                <div class="col-md-12" style="width:100%;">
                  <asp:Image ID="homebanner" runat="server" style="100%;" CssClass="image img-responsive" ClientIDMode="Static" ImageUrl="~/style/images/emptyimage.png"  />
                </div>
              </div>
        
              <br />
              <div class="row">
              <div class="col-md-12">
          
            <div class="rowr" runat="server" id="divannouncement"> 
            </div>
              </div>
            </div>
              <hr />
            
            <hr />
            <br />
     </section>
   
<asp:HiddenField ID="hf_empid" runat="server"/>
</asp:Content>
<asp:Content ID="footer" runat="server" ContentPlaceHolderID="footer">

    <script type="text/javascript">

         $(document).ready(function () {
               var empid=$("[id$=hf_empid]").val();
             
             console.log("c");
                       $.ajax({
                        type: "POST",
                        url: "content/Employee/default.aspx/getLeaveUpdate",
                        data: JSON.stringify({
                            id: empid
                        }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (r) {

                            console.log("success");

                        },
                        error: function (xhr,error,status) {
                        console.log("error: " + xhr.responseText);
                        }
                    });
            
         });
     </script>
</asp:Content>
