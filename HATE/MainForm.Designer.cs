﻿namespace HATE
{
    partial class MainForm
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
                _logWriter.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnCorrupt = new System.Windows.Forms.Button();
            this.chbShuffleText = new System.Windows.Forms.CheckBox();
            this.chbShuffleGFX = new System.Windows.Forms.CheckBox();
            this.chbHitboxFix = new System.Windows.Forms.CheckBox();
            this.chbShuffleFont = new System.Windows.Forms.CheckBox();
            this.chbShuffleBG = new System.Windows.Forms.CheckBox();
            this.chbShuffleAudio = new System.Windows.Forms.CheckBox();
            this.chbShowSeed = new System.Windows.Forms.CheckBox();
            this.txtSeed = new System.Windows.Forms.TextBox();
            this.txtPower = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLaunch = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblGameName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCorrupt
            // 
            this.btnCorrupt.BackColor = System.Drawing.Color.Black;
            this.btnCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCorrupt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCorrupt.ForeColor = System.Drawing.Color.Coral;
            this.btnCorrupt.Location = new System.Drawing.Point(22, 503);
            this.btnCorrupt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCorrupt.Name = "btnCorrupt";
            this.btnCorrupt.Size = new System.Drawing.Size(172, 33);
            this.btnCorrupt.TabIndex = 0;
            this.btnCorrupt.Text = "-CORRUPT-";
            this.btnCorrupt.UseVisualStyleBackColor = false;
            this.btnCorrupt.Click += new System.EventHandler(this.button_Corrupt_Clicked);
            // 
            // chbShuffleText
            // 
            this.chbShuffleText.AutoSize = true;
            this.chbShuffleText.BackColor = System.Drawing.Color.Black;
            this.chbShuffleText.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.chbShuffleText.FlatAppearance.BorderSize = 0;
            this.chbShuffleText.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.chbShuffleText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chbShuffleText.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chbShuffleText.ForeColor = System.Drawing.Color.White;
            this.chbShuffleText.Location = new System.Drawing.Point(14, 326);
            this.chbShuffleText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbShuffleText.Name = "chbShuffleText";
            this.chbShuffleText.Size = new System.Drawing.Size(150, 31);
            this.chbShuffleText.TabIndex = 1;
            this.chbShuffleText.Text = "Shuffle Text";
            this.chbShuffleText.UseVisualStyleBackColor = false;
            this.chbShuffleText.CheckedChanged += new System.EventHandler(this.chbShuffleText_CheckedChanged);
            // 
            // chbShuffleGFX
            // 
            this.chbShuffleGFX.AutoSize = true;
            this.chbShuffleGFX.BackColor = System.Drawing.Color.Black;
            this.chbShuffleGFX.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.chbShuffleGFX.FlatAppearance.BorderSize = 0;
            this.chbShuffleGFX.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.chbShuffleGFX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chbShuffleGFX.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chbShuffleGFX.ForeColor = System.Drawing.Color.White;
            this.chbShuffleGFX.Location = new System.Drawing.Point(14, 277);
            this.chbShuffleGFX.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbShuffleGFX.Name = "chbShuffleGFX";
            this.chbShuffleGFX.Size = new System.Drawing.Size(172, 31);
            this.chbShuffleGFX.TabIndex = 2;
            this.chbShuffleGFX.Text = "Shuffle Sprites";
            this.chbShuffleGFX.UseVisualStyleBackColor = false;
            this.chbShuffleGFX.CheckedChanged += new System.EventHandler(this.chbShuffleGFX_CheckedChanged);
            // 
            // chbHitboxFix
            // 
            this.chbHitboxFix.AutoSize = true;
            this.chbHitboxFix.BackColor = System.Drawing.Color.Black;
            this.chbHitboxFix.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.chbHitboxFix.FlatAppearance.BorderSize = 0;
            this.chbHitboxFix.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.chbHitboxFix.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chbHitboxFix.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chbHitboxFix.ForeColor = System.Drawing.Color.White;
            this.chbHitboxFix.Location = new System.Drawing.Point(13, 229);
            this.chbHitboxFix.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbHitboxFix.Name = "chbHitboxFix";
            this.chbHitboxFix.Size = new System.Drawing.Size(125, 31);
            this.chbHitboxFix.TabIndex = 3;
            this.chbHitboxFix.Text = "Hitbox Fix";
            this.chbHitboxFix.UseVisualStyleBackColor = false;
            this.chbHitboxFix.CheckedChanged += new System.EventHandler(this.chbHitboxFix_CheckedChanged);
            // 
            // chbShuffleFont
            // 
            this.chbShuffleFont.AutoSize = true;
            this.chbShuffleFont.BackColor = System.Drawing.Color.Black;
            this.chbShuffleFont.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.chbShuffleFont.FlatAppearance.BorderSize = 0;
            this.chbShuffleFont.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.chbShuffleFont.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chbShuffleFont.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chbShuffleFont.ForeColor = System.Drawing.Color.White;
            this.chbShuffleFont.Location = new System.Drawing.Point(13, 180);
            this.chbShuffleFont.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbShuffleFont.Name = "chbShuffleFont";
            this.chbShuffleFont.Size = new System.Drawing.Size(156, 31);
            this.chbShuffleFont.TabIndex = 4;
            this.chbShuffleFont.Text = "Shuffle Fonts";
            this.chbShuffleFont.UseVisualStyleBackColor = false;
            this.chbShuffleFont.CheckedChanged += new System.EventHandler(this.chbShuffleFont_CheckedChanged);
            // 
            // chbShuffleBG
            // 
            this.chbShuffleBG.AutoSize = true;
            this.chbShuffleBG.BackColor = System.Drawing.Color.Black;
            this.chbShuffleBG.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.chbShuffleBG.FlatAppearance.BorderSize = 0;
            this.chbShuffleBG.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.chbShuffleBG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chbShuffleBG.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chbShuffleBG.ForeColor = System.Drawing.Color.White;
            this.chbShuffleBG.Location = new System.Drawing.Point(13, 132);
            this.chbShuffleBG.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbShuffleBG.Name = "chbShuffleBG";
            this.chbShuffleBG.Size = new System.Drawing.Size(145, 31);
            this.chbShuffleBG.TabIndex = 5;
            this.chbShuffleBG.Text = "Shuffle GFX";
            this.chbShuffleBG.UseVisualStyleBackColor = false;
            this.chbShuffleBG.CheckedChanged += new System.EventHandler(this.chbShuffleBG_CheckedChanged);
            // 
            // chbShuffleAudio
            // 
            this.chbShuffleAudio.AutoSize = true;
            this.chbShuffleAudio.BackColor = System.Drawing.Color.Black;
            this.chbShuffleAudio.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.chbShuffleAudio.FlatAppearance.BorderSize = 0;
            this.chbShuffleAudio.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.chbShuffleAudio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chbShuffleAudio.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chbShuffleAudio.ForeColor = System.Drawing.Color.White;
            this.chbShuffleAudio.Location = new System.Drawing.Point(13, 84);
            this.chbShuffleAudio.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbShuffleAudio.Name = "chbShuffleAudio";
            this.chbShuffleAudio.Size = new System.Drawing.Size(156, 31);
            this.chbShuffleAudio.TabIndex = 6;
            this.chbShuffleAudio.Text = "Shuffle Audio";
            this.chbShuffleAudio.UseVisualStyleBackColor = false;
            this.chbShuffleAudio.CheckedChanged += new System.EventHandler(this.chbShuffleAudio_CheckedChanged);
            // 
            // chbShowSeed
            // 
            this.chbShowSeed.AutoSize = true;
            this.chbShowSeed.BackColor = System.Drawing.Color.Black;
            this.chbShowSeed.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.chbShowSeed.FlatAppearance.BorderSize = 0;
            this.chbShowSeed.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.chbShowSeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chbShowSeed.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chbShowSeed.ForeColor = System.Drawing.Color.White;
            this.chbShowSeed.Location = new System.Drawing.Point(13, 374);
            this.chbShowSeed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbShowSeed.Name = "chbShowSeed";
            this.chbShowSeed.Size = new System.Drawing.Size(129, 31);
            this.chbShowSeed.TabIndex = 7;
            this.chbShowSeed.Text = "Show Seed";
            this.chbShowSeed.UseVisualStyleBackColor = false;
            this.chbShowSeed.CheckedChanged += new System.EventHandler(this.chbShowSeed_CheckedChanged);
            // 
            // txtSeed
            // 
            this.txtSeed.BackColor = System.Drawing.Color.White;
            this.txtSeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtSeed.ForeColor = System.Drawing.Color.Black;
            this.txtSeed.Location = new System.Drawing.Point(77, 433);
            this.txtSeed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSeed.Name = "txtSeed";
            this.txtSeed.Size = new System.Drawing.Size(116, 20);
            this.txtSeed.TabIndex = 8;
            // 
            // txtPower
            // 
            this.txtPower.BackColor = System.Drawing.Color.White;
            this.txtPower.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtPower.ForeColor = System.Drawing.Color.Black;
            this.txtPower.Location = new System.Drawing.Point(77, 467);
            this.txtPower.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPower.Name = "txtPower";
            this.txtPower.Size = new System.Drawing.Size(116, 20);
            this.txtPower.TabIndex = 9;
            this.txtPower.Text = "0 - 255";
            this.txtPower.Enter += new System.EventHandler(this.txtPower_Enter);
            this.txtPower.Leave += new System.EventHandler(this.txtPower_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(8, 433);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "Seed:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(8, 467);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "Power:";
            // 
            // btnLaunch
            // 
            this.btnLaunch.BackColor = System.Drawing.Color.Black;
            this.btnLaunch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLaunch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLaunch.ForeColor = System.Drawing.Color.Fuchsia;
            this.btnLaunch.Location = new System.Drawing.Point(22, 544);
            this.btnLaunch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLaunch.Name = "btnLaunch";
            this.btnLaunch.Size = new System.Drawing.Size(172, 33);
            this.btnLaunch.TabIndex = 12;
            this.btnLaunch.Text = "-LAUNCH-";
            this.btnLaunch.UseVisualStyleBackColor = false;
            this.btnLaunch.Click += new System.EventHandler(this.btnLaunch_Clicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(14, 12);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 17);
            this.label3.TabIndex = 14;
            this.label3.Text = "Current Game:";
            // 
            // lblGameName
            // 
            this.lblGameName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGameName.AutoSize = true;
            this.lblGameName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblGameName.ForeColor = System.Drawing.Color.Yellow;
            this.lblGameName.Location = new System.Drawing.Point(14, 47);
            this.lblGameName.Margin = new System.Windows.Forms.Padding(4, 13, 4, 0);
            this.lblGameName.Name = "lblGameName";
            this.lblGameName.Size = new System.Drawing.Size(88, 20);
            this.lblGameName.TabIndex = 15;
            this.lblGameName.Text = "Undertale";
            this.lblGameName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(215, 587);
            this.Controls.Add(this.lblGameName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnLaunch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPower);
            this.Controls.Add(this.txtSeed);
            this.Controls.Add(this.chbShowSeed);
            this.Controls.Add(this.chbShuffleAudio);
            this.Controls.Add(this.chbShuffleBG);
            this.Controls.Add(this.chbShuffleFont);
            this.Controls.Add(this.chbHitboxFix);
            this.Controls.Add(this.chbShuffleGFX);
            this.Controls.Add(this.chbShuffleText);
            this.Controls.Add(this.btnCorrupt);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(231, 393);
            this.Name = "MainForm";
            this.Text = "HATE";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCorrupt;
        private System.Windows.Forms.CheckBox chbShuffleText;
        private System.Windows.Forms.CheckBox chbShuffleGFX;
        private System.Windows.Forms.CheckBox chbHitboxFix;
        private System.Windows.Forms.CheckBox chbShuffleFont;
        private System.Windows.Forms.CheckBox chbShuffleBG;
        private System.Windows.Forms.CheckBox chbShuffleAudio;
        private System.Windows.Forms.CheckBox chbShowSeed;
        private System.Windows.Forms.TextBox txtSeed;
        private System.Windows.Forms.TextBox txtPower;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnLaunch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblGameName;
    }
}

