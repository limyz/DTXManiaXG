using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using SharpDX;
using SharpDX.Direct3D9;
using System.Drawing.Text;
using FDK;

using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;
using SlimDXKey = SlimDX.DirectInput.Key;

namespace DTXMania
{
    internal class CStageSongLoading : CStage
    {
        // retain presence from song select
        protected override RichPresence Presence => null;

        // コンストラクタ

        public CStageSongLoading()
        {
            base.eStageID = CStage.EStage.SongLoading;
            base.ePhaseID = CStage.EPhase.Common_DefaultState;
            base.bNotActivated = true;
            //			base.listChildActivities.Add( this.actFI = new CActFIFOBlack() );	// #27787 2012.3.10 yyagi 曲読み込み画面のフェードインの省略
            base.listChildActivities.Add(this.actFO = new CActFIFOBlackStart());

            #region[ 難易度数字 ]
            STCharacterPosition[] stCharacterPositionArray2 = new STCharacterPosition[11];
            STCharacterPosition st文字位置12 = new STCharacterPosition();
            st文字位置12.ch = '0';
            st文字位置12.pt = new Point(0, 0);
            stCharacterPositionArray2[0] = st文字位置12;
            STCharacterPosition st文字位置13 = new STCharacterPosition();
            st文字位置13.ch = '1';
            st文字位置13.pt = new Point(100, 0);
            stCharacterPositionArray2[1] = st文字位置13;
            STCharacterPosition st文字位置14 = new STCharacterPosition();
            st文字位置14.ch = '2';
            st文字位置14.pt = new Point(200, 0);
            stCharacterPositionArray2[2] = st文字位置14;
            STCharacterPosition st文字位置15 = new STCharacterPosition();
            st文字位置15.ch = '3';
            st文字位置15.pt = new Point(300, 0);
            stCharacterPositionArray2[3] = st文字位置15;
            STCharacterPosition st文字位置16 = new STCharacterPosition();
            st文字位置16.ch = '4';
            st文字位置16.pt = new Point(400, 0);
            stCharacterPositionArray2[4] = st文字位置16;
            STCharacterPosition st文字位置17 = new STCharacterPosition();
            st文字位置17.ch = '5';
            st文字位置17.pt = new Point(500, 0);
            stCharacterPositionArray2[5] = st文字位置17;
            STCharacterPosition st文字位置18 = new STCharacterPosition();
            st文字位置18.ch = '6';
            st文字位置18.pt = new Point(600, 0);
            stCharacterPositionArray2[6] = st文字位置18;
            STCharacterPosition st文字位置19 = new STCharacterPosition();
            st文字位置19.ch = '7';
            st文字位置19.pt = new Point(700, 0);
            stCharacterPositionArray2[7] = st文字位置19;
            STCharacterPosition st文字位置20 = new STCharacterPosition();
            st文字位置20.ch = '8';
            st文字位置20.pt = new Point(800, 0);
            stCharacterPositionArray2[8] = st文字位置20;
            STCharacterPosition st文字位置21 = new STCharacterPosition();
            st文字位置21.ch = '9';
            st文字位置21.pt = new Point(900, 0);
            stCharacterPositionArray2[9] = st文字位置21;
            STCharacterPosition st文字位置22 = new STCharacterPosition();
            st文字位置22.ch = '-';
            st文字位置22.pt = new Point(0, 40);
            stCharacterPositionArray2[10] = st文字位置22;
            this.st小文字位置 = stCharacterPositionArray2;

            //大文字
            STCharacterPosition[] st文字位置Array3 = new STCharacterPosition[12];
            STCharacterPosition st文字位置23 = new STCharacterPosition();
            st文字位置23.ch = '.';
            st文字位置23.pt = new Point(1000, 0);
            st文字位置Array3[0] = st文字位置23;
            STCharacterPosition st文字位置24 = new STCharacterPosition();
            st文字位置24.ch = '1';
            st文字位置24.pt = new Point(100, 0);
            st文字位置Array3[1] = st文字位置24;
            STCharacterPosition st文字位置25 = new STCharacterPosition();
            st文字位置25.ch = '2';
            st文字位置25.pt = new Point(200, 0);
            st文字位置Array3[2] = st文字位置25;
            STCharacterPosition st文字位置26 = new STCharacterPosition();
            st文字位置26.ch = '3';
            st文字位置26.pt = new Point(300, 0);
            st文字位置Array3[3] = st文字位置26;
            STCharacterPosition st文字位置27 = new STCharacterPosition();
            st文字位置27.ch = '4';
            st文字位置27.pt = new Point(400, 0);
            st文字位置Array3[4] = st文字位置27;
            STCharacterPosition st文字位置28 = new STCharacterPosition();
            st文字位置28.ch = '5';
            st文字位置28.pt = new Point(500, 0);
            st文字位置Array3[5] = st文字位置28;
            STCharacterPosition st文字位置29 = new STCharacterPosition();
            st文字位置29.ch = '6';
            st文字位置29.pt = new Point(600, 0);
            st文字位置Array3[6] = st文字位置29;
            STCharacterPosition st文字位置30 = new STCharacterPosition();
            st文字位置30.ch = '7';
            st文字位置30.pt = new Point(700, 0);
            st文字位置Array3[7] = st文字位置30;
            STCharacterPosition st文字位置31 = new STCharacterPosition();
            st文字位置31.ch = '8';
            st文字位置31.pt = new Point(800, 0);
            st文字位置Array3[8] = st文字位置31;
            STCharacterPosition st文字位置32 = new STCharacterPosition();
            st文字位置32.ch = '9';
            st文字位置32.pt = new Point(900, 0);
            st文字位置Array3[9] = st文字位置32;
            STCharacterPosition st文字位置33 = new STCharacterPosition();
            st文字位置33.ch = '0';
            st文字位置33.pt = new Point(0, 0);
            st文字位置Array3[10] = st文字位置33;
            STCharacterPosition st文字位置34 = new STCharacterPosition();
            st文字位置34.ch = '-';
            st文字位置34.pt = new Point(0, 0);
            st文字位置Array3[11] = st文字位置34;
            this.st大文字位置 = st文字位置Array3;
            #endregion

            this.stPanelMap = null;
            this.stPanelMap = new STATUSPANEL[12];		// yyagi: 以下、手抜きの初期化でスマン
            string[] labels = new string[12] {
            "DTXMANIA",     //0
            "DEBUT",        //1
            "NOVICE",       //2
            "REGULAR",      //3
            "EXPERT",       //4
            "MASTER",       //5
            "BASIC",        //6
            "ADVANCED",     //7
            "EXTREME",      //8
            "RAW",          //9
            "RWS",          //10
            "REAL"          //11
            };
            int[] status = new int[12] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

            for (int i = 0; i < 12; i++)
            {
                this.stPanelMap[i] = default(STATUSPANEL);
                this.stPanelMap[i].status = status[i];
                this.stPanelMap[i].label = labels[i];
            }
        }

