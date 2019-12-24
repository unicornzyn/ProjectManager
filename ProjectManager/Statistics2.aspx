<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Statistics2.aspx.cs" Inherits="ProjectManager.Statistics2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="datepicker/WdatePicker.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="form-inline">
        <div class="form-group">
            <label for="txtStart" class="control-label">上线时间</label>
            <input type="text" class="form-control" id="txtStart" onfocus="WdatePicker({ onpicked: function () { $('#txtEnd').focus(); } })" runat="server" />
            <label for="txtEnd" class="control-label">-</label>
            <input type="text" class="form-control" id="txtEnd"  onfocus="WdatePicker({minDate:'#F{$dp.$D(\'txtStart\')}'})" runat="server" />
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
                        <th>工作单编号/域名</th>
                        <th>项目</th>
                        <th>上线日期</th>
                        <th>扫描次数</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="rpt">
                        <ItemTemplate>
                            <tr>
                                <td><%#Eval("SheepNo") %></td>
                                <td><%#Eval("Project.Name") %></td>
                                <td><%#Common.St.ToDateTimeString(Eval("PublishTime"),"yyyy-MM-dd") %></td>
                                <td><%#Eval("SecretScanCount") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>

                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
