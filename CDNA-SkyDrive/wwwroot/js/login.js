function login() {
    var LoginInfo = { name: "", pwds: "" };
    LoginInfo.username = document.getElementById("input-user").value;
    LoginInfo.password = document.getElementById("input-passwd").value;
    var xhttp = new XMLHttpRequest();
    xhttp.open("POST", "/api/Login");
    xhttp.send(JSON.stringify(LoginInfo));

}