using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArcsoftIDCardFace.SDKUtil
{
    class ReadIDCardFunctuions
    {
        /// <summary>
        /// SDK动态链接库路径
        /// </summary>
        public const string Dll_PATH = "身份证阅读器.dll库，使用时请正确填写名称，下面为引用C++ dll库示例";

        /// <summary>
        ///   初始化连接;
        /// </summary>
        /// <param name="iPort">串口</param>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CVR_InitComm(int iPort);

        /// <summary>
        /// 卡认证
        /// </summary>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern int CVR_Authenticate();

        /// <summary>
        /// 读卡操作
        /// </summary>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern int CVR_Read_Content(int active);

        /// <summary>
        /// 获取图片数据
        /// </summary>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetJpgData(byte[] pucPHMsg, ref int puiPHMsgLen);

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern int CVR_CloseComm();

        /// <summary>
        /// 得到姓名信息
        /// </summary>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetPeopleName(StringBuilder strTmp, ref int strLen);

        /// <summary>
        /// 得到卡号信息
        /// </summary>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetPeopleIDCode(StringBuilder strTmp, ref int strLen);
    }
}
