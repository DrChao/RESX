/*获取URL参数值*/
function GetReqestParaValue(_Url, _Name) {
    var _String = _Url.substring(_Url.indexOf("?") + 1, _Url.length).split("&");
    var _Obj = {}
    for (i = 0; j = _String[i]; i++) {
        _Obj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
    }
    var _Value = _Obj[_Name.toLowerCase()];
    if (typeof (_Value) == "undefined") {
        return "";
    } else {
        return _Value.replace('#', '');
    }
}

function Open(_Url) {
    $('#frmMain', top.document.body).attr('src', _Url);
}

/*常量定义*/
var CONST=new Object();
CONST.ZH_CN = "zh-CN";
CONST.ZH_TW = "zh-TW";
CONST.EN_US = "en-US";
CONST.JA_JP = "ja-JP";

var Resx = new Object();
Resx.ZH_CN = new Object();
Resx.Other = new Object();

var Translate = new Object();

var Files = new Object();


function CallAjax() {
    this.Url = null;
    this.Data = null;
    this.OnSuccess = null;
    this.OnError = null;
    this.Ajax = function () {
        $.ajax({
            type: 'POST',
            url: this.Url,
            data: this.Data,
            timeout: 3000,
            async: false,
            cache: false,
            dataType: 'json',
            success: this.OnSuccess,
            error: this.OnError
        });
    }

    /*直接调用后台方法*/
    this.AjaxMethod = function (_Url, _Data, _OnSuccess, _OnError) {
        if (_Data == '' || _Data == undefined) {
            _Data = null;
        }
        $.ajax({
            type: "POST",                            //提交方式
            url: _Url,                                   //提交的页面/方法名
            data: _Data,                             //参数（如果没有参数：null） : "{'Name':'keer'}"
            dataType: "json",                   //返回数据类型
            async: false,
            cache: false,
            contentType: "application/json;charset=utf-8;",
            success: _OnSuccess,
            error: _OnError
        });
    }

    this.Form = function (_ID, _Url, _Success, _Error) {
        var _Options = {
            beforeSubmit: function () { return true; },
            //表单提交前被调用的回调函数。"beforeSubmit"回调函数作为一个钩子（hook），被提供来运行预提交逻辑或者校验表单数据。
            //如果"beforeSubmit"回调函数返回false，那么表单将不被提交。
            //beforeSubmit”回调函数带三个调用参数：数组形式的表单数 据，jQuery表单对象，以及传入ajaxForm/ajaxSubmit中的Options对象。
            //表单数组接受以下方式的数据：[ { name: 'username', value: 'jresig' }, { name: 'password', value: 'secret' } ]
            url: _Url,   //指定提交表单数据的URL,默认值：表单的action属性值
            type: 'POST',    //指定提交表单数据的方法（method）："GET"或"POST"。默认值：表单的method属性值（如果没有找到默认为"GET"）。
            //dataType: "json",     //期望返回的数据类型。null、"xml"、"script"、"json"其中之一
            success: _Success,
            error: _Error
        }
        //注意：from 如果是 runat="server" 那option的url只能是提交给自己的.aspx，如果不是则可以提交给其他.aspx接收。
        //注意：from中的<input 标签 必须带有name属性，否则只有id Request.Form[] 会获得不到后增加的标签。
        $("#" + _ID + "").ajaxSubmit(_Options);
    }

}



/** 分页控件 **/
function PageControl(_Container, _Totals, _Paras, callBack) {
    this.pageSize = _Paras.PageSize || 10; //页面大小
    this.currentPage = 1; //当前页，默认为1
    this.totalRecordCount = _Totals || 0; //总记录条数
    this.Paras = _Paras || "";
    this.Url = _Paras.Url;
    this.container = document.getElementById(_Container); //装载分页控件的容器
    this.pageCount = this.div(this.totalRecordCount, this.pageSize);
    this.callBack = callBack;
}

