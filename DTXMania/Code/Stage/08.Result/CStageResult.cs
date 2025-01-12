using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using FDK;
using SharpDX.Direct3D9;
using DirectShowLib;

using SlimDXKey = SlimDX.DirectInput.Key;

namespace DTXMania
{
	internal class CStageResult : CStage
	{
		// プロパティ

		public STDGBVALUE<bool> bNewRecordSkill;
		public STDGBVALUE<bool> bNewRecordScore;
        public STDGBVALUE<bool> bNewRecordRank;
		public STDGBVALUE<float> fPerfect率;
		public STDGBVALUE<float> fGreat率;
		public STDGBVALUE<float> fGood率;
		public STDGBVALUE<float> fPoor率;
        public STDGBVALUE<float> fMiss率;
        public STDGBVALUE<bool> bAuto;        // #23596 10.11.16 add ikanick
                                                //        10.11.17 change (int to bool) ikanick
		public STDGBVALUE<int> nRankValue;
		public STDGBVALUE<int> nNbPerformances;
		public int n総合ランク値;
		public CChip[] rEmptyDrumChip;
		public STDGBVALUE<CScoreIni.CPerformanceEntry> stPerformanceEntry;
		public bool bIsTrainingMode;

		//Progress Bar temp variables
		public STDGBVALUE<string> strBestProgressBarRecord;
		public STDGBVALUE<string> strCurrProgressBarRecord;

		// コンストラクタ

		public CStageResult()
		{
			this.stPerformanceEntry.Drums = new CScoreIni.CPerformanceEntry();
			this.stPerformanceEntry.Guitar = new CScoreIni.CPerformanceEntry();
			this.stPerformanceEntry.Bass = new CScoreIni.CPerformanceEntry();
			this.rEmptyDrumChip = new CChip[ 10 ];
			this.n総合ランク値 = -1;
            this.nチャンネル0Atoレーン07 = new int[] { 1, 2, 3, 4, 5, 7, 6, 1, 8, 0, 9 };
			base.eStageID = CStage.EStage.Result;
			base.ePhaseID = CStage.EPhase.Common_DefaultState;
			base.bNotActivated = true;
			base.listChildActivities.Add( this.actResultImage = new CActResultImage() );
			base.listChildActivities.Add( this.actParameterPanel = new CActResultParameterPanel() );
			base.listChildActivities.Add( this.actRank = new CActResultRank() );
			base.listChildActivities.Add( this.actSongBar = new CActResultSongBar() );
			//base.listChildActivities.Add( this.actProgressBar = new CActPerfProgressBar(true) );
			base.listChildActivities.Add( this.actFI = new CActFIFOWhite() );
			base.listChildActivities.Add( this.actFO = new CActFIFOBlack() );
			base.listChildActivities.Add(this.actBackgroundVideoAVI = new CActSelectBackgroundAVI());
		}

		
		// CStage 実装

