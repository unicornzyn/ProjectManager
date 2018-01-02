<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModifyPassword.aspx.cs" Inherits="ProjectManager.ModifyPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function valid() {
            $("#spPassword1,#spPassword2").text("");
            if ($("#Password1").val() == "") {
                $("#spPassword1").text("请输入新密码"); return false;
            }
            if ($("#Password2").val() == "") {
                $("#spPassword2").text("请再次输入新密码"); return false;
            }
            if ($("#Password1").val() != $("#Password2").val()) {
                $("#spPassword2").text("两次密码输入不一致"); return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <h2>修改密码</h2>
        <div class="row">
            <div class="col-md-12">
                <section class="form-horizontal">
                    <div class="form-group">
                        <label class="col-md-2 control-label" for="Password1">输入新密码</label>
                        <div class="col-md-10">
                            <input class="form-control" id="Password1" name="Password1" runat="server" type="password" />
                            <span class="field-validation-valid text-danger" id="spPassword1"></span>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-md-2 control-label" for="Password2">新密码确认</label>
                        <div class="col-md-10">
                            <input class="form-control" id="Password2" name="Password2" runat="server" type="password" />
                            <span class="field-validation-valid text-danger" id="spPassword2"></span>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" Text="修改密码" ID="btnSave" OnClick="btnSave_Click" OnClientClick="return valid();" CssClass="btn btn-default" />
                        </div>
                    </div>
                </section>
            </div>
        </div>
</asp:Content>
