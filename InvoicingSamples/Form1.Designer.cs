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
            this.PaymentButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CfdiIngresoButton
            // 
            this.CfdiIngresoButton.Location = new System.Drawing.Point(53, 97);
            this.CfdiIngresoButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.CfdiIngresoButton.Name = "CfdiIngresoButton";
            this.CfdiIngresoButton.Size = new System.Drawing.Size(149, 79);
            this.CfdiIngresoButton.TabIndex = 0;
            this.CfdiIngresoButton.Text = "CFDI INGRESO";
            this.CfdiIngresoButton.UseVisualStyleBackColor = true;
            this.CfdiIngresoButton.Click += new System.EventHandler(this.CfdiIngresoButton_Click);
            // 
            // PaymentButton
            // 
            this.PaymentButton.Location = new System.Drawing.Point(253, 97);
            this.PaymentButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.PaymentButton.Name = "PaymentButton";
            this.PaymentButton.Size = new System.Drawing.Size(149, 79);
            this.PaymentButton.TabIndex = 1;
            this.PaymentButton.Text = "CFDI PAGO";
            this.PaymentButton.UseVisualStyleBackColor = true;
            this.PaymentButton.Click += new System.EventHandler(this.PaymentButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 360);
            this.Controls.Add(this.PaymentButton);
            this.Controls.Add(this.CfdiIngresoButton);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Button CfdiIngresoButton;
        private Button PaymentButton;
    }
}