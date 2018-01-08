 $("#btnAdd").click(function () {

        var Post = {
        "Content": $("#Content").val()
        };

        $.ajax({
        type: "POST",
        url: '~/api/Post/AddPost',
            data: JSON.stringify(PersonalDetails),
            contentType: "application/json;charset=utf-8",
            processData: true,
            success: function (data, status, xhr) {
        alert("The result is : " + status);
    },
            error: function (xhr) {
        alert(xhr.responseText);
    }
        });
    });