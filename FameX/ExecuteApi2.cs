using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using FluorineFx.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FameX;

public class ExecuteApi2
{
	public static string ticket;

	public static string Endpoint = "";

	public static int actorid = 0;

	public static int actorid2 = 0;

	public static int sent;

	public static byte[] bytes;

	public static string username;

	public static string password;

	public string sessionId = "";

	public static string DecodeAMF(byte[] body)
	{
		string result = "";
		AMFMessage aMFMessage = null;
		MemoryStream memoryStream = new MemoryStream(body);
		try
		{
			AMFDeserializer aMFDeserializer = new AMFDeserializer(memoryStream);
			aMFMessage = aMFDeserializer.ReadAMFMessage();
		}
		catch (DecoderFallbackException)
		{
			memoryStream.Position = 0L;
			AMFReader aMFReader = new AMFReader(memoryStream);
			aMFReader.FaultTolerancy = true;
			object content = aMFReader.ReadAMF3Data();
			aMFMessage = new AMFMessage(3);
			aMFMessage.AddBody(new AMFBody(string.Empty, string.Empty, content));
		}
		foreach (AMFBody body2 in aMFMessage.Bodies)
		{
			result = JsonConvert.SerializeObject(body2.Content);
		}
		return result;
	}

	public string GetEndpointForServer(string server)
	{
		using WebClient webClient = new WebClient();
		JObject val = JObject.Parse(webClient.DownloadString("https://disco.mspapis.com/disco/v1/services/msp/" + server + "?services=mspwebservice"));
		Endpoint = (string)val["Services"][(object)0][(object)"Endpoint"];
		return Endpoint;
	}

	public static byte[] imgToByteArray(Image img)
	{
		using MemoryStream memoryStream = new MemoryStream();
		img.Save(memoryStream, ImageFormat.Jpeg);
		return memoryStream.ToArray();
	}

