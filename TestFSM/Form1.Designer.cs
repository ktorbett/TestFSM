namespace TestFSM
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
        /// <param refClassName="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.loadSTTsButton = new System.Windows.Forms.Button();
            this.eventListComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listOfSTTsListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listOfInstancesListBox = new System.Windows.Forms.ListBox();
            this.createOMInstancesButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.selectedInstanceStateTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.sendEventButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.writeCodeToFileButton = new System.Windows.Forms.Button();
            this.ignoreExistingCodeCheckBox = new System.Windows.Forms.CheckBox();
            this.changeOnEntryCodeButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // loadSTTsButton
            // 
            this.loadSTTsButton.Location = new System.Drawing.Point(28, 63);
            this.loadSTTsButton.Name = "loadSTTsButton";
            this.loadSTTsButton.Size = new System.Drawing.Size(112, 34);
            this.loadSTTsButton.TabIndex = 0;
            this.loadSTTsButton.Text = "Load STTs";
            this.loadSTTsButton.UseVisualStyleBackColor = true;
            this.loadSTTsButton.Click += new System.EventHandler(this.loadSTTsButton_Click);
            // 
            // eventListComboBox
            // 
            this.eventListComboBox.FormattingEnabled = true;
            this.eventListComboBox.Location = new System.Drawing.Point(480, 82);
            this.eventListComboBox.Name = "eventListComboBox";
            this.eventListComboBox.Size = new System.Drawing.Size(205, 33);
            this.eventListComboBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(480, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select an Event";
            // 
            // listOfSTTsListBox
            // 
            this.listOfSTTsListBox.FormattingEnabled = true;
            this.listOfSTTsListBox.ItemHeight = 25;
            this.listOfSTTsListBox.Location = new System.Drawing.Point(28, 77);
            this.listOfSTTsListBox.Name = "listOfSTTsListBox";
            this.listOfSTTsListBox.Size = new System.Drawing.Size(179, 204);
            this.listOfSTTsListBox.TabIndex = 3;
            this.listOfSTTsListBox.SelectedIndexChanged += new System.EventHandler(this.listOfSTTsListBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(219, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(164, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "List of STTs loaded:";
            // 
            // listOfInstancesListBox
            // 
            this.listOfInstancesListBox.FormattingEnabled = true;
            this.listOfInstancesListBox.ItemHeight = 25;
            this.listOfInstancesListBox.Location = new System.Drawing.Point(233, 77);
            this.listOfInstancesListBox.Name = "listOfInstancesListBox";
            this.listOfInstancesListBox.Size = new System.Drawing.Size(216, 204);
            this.listOfInstancesListBox.TabIndex = 5;
            this.listOfInstancesListBox.SelectedIndexChanged += new System.EventHandler(this.listOfInstancesListBox_SelectedIndexChanged);
            // 
            // createOMInstancesButton
            // 
            this.createOMInstancesButton.Location = new System.Drawing.Point(21, 323);
            this.createOMInstancesButton.Name = "createOMInstancesButton";
            this.createOMInstancesButton.Size = new System.Drawing.Size(197, 34);
            this.createOMInstancesButton.TabIndex = 6;
            this.createOMInstancesButton.Text = "Create Instances";
            this.createOMInstancesButton.UseVisualStyleBackColor = true;
            this.createOMInstancesButton.Click += new System.EventHandler(this.createOMInstancesButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.selectedInstanceStateTextBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.listOfSTTsListBox);
            this.groupBox1.Controls.Add(this.sendEventButton);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.listOfInstancesListBox);
            this.groupBox1.Controls.Add(this.eventListComboBox);
            this.groupBox1.Location = new System.Drawing.Point(21, 377);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(929, 307);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Send an Event";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 25);
            this.label6.TabIndex = 4;
            this.label6.Text = "Pick a Class:";
            // 
            // selectedInstanceStateTextBox
            // 
            this.selectedInstanceStateTextBox.Location = new System.Drawing.Point(735, 183);
            this.selectedInstanceStateTextBox.Name = "selectedInstanceStateTextBox";
            this.selectedInstanceStateTextBox.Size = new System.Drawing.Size(150, 31);
            this.selectedInstanceStateTextBox.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(693, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(192, 25);
            this.label4.TabIndex = 8;
            this.label4.Text = "Selected Instance State";
            // 
            // sendEventButton
            // 
            this.sendEventButton.Location = new System.Drawing.Point(489, 143);
            this.sendEventButton.Name = "sendEventButton";
            this.sendEventButton.Size = new System.Drawing.Size(121, 41);
            this.sendEventButton.TabIndex = 7;
            this.sendEventButton.Text = "Send Event";
            this.sendEventButton.UseVisualStyleBackColor = true;
            this.sendEventButton.Click += new System.EventHandler(this.sendEventButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(244, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(162, 25);
            this.label3.TabIndex = 6;
            this.label3.Text = "Pick aTarget Object";
            // 
            // writeCodeToFileButton
            // 
            this.writeCodeToFileButton.Location = new System.Drawing.Point(272, 263);
            this.writeCodeToFileButton.Name = "writeCodeToFileButton";
            this.writeCodeToFileButton.Size = new System.Drawing.Size(197, 34);
            this.writeCodeToFileButton.TabIndex = 8;
            this.writeCodeToFileButton.Text = "Write Code to File";
            this.writeCodeToFileButton.UseVisualStyleBackColor = true;
            this.writeCodeToFileButton.Click += new System.EventHandler(this.writeCodeToFileButton_Click);
            // 
            // ignoreExistingCodeCheckBox
            // 
            this.ignoreExistingCodeCheckBox.AutoSize = true;
            this.ignoreExistingCodeCheckBox.Location = new System.Drawing.Point(205, 113);
            this.ignoreExistingCodeCheckBox.Name = "ignoreExistingCodeCheckBox";
            this.ignoreExistingCodeCheckBox.Size = new System.Drawing.Size(267, 29);
            this.ignoreExistingCodeCheckBox.TabIndex = 9;
            this.ignoreExistingCodeCheckBox.Text = "Only Generate Missing Parts?";
            this.ignoreExistingCodeCheckBox.UseVisualStyleBackColor = true;
            this.ignoreExistingCodeCheckBox.CheckedChanged += new System.EventHandler(this.ignoreExistingCodeCheckBox_CheckedChanged);
            // 
            // changeOnEntryCodeButton
            // 
            this.changeOnEntryCodeButton.Location = new System.Drawing.Point(205, 63);
            this.changeOnEntryCodeButton.Name = "changeOnEntryCodeButton";
            this.changeOnEntryCodeButton.Size = new System.Drawing.Size(197, 34);
            this.changeOnEntryCodeButton.TabIndex = 10;
            this.changeOnEntryCodeButton.Text = "Change onEntry Code";
            this.changeOnEntryCodeButton.UseVisualStyleBackColor = true;
            this.changeOnEntryCodeButton.Click += new System.EventHandler(this.changeOnEntryCodeButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.loadSTTsButton);
            this.groupBox2.Controls.Add(this.listBox1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(21, 23);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(420, 277);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "State Transition Tables";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 25;
            this.listBox1.Location = new System.Drawing.Point(219, 63);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listBox1.Size = new System.Drawing.Size(180, 129);
            this.listBox1.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(471, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(205, 25);
            this.label5.TabIndex = 4;
            this.label5.Text = "Pick Classes to generate:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.listBox2);
            this.groupBox3.Controls.Add(this.writeCodeToFileButton);
            this.groupBox3.Controls.Add(this.changeOnEntryCodeButton);
            this.groupBox3.Controls.Add(this.ignoreExistingCodeCheckBox);
            this.groupBox3.Location = new System.Drawing.Point(462, 23);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(488, 318);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Code Generation";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(19, 213);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(430, 31);
            this.textBox1.TabIndex = 12;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(205, 158);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(225, 34);
            this.button1.TabIndex = 11;
            this.button1.Text = "Pick Code Gen Directory";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 25;
            this.listBox2.Location = new System.Drawing.Point(19, 63);
            this.listBox2.Name = "listBox2";
            this.listBox2.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox2.Size = new System.Drawing.Size(166, 129);
            this.listBox2.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1031, 703);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.createOMInstancesButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Name = "Form1";
            this.Text = "List of STTs loaded:";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loadSTTsButton;
        private System.Windows.Forms.ComboBox eventListComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listOfSTTsListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listOfInstancesListBox;
        private System.Windows.Forms.Button createOMInstancesButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button sendEventButton;
        private System.Windows.Forms.TextBox selectedInstanceStateTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button writeCodeToFileButton;
        private System.Windows.Forms.CheckBox ignoreExistingCodeCheckBox;
        private System.Windows.Forms.Button changeOnEntryCodeButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
    }
}

