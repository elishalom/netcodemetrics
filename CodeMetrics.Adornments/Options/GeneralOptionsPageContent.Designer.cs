namespace CodeMetrics.Options
{
    partial class GeneralOptionsPageContent
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.thresholdTextbox = new System.Windows.Forms.TextBox();
            this.thresholdLabel = new System.Windows.Forms.Label();
            this.mainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.minimumColorLabel = new System.Windows.Forms.Label();
            this.rangeColorDialog = new System.Windows.Forms.ColorDialog();
            this.minimumColorPreview = new System.Windows.Forms.Panel();
            this.minimumColorButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // thresholdTextbox
            // 
            this.thresholdTextbox.Location = new System.Drawing.Point(164, 31);
            this.thresholdTextbox.Name = "thresholdTextbox";
            this.thresholdTextbox.Size = new System.Drawing.Size(48, 20);
            this.thresholdTextbox.TabIndex = 0;
            this.mainToolTip.SetToolTip(this.thresholdTextbox, "Define minum number of fomplexity to show.\r\nHas to be a number higher or equals t" +
        "han 1.");
            this.thresholdTextbox.TextChanged += new System.EventHandler(this.TextBoxTextChanged);
            // 
            // thresholdLabel
            // 
            this.thresholdLabel.AutoSize = true;
            this.thresholdLabel.Location = new System.Drawing.Point(44, 34);
            this.thresholdLabel.Name = "thresholdLabel";
            this.thresholdLabel.Size = new System.Drawing.Size(104, 13);
            this.thresholdLabel.TabIndex = 1;
            this.thresholdLabel.Text = "Minimum Complexity:";
            // 
            // minimumColorLabel
            // 
            this.minimumColorLabel.AutoSize = true;
            this.minimumColorLabel.Location = new System.Drawing.Point(18, 62);
            this.minimumColorLabel.Name = "minimumColorLabel";
            this.minimumColorLabel.Size = new System.Drawing.Size(130, 13);
            this.minimumColorLabel.TabIndex = 2;
            this.minimumColorLabel.Text = "Minimum complexity Color:";
            // 
            // minimumColorPreview
            // 
            this.minimumColorPreview.Location = new System.Drawing.Point(164, 57);
            this.minimumColorPreview.Name = "minimumColorPreview";
            this.minimumColorPreview.Size = new System.Drawing.Size(30, 23);
            this.minimumColorPreview.TabIndex = 3;
            // 
            // minimumColorButton
            // 
            this.minimumColorButton.Location = new System.Drawing.Point(200, 57);
            this.minimumColorButton.Name = "minimumColorButton";
            this.minimumColorButton.Size = new System.Drawing.Size(64, 23);
            this.minimumColorButton.TabIndex = 0;
            this.minimumColorButton.Text = "Edit";
            this.minimumColorButton.UseVisualStyleBackColor = true;
            this.minimumColorButton.Click += new System.EventHandler(this.MinimumColorButtonClick);
            // 
            // GeneralOptionsPageContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.minimumColorButton);
            this.Controls.Add(this.minimumColorPreview);
            this.Controls.Add(this.minimumColorLabel);
            this.Controls.Add(this.thresholdLabel);
            this.Controls.Add(this.thresholdTextbox);
            this.Name = "GeneralOptionsPageContent";
            this.Size = new System.Drawing.Size(688, 350);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox thresholdTextbox;
        private System.Windows.Forms.ToolTip mainToolTip;
        private System.Windows.Forms.Label thresholdLabel;
        private System.Windows.Forms.Label minimumColorLabel;
        private System.Windows.Forms.ColorDialog rangeColorDialog;
        private System.Windows.Forms.Panel minimumColorPreview;
        private System.Windows.Forms.Button minimumColorButton;
    }
}
