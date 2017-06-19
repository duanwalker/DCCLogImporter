namespace DCCLogImporter
{
    partial class Form1
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
            this.btnStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBarTotalCompletion = new System.Windows.Forms.ProgressBar();
            this.lblTotalCompletion = new System.Windows.Forms.Label();
            this.lblFTP = new System.Windows.Forms.Label();
            this.progressBarFTP = new System.Windows.Forms.ProgressBar();
            this.lblRNPackage = new System.Windows.Forms.Label();
            this.progressBarRNPackage = new System.Windows.Forms.ProgressBar();
            this.lblEBSLoader = new System.Windows.Forms.Label();
            this.progressBarEBSLoader = new System.Windows.Forms.ProgressBar();
            this.lblDCCLoader = new System.Windows.Forms.Label();
            this.progressBarDCCLoader = new System.Windows.Forms.ProgressBar();
            this.lblDCCManifest = new System.Windows.Forms.Label();
            this.progressBarDCCManifest = new System.Windows.Forms.ProgressBar();
            this.lblFTPStatus = new System.Windows.Forms.Label();
            this.lblRNPackageStatus = new System.Windows.Forms.Label();
            this.lblEBSLoaderStatus = new System.Windows.Forms.Label();
            this.lblDCCLoaderStatus = new System.Windows.Forms.Label();
            this.lblDCCManifestStatus = new System.Windows.Forms.Label();
            this.lblFinalCompletion = new System.Windows.Forms.Label();
            this.lblFinalFails = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(12, 466);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(624, 63);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start Processing";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(305, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Click Start button below to Import files for processing";
            // 
            // progressBarTotalCompletion
            // 
            this.progressBarTotalCompletion.Location = new System.Drawing.Point(12, 61);
            this.progressBarTotalCompletion.Name = "progressBarTotalCompletion";
            this.progressBarTotalCompletion.Size = new System.Drawing.Size(624, 52);
            this.progressBarTotalCompletion.TabIndex = 6;
            // 
            // lblTotalCompletion
            // 
            this.lblTotalCompletion.AutoSize = true;
            this.lblTotalCompletion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCompletion.Location = new System.Drawing.Point(12, 43);
            this.lblTotalCompletion.Name = "lblTotalCompletion";
            this.lblTotalCompletion.Size = new System.Drawing.Size(116, 15);
            this.lblTotalCompletion.TabIndex = 8;
            this.lblTotalCompletion.Text = "Total Completion";
            // 
            // lblFTP
            // 
            this.lblFTP.AutoSize = true;
            this.lblFTP.Location = new System.Drawing.Point(15, 125);
            this.lblFTP.Name = "lblFTP";
            this.lblFTP.Size = new System.Drawing.Size(51, 13);
            this.lblFTP.TabIndex = 10;
            this.lblFTP.Text = "FTP Log ";
            // 
            // progressBarFTP
            // 
            this.progressBarFTP.Location = new System.Drawing.Point(15, 144);
            this.progressBarFTP.Name = "progressBarFTP";
            this.progressBarFTP.Size = new System.Drawing.Size(435, 23);
            this.progressBarFTP.TabIndex = 9;
            // 
            // lblRNPackage
            // 
            this.lblRNPackage.AutoSize = true;
            this.lblRNPackage.Location = new System.Drawing.Point(15, 172);
            this.lblRNPackage.Name = "lblRNPackage";
            this.lblRNPackage.Size = new System.Drawing.Size(90, 13);
            this.lblRNPackage.TabIndex = 12;
            this.lblRNPackage.Text = "RN Package Log";
            // 
            // progressBarRNPackage
            // 
            this.progressBarRNPackage.Location = new System.Drawing.Point(15, 189);
            this.progressBarRNPackage.Name = "progressBarRNPackage";
            this.progressBarRNPackage.Size = new System.Drawing.Size(435, 23);
            this.progressBarRNPackage.TabIndex = 11;
            // 
            // lblEBSLoader
            // 
            this.lblEBSLoader.AutoSize = true;
            this.lblEBSLoader.Location = new System.Drawing.Point(15, 217);
            this.lblEBSLoader.Name = "lblEBSLoader";
            this.lblEBSLoader.Size = new System.Drawing.Size(85, 13);
            this.lblEBSLoader.TabIndex = 14;
            this.lblEBSLoader.Text = "EBS Loader Log";
            // 
            // progressBarEBSLoader
            // 
            this.progressBarEBSLoader.Location = new System.Drawing.Point(15, 234);
            this.progressBarEBSLoader.Name = "progressBarEBSLoader";
            this.progressBarEBSLoader.Size = new System.Drawing.Size(435, 23);
            this.progressBarEBSLoader.TabIndex = 13;
            // 
            // lblDCCLoader
            // 
            this.lblDCCLoader.AutoSize = true;
            this.lblDCCLoader.Location = new System.Drawing.Point(15, 262);
            this.lblDCCLoader.Name = "lblDCCLoader";
            this.lblDCCLoader.Size = new System.Drawing.Size(86, 13);
            this.lblDCCLoader.TabIndex = 16;
            this.lblDCCLoader.Text = "DCC Loader Log";
            // 
            // progressBarDCCLoader
            // 
            this.progressBarDCCLoader.Location = new System.Drawing.Point(15, 281);
            this.progressBarDCCLoader.Name = "progressBarDCCLoader";
            this.progressBarDCCLoader.Size = new System.Drawing.Size(435, 23);
            this.progressBarDCCLoader.TabIndex = 15;
            // 
            // lblDCCManifest
            // 
            this.lblDCCManifest.AutoSize = true;
            this.lblDCCManifest.Location = new System.Drawing.Point(15, 307);
            this.lblDCCManifest.Name = "lblDCCManifest";
            this.lblDCCManifest.Size = new System.Drawing.Size(93, 13);
            this.lblDCCManifest.TabIndex = 18;
            this.lblDCCManifest.Text = "DCC Manifest Log";
            // 
            // progressBarDCCManifest
            // 
            this.progressBarDCCManifest.Location = new System.Drawing.Point(15, 326);
            this.progressBarDCCManifest.Name = "progressBarDCCManifest";
            this.progressBarDCCManifest.Size = new System.Drawing.Size(435, 23);
            this.progressBarDCCManifest.TabIndex = 17;
            // 
            // lblFTPStatus
            // 
            this.lblFTPStatus.AutoSize = true;
            this.lblFTPStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFTPStatus.Location = new System.Drawing.Point(456, 152);
            this.lblFTPStatus.Name = "lblFTPStatus";
            this.lblFTPStatus.Size = new System.Drawing.Size(16, 13);
            this.lblFTPStatus.TabIndex = 20;
            this.lblFTPStatus.Text = "- -";
            this.lblFTPStatus.Visible = false;
            // 
            // lblRNPackageStatus
            // 
            this.lblRNPackageStatus.AutoSize = true;
            this.lblRNPackageStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRNPackageStatus.Location = new System.Drawing.Point(456, 198);
            this.lblRNPackageStatus.Name = "lblRNPackageStatus";
            this.lblRNPackageStatus.Size = new System.Drawing.Size(16, 13);
            this.lblRNPackageStatus.TabIndex = 21;
            this.lblRNPackageStatus.Text = "- -";
            this.lblRNPackageStatus.Visible = false;
            // 
            // lblEBSLoaderStatus
            // 
            this.lblEBSLoaderStatus.AutoSize = true;
            this.lblEBSLoaderStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEBSLoaderStatus.Location = new System.Drawing.Point(456, 243);
            this.lblEBSLoaderStatus.Name = "lblEBSLoaderStatus";
            this.lblEBSLoaderStatus.Size = new System.Drawing.Size(16, 13);
            this.lblEBSLoaderStatus.TabIndex = 22;
            this.lblEBSLoaderStatus.Text = "- -";
            this.lblEBSLoaderStatus.Visible = false;
            // 
            // lblDCCLoaderStatus
            // 
            this.lblDCCLoaderStatus.AutoSize = true;
            this.lblDCCLoaderStatus.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblDCCLoaderStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDCCLoaderStatus.Location = new System.Drawing.Point(456, 290);
            this.lblDCCLoaderStatus.Name = "lblDCCLoaderStatus";
            this.lblDCCLoaderStatus.Size = new System.Drawing.Size(16, 13);
            this.lblDCCLoaderStatus.TabIndex = 23;
            this.lblDCCLoaderStatus.Text = "- -";
            this.lblDCCLoaderStatus.Visible = false;
            // 
            // lblDCCManifestStatus
            // 
            this.lblDCCManifestStatus.AutoSize = true;
            this.lblDCCManifestStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDCCManifestStatus.Location = new System.Drawing.Point(456, 335);
            this.lblDCCManifestStatus.Name = "lblDCCManifestStatus";
            this.lblDCCManifestStatus.Size = new System.Drawing.Size(16, 13);
            this.lblDCCManifestStatus.TabIndex = 24;
            this.lblDCCManifestStatus.Text = "- -";
            this.lblDCCManifestStatus.Visible = false;
            // 
            // lblFinalCompletion
            // 
            this.lblFinalCompletion.AutoSize = true;
            this.lblFinalCompletion.Location = new System.Drawing.Point(18, 395);
            this.lblFinalCompletion.Name = "lblFinalCompletion";
            this.lblFinalCompletion.Size = new System.Drawing.Size(16, 13);
            this.lblFinalCompletion.TabIndex = 25;
            this.lblFinalCompletion.Text = "- -";
            // 
            // lblFinalFails
            // 
            this.lblFinalFails.AutoSize = true;
            this.lblFinalFails.Location = new System.Drawing.Point(18, 426);
            this.lblFinalFails.Name = "lblFinalFails";
            this.lblFinalFails.Size = new System.Drawing.Size(16, 13);
            this.lblFinalFails.TabIndex = 26;
            this.lblFinalFails.Text = "- -";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 362);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 17);
            this.label2.TabIndex = 27;
            this.label2.Text = "Results:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 545);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblFinalFails);
            this.Controls.Add(this.lblFinalCompletion);
            this.Controls.Add(this.lblDCCManifestStatus);
            this.Controls.Add(this.lblDCCLoaderStatus);
            this.Controls.Add(this.lblEBSLoaderStatus);
            this.Controls.Add(this.lblRNPackageStatus);
            this.Controls.Add(this.lblFTPStatus);
            this.Controls.Add(this.lblDCCManifest);
            this.Controls.Add(this.progressBarDCCManifest);
            this.Controls.Add(this.lblDCCLoader);
            this.Controls.Add(this.progressBarDCCLoader);
            this.Controls.Add(this.lblEBSLoader);
            this.Controls.Add(this.progressBarEBSLoader);
            this.Controls.Add(this.lblRNPackage);
            this.Controls.Add(this.progressBarRNPackage);
            this.Controls.Add(this.lblFTP);
            this.Controls.Add(this.progressBarFTP);
            this.Controls.Add(this.lblTotalCompletion);
            this.Controls.Add(this.progressBarTotalCompletion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStart);
            this.Name = "Form1";
            this.Text = "DCC Log Importer Application";
            this.Closed += new System.EventHandler(this.Form1_Closed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBarTotalCompletion;
        private System.Windows.Forms.Label lblTotalCompletion;
        private System.Windows.Forms.Label lblFTP;
        private System.Windows.Forms.ProgressBar progressBarFTP;
        private System.Windows.Forms.Label lblRNPackage;
        private System.Windows.Forms.ProgressBar progressBarRNPackage;
        private System.Windows.Forms.Label lblEBSLoader;
        private System.Windows.Forms.ProgressBar progressBarEBSLoader;
        private System.Windows.Forms.Label lblDCCLoader;
        private System.Windows.Forms.ProgressBar progressBarDCCLoader;
        private System.Windows.Forms.Label lblDCCManifest;
        private System.Windows.Forms.ProgressBar progressBarDCCManifest;
        private System.Windows.Forms.Label lblFTPStatus;
        private System.Windows.Forms.Label lblRNPackageStatus;
        private System.Windows.Forms.Label lblEBSLoaderStatus;
        private System.Windows.Forms.Label lblDCCLoaderStatus;
        private System.Windows.Forms.Label lblDCCManifestStatus;
        private System.Windows.Forms.Label lblFinalCompletion;
        private System.Windows.Forms.Label lblFinalFails;
        private System.Windows.Forms.Label label2;
    //    private System.Windows.Forms.Label label10;
    }
}

