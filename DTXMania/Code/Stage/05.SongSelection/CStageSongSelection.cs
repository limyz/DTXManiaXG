using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using FDK;
using DirectShowLib;
using DiscordRPC;

using SlimDXKey = SlimDX.DirectInput.Key;

namespace DTXMania
{
	internal class CStageSongSelection : CStage
	{
        // プロパティ

        protected override RichPresence Presence => new CDTXRichPresence
        {
            State = "In Menu",
            Details = "Selecting a song",
        };

        public int nScrollbarRelativeYCoordinate
		{
			get
			{
				if ( actSongList != null )
				{
					return actSongList.nスクロールバー相対y座標;
				}
				else
				{
					return 0;
				}
			}
		}
		public bool bIsEnumeratingSongs
		{
			get
			{
				return actSongList.bIsEnumeratingSongs;
			}
			set
			{
				actSongList.bIsEnumeratingSongs = value;
			}
		}
		public bool bIsPlayingPremovie
		{
			get
			{
				return this.actPreimagePanel.bIsPlayingPremovie;
			}
		}
		public bool bScrolling
		{
			get
			{
				return this.actSongList.bScrolling;
			}
		}
		public int nConfirmedSongDifficulty
		{
			get;
			private set;
		}
		public CScore rChosenScore
		{
			get;
			private set;
		}
		public CSongListNode rConfirmedSong 
		{
			get;
			private set;
		}
        /// <summary>
        /// <para>現在演奏中の曲のスコアに対応する背景動画。</para>
        /// <para>r現在演奏中の曲のスコア の読み込み時に、自動検索_抽出_生成される。</para>
        /// </summary>
        //public CDirectShow r現在演奏中のスコアの背景動画 = null;
		public int nSelectedSongDifficultyLevel
		{
			get
			{
				return this.actSongList.n現在選択中の曲の現在の難易度レベル;
			}
		}
		public CScore rSelectedScore  // r現在選択中のスコア
		{
			get
			{
				return this.actSongList.rSelectedScore;
			}
		}
		public CSongListNode r現在選択中の曲
		{
			get
			{
				return this.actSongList.rSelectedSong;
			}
		}

		// コンストラクタ
		public CStageSongSelection()
		{
			base.eStageID = CStage.EStage.SongSelection;
			base.ePhaseID = CStage.EPhase.Common_DefaultState;
			base.bNotActivated = true;
//			base.listChildActivities.Add( this.actオプションパネル = new CActOptionPanel() );
			base.listChildActivities.Add( this.actFIFO = new CActFIFOBlack() );
			base.listChildActivities.Add( this.actFIfrom結果画面 = new CActFIFOBlack() );
//			base.listChildActivities.Add( this.actFOtoNowLoading = new CActFIFOBlack() );	// #27787 2012.3.10 yyagi 曲決定時の画面フェードアウトの省略
			base.listChildActivities.Add( this.actSongList = new CActSelectSongList() );
			base.listChildActivities.Add( this.actStatusPanel = new CActSelectStatusPanel() );
			base.listChildActivities.Add( this.actPerHistoryPanel = new CActSelectPerfHistoryPanel() );
			base.listChildActivities.Add( this.actPreimagePanel = new CActSelectPreimagePanel() );
			base.listChildActivities.Add( this.actPresound = new CActSelectPresound() );
			base.listChildActivities.Add( this.actArtistComment = new CActSelectArtistComment() );
			base.listChildActivities.Add( this.actInformation = new CActSelectInformation() );
			base.listChildActivities.Add( this.actSortSongs = new CActSortSongs() );
			base.listChildActivities.Add( this.actShowCurrentPosition = new CActSelectShowCurrentPosition() );
			base.listChildActivities.Add(this.actBackgroundVideoAVI = new CActSelectBackgroundAVI());
			base.listChildActivities.Add( this.actQuickConfig = new CActSelectQuickConfig() );

			//
			base.listChildActivities.Add(this.actTextBox = new CActTextBox());

			this.CommandHistory = new CCommandHistory();        // #24063 2011.1.16 yyagi
			//
			this.bCheckDrumsEnabled = CDTXMania.ConfigIni.bDrumsEnabled;
			this.bCheckRandSubBox = CDTXMania.ConfigIni.bランダムセレクトで子BOXを検索対象とする;
		}
		
		
		// メソッド

		public void tSelectedSongChanged()
		{
			this.actPreimagePanel.t選択曲が変更された();
			this.actPresound.t選択曲が変更された();
			this.actPerHistoryPanel.t選択曲が変更された();
			this.actStatusPanel.tSelectedSongChanged();
			this.actArtistComment.t選択曲が変更された();

			#region [ プラグインにも通知する（BOX, RANDOM, BACK なら通知しない）]
			//---------------------
			if( CDTXMania.app != null )
			{
				var c曲リストノード = CDTXMania.stageSongSelection.r現在選択中の曲;
				var cスコア = CDTXMania.stageSongSelection.rSelectedScore;

				if( c曲リストノード != null && cスコア != null && c曲リストノード.eNodeType == CSongListNode.ENodeType.SCORE )
				{
					string str選択曲ファイル名 = cスコア.FileInformation.AbsoluteFilePath;
					CSetDef setDef = null;
					int nブロック番号inSetDef = -1;
					int n曲番号inブロック = -1;

					if( !string.IsNullOrEmpty( c曲リストノード.pathSetDefの絶対パス ) && File.Exists( c曲リストノード.pathSetDefの絶対パス ) )
					{
						setDef = new CSetDef( c曲リストノード.pathSetDefの絶対パス );
						nブロック番号inSetDef = c曲リストノード.SetDefのブロック番号;
						n曲番号inブロック = CDTXMania.stageSongSelection.actSongList.n現在のアンカ難易度レベルに最も近い難易度レベルを返す( c曲リストノード );
					}

					foreach( CDTXMania.STPlugin stPlugin in CDTXMania.app.listPlugins )
					{
						Directory.SetCurrentDirectory( stPlugin.strプラグインフォルダ );
						stPlugin.plugin.On選択曲変更( str選択曲ファイル名, setDef, nブロック番号inSetDef, n曲番号inブロック );
						Directory.SetCurrentDirectory( CDTXMania.strEXEのあるフォルダ );
					}
				}
			}
			//---------------------
			#endregion
		}

		// CStage 実装

		/// <summary>
		/// 曲リストをリセットする
		/// </summary>
		/// <param name="cs"></param>
		public void Refresh( CSongManager cs, bool bRemakeSongTitleBar)
		{
			this.actSongList.Refresh( cs, bRemakeSongTitleBar );
		}

