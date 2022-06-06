function po_total(input, x) {
    var price = document.getElementById("contents_grid_item_txt_sprice_" + x).value.length == 0 ? 0 : document.getElementById("contents_grid_item_txt_sprice_" + x).value;
    var qty = document.getElementById("contents_grid_item_txt_adqty_" + x).value.length == 0 ? 0 : document.getElementById("contents_grid_item_txt_adqty_" + x).value;

    vva = input.value;
    vva = vva.split(",").join("");
    if (IfNumeric(vva) || price == 0 || qty == 0)
        document.getElementById("contents_grid_item_txt_total_" + x).value = price * qty;
    else
        input.value = vva.substring(0, vva.length - 1);
}
function intinput(elem) {
    var input = !(/^[0-9.]+$/).test(elem.value) ? elem.value = elem.value.replace(/[^0-9.]/ig, '') : null;
    input = elem.value.split(",").join("");
    elem.value = input.split(".").join("");
}
function decimalinput(elem) {

    var input = !(/^[0-9.]+$/).test(elem.value) ? elem.value = elem.value.replace(/[^0-9.]/ig, '') : null;
    input = elem.value.split(",").join("");
    if (!IfNumeric(input))
        elem.value = addCommas(input.substring(0, input.length - 1));
    else
        elem.value = addCommas(input);
}
function checkNumericInput_a(elem) {
    var input = !(/^[0-9.]+$/).test(elem.value) ? elem.value = elem.value.replace(/[^0-9.]/ig, '') : null;
    input= elem.value.split(",").join("");
    if (!IfNumeric(input))
        elem.value = addCommas(input.substring(0, input.length - 1));
    else
        elem.value = addCommas(input);
}

function checkNumericInput(elem) {
    vva = elem.value;
    vva = vva.split(",").join("");
    if (!IfNumeric(vva))
        elem.value = vva.substring(0, vva.length - 1);
  }

