<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Other.aspx.cs" Inherits="Language.Web.Web.Other" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../JScripts/Source/TranslateInfo.js"></script>
    <script type="text/javascript">
        $(document).ready(TranslateOnReady);
    </script>    
</head>
<body>
    <form id="form1" runat="server">
        <div id="round">
            <div class="rtop">
                <div class="r1"></div>
                <div class="r2"></div>
                <div class="r3"></div>
                <div id="topbar" class="r4">
                    当前位置：&nbsp;&nbsp;<a href="../Web/Other.aspx?CultureType=ja-JP" target="_self" class="lastA" >日文翻译</a>
                </div>
            </div>
            <div>
                <table id="taskShow" class="Show">
                    <thead>
                    <tr>
                        <td colspan=6 style=" padding:10px 0px 0px 0px;">
                            <div style="float:left; width:100px;">请选择系统名称：</div>
                            <div style="float:left;">
                                <select id="sltSysName" name="sltSysName" onchange="javascript:$('#Radio1').click();">
                                    <option value="TAOS">TAOS技术资产营运系统</option>
                                    <option value="CPC">CPC中国专利圈</option>
                                    <option value="VimVault">機密雲</option>
                                </select>
                            </div>
                        </td>                     
                    </tr>
                        <tr>
                            <td colspan=6>
                                <div style="float:left; width:100px;">请选择查询条件：</div>
                                <div style="float:left;">
                                    <input id="Radio1" type="radio" name="radTrans" value="0" checked="checked" onclick="ChangeSelectRadio(this);"/>待翻译
                                    <input id="Radio2" type="radio" name="radTrans" value="1" onclick="ChangeSelectRadio(this);"/>已翻译
                                </div>
                                <div style="float:right; padding-left:50px; text-align:right;" id='dMessage'></div>                             
                            </td>
                        </tr>
                        <tr>
                            <td style="width:30px; text-align:center;">选择</td>
                            <td style="width:30px; text-align:center;">序號</td>
                            <td style="text-align:center;">待翻译词条</td>
                            <td style="width:80px; text-align:center;">Key值</td>
                            <td style="width:80px; text-align:center;">所属系统</td>
                            <td style="width:120px; text-align:center;">创建时间</td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan='6' style='text-align:center;'>暂无可显示的数据</td>
                        </tr>
                    </tbody>
                    <tfoot></tfoot>
                </table>       
            </div>
            <div style="padding-top:5px;text-align: right;"><div id='dPage'></div></div>
        </div>
    </form>
    <form id="form2" action="" style="display:none;">
    <div>
        <fieldset class="FieldSet">
            <legend>待翻译信息</legend>
            <table>
                <tr>
                    <td style="text-align:right;">待翻译词条：</td>
                    <td>
                        <textarea id="txtWord"  name="txtWord" cols="20" rows="2"
                             style="width:500px; min-height:30px; font-size:9pt;"  readonly="readonly"></textarea>
                    </td>
                </tr>              
                 <tr>
                    <td style="text-align:right;">日文翻译：</td>
                    <td>
                        <textarea id="txtWordEN"  name="txtWordEN" cols="20" rows="2"
                             style="width:500px; min-height:30px;font-size:9pt;" ></textarea>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td><input type="button" value="保存" onclick="new Translate.Controller().SaveResxInfo();" /></td>
                </tr>
                <tr>
                    <td colspan="8"><input id="txtKey" name="txtKey" type="text"  style="display:none;"/></td>
                </tr>
                 <tr>
                    <td colspan="8"><input id="txtSysName" name="txtSysName" type="text"  style="display:none;"/></td>
                </tr>
            </table>
        </fieldset>
    </div>
    </form>
     
</body>
</html>
