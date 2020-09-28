﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Runtime;
using System.Runtime.Serialization.Formatters.Binary;
using SharpDX;
using SharpDX.Direct3D9;
using FDK;
using SampleFramework;
using DTXMania.Properties;
using System.Reflection;
using DirectShowLib;

using Point = System.Drawing.Point;

namespace DTXMania
{
    internal class CDTXMania : Game
    {
        // プロパティ

        public static readonly string VERSION = "Ver3.70 GD (NX 1.00 200911)";
        public static readonly string D3DXDLL = "d3dx9_43.dll";		// June 2010
        //public static readonly string D3DXDLL = "d3dx9_42.dll";	// February 2010
        //public static readonly string D3DXDLL = "d3dx9_41.dll";	// March 2009

        public static CDTXMania app
        {
            get;
            private set;
        }
        public static Folder Folder
        {
            get;
            protected set;
        }
        public static CCharacterConsole actDisplayString  // act文字コンソール
        {
            get;
            private set;
        }
        public static bool bCompactMode
        {
            get;
            private set;
        }
        public static CConfigIni ConfigIni
        {
            get;
            private set;
        }
        public static CDTX DTX
        {
            get
            {
                return dtx;
            }
            set
            {
                if ((dtx != null) && (app != null))
                {
                    dtx.OnDeactivate();
                    app.listTopLevelActivities.Remove(dtx);
                }
                dtx = value;
                if ((dtx != null) && (app != null))
                {
                    app.listTopLevelActivities.Add(dtx);
                }
            }
        }
        public static CFPS FPS
        {
            get;
            private set;
        }
        public static CInputManager InputManager
        {
            get;
            private set;
        }
        public static int nSongDifficulty
        {
            get;
            set;
        }

        public static string strSongDifficulyName
        {
            get;
            set;
        }
        public static int nPerfectRangeMs
        {
            get
            {
                if (stageSongSelection.rConfirmedSong != null)
                {
                    CSongListNode c曲リストノード = stageSongSelection.rConfirmedSong.r親ノード;
                    if (((c曲リストノード != null) && (c曲リストノード.eNodeType == CSongListNode.ENodeType.BOX)) && (c曲リストノード.nPerfect範囲ms >= 0))
                    {
                        return c曲リストノード.nPerfect範囲ms;
                    }
                }
                return ConfigIni.nヒット範囲ms.Perfect;
            }
        }
        public static int nGreatRangeMs
        {
            get
            {
                if (stageSongSelection.rConfirmedSong != null)
                {
                    CSongListNode c曲リストノード = stageSongSelection.rConfirmedSong.r親ノード;
                    if (((c曲リストノード != null) && (c曲リストノード.eNodeType == CSongListNode.ENodeType.BOX)) && (c曲リストノード.nGreat範囲ms >= 0))
                    {
                        return c曲リストノード.nGreat範囲ms;
                    }
                }
                return ConfigIni.nヒット範囲ms.Great;
            }
        }
        public static int nGoodRangeMs
        {
            get
            {
                if (stageSongSelection.rConfirmedSong != null)
                {
                    CSongListNode c曲リストノード = stageSongSelection.rConfirmedSong.r親ノード;
                    if (((c曲リストノード != null) && (c曲リストノード.eNodeType == CSongListNode.ENodeType.BOX)) && (c曲リストノード.nGood範囲ms >= 0))
                    {
                        return c曲リストノード.nGood範囲ms;
                    }
                }
                return ConfigIni.nヒット範囲ms.Good;
            }
        }
        public static int nPoorRangeMs
        {
            get
            {
                if (stageSongSelection.rConfirmedSong != null)
                {
                    CSongListNode c曲リストノード = stageSongSelection.rConfirmedSong.r親ノード;
                    if (((c曲リストノード != null) && (c曲リストノード.eNodeType == CSongListNode.ENodeType.BOX)) && (c曲リストノード.nPoor範囲ms >= 0))
                    {
                        return c曲リストノード.nPoor範囲ms;
                    }
                }
                return ConfigIni.nヒット範囲ms.Poor;
            }
        }
        public static CPad Pad
        {
            get;
            private set;
        }
        public static Random Random
        {
            get;
            private set;
        }
        public static CSkin Skin
        {
            get;
            private set;
        }
        public static CSongManager SongManager
        {
            get;
            set;	// 2012.1.26 yyagi private解除 CStage起動でのdesirialize読み込みのため
        }
        public static CEnumSongs EnumSongs
        {
            get;
            private set;
        }
        public static CActEnumSongs actEnumSongs
        {
            get;
            private set;
        }
        public static CActFlushGPU actFlushGPU
        {
            get;
            private set;
        }
        public static CSoundManager SoundManager
        {
            get;
            private set;
        }
        public static CStageStartup stageStartup
        {
            get;
            private set;
        }
        public static CStageTitle stageTitle
        {
            get;
            private set;
        }
        public static CStageOption stageOption
        {
            get;
            private set;
        }
        public static CStageConfig stageConfig
        {
            get;
            private set;
        }
        public static CStageSongSelection stageSongSelection
        {
            get;
            private set;
        }
        public static CStageSongLoading stageSongLoading
        {
            get;
            private set;
        }
        public static CStagePerfGuitarScreen stagePerfGuitarScreen
        {
            get;
            private set;
        }
        public static CStagePerfDrumsScreen stagePerfDrumsScreen
        {
            get;
            private set;
        }
        public static CStagePerfCommonScreen stagePlayingScreenCommon
        {
            get;
            private set;
        }
        public static CStageResult stageResult
        {
            get;
            private set;
        }
        public static CStageChangeSkin stageChangeSkin
        {
            get;
            private set;
        }
        public static CStageEnd stageEnd
        {
            get;
            private set;
        }
        public static CStage rCurrentStage = null;
        public static CStage rPreviousStage = null;
        public bool b汎用ムービーである = false;
        public static string strEXEのあるフォルダ
        {
            get;
            private set;
        }
        public static string strCompactModeFile
        {
            get;
            private set;
        }
        public static CTimer Timer
        {
            get;
            private set;
        }
        public static Format TextureFormat = Format.A8R8G8B8;
        internal static IPluginActivity actPluginOccupyingInput = null;  // act現在入力を占有中のプラグイン
        public bool bApplicationActive
        {
            get;
            private set;
        }
        public bool b次のタイミングで垂直帰線同期切り替えを行う
        {
            get;
            set;
        }
        public bool b次のタイミングで全画面_ウィンドウ切り替えを行う
        {
            get;
            set;
        }

        public Device Device
        {
            get { return base.GraphicsDeviceManager.Direct3D9.Device; }
        }
        public CPluginHost PluginHost
        {
            get;
            private set;
        }
        public List<STPlugin> listPlugins = new List<STPlugin>();
        public struct STPlugin
        {
            public IPluginActivity plugin;
            public string strプラグインフォルダ;
            public string strアセンブリ簡易名;
            public Version Version;
        }
        private static Size currentClientSize		// #23510 2010.10.27 add yyagi to keep current window size
        {
            get;
            set;
        }
        //		public static CTimer ct;
        public IntPtr WindowHandle					// 2012.10.24 yyagi; to add ASIO support
        {
            get { return base.Window.Handle; }
        }
        //fork
        public static STDGBVALUE< List<int> > listAutoGhostLag = new STDGBVALUE<List<int>>();
        public static STDGBVALUE< List<int> > listTargetGhsotLag = new STDGBVALUE<List<int>>();
        public static STDGBVALUE< CScoreIni.CPerformanceEntry > listTargetGhostScoreData = new STDGBVALUE< CScoreIni.CPerformanceEntry >();

        // Constructor

        public CDTXMania()
        {
            CDTXMania.app = this;
            this.tStartProcess();
        }


        // Methods

        public void tSwitchFullScreenMode()
        {
#if WindowedFullscreen
            if ( ConfigIni != null )
#else
            DeviceSettings settings = base.GraphicsDeviceManager.CurrentSettings.Clone();
            if ((ConfigIni != null) && (ConfigIni.bウィンドウモード != settings.Windowed))
#endif
            {
#if !WindowedFullscreen
                settings.Windowed = ConfigIni.bウィンドウモード;
#endif
                if (ConfigIni.bウィンドウモード == false)	// #23510 2010.10.27 yyagi: backup current window size before going fullscreen mode
                {
                    currentClientSize = this.Window.ClientSize;
                    ConfigIni.nウインドウwidth = this.Window.ClientSize.Width;
                    ConfigIni.nウインドウheight = this.Window.ClientSize.Height;
                    //                  FDK.CTaskBar.ShowTaskBar( false );
                }
#if !WindowedFullscreen
                base.GraphicsDeviceManager.ChangeDevice(settings);
#endif
                if (ConfigIni.bウィンドウモード == true)	// #23510 2010.10.27 yyagi: to resume window size from backuped value
                {
#if WindowedFullscreen
                    base.Window.ClientSize =
                        new Size(currentClientSize.Width, currentClientSize.Height);
                }
            }
				if ( ConfigIni.bウィンドウモード == true )	// #23510 2010.10.27 yyagi: to resume window size from backuped value
				{
															// #30666 2013.2.2 yyagi Don't use Fullscreen mode becasue NVIDIA GeForce is
															// tend to delay drawing on Fullscreen mode. So DTXMania uses Maximized window
															// in spite of using fullscreen mode.
					app.Window.WindowState = FormWindowState.Normal;
					app.Window.FormBorderStyle = FormBorderStyle.Sizable;
					app.Window.WindowState = FormWindowState.Normal;
#endif
                    base.Window.ClientSize =
                        new Size(currentClientSize.Width, currentClientSize.Height);
                    //                  FDK.CTaskBar.ShowTaskBar( true );
                }
#if WindowedFullscreen
				else 
				{
					app.Window.WindowState = FormWindowState.Normal;
					app.Window.FormBorderStyle = FormBorderStyle.None;
					app.Window.WindowState = FormWindowState.Maximized;
				}
            if ( ConfigIni.bウィンドウモード )
            {
                if ( !this.bマウスカーソル表示中 )
                {
                    Cursor.Show();
                    this.bマウスカーソル表示中 = true;
                }
            }
            else if ( this.bマウスカーソル表示中 )
            {
                Cursor.Hide();
                this.bマウスカーソル表示中 = false;
            }
#endif
            }

        }


        #region [ #24609 リザルト画像をpngで保存する ]		// #24609 2011.3.14 yyagi; to save result screen in case BestRank or HiSkill.
        /// <summary>
        /// リザルト画像のキャプチャと保存。
        /// </summary>
        /// <param name="strFilename">保存するファイル名(フルパス)</param>
        public bool SaveResultScreen(string strFullPath)
        {
            string strSavePath = Path.GetDirectoryName(strFullPath);
            if (!Directory.Exists(strSavePath))
            {
                try
                {
                    Directory.CreateDirectory(strSavePath);
                }
                catch
                {
                    return false;
                }
            }

            // http://www.gamedev.net/topic/594369-dx9slimdxati-incorrect-saving-surface-to-file/
            using (Surface pSurface = CDTXMania.app.Device.GetRenderTarget(0))
            {
                Surface.ToFile(pSurface, strFullPath, ImageFileFormat.Png);
            }
            return true;
        }
        #endregion

