function login() {
    var LoginInfo = { name: "", pwds: "" };
    LoginInfo.name = document.getElementById("input-user").value;
    LoginInfo.pwds = document.getElementById("input-passwd").value;
    var xhttp = new XMLHttpRequest();
    xhttp.open("POST", "/api/Login");
    xhttp.send(JSON.stringify(LoginInfo));

}