$(document).ready(function () {


    var path = $(location).attr('pathname');
    var id = path.replace("/api/Account/Details/", "");
    PostPosts();

    function PostPosts() {
        $(".post-section").remove();
        $.ajax({
            url: "/api/PostApi/InsertPosts/" + id,
            type: "GET",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var s = $('<div class="post-section">' +
                        '<p>' + JSON.stringify(data[i]) + '</p>' +
                        '</div>');
                    $("#postContainer").append(s);
                }
            },

            error: function (xhr) {
                alert("Något gick tyvärr fel :(");
            }
        });
    }


    $('#addPost').click(function () {
        var Post = {
            "Content": $('#postContent').val()
        };
        var mail = $('#email').text();


        $.ajax({
            type: "POST",
            url: '/api/PostApi/AddPost/' + id,
            data: JSON.stringify(Post),
            contentType: "application/json;charset=utf-8",
            processData: true,
            success: function (Post, status, xhr) {
                PostPosts();
            },
            error: function (xhr) {
                alert("Det uppstod ett fel :(");
            }
        });
    });
});