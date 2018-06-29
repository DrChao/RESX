<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddWord.aspx.cs" Inherits="Language.Web.Web.AddWord" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" src="../JScripts/Source/ResourceInfo.js"></script>
    <script type="text/javascript">
        $(document).ready(LanguageOnReady);

        //ie 
        copyValue = function (strValue) {
            if (isIE()) {
                clipboardData.setData("Text", strValue);
            }
            else {
                copy(strValue);
            }
        }
        function isIE(number) {
            if (typeof (number) != number) {
                return !!document.all;
            }
        }
        function copy(text2copy) {
            var flashcopier = 'flashcopier';
            if (!document.getElementById(flashcopier)) {
                var divholder = document.createElement('div');
                divholder.id = flashcopier;
                document.body.appendChild(divholder);
            }
            document.getElementById(flashcopier).innerHTML = '';
            var divinfo = '<embed src="http://files.poluoluo.net/demoimg/200910/_clipboard.swf" FlashVars="clipboard=' + text2copy + '" width="0" height="0" type="application/x-shockwave-flash"></embed>'; //这里是关键 
            document.getElementById(flashcopier).innerHTML = divinfo;
        }

        function CopyToClipbord(id) {
            var _text = $("#" + id).attr("value");
            if (_text == null || _text == undefined) {
                _text = $("#" + id).text();
            }
            copyValue(_text);
            //otextRange = document.getElementById(id).createTextRange();
            //otextRange.execCommand("Copy");

        }
    </script>
</head>
<body>
    <form id="form1" action="">
    <div id="round">
        <div class="rtop">
            <div class="r1">
            </div>
            <div class="r2">
            </div>
            <div class="r3">
            </div>
            <div id="topbar" class="r4">
                当前位置：&nbsp;&nbsp;<a href="../Web/AddWord.aspx" target="_self" class="lastA">添加词条</a>
            </div>
        </div>
        <div style="border: 1px #808080 solid;">
            <table style="line-height: 30px; width: 100%">
                <tr>
                    <td style="text-align: right;">
                        请选择系统名称：
                    </td>
                    <td>
                        <select id="sltSysName" name="sltSysName">
                            <option value=""></option>
                            <option value="ERPClient">ERPClient</option>
                            <option value="SBPWeb">SBPWeb</option>
                            <option value="TAOS">TAOS技术资产营运系统</option>
                            <option value="CPC">CPC中国专利圈</option>
                            <option value="VimVault">機密雲</option>
                            <option value="BAOS">BAOS</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        请输入中文词条：
                    </td>
                    <td>
                        <input id="txtWord" name="txtWord" type="text" class="Input_line_500" onblur="CheckItemInfo();" />
                    </td>
                </tr>
                <tr style="display: none;" id='trShow'>
                
                    <td colspan="8" style="padding-top: 10px; padding-bottom: 10px;">
                        <div style="overflow: scroll; height: 500px;">
                            <table id="tabWord" class="Show">
                                <thead>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        其他：
                    </td>
                    <td>
                        <input id="chkJScript" name="chkJScript" type="checkbox" value="1" checked="checked" />是否生成JavaScript脚本多语言
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        JS替换代码：
                    </td>
                    <td>
                        <input id="txtJScript" name="txtJScript" readonly="readonly" type="text" class="Input_line_500"
                            style="color: Blue" />
                        <a id='dCopy1' name='dCopy' href="javascript:void(0);" onclick="CopyToClipbord('txtJScript');">
                            復制</a>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        CS后臺替换代码：
                    </td>
                    <td>
                        <input id="txtCSharp" name="txtCSharp" readonly="readonly" type="text" class="Input_line_500"
                            style="color: Blue" />
                        <a id='dCopy2' name='dCopy' href="javascript:void(0);" onclick="CopyToClipbord('txtCSharp');">
                            復制</a>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        html替换代码：
                    </td>
                    <td>
                        <input id="txtHtml" name="txtHtml" readonly="readonly" type="text" class="Input_line_500"
                            style="color: Blue" />
                        <a id='dCopy3' name='dCopy' href="javascript:void(0);" onclick="CopyToClipbord('txtHtml');">
                            復制</a>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <input id="Button1" type="button" value="新增" onclick="new Resx.ZH_CN.Controller().Save();" />
                    </td>
                </tr>
            </table>
        </div>
        <div>
        </div>
    </div>
    </form>
</body>
</html>
