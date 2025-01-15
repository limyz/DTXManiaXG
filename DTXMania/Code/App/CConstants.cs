using System;
using System.Runtime.InteropServices;

namespace DTXMania
{
	public enum ECYGroup
	{
		打ち分ける,
		共通
	}
	public enum EFTGroup
	{
		打ち分ける,
		共通
	}
	public enum EHHGroup
	{
		全部打ち分ける,
		ハイハットのみ打ち分ける,
		左シンバルのみ打ち分ける,
		全部共通
	}
	public enum EBDGroup		// #27029 2012.1.4 from add
	{
		打ち分ける,
        BDとLPで打ち分ける,
		左右ペダルのみ打ち分ける,
        どっちもBD
	}
    public enum EType
    {
        A,
        B,
        C,
        D,
        E
    }
    public enum ERDPosition
    {
        RDRC,
        RCRD
    }
	public enum EDarkMode
	{
		OFF,
		HALF,
		FULL
	}
	public enum EDamageLevel
	{
		Small	= 0,
		Normal	= 1,
		High	= 2
	}
	public enum EPad			// 演奏用のenum。ここを修正するときは、次に出てくる EKeyConfigPad と EPadFlag もセットで修正すること。
	{
		HH		= 0,
		R		= 0,
		SD		= 1,
		G		= 1,
		BD		= 2,
		B		= 2,
		HT		= 3,
		Pick	= 3,
		LT		= 4,
		Wail	= 4,
		FT		= 5,
		Help	= 5,
		CY		= 6,
		Decide	= 6,
		HHO		= 7,
        Y       = 7,
		RD		= 8,
		LC		= 9,
        P       = 9,
		//HP		= 10,	// #27029 2012.1.4 from
		LP      = 10,
        LBD     = 11,
		Cancel  = 12,   // fisyher: New Cancel/Go Back Key
        MAX,			// 門番用として定義
		UNKNOWN = 99
	}
	public enum EKeyConfigPad		// #24609 キーコンフィグで使うenum。capture要素あり。
	{
		HH		= EPad.HH,
		R		= EPad.R,
		SD		= EPad.SD,
		G		= EPad.G,
		BD		= EPad.BD,
		B		= EPad.B,
		HT		= EPad.HT,
		Pick	= EPad.Pick,
		LT		= EPad.LT,
		Wail	= EPad.Wail,
		FT		= EPad.FT,
		Help	= EPad.Help,
		CY		= EPad.CY,
		Decide	= EPad.Decide,
		HHO		= EPad.HHO,
        Y       = EPad.Y,
		RD		= EPad.RD,
        P       = EPad.P,
		LC		= EPad.LC,
		//HP		= EPad.HP,		// #27029 2012.1.4 from
        LP      = EPad.LP,
        LBD     = EPad.LBD,
		Cancel  = EPad.Cancel,     // fisyher: New Cancel/Go Back Key
		//Non-Pad System buttons
		Capture,
		Search,
		LoopCreate,
		LoopDelete,
		SkipForward,
		SkipBackward, // = Rewind
		IncreasePlaySpeed,
		DecreasePlaySpeed,
		Restart,
		MAX,          // Gatekeeper
		UNKNOWN = EPad.UNKNOWN
	}
	[Flags]
	public enum EPadFlag		// #24063 2011.1.16 yyagi コマンド入力用 パッド入力のフラグ化
	{
		None	= 0,
		HH		= 1,
		R		= 1,
		SD		= 2,
		G		= 2,
		B		= 4,
		BD		= 4,
		HT		= 8,
		Pick	= 8,
		LT		= 16,
		Wail	= 16,
		FT		= 32,
		Help	= 32,
		CY		= 64,
		Decide	= 128,
		HHO		= 128,
		RD		= 256,
        Y       = 256,
		LC		= 512,
        P       = 512,
        LP      = 1024,
        LBD     = 2048,
		Cancel  = 4096,
		UNKNOWN = 8192
	}
	public enum ERandomMode
	{
		OFF,
        MIRROR,
		RANDOM,
		SUPERRANDOM,
		HYPERRANDOM,
        MASTERRANDOM,
        ANOTHERRANDOM
	}
	public enum EInstrumentPart		// ここを修正するときは、セットで次の EKeyConfigPart も修正すること。
	{
		DRUMS	= 0,
		GUITAR	= 1,
		BASS	= 2,
		//SYSTEM  = 3,
		UNKNOWN	= 99
	}
	public enum EKeyConfigPart	// : EInstrumentPart
	{
		DRUMS	= EInstrumentPart.DRUMS,
		GUITAR	= EInstrumentPart.GUITAR,
		BASS	= EInstrumentPart.BASS,
		SYSTEM,
		UNKNOWN	= EInstrumentPart.UNKNOWN
	}