		public override void OnActivate()
		{
			Trace.TraceInformation( "選曲ステージを活性化します。" );
			Trace.Indent();
			try
			{
				this.eReturnValueWhenFadeOutCompleted = EReturnValue.Continue;
				this.bBGMPlayed = false;
				this.ftFont = new Font( "MS PGothic", 26f, GraphicsUnit.Pixel );
				this.ftSearchInputNotificationFont = new Font("MS PGothic", 14f, GraphicsUnit.Pixel);
				for( int i = 0; i < 4; i++ )
					this.ctKeyRepeat[ i ] = new CCounter( 0, 0, 0, CDTXMania.Timer );

				base.OnActivate();

				this.actTextBox.t検索説明文を表示する設定にする();
				this.actStatusPanel.tSelectedSongChanged(); // 最大ランクを更新

				//Reset random list upon reactivation only when a change in config for drumsEnabled or RandSubBox is detected
				bool bToReset = false;
				if(this.bCheckDrumsEnabled != CDTXMania.ConfigIni.bDrumsEnabled)
                {
					bToReset = true;
					this.bCheckDrumsEnabled = CDTXMania.ConfigIni.bDrumsEnabled;
                }

				if(this.bCheckRandSubBox != CDTXMania.ConfigIni.bランダムセレクトで子BOXを検索対象とする)
                {
					bToReset = true;
					this.bCheckRandSubBox = CDTXMania.ConfigIni.bランダムセレクトで子BOXを検索対象とする;
                }
				
				if (bToReset)
                {
					this.tResetRandomListForNode(null);
				}				
			}
			finally
			{
				Trace.TraceInformation( "選曲ステージの活性化を完了しました。" );
				Trace.Unindent();
			}
		}
		public override void OnDeactivate()
		{
			Trace.TraceInformation( "選曲ステージを非活性化します。" );
			Trace.Indent();
			try
			{
				if (this.rBackgroundVideoAVI != null)
				{
					this.rBackgroundVideoAVI.Dispose();
					this.rBackgroundVideoAVI = null;
				}

				if ( this.ftFont != null )
				{
					this.ftFont.Dispose();
					this.ftFont = null;
				}

				if(this.ftSearchInputNotificationFont != null)
                {
					this.ftSearchInputNotificationFont.Dispose();
					this.ftSearchInputNotificationFont = null;
                }

				for( int i = 0; i < 4; i++ )
				{
					this.ctKeyRepeat[ i ] = null;
				}
				base.OnDeactivate();
			}
			finally
			{
				Trace.TraceInformation( "選曲ステージの非活性化を完了しました。" );
				Trace.Unindent();
			}
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
				this.txBackground = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\5_background.jpg" ), false );
				this.txTopPanel = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\5_header panel.png" ), false );
				this.txBottomPanel = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\5_footer panel.png" ), false );
				this.prvFontSearchInputNotification = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.str選曲リストフォント), 14, FontStyle.Regular);
				//this.dsBackgroundVideo = CDTXMania.t失敗してもスキップ可能なDirectShowを生成する(CSkin.Path(@"Graphics\5_background.mp4"), CDTXMania.app.WindowHandle, true);
				this.txBPMLabel = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\5_BPM.png"), false);

