function login() {
    //var LoginInfo = { "username": "", "password": "" };
    //LoginInfo.username = document.getElementById("input-user").value;
    //LoginInfo.password = document.getElementById("input-passwd").value;
    //window.alert(LoginInfo.username + "+" + LoginInfo.password);
    $.ajax({
        url: "api/Login",
        type: "POST",
        //contentType:"application/json",
        data: {
            name: "123",
            pwds: "abc"
        },
        success: function (data) {
            window.alert(data);
        }
    });
}