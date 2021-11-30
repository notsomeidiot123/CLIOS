using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using System.Net.Sockets;
using System.Diagnostics;

	class Program
	{
		public static string[] pEx = {".h",
		 ".c",
		 ".cpp",
		 ".cs",
		 ".html",
		 ".css",
		 ".lua",
		 ".go",
		 ".png",
		 ".pdf",
		 ".jpeg",
		 ".jpg",
		 ".jar",
		 ".js",
		 ".java",
		 ".gdc",
		 ".f",
		 ".fs",
		 ".f90",
		 ".for",
		 ".lisp",
		 ".lsp",
		 ".l",
		 ".cl",
		 ".fasl",
		 ".exe"};
		public static string[] supEx = {
			".cli",
			".clid",
			".txt",
			".json"
		};
		//program extensions
		public static Dictionary<string, Dictionary<string, List<string>>> dir = new Dictionary<string, Dictionary<string, List<string>>>();
		public static string[] errors = {
			"CLIOS 0001, File does not exist",
			"CLIOS 0002, Trouble parsing argument, try again",
			"CLIOS 0003, Could not save",
			"CLIOS 0004, Quit"
		};
		public static void Main()
		{//50
			Console.ForegroundColor = ConsoleColor.DarkGreen;
			netConnect.establishinfo();
			Console.Clear();
			string savePath = "startup.json";
			//other variables
			DataSave data = new DataSave();
			var netCon = new netConnect();
			if (!File.Exists(savePath) && !File.Exists("backup.json"))
			{
				Console.Write("Welcome to CLIOS, a terrible program that mimics an operating system.\nTo get started, please create an account.\n Username: ");//haha nice
				string user = Console.ReadLine();
				if (user == "")
				{
					user = "root";
				}
				data.username = user;
				Console.Write("\nGreat! now chose a password. password can be anything.\n Password:");
				string pass = Console.ReadLine();
				data.password = pass;
				data.firstTimeStartup = false;
				Directory.CreateDirectory(data.username);
				dir[data.username] = new Dictionary<string, List<string>>();
				data.fileList = dir;
				File.WriteAllText(savePath, JsonSerializer.Serialize<DataSave>(data));
				Console.Write("Thanks! now initiating os... please run program again\n");
				saveData(data);
				Main();
			}
			else
			{
				try
				{
					data = JsonSerializer.Deserialize<DataSave>(File.ReadAllText(savePath));
					//load data
					dir = data.fileList;

					if (Directory.Exists(data.username))
					{
						Console.Write("files loaded\n");
					}
					else
					{
						Console.Write("files partially loaded. may not have access to saved files\n" + errors[0] + "\n");
					}
					//end load
				}
				catch
				{
					Console.Write("failed... trying backup\n\n");
					data = JsonSerializer.Deserialize<DataSave>(File.ReadAllText("backup.json"));
					//load data
					dir = data.fileList;
					if (Directory.Exists(data.username))
					{
						Console.Write("files loaded\n");
					}
					else
					{
						Console.Write("files partially loaded. may not have access to saved files\n" + errors[0] + "\n");
					}//100
					 //end load
				}
				Console.Write($"\n{data.username}\n password:");
				string input = Input();
				if (input == data.password || input == "")
				{
					Console.Clear();
					Thread.Sleep(100);
					Console.Write("				 	CLIOS\n\n				 Made by Chris\n\n 0.2.3 alpha");
					Thread.Sleep(100);
					Console.Write("\n\n\n\ntype -h or help for help\n");
					//Application.LoadApps();
					while (true == true)
					{
						//get input and everything
						
						Console.Write("<$> ");
						Console.ForegroundColor = ConsoleColor.White;
						string inp = Input();
						string[] inpArr = inp.Split(" ");
						//scripting statement
						if (inpArr[0] == "print" && inpArr.Length > 1)
						{
							Console.ForegroundColor =ConsoleColor.Green;
							//concatenate the string together
							string outp = "";
							int i = 0;
							foreach (var item in inpArr)
							{
								i++;
								if (item != "print" || i != 1)
								{
									if (item == "-h" || item == "help" && i != 1)
									{
										outp = "Print something to the console";
									}
									else
									{
										outp += " " + item;
									}
								}

							}
							//output
							Console.Write(outp + "\n");
						}
						else if (inpArr[0] == "help" || inpArr[0] == "-h")
						{
							Console.ForegroundColor = ConsoleColor.DarkBlue;
							Console.Write("()=not included in lite version. [] = not implemented as of now\nprint: write something to the command line\n -h get help\n");
							Console.Write("dir: directories, but will only show those registered through the program that the user has access to. sorry evil maids, you can't find the password from here\n {folder}{file} view file contents\n");
							Console.Write("()ping {ip}: dertermine the connection to a computer/server\n");
							Console.Write("netDebug: development purposes, shows network and host information\n");
							Console.Write("quit: quit the program, does not save and quit\n");
							Console.Write("save: save program data\n");
							Console.Write("run: run a file. non-stock programs and images do not work currently. \nread to read a program file\n");
							Console.Write("create/new {path}: create a file\n");
							Console.Write("edit {path}, {data}: edit file overwrites completely by default\n -k: combine file with data\n");
							Console.Write("delete {path}: delete a file\n");
							Console.Write("download {file}, {url}\n");
							Console.Write("upload{file}, {url}\n");
						}
						else if (inpArr[0] == "netDebug")
						{
							netConnect.selfIdentify();
						}
						//will cause error, do not use on replit
						//150
						else if (inpArr[0] == "ping")
						{
							int i = 0;
							foreach (var item in inpArr)
							{
								i++;
								if (item == "-r" && i == 3)
								{
									for (int j = 0; j < 5; j++)
									{
										netConnect.pingIP(inpArr[1]);
									}
								}
								/*else if(inpArr[3]=="-t"){
									//threading code
								}*/
								else
								{
									netConnect.pingIP(inpArr[1]);
								}
							}
						}
						else if (inpArr[0] == "quit")
						{
							Console.WriteLine("Will show an exception error. do not worry, this is the quickest way for me to be 'smart'");
							System.Environment.Exit(0);
						}
						else if (inpArr[0] == "dir")
						{
							if (inpArr.Length == 1)
							{
								List<string> subDirectories = new List<string>();
								List<string> files = new List<string>();
								string fileString = "";
								string folderString = "";
								subDirectories = Directory.EnumerateDirectories(data.username).ToList();
								files = Directory.EnumerateFiles(data.username).ToList();
								foreach (var file in files)
								{
									fileString += $"->{file}\n";
								}
								foreach (var folder in subDirectories)
								{
									folderString += $"->{folder}\n";
								}
								Console.Write($"Folders\n{folderString}");
								Console.Write($"Files\n{fileString}");
							}
							else if (inpArr.Length >= 2)
							{
								try
								{
									if (inpArr[1] == "new")
									{
										Directory.CreateDirectory($@"{data.username}/{inpArr[2]}");
									}
									else if (inpArr[2] == "-ls")
									{
										try
										{
											List<string> subDirectories = new List<string>();
											List<string> files = new List<string>();
											string fileString = "";
											string folderString = "";
											subDirectories = Directory.EnumerateDirectories($"{data.username}/" + inpArr[1]).ToList();
											files = Directory.EnumerateFiles($"{data.username}/" + inpArr[1]).ToList();//200
											if (files.Count != 0)
											{
												foreach (var file in files)
												{
													fileString += $"->{file}\n";
												}
												Console.Write($"Files\n{fileString}");
											}
											else
											{
												Console.Write("No Files\n");
											}
											if (subDirectories.Count != 0)
											{
												foreach (var folder in subDirectories)
												{
													folderString += $"->{folder}\n";
												}
												Console.Write($"Folders\n{folderString}");
											}
											else
											{
												Console.Write("No Folders\n");
											}
										}
										catch
										{
											Console.Write("Failed\n");
										}
									}
								}
								catch (Exception ex)
								{
									Console.Write($"{ex}\n");
								}
							}
						}
						else if (inpArr[0] == "create" || inpArr[0] == "new")
						{
							try
							{
								string path = $@"{data.username}/{inpArr[1]}";
								File.OpenWrite(path);
							}
							catch
							{
								Console.Write("Invalid syntax");
							}
						}
						else if (inpArr[0] == "edit")
						{
							try
							{
								string path = $@"{data.username}/{inpArr[1]}";
								if (inpArr[2] == "-k")
								{
									string append = "";
									int i = 0;
									foreach (var item in inpArr)
									{
										i++;
										if (i > 3 && item != @"\n")
										{
											append += " " + item;//250
										}
										else if(item == @"\n")
										{
											append += "\n";
										}
									}
									File.AppendAllText(path, append);
								}
								else
								{
									int i = 0;
									string append = "";

									foreach (var item in inpArr)
									{
										i++;
										if (i > 2)
										{
											if (item.Contains(@"\n"))
											{
												string[] tempString = item.Split(@"\n");
												append += " " + tempString[1] + "\n";
											}
											else
											{
												append += " " + item;
											}

										}
									}
									File.WriteAllText(path, append);
								}
								Console.Write(File.ReadAllText(path) + "\n");
							}
							catch
							{
								Console.Write("Failed\n");
							}
						}
						else if (inpArr[0] == "en" || inpArr[0] == "encrypt")
						{
							string path = inpArr[1];
							File.Encrypt(path);
						}
						else if (inpArr[0] == "decrypt")
						{
							string path = inpArr[1];
							File.Decrypt(path);
						}
						else if (inpArr[0] == "reset")
						{
							if (inpArr.Length > 1 && inpArr[1] == "-f")
							{
								File.Delete(savePath);
								File.Delete("backup.json");
								//make this a recursive method, so that the user can have as many directories as they want
								List<string> files = new List<string>();
								List<string> folders = new List<string>();
								folders = Directory.EnumerateDirectories(data.username).ToList();
								files = Directory.EnumerateFiles(data.username).ToList();
								foreach (var item in folders)
								{
									List<string> subfolders = new List<string>();
									subfolders = Directory.EnumerateDirectories(item).ToList();
									List<string> subfiles = new List<string>();
									subfiles = Directory.EnumerateFiles(item).ToList();
									foreach (var subfolder in subfolders)
									{
										List<string> subfolderfiles = new List<string>();
										subfolderfiles = Directory.EnumerateFiles(subfolder).ToList();
										foreach (var file in subfolderfiles)
										{
											File.Delete(file);
										}
										Directory.Delete(subfolder);
									}//300
									foreach (var file in subfiles)
									{
										File.Delete(file);
									}
									Directory.Delete(item);
								}
								foreach (var item in files)
								{
									File.Delete($@"{item}");
								}
								Directory.Delete(data.username);
							}
							else
							{
								Main();
							}
						}
						else if (inpArr[0] == "search")
						{
							try
							{
								Console.Write(netConnect.testRequest(inpArr[1]) + "\n");
							}
							catch
							{
								Console.Write("Failed\n");
							}//haha weed
						}
						else if (inpArr[0] == "download")
						{
							try
							{
								Console.Write(netConnect.downloadTest(inpArr[1]));
								//Console.Write(Application.Update() + "\n");
							}
							catch
							{
								//Console.Write("Failed, invalid file\n");
							}
							if (inpArr[1] == "-c")
							{
								try
								{
									Console.Write(netConnect.downloadCustom(inpArr[2], inpArr[3]));
								}
								catch
								{
									Console.Write("Not implemented or invalid URL/File\n");
								}
							}
						}
						else if (inpArr[0] == "upDebug")
						{
							try
							{
								Console.Write(netConnect.upload(inpArr[1]) + "\n");
							}
							catch
							{
								Console.Write("Failed\n");
							}
						}
						else if (inpArr[0] == "upload")
						{
							if (inpArr[1] == "-c")
							{
								try
								{
									Console.Write("Uploading...\n");
									netConnect.uploadCustom(inpArr[2], inpArr[3]);
									Console.Write("Uploaded!\n");
								}
								catch
								{//350
									Console.Write("Upload Failed\n");
								}
							}
						}
						else if (inpArr[0] == "delete")
						{
							try
							{
								File.Delete($"{data.username}/{inpArr[1]}");
							}
							catch
							{
								Console.Write("Delete Failed, file does not exist\n");
							}
						}
						else if (inpArr[0] == "run")
						{
							bool unsupported = false;
							bool supported = false;
							int i = 0;
							int j = 0;
							foreach (var item in pEx)
							{
								if (inpArr[1].Contains(item) && item != ".cs")
								{
									unsupported = true;
									i = inpArr[1].IndexOf(item);
								}
							}
							foreach (var item in supEx)
							{
								if (inpArr[1].Contains(item))
								{
									supported = false;
									j = inpArr[1].IndexOf(item);
								}
							}
							if (unsupported && supported)
							{
								if (j > i)
								{
									unsupported = false;
								}
								else
								{
									supported = false;
								}
							}
							try
							{
								Console.ForegroundColor = ConsoleColor.DarkBlue;
								if (inpArr[1].Contains(".txt"))
								{
									Console.Write(File.ReadAllText($"{data.username}/" + inpArr[1]) + "\n");
								}
								else
								{
									if (inpArr[1].Contains(".cli") || inpArr[1].Contains(".clid") && inpArr[2] == "-re")
									{
										Compiler.ClScript.Run($"{data.username}/{inpArr[1]}");
										/*}else if(inpArr[1].Contains(".cli") || inpArr[1].Contains(".clid")){
											Console.Write("505\n");*/
									}
								}
								Console.ForegroundColor = ConsoleColor.DarkGreen;
							}
							catch
							{
								Console.Write("File does not exist\n");
							}
						}
						else if (inpArr[0] == "read")
						{
							try
							{
								if (inpArr[1].Contains(".cli") || inpArr[0].Contains(".clid"))
								{
									Compiler.ClScript.Read($"{data.username}/{inpArr[1]}");
								}
								else if (!File.Exists($"{data.username}/{inpArr[1]}"))
								{
									Console.Write("File does not exist\n");
								}
								else
								{
									Console.Write("505\n");
								}
							}
							catch
							{
								Console.Write("File not specified\n");
							}
						}
						if(inpArr[0] == "user" || inpArr[0] == "-u")
            {
							Console.Write($"{data.username}\n {data.password}\n");
            }
					Console.ForegroundColor = ConsoleColor.DarkGreen;
					}
					
				}
				else
				{
					throw new ArgumentException("incorrect password");
				}
			}
			//Opening statements, load commands
		}
		//make input easier
		public static string Input()
		{
			string inp = Console.ReadLine();
			Console.ForegroundColor = ConsoleColor.DarkGreen;
			return inp;
		}
		//now obsolete
		//load dictionaries and commands //400
		public static void saveData(DataSave dataS)
		{
			string path = "backup.json";
			string dataString = JsonSerializer.Serialize<DataSave>(dataS);
			File.WriteAllText(path, dataString);
		}
		//now obsolete
		public static DataSave loadData()
		{
			string path = "backup.json";
			DataSave data1 = new DataSave();
			string dataLoad = "";
			if (File.Exists(path))
			{
				dataLoad = File.ReadAllText(path);
				data1 = JsonSerializer.Deserialize<DataSave>(dataLoad);
				return data1;
			}
			else
			{
				File.Create(path);
				data1.firstTimeStartup = true;
				return data1;
			}
		}
	}

	//reminder that ping does not work on replit
	public class netConnect
	{
		public static DataSave data = new DataSave();
		public static string hostip;
		public static string hostname;
		public static void establishinfo()
		{
			try
			{
				data = JsonSerializer.Deserialize<DataSave>(File.ReadAllText("startup.json"));
			}
			catch
			{
				try
				{
					data = JsonSerializer.Deserialize<DataSave>(File.ReadAllText("startup.json"));
				}
				catch
				{
					return;
				}
			}
		}
		public static void pingIP(string input)
		{
			byte[] buffer = { 99, 108, 105, 32, 112, 105, 108, 111, 116 };
			Ping pinger = new Ping();
			PingReply reply = pinger.Send(input, 100, buffer);
			string replyString = "";
			foreach (var item in reply.Buffer)
			{

				replyString += Convert.ToChar(item);
			}
			Console.Write($"{reply.Status}: Sent {Convert.ToChar(34)}{replyString}{Convert.ToChar(34)} to {reply.Address} in {reply.RoundtripTime} milliseconds\n");
		}//450
		public static void establishConnection()
		{
		}
		//test making a request to a server database
		public static string testRequest(string url)
		{
			WebRequest req = WebRequest.Create($"http://{url}");
			req.Credentials = CredentialCache.DefaultCredentials;
			HttpWebResponse response = (HttpWebResponse)req.GetResponse();
			Console.WriteLine($"{response.StatusDescription}");
			string answer;
			try
			{
				using (Stream dataStream = response.GetResponseStream())
				{
					StreamReader reader = new StreamReader(dataStream);
					string responseFromServer = reader.ReadToEnd();
					answer = responseFromServer;
					File.AppendAllText($@"{data.username}/{url}.html", responseFromServer);
				}
			}
			catch
			{
				answer = response.ToString();
			}
			return $"{answer}";
		}//haha 666
		public static string downloadTest(string file)
		{
			using (var Client = new WebClient())
			{
				Client.DownloadFileAsync(new Uri($@"https://CLIOSserver.csharpisbetter.repl.co/{file}"), $@"{data.username}/{file}");
				return $"downloaded stock:[{file}] at CLIOSserver\n";
			}
		}
		public static string upload(string file)
		{
			using (var Client = new WebClient())
			{
				Client.UploadFile($"https://CLIOSserver.csharpisbetter.repl.co", $@"{data.username}/{file}");

				return $"success\n";
			}
		}
		public static string downloadCustom(string url, string file)
		{
			using (var Client = new WebClient())
			{
				Client.DownloadFile($@"{url}/{file}", $@"{data.username}/{file}");
				return $"Downloaded {file} at {url}";
			}
		}
		public static string uploadCustom(string url, string file)
		{
			var Client = new WebClient();
			Client.UploadFile($"{url}", $"{data.username}/{file}");
			return "";
		}
		public static string selfIdentify()
		{//500
		 //get computer name
			hostname = Dns.GetHostName();
			Console.Write("Host: " + hostname + "\n");
			//get the ip then make it the first in the adress list, before converting it toastring
			hostip = Dns.GetHostByName(hostname).AddressList[0].ToString();
			Console.Write($"ip: {hostip}\n");
			return "";
		}
	}
	public class QuitException : Exception
	{
		public QuitException(string message)
		{
		}
	}
	public class DataSave
	{
		public bool firstTimeStartup { get; set; }
		public string username { get; set; }
		public string password { get; set; }
		public Dictionary<string, Dictionary<string, List<string>>> fileList { get; set; }
		public Dictionary<DateTime, string> history { get; set; }
		public List<string> appList { get; set; }
	}
	public class Application
	{
		public static List<string> apps = new List<string>();
		public static DataSave data = new DataSave();
		/*public static string LoadApps()
		{
			apps = new List<string>();
			if (data.appList == null)
			{
				data.appList = new List<string>();
			}
			try
			{
				data = JsonSerializer.Deserialize<DataSave>(File.ReadAllText("startup.json"));
			}
			catch
			{
				try
				{
					data = JsonSerializer.Deserialize<DataSave>(File.ReadAllText("backup.json"));
				}
				catch
				{
					Console.Write("Loading Failed\n");
				}
			}
			apps = data.appList;
			/*for(int i = 0; i < data.appCount; i++){
				if(i != data.appCount){
					Console.Write($"\rLoading app{i} of {data.appCount}     \n");
					apps[i-1] = data.appList[i-1];
				}else{
					Console.Write($"Loading app{i} of {data.appCount}\n");
					apps[i] = data.appList[i];//550
				}
			}
			//this was my favorite part of my code :(
			int i = 0;
			try
			{
				foreach (var item in apps)
				{
					Random rand = new Random();
					i++;
					Console.Write($"\rLoading app {i} of {apps.Count}     ");
					Thread.Sleep(100 * rand.Next(1, 10));
				}
				Console.Write("\n");
				return "Loaded all apps\n";
			}
			catch
			{
				return "";
			}
		}*/
		/*public static string Update()
		{
			data.appList = apps;
			string saveinfo = JsonSerializer.Serialize<DataSave>(data);
			File.WriteAllText("startup.json", saveinfo);
			File.WriteAllText("backup.json", saveinfo);
			if (data.appList == null)
			{
				data.appList = new List<string>();
			}
			return "saved";
		}*/
	}//575
	public class Compiler
	{
		public class Settings
		{
			public int Repeat { get; set; }
			public bool Silent { get; set; }
			public bool Readable { get; set; }
		}
		public class ClScript
		{
			//make this read, assign line numbers
			public static void Read(string file)
			{
				int i = 0;
				List<string> comp = File.ReadAllLines(file).ToList();
				foreach (var item in comp)
				{
					i++;
					Console.Write($"{i} {item}\n");
				}
				return;
			}
			public static void Run(string file)
			{
				Dictionary<string, string> varDic = new Dictionary<string, string>();
				//int failed = 0;
				string error = "";
				int i = 0;
				List<string> rawText = File.ReadAllLines(file).ToList();
				foreach (var item in rawText)
				{
					//line counter
					i++;
					//line splitter
					string[] splitArr = item.Split(';');
					foreach (var line in splitArr)
					{
						string toPrint = "";
						if (line.Contains("var"))
						{
							char[] splitter = { ' ', '=' };

							string[] splitLine = line.Split(splitter);
							List<string> lineList = new List<string>(splitLine);
							int c = 0;
							foreach (var word in splitLine)
							{
								if (word == "" || word == " ")
								{
									lineList.RemoveAt(c);
									c--;
								}
								c++;
							}
							if (splitLine.Length == 2)
							{
								varDic.Add(splitLine[1], "");
							}
							else
							{
								int l = 0;
								string dicAdd = "";
								foreach(var seg in lineList){
									l++;
									if(l > 2){
										dicAdd += seg + " ";
									}
								}
								varDic.Add(splitLine[2], dicAdd);
							}
						}
						if (line.Contains("print"))
						{
							string[] splitLine = line.Split("<<");
							int j = 0;
							string[] printable = splitLine[1].Split(" ");
							for(int o = 0; o < splitLine.Length; o++){
								splitLine[o] = splitLine[o].Replace(" ", "");
							}
							try
							{
								//Console.Write($"{splitLine[1]}, {splitLine[1].Length-1}, {splitLine[1].LastIndexOf("\"")}, {splitLine[1].IndexOf("\"")} ");
								if (splitLine[1].Contains("\"") && splitLine[1].LastIndexOf("\"") == splitLine[1].Length - 1 && splitLine[1].IndexOf("\"") == 1 || splitLine[1].Contains("\"") && splitLine[1].LastIndexOf("\"") == splitLine[1].Length - 1 && splitLine[1].IndexOf("\"") == 0)
								{
									foreach (var str in printable)
									{
										if (j == 0)
										{
											toPrint += str.Replace("\"", "");	
											j++;
										}
										else
										{
											toPrint += $" {str.Replace("\"", "")}";
										}
									}
									Console.Write(toPrint.Replace("\"", "") + "\n");
								}
								else if(varDic.ContainsKey(splitLine[1]))
								{
									Console.Write(" "+varDic[splitLine[1]].Replace("\"", "")+"\n");
								}
							}
							catch
							{
								Console.Write($"Reference to uninitailized variable at: Line {i}\n");
							}
						}
						
						if (error != "")
						{
							Console.Write(error);
						}
					}
				}
				return;
			}
		}
		//wjbfxmv
	}
