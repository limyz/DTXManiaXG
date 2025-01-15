using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using SharpDX;
using FDK;

using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;
using Color = System.Drawing.Color;

namespace DTXMania
{
	internal class CActResultParameterPanel : CActivity
	{
		// コンストラクタ

        public CActResultParameterPanel()
        {
            this.txCharacter = new CTexture[3];
            ST文字位置[] st文字位置Array = new ST文字位置[11];
            ST文字位置 st文字位置 = new ST文字位置();
            st文字位置.ch = '0';
            st文字位置.pt = new Point(0, 0);
            st文字位置Array[0] = st文字位置;
            ST文字位置 st文字位置2 = new ST文字位置();
            st文字位置2.ch = '1';
            st文字位置2.pt = new Point(28, 0);
            st文字位置Array[1] = st文字位置2;
            ST文字位置 st文字位置3 = new ST文字位置();
            st文字位置3.ch = '2';
            st文字位置3.pt = new Point(56, 0);
            st文字位置Array[2] = st文字位置3;
            ST文字位置 st文字位置4 = new ST文字位置();
            st文字位置4.ch = '3';
            st文字位置4.pt = new Point(84, 0);
            st文字位置Array[3] = st文字位置4;
            ST文字位置 st文字位置5 = new ST文字位置();
            st文字位置5.ch = '4';
            st文字位置5.pt = new Point(112, 0);
            st文字位置Array[4] = st文字位置5;
            ST文字位置 st文字位置6 = new ST文字位置();
            st文字位置6.ch = '5';
            st文字位置6.pt = new Point(140, 0);
            st文字位置Array[5] = st文字位置6;
            ST文字位置 st文字位置7 = new ST文字位置();
            st文字位置7.ch = '6';
            st文字位置7.pt = new Point(168, 0);
            st文字位置Array[6] = st文字位置7;
            ST文字位置 st文字位置8 = new ST文字位置();
            st文字位置8.ch = '7';
            st文字位置8.pt = new Point(196, 0);
            st文字位置Array[7] = st文字位置8;
            ST文字位置 st文字位置9 = new ST文字位置();
            st文字位置9.ch = '8';
            st文字位置9.pt = new Point(224, 0);
            st文字位置Array[8] = st文字位置9;
            ST文字位置 st文字位置10 = new ST文字位置();
            st文字位置10.ch = '9';
            st文字位置10.pt = new Point(252, 0);
            st文字位置Array[9] = st文字位置10;
            ST文字位置 st文字位置11 = new ST文字位置();
            st文字位置11.ch = '.';
            st文字位置11.pt = new Point(280, 0);
            st文字位置Array[10] = st文字位置11;
            this.stLargeStringPosition = st文字位置Array;

            ST文字位置[] st文字位置Array2 = new ST文字位置[11];
            ST文字位置 st文字位置12 = new ST文字位置();
            st文字位置12.ch = '0';
            st文字位置12.pt = new Point(0, 0);
            st文字位置Array2[0] = st文字位置12;
            ST文字位置 st文字位置13 = new ST文字位置();
            st文字位置13.ch = '1';
            st文字位置13.pt = new Point(20, 0);
            st文字位置Array2[1] = st文字位置13;
            ST文字位置 st文字位置14 = new ST文字位置();
            st文字位置14.ch = '2';
            st文字位置14.pt = new Point(40, 0);
            st文字位置Array2[2] = st文字位置14;
            ST文字位置 st文字位置15 = new ST文字位置();
            st文字位置15.ch = '3';
            st文字位置15.pt = new Point(60, 0);
            st文字位置Array2[3] = st文字位置15;
            ST文字位置 st文字位置16 = new ST文字位置();
            st文字位置16.ch = '4';
            st文字位置16.pt = new Point(80, 0);
            st文字位置Array2[4] = st文字位置16;
            ST文字位置 st文字位置17 = new ST文字位置();
            st文字位置17.ch = '5';
            st文字位置17.pt = new Point(100, 0);
            st文字位置Array2[5] = st文字位置17;
            ST文字位置 st文字位置18 = new ST文字位置();
            st文字位置18.ch = '6';
            st文字位置18.pt = new Point(120, 0);
            st文字位置Array2[6] = st文字位置18;
            ST文字位置 st文字位置19 = new ST文字位置();
            st文字位置19.ch = '7';
            st文字位置19.pt = new Point(140, 0);
            st文字位置Array2[7] = st文字位置19;
            ST文字位置 st文字位置20 = new ST文字位置();
            st文字位置20.ch = '8';
            st文字位置20.pt = new Point(160, 0);
            st文字位置Array2[8] = st文字位置20;
            ST文字位置 st文字位置21 = new ST文字位置();
            st文字位置21.ch = '9';
            st文字位置21.pt = new Point(180, 0);
            st文字位置Array2[9] = st文字位置21;
            ST文字位置 st文字位置22 = new ST文字位置();
            st文字位置22.ch = '%';
            st文字位置22.pt = new Point(200, 0);
            st文字位置Array2[10] = st文字位置22;
            this.stSmallStringPosition = st文字位置Array2;
            ST文字位置[] st文字位置Array3 = new ST文字位置[12];
            ST文字位置 st文字位置23 = new ST文字位置();
            st文字位置23.ch = '0';
            st文字位置23.pt = new Point(0, 0);
            st文字位置Array3[0] = st文字位置23;
            ST文字位置 st文字位置24 = new ST文字位置();
            st文字位置24.ch = '1';
            st文字位置24.pt = new Point(0x12, 0);
            st文字位置Array3[1] = st文字位置24;
            ST文字位置 st文字位置25 = new ST文字位置();
            st文字位置25.ch = '2';
            st文字位置25.pt = new Point(0x24, 0);
            st文字位置Array3[2] = st文字位置25;
            ST文字位置 st文字位置26 = new ST文字位置();
            st文字位置26.ch = '3';
            st文字位置26.pt = new Point(0x36, 0);
            st文字位置Array3[3] = st文字位置26;
            ST文字位置 st文字位置27 = new ST文字位置();
            st文字位置27.ch = '4';
            st文字位置27.pt = new Point(0x48, 0);
            st文字位置Array3[4] = st文字位置27;
            ST文字位置 st文字位置28 = new ST文字位置();
            st文字位置28.ch = '5';
            st文字位置28.pt = new Point(0, 0x18);
            st文字位置Array3[5] = st文字位置28;
            ST文字位置 st文字位置29 = new ST文字位置();
            st文字位置29.ch = '6';
            st文字位置29.pt = new Point(0x12, 0x18);
            st文字位置Array3[6] = st文字位置29;
            ST文字位置 st文字位置30 = new ST文字位置();
            st文字位置30.ch = '7';
            st文字位置30.pt = new Point(0x24, 0x18);
            st文字位置Array3[7] = st文字位置30;
            ST文字位置 st文字位置31 = new ST文字位置();
            st文字位置31.ch = '8';
            st文字位置31.pt = new Point(0x36, 0x18);
            st文字位置Array3[8] = st文字位置31;
            ST文字位置 st文字位置32 = new ST文字位置();
            st文字位置32.ch = '9';
            st文字位置32.pt = new Point(0x48, 0x18);
            st文字位置Array3[9] = st文字位置32;
            ST文字位置 st文字位置33 = new ST文字位置();
            st文字位置33.ch = '.';
            st文字位置33.pt = new Point(90, 24);
            st文字位置Array3[10] = st文字位置33;
            ST文字位置 st文字位置34 = new ST文字位置();
            st文字位置34.ch = '%';
            st文字位置34.pt = new Point(90, 0);
            st文字位置Array3[11] = st文字位置34;
            this.st特大文字位置 = st文字位置Array3;


            ST文字位置[] st難易度文字位置Ar = new ST文字位置[11];
            ST文字位置 st難易度文字位置 = new ST文字位置();
            st難易度文字位置.ch = '0';
            st難易度文字位置.pt = new Point(0, 0);
            st難易度文字位置Ar[0] = st難易度文字位置;
            ST文字位置 st難易度文字位置2 = new ST文字位置();
            st難易度文字位置2.ch = '1';
            st難易度文字位置2.pt = new Point(16, 0);
            st難易度文字位置Ar[1] = st難易度文字位置2;
            ST文字位置 st難易度文字位置3 = new ST文字位置();
            st難易度文字位置3.ch = '2';
            st難易度文字位置3.pt = new Point(32, 0);
            st難易度文字位置Ar[2] = st難易度文字位置3;
            ST文字位置 st難易度文字位置4 = new ST文字位置();
            st難易度文字位置4.ch = '3';
            st難易度文字位置4.pt = new Point(48, 0);
            st難易度文字位置Ar[3] = st難易度文字位置4;
            ST文字位置 st難易度文字位置5 = new ST文字位置();
            st難易度文字位置5.ch = '4';
            st難易度文字位置5.pt = new Point(64, 0);
            st難易度文字位置Ar[4] = st難易度文字位置5;
            ST文字位置 st難易度文字位置6 = new ST文字位置();
            st難易度文字位置6.ch = '5';
            st難易度文字位置6.pt = new Point(80, 0);
            st難易度文字位置Ar[5] = st難易度文字位置6;
            ST文字位置 st難易度文字位置7 = new ST文字位置();
            st難易度文字位置7.ch = '6';
            st難易度文字位置7.pt = new Point(96, 0);
            st難易度文字位置Ar[6] = st難易度文字位置7;
            ST文字位置 st難易度文字位置8 = new ST文字位置();
            st難易度文字位置8.ch = '7';
            st難易度文字位置8.pt = new Point(112, 0);
            st難易度文字位置Ar[7] = st難易度文字位置8;
            ST文字位置 st難易度文字位置9 = new ST文字位置();
            st難易度文字位置9.ch = '8';
            st難易度文字位置9.pt = new Point(128, 0);
            st難易度文字位置Ar[8] = st難易度文字位置9;
            ST文字位置 st難易度文字位置10 = new ST文字位置();
            st難易度文字位置10.ch = '9';
            st難易度文字位置10.pt = new Point(144, 0);
            st難易度文字位置Ar[9] = st難易度文字位置10;
            ST文字位置 st難易度文字位置11 = new ST文字位置();
            st難易度文字位置11.ch = '.';
            st難易度文字位置11.pt = new Point(160, 0);
            st難易度文字位置Ar[10] = st難易度文字位置11;
            this.st難易度数字位置 = st難易度文字位置Ar;

            this.ptFullCombo位置 = new Point[] { new Point(220, 160), new Point(0xdf, 0xed), new Point(0x141, 0xed) };

            //Initialize positions of character in lag text sprite
            int nWidth = 15;
            int nHeight = 19;
            Point ptRedTextOffset = new Point(64, 64);
            List<ST文字位置Ex> LagCountBlueTextList = new List<ST文字位置Ex>();
            List<ST文字位置Ex> LagCountRedTextList = new List<ST文字位置Ex>();
            int[] nPosXArray = { 0, 15, 30, 45, 0, 15, 30, 45, 0, 15 };
            int[] nPosYArray = { 0, 0, 0, 0, 19, 19, 19, 19, 38, 38 };
            for (int i = 0; i < nPosXArray.Length; i++)
            {
                ST文字位置Ex stCurrText = new ST文字位置Ex();
                stCurrText.ch = (char)('0' + i);
                stCurrText.rect = new Rectangle(nPosXArray[i], nPosYArray[i], nWidth, nHeight);
                LagCountBlueTextList.Add(stCurrText);

                ST文字位置Ex stNextCurrText = new ST文字位置Ex();
                stNextCurrText.ch = (char)('0' + i);
                stNextCurrText.rect = new Rectangle(nPosXArray[i] + ptRedTextOffset.X,
                    nPosYArray[i] + ptRedTextOffset.Y, nWidth, nHeight);
                LagCountRedTextList.Add(stNextCurrText);
            }

            this.stLagCountBlueText = LagCountBlueTextList.ToArray();
            this.stLagCountRedText = LagCountRedTextList.ToArray();

            base.bNotActivated = true;
        }


