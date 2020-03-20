namespace WindowsFormsApplication1
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.ShowText = new System.Windows.Forms.RichTextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.OnlineText = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.timeText = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.clearTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.ErrorInfo = new System.Windows.Forms.RichTextBox();
            this.userText = new System.Windows.Forms.RichTextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.sendText = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.button8 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.portText = new System.Windows.Forms.TextBox();
            this.portBtn = new System.Windows.Forms.Button();
            this.RoomNumText = new System.Windows.Forms.Label();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.RealOnline = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 7F);
            this.button1.Location = new System.Drawing.Point(27, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 43);
            this.button1.TabIndex = 0;
            this.button1.Text = "启动服务器";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ShowText
            // 
            this.ShowText.Location = new System.Drawing.Point(19, 24);
            this.ShowText.Name = "ShowText";
            this.ShowText.Size = new System.Drawing.Size(321, 270);
            this.ShowText.TabIndex = 1;
            this.ShowText.Text = "";
            this.ShowText.TextChanged += new System.EventHandler(this.ShowText_TextChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1080, 391);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(90, 28);
            this.button3.TabIndex = 3;
            this.button3.Text = "连接测试";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // OnlineText
            // 
            this.OnlineText.AutoSize = true;
            this.OnlineText.Location = new System.Drawing.Point(1080, 22);
            this.OnlineText.Name = "OnlineText";
            this.OnlineText.Size = new System.Drawing.Size(75, 15);
            this.OnlineText.TabIndex = 4;
            this.OnlineText.Text = "连接数：0";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.timeText,
            this.toolStripStatusLabel1,
            this.clearTime});
            this.statusStrip1.Location = new System.Drawing.Point(0, 440);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1182, 25);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // timeText
            // 
            this.timeText.Name = "timeText";
            this.timeText.Size = new System.Drawing.Size(167, 20);
            this.timeText.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(13, 20);
            this.toolStripStatusLabel1.Text = "|";
            // 
            // clearTime
            // 
            this.clearTime.Name = "clearTime";
            this.clearTime.Size = new System.Drawing.Size(167, 20);
            this.clearTime.Text = "toolStripStatusLabel2";
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button7);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.ErrorInfo);
            this.groupBox1.Controls.Add(this.userText);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.sendText);
            this.groupBox1.Controls.Add(this.ShowText);
            this.groupBox1.Location = new System.Drawing.Point(147, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(919, 407);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "日志信息";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(544, 311);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 27);
            this.button7.TabIndex = 8;
            this.button7.Text = "清屏";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(357, 312);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(73, 26);
            this.button6.TabIndex = 7;
            this.button6.Text = "清屏";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(19, 354);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 28);
            this.button2.TabIndex = 6;
            this.button2.Text = "清屏";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ErrorInfo
            // 
            this.ErrorInfo.Location = new System.Drawing.Point(544, 24);
            this.ErrorInfo.Name = "ErrorInfo";
            this.ErrorInfo.Size = new System.Drawing.Size(351, 270);
            this.ErrorInfo.TabIndex = 5;
            this.ErrorInfo.Text = "";
            this.ErrorInfo.TextChanged += new System.EventHandler(this.ErrorInfo_TextChanged);
            // 
            // userText
            // 
            this.userText.Location = new System.Drawing.Point(357, 24);
            this.userText.Name = "userText";
            this.userText.Size = new System.Drawing.Size(171, 270);
            this.userText.TabIndex = 4;
            this.userText.Text = "";
            this.userText.TextChanged += new System.EventHandler(this.userText_TextChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(261, 313);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(79, 25);
            this.button4.TabIndex = 3;
            this.button4.Text = "发送";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // sendText
            // 
            this.sendText.Location = new System.Drawing.Point(19, 313);
            this.sendText.Name = "sendText";
            this.sendText.Size = new System.Drawing.Size(218, 25);
            this.sendText.TabIndex = 2;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(1081, 325);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(90, 35);
            this.button5.TabIndex = 7;
            this.button5.Text = "添加用户";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // timer3
            // 
            this.timer3.Interval = 1000;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // button8
            // 
            this.button8.Font = new System.Drawing.Font("宋体", 8F);
            this.button8.Location = new System.Drawing.Point(12, 80);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(129, 48);
            this.button8.TabIndex = 8;
            this.button8.Text = "开启自动清屏";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1081, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "远程端口：";
            // 
            // portText
            // 
            this.portText.Location = new System.Drawing.Point(1072, 153);
            this.portText.Name = "portText";
            this.portText.Size = new System.Drawing.Size(100, 25);
            this.portText.TabIndex = 11;
            this.portText.Text = "15267";
            this.portText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.portText_KeyPress);
            // 
            // portBtn
            // 
            this.portBtn.Location = new System.Drawing.Point(1083, 184);
            this.portBtn.Name = "portBtn";
            this.portBtn.Size = new System.Drawing.Size(75, 35);
            this.portBtn.TabIndex = 12;
            this.portBtn.Text = "设置";
            this.portBtn.UseVisualStyleBackColor = true;
            this.portBtn.Click += new System.EventHandler(this.port_Click);
            // 
            // RoomNumText
            // 
            this.RoomNumText.AutoSize = true;
            this.RoomNumText.Location = new System.Drawing.Point(1080, 50);
            this.RoomNumText.Name = "RoomNumText";
            this.RoomNumText.Size = new System.Drawing.Size(75, 15);
            this.RoomNumText.TabIndex = 13;
            this.RoomNumText.Text = "房间数：0";
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(1080, 239);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(89, 37);
            this.button10.TabIndex = 14;
            this.button10.Text = "房间列表";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(1081, 282);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(89, 37);
            this.button11.TabIndex = 15;
            this.button11.Text = "在线用户";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // RealOnline
            // 
            this.RealOnline.AutoSize = true;
            this.RealOnline.Location = new System.Drawing.Point(1081, 80);
            this.RealOnline.Name = "RealOnline";
            this.RealOnline.Size = new System.Drawing.Size(75, 15);
            this.RealOnline.TabIndex = 13;
            this.RealOnline.Text = "在线数：0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 465);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.RealOnline);
            this.Controls.Add(this.RoomNumText);
            this.Controls.Add(this.portBtn);
            this.Controls.Add(this.portText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.OnlineText);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "志鑫手游服务器";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox ShowText;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label OnlineText;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel timeText;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox sendText;
        private System.Windows.Forms.RichTextBox ErrorInfo;
        private System.Windows.Forms.RichTextBox userText;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel clearTime;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox portText;
        private System.Windows.Forms.Button portBtn;
        private System.Windows.Forms.Label RoomNumText;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Label RealOnline;
    }
}

