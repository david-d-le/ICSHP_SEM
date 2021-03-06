
namespace ICSHP_SEM_Le
{
    partial class MainMenuForm
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
            this.endBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.newGameBtn = new System.Windows.Forms.Button();
            this.loadGameBtn = new System.Windows.Forms.Button();
            this.mainMenuLabel1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // endBtn
            // 
            this.endBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.endBtn.Location = new System.Drawing.Point(6, 133);
            this.endBtn.Name = "endBtn";
            this.endBtn.Size = new System.Drawing.Size(200, 50);
            this.endBtn.TabIndex = 1;
            this.endBtn.Text = "End Game";
            this.endBtn.UseVisualStyleBackColor = true;
            this.endBtn.Click += new System.EventHandler(this.endBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.newGameBtn);
            this.groupBox1.Controls.Add(this.loadGameBtn);
            this.groupBox1.Controls.Add(this.endBtn);
            this.groupBox1.Location = new System.Drawing.Point(558, 202);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(212, 189);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // newGameBtn
            // 
            this.newGameBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.newGameBtn.Location = new System.Drawing.Point(6, 21);
            this.newGameBtn.Name = "newGameBtn";
            this.newGameBtn.Size = new System.Drawing.Size(200, 50);
            this.newGameBtn.TabIndex = 3;
            this.newGameBtn.Text = "New Game";
            this.newGameBtn.UseVisualStyleBackColor = true;
            this.newGameBtn.Click += new System.EventHandler(this.newGameBtn_Click);
            // 
            // loadGameBtn
            // 
            this.loadGameBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.loadGameBtn.Location = new System.Drawing.Point(6, 77);
            this.loadGameBtn.Name = "loadGameBtn";
            this.loadGameBtn.Size = new System.Drawing.Size(200, 50);
            this.loadGameBtn.TabIndex = 2;
            this.loadGameBtn.Text = "Load saved game";
            this.loadGameBtn.UseVisualStyleBackColor = true;
            this.loadGameBtn.Click += new System.EventHandler(this.loadGameBtn_Click);
            // 
            // mainMenuLabel1
            // 
            this.mainMenuLabel1.AutoSize = true;
            this.mainMenuLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.mainMenuLabel1.Location = new System.Drawing.Point(30, 30);
            this.mainMenuLabel1.Name = "mainMenuLabel1";
            this.mainMenuLabel1.Size = new System.Drawing.Size(356, 69);
            this.mainMenuLabel1.TabIndex = 3;
            this.mainMenuLabel1.Text = "Tic Tac Toe";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(12, 360);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Author: David Le";
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 403);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mainMenuLabel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainMenuForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tick Tack Toe - David Le";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainMenuForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button endBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button loadGameBtn;
        private System.Windows.Forms.Label mainMenuLabel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button newGameBtn;
    }
}