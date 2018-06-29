<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResxTest.aspx.cs" Inherits="Language.Web.Web.ResxTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding:20px;">
        <table style="line-height:30px;">
            <tr>
                <td style="width:120px;text-align:right;">请输入词条编号：</td>
                <td><asp:TextBox runat="server" ID="txtKey"></asp:TextBox></td>
                <td>
                    <asp:Button ID="btSearch" runat="server" Text="查询" onclick="btSearch_Click" />
                </td>
            </tr>
        </table>
    
         <table style="line-height:30px;">
         
            <tr>
                <td style="width:120px;text-align:right;">简体中文：</td>
                <td>
                    <textarea id="txtCN" cols="20" rows="2" runat="server" readonly="readonly" style="width:500px; min-height:60px;font-size:9pt;"></textarea>
                </td>
            </tr>
             <tr>
                <td style="width:120px;text-align:right;">繁体中文：</td>
                <td>
                    <textarea id="txtTW" cols="20" rows="2" runat="server" readonly="readonly" style="width:500px; min-height:60px;font-size:9pt;"></textarea>
                </td>
            </tr>
             <tr>
                <td style="width:120px;text-align:right;">英文：</td>
                <td>
                    <textarea id="txtEN" cols="20" rows="2" runat="server" readonly="readonly" style="width:500px; min-height:60px;font-size:9pt;"></textarea>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
