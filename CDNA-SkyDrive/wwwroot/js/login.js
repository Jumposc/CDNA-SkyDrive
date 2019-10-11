function Login() {
    var LoginInfo = { Name: "", Pwds: "" };
    LoginInfo.Name = document.getElementById("input-user").value.replace(/(^\s*)|(\s*$)/g, '');
    LoginInfo.Pwds = document.getElementById("input-passwd").value.replace(/(^\s*)|(\s*$)/g, '');
    xhttp = new XMLHttpRequest(); 
    xhttp.open("POST", "/api/Login");
    xhttp.send(JSON.stringify(LoginInfo));
    setTimeout("Check()", 800);

}
function Check() {
    var json = JSON.parse(xhttp.responseText);
    if (json.Message == "OK") {
        var day = new Date();
        day.setHours(day.getHours() + 24);
        var cook = "Token=" + json.Data + ";" + "expires=" + day.toUTCString();
        document.cookie = cook;
        window.location.href = "../html/home.html";
    } else {
        var LoginInfo = document.getElementById("login-error-info");
        LoginInfo.style.display = "block";
    }

}