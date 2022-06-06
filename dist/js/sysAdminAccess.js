 /**ADMIN ACCESS**/
    var value = $("#empid").val();

    $(".allower").click(function (e) {

        var d = $(this).val();

        if (d == "1902" || d == "1901") {
            $("#btn_submit").show();
        } else {
            $("#btn_submit").hide();
        }

        $.ajax({
            type: "POST",
            url: "content/hr/addemployee.aspx/readUpdate",
            data: "{rid:" + $(this).val() + ",uid:" + value + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var ru = document.getElementsByClassName("readupdate");

                if (data.d) {
                    console.log("ins");
                    for (var i = 0; i < ru.length; i++) {
                        ru[i].setAttribute("style", "display:none;");
                    }

                }
                else {
                    for (var i = 0; i < ru.length; i++) {
                        ru[i].setAttribute("style", "display:block;");
                    }
                }

                console.log(d);

                    
            },
            error: function (err, result) {
                alert(err);
            }
        });

    });
    /**END**/

});