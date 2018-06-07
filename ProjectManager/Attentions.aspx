<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Attentions.aspx.cs" Inherits="ProjectManager.Attentions" %>
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
                $("#txtRemark").val("");                
                $("#projectidlist").empty();                
                $("#txtProjectId").val("");
            });

            $(".btnmodify").click(function () {
                $("#myModal").modal('show');
                $("#txtId").val($($(this).parent().parent().find("td").get(0)).attr("data-id"));
                $("#txtRemark").val($($(this).parent().parent().find("td").get(1)).text());
                var ids = $($(this).parent().parent().find("td").get(0)).attr("data-projectid");
                var name = $($(this).parent().parent().find("td").get(0)).text();
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
            <input type="text" class="form-control" id="txtSearch" placeholder="" runat="server" />
        </div>
        <asp:Button runat="server" CssClass="btn btn-info" ID="btnSearch" Text="搜 索" OnClick="btnSearch_Click" />
        <%if (((Model.User)Session["user"]).RoleType < 3)
          { %>
        <input id="btnAdd" value="添加" type="button" class="btn btn-success" />
        <%} %>
    </div>
    <hr />
    <div class="row">
        <div class="col-md-12">
            <table class="table table-bordered" id="tblist">
                <thead>
                    <tr>
                        <th style="width:280px;">项目</th>
                        <th>注意事项</th>                       
                        <%if (((Model.User)Session["user"]).RoleType < 3){ %>
                        <th style="width:130px;">操作</th>
                        <%} %>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rpt" runat="server" OnItemCommand="rpt_ItemCommand">
                        <ItemTemplate>
                            <tr>
                                <td data-id="<%#Eval("Id") %>" data-projectid="<%#Eval("ProjectId") %>"><%#Eval("ProjectName") %></td>
                                <td><%#Eval("Remark") %></td>                                
                                <%if (((Model.User)Session["user"]).RoleType < 3)
                                  { %>
                                <td>
                                    <input type="button" class="btn btn-default btnmodify" value="修改" />
                                    <asp:Button runat="server" CssClass="btn btn-default btndel" Text="删除" CommandArgument='<%#Eval("Id") %>' OnClientClick="return window.confirm('确定删除吗?');" CommandName="DeleteAttentions" />
                                </td>
                                <%} %>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <nav>
                <ul class="pager">
                    <li>
                        <asp:LinkButton runat="server" ID="lbFirst" Text="首 页" OnClick="lbFirst_Click"></asp:LinkButton></li>
                    <li>
                        <asp:LinkButton runat="server" ID="lbPrevious" Text="上一页" OnClick="lbPrevious_Click"></asp:LinkButton></li>
                    <li>
                        <span>
                            <span aria-hidden="true" runat="server" id="spPage">0/0</span>
                        </span>
                    </li>
                    <li>
                        <asp:LinkButton runat="server" ID="lbNext" Text="下一页" OnClick="lbNext_Click"></asp:LinkButton></li>
                    <li>
                        <asp:LinkButton runat="server" ID="lbLast" Text="末 页" OnClick="lbLast_Click"></asp:LinkButton></li>
                </ul>
            </nav>
        </div>
    </div>
    <%if (((Model.User)Session["user"]).RoleType < 3)
      { %>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">注意事项</h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtRemark">注意事项</label>
                            <div class="col-md-9">
                                <textarea rows="12" class="form-control" id="txtRemark" type="text" value="" runat="server" ></textarea>
                                <input type="hidden" value="0" id="txtId" class="form-control" runat="server" />
                                <span class="field-validation-valid text-danger" id="spRemark"></span>
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