PageControl.prototype = {

    div: function (firstNum, secondNum) {
        var n1 = Math.round(firstNum);
        var n2 = Math.round(secondNum);
        var rslt = parseInt(n1 / n2);
        var m = n1 % n2;
        return m > 0 ? rslt + 1 : rslt;
    },

    /** 跳转到某一页 **/
    GoToPage: function (pageNum, _IsClick) {
        this.currentPage = pageNum;
        this.Paint();
        if (_IsClick) { this.GetAjax(pageNum, this); }
    },

    /** 上一页 **/
    PrevPage: function () {
        this.currentPage = this.currentPage <= 1 ? 1 : this.currentPage - 1;
        this.GoToPage(this.currentPage, true);
    },

    /** 下一页 **/
    NextPage: function () {
        this.currentPage = this.currentPage >= this.pageCount ? this.pageCount : this.currentPage + 1;
        this.GoToPage(this.currentPage, true);
    },

    GetAjax: function (pagNum, instance) {
        if (this.Paras === "" || this.Paras === null || typeof (this.Paras) == "undefined") return;
        var _Url = this.Url + (this.Url.indexOf("?") >= 0 ? "&" : "?") + "Rand=" + Math.random();
        try {
            this.Paras.PageNum = instance.currentPage;
            $.get(_Url, this.Paras, function (data) {
                if (typeof (instance.callBack) != "undefined" && instance.callBack != null) {
                    instance.callBack(data, instance);
                }
            });

        } catch (e) {
            throw e;
        }
    },

    Paint: function () {
        var strArr = []; var _t = 8; var size = 5;
        if (this.currentPage > 1) {
            strArr.push("<div id='divPageControlPrev' class='page-control-forward'><a href='javascript:void(0);'>上一页</a></div>");
        }
        if (this.pageCount <= _t) {
            this.PaintPart(1, this.pageCount, strArr);
        } else {
            var className = 1 == this.currentPage ? "page-control-pagenum-selected" : "page-control-pagenum";
            strArr.push("<div class='" + className + "'><a href='javascript:void(0);'>" + 1 + "</a></div>");
            if (this.currentPage >= _t) {
                var t = (this.currentPage + size) >= this.pageCount ? (this.pageCount - 1) : (this.currentPage + size);
                strArr.push("<div class='page-control-split'>...</div>");
                this.PaintPart(this.currentPage - size, t, strArr);
                if ((this.currentPage + size) < this.pageCount - 1)
                    strArr.push("<div class='page-control-split'>...</div>");
            } else {
                if (this.pageCount >= _t) {
                    var t = (this.currentPage + size) >= this.pageCount ? (this.pageCount - 1) : (this.currentPage + size);
                    this.PaintPart(2, t, strArr);
                    if ((this.currentPage + size) < this.pageCount - 1)
                        strArr.push("<div class='page-control-split'>...</div>");
                } else {
                    this.PaintPart(2, this.pageCount - 1, strArr);
                }
            }
            var _temp = this.pageCount;
            var className = _temp == this.currentPage ? "page-control-pagenum-selected" : "page-control-pagenum";
            strArr.push("<div class='" + className + "'><a href='javascript:void(0);'>" + _temp + "</a></div>");
        }
        if (this.currentPage < this.pageCount) {
            strArr.push("<div id='divPageControlNext' class='page-control-forward'><a href='javascript:void(0);'>下一页</a></div>");
        }
        //var _Head="<div  class='page-control-forward'><a href='javascript:void(0);'>首页</a></div>"
        //var _End="<div  class='page-control-forward'><a href='javascript:void(0);'>尾页</a></div>"
        var _Other = "<div id='dRedirect' class='page-control-Redirect'>跳转到<input type='text' style='width:30px;'/>页&nbsp;<a href='javascript:void(0)'>GO</a></div>";
        $(this.container).html(strArr.join("") + _Other);
        this.BindEvents(this);
    },
    PaintPart: function (start, end, arr) {
        for (var c = start; c <= end; c++) {
            var className = c == this.currentPage ? "page-control-pagenum-selected" : "page-control-pagenum";
            arr.push("<div class='" + className + "'><a href='javascript:void(0);'>" + c + "</a></div>");
        }
    },

    BindEvents: function (instance) {//绑定事件
        $("#divPageControlPrev").bind("click", function () {
            instance.PrevPage();
        });
        $("#divPageControlNext").bind("click", function () {
            instance.NextPage();
        });
        $(".page-control-pagenum a").each(function () {
            $(this).bind("click", function () {
                var pageNum = parseInt($(this).html());
                instance.GoToPage(pageNum, true);
            });
        });
        $(".page-control-Redirect input").each(function () {
            $(this).bind("keydown", function (e) {
                e = e || event;
                if (e.keyCode == 13) {
                    var _GoNum = parseInt($(this).attr("value"));
                    if (_GoNum > instance.pageCount || _GoNum <= 0 || _GoNum.toString() == "NaN") {
                        return false;
                    }
                    instance.GoToPage(_GoNum, true); return false;
                }
            });
        });
        $(".page-control-Redirect a").each(function () {
            $(this).bind("click", function () {
                var _GoNum = parseInt($(".page-control-Redirect input").attr("value"));
                if (_GoNum > instance.pageCount || _GoNum <= 0 || _GoNum.toString() == "NaN") {
                    return;
                }
                instance.GoToPage(_GoNum, true);
            });
        });
    }
}