		// メソッド

		public void tアニメを完了させる()
		{
			this.ct表示用.nCurrentValue = this.ct表示用.nEndValue;
		}


		// CActivity 実装

		public override void OnActivate()
		{
            #region [ 本体位置 ]

            int n左1X = 136;
            int n右1X = 850;

            int n左2X = 30;
            int n右2X = 1000;

            this.n本体Y = 260;

            for (int j = 0; j < 3; j++)
            {
                this.n本体X[j] = 0;
                this.nスコアX[j] = 0;
            }

            if (CDTXMania.ConfigIni.bDrumsEnabled)
            {
                this.n本体X[0] = 180;
                this.nスコアX[0] = n左2X;
            }
            else if (CDTXMania.ConfigIni.bGuitarEnabled)
            {
                if (CDTXMania.DTX.bチップがある.Guitar)
                {
                    if (CDTXMania.ConfigIni.bIsSwappedGuitarBass)
                    {
                        this.n本体X[1] = n右1X;
                        this.nスコアX[1] = n右2X;
                    }
                    else
                    {
                        this.n本体X[1] = n左1X;
                        this.nスコアX[1] = n左2X;
                    }
                }

                if (CDTXMania.DTX.bチップがある.Bass)
                {
                    if (CDTXMania.ConfigIni.bIsSwappedGuitarBass)
                    {
                        this.n本体X[2] = n左1X;
                        this.nスコアX[2] = n左2X;
                    }
                    else
                    {
                        this.n本体X[2] = n右1X;
                        this.nスコアX[2] = n右2X;
                    }
                }

            }
            #endregion

            this.prv表示用フォント = new CPrivateFastFont( new FontFamily( CDTXMania.ConfigIni.str曲名表示フォント ), 20, FontStyle.Regular );
            this.prv称号フォント = new CPrivateFastFont( new FontFamily( CDTXMania.ConfigIni.str曲名表示フォント ), 12, FontStyle.Regular );

			this.sdDTXで指定されたフルコンボ音 = null;
			base.OnActivate();
		}
		public override void OnDeactivate()
		{
			if( this.ct表示用 != null )
			{
				this.ct表示用 = null;
			}
            if (this.sdDTXで指定されたフルコンボ音 != null)
			{
				CDTXMania.SoundManager.tDiscard( this.sdDTXで指定されたフルコンボ音 );
				this.sdDTXで指定されたフルコンボ音 = null;
			}
			base.OnDeactivate();
		}
		public override void OnManagedCreateResources()
		{
            if (!base.bNotActivated)
            {
                this.txCharacter[0] = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_Ratenumber_s.png"));
                this.txCharacter[1] = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_Ratenumber_l.png"));
                this.txCharacter[2] = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\8_numbers_large.png"));
                this.txNewRecord = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\8_New Record.png"));
                this.txExciteGauge = new CTexture[3];
                this.txExciteGauge[0] = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_Gauge.png"));
                this.txExciteGauge[1] = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_gauge_bar.png"));
                this.txExciteGauge[2] = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_gauge_bar.jpg"));
                this.txScore = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_score numbersGD.png"));
                this.txSkillPanel = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_SkillPanel.png"));
                this.tx難易度パネル = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_Difficulty.png"));
                this.tx難易度用数字 = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_LevelNumber.png"));
                //Load new textures
                this.txPercent = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_RatePercent_l.png"));
                this.txSkillMax = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_skill max.png"));
                //
                this.txProgressBarPanel = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\8_progress_bar_panel.png"));
                this.txLagHitCount = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_lag numbers.png"));
                for ( int i = 0; i < 3; i++ )
                {
                    this.strPlayerName = string.IsNullOrEmpty( CDTXMania.ConfigIni.strCardName[ i ] ) ? "GUEST" : CDTXMania.ConfigIni.strCardName[ i ];
                    this.strTitleName = string.IsNullOrEmpty( CDTXMania.ConfigIni.strGroupName[ i ] ) ? "" : CDTXMania.ConfigIni.strGroupName[ i ];
                    Bitmap image2 = new Bitmap( 257, 100 );
                    Graphics graネームプレート用 = Graphics.FromImage( image2 );

                    #region[ ネームカラー ]
                    //--------------------
                    Color clNameColor = Color.White;
                    Color clNameColorLower = Color.White;
                    switch( CDTXMania.ConfigIni.nNameColor[ i ] )
                    {
                        case 0:
                            clNameColor = Color.White;
                            break;
                        case 1:
                            clNameColor = Color.LightYellow;
                            break;
                        case 2:
                            clNameColor = Color.Yellow;
                            break;
                        case 3:
                            clNameColor = Color.Green;
                            break;
                        case 4:
                            clNameColor = Color.Blue;
                            break;
                        case 5:
                            clNameColor = Color.Purple;
                            break;
                        case 6:
                            clNameColor = Color.Red;
                            break;
                        case 7:
                            clNameColor = Color.Brown;
                            break;
                        case 8:
                            clNameColor = Color.Silver;
                            break;
                        case 9:
                            clNameColor = Color.Gold;
                            break;

                        case 10:
                            clNameColor = Color.White;
                            break;
                        case 11:
                            clNameColor = Color.LightYellow;
                            clNameColorLower = Color.White;
                            break;
                        case 12:
                            clNameColor = Color.Yellow;
                            clNameColorLower = Color.White;
                            break;
                        case 13:
                            clNameColor = Color.FromArgb(0, 255, 33);
                            clNameColorLower = Color.White;
                            break;
                        case 14:
                            clNameColor = Color.FromArgb(0, 38, 255);
                            clNameColorLower = Color.White;
                            break;
                        case 15:
                            clNameColor = Color.FromArgb(72, 0, 255);
                            clNameColorLower = Color.White;
                            break;
                        case 16:
                            clNameColor = Color.FromArgb(255, 255, 0, 0);
                            clNameColorLower = Color.White;
                            break;
                        case 17:
                            clNameColor = Color.FromArgb(255, 232, 182, 149);
                            clNameColorLower = Color.FromArgb(255, 122, 69, 26);
                            break;
                        case 18:
                            clNameColor = Color.FromArgb(246, 245, 255);
                            clNameColorLower = Color.FromArgb(125, 128, 137);
                            break;
                        case 19:
                            clNameColor = Color.FromArgb(255, 238, 196, 85);
                            clNameColorLower = Color.FromArgb(255, 255, 241, 200);
                            break;
                    }
                    //--------------------
                    #endregion
                    #region[ 名前とか ]
                    Bitmap bmpCardName = new Bitmap(1, 1);
                    bmpCardName = this.prv表示用フォント.DrawPrivateFont( this.strPlayerName, Color.White, Color.Transparent, clNameColor, ( CDTXMania.ConfigIni.nNameColor[ i ] > 11 ? clNameColorLower : clNameColor ) );
                    Bitmap bmpTitleName = new Bitmap(1, 1);
                    bmpTitleName = this.prv称号フォント.DrawPrivateFont( this.strTitleName, Color.White, Color.Transparent );

                    graネームプレート用.DrawImage( bmpCardName, -2f, 26f );
                    graネームプレート用.DrawImage( bmpTitleName, 6f, 8f );
                    #endregion
                    CDTXMania.tDisposeSafely( ref bmpCardName );
                    CDTXMania.tDisposeSafely( ref bmpTitleName );
                    this.txネームプレート用文字[ i ] = new CTexture( CDTXMania.app.Device, image2, CDTXMania.TextureFormat, false );
                    CDTXMania.tDisposeSafely( ref image2 );

                    CDTXMania.tDisposeSafely( ref graネームプレート用 );
                }
                this.prv表示用フォント.Dispose();
                this.prv称号フォント.Dispose();

                this.tGetDifficultyLabelFromScript( CDTXMania.stageSongSelection.rConfirmedSong.arDifficultyLabel[ CDTXMania.stageSongSelection.nConfirmedSongDifficulty ] );

                //Progress Bars
                for (int i = 0; i < 3; i++)
                {
                    //Best Record (Previous)
                    CTexture bestRecordProgressBarTexture = this.txPreviousBestProgressBar[i];
                    CDTXMania.tDisposeSafely(ref bestRecordProgressBarTexture);
                    CActPerfProgressBar.txGenerateProgressBarHelper(
                        ref bestRecordProgressBarTexture,
                        CDTXMania.stageResult.strBestProgressBarRecord[i], 4, 425,
                        CActPerfProgressBar.nSectionIntervalCount);
                    this.txPreviousBestProgressBar[i] = bestRecordProgressBarTexture;

                    //Current Progress Bar
                    CTexture currProgressBarTexture = this.txCurrentProgressBar[i];
                    CDTXMania.tDisposeSafely(ref currProgressBarTexture);
                    CActPerfProgressBar.txGenerateProgressBarHelper(
                        ref currProgressBarTexture,
                        CDTXMania.stageResult.strCurrProgressBarRecord[i], 12, 425, 
                        CActPerfProgressBar.nSectionIntervalCount);
                    this.txCurrentProgressBar[i] = currProgressBarTexture;
                }

                base.OnManagedCreateResources();
            }
		}
        public override void OnManagedReleaseResources()
        {
            if (!base.bNotActivated)
            {
                CDTXMania.tReleaseTexture( ref this.txパネル本体 );
                CDTXMania.tReleaseTexture( ref this.txNewRecord );
                CDTXMania.tReleaseTexture( ref this.txSkillPanel );
                CDTXMania.tReleaseTexture( ref this.txScore );
                CDTXMania.tReleaseTexture( ref this.tx難易度パネル );
                CDTXMania.tReleaseTexture( ref this.tx難易度用数字 );
                //Free new texture
                CDTXMania.tReleaseTexture(ref this.txPercent);
                CDTXMania.tReleaseTexture(ref this.txSkillMax);
                CDTXMania.tReleaseTexture(ref this.txProgressBarPanel);
                for ( int i = 0; i < 3; i++ )
                {
                    CDTXMania.tReleaseTexture( ref this.txネームプレート用文字[ i ] );
                    CDTXMania.tReleaseTexture( ref this.txExciteGauge[ i ] );
                    CDTXMania.tReleaseTexture( ref this.txCharacter[ i ] );
                }
                CDTXMania.tReleaseTexture(ref this.txLagHitCount);
                //
                CDTXMania.tDisposeSafely(ref this.txPreviousBestProgressBar.Drums);
                CDTXMania.tDisposeSafely(ref this.txPreviousBestProgressBar.Guitar);
                CDTXMania.tDisposeSafely(ref this.txPreviousBestProgressBar.Bass);
                CDTXMania.tDisposeSafely(ref this.txCurrentProgressBar.Drums);
                CDTXMania.tDisposeSafely(ref this.txCurrentProgressBar.Guitar);
                CDTXMania.tDisposeSafely(ref this.txCurrentProgressBar.Bass);
                base.OnManagedReleaseResources();
            }
        }
        public override int OnUpdateAndDraw()
        {
            if (base.bNotActivated)
            {
                return 0;
            }
            if (base.bJustStartedUpdate)
            {
                this.ct表示用 = new CCounter(0, 999, 3, CDTXMania.Timer);
                base.bJustStartedUpdate = false;
            }
            this.ct表示用.tUpdate();


            for (int j = 0; j < 3; j++)
            {
                if ( this.n本体X[j] != 0 )
                {
                    string str = string.Format("{0:0.00}", ((float)CDTXMania.DTX.LEVEL[j]) / 10.0f + (CDTXMania.DTX.LEVELDEC[j] != 0 ? CDTXMania.DTX.LEVELDEC[j] / 100.0f : 0));
                    bool bCLASSIC = false;
                    //If Skill Mode is CLASSIC, always display lvl as Classic Style
                    if (CDTXMania.ConfigIni.nSkillMode == 0 || (CDTXMania.ConfigIni.bCLASSIC譜面判別を有効にする &&
                        (CDTXMania.DTX.bチップがある.LeftCymbal == false) &&
                        (CDTXMania.DTX.bチップがある.LP == false) &&
                        (CDTXMania.DTX.bチップがある.LBD == false) &&
                        (CDTXMania.DTX.bチップがある.FT == false) &&
                        (CDTXMania.DTX.bチップがある.Ride == false) &&
                        (CDTXMania.DTX.b強制的にXG譜面にする == false)))
                    {
                        str = string.Format("{0:00}", CDTXMania.DTX.LEVEL[j]);
                        bCLASSIC = true;
                    }
                    
                    this.txSkillPanel.tDraw2D(CDTXMania.app.Device, this.n本体X[j], this.n本体Y);
                    this.txネームプレート用文字[ j ].tDraw2D(CDTXMania.app.Device, this.n本体X[j], this.n本体Y);

                    this.tDrawStringSmall(80 + this.n本体X[j], 72 + this.n本体Y, string.Format("{0,4:###0}", CDTXMania.stageResult.stPerformanceEntry[j].nPerfectCount_ExclAuto));
                    this.tDrawStringSmall(80 + this.n本体X[j], 102 + this.n本体Y, string.Format("{0,4:###0}", CDTXMania.stageResult.stPerformanceEntry[j].nGreatCount_ExclAuto));
                    this.tDrawStringSmall(80 + this.n本体X[j], 132 + this.n本体Y, string.Format("{0,4:###0}", CDTXMania.stageResult.stPerformanceEntry[j].nGoodCount_ExclAuto));
                    this.tDrawStringSmall(80 + this.n本体X[j], 162 + this.n本体Y, string.Format("{0,4:###0}", CDTXMania.stageResult.stPerformanceEntry[j].nPoorCount_ExclAuto));
                    this.tDrawStringSmall(80 + this.n本体X[j], 192 + this.n本体Y, string.Format("{0,4:###0}", CDTXMania.stageResult.stPerformanceEntry[j].nMissCount_ExclAuto));
                    this.tDrawStringSmall(80 + this.n本体X[j], 222 + this.n本体Y, string.Format("{0,4:###0}", CDTXMania.stageResult.stPerformanceEntry[j].nMaxCombo));


                    this.tDrawStringSmall(167 + this.n本体X[j], 72 + this.n本体Y, string.Format("{0,3:##0}%", (int)Math.Round(CDTXMania.stageResult.fPerfect率[j])));
                    this.tDrawStringSmall(167 + this.n本体X[j], 102 + this.n本体Y, string.Format("{0,3:##0}%", (int)Math.Round(CDTXMania.stageResult.fGreat率[j])));
                    this.tDrawStringSmall(167 + this.n本体X[j], 132 + this.n本体Y, string.Format("{0,3:##0}%", (int)Math.Round(CDTXMania.stageResult.fGood率[j])));
                    this.tDrawStringSmall(167 + this.n本体X[j], 162 + this.n本体Y, string.Format("{0,3:##0}%", (int)Math.Round(CDTXMania.stageResult.fPoor率[j])));
                    this.tDrawStringSmall(167 + this.n本体X[j], 192 + this.n本体Y, string.Format("{0,3:##0}%", (int)Math.Round(CDTXMania.stageResult.fMiss率[j])));
                    this.tDrawStringSmall(167 + this.n本体X[j], 222 + this.n本体Y, string.Format("{0,3:##0}%", (int)Math.Round((100.0 * CDTXMania.stageResult.stPerformanceEntry[j].nMaxCombo / CDTXMania.stageResult.stPerformanceEntry[j].nTotalChipsCount))));

                    //this.tDrawStringLarge(58 + this.n本体X[j], 277 + this.n本体Y, string.Format("{0,6:##0.00}", CDTXMania.stageResult.stPerformanceEntry[j].dbPerformanceSkill));
                    //Conditional checks for MAX
                    if(this.txSkillMax != null && CDTXMania.stageResult.stPerformanceEntry[j].dbPerformanceSkill >= 100.0)
                    {
                        this.txSkillMax.tDraw2D(CDTXMania.app.Device, 127 + this.n本体X[j], 277 + this.n本体Y);
                    }
                    else
                    {
                        this.tDrawStringLarge(58 + this.n本体X[j], 277 + this.n本体Y, string.Format("{0,6:##0.00}", CDTXMania.stageResult.stPerformanceEntry[j].dbPerformanceSkill));
                        if(this.txPercent != null)
                            this.txPercent.tDraw2D(CDTXMania.app.Device, 217 + this.n本体X[j], 287 + this.n本体Y);
                    }

                    this.tDrawStringLarge(88 + this.n本体X[j], 363 + this.n本体Y, string.Format("{0,6:##0.00}", CDTXMania.stageResult.stPerformanceEntry[j].dbGameSkill));
                    
                    if(this.tx難易度パネル != null)
                        this.tx難易度パネル.tDraw2D(CDTXMania.app.Device, 14 + this.n本体X[j], 266 + this.n本体Y, new Rectangle( this.rectDiffPanelPoint.X, this.rectDiffPanelPoint.Y, 60, 60));
                    this.tレベル数字描画((bCLASSIC == true ? 26 : 18) + this.n本体X[j], 290 + this.n本体Y, str);

                    //Draw Progress Bar Panels first
                    if(this.txProgressBarPanel != null)
                    {
                        this.txProgressBarPanel.tDraw2D(CDTXMania.app.Device, 255 + this.n本体X[j], 1 + this.n本体Y);
                    }

                    //Draw Progress Bars
                    this.txCurrentProgressBar[j].tDraw2D(CDTXMania.app.Device, 256 + this.n本体X[j], 2 + this.n本体Y);
                    this.txPreviousBestProgressBar[j].tDraw2D(CDTXMania.app.Device, 270 + this.n本体X[j], 2 + this.n本体Y);

                    string strScore = string.Format("{0,7:######0}", CDTXMania.stageResult.stPerformanceEntry[j].nスコア);
                    for (int i = 0; i < 7; i++)
                    {
                        Rectangle rectangle;
                        char ch = strScore[i];
                        if (ch.Equals(' '))
                        {
                            rectangle = new Rectangle(0, 0, 0, 0);
                        }
                        else
                        {
                            int num4 = int.Parse(strScore.Substring(i, 1));
                            rectangle = new Rectangle(num4 * 36, 0, 36, 50);
                        }
                        if (this.txScore != null)
                        {
                            this.txScore.tDraw2D(CDTXMania.app.Device, this.nスコアX[j] + (i * 34), 58, rectangle);
                        }
                    }
                    if (this.txScore != null)
                    {
                        this.txScore.tDraw2D(CDTXMania.app.Device, this.nスコアX[j], 30, new Rectangle(0, 50, 86, 28));
                    }

                    for (int i = 0; i < 1; i++)
                    {
                        if (CDTXMania.stageResult.bNewRecordSkill[i])
                        {
                            this.txNewRecord.tDraw2D( CDTXMania.app.Device, 118 + this.n本体X[j], 322 + this.n本体Y );
                        }
                    }
                    if (this.ct表示用.nCurrentValue >= 900)
                    {
                        if (CDTXMania.stageResult.stPerformanceEntry[j].nPerfectCount == CDTXMania.stageResult.stPerformanceEntry[j].nTotalChipsCount)
                        {
                            //えくせ
                        }
                        else if (CDTXMania.stageResult.stPerformanceEntry[j].bIsFullCombo && CDTXMania.stageResult.stPerformanceEntry[j].nPerfectCount != CDTXMania.stageResult.stPerformanceEntry[j].nTotalChipsCount)
                        {
                            //ふるこん
                        }
                    }
                                        
                    //Draw Lag Counters if Lag Display is on
                    if (CDTXMania.ConfigIni.bShowLagHitCount)
                    {
                        //Type-A is Early-Blue, Late-Red
                        bool bTypeAColor = CDTXMania.ConfigIni.nShowLagTypeColor == 0;

                        this.tDrawLagCounterText(this.n本体X[j] + 170, this.n本体Y + 335,
                            string.Format("{0,4:###0}", CDTXMania.stageResult.nTimingHitCount[j].nEarly), !bTypeAColor);
                        this.tDrawLagCounterText(this.n本体X[j] + 245, this.n本体Y + 335,
                            string.Format("{0,4:###0}", CDTXMania.stageResult.nTimingHitCount[j].nLate), bTypeAColor);
                    }
                }
            }
            if (!this.ct表示用.bReachedEndValue)
            {
                return 0;
            }
            return 1;
        }
		// Other