function addCommas(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

function IfNumeric(input) {
     return (input - 0) == input && (input + '').replace(/^\s+|\s+$/g, "").length > 0;
  }
function kita(elem) {
    document.getElementById(elem).style.display = "inline";
}

function addcompute() {

    var selID=document.getElementById("ddl_payrolltype");
    var text = selID.options[selID.selectedIndex].value;
    
    var taxid = document.getElementById("ddl_taxtable")
    var taxtext = taxid.options[taxid.selectedIndex].value;

    var fnodid = document.getElementById("txt_fnod");
    var fnod = fnodid.options[fnodid.selectedIndex].innerHTML;

    //var fnod = document.getElementById("txt_fnod").value.length == 0 ? 0 : document.getElementById("txt_fnod").value.replace(",", "");
   
    var fnoh = document.getElementById("txt_fnoh").value.length == 0 ? 0 : document.getElementById("txt_fnoh").value.replace(",", "");
    var mr = document.getElementById("txt_mr").value.length == 0 ? 0 : document.getElementById("txt_mr").value.replace(",", "");
    var dr = document.getElementById("txt_dr").value.length == 0 ? 0 : document.getElementById("txt_dr").value.replace(",", "");
    var hr = document.getElementById("txt_hr").value.length == 0 ? 0 : document.getElementById("txt_hr").value.replace(",", "");
    var minwage = document.getElementById("hdn_minimumwage").value;

    //alert(minwage)
    if (text == "1") {
        document.getElementById("txt_mr").setAttribute("onfocus", "");
        document.getElementById("txt_nti").setAttribute("onfocus", "");
        var payrolldivission = "";
        switch (taxtext) {
            case "Semi-Monthly":
                payrolldivission = "2";
                break;
            case "Monthly":
                payrolldivission = "1";
                break;
        }
        //var daymonth = (fnod) / 12;
        document.getElementById("txt_pr").value = (mr / payrolldivission);
        document.getElementById("txt_dr").value = (mr * 12 / fnod) >= minwage ? (mr * 12 / fnod) : minwage;
        document.getElementById("txt_hr").value =  (document.getElementById("txt_dr").value.replace(",", "") / document.getElementById("txt_fnoh").value.replace(",", ""));
        document.getElementById("txt_dr").setAttribute("onfocus", "this.blur()");
    }
    else if (text == "2") {

        document.getElementById("txt_dr").setAttribute("onfocus", "");
        document.getElementById("txt_mr").value = 0;
      
        document.getElementById("txt_pr").value = 0;
        document.getElementById("txt_mr").setAttribute("onfocus", "this.blur()");
      
        document.getElementById("txt_pr").setAttribute("onfocus", "this.blur()");
        document.getElementById("txt_nti").setAttribute("onfocus", "this.blur()");

        var dr = document.getElementById("txt_dr").value;
      
        document.getElementById("txt_hr").value = (document.getElementById("txt_dr").value.replace(",", "") / document.getElementById("txt_fnoh").value.replace(",", ""));
        document.getElementById("txt_nti").value = (document.getElementById("txt_meal_allow").value * fnod / 12);
        
    }

}
function get() {
  //  alert('test');
    if (document.getElementById('rb_range').checked == true || document.getElementById('rb_half').checked == true) {
        document.getElementById('grid_leave').outerHTML = "";
    }
}

function get_no_of_hours_between_in_out() 
{
    alert("test");
    var timein = document.getElementById("txt_timein1").value;
    var timeout = document.getElementById("txt_timeout2").value;
    var timeStart = new Date("01/01/2007 " + timein).getHours();
    var timeStartmin = new Date("01/01/2007 " + timein).getMinutes();
    var timeEnd = new Date("01/01/2007 " + timeout).getHours();
    var timeEndmin = new Date("01/01/2007 " + timeout).getMinutes();

    timeStart = timeStart + "." + timeStartmin;
    timeEnd = timeEnd + "." + timeEndmin;
    var hourDiff = timeEnd - timeStart;
     document.getElementById("txt_noh").value = hourDiff;
 }







 //addPayrollOtherIncome.aspx
 function selectpg() 
 {
     var selectedID = document.getElementById("ddl_payroll_group");
     var selectedval = selectedID.options[selectedID.selectedIndex].value;
     $.ajax({
         type: "POST",
         contentType: "application/json; charset=utf-8",
         url: "content/payroll/transotherincome.aspx/getrange",
         data: "{'pgid':'" + selectedval + "'}",
         dataType: "json",
         success: function (rtn) {
             var gg = rtn.d;
             $('[id$=ddl_pg]').empty();
             $.each(gg, function (Value, Text) {
                 $('[id$=ddl_pg]').append(new Option(Text.Text, Text.Value));
             });
         },
         error: function (result) {
             alert(result.responseText);
         }
     });
 }
 function verify_data()
 {
      var PGselectedID = document.getElementById("ddl_payroll_group");
      var PGselectedval = PGselectedID.options[PGselectedID.selectedIndex].value;
      var PRselectedID = document.getElementById("ddl_pg");
      var PRselectedval = PRselectedID.options[PRselectedID.selectedIndex].value;
      var PRselectedtext = PRselectedID.options[PRselectedID.selectedIndex].text;
      console.log(PRselectedtext);
      $.ajax({
          type: "POST",
          contentType: "application/json; charset=utf-8",
          url: "content/payroll/transotherincome.aspx/verifydata",
          data: "{'pgid':'" + PGselectedval + "','prid':'" + PRselectedval + "','prtext':'" + PRselectedtext + "'}",
          dataType: "json",
          success: function (rtn) {
              var data = rtn.d;
              $('[id$=tbl]').find('tbody').empty();
              for (var i = 0; i < data.length; i++) {
                  var thw = data[i].total_hours_worked.lenght > 0 ? data[i].total_hours_worked : "0.00";
                  var row = $("<tr> " +
                                    "<td style='display:none;'><span class='otherincomid'>" + data[i].otherincomid + "</span></td> " +
                                    "<td style='display:none;'><span class='empid'>" + data[i].empid + "</span></td> " +
                                    "<td><span class='emp_name'>" + data[i].emp_name + "</span></td> " +
                                    "<td><span class='OtherIncome'>" + data[i].OtherIncome + "</span></td> " +
                                    "<td><span class='Amount'>" + data[i].Amount + "</span></td> " +
                                    "<td><span class='total_hours_worked'>" + thw + "</span></td> " +
                                    "<td><span class='amt_to_bepaid'>" + data[i].amt_to_bepaid + "</span></td> " +
                                    "<td><span class='amt_to_be_tax'>" + data[i].Amt_to_be_tax + "</span></td> " +
                                    "<td><input type='checkbox' name='record'></td> " +
                                "</tr>");
                  $('[id$=tbl]').find('tbody').append(row);
              }
          },
          error: function (result) {
              alert(result.responseText);
          }
      });
  }
  function add_to_table(classification) {

      var employee = classification == "new" ? $('[id$=txt_searchemp]').val().length : $('[id$=txt_searchemp_his]').val().length;
      var employeeid = $('[id$=lbl_bals]').val().length;
      var amount = 0;
      var incomeid = classification == "new" ? $("#ddl_type :selected").val() : $("#ddl_income_type :selected").val();
      var wh = classification == "new" ?$('[id$=work_hrs]').val().length : $('[id$=work_hrs_his]').val().length;
      var nt = classification == "new" ?$('[id$=nontax]').val().length:$('[id$=nontax_his]').val().length;
      var taxable = classification == "new" ? $('[id$=taxable]').val().length : $('[id$=taxable_his]').val().length;
     
      console.log(employee);
      console.log(employeeid);
      console.log(incomeid);
      console.log(wh);
      console.log(nt);
      console.log(taxable);

      var employeeid_val = $('[id$=lbl_bals]').val();
      var incomeid_val = classification == "new" ? $("#ddl_type :selected").val() : $("#ddl_income_type :selected").val();
      var amount_val = 0;
      var wh_val = classification == "new" ? $('[id$=work_hrs]').val() : $('[id$=work_hrs_his]').val();
      var nt_val = classification == "new" ? $('[id$=nontax]').val() : $('[id$=nontax_his]').val();
      var taxable_val = classification == "new" ? $('[id$=taxable]').val() : $('[id$=taxable_his]').val();
      var trnsid = $('[id$=txt_trn_id]').val();
      var empname = classification == "new" ?$('[id$=txt_searchemp]').val():$('[id$=txt_searchemp_his]').val();
      var oiname = classification == "new" ? $("#ddl_type :selected").text() : $("#ddl_income_type :selected").text();
      if (employee > 0 && employeeid > 0 && incomeid != 0 && wh > 0 && nt > 0 && taxable > 0) {
          var row = $("<tr> " +
                            "<td style='display:none;'><span class='otherincomid'>" + incomeid_val + "</span></td> " +
                            "<td style='display:none;'><span class='empid'>" +  employeeid_val + "</span></td> " +
                            "<td><span class='emp_name'>" +  empname + "</span></td> " +
                            "<td><span class='OtherIncome'>" +  oiname.replace("(","-").replace(")","") + "</span></td> " +
                            "<td><span class='Amount'>-</span></td> " +
                            "<td><span class='total_hours_worked'>" +  wh_val.replace(/,/g, '') + "</span></td> " +
                            "<td><span class='amt_to_bepaid'>" + nt_val.replace(/,/g, '') + "</span></td> " +
                            "<td><span class='amt_to_be_tax'>" + taxable_val.replace(/,/g, '') + "</span></td> " +
                            "<td><input type='checkbox' name='record'></td> " +
                         "</tr>");
       
          if (classification == "new") {
              $('[id$=tbl]').find('tbody').append(row);
          }
          if (classification == "his") {
              $.ajax({
                  type: "post",
                  contentType: "application/json; charset=utf-8",
                  url: "content/payroll/transotherincome.aspx/save_data",
                  data: "{'empid':'" + employeeid_val + "','incomeid':'" + incomeid_val + "','Amount':'" + amount_val + "','wh':'" + wh_val + "','nt':'" + nt_val + "','taxable':'" + taxable_val + "','transid':'" + trnsid + "'}",
                  dataType: "json",
                  success: function (rtn) {
                    
                      if (rtn.d == "Successfully Save") {
                          $('[id$=tbl_det]').find('tbody').append(row);
                      }
                    
                  },
                  error: function (result) {
                      alert(result.responseText);
                  }
              });
              
              console.log('his');
              //something triger to database
              //then after execute create element tr td
          }
      }
      else
      {console.log('else');}
  }
  function delete_rows(classification) {
      $("table tbody").find('input[name="record"]').each(function () {
          if ($(this).is(":checked")) {
              var remove=  $(this);
              if (classification == "new") {
                  $(remove).parents("tr").remove();
              }
              if (classification == "his") {
                  $.ajax({
                      type: "post",
                      contentType: "application/json; charset=utf-8",
                      url: "content/payroll/transotherincome.aspx/delete_perline",
                      data: "{'rowid':'" + $(this).attr('id') + "'}",
                      dataType: "json",
                      success: function (rtn) {
                          
                          if (rtn.d == "Success") {
                              $(remove).parents("tr").remove();
                          }
                          console.log(rtn.d);
                      },
                      error: function (result) {
                          alert(result.responseText);
                      }
                  });
              }
          }
      });
  }



  function input_work_hrs(classification) {
      var empid = $('[id$=lbl_bals]').val();
      var incomeid = classification == "history" ? $("#ddl_income_type :selected").val() : $("#ddl_type :selected").val();
      var wh = classification == "history" ? $('[id$=work_hrs_his]').val() : $('[id$=work_hrs]').val();
    
      $.ajax({
          type: "post",
          contentType: "application/json; charset=utf-8",
          url: "content/payroll/transotherincome.aspx/computedata",
          data: "{'empid':'" + empid + "','incomeid':'" + incomeid + "','wh':'" + wh + "'}",
          dataType: "json",
          success: function (rtn) {
              var data = rtn.d;
              $('.ntt').val(data[0]);
              $('.tax').val(data[1]);
          },
          Error: function (result) {
              alert(result.responseText);
          }
      });
  }
  function save_data() {

      $("#load").removeClass("hide");
      $("#loader").removeClass("hide");
      var pg = $("#ddl_payroll_group :selected").val();
      var periodid = $("#ddl_pg :selected").val();
      var remarks = $('[id$=compose-textarea]').val();
      var url = "content/payroll/transotherincome.aspx/savedata_trans";
      var ddata = "{'pg':'" + pg + "','periodid':'" + periodid + "','remarks':'" + remarks + "'}";
      var transid=0;
      $.ajax({
          type: "post",
          contentType: "application/json; charset=utf-8",
          url: url,
          data: ddata,
          dataType: "json",
          success: function (rtn) {
		console.log(rtn.d);
              if (rtn.d != 0) {
                  transactionline(rtn.d);
                  //$('[id$=btn_success]').click();
                 
              }
              else
                  alert("This transaction is already exist!");
          },
         
          error: function (result) {
              alert(result.responseText);
          }
      });
  }
  function transactionline(transid) {
      console.log('test');

      var rownum = $('[id$=tbl] tbody').find('tr').length;
      console.log(rownum);

      $('[id$=tbl] tbody').find('tr').each(function () {
          var incomeid = $(this).find('.otherincomid').html();
          var empid = $(this).find('.empid').html();
          var Amount = $(this).find('.Amount').html().replace(/,/g, '');
          var wh = $(this).find('.total_hours_worked').html().replace(/,/g, '');
          var nt = $(this).find('.amt_to_bepaid').html().replace(/,/g, '');
          var taxable = $(this).find('.amt_to_be_tax').html().replace(/,/g, '');
          $.ajax({
              type: "post",
              contentType: "application/json; charset=utf-8",
              url: "content/payroll/transotherincome.aspx/save_data",
              data: "{'empid':'" + empid + "','incomeid':'" + incomeid + "','Amount':'" + Amount + "','wh':'" + wh + "','nt':'" + nt + "','taxable':'" + taxable + "','transid':'" + transid + "'}",
              dataType: "json",
              success: function (rtn) {
                  var finishnum = rtn.d.toString();
                      $("#loadersd").html("Please Wait..." + finishnum + "/" + rownum);
                      if (finishnum == rownum) {
                          $('[id$=tbl]').find('tbody').empty();
                          $("#ddl_payroll_group")[0].selectedIndex = 0;
                          $("#ddl_pg")[0].selectedIndex = 0;
                          alert('Successfully Saved!');
                          window.location = "transotherincome";
                  }

              },
              error: function (result) {
                  alert(result.responseText);
              }
          });
      });
  }

  function approve() {
      var txt_trn_id = $('[id$=txt_trn_id]').val();
      $.ajax({
          type: "post",
          contentType: "application/json; charset=utf-8",
          url: "content/payroll/transotherincome.aspx/approve_trans",
          data: "{'trnid':'" + txt_trn_id + "'}",
          dataType: "json",
          success: function (rtn) {
              $('[id$=err_close]').click();
              window.location.href = "transotherincome";
          },
          error: function (result) {
              alert(result.responseText);
          }
      });
  }
 
  function click_approved(id) 
  {
      $('[id$=txt_trn_id]').val(id);
  }

  function view_det(trnid) {
     
      $.ajax({
          type: "POST",
          contentType: "application/json; charset=utf-8",
          url: "content/payroll/transotherincome.aspx/view_det",
          data: "{'trnid':'" + trnid + "'}",
          dataType: "json",
          success: function (rtn) {
              var data = rtn.d;
              $('[id$=lbl_pg]').text(data[0].pgname.toString());
              $('[id$=lbl_pr]').text(data[0].range.toString());
              $('[id$=lbl_entry]').text(data[0].dateentry.toString());
              $('[id$=lbl_rem_disp]').text(data[0].remarks.toString());
              console.log(data[0].pgname.toString());
              $('[id$=tbl_det]').find('tbody').empty();



              for (var i = 0; i < data.length; i++) {
                  var thw = data[i].thw.lenght > 0 ? data[i].thw : "0.00";

                  var row = $("<tr> " +
                                    "<td style='display:none;'><span class='trn_id_line'>" + data[i].trn_id_line + "</span></td> " +
                                    "<td style='display:none;'><span class='otherincomid'>" + data[i].OtherIncome_id + "</span></td> " +
                                    "<td style='display:none;'><span class='empid'>" + data[i].Emp_id + "</span></td> " +
                                    "<td><span class='emp_name'>" + data[i].emp_name + "</span></td> " +
                                    "<td><span class='OtherIncome'>" + data[i].OtherIncome + "</span></td> " +
                                    "<td><span class='Amount'>" + data[i].Amount + "</span></td> " +
                                    "<td><span class='total_hours_worked'>" + thw + "</span></td> " +
                                    "<td><span class='amt_to_bepaid'>" + data[i].amt_to_bepaid + "</span></td> " +
                                    "<td><span class='amt_to_be_tax'>" + data[i].Amt_to_be_tax + "</span></td> " +
                                    "<td><input type='checkbox' id=" + data[i].trn_id_line + " name='record'></td> " +
                                "</tr>");
                  $('[id$=tbl_det]').find('tbody').append(row);
              }
          },
          error: function (result) {
              alert(result.responseText);
          }
      });
  }

  //load excel
  function UploadProcess() {
      //Reference the FileUpload element.
      var fileUpload = document.getElementById("file_data");
     
      //Validate whether File is valid Excel file.
      var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.xls|.xlsx)$/;
      if (regex.test(fileUpload.value.toLowerCase())) {
          if (typeof (FileReader) != "undefined") {
              var reader = new FileReader();

              //For Browsers other than IE.
              if (reader.readAsBinaryString) {
                  reader.onload = function (e) {
                      GetTableFromExcel(e.target.result);
                  };
                  reader.readAsBinaryString(fileUpload.files[0]);
              } else {
                  //For IE Browser.
                  reader.onload = function (e) {
                      var data = "";
                      var bytes = new Uint8Array(e.target.result);
                      for (var i = 0; i < bytes.byteLength; i++) {
                          data += String.fromCharCode(bytes[i]);
                      }
                      GetTableFromExcel(data);
                  };
                  reader.readAsArrayBuffer(fileUpload.files[0]);
              }
          } else {
              alert("This browser does not support HTML5.");
          }
      } else {
          alert("Please upload a valid Excel file.");
      }
  };
  function GetTableFromExcel(data) {
      //Read the Excel File data in binary
      var workbook = XLSX.read(data, {
          type: 'binary'
      });

      //get the name of First Sheet.
      var Sheet = workbook.SheetNames[0];

      //Read all rows from First Sheet into an JSON array.
      var excelRows = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[Sheet]);
      var incometype = $("#ddl_income_type :selected").val();
      var txt_trn_id = $('[id$=txt_trn_id]').val();
      upload(excelRows, incometype, txt_trn_id);

  };



  function upload(excelRows, incometype,txt_trn_id) {
      $.ajax({
          type: "POST",
          contentType: "application/json; charset=utf-8",
          url: "content/payroll/transotherincome.aspx/loadexcel",
          data: "{'arr':'" + JSON.stringify(excelRows) + "','incomtype':'" + incometype + "','transid':'" + txt_trn_id + "'}",
          dataType: "json",
          success: function (rtn) {
              var data = rtn.d;
              console.log(data);
              for (var i = 0; i < data.length; i++) {
                  console.log(data[i].taxable_amt);
                  var row = $("<tr> " +
                                    "<td style='display:none;'><span class='trn_id_line'>" + data[i].trn_id_line + "</span></td> " +
                                    "<td style='display:none;'><span class='otherincomid'>" + data[i].PayOtherIncome_id + "</span></td> " +
                                    "<td style='display:none;'><span class='empid'>" + data[i].Emp_id + "</span></td> " +
                                    "<td><span class='emp_name'>" + data[i].Emp_Name + "</span></td> " +
                                    "<td><span class='OtherIncome'>" + data[i].incometypename + "</span></td> " +
                                    "<td><span class='Amount'>" + data[i].allowed_amt + "</span></td> " +
                                    "<td><span class='total_hours_worked'>" + data[i].worked_hrs + "</span></td> " +
                                    "<td><span class='amt_to_bepaid'>" + data[i].nontaxable_amt + "</span></td> " +
                                    "<td><span class='amt_to_be_tax'>" + data[i].taxable_amt + "</span></td> " +
                                    "<td><input type='checkbox' id=" + data[i].trn_id_line + " name='record'></td> " +
                                "</tr>");
                  $('[id$=tbl_det]').find('tbody').append(row);
              }
          },
          error: function (result) {
              alert(result.responseText);
          }
      });
  }



  //procpay page
  function click_compute(clss) {
      var pg = $('[id$=ddl_pg_tmp]').val();
      var yyyy = $('[id$=ddl_yyyy_tmp]').val();
      
      var clss = $('[id$=label_]').text();
      var per = clss=="Bonus"?$('[id$=ddl_bonus_percentage]').val():0;

      var param = { pg: pg, yyyy: yyyy,per:per,clss:clss };
      console.log(param);
      $.ajax({
          type: "POST",
          contentType: "application/json; charset=utf-8",
          url: "content/payroll/proccesspayroll.aspx/compute",
          data: JSON.stringify(param),
          dataType: "json",
          success: function (rtn) {
              var data = rtn.d;
              var tbl = $('[id$=example1]').find('tbody');
              tbl.empty();
              $.each(data, function (index) {
                  var tr = "<tr> " +
                     "<td style='display:none;'> " + data[index].ID + "</td> " +
                     "<td> " + data[index].idnumber + "</td> " +
                     "<td> " + data[index].Name + "</td> " +
                     "<td> " + data[index].DateHired + "</td> " +
                     "<td> " + data[index].monthlyrate + "</td> " +
                     "<td> " + data[index].total + "</td> " +
                     "</tr>";
                  tbl.append(tr);
              });
              $('[id$=save_data]').attr('style', 'display:block; float:right;')
          },
          error: function (result) {
              alert(result.responseText);
          }
      })
  };
  function save_data_thirteen() {
      var pg = $('[id$=ddl_pg_tmp]').val();
      var yyyy = $('[id$=ddl_yyyy_tmp]').val();
      var clasification = $('[id$=label_]').text();
      var param = { pg: pg, yyyy: yyyy, clasification: clasification };

      $.ajax({
          type: "POST",
          contentType: "application/json; charset=utf-8",
          url: "content/payroll/proccesspayroll.aspx/save_data_thirteen",
          data: JSON.stringify(param),
          dataType: "json",
          beforeSend: function () {
              $('[id$=load]').removeClass("hide");
              $('[id$=loader]').removeClass("hide");
          },
          success: function (rtn) {
              if (rtn.d == "Success") {
                  if (clasification == "Thirteen") {
                      get_tmp();
                  }
                  else if (clasification == "Fourteen") {
                      get_fmp();
                  }
                  else {
                      get_bonus();
                  }
                  $('[id$=close_thirteen]').click();
                  $('[id$=load]').addClass("hide");
                  $('[id$=loader]').addClass("hide");
                  alert('Successfully Save.');
              }
              else {
                  $('[id$=load]').addClass("hide");
                  $('[id$=loader]').addClass("hide");
                  alert('Data is already exist!');
              }
          },
          error: function (result) {
              alert(result.responseText);
          }
      })
  };

  function get_tmp() {
      var param = {};
      $.ajax({
          type: "Post",
          contentType: "application/json; charset=utf-8",
          url: "content/payroll/proccesspayroll.aspx/view_thirteen_trn",
          data: JSON.stringify(param),
          dataType: "Json",
          success: function (rtn) {
              console.log('test');
              var data = rtn.d;
              var tbl = $('[id$=tmp]').find('tbody');
              tbl.empty();
              if (data.length > 0) {
                  $.each(data, function (index) {
                      var btnclass = "";
                      if (data[index].status == "Posted")
                          btnclass = "label label-success";
                      else
                          btnclass = "label label-danger";
                      var tr = "<tr> " +
                                 "<td style='display:none;'> " + data[index].trnid + "</td> " +
                                 "<td> " + data[index].sysdate + "</td> " +
                                 "<td> " + data[index].pg + "</td> " +
                                 "<td> " + data[index].yyyy + "</td> " +
                                 "<td><button type='button' onclick='click_posting_thirteen(this)' class='" + btnclass + "'>" + data[index].status + "</button></td> " +
                                 "<td><a id='" + data[index].classs + "'  title='Details' data-toggle='modal' data-target='#modal-tmp-details' onClick='get_tmp_det(this)'  style=' font-size:14px'><i class='fa fa-list'></i></a> ";
                      if (data[index].status == "Pending") {
                          tr += "<a  title='Cancel Transaction' data-toggle='modal' data-target='#modal-error' onClick='cancel_tmp_trn(this)' style=' font-size:14px'><i class='fa fa-trash'></i></a> ";
                      }
                      tr += "</td></tr> ";
                      tbl.append(tr);
                  });
              }
              else
                  tbl.append("<tr><td colspan='5'>No Data Found!</td></tr>");
          },
          error: function () {
          }
      })
  };
  function get_fmp() {
      var param = {};
      $.ajax({
          type: "Post",
          contentType: "application/json; charset=utf-8",
          url: "content/payroll/proccesspayroll.aspx/view_fourteen_trn",
          data: JSON.stringify(param),
          dataType: "Json",
          success: function (rtn) {
              console.log('test');
              var data = rtn.d;
            
              var tbl = $('[id$=fmp]').find('tbody');
              
              tbl.empty();
              if (data.length > 0) {
                  $.each(data, function (index) {
                      var btnclass = "";
                      if (data[index].status == "Posted")
                          btnclass = "label label-success";
                      else
                          btnclass = "label label-danger";
                      var tr = "<tr> " +
                                 "<td style='display:none;'> " + data[index].trnid + "</td> " +
                                 "<td> " + data[index].sysdate + "</td> " +
                                 "<td> " + data[index].pg + "</td> " +
                                 "<td> " + data[index].yyyy + "</td> " +
                                 "<td><button type='button' onclick='click_posting_thirteen(this)' class='" + btnclass + "'>" + data[index].status + "</button></td> " +
                                 "<td><a id='" + data[index].classs + "' title='Details' data-toggle='modal' data-target='#modal-tmp-details' onClick='get_tmp_det(this)'  style=' font-size:14px'><i class='fa fa-list'></i></a> ";
                      if (data[index].status == "Pending") {
                          tr += "<a  title='Cancel Transaction' data-toggle='modal' data-target='#modal-error' onClick='cancel_tmp_trn(this)' style=' font-size:14px'><i class='fa fa-trash'></i></a> ";
                      }
                      tr += "</td></tr> ";
                      tbl.append(tr);
                  });
              }
              else
                  tbl.append("<tr><td colspan='5'>No Data Found!</td></tr>");
          },
          error: function () {
          }
      })
  };

  function get_bonus() {
      var param = {};
      $.ajax({
          type: "Post",
          contentType: "application/json; charset=utf-8",
          url: "content/payroll/proccesspayroll.aspx/view_bonus_trn",
          data: JSON.stringify(param),
          dataType: "Json",
          success: function (rtn) {
              console.log('test');
              var data = rtn.d;

              var tbl = $('[id$=bonus]').find('tbody');

              tbl.empty();
              if (data.length > 0) {
                  $.each(data, function (index) {
                      var btnclass = "";
                      if (data[index].status == "Posted")
                          btnclass = "label label-success";
                      else
                          btnclass = "label label-danger";
                      var tr = "<tr> " +
                                 "<td style='display:none;'> " + data[index].trnid + "</td> " +
                                 "<td> " + data[index].sysdate + "</td> " +
                                 "<td> " + data[index].pg + "</td> " +
                                 "<td> " + data[index].yyyy + "</td> " +
                                 "<td><button type='button' onclick='click_posting_thirteen(this)' class='" + btnclass + "'>" + data[index].status + "</button></td> " +
                                 "<td><a id='" + data[index].classs + "' title='Details' data-toggle='modal' data-target='#modal-tmp-details' onClick='get_tmp_det(this)'  style=' font-size:14px'><i class='fa fa-list'></i></a> ";
                                if (data[index].status == "Pending") 
                                {
                                    tr += "<a  title='Cancel Transaction' data-toggle='modal' data-target='#modal-error' onClick='cancel_tmp_trn(this)' style=' font-size:14px'><i class='fa fa-trash'></i></a> ";
                                }
                                tr += "</td></tr> ";
                      tbl.append(tr);
                  });
              }
              else
                  tbl.append("<tr><td colspan='5'>No Data Found!</td></tr>");
          },
          error: function () {
          }
      })
  };
  function get_tmp_det(el) {
      var el_id = $(el).attr("id");
      var current = $(el).closest("tr");
      var trnid = current.find("td:eq(0)").text();
      var sysdate = current.find("td:eq(1)").text();
      var pg = current.find("td:eq(2)").text();
      var year = current.find("td:eq(3)").text();
    //  $('[id$=hdn_class]').val(el_id);
     
  
      $('[id$=pg]').text(pg);
      $('[id$=yyyy]').text(year);
      $('[id$=di]').text(sysdate);

      var param={trnid:trnid};
      $.ajax({
          type: "Post",
          contentType: " application/json; charset=utf-8",
          url: "content/payroll/proccesspayroll.aspx/view_thirteen_trn_details",
          data: JSON.stringify(param),
          datatype: "json",
          success: function (rtn) {
              var data = rtn.d;
              var tbl = $('[id$=tmp_det]').find("tbody");
              tbl.empty();
              $.each(data, function (index) {
                  var tr = "<tr> " +
                                 "<td style='display:none;'> " + data[index].rowid + "</td> " +
                                 "<td style='display:none;'> " + data[index].trnid + "</td> " +
                                 "<td> " + data[index].idnumber + "</td> " +
                                 "<td> " + data[index].fullname + "</td> " +
                                 "<td> " + data[index].datehired + "</td> " +
                                 "<td> " + data[index].monthly + "</td> " +
                                 "<td> " + data[index].total + "</td> " +
                                 "<td> " + data[index].taxable + "</td> " +
                                 "<td> " + data[index].nontaxable + "</td> " +
                                 "<td><a  title='Print Slip' href='otherpyment?key=" + data[index].idnumber + "&trn=" + data[index].trnid + "&b=single&cls=" + el_id + "' target='_new'  style=' font-size:14px'><i class='fa fa-print'></i></a> ";
                            "</tr>";
                  tbl.append(tr);
              })
          },
          error: function () { }
      })
  };
  

  function cancel_tmp_trn(el) {
      var current = $(el).closest("tr");
      var trnid = current.find("td:eq(0)").text()
      $('[id$=hdn_trnid]').val(trnid);
      console.log("cancelled");
  };

  function cotinue_cancelled() {
      var trnid = $('[id$=hdn_trnid]').val();
      var param = { trnid: trnid };
      console.log(param);
      $.ajax({
          type: "Post",
          contentType: " application/json; charset=utf-8",
          url: "content/payroll/proccesspayroll.aspx/cancel_tmp_trn",
          data: JSON.stringify(param),
          datatype: "json",
          success: function (rtn) {
              console.log("continue");
              if (rtn.d == "Success") {
                  get_tmp();
                  get_fmp();
                  get_bonus();
                  $('[id$=err_close]').click();
                  alert("Successfully Changed!");
              }
          },
          Error: function (rtn) {
              alert(rtn.responseText);
          }
      })
  };

 

  
  