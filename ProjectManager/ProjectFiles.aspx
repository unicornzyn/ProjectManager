<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectFiles.aspx.cs" Inherits="ProjectManager.ProjectFiles" %>

<!DOCTYPE html>

<html lang="zh-CN">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">
    <title>项目附件管理</title>
    <link href="css/bootstrap.css" rel="stylesheet" />
    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="http://cdn.bootcss.com/html5shiv/3.7.0/html5shiv.js"></script>
      <script src="http://cdn.bootcss.com/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
    <style type="text/css">
        body,a{font-size: 14px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
            <div class="sub-header">
                <asp:FileUpload runat="server" ID="upFilePath" Style="float: left;" CssClass="btn" />
                <asp:Button runat="server" CssClass="btn btn-success" Text="上传附件" style="float: left;" ID="btnSave" OnClick="btnSave_Click" />
            </div>
            <hr style="clear: both;" />
            <h5 class="sub-header">附件列表</h5>
            <div class="table-responsive">
                <table class="table">
                    <tbody>
						<asp:Repeater runat="server" ID="rpt">
                        <ItemTemplate>
                        <tr>
                            <td><%#Eval("FileName") + System.IO.Path.GetExtension(Eval("FilePath").ToString()) %></td>
                            <td style="width:60px;"><a href="<%#GetPreviewUrl(Eval("FilePath").ToString()) %>"" target="_blank">预览</a></td>
                            <td style="width:60px;"><asp:LinkButton runat="server" ID="btnDownLoad" Text="下载" OnCommand="btnDownLoad_Command" CommandArgument='<%#Eval("Id") %>'></asp:LinkButton></td>
                            <td style="width:60px;"><asp:LinkButton runat="server" ID="btnDel" OnCommand="btnDel_Command" OnClientClick="return window.confirm('确定删除吗?');" Text="删除" CommandArgument='<%#Eval("Id") %>'></asp:LinkButton></td>
                        </tr>
						</ItemTemplate>
						</asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