$.fn.showLoading = function (options) {
    var indicatorID;
    var settings = {
        'addClass': '', 'beforeShow': '', 'afterShow': '', 'hPos': 'center', 'vPos': 'center', 'indicatorZIndex': 5001,
        'overlayZIndex': 5000, 'parent': '', 'marginTop': 0, 'marginLeft': 0, 'overlayWidth': null, 'overlayHeight': null
    };

    $.extend(settings, options);

    var loadingDiv = $('<div></div>');
    var overlayDiv = $('<div></div>');

    if (settings.indicatorID) {
        indicatorID = settings.indicatorID;
    }
    else {
        indicatorID = $(this).attr('id');
    }
    if (indicatorID == "" || indicatorID == undefined) {
        indicatorID = "body";
    }

    $(loadingDiv).attr('id', 'loading-indicator-' + indicatorID);
    $(loadingDiv).addClass('loading-indicator');

    if (settings.addClass) {
        $(loadingDiv).addClass(settings.addClass);
    }

    $(overlayDiv).css('display', 'none');

    $(document.body).append(overlayDiv);

    $(overlayDiv).attr('id', 'loading-indicator-' + indicatorID + '-overlay');

    $(overlayDiv).addClass('loading-indicator-overlay');

    if (settings.addClass) {
        $(overlayDiv).addClass(settings.addClass + '-overlay');
    }

    var overlay_width;
    var overlay_height;

    var border_top_width = $(this).css('border-top-width');
    var border_left_width = $(this).css('border-left-width');

    border_top_width = isNaN(parseInt(border_top_width)) ? 0 : border_top_width;
    border_left_width = isNaN(parseInt(border_left_width)) ? 0 : border_left_width;

    var overlay_left_pos = $(this).offset().left + parseInt(border_left_width);
    var overlay_top_pos = $(this).offset().top + parseInt(border_top_width);

    if (settings.overlayWidth !== null) {
        overlay_width = settings.overlayWidth;
    }
    else {
        overlay_width = parseInt($(this).width()) + parseInt($(this).css('padding-right')) + parseInt($(this).css('padding-left'));
    }

    if (settings.overlayHeight !== null) {
        overlay_height = settings.overlayWidth;
    }
    else {
        overlay_height = parseInt($(this).height()) + parseInt($(this).css('padding-top')) + parseInt($(this).css('padding-bottom'));
    }


    $(overlayDiv).css('width', overlay_width.toString() + 'px');
    $(overlayDiv).css('height', overlay_height.toString() + 'px');

    $(overlayDiv).css('left', overlay_left_pos.toString() + 'px');
    $(overlayDiv).css('position', 'absolute');

    $(overlayDiv).css('top', overlay_top_pos.toString() + 'px');
    $(overlayDiv).css('z-index', settings.overlayZIndex);


    if (settings.overlayCSS) {
        $(overlayDiv).css(settings.overlayCSS);
    }

    $(loadingDiv).css('display', 'none');
    $(document.body).append(loadingDiv);

    $(loadingDiv).css('position', 'absolute');
    $(loadingDiv).css('z-index', settings.indicatorZIndex);

    var indicatorTop = overlay_top_pos;

    if (settings.marginTop) {
        indicatorTop += parseInt(settings.marginTop);
    }

    var indicatorLeft = overlay_left_pos;

    if (settings.marginLeft) {
        indicatorLeft += parseInt(settings.marginTop);
    }

    if (settings.hPos.toString().toLowerCase() == 'center') {
        $(loadingDiv).css('left', (indicatorLeft + (($(overlayDiv).width() - parseInt($(loadingDiv).width())) / 2)).toString() + 'px');
    }
    else if (settings.hPos.toString().toLowerCase() == 'left') {
        $(loadingDiv).css('left', (indicatorLeft + parseInt($(overlayDiv).css('margin-left'))).toString() + 'px');
    }
    else if (settings.hPos.toString().toLowerCase() == 'right') {
        $(loadingDiv).css('left', (indicatorLeft + ($(overlayDiv).width() - parseInt($(loadingDiv).width()))).toString() + 'px');
    }
    else {
        $(loadingDiv).css('left', (indicatorLeft + parseInt(settings.hPos)).toString() + 'px');
    }

    if (settings.vPos.toString().toLowerCase() == 'center') {
        $(loadingDiv).css('top', (indicatorTop + (($(overlayDiv).height() - parseInt($(loadingDiv).height())) / 2)).toString() + 'px');
    }
    else if (settings.vPos.toString().toLowerCase() == 'top') {
        $(loadingDiv).css('top', indicatorTop.toString() + 'px');
    }
    else if (settings.vPos.toString().toLowerCase() == 'bottom') {
        $(loadingDiv).css('top', (indicatorTop + ($(overlayDiv).height() - parseInt($(loadingDiv).height()))).toString() + 'px');
    }
    else {
        $(loadingDiv).css('top', (indicatorTop + parseInt(settings.vPos)).toString() + 'px');
    }

    if (settings.css) {
        $(loadingDiv).css(settings.css);
    }

    var callback_options = {
        'overlay': overlayDiv, 'indicator': loadingDiv, 'element': this
    };

    if (typeof (settings.beforeShow) == 'function') {
        settings.beforeShow(callback_options);
    }
    $(overlayDiv).show();
    $(loadingDiv).show();
    if (typeof (settings.afterShow) == 'function') {
        settings.afterShow(callback_options);
    }
    return this;
};


$.fn.hideLoading = function (options) {

    var settings = {};
    $.extend(settings, options);
    if (settings.indicatorID) {
        indicatorID = settings.indicatorID;
    }
    else {
        indicatorID = $(this).attr('id');
    }
    if (indicatorID == "" || indicatorID == undefined) {
        indicatorID = "body";
    }
    $(document.body).find('#loading-indicator-' + indicatorID).remove();
    $(document.body).find('#loading-indicator-' + indicatorID + '-overlay').remove();

    return this;
};
