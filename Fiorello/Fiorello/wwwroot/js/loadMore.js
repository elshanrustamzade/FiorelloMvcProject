let count = 8;

let productsCounts = $("#loadMore").next().val();


$(document).on("click", "#loadMore", function () {
    $.ajax({
        url: "/Products/LoadMore/",
        type: "get",
        data: {
            skip: count
        },
        success: function (res) {
            $("#myProducts").append(res)
            count += 8;

            if (productsCounts <= count) {
                $("#loadMore").remove()
            }
        }
    });
});