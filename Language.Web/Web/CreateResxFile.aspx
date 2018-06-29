<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateResxFile.aspx.cs"
    Inherits="Language.Web.Web.CreateResxFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>生成多语言文件</title>
    <script type="text/javascript" src="../JScripts/Source/FilesInfo.js"></script>
    <script type="text/javascript">
        $(document).ready(FilesInfoOnReady);
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
                当前位置：&nbsp;&nbsp;<a href="../Web/CreateResxFile.aspx" target="_self" class="lastA" >多语言文件</a>
            </div>
        </div>
        <div style="border:1px #808080 solid; ">
        
            <table style="line-height: 30px;">
                <tr>
                    <td>选择系统名称：</td>
                    <td>
                        <select id="sltSysName" name="sltSysName" style="width:150px;">
                            <option value=""></option>
                            <option value="SBPWeb">SBPWeb</option>
                            <option value="TAOS">TAOS技术资产营运系统</option>
                            <option value="CPC">CPC中国专利圈</option>
                            <option value="VimVault">機密雲</option>
                            <option value="VimVault">BAOS</option>
                        </select>
                    </td>
                    <td><input type="button" value="生成多语言文件" onclick="new Files.Controller().CreatFile();" /></td>
                </tr>                
            </table>
            <div style="padding:10px;">
                <table id="taskShow" class="Show">
                    <thead>
                        <tr>
                            <td style="width: 30px; text-align: center;">序號</td>
                            <td style="text-align: center;">文件名</td>
                            <td style="text-align: center;">文件大小</td>
                            <td style="width: 80px; text-align: center;">所属系统</td>
                            <td style="width: 150px; text-align: center;">创建时间</td>
                            <td style="width: 80px; text-align: center;">操作</td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr><td colspan='5'>暂无待处理的数据</td></tr>
                    </tbody>
                </table>
           </div>
    </div>
    </div>
    <div>
        <iframe id="frmDoc" style="display: none;" src=""></iframe>
    </div>
    </form>
</body>
</html>
