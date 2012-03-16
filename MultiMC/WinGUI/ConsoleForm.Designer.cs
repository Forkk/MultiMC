namespace MultiMC.WinGUI
{
	partial class ConsoleForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleForm));
			this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.buttonClose = new System.Windows.Forms.Button();
			this.instConsole = new System.Windows.Forms.TextBox();
			this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.showConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.killMinecraftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.buttonHide = new System.Windows.Forms.Button();
			this.buttonPanel.SuspendLayout();
			this.trayMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonPanel
			// 
			this.buttonPanel.AutoSize = true;
			this.buttonPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.buttonPanel.Controls.Add(this.buttonClose);
			this.buttonPanel.Controls.Add(this.buttonHide);
			this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.buttonPanel.Location = new System.Drawing.Point(0, 233);
			this.buttonPanel.Name = "buttonPanel";
			this.buttonPanel.Size = new System.Drawing.Size(584, 29);
			this.buttonPanel.TabIndex = 0;
			// 
			// buttonClose
			// 
			this.buttonClose.Location = new System.Drawing.Point(506, 3);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(75, 23);
			this.buttonClose.TabIndex = 0;
			this.buttonClose.Text = "&Close";
			this.buttonClose.UseVisualStyleBackColor = true;
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// instConsole
			// 
			this.instConsole.Dock = System.Windows.Forms.DockStyle.Fill;
			this.instConsole.Location = new System.Drawing.Point(0, 0);
			this.instConsole.Multiline = true;
			this.instConsole.Name = "instConsole";
			this.instConsole.ReadOnly = true;
			this.instConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.instConsole.Size = new System.Drawing.Size(584, 233);
			this.instConsole.TabIndex = 1;
			this.instConsole.TabStop = false;
			// 
			// trayIcon
			// 
			this.trayIcon.ContextMenuStrip = this.trayMenu;
			this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
			this.trayIcon.Text = "MultiMC Console";
			this.trayIcon.Visible = true;
			// 
			// trayMenu
			// 
			this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showConsoleToolStripMenuItem,
            this.killMinecraftToolStripMenuItem});
			this.trayMenu.Name = "trayMenu";
			this.trayMenu.Size = new System.Drawing.Size(150, 48);
			// 
			// showConsoleToolStripMenuItem
			// 
			this.showConsoleToolStripMenuItem.Name = "showConsoleToolStripMenuItem";
			this.showConsoleToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
			this.showConsoleToolStripMenuItem.Text = "Show Console";
			this.showConsoleToolStripMenuItem.Click += new System.EventHandler(this.showConsoleToolStripMenuItem_Click);
			// 
			// killMinecraftToolStripMenuItem
			// 
			this.killMinecraftToolStripMenuItem.Name = "killMinecraftToolStripMenuItem";
			this.killMinecraftToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
			this.killMinecraftToolStripMenuItem.Text = "Kill Minecraft";
			// 
			// buttonHide
			// 
			this.buttonHide.Location = new System.Drawing.Point(425, 3);
			this.buttonHide.Name = "buttonHide";
			this.buttonHide.Size = new System.Drawing.Size(75, 23);
			this.buttonHide.TabIndex = 1;
			this.buttonHide.Text = "&Hide";
			this.buttonHide.UseVisualStyleBackColor = true;
			this.buttonHide.Click += new System.EventHandler(this.buttonHide_Click);
			// 
			// ConsoleForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(584, 262);
			this.ControlBox = false;
			this.Controls.Add(this.instConsole);
			this.Controls.Add(this.buttonPanel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConsoleForm";
			this.ShowIcon = false;
			this.Text = "MultiMC Console";
			this.Title = "MultiMC Console";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConsoleForm_FormClosed);
			this.Shown += new System.EventHandler(this.ConsoleForm_Shown);
			this.buttonPanel.ResumeLayout(false);
			this.trayMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel buttonPanel;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.TextBox instConsole;
		private System.Windows.Forms.NotifyIcon trayIcon;
		private System.Windows.Forms.ContextMenuStrip trayMenu;
		private System.Windows.Forms.ToolStripMenuItem showConsoleToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem killMinecraftToolStripMenuItem;
		private System.Windows.Forms.Button buttonHide;

	}
}