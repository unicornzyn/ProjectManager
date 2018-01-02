using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;
using System.Configuration;

namespace Common
{
    public enum ImgUploadSite
    {
        PCPOP = 1,
        IT168 = 2,
        Both = 3        //同时建PCPOP和IT168目录结构。
    }

    public class ImgUploader
    {
        private string m_FullImgFilePath;
        private HttpPostedFile m_PostedFile;
        private string m_ImageSn;
        private string m_RootPath;
        private int[,] m_ThumbnailAry;
        private bool[] m_AddStampAry;
        private string m_LegalExtensions;
        private int m_MaxSize;
        private bool _isTraitImg = false;//是否是双水印。产品图片是单水印。特性图片为双水印。
        private string m_SaveExtension;
        private string m_ErrMsg = String.Empty;
        private string m_ImageExtension = string.Empty;
        private bool m_SaveOx0asOriginal = false;
        //是否是随机水印。
        private bool m_IsRandomLocation = false;
        private bool[] m_FixedWidthAry;
        private bool[] m_FixedAndCutHeightAry;//定高定宽并且将多余的宽度剪掉

        private ImgUploadSite m_BuildImgUploadSite;

        private bool[] m_AddEditerStampAry; //保存是否添加编辑的特定水印 liuyulei

        private int[,] m_EditerWatermarkLoc = new int[2, 2];  //保存编辑所做图片的水印，将会存两组坐标(左上角，右下角)


        public bool[] AddEditerStampAry
        {
            get { return m_AddEditerStampAry; }
        }

        public int[,] EditerWatermarkLoc
        {
            get { return m_EditerWatermarkLoc; }
        }

        /// <summary>
        /// 保存图片的规格 2010-12-27 gaoxin
        /// </summary>
        private string m_standard = "ProductImages";
        public string Standard
        {
            set
            {
                this.m_standard = value;
            }
        }

