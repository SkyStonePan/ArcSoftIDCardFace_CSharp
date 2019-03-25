using ArcsoftIDCardFace.SDKModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcsoftIDCardFace.SDKModels
{
    /// <summary>
    /// 图片检测结果结构体
    /// </summary>
    public struct AFIC_FSDK_FACERES
    {
        /// <summary>
        /// 检测到的人脸数
        /// </summary>
        public int nFace;                     

        /// <summary>
        /// 人脸框位置
        /// </summary>
        public RECT rcFace;                      
    }
}
