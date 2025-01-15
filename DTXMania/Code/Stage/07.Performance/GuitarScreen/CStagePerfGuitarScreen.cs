using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
using SharpDX.Direct3D9;
using FDK;

namespace DTXMania
{
	internal class CStagePerfGuitarScreen : CStagePerfCommonScreen
	{
		// コンストラクタ

		public CStagePerfGuitarScreen()
		{
			base.eStageID = CStage.EStage.Playing;
			base.ePhaseID = CStage.EPhase.Common_DefaultState;
			base.bNotActivated = true;
			base.listChildActivities.Add( this.actStageFailed = new CActPerfStageFailure() );
			base.listChildActivities.Add( this.actDANGER = new CActPerfGuitarDanger() );
			base.listChildActivities.Add( this.actAVI = new CActPerfAVI() );
			base.listChildActivities.Add( this.actBGA = new CActPerfBGA() );
            base.listChildActivities.Add( this.actGraph = new CActPerfSkillMeter() );
			base.listChildActivities.Add(this.actGuitarBonus = new CActPerfGuitarBonus());
//			base.listChildActivities.Add( this.actPanel = new CActPerfPanelString() );
			base.listChildActivities.Add( this.actScrollSpeed = new CActPerfScrollSpeed() );
			base.listChildActivities.Add( this.actStatusPanel = new CActPerfGuitarStatusPanel() );
			base.listChildActivities.Add( this.actWailingBonus = new CActPerfGuitarWailingBonus() );
			base.listChildActivities.Add( this.actScore = new CActPerfGuitarScore() );
			base.listChildActivities.Add( this.actRGB = new CActPerfGuitarRGB() );
			base.listChildActivities.Add( this.actLaneFlushGB = new CActPerfGuitarLaneFlushGB() );
			base.listChildActivities.Add( this.actJudgeString = new CActPerfGuitarJudgementString() );
			base.listChildActivities.Add( this.actGauge = new CActPerfGuitarGauge() );
			base.listChildActivities.Add( this.actCombo = new CActPerfGuitarCombo() );
			base.listChildActivities.Add( this.actChipFireGB = new CActPerfGuitarChipFire() );
			base.listChildActivities.Add( this.actPlayInfo = new CActPerformanceInformation() );
			base.listChildActivities.Add( this.actFI = new CActFIFOBlackStart() );
			base.listChildActivities.Add( this.actFO = new CActFIFOBlack() );
			base.listChildActivities.Add( this.actFOClear = new CActFIFOWhite() );
            base.listChildActivities.Add( this.actFOStageClear = new CActFIFOWhiteClear());
			base.listChildActivities.Add( this.actProgressBar = new CActPerfProgressBar());
			base.listChildActivities.Add( this.actBackgroundAVI = new CActSelectBackgroundAVI());
		}


		// メソッド

		public void tStorePerfResults( out CScoreIni.CPerformanceEntry Drums, out CScoreIni.CPerformanceEntry Guitar, out CScoreIni.CPerformanceEntry Bass, out bool bIsTrainingMode )
		{
			Drums = new CScoreIni.CPerformanceEntry();

			base.tStorePerfResults_Guitar( out Guitar );
			base.tStorePerfResultsBass( out Bass );

			bIsTrainingMode = base.bIsTrainingMode;

			//			if ( CDTXMania.ConfigIni.bIsSwappedGuitarBass )		// #24063 2011.1.24 yyagi Gt/Bsを入れ替えていたなら、演奏結果も入れ替える
			//			{
			//				CScoreIni.CPerformanceEntry t;
			//				t = Guitar;
			//				Guitar = Bass;
			//				Bass = t;
			//			
			//				CDTXMania.DTX.SwapGuitarBassInfos();			// 譜面情報も元に戻す
			//			}
		}


		// CStage 実装

