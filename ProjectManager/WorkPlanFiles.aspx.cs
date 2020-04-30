using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManager
{
    public partial class WorkPlanFiles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            int id = Common.St.ToInt32(Request.QueryString["id"]);
            rpt.DataSource = DAL.WorkPlanFileRule.GetList(id);
            rpt.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (upFilePath.HasFile)
            {
                int id = Common.St.ToInt32(Request.QueryString["id"]);
                string filename = System.IO.Path.GetFileNameWithoutExtension(upFilePath.FileName);
                string filepath = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(upFilePath.FileName);
                upFilePath.SaveAs(Server.MapPath("~/uploads/" + filepath));
                DAL.WorkPlanFileRule.Add(id, filename, filepath);
                BindData();
            }
        }

        protected void btnDownLoad_Command(object sender, CommandEventArgs e)
        {
            int id = Common.St.ToInt32(e.CommandArgument);
            var o = DAL.WorkPlanFileRule.Get(id);

            string strFilePath = Server.MapPath("~/uploads/") + o.FilePath;//服务器文件路径
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(strFilePath);
            Response.Clear();
            Response.Charset = "GB2312";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(o.FileName + System.IO.Path.GetExtension(o.FilePath)));
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.ContentType = "application/x-bittorrent";
            Response.WriteFile(fileInfo.FullName);
            Response.End();
        }

        protected void btnDel_Command(object sender, CommandEventArgs e)
        {
            int id = Common.St.ToInt32(e.CommandArgument);
            DAL.WorkPlanFileRule.Delete(id);
            BindData();
        }

        /// <summary>
        /// 根据path获取对应的预览地址
        /// </summary>
        /// <param name="path"></param>
        protected string GetPreviewUrl(string path)
        {
            //Eval("FilePath").ToString()
            //
            var ext = System.IO.Path.GetExtension(path);
            if(Array.IndexOf(new string[] {".docx", ".doc", ".xls",".xlsx",".ppt", ".pptx" }, ext)>=0)
            {
                return "https://view.officeapps.live.com/op/view.aspx?src=http://projectmanager.91huayi.com/uploads/" + path;
            }
            else
            {
                return "http://projectmanager.91huayi.com/uploads/" + path;
            }
            
        }
    }
}