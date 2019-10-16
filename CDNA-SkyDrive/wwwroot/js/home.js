
function ChangeBackground(html, color) {
    return function () {
        html.style.backgroundColor = color;
    }
}

function SetDisplay(html, css) {
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
    return function () {
        fileobj = document.getElementById("input-file").files;
        var form = new FormData();
        for (i = 0; i < fileobj.length; i++) {
            form.append(fileobj[i].name, fileobj[i]);
        }
        var xhr = new XMLHttpRequest();
        xhr.open("POST", "/api/Load/Up", true);
        xhr.setRequestHeader("Path","./")
        PushBox();
        CreateLoadBox();
        xhr.upload.addEventListener("progress", function (e) {
            {
                if (e.lengthComputable) {
                    PreBox = document.getElementsByClassName('load-file-status-box');
                    var FileSize = [];
                    var j;
                    for (i = 0; i < fileobj.length; i++) {
                        FileSize[i] = fileobj[i].size;
                    }
                    for (i = PreBox.length - fileobj.length; i < PreBox.length; i++) {
                        j = i;
                        if (e.loaded / FileSize[i] < 1) {
                            PreBox[i].style.width = 100 * e.loaded / FileSize[i - j] + "%";
                            PreBox[i].innerHTML = Math.floor(100 * e.loaded / FileSize[i - j]) + "%";
                        }
                        if (e.loaded / FileSize[i - j] >= 1) {
                            PreBox[i].style.width = "100%";
                            PreBox[i].innerHTML = "检验中";
                        }
                        j--;
                    }
                }
            }
        })
        xhr.send(form);
        xhr.onreadystatechange = function () {
            if (xhr.status == 200) {
                for (i = PreBox.length - fileobj.length; i < PreBox.length; i++) {
                    PreBox[i].innerHTML = "上传成功";
                }
            } else {
                for (i = PreBox.length - fileobj.length; i < PreBox.length; i++) {
                    PreBox[i].innerHTML = "上传失败";
                    PreBox[i].style.backgroundColor = "#db2828";
                }
            }
            var FileValue = document.getElementById("input-file");
            FileValue.value = null;
        }
    }
}


function PushBox() {
    var box = document.querySelectorAll(".load-box, .load-box div");
    for (i = 0; i < box.length; i++) {
        box[i].style.display = "block";
    }

}

function CreateLoadBox() {
    var BoxInfo = document.getElementById("load-file");
    var Liname = ["load-file-name", "load-file-size", "load-file-status"];
    for (i = 0; i < fileobj.length; i++) {
        var Ul = document.createElement("ul");
        Ul.className = "load-file-box";
        BoxInfo.appendChild(Ul);
        var fileinfo = [fileobj[i].name, fileobj[i].size / 1024 > 1 ? (fileobj[i].size / 1024 / 1024 > 1 ? Math.floor(fileobj[i].size / 1024 / 1024) + "M" : Math.floor(fileobj[i].size / 1024) + "kb") : fileobj[i].size + "b", "0%"];
        for (j = 0; j < Liname.length; j++) {
            var Li = document.createElement("li");
            Li.className = Liname[j];
            var SpBox = document.createElement("span");
            SpBox.className = Liname[j] + "-box";
            SpBox.innerText = fileinfo[j];
            Ul.appendChild(Li);
            Li.appendChild(SpBox);
        }

    }
}

//获取用户文件列表
function GetUserFileList(path) {
    var xmlhttp = new XMLHttpRequest();
    xmlhttp.open("POST", "/api/List");
    xmlhttp.send(path);

    xmlhttp.onreadystatechange = GetFileList(xmlhttp);
}
function GetFileList(xmlhttp) {
    return function () {
        if (xmlhttp.status == 200) {
            var FileList = JSON.parse(JSON.parse(xmlhttp.responseText).Data);
            FindFileType(FileList);
        }
    }
}
//将文件列表分类
function FindFileType(FileList) {
    var FileDir = JSON.parse("[]");
    var File = JSON.parse("[]");
    for (i in FileList) {
        if (FileList[i].type == "dir") {
            FileDir.push(FileList[i]);
        } else {
            File.push(FileList[i]);
        }
    }
    CreateFileDirList(FileDir);
    CreateFileList(File);
}

function CreateFileDirList(Dir) {
    if (Dir.length > 1) {
        Dir.sort(function (a, b) { return a.name.localeCompare(b.name) });
    }
    var FileBox = document.getElementById("file-list-container");
    var LiName = ["file-name-dir", "file-size", "file-date"];
    var DirInfo = [Dir.name, Dir.size, Dir.time];
    for (i = 0; i < Dir.length;i++) {
        var Ul = document.createElement("ul");
        Ul.className = "file";
        FileBox.appendChild(Ul);
        for (j = 0; j < LiName.length;j++) {
            var Li = document.createElement("li");
            Li.className = LiName[j];
            Ul.appendChild(Li);
            var Span = document.createElement("span");
            Span.value = DirInfo[j];
            Li.appendChild(Span);
        }

    }
}
function CreateFileList(File) {
    if (File.length > 1) {
        File.sort(function (a, b) { return a.name.localeCompare(b.name) });
    }
    var FileBox = document.getElementById("file-list-container");
    var LiName = ["file-name", "file-size", "file-date"];
    for (i = 0; i < File.length;i++) {
        var Ul = document.createElement("ul");
        Ul.className = "file";
        FileBox.appendChild(Ul);
        for (j = 0; j < LiName.length;j++) {
            var Li = document.createElement("li");
            Li.className = LiName[j];
            Ul.appendChild(Li);
        }

    }
}
    

function Down() {
    var xmlhttp = new XMLHttpRequest();
    xmlhttp.open("POST", "/api/Load/Down", true);
    xmlhttp.responseType = "blob";
    xmlhttp.onreadystatechange = function (data) {
        var content = xmlhttp.response;

        var elink = document.createElement('a');
        elink.download = "123.txt";
        elink.style.display = 'none';

        var blob = new Blob([content]);
        elink.href = URL.createObjectURL(blob);

        document.body.appendChild(elink);
        elink.click();

        document.body.removeChild(elink);
    };
    xmlhttp.send();

}
//页面加载时，添加事件
function OnLoadEvn() {
    GetUserFileList("./");
    Down();
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
    inputbtn.addEventListener("change", PostFile());
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
