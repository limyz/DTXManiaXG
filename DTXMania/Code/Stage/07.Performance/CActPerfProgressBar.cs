using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal class CActPerfProgressBar : CActivity
	{
		
		public CActPerfProgressBar(bool bIsCalledFromOutsidePerformance = false)
		{
			this.b演奏画面以外からの呼び出し = bIsCalledFromOutsidePerformance;
			base.bNotActivated = true;
		}


		// CActivity 実装

		public override void OnActivate()
		{
			if (this.bActivated)
				return;

			this.ct登場用 = null;
			this.epartプレイ楽器 = EInstrumentPart.DRUMS;
			this.nWidth = 20;
			this.nHeight = 540; //1080;

			//
			this.pBarPosition[(int)EInstrumentPart.DRUMS] = new Point(855, 15);
			this.pBarPosition[(int)EInstrumentPart.GUITAR] = new Point(334, 85);
			this.pBarPosition[(int)EInstrumentPart.BASS] = new Point(1204, 85);
		
			//n区間分割数 = 54;
			this.nブロック最大数 = 10;
			this.n楽器毎のチップ数基準値.Drums = 1600;
			this.n楽器毎のチップ数基準値.Guitar = 800;
			this.n楽器毎のチップ数基準値.Bass = 800;

			try
			{
				for (EInstrumentPart ePart = EInstrumentPart.DRUMS; ePart <= EInstrumentPart.BASS; ePart++)
				{
					listProgressSection[(int)ePart] = new List<CProgressSection>();
					for (int i = 0; i < nSectionIntervalCount; i++)
					{
						listProgressSection[(int)ePart].Add(new CProgressSection());
					}
					if (!b演奏画面以外からの呼び出し && CDTXMania.ConfigIni.bInstrumentAvailable(ePart) && CDTXMania.DTX.bチップがある[(int)ePart])
					{
						int x = this.pBarPosition[(int)ePart].X;//(int)CDTXMania.Instance.ConfigIni.cdInstX[ePart][CDTXMania.Instance.ConfigIni.eActiveInst] + CDTXMania.Instance.ConfigIni.n楽器W_チップ倍率反映済(ePart);
						int y = 0;
						p表示位置[(int)ePart] = new Point(x, y);
					}
					else
					{
						p表示位置[(int)ePart] = new Point(0, 0);
					}
				}

				//Compute duration for each time-slice in L区間
				if (!b演奏画面以外からの呼び出し)
				{					
					nLastChipTime = CDTXMania.DTX.listChip[CDTXMania.DTX.listChip.Count - 1].nPlaybackTimeMs;
					foreach (CChip item in CDTXMania.DTX.listChip)
					{
						if (item.eInstrumentPart >= EInstrumentPart.DRUMS && item.eInstrumentPart <= EInstrumentPart.BASS)
						{
							int index = item.nPlaybackTimeMs * nSectionIntervalCount / nLastChipTime;
							listProgressSection[(int)item.eInstrumentPart][index].nChipCount++;
						}
					}
				}
				for (EInstrumentPart ePart2 = EInstrumentPart.DRUMS; ePart2 <= EInstrumentPart.BASS; ePart2++)
				{
					double num = (double)n楽器毎のチップ数基準値[(int)ePart2] / (double)nブロック最大数 / (double)nSectionIntervalCount;
					int y2 = nHeight;
					for (int j = 0; j < nSectionIntervalCount; j++)
					{
						CProgressSection c区間 = listProgressSection[(int)ePart2][j];
						int num2 = (int)((double)c区間.nChipCount / num) + 1;
						if (num2 > nブロック最大数)
						{
							num2 = nブロック最大数;
						}
						c区間.rectDrawingFrame.Y = (int)Math.Round((double)nHeight - ((double)j + 1.0) * (double)nHeight / (double)nSectionIntervalCount);
						c区間.rectDrawingFrame.Width = num2 * (nWidth / nブロック最大数);
						c区間.rectDrawingFrame.Height = y2 - c区間.rectDrawingFrame.Y;
						y2 = c区間.rectDrawingFrame.Y;
					}
				}
			}
			catch (Exception ex)
			{
				Trace.TraceError("プログレスバー活性化で例外が発生しました。");
				Trace.TraceError("例外 : " + ex.Message);
			}

			base.OnActivate();
		}

        public override void OnDeactivate()
        {
            if (!this.bNotActivated)
            {
				ct登場用 = null;
			}

            base.OnDeactivate();
        }

        public override void OnManagedCreateResources()
        {
			if (!base.bNotActivated)
			{
				tCreateBestProgressBarRecordTexture(CDTXMania.stageSongSelection.rChosenScore);
				tサイズが絡むテクスチャの生成();
				using (Bitmap bitmap = new Bitmap(64, 64))
				{
					using (Graphics graphics = Graphics.FromImage(bitmap))
					{
						graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, Color.White)), 0, 0, 64, 64);
					}
					tx灰 = CDTXMania.tGenerateTexture(bitmap); 
				}
				using (Bitmap bitmap2 = new Bitmap(64, 64))
				{
					using (Graphics graphics2 = Graphics.FromImage(bitmap2))
					{
						graphics2.FillRectangle(new SolidBrush(Color.FromArgb(192, Color.Yellow)), 0, 0, 64, 64);
					}
					tx黄 = CDTXMania.tGenerateTexture(bitmap2);
				}
				using (Bitmap bitmap3 = new Bitmap(64, 64))
				{
					using (Graphics graphics3 = Graphics.FromImage(bitmap3))
					{
						graphics3.FillRectangle(new SolidBrush(Color.FromArgb(192, Color.DeepSkyBlue)), 0, 0, 64, 64);
						graphics3.FillRectangle(Brushes.DeepSkyBlue, 0, 0, 64, 64);
					}
					tx青 = CDTXMania.tGenerateTexture(bitmap3);
				}

				txProgressBarBackgroundDrums = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_Drum_Progress_bg.png"));
				txProgressBarBackgroundGuitar = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_Guitar_Progress_bg.png"));
				
				base.OnManagedCreateResources();
			}				
        }

        public override void OnManagedReleaseResources()
        {
			if (!base.bNotActivated)
			{
				CDTXMania.tDisposeSafely(ref txパネル用);
				CDTXMania.tDisposeSafely(ref tx背景);
				CDTXMania.tDisposeSafely(ref tx縦線);
				CDTXMania.tDisposeSafely(ref tx進捗);
				CDTXMania.tDisposeSafely(ref tx灰);
				CDTXMania.tDisposeSafely(ref tx黄);
				CDTXMania.tDisposeSafely(ref tx青);

				CDTXMania.tDisposeSafely(ref this.txBestProgressBarRecord.Drums);
				CDTXMania.tDisposeSafely(ref this.txBestProgressBarRecord.Guitar);
				CDTXMania.tDisposeSafely(ref this.txBestProgressBarRecord.Bass);

				CDTXMania.tDisposeSafely(ref this.txProgressBarBackgroundDrums);
				CDTXMania.tDisposeSafely(ref this.txProgressBarBackgroundGuitar);

				base.OnManagedReleaseResources();
			}				
        }

        public override int OnUpdateAndDraw()
		{
			if (!base.bNotActivated)
			{
				//if (base.bJustStartedUpdate)
				//{
				//	//Put First time initialization code here
				//	base.bJustStartedUpdate = false;
				//}

				//Put drawing code here
				if (b演奏画面以外からの呼び出し)
				{
					if (base.bJustStartedUpdate)
					{
						ct登場用 = new CCounter(0, 100, 3, CDTXMania.Timer);
						base.bJustStartedUpdate = false;
					}
					CCounter obj = ct登場用;
					if (obj != null)
					{
						obj.tUpdate();
					}
				}
				for (EInstrumentPart ePart = EInstrumentPart.DRUMS; ePart <= EInstrumentPart.BASS; ePart++)
				{
					if ((!b演奏画面以外からの呼び出し && (b演奏画面以外からの呼び出し || 
						!CDTXMania.ConfigIni.bInstrumentAvailable(ePart) || 
						!CDTXMania.DTX.bチップがある[(int)ePart] || 
						(EDarkMode)CDTXMania.ConfigIni.eDark == EDarkMode.FULL)) || 
						(b演奏画面以外からの呼び出し && epartプレイ楽器 != ePart && (epartプレイ楽器 != EInstrumentPart.UNKNOWN || ePart != 0)))
					{
						continue;
					}
					int num = p表示位置[(int)ePart].X + (b演奏画面以外からの呼び出し ? 20 : 0);
					int num2 = p表示位置[(int)ePart].Y + (b演奏画面以外からの呼び出し ? 20 : 0) + this.pBarPosition[(int)ePart].Y;
					if (b演奏画面以外からの呼び出し)
					{
						num += (int)((double)(-60 - p表示位置[(int)ePart].X) * Math.Cos(Math.PI / 200.0 * (double)ct登場用.nCurrentValue));
					}
					if (b演奏画面以外からの呼び出し)
					{
						txパネル用.tDraw2D(CDTXMania.app.Device, num - 20, num2 - 20);
					}

					if(ePart == EInstrumentPart.DRUMS)
                    {
						txProgressBarBackgroundDrums.tDraw2D(CDTXMania.app.Device, num-2, num2 - 15);
                    }
                    else
                    {
						txProgressBarBackgroundGuitar.tDraw2D(CDTXMania.app.Device, num-2, num2 - 70);
					}

					tx背景.tDraw2D(CDTXMania.app.Device, num, num2);
					
					//Draw Best Record
					if (txBestProgressBarRecord[(int)ePart] != null)
					{
						txBestProgressBarRecord[(int)ePart].tDraw2D(CDTXMania.app.Device, num + 22, num2);
					}
					if (epartプレイ楽器 == EInstrumentPart.UNKNOWN)
					{
						continue;
					}
					if (!b演奏画面以外からの呼び出し)
					{
						tx縦線.tDraw2D(CDTXMania.app.Device, num + nWidth, num2);
						int num3 = (int)((double)((CTimerBase)CDTXMania.Timer).n現在時刻ms / (double)nLastChipTime * nHeightFactor);
						if (num3 > nHeight)
						{
							num3 = nHeight;
						}
						Rectangle rectangle = new Rectangle(0, 0, tx進捗.szTextureSize.Width, num3);
						num2 = nHeight - num3 + this.pBarPosition[(int)ePart].Y;
						tx進捗.tDraw2D(CDTXMania.app.Device, num, num2, rectangle);
					}
					for (int i = 0; i < nSectionIntervalCount; i++)
					{
						CProgressSection c区間 = listProgressSection[(int)ePart][i];
						num2 = p表示位置[(int)ePart].Y + (b演奏画面以外からの呼び出し ? 20 : 0) + c区間.rectDrawingFrame.Y + this.pBarPosition[(int)ePart].Y;
						//if (c区間.nChipCount <= 0)
						//{
						//	continue;
						//}
						if (!CDTXMania.ConfigIni.bIsAutoPlay(ePart) || b演奏画面以外からの呼び出し)
						{
							if ((i + 1) * nLastChipTime / nSectionIntervalCount - 1 > ((CTimerBase)CDTXMania.Timer).n現在時刻ms && !b演奏画面以外からの呼び出し)
							{
								tx灰.tDraw2D(CDTXMania.app.Device, num, num2, c区間.rectDrawingFrame);
							}
							else 
							{
                                if (!c区間.bIsAttempted)
                                {
									c区間.bIsAttempted = true;
                                }
								
								if(c区間.nChipCount > 0)
                                {
									if (c区間.nHitCount == c区間.nChipCount)
									{
										tx黄.tDraw2D(CDTXMania.app.Device, num, num2, c区間.rectDrawingFrame);
									}
									else
									{
										tx青.tDraw2D(CDTXMania.app.Device, num, num2, c区間.rectDrawingFrame);
									}
								}								
							} 
						}
						else
						{
							if (c区間.nChipCount > 0)
							{
								tx灰.tDraw2D(CDTXMania.app.Device, num, num2, c区間.rectDrawingFrame);
							}								
						}
					}
				
					
				}

			}
			return 0;
		}

		private void tCreateBestProgressBarRecordTexture(CScore cScore) 
		{
			if (cScore != null) 
			{
				for (EInstrumentPart ePart = EInstrumentPart.DRUMS; ePart <= EInstrumentPart.BASS; ePart++)
				{
					//Need to check for guitar/bass swap because Hi-Scores, Hi-Skills records are never swapped during Performance
					CTexture currTexture = null;
					txGenerateProgressBarLine(ref currTexture,
						cScore.SongInformation.progress[(int)CDTXMania.ConfigIni.GetFlipInst(ePart)]
						);

					txBestProgressBarRecord[(int)ePart] = currTexture;
				}
			}
			
		}

		private void txGenerateProgressBarLine(ref CTexture txProgressBarTexture, string strProgressBar)
		{
			int nBarWidth = 8;
			int nBarHeight = this.nHeight; //294;

			char[] arrProgress = strProgressBar.ToCharArray();
			if (arrProgress.Length == nSectionIntervalCount)
			{
				using (Bitmap tempBarBitmap = new Bitmap(nBarWidth, nBarHeight))
				{
					using (Graphics barGraphics = Graphics.FromImage(tempBarBitmap))
					{
						int nOffsetY = nBarHeight;
						for (int i = 0; i < nSectionIntervalCount; i++)
						{
							int nCurrentPosY = (int)Math.Round((double)nBarHeight - ((double)i + 1.0) * (double)nBarHeight / (double)CActPerfProgressBar.nSectionIntervalCount);
							int nCurrentSectionHeight = nOffsetY - nCurrentPosY;
							nOffsetY = nCurrentPosY;

							int nColorIndex = (int)(arrProgress[i] - '0');
							//Handle out of range
							if (nColorIndex < 0 || nColorIndex > 3)
							{
								nColorIndex = 0;
							}
							//Draw current section
							barGraphics.FillRectangle(new SolidBrush(this.clProgressBarColors[nColorIndex]), 2, nCurrentPosY, tempBarBitmap.Width - 4, nCurrentSectionHeight);
						}
						barGraphics.FillRectangle(new SolidBrush(Color.Gray), 0, 0, 2, tempBarBitmap.Height);
						barGraphics.FillRectangle(new SolidBrush(Color.Gray), 6, 0, 1, tempBarBitmap.Height);
					}
					txProgressBarTexture = CDTXMania.tGenerateTexture(tempBarBitmap);
				}
			}
            else
            {
				using (Bitmap tempBarBitmap = new Bitmap(nBarWidth, nBarHeight))
				{
					using (Graphics barGraphics = Graphics.FromImage(tempBarBitmap))
					{
						barGraphics.FillRectangle(new SolidBrush(this.clProgressBarColors[0]), 2, 0, tempBarBitmap.Width - 4, tempBarBitmap.Height);
						barGraphics.FillRectangle(new SolidBrush(Color.Gray), 0, 0, 2, tempBarBitmap.Height);
						barGraphics.FillRectangle(new SolidBrush(Color.Gray), 6, 0, 2, tempBarBitmap.Height);
					}
					txProgressBarTexture = CDTXMania.tGenerateTexture(tempBarBitmap);
				}
				
				//CDTXMania.t安全にDisposeする(ref txProgressBarTexture);
			}

		}

		public static void txGenerateProgressBarHelper(ref CTexture txRefProgressBarTexture, string strProgressBar, int nWidth, int nHeight, int nIntervals) 
		{
			if(strProgressBar == null)
            {
				return;
            }

			//
			Color[] clBarColors = new Color[4]
			{
				Color.Black,
				Color.DeepSkyBlue,
				Color.Yellow,
				Color.Yellow
			};

            char[] arrProgress = strProgressBar.ToCharArray();
            if (arrProgress.Length == nIntervals)
            {
                using (Bitmap tempBarBitmap = new Bitmap(nWidth, nHeight))
                {
                    using (Graphics barGraphics = Graphics.FromImage(tempBarBitmap))
                    {
                        int nOffsetY = nHeight;
                        for (int i = 0; i < nIntervals; i++)
                        {
                            int nCurrentPosY = (int)Math.Round((double)nHeight - ((double)i + 1.0) * (double)nHeight / (double)nIntervals);
                            int nCurrentSectionHeight = nOffsetY - nCurrentPosY;
                            nOffsetY = nCurrentPosY;

                            int nColorIndex = (int)(arrProgress[i] - '0');
                            //Handle out of range
                            if (nColorIndex < 0 || nColorIndex > 3)
                            {
                                nColorIndex = 0;
                            }
                            //Draw current section
                            barGraphics.FillRectangle(new SolidBrush(clBarColors[nColorIndex]), 0, nCurrentPosY, tempBarBitmap.Width, nCurrentSectionHeight);
                        }
                    }
                    txRefProgressBarTexture = CDTXMania.tGenerateTexture(tempBarBitmap);
                }
            }
			else
			{
				using (Bitmap tempBarBitmap = new Bitmap(nWidth, nHeight))
				{
					using (Graphics barGraphics = Graphics.FromImage(tempBarBitmap))
					{
						barGraphics.FillRectangle(new SolidBrush(clBarColors[0]), 0, 0, tempBarBitmap.Width, tempBarBitmap.Height);
					}
					txRefProgressBarTexture = CDTXMania.tGenerateTexture(tempBarBitmap);
				}

				//CDTXMania.t安全にDisposeする(ref txProgressBarTexture);
			}
		}

		private void tサイズが絡むテクスチャの生成()
		{
			CDTXMania.tDisposeSafely(ref txパネル用);
			if (b演奏画面以外からの呼び出し)
			{
				using (Bitmap bitmap = new Bitmap(nWidth + 40, nHeight + 40)) 
				{
					using (Graphics graphics = Graphics.FromImage(bitmap))
					{
						graphics.FillRectangle(new SolidBrush(Color.FromArgb(48, Color.White)), 0, 0, bitmap.Width, bitmap.Height);
					}
					txパネル用 = CDTXMania.tGenerateTexture(bitmap);
				} 				
			}
			CDTXMania.tDisposeSafely(ref tx背景);
			int num = 255; // (b演奏画面以外からの呼び出し ? 128 : ((int)CDTXMania.Instance.ConfigIni.nBGAlpha));
			using (Bitmap bitmap3 = new Bitmap(nWidth + ((!b演奏画面以外からの呼び出し) ? 2 : 0), nHeight))
			{
				using (Bitmap bitmap2 = new Bitmap(20, 20))
				{
					for (int i = 0; i < 20; i++)
					{
						for (int j = 0; j < 20; j++)
						{
							//_ = j / 5;
							//_ = i / 5;
							bitmap2.SetPixel(j, i, (i / 5 % 2 == 0) ? Color.FromArgb(num, 10, 10, 10) : Color.FromArgb(num, 14, 14, 14));
						}
					}
					using (TextureBrush brush = new TextureBrush(bitmap2)) 
					{
						using (Graphics graphics2 = Graphics.FromImage(bitmap3)) 
						{
							graphics2.FillRectangle(brush, 0, 0, bitmap3.Width, bitmap3.Height);
						}						
					} 
					
				}
				tx背景 = CDTXMania.tGenerateTexture(bitmap3);
			}
			CDTXMania.tDisposeSafely(ref tx縦線);
			using (Bitmap bitmap4 = new Bitmap(2, nHeight))
			{
				using (Graphics graphics3 = Graphics.FromImage(bitmap4))
				{
					graphics3.DrawLine(new Pen(Color.FromArgb((int)((double)num / 255.0 * 64.0), Color.White)), bitmap4.Width - 2, 0, bitmap4.Width - 2, bitmap4.Height);
					graphics3.DrawLine(new Pen(Color.FromArgb((int)((double)num / 255.0 * 32.0), Color.White)), bitmap4.Width - 1, 0, bitmap4.Width - 1, bitmap4.Height);
				}
				tx縦線 = CDTXMania.tGenerateTexture(bitmap4);
			}
			CDTXMania.tDisposeSafely(ref tx進捗);
			using (Bitmap bitmap5 = new Bitmap(nWidth, nHeight)) 
			{
				using (Graphics graphics4 = Graphics.FromImage(bitmap5))
				{
					graphics4.FillRectangle(new SolidBrush(Color.FromArgb(48, Color.White)), 0, 0, bitmap5.Width, bitmap5.Height);
					graphics4.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.White)), 0, 0, bitmap5.Width, 8);
				}
				tx進捗 = CDTXMania.tGenerateTexture(bitmap5);
			} 
			
		}

		public void Hit(EInstrumentPart inst, int nTime, EJudgement judge)
		{
			if (judge == EJudgement.Perfect || judge == EJudgement.Great || judge == EJudgement.Good)
			{
				listProgressSection[(int)inst][nTime * nSectionIntervalCount / nLastChipTime].nHitCount++;
			}
		}

		public string GetScoreIniString(EInstrumentPart inst)
		{
			string text = "";
			for (int i = 0; i < nSectionIntervalCount; i++)
			{
				CProgressSection c区間 = listProgressSection[(int)inst][i];
				//text += ((c区間.nChipCount > 0) ? ((c区間.nHitCount == c区間.nChipCount) ? "2" : "1") : "0");
				text += GetSectionChar(c区間);
			}
			return text;
		}

		private string GetSectionChar(CProgressSection cProgressSection) 
		{
			string ret = "0";
            if (cProgressSection.bIsAttempted)
            {
				if (cProgressSection.nChipCount > 0)
				{
					if (cProgressSection.nHitCount == cProgressSection.nChipCount)
					{
						ret = "2";
					}
                    else
                    {
						ret = "1";
                    }
				}
                else
                {
					//TODO: May need to check for nHitCount == nChipCount here too
					ret = "3"; 
                }
			}
			
			return ret;
		}

		public void t選択曲が変更された()
		{
			if (base.bNotActivated)
			{
				return;
			}
			for (EInstrumentPart ePart = EInstrumentPart.DRUMS; ePart <= EInstrumentPart.BASS; ePart++)
			{
				for (int i = 0; i < nSectionIntervalCount; i++)
				{
					CProgressSection c区間 = listProgressSection[(int)ePart][i];
					c区間.nChipCount = 0;
					c区間.nHitCount = 0;
					c区間.rectDrawingFrame.Width = 0;
				}
			}
			CScore r現在選択中のスコア = CDTXMania.stageSongSelection.rSelectedScore;
			if (r現在選択中のスコア != null)
			{
				//Use config.ini Drums / Guitar enabled to decide
				epartプレイ楽器 = EInstrumentPart.UNKNOWN;
				if (CDTXMania.ConfigIni.bDrumsEnabled)
                {
					epartプレイ楽器 = EInstrumentPart.DRUMS;
                }
                else
                {
					epartプレイ楽器 = EInstrumentPart.GUITAR;
                    if (CDTXMania.ConfigIni.bIsSwappedGuitarBass)
                    {
						epartプレイ楽器 = EInstrumentPart.BASS;
                    }
                }
				//epartプレイ楽器 = CDTXMania.stageSongSelection.tオートを参考にこれからプレイするであろうパートを推測する();
				//epartプレイ楽器 = CDTXMania.ConfigIni.GetFlipInst(epartプレイ楽器);
				if (epartプレイ楽器 >= EInstrumentPart.DRUMS && epartプレイ楽器 <= EInstrumentPart.BASS && r現在選択中のスコア.SongInformation.progress[(int)epartプレイ楽器] != null)
				{
					char[] arrプログレス = r現在選択中のスコア.SongInformation.progress[(int)epartプレイ楽器].ToCharArray();
					tプログレス配列から区間情報を設定する(arrプログレス);
				}
			}
		}

		public void t演奏記録から区間情報を設定する(STDGBVALUE<CScoreIni.CPerformanceEntry> stPerformanceEntry, EInstrumentPart eInstrumentPart)
		{
			for (EInstrumentPart ePart = EInstrumentPart.DRUMS; ePart <= EInstrumentPart.BASS; ePart++)
			{
				for (int i = 0; i < nSectionIntervalCount; i++)
				{
					CProgressSection c区間 = listProgressSection[(int)ePart][i];
					c区間.nChipCount = 0;
					c区間.nHitCount = 0;
					c区間.rectDrawingFrame.Width = 0;
				}
			}
			epartプレイ楽器 = eInstrumentPart;//CDTXMania.stageSongSelection.tオートを参考にこれからプレイするであろうパートを推測する();
			if (epartプレイ楽器 >= EInstrumentPart.DRUMS && epartプレイ楽器 <= EInstrumentPart.BASS && stPerformanceEntry[(int)epartプレイ楽器] != null)
			{
				char[] arrプログレス = stPerformanceEntry[(int)epartプレイ楽器].strProgress.ToCharArray();
				tプログレス配列から区間情報を設定する(arrプログレス);
			}
		}

		private void tプログレス配列から区間情報を設定する(char[] arrプログレス)
		{
			if (arrプログレス.Length == nSectionIntervalCount)
			{
				_ = nHeight / nSectionIntervalCount;
				for (int i = 0; i < nSectionIntervalCount; i++)
				{
					CProgressSection cSection = listProgressSection[(int)epartプレイ楽器][i];
					/* AL definition
					 * 
					 '0': No chips (Yellow)
					 '1': Has chips with some misses (Blue)
					 '2': Has chips with no misses (Yellow)

					to be changed to

					'0': Section Not attempted (Black)
					'1': Has chips with some misses (Blue)
					'2': Has chips with no misses (Yellow)
					'3': No chips aka Free Pass (Yellow)
					 
					 */
					cSection.bIsAttempted = arrプログレス[i] != '0';
					cSection.nChipCount = ((arrプログレス[i] == '1' || arrプログレス[i] == '2') ? 1 : 0);
					cSection.nHitCount = ((arrプログレス[i] == '2') ? 1 : 0);
					_ = (double)n楽器毎のチップ数基準値[(int)epartプレイ楽器] / (double)nブロック最大数 / (double)nSectionIntervalCount;
					int num = ((cSection.nChipCount > 0) ? nブロック最大数 : 0);
					cSection.rectDrawingFrame.Width = num * (nWidth / nブロック最大数);
				}
			}
		}

		public void t表示レイアウトを設定する(int 本体x, int 本体y, int グラフ部w, int グラフ部h)
		{
			if (!b演奏画面以外からの呼び出し)
			{
				return;
			}
			nWidth = グラフ部w;
			nHeight = グラフ部h;
			tサイズが絡むテクスチャの生成();
			for (EInstrumentPart ePart = EInstrumentPart.DRUMS; ePart <= EInstrumentPart.BASS; ePart++)
			{
				p表示位置[(int)ePart] = new Point(本体x, 本体y);
				int y = nHeight;
				for (int i = 0; i < nSectionIntervalCount; i++)
				{
					CProgressSection c区間 = listProgressSection[(int)ePart][i];
					c区間.rectDrawingFrame.Y = (int)Math.Round((double)nHeight - ((double)i + 1.0) * (double)nHeight / (double)nSectionIntervalCount);
					c区間.rectDrawingFrame.Height = y - c区間.rectDrawingFrame.Y;
					y = c区間.rectDrawingFrame.Y;
				}
			}
		}

		// Other

		#region [ private ]
		//-----------------
		public class CProgressSection
		{
			public int nChipCount;

			public int nHitCount;

			public bool bHasMistakes;

			//New
			public bool bIsAttempted;

			public Rectangle rectDrawingFrame;

			public CProgressSection()
			{
				nChipCount = 0;
				nHitCount = 0;
				bHasMistakes = true;
				bIsAttempted = false;
				rectDrawingFrame = new Rectangle(0, 0, 1, 1);
			}
		}

		private STDGBVALUE<List<CProgressSection>> listProgressSection;

		public static int nSectionIntervalCount = 64;

		private int nブロック最大数;

		private int nLastChipTime;

		private STDGBVALUE<int> n楽器毎のチップ数基準値;

		private CTexture txパネル用;

		private CTexture tx背景;

		private CTexture tx縦線;

		private CTexture tx進捗;

		private CTexture tx灰;

		private CTexture tx黄;

		private CTexture tx青;

		private STDGBVALUE<CTexture> txBestProgressBarRecord;

		private CTexture txProgressBarBackgroundDrums;

		private CTexture txProgressBarBackgroundGuitar;

		private STDGBVALUE<Point> p表示位置;

		private int nWidth;

		private int nHeight;

		private STDGBVALUE<Point> pBarPosition;

		//This value must match value of nHeight
		private readonly double nHeightFactor = 540.0;

		private readonly bool b演奏画面以外からの呼び出し;

		private EInstrumentPart epartプレイ楽器;

		private CCounter ct登場用;

		private Color[] clProgressBarColors = new Color[4]
		{
			Color.Black,
			Color.DeepSkyBlue,
			Color.Yellow,
			Color.Yellow
		};

		//-----------------
		#endregion
	}
}