		public override void OnActivate()
		{
            int nGraphUsePart = CDTXMania.ConfigIni.bGraph有効.Guitar ? 1 : 2;
            this.ct登場用 = new CCounter(0, 12, 16, CDTXMania.Timer);
            dtLastQueueOperation = DateTime.MinValue;
            if( CDTXMania.bCompactMode )
            {
                var score = new CScore();
                CDTXMania.SongManager.tReadScoreIniAndSetScoreInformation(CDTXMania.strCompactModeFile + ".score.ini", ref score);
                this.actGraph.dbGraphValue_Goal = score.SongInformation.HighSkill[ nGraphUsePart ];
            }
            else
            {
				this.actGraph.dbGraphValue_Goal = CDTXMania.stageSongSelection.rChosenScore.SongInformation.HighSkill[ nGraphUsePart ];	// #24074 2011.01.23 add ikanick
                this.actGraph.dbGraphValue_PersonalBest = CDTXMania.stageSongSelection.rChosenScore.SongInformation.HighSkill[ nGraphUsePart ];

                // #35411 2015.08.21 chnmr0 add
                // ゴースト利用可のなとき、0で初期化
                if (CDTXMania.ConfigIni.eTargetGhost[ nGraphUsePart ] != ETargetGhostData.NONE)
                {
                    if (CDTXMania.listTargetGhsotLag[ nGraphUsePart ] != null)
                    {
                        this.actGraph.dbGraphValue_Goal = 0;
                    }
                }
            }
			base.OnActivate();
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
                this.bサビ区間 = false;
				//this.tGenerateBackgroundTexture();
				this.txChip = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\7_Chips_Guitar.png" ) );
                this.txLane = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\7_lanes_Guitar.png") );
                this.txHitBar = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\\ScreenPlayDrums hit-bar.png"));
				//this.txWailingFrame = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlay wailing cursor.png" ) );
				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if( !base.bNotActivated )
			{
				//CDTXMania.tReleaseTexture( ref this.txBackground );
				CDTXMania.tReleaseTexture( ref this.txChip );
                CDTXMania.tReleaseTexture( ref this.txLane );
				CDTXMania.tReleaseTexture( ref this.txHitBar );
				//CDTXMania.tReleaseTexture( ref this.txWailingFrame );
				base.OnManagedReleaseResources();
			}
		}
		public override int OnUpdateAndDraw()
		{
			if( !base.bNotActivated )
			{
                bool bIsFinishedPlaying = false;
                bool bIsFinishedFadeout = false;

				if( base.bJustStartedUpdate )
				{
                    CSoundManager.rcPerformanceTimer.tReset();
					CDTXMania.Timer.tReset();

                    this.UnitTime = ((60.0 / (CDTXMania.stagePerfGuitarScreen.actPlayInfo.dbBPM) / 14.0)); //2014.01.14.kairera0467 これも動かしたいのだが____

					this.ctChipPatternAnimation.Guitar = new CCounter( 0, 0x17, 20, CDTXMania.Timer );
					this.ctChipPatternAnimation.Bass = new CCounter( 0, 0x17, 20, CDTXMania.Timer );
					this.ctChipPatternAnimation[ 0 ] = null;
                    this.ctComboTimer = new CCounter(1, 16, (int)((60.0 / (CDTXMania.stagePerfGuitarScreen.actPlayInfo.dbBPM) / 16.0 * 1000.0)), CDTXMania.Timer);
					this.ctWailingChipPatternAnimation = new CCounter( 0, 4, 50, CDTXMania.Timer );

                    if( this.tx判定画像anime != null && this.txBonusEffect != null )
                    {
                        this.tx判定画像anime.tDraw2D( CDTXMania.app.Device, 1280, 720 );
                        this.txBonusEffect.tDraw2D( CDTXMania.app.Device, 1280, 720 );
                    }
					base.ePhaseID = CStage.EPhase.Common_FadeIn;
					this.actFI.tStartFadeIn();

					if (CDTXMania.DTXVmode.Enabled)
					{
						tSetSettingsForDTXV();
						tJumpInSongToBar(CDTXMania.DTXVmode.nStartBar + 1);
					}

					base.bJustStartedUpdate = false;

					// display presence now that the initial timer reset has been performed
					tDisplayPresence();
				}
				if( CDTXMania.ConfigIni.bSTAGEFAILEDEnabled && !this.bIsTrainingMode && ( base.ePhaseID == CStage.EPhase.Common_DefaultState ) )
				{
					bool bFailedGuitar = this.actGauge.IsFailed( EInstrumentPart.GUITAR );		// #23630 2011.11.12 yyagi: deleted AutoPlay condition: not to be failed at once
					bool bFailedBass   = this.actGauge.IsFailed( EInstrumentPart.BASS );		// #23630
					bool bFailedNoChips = (!CDTXMania.DTX.bチップがある.Guitar && !CDTXMania.DTX.bチップがある.Bass);	// #25216 2011.5.21 yyagi add condition
					if ( bFailedGuitar || bFailedBass || bFailedNoChips )						// #25216 2011.5.21 yyagi: changed codition: && -> ||
					{
						this.actStageFailed.Start();
						CDTXMania.DTX.tStopPlayingAllChips();
						base.ePhaseID = CStage.EPhase.演奏_STAGE_FAILED;
					}
				}
				this.tUpdateAndDraw_Background();
                this.tUpdateAndDraw_AVI();
				this.tUpdateAndDraw_MIDIBGM();

//                if (CDTXMania.ConfigIni.bShowMusicInfo)
//				    this.t進行描画_パネル文字列();

				this.tUpdateAndDraw_LaneFlushGB();

				this.tUpdateAndDraw_DANGER();

				this.tUpdateAndDraw_WailingBonus();				
				this.tUpdateAndDraw_ScrollSpeed();
				this.tUpdateAndDraw_ChipAnimation();
                this.tUpdateAndDraw_BarLines(EInstrumentPart.GUITAR);
				this.tDraw_LoopLines();
				bIsFinishedPlaying = this.tUpdateAndDraw_Chips(EInstrumentPart.GUITAR);
                this.tUpdateAndDraw_RGBButton();
                this.tUpdateAndDraw_GuitarBass_JudgementLine();
				this.tUpdateAndDraw_JudgementString();
				this.actProgressBar.OnUpdateAndDraw();
				this.tUpdateAndDraw_Gauge();
                if (CDTXMania.ConfigIni.nInfoType == 1)
				    this.tUpdateAndDraw_StatusPanel();
                if (CDTXMania.ConfigIni.bShowScore)
                    this.tUpdateAndDraw_Score();
				
				this.tUpdateAndDraw_Graph();
                this.tUpdateAndDraw_Combo();
				this.tUpdateAndDraw_PerformanceInformation();
				//this.tUpdateAndDraw_WailingFrame();
				this.tUpdateAndDraw_PlaySpeed();
				
				this.tUpdateAndDraw_ChipFireGB();
				this.tUpdateAndDraw_GuitarBonus();
				this.tUpdateAndDraw_STAGEFAILED();
                bIsFinishedFadeout = this.tUpdateAndDraw_FadeIn_Out();
                if ( bIsFinishedPlaying && (base.ePhaseID == CStage.EPhase.Common_DefaultState ) )
                {
					//Pause the timer when finished playing in DTXVMode
					if (CDTXMania.DTXVmode.Enabled)
					{
						if (CDTXMania.Timer.b停止していない)
						{
							CDTXMania.Timer.tPause();
						}
						Thread.Sleep(5);
						// Keep waiting for next message from DTX Creator
					}
					else 
					{
						this.eReturnValueAfterFadeOut = EPerfScreenReturnValue.StageClear;
						base.ePhaseID = CStage.EPhase.演奏_STAGE_CLEAR_フェードアウト;
						this.actFOStageClear.tStartFadeOut();
					}
                }
				if( bIsFinishedFadeout )
				{
                    if (!CDTXMania.Skin.soundStageClear.b再生中)
                    {
                        Debug.WriteLine("Total OnUpdateAndDraw=" + sw.ElapsedMilliseconds + "ms");

						//Update Guitar score like in PerfDrumsScreen
						int nNumberOfMistakes = base.nHitCount_ExclAuto.Guitar.Miss + base.nHitCount_ExclAuto.Guitar.Poor;
						if(nNumberOfMistakes == 0)
                        {
							{
								int nNumberPerfects = base.nHitCount_ExclAuto.Guitar.Perfect;
								if (CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay)
								{
									nNumberPerfects = base.nHitCount_IncAuto.Guitar.Perfect;
								}
								if (nNumberPerfects == CDTXMania.DTX.nVisibleChipsCount.Guitar)
								#region[ エクセ ]
								{
									if (CDTXMania.ConfigIni.nSkillMode == 1)
										this.actScore.nCurrentTrueScore.Guitar += 30000;									
								}
								#endregion
								else
								#region[ フルコン ]
								{
									if (CDTXMania.ConfigIni.nSkillMode == 1)
										this.actScore.nCurrentTrueScore.Guitar += 15000;									
								}
								#endregion
							}
						}

						//Repeat for Bass
						nNumberOfMistakes = base.nHitCount_ExclAuto.Bass.Miss + base.nHitCount_ExclAuto.Bass.Poor;
						if (nNumberOfMistakes == 0)
						{
							{
								int nNumberPerfects = base.nHitCount_ExclAuto.Bass.Perfect;
								if (CDTXMania.ConfigIni.bAllBassAreAutoPlay)
								{
									nNumberPerfects = base.nHitCount_IncAuto.Bass.Perfect;
								}
								if (nNumberPerfects == CDTXMania.DTX.nVisibleChipsCount.Bass)
								#region[ エクセ ]
								{
									if (CDTXMania.ConfigIni.nSkillMode == 1)
										this.actScore.nCurrentTrueScore.Bass += 30000;
								}
								#endregion
								else
								#region[ フルコン ]
								{
									if (CDTXMania.ConfigIni.nSkillMode == 1)
										this.actScore.nCurrentTrueScore.Bass += 15000;
								}
								#endregion
							}
						}

						return (int)this.eReturnValueAfterFadeOut;
                    }
				}
				if (base.ePhaseID == CStage.EPhase.演奏_STAGE_RESTART)
				{
					Debug.WriteLine("Restarting");
					return (int)this.eReturnValueAfterFadeOut;
				}
				ManageMixerQueue();

				if (this.LoopEndMs != -1 && CSoundManager.rcPerformanceTimer.nCurrentTime > this.LoopEndMs)
				{
					Trace.TraceInformation("Reached end of loop");
					this.tJumpInSong(this.LoopBeginMs == -1 ? 0 : this.LoopBeginMs);

					//Reset hit counts and scores, so that the displayed score reflects the looped part only
					for (int inst = 1; inst < 3; ++inst)
					{
						this.nHitCount_ExclAuto[inst].Perfect = 0;
						this.nHitCount_ExclAuto[inst].Great = 0;
						this.nHitCount_ExclAuto[inst].Good = 0;
						this.nHitCount_ExclAuto[inst].Poor = 0;
						this.nHitCount_ExclAuto[inst].Miss = 0;
						this.actCombo.nCurrentCombo[inst] = 0;
						this.actCombo.nCurrentCombo.HighestValue[inst] = 0;
						base.actScore.nCurrentTrueScore[inst] = 0;

						//
						this.nTimingHitCount[inst].nLate = 0;
						this.nTimingHitCount[inst].nEarly = 0;
					}
				}

				// キー入力
				this.tHandleKeyInput();
			}
            base.sw.Stop();
			return 0;
		}


