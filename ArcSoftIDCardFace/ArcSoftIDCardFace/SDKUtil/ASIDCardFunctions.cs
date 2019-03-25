using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArcsoftIDCardFace.SDKUtil
{
    /// <summary>
    /// SDK中与人证相关函数封装类
    /// </summary>
    class ASIDCardFunctions
    {
        /// <summary>
        /// SDK动态链接库路径
        /// </summary>
        public const string Dll_PATH = "libarcsoft_idcardveri.dll";

        /// <summary>
        /// 激活人证SDK引擎函数
        /// </summary>
        /// <param name="appId">SDK对应的AppID</param>
        /// <param name="sdkKey">SDK对应的SDKKey</param>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ArcSoft_FIC_Activate(string appId, string sdkKey);

        /// <summary>
        /// 初始化引擎
        /// </summary>
        /// <param name="pEngine">初始化返回的引擎handle</param>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ArcSoft_FIC_InitialEngine(ref IntPtr pEngine);

        /// <summary>
        /// 人脸特征提取 0-静态图片 1-视频 
        /// </summary>
        /// <param name="pEngine">初始化返回的引擎handle</param>
        /// <param name="isVideo"> 人脸数据类型 1-视频 0-静态图片</param>
        /// <param name="pInputFaceData">人脸图像原始数据</param>
        /// <param name="pFaceRes">人脸属性 人脸数/人脸框/角度</param>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ArcSoft_FIC_FaceDataFeatureExtraction(IntPtr pEngine, bool isVideo, IntPtr pInputFaceData, IntPtr pFaceRes);

        /// <summary>
        /// 证件照特征提取
        /// </summary>
        /// <param name="pEngine">初始化返回的引擎handle</param>
        /// <param name="pInputFaceData">图像原始数据</param>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ArcSoft_FIC_IdCardDataFeatureExtraction(IntPtr pEngine, IntPtr pInputFaceData);

        /// <summary>
        /// 人证比对
        /// </summary>
        /// <param name="pEngine">初始化返回的引擎handle</param>
        /// <param name="threshold">比对阈值</param>
        /// <param name="pSimilarScore">比对结果相似度</param>
        /// <param name="pResult">比对结果</param>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ArcSoft_FIC_FaceIdCardCompare(IntPtr pEngine, float threshold, ref float pSimilarScore,ref int pResult);

        /// <summary>
        /// 释放引擎
        /// </summary>
        /// <param name="pResult">引擎Handle</param>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ArcSoft_FIC_UninitialEngine(IntPtr pEngine);


    }
}
