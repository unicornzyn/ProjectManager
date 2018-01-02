/*----------------------------------------------------------------
// Copyright (C) 2010 盛拓传媒 
// 文件名：St.cs
// 文件功能描述：上传文件类
//----------------------------------------------------------------*/
using System;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// ImageUploader 的摘要说明。
    /// 具有定高定宽并且将多余的宽度剪掉的功能
    /// </summary>
    public class UploadHelper
    {

        private string m_FullImgFilePath;
        private HttpPostedFile m_PostedFile;
        private string m_ImageSn;
        private string m_RootPath;
        private int[,] m_ThumbnailAry;
        private bool[] m_AddStampAry;
        private string m_LegalExtensions;
        private int m_MaxSize;
        private string m_SaveExtension = "jpg";
        private string m_ErrMsg = String.Empty;
        private string m_ImageExtension = string.Empty;
        private bool m_SaveOx0asOriginal = false;
        private bool[] m_FixedWidthAry;
        private bool[] m_FixedAndCutHeightAry;//定高定宽并且将多余的宽度剪掉
        private bool m_isFenBao = false;//是否分包
        public bool isFenBao
        {
            set
            {
                this.m_isFenBao = value;
            }
            get { return this.m_isFenBao; }
        }
        public string FullImgFilePath
        {
            set
            {
                this.m_FullImgFilePath = value;
            }
        }
        public string LegalExtensions
        {
            get { return this.m_LegalExtensions; }
            set
            {
                if (value != null)
                {
                    this.m_LegalExtensions = value.ToLower();
                }
            }
        }
        public string SaveExtension
        {
            set { this.m_SaveExtension = value; }
        }
        public string ErrorMsg
        {
            get { return this.m_ErrMsg; }
        }
        public int MaxSize
        {
            get { return this.m_MaxSize; }
            set { this.m_MaxSize = value; }
        }
        public HttpPostedFile PostedFile
        {
            set { this.m_PostedFile = value; }
        }
        public string ImageSn
        {
            set { this.m_ImageSn = value; }
        }
        public string RootPath
        {
            set { this.m_RootPath = value; }
        }
        public int[,] ThumbnailAry
        {
            set { this.m_ThumbnailAry = value; }
        }

        /// <summary>
        /// 作者：高鑫
        /// 时间：2011-4-24
        /// 功能：设置规格字符串（例：0x0,30x40,60x80）
        /// </summary>
        public string ThumbnailAryConfig
        {
            set
            {
                string upLoadStandard = value;
                Dictionary<int, int> arr = new Dictionary<int, int>();
                string[] standard = upLoadStandard.Split(',');
                foreach (string item in standard)
                {
                    string[] x_y = item.Split('x');
                    arr.Add(St.ToInt32(x_y[0]), St.ToInt32(x_y[1]));
                }
                int[,] arrSize = new int[arr.Count, arr.Count + 1];
                int i = 0;
                foreach (int j in arr.Keys)
                {
                    arrSize[i, 0] = j;
                    arrSize[i, 1] = arr[j];
                    i++;
                }
                this.m_ThumbnailAry = arrSize;
            }
        }
        public bool[] AddStampAry
        {
            set { this.m_AddStampAry = value; }
        }
        public bool[] FixedAndCutHeight
        {
            set { this.m_FixedAndCutHeightAry = value; }
        }

        public bool[] FiexedWidthAry
        {
            set { this.m_FixedWidthAry = value; }
        }

        public UploadHelper()
        {
            this.m_ThumbnailAry = null;
            this.m_FixedWidthAry = null;
            this.m_LegalExtensions = "jpg,gif,png,jpeg";
            this.m_MaxSize = 2048;     //2M
            // this.m_SaveExtension = "jpg";
        }

        public bool BuildImage()
        {
            string thumSavePath = null;
            string stampImageDir = null;

            Bitmap baseImage = null;
            Image stamp = null;
            Bitmap thumImage = null;
            Graphics g = null;

            try
            {
                if (this.m_ThumbnailAry != null)
                {
                    if (isFenBao)
                    {
                        baseImage = new Bitmap(m_RootPath + GetImagePath(m_ImageSn, GetFenBao, "bake"));
                    }
                    else
                    {
                        baseImage = new Bitmap(m_RootPath + GetImagePath(m_ImageSn, "bake"));
                    }
                    try
                    {
                        stampImageDir = System.Web.HttpContext.Current.Server.MapPath("./StampImage/");
                    }
                    catch
                    {
                        stampImageDir = "./StampImage/";
                    }
                    ImageCodecInfo encoderInfo = GetEncoderInfoByExtension(this.m_ImageExtension);
                    EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, 90L);
                    EncoderParameters encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = encoderParameter;

                    for (int i = 0; i < this.m_ThumbnailAry.GetLength(0); i++)
                    {
                        int width = m_ThumbnailAry[i, 0];
                        int height = m_ThumbnailAry[i, 1];
                        double rate = baseImage.Height * 1.0 / baseImage.Width;
                        bool IsLessThanBase = false;//是否比原图小
                        if (width == 0 || height == 0)
                        {
                            width = baseImage.Width;
                            height = baseImage.Height;
                        }
                        else if (this.m_FixedWidthAry != null)
                        {
                            if (this.m_FixedWidthAry[i])
                            {
                                height = (int)(width * 1.0 * baseImage.Height / baseImage.Width);
                            }
                            else
                            {
                                if ((height * 1.0f / width) < rate)
                                {
                                    width = (int)(height * 1.0 * baseImage.Width / baseImage.Height);
                                }
                                else if ((height * 1.0f / width) > rate)
                                {
                                    height = (int)(width * 1.0 * baseImage.Height / baseImage.Width);
                                }

                            }
                        }
                        else if (this.m_FixedAndCutHeightAry != null)
                        {
                            if (this.m_FixedAndCutHeightAry[i])//计算图片宽度
                            {
                                width = (int)(height * 1.0 * baseImage.Width / baseImage.Height);
                            }
                            else//正常情况下的
                            {
                                if ((height * 1.0f / width) < rate)
                                {
                                    width = (int)(height * 1.0 * baseImage.Width / baseImage.Height);
                                }
                                else if ((height * 1.0f / width) > rate)
                                {
                                    height = (int)(width * 1.0 * baseImage.Height / baseImage.Width);
                                }

                            }
                        }
                        else
                        {
                            if ((baseImage.Width < width) && (baseImage.Height < height))//如果指定生成的图片比原图小
                            {
                                IsLessThanBase = true;
                            }
                            if ((height * 1.0f / width) < rate)
                            {
                                width = (int)(height * 1.0 * baseImage.Width / baseImage.Height);
                            }
                            else if ((height * 1.0f / width) > rate)
                            {
                                height = (int)(width * 1.0 * baseImage.Height / baseImage.Width);
                            }
                        }
                        if (isFenBao)
                        {
                            thumSavePath = m_RootPath + GetImagePath(m_ImageSn, GetFenBao, m_ThumbnailAry[i, 0].ToString() + "x" + m_ThumbnailAry[i, 1].ToString());
                        }
                        else
                        {
                            thumSavePath = m_RootPath + GetImagePath(m_ImageSn, m_ThumbnailAry[i, 0].ToString() + "x" + m_ThumbnailAry[i, 1].ToString());
                        }
                        CheckDirectory(thumSavePath, true);
                        if (m_ThumbnailAry[i, 0] == 0 && m_ThumbnailAry[i, 1] == 0 && this.m_ImageExtension == ".gif")
                        {
                            string Image0x0Path = String.Empty;
                            if (isFenBao)
                            {
                                Image0x0Path = m_RootPath + GetImagePath(m_ImageSn, GetFenBao, "bake");
                            }
                            else
                            {
                                Image0x0Path = m_RootPath + GetImagePath(m_ImageSn, "bake");
                            }
                            File.Copy(Image0x0Path, thumSavePath, true);
                            continue;
                        }

                        if (this.m_ImageExtension == ".jpg" || this.m_ImageExtension == ".gif")
                        {
                            if (this.m_FixedAndCutHeightAry != null && this.m_FixedAndCutHeightAry[i])//当定宽定高需要裁剪宽度
                            {
                                thumImage = new Bitmap(m_ThumbnailAry[i, 0], m_ThumbnailAry[i, 1]);
                            }
                            else
                            {
                                if (IsLessThanBase)//如果原图比生成图小
                                {
                                    //thumImage = new Bitmap(baseImage.Width, baseImage.Height);
                                    string ImagePath = String.Empty;
                                    if (isFenBao)
                                    {
                                        ImagePath = m_RootPath + GetImagePath(m_ImageSn, GetFenBao, "bake");
                                    }
                                    else
                                    {
                                        ImagePath = m_RootPath + GetImagePath(m_ImageSn, "bake");
                                    }
                                    File.Copy(ImagePath, thumSavePath, true);
                                    continue;
                                }
                                else
                                {
                                    thumImage = new Bitmap(width, height);
                                }
                            }
                            g = Graphics.FromImage(thumImage);
                            //							g.Clear(Color.White);
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                            if (this.m_FixedAndCutHeightAry != null && this.m_FixedAndCutHeightAry[i])//当定宽定高需要裁剪宽度
                            {
                                g.DrawImage(baseImage, -((width - m_ThumbnailAry[i, 0]) / 2), 0, width, height);//开始绘制宽度=(总宽度-需要的宽度)/2  也就是取中
                            }
                            else
                            {
                                g.DrawImage(baseImage, 0, 0, (IsLessThanBase ? baseImage.Width : width), (IsLessThanBase ? baseImage.Height : height));
                            }

                            //Add stamp
                            if (this.m_AddStampAry != null)
                            {
                                if (this.m_AddStampAry[i] == true)
                                {
                                    if (width > 374 && height > 224)
                                    {
                                        g.CompositingMode = CompositingMode.SourceOver;
                                        g.CompositingQuality = CompositingQuality.HighQuality;
                                        stamp = Image.FromFile(stampImageDir + "topStamp.gif");
                                        g.DrawImage(stamp, 5, 5, stamp.Width, stamp.Height);
                                        stamp.Dispose();
                                        stamp = Image.FromFile(stampImageDir + "bottomStamp.gif");
                                        g.DrawImage(stamp, thumImage.Width - stamp.Width - 5, thumImage.Height - stamp.Height - 5, stamp.Width, stamp.Height);
                                        stamp.Dispose();
                                        /*
                                        if (m_ThumbnailAry[i,0] == 0)
                                        {
                                            stamp = Image.FromFile(stampImageDir + "logo.png");
                                            g.DrawImage(stamp, (width-stamp.Width)/2, height-stamp.Height-10, stamp.Width, stamp.Height);
                                            stamp.Dispose();
                                        }
                                        */
                                        stamp = null;
                                    }
                                }
                            }
                            g.Dispose();
                            g = null;

                            thumImage.Save(thumSavePath, encoderInfo, encoderParameters);
                            thumImage.Dispose();
                            thumImage = null;
                        }
                        else
                        {

                            if (m_ThumbnailAry[i, 0] == 0 || m_ThumbnailAry[i, 1] == 0)
                            {
                                if (isFenBao)
                                {
                                    File.Copy(m_RootPath + GetImagePath(m_ImageSn, GetFenBao, "bake"), thumSavePath, true);
                                }
                                else
                                {
                                    File.Copy(m_RootPath + GetImagePath(m_ImageSn, "bake"), thumSavePath, true);
                                }
                            }
                            else
                            {
                                thumImage = (Bitmap)baseImage.GetThumbnailImage(width, height, null, IntPtr.Zero);
                                thumImage.Save(thumSavePath, baseImage.RawFormat);
                                thumImage.Dispose();
                                thumImage = null;
                            }
                        }

                    }

                    if (this.m_SaveOx0asOriginal)
                    {
                        string bakeImagePath = String.Empty;
                        if (isFenBao)
                        {
                            bakeImagePath = m_RootPath + GetImagePath(m_ImageSn, GetFenBao, "bake");
                        }
                        else
                        {
                            bakeImagePath = m_RootPath + GetImagePath(m_ImageSn, "bake");
                        }

                        File.Copy(bakeImagePath, bakeImagePath.Replace('/', '\\').Replace(@"\bake\", @"\0x0\"), true);
                    }
                }
            }
            catch (Exception ee)
            {
                m_ErrMsg = ee.Message;
                return false;
            }
            finally
            {
                if (baseImage != null)
                {
                    baseImage.Dispose();
                    baseImage = null;
                }
                if (g != null)
                {
                    g.Dispose();
                    g = null;
                }
                if (thumImage != null)
                {
                    thumImage.Dispose();
                    thumImage = null;
                }
                if (stamp != null)
                {
                    stamp.Dispose();
                    stamp = null;
                }
            }
            return true;
        }
        public bool Upload()
        {
            if (!CheckInputData())
            {
                return false;
            }
            string imgSerPhyPath = String.Empty;
            string sourceImagePhyPath = String.Empty;
            if (isFenBao)
            {
                imgSerPhyPath = m_RootPath + GetImagePath(m_ImageSn, GetFenBao, "0x0");
                sourceImagePhyPath = m_RootPath + GetImagePath(m_ImageSn, GetFenBao, "bake");
            }
            else
            {
                imgSerPhyPath = m_RootPath + GetImagePath(m_ImageSn, "0x0");
                sourceImagePhyPath = m_RootPath + GetImagePath(m_ImageSn, "bake");
            }
            try
            {
                if (File.Exists(imgSerPhyPath)) File.Delete(imgSerPhyPath);
                CheckDirectory(imgSerPhyPath, true);
                CheckDirectory(sourceImagePhyPath, true);
                if (this.m_FullImgFilePath != "" && this.m_FullImgFilePath != null)
                {
                    File.Copy(this.m_FullImgFilePath, sourceImagePhyPath, true);
                }
                else
                {
                    this.m_PostedFile.SaveAs(sourceImagePhyPath);
                }
                return BuildImage();
            }
            catch (Exception ee)
            {
                m_ErrMsg = ee.Message;
                return false;
            }
        }

        private string GetFenBao
        {
            get
            {
                int big = Convert.ToInt32(m_ImageSn) / 1000000;
                int smail = Convert.ToInt32(m_ImageSn) / 1000;
                string isurl = big + "\\" + smail + "\\";
                return isurl;
            }
        }

        public bool IsLegalExtension(string sExtension, string sAcceptedExts)
        {
            if (sExtension.Length < 1)
            {
                return false;
            }
            else
            {
                if (Array.IndexOf(sAcceptedExts.Split(','), sExtension.Substring(1).ToLower()) == -1)
                    return false;
                else
                    return true;
            }
        }

        private string GetImagePath(string imageSn, string imageType)
        {
            string sRetVal = "";
            sRetVal = imageType + "\\" + imageSn + "." + this.m_SaveExtension;
            return sRetVal;
        }
        /// <summary>
        /// 功能：分包以后路径
        /// 作者：窦海超
        /// 日期：2011-5-24
        /// </summary>
        /// <param name="imageSn"></param>
        /// <param name="isurl"></param>
        /// <param name="imageType"></param>
        /// <returns></returns>
        private string GetImagePath(string imageSn, string isurl, string imageType)
        {
            string sRetVal = "";
            sRetVal = imageType + "\\" + isurl + imageSn + "." + this.m_SaveExtension;
            return sRetVal;
        }
        private bool CheckDirectory(string path, bool createDirectory)
        {
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                if (createDirectory)
                {
                    Directory.CreateDirectory(dir);
                }
                return false;
            }
            return true;
        }
        private bool CheckInputData()
        {
            this.m_ImageExtension = (this.m_PostedFile != null ? Path.GetExtension(m_PostedFile.FileName).ToLower() : Path.GetExtension(m_ImageSn + "." + m_SaveExtension).ToLower());
            if (!IsLegalExtension(m_ImageExtension, m_LegalExtensions))
            {
                m_ErrMsg = "只允许上传扩展名为‘" + this.m_LegalExtensions + "’的文件!";
                return false;
            }
            if (m_PostedFile != null)
            {
                if (m_PostedFile.ContentLength / 1024 > m_MaxSize)
                {
                    m_ErrMsg = "只允许上传小于" + m_MaxSize + "k的文件!";
                    return false;
                }
            }
            return true;
        }

        private ImageCodecInfo GetEncoderInfoByExtension(string fExt)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < encoders.Length; i++)
            {
                if (encoders[i].FilenameExtension.IndexOf(fExt.ToUpper()) > -1)
                {
                    return encoders[i];
                }
            }
            return null;
        }

        private ImageCodecInfo GetEncoderInfoByMimeType(string mimeType)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < encoders.Length; i++)
            {
                if (encoders[i].MimeType == mimeType)
                {
                    return encoders[i];
                }
            }
            return null;
        }

        public static bool UpLoadPic(HttpPostedFile m_PostedFile, string SaveAsFileName, string ThumbnailAry)
        {
            if (m_PostedFile != null && m_PostedFile.FileName != "")
            {
                string m_ImageExtension = System.IO.Path.GetExtension(m_PostedFile.FileName).ToLower();
                string m_LegalExtensions = ".jpg,.gif,.png,.jpeg,.bmp";
                if (m_LegalExtensions.IndexOf(m_ImageExtension) < 0)
                {
                    return false;
                }
                if (m_PostedFile.ContentLength / 1024 > 2048)
                {
                    return false;
                }
                string DirName = GetDirByFileName(SaveAsFileName);
                if (!System.IO.Directory.Exists(DirName))
                {
                    System.IO.Directory.CreateDirectory(DirName);
                }
                if (ThumbnailAry == "0x0")
                {
                    m_PostedFile.SaveAs(SaveAsFileName);
                }
                else { MakeThumbnail(m_PostedFile, SaveAsFileName, int.Parse(ThumbnailAry.Split('x')[0]), int.Parse(ThumbnailAry.Split('x')[1])); }
                return true;
            }
            return false;
        }
        private static string GetDirByFileName(string FileName)
        {
            int position = FileName.LastIndexOf(@"\");
            if (position > 0)
            {
                return FileName.Substring(0, position);
            }
            else
            {
                return "";
            }
        }
        private static void MakeThumbnail(HttpPostedFile m_PostedFile, string SaveAsFileName, int width, int height)
        {
            //获取图片类型  
            string fileExtension = System.IO.Path.GetExtension(m_PostedFile.FileName).ToLower();
            ImageCodecInfo encoderInfo = GetEncoderInfoByMimeType_static(fileExtension);
            EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, 90L);
            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = encoderParameter;
            //获取原始图片  
            System.Drawing.Image originalImage = System.Drawing.Image.FromStream(m_PostedFile.InputStream);
            //缩略图画布宽高  
            int towidth = width;
            int toheight = height;
            //原始图片写入画布坐标和宽高(用来设置裁减溢出部分)  
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            //原始图片画布,设置写入缩略图画布坐标和宽高(用来原始图片整体宽高缩放)  
            int bg_x = 0;
            int bg_y = 0;
            int bg_w = towidth;
            int bg_h = toheight;
            //倍数变量  
            double multiple = 0;
            //获取宽长的或是高长与缩略图的倍数  
            if (originalImage.Width >= originalImage.Height)
                multiple = (double)originalImage.Width / (double)width;
            else
                multiple = (double)originalImage.Height / (double)height;
            //上传的图片的宽和高小等于缩略图  
            if (ow <= width && oh <= height)
            {
                //缩略图按原始宽高  
                bg_w = originalImage.Width;
                bg_h = originalImage.Height;
                //空白部分用背景色填充  
                bg_x = Convert.ToInt32(((double)towidth - (double)ow) / 2);
                bg_y = Convert.ToInt32(((double)toheight - (double)oh) / 2);
            }
            //上传的图片的宽和高大于缩略图  
            else
            {
                //宽高按比例缩放  
                bg_w = Convert.ToInt32((double)originalImage.Width / multiple);
                bg_h = Convert.ToInt32((double)originalImage.Height / multiple);
                //空白部分用背景色填充  
                bg_y = Convert.ToInt32(((double)height - (double)bg_h) / 2);
                bg_x = Convert.ToInt32(((double)width - (double)bg_w) / 2);
            }
            //新建一个bmp图片,并设置缩略图大小.  
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            //新建一个画板  
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法  
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            //设置高质量,低速度呈现平滑程度  
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并设置背景色  
            g.Clear(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            //在指定位置并且按指定大小绘制原图片的指定部分  

            //第一个System.Drawing.Rectangle是原图片的画布坐标和宽高,第二个是原图片写在画布上的坐标和宽高,最后一个参数是指定数值单位为像素  
            g.DrawImage(originalImage, new System.Drawing.Rectangle(bg_x, bg_y, bg_w, bg_h), new System.Drawing.Rectangle(x, y, ow, oh), System.Drawing.GraphicsUnit.Pixel);
            //水印
            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighQuality;
            //Image stamp = Image.FromFile(@"C:\Documents and Settings\Administrator\My Documents\My Pictures\zanque.jpg");
            //g.DrawImage(stamp, 5, 5, stamp.Width, stamp.Height);
            //stamp.Dispose();
            Image stamp = Image.FromFile(System.Web.HttpContext.Current.Server.MapPath("\\watermark\\foot.png"));
            g.DrawImage(stamp, 120 - stamp.Width, 150 - stamp.Height - 2, stamp.Width, stamp.Height);
            stamp.Dispose();
            try
            {
                bitmap.Save(SaveAsFileName, encoderInfo, encoderParameters);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
        private static ImageCodecInfo GetEncoderInfoByMimeType_static(string mimeType)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < encoders.Length; i++)
            {
                if (encoders[i].FilenameExtension.IndexOf(mimeType.ToUpper()) > -1)
                {
                    return encoders[i];
                }
            }
            return null;
        }


    }
}