        // Game 実装
        protected override void Initialize()
        {
            //			new GCBeep();
            if (this.listTopLevelActivities != null)
            {
                foreach (CActivity activity in this.listTopLevelActivities)
                {
                    activity.OnManagedCreateResources();
                }
            }

            foreach (STPlugin st in this.listPlugins)
            {
                Directory.SetCurrentDirectory(st.strプラグインフォルダ);
                st.plugin.OnManagedリソースの作成();
                Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
            }
#if GPUFlushAfterPresent
        FrameEnd += dtxmania_FrameEnd;
#endif
        }
#if GPUFlushAfterPresent
        void dtxmania_FrameEnd( object sender, EventArgs e ) // GraphicsDeviceManager.game_FrameEnd()後に実行される
        {	                                                                     // → Present()直後にGPUをFlushする
                                                                                 // → 画面のカクツキが頻発したため、ここでのFlushは行わない
            actFlushGPU.On進行描画(); // Flush GPU
        }
#endif
        protected override void LoadContent()
        {
            if (ConfigIni.bウィンドウモード)
            {
                if (!this.bマウスカーソル表示中)
                {
                    Cursor.Show();
                    this.bマウスカーソル表示中 = true;
                }
            }
            else if (this.bマウスカーソル表示中)
            {
                Cursor.Hide();
                this.bマウスカーソル表示中 = false;
            }
            this.Device.SetTransform(TransformState.View, Matrix.LookAtLH(new Vector3(0f, 0f, (float)(-SampleFramework.GameWindowSize.Height / 2 * Math.Sqrt(3.0))), new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 0f)));
            this.Device.SetTransform(TransformState.Projection, Matrix.PerspectiveFovLH(CConversion.DegreeToRadian((float)60f), ((float)this.Device.Viewport.Width) / ((float)this.Device.Viewport.Height), -100f, 100f));
            this.Device.SetRenderState(RenderState.Lighting, false);
            this.Device.SetRenderState(RenderState.ZEnable, false);
            this.Device.SetRenderState(RenderState.AntialiasedLineEnable, false);
            this.Device.SetRenderState(RenderState.AlphaTestEnable, true);
            this.Device.SetRenderState(RenderState.AlphaRef, 10);

            this.Device.SetRenderState(RenderState.MultisampleAntialias, true);
            this.Device.SetSamplerState(0, SamplerState.MinFilter, TextureFilter.Linear);
            this.Device.SetSamplerState(0, SamplerState.MagFilter, TextureFilter.Linear);

            this.Device.SetRenderState<Compare>(RenderState.AlphaFunc, Compare.Greater);
            this.Device.SetRenderState(RenderState.AlphaBlendEnable, true);
            this.Device.SetRenderState<Blend>(RenderState.SourceBlend, Blend.SourceAlpha);
            this.Device.SetRenderState<Blend>(RenderState.DestinationBlend, Blend.InverseSourceAlpha);
            this.Device.SetTextureStageState(0, TextureStage.AlphaOperation, TextureOperation.Modulate);
            this.Device.SetTextureStageState(0, TextureStage.AlphaArg1, 2);
            this.Device.SetTextureStageState(0, TextureStage.AlphaArg2, 1);

            if (this.listTopLevelActivities != null)
            {
                foreach (CActivity activity in this.listTopLevelActivities)
                    activity.OnUnmanagedCreateResources();
            }

