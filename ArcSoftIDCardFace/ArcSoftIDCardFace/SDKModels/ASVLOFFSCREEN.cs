using System.Runtime.InteropServices;

namespace ArcsoftIDCardFace.SDKModels
{
    /// <summary>
    /// 图像结构体
    /// </summary>
    public struct ASVLOFFSCREEN
    {
        /// <summary>
        /// RGB24图片格式
        /// </summary>
        public int u32PixelArrayFormat;

        /// <summary>
        /// 图像宽
        /// </summary>
        public int i32Width;

        /// <summary>
        /// 图像高
        /// </summary>
        public int i32Height;


        /// <summary>
        /// 指针数组
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = System.Runtime.InteropServices.UnmanagedType.SysUInt)]
        public System.IntPtr[] ppu8Plane;

        /// <summary>
        /// 整形数组
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I4)]
        public int[] pi32Pitch;

    }
}
