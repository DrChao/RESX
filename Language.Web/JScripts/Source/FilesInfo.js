function FilesInfoOnReady() {
 
}

Files.Layout = function () {
 
}

Files.Controller = function () {

    this.CreatFile = function () {
        var _SysName = $('#sltSysName').attr('value');
        if (_SysName == '') {
            alert("请选择系统名称！");
            $('#sltSysName').focus().select();
            return;
        }
        var _Ajax = new CallAjax();
        _Ajax.Url = '../Services/ResxServ.aspx';
        _Ajax.Data = { Call: "CreatResxFile", SysName: _SysName };
        _Ajax.OnSuccess = OnSuccess;
        _Ajax.Ajax();
        function OnSuccess(_RMeta) {
            if (!_RMeta.ClientInfo.Status) {
                alert(_RMeta.ClientInfo.Message); return;
            }
            var _FileInfo = _RMeta.MetaInfo.FileInfo;
            var _tr = "<tr><td colspan='5'>暂无待处理的数据</td></tr>";
            if (_FileInfo != null) {
                _tr = '';
                for (var i = 0; i < _FileInfo.length; i++) {
                    _tr = _tr + "<tr><td style='text-align: center;'>" + _FileInfo[i].Number + "</td><td style='text-align: center;'>" + _FileInfo[i].FileName + "</td>"
                        + "<td style='text-align: center;'>" + _FileInfo[i].SysName + "</td><td style='text-align: center;'>" + _FileInfo[i].Size + "</td><td style='text-align: center;'>" + _FileInfo[i].CTime + "</td>"
                        + "<td style='text-align: center;'>"
                        + "<a href='javascript:void(0)' onclick='new Files.Controller().DownFile(\"" + _FileInfo[i].FileName + "\",\""
                        + _FileInfo[i].SysName + "\");'>下载</a></td></tr>"
                }
            }
            $('#taskShow>tbody>tr').remove();
            $('#taskShow>tbody').append(_tr);
        }
    }


    this.DownFile = function (_FileName, _SysName) {
        var _frmDoc = document.getElementById("frmDoc");
        var _URL = '../Web/BrowserFile.aspx?FileName=' + _FileName + '&SysName=' + _SysName;
        _frmDoc.src = _URL;
    }

}