        public string FullImgFilePath
        {
            set
            {
                this.m_FullImgFilePath = value;
            }
        }
        public bool IsTraitImg
        {
            set
            {
                this._isTraitImg = value;
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
        /// <summary>
        /// 图片压缩尺寸。
        /// 若设置为空。则尝试从配置文件中获取站点对应的尺寸列表。
        /// </summary>
        public int[,] ThumbnailAry
        {
            set { this.m_ThumbnailAry = value; }
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

        /// <summary>
        /// 图片上传站点对应枚举。
        /// 若设置该属性。当不设置ThumbnailAry,AddStampAry时图片尺寸,水印数组可以从配置文件中读取。
        /// 不设置该属性。则与PCPOP，IT168目录结构无关。
        /// </summary>
        public ImgUploadSite BuildImgUploadSite
        {
            set
            {
                m_BuildImgUploadSite = value;
            }
        }

        public ImgUploader()
        {
            this.m_ThumbnailAry = null;
            this.m_FixedWidthAry = null;
            this.m_LegalExtensions = "jpg,gif";
            this.m_MaxSize = 2048;     //2M
            this.m_SaveExtension = "jpg";
        }



        public bool BuildImage()
        {
            //设置生成一张微信专用图 675*375 补白 赵英楠
            Transfer(St.ToInt32(m_ImageSn, 0));

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
                    baseImage = new Bitmap(m_RootPath + GetImagePath(m_ImageSn, "origin"));
                    try
                    {
                        //stampImageDir = System.Web.HttpContext.Current.Server.MapPath("./StampImage/");

                        /*
                         * date:09.06.01
                         * type:change
                         * author:xuefeng
                         */
                        stampImageDir = System.Web.HttpContext.Current.Server.MapPath("../StampImage");
                    }
                    catch
                    {
                        //if (System.Web.HttpContext.Current == null)
                        //    stampImageDir = AppDomain.CurrentDomain.BaseDirectory + "StampImage\\";
                        //else
                        //    stampImageDir = "./StampImage/";

                        /*
                         * date:09.06.01
                         * type:change
                         * author:xuefeng
                         */
                        stampImageDir = System.Web.HttpContext.Current.Server.MapPath("StampImage");
                    }
                    ImageCodecInfo encoderInfo = GetEncoderInfoByExtension(this.m_ImageExtension);
                    EncoderParameter encoderParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                    EncoderParameters encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = encoderParameter;

                    string Image0x0Path = m_RootPath + GetImagePath(m_ImageSn, "origin"); //原图地址
                    for (int i = 0; i < this.m_ThumbnailAry.GetLength(0); i++)
                    {
                        int width = m_ThumbnailAry[i, 0];
                        int height = m_ThumbnailAry[i, 1];
                        thumSavePath = m_RootPath + GetImagePath(m_ImageSn, width.ToString() + "x" + height.ToString());
                        double rate = baseImage.Height * 1.0 / baseImage.Width;
                        bool IsLessThanBase = false;//是否比原图小
                        if (width == 0 || height == 0)
                        {
                            width = baseImage.Width;
                            height = baseImage.Height;
                        }
                        else if (width == height)  //宽高相等生成方图 不足部分补白 赵英楠 2015-06-29
                        {
                            ImageTransfer(width, height, Image0x0Path, thumSavePath); continue;
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
                            if ((baseImage.Width < width) && (baseImage.Height < height))//如果原图比生成图小
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

                        
                        CheckDirectory(thumSavePath, true);
                        if (m_ThumbnailAry[i, 0] == 0 && m_ThumbnailAry[i, 1] == 0 && this.m_ImageExtension == ".gif")
                        {
                            

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
                                    //string ImagePath = m_RootPath + GetImagePath(m_ImageSn, "origin");
                                    File.Copy(Image0x0Path, thumSavePath, true);
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
                            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                            if (this.m_FixedAndCutHeightAry != null && this.m_FixedAndCutHeightAry[i])//当定宽定高需要裁剪宽度
                            {
                                g.DrawImage(baseImage, -((width - m_ThumbnailAry[i, 0]) / 2), 0, width, height);//开始绘制宽度=(总宽度-需要的宽度)/2  也就是取中
                            }
                            else
                            {
                                g.DrawImage(baseImage, 0, 0, (IsLessThanBase ? baseImage.Width : width), (IsLessThanBase ? baseImage.Height : height));
                            }

                            //添加编辑特定的水印 liuyulei 2011-11-23
                            if (this.m_AddEditerStampAry != null && this.m_AddEditerStampAry[i])
                            {
                                if (width > 374 && height > 224)
                                {
                                    g.CompositingMode = CompositingMode.SourceOver;
                                    g.CompositingQuality = CompositingQuality.HighQuality;

                                    DrawWatermarkToEditerImg(g, stampImageDir, width, height);
                                }
                            }
                            //Add stamp
                            else if (this.m_AddStampAry != null)
                            {
                                if (this.m_AddStampAry[i] == true)
                                {
                                    if (width > 374 && height > 224)
                                    {
                                        g.CompositingMode = CompositingMode.SourceOver;
                                        g.CompositingQuality = CompositingQuality.HighQuality;

                                        int loc_x = 10;
                                        int loc_y = 1;
                                        //当原图尺寸小于到既定尺寸。取原图尺寸。
                                        int basewidth = width < baseImage.Width ? width : baseImage.Width;
                                        int baseheight = height < baseImage.Height ? height : baseImage.Height;

                                        if (_isTraitImg)
                                        {
                                            stamp = Image.FromFile(stampImageDir + ((int)m_BuildImgUploadSite == 0 ? "" : ("\\" + m_BuildImgUploadSite)) + "\\pop.png");
                                            g.DrawImage(stamp, 10, 10, stamp.Width, stamp.Height);
                                            stamp = Image.FromFile(stampImageDir + ((int)m_BuildImgUploadSite == 0 ? "" : ("\\" + m_BuildImgUploadSite)) + "\\168.png");
                                            g.DrawImage(stamp, (width - stamp.Width) - 10, (height - stamp.Height) - 10, stamp.Width, stamp.Height);
                                        }
                                        else
                                        {
                                            stamp = Image.FromFile(stampImageDir + ((int)m_BuildImgUploadSite == 0 ? "" : ("\\" + m_BuildImgUploadSite)) + "\\logo.png");
                                            if (m_IsRandomLocation)
                                            {
                                                if (basewidth > 1200 && baseheight > 1200)
                                                {
                                                    loc_x = getParameterRDLoc(stamp.Width, basewidth);
                                                    loc_y = getParameterRDLoc(stamp.Height, baseheight);
                                                }//对
                                                else
                                                {
                                                    loc_x = getRandomLoc(stamp.Width, basewidth);
                                                    loc_y = getRandomLoc(stamp.Height, baseheight);
                                                }
                                            }
                                            g.DrawImage(stamp, loc_x, loc_y, stamp.Width, stamp.Height);

                                        }
                                        stamp.Dispose();
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
                                File.Copy(m_RootPath + GetImagePath(m_ImageSn, "origin"), thumSavePath, true);
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
                        //string bakeImagePath = m_RootPath + GetImagePath(m_ImageSn, "origin");
                        File.Copy(Image0x0Path, Image0x0Path.Replace('/', '\\').Replace(@"\origin\", @"\0x0\"), true);
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
                //return false;
            }
            string imgSerPhyPath = m_RootPath + GetImagePath(m_ImageSn, "0x0");
            string sourceImagePhyPath = m_RootPath + GetImagePath(m_ImageSn, "origin");
            try
            {
                // if (File.Exists(imgSerPhyPath)) File.Delete(imgSerPhyPath);
                if ((int)m_BuildImgUploadSite == 0)
                    CheckDirectory(imgSerPhyPath, true);
                CheckDirectory(sourceImagePhyPath, true);

                // 注释
                if (this.m_FullImgFilePath != "" && this.m_FullImgFilePath != null)
                {
                    File.Copy(this.m_FullImgFilePath, sourceImagePhyPath, true);
                }
                else
                {
                    this.m_PostedFile.SaveAs(sourceImagePhyPath);
                }
                if ((int)m_BuildImgUploadSite != 0)
                {
                    if (m_BuildImgUploadSite == ImgUploadSite.Both)
                    {
                        bool fromConfig = (m_AddStampAry == null || m_ThumbnailAry == null);
                        bool success = false;
                        m_BuildImgUploadSite = ImgUploadSite.IT168;
                        if (fromConfig)
                            LoadSizeFromConfig(ImgUploadSite.IT168);
                        success = BuildImage();
                        m_BuildImgUploadSite = ImgUploadSite.PCPOP;
                        if (fromConfig)
                            LoadSizeFromConfig(ImgUploadSite.PCPOP);
                        return BuildImage() && success;
                    }
                    else
                    {
                        if (m_AddStampAry == null || m_ThumbnailAry == null)
                            LoadSizeFromConfig(m_BuildImgUploadSite);
                        return BuildImage();
                    }
                }
                else
                {
                    return BuildImage();
                }


            }
            catch (Exception ee)
            {
                ErrorMessage.WriteLog("", "错误信息为: " + ee.ToString());
                m_ErrMsg = ee.Message;
                return false;
            }
        }

        #region 生成675*375图片 微信用 特殊处理 赵英楠 2014-04-14
        private void Transfer(int photoid)
        {
            string upLoadPath = System.Configuration.ConfigurationManager.AppSettings["ProductImagesPath"].Trim();
            // \PCPOP\500x375     675x375
            string opath = upLoadPath + @"\origin\" + (photoid / 1000000) + "\\" + (photoid / 1000) + "\\" + photoid + ".jpg";
            string npath = upLoadPath + @"\PCPOP\675x375\" + (photoid / 1000000) + "\\" + (photoid / 1000) + "\\" + photoid + ".jpg";
            ImageTransfer(675, 375, opath, npath);
        }
        private void ImageTransfer(int w, int h, string opath, string npath)
        {
            try
            {
                Bitmap oldimg = new Bitmap(opath);
                int nw = oldimg.Width;
                int nh = oldimg.Height;
                if (nw > w)
                {
                    nw = w;
                    nh = (oldimg.Height * w) / oldimg.Width;
                }
                if (nh > h)
                {
                    nh = h;
                    nw = (oldimg.Width * h) / oldimg.Height;
                }

                //Bitmap img = new Bitmap(oldimg, nw, nh);
                Bitmap newimg = new Bitmap(w, h);
                Graphics g = Graphics.FromImage(newimg);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                g.Clear(Color.White);
                //g.DrawImage(img, new Point((w - img.Width) / 2, (h - img.Height) / 2));
                g.DrawImage(oldimg, (w - nw) / 2, (h - nh) / 2, nw, nh);
                string dir = Path.GetDirectoryName(npath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                ImageCodecInfo encoderInfo = GetEncoderInfoByExtension(this.m_ImageExtension);
                EncoderParameter encoderParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = encoderParameter;
                newimg.Save(npath, encoderInfo, encoderParameters);
            }
            catch (Exception ex)
            {               
               ErrorMessage.WriteLog("图片转换错误", "错误信息为: " + ex.ToString());
            }
        }
        #endregion

        public bool Upload(int uploadType)
        {
            if (!CheckInputData())
            {
                //return false;
            }
            string sizeConfigName = "";
            switch (uploadType)
            {
                case 0: break;
                case 1: this.RootPath = System.Configuration.ConfigurationManager.AppSettings["CategorySPImagesPath"].Trim();//类目活动图片上传
                    sizeConfigName = "CategorySPImages";
                    break;
                case 2: this.RootPath = System.Configuration.ConfigurationManager.AppSettings["EBWebsiteImagesPath"].Trim();//电商图片上传
                    sizeConfigName = "EBWebsiteImages";
                    break;
                case 3: this.RootPath = System.Configuration.ConfigurationManager.AppSettings["ProductSummaryImagesPath"].Trim();//电商图片上传
                    sizeConfigName = "ProductSummaryImages";
                    break;
                case 4: this.RootPath = System.Configuration.ConfigurationManager.AppSettings["SeoTitleImagePath"].Trim();//手工内容图片上传
                    sizeConfigName = "SeoTitleImages";
                    break;
                case 5: this.RootPath = System.Configuration.ConfigurationManager.AppSettings["QHMImage"].Trim() + "\\QHMProductImage";//抢红米产品图片上传
                    sizeConfigName = "QHMProductImages";
                    break;
                case 6: this.RootPath = System.Configuration.ConfigurationManager.AppSettings["QHMImage"].Trim() + "\\QHMHome";//抢红米首页图片上传
                    sizeConfigName = "QHMHomeImages";
                    break;
                case 7: this.RootPath = System.Configuration.ConfigurationManager.AppSettings["QHMImage"].Trim() + "\\QHMContentImage";//抢红米产品描述图片上传
                    sizeConfigName = "QHMContentImages";
                    break;
                case 8: this.RootPath = System.Configuration.ConfigurationManager.AppSettings["QFQImage"].Trim() + "\\QFQHome";//抢红米产品描述图片上传
                    sizeConfigName = "QFQImages";
                    break;
                case 9: this.RootPath = System.Configuration.ConfigurationManager.AppSettings["QHMImage"].Trim() + "\\TwoClassImages";//专题二级分类图片上传
                    sizeConfigName = "TwoClassImages";
                    break;
                case 10: this.RootPath = System.Configuration.ConfigurationManager.AppSettings["QHMImage"].Trim() + "\\LotteryImage\\";//牛败商城转盘抽奖图片上传
                    sizeConfigName = "LotteryImages";
                    break;
                case 11: this.RootPath = System.Configuration.ConfigurationManager.AppSettings["QHMImage"].Trim() + "\\WX0BuyImages\\";//微信0元购推广图片上传
                    sizeConfigName = "QHMProductImages"; //使用抢红米产品图片尺寸
                    break;
                case 12: this.RootPath = System.Configuration.ConfigurationManager.AppSettings["QHMImage"].Trim() + "\\QHMTopicImages\\";//牛败商城专题图
                    sizeConfigName = "QHMTopicImages"; 
                    break;
                case 13: this.RootPath = System.Configuration.ConfigurationManager.AppSettings["QHMImage"].Trim() + "\\QHMBrandImages\\";//牛败商城品牌图
                    sizeConfigName = "QHMBrandImages";
                    break;
            }
        
            string imgSerPhyPath = m_RootPath + GetImagePath(m_ImageSn, "0x0");
            string sourceImagePhyPath = m_RootPath + GetImagePath(m_ImageSn, "origin");
            ErrorMessage.WriteLog("", "imgSerPhyPath:" + imgSerPhyPath + "  sourceImagePhyPath:" + sourceImagePhyPath);
            try
            {
                // if (File.Exists(imgSerPhyPath)) File.Delete(imgSerPhyPath);
                if ((int)m_BuildImgUploadSite == 0)
                {
                    CheckDirectory(imgSerPhyPath, true);
                }
                CheckDirectory(sourceImagePhyPath, true);
                // 注释
                if (this.m_FullImgFilePath != "" && this.m_FullImgFilePath != null)
                {
                    File.Copy(this.m_FullImgFilePath, sourceImagePhyPath, true);
                }
                else
                {
                    this.m_PostedFile.SaveAs(sourceImagePhyPath);
                }
                if ((int)m_BuildImgUploadSite != 0)
                {
                    if (m_BuildImgUploadSite == ImgUploadSite.Both)
                    {
                        bool fromConfig = (m_AddStampAry == null || m_ThumbnailAry == null);
                        bool success = false;
                        m_BuildImgUploadSite = ImgUploadSite.IT168;
                        if (fromConfig)
                            LoadSizeFromConfig(ImgUploadSite.IT168, sizeConfigName);
                        success = BuildImage();
                        m_BuildImgUploadSite = ImgUploadSite.PCPOP;
                        if (fromConfig)
                            LoadSizeFromConfig(ImgUploadSite.PCPOP, sizeConfigName);
                        return BuildImage() && success;
                    }
                    else
                    {
                        if (m_AddStampAry == null || m_ThumbnailAry == null)
                            LoadSizeFromConfig(m_BuildImgUploadSite, sizeConfigName);
                        return BuildImage();
                    }
                }
                else
                {
                    return BuildImage();
                }
            }
            catch (Exception ee)
            {
                ErrorMessage.WriteLog("", "错误信息为: " + ee.ToString());
                m_ErrMsg = ee.Message;
                return false;
            }
        }
        /// <summary>
        /// 李雅杰 20120817
        /// 普通上传图片
        /// </summary>
        /// <param name="path">上传图片地址</param>
        /// <returns></returns>
        public bool Upload(string path)
        {

            if (!CheckInputData())
            {
                //return false;
            }
            //string imgSerPhyPath = m_RootPath + GetImagePath(m_ImageSn, "0x0");
            //string sourceImagePhyPath = m_RootPath + GetImagePath(m_ImageSn, "origin");
            try
            {
                // if (File.Exists(imgSerPhyPath)) File.Delete(imgSerPhyPath);
                //if ((int)m_BuildImgUploadSite == 0)
                //    CheckDirectory(path, true);
                CheckDirectory(path, true);

                // 注释
                if (this.m_FullImgFilePath != "" && this.m_FullImgFilePath != null)
                {
                    File.Copy(this.m_FullImgFilePath, path, true);
                }
                else
                {
                    this.m_PostedFile.SaveAs("1");
                }
                if ((int)m_BuildImgUploadSite != 0)
                {

                    if (m_AddStampAry == null || m_ThumbnailAry == null)
                        LoadSizeFromConfig(m_BuildImgUploadSite);
                    return BuildImage();
                }
                else
                {
                    return BuildImage();
                }
            }
            catch (Exception ee)
            {
                ErrorMessage.WriteLog("", "错误信息为: " + ee.ToString());
                m_ErrMsg = ee.Message;
                return false;
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

        public string GetImagePath(string imageSn, string imageType)
        {
            string sRetVal = "";
            //int imageId = Convert.ToInt32(imageSn);
            long imageId = Convert.ToInt64(imageSn);//32变64增加范围 2010-12-27 gaoxin
            string subFolderName = Convert.ToString(imageId / 1000000) + "\\" + Convert.ToString(imageId / 1000);
            sRetVal = imageType + "\\" + subFolderName + "\\" + imageSn + "." + this.m_SaveExtension;
            if ((int)m_BuildImgUploadSite == 0)
                return sRetVal;
            if (imageType != "origin")
            {
                if (m_BuildImgUploadSite == ImgUploadSite.IT168)
                    sRetVal = "IT168\\" + sRetVal;
                if (m_BuildImgUploadSite == ImgUploadSite.PCPOP)
                    sRetVal = "PCPOP\\" + sRetVal;
            }
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
            this.m_ImageExtension = (this.m_PostedFile != null ? Path.GetExtension(m_PostedFile.FileName).ToLower() : Path.GetExtension(m_ImageSn + ".jpg").ToLower());
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
        #region LoadSizeFromConfig
        /// <summary>
        /// 从配置文件中获取尺寸信息。
        /// </summary>
        /// <param name="site"></param>
        private void LoadSizeFromConfig(ImgUploadSite site)
        {
            string cfg = ConfigurationManager.AppSettings[m_standard];
            if (string.IsNullOrEmpty(cfg))
                return;
            string sizestr = "";
            if (site == ImgUploadSite.IT168)
            {
                sizestr = getRegValue("it168", cfg);
                this.m_AddStampAry = getMarkList(sizestr, getRegValue("it168Watermark", cfg));
                this.m_AddEditerStampAry = getMarkList(sizestr, getRegValue("it168EditerWatermark", cfg)); //获取特定水印的图片尺寸
                this.m_ThumbnailAry = getSizeArray(sizestr);
            }
            if (site == ImgUploadSite.PCPOP)
            {
                sizestr = getRegValue("pcpop", cfg);
                this.m_AddStampAry = getMarkList(sizestr, getRegValue("pcpopWatermark", cfg));
                this.m_AddEditerStampAry = getMarkList(sizestr, getRegValue("pcpopEditerWatermark", cfg)); //获取特定水印的图片尺寸
                this.m_ThumbnailAry = getSizeArray(sizestr);
            }
        }
        private void LoadSizeFromConfig(ImgUploadSite site, string sizeConfig)
        {
            string cfg = ConfigurationManager.AppSettings[sizeConfig];
            if (string.IsNullOrEmpty(cfg))
                return;
            string sizestr = "";
            if (site == ImgUploadSite.IT168)
            {
                sizestr = getRegValue("it168", cfg);
                this.m_AddStampAry = getMarkList(sizestr, getRegValue("it168Watermark", cfg));
                this.m_AddEditerStampAry = getMarkList(sizestr, getRegValue("it168EditerWatermark", cfg)); //获取特定水印的图片尺寸
                this.m_ThumbnailAry = getSizeArray(sizestr);
            }
            if (site == ImgUploadSite.PCPOP)
            {
                sizestr = getRegValue("pcpop", cfg);
                this.m_AddStampAry = getMarkList(sizestr, getRegValue("pcpopWatermark", cfg));
                this.m_AddEditerStampAry = getMarkList(sizestr, getRegValue("pcpopEditerWatermark", cfg)); //获取特定水印的图片尺寸
                this.m_ThumbnailAry = getSizeArray(sizestr);
            }
        }
        private string getRegValue(string key, string source)
        {
            string regex = key + @":'([\d,x]+)'";
            try
            {
                return Regex.Matches(source, regex)[0].Groups[1].Value;
            }
            catch
            {
                return "";
            }
        }
        private int[,] getSizeArray(string size)
        {
            string[] sizes = size.Split(',');
            int[,] array = new int[sizes.Length, 2];
            for (int i = 0; i < sizes.Length; i++)
            {
                array[i, 0] = St.ToInt32(sizes[i].Split('x')[0]);
                array[i, 1] = St.ToInt32(sizes[i].Split('x')[1]); ;
            }
            return array;
        }
        private bool[] getMarkList(string size, string mark)
        {
            string[] sizes = size.Split(',');
            string[] marks = mark.Split(',');
            bool[] result = new bool[sizes.Length];
            for (int i = 0; i < sizes.Length; i++)
                foreach (string m in marks)
                    if (sizes[i] == m)
                        result[i] = true;
            return result;
        }
        #endregion

        #region 单水印中随机水印

        /// <summary>
        /// 是否是随机水印。在单水印的情况下。允许随机出现水印的位置。
        /// </summary>
        public bool IsRandomLocation
        {
            set { m_IsRandomLocation = value; }
        }
        private static Random r = new Random(100);
        /// <summary>
        /// 获取该长度随机1/3的位置。
        /// </summary>
        /// <param name="logosize">logo的高度或宽度</param>
        /// <param name="imagesize">图片的高度或宽度</param>
        /// <returns></returns>
        private int getRandomLoc(int logosize, int imagesize)
        {
            if (r.Next(10) % 2 == 0)
                return (imagesize / 9 - logosize / 2);
            else
                return (2 * imagesize / 9 - logosize / 2); ;
        }
        /// <summary>
        /// 获取右下角位置（各5%）
        /// </summary>
        /// <param name="logosize"></param>
        /// <param name="imagesize"></param>
        /// <returns></returns>
        private int getParameterRDLoc(int logosize, int imagesize)
        {
            return (int)(imagesize * 0.95 - logosize / 2);
        }



        #region 刘玉磊添加

        /// <summary>
        /// 作者：刘玉磊
        /// 时间：2011-11-22
        /// 功能：获取配置文件中的水印位置(针对编辑)
        /// </summary>
        /// <param name="g">画板</param>
        /// <param name="stampImgDir">存有水印目录的上一级</param>
        /// <param name="realWidth">画板中底图的真正宽</param>
        /// <param name="realHeiht">画板中底图的真正高</param>
        /// <returns></returns>
        public void DrawWatermarkToEditerImg(Graphics g, string stampImgDir, int realWidth, int realHeiht)
        {
            //获取配置文件中的特定水印坐标信息
            LoadEditerWatermarkLoc();

            //绘制第一次(左上角)
            Image stamp = Image.FromFile(stampImgDir + "\\PCPOP\\popTopLeft.png");
            int loc_x = m_EditerWatermarkLoc[0, 0];
            int loc_y = m_EditerWatermarkLoc[0, 1];
            g.DrawImage(stamp, loc_x, loc_y, stamp.Width, stamp.Height);


            //绘制第二次(右下角)
            stamp = Image.FromFile(stampImgDir + "\\PCPOP\\popBottomRight.png");
            loc_x = m_EditerWatermarkLoc[1, 0];
            loc_y = m_EditerWatermarkLoc[1, 1];


            if (realWidth < (stamp.Width + loc_x))
            {
                loc_x = 0;
            }
            if (realHeiht < (stamp.Height + loc_y))
            {
                loc_y = 0;
            }

            g.DrawImage(stamp, realWidth - (loc_x + stamp.Width), realHeiht - (loc_y + stamp.Height), stamp.Width, stamp.Height);
            stamp.Dispose();
            stamp = null;

        }

        /// <summary>
        /// 作者：刘玉磊
        /// 时间：2011-11-22
        /// 功能：获取配置文件中的水印位置(针对编辑)
        /// </summary>
        public void LoadEditerWatermarkLoc()
        {
            string cfg = ConfigurationManager.AppSettings["EditerWatermarkLoc"];
            if (string.IsNullOrEmpty(cfg))
                return;

            int[] intArr = new int[2];


            intArr = GetCoordinate(cfg, "TopLeft");
            for (int i = 0; i < intArr.Length; i++)
            {
                this.m_EditerWatermarkLoc[0, i] = intArr[i];
            }

            intArr = GetCoordinate(cfg, "BottomRight");
            for (int i = 0; i < intArr.Length; i++)
            {
                this.m_EditerWatermarkLoc[1, i] = intArr[i];
            }
        }

        /// <summary>
        /// 作者：刘玉磊
        /// 时间：2011-11-22
        /// 功能：获取水印坐标(针对编辑)
        /// </summary>
        /// <param name="config"></param>
        /// <param name="watermarkLoc"></param>
        /// <returns></returns>
        private int[] GetCoordinate(string config, string watermarkLoc)
        {

            int[] coordinate = new int[2];
            string regVal = getCoordinateRegValue(watermarkLoc, config);
            if (!string.IsNullOrEmpty(regVal))
            {
                coordinate[0] = St.ToInt32(regVal.Split(',')[0], 0);
                coordinate[1] = St.ToInt32(regVal.Split(',')[1], 0);
            }

            return coordinate;
        }

        /// <summary>
        /// 获取特定加水印的尺寸
        /// </summary>
        /// <param name="key"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private string getCoordinateRegValue(string key, string source)
        {
            string regex = key + @":'([\d,]+)'";
            try
            {
                return Regex.Matches(source, regex)[0].Groups[1].Value;
            }
            catch
            {
                return "";
            }
        }

        #endregion



        #endregion
    }
}
