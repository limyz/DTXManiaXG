using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Threading;
using SharpDX;
using FDK;

using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;
using RectangleF = System.Drawing.RectangleF;
using Point = System.Drawing.Point;

namespace DTXMania
{
    internal class CActConfigList : CActivity
    {
        // プロパティ

        public bool bIsKeyAssignSelected		// #24525 2011.3.15 yyagi
        {
            get
            {
                EMenuType e = this.eMenuType;
                if (e == EMenuType.KeyAssignBass || e == EMenuType.KeyAssignDrums ||
                    e == EMenuType.KeyAssignGuitar || e == EMenuType.KeyAssignSystem)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool bIsFocusingParameter		// #32059 2013.9.17 yyagi
        {
            get
            {
                return bFocusIsOnElementValue;
            }
        }
        
        //Keep these temporarily
        private CItemBase iSystemReturnToMenu;
        private CItemBase iDrumsReturnToMenu;
        private CItemBase iGuitarReturnToMenu;
        private CItemBase iBassReturnToMenu;
        
        public bool b現在選択されている項目はReturnToMenuである
        {
            get
            {
                CItemBase currentItem = this.listItems[this.nCurrentSelection];
                if (currentItem == this.iSystemReturnToMenu || currentItem == this.iDrumsReturnToMenu ||
                    currentItem == this.iGuitarReturnToMenu || currentItem == this.iBassReturnToMenu)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public CItemBase ibCurrentSelection
        {
            get
            {
                return this.listItems[this.nCurrentSelection];
            }
        }
        public int nCurrentSelection;


        // メソッド
        #region [ tSetupItemList_System() ]
        public void tSetupItemList_System()
        {
            this.tRecordToConfigIni();
            this.listItems.Clear();

            // #27029 2012.1.5 from: 説明文は最大9行→13行に変更。
            //横は13文字が目安。

            this.iSystemReturnToMenu = new CItemBase("<< ReturnTo Menu", CItemBase.EPanelType.Other,
                "左側のメニューに戻ります。",
                "Return to left menu.");
            this.listItems.Add(this.iSystemReturnToMenu);

            var iSystemReloadDTX = new CItemBase("Reload Songs", CItemBase.EPanelType.Normal,
                "曲データの一覧情報を\n"+
                "取得し直します。",
                "Clear song list cache and fully reload song data from disk.");
            iSystemReloadDTX.action = () =>
            {
                if (CDTXMania.EnumSongs.IsEnumerating)
                {
                    // Debug.WriteLine( "バックグラウンドでEnumeratingSongs中だったので、一旦中断します。" );
                    CDTXMania.EnumSongs.Abort();
                    CDTXMania.actEnumSongs.OnDeactivate();
                }

                CDTXMania.EnumSongs.StartEnumFromDisk(false);
                CDTXMania.EnumSongs.ChangeEnumeratePriority(ThreadPriority.Normal);
                CDTXMania.actEnumSongs.bコマンドでの曲データ取得 = true;
                CDTXMania.actEnumSongs.OnActivate();
            };
            listItems.Add(iSystemReloadDTX);

            var iSystemFastReloadDTX = new CItemBase("Fast Reload", CItemBase.EPanelType.Normal,
                "曲データの一覧情報を\n" +
                "取得し直します。",
                "Detect changes in DTX Data folder from song list cache and load these changes only.\nWARNING: This feature is experimental and may corrupt the song list cache. Select Reload Songs if something goes wrong.");
            iSystemFastReloadDTX.action = () =>
            {
                if (CDTXMania.EnumSongs.IsEnumerating)
                {
                    // Debug.WriteLine( "バックグラウンドでEnumeratingSongs中だったので、一旦中断します。" );
                    CDTXMania.EnumSongs.Abort();
                    CDTXMania.actEnumSongs.OnDeactivate();
                }

                CDTXMania.EnumSongs.StartEnumFromDisk(true);
                CDTXMania.EnumSongs.ChangeEnumeratePriority(ThreadPriority.Normal);
                CDTXMania.actEnumSongs.bコマンドでの曲データ取得 = true;
                CDTXMania.actEnumSongs.OnActivate();
            };
            this.listItems.Add(iSystemFastReloadDTX);

            var iSystemImportConfig = new CItemBase("Import Config", CItemBase.EPanelType.Normal,
                "config.iniファイルから設定\n" +
                "を再読み込みする。",
                "Import and apply settings from an external config.ini file.\nNOTE: Certain configurations such as Window Size and Position require restart of the application to take effect.");
            iSystemImportConfig.action = () =>
            {
                //Import Config                 
                var fileContent = string.Empty;
                var filePath = string.Empty;

                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = ".\\";
                    openFileDialog.FileName = "config.ini";
                    openFileDialog.Filter = "ini files (*.ini)|*.ini";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        filePath = openFileDialog.FileName;

                        Trace.TraceInformation("Selected File to import: " + filePath);
                        try
                        {
                            CConfigIni newConfig = new CConfigIni(filePath);
                            CDTXMania.ConfigIni = newConfig;
                            //Update the display values in config page to ensure UI is in-sync
                            this.tUpdateDisplayValuesFromConfigIni();
                            //Update Toast Message
                            string fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                            this.tUpdateToastMessage(string.Format("Imported {0} successfully.", fileName));
                            this.ctToastMessageCounter.tStart(0, 1, 10000, CDTXMania.Timer);
                        }
                        catch (Exception)
                        {
                            Trace.TraceError("Fail to import config file");
                            this.tUpdateToastMessage("Error importing selected file.");
                            this.ctToastMessageCounter.tStart(0, 1, 10000, CDTXMania.Timer);
                        }
                    }
                    else
                    {
                        Trace.TraceInformation("Cancel import of config");
                    }
                }
            };
            this.listItems.Add(iSystemImportConfig);

            var iSystemExportConfig = new CItemBase("Export Config", CItemBase.EPanelType.Normal,
                "config.iniファイルから設定\n" +
                "を再読み込みする。",
                "Export current settings to an external .ini file");
            iSystemExportConfig.action = () =>
            {
                //Export Config                    
                var fileContent = string.Empty;
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.InitialDirectory = ".\\";
                    saveFileDialog.FileName = "config.ini";
                    saveFileDialog.Filter = "ini files (*.ini)|*.ini";
                    saveFileDialog.FilterIndex = 2;
                    saveFileDialog.RestoreDirectory = true;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        string filePath = saveFileDialog.FileName;
                        Trace.TraceInformation("Selected File to export: " + filePath);
                        //Ensure changes are recorded to config.ini internally before export
                        this.tRecordToConfigIni();
                        CDTXMania.ConfigIni.tWrite(filePath); // CONFIGだけ
                        //Update Toast Message
                        string fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                        this.tUpdateToastMessage(string.Format("Configurations exported to {0}.", fileName));
                        this.ctToastMessageCounter.tStart(0, 1, 10000, CDTXMania.Timer);
                    }
                    else
                    {
                        Trace.TraceInformation("Cancel export of config");
                    }
                }
            };
            this.listItems.Add(iSystemExportConfig);

            int nDGmode = (CDTXMania.ConfigIni.bGuitarEnabled ? 1 : 1) + (CDTXMania.ConfigIni.bDrumsEnabled ? 0 : 1) - 1;
            this.iSystemGRmode = new CItemList("Drums & GR ", CItemBase.EPanelType.Normal, nDGmode,
                "使用楽器の選択：\nDrOnly: ドラムのみ有効にします。\nGROnly: ギター/ベースのみの専用画面を\n用います。",
                "Instrument selection:\nDrOnly: Activate Drums screen.\nGROnly: Activate single screen for Guitar and Bass.\n",
                new string[] { "DrOnly", "GROnly" });
            this.iSystemGRmode.BindConfig(
                () => this.iSystemGRmode.n現在選択されている項目番号 = nDGmode, 
                () => { } );
            this.listItems.Add(this.iSystemGRmode);
            
            var iSystemRisky = new CItemInteger("Risky", 0, 10, CDTXMania.ConfigIni.nRisky,
                "設定した回数分\n" +
                "ミスをすると、強制的に\n"+
                "STAGE FAILEDになります。",
                "Risky mode:\nNumber of mistakes (Poor/Miss) before getting STAGE FAILED.\n"+
                "Set 0 to disable Risky mode.");
            iSystemRisky.BindConfig(
                () => iSystemRisky.nCurrentValue = CDTXMania.ConfigIni.nRisky,
                () => CDTXMania.ConfigIni.nRisky = iSystemRisky.nCurrentValue);
            this.listItems.Add(iSystemRisky);

            var iSystemMovieMode = new CItemList("Movie Mode", CItemBase.EPanelType.Normal, CDTXMania.ConfigIni.nMovieMode,
                "Movie Mode:\n0 = 非表示\n1 = 全画面\n2 = ウインドウモード\n3 = 全画面&ウインドウ\n演奏中にF5キーで切り替え。",
                "Movie Mode:\n0 = Hide\n1 = Full screen\n2 = Window mode\n3 = Both Full screen and window\nUse F5 to switch during game.",
                new string[] { "Off", "Full Screen", "Window Mode", "Both" });
            iSystemMovieMode.BindConfig(
                () => iSystemMovieMode.n現在選択されている項目番号 = CDTXMania.ConfigIni.nMovieMode,
                () => CDTXMania.ConfigIni.nMovieMode = iSystemMovieMode.n現在選択されている項目番号);
            this.listItems.Add(iSystemMovieMode);

            var iSystemMovieAlpha = new CItemList("LaneAlpha", CItemBase.EPanelType.Normal, CDTXMania.ConfigIni.nMovieAlpha,
                "レーンの透明度を指定します。\n0% が完全不透明で、\n100% が完全透明となります。",
                "Degree of transparency for Movie.\n\n0%=No transparency,\n100%=Completely transparent",
                new string[] { "0%", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%" });
            iSystemMovieAlpha.BindConfig(
                () => iSystemMovieAlpha.n現在選択されている項目番号 = CDTXMania.ConfigIni.nMovieAlpha,
                () => CDTXMania.ConfigIni.nMovieAlpha = iSystemMovieAlpha.n現在選択されている項目番号);
            this.listItems.Add(iSystemMovieAlpha);

            this.iCommonPlaySpeed = new CItemInteger("PlaySpeed", CConstants.PLAYSPEED_MIN, CConstants.PLAYSPEED_MAX, CDTXMania.ConfigIni.nPlaySpeed,
                "曲の演奏速度を、速くしたり\n"+
                "遅くしたりすることができます。\n"+
                "※一部のサウンドカードでは、\n"+
                "正しく再生できない可能性が\n"+
                "あります。）",
                "Change the song speed.\nFor example, you can play in half speed by setting PlaySpeed = 0.500 for practice.\nNote: It also changes the song's pitch.");
            this.iCommonPlaySpeed.BindConfig(
                () => this.iCommonPlaySpeed.nCurrentValue = CDTXMania.ConfigIni.nPlaySpeed,
                () => CDTXMania.ConfigIni.nPlaySpeed = this.iCommonPlaySpeed.nCurrentValue);
            this.listItems.Add(this.iCommonPlaySpeed);

            this.iSystemTimeStretch = new CItemToggle("TimeStretch", CDTXMania.ConfigIni.bTimeStretch,
                "演奏速度の変更方式:\n" +
                "ONにすると、\n"+
                "演奏速度の変更を、\n" +
                "周波数変更ではなく\n" +
                "タイムストレッチで行います。",
                "PlaySpeed mode:\n" +
                "Turn ON to use time stretch instead of frequency change.");
            this.iSystemTimeStretch.BindConfig(
                () => this.iSystemTimeStretch.bON = CDTXMania.ConfigIni.bTimeStretch,
                () => CDTXMania.ConfigIni.bTimeStretch = this.iSystemTimeStretch.bON);
            this.listItems.Add(this.iSystemTimeStretch);
            
            var iSystemSkillMode = new CItemList("SkillMode", CItemBase.EPanelType.Normal, CDTXMania.ConfigIni.nSkillMode,
                "達成率、スコアの計算方法を変更します。\n" +
                "CLASSIC:V6までのスコア計算とV8までの\n" +
                "ランク計算です。\n" +
                "XG:XGシリーズの達成率計算とV7以降の\n" +
                "スコア計算です。",
                "Skill rate and score calculation method\nCLASSIC: Pre-V6 score calculation and pre-V8 rank calculation\nXG: Current score and rank calculation",
                new string[] { "CLASSIC", "XG" });
            iSystemSkillMode.BindConfig(
                () => iSystemSkillMode.n現在選択されている項目番号 = CDTXMania.ConfigIni.nSkillMode,
                () => CDTXMania.ConfigIni.nSkillMode = iSystemSkillMode.n現在選択されている項目番号);
            this.listItems.Add(iSystemSkillMode);

            var iSystemClassicNotes = new CItemToggle("CLASSIC Notes", CDTXMania.ConfigIni.bCLASSIC譜面判別を有効にする,
                "CLASSIC譜面の判別の有無を設定します。\n",
                "Use CLASSIC score calculation when a classic song is detected.\n");
            iSystemClassicNotes.BindConfig(
                () => iSystemClassicNotes.bON = CDTXMania.ConfigIni.bCLASSIC譜面判別を有効にする,
                () => CDTXMania.ConfigIni.bCLASSIC譜面判別を有効にする = iSystemClassicNotes.bON);
            this.listItems.Add(iSystemClassicNotes);

            var iSystemFullscreen = new CItemToggle("Fullscreen", CDTXMania.ConfigIni.bFullScreenMode,
                "画面モード設定：\n ON で全画面モード、\n OFF でウィンドウモードになります。",
                "Fullscreen mode or window mode.");
            iSystemFullscreen.action = () => CDTXMania.app.b次のタイミングで全画面_ウィンドウ切り替えを行う = true;
            iSystemFullscreen.BindConfig(
                () =>
                {
                    if (iSystemFullscreen.bON != CDTXMania.ConfigIni.bFullScreenMode)
                    {
                        //NOTE: The assignment is done in reverse because ConfigIni.bFullScreenMode will be toggled by the Draw method once the update flag is set to true
                        CDTXMania.ConfigIni.bFullScreenMode = iSystemFullscreen.bON;
                        CDTXMania.app.b次のタイミングで全画面_ウィンドウ切り替えを行う = true;
                        //Since actual value has changed, the UI should also reflect this
                        iSystemFullscreen.bON = !iSystemFullscreen.bON;
                    }
                },
                () => iSystemFullscreen.bON = CDTXMania.ConfigIni.bFullScreenMode);
            
            this.listItems.Add(iSystemFullscreen);
            
            var iSystemStageFailed = new CItemToggle("StageFailed", CDTXMania.ConfigIni.bSTAGEFAILEDEnabled,
                "ONにするとゲージが\n" +
                "なくなった時にSTAGE FAILED" +
                "となり演奏が中断されます。",
                "Turn OFF if you don't want to encounter STAGE FAILED.");
            iSystemStageFailed.BindConfig(
                () => iSystemStageFailed.bON = CDTXMania.ConfigIni.bSTAGEFAILEDEnabled,
                () => CDTXMania.ConfigIni.bSTAGEFAILEDEnabled = iSystemStageFailed.bON);
            this.listItems.Add(iSystemStageFailed);

            var iSystemRandomFromSubBox = new CItemToggle("RandSubBox", CDTXMania.ConfigIni.bランダムセレクトで子BOXを検索対象とする,
                "子BOXをRANDOMの対象とする：\nON にすると、RANDOM SELECT 時に、\n子BOXも選択対象とします。",
                "Turn ON to use child BOX (subfolders) at RANDOM SELECT.");
            iSystemRandomFromSubBox.BindConfig(
                () => iSystemRandomFromSubBox.bON = CDTXMania.ConfigIni.bランダムセレクトで子BOXを検索対象とする,
                () => CDTXMania.ConfigIni.bランダムセレクトで子BOXを検索対象とする = iSystemRandomFromSubBox.bON);
            this.listItems.Add(iSystemRandomFromSubBox);
            
            var iSystemAdjustWaves = new CItemToggle("AdjustWaves", CDTXMania.ConfigIni.bWave再生位置自動調整機能有効,
                "サウンド再生位置自動補正：\n" +
                "ハードウェアやOSに起因する\n" +
                "サウンドのずれを補正します。\n" +
                "再生時間の長い曲で\n"+
                "効果があります。\n" +
                "※DirectSound使用時のみ有効です。",
                "Automatic wave playing position adjustment feature. When turned on, decreases the lag coming from the difference of hardware/OS.\n" +
                "Usually, you should turn it ON.\n"+
                "Note: This setting is effective only when DirectSound is used.");
            iSystemAdjustWaves.BindConfig(
                () => iSystemAdjustWaves.bON = CDTXMania.ConfigIni.bWave再生位置自動調整機能有効,
                () => CDTXMania.ConfigIni.bWave再生位置自動調整機能有効 = iSystemAdjustWaves.bON);
            this.listItems.Add(iSystemAdjustWaves);

            var iSystemVSyncWait = new CItemToggle("VSyncWait", CDTXMania.ConfigIni.bVerticalSyncWait,
                "垂直帰線同期：\n" +
                "画面の描画をディスプレイの\n" +
                "垂直帰線中に行なう場合には\n" +
                "ONを指定します。\n" + 
                "ONにすると、ガタつきのない\n" +
                "滑らかな画面描画が実現されます。",
                "Turn ON to wait VSync (Vertical Synchronizing signal) at every drawing (so FPS becomes 60)\nIf you have enough CPU/GPU power, the scrolling would become smooth.");
            iSystemVSyncWait.action = () =>
            {
                CDTXMania.ConfigIni.bVerticalSyncWait = iSystemVSyncWait.bON;
                CDTXMania.app.b次のタイミングで垂直帰線同期切り替えを行う = true;
            };
            iSystemVSyncWait.BindConfig(
                () =>
                {
                    if (iSystemVSyncWait.bON != CDTXMania.ConfigIni.bVerticalSyncWait) {
                        CDTXMania.app.b次のタイミングで垂直帰線同期切り替えを行う = true;
                    }            
                    iSystemVSyncWait.bON = CDTXMania.ConfigIni.bVerticalSyncWait;
                }, 
                () => CDTXMania.ConfigIni.bVerticalSyncWait = iSystemVSyncWait.bON);
            this.listItems.Add(iSystemVSyncWait);

            var iSystemAVI = new CItemToggle("AVI", CDTXMania.ConfigIni.bAVIEnabled,
                "AVIの使用：\n動画(AVI)を再生可能にする場合に\nON にします。AVI の再生には、それ\nなりのマシンパワーが必要とされます。",
                "Turn ON to enable video (AVI) playback.\nThis requires some processing power.");
            iSystemAVI.BindConfig(
                () => iSystemAVI.bON = CDTXMania.ConfigIni.bAVIEnabled,
                () => CDTXMania.ConfigIni.bAVIEnabled = iSystemAVI.bON);
            this.listItems.Add(iSystemAVI);

            var iSystemBGA = new CItemToggle("BGA", CDTXMania.ConfigIni.bBGAEnabled,
                "BGAの使用：\n画像(BGA)を表示可能にする場合に\nON にします。BGA の再生には、それ\nなりのマシンパワーが必要とされます。",
                "Turn ON to enable background animation (BGA) playback.\nThis requires some processing power.");
            iSystemBGA.BindConfig(
                () => iSystemBGA.bON = CDTXMania.ConfigIni.bBGAEnabled,
                () => CDTXMania.ConfigIni.bBGAEnabled = iSystemBGA.bON);
            this.listItems.Add(iSystemBGA);
            
            var iSystemPreviewSoundWait = new CItemInteger("PreSoundWait", 0, 0x2710, CDTXMania.ConfigIni.n曲が選択されてからプレビュー音が鳴るまでのウェイトms,
                "カーソルが合わされてから\n"+
                "プレビュー音が鳴り始める\n"+
                "までの時間を指定します。\n"+
                "0～10000[ms]が指定可能です。",
                "Delay time (ms) to start playing preview sound in song selection screen.\nYou can specify from 0ms to 10000ms.");
            iSystemPreviewSoundWait.BindConfig(
                () => iSystemPreviewSoundWait.nCurrentValue = CDTXMania.ConfigIni.n曲が選択されてからプレビュー音が鳴るまでのウェイトms,
                () => CDTXMania.ConfigIni.n曲が選択されてからプレビュー音が鳴るまでのウェイトms = iSystemPreviewSoundWait.nCurrentValue);
            this.listItems.Add(iSystemPreviewSoundWait);

            var iSystemPreviewImageWait = new CItemInteger("PreImageWait", 0, 0x2710, CDTXMania.ConfigIni.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms,
                "カーソルが合わされてから\n"+
                "プレビュー画像が表示\n"+
                "されるまでの時間を\n"+
                "指定します。\n"+
                "0～10000[ms]が指定可能です。",
                "Delay time (ms) to show preview image in song selection screen.\nYou can specify from 0ms to 10000ms.");
            iSystemPreviewImageWait.BindConfig(
                () => iSystemPreviewImageWait.nCurrentValue = CDTXMania.ConfigIni.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms,
                () => CDTXMania.ConfigIni.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms = iSystemPreviewImageWait.nCurrentValue);
            this.listItems.Add(iSystemPreviewImageWait);
            
            var iSystemDebugInfo = new CItemToggle("Debug Info", CDTXMania.ConfigIni.b演奏情報を表示する,
                "演奏情報の表示：\n" +
                "演奏中、BGA領域の下部に\n" +
                "演奏情報を表示します。\n" +
                "また、小節線の横に\n"+
                "小節番号が表示されるように\n"+
                "なります。",
                "Show song information on playing BGA area (FPS, BPM, total time etc)\nYou can turn ON/OFF the indications by pushing [Del] while playing drums, guitar or bass.");
            iSystemDebugInfo.BindConfig(
                () => iSystemDebugInfo.bON = CDTXMania.ConfigIni.b演奏情報を表示する,
                () => CDTXMania.ConfigIni.b演奏情報を表示する = iSystemDebugInfo.bON);
            this.listItems.Add(iSystemDebugInfo);
            
            var iSystemBGAlpha = new CItemInteger("BG Alpha", 0, 0xff, CDTXMania.ConfigIni.nBackgroundTransparency,
                "背景画像をDTXManiaの\n"+
                "背景画像と合成する際の\n"+
                "背景画像の透明度を\n"+
                "指定します。\n"+
                "255に近いほど、不透明\n"+
                "になります。",
                "Degree of transparency for background wallpaper\n\n0=Completely transparent,\n255=No transparency");
            iSystemBGAlpha.BindConfig(
                () => iSystemBGAlpha.nCurrentValue = CDTXMania.ConfigIni.nBackgroundTransparency,
                () => CDTXMania.ConfigIni.nBackgroundTransparency = iSystemBGAlpha.nCurrentValue);
            this.listItems.Add(iSystemBGAlpha);

            var iSystemBGMSound = new CItemToggle("BGM Sound", CDTXMania.ConfigIni.bBGM音を発声する,
                "OFFにするとBGMを再生しません。",
                "Turn OFF if you don't want to play the song music (BGM).");
            iSystemBGMSound.BindConfig(
                () => iSystemBGMSound.bON = CDTXMania.ConfigIni.bBGM音を発声する,
                () => CDTXMania.ConfigIni.bBGM音を発声する = iSystemBGMSound.bON);
            this.listItems.Add(iSystemBGMSound);
            
            var iSystemAudienceSound = new CItemToggle("Audience", CDTXMania.ConfigIni.b歓声を発声する,
                "OFFにすると歓声を再生しません。",
                "Turn ON if you want to be cheered at the end of fill-in zone or not.");
            iSystemAudienceSound.BindConfig(
                () => iSystemAudienceSound.bON = CDTXMania.ConfigIni.b歓声を発声する,
                () => CDTXMania.ConfigIni.b歓声を発声する = iSystemAudienceSound.bON);
            this.listItems.Add(iSystemAudienceSound);

            var iSystemDamageLevel = new CItemList("DamageLevel", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eDamageLevel,
                "Miss時のゲージの減少度合い\n"+
                "を指定します。\n"+
                "Risky時は無効となります",
                "Degree of decrease of the damage gauge when missing chips.\nThis setting is ignored when Risky >= 1.",
                new string[] { "Small", "Normal", "Large" });
            iSystemDamageLevel.BindConfig(
                () => iSystemDamageLevel.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eDamageLevel,
                () => CDTXMania.ConfigIni.eDamageLevel = (EDamageLevel)iSystemDamageLevel.n現在選択されている項目番号);
            this.listItems.Add(iSystemDamageLevel);

            var iSystemSaveScore = new CItemToggle("SaveScore", CDTXMania.ConfigIni.bScoreIniを出力する,
                "演奏記録の保存：\n"+
                "ONで演奏記録を.score.iniに\n"+
                "保存します。\n",
                "Turn ON to save high scores/skills.\nTurn OFF in case your song data are on read-only media.\n"+
                "Note that the score files also contain 'BGM Adjust' parameter, so turn ON to keep adjustment.");
            iSystemSaveScore.BindConfig(
                () => iSystemSaveScore.bON = CDTXMania.ConfigIni.bScoreIniを出力する,
                () => CDTXMania.ConfigIni.bScoreIniを出力する = iSystemSaveScore.bON);
            this.listItems.Add(iSystemSaveScore);
            
            var iSystemChipVolume = new CItemInteger("ChipVolume", 0, 100, CDTXMania.ConfigIni.n手動再生音量,
                "打音の音量：\n入力に反応して再生される\nチップの音量を指定します。\n0 ～ 100 % の値が指定可能\nです。\n",
                "Volume for chips you hit.\nYou can specify from 0 to 100%.");
            iSystemChipVolume.BindConfig(
                () => iSystemChipVolume.nCurrentValue = CDTXMania.ConfigIni.n手動再生音量,
                () => CDTXMania.ConfigIni.n手動再生音量 = iSystemChipVolume.nCurrentValue);
            this.listItems.Add(iSystemChipVolume);

            var iSystemAutoChipVolume = new CItemInteger("AutoVolume", 0, 100, CDTXMania.ConfigIni.n自動再生音量,
                "自動再生音の音量：\n自動的に再生される\nチップの音量を指定します。\n0 ～ 100 % の値が指定可能\nです。\n",
                "Volume for AUTO chips.\nYou can specify from 0 to 100%.");
            iSystemAutoChipVolume.BindConfig(
                () => iSystemAutoChipVolume.nCurrentValue = CDTXMania.ConfigIni.n自動再生音量,
                () => CDTXMania.ConfigIni.n自動再生音量 = iSystemAutoChipVolume.nCurrentValue);
            this.listItems.Add(iSystemAutoChipVolume);

            /*
            var iSystemStoicMode = new CItemToggle("StoicMode", CDTXMania.ConfigIni.bストイックモード,
                "ストイック（禁欲）モード：\n" +
                "以下をまとめて表示ON/OFFします。\n" +
                "_プレビュー画像/動画\n" +
                "_リザルト画像/動画\n" +
                "_NowLoading画像\n" +
                "_演奏画面の背景画像\n" +
                "_BGA 画像 / AVI 動画\n" +
                "_グラフ画像\n",
                "Turn ON to disable drawing\n * preview image / movie\n * result image / movie\n * nowloading image\n * wallpaper (in playing screen)\n * BGA / AVI (in playing screen)");
            this.listItems.Add(this.iSystemStoicMode);
            */
            
            var iSystemStageEffect = new CItemToggle("StageEffect", CDTXMania.ConfigIni.DisplayBonusEffects,
                "OFFにすると、\n" +
                "ゲーム中の背景演出が\n" +
                "非表示になります。",
                "When turned off, background stage effects are disabled");
            iSystemStageEffect.BindConfig(
                () => iSystemStageEffect.bON = CDTXMania.ConfigIni.DisplayBonusEffects,
                () => CDTXMania.ConfigIni.DisplayBonusEffects = iSystemStageEffect.bON);
            this.listItems.Add(iSystemStageEffect);
            
            var iSystemShowLag = new CItemList("ShowLagTime", CItemBase.EPanelType.Normal, CDTXMania.ConfigIni.nShowLagType,
                "ズレ時間表示：\n"+
                "ジャストタイミングからの\n"+
                "ズレ時間(ms)を表示します。\n"+
                "OFF: 表示しません。\n"+
                "ON: ズレ時間を表示します。\n"+
                "GREAT-: PERFECT以外の時\n"+
                "のみ表示します。",
                "Display the lag from ideal hit time (ms)\nOFF: Don't show.\nON: Show.\nGREAT-: Show except for perfect chips.",
                new string[] { "OFF", "ON", "GREAT-" });
            iSystemShowLag.BindConfig(
                () => iSystemShowLag.n現在選択されている項目番号 = CDTXMania.ConfigIni.nShowLagType,
                () => CDTXMania.ConfigIni.nShowLagType = iSystemShowLag.n現在選択されている項目番号);
            this.listItems.Add(iSystemShowLag);

            var iSystemShowLagColor = new CItemList("ShowLagTimeColor", CItemBase.EPanelType.Normal, CDTXMania.ConfigIni.nShowLagTypeColor,
                "ズレ時間表示の表示色変更：\n  TYPE-A: 早ズレを青、遅ズレを赤で表示します。\n  TYPE-B: 早ズレを赤、遅ズレを青で表示します。",
                "Change color of lag time display：\nTYPE-A: early notes in blue and late notes in red.\nTYPE-B: early notes in red and late notes in blue.",
				new string[] { "TYPE-A", "TYPE-B" } );
            iSystemShowLagColor.BindConfig(
                () => iSystemShowLagColor.n現在選択されている項目番号 = CDTXMania.ConfigIni.nShowLagTypeColor,
                () => CDTXMania.ConfigIni.nShowLagTypeColor = iSystemShowLagColor.n現在選択されている項目番号);
			this.listItems.Add(iSystemShowLagColor);

            var iSystemShowLagHitCount = new CItemToggle("ShowLagHitCount", CDTXMania.ConfigIni.bShowLagHitCount,
                "ズレヒット数表示:\n演奏と結果画面に早ズレ、遅ズレヒット数で表示する場合はONにします", //fisyher: Constructed using DeepL, feedback welcomed to improved accuracy
                "ShowLagHitCount:\nTurn ON to display Early/Late Hit Counters in Performance and Result Screen.");
            iSystemShowLagHitCount.BindConfig(
                () => iSystemShowLagHitCount.bON = CDTXMania.ConfigIni.bShowLagHitCount,
                () => CDTXMania.ConfigIni.bShowLagHitCount = iSystemShowLagHitCount.bON);
            this.listItems.Add(iSystemShowLagHitCount);

            var iSystemAutoResultCapture = new CItemToggle("AutoSaveResult", CDTXMania.ConfigIni.bIsAutoResultCapture,
                "ONにすると、NewRecord時に\n"+
                "自動でリザルト画像を\n"+
                "曲データと同じフォルダに\n"+
                "保存します。",
                "AutoSaveResult:\nTurn ON to save your result screen image automatically when you get hiscore/hiskill.");
            iSystemAutoResultCapture.BindConfig(
                () => iSystemAutoResultCapture.bON = CDTXMania.ConfigIni.bIsAutoResultCapture,
                () => CDTXMania.ConfigIni.bIsAutoResultCapture = iSystemAutoResultCapture.bON);
            this.listItems.Add(iSystemAutoResultCapture);

            var iSystemMusicNameDispDef = new CItemToggle("MusicNameDispDEF", CDTXMania.ConfigIni.b曲名表示をdefのものにする,
                "表示される曲名をdefのものにします。\n" +
                "ただし選曲画面の表示は、\n" +
                "defファイルの曲名が\n"+
                "優先されます。",
                "Display the music title from SET.def file");
            iSystemMusicNameDispDef.BindConfig(
                () => iSystemMusicNameDispDef.bON = CDTXMania.ConfigIni.b曲名表示をdefのものにする,
                () => CDTXMania.ConfigIni.b曲名表示をdefのものにする = iSystemMusicNameDispDef.bON);
            this.listItems.Add(iSystemMusicNameDispDef);
            
            var iAutoAddGage = new CItemToggle("AutoAddGage", CDTXMania.ConfigIni.bAutoAddGage,
                "ONの場合、AUTO判定も\n"+
                "ゲージに加算されます。\n",
                "If ON, will be added to the judgment also gauge AUTO.\n" +
                "");
            iAutoAddGage.BindConfig(
                () => iAutoAddGage.bON = CDTXMania.ConfigIni.bAutoAddGage,
                () => CDTXMania.ConfigIni.bAutoAddGage = iAutoAddGage.bON);
            this.listItems.Add(iAutoAddGage);
            
            var iSystemBufferedInput = new CItemToggle("BufferedInput", CDTXMania.ConfigIni.bバッファ入力を行う,
                "バッファ入力モード：\nON にすると、FPS を超える入力解像\n度を実現します。\nOFF にすると、入力解像度は FPS に\n等しくなります。",
                "Select joystick/keyboard/\nmouse input buffer mode.\nON to use buffer input. No lost/lags.\n"+
                "OFF to use realtime input. May cause lost/lags for input. Input frequency is synchronized with FPS.");
            iSystemBufferedInput.BindConfig(
                () => iSystemBufferedInput.bON = CDTXMania.ConfigIni.bバッファ入力を行う,
                () => CDTXMania.ConfigIni.bバッファ入力を行う = iSystemBufferedInput.bON);
            this.listItems.Add(iSystemBufferedInput);

            var iLogOutputLog = new CItemToggle("TraceLog", CDTXMania.ConfigIni.bOutputLogs,
                "Traceログ出力：\nDTXManiaLog.txt にログを出力します。\n変更した場合は、DTXMania の再起動\n後に有効となります。",
                "Turn ON to output debug logs to DTXManiaLog.txt file\nEffective after next DTXMania restart.");
            iLogOutputLog.BindConfig(
                () => iLogOutputLog.bON = CDTXMania.ConfigIni.bOutputLogs,
                () => CDTXMania.ConfigIni.bOutputLogs = iLogOutputLog.bON);
            this.listItems.Add(iLogOutputLog);
            
            // #24820 2013.1.3 yyagi
            this.iSystemSoundType = new CItemList("SoundType", CItemList.EPanelType.Normal, CDTXMania.ConfigIni.nSoundDeviceType,
                "サウンド出力方式を選択\n"+
                "します。\n" +
                "WASAPIはVista以降、\n"+
                "ASIOは対応機器でのみ使用可能です。\n" +
                "WASAPIかASIOを使うと、\n"+
                "遅延を少なくできます。\n",
                "DSound: Direct Sound\n" +
                "WASAPI: from Windows Vista\n" +
                "ASIO: with ASIO compatible devices only\n" +
                "Use WASAPI or ASIO to decrease the sound lag.\n" +
                "Note: Exit CONFIG to make the setting take effect.",
                new string[] { "DSound", "ASIO", "WASAPIExclusive", "WASAPIShared" });
            this.iSystemSoundType.BindConfig(
                () => this.iSystemSoundType.n現在選択されている項目番号 = CDTXMania.ConfigIni.nSoundDeviceType,
                () => CDTXMania.ConfigIni.nSoundDeviceType = this.iSystemSoundType.n現在選択されている項目番号);
            this.listItems.Add(this.iSystemSoundType);

            // #24820 2013.1.15 yyagi
            this.iSystemWASAPIBufferSizeMs = new CItemInteger("WASAPIBufSize", 0, 99999, CDTXMania.ConfigIni.nWASAPIBufferSizeMs,
                "WASAPI時のバッファサイズ:\n" +
                "0～99999msを指定できます。\n" +
                "0を指定すると、OSがサイズを\n" +
                "自動設定します。\n" +
                "値を小さくするほどラグが減少\n" +
                "しますが、音割れや異常を\n" +
                "引き起こす場合があります。\n",
                "Sound buffer size for WASAPI, from 0 to 99999ms.\n" +
                "Set 0 to use default system buffer size.\n" +
                "Small value reduces lag but may cause sound troubles.\n" +
                "Note: Exit CONFIG to make the setting take effect.");
            this.iSystemWASAPIBufferSizeMs.BindConfig(
                () => this.iSystemWASAPIBufferSizeMs.nCurrentValue = CDTXMania.ConfigIni.nWASAPIBufferSizeMs,
                () => CDTXMania.ConfigIni.nWASAPIBufferSizeMs = this.iSystemWASAPIBufferSizeMs.nCurrentValue);
            this.listItems.Add(this.iSystemWASAPIBufferSizeMs);

            // #24820 2013.1.17 yyagi
            string[] asiodevs = CEnumerateAllAsioDevices.GetAllASIODevices();
            this.iSystemASIODevice = new CItemList("ASIO device", CItemList.EPanelType.Normal, CDTXMania.ConfigIni.nASIODevice,
                "ASIOデバイス:\n" +
                "ASIO使用時の\n" +
                "サウンドデバイスを選択\n"+
                "します。\n",
                "ASIO Sound Device:\n" +
                "Select the sound device to use under ASIO mode.\n" +
                "\n" +
                "Note: Exit CONFIG to make the setting take effect.",
                asiodevs);
            this.iSystemASIODevice.BindConfig(
                () => this.iSystemASIODevice.n現在選択されている項目番号 = CDTXMania.ConfigIni.nASIODevice,
                () => CDTXMania.ConfigIni.nASIODevice = this.iSystemASIODevice.n現在選択されている項目番号);
            this.listItems.Add(iSystemASIODevice);
            // #24820 2013.1.3 yyagi

            /*
            var iSystemASIOBufferSizeMs = new CItemInteger("ASIOBuffSize", 0, 99999, CDTXMania.ConfigIni.nASIOBufferSizeMs,
                "ASIO使用時のバッファサイズ:\n" +
                "0～99999ms を指定可能です。\n" +
                "0を指定すると、サウンドデバイスに\n" +
                "指定されている設定値を使用します。\n" +
                "値を小さくするほど発音ラグが\n" +
                "減少しますが、音割れや異常動作を\n" +
                "引き起こす場合があります。\n"+
                "※ 設定はCONFIGURATION画面の\n" +
                "　終了時に有効になります。",
                "Sound buffer size for ASIO:\n" +
                "You can set from 0 to 99999ms.\n" +
                "Set 0 to use a default value already\n" +
                "specified to the sound device.\n" +
                "Smaller value makes smaller lag,\n" +
                "but it may cause sound troubles.\n" +
                "\n" +
                "Note: Exit CONFIGURATION to make\n" +
                " the setting take effect.");
            this.listItems.Add(this.iSystemASIOBufferSizeMs);
            */
            this.iSystemSoundTimerType = new CItemToggle("UseOSTimer", CDTXMania.ConfigIni.bUseOSTimer,
                "OSタイマーを使用するかどうか:\n" +
                "演奏タイマーとして、DTXMania独自のタイマーを使うか\n" +
                "OS標準のタイマーを使うかを選択します。\n" +
                "OS標準タイマーを使うとスクロールが滑らかに\n" +
                "なりますが、演奏で音ズレが発生することが\n" +
                "あります。\n" +
                "(そのためAdjustWavesの効果が適用されます。)\n" +
                "\n" +
                "この指定はWASAPI/ASIO使用時のみ有効です。\n",
                "Use OS Timer or not:\n" +
                "If this settings is ON, DTXMania uses OS Standard timer. It brings smooth scroll, but may cause some sound lag.\n" +
                "(so AdjustWaves is also avilable)\n" +
                "\n" +
                "If OFF, DTXMania uses its original timer and the effect is vice versa.\n" +
                "\n" +
                "This settings is avilable only when you use WASAPI/ASIO.\n"
            );
            this.iSystemSoundTimerType.BindConfig(
                () => this.iSystemSoundTimerType.bON = CDTXMania.ConfigIni.bUseOSTimer,
                () => CDTXMania.ConfigIni.bUseOSTimer = this.iSystemSoundTimerType.bON);
            this.listItems.Add(this.iSystemSoundTimerType);

            // #33700 2013.1.3 yyagi
            this.iSystemMasterVolume = new CItemInteger("MasterVolume", 0, 100, CDTXMania.ConfigIni.nMasterVolume,
                "マスターボリュームの設定:\n" +
                "全体の音量を設定します。\n" +
                "0が無音で、100が最大値です。\n" +
                "(WASAPI/ASIO時のみ有効です)",
                "Master Volume:\n" +
                "You can set 0 - 100.\n" +
                "\n" +
                "Note:\n" +
                "Only for WASAPI/ASIO mode.");
            this.iSystemMasterVolume.BindConfig(
                () => iSystemMasterVolume.nCurrentValue = CDTXMania.ConfigIni.nMasterVolume,
                () => CDTXMania.ConfigIni.nMasterVolume = iSystemMasterVolume.nCurrentValue);
            this.listItems.Add(iSystemMasterVolume);

            this.iSystemWASAPIEventDriven = new CItemToggle("WASAPIEventDriven", CDTXMania.ConfigIni.bEventDrivenWASAPI,
                "WASAPIをEvent Drivenモードで使用します。\n" +
                "これを使うと、サウンド出力の遅延をより小さくできますが、システム負荷は上昇します。",
                "Use WASAPI Event Driven mode.\n" +
                "It reduce sound output lag, but it also decreases system performance.");
            this.listItems.Add(this.iSystemWASAPIEventDriven);

            #region [ GDオプション ]
            
            var iSystemDifficulty = new CItemToggle("Difficulty", CDTXMania.ConfigIni.b難易度表示をXG表示にする,
                "選曲画面での難易度表示方法を変更します。\nON でXG風3ケタ、\nOFF で従来の2ケタ表示になります。",
                "Change difficulty display mode on song selection screen.\n"+
                "ON for XG-style 3-digit display\nOFF for classic 2-digit display.");
            iSystemDifficulty.BindConfig(
                () => iSystemDifficulty.bON = CDTXMania.ConfigIni.b難易度表示をXG表示にする,
                () => CDTXMania.ConfigIni.b難易度表示をXG表示にする = iSystemDifficulty.bON);
            this.listItems.Add(iSystemDifficulty);
            
            var iSystemShowScore = new CItemToggle("ShowScore", CDTXMania.ConfigIni.bShowScore,
                    "演奏中のスコアの表示の有無を設定します。",
                    "Display the score during the game.");
            iSystemShowScore.BindConfig(
                () => iSystemShowScore.bON = CDTXMania.ConfigIni.bShowScore,
                () => CDTXMania.ConfigIni.bShowScore = iSystemShowScore.bON);
            this.listItems.Add(iSystemShowScore);

            var iSystemShowMusicInfo = new CItemToggle("ShowMusicInfo", CDTXMania.ConfigIni.bShowMusicInfo,
                    "OFFにすると演奏中のジャケット、曲情報を\n表示しません。",
                    "When turned OFF, the cover and song information being played are not displayed.");
            iSystemShowMusicInfo.BindConfig(
                () => iSystemShowMusicInfo.bON = CDTXMania.ConfigIni.bShowMusicInfo,
                () => CDTXMania.ConfigIni.bShowMusicInfo = iSystemShowMusicInfo.bON);
            this.listItems.Add(iSystemShowMusicInfo);
             
            #endregion
            
            this.iSystemSkinSubfolder = new CItemList("Skin (General)", CItemBase.EPanelType.Normal, nSkinIndex,
                "スキン切替：スキンを切り替えます。\n" +
                "\n",
                "Choose skin",
                skinNames);
            this.iSystemSkinSubfolder.BindConfig(() =>
            {
                //Handle updating of CDTXMania.ConfigIni.strSystemSkinSubfolderFullName back to UI value
                int nSkinIndex = -1;
                for (int i = 0; i < skinSubFolders.Length; i++)
                {
                    if (skinSubFolders[i] == CDTXMania.ConfigIni.strSystemSkinSubfolderFullName) {
                        nSkinIndex = i;
                        break;
                    }
                }
                
                if (nSkinIndex != -1) {

                    this.iSystemSkinSubfolder.n現在選択されている項目番号 = nSkinIndex;
                    this.nSkinIndex = nSkinIndex;
                    CDTXMania.Skin.SetCurrentSkinSubfolderFullName(CDTXMania.ConfigIni.strSystemSkinSubfolderFullName, true);
                }
            },
            () => { });
            this.iSystemSkinSubfolder.action = tGenerateSkinSample;
            this.listItems.Add(this.iSystemSkinSubfolder);

            var iSystemUseBoxDefSkin = new CItemToggle("Skin (Box)", CDTXMania.ConfigIni.bUseBoxDefSkin,
                "Music boxスキンの利用：\n" +
                "特別なスキンが設定されたMusic box\n" +
                "に出入りしたときに、自動でスキンを\n" +
                "切り替えるかどうかを設定します。\n",
                "Box skin:\n" +
                "Automatically change skin as per box.def file.");
            iSystemUseBoxDefSkin.BindConfig(
                () => iSystemUseBoxDefSkin.bON = CDTXMania.ConfigIni.bUseBoxDefSkin,
                () => CDTXMania.ConfigIni.bUseBoxDefSkin = iSystemUseBoxDefSkin.bON);
            iSystemUseBoxDefSkin.action = () => CSkin.bUseBoxDefSkin = iSystemUseBoxDefSkin.bON;
            this.listItems.Add(iSystemUseBoxDefSkin);

            var iInfoType = new CItemList("InfoType", CItemBase.EPanelType.Normal, CDTXMania.ConfigIni.nInfoType,
                "Helpボタンを押した時に出る\n" +
                "情報表示を変更できます。\n" +
                "Type-A FPS、BGMアジャスト\n" +
                "などの情報が出ます。\n" +
                "Type-B 判定数などが出ます。\n",
                "Type-A: FPS, BGM adjustment are display\n" +
                "Type-B: Number of perfect/great etc. skill rate are displayed.",
                new string[] { "Type-A", "Type-B" });
            iInfoType.BindConfig(
                () => iInfoType.n現在選択されている項目番号 = CDTXMania.ConfigIni.nInfoType,
                () => CDTXMania.ConfigIni.nInfoType = iInfoType.n現在選択されている項目番号);
            this.listItems.Add(iInfoType);

            // #36372 2016.06.19 kairera0467
			var iSystemBGMAdjust = new CItemInteger( "BGMAdjust", -99, 99, CDTXMania.ConfigIni.nCommonBGMAdjustMs,
				"BGMの再生タイミングの微調整を行います。\n" +
				"-99 ～ 99ms まで指定可能です。\n" +
                "値を指定してください。\n",
				"Adjust the BGM play timing.\n" +
				"You can set from -99 to 0 ms.\n" );
            iSystemBGMAdjust.BindConfig(
                () => iSystemBGMAdjust.nCurrentValue = CDTXMania.ConfigIni.nCommonBGMAdjustMs,
                () => CDTXMania.ConfigIni.nCommonBGMAdjustMs = iSystemBGMAdjust.nCurrentValue);
			this.listItems.Add(iSystemBGMAdjust);

            var iSystemGoToKeyAssign = new CItemBase("System Keys", CItemBase.EPanelType.Normal,
                "システムのキー入力に関する項目を設定します。",
                "Settings for the system key/pad inputs.");
            iSystemGoToKeyAssign.action = tSetupItemList_KeyAssignSystem;
            this.listItems.Add(iSystemGoToKeyAssign);
            
            var iSystemMetronome = new CItemToggle("Metronome", CDTXMania.ConfigIni.bMetronome,
                "メトロノームを有効にします。", "Enable Metronome.");
            iSystemMetronome.BindConfig(
                () => iSystemMetronome.bON = CDTXMania.ConfigIni.bMetronome,
                () => CDTXMania.ConfigIni.bMetronome = iSystemMetronome.bON);
            this.listItems.Add(iSystemMetronome);

            var iSystemChipPlayTimeComputeMode = new CItemList("Chip Timing Mode", CItemList.EPanelType.Normal, CDTXMania.ConfigIni.nChipPlayTimeComputeMode,
                "発声時刻の計算方式を選択\n" +
                "します。\n" +
                "Original: 原発声時刻の計算方式\n" +
                "Accurate: BPM変更の時刻偏差修正",
                "Select Chip Timing Mode:\n" +
                "Original: Compatible with other DTXMania players\n" +
                "Accurate: Fixes time loss issue of BPM/Bar-Length Changes\n" +
                "NOTE: Only songs with many BPM/Bar-Length changes have observable time differences. Most songs are not affected by this option.",
                new string[] { "Original", "Accurate" });
            iSystemChipPlayTimeComputeMode.BindConfig(
                () => iSystemChipPlayTimeComputeMode.n現在選択されている項目番号 = CDTXMania.ConfigIni.nChipPlayTimeComputeMode,
                () => CDTXMania.ConfigIni.nChipPlayTimeComputeMode = iSystemChipPlayTimeComputeMode.n現在選択されている項目番号);
            this.listItems.Add(iSystemChipPlayTimeComputeMode);

            OnListMenuの初期化();
            this.nCurrentSelection = 0;
            this.eMenuType = EMenuType.System;
        }
        #endregion
        #region [ t項目リストの設定_Drums() ]
        public void tSetupItemList_Drums()
        {
            this.tRecordToConfigIni();
            this.listItems.Clear();
            
            this.iDrumsReturnToMenu = new CItemBase("<< Return To Menu", CItemBase.EPanelType.Other,
                "左側のメニューに戻ります。",
                "Return to left menu.");
            this.listItems.Add(this.iDrumsReturnToMenu);
            
            //----------AutoPlay----------
            var iDrumsAutoPlayAll = new CItemThreeState("AutoPlay (All)", CItemThreeState.E状態.不定,
                "全パッドの自動演奏のON/OFFを\n" +
                "まとめて切り替えます。",
                "Activate/deactivate Auto for all drum lanes at once.");
            this.listItems.Add(iDrumsAutoPlayAll);

            var iDrumsLeftCymbal = new CItemToggle("    LeftCymbal", CDTXMania.ConfigIni.bAutoPlay.LC,
                "左シンバルを自動で演奏します。",
                "Play Left Cymbal automatically.");
            iDrumsLeftCymbal.BindConfig(
                () => iDrumsLeftCymbal.bON = CDTXMania.ConfigIni.bAutoPlay.LC,
                () => CDTXMania.ConfigIni.bAutoPlay.LC = iDrumsLeftCymbal.bON);
            this.listItems.Add(iDrumsLeftCymbal);

            var iDrumsHiHat = new CItemToggle("    HiHat", CDTXMania.ConfigIni.bAutoPlay.HH,
                "ハイハットを自動で演奏します。\n" +
                "（クローズ、オープンとも）",
                "Play HiHat automatically.\n" +
                "(It affects both HH-close and HH-open)");
            iDrumsHiHat.BindConfig(
                () => iDrumsHiHat.bON = CDTXMania.ConfigIni.bAutoPlay.HH,
                () => CDTXMania.ConfigIni.bAutoPlay.HH = iDrumsHiHat.bON);
            this.listItems.Add(iDrumsHiHat);

            var iDrumsLeftPedal = new CItemToggle("    LeftPedal", CDTXMania.ConfigIni.bAutoPlay.LP,
                "左ペダルを自動で演奏します。",
                "Play Left Pedal automatically.");
            iDrumsLeftPedal.BindConfig(
                () => iDrumsLeftPedal.bON = CDTXMania.ConfigIni.bAutoPlay.LP,
                () => CDTXMania.ConfigIni.bAutoPlay.LP = iDrumsLeftPedal.bON);
            this.listItems.Add(iDrumsLeftPedal);

            var iDrumsLeftBassDrum = new CItemToggle("    LBassDrum", CDTXMania.ConfigIni.bAutoPlay.LBD,
                "左バスドラムを自動で演奏します。",
                "Play Left Bass Drum automatically.");
            iDrumsLeftBassDrum.BindConfig(
                () => iDrumsLeftBassDrum.bON = CDTXMania.ConfigIni.bAutoPlay.LBD,
                () => CDTXMania.ConfigIni.bAutoPlay.LBD = iDrumsLeftBassDrum.bON);
            this.listItems.Add(iDrumsLeftBassDrum);

            var iDrumsSnare = new CItemToggle("    Snare", CDTXMania.ConfigIni.bAutoPlay.SD,
                "スネアを自動で演奏します。",
                "Play Snare automatically.");
            iDrumsSnare.BindConfig(
                () => iDrumsSnare.bON = CDTXMania.ConfigIni.bAutoPlay.SD,
                () => CDTXMania.ConfigIni.bAutoPlay.SD = iDrumsSnare.bON);
            this.listItems.Add(iDrumsSnare);

            var iDrumsBass = new CItemToggle("    BassDrum", CDTXMania.ConfigIni.bAutoPlay.BD,
                "バスドラムを自動で演奏します。",
                "Play Bass Drum automatically.");
            iDrumsBass.BindConfig(
                () => iDrumsBass.bON = CDTXMania.ConfigIni.bAutoPlay.BD,
                () => CDTXMania.ConfigIni.bAutoPlay.BD = iDrumsBass.bON);
            this.listItems.Add(iDrumsBass);

            var iDrumsHighTom = new CItemToggle("    HighTom", CDTXMania.ConfigIni.bAutoPlay.HT,
                "ハイタムを自動で演奏します。",
                "Play High Tom automatically.");
            iDrumsHighTom.BindConfig(
                () => iDrumsHighTom.bON = CDTXMania.ConfigIni.bAutoPlay.HT,
                () => CDTXMania.ConfigIni.bAutoPlay.HT = iDrumsHighTom.bON);
            this.listItems.Add(iDrumsHighTom);

            var iDrumsLowTom = new CItemToggle("    LowTom", CDTXMania.ConfigIni.bAutoPlay.LT,
                "ロータムを自動で演奏します。",
                "Play Low Tom automatically.");
            iDrumsLowTom.BindConfig(
                () => iDrumsLowTom.bON = CDTXMania.ConfigIni.bAutoPlay.LT,
                () => CDTXMania.ConfigIni.bAutoPlay.LT = iDrumsLowTom.bON);
            this.listItems.Add(iDrumsLowTom);

            var iDrumsFloorTom = new CItemToggle("    FloorTom", CDTXMania.ConfigIni.bAutoPlay.FT,
                "フロアタムを自動で演奏します。",
                "Play Floor Tom automatically.");
            iDrumsFloorTom.BindConfig(
                () => iDrumsFloorTom.bON = CDTXMania.ConfigIni.bAutoPlay.FT,
                () => CDTXMania.ConfigIni.bAutoPlay.FT = iDrumsFloorTom.bON);
            this.listItems.Add(iDrumsFloorTom);

            var iDrumsCymbal = new CItemToggle("    Cymbal", CDTXMania.ConfigIni.bAutoPlay.CY,
                "右シンバルを自動で演奏します。",
                "Play Right Cymbal automatically.");
            iDrumsCymbal.BindConfig(
                () => iDrumsCymbal.bON = CDTXMania.ConfigIni.bAutoPlay.CY,
                () => CDTXMania.ConfigIni.bAutoPlay.CY = iDrumsCymbal.bON);
            this.listItems.Add(iDrumsCymbal);

            var iDrumsRide = new CItemToggle("    Ride", CDTXMania.ConfigIni.bAutoPlay.RD,
                "ライドシンバルを自動で演奏します。",
                "Play Ride Cymbal automatically.");
            iDrumsRide.BindConfig(
                () => iDrumsRide.bON = CDTXMania.ConfigIni.bAutoPlay.RD,
                () => CDTXMania.ConfigIni.bAutoPlay.RD = iDrumsRide.bON);
            this.listItems.Add(iDrumsRide);
            
            //add the action for this later, as it needs to be able to change all of the above buttons
            iDrumsAutoPlayAll.action = () =>
            {
                bool bAutoOn = iDrumsAutoPlayAll.e現在の状態 == CItemThreeState.E状態.ON;
                
                iDrumsLeftCymbal.bON = bAutoOn;
                iDrumsHiHat.bON = bAutoOn;
                iDrumsSnare.bON = bAutoOn;
                iDrumsBass.bON = bAutoOn;
                iDrumsHighTom.bON = bAutoOn;
                iDrumsLowTom.bON = bAutoOn;
                iDrumsFloorTom.bON = bAutoOn;
                iDrumsCymbal.bON = bAutoOn;
                iDrumsRide.bON = bAutoOn;
                iDrumsLeftPedal.bON = bAutoOn;
                iDrumsLeftBassDrum.bON = bAutoOn;
            };

            //----------StandardOption----------

            this.iDrumsScrollSpeed = new CItemInteger("ScrollSpeed", 0, 0x7cf, CDTXMania.ConfigIni.nScrollSpeed.Drums,
                "ノーツの流れるスピードを\n"+
                "変更します。\n" +
                "数字が大きくなるほど\n" +
                "スピードが速くなり、\n"+
                "ノーツの間隔が広がります。",
                "Change the scroll speed for drums lanes.\n" +
                "You can set it from x0.5 to x1000.0.\n" +
                "(ScrollSpeed=x0.5 means half speed)");
            iDrumsScrollSpeed.BindConfig(
                () => iDrumsScrollSpeed.nCurrentValue = CDTXMania.ConfigIni.nScrollSpeed.Drums,
                () => CDTXMania.ConfigIni.nScrollSpeed.Drums = iDrumsScrollSpeed.nCurrentValue);
            this.listItems.Add(this.iDrumsScrollSpeed);

            var iDrumsHIDSUD = new CItemList("HID-SUD", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.nHidSud.Drums,
                "HIDDEN:チップが途中から見えなくなります。\n" +
                "SUDDEN:チップが途中まで見えません。\n" +
                "HID-SUD:HIDDEN、SUDDEN\n" +
                "の両方が適用されます。\n" +
                "STEALTH:チップがずっと表示されません。",
                "Hidden: Chips disappear at mid-screen\n" +
                "Sudden: Chips appear from mid-screen\n" +
                "HidSud: Chips appear only at mid-screen\n" +
                "Stealth: Chips are transparent",
                new string[] { "OFF", "Hidden", "Sudden", "HidSud", "Stealth" });
            iDrumsHIDSUD.BindConfig(
                () => iDrumsHIDSUD.n現在選択されている項目番号 = CDTXMania.ConfigIni.nHidSud.Drums,
                () => CDTXMania.ConfigIni.nHidSud.Drums = iDrumsHIDSUD.n現在選択されている項目番号);
            this.listItems.Add(iDrumsHIDSUD);

            //----------DisplayOption----------
            var iDrumsDark = new CItemList("       Dark", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eDark,
                "レーン表示のオプションを\n"+
                "まとめて切り替えます。\n" +
                "HALF:レーンが表示されなく\n"+
                "なります。\n" +
                "FULL:さらに小節線、拍線、\n"+
                "判定ラインも表示されなくなります。",
                "OFF: all display parts are shown.\n"+
                "HALF: lanes and gauge are hidden.\n"+
                "FULL: additionaly to HALF, bar/beat lines, hit bar are hidden.",
                new string[] { "OFF", "HALF", "FULL" });
            this.listItems.Add(iDrumsDark);
            
            var iDrumsLaneDisp = new CItemList("LaneDisp", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.nLaneDisp.Drums,
                "レーンの縦線と小節線の表示を切り替えます。\n" +
                "ALL  ON :全て表示します。\n" +
                "LANE OFF:レーン背景を表示しません。\n" +
                "LINE OFF:小節線を表示しません。\n" +
                "ALL  OFF:全て表示しません。",
                "Display of vertical lines and bar lines in the lane.\n" +
                "ALL ON: Show all\n" +
                "LANE OFF: Do not display lane background\n" +
                "LINE OFF: Do not display bar lines\n" +
                "ALL OFF: Do not display any",
                new string[] { "ALL ON", "LANE OFF", "LINE OFF", "ALL OFF" });
            iDrumsLaneDisp.BindConfig(
                () => iDrumsLaneDisp.n現在選択されている項目番号 = CDTXMania.ConfigIni.nLaneDisp.Drums,
                () => CDTXMania.ConfigIni.nLaneDisp.Drums = iDrumsLaneDisp.n現在選択されている項目番号);
            this.listItems.Add(iDrumsLaneDisp);

            var iDrumsJudgeLineDisp = new CItemToggle("JudgeLineDisp", CDTXMania.ConfigIni.bJudgeLineDisp.Drums,
                "判定ラインの表示 / 非表示を切り替えます。",
                "Toggle JudgeLine");
            iDrumsJudgeLineDisp.BindConfig(
                () => iDrumsJudgeLineDisp.bON = CDTXMania.ConfigIni.bJudgeLineDisp.Drums,
                () => CDTXMania.ConfigIni.bJudgeLineDisp.Drums = iDrumsJudgeLineDisp.bON);
            this.listItems.Add(iDrumsJudgeLineDisp);

            var iDrumsLaneFlush = new CItemToggle("LaneFlush", CDTXMania.ConfigIni.bLaneFlush.Drums,
                "レーンフラッシュの表示 / 非表示を\n" +
                 "切り替えます。",
                "Toggle LaneFlush");
            iDrumsLaneFlush.BindConfig(
                () => iDrumsLaneFlush.bON = CDTXMania.ConfigIni.bLaneFlush.Drums,
                () => CDTXMania.ConfigIni.bLaneFlush.Drums = iDrumsLaneFlush.bON);
            this.listItems.Add(iDrumsLaneFlush);
            
            //add the action for this later, as it needs to be able to change all of the above buttons
            iDrumsDark.action = () =>
            {
                if (iDrumsDark.n現在選択されている項目番号 == (int)EDarkMode.FULL)
                {
                    iDrumsLaneDisp.n現在選択されている項目番号 = 3;
                    iDrumsJudgeLineDisp.bON = false;
                    iDrumsLaneFlush.bON = false;
                }
                else if (iDrumsDark.n現在選択されている項目番号 == (int)EDarkMode.HALF)
                {
                    iDrumsLaneDisp.n現在選択されている項目番号 = 1;
                    iDrumsJudgeLineDisp.bON = true;
                    iDrumsLaneFlush.bON = true;
                }
                else
                {
                    iDrumsLaneDisp.n現在選択されている項目番号 = 0;
                    iDrumsJudgeLineDisp.bON = true;
                    iDrumsLaneFlush.bON = true;
                }
            };

            var iDrumsAttackEffect = new CItemList("AttackEffect", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eAttackEffect.Drums,
                "アタックエフェクトの表示方法を設定します。\n" +
                "ALL ON: すべて表示\n" +
                "ChipOFF: チップエフェクトのみ消す\n" +
                "EffectOnly: エフェクト画像以外消す\n" +
                "ALL OFF: すべて消す",
                "",
                new string[] { "ALL ON", "ChipOFF", "EffectOnly", "ALL OFF" });
            iDrumsAttackEffect.BindConfig(
                () => iDrumsAttackEffect.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eAttackEffect.Drums,
                () => CDTXMania.ConfigIni.eAttackEffect.Drums = (EType)iDrumsAttackEffect.n現在選択されている項目番号);
            this.listItems.Add(iDrumsAttackEffect);

            var iDrumsReverse = new CItemToggle("Reverse", CDTXMania.ConfigIni.bReverse.Drums,
                "ONにすると\n"+
                "判定ラインが上になり、\n" +
                "ノーツが下から上へ\n"+
                "流れます。",
                "The scroll way is reversed. Drums chips flow from the bottom to the top.");
            iDrumsReverse.BindConfig(
                () => iDrumsReverse.bON = CDTXMania.ConfigIni.bReverse.Drums,
                () => CDTXMania.ConfigIni.bReverse.Drums = iDrumsReverse.bON);
            this.listItems.Add(iDrumsReverse);

            var iDrumsPosition = new CItemList("JudgePosition", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.JudgementStringPosition.Drums,
                "ゲーム中に表示される\n"+
                "判定文字の位置を変更します。\n" +
                "  P-A: OnTheLane\n" +
                "  P-B: 判定ライン下\n" +
                "  OFF: 表示しない",
                "The position to show judgement mark.\n" +
                "(Perfect, Great, ...)\n" +
                "\n" +
                " P-A: on the lanes.\n" +
                " P-B: under the hit bar.\n" +
                " OFF: no judgement mark.",
                new string[] { "P-A", "P-B", "OFF" });
            iDrumsPosition.BindConfig(
                () => iDrumsPosition.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.JudgementStringPosition.Drums,
                () => CDTXMania.ConfigIni.JudgementStringPosition.Drums = (EType)iDrumsPosition.n現在選択されている項目番号);
            this.listItems.Add(iDrumsPosition);

            var iDrumsComboDisp = new CItemToggle("Combo", CDTXMania.ConfigIni.bドラムコンボ文字の表示,
                "OFFにするとコンボが表示されなくなります。",
                "Turn ON the Drums Combo Display");
            iDrumsComboDisp.BindConfig(
                () => iDrumsComboDisp.bON = CDTXMania.ConfigIni.bドラムコンボ文字の表示,
                () => CDTXMania.ConfigIni.bドラムコンボ文字の表示 = iDrumsComboDisp.bON);
            this.listItems.Add( iDrumsComboDisp );

            var iDrumsLaneType = new CItemList("LaneType", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eLaneType.Drums,
                "ドラムのレーンの配置を変更します。\n" +
                "Type-A 通常の設定です。\n" +
                "Type-B 2ペダルとタムをそれぞれま\n" +
                "とめた表示です。\n" +
                "Type-C 3タムのみをまとめた表示です。\n" +
                "Type-D 左右完全対象の表示です。",
                "To change the displaying position of\n" +
                "Drum Lanes.\n" +
                "Type-A default\n" +
                "Type-B Summarized 2 pedals and Toms.\n" +
                "Type-C Summarized 3 Toms only.\n" +
                "Type-D Work In Progress....",
                new string[] { "Type-A", "Type-B", "Type-C", "Type-D" });
            iDrumsLaneType.BindConfig(
                () => iDrumsLaneType.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eLaneType.Drums,
                () => CDTXMania.ConfigIni.eLaneType.Drums = (EType)iDrumsLaneType.n現在選択されている項目番号);
            this.listItems.Add(iDrumsLaneType);

            var iDrumsRDPosition = new CItemList("RDPosition", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eRDPosition,
                "ライドシンバルレーンの表示\n" +
                "位置を変更します。\n"+
                "RD RC:最右端はRCレーンになります\n"+
                "RC RD: 最右端はRDレーンになります",
                "Change the display position of the ride cymbal.\n"+
                "RD RC: Rightmost lane is RC\n" +
                "RC RD: Rightmost lane is RD",
                new string[] { "RD RC", "RC RD" });
            iDrumsRDPosition.BindConfig(
                () => iDrumsRDPosition.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eRDPosition,
                () => CDTXMania.ConfigIni.eRDPosition = (ERDPosition)iDrumsRDPosition.n現在選択されている項目番号);
            this.listItems.Add(iDrumsRDPosition);
            
            //----------SpecialOption----------

            var iDrumsHAZARD = new CItemToggle("HAZARD", CDTXMania.ConfigIni.bHAZARD,
                "ドSハザードモード\n" +
                "GREAT以下の判定でも回数が減ります。",
                "SuperHazardMode\n" +
                "");
            iDrumsHAZARD.BindConfig(
                () => iDrumsHAZARD.bON = CDTXMania.ConfigIni.bHAZARD,
                () => CDTXMania.ConfigIni.bHAZARD = iDrumsHAZARD.bON);
            this.listItems.Add(iDrumsHAZARD);

            var iDrumsTight = new CItemToggle("Tight", CDTXMania.ConfigIni.bTight,
                "ドラムチップのないところでパッドを\n" +
                "叩くとミスになります。",
                "Hitting pad without chip is a MISS.");
            iDrumsTight.BindConfig(
                () => iDrumsTight.bON = CDTXMania.ConfigIni.bTight,
                () => CDTXMania.ConfigIni.bTight = iDrumsTight.bON);
            this.listItems.Add(iDrumsTight);
            
            var iSystemHHGroup = new CItemList("HH Group", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eHHGroup,
                "ハイハットレーン打ち分け設定：\n" +
                "左シンバル、ハイハットオープン、ハ\n" +
                "イハットクローズの打ち分け方法を指\n" +
                "定します。\n" +
                "  HH-0 ... LC | HHC | HHO\n" +
                "  HH-1 ... LC & ( HHC | HHO )\n" +
                "  HH-2 ... LC | ( HHC & HHO )\n" +
                "  HH-3 ... LC & HHC & HHO\n" +
                "\n",
                "HH-0: LC|HC|HO; all are separated.\n" +
                "HH-1: LC&(HC|HO);\n" +
                " HC and HO are separted.\n" +
                " LC is grouped with HC and HHO.\n" +
                "HH-2: LC|(HC&HO);\n" +
                " LC and HHs are separated.\n" +
                " HC and HO are grouped.\n" +
                "HH-3: LC&HC&HO; all are grouped.\n" +
                "\n",
                new string[] { "HH-0", "HH-1", "HH-2", "HH-3" });
            iSystemHHGroup.BindConfig(
                () => iSystemHHGroup.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eHHGroup,
                () => CDTXMania.ConfigIni.eHHGroup = (EHHGroup)iSystemHHGroup.n現在選択されている項目番号);
            this.listItems.Add(iSystemHHGroup);

            var iSystemFTGroup = new CItemList("FT Group", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eFTGroup,
                "フロアタム打ち分け設定：\n" +
                "ロータムとフロアタムの打ち分け方法\n" +
                "を指定します。\n" +
                "  FT-0 ... LT | FT\n" +
                "  FT-1 ... LT & FT\n",
                "FT-0: LT|FT\n" +
                " LT and FT are separated.\n" +
                "FT-1: LT&FT\n" +
                " LT and FT are grouped.",
                new string[] { "FT-0", "FT-1" });
            iSystemFTGroup.BindConfig(
                () => iSystemFTGroup.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eFTGroup,
                () => CDTXMania.ConfigIni.eFTGroup = (EFTGroup)iSystemFTGroup.n現在選択されている項目番号);
            this.listItems.Add(iSystemFTGroup);

            var iSystemCYGroup = new CItemList("CY Group", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eCYGroup,
                "シンバルレーン打ち分け設定：\n" +
                "右シンバルとライドシンバルの打ち分\n" +
                "け方法を指定します。\n" +
                "  CY-0 ... CY | RD\n" +
                "  CY-1 ... CY & RD\n",
                "CY-0: CY|RD\n" +
                " CY and RD are separated.\n" +
                "CY-1: CY&RD\n" +
                " CY and RD are grouped.",
                new string[] { "CY-0", "CY-1" });
            iSystemCYGroup.BindConfig(
                () => iSystemCYGroup.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eCYGroup,
                () => CDTXMania.ConfigIni.eCYGroup = (ECYGroup)iSystemCYGroup.n現在選択されている項目番号);
            this.listItems.Add(iSystemCYGroup);

            var iSystemBDGroup = new CItemList("BD Group", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eBDGroup,		// #27029 2012.1.4 from
                "フットペダル打ち分け設定：\n" +
                "左ペダル、左バスドラ、右バスドラの打ち分け\n" +
                "方法を指定します。\n" +
                "  BD-0 ... LP | LBD | BD\n" +
                "  BD-1 ... LP | LBD & BD\n" +
                "  BD-2 ... LP & LBD | BD\n" +
                "  BD-3 ... LP & LBD & BD\n",
                "Foot pedal grouping settings:\n" +
                "Specifies how the left pedal, " +
                "left bass drum, and right bass " +
                "drum are grouped.\n" +
                "  BD-0 ... LP | LBD | BD\n" +
                "  BD-1 ... LP | LBD & BD\n" +
                "  BD-2 ... LP & LBD | BD\n" +
                "  BD-3 ... LP & LBD & BD\n",
                new string[] { "BD-0", "BD-1", "BD-2", "BD-3" });
            iSystemBDGroup.BindConfig(
                () => iSystemBDGroup.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eBDGroup,
                () => CDTXMania.ConfigIni.eBDGroup = (EBDGroup)iSystemBDGroup.n現在選択されている項目番号);
            this.listItems.Add(iSystemBDGroup);

            var iSystemCymbalFree = new CItemToggle("CymbalFree", CDTXMania.ConfigIni.bシンバルフリー,
                "シンバルフリーモード：\n" +
                "左シンバル_右シンバルの区別をなく\n" +
                "します。ライドシンバルまで区別をな\n" +
                "くすか否かは、CYGroup に従います。\n",
                "Turn ON to group LC (left cymbal) and\n" +
                " CY (right cymbal).\n" +
                "Whether RD (ride cymbal) is also\n" +
                " grouped or not depends on the\n" +
                "'CY Group' setting.");
            iSystemCymbalFree.BindConfig(
                () => iSystemCymbalFree.bON = CDTXMania.ConfigIni.bシンバルフリー,
                () => CDTXMania.ConfigIni.bシンバルフリー = iSystemCymbalFree.bON);
            this.listItems.Add(iSystemCymbalFree);

            var iSystemHitSoundPriorityHH = new CItemList("HH Priority", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eHitSoundPriorityHH,
                "発声音決定の優先順位：\n" +
                "ハイハットレーン打ち分け有効時に、\n" +
                "チップの発声音をどのように決定する\n" +
                "かを指定します。\n" +
                "  C > P ... チップの音が優先\n" +
                "  P > C ... 叩いたパッドの音が優先\n" +
                "\n",
                "To specify playing sound in case you're\n" +
                " using HH-0,1 and 2.\n" +
                "\n" +
                "C>P:\n" +
                " Chip sound is prior to the pad sound.\n" +
                "P>C:\n" +
                " Pad sound is prior to the chip sound.\n" +
                "\n" +
                "* This value cannot be changed while \n" +
                "  BD Group is set as BD-1.",
                new string[] { "C>P", "P>C" });
            iSystemHitSoundPriorityHH.BindConfig(
                () => iSystemHitSoundPriorityHH.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eHitSoundPriorityHH,
                () => CDTXMania.ConfigIni.eHitSoundPriorityHH = (EPlaybackPriority)iSystemHitSoundPriorityHH.n現在選択されている項目番号);
            this.listItems.Add(iSystemHitSoundPriorityHH);

            var iSystemHitSoundPriorityFT = new CItemList("FT Priority", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eHitSoundPriorityFT,
                "発声音決定の優先順位：\n" +
                "フロアタム打ち分け有効時に、チップ\n" +
                "の発声音をどのように決定するかを\n" +
                "指定します。\n" +
                "  C > P ... チップの音が優先\n" +
                "  P > C ... 叩いたパッドの音が優先",
                "To specify playing sound in case you're\n" +
                " using FT-0.\n" +
                "\n" +
                "C>P:\n" +
                " Chip sound is prior to the pad sound.\n" +
                "P>C:\n" +
                " Pad sound is prior to the chip sound.",
                new string[] { "C>P", "P>C" });
            iSystemHitSoundPriorityFT.BindConfig(
                () => iSystemHitSoundPriorityFT.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eHitSoundPriorityFT,
                () => CDTXMania.ConfigIni.eHitSoundPriorityFT = (EPlaybackPriority)iSystemHitSoundPriorityFT.n現在選択されている項目番号);
            this.listItems.Add(iSystemHitSoundPriorityFT);

            var iSystemHitSoundPriorityCY = new CItemList("CY Priority", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eHitSoundPriorityCY,
                "発声音決定の優先順位：\n" +
                "シンバルレーン打ち分け有効時に、\n" +
                "チップの発声音をどのように決定する\n" +
                "かを指定します。\n" +
                "  C > P ... チップの音が優先\n" +
                "  P > C ... 叩いたパッドの音が優先",
                "To specify playing sound in case you're\n" +
                " using CY-0.\n" +
                "\n" +
                "C>P:\n" +
                " Chip sound is prior to the pad sound.\n" +
                "P>C:\n" +
                " Pad sound is prior to the chip sound.",
                new string[] { "C>P", "P>C" });
            iSystemHitSoundPriorityCY.BindConfig(
                () => iSystemHitSoundPriorityCY.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eHitSoundPriorityCY,
                () => CDTXMania.ConfigIni.eHitSoundPriorityCY = (EPlaybackPriority)iSystemHitSoundPriorityCY.n現在選択されている項目番号);
            this.listItems.Add(iSystemHitSoundPriorityCY);

            var iSystemFillIn = new CItemToggle("FillIn", CDTXMania.ConfigIni.bFillInEnabled,
                "フィルインエフェクトの使用：\n" +
                "フィルイン区間の爆発パターンに特別" +
                "のエフェクトを使用します。\n" +
                "フィルインエフェクトの描画にはそれな" +
                "りのマシンパワーが必要とされます。",
                "To show bursting effects at the fill-in\n" +
                " zone or not.");
            iSystemFillIn.BindConfig(
                () => iSystemFillIn.bON = CDTXMania.ConfigIni.bFillInEnabled,
                () => CDTXMania.ConfigIni.bFillInEnabled = iSystemFillIn.bON);
            this.listItems.Add(iSystemFillIn); 
            
            var iSystemHitSound = new CItemToggle("HitSound", CDTXMania.ConfigIni.bドラム打音を発声する,
                "打撃音の再生：\n" +
                "これをOFFにすると、パッドを叩いた\n" +
                "ときの音を再生しなくなります（ドラム\n" +
                "のみ）。\n" +
                "DTX の音色で演奏したい場合などに\n" +
                "OFF にします。\n" +
                "\n",
                "Turn OFF if you don't want to play\n" +
                " hitting chip sound.\n" +
                "It is useful to play with real/electric\n" +
                " drums kit.\n");
            iSystemHitSound.BindConfig(
                () => iSystemHitSound.bON = CDTXMania.ConfigIni.bドラム打音を発声する,
                () => CDTXMania.ConfigIni.bドラム打音を発声する = iSystemHitSound.bON);
            this.listItems.Add(iSystemHitSound);

            var iSystemSoundMonitorDrums = new CItemToggle("DrumsMonitor", CDTXMania.ConfigIni.b演奏音を強調する.Drums,
                "ドラム音モニタ：\n" +
                "ドラム音を他の音より大きめの音量で\n" +
                "発声します。\n" +
                "ただし、オートプレイの場合は通常音\n" +
                "量で発声されます。",
                "To enhance the drums chip sound\n" +
                "(except autoplay).");
            iSystemSoundMonitorDrums.BindConfig(
                () => iSystemSoundMonitorDrums.bON = CDTXMania.ConfigIni.b演奏音を強調する.Drums,
                () => CDTXMania.ConfigIni.b演奏音を強調する.Drums = iSystemSoundMonitorDrums.bON);
            this.listItems.Add(iSystemSoundMonitorDrums);
            
            var iSystemMinComboDrums = new CItemInteger("D-MinCombo", 1, 0x1869f, CDTXMania.ConfigIni.n表示可能な最小コンボ数.Drums,
                "表示可能な最小コンボ数（ドラム）：\n" +
                "画面に表示されるコンボの最小の数\n" +
                "を指定します。\n" +
                "1 ～ 99999 の値が指定可能です。",
                "Initial number to show the combo\n" +
                " for the drums.\n" +
                "You can specify from 1 to 99999.");
            iSystemMinComboDrums.BindConfig(
                () => iSystemMinComboDrums.nCurrentValue = CDTXMania.ConfigIni.n表示可能な最小コンボ数.Drums,
                () => CDTXMania.ConfigIni.n表示可能な最小コンボ数.Drums = iSystemMinComboDrums.nCurrentValue);
            this.listItems.Add(iSystemMinComboDrums);
            
            var iDrumsHHOGraphics = new CItemList("HHOGraphics", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eHHOGraphics.Drums,
                "オープンハイハットの表示画像を変更します。\n" +
                "A: DTXMania元仕様\n" +
                "B: ○なし\n" +
                "C: クローズハットと同じ",
                "To change the graphics of open hihat.\n" +
                "A: default graphics of DTXMania\n" +
                "B: A without a circle\n" +
                "C: same as closed hihat",
                new string[] { "Type A", "Type B", "Type C" });
            iDrumsHHOGraphics.BindConfig(
                () => iDrumsHHOGraphics.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eHHOGraphics.Drums,
                () => CDTXMania.ConfigIni.eHHOGraphics.Drums = (EType)iDrumsHHOGraphics.n現在選択されている項目番号);
            this.listItems.Add(iDrumsHHOGraphics);

            var iDrumsLBDGraphics = new CItemList("LBDGraphics", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eLBDGraphics.Drums,
                "LBDチップの表示画像を変更します。\n" +
                "A: LPと同じ画像を使う\n" +
                "B: LBDとLPで色分けをする",
                "To change the graphics of left bass.\n" +
                "A: same as LP chips\n" +
                "B: In LP and LBD Color-coded.",
                new string[] { "Type A", "Type B" });
            iDrumsLBDGraphics.BindConfig(
                () => iDrumsLBDGraphics.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eLBDGraphics.Drums,
                () => CDTXMania.ConfigIni.eLBDGraphics.Drums = (EType)iDrumsLBDGraphics.n現在選択されている項目番号);
            this.listItems.Add(iDrumsLBDGraphics);

            var iDrumsJudgeLinePos = new CItemInteger("JudgeLinePos", 0, 100, CDTXMania.ConfigIni.nJudgeLine.Drums,
                "判定ラインの位置を\n" +
                "調整できます。\n" +
                "0～100の間で指定できます。",
                "To change the judgeLinePosition for the\n" +
                "You can set it from 0 to 100.");
            iDrumsJudgeLinePos.BindConfig(
                () => iDrumsJudgeLinePos.nCurrentValue = CDTXMania.ConfigIni.nJudgeLine.Drums,
                () => CDTXMania.ConfigIni.nJudgeLine.Drums = iDrumsJudgeLinePos.nCurrentValue);
            this.listItems.Add(iDrumsJudgeLinePos);

            var iDrumsShutterInPos = new CItemInteger("ShutterInPos", 0, 100, CDTXMania.ConfigIni.nShutterInSide.Drums,
                "ノーツ出現側の\n"+
                "シャッター位置を調整し\n" +
                "ノーツの見える位置を\n"+
                "制限します。",
                "\n" +
                "\n" +
                "");
            iDrumsShutterInPos.BindConfig(
                () => iDrumsShutterInPos.nCurrentValue = CDTXMania.ConfigIni.nShutterInSide.Drums,
                () => CDTXMania.ConfigIni.nShutterInSide.Drums = iDrumsShutterInPos.nCurrentValue);
            this.listItems.Add(iDrumsShutterInPos);

            var iDrumsShutterOutPos = new CItemInteger("ShutterOutPos", 0, 100, CDTXMania.ConfigIni.nShutterOutSide.Drums,
                "判定ライン側の\n" +
                "シャッター位置を調整し\n" +
                "ノーツの見える位置を\n" +
                "制限します。",
                "\n" +
                "\n" +
                "");
            iDrumsShutterOutPos.BindConfig(
                () => iDrumsShutterOutPos.nCurrentValue = CDTXMania.ConfigIni.nShutterOutSide.Drums,
                () => CDTXMania.ConfigIni.nShutterOutSide.Drums = iDrumsShutterOutPos.nCurrentValue);
            this.listItems.Add(iDrumsShutterOutPos);

            var iMutingLP = new CItemToggle("Muting LP", CDTXMania.ConfigIni.bMutingLP,
                "LPの入力で発声中のHHを\n消音します。",
                "Turn ON to let HH chips be muted\n" +
                "by LP chips.");
            iMutingLP.BindConfig(
                () => iMutingLP.bON = CDTXMania.ConfigIni.bMutingLP,
                () => CDTXMania.ConfigIni.bMutingLP = iMutingLP.bON);
            this.listItems.Add(iMutingLP);

            var iDrumsAssignToLBD = new CItemToggle("AssignToLBD", CDTXMania.ConfigIni.bAssignToLBD.Drums,
                "旧仕様のドコドコチップを\n" +
                "LBDレーンに振り分けます。\n" +
                "LP、LBDがある譜面では\n"+
                "無効になります。",
                "To move some of BassDrum chips to\n" +
                "LBD lane moderately.\n" +
                "(for old-style 2-bass DTX scores\n" +
                "without LP & LBD chips)");
            iDrumsAssignToLBD.BindConfig(
                () => iDrumsAssignToLBD.bON = CDTXMania.ConfigIni.bAssignToLBD.Drums,
                () => CDTXMania.ConfigIni.bAssignToLBD.Drums = iDrumsAssignToLBD.bON);
            this.listItems.Add(iDrumsAssignToLBD);

            var iDrumsDkdkType = new CItemList("DkdkType", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eDkdkType.Drums,
                "ツーバス譜面の仕様を変更する。\n" +
                "L R: デフォルト\n" +
                "R L: 始動足変更\n" +
                "R Only: dkdk1レーン化",
                "To change the style of double-bass-\n" +
                "concerned chips.\n" +
                "L R: default\n" +
                "R L: changes the beginning foot\n" +
                "R Only: puts bass chips into single\n" +
                "lane",
                new string[] { "L R", "R L", "R Only" });
            iDrumsDkdkType.BindConfig(
                () => iDrumsDkdkType.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eDkdkType.Drums,
                () => CDTXMania.ConfigIni.eDkdkType.Drums = (EType)iDrumsDkdkType.n現在選択されている項目番号);
            this.listItems.Add(iDrumsDkdkType);

            var iDrumsNumOfLanes = new CItemList("NumOfLanes", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eNumOfLanes.Drums,
                "10レーン譜面の仕様を変更する。\n" +
                "A: デフォルト10レーン\n" +
                "B: XG仕様9レーン\n" +
                "C: CLASSIC仕様6レーン",

                "To change the number of lanes.\n" +
                "10: default 10 lanes\n" +
                "9: XG style 9 lanes\n" +
                "6: classic style 6 lanes",
                new string[] { "10", "9", "6" });
            iDrumsNumOfLanes.BindConfig(
                () => iDrumsNumOfLanes.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eNumOfLanes.Drums,
                () => CDTXMania.ConfigIni.eNumOfLanes.Drums = (EType)iDrumsNumOfLanes.n現在選択されている項目番号);
            this.listItems.Add(iDrumsNumOfLanes);

            var iDrumsRandomPad = new CItemList("RandomPad", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eRandom.Drums,
                "ドラムのパッドチップが\n" +
                "ランダムに降ってきます。\n" +
                "Mirror:ミラーをかけます\n" +
                "Part:レーン単位で交換\n" +
                "Super:小節単位で交換\n" +
                "Hyper:1拍ごとに交換\n" +
                "Master:死ぬがよい\n" +
                "Another:丁度よくバラける",
                "Drums chips (pads) come randomly.\n" +
                "Mirror: \n" +
                "Part: swapping lanes randomly\n" +
                "Super: swapping for each measure\n" +
                "Hyper: swapping for each 1/4 measure\n" +
                "Master: game over...\n" +
                "Another: moderately swapping each\n" +
                "chip randomly",
                new string[] { "OFF", "Mirror", "Part", "Super", "Hyper", "Master", "Another" });
            iDrumsRandomPad.BindConfig(
                () => iDrumsRandomPad.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eRandom.Drums,
                () => CDTXMania.ConfigIni.eRandom.Drums = (ERandomMode)iDrumsRandomPad.n現在選択されている項目番号);
            this.listItems.Add(iDrumsRandomPad);

            var iDrumsRandomPedal = new CItemList("RandomPedal", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eRandomPedal.Drums,
                "ドラムの足チップがランダムに\n降ってきます。\n" +
                "Mirror:ミラーをかけます\n" +
                "Part:レーン単位で交換\n" +
                "Super:小節単位で交換\n" +
                "Hyper:1拍ごとに交換\n" +
                "Master:死ぬがよい\n" +
                "Another:丁度よくバラける",
                "Drums chips (pedals) come randomly.\n" +
                "Part: swapping lanes randomly\n" +
                "Super: swapping for each measure\n" +
                "Hyper: swapping for each 1/4 measure\n" +
                "Master: game over...\n" +
                "Another: moderately swapping each\n" +
                "chip randomly",
                new string[] { "OFF", "Mirror", "Part", "Super", "Hyper", "Master", "Another" });
            iDrumsRandomPedal.BindConfig(
                () => iDrumsRandomPedal.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eRandomPedal.Drums,
                () => CDTXMania.ConfigIni.eRandomPedal.Drums = (ERandomMode)iDrumsRandomPedal.n現在選択されている項目番号);
            this.listItems.Add(iDrumsRandomPedal);

			this.iDrumsGraph = new CItemToggle( "Graph", CDTXMania.ConfigIni.bGraph有効.Drums,
				"最高スキルと比較できるグラフを表示します。\n" +
				"オートプレイだと表示されません。",
				"To draw Graph or not." );
            this.iDrumsGraph.BindConfig(
                () => this.iDrumsGraph.bON = CDTXMania.ConfigIni.bGraph有効.Drums,
                () => CDTXMania.ConfigIni.bGraph有効.Drums = this.iDrumsGraph.bON );
			this.listItems.Add( this.iDrumsGraph );
            
            // #23580 2011.1.3 yyagi
            var iDrumsInputAdjustTimeMs = new CItemInteger("InputAdjust", -99, 99, CDTXMania.ConfigIni.nInputAdjustTimeMs.Drums,
                "ドラムの入力タイミングの微調整を行います。\n" +
                "-99 ～ 99ms まで指定可能です。\n" +
                "値を指定してください。\n",
                "To adjust the drums input timing.\n" +
                "You can set from -99 to 0ms.\n" +
                "To decrease input lag, set minus value.");
            iDrumsInputAdjustTimeMs.BindConfig(
                () => iDrumsInputAdjustTimeMs.nCurrentValue = CDTXMania.ConfigIni.nInputAdjustTimeMs.Drums,
                () => CDTXMania.ConfigIni.nInputAdjustTimeMs.Drums = iDrumsInputAdjustTimeMs.nCurrentValue);
            this.listItems.Add(iDrumsInputAdjustTimeMs);

            var iDrumsGoToKeyAssign = new CItemBase("Drums Keys", CItemBase.EPanelType.Normal,
                "ドラムのキー入力に関する項目を設定します。",
                "Settings for the drums key/pad inputs.");
            iDrumsGoToKeyAssign.action = t項目リストの設定_KeyAssignDrums;
            this.listItems.Add(iDrumsGoToKeyAssign);

            OnListMenuの初期化();
            this.nCurrentSelection = 0;
            this.eMenuType = EMenuType.Drums;
        }
        #endregion
        #region [ t項目リストの設定_Guitar() ]
        public void tSetupItemList_Guitar()
        {
            this.tRecordToConfigIni();
            this.listItems.Clear();

            // #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

            this.iGuitarReturnToMenu = new CItemBase("<< Return To Menu", CItemBase.EPanelType.Other,
                "左側のメニューに戻ります。",
                "Return to left menu.");
            this.listItems.Add(this.iGuitarReturnToMenu);
            //this.iGuitarAutoPlay = new CItemToggle( "AutoPlay", CDTXMania.ConfigIni.bAutoPlay.Guitar,
            //    "ギターパートを自動で演奏します。",
            //    "To play the guitar part automatically." );
            //this.listItems.Add( this.iGuitarAutoPlay );

            var iGuitarAutoPlayAll = new CItemThreeState("AutoPlay (All)", CItemThreeState.E状態.不定,
                "全ネック/ピックの自動演奏の ON/OFF を\n" +
                "まとめて切り替えます。",
                "You can change whether Auto or not\n" +
                " for all guitar neck/pick at once.");
            this.listItems.Add(iGuitarAutoPlayAll);

            var iGuitarR = new CItemToggle("    R", CDTXMania.ConfigIni.bAutoPlay.GtR,
                "Rネックを自動で演奏します。",
                "To play R neck automatically.");
            iGuitarR.BindConfig(
                () => iGuitarR.bON = CDTXMania.ConfigIni.bAutoPlay.GtR,
                () => CDTXMania.ConfigIni.bAutoPlay.GtR = iGuitarR.bON);
            this.listItems.Add(iGuitarR);

            var iGuitarG = new CItemToggle("    G", CDTXMania.ConfigIni.bAutoPlay.GtG,
                "Gネックを自動で演奏します。",
                "To play G neck automatically.");
            iGuitarG.BindConfig(
                () => iGuitarG.bON = CDTXMania.ConfigIni.bAutoPlay.GtG,
                () => CDTXMania.ConfigIni.bAutoPlay.GtG = iGuitarG.bON);
            this.listItems.Add(iGuitarG);

            var iGuitarB = new CItemToggle("    B", CDTXMania.ConfigIni.bAutoPlay.GtB,
                "Bネックを自動で演奏します。",
                "To play B neck automatically.");
            iGuitarB.BindConfig(
                () => iGuitarB.bON = CDTXMania.ConfigIni.bAutoPlay.GtB,
                () => CDTXMania.ConfigIni.bAutoPlay.GtB = iGuitarB.bON);
            this.listItems.Add(iGuitarB);

            var iGuitarY = new CItemToggle("    Y", CDTXMania.ConfigIni.bAutoPlay.GtY,
                "Yネックを自動で演奏します。",
                "To play Y neck automatically.");
            iGuitarY.BindConfig(
                () => iGuitarY.bON = CDTXMania.ConfigIni.bAutoPlay.GtY,
                () => CDTXMania.ConfigIni.bAutoPlay.GtY = iGuitarY.bON);
            this.listItems.Add(iGuitarY);

            var iGuitarP = new CItemToggle("    P", CDTXMania.ConfigIni.bAutoPlay.GtP,
                "Pネックを自動で演奏します。",
                "To play P neck automatically.");
            iGuitarP.BindConfig(
                () => iGuitarP.bON = CDTXMania.ConfigIni.bAutoPlay.GtP,
                () => CDTXMania.ConfigIni.bAutoPlay.GtP = iGuitarP.bON);
            this.listItems.Add(iGuitarP);

            var iGuitarPick = new CItemToggle("    Pick", CDTXMania.ConfigIni.bAutoPlay.GtPick,
                "ピックを自動で演奏します。",
                "To play Pick automatically.");
            iGuitarPick.BindConfig(
                () => iGuitarPick.bON = CDTXMania.ConfigIni.bAutoPlay.GtPick,
                () => CDTXMania.ConfigIni.bAutoPlay.GtPick = iGuitarPick.bON);
            this.listItems.Add(iGuitarPick);

            var iGuitarW = new CItemToggle("    Wailing", CDTXMania.ConfigIni.bAutoPlay.GtW,
                "ウェイリングを自動で演奏します。",
                "To play wailing automatically.");
            iGuitarW.BindConfig(
                () => iGuitarW.bON = CDTXMania.ConfigIni.bAutoPlay.GtW,
                () => CDTXMania.ConfigIni.bAutoPlay.GtW = iGuitarW.bON);
            this.listItems.Add(iGuitarW);
            
            //add the action for this later, as it needs to be able to change all of the above buttons
            iGuitarAutoPlayAll.action = () =>
            {
                bool bAutoOn = iGuitarAutoPlayAll.e現在の状態 == CItemThreeState.E状態.ON;
                
                iGuitarR.bON = bAutoOn;
                iGuitarG.bON = bAutoOn;
                iGuitarB.bON = bAutoOn;
                iGuitarY.bON = bAutoOn;
                iGuitarP.bON = bAutoOn;
                iGuitarPick.bON = bAutoOn;
                iGuitarW.bON = bAutoOn;
            };

            this.iGuitarScrollSpeed = new CItemInteger("ScrollSpeed", 0, 0x7cf, CDTXMania.ConfigIni.nScrollSpeed.Guitar,
                "演奏時のギター譜面のスクロールの\n速度を指定します。\nx0.5 ～ x1000.0 までを指定可能です。",
                "To change the scroll speed for the\nguitar lanes.\nYou can set it from x0.5 to x1000.0.\n(ScrollSpeed=x0.5 means half speed)");
            this.iGuitarScrollSpeed.BindConfig(
                () => this.iGuitarScrollSpeed.nCurrentValue = CDTXMania.ConfigIni.nScrollSpeed.Guitar,
                () => CDTXMania.ConfigIni.nScrollSpeed.Guitar = this.iGuitarScrollSpeed.nCurrentValue);
            
            this.listItems.Add(iGuitarScrollSpeed);

            var iGuitarHIDSUD = new CItemList("HID-SUD", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.nHidSud.Guitar,
                "HIDDEN:チップが途中から見えなくなります。\n" +
                "SUDDEN:チップが途中まで見えません。\n" +
                "HID-SUD:HIDDEN、SUDDENの両方が適用\n" +
                "されます。\n" +
                "STEALTH:チップがずっと表示されません。",
                "The display position for Drums Combo.\n" +
                "Note that it doesn't take effect\n" +
                " at Autoplay ([Left] is forcely used).",
                new string[] { "OFF", "Hidden", "Sudden", "HidSud", "Stealth" });
            iGuitarHIDSUD.BindConfig(
                () => iGuitarHIDSUD.n現在選択されている項目番号 = CDTXMania.ConfigIni.nHidSud.Guitar,
                () => CDTXMania.ConfigIni.nHidSud.Guitar = iGuitarHIDSUD.n現在選択されている項目番号);
            this.listItems.Add(iGuitarHIDSUD);
            
            //----------DisplayOption----------
            var iGuitarDark = new CItemList("       Dark", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eDark,
                "レーン表示のオプションをまとめて切り替えます。\n" +
                "HALF: レーンが表示されなくなります。\n" +
                "FULL: さらに小節線、拍線、判定ラインも\n" +
                "表示されなくなります。",
                "OFF: all display parts are shown.\nHALF: lanes and gauge are\n disappeared.\nFULL: additionaly to HALF, bar/beat\n lines, hit bar are disappeared.",
                new string[] { "OFF", "HALF", "FULL" });
            this.listItems.Add(iGuitarDark);
            
            var iGuitarLaneDisp = new CItemList("LaneDisp", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.nLaneDisp.Guitar,
                "レーンの縦線と小節線の表示を切り替えます。\n" +
                "ALL  ON :レーン背景、小節線を表示します。\n" +
                "LANE OFF:レーン背景を表示しません。\n" +
                "LINE OFF:小節線を表示しません。\n" +
                "ALL  OFF:レーン背景、小節線を表示しません。",
                "",
                new string[] { "ALL ON", "LANE OFF", "LINE OFF", "ALL OFF" });
            iGuitarLaneDisp.BindConfig(
                () => iGuitarLaneDisp.n現在選択されている項目番号 = CDTXMania.ConfigIni.nLaneDisp.Guitar,
                () => CDTXMania.ConfigIni.nLaneDisp.Guitar = iGuitarLaneDisp.n現在選択されている項目番号);
            this.listItems.Add(iGuitarLaneDisp);

            var iGuitarJudgeLineDisp = new CItemToggle("JudgeLineDisp", CDTXMania.ConfigIni.bJudgeLineDisp.Guitar,
                "判定ラインの表示 / 非表示を切り替えます。",
                "Toggle JudgeLine");
            iGuitarJudgeLineDisp.BindConfig(
                () => iGuitarJudgeLineDisp.bON = CDTXMania.ConfigIni.bJudgeLineDisp.Guitar,
                () => CDTXMania.ConfigIni.bJudgeLineDisp.Guitar = iGuitarJudgeLineDisp.bON);
            this.listItems.Add(iGuitarJudgeLineDisp);

            var iGuitarLaneFlush = new CItemToggle("LaneFlush", CDTXMania.ConfigIni.bLaneFlush.Guitar,
                "レーンフラッシュの表示の表示 / 非表示を\n" +
                 "切り替えます。",
                "Toggle LaneFlush");
            iGuitarLaneFlush.BindConfig(
                () => iGuitarLaneFlush.bON = CDTXMania.ConfigIni.bLaneFlush.Guitar,
                () => CDTXMania.ConfigIni.bLaneFlush.Guitar = iGuitarLaneFlush.bON);
            this.listItems.Add(iGuitarLaneFlush);
            
            //add the action for this later, as it needs to be able to change all of the above buttons
            iGuitarDark.action = () =>
            {
                if (iGuitarDark.n現在選択されている項目番号 == (int)EDarkMode.FULL)
                {
                    iGuitarLaneDisp.n現在選択されている項目番号 = 3;
                    iGuitarJudgeLineDisp.bON = false;
                    iGuitarLaneFlush.bON = false;
                }
                else if (iGuitarDark.n現在選択されている項目番号 == (int)EDarkMode.HALF)
                {
                    iGuitarLaneDisp.n現在選択されている項目番号 = 1;
                    iGuitarJudgeLineDisp.bON = true;
                    iGuitarLaneFlush.bON = true;
                }
                else
                {
                    iGuitarLaneDisp.n現在選択されている項目番号 = 0;
                    iGuitarJudgeLineDisp.bON = true;
                    iGuitarLaneFlush.bON = true;
                }
            };

            var iGuitarAttackEffect = new CItemList("AttackEffect", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eAttackEffect.Guitar,
                 "アタックエフェクトの表示 / 非表示を\n" +
                 "切り替えます。",
                 "",
                new string[] { "ON", "OFF" });
            iGuitarAttackEffect.BindConfig(
                () => iGuitarAttackEffect.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eAttackEffect.Guitar,
                () => CDTXMania.ConfigIni.eAttackEffect.Guitar = (EType)iGuitarAttackEffect.n現在選択されている項目番号);
            this.listItems.Add(iGuitarAttackEffect);

            var iGuitarReverse = new CItemToggle("Reverse", CDTXMania.ConfigIni.bReverse.Guitar,
                "ギターチップが譜面の上から下に\n流れるようになります。",
                "The scroll way is reversed. Guitar chips\nflow from the top to the bottom.");
            iGuitarReverse.BindConfig(
                () => iGuitarReverse.bON = CDTXMania.ConfigIni.bReverse.Guitar,
                () => CDTXMania.ConfigIni.bReverse.Guitar = iGuitarReverse.bON);
            this.listItems.Add(iGuitarReverse);

            //コンボ表示

            //RISKY

            var iGuitarPosition = new CItemList("Position", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.JudgementStringPosition.Guitar,
                "ギターの判定文字の表示位置を指定\nします。\n  P-A: OnTheLane\n  P-B: COMBO の下\n  P-C: 判定ライン上\n  OFF: 表示しない",
                "The position to show judgement mark.\n(Perfect, Great, ...)\n\n P-A: on the lanes.\n P-B: under the COMBO indication.\n P-C: on the JudgeLine.\n OFF: no judgement mark.",
                new string[] { "P-A", "P-B", "P-C", "OFF" });
            iGuitarPosition.BindConfig(
                () => iGuitarPosition.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.JudgementStringPosition.Guitar,
                () => CDTXMania.ConfigIni.JudgementStringPosition.Guitar = (EType)iGuitarPosition.n現在選択されている項目番号);
            this.listItems.Add(iGuitarPosition);

            //実機ではここにオートオプションが入る。

            var iGuitarLight = new CItemToggle("Light", CDTXMania.ConfigIni.bLight.Guitar,
                "ギターチップのないところでピッキングしても\n BAD になりません。",
                "Even if you pick without any chips,\nit doesn't become BAD.");
            iGuitarLight.BindConfig(
                () => iGuitarLight.bON = CDTXMania.ConfigIni.bLight.Guitar,
                () => CDTXMania.ConfigIni.bLight.Guitar = iGuitarLight.bON);
            this.listItems.Add(iGuitarLight);

            var iGuitarSpecialist = new CItemList("Performance Mode", CItemBase.EPanelType.Normal, CDTXMania.ConfigIni.bSpecialist.Guitar ? 1 : 0,
                "ギターの演奏・モード\nします。\n  Normal: 通常の演奏モードです\n  Specialist: 間違えると違う音が流れます",
                "Turn on/off Specialist Mode for Guitar\n Normal: Default Performance mode\n Specialist: Different sound is played when you make a mistake",
                new string[] { "Normal", "Specialist" });
            iGuitarSpecialist.BindConfig(
                () => iGuitarSpecialist.n現在選択されている項目番号 = CDTXMania.ConfigIni.bSpecialist.Guitar ? 1 : 0,
                () => CDTXMania.ConfigIni.bSpecialist.Guitar = iGuitarSpecialist.n現在選択されている項目番号 == 1);
            this.listItems.Add(iGuitarSpecialist);

            var iGuitarRandom = new CItemList("Random", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eRandom.Guitar,
                "ギターのチップがランダムに降ってきます。\n  Mirror: ミラーをかけます\n  Part: 小節_レーン単位で交換\n  Super: チップ単位で交換\n  Hyper: 全部完全に変更",
                "Guitar chips come randomly.\n Mirror: \n Part: swapping lanes randomly for each\n  measures.\n Super: swapping chip randomly\n Hyper: swapping randomly\n  (number of lanes also changes)",
                new string[] { "OFF", "Mirror", "Part", "Super", "Hyper" });
            iGuitarRandom.BindConfig(
                () => iGuitarRandom.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eRandom.Guitar,
                () => CDTXMania.ConfigIni.eRandom.Guitar = (ERandomMode)iGuitarRandom.n現在選択されている項目番号);
            this.listItems.Add(iGuitarRandom);

            //NumOfLanes(ここではレーンオ－トに相当する。)
            //バイブオプション(実装不可)
            //StageEffect

            var iGuitarLeft = new CItemToggle("Left", CDTXMania.ConfigIni.bLeft.Guitar,
                "ギターの RGBYP の並びが左右反転します。\n（左利きモード）",
                "Lane order 'R-G-B-Y-P' becomes 'P-Y-B-G-R'\nfor lefty.");
            iGuitarLeft.BindConfig(
                () => iGuitarLeft.bON = CDTXMania.ConfigIni.bLeft.Guitar,
                () => CDTXMania.ConfigIni.bLeft.Guitar = iGuitarLeft.bON);
            this.listItems.Add(iGuitarLeft);

            var iGuitarJudgeLinePos = new CItemInteger("JudgeLinePos", 0, 100, CDTXMania.ConfigIni.nJudgeLine.Guitar,
                "演奏時の判定ラインの高さを変更します。\n" +
                "0～100の間で指定できます。",
                "To change the judgeLinePosition for the\n" +
                "You can set it from 0 to 100.");
            iGuitarJudgeLinePos.BindConfig(
                () => iGuitarJudgeLinePos.nCurrentValue = CDTXMania.ConfigIni.nJudgeLine.Guitar,
                () => CDTXMania.ConfigIni.nJudgeLine.Guitar = iGuitarJudgeLinePos.nCurrentValue);
            this.listItems.Add(iGuitarJudgeLinePos);

            //比較対象(そもそも比較グラフさえ完成していない)
            //シャッタータイプ
            var iGuitarShutterInPos = new CItemInteger("ShutterInPos", 0, 100, CDTXMania.ConfigIni.nShutterInSide.Guitar,
                "演奏時のノーツが現れる側のシャッターの\n" +
                "位置を変更します。",
                "\n" +
                "\n" +
                "");
            iGuitarShutterInPos.BindConfig(
                () => iGuitarShutterInPos.nCurrentValue = CDTXMania.ConfigIni.nShutterInSide.Guitar,
                () => CDTXMania.ConfigIni.nShutterInSide.Guitar = iGuitarShutterInPos.nCurrentValue);
            this.listItems.Add(iGuitarShutterInPos);

            var iGuitarShutterOutPos = new CItemInteger("ShutterOutPos", 0, 100, CDTXMania.ConfigIni.nShutterOutSide.Guitar,
                "演奏時のノーツが消える側のシャッターの\n" +
                "位置を変更します。",
                "\n" +
                "\n" +
                "");
            iGuitarShutterOutPos.BindConfig(
                () => iGuitarShutterOutPos.nCurrentValue = CDTXMania.ConfigIni.nShutterOutSide.Guitar,
                () => CDTXMania.ConfigIni.nShutterOutSide.Guitar = iGuitarShutterOutPos.nCurrentValue);
            this.listItems.Add(iGuitarShutterOutPos);
            
            var iSystemSoundMonitorGuitar = new CItemToggle("GuitarMonitor", CDTXMania.ConfigIni.b演奏音を強調する.Guitar,
                "ギター音モニタ：\nギター音を他の音より大きめの音量で\n発声します。\nただし、オートプレイの場合は通常音量で\n発声されます。",
                "To enhance the guitar chip sound\n(except autoplay).");
            iSystemSoundMonitorGuitar.BindConfig(
                () => iSystemSoundMonitorGuitar.bON = CDTXMania.ConfigIni.b演奏音を強調する.Guitar,
                () => CDTXMania.ConfigIni.b演奏音を強調する.Guitar = iSystemSoundMonitorGuitar.bON);
            this.listItems.Add(iSystemSoundMonitorGuitar);

            var iSystemMinComboGuitar = new CItemInteger("G-MinCombo", 0, 0x1869f, CDTXMania.ConfigIni.n表示可能な最小コンボ数.Guitar,
                "表示可能な最小コンボ数（ギター）：\n画面に表示されるコンボの最小の数を\n指定します。\n1 ～ 99999 の値が指定可能です。\n0にするとコンボを表示しません。",
                "Initial number to show the combo\n for the guitar.\nYou can specify from 1 to 99999.");
            iSystemMinComboGuitar.BindConfig(
                () => iSystemMinComboGuitar.nCurrentValue = CDTXMania.ConfigIni.n表示可能な最小コンボ数.Guitar,
                () => CDTXMania.ConfigIni.n表示可能な最小コンボ数.Guitar = iSystemMinComboGuitar.nCurrentValue);
            this.listItems.Add(iSystemMinComboGuitar);

			this.iGuitarGraph = new CItemToggle( "Graph", CDTXMania.ConfigIni.bGraph有効.Guitar,
				"最高スキルと比較できるグラフを表示します。\n" +
				"オートプレイだと表示されません。\n" +
                "この項目を有効にすると、ベースパートのグラフは\n" +
                "無効になります。",
				"To draw Graph or not." );
            this.iGuitarGraph.action = () =>
            {
                if (iGuitarGraph.bON)
                {
                    CDTXMania.ConfigIni.bGraph有効.Bass = false;
                    this.iBassGraph.bON = false;
                }
            };
            this.iGuitarGraph.BindConfig(
                () => this.iGuitarGraph.bON = CDTXMania.ConfigIni.bGraph有効.Guitar,
                () => CDTXMania.ConfigIni.bGraph有効.Guitar = this.iGuitarGraph.bON);
			this.listItems.Add(this.iGuitarGraph);

            // #23580 2011.1.3 yyagi
            var iGuitarInputAdjustTimeMs = new CItemInteger("InputAdjust", -99, 99, CDTXMania.ConfigIni.nInputAdjustTimeMs.Guitar,
                "ギターの入力タイミングの微調整を行います。\n-99 ～ 99ms まで指定可能です。\n入力ラグを軽減するためには、\n負の値を指定してください。",
                "To adjust the guitar input timing.\nYou can set from -99 to 0ms.\nTo decrease input lag, set minus value.");
            iGuitarInputAdjustTimeMs.BindConfig(
                () => iGuitarInputAdjustTimeMs.nCurrentValue = CDTXMania.ConfigIni.nInputAdjustTimeMs.Guitar,
                () => CDTXMania.ConfigIni.nInputAdjustTimeMs.Guitar = iGuitarInputAdjustTimeMs.nCurrentValue);
            this.listItems.Add(iGuitarInputAdjustTimeMs);

            var iGuitarGoToKeyAssign = new CItemBase("Guitar Keys", CItemBase.EPanelType.Normal,
                "ギターのキー入力に関する項目を設定します。",
                "Settings for the guitar key/pad inputs.");
            iGuitarGoToKeyAssign.action = t項目リストの設定_KeyAssignGuitar;
            this.listItems.Add(iGuitarGoToKeyAssign);

            OnListMenuの初期化();
            this.nCurrentSelection = 0;
            this.eMenuType = EMenuType.Guitar;
        }
        #endregion
        #region [ t項目リストの設定_Bass() ]
        public void tSetupItemList_Bass()
        {
            this.tRecordToConfigIni();
            this.listItems.Clear();

            // #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

            this.iBassReturnToMenu = new CItemBase("<< Return To Menu", CItemBase.EPanelType.Other,
                "左側のメニューに戻ります。",
                "Return to left menu.");
            this.listItems.Add(iBassReturnToMenu);

            var iBassAutoPlayAll = new CItemThreeState("AutoPlay (All)", CItemThreeState.E状態.不定,
                "全ネック/ピックの自動演奏の ON/OFF を\n" +
                "まとめて切り替えます。",
                "You can change whether Auto or not\n" +
                " for all bass neck/pick at once.");
            this.listItems.Add(iBassAutoPlayAll);
            
            var iBassR = new CItemToggle("    R", CDTXMania.ConfigIni.bAutoPlay.BsR,
                "Rネックを自動で演奏します。",
                "To play R neck automatically.");
            iBassR.BindConfig(
                () => iBassR.bON = CDTXMania.ConfigIni.bAutoPlay.BsR,
                () => CDTXMania.ConfigIni.bAutoPlay.BsR = iBassR.bON);
            this.listItems.Add(iBassR);

            var iBassG = new CItemToggle("    G", CDTXMania.ConfigIni.bAutoPlay.BsG,
                "Gネックを自動で演奏します。",
                "To play G neck automatically.");
            iBassG.BindConfig(
                () => iBassG.bON = CDTXMania.ConfigIni.bAutoPlay.BsG,
                () => CDTXMania.ConfigIni.bAutoPlay.BsG = iBassG.bON);
            this.listItems.Add(iBassG);

            var iBassB = new CItemToggle("    B", CDTXMania.ConfigIni.bAutoPlay.BsB,
                "Bネックを自動で演奏します。",
                "To play B neck automatically.");
            iBassB.BindConfig(
                () => iBassB.bON = CDTXMania.ConfigIni.bAutoPlay.BsB,
                () => CDTXMania.ConfigIni.bAutoPlay.BsB = iBassB.bON);
            this.listItems.Add(iBassB);

            var iBassY = new CItemToggle("    Y", CDTXMania.ConfigIni.bAutoPlay.BsY,
                "Yネックを自動で演奏します。",
                "To play Y neck automatically.");
            iBassY.BindConfig(
                () => iBassY.bON = CDTXMania.ConfigIni.bAutoPlay.BsY,
                () => CDTXMania.ConfigIni.bAutoPlay.BsY = iBassY.bON);
            this.listItems.Add(iBassY);

            var iBassP = new CItemToggle("    P", CDTXMania.ConfigIni.bAutoPlay.BsP,
                "Pネックを自動で演奏します。",
                "To play P neck automatically.");
            iBassP.BindConfig(
                () => iBassP.bON = CDTXMania.ConfigIni.bAutoPlay.BsP,
                () => CDTXMania.ConfigIni.bAutoPlay.BsP = iBassP.bON);
            this.listItems.Add(iBassP);

            var iBassPick = new CItemToggle("    Pick", CDTXMania.ConfigIni.bAutoPlay.BsPick,
                "ピックを自動で演奏します。",
                "To play Pick automatically.");
            iBassPick.BindConfig(
                () => iBassPick.bON = CDTXMania.ConfigIni.bAutoPlay.BsPick,
                () => CDTXMania.ConfigIni.bAutoPlay.BsPick = iBassPick.bON);
            this.listItems.Add(iBassPick);

            var iBassW = new CItemToggle("    Wailing", CDTXMania.ConfigIni.bAutoPlay.BsW,
                "ウェイリングを自動で演奏します。",
                "To play wailing automatically.");
            iBassW.BindConfig(
                () => iBassW.bON = CDTXMania.ConfigIni.bAutoPlay.BsW,
                () => CDTXMania.ConfigIni.bAutoPlay.BsW = iBassW.bON);
            this.listItems.Add(iBassW);
            
            //add the action for this later, as it needs to be able to change all of the above buttons
            iBassAutoPlayAll.action = () =>
            {
                bool bAutoOn = iBassAutoPlayAll.e現在の状態 == CItemThreeState.E状態.ON;

                iBassR.bON = bAutoOn;
                iBassG.bON = bAutoOn;
                iBassB.bON = bAutoOn;
                iBassY.bON = bAutoOn;
                iBassP.bON = bAutoOn;
                iBassPick.bON = bAutoOn;
                iBassW.bON = bAutoOn;
            };

            this.iBassScrollSpeed = new CItemInteger("ScrollSpeed", 0, 0x7cf, CDTXMania.ConfigIni.nScrollSpeed.Bass,
                "演奏時のベース譜面のスクロールの\n速度を指定します。\nx0.5 ～ x1000.0 までを指定可能です。",
                "To change the scroll speed for the\nbass lanes.\nYou can set it from x0.5 to x1000.0.\n(ScrollSpeed=x0.5 means half speed)");
            this.iBassScrollSpeed.BindConfig(
                () => this.iBassScrollSpeed.nCurrentValue = CDTXMania.ConfigIni.nScrollSpeed.Bass,
                () => CDTXMania.ConfigIni.nScrollSpeed.Bass = this.iBassScrollSpeed.nCurrentValue);
            this.listItems.Add(this.iBassScrollSpeed);

            var iBassHIDSUD = new CItemList("HID-SUD", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.nHidSud.Bass,
                "HIDDEN:チップが途中から見えなくなります。\n" +
                "SUDDEN:チップが途中まで見えません。\n" +
                "HID-SUD:HIDDEN、SUDDENの両方が適用\n" +
                "されます。\n" +
                "STEALTH:チップがずっと表示されません。",
                "The display position for Drums Combo.\n" +
                "Note that it doesn't take effect\n" +
                " at Autoplay ([Left] is forcely used).",
                new string[] { "OFF", "Hidden", "Sudden", "HidSud", "Stealth" });
            iBassHIDSUD.BindConfig(
                () => iBassHIDSUD.n現在選択されている項目番号 = CDTXMania.ConfigIni.nHidSud.Bass,
                () => CDTXMania.ConfigIni.nHidSud.Bass = iBassHIDSUD.n現在選択されている項目番号);
            this.listItems.Add(iBassHIDSUD);

            //----------DisplayOption----------
            var iBassDark = new CItemList("       Dark", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eDark,
                "レーン表示のオプションをまとめて切り替えます。\n" +
                "HALF: レーンが表示されなくなります。\n" +
                "FULL: さらに小節線、拍線、判定ラインも\n" +
                "表示されなくなります。",
                "OFF: all display parts are shown.\nHALF: lanes and gauge are\n disappeared.\nFULL: additionaly to HALF, bar/beat\n lines, hit bar are disappeared.",
                new string[] { "OFF", "HALF", "FULL" });
            this.listItems.Add(iBassDark);
            
            var iBassLaneDisp = new CItemList("LaneDisp", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.nLaneDisp.Bass,
                "レーンの縦線と小節線の表示を切り替えます。\n" +
                "ALL  ON :レーン背景、小節線を表示します。\n" +
                "LANE OFF:レーン背景を表示しません。\n" +
                "LINE OFF:小節線を表示しません。\n" +
                "ALL  OFF:レーン背景、小節線を表示しません。",
                "",
                new string[] { "ALL ON", "LANE OFF", "LINE OFF", "ALL OFF" });
            iBassLaneDisp.BindConfig(
                () => iBassLaneDisp.n現在選択されている項目番号 = CDTXMania.ConfigIni.nLaneDisp.Bass,
                () => CDTXMania.ConfigIni.nLaneDisp.Bass = iBassLaneDisp.n現在選択されている項目番号);
            this.listItems.Add(iBassLaneDisp);

            var iBassJudgeLineDisp = new CItemToggle("JudgeLineDisp", CDTXMania.ConfigIni.bJudgeLineDisp.Bass,
                "判定ラインの表示 / 非表示を切り替えます。",
                "Toggle JudgeLine");
            iBassJudgeLineDisp.BindConfig(
                () => iBassJudgeLineDisp.bON = CDTXMania.ConfigIni.bJudgeLineDisp.Bass,
                () => CDTXMania.ConfigIni.bJudgeLineDisp.Bass = iBassJudgeLineDisp.bON);
            this.listItems.Add(iBassJudgeLineDisp);

            var iBassLaneFlush = new CItemToggle("LaneFlush", CDTXMania.ConfigIni.bLaneFlush.Bass,
                "レーンフラッシュの表示 / 非表示を\n" +
                 "切り替えます。",
                "Toggle LaneFlush");
            iBassLaneFlush.BindConfig(
                () => iBassLaneFlush.bON = CDTXMania.ConfigIni.bLaneFlush.Bass,
                () => CDTXMania.ConfigIni.bLaneFlush.Bass = iBassLaneFlush.bON);
            this.listItems.Add(iBassLaneFlush);
            
            //add the action for this later, as it needs to be able to change all of the above buttons
            iBassDark.action = () =>
            {
                if (iBassDark.n現在選択されている項目番号 == (int)EDarkMode.FULL)
                {
                    iBassLaneDisp.n現在選択されている項目番号 = 3;
                    iBassJudgeLineDisp.bON = false;
                    iBassLaneFlush.bON = false;
                }
                else if (iBassDark.n現在選択されている項目番号 == (int)EDarkMode.HALF)
                {
                    iBassLaneDisp.n現在選択されている項目番号 = 1;
                    iBassJudgeLineDisp.bON = true;
                    iBassLaneFlush.bON = true;
                }
                else
                {
                    iBassLaneDisp.n現在選択されている項目番号 = 0;
                    iBassJudgeLineDisp.bON = true;
                    iBassLaneFlush.bON = true;
                }
            };

            var iBassAttackEffect = new CItemList("AttackEffect", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eAttackEffect.Bass,
                 "アタックエフェクトの表示 / 非表示を\n" +
                 "切り替えます。",
                 "",
                 new string[] { "ON", "OFF" });
            iBassAttackEffect.BindConfig(
                () => iBassAttackEffect.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eAttackEffect.Bass,
                () => CDTXMania.ConfigIni.eAttackEffect.Bass = (EType)iBassAttackEffect.n現在選択されている項目番号);
            this.listItems.Add(iBassAttackEffect);


            var iBassReverse = new CItemToggle("Reverse", CDTXMania.ConfigIni.bReverse.Bass,
                "ベースチップが譜面の上から下に\n流れるようになります。",
                "The scroll way is reversed. Bass chips\nflow from the top to the bottom.");
            iBassReverse.BindConfig(
                () => iBassReverse.bON = CDTXMania.ConfigIni.bReverse.Bass,
                () => CDTXMania.ConfigIni.bReverse.Bass = iBassReverse.bON);
            this.listItems.Add(iBassReverse);

            var iBassPosition = new CItemList("Position", CItemBase.EPanelType.Normal,
                (int)CDTXMania.ConfigIni.JudgementStringPosition.Bass,
                "ベースの判定文字の表示位置を指定します。\n  P-A: OnTheLane\n  P-B: COMBO の下\n  P-C: 判定ライン上\n  OFF: 表示しない",
                "The position to show judgement mark.\n(Perfect, Great, ...)\n\n P-A: on the lanes.\n P-B: under the COMBO indication.\n P-C: on the JudgeLine.\n OFF: no judgement mark.",
                new string[] { "P-A", "P-B", "P-C", "OFF" });
            iBassPosition.BindConfig(
                () => iBassPosition.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.JudgementStringPosition.Bass,
                () => CDTXMania.ConfigIni.JudgementStringPosition.Bass = (EType)iBassPosition.n現在選択されている項目番号);
            this.listItems.Add(iBassPosition);

            var iBassRandom = new CItemList("Random", CItemBase.EPanelType.Normal,
                (int)CDTXMania.ConfigIni.eRandom.Bass,
                "ベースのチップがランダムに降ってきます。\n  Mirror: ミラーをかけます\n  Part: 小節_レーン単位で交換\n  Super: チップ単位で交換\n  Hyper: 全部完全に変更",
                "Bass chips come randomly.\n Mirror: \n Part: swapping lanes randomly for each\n  measures.\n Super: swapping chip randomly\n Hyper: swapping randomly\n  (number of lanes also changes)",
                new string[] { "OFF", "Mirror", "Part", "Super", "Hyper" });
            iBassRandom.BindConfig(
                () => iBassRandom.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eRandom.Bass,
                () => CDTXMania.ConfigIni.eRandom.Bass = (ERandomMode)iBassRandom.n現在選択されている項目番号);
            this.listItems.Add(iBassRandom);

            var iBassLight = new CItemToggle("Light", CDTXMania.ConfigIni.bLight.Bass,
                "ベースチップのないところでピッキングしても\n BAD になりません。",
                "Even if you pick without any chips,\nit doesn't become BAD.");
            iBassLight.BindConfig(
                () => iBassLight.bON = CDTXMania.ConfigIni.bLight.Bass,
                () => CDTXMania.ConfigIni.bLight.Bass = iBassLight.bON);
            this.listItems.Add(iBassLight);

            var iBassSpecialist = new CItemList("Performance Mode", CItemBase.EPanelType.Normal, CDTXMania.ConfigIni.bSpecialist.Bass ? 1 : 0,
                "ベースの演奏・モード\nします。\n  Normal: 通常の演奏モードです\n  Specialist: 間違えると違う音が流れます",
                "Turn on/off Specialist Mode for Bass\n Normal: Default Performance mode\n Specialist: Different sound is played when you make a mistake",
                new string[] { "Normal", "Specialist" });
            iBassSpecialist.BindConfig(
                () => iBassSpecialist.n現在選択されている項目番号 = CDTXMania.ConfigIni.bSpecialist.Bass ? 1 : 0,
                () => CDTXMania.ConfigIni.bSpecialist.Bass = iBassSpecialist.n現在選択されている項目番号 == 1);
            this.listItems.Add(iBassSpecialist);

            var iBassLeft = new CItemToggle("Left", CDTXMania.ConfigIni.bLeft.Bass,
                "ベースの RGBYP の並びが左右反転します。\n（左利きモード）",
                "Lane order 'R-G-B-Y-P' becomes 'P-Y-B-G-R'\nfor lefty.");
            iBassLeft.BindConfig(
                () => iBassLeft.bON = CDTXMania.ConfigIni.bLeft.Bass,
                () => CDTXMania.ConfigIni.bLeft.Bass = iBassLeft.bON);
            this.listItems.Add(iBassLeft);

            var iSystemSoundMonitorBass = new CItemToggle("BassMonitor", CDTXMania.ConfigIni.b演奏音を強調する.Bass,
            "ベース音モニタ：\nベース音を他の音より大きめの音量で\n発声します。\nただし、オートプレイの場合は通常音量で\n発声されます。",
            "To enhance the bass chip sound\n(except autoplay).");
            iSystemSoundMonitorBass.BindConfig(
                () => iSystemSoundMonitorBass.bON = CDTXMania.ConfigIni.b演奏音を強調する.Bass,
                () => CDTXMania.ConfigIni.b演奏音を強調する.Bass = iSystemSoundMonitorBass.bON);
            this.listItems.Add(iSystemSoundMonitorBass);

            var iSystemMinComboBass = new CItemInteger("B-MinCombo", 0, 0x1869f, CDTXMania.ConfigIni.n表示可能な最小コンボ数.Bass,
                "表示可能な最小コンボ数（ベース）：\n画面に表示されるコンボの最小の数\nを指定します。\n1 ～ 99999 の値が指定可能です。\n0にするとコンボを表示しません。",
                "Initial number to show the combo\n for the bass.\nYou can specify from 1 to 99999.");
            iSystemMinComboBass.BindConfig(
                () => iSystemMinComboBass.nCurrentValue = CDTXMania.ConfigIni.n表示可能な最小コンボ数.Bass,
                () => CDTXMania.ConfigIni.n表示可能な最小コンボ数.Bass = iSystemMinComboBass.nCurrentValue);
            this.listItems.Add(iSystemMinComboBass);

            var iBassJudgeLinePos = new CItemInteger("JudgeLinePos", 0, 100, CDTXMania.ConfigIni.nJudgeLine.Bass,
                "演奏時の判定ラインの高さを変更します。\n" +
                "0～100の間で指定できます。",
                "To change the judgeLinePosition for the\n" +
                "You can set it from 0 to 100.");
            iBassJudgeLinePos.BindConfig(
                () => iBassJudgeLinePos.nCurrentValue = CDTXMania.ConfigIni.nJudgeLine.Bass,
                () => CDTXMania.ConfigIni.nJudgeLine.Bass = iBassJudgeLinePos.nCurrentValue);
            this.listItems.Add(iBassJudgeLinePos);

            var iBassShutterInPos = new CItemInteger("ShutterInPos", 0, 100, CDTXMania.ConfigIni.nShutterInSide.Bass,
                "演奏時のノーツが現れる側のシャッターの\n" +
                "位置を変更します。",
                "\n" +
                "\n" +
                "");
            iBassShutterInPos.BindConfig(
                () => iBassShutterInPos.nCurrentValue = CDTXMania.ConfigIni.nShutterInSide.Bass,
                () => CDTXMania.ConfigIni.nShutterInSide.Bass = iBassShutterInPos.nCurrentValue);
            this.listItems.Add(iBassShutterInPos);

            var iBassShutterOutPos = new CItemInteger("ShutterOutPos", 0, 100, CDTXMania.ConfigIni.nShutterOutSide.Bass,
                "演奏時のノーツが消える側のシャッターの\n" +
                "位置を変更します。",
                "\n" +
                "\n" +
                "");
            iBassShutterOutPos.BindConfig(
                () => iBassShutterOutPos.nCurrentValue = CDTXMania.ConfigIni.nShutterOutSide.Bass,
                () => CDTXMania.ConfigIni.nShutterOutSide.Bass = iBassShutterOutPos.nCurrentValue);
            this.listItems.Add(iBassShutterOutPos);

			this.iBassGraph = new CItemToggle( "Graph", CDTXMania.ConfigIni.bGraph有効.Bass,
				"最高スキルと比較できるグラフを表示します。\n" +
				"オートプレイだと表示されません。\n" +
                "この項目を有効にすると、ギターパートのグラフは\n" +
                "無効になります。",
				"To draw Graph or not." );
            iBassGraph.BindConfig(
                () => iBassGraph.bON = CDTXMania.ConfigIni.bGraph有効.Bass,
                () => CDTXMania.ConfigIni.bGraph有効.Bass = iBassGraph.bON);
            iBassGraph.action = () =>
            {
                if (this.iBassGraph.bON == true)
                {
                    CDTXMania.ConfigIni.bGraph有効.Guitar = false;
                    this.iGuitarGraph.bON = false;
                }
            };
            this.listItems.Add( iBassGraph );

            // #23580 2011.1.3 yyagi
            var iBassInputAdjustTimeMs = new CItemInteger("InputAdjust", -99, 99, CDTXMania.ConfigIni.nInputAdjustTimeMs.Bass,
                "ベースの入力タイミングの微調整を行います。\n-99 ～ 99ms まで指定可能です。\n入力ラグを軽減するためには、\n負の値を指定してください。",
                "To adjust the bass input timing.\nYou can set from -99 to 0ms.\nTo decrease input lag, set minus value.");
            iBassInputAdjustTimeMs.BindConfig(
                () => iBassInputAdjustTimeMs.nCurrentValue = CDTXMania.ConfigIni.nInputAdjustTimeMs.Bass,
                () => CDTXMania.ConfigIni.nInputAdjustTimeMs.Bass = iBassInputAdjustTimeMs.nCurrentValue);
            this.listItems.Add(iBassInputAdjustTimeMs);

            var iBassGoToKeyAssign = new CItemBase("Bass Keys", CItemBase.EPanelType.Normal,
                "ベースのキー入力に関する項目を設定します。",
                "Settings for the bass key/pad inputs.");
            iBassGoToKeyAssign.action = t項目リストの設定_KeyAssignBass;
            this.listItems.Add(iBassGoToKeyAssign);

            OnListMenuの初期化();
            this.nCurrentSelection = 0;
            this.eMenuType = EMenuType.Bass;
        }
        #endregion

        /// <summary>
        /// ESC押下時の右メニュー描画
        /// </summary>
        public void tPressEsc()
        {
            if (this.eMenuType == EMenuType.KeyAssignSystem)
            {
                tSetupItemList_System();
            }
            else if (this.eMenuType == EMenuType.KeyAssignDrums)
            {
                tSetupItemList_Drums();
            }
            else if (this.eMenuType == EMenuType.KeyAssignGuitar)
            {
                tSetupItemList_Guitar();
            }
            else if (this.eMenuType == EMenuType.KeyAssignBass)
            {
                tSetupItemList_Bass();
            }
            // これ以外なら何もしない
        }
        public void tPressEnter()
        {
            CDTXMania.Skin.soundDecide.tPlay();
            
            if (this.bFocusIsOnElementValue)
            {
                this.bFocusIsOnElementValue = false;
            }
            else if (this.listItems[this.nCurrentSelection].eType == CItemBase.EType.Integer)
            {
                this.bFocusIsOnElementValue = true;
            }
            else
            {
                // Enter押下後の後処理
                this.listItems[this.nCurrentSelection].RunAction();
            }
        }   

        private void tGenerateSkinSample()
        {
            nSkinIndex = ((CItemList)this.listItems[this.nCurrentSelection]).n現在選択されている項目番号;
            if (nSkinSampleIndex != nSkinIndex)
            {
                string path = skinSubFolders[nSkinIndex];
                path = System.IO.Path.Combine(path, @"Graphics\2_background.jpg");
                Bitmap bmSrc = new Bitmap(path);
                Bitmap bmDest = new Bitmap(1280, 720);
                Graphics g = Graphics.FromImage(bmDest);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmSrc, new Rectangle(60, 106, (int)(1280 * 0.1984), (int)(720 * 0.1984)),
                    0, 0, 1280, 720, GraphicsUnit.Pixel);
                if (txSkinSample1 != null)
                {
                    CDTXMania.t安全にDisposeする(ref txSkinSample1);
                }
                txSkinSample1 = CDTXMania.tGenerateTexture(bmDest, false);
                g.Dispose();
                bmDest.Dispose();
                bmSrc.Dispose();
                nSkinSampleIndex = nSkinIndex;
            }
        }

        #region [ 項目リストの設定 ( Exit, KeyAssignSystem/Drums/Guitar/Bass) ]
        public void tSetupItemList_Exit()
        {
            this.tRecordToConfigIni();
            this.eMenuType = EMenuType.Unknown;
        }
        public void tSetupItemList_KeyAssignSystem()
        {
            //this.tRecordToConfigIni();
            this.listItems.Clear();

            // #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

            var iKeyAssignSystemReturnToMenu = new CItemBase("<< ReturnTo Menu", CItemBase.EPanelType.Other,
                "左側のメニューに戻ります。",
                "Return to left menu.");
            iKeyAssignSystemReturnToMenu.action = tSetupItemList_System;
            this.listItems.Add(iKeyAssignSystemReturnToMenu);

            var iKeyAssignSystemCapture = new CItemBase("Capture",
                "キャプチャキー設定：\n画面キャプチャのキーの割り当てを設\n定します。",
                "Capture key assign:\nTo assign key for screen capture.\n (You can use keyboard only. You can't\nuse pads to capture screenshot.");
            this.listItems.Add(iKeyAssignSystemCapture);

            var iKeyAssignSystemSearch = new CItemBase("Search",
                "サーチボタンのキー設定：\nサーチボタンへのキーの割り当\nてを設定します。",
                "Search button key assign:\nTo assign key for Search Button.");
            this.listItems.Add(iKeyAssignSystemSearch);

            var iKeyAssignGuitarHelp = new CItemBase("Help",
                "ヘルプボタンのキー設定：\nヘルプボタンへのキーの割り当\nてを設定します。",
                "Help button key assign:\nTo assign key/pads for Help button.");
            iKeyAssignGuitarHelp.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.GUITAR, EKeyConfigPad.Help);
            this.listItems.Add(iKeyAssignGuitarHelp);

            var iKeyAssignBassHelp = new CItemBase("Pause",
                "一時停止キー設定：\n 一時停止キーの割り当てを設定します。",
                "Pause key assign:\n To assign key/pads for Pause button.");
            this.listItems.Add(iKeyAssignBassHelp);

            var iKeyAssignSystemLoopCreate = new CItemBase("Loop Create",
                "",
                "Loop Create assign:\n To assign key/pads for loop creation.");
            this.listItems.Add(iKeyAssignSystemLoopCreate);

            var iKeyAssignSystemLoopDelete = new CItemBase("Loop Delete",
                "",
                "Pause key assign:\n To assign key/pads for loop deletion.");
            this.listItems.Add(iKeyAssignSystemLoopDelete);

            var iKeyAssignSystemSkipForward = new CItemBase("Skip forward",
                "",
                "Skip forward assign:\n To assign key/pads for Skip forward.");
            this.listItems.Add(iKeyAssignSystemSkipForward);

            var iKeyAssignSystemSkipBackward = new CItemBase("Skip backward",
                "",
                "Skip backward assign:\n To assign key/pads for Skip backward (rewind).");
            this.listItems.Add(iKeyAssignSystemSkipBackward);

            var iKeyAssignSystemIncreasePlaySpeed = new CItemBase("Increase play speed",
                "",
                "Increase play speed assign:\n To assign key/pads for increasing play speed.");
            this.listItems.Add(iKeyAssignSystemIncreasePlaySpeed);

            var iKeyAssignSystemDecreasePlaySpeed = new CItemBase("Decrease play speed",
                "",
                "Decrease play speed assign:\n To assign key/pads for decreasing play speed.");
            this.listItems.Add(iKeyAssignSystemDecreasePlaySpeed);

            var iKeyAssignSystemRestart = new CItemBase("Restart",
                "",
                "Restart assign:\n To assign key/pads for Restart button.");
            this.listItems.Add(iKeyAssignSystemRestart);

            OnListMenuの初期化();
            this.nCurrentSelection = 0;
            this.eMenuType = EMenuType.KeyAssignSystem;
        }
        public void t項目リストの設定_KeyAssignDrums()
        {
            //			this.tRecordToConfigIni();
            this.listItems.Clear();

            // #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

            var iKeyAssignDrumsReturnToMenu = new CItemBase("<< ReturnTo Menu", CItemBase.EPanelType.Other,
                "左側のメニューに戻ります。",
                "Return to left menu.");
            iKeyAssignDrumsReturnToMenu.action = tSetupItemList_Drums;
            this.listItems.Add(iKeyAssignDrumsReturnToMenu);

            var iKeyAssignDrumsLC = new CItemBase("LeftCymbal",
                "ドラムのキー設定：\n左シンバルへのキーの割り当てを設\n定します。",
                "Drums key assign:\nTo assign key/pads for LeftCymbal\n button.");
            iKeyAssignDrumsLC.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.LC);
            this.listItems.Add(iKeyAssignDrumsLC);

