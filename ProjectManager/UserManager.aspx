<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserManager.aspx.cs" Inherits="ProjectManager.UserManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="datepicker/WdatePicker.js"></script>
    <script type="text/javascript">
        $(function () {
            if ($("#hidUserRole").val() != "1") {
                $("#btnAdd,.btnmodify,.btndel").hide();
            }
            $("#btnAdd").click(function () {
                $("#txtUserName").val("");
                $("#txtRealName").val("");
            });

            $(".btnmodify").click(function () {
                $("#myModal").modal('show');
                $("#txtUserName").val($($(this).parent().parent().find("td").get(0)).text());
                $("#txtRealName").val($($(this).parent().parent().find("td").get(1)).text());                
                var role = $($(this).parent().parent().find("td").get(2)).text();
                $("#selRole").val(role == "用户" ? "2" : "1");
                $("#txtLeaveTime").val($($(this).parent().parent().find("td").get(3)).text());
            });
        });

        function valid() {
            $("#spUserName,#spRealName").text("");
            if ($("#txtUserName").val() == "") {
                $("#spUserName").text("请输入用户名"); return false;
            }
            if ($("#txtRealName").val() == "") {
                $("#spRealName").text("请输入姓名"); return false;
            }            
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-12">
            <input type="button" id="btnAdd" class="btn btn-success" value="添加用户" data-toggle="modal" data-target="#myModal" />
        </div>
    </div>
    <hr />
    <div class="row">
        <div class="col-md-12">
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>用户名</th>
                        <th>姓名</th>
                        <th>角色</th>
                        <th>离职时间</th>
                        <th>操作</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rpt" runat="server" OnItemCommand="rpt_ItemCommand">
                        <ItemTemplate>
                            <tr>
                                <td><%#Eval("UserName") %></td>
                                <td><%#Eval("RealName") %></td>
                                <td><%#GetRoleName(Eval("RoleType").ToString()) %></td>
                                <td><%#Eval("Status").ToString()=="1"?"":Common.St.ToDateTimeString(Eval("LeaveTime"),"yyyy-MM-dd") %></td>
                                <td>
                                    <input type="button" class="btn btn-default btnmodify" value="修改" />
                                    <asp:Button runat="server" CssClass="btn btn-default btndel" Text="删除" CommandArgument='<%#Eval("Id") %>' OnClientClick="return window.confirm('确定删除吗?');" CommandName="DeleteUser" />
                                    <asp:Button runat="server"  CssClass="btn btn-default" Text='<%#Eval("Status").ToString()=="1"?"禁用":"启用" %>' CommandArgument='<%#Eval("Id") %>' CommandName="ValidUser" />
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
                    <h4 class="modal-title" id="myModalLabel">用户管理</h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal">
                        <div class="text-info">
                            <ul>
                                <li>默认密码 123456</li>
                                <li>用户名存在修改，用户名不存在添加</li>                                
                                <li>禁用状态的用户离职时间才会生效</li>  
                            </ul>
                        </div>
                        <div class="form-group">
                            <label class="col-md-2 control-label" for="txtUserName">用户名</label>
                            <div class="col-md-10">
                                <input class="form-control" id="txtUserName" runat="server" type="text" value="" />
                                <span class="field-validation-valid text-danger" id="spUserName"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-2 control-label" for="txtRealName">姓名</label>
                            <div class="col-md-10">
                                <input class="form-control" id="txtRealName" runat="server" type="text" value="" />
                                <span class="field-validation-valid text-danger" id="spRealName"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-2 control-label" for="selRole">角色</label>
                            <div class="col-md-10">
                                <select class="form-control" id="selRole" runat="server">
                                    <option value="3">普通用户</option>
                                    <option value="2">录入用户</option>
                                    <option value="1">管理员</option>
                                </select>
                                <span class="field-validation-valid text-danger" id="spSelRole"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-2 control-label" for="selRole">离职时间</label>
                            <div class="col-md-10">
                                <input class="form-control" id="txtLeaveTime" onclick="WdatePicker()" runat="server" type="text" value="" />
                                <span class="field-validation-valid text-danger" id="spLeaveTime"></span>
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
