using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Runtime.InteropServices;
using SharpDX;
using FDK;

using Rectangle = System.Drawing.Rectangle;

namespace DTXMania
{
	internal class CActPerfGuitarJudgementString : CActPerfCommonJudgementString
	{
		// コンストラクタ

		public CActPerfGuitarJudgementString()
		{
			this.stレーンサイズ = new STレーンサイズ[ 15 ];
			STレーンサイズ stレーンサイズ = new STレーンサイズ();
            int[,] sizeXW = new int[,] 	{{30, 36},{71, 30},
		
		{
			135,
			30
		},
		
		{
			202,
			30
		},
		
		{
			167,
			30
		},
		
		{
			237,
			30
		},
		
		{
			269,
			30
		},
		
		{
			333,
			36
		},
		
		{
			103,
			30
		},
		
		{
			301,
			30
		},
		
		{
			103,
			30
		},
		
		{
			0,
			0
		},
		
		{
			0,
			0
		},
		
		{
			26,
			111
		},
		
		{
			480,
			111
		}
	};
			for ( int i = 0; i < 15; i++ )
			{
				this.stレーンサイズ[ i ] = new STレーンサイズ();
				this.stレーンサイズ[ i ].x = sizeXW[ i, 0 ];
				this.stレーンサイズ[ i ].w = sizeXW[ i, 1 ];
			}
			base.bNotActivated = true; 
		}


		// CActivity 実装（共通クラスからの差分のみ）