            var iKeyAssignDrumsHHC = new CItemBase("HiHat(Close)",
                "ドラムのキー設定：\nハイハット（クローズ）へのキーの割り\n当てを設定します。",
                "Drums key assign:\nTo assign key/pads for HiHat(Close)\n button.");
            iKeyAssignDrumsHHC.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.HH);
            this.listItems.Add(iKeyAssignDrumsHHC);

            var iKeyAssignDrumsHHO = new CItemBase("HiHat(Open)",
                "ドラムのキー設定：\nハイハット（オープン）へのキーの割り\n当てを設定します。",
                "Drums key assign:\nTo assign key/pads for HiHat(Open)\n button.");
            iKeyAssignDrumsHHO.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.HHO);
            this.listItems.Add(iKeyAssignDrumsHHO);

            var iKeyAssignDrumsSD = new CItemBase("Snare",
                "ドラムのキー設定：\nスネアへのキーの割り当てを設定し\nます。",
                "Drums key assign:\nTo assign key/pads for Snare button.");
            iKeyAssignDrumsSD.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.SD);
            this.listItems.Add(iKeyAssignDrumsSD);

            var iKeyAssignDrumsBD = new CItemBase("Bass",
                "ドラムのキー設定：\nバスドラムへのキーの割り当てを設定\nします。",
                "Drums key assign:\nTo assign key/pads for Bass button.");
            iKeyAssignDrumsBD.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.BD);
            this.listItems.Add(iKeyAssignDrumsBD);

            var iKeyAssignDrumsHT = new CItemBase("HighTom",
                "ドラムのキー設定：\nハイタムへのキーの割り当てを設定\nします。",
                "Drums key assign:\nTo assign key/pads for HighTom\n button.");
            iKeyAssignDrumsHT.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.HT);
            this.listItems.Add(iKeyAssignDrumsHT);

            var iKeyAssignDrumsLT = new CItemBase("LowTom",
                "ドラムのキー設定：\nロータムへのキーの割り当てを設定\nします。",
                "Drums key assign:\nTo assign key/pads for LowTom button.");
            iKeyAssignDrumsLT.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.LT);
            this.listItems.Add(iKeyAssignDrumsLT);

            var iKeyAssignDrumsFT = new CItemBase("FloorTom",
                "ドラムのキー設定：\nフロアタムへのキーの割り当てを設\n定します。",
                "Drums key assign:\nTo assign key/pads for FloorTom\n button.");
            iKeyAssignDrumsFT.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.FT);
            this.listItems.Add(iKeyAssignDrumsFT);

            var iKeyAssignDrumsCY = new CItemBase("RightCymbal",
                "ドラムのキー設定：\n右シンバルへのキーの割り当てを設\n定します。",
                "Drums key assign:\nTo assign key/pads for RightCymbal\n button.");
            iKeyAssignDrumsCY.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.CY);
            this.listItems.Add(iKeyAssignDrumsCY);

            var iKeyAssignDrumsRD = new CItemBase("RideCymbal",
                "ドラムのキー設定：\nライドシンバルへのキーの割り当て\nを設定します。",
                "Drums key assign:\nTo assign key/pads for RideCymbal\n button.");
            iKeyAssignDrumsRD.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.RD);
            this.listItems.Add(iKeyAssignDrumsRD);

            var iKeyAssignDrumsLP = new CItemBase("LeftPedal",									// #27029 2012.1.4 from
                "ドラムのキー設定：\n左ペダルへのキーの\n割り当てを設定します。",
                "Drums key assign:\nTo assign key/pads for HiHatPedal\n button.");
            iKeyAssignDrumsLP.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.LP);
            this.listItems.Add(iKeyAssignDrumsLP);

            var iKeyAssignDrumsLBD = new CItemBase("LeftBassDrum",
                "ドラムのキー設定：\n左バスドラムへのキーの割り当てを設\n定します。",
                "Drums key assign:\nTo assign key/pads for RightCymbal\n button.");
            iKeyAssignDrumsLBD.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.LBD);
            this.listItems.Add(iKeyAssignDrumsLBD);

            OnListMenuの初期化();
            this.nCurrentSelection = 0;
            this.eMenuType = EMenuType.KeyAssignDrums;
        }
        public void t項目リストの設定_KeyAssignGuitar()
        {
            //			this.tRecordToConfigIni();
            this.listItems.Clear();

            // #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

            var iKeyAssignGuitarReturnToMenu = new CItemBase("<< ReturnTo Menu", CItemBase.EPanelType.Other,
                "左側のメニューに戻ります。",
                "Return to left menu.");
            iKeyAssignGuitarReturnToMenu.action = tSetupItemList_Guitar;
            this.listItems.Add(iKeyAssignGuitarReturnToMenu);

            var iKeyAssignGuitarR = new CItemBase("R",
                "ギターのキー設定：\nRボタンへのキーの割り当てを設定し\nます。",
                "Guitar key assign:\nTo assign key/pads for R button.");
            iKeyAssignGuitarR.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.GUITAR, EKeyConfigPad.R);
            this.listItems.Add(iKeyAssignGuitarR);

            var iKeyAssignGuitarG = new CItemBase("G",
                "ギターのキー設定：\nGボタンへのキーの割り当てを設定し\nます。",
                "Guitar key assign:\nTo assign key/pads for G button.");
            iKeyAssignGuitarG.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.GUITAR, EKeyConfigPad.G);
            this.listItems.Add(iKeyAssignGuitarG);

            var iKeyAssignGuitarB = new CItemBase("B",
                "ギターのキー設定：\nBボタンへのキーの割り当てを設定し\nます。",
                "Guitar key assign:\nTo assign key/pads for B button.");
            iKeyAssignGuitarB.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.GUITAR, EKeyConfigPad.B);
            this.listItems.Add(iKeyAssignGuitarB);

            var iKeyAssignGuitarY = new CItemBase("Y",
                "ギターのキー設定：\nYボタンへのキーの割り当てを設定し\nます。",
                "Guitar key assign:\nTo assign key/pads for Y button.");
            iKeyAssignGuitarY.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.GUITAR, EKeyConfigPad.Y);
            this.listItems.Add(iKeyAssignGuitarY);

            var iKeyAssignGuitarP = new CItemBase("P",
                "ギターのキー設定：\nPボタンへのキーの割り当てを設定し\nます。",
                "Guitar key assign:\nTo assign key/pads for P button.");
            iKeyAssignGuitarP.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.GUITAR, EKeyConfigPad.P);
            this.listItems.Add(iKeyAssignGuitarP);

            var iKeyAssignGuitarPick = new CItemBase("Pick",
                "ギターのキー設定：\nピックボタンへのキーの割り当てを設\n定します。",
                "Guitar key assign:\nTo assign key/pads for Pick button.");
            iKeyAssignGuitarPick.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.GUITAR, EKeyConfigPad.Pick);
            this.listItems.Add(iKeyAssignGuitarPick);

            var iKeyAssignGuitarWail = new CItemBase("Wailing",
                "ギターのキー設定：\nWailingボタンへのキーの割り当てを\n設定します。",
                "Guitar key assign:\nTo assign key/pads for Wailing button.");
            iKeyAssignGuitarWail.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.GUITAR, EKeyConfigPad.Wail); 
            this.listItems.Add(iKeyAssignGuitarWail);

            var iKeyAssignGuitarDecide = new CItemBase("Decide",
                "ギターのキー設定：\n決定ボタンへのキーの割り当てを設\n定します。",
                "Guitar key assign:\nTo assign key/pads for Decide button.");
            iKeyAssignGuitarDecide.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.GUITAR, EKeyConfigPad.Decide);
            this.listItems.Add(iKeyAssignGuitarDecide);

            var iKeyAssignGuitarCancel = new CItemBase("Cancel",
                "ギターのキー設定：\n取消ボタンへのキーの割り当てを設\n定します。",
                "Guitar key assign:\nTo assign key/pads for Cancel button.");
            iKeyAssignGuitarCancel.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.GUITAR, EKeyConfigPad.Cancel);
            this.listItems.Add(iKeyAssignGuitarCancel);

            OnListMenuの初期化();
            this.nCurrentSelection = 0;
            this.eMenuType = EMenuType.KeyAssignGuitar;
        }
        public void t項目リストの設定_KeyAssignBass()
        {
            //			this.tRecordToConfigIni();
            this.listItems.Clear();

            // #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

            var iKeyAssignBassReturnToMenu = new CItemBase("<< ReturnTo Menu", CItemBase.EPanelType.Other,
                "左側のメニューに戻ります。",
                "Return to left menu.");
            iKeyAssignBassReturnToMenu.action = tSetupItemList_Bass;
            this.listItems.Add(iKeyAssignBassReturnToMenu);

            var iKeyAssignBassR = new CItemBase("R",
                "ベースのキー設定：\nRボタンへのキーの割り当てを設定し\nます。",
                "Bass key assign:\nTo assign key/pads for R button.");
            this.listItems.Add(iKeyAssignBassR);

            var iKeyAssignBassG = new CItemBase("G",
                "ベースのキー設定：\nGボタンへのキーの割り当てを設定し\nます。",
                "Bass key assign:\nTo assign key/pads for G button.");
            this.listItems.Add(iKeyAssignBassG);

            var iKeyAssignBassB = new CItemBase("B",
                "ベースのキー設定：\nBボタンへのキーの割り当てを設定し\nます。",
                "Bass key assign:\nTo assign key/pads for B button.");
            this.listItems.Add(iKeyAssignBassB);

            var iKeyAssignBassY = new CItemBase("Y",
                "ベースのキー設定：\nYボタンへのキーの割り当てを設定し\nます。",
                "Bass key assign:\nTo assign key/pads for Y button.");
            this.listItems.Add(iKeyAssignBassY);

            var iKeyAssignBassP = new CItemBase("P",
                "ベースのキー設定：\nPボタンへのキーの割り当てを設定し\nます。",
                "Bass key assign:\nTo assign key/pads for P button.");
            this.listItems.Add(iKeyAssignBassP);

            var iKeyAssignBassPick = new CItemBase("Pick",
                "ベースのキー設定：\nピックボタンへのキーの割り当てを設\n定します。",
                "Bass key assign:\nTo assign key/pads for Pick button.");
            this.listItems.Add(iKeyAssignBassPick);

            var iKeyAssignBassWail = new CItemBase("Wailing",
                "ベースのキー設定：\nWailingボタンへのキーの割り当てを設\n定します。",
                "Bass key assign:\nTo assign key/pads for Wailing button.");
            this.listItems.Add(iKeyAssignBassWail);

            var iKeyAssignBassDecide = new CItemBase("Decide",
                "ベースのキー設定：\n決定ボタンへのキーの割り当てを設\n定します。",
                "Bass key assign:\nTo assign key/pads for Decide button.");
            this.listItems.Add(iKeyAssignBassDecide);

            var iKeyAssignBassCancel = new CItemBase("Cancel",
                "ベースのキー設定：\n取消ボタンへのキーの割り当てを設\n定します。",
                "Bass key assign:\nTo assign key/pads for Cancel button.");
            this.listItems.Add(iKeyAssignBassCancel);

            OnListMenuの初期化();
            this.nCurrentSelection = 0;
            this.eMenuType = EMenuType.KeyAssignBass;
        }
        #endregion
        public void tMoveToPrevious()
        {
            CDTXMania.Skin.soundCursorMovement.tPlay();
            if (this.bFocusIsOnElementValue)
            {
                this.listItems[this.nCurrentSelection].tMoveItemValueToPrevious();
                tPostProcessMoveUpDown();
            }
            else
            {
                this.nTargetScrollCounter += 100;
                CDTXMania.stageConfig.ctDisplayWait.nCurrentValue = 0;
            }
        }
        public void tMoveToNext()
        {
            CDTXMania.Skin.soundCursorMovement.tPlay();
            if (this.bFocusIsOnElementValue)
            {
                this.listItems[this.nCurrentSelection].tMoveItemValueToNext();
                tPostProcessMoveUpDown();
            }
            else
            {
                this.nTargetScrollCounter -= 100;
                CDTXMania.stageConfig.ctDisplayWait.nCurrentValue = 0;
            }
        }
        private void tPostProcessMoveUpDown()  // t要素値を上下に変更中の処理
        {
            if (this.listItems[this.nCurrentSelection] == this.iSystemMasterVolume)              // #33700 2014.4.26 yyagi
            {
                CDTXMania.SoundManager.nMasterVolume = this.iSystemMasterVolume.nCurrentValue;
            }
        }


        // CActivity 実装

        public override void OnActivate()
        {
            if (this.bActivated)
                return;

            this.listItems = new List<CItemBase>();
            this.eMenuType = EMenuType.Unknown;

            #region [ スキン選択肢と、現在選択中のスキン(index)の準備 #28195 2012.5.2 yyagi ]
            int ns = (CDTXMania.Skin.strSystemSkinSubfolders == null) ? 0 : CDTXMania.Skin.strSystemSkinSubfolders.Length;
            int nb = (CDTXMania.Skin.strBoxDefSkinSubfolders == null) ? 0 : CDTXMania.Skin.strBoxDefSkinSubfolders.Length;
            skinSubFolders = new string[ns + nb];
            for (int i = 0; i < ns; i++)
            {
                skinSubFolders[i] = CDTXMania.Skin.strSystemSkinSubfolders[i];
            }
            for (int i = 0; i < nb; i++)
            {
                skinSubFolders[ns + i] = CDTXMania.Skin.strBoxDefSkinSubfolders[i];
            }
            skinSubFolder_org = CDTXMania.Skin.GetCurrentSkinSubfolderFullName(true);
            Array.Sort(skinSubFolders);
            skinNames = CSkin.GetSkinName(skinSubFolders);
            nSkinIndex = Array.BinarySearch(skinSubFolders, skinSubFolder_org);
            if (nSkinIndex < 0)	// 念のため
            {
                nSkinIndex = 0;
            }
            nSkinSampleIndex = -1;
            #endregion

            this.prvFont = new CPrivateFastFont( new FontFamily( CDTXMania.ConfigIni.str選曲リストフォント ), 15 );	// t項目リストの設定 の前に必要

            this.tSetupItemList_Bass();		// #27795 2012.3.11 yyagi; System設定の中でDrumsの設定を参照しているため、
            this.tSetupItemList_Guitar();	// 活性化の時点でDrumsの設定も入れ込んでおかないと、System設定中に例外発生することがある。
            this.tSetupItemList_Drums();	// 
            this.tSetupItemList_System();	// 順番として、最後にSystemを持ってくること。設定一覧の初期位置がSystemのため。
            this.bFocusIsOnElementValue = false;
            this.nTargetScrollCounter = 0;
            this.n現在のスクロールカウンタ = 0;
            this.nスクロール用タイマ値 = -1;
            this.ctTriangleArrowAnimation = new CCounter();
            this.ctToastMessageCounter = new CCounter(0, 1, 10000, CDTXMania.Timer);

            this.iSystemSoundType_initial = this.iSystemSoundType.n現在選択されている項目番号; // CONFIGに入ったときの値を保持しておく
            this.iSystemWASAPIBufferSizeMs_initial = this.iSystemWASAPIBufferSizeMs.nCurrentValue; // CONFIG脱出時にこの値から変更されているようなら
            //this.iSystemASIOBufferSizeMs_initial = this.iSystemASIOBufferSizeMs.nCurrentValue; // サウンドデバイスを再構築する
            this.iSystemASIODevice_initial = this.iSystemASIODevice.n現在選択されている項目番号; //
            base.OnActivate();
        }
        public override void OnDeactivate()
        {
            if (this.bNotActivated)
                return;

            this.tRecordToConfigIni();
            this.listItems.Clear();
            this.ctTriangleArrowAnimation = null;

            OnListMenuの解放();
            prvFont.Dispose();

            base.OnDeactivate();

            #region [ Skin変更 ]
            if (CDTXMania.Skin.GetCurrentSkinSubfolderFullName(true) != this.skinSubFolder_org)
            {
                CDTXMania.stageChangeSkin.tChangeSkinMain();	// #28195 2012.6.11 yyagi CONFIG脱出時にSkin更新
            }
            #endregion

            // #24820 2013.1.22 yyagi CONFIGでWASAPI/ASIO/DirectSound関連の設定を変更した場合、サウンドデバイスを再構築する。
            #region [ サウンドデバイス変更 ]
            if (this.iSystemSoundType_initial != this.iSystemSoundType.n現在選択されている項目番号 ||
                this.iSystemWASAPIBufferSizeMs_initial != this.iSystemWASAPIBufferSizeMs.nCurrentValue ||
                //this.iSystemASIOBufferSizeMs_initial != this.iSystemASIOBufferSizeMs.nCurrentValue ||
                this.iSystemASIODevice_initial != this.iSystemASIODevice.n現在選択されている項目番号 ||
                this.iSystemSoundTimerType_initial != this.iSystemSoundTimerType.GetIndex())
            {
                ESoundDeviceType soundDeviceType;
                switch (this.iSystemSoundType.n現在選択されている項目番号)
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

                CDTXMania.SoundManager.t初期化(soundDeviceType,
                                        this.iSystemWASAPIBufferSizeMs.nCurrentValue,
                                        false,
                                        0,
                                        this.iSystemASIODevice.n現在選択されている項目番号,
                                        this.iSystemSoundTimerType.bON);
                //CDTXMania.app.ShowWindowTitleWithSoundType();   //XGオプション
                CDTXMania.app.AddSoundTypeToWindowTitle();    //GDオプション
            }
            #endregion
            #region [ サウンドのタイムストレッチモード変更 ]
            FDK.CSoundManager.bIsTimeStretch = this.iSystemTimeStretch.bON;
            #endregion
        }
        public override void OnManagedCreateResources()
        {
            if (this.bNotActivated)
                return;

            this.tx通常項目行パネル = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\4_itembox.png"), false);
            this.txその他項目行パネル = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\4_itembox other.png"), false);
            this.tx三角矢印 = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\4_triangle arrow.png"), false);
            this.tx説明文パネル = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\4_Description Panel.png" ) );
            this.tx矢印 = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\4_Arrow.png" ) );
            this.txカーソル = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\4_itembox cursor.png" ) );
            this.txJudgementLine = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\ScreenPlayDrums hit-bar.png"));
            this.txLane = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_Paret.png"));
            this.txShutter = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_shutter.png"));
            this.txSkinSample1 = null;		// スキン選択時に動的に設定するため、ここでは初期化しない
            this.prvFontForToastMessage = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.str選曲リストフォント), 14, FontStyle.Regular);
            base.OnManagedCreateResources();
        }
        public override void OnManagedReleaseResources()
        {
            if (this.bNotActivated)
                return;

            CDTXMania.tReleaseTexture(ref this.txSkinSample1);
            CDTXMania.tReleaseTexture(ref this.tx通常項目行パネル);
            CDTXMania.tReleaseTexture(ref this.txその他項目行パネル);
            CDTXMania.tReleaseTexture(ref this.tx三角矢印);
            CDTXMania.tReleaseTexture( ref this.tx説明文パネル );
            CDTXMania.tReleaseTexture( ref this.tx矢印 );
            CDTXMania.tReleaseTexture( ref this.txカーソル );
            CDTXMania.tReleaseTexture(ref this.txLane);
            CDTXMania.tReleaseTexture(ref this.txJudgementLine);
            CDTXMania.tReleaseTexture(ref this.txShutter);
            CDTXMania.tReleaseTexture(ref this.txToastMessage);
            CDTXMania.t安全にDisposeする(ref this.prvFontForToastMessage);
            base.OnManagedReleaseResources();
        }

		private void OnListMenuの初期化()
		{
			OnListMenuの解放();
			this.listMenu = new stMenuItemRight[ this.listItems.Count ];
		}

		/// <summary>
		/// 事前にレンダリングしておいたテクスチャを解放する。
		/// </summary>
		private void OnListMenuの解放()
		{
			if ( listMenu != null )
			{
				for ( int i = 0; i < listMenu.Length; i++ )
				{
					if ( listMenu[ i ].txParam != null )
					{
						listMenu[ i ].txParam.Dispose();
					}
					if ( listMenu[ i ].txMenuItemRight != null )
					{
						listMenu[ i ].txMenuItemRight.Dispose();
					}
				}
				this.listMenu = null;
			}
		}
        public override int OnUpdateAndDraw()
        {
            throw new InvalidOperationException("tUpdateAndDraw(bool)のほうを使用してください。");
        }
        public int tUpdateAndDraw(bool b項目リスト側にフォーカスがある)  // t進行描画
        {
            if (this.bNotActivated)
                return 0;

            // 進行

            #region [ 初めての進行描画 ]
            //-----------------
            if (base.bJustStartedUpdate)
            {
                this.nスクロール用タイマ値 = CSoundManager.rcPerformanceTimer.nCurrentTime;
                this.ctTriangleArrowAnimation.tStart(0, 9, 50, CDTXMania.Timer);

                base.bJustStartedUpdate = false;
            }
            //-----------------
            #endregion

            this.bFocusIsOnItemList = b項目リスト側にフォーカスがある;		// 記憶

            #region [ 項目スクロールの進行 ]
            //-----------------
            long n現在時刻 = CDTXMania.Timer.nCurrentTime;
            if (n現在時刻 < this.nスクロール用タイマ値) this.nスクロール用タイマ値 = n現在時刻;

            const int INTERVAL = 2;	// [ms]
            while ((n現在時刻 - this.nスクロール用タイマ値) >= INTERVAL)
            {
                int n目標項目までのスクロール量 = Math.Abs((int)(this.nTargetScrollCounter - this.n現在のスクロールカウンタ));
                int n加速度 = 0;

                #region [ n加速度の決定；目標まで遠いほど加速する。]
                //-----------------
                if (n目標項目までのスクロール量 <= 100)
                {
                    n加速度 = 2;
                }
                else if (n目標項目までのスクロール量 <= 300)
                {
                    n加速度 = 3;
                }
                else if (n目標項目までのスクロール量 <= 500)
                {
                    n加速度 = 4;
                }
                else
                {
                    n加速度 = 8;
                }
                //-----------------
                #endregion
                #region [ this.n現在のスクロールカウンタに n加速度 を加減算。]
                //-----------------
                if (this.n現在のスクロールカウンタ < this.nTargetScrollCounter)
                {
                    this.n現在のスクロールカウンタ += n加速度;
                    if (this.n現在のスクロールカウンタ > this.nTargetScrollCounter)
                    {
                        // 目標を超えたら目標値で停止。
                        this.n現在のスクロールカウンタ = this.nTargetScrollCounter;
                    }
                }
                else if (this.n現在のスクロールカウンタ > this.nTargetScrollCounter)
                {
                    this.n現在のスクロールカウンタ -= n加速度;
                    if (this.n現在のスクロールカウンタ < this.nTargetScrollCounter)
                    {
                        // 目標を超えたら目標値で停止。
                        this.n現在のスクロールカウンタ = this.nTargetScrollCounter;
                    }
                }
                //-----------------
                #endregion
                #region [ 行超え処理、ならびに目標位置に到達したらスクロールを停止して項目変更通知を発行。]
                //-----------------
                if (this.n現在のスクロールカウンタ >= 100)
                {
                    this.nCurrentSelection = this.tNextItem(this.nCurrentSelection);
                    this.n現在のスクロールカウンタ -= 100;
                    this.nTargetScrollCounter -= 100;
                    if (this.nTargetScrollCounter == 0)
                    {
                        CDTXMania.stageConfig.tNotifyItemChange();
                    }
                }
                else if (this.n現在のスクロールカウンタ <= -100)
                {
                    this.nCurrentSelection = this.tPreviousItem(this.nCurrentSelection);
                    this.n現在のスクロールカウンタ += 100;
                    this.nTargetScrollCounter += 100;
                    if (this.nTargetScrollCounter == 0)
                    {
                        CDTXMania.stageConfig.tNotifyItemChange();
                    }
                }
                //-----------------
                #endregion

                this.nスクロール用タイマ値 += INTERVAL;
            }
            //-----------------
            #endregion

            #region [ ▲印アニメの進行 ]
            //-----------------
            if (this.bFocusIsOnItemList && (this.nTargetScrollCounter == 0))
                this.ctTriangleArrowAnimation.tUpdateLoop();
            //-----------------
            #endregion

            #region [ Update Toast Message Counter] 
            this.ctToastMessageCounter.tUpdate();
            if (this.ctToastMessageCounter.bReachedEndValue)
            {
                this.tUpdateToastMessage("");
            }
            #endregion

            // 描画

            this.ptパネルの基本座標[4].X = this.bFocusIsOnItemList ? 0x228 : 0x25a;		// メニューにフォーカスがあるなら、項目リストの中央は頭を出さない。

            //2014.04.25 kairera0467 GITADORAでは項目パネルが11個だが、選択中のカーソルは中央に無いので両方を同じにすると7×2+1=15個パネルが必要になる。
            //　　　　　　　　　　　 さらに画面に映らないがアニメーション中に見える箇所を含めると17個は必要とされる。
            //　　　　　　　　　　　 ただ、画面に表示させる分には上のほうを考慮しなくてもよさそうなので、上4個は必要なさげ。
            #region [ 計11個の項目パネルを描画する。]
            //-----------------
            int nItem = this.nCurrentSelection;
            for (int i = 0; i < 4; i++)
                nItem = this.tPreviousItem(nItem);

            for (int n行番号 = -4; n行番号 < 10; n行番号++)		// n行番号 == 0 がフォーカスされている項目パネル。
            {
                #region [ 今まさに画面外に飛びだそうとしている項目パネルは描画しない。]
                //-----------------
                if (((n行番号 == -4) && (this.n現在のスクロールカウンタ > 0)) ||		// 上に飛び出そうとしている
                    ((n行番号 == +9) && (this.n現在のスクロールカウンタ < 0)))		// 下に飛び出そうとしている
                {
                    nItem = this.tNextItem(nItem);
                    continue;
                }
                //-----------------
                #endregion

                int n移動元の行の基本位置 = n行番号 + 4;
                int n移動先の行の基本位置 = (this.n現在のスクロールカウンタ <= 0) ? ((n移動元の行の基本位置 + 1) % 14) : (((n移動元の行の基本位置 - 1) + 14) % 14);
                int x = this.pt新パネルの基本座標[n移動元の行の基本位置].X + ((int)((this.pt新パネルの基本座標[n移動先の行の基本位置].X - this.pt新パネルの基本座標[n移動元の行の基本位置].X) * (((double)Math.Abs(this.n現在のスクロールカウンタ)) / 100.0)));
                int y = this.pt新パネルの基本座標[n移動元の行の基本位置].Y + ((int)((this.pt新パネルの基本座標[n移動先の行の基本位置].Y - this.pt新パネルの基本座標[n移動元の行の基本位置].Y) * (((double)Math.Abs(this.n現在のスクロールカウンタ)) / 100.0)));
                int n新項目パネルX = 420;

                #region [ 現在の行の項目パネル枠を描画。]
                //-----------------
                switch (this.listItems[nItem].ePanelType)
                {
                    case CItemBase.EPanelType.Normal:
                        if (this.tx通常項目行パネル != null)
                            this.tx通常項目行パネル.tDraw2D(CDTXMania.app.Device, n新項目パネルX, y);
                        break;

                    case CItemBase.EPanelType.Other:
                        if (this.txその他項目行パネル != null)
                            this.txその他項目行パネル.tDraw2D(CDTXMania.app.Device, n新項目パネルX, y);
                        break;
                }
                //-----------------
                #endregion
                #region [ 現在の行の項目名を描画。]
                //-----------------
				if ( listMenu[ nItem ].txMenuItemRight != null )	// 自前のキャッシュに含まれているようなら、再レンダリングせずキャッシュを使用
				{
					listMenu[ nItem ].txMenuItemRight.tDraw2D( CDTXMania.app.Device, ( n新項目パネルX + 20 ), ( y + 24 ) );
				}
				else
				{
					Bitmap bmpItem = prvFont.DrawPrivateFont( this.listItems[ nItem ].strItemName, Color.White, Color.Transparent );
					listMenu[ nItem ].txMenuItemRight = CDTXMania.tGenerateTexture( bmpItem );
//					ctItem.tDraw2D( CDTXMania.app.Device, ( x + 0x12 ) * Scale.X, ( y + 12 ) * Scale.Y - 20 );
//					CDTXMania.tReleaseTexture( ref ctItem );
					CDTXMania.t安全にDisposeする( ref bmpItem );
				}
				//CDTXMania.stageConfig.actFont.tDrawString( x + 0x12, y + 12, this.listItems[ nItem ].strItemName );
                //-----------------
                #endregion
                #region [ 現在の行の項目の要素を描画。]
				//-----------------
				string strParam = null;
				bool b強調 = false;
				switch( this.listItems[ nItem ].eType )
				{
					case CItemBase.EType.ONorOFFToggle:
						#region [ *** ]
						//-----------------
						//CDTXMania.stageConfig.actFont.tDrawString( x + 210, y + 12, ( (CItemToggle) this.listItems[ nItem ] ).bON ? "ON" : "OFF" );
						strParam = ( (CItemToggle) this.listItems[ nItem ] ).bON ? "ON" : "OFF";
						break;
						//-----------------
						#endregion

					case CItemBase.EType.ONorOFForUndefined3State:
						#region [ *** ]
						//-----------------
						switch( ( (CItemThreeState) this.listItems[ nItem ] ).e現在の状態 )
						{
							case CItemThreeState.E状態.ON:
								strParam = "ON";
								break;

							case CItemThreeState.E状態.不定:
								strParam = "- -";
								break;

							default:
								strParam = "OFF";
								break;
						}
						//CDTXMania.stageConfig.actFont.tDrawString( x + 210, y + 12, "ON" );
						break;
						//-----------------
						#endregion

					case CItemBase.EType.Integer:		// #24789 2011.4.8 yyagi: add PlaySpeed supports (copied them from OPTION)
						#region [ *** ]
						//-----------------
						if( this.listItems[ nItem ] == this.iCommonPlaySpeed )
						{
							double d = ( (double) ( (CItemInteger) this.listItems[ nItem ] ).nCurrentValue ) / 20.0;
							//CDTXMania.stageConfig.actFont.tDrawString( x + 210, y + 12, d.ToString( "0.000" ), ( n行番号 == 0 ) && this.bFocusIsOnElementValue );
							strParam = d.ToString( "0.000" );
						}
						else if( this.listItems[ nItem ] == this.iDrumsScrollSpeed || this.listItems[ nItem ] == this.iGuitarScrollSpeed || this.listItems[ nItem ] == this.iBassScrollSpeed )
						{
							float f = ( ( (CItemInteger) this.listItems[ nItem ] ).nCurrentValue + 1 ) * 0.5f;
							//CDTXMania.stageConfig.actFont.tDrawString( x + 210, y + 12, f.ToString( "x0.0" ), ( n行番号 == 0 ) && this.bFocusIsOnElementValue );
							strParam = f.ToString( "x0.0" );
						}
						else
						{
							//CDTXMania.stageConfig.actFont.tDrawString( x + 210, y + 12, ( (CItemInteger) this.listItems[ nItem ] ).nCurrentValue.ToString(), ( n行番号 == 0 ) && this.bFocusIsOnElementValue );
							strParam = ( (CItemInteger) this.listItems[ nItem ] ).nCurrentValue.ToString();
						}
						b強調 = ( n行番号 == 0 ) && this.bFocusIsOnElementValue;
						break;
						//-----------------
						#endregion

					case CItemBase.EType.List:	// #28195 2012.5.2 yyagi: add Skin supports
						#region [ *** ]
						//-----------------
						{
							CItemList list = (CItemList) this.listItems[ nItem ];
							//CDTXMania.stageConfig.actFont.tDrawString( x + 210, y + 12, list.list項目値[ list.n現在選択されている項目番号 ] );
							strParam = list.list項目値[ list.n現在選択されている項目番号 ];

							#region [ 必要な場合に、Skinのサンプルを生成_描画する。#28195 2012.5.2 yyagi ]
							if ( this.listItems[ this.nCurrentSelection ] == this.iSystemSkinSubfolder )
							{
								tGenerateSkinSample();		// 最初にSkinの選択肢にきたとき(Enterを押す前)に限り、サンプル生成が発生する。

							}
							#endregion
							break;
						}
						//-----------------
						#endregion
				}
				if ( b強調 )
				{
					Bitmap bmpStr = b強調 ?
						prvFont.DrawPrivateFont( strParam, Color.White, Color.Black, Color.Yellow, Color.OrangeRed ) :
						prvFont.DrawPrivateFont( strParam, Color.Black, Color.Transparent );
					CTexture txStr = CDTXMania.tGenerateTexture( bmpStr, false );
					txStr.tDraw2D( CDTXMania.app.Device, ( n新項目パネルX + 260 ) , ( y + 20 ) );
					CDTXMania.tReleaseTexture( ref txStr );
					CDTXMania.t安全にDisposeする( ref bmpStr );
				}
				else
				{
					int nIndex = this.listItems[ nItem ].GetIndex();
					if ( listMenu[ nItem ].nParam != nIndex || listMenu[ nItem ].txParam == null )
					{
						stMenuItemRight stm = listMenu[ nItem ];
						stm.nParam = nIndex;
						object o = this.listItems[ nItem ].obj現在値();
						stm.strParam = ( o == null ) ? "" : o.ToString();

				        Bitmap bmpStr =
				            prvFont.DrawPrivateFont( strParam, Color.Black, Color.Transparent );
				        stm.txParam = CDTXMania.tGenerateTexture( bmpStr, false );
				        CDTXMania.t安全にDisposeする( ref bmpStr );

				        listMenu[ nItem ] = stm;
				    }
				    listMenu[ nItem ].txParam.tDraw2D( CDTXMania.app.Device, ( n新項目パネルX + 260 ) , ( y + 24 ) );
				}
				//-----------------
                #endregion

                nItem = this.tNextItem(nItem);
            }
            //-----------------
            #endregion

            #region[ カーソル ]
            if( this.bFocusIsOnItemList )
            {
                this.txカーソル.tDraw2D( CDTXMania.app.Device, 413, 193 );
            }
            #endregion

            #region[ 説明文パネル ]
            if( this.bFocusIsOnItemList && this.nTargetScrollCounter == 0 && CDTXMania.stageConfig.ctDisplayWait.bReachedEndValue )
            {
                // 15SEP20 Increasing x position by 180 pixels (was 601)
                this.tx説明文パネル.tDraw2D( CDTXMania.app.Device, 781, 252 );
                if ( txSkinSample1 != null && this.nTargetScrollCounter == 0 && this.listItems[ this.nCurrentSelection ] == this.iSystemSkinSubfolder )
				{
                    // 15SEP20 Increasing x position by 180 pixels (was 615 - 60)
                    txSkinSample1.tDraw2D( CDTXMania.app.Device, 735, 442 - 106 );
				}
            }
            #endregion

            #region [ 項目リストにフォーカスがあって、かつスクロールが停止しているなら、パネルの上下に▲印を描画する。]
            //-----------------
            if( this.bFocusIsOnItemList )//&& (this.nTargetScrollCounter == 0))
            {
                int x;
                int y_upper;
                int y_lower;

                int n新カーソルX = 394;
                int n新カーソル上Y = 174;
                int n新カーソル下Y = 240;

                // 位置決定。

                if (this.bFocusIsOnElementValue)
                {
                    x = 552;	// 要素値の上下あたり。
                    y_upper = 0x117 - this.ctTriangleArrowAnimation.nCurrentValue;
                    y_lower = 0x17d + this.ctTriangleArrowAnimation.nCurrentValue;
                }
                else
                {
                    x = 552;	// 項目名の上下あたり。
                    y_upper = 0x129 - this.ctTriangleArrowAnimation.nCurrentValue;
                    y_lower = 0x16b + this.ctTriangleArrowAnimation.nCurrentValue;
                }

                //新矢印
                if( this.tx矢印 != null )
                {
                    this.tx矢印.tDraw2D(CDTXMania.app.Device, n新カーソルX, n新カーソル上Y, new Rectangle(0, 0, 40, 40));
                    this.tx矢印.tDraw2D(CDTXMania.app.Device, n新カーソルX, n新カーソル下Y, new Rectangle(0, 40, 40, 40));
                }
            }
            //-----------------
            #endregion

            #region [ Draw Toast Message ]

            if (this.txToastMessage != null)
            {
                this.txToastMessage.tDraw2D(CDTXMania.app.Device, 15, 325);
            }
            #endregion

            return 0;
        }


        // Other

        #region [ private ]
        //-----------------
        private enum EMenuType
        {
            System,
            Drums,
            Guitar,
            Bass,
            KeyAssignSystem,		// #24609 2011.4.12 yyagi: 画面キャプチャキーのアサイン
            KeyAssignDrums,
            KeyAssignGuitar,
            KeyAssignBass,
            Unknown
        }

        private bool bFocusIsOnItemList;
        private bool bFocusIsOnElementValue;
        private CCounter ctTriangleArrowAnimation;
        private EMenuType eMenuType;
        
        private CItemList iSystemSoundType;         // #24820 2013.1.3 yyagi
        private CItemInteger iSystemWASAPIBufferSizeMs;		// #24820 2013.1.15 yyagi
        private CItemList iSystemASIODevice;                // #24820 2013.1.17 yyagi
        private CItemToggle iSystemSoundTimerType;			// #33689 2014.6.17 yyagi
        private CItemToggle iSystemWASAPIEventDriven;

        private int iSystemSoundType_initial;
        private int iSystemWASAPIBufferSizeMs_initial;
        //private int iSystemASIOBufferSizeMs_initial;
        private int iSystemASIODevice_initial;
        private int iSystemSoundTimerType_initial;			// #33689 2014.6.17 yyagi
        private CItemInteger iSystemMasterVolume;
        private CItemToggle iSystemTimeStretch;             // #23664 2013.2.24 yyagi

        private List<CItemBase> listItems;
        private long nスクロール用タイマ値;
        private int n現在のスクロールカウンタ;
        public int nTargetScrollCounter;
        private Point[] ptパネルの基本座標 = new Point[] { new Point(0x25a, 4), new Point(0x25a, 0x4f), new Point(0x25a, 0x9a), new Point(0x25a, 0xe5), new Point(0x228, 0x130), new Point(0x25a, 0x17b), new Point(0x25a, 0x1c6), new Point(0x25a, 0x211), new Point(0x25a, 0x25c), new Point(0x25a, 0x2a7), new Point(0x25a, 0x2d0) };
        private Point[] pt新パネルの基本座標 = new Point[] { new Point(0x25a, -79), new Point(0x25a, -12), new Point(0x25a, 55), new Point(0x25a, 122), new Point(0x228, 189), new Point(0x25a, 256), new Point(0x25a, 323), new Point(0x25a, 390), new Point(0x25a, 457), new Point(0x25a, 524), new Point(0x25a, 591), new Point(0x25a, 658), new Point(0x25a, 725), new Point(0x25a, 792) };
        private CTexture txその他項目行パネル;
        private CTexture tx三角矢印;
        private CTexture tx矢印;
        private CTexture tx通常項目行パネル;
        private CTexture txカーソル;
        private CTexture tx説明文パネル;
        private CTexture txToastMessage;
        private CPrivateFastFont prvFontForToastMessage;
        private CCounter ctToastMessageCounter;

        private CPrivateFastFont prvFont;
        //private List<string> list項目リスト_str最終描画名;
        private struct stMenuItemRight
        {
            //	public string strMenuItem;
            public CTexture txMenuItemRight;
            public int nParam;
            public string strParam;
            public CTexture txParam;
        }
        private stMenuItemRight[] listMenu;

        private CTexture txSkinSample1;				// #28195 2012.5.2 yyagi
        private string[] skinSubFolders;			//
        private string[] skinNames;					//
        private string skinSubFolder_org;			//
        private int nSkinSampleIndex;				//
        private int nSkinIndex;						//

        private CTexture txLane;
        private CTexture txJudgementLine;
        private CTexture txShutter;
        
        private CItemList iSystemGRmode;

        private CItemInteger iCommonPlaySpeed;

        #region [ Drums ]
        private CItemInteger iDrumsScrollSpeed;
        private CItemToggle iDrumsGraph;
        #endregion

        #region [ Guitar ]
        private CItemInteger iGuitarScrollSpeed;
        private CItemToggle iGuitarGraph;
        #endregion
        
        #region [ Bass ]
        private CItemInteger iBassScrollSpeed;
        private CItemToggle iBassGraph;
        #endregion
        
        private CItemList iSystemSkinSubfolder;				// #28195 2012.5.2 yyagi

        private int tPreviousItem(int nItem)
        {
            if (--nItem < 0)
            {
                nItem = this.listItems.Count - 1;
            }
            return nItem;
        }
        private int tNextItem(int nItem)
        {
            if (++nItem >= this.listItems.Count)
            {
                nItem = 0;
            }
            return nItem;
        }

        private void tUpdateDisplayValuesFromConfigIni()
        {
            foreach (var item in listItems)
            {
                item.ReadFromConfig();
            }
        }

        private void tRecordToConfigIni()
        {
            foreach (var item in listItems)
            {
                item.WriteToConfig();
            }

            if (eMenuType == EMenuType.System)
            {
                CDTXMania.ConfigIni.bGuitarEnabled = (((this.iSystemGRmode.n現在選択されている項目番号 + 1) / 2) == 1);
                CDTXMania.ConfigIni.bDrumsEnabled = (((this.iSystemGRmode.n現在選択されている項目番号 + 1) % 2) == 1);

                CDTXMania.ConfigIni.strSystemSkinSubfolderFullName = skinSubFolders[nSkinIndex];				// #28195 2012.5.2 yyagi
                CDTXMania.Skin.SetCurrentSkinSubfolderFullName(CDTXMania.ConfigIni.strSystemSkinSubfolderFullName, true);
            }
        }

        private void tUpdateToastMessage(string strMessage) {
            CDTXMania.t安全にDisposeする(ref this.txToastMessage);

            if (strMessage != "" && this.prvFontForToastMessage != null)
            {                
                Bitmap bmpItem = this.prvFontForToastMessage.DrawPrivateFont(strMessage, Color.White, Color.Black);
                this.txToastMessage = CDTXMania.tGenerateTexture(bmpItem);                
                CDTXMania.t安全にDisposeする(ref bmpItem);
            }
            else 
            {
                this.txToastMessage = null;
            }

        }
        //-----------------
        #endregion
    }
}
