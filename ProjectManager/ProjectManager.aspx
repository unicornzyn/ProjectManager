<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProjectManager.aspx.cs" ValidateRequest="false" Inherits="ProjectManager.ProjectManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            var k = location.search.replace('?k=', '');
            if (k.length > 0) {
                if ($("#txtSearch").val() == "") {
                    $("#txtSearch").val(unescape(k));
                    $("#btnSearch").trigger('click');
                }
                
            }
            if ($("#hidUserRole").val() != "1") {
                $("#btnAdd,.btnmodify,.btndel").hide();
            }
            $("#btnAdd").click(function () {
                $("#myModal").modal('show');
                $("#txtId").val("0");
                $("#txtName").val("");
                $("#txtUrl").val("");
                $("#selParent").val("0");
                $("#txtTestUrl").val("");
                $("#txtSiteFileName").val("");
                $("#txtDatabaseName").val("");
                $("#txtTestUserName").val("");
                $("#txtTestPassword").val("");
                $("#txtRemark").val("");
                $("#chkIsShow").attr("checked", true);
            });


            $(".btnmodify").click(function () {
                $("#myModal").modal('show');
                $("#txtName").val($($(this).parent().parent().find("td").get(0)).text());
                $("#txtUrl").val($($(this).parent().parent().find("td").get(1)).text());
                $("#txtTestUrl").val($($(this).parent().parent().find("td").get(2)).text());
                $("#txtDatabaseName").val($($(this).parent().parent().find("td").get(3)).text());
                $("#txtTestUserName").val($($(this).parent().parent().find("td").get(4)).text());
                $("#txtTestPassword").val($($(this).parent().parent().find("td").get(5)).text());

                $("#txtId").val($($(this).parent().parent().find("td").get(0)).attr("data-id"));                
                $("#selParent").val($($(this).parent().parent().find("td").get(0)).attr("data-pid"));              
                $("#txtSiteFileName").val($($(this).parent().parent().find("td").get(0)).attr("data-sitefilename"));
                $("#txtRemark").val($($(this).parent().parent().find("td").get(0)).attr("data-remark"));
                $("#chkIsShow").attr("checked", $($(this).parent().parent().find("td").get(0)).attr("data-isshow") == "是" ? true : false);

            });

            $(".btndetail").click(function () {
                $("#myModalDetail").modal('show');
                $("#txtNameDetail").val($($(this).parent().parent().find("td").get(0)).text());
                $("#txtUrlDetail").val($($(this).parent().parent().find("td").get(1)).text());
                $("#txtTestUrlDetail").val($($(this).parent().parent().find("td").get(2)).text());
                $("#txtDatabaseNameDetail").val($($(this).parent().parent().find("td").get(3)).text());
                $("#txtTestUserNameDetail").val($($(this).parent().parent().find("td").get(4)).text());
                $("#txtTestPasswordDetail").val($($(this).parent().parent().find("td").get(5)).text());

                //$("#txtId").val($($(this).parent().parent().find("td").get(0)).attr("data-id"));
                $("#selParentDetail").val($($(this).parent().parent().find("td").get(0)).attr("data-pid"));
                $("#txtSiteFileNameDetail").val($($(this).parent().parent().find("td").get(0)).attr("data-sitefilename"));
                $("#txtRemarkDetail").val($($(this).parent().parent().find("td").get(0)).attr("data-remark"));
                $("#chkIsShowDetail").val($($(this).parent().parent().find("td").get(0)).attr("data-isshow"));

            });

        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-inline">
        <div class="form-group">
            <label for="txtSearch" class="control-label">搜索条件</label>
            <input type="text" class="form-control" id="txtSearch" runat="server" placeholder="" />            
        </div>
        <asp:Button runat="server" CssClass="btn btn-info" ID="btnSearch" Text="搜 索" OnClick="btnSearch_Click" />
        <input id="btnAdd" value="添加" type="button" class="btn btn-success" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="导出Excel" CssClass="btn btn-danger" />
    </div>
    <hr />
    <div class="row">
        <div class="col-md-12">
            <table class="table table-bordered">
                <thead>
                    <tr class="info">
                        <th style="min-width: 200px;">名称</th>
                        <th>域名</th>
                        <th>测试地址</th>
                        <th>数据库</th>
                        <th style="min-width: 100px;">测试用户名</th>
                        <th style="min-width: 60px;">密码</th>                      
                        <th style="min-width: 190px;">操作</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rpt" runat="server" OnItemCommand="rpt_ItemCommand">
                        <ItemTemplate>
                            <tr>
                                <td data-id='<%#Eval("Id") %>' data-pid='<%#Eval("ParentId") %>' data-sitefilename='<%#Eval("SiteFileName") %>' data-remark='<%#Eval("Remark") %>' data-isshow='<%#Eval("IsShow").ToString()=="0"?"否":"是" %>'><%#Eval("Name") %></td>
                                <td><%#Eval("Url") %></td>
                                <td><%#Eval("TestUrl") %></td>
                                <td><%#Eval("DatabaseName") %></td>
                                <td><%#Eval("TestUserName") %></td>
                                <td><%#Eval("TestPassword") %></td>
                                <td>
                                    <input type="button" class="btn btn-default btndetail" value="查看" />
                                    <input type="button" class="btn btn-default btnmodify" value="修改" />
                                    <asp:Button runat="server" Text="删除" OnClientClick="return window.confirm('确定删除吗?');" CssClass="btn btn-default btndel" CommandArgument='<%#Eval("Id") %>' CommandName="DeleteProject" />
                                </td>
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

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">项目管理</h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtName">名称</label>
                            <div class="col-md-9">
                                <input class="form-control" id="txtName" type="text" value="" runat="server" />
                                <input type="hidden" value="0" id="txtId" class="form-control" runat="server" />
                                <span class="field-validation-valid text-danger" id="spName"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtUrl">域名</label>
                            <div class="col-md-9">
                                <input class="form-control" id="txtUrl" type="text" value="" runat="server" />
                                <span class="field-validation-valid text-danger" id="spUrl"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="chkIsShow">是否显示</label>
                            <div class="col-md-9">
                                <div class="checkbox">
                                    <label>
                                        <input id="chkIsShow" type="checkbox" value="" runat="server" />勾选显示</label>
                                </div>
                                <span class="field-validation-valid text-danger" id="spIsShow"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="selParent">父级</label>
                            <div class="col-md-9">
                                <select id="selParent" class="form-control" runat="server">
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtTestUrl">测试地址</label>
                            <div class="col-md-9">
                                <input class="form-control" id="txtTestUrl" type="text" value="" runat="server" />
                                <span class="field-validation-valid text-danger" id="spTestUrl"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtSiteFileName">网站文件名</label>
                            <div class="col-md-9">
                                <input class="form-control" id="txtSiteFileName" type="text" value="" runat="server" />
                                <span class="field-validation-valid text-danger" id="spSiteFileName"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtDatabaseName">数据库</label>
                            <div class="col-md-9">
                                <input class="form-control" id="txtDatabaseName" type="text" value="" runat="server" />
                                <span class="field-validation-valid text-danger" id="spDatabaseName"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtTestUserName">测试用户名</label>
                            <div class="col-md-9">
                                <input class="form-control" id="txtTestUserName" type="text" value="" runat="server" />
                                <span class="field-validation-valid text-danger" id="spTestUserName"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtTestPassword">密码</label>
                            <div class="col-md-9">
                                <input class="form-control" id="txtTestPassword" type="text" value="" runat="server" />
                                <span class="field-validation-valid text-danger" id="spTestPassword"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtRemark">备注</label>
                            <div class="col-md-9">
                                <textarea id="txtRemark" class="form-control" runat="server"></textarea>
                                <span class="field-validation-valid text-danger" id="spRemark"></span>
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

    <div class="modal fade" id="myModalDetail" tabindex="-1" role="dialog" aria-labelledby="myModalLabelDetail" data-backdrop="static">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabelDetail">项目详细信息</h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtNameDetail">名称</label>
                            <div class="col-md-9">
                                <input class="form-control" id="txtNameDetail" type="text" value="" readonly="true"/>                                                                
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtUrlDetail">域名</label>
                            <div class="col-md-9">
                                <input class="form-control" id="txtUrlDetail" type="text" value=""  readonly="true" />
                              
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="chkIsShowDetail">是否显示</label>
                            <div class="col-md-9">
                                 <input class="form-control" id="chkIsShowDetail" type="text" value=""  readonly="true" />
                               
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="selParentDetail">父级</label>
                            <div class="col-md-9">
                                <input class="form-control" id="selParentDetail" type="text" value=""  readonly="true" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtTestUrlDetail">测试地址</label>
                            <div class="col-md-9">
                                <input class="form-control" id="txtTestUrlDetail" type="text" value=""  readonly="true"  />
                               
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtSiteFileNameDetail">网站文件名</label>
                            <div class="col-md-9">
                                <input class="form-control" id="txtSiteFileNameDetail" type="text" value=""  readonly="true"  />
                               
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtDatabaseNameDetail">数据库</label>
                            <div class="col-md-9">
                                <input class="form-control" id="txtDatabaseNameDetail" type="text" value=""  readonly="true"  />
                        
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtTestUserNameDetail">测试用户名</label>
                            <div class="col-md-9">
                                <input class="form-control" id="txtTestUserNameDetail" type="text" value="" readonly="true"  />
                             
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtTestPasswordDetail">密码</label>
                            <div class="col-md-9">
                                <input class="form-control" id="txtTestPasswordDetail" type="text" value="" readonly="true"  />
                             
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="txtRemarkDetail">备注</label>
                            <div class="col-md-9">
                                <textarea id="txtRemarkDetail" class="form-control" readonly="true" ></textarea>
                              
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