		#region [ private ]
		//-----------------
		[StructLayout( LayoutKind.Sequential )]
		private struct ST文字位置
		{
			public char ch;
			public Point pt;
		}

        //-----------------
        [StructLayout(LayoutKind.Sequential)]
        private struct ST文字位置Ex
        {
            public char ch;
            public Rectangle rect;
        }

        private CCounter ct表示用;
        private STDGBVALUE<int> n本体X;
        private int n本体Y;
        private STDGBVALUE<int> nスコアX;
        private readonly Point[] ptFullCombo位置;
        private CSound sdDTXで指定されたフルコンボ音;
        private readonly ST文字位置[] stSmallStringPosition;
        private readonly ST文字位置[] stLargeStringPosition;
        private readonly ST文字位置[] st特大文字位置;
        private readonly ST文字位置[] st難易度数字位置;
        private CTexture txNewRecord;
        private CTexture txパネル本体;
        private CTexture[] txCharacter;
        private CTexture[] txExciteGauge;
        private CTexture txSkillPanel;
        private CTexture txScore;
        private CTexture[] txネームプレート用文字 = new CTexture[ 3 ];
        private string strPlayerName;
        private string strTitleName;
        private CPrivateFastFont prv表示用フォント;
        private CPrivateFastFont prv称号フォント;