	public enum EPlaybackPriority
	{
		ChipOverPadPriority,
		PadOverChipPriority
	}
	internal enum EInputDevice
	{
		Keyboard	= 0,
		MIDI入力		= 1,
		Joypad		= 2,
		Mouse		= 3,
		Unknown		= -1
	}
	public enum EJudgement
	{
		Perfect	= 0,
		Great	= 1,
		Good	= 2,
		Poor	= 3,
		Miss	= 4,
		Bad		= 5,
		Auto
	}



	internal enum E判定文字表示位置
	{
		OnTheLane,
		判定ライン上または横,
		表示OFF
	}
	internal enum EAVIType
	{
		Unknown,
		AVI,
		AVIPAN
	}
	internal enum EBGAType
	{
		Unknown,
		BMP,
		BMPTEX,
		BGA,
		BGAPAN
	}
	internal enum EFIFOMode
	{
		FadeIn,
		FadeOut
	}
	internal enum EDrumComboTextDisplayPosition
	{
		LEFT,
		CENTER,
		RIGHT,
		OFF
	}
	internal enum ELane
	{
		LC = 0,
		HH,
		SD,
		BD,
		HT,
		LT,
		FT,
		CY,
        LP,
		RD,		// 将来の独立レーン化/独立AUTO設定を見越して追加
        LBD,
		Guitar,	// AUTOレーン判定を容易にするため、便宜上定義しておく(未使用)
		Bass,	// (未使用)
		GtR,
		GtG,
		GtB,
        GtY,
        GtP,
		GtPick,
		GtW,
		BsR,
		BsG,
		BsB,
        BsY,
        BsP,
		BsPick,
		BsW,
		MAX,	// 要素数取得のための定義 ("BGM"は使わない前提で)
		BGM
	}
	internal enum EOutputLog
	{
		OFF,
		ONNormal,
		ONVerbose
	}
	internal enum EPerfScreenReturnValue
	{
		Continue,
		Interruption,
		Restart,
		StageFailure,
		StageClear
	}
    internal enum ESongLoadingScreenReturnValue
    {
        Continue = 0,
        LoadingComplete,
        LoadingStopped
    }
	/// <summary>
	/// 入力ラグ表示タイプ
	/// </summary>
	internal enum EShowLagType
	{
		OFF,			// 全く表示しない
		ON,				// 判定に依らず全て表示する
		GREAT_POOR		// GREAT-MISSの時のみ表示する(PERFECT時は表示しない)
	}

	internal enum EShowPlaySpeed
    {
		OFF,
		ON,
		IF_CHANGED_IN_GAME
    }

    /// <summary>
    /// 使用するAUTOゴーストデータの種類 (#35411 chnmr0)
    /// </summary>
    public enum EAutoGhostData
    {
        PERFECT = 0, // 従来のAUTO
        LAST_PLAY = 1, // (.score.ini) の LastPlay ゴースト
        HI_SKILL = 2, // (.score.ini) の HiSkill ゴースト
        HI_SCORE = 3, // (.score.ini) の HiScore ゴースト
        ONLINE = 4 // オンラインゴースト (DTXMOS からプラグインが取得、本体のみでは指定しても無効)
    }

    /// <summary>
    /// 使用するターゲットゴーストデータの種類 (#35411 chnmr0)
    /// ここでNONE以外を指定してかつ、ゴーストが利用可能な場合グラフの目標値に描画される
    /// NONE の場合従来の動作
    /// </summary>
    public enum ETargetGhostData
    {
        NONE = 0,
        PERFECT = 1,
        LAST_PLAY = 2, // (.score.ini) の LastPlay ゴースト
        HI_SKILL = 3, // (.score.ini) の HiSkill ゴースト
        HI_SCORE = 4, // (.score.ini) の HiScore ゴースト
        ONLINE = 5 // オンラインゴースト (DTXMOS からプラグインが取得、本体のみでは指定しても無効)
    }
	/// <summary>
	/// Drum/Guitar/Bass の値を扱う汎用の構造体。
	/// </summary>
	/// <typeparam name="T">値の型。</typeparam>
	[Serializable]
	[StructLayout( LayoutKind.Sequential )]
	public struct STDGBVALUE<T>			// indexはE楽器パートと一致させること
	{
		public T Drums;
		public T Guitar;
		public T Bass;
		public T Unknown;
		public T this[ int index ]
		{
			get
			{
				switch( index )
				{
					case (int) EInstrumentPart.DRUMS:
						return this.Drums;

					case (int) EInstrumentPart.GUITAR:
						return this.Guitar;

					case (int) EInstrumentPart.BASS:
						return this.Bass;

					case (int) EInstrumentPart.UNKNOWN:
						return this.Unknown;
				}
				throw new IndexOutOfRangeException();
			}
			set
			{
				switch( index )
				{
					case (int) EInstrumentPart.DRUMS:
						this.Drums = value;
						return;

					case (int) EInstrumentPart.GUITAR:
						this.Guitar = value;
						return;

					case (int) EInstrumentPart.BASS:
						this.Bass = value;
						return;

					case (int) EInstrumentPart.UNKNOWN:
						this.Unknown = value;
						return;
				}
				throw new IndexOutOfRangeException();
			}
		}
	}

