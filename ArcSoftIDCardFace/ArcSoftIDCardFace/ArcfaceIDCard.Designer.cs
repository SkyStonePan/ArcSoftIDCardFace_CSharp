namespace ArcsoftIDCardFace
{
    partial class ArcfaceIDCard
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArcfaceIDCard));
            this.IDCardPic = new System.Windows.Forms.PictureBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.IdCardLabel = new System.Windows.Forms.Label();
            this.scoreLabel = new System.Windows.Forms.Label();
            this.scoreText = new System.Windows.Forms.TextBox();
            this.messageLabel = new System.Windows.Forms.Label();
            this.timerRead = new System.Windows.Forms.Timer(this.components);
            this.videoSource = new AForge.Controls.VideoSourcePlayer();
            this.msgLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.IDCardPic)).BeginInit();
            this.SuspendLayout();
            // 
            // IDCardPic
            // 
            this.IDCardPic.Location = new System.Drawing.Point(650, 85);
            this.IDCardPic.Name = "IDCardPic";
            this.IDCardPic.Size = new System.Drawing.Size(128, 133);
            this.IDCardPic.TabIndex = 43;
            this.IDCardPic.TabStop = false;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(640, 276);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(35, 12);
            this.nameLabel.TabIndex = 44;
            this.nameLabel.Text = "姓名:";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // IdCardLabel
            // 
            this.IdCardLabel.AutoSize = true;
            this.IdCardLabel.Location = new System.Drawing.Point(628, 319);
            this.IdCardLabel.Name = "IdCardLabel";
            this.IdCardLabel.Size = new System.Drawing.Size(47, 12);
            this.IdCardLabel.TabIndex = 45;
            this.IdCardLabel.Text = "证件号:";
            this.IdCardLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // scoreLabel
            // 
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.Location = new System.Drawing.Point(640, 361);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(35, 12);
            this.scoreLabel.TabIndex = 46;
            this.scoreLabel.Text = "阈值:";
            // 
            // scoreText
            // 
            this.scoreText.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.scoreText.Location = new System.Drawing.Point(678, 358);
            this.scoreText.Name = "scoreText";
            this.scoreText.Size = new System.Drawing.Size(100, 23);
            this.scoreText.TabIndex = 47;
            this.scoreText.Text = "0.82";
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.Location = new System.Drawing.Point(616, 402);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(59, 12);
            this.messageLabel.TabIndex = 49;
            this.messageLabel.Text = "提示信息:";
            // 
            // timerRead
            // 
            this.timerRead.Enabled = true;
            this.timerRead.Interval = 3000;
            this.timerRead.Tick += new System.EventHandler(this.timerRead_Tick);
            // 
            // videoSource
            // 
            this.videoSource.Location = new System.Drawing.Point(24, 22);
            this.videoSource.Name = "videoSource";
            this.videoSource.Size = new System.Drawing.Size(556, 438);
            this.videoSource.TabIndex = 0;
            this.videoSource.Text = "videoSourcePlayer1";
            this.videoSource.VideoSource = null;
            this.videoSource.Paint += new System.Windows.Forms.PaintEventHandler(this.videoSourcePlayer_Paint);
            // 
            // msgLabel
            // 
            this.msgLabel.AutoSize = true;
            this.msgLabel.Location = new System.Drawing.Point(682, 402);
            this.msgLabel.Name = "msgLabel";
            this.msgLabel.Size = new System.Drawing.Size(0, 12);
            this.msgLabel.TabIndex = 50;
            // 
            // ArcfaceIDCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 472);
            this.Controls.Add(this.msgLabel);
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.scoreText);
            this.Controls.Add(this.scoreLabel);
            this.Controls.Add(this.IdCardLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.IDCardPic);
            this.Controls.Add(this.videoSource);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ArcfaceIDCard";
            this.Text = "ArcSoftFace IDCardVeri C# demo";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ArcfaceIDCard_FormClosed);
            this.Load += new System.EventHandler(this.ArcfaceIDCard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.IDCardPic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AForge.Controls.VideoSourcePlayer videoSource;
        private System.Windows.Forms.PictureBox IDCardPic;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label IdCardLabel;
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.TextBox scoreText;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Timer timerRead;
        private System.Windows.Forms.Label msgLabel;
    }
}

