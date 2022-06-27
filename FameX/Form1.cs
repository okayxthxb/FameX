using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Windows.Forms;
using Guna.UI.WinForms;
using Newtonsoft.Json;
using WebSocketSharp;

namespace FameX;

public class Form1 : Form
{
	public static WebSocket server;

	public static Form2 Tools = new Form2();

	public static string Version = "1.3";

	public static string Key = "";

	public int nonce = new Random().Next();

	public static SessionID session = new SessionID();

	public static Form1 instance;

	public bool UpdateMessageBoxShown = false;

	public bool ActivationMessageBoxShown = false;

	private IContainer components = null;

	private GunaElipse gunaElipse1;

	private VitalityTabControl vitalityTabControl1;

	private TabPage tabPage1;

	private GunaElipsePanel gunaElipsePanel1;

	private GunaLabel gunaLabel4;

	private GunaLabel gunaLabel3;

	private GunaImageButton gunaImageButton3;

	private GunaImageButton gunaImageButton2;

	private GunaLabel gunaLabel2;

	private GunaComboBox serverComboBox;

	private GunaTextBox passwordTextBox;

	private GunaSeparator gunaSeparator2;

	private GunaTextBox usernameTextBox;

	private GunaImageButton gunaImageButton1;

	private GunaButton gunaButton1;

	private GunaSeparator gunaSeparator1;

	private TabPage tabPage2;

	private GunaElipsePanel gunaElipsePanel2;

	private GunaLabel gunaLabel1;

	private GunaImageButton gunaImageButton4;

	private GunaImageButton gunaImageButton5;

	private GunaSeparator gunaSeparator3;

	private GunaLabel gunaLabel5;

	private GunaTextBox gunaTextBox3;

	private GunaImageButton gunaImageButton6;

	private GunaButton gunaButton2;

	private GunaSeparator gunaSeparator4;

	private GunaDragControl gunaDragControl1;

	private GunaDragControl gunaDragControl2;

