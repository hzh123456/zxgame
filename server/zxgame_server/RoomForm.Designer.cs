namespace zxgame_server
{
    partial class RoomForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoomForm));
            this.data = new System.Windows.Forms.DataGridView();
            this.roomid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fangzhu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.playernum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.style = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.data1 = new System.Windows.Forms.DataGridView();
            this.username = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.data)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.data1)).BeginInit();
            this.SuspendLayout();
            // 
            // data
            // 
            this.data.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.roomid,
            this.type,
            this.fangzhu,
            this.playernum,
            this.style});
            this.data.Location = new System.Drawing.Point(12, 55);
            this.data.Name = "data";
            this.data.RowTemplate.Height = 27;
            this.data.Size = new System.Drawing.Size(764, 288);
            this.data.TabIndex = 0;
            this.data.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.data_CellClick);
            // 
            // roomid
            // 
            this.roomid.HeaderText = "房间号";
            this.roomid.Name = "roomid";
            this.roomid.ReadOnly = true;
            // 
            // type
            // 
            this.type.HeaderText = "房间类型";
            this.type.Name = "type";
            this.type.ReadOnly = true;
            // 
            // fangzhu
            // 
            this.fangzhu.HeaderText = "房主";
            this.fangzhu.Name = "fangzhu";
            this.fangzhu.ReadOnly = true;
            // 
            // playernum
            // 
            this.playernum.HeaderText = "房间最大玩家数";
            this.playernum.Name = "playernum";
            this.playernum.ReadOnly = true;
            this.playernum.Width = 120;
            // 
            // style
            // 
            this.style.HeaderText = "房间状态";
            this.style.Name = "style";
            this.style.ReadOnly = true;
            // 
            // data1
            // 
            this.data1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.username,
            this.lastname});
            this.data1.Location = new System.Drawing.Point(820, 55);
            this.data1.Name = "data1";
            this.data1.RowTemplate.Height = 27;
            this.data1.Size = new System.Drawing.Size(302, 288);
            this.data1.TabIndex = 1;
            // 
            // username
            // 
            this.username.HeaderText = "用户名";
            this.username.Name = "username";
            this.username.ReadOnly = true;
            // 
            // lastname
            // 
            this.lastname.HeaderText = "昵称";
            this.lastname.Name = "lastname";
            this.lastname.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "房间信息：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(816, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(169, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "房间内玩家信息：";
            // 
            // RoomForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1161, 392);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.data1);
            this.Controls.Add(this.data);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RoomForm";
            this.Text = "RoomForm";
            this.Load += new System.EventHandler(this.RoomForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.data)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.data1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView data;
        private System.Windows.Forms.DataGridView data1;
        private System.Windows.Forms.DataGridViewTextBoxColumn username;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn roomid;
        private System.Windows.Forms.DataGridViewTextBoxColumn type;
        private System.Windows.Forms.DataGridViewTextBoxColumn fangzhu;
        private System.Windows.Forms.DataGridViewTextBoxColumn playernum;
        private System.Windows.Forms.DataGridViewTextBoxColumn style;
    }
}