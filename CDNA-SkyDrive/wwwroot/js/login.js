function Login() {
    var LoginInfo = { Name: "", Pwds: "" };
    LoginInfo.Name = document.getElementById("input-user").value.replace(/(^\s*)|(\s*$)/g, '');
    LoginInfo.Pwds = document.getElementById("input-passwd").value.replace(/(^\s*)|(\s*$)/g, '');
    xhttp = new XMLHttpRequest(); 
    xhttp.open("POST", "/api/Login");
    xhttp.send(JSON.stringify(LoginInfo));
    timeout = 0;
    setTimeout("Check()", 10);

}
var timeout = 0;
function Check() {
    timeout++;
    if (xhttp.status == 200) {
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
    } else {
        if (timeout > 110) {
            window.alert("timeout");
            return;
        }
        setTimeout("Check()", 10);
    }
    

}