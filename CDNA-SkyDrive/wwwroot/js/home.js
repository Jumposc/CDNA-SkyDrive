var NowPath = "./";

//设置事件背景色
function ChangeBackground(html, color) {
    return function () {
        html.style.backgroundColor = color;
    }
}
//设置事件样式
function SetDisplay(html, css) {
    return function () {
        html.style.display = css;
    }
}
//对文件列表进行排序
function sortByKey(array, key) {
    return array.sort(function (a, b) {
        var x = a[key]; var y = b[key];
        return ((x < y) ? -1 : ((x > y) ? 1 : 0));
    });
}
//设置size对应的单位
function SizeUnit(size) {
    return size / 1024 > 1 ? (size / 1024 / 1024 > 1 ? Math.floor(size / 1024 / 1024) + "M" : Math.floor(size / 1024) + "kb") : size + "b";
}
//返回上级目录
function BackDir() {
    if (NowPath == "./") {
        return;
    }
    var path = NowPath.split("/");
    path.splice((path.length - 2), 1);
    NowPath = path.join("/");
    GetUserFileList(NowPath);
}
//关闭上传窗体
function LoadBoxClose() {
    var CloseHTML = document.getElementsByClassName("load-box");
    for (i = 0; i < CloseHTML.length; i++) {
        CloseHTML[i].style.display = "none";
    }
}
//上传文件
function PostFile() {
    return function () {
        fileobj = document.getElementById("input-file").files;
        var form = new FormData();
        for (i = 0; i < fileobj.length; i++) {
            form.append(fileobj[i].name, fileobj[i]);
        }
        var xhr = new XMLHttpRequest();
        xhr.open("POST", "/api/Load/Up", true);
        xhr.setRequestHeader("Path", "./")
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
            if (xhr.status == 200 && JSON.parse(xhr.responseText).Message == "OK") {
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

//显示上传窗体
function PushBox() {
    var box = document.querySelectorAll(".load-box, .load-box div");
    for (i = 0; i < box.length; i++) {
        box[i].style.display = "block";
    }
}
//创建上传窗体
function CreateLoadBox() {
    var BoxInfo = document.getElementById("load-file");
    var Liname = ["load-file-name", "load-file-size", "load-file-status"];
    for (i = 0; i < fileobj.length; i++) {
        var Ul = document.createElement("ul");
        Ul.className = "load-file-box";
        BoxInfo.appendChild(Ul);
        var fileinfo = [fileobj[i].name, SizeUnit(fileobj[i].size), "0%"];
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
//清楚用户当前列表
function ClearFileList() {
    var Box = document.getElementById("file-list-container");
    var List = document.querySelectorAll(".file-list-container ul");
    var BackBtn = document.getElementById("back-bnt");
    if (BackBtn != null) {
        Box.removeChild(BackBtn);
    }
    if (List.length != 0) {
        for (i = 0; i < List.length; i++) {
            Box.removeChild(List[i]);
        }
    }
}

//获取用户文件列表
function GetUserFileList(path) {
    ClearFileList();
    if (path != "./") {
        NowPath = NowPath + path + "\/";
    }
    var xmlhttp = new XMLHttpRequest();
    xmlhttp.open("POST", "/api/List");
    xmlhttp.send(NowPath);
    xmlhttp.onreadystatechange = GetFileList(xmlhttp);
}
function GetFileList(xmlhttp) {
    return function () {
        if (xmlhttp.status == 200 && xmlhttp.readyState == 4) {
            var FileList = JSON.parse(JSON.parse(xmlhttp.responseText).Data);
            FindFileType(FileList);
        } else {
            return;
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
//创建目录列表
function CreateFileDirList(Dir) {
    var FileBox = document.getElementById("file-list-container");
    if (NowPath != "./") {
        var Back = document.createElement("a");
        Back.id = "back-bnt";
        Back.innerHTML = "返回上级";
        Back.onclick = function () { BackDir() };
        FileBox.appendChild(Back);
    }
    if (Dir.length > 1) {
        Dir = sortByKey(Dir, "name");
    }
    var LiName = ["file-name-dir", "file-size", "file-date"];
    for (i = 0; i < Dir.length; i++) {
        var DirInfo = [Dir[i].name, "目录", Dir[i].time];
        var Ul = document.createElement("ul");
        Ul.className = "file";
        FileBox.appendChild(Ul);
        for (j = 0; j < LiName.length; j++) {
            var Li = document.createElement("li");
            Li.className = LiName[j];
            Ul.appendChild(Li);
            var Span = document.createElement("span");
            Span.innerHTML = DirInfo[j];
            Li.appendChild(Span);
        }
    }
    var DirList = document.getElementsByClassName("file-name-dir");
    for (i = 0; i < DirList.length; i++) {
        DirList[i].innerHTML = "";
        var A = document.createElement("a");
        A.innerHTML = "<span>" + Dir[i].name;
        A.name = Dir[i].name;
        A.style.cursor = "pointer";
        A.onclick = function (a) { GetUserFileList(A.name) };
        DirList[i].appendChild(A);
        var Div = document.createElement("div");
        Div.className = "file-handle";
        Div.style.display = "none";
        Div.innerHTML = "<img src=\"../images/mv.png\">" + "<img src=\"../images/rename.png\">" + "<img src=\"../images/delete.png\">";
        DirList[i].appendChild(Div);
    }
    var LiDir = document.querySelectorAll(".file-list-container .file-name-dir");
    for (i = 0; i < LiDir.length; i++) {
        LiDir[i].style.cursor = "pointer";
        LiDir[i].addEventListener("click", function () { GetUserFileList(this.children[0].name)})
    }
}
//创建文件列表
function CreateFileList(File) {
    if (File.length > 1) {
        File = sortByKey(File, "name");
    }
    var FileBox = document.getElementById("file-list-container");
    var LiName = ["file-name", "file-size", "file-date"];
    for (i = 0; i < File.length; i++) {
        var FileInfo = [File[i].name, SizeUnit(File[i].size), File[i].time];
        var Ul = document.createElement("ul");
        Ul.className = "file";
        FileBox.appendChild(Ul);
        for (j = 0; j < LiName.length; j++) {
            var Li = document.createElement("li");
            Li.className = LiName[j];
            Ul.appendChild(Li);
            var Span = document.createElement("span");
            Span.innerHTML = FileInfo[j];
            Li.appendChild(Span);
        }
    }
    var FileLiName = document.getElementsByClassName("file-name");
    for (i = 0; i < FileLiName.length; i++) {
        var CheckBox = document.createElement("input");
        var Div = document.createElement("div");
        Div.className = "file-handle";
        Div.style.display = "none";
        Div.innerHTML = "<img src=\"../images/mv.png\">" + "<img src=\"../images/cp.png\">" + "<img src=\"../images/rename.png\">" + "<img src=\"../images/down.png\" id=\"" + File[i].name + "\"" + "onclick=\"Down(this.id)\"" + ">" + "<img src=\"../images/delete.png\">";
        CheckBox.type = "checkbox";
        CheckBox.className = "checkbox";
        CheckBox.value = File[i].name;
        FileLiName[i].appendChild(CheckBox);
        FileLiName[i].appendChild(Div);
    }
    AddEven();
}

//下载文件方法
function Down(name) {
    var xmlhttp = new XMLHttpRequest();
    xmlhttp.open("POST", "/api/Load/Down", true);
    xmlhttp.responseType = "blob";
    xmlhttp.setRequestHeader("Path", NowPath + name);
    xmlhttp.onreadystatechange = function (data) {
        if (xmlhttp.status == 200 && xmlhttp.readyState == 4) {
            var content = xmlhttp.response;
            var elink = document.createElement('a');
            elink.download = name;
            elink.style.display = 'none';
            var blob = new Blob([content]);
            elink.href = URL.createObjectURL(blob);
            document.body.appendChild(elink);
            elink.click();
            document.body.removeChild(elink);
        } else {
            return;
        }
    };
    xmlhttp.send();
}
//添加列表动态样式
function AddEven() {
    //var ul = document.querySelectorAll(".file-list-container ul,.file-info-container li");
    //for (i = 0; i < ul.length; i++) {
    //    ul[i].addEventListener("mouseover", ChangeBackground(ul[i], "rgba(128,128,128,0.5)"));
    //    ul[i].addEventListener("mouseout", ChangeBackground(ul[i], "rgb(255,255,255)"));
    //}
    var evntul = document.querySelectorAll(".file-list-container ul");
    var han = document.querySelectorAll(".file-handle");
    for (i = 0; i < evntul.length; i++) {
        evntul[i].addEventListener("mouseover", SetDisplay(han[i], "block"));
        evntul[i].addEventListener("mouseout", SetDisplay(han[i], "none"));
    }
}
//页面加载时，添加事件
function OnLoadEvn() {
    GetUserFileList(NowPath);
    //Down();
    //var li = document.querySelectorAll(".type-ul li");
    //for (i = 0; i < li.length; i++) {
    //    li[i].addEventListener("mouseover", ChangeBackground(li[i], "rgba(128,128,128,0.5)"));
    //    li[i].addEventListener("mouseout", ChangeBackground(li[i], "rgb(248,248,248)"));
    //}
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