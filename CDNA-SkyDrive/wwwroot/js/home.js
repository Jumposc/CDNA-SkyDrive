
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
