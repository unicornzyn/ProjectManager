<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Statistics5.aspx.cs" Inherits="ProjectManager.Statistics5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="datepicker/WdatePicker.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="form-inline">
        <div class="form-group">
            <label for="txtStart" class="control-label">统计时间</label>
            <input type="text" class="form-control" id="txtStart" onfocus="WdatePicker({ onpicked: function () { $('#txtEnd').focus(); } })" runat="server" />
            <label for="txtEnd" class="control-label">-</label>
            <input type="text" class="form-control" id="txtEnd"  onfocus="WdatePicker({minDate:'#F{$dp.$D(\'txtStart\')}'})" runat="server" />
            <label class="control-label">快速选择</label>
            <asp:DropDownList ID="ddl" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged" runat="server">
                <asp:ListItem Value="1" Text="本周" Selected="True"></asp:ListItem>
                <asp:ListItem Value="2" Text="本月"></asp:ListItem>
            </asp:DropDownList>            
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
                        <th>测试人员</th>
                        <th>bug数</th>                        
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="rpt">
                        <ItemTemplate>
                            <tr>
                                <td><%#Eval("Name") %></td>
                                <td><a href="<%#Eval("Link") %>" target="_blank"><%#Eval("CC") %></a></td>                                
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>

                </tbody>
            </table>
        </div>
    </div>
</asp:Content>