				//
				this.rBackgroundVideoAVI = new CDTX.CAVI(1290, CSkin.Path(@"Graphics\5_background.mp4"), "", 20.0);
				this.rBackgroundVideoAVI.OnDeviceCreated();
				if (rBackgroundVideoAVI.avi != null)
				{					
					this.actBackgroundVideoAVI.bLoop = true;
					this.actBackgroundVideoAVI.Start(EChannel.MovieFull, rBackgroundVideoAVI, 0, -1);
					Trace.TraceInformation("選曲ムービーを有効化しました。");
				}

				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if( !base.bNotActivated )
			{
				actBackgroundVideoAVI.Stop();
				
				//CDTXMania.t安全にDisposeする( ref this.r現在演奏中のスコアの背景動画 );
				//CDTXMania.t安全にDisposeする(ref this.dsBackgroundVideo);			

				CDTXMania.tReleaseTexture( ref this.txBackground);
				CDTXMania.tReleaseTexture( ref this.txTopPanel);
				CDTXMania.tReleaseTexture( ref this.txBottomPanel);
				CDTXMania.tReleaseTexture(ref this.txBPMLabel);
				//
				CDTXMania.t安全にDisposeする(ref this.txSearchInputNotification);
				CDTXMania.t安全にDisposeする(ref this.prvFontSearchInputNotification);

				base.OnManagedReleaseResources();
			}
		}
		public override int OnUpdateAndDraw()
		{
			if( !base.bNotActivated )
			{
				#region [ 初めての進行描画 ]
				//---------------------
				if( base.bJustStartedUpdate )
				{
					this.ct登場時アニメ用共通 = new CCounter( 0, 100, 3, CDTXMania.Timer );
					if( CDTXMania.rPreviousStage == CDTXMania.stageResult )
					{
						this.actFIfrom結果画面.tフェードイン開始();
						base.ePhaseID = CStage.EPhase.選曲_結果画面からのフェードイン;
					}
					else
					{
						this.actFIFO.tフェードイン開始();
						base.ePhaseID = CStage.EPhase.Common_FadeIn;
					}
					this.ctSearchInputDisplayCounter = new CCounter(0, 1, 10000, CDTXMania.Timer);
					this.tSelectedSongChanged();
					base.bJustStartedUpdate = false;
				}
				//---------------------
				#endregion

				this.ct登場時アニメ用共通.tUpdate();
				this.ctSearchInputDisplayCounter.tUpdate();
                if (this.ctSearchInputDisplayCounter.bReachedEndValue)
                {
					this.tUpdateSearchNotification("");
                }

				//Draw Background video  via CActPerfAVI methods
				this.actBackgroundVideoAVI.tUpdateAndDraw();
				//Draw background video and image
				//if(this.dsBackgroundVideo != null)
    //            {
				//	this.dsBackgroundVideo.t再生開始();
				//	this.dsBackgroundVideo.MediaSeeking.GetPositions(out this.lDshowPosition, out this.lStopPosition);
				//	this.dsBackgroundVideo.bループ再生 = true;

				//	if (this.lDshowPosition == this.lStopPosition)
				//	{
				//		this.dsBackgroundVideo.MediaSeeking.SetPositions(
				//		DsLong.FromInt64((long)(0)),
				//		AMSeekingSeekingFlags.AbsolutePositioning,
				//		0,
				//		AMSeekingSeekingFlags.NoPositioning);
				//	}

				//	this.dsBackgroundVideo.t現時点における最新のスナップイメージをTextureに転写する(this.txBackground);
				//}

				if( this.txBackground != null && this.rBackgroundVideoAVI.avi == null)
                {
					this.txBackground.tDraw2D(CDTXMania.app.Device, 0, 0);
					//Removed drawing upside down	
				}

				if (this.txBPMLabel != null)
					this.txBPMLabel.tDraw2D(CDTXMania.app.Device, 32, 258);

				this.actPreimagePanel.OnUpdateAndDraw();
			//	this.bIsEnumeratingSongs = !this.actPreimageパネル.bIsPlayingPremovie;				// #27060 2011.3.2 yyagi: #PREMOVIE再生中は曲検索を中断する

				//this.actStatusPanel.OnUpdateAndDraw();
				this.actArtistComment.OnUpdateAndDraw();
				this.actSongList.OnUpdateAndDraw();
				this.actStatusPanel.OnUpdateAndDraw();
				this.actPerHistoryPanel.OnUpdateAndDraw();
				int y = 0;
				if( this.ct登場時アニメ用共通.bInProgress )
				{
					double db登場割合 = ( (double) this.ct登場時アニメ用共通.nCurrentValue ) / 100.0;	// 100が最終値
					double dbY表示割合 = Math.Sin( Math.PI / 2 * db登場割合 );
					y = ( (int) ( this.txTopPanel.szImageSize.Height * dbY表示割合 ) ) - this.txTopPanel.szImageSize.Height;
				}
				if( this.txTopPanel != null )
						this.txTopPanel.tDraw2D( CDTXMania.app.Device, 0, y );

				this.actInformation.OnUpdateAndDraw();
				if( this.txBottomPanel != null )
					this.txBottomPanel.tDraw2D( CDTXMania.app.Device, 0, 720 - this.txBottomPanel.szImageSize.Height );

				this.actPresound.OnUpdateAndDraw();
//				this.actオプションパネル.OnUpdateAndDraw();
				this.actShowCurrentPosition.OnUpdateAndDraw();								// #27648 2011.3.28 yyagi

				switch ( base.ePhaseID )
				{
					case CStage.EPhase.Common_FadeIn:
						if( this.actFIFO.OnUpdateAndDraw() != 0 )
						{
							base.ePhaseID = CStage.EPhase.Common_DefaultState;
						}
						break;

					case CStage.EPhase.Common_FadeOut:
						if( this.actFIFO.OnUpdateAndDraw() == 0 )
						{
							break;
						}
						return (int) this.eReturnValueWhenFadeOutCompleted;

					case CStage.EPhase.選曲_結果画面からのフェードイン:
						if( this.actFIfrom結果画面.OnUpdateAndDraw() != 0 )
						{
							base.ePhaseID = CStage.EPhase.Common_DefaultState;
						}
						break;

					case CStage.EPhase.選曲_NowLoading画面へのフェードアウト:
                        //if (this.actFOtoNowLoading.OnUpdateAndDraw() == 0)
                        //{
                        //    break;
                        //}
                        return (int) this.eReturnValueWhenFadeOutCompleted;
				}
				if( !this.bBGMPlayed && ( base.ePhaseID == CStage.EPhase.Common_DefaultState ) )
				{
					CDTXMania.Skin.bgm選曲画面.n音量_次に鳴るサウンド = 100;
					CDTXMania.Skin.bgm選曲画面.tPlay();
					this.bBGMPlayed = true;
				}


//Debug.WriteLine( "パンくず=" + this.r現在選択中の曲.strBreadcrumbs );


				// キー入力
				if( base.ePhaseID == CStage.EPhase.Common_DefaultState 
					&& CDTXMania.actPluginOccupyingInput == null )
				{
					#region [ 簡易CONFIGでMore、またはShift+F1: 詳細CONFIG呼び出し ]
					if (  actQuickConfig.bGotoDetailConfig )
					{	// 詳細CONFIG呼び出し
						actQuickConfig.tDeativatePopupMenu();
						this.actPresound.tサウンド停止();
						this.eReturnValueWhenFadeOutCompleted = EReturnValue.CallConfig;	// #24525 2011.3.16 yyagi: [SHIFT]-[F1]でCONFIG呼び出し
						this.actFIFO.tStartFadeOut();
						base.ePhaseID = CStage.EPhase.Common_FadeOut;
						CDTXMania.Skin.soundCancel.tPlay();
						return 0;
					}
					#endregion
					if ( !this.actSortSongs.bIsActivePopupMenu && !this.actQuickConfig.bIsActivePopupMenu && !CDTXMania.app.bテキスト入力中)
					{
                        #region [ ESC ]
                        if (CDTXMania.InputManager.Keyboard.bKeyPressed((int)SlimDXKey.Escape) || ((CDTXMania.Pad.bPressed(EInstrumentPart.DRUMS, EPad.LC) || CDTXMania.Pad.bPressedGB(EPad.Cancel)) && ((this.actSongList.rSelectedSong != null) && (this.actSongList.rSelectedSong.r親ノード == null))))
                        {	// [ESC]
                            CDTXMania.Skin.soundCancel.tPlay();
                            this.eReturnValueWhenFadeOutCompleted = EReturnValue.ReturnToTitle;
                            this.actFIFO.tStartFadeOut();
                            base.ePhaseID = CStage.EPhase.Common_FadeOut;
                            return 0;
                        }
                        #endregion
                        #region [ CONFIG画面 ]
                        if (CDTXMania.Pad.bPressed(EInstrumentPart.GUITAR, EPad.Help))
                        {	// [SHIFT] + [F1] CONFIG
                            this.actPresound.tサウンド停止();
                            this.eReturnValueWhenFadeOutCompleted = EReturnValue.CallConfig;	// #24525 2011.3.16 yyagi: [SHIFT]-[F1]でCONFIG呼び出し
                            this.actFIFO.tStartFadeOut();
                            base.ePhaseID = CStage.EPhase.Common_FadeOut;
                            CDTXMania.Skin.soundCancel.tPlay();
                            return 0;
                        }
						#endregion
						#region [ Shift-F2: 未使用 ]
						// #24525 2011.3.16 yyagi: [SHIFT]+[F2]は廃止(将来発生するかもしれない別用途のためにキープ)
						/*
                        if ((CDTXMania.InputManager.Keyboard.bKeyPressing((int)SlimDXKey.RightShift) || CDTXMania.InputManager.Keyboard.bKeyPressing((int)SlimDXKey.LeftShift)) &&
                            CDTXMania.InputManager.Keyboard.bKeyPressed((int)SlimDXKey.F2))
                        {	// [SHIFT] + [F2] CONFIGURATION
                            this.actPresound.tサウンド停止();
                            this.eReturnValueAfterFadeOut = EReturnValue.オプション呼び出し;
                            this.actFIFO.tStartFadeOut();
                            base.ePhaseID = CStage.EPhase.Common_FadeOut;
                            CDTXMania.Skin.soundCancel.tPlay();
                            return 0;
                        }
						*/
						#endregion
						if (this.actSongList.rSelectedSong != null)
                        {
                            #region [ Decide ]
                            if ((CDTXMania.Pad.bPressedDGB(EPad.Decide) || CDTXMania.Pad.bPressed(EInstrumentPart.DRUMS, EPad.CY) || CDTXMania.Pad.bPressed(EInstrumentPart.DRUMS, EPad.RD)) ||
                                (CDTXMania.ConfigIni.bEnterがキー割り当てのどこにも使用されていない && CDTXMania.InputManager.Keyboard.bKeyPressed((int)SlimDXKey.Return)))
                            {
                                if (this.actSongList.rSelectedSong != null)
                                {
                                    switch (this.actSongList.rSelectedSong.eNodeType)
                                    {
                                        case CSongListNode.ENodeType.SCORE:
                                            CDTXMania.Skin.soundDecide.tPlay();
                                            this.tSelectSong();
                                            break;

                                        case CSongListNode.ENodeType.SCORE_MIDI:
                                            CDTXMania.Skin.soundDecide.tPlay();
                                            this.tSelectSong();
                                            break;

                                        case CSongListNode.ENodeType.BOX:
                                            {
                                                CDTXMania.Skin.soundDecide.tPlay();
                                                bool bNeedChangeSkin = this.actSongList.tGoIntoBOX();
                                                if (bNeedChangeSkin)
                                                {
                                                    this.eReturnValueWhenFadeOutCompleted = EReturnValue.ChangeSking;
                                                    base.ePhaseID = EPhase.選曲_NowLoading画面へのフェードアウト;
                                                }
                                            }
                                            break;

                                        case CSongListNode.ENodeType.BACKBOX:
                                            {
                                                CDTXMania.Skin.soundCancel.tPlay();
                                                bool bNeedChangeSkin = this.actSongList.tExitBOX();
                                                if (bNeedChangeSkin)
                                                {
                                                    this.eReturnValueWhenFadeOutCompleted = EReturnValue.ChangeSking;
                                                    base.ePhaseID = EPhase.選曲_NowLoading画面へのフェードアウト;
                                                }
                                            }
                                            break;

                                        case CSongListNode.ENodeType.RANDOM:
                                            CDTXMania.Skin.soundDecide.tPlay();
                                            this.tSelectSongRandomly();
                                            break;
                                    }
                                }
                            }
                            #endregion
                            #region [ Up ]
                            this.ctKeyRepeat.Up.tRepeatKey(CDTXMania.InputManager.Keyboard.bKeyPressing((int)SlimDXKey.UpArrow), new CCounter.DGキー処理(this.tMoveCursorUp));
                            this.ctKeyRepeat.R.tRepeatKey(CDTXMania.Pad.bPressingGB(EPad.R), new CCounter.DGキー処理(this.tMoveCursorUp));
                            //SD changed to HT to follow Gitadora style
							if (CDTXMania.Pad.bPressed(EInstrumentPart.DRUMS, EPad.HT))
                            {
                                this.tMoveCursorUp();
                            }
                            #endregion
                            #region [ Down ]
                            this.ctKeyRepeat.Down.tRepeatKey(CDTXMania.InputManager.Keyboard.bKeyPressing((int)SlimDXKey.DownArrow), new CCounter.DGキー処理(this.tMoveCursorDown));
                            this.ctKeyRepeat.B.tRepeatKey(CDTXMania.Pad.bPressingGB(EPad.G), new CCounter.DGキー処理(this.tMoveCursorDown));
							//FT changed to LT to follow Gitadora style
							if (CDTXMania.Pad.bPressed(EInstrumentPart.DRUMS, EPad.LT))
                            {
                                this.tMoveCursorDown();
                            }
                            #endregion
                            #region [ Upstairs ]
                            if (((this.actSongList.rSelectedSong != null) && (this.actSongList.rSelectedSong.r親ノード != null)) && (CDTXMania.Pad.bPressed(EInstrumentPart.DRUMS, EPad.LC) || CDTXMania.Pad.bPressedGB(EPad.Cancel)))
                            {
                                this.actPresound.tサウンド停止();
                                CDTXMania.Skin.soundCancel.tPlay();
                                this.actSongList.tExitBOX();
                                this.tSelectedSongChanged();
                            }
                            #endregion
                            #region [ BDx2: 簡易CONFIG ]
                            if (CDTXMania.Pad.bPressed(EInstrumentPart.DRUMS, EPad.BD))
                            {	// [BD]x2 スクロール速度変更
                                CommandHistory.Add(EInstrumentPart.DRUMS, EPadFlag.BD);
                                EPadFlag[] comChangeScrollSpeed = new EPadFlag[] { EPadFlag.BD, EPadFlag.BD };
                                if (CommandHistory.CheckCommand(comChangeScrollSpeed, EInstrumentPart.DRUMS))
                                {
                                    // Debug.WriteLine( "ドラムススクロール速度変更" );
                                    // CDTXMania.ConfigIni.nScrollSpeed.Drums = ( CDTXMania.ConfigIni.nScrollSpeed.Drums + 1 ) % 0x10;
                                    CDTXMania.Skin.soundChange.tPlay();
                                    this.actQuickConfig.tActivatePopupMenu(EInstrumentPart.DRUMS);
                                }
                            }
                            #endregion
                            #region [ HHx2: 難易度変更 ]
                            if (CDTXMania.Pad.bPressed(EInstrumentPart.DRUMS, EPad.HH) || CDTXMania.Pad.bPressed(EInstrumentPart.DRUMS, EPad.HHO))
                            {	// [HH]x2 難易度変更
                                CommandHistory.Add(EInstrumentPart.DRUMS, EPadFlag.HH);
                                EPadFlag[] comChangeDifficulty = new EPadFlag[] { EPadFlag.HH, EPadFlag.HH };
                                if (CommandHistory.CheckCommand(comChangeDifficulty, EInstrumentPart.DRUMS))
                                {
                                    Debug.WriteLine("ドラムス難易度変更");
                                    this.actSongList.t難易度レベルをひとつ進める();
                                    //CDTXMania.Skin.sound変更音.tPlay();
                                }
                            }
                            #endregion
                            #region [ Bx2 Guitar: 難易度変更 ]
                            if (CDTXMania.Pad.bPressed(EInstrumentPart.GUITAR, EPad.B))	// #24177 2011.1.17 yyagi || -> &&
                            {	// [B]x2 ギター難易度変更
                                CommandHistory.Add(EInstrumentPart.GUITAR, EPadFlag.B);
                                EPadFlag[] comChangeDifficultyG = new EPadFlag[] { EPadFlag.B, EPadFlag.B };
                                if (CommandHistory.CheckCommand(comChangeDifficultyG, EInstrumentPart.GUITAR))
                                {
                                    Debug.WriteLine("ギター難易度変更");
                                    this.actSongList.t難易度レベルをひとつ進める();
                                    //CDTXMania.Skin.sound変更音.tPlay();
                                }
                            }
                            #endregion
                            #region [ Bx2 Bass: 難易度変更 ]
                            if (CDTXMania.Pad.bPressed(EInstrumentPart.BASS, EPad.B))		// #24177 2011.1.17 yyagi || -> &&
                            {	// [B]x2 ベース難易度変更
                                CommandHistory.Add(EInstrumentPart.BASS, EPadFlag.B);
                                EPadFlag[] comChangeDifficultyB = new EPadFlag[] { EPadFlag.B, EPadFlag.B };
                                if (CommandHistory.CheckCommand(comChangeDifficultyB, EInstrumentPart.BASS))
                                {
                                    Debug.WriteLine("ベース難易度変更");
                                    this.actSongList.t難易度レベルをひとつ進める();
                                    //CDTXMania.Skin.sound変更音.tPlay();
                                }
                            }
                            #endregion
                            #region [ Yx2 Guitar: ギターとベースを入れ替え ]
                            if (CDTXMania.Pad.bPressed(EInstrumentPart.GUITAR, EPad.Y))
                            {	// Pick, Y, Y, Pick で、ギターとベースを入れ替え
                                CommandHistory.Add(EInstrumentPart.GUITAR, EPadFlag.Y);
                                EPadFlag[] comSwapGtBs1 = new EPadFlag[] { EPadFlag.Y, EPadFlag.Y };
                                if (CommandHistory.CheckCommand(comSwapGtBs1, EInstrumentPart.GUITAR))
                                {
                                    Debug.WriteLine("ギターとベースの入れ替え1");
                                    CDTXMania.Skin.soundChange.tPlay();
                                    // ギターとベースのキーを入れ替え
                                    //CDTXMania.ConfigIni.SwapGuitarBassKeyAssign();
                                    CDTXMania.ConfigIni.bIsSwappedGuitarBass = !CDTXMania.ConfigIni.bIsSwappedGuitarBass;
									this.actSongList.tSwapClearLamps();
								}
                            }
                            #endregion
                            #region [ Yx2 Bass: ギターとベースを入れ替え ]
                            if (CDTXMania.Pad.bPressed(EInstrumentPart.BASS, EPad.Y))
                            {	// ベース[Pick]: コマンドとしてEnqueue
                                CommandHistory.Add(EInstrumentPart.BASS, EPadFlag.Y);
                                // Pick, Y, Y, Pick で、ギターとベースを入れ替え
                                EPadFlag[] comSwapGtBs1 = new EPadFlag[] { EPadFlag.Y, EPadFlag.Y };
                                if (CommandHistory.CheckCommand(comSwapGtBs1, EInstrumentPart.BASS))
                                {
                                    Debug.WriteLine("ギターとベースの入れ替え2");
                                    CDTXMania.Skin.soundChange.tPlay();
                                    // ギターとベースのキーを入れ替え
                                    //CDTXMania.ConfigIni.SwapGuitarBassKeyAssign();
                                    CDTXMania.ConfigIni.bIsSwappedGuitarBass = !CDTXMania.ConfigIni.bIsSwappedGuitarBass;
									this.actSongList.tSwapClearLamps();
								}
                            }
                            #endregion
                            #region [ Px2 Guitar: 簡易CONFIG ]
                            if (CDTXMania.Pad.bPressed(EInstrumentPart.GUITAR, EPad.P))
                            {	// [BD]x2 スクロール速度変更
                                CommandHistory.Add(EInstrumentPart.GUITAR, EPadFlag.P);
                                EPadFlag[] comChangeScrollSpeed = new EPadFlag[] { EPadFlag.P, EPadFlag.P };
                                if (CommandHistory.CheckCommand(comChangeScrollSpeed, EInstrumentPart.GUITAR))
                                {
                                    // Debug.WriteLine( "ドラムススクロール速度変更" );
                                    // CDTXMania.ConfigIni.nScrollSpeed.Drums = ( CDTXMania.ConfigIni.nScrollSpeed.Drums + 1 ) % 0x10;
                                    CDTXMania.Skin.soundChange.tPlay();
                                    this.actQuickConfig.tActivatePopupMenu(EInstrumentPart.GUITAR);
                                }
                            }
                            #endregion
                            #region [ Px2 Bass: 簡易CONFIG ]
                            if (CDTXMania.Pad.bPressed(EInstrumentPart.BASS, EPad.P))
                            {	// [BD]x2 スクロール速度変更
                                CommandHistory.Add(EInstrumentPart.BASS, EPadFlag.P);
                                EPadFlag[] comChangeScrollSpeed = new EPadFlag[] { EPadFlag.P, EPadFlag.P };
                                if (CommandHistory.CheckCommand(comChangeScrollSpeed, EInstrumentPart.BASS))
                                {
                                    // Debug.WriteLine( "ドラムススクロール速度変更" );
                                    // CDTXMania.ConfigIni.nScrollSpeed.Drums = ( CDTXMania.ConfigIni.nScrollSpeed.Drums + 1 ) % 0x10;
                                    CDTXMania.Skin.soundChange.tPlay();
                                    this.actQuickConfig.tActivatePopupMenu(EInstrumentPart.BASS);
                                }
                            }
                            #endregion
                            #region [ Y P Guitar: ソート画面 ]
                            if (CDTXMania.Pad.bPressing(EInstrumentPart.GUITAR, EPad.Y) && CDTXMania.Pad.bPressed(EInstrumentPart.GUITAR, EPad.P))
                            {	// ギター[Pick]: コマンドとしてEnqueue
                                CDTXMania.Skin.soundChange.tPlay();
                                this.actSortSongs.tActivatePopupMenu(EInstrumentPart.GUITAR, ref this.actSongList);
                            }
                            #endregion
                            #region [ Y P Bass: ソート画面 ]
                            if (CDTXMania.Pad.bPressing(EInstrumentPart.BASS, EPad.Y) && CDTXMania.Pad.bPressed(EInstrumentPart.BASS, EPad.P))
                            {	// ベース[Pick]: コマンドとしてEnqueue
                                CDTXMania.Skin.soundChange.tPlay();
                                this.actSortSongs.tActivatePopupMenu(EInstrumentPart.BASS, ref this.actSongList);
                            }
                            #endregion
                            #region [ FTx2 Drums: ソート画面 ]
                            if (CDTXMania.Pad.bPressed(EInstrumentPart.DRUMS, EPad.FT))
                            {	// [HT]x2 ソート画面        2013.12.31.kairera0467
								//Change to FT x 2 to follow Gitadora style
                                //
                                CommandHistory.Add(EInstrumentPart.DRUMS, EPadFlag.FT);
                                EPadFlag[] comSort = new EPadFlag[] { EPadFlag.FT, EPadFlag.FT };
                                if (CommandHistory.CheckCommand(comSort, EInstrumentPart.DRUMS))
                                {
                                    CDTXMania.Skin.soundChange.tPlay();
                                    this.actSortSongs.tActivatePopupMenu(EInstrumentPart.DRUMS, ref this.actSongList);
                                }
                            }
                            #endregion
                        }
                        //if( CDTXMania.InputManager.Keyboard.bKeyPressed((int)SlimDXKey.F6) )
                        //{
                        //    if (CDTXMania.EnumSongs.IsEnumerating)
                        //    {
                        //        // Debug.WriteLine( "バックグラウンドでEnumeratingSongs中だったので、一旦中断します。" );
                        //        CDTXMania.EnumSongs.Abort();
                        //        CDTXMania.actEnumSongs.OnDeactivate();
                        //    }

                        //    CDTXMania.EnumSongs.StartEnumFromDisk();
                        //    //CDTXMania.EnumSongs.ChangeEnumeratePriority(ThreadPriority.Normal);
                        //    CDTXMania.actEnumSongs.bコマンドでの曲データ取得 = true;
                        //    CDTXMania.actEnumSongs.OnActivate();
                        //}
					}

					#region [Test text field]
					if (!CDTXMania.app.bテキスト入力中 && CDTXMania.Pad.bPressed(EKeyConfigPart.SYSTEM, EKeyConfigPad.Search))
					{
						CDTXMania.Skin.soundDecide.tPlay();
						this.actTextBox.t表示();
						this.actTextBox.t入力を開始();
					}
					#endregion

					this.actSortSongs.tUpdateAndDraw();
					this.actQuickConfig.tUpdateAndDraw();
					this.actTextBox.OnUpdateAndDraw();
					if (actTextBox.b入力が終了した)
					{
						strSearchString = actTextBox.str確定文字列を返す();
						string searchOutcome = "";
						if(strSearchString != "" && strSearchString != CSongSearch.ExitSwitch)
                        {
							searchOutcome = "Search Input: " + strSearchString;
							Trace.TraceInformation("Search Input: " + strSearchString);
							if(CDTXMania.SongManager.listSongBeforeSearch == null)
                            {
								CDTXMania.SongManager.listSongBeforeSearch = CDTXMania.SongManager.listSongRoot;
							}

							List<CSongListNode> searchOutputList = CSongSearch.tSearchForSongs(CDTXMania.SongManager.listSongBeforeSearch, strSearchString);
							if(searchOutputList.Count == 0)
                            {
								Trace.TraceInformation("No songs found!");
								//To print a outcome message
								searchOutcome += "\r\nNo songs found";
							}
                            else
                            {
								CDTXMania.SongManager.listSongRoot = searchOutputList;

								//
								this.actSongList.SearchUpdate();
								//this.actSongList.Refresh(CDTXMania.SongManager, true);
							}

							this.tUpdateSearchNotification(searchOutcome);
							this.ctSearchInputDisplayCounter.tStart(0, 1, 10000, CDTXMania.Timer);
							CDTXMania.Skin.soundDecide.tPlay();
						}
						else if(strSearchString == CSongSearch.ExitSwitch)
                        {
							if(CDTXMania.SongManager.listSongBeforeSearch != null)
                            {
								CDTXMania.SongManager.listSongRoot = CDTXMania.SongManager.listSongBeforeSearch;
								CDTXMania.SongManager.listSongBeforeSearch = null;
								this.actSongList.SearchUpdate();
								this.tUpdateSearchNotification("Exit Search Mode");
								this.ctSearchInputDisplayCounter.tStart(0, 1, 10000, CDTXMania.Timer);
								CDTXMania.Skin.soundDecide.tPlay();
							}
                            else
                            {
								//Play cancel sound if input has no effect
								CDTXMania.Skin.soundCancel.tPlay(); 
							}
						}
                        else
                        {
							//Play cancel sound if input has no effect
							CDTXMania.Skin.soundCancel.tPlay();
						}						
						
						actTextBox.t非表示();
					}

					if(this.txSearchInputNotification != null)
                    {
						this.txSearchInputNotification.tDraw2D(CDTXMania.app.Device, 10, 130);
                    }

				}
			}
			return 0;
		}

		public enum EReturnValue : int  // E戻り値
		{
			Continue,      // 継続
			ReturnToTitle, // タイトルに戻る
			Selected,      // 選曲した
			CallConfig,    // コンフィグ呼び出し
			ChangeSking    // スキン変更
		}
		

		// Other

		#region [ private ]
		//-----------------
		[StructLayout( LayoutKind.Sequential )]
		private struct STKeyRepeatCounter  // STキー反復用カウンタ
		{
			public CCounter Up;
			public CCounter Down;
			public CCounter R;
			public CCounter B;
			public CCounter this[ int index ]
			{
				get
				{
					switch( index )
					{
						case 0:
							return this.Up;

						case 1:
							return this.Down;

						case 2:
							return this.R;

						case 3:
							return this.B;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch( index )
					{
						case 0:
							this.Up = value;
							return;

						case 1:
							this.Down = value;
							return;

						case 2:
							this.R = value;
							return;

						case 3:
							this.B = value;
							return;
					}
					throw new IndexOutOfRangeException();
				}
			}
		}
		private CActSelectArtistComment actArtistComment;
		private CActFIFOBlack actFIFO;
		private CActFIFOBlack actFIfrom結果画面;
//		private CActFIFOBlack actFOtoNowLoading;	// #27787 2012.3.10 yyagi 曲決定時の画面フェードアウトの省略
		private CActSelectInformation actInformation;
		private CActSelectPreimagePanel actPreimagePanel;  // actPreimageパネル
		private CActSelectPresound actPresound;
//		private CActOptionPanel actオプションパネル;
		public CActSelectStatusPanel actStatusPanel;  // actステータスパネル
		private CActSelectPerfHistoryPanel actPerHistoryPanel;  // act演奏履歴パネル
		private CActSelectSongList actSongList;
		private CActSelectShowCurrentPosition actShowCurrentPosition;
		private readonly CActSelectBackgroundAVI actBackgroundVideoAVI;

		private CActSortSongs actSortSongs;
		private CActSelectQuickConfig actQuickConfig;

		//
		private CActTextBox actTextBox;
		private string strSearchString;
		private bool bBGMPlayed;  // bBGM再生済み
		private STKeyRepeatCounter ctKeyRepeat;  // ctキー反復用
		public CCounter ct登場時アニメ用共通;
		private CCounter ctSearchInputDisplayCounter;
		private EReturnValue eReturnValueWhenFadeOutCompleted;  // eフェードアウト完了時の戻り値
		private Font ftFont;  // ftフォント
		private CTexture txBottomPanel;  // tx下部パネル
		private CTexture txTopPanel;  // tx上部パネル
		private CTexture txBackground;  // tx背景
		private CTexture txBPMLabel;
		//private CDirectShow dsBackgroundVideo; // background Video
		private CDTX.CAVI rBackgroundVideoAVI;// background Video using CAVI class
		private long lDshowPosition;
		private long lStopPosition;

		//
		private Font ftSearchInputNotificationFont;
		private CPrivateFastFont prvFontSearchInputNotification;
		private CTexture txSearchInputNotification = null;

		//
		private bool bCheckDrumsEnabled = false;
		private bool bCheckRandSubBox = false;

		private struct STCommandTime		// #24063 2011.1.16 yyagi コマンド入力時刻の記録用
		{
			public EInstrumentPart eInst;		// 使用楽器
			public EPadFlag ePad;		// 押されたコマンド(同時押しはOR演算で列挙する)
			public long time;				// コマンド入力時刻
		}
		public class CCommandHistory		// #24063 2011.1.16 yyagi コマンド入力履歴を保持_確認するクラス
		{
			readonly int buffersize = 16;
			private List<STCommandTime> stct;

			public CCommandHistory()		// コンストラクタ
			{
				stct = new List<STCommandTime>( buffersize );
			}

			/// <summary>
			/// コマンド入力履歴へのコマンド追加
			/// </summary>
			/// <param name="_eInst">楽器の種類</param>
			/// <param name="_ePad">入力コマンド(同時押しはOR演算で列挙すること)</param>
			public void Add( EInstrumentPart _eInst, EPadFlag _ePad )
			{
				STCommandTime _stct = new STCommandTime {
					eInst = _eInst,
					ePad = _ePad,
					time = CDTXMania.Timer.nCurrentTime
				};

				if ( stct.Count >= buffersize )
				{
					stct.RemoveAt( 0 );
				}
				stct.Add(_stct);
//Debug.WriteLine( "CMDHIS: 楽器=" + _stct.eInst + ", CMD=" + _stct.ePad + ", time=" + _stct.time );
			}
			public void RemoveAt( int index )
			{
				stct.RemoveAt( index );
			}

			/// <summary>
			/// コマンド入力に成功しているか調べる
			/// </summary>
			/// <param name="_ePad">入力が成功したか調べたいコマンド</param>
			/// <param name="_eInst">対象楽器</param>
			/// <returns>コマンド入力成功時true</returns>
			public bool CheckCommand( EPadFlag[] _ePad, EInstrumentPart _eInst)
			{
				int targetCount = _ePad.Length;
				int stciCount = stct.Count;
				if ( stciCount < targetCount )
				{
//Debug.WriteLine("NOT start checking...stciCount=" + stciCount + ", targetCount=" + targetCount);
					return false;
				}

				long curTime = CDTXMania.Timer.nCurrentTime;
//Debug.WriteLine("Start checking...targetCount=" + targetCount);
				for ( int i = targetCount - 1, j = stciCount - 1; i >= 0; i--, j-- )
				{
					if ( _ePad[ i ] != stct[ j ].ePad )
					{
//Debug.WriteLine( "CMD解析: false targetCount=" + targetCount + ", i=" + i + ", j=" + j + ": ePad[]=" + _ePad[i] + ", stci[j] = " + stct[j].ePad );
						return false;
					}
					if ( stct[ j ].eInst != _eInst )
					{
//Debug.WriteLine( "CMD解析: false " + i );
						return false;
					}
					if ( curTime - stct[ j ].time > 500 )
					{
//Debug.WriteLine( "CMD解析: false " + i + "; over 500ms" );
						return false;
					}
					curTime = stct[ j ].time;
				}

//Debug.Write( "CMD解析: 成功!(" + _ePad.Length + ") " );
//for ( int i = 0; i < _ePad.Length; i++ ) Debug.Write( _ePad[ i ] + ", " );
//Debug.WriteLine( "" );
				//stct.RemoveRange( 0, targetCount );			// #24396 2011.2.13 yyagi 
				stct.Clear();									// #24396 2011.2.13 yyagi Clear all command input history in case you succeeded inputting some command

				return true;
			}
		}
		public CCommandHistory CommandHistory;

		private void tMoveCursorDown()  // tカーソルを下へ移動する
		{
			CDTXMania.Skin.soundCursorMovement.tPlay();
			this.actSongList.tMoveToNext();
		}
		private void tMoveCursorUp()  // tカーソルを上へ移動する
		{
			CDTXMania.Skin.soundCursorMovement.tPlay();
			this.actSongList.tMoveToPrevious();
		}
		private void tSelectSongRandomly()
		{
			CSongListNode song = this.actSongList.rSelectedSong;
			if( ( song.stackRandomPerformanceNumber.Count == 0 ) || ( song.listランダム用ノードリスト == null ) )
			{
				if( song.listランダム用ノードリスト == null )
				{
					song.listランダム用ノードリスト = this.t指定された曲が存在する場所の曲を列挙する_子リスト含む( song );
				}
				int count = song.listランダム用ノードリスト.Count;
				if( count == 0 )
				{
					this.tUpdateSearchNotification(string.Format("Random Song List is empty for {0} mode",
					CDTXMania.ConfigIni.bDrumsEnabled ? "Drum" : "Guitar/Bass"
					));
					this.ctSearchInputDisplayCounter.tStart(0, 1, 10000, CDTXMania.Timer);
					return;
				}
				int[] numArray = new int[ count ];
				for( int i = 0; i < count; i++ )
				{
					numArray[ i ] = i;
				}
				for( int j = 0; j < ( count * 1.5 ); j++ )
				{
					int index = CDTXMania.Random.Next( count );
					int num5 = CDTXMania.Random.Next( count );
					int num6 = numArray[ num5 ];
					numArray[ num5 ] = numArray[ index ];
					numArray[ index ] = num6;
				}
				for( int k = 0; k < count; k++ )
				{
					song.stackRandomPerformanceNumber.Push( numArray[ k ] );
				}
				if( CDTXMania.ConfigIni.bLogDTX詳細ログ出力 )
				{
					StringBuilder builder = new StringBuilder( 0x400 );
					builder.Append( string.Format( "ランダムインデックスリストを作成しました: {0}曲: ", song.stackRandomPerformanceNumber.Count ) );
					for( int m = 0; m < count; m++ )
					{
						builder.Append( string.Format( "{0} ", numArray[ m ] ) );
					}
					Trace.TraceInformation( builder.ToString() );
				}
			}
			this.rConfirmedSong = song.listランダム用ノードリスト[ song.stackRandomPerformanceNumber.Pop() ];
			this.nConfirmedSongDifficulty = this.actSongList.n現在のアンカ難易度レベルに最も近い難易度レベルを返す( this.rConfirmedSong );
			this.rChosenScore = this.rConfirmedSong.arScore[ this.nConfirmedSongDifficulty ];
			this.eReturnValueWhenFadeOutCompleted = EReturnValue.Selected;
		//	this.actFOtoNowLoading.tStartFadeOut();					// #27787 2012.3.10 yyagi 曲決定時の画面フェードアウトの省略
			base.ePhaseID = CStage.EPhase.選曲_NowLoading画面へのフェードアウト;
			if( CDTXMania.ConfigIni.bLogDTX詳細ログ出力 )
			{
				int[] numArray2 = song.stackRandomPerformanceNumber.ToArray();
				StringBuilder builder2 = new StringBuilder( 0x400 );
				builder2.Append( "ランダムインデックスリスト残り: " );
				if( numArray2.Length > 0 )
				{
					for( int n = 0; n < numArray2.Length; n++ )
					{
						builder2.Append( string.Format( "{0} ", numArray2[ n ] ) );
					}
				}
				else
				{
					builder2.Append( "(なし)" );
				}
				Trace.TraceInformation( builder2.ToString() );
			}
			CDTXMania.Skin.bgm選曲画面.t停止する();
		}
		private void tSelectSong()  // t曲を選択する
		{
			this.rConfirmedSong = this.actSongList.rSelectedSong;
			this.rChosenScore = this.actSongList.rSelectedScore;
			this.nConfirmedSongDifficulty = this.actSongList.n現在選択中の曲の現在の難易度レベル;
			//
			bool bScoreExistForMode = this.tCheckScoreExistForMode(this.rChosenScore);
			if ( ( this.rConfirmedSong != null ) && ( this.rChosenScore != null ) && bScoreExistForMode)
			{
				this.eReturnValueWhenFadeOutCompleted = EReturnValue.Selected;
//				this.actFOtoNowLoading.tStartFadeOut();                 // #27787 2012.3.10 yyagi 曲決定時の画面フェードアウトの省略
				base.ePhaseID = CStage.EPhase.選曲_NowLoading画面へのフェードアウト;
			}
            else
            {
				this.tUpdateSearchNotification(string.Format("Score unavailable for {0} mode",
					CDTXMania.ConfigIni.bDrumsEnabled ? "Drum":"Guitar/Bass"
					));
				this.ctSearchInputDisplayCounter.tStart(0, 1, 10000, CDTXMania.Timer);
			}
			CDTXMania.Skin.bgm選曲画面.t停止する();
		}

		private bool tCheckScoreExistForMode(CScore score)
        {
			bool bScoreExistForMode = CDTXMania.ConfigIni.bDrumsEnabled && score.SongInformation.bScoreExists.Drums;
			if (!bScoreExistForMode)
			{
				bScoreExistForMode = CDTXMania.ConfigIni.bGuitarEnabled &&
					(score.SongInformation.bScoreExists.Guitar || score.SongInformation.bScoreExists.Bass);
			}
			return bScoreExistForMode;
		}

		private void tResetRandomListForNode(CSongListNode song)
        {
			//If songNode is null, start from root and recursively call each child
			if (song == null && CDTXMania.SongManager.listSongRoot.Count > 0)
			{
				foreach (CSongListNode cSong in CDTXMania.SongManager.listSongRoot)
				{
					tResetRandomListForNode(cSong);
				}
			}
            else
            {
				song.listランダム用ノードリスト = null;
				song.stackRandomPerformanceNumber = new Stack<int>();
				if(song.list子リスト != null)
                {
					foreach (CSongListNode cSong in song.list子リスト)
					{
						tResetRandomListForNode(cSong);
					}
				}
            }
        }

		private List<CSongListNode> t指定された曲が存在する場所の曲を列挙する_子リスト含む( CSongListNode song )
		{
			List<CSongListNode> list = new List<CSongListNode>();
			song = song.r親ノード;
			if( ( song == null ) && ( CDTXMania.SongManager.listSongRoot.Count > 0 ) )
			{
				foreach( CSongListNode c曲リストノード in CDTXMania.SongManager.listSongRoot )
				{
					if( ( c曲リストノード.eNodeType == CSongListNode.ENodeType.SCORE ) || ( c曲リストノード.eNodeType == CSongListNode.ENodeType.SCORE_MIDI ) )
					{
						//Check that at least one score is available for current game mode
						bool bAtLeastOneScoreExist = false;
                        for (int i = 0; i < 5; i++)
                        {
							if(c曲リストノード.arScore[i] != null)
                            {
								bAtLeastOneScoreExist = this.tCheckScoreExistForMode(c曲リストノード.arScore[i]);
								if(bAtLeastOneScoreExist)
                                {
									break;
                                }
                            }
						}

						//Add to list only if score exist for current mode
						if(bAtLeastOneScoreExist)
                        {
							list.Add(c曲リストノード);
						}						
					}
					if( ( c曲リストノード.list子リスト != null ) && CDTXMania.ConfigIni.bランダムセレクトで子BOXを検索対象とする )
					{
						this.t指定された曲の子リストの曲を列挙する_孫リスト含む( c曲リストノード, ref list );
					}
				}
				return list;
			}
			this.t指定された曲の子リストの曲を列挙する_孫リスト含む( song, ref list );
			return list;
		}
		private void t指定された曲の子リストの曲を列挙する_孫リスト含む( CSongListNode r親, ref List<CSongListNode> list )
		{
			if( ( r親 != null ) && ( r親.list子リスト != null ) )
			{
				foreach( CSongListNode c曲リストノード in r親.list子リスト )
				{
					if( ( c曲リストノード.eNodeType == CSongListNode.ENodeType.SCORE ) || ( c曲リストノード.eNodeType == CSongListNode.ENodeType.SCORE_MIDI ) )
					{
						//Check that at least one score is available for current game mode
						bool bAtLeastOneScoreExist = false;
						for (int i = 0; i < 5; i++)
						{
							if (c曲リストノード.arScore[i] != null)
							{
								bAtLeastOneScoreExist = this.tCheckScoreExistForMode(c曲リストノード.arScore[i]);
								if (bAtLeastOneScoreExist)
								{
									break;
								}
							}
						}
						//Add to list only if score exist for current mode
						if (bAtLeastOneScoreExist)
						{
							list.Add(c曲リストノード);
						}
					}
					if( ( c曲リストノード.list子リスト != null ) && CDTXMania.ConfigIni.bランダムセレクトで子BOXを検索対象とする )
					{
						this.t指定された曲の子リストの曲を列挙する_孫リスト含む( c曲リストノード, ref list );
					}
				}
			}
		}

		public void tUpdateSearchNotification(string strNotification)
        {
			CDTXMania.t安全にDisposeする(ref this.txSearchInputNotification);

			//
			if(strNotification != "")
            {
				//using (Bitmap bmp = prvFontSearchInputNotification.DrawPrivateFont(strNotification,
				//CPrivateFont.DrawMode.Edge, Color.White, Color.White, Color.White, Color.White, true))
				using (Bitmap bmp = prvFontSearchInputNotification.DrawPrivateFont(strNotification, Color.White, Color.Black))
				{
					this.txSearchInputNotification = CDTXMania.tGenerateTexture(bmp);
				}
			}
            else
            {
				this.txSearchInputNotification = null;
            }			

		}

		//-----------------
		#endregion
	}
}