		// Other

		#region [ private ]
		//-----------------
        private CTexture txLane;
        public bool bサビ区間;
        public double UnitTime;

		protected override EJudgement tProcessChipHit( long nHitTime, CChip pChip, bool bCorrectLane )
		{
			EJudgement eJudgeResult = tProcessChipHit( nHitTime, pChip, EInstrumentPart.GUITAR, bCorrectLane );
            if(pChip.eInstrumentPart == EInstrumentPart.GUITAR)
			{
				if (CDTXMania.ConfigIni.nSkillMode == 0)
					this.actGraph.dbグラフ値現在_渡 = CScoreIni.tCalculatePlayingSkillOld(CDTXMania.DTX.nVisibleChipsCount.Guitar, this.nHitCount_ExclAuto.Guitar.Perfect, this.nHitCount_ExclAuto.Guitar.Great, this.nHitCount_ExclAuto.Guitar.Good, this.nHitCount_ExclAuto.Guitar.Poor, this.nHitCount_ExclAuto.Guitar.Miss, this.actCombo.nCurrentCombo.HighestValue.Guitar, EInstrumentPart.GUITAR, bIsAutoPlay);
				else
					this.actGraph.dbグラフ値現在_渡 = CScoreIni.tCalculatePlayingSkill(CDTXMania.DTX.nVisibleChipsCount.Guitar, this.nHitCount_ExclAuto.Guitar.Perfect, this.nHitCount_ExclAuto.Guitar.Great, this.nHitCount_ExclAuto.Guitar.Good, this.nHitCount_ExclAuto.Guitar.Poor, this.nHitCount_ExclAuto.Guitar.Miss, this.actCombo.nCurrentCombo.HighestValue.Guitar, EInstrumentPart.GUITAR, bIsAutoPlay);
				this.actStatusPanel.db現在の達成率.Guitar = this.actGraph.dbグラフ値現在_渡;

				if (CDTXMania.ConfigIni.bGraph有効.Guitar)
				{

					if (CDTXMania.listTargetGhsotLag.Guitar != null &&
						CDTXMania.ConfigIni.eTargetGhost.Guitar == ETargetGhostData.ONLINE &&
						CDTXMania.DTX.nVisibleChipsCount.Guitar > 0)
					{

						this.actGraph.dbグラフ値現在_渡 = 100 *
										(this.nHitCount_ExclAuto.Guitar.Perfect * 17 +
										 this.nHitCount_ExclAuto.Guitar.Great * 7 +
										 this.actCombo.nCurrentCombo.HighestValue.Guitar * 3) / (20.0 * CDTXMania.DTX.nVisibleChipsCount.Guitar);
					}

					this.actGraph.n現在のAutoを含まない判定数_渡[0] = this.nHitCount_ExclAuto.Guitar.Perfect;
					this.actGraph.n現在のAutoを含まない判定数_渡[1] = this.nHitCount_ExclAuto.Guitar.Great;
					this.actGraph.n現在のAutoを含まない判定数_渡[2] = this.nHitCount_ExclAuto.Guitar.Good;
					this.actGraph.n現在のAutoを含まない判定数_渡[3] = this.nHitCount_ExclAuto.Guitar.Poor;
					this.actGraph.n現在のAutoを含まない判定数_渡[4] = this.nHitCount_ExclAuto.Guitar.Miss;
				}
			}
			else if(pChip.eInstrumentPart == EInstrumentPart.BASS)
			{
				if (CDTXMania.ConfigIni.nSkillMode == 0)
					this.actGraph.dbグラフ値現在_渡 = CScoreIni.tCalculatePlayingSkillOld(CDTXMania.DTX.nVisibleChipsCount.Bass, this.nHitCount_ExclAuto.Bass.Perfect, this.nHitCount_ExclAuto.Bass.Great, this.nHitCount_ExclAuto.Bass.Good, this.nHitCount_ExclAuto.Bass.Poor, this.nHitCount_ExclAuto.Bass.Miss, this.actCombo.nCurrentCombo.HighestValue.Bass, EInstrumentPart.BASS, bIsAutoPlay);
				else
					this.actGraph.dbグラフ値現在_渡 = CScoreIni.tCalculatePlayingSkill(CDTXMania.DTX.nVisibleChipsCount.Bass, this.nHitCount_ExclAuto.Bass.Perfect, this.nHitCount_ExclAuto.Bass.Great, this.nHitCount_ExclAuto.Bass.Good, this.nHitCount_ExclAuto.Bass.Poor, this.nHitCount_ExclAuto.Bass.Miss, this.actCombo.nCurrentCombo.HighestValue.Bass, EInstrumentPart.BASS, bIsAutoPlay);
				this.actStatusPanel.db現在の達成率.Bass = this.actGraph.dbグラフ値現在_渡;

				if (CDTXMania.ConfigIni.bGraph有効.Bass)
				{

					if (CDTXMania.listTargetGhsotLag.Bass != null &&
						CDTXMania.ConfigIni.eTargetGhost.Bass == ETargetGhostData.ONLINE &&
						CDTXMania.DTX.nVisibleChipsCount.Bass > 0)
					{

						this.actGraph.dbグラフ値現在_渡 = 100 *
										(this.nHitCount_ExclAuto.Bass.Perfect * 17 +
										 this.nHitCount_ExclAuto.Bass.Great * 7 +
										 this.actCombo.nCurrentCombo.HighestValue.Bass * 3) / (20.0 * CDTXMania.DTX.nVisibleChipsCount.Bass);
					}

					this.actGraph.n現在のAutoを含まない判定数_渡[0] = this.nHitCount_ExclAuto.Bass.Perfect;
					this.actGraph.n現在のAutoを含まない判定数_渡[1] = this.nHitCount_ExclAuto.Bass.Great;
					this.actGraph.n現在のAutoを含まない判定数_渡[2] = this.nHitCount_ExclAuto.Bass.Good;
					this.actGraph.n現在のAutoを含まない判定数_渡[3] = this.nHitCount_ExclAuto.Bass.Poor;
					this.actGraph.n現在のAutoを含まない判定数_渡[4] = this.nHitCount_ExclAuto.Bass.Miss;
				}
            }
			return eJudgeResult;
		}

