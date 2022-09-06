$(function () {
    if ($("#confirm-deletion").length) {
        $("#confirm-deletion").click(() => {
            if (!confirm("Confirm Deletion")) return false;
        });
    }

    if ($("div.alert").length) {
        setTimeout(() => {
            $("div.alert").fadeOut();
        }, 5000)
    }
});
