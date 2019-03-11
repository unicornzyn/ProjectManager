<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Statistics1.aspx.cs" Inherits="ProjectManager.Statistics1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.bootcss.com/bootstrap-select/1.13.6/css/bootstrap-select.css" rel="stylesheet">
    <script src="https://cdn.bootcss.com/bootstrap-select/1.13.6/js/bootstrap-select.js"></script>
    <script src="https://cdn.bootcss.com/bootstrap-select/1.13.6/js/i18n/defaults-zh_CN.min.js"></script>
    <script type="text/javascript">
        $(function () {
            var temp = "", j = 0;
            var m1 = 0, m2 = 0, m3 = 0, m4 = 0, m5 = 0, m6 = 0, m7 = 0, m8 = 0, m9 = 0, m10 = 0, m11 = 0, m12 = 0;
            $.map($(".table tbody tr"), function (n, i) {
                var txt = $($(n).find("td")[0]).text();
                m1 += parseInt($($(n).find("td")[2]).text());
                m2 += parseInt($($(n).find("td")[3]).text());
                m3 += parseInt($($(n).find("td")[4]).text());
                m4 += parseInt($($(n).find("td")[5]).text());
                m5 += parseInt($($(n).find("td")[6]).text());
                m6 += parseInt($($(n).find("td")[7]).text());
                m7 += parseInt($($(n).find("td")[8]).text());
                m8 += parseInt($($(n).find("td")[9]).text());
                m9 += parseInt($($(n).find("td")[10]).text());
                m10 += parseInt($($(n).find("td")[11]).text());
                m11 += parseInt($($(n).find("td")[12]).text());
                m12 += parseInt($($(n).find("td")[13]).text());

                if (temp == txt) {
                    $($(n).find("td")[0]).remove();
                    $($($(".table tbody tr")[j]).find("td")[0]).attr("rowspan", parseInt($($($(".table tbody tr")[j]).find("td")[0]).attr("rowspan")) + 1);
                } else {
                    $(n).css("border-top", "solid 2px #ddd");
                    temp = txt; j = i;
                }

            });

            $(".table tbody").append("<tr style='border-top:solid 2px #ddd;'><td>总计</td><td>" + (m1 + m2 + m3 + m4 + m5 + m6 + m7 + m8 + m9 + m10 + m11 + m12) + "</td><td>" + m1 + "</td><td>" + m2 + "</td><td>" + m3 + "</td><td>" + m4 + "</td><td>" + m5 + "</td><td>" + m6 + "</td><td>" + m7 + "</td><td>" + m8 + "</td><td>" + m9 + "</td><td>" + m10 + "</td><td>" + m11 + "</td><td>" + m12 + "</td></tr>");

            $("td[data-info]").click(function () {
                if ($(this).attr("data-info") != "") {
                    $(".modal-body").html($(this).attr("data-info"));
                    $('#myModal').modal();
                }
            });

            $('#selParent').selectpicker({
                'width': '280px'
            });
        });

        function sel() {
            $("#txtProjectParent").val($('#selParent').val());
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-inline">
        <div class="form-group">
            <label for="selYear" class="control-label">年份</label>
            <select id="selYear" class="form-control" runat="server">
            </select>
            &nbsp;&nbsp;
            <label for="txtProjectParent" class="control-label">项目归属</label>
            <input type="hidden" id="txtProjectParent" runat="server" />
            <select id="selParent" class="form-control selectpicker" runat="server" multiple="true" size="1">
            </select>
            &nbsp;&nbsp;
            <label for="txtProject" class="control-label">项目名称</label>
            <input type="text" class="form-control" id="txtProject" runat="server" />
        </div>
        <asp:Button runat="server" ID="btnSearch" Text="搜 索" CssClass="btn btn-info" OnClick="btnSearch_Click" OnClientClick="sel()" />
        <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="导出Excel" CssClass="btn btn-danger" />
    </div>
    <hr />
    <div class="row">
        <div class="col-md-12">
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>项目归属</th>
                        <th>项目名称</th>
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
                                <td rowspan="1" style="vertical-align: middle;"><%#Eval("PName") %></td>
                                <td><%#Eval("Name") %></td>
                                <td data-info='<%#Eval("S1") %>'><%#Eval("M1") %></td>
                                <td data-info='<%#Eval("S2") %>'><%#Eval("M2") %></td>
                                <td data-info='<%#Eval("S3") %>'><%#Eval("M3") %></td>
                                <td data-info='<%#Eval("S4") %>'><%#Eval("M4") %></td>
                                <td data-info='<%#Eval("S5") %>'><%#Eval("M5") %></td>
                                <td data-info='<%#Eval("S6") %>'><%#Eval("M6") %></td>
                                <td data-info='<%#Eval("S7") %>'><%#Eval("M7") %></td>
                                <td data-info='<%#Eval("S8") %>'><%#Eval("M8") %></td>
                                <td data-info='<%#Eval("S9") %>'><%#Eval("M9") %></td>
                                <td data-info='<%#Eval("S10") %>'><%#Eval("M10") %></td>
                                <td data-info='<%#Eval("S11") %>'><%#Eval("M11") %></td>
                                <td data-info='<%#Eval("S12") %>'><%#Eval("M12") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>

                </tbody>
            </table>
        </div>
    </div>

    <div class="modal fade bs-example-modal-sm" id="myModal" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="关闭"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title" id="mySmallModalLabel">项目上线明细</h4>
                </div>
                <div class="modal-body">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
