using AForge.Video.DirectShow;
using ArcsoftIDCardFace.SDKModels;
using ArcsoftIDCardFace.SDKUtil;
using ArcsoftIDCardFace.Utils;
using System;
using System.Configuration;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ArcsoftIDCardFace
{
    public partial class ArcfaceIDCard : Form
    {
        //引擎Handle
        private IntPtr pEngine = IntPtr.Zero;

        //身份证图片byte
        private byte[] byteImage = new byte[40000];

        //是否显示相似度
        private bool isShow = false;

        private bool isRead = false;
        #region 视频相关
        /// <summary>
        /// 视频输入设备信息
        /// </summary>
        private FilterInfoCollection filterInfoCollection;
        private VideoCaptureDevice deviceVideo;
        #endregion

        public ArcfaceIDCard()
        {
            InitializeComponent();
            InitEngines();
        }

        private void ArcfaceIDCard_Load(object sender, EventArgs e)
        {
            //设置Timer控件可用
            timerRead.Enabled = true;
            //设置时间间隔（毫秒为单位）
            timerRead.Interval = 3000;
            //启动定时器
            timerRead.Start();
        }
        /// <summary>
        /// 初始化引擎
        /// </summary>
        private void InitEngines()
        {
            //读取配置文件
            AppSettingsReader reader = new AppSettingsReader();
            string appId = (string)reader.GetValue("APP_ID", typeof(string));
            string sdkKey64 = (string)reader.GetValue("SDKKEY64", typeof(string));
            string sdkKey32 = (string)reader.GetValue("SDKKEY32", typeof(string));

            var is64CPU = Environment.Is64BitProcess;
            if (is64CPU)
            {
                if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(sdkKey64))
                {
                 
                    MessageBox.Show("请在App.config配置文件中先配置APP_ID和SDKKEY64!");
                    return;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(sdkKey32))
                {
                  
                    MessageBox.Show("请在App.config配置文件中先配置APP_ID和SDKKEY32!");
                    return;
                }
            }

            //激活引擎    如出现错误，1.请先确认从官网下载的sdk库已放到对应的bin中，2.当前选择的CPU为x86或者x64
            int retCode = 0;

            try
            {
                retCode = ASIDCardFunctions.ArcSoft_FIC_Activate(appId, is64CPU ? sdkKey64 : sdkKey32);
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("无法加载 DLL") > -1)
                {
                    MessageBox.Show("请将sdk相关DLL放入bin对应的x86或x64下的文件夹中!");
                }
                else
                {
                    MessageBox.Show("激活引擎失败!");
                }
                return;
            }
            Console.WriteLine("Activate Result:" + retCode);

            //初始化引擎
            retCode =  ASIDCardFunctions.ArcSoft_FIC_InitialEngine(ref pEngine);
            Console.WriteLine("InitEngine Result:" + retCode);
            if (retCode != 0)
            {
                MessageBox.Show(string.Format("引擎初始化失败!错误码为:{0}\n", retCode));
                return;
            }
          
            initVideo();
            readIDCard();
          
        }

        /// <summary>
        /// 初始化摄像头
        /// </summary>
        private void initVideo()
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (filterInfoCollection.Count == 0)
            {
                MessageBox.Show("未检测到摄像头，请确保已安装摄像头或驱动!");
                return;
            }
            if (videoSource.IsRunning)
            {
                videoSource.SignalToStop(); //关闭，但是摄像头还是在使用
                videoSource.Hide();
            }
            else
            {
                //默认选中第一个
                deviceVideo = new VideoCaptureDevice(filterInfoCollection[0].MonikerString);
                deviceVideo.VideoResolution = deviceVideo.VideoCapabilities[0];
                videoSource.VideoSource = deviceVideo;
                videoSource.Start();
            }

        }
        
        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        private void ArcfaceIDCard_FormClosed(object sender, FormClosedEventArgs e)
        {
            //销毁引擎
            int retCode = ASIDCardFunctions.ArcSoft_FIC_UninitialEngine(pEngine);
            //ReadIDCardFunctuions.CVR_CloseComm();
            videoSource.SignalToStop();
            //关闭定时器
            timerRead.Stop();
            Console.WriteLine("UninitEngine Result:" + retCode);
        }

        /// <summary>
        ///图像显示到窗体上，得到每一帧图像，并进行处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void videoSourcePlayer_Paint(object sender, PaintEventArgs e)
        {
            if (videoSource.IsRunning)
            {
                Bitmap bitmap = videoSource.GetCurrentVideoFrame();
                if (bitmap == null)
                {
                    return;
                }
                Graphics g = e.Graphics;
                float offsetX = videoSource.Width * 1f / bitmap.Width;
                float offsetY = videoSource.Height * 1f / bitmap.Height;
                AFIC_FSDK_FACERES faceInfo = new AFIC_FSDK_FACERES();
                int result = IDCardUtil.FaceDataFeatureExtraction(pEngine, true, bitmap, ref faceInfo);
                if (result == 0 && faceInfo.nFace > 0)
                {
                    string message = "";
                    if (isShow && isRead)
                    {
                        float pSimilarScore = 0;
                        int pResult = 0;
                        float threshold = 0.82f;
                        float.TryParse(scoreText.Text, out threshold);
                        result = IDCardUtil.FaceIdCardCompare(ref pSimilarScore, ref pResult, pEngine, threshold);
                        if (result == 0)
                        {
                            if (threshold > pSimilarScore)
                            {
                                msgLabel.ForeColor = Color.Red;
                                msgLabel.Text = "人证核验失败";
                              
                            }
                            else
                            {
                                 msgLabel.ForeColor = Color.Green;
                                msgLabel.Text = "人证核验成功";
                            }
                        }
                        message = "相似度:" + pSimilarScore;
                    }
                    else
                    {
                        msgLabel.ForeColor = Color.Red;
                        msgLabel.Text = "请放置身份证";
                    }
                    ASF_SingleFaceInfo maxFace = IDCardUtil.GetMaxFace(faceInfo);
                    RECT rect = maxFace.faceRect;
                    float x = rect.left * offsetX;
                    float width = rect.right * offsetX - x;
                    float y = rect.top * offsetY;
                    float height = rect.bottom * offsetY - y;
                    using (Pen pen = new Pen(Color.Red))
                    {
                        g.DrawRectangle(pen, x, y, width, height);
                    }

                }
            }
        }
         
        /// <summary>
        /// 初始化读卡器
        /// </summary>
        private bool initReader()
        {
            //初始化读卡器，如果可用返回true
            
            return true;
        }

        /// <summary>
        /// 读卡器读取内容
        /// </summary>
        /// <returns></returns>
        private bool readContent()
        {
            isShow = false;
            //读取身份证信息，这里默认读取成功，具体信息需要根据读卡器内容进行具体实现
            BeginInvoke(new Action(delegate
            {
                IDCardPic.Visible = true;
                Image image = Image.FromFile(@"替换为当前可用图片绝对地址");
                //调整图像宽度，图像宽度必须为4的倍数
                if (image.Width % 4 != 0)
                {
                    image = ImageUtil.ScaleImage(image, image.Width - (image.Width % 4), image.Height);
                }
                IDCardPic.Image = image;

                nameLabel.Text = "姓名:张三";
                IdCardLabel.Text = "证件号:" + IDCardUtil.repleaseIDCard("111111888888888888");
                msgLabel.ForeColor = Color.Red;
                msgLabel.Text = "请正对摄像机！";
                isRead = true;
                //提取图片特征值
                int result = IDCardUtil.IdCardDataFeatureExtraction(pEngine, image);
                if (result == 0)
                {
                    isShow = true;
                }
            }));
            return true;
        }

        /// <summary>
        /// 读卡器线程
        /// </summary>
        private void readIDCard()
        {
            initReader();
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
            {
                while (true)
                {
                    Thread.Sleep(300);

                    if (!readContent())
                    {
                        Thread.Sleep(300);
                        BeginInvoke(new Action(delegate
                        {
                            msgLabel.ForeColor = Color.Red;
                            msgLabel.Text = "读卡失败！";
                        }));
                    }
                }
            }));
        }

        /// <summary>
        /// 3秒刷新读取身份证数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerRead_Tick(object sender, EventArgs e)
        {
            if (IDCardPic.Image != null)
            {
                IDCardPic.Visible = false;
            }
            isRead = true;
            nameLabel.Text = "姓名:" ;
            IdCardLabel.Text = "证件号:";
            msgLabel.Text = "";
        }
    }
}
