function login() {
    var LoginInfo = { name: "", Pwds: "" };
    LoginInfo.name = document.getElementById("input-user").value.replace(/(^\s*)|(\s*$)/g, '');
    LoginInfo.Pwds = document.getElementById("input-passwd").value.replace(/(^\s*)|(\s*$)/g, '');
    var xhttp = new XMLHttpRequest();
    xhttp.open("POST", "/api/Login");
    var Json = xhttp.send(JSON.stringify(LoginInfo));
    
}