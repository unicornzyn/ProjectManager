<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" ValidateRequest="false"  Inherits="ProjectManager.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="datepicker/WdatePicker.js"></script>
    <script type="text/javascript">
        $(function () {
            if ($("#hidUserRole").val() != "1" && $("#hidUserRole").val() != "2") {
                $("#btnAdd,.btnmodify,.btndel").hide();
            }

            $("#btnAdd").click(function () {
                $("#myModal").modal('show');
                $("#ulProject a").show();
                $("#seltxt").val("");
                $("#spProjectId").text("");
                $("#ProjectId").val("");                
                $("#WorkRemark").val("");
                $("#RealStartTime").val("");
                $("#RealEndTime").val("");


                $("#hId").val("");
                $("#SheepNo").val("");
                $("#StartTime").val("");
                $("#EndTime").val("");
                $("#PublishTime").val("");
                $("#NeederId").val("0");
                $("#Remark").val("");
            });
;

            $(".btnmodify").click(function () {
                $("#myModal").modal('show');
                $("#ulProject a").show();
                $("#spProjectId").text("");
                
                $("#ProjectId").val($($(this).parent().parent().find("td").get(0)).attr("data-ProjectId"));
                $("#seltxt").val($($(this).parent().parent().find("td").get(0)).text());
                $("#WorkRemark").val($($(this).parent().parent().find("td").get(1)).find('div').html().replace(/(<br>)/, "\r\n"));
                $("#RealStartTime").val($($(this).parent().parent().find("td").get(2)).text());
                $("#RealEndTime").val($($(this).parent().parent().find("td").get(3)).text());


                $("#hId").val($($(this).parent().parent().find("td").get(0)).attr("data-Id"));
                $("#SheepNo").val($($(this).parent().parent().find("td").get(0)).attr("data-SheepNo"));
                $("#StartTime").val($($(this).parent().parent().find("td").get(0)).attr("data-StartTime"));
                $("#EndTime").val($($(this).parent().parent().find("td").get(0)).attr("data-EndTime"));
                $("#PublishTime").val($($(this).parent().parent().find("td").get(0)).attr("data-PublishTime"));               
                $("#Remark").val($($(this).parent().parent().find("td").get(0)).attr("data-Remark"));
                $("#NeederId").val($($(this).parent().parent().find("td").get(0)).attr("data-NeederId"));
                $("#PlanType").val($($(this).parent().parent().find("td").get(0)).attr("data-PlanType"));
                $("#State").val($($(this).parent().parent().find("td").get(0)).attr("data-State"));
            });

            $("#ulProject a").click(function () {
                $("#ProjectId").val($(this).attr("data-val"));
                $("#seltxt").val($(this).text());
                $("#SheepNo").val($(this).attr("data-url"));
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
                if (!par.hasClass("open")) { par.addClass("open");}
            });

            $(".btndetail").click(function () {
                $("#myModalDetail").modal('show');
                

                $("#seltxtDetail").val($($(this).parent().parent().find("td").get(0)).text());
                $("#WorkRemarkDetail").val($($(this).parent().parent().find("td").get(1)).find('div').html().replace(/(<br>)/, "\r\n"));
                $("#RealStartTimeDetail").val($($(this).parent().parent().find("td").get(2)).text());
                $("#RealEndTimeDetail").val($($(this).parent().parent().find("td").get(3)).text());
                $("#SheepNoDetail").val($($(this).parent().parent().find("td").get(0)).attr("data-SheepNo"));
                $("#PlanTypeDetail").val($($(this).parent().parent().find("td").get(0)).attr("data-PlanTypeStr"));
                $("#StartTimeDetail").val($($(this).parent().parent().find("td").get(0)).attr("data-StartTime"));
                $("#EndTimeDetail").val($($(this).parent().parent().find("td").get(0)).attr("data-EndTime"));
                $("#PublishTimeDetail").val($($(this).parent().parent().find("td").get(0)).attr("data-PublishTime"));
                $("#StateDetail").val($($(this).parent().parent().find("td").get(5)).text());
                $("#NeederIdDetail").val($($(this).parent().parent().find("td").get(6)).text());
                $("#RemarkDetail").val($($(this).parent().parent().find("td").get(0)).attr("data-Remark"));

            });
        });

        function valid() {
            $("#spProjectId").text("");
            if ($("#ProjectId").val() == "") {
                $("#spProjectId").text("请选择项目!"); return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-inline">
        <div class="form-group">
            <label for="selYear" class="control-label">搜索条件</label>
            <select id="selYear" class="form-control" runat="server">
            </select>
            <label class="control-label">年</label>
            <select id="selMonth" class="form-control" runat="server">
                <option value="0">0</option>
                <option value="1">1</option>
                <option value="2">2</option>
                <option value="3">3</option>
                <option value="4">4</option>
                <option value="5">5</option>
                <option value="6">6</option>
                <option value="7">7</option>
                <option value="8">8</option>
                <option value="9">9</option>
                <option value="10">10</option>
                <option value="11">11</option>
                <option value="12">12</option>
            </select>
            <label class="control-label">月</label>
            &nbsp;&nbsp;
            <label for="selNeeder" class="control-label">项目负责人</label>&nbsp;
            <select id="selNeeder" class="form-control" runat="server"></select>
            &nbsp;&nbsp;
            <label for="selWorkState" class="control-label">任务状态</label>
            <select id="selWorkState" class="form-control" runat="server">
                <option value="-1">全部</option>
                <option value="0">未开始</option>
                <option value="1">进行中</option>
                <option value="2">完成</option>
            </select>
            &nbsp;&nbsp;
            <label for="txtProjectSearch" class="control-label">项目</label>
            <input type="text" class="form-control" id="txtProjectSearch" runat="server" />
        </div>
        <asp:Button runat="server" ID="btnSearch" CssClass="btn btn-info" Text="搜 索" OnClick="btnSearch_Click" />
        <input id="btnAdd" value="添 加" type="button" class="btn btn-success" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="导出Excel" CssClass="btn btn-danger" />
    </div>
    <hr />
    <div class="row">
        <div class="col-md-12">
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th style="min-width:150px;">项目</th>
                        <th>工作</th>
                        <th style="width:103px;">实际开始时间</th>
                        <th style="width:103px;">实际结束时间</th>
                        <th style="width:103px;">上线时间</th>
                        <th style="width:70px;">任务状态</th>
                        <th style="width:70px;">项目负责人</th>
                        <th style="width: 190px;">操作</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="rpt" OnItemCommand="rpt_ItemCommand">
                        <ItemTemplate>
                            <tr class='<%#GetTrClass(Eval("State").ToString()) %>'>
                                <td data-Id='<%#Eval("Id") %>' data-SheepNo='<%#Eval("SheepNo") %>' data-ProjectId='<%#Eval("ProjectId") %>' data-PlanType='<%#Eval("PlanType") %>' data-PlanTypeStr='<%#Eval("PlanTypeStr") %>' data-StartTime='<%#Common.St.ToDateTimeString(Eval("StartTime"),"yyyy-MM-dd") %>' data-EndTime='<%#Common.St.ToDateTimeString(Eval("EndTime"),"yyyy-MM-dd") %>' data-PublishTime='<%#Common.St.ToDateTimeString(Eval("PublishTime"),"yyyy-MM-dd") %>' data-State='<%#Eval("State") %>' data-NeederId='<%#Eval("NeederId") %>' data-Remark='<%#Eval("Remark") %>'><%#Eval("Project.Name") %></td>
                                <td><div style="width:100%;max-height:100px;overflow-y:auto;"><%#Eval("WorkRemark").ToString().Replace("\r\n","<br>") %></div></td>
                                <td><%#Common.St.ToDateTimeString(Eval("RealStartTime"),"yyyy-MM-dd") %></td>
                                <td><%#Common.St.ToDateTimeString(Eval("RealEndTime"),"yyyy-MM-dd") %></td>
                                <td><%#Common.St.ToDateTimeString(Eval("PublishTime"),"yyyy-MM-dd") %></td>
                                <td><%#Eval("StateStr") %></td>
                                <td><%#Eval("Needer.RealName") %></td>
                                <td>
                                    <input type="button" class="btn btn-default btndetail" value="查看" />
                                    <input type="button" class="btn btn-default btnmodify" value="修改" />
                                    <asp:Button runat="server" CssClass="btn btn-default" Text="删除" CommandArgument='<%#Eval("Id") %>' OnClientClick="return window.confirm('确定删除吗?');" CommandName="DeleteWorkPlan" />
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
                    <h4 class="modal-title" id="myModalLabel">工作计划管理</h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="seltxt">项目</label>
                            <asp:HiddenField runat="server" ID="ProjectId" />
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
                                <span class="field-validation-valid text-danger" id="spProjectId"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="SheepNo">工单编号/域名</label>
                            <div class="col-md-9">
                                <asp:HiddenField runat="server" ID="hId" />
                                <input class="form-control" id="SheepNo" name="SheepNo" runat="server" type="text" />
                                <span class="field-validation-valid text-danger" id="spSheepNo"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="WorkRemark">工作</label>
                            <div class="col-md-9">
                                <textarea id="WorkRemark" name="WorkRemark" class="form-control" runat="server"></textarea>
                                <span class="field-validation-valid text-danger" id="spWorkRemark"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="PlanType">插入/正常</label>
                            <div class="col-md-9">
                                <select class="form-control" id="PlanType" name="PlanType" runat="server">
                                    <option value="1">正常</option>
                                    <option value="2">插入</option>
                                </select>
                                <span class="field-validation-valid text-danger" id="spPlanType"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="StartTime">计划开始时间</label>
                            <div class="col-md-9">
                                <input class="form-control" id="StartTime" name="StartTime" onclick="WdatePicker()" runat="server" type="text"  autocomplete="off"/>
                                <span class="field-validation-valid text-danger" id="spStartTime"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="EndTime">计划结束时间</label>
                            <div class="col-md-9">
                                <input class="form-control" id="EndTime" name="EndTime" onclick="WdatePicker()" runat="server" type="text"  autocomplete="off"/>
                                <span class="field-validation-valid text-danger" id="spEndTime"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="RealStartTime">实际开始时间</label>
                            <div class="col-md-9">
                                <input class="form-control" id="RealStartTime" name="RealStartTime" onclick="WdatePicker()" runat="server" type="text"  autocomplete="off"/>
                                <span class="field-validation-valid text-danger" id="spRealStartTime"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="RealEndTime">实际结束时间</label>
                            <div class="col-md-9">
                                <input class="form-control" id="RealEndTime" name="RealEndTime" onclick="WdatePicker({ onpicked: function () { $('#PublishTime').val($('#RealEndTime').val());}})" runat="server" type="text"  autocomplete="off"/>
                                <span class="field-validation-valid text-danger" id="spRealEndTime"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="PublishTime">上线时间</label>
                            <div class="col-md-9">
                                <input class="form-control" id="PublishTime" name="PublishTime" onclick="WdatePicker()" runat="server" type="text"  autocomplete="off"/>
                                <span class="field-validation-valid text-danger" id="spPublishTime"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="State">任务状态</label>
                            <div class="col-md-9">
                                <select class="form-control" id="State" name="State" runat="server">
                                    <option value="0">未开始</option>
                                    <option value="1">进行中</option>
                                    <option value="2">完成</option>
                                </select>
                                <span class="field-validation-valid text-danger" id="spState"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="NeederId">项目负责人</label>
                            <div class="col-md-9">
                                <select class="form-control" id="NeederId" name="NeederId" runat="server">
                                </select>
                                <span class="field-validation-valid text-danger" id="spNeederId"></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="Remark">备注</label>
                            <div class="col-md-9">
                                <textarea id="Remark" name="Remark" class="form-control" runat="server"></textarea>
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



    <div class="modal fade" id="myModalDetail" tabindex="-1" role="dialog" aria-labelledby="myModalLabelDetail" data-backdrop="static">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabelDetail">工作计划详细信息</h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="seltxtDetail">项目</label>
                            <div class="col-md-9">
                                <input class="form-control" id="seltxtDetail" type="text" value="" readonly="true"/>            
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="SheepNoDetail">工单编号/域名</label>
                            <div class="col-md-9">
                                <input class="form-control" id="SheepNoDetail" type="text" value="" readonly="true"/>      
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="WorkRemarkDetail">工作</label>
                            <div class="col-md-9">
                                <textarea id="WorkRemarkDetail" class="form-control" readonly="true" ></textarea>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="PlanTypeDetail">插入/正常</label>
                            <div class="col-md-9">
                                <input class="form-control" id="PlanTypeDetail" type="text" value="" readonly="true"/>  
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="StartTimeDetail">计划开始时间</label>
                            <div class="col-md-9">
                                <input class="form-control" id="StartTimeDetail" type="text" value="" readonly="true"/>  
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="EndTimeDetail">计划结束时间</label>
                            <div class="col-md-9">
                                <input class="form-control" id="EndTimeDetail" type="text" value="" readonly="true"/>  
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="RealStartTimeDetail">实际开始时间</label>
                            <div class="col-md-9">
                                <input class="form-control" id="RealStartTimeDetail" type="text" value="" readonly="true"/>  
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="RealEndTimeDetail">实际结束时间</label>
                            <div class="col-md-9">
                                <input class="form-control" id="RealEndTimeDetail" type="text" value="" readonly="true"/>  
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="PublishTimeDetail">上线时间</label>
                            <div class="col-md-9">
                                <input class="form-control" id="PublishTimeDetail" type="text" value="" readonly="true"/>  
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="StateDetail">任务状态</label>
                            <div class="col-md-9">
                                <input class="form-control" id="StateDetail" type="text" value="" readonly="true"/>  
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="NeederIdDetail">项目负责人</label>
                            <div class="col-md-9">
                                 <input class="form-control" id="NeederIdDetail" type="text" value="" readonly="true"/>  
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3 control-label" for="RemarkDetail">备注</label>
                            <div class="col-md-9">
                                <input class="form-control" id="RemarkDetail" type="text" value="" readonly="true"/>  
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                    <asp:Button runat="server" ID="Button1" Text="保存" CssClass="btn btn-info" OnClick="btnSave_Click" OnClientClick="return valid();" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
