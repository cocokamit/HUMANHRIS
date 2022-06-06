
function comm(val) {

    if (NumberOnlyTextBox(val))
        document.getElementById('txt_amount').value = addCommas(val);
    else {
        alert('input numeric');
    }
}


function NumberOnlyTextBox(el) {
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el) == false)
        return false;
    else
        return true;
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
   // alert(x2 + x1);
    if (x2 == '' && x1!='')
        return x1 + ".00";
    else
        return x1 + x2;
}

function kita(elem) {
    //alert(elem)
    document.getElementById(elem).style.display = "inline";

}

var oi = 0;
var oii = 0;

function validate() {
  

    var TextBox2 = document.getElementById("ContentPlaceHolder1_TextBox2"); var errorTxt2 = document.getElementById("errorTxt2");
    var TextBox3 = document.getElementById("ContentPlaceHolder1_TextBox3"); var errorTxt3 = document.getElementById("errorTxt3");
    var TextBox4 = document.getElementById("ContentPlaceHolder1_TextBox4"); var errorTxt4 = document.getElementById("errorTxt4");
    var TextBox5 = document.getElementById("ContentPlaceHolder1_TextBox5"); var errorTxt5 = document.getElementById("errorTxt5");
    var TextBox6 = document.getElementById("ContentPlaceHolder1_TextBox6"); var errorTxt6 = document.getElementById("errorTxt6");
    var TextBox7 = document.getElementById("ContentPlaceHolder1_TextBox7"); var errorTxt7 = document.getElementById("errorTxt7");
    var TextBox8 = document.getElementById("ContentPlaceHolder1_TextBox8"); var errorTxt8 = document.getElementById("errorTxt8");

    // emptyset
    //*********************************************** emptyset ***********************************************
    if (notEmpty(TextBox2))
        ErrorChange(TextBox2, errorTxt2, "Empty Field");
    else {
        changeError(TextBox2, errorTxt2);
        oi = 0;
    }

    if (notEmpty(TextBox3))
        ErrorChange(TextBox3, errorTxt3, "Empty Field");
    else {

        if (isAlphabet(TextBox3))
            ErrorChange(TextBox3, errorTxt3, "Specify Correct Color");
        else {
            changeError(TextBox3, errorTxt3); 
        } 
    }

    if (notEmpty(TextBox4))
        ErrorChange(TextBox4, errorTxt4, "Empty Field");
    else {
        if (isAlphanumeric(TextBox4))
            ErrorChange(TextBox4, errorTxt4, "Invalid Input");
        else {
            changeError(TextBox4, errorTxt4); 
        } 
    }

    if (notEmpty(TextBox5))
        ErrorChange(TextBox5, errorTxt5, "Empty Field");
    else {
        if (isAlphanumeric(TextBox5))
            ErrorChange(TextBox5, errorTxt5, "Invalid Input");
        else {
            changeError(TextBox5, errorTxt5); 
        } 
    }


    if (notEmpty(TextBox6))
        ErrorChange(TextBox6, errorTxt6, "Empty Field");
    else {
        if (isAlphanumeric(TextBox6))
            ErrorChange(errorTxt6, errorTxt6, "Invalid Input");
        else {
            changeError(errorTxt6, errorTxt6); 
        }
    }


//    if (notEmpty(TextBox7))
//        ErrorChange(TextBox7, errorTxt7, "Empty Field");
//    else {

//        if (isAlphabet(TextBox7))
//            ErrorChange(TextBox7, errorTxt7, "Specify Correct Color");
//        else {
//            changeError(TextBox7, errorTxt7);
//        }
//    }


//    if (notEmpty(TextBox8))
//        ErrorChange(TextBox8, errorTxt8, "Empty Field");
//    else {

//        if (isAlphabet(TextBox8))
//            ErrorChange(TextBox8, errorTxt8, "Specify Correct Color");
//        else {
//            changeError(TextBox8, errorTxt8);
//        }
//    }

//    


    if (oi == 0) {

        document.getElementById("ContentPlaceHolder1_btnSave").click();

    }



 
  

}


function changeError(elem, err) {
    elem.style.border = "1px solid silver";
    err.style.display = "none";

 

}
function ErrorChange(elem, err, msg) {

    elem.style.border = "1px solid red";
    elem.style.background = "#FFF";
    err.style.display = "block";
    err.innerHTML = msg;
    oi = 1;
}

function notEmpty(elem) {
    
    if (elem.value.length == 0) {
        elem.focus(); // set the focus to this input
        return true;
    }
    else
        return false;
} 

function isNumeric(elem, helperMsg) {
    var numericExpression = /^[0-9]+$/;
    if (elem.value.match(numericExpression)) {
        return true;
    } else {
        elem.focus();
        return false;
    }
}

function isAlphabet(elem, helperMsg) {
    var alphaExp = /^[a-zA-Z]+$/;
    if (elem.value.match(alphaExp)) {
 
        return false;

    } else {
 
        elem.focus();
        return true;
    }
}

function isAlphanumeric(elem, helperMsg) {
    var alphaExp = /^[0-9a-zA-Z]+$/;
    if (elem.value.match(alphaExp)) {
 
        return false;
    } else {
       
       return true;
    }
}

function lengthRestriction(elem, min, max) {
    var uInput = elem.value;
    if (uInput.length >= min && uInput.length <= max) {
        return true;
    } else {
        alert("Please enter between " + min + " and " + max + " characters");
        elem.focus();
        return false;
    }
}

function madeSelection(elem, helperMsg) {
    if (elem.value == "Please Choose") {
        alert(helperMsg);
        elem.focus();
        return false;
    } else {
        return true;
    }
}

function emailValidator(elem, helperMsg) {
    var emailExp = /^[\w\-\.\+]+\@[a-zA-Z0-9\.\-]+\.[a-zA-z0-9]{2,4}$/;
    if (elem.value.match(emailExp)) {
        return true;
    } else {
        alert(helperMsg);
        elem.focus();
        return false;
    }
}


 