using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal class CActPerfDrumsLaneFlushGB : CActPerfCommonLaneFlushGB
	{
		// CActivity 実装（共通クラスからの差分のみ）

		public override int OnUpdateAndDraw()
		{
			if( !base.bNotActivated )
			{
				for( int i = 0; i < 6; i++ )
				{
					if( !base.ctUpdate[ i ].bStopped )
					{
						EInstrumentPart e楽器パート = ( i < 3 ) ? EInstrumentPart.GUITAR : EInstrumentPart.BASS;
						CTexture texture = CDTXMania.ConfigIni.bReverse[ (int) e楽器パート ] ? base.txFlush[ ( i % 3 ) + 3 ] : base.txFlush[ i % 3 ];
						int num2 = CDTXMania.ConfigIni.bLeft[ (int) e楽器パート ] ? 1 : 0;
						for( int j = 0; j < 3; j++ )
						{
							int x = ( ( ( i < 3 ) ? 0x1fb : 0x18e ) + this.nRGBのX座標[ num2, i ] ) + ( ( 0x10 * base.ctUpdate[ i ].nCurrentValue ) / 100 );
							int y = ( ( i < 3 ) ? 0x39 : 0x39 ) + ( j * 0x76 );
							if( texture != null )
							{
								texture.tDraw2D( CDTXMania.app.Device, x, y, new Rectangle( j * 0x20, 0, ( 0x18 * ( 100 - base.ctUpdate[ i ].nCurrentValue ) ) / 100, 0x76 ) );
							}
						}
						base.ctUpdate[ i ].tUpdate();
						if( base.ctUpdate[ i ].bReachedEndValue )
						{
							base.ctUpdate[ i ].tStop();
						}
					}
				}
			}
			return 0;
		}


		// Other

		#region [ private ]
		//-----------------
		private readonly int[,] nRGBのX座標 = new int[ , ] { { 2, 0x1c, 0x36, 2, 0x1c, 0x36 }, { 0x36, 0x1c, 2, 0x36, 0x1c, 2 } };
		//-----------------
		#endregion
	}
}
