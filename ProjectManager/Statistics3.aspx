<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Statistics3.aspx.cs" Inherits="ProjectManager.Statistics3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-inline">
        <div class="form-group">
            <label for="selYear" class="control-label">搜索条件</label>
            <select id="selYear" class="form-control" runat="server">
            </select>
            <label class="control-label">年</label>
        </div>
        <asp:Button runat="server" ID="btnSearch" CssClass="btn btn-info" Text="搜 索" OnClick="btnSearch_Click" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="导出Excel" CssClass="btn btn-danger" />
    </div>
    <hr />
    <div class="row">
        <div class="col-md-12">
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>月份</th>
                        <th>工单数</th>
                        <th>项目数</th>
                        <th>测试人数</th>
                        <th>上线次数</th>
                        <th>上线项目数</th>
                    </tr>
                </thead>
                <tbody>
                    <%=html %>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
