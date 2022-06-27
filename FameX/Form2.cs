using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI.WinForms;
using WebSocketSharp;

namespace FameX;

public class Form2 : Form
{
	public static string Ticket = "";

	public static int ActorId = 0;

	public static string[] Bots = new string[0];

	public static SessionID session = new SessionID();

	public static string server = "";

	public static string webSocketPath = "";

	public ExecuteApi2 Validator = new ExecuteApi2();

	public static int botsValidated;

	public static int MovieId = 0;

	public List<object> moviestars = new List<object>();

	public int MovieViews = 0;

	public int StarCoinsEarned = 0;

	public int AutographsSent = 0;

	private static Random random = new Random();

	private IContainer components = null;

	private GunaElipse gunaElipse1;

	private VitalityTabControl vitalityTabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private TabPage tabPage3;

	private GunaElipsePanel gunaElipsePanel1;

	private GunaLabel gunaLabel10;

	private GunaLabel gunaLabel9;

	private GunaLabel gunaLabel7;

	private GunaButton gunaButton3;

	private GunaLabel gunaLabel5;

	private GunaLabel gunaLabel1;

	private GunaImageButton gunaImageButton4;

	private GunaButton gunaButton1;

	private GunaLabel gunaLabel4;

	private GunaTileButton gunaTileButton3;

	private GunaTileButton gunaTileButton2;

	private GunaTileButton gunaTileButton1;

	private GunaImageButton gunaImageButton1;

	private GunaLabel gunaLabel3;

	private GunaImageButton gunaImageButton3;

	private GunaImageButton gunaImageButton2;

	private GunaSeparator gunaSeparator1;

	private GunaElipsePanel gunaElipsePanel2;

	private GunaGroupBox gunaGroupBox1;

	private GunaPictureBox gunaPictureBox1;

	private GunaButton gunaButton4;

	private GunaButton gunaButton5;

	private GunaImageButton gunaImageButton6;

	private GunaImageButton gunaImageButton5;

	private GunaLabel gunaLabel11;

	private GunaLabel gunaLabel12;

	private GunaLabel gunaLabel15;

	private GunaImageButton gunaImageButton7;

	private GunaLabel gunaLabel16;

	private GunaTileButton gunaTileButton4;

	private GunaTileButton gunaTileButton5;

	private GunaTileButton gunaTileButton6;

	private GunaImageButton gunaImageButton8;

	private GunaLabel gunaLabel17;

	private GunaImageButton gunaImageButton9;

	private GunaImageButton gunaImageButton10;

	private GunaSeparator gunaSeparator2;

	private GunaElipsePanel gunaElipsePanel3;

	private GunaLabel gunaLabel18;

	private GunaLabel gunaLabel19;

	private GunaLabel gunaLabel20;

	private GunaImageButton gunaImageButton11;

	private GunaButton gunaButton6;

	private GunaLabel gunaLabel21;

	private GunaTileButton gunaTileButton7;

	private GunaTileButton gunaTileButton8;

	private GunaTileButton gunaTileButton9;

	private GunaImageButton gunaImageButton12;

	private GunaLabel gunaLabel22;

	private GunaImageButton gunaImageButton13;

	private GunaImageButton gunaImageButton14;

	private GunaSeparator gunaSeparator3;

	private GunaDragControl gunaDragControl1;

	private GunaDragControl gunaDragControl2;

	private GunaDragControl gunaDragControl3;

	private BackgroundWorker backgroundWorker1;

	private BackgroundWorker backgroundWorker2;

	private BackgroundWorker backgroundWorker3;

	private GunaCheckBox gunaCheckBox1;

	public Form2()
	{
		InitializeComponent();
	}

	public void SetValues(int aid, string ticket, string Server, string Name)
	{
		ActorId = aid;
		Ticket = ticket;
		server = Server;
		webSocketPath = getWebSocketUrl(Server);
	}

	public string getWebSocketUrl(string server)
	{
		WebClient webClient = new WebClient();
		webClient.Proxy = null;
		return webClient.DownloadString((server == "US") ? "https://presence-us.mspapis.com/getServer" : "https://presence.mspapis.com/getServer");
	}

