<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Statistics4.aspx.cs" Inherits="ProjectManager.Statistics4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-inline">
        <div class="form-group">
            <label for="input" class="control-label">年份</label>
            <select id="selYear" class="form-control" runat="server">
            </select>           
        </div>
        <asp:Button runat="server" ID="btnSearch" Text="搜 索" CssClass="btn btn-info" OnClick="btnSearch_Click" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="导出Excel" CssClass="btn btn-danger" />
    </div>
    <hr />
    <div class="row">
        <div class="col-md-12">
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>负责人</th>
                        <th>一月</th>
                        <th>二月</th>
                        <th>三月</th>
                        <th>四月</th>
                        <th>五月</th>
                        <th>六月</th>
                        <th>七月</th>
                        <th>八月</th>
                        <th>九月</th>
                        <th>十月</th>
                        <th>十一月</th>
                        <th>十二月</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="rpt">
                        <ItemTemplate>
                            <tr>
                                <td><%#Eval("Name") %></td>
                                <td><%#Eval("M1") %></td>
                                <td><%#Eval("M2") %></td>
                                <td><%#Eval("M3") %></td>
                                <td><%#Eval("M4") %></td>
                                <td><%#Eval("M5") %></td>
                                <td><%#Eval("M6") %></td>
                                <td><%#Eval("M7") %></td>
                                <td><%#Eval("M8") %></td>
                                <td><%#Eval("M9") %></td>
                                <td><%#Eval("M10") %></td>
                                <td><%#Eval("M11") %></td>
                                <td><%#Eval("M12") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>

                </tbody>
            </table>
        </div>
    </div>
</asp:Content>