		public override void OnActivate()
		{
			Trace.TraceInformation( "結果ステージを活性化します。" );
			Trace.Indent();
			try
			{
				#region [ Initialize ]
				//---------------------
				this.eReturnValueWhenFadeOutCompleted = EReturnValue.Continue;
				this.bAnimationComplete = false;
				this.bIsCheckedWhetherResultScreenShouldSaveOrNot = false;				// #24609 2011.3.14 yyagi
				this.n最後に再生したHHのWAV番号 = -1;
				this.n最後に再生したHHのチャンネル番号 = 0;
				for( int i = 0; i < 3; i++ )
				{
					this.bNewRecordSkill[ i ] = false;
                    this.bNewRecordScore[ i ] = false;
                    this.bNewRecordRank[ i ] = false;
					//Initialize to empty string so that the Progress Bar texture can be drawn correctly
					this.strBestProgressBarRecord[i] = "";
					this.strCurrProgressBarRecord[i] = "";
				}
				//---------------------
				#endregion

				if (CDTXMania.ConfigIni.bScoreIniを出力する && !bIsTrainingMode && (CDTXMania.ConfigIni.bSaveScoreIfModifiedPlaySpeed || CDTXMania.ConfigIni.nPlaySpeed == 20))
				{
					#region [ Calculate results ]
					//---------------------
					for (int i = 0; i < 3; i++)
					{
						this.nRankValue[i] = -1;
						this.fPerfect率[i] = this.fGreat率[i] = this.fGood率[i] = this.fPoor率[i] = this.fMiss率[i] = 0.0f;  // #28500 2011.5.24 yyagi
						if ((((i != 0) || (CDTXMania.DTX.bチップがある.Drums && !CDTXMania.ConfigIni.bGuitarRevolutionMode)) &&
							((i != 1) || (CDTXMania.DTX.bチップがある.Guitar && CDTXMania.ConfigIni.bGuitarRevolutionMode))) &&
							((i != 2) || (CDTXMania.DTX.bチップがある.Bass && CDTXMania.ConfigIni.bGuitarRevolutionMode)))
						{
							CScoreIni.CPerformanceEntry part = this.stPerformanceEntry[i];
							bool bIsAutoPlay = true;
							switch (i)
							{
								case 0:
									bIsAutoPlay = CDTXMania.ConfigIni.bAllDrumsAreAutoPlay;
									break;

								case 1:
									bIsAutoPlay = CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay;
									break;

								case 2:
									bIsAutoPlay = CDTXMania.ConfigIni.bAllBassAreAutoPlay;
									break;
							}

							this.fPerfect率[i] = bIsAutoPlay ? 0f : ((100f * part.nPerfectCount) / ((float)part.nTotalChipsCount));
							this.fGreat率[i] = bIsAutoPlay ? 0f : ((100f * part.nGreatCount) / ((float)part.nTotalChipsCount));
							this.fGood率[i] = bIsAutoPlay ? 0f : ((100f * part.nGoodCount) / ((float)part.nTotalChipsCount));
							this.fPoor率[i] = bIsAutoPlay ? 0f : ((100f * part.nPoorCount) / ((float)part.nTotalChipsCount));
							this.fMiss率[i] = bIsAutoPlay ? 0f : ((100f * part.nMissCount) / ((float)part.nTotalChipsCount));
							this.bAuto[i] = bIsAutoPlay; // #23596 10.11.16 add ikanick そのパートがオートなら1
														 //        10.11.17 change (int to bool) ikanick
														 //18072020: Change first condition check to 1, XG mode is 1, not 0. Fisyher
							if (CDTXMania.ConfigIni.nSkillMode == 1)
							{
								this.nRankValue[i] = CScoreIni.tCalculateRank(part);
							}
							else if (CDTXMania.ConfigIni.nSkillMode == 0)
							{
								this.nRankValue[i] = CScoreIni.tCalculateRankOld(part);
							}

							//Save progress bar records
							CScore cScore = CDTXMania.stageSongSelection.rChosenScore;
							this.strBestProgressBarRecord[i] = cScore.SongInformation.progress[i];
							//May not need to save this...
							this.strCurrProgressBarRecord[i] = this.stPerformanceEntry[i].strProgress;
						}
					}
					this.n総合ランク値 = CScoreIni.tCalculateOverallRankValue(this.stPerformanceEntry.Drums, this.stPerformanceEntry.Guitar, this.stPerformanceEntry.Bass);
					//---------------------
					#endregion

					#region [ Write .score.ini ]
					//---------------------
					string str = CDTXMania.DTX.strファイル名の絶対パス + ".score.ini";
					CScoreIni ini = new CScoreIni(str);

					bool[] b今までにフルコンボしたことがある = new bool[] { false, false, false };
					if (!CDTXMania.ConfigIni.bAllDrumsAreAutoPlay || !CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay || !CDTXMania.ConfigIni.bAllBassAreAutoPlay)
					{
						for (int i = 0; i < 3; i++)
						{

							// フルコンボチェックならびに新記録ランクチェックは、ini.Record[] が、スコアチェックや演奏型スキルチェックの IF 内で書き直されてしまうよりも前に行う。(2010.9.10)

							b今までにフルコンボしたことがある[i] = ini.stSection[i * 2].bIsFullCombo | ini.stSection[i * 2 + 1].bIsFullCombo;

							#region [deleted by #24459]
							//		if( this.nRankValue[ i ] <= CScoreIni.tCalculateRank( ini.stSection[ ( i * 2 ) + 1 ] ) )
							//		{
							//			this.bNewRecordRank[ i ] = true;
							//		}
							#endregion
							// #24459 上記の条件だと[HiSkill.***]でのランクしかチェックしていないので、BestRankと比較するよう変更。
							if (this.nRankValue[i] >= 0 && ini.stFile.BestRank[i] > this.nRankValue[i])     // #24459 2011.3.1 yyagi update BestRank
							{
								this.bNewRecordRank[i] = true;
								ini.stFile.BestRank[i] = this.nRankValue[i];
							}

							// 新記録スコアチェック
							if (this.stPerformanceEntry[i].nスコア > ini.stSection[i * 2].nスコア)
							{
								this.bNewRecordScore[i] = true;
								ini.stSection[i * 2] = this.stPerformanceEntry[i];
								this.SaveGhost(i * 2); // #35411 chnmr0 add
							}

							// 新記録スキルチェック
							if ((this.stPerformanceEntry[i].dbPerformanceSkill > ini.stSection[(i * 2) + 1].dbPerformanceSkill) && !this.bAuto[i])
							{
								this.bNewRecordSkill[i] = true;
								ini.stSection[(i * 2) + 1] = this.stPerformanceEntry[i];
								this.SaveGhost((i * 2) + 1); // #35411 chnmr0 add
							}

							// ラストプレイ #23595 2011.1.9 ikanick
							// オートじゃなければプレイ結果を書き込む
							if (this.bAuto[i] == false)
							{
								ini.stSection[i + 6] = this.stPerformanceEntry[i];
								this.SaveGhost(i + 6); // #35411 chnmr0 add
							}

							// #23596 10.11.16 add ikanick オートじゃないならクリア回数を1増やす
							//        11.02.05 bAuto to tGetIsUpdateNeeded use      ikanick
							bool[] b更新が必要か否か = new bool[3];
							CScoreIni.tGetIsUpdateNeeded(out b更新が必要か否か[0], out b更新が必要か否か[1], out b更新が必要か否か[2]);

							if (b更新が必要か否か[i])
							{
								switch (i)
								{
									case 0:
										ini.stFile.ClearCountDrums++;
										break;
									case 1:
										ini.stFile.ClearCountGuitar++;
										break;
									case 2:
										ini.stFile.ClearCountBass++;
										break;
									default:
										throw new Exception("クリア回数増加のk(0-2)が範囲外です。");
								}
							}

							//---------------------------------------------------------------------/
						}
						// Already checked
						//if (CDTXMania.ConfigIni.bScoreIniを出力する)
						{
							ini.tExport(str);
						}
					}
					//---------------------
					#endregion

					#region [ Update nb of performance #24281 2011.1.30 yyagi]
					this.nNbPerformances.Drums = ini.stFile.PlayCountDrums;
					this.nNbPerformances.Guitar = ini.stFile.PlayCountGuitar;
					this.nNbPerformances.Bass = ini.stFile.PlayCountBass;
					#endregion
					#region [ Update score information on Song Selection screen ]
					//---------------------
					if (!CDTXMania.bCompactMode)
					{
						CScore cScore = CDTXMania.stageSongSelection.rChosenScore;
						bool[] b更新が必要か否か = new bool[3];
						CScoreIni.tGetIsUpdateNeeded(out b更新が必要か否か[0], out b更新が必要か否か[1], out b更新が必要か否か[2]);
						for (int m = 0; m < 3; m++)
						{
							if (b更新が必要か否か[m])
							{
								// FullCombo した記録を FullCombo なしで超えた場合、FullCombo マークが消えてしまう。
								// → FullCombo は、最新記録と関係なく、一度達成したらずっとつくようにする。(2010.9.11)
								cScore.SongInformation.FullCombo[m] = this.stPerformanceEntry[m].bIsFullCombo | b今までにフルコンボしたことがある[m];

								if (this.bNewRecordSkill[m])
								{
									cScore.SongInformation.HighSkill[m] = this.stPerformanceEntry[m].dbPerformanceSkill;
									// New Song Progress for new skill record
									cScore.SongInformation.progress[m] = this.stPerformanceEntry[m].strProgress;
								}

								if (this.bNewRecordRank[m])
								{
									cScore.SongInformation.BestRank[m] = this.nRankValue[m];
								}

								//Check if Progress record existed or not; if not, update anyway
								if(CScoreIni.tProgressBarLength(cScore.SongInformation.progress[m]) == 0)
                                {
									cScore.SongInformation.progress[m] = this.stPerformanceEntry[m].strProgress;
								}
							}
						}
					}
					//---------------------
					#endregion
				}

				base.OnActivate();
				//this.actProgressBar.t表示レイアウトを設定する(180, 540, 20, 460);
				//this.actProgressBar.t演奏記録から区間情報を設定する(st演奏記録);
			}
			finally
			{
				Trace.TraceInformation( "結果ステージの活性化を完了しました。" );
				Trace.Unindent();
			}
		}
        //fork
        // #35411 chnmr0 add
        private void SaveGhost(int sectionIndex)
        {
            //return; //2015.12.31 kairera0467 以下封印

            STDGBVALUE<bool> saveCond = new STDGBVALUE<bool>();
            saveCond.Drums = true;
            saveCond.Guitar = true;
            saveCond.Bass = true;
            
            foreach( CChip chip in CDTXMania.DTX.listChip )
            {
                if( chip.bIsAutoPlayed )
                {
					if (chip.nChannelNumber != EChannel.Guitar_Wailing && chip.nChannelNumber != EChannel.Bass_Wailing) // Guitar/Bass Wailing は OK
					{
						saveCond[(int)(chip.eInstrumentPart)] = false;
					}
                }
            }
            for(int instIndex = 0; instIndex < 3; ++instIndex)
            {
                saveCond[instIndex] &= CDTXMania.listAutoGhostLag.Drums == null;
            }

            string directory = CDTXMania.DTX.strFolderName;
            string filename = CDTXMania.DTX.strファイル名 + ".";
            EInstrumentPart inst = EInstrumentPart.UNKNOWN;

            if ( sectionIndex == 0 )
            {
                filename += "hiscore.dr.ghost";
                inst = EInstrumentPart.DRUMS;
            }
            else if (sectionIndex == 1 )
            {
                filename += "hiskill.dr.ghost";
                inst = EInstrumentPart.DRUMS;
            }
            if (sectionIndex == 2 )
            {
                filename += "hiscore.gt.ghost";
                inst = EInstrumentPart.GUITAR;
            }
            else if (sectionIndex == 3 )
            {
                filename += "hiskill.gt.ghost";
                inst = EInstrumentPart.GUITAR;
            }
            if (sectionIndex == 4 )
            {
                filename += "hiscore.bs.ghost";
                inst = EInstrumentPart.BASS;
            }
            else if (sectionIndex == 5)
            {
                filename += "hiskill.bs.ghost";
                inst = EInstrumentPart.BASS;
            }
            else if (sectionIndex == 6)
            {
                filename += "lastplay.dr.ghost";
                inst = EInstrumentPart.DRUMS;
            }
            else if (sectionIndex == 7 )
            {
                filename += "lastplay.gt.ghost";
                inst = EInstrumentPart.GUITAR;
            }
            else if (sectionIndex == 8)
            {
                filename += "lastplay.bs.ghost";
                inst = EInstrumentPart.BASS;
            }

            if (inst == EInstrumentPart.UNKNOWN)
            {
                return;
            }

            int cnt = 0;
            foreach (DTXMania.CChip chip in CDTXMania.DTX.listChip)
            {
                if (chip.eInstrumentPart == inst)
                {
                    ++cnt;
                }
            }

            if( saveCond[(int)inst] )
            //if(false)
            {
                using (FileStream fs = new FileStream(directory + "\\" + filename, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write((Int32)cnt);
                        foreach (DTXMania.CChip chip in CDTXMania.DTX.listChip)
                        {
                            if (chip.eInstrumentPart == inst)
                            {
                            	// -128 ms から 127 ms までのラグしか保存しない
                            	// その範囲を超えているラグはクランプ
								// ラグデータの 上位８ビットでそのチップの前でギター空打ちBADがあったことを示す
								int lag = chip.nLag;
								if (lag < -128)
								{
									lag = -128;
								}
								if (lag > 127)
								{
									lag = 127;
								}
								byte lower = (byte)(lag + 128);
								int upper = chip.nCurrentComboForGhost == 0 ? 1 : 0;
								bw.Write((short)( (upper<<8) | lower));
                            }
                        }
                    }
                }

                //Ver.K追加 演奏結果の記録
                //CScoreIni.CPerformanceEntry cScoreData;
                //cScoreData = this.stPerformanceEntry[ (int)inst ];
                //using (FileStream fs = new FileStream(directory + "\\" + filename + ".score", FileMode.Create, FileAccess.Write))
                //{
                //    using (StreamWriter sw = new StreamWriter(fs))
                //    {
                //        sw.WriteLine( "Score=" + cScoreData.nスコア );
                //        sw.WriteLine( "PlaySkill=" + cScoreData.dbPerformanceSkill );
                //        sw.WriteLine( "Skill=" + cScoreData.dbGameSkill );
                //        sw.WriteLine( "Perfect=" + cScoreData.nPerfectCount_ExclAuto );
                //        sw.WriteLine( "Great=" + cScoreData.nGreatCount_ExclAuto );
                //        sw.WriteLine( "Good=" + cScoreData.nGoodCount_ExclAuto );
                //        sw.WriteLine( "Poor=" + cScoreData.nPoorCount_ExclAuto );
                //        sw.WriteLine( "Miss=" + cScoreData.nMissCount_ExclAuto );
                //        sw.WriteLine( "MaxCombo=" + cScoreData.nMaxCombo );
                //    }
                //}
            }
        }
		public override void OnDeactivate()
		{
			if( this.rResultSound != null )
			{
				CDTXMania.SoundManager.tDiscard( this.rResultSound );
				this.rResultSound = null;
			}

			if (this.rBackgroundVideoAVI != null)
			{
				this.rBackgroundVideoAVI.Dispose();
				this.rBackgroundVideoAVI = null;
			}

			base.OnDeactivate();
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
				//
				this.rBackgroundVideoAVI = new CDTX.CAVI(1290, CSkin.Path(@"Graphics\8_background.mp4"), "", 20.0);
				this.rBackgroundVideoAVI.OnDeviceCreated();
				if (rBackgroundVideoAVI.avi != null)
				{					
					this.actBackgroundVideoAVI.bLoop = true;
					this.actBackgroundVideoAVI.Start(EChannel.MovieFull, rBackgroundVideoAVI, 0, -1);
					Trace.TraceInformation("Playing Background video for Result Screen");
				}

				//this.ds背景動画 = CDTXMania.t失敗してもスキップ可能なDirectShowを生成する(CSkin.Path(@"Graphics\8_background.mp4"), CDTXMania.app.WindowHandle, true);
				this.txBackground = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\8_background.jpg" ) );
                switch (CDTXMania.stageResult.n総合ランク値)
                {
                    case 0:
                        if (File.Exists(CSkin.Path(@"Graphics\8_background rankSS.png")))
                        {
                            this.txBackground = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\8_background rankSS.png"));
                        }
                        break;
                    case 1:
                        if (File.Exists(CSkin.Path(@"Graphics\8_background rankS.png")))
                        {
                            this.txBackground = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\8_background rankS.png"));
                        }
                        break;
                    case 2:
                        if (File.Exists(CSkin.Path(@"Graphics\8_background rankA.png")))
                        {
                            this.txBackground = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\8_background rankA.png"));
                        }
                        break;
                    case 3:
                        if (File.Exists(CSkin.Path(@"Graphics\8_background rankB.png")))
                        {
                            this.txBackground = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\8_background rankB.png"));
                        }
                        break;
                    case 4:
                        if (File.Exists(CSkin.Path(@"Graphics\8_background rankC.png")))
                        {
                            this.txBackground = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\8_background rankC.png"));
                        }
                        break;
                    case 5:
                        if (File.Exists(CSkin.Path(@"Graphics\8_background rankD.png")))
                        {
                            this.txBackground = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\8_background rankD.png"));
                        }
                        break;
                    case 6:
                    case 99:
                        if (File.Exists(CSkin.Path(@"Graphics\8_background rankE.png")))
                        {
                            this.txBackground = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\8_background rankE.png"));
                        }
                        break;
                }
				this.txTopPanel = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\8_header panel.png" ), true );
				this.txBottomPanel = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\8_footer panel.png" ), true );
				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if( !base.bNotActivated )
			{
				if( this.ct登場用 != null )
				{
					this.ct登場用 = null;
				}
				//
				if(this.ctPlayNewRecord != null)
                {
					this.ctPlayNewRecord = null;
                }

				actBackgroundVideoAVI.Stop();

				//CDTXMania.t安全にDisposeする( ref this.ds背景動画 );
				CDTXMania.tReleaseTexture( ref this.txBackground );
				CDTXMania.tReleaseTexture( ref this.txTopPanel );
				CDTXMania.tReleaseTexture( ref this.txBottomPanel );
				base.OnManagedReleaseResources();
			}
		}
		public override int OnUpdateAndDraw()
		{
			if( !base.bNotActivated )
			{
				int num;
				if( base.bJustStartedUpdate )
				{
					this.ct登場用 = new CCounter( 0, 100, 5, CDTXMania.Timer );

					//Check result to select the correct sound to play
					int l_outputSoundEnum = 0; //0: Stage Clear 1: FC 2: EXC
					bool l_newRecord = false;
					for (int i = 0; i < 3; i++)
					{
						if ((((i != 0) || (CDTXMania.DTX.bチップがある.Drums && !CDTXMania.ConfigIni.bGuitarRevolutionMode)) &&
							((i != 1) || (CDTXMania.DTX.bチップがある.Guitar && CDTXMania.ConfigIni.bGuitarRevolutionMode))) &&
							((i != 2) || (CDTXMania.DTX.bチップがある.Bass && CDTXMania.ConfigIni.bGuitarRevolutionMode)))
						{ 
							if(bAuto[i] == false)
                            {
								if(fPerfect率[i] == 100.0)
                                {
									l_outputSoundEnum = 2; //Excellent
								}
								else if(fPoor率[i] == 0.0 && fMiss率[i] == 0.0)
                                {
									l_outputSoundEnum = 1; //Full Combo
								}
                            }

							if(this.bNewRecordSkill[i] == true)
                            {
								l_newRecord = true;
							}
						}
					}

					//Play the corresponding sound
					if(l_outputSoundEnum == 1)
                    {
						CDTXMania.Skin.soundFullCombo.tPlay();
					}
					else if(l_outputSoundEnum == 2)
                    {
						CDTXMania.Skin.soundExcellent.tPlay();
					}
					else
                    {
						CDTXMania.Skin.soundStageClear.tPlay();
					}

					//Create the delay timer of 150 x 10 = 1500 ms to play New Record
					if(l_newRecord)
                    {
						this.ctPlayNewRecord = new CCounter(0, 150, 10, CDTXMania.Timer);
					}
						
                    this.actFI.tStartFadeIn(false);
					base.ePhaseID = CStage.EPhase.Common_FadeIn;
					base.bJustStartedUpdate = false;
				}

				
				//Draw Background video  via CActPerfAVI methods
				this.actBackgroundVideoAVI.tUpdateAndDraw();

				//if ( this.ds背景動画 != null )
    //            {
    //                this.ds背景動画.t再生開始();
    //                this.ds背景動画.MediaSeeking.GetPositions(out this.lDshowPosition, out this.lStopPosition);
    //                this.ds背景動画.bループ再生 = true;
                    
    //                if (this.lDshowPosition == this.lStopPosition)
    //                {
    //                    this.ds背景動画.MediaSeeking.SetPositions(
    //                    DsLong.FromInt64((long)(0)),
    //                    AMSeekingSeekingFlags.AbsolutePositioning,
    //                    0,
    //                    AMSeekingSeekingFlags.NoPositioning);
    //                }
                    
    //                this.ds背景動画.t現時点における最新のスナップイメージをTextureに転写する( this.txBackground );
    //            }

				this.bAnimationComplete = true;
				if( this.ct登場用.bInProgress )
				{
					this.ct登場用.tUpdate();
					if( this.ct登場用.bReachedEndValue )
					{
						this.ct登場用.tStop();
					}
					else
					{
						this.bAnimationComplete = false;
					}
				}

				//Play new record if available
				if(this.ctPlayNewRecord != null && this.ctPlayNewRecord.bInProgress)
                {
					this.ctPlayNewRecord.tUpdate();
					if (this.ctPlayNewRecord.bReachedEndValue)
					{
						CDTXMania.Skin.soundNewRecord.tPlay();
						this.ctPlayNewRecord.tStop();
					}
				}

				// 描画

				if( this.txBackground != null && this.rBackgroundVideoAVI.avi == null)
				{
                    this.txBackground.tDraw2D( CDTXMania.app.Device, 0, 0 );
				}

				if( this.ct登場用.bInProgress && ( this.txTopPanel != null ) )
				{
					double num2 = ( (double) this.ct登場用.nCurrentValue ) / 100.0;
					double num3 = Math.Sin( Math.PI / 2 * num2 );
					num = ( (int) ( this.txTopPanel.szImageSize.Height * num3 ) ) - this.txTopPanel.szImageSize.Height;
				}
				else
				{
					num = 0;
				}
				if( this.txTopPanel != null )
				{
					this.txTopPanel.tDraw2D( CDTXMania.app.Device, 0, num );
				}
				if( this.txBottomPanel != null )
				{
					this.txBottomPanel.tDraw2D( CDTXMania.app.Device, 0, 720 - this.txBottomPanel.szImageSize.Height );
				}
                if( this.actResultImage.OnUpdateAndDraw() == 0 )
				{
					this.bAnimationComplete = false;
				}
				if ( this.actParameterPanel.OnUpdateAndDraw() == 0 )
				{
					this.bAnimationComplete = false;
				}
                if (this.actRank.OnUpdateAndDraw() == 0)
                {
                    this.bAnimationComplete = false;
                }
				if( base.ePhaseID == CStage.EPhase.Common_FadeIn )
				{
					if( this.actFI.OnUpdateAndDraw() != 0 )
					{
						base.ePhaseID = CStage.EPhase.Common_DefaultState;
					}
				}
				else if( ( base.ePhaseID == CStage.EPhase.Common_FadeOut ) )			//&& ( this.actFO.OnUpdateAndDraw() != 0 ) )
				{
					return (int) this.eReturnValueWhenFadeOutCompleted;
				}
				#region [ #24609 2011.3.14 yyagi ランク更新or演奏型スキル更新時、リザルト画像をpngで保存する ]
				if ( this.bAnimationComplete == true && this.bIsCheckedWhetherResultScreenShouldSaveOrNot == false	// #24609 2011.3.14 yyagi; to save result screen in case BestRank or HiSkill.
					&& CDTXMania.ConfigIni.bScoreIniを出力する
					&& CDTXMania.ConfigIni.bIsAutoResultCapture)												// #25399 2011.6.9 yyagi
				{
					CheckAndSaveResultScreen(true);
					this.bIsCheckedWhetherResultScreenShouldSaveOrNot = true;
				}
				#endregion

				// キー入力
				if( CDTXMania.ConfigIni.bドラム打音を発声する && CDTXMania.ConfigIni.bDrumsEnabled )
				{
					for( int i = 0; i < 11; i++ )
					{
						List<STInputEvent> events = CDTXMania.Pad.GetEvents( EInstrumentPart.DRUMS, (EPad) i );
						if( ( events != null ) && ( events.Count > 0 ) )
						{
							foreach( STInputEvent event2 in events )
							{
								if( !event2.b押された )
								{
									continue;
								}
								CChip rChip = this.rEmptyDrumChip[ i ];
								if( rChip == null )
								{
									switch( ( (EPad) i ) )
									{
										case EPad.HH:
											rChip = this.rEmptyDrumChip[ 7 ];
											if( rChip == null )
											{
												rChip = this.rEmptyDrumChip[ 9 ];
											}
											break;

										case EPad.FT:
											rChip = this.rEmptyDrumChip[ 4 ];
											break;

										case EPad.CY:
											rChip = this.rEmptyDrumChip[ 8 ];
											break;

										case EPad.HHO:
											rChip = this.rEmptyDrumChip[ 0 ];
											if( rChip == null )
											{
												rChip = this.rEmptyDrumChip[ 9 ];
											}
											break;

										case EPad.RD:
											rChip = this.rEmptyDrumChip[ 6 ];
											break;

										case EPad.LC:
											rChip = this.rEmptyDrumChip[ 0 ];
											if( rChip == null )
											{
												rChip = this.rEmptyDrumChip[ 7 ];
											}
											break;
									}
								}
								if( ( ( rChip != null ) && ( rChip.nChannelNumber >= EChannel.HiHatClose ) ) && ( rChip.nChannelNumber <= EChannel.LeftPedal ) )
								{
									int nLane = this.nチャンネル0Atoレーン07[ rChip.nChannelNumber - EChannel.HiHatClose ];
									if( ( nLane == 1 ) && ( ( rChip.nChannelNumber == EChannel.HiHatClose ) || ( ( rChip.nChannelNumber == EChannel.HiHatOpen ) && ( this.n最後に再生したHHのチャンネル番号 != EChannel.HiHatOpen ) ) ) )
									{
										CDTXMania.DTX.tStopPlayingWav( this.n最後に再生したHHのWAV番号 );
										this.n最後に再生したHHのWAV番号 = rChip.nIntegerValue_InternalNumber;
										this.n最後に再生したHHのチャンネル番号 = rChip.nChannelNumber;
									}
									CDTXMania.DTX.tPlayChip( rChip, CDTXMania.Timer.nシステム時刻, nLane, CDTXMania.ConfigIni.n手動再生音量, CDTXMania.ConfigIni.b演奏音を強調する.Drums );
								}
							}
						}
					}
				}
				if( ( ( CDTXMania.Pad.bPressedDGB( EPad.CY ) || CDTXMania.Pad.bPressed( EInstrumentPart.DRUMS, EPad.RD ) ) || ( CDTXMania.Pad.bPressed( EInstrumentPart.DRUMS, EPad.LC ) || CDTXMania.InputManager.Keyboard.bKeyPressed( (int)SlimDXKey.Return ) ) ) && !this.bAnimationComplete )
				{
					this.actFI.tフェードイン完了();					// #25406 2011.6.9 yyagi
					this.actResultImage.tアニメを完了させる();
					this.actParameterPanel.tアニメを完了させる();
					this.actRank.tアニメを完了させる();
					this.ct登場用.tStop();
				}
				#region [ #24609 2011.4.7 yyagi リザルト画面で[F12]を押下すると、リザルト画像をpngで保存する機能は、CDTXManiaに移管。 ]
//					if ( CDTXMania.InputManager.Keyboard.bKeyPressed( (int) SlimDXKey.F12 ) &&
//						CDTXMania.ConfigIni.bScoreIniを出力する )
//					{
//						CheckAndSaveResultScreen(false);
//						this.bIsCheckedWhetherResultScreenShouldSaveOrNot = true;
//					}
				#endregion
				if ( base.ePhaseID == CStage.EPhase.Common_DefaultState )
				{
					if ( CDTXMania.InputManager.Keyboard.bKeyPressed( (int)SlimDXKey.Escape ) )
					{
						CDTXMania.Skin.soundCancel.tPlay();
						this.actFO.tStartFadeOut();
						base.ePhaseID = CStage.EPhase.Common_FadeOut;
						this.eReturnValueWhenFadeOutCompleted = EReturnValue.Complete;
					}
					if ( ( ( CDTXMania.Pad.bPressedDGB( EPad.CY ) || CDTXMania.Pad.bPressed( EInstrumentPart.DRUMS, EPad.RD ) ) || ( CDTXMania.Pad.bPressed( EInstrumentPart.DRUMS, EPad.LC ) || CDTXMania.InputManager.Keyboard.bKeyPressed( (int) SlimDXKey.Return ) ) ) && this.bAnimationComplete )
					{
						CDTXMania.Skin.soundCancel.tPlay();
						base.ePhaseID = CStage.EPhase.Common_FadeOut;
						this.eReturnValueWhenFadeOutCompleted = EReturnValue.Complete;
					}
				}
			}
			return 0;
		}

