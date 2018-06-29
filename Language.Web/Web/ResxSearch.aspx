<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResxSearch.aspx.cs" Inherits="Language.Web.Web.ResxSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="round">
        <div class="rtop">
            <div class="r1">
            </div>
            <div class="r2">
            </div>
            <div class="r3">
            </div>
            <div id="topbar" class="r4">
                当前位置：&nbsp;&nbsp;<a href="../Web/ResxSearch.aspx" target="_self" class="lastA">查询词条</a>
            </div>
        </div>
        <div>
            <div style="border: 1px #808080 solid;">
                <table style="line-height: 30px;">
                    <tr>
                        <td style="width: 120px; text-align: right;">
                            选择系统名称：
                        </td>
                        <td>
                            <select id="sltSysName" name="sltSysName" style="width: 200px;" runat="server">
                                <option value=''></option>
                                <option value="TAOS">TAOS技术资产营运系统</option>
                                <option value="CPC">CPC中国专利圈</option>
                                <option value="VimVault">機密雲</option>
                                <option value="VimVault">BAOS</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px; text-align: right;">
                            请选择查询条件：
                        </td>
                        <td>
                            <select id="sltSearchField" name="sltSearchField" runat="server" style="width: 200px;">
                                <option value="Fkey">词条编号</option>
                                <option value="FValue">待翻译词条</option>
                            </select>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSearch"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btSearch" runat="server" Text="查询" OnClientClick="javascript:$('body').showLoading();"
                                OnClick="btSearch_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <table style="line-height: 40px;">
                    <tr>
                        <td style="width: 120px; text-align: right;">
                            词条编号：
                        </td>
                        <td>
                            <input id="txtFkey" name="txtFkey" runat="server" readonly="readonly" type="text"
                                class="Input_line_500" style="color: Blue" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px; text-align: right;">
                            待翻译词条：
                        </td>
                        <td>
                            <input id="txtWord" name="txtWord" runat="server" readonly="readonly" type="text"
                                class="Input_line_500" style="color: Blue" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px; text-align: right;">
                            简体中文：
                        </td>
                        <td>
                            <textarea id="txtCN" cols="20" rows="2" runat="server" readonly="readonly" style="width: 500px;
                                min-height: 60px; font-size: 9pt;"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px; text-align: right;">
                            繁体中文：
                        </td>
                        <td>
                            <textarea id="txtTW" cols="20" rows="2" runat="server" readonly="readonly" style="width: 500px;
                                min-height: 60px; font-size: 9pt;"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px; text-align: right;">
                            英文：
                        </td>
                        <td>
                            <textarea id="txtEN" cols="20" rows="2" runat="server" readonly="readonly" style="width: 500px;
                                min-height: 60px; font-size: 9pt;"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            C# 替换代码：
                        </td>
                        <td>
                            <input id="txtCSharp" runat="server" name="txtCSharp" readonly="readonly" type="text"
                                class="Input_line_500" style="color: Blue" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            JScript 替换代码：
                        </td>
                        <td>
                            <input id="txtJScript" runat="server" name="txtJScript" readonly="readonly" type="text"
                                class="Input_line_500" style="color: Blue" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