	/// <summary>
	/// レーンの値を扱う汎用の構造体。列挙型"Eドラムレーン"に準拠。
	/// </summary>
	/// <typeparam name="T">値の型。</typeparam>
	[StructLayout( LayoutKind.Sequential )]
	public struct STLANEVALUE<T>
	{
		public T LC;
		public T HH;
		public T SD;
		public T BD;
		public T HT;
		public T LT;
		public T FT;
		public T CY;
        public T LP;
		public T RD;
        public T LBD;
		public T Guitar;
		public T Bass;
		public T GtR;
		public T GtG;
		public T GtB;
        public T GtY;
        public T GtP;
		public T GtPick;
		public T GtW;
		public T BsR;
		public T BsG;
		public T BsB;
        public T BsY;
        public T BsP;
		public T BsPick;
		public T BsW;
		public T BGM;

		public T this[ int index ]
		{
			get
			{
				switch ( index )
				{
					case (int) ELane.LC:
						return this.LC;
					case (int) ELane.HH:
						return this.HH;
					case (int) ELane.SD:
						return this.SD;
					case (int) ELane.BD:
						return this.BD;
					case (int) ELane.HT:
						return this.HT;
					case (int) ELane.LT:
						return this.LT;
					case (int) ELane.FT:
						return this.FT;
					case (int) ELane.CY:
						return this.CY;
                    case (int) ELane.LP:
                        return this.LP;
					case (int) ELane.RD:
						return this.RD;
                    case (int) ELane.LBD:
                        return this.LBD;
					case (int) ELane.Guitar:
						return this.Guitar;
					case (int) ELane.Bass:
						return this.Bass;
					case (int) ELane.GtR:
						return this.GtR;
					case (int) ELane.GtG:
						return this.GtG;
					case (int) ELane.GtB:
						return this.GtB;
                    case (int) ELane.GtY:
                        return this.GtY;
                    case (int) ELane.GtP:
                        return this.GtP;
					case (int) ELane.GtPick:
						return this.GtPick;
					case (int) ELane.GtW:
						return this.GtW;
					case (int) ELane.BsR:
						return this.BsR;
					case (int) ELane.BsG:
						return this.BsG;
					case (int) ELane.BsB:
						return this.BsB;
                    case (int) ELane.BsY:
                        return this.BsY;
                    case (int) ELane.BsP:
                        return this.BsP;
					case (int) ELane.BsPick:
						return this.BsPick;
					case (int) ELane.BsW:
						return this.BsW;
				}
				throw new IndexOutOfRangeException();
			}
			set
			{
				switch ( index )
				{
					case (int) ELane.LC:
						this.LC = value;
						return;
					case (int) ELane.HH:
						this.HH = value;
						return;
					case (int) ELane.SD:
						this.SD = value;
						return;
					case (int) ELane.BD:
						this.BD = value;
						return;
					case (int) ELane.HT:
						this.HT = value;
						return;
					case (int) ELane.LT:
						this.LT = value;
						return;
					case (int) ELane.FT:
						this.FT = value;
						return;
					case (int) ELane.CY:
						this.CY = value;
						return;
                    case (int) ELane.LP:
                        this.LP = value;
                        return;
					case (int) ELane.RD:
						this.RD = value;
						return;
                    case (int) ELane.LBD:
                        this.LBD = value;
                        return;
					case (int) ELane.Guitar:
						this.Guitar = value;
						return;
					case (int) ELane.Bass:
						this.Bass = value;
						return;
					case (int) ELane.GtR:
						this.GtR = value;
						return;
					case (int) ELane.GtG:
						this.GtG = value;
						return;
					case (int) ELane.GtB:
						this.GtB = value;
						return;
                    case (int) ELane.GtY:
                        this.GtY = value;
                        return;
                    case (int) ELane.GtP:
                        this.GtP = value;
                        return;
					case (int) ELane.GtPick:
						this.GtPick = value;
						return;
					case (int) ELane.GtW:
						this.GtW = value;
						return;
					case (int) ELane.BsR:
						this.BsR = value;
						return;
					case (int) ELane.BsG:
						this.BsG = value;
						return;
					case (int) ELane.BsB:
						this.BsB = value;
						return;
                    case (int) ELane.BsY:
                        this.BsY = value;
                        return;
                    case (int) ELane.BsP:
                        this.BsP = value;
                        return;
					case (int) ELane.BsPick:
						this.BsPick = value;
						return;
					case (int) ELane.BsW:
						this.BsW = value;
						return;
				}
				throw new IndexOutOfRangeException();
			}
		}
	}