		protected override void tチップのヒット処理_BadならびにTight時のMiss( EInstrumentPart part )
		{
			this.tチップのヒット処理_BadならびにTight時のMiss( part, 0, EInstrumentPart.GUITAR );
		}
		protected override void tチップのヒット処理_BadならびにTight時のMiss( EInstrumentPart part, int nLane )
		{
			this.tチップのヒット処理_BadならびにTight時のMiss( part, nLane, EInstrumentPart.GUITAR );
		}

        /*
		protected override void tUpdateAndDraw_AVI()
		{
		    base.tUpdateAndDraw_AVI( 0, 0 );
		}
		protected override void t進行描画_BGA()
		{
		    base.t進行描画_BGA( 500, 50 );
		}
         */
		protected override void tUpdateAndDraw_DANGER()			// #23631 2011.4.19 yyagi
		{
			//this.actDANGER.tUpdateAndDraw( false, this.actGauge.db現在のゲージ値.Guitar < 0.3, this.actGauge.db現在のゲージ値.Bass < 0.3 );
			this.actDANGER.tUpdateAndDraw( false, this.actGauge.IsDanger(EInstrumentPart.GUITAR), this.actGauge.IsDanger(EInstrumentPart.BASS) );
		}
        private void tUpdateAndDraw_Graph()  // t進行描画_グラフ
		{
			if ( !CDTXMania.ConfigIni.bストイックモード && ( CDTXMania.ConfigIni.bGraph有効.Guitar || CDTXMania.ConfigIni.bGraph有効.Bass ) )
			{
                this.actGraph.OnUpdateAndDraw();
            }
        }
		protected override void tUpdateAndDraw_WailingFrame()
		{
			base.tUpdateAndDraw_WailingFrame( 292, 0x251,
				CDTXMania.ConfigIni.bReverse.Guitar ? 340 : 130,
				CDTXMania.ConfigIni.bReverse.Bass ?   340 : 130
			);
		}
		private void tUpdateAndDraw_GuitarBass_JudgementLine()  // t進行描画_ギターベース判定ライン	yyagi: ドラム画面とは座標が違うだけですが、まとめづらかったのでそのまま放置してます。
		{
			if ( CDTXMania.ConfigIni.bGuitarEnabled )
			{
				if ( CDTXMania.DTX.bチップがある.Guitar )
				{
                    int y = CDTXMania.ConfigIni.bReverse.Guitar ? this.nJudgeLinePosY.Guitar : this.nJudgeLinePosY.Guitar - 1;

						if ( this.txHitBar != null && CDTXMania.ConfigIni.bJudgeLineDisp.Guitar )
							this.txHitBar.tDraw2D( CDTXMania.app.Device, 80, y, new Rectangle( 0, 0, 252, 6 ) );

                    if (CDTXMania.ConfigIni.b演奏情報を表示する)
                        this.actLVFont.tDrawString(310, (CDTXMania.ConfigIni.bReverse.Guitar ? y + 8 : y - 20), CDTXMania.ConfigIni.nJudgeLine.Guitar.ToString());
				}
				if ( CDTXMania.DTX.bチップがある.Bass )
				{
                    int y = CDTXMania.ConfigIni.bReverse.Bass ? this.nJudgeLinePosY.Bass : this.nJudgeLinePosY.Bass - 1;

						if ( this.txHitBar != null && CDTXMania.ConfigIni.bJudgeLineDisp.Bass )
                            this.txHitBar.tDraw2D(CDTXMania.app.Device, 950, y, new Rectangle(0, 0, 252, 6));

                    if (CDTXMania.ConfigIni.b演奏情報を表示する)
                        this.actLVFont.tDrawString(1180, (CDTXMania.ConfigIni.bReverse.Bass ? y + 8 : y - 20), CDTXMania.ConfigIni.nJudgeLine.Bass.ToString());
                }
			}
		}

        /*
		protected override void t進行描画_パネル文字列()
		{
			base.t進行描画_パネル文字列( 0xb5, 430 );
		}
         */

		protected override void tUpdateAndDraw_PerformanceInformation()
		{
			base.tUpdateAndDraw_PerformanceInformation( 500, 257 );
		}

		private void tUpdateAndDraw_PlaySpeed()
		{
			if (this.txPlaySpeed != null)
			{
				this.txPlaySpeed.tDraw2D(CDTXMania.app.Device, 600, 687);
			}
		}

		protected override void tJudgeLineMovingUpandDown()
        {

        }

		protected override void ScrollSpeedUp()
		{
            CDTXMania.ConfigIni.nScrollSpeed.Guitar = Math.Min(CDTXMania.ConfigIni.nScrollSpeed.Guitar + 1, 1999);
		}
		protected override void ScrollSpeedDown()
		{
            CDTXMania.ConfigIni.nScrollSpeed.Guitar = Math.Max(CDTXMania.ConfigIni.nScrollSpeed.Guitar - 1, 0);
		}

		protected override void tHandleInput_Drums()
		{
			// ギタレボモードでは何もしない
		}

