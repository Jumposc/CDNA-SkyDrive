function Login() {
    var LoginInfo = { Name: "", Pwds: "" };
    LoginInfo.Name = document.getElementById("input-user").value.replace(/(^\s*)|(\s*$)/g, '');
    LoginInfo.Pwds = document.getElementById("input-passwd").value.replace(/(^\s*)|(\s*$)/g, '');
    if ((LoginInfo.Name || LoginInfo.Pwds) == "" || (LoginInfo.Name || LoginInfo.Pwds) == undefined || (LoginInfo.Name || LoginInfo.Pwds) == null) {
        EchoLoginError("用户名或密码不能为空");
    } else {
        xhttp = new XMLHttpRequest();
        xhttp.open("POST", "/api/Login");
        xhttp.send(JSON.stringify(LoginInfo));
        timeout = 0;
        CheckLogin();
    }
}
function CheckLoginToken() {
    if (document.cookie.indexOf("Token") != -1) {
        var xmlhttp = new XMLHttpRequest();
        xmlhttp.open("GET", "/api/Token");
        xmlhttp.onreadystatechange = function () {
            if (xmlhttp.status == 200 && xmlhttp.readyState == 4) {
                window.location.href = "./html/home.html";
                return;
            } else if ((xmlhttp.status != 200) && (xmlhttp.readyState == 4)) {
                document.cookie = "Token=;" + "expires=Thu, 01 Jan 1970 00: 00: 00 GMT";
                return;
            }
        }
    } else {
        return;
    }
    xmlhttp.send();
}
var timeout = 0;
function CheckLogin() {
    timeout++;
    if (xhttp.status == 200) {
        var json = JSON.parse(xhttp.responseText);
        if (json.Message == "OK") {
            var day = new Date();
            day.setHours(day.getHours() + 24);
            var cook = "Token=" + json.Data + ";" + "expires=" + day.toUTCString();
            var Username = "UserName=" + document.getElementById("input-user").value + ";";
            document.cookie = cook;
            document.cookie = Username;
            window.location.href = "./html/home.html";
        } else {
            EchoLoginError("用户名或密码错误");
        }
    } else {
        if (timeout > 110) {
            EchoLoginError("连接超时");
            return;
        }
        setTimeout("CheckLogin()", 10);
    }
}
function EchoLoginError(str) {
    var LoginInfo = document.getElementById("login-error-info");
    LoginInfo.innerHTML = str;
    LoginInfo.style.display = "block";
}