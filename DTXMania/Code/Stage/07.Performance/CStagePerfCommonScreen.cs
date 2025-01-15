using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
using System.Linq;
using FDK;

using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;
using SlimDXKey = SlimDX.DirectInput.Key;

namespace DTXMania
{
    /// <summary>
    /// 演奏画面の共通クラス (ドラム演奏画面, ギター演奏画面の継承元)
    /// </summary>
    internal abstract class CStagePerfCommonScreen : CStage
    {
        protected override RichPresence Presence
        {
            get
            {
                // if presence is displayed before this point then it will use
                // the unitialised timer time, displaying an incorrect timestamp
                // so dont display any presence until initialisation has occurred
                if (bJustStartedUpdate || CDTXMania.bCompactMode)
                    return null;

                var stSongInformation = CDTXMania.stageSongSelection.rSelectedScore.SongInformation;
                var rConfirmedSong = CDTXMania.stageSongSelection.rConfirmedSong;
                var nConfirmedDifficulty = CDTXMania.stageSongSelection.nConfirmedSongDifficulty;
                var nEndTimeMs = CDTXMania.DTX.listChip.OrderBy(c => c.nPlaybackTimeMs).LastOrDefault()?.nPlaybackTimeMs ?? 0;

                //Shorten details string to avoid hitting max of 128 bytes
                string detailsString = $"{rConfirmedSong.strタイトル}";
                if(detailsString.Length > 50)
                {
                    detailsString = detailsString.Substring(0, 50);
                }
                detailsString += $" [{rConfirmedSong.arDifficultyLabel[nConfirmedDifficulty]}]";
                return new CDTXRichPresence
                {
                    State = "In Game",

                    // title [difficulty]
                    // some songs omit the artist, so dont include the dash separator in such cases
                    Details = detailsString,

                    // playback speed is automatically applied as chip timings are modified,
                    // but the current time must be accounted for in start/end to display correct timestamps when seeking around
                    Timestamps = Timestamps.FromTimeSpan((nEndTimeMs - CSoundManager.rcPerformanceTimer.nCurrentTime) / 1000.0),
                };
            }
        }

        // プロパティ

        public CStagePerfCommonScreen()
        {
            base.listChildActivities.Add(this.actLVFont = new CActLVLNFont());
        }

        public bool bAUTOでないチップが１つでもバーを通過した
        {
            get;
            protected set;
        }

        // メソッド

        #region [ tStorePerfResults_Drums() ]
        public void tStorePerfResults_Drums(out CScoreIni.CPerformanceEntry Drums)
        {
            Drums = new CScoreIni.CPerformanceEntry();

            if (CDTXMania.DTX.bチップがある.Drums && !CDTXMania.ConfigIni.bGuitarRevolutionMode)
            {
                Drums.nスコア = (long)this.actScore.Get(EInstrumentPart.DRUMS);
                if (CDTXMania.ConfigIni.nSkillMode == 0)
                {
                    Drums.dbGameSkill = CScoreIni.tCalculateGameSkillOld(CDTXMania.DTX.LEVEL.Drums, CDTXMania.DTX.LEVELDEC.Drums, CDTXMania.DTX.nVisibleChipsCount.Drums, this.nHitCount_ExclAuto.Drums.Perfect, this.nHitCount_ExclAuto.Drums.Great, this.actCombo.nCurrentCombo.HighestValue.Drums, EInstrumentPart.DRUMS, bIsAutoPlay);
                    Drums.dbPerformanceSkill = CScoreIni.tCalculatePlayingSkillOld(CDTXMania.DTX.nVisibleChipsCount.Drums, this.nHitCount_ExclAuto.Drums.Perfect, this.nHitCount_ExclAuto.Drums.Great, this.nHitCount_ExclAuto.Drums.Good, this.nHitCount_ExclAuto.Drums.Poor, this.nHitCount_ExclAuto.Drums.Miss, this.actCombo.nCurrentCombo.HighestValue.Drums, EInstrumentPart.DRUMS, bIsAutoPlay);
                }
                else if (CDTXMania.ConfigIni.nSkillMode == 1)
                {
                    Drums.dbPerformanceSkill = CScoreIni.tCalculatePlayingSkill(CDTXMania.DTX.nVisibleChipsCount.Drums, this.nHitCount_ExclAuto.Drums.Perfect, this.nHitCount_ExclAuto.Drums.Great, this.nHitCount_ExclAuto.Drums.Good, this.nHitCount_ExclAuto.Drums.Poor, this.nHitCount_ExclAuto.Drums.Miss, this.actCombo.nCurrentCombo.HighestValue.Drums, EInstrumentPart.DRUMS, bIsAutoPlay);
                    Drums.dbGameSkill = CScoreIni.tCalculateGameSkillFromPlayingSkill(CDTXMania.DTX.LEVEL.Drums, CDTXMania.DTX.LEVELDEC.Drums, Drums.dbPerformanceSkill);
                }
                Drums.nPerfectCount = CDTXMania.ConfigIni.bAllDrumsAreAutoPlay ? this.nHitCount_IncAuto.Drums.Perfect : this.nHitCount_ExclAuto.Drums.Perfect;
                Drums.nGreatCount = CDTXMania.ConfigIni.bAllDrumsAreAutoPlay ? this.nHitCount_IncAuto.Drums.Great : this.nHitCount_ExclAuto.Drums.Great;
                Drums.nGoodCount = CDTXMania.ConfigIni.bAllDrumsAreAutoPlay ? this.nHitCount_IncAuto.Drums.Good : this.nHitCount_ExclAuto.Drums.Good;
                Drums.nPoorCount = CDTXMania.ConfigIni.bAllDrumsAreAutoPlay ? this.nHitCount_IncAuto.Drums.Poor : this.nHitCount_ExclAuto.Drums.Poor;
                Drums.nMissCount = CDTXMania.ConfigIni.bAllDrumsAreAutoPlay ? this.nHitCount_IncAuto.Drums.Miss : this.nHitCount_ExclAuto.Drums.Miss;
                Drums.nPerfectCount_ExclAuto = this.nHitCount_ExclAuto.Drums.Perfect;
                Drums.nGreatCount_ExclAuto = this.nHitCount_ExclAuto.Drums.Great;
                Drums.nGoodCount_ExclAuto = this.nHitCount_ExclAuto.Drums.Good;
                Drums.nPoorCount_ExclAuto = this.nHitCount_ExclAuto.Drums.Poor;
                Drums.nMissCount_ExclAuto = this.nHitCount_ExclAuto.Drums.Miss;
                Drums.nMaxCombo = this.actCombo.nCurrentCombo.HighestValue.Drums;
                Drums.nTotalChipsCount = CDTXMania.DTX.nVisibleChipsCount.Drums;
                for (int i = 0; i < (int)ELane.MAX; i++)
                {
                    Drums.bAutoPlay[i] = bIsAutoPlay[i];
                }
                Drums.bTight = CDTXMania.ConfigIni.bTight;
                for (int i = 0; i < 3; i++)
                {
                    Drums.bSudden[i] = CDTXMania.ConfigIni.bSudden[i];
                    Drums.bHidden[i] = CDTXMania.ConfigIni.bHidden[i];
                    Drums.bReverse[i] = CDTXMania.ConfigIni.bReverse[i];
                    Drums.eRandom[i] = CDTXMania.ConfigIni.eRandom[i];
                    Drums.bLight[i] = CDTXMania.ConfigIni.bLight[i];
                    Drums.bLeft[i] = CDTXMania.ConfigIni.bLeft[i];
                    Drums.fScrollSpeed[i] = ((float)(CDTXMania.ConfigIni.nScrollSpeed[i] + 1)) * 0.5f;
                }
                Drums.eDark = CDTXMania.ConfigIni.eDark;
                Drums.nPlaySpeedNumerator = CDTXMania.ConfigIni.nPlaySpeed;
                Drums.nPlaySpeedDenominator = 20;
                Drums.eHHGroup = CDTXMania.ConfigIni.eHHGroup;
                Drums.eFTGroup = CDTXMania.ConfigIni.eFTGroup;
                Drums.eCYGroup = CDTXMania.ConfigIni.eCYGroup;
                Drums.eHitSoundPriorityHH = CDTXMania.ConfigIni.eHitSoundPriorityHH;
                Drums.eHitSoundPriorityFT = CDTXMania.ConfigIni.eHitSoundPriorityFT;
                Drums.eHitSoundPriorityCY = CDTXMania.ConfigIni.eHitSoundPriorityCY;
                Drums.bGuitarEnabled = CDTXMania.ConfigIni.bGuitarEnabled;
                Drums.bDrumsEnabled = CDTXMania.ConfigIni.bDrumsEnabled;
                Drums.bSTAGEFAILEDEnabled = CDTXMania.ConfigIni.bSTAGEFAILEDEnabled;
                Drums.eDamageLevel = CDTXMania.ConfigIni.eDamageLevel;
                Drums.bKeyboardUsed = this.bKeyboardUsed.Drums;
                Drums.bMIDIUsed = this.bMIDIUsed.Drums;
                Drums.bJoypadUsed = this.bJoypadUsed.Drums;
                Drums.bMouseUsed = this.bMouseUsed.Drums;
                Drums.stPrimaryHitRanges = CDTXMania.stDrumHitRanges;
                Drums.stSecondaryHitRanges = CDTXMania.stDrumPedalHitRanges;
                Drums.strDTXManiaVersion = CDTXMania.VERSION;
                Drums.strDateTime = DateTime.Now.ToString();
                Drums.strProgress = this.actProgressBar.GetScoreIniString(EInstrumentPart.DRUMS);
                Drums.Hash = CScoreIni.tComputePerformanceSectionMD5(Drums);
            }
        }
        #endregion
        #region [ tStorePerfResults_Guitar() ]
        public void tStorePerfResults_Guitar(out CScoreIni.CPerformanceEntry Guitar)
        {
            Guitar = new CScoreIni.CPerformanceEntry();

            if (CDTXMania.DTX.bチップがある.Guitar)
            {
                Guitar.nスコア = (long)this.actScore.Get(EInstrumentPart.GUITAR);
                if (CDTXMania.ConfigIni.nSkillMode == 0)
                {
                    Guitar.dbGameSkill = CScoreIni.tCalculateGameSkillOld(CDTXMania.DTX.LEVEL.Guitar, CDTXMania.DTX.LEVELDEC.Guitar, CDTXMania.DTX.nVisibleChipsCount.Guitar, this.nHitCount_ExclAuto.Guitar.Perfect, this.nHitCount_ExclAuto.Guitar.Great, this.actCombo.nCurrentCombo.HighestValue.Guitar, EInstrumentPart.GUITAR, bIsAutoPlay);
                    Guitar.dbPerformanceSkill = CScoreIni.tCalculatePlayingSkillOld(CDTXMania.DTX.nVisibleChipsCount.Guitar, this.nHitCount_ExclAuto.Guitar.Perfect, this.nHitCount_ExclAuto.Guitar.Great, this.nHitCount_ExclAuto.Guitar.Good, this.nHitCount_ExclAuto.Guitar.Poor, this.nHitCount_ExclAuto.Guitar.Miss, this.actCombo.nCurrentCombo.HighestValue.Guitar, EInstrumentPart.GUITAR, bIsAutoPlay);
                }
                else if (CDTXMania.ConfigIni.nSkillMode == 1)
                {
                    Guitar.dbPerformanceSkill = CScoreIni.tCalculatePlayingSkill(CDTXMania.DTX.nVisibleChipsCount.Guitar, this.nHitCount_ExclAuto.Guitar.Perfect, this.nHitCount_ExclAuto.Guitar.Great, this.nHitCount_ExclAuto.Guitar.Good, this.nHitCount_ExclAuto.Guitar.Poor, this.nHitCount_ExclAuto.Guitar.Miss, this.actCombo.nCurrentCombo.HighestValue.Guitar, EInstrumentPart.GUITAR, bIsAutoPlay);
                    Guitar.dbGameSkill = CScoreIni.tCalculateGameSkillFromPlayingSkill(CDTXMania.DTX.LEVEL.Guitar, CDTXMania.DTX.LEVELDEC.Guitar, Guitar.dbPerformanceSkill);
                }
                Guitar.nPerfectCount = CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay ? this.nHitCount_IncAuto.Guitar.Perfect : this.nHitCount_ExclAuto.Guitar.Perfect;
                Guitar.nGreatCount = CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay ? this.nHitCount_IncAuto.Guitar.Great : this.nHitCount_ExclAuto.Guitar.Great;
                Guitar.nGoodCount = CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay ? this.nHitCount_IncAuto.Guitar.Good : this.nHitCount_ExclAuto.Guitar.Good;
                Guitar.nPoorCount = CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay ? this.nHitCount_IncAuto.Guitar.Poor : this.nHitCount_ExclAuto.Guitar.Poor;
                Guitar.nMissCount = CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay ? this.nHitCount_IncAuto.Guitar.Miss : this.nHitCount_ExclAuto.Guitar.Miss;
                Guitar.nPerfectCount_ExclAuto = this.nHitCount_ExclAuto.Guitar.Perfect;
                Guitar.nGreatCount_ExclAuto = this.nHitCount_ExclAuto.Guitar.Great;
                Guitar.nGoodCount_ExclAuto = this.nHitCount_ExclAuto.Guitar.Good;
                Guitar.nPoorCount_ExclAuto = this.nHitCount_ExclAuto.Guitar.Poor;
                Guitar.nMissCount_ExclAuto = this.nHitCount_ExclAuto.Guitar.Miss;
                Guitar.nMaxCombo = this.actCombo.nCurrentCombo.HighestValue.Guitar;
                Guitar.nTotalChipsCount = CDTXMania.DTX.nVisibleChipsCount.Guitar;
                for (int i = 0; i < (int)ELane.MAX; i++)
                {
                    Guitar.bAutoPlay[i] = bIsAutoPlay[i];
                }
                Guitar.bTight = CDTXMania.ConfigIni.bTight;
                for (int i = 0; i < 3; i++)
                {
                    Guitar.bSudden[i] = CDTXMania.ConfigIni.bSudden[i];
                    Guitar.bHidden[i] = CDTXMania.ConfigIni.bHidden[i];
                    Guitar.bReverse[i] = CDTXMania.ConfigIni.bReverse[i];
                    Guitar.eRandom[i] = CDTXMania.ConfigIni.eRandom[i];
                    Guitar.bLight[i] = CDTXMania.ConfigIni.bLight[i];
                    Guitar.bLeft[i] = CDTXMania.ConfigIni.bLeft[i];
                    Guitar.fScrollSpeed[i] = ((float)(CDTXMania.ConfigIni.nScrollSpeed[i] + 1)) * 0.5f;
                }
                Guitar.eDark = CDTXMania.ConfigIni.eDark;
                Guitar.nPlaySpeedNumerator = CDTXMania.ConfigIni.nPlaySpeed;
                Guitar.nPlaySpeedDenominator = 20;
                Guitar.eHHGroup = CDTXMania.ConfigIni.eHHGroup;
                Guitar.eFTGroup = CDTXMania.ConfigIni.eFTGroup;
                Guitar.eCYGroup = CDTXMania.ConfigIni.eCYGroup;
                Guitar.eHitSoundPriorityHH = CDTXMania.ConfigIni.eHitSoundPriorityHH;
                Guitar.eHitSoundPriorityFT = CDTXMania.ConfigIni.eHitSoundPriorityFT;
                Guitar.eHitSoundPriorityCY = CDTXMania.ConfigIni.eHitSoundPriorityCY;
                Guitar.bGuitarEnabled = CDTXMania.ConfigIni.bGuitarEnabled;
                Guitar.bDrumsEnabled = CDTXMania.ConfigIni.bDrumsEnabled;
                Guitar.bSTAGEFAILEDEnabled = CDTXMania.ConfigIni.bSTAGEFAILEDEnabled;
                Guitar.eDamageLevel = CDTXMania.ConfigIni.eDamageLevel;
                Guitar.bKeyboardUsed = this.bKeyboardUsed.Guitar;
                Guitar.bMIDIUsed = this.bMIDIUsed.Guitar;
                Guitar.bJoypadUsed = this.bJoypadUsed.Guitar;
                Guitar.bMouseUsed = this.bMouseUsed.Guitar;
                Guitar.stPrimaryHitRanges = CDTXMania.stGuitarHitRanges;
                Guitar.stSecondaryHitRanges = new STHitRanges();
                Guitar.strDTXManiaVersion = CDTXMania.VERSION;
                Guitar.strDateTime = DateTime.Now.ToString();
                Guitar.strProgress = this.actProgressBar.GetScoreIniString(EInstrumentPart.GUITAR);
                Guitar.Hash = CScoreIni.tComputePerformanceSectionMD5(Guitar);
            }
        }
        #endregion
        #region [ tStorePerfResultsBass() ]
        public void tStorePerfResultsBass(out CScoreIni.CPerformanceEntry Bass)
        {
            Bass = new CScoreIni.CPerformanceEntry();

            if (CDTXMania.DTX.bチップがある.Bass)
            {
                Bass.nスコア = (long)this.actScore.Get(EInstrumentPart.BASS);
                if (CDTXMania.ConfigIni.nSkillMode == 0)
                {
                    Bass.dbGameSkill = CScoreIni.tCalculateGameSkillOld(CDTXMania.DTX.LEVEL.Bass, CDTXMania.DTX.LEVELDEC.Bass, CDTXMania.DTX.nVisibleChipsCount.Bass, this.nHitCount_ExclAuto.Bass.Perfect, this.nHitCount_ExclAuto.Bass.Great, this.actCombo.nCurrentCombo.HighestValue.Bass, EInstrumentPart.BASS, bIsAutoPlay);
                    Bass.dbPerformanceSkill = CScoreIni.tCalculatePlayingSkillOld(CDTXMania.DTX.nVisibleChipsCount.Bass, this.nHitCount_ExclAuto.Bass.Perfect, this.nHitCount_ExclAuto.Bass.Great, this.nHitCount_ExclAuto.Bass.Good, this.nHitCount_ExclAuto.Bass.Poor, this.nHitCount_ExclAuto.Bass.Miss, this.actCombo.nCurrentCombo.HighestValue.Bass, EInstrumentPart.BASS, bIsAutoPlay);
                }
                else if (CDTXMania.ConfigIni.nSkillMode == 1)
                {
                    Bass.dbPerformanceSkill = CScoreIni.tCalculatePlayingSkill(CDTXMania.DTX.nVisibleChipsCount.Bass, this.nHitCount_ExclAuto.Bass.Perfect, this.nHitCount_ExclAuto.Bass.Great, this.nHitCount_ExclAuto.Bass.Good, this.nHitCount_ExclAuto.Bass.Poor, this.nHitCount_ExclAuto.Bass.Miss, this.actCombo.nCurrentCombo.HighestValue.Bass, EInstrumentPart.BASS, bIsAutoPlay);
                    Bass.dbGameSkill = CScoreIni.tCalculateGameSkillFromPlayingSkill(CDTXMania.DTX.LEVEL.Bass, CDTXMania.DTX.LEVELDEC.Bass, Bass.dbPerformanceSkill);
                }
                Bass.nPerfectCount = CDTXMania.ConfigIni.bAllBassAreAutoPlay ? this.nHitCount_IncAuto.Bass.Perfect : this.nHitCount_ExclAuto.Bass.Perfect;
                Bass.nGreatCount = CDTXMania.ConfigIni.bAllBassAreAutoPlay ? this.nHitCount_IncAuto.Bass.Great : this.nHitCount_ExclAuto.Bass.Great;
                Bass.nGoodCount = CDTXMania.ConfigIni.bAllBassAreAutoPlay ? this.nHitCount_IncAuto.Bass.Good : this.nHitCount_ExclAuto.Bass.Good;
                Bass.nPoorCount = CDTXMania.ConfigIni.bAllBassAreAutoPlay ? this.nHitCount_IncAuto.Bass.Poor : this.nHitCount_ExclAuto.Bass.Poor;
                Bass.nMissCount = CDTXMania.ConfigIni.bAllBassAreAutoPlay ? this.nHitCount_IncAuto.Bass.Miss : this.nHitCount_ExclAuto.Bass.Miss;
                Bass.nPerfectCount_ExclAuto = this.nHitCount_ExclAuto.Bass.Perfect;
                Bass.nGreatCount_ExclAuto = this.nHitCount_ExclAuto.Bass.Great;
                Bass.nGoodCount_ExclAuto = this.nHitCount_ExclAuto.Bass.Good;
                Bass.nPoorCount_ExclAuto = this.nHitCount_ExclAuto.Bass.Poor;
                Bass.nMissCount_ExclAuto = this.nHitCount_ExclAuto.Bass.Miss;
                Bass.nMaxCombo = this.actCombo.nCurrentCombo.HighestValue.Bass;
                Bass.nTotalChipsCount = CDTXMania.DTX.nVisibleChipsCount.Bass;
                for (int i = 0; i < (int)ELane.MAX; i++)
                {
                    Bass.bAutoPlay[i] = bIsAutoPlay[i];
                }
                Bass.bTight = CDTXMania.ConfigIni.bTight;
                for (int i = 0; i < 3; i++)
                {
                    Bass.bSudden[i] = CDTXMania.ConfigIni.bSudden[i];
                    Bass.bHidden[i] = CDTXMania.ConfigIni.bHidden[i];
                    Bass.bReverse[i] = CDTXMania.ConfigIni.bReverse[i];
                    Bass.eRandom[i] = CDTXMania.ConfigIni.eRandom[i];
                    Bass.bLight[i] = CDTXMania.ConfigIni.bLight[i];
                    Bass.bLeft[i] = CDTXMania.ConfigIni.bLeft[i];
                    Bass.fScrollSpeed[i] = ((float)(CDTXMania.ConfigIni.nScrollSpeed[i] + 1)) * 0.5f;
                }
                Bass.eDark = CDTXMania.ConfigIni.eDark;
                Bass.nPlaySpeedNumerator = CDTXMania.ConfigIni.nPlaySpeed;
                Bass.nPlaySpeedDenominator = 20;
                Bass.eHHGroup = CDTXMania.ConfigIni.eHHGroup;
                Bass.eFTGroup = CDTXMania.ConfigIni.eFTGroup;
                Bass.eCYGroup = CDTXMania.ConfigIni.eCYGroup;
                Bass.eHitSoundPriorityHH = CDTXMania.ConfigIni.eHitSoundPriorityHH;
                Bass.eHitSoundPriorityFT = CDTXMania.ConfigIni.eHitSoundPriorityFT;
                Bass.eHitSoundPriorityCY = CDTXMania.ConfigIni.eHitSoundPriorityCY;
                Bass.bGuitarEnabled = CDTXMania.ConfigIni.bGuitarEnabled;
                Bass.bDrumsEnabled = CDTXMania.ConfigIni.bDrumsEnabled;
                Bass.bSTAGEFAILEDEnabled = CDTXMania.ConfigIni.bSTAGEFAILEDEnabled;
                Bass.eDamageLevel = CDTXMania.ConfigIni.eDamageLevel;
                Bass.bKeyboardUsed = this.bKeyboardUsed.Bass;			// #24280 2011.1.29 yyagi
                Bass.bMIDIUsed = this.bMIDIUsed.Bass;				//
                Bass.bJoypadUsed = this.bJoypadUsed.Bass;		//
                Bass.bMouseUsed = this.bMouseUsed.Bass;					//
                Bass.stPrimaryHitRanges = CDTXMania.stBassHitRanges;
                Bass.stSecondaryHitRanges = new STHitRanges();
                Bass.strDTXManiaVersion = CDTXMania.VERSION;
                Bass.strDateTime = DateTime.Now.ToString();
                Bass.strProgress = this.actProgressBar.GetScoreIniString(EInstrumentPart.BASS);
                Bass.Hash = CScoreIni.tComputePerformanceSectionMD5(Bass);
            }
        }
        #endregion

        // CStage 実装

