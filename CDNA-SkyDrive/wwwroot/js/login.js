function login() {
    var LoginInfo = { "username": "", "password": "" };
    LoginInfo.username = document.getElementById("input-user").value;
    LoginInfo.password = document.getElementById("input-passwd").value;
    window.alert(LoginInfo.username + "+" + LoginInfo.password);
}