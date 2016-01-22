namespace QumarionForMMM
{
    partial class QumarionSettingGui
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.checkBoxUseAccelerometer = new System.Windows.Forms.CheckBox();
            this.groupBoxAccelLimit = new System.Windows.Forms.GroupBox();
            this.radioButtonAccelLimitZ = new System.Windows.Forms.RadioButton();
            this.radioButtonAccelLimitX = new System.Windows.Forms.RadioButton();
            this.radioButtonAccelLimitNone = new System.Windows.Forms.RadioButton();
            this.checkBoxUseAccelFilter = new System.Windows.Forms.CheckBox();
            this.textBoxLegIKScaleFactor = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTipLegScaleFactor = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxBindToGround = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxArmAngle = new System.Windows.Forms.TextBox();
            this.groupBoxAccelLimit.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxUseAccelerometer
            // 
            this.checkBoxUseAccelerometer.AutoSize = true;
            this.checkBoxUseAccelerometer.Location = new System.Drawing.Point(9, 12);
            this.checkBoxUseAccelerometer.Name = "checkBoxUseAccelerometer";
            this.checkBoxUseAccelerometer.Size = new System.Drawing.Size(122, 16);
            this.checkBoxUseAccelerometer.TabIndex = 0;
            this.checkBoxUseAccelerometer.Text = "加速度センサを使用";
            this.checkBoxUseAccelerometer.UseVisualStyleBackColor = true;
            this.checkBoxUseAccelerometer.CheckedChanged += new System.EventHandler(this.checkBoxUseAccelerometer_CheckedChanged);
            // 
            // groupBoxAccelLimit
            // 
            this.groupBoxAccelLimit.Controls.Add(this.radioButtonAccelLimitZ);
            this.groupBoxAccelLimit.Controls.Add(this.radioButtonAccelLimitX);
            this.groupBoxAccelLimit.Controls.Add(this.radioButtonAccelLimitNone);
            this.groupBoxAccelLimit.Location = new System.Drawing.Point(3, 67);
            this.groupBoxAccelLimit.Name = "groupBoxAccelLimit";
            this.groupBoxAccelLimit.Size = new System.Drawing.Size(186, 95);
            this.groupBoxAccelLimit.TabIndex = 1;
            this.groupBoxAccelLimit.TabStop = false;
            this.groupBoxAccelLimit.Text = "加速度センサの回転制限";
            // 
            // radioButtonAccelLimitZ
            // 
            this.radioButtonAccelLimitZ.AutoSize = true;
            this.radioButtonAccelLimitZ.Location = new System.Drawing.Point(6, 62);
            this.radioButtonAccelLimitZ.Name = "radioButtonAccelLimitZ";
            this.radioButtonAccelLimitZ.Size = new System.Drawing.Size(87, 16);
            this.radioButtonAccelLimitZ.TabIndex = 4;
            this.radioButtonAccelLimitZ.TabStop = true;
            this.radioButtonAccelLimitZ.Text = "Z軸方向のみ";
            this.radioButtonAccelLimitZ.UseVisualStyleBackColor = true;
            this.radioButtonAccelLimitZ.CheckedChanged += new System.EventHandler(this.radioButtonAccelLimitZ_CheckedChanged);
            // 
            // radioButtonAccelLimitX
            // 
            this.radioButtonAccelLimitX.AutoSize = true;
            this.radioButtonAccelLimitX.Location = new System.Drawing.Point(6, 40);
            this.radioButtonAccelLimitX.Name = "radioButtonAccelLimitX";
            this.radioButtonAccelLimitX.Size = new System.Drawing.Size(87, 16);
            this.radioButtonAccelLimitX.TabIndex = 3;
            this.radioButtonAccelLimitX.TabStop = true;
            this.radioButtonAccelLimitX.Text = "X軸方向のみ";
            this.radioButtonAccelLimitX.UseVisualStyleBackColor = true;
            this.radioButtonAccelLimitX.CheckedChanged += new System.EventHandler(this.radioButtonAccelLimitX_CheckedChanged);
            // 
            // radioButtonAccelLimitNone
            // 
            this.radioButtonAccelLimitNone.AutoSize = true;
            this.radioButtonAccelLimitNone.Checked = true;
            this.radioButtonAccelLimitNone.Location = new System.Drawing.Point(6, 18);
            this.radioButtonAccelLimitNone.Name = "radioButtonAccelLimitNone";
            this.radioButtonAccelLimitNone.Size = new System.Drawing.Size(90, 16);
            this.radioButtonAccelLimitNone.TabIndex = 2;
            this.radioButtonAccelLimitNone.TabStop = true;
            this.radioButtonAccelLimitNone.Text = "回転制限なし";
            this.radioButtonAccelLimitNone.UseVisualStyleBackColor = true;
            this.radioButtonAccelLimitNone.CheckedChanged += new System.EventHandler(this.radioButtonAccelLimitNone_CheckedChanged);
            // 
            // checkBoxUseAccelFilter
            // 
            this.checkBoxUseAccelFilter.AutoSize = true;
            this.checkBoxUseAccelFilter.Location = new System.Drawing.Point(9, 34);
            this.checkBoxUseAccelFilter.Name = "checkBoxUseAccelFilter";
            this.checkBoxUseAccelFilter.Size = new System.Drawing.Size(164, 16);
            this.checkBoxUseAccelFilter.TabIndex = 2;
            this.checkBoxUseAccelFilter.Text = "加速度センサにフィルタを適用";
            this.checkBoxUseAccelFilter.UseVisualStyleBackColor = true;
            this.checkBoxUseAccelFilter.CheckedChanged += new System.EventHandler(this.checkBoxUseAccelFilter_CheckedChanged);
            // 
            // textBoxLegIKScaleFactor
            // 
            this.textBoxLegIKScaleFactor.Location = new System.Drawing.Point(96, 215);
            this.textBoxLegIKScaleFactor.Name = "textBoxLegIKScaleFactor";
            this.textBoxLegIKScaleFactor.Size = new System.Drawing.Size(100, 19);
            this.textBoxLegIKScaleFactor.TabIndex = 3;
            this.textBoxLegIKScaleFactor.Text = "10";
            this.textBoxLegIKScaleFactor.TextChanged += new System.EventHandler(this.textBoxLegIKScaleFactor_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 218);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "足の長さ補正値";
            this.toolTipLegScaleFactor.SetToolTip(this.label1, "QumarionとMMDモデルの足の長さは異なるため、\r\n足IKの結果をうまく反映するために調整します。\r\n0より大きい小数値(通常は5~20程度)で設定します。" +
        "\r\n足が短いキャラには小さい値、足が長いキャラには大きい値を適用すると\r\nQumarionと動きが揃うようになります。");
            // 
            // checkBoxBindToGround
            // 
            this.checkBoxBindToGround.AutoSize = true;
            this.checkBoxBindToGround.Location = new System.Drawing.Point(9, 190);
            this.checkBoxBindToGround.Name = "checkBoxBindToGround";
            this.checkBoxBindToGround.Size = new System.Drawing.Size(93, 16);
            this.checkBoxBindToGround.TabIndex = 5;
            this.checkBoxBindToGround.Text = "足を常時接地";
            this.checkBoxBindToGround.UseVisualStyleBackColor = true;
            this.checkBoxBindToGround.CheckedChanged += new System.EventHandler(this.checkBoxBindToGround_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 243);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "腕下げ角度(deg)";
            this.toolTipLegScaleFactor.SetToolTip(this.label2, "QumarionとMMDモデルの足の長さは異なるため、\r\n足IKの結果をうまく反映するために調整します。\r\n0より大きい小数値(通常は5~20程度)で設定します。" +
        "\r\n足が短いキャラには小さい値、足が長いキャラには大きい値を適用すると\r\nQumarionと動きが揃うようになります。");
            // 
            // textBoxArmAngle
            // 
            this.textBoxArmAngle.Location = new System.Drawing.Point(96, 240);
            this.textBoxArmAngle.Name = "textBoxArmAngle";
            this.textBoxArmAngle.Size = new System.Drawing.Size(100, 19);
            this.textBoxArmAngle.TabIndex = 7;
            this.textBoxArmAngle.Text = "35";
            this.textBoxArmAngle.TextChanged += new System.EventHandler(this.textBoxArmAngle_TextChanged);
            // 
            // QumarionSettingGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxArmAngle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBoxBindToGround);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxLegIKScaleFactor);
            this.Controls.Add(this.checkBoxUseAccelFilter);
            this.Controls.Add(this.groupBoxAccelLimit);
            this.Controls.Add(this.checkBoxUseAccelerometer);
            this.Name = "QumarionSettingGui";
            this.Size = new System.Drawing.Size(449, 407);
            this.groupBoxAccelLimit.ResumeLayout(false);
            this.groupBoxAccelLimit.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxUseAccelerometer;
        private System.Windows.Forms.GroupBox groupBoxAccelLimit;
        private System.Windows.Forms.RadioButton radioButtonAccelLimitNone;
        private System.Windows.Forms.RadioButton radioButtonAccelLimitX;
        private System.Windows.Forms.RadioButton radioButtonAccelLimitZ;
        private System.Windows.Forms.CheckBox checkBoxUseAccelFilter;
        private System.Windows.Forms.TextBox textBoxLegIKScaleFactor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTipLegScaleFactor;
        private System.Windows.Forms.CheckBox checkBoxBindToGround;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxArmAngle;
    }
}
