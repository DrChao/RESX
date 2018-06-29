var _CultureType = '';
var _TransType = '0';

function TranslateOnReady() {
    _CultureType = GetReqestParaValue(document.location.href, "CultureType");
    new Translate.LayOut().Load(_CultureType, _TransType);
}

function ChangeSelectRadio(_this) {
    $("#form2").resetForm().css("display", "none");
    _TransType = _this.value;
    _CultureType = GetReqestParaValue(document.location.href, "CultureType");
    new Translate.LayOut().Load(_CultureType, _TransType);
}


function LoadTable() { }

LoadTable.prototype = {

    LoadWaitTableInfo: function (_Data) {
        var _tr = ''; var _Counts = 0;
        if (_Data != null) {
            for (var i = 0; i < _Data.length; i++) {
                var _radId = "rad" + i.toString();
                _Counts = _Data[i].Counts;
                _foot = "共有 <font style='color:blue'>" + _Counts.toString()
                        + "</font> 个词条等待翻译，每页显示<font style='color:blue'> 10 </font>个词条，共 <font style='color:blue'>"
                        + Math.ceil(_Counts / 10) + "</font> 页";
                _tr = _tr + "<tr onclick='new Translate.Controller().SelectRadioCtrl(this,\"" + _CultureType + "\")'>"
                    + "<td style='text-align:center;'> <input name='radInfo' type='radio' value='" + _Data[i].FKey + "'/></td>"
                    + "<td style='text-align:center;'>" + (i + 1) + "</td>"
                    + "<td >" + _Data[i].FValue + "</td><td>" + _Data[i].FKey + "</td>"
                    + "<td style='text-align:center;'>" + _Data[i].Sys_Name + "</td>"
                    + "<td>" + _Data[i].CTime + "</td>"
                + "</tr>"
            }
        }
        if (_tr == '') {
            $('#taskShow>tbody>tr').remove();
            $('#taskShow>tbody').append("<tr><td style='text-align:center;' colspan='6'>暂无显示的数据</td></tr>");
            $('#dMessage>div').remove();
            $('#dPage').css('display', 'none');
        } else {
            $('#taskShow>tbody>tr').remove();
            $('#taskShow>tbody').append(_tr);
            $('#dMessage>div').remove();
            $('#dMessage').append("<div>" + _foot + "</div>");
            $('#dPage').css('display', 'block');
        }
        return _Counts;
    }

}

var LoadTableCtrl = new LoadTable();

Translate.WordInfo = null;
Translate.LayOut = function () {

    this.Load = function (_CultureType, _IsTrans) {
        $('body').showLoading();
        var _Name = $("#sltSysName").attr("value");        
        var _Data = { Call: 'LoadWaitWordInfo', PageSize: 10, PageNum: 1, SysName: _Name,
            Url: '../Services/ResxServ.aspx', CultureType: _CultureType, IsTrans: _IsTrans
        };
        Ext.Ajax.request({
            url: _Data.Url, method: 'GET',
            params: _Data, success: OnSuccess,
            failure: function (_Response, _Request) {
                $('body').showLoading();
            }
        });

        function OnSuccess(_Response, _Request) {
            var _RMeta = jQuery.parseJSON(_Response.responseText);
            var _foot = _tr = '';

            Translate.WordInfo = _RMeta.MetaInfo.WordInfo;
            var _Totals = LoadTableCtrl.LoadWaitTableInfo(_RMeta.MetaInfo.WordInfo);
            if (_Totals == 0) {
                $('body').hideLoading();
                return;
            }
            var _PageControl = new PageControl("dPage", _Totals, _Data, function (_Response, _Instance) {
                LoadTableCtrl.LoadWaitTableInfo(_Response.MetaInfo.WordInfo);
            });
            _PageControl.GoToPage(1);
            $('body').hideLoading();
        }
    }

}

Translate.Controller = function () {

    this.SelectRadioCtrl = function (_this, _CultureType) {

        $("#form2").resetForm().css("display", "block");

        var _title = $(_this).find("td")[2].innerHTML;
        var _SysName = $(_this).find("td")[4].innerHTML;
        $('#txtWord').attr('value', _title);
        $('#txtSysName').attr('value', _SysName);
        var _FKey = '';
        $(_this).find("input").each(function () {
            $(this).attr('checked', true);
            _FKey = this.value;
        })
        $('#txtKey').attr('value', _FKey);
        LoadInfo(_FKey);
        function LoadInfo(_FKey) {
            new Translate.Controller().GetDetailWordInfo(_FKey);
        }
    }

    this.GetDetailWordInfo = function (_FKey) {
        var _CultureType = GetReqestParaValue(document.location.href, "CultureType");
        var _Ajax = new CallAjax();
        _Ajax.Url = '../Services/ResxServ.aspx';
        _Ajax.Data = { Call: "GetDetailWordInfo", FKey: _FKey, CultureType: _CultureType };
        _Ajax.OnSuccess = OnSuccess;
        _Ajax.Ajax();
        function OnSuccess(_RMeta) {
            if (!_RMeta.ClientInfo.Status) {
                alert(_RMeta.ClientInfo.Message); return;
            }

            if (_RMeta.MetaInfo.DetailInfo == null) {
                return;
            }

            for (var i = 0; i < _RMeta.MetaInfo.DetailInfo.length; i++) {
                if (_RMeta.MetaInfo.DetailInfo[i].Resx_Type.toString().toLowerCase() == CONST.ZH_CN.toString().toLowerCase()) {
                    $('#txtWordCN').attr('value', _RMeta.MetaInfo.DetailInfo[i].Resx_Text)
                }
                if (_RMeta.MetaInfo.DetailInfo[i].Resx_Type.toString().toLowerCase() == CONST.ZH_TW.toString().toLowerCase()) {
                    $('#txtWordTW').attr('value', _RMeta.MetaInfo.DetailInfo[i].Resx_Text)
                }
                if (_RMeta.MetaInfo.DetailInfo[i].Resx_Type.toString().toLowerCase() == CONST.EN_US.toString().toLowerCase()) {
                    $('#txtWordEN').attr('value', _RMeta.MetaInfo.DetailInfo[i].Resx_Text)
                }
                if (_CultureType != "") {
                    if (_RMeta.MetaInfo.DetailInfo[i].Resx_Type.toString().toLowerCase() == CONST.JA_JP.toString().toLowerCase()) {
                        $('#txtWordEN').attr('value', _RMeta.MetaInfo.DetailInfo[i].Resx_Text)
                    }
                }
            }
        }
    }

    this.SaveResxInfo = function () {
        if ($('#txtWord').attr('value') == '') {
            alert("请选择待翻译词条！"); return;
        }
        var _CultureType = GetReqestParaValue(document.location.href, "CultureType");
        var _Ajax = new CallAjax();
        _Ajax.Form("form2", "../Services/ResxServ.aspx?Call=SaveResxInfo&CultureType="
            + _CultureType, OnSuccess, null);
        function OnSuccess(_RMeta) {
            if (!_RMeta.ClientInfo.Status) {
                alert(_RMeta.ClientInfo.Message);
                return;
            }
            $("#form2").resetForm();
            alert(_RMeta.ClientInfo.Message);
            if (_TransType == "0") {
                document.location.reload();
            } else {
                $("#form2").resetForm().css("display", "none");
            }
        }
    }

}