        public override void OnActivate()
        {
            listChip = CDTXMania.DTX.listChip;
            listWAV = CDTXMania.DTX.listWAV;

            this.eReturnValueAfterFadeOut = EPerfScreenReturnValue.Continue;
            this.nCurrentTopChip = (listChip.Count > 0) ? 0 : -1;
            this.LLastPlayedHHWAVNumber = new List<int>(16);
            this.nLastPlayedHHChannelNumber = 0;
            this.nLastPlayedWAVNumber.Guitar = -1;
            this.nLastPlayedWAVNumber.Bass = -1;
            for (int i = 0; i < 50; i++)
            {
                this.nLastPlayedBGMWAVNumber[i] = -1;
            }
            this.rNextGuitarChip = null;
            this.rNextBassChip = null;
            for (int j = 0; j < 12; j++)
            {
                this.r現在の空うちドラムChip[j] = null;
            }
            this.r現在の空うちギターChip = null;
            this.r現在の空うちベースChip = null;
            this.n最大コンボ数_TargetGhost = new STDGBVALUE<int>(); // #35411 2015.08.21 chnmr0 add
            for (int k = 0; k < 3; k++)
            {
                //for ( int n = 0; n < 5; n++ )
                {
                    this.nHitCount_ExclAuto[ k ] = new CHITCOUNTOFRANK();
                    this.nHitCount_IncAuto[ k ] = new CHITCOUNTOFRANK();
                    this.nヒット数_TargetGhost[ k ] = new CHITCOUNTOFRANK(); // #35411 2015.08.21 chnmr0 add

                    this.nTimingHitCount[k] = new CLAGTIMINGHITCOUNT();
                }
                this.queWailing[k] = new Queue<CChip>();
                this.r現在の歓声Chip[k] = null;
                //
                this.chipロングノートHit中[k] = null;
                this.nCurrentLongNoteDuration[k] = 0;
                this.nロングノートPart[k] = 0;
                //
                this.nAccumulatedLongNoteBonusScore[k] = 0;
            }
            for (int i = 0; i < 3; i++)
            {
                this.bKeyboardUsed[i] = false;
                this.bJoypadUsed[i] = false;
                this.bMIDIUsed[i] = false;
                this.bMouseUsed[i] = false;
                this.ctTimer[i] = null;//new CCounter(0, 3000, 1, CDTXMania.Timer);
                this.ctTimer[i] = new CCounter(0, 3000, 1, CDTXMania.Timer);
            }
            this.bAUTOでないチップが１つでもバーを通過した = false;
            //base.OnActivate();
            this.tSetStatusPanel();
            //this.tパネル文字列の設定();
            this.nJudgeLinePosY.Drums = (CDTXMania.ConfigIni.bReverse.Drums ? 159 + CDTXMania.ConfigIni.nJudgeLine.Drums : 561 - CDTXMania.ConfigIni.nJudgeLine.Drums);
            this.nJudgeLinePosY.Guitar = (CDTXMania.ConfigIni.bReverse.Guitar ? 611 - CDTXMania.ConfigIni.nJudgeLine.Guitar : 154 + CDTXMania.ConfigIni.nJudgeLine.Guitar);
            this.nJudgeLinePosY.Bass = (CDTXMania.ConfigIni.bReverse.Bass ? 611 - CDTXMania.ConfigIni.nJudgeLine.Bass : 154 + CDTXMania.ConfigIni.nJudgeLine.Bass);

            this.nShutterInPosY.Drums = CDTXMania.ConfigIni.nShutterInSide.Drums;
            this.nShutterOutPosY.Drums = CDTXMania.ConfigIni.nShutterOutSide.Drums;
            this.nShutterInPosY.Guitar = CDTXMania.ConfigIni.nShutterInSide.Guitar;
            this.nShutterOutPosY.Guitar = CDTXMania.ConfigIni.nShutterOutSide.Guitar;
            this.nShutterInPosY.Bass = CDTXMania.ConfigIni.nShutterInSide.Bass;
            this.nShutterOutPosY.Bass = CDTXMania.ConfigIni.nShutterOutSide.Bass;

            this.actJudgeString.iP_A = CDTXMania.ConfigIni.bReverse.Drums ? 159 + 0xbd : 561 - 0xbd;
            this.actJudgeString.iP_B = CDTXMania.ConfigIni.bReverse.Drums ? 159 - 0x17 : 561 + 0x17;

            this.nInputAdjustTimeMs.Drums = CDTXMania.ConfigIni.nInputAdjustTimeMs.Drums;		// #23580 2011.1.3 yyagi
            this.nInputAdjustTimeMs.Guitar = CDTXMania.ConfigIni.nInputAdjustTimeMs.Guitar;		//        2011.1.7 ikanick 修正
            this.nInputAdjustTimeMs.Bass = CDTXMania.ConfigIni.nInputAdjustTimeMs.Bass;			//
            this.nJudgeLinePosY_delta.Drums = CDTXMania.ConfigIni.nJudgeLinePosOffset.Drums;    // #31602 2013.6.23 yyagi
            this.nJudgeLinePosY_delta.Guitar = CDTXMania.ConfigIni.nJudgeLinePosOffset.Guitar;  //
            this.nJudgeLinePosY_delta.Bass = CDTXMania.ConfigIni.nJudgeLinePosOffset.Bass;      //
            this.bIsAutoPlay = CDTXMania.ConfigIni.bAutoPlay;									// #24239 2011.1.23 yyagi
            //this.bIsAutoPlay.Guitar = CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay;
            //this.bIsAutoPlay.Bass = CDTXMania.ConfigIni.bAllBassAreAutoPlay;										// #23559 2011.7.28 yyagi
            actGauge.Init(CDTXMania.ConfigIni.nRisky);									// #23559 2011.7.28 yyagi

            this.nPolyphonicSounds = CDTXMania.ConfigIni.nPoliphonicSounds;

            CDTXMania.Skin.tRemoveMixerAll();	// 効果音のストリームをミキサーから解除しておく

            //lockmixer = new object();
            queueMixerSound = new Queue<stmixer>(64);
            bIsDirectSound = (CDTXMania.SoundManager.GetCurrentSoundDeviceType() == "DirectSound");
            dbPlaySpeed = ((double)CDTXMania.ConfigIni.nPlaySpeed) / 20.0;
            bValidScore = true;

            this.LoopBeginMs = -1;
            this.LoopEndMs = -1;
            this.bIsTrainingMode = false;
            this.bPAUSE = false;

            #region [ Sounds that should be registered in the mixer before starting playing (chip sounds that will be played immediately after the start of the performance) ]
            foreach (CChip pChip in listChip)
            {
                //				Debug.WriteLine( "CH=" + pChip.nChannelNumber.ToString( "x2" ) + ", 整数値=" + pChip.nIntegerValue +  ", time=" + pChip.nPlaybackTimeMs );
                if (pChip.nPlaybackTimeMs <= 0)
                {
                    if (pChip.nChannelNumber == EChannel.MixChannel1_unc)
                    {
                        pChip.bHit = true;
                        //						Debug.WriteLine( "first [DA] BAR=" + pChip.nPlaybackPosition / 384 + " ch=" + pChip.nChannelNumber.ToString( "x2" ) + ", wav=" + pChip.nIntegerValue + ", time=" + pChip.nPlaybackTimeMs );
                        if (listWAV.ContainsKey(pChip.nIntegerValue_InternalNumber))
                        {
                            CDTX.CWAV wc = listWAV[pChip.nIntegerValue_InternalNumber];
                            for (int i = 0; i < nPolyphonicSounds; i++)
                            {
                                if (wc.rSound[i] != null)
                                {
                                    CDTXMania.SoundManager.AddMixer(wc.rSound[i], dbPlaySpeed, pChip.bChipKeepsPlayingAfterPerfEnds);
                                    //AddMixer( wc.rSound[ i ] );		// 最初はqueueを介さず直接ミキサー登録する
                                }
                            }
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            #endregion
            if (CDTXMania.ConfigIni.bIsSwappedGuitarBass)	// #24063 2011.1.24 yyagi Gt/Bsの譜面情報入れ替え
            {
                CDTXMania.DTX.SwapGuitarBassInfos();
            }
            this.bブーストボーナス = false;
            this.sw = new Stopwatch();
            this.sw2 = new Stopwatch();
            base.OnActivate();
            //this.tSetStatusPanel();
            //			this.gclatencymode = GCSettings.LatencyMode;
            //          GCSettings.LatencyMode = GCLatencyMode.Batch; // 演奏画面中はGCを抑止する
        }
        public override void OnDeactivate()
        {
            this.LLastPlayedHHWAVNumber.Clear();	// #23921 2011.1.4 yyagi
            this.LLastPlayedHHWAVNumber = null;	//
            for (int i = 0; i < 3; i++)
            {
                this.queWailing[i].Clear();
                this.queWailing[i] = null;
            }
            this.ctWailingChipPatternAnimation = null;
            this.ctBPMBar = null;
            this.ctChipPatternAnimation.Drums = null;
            this.ctChipPatternAnimation.Guitar = null;
            this.ctChipPatternAnimation.Bass = null;
            //listWAV.Clear();
            listWAV = null;
            listChip = null;
            queueMixerSound.Clear();
            queueMixerSound = null;
            //          GCSettings.LatencyMode = this.gclatencymode;
            if (this.caviGenericBackgroundVideo != null)
            {
                this.caviGenericBackgroundVideo.Dispose();
                this.caviGenericBackgroundVideo = null;
            }

            base.OnDeactivate();
        }
        public override void OnManagedCreateResources()
        {
            if (!base.bNotActivated)
            {
                //
                this.caviGenericBackgroundVideo = new CDTX.CAVI(1290, CSkin.Path(@"Graphics\7_Movie.mp4"), "", 20.0);
                this.caviGenericBackgroundVideo.OnDeviceCreated();
                if (caviGenericBackgroundVideo.avi != null)
                {
                    Trace.TraceInformation("Generic Background video loaded successfully");
                    this.actBackgroundAVI.bLoop = true;
                    this.actBackgroundAVI.Start(EChannel.MovieFull, caviGenericBackgroundVideo, 0, -1);
                    this.bGenericVideoEnabled = true;
                }
                else
                {
                    this.bGenericVideoEnabled = false;
                }
                this.tGenerateBackgroundTexture();

				this.txWailingFrame = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlay wailing cursor.png" ) );
                this.txBonusEffect = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\7_Fillin Effect.png" ) );
                if( CDTXMania.ConfigIni.nJudgeAnimeType == 1 )
                    this.tx判定画像anime = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\7_judge strings.png" ) );
                else if( CDTXMania.ConfigIni.nJudgeAnimeType == 2 )
                {
                    this.tx判定画像anime = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\7_JudgeStrings_XG.png" ) );
                    this.tx判定画像anime_2 = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\7_JudgeStrings_XG.png" ) );
                    this.tx判定画像anime_3 = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\7_JudgeStrings_XG.png" ) );
                }
                if (CDTXMania.ConfigIni.nShowPlaySpeed == (int)EShowPlaySpeed.ON)
                {
                    tGeneratePlaySpeedTexture();
                }

                base.OnManagedCreateResources();
            }
        }
        public override void OnManagedReleaseResources()
        {
            if (!base.bNotActivated)
            {
                this.actBackgroundAVI.Stop();

                CDTXMania.tReleaseTexture(ref this.tx背景);

                CDTXMania.tReleaseTexture(ref this.txWailingFrame);
                CDTXMania.tReleaseTexture(ref this.tx判定画像anime);
                CDTXMania.tReleaseTexture(ref this.tx判定画像anime_2);
                CDTXMania.tReleaseTexture(ref this.tx判定画像anime_3);
                CDTXMania.tReleaseTexture(ref this.txBonusEffect);
                CDTXMania.tReleaseTexture(ref this.txPlaySpeed);
                base.OnManagedReleaseResources();
            }
        }

        // Other

        #region [ protected ]
        //-----------------
        public class CHITCOUNTOFRANK
        {
            // Fields
            public int Good;
            public int Great;
            public int Miss;
            public int Perfect;
            public int Poor;

            // Properties
            public int this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return this.Perfect;

                        case 1:
                            return this.Great;

                        case 2:
                            return this.Good;

                        case 3:
                            return this.Poor;

                        case 4:
                            return this.Miss;
                    }
                    throw new IndexOutOfRangeException();
                }
                set
                {
                    switch (index)
                    {
                        case 0:
                            this.Perfect = value;
                            return;

                        case 1:
                            this.Great = value;
                            return;

                        case 2:
                            this.Good = value;
                            return;

                        case 3:
                            this.Poor = value;
                            return;

                        case 4:
                            this.Miss = value;
                            return;
                    }
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public class CLAGTIMINGHITCOUNT 
        {
            public enum ETiming
            {
                Late,                
                Early,                
                Unknown
            }

            // Fields
            public int nLate;
            //public int nPerfect;
            public int nEarly;
            //public int nOutOfRange;

            // Properties
            public int this[ETiming index]
            {
                get
                {
                    switch (index)
                    {
                        case ETiming.Late:
                            return this.nLate;

                        case ETiming.Early:
                            return this.nEarly;
                    }
                    throw new IndexOutOfRangeException();
                }
                set
                {
                    switch (index)
                    {
                        case ETiming.Late:
                            this.nLate = value;
                            return;

                        case ETiming.Early:
                            this.nEarly = value;
                            return;
                    }
                    throw new IndexOutOfRangeException();
                }
            }

        }

        static CStagePerfCommonScreen()
        {
            nJudgeLineMinPosY = 461;//(CDTXMania.ConfigIni.bReverse.Drums ? 259 : 461);
            nJudgeLineMaxPosY = 561;//(CDTXMania.ConfigIni.bReverse.Drums ? 159 : 561);
            nShutterMinPosY = 0;
            nShutterMaxPosY = 100;
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct STKARAUCHI
        {
            public CChip HH;
            public CChip SD;
            public CChip BD;
            public CChip HT;
            public CChip LT;
            public CChip FT;
            public CChip CY;
            public CChip HHO;
            public CChip RD;
            public CChip LC;
            public CChip LP;
            public CChip LBD;
            public CChip this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return this.HH;

                        case 1:
                            return this.SD;

                        case 2:
                            return this.BD;

                        case 3:
                            return this.HT;

                        case 4:
                            return this.LT;

                        case 5:
                            return this.FT;

                        case 6:
                            return this.CY;

                        case 7:
                            return this.HHO;

                        case 8:
                            return this.RD;

                        case 9:
                            return this.LC;

                        case 10:
                            return this.LP;

                        case 11:
                            return this.LBD;

                    }
                    throw new IndexOutOfRangeException();
                }
                set
                {
                    switch (index)
                    {
                        case 0:
                            this.HH = value;
                            return;

                        case 1:
                            this.SD = value;
                            return;

                        case 2:
                            this.BD = value;
                            return;

                        case 3:
                            this.HT = value;
                            return;

                        case 4:
                            this.LT = value;
                            return;

                        case 5:
                            this.FT = value;
                            return;

                        case 6:
                            this.CY = value;
                            return;

                        case 7:
                            this.HHO = value;
                            return;

                        case 8:
                            this.RD = value;
                            return;

                        case 9:
                            this.LC = value;
                            return;

                        case 10:
                            this.LP = value;
                            return;

                        case 11:
                            this.LBD = value;
                            return;

                    }
                    throw new IndexOutOfRangeException();
                }
            }
        }
        protected struct stmixer
        {
            internal bool bIsAdd;
            internal CSound csound;
            internal bool b演奏終了後も再生が続くチップである;
        };

        protected enum HitState
        {
            NotHit,
            Hit,
            DontCare
        }

        public CActPerfAVI actAVI;
        public CActPerfBGA actBGA;

        protected CActLVLNFont actLVFont;
        protected CActPerfChipFireGB actChipFireGB;
        public CActPerfCommonCombo actCombo;
        protected CActPerfCommonDanger actDANGER;
        protected CActFIFOBlackStart actFI;
        protected CActFIFOBlack actFO;
        protected CActFIFOWhite actFOClear;
        public CActFIFOWhiteClear actFOStageClear;
        //protected CActPerfStageClear actStageClear;
        public CActPerfCommonGauge actGauge;
        public CActPerfDrumsFillingEffect actFillin;
        protected CActPerfCommonJudgementString actJudgeString;
        protected CActPerfDrumsLaneFlushD actLaneFlushD;
        protected CActPerfCommonLaneFlushGB actLaneFlushGB;
//      protected CActPerfPanelString actPanel;
        protected CActPerformanceInformation actPlayInfo;
        public CActPerfCommonRGB actRGB;
        public CActPerfCommonScore actScore;
        protected CActPerfStageFailure actStageFailed;
        public CActPerfCommonStatusPanel actStatusPanel;
        protected CActPerfCommonWailingBonus actWailingBonus;
        public CActPerfScrollSpeed actScrollSpeed;
        protected CActPerfSkillMeter actGraph;
        protected CActPerfGuitarBonus actGuitarBonus;
        protected CActPerfProgressBar actProgressBar;
        protected CActSelectBackgroundAVI actBackgroundAVI;
        protected bool bPAUSE;
        protected STDGBVALUE<bool> bMIDIUsed;
        protected STDGBVALUE<bool> bKeyboardUsed;
        protected STDGBVALUE<bool> bJoypadUsed;
        protected STDGBVALUE<bool> bMouseUsed;
        protected CCounter ctWailingChipPatternAnimation;
        public CCounter ctBPMBar;
        public CCounter ct登場用;
        public CCounter ctComboTimer;

        protected STDGBVALUE<CCounter> ctChipPatternAnimation;
        protected abstract void tJudgeLineMovingUpandDown();
        protected EPerfScreenReturnValue eReturnValueAfterFadeOut;
        protected readonly EChannel[,] nBGAスコープチャンネルマップ = new EChannel[,] { { EChannel.BGALayer1_Swap, EChannel.BGALayer2_Swap, EChannel.BGALayer3_Swap, EChannel.BGALayer4_Swap, EChannel.BGALayer5_Swap, EChannel.BGALayer6_Swap, EChannel.BGALayer7_Swap, EChannel.BGALayer8_Swap }, { EChannel.BGALayer1, EChannel.BGALayer2, EChannel.BGALayer3, EChannel.BGALayer4, EChannel.BGALayer5, EChannel.BGALayer6, EChannel.BGALayer7, EChannel.BGALayer8 } };
        protected readonly int[] nチャンネル0Atoパッド08 = new int[] { 1, 2, 3, 4, 5, 7, 6, 1, 8, 0, 9, 9 };
        protected readonly int[] nチャンネル0Atoレーン07 = new int[] { 1, 2, 3, 4, 5, 7, 6, 1, 9, 0, 8, 8 };
        //                         RD LC  LP  RD
        protected readonly int[] nパッド0Atoチャンネル0A = new int[] { 0x11, 0x12, 0x13, 0x14, 0x15, 0x17, 0x16, 0x18, 0x19, 0x1a, 0x1b, 0x1c };
        protected readonly int[] nパッド0Atoパッド08 = new int[] { 1, 2, 3, 4, 5, 6, 7, 1, 8, 0, 9, 9 };// パッド画像のヒット処理用
        //   HH SD BD HT LT FT CY HHO RD LC LP LBD
        protected readonly int[] nパッド0Atoレーン07 = new int[] { 1, 2, 3, 4, 5, 6, 7, 1, 9, 0, 8, 8 };
        protected readonly float[,] fDamageGaugeDelta = new float[,] { { 0.004f, 0.006f, 0.006f }, { 0.002f, 0.003f, 0.003f }, { 0f, 0f, 0f }, { -0.02f, -0.03f, -0.03f }, { -0.05f, -0.05f, -0.05f } };
        protected readonly float[] fDamageLevelFactor = new float[] { 0.25f, 0.5f, 0.75f }; //Original: 0.5f, 1f, 1.5f

        public STDGBVALUE<int> nJudgeLinePosY = new STDGBVALUE<int>();//(CDTXMania.ConfigIni.bReverse.Drums ? 159 : 561);
        public STDGBVALUE<int> nShutterInPosY = new STDGBVALUE<int>();
        public STDGBVALUE<int> nShutterOutPosY = new STDGBVALUE<int>();
        public long n現在のスコア = 0;
        public STDGBVALUE<CHITCOUNTOFRANK> nHitCount_ExclAuto;
        public STDGBVALUE<CHITCOUNTOFRANK> nHitCount_IncAuto;
        //
        public STDGBVALUE<CLAGTIMINGHITCOUNT> nTimingHitCount;

        protected int nCurrentTopChip = -1;
        protected int[] nLastPlayedBGMWAVNumber = new int[50];
        protected static int nJudgeLineMaxPosY;
        protected static int nJudgeLineMinPosY;
        protected static int nShutterMaxPosY;
        protected static int nShutterMinPosY;
        protected EChannel nLastPlayedHHChannelNumber;
        protected List<int> LLastPlayedHHWAVNumber;		// #23921 2011.1.4 yyagi: change "int" to "List<int>", for recording multiple wav No.
        protected STLANEVALUE<int> nLastPlayedWAVNumber;	// #26388 2011.11.8 yyagi: change "nLastPlayedWAVNumber.GUITAR" and "nLastPlayedWAVNumber.BASS"
        //							into "nLastPlayedWAVNumber";
        //		protected int nLastPlayedWAVNumber.GUITAR;
        //		protected int nLastPlayedWAVNumber.BASS;

        protected volatile Queue<stmixer> queueMixerSound; // #24820 2013.1.21 yyagi まずは単純にAdd/Removeを1個のキューでまとめて管理するやり方で設計する
        protected DateTime dtLastQueueOperation; //
        protected bool bIsDirectSound; //
        protected double dbPlaySpeed;
        protected bool bValidScore;
        protected STDGBVALUE<int> nJudgeLinePosY_delta; // #31602 2013.6.23 yyagi 表示遅延対策として、判定ラインの表示位置をずらす機能を追加する

        private CCounter[] ctTimer = new CCounter[3];
        public bool bブーストボーナス = false;

        protected STDGBVALUE<Queue<CChip>> queWailing;
        protected STDGBVALUE<CChip> r現在の歓声Chip;
        protected CChip r現在の空うちギターChip;
        protected STKARAUCHI r現在の空うちドラムChip;
        protected CChip r現在の空うちベースChip;
        protected CChip rNextGuitarChip;
        protected CChip rNextBassChip;
        protected CTexture txWailingFrame;
        protected CTexture txChip;  // txチップ
        protected CTexture txHitBar;  // txヒットバー
        protected CTexture txPlaySpeed;
        public CTexture tx判定画像anime;     //2013.8.2 kairera0467 アニメーションの場合はあらかじめこっちで読み込む。
        public CTexture tx判定画像anime_2;   //2014.3.16 kairera0467 棒とかで必要になる。
        public CTexture tx判定画像anime_3;
        public CTexture txBonusEffect;

        //fork
        protected STDGBVALUE<CHITCOUNTOFRANK> nヒット数_TargetGhost; // #35411 2015.08.21 chnmr0 add
        protected STDGBVALUE<int> nコンボ数_TargetGhost;
        public STDGBVALUE<int> n最大コンボ数_TargetGhost;

        protected CTexture tx背景;
        protected STDGBVALUE<int> nInputAdjustTimeMs;		// #23580 2011.1.3 yyagi
        public STAUTOPLAY bIsAutoPlay;		// #24239 2011.1.23 yyagi
        //		protected int nRisky_InitialVar, nRiskyTime;		// #23559 2011.7.28 yyagi → CAct演奏ゲージ共通クラスに隠蔽
        protected int nPolyphonicSounds;

        protected List<CChip> listChip;
        protected Dictionary<int, CDTX.CWAV> listWAV;

        protected Stopwatch sw;		// 2011.6.13 最適化検討用のストップウォッチ
        protected Stopwatch sw2;
        //		protected GCLatencyMode gclatencymode;

        protected long LoopBeginMs;
        protected long LoopEndMs;

        //Generic video object
        private CDTX.CAVI caviGenericBackgroundVideo;
        private bool bGenericVideoEnabled;

        // Use a property instead of a field to automatically set training mode on the graph too
        private bool _bIsTrainingMode;
        public bool bIsTrainingMode
        {
            get
            {
                return this._bIsTrainingMode;
            }
            set
            {
                this._bIsTrainingMode = value;
                if (this.actGraph != null)
                {
                    actGraph.bIsTrainingMode = value;
                }
            }
        }

        //Long notes Cache variables
        private STDGBVALUE<CChip> chipロングノートHit中;
        private STDGBVALUE<int> nロングノートPart; //0 to 5, default 0
        private STDGBVALUE<int> nCurrentLongNoteDuration; //in ms

        //Long Note Accumulated Bonus (For Max score computation only)
        private STDGBVALUE<int> nAccumulatedLongNoteBonusScore;


        public void AddMixer(CSound cs, bool _b演奏終了後も再生が続くチップである)
        {
            stmixer stm = new stmixer()
            {
                bIsAdd = true,
                csound = cs,
                b演奏終了後も再生が続くチップである = _b演奏終了後も再生が続くチップである
            };
            queueMixerSound.Enqueue(stm);
            //Debug.WriteLine("★Queue: add " + Path.GetFileName(stm.csound.strFilename));
        }
        public void RemoveMixer(CSound cs)
        {
            stmixer stm = new stmixer()
            {
                bIsAdd = false,
                csound = cs,
                b演奏終了後も再生が続くチップである = false
            };
            queueMixerSound.Enqueue(stm);
            //Debug.WriteLine("★Queue: remove " + Path.GetFileName(stm.csound.strFilename));
        }
        public void ManageMixerQueue()
        {
            // もしサウンドの登録/削除が必要なら、実行する
            if (queueMixerSound.Count > 0)
            {
                //Debug.WriteLine( "☆queueLength=" + queueMixerSound.Count );
                DateTime dtnow = DateTime.Now;
                TimeSpan ts = dtnow - dtLastQueueOperation;
                if (ts.Milliseconds > 7)
                {
                    for (int i = 0; i < 2 && queueMixerSound.Count > 0; i++)
                    {
                        dtLastQueueOperation = dtnow;
                        stmixer stm = queueMixerSound.Dequeue();
                        if (stm.bIsAdd)
                        {
                            CDTXMania.SoundManager.AddMixer(stm.csound, dbPlaySpeed, stm.b演奏終了後も再生が続くチップである);
                        }
                        else
                        {
                            CDTXMania.SoundManager.RemoveMixer(stm.csound);
                        }
                    }
                }
            }
        }

		protected EJudgement e指定時刻からChipのJUDGEを返す( long nTime, CChip pChip, int nInputAdjustTime, bool saveLag = true )
        {
            if (pChip == null)
                return EJudgement.Miss;

            // #35411 2015.08.22 chnmr0 modified add check save lag flag for ghost
            int lag = (int)(nTime + nInputAdjustTime - pChip.nPlaybackTimeMs);
            if (saveLag)
            {
                pChip.nLag = lag;       // #23580 2011.1.3 yyagi: add "nInputAdjustTime" to add input timing adjust feature
				if (pChip.eInstrumentPart != EInstrumentPart.UNKNOWN)
				{
					pChip.nCurrentComboForGhost = this.actCombo.nCurrentCombo[(int)pChip.eInstrumentPart];
				}
            }
            // #35411 modify end

            // chips for all instrument parts are passed here, so processing cannot be delegated to subclasses
            // note that unknown part chips can also be passed, such as measure and quarter lines
            // in these cases it expects to receive default hit ranges
            int nDeltaTimeMs = Math.Abs(lag);
            switch (pChip.eInstrumentPart)
            {
                // drum chips
                case EInstrumentPart.DRUMS:
                    switch (pChip.nChannelNumber)
                    {
                        // drum pedal chips
                        case EChannel.BassDrum: //kick
                        case EChannel.LeftPedal: //left pedal
                        case EChannel.LeftBassDrum: //left bass drum
                            return CDTXMania.stDrumPedalHitRanges.tGetJudgement(nDeltaTimeMs);

                        // all other drum chips
                        default:
                            return CDTXMania.stDrumHitRanges.tGetJudgement(nDeltaTimeMs);
                    }

                // guitar chips
                case EInstrumentPart.GUITAR:
                    return CDTXMania.stGuitarHitRanges.tGetJudgement(nDeltaTimeMs);

                // bass chips
                case EInstrumentPart.BASS:
                    return CDTXMania.stBassHitRanges.tGetJudgement(nDeltaTimeMs);

                // all other chips (measure lines, quarter lines, etc.)
                case EInstrumentPart.UNKNOWN:
                default:
                    return STHitRanges.tCreateDefaultDTXHitRanges().tGetJudgement(nDeltaTimeMs);
            }
        }

        protected CChip r空うちChip(EInstrumentPart part, EPad pad)
        {
            switch (part)
            {
                case EInstrumentPart.DRUMS:
                    switch (pad)
                    {
                        case EPad.HH:
                            if (this.r現在の空うちドラムChip.HH != null)
                            {
                                return this.r現在の空うちドラムChip.HH;
                            }
                            if (CDTXMania.ConfigIni.eHHGroup != EHHGroup.ハイハットのみ打ち分ける)
                            {
                                if (CDTXMania.ConfigIni.eHHGroup == EHHGroup.左シンバルのみ打ち分ける)
                                {
                                    return this.r現在の空うちドラムChip.HHO;
                                }
                                if (this.r現在の空うちドラムChip.HHO != null)
                                {
                                    return this.r現在の空うちドラムChip.HHO;
                                }
                            }
                            return this.r現在の空うちドラムChip.LC;

                        case EPad.SD:
                            return this.r現在の空うちドラムChip.SD;

                        case EPad.BD:
                            return this.r現在の空うちドラムChip.BD;

                        case EPad.HT:
                            return this.r現在の空うちドラムChip.HT;

                        case EPad.LT:
                            if (this.r現在の空うちドラムChip.LT != null)
                            {
                                return this.r現在の空うちドラムChip.LT;
                            }
                            if (CDTXMania.ConfigIni.eFTGroup == EFTGroup.共通)
                            {
                                return this.r現在の空うちドラムChip.FT;
                            }
                            return null;

                        case EPad.FT:
                            if (this.r現在の空うちドラムChip.FT != null)
                            {
                                return this.r現在の空うちドラムChip.FT;
                            }
                            if (CDTXMania.ConfigIni.eFTGroup == EFTGroup.共通)
                            {
                                return this.r現在の空うちドラムChip.LT;
                            }
                            return null;

                        case EPad.CY:
                            if (this.r現在の空うちドラムChip.CY != null)
                            {
                                return this.r現在の空うちドラムChip.CY;
                            }
                            if (CDTXMania.ConfigIni.eCYGroup == ECYGroup.共通)
                            {
                                return this.r現在の空うちドラムChip.RD;
                            }
                            return null;

                        case EPad.HHO:
                            if (this.r現在の空うちドラムChip.HHO != null)
                            {
                                return this.r現在の空うちドラムChip.HHO;
                            }
                            if (CDTXMania.ConfigIni.eHHGroup != EHHGroup.ハイハットのみ打ち分ける)
                            {
                                if (CDTXMania.ConfigIni.eHHGroup == EHHGroup.左シンバルのみ打ち分ける)
                                {
                                    return this.r現在の空うちドラムChip.HH;
                                }
                                if (this.r現在の空うちドラムChip.HH != null)
                                {
                                    return this.r現在の空うちドラムChip.HH;
                                }
                            }
                            return this.r現在の空うちドラムChip.LC;

                        case EPad.RD:
                            if (this.r現在の空うちドラムChip.RD != null)
                            {
                                return this.r現在の空うちドラムChip.RD;
                            }
                            if (CDTXMania.ConfigIni.eCYGroup == ECYGroup.共通)
                            {
                                return this.r現在の空うちドラムChip.CY;
                            }
                            return null;

                        case EPad.LC:
                            if (this.r現在の空うちドラムChip.LC != null)
                            {
                                return this.r現在の空うちドラムChip.LC;
                            }
                            if ((CDTXMania.ConfigIni.eHHGroup != EHHGroup.ハイハットのみ打ち分ける) && (CDTXMania.ConfigIni.eHHGroup != EHHGroup.全部共通))
                            {
                                return null;
                            }
                            if (this.r現在の空うちドラムChip.HH != null)
                            {
                                return this.r現在の空うちドラムChip.HH;
                            }
                            return this.r現在の空うちドラムChip.HHO;

                        case EPad.LP:
                            if (this.r現在の空うちドラムChip.LP != null)
                            {
                                return this.r現在の空うちドラムChip.LP;
                            }
                            if (CDTXMania.ConfigIni.eBDGroup != EBDGroup.左右ペダルのみ打ち分ける)
                            {
                                if (this.r現在の空うちドラムChip.LBD != null)
                                {
                                    return this.r現在の空うちドラムChip.LBD;
                                }
                            }
                            return this.r現在の空うちドラムChip.LP;

                        case EPad.LBD:
                            if (this.r現在の空うちドラムChip.LBD != null)
                            {
                                return this.r現在の空うちドラムChip.LBD;
                            }
                            if (CDTXMania.ConfigIni.eBDGroup != EBDGroup.左右ペダルのみ打ち分ける)
                            {
                                if (this.r現在の空うちドラムChip.LP != null)
                                {
                                    return this.r現在の空うちドラムChip.LP;
                                }
                            }
                            return this.r現在の空うちドラムChip.LBD;


                    }
                    break;

                case EInstrumentPart.GUITAR:
                    return this.r現在の空うちギターChip;

                case EInstrumentPart.BASS:
                    return this.r現在の空うちベースChip;
            }
            return null;
        }
        protected CChip r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(long nTime, int int_nChannel, int nInputAdjustTime)
        {
            EChannel nChannel = (EChannel)int_nChannel;
            sw2.Start();
            nTime += nInputAdjustTime;						// #24239 2011.1.23 yyagi InputAdjust

            int nIndex_InitialPositionSearchingToPast;
            if (this.nCurrentTopChip == -1)				// 演奏データとして1個もチップがない場合は
            {
                sw2.Stop();
                return null;
            }
            int count = listChip.Count;
            int nIndex_NearestChip_Future = nIndex_InitialPositionSearchingToPast = this.nCurrentTopChip;
            if (this.nCurrentTopChip >= count)			// その時点で演奏すべきチップが既に全部無くなっていたら
            {
                nIndex_NearestChip_Future = nIndex_InitialPositionSearchingToPast = count - 1;
            }
            for (; nIndex_NearestChip_Future < count; nIndex_NearestChip_Future++)
            {
                CChip chip = CDTXMania.DTX.listChip[nIndex_NearestChip_Future];
                if (((EChannel.HiHatClose <= nChannel) && (nChannel <= EChannel.LeftBassDrum)))
                {
                    if ((chip.nChannelNumber == nChannel) || (chip.nChannelNumber == ((int)nChannel + EChannel.Guitar_Open)))
                    {
                        if (chip.nPlaybackTimeMs > nTime)
                        {
                            break;
                        }
                        nIndex_InitialPositionSearchingToPast = nIndex_NearestChip_Future;
                    }
                    continue;	// ほんの僅かながら高速化
                }
                else if ((nChannel == EChannel.Guitar_WailingSound && chip.eInstrumentPart == EInstrumentPart.GUITAR) || (((EChannel.Guitar_Open <= nChannel && nChannel <= EChannel.Guitar_Wailing) || (EChannel.Guitar_xxxYx <= nChannel && nChannel <= EChannel.Guitar_RxxxP) || (EChannel.Guitar_RxBxP <= nChannel && nChannel <= EChannel.Guitar_xGBYP) || (EChannel.Guitar_RxxYP <= nChannel && nChannel <= EChannel.Guitar_RGBYP)) && chip.nChannelNumber == nChannel))
                {
                    if (chip.nPlaybackTimeMs > nTime)
                    {
                        break;
                    }
                    nIndex_InitialPositionSearchingToPast = nIndex_NearestChip_Future;
                }
                else if ((nChannel == EChannel.BonusEffect) && (chip.eInstrumentPart == EInstrumentPart.BASS) || (((EChannel.Bass_Open <= nChannel && nChannel <= EChannel.Bass_Wailing) || (EChannel.Bass_xxxYx <= nChannel && nChannel <= EChannel.Bass_xxBYx) || (EChannel.Bass_xGxYx <= nChannel && nChannel <= EChannel.Bass_xxBxP) || (EChannel.Bass_xGxxP <= nChannel && nChannel <= EChannel.Bass_RGBxP) || (EChannel.Bass_xxxYP <= nChannel && nChannel <= EChannel.Bass_RGBYP)) && chip.nChannelNumber == nChannel))
                {
                    if (chip.nPlaybackTimeMs > nTime)
                    {
                        break;
                    }
                    nIndex_InitialPositionSearchingToPast = nIndex_NearestChip_Future;
                }
                // nIndex_NearestChip_Future++;
            }
            int nIndex_NearestChip_Past = nIndex_InitialPositionSearchingToPast;
            //while ( nIndex_NearestChip_Past >= 0 )			// 過去方向への検索
            for (; nIndex_NearestChip_Past >= 0; nIndex_NearestChip_Past--)
            {
                CChip chip = listChip[nIndex_NearestChip_Past];
                if ((EChannel.HiHatClose <= nChannel) && (nChannel <= EChannel.LeftBassDrum))
                {
                    if ((chip.nChannelNumber == nChannel) || (chip.nChannelNumber == ((int)nChannel + EChannel.Guitar_Open)))
                    {
                        break;
                    }
                }
                else if ((nChannel == EChannel.Guitar_WailingSound && chip.eInstrumentPart == EInstrumentPart.GUITAR) || (((EChannel.Guitar_Open <= nChannel && nChannel <= EChannel.Guitar_Wailing) || (EChannel.Guitar_xxxYx <= nChannel && nChannel <= EChannel.Guitar_RxxxP) || (EChannel.Guitar_RxBxP <= nChannel && nChannel <= EChannel.Guitar_xGBYP) || (EChannel.Guitar_RxxYP <= nChannel && nChannel <= EChannel.Guitar_RGBYP)) && chip.nChannelNumber == nChannel))
                {
                    if ((EChannel.Guitar_Open <= chip.nChannelNumber && chip.nChannelNumber <= EChannel.Guitar_Wailing) || (((EChannel.Guitar_Open <= nChannel && nChannel <= EChannel.Guitar_Wailing) || (EChannel.Guitar_xxxYx <= nChannel && nChannel <= EChannel.Guitar_RxxxP) || (EChannel.Guitar_RxBxP <= nChannel && nChannel <= EChannel.Guitar_xGBYP) || (EChannel.Guitar_RxxYP <= nChannel && nChannel <= EChannel.Guitar_RGBYP)) && chip.nChannelNumber == nChannel))
                    {
                        break;
                    }
                }
                else if (((nChannel == EChannel.Guitar_xGBYP && chip.eInstrumentPart == EInstrumentPart.BASS) || (((EChannel.Bass_Open <= nChannel && nChannel <= EChannel.Bass_Wailing) || (EChannel.Bass_xxxYx <= nChannel && nChannel <= EChannel.Bass_xxBYx) || (EChannel.Bass_xGxYx <= nChannel && nChannel <= EChannel.Bass_xxBxP) || (EChannel.Bass_xGxxP <= nChannel && nChannel <= EChannel.Bass_RGBxP) || (EChannel.Bass_xxxYP <= nChannel && nChannel <= EChannel.Bass_RGBYP)) && chip.nChannelNumber == nChannel)))
                {
                    if ((EChannel.Bass_Open <= nChannel && nChannel <= EChannel.Bass_Wailing) || (((EChannel.Bass_xxxYx <= nChannel && nChannel <= EChannel.Bass_xxBYx) || (EChannel.Bass_xGxYx <= nChannel && nChannel <= EChannel.Bass_xxBxP) || (EChannel.Bass_xGxxP <= nChannel && nChannel <= EChannel.Bass_RGBxP) || (EChannel.Bass_xxxYP <= nChannel && nChannel <= EChannel.Bass_RGBYP)) && chip.nChannelNumber == nChannel))
                    {
                        break;
                    }
                }
                // nIndex_NearestChip_Past--;
            }

            if (nIndex_NearestChip_Future >= count)
            {
                if (nIndex_NearestChip_Past < 0)	// 検索対象が過去未来どちらにも見つからなかった場合
                {
                    return null;
                }
                else 								// 検索対象が未来方向には見つからなかった(しかし過去方向には見つかった)場合
                {
                    sw2.Stop();
                    return listChip[nIndex_NearestChip_Past];
                }
            }
            else if (nIndex_NearestChip_Past < 0)	// 検索対象が過去方向には見つからなかった(しかし未来方向には見つかった)場合
            {
                sw2.Stop();
                return listChip[nIndex_NearestChip_Future];
            }
            // 検索対象が過去未来の双方に見つかったなら、より近い方を採用する
            CChip nearestChip_Future = listChip[nIndex_NearestChip_Future];
            CChip nearestChip_Past = listChip[nIndex_NearestChip_Past];
            int nDiffTime_Future = Math.Abs((int)(nTime - nearestChip_Future.nPlaybackTimeMs));
            int nDiffTime_Past = Math.Abs((int)(nTime - nearestChip_Past.nPlaybackTimeMs));
            if (nDiffTime_Future >= nDiffTime_Past)
            {
                sw2.Stop();
                return nearestChip_Past;
            }
            sw2.Stop();
            return nearestChip_Future;
        }

        //NOTE: Implementation is from AL, which is completely different from its overloaded sibling method
        protected CChip r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(long nTime, EChannel eChannel, int nInputAdjustTime, EInstrumentPart inst = EInstrumentPart.UNKNOWN) {
            return r指定時刻に一番近いChip(nTime, eChannel, nInputAdjustTime, 0, b過去優先: false, HitState.DontCare, inst);
        }

        protected void tPlaySound(CChip rChip, long n再生開始システム時刻ms, EInstrumentPart part)
        {
            this.tPlaySound(rChip, n再生開始システム時刻ms, part, CDTXMania.ConfigIni.n手動再生音量, false, false);
        }
        protected void tPlaySound(CChip rChip, long n再生開始システム時刻ms, EInstrumentPart part, int n音量)
        {
            this.tPlaySound(rChip, n再生開始システム時刻ms, part, n音量, false, false);
        }
        protected void tPlaySound(CChip rChip, long n再生開始システム時刻ms, EInstrumentPart part, int n音量, bool bモニタ)
        {
            this.tPlaySound(rChip, n再生開始システム時刻ms, part, n音量, bモニタ, false);
        }
        protected void tPlaySound(CChip pChip, long n再生開始システム時刻ms, EInstrumentPart part, int n音量, bool bモニタ, bool b音程をずらして再生)
        {
            if (pChip != null)
            {
                bool overwrite = false;
                switch (part)
                {
                    case EInstrumentPart.DRUMS:
                        #region [ DRUMS ]
                        {
                            EChannel index = pChip.nChannelNumber;
                            if ((EChannel.HiHatClose <= index) && (index <= EChannel.LeftBassDrum))
                            {
                                index -= 0x11;
                            }
                            else if ((EChannel.HiHatClose_Hidden <= index) && (index <= EChannel.LeftBassDrum_Hidden))
                            {
                                index -= EChannel.HiHatClose_Hidden;
                            }
                            // mute sound (auto)
                            // 4A: 84: HH (HO/HC)
                            // 4B: 85: CY
                            // 4C: 86: RD
                            // 4D: 87: LC
                            // 2A: 88: Gt
                            // AA: 89: Bs
                            else if (EChannel.SE24 == index)	// 仮に今だけ追加 HHは消音処理があるので overwriteフラグ系の処理は改めて不要
                            {
                                index = 0;
                            }
                            else if ((EChannel.SE25 <= index) && (index <= EChannel.SE27))	// 仮に今だけ追加
                            {
                                //            CY    RD    LC
                                EChannel[] ch = { EChannel.Cymbal, EChannel.RideCymbal, EChannel.LeftCymbal };
                                pChip.nChannelNumber = ch[pChip.nChannelNumber - EChannel.SE25];
                                index = (EChannel)(pChip.nChannelNumber - EChannel.HiHatClose);
                                overwrite = true;
                            }
                            else
                            {
                                return;
                            }

                            int nLane = this.nチャンネル0Atoレーン07[(int)index];
                            if (((((nLane == 1) && (index == 0)) && ((this.nLastPlayedHHChannelNumber != EChannel.HiHatOpen) && (this.nLastPlayedHHChannelNumber != EChannel.HiHatOpen_Hidden))) || ((((nLane == 8)) && ((index == EChannel.BMS_reserved_0A) && (this.nLastPlayedHHChannelNumber != EChannel.HiHatOpen))) && (this.nLastPlayedHHChannelNumber != EChannel.HiHatOpen_Hidden))) && CDTXMania.ConfigIni.bMutingLP)
                            {
                                for (int i = 0; i < this.LLastPlayedHHWAVNumber.Count; i++)
                                {
                                    CDTXMania.DTX.tStopPlayingWav(this.LLastPlayedHHWAVNumber[i]);
                                }
                                this.LLastPlayedHHWAVNumber.Clear();
                                this.nLastPlayedHHChannelNumber = pChip.nChannelNumber;
                            }
                            switch (index)
                            {
                                case EChannel.Nil:
                                case EChannel.BGALayer2:
                                case EChannel.Guitar_Open:
                                case EChannel.Guitar_RGBxx:
                                    if (this.LLastPlayedHHWAVNumber.Count >= 0x10)
                                    {
                                        this.LLastPlayedHHWAVNumber.RemoveAt(0);
                                    }
                                    if (!this.LLastPlayedHHWAVNumber.Contains(pChip.nIntegerValue_InternalNumber))
                                    {
                                        this.LLastPlayedHHWAVNumber.Add(pChip.nIntegerValue_InternalNumber);
                                    }
                                    break;
                            }

                            CDTXMania.DTX.tPlayChip(pChip, n再生開始システム時刻ms, nLane, n音量, bモニタ);
                            this.nLastPlayedWAVNumber[nLane] = pChip.nIntegerValue_InternalNumber;		// nLaneでなくindexにすると、LC(1A-11=09)とギター(enumで09)がかぶってLC音が消されるので注意
                            return;
                        }
                        #endregion
                    case EInstrumentPart.GUITAR:
                        #region [ GUITAR ]
                        CDTXMania.DTX.tStopPlayingWav(this.nLastPlayedWAVNumber.Guitar);
                        CDTXMania.DTX.tPlayChip(pChip, n再生開始システム時刻ms, (int)ELane.Guitar, n音量, bモニタ, b音程をずらして再生);
                        this.nLastPlayedWAVNumber.Guitar = pChip.nIntegerValue_InternalNumber;
                        return;
                        #endregion
                    case EInstrumentPart.BASS:
                        #region [ BASS ]
                        CDTXMania.DTX.tStopPlayingWav(this.nLastPlayedWAVNumber.Bass);
                        CDTXMania.DTX.tPlayChip(pChip, n再生開始システム時刻ms, (int)ELane.Bass, n音量, bモニタ, b音程をずらして再生);
                        this.nLastPlayedWAVNumber.Bass = pChip.nIntegerValue_InternalNumber;
                        return;
                    #endregion

                    default:
                        break;
                }
            }
        }
        protected void tSetStatusPanel()  // tステータスパネルの選択
        {
            if( CDTXMania.bCompactMode || CDTXMania.DTXVmode.Enabled || CDTXMania.DTX2WAVmode.Enabled)
            {
                //this.actStatusPanel.tSetDifficultyLabelFromScript( CDTXMania.stageSongSelection.rConfirmedSong.arDifficultyLabel[ CDTXMania.stageSongSelection.nConfirmedSongDifficulty ] );
            }
            else if( CDTXMania.stageSongSelection.rConfirmedSong != null )
            {
                this.actStatusPanel.tSetDifficultyLabelFromScript( CDTXMania.stageSongSelection.rConfirmedSong.arDifficultyLabel[ CDTXMania.stageSongSelection.nConfirmedSongDifficulty ] );
            }
        }
        protected EJudgement tProcessChipHit(long nHitTime, CChip pChip)  // tチップのヒット処理
        {
            return tProcessChipHit(nHitTime, pChip, true);
        }
        protected abstract EJudgement tProcessChipHit(long nHitTime, CChip pChip, bool bCorrectLane);  // tチップのヒット処理
        protected EJudgement tProcessChipHit(long nHitTime, CChip pChip, EInstrumentPart screenmode)		// EInstrumentPart screenmode
        {
            return tProcessChipHit(nHitTime, pChip, screenmode, true);
        }
        protected EJudgement tProcessChipHit(long nHitTime, CChip pChip, EInstrumentPart screenmode, bool bCorrectLane)
        {
            pChip.bHit = true;
            //Start of Long Note
            if (pChip.bロングノートである)
            {
                pChip.bロングノートHit中 = true;
                chipロングノートHit中[(int)pChip.eInstrumentPart] = pChip;
                nCurrentLongNoteDuration[(int)pChip.eInstrumentPart] = pChip.chipロングノート終端.nPlaybackTimeMs - pChip.nPlaybackTimeMs;
                nロングノートPart[(int)pChip.eInstrumentPart] = 0;
            }

            if (pChip.eInstrumentPart == EInstrumentPart.UNKNOWN)
            {
                this.bAUTOでないチップが１つでもバーを通過した = true;
            }
            bool bPChipIsAutoPlay = bCheckAutoPlay(pChip);
            //bPChipIsAutoPlay = false; // Test code only Fisyher
            pChip.bIsAutoPlayed = bPChipIsAutoPlay;			// 2011.6.10 yyagi
            EJudgement eJudgeResult = EJudgement.Auto;
            switch (pChip.eInstrumentPart)
            {
                case EInstrumentPart.DRUMS:
                    {
                        int nInputAdjustTime = bPChipIsAutoPlay ? 0 : this.nInputAdjustTimeMs.Drums;
                        eJudgeResult = (bCorrectLane) ? this.e指定時刻からChipのJUDGEを返す(nHitTime, pChip, nInputAdjustTime) : EJudgement.Miss;
                        this.actJudgeString.Start(this.nチャンネル0Atoレーン07[pChip.nChannelNumber - EChannel.HiHatClose], bPChipIsAutoPlay ? EJudgement.Auto : eJudgeResult, pChip.nLag);
                    }
                    break;

                case EInstrumentPart.GUITAR:
                    {
                        int nInputAdjustTime = bPChipIsAutoPlay ? 0 : this.nInputAdjustTimeMs.Guitar;
                        eJudgeResult = (bCorrectLane) ? this.e指定時刻からChipのJUDGEを返す(nHitTime, pChip, nInputAdjustTime) : EJudgement.Miss;
                        this.actJudgeString.Start(13, bPChipIsAutoPlay ? EJudgement.Auto : eJudgeResult, pChip.nLag);
                        break;
                    }

                case EInstrumentPart.BASS:
                    {
                        int nInputAdjustTime = bPChipIsAutoPlay ? 0 : this.nInputAdjustTimeMs.Bass;
                        eJudgeResult = (bCorrectLane) ? this.e指定時刻からChipのJUDGEを返す(nHitTime, pChip, nInputAdjustTime) : EJudgement.Miss;
                        this.actJudgeString.Start(14, bPChipIsAutoPlay ? EJudgement.Auto : eJudgeResult, pChip.nLag);
                    }
                    break;

                case EInstrumentPart.UNKNOWN:
                    {
                        if (pChip.nChannelNumber == EChannel.BonusEffect)
                        {
                            int nInputAdjustTime = bPChipIsAutoPlay ? 0 : this.nInputAdjustTimeMs.Drums;
                            eJudgeResult = (bCorrectLane) ? this.e指定時刻からChipのJUDGEを返す(nHitTime, pChip, nInputAdjustTime) : EJudgement.Miss;
                        }
                    }
                    break;
            }

            if (CDTXMania.ConfigIni.bAutoAddGage == false)
            {
                if (!bPChipIsAutoPlay && (pChip.eInstrumentPart != EInstrumentPart.UNKNOWN))
                {
                    //this.t判定にあわせてゲージを増減する( screenmode, pChip.eInstrumentPart, eJudgeResult );
                    actGauge.Damage(screenmode, pChip.eInstrumentPart, eJudgeResult);
                }
            }
            else if (CDTXMania.ConfigIni.bAutoAddGage == true)
            {
                if ((pChip.eInstrumentPart != EInstrumentPart.UNKNOWN))
                {
                    actGauge.Damage(screenmode, pChip.eInstrumentPart, eJudgeResult);
                }
            }

            //Update progressBar
            if (!bPChipIsAutoPlay && (eJudgeResult == EJudgement.Perfect || eJudgeResult == EJudgement.Great || eJudgeResult == EJudgement.Good))
            {
                this.actProgressBar.Hit(pChip.eInstrumentPart, pChip.nPlaybackTimeMs, eJudgeResult);
            }

            //Update Lag Timing Counter data
            if (!bPChipIsAutoPlay)
            {
                this.tUpdateLagTimingCounter(pChip, screenmode, eJudgeResult);
            }

            switch (pChip.eInstrumentPart)
            {
                case EInstrumentPart.DRUMS:
                    switch (eJudgeResult)
                    {
                        #region[ ヒット数の加算 ]
                        case EJudgement.Miss:
                        case EJudgement.Bad:
                            this.actCombo.tコンボリセット処理();
                            this.nHitCount_IncAuto.Drums.Miss++;
                            if (!bPChipIsAutoPlay)
                            {
                                this.nHitCount_ExclAuto.Drums.Miss++;
                                this.actPlayInfo.nMISS数++;
                            }
                            break;
                        case EJudgement.Poor:
                            this.nHitCount_IncAuto.Drums.Poor++;
                            if (!bPChipIsAutoPlay)
                            {
                                this.nHitCount_ExclAuto.Drums.Poor++;
                                this.actPlayInfo.nPOOR数++;
                            }
                            break;
                        case EJudgement.Good:
                            this.nHitCount_IncAuto.Drums.Good++;
                            if (!bPChipIsAutoPlay)
                            {
                                this.nHitCount_ExclAuto.Drums.Good++;
                                this.actPlayInfo.nGOOD数++;
                            }
                            break;
                        case EJudgement.Great:
                            this.nHitCount_IncAuto.Drums.Great++;
                            if (!bPChipIsAutoPlay)
                            {
                                this.nHitCount_ExclAuto.Drums.Great++;
                                this.actPlayInfo.nGREAT数++;
                            }
                            break;
                        case EJudgement.Perfect:
                            this.nHitCount_IncAuto.Drums.Perfect++;
                            if (!bPChipIsAutoPlay)
                            {
                                this.nHitCount_ExclAuto.Drums.Perfect++;
                                this.actPlayInfo.nPERFECT数++;
                            }
                            break;
                        case EJudgement.Auto:
                            break;
                        #endregion
                    }

                    if (CDTXMania.ConfigIni.bAllDrumsAreAutoPlay || !bPChipIsAutoPlay)
                    {
                        switch (eJudgeResult)
                        {
                            case EJudgement.Perfect:
                            case EJudgement.Great:
                            case EJudgement.Good:
                                this.actCombo.nCurrentCombo.Drums++;
                                this.actCombo.tComboAnime(EInstrumentPart.DRUMS);
                                break;

                            default:
                                this.actCombo.nCurrentCombo.Drums = 0;
                                break;
                        }
                    }

                    if( eJudgeResult == EJudgement.Great || eJudgeResult == EJudgement.Perfect || eJudgeResult == EJudgement.Auto )
                        CDTXMania.stagePerfDrumsScreen.tProcessChipHit_BonusChip( CDTXMania.ConfigIni, CDTXMania.DTX, pChip );
                    break;

                case EInstrumentPart.GUITAR:
                case EInstrumentPart.BASS:
                    int indexInst = (int)pChip.eInstrumentPart;
                    switch (eJudgeResult)
                    {
                        case EJudgement.Miss:
                        case EJudgement.Bad:
                            this.nHitCount_IncAuto[indexInst].Miss++;
                            if (!bPChipIsAutoPlay)
                            {
                                this.nHitCount_ExclAuto[indexInst].Miss++;
                            }
                            if (eJudgeResult == EJudgement.Miss)
                            {
                                pChip.bロングノートHit中 = false;
                                chipロングノートHit中[indexInst] = null;
                                nCurrentLongNoteDuration[indexInst] = 0;
                                nロングノートPart[indexInst] = 0;
                                //pChip.bMissForGhost = true;
                            }
                            break;
                        default:	// #24068 2011.1.10 ikanick changed
                            // #24167 2011.1.16 yyagi changed
                            this.nHitCount_IncAuto[indexInst][(int)eJudgeResult]++;
                            if (!bPChipIsAutoPlay)
                            {
                                this.nHitCount_ExclAuto[indexInst][(int)eJudgeResult]++;
                            }
                            break;
                    }
                    switch (eJudgeResult)
                    {
                        case EJudgement.Perfect:
                        case EJudgement.Great:
                        case EJudgement.Good:
                            this.actCombo.nCurrentCombo[indexInst]++;
                            break;

                        default:
                            this.actCombo.nCurrentCombo[indexInst] = 0;
                            break;
                    }
                    break;

                case EInstrumentPart.UNKNOWN:
                    if (pChip.nChannelNumber == EChannel.BonusEffect)
                    {
                        switch (eJudgeResult)
                        {
                            case EJudgement.Perfect:
                            case EJudgement.Great:
                                {
                                }
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
            for (int i = 0; i < 3; i++)
            {
                if (this.ctTimer[i] != null)
                {
                    if (!this.ctTimer[i].bStopped)
                    {
                        if (this.ctTimer[i].bReachedEndValue)
                        {
                            this.bブーストボーナス = false;
                            this.ctTimer[i].tStop();

                        }
                    }
                }
            }
            #region[スコア]
            //!bPChipIsAutoPlayを入れるとオート時にスコアを加算しなくなる。
            if (CDTXMania.ConfigIni.nSkillMode == 1)
            {
                int nRate = this.bブーストボーナス == true ? 2 : 1;
                if (CDTXMania.ConfigIni.bAutoAddGage == true)
                {
                    if (((pChip.eInstrumentPart == EInstrumentPart.DRUMS)) && (eJudgeResult != EJudgement.Miss) && (eJudgeResult != EJudgement.Bad))
                    {
                        #region[ドラム]
                        int nCombos = this.actCombo.nCurrentCombo.Drums;
                        float nScoreDelta = 0;
                        float nComboMax;
                        nComboMax = CDTXMania.DTX.nVisibleChipsCount.Drums;
                        if (eJudgeResult == EJudgement.Perfect)//ここでパフェ基準を作成。
                        {
                            if (nCombos < nComboMax)
                            {
                                nScoreDelta = (1000000.0f - 500.0f * CDTXMania.DTX.nボーナスチップ数) / (1275.0f + 50.0f * (nComboMax - 50f));
                            }
                            //1000000÷50÷(その曲のMAXCOMBO-24.5)
                            else if (this.nHitCount_IncAuto.Drums.Perfect >= nComboMax)
                            {
                                nScoreDelta = 1000000.0f - (float)this.actScore.nCurrentTrueScore.Drums;
                                //nScoreDelta = (1000000.0f - 500.0f * CDTXMania.DTX.nボーナスチップ数 - (1000000.0f - 500.0f * CDTXMania.DTX.nボーナスチップ数) / (1275.0f + 50.0f * (nComboMax - 50f)) * ((1275.0f + 50.0f * (nComboMax - 49.0f))));
                            }
                            //1000000-PERFECT基準値×50×(その曲のMAXCOMBO-25.5)

                        }
                        else if (eJudgeResult == EJudgement.Great)
                        {
                            nScoreDelta = ((1000000.0f - 500.0f * CDTXMania.DTX.nボーナスチップ数) / (1275.0f + 50.0f * (nComboMax - 50f))) * 0.5f;
                        }
                        else if (eJudgeResult == EJudgement.Good)
                        {
                            nScoreDelta = ((1000000.0f - 500.0f * CDTXMania.DTX.nボーナスチップ数) / (1275.0f + 50.0f * (nComboMax - 50f))) * 0.2f;
                        }

                        if (nCombos < 50)
                        {
                            nScoreDelta = nScoreDelta * nCombos;
                        }
                        else if (nCombos == nComboMax || this.nHitCount_ExclAuto.Drums.Perfect == nComboMax)
                        {
                        }
                        else
                        {
                            nScoreDelta = nScoreDelta * 50;
                        }

                        this.actScore.Add(pChip.eInstrumentPart, bIsAutoPlay, (long)nScoreDelta);
                        this.actStatusPanel.nCurrentScore += (long)nScoreDelta;
                        #endregion
                    }
                    else if (((pChip.eInstrumentPart == EInstrumentPart.GUITAR) || pChip.eInstrumentPart == EInstrumentPart.BASS) && (eJudgeResult != EJudgement.Miss) && (eJudgeResult != EJudgement.Bad))
                    {
                        #region[ ギター&ベース ]
                        int nCombos = this.actCombo.nCurrentCombo[(int)pChip.eInstrumentPart];
                        float nScoreDelta = 0;
                        float nComboMax = (pChip.eInstrumentPart == EInstrumentPart.GUITAR ? CDTXMania.DTX.nVisibleChipsCount.Guitar : CDTXMania.DTX.nVisibleChipsCount.Bass);
                        if (eJudgeResult == EJudgement.Perfect || eJudgeResult == EJudgement.Auto)//ここでパフェ基準を作成。
                        {
                            if (nCombos < nComboMax)
                            {
                                nScoreDelta = 1000000.0f / (1275.0f + 50.0f * (nComboMax - 50.0f));
                            }
                            // 100万/{1275+50×(総ノーツ数-50)}
                            else if (this.nHitCount_IncAuto[(int)pChip.eInstrumentPart].Perfect >= nComboMax)
                            {
                                nScoreDelta = 1000000.0f - (float)this.actScore.nCurrentTrueScore[(int)pChip.eInstrumentPart];
                                //Also add bonus score from Long Note
                                nScoreDelta = nScoreDelta + this.nAccumulatedLongNoteBonusScore[(int)pChip.eInstrumentPart];
                                //nScoreDelta = 1000000.0f - (1000000.0f / (1275.0f + 50.0f / (nComboMax - 50.0f))) * ((1275.0f + 50.0f * (nComboMax - 49.0f)));
                            }
                            //1000000-PERFECT基準値×50×(その曲のMAXCOMBO-25.5)

                        }
                        else if (eJudgeResult == EJudgement.Great)
                        {
                            nScoreDelta = 1000000.0f / (1275.0f + 50.0f * (nComboMax - 50.0f)) * 0.5f;
                        }
                        else if (eJudgeResult == EJudgement.Good)
                        {
                            nScoreDelta = 1000000.0f / (1275.0f + 50.0f * (nComboMax - 50.0f)) * 0.2f;
                        }


                        if (nCombos < 50)
                        {
                            nScoreDelta = nScoreDelta * nCombos;
                        }
                        else if (nCombos == nComboMax || this.nHitCount_ExclAuto[(int)pChip.eInstrumentPart].Perfect == nComboMax)
                        {
                        }
                        else
                        {
                            nScoreDelta = nScoreDelta * 50.0f;
                        }

                        this.actScore.Add(pChip.eInstrumentPart, bIsAutoPlay, (long)nScoreDelta);
                        //this.actStatusPanel.nCurrentScore += (long)nScoreDelta;
                        #endregion
                    }
                    else
                    {
                    }
                    //return eJudgeResult;
                }
                else
                {
                    if(!bPChipIsAutoPlay && pChip.eInstrumentPart == EInstrumentPart.DRUMS && eJudgeResult != EJudgement.Miss && eJudgeResult != EJudgement.Bad)
                    {
                        #region[ドラム]
                        int nCombos = this.actCombo.nCurrentCombo.Drums;
                        float nScoreDelta = 0;
                        float nComboMax;
                        nComboMax = CDTXMania.DTX.nVisibleChipsCount.Drums;
                        if (eJudgeResult == EJudgement.Perfect)//ここでパフェ基準を作成。
                        {
                            if (nCombos < nComboMax)
                            {
                                nScoreDelta = (1000000.0f - 500.0f * CDTXMania.DTX.nボーナスチップ数) / (1275.0f + 50.0f * (nComboMax - 50f));
                            }
                            //1000000÷50÷(その曲のMAXCOMBO-24.5)
                            else if (this.nHitCount_IncAuto.Drums.Perfect >= nComboMax)
                            {
                                nScoreDelta = 1000000.0f - (float)this.actScore.nCurrentTrueScore.Drums;
                            }
                            //1000000-PERFECT基準値×50×(その曲のMAXCOMBO-25.5)

                        }
                        else if (eJudgeResult == EJudgement.Great)
                        {
                            nScoreDelta = ((1000000.0f - 500.0f * CDTXMania.DTX.nボーナスチップ数) / (1275.0f + 50.0f * (nComboMax - 50.0f))) * 0.5f;
                        }
                        else if (eJudgeResult == EJudgement.Good)
                        {
                            nScoreDelta = ((1000000.0f - 500.0f * CDTXMania.DTX.nボーナスチップ数) / (1275.0f + 50.0f * (nComboMax - 50.0f))) * 0.2f;
                        }

                        if (nCombos < 50)
                        {
                            nScoreDelta = nScoreDelta * nCombos;
                        }
                        else if (nCombos == nComboMax || this.nHitCount_ExclAuto.Drums.Perfect == nComboMax)
                        {
                        }
                        else
                        {
                            nScoreDelta = nScoreDelta * 50;
                        }

                        this.actScore.Add(pChip.eInstrumentPart, bIsAutoPlay, (long)nScoreDelta);
                        this.actStatusPanel.nCurrentScore += (long)nScoreDelta;
                        #endregion
                    }
                    else if (pChip.eInstrumentPart == EInstrumentPart.GUITAR || pChip.eInstrumentPart == EInstrumentPart.BASS)
                    {
                        if(eJudgeResult != EJudgement.Miss && eJudgeResult != EJudgement.Bad)
                        {
                            #region[ ギター&ベース ]
                            int nCombos = this.actCombo.nCurrentCombo[(int)pChip.eInstrumentPart];
                            float nScoreDelta = 0;
                            float nComboMax = (pChip.eInstrumentPart == EInstrumentPart.GUITAR ? CDTXMania.DTX.nVisibleChipsCount.Guitar : CDTXMania.DTX.nVisibleChipsCount.Bass);
                            if (eJudgeResult == EJudgement.Perfect || eJudgeResult == EJudgement.Auto)//ここでパフェ基準を作成。
                            {
                                if (nCombos < nComboMax)
                                {
                                    nScoreDelta = 1000000.0f / (1275.0f + 50.0f * (nComboMax - 50.0f));
                                }
                                // 100万/{1275+50×(総ノーツ数-50)}
                                else if (this.nHitCount_IncAuto[(int)pChip.eInstrumentPart].Perfect >= nComboMax)
                                {
                                    nScoreDelta = 1000000.0f - (float)this.actScore.nCurrentTrueScore[(int)pChip.eInstrumentPart];
                                    //Also add bonus score from Long Note
                                    nScoreDelta = nScoreDelta + this.nAccumulatedLongNoteBonusScore[(int)pChip.eInstrumentPart];
                                    //nScoreDelta = 1000000.0f - (1000000.0f / (1275.0f + 50.0f / (nComboMax - 50.0f))) * ((1275.0f + 50.0f * (nComboMax - 49.0f)));
                                }
                                //1000000-PERFECT基準値×50×(その曲のMAXCOMBO-25.5)

                            }
                            else if (eJudgeResult == EJudgement.Great)
                            {
                                nScoreDelta = 1000000.0f / (1275.0f + 50.0f * (nComboMax - 50.0f)) * 0.5f;
                            }
                            else if (eJudgeResult == EJudgement.Good)
                            {
                                nScoreDelta = 1000000.0f / (1275.0f + 50.0f * (nComboMax - 50.0f)) * 0.2f;
                            }


                            if (nCombos < 50)
                            {
                                nScoreDelta = nScoreDelta * nCombos;
                            }
                            else if (nCombos == nComboMax || this.nHitCount_ExclAuto[(int)pChip.eInstrumentPart].Perfect == nComboMax)
                            {

                            }
                            else
                            {
                                nScoreDelta = nScoreDelta * 50.0f;
                            }

                            this.actScore.Add(pChip.eInstrumentPart, bIsAutoPlay, (long)nScoreDelta);
                            //this.actStatusPanel.nCurrentScore += (long)nScoreDelta;
                            #endregion
                        }
                    }
                                        
                }
            }
            else if (CDTXMania.ConfigIni.nSkillMode == 0)
            {
                if (CDTXMania.ConfigIni.bAutoAddGage)
                {
                    if (((pChip.eInstrumentPart != EInstrumentPart.UNKNOWN)) && (eJudgeResult != EJudgement.Miss) && (eJudgeResult != EJudgement.Bad))
                    {
                        int nCombos = this.actCombo.nCurrentCombo[(int)pChip.eInstrumentPart];
                        float nScoreDelta = 0;
                        long[] nComboScoreDelta = new long[] { 350L, 200L, 50L, 0L };
                        if ((nCombos <= 500) || (eJudgeResult == EJudgement.Good))
                        {
                            nScoreDelta = nComboScoreDelta[(int)eJudgeResult] * nCombos;
                        }
                        else if ((eJudgeResult == EJudgement.Perfect))
                        {
                            nScoreDelta = nComboScoreDelta[(int)eJudgeResult] * 500L;
                        }
                        this.actScore.Add(pChip.eInstrumentPart, bIsAutoPlay, (long)nScoreDelta);
                        this.actStatusPanel.nCurrentScore += (long)nScoreDelta;
                    }
                }
                else
                {
                    if ((!bPChipIsAutoPlay && (pChip.eInstrumentPart != EInstrumentPart.UNKNOWN)) && (eJudgeResult != EJudgement.Miss) && (eJudgeResult != EJudgement.Bad))
                    {
                        int nCombos = this.actCombo.nCurrentCombo[(int)pChip.eInstrumentPart];
                        float nScoreDelta = 0;
                        long[] nComboScoreDelta = new long[] { 350L, 200L, 50L, 0L };
                        if ((nCombos <= 500) || (eJudgeResult == EJudgement.Good))
                        {
                            nScoreDelta = nComboScoreDelta[(int)eJudgeResult] * nCombos;
                        }
                        else if ((eJudgeResult == EJudgement.Perfect))
                        {
                            nScoreDelta = nComboScoreDelta[(int)eJudgeResult] * 500L;
                        }
                        this.actScore.Add(pChip.eInstrumentPart, bIsAutoPlay, (long)nScoreDelta);
                        this.actStatusPanel.nCurrentScore += (long)nScoreDelta;
                    }
                }
            }
            #endregion

            return eJudgeResult;
        }

        private void tUpdateLagTimingCounter(CChip pChip, EInstrumentPart screenmode, EJudgement eJudgeResult) 
        {
            if (pChip.eInstrumentPart == EInstrumentPart.UNKNOWN)
            {
                return;
            }

            if (pChip.eInstrumentPart == EInstrumentPart.DRUMS && screenmode != EInstrumentPart.DRUMS) 
            {
                return;
            }

            if ((pChip.eInstrumentPart == EInstrumentPart.GUITAR ||
                pChip.eInstrumentPart == EInstrumentPart.BASS) &&
                screenmode != EInstrumentPart.GUITAR
                )
            {
                return;
            }

            switch (eJudgeResult)
            {
                case EJudgement.Miss:
                case EJudgement.Bad:
                case EJudgement.Poor:
                case EJudgement.Good:
                case EJudgement.Great:
                case EJudgement.Perfect:
                    if (pChip.nLag > 0)
                    {
                        this.nTimingHitCount[(int)pChip.eInstrumentPart].nLate++;
                    }
                    else
                    {
                        this.nTimingHitCount[(int)pChip.eInstrumentPart].nEarly++;
                    }
                    break;
            }
        }
        
        protected abstract void tチップのヒット処理_BadならびにTight時のMiss(EInstrumentPart part);
        protected abstract void tチップのヒット処理_BadならびにTight時のMiss(EInstrumentPart part, int nLane);
        protected void tチップのヒット処理_BadならびにTight時のMiss(EInstrumentPart part, EInstrumentPart screenmode)
        {
            this.tチップのヒット処理_BadならびにTight時のMiss(part, 0, screenmode);
        }
        protected void tチップのヒット処理_BadならびにTight時のMiss(EInstrumentPart part, int nLane, EInstrumentPart screenmode)
        {
            this.bAUTOでないチップが１つでもバーを通過した = true;
            //this.t判定にあわせてゲージを増減する( screenmode, part, EJudgement.Miss );
            actGauge.Damage(screenmode, part, EJudgement.Miss);
            switch (part)
            {
                case EInstrumentPart.DRUMS:
                    if ((nLane >= 0) && (nLane <= 10))
                    {
                        this.actJudgeString.Start(nLane, bIsAutoPlay[nLane] ? EJudgement.Auto : EJudgement.Miss, 999);
                    }
                    this.actCombo.nCurrentCombo.Drums = 0;
                    return;

                case EInstrumentPart.GUITAR:
                    this.actJudgeString.Start(13, EJudgement.Bad, 999);
                    this.actCombo.nCurrentCombo.Guitar = 0;
                    return;

                case EInstrumentPart.BASS:
                    this.actJudgeString.Start(14, EJudgement.Bad, 999);
                    this.actCombo.nCurrentCombo.Bass = 0;
                    break;

                default:
                    return;
            }
        }

        //From AL
        protected CChip r指定時刻に一番近いChip(long nTime, EChannel search, int nInputAdjustTime, int n検索範囲時間ms = 0, bool b過去優先 = true, HitState hs = HitState.NotHit, EInstrumentPart inst = EInstrumentPart.UNKNOWN)
        {
            CChip cChip = null;
            nTime += nInputAdjustTime;
            if (this.nCurrentTopChip >= 0 && this.nCurrentTopChip <= this.listChip.Count)
            {
                int num = -1;
                int num2 = -1;
                int num3 = this.listChip.Count - 1;
                for (int i = this.nCurrentTopChip; i < this.listChip.Count; i++)
                {
                    CChip chip2 = this.listChip[i];
                    if (Future(chip2) && OutOfRange(chip2))
                    {
                        break;
                    }
                    if (Found(chip2, Future))
                    {
                        num = i;
                        num3 = i;
                        break;
                    }
                }
                for (int num4 = num3; num4 >= 0; num4--)
                {
                    CChip chip3 = this.listChip[num4];
                    if (Past(chip3) && OutOfRange(chip3))
                    {
                        break;
                    }
                    if (Found(chip3, Past))
                    {
                        num2 = num4;
                        break;
                    }
                }
                if (num >= 0 || num2 >= 0)
                {
                    if (num2 >= 0)
                    {
                        cChip = this.listChip[num2];
                    }
                    else if (num >= 0)
                    {
                        cChip = this.listChip[num];
                    }
                }
                if (!b過去優先 && num >= 0 && num2 >= 0)
                {
                    long num5 = Math.Abs(nTime - this.listChip[num].nPlaybackTimeMs);
                    long num6 = Math.Abs(nTime - this.listChip[num2].nPlaybackTimeMs);
                    cChip = ((num5 >= num6) ? this.listChip[num2] : this.listChip[num]);
                }
                if (cChip != null && OutOfRange(cChip))
                {
                    cChip = null;
                }
            }
            return cChip;
            bool Found(CChip chip, Func<CChip, bool> futureOrPast)
            {
                if (futureOrPast(chip) && ((hs == HitState.NotHit && !chip.bHit) || (hs == HitState.Hit && chip.bHit) || hs == HitState.DontCare) && !chip.bIsEmptyChip)
                {
                    if (((search != chip.nChannelNumber && search + 32 != chip.nChannelNumber) || !chip.bDrums可視チップ) && (search != chip.nChannelNumber || !chip.bGuitar可視チップ_Wailing含む) && (search != chip.nChannelNumber || !chip.bBass可視チップ_Wailing含む) && (inst != EInstrumentPart.GUITAR || !chip.bGuitar可視チップ))
                    {
                        if (inst == EInstrumentPart.BASS)
                        {
                            return chip.bBass可視チップ;
                        }
                        return false;
                    }
                    return true;
                }
                return false;
            }
            bool Future(CChip chip)
            {
                return chip.nPlaybackTimeMs > nTime;
            }
            bool OutOfRange(CChip chip)
            {
                if (n検索範囲時間ms > 0)
                {
                    return Math.Abs(nTime - chip.nPlaybackTimeMs) > n検索範囲時間ms;
                }
                return false;
            }
            bool Past(CChip chip)
            {
                return chip.nPlaybackTimeMs <= nTime;
            }
        }

        protected CChip r指定時刻に一番近い未ヒットChip(long nTime, int nChannelFlag, int nInputAdjustTime, bool enableTrace = false)
        {
            return this.r指定時刻に一番近い未ヒットChip(nTime, nChannelFlag, nInputAdjustTime, 0, enableTrace);
        }
               
        //This method occassionally returns non-hittable chip! for Guitar and Bass
        protected CChip r指定時刻に一番近い未ヒットChip(long nTime, int int_nChannel, int nInputAdjustTime, int n検索範囲時間ms, bool enableTrace = false)
        {
            if (enableTrace) 
            {
                Trace.TraceInformation("r指定時刻に一番近い未ヒットChip arg: int_nChannel={0:x2}", int_nChannel);
            }

            EChannel nChannel = (EChannel)int_nChannel;
            sw2.Start();
            //Trace.TraceInformation( "nTime={0}, nChannel={1:x2}, 現在のTop={2}", nTime, nChannel,CDTXMania.DTX.listChip[ this.nCurrentTopChip ].nPlaybackTimeMs );
            nTime += nInputAdjustTime;

            int nIndex_InitialPositionSearchingToPast;
            int nTimeDiff;
            if (this.nCurrentTopChip == -1)			// 演奏データとして1個もチップがない場合は
            {
                sw2.Stop();
                return null;
            }
            int count = listChip.Count;
            int nIndex_NearestChip_Future = nIndex_InitialPositionSearchingToPast = this.nCurrentTopChip;
            if (this.nCurrentTopChip >= count)		// その時点で演奏すべきチップが既に全部無くなっていたら
            {
                nIndex_NearestChip_Future = nIndex_InitialPositionSearchingToPast = count - 1;
            }
            // int nIndex_NearestChip_Future = nIndex_InitialPositionSearchingToFuture;
            //			while ( nIndex_NearestChip_Future < count )	// 未来方向への検索
            for (; nIndex_NearestChip_Future < count; nIndex_NearestChip_Future++)
            {
                CChip chip = listChip[nIndex_NearestChip_Future];
                if (!chip.bHit)
                {
                    if ((EChannel.HiHatClose <= nChannel) && (nChannel <= EChannel.LeftBassDrum))
                    {
                        if ((chip.nChannelNumber == nChannel) || (chip.nChannelNumber == ((int)nChannel + EChannel.Guitar_Open)))
                        {
                            if (chip.nPlaybackTimeMs > nTime)
                            {
                                break;
                            }
                            nIndex_InitialPositionSearchingToPast = nIndex_NearestChip_Future;
                        }
                        continue;
                    }
                    else if ((nChannel == EChannel.Guitar_WailingSound && chip.eInstrumentPart == EInstrumentPart.GUITAR) || 
                        (((EChannel.Guitar_Open <= nChannel && nChannel <= EChannel.Guitar_Wailing) || (EChannel.Guitar_xxxYx <= nChannel && nChannel <= EChannel.Guitar_RxxxP) || (EChannel.Guitar_RxBxP <= nChannel && nChannel <= EChannel.Guitar_xGBYP) || (EChannel.Guitar_RxxYP <= nChannel && nChannel <= EChannel.Guitar_RGBYP)) && chip.nChannelNumber == nChannel))
                    {
                        if (chip.nPlaybackTimeMs > nTime)
                        {
                            break;
                        }
                        nIndex_InitialPositionSearchingToPast = nIndex_NearestChip_Future;
                    }
                    else if ((nChannel == EChannel.Guitar_xGBYP && chip.eInstrumentPart == EInstrumentPart.BASS) || (((nChannel >= EChannel.Bass_Open && nChannel <= EChannel.Bass_Wailing) || (EChannel.Bass_xxxYx == nChannel) || (nChannel == EChannel.Bass_xxBYx) || (nChannel >= EChannel.Bass_xGxYx && nChannel <= EChannel.Bass_xxBxP) || (nChannel >= EChannel.Bass_xGxxP && nChannel <= EChannel.Bass_RGBxP) || (nChannel >= EChannel.Bass_xxxYP && nChannel <= EChannel.Bass_RGBYP)) && chip.nChannelNumber == nChannel))
                    {
                        EChannel currChipChannel = chip.nChannelNumber;
                        if ((currChipChannel >= EChannel.Bass_Open && currChipChannel <= EChannel.Bass_Wailing) || (EChannel.Bass_xxxYx == currChipChannel) || (currChipChannel == EChannel.Bass_xxBYx) || (currChipChannel >= EChannel.Bass_xGxYx && currChipChannel <= EChannel.Bass_xxBxP) || (currChipChannel >= EChannel.Bass_xGxxP && currChipChannel <= EChannel.Bass_RGBxP) || (currChipChannel >= EChannel.Bass_xxxYP && currChipChannel <= EChannel.Bass_RGBYP)) {
                            if (enableTrace)
                            {
                                Trace.TraceInformation("r指定時刻に一番近い未ヒットChip 1st condition met: chip_channel={0:x2}, nIndexPast={1}, nIndexNearestChipFuture={2}", (int)chip.nChannelNumber, nIndex_InitialPositionSearchingToPast, nIndex_NearestChip_Future);
                            }

                            if (chip.nPlaybackTimeMs > nTime)
                            {
                                break;
                            }
                            nIndex_InitialPositionSearchingToPast = nIndex_NearestChip_Future;
                        }
                    }
                }
                //				nIndex_NearestChip_Future++;
            }
            int nIndex_NearestChip_Past = nIndex_InitialPositionSearchingToPast;
            //			while ( nIndex_NearestChip_Past >= 0 )		// 過去方向への検索
            for (; nIndex_NearestChip_Past >= 0; nIndex_NearestChip_Past--)
            {
                CChip chip = listChip[nIndex_NearestChip_Past];
                if ((!chip.bHit) &&
                        (
                            ((nChannel >= EChannel.HiHatClose) && (nChannel <= EChannel.LeftBassDrum) &&
                                ((chip.nChannelNumber == nChannel) || (chip.nChannelNumber == ((int)nChannel + EChannel.Guitar_Open)))
                            )
                            ||
                            (
                                ((nChannel == EChannel.Guitar_WailingSound) && (chip.eInstrumentPart == EInstrumentPart.GUITAR)) ||
                                (((nChannel >= EChannel.Guitar_Open) && (nChannel <= EChannel.Guitar_Wailing)) ||
                                (EChannel.Guitar_xxxYx <= nChannel && nChannel <= EChannel.Guitar_RxxxP) ||
                                (EChannel.Guitar_RxBxP <= nChannel && nChannel <= EChannel.Guitar_xGBYP) ||
                                (EChannel.Guitar_RxxYP <= nChannel && nChannel <= EChannel.Guitar_RGBYP)
                                && (chip.nChannelNumber == nChannel))
                            )
                            ||
                            (
                                ((nChannel == EChannel.Guitar_xGBYP) && (chip.eInstrumentPart == EInstrumentPart.BASS)) ||
                                (((nChannel >= EChannel.Bass_Open) && (nChannel <= EChannel.Bass_Wailing)) ||
                                (EChannel.Bass_xxxYx <= nChannel && nChannel <= EChannel.Bass_xxBYx) ||
                                (EChannel.Bass_xGxYx <= nChannel && nChannel <= EChannel.Bass_xxBxP) ||
                                (EChannel.Bass_xGxxP <= nChannel && nChannel <= EChannel.Bass_RGBxP) ||
                                (EChannel.Bass_xxxYP <= nChannel && nChannel <= EChannel.Bass_RGBYP)
                                && (chip.nChannelNumber == nChannel))
                            )
                        )
                    )
                {
                    if (chip.eInstrumentPart == EInstrumentPart.BASS)
                    {
                        EChannel currChipChannel = chip.nChannelNumber;
                        

                        if (((currChipChannel >= EChannel.Bass_Open) && (currChipChannel <= EChannel.Bass_Wailing)) ||
                                (EChannel.Bass_xxxYx <= currChipChannel && currChipChannel <= EChannel.Bass_xxBYx) ||
                                (EChannel.Bass_xGxYx <= currChipChannel && currChipChannel <= EChannel.Bass_xxBxP) ||
                                (EChannel.Bass_xGxxP <= currChipChannel && currChipChannel <= EChannel.Bass_RGBxP) ||
                                (EChannel.Bass_xxxYP <= currChipChannel && currChipChannel <= EChannel.Bass_RGBYP)) 
                        {
                            if (enableTrace)
                            {
                                Trace.TraceInformation("r指定時刻に一番近い未ヒットChip 2nd condition met: chip_channel={0:x2}", (int)chip.nChannelNumber);
                            }
                            break;                        
                        }

                        //break;
                    }
                    else 
                    {
                        break;
                    }
                    
                }
                //				nIndex_NearestChip_Past--;
            }
            if ((nIndex_NearestChip_Future >= count) && (nIndex_NearestChip_Past < 0))	// 検索対象が過去未来どちらにも見つからなかった場合
            {
                sw2.Stop();
                return null;
            }
            CChip nearestChip;	// = null;	// 以下のifブロックのいずれかで必ずnearestChipには非nullが代入されるので、null初期化を削除
            if (nIndex_NearestChip_Future >= count)											// 検索対象が未来方向には見つからなかった(しかし過去方向には見つかった)場合
            {
                if (enableTrace)
                {
                    Trace.TraceInformation("r指定時刻に一番近い未ヒットChip Nearest Chip Assign 1: nIndex_NearestChip_Past={0}", nIndex_NearestChip_Past);
                }
                nearestChip = listChip[nIndex_NearestChip_Past];
                //				nTimeDiff = Math.Abs( (int) ( nTime - nearestChip.nPlaybackTimeMs ) );
            }
            else if (nIndex_NearestChip_Past < 0)												// 検索対象が過去方向には見つからなかった(しかし未来方向には見つかった)場合
            {
                if (enableTrace)
                {
                    Trace.TraceInformation("r指定時刻に一番近い未ヒットChip Nearest Chip Assign 2: nIndex_NearestChip_Future={0}", nIndex_NearestChip_Future);
                }
                nearestChip = listChip[nIndex_NearestChip_Future];
                //				nTimeDiff = Math.Abs( (int) ( nTime - nearestChip.nPlaybackTimeMs ) );
            }
            else
            {
                int nTimeDiff_Future = Math.Abs((int)(nTime - listChip[nIndex_NearestChip_Future].nPlaybackTimeMs));
                int nTimeDiff_Past = Math.Abs((int)(nTime - listChip[nIndex_NearestChip_Past].nPlaybackTimeMs));
                if (nTimeDiff_Future < nTimeDiff_Past)
                {
                    if (enableTrace)
                    {
                        Trace.TraceInformation("r指定時刻に一番近い未ヒットChip Nearest Chip Assign 3: nIndex_NearestChip_Future={0}", nIndex_NearestChip_Future);
                    }
                    nearestChip = listChip[nIndex_NearestChip_Future];
                    //					nTimeDiff = Math.Abs( (int) ( nTime - nearestChip.nPlaybackTimeMs ) );
                }
                else
                {
                    if (enableTrace)
                    {
                        Trace.TraceInformation("r指定時刻に一番近い未ヒットChip Nearest Chip Assign 4: nIndex_NearestChip_Past={0}", nIndex_NearestChip_Past);
                    }
                    nearestChip = listChip[nIndex_NearestChip_Past];
                    //					nTimeDiff = Math.Abs( (int) ( nTime - nearestChip.nPlaybackTimeMs ) );
                }
            }
            nTimeDiff = Math.Abs((int)(nTime - nearestChip.nPlaybackTimeMs));
            if ((n検索範囲時間ms > 0) && (nTimeDiff > n検索範囲時間ms))					// チップは見つかったが、検索範囲時間外だった場合
            {
                sw2.Stop();
                return null;
            }
            sw2.Stop();

            if (enableTrace)                
            {
                if (!nearestChip.bChannelWithVisibleChip && nearestChip.nChannelNumber != EChannel.Guitar_Wailing && nearestChip.nChannelNumber != EChannel.Bass_Wailing)
                {
                    Trace.TraceWarning("Nearest chip is not a visible chip!!! ch={0:x2}", (int)nearestChip.nChannelNumber);
                    Trace.TraceWarning("r指定時刻に一番近い未ヒットChip arguments: nTime (adjusted):{0}, int_nChannel:{1:x2}, nInputAdjustTime:{2}, n検索範囲時間ms:{3}", nTime, int_nChannel, nInputAdjustTime, n検索範囲時間ms);
                }
            }

            return nearestChip;
        }

        protected CChip r次に来る指定楽器Chipを更新して返す(EInstrumentPart inst)
        {
            switch ((int)inst)
            {
                case (int)EInstrumentPart.GUITAR:
                    return r次にくるギターChipを更新して返す();
                case (int)EInstrumentPart.BASS:
                    return r次にくるベースChipを更新して返す();
                default:
                    return null;
            }
        }
        protected CChip r次にくるギターChipを更新して返す()
        {
            int nInputAdjustTime = this.bIsAutoPlay.GtPick ? 0 : this.nInputAdjustTimeMs.Guitar;
            //this.rNextGuitarChip = this.r指定時刻に一番近い未ヒットChip(CSoundManager.rcPerformanceTimer.nCurrentTime, (int)EChannel.Guitar_WailingSound, nInputAdjustTime, 500);
            this.rNextGuitarChip = r指定時刻に一番近いChip(CSoundManager.rcPerformanceTimer.nCurrentTime, EChannel.Guitar_Open, nInputAdjustTime, 800, b過去優先: true, HitState.NotHit, EInstrumentPart.GUITAR);
            return this.rNextGuitarChip;
        }
        protected CChip r次にくるベースChipを更新して返す()  // r次にくるベースChipを更新して返す
        {
            int nInputAdjustTime = this.bIsAutoPlay.BsPick ? 0 : this.nInputAdjustTimeMs.Bass;//Guitar_xGBYP
            //this.rNextBassChip = this.r指定時刻に一番近い未ヒットChip(CSoundManager.rcPerformanceTimer.nCurrentTime, (int)EChannel.Guitar_xGBYP, nInputAdjustTime, 500);
            this.rNextBassChip = r指定時刻に一番近いChip(CSoundManager.rcPerformanceTimer.nCurrentTime, EChannel.Bass_Open, nInputAdjustTime, 800, b過去優先: true, HitState.NotHit, EInstrumentPart.BASS);
            return this.rNextBassChip;
        }

        protected void ChangeInputAdjustTimeInPlaying(IInputDevice keyboard, int plusminus)		// #23580 2011.1.16 yyagi UI for InputAdjustTime in playing screen.
        {
            int part, offset = plusminus;
            if (keyboard.bKeyPressing((int)SlimDXKey.LeftShift) || keyboard.bKeyPressing((int)SlimDXKey.RightShift))	// Guitar InputAdjustTime
            {
                part = (int)EInstrumentPart.GUITAR;
            }
            else if (keyboard.bKeyPressing((int)SlimDXKey.LeftAlt) || keyboard.bKeyPressing((int)SlimDXKey.RightAlt))	// Bass InputAdjustTime
            {
                part = (int)EInstrumentPart.BASS;
            }
            else	// Drums InputAdjustTime
            {
                part = (int)EInstrumentPart.DRUMS;
            }
            if (!keyboard.bKeyPressing((int)SlimDXKey.LeftControl) && !keyboard.bKeyPressing((int)SlimDXKey.RightControl))
            {
                offset *= 10;
            }

            this.nInputAdjustTimeMs[part] += offset;
            if (this.nInputAdjustTimeMs[part] > 99)
            {
                this.nInputAdjustTimeMs[part] = 99;
            }
            else if (this.nInputAdjustTimeMs[part] < -99)
            {
                this.nInputAdjustTimeMs[part] = -99;
            }
            CDTXMania.ConfigIni.nInputAdjustTimeMs[part] = this.nInputAdjustTimeMs[part];
        }

        protected abstract void tHandleInput_Drums();
        protected abstract void ScrollSpeedUp();
        protected abstract void ScrollSpeedDown();
        protected void tHandleKeyInput()
        {
            IInputDevice keyboard = CDTXMania.InputManager.Keyboard;
            if (CDTXMania.Pad.bPressed(EInstrumentPart.BASS, EPad.Help))
            {	// shift+f1 (pause)
                this.bPAUSE = !this.bPAUSE;
                if (this.bPAUSE)
                {
                    CSoundManager.rcPerformanceTimer.tPause();
                    CDTXMania.Timer.tPause();
                    CDTXMania.DTX.tPausePlaybackForAllChips();
                }
                else
                {
                    CSoundManager.rcPerformanceTimer.tResume();
                    CDTXMania.Timer.tResume();
                    CDTXMania.DTX.tResumePlaybackForAllChips();
                }
            }
            if (((base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED)) && (base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED_フェードアウト))
            {
                if (!this.bPAUSE)
                {
                    this.tHandleInput_Drums();
                    this.tHandleInput_GuitarBass(EInstrumentPart.GUITAR);
                    this.tHandleInput_GuitarBass(EInstrumentPart.BASS);
                }
                if (!this.bPAUSE && keyboard.bKeyPressed((int)SlimDXKey.UpArrow) && (keyboard.bKeyPressing((int)SlimDXKey.RightShift) || keyboard.bKeyPressing((int)SlimDXKey.LeftShift)))
                {	// shift (+ctrl) + UpArrow (BGMAdjust)
                    CDTXMania.DTX.t各自動再生音チップの再生時刻を変更する((keyboard.bKeyPressing((int)SlimDXKey.LeftControl) || keyboard.bKeyPressing((int)SlimDXKey.RightControl)) ? 1 : 10);
                    CDTXMania.DTX.tAutoCorrectWavPlaybackPosition();
                }
                else if (!this.bPAUSE && keyboard.bKeyPressed((int)SlimDXKey.DownArrow) && (keyboard.bKeyPressing((int)SlimDXKey.RightShift) || keyboard.bKeyPressing((int)SlimDXKey.LeftShift)))
                {	// shift + DownArrow (BGMAdjust)
                    CDTXMania.DTX.t各自動再生音チップの再生時刻を変更する((keyboard.bKeyPressing((int)SlimDXKey.LeftControl) || keyboard.bKeyPressing((int)SlimDXKey.RightControl)) ? -1 : -10);
                    CDTXMania.DTX.tAutoCorrectWavPlaybackPosition();
                }
                else if (keyboard.bKeyPressed((int)SlimDXKey.UpArrow))
                {	// UpArrow(scrollspeed up)
                    ScrollSpeedUp();
                }
                else if (keyboard.bKeyPressed((int)SlimDXKey.DownArrow))
                {	// DownArrow (scrollspeed down)
                    ScrollSpeedDown();
                }

                else if (CDTXMania.Pad.bPressed(EInstrumentPart.GUITAR, EPad.Help))
                {	// del (debug info)
                    CDTXMania.ConfigIni.b演奏情報を表示する = !CDTXMania.ConfigIni.b演奏情報を表示する;
                }
                else if (!this.bPAUSE && keyboard.bKeyPressed((int)SlimDXKey.LeftArrow))		// #24243 2011.1.16 yyagi UI for InputAdjustTime in playing screen.
                {
                    ChangeInputAdjustTimeInPlaying(keyboard, -1);
                }
                else if (!this.bPAUSE && keyboard.bKeyPressed((int)SlimDXKey.RightArrow))		// #24243 2011.1.16 yyagi UI for InputAdjustTime in playing screen.
                {
                    ChangeInputAdjustTimeInPlaying(keyboard, +1);
                }
                else if (!this.bPAUSE && (base.ePhaseID == CStage.EPhase.Common_DefaultState) && !CDTXMania.DTXVmode.Enabled && (keyboard.bKeyPressed((int)SlimDXKey.Escape)))
                {	// escape (exit)
                    this.actFO.tStartFadeOut();
                    base.ePhaseID = CStage.EPhase.Common_FadeOut;
                    this.eReturnValueAfterFadeOut = EPerfScreenReturnValue.Interruption;
                }
                else if (CDTXMania.Pad.bPressed(EKeyConfigPart.SYSTEM, EKeyConfigPad.Restart))
                {
                    if (this.bPAUSE)
                    {
                        CSoundManager.rcPerformanceTimer.tResume();
                        CDTXMania.Timer.tResume();
                    }
                    base.ePhaseID = CStage.EPhase.演奏_STAGE_RESTART;
                    this.eReturnValueAfterFadeOut = EPerfScreenReturnValue.Restart;
                }
                else if (CDTXMania.Pad.bPressed(EKeyConfigPart.SYSTEM, EKeyConfigPad.SkipForward))
                {
                    this.bIsTrainingMode = true;
                    Trace.TraceInformation("SKIP FORWARD CSoundManager.rcPerformanceTimer.nCurrentTime=" + CSoundManager.rcPerformanceTimer.nCurrentTime + ", CDTXMania.Timer.nCurrentTime=" + CDTXMania.Timer.nCurrentTime);
                    this.tJumpInSong(CSoundManager.rcPerformanceTimer.nCurrentTime + CDTXMania.ConfigIni.nSkipTimeMs);
                }
                else if (CDTXMania.Pad.bPressed(EKeyConfigPart.SYSTEM, EKeyConfigPad.SkipBackward))
                {
                    this.bIsTrainingMode = true;
                    Trace.TraceInformation("SKIP BACKWARD CSoundManager.rcPerformanceTimer.nCurrentTime=" + CSoundManager.rcPerformanceTimer.nCurrentTime + ", CDTXMania.Timer.nCurrentTime=" + CDTXMania.Timer.nCurrentTime);
                    this.tJumpInSong(Math.Max(0, CSoundManager.rcPerformanceTimer.nCurrentTime - CDTXMania.ConfigIni.nSkipTimeMs));
                }
                else if (CDTXMania.Pad.bPressed(EKeyConfigPart.SYSTEM, EKeyConfigPad.LoopCreate))
                {
                    this.bIsTrainingMode = true;
                    if (this.LoopBeginMs == -1)
                    {
                        Trace.TraceInformation("INSERT LOOP BEGIN CSoundManager.rcPerformanceTimer.nCurrentTime=" + CSoundManager.rcPerformanceTimer.nCurrentTime + ", CDTXMania.Timer.nCurrentTime=" + CDTXMania.Timer.nCurrentTime);
                        this.LoopBeginMs = CSoundManager.rcPerformanceTimer.nCurrentTime;
                    }
                    else
                    {
                        if (this.LoopEndMs == -1)
                        {
                            if (this.LoopBeginMs < CSoundManager.rcPerformanceTimer.nCurrentTime)
                            {
                                Trace.TraceInformation("INSERT LOOP END CSoundManager.rcPerformanceTimer.nCurrentTime=" + CSoundManager.rcPerformanceTimer.nCurrentTime + ", CDTXMania.Timer.nCurrentTime=" + CDTXMania.Timer.nCurrentTime);
                                this.LoopEndMs = CSoundManager.rcPerformanceTimer.nCurrentTime;
                            }
                            else
                            {
                                Trace.TraceInformation("INSERT LOOP BEGIN AND SWAP CSoundManager.rcPerformanceTimer.nCurrentTime=" + CSoundManager.rcPerformanceTimer.nCurrentTime + ", CDTXMania.Timer.nCurrentTime=" + CDTXMania.Timer.nCurrentTime);
                                this.LoopEndMs = this.LoopBeginMs;
                                this.LoopBeginMs = CSoundManager.rcPerformanceTimer.nCurrentTime;
                            }
                        }
                        //Else loop already set, do nothing
                    }
                }
                else if (CDTXMania.Pad.bPressed(EKeyConfigPart.SYSTEM, EKeyConfigPad.LoopDelete))
                {
                    Trace.TraceInformation("REMOVE LOOP CSoundManager.rcPerformanceTimer.nCurrentTime=" + CSoundManager.rcPerformanceTimer.nCurrentTime + ", CDTXMania.Timer.nCurrentTime=" + CDTXMania.Timer.nCurrentTime);
                    this.LoopBeginMs = -1;
                    this.LoopEndMs = -1;
                }
                else if (CDTXMania.Pad.bPressed(EKeyConfigPart.SYSTEM, EKeyConfigPad.DecreasePlaySpeed))
                {
                    // Decrease Play Speed
                    if (CDTXMania.ConfigIni.nPlaySpeed > CConstants.PLAYSPEED_MIN)
                    {
                        this.bIsTrainingMode = true;
                        this.tChangePlaySpeed(-1);
                    }
                }
                else if (CDTXMania.Pad.bPressed(EKeyConfigPart.SYSTEM, EKeyConfigPad.IncreasePlaySpeed))
                {
                    // Increase Play Speed
                    if (CDTXMania.ConfigIni.nPlaySpeed < CConstants.PLAYSPEED_MAX)
                    {
                        this.bIsTrainingMode = true;
                        this.tChangePlaySpeed(1);
                    }
                }

                if (!CDTXMania.ConfigIni.bReverse.Drums && keyboard.bKeyPressing((int)SlimDXKey.PageUp))
                {
                    if (!this.sw.IsRunning)
                    {
                        this.sw2 = Stopwatch.StartNew();
                        this.sw = Stopwatch.StartNew();
                        if (this.nJudgeLinePosY.Drums > nJudgeLineMinPosY)
                        {
                            this.nJudgeLinePosY.Drums--;
                        }
                    }
                    else
                    {
                        if (this.sw.ElapsedMilliseconds > 10L)
                        {
                            if (this.sw2.IsRunning)
                            {
                                if (this.sw2.ElapsedMilliseconds > 100L)
                                {
                                    this.sw2.Reset();
                                }
                            }
                            else if (this.nJudgeLinePosY.Drums > nJudgeLineMinPosY)
                            {
                                this.nJudgeLinePosY.Drums--;
                            }
                            this.sw.Reset();
                            this.sw.Start();
                        }
                    }
                    CDTXMania.ConfigIni.nJudgeLine.Drums = nJudgeLineMaxPosY - this.nJudgeLinePosY.Drums;
                    CDTXMania.stagePerfDrumsScreen.tJudgeLineMovingUpandDown();
                }
                if (!CDTXMania.ConfigIni.bReverse.Drums && keyboard.bKeyPressing((int)SlimDXKey.PageDown))
                {
                    if (!this.sw.IsRunning)
                    {
                        this.sw2 = Stopwatch.StartNew();
                        this.sw = Stopwatch.StartNew();
                        if (this.nJudgeLinePosY.Drums < nJudgeLineMaxPosY)
                        {
                            this.nJudgeLinePosY.Drums++;
                        }
                    }
                    else if (this.sw.ElapsedMilliseconds > 10L)
                    {
                        if (this.sw2.IsRunning)
                        {
                            if (this.sw2.ElapsedMilliseconds > 100L)
                            {
                                this.sw2.Reset();
                            }
                        }
                        else if (this.nJudgeLinePosY.Drums < nJudgeLineMaxPosY)
                        {
                            this.nJudgeLinePosY.Drums++;
                        }
                        this.sw.Reset();
                        this.sw.Start();
                    }
                    CDTXMania.ConfigIni.nJudgeLine.Drums = nJudgeLineMaxPosY - this.nJudgeLinePosY.Drums;
                    CDTXMania.stagePerfDrumsScreen.tJudgeLineMovingUpandDown();
                }

                if (keyboard.bKeyPressing((int)SlimDXKey.NumberPad8))
                {
                    if (!this.sw.IsRunning)
                    {
                        this.sw2 = Stopwatch.StartNew();
                        this.sw = Stopwatch.StartNew();
                        if (this.nShutterInPosY.Drums > 0)
                        {
                            this.nShutterInPosY.Drums--;
                        }
                    }
                    else if (this.sw.ElapsedMilliseconds > 10L)
                    {
                        if (this.sw2.IsRunning)
                        {
                            if (this.sw2.ElapsedMilliseconds > 100L)
                            {
                                this.sw2.Reset();
                            }
                        }
                        else if (this.nShutterInPosY.Drums > 0)
                        {
                            this.nShutterInPosY.Drums--;
                        }
                        this.sw.Reset();
                        this.sw.Start();
                    }
                    CDTXMania.ConfigIni.nShutterInSide.Drums = this.nShutterInPosY.Drums;
                }
                if (keyboard.bKeyPressing((int)SlimDXKey.NumberPad2))
                {
                    if (!this.sw.IsRunning)
                    {
                        this.sw2 = Stopwatch.StartNew();
                        this.sw = Stopwatch.StartNew();
                        if (this.nShutterInPosY.Drums < 100)
                        {
                            if (this.nShutterInPosY.Drums + this.nShutterOutPosY.Drums <= 99)
                                this.nShutterInPosY.Drums++;
                        }
                    }
                    else if (this.sw.ElapsedMilliseconds > 10L)
                    {
                        if (this.sw2.IsRunning)
                        {
                            if (this.sw2.ElapsedMilliseconds > 100L)
                            {
                                this.sw2.Reset();
                            }
                        }
                        else if (this.nShutterInPosY.Drums < 100)
                        {
                            if (this.nShutterInPosY.Drums + this.nShutterOutPosY.Drums <= 99)
                                this.nShutterInPosY.Drums++;
                        }
                        this.sw.Reset();
                        this.sw.Start();
                    }
                    CDTXMania.ConfigIni.nShutterInSide = this.nShutterInPosY;
                }
                if (keyboard.bKeyPressing((int)SlimDXKey.NumberPad4))
                {
                    if (!this.sw.IsRunning)
                    {
                        this.sw2 = Stopwatch.StartNew();
                        this.sw = Stopwatch.StartNew();
                        if (this.nShutterOutPosY.Drums < 100)
                        {
                            if (this.nShutterInPosY.Drums + this.nShutterOutPosY.Drums <= 99)
                                this.nShutterOutPosY.Drums++;
                        }
                    }
                    else if (this.sw.ElapsedMilliseconds > 10L)
                    {
                        if (this.sw2.IsRunning)
                        {
                            if (this.sw2.ElapsedMilliseconds > 100L)
                            {
                                this.sw2.Reset();
                            }
                        }
                        else if (this.nShutterOutPosY.Drums < 100)
                        {
                            if (this.nShutterInPosY.Drums + this.nShutterOutPosY.Drums <= 99)
                                this.nShutterOutPosY.Drums++;
                        }
                        this.sw.Reset();
                        this.sw.Start();
                    }
                    CDTXMania.ConfigIni.nShutterOutSide.Drums = this.nShutterOutPosY.Drums;
                }
                if (keyboard.bKeyPressing((int)SlimDXKey.NumberPad6))
                {
                    if (!this.sw.IsRunning)
                    {
                        this.sw2 = Stopwatch.StartNew();
                        this.sw = Stopwatch.StartNew();
                        if (this.nShutterOutPosY.Drums > 0)
                        {
                            this.nShutterOutPosY.Drums--;
                        }
                    }
                    else if (this.sw.ElapsedMilliseconds > 10L)
                    {
                        if (this.sw2.IsRunning)
                        {
                            if (this.sw2.ElapsedMilliseconds > 100L)
                            {
                                this.sw2.Reset();
                            }
                        }
                        else if (this.nShutterOutPosY.Drums > 0)
                        {
                            this.nShutterOutPosY.Drums--;
                        }
                        this.sw.Reset();
                        this.sw.Start();
                    }
                    CDTXMania.ConfigIni.nShutterOutSide.Drums = this.nShutterOutPosY.Drums;
                }

                if (keyboard.bKeyPressed(0x3a))
                {
                    CConfigIni configIni = CDTXMania.ConfigIni;
                    configIni.nMovieMode++;
                    if (CDTXMania.ConfigIni.nMovieMode >= 4)
                    {
                        CDTXMania.ConfigIni.nMovieMode = 0;
                    }
                    this.actAVI.MovieMode();
                }
                if (keyboard.bKeyPressed(0x3b))
                {
                    CConfigIni configIni = CDTXMania.ConfigIni;
                    configIni.nInfoType++;
                    if (CDTXMania.ConfigIni.nInfoType >= 2)
                    {
                        CDTXMania.ConfigIni.nInfoType = 0;
                    }
                }
                if ((keyboard.bKeyPressing(0x3c)))
                {
                    //F7
                    //CDTXMania.stagePerfDrumsScreen.actGauge.db現在のゲージ値.Drums = 1.0;
                    //CDTXMania.stagePerfDrumsScreen.actGraph.dbグラフ値現在_渡 = 100.0;
                    //CDTXMania.ConfigIni.nヒット範囲ms.Perfect = 1000;
                }
                if (keyboard.bKeyPressed(0x3d))
                {
                    //F8キー

                }
            }
        }

        protected void tSaveInputMethod(EInstrumentPart part)
        {
            if (CDTXMania.Pad.stDetectedDevice.Keyboard)
            {
                this.bKeyboardUsed[(int)part] = true;
            }
            if (CDTXMania.Pad.stDetectedDevice.Joypad)
            {
                this.bJoypadUsed[(int)part] = true;
            }
            if (CDTXMania.Pad.stDetectedDevice.MIDIIN)
            {
                this.bMIDIUsed[(int)part] = true;
            }
            if (CDTXMania.Pad.stDetectedDevice.Mouse)
            {
                this.bMouseUsed[(int)part] = true;
            }
        }

        //      protected abstract void tUpdateAndDraw_AVI();
        protected void tUpdateAndDraw_AVI()
        {
            if (((base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED) && (base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED_フェードアウト)) && (!CDTXMania.ConfigIni.bストイックモード))
            {
                this.actAVI.tUpdateAndDraw(0, 0);
            }
        }
        /*
        protected abstract void t進行描画_BGA();
        protected void t進行描画_BGA(int x, int y)
        {
            if (((base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED) && (base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED_フェードアウト)) && (CDTXMania.ConfigIni.bBGAEnabled))
            {
                this.actBGA.tUpdateAndDraw(x, y);
            }
        }
         */
        protected abstract void tUpdateAndDraw_DANGER();
        protected void tUpdateAndDraw_MIDIBGM()
        {
            if (base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED)
            {
                CStage.EPhase ePhaseid1 = base.ePhaseID;
            }
        }
        protected void tUpdateAndDraw_RGBButton()
        {
            this.actRGB.OnUpdateAndDraw();
        }
        protected void tUpdateAndDraw_STAGEFAILED()
        {
            if (((base.ePhaseID == CStage.EPhase.演奏_STAGE_FAILED) || (base.ePhaseID == CStage.EPhase.演奏_STAGE_FAILED_フェードアウト)) && ((this.actStageFailed.OnUpdateAndDraw() != 0) && (base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED_フェードアウト)))
            {
                this.eReturnValueAfterFadeOut = EPerfScreenReturnValue.StageFailure;
                base.ePhaseID = CStage.EPhase.演奏_STAGE_FAILED_フェードアウト;
                CDTXMania.DTX.tStopPlayingAllChips();
                this.actFO.tStartFadeOut();
            }
        }
        protected void tUpdateAndDraw_WailingBonus()
        {
            if ((base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED) && (base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED_フェードアウト))
            {
                this.actWailingBonus.OnUpdateAndDraw();
            }
        }

        protected void tUpdateAndDraw_GuitarBonus()
        {
            if ((base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED) && (base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED_フェードアウト))
            {
                this.actGuitarBonus.OnUpdateAndDraw();
            }
        }
        protected abstract void tUpdateAndDraw_WailingFrame();
        protected void tUpdateAndDraw_WailingFrame(int GtWailingFrameX, int BsWailingFrameX, int GtWailingFrameY, int BsWailingFrameY)
        {
            if (this.txWailingFrame != null && CDTXMania.ConfigIni.bGuitarEnabled)
            {
                if (CDTXMania.DTX.bチップがある.Guitar)
                {
                    this.txWailingFrame.tDraw2D(CDTXMania.app.Device, GtWailingFrameX, GtWailingFrameY);
                }
                if (CDTXMania.DTX.bチップがある.Bass)
                {
                    this.txWailingFrame.tDraw2D(CDTXMania.app.Device, BsWailingFrameX, BsWailingFrameY);
                }
            }
        }


        protected void tUpdateAndDraw_ChipFireGB()
        {
            this.actChipFireGB.OnUpdateAndDraw();
        }

        /*
        protected abstract void t進行描画_パネル文字列();
        protected void t進行描画_パネル文字列()
        {
            if ((base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED) && (base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED_フェードアウト))
            {
                this.actPanel.tUpdateAndDraw();
            }
        }
        protected void tパネル文字列の設定()
        {
            this.actPanel.SetPanelString(string.IsNullOrEmpty(CDTXMania.DTX.PANEL) ? CDTXMania.stageSongSelection.rConfirmedSong.strタイトル : CDTXMania.DTX.PANEL);
        }
         */

        protected void tUpdateAndDraw_Gauge()
        {
            this.actGauge.OnUpdateAndDraw();
        }
        protected void tUpdateAndDraw_Combo()
        {
            this.actCombo.OnUpdateAndDraw();
        }
        protected void tUpdateAndDraw_Score()
        {
            this.actScore.OnUpdateAndDraw();
        }
        protected void tUpdateAndDraw_StatusPanel()
        {
            this.actStatusPanel.OnUpdateAndDraw();
        }
        protected bool tUpdateAndDraw_Chips(EInstrumentPart ePlayMode)
        {
            if ((base.ePhaseID == CStage.EPhase.演奏_STAGE_FAILED) || (base.ePhaseID == CStage.EPhase.演奏_STAGE_FAILED_フェードアウト))
            {
                return true;
            }
            if ((this.nCurrentTopChip == -1) || (this.nCurrentTopChip >= listChip.Count))
            {
                return true;
            }
            //if (this.nCurrentTopChip == -1)
            //{
            //    return true;
            //}

            //Update currentTopChip index when current top chip has moved past bar
            //Updates nCurrentTopChip for Long Notes
            #region [Update nCurrentTopChip for Long Notes]
            CChip cChip = CDTXMania.DTX.listChip[this.nCurrentTopChip];
            if (cChip.bHit && cChip.nDistanceFromBar.Drums < -200 && cChip.nDistanceFromBar.Guitar < -200 && cChip.nDistanceFromBar.Bass < -200)
            {
                if (cChip.bロングノートである)
                {
                    CChip chipロングノート終端 = cChip.chipロングノート終端;
                    if (chipロングノート終端.bHit && chipロングノート終端.nDistanceFromBar.Drums < -200 && chipロングノート終端.nDistanceFromBar.Guitar < -200 && chipロングノート終端.nDistanceFromBar.Bass < -200)
                    {
                        this.nCurrentTopChip++;
                    }
                }
                else
                {
                    this.nCurrentTopChip++;
                }
            }
            #endregion

            const double speed = 286;	// BPM150の時の1小節の長さ[dot]
            //XGのHS4.5が1289。思えばBPMじゃなくて拍の長さが関係あるよね。

            //double ScrollSpeedDrums = (this.actScrollSpeed.db現在の譜面スクロール速度.Drums + 1.0) * 0.5 * 37.5 * speed / 60000.0;
            //double ScrollSpeedGuitar = (this.actScrollSpeed.db現在の譜面スクロール速度.Guitar + 1.0) * 0.5 * 0.5 * 37.5 * speed / 60000.0;
            //double ScrollSpeedBass = (this.actScrollSpeed.db現在の譜面スクロール速度.Bass + 1.0) * 0.5 * 0.5 * 37.5 * speed / 60000.0;

            CDTX dTX = CDTXMania.DTX;
            CConfigIni configIni = CDTXMania.ConfigIni;
            for (int nCurrentTopChip = this.nCurrentTopChip; nCurrentTopChip < dTX.listChip.Count; nCurrentTopChip++)
            {
                CChip pChip = dTX.listChip[nCurrentTopChip];
                //Debug.WriteLine( "nCurrentTopChip=" + nCurrentTopChip + ", ch=" + pChip.nChannelNumber.ToString("x2") + ", 発音位置=" + pChip.nPlaybackPosition + ", 発声時刻ms=" + pChip.nPlaybackTimeMs );
                //pChip.nDistanceFromBar.Drums = (int)((pChip.nPlaybackTimeMs - CSoundManager.rcPerformanceTimer.nCurrentTime) * ScrollSpeedDrums);
                //pChip.nDistanceFromBar.Guitar = (int)((pChip.nPlaybackTimeMs - CSoundManager.rcPerformanceTimer.nCurrentTime) * ScrollSpeedGuitar);
                //pChip.nDistanceFromBar.Bass = (int)((pChip.nPlaybackTimeMs - CSoundManager.rcPerformanceTimer.nCurrentTime) * ScrollSpeedBass);
                pChip.ComputeDistanceFromBar(CSoundManager.rcPerformanceTimer.nCurrentTime, this.actScrollSpeed.db現在の譜面スクロール速度);
                if (Math.Min(Math.Min(pChip.nDistanceFromBar.Drums, pChip.nDistanceFromBar.Guitar), pChip.nDistanceFromBar.Bass) > 600)
                {
                    break;
                }
                //				if ( ( ( nCurrentTopChip == this.nCurrentTopChip ) && ( pChip.nDistanceFromBar.Drums < -65 ) ) && pChip.bHit )
                // #28026 2012.4.5 yyagi; 信心ワールドエンドの曲終了後リザルトになかなか行かない問題の修正
                if ((dTX.listChip[this.nCurrentTopChip].nDistanceFromBar.Drums < -65) && dTX.listChip[this.nCurrentTopChip].bHit)
                {
                    //					nCurrentTopChip = ++this.nCurrentTopChip;

                    if (dTX.listChip[this.nCurrentTopChip].bロングノートである)
                    {
                        CChip chipロングノート終端 = dTX.listChip[this.nCurrentTopChip].chipロングノート終端;
                        if (chipロングノート終端.bHit && chipロングノート終端.nDistanceFromBar.Drums < -65)
                        {
                            this.nCurrentTopChip++;
                            continue;
                        }
                    }
                    else
                    {
                        ++this.nCurrentTopChip;
                        continue;
                    }
                    
                }

                bool bPChipIsAutoPlay = bCheckAutoPlay(pChip);

                int nInputAdjustTime = (bPChipIsAutoPlay || (pChip.eInstrumentPart == EInstrumentPart.UNKNOWN)) ? 0 : this.nInputAdjustTimeMs[(int)pChip.eInstrumentPart];

				int instIndex = (int) pChip.eInstrumentPart;
				if ( ( ( pChip.eInstrumentPart != EInstrumentPart.UNKNOWN ) && !pChip.bHit ) &&
				    ( ( pChip.nDistanceFromBar[ instIndex ] < 0 ) && ( this.e指定時刻からChipのJUDGEを返す( CSoundManager.rcPerformanceTimer.nCurrentTime, pChip, nInputAdjustTime ) == EJudgement.Miss ) ) )
				{
				    this.tProcessChipHit( CSoundManager.rcPerformanceTimer.nCurrentTime, pChip );
				}

                // #35411 chnmr0 add (ターゲットゴースト)
                if ( CDTXMania.ConfigIni.eTargetGhost[instIndex] != ETargetGhostData.NONE &&
                     CDTXMania.listTargetGhsotLag[instIndex] != null &&
                     pChip.eInstrumentPart != EInstrumentPart.UNKNOWN &&
                     pChip.nDistanceFromBar[instIndex] < 0 )
                {
                    if ( !pChip.bTargetGhost判定済み )
                    {
                        pChip.bTargetGhost判定済み = true;

						int ghostLag = 128;
						if( 0 <= pChip.n楽器パートでの出現順 && pChip.n楽器パートでの出現順 < CDTXMania.listTargetGhsotLag[instIndex].Count )
                        {
                            ghostLag = CDTXMania.listTargetGhsotLag[instIndex][pChip.n楽器パートでの出現順];
							// 上位８ビットが１ならコンボが途切れている（ギターBAD空打ちでコンボ数を再現するための措置）
							if( ghostLag > 255 )
							{
								this.nコンボ数_TargetGhost[instIndex] = 0;
							}
							ghostLag = (ghostLag & 255) - 128;
						}
                        else if( CDTXMania.ConfigIni.eTargetGhost[instIndex] == ETargetGhostData.PERFECT )
                        {
                            ghostLag = 0;
                        }
                        
                        if ( ghostLag <= 127 )
                        {
                            EJudgement eJudge = this.e指定時刻からChipのJUDGEを返す(pChip.nPlaybackTimeMs + ghostLag, pChip, 0, false);
                            this.nヒット数_TargetGhost[instIndex][(int)eJudge]++;
                            if (eJudge == EJudgement.Miss || eJudge == EJudgement.Poor)
                            {
                                this.n最大コンボ数_TargetGhost[instIndex] = Math.Max(this.n最大コンボ数_TargetGhost[instIndex], this.nコンボ数_TargetGhost[instIndex]);
                                this.nコンボ数_TargetGhost[instIndex] = 0;
                            }
                            else
                            {
                                this.n最大コンボ数_TargetGhost[instIndex] = Math.Max(this.n最大コンボ数_TargetGhost[instIndex], this.nコンボ数_TargetGhost[instIndex]);
                                this.nコンボ数_TargetGhost[instIndex]++;
                            }
                        }
                    }
                }

                switch (pChip.nChannelNumber)
                {
                    //描画順の都合上こちらから描画。

                    #region [ 11-1c: Drums ]
                    case EChannel.HiHatClose:	// ドラム演奏
                    case EChannel.Snare:
                    case EChannel.BassDrum:
                    case EChannel.HighTom:
                    case EChannel.LowTom:
                    case EChannel.Cymbal:
                    case EChannel.FloorTom:
                    case EChannel.HiHatOpen:
                    case EChannel.RideCymbal:
                    case EChannel.LeftCymbal:
                    case EChannel.LeftPedal:
                    case EChannel.LeftBassDrum:
                        this.tUpdateAndDraw_Chip_Drums(configIni, ref dTX, ref pChip);
                        break;
                    #endregion

                    #region [ 01: BGM ]
                    case EChannel.BGM:	// BGM
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                            if (configIni.bBGM音を発声する)
                            {
                                dTX.tPlayChip(pChip, CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻 + pChip.nPlaybackTimeMs, (int)ELane.BGM, dTX.nモニタを考慮した音量(EInstrumentPart.UNKNOWN));
                            }
                        }
                        break;
                    #endregion
                    #region [ 03: BPM変更 ]
                    case EChannel.BPM:	// BPM変更
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                            this.actPlayInfo.dbBPM = (pChip.nIntegerValue * (((double)configIni.nPlaySpeed) / 20.0)) + dTX.BASEBPM;

                            if (CDTXMania.ConfigIni.bDrumsEnabled)
                            {
                                CDTXMania.stagePerfDrumsScreen.UnitTime = ((60.0 / (CDTXMania.stagePerfDrumsScreen.actPlayInfo.dbBPM) / 14.0));
                                CDTXMania.stagePerfDrumsScreen.ctComboTimer = new CCounter(1, 16, (int)((60.0 / (CDTXMania.stagePerfDrumsScreen.actPlayInfo.dbBPM) / 16) * 1000.0), CDTXMania.Timer);
                            }
                            else if (CDTXMania.ConfigIni.bGuitarEnabled)
                            {
                                CDTXMania.stagePerfGuitarScreen.UnitTime = ((60.0 / (CDTXMania.stagePerfGuitarScreen.actPlayInfo.dbBPM) / 14.0));
                                CDTXMania.stagePerfGuitarScreen.ctComboTimer = new CCounter(1, 16, (int)((60.0 / (CDTXMania.stagePerfGuitarScreen.actPlayInfo.dbBPM) / 16) * 1000.0), CDTXMania.Timer);
                            }
                        }
                        break;
                    #endregion
                    #region [ 04, 07, 55, 56,57, 58, 59, 60:レイヤーBGA ]
                    case EChannel.BGALayer1:	// レイヤーBGA
                    case EChannel.BGALayer2:
                    case EChannel.BGALayer3:
                    case EChannel.BGALayer4:
                    case EChannel.BGALayer5:
                    case EChannel.BGALayer6:
                    case EChannel.BGALayer7:
                    case EChannel.BGALayer8:
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                            if (configIni.bBGAEnabled)
                            {
                                switch (pChip.eBGA種別)
                                {
                                    case EBGAType.BMPTEX:
                                        if (pChip.rBMPTEX != null)
                                        {
                                            this.actBGA.Start(pChip.nChannelNumber, null, pChip.rBMPTEX, pChip.rBMPTEX.tx画像.szImageSize.Width, pChip.rBMPTEX.tx画像.szImageSize.Height, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                                        }
                                        break;

                                    case EBGAType.BGA:
                                        if ((pChip.rBGA != null) && ((pChip.rBMP != null) || (pChip.rBMPTEX != null)))
                                        {
                                            this.actBGA.Start(pChip.nChannelNumber, pChip.rBMP, pChip.rBMPTEX, pChip.rBGA.pt画像側右下座標.X - pChip.rBGA.pt画像側左上座標.X, pChip.rBGA.pt画像側右下座標.Y - pChip.rBGA.pt画像側左上座標.Y, 0, 0, pChip.rBGA.pt画像側左上座標.X, pChip.rBGA.pt画像側左上座標.Y, 0, 0, pChip.rBGA.pt表示座標.X, pChip.rBGA.pt表示座標.Y, 0, 0, 0);
                                        }
                                        break;

                                    case EBGAType.BGAPAN:
                                        if ((pChip.rBGAPan != null) && ((pChip.rBMP != null) || (pChip.rBMPTEX != null)))
                                        {
                                            this.actBGA.Start(pChip.nChannelNumber, pChip.rBMP, pChip.rBMPTEX, pChip.rBGAPan.sz開始サイズ.Width, pChip.rBGAPan.sz開始サイズ.Height, pChip.rBGAPan.sz終了サイズ.Width, pChip.rBGAPan.sz終了サイズ.Height, pChip.rBGAPan.pt画像側開始位置.X, pChip.rBGAPan.pt画像側開始位置.Y, pChip.rBGAPan.pt画像側終了位置.X, pChip.rBGAPan.pt画像側終了位置.Y, pChip.rBGAPan.pt表示側開始位置.X, pChip.rBGAPan.pt表示側開始位置.Y, pChip.rBGAPan.pt表示側終了位置.X, pChip.rBGAPan.pt表示側終了位置.Y, pChip.n総移動時間);
                                        }
                                        break;

                                    default:
                                        if (pChip.rBMP != null)
                                        {
                                            this.actBGA.Start(pChip.nChannelNumber, pChip.rBMP, null, pChip.rBMP.n幅, pChip.rBMP.n高さ, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    #endregion
                    #region [ 08: BPM変更(拡張) ]
                    case EChannel.BPMEx:	// BPM変更(拡張)
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                            if (dTX.listBPM.ContainsKey(pChip.nIntegerValue_InternalNumber))
                            {
                                this.actPlayInfo.dbBPM = (dTX.listBPM[pChip.nIntegerValue_InternalNumber].dbBPM値 * (((double)configIni.nPlaySpeed) / 20.0)) + dTX.BASEBPM;

                                if (CDTXMania.ConfigIni.bDrumsEnabled)
                                {
                                    CDTXMania.stagePerfDrumsScreen.UnitTime = ((60.0 / (CDTXMania.stagePerfDrumsScreen.actPlayInfo.dbBPM) / 14.0));
                                    CDTXMania.stagePerfDrumsScreen.ctComboTimer = new CCounter(1, 16, (int)((60.0 / (CDTXMania.stagePerfDrumsScreen.actPlayInfo.dbBPM) / 16) * 1000.0), CDTXMania.Timer);
                                }
                                else if (CDTXMania.ConfigIni.bGuitarEnabled)
                                {
                                    CDTXMania.stagePerfGuitarScreen.UnitTime = ((60.0 / (CDTXMania.stagePerfGuitarScreen.actPlayInfo.dbBPM) / 14.0));
                                    CDTXMania.stagePerfGuitarScreen.ctComboTimer = new CCounter(1, 16, (int)((60.0 / (CDTXMania.stagePerfGuitarScreen.actPlayInfo.dbBPM) / 16) * 1000.0), CDTXMania.Timer);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region [ 1f: フィルインサウンド(ドラム) ]
                    case EChannel.DrumsFillin:	// フィルインサウンド(ドラム)
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                            this.r現在の歓声Chip.Drums = pChip;
                        }
                        break;
                    #endregion
                    #region [ 20-27: ギター演奏 ]
                    case EChannel.Guitar_Open:	// ギター演奏
                    case EChannel.Guitar_xxBxx:
                    case EChannel.Guitar_xGxxx:
                    case EChannel.Guitar_xGBxx:
                    case EChannel.Guitar_Rxxxx:
                    case EChannel.Guitar_RxBxx:
                    case EChannel.Guitar_RGxxx:
                    case EChannel.Guitar_RGBxx:


                    case EChannel.Guitar_xxxYx:
                    case EChannel.Guitar_xxBYx:
                    case EChannel.Guitar_xGxYx:
                    case EChannel.Guitar_xGBYx:
                    case EChannel.Guitar_RxxYx:
                    case EChannel.Guitar_RxBYx:
                    case EChannel.Guitar_RGxYx:
                    case EChannel.Guitar_RGBYx:
                    case EChannel.Guitar_xxxxP:
                    case EChannel.Guitar_xxBxP:
                    case EChannel.Guitar_xGxxP:
                    case EChannel.Guitar_xGBxP:
                    case EChannel.Guitar_RxxxP:
                    case EChannel.Guitar_RxBxP:
                    case EChannel.Guitar_RGxxP:
                    case EChannel.Guitar_RGBxP:
                    case EChannel.Guitar_xxxYP:
                    case EChannel.Guitar_xxBYP:
                    case EChannel.Guitar_xGxYP:
                    case EChannel.Guitar_xGBYP:
                    case EChannel.Guitar_RxxYP:
                    case EChannel.Guitar_RxBYP:
                    case EChannel.Guitar_RGxYP:
                    case EChannel.Guitar_RGBYP:

                        this.tUpdateAndDraw_Chip_GuitarBass(configIni, ref dTX, ref pChip, EInstrumentPart.GUITAR);
                        break;
                    #endregion
                    #region [ 28: ウェイリング(ギター) ]
                    case EChannel.Guitar_Wailing:	// ウェイリング(ギター)
                        this.tUpdateAndDraw_Chip_Guitar_Wailing(configIni, ref dTX, ref pChip);
                        break;
                    #endregion
                    #region [ 2f: ウェイリングサウンド(ギター) ]
                    case EChannel.Guitar_WailingSound:	// ウェイリングサウンド(ギター)
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Guitar < 0))
                        {
                            pChip.bHit = true;
                            this.r現在の歓声Chip.Guitar = pChip;
                        }
                        break;
                    #endregion
                    #region [ 31-3a: 不可視チップ配置(ドラム) ]
                    case EChannel.HiHatClose_Hidden:	// 不可視チップ配置(ドラム)
                    case EChannel.Snare_Hidden:
                    case EChannel.BassDrum_Hidden:
                    case EChannel.HighTom_Hidden:
                    case EChannel.LowTom_Hidden:
                    case EChannel.Cymbal_Hidden:
                    case EChannel.FloorTom_Hidden:
                    case EChannel.HiHatOpen_Hidden:
                    case EChannel.RideCymbal_Hidden:
                    case EChannel.LeftCymbal_Hidden:
                    case EChannel.LeftPedal_Hidden:
                    case EChannel.LeftBassDrum_Hidden:
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                        }
                        break;
                    #endregion
                    #region [ 4F、4E、4D、4C: ボーナス ]
                    case EChannel.BonusEffect_Min:
                    case EChannel.BonusEffect2:
                    case EChannel.BonusEffect3:
                    case EChannel.BonusEffect:  //追加した順番の都合上、4F、4E____という順でBonus1、Bonus2___という割り当てになってます。
                        //this.t進行描画_チップ_ボーナス(configIni, ref dTX, ref pChip);
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                        }
                        break;
                    #endregion

                    #region [ 52: MIDIコーラス ]
                    case EChannel.MIDIChorus:	// MIDIコーラス
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                        }
                        break;
                    #endregion
                    #region [ 53: フィルイン ]
                    case EChannel.FillIn:	// フィルイン
                        this.tUpdateAndDraw_Chip_FillIn(configIni, ref dTX, ref pChip);
                        break;
                    #endregion
                    #region [ 54, 5A: 動画再生 ]
                    case EChannel.Movie:	// 動画再生
                    case EChannel.MovieFull:
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                            if (configIni.bAVIEnabled)
                            {
                                switch (pChip.eAVI種別)
                                {
                                    case EAVIType.AVI:
                                        if (pChip.rAVI != null)
                                        {
                                            this.actAVI.bLoop = false;
                                            this.actAVI.Start(pChip.nChannelNumber, pChip.rAVI, 278, 355, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, pChip.nPlaybackTimeMs);
                                        }                                        
                                        break;

                                    case EAVIType.AVIPAN:
                                        if (pChip.rAVIPan != null)
                                        {
                                            this.actAVI.bLoop = false;
                                            this.actAVI.Start(pChip.nChannelNumber, pChip.rAVI, pChip.rAVIPan.sz開始サイズ.Width, pChip.rAVIPan.sz開始サイズ.Height, pChip.rAVIPan.sz終了サイズ.Width, pChip.rAVIPan.sz終了サイズ.Height, pChip.rAVIPan.pt動画側開始位置.X, pChip.rAVIPan.pt動画側開始位置.Y, pChip.rAVIPan.pt動画側終了位置.X, pChip.rAVIPan.pt動画側終了位置.Y, pChip.rAVIPan.pt表示側開始位置.X, pChip.rAVIPan.pt表示側開始位置.Y, pChip.rAVIPan.pt表示側終了位置.X, pChip.rAVIPan.pt表示側終了位置.Y, pChip.n総移動時間, pChip.nPlaybackTimeMs);
                                        }                                        
                                        break;
                                }
                            }
                        }
                        break;
                    #endregion
                    #region [ 61-65: 自動再生(Muting SE) ]
                    //SE01-05 are reserved as additional Muting channels i.e. only one wav can be played at any given time per channel
                    case EChannel.SE01:
                    case EChannel.SE02:
                    case EChannel.SE03:
                    case EChannel.SE04:	
                    case EChannel.SE05:
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                            if (configIni.bBGM音を発声する)
                            {   
                                dTX.tStopPlayingWav(this.nLastPlayedBGMWAVNumber[pChip.nChannelNumber - EChannel.SE01]);
                                dTX.tPlayChip(pChip, CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻 + pChip.nPlaybackTimeMs, (int)ELane.BGM, dTX.nモニタを考慮した音量(EInstrumentPart.UNKNOWN));
                                this.nLastPlayedBGMWAVNumber[pChip.nChannelNumber - EChannel.SE01] = pChip.nIntegerValue_InternalNumber;
                            }
                        }
                        break;
                    #endregion
                    #region [ 66-92: 自動再生(Non-muting SE) ]
                    //SE06 to SE23, SE30 to SE32 are updated to be non-muting SE channels
                    case EChannel.SE06:
                    case EChannel.SE07:
                    case EChannel.SE08:
                    case EChannel.SE09:
                    case EChannel.SE10:
                    case EChannel.SE11:
                    case EChannel.SE12:
                    case EChannel.SE13:
                    case EChannel.SE14:
                    case EChannel.SE15:
                    case EChannel.SE16:
                    case EChannel.SE17:
                    case EChannel.SE18:
                    case EChannel.SE19:
                    case EChannel.SE20:
                    case EChannel.SE21:
                    case EChannel.SE22:
                    case EChannel.SE23:
                    case EChannel.SE30:
                    case EChannel.SE31:
                    case EChannel.SE32:
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                            if (configIni.bBGM音を発声する)
                            {
                                dTX.tPlayChip(pChip, CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻 + pChip.nPlaybackTimeMs, (int)ELane.BGM, dTX.nモニタを考慮した音量(EInstrumentPart.UNKNOWN));
                            }
                        }
                        break;
                    #endregion

                    #region [ 84-89: 仮: override sound ]	// #26338 2011.11.8 yyagi
                    case EChannel.SE24:	// HH (HO/HC)
                    case EChannel.SE25:	// CY
                    case EChannel.SE26:	// RD
                    case EChannel.SE27:	// LC
                    case EChannel.SE28:	// Guitar
                    case EChannel.SE29:	// Bass
                        // mute sound (auto)
                        // 4A: 84: HH (HO/HC)
                        // 4B: 85: CY
                        // 4C: 86: RD
                        // 4D: 87: LC
                        // 2A: 88: Gt
                        // AA: 89: Bs

                        //	CDTXMania.DTX.tStopPlayingWav( this.nLastPlayedWAVNumber.Guitar );
                        //	CDTXMania.DTX.tPlayChip( pChip, n再生開始システム時刻ms, 8, nVolume, bモニタ, b音程をずらして再生 );
                        //	this.nLastPlayedWAVNumber.Guitar = pChip.nIntegerValue_InternalNumber;

                        //	protected void tPlaySound( CChip pChip, long n再生開始システム時刻ms, EInstrumentPart part, int nVolume, bool bモニタ, bool b音程をずらして再生 )
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                            EInstrumentPart[] p = { EInstrumentPart.DRUMS, EInstrumentPart.DRUMS, EInstrumentPart.DRUMS, EInstrumentPart.DRUMS, EInstrumentPart.GUITAR, EInstrumentPart.BASS };

                            EInstrumentPart pp = p[pChip.nChannelNumber - EChannel.SE24];

                            //							if ( pp == EInstrumentPart.DRUMS ) {			// pChip.nChannelNumber= ..... HHとか、ドラムの場合は変える。
                            //								//            HC    CY    RD    LC
                            //								int[] ch = { 0x11, 0x16, 0x19, 0x1A };
                            //								pChip.nChannelNumber = ch[ pChip.nChannelNumber - 0x84 ]; 
                            //							}
                            this.tPlaySound(pChip, CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻 + pChip.nPlaybackTimeMs, pp, dTX.nモニタを考慮した音量(pp));
                        }
                        break;
                    #endregion

                    #region [ a0-a7: ベース演奏 ]
                    case EChannel.Bass_Open:	// ベース演奏
                    case EChannel.Bass_xxBxx:
                    case EChannel.Bass_xGxxx:
                    case EChannel.Bass_xGBxx:
                    case EChannel.Bass_Rxxxx:
                    case EChannel.Bass_RxBxx:
                    case EChannel.Bass_RGxxx:
                    case EChannel.Bass_RGBxx:

                    case EChannel.Bass_xxxYx:
                    case EChannel.Bass_xxBYx:
                    case EChannel.Bass_xGxYx:
                    case EChannel.Bass_xGBYx:
                    case EChannel.Bass_RxxYx:
                    case EChannel.Bass_RxBYx:
                    case EChannel.Bass_RGxYx:
                    case EChannel.Bass_RGBYx:
                    case EChannel.Bass_xxxxP:
                    case EChannel.Bass_xxBxP:
                    case EChannel.Bass_xGxxP:
                    case EChannel.Bass_xGBxP:
                    case EChannel.Bass_RxxxP:
                    case EChannel.Bass_RxBxP:
                    case EChannel.Bass_RGxxP:
                    case EChannel.Bass_RGBxP:
                    case EChannel.Bass_xxxYP:
                    case EChannel.Bass_xxBYP:
                    case EChannel.Bass_xGxYP:
                    case EChannel.Bass_xGBYP:
                    case EChannel.Bass_RxxYP:
                    case EChannel.Bass_RxBYP:
                    case EChannel.Bass_RGxYP:
                    case EChannel.Bass_RGBYP:
                        this.tUpdateAndDraw_Chip_GuitarBass(configIni, ref dTX, ref pChip, EInstrumentPart.BASS);
                        break;
                    #endregion
                    #region [ a8: ウェイリング(ベース) ]
                    case EChannel.Bass_Wailing:	// ウェイリング(ベース)
                        this.tUpdateAndDraw_Chip_Bass_Wailing(configIni, ref dTX, ref pChip);
                        break;
                    #endregion
                    #region [2c, 2d: Hold notes]
                    case EChannel.Guitar_LongNote:
                    case EChannel.Bass_LongNote:
                        {
                            if (!pChip.bHit && pChip.nDistanceFromBar.Drums <= 0)
                            {                                
                                pChip.bHit = true;
                                EInstrumentPart index = (pChip.nChannelNumber == EChannel.Guitar_LongNote ? EInstrumentPart.GUITAR : EInstrumentPart.BASS);
                                if (chipロングノートHit中[(int)index] != null && chipロングノートHit中[(int)index].chipロングノート終端 == pChip)
                                {                                    
                                    chipロングノートHit中[(int)index].bロングノートHit中 = false;
                                    chipロングノートHit中[(int)index] = null;
                                    nCurrentLongNoteDuration[(int)index] = 0;
                                    nロングノートPart[(int)index] = 0;
                                }
                            }
                        }
                        break;
                    #endregion
                    #region [ af: ウェイリングサウンド(ベース) ]
                    /*
					case 0xaf:	// ウェイリングサウンド(ベース)
						if ( !pChip.bHit && ( pChip.nDistanceFromBar.Bass < 0 ) )
						{
							pChip.bHit = true;
							this.r現在の歓声Chip.Bass = pChip;
						}
						break;
                        */
                    #endregion
                    #region [ b1-b9, bc: 空打ち音設定(ドラム) ]
                    case EChannel.HiHatClose_NoChip:	// 空打ち音設定(ドラム)
                    case EChannel.Snare_NoChip:
                    case EChannel.BassDrum_NoChip:
                    case EChannel.HighTom_NoChip:
                    case EChannel.LowTom_NoChip:
                    case EChannel.Cymbal_NoChip:
                    case EChannel.FloorTom_NoChip:
                    case EChannel.HiHatOpen_NoChip:
                    case EChannel.RideCymbal_NoChip:
                    case EChannel.LeftCymbal_NoChip:
                    case EChannel.LeftPedal_NoChip:
                    case EChannel.LeftBassDrum_NoChip:
                        this.tUpdateAndDraw_Chip_NoSound_Drums(configIni, ref dTX, ref pChip);
                        break;
                    #endregion
                    #region [ ba: 空打ち音設定(ギター) ]
                    case EChannel.Guitar_NoChip:	// 空打ち音設定(ギター)
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Guitar < 0))
                        {
                            pChip.bHit = true;
                            this.r現在の空うちギターChip = pChip;
                            pChip.nChannelNumber = EChannel.Guitar_Open;
                        }
                        break;
                    #endregion
                    #region [ bb: 空打ち音設定(ベース) ]
                    case EChannel.Bass_NoChip:	// 空打ち音設定(ベース)
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Bass < 0))
                        {
                            pChip.bHit = true;
                            this.r現在の空うちベースChip = pChip;
                            pChip.nChannelNumber = EChannel.Bass_Open;
                        }
                        break;
                    #endregion
                    #region [ c4, c7, d5-d9, e0: BGA画像入れ替え ]
                    case EChannel.BGALayer1_Swap:
                    case EChannel.BGALayer2_Swap:
                    case EChannel.BGALayer3_Swap:
                    case EChannel.BGALayer4_Swap:	// BGA画像入れ替え
                    case EChannel.BGALayer5_Swap:
                    case EChannel.BGALayer6_Swap:
                    case EChannel.BGALayer7_Swap:
                    case EChannel.BGALayer8_Swap:
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                            if ((configIni.bBGAEnabled && (pChip.eBGA種別 == EBGAType.BMP)) || (pChip.eBGA種別 == EBGAType.BMPTEX))
                            {
                                for (int i = 0; i < 8; i++)
                                {
                                    if (this.nBGAスコープチャンネルマップ[0, i] == pChip.nChannelNumber)
                                    {
                                        this.actBGA.ChangeScope(this.nBGAスコープチャンネルマップ[1, i], pChip.rBMP, pChip.rBMPTEX);
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region [ ea: ミキサーへチップ音追加 ]
                    case EChannel.MixChannel1_unc:
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            //Debug.WriteLine("[DA(AddMixer)] BAR=" + pChip.nPlaybackPosition / 384 + " ch=" + pChip.nChannelNumber.ToString("x2") + ", wav=" + pChip.nIntegerValue.ToString("x2") + ", time=" + pChip.nPlaybackTimeMs);
                            pChip.bHit = true;
                            if (listWAV.ContainsKey(pChip.nIntegerValue_InternalNumber)) // 参照が遠いので後日最適化する
                            {
                                CDTX.CWAV wc = listWAV[pChip.nIntegerValue_InternalNumber];
                                for (int i = 0; i < nPolyphonicSounds; i++)
                                {
                                    if (wc.rSound[i] != null)
                                    {
                                        //CDTXMania.SoundManager.AddMixer( wc.rSound[ i ] );
                                        AddMixer(wc.rSound[i], pChip.bChipKeepsPlayingAfterPerfEnds);
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region [ eb: ミキサーからチップ音削除 ]
                    case EChannel.MixChannel2_unc:
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            //Debug.WriteLine("[DB(RemoveMixer)] BAR=" + pChip.nPlaybackPosition / 384 + " ch=" + pChip.nChannelNumber.ToString("x2") + ", wav=" + pChip.nIntegerValue.ToString("x2") + ", time=" + pChip.nPlaybackTimeMs);
                            pChip.bHit = true;
                            if (listWAV.ContainsKey(pChip.nIntegerValue_InternalNumber))	// 参照が遠いので後日最適化する
                            {
                                CDTX.CWAV wc = listWAV[pChip.nIntegerValue_InternalNumber];
                                for (int i = 0; i < nPolyphonicSounds; i++)
                                {
                                    if (wc.rSound[i] != null)
                                    {
                                        //CDTXMania.SoundManager.RemoveMixer( wc.rSound[ i ] );
                                        RemoveMixer(wc.rSound[i]);
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region [ その他(未定義) ]
                    default:
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                        }
                        break;
                    #endregion
                }
            }
            return false;
        }
        protected bool tUpdateAndDraw_BarLines(EInstrumentPart ePlayMode)
        {
            if ((base.ePhaseID == CStage.EPhase.演奏_STAGE_FAILED) || (base.ePhaseID == CStage.EPhase.演奏_STAGE_FAILED_フェードアウト))
            {
                return true;
            }
            if ((this.nCurrentTopChip == -1) || (this.nCurrentTopChip >= listChip.Count))
            {
                return true;
            }
            if (this.nCurrentTopChip == -1)
            {
                return true;
            }

            const double speed = 286;	// BPM150の時の1小節の長さ[dot]
            //XGのHS4.5が1289。思えばBPMじゃなくて拍の長さが関係あるよね。

            //double ScrollSpeedDrums = (this.actScrollSpeed.db現在の譜面スクロール速度.Drums + 1.0) * 0.5 * 37.5 * speed / 60000.0;
            //double ScrollSpeedGuitar = (this.actScrollSpeed.db現在の譜面スクロール速度.Guitar + 1.0) * 0.5 * 0.5 * 37.5 * speed / 60000.0;
            //double ScrollSpeedBass = (this.actScrollSpeed.db現在の譜面スクロール速度.Bass + 1.0) * 0.5 * 0.5 * 37.5 * speed / 60000.0;

            CDTX dTX = CDTXMania.DTX;
            CConfigIni configIni = CDTXMania.ConfigIni;            
            for (int nCurrentTopChip = this.nCurrentTopChip; nCurrentTopChip < dTX.listChip.Count; nCurrentTopChip++)
            {
                CChip pChip = dTX.listChip[nCurrentTopChip];
                //Debug.WriteLine( "nCurrentTopChip=" + nCurrentTopChip + ", ch=" + pChip.nChannelNumber.ToString("x2") + ", 発音位置=" + pChip.nPlaybackPosition + ", 発声時刻ms=" + pChip.nPlaybackTimeMs );
                //pChip.nDistanceFromBar.Drums = (int)((pChip.nPlaybackTimeMs - CSoundManager.rcPerformanceTimer.nCurrentTime) * ScrollSpeedDrums);
                //pChip.nDistanceFromBar.Guitar = (int)((pChip.nPlaybackTimeMs - CSoundManager.rcPerformanceTimer.nCurrentTime) * ScrollSpeedGuitar);
                //pChip.nDistanceFromBar.Bass = (int)((pChip.nPlaybackTimeMs - CSoundManager.rcPerformanceTimer.nCurrentTime) * ScrollSpeedBass);
                pChip.ComputeDistanceFromBar(CSoundManager.rcPerformanceTimer.nCurrentTime, this.actScrollSpeed.db現在の譜面スクロール速度);
                if (Math.Min(Math.Min(pChip.nDistanceFromBar.Drums, pChip.nDistanceFromBar.Guitar), pChip.nDistanceFromBar.Bass) > 600)
                {
                    break;
                }
                //				if ( ( ( nCurrentTopChip == this.nCurrentTopChip ) && ( pChip.nDistanceFromBar.Drums < -65 ) ) && pChip.bHit )
                // #28026 2012.4.5 yyagi; 信心ワールドエンドの曲終了後リザルトになかなか行かない問題の修正
                if ((dTX.listChip[this.nCurrentTopChip].nDistanceFromBar.Drums < -65) && dTX.listChip[this.nCurrentTopChip].bHit)
                {
                    //					nCurrentTopChip = ++this.nCurrentTopChip;
                    if (dTX.listChip[this.nCurrentTopChip].bロングノートである)
                    {
                        CChip chipロングノート終端 = dTX.listChip[this.nCurrentTopChip].chipロングノート終端;
                        if (chipロングノート終端.bHit && chipロングノート終端.nDistanceFromBar.Drums < -65)
                        {
                            this.nCurrentTopChip++;
                            continue;
                        }
                    }
                    else
                    {
                        ++this.nCurrentTopChip;
                        continue;
                    }
                }

                bool bPChipIsAutoPlay = bCheckAutoPlay(pChip);

                int nInputAdjustTime = (bPChipIsAutoPlay || (pChip.eInstrumentPart == EInstrumentPart.UNKNOWN)) ? 0 : this.nInputAdjustTimeMs[(int)pChip.eInstrumentPart];

                switch (pChip.nChannelNumber)
                {
                    #region [ 50: 小節線 ]
                    case EChannel.BarLine:	// 小節線
                        {
                            this.tUpdateAndDraw_Chip_BarLine(configIni, ref dTX, ref pChip);
                            break;
                        }
                    #endregion
                    #region [ 51: 拍線 ]
                    case EChannel.BeatLine:	// 拍線
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;

                            if (CDTXMania.ConfigIni.bMetronome)
                            {
                                CDTXMania.Skin.soundMetronome.tPlay(40);
                            }
                        }
                        if ((ePlayMode == EInstrumentPart.DRUMS) && (configIni.nLaneDisp.Drums == 0 || configIni.nLaneDisp.Drums == 1) && pChip.bVisible && (this.txChip != null))
                        {
                            int l_drumPanelWidth = 0x22f;
                            int l_xOffset = 0;
                            if(configIni.eNumOfLanes.Drums == EType.B)
                            {
                                l_drumPanelWidth = 0x207;
                            }
                            else if (CDTXMania.ConfigIni.eNumOfLanes.Drums == EType.C)
                            {
                                l_drumPanelWidth = 447;
                                l_xOffset = 72;
                            }
                            this.txChip.vcScaleRatio.Y = 1f;
                            this.txChip.tDraw2D(CDTXMania.app.Device, 0x127 + l_xOffset, configIni.bReverse.Drums ? ((this.nJudgeLinePosY.Drums + pChip.nDistanceFromBar.Drums) - 1) : ((this.nJudgeLinePosY.Drums - pChip.nDistanceFromBar.Drums) - 1), new Rectangle(0, 772, l_drumPanelWidth, 2));
                        }
                        break;
                    #endregion
                }
            }
            return false;
        }

        protected bool tDraw_LoopLines()
        {
            if (this.LoopBeginMs != -1)
            {
                this.tDraw_LoopLine(CDTXMania.ConfigIni, false);
                if (this.LoopEndMs != -1)
                {
                    this.tDraw_LoopLine(CDTXMania.ConfigIni, true);
                }
            }
            return false;
        }

        protected bool tUpdateAndDraw_Chip_PatternOnly(EInstrumentPart ePlayMode)
        {
            if ((base.ePhaseID == CStage.EPhase.演奏_STAGE_FAILED) || (base.ePhaseID == CStage.EPhase.演奏_STAGE_FAILED_フェードアウト))
            {
                return true;
            }
            if ((this.nCurrentTopChip == -1) || (this.nCurrentTopChip >= listChip.Count))
            {
                return true;
            }
            if (this.nCurrentTopChip == -1)
            {
                return true;
            }

            const double speed = 286;	// BPM150の時の1小節の長さ[dot]
            //XGのHS4.5が1289。思えばBPMじゃなくて拍の長さが関係あるよね。

            //double ScrollSpeedDrums = (this.actScrollSpeed.db現在の譜面スクロール速度.Drums + 1.0) * 0.5 * 37.5 * speed / 60000.0;
            //double ScrollSpeedGuitar = (this.actScrollSpeed.db現在の譜面スクロール速度.Guitar + 1.0) * 0.5 * 0.5 * 37.5 * speed / 60000.0;
            //double ScrollSpeedBass = (this.actScrollSpeed.db現在の譜面スクロール速度.Bass + 1.0) * 0.5 * 0.5 * 37.5 * speed / 60000.0;

            CDTX dTX = CDTXMania.DTX;
            CConfigIni configIni = CDTXMania.ConfigIni;
            for (int nCurrentTopChip = this.nCurrentTopChip; nCurrentTopChip < dTX.listChip.Count; nCurrentTopChip++)
            {
                CChip pChip = dTX.listChip[nCurrentTopChip];
                //Debug.WriteLine( "nCurrentTopChip=" + nCurrentTopChip + ", ch=" + pChip.nChannelNumber.ToString("x2") + ", 発音位置=" + pChip.nPlaybackPosition + ", 発声時刻ms=" + pChip.nPlaybackTimeMs );
                //pChip.nDistanceFromBar.Drums = (int)((pChip.nPlaybackTimeMs - CSoundManager.rcPerformanceTimer.nCurrentTime) * ScrollSpeedDrums);
                //pChip.nDistanceFromBar.Guitar = (int)((pChip.nPlaybackTimeMs - CSoundManager.rcPerformanceTimer.nCurrentTime) * ScrollSpeedGuitar);
                //pChip.nDistanceFromBar.Bass = (int)((pChip.nPlaybackTimeMs - CSoundManager.rcPerformanceTimer.nCurrentTime) * ScrollSpeedBass);
                pChip.ComputeDistanceFromBar(CSoundManager.rcPerformanceTimer.nCurrentTime, this.actScrollSpeed.db現在の譜面スクロール速度);
                if (Math.Min(Math.Min(pChip.nDistanceFromBar.Drums, pChip.nDistanceFromBar.Guitar), pChip.nDistanceFromBar.Bass) > 600)
                {
                    break;
                }
                //				if ( ( ( nCurrentTopChip == this.nCurrentTopChip ) && ( pChip.nDistanceFromBar.Drums < -65 ) ) && pChip.bHit )
                // #28026 2012.4.5 yyagi; 信心ワールドエンドの曲終了後リザルトになかなか行かない問題の修正
                if ((dTX.listChip[this.nCurrentTopChip].nDistanceFromBar.Drums < -65) && dTX.listChip[this.nCurrentTopChip].bHit)
                {
                    //					nCurrentTopChip = ++this.nCurrentTopChip;                    
                    //if (dTX.listChip[this.nCurrentTopChip].bロングノートである)
                    //{
                    //    CChip chipロングノート終端 = dTX.listChip[this.nCurrentTopChip].chipロングノート終端;
                    //    if (chipロングノート終端.bHit && chipロングノート終端.nDistanceFromBar.Drums < -65)
                    //    {
                    //        this.nCurrentTopChip++;
                    //    }
                    //}

                    if (dTX.listChip[this.nCurrentTopChip].bロングノートである)
                    {
                        CChip chipロングノート終端 = dTX.listChip[this.nCurrentTopChip].chipロングノート終端;
                        if (chipロングノート終端.bHit && chipロングノート終端.nDistanceFromBar.Drums < -65)
                        {
                            this.nCurrentTopChip++;
                            continue;
                        }
                    }
                    else
                    {
                        ++this.nCurrentTopChip;
                        continue;
                    }
                }

                bool bPChipIsAutoPlay = bCheckAutoPlay(pChip);

                int nInputAdjustTime = (bPChipIsAutoPlay || (pChip.eInstrumentPart == EInstrumentPart.UNKNOWN)) ? 0 : this.nInputAdjustTimeMs[(int)pChip.eInstrumentPart];

                switch (pChip.nChannelNumber)
                {
                    #region [ 11-1c: ドラム演奏 ]
                    case EChannel.HiHatClose:	// ドラム演奏
                    case EChannel.Snare:
                    case EChannel.BassDrum:
                    case EChannel.HighTom:
                    case EChannel.LowTom:
                    case EChannel.Cymbal:
                    case EChannel.FloorTom:
                    case EChannel.HiHatOpen:
                    case EChannel.RideCymbal:
                    case EChannel.LeftCymbal:
                    case EChannel.LeftPedal:
                    case EChannel.LeftBassDrum:
                        this.tUpdateAndDraw_Chip_PatternOnly_Drums(configIni, ref dTX, ref pChip);
                        break;
                    #endregion
                    #region [ その他(未定義) ]
                    default:
                        //if ( !pChip.bHit && ( pChip.nDistanceFromBar.Drums < 0 ) )
                        {
                            //	pChip.bHit = true;
                        }
                        break;
                    #endregion
                }
            }
            return false;
        }

        public bool bCheckAutoPlay(CChip pChip)
        {
            bool bPChipIsAutoPlay = false;
                                    
            if (pChip.eInstrumentPart == EInstrumentPart.DRUMS)
            {
                bPChipIsAutoPlay = true;
                int offsetIndex = pChip.nChannelNumber - EChannel.HiHatClose;
                if (offsetIndex >= 0 && offsetIndex < this.nチャンネル0Atoレーン07.Length)
                {
                    if (bIsAutoPlay[this.nチャンネル0Atoレーン07[offsetIndex]] == false)
                    {
                        bPChipIsAutoPlay = false;
                    }
                }
            }
            else if(pChip.eInstrumentPart == EInstrumentPart.GUITAR || pChip.eInstrumentPart == EInstrumentPart.BASS)
            {
                //bChipHasButtonArray is array of 5, RGBYP
                bool[] bChipHasButtonArray = EnumConverter.GetArrayBoolFromEChannel(pChip.nChannelNumber);
                bool bGtBsW = pChip.bChipIsWailingNote;
                bool bGtBsO = pChip.bChipIsOpenNote;

                if (pChip.eInstrumentPart == EInstrumentPart.GUITAR)
                {
                    //Trace.TraceInformation( "chip:{0}{1}{2} ", bGtBsR, bGtBsG, bGtBsB );
                    //Trace.TraceInformation( "auto:{0}{1}{2} ", bIsAutoPlay[ (int) ELane.GtR ], bIsAutoPlay[ (int) ELane.GtG ], bIsAutoPlay[ (int) ELane.GtB ]);
                    bPChipIsAutoPlay = true;
                    if (bIsAutoPlay[(int)ELane.GtPick] == false) bPChipIsAutoPlay = false;
                    else
                    {
                        if (bChipHasButtonArray[0] == true && bIsAutoPlay[(int)ELane.GtR] == false) bPChipIsAutoPlay = false;
                        else if (bChipHasButtonArray[1] == true && bIsAutoPlay[(int)ELane.GtG] == false) bPChipIsAutoPlay = false;
                        else if (bChipHasButtonArray[2] == true && bIsAutoPlay[(int)ELane.GtB] == false) bPChipIsAutoPlay = false;
                        else if (bChipHasButtonArray[3] == true && bIsAutoPlay[(int)ELane.GtY] == false) bPChipIsAutoPlay = false;
                        else if (bChipHasButtonArray[4] == true && bIsAutoPlay[(int)ELane.GtP] == false) bPChipIsAutoPlay = false;
                        else if (bGtBsW == true && bIsAutoPlay[(int)ELane.GtW] == false) bPChipIsAutoPlay = false;
                        else if (bGtBsO == true &&
                            (bIsAutoPlay[(int)ELane.GtR] == false || bIsAutoPlay[(int)ELane.GtG] == false || bIsAutoPlay[(int)ELane.GtB] == false || bIsAutoPlay[(int)ELane.GtY] == false || bIsAutoPlay[(int)ELane.GtP] == false))
                            bPChipIsAutoPlay = false;
                    }
                    //Trace.TraceInformation( "{0:x2}: {1}", pChip.nChannelNumber, bPChipIsAutoPlay.ToString() );
                }
                else if (pChip.eInstrumentPart == EInstrumentPart.BASS)
                {
                    bPChipIsAutoPlay = true;
                    if (bIsAutoPlay[(int)ELane.BsPick] == false) bPChipIsAutoPlay = false;
                    else
                    {
                        if (bChipHasButtonArray[0] == true && bIsAutoPlay[(int)ELane.BsR] == false) bPChipIsAutoPlay = false;
                        else if (bChipHasButtonArray[1] == true && bIsAutoPlay[(int)ELane.BsG] == false) bPChipIsAutoPlay = false;
                        else if (bChipHasButtonArray[2] == true && bIsAutoPlay[(int)ELane.BsB] == false) bPChipIsAutoPlay = false;                       
                        else if (bChipHasButtonArray[3] == true && bIsAutoPlay[(int)ELane.BsY] == false) bPChipIsAutoPlay = false;
                        else if (bChipHasButtonArray[4] == true && bIsAutoPlay[(int)ELane.BsP] == false) bPChipIsAutoPlay = false;
                        else if (bGtBsW == true && bIsAutoPlay[(int)ELane.BsW] == false) bPChipIsAutoPlay = false;
                        else if (bGtBsO == true &&
                            (bIsAutoPlay[(int)ELane.BsR] == false || bIsAutoPlay[(int)ELane.BsG] == false || bIsAutoPlay[(int)ELane.BsB] == false || bIsAutoPlay[(int)ELane.BsY] == false || bIsAutoPlay[(int)ELane.BsP] == false))
                            bPChipIsAutoPlay = false;
                    }
                }
            }
             
            return bPChipIsAutoPlay;
        }


        protected abstract void tUpdateAndDraw_Chip_Drums(CConfigIni configIni, ref CDTX dTX, ref CChip pChip);
        protected abstract void tUpdateAndDraw_Chip_PatternOnly_Drums(CConfigIni configIni, ref CDTX dTX, ref CChip pChip);
        //protected abstract void t進行描画_チップ_ギター( CConfigIni configIni, ref CDTX dTX, ref CChip pChip );
        protected abstract void tUpdateAndDraw_Chip_GuitarBass(CConfigIni configIni, ref CDTX dTX, ref CChip pChip, EInstrumentPart inst);  // t進行描画_チップ_ギターベース

        protected void tUpdateAndDraw_Chip_GuitarBass(CConfigIni configIni, ref CDTX dTX, ref CChip pChip, EInstrumentPart inst,  // t進行描画_チップ_ギターベース
            int barYNormal, int barYReverse, int showRangeY0, int showRangeY1, int openXg, int openXb,
            int rectOpenOffsetX, int rectOpenOffsetY, int openChipWidth, int chipHeight,
            int chipWidth, int guitarNormalX, int guitarLeftyX, int bassNormalX, int bassLeftyX, int drawDeltaX, int chipTexDeltaX)
        {
            int instIndex = (int)inst;
            if (configIni.bGuitarEnabled)
            {
                #region [ Hidden/Sudden処理 ]
                #region [ Sudden処理 ]
                if ((CDTXMania.ConfigIni.nHidSud[instIndex] == 2) || (CDTXMania.ConfigIni.nHidSud[instIndex] == 3))
                {
                    if (pChip.nDistanceFromBar[instIndex] < 250)
                    {
                        pChip.bVisible = true;
                        pChip.nTransparency = 0xff;
                    }
                    else if (pChip.nDistanceFromBar[instIndex] < 300)
                    {
                        pChip.bVisible = true;
                        pChip.nTransparency = 0xff - ((int)((((double)(pChip.nDistanceFromBar[instIndex] - 250)) * 255.0) / 75.0));
                    }
                    else
                    {
                        pChip.bVisible = false;
                        pChip.nTransparency = 0;
                    }
                }
                #endregion
                #region [ Hidden処理 ]
                if ((CDTXMania.ConfigIni.nHidSud[instIndex] == 1) || (CDTXMania.ConfigIni.nHidSud[instIndex] == 3))
                {
                    if (pChip.nDistanceFromBar[instIndex] < 150)
                    {
                        pChip.bVisible = false;
                    }
                    else if (pChip.nDistanceFromBar[instIndex] < 200)
                    {
                        pChip.bVisible = true;
                        pChip.nTransparency = (int)((((double)(pChip.nDistanceFromBar[instIndex] - 150)) * 255.0) / 75.0);
                    }
                }
                #endregion
                #region [ ステルス処理 ]
                if (CDTXMania.ConfigIni.nHidSud[instIndex] == 4)
                {
                    pChip.bVisible = false;
                }
                if (this.txChip != null)
                {
                    this.txChip.nTransparency = pChip.nTransparency;
                }
                #endregion
                #endregion

                bool bChipHasR = false;
                bool bChipHasG = false;
                bool bChipHasB = false;
                bool bChipHasY = false;
                bool bChipHasP = false;
                bool bChipHasW = false;
                bool bChipIsO = false;
                EChannel nチャンネル番号 = pChip.nChannelNumber;

                switch (nチャンネル番号)
                {
                    case EChannel.Guitar_Open:
                        bChipIsO = true;
                        break;
                    case EChannel.Guitar_xxBxx:
                        bChipHasB = true;
                        break;
                    case EChannel.Guitar_xGxxx:
                        bChipHasG = true;
                        break;
                    case EChannel.Guitar_xGBxx:
                        bChipHasG = true;
                        bChipHasB = true;
                        break;
                    case EChannel.Guitar_Rxxxx:
                        bChipHasR = true;
                        break;
                    case EChannel.Guitar_RxBxx:
                        bChipHasR = true;
                        bChipHasB = true;
                        break;
                    case EChannel.Guitar_RGxxx:
                        bChipHasR = true;
                        bChipHasG = true;
                        break;
                    case EChannel.Guitar_RGBxx:
                        bChipHasR = true;
                        bChipHasG = true;
                        bChipHasB = true;
                        break;
                    case EChannel.Guitar_Wailing:
                        bChipHasW = true;
                        break;
                    default:
                        switch (nチャンネル番号)
                        {
                            case EChannel.Guitar_xxxYx:
                                bChipHasY = true;
                                break;
                            case EChannel.Guitar_xxBYx:
                                bChipHasB = true;
                                bChipHasY = true;
                                break;
                            case EChannel.Guitar_xGxYx:
                                bChipHasG = true;
                                bChipHasY = true;
                                break;
                            case EChannel.Guitar_xGBYx:
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                break;
                            case EChannel.Guitar_RxxYx:
                                bChipHasR = true;
                                bChipHasY = true;
                                break;
                            case EChannel.Guitar_RxBYx:
                                bChipHasR = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                break;
                            case EChannel.Guitar_RGxYx:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasY = true;
                                break;
                            case EChannel.Guitar_RGBYx:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                break;
                            case EChannel.Guitar_xxxxP:
                                bChipHasP = true;
                                break;
                            case EChannel.Guitar_xxBxP:
                                bChipHasB = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Guitar_xGxxP:
                                bChipHasG = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Guitar_xGBxP:
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Guitar_RxxxP:
                                bChipHasR = true;
                                bChipHasP = true;
                                break;

                            case EChannel.Bass_Open:
                                bChipIsO = true;
                                break;
                            case EChannel.Bass_xxBxx:
                                bChipHasB = true;
                                break;
                            case EChannel.Bass_xGxxx:
                                bChipHasG = true;
                                break;
                            case EChannel.Bass_xGBxx:
                                bChipHasG = true;
                                bChipHasB = true;
                                break;
                            case EChannel.Bass_Rxxxx:
                                bChipHasR = true;
                                break;
                            case EChannel.Bass_RxBxx:
                                bChipHasR = true;
                                bChipHasB = true;
                                break;
                            case EChannel.Bass_RGxxx:
                                bChipHasR = true;
                                bChipHasG = true;
                                break;
                            case EChannel.Bass_RGBxx:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasB = true;
                                break;
                            case EChannel.Bass_Wailing:
                                bChipHasW = true;
                                break;

                            case EChannel.Guitar_RxBxP:
                                bChipHasR = true;
                                bChipHasB = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Guitar_RGxxP:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Guitar_RGBxP:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Guitar_xxxYP:
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Guitar_xxBYP:
                                bChipHasB = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Guitar_xGxYP:
                                bChipHasG = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Guitar_xGBYP:
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Bass_xxxYx:
                                bChipHasY = true;
                                break;
                            case EChannel.Bass_xxBYx:
                                bChipHasB = true;
                                bChipHasY = true;
                                break;

                            case EChannel.Bass_xGxYx:
                                bChipHasG = true;
                                bChipHasY = true;
                                break;
                            case EChannel.Bass_xGBYx:
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                break;
                            case EChannel.Bass_RxxYx:
                                bChipHasR = true;
                                bChipHasY = true;
                                break;
                            case EChannel.Bass_RxBYx:
                                bChipHasR = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                break;
                            case EChannel.Bass_RGxYx:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasY = true;
                                break;
                            case EChannel.Bass_RGBYx:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                break;
                            case EChannel.Bass_xxxxP:
                                bChipHasP = true;
                                break;
                            case EChannel.Bass_xxBxP:
                                bChipHasB = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Guitar_RxxYP:
                                bChipHasR = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Guitar_RxBYP:
                                bChipHasR = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Guitar_RGxYP:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Guitar_RGBYP:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;


                            case EChannel.Bass_xGxxP:
                                bChipHasG = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Bass_xGBxP:
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasP = true;
                                break;

                            case EChannel.Bass_RxxxP:
                                bChipHasR = true;
                                bChipHasP = true;
                                break;

                            case EChannel.Bass_RxBxP:
                                bChipHasR = true;
                                bChipHasB = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Bass_RGxxP:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Bass_RGBxP:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Bass_xxxYP:
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Bass_xxBYP:
                                bChipHasB = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Bass_xGxYP:
                                bChipHasG = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Bass_xGBYP:
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Bass_RxxYP:
                                bChipHasR = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Bass_RxBYP:
                                bChipHasR = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Bass_RGxYP:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case EChannel.Bass_RGBYP:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                        }
                        break;
                }

                #region [ chip描画 ]
                int OPEN = (inst == EInstrumentPart.GUITAR) ? 10 : 10;
                //if (!pChip.bHit && pChip.bVisible)
                if ((!pChip.bHit || pChip.bロングノートである) && pChip.bVisible)
                {
                    int yBarPos = configIni.bReverse[instIndex] ? barYReverse : barYNormal;
                    int y = configIni.bReverse[instIndex] ? (barYReverse - pChip.nDistanceFromBar[instIndex]) : (barYNormal + pChip.nDistanceFromBar[instIndex]);

                    //
                    int num3 = 0;
                    if (pChip.bロングノートである)
                    {
                        if (pChip.chipロングノート終端.nDistanceFromBar[(int)inst] <= 0)
                        {
                            return;
                        }
                        num3 = pChip.chipロングノート終端.nDistanceFromBar[(int)inst] - pChip.nDistanceFromBar[(int)inst];
                        if (pChip.bHit && pChip.bロングノートHit中)
                        {
                            y = yBarPos;
                            num3 = pChip.chipロングノート終端.nDistanceFromBar[(int)inst];
                        }

                    }

                    //if ((showRangeY0 < y) && (y < showRangeY1))
                    {
                        if (this.txChip != null)
                        {
                            int nアニメカウンタ現在の値 = this.ctChipPatternAnimation[instIndex].nCurrentValue;
                            if (bChipIsO)
                            {
                                this.txChip.vcScaleRatio.Y = 1f;
                                int xo = (inst == EInstrumentPart.GUITAR) ? 88 : 959;
                                this.txChip.tDraw2D(CDTXMania.app.Device, xo, y - 2, new Rectangle(0, 10, 196, 10));
                            }
                            Rectangle rc = new Rectangle(rectOpenOffsetX, chipHeight, chipWidth, 10);
                            int x;
                            if (inst == EInstrumentPart.GUITAR)
                            {
                                x = (configIni.bLeft.Guitar) ? guitarLeftyX : guitarNormalX;
                            }
                            else
                            {
                                x = (configIni.bLeft.Bass) ? bassLeftyX : bassNormalX;
                            }
                            int deltaX = (configIni.bLeft[instIndex]) ? -drawDeltaX : +drawDeltaX;

                            

                            //Refactored code for drawing
                            int[] nChipXPos = {
                                inst == EInstrumentPart.GUITAR ? 88 : 959,
                                inst == EInstrumentPart.GUITAR ? 127 : 998,
                                inst == EInstrumentPart.GUITAR ? 166 : 1036,
                                inst == EInstrumentPart.GUITAR ? 205 : 1076,
                                inst == EInstrumentPart.GUITAR ? 244 : 1115
                            };
                            
                            if(inst == EInstrumentPart.GUITAR && CDTXMania.ConfigIni.bLeft.Guitar)
                            {
                                Array.Reverse(nChipXPos);
                            }

                            if (inst == EInstrumentPart.BASS && CDTXMania.ConfigIni.bLeft.Bass)
                            {
                                Array.Reverse(nChipXPos);
                            }

                            Rectangle[] rChipTxRectArray = {
                            new Rectangle(0, 0, 38, 10),
                            new Rectangle(38, 0, 38, 10),
                            new Rectangle(76, 0, 38, 10),
                            new Rectangle(114, 0, 38, 10),
                            new Rectangle(152, 0, 38, 10)
                            };

                            bool[] bChipColorFlags = {
                            bChipHasR,
                            bChipHasG,
                            bChipHasB,
                            bChipHasY,
                            bChipHasP
                            };

                            for (int i = 0; i < bChipColorFlags.Length; i++)
                            {
                                if (bChipColorFlags[i])
                                {
                                    if(inst == EInstrumentPart.GUITAR || inst == EInstrumentPart.BASS)
                                    {
                                        int num8 = nChipXPos[i];
                                        Rectangle rect1 = rChipTxRectArray[i];
                                        //this.txChip.tDraw2D(CDTXMania.app.Device, num8, y - chipHeight / 2, rect1);
                                        this.txChip.vcScaleRatio.Y = 1f;
                                        if (!pChip.bHit)
                                        {
                                            this.txChip.nTransparency = pChip.nTransparency;
                                            this.txChip.tDraw2D(CDTXMania.app.Device, num8, y - chipHeight / 2, rect1);
                                        }
                                        if (pChip.bロングノートである)
                                        {
                                            //_ = (bool)CDTXMania.Instance.ConfigIni.bReverse[inst];
                                            Rectangle rectangle2 = rect1;
                                            rectangle2.Y += 3;
                                            rectangle2.Height = 5;
                                            this.txChip.nTransparency = 128;
                                            if (pChip.bHit && !pChip.bロングノートHit中)                                            
                                            {
                                                CTexture obj = txChip;
                                                obj.nTransparency = obj.nTransparency / 2;
                                            }
                                            this.txChip.vcScaleRatio.Y = 1f * (float)num3 / (float)rectangle2.Height;
                                            this.txChip.tDraw2D(CDTXMania.app.Device, num8, y - (CDTXMania.ConfigIni.bReverse[(int)inst] ? num3 : 0), rectangle2);
                                        }
                                    }
                                }
                            }
                            //Trace.TraceInformation( "chip={0:x2}, EInstrumentPart={1}, x={2}", pChip.nChannelNumber, inst, x );
                         }
                    }                   
                }

                #endregion
                //if ( ( configIni.bAutoPlay.Guitar && !pChip.bHit ) && ( pChip.nDistanceFromBar.Guitar < 0 ) )
                //if ( ( !pChip.bHit ) && ( pChip.nDistanceFromBar[ instIndex ] < 0 ) )

                // #35411 2015.08.20 chnmr0 modified
                // 従来のAUTO処理に加えてプレーヤーゴーストの再生機能を追加
                bool autoPlayCondition = (!pChip.bHit) && (pChip.nDistanceFromBar[instIndex] < 0);
				if ( autoPlayCondition )
                {
                    //cInvisibleChip.StartSemiInvisible( inst );
                }

                bool autoPick = ( inst == EInstrumentPart.GUITAR ) ? bIsAutoPlay.GtPick : bIsAutoPlay.BsPick;
                autoPlayCondition = !pChip.bHit && autoPick;
                long ghostLag = 0;
                bool bUsePerfectGhost = true;

                if ( (pChip.eInstrumentPart == EInstrumentPart.GUITAR || pChip.eInstrumentPart == EInstrumentPart.BASS ) &&
                    CDTXMania.ConfigIni.eAutoGhost[(int)(pChip.eInstrumentPart)] != EAutoGhostData.PERFECT &&
                    CDTXMania.listAutoGhostLag[(int)pChip.eInstrumentPart] != null &&
                    0 <= pChip.n楽器パートでの出現順 &&
                    pChip.n楽器パートでの出現順 < CDTXMania.listAutoGhostLag[(int)pChip.eInstrumentPart].Count)
                {
                	// #35411 (mod) Ghost data が有効なので 従来のAUTOではなくゴーストのラグを利用
                	// 発生時刻と現在時刻からこのタイミングで演奏するかどうかを決定
					ghostLag = CDTXMania.listAutoGhostLag[(int)pChip.eInstrumentPart][pChip.n楽器パートでの出現順];
					bool resetCombo = ghostLag > 255;
					ghostLag = (ghostLag & 255) - 128;
					ghostLag -= (pChip.eInstrumentPart == EInstrumentPart.GUITAR ? nInputAdjustTimeMs.Guitar : nInputAdjustTimeMs.Bass);
                    autoPlayCondition &= (pChip.nPlaybackTimeMs + ghostLag <= CSoundManager.rcPerformanceTimer.n現在時刻ms);
					if (resetCombo && autoPlayCondition )
					{
						this.actCombo.nCurrentCombo[(int)pChip.eInstrumentPart] = 0;
					}
					bUsePerfectGhost = false;
                }

                if( bUsePerfectGhost )
                {
                	// 従来のAUTOを使用する場合
                    autoPlayCondition &= ( pChip.nDistanceFromBar[ instIndex ] < 0 );
                }

                if ( autoPlayCondition )
                {
                    int lo = (inst == EInstrumentPart.GUITAR) ? 0 : 5;	// lane offset
                    bool autoR = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtR : bIsAutoPlay.BsR;
                    bool autoG = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtG : bIsAutoPlay.BsG;
                    bool autoB = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtB : bIsAutoPlay.BsB;
                    bool autoY = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtY : bIsAutoPlay.BsY;
                    bool autoP = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtP : bIsAutoPlay.BsP;
                    bool pushingR = CDTXMania.Pad.bPressing(inst, EPad.R);
                    bool pushingG = CDTXMania.Pad.bPressing(inst, EPad.G);
                    bool pushingB = CDTXMania.Pad.bPressing(inst, EPad.B);
                    bool pushingY = CDTXMania.Pad.bPressing(inst, EPad.Y);
                    bool pushingP = CDTXMania.Pad.bPressing(inst, EPad.P);

					#region [ Chip Fire effects (auto時用) ]
                    // autoPickでない時の処理は、 tHandleInput_GuitarBass(EInstrumentPart) で行う
					bool bSuccessOPEN = bChipIsO && ( autoR || !pushingR ) && ( autoG || !pushingG ) && ( autoB || !pushingB ) && ( autoY || !pushingY ) && ( autoP || !pushingP );
					if ( ( bChipHasR && ( autoR || pushingR ) && autoPick ) || bSuccessOPEN && autoPick )
					{
						this.actChipFireGB.Start( 0 + lo );
					}
					if ( ( bChipHasG && ( autoG || pushingG ) && autoPick ) || bSuccessOPEN && autoPick )
					{
						this.actChipFireGB.Start( 1 + lo );
					}
					if ( ( bChipHasB && ( autoB || pushingB ) && autoPick ) || bSuccessOPEN && autoPick )
					{
						this.actChipFireGB.Start( 2 + lo );
					}
                    if ( ( bChipHasY && ( autoY || pushingY ) && autoPick ) || bSuccessOPEN && autoPick )
					{
						this.actChipFireGB.Start( 3 + lo );
					}
                    if ( ( bChipHasP && ( autoP || pushingP ) && autoPick ) || bSuccessOPEN && autoPick )
					{
						this.actChipFireGB.Start( 4 + lo );
					}
					#endregion
					#region [ autopick ]
					if ( autoPick )
					{
						bool bMiss = true;
						if ( bChipHasR == autoR && bChipHasG == autoG && bChipHasB == autoB && bChipHasY == autoY && bChipHasP == autoP )		// autoレーンとチップレーン一致時はOK
						{																			// この条件を加えないと、同時に非autoレーンを押下している時にNGとなってしまう。
							bMiss = false;
						}
						else if ( ( autoR || ( bChipHasR == pushingR ) ) && ( autoG || ( bChipHasG == pushingG ) ) && ( autoB || ( bChipHasB == pushingB ) ) && ( autoY || ( bChipHasY == pushingY ) ) && ( autoP || bChipHasP == pushingP ) )
							// ( bChipHasR == ( pushingR | autoR ) ) && ( bChipHasG == ( pushingG | autoG ) ) && ( bChipHasB == ( pushingB | autoB ) ) )
						{
							bMiss = false;
						}
						else if ( ( ( bChipIsO == true ) && ( !pushingR | autoR ) && ( !pushingG | autoG ) && ( !pushingB | autoB ) && ( !pushingY | autoY) && ( !pushingP | autoP) ) )	// OPEN時
						{
							bMiss = false;
						}
                        bool bCurrInstrumentSpecialist = (inst == EInstrumentPart.GUITAR) ? CDTXMania.ConfigIni.bSpecialist.Guitar : CDTXMania.ConfigIni.bSpecialist.Bass;
                        pChip.bHit = true;
						this.tPlaySound( pChip, CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻 + pChip.nPlaybackTimeMs + ghostLag, inst, dTX.nモニタを考慮した音量( inst ), false, bMiss && bCurrInstrumentSpecialist);
						this.rNextGuitarChip = null;
						if ( !bMiss )
						{
							this.tProcessChipHit( pChip.nPlaybackTimeMs + ghostLag, pChip );
						}
						else
						{
							pChip.nLag = 0;		// tProcessChipHit()の引数最後がfalseの時はpChip.nLagを計算しないため、ここでAutoPickかつMissのLag=0を代入
							this.tProcessChipHit( pChip.nPlaybackTimeMs + ghostLag, pChip, false );
						}

						//int chWailingChip = ( inst == EInstrumentPart.GUITAR ) ? (int)EChannel.Guitar_Wailing : (int)EChannel.Bass_Wailing;
						//CChip item = this.r指定時刻に一番近い未ヒットChip( pChip.nPlaybackTimeMs + ghostLag, chWailingChip, this.nInputAdjustTimeMs[ instIndex ], 140 );

                        //New method for Guitar and Bass
                        EChannel search = ((inst == EInstrumentPart.GUITAR) ? EChannel.Guitar_Wailing : EChannel.Bass_Wailing);
                        CChip item = r指定時刻に一番近いChip(pChip.nPlaybackTimeMs + ghostLag, search, this.nInputAdjustTimeMs[instIndex], 140);

                        if ( item != null && !bMiss )
						{
							this.queWailing[ instIndex ].Enqueue( item );
						}
					}
					#endregion

					// #35411 modify end
				}

                if( pChip.eInstrumentPart == EInstrumentPart.GUITAR && CDTXMania.ConfigIni.bGraph有効.Guitar )
                {
                    #region[ ギターゴースト ]
                    if (CDTXMania.ConfigIni.eTargetGhost.Guitar != ETargetGhostData.NONE &&
                        CDTXMania.listTargetGhsotLag.Guitar != null)
                    {
                        double val = 0;
                        if (CDTXMania.ConfigIni.eTargetGhost.Guitar == ETargetGhostData.ONLINE)
                        {
                            if (CDTXMania.DTX.nVisibleChipsCount.Guitar > 0)
                            {
                            	// Online Stats の計算式
                                val = 100 *
                                    (this.nヒット数_TargetGhost.Guitar.Perfect * 17 +
                                     this.nヒット数_TargetGhost.Guitar.Great * 7 +
                                     this.n最大コンボ数_TargetGhost.Guitar * 3) / (20.0 * CDTXMania.DTX.nVisibleChipsCount.Guitar);
                            }
                        }
                        else
                        {
                            if( CDTXMania.ConfigIni.nSkillMode == 0 )
                            {
                                val = CScoreIni.tCalculatePlayingSkillOld(
                                    CDTXMania.DTX.nVisibleChipsCount.Guitar,
                                    this.nヒット数_TargetGhost.Guitar.Perfect,
                                    this.nヒット数_TargetGhost.Guitar.Great,
                                    this.nヒット数_TargetGhost.Guitar.Good,
                                    this.nヒット数_TargetGhost.Guitar.Poor,
                                    this.nヒット数_TargetGhost.Guitar.Miss,
                                    this.n最大コンボ数_TargetGhost.Guitar,
                                    EInstrumentPart.GUITAR, new STAUTOPLAY());
                            }
                            else
                            {
                                val = CScoreIni.tCalculatePlayingSkill(
                                    CDTXMania.DTX.nVisibleChipsCount.Guitar,
                                    this.nヒット数_TargetGhost.Guitar.Perfect,
                                    this.nヒット数_TargetGhost.Guitar.Great,
                                    this.nヒット数_TargetGhost.Guitar.Good,
                                    this.nヒット数_TargetGhost.Guitar.Poor,
                                    this.nヒット数_TargetGhost.Guitar.Miss,
                                    this.n最大コンボ数_TargetGhost.Guitar,
                                    EInstrumentPart.GUITAR, new STAUTOPLAY());
                            }

                        }
                        if (val < 0) val = 0;
                        if (val > 100) val = 100;
                        this.actGraph.dbGraphValue_Goal = val;
                    }
                    #endregion
                }
                else if( pChip.eInstrumentPart == EInstrumentPart.BASS && CDTXMania.ConfigIni.bGraph有効.Bass )
                {
                    #region[ ベースゴースト ]
                    if (CDTXMania.ConfigIni.eTargetGhost.Bass != ETargetGhostData.NONE &&
                        CDTXMania.listTargetGhsotLag.Bass != null)
                    {
                        double val = 0;
                        if (CDTXMania.ConfigIni.eTargetGhost.Bass == ETargetGhostData.ONLINE)
                        {
                            if (CDTXMania.DTX.nVisibleChipsCount.Bass > 0)
                            {
                            	// Online Stats の計算式
                                val = 100 *
                                    (this.nヒット数_TargetGhost.Bass.Perfect * 17 +
                                     this.nヒット数_TargetGhost.Bass.Great * 7 +
                                     this.n最大コンボ数_TargetGhost.Bass * 3) / (20.0 * CDTXMania.DTX.nVisibleChipsCount.Bass);
                            }
                        }
                        else
                        {
                            if( CDTXMania.ConfigIni.nSkillMode == 0 )
                            {
                                val = CScoreIni.tCalculatePlayingSkillOld(
                                    CDTXMania.DTX.nVisibleChipsCount.Bass,
                                    this.nヒット数_TargetGhost.Bass.Perfect,
                                    this.nヒット数_TargetGhost.Bass.Great,
                                    this.nヒット数_TargetGhost.Bass.Good,
                                    this.nヒット数_TargetGhost.Bass.Poor,
                                    this.nヒット数_TargetGhost.Bass.Miss,
                                    this.n最大コンボ数_TargetGhost.Bass,
                                    EInstrumentPart.BASS, new STAUTOPLAY());
                            }
                            else
                            {
                                val = CScoreIni.tCalculatePlayingSkill(
                                    CDTXMania.DTX.nVisibleChipsCount.Bass,
                                    this.nヒット数_TargetGhost.Bass.Perfect,
                                    this.nヒット数_TargetGhost.Bass.Great,
                                    this.nヒット数_TargetGhost.Bass.Good,
                                    this.nヒット数_TargetGhost.Bass.Poor,
                                    this.nヒット数_TargetGhost.Bass.Miss,
                                    this.n最大コンボ数_TargetGhost.Bass,
                                    EInstrumentPart.BASS, new STAUTOPLAY());
                            }

                        }
                        if (val < 0) val = 0;
                        if (val > 100) val = 100;
                        this.actGraph.dbGraphValue_Goal = val;
                    }
                    #endregion
                }

				return;
			}	// end of "if configIni.bGuitarEnabled"
			if ( !pChip.bHit && ( pChip.nDistanceFromBar[ instIndex ] < 0 ) )	// Guitar/Bass無効の場合は、自動演奏する
			{
				pChip.bHit = true;
				this.tPlaySound( pChip, CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻 + pChip.nPlaybackTimeMs, inst, dTX.nモニタを考慮した音量( inst ) );
			}
        }


        protected virtual void tUpdateAndDraw_Chip_GuitarBass_Wailing(CConfigIni configIni, ref CDTX dTX, ref CChip pChip, EInstrumentPart inst)
        {
            int indexInst = (int)inst;
            if (configIni.bGuitarEnabled)
            {
                #region [ Hidden/Sudden処理 ]
                #region [ Sudden処理 ]
                if ((CDTXMania.ConfigIni.nHidSud[indexInst] == 2) || (CDTXMania.ConfigIni.nHidSud[indexInst] == 3))
                {
                    if (pChip.nDistanceFromBar[indexInst] < 250)
                    {
                        pChip.bVisible = true;
                        pChip.nTransparency = 0xff;
                    }
                    else if (pChip.nDistanceFromBar[indexInst] < 300)
                    {
                        pChip.bVisible = true;
                        pChip.nTransparency = 0xff - ((int)((((double)(pChip.nDistanceFromBar[indexInst] - 250)) * 255.0) / 75.0));
                    }
                    else
                    {
                        pChip.bVisible = false;
                        pChip.nTransparency = 0;
                    }
                }
                #endregion
                #region [ Hidden処理 ]
                if ((CDTXMania.ConfigIni.nHidSud[indexInst] == 1) || (CDTXMania.ConfigIni.nHidSud[indexInst] == 3))
                {
                    if (pChip.nDistanceFromBar[indexInst] < 150)
                    {
                        pChip.bVisible = false;
                    }
                    else if (pChip.nDistanceFromBar[indexInst] < 200)
                    {
                        pChip.bVisible = true;
                        pChip.nTransparency = (int)((((double)(pChip.nDistanceFromBar[indexInst] - 150)) * 255.0) / 75.0);
                    }
                }
                #endregion
                #region [ ステルス処理 ]
                if (CDTXMania.ConfigIni.nHidSud[indexInst] == 4)
                {
                    pChip.bVisible = false;
                }
                if (this.txChip != null)
                {
                    this.txChip.nTransparency = pChip.nTransparency;
                }
                #endregion
                #endregion
                //
                // ここにチップ更新処理が入る(overrideで入れる)。といっても座標とチップサイズが違うだけで処理はまるまる同じ。
                //
                if (!pChip.bHit && (pChip.nDistanceFromBar[indexInst] < 0))
                {
                    if (pChip.nDistanceFromBar[indexInst] < -234)	// #25253 2011.5.29 yyagi: Don't set pChip.bHit=true for wailing at once. It need to 1sec-delay (234pix per 1sec). 
                    {
                        pChip.bHit = true;
                    }
                    bool autoW = (inst == EInstrumentPart.GUITAR) ? configIni.bAutoPlay.GtW : configIni.bAutoPlay.BsW;
                    //if ( configIni.bAutoPlay[ ((int) ELane.Guitar - 1) + indexInst ] )	// このような、バグの入りやすい書き方(GT/BSのindex値が他と異なる)はいずれ見直したい
                    if (autoW)
                    {
                        //    pChip.bHit = true;								// #25253 2011.5.29 yyagi: Set pChip.bHit=true if autoplay.
                        //    this.actWailingBonus.Start( inst, this.r現在の歓声Chip[indexInst] );
                        // #23886 2012.5.22 yyagi; To support auto Wailing; Don't do wailing for ALL wailing chips. Do wailing for queued wailing chip.
                        // wailing chips are queued when 1) manually wailing and not missed at that time 2) AutoWailing=ON and not missed at that time
                        long nTimeStamp_Wailed = pChip.nPlaybackTimeMs + CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻;
                        DoWailingFromQueue(inst, nTimeStamp_Wailed, autoW);
                    }
                }
                return;
            }
            pChip.bHit = true;
        }
        protected virtual void tUpdateAndDraw_Chip_Guitar_Wailing(CConfigIni configIni, ref CDTX dTX, ref CChip pChip)
        {
            tUpdateAndDraw_Chip_GuitarBass_Wailing(configIni, ref dTX, ref pChip, EInstrumentPart.GUITAR);
        }
        protected abstract void tUpdateAndDraw_Chip_FillIn(CConfigIni configIni, ref CDTX dTX, ref CChip pChip);  // t進行描画_チップ_フィルイン
        protected abstract void tUpdateAndDraw_Chip_Bonus(CConfigIni configIni, ref CDTX dTX, ref CChip pChip);
        protected void tUpdateAndDraw_FillInEffect()  // t進行描画_フィルインエフェクト
        {
            this.actFillin.OnUpdateAndDraw();
        }
        protected abstract void tUpdateAndDraw_Chip_BarLine(CConfigIni configIni, ref CDTX dTX, ref CChip pChip);
        protected abstract void tDraw_LoopLine(CConfigIni configIni, bool bIsEnd);
        //protected abstract void t進行描画_チップ_ベース( CConfigIni configIni, ref CDTX dTX, ref CChip pChip );
        protected virtual void tUpdateAndDraw_Chip_Bass_Wailing(CConfigIni configIni, ref CDTX dTX, ref CChip pChip)  // t進行描画_チップ_ベース_ウェイリング
        {
            tUpdateAndDraw_Chip_GuitarBass_Wailing(configIni, ref dTX, ref pChip, EInstrumentPart.BASS);
        }
        protected abstract void tUpdateAndDraw_Chip_NoSound_Drums(CConfigIni configIni, ref CDTX dTX, ref CChip pChip);  // t進行描画_チップ_空打ち音設定_ドラム
        protected void tUpdateAndDraw_ChipAnimation()
        {
            for (int i = 0; i < 3; i++)			// 0=drums, 1=guitar, 2=bass
            {
                if (this.ctChipPatternAnimation[i] != null)
                {
                    this.ctChipPatternAnimation[i].tUpdateLoop();
                }
            }
            if (this.ctWailingChipPatternAnimation != null)
            {
                this.ctWailingChipPatternAnimation.tUpdateLoop();
            }
            if (this.ctBPMBar != null)
            {
                this.ctBPMBar.tUpdateLoop();
                this.ctComboTimer.tUpdateLoop();
            }
            if (CDTXMania.ConfigIni.bDrumsEnabled)  //2013.05.16.kairera0467 ギター側のアニメーションは未実装なのでとりあえず。
                this.ct登場用.tUpdate();
        }

        protected bool tUpdateAndDraw_FadeIn_Out()
        {
            switch (base.ePhaseID)
            {
                case CStage.EPhase.Common_FadeIn:
                    if (this.actFI.OnUpdateAndDraw() != 0)
                    {
                        base.ePhaseID = CStage.EPhase.Common_DefaultState;
                    }
                    break;

                case CStage.EPhase.Common_FadeOut:
                case CStage.EPhase.演奏_STAGE_FAILED_フェードアウト:
                    if (this.actFO.OnUpdateAndDraw() != 0)
                    {
                        return true;
                    }
                    break;

                case CStage.EPhase.演奏_STAGE_CLEAR_フェードアウト:
                    if (this.actFOStageClear.OnUpdateAndDraw() == 0)
                    {
                        break;
                    }
                    return true;

            }
            return false;
        }
        protected void tUpdateAndDraw_LaneFlushD()
        {
            if ((base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED) && (base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED_フェードアウト))
            {
                this.actLaneFlushD.OnUpdateAndDraw();
            }
        }
        protected void tUpdateAndDraw_LaneFlushGB()
        {
            if (CDTXMania.ConfigIni.bGuitarEnabled)
            {
                this.actLaneFlushGB.OnUpdateAndDraw();
            }
        }
        protected abstract void tUpdateAndDraw_PerformanceInformation();
        protected void tUpdateAndDraw_PerformanceInformation(int x, int y)
        {
            if (!CDTXMania.ConfigIni.b演奏情報を表示しない)
            {
                this.actPlayInfo.tUpdateAndDraw(x, y);
            }
        }
        protected void tUpdateAndDraw_Background()
        {
            //Draw either Background image or video
            if (this.bGenericVideoEnabled) {
                this.actBackgroundAVI.tUpdateAndDraw();
            }
            else if (this.tx背景 != null)
            {
                this.tx背景.tDraw2D(CDTXMania.app.Device, 0, 0);
            }
            //CDTXMania.app.Device.Clear( ClearFlags.ZBuffer | ClearFlags.Target, Color.Black, 0f, 0 );
        }

        protected void tUpdateAndDraw_JudgementLine()  // t進行描画_判定ライン
        {
            if (CDTXMania.ConfigIni.bDrumsEnabled)
            {
                int y = CDTXMania.ConfigIni.bReverse.Drums ? nJudgeLinePosY.Drums - nJudgeLinePosY_delta.Drums : nJudgeLinePosY.Drums + nJudgeLinePosY_delta.Drums;
                if (CDTXMania.ConfigIni.bJudgeLineDisp.Drums)
                {
                    // #31602 2013.6.23 yyagi 描画遅延対策として、判定ラインの表示位置をオフセット調整できるようにする
                    // Check for numofLanes config to decide on the length to draw
                    int l_drumPanelWidth = 0x22f;
                    int l_xOffset = 0;
                    if (CDTXMania.ConfigIni.eNumOfLanes.Drums == EType.B)
                    {
                        l_drumPanelWidth = 0x207;
                    }
                    else if (CDTXMania.ConfigIni.eNumOfLanes.Drums == EType.C)
                    {
                        l_drumPanelWidth = 447;
                        l_xOffset = 72;
                    }
                    this.txHitBar.tDraw2D(CDTXMania.app.Device, 295 + l_xOffset, y, new Rectangle(0, 0, l_drumPanelWidth, 6));
                }
                if (CDTXMania.ConfigIni.b演奏情報を表示する)
                    this.actLVFont.tDrawString(295, (CDTXMania.ConfigIni.bReverse.Drums ? y - 20 : y + 8), CDTXMania.ConfigIni.nJudgeLine.Drums.ToString());
            }
        }

        protected void tUpdateAndDraw_JudgementString()  // t進行描画_判定文字列
        {
            this.actJudgeString.OnUpdateAndDraw();
        }
        protected void tUpdateAndDraw_JudgementString1_ForNormalPosition()  // t進行描画_判定文字列1_通常位置指定の場合
        {
            if (((EType)CDTXMania.ConfigIni.JudgementStringPosition.Drums) != EType.B)
            {
                this.actJudgeString.OnUpdateAndDraw();
            }
        }
        protected void tUpdateAndDraw_JudgementString2_ForPositionOnJudgementLine()  // t進行描画_判定文字列2_判定ライン上指定の場合
        {
            if (((EType)CDTXMania.ConfigIni.JudgementStringPosition.Drums) == EType.B)
            {
                this.actJudgeString.OnUpdateAndDraw();
            }
        }

        protected void tUpdateAndDraw_ScrollSpeed()
        {
            this.actScrollSpeed.OnUpdateAndDraw();
        }

        protected abstract void tGenerateBackgroundTexture();



        protected void tGenerateBackgroundTexture(string DefaultBgFilename, Rectangle bgrect, string bgfilename)
        {
            Bitmap image = null;
            bool flag = true;

            if (bgfilename != null && File.Exists(bgfilename))
            {
                try
                {
                    Bitmap bitmap2 = null;
                    bitmap2 = new Bitmap(bgfilename);
                    if ((bitmap2.Size.Width == 0) && (bitmap2.Size.Height == 0))
                    {
                        this.tx背景 = null;
                        return;
                    }
                    Bitmap bitmap3 = new Bitmap(SampleFramework.GameWindowSize.Width, SampleFramework.GameWindowSize.Height);
                    Graphics graphics = Graphics.FromImage(bitmap3);
                    for (int i = 0; i < SampleFramework.GameWindowSize.Height; i += bitmap2.Size.Height)
                    {
                        for (int j = 0; j < SampleFramework.GameWindowSize.Width; j += bitmap2.Size.Width)
                        {
                            graphics.DrawImage(bitmap2, j, i, bitmap2.Width, bitmap2.Height);
                        }
                    }
                    graphics.Dispose();
                    bitmap2.Dispose();
                    image = new Bitmap(CSkin.Path(DefaultBgFilename));
                    graphics = Graphics.FromImage(image);
                    ColorMatrix matrix2 = new ColorMatrix();
                    matrix2.Matrix00 = 1f;
                    matrix2.Matrix11 = 1f;
                    matrix2.Matrix22 = 1f;
                    matrix2.Matrix33 = ((float)CDTXMania.ConfigIni.nBackgroundTransparency) / 255f;
                    matrix2.Matrix44 = 1f;
                    ColorMatrix newColorMatrix = matrix2;
                    ImageAttributes imageAttr = new ImageAttributes();
                    imageAttr.SetColorMatrix(newColorMatrix);
                    graphics.DrawImage(bitmap3, new Rectangle(0, 0, SampleFramework.GameWindowSize.Width, SampleFramework.GameWindowSize.Height), 0, 0, SampleFramework.GameWindowSize.Width, SampleFramework.GameWindowSize.Height, GraphicsUnit.Pixel, imageAttr);
                    imageAttr.Dispose();
                    graphics.DrawImage(bitmap3, bgrect, bgrect.X, bgrect.Y, bgrect.Width, bgrect.Height, GraphicsUnit.Pixel);
                    graphics.Dispose();
                    bitmap3.Dispose();
                    flag = false;
                }
                catch
                {
                    Trace.TraceError("背景画像の読み込みに失敗しました。({0})", new object[] { bgfilename });
                }
            }
            if (flag)
            {
                bgfilename = CSkin.Path(DefaultBgFilename);
                try
                {
                    image = new Bitmap(bgfilename);
                }
                catch
                {
                    Trace.TraceError("背景画像の読み込みに失敗しました。({0})", new object[] { bgfilename });
                    this.tx背景 = null;
                    return;
                }
            }
            if ((CDTXMania.DTX.listBMP.Count > 0) || (CDTXMania.DTX.listBMPTEX.Count > 0))
            {
                Graphics graphics2 = Graphics.FromImage(image);
                graphics2.FillRectangle(Brushes.Black, bgrect.X, bgrect.Y, bgrect.Width, bgrect.Height);
                graphics2.Dispose();
            }
            try
            {
                this.tx背景 = new CTexture(CDTXMania.app.Device, image, CDTXMania.TextureFormat);
            }
            catch (CTextureCreateFailedException)
            {
                Trace.TraceError("背景テクスチャの生成に失敗しました。");
                this.tx背景 = null;
            }
            image.Dispose();
        }

        protected virtual void tHandleInput_Guitar()
        {
            tHandleInput_GuitarBass(EInstrumentPart.GUITAR);
        }
        protected virtual void tHandleInput_Bass()
        {
            tHandleInput_GuitarBass(EInstrumentPart.BASS);
        }


        protected virtual void tHandleInput_GuitarBass(EInstrumentPart inst)
        {
            bool bCurrInstrumentSpecialist = (inst == EInstrumentPart.GUITAR) ? CDTXMania.ConfigIni.bSpecialist.Guitar : CDTXMania.ConfigIni.bSpecialist.Bass;
            int indexInst = (int)inst;
            #region [ スクロール速度変更 ]
            if (CDTXMania.Pad.bPressing(inst, EPad.Decide) && CDTXMania.Pad.bPressed(inst, EPad.B))
            {
                CDTXMania.ConfigIni.nScrollSpeed[indexInst] = Math.Min(CDTXMania.ConfigIni.nScrollSpeed[indexInst] + 1, 0x7cf);
            }
            if (CDTXMania.Pad.bPressing(inst, EPad.Decide) && CDTXMania.Pad.bPressed(inst, EPad.R))
            {
                CDTXMania.ConfigIni.nScrollSpeed[indexInst] = Math.Max(CDTXMania.ConfigIni.nScrollSpeed[indexInst] - 1, 0);
            }
            #endregion

            if (!CDTXMania.ConfigIni.bGuitarEnabled || !CDTXMania.DTX.bチップがある[indexInst])
            {
                return;
            }

            int R = (inst == EInstrumentPart.GUITAR) ? 0 : 5;
            int G = R + 1;
            int B = R + 2;
            int Y = R + 3;
            int P = R + 4;
            bool autoW = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtW : bIsAutoPlay.BsW;
            bool autoR = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtR : bIsAutoPlay.BsR;
            bool autoG = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtG : bIsAutoPlay.BsG;
            bool autoB = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtB : bIsAutoPlay.BsB;
            bool autoY = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtY : bIsAutoPlay.BsY;
            bool autoP = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtP : bIsAutoPlay.BsP;
            bool autoPick = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtPick : bIsAutoPlay.BsPick;
            int nAutoW = (autoW) ? 8 : 0;
            int nAutoR = (autoR) ? 4 : 0;
            int nAutoG = (autoG) ? 2 : 0;
            int nAutoB = (autoB) ? 1 : 0;
            int nAutoY = (autoY) ? 16 : 0;
            int nAutoP = (autoP) ? 32 : 0;
            int nAutoMask = nAutoW | nAutoR | nAutoG | nAutoB | nAutoY | nAutoP;

            //			if ( bIsAutoPlay[ (int) ELane.Guitar - 1 + indexInst ] )	// このような、バグの入りやすい書き方(GT/BSのindex値が他と異なる)はいずれ見直したい
            //			{
            //CChip chip = this.r次に来る指定楽器Chipを更新して返す(inst);
            CChip chip = ((chipロングノートHit中[(int)inst] == null) ? r次に来る指定楽器Chipを更新して返す(inst) : chipロングノートHit中[(int)inst]);
            int nChipColorFlag = 0;
            bool bChipColorHasR = false;
            bool bChipColorHasG = false;
            bool bChipColorHasB = false;
            bool bChipColorHasY = false;
            bool bChipColorHasP = false;
            if (chip != null)
            {
                bool bAutoGuitarR = false;
                bool bAutoGuitarG = false;
                bool bAutoGuitarB = false;
                bool bAutoGuitarY = false;
                bool bAutoGuitarP = false;
                bool bAutoBassR = false;
                bool bAutoBassG = false;
                bool bAutoBassB = false;
                bool bAutoBassY = false;
                bool bAutoBassP = false;

                switch (chip.nChannelNumber)
                {
                    case EChannel.Guitar_Open:
                        break;
                    case EChannel.Guitar_xxBxx:
                        bAutoGuitarB = true;
                        break;
                    case EChannel.Guitar_xGxxx:
                        bAutoGuitarG = true;
                        break;
                    case EChannel.Guitar_xGBxx:
                        bAutoGuitarG = true;
                        bAutoGuitarB = true;
                        break;
                    case EChannel.Guitar_Rxxxx:
                        bAutoGuitarR = true;
                        break;
                    case EChannel.Guitar_RxBxx:
                        bAutoGuitarR = true;
                        bAutoGuitarB = true;
                        break;
                    case EChannel.Guitar_RGxxx:
                        bAutoGuitarR = true;
                        bAutoGuitarG = true;
                        break;
                    case EChannel.Guitar_RGBxx:
                        bAutoGuitarR = true;
                        bAutoGuitarG = true;
                        bAutoGuitarB = true;
                        break;

                    case EChannel.Guitar_xxxYx:
                        bAutoGuitarY = true;
                        break;
                    case EChannel.Guitar_xxBYx:
                        bAutoGuitarB = true;
                        bAutoGuitarY = true;
                        break;
                    case EChannel.Guitar_xGxYx:
                        bAutoGuitarG = true;
                        bAutoGuitarY = true;
                        break;
                    case EChannel.Guitar_xGBYx:
                        bAutoGuitarG = true;
                        bAutoGuitarB = true;
                        bAutoGuitarY = true;
                        break;
                    case EChannel.Guitar_RxxYx:
                        bAutoGuitarR = true;
                        bAutoGuitarY = true;
                        break;
                    case EChannel.Guitar_RxBYx:
                        bAutoGuitarR = true;
                        bAutoGuitarB = true;
                        bAutoGuitarY = true;
                        break;
                    case EChannel.Guitar_RGxYx:
                        bAutoGuitarR = true;
                        bAutoGuitarG = true;
                        bAutoGuitarY = true;
                        break;
                    case EChannel.Guitar_RGBYx:
                        bAutoGuitarR = true;
                        bAutoGuitarG = true;
                        bAutoGuitarB = true;
                        bAutoGuitarY = true;
                        break;
                    case EChannel.Guitar_xxxxP:
                        bAutoGuitarP = true;
                        break;
                    case EChannel.Guitar_xxBxP:
                        bAutoGuitarB = true;
                        bAutoGuitarP = true;
                        break;
                    case EChannel.Guitar_xGxxP:
                        bAutoGuitarG = true;
                        bAutoGuitarP = true;
                        break;
                    case EChannel.Guitar_xGBxP:
                        bAutoGuitarG = true;
                        bAutoGuitarB = true;
                        bAutoGuitarP = true;
                        break;
                    case EChannel.Guitar_RxxxP:
                        bAutoGuitarR = true;
                        bAutoGuitarP = true;
                        break;

                    //BASS
                    case EChannel.Bass_xxBxx:
                        bAutoBassB = true;
                        break;

                    case EChannel.Bass_xGxxx:
                        bAutoBassG = true;
                        break;

                    case EChannel.Bass_xGBxx:
                        bAutoBassG = true;
                        bAutoBassB = true;
                        break;

                    case EChannel.Bass_Rxxxx:
                        bAutoBassR = true;
                        break;

                    case EChannel.Bass_RxBxx:
                        bAutoBassR = true;
                        bAutoBassB = true;
                        break;

                    case EChannel.Bass_RGxxx:
                        bAutoBassR = true;
                        bAutoBassG = true;
                        break;

                    case EChannel.Bass_RGBxx:
                        bAutoBassR = true;
                        bAutoBassG = true;
                        bAutoBassB = true;
                        break;

                    //A8 WAILING(BASS)

                    case EChannel.Guitar_RxBxP:
                        bAutoGuitarR = true;
                        bAutoGuitarB = true;
                        bAutoGuitarP = true;
                        break;

                    case EChannel.Guitar_RGxxP:
                        bAutoGuitarR = true;
                        bAutoGuitarG = true;
                        bAutoGuitarP = true;
                        break;

                    case EChannel.Guitar_RGBxP:
                        bAutoGuitarR = true;
                        bAutoGuitarG = true;
                        bAutoGuitarB = true;
                        bAutoGuitarP = true;
                        break;

                    case EChannel.Guitar_xxxYP:
                        bAutoGuitarY = true;
                        bAutoGuitarP = true;
                        break;

                    case EChannel.Guitar_xxBYP:
                        bAutoGuitarB = true;
                        bAutoGuitarY = true;
                        bAutoGuitarP = true;
                        break;

                    case EChannel.Guitar_xGxYP:
                        bAutoGuitarG = true;
                        bAutoGuitarY = true;
                        bAutoGuitarP = true;
                        break;

                    case EChannel.Guitar_xGBYP:
                        bAutoGuitarG = true;
                        bAutoGuitarB = true;
                        bAutoGuitarY = true;
                        bAutoGuitarP = true;
                        break;

                    case EChannel.Bass_xxxYx:
                        bAutoBassY = true;
                        break;

                    case EChannel.Bass_xxBYx:
                        bAutoBassB = true;
                        bAutoBassY = true;
                        break;

                    case EChannel.Bass_xGxYx:
                        bAutoBassG = true;
                        bAutoBassY = true;
                        break;

                    case EChannel.Bass_xGBYx:
                        bAutoBassG = true;
                        bAutoBassB = true;
                        bAutoBassY = true;
                        break;

                    case EChannel.Bass_RxxYx:
                        bAutoBassR = true;
                        bAutoBassY = true;
                        break;

                    case EChannel.Bass_RxBYx:
                        bAutoBassR = true;
                        bAutoBassB = true;
                        bAutoBassY = true;
                        break;
                    case EChannel.Bass_RGxYx:
                        bAutoBassR = true;
                        bAutoBassG = true;
                        bAutoBassY = true;
                        break;
                    case EChannel.Bass_RGBYx:
                        bAutoBassR = true;
                        bAutoBassG = true;
                        bAutoBassB = true;
                        bAutoBassY = true;
                        break;
                    case EChannel.Bass_xxxxP:
                        bAutoBassP = true;
                        break;
                    case EChannel.Bass_xxBxP:
                        bAutoBassB = true;
                        bAutoBassP = true;
                        break;

                    case EChannel.Guitar_RxxYP:
                        bAutoGuitarR = true;
                        bAutoGuitarY = true;
                        bAutoGuitarP = true;
                        break;
                    case EChannel.Guitar_RxBYP:
                        bAutoGuitarR = true;
                        bAutoGuitarB = true;
                        bAutoGuitarY = true;
                        bAutoGuitarP = true;
                        break;
                    case EChannel.Guitar_RGxYP:
                        bAutoGuitarR = true;
                        bAutoGuitarG = true;
                        bAutoGuitarY = true;
                        bAutoGuitarP = true;
                        break;
                    case EChannel.Guitar_RGBYP:
                        bAutoGuitarR = true;
                        bAutoGuitarG = true;
                        bAutoGuitarB = true;
                        bAutoGuitarY = true;
                        bAutoGuitarP = true;
                        break;

                    case EChannel.Bass_xGxxP:
                        bAutoBassG = true;
                        bAutoBassP = true;
                        break;
                    case EChannel.Bass_xGBxP:
                        bAutoBassG = true;
                        bAutoBassB = true;
                        bAutoBassP = true;
                        break;
                    case EChannel.Bass_RxxxP:
                        bAutoBassR = true;
                        bAutoBassP = true;
                        break;
                    case EChannel.Bass_RxBxP:
                        bAutoBassR = true;
                        bAutoBassB = true;
                        bAutoBassP = true;
                        break;
                    case EChannel.Bass_RGxxP:
                        bAutoBassR = true;
                        bAutoBassG = true;
                        bAutoBassP = true;
                        break;
                    case EChannel.Bass_RGBxP:
                        bAutoBassR = true;
                        bAutoBassG = true;
                        bAutoBassB = true;
                        bAutoBassP = true;
                        break;
                    case EChannel.Bass_xxxYP:
                        bAutoBassY = true;
                        bAutoBassP = true;
                        break;
                    case EChannel.Bass_xxBYP:
                        bAutoBassB = true;
                        bAutoBassY = true;
                        bAutoBassP = true;
                        break;
                    case EChannel.Bass_xGxYP:
                        bAutoBassG = true;
                        bAutoBassY = true;
                        bAutoBassP = true;
                        break;
                    case EChannel.Bass_xGBYP:
                        bAutoBassG = true;
                        bAutoBassB = true;
                        bAutoBassY = true;
                        bAutoBassP = true;
                        break;
                    case EChannel.Bass_RxxYP:
                        bAutoBassR = true;
                        bAutoBassY = true;
                        bAutoBassP = true;
                        break;
                    case EChannel.Bass_RxBYP:
                        bAutoBassR = true;
                        bAutoBassB = true;
                        bAutoBassY = true;
                        bAutoBassP = true;
                        break;
                    case EChannel.Bass_RGxYP:
                        bAutoBassR = true;
                        bAutoBassG = true;
                        bAutoBassY = true;
                        bAutoBassP = true;
                        break;
                    case EChannel.Bass_RGBYP:
                        bAutoBassR = true;
                        bAutoBassG = true;
                        bAutoBassB = true;
                        bAutoBassY = true;
                        bAutoBassP = true;
                        break;
                }
                //オートさん専用
                if (bAutoGuitarR && bIsAutoPlay.GtR)
                {
                    this.actLaneFlushGB.Start(0);
                    this.actRGB.Push(R);
                }
                if (bAutoGuitarG && bIsAutoPlay.GtG)
                {
                    this.actLaneFlushGB.Start(1);
                    this.actRGB.Push(G);
                }
                if (bAutoGuitarB && bIsAutoPlay.GtB)
                {
                    this.actLaneFlushGB.Start(2);
                    this.actRGB.Push(B);
                }
                if (bAutoGuitarY && bIsAutoPlay.GtY)
                {
                    this.actLaneFlushGB.Start(3);
                    this.actRGB.Push(Y);
                }
                if (bAutoGuitarP && bIsAutoPlay.GtP)
                {
                    this.actLaneFlushGB.Start(4);
                    this.actRGB.Push(P);
                }

                //バグが起こる場所(推定)　こ↑こ↓から
                if (bAutoBassR && bIsAutoPlay.BsR)
                {
                    this.actLaneFlushGB.Start(5);
                    this.actRGB.Push(R);
                }
                if (bAutoBassG && bIsAutoPlay.BsG)
                {
                    this.actLaneFlushGB.Start(6);
                    this.actRGB.Push(G);
                }
                if (bAutoBassB && bIsAutoPlay.BsB)
                {
                    this.actLaneFlushGB.Start(7);
                    this.actRGB.Push(B);
                }
                if (bAutoBassY && bIsAutoPlay.BsY)
                {
                    this.actLaneFlushGB.Start(8);
                    this.actRGB.Push(Y);
                }
                if (bAutoBassP && bIsAutoPlay.BsP)
                {
                    this.actLaneFlushGB.Start(9);
                    this.actRGB.Push(P);
                }
                //バグが起こる場所(推定)　こ↑こ↓まで

                //New: Set ChipColorFlag, regardless of instrument
                bChipColorHasR = bAutoGuitarR || bAutoBassR;
                bChipColorHasG = bAutoGuitarG || bAutoBassG;
                bChipColorHasB = bAutoGuitarB || bAutoBassB;
                bChipColorHasY = bAutoGuitarY || bAutoBassY;
                bChipColorHasP = bAutoGuitarP || bAutoBassP;

                nChipColorFlag = (bChipColorHasR ? 4 : 0) | (bChipColorHasG ? 2 : 0) | (bChipColorHasB ? 1 : 0) | (bChipColorHasY ? 16 : 0) | (bChipColorHasP ? 32 : 0);
            }
            //			else
            {
                //オートさん以外
                //
                bool[] buttonPressArray = new bool[5]
                {
                    CDTXMania.Pad.bPressing(inst, EPad.R),
                    CDTXMania.Pad.bPressing(inst, EPad.G),
                    CDTXMania.Pad.bPressing(inst, EPad.B),
                    CDTXMania.Pad.bPressing(inst, EPad.Y),
                    CDTXMania.Pad.bPressing(inst, EPad.P)
                };
                
                int pushingR = buttonPressArray[0] ? 4 : 0;
                this.tSaveInputMethod(inst);
                int pushingG = buttonPressArray[1] ? 2 : 0;
                this.tSaveInputMethod(inst);
                int pushingB = buttonPressArray[2] ? 1 : 0;
                this.tSaveInputMethod(inst);
                int pushingY = buttonPressArray[3] ? 16 : 0;
                this.tSaveInputMethod(inst);
                int pushingP = buttonPressArray[4] ? 32 : 0;
                this.tSaveInputMethod(inst);
                int nKeyPressRGBFlag = pushingR | pushingG | pushingB | pushingY | pushingP;
                if (pushingR != 0)
                {
                    this.actLaneFlushGB.Start(R);
                    this.actRGB.Push(R);
                }
                if (pushingG != 0)
                {
                    this.actLaneFlushGB.Start(G);
                    this.actRGB.Push(G);
                }
                if (pushingB != 0)
                {
                    this.actLaneFlushGB.Start(B);
                    this.actRGB.Push(B);
                }
                if (pushingY != 0)
                {
                    this.actLaneFlushGB.Start(Y);
                    this.actRGB.Push(Y);
                }
                if (pushingP != 0)
                {
                    this.actLaneFlushGB.Start(P);
                    this.actRGB.Push(P);
                }
                //Add Input processing for Long Notes
                if (chipロングノートHit中[(int)inst] != null)
                {
                    CChip cChip2 = chipロングノートHit中[(int)inst];
                    //bool[] arrayBoolFromEChannel2 = EnumConverter.GetArrayBoolFromEChannel(cChip2.eチャンネル番号);
                    //int num4 = nRGBYPのbool配列からマスク値を返す(arrayBoolFromEChannel2);
                    //if ((num4 & ~num2) == (num3 & ~num2))
                    if ((nChipColorFlag & ~nAutoMask & 0x3F) == (nKeyPressRGBFlag & ~nAutoMask & 0x3F))
                    {
                        if ((nChipColorFlag & nAutoMask & 0x3F) != nChipColorFlag)
                        {
                            actGauge.Damage(inst, cChip2.eInstrumentPart, EJudgement.Good);
                        }
                        
                        //
                        if ((bChipColorHasR && (autoR || pushingR != 0)))
                        {
                            this.actChipFireGB.Start(R);
                        }
                        if ((bChipColorHasG && (autoG || pushingG != 0)))
                        {
                            this.actChipFireGB.Start(G);
                        }
                        if ((bChipColorHasB && (autoB || pushingB != 0)))
                        {
                            this.actChipFireGB.Start(B);
                        }
                        if ((bChipColorHasY && (autoY || pushingY != 0)))
                        {
                            this.actChipFireGB.Start(Y);
                        }
                        if ((bChipColorHasP && (autoP || pushingP != 0)))
                        {
                            this.actChipFireGB.Start(P);
                        }

                        //Check Long Note status
                        if(this.nロングノートPart[(int)inst] < 5)
                        {
                            int nLongNoteNextPart = this.nロングノートPart[(int)inst] + 1;
                            int nLongNoteNextPartTime = chipロングノートHit中[(int)inst].nPlaybackTimeMs + (nLongNoteNextPart * this.nCurrentLongNoteDuration[(int)inst] / 6);
                            if(CSoundManager.rcPerformanceTimer.nCurrentTime >= nLongNoteNextPartTime)
                            {
                                //Fire off 100 bonus pt up to 500 pts for holding long notes
                                this.actScore.Add(inst, bIsAutoPlay, 100);
                                //Also accumulate Bonus score to eventually correct Max score computation
                                this.nAccumulatedLongNoteBonusScore[(int)inst] += 100;
                                //Set off Bonus Score animation
                                this.actGuitarBonus.startBonus(inst);
                                //Increment part number
                                this.nロングノートPart[(int)inst]++;
                            }
                        }
                    }
                    else if (e指定時刻からChipのJUDGEを返す(CSoundManager.rcPerformanceTimer.nCurrentTime, chipロングノートHit中[(int)inst].chipロングノート終端, CDTXMania.ConfigIni.nInputAdjustTimeMs[(int)inst]) >= EJudgement.Miss)
                    {
                        cChip2.bロングノートHit中 = false;
                        chipロングノートHit中[(int)inst] = null;
                        nCurrentLongNoteDuration[(int)inst] = 0;
                        nロングノートPart[(int)inst] = 0;
                        //EPad e = ((inst == EInstrumentPart.GUITAR) ? EPad.GtPick : EPad.BsPick);
                        int nWaveChannelNum = (inst == EInstrumentPart.GUITAR) ? nLastPlayedWAVNumber.GtPick : nLastPlayedWAVNumber.BsPick;
                        //CDTXMania.DTX.tStopPlayingWav(n最後に再生した実WAV番号[e]);
                        CDTXMania.DTX.tStopPlayingWav(nWaveChannelNum);
                    }
                }

                // auto pickだとここから先に行かないので注意
                List<STInputEvent> events = CDTXMania.Pad.GetEvents(inst, EPad.Pick);
                if ((events != null) && (events.Count > 0))
                {
                    foreach (STInputEvent eventPick in events)
                    {
                        if (!eventPick.b押された)
                        {
                            continue;
                        }
                        this.tSaveInputMethod(inst);
                        //Trace.TraceInformation("Pick Event: {0} with Timestamp: {1}", eventPick.nKey, eventPick.nTimeStamp);
                        long nTime = eventPick.nTimeStamp - CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻;
                        //int chWailingSound = (inst == EInstrumentPart.GUITAR) ? 0x2F : 0xAF;
                        int chWailingSound = (inst == EInstrumentPart.GUITAR) ? (int)EChannel.Guitar_Wailing : (int)EChannel.Guitar_xGBYP;


                        //Test new method to find next hittable chip
                        //CChip pChip = this.r指定時刻に一番近い未ヒットChip(nTime, chWailingSound, this.nInputAdjustTimeMs[indexInst], true);	// EInstrumentPart.GUITARなチップ全てにヒットする
                        EChannel eChannelFromInstAndArrayBool = EnumConverter.GetEChannelFromInstAndArrayBool(inst, buttonPressArray);
                        int nCurrPoorRangeMs = (inst == EInstrumentPart.GUITAR) ? CDTXMania.stGuitarHitRanges.nPoorSizeMs : CDTXMania.stBassHitRanges.nPoorSizeMs;
                        CChip pChip = r指定時刻に一番近いChip(nTime, eChannelFromInstAndArrayBool, this.nInputAdjustTimeMs[indexInst], nCurrPoorRangeMs, b過去優先: true, HitState.NotHit, inst);

                        EJudgement e判定 = this.e指定時刻からChipのJUDGEを返す(nTime, pChip, this.nInputAdjustTimeMs[indexInst]);
                        //Trace.TraceInformation("ch={0:x2}, mask1={1:x1}, mask2={2:x2}", pChip.nChannelNumber,  ( pChip.nChannelNumber & ~nAutoMask ) & 0x0F, ( flagRGB & ~nAutoMask) & 0x0F );
                        if (pChip != null)
                        {
                            //bChipHasButtonArray is array of 5, RGBYP
                            bool[] bChipHasButtonArray = EnumConverter.GetArrayBoolFromEChannel(pChip.nChannelNumber);

                            //Check for Open notes
                            bool bChipIsO = pChip.bChipIsOpenNote;                            
                            bool bSuccessOPEN = bChipIsO && (autoR || pushingR == 0) && (autoG || pushingG == 0) && (autoB || pushingB == 0) && (autoY || pushingY == 0) && (autoP || pushingP == 0);

                            int num17 = (bChipHasButtonArray[0] ? 4 : 0) | (bChipHasButtonArray[1] ? 2 : 0) | (bChipHasButtonArray[2] ? 1 : 0) | (bChipHasButtonArray[3] ? 16 : 0) | (bChipHasButtonArray[4] ? 32 : 0);
                            if (pChip != null && (num17 & ~nAutoMask & 0x3F) == (nKeyPressRGBFlag & ~nAutoMask & 0x3F) && e判定 != EJudgement.Miss)
                            {
                                //Trace.TraceInformation("After successful mask check: ch={0:x2}, Judgement={1}, num17={2:x2}, nKeyPressRGBFlag={3:x2}, nAutoMask={4:x2}", (int)pChip.nChannelNumber, e判定, num17, nKeyPressRGBFlag, nAutoMask);
                                if ((bChipHasButtonArray[0] && (autoR || pushingR != 0)) || bSuccessOPEN)
                                {
                                    this.actChipFireGB.Start(R);
                                }
                                if ((bChipHasButtonArray[1] && (autoG || pushingG != 0)) || bSuccessOPEN)
                                {
                                    this.actChipFireGB.Start(G);
                                }
                                if ((bChipHasButtonArray[2] && (autoB || pushingB != 0)) || bSuccessOPEN)
                                {
                                    this.actChipFireGB.Start(B);
                                }
                                if ((bChipHasButtonArray[3] && (autoY || pushingY != 0)) || bSuccessOPEN)
                                {
                                    this.actChipFireGB.Start(Y);
                                }
                                if ((bChipHasButtonArray[4] && (autoP || pushingP != 0)) || bSuccessOPEN)
                                {
                                    this.actChipFireGB.Start(P);
                                }
                                this.tProcessChipHit(nTime, pChip);
                                this.tPlaySound(pChip, CSoundManager.rcPerformanceTimer.nシステム時刻, inst, CDTXMania.ConfigIni.n手動再生音量, CDTXMania.ConfigIni.b演奏音を強調する[indexInst], e判定 == EJudgement.Poor && bCurrInstrumentSpecialist);

                                //int chWailingChip = (inst == EInstrumentPart.GUITAR) ? (int)EChannel.Guitar_Wailing : (int)EChannel.Bass_Wailing;                                
                                //CChip item = this.r指定時刻に一番近い未ヒットChip(nTime, chWailingChip, this.nInputAdjustTimeMs[indexInst], 140);
                                //New Method
                                EChannel search = ((inst == EInstrumentPart.GUITAR) ? EChannel.Guitar_Wailing : EChannel.Bass_Wailing);
                                CChip item = r指定時刻に一番近いChip(nTime, search, this.nInputAdjustTimeMs[indexInst], 140);

                                if (item != null)
                                {
                                    this.queWailing[indexInst].Enqueue(item);
                                }
                                continue;
                            }
                            else
                            {
                                //Mis-hits seems to coincide with 3 channels: 80 (BarLine), 81 (BeatLine) and 45 (Bass_LongNote)
                            }
                        }
                        else 
                        {
                            //Trace.TraceInformation("pChip is null");
                        }

                        // 以下、間違いレーンでのピック時
                        CChip NoChipPicked = (inst == EInstrumentPart.GUITAR) ? this.r現在の空うちギターChip : this.r現在の空うちベースChip;
                        if ((NoChipPicked != null) || ((NoChipPicked = r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, EChannel.Guitar_Open, this.nInputAdjustTimeMs[indexInst], inst)) != null))
                        {
                            this.tPlaySound(NoChipPicked, CSoundManager.rcPerformanceTimer.nシステム時刻, inst, CDTXMania.ConfigIni.n手動再生音量, CDTXMania.ConfigIni.b演奏音を強調する[indexInst], bCurrInstrumentSpecialist);                   
                        }
                        if (!CDTXMania.ConfigIni.bLight[indexInst])
                        {
                            this.tチップのヒット処理_BadならびにTight時のMiss(inst);
                        }
                    }
                }
                List<STInputEvent> list = CDTXMania.Pad.GetEvents(inst, EPad.Wail);
                if ((list != null) && (list.Count > 0))
                {
                    foreach (STInputEvent eventWailed in list)
                    {
                        if (!eventWailed.b押された)
                        {
                            continue;
                        }
                        DoWailingFromQueue(inst, eventWailed.nTimeStamp, autoW);
                    }
                }
            }
        }

        private void DoWailingFromQueue(EInstrumentPart inst, long nTimeStamp_Wailed, bool autoW)
        {
            int indexInst = (int)inst;
            long nTimeWailed = nTimeStamp_Wailed - CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻;
            CChip chipWailing;
            while ((this.queWailing[indexInst].Count > 0) && ((chipWailing = this.queWailing[indexInst].Dequeue()) != null))
            {
                if ((nTimeWailed - chipWailing.nPlaybackTimeMs) <= 1000)		// #24245 2011.1.26 yyagi: 800 -> 1000
                {
                    chipWailing.bHit = true;
                    this.actWailingBonus.Start(inst, this.r現在の歓声Chip[indexInst]);
                    //if ( !bIsAutoPlay[indexInst] )
                    if (!autoW)
                    {
                        if (CDTXMania.ConfigIni.nSkillMode == 0)
                        {
                            int nCombo = (this.actCombo.nCurrentCombo[indexInst] < 500) ? this.actCombo.nCurrentCombo[indexInst] : 500;
                            this.actScore.Add(inst, bIsAutoPlay, nCombo * 3000L);		// #24245 2011.1.26 yyagi changed DRUMS->BASS, add nCombo conditions
                        }
                        else
                        {
                            int nAddScore = this.actCombo.nCurrentCombo[indexInst] > 500 ? 50000 : this.actCombo.nCurrentCombo[indexInst] * 100;
                            this.actScore.Add(inst, bIsAutoPlay, nAddScore);		// #24245 2011.1.26 yyagi changed DRUMS->BASS, add nCombo conditions

                            this.tBoostBonus();
                        }
                    }
                }
            }
        }

        private void tBoostBonus()
        {

            for (int i = 0; i < 1; i++)
            {

                if (this.ctTimer[i].bInProgress)
                {
                    this.ctTimer[i].tStop();
                }

            }

            for (int i = 0; i < 1; i++)
            {
                if (!this.ctTimer[i].bInProgress)
                {
                    this.bブーストボーナス = true;
                    this.ctTimer[i].tUpdate();
                    if (this.ctTimer[i].bReachedEndValue)
                    {
                        this.ctTimer[i].tStop();
                        this.bブーストボーナス = false;
                    }
                }

            }
        }

        #endregion
        public void tJumpInSongToBar(int nStartBar)
        {
            int nTopChip = 0;
            for (int i = 0; i < CDTXMania.DTX.listChip.Count; i++)
            {
                CChip pChip = CDTXMania.DTX.listChip[i];
                if (pChip.nPlaybackPosition >= 384 * nStartBar)
                {
                    nTopChip = i;
                    break;
                }
            }
            int nStartTime = CDTXMania.DTX.listChip[nTopChip].nPlaybackTimeMs;

            tJumpInSong(nStartTime);
        }
        protected void tJumpInSong(long newPosition)
        {
            long nNewPosition = Math.Max(0, newPosition);
            Trace.TraceInformation("JUMP IN SONG currentPosition={0}, newPosition={1}", CSoundManager.rcPerformanceTimer.nCurrentTime, nNewPosition);

            long oldPosition = CSoundManager.rcPerformanceTimer.nCurrentTime;
            CSoundManager.rcPerformanceTimer.tReset();
            CSoundManager.rcPerformanceTimer.tPause();
            CSoundManager.rcPerformanceTimer.nCurrentTime = nNewPosition;
            CDTXMania.Timer.tReset();
            CDTXMania.Timer.nCurrentTime = nNewPosition;

            //Stop any AVI and BGA
            this.actAVI.Stop();
            this.actBGA.Stop();
            //Reset Hold note cache
            this.chipロングノートHit中 = default(STDGBVALUE<CChip>);
            this.nロングノートPart = default(STDGBVALUE<int>);
            this.nCurrentLongNoteDuration = default(STDGBVALUE<int>);

            //Reset Accumulated Score
            this.nAccumulatedLongNoteBonusScore = default(STDGBVALUE<int>);

            // Loop to set new nCurrentTopChip
            // Also, if we are going backward, we need to unhit some chips, and reset TopChip
            this.nCurrentTopChip = 0;
            bool bIsTopChipSet = false;
            for (int nCurrentChip = 0; nCurrentChip < CDTXMania.DTX.listChip.Count; nCurrentChip++)
            {
                CChip pChip = CDTXMania.DTX.listChip[nCurrentChip];

                if (bIsTopChipSet && pChip.nPlaybackTimeMs > oldPosition)
                {
                    break;
                }

                if (pChip.nPlaybackTimeMs >= nNewPosition)
                {
                    if (!bIsTopChipSet)
                    {
                        this.nCurrentTopChip = nCurrentChip;
                        bIsTopChipSet = true;
                        if (nNewPosition > oldPosition)
                        {
                            break;
                        }
                    }
                    // Unhit the chip so it displays again (and is hittable again)
                    if (pChip.bHit)
                    {
                        pChip.bHit = false;
                    }

                    //NEW: Reset Long Note too
                    if (pChip.bロングノートHit中)
                    {
                        pChip.bロングノートHit中 = false;
                    }
                }
            }

            //Adjust BGM to new position
            List<CSound> pausedCSound = new List<CSound>();

            #region [ BGMやギターなど、演奏開始のタイミングで再生がかかっているサウンドのの途中再生開始 ] // (CDTXのt入力・行解析・チップ配置()で小節番号が+1されているのを削っておくこと)
            for (int i = this.nCurrentTopChip; i >= 0; i--)
            {
                CChip pChip = CDTXMania.DTX.listChip[i];
                int nDuration = pChip.GetDuration();

                if ((pChip.nPlaybackTimeMs + nDuration > 0) && (pChip.nPlaybackTimeMs <= nNewPosition) && (nNewPosition <= pChip.nPlaybackTimeMs + nDuration))
                {
                    if (pChip.bWAVを使うチャンネルである /*&& !pChip.b空打ちチップである*/) // wav系チャンネル、且つ、空打ちチップではない
                    {
                        CDTX.CWAV wc;
                        bool b = CDTXMania.DTX.listWAV.TryGetValue(pChip.nIntegerValue_InternalNumber, out wc);
                        if (!b) continue;

                        if ((wc.bIsBGMSound && CDTXMania.ConfigIni.bBGM音を発声する) || (!wc.bIsBGMSound))
                        {
                            CDTXMania.DTX.tチップの再生(pChip, CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻 + pChip.nPlaybackTimeMs, CDTXMania.DTX.nモニタを考慮した音量(EInstrumentPart.UNKNOWN));
                            #region [ PAUSEする ]
                            int j = wc.n現在再生中のサウンド番号;
                            if (wc.rSound[j] != null)
                            {
                                // Needed only if the tJumpInSong is called by tChangePlaySpeed
                                wc.rSound[j].dbPlaySpeed = ((double)CDTXMania.ConfigIni.nPlaySpeed) / 20.0;

                                wc.rSound[j].tPausePlayback();
                                wc.rSound[j].tChangePlaybackPosition(nNewPosition - pChip.nPlaybackTimeMs);
                                pausedCSound.Add(wc.rSound[j]);
                            }
                            #endregion
                        }
                    }
                }
            }
            #endregion
            #region [ 演奏開始時点で既に表示されているBGAとAVIの、シークと再生 ]
            //Re-enable SkipStart now that we have migrated to AVI renderer 
            this.actBGA.SkipStart((int)nNewPosition);
            this.actAVI.SkipStart((int)nNewPosition);
            #endregion
            #region [ PAUSEしていたサウンドを一斉に再生再開する(ただしタイマを止めているので、ここではまだ再生開始しない) ]
            foreach (CSound cs in pausedCSound)
            {
                cs.tPlaySound();
            }
            pausedCSound.Clear();
            #endregion

            this.bPAUSE = false;
            CSoundManager.rcPerformanceTimer.tResume();

            // re-display presence with new timestamps
            tDisplayPresence();
        }

        protected void tChangePlaySpeed(int nSpeedOffset)
        {
            Trace.TraceInformation(((nSpeedOffset>0) ? "Increase" : "Decrease") + " Play Speed from " + CDTXMania.ConfigIni.nPlaySpeed + " to " + (CDTXMania.ConfigIni.nPlaySpeed+nSpeedOffset));

            double dbOldSpeed = ((double)CDTXMania.ConfigIni.nPlaySpeed) / 20;
            CDTXMania.ConfigIni.nPlaySpeed += nSpeedOffset;
            double dbNewSpeed = ((double)CDTXMania.ConfigIni.nPlaySpeed) / 20;

            foreach (CChip chip in CDTXMania.DTX.listChip)
            {
                chip.nPlaybackTimeMs = (int)(((double)chip.nPlaybackTimeMs * dbOldSpeed) / dbNewSpeed);
            }
            if (this.LoopBeginMs != -1)
            {
                this.LoopBeginMs = (int)(((double)this.LoopBeginMs * dbOldSpeed) / dbNewSpeed);
            }
            if (this.LoopEndMs != -1)
            {
                this.LoopEndMs = (int)(((double)this.LoopEndMs * dbOldSpeed) / dbNewSpeed);
            }

            CSoundManager.rcPerformanceTimer.nCurrentTime = (int)(((double)CSoundManager.rcPerformanceTimer.nCurrentTime * dbOldSpeed) / dbNewSpeed);

            tJumpInSong(CSoundManager.rcPerformanceTimer.nCurrentTime);

            // Display new play speed
            if (CDTXMania.ConfigIni.nShowPlaySpeed == (int)EShowPlaySpeed.ON || CDTXMania.ConfigIni.nShowPlaySpeed == (int)EShowPlaySpeed.IF_CHANGED_IN_GAME)
            {
                tGeneratePlaySpeedTexture();
            }

            // re-display presence with new timestamps
            tDisplayPresence();
        }

        private void tGeneratePlaySpeedTexture()
        {
            CDTXMania.tReleaseTexture(ref this.txPlaySpeed);
            if (CDTXMania.ConfigIni.nPlaySpeed != 20)
            {
                double d = (double)(CDTXMania.ConfigIni.nPlaySpeed / 20.0);
                String strModifiedPlaySpeed = "Play Speed: x" + d.ToString("0.000");
                CPrivateFastFont pfModifiedPlaySpeed = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.str選曲リストフォント), 18, FontStyle.Regular);
                Bitmap bmpModifiedPlaySpeed = pfModifiedPlaySpeed.DrawPrivateFont(strModifiedPlaySpeed, CPrivateFont.DrawMode.Edge, Color.White, Color.White, Color.Black, Color.Red, true);
                this.txPlaySpeed = CDTXMania.tGenerateTexture(bmpModifiedPlaySpeed, false);
                bmpModifiedPlaySpeed.Dispose();
                pfModifiedPlaySpeed.Dispose();
            }
        }

        protected void tSetSettingsForDTXV()
        {
            for (int i = 0; i < (int)ELane.MAX; i++)
            {
                CDTXMania.ConfigIni.bAutoPlay[i] = true;
            }
            CDTXMania.ConfigIni.bAVIEnabled = true;
            CDTXMania.ConfigIni.nMovieMode = 2;
            CDTXMania.ConfigIni.bBGAEnabled = true;
            for (int i = 0; i < 3; i++)
            {
                CDTXMania.ConfigIni.bGraph有効[i] = false;
                CDTXMania.ConfigIni.nHidSud[i] = 0; // ESudHidInv.Off;
                CDTXMania.ConfigIni.bLight[i] = false;
                CDTXMania.ConfigIni.bReverse[i] = false;
                CDTXMania.ConfigIni.eRandom[i] = ERandomMode.OFF;
                CDTXMania.ConfigIni.n表示可能な最小コンボ数[i] = 65535;
                CDTXMania.ConfigIni.bDisplayJudge[i] = false;
            }
            CDTXMania.ConfigIni.bドラムコンボ文字の表示 = false;
            CDTXMania.ConfigIni.eDark = EDarkMode.OFF;
            //TODO add this option: CDTXMania.ConfigIni.bDebugInfo = CDTXMania.ConfigIni.bViewerShowDebugStatus;
            CDTXMania.ConfigIni.b演奏情報を表示しない = false;
            CDTXMania.ConfigIni.bFillInEnabled = true;
            CDTXMania.ConfigIni.bScoreIniを出力する = false;
            CDTXMania.ConfigIni.bSTAGEFAILEDEnabled = false;
            CDTXMania.ConfigIni.bTight = false;
            CDTXMania.ConfigIni.bストイックモード = false;
            CDTXMania.ConfigIni.bドラム打音を発声する = true;
            CDTXMania.ConfigIni.bBGM音を発声する = true;
            CDTXMania.ConfigIni.nRisky = 0;
            CDTXMania.ConfigIni.nShowLagType = (int)EShowLagType.OFF;
            CDTXMania.ConfigIni.bShowLagHitCount = false;
            //CDTXMania.ConfigIni.bForceScalingAVI = false;		// DTXVモード時の各種表示要素の表示座標を「譜面制作者のカスタマイズ状態」にするか「DTXMania初期状態」にするかで
            // 悩みました。
        }

        public void t再読込()
        {
            CDTXMania.DTX.tStopPlayingAllChips();
            this.eReturnValueAfterFadeOut = EPerfScreenReturnValue.Restart;
            base.ePhaseID = CStage.EPhase.演奏_STAGE_RESTART;
            this.bPAUSE = false;

            // #34048 2014.7.16 yyagi
            #region [ 読み込み画面に遷移する前に、設定変更した可能性があるパラメータをConfigIniクラスに書き戻す ]
            //for (i = 0; i < 3; i++)
            //{
            //    CDTXMania.ConfigIni.nViewerScrollSpeed[i] = CDTXMania.Instance.ConfigIni.nScrollSpeed[i];
            //}
            //TODO CDTXMania.ConfigIni.bDebugInfo = CDTXMania.ConfigIni.bViewerShowDebugStatus;
            #endregion
        }

        public void t停止()
        {
            Trace.TraceInformation("Stop command received");
            CDTXMania.DTX.tStopPlayingAllChips();
            this.actAVI.Stop();
            this.actBGA.Stop();
            //this.perfpanel.Stop();               // PANEL表示停止
            CDTXMania.Timer.tPause();       // 再生時刻カウンタ停止

            this.nCurrentTopChip = CDTXMania.DTX.listChip.Count - 1;   // 終端にシーク
            //Reset Hold note cache
            this.chipロングノートHit中 = default(STDGBVALUE<CChip>);
            this.nロングノートPart = default(STDGBVALUE<int>);
            this.nCurrentLongNoteDuration = default(STDGBVALUE<int>);
            //
            this.nAccumulatedLongNoteBonusScore = default(STDGBVALUE<int>);
            // 自分自身のOn活性化()相当の処理もすべき。
        }
    }
}
