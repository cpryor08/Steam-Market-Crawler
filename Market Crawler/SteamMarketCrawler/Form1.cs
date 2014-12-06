using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace SteamMarketCrawler
{
	public class Form1 : Form
	{
		public const string ItemURL = "http://steamcommunity.com/market/listings/730/";
		public const string MarketURL = "http://steamcommunity.com/market/priceoverview/?country=US&currency=3&appid=730&market_hash_name=";
		public List<string> WeaponHashes;
		public int MaxPercent = 25;
		public int Interval = 60000;
		public bool PlaySound = true;
		public bool Email = false;
		public MailAddress EmailAddress;
		public string EmailPassword = "";
		public string SMTP = "";
		public ushort Port = 0;
		private IContainer components = null;
		private DataGridView dataGridView1;
		private DataGridViewTextBoxColumn Weapon;
		private DataGridViewTextBoxColumn AveragePrice;
		private DataGridViewTextBoxColumn Price;
		private DataGridViewTextBoxColumn URL;
		private StatusStrip statusStrip1;
		private ToolStripStatusLabel toolStripStatusLabel1;
		public Form1()
		{
			this.InitializeComponent();
			this.WeaponHashes = File.ReadAllLines("WeaponHashes.txt", Encoding.UTF8).ToList<string>();
			IniFile iniFile = new IniFile("settings.ini");
			this.MaxPercent = iniFile.ReadInt32("Settings", "MaxPercent");
			this.Interval = iniFile.ReadInt32("Settings", "Interval");
			this.PlaySound = iniFile.ReadBoolean("Settings", "PlaySound");
			this.Email = iniFile.ReadBoolean("Email", "SendEmail");
			this.EmailAddress = new MailAddress(iniFile.ReadString("Email", "Address"));
			this.EmailPassword = iniFile.ReadString("Email", "Password");
			this.SMTP = iniFile.ReadString("Email", "SMTP");
			this.Port = iniFile.ReadUInt16("Email", "Port");
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += new DoWorkEventHandler(this.bw_DoWork);
			backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(this.bw_ProgressChanged);
			backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.bw_RunWorkerCompleted);
			backgroundWorker.WorkerReportsProgress = true;
			backgroundWorker.RunWorkerAsync();
		}
		private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
		}
		private void bw_DoWork(object sender, DoWorkEventArgs e)
		{
			while (true)
			{
				(sender as BackgroundWorker).ReportProgress(0);
				Thread.Sleep(this.Interval);
			}
		}
		private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.toolStripStatusLabel1.Text = "Scanning market... ";
			this.dataGridView1.Rows.Clear();
			string[] array = this.WeaponHashes.ToArray();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				try
				{
					MarketItem marketItem = JsonConvert.DeserializeObject<MarketItem>(new WebClient().DownloadString("http://steamcommunity.com/market/priceoverview/?country=US&currency=3&appid=730&market_hash_name=" + text));
					if (marketItem.success)
					{
						marketItem.lowest_price = marketItem.lowest_price.Split(new char[]
						{
							'&'
						})[0].Replace(',', '.').Replace('-', '0');
						marketItem.median_price = marketItem.median_price.Split(new char[]
						{
							'&'
						})[0].Replace(',', '.').Replace('-', '0');
						double num = double.Parse(marketItem.lowest_price);
						num = Math.Round(num + 0.15 * num, 2);
						double num2 = double.Parse(marketItem.median_price);
						num2 = Math.Round(num2 + 0.15 * num2, 2);
						string text2 = text.Split(new char[]
						{
							'%'
						})[0];
						if (num / num2 * 100.0 < (double)this.MaxPercent || this.MaxPercent == 0)
						{
							this.dataGridView1.Rows.Add(new object[]
							{
								text2,
								num2,
								num,
								"http://steamcommunity.com/market/listings/730/" + text
							});
							if (this.PlaySound)
							{
								using (SoundPlayer soundPlayer = new SoundPlayer("sound.wav"))
								{
									soundPlayer.Play();
								}
							}
							if (this.Email)
							{
								SmtpClient smtpClient = new SmtpClient
								{
									Host = this.SMTP,
									Port = (int)this.Port,
									EnableSsl = true,
									DeliveryMethod = SmtpDeliveryMethod.Network,
									UseDefaultCredentials = false,
									Credentials = new NetworkCredential(this.EmailAddress.Address, this.EmailPassword)
								};
								using (MailMessage mailMessage = new MailMessage(this.EmailAddress, this.EmailAddress))
								{
									mailMessage.Subject = "MarketCrawler: New Cheap " + text2;
									mailMessage.Body = string.Concat(new object[]
									{
										"A(n) ",
										text2,
										" went up for auction for $",
										num,
										" (%",
										100.0 - Math.Round(num / num2 * 100.0, 2),
										" off, median: $",
										num2,
										") at ",
										DateTime.Now.ToString()
									});
									smtpClient.Send(mailMessage);
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					this.WeaponHashes.Remove(text);
					MessageBox.Show(ex.ToString() + "Removed Weapon Hash: " + text);
				}
			}
			this.toolStripStatusLabel1.Text = "Done";
		}
		private void dataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			Process.Start(this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.dataGridView1 = new DataGridView();
			this.Weapon = new DataGridViewTextBoxColumn();
			this.AveragePrice = new DataGridViewTextBoxColumn();
			this.Price = new DataGridViewTextBoxColumn();
			this.URL = new DataGridViewTextBoxColumn();
			this.statusStrip1 = new StatusStrip();
			this.toolStripStatusLabel1 = new ToolStripStatusLabel();
			((ISupportInitialize)this.dataGridView1).BeginInit();
			this.statusStrip1.SuspendLayout();
			base.SuspendLayout();
			this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new DataGridViewColumn[]
			{
				this.Weapon,
				this.AveragePrice,
				this.Price,
				this.URL
			});
			this.dataGridView1.Location = new Point(13, 13);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new Size(446, 420);
			this.dataGridView1.TabIndex = 0;
			this.dataGridView1.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dataGridView1_RowHeaderMouseDoubleClick);
			this.Weapon.HeaderText = "Weapon";
			this.Weapon.Name = "Weapon";
			this.AveragePrice.HeaderText = "Average Price";
			this.AveragePrice.Name = "AveragePrice";
			this.Price.HeaderText = "Price";
			this.Price.Name = "Price";
			this.URL.HeaderText = "URL";
			this.URL.Name = "URL";
			this.statusStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripStatusLabel1
			});
			this.statusStrip1.Location = new Point(0, 449);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new Size(474, 22);
			this.statusStrip1.TabIndex = 1;
			this.statusStrip1.Text = "statusStrip1";
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new Size(35, 17);
			this.toolStripStatusLabel1.Text = "Done";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(474, 471);
			base.Controls.Add(this.statusStrip1);
			base.Controls.Add(this.dataGridView1);
			base.Name = "Form1";
			this.Text = "Market Crawler";
			((ISupportInitialize)this.dataGridView1).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
