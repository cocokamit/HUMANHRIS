function freaze(x, y) {
    var width = new Array();
    var table = $("table[id*=" + x + "]"); //Pass your gridview id here.
    table.find("th").each(function(i) {
        width[i] = $(this).width();
    });
    headerRow = table.find("tr:first");
    headerRow.find("th").each(function(i) {
        $(this).width(width[i]);
    });
    firstRow = table.find("tr:first").next();
    firstRow.find("td").each(function(i) {
        $(this).width(width[i]);
    });
    var header = table.clone();
    header.empty();
    header.append(headerRow);
    //            header.append(firstRow);
    // header.css("width", width);
    header.width(table.width() - 5);
    $("#" + y + "").before(header);
    table.find("tr:first td").each(function(i) {
        $(this).width(width[i]);
    });
    //  $("#" + y + "").css("width", "100%");
    $("#" + y + "").height($(window).height() - 280);
    $("#" + y + "").width(table.width() + 14);
    $("#" + y + "").append(table);
}