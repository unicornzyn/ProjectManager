<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ServerManager.aspx.cs" Inherits="ProjectManager.ServerManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        table {table-layout:fixed;
        }
        td {word-break:break-all; word-wrap:break-word; 
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#btnAdd").click(function () {
                $("#myModal").modal('show');
                $("#txtId").val("0");
                $("#txtServerName").val("");
                $("#txtUserName").val("");
                $("#txtPassword").val("");
                $("#txtIISVersion").val("");
                $("#txtSqlVersion").val("");
                $("#txtOSName").val("");
                $("#txtServerType").val("");
                $("#ckServerType1,#ckServerType2").attr("checked", false);
                $("#projectidlist").empty();                
                $("#txtProjectId").val("");
            });

            $(".btnmodify").click(function () {
                $("#myModal").modal('show');
                $("#txtId").val($($(this).parent().parent().find("td").get(0)).attr("data-id"));
                $("#txtServerName").val($($(this).parent().parent().find("td").get(0)).text());
                $("#txtUserName").val($($(this).parent().parent().find("td").get(1)).text());
                $("#txtPassword").val($($(this).parent().parent().find("td").get(2)).text());
                $("#txtIISVersion").val($($(this).parent().parent().find("td").get(3)).text());
                $("#txtSqlVersion").val($($(this).parent().parent().find("td").get(4)).text());
                $("#txtOSName").val($($(this).parent().parent().find("td").get(6)).text());
                var ServerType = $($(this).parent().parent().find("td").get(7)).text();
                $("#txtServerType").val(ServerType);
                if (ServerType.indexOf("WEB服务器") >= 0) { $("#ckServerType1").prop("checked", true); }
                if (ServerType.indexOf("SQL服务器") >= 0) { $("#ckServerType2").prop("checked", true); }
                var ids = $($(this).parent().parent().find("td").get(5)).attr("data-id");
                var name = $($(this).parent().parent().find("td").get(5)).text();
                $("#projectidlist").empty();
                $("#txtProjectId").val("");
                if (ids != "") {
                    var a_ids = ids.split(','); var a_names = name.split(','); $("#txtProjectId").val(ids);
                    for (i = 0; i < a_ids.length; i++) {
                        if (!a_ids[i]) { continue; }
                        $("#projectidlist").append('<li class="list-group-item" data-id="' + a_ids[i] + '">' + a_names[i] + '<span class="glyphicon glyphicon-remove badge"> </span></li>');
                    }
                }
            });
            $("#ckServerType1,#ckServerType2").click(function () {
                var s = "";
                if ($("#ckServerType1").prop("checked")) { s = "WEB服务器"; }
                if ($("#ckServerType2").prop("checked")) { if (s) { s = "WEB服务器,SQL服务器"; } else { s = "SQL服务器"; } }
                $("#txtServerType").val(s);
            });
            $("#btnSearch").click(function () {
                var txt = $.trim($("#txtSearch").val());
                $.each($("#tblist tr"), function (i, n) {
                    if (txt) {
                        if ($($(n).find("td").get(0)).text().indexOf(txt) >= 0) {
                            $(n).show();
                        } else {
                            $(n).hide();
                        }
                    } else {
                        $(n).show();
                    }
                });
            });

            function getprojectid() {
                var ids = "";
                $.each($("#projectidlist li"), function (i, n) {
                    ids += $(n).attr("data-id") + ",";
                });
                $("#txtProjectId").val(ids);
            }
            $("#projectidlist").on("click", ".glyphicon-remove", function () { $(this).parent().remove(); getprojectid() });
            $("#ulProject a").click(function () {
                if ($("#projectidlist li[data-id='" + $(this).attr("data-val") + "']").length == 0) {
                    $("#projectidlist").append('<li class="list-group-item" data-id="' + $(this).attr("data-val") + '">' + $(this).text() + '<span class="glyphicon glyphicon-remove badge"> </span></li>');
                    getprojectid();
                }
            });
            $("#seltxt").keyup(function () {
                var txt = $("#seltxt").val();
                if (txt == "") {
                    $("#ulProject a").show();
                }
                $.map($("#ulProject a"), function (n, i) {
                    if ($(n).text().indexOf(txt) >= 0 || $(n).attr("data-py").indexOf(txt) >= 0) {
                        $(n).show();
                    } else {
                        $(n).hide();
                    }
                });
                var par = $(this).parent();
                if (!par.hasClass("open")) { par.addClass("open"); }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-inline">
        <div class="form-group">
            <label for="txtSearch" class="control-label">搜索条件</label>
            <input type="text" class="form-control" id="txtSearch" placeholder="" />
        </div>
        <input type="button" class="btn btn-info" id="btnSearch" value="搜 索" />
        <%if (((Model.User)Session["user"]).UserName == "youjing"){ %>
        <input id="btnAdd" value="添加" type="button" class="btn btn-success" />
        <%} %>
    </div>
    <hr />
    <div class="row">
        <div class="col-md-12">
            <table class="table table-bordered" id="tblist">
                <thead>
                    <tr>
                        <th style="width:280px;">服务器名称</th>
                        <th>用户名</th>
                        <th>密码</th>
                        <th>IIS管理版本</th>
                        <th>sql版本</th>
                        <th>项目</th>
                        <th>操作系统</th>
                        <th style="width:90px;">服务器类型</th>
                        <%if (((Model.User)Session["user"]).UserName == "youjing"){ %>
                        <th style="width:130px;">操作</th>
                        <%} %>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rpt" runat="server" OnItemCommand="rpt_ItemCommand">
                        <ItemTemplate>
                            <tr>
                                <td data-id="<%#Eval("Id") %>"><%#Eval("ServerName") %></td>
                                <td><%#Eval("UserName") %></td>
                                <td><%#Eval("Password") %></td>
                                <td><%#Eval("IISVersion") %></td>
                                <td><%#Eval("SqlVersion") %></td>
                                <td data-id="<%#Eval("ProjectId") %>"><%#Eval("ProjectName") %></td>
                                <td><%#Eval("OSName") %></td>
                                <td><%#Eval("ServerType") %></td>
                                <%if (((Model.User)Session["user"]).UserName == "youjing")
                                  { %>
                                <td>
                                    <input type="button" class="btn btn-default btnmodify" value="修改" />
                                    <asp:Button runat="server" CssClass="btn btn-default btndel" Text="删除" CommandArgument='<%#Eval("Id") %>' OnClientClick="return window.confirm('确定删除吗?');" CommandName="DeleteServer" />
                                </td>
                                <%} %>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </div>
    <%if (((Model.User)Session["user"]).UserName == "youjing")
      { %>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">服务器管理</h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtServerName">服务器名称</label>
                            <div class="col-md-9">
                                <input class="form-control" id="txtServerName" type="text" value="" runat="server" />
                                <input type="hidden" value="0" id="txtId" class="form-control" runat="server" />
                                <span class="field-validation-valid text-danger" id="spServerName"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtUserName">用户名</label>
                            <div class="col-md-9">
                                <input class="form-control" id="txtUserName" type="text" value="" runat="server" />
                                <span class="field-validation-valid text-danger" id="spUserName"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtPassword">密码</label>
                            <div class="col-md-9">
                                <input class="form-control" id="txtPassword" type="text" value="" runat="server" />
                                <span class="field-validation-valid text-danger" id="spPassword"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtIISVersion">IIS管理版本</label>
                            <div class="col-md-9">
                                <input class="form-control" id="txtIISVersion" type="text" value="" runat="server" />
                                <span class="field-validation-valid text-danger" id="spIISVersion"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtSqlVersion">sql版本</label>
                            <div class="col-md-9">
                                <input class="form-control" id="txtSqlVersion" type="text" value="" runat="server" />
                                <span class="field-validation-valid text-danger" id="spSqlVersion"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtOSName">操作系统</label>
                            <div class="col-md-9">
                                <input class="form-control" id="txtOSName" type="text" value="" runat="server" />
                                <span class="field-validation-valid text-danger" id="spOSName"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtServerType">服务器类型</label>
                            <div class="col-md-9">
                                <label class="checkbox-inline">
                                    <input type="checkbox" value="" id="ckServerType1" />
                                    WEB服务器
                                </label>
                                <label class="checkbox-inline">
                                    <input type="checkbox" value="" id="ckServerType2" />
                                    SQL服务器
                                </label>
                                <input class="form-control" id="txtServerType" type="hidden" value="" runat="server" />
                                <span class="field-validation-valid text-danger" id="spServerType"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="ulProject">项目</label><asp:HiddenField ID="txtProjectId" runat="server" />
                            <div class="col-md-9">
                                <div class="input-group">
                                    <div class="input-group-btn">
                                        <input type="text" class="form-control" id="seltxt" aria-label="..." autocomplete="off" data-toggle="dropdown" />
                                        <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true"><span class="caret"></span></button>
                                        <ul class="dropdown-menu" id="ulProject">
                                            <asp:Repeater runat="server" ID="rptPorject">
                                                <ItemTemplate>
                                                    <li><a href="#" data-val='<%#Eval("Id") %>' data-py='<%#Common.St.GetPY(Eval("Name").ToString()) %>' data-url='<%#Eval("Url") %>'><%#Eval("Name") %></a></li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                </div>
                                <ul class="list-group" id="projectidlist">
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                    <asp:Button runat="server" ID="btnSave" CssClass="btn btn-info" Text="保存" OnClick="btnSave_Click" />
                </div>
            </div>
        </div>
    </div>
    <%} %>
</asp:Content>
