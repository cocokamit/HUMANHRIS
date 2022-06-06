function check(elem) {
    vva = elem.value;
    vva = vva.split(",").join("");
    if (!IsNumerica(vva)) {
        elem.value = vva.substring(0, vva.length - 1);
    }
    else {
        elem.value = addCommas(vva);
    }
}

function check_wo_comma(elem) {
    vva = elem.value;
    vva = vva.split(",").join("");
    if (!IsNumerica(vva)) {
        elem.value = vva.substring(0, vva.length - 1);
    }
    else {
        elem.value = vva;
    }
}

function IsNumerica(input) {
    return (input - 0) == input && (input + '').replace(/^\s+|\s+$/g, "").length > 0;
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

function preventBack() {
    window.history.forward();
}