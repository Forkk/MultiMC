using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MultiMC.GUI;

namespace MultiMC.WinGUI
{
	public partial class SettingsForm : WinFormsDialog
	{
		public SettingsForm()
		{
			InitializeComponent();

			minMemAllocSpinner.Maximum = 65536;
			minMemAllocSpinner.Minimum = 512;
			minMemAllocSpinner.Increment = 512;

			maxMemAllocSpinner.Maximum = 65536;
			maxMemAllocSpinner.Minimum = 512;
			maxMemAllocSpinner.Increment = 512;

			LoadSettings();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			OnResponse(GUI.DialogResponse.OK);
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

			minMemAllocSpinner.Value = AppSettings.Main.MinMemoryAlloc;
			maxMemAllocSpinner.Value = AppSettings.Main.MaxMemoryAlloc;

			textBoxJavaPath.Text = AppSettings.Main.JavaPath;
		}
	}
}
