using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal class CActPerfGuitarLaneFlushGB : CActPerfCommonLaneFlushGB
	{
		// コンストラクタ

		public CActPerfGuitarLaneFlushGB()
		{
			base.bNotActivated = true;
		}
        // 2013.02.22 kairera0467
        // ギターのレーンフラッシュの幅は37。

		// CActivity 実装（共通クラスからの差分のみ）

        public override void OnManagedCreateResources()
        {
            this.txLane = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_Paret_Guitar.png"));
            this.txLaneDark = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_Paret_Guitar_Dark.png"));
            this.txLaneFlush = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_guitar line.png"));

            base.OnManagedCreateResources();
        }
        public override void OnManagedReleaseResources()
        {
            CDTXMania.tReleaseTexture( ref this.txLane );
            CDTXMania.tReleaseTexture( ref this.txLaneDark );
            CDTXMania.tReleaseTexture( ref this.txLaneFlush );
            base.OnManagedReleaseResources();
        }

		public override int OnUpdateAndDraw()
		{
			if( !base.bNotActivated )
            {
                #region[ レーンの描画 ]
                //---------------
                //レ－ンのみ先に描画しておく。
                if (CDTXMania.DTX.bチップがある.Guitar)
                {
                    if ( CDTXMania.ConfigIni.nLaneDisp.Guitar == 0 || CDTXMania.ConfigIni.nLaneDisp.Guitar == 2 )
                        this.txLane.tDraw2D(CDTXMania.app.Device, 67, 42);
                    else
                        this.txLaneDark.tDraw2D(CDTXMania.app.Device, 67, 42);
                }
                if (CDTXMania.DTX.bチップがある.Bass)
                {
                    if ( CDTXMania.ConfigIni.nLaneDisp.Bass == 0 || CDTXMania.ConfigIni.nLaneDisp.Bass == 2 )
                        this.txLane.tDraw2D(CDTXMania.app.Device, 937, 42);
                    else
                        this.txLaneDark.tDraw2D(CDTXMania.app.Device, 937, 42);
                }
                //---------------
                #endregion

                for ( int i = 0; i < 10; i++ )
				{
					if( !base.ctUpdate[ i ].bStopped )
					{
						EInstrumentPart e楽器パート = ( i < 5 ) ? EInstrumentPart.GUITAR : EInstrumentPart.BASS;
						CTexture texture = CDTXMania.ConfigIni.bReverse[ (int) e楽器パート ] ? base.txFlush[ ( i % 5 ) + 5 ] : base.txFlush[ i % 5 ];
						int num2 = CDTXMania.ConfigIni.bLeft[ (int) e楽器パート ] ? 1 : 0;
						//int x = ( ( ( i < 5 ) ? 88 : 480 ) + this.nRGBのX座標[ num2, i ] ) + ( ( 37 * base.ctUpdate[ i ].nCurrentValue ) / 100 );
                        int x = (((i < 5) ? 88 : 958) + this.nRGBのX座標[num2, i] + ( ( 19 * base.ctUpdate[ i ].nCurrentValue ) / 70 ));
                        int x2 = ((i < 5) ? 88 : 954);
		                int y = CDTXMania.ConfigIni.bReverse[ (int) e楽器パート ] ? 414 : 100;
                        int y2 = CDTXMania.ConfigIni.bReverse[(int)e楽器パート] ? 414 : 104;
						if( texture != null && CDTXMania.ConfigIni.bLaneFlush[ (int) e楽器パート ] )
						{
                            texture.tDraw2D( CDTXMania.app.Device, x, y, new Rectangle( 37, 0, ( 37 * ( 70 - base.ctUpdate[ i ].nCurrentValue)) / 70, 256 ) );
                            //if( j == 4 )
                                //this.txレーンフラッシュ.tDraw2D( CDTXMania.app.Device, x2 + ( ( i < 5 ? i : i - 5 ) * 39 ), y2, new Rectangle( i * 39, 0, 41, 566 ) );
				        }
                        base.ctUpdate[ i ].tUpdate();
						if( base.ctUpdate[ i ].bReachedEndValue )
						{
							base.ctUpdate[ i ].tStop();
						}
					}
				}
                //ここの分岐文はbase.ctUpdate[ n ]のものを使わないと、停止中にレーンフラッシュが消えてしまう。
                
                if ( !base.ctUpdate[ 0 ].bStopped && CDTXMania.ConfigIni.bLaneFlush.Guitar )
                {
                    this.txLaneFlush.tDraw2D(CDTXMania.app.Device, ( CDTXMania.ConfigIni.bLeft.Guitar ? 242 : 86 ), 104, new Rectangle(0, 0, 41, 566));
                }
                if ( !base.ctUpdate[ 1 ].bStopped && CDTXMania.ConfigIni.bLaneFlush.Guitar )
                {
                    this.txLaneFlush.tDraw2D(CDTXMania.app.Device, ( CDTXMania.ConfigIni.bLeft.Guitar ? 203 : 125 ), 104, new Rectangle(39, 0, 41, 566));
                }
                if ( !base.ctUpdate[ 2 ].bStopped && CDTXMania.ConfigIni.bLaneFlush.Guitar )
                {
                    this.txLaneFlush.tDraw2D(CDTXMania.app.Device, 164, 104, new Rectangle(78, 0, 41, 566));
                }
                if ( !base.ctUpdate[ 3 ].bStopped && CDTXMania.ConfigIni.bLaneFlush.Guitar )
                {
                    this.txLaneFlush.tDraw2D(CDTXMania.app.Device, ( CDTXMania.ConfigIni.bLeft.Guitar ? 125 : 203 ), 104, new Rectangle(117, 0, 41, 566));
                }
                if ( !base.ctUpdate[ 4 ].bStopped && CDTXMania.ConfigIni.bLaneFlush.Guitar )
                {
                    this.txLaneFlush.tDraw2D(CDTXMania.app.Device, ( CDTXMania.ConfigIni.bLeft.Guitar ? 86 : 242 ), 104, new Rectangle(156, 0, 41, 566));
                }
                if( !base.ctUpdate[ 5 ].bStopped && CDTXMania.ConfigIni.bLaneFlush.Bass )
                {
                    this.txLaneFlush.tDraw2D(CDTXMania.app.Device, ( CDTXMania.ConfigIni.bLeft.Bass ? 1112 : 957 ), 104, new Rectangle(0, 0, 41, 566));
                }
                if( !base.ctUpdate[ 6 ].bStopped && CDTXMania.ConfigIni.bLaneFlush.Bass )
                {
                    this.txLaneFlush.tDraw2D(CDTXMania.app.Device, ( CDTXMania.ConfigIni.bLeft.Bass ? 1073 : 995 ), 104, new Rectangle(39, 0, 41, 566));
                }
                if( !base.ctUpdate[ 7 ].bStopped && CDTXMania.ConfigIni.bLaneFlush.Bass )
                {
                    this.txLaneFlush.tDraw2D(CDTXMania.app.Device, 1034, 104, new Rectangle(78, 0, 41, 566));
                }
                if( !base.ctUpdate[ 8 ].bStopped && CDTXMania.ConfigIni.bLaneFlush.Bass )
                {
                    this.txLaneFlush.tDraw2D(CDTXMania.app.Device, ( CDTXMania.ConfigIni.bLeft.Bass ? 995 : 1073 ), 104, new Rectangle(117, 0, 41, 566));
                }
                if( !base.ctUpdate[ 9 ].bStopped && CDTXMania.ConfigIni.bLaneFlush.Bass )
                {
                    this.txLaneFlush.tDraw2D(CDTXMania.app.Device, ( CDTXMania.ConfigIni.bLeft.Bass ? 957 : 1112 ), 104, new Rectangle(156, 0, 41, 566));
                }

			}
			return 0;
		}
		

		// Other

		#region [ private ]
		//-----------------
		private readonly int[,] nRGBのX座標 = new int[ , ] { { 0, 39, 78, 117, 156, 0, 39, 78, 117, 156 }, { 156, 117, 78, 39, 0, 156, 117, 78, 39, 0 } };

        private CTexture txLane;
        private CTexture txLaneDark;
        private CTexture txLaneFlush;
		//-----------------
		#endregion
	}
}
