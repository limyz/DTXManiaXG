using FDK;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DTXMania
{
	internal class CActTextBox : CActivity
	{
		private bool b表示中;

		private bool b次のフレームで入力中にする;

		private bool b入力終了直後;

		private bool b入力終了時に非表示にする;

		private bool b検索説明文表示;

		private string str入力中文字列;

		private string str確定文字列;

		private CTexture tx背景;

		private CTexture tx文字列;

		private CTexture tx説明;

		private CTexture txカーソル;

		private CPrivateFastFont prvf入力文字列;

		private CPrivateFastFont prvf説明;

		private Rectangle rectパネル基本位置;

		private bool bIME確定文字列を入力した直後;

		private string strIME入力中文字列;

		private string strIME確定文字列;

		private string strIME入力中文字列_前フレーム;

		private string strIME確定文字列_前回;

		private int nカーソル位置;

		private int nカーソル座標X;

		private CCounter ctカーソル;

		private int n前回確定した文字列リスト_参照カウンタ;

		private static List<string> L前回確定した文字列リスト = new List<string>();

		public bool b入力中
		{
			get;
			set;
		}

		private bool bIME取得可能 => CDTXMania.app.cIMEHook.bAccessible;

		public bool b入力が終了した
		{
			get
			{
				bool result = b入力終了直後;
				b入力終了直後 = false;
				return result;
			}
		}

		public CActTextBox()
		{
			base.bNotActivated = true;
		}

		public override void OnActivate()
		{
			rectパネル基本位置 = new Rectangle(390, 200, 500, 40);
			b表示中 = false;
			b入力中 = false;
			b入力終了直後 = false;
			b次のフレームで入力中にする = false;
			b入力終了時に非表示にする = false;
			b検索説明文表示 = false;
			str入力中文字列 = "";
			str確定文字列 = "";
			strIME入力中文字列 = "";
			strIME確定文字列 = "";
			strIME入力中文字列_前フレーム = "";
			strIME確定文字列_前回 = "";
			bIME確定文字列を入力した直後 = false;
			nカーソル位置 = 0;
			nカーソル座標X = 0;
			ctカーソル = new CCounter(1, 1000, 1, CDTXMania.Timer);
			n前回確定した文字列リスト_参照カウンタ = L前回確定した文字列リスト.Count;
			base.OnActivate();
		}

		public override void OnDeactivate()
		{
			ctカーソル = null;
			base.OnDeactivate();
		}

		public override void OnManagedCreateResources()
		{
			if (base.bNotActivated)
			{
				return;
			}
			//string strファイルの相対パス = Path.Combine("Graphics\\fonts", CDTXMania.app.Resources.Explanation("strCfgSelectMusicInformationFontFileName"));
			//prvf入力文字列 = new CPrivateFastFont(CSkin.Path(strファイルの相対パス), 20);
			//prvf説明 = new CPrivateFastFont(CSkin.Path(strファイルの相対パス), 16);

			prvf入力文字列 = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.str選曲リストフォント), 20, FontStyle.Regular);
			prvf説明 = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.str選曲リストフォント), 14, FontStyle.Regular);
			t基本位置に応じて文字の描画範囲を設定する();
			t背景テクスチャを生成();
			using (Bitmap bitmap = new Bitmap(rectパネル基本位置.Width, 350))
			{
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					graphics.FillRectangle(new SolidBrush(Color.FromArgb(160, Color.Black)), 0, 0, bitmap.Width, bitmap.Height);
					StringBuilder stringBuilder = new StringBuilder(256);
					stringBuilder.AppendLine("*Song Search*");
					stringBuilder.AppendLine("Type in Text and press Enter to search by Title, Artist");
					stringBuilder.AppendLine("and Comment");
					stringBuilder.AppendLine("");
					stringBuilder.AppendLine("Examples:");
					stringBuilder.AppendLine("/t title - Search by Title only");
					stringBuilder.AppendLine("/a artist - Search by Artist only");
					stringBuilder.AppendLine("/c Comment - Search by Comment only");
					stringBuilder.AppendLine("/s search text - Case-sensitive Search");
					stringBuilder.AppendLine("/t /a /s Text - Case-sensitive Search of Title and Artist");
					stringBuilder.AppendLine("/all - Returns all songs in a single list");
					stringBuilder.AppendLine("Type /q followed by Enter to exit search mode");

					using (Bitmap bitmap2 = prvf説明.DrawPrivateFont(stringBuilder.ToString(), CPrivateFont.DrawMode.Edge, Color.PeachPuff, Color.Black, Color.DarkRed, Color.DarkRed, true))
					{
						graphics.DrawImage(bitmap2, 20, 20, bitmap2.Width, bitmap2.Height);
					}
				}
				tx説明 = CDTXMania.tGenerateTexture(bitmap, b黒を透過する: false);
			}
			using (Bitmap bitmap3 = new Bitmap(6, rectパネル基本位置.Height - 4))
			{
				using (Graphics graphics2 = Graphics.FromImage(bitmap3))
				{
					graphics2.FillRectangle(Brushes.White, 0, 0, bitmap3.Width, bitmap3.Height);
				}
				txカーソル = CDTXMania.tGenerateTexture(bitmap3, b黒を透過する: false);
				txカーソル.nTransparency = 192;
			}
			t文字テクスチャを生成();
			base.OnManagedCreateResources();
		}

		public override void OnManagedReleaseResources()
		{
			if (!base.bNotActivated)
			{
				CDTXMania.tDisposeSafely(ref prvf入力文字列);
				CDTXMania.tDisposeSafely(ref prvf説明);
				CDTXMania.tDisposeSafely(ref tx背景);
				CDTXMania.tDisposeSafely(ref tx文字列);
				CDTXMania.tDisposeSafely(ref tx説明);
				CDTXMania.tDisposeSafely(ref txカーソル);
				base.OnManagedReleaseResources();
			}
		}

		public override int OnUpdateAndDraw()
		{
			if (base.bNotActivated)
			{
				return 0;
			}
			if (b表示中)
			{
				tx背景?.tDraw2D(CDTXMania.app.Device, rectパネル基本位置.X, rectパネル基本位置.Y);
				if (b入力中)
				{
					bool flag = false;
					if (bIME取得可能)
					{
						strIME入力中文字列_前フレーム = strIME入力中文字列;
						strIME入力中文字列 = CDTXMania.app.cIMEHook.str入力中文字列;
						strIME確定文字列 = CDTXMania.app.cIMEHook.str確定文字列;
						if (strIME入力中文字列 != strIME入力中文字列_前フレーム)
						{
							flag = true;
						}
						if (strIME確定文字列 != "")
						{
							if (strIME確定文字列 != strIME確定文字列_前回)
							{
								t入力中文字列のカーソル位置に文字を挿入する(strIME確定文字列);
								bIME確定文字列を入力した直後 = true;
								flag = true;
								strIME確定文字列_前回 = strIME確定文字列;
							}
							else
							{
								bIME確定文字列を入力した直後 = false;
							}
						}
					}
					if (CDTXMania.InputManager.Keyboard.bKeyPressed(117) || CDTXMania.InputManager.Keyboard.bKeyPressed(100))
					{
						if (!bIME確定文字列を入力した直後)
						{
							t入力を確定して終了();
						}
					}
					else if (CDTXMania.InputManager.Keyboard.bKeyPressed(53))
					{
						if (strIME入力中文字列_前フレーム == "")
						{
							t入力を確定せずに終了();
						}
					}
					else if (CDTXMania.InputManager.Keyboard.bKeyPressed(31))
					{
						if ((CDTXMania.InputManager.Keyboard.bKeyPressing(75) || CDTXMania.InputManager.Keyboard.bKeyPressing(116)) && strIME入力中文字列 == "")
						{
							string text = tクリップボードから文字列を取得する().Replace("\r\n", "");
							if (text != string.Empty)
							{
								str入力中文字列 = str入力中文字列.Substring(0, nカーソル位置) + text + str入力中文字列.Substring(nカーソル位置);
								nカーソル位置 += text.Length;
								flag = true;
							}
						}
					}
					else if (CDTXMania.InputManager.Keyboard.bKeyPressed(12))
					{
						if ((CDTXMania.InputManager.Keyboard.bKeyPressing(75) || CDTXMania.InputManager.Keyboard.bKeyPressing(116)) && strIME入力中文字列 == "")
						{
							tクリップボードに文字列を設定する(str入力中文字列);
						}
					}
					else if (CDTXMania.InputManager.Keyboard.bKeyPressed(10))
					{
						if ((CDTXMania.InputManager.Keyboard.bKeyPressing(75) || CDTXMania.InputManager.Keyboard.bKeyPressing(116)) && strIME入力中文字列 == "")
						{
							str入力中文字列 = "";
							nカーソル位置 = 0;
							flag = true;
						}
					}
					else if (CDTXMania.InputManager.Keyboard.bKeyPressed(76))
					{
						if (strIME入力中文字列 == "")
						{
							nカーソル位置--;
							if (nカーソル位置 < 0)
							{
								nカーソル位置 = 0;
							}
							flag = true;
						}
					}
					else if (CDTXMania.InputManager.Keyboard.bKeyPressed(118))
					{
						if (strIME入力中文字列 == "")
						{
							nカーソル位置++;
							if (nカーソル位置 > str入力中文字列.Length)
							{
								nカーソル位置 = str入力中文字列.Length;
							}
							flag = true;
						}
					}
					else if (CDTXMania.InputManager.Keyboard.bKeyPressed(132))
					{
						if (strIME入力中文字列 == "" && L前回確定した文字列リスト.Count > 0)
						{
							n前回確定した文字列リスト_参照カウンタ--;
							if (n前回確定した文字列リスト_参照カウンタ < 0)
							{
								n前回確定した文字列リスト_参照カウンタ = 0;
							}
							str入力中文字列 = L前回確定した文字列リスト[n前回確定した文字列リスト_参照カウンタ];
							nカーソル位置 = str入力中文字列.Length;
							flag = true;
						}
					}
					else if (CDTXMania.InputManager.Keyboard.bKeyPressed(50))
					{
						if (strIME入力中文字列 == "" && L前回確定した文字列リスト.Count > 0)
						{
							n前回確定した文字列リスト_参照カウンタ++;
							if (n前回確定した文字列リスト_参照カウンタ >= L前回確定した文字列リスト.Count)
							{
								n前回確定した文字列リスト_参照カウンタ = L前回確定した文字列リスト.Count;
								str入力中文字列 = "";
							}
							else
							{
								str入力中文字列 = L前回確定した文字列リスト[n前回確定した文字列リスト_参照カウンタ];
							}
							nカーソル位置 = str入力中文字列.Length;
							flag = true;
						}
					}
					else if (CDTXMania.InputManager.Keyboard.bKeyPressed(49))
					{
						if (strIME入力中文字列 == "")
						{
							if (nカーソル位置 < str入力中文字列.Length)
							{
								str入力中文字列 = str入力中文字列.Substring(0, nカーソル位置) + str入力中文字列.Substring(nカーソル位置 + 1);
							}
							flag = true;
						}
					}
					else
					{
						_ = (strIME入力中文字列 == "");
					}
					if (flag)
					{
						t文字テクスチャを生成();
					}
					ctカーソル?.tUpdateLoop();
					CCounter cCounter = ctカーソル;
					if (cCounter != null && cCounter.nCurrentValue <= 500)
					{
						txカーソル?.tDraw2D(CDTXMania.app.Device, rectパネル基本位置.X + nカーソル座標X, rectパネル基本位置.Y + 2);
					}
					if (b検索説明文表示)
					{
						tx説明?.tDraw2D(CDTXMania.app.Device, rectパネル基本位置.X, rectパネル基本位置.Y + 60);
					}
				}
				tx文字列?.tDraw2D(CDTXMania.app.Device, rectパネル基本位置.X, rectパネル基本位置.Y);
			}
			if (b次のフレームで入力中にする)
			{
				b入力中 = true;
				b次のフレームで入力中にする = false;
			}
			return 0;
		}

		private void t基本位置に応じて文字の描画範囲を設定する()
		{
			//prvf入力文字列.SetLayout(rectパネル基本位置);
		}

		private void t背景テクスチャを生成()
		{
			CDTXMania.tDisposeSafely(ref tx背景);
			using (Bitmap bitmap = new Bitmap(rectパネル基本位置.Width, rectパネル基本位置.Height))
			{
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					graphics.FillRectangle(Brushes.Black, 0, 0, bitmap.Width, bitmap.Height);
				}
				tx背景 = CDTXMania.tGenerateTexture(bitmap);
				tx背景.nTransparency = 192;
			}
		}

		private void t文字テクスチャを生成()
		{
			CDTXMania.tDisposeSafely(ref tx文字列);
			string text = str入力中文字列.Substring(0, nカーソル位置) + strIME入力中文字列 + str入力中文字列.Substring(nカーソル位置);
			Color fontColor = (strIME入力中文字列 != "") ? Color.Yellow : Color.White;
			if (text.Length >= nカーソル位置)
			{
				string text2 = text.Substring(0, nカーソル位置);
				using (prvf入力文字列.DrawPrivateFont(text2, Color.White, Color.Black))
				{
					nカーソル座標X = prvf入力文字列.RectStrings.Width + prvf入力文字列.RectStrings.X + 2;
					if (text2.EndsWith(" "))
					{
						double num = 0.0;
						int num2 = text2.Length - 1;
						while (text2[num2] == ' ')
						{
							num += 8.6956521739130448;
							num2--;
							if (num2 < 0)
							{
								break;
							}
						}
						nカーソル座標X += (int)num;
					}
					if (nカーソル座標X > rectパネル基本位置.Width)
					{
						nカーソル座標X = rectパネル基本位置.Width;
					}
				}
			}
			using (Bitmap bitmap2 = prvf入力文字列.DrawPrivateFont(text, fontColor, Color.Black))
			{
				tx文字列 = CDTXMania.tGenerateTexture(bitmap2);
			}
			if (ctカーソル != null)
			{
				ctカーソル.nCurrentValue = 1;
			}
		}

		public void t表示()
		{
			t表示(rectパネル基本位置.X, rectパネル基本位置.Y);
		}

		public void t表示(int x, int y)
		{
			t表示位置を変更する(x, y);
			b表示中 = true;
		}

		public void t非表示()
		{
			b表示中 = false;
		}

		public void t表示位置を変更する(int x, int y)
		{
			rectパネル基本位置.X = x;
			rectパネル基本位置.Y = y;
		}

		public void t表示位置を変更する(int x, int y, int w)
		{
			rectパネル基本位置.X = x;
			rectパネル基本位置.Y = y;
			rectパネル基本位置.Width = w;
			t背景テクスチャを生成();
			t基本位置に応じて文字の描画範囲を設定する();
		}

		public void t検索説明文を表示する設定にする()
		{
			b検索説明文表示 = true;
		}

		public void t検索説明文を表示しない設定にする()
		{
			b検索説明文表示 = false;
		}

		public void t入力を開始(bool bWithShow = true)
		{
			CDTXMania.app.cIMEHook.Focus();
			if (ctカーソル != null)
			{
				ctカーソル.tUpdateLoop();
				ctカーソル.nCurrentValue = 1;
			}
			b次のフレームで入力中にする = true;
			CDTXMania.app.textboxテキスト入力中 = this;
			if (bWithShow)
			{
				t表示();
				b入力終了時に非表示にする = true;
			}
		}

		private void t入力を確定して終了()
		{
			CDTXMania.app.Window.ActiveControl = null;
			str確定文字列 = str入力中文字列;
			str入力中文字列 = "";
			nカーソル位置 = 0;
			if (str確定文字列 != "")
			{
				L前回確定した文字列リスト.Add(str確定文字列);
				n前回確定した文字列リスト_参照カウンタ = L前回確定した文字列リスト.Count;
			}
			t文字テクスチャを生成();
			b入力終了直後 = true;
			b入力中 = false;
			CDTXMania.app.textboxテキスト入力中 = null;
			if (b入力終了時に非表示にする)
			{
				t非表示();
				b入力終了時に非表示にする = false;
			}
		}

		private void t入力を確定せずに終了()
		{
			str入力中文字列 = "";
			t入力を確定して終了();
		}

		public string str確定文字列を返す(bool b確定文字列をクリアする = false)
		{
			string result = str確定文字列;
			if (b確定文字列をクリアする)
			{
				str確定文字列 = "";
			}
			return result;
		}

		public void t1文字格納する(char ch)
		{
			if (!b入力中 || strIME入力中文字列 != "")
			{
				return;
			}
			switch (ch)
			{
			case '\b':
				if (str入力中文字列.Length > 0 && nカーソル位置 > 0)
				{
					str入力中文字列 = str入力中文字列.Substring(0, nカーソル位置 - 1) + str入力中文字列.Substring(nカーソル位置);
					nカーソル位置--;
				}
				break;
			default:
				t入力中文字列のカーソル位置に文字を挿入する(ch.ToString() ?? "");
				break;
			case '\u0003':
			case '\r':
			case '\u0016':
				break;
			}
			t文字テクスチャを生成();
		}

		private void t入力中文字列のカーソル位置に文字を挿入する(string str挿入文字列)
		{
			str入力中文字列 = str入力中文字列.Substring(0, nカーソル位置) + str挿入文字列 + str入力中文字列.Substring(nカーソル位置);
			nカーソル位置 += str挿入文字列.Length;
		}

		private string tクリップボードから文字列を取得する()
		{
			return tクリップボード文字列操作(b取得: true);
		}

		private void tクリップボードに文字列を設定する(string str)
		{
			tクリップボード文字列操作(b取得: false, str);
		}

		private string tクリップボード文字列操作(bool b取得, string str = "")
		{
			string ret = "";
			Thread thread = new Thread((ThreadStart)delegate
			{
				if (b取得)
				{
					ret = Clipboard.GetText();
				}
				else
				{
					Clipboard.SetText(str);
				}
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
			return ret;
		}
	}
}
