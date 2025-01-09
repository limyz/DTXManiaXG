using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using FDK;

using SlimDXKey = SlimDX.DirectInput.Key;

namespace DTXMania
{
	internal class CConfigIni
	{
		// Class

		public class CKeyAssign
		{
			public class CKeyAssignPad
			{
				public CConfigIni.CKeyAssign.STKEYASSIGN[] HH
				{
					get
					{
						return this.padHH_R;
					}
					set
					{
						this.padHH_R = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] R
				{
					get
					{
						return this.padHH_R;
					}
					set
					{
						this.padHH_R = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] SD
				{
					get
					{
						return this.padSD_G;
					}
					set
					{
						this.padSD_G = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] G
				{
					get
					{
						return this.padSD_G;
					}
					set
					{
						this.padSD_G = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] BD
				{
					get
					{
						return this.padBD_B;
					}
					set
					{
						this.padBD_B = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] B
				{
					get
					{
						return this.padBD_B;
					}
					set
					{
						this.padBD_B = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] HT
				{
					get
					{
						return this.padHT_Pick;
					}
					set
					{
						this.padHT_Pick = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] Pick
				{
					get
					{
						return this.padHT_Pick;
					}
					set
					{
						this.padHT_Pick = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] LT
				{
					get
					{
						return this.padLT_Wail;
					}
					set
					{
						this.padLT_Wail = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] Wail
				{
					get
					{
						return this.padLT_Wail;
					}
					set
					{
						this.padLT_Wail = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] FT
				{
					get
					{
                        return this.padFT_Help;
					}
					set
					{
                        this.padFT_Help = value;
					}
				}
                public CConfigIni.CKeyAssign.STKEYASSIGN[] Help
				{
					get
					{
                        return this.padFT_Help;
					}
					set
					{
                        this.padFT_Help = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] CY
				{
					get
					{
						return this.padCY_Decide;
					}
					set
					{
						this.padCY_Decide = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] Decide
				{
					get
					{
						return this.padCY_Decide;
					}
					set
					{
						this.padCY_Decide = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] HHO
				{
					get
					{
						return this.padHHO_Y;
					}
					set
					{
						this.padHHO_Y = value;
					}
				}
                public CConfigIni.CKeyAssign.STKEYASSIGN[] Y
				{
					get
					{
						return this.padHHO_Y;
					}
					set
					{
						this.padHHO_Y = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] RD
				{
					get
					{
						return this.padRD;
					}
					set
					{
						this.padRD = value;
					}
				}
                public CConfigIni.CKeyAssign.STKEYASSIGN[] P
				{
					get
					{
						return this.padLC_P;
					}
					set
					{
						this.padLC_P = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] LC
				{
					get
					{
						return this.padLC_P;
					}
					set
					{
						this.padLC_P = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] LP
				{
					get
					{
						return this.padLP;
					}
					set
					{
						this.padLP = value;
					}
				}

                public CConfigIni.CKeyAssign.STKEYASSIGN[] LBD
                {
                    get
                    {
                        return this.padLBD;
                    }
                    set
                    {
                        this.padLBD = value;
                    }
                }

				public CConfigIni.CKeyAssign.STKEYASSIGN[] Cancel
				{
					get
					{
						return this.padCancel;
					}
					set
					{
						this.padCancel = value;
					}
				}

				public CConfigIni.CKeyAssign.STKEYASSIGN[] Capture
				{
					get
					{
						return this.padCapture;
					}
					set
					{
						this.padCapture = value;
					}
				}

				public CConfigIni.CKeyAssign.STKEYASSIGN[] Search
				{
					get
					{
						return this.padSearch;
					}
					set
					{
						this.padSearch = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] LoopCreate
				{
					get
					{
						return this.padLoopCreate;
					}
					set
					{
						this.padLoopCreate = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] LoopDelete
				{
					get
					{
						return this.padLoopDelete;
					}
					set
					{
						this.padLoopDelete = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] SkipForward
				{
					get
					{
						return this.padSkipForward;
					}
					set
					{
						this.padSkipForward = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] SkipBackward
				{
					get
					{
						return this.padSkipBackward;
					}
					set
					{
						this.padSkipBackward = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] IncreasePlaySpeed
				{
					get
					{
						return this.padIncreasePlaySpeed;
					}
					set
					{
						this.padIncreasePlaySpeed = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] DecreasePlaySpeed
				{
					get
					{
						return this.padDecreasePlaySpeed;
					}
					set
					{
						this.padDecreasePlaySpeed = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] Restart
				{
					get
					{
						return this.padRestart;
					}
					set
					{
						this.padRestart = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] this[ int index ]
				{
					get
					{
						switch ( index )
						{
							case (int) EKeyConfigPad.HH:
								return this.padHH_R;

							case (int) EKeyConfigPad.SD:
								return this.padSD_G;

							case (int) EKeyConfigPad.BD:
								return this.padBD_B;

							case (int) EKeyConfigPad.HT:
								return this.padHT_Pick;

							case (int) EKeyConfigPad.LT:
								return this.padLT_Wail;

							case (int) EKeyConfigPad.FT:
                                return this.padFT_Help;

							case (int) EKeyConfigPad.CY:
								return this.padCY_Decide;

							case (int) EKeyConfigPad.HHO:
								return this.padHHO_Y;

							case (int) EKeyConfigPad.RD:
								return this.padRD;

							case (int) EKeyConfigPad.LC:
								return this.padLC_P;

							case (int) EKeyConfigPad.LP:	// #27029 2012.1.4 from
								return this.padLP;			//(HPからLPに。)

                            case (int) EKeyConfigPad.LBD:
                                return this.padLBD;

							case (int) EKeyConfigPad.Cancel:
								return this.padCancel;

							case (int) EKeyConfigPad.Capture:
								return this.padCapture;

							case (int)EKeyConfigPad.Search:
								return this.padSearch;

							case (int)EKeyConfigPad.LoopCreate:
								return this.padLoopCreate;

							case (int)EKeyConfigPad.LoopDelete:
								return this.padLoopDelete;

							case (int)EKeyConfigPad.SkipForward:
								return this.padSkipForward;

							case (int)EKeyConfigPad.SkipBackward:
								return this.padSkipBackward;

							case (int)EKeyConfigPad.IncreasePlaySpeed:
								return this.padIncreasePlaySpeed;

							case (int)EKeyConfigPad.DecreasePlaySpeed:
								return this.padDecreasePlaySpeed;

							case (int)EKeyConfigPad.Restart:
								return this.padRestart;
						}
						throw new IndexOutOfRangeException();
					}
					set
					{
						switch ( index )
						{
							case (int) EKeyConfigPad.HH:
								this.padHH_R = value;
								return;

							case (int) EKeyConfigPad.SD:
								this.padSD_G = value;
								return;

							case (int) EKeyConfigPad.BD:
								this.padBD_B = value;
								return;

							case (int) EKeyConfigPad.Pick:
								this.padHT_Pick = value;
								return;

							case (int) EKeyConfigPad.LT:
								this.padLT_Wail = value;
								return;

							case (int) EKeyConfigPad.FT:
                                this.padFT_Help = value;
								return;

							case (int) EKeyConfigPad.CY:
								this.padCY_Decide = value;
								return;

							case (int) EKeyConfigPad.HHO:
								this.padHHO_Y = value;
								return;
                            
                            case (int) EKeyConfigPad.RD:
								this.padRD = value;
								return;
                            
                            case (int) EKeyConfigPad.LC:
								this.padLC_P = value;
								return;
                            
                            case (int) EKeyConfigPad.LP:
                                this.padLP = value;
                                return;
                            
                            case (int) EKeyConfigPad.LBD:
								this.padLBD = value;
								return;

							case (int) EKeyConfigPad.Cancel:
								this.padCancel = value;
								return;

							case (int) EKeyConfigPad.Capture:
								this.padCapture = value;
								return;

							case (int)EKeyConfigPad.Search:
								this.padSearch = value;
								return;

							case (int)EKeyConfigPad.LoopCreate:
								this.padLoopCreate = value;
								return;

							case (int)EKeyConfigPad.LoopDelete:
								this.padLoopDelete = value;
								return;

							case (int)EKeyConfigPad.SkipForward:
								this.padSkipForward = value;
								return;

							case (int)EKeyConfigPad.SkipBackward:
								this.padSkipBackward = value;
								return;

							case (int)EKeyConfigPad.IncreasePlaySpeed:
								this.padIncreasePlaySpeed = value;
								return;

							case (int)EKeyConfigPad.DecreasePlaySpeed:
								this.padDecreasePlaySpeed = value;
								return;

							case (int)EKeyConfigPad.Restart:
								this.padRestart = value;
								return;
						}
						throw new IndexOutOfRangeException();
					}
				}

				#region [ private ]
				//-----------------
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padBD_B;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padCY_Decide;
                private CConfigIni.CKeyAssign.STKEYASSIGN[] padFT_Help;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padHH_R;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padHHO_Y;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padHT_Pick;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padLC_P;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padLT_Wail;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padRD;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padSD_G;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padLP;
                private CConfigIni.CKeyAssign.STKEYASSIGN[] padLBD;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padCancel; 
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padCapture;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padSearch;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padLoopCreate;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padLoopDelete;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padSkipForward;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padSkipBackward;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padIncreasePlaySpeed;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padDecreasePlaySpeed;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padRestart;
				//-----------------
				#endregion
			}

			[StructLayout( LayoutKind.Sequential )]
			public struct STKEYASSIGN
			{
				public EInputDevice InputDevice;
				public int ID;
				public int Code;
				public STKEYASSIGN( EInputDevice DeviceType, int nID, int nCode )
				{
					this.InputDevice = DeviceType;
					this.ID = nID;
					this.Code = nCode;
				}
			}

			public CKeyAssignPad Bass = new CKeyAssignPad();
			public CKeyAssignPad Drums = new CKeyAssignPad();
			public CKeyAssignPad Guitar = new CKeyAssignPad();
			public CKeyAssignPad System = new CKeyAssignPad();
			public CKeyAssignPad this[ int index ]
			{
				get
				{
					switch( index )
					{
						case (int) EKeyConfigPart.DRUMS:
							return this.Drums;

						case (int) EKeyConfigPart.GUITAR:
							return this.Guitar;

						case (int) EKeyConfigPart.BASS:
							return this.Bass;

						case (int) EKeyConfigPart.SYSTEM:
							return this.System;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch( index )
					{
						case (int) EKeyConfigPart.DRUMS:
							this.Drums = value;
							return;

						case (int) EKeyConfigPart.GUITAR:
							this.Guitar = value;
							return;

						case (int) EKeyConfigPart.BASS:
							this.Bass = value;
							return;

						case (int) EKeyConfigPart.SYSTEM:
							this.System = value;
							return;
					}
					throw new IndexOutOfRangeException();
				}
			}
		}
        public enum ESoundDeviceTypeForConfig
        {
            ACM = 0,
            ASIO,
            WASAPI,
			WASAPI_Share,
			Unknown = 99
        }

		// プロパティ

#if false		// #23625 2011.1.11 Config.iniからダメージ/回復値の定数変更を行う場合はここを有効にする 087リリースに合わせ機能無効化
		//----------------------------------------
		public float[,] fGaugeFactor = new float[5,2];
		public float[] fDamageLevelFactor = new float[3];
		//----------------------------------------
#endif
		public int nBGAlpha;
        public int nMovieAlpha;
		public bool bAVIEnabled;
		public bool bBGAEnabled;
		public bool bBGM音を発声する;
		public STDGBVALUE<bool> bHidden;
		public STDGBVALUE<bool> bLeft;
		public STDGBVALUE<bool> bLight;
		public STDGBVALUE<bool> bSpecialist; // 2024.02.22 Add Specialist mode for Guitar/Bass
		public bool bLogDTX詳細ログ出力;
		public bool bLogSongSearch;
		public bool bLog作成解放ログ出力;
		public STDGBVALUE<bool> bReverse;
		public bool bScoreIniを出力する;
		public bool bSTAGEFAILEDEnabled;
		public STDGBVALUE<bool> bSudden;
		public bool bTight;
		public STDGBVALUE<bool> bGraph有効;     // #24074 2011.01.23 add ikanick
		public bool bSmallGraph;
		public bool bWave再生位置自動調整機能有効;
		public bool bシンバルフリー;
		public bool bストイックモード;
		public bool bドラム打音を発声する;
		public bool bFillInEnabled;
		public bool bランダムセレクトで子BOXを検索対象とする;
		public bool bOutputLogs;
		public STDGBVALUE<bool> b演奏音を強調する;
		public bool b演奏情報を表示する;
        public bool bAutoAddGage; //2012.9.18
		public bool b歓声を発声する;
		public bool bVerticalSyncWait;
		public bool b選曲リストフォントを斜体にする;
		public bool b選曲リストフォントを太字にする;
        //public bool bDirectShowMode;
		public bool bFullScreenMode;
		public bool bFullScreenExclusive;
		public int n初期ウィンドウ開始位置X; // #30675 2013.02.04 ikanick add
	    public int n初期ウィンドウ開始位置Y;
		public int nウインドウwidth;				// #23510 2010.10.31 yyagi add
		public int nウインドウheight;				// #23510 2010.10.31 yyagi add
        public bool DisplayBonusEffects;
        public bool bHAZARD;
        public int nSoundDeviceType; // #24820 2012.12.23 yyagi 出力サウンドデバイス(0=ACM(にしたいが設計がきつそうならDirectShow), 1=ASIO, 2=WASAPI)
        public int nWASAPIBufferSizeMs; // #24820 2013.1.15 yyagi WASAPIのバッファサイズ
        //public int nASIOBufferSizeMs; // #24820 2012.12.28 yyagi ASIOのバッファサイズ
        public int nASIODevice; // #24820 2013.1.17 yyagi ASIOデバイス
		public bool bEventDrivenWASAPI;
		public bool bMetronome; // 2023.9.22 henryzx
		public bool bUseOSTimer;
        public bool bDynamicBassMixerManagement; // #24820
		public int nMasterVolume;
		public int nChipPlayTimeComputeMode; // 2024.2.17 fisyher (0=Original, 1=Accurate)

        public STDGBVALUE<EType> eAttackEffect;
        public STDGBVALUE<EType> eNumOfLanes;
        public STDGBVALUE<EType> eDkdkType;
        public STDGBVALUE<EType> eLaneType;
        public STDGBVALUE<EType> eLBDGraphics;
        public STDGBVALUE<EType> eHHOGraphics;
        public ERDPosition eRDPosition;
        public int nInfoType;
        public int nSkillMode;
		public Dictionary<int, string> dicJoystick;
		public ECYGroup eCYGroup;
		public EDarkMode eDark;
		public EFTGroup eFTGroup;
		public EHHGroup eHHGroup;
		public EBDGroup eBDGroup;					// #27029 2012.1.4 from add
		public EPlaybackPriority eHitSoundPriorityCY;
		public EPlaybackPriority eHitSoundPriorityFT;
		public EPlaybackPriority eHitSoundPriorityHH;
        public EPlaybackPriority eHitSoundPriorityLP;
        public STDGBVALUE<ERandomMode> eRandom;
        public STDGBVALUE<ERandomMode> eRandomPedal;
        public STDGBVALUE<bool> bAssignToLBD;
		public EDamageLevel eDamageLevel;
        public CKeyAssign KeyAssign;

        public STDGBVALUE<int> nLaneDisp;
		public STDGBVALUE<bool> bDisplayJudge;
		public STDGBVALUE<bool> bJudgeLineDisp;
        public STDGBVALUE<bool> bLaneFlush;

        public int nPedalLagTime;   //#xxxxx 2013.07.11 kairera0467

		public int n非フォーカス時スリープms;       // #23568 2010.11.04 ikanick add
		public int nフレーム毎スリープms;			// #xxxxx 2011.11.27 yyagi add
		public int nPlaySpeed;
		public bool bSaveScoreIfModifiedPlaySpeed;
		public int n曲が選択されてからプレビュー音が鳴るまでのウェイトms;
		public int n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms;
		public int n自動再生音量;  // nAutoVolume
		public int n手動再生音量;  // nChipVolume
		public int n選曲リストフォントのサイズdot;
        public int[] nNameColor;
		public STDGBVALUE<int> n表示可能な最小コンボ数;
		public STDGBVALUE<int> nScrollSpeed;
		public string strDTXManiaのバージョン;
		public string str曲データ検索パス;
		public string str選曲リストフォント;
        public string[] strCardName; //2015.12.3 kaiera0467 DrumとGuitarとBassで名前を別々にするため、string[3]に変更。
        public string[] strGroupName;
		public EDrumComboTextDisplayPosition ドラムコンボ文字の表示位置;
        public bool bドラムコンボ文字の表示;
        public STDGBVALUE<EType> JudgementStringPosition;  // 判定文字表示位置
		public int nMovieMode;
        public STDGBVALUE<int> nJudgeLine;
        public STDGBVALUE<int> nShutterInSide;
        public STDGBVALUE<int> nShutterOutSide;
        public bool bCLASSIC譜面判別を有効にする;
        public bool bMutingLP;
        public bool bSkillModeを自動切換えする;

        public bool b曲名表示をdefのものにする;

        #region [ XGオプション ]
        public EType eNamePlate;
        public EType eドラムセットを動かす;
        public EType eBPMbar;
        public bool bLivePoint;
        public bool bSpeaker;
        #endregion

        #region [ GDオプション ]
        public bool b難易度表示をXG表示にする;
        public bool bShowMusicInfo;
        public bool bShowScore;
        public string str曲名表示フォント;
        #endregion

        #region[ 画像関連 ]
        public int nJudgeAnimeType;
        public int nJudgeFrames;
        public int nJudgeInterval;
        public int nJudgeWidgh;
        public int nJudgeHeight;
        public int nExplosionFrames;
        public int nExplosionInterval;
        public int nExplosionWidgh;
        public int nExplosionHeight;
        public int nWailingFireFrames;
        public int nWailingFireInterval;
        public int nWailingFireWidgh;
        public int nWailingFireHeight;
        public STDGBVALUE<int> nWailingFireX;
        public int nWailingFireY;
        #endregion

        public STDGBVALUE<int> nInputAdjustTimeMs;	// #23580 2011.1.3 yyagi タイミングアジャスト機能
        public int nCommonBGMAdjustMs;              // #36372 2016.06.19 kairera0467 全曲共通のBGMオフセット
        public STDGBVALUE<int> nJudgeLinePosOffset; // #31602 2013.6.23 yyagi 判定ライン表示位置のオフセット
        public int nShowLagType;					// #25370 2011.6.5 yyagi ズレ時間表示機能
        public int nShowLagTypeColor;
		public bool bShowLagHitCount;				// fisyher New Config to enable hit count display or not
		public int nShowPlaySpeed;
        public STDGBVALUE<int> nHidSud;
        public bool bIsAutoResultCapture;			// #25399 2011.6.9 yyagi リザルト画像自動保存機能のON/OFF制御
		public int nPoliphonicSounds;				// #28228 2012.5.1 yyagi レーン毎の最大同時発音数
		public bool bバッファ入力を行う;
		public bool bIsEnabledSystemMenu;			// #28200 2012.5.1 yyagi System Menuの使用可否切替
		public string strSystemSkinSubfolderFullName;	// #28195 2012.5.2 yyagi Skin切替用 System/以下のサブフォルダ名
		public bool bUseBoxDefSkin;                     // #28195 2012.5.6 yyagi Skin切替用 box.defによるスキン変更機能を使用するか否か
		public int nSkipTimeMs;

        //つまみ食い
        public STDGBVALUE<EAutoGhostData> eAutoGhost;               // #35411 2015.8.18 chnmr0 プレー時使用ゴーストデータ種別
        public STDGBVALUE<ETargetGhostData> eTargetGhost;               // #35411 2015.8.18 chnmr0 ゴーストデータ再生方法

        public bool bConfigIniがないかDTXManiaのバージョンが異なる
		{
			get
			{
				return ( !this.bConfigIniが存在している || !CDTXMania.VERSION.Equals( this.strDTXManiaのバージョン ) );
			}
		}
		public bool bDrumsEnabled
		{
			get
			{
				return this._bDrums有効;
			}
			set
			{
				this._bDrums有効 = value;
				if( !this._bGuitar有効 && !this._bDrums有効 )
				{
					this._bGuitar有効 = true;
				}
			}
		}

		public bool bInstrumentAvailable(EInstrumentPart inst)
		{
			switch (inst)
			{
				case EInstrumentPart.DRUMS:
					return this._bDrums有効;
				case EInstrumentPart.GUITAR:
				case EInstrumentPart.BASS:
					return this._bGuitar有効;
				default:
					return false;
			}
		}

		public bool bEnterがキー割り当てのどこにも使用されていない
		{
			get
			{
				for( int i = 0; i <= (int)EKeyConfigPart.SYSTEM; i++ )
				{
					for( int j = 0; j < (int)EKeyConfigPad.MAX; j++ )
					{
						for( int k = 0; k < 0x10; k++ )
						{
							if( ( this.KeyAssign[ i ][ j ][ k ].InputDevice == EInputDevice.Keyboard ) && ( this.KeyAssign[ i ][ j ][ k ].Code == (int) SlimDXKey.Return ) )
							{
								return false;
							}
						}
					}
				}
				return true;
			}
		}
		public bool bGuitarEnabled
		{
			get
			{
				return this._bGuitar有効;
			}
			set
			{
				this._bGuitar有効 = value;
				if( !this._bGuitar有効 && !this._bDrums有効 )
				{
					this._bDrums有効 = true;
				}
			}
		}
		public bool bWindowMode
		{
			get
			{
				return !this.bFullScreenMode;
			}
			set
			{
				this.bFullScreenMode = !value;
			}
		}
		public bool bGuitarRevolutionMode
		{
			get
			{
				return ( !this.bDrumsEnabled && this.bGuitarEnabled );
			}
		}
		public bool bAllDrumsAreAutoPlay
		{
			get
			{
                for (int i = (int) ELane.LC; i < (int) ELane.LBD; i++)
				{
					if( !this.bAutoPlay[ i ] )
					{
						return false;
					}
				}
				return true;
			}
		}
		public bool bAllGuitarsAreAutoPlay
		{
			get
			{
				for ( int i = (int) ELane.GtR; i <= (int) ELane.GtPick; i++ )
				{
					if ( !this.bAutoPlay[ i ] )
					{
						return false;
					}
				}
				return true;
			}
		}
		public bool bAllBassAreAutoPlay
		{
			get
			{
				for ( int i = (int) ELane.BsR; i <= (int) ELane.BsPick; i++ )
				{
					if ( !this.bAutoPlay[ i ] )
					{
						return false;
					}
				}
				return true;
			}
		}

		public bool bIsAutoPlay(EInstrumentPart inst)
		{
			bool result = false;
			switch (inst)
			{
				case EInstrumentPart.DRUMS:
					result = bAllDrumsAreAutoPlay;
					break;
				case EInstrumentPart.GUITAR:
					result = bAllGuitarsAreAutoPlay;
					break;
				case EInstrumentPart.BASS:
					result = bAllBassAreAutoPlay;
					break;
			}
			return result;
		}

		public bool b演奏情報を表示しない
		{
			get
			{
				return !this.b演奏情報を表示する;
			}
			set
			{
				this.b演奏情報を表示する = !value;
			}
		}
		public int nBackgroundTransparency
		{
			get
			{
				return this.nBGAlpha;
			}
			set
			{
				if( value < 0 )
				{
					this.nBGAlpha = 0;
				}
				else if( value > 0xff )
				{
					this.nBGAlpha = 0xff;
				}
				else
				{
					this.nBGAlpha = value;
				}
			}
		}
		public int nRisky;						// #23559 2011.6.20 yyagi Riskyでの残ミス数。0で閉店
		public bool bIsAllowedDoubleClickFullscreen;	// #26752 2011.11.27 yyagi ダブルクリックしてもフルスクリーンに移行しない
		public bool bIsSwappedGuitarBass			// #24063 2011.1.16 yyagi ギターとベースの切り替え中か否か
		{
			get;
			set;
		}
		public bool bIsSwappedGuitarBass_AutoFlagsAreSwapped	// #24415 2011.2.21 yyagi FLIP中にalt-f4終了で、AUTOフラグがswapした状態でconfig.iniが出力されてしまうことを避けるためのフラグ
		{
		    get;
		    set;
		}
        public bool bTimeStretch;					// #23664 2013.2.24 yyagi ピッチ変更無しで再生速度を変更するかどうか
		public STAUTOPLAY bAutoPlay;

		/// <summary>
		/// The <see cref="STHitRanges"/> for all drum chips, except pedals.
		/// </summary>
		public STHitRanges stDrumHitRanges;

		/// <summary>
		/// The <see cref="STHitRanges"/> for drum pedal chips.
		/// </summary>
		public STHitRanges stDrumPedalHitRanges;

		/// <summary>
		/// The <see cref="STHitRanges"/> for guitar chips.
		/// </summary>
		public STHitRanges stGuitarHitRanges;

		/// <summary>
		/// The <see cref="STHitRanges"/> for bass guitar chips.
		/// </summary>
		public STHitRanges stBassHitRanges;

		/// <summary>
		/// Whether or not Discord Rich Presence integration is enabled.
		/// </summary>
		public bool bDiscordRichPresenceEnabled;

		/// <summary>
		/// The unique client identifier of the Discord Application to use for Discord Rich Presence integration.
		/// </summary>
		public string strDiscordRichPresenceApplicationID;

		/// <summary>
		/// The unique identifier of the large image to display alongside presences.
		/// </summary>
		public string strDiscordRichPresenceLargeImageKey;

		/// <summary>
		/// The unique identifier of the small image to display alongside presences when playing in drum mode.
		/// </summary>
		public string strDiscordRichPresenceSmallImageKeyDrums;

		/// <summary>
		/// The unique identifier of the small image to display alongside presences when playing in guitar mode.
		/// </summary>
		public string strDiscordRichPresenceSmallImageKeyGuitar;

		public STLANEVALUE nVelocityMin;
		[StructLayout( LayoutKind.Sequential )]
		public struct STLANEVALUE
		{
			public int LC;
			public int HH;
			public int SD;
			public int BD;
			public int HT;
			public int LT;
			public int FT;
			public int CY;
			public int RD;
            public int LP;
            public int LBD;
			public int Guitar;
			public int Bass;
			public int this[ int index ]
			{
				get
				{
					switch( index )
					{
						case 0:
							return this.LC;

						case 1:
							return this.HH;

						case 2:
							return this.SD;

						case 3:
							return this.BD;

						case 4:
							return this.HT;

						case 5:
							return this.LT;

						case 6:
							return this.FT;

						case 7:
							return this.CY;

						case 8:
							return this.RD;

                        case 9:
                            return this.LP;

                        case 10:
                            return this.LBD;

						case 11:
							return this.Guitar;

						case 12:
							return this.Bass;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch( index )
					{
						case 0:
							this.LC = value;
							return;

						case 1:
							this.HH = value;
							return;

						case 2:
							this.SD = value;
							return;

						case 3:
							this.BD = value;
							return;

						case 4:
							this.HT = value;
							return;

						case 5:
							this.LT = value;
							return;

						case 6:
							this.FT = value;
							return;

						case 7:
							this.CY = value;
							return;

						case 8:
							this.RD = value;
							return;

                        case 9:
                            this.LP = value;
                            return;

                        case 10:
                            this.LBD = value;
                            return;

						case 11:
							this.Guitar = value;
							return;

						case 12:
							this.Bass = value;
							return;
					}
					throw new IndexOutOfRangeException();
				}
			}
		}

        /*
		// #27029 2012.1.5 from:
		// BDGroup が FP|BD→FP&BD に変化した際に自動変化するパラメータの値のバックアップ。FP&BD→FP|BD の時に元に戻す。
		// これらのバックアップ値は、BDGroup が FP&BD 状態の時にのみ Config.ini に出力され、BDGroup が FP|BD に戻ったら Config.ini から消える。
		public class CBackupOf1BD
		{
			public EHHGroup eHHGroup = EHHGroup.全部打ち分ける;
			public EPlaybackPriority eHitSoundPriorityHH = EPlaybackPriority.ChipOverPadPriority;
		}
		public CBackupOf1BD BackupOf1BD = null;
        */
        public void SwapGuitarBassInfos_AutoFlags()
        {
            //bool ts = CDTXMania.ConfigIni.bAutoPlay.Bass;			// #24415 2011.2.21 yyagi: FLIP時のリザルトにAUTOの記録が混ざらないよう、AUTOのフラグもswapする
            //CDTXMania.ConfigIni.bAutoPlay.Bass = CDTXMania.ConfigIni.bAutoPlay.Guitar;
            //CDTXMania.ConfigIni.bAutoPlay.Guitar = ts;

            int looptime = (int)ELane.GtW - (int)ELane.GtR + 1;		// #29390 2013.1.25 yyagi ギターのAutoLane/AutoPick対応に伴い、FLIPもこれに対応
            for (int i = 0; i < looptime; i++)							// こんなに離れたところを独立して修正しなければならない設計ではいけませんね___
            {
                bool b = CDTXMania.ConfigIni.bAutoPlay[i + (int)ELane.BsR];
                CDTXMania.ConfigIni.bAutoPlay[i + (int)ELane.BsR] = CDTXMania.ConfigIni.bAutoPlay[i + (int)ELane.GtR];
                CDTXMania.ConfigIni.bAutoPlay[i + (int)ELane.GtR] = b;
            }

            CDTXMania.ConfigIni.bIsSwappedGuitarBass_AutoFlagsAreSwapped = !CDTXMania.ConfigIni.bIsSwappedGuitarBass_AutoFlagsAreSwapped;
        }

		public EInstrumentPart GetFlipInst(EInstrumentPart inst)
		{
			EInstrumentPart retPart = inst;
			if (bIsSwappedGuitarBass)
			{
				switch (inst)
				{
					case EInstrumentPart.GUITAR:
						retPart = EInstrumentPart.BASS;
						break;
					case EInstrumentPart.BASS:
						retPart = EInstrumentPart.GUITAR;
						break;
				}
			}
			return retPart;
		}

		// コンストラクタ

		public CConfigIni()
		{
#if false		// #23625 2011.1.11 Config.iniからダメージ/回復値の定数変更を行う場合はここを有効にする 087リリースに合わせ機能無効化
			//----------------------------------------
			this.fGaugeFactor[0,0] =  0.004f;
			this.fGaugeFactor[0,1] =  0.006f;
			this.fGaugeFactor[1,0] =  0.002f;
			this.fGaugeFactor[1,1] =  0.003f;
			this.fGaugeFactor[2,0] =  0.000f;
			this.fGaugeFactor[2,1] =  0.000f;
			this.fGaugeFactor[3,0] = -0.020f;
			this.fGaugeFactor[3,1] = -0.030f;
			this.fGaugeFactor[4,0] = -0.050f;
			this.fGaugeFactor[4,1] = -0.050f;

			this.fDamageLevelFactor[0] = 0.5f;
			this.fDamageLevelFactor[1] = 1.0f;
			this.fDamageLevelFactor[2] = 1.5f;
			//----------------------------------------
#endif
			this.strDTXManiaのバージョン = "Unknown";
			this.str曲データ検索パス = @".\";
			this.bFullScreenMode = false;
			this.bFullScreenExclusive = true;
			this.bVerticalSyncWait = true;
            this.n初期ウィンドウ開始位置X = 0; // #30675 2013.02.04 ikanick add
            this.n初期ウィンドウ開始位置Y = 0;
            //this.bDirectShowMode = true;
			this.nウインドウwidth = SampleFramework.GameWindowSize.Width;			// #23510 2010.10.31 yyagi add
			this.nウインドウheight = SampleFramework.GameWindowSize.Height;			// 
            this.nMovieMode = 1;
            this.nMovieAlpha = 0;
            this.nJudgeLine.Drums = 0;
            this.nJudgeLine.Guitar = 0;
            this.nJudgeLine.Bass = 0;
            this.nShutterInSide = new STDGBVALUE<int>();
            this.nShutterInSide.Drums = 0;
            this.nShutterOutSide = new STDGBVALUE<int>();
            this.nShutterOutSide.Drums = 0;
			this.nフレーム毎スリープms = -1;			// #xxxxx 2011.11.27 yyagi add
			this.n非フォーカス時スリープms = 1;			// #23568 2010.11.04 ikanick add
			this._bGuitar有効 = false;
			this._bDrums有効 = true;
			this.nBGAlpha = 255;
			this.eDamageLevel = EDamageLevel.Normal;
			this.bSTAGEFAILEDEnabled = true;
			this.bAVIEnabled = true;
			this.bBGAEnabled = true;
			this.bFillInEnabled = true;
            this.DisplayBonusEffects = true;
            this.eRDPosition = ERDPosition.RCRD;
            this.nInfoType = 1;
            this.nSkillMode = 1;
            this.eAttackEffect.Drums = EType.A;
            this.eAttackEffect.Guitar = EType.A;
            this.eAttackEffect.Bass = EType.A;
            this.eLaneType = new STDGBVALUE<EType>();
            this.eLaneType.Drums = EType.A;
            this.eHHOGraphics = new STDGBVALUE<EType>();
            this.eHHOGraphics.Drums = EType.A;
            this.eLBDGraphics = new STDGBVALUE<EType>();
            this.eLBDGraphics.Drums = EType.A;
            this.eDkdkType = new STDGBVALUE<EType>();
            this.eDkdkType.Drums = EType.A;
            this.eNumOfLanes = new STDGBVALUE<EType>();
            this.eNumOfLanes.Drums = EType.A;
            this.eNumOfLanes.Guitar = EType.A;
            this.eNumOfLanes.Bass = EType.A;
            this.bAssignToLBD = default(STDGBVALUE<bool>);
            this.bAssignToLBD.Drums = false;
            this.eRandom = default(STDGBVALUE<ERandomMode>);
            this.eRandom.Drums = ERandomMode.OFF;
            this.eRandom.Guitar = ERandomMode.OFF;
            this.eRandom.Bass = ERandomMode.OFF;
            this.eRandomPedal = default(STDGBVALUE<ERandomMode>);
            this.eRandomPedal.Drums = ERandomMode.OFF;
            this.eRandomPedal.Guitar = ERandomMode.OFF;
            this.eRandomPedal.Bass = ERandomMode.OFF;
            this.nLaneDisp = new STDGBVALUE<int>();
            this.nLaneDisp.Drums = 0;
            this.nLaneDisp.Guitar = 0;
            this.nLaneDisp.Bass = 0;
			this.bDisplayJudge = new STDGBVALUE<bool>();
			this.bDisplayJudge.Drums = true;
			this.bDisplayJudge.Guitar = true;
			this.bDisplayJudge.Bass = true;
			this.bJudgeLineDisp = new STDGBVALUE<bool>();
            this.bJudgeLineDisp.Drums = true;
            this.bJudgeLineDisp.Guitar = true;
            this.bJudgeLineDisp.Bass = true;
            this.bLaneFlush = new STDGBVALUE<bool>();
            this.bLaneFlush.Drums = true;
            this.bLaneFlush.Guitar = true;
            this.bLaneFlush.Bass = true;

            this.strCardName = new string[ 3 ];
            this.strGroupName = new string[ 3 ];
            this.nNameColor = new int[ 3 ];

            #region[ 画像関連 ]
            this.nJudgeAnimeType = 1;
            this.nJudgeFrames = 24;
            this.nJudgeInterval = 14;
            this.nJudgeWidgh = 250;
            this.nJudgeHeight = 170;

            this.nExplosionFrames = 1;
            this.nExplosionInterval = 50;
            this.nExplosionWidgh = 0;
            this.nExplosionHeight = 0;

            this.nWailingFireFrames = 0;
            this.nWailingFireInterval = 0;
            this.nWailingFireWidgh = 0;
            this.nWailingFireHeight = 0;
            this.nWailingFireY = 0;
            #endregion

            this.nPedalLagTime = 0;

            this.bAutoAddGage = false;
			this.n曲が選択されてからプレビュー音が鳴るまでのウェイトms = 1000;
			this.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms = 100;
			this.bWave再生位置自動調整機能有効 = true;
			this.bBGM音を発声する = true;
			this.bドラム打音を発声する = true;
			this.b歓声を発声する = true;
			this.bScoreIniを出力する = true;
			this.bランダムセレクトで子BOXを検索対象とする = true;
			this.n表示可能な最小コンボ数 = new STDGBVALUE<int>();
			this.n表示可能な最小コンボ数.Drums = 10;
			this.n表示可能な最小コンボ数.Guitar = 2;
			this.n表示可能な最小コンボ数.Bass = 2;
			this.str選曲リストフォント = "MS PGothic";
			this.n選曲リストフォントのサイズdot = 20;
			this.b選曲リストフォントを太字にする = true;
			this.n自動再生音量 = 80;
			this.n手動再生音量 = 100;
			this.bOutputLogs = true;
            this.b曲名表示をdefのものにする = true;
			this.b演奏音を強調する = new STDGBVALUE<bool>();
			this.bSudden = new STDGBVALUE<bool>();
			this.bHidden = new STDGBVALUE<bool>();
			this.bReverse = new STDGBVALUE<bool>();
			this.eRandom = new STDGBVALUE<ERandomMode>();
			this.bLight = new STDGBVALUE<bool>();
			this.bSpecialist = new STDGBVALUE<bool>();
			this.bLeft = new STDGBVALUE<bool>();
            this.JudgementStringPosition = new STDGBVALUE<EType>();
			this.nScrollSpeed = new STDGBVALUE<int>();
			this.nInputAdjustTimeMs = new STDGBVALUE<int>();	// #23580 2011.1.3 yyagi
            this.nCommonBGMAdjustMs = 0; // #36372 2016.06.19 kairera0467
            this.nJudgeLinePosOffset = new STDGBVALUE<int>(); // #31602 2013.6.23 yyagi
			for ( int i = 0; i < 3; i++ )
			{
				this.b演奏音を強調する[ i ] = true;
				this.bSudden[ i ] = false;
				this.bHidden[ i ] = false;
				this.bReverse[ i ] = false;
				this.eRandom[ i ] = ERandomMode.OFF;
				this.bLight[ i ] = true; //fisyher: Change to default true, following actual game
				this.bSpecialist[ i ] = false;
				this.bLeft[ i ] = false;
				this.JudgementStringPosition[ i ] = EType.A;
				this.nScrollSpeed[ i ] = 1;
				this.nInputAdjustTimeMs[ i ] = 0;
                this.nJudgeLinePosOffset[i] = 0;
			}
			this.nPlaySpeed = 20;
			this.bSaveScoreIfModifiedPlaySpeed = false;
			this.bSmallGraph = true;
            this.ドラムコンボ文字の表示位置 = EDrumComboTextDisplayPosition.RIGHT;
            this.bドラムコンボ文字の表示 = true;
            this.bCLASSIC譜面判別を有効にする = false;
            this.bSkillModeを自動切換えする = false;
            this.bMutingLP = true;
            #region [ AutoPlay ]
            this.bAutoPlay = new STAUTOPLAY();
			this.bAutoPlay.HH = false;
			this.bAutoPlay.SD = false;
			this.bAutoPlay.BD = false;
			this.bAutoPlay.HT = false;
			this.bAutoPlay.LT = false;
			this.bAutoPlay.FT = false;
			this.bAutoPlay.CY = false;
            this.bAutoPlay.RD = false;
			this.bAutoPlay.LC = false;
            this.bAutoPlay.LP = false;
            this.bAutoPlay.LBD = false;
			//this.bAutoPlay.Guitar = true;
			//this.bAutoPlay.Bass = true;
			this.bAutoPlay.GtR = false;
			this.bAutoPlay.GtG = false;
			this.bAutoPlay.GtB = false;
            this.bAutoPlay.GtY = false;
            this.bAutoPlay.GtP = false;
			this.bAutoPlay.GtPick = false;
			this.bAutoPlay.GtW = false;
			this.bAutoPlay.BsR = false;
			this.bAutoPlay.BsG = false;
			this.bAutoPlay.BsB = false;
            this.bAutoPlay.BsY = false;
            this.bAutoPlay.BsP = false;
			this.bAutoPlay.BsPick = false;
			this.bAutoPlay.BsW = false;
			#endregion

			#region [ HitRange ]

			stDrumHitRanges = STHitRanges.tCreateDefaultDTXHitRanges();
			stDrumPedalHitRanges = STHitRanges.tCreateDefaultDTXHitRanges();
			stGuitarHitRanges = STHitRanges.tCreateDefaultDTXHitRanges();
			stBassHitRanges = STHitRanges.tCreateDefaultDTXHitRanges();

			#endregion

			#region [ DiscordRichPresence ]
			this.bDiscordRichPresenceEnabled = false;
			this.strDiscordRichPresenceApplicationID = @"802329379979657257";
			this.strDiscordRichPresenceLargeImageKey = @"dtxmania";
			this.strDiscordRichPresenceSmallImageKeyDrums = @"drums";
			this.strDiscordRichPresenceSmallImageKeyGuitar = @"guitar";
			#endregion

			this.ConfigIniファイル名 = "";
			this.dicJoystick = new Dictionary<int, string>( 10 );
			this.tSetDefaultKeyAssignments();
            #region [ velocityMin ]
            this.nVelocityMin.LC = 0;					// #23857 2011.1.31 yyagi VelocityMin
			this.nVelocityMin.HH = 20;
			this.nVelocityMin.SD = 0;
			this.nVelocityMin.BD = 0;
			this.nVelocityMin.HT = 0;
			this.nVelocityMin.LT = 0;
			this.nVelocityMin.FT = 0;
			this.nVelocityMin.CY = 0;
			this.nVelocityMin.RD = 0;
            this.nVelocityMin.LP = 0;
            this.nVelocityMin.LBD = 0;
            #endregion

            this.bHAZARD = false;
			this.nRisky = 0;							// #23539 2011.7.26 yyagi RISKYモード
			this.nShowLagType = (int) EShowLagType.OFF;	// #25370 2011.6.3 yyagi ズレ時間表示
            this.nShowLagTypeColor = 0;
			this.bShowLagHitCount = false;
			this.nShowPlaySpeed = (int)EShowPlaySpeed.IF_CHANGED_IN_GAME;
			this.bIsAutoResultCapture = true;			// #25399 2011.6.9 yyagi リザルト画像自動保存機能ON/OFF

            #region [ XGオプション ]
            this.bLivePoint = true;
            this.bSpeaker = true;
            this.eNamePlate = EType.A;
            #endregion

            #region [ GDオプション ]
            this.b難易度表示をXG表示にする = true;
            this.bShowScore = true;
            this.bShowMusicInfo = true;
            this.str曲名表示フォント = "MS PGothic";
            #endregion

			this.bバッファ入力を行う = true;
			this.bIsSwappedGuitarBass = false;			// #24063 2011.1.16 yyagi ギターとベースの切り替え
			this.bIsAllowedDoubleClickFullscreen = true;	// #26752 2011.11.26 ダブルクリックでのフルスクリーンモード移行を許可
			this.eBDGroup = EBDGroup.打ち分ける;		// #27029 2012.1.4 from HHPedalとBassPedalのグルーピング
            this.nPoliphonicSounds = 4;                 // #28228 2012.5.1 yyagi レーン毎の最大同時発音数
                                                        // #24820 2013.1.15 yyagi 初期値を4から2に変更。BASS.net使用時の負荷軽減のため。
                                                        // #24820 2013.1.17 yyagi 初期値を4に戻した。動的なミキサー制御がうまく動作しているため。
			this.bIsEnabledSystemMenu = true;			// #28200 2012.5.1 yyagi System Menuの利用可否切替(使用可)
			this.strSystemSkinSubfolderFullName = "";	// #28195 2012.5.2 yyagi 使用中のSkinサブフォルダ名
			this.bUseBoxDefSkin = true;					// #28195 2012.5.6 yyagi box.defによるスキン切替機能を使用するか否か
            this.bTight = false;                        // #29500 2012.9.11 kairera0467
            this.nSoundDeviceType = (int)ESoundDeviceTypeForConfig.ACM; // #24820 2012.12.23 yyagi 初期値はACM
            this.nWASAPIBufferSizeMs = 0;               // #24820 2013.1.15 yyagi 初期値は0(自動設定)
            this.nASIODevice = 0;                       // #24820 2013.1.17 yyagi
//          this.nASIOBufferSizeMs = 0;                 // #24820 2012.12.25 yyagi 初期値は0(自動設定)
			this.bEventDrivenWASAPI = false;
			this.bUseOSTimer = false; ;                 // #33689 2014.6.6 yyagi 初期値はfalse (FDKのタイマー。ＦＲＯＭ氏考案の独自タイマー)
			this.bDynamicBassMixerManagement = true;    //
			this.nMasterVolume = 100;
            this.bTimeStretch = false;                  // #23664 2013.2.24 yyagi 初期値はfalse (再生速度変更を、ピッチ変更にて行う)
			this.nSkipTimeMs = 5000;
			this.nChipPlayTimeComputeMode = 1;			// 2024.2.17 fisyher Set to Accurate by default

		}
		public CConfigIni( string iniファイル名 )
			: this()
		{
			this.tReadFromFile( iniファイル名 );
		}


		// メソッド

		public void tDeleteAlreadyAssignedInputs( EInputDevice DeviceType, int nID, int nCode )
		{
			for( int i = 0; i <= (int)EKeyConfigPart.SYSTEM; i++ )
			{
				for( int j = 0; j < (int)EKeyConfigPad.MAX; j++ )
				{
					for( int k = 0; k < 0x10; k++ )
					{
						if( ( ( this.KeyAssign[ i ][ j ][ k ].InputDevice == DeviceType ) && ( this.KeyAssign[ i ][ j ][ k ].ID == nID ) ) && ( this.KeyAssign[ i ][ j ][ k ].Code == nCode ) )
						{
							for( int m = k; m < 15; m++ )
							{
								this.KeyAssign[ i ][ j ][ m ] = this.KeyAssign[ i ][ j ][ m + 1 ];
							}
							this.KeyAssign[ i ][ j ][ 15 ].InputDevice = EInputDevice.Unknown;
							this.KeyAssign[ i ][ j ][ 15 ].ID = 0;
							this.KeyAssign[ i ][ j ][ 15 ].Code = 0;
							k--;
						}
					}
				}
			}
		}
		public void tWrite( string iniFilename )
		{
			StreamWriter sw = new StreamWriter( iniFilename, false, Encoding.GetEncoding( "unicode" ) );
			sw.WriteLine( ";-------------------" );
			
			#region [ System ]
			sw.WriteLine( "[System]" );
			sw.WriteLine();

#if false		// #23625 2011.1.11 Config.iniからダメージ/回復値の定数変更を行う場合はここを有効にする 087リリースに合わせ機能無効化
	//------------------------------
			sw.WriteLine("; ライフゲージのパラメータ調整(調整完了後削除予定)");
			sw.WriteLine("; GaugeFactorD: ドラムのPerfect, Great,... の回復量(ライフMAXを1.0としたときの値を指定)");
			sw.WriteLine("; GaugeFactorG:  Gt/BsのPerfect, Great,... の回復量(ライフMAXを1.0としたときの値を指定)");
			sw.WriteLine("; DamageFactorD: DamageLevelがSmall, Normal, Largeの時に対するダメージ係数");
			sw.WriteLine("GaugeFactorD={0}, {1}, {2}, {3}, {4}", this.fGaugeFactor[0, 0], this.fGaugeFactor[1, 0], this.fGaugeFactor[2, 0], this.fGaugeFactor[3, 0], this.fGaugeFactor[4, 0]);
			sw.WriteLine("GaugeFactorG={0}, {1}, {2}, {3}, {4}", this.fGaugeFactor[0, 1], this.fGaugeFactor[1, 1], this.fGaugeFactor[2, 1], this.fGaugeFactor[3, 1], this.fGaugeFactor[4, 1]);
			sw.WriteLine("DamageFactor={0}, {1}, {2}", this.fDamageLevelFactor[0], this.fDamageLevelFactor[1], fDamageLevelFactor[2]);
			sw.WriteLine();
	//------------------------------
#endif
            #region [ Version ]
            sw.WriteLine( "; リリースバージョン" );
			sw.WriteLine( "; Release Version." );
			sw.WriteLine( "Version={0}", CDTXMania.VERSION );
			sw.WriteLine();
            #endregion
            #region [ DTXPath ]
            sw.WriteLine( "; 演奏データの格納されているフォルダへのパス。" );
			sw.WriteLine( @"; セミコロン(;)で区切ることにより複数のパスを指定できます。（例: d:\DTXFiles1\;e:\DTXFiles2\）" );
			sw.WriteLine( "; Pathes for DTX data." );
			sw.WriteLine( @"; You can specify many pathes separated with semicolon(;). (e.g. d:\DTXFiles1\;e:\DTXFiles2\)" );
			sw.WriteLine( "DTXPath={0}", this.str曲データ検索パス );
			sw.WriteLine();
            #endregion
            sw.WriteLine("; プレイヤーネーム。");
            sw.WriteLine(@"; 演奏中のネームプレートに表示される名前を設定できます。");
            sw.WriteLine("; 英字、数字の他、ひらがな、カタカナ、半角カナ、漢字なども入力できます。");
            sw.WriteLine("; 入力されていない場合は「GUEST」と表示されます。");
            sw.WriteLine("CardNameDrums={0}", this.strCardName[ 0 ] );
            sw.WriteLine("CardNameGuitar={0}", this.strCardName[ 1 ] );
            sw.WriteLine("CardNameBass={0}", this.strCardName[ 2 ] );
            sw.WriteLine();
            sw.WriteLine("; グループ名っぽいあれ。");
            sw.WriteLine(@"; 演奏中のネームプレートに表示されるXG2でいうグループ名を設定できます。");
            sw.WriteLine("; 英字、数字の他、ひらがな、カタカナ、半角カナ、漢字なども入力できます。");
            sw.WriteLine("; 入力されていない場合は何も表示されません。");
            sw.WriteLine("GroupNameDrums={0}", this.strGroupName[ 0 ]);
            sw.WriteLine("GroupNameGuitar={0}", this.strGroupName[ 1 ]);
            sw.WriteLine("GroupNameBass={0}", this.strGroupName[ 2 ]);
            sw.WriteLine();
            sw.WriteLine("; ネームカラー");
            sw.WriteLine("; 0=白, 1=薄黄色, 2=黄色, 3=緑, 4=青, 5=紫 以下略。");
            sw.WriteLine("NameColorDrums={0}", this.nNameColor[ 0 ]);
            sw.WriteLine("NameColorGuitar={0}", this.nNameColor[ 1 ]);
            sw.WriteLine("NameColorBass={0}", this.nNameColor[ 2 ]);
            sw.WriteLine();
            sw.WriteLine("; クリップの表示位置");
            sw.WriteLine("; 0=表示しない, 1=全画面, 2=ウインドウ, 3=全画面&ウインドウ");
            sw.WriteLine("MovieMode={0}", this.nMovieMode);
            sw.WriteLine();
            sw.WriteLine("; レーンの透明度(名前に突っ込まないでください。)");
            sw.WriteLine("; 数値が高いほどレーンが薄くなります。");
            sw.WriteLine("; 0=0% 10=100%");
            sw.WriteLine("MovieAlpha={0}", this.nMovieAlpha);
            sw.WriteLine();
            sw.WriteLine("; プレイ中にHelpボタンを押したときに出てくる演奏情報の種類。");
            sw.WriteLine("; 0=デバッグ情報 1=判定情報");
            sw.WriteLine("InfoType={0}", this.nInfoType);
            sw.WriteLine();
			#region [ スキン関連 ]
			#region [ Skinパスの絶対パス→相対パス変換 ]
			Uri uriRoot = new Uri( System.IO.Path.Combine( CDTXMania.strEXEのあるフォルダ, "System" + System.IO.Path.DirectorySeparatorChar ) );
			Uri uriPath = new Uri( System.IO.Path.Combine( this.strSystemSkinSubfolderFullName, "." + System.IO.Path.DirectorySeparatorChar ) );
			string relPath = uriRoot.MakeRelativeUri( uriPath ).ToString();				// 相対パスを取得
			relPath = System.Web.HttpUtility.UrlDecode( relPath );						// デコードする
			relPath = relPath.Replace( '/', System.IO.Path.DirectorySeparatorChar );	// 区切り文字が\ではなく/なので置換する
			#endregion
			sw.WriteLine( "; 使用するSkinのフォルダ名。" );
			sw.WriteLine( "; 例えば System\\Default\\Graphics\\... などの場合は、SkinPath=.\\Default\\ を指定します。" );
			sw.WriteLine( "; Skin folder path." );
			sw.WriteLine( "; e.g. System\\Default\\Graphics\\... -> Set SkinPath=.\\Default\\" );
			sw.WriteLine( "SkinPath={0}", relPath );
			sw.WriteLine();
			sw.WriteLine( "; box.defが指定するSkinに自動で切り替えるかどうか (0=切り替えない、1=切り替える)" );
			sw.WriteLine( "; Automatically change skin specified in box.def. (0=No 1=Yes)" );
			sw.WriteLine( "SkinChangeByBoxDef={0}", this.bUseBoxDefSkin? 1 : 0 );
			sw.WriteLine();
			#endregion
            #region [ Window関連 ]
            sw.WriteLine( "; 画面モード(0:ウィンドウ, 1:全画面)" );
			sw.WriteLine( "; Screen mode. (0:Window, 1:Fullscreen)" );
			sw.WriteLine( "FullScreen={0}", this.bFullScreenMode ? 1 : 0 );
            sw.WriteLine();
			sw.WriteLine("; Fullscreen mode uses DirectX exclusive mode instead of maximized window. (0:Maximized window, 1:Exclusive)");
			sw.WriteLine("FullScreenExclusive={0}", this.bFullScreenExclusive ? 1 : 0);
			sw.WriteLine();
			sw.WriteLine("; ウインドウモード時の画面幅");				// #23510 2010.10.31 yyagi add
			sw.WriteLine("; A width size in the window mode.");			//
			sw.WriteLine("WindowWidth={0}", this.nウインドウwidth);		//
			sw.WriteLine();												//
			sw.WriteLine("; ウインドウモード時の画面高さ");				//
			sw.WriteLine("; A height size in the window mode.");		//
			sw.WriteLine("WindowHeight={0}", this.nウインドウheight);	//
			sw.WriteLine();												//
            sw.WriteLine("; ウィンドウモード時の位置X");				            // #30675 2013.02.04 ikanick add
            sw.WriteLine("; X position in the window mode.");			            //
            sw.WriteLine("WindowX={0}", this.n初期ウィンドウ開始位置X);			//
            sw.WriteLine();											            	//
            sw.WriteLine("; ウィンドウモード時の位置Y");			            	//
            sw.WriteLine("; Y position in the window mode.");	            	    //
            sw.WriteLine("WindowY={0}", this.n初期ウィンドウ開始位置Y);   		//
            sw.WriteLine();												            //

			sw.WriteLine( "; ウインドウをダブルクリックした時にフルスクリーンに移行するか(0:移行しない,1:移行する)" );	// #26752 2011.11.27 yyagi
			sw.WriteLine( "; Whether double click to go full screen mode or not." );					//
			sw.WriteLine( "DoubleClickFullScreen={0}", this.bIsAllowedDoubleClickFullscreen? 1 : 0);	//
			sw.WriteLine();																				//
			sw.WriteLine( "; ALT+SPACEのメニュー表示を抑制するかどうか(0:抑制する 1:抑制しない)" );		// #28200 2012.5.1 yyagi
			sw.WriteLine( "; Whether ALT+SPACE menu would be masked or not.(0=masked 1=not masked)" );	//
			sw.WriteLine( "EnableSystemMenu={0}", this.bIsEnabledSystemMenu? 1 : 0 );					//
			sw.WriteLine();																				//
            sw.WriteLine("; 非フォーカス時のsleep値[ms]");	    			    // #23568 2011.11.04 ikanick add
			sw.WriteLine("; A sleep time[ms] while the window is inactive.");	//
			sw.WriteLine("BackSleep={0}", this.n非フォーカス時スリープms);		// そのまま引用（苦笑）
            sw.WriteLine();											        			//
            #endregion
            #region [ フレーム処理関連(VSync, フレーム毎のsleep) ]
            sw.WriteLine("; 垂直帰線同期(0:OFF,1:ON)");
			sw.WriteLine( "VSyncWait={0}", this.bVerticalSyncWait ? 1 : 0 );
            sw.WriteLine();
            sw.WriteLine("; フレーム毎のsleep値[ms] (-1でスリープ無し, 0以上で毎フレームスリープ。動画キャプチャ等で活用下さい)");	// #xxxxx 2011.11.27 yyagi add
            sw.WriteLine("; A sleep time[ms] per frame.");							//
            sw.WriteLine("SleepTimePerFrame={0}", this.nフレーム毎スリープms); //
            sw.WriteLine();											        			//
            #endregion
            #region [ WASAPI/ASIO関連 ]
            sw.WriteLine("; サウンド出力方式(0=ACM(って今はまだDirectShowですが), 1=ASIO, 2=WASAPI排他, 3=WASAPI共有");
            sw.WriteLine("; WASAPIはVista以降のOSで使用可能。推奨方式はWASAPI。");
            sw.WriteLine("; なお、WASAPIが使用不可ならASIOを、ASIOが使用不可ならACMを使用します。");
            sw.WriteLine("; Sound device type(0=ACM, 1=ASIO, 2=WASAPI Exclusive, 3=WASAPI Shared)");
            sw.WriteLine("; WASAPI can use on Vista or later OSs.");
            sw.WriteLine("; If WASAPI is not available, DTXMania try to use ASIO. If ASIO can't be used, ACM is used.");
            sw.WriteLine("SoundDeviceType={0}", (int)this.nSoundDeviceType);
            sw.WriteLine();

            sw.WriteLine("; WASAPI使用時のサウンドバッファサイズ");
            sw.WriteLine("; (0=デバイスに設定されている値を使用, 1～9999=バッファサイズ(単位:ms)の手動指定");
            sw.WriteLine("; WASAPI Sound Buffer Size.");
            sw.WriteLine("; (0=Use system default buffer size, 1-9999=specify the buffer size(ms) by yourself)");
            sw.WriteLine("WASAPIBufferSizeMs={0}", (int)this.nWASAPIBufferSizeMs);
            sw.WriteLine();

            sw.WriteLine("; ASIO使用時のサウンドデバイス");
            sw.WriteLine("; 存在しないデバイスを指定すると、DTXManiaが起動しないことがあります。");
            sw.WriteLine("; Sound device used by ASIO.");
            sw.WriteLine("; Don't specify unconnected device, as the DTXMania may not bootup.");
            string[] asiodev = CEnumerateAllAsioDevices.GetAllASIODevices();
            for (int i = 0; i < asiodev.Length; i++)
            {
                sw.WriteLine("; {0}: {1}", i, asiodev[i]);
            }
            sw.WriteLine("ASIODevice={0}", (int)this.nASIODevice);
            sw.WriteLine();

			//sw.WriteLine( "; ASIO使用時のサウンドバッファサイズ" );
			//sw.WriteLine( "; (0=デバイスに設定されている値を使用, 1～9999=バッファサイズ(単位:ms)の手動指定" );
			//sw.WriteLine( "; ASIO Sound Buffer Size." );
			//sw.WriteLine( "; (0=Use the value specified to the device, 1-9999=specify the buffer size(ms) by yourself)" );
			//sw.WriteLine( "ASIOBufferSizeMs={0}", (int) this.nASIOBufferSizeMs );
			//sw.WriteLine();

			//sw.WriteLine("; Bass.Mixの制御を動的に行うか否か。");
			//sw.WriteLine("; ONにすると、ギター曲などチップ音の多い曲も再生できますが、画面が少しがたつきます。");
			//sw.WriteLine("; (0=行わない, 1=行う)");
			//sw.WriteLine("DynamicBassMixerManagement={0}", this.bDynamicBassMixerManagement ? 1 : 0);
			//sw.WriteLine();

			sw.WriteLine("; WASAPI/ASIO時に使用する演奏タイマーの種類");
			sw.WriteLine("; Playback timer used for WASAPI/ASIO");
			sw.WriteLine("; (0=FDK Timer, 1=System Timer)");
			sw.WriteLine("SoundTimerType={0}", this.bUseOSTimer ? 1 : 0);
			sw.WriteLine();

			sw.WriteLine("; WASAPI使用時にEventDrivenモードを使う");
			sw.WriteLine("EventDrivenWASAPI={0}", this.bEventDrivenWASAPI ? 1 : 0);
			sw.WriteLine();

			sw.WriteLine("; Enable Embedded Metronome");
			sw.WriteLine("; Please make sure Metronome.ogg exists in Your current skin sounds folder");
            sw.WriteLine("; e.g. ./System/{Skin}/Sounds/Metronome.ogg");
            sw.WriteLine("Metronome={0}", this.bMetronome ? 1 : 0);
			sw.WriteLine();

            sw.WriteLine("; Chip PlayTime Compute Mode");
            sw.WriteLine("; Select which method of Chip PlayTime Computation to use: (0=Original, 1=Accurate)");
            sw.WriteLine("; Original method is compatible with other DTXMania players but loses time due to integer truncation");
            sw.WriteLine("; Accurate method improves overall accuracy by using proper number rounding");
            sw.WriteLine("; NOTE: Only songs with many BPM changes will have observable difference in either mode. Single BPM songs are not affected");
            sw.WriteLine("ChipPlayTimeComputeMode={0}", nChipPlayTimeComputeMode);
            sw.WriteLine();

            sw.WriteLine("; 全体ボリュームの設定");
			sw.WriteLine("; (0=無音 ～ 100=最大。WASAPI/ASIO時のみ有効)");
			sw.WriteLine("; Master volume settings");
			sw.WriteLine("; (0=Silent - 100=Max)");
			sw.WriteLine("MasterVolume={0}", this.nMasterVolume);
			sw.WriteLine();

			#endregion
			#region [ ギター/ベース/ドラム 有効/無効 ]
			sw.WriteLine( "; ギター/ベース有効(0:OFF,1:ON)" );
			sw.WriteLine( "; Enable Guitar/Bass or not.(0:OFF,1:ON)" );
			sw.WriteLine( "Guitar={0}", this.bGuitarEnabled ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; ドラム有効(0:OFF,1:ON)" );
			sw.WriteLine( "; Enable Drums or not.(0:OFF,1:ON)" );
			sw.WriteLine( "Drums={0}", this.bDrumsEnabled ? 1 : 0 );
			sw.WriteLine();
            #endregion
			sw.WriteLine( "; 背景画像の半透明割合(0:透明～255:不透明)" );
			sw.WriteLine( "; Transparency for background image in playing screen.(0:tranaparent - 255:no transparent)" );
			sw.WriteLine( "BGAlpha={0}", this.nBGAlpha );
			sw.WriteLine();
			sw.WriteLine( "; Missヒット時のゲージ減少割合(0:少, 1:Normal, 2:大)" );
			sw.WriteLine( "DamageLevel={0}", (int) this.eDamageLevel );
			sw.WriteLine();
            sw.WriteLine("; ゲージゼロでSTAGE FAILED (0:OFF, 1:ON)");
            sw.WriteLine("StageFailed={0}", this.bSTAGEFAILEDEnabled ? 1 : 0);
            sw.WriteLine();
            #region [ 打ち分け関連 ]
            sw.WriteLine("; LC/HHC/HHO 打ち分けモード (0:LC|HHC|HHO, 1:LC&(HHC|HHO), 2:LC|(HHC&HHO), 3:LC&HHC&HHO)");
            sw.WriteLine("; LC/HHC/HHO Grouping       (0:LC|HHC|HHO, 1:LC&(HHC|HHO), 2:LC|(HHC&HHO), 3:LC&HHC&HHO)");
            sw.WriteLine("HHGroup={0}", (int)this.eHHGroup);
            sw.WriteLine();
            sw.WriteLine("; LT/FT 打ち分けモード (0:LT|FT, 1:LT&FT)");
            sw.WriteLine("; LT/FT Grouping       (0:LT|FT, 1:LT&FT)");
            sw.WriteLine("FTGroup={0}", (int)this.eFTGroup);
            sw.WriteLine();
            sw.WriteLine("; CY/RD 打ち分けモード (0:CY|RD, 1:CY&RD)");
            sw.WriteLine("; CY/RD Grouping       (0:CY|RD, 1:CY&RD)");
            sw.WriteLine("CYGroup={0}", (int)this.eCYGroup);
            sw.WriteLine();
			sw.WriteLine( "; LP/LBD/BD 打ち分けモード(0:LP|LBD|BD, 1:LP|(LBD&BD), 2:LP&(LBD|BD), 3:LP&LBD&BD)" );		// #27029 2012.1.4 from
            sw.WriteLine( "; LP/LBD/BD Grouping     (0:LP|LBD|BD, 1:LP(LBD&BD), 2:LP&(LBD|BD), 3:LP&LBD&BD)");
			sw.WriteLine( "BDGroup={0}", (int) this.eBDGroup );				// 
			sw.WriteLine();													//
			sw.WriteLine( "; 打ち分け時の再生音の優先順位(HHGroup)(0:Chip>Pad, 1:Pad>Chip)" );
			sw.WriteLine( "HitSoundPriorityHH={0}", (int) this.eHitSoundPriorityHH );
			sw.WriteLine();
			sw.WriteLine( "; 打ち分け時の再生音の優先順位(FTGroup)(0:Chip>Pad, 1:Pad>Chip)" );
			sw.WriteLine( "HitSoundPriorityFT={0}", (int) this.eHitSoundPriorityFT );
			sw.WriteLine();
			sw.WriteLine( "; 打ち分け時の再生音の優先順位(CYGroup)(0:Chip>Pad, 1:Pad>Chip)" );
			sw.WriteLine( "HitSoundPriorityCY={0}", (int) this.eHitSoundPriorityCY );
			sw.WriteLine();
            sw.WriteLine( "; 打ち分け時の再生音の優先順位(LPGroup)(0:Chip>Pad, 1:Pad>Chip)");
            sw.WriteLine( "HitSoundPriorityLP={0}", (int) this.eHitSoundPriorityLP);
            sw.WriteLine();
            sw.WriteLine("; シンバルフリーモード(0:OFF, 1:ON)");
            sw.WriteLine("CymbalFree={0}", this.bシンバルフリー ? 1 : 0);
            sw.WriteLine();
            #endregion
            #region [ AVI/BGA ]
			sw.WriteLine( "; AVIの表示(0:OFF, 1:ON)" );
			sw.WriteLine( "AVI={0}", this.bAVIEnabled ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; BGAの表示(0:OFF, 1:ON)" );
			sw.WriteLine( "BGA={0}", this.bBGAEnabled ? 1 : 0 );
			sw.WriteLine();
            #endregion
            #region [ フィルイン ]
            sw.WriteLine( "; フィルイン効果(0:OFF, 1:ON)" );
			sw.WriteLine( "FillInEffect={0}", this.bFillInEnabled ? 1 : 0 );
			sw.WriteLine();
            sw.WriteLine("; フィルイン達成時の歓声の再生(0:OFF, 1:ON)");
            sw.WriteLine("AudienceSound={0}", this.b歓声を発声する ? 1 : 0);
            sw.WriteLine();
            #endregion     
            #region [ プレビュー音 ]
            sw.WriteLine( "; 曲選択からプレビュー音の再生までのウェイト[ms]" );
			sw.WriteLine( "PreviewSoundWait={0}", this.n曲が選択されてからプレビュー音が鳴るまでのウェイトms );
			sw.WriteLine();
			sw.WriteLine( "; 曲選択からプレビュー画像表示までのウェイト[ms]" );
			sw.WriteLine( "PreviewImageWait={0}", this.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms );
			sw.WriteLine();
            #endregion
            sw.WriteLine( "; Waveの再生位置自動補正(0:OFF, 1:ON)" );
			sw.WriteLine( "AdjustWaves={0}", this.bWave再生位置自動調整機能有効 ? 1 : 0 );
			sw.WriteLine();
            #region [ BGM/ドラムヒット音の再生 ]
            sw.WriteLine( "; BGM の再生(0:OFF, 1:ON)" );
			sw.WriteLine( "BGMSound={0}", this.bBGM音を発声する ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; ドラム打音の再生(0:OFF, 1:ON)" );
			sw.WriteLine( "HitSound={0}", this.bドラム打音を発声する ? 1 : 0 );
			sw.WriteLine();
            #endregion
			sw.WriteLine( "; 演奏記録（～.score.ini）の出力 (0:OFF, 1:ON)" );
			sw.WriteLine( "SaveScoreIni={0}", this.bScoreIniを出力する ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; RANDOM SELECT で子BOXを検索対象に含める (0:OFF, 1:ON)" );
			sw.WriteLine( "RandomFromSubBox={0}", this.bランダムセレクトで子BOXを検索対象とする ? 1 : 0 );
			sw.WriteLine();
            #region [ モニターサウンド(ヒット音の再生音量アップ) ]
            sw.WriteLine( "; ドラム演奏時にドラム音を強調する (0:OFF, 1:ON)" );
			sw.WriteLine( "SoundMonitorDrums={0}", this.b演奏音を強調する.Drums ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; ギター演奏時にギター音を強調する (0:OFF, 1:ON)" );
			sw.WriteLine( "SoundMonitorGuitar={0}", this.b演奏音を強調する.Guitar ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; ベース演奏時にベース音を強調する (0:OFF, 1:ON)" );
			sw.WriteLine( "SoundMonitorBass={0}", this.b演奏音を強調する.Bass ? 1 : 0 );
			sw.WriteLine();
            #endregion
            sw.WriteLine( "; 表示可能な最小コンボ数(1～99999)" );
            sw.WriteLine( "; ギターとベースでは0にするとコンボを表示しません。" );
			sw.WriteLine( "MinComboDrums={0}", this.n表示可能な最小コンボ数.Drums );
			sw.WriteLine( "MinComboGuitar={0}", this.n表示可能な最小コンボ数.Guitar );
			sw.WriteLine( "MinComboBass={0}", this.n表示可能な最小コンボ数.Bass );
			sw.WriteLine();
            sw.WriteLine( "; 曲名表示をdefファイルの曲名にする (0:OFF, 1:ON)" );
			sw.WriteLine( "MusicNameDispDef={0}", this.b曲名表示をdefのものにする ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; 演奏情報を表示する (0:OFF, 1:ON)" );
            sw.WriteLine( "; Showing playing info on the playing screen. (0:OFF, 1:ON)" );
			sw.WriteLine( "ShowDebugStatus={0}", this.b演奏情報を表示する ? 1 : 0 );
			sw.WriteLine();
            #region [ GDオプション ]
            sw.WriteLine( "; 選曲画面の難易度表示をXG表示にする (0:OFF, 1:ON)");
            sw.WriteLine( "Difficulty={0}", this.b難易度表示をXG表示にする ? 1 : 0);
            sw.WriteLine();
            sw.WriteLine("; スコアの表示(0:OFF, 1:ON)");
            sw.WriteLine("ShowScore={0}", this.bShowScore ? 1 : 0);
            sw.WriteLine();
            sw.WriteLine("; 演奏中の曲情報の表示(0:OFF, 1:ON)");
            sw.WriteLine("ShowMusicInfo={0}", this.bShowMusicInfo ? 1 : 0);
            sw.WriteLine();
			sw.WriteLine("; Show custom play speed (0:OFF, 1:ON, 2:If changed in game)");    //
			sw.WriteLine("ShowPlaySpeed={0}", this.nShowPlaySpeed);                         //
			sw.WriteLine();
			sw.WriteLine("; 読み込み画面、演奏画面、ネームプレート、リザルト画面の曲名で使用するフォント名");
            sw.WriteLine("DisplayFontName={0}", this.str曲名表示フォント);
            sw.WriteLine();
            #endregion
            #region [ 選曲リストのフォント ]
            sw.WriteLine( "; 選曲リストのフォント名" );
            sw.WriteLine( "; Font name for select song item." );
			sw.WriteLine( "SelectListFontName={0}", this.str選曲リストフォント );
			sw.WriteLine();
			sw.WriteLine( "; 選曲リストのフォントのサイズ[dot]" );
            sw.WriteLine( "; Font size[dot] for select song item." );
			sw.WriteLine( "SelectListFontSize={0}", this.n選曲リストフォントのサイズdot );
			sw.WriteLine();
			sw.WriteLine( "; 選曲リストのフォントを斜体にする (0:OFF, 1:ON)" );
            sw.WriteLine( "; Using italic font style select song list. (0:OFF, 1:ON)" );
			sw.WriteLine( "SelectListFontItalic={0}", this.b選曲リストフォントを斜体にする ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; 選曲リストのフォントを太字にする (0:OFF, 1:ON)" );
            sw.WriteLine( "; Using bold font style select song list. (0:OFF, 1:ON)" );
			sw.WriteLine( "SelectListFontBold={0}", this.b選曲リストフォントを太字にする ? 1 : 0 );
			sw.WriteLine();
            #endregion
            sw.WriteLine( "; 打音の音量(0～100%)" );
            sw.WriteLine( "; Sound volume (you're playing) (0-100%)" );
			sw.WriteLine( "ChipVolume={0}", this.n手動再生音量 );
			sw.WriteLine();
			sw.WriteLine( "; 自動再生音の音量(0～100%)" );
            sw.WriteLine( "; Sound volume (auto playing) (0-100%)" );
			sw.WriteLine( "AutoChipVolume={0}", this.n自動再生音量 );
			sw.WriteLine();
			sw.WriteLine( "; ストイックモード(0:OFF, 1:ON)" );
            sw.WriteLine( "; Stoic mode. (0:OFF, 1:ON)" );
			sw.WriteLine( "StoicMode={0}", this.bストイックモード ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; バッファ入力モード(0:OFF, 1:ON)" );
            sw.WriteLine( "; Using Buffered input (0:OFF, 1:ON)" );
			sw.WriteLine( "BufferedInput={0}", this.bバッファ入力を行う ? 1 : 0 );
			sw.WriteLine();
            sw.WriteLine("; オープンハイハットの表示画像(0:DTXMania仕様, 1:○なし, 2:クローズハットと同じ)");
            sw.WriteLine("HHOGraphics={0}", (int)this.eHHOGraphics.Drums);
            sw.WriteLine();
            sw.WriteLine("; 左バスペダルの表示画像(0:バス寄り, 1:LPと同じ)");
            sw.WriteLine("LBDGraphics={0}", (int)this.eLBDGraphics.Drums);
            sw.WriteLine();
            sw.WriteLine("; ライドシンバルレーンの表示位置(0:...RD RC, 1:...RC RD)");
            sw.WriteLine("RDPosition={0}", (int)this.eRDPosition);
            sw.WriteLine();
			sw.WriteLine( "; レーン毎の最大同時発音数(1～8)" );
			sw.WriteLine( "; Number of polyphonic sounds per lane. (1-8)" );
			sw.WriteLine( "PolyphonicSounds={0}", this.nPoliphonicSounds );
			sw.WriteLine();
			sw.WriteLine( "; 判定ズレ時間表示(0:OFF, 1:ON, 2=GREAT-POOR)" );				// #25370 2011.6.3 yyagi
			sw.WriteLine( "; Whether displaying the lag times from the just timing or not." );	//
			sw.WriteLine( "ShowLagTime={0}", this.nShowLagType );							//
			sw.WriteLine();
			sw.WriteLine("; 判定ズレ時間表示の色(0:Slow赤、Fast青, 1:Slow青、Fast赤)");
			sw.WriteLine( "ShowLagTimeColor={0}", this.nShowLagTypeColor );                         //
			sw.WriteLine();
			sw.WriteLine("; 判定ズレヒット数表示(0:OFF, 1:ON)");
			sw.WriteLine("ShowLagHitCount={0}", this.bShowLagHitCount ? 1 : 0);                         //
			sw.WriteLine();
			sw.WriteLine( "; リザルト画像自動保存機能(0:OFF, 1:ON)" );						// #25399 2011.6.9 yyagi
			sw.WriteLine( "; Set ON if you'd like to save result screen image automatically");	//
			sw.WriteLine( "; when you get hiscore/hiskill.");								//
			sw.WriteLine( "AutoResultCapture={0}", this.bIsAutoResultCapture? 1 : 0 );		//
            sw.WriteLine();
            sw.WriteLine("; 再生速度変更を、ピッチ変更で行うかどうか(0:ピッチ変更, 1:タイムストレッチ");	// #23664 2013.2.24 yyagi
            sw.WriteLine("; (WASAPI/ASIO使用時のみ有効) ");
            sw.WriteLine("; Set \"0\" if you'd like to use pitch shift with PlaySpeed.");	//
            sw.WriteLine("; Set \"1\" for time stretch.");								//
            sw.WriteLine("; (Only available when you're using using WASAPI or ASIO)");	//
            sw.WriteLine("TimeStretch={0}", this.bTimeStretch ? 1 : 0);					//
            sw.WriteLine();
            #region [ Adjust ]
            sw.WriteLine("; 判定タイミング調整(ドラム, ギター, ベース)(-99～99)[ms]");		// #23580 2011.1.3 yyagi
            sw.WriteLine("; Revision value to adjust judgement timing for the drums, guitar and bass.");	//
            sw.WriteLine("InputAdjustTimeDrums={0}", this.nInputAdjustTimeMs.Drums);		//
            sw.WriteLine("InputAdjustTimeGuitar={0}", this.nInputAdjustTimeMs.Guitar);		//
            sw.WriteLine("InputAdjustTimeBass={0}", this.nInputAdjustTimeMs.Bass);			//
            sw.WriteLine();

            sw.WriteLine( "; BGMタイミング調整(-99～99)[ms]" );                              // #36372 2016.06.19 kairera0467
            sw.WriteLine( "; Revision value to adjust judgement timing for BGM." );	        //
            sw.WriteLine( "BGMAdjustTime={0}", this.nCommonBGMAdjustMs );		            //
            sw.WriteLine();

            sw.WriteLine("; 判定ラインの表示位置調整(ドラム, ギター, ベース)(-99～99)[px]"); // #31602 2013.6.23 yyagi 判定ラインの表示位置オフセット
            sw.WriteLine("; Offset value to adjust displaying judgement line for the drums, guitar and bass."); //
            sw.WriteLine("JudgeLinePosOffsetDrums={0}", this.nJudgeLinePosOffset.Drums); //
            sw.WriteLine("JudgeLinePosOffsetGuitar={0}", this.nJudgeLinePosOffset.Guitar); //
            sw.WriteLine("JudgeLinePosOffsetBass={0}", this.nJudgeLinePosOffset.Bass); //
            sw.WriteLine();
            #endregion
            sw.WriteLine( "; LC, HH, SD,...の入力切り捨て下限Velocity値(0～127)" );			// #23857 2011.1.31 yyagi
			sw.WriteLine( "; Minimum velocity value for LC, HH, SD, ... to accept." );		//
			sw.WriteLine( "LCVelocityMin={0}", this.nVelocityMin.LC );						//
			sw.WriteLine("HHVelocityMin={0}", this.nVelocityMin.HH );						//
//			sw.WriteLine("; ハイハット以外の入力切り捨て下限Velocity値(0～127)");			// #23857 2010.12.12 yyagi
//			sw.WriteLine("; Minimum velocity value to accept. (except HiHat)");				//
//			sw.WriteLine("VelocityMin={0}", this.n切り捨て下限Velocity);					//
//			sw.WriteLine();																	//
			sw.WriteLine( "SDVelocityMin={0}", this.nVelocityMin.SD );						//
			sw.WriteLine( "BDVelocityMin={0}", this.nVelocityMin.BD );						//
			sw.WriteLine( "HTVelocityMin={0}", this.nVelocityMin.HT );						//
			sw.WriteLine( "LTVelocityMin={0}", this.nVelocityMin.LT );						//
			sw.WriteLine( "FTVelocityMin={0}", this.nVelocityMin.FT );						//
			sw.WriteLine( "CYVelocityMin={0}", this.nVelocityMin.CY );						//
			sw.WriteLine( "RDVelocityMin={0}", this.nVelocityMin.RD );						//
            sw.WriteLine( "LPVelocityMin={0}", this.nVelocityMin.LP );
            sw.WriteLine( "LBDVelocityMin={0}",this.nVelocityMin.LBD ); 
			sw.WriteLine();																	//
            sw.WriteLine( "; オート時のゲージ加算(0:OFF, 1:ON )");
            sw.WriteLine( "AutoAddGage={0}", this.bAutoAddGage ? 1 : 0);
            sw.WriteLine();
			sw.WriteLine("; Number of milliseconds to skip forward/backward (100-10000)");
			sw.WriteLine("SkipTimeMs={0}", this.nSkipTimeMs);
			sw.WriteLine();

			sw.WriteLine( ";-------------------" );
			#endregion

			#region [ Log ]
			sw.WriteLine( "[Log]" );
			sw.WriteLine();
			sw.WriteLine( "; Log出力(0:OFF, 1:ON)" );
			sw.WriteLine( "OutputLog={0}", this.bOutputLogs ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; 曲データ検索に関するLog出力(0:OFF, 1:ON)" );
			sw.WriteLine( "TraceSongSearch={0}", this.bLogSongSearch ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; 画像やサウンドの作成_解放に関するLog出力(0:OFF, 1:ON)" );
			sw.WriteLine( "TraceCreatedDisposed={0}", this.bLog作成解放ログ出力 ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; DTX読み込み詳細に関するLog出力(0:OFF, 1:ON)" );
			sw.WriteLine( "TraceDTXDetails={0}", this.bLogDTX詳細ログ出力 ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( ";-------------------" );
			#endregion

			#region [ PlayOption ]
			sw.WriteLine( "[PlayOption]" );
			sw.WriteLine();
			sw.WriteLine( "; REVERSEモード(0:OFF, 1:ON)" );
			sw.WriteLine( "DrumsReverse={0}", this.bReverse.Drums ? 1 : 0 );
			sw.WriteLine( "GuitarReverse={0}", this.bReverse.Guitar ? 1 : 0 );
			sw.WriteLine( "BassReverse={0}", this.bReverse.Bass ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; ギター/ベースRANDOMモード(0:OFF, 1:Mirror, 2:Random, 3:SuperRandom, 4:HyperRandom)" );
			sw.WriteLine( "GuitarRandom={0}", (int) this.eRandom.Guitar );
			sw.WriteLine( "BassRandom={0}", (int) this.eRandom.Bass );
			sw.WriteLine();
			sw.WriteLine( "; ギター/ベースLIGHTモード(0:OFF, 1:ON)" );
			sw.WriteLine( "GuitarLight={0}", this.bLight.Guitar ? 1 : 0 );
			sw.WriteLine( "BassLight={0}", this.bLight.Bass ? 1 : 0 );
			sw.WriteLine();
            sw.WriteLine("; ギター/ベース演奏モード(0:Normal, 1:Specialist)");
            sw.WriteLine("GuitarSpecialist={0}", this.bSpecialist.Guitar ? 1 : 0);
            sw.WriteLine("BassSpecialist={0}", this.bSpecialist.Bass ? 1 : 0);
            sw.WriteLine();
            sw.WriteLine( "; ギター/ベースLEFTモード(0:OFF, 1:ON)" );
			sw.WriteLine( "GuitarLeft={0}", this.bLeft.Guitar ? 1 : 0 );
			sw.WriteLine( "BassLeft={0}", this.bLeft.Bass ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; RISKYモード(0:OFF, 1-10)" );									// #23559 2011.6.23 yyagi
			sw.WriteLine( "; RISKY mode. 0=OFF, 1-10 is the times of misses to be Failed." );	//
			sw.WriteLine( "Risky={0}", this.nRisky );			//
            sw.WriteLine();
            sw.WriteLine("; HAZARDモード(0:OFF, 1:ON)");									// #23559 2011.6.23 yyagi
            sw.WriteLine("; HAZARD mode. 0=OFF, 1=ON is the times of misses to be Failed.");	//
            sw.WriteLine("HAZARD={0}", this.bHAZARD ? 1 : 0);			//
            sw.WriteLine();
            sw.WriteLine("; TIGHTモード(0:OFF, 1:ON)");									// #29500 2012.9.11 kairera0467
            sw.WriteLine(": TIGHT mode. 0=OFF, 1=ON ");
            sw.WriteLine("DrumsTight={0}", this.bTight ? 1 : 0 );									//
            sw.WriteLine();
            sw.WriteLine("; Hidden/Suddenモード(0:OFF, 1:HIDDEN, 2:SUDDEN, 3:HID/SUD, 4:STEALTH)");
            sw.WriteLine("; Hidden/Sudden mode. 0=OFF, 1=HIDDEN, 2=SUDDEN, 3=HID/SUD, 4=STEALTH ");
            sw.WriteLine("DrumsHiddenSudden={0}", (int)this.nHidSud.Drums);
            sw.WriteLine("GuitarHiddenSudden={0}", (int)this.nHidSud.Guitar);
            sw.WriteLine("BassHiddenSudden={0}", (int)this.nHidSud.Bass);
            sw.WriteLine();
			sw.WriteLine( "; ドラム判定文字表示位置(0:OnTheLane,1:判定ライン上,2:表示OFF)" );
			sw.WriteLine( "DrumsPosition={0}", (int) this.JudgementStringPosition.Drums );
			sw.WriteLine();
			sw.WriteLine( "; ギター/ベース判定文字表示位置(0:OnTheLane, 1:レーン横, 2:判定ライン上, 3:表示OFF)" );
			sw.WriteLine( "GuitarPosition={0}", (int) this.JudgementStringPosition.Guitar );
			sw.WriteLine( "BassPosition={0}", (int) this.JudgementStringPosition.Bass );
			sw.WriteLine();
			sw.WriteLine( "; 譜面スクロール速度(0:x0.5, 1:x1.0, 2:x1.5,…,1999:x1000.0)" );
			sw.WriteLine( "DrumsScrollSpeed={0}", this.nScrollSpeed.Drums );
			sw.WriteLine( "GuitarScrollSpeed={0}", this.nScrollSpeed.Guitar );
			sw.WriteLine( "BassScrollSpeed={0}", this.nScrollSpeed.Bass );
			sw.WriteLine();
			sw.WriteLine( "; 演奏速度(5～40)(→x5/20～x40/20)" );
			sw.WriteLine( "PlaySpeed={0}", this.nPlaySpeed );
			sw.WriteLine();
			sw.WriteLine("; Save score when PlaySpeed is not 100% (0:OFF, 1:ON)");
			sw.WriteLine("SaveScoreIfModifiedPlaySpeed={0}", this.bSaveScoreIfModifiedPlaySpeed ? 1 : 0);
			sw.WriteLine();

			// #24074 2011.01.23 add ikanick
			sw.WriteLine( "; グラフ表示(0:OFF, 1:ON)" );
			sw.WriteLine( "DrumGraph={0}", this.bGraph有効.Drums ? 1 : 0 );
			sw.WriteLine( "GuitarGraph={0}", this.bGraph有効.Guitar ? 1 : 0 );
			sw.WriteLine( "BassGraph={0}", this.bGraph有効.Bass ? 1 : 0 );
			sw.WriteLine();

			sw.WriteLine("; Small Graph (0:OFF, 1:ON)");
			sw.WriteLine("SmallGraph={0}", this.bSmallGraph ? 1 : 0);
			sw.WriteLine();

			sw.WriteLine( "; ドラムコンボの表示(0:OFF, 1:ON)" );									// #29500 2012.9.11 kairera0467
            sw.WriteLine( ": DrumPart Display Combo. 0=OFF, 1=ON " );
            sw.WriteLine( "DrumComboDisp={0}", this.bドラムコンボ文字の表示 ? 1 : 0 );				//
            sw.WriteLine();

            //fork
            // #35411 2015.8.18 chnmr0 add
            sw.WriteLine("; AUTOゴースト種別 (0:PERFECT, 1:LAST_PLAY, 2:HI_SKILL, 3:HI_SCORE)" );
            sw.WriteLine("DrumAutoGhost={0}", (int)eAutoGhost.Drums);
            sw.WriteLine("GuitarAutoGhost={0}", (int)eAutoGhost.Guitar);
            sw.WriteLine("BassAutoGhost={0}", (int)eAutoGhost.Bass);
            sw.WriteLine();
            sw.WriteLine("; ターゲットゴースト種別 (0:NONE, 1:PERFECT, 2:LAST_PLAY, 3:HI_SKILL, 4:HI_SCORE)");
            sw.WriteLine("DrumTargetGhost={0}", (int)eTargetGhost.Drums);
            sw.WriteLine("GuitarTargetGhost={0}", (int)eTargetGhost.Guitar);
            sw.WriteLine("BassTargetGhost={0}", (int)eTargetGhost.Bass);
            sw.WriteLine();

            #region[DTXManiaXG追加オプション]
            sw.WriteLine("; 譜面仕様変更(0:デフォルト10レーン, 1:XG9レーン, 2:CLASSIC6レーン)");
            sw.WriteLine("NumOfLanes={0}", (int)this.eNumOfLanes.Drums);
            sw.WriteLine();
            sw.WriteLine("; dkdk仕様変更(0:デフォルト, 1:始動足変更, 2:dkdk1レーン化)");
            sw.WriteLine("DkdkType={0}", (int)this.eDkdkType.Drums);
            sw.WriteLine();
            sw.WriteLine("; バスをLBDに振り分け(0:OFF, 1:ON)");
            sw.WriteLine("AssignToLBD={0}", this.bAssignToLBD.Drums ? 1 : 0);
            sw.WriteLine();
            sw.WriteLine("; ドラムパッドRANDOMモード(0:OFF, 1:Mirror, 2:Random, 3:SuperRandom, 4:HyperRandom, 5:MasterRandom, 6:AnotherRandom)");
            sw.WriteLine("DrumsRandomPad={0}", (int)this.eRandom.Drums);
            sw.WriteLine();
            sw.WriteLine("; ドラム足RANDOMモード(0:OFF, 1:Mirror, 2:Random, 3:SuperRandom, 4:HyperRandom, 5:MasterRandom, 6:AnotherRandom)");
            sw.WriteLine("DrumsRandomPedal={0}", (int)this.eRandomPedal.Drums);
            sw.WriteLine();
            sw.WriteLine("; LP消音機能(0:OFF, 1:ON)");
            sw.WriteLine("MutingLP={0}", this.bMutingLP ? 1 : 0);
            sw.WriteLine();
            #endregion
            #region[ DTXHD追加オプション ]
            sw.WriteLine("; 判定ライン(0～100)" );
            sw.WriteLine("DrumsJudgeLine={0}", (int)this.nJudgeLine.Drums);
            sw.WriteLine("GuitarJudgeLine={0}", (int)this.nJudgeLine.Guitar);
            sw.WriteLine("BassJudgeLine={0}", (int)this.nJudgeLine.Bass);
            sw.WriteLine();
            #endregion
            #region[ ver.K追加オプション ]
            #region [ XGオプション ]
            sw.WriteLine("; ネームプレートタイプ");
            sw.WriteLine("; 0:タイプA XG2風の表示がされます。 ");
            sw.WriteLine("; 1:タイプB XG風の表示がされます。このタイプでは7_NamePlate_XG.png、7_Difficulty_XG.pngが読み込まれます。");
            sw.WriteLine("NamePlateType={0}", (int)this.eNamePlate);
            sw.WriteLine();
            sw.WriteLine("; 動くドラムセット(0:ON, 1:OFF, 2:NONE)");
            sw.WriteLine("DrumSetMoves={0}", (int)this.eドラムセットを動かす);
            sw.WriteLine();
            sw.WriteLine("; BPMバーの表示(0:表示する, 1:左のみ表示, 2:動くバーを表示しない, 3:表示しない)");
            sw.WriteLine("BPMBar={0}", (int)this.eBPMbar); ;
            sw.WriteLine();
            sw.WriteLine("; LivePointの表示(0:OFF, 1:ON)");
            sw.WriteLine("LivePoint={0}", this.bLivePoint ? 1 : 0);
            sw.WriteLine();
            sw.WriteLine("; スピーカーの表示(0:OFF, 1:ON)");
            sw.WriteLine("Speaker={0}", this.bSpeaker ? 1 : 0);
            sw.WriteLine();
            #endregion
            sw.WriteLine("; シャッターINSIDE(0～100)");
            sw.WriteLine("DrumsShutterIn={0}", (int)this.nShutterInSide.Drums);
            sw.WriteLine("GuitarShutterIn={0}", (int)this.nShutterInSide.Guitar);
            sw.WriteLine("BassShutterIn={0}", (int)this.nShutterInSide.Bass);
            sw.WriteLine();
            sw.WriteLine("; シャッターOUTSIDE(0～100)");
            sw.WriteLine("DrumsShutterOut={0}", (int)this.nShutterOutSide.Drums);
            sw.WriteLine("GuitarShutterOut={0}", (int)this.nShutterOutSide.Guitar);
            sw.WriteLine("BassShutterOut={0}", (int)this.nShutterOutSide.Bass);
            sw.WriteLine();
            sw.WriteLine( "; ボーナス演出の表示(0:表示しない, 1:表示する)");
            sw.WriteLine("DrumsStageEffect={0}", this.DisplayBonusEffects ? 1 : 0);
            sw.WriteLine();
            sw.WriteLine("; ドラムレーンタイプ(0:A, 1:B, 2:C 3:D )");
            sw.WriteLine("DrumsLaneType={0}", (int)this.eLaneType.Drums);
            sw.WriteLine();
            sw.WriteLine("; CLASSIC譜面判別");
            sw.WriteLine("CLASSIC={0}", this.bCLASSIC譜面判別を有効にする ? 1 : 0);
            sw.WriteLine();
            sw.WriteLine("; スキルモード(0:旧仕様, 1:XG仕様)");
            sw.WriteLine("SkillMode={0}", (int)this.nSkillMode);
            sw.WriteLine();
            sw.WriteLine("; スキルモードの自動切換え(0:OFF, 1:ON)");
            sw.WriteLine("SwitchSkillMode={0}", this.bSkillModeを自動切換えする ? 1 : 0);
            sw.WriteLine();
            sw.WriteLine("; ドラム アタックエフェクトタイプ");
            sw.WriteLine("; 0:ALL 粉と爆発エフェクトを表示します。");
            sw.WriteLine("; 1:ChipOFF チップのエフェクトを消します。");
            sw.WriteLine("; 2:EffectOnly 粉を消します。");
            sw.WriteLine("; 3:ALLOFF すべて消します。");
            sw.WriteLine("DrumsAttackEffect={0}", (int)this.eAttackEffect.Drums);
            sw.WriteLine();
            sw.WriteLine("; ギター / ベース アタックエフェクトタイプ (0:OFF, 1:ON)");
            sw.WriteLine("GuitarAttackEffect={0}", (int)this.eAttackEffect.Guitar);
            sw.WriteLine("BassAttackEffect={0}", (int)this.eAttackEffect.Bass);
            sw.WriteLine();
            sw.WriteLine("; レーン表示");
            sw.WriteLine("; 0:ALL ON レーン背景、小節線を表示します。");
            sw.WriteLine("; 1:LANE FF レーン背景を消します。");
            sw.WriteLine("; 2:LINE OFF 小節線を消します。");
            sw.WriteLine("; 3:ALL OFF すべて消します。");
            sw.WriteLine("DrumsLaneDisp={0}", (int)this.nLaneDisp.Drums);
            sw.WriteLine("GuitarLaneDisp={0}", (int)this.nLaneDisp.Guitar);
            sw.WriteLine("BassLaneDisp={0}", (int)this.nLaneDisp.Bass);
            sw.WriteLine();
            sw.WriteLine("; Display Judgement");
            sw.WriteLine("DrumsDisplayJudge={0}", this.bDisplayJudge.Drums ? 1 : 0);
            sw.WriteLine("GuitarDisplayJudge={0}", this.bDisplayJudge.Guitar ? 1 : 0);
            sw.WriteLine("BassDisplayJudge={0}", this.bDisplayJudge.Bass ? 1 : 0);
			sw.WriteLine();
			sw.WriteLine("; 判定ライン表示");
			sw.WriteLine("DrumsJudgeLineDisp={0}", this.bJudgeLineDisp.Drums ? 1 : 0);
			sw.WriteLine("GuitarJudgeLineDisp={0}", this.bJudgeLineDisp.Guitar ? 1 : 0);
			sw.WriteLine("BassJudgeLineDisp={0}", this.bJudgeLineDisp.Bass ? 1 : 0);
			sw.WriteLine();
            sw.WriteLine("; レーンフラッシュ表示");
            sw.WriteLine("DrumsLaneFlush={0}", this.bLaneFlush.Drums ? 1 : 0);
            sw.WriteLine("GuitarLaneFlush={0}", this.bLaneFlush.Guitar ? 1 : 0);
            sw.WriteLine("BassLaneFlush={0}", this.bLaneFlush.Bass ? 1 : 0);
            sw.WriteLine();
            sw.WriteLine("; ペダル部分のラグ時間調整");
            sw.WriteLine("; 入力が遅い場合、マイナス方向に調節してください。");
            sw.WriteLine("PedalLagTime={0}", this.nPedalLagTime );
            sw.WriteLine();
            #endregion

            //sw.WriteLine( ";-------------------" );
			#endregion
            #region[ 画像周り ]
            sw.WriteLine( ";判定画像のアニメーション方式" );
            sw.WriteLine( ";(0:旧DTXMania方式 1:コマ方式 2:擬似XG方式)");
            sw.WriteLine( "JudgeAnimeType={0}", this.nJudgeAnimeType );
            sw.WriteLine();
            sw.WriteLine( ";判定画像のコマ数" );
            sw.WriteLine( "JudgeFrames={0}", this.nJudgeFrames );
            sw.WriteLine();
            sw.WriteLine( ";判定画像の1コマのフレーム数" );
            sw.WriteLine( "JudgeInterval={0}", this.nJudgeInterval );
            sw.WriteLine();
            sw.WriteLine( ";判定画像の1コマの幅" );
            sw.WriteLine( "JudgeWidgh={0}", this.nJudgeWidgh );
            sw.WriteLine();
            sw.WriteLine( ";判定画像の1コマの高さ" );
            sw.WriteLine( "JudgeHeight={0}", this.nJudgeHeight );
            sw.WriteLine();
            sw.WriteLine( ";アタックエフェクトのコマ数" );
            sw.WriteLine( "ExplosionFrames={0}", (int)this.nExplosionFrames );
            sw.WriteLine();
            sw.WriteLine( ";アタックエフェクトの1コマのフレーム数" );
            sw.WriteLine( "ExplosionInterval={0}", (int)this.nExplosionInterval );
            sw.WriteLine();
            sw.WriteLine( ";アタックエフェクトの1コマの幅" );
            sw.WriteLine( "ExplosionWidgh={0}", this.nExplosionWidgh );
            sw.WriteLine();
            sw.WriteLine( ";アタックエフェクトの1コマの高さ" );
            sw.WriteLine( "ExplosionHeight={0}", this.nExplosionHeight );
            sw.WriteLine();
            sw.WriteLine( "ワイリングエフェクトのコマ数;" );
            sw.WriteLine( "WailingFireFrames={0}", (int)this.nWailingFireFrames );
            sw.WriteLine();
            sw.WriteLine( ";ワイリングエフェクトの1コマのフレーム数" );
            sw.WriteLine( "WailingFireInterval={0}", (int)this.nWailingFireInterval );
            sw.WriteLine();
            sw.WriteLine( ";ワイリングエフェクトの1コマの幅" );
            sw.WriteLine( "WailingFireWidgh={0}", this.nWailingFireWidgh );
            sw.WriteLine();
            sw.WriteLine( ";ワイリングエフェクトの1コマの高さ" );
            sw.WriteLine( "WailingFireHeight={0}", this.nWailingFireHeight );
            sw.WriteLine();
            sw.WriteLine( ";ワイリングエフェクトのX座標" );
            sw.WriteLine( "WailingFirePosXGuitar={0}", this.nWailingFireX.Guitar );
            sw.WriteLine( "WailingFirePosXBass={0}", this.nWailingFireX.Bass );
            sw.WriteLine();
            sw.WriteLine( ";ワイリングエフェクトのY座標(Guitar、Bass共通)" );
            sw.WriteLine( "WailingFirePosY={0}", this.nWailingFireY );
            sw.WriteLine();
            sw.WriteLine(";-------------------");
            #endregion
			#region [ AutoPlay ]
			sw.WriteLine( "[AutoPlay]" );
			sw.WriteLine();
			sw.WriteLine( "; 自動演奏(0:OFF, 1:ON)" );
			sw.WriteLine();
			sw.WriteLine( "; Drums" );
            sw.WriteLine("LC={0}", this.bAutoPlay.LC ? 1 : 0);
            sw.WriteLine("HH={0}", this.bAutoPlay.HH ? 1 : 0);
            sw.WriteLine("SD={0}", this.bAutoPlay.SD ? 1 : 0);
            sw.WriteLine("BD={0}", this.bAutoPlay.BD ? 1 : 0);
            sw.WriteLine("HT={0}", this.bAutoPlay.HT ? 1 : 0);
            sw.WriteLine("LT={0}", this.bAutoPlay.LT ? 1 : 0);
            sw.WriteLine("FT={0}", this.bAutoPlay.FT ? 1 : 0);
            sw.WriteLine("CY={0}", this.bAutoPlay.CY ? 1 : 0);
            sw.WriteLine("RD={0}", this.bAutoPlay.RD ? 1 : 0);
            sw.WriteLine("LP={0}", this.bAutoPlay.LP ? 1 : 0);
            sw.WriteLine("LBD={0}", this.bAutoPlay.LBD ? 1 : 0);
			sw.WriteLine();
			sw.WriteLine( "; Guitar" );
			//sw.WriteLine( "Guitar={0}", this.bAutoPlay.Guitar ? 1 : 0 );
			sw.WriteLine( "GuitarR={0}", this.bAutoPlay.GtR ? 1 : 0 );
			sw.WriteLine( "GuitarG={0}", this.bAutoPlay.GtG ? 1 : 0 );
			sw.WriteLine( "GuitarB={0}", this.bAutoPlay.GtB ? 1 : 0 );
            sw.WriteLine( "GuitarY={0}", this.bAutoPlay.GtY ? 1 : 0 );
            sw.WriteLine( "GuitarP={0}", this.bAutoPlay.GtP ? 1 : 0 );
			sw.WriteLine( "GuitarPick={0}", this.bAutoPlay.GtPick ? 1 : 0 );
			sw.WriteLine( "GuitarWailing={0}", this.bAutoPlay.GtW ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; Bass" );
			// sw.WriteLine( "Bass={0}", this.bAutoPlay.Bass ? 1 : 0 );
			sw.WriteLine( "BassR={0}", this.bAutoPlay.BsR ? 1 : 0 );
			sw.WriteLine( "BassG={0}", this.bAutoPlay.BsG ? 1 : 0 );
			sw.WriteLine( "BassB={0}", this.bAutoPlay.BsB ? 1 : 0 );
            sw.WriteLine( "BassY={0}", this.bAutoPlay.BsY ? 1 : 0);
            sw.WriteLine( "BassP={0}", this.bAutoPlay.BsP ? 1 : 0);
			sw.WriteLine( "BassPick={0}", this.bAutoPlay.BsPick ? 1 : 0 );
			sw.WriteLine( "BassWailing={0}", this.bAutoPlay.BsW ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( ";-------------------" );
			#endregion
            #region [ HitRange ]
			sw.WriteLine(@"[HitRange]");
			sw.WriteLine();
			sw.WriteLine(@"; Perfect～Poor とみなされる範囲[ms]");
			sw.WriteLine(@"; Hit ranges for each judgement type (in ± milliseconds)");
			sw.WriteLine();
			sw.WriteLine(@"; Drum chips, except pedals");
			tWriteHitRanges(stDrumHitRanges, @"Drum", sw);
			sw.WriteLine();
			sw.WriteLine(@"; Drum pedal chips");
			tWriteHitRanges(stDrumPedalHitRanges, @"DrumPedal", sw);
			sw.WriteLine();
			sw.WriteLine(@"; Guitar chips");
			tWriteHitRanges(stGuitarHitRanges, @"Guitar", sw);
			sw.WriteLine();
			sw.WriteLine(@"; Bass chips");
			tWriteHitRanges(stBassHitRanges, @"Bass", sw);
			sw.WriteLine();
			sw.WriteLine( ";-------------------" );
			#endregion

			#region [ Discord Rich Presence ]
			sw.WriteLine(@"[DiscordRichPresence]");
			sw.WriteLine();
			sw.WriteLine("; Enable Rich Presence integration (0:OFF, 1:ON)");
			sw.WriteLine($"Enable={(bDiscordRichPresenceEnabled ? 1 : 0)}");
			sw.WriteLine();
			sw.WriteLine("; Unique client identifier of the Discord Application to use");
			sw.WriteLine($"ApplicationID={strDiscordRichPresenceApplicationID}");
			sw.WriteLine();
			sw.WriteLine("; Unique identifier of the large image to display alongside presences");
			sw.WriteLine($"LargeImage={strDiscordRichPresenceLargeImageKey}");
			sw.WriteLine();
			sw.WriteLine("; Unique identifier of the small image to display alongside presences in drum mode");
			sw.WriteLine($"SmallImageDrums={strDiscordRichPresenceSmallImageKeyDrums}");
			sw.WriteLine();
			sw.WriteLine("; Unique identifier of the small image to display alongside presences in guitar mode");
			sw.WriteLine($"SmallImageGuitar={strDiscordRichPresenceSmallImageKeyGuitar}");
			sw.WriteLine();
			sw.WriteLine(@";-------------------");
			#endregion

			#region [ GUID ]
			sw.WriteLine( "[GUID]" );
			sw.WriteLine();
			foreach( KeyValuePair<int, string> pair in this.dicJoystick )
			{
				sw.WriteLine( "JoystickID={0},{1}", pair.Key, pair.Value );
			}
			#endregion
			#region [ DrumsKeyAssign ]
			sw.WriteLine();
			sw.WriteLine( ";-------------------" );
			sw.WriteLine( "; キーアサイン" );
			sw.WriteLine( ";   項　目：Keyboard → 'K'＋'0'＋キーコード(10進数)" );
			sw.WriteLine( ";           Mouse    → 'N'＋'0'＋ボタン番号(0～7)" );
			sw.WriteLine( ";           MIDI In  → 'M'＋デバイス番号1桁(0～9,A～Z)＋ノート番号(10進数)" );
			sw.WriteLine( ";           Joystick → 'J'＋デバイス番号1桁(0～9,A～Z)＋ 0 ...... Ｘ減少(左)ボタン" );
			sw.WriteLine( ";                                                         1 ...... Ｘ増加(右)ボタン" );
			sw.WriteLine( ";                                                         2 ...... Ｙ減少(上)ボタン" );
			sw.WriteLine( ";                                                         3 ...... Ｙ増加(下)ボタン" );
			sw.WriteLine( ";                                                         4 ...... Ｚ減少(前)ボタン" );
			sw.WriteLine( ";                                                         5 ...... Ｚ増加(後)ボタン" );
			sw.WriteLine( ";                                                         6～133.. ボタン1～128" );
			sw.WriteLine( ";           これらの項目を 16 個まで指定可能(',' で区切って記述）。" );
			sw.WriteLine( ";" );
			sw.WriteLine( ";   表記例：HH=K044,M042,J16" );
			sw.WriteLine( ";           → HiHat を Keyboard の 44 ('Z'), MidiIn#0 の 42, JoyPad#1 の 6(ボタン1) に割当て" );
			sw.WriteLine( ";" );
			sw.WriteLine( ";   ※Joystick のデバイス番号とデバイスとの関係は [GUID] セクションに記してあるものが有効。" );
			sw.WriteLine( ";" );
			sw.WriteLine();
			sw.WriteLine( "[DrumsKeyAssign]" );
			sw.WriteLine();
			sw.Write( "HH=" );
			this.tWriteKey( sw, this.KeyAssign.Drums.HH );
			sw.WriteLine();
			sw.Write( "SD=" );
			this.tWriteKey( sw, this.KeyAssign.Drums.SD );
			sw.WriteLine();
			sw.Write( "BD=" );
			this.tWriteKey( sw, this.KeyAssign.Drums.BD );
			sw.WriteLine();
			sw.Write( "HT=" );
			this.tWriteKey( sw, this.KeyAssign.Drums.HT );
			sw.WriteLine();
			sw.Write( "LT=" );
			this.tWriteKey( sw, this.KeyAssign.Drums.LT );
			sw.WriteLine();
			sw.Write( "FT=" );
			this.tWriteKey( sw, this.KeyAssign.Drums.FT );
			sw.WriteLine();
			sw.Write( "CY=" );
			this.tWriteKey( sw, this.KeyAssign.Drums.CY );
			sw.WriteLine();
			sw.Write( "HO=" );
			this.tWriteKey( sw, this.KeyAssign.Drums.HHO );
			sw.WriteLine();
			sw.Write( "RD=" );
			this.tWriteKey( sw, this.KeyAssign.Drums.RD );
			sw.WriteLine();
			sw.Write( "LC=" );
			this.tWriteKey( sw, this.KeyAssign.Drums.LC );
			sw.WriteLine();
			sw.Write( "LP=" );										// #27029 2012.1.4 from
			this.tWriteKey( sw, this.KeyAssign.Drums.LP );	//
			sw.WriteLine();											//
            sw.Write( "LBD=" );
            this.tWriteKey( sw, this.KeyAssign.Drums.LBD );
			sw.WriteLine();
            sw.WriteLine();
            #endregion
			#region [ GuitarKeyAssign ]
			sw.WriteLine( "[GuitarKeyAssign]" );
			sw.WriteLine();
			sw.Write( "R=" );
			this.tWriteKey( sw, this.KeyAssign.Guitar.R );
			sw.WriteLine();
			sw.Write( "G=" );
			this.tWriteKey( sw, this.KeyAssign.Guitar.G );
			sw.WriteLine();
			sw.Write( "B=" );
			this.tWriteKey( sw, this.KeyAssign.Guitar.B );
            sw.WriteLine();
            sw.Write( "Y=" );
            this.tWriteKey( sw, this.KeyAssign.Guitar.Y );
            sw.WriteLine();
            sw.Write( "P=" );
            this.tWriteKey( sw, this.KeyAssign.Guitar.P );
			sw.WriteLine();
			sw.Write( "Pick=" );
			this.tWriteKey( sw, this.KeyAssign.Guitar.Pick );
			sw.WriteLine();
			sw.Write( "Wail=" );
			this.tWriteKey( sw, this.KeyAssign.Guitar.Wail );
			sw.WriteLine();
			sw.Write( "Decide=" );
			this.tWriteKey( sw, this.KeyAssign.Guitar.Decide );
			sw.WriteLine();
			sw.Write("Cancel=");
			this.tWriteKey(sw, this.KeyAssign.Guitar.Cancel);
			sw.WriteLine();
			sw.WriteLine();
			#endregion
			#region [ BassKeyAssign ]
			sw.WriteLine( "[BassKeyAssign]" );
			sw.WriteLine();
			sw.Write( "R=" );
			this.tWriteKey( sw, this.KeyAssign.Bass.R );
			sw.WriteLine();
			sw.Write( "G=" );
			this.tWriteKey( sw, this.KeyAssign.Bass.G );
			sw.WriteLine();
			sw.Write( "B=" );
			this.tWriteKey( sw, this.KeyAssign.Bass.B );
            sw.WriteLine();
            sw.Write( "Y=" );
            this.tWriteKey( sw, this.KeyAssign.Bass.Y );
            sw.WriteLine();
            sw.Write( "P=" );
            this.tWriteKey( sw, this.KeyAssign.Bass.P );
			sw.WriteLine();
			sw.Write( "Pick=" );
			this.tWriteKey( sw, this.KeyAssign.Bass.Pick );
			sw.WriteLine();
			sw.Write( "Wail=" );
			this.tWriteKey( sw, this.KeyAssign.Bass.Wail );
			sw.WriteLine();
			sw.Write( "Decide=" );
			this.tWriteKey( sw, this.KeyAssign.Bass.Decide );
			sw.WriteLine();
			sw.Write("Cancel=");
			this.tWriteKey(sw, this.KeyAssign.Bass.Cancel);
			sw.WriteLine();
			sw.WriteLine();
			#endregion
			#region [ SystemkeyAssign ]
			sw.WriteLine( "[SystemKeyAssign]" );
			sw.WriteLine();
			sw.Write( "Capture=" );
			this.tWriteKey( sw, this.KeyAssign.System.Capture );
			sw.WriteLine();
			sw.Write("Search=");
			this.tWriteKey(sw, this.KeyAssign.System.Search);
			sw.WriteLine();
			sw.Write( "Help=" );
			this.tWriteKey( sw, this.KeyAssign.Guitar.Help );
			sw.WriteLine();
			sw.Write( "Pause=" );
			this.tWriteKey( sw, this.KeyAssign.Bass.Help );
			sw.WriteLine();
			sw.Write("LoopCreate=");
			this.tWriteKey(sw, this.KeyAssign.System.LoopCreate);
			sw.WriteLine();
			sw.Write("LoopDelete=");
			this.tWriteKey(sw, this.KeyAssign.System.LoopDelete);
			sw.WriteLine();
			sw.Write("SkipForward=");
			this.tWriteKey(sw, this.KeyAssign.System.SkipForward);
			sw.WriteLine();
			sw.Write("SkipBackward=");
			this.tWriteKey(sw, this.KeyAssign.System.SkipBackward);
			sw.WriteLine();
			sw.Write("IncreasePlaySpeed=");
			this.tWriteKey(sw, this.KeyAssign.System.IncreasePlaySpeed);
			sw.WriteLine();
			sw.Write("DecreasePlaySpeed=");
			this.tWriteKey(sw, this.KeyAssign.System.DecreasePlaySpeed);
			sw.WriteLine();
			sw.Write("Restart=");
			this.tWriteKey(sw, this.KeyAssign.System.Restart);
			sw.WriteLine();
			sw.WriteLine();
			#endregion
			
			sw.Close();
		}

		/// <summary>
		/// Write the given <see cref="STHitRanges"/> as INI fields to the given <see cref="StreamWriter"/>.
		/// </summary>
		/// <param name="stHitRanges">The <see cref="STHitRanges"/> to write.</param>
		/// <param name="strName">The unique identifier of <paramref name="stHitRanges"/>.</param>
		/// <param name="writer">The <see cref="StreamWriter"/> to write to.</param>
		private void tWriteHitRanges(STHitRanges stHitRanges, string strName, StreamWriter writer)
		{
			writer.WriteLine($@"{strName}Perfect={stHitRanges.nPerfectSizeMs}");
			writer.WriteLine($@"{strName}Great={stHitRanges.nGreatSizeMs}");
			writer.WriteLine($@"{strName}Good={stHitRanges.nGoodSizeMs}");
			writer.WriteLine($@"{strName}Poor={stHitRanges.nPoorSizeMs}");
		}

		public void tReadFromFile( string iniファイル名 )
		{
			this.ConfigIniファイル名 = iniファイル名;
			this.bConfigIniが存在している = File.Exists( this.ConfigIniファイル名 );
			if( this.bConfigIniが存在している )
			{
				string str;
				StreamReader reader = new StreamReader( this.ConfigIniファイル名, Encoding.GetEncoding( "Shift_JIS" ) );
				str = reader.ReadToEnd();
				tReadFromString( str );
				CDTXVersion version = new CDTXVersion( this.strDTXManiaのバージョン );
			}
		}

		private void tReadFromString( string strAllSettings )	// 2011.4.13 yyagi; refactored to make initial KeyConfig easier.
		{
			ESectionType unknown = ESectionType.Unknown;
			string[] delimiter = { "\n" };
			string[] strSingleLine = strAllSettings.Split( delimiter, StringSplitOptions.RemoveEmptyEntries );
			foreach ( string s in strSingleLine )
			{
				string str = s.Replace( '\t', ' ' ).TrimStart( new char[] { '\t', ' ' } );
				if ( ( str.Length != 0 ) && ( str[ 0 ] != ';' ) )
				{
					try
					{
						string str3;
						string str4;
						if ( str[ 0 ] == '[' )
						{
							#region [ セクションの変更 ]
							//-----------------------------
							StringBuilder builder = new StringBuilder( 32 );
							int num = 1;
							while ( ( num < str.Length ) && ( str[ num ] != ']' ) )
							{
								builder.Append( str[ num++ ] );
							}
							string str2 = builder.ToString();
							if ( str2.Equals( "System" ) )
							{
								unknown = ESectionType.System;
							}
							else if ( str2.Equals( "Log" ) )
							{
								unknown = ESectionType.Log;
							}
							else if ( str2.Equals( "PlayOption" ) )
							{
								unknown = ESectionType.PlayOption;
							}
							else if ( str2.Equals( "AutoPlay" ) )
							{
								unknown = ESectionType.AutoPlay;
							}
							else if ( str2.Equals( "HitRange" ) )
							{
								unknown = ESectionType.HitRange;
							}
							else if (str2.Equals(@"DiscordRichPresence"))
							{
								unknown = ESectionType.DiscordRichPresence;
							}
							else if ( str2.Equals( "GUID" ) )
							{
								unknown = ESectionType.GUID;
							}
							else if ( str2.Equals( "DrumsKeyAssign" ) )
							{
								unknown = ESectionType.DrumsKeyAssign;
							}
							else if ( str2.Equals( "GuitarKeyAssign" ) )
							{
								unknown = ESectionType.GuitarKeyAssign;
							}
							else if ( str2.Equals( "BassKeyAssign" ) )
							{
								unknown = ESectionType.BassKeyAssign;
							}
							else if ( str2.Equals( "SystemKeyAssign" ) )
							{
								unknown = ESectionType.SystemKeyAssign;
							}
							else
							{
								unknown = ESectionType.Unknown;
							}
							//-----------------------------
							#endregion
						}
						else
						{
							string[] strArray = str.Split( new char[] { '=' } );
							if( strArray.Length == 2 )
							{
								str3 = strArray[ 0 ].Trim();
								str4 = strArray[ 1 ].Trim();
								switch( unknown )
								{
									#region [ [System] ]
									//-----------------------------
									case ESectionType.System:
										{
#if false		// #23625 2011.1.11 Config.iniからダメージ/回復値の定数変更を行う場合はここを有効にする 087リリースに合わせ機能無効化
										//----------------------------------------
												if (str3.Equals("GaugeFactorD"))
												{
													int p = 0;
													string[] splittedFactor = str4.Split(',');
													foreach (string s in splittedFactor) {
														this.fGaugeFactor[p++, 0] = Convert.ToSingle(s);
													}
												} else
												if (str3.Equals("GaugeFactorG"))
												{
													int p = 0;
													string[] splittedFactor = str4.Split(',');
													foreach (string s in splittedFactor)
													{
														this.fGaugeFactor[p++, 1] = Convert.ToSingle(s);
													}
												}
												else
												if (str3.Equals("DamageFactor"))
												{
													int p = 0;
													string[] splittedFactor = str4.Split(',');
													foreach (string s in splittedFactor)
													{
														this.fDamageLevelFactor[p++] = Convert.ToSingle(s);
													}
												}
												else
										//----------------------------------------
#endif
											if( str3.Equals( "Version" ) )
											{
												this.strDTXManiaのバージョン = str4;
											}
											else if( str3.Equals( "DTXPath" ) )
											{
												this.str曲データ検索パス = str4;
											}
											else if ( str3.Equals( "SkinPath" ) )
											{
												string absSkinPath = str4;
												if ( !System.IO.Path.IsPathRooted( str4 ) )
												{
													absSkinPath = System.IO.Path.Combine( CDTXMania.strEXEのあるフォルダ, "System" );
													absSkinPath = System.IO.Path.Combine( absSkinPath, str4 );
													Uri u = new Uri( absSkinPath );
													absSkinPath = u.AbsolutePath.ToString();	// str4内に相対パスがある場合に備える
													absSkinPath = System.Web.HttpUtility.UrlDecode( absSkinPath );						// デコードする
													absSkinPath = absSkinPath.Replace( '/', System.IO.Path.DirectorySeparatorChar );	// 区切り文字が\ではなく/なので置換する
												}
												if ( absSkinPath[ absSkinPath.Length - 1 ] != System.IO.Path.DirectorySeparatorChar )	// フォルダ名末尾に\を必ずつけて、CSkin側と表記を統一する
												{
													absSkinPath += System.IO.Path.DirectorySeparatorChar;
												}
												this.strSystemSkinSubfolderFullName = absSkinPath;
											}
                                            else if( str3.Equals( "CardNameDrums" ) )
                                            {
                                                this.strCardName[0] = str4;
                                            }
                                            else if( str3.Equals( "CardNameGuitar" ) )
                                            {
                                                this.strCardName[1] = str4;
                                            }
                                            else if( str3.Equals( "CardNameBass" ) )
                                            {
                                                this.strCardName[2] = str4;
                                            }
                                            else if( str3.Equals( "GroupNameDrums" ) )
                                            {
                                                this.strGroupName[0] = str4;
                                            }
                                            else if( str3.Equals( "GroupNameGuitar" ) )
                                            {
                                                this.strGroupName[1] = str4;
                                            }
                                            else if( str3.Equals( "GroupNameBass" ) )
                                            {
                                                this.strGroupName[2] = str4;
                                            }
                                            else if( str3.Equals( "NameColorDrums" ) )
                                            {
                                                this.nNameColor[ 0 ] = CConversion.nGetNumberIfInRange(str4, 0, 19, 0);
                                            }
                                            else if( str3.Equals( "NameColorGuitar" ) )
                                            {
                                                this.nNameColor[ 1 ] = CConversion.nGetNumberIfInRange(str4, 0, 19, 0);
                                            }
                                            else if( str3.Equals( "NameColorBass" ) )
                                            {
                                                this.nNameColor[ 2 ] = CConversion.nGetNumberIfInRange(str4, 0, 19, 0);
                                            }
                                            else if (str3.Equals("SkinChangeByBoxDef"))
                                            {
                                                this.bUseBoxDefSkin = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("FullScreen"))
                                            {
                                                this.bFullScreenMode = CConversion.bONorOFF(str4[0]);
                                            }
											else if (str3.Equals("FullScreenExclusive"))
											{
												this.bFullScreenExclusive = CConversion.bONorOFF(str4[0]);
											}
											else if (str3.Equals("WindowWidth"))		// #23510 2010.10.31 yyagi add
                                            {
                                                this.nウインドウwidth = CConversion.nGetNumberIfInRange(str4, 1, 65535, this.nウインドウwidth);
                                                if (this.nウインドウwidth <= 0)
                                                {
                                                    this.nウインドウwidth = SampleFramework.GameWindowSize.Width;
                                                }
                                            }
                                            else if (str3.Equals("WindowHeight"))		// #23510 2010.10.31 yyagi add
                                            {
                                                this.nウインドウheight = CConversion.nGetNumberIfInRange(str4, 1, 65535, this.nウインドウheight);
                                                if (this.nウインドウheight <= 0)
                                                {
                                                    this.nウインドウheight = SampleFramework.GameWindowSize.Height;
                                                }
                                            }
                                            else if (str3.Equals("WindowX"))		// #30675 2013.02.04 ikanick add
                                            {
                                                this.n初期ウィンドウ開始位置X = CConversion.nGetNumberIfInRange(
                                                    str4, 0, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 1, this.n初期ウィンドウ開始位置X);
                                            }
                                            else if (str3.Equals("WindowY"))		// #30675 2013.02.04 ikanick add
                                            {
                                                this.n初期ウィンドウ開始位置Y = CConversion.nGetNumberIfInRange(
                                                    str4, 0, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 1, this.n初期ウィンドウ開始位置Y);
                                            }
                                            else if (str3.Equals("MovieMode"))
                                            {
                                                this.nMovieMode = CConversion.nGetNumberIfInRange(str4, 0, 0xffff, this.nMovieMode);
                                                if (this.nMovieMode > 3)
                                                {
                                                    this.nMovieMode = 0;
                                                }
                                            }
                                            else if (str3.Equals("MovieAlpha"))
                                            {
                                                this.nMovieAlpha = CConversion.nGetNumberIfInRange(str4, 0, 10, this.nMovieAlpha);
                                                if (this.nMovieAlpha > 10)
                                                {
                                                    this.nMovieAlpha = 10;
                                                }
                                            }
                                            else if (str3.Equals("InfoType"))
                                            {
                                                this.nInfoType = CConversion.nGetNumberIfInRange(str4, 0, 1, (int)this.nInfoType);
                                            }
                                            else if (str3.Equals("DoubleClickFullScreen"))	// #26752 2011.11.27 yyagi
                                            {
                                                this.bIsAllowedDoubleClickFullscreen = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("EnableSystemMenu"))		// #28200 2012.5.1 yyagi
                                            {
                                                this.bIsEnabledSystemMenu = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("SoundDeviceType"))
                                            {
                                                this.nSoundDeviceType = CConversion.nGetNumberIfInRange(str4, 0, 3, this.nSoundDeviceType);
                                            }
                                            else if (str3.Equals("WASAPIBufferSizeMs"))
                                            {
                                                this.nWASAPIBufferSizeMs = CConversion.nGetNumberIfInRange(str4, 0, 9999, this.nWASAPIBufferSizeMs);
                                            }
                                            else if (str3.Equals("ASIODevice"))
                                            {
                                                string[] asiodev = CEnumerateAllAsioDevices.GetAllASIODevices();
                                                this.nASIODevice = CConversion.nGetNumberIfInRange(str4, 0, asiodev.Length - 1, this.nASIODevice);
                                            }
											//else if (str3.Equals("ASIOBufferSizeMs"))
											//{
											//    this.nASIOBufferSizeMs = CConversion.nGetNumberIfInRange(str4, 0, 9999, this.nASIOBufferSizeMs);
											//}
											//else if (str3.Equals("DynamicBassMixerManagement"))
											//{
											//    this.bDynamicBassMixerManagement = CConversion.bONorOFF(str4[0]);
											//}
											else if (str3.Equals("SoundTimerType"))         // #33689 2014.6.6 yyagi
											{
												this.bUseOSTimer = CConversion.bONorOFF(str4[0]);
											}
											else if (str3.Equals("EventDrivenWASAPI"))
											{
												this.bEventDrivenWASAPI = CConversion.bONorOFF(str4[0]);
											}
											else if (str3.Equals("Metronome"))
											{
												this.bMetronome = CConversion.bONorOFF(str4[0]);
											}
                                            else if (str3.Equals("ChipPlayTimeComputeMode"))
                                            {
                                                this.nChipPlayTimeComputeMode = CConversion.nGetNumberIfInRange(str4, 0, 1, this.nChipPlayTimeComputeMode);
                                            }
                                            else if (str3.Equals("MasterVolume"))
											{
												this.nMasterVolume = CConversion.nGetNumberIfInRange(str4, 0, 100, this.nMasterVolume);
											}
											else if (str3.Equals("VSyncWait"))
                                            {
                                                this.bVerticalSyncWait = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("BackSleep"))				// #23568 2010.11.04 ikanick add
                                            {
                                                this.n非フォーカス時スリープms = CConversion.nRoundToRange(str4, 0, 50, this.n非フォーカス時スリープms);
                                            }
                                            else if (str3.Equals("SleepTimePerFrame"))		// #23568 2011.11.27 yyagi
                                            {
                                                this.nフレーム毎スリープms = CConversion.nRoundToRange(str4, -1, 50, this.nフレーム毎スリープms);
                                            }
                                            else if (str3.Equals("Guitar"))
                                            {
                                                this.bGuitarEnabled = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("Drums"))
                                            {
                                                this.bDrumsEnabled = CConversion.bONorOFF(str4[0]);
                                            }                                            
                                            else if (str3.Equals("BGAlpha"))
                                            {
                                                this.nBackgroundTransparency = CConversion.nGetNumberIfInRange(str4, 0, 0xff, this.nBackgroundTransparency);
                                            }
                                            else if (str3.Equals("DamageLevel"))
                                            {
                                                this.eDamageLevel = (EDamageLevel)CConversion.nGetNumberIfInRange(str4, 0, 2, (int)this.eDamageLevel);
                                            }
                                            else if (str3.Equals("HHGroup"))
                                            {
                                                this.eHHGroup = (EHHGroup)CConversion.nGetNumberIfInRange(str4, 0, 3, (int)this.eHHGroup);
                                            }
                                            else if (str3.Equals("FTGroup"))
                                            {
                                                this.eFTGroup = (EFTGroup)CConversion.nGetNumberIfInRange(str4, 0, 2, (int)this.eFTGroup);
                                            }
                                            else if (str3.Equals("CYGroup"))
                                            {
                                                this.eCYGroup = (ECYGroup)CConversion.nGetNumberIfInRange(str4, 0, 2, (int)this.eCYGroup);
                                            }
                                            else if (str3.Equals("BDGroup"))		// #27029 2012.1.4 from
                                            {
                                                this.eBDGroup = (EBDGroup)CConversion.nGetNumberIfInRange(str4, 0, 3, (int)this.eBDGroup);
                                            }
                                            else if (str3.Equals("HitSoundPriorityHH"))
                                            {
                                                this.eHitSoundPriorityHH = (EPlaybackPriority)CConversion.nGetNumberIfInRange(str4, 0, 1, (int)this.eHitSoundPriorityHH);
                                            }
                                            else if (str3.Equals("HitSoundPriorityFT"))
                                            {
                                                this.eHitSoundPriorityFT = (EPlaybackPriority)CConversion.nGetNumberIfInRange(str4, 0, 1, (int)this.eHitSoundPriorityFT);
                                            }
                                            else if (str3.Equals("HitSoundPriorityCY"))
                                            {
                                                this.eHitSoundPriorityCY = (EPlaybackPriority)CConversion.nGetNumberIfInRange(str4, 0, 1, (int)this.eHitSoundPriorityCY);
                                            }
                                            else if (str3.Equals("HitSoundPriorityLP"))
                                            {
                                                this.eHitSoundPriorityLP = (EPlaybackPriority)CConversion.nGetNumberIfInRange(str4, 0, 1, (int)this.eHitSoundPriorityLP);
                                            }
                                            else if (str3.Equals("StageFailed"))
                                            {
                                                this.bSTAGEFAILEDEnabled = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("AVI"))
                                            {
                                                this.bAVIEnabled = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("BGA"))
                                            {
                                                this.bBGAEnabled = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("FillInEffect"))
                                            {
                                                this.bFillInEnabled = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("PreviewSoundWait"))
                                            {
                                                this.n曲が選択されてからプレビュー音が鳴るまでのウェイトms = CConversion.nGetNumberIfInRange(str4, 0, 0x5f5e0ff, this.n曲が選択されてからプレビュー音が鳴るまでのウェイトms);
                                            }
                                            else if (str3.Equals("PreviewImageWait"))
                                            {
                                                this.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms = CConversion.nGetNumberIfInRange(str4, 0, 0x5f5e0ff, this.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms);
                                            }
                                            else if (str3.Equals("AdjustWaves"))
                                            {
                                                this.bWave再生位置自動調整機能有効 = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("BGMSound"))
                                            {
                                                this.bBGM音を発声する = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("HitSound"))
                                            {
                                                this.bドラム打音を発声する = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("AudienceSound"))
                                            {
                                                this.b歓声を発声する = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("SaveScoreIni"))
                                            {
                                                this.bScoreIniを出力する = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("RandomFromSubBox"))
                                            {
                                                this.bランダムセレクトで子BOXを検索対象とする = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("SoundMonitorDrums"))
                                            {
                                                this.b演奏音を強調する.Drums = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("SoundMonitorGuitar"))
                                            {
                                                this.b演奏音を強調する.Guitar = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("SoundMonitorBass"))
                                            {
                                                this.b演奏音を強調する.Bass = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("MinComboDrums"))
                                            {
                                                this.n表示可能な最小コンボ数.Drums = CConversion.nGetNumberIfInRange(str4, 1, 0x1869f, this.n表示可能な最小コンボ数.Drums);
                                            }
                                            else if (str3.Equals("MinComboGuitar"))
                                            {
                                                this.n表示可能な最小コンボ数.Guitar = CConversion.nGetNumberIfInRange(str4, 0, 0x1869f, this.n表示可能な最小コンボ数.Guitar);
                                            }
                                            else if (str3.Equals("MinComboBass"))
                                            {
                                                this.n表示可能な最小コンボ数.Bass = CConversion.nGetNumberIfInRange(str4, 0, 0x1869f, this.n表示可能な最小コンボ数.Bass);
                                            }
                                            else if( str3.Equals( "MusicNameDispDef" ) )
                                            {
                                                this.b曲名表示をdefのものにする = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("ShowDebugStatus"))
                                            {
                                                this.b演奏情報を表示する = CConversion.bONorOFF(str4[0]);
                                            }
                                            #region [ GDオプション ]
                                            else if (str3.Equals("Difficulty"))
                                            {
                                                this.b難易度表示をXG表示にする = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("ShowScore"))
                                            {
                                                this.bShowScore = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("ShowMusicInfo"))
                                            {
                                                this.bShowMusicInfo = CConversion.bONorOFF(str4[0]);
                                            }
											else if (str3.Equals("ShowPlaySpeed"))
											{
												this.nShowPlaySpeed = CConversion.nGetNumberIfInRange(str4, 0, 2, this.nShowPlaySpeed);
											}
											else if (str3.Equals("DisplayFontName"))
                                            {
                                                this.str曲名表示フォント = str4;
                                            }
                                            #endregion
                                            else if (str3.Equals("SelectListFontName"))
                                            {
                                                this.str選曲リストフォント = str4;
                                            }
                                            else if (str3.Equals("SelectListFontSize"))
                                            {
                                                this.n選曲リストフォントのサイズdot = CConversion.nGetNumberIfInRange(str4, 1, 0x3e7, this.n選曲リストフォントのサイズdot);
                                            }
                                            else if (str3.Equals("SelectListFontItalic"))
                                            {
                                                this.b選曲リストフォントを斜体にする = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("SelectListFontBold"))
                                            {
                                                this.b選曲リストフォントを太字にする = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("ChipVolume"))
                                            {
                                                this.n手動再生音量 = CConversion.nGetNumberIfInRange(str4, 0, 100, this.n手動再生音量);
                                            }
                                            else if (str3.Equals("AutoChipVolume"))
                                            {
                                                this.n自動再生音量 = CConversion.nGetNumberIfInRange(str4, 0, 100, this.n自動再生音量);
                                            }
                                            else if (str3.Equals("StoicMode"))
                                            {
                                                this.bストイックモード = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("CymbalFree"))
                                            {
                                                this.bシンバルフリー = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("HHOGraphics"))
                                            {
                                                this.eHHOGraphics.Drums = (EType)CConversion.nGetNumberIfInRange(str4, 0, 2, (int)this.eHHOGraphics.Drums);
                                            }
                                            else if (str3.Equals("LBDGraphics"))
                                            {
                                                this.eLBDGraphics.Drums = (EType)CConversion.nGetNumberIfInRange(str4, 0, 1, (int)this.eLBDGraphics.Drums);
                                            }
                                            else if (str3.Equals("RDPosition"))
                                            {
                                                this.eRDPosition = (ERDPosition)CConversion.nGetNumberIfInRange(str4, 0, 1, (int)this.eRDPosition);
                                            }
                                            else if (str3.Equals("ShowLagTime"))				// #25370 2011.6.3 yyagi
                                            {
                                                this.nShowLagType = CConversion.nGetNumberIfInRange(str4, 0, 2, this.nShowLagType);
                                            }
                                            else if (str3.Equals("ShowLagTimeColor"))				// #25370 2011.6.3 yyagi
                                            {
                                                this.nShowLagTypeColor = CConversion.nGetNumberIfInRange( str4, 0, 1, this.nShowLagTypeColor );
                                            }
											else if (str3.Equals("ShowLagHitCount"))          //fisyher: New field
											{
												this.bShowLagHitCount = CConversion.bONorOFF(str4[0]);
											}
											else if (str3.Equals("TimeStretch"))				// #23664 2013.2.24 yyagi
                                            {
                                                this.bTimeStretch = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("AutoResultCapture"))			// #25399 2011.6.9 yyagi
                                            {
                                                this.bIsAutoResultCapture = CConversion.bONorOFF(str4[0]);
                                            }
                                            #region [ AdjustTime ]
                                            else if ( str3.Equals( "InputAdjustTimeDrums" ) )		// #23580 2011.1.3 yyagi
                                            {
                                                this.nInputAdjustTimeMs.Drums = CConversion.nGetNumberIfInRange(str4, -99, 99, this.nInputAdjustTimeMs.Drums);
                                            }
                                            else if ( str3.Equals( "InputAdjustTimeGuitar" ) )	// #23580 2011.1.3 yyagi
                                            {
                                                this.nInputAdjustTimeMs.Guitar = CConversion.nGetNumberIfInRange(str4, -99, 99, this.nInputAdjustTimeMs.Guitar);
                                            }
                                            else if ( str3.Equals( "InputAdjustTimeBass" ) )		// #23580 2011.1.3 yyagi
                                            {
                                                this.nInputAdjustTimeMs.Bass = CConversion.nGetNumberIfInRange(str4, -99, 99, this.nInputAdjustTimeMs.Bass);
                                            }
                                            else if ( str3.Equals( "BGMAdjustTime" ) )              // #36372 2016.06.19 kairera0467
                                            {
                                                this.nCommonBGMAdjustMs = CConversion.nGetNumberIfInRange( str4, -99, 99, this.nCommonBGMAdjustMs );
                                            }
                                            else if ( str3.Equals( "JudgeLinePosOffsetDrums" ) ) // #31602 2013.6.23 yyagi
                                            {
                                                this.nJudgeLinePosOffset.Drums = CConversion.nGetNumberIfInRange( str4, -99, 99, this.nJudgeLinePosOffset.Drums );
                                            }
                                            else if ( str3.Equals( "JudgeLinePosOffsetGuitar" ) ) // #31602 2013.6.23 yyagi
                                            {
                                                this.nJudgeLinePosOffset.Guitar = CConversion.nGetNumberIfInRange( str4, -99, 99, this.nJudgeLinePosOffset.Guitar );
                                            }
                                            else if ( str3.Equals( "JudgeLinePosOffsetBass" ) ) // #31602 2013.6.23 yyagi
                                            {
                                                this.nJudgeLinePosOffset.Bass = CConversion.nGetNumberIfInRange( str4, -99, 99, this.nJudgeLinePosOffset.Bass );
                                            }
                                            #endregion
                                            else if (str3.Equals("BufferedInput"))
                                            {
                                                this.bバッファ入力を行う = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("PolyphonicSounds"))		// #28228 2012.5.1 yyagi
                                            {
                                                this.nPoliphonicSounds = CConversion.nGetNumberIfInRange(str4, 1, 8, this.nPoliphonicSounds);
                                            }
                                            else if (str3.Equals("LCVelocityMin"))			// #23857 2010.12.12 yyagi
                                            {
                                                this.nVelocityMin.LC = CConversion.nGetNumberIfInRange(str4, 0, 127, this.nVelocityMin.LC);
                                            }
                                            else if (str3.Equals("HHVelocityMin"))
                                            {
                                                this.nVelocityMin.HH = CConversion.nGetNumberIfInRange(str4, 0, 127, this.nVelocityMin.HH);
                                            }
                                            else if (str3.Equals("SDVelocityMin"))			// #23857 2011.1.31 yyagi
                                            {
                                                this.nVelocityMin.SD = CConversion.nGetNumberIfInRange(str4, 0, 127, this.nVelocityMin.SD);
                                            }
                                            else if (str3.Equals("BDVelocityMin"))			// #23857 2011.1.31 yyagi
                                            {
                                                this.nVelocityMin.BD = CConversion.nGetNumberIfInRange(str4, 0, 127, this.nVelocityMin.BD);
                                            }
                                            else if (str3.Equals("HTVelocityMin"))			// #23857 2011.1.31 yyagi
                                            {
                                                this.nVelocityMin.HT = CConversion.nGetNumberIfInRange(str4, 0, 127, this.nVelocityMin.HT);
                                            }
                                            else if (str3.Equals("LTVelocityMin"))			// #23857 2011.1.31 yyagi
                                            {
                                                this.nVelocityMin.LT = CConversion.nGetNumberIfInRange(str4, 0, 127, this.nVelocityMin.LT);
                                            }
                                            else if (str3.Equals("FTVelocityMin"))			// #23857 2011.1.31 yyagi
                                            {
                                                this.nVelocityMin.FT = CConversion.nGetNumberIfInRange(str4, 0, 127, this.nVelocityMin.FT);
                                            }
                                            else if (str3.Equals("CYVelocityMin"))			// #23857 2011.1.31 yyagi
                                            {
                                                this.nVelocityMin.CY = CConversion.nGetNumberIfInRange(str4, 0, 127, this.nVelocityMin.CY);
                                            }
                                            else if (str3.Equals("RDVelocityMin"))			// #23857 2011.1.31 yyagi
                                            {
                                                this.nVelocityMin.RD = CConversion.nGetNumberIfInRange(str4, 0, 127, this.nVelocityMin.RD);
                                            }
                                            else if (str3.Equals("LPVelocityMin"))			// #23857 2011.1.31 yyagi
                                            {
                                                this.nVelocityMin.LP = CConversion.nGetNumberIfInRange(str4, 0, 127, this.nVelocityMin.LP);
                                            }
                                            else if (str3.Equals("LBDVelocityMin"))			// #23857 2011.1.31 yyagi
                                            {
                                                this.nVelocityMin.LBD = CConversion.nGetNumberIfInRange(str4, 0, 127, this.nVelocityMin.LBD);
                                            }
                                            else if (str3.Equals("AutoAddGage"))
                                            {
                                                this.bAutoAddGage = CConversion.bONorOFF(str4[0]);
                                            }
											else if (str3.Equals("SkipTimeMs"))
											{
												this.nSkipTimeMs = CConversion.nGetNumberIfInRange(str4, 100, 20000, this.nSkipTimeMs);
											}
											continue;
										}
									//-----------------------------
									#endregion

									#region [ [Log] ]
									//-----------------------------
									case ESectionType.Log:
										{
											if( str3.Equals( "OutputLog" ) )
											{
												this.bOutputLogs = CConversion.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "TraceCreatedDisposed" ) )
											{
												this.bLog作成解放ログ出力 = CConversion.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "TraceDTXDetails" ) )
											{
												this.bLogDTX詳細ログ出力 = CConversion.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "TraceSongSearch" ) )
											{
												this.bLogSongSearch = CConversion.bONorOFF( str4[ 0 ] );
											}
											continue;
										}
									//-----------------------------
									#endregion

									#region [ [PlayOption] ]
									//-----------------------------
									case ESectionType.PlayOption:
										{
                                            if( str3.Equals( "DrumGraph" ) )  // #24074 2011.01.23 addikanick
											{
												this.bGraph有効.Drums = CConversion.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "GuitarGraph" ) )  // #24074 2011.01.23 addikanick
											{
												this.bGraph有効.Guitar = CConversion.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "BassGraph" ) )  // #24074 2011.01.23 addikanick
											{
												this.bGraph有効.Bass = CConversion.bONorOFF( str4[ 0 ] );
											}
											else if (str3.Equals("SmallGraph"))
											{
												this.bSmallGraph = CConversion.bONorOFF(str4[0]);
											}
											else if ( str3.Equals( "DrumsReverse" ) )
											{
												this.bReverse.Drums = CConversion.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "GuitarReverse" ) )
											{
												this.bReverse.Guitar = CConversion.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "BassReverse" ) )
											{
												this.bReverse.Bass = CConversion.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "GuitarRandom" ) )
											{
												this.eRandom.Guitar = (ERandomMode) CConversion.nGetNumberIfInRange( str4, 0, 4, (int) this.eRandom.Guitar );
											}
											else if( str3.Equals( "BassRandom" ) )
											{
												this.eRandom.Bass = (ERandomMode) CConversion.nGetNumberIfInRange( str4, 0, 4, (int) this.eRandom.Bass );
											}
											else if( str3.Equals( "DrumsTight" ) )
											{
												this.bTight = CConversion.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "GuitarLight" ) )
											{
												this.bLight.Guitar = CConversion.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "BassLight" ) )
											{
												this.bLight.Bass = CConversion.bONorOFF( str4[ 0 ] );
											}
                                            else if (str3.Equals("GuitarSpecialist"))
                                            {
                                                this.bSpecialist.Guitar = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("BassSpecialist"))
                                            {
                                                this.bSpecialist.Bass = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if( str3.Equals( "GuitarLeft" ) )
											{
												this.bLeft.Guitar = CConversion.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "BassLeft" ) )
											{
												this.bLeft.Bass = CConversion.bONorOFF( str4[ 0 ] );
											}
                                            else if (str3.Equals( "DrumsHiddenSudden") )
                                            {
                                                this.nHidSud.Drums = CConversion.nGetNumberIfInRange(str4, 0, 5, this.nHidSud.Drums);
                                            }
                                            else if (str3.Equals( "GuitarHiddenSudden") )
                                            {
                                                this.nHidSud.Guitar = CConversion.nGetNumberIfInRange(str4, 0, 5, this.nHidSud.Guitar);
                                            }
                                            else if (str3.Equals( "BassHiddenSudden") )
                                            {
                                                this.nHidSud.Bass = CConversion.nGetNumberIfInRange(str4, 0, 5, this.nHidSud.Bass);
                                            }
											else if( str3.Equals( "DrumsPosition" ) )
											{
                                                this.JudgementStringPosition.Drums = (EType)CConversion.nGetNumberIfInRange(str4, 0, 2, (int)this.JudgementStringPosition.Drums);
											}
											else if( str3.Equals( "GuitarPosition" ) )
											{
                                                this.JudgementStringPosition.Guitar = (EType)CConversion.nGetNumberIfInRange(str4, 0, 3, (int)this.JudgementStringPosition.Guitar);
											}
											else if( str3.Equals( "BassPosition" ) )
											{
                                                this.JudgementStringPosition.Bass = (EType)CConversion.nGetNumberIfInRange(str4, 0, 3, (int)this.JudgementStringPosition.Bass);
											}
											else if( str3.Equals( "DrumsScrollSpeed" ) )
											{
												this.nScrollSpeed.Drums = CConversion.nGetNumberIfInRange( str4, 0, 0x7cf, this.nScrollSpeed.Drums );
											}
											else if( str3.Equals( "GuitarScrollSpeed" ) )
											{
												this.nScrollSpeed.Guitar = CConversion.nGetNumberIfInRange( str4, 0, 0x7cf, this.nScrollSpeed.Guitar );
											}
											else if( str3.Equals( "BassScrollSpeed" ) )
											{
												this.nScrollSpeed.Bass = CConversion.nGetNumberIfInRange( str4, 0, 0x7cf, this.nScrollSpeed.Bass );
											}
											else if( str3.Equals( "PlaySpeed" ) )
											{
												this.nPlaySpeed = CConversion.nGetNumberIfInRange( str4, CConstants.PLAYSPEED_MIN, CConstants.PLAYSPEED_MAX, this.nPlaySpeed );
											}
											else if (str3.Equals("SaveScoreIfModifiedPlaySpeed"))
											{
												this.bSaveScoreIfModifiedPlaySpeed = CConversion.bONorOFF(str4[0]);
											}
											else if ( str3.Equals( "ComboPosition" ) )
											{
												this.ドラムコンボ文字の表示位置 = (EDrumComboTextDisplayPosition) CConversion.nGetNumberIfInRange( str4, 0, 3, (int) this.ドラムコンボ文字の表示位置 );
											}
											else if( str3.Equals( "Risky" ) )					// #2359 2011.6.23  yyagi
											{
												this.nRisky = CConversion.nGetNumberIfInRange( str4, 0, 10, this.nRisky );
											}
                                            else if( str3.Equals( "HAZARD" ) )				// #29500 2012.9.11 kairera0467
                                            {
                                                this.bHAZARD = CConversion.bONorOFF( str4[ 0 ] );
                                            }
                                            else if( str3.Equals( "AssignToLBD" ) )
                                            {
                                                this.bAssignToLBD.Drums = CConversion.bONorOFF( str4[ 0 ] );
                                            }
                                            else if (str3.Equals("DrumsJudgeLine"))
                                            {
                                                this.nJudgeLine.Drums = CConversion.nRoundToRange(str4, 0, 100, this.nJudgeLine.Drums);
                                            }
                                            else if ( str3.Equals( "DrumsShutterIn" ) )
                                            {
                                                this.nShutterInSide.Drums = CConversion.nGetNumberIfInRange( str4, 0, 100, this.nShutterInSide.Drums );
                                            }
                                            else if ( str3.Equals( "DrumsShutterOut" ) )
                                            {
                                                this.nShutterOutSide.Drums = CConversion.nGetNumberIfInRange( str4, -100, 100, this.nShutterOutSide.Drums );
                                            }
                                            else if ( str3.Equals( "GuitarJudgeLine" ) )
                                            {
                                                this.nJudgeLine.Guitar = CConversion.nRoundToRange(str4, 0, 100, this.nJudgeLine.Guitar);
                                            }
                                            else if ( str3.Equals( "GuitarShutterIn" ) )
                                            {
                                                this.nShutterInSide.Guitar = CConversion.nGetNumberIfInRange( str4, 0, 100, this.nShutterInSide.Guitar );
                                            }
                                            else if ( str3.Equals( "GuitarShutterOut" ) )
                                            {
                                                this.nShutterOutSide.Guitar = CConversion.nGetNumberIfInRange( str4, -100, 100, this.nShutterOutSide.Guitar );
                                            }
                                            else if ( str3.Equals( "BassJudgeLine" ) )
                                            {
                                                this.nJudgeLine.Bass = CConversion.nRoundToRange(str4, 0, 100, this.nJudgeLine.Bass);
                                            }
                                            else if ( str3.Equals( "BassShutterIn" ) )
                                            {
                                                this.nShutterInSide.Bass = CConversion.nGetNumberIfInRange( str4, 0, 100, this.nShutterInSide.Bass );
                                            }
                                            else if ( str3.Equals( "BassShutterOut" ) )
                                            {
                                                this.nShutterOutSide.Bass = CConversion.nGetNumberIfInRange( str4, -100, 100, this.nShutterOutSide.Guitar );
                                            }
                                            else if (str3.Equals("DrumsLaneType"))
                                            {
                                                this.eLaneType.Drums = (EType) CConversion.nGetNumberIfInRange(str4, 0, 3, (int) this.eLaneType.Drums);
                                            }
                                            else if (str3.Equals("RDPosition"))
                                            {
                                                this.eRDPosition = (ERDPosition)CConversion.nGetNumberIfInRange(str4, 0, 2, (int)this.eRDPosition);
                                            }
                                            else if (str3.Equals("DrumsTight"))				// #29500 2012.9.11 kairera0467
                                            {
                                                this.bTight = CConversion.bONorOFF(str4[0]);
                                            }
                                            #region [ XGオプション ]
                                            else if (str3.Equals("NamePlateType"))
                                            {
                                                this.eNamePlate = (EType)CConversion.nGetNumberIfInRange(str4, 0, 3, (int)this.eNamePlate);
                                            }
                                            else if (str3.Equals("DrumSetMoves"))
                                            {
                                                this.eドラムセットを動かす = (EType)CConversion.nGetNumberIfInRange(str4, 0, 2, (int)this.eドラムセットを動かす);
                                            }
                                            else if (str3.Equals("BPMBar"))
                                            {
                                                this.eBPMbar = ( EType )CConversion.nGetNumberIfInRange(str4, 0, 3, (int)this.eBPMbar);
                                            }
                                            else if (str3.Equals("LivePoint"))
                                            {
                                                this.bLivePoint = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("Speaker"))
                                            {
                                                this.bSpeaker = CConversion.bONorOFF(str4[0]);
                                            }
                                            #endregion
                                            else if (str3.Equals("DrumsStageEffect"))
                                            {
                                                this.DisplayBonusEffects = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("CLASSIC"))
                                            {
                                                this.bCLASSIC譜面判別を有効にする = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("MutingLP"))
                                            {
                                                this.bMutingLP = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("SkillMode"))
                                            {
                                                this.nSkillMode = CConversion.nGetNumberIfInRange(str4, 0, 1, (int)this.nSkillMode);
                                            }
                                            else if (str3.Equals("SwitchSkillMode"))
                                            {
                                                this.bSkillModeを自動切換えする = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("NumOfLanes"))
                                            {
                                                this.eNumOfLanes.Drums = (EType)CConversion.nGetNumberIfInRange(str4, 0, 2, (int)this.eNumOfLanes.Drums);
                                            }
                                            else if (str3.Equals("DkdkType"))
                                            {
                                                this.eDkdkType.Drums = (EType)CConversion.nGetNumberIfInRange(str4, 0, 2, (int)this.eDkdkType.Drums);
                                            }
                                            else if (str3.Equals("DrumsRandomPad"))
                                            {
                                                this.eRandom.Drums = (ERandomMode)CConversion.nGetNumberIfInRange(str4, 0, 6, (int)this.eRandom.Drums);
                                            }
                                            else if (str3.Equals("DrumsRandomPedal"))
                                            {
                                                this.eRandomPedal.Drums = (ERandomMode)CConversion.nGetNumberIfInRange(str4, 0, 6, (int)this.eRandomPedal.Drums);
                                            }
                                            else if (str3.Equals("DrumsAttackEffect"))
                                            {
                                                this.eAttackEffect.Drums = (EType)CConversion.nGetNumberIfInRange(str4, 0, 3, (int)this.eAttackEffect.Drums);
                                            }
                                            else if (str3.Equals("GuitarAttackEffect"))
                                            {
                                                this.eAttackEffect.Guitar = (EType)CConversion.nGetNumberIfInRange(str4, 0, 1, (int)this.eAttackEffect.Guitar);
                                            }
                                            else if (str3.Equals("BassAttackEffect"))
                                            {
                                                this.eAttackEffect.Bass = (EType)CConversion.nGetNumberIfInRange(str4, 0, 1, (int)this.eAttackEffect.Bass);
                                            }
                                            else if (str3.Equals("DrumsLaneDisp"))
                                            {
                                                this.nLaneDisp.Drums = CConversion.nGetNumberIfInRange(str4, 0, 4, (int)this.nLaneDisp.Drums);
                                            }
                                            else if (str3.Equals("GuitarLaneDisp"))
                                            {
                                                this.nLaneDisp.Guitar = CConversion.nGetNumberIfInRange(str4, 0, 4, (int)this.nLaneDisp.Guitar);
                                            }
                                            else if (str3.Equals("BassLaneDisp"))
                                            {
                                                this.nLaneDisp.Bass = CConversion.nGetNumberIfInRange(str4, 0, 4, (int)this.nLaneDisp.Bass);
                                            }
                                            else if (str3.Equals("DrumsDisplayJudge"))
                                            {
                                                this.bDisplayJudge.Drums = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if( str3.Equals("GuitarDisplayJudge") )
                                            {
                                                this.bDisplayJudge.Guitar = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if( str3.Equals("BassDisplayJudge") )
                                            {
                                                this.bDisplayJudge.Bass = CConversion.bONorOFF(str4[0]);
                                            }
											else if (str3.Equals("DrumsJudgeLineDisp"))
											{
												this.bJudgeLineDisp.Drums = CConversion.bONorOFF(str4[0]);
											}
											else if (str3.Equals("GuitarJudgeLineDisp"))
											{
												this.bJudgeLineDisp.Guitar = CConversion.bONorOFF(str4[0]);
											}
											else if (str3.Equals("BassJudgeLineDisp"))
											{
												this.bJudgeLineDisp.Bass = CConversion.bONorOFF(str4[0]);
											}
											else if (str3.Equals("DrumsLaneFlush"))
                                            {
                                                this.bLaneFlush.Drums = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("GuitarLaneFlush"))
                                            {
                                                this.bLaneFlush.Guitar = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if (str3.Equals("BassLaneFlush"))
                                            {
                                                this.bLaneFlush.Bass = CConversion.bONorOFF(str4[0]);
                                            }
                                            else if( str3.Equals( "JudgeAnimeType" ) )
                                            {
                                                this.nJudgeAnimeType = CConversion.nGetNumberIfInRange( str4, 0, 2, this.nJudgeAnimeType );
                                            }
                                            else if (str3.Equals( "JudgeFrames"))
                                            {
                                                this.nJudgeFrames = CConversion.nStringToInt( str4, this.nJudgeFrames );
                                            }
                                            else if ( str3.Equals( "JudgeInterval" ))
                                            {
                                                this.nJudgeInterval = CConversion.nStringToInt( str4, this.nJudgeInterval );
                                            }
                                            else if ( str3.Equals( "JudgeWidgh" ))
                                            {
                                                this.nJudgeWidgh = CConversion.nStringToInt( str4, this.nJudgeWidgh );
                                            }
                                            else if ( str3.Equals( "JudgeHeight" ))
                                            {
                                                this.nJudgeHeight = CConversion.nStringToInt( str4, this.nJudgeHeight );
                                            }
                                            else if ( str3.Equals( "ExplosionFrames" ))
                                            {
                                                this.nExplosionFrames = CConversion.nGetNumberIfInRange( str4, 0, int.MaxValue, (int)this.nExplosionFrames );
                                            }
                                            else if ( str3.Equals( "ExplosionInterval" ))
                                            {
                                                this.nExplosionInterval = CConversion.nGetNumberIfInRange( str4, 0, int.MaxValue, (int)this.nExplosionInterval );
                                            }
                                            else if ( str3.Equals( "ExplosionWidgh" ))
                                            {
                                                this.nExplosionWidgh = CConversion.nGetNumberIfInRange(str4, 0, int.MaxValue, (int)this.nExplosionWidgh);
                                            }
                                            else if ( str3.Equals( "ExplosionHeight" ))
                                            {
                                                this.nExplosionHeight = CConversion.nGetNumberIfInRange(str4, 0, int.MaxValue, (int)this.nExplosionHeight);
                                            }
                                            else if ( str3.Equals( "PedalLagTime" ) )
                                            {
                                                this.nPedalLagTime = CConversion.nGetNumberIfInRange( str4, -100, 100, this.nPedalLagTime );
                                            }
                                            else if ( str3.Equals( "WailingFireFrames" ))
                                            {
                                                this.nWailingFireFrames = CConversion.nGetNumberIfInRange( str4, 0, int.MaxValue, (int)this.nWailingFireFrames );
                                            }
                                            else if (str3.Equals("WailingFireInterval"))
                                            {
                                                this.nWailingFireInterval = CConversion.nGetNumberIfInRange( str4, 0, int.MaxValue, (int)this.nWailingFireInterval );
                                            }
                                            else if (str3.Equals("WailingFireWidgh"))
                                            {
                                                this.nWailingFireWidgh = CConversion.nGetNumberIfInRange(str4, 0, int.MaxValue, (int)this.nWailingFireWidgh);
                                            }
                                            else if (str3.Equals("WailingFireHeight"))
                                            {
                                                this.nWailingFireHeight = CConversion.nGetNumberIfInRange(str4, 0, int.MaxValue, (int)this.nWailingFireHeight);
                                            }
                                            else if (str3.Equals("WailingFirePosXGuitar"))
                                            {
                                                this.nWailingFireX.Guitar = CConversion.nStringToInt( str4, this.nWailingFireX.Guitar );
                                            }
                                            else if (str3.Equals("WailingFirePosXBass"))
                                            {
                                                this.nWailingFireX.Bass = CConversion.nStringToInt( str4, this.nWailingFireX.Bass );
                                            }
                                            else if (str3.Equals("WailingFirePosY"))
                                            {
                                                this.nWailingFireY = CConversion.nStringToInt( str4, this.nWailingFireY );
                                            }
                                            else if ( str3.Equals( "DrumComboDisp" ) )				// #29500 2012.9.11 kairera0467
                                            {
                                                this.bドラムコンボ文字の表示 = CConversion.bONorOFF(str4[0]);
                                            }

                                            //fork
                                            else if (str3.Equals("DrumAutoGhost")) // #35411 2015.08.18 chnmr0 add
                                            {
                                                this.eAutoGhost.Drums = (EAutoGhostData)CConversion.nGetNumberIfInRange(str4, 0, 3, 0);
                                            }
                                            else if (str3.Equals("GuitarAutoGhost")) // #35411 2015.08.18 chnmr0 add
                                            {
                                                this.eAutoGhost.Guitar = (EAutoGhostData)CConversion.nGetNumberIfInRange(str4, 0, 3, 0);
                                            }
                                            else if (str3.Equals("BassAutoGhost")) // #35411 2015.08.18 chnmr0 add
                                            {
                                                this.eAutoGhost.Bass = (EAutoGhostData)CConversion.nGetNumberIfInRange(str4, 0, 3, 0);
                                            }
                                            else if (str3.Equals("DrumTargetGhost")) // #35411 2015.08.18 chnmr0 add
                                            {
                                                this.eTargetGhost.Drums = (ETargetGhostData)CConversion.nGetNumberIfInRange(str4, 0, 4, 0);
                                            }
                                            else if (str3.Equals("GuitarTargetGhost")) // #35411 2015.08.18 chnmr0 add
                                            {
                                                this.eTargetGhost.Guitar = (ETargetGhostData)CConversion.nGetNumberIfInRange(str4, 0, 4, 0);
                                            }
                                            else if (str3.Equals("BassTargetGhost")) // #35411 2015.08.18 chnmr0 add
                                            {
                                                this.eTargetGhost.Bass = (ETargetGhostData)CConversion.nGetNumberIfInRange(str4, 0, 4, 0);
                                            }
											continue;
										}
									//-----------------------------
									#endregion

									#region [ [AutoPlay] ]
									//-----------------------------
									case ESectionType.AutoPlay:
										if( str3.Equals( "LC" ) )
										{
											this.bAutoPlay.LC = CConversion.bONorOFF( str4[ 0 ] );
										}
										if( str3.Equals( "HH" ) )
										{
										this.bAutoPlay.HH = CConversion.bONorOFF( str4[ 0 ] );
										}
										else if( str3.Equals( "SD" ) )
										{
											this.bAutoPlay.SD = CConversion.bONorOFF( str4[ 0 ] );
										}
										else if( str3.Equals( "BD" ) )
										{
											this.bAutoPlay.BD = CConversion.bONorOFF( str4[ 0 ] );
										}
										else if( str3.Equals( "HT" ) )
										{
											this.bAutoPlay.HT = CConversion.bONorOFF( str4[ 0 ] );
										}
										else if( str3.Equals( "LT" ) )
										{
											this.bAutoPlay.LT = CConversion.bONorOFF( str4[ 0 ] );
										}
										else if( str3.Equals( "FT" ) )
										{
										    this.bAutoPlay.FT = CConversion.bONorOFF( str4[ 0 ] );
										}
										else if( str3.Equals( "CY" ) )
										{
											this.bAutoPlay.CY = CConversion.bONorOFF( str4[ 0 ] );
                                        }
                                        else if (str3.Equals("RD"))
                                        {
                                            this.bAutoPlay.RD= CConversion.bONorOFF(str4[0]);
                                        }
                                        else if( str3.Equals( "LP" ) )
                                        {
                                            this.bAutoPlay.LP = CConversion.bONorOFF(str4[0]);
										}
                                        else if (str3.Equals("LBD"))
                                        {
                                            this.bAutoPlay.LBD = CConversion.bONorOFF(str4[0]);
                                        }
										//else if( str3.Equals( "Guitar" ) )
										//{
										//    this.bAutoPlay.Guitar = CConversion.bONorOFF( str4[ 0 ] );
										//}
										else if ( str3.Equals( "GuitarR" ) )
										{
											this.bAutoPlay.GtR = CConversion.bONorOFF( str4[ 0 ] );
										}
										else if ( str3.Equals( "GuitarG" ) )
										{
											this.bAutoPlay.GtG = CConversion.bONorOFF( str4[ 0 ] );
										}
										else if ( str3.Equals( "GuitarB" ) )
										{
											this.bAutoPlay.GtB = CConversion.bONorOFF( str4[ 0 ] );
										}
                                        else if ( str3.Equals( "GuitarY" ) )
                                        {
                                            this.bAutoPlay.GtY = CConversion.bONorOFF(str4[0]);
                                        }
                                        else if ( str3.Equals( "GuitarP" ) )
                                        {
                                            this.bAutoPlay.GtP = CConversion.bONorOFF(str4[0]);
                                        }
										else if ( str3.Equals( "GuitarPick" ) )
										{
											this.bAutoPlay.GtPick = CConversion.bONorOFF( str4[ 0 ] );
										}
										else if ( str3.Equals( "GuitarWailing" ) )
										{
											this.bAutoPlay.GtW = CConversion.bONorOFF( str4[ 0 ] );
										}
										//else if ( str3.Equals( "Bass" ) )
										//{
										//    this.bAutoPlay.Bass = CConversion.bONorOFF( str4[ 0 ] );
										//}
										else if ( str3.Equals( "BassR" ) )
										{
											this.bAutoPlay.BsR = CConversion.bONorOFF( str4[ 0 ] );
										}
										else if ( str3.Equals( "BassG" ) )
										{
											this.bAutoPlay.BsG = CConversion.bONorOFF( str4[ 0 ] );
										}
										else if ( str3.Equals( "BassB" ) )
										{
											this.bAutoPlay.BsB = CConversion.bONorOFF( str4[ 0 ] );
										}
                                        else if ( str3.Equals( "BassY" ) )
										{
											this.bAutoPlay.BsY = CConversion.bONorOFF( str4[ 0 ] );
										}
										else if ( str3.Equals( "BassP" ) )
										{
											this.bAutoPlay.BsP = CConversion.bONorOFF( str4[ 0 ] );
										}
										else if ( str3.Equals( "BassPick" ) )
										{
											this.bAutoPlay.BsPick = CConversion.bONorOFF( str4[ 0 ] );
										}
										else if ( str3.Equals( "BassWailing" ) )
										{
											this.bAutoPlay.BsW = CConversion.bONorOFF( str4[ 0 ] );
										}
										continue;
									//-----------------------------
									#endregion

                                    #region [ [HitRange] ]
                                    //-----------------------------
									case ESectionType.HitRange:
										// map the legacy hit ranges to apply to each category
										// they will only appear when the program is running from an unmigrated state,
										// so simply copy values over whenever there is a change
										STHitRanges stLegacyHitRanges = new STHitRanges(nDefaultSizeMs: -1);
										if (tTryReadHitRangesField(str3, str4, string.Empty, ref stLegacyHitRanges))
										{
											stDrumHitRanges = STHitRanges.tCompose(stLegacyHitRanges, stDrumHitRanges);
											stDrumPedalHitRanges = STHitRanges.tCompose(stLegacyHitRanges, stDrumPedalHitRanges);
											stGuitarHitRanges = STHitRanges.tCompose(stLegacyHitRanges, stGuitarHitRanges);
											stBassHitRanges = STHitRanges.tCompose(stLegacyHitRanges, stBassHitRanges);
											continue;
										}

										if (tTryReadHitRangesField(str3, str4, @"Drum", ref stDrumHitRanges))
											continue;

										if (tTryReadHitRangesField(str3, str4, @"DrumPedal", ref stDrumPedalHitRanges))
											continue;

										if (tTryReadHitRangesField(str3, str4, @"Guitar", ref stGuitarHitRanges))
											continue;

										if (tTryReadHitRangesField(str3, str4, @"Bass", ref stBassHitRanges))
											continue;

										continue;
									//-----------------------------
									#endregion

									#region [ [DiscordRichPresence] ]
									case ESectionType.DiscordRichPresence:
										switch (str3)
										{
											case @"Enable":
												bDiscordRichPresenceEnabled = CConversion.bONorOFF(str4[0]);
												break;
											case @"ApplicationID":
												strDiscordRichPresenceApplicationID = str4;
												break;
											case @"LargeImage":
												strDiscordRichPresenceLargeImageKey = str4;
												break;
											case @"SmallImageDrums":
												strDiscordRichPresenceSmallImageKeyDrums = str4;
												break;
											case @"SmallImageGuitar":
												strDiscordRichPresenceSmallImageKeyGuitar = str4;
												break;
										}
										continue;
									#endregion

									#region [ [GUID] ]
									//-----------------------------
									case ESectionType.GUID:
										if( str3.Equals( "JoystickID" ) )
										{
											this.tAcquireJoystickID( str4 );
										}
										continue;
									//-----------------------------
									#endregion

									#region [ [DrumsKeyAssign] ]
									//-----------------------------
									case ESectionType.DrumsKeyAssign:
										{
											if( str3.Equals( "HH" ) )
											{
												this.tReadAndSetSkey( str4, this.KeyAssign.Drums.HH );
											}
											else if( str3.Equals( "SD" ) )
											{
												this.tReadAndSetSkey( str4, this.KeyAssign.Drums.SD );
											}
											else if( str3.Equals( "BD" ) )
											{
												this.tReadAndSetSkey( str4, this.KeyAssign.Drums.BD );
											}
											else if( str3.Equals( "HT" ) )
											{
												this.tReadAndSetSkey( str4, this.KeyAssign.Drums.HT );
											}
											else if( str3.Equals( "LT" ) )
											{
												this.tReadAndSetSkey( str4, this.KeyAssign.Drums.LT );
											}
											else if( str3.Equals( "FT" ) )
											{
												this.tReadAndSetSkey( str4, this.KeyAssign.Drums.FT );
											}
											else if( str3.Equals( "CY" ) )
											{
												this.tReadAndSetSkey( str4, this.KeyAssign.Drums.CY );
											}
											else if( str3.Equals( "HO" ) )
											{
												this.tReadAndSetSkey( str4, this.KeyAssign.Drums.HHO );
											}
											else if( str3.Equals( "RD" ) )
											{
												this.tReadAndSetSkey( str4, this.KeyAssign.Drums.RD );
											}
											else if( str3.Equals( "LC" ) )
											{
												this.tReadAndSetSkey( str4, this.KeyAssign.Drums.LC );
											}
											else if( str3.Equals( "LP" ) )										// #27029 2012.1.4 from
											{																	//
												this.tReadAndSetSkey( str4, this.KeyAssign.Drums.LP );	//
											}																	//
                                            else if (str3.Equals( "LBD" ))										
                                            {																	
                                                this.tReadAndSetSkey( str4, this.KeyAssign.Drums.LBD );	
                                            }	
											continue;
										}
									//-----------------------------
									#endregion

									#region [ [GuitarKeyAssign] ]
									//-----------------------------
									case ESectionType.GuitarKeyAssign:
										{
											if( str3.Equals( "R" ) )
											{
												this.tReadAndSetSkey( str4, this.KeyAssign.Guitar.R );
											}
											else if( str3.Equals( "G" ) )
											{
												this.tReadAndSetSkey( str4, this.KeyAssign.Guitar.G );
											}
											else if( str3.Equals( "B" ) )
											{
												this.tReadAndSetSkey( str4, this.KeyAssign.Guitar.B );
											}
                                            else if( str3.Equals( "Y" ) )
											{
												this.tReadAndSetSkey( str4, this.KeyAssign.Guitar.Y );
											}
                                            else if( str3.Equals( "P" ) )
											{
												this.tReadAndSetSkey( str4, this.KeyAssign.Guitar.P );
											}
											else if( str3.Equals( "Pick" ) )
											{
												this.tReadAndSetSkey( str4, this.KeyAssign.Guitar.Pick );
											}
											else if( str3.Equals( "Wail" ) )
											{
												this.tReadAndSetSkey( str4, this.KeyAssign.Guitar.Wail );
											}
											else if( str3.Equals( "Decide" ) )
											{
												this.tReadAndSetSkey( str4, this.KeyAssign.Guitar.Decide );
											}
											else if (str3.Equals("Cancel"))
											{
												this.tReadAndSetSkey(str4, this.KeyAssign.Guitar.Cancel);
											}
											continue;
										}
									//-----------------------------
									#endregion

									#region [ [BassKeyAssign] ]
									//-----------------------------
									case ESectionType.BassKeyAssign:
										if( str3.Equals( "R" ) )
										{
											this.tReadAndSetSkey( str4, this.KeyAssign.Bass.R );
										}
										else if( str3.Equals( "G" ) )
										{
										    this.tReadAndSetSkey( str4, this.KeyAssign.Bass.G );
										}
										else if( str3.Equals( "B" ) )
										{
											this.tReadAndSetSkey( str4, this.KeyAssign.Bass.B );
										}
                                        else if( str3.Equals( "Y" ) )
                                        {
                                            this.tReadAndSetSkey( str4, this.KeyAssign.Bass.Y );
                                        }
                                        else if( str3.Equals( "P" ) ) 
                                        {
                                            this.tReadAndSetSkey( str4, this.KeyAssign.Bass.P );
                                        }
										else if( str3.Equals( "Pick" ) )
										{
											this.tReadAndSetSkey( str4, this.KeyAssign.Bass.Pick );
										}
										else if( str3.Equals( "Wail" ) )
										{
											this.tReadAndSetSkey( str4, this.KeyAssign.Bass.Wail );
										}
										else if( str3.Equals( "Decide" ) )
										{
											this.tReadAndSetSkey( str4, this.KeyAssign.Bass.Decide );
										}
										else if (str3.Equals("Cancel"))
										{
											this.tReadAndSetSkey(str4, this.KeyAssign.Bass.Cancel);
										}
										continue;
									//-----------------------------
									#endregion

									#region [ [SystemKeyAssign] ]
									//-----------------------------
									case ESectionType.SystemKeyAssign:
										if( str3.Equals( "Capture" ) )
										{
											this.tReadAndSetSkey( str4, this.KeyAssign.System.Capture );
										}
										else if (str3.Equals("Search"))
										{
											this.tReadAndSetSkey(str4, this.KeyAssign.System.Search);
										}
										else if (str3.Equals("Help"))
                                        {
                                            this.tReadAndSetSkey(str4, this.KeyAssign.Guitar.Help);
                                        }
                                        else if (str3.Equals("Pause"))
                                        {
                                            this.tReadAndSetSkey(str4, this.KeyAssign.Bass.Help);
                                        }
										else if (str3.Equals("LoopCreate"))
										{
											this.tReadAndSetSkey(str4, this.KeyAssign.System.LoopCreate);
										}
										else if (str3.Equals("LoopDelete"))
										{
											this.tReadAndSetSkey(str4, this.KeyAssign.System.LoopDelete);
										}
										else if (str3.Equals("SkipForward"))
										{
											this.tReadAndSetSkey(str4, this.KeyAssign.System.SkipForward);
										}
										else if (str3.Equals("SkipBackward"))
										{
											this.tReadAndSetSkey(str4, this.KeyAssign.System.SkipBackward);
										}
										else if (str3.Equals("IncreasePlaySpeed"))
										{
											this.tReadAndSetSkey(str4, this.KeyAssign.System.IncreasePlaySpeed);
										}
										else if (str3.Equals("DecreasePlaySpeed"))
										{
											this.tReadAndSetSkey(str4, this.KeyAssign.System.DecreasePlaySpeed);
										}
										else if (str3.Equals("Restart"))
										{
											this.tReadAndSetSkey(str4, this.KeyAssign.System.Restart);
										}
										continue;
									//-----------------------------
									#endregion

								}
							}
						}
						continue;
					}
					catch ( Exception exception )
					{
						Trace.TraceError( exception.Message );
						continue;
					}
				}
			}
		}

		/// <summary>
		/// Read the INI <see cref="STHitRanges"/> field, if any, described by the given parameters into the given <see cref="STHitRanges"/>.
		/// </summary>
		/// <param name="strFieldName">The name of the INI field being read from.</param>
		/// <param name="strFieldValue">The value of the INI field being read from.</param>
		/// <param name="strName">The unique identifier of <paramref name="stHitRanges"/>.</param>
		/// <param name="stHitRanges">The <see cref="STHitRanges"/> to read into.</param>
		/// <returns>Whether or not a field was read.</returns>
		private bool tTryReadHitRangesField(string strFieldName, string strFieldValue, string strName, ref STHitRanges stHitRanges)
		{
			const int nRangeMin = 0, nRangeMax = 0x3e7;
			switch (strFieldName)
			{
				// perfect range size (±ms)
				case var n when n == $@"{strName}Perfect":
					stHitRanges.nPerfectSizeMs = CConversion.nGetNumberIfInRange(strFieldValue, nRangeMin, nRangeMax, stHitRanges.nPerfectSizeMs);
					return true;

				// great range size (±ms)
				case var n when n == $@"{strName}Great":
					stHitRanges.nGreatSizeMs = CConversion.nGetNumberIfInRange(strFieldValue, nRangeMin, nRangeMax, stHitRanges.nGreatSizeMs);
					return true;

				// good range size (±ms)
				case var n when n == $@"{strName}Good":
					stHitRanges.nGoodSizeMs = CConversion.nGetNumberIfInRange(strFieldValue, nRangeMin, nRangeMax, stHitRanges.nGoodSizeMs);
					return true;

				// poor range size (±ms)
				case var n when n == $@"{strName}Poor":
					stHitRanges.nPoorSizeMs = CConversion.nGetNumberIfInRange(strFieldValue, nRangeMin, nRangeMax, stHitRanges.nPoorSizeMs);
					return true;

				// unknown field
				default:
					return false;
			}
		}

		/// <summary>
		/// ギターとベースのキーアサイン入れ替え
		/// </summary>
        /*
		public void SwapGuitarBassKeyAssign()		// #24063 2011.1.16 yyagi
		{
			for ( int j = 0; j <= (int)EKeyConfigPad.Capture; j++ )
			{
				CKeyAssign.STKEYASSIGN t; //= new CConfigIni.CKeyAssign.STKEYASSIGN();
				for ( int k = 0; k < 16; k++ )
				{
					t = this.KeyAssign[ (int)EKeyConfigPart.GUITAR ][ j ][ k ];
					this.KeyAssign[ (int)EKeyConfigPart.GUITAR ][ j ][ k ] = this.KeyAssign[ (int)EKeyConfigPart.BASS ][ j ][ k ];
					this.KeyAssign[ (int)EKeyConfigPart.BASS ][ j ][ k ] = t;
				}
			}
			this.bIsSwappedGuitarBass = !bIsSwappedGuitarBass;
		}
        */


		// Other

		#region [ private ]
		//-----------------
		private enum ESectionType
		{
			Unknown,
			System,
			Log,
			PlayOption,
			AutoPlay,
			HitRange,
			DiscordRichPresence,
			GUID,
			DrumsKeyAssign,
			GuitarKeyAssign,
			BassKeyAssign,
			SystemKeyAssign,
			Temp,
		}

		private bool _bDrums有効;
		private bool _bGuitar有効;
		private bool bConfigIniが存在している;
		private string ConfigIniファイル名;

		private void tAcquireJoystickID( string strキー記述 )
		{
			string[] strArray = strキー記述.Split( new char[] { ',' } );
			if( strArray.Length >= 2 )
			{
				int result = 0;
				if( ( int.TryParse( strArray[ 0 ], out result ) && ( result >= 0 ) ) && ( result <= 9 ) )
				{
					if( this.dicJoystick.ContainsKey( result ) )
					{
						this.dicJoystick.Remove( result );
					}
					this.dicJoystick.Add( result, strArray[ 1 ] );
				}
			}
		}
		private void tClearAllKeyAssignments()
		{
			this.KeyAssign = new CKeyAssign();
			for( int i = 0; i <= (int)EKeyConfigPart.SYSTEM; i++ )
			{
				for( int j = 0; j < (int)EKeyConfigPad.MAX; j++ )
				{
					this.KeyAssign[ i ][ j ] = new CKeyAssign.STKEYASSIGN[ 16 ];
					for( int k = 0; k < 16; k++ )
					{
						this.KeyAssign[ i ][ j ][ k ] = new CKeyAssign.STKEYASSIGN( EInputDevice.Unknown, 0, 0 );
					}
				}
			}
		}
		private void tWriteKey( StreamWriter sw, CKeyAssign.STKEYASSIGN[] assign )
		{
			bool flag = true;
			for( int i = 0; i < 0x10; i++ )
			{
				if( assign[ i ].InputDevice == EInputDevice.Unknown )
				{
					continue;
				}
				if( !flag )
				{
					sw.Write( ',' );
				}
				flag = false;
				switch( assign[ i ].InputDevice )
				{
					case EInputDevice.Keyboard:
						sw.Write( 'K' );
						break;

					case EInputDevice.MIDI入力:
						sw.Write( 'M' );
						break;

					case EInputDevice.Joypad:
						sw.Write( 'J' );
						break;

					case EInputDevice.Mouse:
						sw.Write( 'N' );
						break;
				}
				sw.Write( "{0}{1}", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring( assign[ i ].ID, 1 ), assign[ i ].Code );	// #24166 2011.1.15 yyagi: to support ID > 10, change 2nd character from Decimal to 36-numeral system. (e.g. J1023 -> JA23)
			}
		}
        private void tReadAndSetSkey(string strキー記述, CKeyAssign.STKEYASSIGN[] assign)
        {
            string[] strArray = strキー記述.Split(new char[] { ',' });
            for (int i = 0; (i < strArray.Length) && (i < 16); i++)
            {
                EInputDevice eInputDevice;
                int id;
                int code;
                string str = strArray[i].Trim().ToUpper();
                if (str.Length >= 3)
                {
                    eInputDevice = EInputDevice.Unknown;
                    switch (str[0])
                    {
                        case 'J':
                            eInputDevice = EInputDevice.Joypad;
                            break;

                        case 'K':
                            eInputDevice = EInputDevice.Keyboard;
                            break;

                        case 'L':
                            continue;

                        case 'M':
                            eInputDevice = EInputDevice.MIDI入力;
                            break;

                        case 'N':
                            eInputDevice = EInputDevice.Mouse;
                            break;
                    }
                }
                else
                {
                    continue;
                }
                id = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(str[1]);	// #24166 2011.1.15 yyagi: to support ID > 10, change 2nd character from Decimal to 36-numeral system. (e.g. J1023 -> JA23)
                if (((id >= 0) && int.TryParse(str.Substring(2), out code)) && ((code >= 0) && (code <= 0xff)))
                {
                    this.tDeleteAlreadyAssignedInputs(eInputDevice, id, code);
                    assign[i].InputDevice = eInputDevice;
                    assign[i].ID = id;
                    assign[i].Code = code;
                }
            }
        }
		private void tSetDefaultKeyAssignments()
		{
			this.tClearAllKeyAssignments();

			string strDefaultKeyAssign = @"
[DrumsKeyAssign]

HH=K033
SD=K012,K013
BD=K0126,K048
HT=K031,K015
LT=K011,K016
FT=K023,K017
CY=K022,K019
HO=K028
RD=K047,K020
LC=K035,K010
LP=K087
LBD=K077

[GuitarKeyAssign]

R=K054
G=K055,J012
B=K056
Y=K057
P=K058
Pick=K0115,K046,J06
Wail=K0116
Decide=K060
Cancel=K0115

[BassKeyAssign]

R=K090
G=K091,J013
B=K092
Y=K093
P=K094
Pick=K0103,K0100,J08
Wail=K089
Decide=K096
Cancel=K0103

[SystemKeyAssign]
Capture=K065
Search=K042
Help=K064
Pause=K0110
LoopCreate=
LoopDelete=
SkipForward=
SkipBackward=
IncreasePlaySpeed=
DecreasePlaySpeed=
Restart=K052
";
			tReadFromString( strDefaultKeyAssign );
		}
		//-----------------
		#endregion
	}
}
