function po_total(input,x) {
    var price = document.getElementById("content_grid_item_txt_price_" + x).value.length == 0 ? 0 : document.getElementById("content_grid_item_txt_price_" + x).value;
    var qty = document.getElementById("content_grid_item_txt_quantity_" + x).value.length == 0 ? 0 : document.getElementById("content_grid_item_txt_quantity_" + x).value;
 
    vva = input.value;
    vva = vva.split(",").join("");
    if (IfNumeric(vva) || price == 0 || qty == 0)
        document.getElementById("content_grid_item_txt_tprice_" + x).value = price * qty;
    else
        input.value = vva.substring(0, vva.length - 1);
}

function checkNumericInput(elem) {
    vva = elem.value;
    vva = vva.split(",").join("");
    if (!IfNumeric(vva)) 
        elem.value = vva.substring(0, vva.length - 1);
}

function IfNumeric(input) {
    return (input - 0) == input && (input + '').replace(/^\s+|\s+$/g, "").length > 0;
}

function kita(elem) {
    document.getElementById(elem).style.display = "inline";
}