	[StructLayout( LayoutKind.Sequential )]
	public struct STAUTOPLAY								// Eレーンとindexを一致させること
	{
		public bool LC;			// 0
		public bool HH;			// 1
		public bool SD;			// 2
		public bool BD;			// 3
		public bool HT;			// 4
		public bool LT;			// 5
		public bool FT;			// 6
		public bool CY;			// 7
        public bool LP;
		public bool RD;			// 8
        public bool LBD;
		public bool Guitar;		// 9	(not used)
		public bool Bass;		// 10	(not used)
		public bool GtR;		// 11
		public bool GtG;		// 12
		public bool GtB;		// 13
        public bool GtY;
        public bool GtP;
		public bool GtPick;		// 14
		public bool GtW;		// 15
		public bool BsR;		// 16
		public bool BsG;		// 17
		public bool BsB;		// 18
        public bool BsY;
        public bool BsP;
		public bool BsPick;		// 19
		public bool BsW;		// 20
		public bool this[ int index ]
		{
			get
			{
				switch ( index )
				{
					case (int) ELane.LC:
						return this.LC;
					case (int) ELane.HH:
						return this.HH;
					case (int) ELane.SD:
						return this.SD;
					case (int) ELane.BD:
						return this.BD;
					case (int) ELane.HT:
						return this.HT;
					case (int) ELane.LT:
						return this.LT;
					case (int) ELane.FT:
						return this.FT;
					case (int) ELane.CY:
						return this.CY;
                    case (int) ELane.LP:
                        return this.LP;
					case (int) ELane.RD:
						return this.RD;
                    case (int) ELane.LBD:
                        return this.LBD;
                    case (int)ELane.Guitar:
                        if (!this.GtR) return false;
                        if (!this.GtG) return false;
                        if (!this.GtB) return false;
                        if (!this.GtY) return false;
                        if (!this.GtP) return false;
                        if (!this.GtPick) return false;
                        if (!this.GtW) return false;
                        return true;
                    case (int)ELane.Bass:
                        if (!this.BsR) return false;
                        if (!this.BsG) return false;
                        if (!this.BsB) return false;
                        if (!this.BsY) return false;
                        if (!this.BsP) return false;
                        if (!this.BsPick) return false;
                        if (!this.BsW) return false;
                        return true;
					case (int) ELane.GtR:
						return this.GtR;
					case (int) ELane.GtG:
						return this.GtG;
					case (int) ELane.GtB:
						return this.GtB;
                    case (int) ELane.GtY:
                        return this.GtY;
                    case (int) ELane.GtP:
                        return this.GtP;
					case (int) ELane.GtPick:
						return this.GtPick;
					case (int) ELane.GtW:
						return this.GtW;
					case (int) ELane.BsR:
						return this.BsR;
					case (int) ELane.BsG:
						return this.BsG;
					case (int) ELane.BsB:
						return this.BsB;
                    case (int) ELane.BsY:
                        return this.BsY;
                    case (int) ELane.BsP:
                        return this.BsP;
					case (int) ELane.BsPick:
						return this.BsPick;
					case (int) ELane.BsW:
						return this.BsW;
				}
				throw new IndexOutOfRangeException();
			}
			set
			{
				switch ( index )
				{
					case (int) ELane.LC:
						this.LC = value;
						return;
					case (int) ELane.HH:
						this.HH = value;
						return;
					case (int) ELane.SD:
						this.SD = value;
						return;
					case (int) ELane.BD:
						this.BD = value;
						return;
					case (int) ELane.HT:
						this.HT = value;
						return;
					case (int) ELane.LT:
						this.LT = value;
						return;
					case (int) ELane.FT:
						this.FT = value;
						return;
					case (int) ELane.CY:
						this.CY = value;
						return;
                    case (int) ELane.LP:
                        this.LP = value;
                        return;
					case (int) ELane.RD:
						this.RD = value;
						return;
                    case (int) ELane.LBD:
                        this.LBD = value;
                        return;
                    case (int)ELane.Guitar:
                        this.GtR = this.GtG = this.GtB = this.GtY = this.GtP = this.GtPick = this.GtW = value;
                        //this.GtR = this.GtG = this.GtB = this.GtPick = this.GtW = value;
                        return;
                    case (int)ELane.Bass:
                        this.BsR = this.BsG = this.BsB = this.BsY = this.BsP = this.BsPick = this.BsW = value;
                        //this.BsR = this.BsG = this.BsB = this.BsPick = this.BsW = value;
                        return;
					case (int) ELane.GtR:
						this.GtR = value;
						return;
					case (int) ELane.GtG:
						this.GtG = value;
						return;
					case (int) ELane.GtB:
						this.GtB = value;
						return;
                    case (int) ELane.GtY:
                        this.GtY = value;
                        return;
                    case (int) ELane.GtP:
                        this.GtP = value;
                        return;
					case (int) ELane.GtPick:
						this.GtPick = value;
						return;
					case (int) ELane.GtW:
						this.GtW = value;
						return;
					case (int) ELane.BsR:
						this.BsR = value;
						return;
					case (int) ELane.BsG:
						this.BsG = value;
						return;
					case (int) ELane.BsB:
						this.BsB = value;
						return;
                    case (int) ELane.BsY:
                        this.BsY = value;
                        return;
                    case (int) ELane.BsP:
                        this.BsP = value;
                        return;
					case (int) ELane.BsPick:
						this.BsPick = value;
						return;
					case (int) ELane.BsW:
						this.BsW = value;
						return;
				}
				throw new IndexOutOfRangeException();
			}
		}
	}