	public void LoginAndValidate(string user, string pass)
	{
		try
		{
			SessionID sessionID = new SessionID();
			sessionId = sessionID.GetSessionId();
			username = user;
			string text = "MovieStarPlanet.WebService.User.AMFUserServiceWeb.Login";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[6]
			{
				user,
				pass,
				new object[1],
				null,
				null,
				"dfe63db81352644256c125a22e84cd6a"
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text, "/1", new object[6]
			{
				user,
				pass,
				new object[1],
				null,
				null,
				"dfe63db81352644256c125a22e84cd6a"
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text, "POST", data);
			dynamic val = JsonConvert.DeserializeObject(DecodeAMF(body));
			if (val.loginStatus.status == "Success")
			{
				actorid = (int)val.loginStatus.actor.ActorId;
				ticket = (string)val.loginStatus.ticket;
				GetPostLoginBundle();
			}
			else if (val.loginStatus.status == "ThirdPartyCreated")
			{
				actorid = (int)val.loginStatus.actor.ActorId;
				ticket = (string)val.loginStatus.ticket;
				GetPostLoginBundle();
			}
		}
		catch (Exception)
		{
		}
	}

	public void LoginCheck(string user, string pass)
	{
		try
		{
			GetEndpointForServer("us");
			username = user;
			string text = "MovieStarPlanet.WebService.User.AMFUserServiceWeb.Login";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[6]
			{
				user,
				pass,
				new object[1],
				null,
				null,
				"dfe63db81352644256c125a22e84cd6a"
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text, "/1", new object[6]
			{
				user,
				pass,
				new object[1],
				null,
				null,
				"dfe63db81352644256c125a22e84cd6a"
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text, "POST", data);
			dynamic val = JsonConvert.DeserializeObject(DecodeAMF(body));
			if (val.loginStatus.status == "Success")
			{
				actorid = (int)val.loginStatus.actor.ActorId;
				ticket = (string)val.loginStatus.ticket;
				GetProfileTodosCount();
			}
			else if (val.loginStatus.status == "ThirdPartyCreated")
			{
				actorid = (int)val.loginStatus.actor.ActorId;
				ticket = (string)val.loginStatus.ticket;
				GetProfileTodosCount();
			}
		}
		catch (Exception)
		{
		}
	}

	public void GetEmoticonPackages()
	{
		GetEndpointForServer("us");
		string text = TicketGenerator.headerTicket(ticket);
		string text2 = "MovieStarPlanet.WebService.Spending.AMFSpendingService.GetEmoticonPackages";
		AMFMessage aMFMessage = new AMFMessage(3);
		if (sessionId != null)
		{
			aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
		}
		else
		{
			aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
		}
		aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[2]
		{
			new TicketHeader
			{
				Ticket = text,
				anyAttribute = null
			},
			actorid
		})));
		aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
		aMFMessage.AddBody(new AMFBody(text2, "/1", new object[2]
		{
			new TicketHeader
			{
				Ticket = text,
				anyAttribute = null
			},
			actorid
		}));
		MemoryStream memoryStream = new MemoryStream();
		AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
		aMFSerializer.WriteMessage(aMFMessage);
		aMFSerializer.Flush();
		byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
		using WebClient webClient = new WebClient();
		webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
		webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
		webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
		byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
		object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
		LoadBlockedAndBlockingActors();
	}

	public void LoadBlockedAndBlockingActors()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.LoadBlockedAndBlockingActors";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
			LoadBlockedAndBlockingActorsNeb();
		}
		catch (Exception)
		{
		}
	}

	public void LoadBlockedAndBlockingActorsNeb()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.LoadBlockedAndBlockingActorsNeb";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
			GetPostLoginBundle();
		}
		catch (Exception)
		{
		}
	}

	public void GetPostLoginBundle()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.GetPostLoginBundle";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
			LoadMovieStarListRevised1();
		}
		catch (Exception)
		{
		}
	}

	public void LoadMovieStarListRevised1()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.MovieStar.AMFMovieStarService.LoadMovieStarListRevised";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				new object[1] { actorid }
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				new object[1] { actorid }
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
			GetFriendListWithNameAndScore();
		}
		catch (Exception)
		{
		}
	}

	public void GetFriendListWithNameAndScore()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.Friendships.AMFFriendshipService.GetFriendListWithNameAndScore";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
			LoadMood();
		}
		catch (Exception)
		{
		}
	}

	public void LoadMood()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.LoadMood";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
			GetProfileTodosCount();
		}
		catch (Exception)
		{
		}
	}

	public void GetProfileTodosCount()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.Friendships.AMFFriendshipService.GetProfileTodosCount";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
			GetActiveSpecialsItems();
		}
		catch (Exception)
		{
		}
	}

	public void GetActiveSpecialsItems()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.Spending.AMFSpendingService.GetActiveSpecialsItems";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
			GetOfflineTodos();
		}
		catch (Exception)
		{
		}
	}

	public void GetOfflineTodos()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.Activity.AMFActivityService.GetOfflineTodos";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[4]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid,
				0,
				100
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[4]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid,
				0,
				100
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
			GetAllQuestStatus();
		}
		catch (Exception)
		{
		}
	}

	public void GetAllQuestStatus()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.Quest.AMFQuestService.GetAllQuestStatus";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
			LoadModeratorInformation();
		}
		catch (Exception)
		{
		}
	}

	public void LoadModeratorInformation()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.LoadModeratorInformation";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
			GetAnchorCharacterList();
		}
		catch (Exception)
		{
		}
	}

	public void GetAnchorCharacterList()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.AnchorCharacter.AMFAnchorCharacterService.GetAnchorCharacterList";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[0])));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[0]));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
			LoadMovieStarListRevised2();
		}
		catch (Exception)
		{
		}
	}

	public void LoadMovieStarListRevised2()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.MovieStar.AMFMovieStarService.LoadMovieStarListRevised";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				new object[1] { 4 }
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				new object[1] { 4 }
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
			GetNotificationCount();
		}
		catch (Exception)
		{
		}
	}

	public void GetNotificationCount()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.NotificationCenter.AMFNotificationCenterService.GetNotificationCount";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
			UpdateBehaviourStatusNew();
		}
		catch (Exception)
		{
		}
	}

	public void UpdateBehaviourStatusNew()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.UserSession.AMFUserSessionService.UpdateBehaviourStatusNew";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[6]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid,
				0,
				"",
				-1,
				-1
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[6]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid,
				0,
				"",
				-1,
				-1
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
			GetPiggyBank();
		}
		catch (Exception)
		{
		}
	}

	public void GetPiggyBank()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.PiggyBank.AMFPiggyBankService.GetPiggyBank";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[1]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				}
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[1]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				}
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
			ApproveDefaultAnchorFriendship();
		}
		catch (Exception)
		{
		}
	}

	public void ApproveDefaultAnchorFriendship()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.Friendships.AMFFriendshipService.ApproveDefaultAnchorFriendship";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
			UpdateGift();
		}
		catch (Exception)
		{
		}
	}

	public void UpdateGift()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.UserSession.AMFUserSessionService.UpdateGift";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
			ApproveDefaultAnchorFriendshipLast();
		}
		catch (Exception)
		{
		}
	}

	public void ApproveDefaultAnchorFriendshipLast()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.Friendships.AMFFriendshipService.ApproveDefaultAnchorFriendship";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
		}
		catch (Exception)
		{
		}
	}

	public void GetClaimableCategories()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.Achievement.AMFAchievementWebService.GetClaimableCategories";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
			GetProfileTodosCount();
		}
		catch (Exception)
		{
		}
	}

	public void GetLevelUps()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.ActorService.AMFActorServiceForWeb.GetLevelUps";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[1]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				}
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[1]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				}
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
			approveFriendShip();
		}
		catch (Exception)
		{
		}
	}

	public void approveFriendShip()
	{
		try
		{
			string text = TicketGenerator.headerTicket(ticket);
			string text2 = "MovieStarPlanet.WebService.Friendships.AMFFriendshipService.ApproveDefaultAnchorFriendship";
			AMFMessage aMFMessage = new AMFMessage(3);
			if (sessionId != null)
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, sessionId));
			}
			else
			{
				aMFMessage.AddHeader(new AMFHeader("sessionID", mustUnderstand: false, "Nm5tIdwPyRR1x7QrYP034QRtHux4okompdYNpu6sJibIhODUoCGRHskqqnil3SV/"));
			}
			aMFMessage.AddHeader(new AMFHeader("id", mustUnderstand: false, ChecksumCalculator.createChecksum(new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			})));
			aMFMessage.AddHeader(new AMFHeader("needClassName", mustUnderstand: true, false));
			aMFMessage.AddBody(new AMFBody(text2, "/1", new object[2]
			{
				new TicketHeader
				{
					Ticket = text,
					anyAttribute = null
				},
				actorid
			}));
			MemoryStream memoryStream = new MemoryStream();
			AMFSerializer aMFSerializer = new AMFSerializer(memoryStream);
			aMFSerializer.WriteMessage(aMFMessage);
			aMFSerializer.Flush();
			byte[] data = Encoding.Default.GetBytes(Encoding.Default.GetString(memoryStream.ToArray()));
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-amf";
			webClient.Headers[HttpRequestHeader.Referer] = "https://assets.mspcdns.com/msp/84.2.1/Main_20190620_082745.swf/[[DYNAMIC]]/1";
			webClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.120 Safari/537.36 OPR/64.0.3417.167";
			byte[] body = webClient.UploadData(Endpoint + "/Gateway.aspx?method=" + text2, "POST", data);
			object obj = JsonConvert.DeserializeObject(DecodeAMF(body));
		}
		catch (Exception)
		{
		}
	}
}