        // CStage 実装

        public void tDetermineStatusLabelFromLabelName( string strラベル名 )
        {
            if( string.IsNullOrEmpty( strラベル名 ) )
            {
                this.nIndex = 0;
            }
            else
            {
                STATUSPANEL[] array = this.stPanelMap;
                for( int i = 0; i < array.Length; i++ )
                {
                    STATUSPANEL sTATUSPANEL = array[ i ];
                    if( strラベル名.Equals( sTATUSPANEL.label, StringComparison.CurrentCultureIgnoreCase ) )
                    {
                        this.nIndex = sTATUSPANEL.status;
                        CDTXMania.nSongDifficulty = sTATUSPANEL.status;
                        return;
                    }
                    this.nIndex++;
                }
            }
        }

        public override void OnActivate()
        {
            Trace.TraceInformation( "曲読み込みステージを活性化します。" );
            Trace.Indent();
            try
            {
                this.strSongTitle = "";
                this.strArtistName = "";
                this.strSTAGEFILE = "";

                this.nBGMPlayStartTime = -1L;
                this.nBGMTotalPlayTimeMs = 0;
                if( this.sdLoadingSound != null )
                {
                    CDTXMania.SoundManager.tDiscard( this.sdLoadingSound );
                    this.sdLoadingSound = null;
                }

                string strDTXFilePath = ( CDTXMania.bCompactMode ) ?
                    CDTXMania.strCompactModeFile : CDTXMania.stageSongSelection.rChosenScore.FileInformation.AbsoluteFilePath;

                CDTX cdtx = new CDTX( strDTXFilePath, true );

                if( !CDTXMania.bCompactMode && CDTXMania.ConfigIni.b曲名表示をdefのものにする )
                    this.strSongTitle = CDTXMania.stageSongSelection.rConfirmedSong.strタイトル;
                else
                    this.strSongTitle = cdtx.TITLE;

                this.strArtistName = cdtx.ARTIST;
                if( ( ( cdtx.SOUND_NOWLOADING != null ) && ( cdtx.SOUND_NOWLOADING.Length > 0 ) ) && File.Exists( cdtx.strFolderName + cdtx.SOUND_NOWLOADING )
                    && (!CDTXMania.DTXVmode.Enabled)
                    && (!CDTXMania.DTX2WAVmode.Enabled))
                {
                    string strNowLoadingサウンドファイルパス = cdtx.strFolderName + cdtx.SOUND_NOWLOADING;
                    try
                    {
                        this.sdLoadingSound = CDTXMania.SoundManager.tGenerateSound( strNowLoadingサウンドファイルパス );
                    }
                    catch
                    {
                        Trace.TraceError( "#SOUND_NOWLOADING に指定されたサウンドファイルの読み込みに失敗しました。({0})", strNowLoadingサウンドファイルパス );
                    }
                }
                // 2015.12.26 kairera0467 本家DTXからつまみ食い。
                // #35411 2015.08.19 chnmr0 add
                // Read ghost data by config
                // It does not exist a ghost file for 'perfect' actually
                string [] inst = {"dr", "gt", "bs"};
				if( CDTXMania.ConfigIni.bIsSwappedGuitarBass )
				{
					inst[1] = "bs";
					inst[2] = "gt";
				}

                for(int instIndex = 0; instIndex < inst.Length; ++instIndex)
                {
                    //break; //2016.01.03 kairera0467 以下封印。
                    bool readAutoGhostCond = false;
                    readAutoGhostCond |= instIndex == 0 ? CDTXMania.ConfigIni.bAllDrumsAreAutoPlay : false;
                    readAutoGhostCond |= instIndex == 1 ? CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay : false;
                    readAutoGhostCond |= instIndex == 2 ? CDTXMania.ConfigIni.bAllBassAreAutoPlay : false;

                    CDTXMania.listTargetGhsotLag[instIndex] = null;
                    CDTXMania.listAutoGhostLag[instIndex] = null;
                    CDTXMania.listTargetGhostScoreData[instIndex] = null;
                    this.nCurrentInst = instIndex;

                    if ( readAutoGhostCond )
                    {
                        string[] prefix = { "perfect", "lastplay", "hiskill", "hiscore", "online" };
                        int indPrefix = (int)CDTXMania.ConfigIni.eAutoGhost[ instIndex ];
                        string filename = cdtx.strFolderName + "\\" + cdtx.strファイル名 + "." + prefix[ indPrefix ] + "." + inst[ instIndex ] + ".ghost";
                        if( File.Exists( filename ) )
                        {
                            CDTXMania.listAutoGhostLag[ instIndex ] = new List<int>();
                            CDTXMania.listTargetGhostScoreData[ instIndex ] = new CScoreIni.CPerformanceEntry();
                            ReadGhost(filename, CDTXMania.listAutoGhostLag[ instIndex ]);
                        }
                    }

                    if( CDTXMania.ConfigIni.eTargetGhost[instIndex] != ETargetGhostData.NONE )
                    {
                        string[] prefix = { "none", "perfect", "lastplay", "hiskill", "hiscore", "online" };
                        int indPrefix = (int)CDTXMania.ConfigIni.eTargetGhost[ instIndex ];
                        string filename = cdtx.strFolderName + "\\" + cdtx.strファイル名 + "." + prefix[ indPrefix ] + "." + inst[ instIndex ] + ".ghost";
                        if( File.Exists( filename ) )
                        {
                            CDTXMania.listTargetGhsotLag[instIndex] = new List<int>();
                            CDTXMania.listTargetGhostScoreData[ instIndex ] = new CScoreIni.CPerformanceEntry();
                            this.stGhostLag[instIndex] = new List<STGhostLag>();
                            ReadGhost(filename, CDTXMania.listTargetGhsotLag[instIndex]);
                        }
                        else if( CDTXMania.ConfigIni.eTargetGhost[instIndex] == ETargetGhostData.PERFECT )
                        {
                            // All perfect
                            CDTXMania.listTargetGhsotLag[instIndex] = new List<int>();
                        }
                    }
                }

                cdtx.OnDeactivate();
                base.OnActivate();
                if( !CDTXMania.bCompactMode && !CDTXMania.DTXVmode.Enabled && !CDTXMania.DTX2WAVmode.Enabled)
                    this.tDetermineStatusLabelFromLabelName( CDTXMania.stageSongSelection.rConfirmedSong.arDifficultyLabel[ CDTXMania.stageSongSelection.nConfirmedSongDifficulty ] );
            }
            finally
            {
                Trace.TraceInformation( "曲読み込みステージの活性化を完了しました。" );
                Trace.Unindent();
            }
        }
        public override void OnDeactivate()
        {
            Trace.TraceInformation("曲読み込みステージを非活性化します。");
            Trace.Indent();
            try
            {
                base.OnDeactivate();
            }
            finally
            {
                Trace.TraceInformation("曲読み込みステージの非活性化を完了しました。");
                Trace.Unindent();
            }
        }
        public override void OnManagedCreateResources()
        {
            if (!base.bNotActivated)
            {
                this.txBackground = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\6_background.jpg" ) );
                this.txLevel = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\6_LevelNumber.png" ) );
                this.txDifficultyPanel = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\6_Difficulty.png" ) );
                this.txPartPanel = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\6_Part.png"));

                #region[ 曲名、アーティスト名テクスチャの生成 ]
                try
                {
                    #region[ 曲名、アーティスト名テクスチャの生成 ]
                    if ((this.strSongTitle != null) && (this.strSongTitle.Length > 0))
                    {
                        this.pfタイトル = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.str選曲リストフォント), 40, FontStyle.Regular);
                        Bitmap bmpSongName = new Bitmap(1, 1);
                        bmpSongName = this.pfタイトル.DrawPrivateFont(this.strSongTitle, CPrivateFont.DrawMode.Edge, Color.Black, Color.Black, this.clGITADORAgradationTopColor, this.clGITADORAgradationBottomColor, true);
                        this.txTitle = CDTXMania.tGenerateTexture(bmpSongName, false);
                        CDTXMania.tDisposeSafely( ref bmpSongName );
                        CDTXMania.tDisposeSafely( ref this.pfタイトル );
                    }
                    else
                    {
                        this.txTitle = null;
                    }

                    if ((this.strArtistName != null) && (this.strArtistName.Length > 0))
                    {
                        pfアーティスト = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.str選曲リストフォント), 30, FontStyle.Regular);
                        Bitmap bmpArtistName = new Bitmap(1, 1);
                        bmpArtistName = pfアーティスト.DrawPrivateFont(this.strArtistName, CPrivateFont.DrawMode.Edge, Color.Black, Color.Black, this.clGITADORAgradationTopColor, this.clGITADORAgradationBottomColor, true);
                        this.txArtist = CDTXMania.tGenerateTexture(bmpArtistName, false);
                        CDTXMania.tDisposeSafely( ref bmpArtistName );
                        CDTXMania.tDisposeSafely( ref this.pfアーティスト );
                    }
                    else
                    {
                        this.txArtist = null;
                    }
                    #endregion
                }
                catch( CTextureCreateFailedException )
                {
                    Trace.TraceError("テクスチャの生成に失敗しました。({0})", new object[] { this.strSTAGEFILE });
                    this.txTitle = null;
                    this.txBackground = null;
                }
                #endregion
                base.OnManagedCreateResources();
            }
        }
        public override void OnManagedReleaseResources()
        {
            if( !base.bNotActivated )
            {
                //テクスチャ11枚
                //2018.03.15 kairera0467 PrivateFontが抜けていた＆フォント生成直後に解放するようにしてみる
                CDTXMania.tReleaseTexture( ref this.txBackground );
                CDTXMania.tReleaseTexture( ref this.txJacket );
                CDTXMania.tReleaseTexture( ref this.txTitle );
                CDTXMania.tReleaseTexture( ref this.txArtist );
                CDTXMania.tReleaseTexture( ref this.txDifficultyPanel );
                CDTXMania.tReleaseTexture( ref this.txPartPanel );
                CDTXMania.tReleaseTexture( ref this.txLevel );
                base.OnManagedReleaseResources();
            }
        }
        public override int OnUpdateAndDraw()
        {
            string str;

            if( base.bNotActivated )
                return 0;

            #region [ 初めての進行描画 ]
            //-----------------------------
            if (base.bJustStartedUpdate)
            {
                CScore cScore1 = CDTXMania.stageSongSelection.rChosenScore;
                if (this.sdLoadingSound != null)
                {
                    if (CDTXMania.Skin.soundNowLoading.bExclusive && (CSkin.CSystemSound.rLastPlayedExclusiveSystemSound != null))
                    {
                        CSkin.CSystemSound.rLastPlayedExclusiveSystemSound.t停止する();
                    }
                    this.sdLoadingSound.tStartPlaying();
                    this.nBGMPlayStartTime = CSoundManager.rcPerformanceTimer.nCurrentTime;
                    this.nBGMTotalPlayTimeMs = this.sdLoadingSound.nTotalPlayTimeMs;
                }
                else if (!CDTXMania.DTXVmode.Enabled && !CDTXMania.DTX2WAVmode.Enabled)
                {
                    
                    CDTXMania.Skin.soundNowLoading.tPlay();
                    this.nBGMPlayStartTime = CSoundManager.rcPerformanceTimer.nCurrentTime;
                    this.nBGMTotalPlayTimeMs = CDTXMania.Skin.soundNowLoading.nLength_CurrentSound;
                }
                //				this.actFI.tフェードイン開始();							// #27787 2012.3.10 yyagi 曲読み込み画面のフェードインの省略
                base.ePhaseID = CStage.EPhase.Common_FadeIn;
                base.bJustStartedUpdate = false;
                // Already done in OnActivate
                //this.tDetermineStatusLabelFromLabelName( CDTXMania.stageSongSelection.rConfirmedSong.arDifficultyLabel[ CDTXMania.stageSongSelection.nConfirmedSongDifficulty ] );

                nWAVcount = 1;
            }
            //-----------------------------
            #endregion

            #region [ ESC押下時は選曲画面に戻る ]
            if (tHandleKeyInput())
            {
                if (this.sdLoadingSound != null)
                {
                    this.sdLoadingSound.tStopSound();
                    this.sdLoadingSound.tRelease();
                }
                return (int)ESongLoadingScreenReturnValue.LoadingStopped;
            }
            #endregion

            #region [ 背景、レベル、タイトル表示 ]
            //-----------------------------
            if( this.txBackground != null )
                this.txBackground.tDraw2D( CDTXMania.app.Device, 0, 0 );

            string strDTXFilePath = (CDTXMania.bCompactMode) ?
            CDTXMania.strCompactModeFile : CDTXMania.stageSongSelection.rChosenScore.FileInformation.AbsoluteFilePath;
            CDTX cdtx = new CDTX(strDTXFilePath, true);

            string path = cdtx.strFolderName + cdtx.PREIMAGE;
            try
            {
                if( this.txJacket == null ) // 2019.04.26 kairera0467
                {
                    if( !File.Exists( path ) )
                    {
                        this.txJacket = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\5_preimage default.png" ) );
                    }
                    else
                    {
                        this.txJacket = CDTXMania.tGenerateTexture( path );
                    }
                }
            }
            catch( Exception ex )
            {
                Trace.TraceError( ex.StackTrace );
            }


            int y = 184;

            if( this.txJacket != null )
            {  
                Matrix mat = Matrix.Identity;
                float fScalingFactor;
                float jacketOnScreenSize = 384.0f;
                //Maintain aspect ratio by scaling only to the smaller scalingFactor
                if (jacketOnScreenSize / this.txJacket.szImageSize.Width > jacketOnScreenSize / this.txJacket.szImageSize.Height) 
                {
                    fScalingFactor = jacketOnScreenSize / this.txJacket.szImageSize.Height;
                }
                else
                {
                    fScalingFactor = jacketOnScreenSize / this.txJacket.szImageSize.Width;
                }
                mat *= Matrix.Scaling(fScalingFactor, fScalingFactor, 1f);
                mat *= Matrix.Translation(206f, 66f, 0f);
                mat *= Matrix.RotationZ(0.28f);

                this.txJacket.tDraw3D(CDTXMania.app.Device, mat);
            }

            if (this.txTitle != null)
            {
                if (this.txTitle.szImageSize.Width > 625)
                    this.txTitle.vcScaleRatio.X = 625f / this.txTitle.szImageSize.Width;

                this.txTitle.tDraw2D(CDTXMania.app.Device, 190, 285);
            }

            if (this.txArtist != null)
            {
                if (this.txArtist.szImageSize.Width > 625)
                    this.txArtist.vcScaleRatio.X = 625f / this.txArtist.szImageSize.Width;

                this.txArtist.tDraw2D(CDTXMania.app.Device, 190, 360);
            }

            int[] iPart = { 0, CDTXMania.ConfigIni.bIsSwappedGuitarBass ? 2 : 1, CDTXMania.ConfigIni.bIsSwappedGuitarBass ? 1 : 2 };

            int j = 0;
            int k = 0;
            int DTXLevel = 0;
            double DTXLevelDeci = 0;

            for (int i = 0; i < 3; i++)
            {
                j = iPart[i];

                DTXLevel = cdtx.LEVEL[j];
                DTXLevelDeci = cdtx.LEVELDEC[j];

                if ((CDTXMania.ConfigIni.bDrumsEnabled && i == 0) || (CDTXMania.ConfigIni.bGuitarEnabled && i != 0))
                {

                    if (DTXLevel != 0 || DTXLevelDeci != 0)
                    {
                        //Always display CLASSIC style if Skill Mode is Classic
                        if (CDTXMania.ConfigIni.nSkillMode == 0 || (CDTXMania.ConfigIni.bCLASSIC譜面判別を有効にする && 
                            CDTXMania.stageSongSelection.rChosenScore.SongInformation.b完全にCLASSIC譜面である[j] && 
                            !cdtx.b強制的にXG譜面にする))
                        {
                            this.tDrawStringLarge(187 + k, 152, string.Format("{0:00}", DTXLevel));
                        }
                        else
                        {
                            if (cdtx.LEVEL[j] > 99)
                            {
                                DTXLevel = cdtx.LEVEL[j] / 100;
                                DTXLevelDeci = cdtx.LEVEL[j] - (DTXLevel * 100);
                            }
                            else
                            {
                                DTXLevel = cdtx.LEVEL[j] / 10;
                                DTXLevelDeci = ((cdtx.LEVEL[j] - DTXLevel * 10) * 10) + cdtx.LEVELDEC[j];
                            }                            
                            
                            this.txLevel.tDraw2D(CDTXMania.app.Device, 282 + k, 243, new Rectangle(1000, 92, 30, 38));
                            this.tDrawStringLarge(187 + k, 152, string.Format("{0:0}", DTXLevel));
                            this.tDrawStringLarge(307 + k, 152, string.Format("{0:00}", DTXLevelDeci));

                        }

                        if (this.txPartPanel != null)
                            this.txPartPanel.tDraw2D(CDTXMania.app.Device, 191 + k, 52, new Rectangle(0, j * 50, 262, 50));

                        //this.txJacket.Dispose();
                        if (!CDTXMania.bCompactMode && !CDTXMania.DTXVmode.Enabled && !CDTXMania.DTX2WAVmode.Enabled)
                            this.tDrawDifficultyPanel( CDTXMania.stageSongSelection.rConfirmedSong.arDifficultyLabel[ CDTXMania.stageSongSelection.nConfirmedSongDifficulty ], 191 + k, 102 );

                        k = 700;
                    }
                }

                if (i == 2 && k == 0)
                {
                    if (this.txPartPanel != null && CDTXMania.ConfigIni.bDrumsEnabled)
                        this.txPartPanel.tDraw2D(CDTXMania.app.Device, 191, 52, new Rectangle(0, 0, 262, 50));

                    if (this.txDifficultyPanel != null)
                        this.txDifficultyPanel.tDraw2D(CDTXMania.app.Device, 191, 102, new Rectangle(0, this.nIndex * 50, 262, 50));
                }
            }
            //-----------------------------
            #endregion

            switch (base.ePhaseID)
            {
                case CStage.EPhase.Common_FadeIn:
                    //if( this.actFI.OnUpdateAndDraw() != 0 )					// #27787 2012.3.10 yyagi 曲読み込み画面のフェードインの省略
                    // 必ず一度「CStaeg.EPhase.Common_FadeIn」フェーズを経由させること。
                    // さもないと、曲読み込みが完了するまで、曲読み込み画面が描画されない。
                    base.ePhaseID = CStage.EPhase.NOWLOADING_DTX_FILE_READING;
                    return (int)ESongLoadingScreenReturnValue.Continue;

                case CStage.EPhase.NOWLOADING_DTX_FILE_READING:
                    {
                        timeBeginLoad = DateTime.Now;
                        TimeSpan span;
                        str = null;
                        if (!CDTXMania.bCompactMode)
                            str = CDTXMania.stageSongSelection.rChosenScore.FileInformation.AbsoluteFilePath;
                        else
                            str = CDTXMania.strCompactModeFile;

                        CScoreIni ini = new CScoreIni(str + ".score.ini");
                        ini.tCheckIntegrity();

                        if ((CDTXMania.DTX != null) && CDTXMania.DTX.bActivated)
                            CDTXMania.DTX.OnDeactivate();

                        CDTXMania.DTX = new CDTX(str, false, ((double)CDTXMania.ConfigIni.nPlaySpeed) / 20.0, ini.stFile.BGMAdjust);
                        Trace.TraceInformation("----曲情報-----------------");
                        Trace.TraceInformation("TITLE: {0}", CDTXMania.DTX.TITLE);
                        Trace.TraceInformation("FILE: {0}", CDTXMania.DTX.strファイル名の絶対パス);
                        Trace.TraceInformation("---------------------------");

                       // #35411 2015.08.19 chnmr0 add ゴースト機能のためList chip 読み込み後楽器パート出現順インデックスを割り振る
                        int[] curCount = new int[(int)EInstrumentPart.UNKNOWN];
                        for (int i = 0; i < curCount.Length; ++i)
                        {
                            curCount[i] = 0;
                        }
                        foreach (CChip chip in CDTXMania.DTX.listChip)
                        {
                            if (chip.eInstrumentPart != EInstrumentPart.UNKNOWN)
                            {
                                chip.n楽器パートでの出現順 = curCount[(int)chip.eInstrumentPart]++;
                                if( CDTXMania.listTargetGhsotLag[ (int)chip.eInstrumentPart ] != null )
                                {
                                    var lag = new STGhostLag();
                                    lag.index = chip.n楽器パートでの出現順;
                                    lag.nJudgeTime = chip.nPlaybackTimeMs + CDTXMania.listTargetGhsotLag[ (int)chip.eInstrumentPart ][ chip.n楽器パートでの出現順 ];
                                    lag.nLagTime = CDTXMania.listTargetGhsotLag[ (int)chip.eInstrumentPart ][ chip.n楽器パートでの出現順 ];

                                    this.stGhostLag[ (int)chip.eInstrumentPart ].Add( lag );
                                }
                            }
                        }
                        
                        string [] inst = {"dr", "gt", "bs"};
        				if( CDTXMania.ConfigIni.bIsSwappedGuitarBass )
				        {
		        			inst[1] = "bs";
        					inst[2] = "gt";
				        }
                        //演奏記録をゴーストから逆生成
                        for( int i = 0; i < 3; i++ )
                        {
                            int nNowCombo = 0;
                            int nMaxCombo = 0;

                            //2016.06.18 kairera0467 「.ghost.score」ファイルが無かった場合ghostファイルから逆算を行う形に変更。
                            string[] prefix = { "none", "perfect", "lastplay", "hiskill", "hiscore", "online" };
                            int indPrefix = (int)CDTXMania.ConfigIni.eTargetGhost[ i ];
                            string filename = cdtx.strFolderName + "\\" + cdtx.strファイル名 + "." + prefix[ indPrefix ] + "." + inst[ i ] + ".ghost";

                            if( this.stGhostLag[ i ] == null || File.Exists( filename + ".score" ) )
                                continue;
                            CDTXMania.listTargetGhostScoreData[ i ] = new CScoreIni.CPerformanceEntry();

                            for( int n = 0; n < this.stGhostLag[ i ].Count; n++ )
                            {
                                int ghostLag = 128;
                                ghostLag = this.stGhostLag[ i ][ n ].nLagTime;
                                // 上位８ビットが１ならコンボが途切れている（ギターBAD空打ちでコンボ数を再現するための措置）
                                if (ghostLag > 255)
                                {
                                    nNowCombo = 0;
                                }
                                ghostLag = (ghostLag & 255) - 128;

                                if( ghostLag <= 127 )
                                {
                                    EJudgement eJudge = this.e指定時刻からChipのJUDGEを返す( ghostLag, 0, (EInstrumentPart)i );

                                    switch( eJudge )
                                    {
                                        case EJudgement.Perfect:
                                            CDTXMania.listTargetGhostScoreData[ i ].nPerfectCount++;
                                            break;
                                        case EJudgement.Great:
                                            CDTXMania.listTargetGhostScoreData[ i ].nGreatCount++;
                                            break;
                                        case EJudgement.Good:
                                            CDTXMania.listTargetGhostScoreData[ i ].nGoodCount++;
                                            break;
                                        case EJudgement.Poor:
                                            CDTXMania.listTargetGhostScoreData[ i ].nPoorCount++;
                                            break;
                                        case EJudgement.Miss:
                                        case EJudgement.Bad:
                                            CDTXMania.listTargetGhostScoreData[ i ].nMissCount++;
                                            break;
                                    }
                                    switch( eJudge )
                                    {
                                        case EJudgement.Perfect:
                                        case EJudgement.Great:
                                        case EJudgement.Good:
                                            nNowCombo++;
                                            CDTXMania.listTargetGhostScoreData[ i ].nMaxCombo = Math.Max( nNowCombo, CDTXMania.listTargetGhostScoreData[ i ].nMaxCombo );
                                            break;
                                        case EJudgement.Poor:
                                        case EJudgement.Miss:
                                        case EJudgement.Bad:
                                            CDTXMania.listTargetGhostScoreData[ i ].nMaxCombo = Math.Max( nNowCombo, CDTXMania.listTargetGhostScoreData[ i ].nMaxCombo );
                                            nNowCombo = 0;
                                            break;
                                    }
                                    //Trace.WriteLine( eJudge.ToString() + " " + nNowCombo.ToString() + "Combo Max:" + nMaxCombo.ToString() + "Combo" );
                                }
                            }
                            //CDTXMania.listTargetGhostScoreData[ i ].nMaxCombo = nMaxCombo;
                            int nTotal = CDTXMania.DTX.nVisibleChipsCount.Drums;
                            if( i == 1 ) nTotal = CDTXMania.DTX.nVisibleChipsCount.Guitar;
                            else if( i == 2 ) nTotal = CDTXMania.DTX.nVisibleChipsCount.Bass;
                            if( CDTXMania.ConfigIni.nSkillMode == 0 )
                            {
                                CDTXMania.listTargetGhostScoreData[ i ].dbPerformanceSkill = CScoreIni.tCalculatePlayingSkillOld( nTotal, CDTXMania.listTargetGhostScoreData[ i ].nPerfectCount, CDTXMania.listTargetGhostScoreData[ i ].nGreatCount, CDTXMania.listTargetGhostScoreData[ i ].nGoodCount, CDTXMania.listTargetGhostScoreData[ i ].nPoorCount, CDTXMania.listTargetGhostScoreData[ i ].nMissCount, CDTXMania.listTargetGhostScoreData[i].nMaxCombo, (EInstrumentPart)i, CDTXMania.listTargetGhostScoreData[ i ].bAutoPlay );
                            }
                            else
                            {
                                CDTXMania.listTargetGhostScoreData[ i ].dbPerformanceSkill = CScoreIni.tCalculatePlayingSkill( nTotal, CDTXMania.listTargetGhostScoreData[ i ].nPerfectCount, CDTXMania.listTargetGhostScoreData[ i ].nGreatCount, CDTXMania.listTargetGhostScoreData[ i ].nGoodCount, CDTXMania.listTargetGhostScoreData[ i ].nPoorCount, CDTXMania.listTargetGhostScoreData[ i ].nMissCount, CDTXMania.listTargetGhostScoreData[ i ].nMaxCombo, (EInstrumentPart)i, CDTXMania.listTargetGhostScoreData[ i ].bAutoPlay );
                            }
                        }

                        span = (TimeSpan)(DateTime.Now - timeBeginLoad);
                        Trace.TraceInformation("DTX読込所要時間:           {0}", span.ToString());

                        if (CDTXMania.bCompactMode)
                            CDTXMania.DTX.MIDIレベル = 1;
                        else
                            CDTXMania.DTX.MIDIレベル = (CDTXMania.stageSongSelection.rConfirmedSong.eNodeType == CSongListNode.ENodeType.SCORE_MIDI) ? CDTXMania.stageSongSelection.nSelectedSongDifficultyLevel : 0;

                        base.ePhaseID = CStage.EPhase.NOWLOADING_WAV_FILE_READING;
                        timeBeginLoadWAV = DateTime.Now;
                        return (int)ESongLoadingScreenReturnValue.Continue;
                    }

                case CStage.EPhase.NOWLOADING_WAV_FILE_READING:
                    {
                        if (nWAVcount == 1 && CDTXMania.DTX.listWAV.Count > 0)			// #28934 2012.7.7 yyagi (added checking Count)
                        {
                            //ShowProgressByFilename(CDTXMania.DTX.listWAV[nWAVcount].strFilename);
                        }
                        int looptime = (CDTXMania.ConfigIni.bVerticalSyncWait) ? 3 : 1;	// VSyncWait=ON時は1frame(1/60s)あたり3つ読むようにする
                        for (int i = 0; i < looptime && nWAVcount <= CDTXMania.DTX.listWAV.Count; i++)
                        {
                            if (CDTXMania.DTX.listWAV[nWAVcount].listこのWAVを使用するチャンネル番号の集合.Count > 0)	// #28674 2012.5.8 yyagi
                            {
                                CDTXMania.DTX.tLoadWAV(CDTXMania.DTX.listWAV[nWAVcount]);
                            }
                            nWAVcount++;
                        }
                        if (nWAVcount <= CDTXMania.DTX.listWAV.Count)
                        {
                            //ShowProgressByFilename(CDTXMania.DTX.listWAV[nWAVcount].strFilename);
                        }
                        if (nWAVcount > CDTXMania.DTX.listWAV.Count)
                        {
                            TimeSpan span = (TimeSpan)(DateTime.Now - timeBeginLoadWAV);
                            Trace.TraceInformation("WAV読込所要時間({0,4}):     {1}", CDTXMania.DTX.listWAV.Count, span.ToString());
                            timeBeginLoadWAV = DateTime.Now;

                            if (CDTXMania.ConfigIni.bDynamicBassMixerManagement)
                            {
                                CDTXMania.DTX.PlanToAddMixerChannel();
                            }
                            CDTXMania.DTX.t旧仕様のドコドコチップを振り分ける(EInstrumentPart.DRUMS, CDTXMania.ConfigIni.bAssignToLBD.Drums);
                            CDTXMania.DTX.tドコドコ仕様変更(EInstrumentPart.DRUMS, CDTXMania.ConfigIni.eDkdkType.Drums);
                            CDTXMania.DTX.tドラムのランダム化(EInstrumentPart.DRUMS, CDTXMania.ConfigIni.eRandom.Drums);
                            CDTXMania.DTX.tRandomizeDrumPedal(EInstrumentPart.DRUMS, CDTXMania.ConfigIni.eRandomPedal.Drums);
                            CDTXMania.DTX.t譜面仕様変更(EInstrumentPart.DRUMS, CDTXMania.ConfigIni.eNumOfLanes.Drums);
                            CDTXMania.DTX.tRandomizeGuitarAndBass(EInstrumentPart.GUITAR, CDTXMania.ConfigIni.eRandom.Guitar);
                            CDTXMania.DTX.tRandomizeGuitarAndBass(EInstrumentPart.BASS, CDTXMania.ConfigIni.eRandom.Bass);

                            if (CDTXMania.ConfigIni.bGuitarRevolutionMode)
                                CDTXMania.stagePerfGuitarScreen.OnActivate();
                            else
                                CDTXMania.stagePerfDrumsScreen.OnActivate();

                            span = (TimeSpan)(DateTime.Now - timeBeginLoadWAV);
                            Trace.TraceInformation("WAV/譜面後処理時間({0,4}):  {1}", (CDTXMania.DTX.listBMP.Count + CDTXMania.DTX.listBMPTEX.Count + CDTXMania.DTX.listAVI.Count), span.ToString());

                            base.ePhaseID = CStage.EPhase.NOWLOADING_BMP_FILE_READING;
                        }
                        return (int)ESongLoadingScreenReturnValue.Continue;
                    }

                case CStage.EPhase.NOWLOADING_BMP_FILE_READING:
                    {
                        TimeSpan span;
                        DateTime timeBeginLoadBMPAVI = DateTime.Now;
                        if (CDTXMania.ConfigIni.bBGAEnabled)
                            CDTXMania.DTX.tLoadBMP_BMPTEX();

                        if (CDTXMania.ConfigIni.bAVIEnabled)
                            CDTXMania.DTX.tLoadAVI();
                        span = (TimeSpan)(DateTime.Now - timeBeginLoadBMPAVI);
                        Trace.TraceInformation("BMP/AVI読込所要時間({0,4}): {1}", (CDTXMania.DTX.listBMP.Count + CDTXMania.DTX.listBMPTEX.Count + CDTXMania.DTX.listAVI.Count), span.ToString());

                        span = (TimeSpan)(DateTime.Now - timeBeginLoad);
                        Trace.TraceInformation("総読込時間:                {0}", span.ToString());
                        CDTXMania.Timer.tUpdate();
                        base.ePhaseID = CStage.EPhase.NOWLOADING_WAIT_BGM_SOUND_COMPLETION;
                        return (int)ESongLoadingScreenReturnValue.Continue;
                    }

                case CStage.EPhase.NOWLOADING_WAIT_BGM_SOUND_COMPLETION:
                    {
                        long nCurrentTime = CDTXMania.Timer.nCurrentTime;
                        if (nCurrentTime < this.nBGMPlayStartTime)
                            this.nBGMPlayStartTime = nCurrentTime;

                        //						if ( ( nCurrentTime - this.nBGMPlayStartTime ) > ( this.nBGMTotalPlayTimeMs - 1000 ) )
                        if ((nCurrentTime - this.nBGMPlayStartTime) > (this.nBGMTotalPlayTimeMs))	// #27787 2012.3.10 yyagi 1000ms == フェードイン分の時間
                        {
                            this.actFO.tStartFadeOut();
                            base.ePhaseID = CStage.EPhase.Common_FadeOut;
                        }
                        return (int)ESongLoadingScreenReturnValue.Continue;
                    }

                case CStage.EPhase.Common_FadeOut:
                    //if (this.actFO.OnUpdateAndDraw() == 0)
                    //return 0;
                    if (this.sdLoadingSound != null)
                    {
                        this.sdLoadingSound.tRelease();
                    }
                    return (int)ESongLoadingScreenReturnValue.LoadingComplete;
            }
            return (int)ESongLoadingScreenReturnValue.Continue;
        }


        /// <summary>
        /// ESC押下時、trueを返す
        /// </summary>
        /// <returns></returns>
        protected bool tHandleKeyInput()
        {
            IInputDevice keyboard = CDTXMania.InputManager.Keyboard;
            if ( keyboard.bKeyPressed( (int)SlimDXKey.Escape ) )		// escape (exit)
            {
                if ( CDTXMania.ConfigIni.bGuitarRevolutionMode )
                {
                    if (CDTXMania.stagePerfGuitarScreen.bActivated == true)
                        CDTXMania.stagePerfGuitarScreen.OnDeactivate();
                }
                else
                {
                    if (CDTXMania.stagePerfDrumsScreen.bActivated == true)
                        CDTXMania.stagePerfDrumsScreen.OnDeactivate();
                }

                return true;
            }
            return false;
        }

        // Other

        #region [ private ]
        //-----------------
        [StructLayout(LayoutKind.Sequential)]
        private struct STCharacterPosition
        {
            public char ch;
            public Point pt;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct ST泡
        {
            public bool b使用中;
            public CCounter ct進行;
            public int n前回のValue;
            public float fX;
            public float fY;
            public float f加速度X;
            public float f加速度Y;
            public float f加速度の加速度X;
            public float f加速度の加速度Y;
            public float f半径;
        }
        //		private CActFIFOBlack actFI;
        private CActFIFOBlackStart actFO;

        private readonly STCharacterPosition[] st小文字位置;
        private readonly STCharacterPosition[] st大文字位置;
        private int nCurrentInst;
        private long nBGMTotalPlayTimeMs;
        private long nBGMPlayStartTime;
        private CSound sdLoadingSound;
        private string strSTAGEFILE;
        private string strSongTitle;
        private string strArtistName;
        private CTexture txTitle;
        private CTexture txArtist;
        private CTexture txJacket;
        private CTexture txBackground;
        private CTexture txDifficultyPanel;
        private CTexture txPartPanel;

        private CPrivateFastFont pfタイトル;
        private CPrivateFastFont pfアーティスト;

        //2014.04.05.kairera0467 GITADORAグラデーションの色。
        //本当は共通のクラスに設置してそれを参照する形にしたかったが、なかなかいいメソッドが無いため、とりあえず個別に設置。
        //private Color clGITADORAgradationTopColor = Color.FromArgb(0, 220, 200);
        //private Color clGITADORAgradationBottomColor = Color.FromArgb(255, 250, 40);
        private Color clGITADORAgradationTopColor = Color.FromArgb(255, 255, 255);
        private Color clGITADORAgradationBottomColor = Color.FromArgb(255, 255, 255);

        private DateTime timeBeginLoad;
        private DateTime timeBeginLoadWAV;
        private int nWAVcount;
        private CTexture txLevel;

        [StructLayout(LayoutKind.Sequential)]
        public struct STATUSPANEL
        {
            public string label;
            public int status;
        }
        public int nIndex;
        public STATUSPANEL[] stPanelMap;

        private STDGBVALUE<List<STGhostLag>> stGhostLag;

        [StructLayout(LayoutKind.Sequential)]
        private struct STGhostLag
        {
            public int index;
            public int nJudgeTime;
            public int nLagTime;
            public STGhostLag( int index, int nJudgeTime, int nLagTime )
            {
                this.index = index;
                this.nJudgeTime = nJudgeTime;
                this.nLagTime = nLagTime;
            }
        }
        protected EJudgement e指定時刻からChipのJUDGEを返す( long nTime, int nInputAdjustTime, EInstrumentPart part )
        {
            int nDeltaTimeMs = Math.Abs((int)nTime + nInputAdjustTime);
            switch (part)
            {
                case EInstrumentPart.DRUMS:
                    // TODO: ghosts do not track columns, so pedal ranges cannot be used
                    return CDTXMania.stDrumHitRanges.tGetJudgement(nDeltaTimeMs);
                case EInstrumentPart.GUITAR:
                    return CDTXMania.stGuitarHitRanges.tGetJudgement(nDeltaTimeMs);
                case EInstrumentPart.BASS:
                    return CDTXMania.stBassHitRanges.tGetJudgement(nDeltaTimeMs);
                case EInstrumentPart.UNKNOWN:
                default:
                    return STHitRanges.tCreateDefaultDTXHitRanges().tGetJudgement(nDeltaTimeMs);
            }
        }
        //-----------------
        private void ReadGhost( string filename, List<int> list ) // #35411 2015.08.19 chnmr0 add
        {
            //return; //2015.12.31 kairera0467 以下封印

            if( File.Exists( filename ) )
            {
                using( FileStream fs = new FileStream( filename, FileMode.Open, FileAccess.Read ) )
                {
                    using( BinaryReader br = new BinaryReader( fs ) )
                    {
                        try
                        {
                            int cnt = br.ReadInt32();
                            for( int i = 0; i < cnt; ++i )
                            {
                                short lag = br.ReadInt16();
                                list.Add( lag );
                            }
                        }
                        catch( EndOfStreamException )
                        {
                            Trace.TraceInformation("ゴーストデータは正しく読み込まれませんでした。");
                            list.Clear();
                        }
                    }
                }
            }

            if( File.Exists( filename + ".score" ) )
            {
                using( FileStream fs = new FileStream( filename + ".score", FileMode.Open, FileAccess.Read ) )
                {
                    using( StreamReader sr = new StreamReader( fs ) )
                    {
                        try
                        {
                            string strScoreDataFile = sr.ReadToEnd();

                            strScoreDataFile = strScoreDataFile.Replace( Environment.NewLine, "\n" );
                            string[] delimiter = { "\n" };
                            string[] strSingleLine = strScoreDataFile.Split( delimiter, StringSplitOptions.RemoveEmptyEntries );

                            for( int i = 0; i < strSingleLine.Length; i++ )
                            {
                                string[] strA = strSingleLine[ i ].Split( '=' );
                                if (strA.Length != 2)
                                    continue;

                                switch( strA[ 0 ] )
                                {
                                    case "Score":
                                        CDTXMania.listTargetGhostScoreData[ (int)this.nCurrentInst ].nスコア = Convert.ToInt32( strA[ 1 ] );
                                        continue;
                                    case "PlaySkill":
                                        CDTXMania.listTargetGhostScoreData[ (int)this.nCurrentInst ].dbPerformanceSkill = Convert.ToDouble( strA[ 1 ] );
                                        continue;
                                    case "Skill":
                                        CDTXMania.listTargetGhostScoreData[ (int)this.nCurrentInst ].dbGameSkill = Convert.ToDouble( strA[ 1 ] );
                                        continue;
                                    case "Perfect":
                                        CDTXMania.listTargetGhostScoreData[ (int)this.nCurrentInst ].nPerfectCount_ExclAuto = Convert.ToInt32( strA[ 1 ] );
                                        continue;
                                    case "Great":
                                        CDTXMania.listTargetGhostScoreData[ (int)this.nCurrentInst ].nGreatCount_ExclAuto = Convert.ToInt32( strA[ 1 ] );
                                        continue;
                                    case "Good":
                                        CDTXMania.listTargetGhostScoreData[ (int)this.nCurrentInst ].nGoodCount_ExclAuto = Convert.ToInt32( strA[ 1 ] );
                                        continue;
                                    case "Poor":
                                        CDTXMania.listTargetGhostScoreData[ (int)this.nCurrentInst ].nPoorCount_ExclAuto = Convert.ToInt32( strA[ 1 ] );
                                        continue;
                                    case "Miss":
                                        CDTXMania.listTargetGhostScoreData[ (int)this.nCurrentInst ].nMissCount_ExclAuto = Convert.ToInt32( strA[ 1 ] );
                                        continue;
                                    case "MaxCombo":
                                        CDTXMania.listTargetGhostScoreData[ (int)this.nCurrentInst ].nMaxCombo = Convert.ToInt32( strA[ 1 ] );
                                        continue;
                                    default:
                                        continue;
                                }
                            }
                        }
                        catch( NullReferenceException )
                        {
                            Trace.TraceInformation("ゴーストデータの記録が正しく読み込まれませんでした。");
                        }
                        catch( EndOfStreamException )
                        {
                            Trace.TraceInformation("ゴーストデータの記録が正しく読み込まれませんでした。");
                        }
                    }
                }
            }
            else
            {
                CDTXMania.listTargetGhostScoreData[ (int)this.nCurrentInst ] = null;
            }
        }
        private void tDrawStringSmall(int x, int y, string str)
        {
            this.tDrawStringSmall(x, y, str, false);
        }
        private void tDrawStringSmall(int x, int y, string str, bool b強調)
        {
            foreach (char ch in str)
            {
                for (int i = 0; i < this.st小文字位置.Length; i++)
                {
                    if (this.st小文字位置[i].ch == ch)
                    {
                        Rectangle rectangle = new Rectangle(this.st小文字位置[i].pt.X, this.st小文字位置[i].pt.Y, 13, 22);
                        if (this.txLevel != null)
                        {
                            this.txLevel.tDraw2D(CDTXMania.app.Device, x, y, rectangle);
                        }
                        break;
                    }
                }
                x += 12;
            }
        }
        private void tDrawStringLarge(int x, int y, string str)
        {
            this.tDrawStringLarge(x, y, str, false);
        }
        private void tDrawStringLarge(int x, int y, string str, bool bExtraLarge)
        {
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                for (int j = 0; j < this.st大文字位置.Length; j++)
                {
                    if (this.st大文字位置[j].ch == c)
                    {
                        int num;
                        int num2;
                        num = 0;
                        num2 = 0;
                        Rectangle rc画像内の描画領域 = new Rectangle(this.st大文字位置[j].pt.X, this.st大文字位置[j].pt.Y, 100, 130);
                        if (this.txLevel != null)
                        {
                            this.txLevel.tDraw2D(CDTXMania.app.Device, x, y, rc画像内の描画領域);
                        }
                        break;
                    }
                }
                if (c == '.')
                {
                    x += 30;
                }
                else
                {
                    x += 90;
                }
            }
        }
        private void tDrawDifficultyPanel( string strラベル名, int nX, int nY )
        {
            string strRawScriptFile;

            Rectangle rect = new Rectangle( 0, 0, 262, 50 );

            //ファイルの存在チェック
            if( File.Exists( CSkin.Path( @"Script\difficult.dtxs" ) ) )
            {
                //スクリプトを開く
                StreamReader reader = new StreamReader( CSkin.Path( @"Script\difficult.dtxs" ), Encoding.GetEncoding( "Shift_JIS" ) );
                strRawScriptFile = reader.ReadToEnd();

                strRawScriptFile = strRawScriptFile.Replace( Environment.NewLine, "\n" );
                string[] delimiter = { "\n" };
                string[] strSingleLine = strRawScriptFile.Split( delimiter, StringSplitOptions.RemoveEmptyEntries );

                for( int i = 0; i < strSingleLine.Length; i++ )
                {
                    if( strSingleLine[ i ].StartsWith( "//" ) )
                        continue; //コメント行の場合は無視

                    //まずSplit
                    string[] arScriptLine = strSingleLine[ i ].Split( ',' );

                    if( ( arScriptLine.Length >= 4 && arScriptLine.Length <= 5 ) == false )
                        continue; //引数が4つか5つじゃなければ無視。

                    if( arScriptLine[ 0 ] != "6" )
                        continue; //使用するシーンが違うなら無視。

                    if( arScriptLine.Length == 4 )
                    {
                        if( String.Compare( arScriptLine[ 1 ], strラベル名, true ) != 0 )
                            continue; //ラベル名が違うなら無視。大文字小文字区別しない
                    }
                    else if( arScriptLine.Length == 5 )
                    {
                        if( arScriptLine[ 4 ] == "1" )
                        {
                            if( arScriptLine[ 1 ] != strラベル名 )
                                continue; //ラベル名が違うなら無視。
                        }
                        else
                        {
                            if( String.Compare( arScriptLine[ 1 ], strラベル名, true ) != 0 )
                                continue; //ラベル名が違うなら無視。大文字小文字区別しない
                        }
                    }
                    rect.X = Convert.ToInt32( arScriptLine[ 2 ] );
                    rect.Y = Convert.ToInt32( arScriptLine[ 3 ] );

                    break;
                }
            }

            if( this.txDifficultyPanel != null )
                this.txDifficultyPanel.tDraw2D( CDTXMania.app.Device, nX, nY, rect );
        }
        #endregion
    }
}