		public enum EReturnValue : int
		{
			Continue,
			Complete
		}


		// Other

		#region [ private ]
		//-----------------
		private CCounter ct登場用;
		//New Counter
		private CCounter ctPlayNewRecord;
		private EReturnValue eReturnValueWhenFadeOutCompleted;  // eフェードアウト完了時の戻り値
		private CActFIFOWhite actFI;
		private CActFIFOBlack actFO;
		private CActResultParameterPanel actParameterPanel;
		private CActResultRank actRank;
		private CActResultImage actResultImage;
		private CActResultSongBar actSongBar;		
		//private CActPerfProgressBar actProgressBar;
		private bool bAnimationComplete;  // bアニメが完了
		private bool bIsCheckedWhetherResultScreenShouldSaveOrNot;				// #24509 2011.3.14 yyagi
		private readonly int[] nチャンネル0Atoレーン07;
		private int n最後に再生したHHのWAV番号;
		private EChannel n最後に再生したHHのチャンネル番号;
		private CSound rResultSound;
		private CTexture txBottomPanel;  // tx下部パネル
		private CTexture txTopPanel;  // tx上部パネル
		private CTexture txBackground;  // tx背景
		//Copy from CStagePerfCommonScreen
		public STDGBVALUE<CStagePerfCommonScreen.CLAGTIMINGHITCOUNT> nTimingHitCount;

