using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using MultiMC.GUI;

namespace MultiMC.WinGUI
{
	public partial class SettingsForm : WinFormsDialog, ISettingsDialog
	{
		public SettingsForm()
		{
			InitializeComponent();

			minMemAllocSpinner.Maximum = 65536;
			minMemAllocSpinner.Minimum = 256;
			minMemAllocSpinner.Increment = 256;

			maxMemAllocSpinner.Maximum = 65536;
			maxMemAllocSpinner.Minimum = 512;
			maxMemAllocSpinner.Increment = 256;

			LoadSettings();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			OnResponse(GUI.DialogResponse.OK);
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			OnResponse(GUI.DialogResponse.Cancel);
		}

		protected override void OnResponse(DialogResponse response)
		{
			if (response == DialogResponse.OK)
				ApplySettings();

			base.OnResponse(response);
		}

		void ApplySettings()
		{
			AppSettings.Main.ShowConsole = checkBoxShowConsole.Checked;
			AppSettings.Main.AutoCloseConsole = checkBoxAutoClose.Checked;

			AppSettings.Main.AutoUpdate = autoUpdateCheckBox.Checked;

			if (AppSettings.Main.RawInstanceDir != textBoxInstDir.Text)
				ChangeInstDir();

			AppSettings.Main.MinMemoryAlloc = (int)minMemAllocSpinner.Value;
			AppSettings.Main.MaxMemoryAlloc = (int)maxMemAllocSpinner.Value;

			AppSettings.Main.JavaPath = textBoxJavaPath.Text;
			AppSettings.Main.Save();
			LoadSettings();
		}

		void LoadSettings()
		{
			AppSettings.Main.Load();

			checkBoxShowConsole.Checked = AppSettings.Main.ShowConsole;
			checkBoxAutoClose.Checked = AppSettings.Main.AutoCloseConsole;

			autoUpdateCheckBox.Checked = AppSettings.Main.AutoUpdate;

			textBoxInstDir.Text = AppSettings.Main.RawInstanceDir;

			minMemAllocSpinner.Value = AppSettings.Main.MinMemoryAlloc;
			maxMemAllocSpinner.Value = AppSettings.Main.MaxMemoryAlloc;

			textBoxJavaPath.Text = AppSettings.Main.JavaPath;
		}

		void ChangeInstDir()
		{
			DialogResponse response = MessageDialog.Show(this,
					"You have changed the location of your instance " +
					" folder. Would you like to move all of your instances to " +
					"the new location?", "Transfer instances?",
					MessageButtons.YesNoCancel);

			string oldInstDir = AppSettings.Main.RawInstanceDir;
			string resolvedOldInstDir = AppSettings.Main.InstanceDir;

			string newInstDir = textBoxInstDir.Text;

			if (response != DialogResponse.Cancel)
			{
				AppSettings.Main.InstanceDir = newInstDir;
			}
			else
			{
				AppSettings.Main.InstanceDir = oldInstDir;
				return;
			}

			if (response == DialogResponse.Yes)
			{
			Retry:
				string resolvedNewInstDir = 
					Environment.ExpandEnvironmentVariables(newInstDir);

				if (Directory.Exists(resolvedNewInstDir))
				{
					DialogResponse response2 = MessageDialog.Show(this,
						"That folder already exists.", "Error",
						MessageButtons.RetryCancel);
					if (response2 == DialogResponse.OK)
					{
						goto Retry;
					}
					else
					{
						AppSettings.Main.InstanceDir = oldInstDir;
					}
				}

				try
				{
					if (!Directory.Exists(Path.GetDirectoryName(resolvedNewInstDir)))
						Directory.CreateDirectory(
							Path.GetDirectoryName(resolvedNewInstDir));

					Directory.Move(oldInstDir, Path.GetFullPath(resolvedNewInstDir));
				}
				catch (IOException ex)
				{
					DialogResponse response2 = MessageDialog.Show(this,
						"Failed to move instances.\n" + ex.Message, "Error",
						MessageButtons.RetryCancel);
					if (response2 == DialogResponse.OK)
					{
						goto Retry;
					}
					else
					{
						AppSettings.Main.InstanceDir = oldInstDir;
					}
				}
				catch (UnauthorizedAccessException ex)
				{
					DialogResponse response2 = MessageDialog.Show(this,
						"Failed to move instances.\n" + ex.Message, "Error",
						MessageButtons.RetryCancel);
					if (response2 == DialogResponse.OK)
					{
						goto Retry;
					}
					else
					{
						AppSettings.Main.InstanceDir = oldInstDir;
					}
				}
			}
		}

		private void buttonAutoDetect_Click(object sender, EventArgs e)
		{
			AppSettings.Main.AutoDetectJavaPath();
			textBoxJavaPath.Text = AppSettings.Main.JavaPath;
		}

		public bool ForceUpdate
		{
			get { return forceUpdateCheckBox.Checked; }
		}

		private void instDirBrowseButton_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog folderDialog = new FolderBrowserDialog();
			folderDialog.SelectedPath = 
				Environment.ExpandEnvironmentVariables(textBoxInstDir.Text);
			folderDialog.ShowNewFolderButton = true;

			DialogResult result = folderDialog.ShowDialog(this);
			
			if (result == DialogResult.OK)
				textBoxInstDir.Text = folderDialog.SelectedPath;
		}
	}
}