		public override int OnUpdateAndDraw()
		{
			if( !base.bNotActivated  && (CDTXMania.ConfigIni.bDisplayJudge.Guitar || CDTXMania.ConfigIni.bDisplayJudge.Bass))
			{
                int index = 0;
                #region[ 座標など定義 ]
                if( CDTXMania.ConfigIni.nJudgeAnimeType == 1 )
                {
                    #region[ コマ方式 ]
                    for (int i = 0; i < 15; i++)
                    {
                        if (!base.st状態[i].ct進行.bStopped)
                        {
                            base.st状態[i].ct進行.tUpdate();
                            if (base.st状態[i].ct進行.bReachedEndValue)
                            {
                                base.st状態[i].ct進行.tStop();
                            }
                            base.st状態[i].nRect = base.st状態[i].ct進行.nCurrentValue;
                        }
                        index++;
                    }
                    #endregion
                }
                else if( CDTXMania.ConfigIni.nJudgeAnimeType == 2 )
                {
                    #region[ 新しいやつ ]
                    for (int i = 0; i < 15; i++)
                    {
                        if (!base.st状態[i].ct進行.bStopped)
                        {
                            base.st状態[i].ct進行.tUpdate();
                            if (base.st状態[i].ct進行.bReachedEndValue)
                            {
                                base.st状態[i].ct進行.tStop();
                            }
                            //int num2 = base.st状態[i].ctUpdate.nCurrentValue;
                            int nNowFrame = base.st状態[ i ].ct進行.nCurrentValue;

                            //テンプレのようなもの。
                            //拡大処理を先に行わないとめちゃくちゃになる。
                            /*
                            base.st状態[i].fX方向拡大率 = 1.0f;
                            base.st状態[i].fY方向拡大率 = 1.0f;
                            base.st状態[i].n相対X座標 = 0;
                            base.st状態[i].n相対Y座標 = 0;
                            base.st状態[i].nTransparency = 0;
                            */

                            //base.st状態[i].judge = EJudgement.Perfect;
                            //nNowFrame = 22;
                            if( base.st状態[ i ].judge == EJudgement.Perfect )
                            {
                                #region[ PERFECT ]
                                #region[ 0～10 ]
                                if( nNowFrame == 0 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.67f;
                                    base.st状態[i].fY方向拡大率 = 1.67f;

                                    base.st状態[i].fZ軸回転度 = 0;
                                    //base.st状態[i].fX方向拡大率 = 1f;
                                    //base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].n相対X座標 = 28;
                                    base.st状態[i].n相対Y座標 = 0;
                                    base.st状態[i].n透明度 = 0;
                                    
                                    base.st状態[i].fX方向拡大率_棒 = 0f;
                                    base.st状態[i].fY方向拡大率_棒 = 0f;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian( -43f );
                                }
                                else if( nNowFrame == 1 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.33f;
                                    base.st状態[i].fY方向拡大率 = 1.33f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 7f );
                                    base.st状態[i].n相対X座標 = 26;
                                    base.st状態[i].n相対Y座標 = 4;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 0.63f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -98;
                                    base.st状態[i].n相対Y座標_棒 = 6;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian( -43f );
                                }
                                else if( nNowFrame == 2 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率B = 1f;
                                    base.st状態[i].fY方向拡大率B = 1f;
                                    base.st状態[i].n相対X座標B = -2;
                                    base.st状態[i].n相対Y座標B = 2;
                                    base.st状態[i].n透明度B = 127;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -40;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(-14.5f);
                                }
                                else if( nNowFrame == 3 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率B = 1.1f;
                                    base.st状態[i].fY方向拡大率B = 1.1f;
                                    base.st状態[i].n相対X座標B = -3;
                                    base.st状態[i].n相対Y座標B = 1;
                                    base.st状態[i].n透明度B = 127;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -40;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(15f);
                                }
                                else if( nNowFrame == 4 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率B = 1.2f;
                                    base.st状態[i].fY方向拡大率B = 1.2f;
                                    base.st状態[i].n相対X座標B = -4;
                                    base.st状態[i].n相対Y座標B = 0;
                                    base.st状態[i].n透明度B = 127;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -40;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(18.5f);
                                }
                                else if( nNowFrame == 5 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率B = 1.25f;
                                    base.st状態[i].fY方向拡大率B = 1.25f;
                                    base.st状態[i].n相対X座標B = -5;
                                    base.st状態[i].n相対Y座標B = -1;
                                    base.st状態[i].n透明度B = 127;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -38;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(20.5f);
                                }
                                else if( nNowFrame == 6 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率B = 1.3f;
                                    base.st状態[i].fY方向拡大率B = 1.3f;
                                    base.st状態[i].n相対X座標B = -6;
                                    base.st状態[i].n相対Y座標B = -2;
                                    base.st状態[i].n透明度B = 127;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -38;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(20.5f);
                                }
                                else if( nNowFrame == 7 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率B = 1.35f;
                                    base.st状態[i].fY方向拡大率B = 1.35f;
                                    base.st状態[i].n相対X座標B = -7;
                                    base.st状態[i].n相対Y座標B = -3;
                                    base.st状態[i].n透明度B = 127;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -39;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(22f);
                                }
                                else if( nNowFrame == 8 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率B = 1.4f;
                                    base.st状態[i].fY方向拡大率B = 1.4f;
                                    base.st状態[i].n相対X座標B = -8;
                                    base.st状態[i].n相対Y座標B = -4;
                                    base.st状態[i].n透明度B = 127;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -40;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(23.5f);
                                }
                                else if( nNowFrame == 9 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率B = 1.45f;
                                    base.st状態[i].fY方向拡大率B = 1.45f;
                                    base.st状態[i].n相対X座標B = -9;
                                    base.st状態[i].n相対Y座標B = -5;
                                    base.st状態[i].n透明度B = 112;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -40;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(25.5f);
                                }
                                else if( nNowFrame == 10 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;


                                    base.st状態[i].fX方向拡大率B = 1.5f;
                                    base.st状態[i].fY方向拡大率B = 1.5f;
                                    base.st状態[i].n相対X座標B = -10;
                                    base.st状態[i].n相対Y座標B = -6;
                                    base.st状態[i].n透明度B = 100;


                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -40;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(27f);
                                }
                                #endregion
                                #region[ 11～18 ]
                                else if( nNowFrame == 11 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率B = 1.55f;
                                    base.st状態[i].fY方向拡大率B = 1.55f;
                                    base.st状態[i].n相対X座標B = -11;
                                    base.st状態[i].n相対Y座標B = -7;
                                    base.st状態[i].n透明度B = 70;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -40;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(29.5f);
                                }
                                else if( nNowFrame == 12 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率B = 1.6f;
                                    base.st状態[i].fY方向拡大率B = 1.6f;
                                    base.st状態[i].n相対X座標B = -12;
                                    base.st状態[i].n相対Y座標B = -8;
                                    base.st状態[i].n透明度B = 40;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -38;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(31f);
                                }
                                else if( nNowFrame == 13 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率B = 1.65f;
                                    base.st状態[i].fY方向拡大率B = 1.65f;
                                    base.st状態[i].n相対X座標B = -13;
                                    base.st状態[i].n相対Y座標B = -9;
                                    base.st状態[i].n透明度B = 40;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -38;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(32.5f);
                                }
                                else if( nNowFrame == 14 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率B = 1.7f;
                                    base.st状態[i].fY方向拡大率B = 1.7f;
                                    base.st状態[i].n相対X座標B = -14;
                                    base.st状態[i].n相対Y座標B = -10;
                                    base.st状態[i].n透明度B = 20;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -38;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(34f);
                                }
                                else if( nNowFrame == 15 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率B = 1f;
                                    base.st状態[i].fY方向拡大率B = 1f;
                                    base.st状態[i].n相対X座標B = -14;
                                    base.st状態[i].n相対Y座標B = -10;
                                    base.st状態[i].n透明度B = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -38;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(36f);
                                }
                                else if( nNowFrame == 16 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -38;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(38f);
                                }
                                else if( nNowFrame == 17 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -46;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(40.5f);
                                }
                                else if( nNowFrame == 18 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -46;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(43f);
                                }
                                #endregion
                                #region[ 19～23 ]
                                else if( nNowFrame == 19 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.22f;
                                    base.st状態[i].fY方向拡大率 = 0.77f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = 16;
                                    base.st状態[i].n相対Y座標 = -2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.1f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -55;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(43f);
                                }
                                else if( nNowFrame == 20 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.45f;
                                    base.st状態[i].fY方向拡大率 = 0.64f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = 36;
                                    base.st状態[i].n相対Y座標 = -6;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 0.9f;
                                    base.st状態[i].fY方向拡大率_棒 = 0.7f;
                                    base.st状態[i].n相対X座標_棒 = -70;
                                    base.st状態[i].n相対Y座標_棒 = 4;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(43f);
                                }
                                else if( nNowFrame == 21 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.70f;
                                    base.st状態[i].fY方向拡大率 = 0.41f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = 57;
                                    base.st状態[i].n相対Y座標 = -9;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 0.6f;
                                    base.st状態[i].fY方向拡大率_棒 = 0.45f;
                                    base.st状態[i].n相対X座標_棒 = -98;
                                    base.st状態[i].n相対Y座標_棒 = 2;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(43f);
                                }
                                else if( nNowFrame == 22 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.91f;
                                    base.st状態[i].fY方向拡大率 = 0.23f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = 75;
                                    base.st状態[i].n相対Y座標 = -12;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 0.4f;
                                    base.st状態[i].fY方向拡大率_棒 = 0.25f;
                                    base.st状態[i].n相対X座標_棒 = -120;
                                    base.st状態[i].n相対Y座標_棒 = 2;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(43f);
                                }
                                else if( nNowFrame == 23 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.91f;
                                    base.st状態[i].fY方向拡大率 = 0.23f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( 15f );
                                    base.st状態[i].n相対X座標 = 75;
                                    base.st状態[i].n相対Y座標 = -12;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 0f;
                                    base.st状態[i].fY方向拡大率_棒 = 0f;
                                    base.st状態[i].n相対X座標_棒 = -120;
                                    base.st状態[i].n相対Y座標_棒 = 2;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(43f);
                                }
                                #endregion
                                #endregion
                            }
                            else if( base.st状態[ i ].judge == EJudgement.Great )
                            {
                                #region[ GREAT ]
                                #region[ 0～10 ]
                                if (nNowFrame == 0)
                                {
                                    base.st状態[i].fX方向拡大率 = 1.67f;
                                    base.st状態[i].fY方向拡大率 = 1.67f;

                                    base.st状態[i].fZ軸回転度 = 0;
                                    //base.st状態[i].fX方向拡大率 = 1f;
                                    //base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].n相対X座標 = 28;
                                    base.st状態[i].n相対Y座標 = 0;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 0f;
                                    base.st状態[i].fY方向拡大率_棒 = 0f;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(-43f);
                                }
                                else if (nNowFrame == 1)
                                {
                                    base.st状態[i].fX方向拡大率 = 1.33f;
                                    base.st状態[i].fY方向拡大率 = 1.33f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(7f);
                                    base.st状態[i].n相対X座標 = 26;
                                    base.st状態[i].n相対Y座標 = 4;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 0.63f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -98;
                                    base.st状態[i].n相対Y座標_棒 = 6;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(-43f);
                                }
                                else if (nNowFrame == 2)
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -40;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(-14.5f);
                                }
                                else if (nNowFrame == 3)
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -40;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(15f);
                                }
                                else if (nNowFrame == 4)
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -40;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(18.5f);
                                }
                                else if (nNowFrame == 5)
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -38;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(20.5f);
                                }
                                else if (nNowFrame == 6)
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -38;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(20.5f);
                                }
                                else if (nNowFrame == 7)
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -39;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(22f);
                                }
                                else if (nNowFrame == 8)
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -40;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(23.5f);
                                }
                                else if (nNowFrame == 9)
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -40;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(25.5f);
                                }
                                else if (nNowFrame == 10)
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -40;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(27f);
                                }
                                #endregion
                                #region[ 11～18 ]
                                else if (nNowFrame == 11)
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -40;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(29.5f);
                                }
                                else if (nNowFrame == 12)
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -38;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(31f);
                                }
                                else if (nNowFrame == 13)
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -38;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(32.5f);
                                }
                                else if (nNowFrame == 14)
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -38;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(34f);
                                }
                                else if (nNowFrame == 15)
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -38;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(36f);
                                }
                                else if (nNowFrame == 16)
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -38;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(38f);
                                }
                                else if (nNowFrame == 17)
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -46;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(40.5f);
                                }
                                else if (nNowFrame == 18)
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = -2;
                                    base.st状態[i].n相対Y座標 = 2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.25f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -46;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(43f);
                                }
                                #endregion
                                #region[ 19～23 ]
                                else if (nNowFrame == 19)
                                {
                                    base.st状態[i].fX方向拡大率 = 1.22f;
                                    base.st状態[i].fY方向拡大率 = 0.77f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = 16;
                                    base.st状態[i].n相対Y座標 = -2;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 1.1f;
                                    base.st状態[i].fY方向拡大率_棒 = 1f;
                                    base.st状態[i].n相対X座標_棒 = -55;
                                    base.st状態[i].n相対Y座標_棒 = 10;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(43f);
                                }
                                else if (nNowFrame == 20)
                                {
                                    base.st状態[i].fX方向拡大率 = 1.45f;
                                    base.st状態[i].fY方向拡大率 = 0.64f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = 36;
                                    base.st状態[i].n相対Y座標 = -6;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 0.9f;
                                    base.st状態[i].fY方向拡大率_棒 = 0.7f;
                                    base.st状態[i].n相対X座標_棒 = -70;
                                    base.st状態[i].n相対Y座標_棒 = 4;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(43f);
                                }
                                else if (nNowFrame == 21)
                                {
                                    base.st状態[i].fX方向拡大率 = 1.70f;
                                    base.st状態[i].fY方向拡大率 = 0.41f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = 57;
                                    base.st状態[i].n相対Y座標 = -9;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 0.6f;
                                    base.st状態[i].fY方向拡大率_棒 = 0.45f;
                                    base.st状態[i].n相対X座標_棒 = -98;
                                    base.st状態[i].n相対Y座標_棒 = 2;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(43f);
                                }
                                else if (nNowFrame == 22)
                                {
                                    base.st状態[i].fX方向拡大率 = 1.91f;
                                    base.st状態[i].fY方向拡大率 = 0.23f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = 75;
                                    base.st状態[i].n相対Y座標 = -12;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 0.4f;
                                    base.st状態[i].fY方向拡大率_棒 = 0.25f;
                                    base.st状態[i].n相対X座標_棒 = -120;
                                    base.st状態[i].n相対Y座標_棒 = 2;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(43f);
                                }
                                else if (nNowFrame == 23)
                                {
                                    base.st状態[i].fX方向拡大率 = 1.91f;
                                    base.st状態[i].fY方向拡大率 = 0.23f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian(15f);
                                    base.st状態[i].n相対X座標 = 75;
                                    base.st状態[i].n相対Y座標 = -12;
                                    base.st状態[i].n透明度 = 0;

                                    base.st状態[i].fX方向拡大率_棒 = 0f;
                                    base.st状態[i].fY方向拡大率_棒 = 0f;
                                    base.st状態[i].n相対X座標_棒 = -120;
                                    base.st状態[i].n相対Y座標_棒 = 2;
                                    base.st状態[i].fZ軸回転度_棒 = CConversion.DegreeToRadian(43f);
                                }
                                #endregion
                                #endregion
                            }
                            else if( base.st状態[ i ].judge == EJudgement.Good )
                            {
                                #region[ GOOD ]
                                if( nNowFrame == 0 )
                                {
                                    base.st状態[i].fX方向拡大率 = 0.625f;
                                    base.st状態[i].fY方向拡大率 = 3.70f;
                                    base.st状態[i].n相対X座標 = -19;
                                    base.st状態[i].n相対Y座標 = 0;
                                    base.st状態[i].n透明度 = 0;
                                }
                                else if( nNowFrame == 1 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.125f;
                                    base.st状態[i].fY方向拡大率 = 2.00f;
                                    base.st状態[i].n相対X座標 = 4;
                                    base.st状態[i].n相対Y座標 = 1;
                                    base.st状態[i].n透明度 = 0;
                                }
                                else if( nNowFrame == 2 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.375f;
                                    base.st状態[i].fY方向拡大率 = 0.66f;
                                    base.st状態[i].n相対X座標 = 13;
                                    base.st状態[i].n相対Y座標 = 1;
                                    base.st状態[i].n透明度 = 0;
                                }
                                else if( nNowFrame == 3 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.25f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].n相対X座標 = 8;
                                    base.st状態[i].n相対Y座標 = 1;
                                    base.st状態[i].n透明度 = 0;
                                }
                                else if( nNowFrame >= 4 && nNowFrame <= 18 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].n相対X座標 = 0;
                                    base.st状態[i].n相対Y座標 = 1;
                                    base.st状態[i].n透明度 = 0;
                                }
                                else if( nNowFrame == 19 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.25f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].n相対X座標 = 8;
                                    base.st状態[i].n相対Y座標 = 1;
                                    base.st状態[i].n透明度 = 0;
                                }
                                else if( nNowFrame == 20 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.375f;
                                    base.st状態[i].fY方向拡大率 = 0.66f;
                                    base.st状態[i].n相対X座標 = 13;
                                    base.st状態[i].n相対Y座標 = 1;
                                    base.st状態[i].n透明度 = 0;
                                }
                                else if( nNowFrame == 21 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.50f;
                                    base.st状態[i].fY方向拡大率 = 0.50f;
                                    base.st状態[i].n相対X座標 = 20;
                                    base.st状態[i].n相対Y座標 = 1;
                                    base.st状態[i].n透明度 = 0;
                                }
                                else if( nNowFrame == 22 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.91f;
                                    base.st状態[i].fY方向拡大率 = 0.23f;
                                    base.st状態[i].n相対X座標 = 37;
                                    base.st状態[i].n相対Y座標 = 1;
                                    base.st状態[i].n透明度 = 0;
                                }
                                else if( nNowFrame == 23 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.91f;
                                    base.st状態[i].fY方向拡大率 = 0.23f;
                                    base.st状態[i].n相対X座標 = 37;
                                    base.st状態[i].n相対Y座標 = 1;
                                    base.st状態[i].n透明度 = 0;
                                }
                                #endregion
                            }
                            else if( base.st状態[ i ].judge == EJudgement.Poor || base.st状態[ i ].judge == EJudgement.Miss )
                            {
                                #region[ POOR & MISS ]
                                if( nNowFrame == 0 )
                                {
                                    base.st状態[i].n相対X座標 = 0;
                                    base.st状態[i].n相対Y座標 = -18;
                                    base.st状態[i].n透明度 = 100;
                                }
                                else if( nNowFrame == 1 )
                                {
                                    base.st状態[i].n相対X座標 = 0;
                                    base.st状態[i].n相対Y座標 = -12;
                                    base.st状態[i].n透明度 = 140;
                                }
                                else if( nNowFrame == 2 )
                                {
                                    base.st状態[i].n相対X座標 = 0;
                                    base.st状態[i].n相対Y座標 = -6;
                                    base.st状態[i].n透明度 = 190;
                                }
                                else if( nNowFrame == 3 )
                                {
                                    base.st状態[i].n相対X座標 = 0;
                                    base.st状態[i].n相対Y座標 = 0;
                                    base.st状態[i].n透明度 = 220;
                                }
                                else if( nNowFrame == 4 )
                                {
                                    base.st状態[i].n相対X座標 = 0;
                                    base.st状態[i].n相対Y座標 = -4;
                                    base.st状態[i].n透明度 = 255;
                                }
                                else if( nNowFrame == 5 )
                                {
                                    base.st状態[i].n相対X座標 = 0;
                                    base.st状態[i].n相対Y座標 = -6;
                                    base.st状態[i].n透明度 = 255;
                                }
                                else if( nNowFrame >= 6 && nNowFrame <= 18 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].n相対X座標 = 0;
                                    base.st状態[i].n相対Y座標 = 0;
                                    base.st状態[i].n透明度 = 255;
                                }
                                else if( nNowFrame == 19 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( -4f );
                                    base.st状態[i].n相対X座標 = 0;
                                    base.st状態[i].n相対Y座標 = 0;
                                    base.st状態[i].n透明度 = 220;
                                }
                                else if( nNowFrame == 20 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( -8f );
                                    base.st状態[i].n相対X座標 = 0;
                                    base.st状態[i].n相対Y座標 = 6;
                                    base.st状態[i].n透明度 = 190;
                                }
                                else if( nNowFrame == 21 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( -8f );
                                    base.st状態[i].n相対X座標 = 20;
                                    base.st状態[i].n相対Y座標 = 12;
                                    base.st状態[i].n透明度 = 140;
                                }
                                else if( nNowFrame == 22 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( -12f );
                                    base.st状態[i].n相対X座標 = 0;
                                    base.st状態[i].n相対Y座標 = 18;
                                    base.st状態[i].n透明度 = 100;
                                }
                                else if( nNowFrame == 23 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].fZ軸回転度 = CConversion.DegreeToRadian( -16f );
                                    base.st状態[i].n相対X座標 = 0;
                                    base.st状態[i].n相対Y座標 = 24;
                                    base.st状態[i].n透明度 = 70;
                                }
                                #endregion
                            }
                            else if( base.st状態[ i ].judge == EJudgement.Auto )
                            {
                                #region[ Auto ]
                                if( nNowFrame == 0 )
                                {
                                    base.st状態[i].fX方向拡大率 = 0.625f;
                                    base.st状態[i].fY方向拡大率 = 3.70f;
                                    base.st状態[i].n相対X座標 = -19;
                                    base.st状態[i].n相対Y座標 = 0;
                                    base.st状態[i].n透明度 = 0;
                                }
                                else if( nNowFrame == 1 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.125f;
                                    base.st状態[i].fY方向拡大率 = 2.00f;
                                    base.st状態[i].n相対X座標 = 4;
                                    base.st状態[i].n相対Y座標 = 1;
                                    base.st状態[i].n透明度 = 0;
                                }
                                else if( nNowFrame == 2 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.375f;
                                    base.st状態[i].fY方向拡大率 = 0.66f;
                                    base.st状態[i].n相対X座標 = 13;
                                    base.st状態[i].n相対Y座標 = 1;
                                    base.st状態[i].n透明度 = 0;
                                }
                                else if( nNowFrame == 3 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.25f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].n相対X座標 = 8;
                                    base.st状態[i].n相対Y座標 = 1;
                                    base.st状態[i].n透明度 = 0;
                                }
                                else if( nNowFrame >= 4 && nNowFrame <= 18 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].n相対X座標 = 0;
                                    base.st状態[i].n相対Y座標 = 1;
                                    base.st状態[i].n透明度 = 0;
                                }
                                else if( nNowFrame == 19 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.25f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].n相対X座標 = 8;
                                    base.st状態[i].n相対Y座標 = 1;
                                    base.st状態[i].n透明度 = 0;
                                }
                                else if( nNowFrame == 20 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.375f;
                                    base.st状態[i].fY方向拡大率 = 0.66f;
                                    base.st状態[i].n相対X座標 = 13;
                                    base.st状態[i].n相対Y座標 = 1;
                                    base.st状態[i].n透明度 = 0;
                                }
                                else if( nNowFrame == 21 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.50f;
                                    base.st状態[i].fY方向拡大率 = 0.50f;
                                    base.st状態[i].n相対X座標 = 20;
                                    base.st状態[i].n相対Y座標 = 1;
                                    base.st状態[i].n透明度 = 0;
                                }
                                else if( nNowFrame == 22 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.91f;
                                    base.st状態[i].fY方向拡大率 = 0.23f;
                                    base.st状態[i].n相対X座標 = 37;
                                    base.st状態[i].n相対Y座標 = 1;
                                    base.st状態[i].n透明度 = 0;
                                }
                                else if( nNowFrame == 23 )
                                {
                                    base.st状態[i].fX方向拡大率 = 1.91f;
                                    base.st状態[i].fY方向拡大率 = 0.23f;
                                    base.st状態[i].n相対X座標 = 37;
                                    base.st状態[i].n相対Y座標 = 1;
                                    base.st状態[i].n透明度 = 0;
                                }
                                #endregion
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region[ むかしの ]
                    for (int i = 0; i < 12; i++)
                    {
                        if (!base.st状態[i].ct進行.bStopped)
                        {
                            base.st状態[i].ct進行.tUpdate();
                            if (base.st状態[i].ct進行.bReachedEndValue)
                            {
                                base.st状態[i].ct進行.tStop();
                            }
                            int num2 = base.st状態[i].ct進行.nCurrentValue;
                            if ((base.st状態[i].judge != EJudgement.Miss) && (base.st状態[i].judge != EJudgement.Bad))
                            {
                                if (num2 < 50)
                                {
                                    base.st状態[i].fX方向拡大率 = 1f + (1f * (1f - (((float)num2) / 50f)));
                                    base.st状態[i].fY方向拡大率 = ((float)num2) / 50f;
                                    base.st状態[i].n相対X座標 = 0;
                                    base.st状態[i].n相対Y座標 = 0;
                                    base.st状態[i].n透明度 = 0xff;
                                }
                                else if (num2 < 130)
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].n相対X座標 = 0;
                                    base.st状態[i].n相対Y座標 = ((num2 % 6) == 0) ? (CDTXMania.Random.Next(6) - 3) : base.st状態[i].n相対Y座標;
                                    base.st状態[i].n透明度 = 0xff;
                                }
                                else if (num2 >= 240)
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f - ((1f * (num2 - 240)) / 60f);
                                    base.st状態[i].n相対X座標 = 0;
                                    base.st状態[i].n相対Y座標 = 0;
                                    base.st状態[i].n透明度 = 0xff;
                                }
                                else
                                {
                                    base.st状態[i].fX方向拡大率 = 1f;
                                    base.st状態[i].fY方向拡大率 = 1f;
                                    base.st状態[i].n相対X座標 = 0;
                                    base.st状態[i].n相対Y座標 = 0;
                                    base.st状態[i].n透明度 = 0xff;
                                }
                            }
                            else if (num2 < 50)
                            {
                                base.st状態[i].fX方向拡大率 = 1f;
                                base.st状態[i].fY方向拡大率 = ((float)num2) / 50f;
                                base.st状態[i].n相対X座標 = 0;
                                base.st状態[i].n相対Y座標 = 0;
                                base.st状態[i].n透明度 = 0xff;
                            }
                            else if (num2 >= 200)
                            {
                                base.st状態[i].fX方向拡大率 = 1f - (((float)(num2 - 200)) / 100f);
                                base.st状態[i].fY方向拡大率 = 1f - (((float)(num2 - 200)) / 100f);
                                base.st状態[i].n相対X座標 = 0;
                                base.st状態[i].n相対Y座標 = 0;
                                base.st状態[i].n透明度 = 0xff;
                            }
                            else
                            {
                                base.st状態[i].fX方向拡大率 = 1f;
                                base.st状態[i].fY方向拡大率 = 1f;
                                base.st状態[i].n相対X座標 = 0;
                                base.st状態[i].n相対Y座標 = 0;
                                base.st状態[i].n透明度 = 0xff;
                            }
                        }
                    }
                    #endregion
                }
                #endregion

                for (int j = 0; j < 15; j++)
                {
                    if (!base.st状態[j].ct進行.bStopped)
                    {
                        #region[ 以前まで ]
                        if (CDTXMania.ConfigIni.nJudgeAnimeType < 2)
                        {
                            int num4 = CDTXMania.ConfigIni.nJudgeFrames > 1 ? 0 : base.st判定文字列[(int)base.st状態[j].judge].n画像番号;
                            int num5 = 0;
                            int num6 = 0;
							if( j == 14 )
							{
								if( ( (EType) CDTXMania.ConfigIni.JudgementStringPosition.Bass ) == EType.D )
								{
									// goto Label_06B7;
									continue;
								}
								num5 = ( ( (EType) CDTXMania.ConfigIni.JudgementStringPosition.Bass ) == EType.B ) ? 770 : 1060;
                                if ( ( (EType) CDTXMania.ConfigIni.JudgementStringPosition.Bass ) == EType.C )
                                    num6 = CDTXMania.ConfigIni.bReverse.Bass ? 650 : 80;
                                else
                                    num6 = CDTXMania.ConfigIni.bReverse.Bass ? 450 : 300;
							}
							else if( j == 13 )
							{
								if( ( (EType) CDTXMania.ConfigIni.JudgementStringPosition.Guitar ) == EType.D )
								{
									// goto Label_06B7;
									continue;
								}
								num5 = ( ( (EType) CDTXMania.ConfigIni.JudgementStringPosition.Guitar ) == EType.B ) ? 420 : 180;
                                if ( ( (EType) CDTXMania.ConfigIni.JudgementStringPosition.Guitar ) == EType.C )
                                    num6 = CDTXMania.ConfigIni.bReverse.Guitar ? 650 : 80;
                                else
                                    num6 = CDTXMania.ConfigIni.bReverse.Guitar ? 450 : 300;
							}

                            int nRectX = CDTXMania.ConfigIni.nJudgeWidgh;
                            int nRectY = CDTXMania.ConfigIni.nJudgeHeight;

                            int xc = (num5 + base.st状態[j].n相対X座標) + (this.stレーンサイズ[j].w / 2);
                            int x = (xc - ((int)((110f * base.st状態[j].fX方向拡大率)))) - ((nRectX - 225) / 2);
                            int y = ((num6 + base.st状態[j].n相対Y座標) - ((int)(((140f * base.st状態[j].fY方向拡大率)) / 2.0))) - ((nRectY - 135) / 2);

                            //if (base.tx判定文字列[num4] != null)
                            {
                                if (CDTXMania.ConfigIni.nJudgeFrames > 1 && CDTXMania.stagePerfGuitarScreen.tx判定画像anime != null)
                                {
                                    if (base.st状態[j].judge == EJudgement.Perfect)
                                    {
                                        //base.tx判定文字列[num4].tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(0, nRectY * base.st状態[j].nRect, nRectX, nRectY));
                                        CDTXMania.stagePerfGuitarScreen.tx判定画像anime.tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(0, nRectY * base.st状態[j].nRect, nRectX, nRectY));
                                    }
                                    if (base.st状態[j].judge == EJudgement.Great)
                                    {
                                        //base.tx判定文字列[num4].tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(nRectX * 1, nRectY * base.st状態[j].nRect, nRectX, nRectY));
                                        CDTXMania.stagePerfGuitarScreen.tx判定画像anime.tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(nRectX * 1, nRectY * base.st状態[j].nRect, nRectX, nRectY));
                                    }
                                    if (base.st状態[j].judge == EJudgement.Good)
                                    {
                                        //base.tx判定文字列[num4].tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(nRectX * 2, nRectY * base.st状態[j].nRect, nRectX, nRectY));
                                        CDTXMania.stagePerfGuitarScreen.tx判定画像anime.tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(nRectX * 2, nRectY * base.st状態[j].nRect, nRectX, nRectY));
                                    }
                                    if (base.st状態[j].judge == EJudgement.Poor)
                                    {
                                        //base.tx判定文字列[num4].tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(nRectX * 3, nRectY * base.st状態[j].nRect, nRectX, nRectY));
                                        CDTXMania.stagePerfGuitarScreen.tx判定画像anime.tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(nRectX * 3, nRectY * base.st状態[j].nRect, nRectX, nRectY));
                                    }
                                    if (base.st状態[j].judge == EJudgement.Miss)
                                    {
                                        //base.tx判定文字列[num4].tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(nRectX * 4, nRectY * base.st状態[j].nRect, nRectX, nRectY));
                                        CDTXMania.stagePerfGuitarScreen.tx判定画像anime.tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(nRectX * 4, nRectY * base.st状態[j].nRect, nRectX, nRectY));
                                    }
                                    if (base.st状態[j].judge == EJudgement.Auto)
                                    {
                                        //base.tx判定文字列[num4].tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(nRectX * 5, nRectY * base.st状態[j].nRect, nRectX, nRectY));
                                        CDTXMania.stagePerfGuitarScreen.tx判定画像anime.tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(nRectX * 5, nRectY * base.st状態[j].nRect, nRectX, nRectY));
                                    }
                                }
                                else if (base.tx判定文字列[num4] != null)
                                {
                                    x = xc - ((int)((64f * base.st状態[j].fX方向拡大率)));
                                    y = (num6 + base.st状態[j].n相対Y座標) - ((int)(((43f * base.st状態[j].fY方向拡大率)) / 2.0));

                                    base.tx判定文字列[num4].nTransparency = base.st状態[j].n透明度;
                                    base.tx判定文字列[num4].vcScaleRatio = new Vector3(base.st状態[j].fX方向拡大率, base.st状態[j].fY方向拡大率, 1f);
                                    base.tx判定文字列[num4].tDraw2D(CDTXMania.app.Device, x, y, base.st判定文字列[(int)base.st状態[j].judge].rc);
                                }


                                if (base.nShowLagType == (int)EShowLagType.ON ||
                                     ((base.nShowLagType == (int)EShowLagType.GREAT_POOR) && (base.st状態[j].judge != EJudgement.Perfect)))
                                {
                                    if (base.st状態[j].judge != EJudgement.Auto && base.txlag数値 != null)		// #25370 2011.2.1 yyagi
                                    {
                                        bool minus = false;
                                        int offsetX = 0;
                                        string strDispLag = base.st状態[j].nLag.ToString();
                                        if (st状態[j].nLag < 0)
                                        {
                                            minus = true;
                                        }
                                        x = xc - strDispLag.Length * 15 / 2;
                                        for (int i = 0; i < strDispLag.Length; i++)
                                        {
                                            int p = (strDispLag[i] == '-') ? 11 : (int)(strDispLag[i] - '0');	//int.Parse(strDispLag[i]);
                                            p += minus ? 0 : 12;		// change color if it is minus value
                                            base.txlag数値.tDraw2D(CDTXMania.app.Device, x + offsetX, y + 34, base.stLag数値[p].rc);
                                            offsetX += 15;
                                        }
                                    }
                                }

                            }
                        }
                        #endregion
                        #region[ さいしんばん ]
                        else if (CDTXMania.ConfigIni.nJudgeAnimeType == 2)
                        {
                            int num4 = 0;
                            int num5 = 0;
                            int num6 = 0;
							if( j == 14 )
							{
								if( ( (EType) CDTXMania.ConfigIni.JudgementStringPosition.Bass ) == EType.D )
								{
									// goto Label_06B7;
									continue;
								}
								num5 = ( ( (EType) CDTXMania.ConfigIni.JudgementStringPosition.Bass ) == EType.B ) ? 770 : 1020;
                                if ( ( (EType) CDTXMania.ConfigIni.JudgementStringPosition.Bass ) == EType.C )
                                    num6 = CDTXMania.ConfigIni.bReverse.Bass ? 650 : 80;
                                else
                                    num6 = CDTXMania.ConfigIni.bReverse.Bass ? 450 : 300;
							}
							else if( j == 13 )
							{
								if( ( (EType) CDTXMania.ConfigIni.JudgementStringPosition.Guitar ) == EType.D )
								{
									// goto Label_06B7;
									continue;
								}
								num5 = ( ( (EType) CDTXMania.ConfigIni.JudgementStringPosition.Guitar ) == EType.B ) ? 420 : 160;
                                if ( ( (EType) CDTXMania.ConfigIni.JudgementStringPosition.Guitar ) == EType.C )
                                    num6 = CDTXMania.ConfigIni.bReverse.Guitar ? 650 : 80;
                                else
                                    num6 = CDTXMania.ConfigIni.bReverse.Guitar ? 450 : 300;
							}

                            int nRectX = 85;
                            int nRectY = 35;

                            int xc = (num5 + base.st状態[j].n相対X座標) + (this.stレーンサイズ[j].w / 2);
                            int yc = (num6 + base.st状態[j].n相対Y座標) + (num6 / 2);
                            float fRot = base.st状態[j].fZ軸回転度;
                            int x = (xc - ((int)(((nRectX * base.st状態[j].fX方向拡大率) / base.st状態[j].fX方向拡大率) * base.st状態[j].fX方向拡大率)) + (nRectX / 2));
                            int y = (num6 + base.st状態[j].n相対Y座標) - ((int)((((nRectY) / 2) * base.st状態[j].fY方向拡大率)));


                            int xc_棒 = (num5 + base.st状態[j].n相対X座標_棒) + (this.stレーンサイズ[j].w / 2);
                            int yc_棒 = (num6 + base.st状態[j].n相対Y座標_棒) + (num6 / 2);
                            float fRot_棒 = base.st状態[j].fZ軸回転度_棒;
                            int x_棒 = (xc_棒 - ((int)(((nRectX * base.st状態[j].fX方向拡大率_棒) / base.st状態[j].fX方向拡大率_棒) * base.st状態[j].fX方向拡大率_棒)) + (nRectX / 2));
                            int y_棒 = (num6 + base.st状態[j].n相対Y座標_棒) - ((int)((((nRectY) / 2) * base.st状態[j].fY方向拡大率_棒)));

                            if (CDTXMania.stagePerfGuitarScreen.tx判定画像anime != null)
                            {
                                if (base.st状態[j].judge == EJudgement.Perfect)
                                {

                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime_2.vcScaleRatio = new Vector3(base.st状態[j].fX方向拡大率_棒, base.st状態[j].fY方向拡大率_棒, 1f);
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime_2.fZAxisRotation = base.st状態[j].fZ軸回転度_棒;
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime_2.tDraw2D(CDTXMania.app.Device, x_棒, y_棒, new Rectangle(0, 110, 210, 20));

                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.vcScaleRatio = new Vector3(base.st状態[j].fX方向拡大率, base.st状態[j].fY方向拡大率, 1f);
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.fZAxisRotation = base.st状態[j].fZ軸回転度;
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.nTransparency = 255;
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(0, 0, nRectX, nRectY));

                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime_3.vcScaleRatio = new Vector3(base.st状態[j].fX方向拡大率B, base.st状態[j].fY方向拡大率B, 1f);
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime_3.fZAxisRotation = base.st状態[j].fZ軸回転度;
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime_3.nTransparency = base.st状態[j].n透明度B;
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime_3.bAdditiveBlending = true;
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime_3.tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(0, 0, nRectX, nRectY));


                                }
                                if (base.st状態[j].judge == EJudgement.Great)
                                {
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime_2.vcScaleRatio = new Vector3(base.st状態[j].fX方向拡大率_棒, base.st状態[j].fY方向拡大率_棒, 1f);
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime_2.fZAxisRotation = base.st状態[j].fZ軸回転度_棒;
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime_2.tDraw2D(CDTXMania.app.Device, x_棒, y_棒, new Rectangle(0, 130, 210, 20));

                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.vcScaleRatio = new Vector3(base.st状態[j].fX方向拡大率, base.st状態[j].fY方向拡大率, 1f);
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.fZAxisRotation = base.st状態[j].fZ軸回転度;
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.nTransparency = 255;
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(nRectX + 5, 0, nRectX, nRectY));
                                }
                                if (base.st状態[j].judge == EJudgement.Good)
                                {
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.vcScaleRatio = new Vector3(base.st状態[j].fX方向拡大率, base.st状態[j].fY方向拡大率, 1f);
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.fZAxisRotation = 0;
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.nTransparency = 255;
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(0, nRectY + 2, nRectX, nRectY));
                                }
                                if (base.st状態[j].judge == EJudgement.Poor)
                                {
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.vcScaleRatio = new Vector3(1f, 1f, 1f);
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.fZAxisRotation = base.st状態[j].fZ軸回転度;
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.nTransparency = base.st状態[j].n透明度;
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(nRectX + 5, nRectY + 2, nRectX, nRectY));
                                }
                                if (base.st状態[j].judge == EJudgement.Miss)
                                {
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.vcScaleRatio = new Vector3(1f, 1f, 1f);
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.fZAxisRotation = base.st状態[j].fZ軸回転度;
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.nTransparency = base.st状態[j].n透明度;
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(0, nRectY * 2 + 4, nRectX, nRectY));
                                }
                                if (base.st状態[j].judge == EJudgement.Auto)
                                {
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.vcScaleRatio = new Vector3(base.st状態[j].fX方向拡大率, base.st状態[j].fY方向拡大率, 1f);
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.fZAxisRotation = base.st状態[j].fZ軸回転度;
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.nTransparency = 255;
                                    CDTXMania.stagePerfGuitarScreen.tx判定画像anime.tDraw2D(CDTXMania.app.Device, x + 5, y, new Rectangle(nRectX * 2 + 3, nRectY * 2 + 4, nRectX, nRectY));
                                }


                                if (base.nShowLagType == (int)EShowLagType.ON ||
                                     ((base.nShowLagType == (int)EShowLagType.GREAT_POOR) && (base.st状態[j].judge != EJudgement.Perfect)))
                                {
                                    if (base.st状態[j].judge != EJudgement.Auto && base.txlag数値 != null)		// #25370 2011.2.1 yyagi
                                    {
                                        bool minus = false;
                                        int offsetX = 0;
                                        string strDispLag = base.st状態[j].nLag.ToString();
                                        if (st状態[j].nLag < 0)
                                        {
                                            minus = true;
                                        }
                                        //x = xc - strDispLag.Length * 15 / 2;
                                        x = ( ( num5 ) + (this.stレーンサイズ[j].w / 2) ) - strDispLag.Length * 15 / 2;
                                        for (int i = 0; i < strDispLag.Length; i++)
                                        {
                                            int p = (strDispLag[i] == '-') ? 11 : (int)(strDispLag[i] - '0');	//int.Parse(strDispLag[i]);
                                            p += minus ? 0 : 12;		// change color if it is minus value
                                            base.txlag数値.tDraw2D(CDTXMania.app.Device, x + offsetX, y + 34, base.stLag数値[p].rc);
                                            offsetX += 15;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
			}
			return 0;
		}


		// Other

		#region [ private ]
		//-----------------
		//[StructLayout( LayoutKind.Sequential )]
		//private struct STレーンサイズ
		//{
		//	public int x;
		//	public int w;
		//}

		//private STレーンサイズ[] stレーンサイズ;
		//-----------------
		#endregion
	}
}