        private CTexture tx難易度パネル;
        private CTexture tx難易度用数字;
        protected Rectangle rectDiffPanelPoint;
        //New texture % and MAX
        private CTexture txPercent;
        private CTexture txSkillMax;
        //
        private STDGBVALUE<CTexture> txPreviousBestProgressBar;
        private STDGBVALUE<CTexture> txCurrentProgressBar;
        //
        private CTexture txProgressBarPanel;
        private CTexture txLagHitCount;

        private readonly ST文字位置Ex[] stLagCountBlueText;//15x19 start at 0,0
        private readonly ST文字位置Ex[] stLagCountRedText;//15x19 start at 64,64

        private void tDrawStringSmall(int x, int y, string str)
        {
            foreach (char ch in str)
            {
                for (int i = 0; i < this.stSmallStringPosition.Length; i++)
                {
                    if (this.stSmallStringPosition[i].ch == ch)
                    {
                        Rectangle rectangle = new Rectangle(this.stSmallStringPosition[i].pt.X, this.stSmallStringPosition[i].pt.Y, 20, 26);
                        if (this.txCharacter[0] != null)
                        {
                            this.txCharacter[0].tDraw2D(CDTXMania.app.Device, x, y, rectangle);
                        }
                        break;
                    }
                }
                x += 20;
            }
        }
		private void tDrawStringSmall( int x, int y, string str, bool b強調 )
		{
			foreach( char ch in str )
			{
				for( int i = 0; i < this.stSmallStringPosition.Length; i++ )
				{
					if( this.stSmallStringPosition[ i ].ch == ch )
					{
						Rectangle rectangle = new Rectangle( this.stSmallStringPosition[ i ].pt.X, this.stSmallStringPosition[ i ].pt.Y, 14, 0x12 );
						if( ch == '%' )
						{
							rectangle.Width -= 2;
							rectangle.Height -= 2;
						}
						if( this.txCharacter[ 0 ] != null )
						{
							this.txCharacter[ 0 ].tDraw2D( CDTXMania.app.Device, x, y, rectangle );
						}
						break;
					}
				}
				x += 11;
			}
		}
		private void tDrawStringLarge( int x, int y, string str )
		{
			this.tDrawStringLarge( x, y, str, false );
		}
		private void tDrawStringLarge( int x, int y, string str, bool b強調 )
		{
			foreach( char ch in str )
			{
				for( int i = 0; i < this.stLargeStringPosition.Length; i++ )
				{
					if( this.stLargeStringPosition[ i ].ch == ch )
					{
						Rectangle rectangle = new Rectangle( this.stLargeStringPosition[ i ].pt.X, this.stLargeStringPosition[ i ].pt.Y, 28, 42 );
						if( ch == '.' )
						{
							rectangle.Width -= 18;
						}
						if( this.txCharacter[ 1 ] != null )
						{
							this.txCharacter[ 1 ].tDraw2D( CDTXMania.app.Device, x, y, rectangle );
						}
						break;
					}
				}
                x += (ch == '.' ? 12 : 28);
			}
		}