	private void gunaButton1_Click(object sender, EventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "Text|*.txt|All|*.*";
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			Bots = File.ReadAllLines(openFileDialog.FileName);
			gunaLabel5.Text = $"{Bots.Length:n0}" + "/" + $"{Bots.Length:n0}";
			gunaLabel10.Text = "0/" + $"{Bots.Length:n0}";
			gunaLabel18.Text = "0/" + $"{Bots.Length:n0}";
			MessageBox.Show("Successfully imported " + $"{Bots.Length:n0}" + " bots!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}

	private void gunaTileButton1_Click(object sender, EventArgs e)
	{
		vitalityTabControl1.SelectTab(0);
	}

	private void gunaTileButton2_Click(object sender, EventArgs e)
	{
		vitalityTabControl1.SelectTab(1);
	}

	private void gunaTileButton3_Click(object sender, EventArgs e)
	{
		vitalityTabControl1.SelectTab(2);
	}

	private void gunaTileButton6_Click(object sender, EventArgs e)
	{
		vitalityTabControl1.SelectTab(0);
	}

	private void gunaTileButton5_Click(object sender, EventArgs e)
	{
		vitalityTabControl1.SelectTab(1);
	}

	private void gunaTileButton4_Click(object sender, EventArgs e)
	{
		vitalityTabControl1.SelectTab(2);
	}

	private void gunaTileButton9_Click(object sender, EventArgs e)
	{
		vitalityTabControl1.SelectTab(0);
	}

	private void gunaTileButton8_Click(object sender, EventArgs e)
	{
		vitalityTabControl1.SelectTab(1);
	}

	private void gunaTileButton7_Click(object sender, EventArgs e)
	{
		vitalityTabControl1.SelectTab(2);
	}

	private void gunaButton5_Click(object sender, EventArgs e)
	{
		CreateMovie();
	}

	private void gunaButton2_Click(object sender, EventArgs e)
	{
		if (backgroundWorker1.IsBusy)
		{
			MessageBox.Show("Your bots are already being levelled up, please wait.", "Busy", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else
		{
			backgroundWorker1.RunWorkerAsync();
		}
	}

	private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
	{
		try
		{
			Validator.GetEndpointForServer(server);
			for (int i = 0; i < Bots.Length; i++)
			{
				string[] array = Bots[i].ToString().Split(':');
				Validator.LoginAndValidate(array[0], array[1]);
				BeginInvoke((Action)delegate
				{
					botsValidated++;
					gunaLabel10.Text = "Bots Verfied: " + $"{botsValidated:n0}" + "/" + $"{Bots.Length:n0}";
				});
			}
			MessageBox.Show("Successfully finished validating bots!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			backgroundWorker1.CancelAsync();
		}
		catch (Exception)
		{
		}
	}

	public void loginBotAMF(string user, string type)
	{
		AMF.sessionID = session.GetSessionId();
		dynamic val = AMF.SendAMF("MovieStarPlanet.WebService.User.AMFUserServiceWeb.Login", new object[6]
		{
			user.Split(':')[0],
			user.Split(':')[1],
			null,
			null,
			null,
			"MSP1-Standalone:XXXXXX"
		});
		if ((val == null) || (val.ToString().Contains("ERROR")) || !((val["loginStatus"]["status"] == "Success" || val["loginStatus"]["status"] == "ThirdPartyCreated") ? true : false) || 1 == 0)
		{
			return;
		}
		int num = (int)val["loginStatus"]["actor"]["ActorId"];
		string tick = (string)val["loginStatus"]["ticket"];
		string nebProfile = (string)val["loginStatus"]["nebulaLoginStatus"]["profileId"];
		string nebToken = (string)val["loginStatus"]["nebulaLoginStatus"]["accessToken"];
		if (!(type == "movie"))
		{
			if (type == "autograph")
			{
				ConnectSockAutograph(nebProfile, nebToken, tick);
			}
		}
		else
		{
			ConnectSockMovie(nebProfile, nebToken, tick);
		}
	}

	private void gunaButton3_Click(object sender, EventArgs e)
	{
		if (backgroundWorker1.IsBusy)
		{
			MessageBox.Show("Your bots are already being verified, please wait.", "Busy", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else
		{
			backgroundWorker1.RunWorkerAsync();
		}
	}

	public void CreateMovie()
	{
		WebClient webClient = new WebClient();
		webClient.Proxy = null;
		byte[] array = webClient.DownloadData("https://media.discordapp.net/attachments/964221395276025867/977864755991695360/Movies.png");
		byte[] array2 = webClient.DownloadData("https://media.discordapp.net/attachments/964221395276025867/977864755991695360/Movies.png");
		byte[] array3 = webClient.DownloadData("https://snapshots.mspcdns.com/v1/snapshots/MSP_AU_blob_movieactorclothesdata_0_1_120_379");
		byte[] array4 = webClient.DownloadData("https://snapshots.mspcdns.com/v1/snapshots/MSP_AU_blob_moviedata_0_1_120_379.jpg?SMode=pqh1");
		string text = RandomString(12);
		AMF.sessionID = session.GetSessionId();
		dynamic val = AMF.SendAMF("MovieStarPlanet.MobileServices.AMFMovieService.CreateMovieWithSnapshot", new object[9]
		{
			new TicketHeader
			{
				Ticket = TicketGenerator.headerTicket(Ticket)
			},
			text,
			false,
			new Random().Next(29999, 217330),
			array3,
			array4,
			moviestars.ToArray(),
			array,
			array2
		});
		if (val == null)
		{
			MessageBox.Show("Failed to create movie!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		else if (val.ToString().Contains("ERROR"))
		{
			MessageBox.Show("Failed to create movie!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		else if (val["movieId"] > -1)
		{
			MovieViews = 0;
			StarCoinsEarned = 0;
			gunaLabel12.Text = $"{MovieViews:n0}";
			gunaLabel11.Text = $"{StarCoinsEarned:n0}";
			MessageBox.Show("Successfully created movie!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			MovieId = val["movieId"];
		}
		else
		{
			MessageBox.Show("Failed to create movie!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static string RandomString(int length)
	{
		return new string((from s in Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length)
			select s[random.Next(s.Length)]).ToArray());
	}

	private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
	{
		MovieViews = 0;
		StarCoinsEarned = 0;
		gunaLabel12.Text = $"{MovieViews:n0}";
		gunaLabel11.Text = $"{StarCoinsEarned:n0}";
		if (gunaCheckBox1.Checked)
		{
			string[] bots = Bots;
			foreach (string text in bots)
			{
				loginBotAMF(text.ToString(), "movie");
			}
		}
		else
		{
			Parallel.For(0, Bots.Length, delegate(int i)
			{
				loginBotAMF(Bots[i].ToString(), "movie");
			});
		}
		new Thread((ParameterizedThreadStart)delegate
		{
			DeleteMovie(MovieId);
		}).Start();
		MessageBox.Show("Successfully finished sending movie views!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		backgroundWorker2.CancelAsync();
	}

	public static void DeleteMovie(int mid)
	{
		AMF.sessionID = session.GetSessionId();
		dynamic val = AMF.SendAMF("MovieStarPlanet.WebService.MovieService.AMFMovieService.DeleteMovie", new object[3]
		{
			new TicketHeader
			{
				Ticket = TicketGenerator.headerTicket(Ticket)
			},
			mid,
			ActorId
		});
		if (!((val == null) ? true : false) && (val.ToString().Contains("ERROR") ? true : false))
		{
			DeleteMovie(mid);
		}
	}

	public void ConnectSockMovie(string nebProfile, string nebToken, string tick)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		try
		{
			WebSocket val = new WebSocket("ws://" + webSocketPath.Replace('-', '.') + ":10843/" + webSocketPath.Replace('.', '-') + "/?transport=websocket", Array.Empty<string>());
			try
			{
				val.Connect();
				val.Send("42[\"10\",{\"messageType\":10,\"messageContent\":{\"version\":3,\"applicationId\":\"APPLICATION_WEB\",\"country\":\"" + server + "\",\"username\":\"" + nebProfile + "\",\"access_token\":\"" + nebToken + "\"}}]");
				Thread.Sleep(200);
				WatchMovie(tick);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception)
		{
		}
	}

	public void ConnectSockAutograph(string nebProfile, string nebToken, string tick)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		try
		{
			WebSocket val = new WebSocket("ws://" + webSocketPath.Replace('-', '.') + ":10843/" + webSocketPath.Replace('.', '-') + "/?transport=websocket", Array.Empty<string>());
			try
			{
				val.Connect();
				val.Send("42[\"10\",{\"messageType\":10,\"messageContent\":{\"version\":3,\"applicationId\":\"APPLICATION_WEB\",\"country\":\"" + server + "\",\"username\":\"" + nebProfile + "\",\"access_token\":\"" + nebToken + "\"}}]");
				Thread.Sleep(200);
				SendAutograph(tick);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception)
		{
		}
	}

	public void SendAutograph(string tick)
	{
		dynamic val = AMF.SendAMF("MovieStarPlanet.WebService.UserSession.AMFUserSessionService.GiveAutographAndCalculateTimestamp", new object[3]
		{
			new TicketHeader
			{
				Ticket = TicketGenerator.headerTicket(tick)
			},
			Convert.ToInt32(tick.Split(',')[1]),
			ActorId
		});
		if (!((val == null) ? true : false) && !(val.ToString().Contains("ERROR") ? true : false) && ((val["Fame"] > 0) ? true : false))
		{
			BeginInvoke((Action)delegate
			{
				AutographsSent++;
				gunaLabel18.Text = $"{AutographsSent:n0}" + "/" + $"{Bots.Length:n0}";
			});
		}
	}

	public void WatchMovie(string tick)
	{
		dynamic val = AMF.SendAMF("MovieStarPlanet.MobileServices.AMFMovieService.MovieWatched", new object[2]
		{
			new TicketHeader
			{
				Ticket = TicketGenerator.headerTicket(tick)
			},
			MovieId
		});
		if (!((val == null) ? true : false) && !(val.ToString().Contains("ERROR") ? true : false) && ((val["awardedFame"] > 10 && val["returnType"] == 2) ? true : false))
		{
			BeginInvoke((Action)delegate
			{
				MovieViews++;
				StarCoinsEarned = MovieViews * 25;
				gunaLabel12.Text = $"{MovieViews:n0}";
				gunaLabel11.Text = $"{StarCoinsEarned:n0}";
			});
		}
	}

	private void gunaButton4_Click(object sender, EventArgs e)
	{
		if (backgroundWorker2.IsBusy)
		{
			MessageBox.Show("Your movie is already being watched... Please wait", "Busy", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else if (MovieId == 0)
		{
			MessageBox.Show("You need to create a movie first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		else
		{
			backgroundWorker2.RunWorkerAsync();
		}
	}

	private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
	{
		AutographsSent = 0;
		gunaLabel18.Text = $"0/{Bots.Length:n0}";
		if (gunaCheckBox1.Checked)
		{
			string[] bots = Bots;
			foreach (string text in bots)
			{
				loginBotAMF(text.ToString(), "autograph");
			}
		}
		else
		{
			Parallel.For(0, Bots.Length, delegate(int i)
			{
				loginBotAMF(Bots[i].ToString(), "autograph");
			});
		}
		MessageBox.Show("Successfully finished botting autographs!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		backgroundWorker3.CancelAsync();
	}

	private void gunaButton6_Click(object sender, EventArgs e)
	{
		if (backgroundWorker3.IsBusy)
		{
			MessageBox.Show("Your autographs are already being botted... Please wait", "Busy", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		else
		{
			backgroundWorker3.RunWorkerAsync();
		}
	}

	private void gunaImageButton2_Click(object sender, EventArgs e)
	{
		Environment.Exit(0);
		Application.Exit();
	}

	private void gunaImageButton10_Click(object sender, EventArgs e)
	{
		Environment.Exit(0);
		Application.Exit();
	}

	private void gunaImageButton14_Click(object sender, EventArgs e)
	{
		Environment.Exit(0);
		Application.Exit();
	}

	private void gunaImageButton13_Click(object sender, EventArgs e)
	{
		base.WindowState = FormWindowState.Minimized;
	}

	private void gunaImageButton9_Click(object sender, EventArgs e)
	{
		base.WindowState = FormWindowState.Minimized;
	}

	private void gunaImageButton3_Click(object sender, EventArgs e)
	{
		base.WindowState = FormWindowState.Minimized;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FameX.Form2));
		this.gunaElipse1 = new Guna.UI.WinForms.GunaElipse(this.components);
		this.gunaDragControl1 = new Guna.UI.WinForms.GunaDragControl(this.components);
		this.gunaDragControl2 = new Guna.UI.WinForms.GunaDragControl(this.components);
		this.gunaDragControl3 = new Guna.UI.WinForms.GunaDragControl(this.components);
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
		this.vitalityTabControl1 = new VitalityTabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.gunaElipsePanel1 = new Guna.UI.WinForms.GunaElipsePanel();
		this.gunaCheckBox1 = new Guna.UI.WinForms.GunaCheckBox();
		this.gunaLabel10 = new Guna.UI.WinForms.GunaLabel();
		this.gunaLabel9 = new Guna.UI.WinForms.GunaLabel();
		this.gunaLabel7 = new Guna.UI.WinForms.GunaLabel();
		this.gunaButton3 = new Guna.UI.WinForms.GunaButton();
		this.gunaLabel5 = new Guna.UI.WinForms.GunaLabel();
		this.gunaLabel1 = new Guna.UI.WinForms.GunaLabel();
		this.gunaImageButton4 = new Guna.UI.WinForms.GunaImageButton();
		this.gunaButton1 = new Guna.UI.WinForms.GunaButton();
		this.gunaLabel4 = new Guna.UI.WinForms.GunaLabel();
		this.gunaTileButton3 = new Guna.UI.WinForms.GunaTileButton();
		this.gunaTileButton2 = new Guna.UI.WinForms.GunaTileButton();
		this.gunaTileButton1 = new Guna.UI.WinForms.GunaTileButton();
		this.gunaImageButton1 = new Guna.UI.WinForms.GunaImageButton();
		this.gunaLabel3 = new Guna.UI.WinForms.GunaLabel();
		this.gunaImageButton3 = new Guna.UI.WinForms.GunaImageButton();
		this.gunaImageButton2 = new Guna.UI.WinForms.GunaImageButton();
		this.gunaSeparator1 = new Guna.UI.WinForms.GunaSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.gunaElipsePanel2 = new Guna.UI.WinForms.GunaElipsePanel();
		this.gunaGroupBox1 = new Guna.UI.WinForms.GunaGroupBox();
		this.gunaPictureBox1 = new Guna.UI.WinForms.GunaPictureBox();
		this.gunaButton4 = new Guna.UI.WinForms.GunaButton();
		this.gunaButton5 = new Guna.UI.WinForms.GunaButton();
		this.gunaImageButton6 = new Guna.UI.WinForms.GunaImageButton();
		this.gunaImageButton5 = new Guna.UI.WinForms.GunaImageButton();
		this.gunaLabel11 = new Guna.UI.WinForms.GunaLabel();
		this.gunaLabel12 = new Guna.UI.WinForms.GunaLabel();
		this.gunaLabel15 = new Guna.UI.WinForms.GunaLabel();
		this.gunaImageButton7 = new Guna.UI.WinForms.GunaImageButton();
		this.gunaLabel16 = new Guna.UI.WinForms.GunaLabel();
		this.gunaTileButton4 = new Guna.UI.WinForms.GunaTileButton();
		this.gunaTileButton5 = new Guna.UI.WinForms.GunaTileButton();
		this.gunaTileButton6 = new Guna.UI.WinForms.GunaTileButton();
		this.gunaImageButton8 = new Guna.UI.WinForms.GunaImageButton();
		this.gunaLabel17 = new Guna.UI.WinForms.GunaLabel();
		this.gunaImageButton9 = new Guna.UI.WinForms.GunaImageButton();
		this.gunaImageButton10 = new Guna.UI.WinForms.GunaImageButton();
		this.gunaSeparator2 = new Guna.UI.WinForms.GunaSeparator();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.gunaElipsePanel3 = new Guna.UI.WinForms.GunaElipsePanel();
		this.gunaLabel18 = new Guna.UI.WinForms.GunaLabel();
		this.gunaLabel19 = new Guna.UI.WinForms.GunaLabel();
		this.gunaLabel20 = new Guna.UI.WinForms.GunaLabel();
		this.gunaImageButton11 = new Guna.UI.WinForms.GunaImageButton();
		this.gunaButton6 = new Guna.UI.WinForms.GunaButton();
		this.gunaLabel21 = new Guna.UI.WinForms.GunaLabel();
		this.gunaTileButton7 = new Guna.UI.WinForms.GunaTileButton();
		this.gunaTileButton8 = new Guna.UI.WinForms.GunaTileButton();
		this.gunaTileButton9 = new Guna.UI.WinForms.GunaTileButton();
		this.gunaImageButton12 = new Guna.UI.WinForms.GunaImageButton();
		this.gunaLabel22 = new Guna.UI.WinForms.GunaLabel();
		this.gunaImageButton13 = new Guna.UI.WinForms.GunaImageButton();
		this.gunaImageButton14 = new Guna.UI.WinForms.GunaImageButton();
		this.gunaSeparator3 = new Guna.UI.WinForms.GunaSeparator();
		this.vitalityTabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.gunaElipsePanel1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.gunaElipsePanel2.SuspendLayout();
		this.gunaGroupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gunaPictureBox1).BeginInit();
		this.tabPage3.SuspendLayout();
		this.gunaElipsePanel3.SuspendLayout();
		base.SuspendLayout();
		this.gunaElipse1.TargetControl = this;
		this.gunaDragControl1.TargetControl = this.gunaElipsePanel1;
		this.gunaDragControl2.TargetControl = this.gunaElipsePanel2;
		this.gunaDragControl3.TargetControl = this.gunaElipsePanel3;
		this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker1_DoWork);
		this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker2_DoWork);
		this.backgroundWorker3.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker3_DoWork);
		this.vitalityTabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.vitalityTabControl1.Controls.Add(this.tabPage1);
		this.vitalityTabControl1.Controls.Add(this.tabPage2);
		this.vitalityTabControl1.Controls.Add(this.tabPage3);
		this.vitalityTabControl1.Location = new System.Drawing.Point(-5, -5);
		this.vitalityTabControl1.Name = "vitalityTabControl1";
		this.vitalityTabControl1.SelectedIndex = 0;
		this.vitalityTabControl1.Size = new System.Drawing.Size(480, 358);
		this.vitalityTabControl1.TabIndex = 0;
		this.tabPage1.BackColor = System.Drawing.Color.FromArgb(21, 21, 21);
		this.tabPage1.Controls.Add(this.gunaElipsePanel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(472, 329);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "tabPage1";
		this.gunaElipsePanel1.BackColor = System.Drawing.Color.Transparent;
		this.gunaElipsePanel1.BaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaElipsePanel1.Controls.Add(this.gunaCheckBox1);
		this.gunaElipsePanel1.Controls.Add(this.gunaLabel10);
		this.gunaElipsePanel1.Controls.Add(this.gunaLabel9);
		this.gunaElipsePanel1.Controls.Add(this.gunaLabel7);
		this.gunaElipsePanel1.Controls.Add(this.gunaButton3);
		this.gunaElipsePanel1.Controls.Add(this.gunaLabel5);
		this.gunaElipsePanel1.Controls.Add(this.gunaLabel1);
		this.gunaElipsePanel1.Controls.Add(this.gunaImageButton4);
		this.gunaElipsePanel1.Controls.Add(this.gunaButton1);
		this.gunaElipsePanel1.Controls.Add(this.gunaLabel4);
		this.gunaElipsePanel1.Controls.Add(this.gunaTileButton3);
		this.gunaElipsePanel1.Controls.Add(this.gunaTileButton2);
		this.gunaElipsePanel1.Controls.Add(this.gunaTileButton1);
		this.gunaElipsePanel1.Controls.Add(this.gunaImageButton1);
		this.gunaElipsePanel1.Controls.Add(this.gunaLabel3);
		this.gunaElipsePanel1.Controls.Add(this.gunaImageButton3);
		this.gunaElipsePanel1.Controls.Add(this.gunaImageButton2);
		this.gunaElipsePanel1.Controls.Add(this.gunaSeparator1);
		this.gunaElipsePanel1.Location = new System.Drawing.Point(6, 6);
		this.gunaElipsePanel1.Name = "gunaElipsePanel1";
		this.gunaElipsePanel1.Radius = 3;
		this.gunaElipsePanel1.Size = new System.Drawing.Size(460, 304);
		this.gunaElipsePanel1.TabIndex = 1;
		this.gunaCheckBox1.BaseColor = System.Drawing.Color.White;
		this.gunaCheckBox1.CheckedOffColor = System.Drawing.Color.Gray;
		this.gunaCheckBox1.CheckedOnColor = System.Drawing.Color.FromArgb(100, 88, 255);
		this.gunaCheckBox1.FillColor = System.Drawing.Color.White;
		this.gunaCheckBox1.ForeColor = System.Drawing.Color.Silver;
		this.gunaCheckBox1.Location = new System.Drawing.Point(139, 142);
		this.gunaCheckBox1.Name = "gunaCheckBox1";
		this.gunaCheckBox1.Size = new System.Drawing.Size(183, 20);
		this.gunaCheckBox1.TabIndex = 67;
		this.gunaCheckBox1.Text = "Slow Mode (Prevents IP Bans)";
		this.gunaLabel10.AutoSize = true;
		this.gunaLabel10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaLabel10.ForeColor = System.Drawing.Color.White;
		this.gunaLabel10.Location = new System.Drawing.Point(264, 223);
		this.gunaLabel10.Name = "gunaLabel10";
		this.gunaLabel10.Size = new System.Drawing.Size(66, 15);
		this.gunaLabel10.TabIndex = 32;
		this.gunaLabel10.Text = "0000/0000";
		this.gunaLabel9.AutoSize = true;
		this.gunaLabel9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaLabel9.ForeColor = System.Drawing.Color.Silver;
		this.gunaLabel9.Location = new System.Drawing.Point(258, 205);
		this.gunaLabel9.Name = "gunaLabel9";
		this.gunaLabel9.Size = new System.Drawing.Size(78, 15);
		this.gunaLabel9.TabIndex = 31;
		this.gunaLabel9.Text = "Bots Verified:";
		this.gunaLabel7.AutoSize = true;
		this.gunaLabel7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaLabel7.ForeColor = System.Drawing.Color.Silver;
		this.gunaLabel7.Location = new System.Drawing.Point(117, 205);
		this.gunaLabel7.Name = "gunaLabel7";
		this.gunaLabel7.Size = new System.Drawing.Size(79, 15);
		this.gunaLabel7.TabIndex = 29;
		this.gunaLabel7.Text = "Bots Loaded:";
		this.gunaButton3.Animated = true;
		this.gunaButton3.AnimationHoverSpeed = 0.07f;
		this.gunaButton3.AnimationSpeed = 0.03f;
		this.gunaButton3.BackColor = System.Drawing.Color.Transparent;
		this.gunaButton3.BaseColor = System.Drawing.Color.FromArgb(18, 18, 18);
		this.gunaButton3.BorderColor = System.Drawing.Color.FromArgb(18, 18, 18);
		this.gunaButton3.BorderSize = 2;
		this.gunaButton3.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaButton3.FocusedColor = System.Drawing.Color.Empty;
		this.gunaButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaButton3.ForeColor = System.Drawing.Color.Silver;
		this.gunaButton3.Image = (System.Drawing.Image)resources.GetObject("gunaButton3.Image");
		this.gunaButton3.ImageSize = new System.Drawing.Size(30, 30);
		this.gunaButton3.Location = new System.Drawing.Point(228, 168);
		this.gunaButton3.Name = "gunaButton3";
		this.gunaButton3.OnHoverBaseColor = System.Drawing.Color.FromArgb(30, 30, 30);
		this.gunaButton3.OnHoverBorderColor = System.Drawing.Color.Empty;
		this.gunaButton3.OnHoverForeColor = System.Drawing.Color.White;
		this.gunaButton3.OnHoverImage = (System.Drawing.Image)resources.GetObject("gunaButton3.OnHoverImage");
		this.gunaButton3.OnPressedColor = System.Drawing.Color.Black;
		this.gunaButton3.Radius = 2;
		this.gunaButton3.Size = new System.Drawing.Size(135, 35);
		this.gunaButton3.TabIndex = 28;
		this.gunaButton3.Text = "VERIFY BOTS";
		this.gunaButton3.Click += new System.EventHandler(gunaButton3_Click);
		this.gunaLabel5.AutoSize = true;
		this.gunaLabel5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaLabel5.ForeColor = System.Drawing.Color.White;
		this.gunaLabel5.Location = new System.Drawing.Point(124, 224);
		this.gunaLabel5.Name = "gunaLabel5";
		this.gunaLabel5.Size = new System.Drawing.Size(66, 15);
		this.gunaLabel5.TabIndex = 25;
		this.gunaLabel5.Text = "0000/0000";
		this.gunaLabel1.AutoSize = true;
		this.gunaLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaLabel1.ForeColor = System.Drawing.Color.White;
		this.gunaLabel1.Location = new System.Drawing.Point(189, 112);
		this.gunaLabel1.Name = "gunaLabel1";
		this.gunaLabel1.Size = new System.Drawing.Size(80, 29);
		this.gunaLabel1.TabIndex = 23;
		this.gunaLabel1.Text = "BOTS";
		this.gunaImageButton4.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaImageButton4.Image = (System.Drawing.Image)resources.GetObject("gunaImageButton4.Image");
		this.gunaImageButton4.ImageSize = new System.Drawing.Size(50, 50);
		this.gunaImageButton4.Location = new System.Drawing.Point(203, 64);
		this.gunaImageButton4.Name = "gunaImageButton4";
		this.gunaImageButton4.OnHoverImage = (System.Drawing.Image)resources.GetObject("gunaImageButton4.OnHoverImage");
		this.gunaImageButton4.OnHoverImageOffset = new System.Drawing.Point(0, 0);
		this.gunaImageButton4.Size = new System.Drawing.Size(49, 56);
		this.gunaImageButton4.TabIndex = 22;
		this.gunaButton1.Animated = true;
		this.gunaButton1.AnimationHoverSpeed = 0.07f;
		this.gunaButton1.AnimationSpeed = 0.03f;
		this.gunaButton1.BackColor = System.Drawing.Color.Transparent;
		this.gunaButton1.BaseColor = System.Drawing.Color.FromArgb(18, 18, 18);
		this.gunaButton1.BorderColor = System.Drawing.Color.FromArgb(18, 18, 18);
		this.gunaButton1.BorderSize = 2;
		this.gunaButton1.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaButton1.FocusedColor = System.Drawing.Color.Empty;
		this.gunaButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaButton1.ForeColor = System.Drawing.Color.Silver;
		this.gunaButton1.Image = (System.Drawing.Image)resources.GetObject("gunaButton1.Image");
		this.gunaButton1.ImageSize = new System.Drawing.Size(30, 30);
		this.gunaButton1.Location = new System.Drawing.Point(89, 168);
		this.gunaButton1.Name = "gunaButton1";
		this.gunaButton1.OnHoverBaseColor = System.Drawing.Color.FromArgb(30, 30, 30);
		this.gunaButton1.OnHoverBorderColor = System.Drawing.Color.Empty;
		this.gunaButton1.OnHoverForeColor = System.Drawing.Color.White;
		this.gunaButton1.OnHoverImage = (System.Drawing.Image)resources.GetObject("gunaButton1.OnHoverImage");
		this.gunaButton1.OnPressedColor = System.Drawing.Color.Black;
		this.gunaButton1.Radius = 2;
		this.gunaButton1.Size = new System.Drawing.Size(135, 35);
		this.gunaButton1.TabIndex = 21;
		this.gunaButton1.Text = "IMPORT BOTS";
		this.gunaButton1.Click += new System.EventHandler(gunaButton1_Click);
		this.gunaLabel4.AutoSize = true;
		this.gunaLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaLabel4.ForeColor = System.Drawing.Color.Silver;
		this.gunaLabel4.Location = new System.Drawing.Point(29, 7);
		this.gunaLabel4.Name = "gunaLabel4";
		this.gunaLabel4.Size = new System.Drawing.Size(56, 18);
		this.gunaLabel4.TabIndex = 16;
		this.gunaLabel4.Text = "FameX";
		this.gunaTileButton3.AnimationHoverSpeed = 0.07f;
		this.gunaTileButton3.AnimationSpeed = 0.03f;
		this.gunaTileButton3.BaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton3.BorderColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton3.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaTileButton3.FocusedColor = System.Drawing.Color.Empty;
		this.gunaTileButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaTileButton3.ForeColor = System.Drawing.Color.Silver;
		this.gunaTileButton3.Image = null;
		this.gunaTileButton3.ImageSize = new System.Drawing.Size(52, 52);
		this.gunaTileButton3.Location = new System.Drawing.Point(260, 8);
		this.gunaTileButton3.Name = "gunaTileButton3";
		this.gunaTileButton3.OnHoverBaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton3.OnHoverBorderColor = System.Drawing.Color.Black;
		this.gunaTileButton3.OnHoverForeColor = System.Drawing.Color.White;
		this.gunaTileButton3.OnHoverImage = null;
		this.gunaTileButton3.OnPressedColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton3.Size = new System.Drawing.Size(112, 21);
		this.gunaTileButton3.TabIndex = 20;
		this.gunaTileButton3.Text = "Autos";
		this.gunaTileButton3.Click += new System.EventHandler(gunaTileButton3_Click);
		this.gunaTileButton2.AnimationHoverSpeed = 0.07f;
		this.gunaTileButton2.AnimationSpeed = 0.03f;
		this.gunaTileButton2.BaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton2.BorderColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton2.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaTileButton2.FocusedColor = System.Drawing.Color.Empty;
		this.gunaTileButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaTileButton2.ForeColor = System.Drawing.Color.Silver;
		this.gunaTileButton2.Image = null;
		this.gunaTileButton2.ImageSize = new System.Drawing.Size(52, 52);
		this.gunaTileButton2.Location = new System.Drawing.Point(174, 8);
		this.gunaTileButton2.Name = "gunaTileButton2";
		this.gunaTileButton2.OnHoverBaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton2.OnHoverBorderColor = System.Drawing.Color.Black;
		this.gunaTileButton2.OnHoverForeColor = System.Drawing.Color.White;
		this.gunaTileButton2.OnHoverImage = null;
		this.gunaTileButton2.OnPressedColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton2.Size = new System.Drawing.Size(112, 21);
		this.gunaTileButton2.TabIndex = 19;
		this.gunaTileButton2.Text = "Movies";
		this.gunaTileButton2.Click += new System.EventHandler(gunaTileButton2_Click);
		this.gunaTileButton1.AnimationHoverSpeed = 0.07f;
		this.gunaTileButton1.AnimationSpeed = 0.03f;
		this.gunaTileButton1.BaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton1.BorderColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton1.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaTileButton1.FocusedColor = System.Drawing.Color.Empty;
		this.gunaTileButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaTileButton1.ForeColor = System.Drawing.Color.Silver;
		this.gunaTileButton1.Image = null;
		this.gunaTileButton1.ImageSize = new System.Drawing.Size(52, 52);
		this.gunaTileButton1.Location = new System.Drawing.Point(90, 8);
		this.gunaTileButton1.Name = "gunaTileButton1";
		this.gunaTileButton1.OnHoverBaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton1.OnHoverBorderColor = System.Drawing.Color.Black;
		this.gunaTileButton1.OnHoverForeColor = System.Drawing.Color.White;
		this.gunaTileButton1.OnHoverImage = null;
		this.gunaTileButton1.OnPressedColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton1.Size = new System.Drawing.Size(112, 21);
		this.gunaTileButton1.TabIndex = 18;
		this.gunaTileButton1.Text = "Bots";
		this.gunaTileButton1.Click += new System.EventHandler(gunaTileButton1_Click);
		this.gunaImageButton1.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaImageButton1.Image = (System.Drawing.Image)resources.GetObject("gunaImageButton1.Image");
		this.gunaImageButton1.ImageSize = new System.Drawing.Size(20, 20);
		this.gunaImageButton1.Location = new System.Drawing.Point(6, 7);
		this.gunaImageButton1.Name = "gunaImageButton1";
		this.gunaImageButton1.OnHoverImage = (System.Drawing.Image)resources.GetObject("gunaImageButton1.OnHoverImage");
		this.gunaImageButton1.OnHoverImageOffset = new System.Drawing.Point(0, 0);
		this.gunaImageButton1.Size = new System.Drawing.Size(23, 21);
		this.gunaImageButton1.TabIndex = 17;
		this.gunaLabel3.AutoSize = true;
		this.gunaLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaLabel3.ForeColor = System.Drawing.Color.FromArgb(34, 34, 34);
		this.gunaLabel3.Location = new System.Drawing.Point(170, 284);
		this.gunaLabel3.Name = "gunaLabel3";
		this.gunaLabel3.Size = new System.Drawing.Size(110, 13);
		this.gunaLabel3.TabIndex = 15;
		this.gunaLabel3.Text = "Loc#9666 vida#6666";
		this.gunaImageButton3.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaImageButton3.Image = (System.Drawing.Image)resources.GetObject("gunaImageButton3.Image");
		this.gunaImageButton3.ImageSize = new System.Drawing.Size(20, 20);
		this.gunaImageButton3.Location = new System.Drawing.Point(414, 7);
		this.gunaImageButton3.Name = "gunaImageButton3";
		this.gunaImageButton3.OnHoverImage = (System.Drawing.Image)resources.GetObject("gunaImageButton3.OnHoverImage");
		this.gunaImageButton3.OnHoverImageOffset = new System.Drawing.Point(0, 0);
		this.gunaImageButton3.Size = new System.Drawing.Size(20, 20);
		this.gunaImageButton3.TabIndex = 14;
		this.gunaImageButton3.Click += new System.EventHandler(gunaImageButton3_Click);
		this.gunaImageButton2.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaImageButton2.Image = (System.Drawing.Image)resources.GetObject("gunaImageButton2.Image");
		this.gunaImageButton2.ImageSize = new System.Drawing.Size(20, 20);
		this.gunaImageButton2.Location = new System.Drawing.Point(435, 7);
		this.gunaImageButton2.Name = "gunaImageButton2";
		this.gunaImageButton2.OnHoverImage = (System.Drawing.Image)resources.GetObject("gunaImageButton2.OnHoverImage");
		this.gunaImageButton2.OnHoverImageOffset = new System.Drawing.Point(0, 0);
		this.gunaImageButton2.Size = new System.Drawing.Size(20, 20);
		this.gunaImageButton2.TabIndex = 13;
		this.gunaImageButton2.Click += new System.EventHandler(gunaImageButton2_Click);
		this.gunaSeparator1.LineColor = System.Drawing.Color.FromArgb(197, 197, 197);
		this.gunaSeparator1.Location = new System.Drawing.Point(8, 30);
		this.gunaSeparator1.Name = "gunaSeparator1";
		this.gunaSeparator1.Size = new System.Drawing.Size(445, 10);
		this.gunaSeparator1.TabIndex = 0;
		this.tabPage2.BackColor = System.Drawing.Color.FromArgb(21, 21, 21);
		this.tabPage2.Controls.Add(this.gunaElipsePanel2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(472, 329);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "tabPage2";
		this.gunaElipsePanel2.BackColor = System.Drawing.Color.Transparent;
		this.gunaElipsePanel2.BaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaElipsePanel2.Controls.Add(this.gunaGroupBox1);
		this.gunaElipsePanel2.Controls.Add(this.gunaLabel15);
		this.gunaElipsePanel2.Controls.Add(this.gunaImageButton7);
		this.gunaElipsePanel2.Controls.Add(this.gunaLabel16);
		this.gunaElipsePanel2.Controls.Add(this.gunaTileButton4);
		this.gunaElipsePanel2.Controls.Add(this.gunaTileButton5);
		this.gunaElipsePanel2.Controls.Add(this.gunaTileButton6);
		this.gunaElipsePanel2.Controls.Add(this.gunaImageButton8);
		this.gunaElipsePanel2.Controls.Add(this.gunaLabel17);
		this.gunaElipsePanel2.Controls.Add(this.gunaImageButton9);
		this.gunaElipsePanel2.Controls.Add(this.gunaImageButton10);
		this.gunaElipsePanel2.Controls.Add(this.gunaSeparator2);
		this.gunaElipsePanel2.Location = new System.Drawing.Point(6, 6);
		this.gunaElipsePanel2.Name = "gunaElipsePanel2";
		this.gunaElipsePanel2.Radius = 3;
		this.gunaElipsePanel2.Size = new System.Drawing.Size(460, 304);
		this.gunaElipsePanel2.TabIndex = 1;
		this.gunaGroupBox1.BackColor = System.Drawing.Color.Transparent;
		this.gunaGroupBox1.BaseColor = System.Drawing.Color.FromArgb(16, 16, 16);
		this.gunaGroupBox1.BorderColor = System.Drawing.Color.FromArgb(10, 10, 10);
		this.gunaGroupBox1.Controls.Add(this.gunaPictureBox1);
		this.gunaGroupBox1.Controls.Add(this.gunaButton4);
		this.gunaGroupBox1.Controls.Add(this.gunaButton5);
		this.gunaGroupBox1.Controls.Add(this.gunaImageButton6);
		this.gunaGroupBox1.Controls.Add(this.gunaImageButton5);
		this.gunaGroupBox1.Controls.Add(this.gunaLabel11);
		this.gunaGroupBox1.Controls.Add(this.gunaLabel12);
		this.gunaGroupBox1.ForeColor = System.Drawing.Color.White;
		this.gunaGroupBox1.LineColor = System.Drawing.Color.FromArgb(10, 10, 10);
		this.gunaGroupBox1.Location = new System.Drawing.Point(8, 135);
		this.gunaGroupBox1.Name = "gunaGroupBox1";
		this.gunaGroupBox1.Radius = 3;
		this.gunaGroupBox1.Size = new System.Drawing.Size(443, 145);
		this.gunaGroupBox1.TabIndex = 56;
		this.gunaGroupBox1.Text = "Current Movie";
		this.gunaGroupBox1.TextLocation = new System.Drawing.Point(10, 8);
		this.gunaPictureBox1.BaseColor = System.Drawing.Color.White;
		this.gunaPictureBox1.Image = (System.Drawing.Image)resources.GetObject("gunaPictureBox1.Image");
		this.gunaPictureBox1.Location = new System.Drawing.Point(11, 41);
		this.gunaPictureBox1.Name = "gunaPictureBox1";
		this.gunaPictureBox1.Size = new System.Drawing.Size(161, 88);
		this.gunaPictureBox1.TabIndex = 65;
		this.gunaPictureBox1.TabStop = false;
		this.gunaButton4.Animated = true;
		this.gunaButton4.AnimationHoverSpeed = 0.07f;
		this.gunaButton4.AnimationSpeed = 0.03f;
		this.gunaButton4.BackColor = System.Drawing.Color.Transparent;
		this.gunaButton4.BaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaButton4.BorderColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaButton4.BorderSize = 2;
		this.gunaButton4.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaButton4.FocusedColor = System.Drawing.Color.Empty;
		this.gunaButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaButton4.ForeColor = System.Drawing.Color.Silver;
		this.gunaButton4.Image = (System.Drawing.Image)resources.GetObject("gunaButton4.Image");
		this.gunaButton4.ImageSize = new System.Drawing.Size(25, 25);
		this.gunaButton4.Location = new System.Drawing.Point(178, 74);
		this.gunaButton4.Name = "gunaButton4";
		this.gunaButton4.OnHoverBaseColor = System.Drawing.Color.FromArgb(30, 30, 30);
		this.gunaButton4.OnHoverBorderColor = System.Drawing.Color.Empty;
		this.gunaButton4.OnHoverForeColor = System.Drawing.Color.White;
		this.gunaButton4.OnHoverImage = (System.Drawing.Image)resources.GetObject("gunaButton4.OnHoverImage");
		this.gunaButton4.OnPressedColor = System.Drawing.Color.Black;
		this.gunaButton4.Radius = 2;
		this.gunaButton4.Size = new System.Drawing.Size(262, 29);
		this.gunaButton4.TabIndex = 64;
		this.gunaButton4.Text = "BOT MOVIE";
		this.gunaButton4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.gunaButton4.Click += new System.EventHandler(gunaButton4_Click);
		this.gunaButton5.Animated = true;
		this.gunaButton5.AnimationHoverSpeed = 0.07f;
		this.gunaButton5.AnimationSpeed = 0.03f;
		this.gunaButton5.BackColor = System.Drawing.Color.Transparent;
		this.gunaButton5.BaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaButton5.BorderColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaButton5.BorderSize = 2;
		this.gunaButton5.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaButton5.FocusedColor = System.Drawing.Color.Empty;
		this.gunaButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaButton5.ForeColor = System.Drawing.Color.Silver;
		this.gunaButton5.Image = (System.Drawing.Image)resources.GetObject("gunaButton5.Image");
		this.gunaButton5.ImageSize = new System.Drawing.Size(25, 25);
		this.gunaButton5.Location = new System.Drawing.Point(178, 41);
		this.gunaButton5.Name = "gunaButton5";
		this.gunaButton5.OnHoverBaseColor = System.Drawing.Color.FromArgb(30, 30, 30);
		this.gunaButton5.OnHoverBorderColor = System.Drawing.Color.Empty;
		this.gunaButton5.OnHoverForeColor = System.Drawing.Color.White;
		this.gunaButton5.OnHoverImage = (System.Drawing.Image)resources.GetObject("gunaButton5.OnHoverImage");
		this.gunaButton5.OnPressedColor = System.Drawing.Color.Black;
		this.gunaButton5.Radius = 2;
		this.gunaButton5.Size = new System.Drawing.Size(262, 29);
		this.gunaButton5.TabIndex = 63;
		this.gunaButton5.Text = "CREATE MOVIE";
		this.gunaButton5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.gunaButton5.Click += new System.EventHandler(gunaButton5_Click);
		this.gunaImageButton6.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaImageButton6.Image = (System.Drawing.Image)resources.GetObject("gunaImageButton6.Image");
		this.gunaImageButton6.ImageSize = new System.Drawing.Size(20, 20);
		this.gunaImageButton6.Location = new System.Drawing.Point(288, 109);
		this.gunaImageButton6.Name = "gunaImageButton6";
		this.gunaImageButton6.OnHoverImage = (System.Drawing.Image)resources.GetObject("gunaImageButton6.OnHoverImage");
		this.gunaImageButton6.OnHoverImageOffset = new System.Drawing.Point(0, 0);
		this.gunaImageButton6.Size = new System.Drawing.Size(20, 20);
		this.gunaImageButton6.TabIndex = 62;
		this.gunaImageButton5.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaImageButton5.Image = (System.Drawing.Image)resources.GetObject("gunaImageButton5.Image");
		this.gunaImageButton5.ImageSize = new System.Drawing.Size(20, 20);
		this.gunaImageButton5.Location = new System.Drawing.Point(175, 109);
		this.gunaImageButton5.Name = "gunaImageButton5";
		this.gunaImageButton5.OnHoverImage = (System.Drawing.Image)resources.GetObject("gunaImageButton5.OnHoverImage");
		this.gunaImageButton5.OnHoverImageOffset = new System.Drawing.Point(0, 0);
		this.gunaImageButton5.Size = new System.Drawing.Size(20, 20);
		this.gunaImageButton5.TabIndex = 57;
		this.gunaLabel11.AutoSize = true;
		this.gunaLabel11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaLabel11.ForeColor = System.Drawing.Color.White;
		this.gunaLabel11.Location = new System.Drawing.Point(310, 111);
		this.gunaLabel11.Name = "gunaLabel11";
		this.gunaLabel11.Size = new System.Drawing.Size(14, 15);
		this.gunaLabel11.TabIndex = 61;
		this.gunaLabel11.Text = "0";
		this.gunaLabel12.AutoSize = true;
		this.gunaLabel12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaLabel12.ForeColor = System.Drawing.Color.White;
		this.gunaLabel12.Location = new System.Drawing.Point(197, 111);
		this.gunaLabel12.Name = "gunaLabel12";
		this.gunaLabel12.Size = new System.Drawing.Size(14, 15);
		this.gunaLabel12.TabIndex = 60;
		this.gunaLabel12.Text = "0";
		this.gunaLabel15.AutoSize = true;
		this.gunaLabel15.Font = new System.Drawing.Font("Microsoft Sans Serif", 18f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaLabel15.ForeColor = System.Drawing.Color.White;
		this.gunaLabel15.Location = new System.Drawing.Point(178, 103);
		this.gunaLabel15.Name = "gunaLabel15";
		this.gunaLabel15.Size = new System.Drawing.Size(105, 29);
		this.gunaLabel15.TabIndex = 23;
		this.gunaLabel15.Text = "MOVIES";
		this.gunaImageButton7.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaImageButton7.Image = (System.Drawing.Image)resources.GetObject("gunaImageButton7.Image");
		this.gunaImageButton7.ImageSize = new System.Drawing.Size(50, 50);
		this.gunaImageButton7.Location = new System.Drawing.Point(206, 54);
		this.gunaImageButton7.Name = "gunaImageButton7";
		this.gunaImageButton7.OnHoverImage = (System.Drawing.Image)resources.GetObject("gunaImageButton7.OnHoverImage");
		this.gunaImageButton7.OnHoverImageOffset = new System.Drawing.Point(0, 0);
		this.gunaImageButton7.Size = new System.Drawing.Size(49, 56);
		this.gunaImageButton7.TabIndex = 22;
		this.gunaLabel16.AutoSize = true;
		this.gunaLabel16.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaLabel16.ForeColor = System.Drawing.Color.Silver;
		this.gunaLabel16.Location = new System.Drawing.Point(29, 7);
		this.gunaLabel16.Name = "gunaLabel16";
		this.gunaLabel16.Size = new System.Drawing.Size(56, 18);
		this.gunaLabel16.TabIndex = 16;
		this.gunaLabel16.Text = "FameX";
		this.gunaTileButton4.AnimationHoverSpeed = 0.07f;
		this.gunaTileButton4.AnimationSpeed = 0.03f;
		this.gunaTileButton4.BaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton4.BorderColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton4.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaTileButton4.FocusedColor = System.Drawing.Color.Empty;
		this.gunaTileButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaTileButton4.ForeColor = System.Drawing.Color.Silver;
		this.gunaTileButton4.Image = null;
		this.gunaTileButton4.ImageSize = new System.Drawing.Size(52, 52);
		this.gunaTileButton4.Location = new System.Drawing.Point(260, 8);
		this.gunaTileButton4.Name = "gunaTileButton4";
		this.gunaTileButton4.OnHoverBaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton4.OnHoverBorderColor = System.Drawing.Color.Black;
		this.gunaTileButton4.OnHoverForeColor = System.Drawing.Color.White;
		this.gunaTileButton4.OnHoverImage = null;
		this.gunaTileButton4.OnPressedColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton4.Size = new System.Drawing.Size(112, 21);
		this.gunaTileButton4.TabIndex = 20;
		this.gunaTileButton4.Text = "Autos";
		this.gunaTileButton4.Click += new System.EventHandler(gunaTileButton4_Click);
		this.gunaTileButton5.AnimationHoverSpeed = 0.07f;
		this.gunaTileButton5.AnimationSpeed = 0.03f;
		this.gunaTileButton5.BaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton5.BorderColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton5.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaTileButton5.FocusedColor = System.Drawing.Color.Empty;
		this.gunaTileButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaTileButton5.ForeColor = System.Drawing.Color.Silver;
		this.gunaTileButton5.Image = null;
		this.gunaTileButton5.ImageSize = new System.Drawing.Size(52, 52);
		this.gunaTileButton5.Location = new System.Drawing.Point(174, 8);
		this.gunaTileButton5.Name = "gunaTileButton5";
		this.gunaTileButton5.OnHoverBaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton5.OnHoverBorderColor = System.Drawing.Color.Black;
		this.gunaTileButton5.OnHoverForeColor = System.Drawing.Color.White;
		this.gunaTileButton5.OnHoverImage = null;
		this.gunaTileButton5.OnPressedColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton5.Size = new System.Drawing.Size(112, 21);
		this.gunaTileButton5.TabIndex = 19;
		this.gunaTileButton5.Text = "Movies";
		this.gunaTileButton5.Click += new System.EventHandler(gunaTileButton5_Click);
		this.gunaTileButton6.AnimationHoverSpeed = 0.07f;
		this.gunaTileButton6.AnimationSpeed = 0.03f;
		this.gunaTileButton6.BaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton6.BorderColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton6.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaTileButton6.FocusedColor = System.Drawing.Color.Empty;
		this.gunaTileButton6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaTileButton6.ForeColor = System.Drawing.Color.Silver;
		this.gunaTileButton6.Image = null;
		this.gunaTileButton6.ImageSize = new System.Drawing.Size(52, 52);
		this.gunaTileButton6.Location = new System.Drawing.Point(90, 8);
		this.gunaTileButton6.Name = "gunaTileButton6";
		this.gunaTileButton6.OnHoverBaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton6.OnHoverBorderColor = System.Drawing.Color.Black;
		this.gunaTileButton6.OnHoverForeColor = System.Drawing.Color.White;
		this.gunaTileButton6.OnHoverImage = null;
		this.gunaTileButton6.OnPressedColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton6.Size = new System.Drawing.Size(112, 21);
		this.gunaTileButton6.TabIndex = 18;
		this.gunaTileButton6.Text = "Bots";
		this.gunaTileButton6.Click += new System.EventHandler(gunaTileButton6_Click);
		this.gunaImageButton8.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaImageButton8.Image = (System.Drawing.Image)resources.GetObject("gunaImageButton8.Image");
		this.gunaImageButton8.ImageSize = new System.Drawing.Size(20, 20);
		this.gunaImageButton8.Location = new System.Drawing.Point(6, 7);
		this.gunaImageButton8.Name = "gunaImageButton8";
		this.gunaImageButton8.OnHoverImage = (System.Drawing.Image)resources.GetObject("gunaImageButton8.OnHoverImage");
		this.gunaImageButton8.OnHoverImageOffset = new System.Drawing.Point(0, 0);
		this.gunaImageButton8.Size = new System.Drawing.Size(23, 21);
		this.gunaImageButton8.TabIndex = 17;
		this.gunaLabel17.AutoSize = true;
		this.gunaLabel17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaLabel17.ForeColor = System.Drawing.Color.FromArgb(34, 34, 34);
		this.gunaLabel17.Location = new System.Drawing.Point(170, 284);
		this.gunaLabel17.Name = "gunaLabel17";
		this.gunaLabel17.Size = new System.Drawing.Size(110, 13);
		this.gunaLabel17.TabIndex = 15;
		this.gunaLabel17.Text = "Loc#9666 vida#6666";
		this.gunaImageButton9.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaImageButton9.Image = (System.Drawing.Image)resources.GetObject("gunaImageButton9.Image");
		this.gunaImageButton9.ImageSize = new System.Drawing.Size(20, 20);
		this.gunaImageButton9.Location = new System.Drawing.Point(414, 7);
		this.gunaImageButton9.Name = "gunaImageButton9";
		this.gunaImageButton9.OnHoverImage = (System.Drawing.Image)resources.GetObject("gunaImageButton9.OnHoverImage");
		this.gunaImageButton9.OnHoverImageOffset = new System.Drawing.Point(0, 0);
		this.gunaImageButton9.Size = new System.Drawing.Size(20, 20);
		this.gunaImageButton9.TabIndex = 14;
		this.gunaImageButton9.Click += new System.EventHandler(gunaImageButton9_Click);
		this.gunaImageButton10.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaImageButton10.Image = (System.Drawing.Image)resources.GetObject("gunaImageButton10.Image");
		this.gunaImageButton10.ImageSize = new System.Drawing.Size(20, 20);
		this.gunaImageButton10.Location = new System.Drawing.Point(435, 7);
		this.gunaImageButton10.Name = "gunaImageButton10";
		this.gunaImageButton10.OnHoverImage = (System.Drawing.Image)resources.GetObject("gunaImageButton10.OnHoverImage");
		this.gunaImageButton10.OnHoverImageOffset = new System.Drawing.Point(0, 0);
		this.gunaImageButton10.Size = new System.Drawing.Size(20, 20);
		this.gunaImageButton10.TabIndex = 13;
		this.gunaImageButton10.Click += new System.EventHandler(gunaImageButton10_Click);
		this.gunaSeparator2.LineColor = System.Drawing.Color.FromArgb(197, 197, 197);
		this.gunaSeparator2.Location = new System.Drawing.Point(8, 30);
		this.gunaSeparator2.Name = "gunaSeparator2";
		this.gunaSeparator2.Size = new System.Drawing.Size(445, 10);
		this.gunaSeparator2.TabIndex = 0;
		this.tabPage3.BackColor = System.Drawing.Color.FromArgb(21, 21, 21);
		this.tabPage3.Controls.Add(this.gunaElipsePanel3);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(472, 329);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "tabPage3";
		this.gunaElipsePanel3.BackColor = System.Drawing.Color.Transparent;
		this.gunaElipsePanel3.BaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaElipsePanel3.Controls.Add(this.gunaLabel18);
		this.gunaElipsePanel3.Controls.Add(this.gunaLabel19);
		this.gunaElipsePanel3.Controls.Add(this.gunaLabel20);
		this.gunaElipsePanel3.Controls.Add(this.gunaImageButton11);
		this.gunaElipsePanel3.Controls.Add(this.gunaButton6);
		this.gunaElipsePanel3.Controls.Add(this.gunaLabel21);
		this.gunaElipsePanel3.Controls.Add(this.gunaTileButton7);
		this.gunaElipsePanel3.Controls.Add(this.gunaTileButton8);
		this.gunaElipsePanel3.Controls.Add(this.gunaTileButton9);
		this.gunaElipsePanel3.Controls.Add(this.gunaImageButton12);
		this.gunaElipsePanel3.Controls.Add(this.gunaLabel22);
		this.gunaElipsePanel3.Controls.Add(this.gunaImageButton13);
		this.gunaElipsePanel3.Controls.Add(this.gunaImageButton14);
		this.gunaElipsePanel3.Controls.Add(this.gunaSeparator3);
		this.gunaElipsePanel3.Location = new System.Drawing.Point(6, 6);
		this.gunaElipsePanel3.Name = "gunaElipsePanel3";
		this.gunaElipsePanel3.Radius = 3;
		this.gunaElipsePanel3.Size = new System.Drawing.Size(460, 304);
		this.gunaElipsePanel3.TabIndex = 1;
		this.gunaLabel18.AutoSize = true;
		this.gunaLabel18.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaLabel18.ForeColor = System.Drawing.Color.White;
		this.gunaLabel18.Location = new System.Drawing.Point(235, 201);
		this.gunaLabel18.Name = "gunaLabel18";
		this.gunaLabel18.Size = new System.Drawing.Size(76, 18);
		this.gunaLabel18.TabIndex = 27;
		this.gunaLabel18.Text = "0000/0000";
		this.gunaLabel19.AutoSize = true;
		this.gunaLabel19.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaLabel19.ForeColor = System.Drawing.Color.Silver;
		this.gunaLabel19.Location = new System.Drawing.Point(145, 201);
		this.gunaLabel19.Name = "gunaLabel19";
		this.gunaLabel19.Size = new System.Drawing.Size(84, 18);
		this.gunaLabel19.TabIndex = 26;
		this.gunaLabel19.Text = "Autos Sent:";
		this.gunaLabel20.AutoSize = true;
		this.gunaLabel20.Font = new System.Drawing.Font("Microsoft Sans Serif", 18f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaLabel20.ForeColor = System.Drawing.Color.White;
		this.gunaLabel20.Location = new System.Drawing.Point(144, 111);
		this.gunaLabel20.Name = "gunaLabel20";
		this.gunaLabel20.Size = new System.Drawing.Size(179, 29);
		this.gunaLabel20.TabIndex = 23;
		this.gunaLabel20.Text = "AUTOGRAPHS";
		this.gunaImageButton11.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaImageButton11.Image = (System.Drawing.Image)resources.GetObject("gunaImageButton11.Image");
		this.gunaImageButton11.ImageSize = new System.Drawing.Size(50, 50);
		this.gunaImageButton11.Location = new System.Drawing.Point(206, 62);
		this.gunaImageButton11.Name = "gunaImageButton11";
		this.gunaImageButton11.OnHoverImage = (System.Drawing.Image)resources.GetObject("gunaImageButton11.OnHoverImage");
		this.gunaImageButton11.OnHoverImageOffset = new System.Drawing.Point(0, 0);
		this.gunaImageButton11.Size = new System.Drawing.Size(49, 56);
		this.gunaImageButton11.TabIndex = 22;
		this.gunaButton6.Animated = true;
		this.gunaButton6.AnimationHoverSpeed = 0.07f;
		this.gunaButton6.AnimationSpeed = 0.03f;
		this.gunaButton6.BackColor = System.Drawing.Color.Transparent;
		this.gunaButton6.BaseColor = System.Drawing.Color.FromArgb(18, 18, 18);
		this.gunaButton6.BorderColor = System.Drawing.Color.FromArgb(18, 18, 18);
		this.gunaButton6.BorderSize = 2;
		this.gunaButton6.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaButton6.FocusedColor = System.Drawing.Color.Empty;
		this.gunaButton6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaButton6.ForeColor = System.Drawing.Color.Silver;
		this.gunaButton6.Image = (System.Drawing.Image)resources.GetObject("gunaButton6.Image");
		this.gunaButton6.ImageSize = new System.Drawing.Size(25, 25);
		this.gunaButton6.Location = new System.Drawing.Point(105, 155);
		this.gunaButton6.Name = "gunaButton6";
		this.gunaButton6.OnHoverBaseColor = System.Drawing.Color.FromArgb(30, 30, 30);
		this.gunaButton6.OnHoverBorderColor = System.Drawing.Color.Empty;
		this.gunaButton6.OnHoverForeColor = System.Drawing.Color.White;
		this.gunaButton6.OnHoverImage = (System.Drawing.Image)resources.GetObject("gunaButton6.OnHoverImage");
		this.gunaButton6.OnPressedColor = System.Drawing.Color.Black;
		this.gunaButton6.Radius = 2;
		this.gunaButton6.Size = new System.Drawing.Size(250, 35);
		this.gunaButton6.TabIndex = 21;
		this.gunaButton6.Text = "BOT AUTOGRAPHS";
		this.gunaButton6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.gunaButton6.Click += new System.EventHandler(gunaButton6_Click);
		this.gunaLabel21.AutoSize = true;
		this.gunaLabel21.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaLabel21.ForeColor = System.Drawing.Color.Silver;
		this.gunaLabel21.Location = new System.Drawing.Point(29, 7);
		this.gunaLabel21.Name = "gunaLabel21";
		this.gunaLabel21.Size = new System.Drawing.Size(56, 18);
		this.gunaLabel21.TabIndex = 16;
		this.gunaLabel21.Text = "FameX";
		this.gunaTileButton7.AnimationHoverSpeed = 0.07f;
		this.gunaTileButton7.AnimationSpeed = 0.03f;
		this.gunaTileButton7.BaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton7.BorderColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton7.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaTileButton7.FocusedColor = System.Drawing.Color.Empty;
		this.gunaTileButton7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaTileButton7.ForeColor = System.Drawing.Color.Silver;
		this.gunaTileButton7.Image = null;
		this.gunaTileButton7.ImageSize = new System.Drawing.Size(52, 52);
		this.gunaTileButton7.Location = new System.Drawing.Point(260, 8);
		this.gunaTileButton7.Name = "gunaTileButton7";
		this.gunaTileButton7.OnHoverBaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton7.OnHoverBorderColor = System.Drawing.Color.Black;
		this.gunaTileButton7.OnHoverForeColor = System.Drawing.Color.White;
		this.gunaTileButton7.OnHoverImage = null;
		this.gunaTileButton7.OnPressedColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton7.Size = new System.Drawing.Size(112, 21);
		this.gunaTileButton7.TabIndex = 20;
		this.gunaTileButton7.Text = "Autos";
		this.gunaTileButton7.Click += new System.EventHandler(gunaTileButton7_Click);
		this.gunaTileButton8.AnimationHoverSpeed = 0.07f;
		this.gunaTileButton8.AnimationSpeed = 0.03f;
		this.gunaTileButton8.BaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton8.BorderColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton8.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaTileButton8.FocusedColor = System.Drawing.Color.Empty;
		this.gunaTileButton8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaTileButton8.ForeColor = System.Drawing.Color.Silver;
		this.gunaTileButton8.Image = null;
		this.gunaTileButton8.ImageSize = new System.Drawing.Size(52, 52);
		this.gunaTileButton8.Location = new System.Drawing.Point(174, 8);
		this.gunaTileButton8.Name = "gunaTileButton8";
		this.gunaTileButton8.OnHoverBaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton8.OnHoverBorderColor = System.Drawing.Color.Black;
		this.gunaTileButton8.OnHoverForeColor = System.Drawing.Color.White;
		this.gunaTileButton8.OnHoverImage = null;
		this.gunaTileButton8.OnPressedColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton8.Size = new System.Drawing.Size(112, 21);
		this.gunaTileButton8.TabIndex = 19;
		this.gunaTileButton8.Text = "Movies";
		this.gunaTileButton8.Click += new System.EventHandler(gunaTileButton8_Click);
		this.gunaTileButton9.AnimationHoverSpeed = 0.07f;
		this.gunaTileButton9.AnimationSpeed = 0.03f;
		this.gunaTileButton9.BaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton9.BorderColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton9.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaTileButton9.FocusedColor = System.Drawing.Color.Empty;
		this.gunaTileButton9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaTileButton9.ForeColor = System.Drawing.Color.Silver;
		this.gunaTileButton9.Image = null;
		this.gunaTileButton9.ImageSize = new System.Drawing.Size(52, 52);
		this.gunaTileButton9.Location = new System.Drawing.Point(90, 8);
		this.gunaTileButton9.Name = "gunaTileButton9";
		this.gunaTileButton9.OnHoverBaseColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton9.OnHoverBorderColor = System.Drawing.Color.Black;
		this.gunaTileButton9.OnHoverForeColor = System.Drawing.Color.White;
		this.gunaTileButton9.OnHoverImage = null;
		this.gunaTileButton9.OnPressedColor = System.Drawing.Color.FromArgb(24, 24, 24);
		this.gunaTileButton9.Size = new System.Drawing.Size(112, 21);
		this.gunaTileButton9.TabIndex = 18;
		this.gunaTileButton9.Text = "Bots";
		this.gunaTileButton9.Click += new System.EventHandler(gunaTileButton9_Click);
		this.gunaImageButton12.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaImageButton12.Image = (System.Drawing.Image)resources.GetObject("gunaImageButton12.Image");
		this.gunaImageButton12.ImageSize = new System.Drawing.Size(20, 20);
		this.gunaImageButton12.Location = new System.Drawing.Point(6, 7);
		this.gunaImageButton12.Name = "gunaImageButton12";
		this.gunaImageButton12.OnHoverImage = (System.Drawing.Image)resources.GetObject("gunaImageButton12.OnHoverImage");
		this.gunaImageButton12.OnHoverImageOffset = new System.Drawing.Point(0, 0);
		this.gunaImageButton12.Size = new System.Drawing.Size(23, 21);
		this.gunaImageButton12.TabIndex = 17;
		this.gunaLabel22.AutoSize = true;
		this.gunaLabel22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gunaLabel22.ForeColor = System.Drawing.Color.FromArgb(34, 34, 34);
		this.gunaLabel22.Location = new System.Drawing.Point(170, 284);
		this.gunaLabel22.Name = "gunaLabel22";
		this.gunaLabel22.Size = new System.Drawing.Size(110, 13);
		this.gunaLabel22.TabIndex = 15;
		this.gunaLabel22.Text = "Loc#9666 vida#6666";
		this.gunaImageButton13.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaImageButton13.Image = (System.Drawing.Image)resources.GetObject("gunaImageButton13.Image");
		this.gunaImageButton13.ImageSize = new System.Drawing.Size(20, 20);
		this.gunaImageButton13.Location = new System.Drawing.Point(414, 7);
		this.gunaImageButton13.Name = "gunaImageButton13";
		this.gunaImageButton13.OnHoverImage = (System.Drawing.Image)resources.GetObject("gunaImageButton13.OnHoverImage");
		this.gunaImageButton13.OnHoverImageOffset = new System.Drawing.Point(0, 0);
		this.gunaImageButton13.Size = new System.Drawing.Size(20, 20);
		this.gunaImageButton13.TabIndex = 14;
		this.gunaImageButton13.Click += new System.EventHandler(gunaImageButton13_Click);
		this.gunaImageButton14.DialogResult = System.Windows.Forms.DialogResult.None;
		this.gunaImageButton14.Image = (System.Drawing.Image)resources.GetObject("gunaImageButton14.Image");
		this.gunaImageButton14.ImageSize = new System.Drawing.Size(20, 20);
		this.gunaImageButton14.Location = new System.Drawing.Point(435, 7);
		this.gunaImageButton14.Name = "gunaImageButton14";
		this.gunaImageButton14.OnHoverImage = (System.Drawing.Image)resources.GetObject("gunaImageButton14.OnHoverImage");
		this.gunaImageButton14.OnHoverImageOffset = new System.Drawing.Point(0, 0);
		this.gunaImageButton14.Size = new System.Drawing.Size(20, 20);
		this.gunaImageButton14.TabIndex = 13;
		this.gunaImageButton14.Click += new System.EventHandler(gunaImageButton14_Click);
		this.gunaSeparator3.LineColor = System.Drawing.Color.FromArgb(197, 197, 197);
		this.gunaSeparator3.Location = new System.Drawing.Point(8, 30);
		this.gunaSeparator3.Name = "gunaSeparator3";
		this.gunaSeparator3.Size = new System.Drawing.Size(445, 10);
		this.gunaSeparator3.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(471, 315);
		base.Controls.Add(this.vitalityTabControl1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "Form2";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Form2";
		this.vitalityTabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.gunaElipsePanel1.ResumeLayout(false);
		this.gunaElipsePanel1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.gunaElipsePanel2.ResumeLayout(false);
		this.gunaElipsePanel2.PerformLayout();
		this.gunaGroupBox1.ResumeLayout(false);
		this.gunaGroupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.gunaPictureBox1).EndInit();
		this.tabPage3.ResumeLayout(false);
		this.gunaElipsePanel3.ResumeLayout(false);
		this.gunaElipsePanel3.PerformLayout();
		base.ResumeLayout(false);
	}
}
