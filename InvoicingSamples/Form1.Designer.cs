namespace InvoicingSamples
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CfdiIngresoButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CfdiIngresoButton
            // 
            this.CfdiIngresoButton.Location = new System.Drawing.Point(313, 158);
            this.CfdiIngresoButton.Name = "CfdiIngresoButton";
            this.CfdiIngresoButton.Size = new System.Drawing.Size(171, 50);
            this.CfdiIngresoButton.TabIndex = 0;
            this.CfdiIngresoButton.Text = "Cfdi Ingreso";
            this.CfdiIngresoButton.UseVisualStyleBackColor = true;
            this.CfdiIngresoButton.Click += new System.EventHandler(this.CfdiIngresoButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CfdiIngresoButton);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Button CfdiIngresoButton;
    }
}