		//private CDirectShow ds背景動画;
		private long lDshowPosition;
        private long lStopPosition;

		private readonly CActSelectBackgroundAVI actBackgroundVideoAVI;
		private CDTX.CAVI rBackgroundVideoAVI;

		#region [ #24609 リザルト画像をpngで保存する ]		// #24609 2011.3.14 yyagi; to save result screen in case BestRank or HiSkill.
		/// <summary>
		/// リザルト画像のキャプチャと保存。
		/// 自動保存モード時は、ランク更新or演奏型スキル更新時に自動保存。
		/// 手動保存モード時は、ランクに依らず保存。
		/// </summary>
		/// <param name="bIsAutoSave">true=自動保存モード, false=手動保存モード</param>
		private void CheckAndSaveResultScreen(bool bIsAutoSave)
		{
			string path = Path.GetDirectoryName( CDTXMania.DTX.strファイル名の絶対パス );
			string datetime = DateTime.Now.ToString( "yyyyMMddHHmmss" );
			if ( bIsAutoSave )
			{
				// リザルト画像を自動保存するときは、dtxファイル名.yyMMddHHmmss_DRUMS_SS.png という形式で保存。
				for ( int i = 0; i < 3; i++ )
				{
					if ( this.bNewRecordRank[ i ] == true || this.bNewRecordSkill[ i ] == true )
					{
						string strPart = ( (EInstrumentPart) ( i ) ).ToString();
						string strRank = ( (CScoreIni.ERANK) ( this.nRankValue[ i ] ) ).ToString();
						string strFullPath = CDTXMania.DTX.strファイル名の絶対パス + "." + datetime + "_" + strPart + "_" + strRank + ".png";
						//Surface.ToFile( pSurface, strFullPath, ImageFileFormat.Png );
						CDTXMania.app.SaveResultScreen( strFullPath );
					}
				}
			}
			#region [ #24609 2011.4.11 yyagi; リザルトの手動保存ロジックは、CDTXManiaに移管した。]
//			else
//			{
//				// リザルト画像を手動保存するときは、dtxファイル名.yyMMddHHmmss_SS.png という形式で保存。(楽器名無し)
//				string strRank = ( (CScoreIni.ERANK) ( CDTXMania.stageResult.n総合ランク値 ) ).ToString();
//				string strSavePath = CDTXMania.strEXEのあるフォルダ + "\\" + "Capture_img";
//				if ( !Directory.Exists( strSavePath ) )
//				{
//					try
//					{
//						Directory.CreateDirectory( strSavePath );
//					}
//					catch
//					{
//					}
//				}
//				string strSetDefDifficulty = CDTXMania.stageSongSelection.rConfirmedSong.arDifficultyLabel[ CDTXMania.stageSongSelection.nConfirmedSongDifficulty ];
//				if ( strSetDefDifficulty != null )
//				{
//					strSetDefDifficulty = "(" + strSetDefDifficulty + ")";
//				}
//				string strFullPath = strSavePath + "\\" + CDTXMania.DTX.TITLE + strSetDefDifficulty +
//					"." + datetime + "_" + strRank + ".png";
//				// Surface.ToFile( pSurface, strFullPath, ImageFileFormat.Png );
//				CDTXMania.app.SaveResultScreen( strFullPath );
//			}
			#endregion
		}
		#endregion
		//-----------------
		#endregion
	}
}