            foreach (STPlugin st in this.listPlugins)
            {
                Directory.SetCurrentDirectory(st.strプラグインフォルダ);
                st.plugin.OnUnmanagedリソースの作成();
                Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
            }
        }
        protected override void UnloadContent()
        {
            if (this.listTopLevelActivities != null)
            {
                foreach (CActivity activity in this.listTopLevelActivities)
                    activity.OnUnmanagedReleaseResources();
            }

            foreach (STPlugin st in this.listPlugins)
            {
                Directory.SetCurrentDirectory(st.strプラグインフォルダ);
                st.plugin.OnUnmanagedリソースの解放();
                Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
            }
        }
        protected override void OnExiting(EventArgs e)
        {
            CPowerManagement.tEnableMonitorSuspend();		// スリープ抑止状態を解除
            this.tTerminate();
            base.OnExiting(e);
        }
        protected override void Update(GameTime gameTime)
        {
        }
        protected override void Draw(GameTime gameTime)
        {
            //Do not draw until SoundManager is initialized
            //Fixed issue where exception is raised upon loading when Japanese IME is enabled
            if(SoundManager == null)
            {
                return;
            }

            SoundManager.t再生中の処理をする();

            if (Timer != null)
                Timer.tUpdate();
            if (CSoundManager.rcPerformanceTimer != null)
                CSoundManager.rcPerformanceTimer.tUpdate();

            if (InputManager != null)
                InputManager.tPolling(this.bApplicationActive, CDTXMania.ConfigIni.bバッファ入力を行う);

            if (FPS != null)
                FPS.tカウンタ更新();

            //if( Pad != null )					ポーリング時にクリアしたらダメ！曲の開始時に1回だけクリアする。(2010.9.11)
            //	Pad.stDetectedDevice.Clear();

            if (this.Device == null)
                return;

            if (this.bApplicationActive)	// DTXMania本体起動中の本体/モニタの省電力モード移行を抑止
                CPowerManagement.tDisableMonitorSuspend();

            // #xxxxx 2013.4.8 yyagi; sleepの挿入位置を、EndScnene～Present間から、BeginScene前に移動。描画遅延を小さくするため。
            #region [ スリープ ]
            if (ConfigIni.nフレーム毎スリープms >= 0)			// #xxxxx 2011.11.27 yyagi
            {
                Thread.Sleep(ConfigIni.nフレーム毎スリープms);
            }
            #endregion

            this.Device.BeginScene();
            this.Device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, SharpDX.Color.Black, 1f, 0);

            if (rCurrentStage != null)
            {
                this.nUpdateAndDrawReturnValue = (rCurrentStage != null) ? rCurrentStage.OnUpdateAndDraw() : 0;

                #region [ プラグインの進行描画 ]
                //---------------------
                foreach (STPlugin sp in this.listPlugins)
                {
                    Directory.SetCurrentDirectory(sp.strプラグインフォルダ);

                    if (CDTXMania.actPluginOccupyingInput == null || CDTXMania.actPluginOccupyingInput == sp.plugin)
                        sp.plugin.On進行描画(CDTXMania.Pad, CDTXMania.InputManager.Keyboard);
                    else
                        sp.plugin.On進行描画(null, null);

                    Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                }
                //---------------------
                #endregion


                CScoreIni scoreIni = null;

                //if (Control.IsKeyLocked(Keys.CapsLock)) // #30925 2013.3.11 yyagi; capslock=ON時は、EnumSongsしないようにして、起動負荷とASIOの音切れの関係を確認する
                //{                                       // → songs.db等の書き込み時だと音切れするっぽい
                //    CDTXMania.stageSongSelection.bIsEnumeratingSongs = false;
                //    actEnumSongs.OnDeactivate();
                //    EnumSongs.SongListEnumCompletelyDone();
                //}
                #region [ 曲検索スレッドの起動/終了 ]					// ここに"Enumerating Songs..."表示を集約
                if (!CDTXMania.bCompactMode)
                {
                    actEnumSongs.OnUpdateAndDraw();							// "Enumerating Songs..."アイコンの描画
                }							// "Enumerating Songs..."アイコンの描画
                switch (rCurrentStage.eStageID)
                {
                    case CStage.EStage.Title:
                    case CStage.EStage.Config:
                    case CStage.EStage.Option:
                    case CStage.EStage.SongSelection:
                    case CStage.EStage.SongLoading:
                        if (EnumSongs != null)
                        {
                            #region [ (特定条件時) 曲検索スレッドの起動_開始 ]
                            if (rCurrentStage.eStageID == CStage.EStage.Title &&
                                 rPreviousStage.eStageID == CStage.EStage.Startup &&
                                 this.nUpdateAndDrawReturnValue == (int)CStageTitle.E戻り値.継続 &&
                                 !EnumSongs.IsSongListEnumStarted)
                            {
                                actEnumSongs.OnActivate();
                                CDTXMania.stageSongSelection.bIsEnumeratingSongs = true;
                                EnumSongs.Init(CDTXMania.SongManager.listSongsDB, CDTXMania.SongManager.nNbScoresFromSongsDB);	// songs.db情報と、取得した曲数を、新インスタンスにも与える
                                EnumSongs.StartEnumFromDisk();		// 曲検索スレッドの起動_開始
                                if (CDTXMania.SongManager.nNbScoresFromSongsDB == 0)	// もし初回起動なら、検索スレッドのプライオリティをLowestでなくNormalにする
                                {
                                    EnumSongs.ChangeEnumeratePriority(ThreadPriority.Normal);
                                }
                            }
                            #endregion

                            #region [ 曲検索の中断と再開 ]
                            if (rCurrentStage.eStageID == CStage.EStage.SongSelection && !EnumSongs.IsSongListEnumCompletelyDone)
                            {
                                switch (this.nUpdateAndDrawReturnValue)
                                {
                                    case 0:		// 何もない
                                        //if ( CDTXMania.stageSongSelection.bIsEnumeratingSongs )
                                        if (!CDTXMania.stageSongSelection.bIsPlayingPremovie)
                                        {
                                            EnumSongs.Resume();						// #27060 2012.2.6 yyagi 中止していたバックグランド曲検索を再開
                                            EnumSongs.IsSlowdown = false;
                                        }
                                        else
                                        {
                                            // EnumSongs.Suspend();					// #27060 2012.3.2 yyagi #PREMOVIE再生中は曲検索を低速化
                                            EnumSongs.IsSlowdown = true;
                                        }
                                        actEnumSongs.OnActivate();
                                        break;

                                    case 2:		// 曲決定
                                        EnumSongs.Suspend();						// #27060 バックグラウンドの曲検索を一時停止
                                        actEnumSongs.OnDeactivate();
                                        break;
                                }
                            }
                            #endregion

                            #region [ 曲探索中断待ち待機 ]
                            if (rCurrentStage.eStageID == CStage.EStage.SongLoading && !EnumSongs.IsSongListEnumCompletelyDone &&
                                EnumSongs.thDTXFileEnumerate != null)							// #28700 2012.6.12 yyagi; at Compact mode, enumerating thread does not exist.
                            {
                                EnumSongs.WaitUntilSuspended();									// 念のため、曲検索が一時中断されるまで待機
                            }
                            #endregion

                            #region [ 曲検索が完了したら、実際の曲リストに反映する ]
                            // CStageSongSelection.OnActivate() に回した方がいいかな？
                            if (EnumSongs.IsSongListEnumerated)
                            {
                                actEnumSongs.OnDeactivate();
                                CDTXMania.stageSongSelection.bIsEnumeratingSongs = false;

                                bool bRemakeSongTitleBar = (rCurrentStage.eStageID == CStage.EStage.SongSelection) ? true : false;
                                CDTXMania.stageSongSelection.Refresh(EnumSongs.Songs管理, bRemakeSongTitleBar);
                                EnumSongs.SongListEnumCompletelyDone();
                            }
                            #endregion
                        }
                        break;
                }
                #endregion

                switch (rCurrentStage.eStageID)
                {
                    case CStage.EStage.DoNothing:
                        break;

                    case CStage.EStage.Startup:
                        #region [ *** ]
                        //-----------------------------
                        if (this.nUpdateAndDrawReturnValue != 0)
                        {
                            if (!bCompactMode)
                            {
                                rCurrentStage.OnDeactivate();
                                Trace.TraceInformation("----------------------");
                                Trace.TraceInformation("■ Title");
                                stageTitle.OnActivate();
                                rPreviousStage = rCurrentStage;
                                rCurrentStage = stageTitle;
                            }
                            else
                            {
                                rCurrentStage.OnDeactivate();
                                Trace.TraceInformation("----------------------");
                                Trace.TraceInformation("■ SongLoading");
                                stageSongLoading.OnActivate();
                                rPreviousStage = rCurrentStage;
                                rCurrentStage = stageSongLoading;

                            }
                            foreach (STPlugin pg in this.listPlugins)
                            {
                                Directory.SetCurrentDirectory(pg.strプラグインフォルダ);
                                pg.plugin.OnChangeStage();
                                Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                            }

                            this.tRunGarbageCollector();
                        }
                        //-----------------------------
                        #endregion
                        break;

                    case CStage.EStage.Title:
                        #region [ *** ]
                        //-----------------------------
                        if( this.nUpdateAndDrawReturnValue != 0 )
                        {
                            switch (this.nUpdateAndDrawReturnValue)
                            {
                                case (int)CStageTitle.E戻り値.GAMESTART:
                                    #region [ 選曲処理へ ]
                                    //-----------------------------
                                    rCurrentStage.OnDeactivate();
                                    Trace.TraceInformation("----------------------");
                                    Trace.TraceInformation("■ SongSelection");
                                    stageSongSelection.OnActivate();
                                    rPreviousStage = rCurrentStage;
                                    rCurrentStage = stageSongSelection;
                                    //-----------------------------
                                    #endregion
                                    break;

                                #region [ OPTION: 廃止済 ]
                                /*
							    case 2:									// #24525 OPTIONとCONFIGの統合に伴い、OPTIONは廃止
								    #region [ *** ]
								    //-----------------------------
								    rCurrentStage.OnDeactivate();
								    Trace.TraceInformation( "----------------------" );
								    Trace.TraceInformation( "■ Option" );
								    stageOption.OnActivate();
								    rPreviousStage = rCurrentStage;
								    rCurrentStage = stageOption;
								    //-----------------------------
								    #endregion
								    break;
                                    */
                                #endregion

                                case (int)CStageTitle.E戻り値.CONFIG:
                                    #region [ *** ]
                                    //-----------------------------
                                    rCurrentStage.OnDeactivate();
                                    Trace.TraceInformation("----------------------");
                                    Trace.TraceInformation("■ Config");
                                    stageConfig.OnActivate();
                                    rPreviousStage = rCurrentStage;
                                    rCurrentStage = stageConfig;
                                    //-----------------------------
                                    #endregion
                                    break;

                                case (int)CStageTitle.E戻り値.EXIT:
                                    #region [ *** ]
                                    //-----------------------------
                                    rCurrentStage.OnDeactivate();
                                    Trace.TraceInformation("----------------------");
                                    Trace.TraceInformation("■ End");
                                    stageEnd.OnActivate();
                                    rPreviousStage = rCurrentStage;
                                    rCurrentStage = stageEnd;
                                    //-----------------------------
                                    #endregion
                                    break;
                            }

                            foreach (STPlugin pg in this.listPlugins)
                            {
                                Directory.SetCurrentDirectory(pg.strプラグインフォルダ);
                                pg.plugin.OnChangeStage();
                                Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                            }

                            this.tRunGarbageCollector();       // #31980 2013.9.3 yyagi タイトル画面でだけ、毎フレームGCを実行して重くなっていた問題の修正
                        }

                        //-----------------------------
                        #endregion
                        break;


                    case CStage.EStage.Option:
                        #region [ *** ]
                        //-----------------------------
                        if (this.nUpdateAndDrawReturnValue != 0)
                        {
                            switch (rPreviousStage.eStageID)
                            {
                                case CStage.EStage.Title:
                                    #region [ *** ]
                                    //-----------------------------
                                    rCurrentStage.OnDeactivate();
                                    Trace.TraceInformation("----------------------");
                                    Trace.TraceInformation("■ Title");
                                    stageTitle.OnActivate();
                                    rPreviousStage = rCurrentStage;
                                    rCurrentStage = stageTitle;

                                    foreach (STPlugin pg in this.listPlugins)
                                    {
                                        Directory.SetCurrentDirectory(pg.strプラグインフォルダ);
                                        pg.plugin.OnChangeStage();
                                        Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                                    }

                                    this.tRunGarbageCollector();
                                    break;
                                //-----------------------------
                                    #endregion

                                case CStage.EStage.SongSelection:
                                    #region [ *** ]
                                    //-----------------------------
                                    rCurrentStage.OnDeactivate();
                                    Trace.TraceInformation("----------------------");
                                    Trace.TraceInformation("■ SongSelection");
                                    stageSongSelection.OnActivate();
                                    rPreviousStage = rCurrentStage;
                                    rCurrentStage = stageSongSelection;

                                    foreach (STPlugin pg in this.listPlugins)
                                    {
                                        Directory.SetCurrentDirectory(pg.strプラグインフォルダ);
                                        pg.plugin.OnChangeStage();
                                        Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                                    }

                                    this.tRunGarbageCollector();
                                    break;
                                //-----------------------------
                                    #endregion
                            }
                        }
                        //-----------------------------
                        #endregion
                        break;


                    case CStage.EStage.Config:
                        #region [ *** ]
                        //-----------------------------
                        if (this.nUpdateAndDrawReturnValue != 0)
                        {
                            switch (rPreviousStage.eStageID)
                            {
                                case CStage.EStage.Title:
                                    #region [ *** ]
                                    //-----------------------------
                                    rCurrentStage.OnDeactivate();
                                    Trace.TraceInformation("----------------------");
                                    Trace.TraceInformation("■ Title");
                                    stageTitle.OnActivate();
                                    rPreviousStage = rCurrentStage;
                                    rCurrentStage = stageTitle;

                                    foreach (STPlugin pg in this.listPlugins)
                                    {
                                        Directory.SetCurrentDirectory(pg.strプラグインフォルダ);
                                        pg.plugin.OnChangeStage();
                                        Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                                    }

                                    this.tRunGarbageCollector();
                                    break;
                                //-----------------------------
                                    #endregion

                                case CStage.EStage.SongSelection:
                                    #region [ *** ]
                                    //-----------------------------
                                    rCurrentStage.OnDeactivate();
                                    Trace.TraceInformation("----------------------");
                                    Trace.TraceInformation("■ SongSelection");
                                    stageSongSelection.OnActivate();
                                    rPreviousStage = rCurrentStage;
                                    rCurrentStage = stageSongSelection;

                                    foreach (STPlugin pg in this.listPlugins)
                                    {
                                        Directory.SetCurrentDirectory(pg.strプラグインフォルダ);
                                        pg.plugin.OnChangeStage();
                                        Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                                    }

                                    this.tRunGarbageCollector();
                                    break;
                                //-----------------------------
                                    #endregion
                            }
                        }
                        //-----------------------------
                        #endregion
                        break;

                    case CStage.EStage.SongSelection:
                        #region [ *** ]
                        //-----------------------------
                        switch (this.nUpdateAndDrawReturnValue)
                        {
                            case (int)CStageSongSelection.EReturnValue.ReturnToTitle:
                                #region [ *** ]
                                //-----------------------------
                                rCurrentStage.OnDeactivate();
                                Trace.TraceInformation("----------------------");
                                Trace.TraceInformation("■ Title");
                                stageTitle.OnActivate();
                                rPreviousStage = rCurrentStage;
                                rCurrentStage = stageTitle;

                                foreach (STPlugin pg in this.listPlugins)
                                {
                                    Directory.SetCurrentDirectory(pg.strプラグインフォルダ);
                                    pg.plugin.OnChangeStage();
                                    Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                                }

                                this.tRunGarbageCollector();
                                break;
                            //-----------------------------
                                #endregion

                            case (int)CStageSongSelection.EReturnValue.Selected:
                                #region [ *** ]
                                //-----------------------------
                                rCurrentStage.OnDeactivate();
                                Trace.TraceInformation("----------------------");
                                Trace.TraceInformation("■ SongLoading");
                                stageSongLoading.OnActivate();
                                rPreviousStage = rCurrentStage;
                                rCurrentStage = stageSongLoading;

                                foreach (STPlugin pg in this.listPlugins)
                                {
                                    Directory.SetCurrentDirectory(pg.strプラグインフォルダ);
                                    pg.plugin.OnChangeStage();
                                    Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                                }

                                this.tRunGarbageCollector();
                                break;
                            //-----------------------------
                                #endregion


                            case (int)CStageSongSelection.EReturnValue.CallOptions:
                                #region [ *** ]
                                //-----------------------------

                                rCurrentStage.OnDeactivate();
                                Trace.TraceInformation("----------------------");
                                Trace.TraceInformation("■ Option");
                                stageOption.OnActivate();
                                rPreviousStage = rCurrentStage;
                                rCurrentStage = stageOption;

                                foreach (STPlugin pg in this.listPlugins)
                                {
                                    Directory.SetCurrentDirectory(pg.strプラグインフォルダ);
                                    pg.plugin.OnChangeStage();
                                    Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                                }

                                this.tRunGarbageCollector();
                                break;
                            //-----------------------------
                                #endregion

                            case (int)CStageSongSelection.EReturnValue.CallConfig:
                                #region [ *** ]
                                //-----------------------------
                                rCurrentStage.OnDeactivate();
                                Trace.TraceInformation("----------------------");
                                Trace.TraceInformation("■ Config");
                                stageConfig.OnActivate();
                                rPreviousStage = rCurrentStage;
                                rCurrentStage = stageConfig;

                                foreach (STPlugin pg in this.listPlugins)
                                {
                                    Directory.SetCurrentDirectory(pg.strプラグインフォルダ);
                                    pg.plugin.OnChangeStage();
                                    Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                                }

                                this.tRunGarbageCollector();
                                break;
                            //-----------------------------
                                #endregion

                            case (int)CStageSongSelection.EReturnValue.ChangeSking:

                                #region [ *** ]
                                //-----------------------------
                                rCurrentStage.OnDeactivate();
                                Trace.TraceInformation("----------------------");
                                Trace.TraceInformation("■ スキン切り替え");
                                stageChangeSkin.OnActivate();
                                rPreviousStage = rCurrentStage;
                                rCurrentStage = stageChangeSkin;
                                break;
                            //-----------------------------
                                #endregion
                        }
                        //-----------------------------
                        #endregion
                        break;

                    case CStage.EStage.SongLoading:
                        #region [ *** ]
                        //-----------------------------
                        if (this.nUpdateAndDrawReturnValue != 0)
                        {
                            CDTXMania.Pad.stDetectedDevice.Clear();	// 入力デバイスフラグクリア(2010.9.11)

                            rCurrentStage.OnDeactivate();

                            #region [ ESC押下時は、曲の読み込みを中止して選曲画面に戻る ]
                            if (this.nUpdateAndDrawReturnValue == (int)ESongLoadingScreenReturnValue.LoadingStopped)
                            {
                                //DTX.tStopPlayingAllChips();
                                DTX.OnDeactivate();
                                Trace.TraceInformation("曲の読み込みを中止しました。");
                                this.tRunGarbageCollector();
                                Trace.TraceInformation("----------------------");
                                Trace.TraceInformation("■ SongSelection");
                                stageSongSelection.OnActivate();
                                rPreviousStage = rCurrentStage;
                                rCurrentStage = stageSongSelection;
                                foreach (STPlugin pg in this.listPlugins)
                                {
                                    Directory.SetCurrentDirectory(pg.strプラグインフォルダ);
                                    pg.plugin.OnChangeStage();
                                    Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                                }
                                break;
                            }
                            #endregion


                            if (!ConfigIni.bGuitarRevolutionMode)
                            {
                                Trace.TraceInformation("----------------------");
                                Trace.TraceInformation("■ Playing（ドラム画面）");
#if false		// #23625 2011.1.11 Config.iniからダメージ/回復値の定数変更を行う場合はここを有効にする 087リリースに合わせ機能無効化
for (int i = 0; i < 5; i++)
{
	for (int j = 0; j < 2; j++)
	{
		stage演奏ドラム画面.fDamageGaugeDelta[i, j] = ConfigIni.fGaugeFactor[i, j];
	}
}
for (int i = 0; i < 3; i++) {
	stage演奏ドラム画面.fDamageLevelFactor[i] = ConfigIni.fDamageLevelFactor[i];
}		
#endif
                                rPreviousStage = rCurrentStage;
                                rCurrentStage = stagePerfDrumsScreen;
                            }
                            else
                            {
                                Trace.TraceInformation("----------------------");
                                Trace.TraceInformation("■ Playing（ギター画面）");
#if false		// #23625 2011.1.11 Config.iniからダメージ/回復値の定数変更を行う場合はここを有効にする 087リリースに合わせ機能無効化
for (int i = 0; i < 5; i++)
{
	for (int j = 0; j < 2; j++)
	{
		stage演奏ギター画面.fDamageGaugeDelta[i, j] = ConfigIni.fGaugeFactor[i, j];
	}
}
for (int i = 0; i < 3; i++) {
	stage演奏ギター画面.fDamageLevelFactor[i] = ConfigIni.fDamageLevelFactor[i];
}		
#endif
                                rPreviousStage = rCurrentStage;
                                rCurrentStage = stagePerfGuitarScreen;
                            }

                            foreach (STPlugin pg in this.listPlugins)
                            {
                                Directory.SetCurrentDirectory(pg.strプラグインフォルダ);
                                pg.plugin.OnChangeStage();
                                Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                            }

                            this.tRunGarbageCollector();
                        }
                        //-----------------------------
                        #endregion
                        break;

                    case CStage.EStage.Playing:
                        #region [ *** ]
                        //-----------------------------
                        switch (this.nUpdateAndDrawReturnValue)
                        {
                            case (int)EPerfScreenReturnValue.Continue:
                                break;

                            case (int)EPerfScreenReturnValue.Interruption:
                            case (int)EPerfScreenReturnValue.Restart:
                                #region [ Cancel performance ]
                                //-----------------------------
                                scoreIni = this.tScoreIniへBGMAdjustとHistoryとPlayCountを更新("Play canceled");

                                #region [ プラグイン On演奏キャンセル() の呼び出し ]
                                //---------------------
                                foreach (STPlugin pg in this.listPlugins)
                                {
                                    Directory.SetCurrentDirectory(pg.strプラグインフォルダ);
                                    pg.plugin.On演奏キャンセル(scoreIni);
                                    Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                                }
                                //---------------------
                                #endregion

                                DTX.tStopPlayingAllChips();
                                DTX.OnDeactivate();
                                rCurrentStage.OnDeactivate();
                                if (bCompactMode)
                                {
                                    base.Window.Close();
                                }
                                else if (this.nUpdateAndDrawReturnValue == (int)EPerfScreenReturnValue.Restart)
                                {
                                    Trace.TraceInformation("----------------------");
                                    Trace.TraceInformation("■ SongLoading");
                                    stageSongLoading.OnActivate();
                                    rPreviousStage = rCurrentStage;
                                    rCurrentStage = stageSongLoading;

                                    foreach (STPlugin pg in this.listPlugins)
                                    {
                                        Directory.SetCurrentDirectory(pg.strプラグインフォルダ);
                                        pg.plugin.OnChangeStage();
                                        Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                                    }

                                    this.tRunGarbageCollector();
                                }
                                else
                                {
                                    Trace.TraceInformation("----------------------");
                                    Trace.TraceInformation("■ SongSelection");
                                    stageSongSelection.OnActivate();
                                    rPreviousStage = rCurrentStage;
                                    rCurrentStage = stageSongSelection;

                                    #region [ プラグイン Onステージ変更() の呼び出し ]
                                    //---------------------
                                    foreach (STPlugin pg in this.listPlugins)
                                    {
                                        Directory.SetCurrentDirectory(pg.strプラグインフォルダ);
                                        pg.plugin.OnChangeStage();
                                        Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                                    }
                                    //---------------------
                                    #endregion

                                    this.tRunGarbageCollector();
                                }
                                break;
                            //-----------------------------
                                #endregion

                            case (int)EPerfScreenReturnValue.StageFailure:
                                #region [ 演奏失敗(StageFailed) ]
                                //-----------------------------
                                scoreIni = this.tScoreIniへBGMAdjustとHistoryとPlayCountを更新("Stage failed");

                                #region [ プラグイン On演奏失敗() の呼び出し ]
                                //---------------------
                                foreach (STPlugin pg in this.listPlugins)
                                {
                                    Directory.SetCurrentDirectory(pg.strプラグインフォルダ);
                                    pg.plugin.On演奏失敗(scoreIni);
                                    Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                                }
                                //---------------------
                                #endregion

                                DTX.tStopPlayingAllChips();
                                DTX.OnDeactivate();
                                rCurrentStage.OnDeactivate();
                                if (bCompactMode)
                                {
                                    base.Window.Close();
                                }
                                else
                                {
                                    Trace.TraceInformation("----------------------");
                                    Trace.TraceInformation("■ SongSelection");
                                    stageSongSelection.OnActivate();
                                    rPreviousStage = rCurrentStage;
                                    rCurrentStage = stageSongSelection;

                                    #region [ プラグイン Onステージ変更() の呼び出し ]
                                    //---------------------
                                    foreach (STPlugin pg in this.listPlugins)
                                    {
                                        Directory.SetCurrentDirectory(pg.strプラグインフォルダ);
                                        pg.plugin.OnChangeStage();
                                        Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                                    }
                                    //---------------------
                                    #endregion

                                    this.tRunGarbageCollector();
                                }
                                break;
                            //-----------------------------
                                #endregion

                            case (int)EPerfScreenReturnValue.StageClear:
                                #region [ 演奏クリア ]
                                //-----------------------------
                                CScoreIni.CPerformanceEntry c演奏記録_Drums, c演奏記録_Guitar, c演奏記録_Bass;
                                bool bIsTrainingMode = false;
                                CDTX.CChip[] chipArray = new CDTX.CChip[10];
                                if (ConfigIni.bGuitarRevolutionMode)
                                {
                                    stagePerfGuitarScreen.tStorePerfResults(out c演奏記録_Drums, out c演奏記録_Guitar, out c演奏記録_Bass, out bIsTrainingMode);
                                }
                                else
                                {
                                    stagePerfDrumsScreen.tStorePerfResults(out c演奏記録_Drums, out c演奏記録_Guitar, out c演奏記録_Bass, out chipArray, out bIsTrainingMode);
                                }

                                if (!bIsTrainingMode)
                                {
                                    if (CDTXMania.ConfigIni.bIsSwappedGuitarBass)		// #24063 2011.1.24 yyagi Gt/Bsを入れ替えていたなら、演奏結果も入れ替える
                                    {
                                        CScoreIni.CPerformanceEntry t;
                                        t = c演奏記録_Guitar;
                                        c演奏記録_Guitar = c演奏記録_Bass;
                                        c演奏記録_Bass = t;

                                        CDTXMania.DTX.SwapGuitarBassInfos();			// 譜面情報も元に戻す
                                        CDTXMania.ConfigIni.SwapGuitarBassInfos_AutoFlags(); // #24415 2011.2.27 yyagi
                                        // リザルト集計時のみ、Auto系のフラグも元に戻す。
                                        // これを戻すのは、リザルト集計後。
                                    }													// "case CStage.EStage.Result:"のところ。

                                    double ps = 0.0, gs = 0.0;
                                    if (!c演奏記録_Drums.b全AUTOである && c演奏記録_Drums.nTotalChipsCount > 0)
                                    {
                                        ps = c演奏記録_Drums.dbPerformanceSkill;
                                        gs = c演奏記録_Drums.dbGameSkill;
                                    }
                                    else if (!c演奏記録_Guitar.b全AUTOである && c演奏記録_Guitar.nTotalChipsCount > 0)
                                    {
                                        ps = c演奏記録_Guitar.dbPerformanceSkill;
                                        gs = c演奏記録_Guitar.dbGameSkill;
                                    }
                                    else
                                    {
                                        ps = c演奏記録_Bass.dbPerformanceSkill;
                                        gs = c演奏記録_Bass.dbGameSkill;
                                    }
                                    string str = "Cleared";
                                    switch (CScoreIni.tCalculateOverallRankValue(c演奏記録_Drums, c演奏記録_Guitar, c演奏記録_Bass))
                                    {
                                        case (int)CScoreIni.ERANK.SS:
                                            str = string.Format("Cleared (SS: {0:F2})", ps);
                                            break;

                                        case (int)CScoreIni.ERANK.S:
                                            str = string.Format("Cleared (S: {0:F2})", ps);
                                            break;

                                        case (int)CScoreIni.ERANK.A:
                                            str = string.Format("Cleared (A: {0:F2})", ps);
                                            break;

                                        case (int)CScoreIni.ERANK.B:
                                            str = string.Format("Cleared (B: {0:F2})", ps);
                                            break;

                                        case (int)CScoreIni.ERANK.C:
                                            str = string.Format("Cleared (C: {0:F2})", ps);
                                            break;

                                        case (int)CScoreIni.ERANK.D:
                                            str = string.Format("Cleared (D: {0:F2})", ps);
                                            break;

                                        case (int)CScoreIni.ERANK.E:
                                            str = string.Format("Cleared (E: {0:F2})", ps);
                                            break;

                                        case (int)CScoreIni.ERANK.UNKNOWN:	// #23534 2010.10.28 yyagi add: 演奏チップが0個のとき
                                            str = "Cleared (No chips)";
                                            break;
                                    }
                                    
                                    scoreIni = this.tScoreIniへBGMAdjustとHistoryとPlayCountを更新(str);
                                }

                                #region [ プラグイン On演奏クリア() の呼び出し ]
                                //---------------------
                                foreach (STPlugin pg in this.listPlugins)
                                {
                                    Directory.SetCurrentDirectory(pg.strプラグインフォルダ);
                                    pg.plugin.On演奏クリア(scoreIni);
                                    Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                                }
                                //---------------------
                                #endregion

                                rCurrentStage.OnDeactivate();
                                Trace.TraceInformation("----------------------");
                                Trace.TraceInformation("■ Result");
                                stageResult.stPerformanceEntry.Drums = c演奏記録_Drums;
                                stageResult.stPerformanceEntry.Guitar = c演奏記録_Guitar;
                                stageResult.stPerformanceEntry.Bass = c演奏記録_Bass;
                                stageResult.rEmptyDrumChip = chipArray;
                                stageResult.bIsTrainingMode = bIsTrainingMode;
                                stageResult.OnActivate();
                                rPreviousStage = rCurrentStage;
                                rCurrentStage = stageResult;

                                #region [ プラグイン Onステージ変更() の呼び出し ]
                                //---------------------
                                foreach (STPlugin pg in this.listPlugins)
                                {
                                    Directory.SetCurrentDirectory(pg.strプラグインフォルダ);
                                    pg.plugin.OnChangeStage();
                                    Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                                }
                                //---------------------
                                #endregion

                                break;
                            //-----------------------------
                                #endregion
                        }
                        //-----------------------------
                        #endregion
                        break;

                    case CStage.EStage.Result:
                        #region [ *** ]
                        //-----------------------------
                        if (this.nUpdateAndDrawReturnValue != 0)
                        {
                            if (CDTXMania.ConfigIni.bIsSwappedGuitarBass)		// #24415 2011.2.27 yyagi Gt/Bsを入れ替えていたなら、Auto状態をリザルト画面終了後に元に戻す
                            {
                                CDTXMania.ConfigIni.SwapGuitarBassInfos_AutoFlags(); // Auto入れ替え
                            }

                            DTX.tPausePlaybackForAllChips();
                            DTX.OnDeactivate();
                            rCurrentStage.OnDeactivate();
                            if (!bCompactMode)
                            {
                                Trace.TraceInformation("----------------------");
                                Trace.TraceInformation("■ SongSelection");
                                stageSongSelection.OnActivate();
                                rPreviousStage = rCurrentStage;
                                rCurrentStage = stageSongSelection;

                                foreach (STPlugin pg in this.listPlugins)
                                {
                                    Directory.SetCurrentDirectory(pg.strプラグインフォルダ);
                                    pg.plugin.OnChangeStage();
                                    Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                                }

                                this.tRunGarbageCollector();
                            }
                            else
                            {
                                base.Window.Close();
                            }
                        }
                        //-----------------------------
                        #endregion
                        break;

                    case CStage.EStage.ChangeSkin:
                        #region [ *** ]
                        //-----------------------------
                        if (this.nUpdateAndDrawReturnValue != 0)
                        {
                            rCurrentStage.OnDeactivate();
                            Trace.TraceInformation("----------------------");
                            Trace.TraceInformation("■ SongSelection");
                            stageSongSelection.OnActivate();
                            rPreviousStage = rCurrentStage;
                            rCurrentStage = stageSongSelection;
                            this.tRunGarbageCollector();
                        }
                        //-----------------------------
                        #endregion
                        break;

                    case CStage.EStage.End:
                        #region [ *** ]
                        //-----------------------------
                        if (this.nUpdateAndDrawReturnValue != 0)
                        {
                            base.Exit();
                        }
                        //-----------------------------
                        #endregion
                        break;
                }
            }
            this.Device.EndScene();			// Present()は game.csのOnFrameEnd()に登録された、GraphicsDeviceManager.game_FrameEnd() 内で実行されるので不要
            // (つまり、Present()は、Draw()完了後に実行される)
