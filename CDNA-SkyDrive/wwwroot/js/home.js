
function ChangeBackground(html, color) {
    return function (){
        html.style.backgroundColor = color;
    }
}

function SetDisplay(html,css) {
    return function () {
        html.style.display = css;
    }
}
function LoadBoxClose() {
    var CloseHTML = document.getElementsByClassName("load-box");
    for (i = 0; i < CloseHTML.length; i++) {
        CloseHTML[i].style.display = "none";
    }

}

function PostFile() {
    var xhttp = new XMLHttpRequest();

}

function OnLoadEvn() {
    var li = document.querySelectorAll(".type-ul li");
    for (i = 0; i < li.length; i++) {
        li[i].addEventListener("mouseover", ChangeBackground(li[i], "rgba(128,128,128,0.5)"));
        li[i].addEventListener("mouseout", ChangeBackground(li[i], "rgb(248,248,248)"));
    }
    var ul = document.querySelectorAll(".file-info ul,.file-info-container li");
    for (i = 0; i < ul.length; i++) {
        ul[i].addEventListener("mouseover", ChangeBackground(ul[i], "rgba(128,128,128,0.5)"));
        ul[i].addEventListener("mouseout", ChangeBackground(ul[i], "rgb(255,255,255)"));
    }
    var evntul = document.querySelectorAll(".file-info ul");
    var han = document.querySelectorAll(".file-handle");
    for (i = 0; i < evntul.length; i++) {
        evntul[i].addEventListener("mouseover", SetDisplay(han[i], "block"));
        evntul[i].addEventListener("mouseout", SetDisplay(han[i], "none"));
    }
    var inputbtn = document.getElementById("input-file");
    inputbtn.addEventListener("change", function () {
        var form = document.getElementById("input-form");
        form.submit();

    })
    var loadbox = document.getElementById("load-box");
    var x = 0;
    var y = 0;
    var l = 0;
    var t = 0;
    var isdown = false;
    //鼠标按下时计算坐标
    loadbox.onmousedown = function (e) {
        x = e.clientX;
        y = e.clientY;
        l = loadbox.offsetLeft;
        t = loadbox.offsetTop;
        isdown = true;
        loadbox.style.cursor = "move";
    }
    window.onmousemove = function (e) {
        if (isdown == false) {
            return;
        }
        //获取x和y
        var nx = e.clientX;
        var ny = e.clientY;
        //计算移动后的左偏移量和顶部的偏移量
        var nl = nx - (x - l);
        var nt = ny - (y - t);
        loadbox.style.top = nt + "px";
        loadbox.style.left = nl + "px"
    }
    //鼠标放开
    loadbox.onmouseup = function () {
        isdown = false;
        loadbox.style.cursor = 'default';
        }
    
}
