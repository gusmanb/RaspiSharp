namespace RaspiImporter
{
	partial class MainDesigner
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDesigner));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.designer1 = new Dalssoft.DiagramNet.Designer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.tvElementTypes = new System.Windows.Forms.TreeView();
			this.tvImagelit = new System.Windows.Forms.ImageList(this.components);
			this.pgSelectedElement = new System.Windows.Forms.PropertyGrid();
			this.toolStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton4,
            this.toolStripButton3,
            this.toolStripSeparator1,
            this.toolStripButton1,
            this.toolStripButton2});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(955, 25);
			this.toolStrip1.TabIndex = 10;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButton4
			// 
			this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton4.Image = global::RaspiImporter.Properties.Resources.folder_go;
			this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton4.Name = "toolStripButton4";
			this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton4.Text = "toolStripButton4";
			this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
			// 
			// toolStripButton3
			// 
			this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton3.Image = global::RaspiImporter.Properties.Resources.disk;
			this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton3.Name = "toolStripButton3";
			this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton3.Text = "toolStripButton3";
			this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = global::RaspiImporter.Properties.Resources.magnifier_zoom_in;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton1.Text = "toolStripButton1";
			this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton2.Image = global::RaspiImporter.Properties.Resources.magnifier_zoom_out;
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton2.Text = "toolStripButton2";
			this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 25);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.designer1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(955, 634);
			this.splitContainer1.SplitterDistance = 657;
			this.splitContainer1.TabIndex = 11;
			// 
			// designer1
			// 
			this.designer1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.designer1.BackColor = System.Drawing.SystemColors.Window;
			this.designer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.designer1.Location = new System.Drawing.Point(0, 0);
			this.designer1.Name = "designer1";
			this.designer1.Size = new System.Drawing.Size(657, 634);
			this.designer1.TabIndex = 2;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.tvElementTypes);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.pgSelectedElement);
			this.splitContainer2.Size = new System.Drawing.Size(294, 634);
			this.splitContainer2.SplitterDistance = 244;
			this.splitContainer2.TabIndex = 0;
			// 
			// tvElementTypes
			// 
			this.tvElementTypes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvElementTypes.ImageIndex = 0;
			this.tvElementTypes.ImageList = this.tvImagelit;
			this.tvElementTypes.Location = new System.Drawing.Point(0, 0);
			this.tvElementTypes.Name = "tvElementTypes";
			this.tvElementTypes.SelectedImageIndex = 10;
			this.tvElementTypes.Size = new System.Drawing.Size(294, 244);
			this.tvElementTypes.TabIndex = 0;
			this.tvElementTypes.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvElementTypes_NodeMouseClick);
			this.tvElementTypes.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvElementTypes_NodeMouseDoubleClick);
			// 
			// tvImagelit
			// 
			this.tvImagelit.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("tvImagelit.ImageStream")));
			this.tvImagelit.TransparentColor = System.Drawing.Color.Transparent;
			this.tvImagelit.Images.SetKeyName(0, "brick.png");
			this.tvImagelit.Images.SetKeyName(1, "brick_add.png");
			this.tvImagelit.Images.SetKeyName(2, "brick_delete.png");
			this.tvImagelit.Images.SetKeyName(3, "brick_edit.png");
			this.tvImagelit.Images.SetKeyName(4, "brick_error.png");
			this.tvImagelit.Images.SetKeyName(5, "brick_go.png");
			this.tvImagelit.Images.SetKeyName(6, "brick_link.png");
			this.tvImagelit.Images.SetKeyName(7, "bricks.png");
			this.tvImagelit.Images.SetKeyName(8, "folder.png");
			this.tvImagelit.Images.SetKeyName(9, "folder_add.png");
			this.tvImagelit.Images.SetKeyName(10, "folder_bell.png");
			this.tvImagelit.Images.SetKeyName(11, "folder_brick.png");
			this.tvImagelit.Images.SetKeyName(12, "folder_bug.png");
			this.tvImagelit.Images.SetKeyName(13, "folder_camera.png");
			this.tvImagelit.Images.SetKeyName(14, "folder_database.png");
			this.tvImagelit.Images.SetKeyName(15, "folder_delete.png");
			this.tvImagelit.Images.SetKeyName(16, "folder_edit.png");
			this.tvImagelit.Images.SetKeyName(17, "folder_error.png");
			this.tvImagelit.Images.SetKeyName(18, "folder_explore.png");
			this.tvImagelit.Images.SetKeyName(19, "folder_feed.png");
			this.tvImagelit.Images.SetKeyName(20, "folder_find.png");
			this.tvImagelit.Images.SetKeyName(21, "folder_go.png");
			this.tvImagelit.Images.SetKeyName(22, "folder_heart.png");
			this.tvImagelit.Images.SetKeyName(23, "folder_image.png");
			this.tvImagelit.Images.SetKeyName(24, "folder_key.png");
			this.tvImagelit.Images.SetKeyName(25, "folder_lightbulb.png");
			this.tvImagelit.Images.SetKeyName(26, "folder_link.png");
			this.tvImagelit.Images.SetKeyName(27, "folder_page.png");
			this.tvImagelit.Images.SetKeyName(28, "folder_page_white.png");
			this.tvImagelit.Images.SetKeyName(29, "folder_palette.png");
			this.tvImagelit.Images.SetKeyName(30, "folder_picture.png");
			this.tvImagelit.Images.SetKeyName(31, "folder_star.png");
			this.tvImagelit.Images.SetKeyName(32, "folder_table.png");
			this.tvImagelit.Images.SetKeyName(33, "folder_user.png");
			this.tvImagelit.Images.SetKeyName(34, "folder_wrench.png");
			// 
			// pgSelectedElement
			// 
			this.pgSelectedElement.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pgSelectedElement.Location = new System.Drawing.Point(0, 0);
			this.pgSelectedElement.Name = "pgSelectedElement";
			this.pgSelectedElement.Size = new System.Drawing.Size(294, 386);
			this.pgSelectedElement.TabIndex = 0;
			this.pgSelectedElement.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgSelectedElement_PropertyValueChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(955, 659);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "Form1";
			this.Text = "RaspiStudio V0.1 Alpha";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private Dalssoft.DiagramNet.Designer designer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.TreeView tvElementTypes;
		private System.Windows.Forms.PropertyGrid pgSelectedElement;
		private System.Windows.Forms.ImageList tvImagelit;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
		private System.Windows.Forms.ToolStripButton toolStripButton3;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolStripButton4;
	}
}

