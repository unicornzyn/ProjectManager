<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DicManager.aspx.cs" Inherits="ProjectManager.DicManager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            if ($("#hidUserRole").val() != "1") {
                $("#btnAdd,.btnmodify,.btndel").hide();
            }
            $("#btnAdd").click(function () {
                $("#hid").val("0");
                $("#txtCode").val("");
                $("#txtName").val("");
                $("#txtRemark").val("");
            });

            $(".btnmodify").click(function () {
                $("#myModal").modal('show');
                $("#hid").val($($(this).parent().parent()).data("id"));
                $("#selType").val(($($(this).parent().parent().find("td").get(0)).data("type")));
                $("#txtCode").val($($(this).parent().parent().find("td").get(1)).text());
                $("#txtName").val($($(this).parent().parent().find("td").get(2)).text());                   
                $("#txtRemark").val($($(this).parent().parent().find("td").get(3)).text());
            });
        });

        function valid() {
            $("#spName").text("");
            if ($("#txtName").val() == "") {
                $("#spName").text("请输入字典值"); return false;
            }            
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-12">
            <input type="button" id="btnAdd" class="btn btn-success" value="添加字典" data-toggle="modal" data-target="#myModal" />
        </div>
    </div>
    <hr />
    <div class="row">
        <div class="col-md-12">
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>类型</th>
                        <th>字典项</th>
                        <th>字典值</th>
                        <th>描述</th>                        
                        <th>操作</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rpt" runat="server" OnItemCommand="rpt_ItemCommand">
                        <ItemTemplate>
                            <tr data-id="<%#Eval("Id") %>">
                                <td data-type="<%#Eval("Type") %>"><%#GetDicName(Eval("Type").ToString()) %></td>
                                <td><%#Eval("Code") %></td>
                                <td><%#Eval("Name") %></td>
                                <td><%#Eval("Remark") %></td>
                                <td>
                                    <input type="button" class="btn btn-default btnmodify" value="修改" />
                                    <asp:Button runat="server" CssClass="btn btn-default btndel" Text="删除" CommandArgument='<%#Eval("Id") %>' OnClientClick="return window.confirm('确定删除吗?');" CommandName="DeleteDic" />
</td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </div>

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">字典管理</h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-md-2 control-label" for="selType">类型</label>
                            <div class="col-md-10">
                                <select class="form-control" id="selType" runat="server">
                                    <option value="1">研发人员</option>
                                </select>
                                <span class="field-validation-valid text-danger" id="spSelType"></span>
                                <input type="hidden" value="0" runat="server" id="hid" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-md-2 control-label" for="txtCode">字典项</label>
                            <div class="col-md-10">
                                <input class="form-control" id="txtCode" runat="server" type="text" value="" />
                                <span class="field-validation-valid text-danger" id="spCode"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-2 control-label" for="txtName">字典值</label>
                            <div class="col-md-10">
                                <input class="form-control" id="txtName" runat="server" type="text" value="" />
                                <span class="field-validation-valid text-danger" id="spName"></span>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label class="col-md-2 control-label" for="txtRemark">描述</label>
                            <div class="col-md-10">
                                <input class="form-control" id="txtRemark" runat="server" type="text" value="" />
                                <span class="field-validation-valid text-danger" id="spRemark"></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                    <asp:Button runat="server" ID="btnSave" Text="保存" CssClass="btn btn-info" OnClick="btnSave_Click" OnClientClick="return valid();" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