        //Note: Lag Text is draw right-justified
        //i.e. x,y is the top right corner of rect
        private void tDrawLagCounterText(int x, int y, string str, bool isRed)
        {
            ST文字位置Ex[] currTextPosStructArray = isRed ? this.stLagCountRedText : this.stLagCountBlueText;

            for (int j = str.Length - 1; j >= 0; j--)
            {
                for (int i = 0; i < currTextPosStructArray.Length; i++)
                {
                    if (currTextPosStructArray[i].ch == str[j])
                    {
                        Rectangle rectangle = new Rectangle(
                            currTextPosStructArray[i].rect.X,
                            currTextPosStructArray[i].rect.Y,
                            currTextPosStructArray[i].rect.Width,
                            currTextPosStructArray[i].rect.Height);

                        if (this.txLagHitCount != null)
                        {
                            this.txLagHitCount.tDraw2D(CDTXMania.app.Device, x - currTextPosStructArray[i].rect.Width, y, rectangle);
                        }
                        break;
                    }
                }
                //15 is width of char in txLag
                x -= 15;
            }
        }
        private void t特大文字表示(int x, int y, string str)
        {
            this.t特大文字表示(x, y, str, false);
        }
        private void t特大文字表示(int x, int y, string str, bool bExtraLarge)
        {
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                for (int j = 0; j < this.st特大文字位置.Length; j++)
                {
                    if (this.st特大文字位置[j].ch == c)
                    {
                        int num;
                        int num2;
                        if (bExtraLarge)
                        {
                            if (j < 5)
                            {
                                num = 6 * j;
                            }
                            else
                            {
                                if (j < 11)
                                {
                                    num = 6 * (j - 5);
                                }
                                else
                                {
                                    num = 24;
                                }
                            }
                            if (j < 5)
                            {
                                num2 = 48;
                            }
                            else
                            {
                                if (j < 11)
                                {
                                    num2 = 56;
                                }
                                else
                                {
                                    num2 = 48;
                                }
                            }
                        }
                        else
                        {
                            num = 0;
                            num2 = 0;
                        }
                        Rectangle rc画像内の描画領域 = new Rectangle(this.st特大文字位置[j].pt.X + num, this.st特大文字位置[j].pt.Y + num2, bExtraLarge ? 24 : 18, bExtraLarge ? 32 : 24);
                        if (c == '.')
                        {
                            rc画像内の描画領域.Width -= 2;
                            rc画像内の描画領域.Height -= 2;
                        }
                        if (this.txCharacter[2] != null)
                        {
                            this.txCharacter[2].tDraw2D(CDTXMania.app.Device, x, y, rc画像内の描画領域);
                        }
                        break;
                    }
                }
                if (bExtraLarge)
                {
                    if (c == '.')
                    {
                        x += 20;
                    }
                    else
                    {
                        x += 23;
                    }
                }
                else
                {
                    if (c == '.')
                    {
                        x += 14;
                    }
                    else
                    {
                        x += 17;
                    }
                }
            }
        }


        private void tレベル数字描画(int x, int y, string str)
        {
            foreach (char ch in str)
            {
                for (int i = 0; i < this.st難易度数字位置.Length; i++)
                {
                    if (this.st難易度数字位置[i].ch == ch)
                    {
                        Rectangle rectangle = new Rectangle(this.st難易度数字位置[i].pt.X, this.st難易度数字位置[i].pt.Y, 16, 32);
                        if (ch == '.')
                        {
                            rectangle.Width -= 11;
                        }
                        if (this.tx難易度用数字 != null)
                        {
                            this.tx難易度用数字.tDraw2D(CDTXMania.app.Device, x, y, rectangle);
                        }
                        break;
                    }
                }
                x += (ch == '.' ? 5 : 16);
            }
        }
 
        public void tGetDifficultyLabelFromScript( string strラベル名 )
        {
            string strRawScriptFile;

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

                    if( arScriptLine[ 0 ] != "7" )
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
                    this.rectDiffPanelPoint.X = Convert.ToInt32( arScriptLine[ 2 ] );
                    this.rectDiffPanelPoint.Y = Convert.ToInt32( arScriptLine[ 3 ] );

                    reader.Close();
                    break;
                }
            }
        }


		//-----------------
		#endregion
	}
}
