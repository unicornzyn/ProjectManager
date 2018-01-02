/*短信发送*/
var iWidth = 410; //弹出窗口的宽度;
var iHeight = 300; //弹出窗口的高度;
var iTop = (window.screen.availHeight - 30 - iHeight) / 2;       //获得窗口的垂直位置;
var iLeft = (window.screen.availWidth - 10 - iWidth) / 2;           //获得窗口的水平位置;
var send = {
    fun: function (strNum, strState) {
        if (strState == "True") {
            window.open("SendMessageN.aspx?strnum=" + strNum, "aa", "height=" + iHeight + ", width=" + iWidth + ", top=" + iTop + ", left=" + iLeft + ", depended = yes, resizable = yes");
        }
        else {
            window.open("SendMessageY.aspx?strnum=" + strNum, "aa", "height=" + iHeight + ", width=" + iWidth + ", top=" + iTop + ", left=" + iLeft);
        }
    }
}
function reflash() {
    window.location.href = window.location.href;
}
