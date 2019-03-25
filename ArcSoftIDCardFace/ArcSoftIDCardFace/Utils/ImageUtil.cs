using ArcsoftIDCardFace.SDKModels;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ArcsoftIDCardFace.Utils
{
    public class ImageUtil
    {
        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static ASVLOFFSCREEN ReadBmp(Image image)
        {
            ASVLOFFSCREEN offInput = new ASVLOFFSCREEN();
            //将Image转换为Format24bppRgb格式的BMP
            Bitmap bm = new Bitmap(image);
            //将Bitmap锁定到系统内存中,获得BitmapData
            BitmapData data = bm.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            //位图中第一个像素数据的地址。它也可以看成是位图中的第一个扫描行
            IntPtr ptr = data.Scan0;
            //定义数组长度
            int soureBitArrayLength = data.Height * Math.Abs(data.Stride);
            byte[] sourceBitArray = new byte[soureBitArrayLength];
            //将bitmap中的内容拷贝到ptr_bgr数组中
            MemoryUtil.Copy(ptr, sourceBitArray, 0, soureBitArrayLength);
            int width = data.Width;
            int height = data.Height;
            int pitch = Math.Abs(data.Stride);
            //获取去除对齐位后度图像数据
            int line = width * 3;
            int bgr_len = line * height;
            byte[] destBitArray = new byte[bgr_len];
            /*
               * 图片像素数据在内存中是按行存储，一般图像库都会有一个内存对齐，在每行像素的末尾位置
               * 每行的对齐位会使每行多出一个像素空间（三通道如RGB会多出3个字节，四通道RGBA会多出4个字节）
               * 以下循环目的是去除每行末尾的对齐位，将有效的像素拷贝到新的数组
               */
            for (int i = 0; i < height; ++i)
            {
                Array.Copy(sourceBitArray, i * pitch, destBitArray, i * line, line);
            }
            pitch = line;
            bm.UnlockBits(data);

            IntPtr imageDataPtr = MemoryUtil.Malloc(destBitArray.Length);
            MemoryUtil.Copy(destBitArray, 0, imageDataPtr, destBitArray.Length);
            offInput.u32PixelArrayFormat = ASF_ImagePixelFormat.ASVL_PAF_RGB24_B8G8R8;
            offInput.ppu8Plane = new IntPtr[4];
            offInput.ppu8Plane[0] = imageDataPtr;
            offInput.i32Width = width;
            offInput.i32Height = height;
            offInput.pi32Pitch = new int[4];
            offInput.pi32Pitch[0] = pitch;
            return offInput;
        }

        /// <summary>
        /// 用矩形框标记图片指定区域
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="startX">矩形框左上角X坐标</param>
        /// <param name="startY">矩形框左上角Y坐标</param>
        /// <param name="width">矩形框宽度</param>
        /// <param name="height">矩形框高度</param>
        /// <returns>标记后的图片</returns>
        public static Image MarkRect(Image image, int startX, int startY, int width, int height)
        {
            Image clone = (Image)image.Clone();
            Graphics g = Graphics.FromImage(clone);
            try
            {
                using (Brush brush = new SolidBrush(Color.Red))
                {
                    using (Pen pen = new Pen(brush, 2))
                    {
                        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        g.DrawRectangle(pen, new Rectangle(startX, startY, width, height));
                    }
                }
                return clone;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                g.Dispose();
            }

            return null;
        }
        
        /// <summary>
        /// 按指定宽高缩放图片
        /// </summary>
        /// <param name="image">原图片</param>
        /// <param name="dstWidth">目标图片宽</param>
        /// <param name="dstHeight">目标图片高</param>
        /// <returns></returns>
        public static Image ScaleImage(Image image, int dstWidth, int dstHeight)
        {
            Graphics g = null;
            try
            {
                //按比例缩放           
                float scaleRate = getWidthAndHeight(image.Width, image.Height, dstWidth, dstHeight);
                int width = (int)(image.Width * scaleRate);
                int height = (int)(image.Height * scaleRate);

                //将宽度调整为4的整数倍
                if (width % 4 != 0) {
                    width = width - width % 4;
                }

                Bitmap destBitmap = new Bitmap(width, height);
                g = Graphics.FromImage(destBitmap);
                g.Clear(Color.Transparent);

                //设置画布的描绘质量         
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, new Rectangle((width - width) / 2, (height - height) / 2, width, height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);

                //设置压缩质量     
                EncoderParameters encoderParams = new EncoderParameters();
                long[] quality = new long[1];
                quality[0] = 100;
                EncoderParameter encoderParam = new EncoderParameter(Encoder.Quality, quality);
                encoderParams.Param[0] = encoderParam;

                return destBitmap;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (g != null)
                {
                    g.Dispose();
                }
            }

            return null;
        }

        /// <summary>
        /// byte[] 转图片
        /// </summary>
        /// <param name="Bytes">原图片Bytes数组</param>
        /// <returns>Bitmap图像</returns>
        public static Bitmap BytesToBitmap(byte[] Bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(Bytes);
                return new Bitmap(stream);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldWidth"></param>
        /// <param name="oldHeigt"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <returns></returns>
        public static float getWidthAndHeight(int oldWidth,int oldHeigt,int newWidth,int newHeight)
        {
            //按比例缩放           
            float scaleRate = 0.0f;
            if (oldWidth >= newWidth && oldHeigt >= newHeight)
            {
                int widthDis = oldWidth - newWidth;
                int heightDis = oldHeigt - newHeight;
                if (widthDis > heightDis)
                {
                    scaleRate = newWidth * 1f / oldWidth;
                }
                else
                {
                    scaleRate = newHeight * 1f / oldHeigt;
                }
            }
            else if (oldWidth >= newWidth && oldHeigt < newHeight)
            {
                scaleRate = newWidth * 1f / oldWidth;
            }
            else if (oldWidth < newWidth && oldHeigt >= newHeight)
            {
                scaleRate = newHeight * 1f / oldHeigt;
            }
            else
            {
                int widthDis = newWidth - oldWidth;
                int heightDis = newHeight - oldHeigt;
                if (widthDis > heightDis)
                {
                    scaleRate = newHeight * 1f / oldHeigt;
                }
                else
                {
                    scaleRate = newWidth * 1f / oldWidth;
                }
            }
            return scaleRate;
        }

        /// <summary>
        /// 剪裁图片
        /// </summary>
        /// <param name="src">原图片</param>
        /// <param name="left">左坐标</param>
        /// <param name="top">顶部坐标</param>
        /// <param name="right">右坐标</param>
        /// <param name="bottom">底部坐标</param>
        /// <returns>剪裁后的图片</returns>
        public static Image CutImage(Image src,int left,int top,int right,int bottom)
        {
            try
            {
                Bitmap srcBitmap = new Bitmap(src);
                Bitmap dstBitmap = srcBitmap.Clone(new Rectangle(left,top,right - left,bottom - top),PixelFormat.DontCare);
                return dstBitmap;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }


    }
}
