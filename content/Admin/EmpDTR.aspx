<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmpDTR.aspx.cs" Inherits="content_Admin_EmpDTR"
    MasterPageFile="~/content/MasterPageNew.master" MaintainScrollPositionOnPostback="true" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="head_dtr">
    <script src="script/datepicker/commonJScript.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-1.8.3.js" type="text/javascript"></script>
    <script src="script/datepicker/jquery-ui.js" type="text/javascript"></script>
    <link href="script/datepicker/jquery-ui-1.8.16.custom.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(function () {
            $("#Button2").trigger("click");
            console.log("asdasdsa");
        });
        jQuery.noConflict();
        (function ($) {
            $(function () {
                $(".txt_f").datepicker();
                $(".txt_t").datepicker();
                $(".datee").datepicker({ changeMonth: true,
                    yearRange: "s-100:+0",
                    changeYear: true
                });
            });
        })(jQuery);
    </script>
    <script type="text/javascript" src="script/autocomplete/jquery.min.js"></script>
    <script src="script/autocomplete/jquery-ui.js" type="text/javascript"></script>
    <script type="text/javascript">


        //        function dtrfunction() {
        //            var empidss = $("[id$=hfempid]").val();
        //            var cutoffspans = $("[id$=ddl_pay_range]").val();
        //            var txt_froms = $("[id$=txt_from]").val();
        //            var txt_tos = $("[id$=txt_to]").val();

        //            $.ajax({
        //                type: "POST",
        //                contentType: "application/json;",
        //                url: "content/Admin/EmpDTR.aspx/disps",
        //                data: JSON.stringify({ hfempid: empidss, cutoffspan: cutoffspans, txt_from: txt_froms, txt_to: txt_tos }),
        //                dataType: "json",
        //                success: function (data) {
        //                    $("[id$=btnSample]").click();
        //                },
        //                error: function (result) {
        //                    alert(result.responseText);
        //                }
        //            });
        //        }

        function checkboxradio(evs) {
            //            $(".chb").prop('checked', false);
            //            $(evs).prop('checked', true);
            //            $('#tblItems').find('input[type="checkbox"]:checked').each(function () {
            //            });

            console.log($("#HiddenField1").val());
            console.log($("#HiddenField2").val());
            var offsethrs = $("#HiddenField2").val();
            var excesstotal = "";
            var excesstemp = "0";
            var d = "0";

            //            if ($("#hfmax").val() == "--") {
            //                evs.setAttribute("max", "" + offsethrs);
            //            } else {
            //                evs.setAttribute("max", "" + $("#hfmax").val());
            //            }

            $('#tblItems').find('tr').each(function () {
                var row = $(this);
                if (!isNaN(row.find('input[type="number"]').val()) && row.find('input[type="number"]').val() != "" && row.find('input[type="number"]').val() != "0") {
                    excesstotal = excesstotal + row.find('input[type="number"]').val() + ",";
                    console.log(excesstotal);
                    //                  
                    //                  console.log("excesstemp/add: " + excesstemp + " + " + row.find('input[type="number"]').val());  
                    excesstemp = parseInt(excesstemp) + parseInt(row.find('input[type="number"]').val());
                    //                  d = parseInt(offsethrs)-parseInt(excesstemp);
                    //                  console.log("offsethrs/excesstemp: " + offsethrs + " - " + excesstemp);
                    ////                evs.setAttribute("max", "" + d);

                    $("#HiddenField1").val(excesstotal);
                }
            });

            $("#hfmax").val(excesstemp);
            console.log("max: " + $("#hfmax").val());
        }


        function closer() {
            console.log("asdasda");

            var tBody = $("#tblItems > TBODY")[0];
            var new_tbody = document.createElement('tbody');
            tBody.parentNode.replaceChild(new_tbody, tBody);

            $("#modal_os").hide();
        }

        function ossaver() {
            var excesstotal = "";
            $('#tblItems').find('tr').each(function () {
                var row = $(this);
                if (!isNaN(row.find('input[type="number"]').val()) && row.find('input[type="number"]').val() != "" && row.find('input[type="number"]').val() != "0") {
                    excesstotal = excesstotal + row.find('.txtdate').text() + ",";
                    console.log(excesstotal);
                    $("#HiddenDate3").val(excesstotal);
                }
            });
            var evs = $("#HiddenRow3").val();

            $('#grid_item').find('tr').each(function () {
                var row = $(this);
                if (row.find('#txtdate').text() == evs) {
                    console.log(row.find('#txtdate').text());
                    var offsethrs = $("#HiddenField2").val();

                    if ($("#hfmax").val() <= offsethrs) {
                        if ((parseInt(offsethrs) + parseInt($("#hfoffset").val())) >= (parseInt($("#hfmax").val()) + parseInt(row.find("#setoslb").text()))) {
                            row.find("#setoslb").text(parseInt($("#hfmax").val()) + parseInt(row.find("#setoslb").text()) + ".00");
                            console.log("if");
                            console.log((parseInt(offsethrs) + parseInt($("#hfoffset").val())));
                            console.log((parseInt($("#hfmax").val()) + parseInt(row.find("#setoslb").text())));
                        }
                        else {
                            row.find("#setoslb").text((parseInt(offsethrs) + parseInt($("#hfoffset").val())) + ".00");
                            console.log("else");
                        }
                    }
                    else {
                        row.find("#setoslb").text((parseInt(offsethrs) + parseInt($("#hfoffset").val())) + ".00");
                    }
                }
            });

            var tBody = $("#tblItems > TBODY")[0];
            var new_tbody = document.createElement('tbody');
            tBody.parentNode.replaceChild(new_tbody, tBody);

            console.log("dates: " + $("#HiddenDate3").val());
            console.log("excesses: " + $("#HiddenField1").val());
            console.log("offsets: " + $("#HiddenField2").val());

            $("#modal_os").hide();
        }

        function searchos(evs) {
            var row = evs.parentNode.parentNode;

            //            var ifos = evs.parentNode.parentNode.cells[36].innerHTML;

            //            if (parseFloat(ifos) <= 0) {
            //                alert("No 'inadequate' hours to perform offset.");
            //            }
            //            else {
            var empids = $("[id$=hfempid]").val();
            var timeiner1 = $("[id$=txtin1date]", row).val() + " " + $("[id$=txtin1time]", row).val();
            var timeouter1 = $("[id$=txtout2date]", row).val() + " " + $("[id$=txtout2time]", row).val();
            var absentor = $("[id$=ddl_absent]", row).val();
            var datetimer = $("[id$=txtdate]", row).text();
            var oss = $("[id$=setoslb]", row);
            var utt = evs.parentNode.parentNode.cells[24].innerHTML;

            $("#HiddenRow3").val(datetimer);
            console.log(datetimer);

            console.log("empid: " + empids);
            console.log("timein: " + timeiner1);
            console.log("timeout: " + timeouter1);
            console.log("absent: " + absentor);
            console.log("datetime: " + datetimer);
            console.log("offset: " + oss);
            console.log("undertime: " + utt);

            if ($("[id$=txtin1time]", row).val() != "" && $("[id$=txtout2time]", row).val() != "") {

                $.ajax({
                    type: "POST",
                    contentType: "application/json;",
                    url: "content/Admin/EmpDTR.aspx/GetOffset",
                    data: JSON.stringify({ empid: empids, ut: utt, aw: absentor, datepoint: datetimer, timeout: timeouter1, timein: timeiner1 }),
                    dataType: "json",
                    success: function (data) {

                        var tBody = $("#tblItems > TBODY")[0];

                        if (data.d.length > 0) {
                            for (var i = 0; i < data.d.length; i++) {
                                //Add Row.
                                var row = tBody.insertRow(-1);

                                var cell = $(row.insertCell(-1));
                                cell.html("<label class='txtdate'>" + data.d[i].date + "</label>");

                                cell = $(row.insertCell(-1));
                                cell.html("<label class='txtexcess'>" + data.d[i].excess + "</label>");

                                cell = $(row.insertCell(-1));
                                cell.html("<input type='number' name='a" + i + "' text='0' step='1' min='0' max='" + data.d[i].excess + "' onchange='checkboxradio(this)' class='chb' style='width:50px;' />");

                                document.getElementById("txt_off_hrs").value = data.d[i].oshrs;
                                $("#HiddenField2").val(data.d[i].oshrs);
                                $("#HiddenField1").val("");
                                $("#HiddenDate3").val("");
                                console.log($("#HiddenField2").val());

                                console.log(data.d[i].date);
                                console.log(data.d[i].excess);

                                console.log(data.d[i].oshrs);
                            }
                        }
                        console.log(data.d);
                        $("#modal_os").show();
                    },
                    error: function (result) {
                        alert(result.responseText);

                        location.reload();
                    }
                });
            } else {
                alert("Please fill up the complete timein and timeout before handling the offset.");
            }
            //}
        }


        function savetxt(evs) {

            var row = evs.parentNode.parentNode.parentNode.parentNode;

            var empids = $("[id$=hfempid]").val();
            var sc = $("[id$=ddl_shiftcode]", row).val();
            var timeiner1 = $("[id$=txtin1date]", row).val() + " " + $("[id$=txtin1time]", row).val();
            var timeouter1 = $("[id$=txtout2date]", row).val() + " " + $("[id$=txtout2time]", row).val();
            var absentor = $("[id$=ddl_absent]", row).val();
            var datetimer = $("[id$=txtdate]", row).text();
            var oss = $("[id$=setoslb]", row).text();
            var ott = $("[id$=setot]", row).val();
            var otnn = $("[id$=setotn]", row).val();
            var latee = evs.parentNode.parentNode.parentNode.parentNode.cells[23].innerHTML;
            var utt = evs.parentNode.parentNode.parentNode.parentNode.cells[24].innerHTML;
            var remark = $("[id$=txtremarks]", row).val();
            var nights = evs.parentNode.parentNode.parentNode.parentNode.cells[18].innerHTML;
            var reghrss = evs.parentNode.parentNode.parentNode.parentNode.cells[17].innerHTML;
            var finalouts = evs.parentNode.parentNode.parentNode.parentNode.cells[30].innerHTML;
            var osdate = $("[id$=HiddenDate3]").val();
            var excess = $("[id$=HiddenField1]").val();
            var offsethrs = $("[id$=HiddenField2]").val();

            console.log("---------------------------------------------------- ");
            console.log("id: " + empids);
            console.log("shiftcode: " + sc);
            console.log("in: " + timeiner1);
            console.log("out: " + timeouter1);
            console.log("absent: " + absentor);
            console.log("date: " + datetimer);
            console.log("offset: " + oss);
            console.log("overtime: " + ott);
            console.log("overtimenight: " + otnn);
            console.log("late: " + latee);
            console.log("undertime: " + utt);
            console.log("remark: " + remark);
            console.log("nighthr: " + nights);
            console.log("reghr: " + reghrss);
            console.log("finalout: " + finalouts);
            console.log("osdate: " + osdate);
            console.log("excess: " + excess);

            console.log($("#hfshiftcode").val());
            console.log(($("[id$=hfindate]").val() + " " + $("[id$=hfintime]").val()));
            console.log(($("[id$=hfoutdate]").val() + " " + $("[id$=hfouttime]").val()));
            console.log($("#hfoffset").val());
            console.log($("#hfot").val());
            console.log($("#hfotn").val());

            if (sc == $("#hfshiftcode").val()) {
                sc = "";
            }
            if (timeiner1 == ($("[id$=hfindate]").val() + " " + $("[id$=hfintime]").val()) && timeouter1 == ($("[id$=hfoutdate]").val() + " " + $("[id$=hfouttime]").val())) {
                if (ott != $("#hfot").val() || otnn != $("#hfotn").val()) {
                    timeiner1 = "";
                }
                else {
                    timeiner1 = "";
                    timeouter1 = "";
                }
            }

            if (oss == $("#hfoffset").val()) {
                oss = "";
            }
            if (ott == $("#hfot").val() && otnn == $("#hfotn").val()) {
                ott = "";
                otnn = "";
            }


            console.log("shiftcode: " + sc);
            console.log("in: " + timeiner1);
            console.log("out: " + timeouter1);
            console.log("offset: " + oss);
            console.log("overtime: " + ott);
            console.log("overtimenight: " + otnn);

            if (confirm("Are you sure you want to update this data?")) {

                if ($("[id$=txtin1time]", row).val() != "" && $("[id$=txtout2time]", row).val() != "") {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json;",
                        url: "content/Admin/EmpDTR.aspx/SaveInfo",
                        data: JSON.stringify({ empid: empids, shiftc: sc, timein1: timeiner1, timeout1: timeouter1, absenter: absentor, datepoint: datetimer, os: oss, ot: ott, otn: otnn, late: latee, ut: utt, remarks: remark, reghrs: reghrss, night: nights, finalout: finalouts, osdates: osdate, excesses: excess, offsethr: offsethrs }),
                        dataType: "json",
                        success: function (data) {

                            $("[id$=Button4]", row).click();
                            window.location.href = window.location.href;

                        },
                        error: function (result) {
                            alert(result.responseText);
                        }
                    });
                }
                else {

                    var cutoffspan = $("[id$=ddl_pay_range]").val();

                    $.ajax({
                        type: "POST",
                        contentType: "application/json;",
                        url: "content/Admin/EmpDTR.aspx/IfRD",
                        data: JSON.stringify({ empid: empids, ff: cutoffspan, shiftc: sc }),
                        dataType: "json",
                        success: function(data) {
                            if (data.d.toString() == "yes") {

                                $.ajax({
                                    type: "POST",
                                    contentType: "application/json;",
                                    url: "content/Admin/EmpDTR.aspx/SaveSC",
                                    data: JSON.stringify({ empid: empids, shiftc: sc, datepoint: datetimer, remarks: remark }),
                                    dataType: "json",
                                    success: function(data) {

                                        $("[id$=Button4]", row).click();
                                        window.location.href = window.location.href;

                                    },
                                    error: function(result) {
                                        alert(result.responseText);
                                    }
                                });

                            }
                            else {
                                alert("Time in and Time out must be provided unless shiftcode is RD.");
                            }

                        },
                        error: function(result) {
                            alert(result.responseText);
                        }
                    });

                }
            }
        }

        function Calculate(evs) {
            var row = evs.parentNode.parentNode;

            var empids = $("[id$=hfempid]").val();
            var sc = $("[id$=ddl_shiftcode]", row).val();
            var cutoffspan = $("[id$=ddl_pay_range]").val();
            var timeiner1 = $("[id$=txtin1date]", row).val() + " " + $("[id$=txtin1time]", row).val();
            var timeouter1 = $("[id$=txtout2date]", row).val() + " " + $("[id$=txtout2time]", row).val();
            var absentor = $("[id$=ddl_absent]", row).val();
            var datetimer = $("[id$=txtdate]", row).text();
            var scid = $("[id$=txtscid]", row).text();

            console.log(sc);
            console.log(timeiner1+"*****");

            if ($("[id$=txtin1time]", row).val() != "" && $("[id$=txtout2time]", row).val() != "") {

                console.log("---------------------");

                console.log("id: " + empids);
                console.log("cutoff: " + cutoffspan);
                console.log("shiftcode: " + sc);
                console.log("in: " + timeiner1);
                console.log("out: " + timeouter1);
                console.log("absent: " + absentor);
                console.log("date: " + datetimer);
                console.log("scid: " + sc);

                $.ajax({
                    type: "POST",
                    contentType: "application/json;",
                    url: "content/Admin/EmpDTR.aspx/GetInfo",
                    data: JSON.stringify({ empid: empids, ff: cutoffspan, shiftc: sc, timein1: timeiner1, timeout1: timeouter1, absenter: absentor, datepoint: datetimer }),
                    dataType: "json",
                    success: function (data) {
                        var array = data.d.toString().split('~');
                        evs.parentNode.parentNode.cells[17].innerHTML = array[0].toString();
                        evs.parentNode.parentNode.cells[18].innerHTML = array[1].toString();
                        evs.parentNode.parentNode.cells[23].innerHTML = array[2].toString();
                        evs.parentNode.parentNode.cells[24].innerHTML = array[3].toString();
                        evs.parentNode.parentNode.cells[28].innerHTML = array[4].toString();

                        console.log(array[0].toString());
                        console.log(array[1].toString());
                        console.log(array[2].toString());
                        console.log(array[3].toString());
                        console.log(array[4].toString());

                    },
                    error: function (result) {
                        alert(result.responseText);
                    }
                });
                $("[id$=ddl_absent]", row).val("False");
            }
            else {

                $.ajax({
                    type: "POST",
                    contentType: "application/json;",
                    url: "content/Admin/EmpDTR.aspx/IfRD",
                    data: JSON.stringify({ empid: empids, ff: cutoffspan, shiftc: sc }),
                    dataType: "json",
                    success: function(data) {

                        if (data.d.toString() == "yes") {

                            evs.parentNode.parentNode.cells[28].innerHTML = "RD";

                            $("[id$=ddl_absent]", row).val("False");

                        }
                        else {

                            $("[id$=ddl_absent]", row).val("True");
                        }

                    },
                    error: function(result) {
                        alert(result.responseText);
                    }
                });


            }
        }

        function asd() {
            console.log("1111111");
            $('#A1').click();
            console.log($('#A1'));
        }

        function onEdit(evs) {

            if ($("#hfshiftcode").val() == "--" && $("#hfdate").val() == "--" && $("#hfindate").val() == "--" &&
            $("#hfintime").val() == "--" &&
            $("#hfoutdate").val() == "--" &&
            $("#hfouttime").val() == "--" &&
            $("#hfabsent").val() == "--" &&
            $("#hfreg").val() == "--" &&
            $("#hfnight").val() == "--" &&
            $("#hfoffset").val() == "--" &&
            $("#hfot").val() == "--" &&
            $("#hfotn").val() == "--" &&
            $("#hflate").val() == "--" &&
            $("#hfut").val() == "--" &&
            $("#hfstatus").val() == "--" &&
            $("#hfremark").val() == "--") {
                var row = evs.parentNode.parentNode.parentNode.parentNode;
                console.log(row);


                //enable controls
                $("[id$=ddl_shiftcode]", row).prop("disabled", false);
                $("[id$=txtin1date]", row).prop("disabled", false);
                $("[id$=txtin1time]", row).prop("disabled", false);
                $("[id$=txtout2date]", row).prop("disabled", false);
                $("[id$=txtout2time]", row).prop("disabled", false);
                $("[id$=ddl_absent]", row).prop("disabled", false);
                $("[id$=setoss]", row).prop("disabled", false);
                $("[id$=setot]", row).prop("disabled", false);
                $("[id$=setotn]", row).prop("disabled", false);
                $("[id$=txtremarks]", row).prop("disabled", false);

                //store temporarily 
                $("#hfshiftcode").val($("[id$=ddl_shiftcode]", row).val());
                $("#hfdate").val($("[id$=txtdate]", row).text());
                $("#hfindate").val($("[id$=txtin1date]", row).val());
                $("#hfintime").val($("[id$=txtin1time]", row).val());
                $("#hfoutdate").val($("[id$=txtout2date]", row).val());
                $("#hfouttime").val($("[id$=txtout2time]", row).val());
                $("#hfabsent").val($("[id$=ddl_absent]", row).val());
                $("#hfreg").val(evs.parentNode.parentNode.parentNode.parentNode.cells[17].innerHTML);
                $("#hfnight").val(evs.parentNode.parentNode.parentNode.parentNode.cells[18].innerHTML);
                $("#hfoffset").val($("[id$=setoslb]", row).text());
                $("#hfot").val($("[id$=setot]", row).val());
                $("#hfotn").val($("[id$=setotn]", row).val());
                $("#hflate").val(evs.parentNode.parentNode.parentNode.parentNode.cells[23].innerHTML);
                $("#hfut").val(evs.parentNode.parentNode.parentNode.parentNode.cells[24].innerHTML);
                $("#hfstatus").val($("[id$=lnk_status]", row).text());
                $("#hfremark").val($("[id$=txtremarks]", row).text());

                var todayTime = $("[id$=txtdate]", row).text().split("-")[0].split("/");

                if (todayTime[0].length == 1) {
                    todayTime[0] = "0" + todayTime[0];
                }
                if (todayTime[1].length == 1) {
                    todayTime[1] = "0" + todayTime[1];
                }

                var month = todayTime[0];

                var day = todayTime[1];

                var year = todayTime[2];
                console.log(year + "-" + month + "-" + day+"****");
                $("[id$=txtin1date]", row).val(year + "-" + month + "-" + day);
                $("[id$=txtout2date]", row).val(year + "-" + month + "-" + day);

                console.log($("[id$=setot]", row));
                console.log($("[id$=setotn]", row));

                console.log($("#hfshiftcode").val());
                console.log("asdasd" + $("#hfdate").val().split("-")[0]);
                console.log($("#hfindate").val());
                console.log($("#hfintime").val());
                console.log($("#hfoutdate").val());
                console.log($("#hfouttime").val());
                console.log($("#hfabsent").val());
                console.log($("#hfreg").val());
                console.log($("#hfnight").val());
                console.log($("#hfoffset").val());
                console.log($("#hfot").val());
                console.log($("#hfotn").val());
                console.log($("#hflate").val());
                console.log($("#hfut").val());
                console.log($("#hfstatus").val());
                console.log($("#hfremark").val());

                row.cells[0].style.backgroundColor = "aqua";

                $("#Button4", row).attr("title", "cancel");
                $("#Button4", row).removeAttr("onclick");
                $("#Button4", row).attr("onclick", "onCancel(this)");
                $("#Button4>i", row).attr("class", "fa fa-times");
                $("#btnSave", row).show();
            }
            else {
                alert("Please save or cancel the previous row you've edited before proceeding to another.");
            }
        }

        function onCancel(evs) {
            var row = evs.parentNode.parentNode.parentNode.parentNode;

            //get stored data 
            $("[id$=ddl_shiftcode]", row).val($("#hfshiftcode").val());
            $("[id$=txtdate]", row).text($("#hfdate").val());
            $("[id$=txtin1date]", row).val($("#hfindate").val());
            $("[id$=txtin1time]", row).val($("#hfintime").val());
            $("[id$=txtout2date]", row).val($("#hfoutdate").val());
            $("[id$=txtout2time]", row).val($("#hfouttime").val());
            $("[id$=ddl_absent]", row).val($("#hfabsent").val());
            $("[id$=setoslb]", row).text($("#hfoffset").val());
            $("[id$=setot]", row).val($("#hfot").val());
            $("[id$=setotn]", row).val($("#hfotn").val());
            $("[id$=lnk_status]", row).text($("#hfstatus").val());
            $("[id$=txtremarks]", row).text($("#hfremark").val());
            evs.parentNode.parentNode.parentNode.parentNode.cells[23].innerHTML = $("#hflate").val();
            evs.parentNode.parentNode.parentNode.parentNode.cells[18].innerHTML = $("#hfnight").val();
            evs.parentNode.parentNode.parentNode.parentNode.cells[17].innerHTML = $("#hfreg").val();
            evs.parentNode.parentNode.parentNode.parentNode.cells[24].innerHTML = $("#hfut").val();
            evs.parentNode.parentNode.parentNode.parentNode.cells[28].innerHTML = $("#hfstatus").val();

            //disable controls
            $("[id$=ddl_shiftcode]", row).attr("disabled", "disabled");
            $("[id$=txtin1date]", row).attr("disabled", "disabled");
            $("[id$=txtin1time]", row).attr("disabled", "disabled");
            $("[id$=txtout2date]", row).attr("disabled", "disabled");
            $("[id$=txtout2time]", row).attr("disabled", "disabled");
            $("[id$=ddl_absent]", row).attr("disabled", "disabled");
            $("[id$=setoss]", row).attr("disabled", "disabled");
            $("[id$=setot]", row).attr("disabled", "disabled");
            $("[id$=setotn]", row).attr("disabled", "disabled");
            $("[id$=txtremarks]", row).attr("disabled", "disabled");

            row.cells[0].style.backgroundColor = "white";

            $("#hfshiftcode").val("--");
            $("#hfdate").val("--");
            $("#hfindate").val("--");
            $("#hfintime").val("--");
            $("#hfoutdate").val("--");
            $("#hfouttime").val("--");
            $("#hfabsent").val("--");
            $("#hfreg").val("--");
            $("#hfnight").val("--");
            $("#hfoffset").val("--");
            $("#hfot").val("--");
            $("#hfotn").val("--");
            $("#hflate").val("--");
            $("#hfut").val("--");
            $("#hfstatus").val("--");
            $("#hfremark").val("--");

            $("#Button4", row).attr("title", "Edit");
            $("#Button4", row).removeAttr("onclick");
            $("#Button4", row).attr("onclick", "onEdit(this)");
            $("#Button4>i", row).attr("class", "fa fa-edit");
            $("#btnSave", row).hide();

        }
        function GetFormattedDate() {

            var todayTime = dateta.split("/");

            var month = todayTime[0];

            var day = todayTime[1];

            var year = todayTime[2];

            return year + "-" + month + "-" + day;

        }

        function checkreq(evs) {
            var row = evs.parentNode.parentNode.parentNode.parentNode;
            var empids = $("[id$=hfempid]").val();
            var datetimer = $("[id$=txtdate]", row).text();
            var sc = $("[id$=ddl_shiftcode]", row).val();


            $.ajax({
                type: "POST",
                contentType: "application/json;",
                url: "content/Admin/EmpDTR.aspx/GetRequests",
                data: JSON.stringify({ empid: empids, datepoint: datetimer, shiftc: sc }),
                dataType: "json",
                success: function (data) {

                    var tBody = $("#Table1 > TBODY")[0];

                    if (data.d.length > 0) {
                        for (var i = 0; i < data.d.length; i++) {
                            //Add Row.
                            var row = tBody.insertRow(-1);

                            var cell = $(row.insertCell(-1));
                            cell.html("<label>" + data.d[i].date + "</label>");

                            cell = $(row.insertCell(-1));
                            cell.html("<label>" + data.d[i].type + "</label>");

                            cell = $(row.insertCell(-1));
                            cell.html("<label>" + data.d[i].approver + "</label>");


                        }
                    }
                    console.log(data.d);

                },
                error: function (result) {
                    alert(result.responseText);
                }
            });

            $("#Div1").show();

        }

        function closediv1() {
            console.log("asdasda");

            var tBody = $("#Table1 > TBODY")[0];
            var new_tbody = document.createElement('tbody');
            tBody.parentNode.replaceChild(new_tbody, tBody);

            $("#Div1").hide();
        }


        $(document).ready(function() {
            $.noConflict();
            $(".auto").autocomplete({
                source: function(request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "content/Admin/EmpDTR.aspx/GetEmployee",
                        data: "{'term':'" + $(".auto").val() + "'}",
                        dataType: "json",
                        success: function(data) {
                            response($.map(data.d, function(item) {
                                return {
                                    label: item.split('-')[1],
                                    val: item.split('-')[0]
                                }
                            }))
                        },
                        error: function(result) {
                            alert(result.responseText);
                        }
                    });
                },
                select: function(e, i) {
                    index = $(".auto").parent().parent().index();
                    $("[id$=hfempid]").val(i.item.val);
                    $("[id$=fullnamess]").val(i.item.label);
                }
            });
        });
    </script>
    <style type="text/css">
        #grid_item tr td, #grid_item tr th, #grid_item tr td input[type=number], #grid_item tr td textarea, #grid_item tr td select
        {
            font-size: 9px;
        }
        #grid_item tr td input[type=Date], #grid_item tr td input[type=Time]
        {
            font-size: 11px;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server" ID="content_dtr">
    <section class="content-header">
    <h4>Employee DTR</h4>
    <ol class="breadcrumb">
    </ol>
</section>
    <section class="content">
    <div class="row">
        <div class="col-xs-12">
                <div class="input-group input-group-sm">
                <div class="row">
                    <div class="col-md-6">
                        <asp:DropDownList ID="ddl_pay_range" CssClass="txt_f form-control" Width="300px" OnTextChanged="btn_go" AutoPostBack="true" runat="server"></asp:DropDownList>
             
                    </div>
                      <div class="col-md-6">
                      <div class="input-group" style="width: 300px">
                    <asp:TextBox ID="tb_search" placeholder="Employee Name" Width="250px" Height="33px" CssClass="auto" runat="server"></asp:TextBox> 
                    <span class="input-group-btn">
                        <asp:Button ID="b_search" runat="server" CssClass="btn btn-primary" placeholder="Search"
                            Text="GO" OnClick="click_search" />
                    </span>
                </div>
                   </div>
               </div>
                  <asp:Button ID="Button1" runat="server" OnClick="btn_go" style=" display:none;" Text="search" CssClass="btn btn-primary"/>
                    <asp:TextBox ID="txt_from" placeholder="From" CssClass="txt_f form-control" style="float:left; width:150px; margin-right:5px; display:none; " ClientIDMode="Static" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txt_to" ClientIDMode="Static" CssClass="txt_f form-control" style="float:left; width:150px;margin-right:5px;  display:none; "  placeholder="To" runat="server" ></asp:TextBox>
                </div>
            <div class="box-body table-responsive">
      <asp:Button runat="server" ID="btnSample" Text="" style="display:none;" OnClick="btnSample_Click" />
     
                <%--<input type="button" id="Button2" aria-hidden="true" onclick="asd()" runat="server" value="<->" />--%>
           <asp:GridView ID="grid_item" runat="server" ClientIDMode="Static" OnRowDataBound="ordb" BorderColor="Turquoise"  AutoGenerateColumns="False" CssClass="table table-striped table-bordered no-margin">
            
                <Columns>
                    <%--0--%>
                    <asp:TemplateField HeaderText="Action"> 
                            <ItemTemplate>
                            <div class="row">
                         <div class="col-md-1">
                          <button type="button" id="Button4" title="Edit" onclick="onEdit(this)" ><i class="fa fa-edit"></i></button>
                         </div>
                        </div>
                         <div class="row">
                           <div class="col-md-1"> 
                          <button type="button" id="btnSave" title="Save" onclick="savetxt(this)" visible="false" style="display:none;"><i class="fa fa-save"></i></button>
                            </div>
                         </div>
                         
                           <div class="row">
                           <div class="col-md-1"> 
                             <button type="button" id="Button5" title="Check pending requests" onclick="checkreq(this)"><i class="fa fa-ellipsis-h"></i></button>
                         
                            </div>
                         </div>
                          
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--1--%>
                    <asp:BoundField DataField="employee" HeaderText="Employee" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                    <%--2--%>    
                    <asp:TemplateField HeaderText="ShiftCodes" >
                                <ItemTemplate> 
                                    <asp:DropDownList runat="server" ID="ddl_shiftcode" disabled="disabled" onchange="Calculate(this)" ClientIDMode="Static" >
                                    </asp:DropDownList> 
                                </ItemTemplate>
                    </asp:TemplateField>
                    <%--3--%>
                    <asp:BoundField DataField="ShiftCode" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                    <%--4--%>
                    <asp:TemplateField HeaderText="Date" >
                                <ItemTemplate> 
                                     <asp:Label runat="server" ID="txtdate" Text='<%# Bind("Date") %>'></asp:Label>
                                </ItemTemplate>
                    </asp:TemplateField>
                    <%--5--%>
                    <asp:BoundField DataField="daytype" HeaderText="Day Type" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <%--6--%>
                    <asp:BoundField DataField="dm" HeaderText="Day Multiplier" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <%--7--%>
                    <asp:TemplateField HeaderText="In 1" >
                                <ItemTemplate> 
                                 <asp:TextBox ID="txtin1date" runat="server" type="Date" disabled="disabled" CssClass="form-control" onchange="Calculate(this)" Text='<%# Eval("timein1").ToString()=="--"? "--": Convert.ToDateTime(Eval("timein1")).ToString("yyyy-MM-dd") %>'>'></asp:TextBox>
                                 <asp:TextBox ID="txtin1time" runat="server" type="Time" disabled="disabled" CssClass="form-control" onchange="Calculate(this)" Text='<%# Eval("timein1").ToString()=="--"? "--": Convert.ToDateTime(Eval("timein1")).ToString("HH\\:mm") %>'>'></asp:TextBox>
                                </ItemTemplate>
                    </asp:TemplateField>
                    <%--8--%>
                    <asp:TemplateField HeaderText="Out 1" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide">
                                <ItemTemplate> 
                                 <asp:TextBox ID="txtout1date" runat="server" type="Date" CssClass="form-control" Text='<%# Eval("timeout1").ToString()=="--"? "--": Convert.ToDateTime(Eval("timeout1")).ToString("yyyy-MM-dd") %>'>'></asp:TextBox>
                                 <asp:TextBox ID="txtout1time" runat="server" type="Time" CssClass="form-control" Text='<%# Eval("timeout1").ToString()=="--"? "--": Convert.ToDateTime(Eval("timeout1")).ToString("HH\\:mm") %>'>'></asp:TextBox>
                                </ItemTemplate>
                    </asp:TemplateField>
                    <%--9--%>
                    <asp:TemplateField HeaderText="In 2" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide">
                                <ItemTemplate> 
                                 <asp:TextBox ID="txtin2date" runat="server" type="Date" CssClass="form-control" Text='<%# Eval("timein2").ToString()=="--"? "--": Convert.ToDateTime(Eval("timein2")).ToString("yyyy-MM-dd") %>'>'></asp:TextBox>
                                 <asp:TextBox ID="txtin2time" runat="server" type="Time" CssClass="form-control" Text='<%# Eval("timein2").ToString()=="--"? "--": Convert.ToDateTime(Eval("timein2")).ToString("HH\\:mm") %>'>'></asp:TextBox>
                                </ItemTemplate>
                    </asp:TemplateField>
                    <%--10--%>
                    <asp:TemplateField HeaderText="Out 2" >
                                <ItemTemplate> 
                                 <asp:TextBox ID="txtout2date" runat="server" type="Date" disabled="disabled" CssClass="form-control" onchange="Calculate(this)" Text='<%# Eval("timeout2").ToString()=="--"? "--": Convert.ToDateTime(Eval("timeout2")).ToString("yyyy-MM-dd") %>'>'></asp:TextBox>
                                 <asp:TextBox ID="txtout2time" runat="server" type="Time" disabled="disabled" CssClass="form-control" onchange="Calculate(this)" Text='<%# Eval("timeout2").ToString()=="--"? "--": Convert.ToDateTime(Eval("timeout2")).ToString("HH\\:mm") %>'>'></asp:TextBox>
                                </ItemTemplate>
                    </asp:TemplateField>
                    <%--11--%>
                    <asp:BoundField DataField="olw" HeaderText="leave"/>
                    <%--12--%>
                    <asp:BoundField DataField="aw" HeaderText="Absent" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                    <%--13--%>
                    <asp:TemplateField HeaderText="Absent" >
                                <ItemTemplate> 
                                    <asp:DropDownList runat="server" ID="ddl_absent" disabled="disabled" CssClass=" Select2" onchange="Calculate(this)" ClientIDMode="Static" >
                                        <asp:ListItem Value="True">True</asp:ListItem>
                                        <asp:ListItem Value="False">False</asp:ListItem>
                                    </asp:DropDownList> 
                                </ItemTemplate>
                    </asp:TemplateField>
                    <%--14--%>
                    <asp:BoundField DataField="timeout2" HeaderText="Outs" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                    <%--15--%>
                    <asp:BoundField DataField="olh" HeaderText="On Leave Halfday" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <%--16--%>
                    <asp:BoundField DataField="ah" HeaderText="Absent Halfday"  HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                    <%--17--%>
                    <asp:BoundField DataField="reg_hr" HeaderText="REG" />
                    <%--18--%>
                    <asp:BoundField DataField="night" HeaderText="NIGHT" />
                    <%--19--%>
                     <asp:TemplateField HeaderText="OFFSET" >
                                <ItemTemplate> 
                                    <asp:Label runat="server" ID="setoslb" Text='<%# Eval("offsethrs") %>'></asp:Label>
                                    
                                    <asp:TextBox ID="setos" runat="server" TextMode="Number" Text="0" min="0" step="0.01" style="width:50px; display:none;" ></asp:TextBox>
                                      <button type="button" id="setoss" disabled="disabled" onclick="searchos(this)" runat="server" style='<%# Eval("reg_hr").ToString()=="8.00"? "display:none;":""%>' ><i class="fa fa-info"></i></button>
                                </ItemTemplate>
                    </asp:TemplateField>
                    <%--20--%>
                    <asp:TemplateField HeaderText="OT" >
                                <ItemTemplate> 
                                    <asp:TextBox ID="setot" runat="server" ClientIDMode="Static" disabled="disabled" type="number"  min="0" step="0.1" style="width:50px;" Text='<%# Eval("ot").ToString() %>'>'></asp:TextBox>
                                    </ItemTemplate>
                    </asp:TemplateField>
                    <%--21--%>
                    <asp:TemplateField HeaderText="OTN" >
                                <ItemTemplate> 
                                    <asp:TextBox ID="setotn" runat="server" ClientIDMode="Static" disabled="disabled" type="number"  min="0" step="0.1" style="width:50px;"  Text='<%# Eval("otn").ToString() %>'>' ></asp:TextBox>
                                    </ItemTemplate>
                    </asp:TemplateField>
                    <%--22--%>
                    <asp:BoundField DataField="totalhrs" HeaderText="Total" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <%--23--%>
                    <asp:BoundField DataField="late" HeaderText="LATE" />
                    <%--24--%>
                    <asp:BoundField DataField="ut" HeaderText="UT" />
                    <%--25--%>
                    <asp:BoundField DataField="nethours" HeaderText="Net HRS" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" />
                    <%--26--%>
                    <asp:BoundField DataField="shiftcodeid" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <%--27--%>
                    <asp:BoundField DataField="empid" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <%--28--%>
                    <asp:TemplateField HeaderText="Status" >
                        <ItemTemplate > 
                            <asp:LinkButton ID="lnk_status" ClientIDMode="Static" disabled="disabled" Text='<%#bind("status")%>'  style=" font-size:10px;"   runat="server"></asp:LinkButton>
                          </ItemTemplate>
                    <ItemStyle/>
                    </asp:TemplateField>
                    <%--29--%>
                    <asp:BoundField DataField="irregularity" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <%--30--%>
                    <asp:BoundField DataField="setupfinalout" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                    <%--31--%>
                    <asp:TemplateField HeaderText="scid" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide">
                                <ItemTemplate> 
                                     <asp:Label runat="server" ID="txtscid" Text='<%# Bind("shiftcodeid") %>'></asp:Label>
                                </ItemTemplate>
                    </asp:TemplateField>
                     <%--32--%>
                    <asp:TemplateField HeaderText="Remarks" >
                                <ItemTemplate> 
                                         <asp:TextBox ID="txtremarks" ClientIDMode="Static" runat="server" disabled="disabled" TextMode="MultiLine" style="min-width:200px;" CssClass="form-control"  Text='<%# Eval("remarks").ToString() %>'></asp:TextBox>
                                </ItemTemplate>
                    </asp:TemplateField>
                     <%--33--%>
                       <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide" > 
                    
                    </asp:TemplateField>
                       <%--34--%>
                    <asp:BoundField DataField="ot" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                       <%--35--%>
                    <asp:BoundField DataField="otn" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                       <%--36--%>
                    <asp:BoundField DataField="offsethrs" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                       <%--37--%>
                    <asp:BoundField DataField="remarks" HeaderStyle-CssClass="hide" ItemStyle-CssClass="hide"/>
                </Columns>
                </asp:GridView>
               
            </div>
          </div>
    </div>
    
<div id="modal_os" class="modal fade in">
    <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                        <input type="button" id="Button222" onclick="closer()" class="close" aria-hidden="true" value="&times;" />
                <h4 class="modal-title">Offset Request<asp:label ID="hdn_alocated_bal" style=" display:none;" runat="server"/></asp:label></h4>
            </div>
            <div class="modal-body">
                <%--<asp:GridView ID="grid_temp_ot"  CssClass="table table-striped table-bordered no-margin" ClientIDMode="Static" runat="server">
                </asp:GridView>--%>
                   <table id="tblItems" class="table" style="width: 100%" cellpadding="0" cellspacing="0">
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            Date
                                                        </th>
                                                        <th>
                                                            Excess Hours
                                                        </th>
                                                        <th>
                                                            
                                                        </th>
                                                       
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                                <tfoot>      </tfoot>
                                            </table>
                
            <div class="form-group">
               
                <div class="form-group">
                <input type="button" id="Button3" class="push-right"  aria-hidden="true" onclick="ossaver()" value="Confirm" />
                </div>
                <div class="form-group">
                    <label>Offset Hrs</label>
                    <asp:TextBox ID="txt_off_hrs" Enabled="false" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="lbl_off_err" runat="server" style=" display:none;" ForeColor="Red" CssClass="form-control"></asp:Label>
                </div>
            </div>
            <div class="box-footer pad"> 
            </div>
            
                </div>
        </div>
    </div>
</div>

 
<div id="Div1" class="modal fade in">
    <div class="modal-dialog">  
        <div class="modal-content">
            <div class="modal-header">
                        <input type="button" id="Button6" onclick="closediv1()" class="close" aria-hidden="true" value="&times;" />
                <h4 class="modal-title">Check Pending Requests<asp:label ID="Label1" style=" display:none;" runat="server"/></h4>
            </div>
            <div class="modal-body">
                   <table id="Table1" class="table" style="width: 100%" cellpadding="0" cellspacing="0">
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            Date
                                                        </th>
                                                        <th>
                                                            Type
                                                        </th>
                                                        <th>
                                                            Approver
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                                <tfoot>      </tfoot>
                                            </table>
                
            <div class="form-group">
               
           
            </div>
            <div class="box-footer pad"> 
            </div>
            
                </div>
        </div>
    </div>
</div>


</section>

    <asp:HiddenField ID="fullnamess" runat="server" />
    <asp:HiddenField ID="hfempid" runat="server" />
    <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HiddenField2" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HiddenDate3" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HiddenRow3" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdn_total_tempot" runat="server" />
    <asp:HiddenField ID="hdn_max_offset" runat="server" />
    <%----------------------------------------------Stored hiddenfields-------------------------------------------------------------%>
    <asp:HiddenField ID="hfshiftcode" runat="server" ClientIDMode="Static" Value="--" />
    <asp:HiddenField ID="hfdate" runat="server" ClientIDMode="Static" Value="--" />
    <asp:HiddenField ID="hfindate" runat="server" ClientIDMode="Static" Value="--" />
    <asp:HiddenField ID="hfintime" runat="server" ClientIDMode="Static" Value="--" />
    <asp:HiddenField ID="hfoutdate" runat="server" ClientIDMode="Static" Value="--" />
    <asp:HiddenField ID="hfouttime" runat="server" ClientIDMode="Static" Value="--" />
    <asp:HiddenField ID="hfabsent" runat="server" ClientIDMode="Static" Value="--" />
    <asp:HiddenField ID="hfreg" runat="server" ClientIDMode="Static" Value="--" />
    <asp:HiddenField ID="hfnight" runat="server" ClientIDMode="Static" Value="--" />
    <asp:HiddenField ID="hfoffset" runat="server" ClientIDMode="Static" Value="--" />
    <asp:HiddenField ID="hfot" runat="server" ClientIDMode="Static" Value="--" />
    <asp:HiddenField ID="hfotn" runat="server" ClientIDMode="Static" Value="--" />
    <asp:HiddenField ID="hflate" runat="server" ClientIDMode="Static" Value="--" />
    <asp:HiddenField ID="hfut" runat="server" ClientIDMode="Static" Value="--" />
    <asp:HiddenField ID="hfstatus" runat="server" ClientIDMode="Static" Value="--" />
    <asp:HiddenField ID="hfremark" runat="server" ClientIDMode="Static" Value="--" />
    <asp:HiddenField ID="hfmax" runat="server" ClientIDMode="Static" Value="--" />
</asp:Content>
<asp:Content ContentPlaceHolderID="footer" runat="server" ID="footers">
    <script type="text/javascript">


    </script>
</asp:Content>
