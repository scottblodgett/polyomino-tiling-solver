namespace CheckerBoard
{
    partial class CBoard
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
            this.clearButton = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.lblDebug = new System.Windows.Forms.Label();
            this.btnPlaceOne = new System.Windows.Forms.Button();
            this.lblPos = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.ddlItems = new System.Windows.Forms.ComboBox();
            this.ddlPos = new System.Windows.Forms.ComboBox();
            this.lblPiece = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.btnShowPieces = new System.Windows.Forms.Button();
            this.btnAnother = new System.Windows.Forms.Button();
            this.txtBW = new System.Windows.Forms.TextBox();
            this.txtBoard = new System.Windows.Forms.TextBox();
            this.btnSuper = new System.Windows.Forms.Button();
            this.btnAll = new System.Windows.Forms.Button();
            this.lblFinal = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.lblNum = new System.Windows.Forms.Label();
            this.groupBox5.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.SuspendLayout();
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(87, 39);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 0;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(47, 19);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 2;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // lblDebug
            // 
            this.lblDebug.AutoSize = true;
            this.lblDebug.Location = new System.Drawing.Point(27, 375);
            this.lblDebug.Name = "lblDebug";
            this.lblDebug.Size = new System.Drawing.Size(0, 13);
            this.lblDebug.TabIndex = 10;
            // 
            // btnPlaceOne
            // 
            this.btnPlaceOne.Location = new System.Drawing.Point(6, 39);
            this.btnPlaceOne.Name = "btnPlaceOne";
            this.btnPlaceOne.Size = new System.Drawing.Size(75, 23);
            this.btnPlaceOne.TabIndex = 12;
            this.btnPlaceOne.Text = "Place Many";
            this.btnPlaceOne.UseVisualStyleBackColor = true;
            this.btnPlaceOne.Click += new System.EventHandler(this.btnPlaceOne_Click);
            // 
            // lblPos
            // 
            this.lblPos.AutoSize = true;
            this.lblPos.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPos.Location = new System.Drawing.Point(7, 14);
            this.lblPos.Name = "lblPos";
            this.lblPos.Size = new System.Drawing.Size(29, 16);
            this.lblPos.TabIndex = 15;
            this.lblPos.Text = "Pos.";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lblNum);
            this.groupBox5.Controls.Add(this.btnPrev);
            this.groupBox5.Controls.Add(this.btnNext);
            this.groupBox5.Controls.Add(this.btnGo);
            this.groupBox5.Location = new System.Drawing.Point(390, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(169, 81);
            this.groupBox5.TabIndex = 19;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Control";
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(5, 19);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(39, 23);
            this.btnPrev.TabIndex = 3;
            this.btnPrev.Text = "<<";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(124, 19);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(39, 23);
            this.btnNext.TabIndex = 26;
            this.btnNext.Text = ">>";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // ddlItems
            // 
            this.ddlItems.FormattingEnabled = true;
            this.ddlItems.Location = new System.Drawing.Point(124, 10);
            this.ddlItems.Name = "ddlItems";
            this.ddlItems.Size = new System.Drawing.Size(38, 21);
            this.ddlItems.TabIndex = 25;
            // 
            // ddlPos
            // 
            this.ddlPos.FormattingEnabled = true;
            this.ddlPos.Location = new System.Drawing.Point(47, 12);
            this.ddlPos.Name = "ddlPos";
            this.ddlPos.Size = new System.Drawing.Size(38, 21);
            this.ddlPos.TabIndex = 27;
            // 
            // lblPiece
            // 
            this.lblPiece.AutoSize = true;
            this.lblPiece.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPiece.Location = new System.Drawing.Point(88, 12);
            this.lblPiece.Name = "lblPiece";
            this.lblPiece.Size = new System.Drawing.Size(34, 16);
            this.lblPiece.TabIndex = 17;
            this.lblPiece.Text = "Piece";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.ddlItems);
            this.groupBox9.Controls.Add(this.lblPiece);
            this.groupBox9.Controls.Add(this.btnShowPieces);
            this.groupBox9.Controls.Add(this.ddlPos);
            this.groupBox9.Controls.Add(this.btnAnother);
            this.groupBox9.Controls.Add(this.lblPos);
            this.groupBox9.Controls.Add(this.txtBW);
            this.groupBox9.Controls.Add(this.txtBoard);
            this.groupBox9.Controls.Add(this.btnSuper);
            this.groupBox9.Controls.Add(this.btnAll);
            this.groupBox9.Controls.Add(this.btnPlaceOne);
            this.groupBox9.Controls.Add(this.clearButton);
            this.groupBox9.Location = new System.Drawing.Point(387, 118);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(171, 195);
            this.groupBox9.TabIndex = 23;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Debug";
            this.groupBox9.Visible = false;
            // 
            // btnShowPieces
            // 
            this.btnShowPieces.Location = new System.Drawing.Point(87, 150);
            this.btnShowPieces.Name = "btnShowPieces";
            this.btnShowPieces.Size = new System.Drawing.Size(75, 23);
            this.btnShowPieces.TabIndex = 18;
            this.btnShowPieces.Text = "Pieces File";
            this.btnShowPieces.UseVisualStyleBackColor = true;
            this.btnShowPieces.Click += new System.EventHandler(this.btnShowPieces_Click);
            // 
            // btnAnother
            // 
            this.btnAnother.Location = new System.Drawing.Point(6, 150);
            this.btnAnother.Name = "btnAnother";
            this.btnAnother.Size = new System.Drawing.Size(75, 23);
            this.btnAnother.TabIndex = 17;
            this.btnAnother.Text = "Another One";
            this.btnAnother.UseVisualStyleBackColor = true;
            this.btnAnother.Click += new System.EventHandler(this.btnAnother_Click);
            // 
            // txtBW
            // 
            this.txtBW.Location = new System.Drawing.Point(6, 124);
            this.txtBW.Name = "txtBW";
            this.txtBW.Size = new System.Drawing.Size(156, 20);
            this.txtBW.TabIndex = 16;
            // 
            // txtBoard
            // 
            this.txtBoard.Location = new System.Drawing.Point(6, 98);
            this.txtBoard.Name = "txtBoard";
            this.txtBoard.Size = new System.Drawing.Size(156, 20);
            this.txtBoard.TabIndex = 15;
            // 
            // btnSuper
            // 
            this.btnSuper.Location = new System.Drawing.Point(87, 68);
            this.btnSuper.Name = "btnSuper";
            this.btnSuper.Size = new System.Drawing.Size(75, 23);
            this.btnSuper.TabIndex = 14;
            this.btnSuper.Text = "Super";
            this.btnSuper.UseVisualStyleBackColor = true;
            this.btnSuper.Click += new System.EventHandler(this.btnSuper_Click);
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(6, 68);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(75, 23);
            this.btnAll.TabIndex = 13;
            this.btnAll.Text = "Place All";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // lblFinal
            // 
            this.lblFinal.AutoSize = true;
            this.lblFinal.Location = new System.Drawing.Point(27, 398);
            this.lblFinal.Name = "lblFinal";
            this.lblFinal.Size = new System.Drawing.Size(0, 13);
            this.lblFinal.TabIndex = 24;
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(27, 407);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 13);
            this.lblError.TabIndex = 25;
            // 
            // lblNum
            // 
            this.lblNum.AutoSize = true;
            this.lblNum.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNum.Location = new System.Drawing.Point(73, 52);
            this.lblNum.Name = "lblNum";
            this.lblNum.Size = new System.Drawing.Size(0, 16);
            this.lblNum.TabIndex = 26;
            // 
            // CBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 434);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblFinal);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.lblDebug);
            this.Name = "CBoard";
            this.Text = "Pentominoes";
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Label lblDebug;
        private System.Windows.Forms.Button btnPlaceOne;
        private System.Windows.Forms.Label lblPos;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lblPiece;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label lblFinal;
        private System.Windows.Forms.ComboBox ddlItems;
        private System.Windows.Forms.ComboBox ddlPos;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.TextBox txtBoard;
        private System.Windows.Forms.Button btnSuper;
        private System.Windows.Forms.TextBox txtBW;
        private System.Windows.Forms.Button btnAnother;
        private System.Windows.Forms.Button btnShowPieces;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblNum;
    }
}

