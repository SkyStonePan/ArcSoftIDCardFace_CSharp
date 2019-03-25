using ArcsoftIDCardFace.SDKModels;
using ArcsoftIDCardFace.SDKUtil;
using System;
using System.Drawing;

namespace ArcsoftIDCardFace.Utils
{
    class IDCardUtil
    {
        private static object locks = new object();

        /// <summary>
        /// 人脸特征提取
        /// </summary>
        /// <param name="hFICEngine">FIC 引擎Handle</param>
        /// <param name="isVideo">人脸数据类型 1-视频 0-静态图片</param>
        /// <param name="bitmap">人脸图像原始数据</param>
        /// <returns>人脸检测结果</returns>
        public static int FaceDataFeatureExtraction(IntPtr hFICEngine, bool isVideo, Bitmap bitmap, ref AFIC_FSDK_FACERES faceRes)
        {
            lock (locks)
            {
                int result = -1;
                if (bitmap != null)
                {
                    ASVLOFFSCREEN offInput = ImageUtil.ReadBmp(bitmap);

                    IntPtr offInputPtr = MemoryUtil.Malloc(MemoryUtil.SizeOf<ASVLOFFSCREEN>());
                    MemoryUtil.StructureToPtr(offInput, offInputPtr);

                    IntPtr faceResPtr = MemoryUtil.Malloc(MemoryUtil.SizeOf<AFIC_FSDK_FACERES>());
                    result = ASIDCardFunctions.ArcSoft_FIC_FaceDataFeatureExtraction(hFICEngine, isVideo, offInputPtr, faceResPtr);
                    faceRes = MemoryUtil.PtrToStructure<AFIC_FSDK_FACERES>(faceResPtr);
                    MemoryUtil.Free(offInput.ppu8Plane[0]);
                    MemoryUtil.Free(offInputPtr);
                    MemoryUtil.Free(faceResPtr);
                }
                return result;
            }
        }

        /// <summary>
        /// 证件照特征提取
        /// </summary>
        /// <param name="hFICEngine">FIC 引擎Handle</param>
        /// <param name="isVideo">人脸数据类型 1-视频 0-静态图片</param>
        /// <param name="bitmap">人脸图像原始数据</param>
        /// <returns>人脸特征提取结果</returns>
        public static int IdCardDataFeatureExtraction(IntPtr hFICEngine, Image image)
        {
            lock (locks)
            {
                if (image.Width % 4 != 0)
                {
                    image = ImageUtil.ScaleImage(image, image.Width - (image.Width % 4), image.Height);
                }
                //Bitmap bitmap = new Bitmap(image);
                ASVLOFFSCREEN offInput = ImageUtil.ReadBmp(image);

                IntPtr offInputPtr = MemoryUtil.Malloc(MemoryUtil.SizeOf<ASVLOFFSCREEN>());
                MemoryUtil.StructureToPtr(offInput, offInputPtr);

                IntPtr faceResPtr = MemoryUtil.Malloc(MemoryUtil.SizeOf<AFIC_FSDK_FACERES>());
                int result = ASIDCardFunctions.ArcSoft_FIC_IdCardDataFeatureExtraction(hFICEngine, offInputPtr);
                MemoryUtil.Free(offInput.ppu8Plane[0]);
                MemoryUtil.Free(offInputPtr);
                MemoryUtil.Free(faceResPtr);
                return result;
            }
        }

        /// <summary>
        /// 人证比对
        /// </summary>
        /// <param name="pSimilarScore">FIC 引擎Handle</param>
        /// <param name="pResult">人脸数据类型 1-视频 0-静态图片</param>
        /// <param name="hFICEngine">引擎Handle</param>
        /// <param name="threshold">引擎Handle</param>
        /// <returns>人脸比对结果</returns>
        public static int FaceIdCardCompare(ref float pSimilarScore, ref int pResult, IntPtr hFICEngine, float threshold = 0.82f)
        {
            return ASIDCardFunctions.ArcSoft_FIC_FaceIdCardCompare(hFICEngine, threshold, ref pSimilarScore, ref pResult);
        }

        /// <summary>
        /// 获取多个人脸检测结果中面积最大的人脸
        /// </summary>
        /// <param name="multiFaceInfo">人脸检测结果</param>
        /// <returns>面积最大的人脸信息</returns>
        public static ASF_SingleFaceInfo GetMaxFace(AFIC_FSDK_FACERES multiFaceInfo)
        {
            ASF_SingleFaceInfo singleFaceInfo = new ASF_SingleFaceInfo();
            singleFaceInfo.faceRect = new RECT();
            singleFaceInfo.faceOrient = 1;

            int maxArea = 0;
            int index = -1;
            for (int i = 0; i < multiFaceInfo.nFace; i++)
            {
                RECT rect = multiFaceInfo.rcFace;
                int area = (rect.right - rect.left) * (rect.bottom - rect.top);
                if (maxArea <= area)
                {
                    maxArea = area;
                    index = i;
                }
            }
            if (index != -1)
            {
                singleFaceInfo.faceRect = multiFaceInfo.rcFace;
            }
            return singleFaceInfo;
        }

        /// <summary>
        /// 将身份证号字符串的年月日替换为*
        /// </summary>
        /// <param name="IDCard"></param>
        /// <returns></returns>
        public static string repleaseIDCard(string IDCard)
        {
            if (IDCard.Length == 15)
            {
                string date = IDCard.Substring(6, 6);
                return IDCard.Replace(date, "******");
            }
            else if (IDCard.Length == 18)
            {
                string date = IDCard.Substring(6, 8);
                return IDCard.Replace(date, "********");
            }
            else
            {
                return IDCard;
            }
        }
    } 
}

