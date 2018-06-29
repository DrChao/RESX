<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Language.Web._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>多语言翻译系统</title>
    <script type="text/javascript">      
//        var zh_CN = { 'A120928111213': '是否生成JavaScript脚本多语言', 'S120928111319': '系统管理员' }
//        var zh_TW = { 'A120928111213': '是否生成JavaScript腳本多語言', 'S120928111319': '系統管理員' }
//        var en_US = { 'A120928111213': 'Is Create JavaScript', 'S120928111319': 'System admin' }
//        alert(en_US.A120928113811);
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 100%">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td style="width: 150px;vertical-align: top; ">
                    <div style="border: 1px #AABBCE solid; min-height: 750px;">
                        <table style="line-height:30px;">
                            <tr>
                                <td><a href="javascript:void(0)" onclick="Open('Web/AddWord.aspx');">添加词条</a></td>
                            </tr>
                            <tr>
                                <td>待翻译词条</td>
                            </tr>
                            <tr>
                                <td style="padding-left:20px;">
                                    <a href="javascript:void(0)" onclick="Open('Web/CNTWEN.aspx');">中英文翻译</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left:20px;">
                                    <a href="javascript:void(0)" onclick="Open('Web/Other.aspx?CultureType=ja-JP');">日文翻译</a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <a href="javascript:void(0)" onclick="Open('Web/CreateResxFile.aspx');">多语言文件</a>
                                </td>
                            </tr>
                            <tr>
                                <td><a href="javascript:void(0)" onclick="Open('Web/ResxSearch.aspx');">查询词条</a></td>
                            </tr>
                            <tr>
                                <td>
                                    <a href="javascript:void(0)" onclick="Open('Web/ResxTest.aspx');">多语言测试</a>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td style="width: 10px;"></td>
                <td style="width: 1000px; text-align: left; vertical-align: top; min-height: 800px;">
                    <iframe id="frmMain" name="frmMain" style="border: 0px; overflow-x: hidden;" frameborder="no"
                        marginwidth="0" marginheight="0" scrolling="no" allowtransparency="yes" width="1000px;"
                        height="800px;" src="Web/AddWord.aspx"></iframe>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