#if !GPUFlushAfterPresent
            actFlushGPU.OnUpdateAndDraw();		// Flush GPU	// EndScene()～Present()間 (つまりVSync前) でFlush実行
#endif
            #region [ 全画面_ウインドウ切り替え ]
            if (this.b次のタイミングで全画面_ウィンドウ切り替えを行う)
            {
                ConfigIni.bFullScreenMode = !ConfigIni.bFullScreenMode;
                app.tSwitchFullScreenMode();
                this.b次のタイミングで全画面_ウィンドウ切り替えを行う = false;
            }
            #endregion
            #region [ 垂直基線同期切り替え ]
            if (this.b次のタイミングで垂直帰線同期切り替えを行う)
            {
                bool bIsMaximized = this.Window.IsMaximized;											// #23510 2010.11.3 yyagi: to backup current window mode before changing VSyncWait
                currentClientSize = this.Window.ClientSize;												// #23510 2010.11.3 yyagi: to backup current window size before changing VSyncWait
                DeviceSettings currentSettings = app.GraphicsDeviceManager.CurrentSettings;
                currentSettings.EnableVSync = ConfigIni.bVerticalSyncWait;
                app.GraphicsDeviceManager.ChangeDevice(currentSettings);
                this.b次のタイミングで垂直帰線同期切り替えを行う = false;
                base.Window.ClientSize = new Size(currentClientSize.Width, currentClientSize.Height);	// #23510 2010.11.3 yyagi: to resume window size after changing VSyncWait
                if (bIsMaximized)
                {
                    this.Window.WindowState = FormWindowState.Maximized;								// #23510 2010.11.3 yyagi: to resume window mode after changing VSyncWait
                }
            }
            #endregion
        }


        // Other

		#region [ 汎用ヘルパー ]
		//-----------------
		public static CTexture tGenerateTexture( string fileName )
		{
			return tGenerateTexture( fileName, false );
		}
		public static CTexture tGenerateTexture( string fileName, bool b黒を透過する )
		{
			if ( app == null )
			{
				return null;
			}
			try
			{
                //Trace.WriteLine("CTextureをFileから生成 + Filename:" + fileName);
				return new CTexture( app.Device, fileName, TextureFormat, b黒を透過する );
			}
			catch ( CTextureCreateFailedException )
			{
				Trace.TraceError( "テクスチャの生成に失敗しました。({0})", fileName );
				return null;
			}
			catch ( FileNotFoundException )
			{
				Trace.TraceError( "テクスチャファイルが見つかりませんでした。({0})", fileName );
				return null;
			}
		}
		public static void tReleaseTexture( ref CTexture tx )
		{
            if (tx != null) {
                //Trace.WriteLine( "CTextureを解放 Size W:" + tx.szImageSize.Width + " H:" + tx.szImageSize.Height );
			    CDTXMania.t安全にDisposeする( ref tx );
            }
		}
        public static void tReleaseTexture( ref CTextureAf tx )
		{
			CDTXMania.t安全にDisposeする( ref tx );
		}
		public static CTexture tGenerateTexture( byte[] txData )
		{
			return tGenerateTexture( txData, false );
		}
		public static CTexture tGenerateTexture( byte[] txData, bool b黒を透過する )
		{
			if ( app == null )
			{
				return null;
			}
			try
			{
				return new CTexture( app.Device, txData, TextureFormat, b黒を透過する );
			}
			catch ( CTextureCreateFailedException )
			{
				Trace.TraceError( "テクスチャの生成に失敗しました。(txData)" );
				return null;
			}
		}

		public static CTexture tGenerateTexture( Bitmap bitmap )
		{
			return tGenerateTexture( bitmap, false );
		}
		public static CTexture tGenerateTexture( Bitmap bitmap, bool b黒を透過する )
		{
			if ( app == null )
			{
				return null;
			}
			try
			{
                //Trace.WriteLine( "CTextureをBitmapから生成" );
				return new CTexture( app.Device, bitmap, TextureFormat, b黒を透過する );
			}
			catch ( CTextureCreateFailedException )
			{
				Trace.TraceError( "テクスチャの生成に失敗しました。(txData)" );
				return null;
			}
        }

        public static CTextureAf tテクスチャの生成Af( string fileName )
		{
			return tテクスチャの生成Af( fileName, false );
		}
		public static CTextureAf tテクスチャの生成Af( string fileName, bool b黒を透過する )
		{
			if ( app == null )
			{
				return null;
			}
			try
			{
				return new CTextureAf( app.Device, fileName, TextureFormat, b黒を透過する );
			}
			catch ( CTextureCreateFailedException )
			{
				Trace.TraceError( "テクスチャの生成に失敗しました。({0})", fileName );
				return null;
			}
			catch ( FileNotFoundException )
			{
				Trace.TraceError( "テクスチャファイルが見つかりませんでした。({0})", fileName );
				return null;
			}
		}

        public static CDirectShow t失敗してもスキップ可能なDirectShowを生成する(string fileName, IntPtr hWnd, bool bオーディオレンダラなし)
        {
            CDirectShow ds = null;
            if( File.Exists( fileName ) )
            {
                try
                {
                    ds = new CDirectShow(fileName, hWnd, bオーディオレンダラなし);
                }
                catch (FileNotFoundException)
                {
                    Trace.TraceError("動画ファイルが見つかりませんでした。({0})", fileName);
                    ds = null;      // Dispose はコンストラクタ内で実施済み
                }
                catch
                {
                    Trace.TraceError("DirectShow の生成に失敗しました。[{0}]", fileName);
                    ds = null;      // Dispose はコンストラクタ内で実施済み
                }
            }
            else
            {
                Trace.TraceError("動画ファイルが見つかりませんでした。({0})", fileName);
                return null;
            }

            return ds;
        }

		/// <summary>プロパティ、インデクサには ref は使用できないので注意。</summary>
		public static void t安全にDisposeする<T>( ref T obj )
		{
			if ( obj == null )
				return;

			var d = obj as IDisposable;

			if ( d != null )
				d.Dispose();

			obj = default( T );
		}
		//-----------------
		#endregion
        #region [ private ]
        //-----------------
        private bool bマウスカーソル表示中 = true;
        public bool bウィンドウがアクティブである = true;
        private bool b終了処理完了済み;
        private static CDTX dtx;
        private List<CActivity> listTopLevelActivities;
        public int nUpdateAndDrawReturnValue;
        private MouseButtons mb = System.Windows.Forms.MouseButtons.Left;
        private string strWindowTitle = "";

        //
        public CIMEHook cIMEHook;
        public CActTextBox textboxテキスト入力中;
        public bool bテキスト入力中 => textboxテキスト入力中 != null;

        private void tStartProcess()
        {
            #region [ Determine strEXE folder ]
            //-----------------
            // BEGIN #23629 2010.11.13 from: デバッグ時は Application.ExecutablePath が ($SolutionDir)/bin/x86/Debug/ などになり System/ の読み込みに失敗するので、カレントディレクトリを採用する。（プロジェクトのプロパティ→デバッグ→作業ディレクトリが有効になる）
#if DEBUG
            strEXEのあるフォルダ = Environment.CurrentDirectory + @"\";
#else
			strEXEのあるフォルダ = Path.GetDirectoryName( Application.ExecutablePath ) + @"\";	// #23629 2010.11.9 yyagi: set correct pathname where DTXManiaGR.exe is.
#endif
            // END #23629 2010.11.13 from
            //-----------------
            #endregion

            #region [ Read Config.ini ]
            //---------------------
            ConfigIni = new CConfigIni();
            string path = strEXEのあるフォルダ + "Config.ini";
            if (File.Exists(path))
            {
                try
                {
                    ConfigIni.tReadFromFile(path);
                }
                catch
                {
                    //ConfigIni = new CConfigIni();	// 存在してなければ新規生成
                }
            }
            this.Window.EnableSystemMenu = CDTXMania.ConfigIni.bIsEnabledSystemMenu;	// #28200 2011.5.1 yyagi
            // 2012.8.22 Config.iniが無いときに初期値が適用されるよう、この設定行をifブロック外に移動

            //---------------------
            #endregion
            #region [ Start output log ]
            //---------------------
            Trace.AutoFlush = true;
            if (ConfigIni.bOutputLogs)
            {
                try
                {
                    Trace.Listeners.Add(new CTraceLogListener(new StreamWriter("DTXManiaLog.txt", false, Encoding.GetEncoding("shift-jis"))));
                }
                catch (System.UnauthorizedAccessException)			// #24481 2011.2.20 yyagi
                {
                    int c = (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ja") ? 0 : 1;
                    string[] mes_writeErr = {
						"DTXManiaLog.txtへの書き込みができませんでした。書き込みできるようにしてから、再度起動してください。",
						"Failed to write DTXManiaLog.txt. Please set it writable and try again."
					};
                    MessageBox.Show(mes_writeErr[c], "DTXMania boot error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(1);
                }
            }
            Trace.WriteLine("");
            Trace.WriteLine("DTXMania powered by YAMAHA Silent Session Drums");
            Trace.WriteLine(string.Format("Release: {0}", VERSION));
            Trace.WriteLine("");
            Trace.TraceInformation("----------------------");
            Trace.TraceInformation("■ アプリケーションの初期化");
            Trace.TraceInformation("OS Version: " + Environment.OSVersion);
            Trace.TraceInformation("ProcessorCount: " + Environment.ProcessorCount.ToString());
            Trace.TraceInformation("CLR Version: " + Environment.Version.ToString());
            //---------------------
            #endregion
            #region [ Detect compact mode ]
            //---------------------
            bCompactMode = false;
            strCompactModeFile = "";
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            if ((commandLineArgs != null) && (commandLineArgs.Length > 1))
            {
                bCompactMode = true;
                strCompactModeFile = commandLineArgs[1];
                Trace.TraceInformation("Start in compact mode. [{0}]", strCompactModeFile);
            }
            //---------------------
            #endregion

            #region [ Initialize window ]
            //---------------------
            this.strWindowTitle = "DTXMania .NET style release " + VERSION;
            base.Window.StartPosition = FormStartPosition.Manual;                                                       // #30675 2013.02.04 ikanick add
            base.Window.Location = new Point(ConfigIni.n初期ウィンドウ開始位置X, ConfigIni.n初期ウィンドウ開始位置Y);   // #30675 2013.02.04 ikanick add

            base.Window.Text = this.strWindowTitle;
            base.Window.ClientSize = new Size(ConfigIni.nウインドウwidth, ConfigIni.nウインドウheight);	// #34510 yyagi 2010.10.31 to change window size got from Config.ini
#if !WindowedFullscreen
            if (!ConfigIni.bウィンドウモード)						// #23510 2010.11.02 yyagi: add; to recover window size in case bootup with fullscreen mode
            {
#endif
                currentClientSize = new Size(ConfigIni.nウインドウwidth, ConfigIni.nウインドウheight);
#if !WindowedFullscreen
            }
#endif
            base.Window.MaximizeBox = true;							// #23510 2010.11.04 yyagi: to support maximizing window
            base.Window.FormBorderStyle = FormBorderStyle.Sizable;	// #23510 2010.10.27 yyagi: changed from FixedDialog to Sizable, to support window resize
            base.Window.ShowIcon = true;
            base.Window.Icon = Properties.Resources.dtx;
            base.Window.KeyDown += new KeyEventHandler(this.Window_KeyDown);
            base.Window.MouseUp += new MouseEventHandler(this.Window_MouseUp);
            base.Window.MouseDoubleClick += new MouseEventHandler(this.Window_MouseDoubleClick);	// #23510 2010.11.13 yyagi: to go fullscreen mode
            base.Window.ResizeEnd += new EventHandler(this.Window_ResizeEnd);						// #23510 2010.11.20 yyagi: to set resized window size in Config.ini
            base.Window.ApplicationActivated += new EventHandler(this.Window_ApplicationActivated);
            base.Window.ApplicationDeactivated += new EventHandler(this.Window_ApplicationDeactivated);
            //Add CIMEHook
            base.Window.Controls.Add(cIMEHook = new CIMEHook());
            //---------------------
            #endregion
            #region [ Generate Direct3D9 device ]
            //---------------------
            DeviceSettings settings = new DeviceSettings();
#if WindowedFullscreen
			settings.Windowed = true;								// #30666 2013.2.2 yyagi: Fullscreenmode is "Maximized window" mode
#else
            settings.Windowed = ConfigIni.bウィンドウモード;
#endif
            settings.BackBufferWidth = SampleFramework.GameWindowSize.Width;
            settings.BackBufferHeight = SampleFramework.GameWindowSize.Height;
            //			settings.BackBufferCount = 3;
            settings.EnableVSync = ConfigIni.bVerticalSyncWait;
            //			settings.BackBufferFormat = Format.A8R8G8B8;
            //			settings.MultisampleType = MultisampleType.FourSamples;
            //			settings.MultisampleQuality = 4;
            //			settings.MultisampleType = MultisampleType.None;
            //			settings.MultisampleQuality = 0;

            try
            {
                base.GraphicsDeviceManager.ChangeDevice(settings);
            }
            catch (DeviceCreationException e)
            {
                Trace.TraceError(e.ToString());
                MessageBox.Show(e.Message + e.ToString(), "DTXMania failed to boot: DirectX9 Initialize Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }

            base.IsFixedTimeStep = false;
            //			base.TargetElapsedTime = TimeSpan.FromTicks( 10000000 / 75 );
            base.Window.ClientSize = new Size(ConfigIni.nウインドウwidth, ConfigIni.nウインドウheight);	// #23510 2010.10.31 yyagi: to recover window size. width and height are able to get from Config.ini.
            base.InactiveSleepTime = TimeSpan.FromMilliseconds((float)(ConfigIni.n非フォーカス時スリープms));	// #23568 2010.11.3 yyagi: to support valiable sleep value when !IsActive
            // #23568 2010.11.4 ikanick changed ( 1 -> ConfigIni )
#if WindowedFullscreen
			this.t全画面_ウィンドウモード切り替え();				// #30666 2013.2.2 yyagi: finalize settings for "Maximized window mode"
#endif
            actFlushGPU = new CActFlushGPU();
            //---------------------
            #endregion

            DTX = null;

            #region [ Initialize Skin ]
            //---------------------
            Trace.TraceInformation("スキンの初期化を行います。");
            Trace.Indent();
            try
            {
                Skin = new CSkin(CDTXMania.ConfigIni.strSystemSkinSubfolderFullName, CDTXMania.ConfigIni.bUseBoxDefSkin);
                CDTXMania.ConfigIni.strSystemSkinSubfolderFullName = CDTXMania.Skin.GetCurrentSkinSubfolderFullName(true);	// 旧指定のSkinフォルダが消滅していた場合に備える
                Trace.TraceInformation("スキンの初期化を完了しました。");
            }
            catch
            {
                Trace.TraceInformation("スキンの初期化に失敗しました。");
                throw;
            }
            finally
            {
                Trace.Unindent();
            }
            //---------------------
            #endregion
            #region [ Initialize Timer ]
            //---------------------
            Trace.TraceInformation("タイマの初期化を行います。");
            Trace.Indent();
            try
            {
                Timer = new CTimer(CTimer.EType.MultiMedia);
                Trace.TraceInformation("タイマの初期化を完了しました。");
            }
            finally
            {
                Trace.Unindent();
            }
            //---------------------
            #endregion
            #region [ FPS カウンタの初期化 ]
            //---------------------
            Trace.TraceInformation("FPSカウンタの初期化を行います。");
            Trace.Indent();
            try
            {
                FPS = new CFPS();
                Trace.TraceInformation("FPSカウンタを生成しました。");
            }
            finally
            {
                Trace.Unindent();
            }
            //---------------------
            #endregion
            #region [ act文字コンソールの初期化 ]
            //---------------------
            Trace.TraceInformation("文字コンソールの初期化を行います。");
            Trace.Indent();
            try
            {
                actDisplayString = new CCharacterConsole();
                Trace.TraceInformation("文字コンソールを生成しました。");
                actDisplayString.OnActivate();
                Trace.TraceInformation("文字コンソールを活性化しました。");
                Trace.TraceInformation("文字コンソールの初期化を完了しました。");
            }
            catch (Exception exception)
            {
                Trace.TraceError(exception.Message);
                Trace.TraceError("文字コンソールの初期化に失敗しました。");
            }
            finally
            {
                Trace.Unindent();
            }
            //---------------------
            #endregion
            #region [ Initialize Input Manager ]
            //---------------------
            Trace.TraceInformation("DirectInput, MIDI入力の初期化を行います。");
            Trace.Indent();
            try
            {
                InputManager = new CInputManager(base.Window.Handle);
                foreach (IInputDevice device in InputManager.listInputDevices)
                {
                    if ((device.eInputDeviceType == EInputDeviceType.Joystick) && !ConfigIni.dicJoystick.ContainsValue(device.GUID))
                    {
                        int key = 0;
                        while (ConfigIni.dicJoystick.ContainsKey(key))
                        {
                            key++;
                        }
                        ConfigIni.dicJoystick.Add(key, device.GUID);
                    }
                }
                foreach (IInputDevice device2 in InputManager.listInputDevices)
                {
                    if (device2.eInputDeviceType == EInputDeviceType.Joystick)
                    {
                        foreach (KeyValuePair<int, string> pair in ConfigIni.dicJoystick)
                        {
                            if (device2.GUID.Equals(pair.Value))
                            {
                                ((CInputJoystick)device2).SetID(pair.Key);
                                break;
                            }
                        }
                        continue;
                    }
                }
                Trace.TraceInformation("DirectInput の初期化を完了しました。");
            }
            catch (Exception exception2)
            {
                Trace.TraceError(exception2.Message);
                Trace.TraceError("DirectInput, MIDI入力の初期化に失敗しました。");
                throw;
            }
            finally
            {
                Trace.Unindent();
            }
            //---------------------
            #endregion
            #region [ Initialize Pad ]
            //---------------------
            Trace.TraceInformation("パッドの初期化を行います。");
            Trace.Indent();
            try
            {
                Pad = new CPad(ConfigIni, InputManager);
                Trace.TraceInformation("パッドの初期化を完了しました。");
            }
            catch (Exception exception3)
            {
                Trace.TraceError(exception3.Message);
                Trace.TraceError("パッドの初期化に失敗しました。");
            }
            finally
            {
                Trace.Unindent();
            }
            //---------------------
            #endregion
            #region [ Initialize Sound Manager ]
            //---------------------
            Trace.TraceInformation("サウンドデバイスの初期化を行います。");
            Trace.Indent();
            try
            {
                {
                    ESoundDeviceType soundDeviceType;
                    switch (CDTXMania.ConfigIni.nSoundDeviceType)
                    {
                        case 0:
                            soundDeviceType = ESoundDeviceType.DirectSound;
                            break;
                        case 1:
                            soundDeviceType = ESoundDeviceType.ASIO;
                            break;
                        case 2:
                            soundDeviceType = ESoundDeviceType.ExclusiveWASAPI;
                            break;
                        case 3:
                            soundDeviceType = ESoundDeviceType.SharedWASAPI;
                            break;
                        default:
                            soundDeviceType = ESoundDeviceType.Unknown;
                            break;
                    }
                    SoundManager = new CSoundManager(base.Window.Handle,
                                                soundDeviceType,
                                                CDTXMania.ConfigIni.nWASAPIBufferSizeMs,
                                                CDTXMania.ConfigIni.bEventDrivenWASAPI,
                                                0,
                                                CDTXMania.ConfigIni.nASIODevice,
                                                CDTXMania.ConfigIni.bUseOSTimer
                    );
                    AddSoundTypeToWindowTitle();
                    FDK.CSoundManager.bIsTimeStretch = CDTXMania.ConfigIni.bTimeStretch;
                    SoundManager.nMasterVolume = CDTXMania.ConfigIni.nMasterVolume;
                    //FDK.CSound管理.bIsMP3DecodeByWindowsCodec = CDTXMania.ConfigIni.bNoMP3Streaming;


                    string strDefaultSoundDeviceBusType = CSoundManager.strDefaultDeviceBusType;
                    Trace.TraceInformation($"Bus type of the default sound device = {strDefaultSoundDeviceBusType}");

                    Trace.TraceInformation("サウンドデバイスの初期化を完了しました。");
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                throw;
            }
            finally
            {
                Trace.Unindent();
            }
            //---------------------
            #endregion
            #region [ Initialize Song Manager ]
            //---------------------
            Trace.TraceInformation("曲リストの初期化を行います。");
            Trace.Indent();
            try
            {
                SongManager = new CSongManager();
                //				Songs管理_裏読 = new CSongManager();
                EnumSongs = new CEnumSongs();
                actEnumSongs = new CActEnumSongs();
                Trace.TraceInformation("曲リストの初期化を完了しました。");
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                Trace.TraceError("曲リストの初期化に失敗しました。");
            }
            finally
            {
                Trace.Unindent();
            }
            //---------------------
            #endregion
            #region [ Initialize CAvi ]
            //---------------------
            CAvi.t初期化();
            //---------------------
            #endregion
            #region [ Initialize Random ]
            //---------------------
            Random = new Random((int)Timer.nシステム時刻);
            //---------------------
            #endregion
            #region [ Initialize Stage ]
            //---------------------
            rCurrentStage = null;
            rPreviousStage = null;
            stageStartup = new CStageStartup();
            stageTitle = new CStageTitle();
            stageOption = new CStageOption();
            stageConfig = new CStageConfig();
            stageSongSelection = new CStageSongSelection();
            stageSongLoading = new CStageSongLoading();
            stagePerfDrumsScreen = new CStagePerfDrumsScreen();
            stagePerfGuitarScreen = new CStagePerfGuitarScreen();
            stageResult = new CStageResult();
            stageChangeSkin = new CStageChangeSkin();
            stageEnd = new CStageEnd();
            this.listTopLevelActivities = new List<CActivity>();
            this.listTopLevelActivities.Add(actEnumSongs);
            this.listTopLevelActivities.Add(actDisplayString);
            this.listTopLevelActivities.Add(stageStartup);
            this.listTopLevelActivities.Add(stageTitle);
            this.listTopLevelActivities.Add(stageOption);
            this.listTopLevelActivities.Add(stageConfig);
            this.listTopLevelActivities.Add(stageSongSelection);
            this.listTopLevelActivities.Add(stageSongLoading);
            this.listTopLevelActivities.Add(stagePerfDrumsScreen);
            this.listTopLevelActivities.Add(stagePerfGuitarScreen);
            this.listTopLevelActivities.Add(stageResult);
            this.listTopLevelActivities.Add(stageChangeSkin);
            this.listTopLevelActivities.Add(stageEnd);
            this.listTopLevelActivities.Add(actFlushGPU);
            //---------------------
            #endregion
            #region [ Search and generate Plugin ]
            //---------------------
            PluginHost = new CPluginHost();

            Trace.TraceInformation("プラグインの検索と生成を行います。");
            Trace.Indent();
            try
            {
                this.tプラグイン検索と生成();
                Trace.TraceInformation("プラグインの検索と生成を完了しました。");
            }
            finally
            {
                Trace.Unindent();
            }
            //---------------------
            #endregion
            #region [ Initialize Plugin ]
            //---------------------
            if (this.listPlugins != null && this.listPlugins.Count > 0)
            {
                Trace.TraceInformation("プラグインの初期化を行います。");
                Trace.Indent();
                try
                {
                    foreach (STPlugin st in this.listPlugins)
                    {
                        Directory.SetCurrentDirectory(st.strプラグインフォルダ);
                        st.plugin.On初期化(this.PluginHost);
                        st.plugin.OnManagedリソースの作成();
                        st.plugin.OnUnmanagedリソースの作成();
                        Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                    }
                    Trace.TraceInformation("すべてのプラグインの初期化を完了しました。");
                }
                catch
                {
                    Trace.TraceError("プラグインのどれかの初期化に失敗しました。");
                    throw;
                }
                finally
                {
                    Trace.Unindent();
                }
            }

            //---------------------
            #endregion


            Trace.TraceInformation("アプリケーションの初期化を完了しました。");

            #region [ Launch First stage ]
            //---------------------
            Trace.TraceInformation("----------------------");
            Trace.TraceInformation("■ Startup");

            if (CDTXMania.bCompactMode)
            {
                rCurrentStage = stageSongLoading;
            }
            else
            {
                rCurrentStage = stageStartup;
            }
            rCurrentStage.OnActivate();
            //---------------------
            #endregion
        }
        public void AddSoundTypeToWindowTitle()
        {
            string delay = "";
            if (SoundManager.GetCurrentSoundDeviceType() != "DirectSound")
            {
                delay = "(" + SoundManager.GetSoundDelay() + "ms)";
            }
            base.Window.Text = strWindowTitle + " (" + SoundManager.GetCurrentSoundDeviceType() + delay + ")";
        }

        private void tTerminate()  // t終了処理
        {
            if (!this.b終了処理完了済み)
            {
                Trace.TraceInformation("----------------------");
                Trace.TraceInformation("■ アプリケーションの終了");
                #region [ 曲検索の終了処理 ]
                //---------------------
                if (actEnumSongs != null)
                {
                    Trace.TraceInformation("曲検索actの終了処理を行います。");
                    Trace.Indent();
                    try
                    {
                        actEnumSongs.OnDeactivate();
                        actEnumSongs = null;
                        Trace.TraceInformation("曲検索actの終了処理を完了しました。");
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError(e.Message);
                        Trace.TraceError("曲検索actの終了処理に失敗しました。");
                    }
                    finally
                    {
                        Trace.Unindent();
                    }
                }
                //---------------------
                #endregion
                #region [ 現在のステージの終了処理 ]
                //---------------------
                if (CDTXMania.rCurrentStage != null && CDTXMania.rCurrentStage.bActivated)		// #25398 2011.06.07 MODIFY FROM
                {
                    Trace.TraceInformation("現在のステージを終了します。");
                    Trace.Indent();
                    try
                    {
                        rCurrentStage.OnDeactivate();
                        Trace.TraceInformation("現在のステージの終了処理を完了しました。");
                    }
                    finally
                    {
                        Trace.Unindent();
                    }
                }
                //---------------------
                #endregion
                #region [ プラグインの終了処理 ]
                //---------------------
                if (this.listPlugins != null && this.listPlugins.Count > 0)
                {
                    Trace.TraceInformation("すべてのプラグインを終了します。");
                    Trace.Indent();
                    try
                    {
                        foreach (STPlugin st in this.listPlugins)
                        {
                            Directory.SetCurrentDirectory(st.strプラグインフォルダ);
                            st.plugin.OnUnmanagedリソースの解放();
                            st.plugin.OnManagedリソースの解放();
                            st.plugin.On終了();
                            Directory.SetCurrentDirectory(CDTXMania.strEXEのあるフォルダ);
                        }
                        PluginHost = null;
                        Trace.TraceInformation("すべてのプラグインの終了処理を完了しました。");
                    }
                    finally
                    {
                        Trace.Unindent();
                    }
                }
                //---------------------
                #endregion
                #region [ 曲リストの終了処理 ]
                //---------------------
                if (SongManager != null)
                {
                    Trace.TraceInformation("曲リストの終了処理を行います。");
                    Trace.Indent();
                    try
                    {
                        SongManager = null;
                        Trace.TraceInformation("曲リストの終了処理を完了しました。");
                    }
                    catch (Exception exception)
                    {
                        Trace.TraceError(exception.Message);
                        Trace.TraceError("曲リストの終了処理に失敗しました。");
                    }
                    finally
                    {
                        Trace.Unindent();
                    }
                }
                CAvi.t終了();
                //---------------------
                #endregion
                #region [ スキンの終了処理 ]
                //---------------------
                if (Skin != null)
                {
                    Trace.TraceInformation("スキンの終了処理を行います。");
                    Trace.Indent();
                    try
                    {
                        CDTXMania.Skin.tSaveSkinConfig(); //2016.07.30 kairera0467 #36413
                        Skin.Dispose();
                        Skin = null;
                        Trace.TraceInformation("スキンの終了処理を完了しました。");
                    }
                    catch (Exception exception2)
                    {
                        Trace.TraceError(exception2.Message);
                        Trace.TraceError("スキンの終了処理に失敗しました。");
                    }
                    finally
                    {
                        Trace.Unindent();
                    }
                }
                //---------------------
                #endregion
                #region [ DirectSoundの終了処理 ]
                //---------------------
                if (SoundManager != null)
                {
                    Trace.TraceInformation("DirectSound の終了処理を行います。");
                    Trace.Indent();
                    try
                    {
                        SoundManager.Dispose();
                        SoundManager = null;
                        Trace.TraceInformation("DirectSound の終了処理を完了しました。");
                    }
                    catch (Exception exception3)
                    {
                        Trace.TraceError(exception3.Message);
                        Trace.TraceError("DirectSound の終了処理に失敗しました。");
                    }
                    finally
                    {
                        Trace.Unindent();
                    }
                }
                //---------------------
                #endregion
                #region [ パッドの終了処理 ]
                //---------------------
                if (Pad != null)
                {
                    Trace.TraceInformation("パッドの終了処理を行います。");
                    Trace.Indent();
                    try
                    {
                        Pad = null;
                        Trace.TraceInformation("パッドの終了処理を完了しました。");
                    }
                    catch (Exception exception4)
                    {
                        Trace.TraceError(exception4.Message);
                        Trace.TraceError("パッドの終了処理に失敗しました。");
                    }
                    finally
                    {
                        Trace.Unindent();
                    }
                }
                //---------------------
                #endregion
                #region [ DirectInput, MIDI入力の終了処理 ]
                //---------------------
                if (InputManager != null)
                {
                    Trace.TraceInformation("DirectInput, MIDI入力の終了処理を行います。");
                    Trace.Indent();
                    try
                    {
                        InputManager.Dispose();
                        InputManager = null;
                        Trace.TraceInformation("DirectInput, MIDI入力の終了処理を完了しました。");
                    }
                    catch (Exception exception5)
                    {
                        Trace.TraceError(exception5.Message);
                        Trace.TraceError("DirectInput, MIDI入力の終了処理に失敗しました。");
                    }
                    finally
                    {
                        Trace.Unindent();
                    }
                }
                //---------------------
                #endregion
                #region [ 文字コンソールの終了処理 ]
                //---------------------
                if (actDisplayString != null)
                {
                    Trace.TraceInformation("文字コンソールの終了処理を行います。");
                    Trace.Indent();
                    try
                    {
                        actDisplayString.OnDeactivate();
                        actDisplayString = null;
                        Trace.TraceInformation("文字コンソールの終了処理を完了しました。");
                    }
                    catch (Exception exception6)
                    {
                        Trace.TraceError(exception6.Message);
                        Trace.TraceError("文字コンソールの終了処理に失敗しました。");
                    }
                    finally
                    {
                        Trace.Unindent();
                    }
                }
                //---------------------
                #endregion
                #region [ FPSカウンタの終了処理 ]
                //---------------------
                Trace.TraceInformation("FPSカウンタの終了処理を行います。");
                Trace.Indent();
                try
                {
                    if (FPS != null)
                    {
                        FPS = null;
                    }
                    Trace.TraceInformation("FPSカウンタの終了処理を完了しました。");
                }
                finally
                {
                    Trace.Unindent();
                }
                //---------------------
                #endregion

                //				ct.Dispose();

                #region [ タイマの終了処理 ]
                //---------------------
                Trace.TraceInformation("タイマの終了処理を行います。");
                Trace.Indent();
                try
                {
                    if (Timer != null)
                    {
                        Timer.Dispose();
                        Timer = null;
                        Trace.TraceInformation("タイマの終了処理を完了しました。");
                    }
                    else
                    {
                        Trace.TraceInformation("タイマは使用されていません。");
                    }
                }
                finally
                {
                    Trace.Unindent();
                }
                //---------------------
                #endregion
                #region [ Config.iniの出力 ]
                //---------------------
                Trace.TraceInformation("Config.ini を出力します。");
                //				if ( ConfigIni.bIsSwappedGuitarBass )			// #24063 2011.1.16 yyagi ギターベースがスワップしているときは元に戻す
                if (ConfigIni.bIsSwappedGuitarBass_AutoFlagsAreSwapped)	// #24415 2011.2.21 yyagi FLIP中かつ演奏中にalt-f4で終了したときは、AUTOのフラグをswapして戻す
                {
                    ConfigIni.SwapGuitarBassInfos_AutoFlags();
                }
                string str = strEXEのあるフォルダ + "Config.ini";
                Trace.Indent();
                try
                {
                    ConfigIni.tWrite(str);
                    Trace.TraceInformation("保存しました。({0})", new object[] { str });
                }
                catch (Exception e)
                {
                    Trace.TraceError(e.Message);
                    Trace.TraceError("Config.ini の出力に失敗しました。({0})", new object[] { str });
                }
                finally
                {
                    Trace.Unindent();
                }
                //---------------------
                #endregion
                Trace.TraceInformation("アプリケーションの終了処理を完了しました。");


                this.b終了処理完了済み = true;
            }
        }

        private CScoreIni tScoreIniへBGMAdjustとHistoryとPlayCountを更新(string str新ヒストリ行)
        {
            bool bIsUpdatedDrums, bIsUpdatedGuitar, bIsUpdatedBass;
            string strFilename = DTX.strファイル名の絶対パス + ".score.ini";
            CScoreIni ini = new CScoreIni(strFilename);
            if (!File.Exists(strFilename))
            {
                ini.stFile.Title = DTX.TITLE;
                ini.stFile.Name = DTX.strファイル名;
                ini.stFile.Hash = CScoreIni.tComputeFileMD5(DTX.strファイル名の絶対パス);
                for (int i = 0; i < 6; i++)
                {
                    ini.stSection[i].nPerfectRangeMs = nPerfectRangeMs;
                    ini.stSection[i].nGreatRangeMs = nGreatRangeMs;
                    ini.stSection[i].nGoodRangeMs = nGoodRangeMs;
                    ini.stSection[i].nPoorRangeMs = nPoorRangeMs;
                }
            }
            ini.stFile.BGMAdjust = DTX.nBGMAdjust;
            CScoreIni.tGetIsUpdateNeeded(out bIsUpdatedDrums, out bIsUpdatedGuitar, out bIsUpdatedBass);
            if (bIsUpdatedDrums || bIsUpdatedGuitar || bIsUpdatedBass)
            {
                if (bIsUpdatedDrums)
                {
                    ini.stFile.PlayCountDrums++;
                }
                if (bIsUpdatedGuitar)
                {
                    ini.stFile.PlayCountGuitar++;
                }
                if (bIsUpdatedBass)
                {
                    ini.stFile.PlayCountBass++;
                }
                ini.tAddHistory(str新ヒストリ行);
                if (!bCompactMode)
                {
                    stageSongSelection.rSelectedScore.SongInformation.NbPerformances.Drums = ini.stFile.PlayCountDrums;
                    stageSongSelection.rSelectedScore.SongInformation.NbPerformances.Guitar = ini.stFile.PlayCountGuitar;
                    stageSongSelection.rSelectedScore.SongInformation.NbPerformances.Bass = ini.stFile.PlayCountBass;
                    for (int j = 0; j < ini.stFile.History.Length; j++)
                    {
                        stageSongSelection.rSelectedScore.SongInformation.PerformanceHistory[j] = ini.stFile.History[j];
                    }
                }
            }
            if (ConfigIni.bScoreIniを出力する)
            {
                ini.tExport(strFilename);
            }

            return ini;
        }
        private void tRunGarbageCollector()
        {
            //LOHに対するコンパクションを要求
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            //通常通り、LOHへのGCを抑制
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.Default;
        }
        private void tプラグイン検索と生成()
        {
            this.listPlugins = new List<STPlugin>();

            string strIPluginActivityの名前 = typeof(IPluginActivity).FullName;
            string strプラグインフォルダパス = strEXEのあるフォルダ + "Plugins\\";

            this.t指定フォルダ内でのプラグイン検索と生成(strプラグインフォルダパス, strIPluginActivityの名前);

            if (this.listPlugins.Count > 0)
                Trace.TraceInformation(this.listPlugins.Count + " 個のプラグインを読み込みました。");
        }
        #region [ Windowイベント処理 ]
        private void t指定フォルダ内でのプラグイン検索と生成(string strプラグインフォルダパス, string strプラグイン型名)
        {
            // 指定されたパスが存在しないとエラー
            if (!Directory.Exists(strプラグインフォルダパス))
            {
                Trace.TraceWarning("プラグインフォルダが存在しません。(" + strプラグインフォルダパス + ")");
                return;
            }

            // (1) すべての *.dll について…
            string[] strDLLs = System.IO.Directory.GetFiles(strプラグインフォルダパス, "*.dll");
            foreach (string dllName in strDLLs)
            {
                try
                {
                    // (1-1) dll をアセンブリとして読み込む。
                    System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom(dllName);

                    // (1-2) アセンブリ内のすべての型について、プラグインとして有効か調べる
                    foreach (Type t in asm.GetTypes())
                    {
                        //  (1-3) ↓クラスであり↓Publicであり↓抽象クラスでなく↓IPlugin型のインスタンスが作れる　型を持っていれば有効
                        if (t.IsClass && t.IsPublic && !t.IsAbstract && t.GetInterface(strプラグイン型名) != null)
                        {
                            // (1-4) クラス名からインスタンスを作成する
                            var st = new STPlugin()
                            {
                                plugin = (IPluginActivity)asm.CreateInstance(t.FullName),
                                strプラグインフォルダ = Path.GetDirectoryName(dllName),
                                strアセンブリ簡易名 = asm.GetName().Name,
                                Version = asm.GetName().Version,
                            };

                            // (1-5) プラグインリストへ登録
                            this.listPlugins.Add(st);
                            Trace.TraceInformation("プラグイン {0} ({1}, {2}, {3}) を読み込みました。", t.FullName, Path.GetFileName(dllName), st.strアセンブリ簡易名, st.Version.ToString());
                        }
                    }
                }
                catch
                {
                    Trace.TraceInformation(dllName + " からプラグインを生成することに失敗しました。スキップします。");
                }
            }

            // (2) サブフォルダがあれば再帰する
            string[] strDirs = Directory.GetDirectories(strプラグインフォルダパス, "*");
            foreach (string dir in strDirs)
                this.t指定フォルダ内でのプラグイン検索と生成(dir + "\\", strプラグイン型名);
        }
        //-----------------
        private void Window_ApplicationActivated(object sender, EventArgs e)
        {
            this.bApplicationActive = true;
        }
        private void Window_ApplicationDeactivated(object sender, EventArgs e)
        {
            this.bApplicationActive = false;
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Menu)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if ((e.KeyCode == Keys.Return) && e.Alt)
            {
                if (ConfigIni != null)
                {
                    ConfigIni.bウィンドウモード = !ConfigIni.bウィンドウモード;
                    this.tSwitchFullScreenMode();
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else
            {
                for (int i = 0; i < 0x10; i++)
                {
                    var captureCode = (SlimDX.DirectInput.Key)ConfigIni.KeyAssign.System[(int)EKeyConfigPad.Capture][i].Code;

                    if ((int)captureCode > 0 &&
                        e.KeyCode == DeviceConstantConverter.KeyToKeys(captureCode))
                    {
                        // Debug.WriteLine( "capture: " + string.Format( "{0:2x}", (int) e.KeyCode ) + " " + (int) e.KeyCode );
                        string strFullPath =
                           Path.Combine(CDTXMania.strEXEのあるフォルダ, "Capture_img");
                        strFullPath = Path.Combine(strFullPath, DateTime.Now.ToString("yyyyMMddHHmmss") + ".png");
                        SaveResultScreen(strFullPath);
                    }
                }
            }
        }
        private void Window_MouseUp(object sender, MouseEventArgs e)
        {
            mb = e.Button;
        }

        private void Window_MouseDoubleClick(object sender, MouseEventArgs e)	// #23510 2010.11.13 yyagi: to go full screen mode
        {
            if (mb.Equals(MouseButtons.Left) && ConfigIni.bIsAllowedDoubleClickFullscreen)	// #26752 2011.11.27 yyagi
            {
                ConfigIni.bウィンドウモード = false;
                this.tSwitchFullScreenMode();
            }
        }
        private void Window_ResizeEnd(object sender, EventArgs e)				// #23510 2010.11.20 yyagi: to get resized window size
        {
            if (ConfigIni.bウィンドウモード)
            {
                ConfigIni.n初期ウィンドウ開始位置X = base.Window.Location.X;	// #30675 2013.02.04 ikanick add
                ConfigIni.n初期ウィンドウ開始位置Y = base.Window.Location.Y;	//
            }

            ConfigIni.nウインドウwidth = (ConfigIni.bウィンドウモード) ? base.Window.ClientSize.Width : currentClientSize.Width;	// #23510 2010.10.31 yyagi add
            ConfigIni.nウインドウheight = (ConfigIni.bウィンドウモード) ? base.Window.ClientSize.Height : currentClientSize.Height;
        }
        #endregion

        //internal sealed class GCBeep	// GC発生の度にbeep
        //{
        //    ~GCBeep()
        //    {
        //        Console.Beep();
        //        if ( !AppDomain.CurrentDomain.IsFinalizingForUnload()
        //            && !Environment.HasShutdownStarted )
        //        {
        //            new GCBeep();
        //        }
        //    }
        //}

        //-----------------
        #endregion
    }
}