		protected override void tGenerateBackgroundTexture()
		{
			Rectangle bgrect = new Rectangle( 0, 0, 1280, 720 );
			string DefaultBgFilename = @"Graphics\7_background_Guitar.jpg";
			string BgFilename = "";
			string BACKGROUND = null;
			if ( ( CDTXMania.DTX.BACKGROUND_GR != null ) && ( CDTXMania.DTX.BACKGROUND_GR.Length > 0 ) )
			{
				BACKGROUND = CDTXMania.DTX.BACKGROUND_GR;
			}
			else if ( ( CDTXMania.DTX.BACKGROUND != null ) && ( CDTXMania.DTX.BACKGROUND.Length > 0 ) )
			{
				BACKGROUND = CDTXMania.DTX.BACKGROUND;
			}
			if ( ( BACKGROUND != null ) && ( BACKGROUND.Length > 0 ) )
			{
				BgFilename = CDTXMania.DTX.strFolderName + BACKGROUND;
			}
			base.tGenerateBackgroundTexture( DefaultBgFilename, bgrect, BgFilename );
		}
        protected override void tUpdateAndDraw_Chip_PatternOnly_Drums(CConfigIni configIni, ref CDTX dTX, ref CChip pChip)
        {
            // int indexSevenLanes = this.nチャンネル0Atoレーン07[ pChip.nChannelNumber - 0x11 ];
            if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
            {
                //pChip.bHit = true;
                //this.tPlaySound(pChip, CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻 + pChip.nPlaybackTimeMs, EInstrumentPart.DRUMS, dTX.nモニタを考慮した音量(EInstrumentPart.DRUMS));
            }
        }
        protected override void tUpdateAndDraw_Chip_Drums(CConfigIni configIni, ref CDTX dTX, ref CChip pChip)
		{
			// int indexSevenLanes = this.nチャンネル0Atoレーン07[ pChip.nChannelNumber - 0x11 ];
			if ( !pChip.bHit && ( pChip.nDistanceFromBar.Drums < 0 ) )
			{
				pChip.bHit = true;
                this.tPlaySound(pChip, CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻 + pChip.nPlaybackTimeMs, EInstrumentPart.DRUMS, dTX.nモニタを考慮した音量(EInstrumentPart.DRUMS));
			}
		}
		protected override void tUpdateAndDraw_Chip_GuitarBass( CConfigIni configIni, ref CDTX dTX, ref CChip pChip, EInstrumentPart inst )
		{
			base.tUpdateAndDraw_Chip_GuitarBass( configIni, ref dTX, ref pChip, inst,
                this.nJudgeLinePosY[ (int) inst ] + 10, this.nJudgeLinePosY[ (int) inst ] + 1, 104, 670, 0, 0, 0, 11, 196, 10, 38, 38, 1000, 1000, 1000, 38, 38);
		}
#if false
		protected override void t進行描画_チップ_ギターベース( CConfigIni configIni, ref CDTX dTX, ref CChip pChip, E楽器パート inst )
		{
			int instIndex = (int) inst;
			if ( configIni.bGuitar有効 )
			{
				if ( configIni.bSudden[instIndex ] )
				{
					pChip.b可視 = pChip.nバーからの距離dot[ instIndex ] < 200;
				}
				if ( configIni.bHidden[ instIndex ] && ( pChip.nバーからの距離dot[ instIndex ] < 100 ) )
				{
					pChip.b可視 = false;
				}

				bool bChipHasR = ( ( pChip.nチャンネル番号 & 4 ) > 0 );
				bool bChipHasG = ( ( pChip.nチャンネル番号 & 2 ) > 0 );
				bool bChipHasB = ( ( pChip.nチャンネル番号 & 1 ) > 0 );
				bool bChipHasW = ( ( pChip.nチャンネル番号 & 0x0F ) == 0x08 );
				bool bChipIsO  = ( ( pChip.nチャンネル番号 & 0x0F ) == 0x00 );

				int OPEN = ( inst == E楽器パート.GUITAR ) ? 0x20 : 0xA0;
				if ( !pChip.bHit && pChip.b可視 )
				{
					int y = configIni.bReverse[ instIndex ] ? ( 369 - pChip.nバーからの距離dot[ instIndex ]) : ( 40 + pChip.nバーからの距離dot[ instIndex ] );
					if ( ( y > 0 ) && ( y < 409 ) )
					{
						if ( this.txチップ != null )
						{
							int nアニメカウンタ現在の値 = this.ctチップ模様アニメ[ instIndex ].n現在の値;
							if ( pChip.nチャンネル番号 == OPEN )
							{
								{
									int xo = ( inst == E楽器パート.GUITAR ) ? 26 : 480;
									this.txチップ.t2D描画( CDTXMania.app.Device, xo, y - 4, new Rectangle( 0, 192 + ( ( nアニメカウンタ現在の値 % 5 ) * 8 ), 103, 8 ) );
								}
							}
							Rectangle rc = new Rectangle( 0, nアニメカウンタ現在の値 * 8, 32, 8 );
							int x;
							if ( inst == E楽器パート.GUITAR )
							{
								x = ( configIni.bLeft.Guitar ) ? 98 : 26;
							}
							else
							{
								x = ( configIni.bLeft.Bass ) ? 552 : 480;
							}
							int deltaX = ( configIni.bLeft[ instIndex ] ) ? -36 : +36; 
							if ( bChipHasR )
							{
								this.txチップ.t2D描画( CDTXMania.app.Device, x, y - 4, rc );
							}
							rc.X += 32;
							if ( bChipHasG )
							{
								this.txチップ.t2D描画( CDTXMania.app.Device, x, y - 4, rc );
							}
							rc.X += 32;
							if ( bChipHasB )
							{
								this.txチップ.t2D描画( CDTXMania.app.Device, x, y - 4, rc );
							}
						}
					}
				}
				// if ( ( configIni.bAutoPlay.Guitar && !pChip.bHit ) && ( pChip.nバーからの距離dot.Guitar < 0 ) )
				if ( ( !pChip.bHit ) && ( pChip.nバーからの距離dot[ instIndex ] < 0 ) )
				{
					int lo = ( inst == E楽器パート.GUITAR ) ? 0 : 3;	// lane offset
					bool autoR = ( inst == E楽器パート.GUITAR ) ? bIsAutoPlay.GtR : bIsAutoPlay.BsR;
					bool autoG = ( inst == E楽器パート.GUITAR ) ? bIsAutoPlay.GtG : bIsAutoPlay.BsG;
					bool autoB = ( inst == E楽器パート.GUITAR ) ? bIsAutoPlay.GtB : bIsAutoPlay.BsB;
					if ( ( bChipHasR || bChipIsO ) && autoR )
					{
						this.actChipFireGB.Start( 0 + lo );
					}
					if ( ( bChipHasG || bChipIsO ) && autoG )
					{
						this.actChipFireGB.Start( 1 + lo );
					}
					if ( ( bChipHasB || bChipIsO ) && autoB )
					{
						this.actChipFireGB.Start( 2 + lo );
					}
					if ( ( inst == E楽器パート.GUITAR && bIsAutoPlay.GtPick ) || ( inst == E楽器パート.BASS && bIsAutoPlay.BsPick ) )
					{
						bool pushingR = CDTXMania.Pad.b押されている( inst, Eパッド.R );
						bool pushingG = CDTXMania.Pad.b押されている( inst, Eパッド.G );
						bool pushingB = CDTXMania.Pad.b押されている( inst, Eパッド.B );
						bool bMiss = true;
						if ( ( ( bChipIsO == true ) && ( !pushingR | autoR ) && ( !pushingG | autoG ) && ( !pushingB | autoB ) ) ||
							( ( bChipHasR == ( pushingR | autoR ) ) && ( bChipHasG == ( pushingG | autoG ) ) && ( bChipHasB == ( pushingB | autoB ) ) )
						)
						{
							bMiss = false;
						}
						pChip.bHit = true;
						this.tサウンド再生( pChip, CDTXMania.Timer.n前回リセットした時のシステム時刻 + pChip.n発声時刻ms, inst, dTX.nモニタを考慮した音量( inst ) );
						this.r次にくるギターChip = null;
						this.tチップのヒット処理( pChip.n発声時刻ms, pChip );
					}
				}
				// break;
				return;
			}
			if ( !pChip.bHit && ( pChip.nバーからの距離dot[ instIndex ] < 0 ) )
			{
				pChip.bHit = true;
				this.tサウンド再生( pChip, CDTXMania.Timer.n前回リセットした時のシステム時刻 + pChip.n発声時刻ms, inst, dTX.nモニタを考慮した音量( inst ) );
			}
		}
#endif
		protected override void tUpdateAndDraw_Chip_Guitar_Wailing( CConfigIni configIni, ref CDTX dTX, ref CChip pChip )
		{
			if ( configIni.bGuitarEnabled )
			{
				//if ( configIni.bSudden.Guitar )
				//{
				//    pChip.bVisible = pChip.nDistanceFromBar.Guitar < 200;
				//}
				//if ( configIni.bHidden.Guitar && ( pChip.nDistanceFromBar.Guitar < 100 ) )
				//{
				//    pChip.bVisible = false;
				//}

				//
				// 後日、以下の部分を何とかCStage演奏画面共通.csに移したい。
				//
				if ( !pChip.bHit && pChip.bVisible )
				{
					int[] y_base = { 154, 611 };			// ドラム画面かギター画面かで変わる値
					int offset = 0;						// ドラム画面かギター画面かで変わる値

					const int WailingWidth = 54;		// 4種全て同じ値
					const int WailingHeight = 68;		// 4種全て同じ値
					const int baseTextureOffsetX = 0;	// ドラム画面かギター画面かで変わる値
					const int baseTextureOffsetY = 22;	// ドラム画面かギター画面かで変わる値
					const int drawX = 287;				// 4種全て異なる値

					const int numA = 34;				// 4種全て同じ値;
					int y = configIni.bReverse.Guitar ? ( y_base[ 1 ] - pChip.nDistanceFromBar.Guitar ) : ( y_base[ 0 ] + pChip.nDistanceFromBar.Guitar );
					int numB = y - offset;				// 4種全て同じ定義
					int numC = 0;						// 4種全て同じ初期値
					const int numD = 709;				// ドラム画面かギター画面かで変わる値
					if ( ( numB < ( numD + numA ) ) && ( numB > -numA ) )	// 以下のロジックは4種全て同じ
					{
						int c = this.ctWailingChipPatternAnimation.nCurrentValue;
						Rectangle rect = new Rectangle( baseTextureOffsetX, baseTextureOffsetY, WailingWidth, WailingHeight );
						if ( numB < numA )
						{
							rect.Y += numA - numB;
							rect.Height -= numA - numB;
							numC = numA - numB;
						}
						if ( numB > ( numD - numA ) )
						{
							rect.Height -= numB - ( numD - numA );
						}
						if ( ( rect.Bottom > rect.Top ) && ( this.txChip != null ) )
						{
							this.txChip.vcScaleRatio.Y = 1f;
							this.txChip.tDraw2D( CDTXMania.app.Device, drawX, ( y - numA ) + numC, rect );
						}
					}
				}
				//    if( !pChip.bHit && ( pChip.nDistanceFromBar.Guitar < 0 ) )
				//    {
				//        pChip.bHit = true;
				//        if( configIni.bAutoPlay.Guitar )
				//        {
				//            this.actWailingBonus.Start( EInstrumentPart.GUITAR, this.r現在の歓声Chip.Guitar );
				//        }
				//    }
				//    return;
				//}
				//pChip.bHit = true;
			}
			base.tUpdateAndDraw_Chip_Guitar_Wailing( configIni, ref dTX, ref pChip );
		}
		protected override void tUpdateAndDraw_Chip_FillIn( CConfigIni configIni, ref CDTX dTX, ref CChip pChip )
		{
			if ( !pChip.bHit && ( pChip.nDistanceFromBar.Drums < 0 ) )
			{
				pChip.bHit = true;
			}
#if TEST_NOTEOFFMODE	// 2011.1.1 yyagi TEST
			switch ( pChip.n整数値 )
			{
				case 0x04:	// HH消音あり(従来同等)
					CDTXMania.DTX.b演奏で直前の音を消音する.HH = true;
					break;
				case 0x05:	// HH消音無し
					CDTXMania.DTX.b演奏で直前の音を消音する.HH = false;
					break;
				case 0x06:	// ギター消音あり(従来同等)
					CDTXMania.DTX.b演奏で直前の音を消音する.Guitar = true;
					break;
				case 0x07:	// ギター消音無し
					CDTXMania.DTX.b演奏で直前の音を消音する.Guitar = false;
					break;
				case 0x08:	// ベース消音あり(従来同等)
					CDTXMania.DTX.b演奏で直前の音を消音する.Bass = true;
					break;
				case 0x09:	// ベース消音無し
					CDTXMania.DTX.b演奏で直前の音を消音する.Bass = false;
					break;
			}
#endif

		}
        protected override void tUpdateAndDraw_Chip_Bonus(CConfigIni configIni, ref CDTX dTX, ref CChip pChip)
        {
            if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
            {
                pChip.bHit = true;
            }
        }
#if false
		protected override void t進行描画_チップ_ベース( CConfigIni configIni, ref CDTX dTX, ref CChip pChip )
		{
			if ( configIni.bGuitar有効 )
			{
				if ( configIni.bSudden.Bass )
				{
					pChip.b可視 = pChip.nバーからの距離dot.Bass < 200;
				}
				if ( configIni.bHidden.Bass && ( pChip.nバーからの距離dot.Bass < 100 ) )
				{
					pChip.b可視 = false;
				}
				if ( !pChip.bHit && pChip.b可視 )
				{
					int num8 = configIni.bReverse.Bass ? ( 0x171 - pChip.nバーからの距離dot.Bass ) : ( 40 + pChip.nバーからの距離dot.Bass );
					if ( ( num8 > 0 ) && ( num8 < 0x199 ) )
					{
						int num9 = this.ctチップ模様アニメ.Bass.n現在の値;
						if ( pChip.nチャンネル番号 == 160 )
						{
							if ( this.txチップ != null )
							{
								this.txチップ.t2D描画( CDTXMania.app.Device, 480, num8 - 4, new Rectangle( 0, 0xc0 + ( ( num9 % 5 ) * 8 ), 0x67, 8 ) );
							}
						}
						else if ( !configIni.bLeft.Bass )
						{
							Rectangle rectangle3 = new Rectangle( 0, num9 * 8, 0x20, 8 );
							if ( ( ( pChip.nチャンネル番号 & 4 ) != 0 ) && ( this.txチップ != null ) )
							{
								this.txチップ.t2D描画( CDTXMania.app.Device, 480, num8 - 4, rectangle3 );
							}
							rectangle3.X += 0x20;
							if ( ( ( pChip.nチャンネル番号 & 2 ) != 0 ) && ( this.txチップ != null ) )
							{
								this.txチップ.t2D描画( CDTXMania.app.Device, 0x204, num8 - 4, rectangle3 );
							}
							rectangle3.X += 0x20;
							if ( ( ( pChip.nチャンネル番号 & 1 ) != 0 ) && ( this.txチップ != null ) )
							{
								this.txチップ.t2D描画( CDTXMania.app.Device, 0x228, num8 - 4, rectangle3 );
							}
						}
						else
						{
							Rectangle rectangle4 = new Rectangle( 0, num9 * 8, 0x20, 8 );
							if ( ( ( pChip.nチャンネル番号 & 4 ) != 0 ) && ( this.txチップ != null ) )
							{
								this.txチップ.t2D描画( CDTXMania.app.Device, 0x228, num8 - 4, rectangle4 );
							}
							rectangle4.X += 0x20;
							if ( ( ( pChip.nチャンネル番号 & 2 ) != 0 ) && ( this.txチップ != null ) )
							{
								this.txチップ.t2D描画( CDTXMania.app.Device, 0x204, num8 - 4, rectangle4 );
							}
							rectangle4.X += 0x20;
							if ( ( ( pChip.nチャンネル番号 & 1 ) != 0 ) && ( this.txチップ != null ) )
							{
								this.txチップ.t2D描画( CDTXMania.app.Device, 480, num8 - 4, rectangle4 );
							}
						}
					}
				}
				if ( ( configIni.bAutoPlay.Bass && !pChip.bHit ) && ( pChip.nバーからの距離dot.Bass < 0 ) )
				{
					pChip.bHit = true;
					if ( ( ( pChip.nチャンネル番号 & 4 ) != 0 ) || ( pChip.nチャンネル番号 == 0xA0 ) )
					{
						this.actChipFireGB.Start( 3 );
					}
					if ( ( ( pChip.nチャンネル番号 & 2 ) != 0 ) || ( pChip.nチャンネル番号 == 0xA0 ) )
					{
						this.actChipFireGB.Start( 4 );
					}
					if ( ( ( pChip.nチャンネル番号 & 1 ) != 0 ) || ( pChip.nチャンネル番号 == 0xA0 ) )
					{
						this.actChipFireGB.Start( 5 );
					}
					this.tサウンド再生( pChip, CSound管理.rc演奏用タイマ.n前回リセットした時のシステム時刻 + pChip.n発声時刻ms, E楽器パート.BASS, dTX.nモニタを考慮した音量( E楽器パート.BASS ) );
					this.r次にくるベースChip = null;
					this.tチップのヒット処理( pChip.n発声時刻ms, pChip );
				}
				return;
			}
			if ( !pChip.bHit && ( pChip.nバーからの距離dot.Bass < 0 ) )
			{
				pChip.bHit = true;
				this.tサウンド再生( pChip, CSound管理.rc演奏用タイマ.n前回リセットした時のシステム時刻 + pChip.n発声時刻ms, E楽器パート.BASS, dTX.nモニタを考慮した音量( E楽器パート.BASS ) );
			}
		}
#endif
        protected override void tUpdateAndDraw_Chip_Bass_Wailing( CConfigIni configIni, ref CDTX dTX, ref CChip pChip )
		{
			if ( configIni.bGuitarEnabled )
			{
				//if ( configIni.bSudden.Bass )
				//{
				//    pChip.bVisible = pChip.nDistanceFromBar.Bass < 200;
				//}
				//if ( configIni.bHidden.Bass && ( pChip.nDistanceFromBar.Bass < 100 ) )
				//{
				//    pChip.bVisible = false;
				//}

				//
				// 後日、以下の部分を何とかCStage演奏画面共通.csに移したい。
				//
				if ( !pChip.bHit && pChip.bVisible )
				{
					int[] y_base = { 154, 611 };			// ドラム画面かギター画面かで変わる値
					int offset = 0;						// ドラム画面かギター画面かで変わる値

					const int WailingWidth = 54;		// 4種全て同じ値
					const int WailingHeight = 68;		// 4種全て同じ値
					const int baseTextureOffsetX = 0;	// ドラム画面かギター画面かで変わる値
					const int baseTextureOffsetY = 22;	// ドラム画面かギター画面かで変わる値
					const int drawX = 1155;				// 4種全て異なる値

					const int numA = 34;				// 4種全て同じ値
					int y = CDTXMania.ConfigIni.bReverse.Bass ? ( y_base[ 1 ] - pChip.nDistanceFromBar.Bass ) : ( y_base[ 0 ] + pChip.nDistanceFromBar.Bass );
					int numB = y - offset;				// 4種全て同じ定義
					int numC = 0;						// 4種全て同じ初期値
					const int numD = 709;				// ドラム画面かギター画面かで変わる値
					if ( ( numB < ( numD + numA ) ) && ( numB > -numA ) )	// 以下のロジックは4種全て同じ
					{
						int c = this.ctWailingChipPatternAnimation.nCurrentValue;
                        Rectangle rect = new Rectangle(baseTextureOffsetX, baseTextureOffsetY, WailingWidth, WailingHeight);
						if ( numB < numA )
						{
							rect.Y += numA - numB;
							rect.Height -= numA - numB;
							numC = numA - numB;
						}
						if ( numB > ( numD - numA ) )
						{
							rect.Height -= numB - ( numD - numA );
						}
						if ( ( rect.Bottom > rect.Top ) && ( this.txChip != null ) )
						{
							this.txChip.vcScaleRatio.Y = 1.0f;
                            this.txChip.tDraw2D(CDTXMania.app.Device, drawX, (y - numA) + numC, rect);
						}
					}
				}
				//    if ( !pChip.bHit && ( pChip.nDistanceFromBar.Bass < 0 ) )
				//    {
				//        pChip.bHit = true;
				//        if ( configIni.bAutoPlay.Bass )
				//        {
				//            this.actWailingBonus.Start( EInstrumentPart.BASS, this.r現在の歓声Chip.Bass );
				//        }
				//    }
				//    return;
				//}
				//pChip.bHit = true;
				base.tUpdateAndDraw_Chip_Bass_Wailing( configIni, ref dTX, ref pChip );
			}
		}
		protected override void tUpdateAndDraw_Chip_NoSound_Drums( CConfigIni configIni, ref CDTX dTX, ref CChip pChip )
		{
			if ( !pChip.bHit && ( pChip.nDistanceFromBar.Drums < 0 ) )
			{
				pChip.bHit = true;
			}
		}
		protected override void tUpdateAndDraw_Chip_BarLine( CConfigIni configIni, ref CDTX dTX, ref CChip pChip )
		{
			int n小節番号plus1 = pChip.nPlaybackPosition / 0x180;
			if ( !pChip.bHit && ( pChip.nDistanceFromBar.Drums < 0 ) )
			{
				pChip.bHit = true;
				this.actPlayInfo.n小節番号 = n小節番号plus1 - 1;
				if ( configIni.bWave再生位置自動調整機能有効 && bIsDirectSound )
				{
					dTX.tAutoCorrectWavPlaybackPosition();
				}
			}
			if ( ( pChip.bVisible && configIni.bGuitarEnabled ))
			{
                int y = CDTXMania.ConfigIni.bReverse.Guitar ? ((this.nJudgeLinePosY.Guitar - pChip.nDistanceFromBar.Guitar) + 0) : ((this.nJudgeLinePosY.Guitar + pChip.nDistanceFromBar.Guitar) + 9);
                if ( ( dTX.bチップがある.Guitar && ( y > 104 ) ) && ( ( y < 670 ) && ( this.txChip != null ) ) )
                {
					if (CDTXMania.ConfigIni.nLaneDisp.Guitar == 0 || CDTXMania.ConfigIni.nLaneDisp.Guitar == 1)
					{
						this.txChip.vcScaleRatio.Y = 1f;
						this.txChip.tDraw2D(CDTXMania.app.Device, 88, y, new Rectangle(0, 20, 193, 2));
					}
                    if ( configIni.b演奏情報を表示する )
                    {
                        int n小節番号 = n小節番号plus1 - 1;
                        CDTXMania.actDisplayString.tPrint(60, y - 16, CCharacterConsole.EFontType.White, n小節番号.ToString());
                    }
				}
                y = CDTXMania.ConfigIni.bReverse.Bass ? ((this.nJudgeLinePosY.Bass - pChip.nDistanceFromBar.Bass) + 0) : ((this.nJudgeLinePosY.Bass + pChip.nDistanceFromBar.Bass) + 9);
                if ( ( dTX.bチップがある.Bass && ( y > 104 ) ) && ( ( y < 670 ) && ( this.txChip != null ) ) )
                {
                    if( CDTXMania.ConfigIni.nLaneDisp.Bass == 0 || CDTXMania.ConfigIni.nLaneDisp.Bass == 1 )
                    {
						this.txChip.vcScaleRatio.Y = 1f;
						this.txChip.tDraw2D(CDTXMania.app.Device, 959, y, new Rectangle(0, 20, 193, 2));
					}
					    

                    if ( configIni.b演奏情報を表示する )
                    {
                        int n小節番号 = n小節番号plus1 - 1;
                        CDTXMania.actDisplayString.tPrint(930, y - 16, CCharacterConsole.EFontType.White, n小節番号.ToString());
                    }
				}
			}

		}