	internal class CConstants
	{
		public const int BGA_H = 1280;
		public const int BGA_W = 720;
		public const int HIDDEN_POS = 100;
		public const int MAX_AVI_LAYER = 1;
		public const int MAX_WAILING = 4;
		public const int PANEL_H = 0x1a;
		public const int PANEL_W = 0x116;
		public const int PREVIEW_H = 0x10d;
		public const int PREVIEW_W = 0xcc;
		public const int SCORE_H = 0x18;
		public const int SCORE_W = 12;
		public const int SUDDEN_POS = 200;
		public const int PLAYSPEED_MIN = 5;
		public const int PLAYSPEED_MAX = 40;

		public class Drums
		{
            public const int BAR_Y = 0x1a6;
            public const int BAR_Y_REV = 0x38;
            public const int BASS_BAR_Y = 0x5f;
            public const int BASS_BAR_Y_REV = 0x176;
            public const int BASS_H = 0x163;
            public const int BASS_W = 0x6d;
            public const int BASS_X = 0x18e;
            public const int BASS_Y = 0x39;
            public const int BGA_X = 1280;
            public const int BGA_Y = 720;
            public const int GAUGE_H = 0x160;
            public const int GAUGE_W = 0x10;
            public const int GAUGE_X = 6;
            public const int GAUGE_Y = 0x35;
            public const int GUITAR_BAR_Y = 0x5f;
            public const int GUITAR_BAR_Y_REV = 0x176;
            public const int GUITAR_H = 0x163;
            public const int GUITAR_W = 0x6d;
            public const int GUITAR_X = 0x1fb;
            public const int GUITAR_Y = 0x39;
            public const int PANEL_X = 0x150;
            public const int PANEL_Y = 0x1ab;
            public const int SCORE_X = 0x164;
            public const int SCORE_Y = 14;
		}
		public class Guitar
		{
			public const int BAR_Y = 40;
			public const int BAR_Y_REV = 0x171;
			public const int BASS_H = 0x199;
			public const int BASS_W = 140;
			public const int BASS_X = 480;
			public const int BASS_Y = 0;
			public const int BGA_X = 0xb5;
			public const int BGA_Y = 50;
			public const int GAUGE_H = 0x10;
			public const int GAUGE_W = 0x80;
			public const int GAUGE_X_BASS = 0x14f;
			public const int GAUGE_X_GUITAR = 0xb2;
			public const int GAUGE_Y_BASS = 8;
			public const int GAUGE_Y_GUITAR = 8;
			public const int GUITAR_H = 0x199;
			public const int GUITAR_W = 140;
			public const int GUITAR_X = 0x1a;
			public const int GUITAR_Y = 0;
			public const int PANEL_X = 0xb5;
			public const int PANEL_Y = 430;
		}
	}
}
