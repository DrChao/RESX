function LoadTable() { }

LoadTable.prototype = {

    LoadCheckWordInfo: function (_Data) {
        var _tr = ''; var _Counts = 0;

        var _Head = "<tr><td colspan='5'>相关词条内容</td></tr>"
            + "<tr style='text-align:center;'><td style='width:30px;'>序号</td>"
            + "<td>词条内容</td><td>替换内容</td></tr>";
        var _End = "<tr><td colspan='3' style='text-align:center;'>无可显示的数据</td></tr>";

        if (_Data != null) {
            for (var i = 0; i < _Data.length; i++) {
                var _radId = "rad" + i.toString();
                _Counts = 0; //_Data[i].Counts;
                _foot = "共有 <font style='color:blue'>" + _Counts.toString()
                        + "</font> 个词条等待翻译，每页显示<font style='color:blue'> 10 </font>个词条，共 <font style='color:blue'>"
                        + Math.ceil(_Counts / 10) + "</font> 页";
                _tr = _tr + "<tr>"
                    + "<td style='text-align:center;'>" + (i + 1) + "</td>"
                    + "<td style='text-align:center;'>" + _Data[i].FValue + "</td>"
                    + "<td>" + _Data[i].Context + "</td>"
                + "</tr>"
            }
        }
        $('#tabWord>thead>tr').remove();
        if (_tr == '') {
            $('#tabWord>tbody>tr').remove();
            $('#trShow').css('display', 'none');

        } else {
            $('#tabWord>thead').append(_Head);
            $('#tabWord>tbody>tr').remove();
            $('#tabWord>tbody').append(_tr);
            $('#trShow').css('display', 'block');
        }
        return _Counts;
    }

}

var LoadTableCtrl = new LoadTable();

function LanguageOnReady() {
    new Resx.ZH_CN.Layout().LoadInfo();

}

function CheckItemInfo() {
    $("#txtJScript").attr("value", "");
    $("#txtCSharp").attr("value", "");
    $("#txtHtml").attr("value", "");
    var _Name = $('#sltSysName').attr("value");
    var _itemword = $("#txtWord").attr("value");
    if (_itemword == "") { return; }
    if (_Name == "") { return; }
    $('body').showLoading();    
    var _Data = { Call: 'CheckItemInfo', Word: _itemword, SysName: _Name };
    Ext.Ajax.request({
        url: "../Services/ResxServ.aspx", method: 'GET',
        params: _Data, success: OnSuccess,
        failure: function (_Response, _Request) {
            $('body').hideLoading();
        }
    });
    function OnSuccess(_Response, _Request) {
        var _RMeta = jQuery.parseJSON(_Response.responseText);
        LoadTableCtrl.LoadCheckWordInfo(_RMeta.MetaInfo.WordInfo);
        $('body').hideLoading();
    }
}




Resx.ZH_CN.Layout = function () {

    this.LoadInfo = function () {
        new Resx.ZH_CN.Controller().LoadTableInfo();
    }
}


Resx.ZH_CN.Controller = function () {

    this.LoadTableInfo = function () {
        var _Head = "<tr><td colspan='5'>待翻译词条</td></tr>"
            + "<tr class='trCenter'><td class='tdNumber'>序号</td><td>机构名称</td><td class='tdTime'>认证时间</td><td>领域</td>"
            + "<td class='td100'>联系电话</td></tr>";
        var _End = "<tr><td colspan='5' style='text-align:center;'>无可显示的数据</td></tr>";
        $('#taskShow>thead>tr').remove();
        $('#taskShow>tbody>tr').remove();
        $('#taskShow>thead').append(_Head);
        $('#taskShow>tbody').append(_End);
    }
    this.Save = function () {
        if ($('#sltSysName').attr('value') == '') {
            alert("请选择系统名称！");
            $('#sltSysName').focus().select();
            return;
        }
        if ($('#txtWord').attr('value') == '') {
            alert("请输入待翻译词条名称！");
            $('#txtWord').focus().select();
            return;
        }
        $('body').showLoading();
        $("#txtJScript").attr("value", "");
        $("#txtCSharp").attr("value", "");
        $("#txtHtml").attr("value", "");
        var _Ajax = new CallAjax();
        _Ajax.Form("form1", "../Services/ResxServ.aspx?Call=CreateWordItem", OnSuccess, null);
        function OnSuccess(_RMeta) {
            $('body').hideLoading();
            if (!_RMeta.ClientInfo.Status) {
                alert(_RMeta.ClientInfo.Message);
                return;
            }
            $('#txtCSharp').attr('value', _RMeta.MetaInfo.WordInfo.CSharp);
            $('#txtJScript').attr('value', _RMeta.MetaInfo.WordInfo.JScript);
            $('#txtHtml').attr('value', "<%=" + _RMeta.MetaInfo.WordInfo.JScript + " %");

        }
    }
}