		protected override void tDraw_LoopLine(CConfigIni configIni, bool bIsEnd)
		{
			const double speed = 286;   // BPM150の時の1小節の長さ[dot]
			double ScrollSpeedGuitar = (this.actScrollSpeed.db現在の譜面スクロール速度.Guitar + 1.0) * 0.5 * 0.5 * 37.5 * speed / 60000.0;
			double ScrollSpeedBass = (this.actScrollSpeed.db現在の譜面スクロール速度.Bass + 1.0) * 0.5 * 0.5 * 37.5 * speed / 60000.0;

			int nDistanceFromBarGuitar = (int)(((bIsEnd ? this.LoopEndMs : this.LoopBeginMs) - CSoundManager.rcPerformanceTimer.nCurrentTime) * ScrollSpeedGuitar);
			int nDistanceFromBarBass = (int)(((bIsEnd ? this.LoopEndMs : this.LoopBeginMs) - CSoundManager.rcPerformanceTimer.nCurrentTime) * ScrollSpeedBass);

			if (configIni.bGuitarEnabled)
			{
				int y = CDTXMania.ConfigIni.bReverse.Guitar ? ((this.nJudgeLinePosY.Guitar - nDistanceFromBarGuitar) + 0) : ((this.nJudgeLinePosY.Guitar + nDistanceFromBarGuitar) + 9);
				if ((CDTXMania.DTX.bチップがある.Guitar && (y > 104)) && ((y < 670) && (this.txChip != null)))
				{
					//Display Loop Begin/Loop End text
					CDTXMania.actDisplayString.tPrint(60, y - 16, CCharacterConsole.EFontType.White, (bIsEnd ? "End loop" : "Begin loop"));

					if (CDTXMania.ConfigIni.nLaneDisp.Guitar == 0 || CDTXMania.ConfigIni.nLaneDisp.Guitar == 1)
					{
						this.txChip.vcScaleRatio.Y = 1.0f;
						this.txChip.tDraw2D(CDTXMania.app.Device, 88, y - 1, new Rectangle(0, 20, 193, 2));
						this.txChip.tDraw2D(CDTXMania.app.Device, 88, y + 1, new Rectangle(0, 20, 193, 2));
					}
				}
				y = CDTXMania.ConfigIni.bReverse.Bass ? ((this.nJudgeLinePosY.Bass - nDistanceFromBarBass) + 0) : ((this.nJudgeLinePosY.Bass + nDistanceFromBarBass) + 9);
				if ((CDTXMania.DTX.bチップがある.Bass && (y > 104)) && ((y < 670) && (this.txChip != null)))
				{
					//Display Loop Begin/Loop End text
					CDTXMania.actDisplayString.tPrint(930, y - 16, CCharacterConsole.EFontType.White, (bIsEnd ? "End loop" : "Begin loop"));

					if (CDTXMania.ConfigIni.nLaneDisp.Bass == 0 || CDTXMania.ConfigIni.nLaneDisp.Bass == 1)
					{
						this.txChip.vcScaleRatio.Y = 1.0f;
						this.txChip.tDraw2D(CDTXMania.app.Device, 959, y - 1, new Rectangle(0, 20, 193, 2));
						this.txChip.tDraw2D(CDTXMania.app.Device, 959, y + 1, new Rectangle(0, 20, 193, 2));
					}
				}
			}
		}
		#endregion
	}
}
