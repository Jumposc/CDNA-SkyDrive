function Register() {
    var RegisterInfo = { Name: "", Pwds: "", PhoneNumber: "" };
    RegisterInfo.Name = document.getElementById("reg-input-user").value;
    RegisterInfo.Pwds = document.getElementById("reg-input-passwd").value;
    RegisterInfo.PhoneNumber = document.getElementById("reg-input-phone-number").value;
    if ((RegisterInfo.Name || RegisterInfo.Pwds || RegisterInfo.PhoneNumber) == "" || (RegisterInfo.Name || RegisterInfo.Pwds || RegisterInfo.PhoneNumber) == undefined || (RegisterInfo.Name || RegisterInfo.Pwds || RegisterInfo.PhoneNumber) == null) {
        EchoRegistryError("信息不能为空");
        return;
    }
    if (RegisterInfo.Pwds != document.getElementById("reg-input-confirm-passwd").value) {
        EchoRegistryError("重复密码不匹配");
        return;
    }
    if (RegisterInfo.PhoneNumber.length != 11) {
        EchoRegistryError("手机号码长度错误");
        return;
    }
        xhttp = new XMLHttpRequest();
        xhttp.open("POST", "/api/Register");
        xhttp.send(JSON.stringify(RegisterInfo));
        CheckReg();
}
var timeout = 0;
function CheckReg() {
    timeout++;
    if (xhttp.status == 200) {
        var json = JSON.parse(xhttp.responseText);
        if (json.Message == "OK") {
            window.confirm(json.Data);
            window.location.href = "../index.html";
        } else {
            EchoRegistryError(json.Data);
        }
    } else {
        if (timeout > 110) {
            EchoRegistryError("连接超时");
            return;
        }
        setTimeout("CheckReg()", 10);
    }
}
function EchoRegistryError(error) {
    var html = document.getElementById("register-error");
    html.innerHTML = error;
    html.style.display = "block";
}