	public Form1()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		/*server = new WebSocket("ws://51.77.175.204:421", Array.Empty<string>());
		server.OnError += ((EventHandler<WebSocketSharp.ErrorEventArgs>)delegate
		{
			try
			{
				BeginInvoke((Action)delegate
				{
					base.Enabled = false;
				});
			}
			catch (Exception)
			{
			}
			try
			{
				Tools.BeginInvoke((Action)delegate
				{
					Tools.Enabled = false;
				});
			}
			catch (Exception)
			{
			}
			MessageBox.Show("There has been an error connecting to the FameX servers, the tool will close now.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			Environment.Exit(0);
			Application.Exit();
		});
		server.OnClose += (EventHandler<CloseEventArgs>)delegate
		{
			try
			{
				BeginInvoke((Action)delegate
				{
					base.Enabled = false;
				});
			}
			catch (Exception)
			{
			}
			try
			{
				Tools.BeginInvoke((Action)delegate
				{
					Tools.Enabled = false;
				});
			}
			catch (Exception)
			{
			}
			MessageBox.Show("There has been an error connecting to the FameX servers, the tool will close now.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			Environment.Exit(0);
			Application.Exit();
		};
		server.OnMessage += ((EventHandler<MessageEventArgs>)websocket_Message);
		server.Connect();*/
		InitializeComponent();
		vitalityTabControl1.SelectTab(0);
		instance = this;
	}

	public void websocket_Message(object sender, MessageEventArgs e)
	{
		string data = e.Data;
		dynamic val = JsonConvert.DeserializeObject(Encryption.DecryptPayload(data));
		switch ((string)val.op)
		{
		case "Version":
		{
			if (!((string)val.data.version != Encryption.HashSHA256("MSPM-FAMEX-" + Version)) || UpdateMessageBoxShown)
			{
				break;
			}
			UpdateMessageBoxShown = true;
			BeginInvoke((Action)delegate
			{
				base.Enabled = false;
			});
			try
			{
				Tools.BeginInvoke((Action)delegate
				{
					Tools.Enabled = false;
				});
			}
			catch (Exception)
			{
			}
			DialogResult dialogResult = MessageBox.Show("New version of the FameX has been released!\nDo you want to update now?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
			if (dialogResult == DialogResult.Yes)
			{
				Process.Start("http://195.133.40.205/famex/update.zip");
			}
			Environment.Exit(0);
			Application.Exit();
			break;
		}
		case "KeyChecked":
			if ((string)val.data == Encryption.HashSHA256($"MSPM-FAMEX-{getHwid()}-{nonce}-41366401"))
			{
				BeginInvoke((Action)delegate
				{
					vitalityTabControl1.SelectTab(0);
				});
				break;
			}
			if (!ActivationMessageBoxShown)
			{
				ActivationMessageBoxShown = true;
				sendSocket(new
				{
					op = "AccessDenied",
					data = new
					{
						HWID = getHwid(),
						Key = Key,
						Stage = 0
					}
				});
				MessageBox.Show("It seems that you haven't activated FameX yet. Please contact vida#6666 or Loc#9666 on Discord to purchase a key!", "License", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			BeginInvoke((Action)delegate
			{
				vitalityTabControl1.SelectTab(1);
				Show();
			});
			Tools.Hide();
			break;
		case "KeyRedeemed":
			if ((string)val.data == Encryption.HashSHA256($"MSPM-FAMEX-{gunaTextBox3.Text}-{nonce}-60000597"))
			{
				ActivationMessageBoxShown = false;
				MessageBox.Show("Welcome to FameX, " + Environment.UserName + "!\nIf you have any questions regarding the tool, feel free to ask them in the Discord server!", "License", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				File.WriteAllText(folderPath + "\\z6nu2j4a.txt", gunaTextBox3.Text);
				BeginInvoke((Action)delegate
				{
					vitalityTabControl1.SelectTab(0);
				});
			}
			else
			{
				sendSocket(new
				{
					op = "AccessDenied",
					data = new
					{
						HWID = getHwid(),
						Key = gunaTextBox3.Text,
						Stage = 1
					}
				});
				MessageBox.Show("Hm, It seems the key you applied was invalid. Please contact vida#6666 or Loc#9666 on Discord to purchase a key!", "License", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			break;
		case "ActorList":
			foreach (dynamic item in val.data)
			{
				Tools.moviestars.Add(Convert.ToInt32(item.id));
			}
			Tools.CreateMovie();
			break;
		}
	}

	public void sendSocket(dynamic obj)
	{
		try
		{
			server.Send(Encryption.EncryptPayload(JsonConvert.SerializeObject(obj)));
		}
		catch (Exception)
		{
		}
	}

	public void Initialize()
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		if (File.Exists(folderPath + "\\z6nu2j4a.txt"))
		{
			Key = File.ReadAllText(folderPath + "\\z6nu2j4a.txt");
		}
		nonce = new Random().Next();
		sendSocket(new
		{
			op = "CheckVersion"
		});
		sendSocket(new
		{
			op = "CheckKey",
			data = new
			{
				HWID = getHwid(),
				Key = Key,
				Nonce = nonce
			}
		});
	}

	public string getHwid()
	{
		ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
		ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
		string data = "";
		using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectCollection.GetEnumerator())
		{
			if (managementObjectEnumerator.MoveNext())
			{
				ManagementObject managementObject = (ManagementObject)managementObjectEnumerator.Current;
				data = managementObject["ProcessorId"].ToString();
			}
		}
		return "MSPM," + Environment.UserName + "," + Encryption.HashSHA256(data);
	}

	private void gunaButton2_Click(object sender, EventArgs e)
	{
		nonce = new Random().Next();
		sendSocket(new
		{
			op = "Activate",
			data = new
			{
				HWID = getHwid(),
				Key = gunaTextBox3.Text,
				Nonce = nonce
			}
		});
	}

	private void gunaButton1_Click(object sender, EventArgs e)
	{
		if (usernameTextBox.Text == "" || passwordTextBox.Text == "" || serverComboBox.Text == "")
		{
			MessageBox.Show("Please fill out all the fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		AMF.sessionID = session.GetSessionId();
		AMF.GetEndpointForServer(serverComboBox.Text);
		dynamic val = AMF.SendAMF("MovieStarPlanet.WebService.User.AMFUserServiceWeb.Login", new object[6] { usernameTextBox.Text, passwordTextBox.Text, null, null, null, "MSP1-Standalone:XXXXXX" });
		if (val.ToString().Contains("ERROR"))
		{
			MessageBox.Show("There has been an error logging in! Please try to change your VPN location.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		else if (val["loginStatus"]["status"] == "Success" || val["loginStatus"]["status"] == "ThirdPartyCreated")
		{
			MessageBox.Show("Successfully logged in!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			Hide();
			Tools.SetValues(val["loginStatus"]["actor"]["ActorId"], val["loginStatus"]["ticket"], serverComboBox.Text, val["loginStatus"]["actor"]["Name"]);
			Tools.Show();
		}
		else if (val["loginStatus"]["status"] == "InvalidCredentials")
		{
			MessageBox.Show("Invalid username or password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		else
		{
			MessageBox.Show("User is banned, please login to not banned user!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void gunaImageButton2_Click(object sender, EventArgs e)
	{
		Environment.Exit(0);
		Application.Exit();
	}

	private void gunaImageButton3_Click(object sender, EventArgs e)
	{
		base.WindowState = FormWindowState.Minimized;
	}

	private void gunaImageButton4_Click(object sender, EventArgs e)
	{
		base.WindowState = FormWindowState.Minimized;
	}

	private void gunaImageButton5_Click(object sender, EventArgs e)
	{
		Environment.Exit(0);
		Application.Exit();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
            this.components = new System.ComponentModel.Container();
            this.gunaElipse1 = new Guna.UI.WinForms.GunaElipse(this.components);
            this.gunaDragControl1 = new Guna.UI.WinForms.GunaDragControl(this.components);
            this.gunaElipsePanel1 = new Guna.UI.WinForms.GunaElipsePanel();
            this.gunaLabel4 = new Guna.UI.WinForms.GunaLabel();
            this.gunaLabel3 = new Guna.UI.WinForms.GunaLabel();
            this.gunaImageButton3 = new Guna.UI.WinForms.GunaImageButton();
            this.gunaImageButton2 = new Guna.UI.WinForms.GunaImageButton();
            this.gunaLabel2 = new Guna.UI.WinForms.GunaLabel();
            this.serverComboBox = new Guna.UI.WinForms.GunaComboBox();
            this.passwordTextBox = new Guna.UI.WinForms.GunaTextBox();
            this.gunaSeparator2 = new Guna.UI.WinForms.GunaSeparator();
            this.usernameTextBox = new Guna.UI.WinForms.GunaTextBox();
            this.gunaImageButton1 = new Guna.UI.WinForms.GunaImageButton();
            this.gunaButton1 = new Guna.UI.WinForms.GunaButton();
            this.gunaSeparator1 = new Guna.UI.WinForms.GunaSeparator();
            this.gunaDragControl2 = new Guna.UI.WinForms.GunaDragControl(this.components);
            this.gunaElipsePanel2 = new Guna.UI.WinForms.GunaElipsePanel();
            this.gunaLabel1 = new Guna.UI.WinForms.GunaLabel();
            this.gunaImageButton4 = new Guna.UI.WinForms.GunaImageButton();
            this.gunaImageButton5 = new Guna.UI.WinForms.GunaImageButton();
            this.gunaSeparator3 = new Guna.UI.WinForms.GunaSeparator();
            this.gunaLabel5 = new Guna.UI.WinForms.GunaLabel();
            this.gunaTextBox3 = new Guna.UI.WinForms.GunaTextBox();
            this.gunaImageButton6 = new Guna.UI.WinForms.GunaImageButton();
            this.gunaButton2 = new Guna.UI.WinForms.GunaButton();
            this.gunaSeparator4 = new Guna.UI.WinForms.GunaSeparator();
            this.vitalityTabControl1 = new VitalityTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gunaElipsePanel1.SuspendLayout();
            this.gunaElipsePanel2.SuspendLayout();
            this.vitalityTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gunaElipse1
            // 
            this.gunaElipse1.TargetControl = this;
            // 
            // gunaDragControl1
            // 
            this.gunaDragControl1.TargetControl = this.gunaElipsePanel1;
            // 
            // gunaElipsePanel1
            // 
            this.gunaElipsePanel1.BackColor = System.Drawing.Color.Transparent;
            this.gunaElipsePanel1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.gunaElipsePanel1.Controls.Add(this.gunaLabel4);
            this.gunaElipsePanel1.Controls.Add(this.gunaLabel3);
            this.gunaElipsePanel1.Controls.Add(this.gunaImageButton3);
            this.gunaElipsePanel1.Controls.Add(this.gunaImageButton2);
            this.gunaElipsePanel1.Controls.Add(this.gunaLabel2);
            this.gunaElipsePanel1.Controls.Add(this.serverComboBox);
            this.gunaElipsePanel1.Controls.Add(this.passwordTextBox);
            this.gunaElipsePanel1.Controls.Add(this.gunaSeparator2);
            this.gunaElipsePanel1.Controls.Add(this.usernameTextBox);
            this.gunaElipsePanel1.Controls.Add(this.gunaImageButton1);
            this.gunaElipsePanel1.Controls.Add(this.gunaButton1);
            this.gunaElipsePanel1.Controls.Add(this.gunaSeparator1);
            this.gunaElipsePanel1.Location = new System.Drawing.Point(6, 6);
            this.gunaElipsePanel1.Name = "gunaElipsePanel1";
            this.gunaElipsePanel1.Radius = 3;
            this.gunaElipsePanel1.Size = new System.Drawing.Size(289, 440);
            this.gunaElipsePanel1.TabIndex = 1;
            // 
            // gunaLabel4
            // 
            this.gunaLabel4.AutoSize = true;
            this.gunaLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gunaLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.gunaLabel4.Location = new System.Drawing.Point(86, 142);
            this.gunaLabel4.Name = "gunaLabel4";
            this.gunaLabel4.Size = new System.Drawing.Size(113, 20);
            this.gunaLabel4.TabIndex = 16;
            this.gunaLabel4.Text = "FameX - Login";
            // 
            // gunaLabel3
            // 
            this.gunaLabel3.AutoSize = true;
            this.gunaLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gunaLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.gunaLabel3.Location = new System.Drawing.Point(84, 417);
            this.gunaLabel3.Name = "gunaLabel3";
            this.gunaLabel3.Size = new System.Drawing.Size(110, 13);
            this.gunaLabel3.TabIndex = 15;
            this.gunaLabel3.Text = "Loc#9666 vida#6666";
            // 
            // gunaImageButton3
            // 
            this.gunaImageButton3.DialogResult = System.Windows.Forms.DialogResult.None;
            this.gunaImageButton3.Image = null;
            this.gunaImageButton3.ImageSize = new System.Drawing.Size(20, 20);
            this.gunaImageButton3.Location = new System.Drawing.Point(244, 4);
            this.gunaImageButton3.Name = "gunaImageButton3";
            this.gunaImageButton3.OnHoverImage = null;
            this.gunaImageButton3.OnHoverImageOffset = new System.Drawing.Point(0, 0);
            this.gunaImageButton3.Size = new System.Drawing.Size(20, 20);
            this.gunaImageButton3.TabIndex = 14;
            this.gunaImageButton3.Click += new System.EventHandler(this.gunaImageButton3_Click);
            // 
            // gunaImageButton2
            // 
            this.gunaImageButton2.DialogResult = System.Windows.Forms.DialogResult.None;
            this.gunaImageButton2.Image = null;
            this.gunaImageButton2.ImageSize = new System.Drawing.Size(20, 20);
            this.gunaImageButton2.Location = new System.Drawing.Point(265, 4);
            this.gunaImageButton2.Name = "gunaImageButton2";
            this.gunaImageButton2.OnHoverImage = null;
            this.gunaImageButton2.OnHoverImageOffset = new System.Drawing.Point(0, 0);
            this.gunaImageButton2.Size = new System.Drawing.Size(20, 20);
            this.gunaImageButton2.TabIndex = 13;
            this.gunaImageButton2.Click += new System.EventHandler(this.gunaImageButton2_Click);
            // 
            // gunaLabel2
            // 
            this.gunaLabel2.AutoSize = true;
            this.gunaLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.gunaLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gunaLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.gunaLabel2.Location = new System.Drawing.Point(121, 278);
            this.gunaLabel2.Name = "gunaLabel2";
            this.gunaLabel2.Size = new System.Drawing.Size(51, 13);
            this.gunaLabel2.TabIndex = 12;
            this.gunaLabel2.Text = "SERVER";
            // 
            // serverComboBox
            // 
            this.serverComboBox.BackColor = System.Drawing.Color.Transparent;
            this.serverComboBox.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.serverComboBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.serverComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.serverComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.serverComboBox.FocusedColor = System.Drawing.Color.Empty;
            this.serverComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverComboBox.ForeColor = System.Drawing.Color.Silver;
            this.serverComboBox.FormattingEnabled = true;
            this.serverComboBox.Items.AddRange(new object[] {
            "US",
            "GB",
            "NL",
            "AU",
            "CA",
            "DK",
            "NO",
            "SE",
            "FI",
            "DE",
            "PL",
            "FR",
            "TR",
            "IE",
            "NZ",
            "ES"});
            this.serverComboBox.Location = new System.Drawing.Point(19, 271);
            this.serverComboBox.Name = "serverComboBox";
            this.serverComboBox.OnHoverItemBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.serverComboBox.OnHoverItemForeColor = System.Drawing.Color.White;
            this.serverComboBox.Radius = 2;
            this.serverComboBox.Size = new System.Drawing.Size(250, 27);
            this.serverComboBox.TabIndex = 11;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.BackColor = System.Drawing.Color.Transparent;
            this.passwordTextBox.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.passwordTextBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.passwordTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.passwordTextBox.FocusedBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.passwordTextBox.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.passwordTextBox.FocusedForeColor = System.Drawing.Color.White;
            this.passwordTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordTextBox.ForeColor = System.Drawing.Color.Silver;
            this.passwordTextBox.Location = new System.Drawing.Point(19, 234);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '●';
            this.passwordTextBox.Radius = 2;
            this.passwordTextBox.SelectedText = "";
            this.passwordTextBox.Size = new System.Drawing.Size(250, 30);
            this.passwordTextBox.TabIndex = 10;
            this.passwordTextBox.Text = "PASSWORD";
            this.passwordTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // gunaSeparator2
            // 
            this.gunaSeparator2.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.gunaSeparator2.Location = new System.Drawing.Point(19, 348);
            this.gunaSeparator2.Name = "gunaSeparator2";
            this.gunaSeparator2.Size = new System.Drawing.Size(250, 10);
            this.gunaSeparator2.TabIndex = 9;
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.BackColor = System.Drawing.Color.Transparent;
            this.usernameTextBox.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.usernameTextBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.usernameTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.usernameTextBox.FocusedBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.usernameTextBox.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.usernameTextBox.FocusedForeColor = System.Drawing.Color.White;
            this.usernameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usernameTextBox.ForeColor = System.Drawing.Color.Silver;
            this.usernameTextBox.Location = new System.Drawing.Point(19, 196);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.PasswordChar = '\0';
            this.usernameTextBox.Radius = 2;
            this.usernameTextBox.SelectedText = "";
            this.usernameTextBox.Size = new System.Drawing.Size(250, 30);
            this.usernameTextBox.TabIndex = 7;
            this.usernameTextBox.Text = "USERNAME";
            this.usernameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // gunaImageButton1
            // 
            this.gunaImageButton1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.gunaImageButton1.Image = null;
            this.gunaImageButton1.ImageSize = new System.Drawing.Size(100, 100);
            this.gunaImageButton1.Location = new System.Drawing.Point(85, 24);
            this.gunaImageButton1.Name = "gunaImageButton1";
            this.gunaImageButton1.OnHoverImage = null;
            this.gunaImageButton1.OnHoverImageOffset = new System.Drawing.Point(0, 0);
            this.gunaImageButton1.Size = new System.Drawing.Size(119, 115);
            this.gunaImageButton1.TabIndex = 6;
            // 
            // gunaButton1
            // 
            this.gunaButton1.Animated = true;
            this.gunaButton1.AnimationHoverSpeed = 0.07F;
            this.gunaButton1.AnimationSpeed = 0.03F;
            this.gunaButton1.BackColor = System.Drawing.Color.Transparent;
            this.gunaButton1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.gunaButton1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.gunaButton1.BorderSize = 2;
            this.gunaButton1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.gunaButton1.FocusedColor = System.Drawing.Color.Empty;
            this.gunaButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gunaButton1.ForeColor = System.Drawing.Color.Silver;
            this.gunaButton1.Image = null;
            this.gunaButton1.ImageSize = new System.Drawing.Size(20, 20);
            this.gunaButton1.Location = new System.Drawing.Point(19, 306);
            this.gunaButton1.Name = "gunaButton1";
            this.gunaButton1.OnHoverBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.gunaButton1.OnHoverBorderColor = System.Drawing.Color.Empty;
            this.gunaButton1.OnHoverForeColor = System.Drawing.Color.White;
            this.gunaButton1.OnHoverImage = null;
            this.gunaButton1.OnPressedColor = System.Drawing.Color.Black;
            this.gunaButton1.Radius = 2;
            this.gunaButton1.Size = new System.Drawing.Size(250, 30);
            this.gunaButton1.TabIndex = 4;
            this.gunaButton1.Text = "LOGIN";
            this.gunaButton1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.gunaButton1.Click += new System.EventHandler(this.gunaButton1_Click);
            // 
            // gunaSeparator1
            // 
            this.gunaSeparator1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.gunaSeparator1.Location = new System.Drawing.Point(19, 172);
            this.gunaSeparator1.Name = "gunaSeparator1";
            this.gunaSeparator1.Size = new System.Drawing.Size(250, 10);
            this.gunaSeparator1.TabIndex = 0;
            // 
            // gunaDragControl2
            // 
            this.gunaDragControl2.TargetControl = this.gunaElipsePanel2;
            // 
            // gunaElipsePanel2
            // 
            this.gunaElipsePanel2.BackColor = System.Drawing.Color.Transparent;
            this.gunaElipsePanel2.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.gunaElipsePanel2.Controls.Add(this.gunaLabel1);
            this.gunaElipsePanel2.Controls.Add(this.gunaImageButton4);
            this.gunaElipsePanel2.Controls.Add(this.gunaImageButton5);
            this.gunaElipsePanel2.Controls.Add(this.gunaSeparator3);
            this.gunaElipsePanel2.Controls.Add(this.gunaLabel5);
            this.gunaElipsePanel2.Controls.Add(this.gunaTextBox3);
            this.gunaElipsePanel2.Controls.Add(this.gunaImageButton6);
            this.gunaElipsePanel2.Controls.Add(this.gunaButton2);
            this.gunaElipsePanel2.Controls.Add(this.gunaSeparator4);
            this.gunaElipsePanel2.Location = new System.Drawing.Point(6, 6);
            this.gunaElipsePanel2.Name = "gunaElipsePanel2";
            this.gunaElipsePanel2.Radius = 3;
            this.gunaElipsePanel2.Size = new System.Drawing.Size(289, 436);
            this.gunaElipsePanel2.TabIndex = 1;
            // 
            // gunaLabel1
            // 
            this.gunaLabel1.AutoSize = true;
            this.gunaLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gunaLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.gunaLabel1.Location = new System.Drawing.Point(84, 417);
            this.gunaLabel1.Name = "gunaLabel1";
            this.gunaLabel1.Size = new System.Drawing.Size(110, 13);
            this.gunaLabel1.TabIndex = 12;
            this.gunaLabel1.Text = "Loc#9666 vida#6666";
            // 
            // gunaImageButton4
            // 
            this.gunaImageButton4.DialogResult = System.Windows.Forms.DialogResult.None;
            this.gunaImageButton4.Image = null;
            this.gunaImageButton4.ImageSize = new System.Drawing.Size(20, 20);
            this.gunaImageButton4.Location = new System.Drawing.Point(244, 4);
            this.gunaImageButton4.Name = "gunaImageButton4";
            this.gunaImageButton4.OnHoverImage = null;
            this.gunaImageButton4.OnHoverImageOffset = new System.Drawing.Point(0, 0);
            this.gunaImageButton4.Size = new System.Drawing.Size(20, 20);
            this.gunaImageButton4.TabIndex = 11;
            this.gunaImageButton4.Click += new System.EventHandler(this.gunaImageButton4_Click);
            // 
            // gunaImageButton5
            // 
            this.gunaImageButton5.DialogResult = System.Windows.Forms.DialogResult.None;
            this.gunaImageButton5.Image = null;
            this.gunaImageButton5.ImageSize = new System.Drawing.Size(20, 20);
            this.gunaImageButton5.Location = new System.Drawing.Point(265, 4);
            this.gunaImageButton5.Name = "gunaImageButton5";
            this.gunaImageButton5.OnHoverImage = null;
            this.gunaImageButton5.OnHoverImageOffset = new System.Drawing.Point(0, 0);
            this.gunaImageButton5.Size = new System.Drawing.Size(20, 20);
            this.gunaImageButton5.TabIndex = 10;
            this.gunaImageButton5.Click += new System.EventHandler(this.gunaImageButton5_Click);
            // 
            // gunaSeparator3
            // 
            this.gunaSeparator3.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.gunaSeparator3.Location = new System.Drawing.Point(19, 285);
            this.gunaSeparator3.Name = "gunaSeparator3";
            this.gunaSeparator3.Size = new System.Drawing.Size(250, 10);
            this.gunaSeparator3.TabIndex = 9;
            // 
            // gunaLabel5
            // 
            this.gunaLabel5.AutoSize = true;
            this.gunaLabel5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gunaLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.gunaLabel5.Location = new System.Drawing.Point(113, 142);
            this.gunaLabel5.Name = "gunaLabel5";
            this.gunaLabel5.Size = new System.Drawing.Size(61, 20);
            this.gunaLabel5.TabIndex = 8;
            this.gunaLabel5.Text = "FameX";
            // 
            // gunaTextBox3
            // 
            this.gunaTextBox3.BackColor = System.Drawing.Color.Transparent;
            this.gunaTextBox3.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.gunaTextBox3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.gunaTextBox3.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.gunaTextBox3.FocusedBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.gunaTextBox3.FocusedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.gunaTextBox3.FocusedForeColor = System.Drawing.Color.White;
            this.gunaTextBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gunaTextBox3.ForeColor = System.Drawing.Color.Silver;
            this.gunaTextBox3.Location = new System.Drawing.Point(19, 198);
            this.gunaTextBox3.Name = "gunaTextBox3";
            this.gunaTextBox3.PasswordChar = '\0';
            this.gunaTextBox3.Radius = 2;
            this.gunaTextBox3.SelectedText = "";
            this.gunaTextBox3.Size = new System.Drawing.Size(250, 30);
            this.gunaTextBox3.TabIndex = 7;
            this.gunaTextBox3.Text = "ENTER FAMEX KEY";
            this.gunaTextBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // gunaImageButton6
            // 
            this.gunaImageButton6.DialogResult = System.Windows.Forms.DialogResult.None;
            this.gunaImageButton6.Image = null;
            this.gunaImageButton6.ImageSize = new System.Drawing.Size(100, 100);
            this.gunaImageButton6.Location = new System.Drawing.Point(85, 24);
            this.gunaImageButton6.Name = "gunaImageButton6";
            this.gunaImageButton6.OnHoverImage = null;
            this.gunaImageButton6.OnHoverImageOffset = new System.Drawing.Point(0, 0);
            this.gunaImageButton6.Size = new System.Drawing.Size(119, 115);
            this.gunaImageButton6.TabIndex = 6;
            // 
            // gunaButton2
            // 
            this.gunaButton2.Animated = true;
            this.gunaButton2.AnimationHoverSpeed = 0.07F;
            this.gunaButton2.AnimationSpeed = 0.03F;
            this.gunaButton2.BackColor = System.Drawing.Color.Transparent;
            this.gunaButton2.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.gunaButton2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.gunaButton2.BorderSize = 2;
            this.gunaButton2.DialogResult = System.Windows.Forms.DialogResult.None;
            this.gunaButton2.FocusedColor = System.Drawing.Color.Empty;
            this.gunaButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gunaButton2.ForeColor = System.Drawing.Color.Silver;
            this.gunaButton2.Image = null;
            this.gunaButton2.ImageSize = new System.Drawing.Size(20, 20);
            this.gunaButton2.Location = new System.Drawing.Point(19, 241);
            this.gunaButton2.Name = "gunaButton2";
            this.gunaButton2.OnHoverBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.gunaButton2.OnHoverBorderColor = System.Drawing.Color.Empty;
            this.gunaButton2.OnHoverForeColor = System.Drawing.Color.White;
            this.gunaButton2.OnHoverImage = null;
            this.gunaButton2.OnPressedColor = System.Drawing.Color.Black;
            this.gunaButton2.Radius = 2;
            this.gunaButton2.Size = new System.Drawing.Size(250, 30);
            this.gunaButton2.TabIndex = 4;
            this.gunaButton2.Text = "ACTIVATE";
            this.gunaButton2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.gunaButton2.Click += new System.EventHandler(this.gunaButton2_Click);
            // 
            // gunaSeparator4
            // 
            this.gunaSeparator4.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.gunaSeparator4.Location = new System.Drawing.Point(19, 172);
            this.gunaSeparator4.Name = "gunaSeparator4";
            this.gunaSeparator4.Size = new System.Drawing.Size(250, 10);
            this.gunaSeparator4.TabIndex = 0;
            // 
            // vitalityTabControl1
            // 
            this.vitalityTabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.vitalityTabControl1.Controls.Add(this.tabPage1);
            this.vitalityTabControl1.Controls.Add(this.tabPage2);
            this.vitalityTabControl1.Location = new System.Drawing.Point(-5, -5);
            this.vitalityTabControl1.Name = "vitalityTabControl1";
            this.vitalityTabControl1.SelectedIndex = 0;
            this.vitalityTabControl1.Size = new System.Drawing.Size(310, 495);
            this.vitalityTabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.tabPage1.Controls.Add(this.gunaElipsePanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(302, 466);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(21)))), ((int)(((byte)(21)))));
            this.tabPage2.Controls.Add(this.gunaElipsePanel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(302, 466);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 451);
            this.Controls.Add(this.vitalityTabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.gunaElipsePanel1.ResumeLayout(false);
            this.gunaElipsePanel1.PerformLayout();
            this.gunaElipsePanel2.ResumeLayout(false);
            this.gunaElipsePanel2.PerformLayout();
            this.vitalityTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

	}
}
