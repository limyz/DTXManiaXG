using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal class CActPerfGuitarWailingBonus : CActPerfCommonWailingBonus
	{
		// メソッド
        private int posXGuitar = CDTXMania.ConfigIni.nWailingFireX.Guitar;
        private int posXBass = CDTXMania.ConfigIni.nWailingFireX.Bass;
        private int posY = CDTXMania.ConfigIni.nWailingFireY;
        private int rectW = CDTXMania.ConfigIni.nWailingFireWidgh;
        private int rectH = CDTXMania.ConfigIni.nWailingFireHeight;
        private int frames = CDTXMania.ConfigIni.nWailingFireFrames;
        private int interval = CDTXMania.ConfigIni.nWailingFireInterval;


		public CActPerfGuitarWailingBonus()
		{
			base.bNotActivated = true;
		}
		//public override void Start( EInstrumentPart part )
		//{
		//    this.Start( part, null );
		//}
		public override void Start( EInstrumentPart part, CChip r歓声Chip )
		{
			if( part != EInstrumentPart.DRUMS )
			{
				for( int i = 0; i < 4; i++ )
				{
					if( ( this.ct進行用[ (int) part, i ] == null ) || this.ct進行用[ (int) part, i ].bStopped )
					{
						this.ct進行用[ (int) part, i ] = new CCounter( 0, 300, 2, CDTXMania.Timer );
                        this.ctWailing炎[ (int) part, i ] = new CCounter( 0, this.frames, this.interval, CDTXMania.Timer );
						if( CDTXMania.ConfigIni.b歓声を発声する )
						{
							if( r歓声Chip != null )
							{
								CDTXMania.DTX.tPlayChip( r歓声Chip, CSoundManager.rcPerformanceTimer.nシステム時刻, (int) ELane.BGM, CDTXMania.DTX.nモニタを考慮した音量( EInstrumentPart.UNKNOWN ) );
								return;
							}
							CDTXMania.Skin.soundAudience.n位置_次に鳴るサウンド = ( part == EInstrumentPart.GUITAR ) ? -50 : 50;
							CDTXMania.Skin.soundAudience.tPlay();
							return;
						}
						break;
					}
				}
			}
		}


		// CActivity 実装

		public override void OnActivate()
		{
			for( int i = 0; i < 3; i++ )
			{
				for( int j = 0; j < 4; j++ )
				{
					this.ct進行用[ i, j ] = null;
                    this.ctWailing炎[ i, j  ] = null;
				}
			}
			base.OnActivate();
		}
		public override int OnUpdateAndDraw()
		{
			if( !base.bNotActivated )
			{
				for( int i = 0; i < 2; i++ )
				{
					EInstrumentPart e楽器パート = ( i == 0 ) ? EInstrumentPart.GUITAR : EInstrumentPart.BASS;
					for( int k = 0; k < 4; k++ )
					{
						if( ( this.ct進行用[ (int) e楽器パート, k ] != null ) && !this.ct進行用[ (int) e楽器パート, k ].bStopped )
						{
							if( this.ct進行用[ (int) e楽器パート, k ].bReachedEndValue )
							{
								this.ct進行用[ (int) e楽器パート, k ].tStop();
							}
							else
							{
								this.ct進行用[ (int) e楽器パート, k ].tUpdate();
							}
						}

                        if( ( this.ctWailing炎[ (int) e楽器パート, k ] != null ) && ( !this.ctWailing炎[ (int) e楽器パート, k ].bStopped ) )
                        {
                            if( this.ctWailing炎[ (int) e楽器パート, k ].bReachedEndValue )
                            {
                                this.ctWailing炎[ (int) e楽器パート, k ].tStop();
                            }
                            else
                            {
                                this.ctWailing炎[ (int) e楽器パート, k ].tUpdate();
                            }
                        }

					}
				}
				for( int j = 0; j < 2; j++ )
				{
					EInstrumentPart e楽器パート2 = ( j == 0 ) ? EInstrumentPart.GUITAR : EInstrumentPart.BASS;
					for( int m = 0; m < 4; m++ )
					{
						if( ( this.ct進行用[ (int) e楽器パート2, m ] != null ) && !this.ct進行用[ (int) e楽器パート2, m ].bStopped )
						{
                            //XGではWailingレーンの幅が42px
							int x = ( ( e楽器パート2 == EInstrumentPart.GUITAR ) ? 160 : 1030 ) + 133;
							int num6 = 0;
							int num7 = 0;
							int num8 = this.ct進行用[ (int) e楽器パート2, m ].nCurrentValue;
							if( num8 < 100 )
							{
								num6 = (int) ( 120.0 + ( 290.0 * Math.Cos( Math.PI / 2 * ( ( (double) num8 ) / 100.0 ) ) ) );
							}
							else if( num8 < 150 )
							{
								num6 = (int) ( 120.0 + ( ( 150 - num8 ) * Math.Sin( ( Math.PI * ( ( num8 - 100 ) % 25 ) ) / 25.0 ) ) );
							}
							else if( num8 < 200 )
							{
								num6 = 64;
							}
							else
							{
								num6 = (int) ( 64.0 - ( ( (double) ( 290 * ( num8 - 200 ) ) ) / 100.0 ) );
							}
							if( CDTXMania.ConfigIni.bReverse[ (int) e楽器パート2 ] )
							{
								num6 = ( 670 - num6 ) - 244;
							}
							Rectangle rectangle = new Rectangle( 0, 0, 26, 122 );
							if( ( 720 - num6 ) < rectangle.Bottom )
							{
								rectangle.Height = ( 720 - num6 ) - rectangle.Top;
							}
							if( num6 < 0 )
							{
								rectangle.Y = -num6;
								rectangle.Height -= -num6;
								num7 = -num6;
							}
							if( ( rectangle.Top < rectangle.Bottom ) && ( this.txWailingBonus != null ) )
							{
								this.txWailingBonus.tDraw2D( CDTXMania.app.Device, x, num6 + num7, rectangle );
							}
							num7 = 0;
							rectangle = new Rectangle( 26, 0, 26, 122 );
							if( ( 720 - ( num6 + 122 ) ) < rectangle.Bottom )
							{
								rectangle.Height = ( 720 - ( num6 + 122 ) ) - rectangle.Top;
							}
							if( ( num6 + 0x7a ) < 0 )
							{
								rectangle.Y = -( num6 + 122 );
								rectangle.Height -= rectangle.Y;
								num7 = -( num6 + 122 );
							}
							if( ( rectangle.Top < rectangle.Bottom ) && ( this.txWailingBonus != null ) && CDTXMania.ConfigIni.nWailingFireFrames == 0 )
							{
								this.txWailingBonus.tDraw2D( CDTXMania.app.Device, x, ( num6 + num7 ) + 122, rectangle );
							}

                            if( this.txWailingFlush != null && CDTXMania.ConfigIni.nWailingFireFrames == 0 )
                            {
                                for( int i = 0; i <= 12; i++ )
                                {
                                    this.txWailingFlush.tDraw2D( CDTXMania.app.Device, ( e楽器パート2 == EInstrumentPart.GUITAR ) ? 283 : 1153, 64 * i, new Rectangle( 0, 0, 42, 64 ) );
                                }

                                int count = this.ct進行用[ (int)e楽器パート2, m ].nCurrentValue;
                                this.txWailingFlush.nTransparency = ( count <= 55 ? 255 : 255 - count );
                            }
						}

                        if( ( this.ctWailing炎[ (int) e楽器パート2, m ] != null ) && !this.ctWailing炎[ (int) e楽器パート2, m ].bStopped )
                        {
                            if( this.txWailingFire != null )
                            {
                                if( e楽器パート2 == EInstrumentPart.GUITAR )
                                {
                                    this.txWailingFire.t2D描画( CDTXMania.app.Device, this.posXGuitar, this.posY, new Rectangle( this.rectW * this.ctWailing炎[ (int) e楽器パート2, m ].nCurrentValue, 0, this.rectW, this.rectH ) );
                                }
                                if( e楽器パート2 == EInstrumentPart.BASS )
                                {
                                    this.txWailingFire.t2D描画( CDTXMania.app.Device, this.posXBass, this.posY, new Rectangle( this.rectW * this.ctWailing炎[ (int) e楽器パート2, m ].nCurrentValue, 0, this.rectW, this.rectH ) );
                                }
                            }
                        }
					}
				}
			}
			return 0;
		}
	}
}
