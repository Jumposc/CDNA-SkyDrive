
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
