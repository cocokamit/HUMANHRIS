function click_done() {
    //Uploading resume
    var nme = document.getElementById("FileUpload1");
    var oFile = nme.files[0];

    if (nme.value.length == 0) {
        alert('Select file');
        nme.focus();
        return false;
    }
    else {
        if (oFile.size / 1048576 < 3) {
            document.getElementById("done").style.display = "none";
            document.getElementById("upf").style.display = "block";
            document.getElementById("fountainG").style.display = "block";
            document.getElementById("lb_upload").click();
        }
        else
            alert("file too large");
    }
}

function crop() {
    var modal = document.getElementById('myModal');
    var modalcon = document.getElementById('myModalContent');
    var modalrem = document.getElementById('modalresume');
    var setAspectRatio = document.getElementById('setAspectRatio');
    setAspectRatio.style.display = "none";
    modal.style.display = "none";
    modalcon.style.display = "none";
    modalrem.style.display = "none";
}

function upfile() {
    var modal = document.getElementById('img_container');
    var li_crop = document.getElementById('getDataURL2');
    var img_up = document.getElementById('img_up');
    modal.style.display = "block";
    li_crop.style.display = "inline-block";
    img_up.style.width = "40px";
}

function upresume() {
    //console.log("resume");
    $("#done").css("display", "block");
}

function clos() {
    var modal = document.getElementById('myModal');
    var modalcon = document.getElementById('myModalContent');
    var modalrem = document.getElementById('modalresume');
    modal.style.display = "none";
    modalcon.style.display = "none";
    modalrem.style.display = "none";
    console.log("close cropper");
    $(".profile-a").css("display", "block");
}

function modaliza(el) {
    //console.log("Show cropper");
    var modal = document.getElementById('myModal');
    var modalcon = document.getElementById('myModalContent');
    var modalrem = document.getElementById('modalresume');
    modal.style.display = "block";
    if (el == 1)
        modalcon.style.display = "block";
    else
        modalrem.style.display = "block";


    var ss = document.getElementsByClassName('cropper-container');
    $(".profile-a").css("display", "none");
}

function dss(elem) {
    $("#" + elem.id).datepicker({
        changeMonth: true,
        yearRange: "-100:+0",
        changeYear: true
    });
}
function selectchange(elem) {
    elem.style.color = "red";
    alert(elem.selectedIndex);
}