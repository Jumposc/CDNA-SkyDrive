function login() {
    var LoginInfo = { name: "", pwds: "" };
    LoginInfo.name = document.getElementById("input-user").value.replace(/(^\s*)|(\s*$)/g, '');
    LoginInfo.pwds = document.getElementById("input-passwd").value.replace(/(^\s*)|(\s*$)/g, '');
    var xhttp = new XMLHttpRequest();
    xhttp.open("POST", "/api/Login");
    var Json = xhttp.send(JSON.stringify(LoginInfo));
    
}