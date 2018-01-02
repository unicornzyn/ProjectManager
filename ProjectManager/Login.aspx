<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ProjectManager.Login" %>

<!DOCTYPE html>

<html lang="en">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">
    <title>登录</title>
    <!-- Bootstrap core CSS -->
    <link href="css/bootstrap.css" rel="stylesheet">

    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="http://cdn.bootcss.com/html5shiv/3.7.0/html5shiv.js"></script>
      <script src="http://cdn.bootcss.com/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
    <link href="css/Site.css" rel="stylesheet" />
    <script src="js/jquery-1.10.2.js"></script>
    <script type="text/javascript">
        function valid() {
            $("#spUserName,#spPassword").text("");
            if ($.trim($("#UserName").val()) == "") {
                $("#spUserName").text("用户名不能为空"); return false;
            }
            if ($("#Password").val() == "") {
                $("#spPassword").text("密码不能为空"); return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <div class="container">
        <div class="modal show">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h1 class="text-center text-primary">工作计划管理</h1>
                    </div>
                    <div class="modal-body">
                        <form class="form-horizontal" runat="server">
                            <div class="validation-summary-errors text-danger" runat="server" id="divError">
                                <ul>
                                    <li>用户名或密码错误。</li>
                                </ul>
                            </div>
                            <div class="form-group">
                                <label class="col-md-2 control-label" for="UserName">用户名</label>
                                <div class="col-md-10">
                                    <input class="form-control" id="UserName" name="UserName" runat="server" type="text" value="" />
                                    <span class="field-validation-valid text-danger" id="spUserName"></span>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-2 control-label" for="Password">密码</label>
                                <div class="col-md-10">
                                    <input class="form-control" id="Password" name="Password" runat="server" type="password" />
                                    <span class="field-validation-valid text-danger" id="spPassword"></span>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-offset-2 col-md-10">
                                    <asp:Button runat="server" Text="登录" ID="btnLogin" OnClick="btnLogin_Click" OnClientClick="return valid();" CssClass="btn btn-lg btn-primary btn-block" />
                                </div>
                            </div>
                        </form>
                    </div>
                    <div class="modal-footer"></div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
