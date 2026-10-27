namespace racman
{
    partial class MemoryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MemoryForm));
            this.registerAddressTextBox = new System.Windows.Forms.TextBox();
            this.registerAddressTypeCombo = new System.Windows.Forms.ComboBox();
            this.addMemoryWatchButton = new System.Windows.Forms.Button();
            this.watchedMemoryAddressesListView = new System.Windows.Forms.ListView();
            this.nameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AddressColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ValueColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.selectedMobyComboBox = new System.Windows.Forms.ComboBox();
            this.refreshMobysButton = new System.Windows.Forms.Button();
            this.mobyInspectorListView = new System.Windows.Forms.ListView();
            this.mobyAddressHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mobyPropNameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mobyValueHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.saveWatchListButton = new System.Windows.Forms.Button();
            this.addressLabel = new System.Windows.Forms.Label();
            this.typeLabel = new System.Windows.Forms.Label();
            this.savedWatchlistsComboBox = new System.Windows.Forms.ComboBox();
            this.watchlistsLabel = new System.Windows.Forms.Label();
            this.memoryWatchLabel = new System.Windows.Forms.Label();
            this.mobyInspectorLabel = new System.Windows.Forms.Label();
            this.dumpButton = new System.Windows.Forms.Button();
            this.sortByOClassCheckBox = new System.Windows.Forms.CheckBox();
            this.toggleMobyInspectorButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // registerAddressTextBox
            // 
            this.registerAddressTextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.registerAddressTextBox.Location = new System.Drawing.Point(12, 496);
            this.registerAddressTextBox.MaximumSize = new System.Drawing.Size(106, 20);
            this.registerAddressTextBox.MinimumSize = new System.Drawing.Size(106, 20);
            this.registerAddressTextBox.Name = "registerAddressTextBox";
            this.registerAddressTextBox.Size = new System.Drawing.Size(106, 20);
            this.registerAddressTextBox.TabIndex = 0;
            // 
            // registerAddressTypeCombo
            // 
            this.registerAddressTypeCombo.FormattingEnabled = true;
            this.registerAddressTypeCombo.Items.AddRange(new object[] {
            "Int32",
            "Int64",
            "Int16",
            "Byte",
            "Float",
            "Pointer",
            "Instruction"});
            this.registerAddressTypeCombo.Location = new System.Drawing.Point(122, 496);
            this.registerAddressTypeCombo.MaximumSize = new System.Drawing.Size(121, 0);
            this.registerAddressTypeCombo.MinimumSize = new System.Drawing.Size(121, 0);
            this.registerAddressTypeCombo.Name = "registerAddressTypeCombo";
            this.registerAddressTypeCombo.Size = new System.Drawing.Size(121, 21);
            this.registerAddressTypeCombo.TabIndex = 1;
            this.registerAddressTypeCombo.Text = "Int32";
            // 
            // addMemoryWatchButton
            // 
            this.addMemoryWatchButton.Location = new System.Drawing.Point(247, 495);
            this.addMemoryWatchButton.MaximumSize = new System.Drawing.Size(75, 23);
            this.addMemoryWatchButton.MinimumSize = new System.Drawing.Size(75, 23);
            this.addMemoryWatchButton.Name = "addMemoryWatchButton";
            this.addMemoryWatchButton.Size = new System.Drawing.Size(75, 23);
            this.addMemoryWatchButton.TabIndex = 2;
            this.addMemoryWatchButton.Text = "Add";
            this.addMemoryWatchButton.UseVisualStyleBackColor = true;
            this.addMemoryWatchButton.Click += new System.EventHandler(this.addMemoryWatchButton_Click);
            // 
            // watchedMemoryAddressesListView
            // 
            this.watchedMemoryAddressesListView.AllowColumnReorder = true;
            this.watchedMemoryAddressesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumnHeader,
            this.AddressColumnHeader,
            this.ValueColumnHeader});
            this.watchedMemoryAddressesListView.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.watchedMemoryAddressesListView.GridLines = true;
            this.watchedMemoryAddressesListView.HideSelection = false;
            this.watchedMemoryAddressesListView.LabelEdit = true;
            this.watchedMemoryAddressesListView.Location = new System.Drawing.Point(12, 26);
            this.watchedMemoryAddressesListView.MaximumSize = new System.Drawing.Size(404, 448);
            this.watchedMemoryAddressesListView.MinimumSize = new System.Drawing.Size(308, 448);
            this.watchedMemoryAddressesListView.Name = "watchedMemoryAddressesListView";
            this.watchedMemoryAddressesListView.Size = new System.Drawing.Size(404, 448);
            this.watchedMemoryAddressesListView.TabIndex = 3;
            this.watchedMemoryAddressesListView.UseCompatibleStateImageBehavior = false;
            this.watchedMemoryAddressesListView.View = System.Windows.Forms.View.Details;
            this.watchedMemoryAddressesListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.watchedMemoryAddressesListView_MouseClick);
            // 
            // nameColumnHeader
            // 
            this.nameColumnHeader.Text = "Name";
            this.nameColumnHeader.Width = 195;
            // 
            // AddressColumnHeader
            // 
            this.AddressColumnHeader.Text = "Address";
            this.AddressColumnHeader.Width = 97;
            // 
            // ValueColumnHeader
            // 
            this.ValueColumnHeader.Text = "Value";
            this.ValueColumnHeader.Width = 111;
            // 
            // selectedMobyComboBox
            // 
            this.selectedMobyComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedMobyComboBox.FormattingEnabled = true;
            this.selectedMobyComboBox.Location = new System.Drawing.Point(433, 26);
            this.selectedMobyComboBox.Name = "selectedMobyComboBox";
            this.selectedMobyComboBox.Size = new System.Drawing.Size(310, 21);
            this.selectedMobyComboBox.TabIndex = 4;
            this.selectedMobyComboBox.SelectedIndexChanged += new System.EventHandler(this.selectedMobyComboBox_SelectedIndexChanged);
            // 
            // refreshMobysButton
            // 
            this.refreshMobysButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshMobysButton.Location = new System.Drawing.Point(749, 25);
            this.refreshMobysButton.Name = "refreshMobysButton";
            this.refreshMobysButton.Size = new System.Drawing.Size(160, 23);
            this.refreshMobysButton.TabIndex = 5;
            this.refreshMobysButton.Text = "Refresh mobys (pauses game)";
            this.refreshMobysButton.UseVisualStyleBackColor = true;
            this.refreshMobysButton.Click += new System.EventHandler(this.refreshMobysButton_Click);
            // 
            // mobyInspectorListView
            // 
            this.mobyInspectorListView.AllowColumnReorder = true;
            this.mobyInspectorListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mobyInspectorListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.mobyAddressHeader,
            this.mobyPropNameHeader,
            this.mobyValueHeader});
            this.mobyInspectorListView.GridLines = true;
            this.mobyInspectorListView.HideSelection = false;
            this.mobyInspectorListView.Location = new System.Drawing.Point(433, 53);
            this.mobyInspectorListView.Name = "mobyInspectorListView";
            this.mobyInspectorListView.Size = new System.Drawing.Size(541, 503);
            this.mobyInspectorListView.TabIndex = 6;
            this.mobyInspectorListView.UseCompatibleStateImageBehavior = false;
            this.mobyInspectorListView.View = System.Windows.Forms.View.Details;
            // 
            // mobyAddressHeader
            // 
            this.mobyAddressHeader.Text = "Address";
            this.mobyAddressHeader.Width = 96;
            // 
            // mobyPropNameHeader
            // 
            this.mobyPropNameHeader.Text = "Property";
            this.mobyPropNameHeader.Width = 149;
            // 
            // mobyValueHeader
            // 
            this.mobyValueHeader.Text = "Value";
            this.mobyValueHeader.Width = 217;
            // 
            // saveWatchListButton
            // 
            this.saveWatchListButton.Location = new System.Drawing.Point(124, 535);
            this.saveWatchListButton.Name = "saveWatchListButton";
            this.saveWatchListButton.Size = new System.Drawing.Size(90, 21);
            this.saveWatchListButton.TabIndex = 7;
            this.saveWatchListButton.Text = "Save";
            this.saveWatchListButton.UseVisualStyleBackColor = true;
            this.saveWatchListButton.Click += new System.EventHandler(this.saveWatchListButton_Click);
            // 
            // addressLabel
            // 
            this.addressLabel.AutoSize = true;
            this.addressLabel.Location = new System.Drawing.Point(9, 480);
            this.addressLabel.Name = "addressLabel";
            this.addressLabel.Size = new System.Drawing.Size(48, 13);
            this.addressLabel.TabIndex = 8;
            this.addressLabel.Text = "Address:";
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.Location = new System.Drawing.Point(119, 480);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(34, 13);
            this.typeLabel.TabIndex = 9;
            this.typeLabel.Text = "Type:";
            // 
            // savedWatchlistsComboBox
            // 
            this.savedWatchlistsComboBox.FormattingEnabled = true;
            this.savedWatchlistsComboBox.Location = new System.Drawing.Point(12, 535);
            this.savedWatchlistsComboBox.Name = "savedWatchlistsComboBox";
            this.savedWatchlistsComboBox.Size = new System.Drawing.Size(106, 21);
            this.savedWatchlistsComboBox.TabIndex = 10;
            this.savedWatchlistsComboBox.SelectedIndexChanged += new System.EventHandler(this.savedWatchlistsComboBox_SelectedIndexChanged);
            // 
            // watchlistsLabel
            // 
            this.watchlistsLabel.AutoSize = true;
            this.watchlistsLabel.Location = new System.Drawing.Point(9, 519);
            this.watchlistsLabel.Name = "watchlistsLabel";
            this.watchlistsLabel.Size = new System.Drawing.Size(59, 13);
            this.watchlistsLabel.TabIndex = 11;
            this.watchlistsLabel.Text = "Watchlists:";
            // 
            // memoryWatchLabel
            // 
            this.memoryWatchLabel.AutoSize = true;
            this.memoryWatchLabel.Location = new System.Drawing.Point(12, 9);
            this.memoryWatchLabel.Name = "memoryWatchLabel";
            this.memoryWatchLabel.Size = new System.Drawing.Size(82, 13);
            this.memoryWatchLabel.TabIndex = 12;
            this.memoryWatchLabel.Text = "Memory Watch:";
            // 
            // mobyInspectorLabel
            // 
            this.mobyInspectorLabel.AutoSize = true;
            this.mobyInspectorLabel.Location = new System.Drawing.Point(430, 9);
            this.mobyInspectorLabel.Name = "mobyInspectorLabel";
            this.mobyInspectorLabel.Size = new System.Drawing.Size(83, 13);
            this.mobyInspectorLabel.TabIndex = 13;
            this.mobyInspectorLabel.Text = "Moby Inspector:";
            // 
            // dumpButton
            // 
            this.dumpButton.Location = new System.Drawing.Point(915, 25);
            this.dumpButton.Name = "dumpButton";
            this.dumpButton.Size = new System.Drawing.Size(59, 23);
            this.dumpButton.TabIndex = 14;
            this.dumpButton.Text = "Dump";
            this.dumpButton.UseVisualStyleBackColor = true;
            this.dumpButton.Click += new System.EventHandler(this.dumpButton_Click);
            // 
            // sortByOClassCheckBox
            // 
            this.sortByOClassCheckBox.AutoSize = true;
            this.sortByOClassCheckBox.Location = new System.Drawing.Point(519, 8);
            this.sortByOClassCheckBox.Name = "sortByOClassCheckBox";
            this.sortByOClassCheckBox.Size = new System.Drawing.Size(93, 17);
            this.sortByOClassCheckBox.TabIndex = 15;
            this.sortByOClassCheckBox.Text = "Sort by oClass";
            this.sortByOClassCheckBox.UseVisualStyleBackColor = true;
            this.sortByOClassCheckBox.CheckedChanged += new System.EventHandler(this.sortByOClassCheckBox_CheckedChanged);
            // 
            // toggleMobyInspectorButton
            // 
            this.toggleMobyInspectorButton.Location = new System.Drawing.Point(296, 4);
            this.toggleMobyInspectorButton.Name = "toggleMobyInspectorButton";
            this.toggleMobyInspectorButton.Size = new System.Drawing.Size(120, 21);
            this.toggleMobyInspectorButton.TabIndex = 16;
            this.toggleMobyInspectorButton.Text = "Hide moby inspector";
            this.toggleMobyInspectorButton.UseVisualStyleBackColor = true;
            this.toggleMobyInspectorButton.Click += new System.EventHandler(this.toggleMobyInspectorButton_Click);
            // 
            // MemoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 566);
            this.Controls.Add(this.toggleMobyInspectorButton);
            this.Controls.Add(this.sortByOClassCheckBox);
            this.Controls.Add(this.dumpButton);
            this.Controls.Add(this.mobyInspectorLabel);
            this.Controls.Add(this.memoryWatchLabel);
            this.Controls.Add(this.watchlistsLabel);
            this.Controls.Add(this.savedWatchlistsComboBox);
            this.Controls.Add(this.typeLabel);
            this.Controls.Add(this.addressLabel);
            this.Controls.Add(this.saveWatchListButton);
            this.Controls.Add(this.mobyInspectorListView);
            this.Controls.Add(this.refreshMobysButton);
            this.Controls.Add(this.selectedMobyComboBox);
            this.Controls.Add(this.watchedMemoryAddressesListView);
            this.Controls.Add(this.addMemoryWatchButton);
            this.Controls.Add(this.registerAddressTypeCombo);
            this.Controls.Add(this.registerAddressTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MemoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Memory utilities";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MemoryForm_FormClosing);
            this.Load += new System.EventHandler(this.MemoryForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox registerAddressTextBox;
        private System.Windows.Forms.ComboBox registerAddressTypeCombo;
        private System.Windows.Forms.Button addMemoryWatchButton;
        private System.Windows.Forms.ListView watchedMemoryAddressesListView;
        private System.Windows.Forms.ColumnHeader AddressColumnHeader;
        private System.Windows.Forms.ColumnHeader ValueColumnHeader;
        private System.Windows.Forms.ColumnHeader nameColumnHeader;
        private System.Windows.Forms.ComboBox selectedMobyComboBox;
        private System.Windows.Forms.Button refreshMobysButton;
        private System.Windows.Forms.ListView mobyInspectorListView;
        private System.Windows.Forms.ColumnHeader mobyAddressHeader;
        private System.Windows.Forms.ColumnHeader mobyPropNameHeader;
        private System.Windows.Forms.ColumnHeader mobyValueHeader;
        private System.Windows.Forms.Button saveWatchListButton;
        private System.Windows.Forms.Label addressLabel;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.ComboBox savedWatchlistsComboBox;
        private System.Windows.Forms.Label watchlistsLabel;
        private System.Windows.Forms.Label memoryWatchLabel;
        private System.Windows.Forms.Label mobyInspectorLabel;
        private System.Windows.Forms.Button dumpButton;
        private System.Windows.Forms.CheckBox sortByOClassCheckBox;
        private System.Windows.Forms.Button toggleMobyInspectorButton;
    }
}