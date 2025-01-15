using System.Drawing;
using FDK;

namespace DTXMania
{
	internal class CCharacterConsole : CActivity
	{
		// 定数

		public enum EFontType  // Eフォント種別
		{
			White,     // 白
			Red,       // 赤
			Ash,	   // 灰
			WhiteThin, // 白細
			RedThin,   // 赤細
			AshThin    // 灰細
		}
		public enum E配置
		{
			左詰,
			中央,
			右詰
		}


		// メソッド

		public void tPrint( int x, int y, EFontType font, string str英数字文字列 )
		{
			if( !base.bNotActivated && !string.IsNullOrEmpty( str英数字文字列 ) )
			{
				int BOL = x;
				for( int i = 0; i < str英数字文字列.Length; i++ )
				{
					char ch = str英数字文字列[ i ];
					if( ch == '\n' )
					{
						x = BOL;
						y += nFontHeight;
					}
					else
					{
						int index = str表記可能文字.IndexOf( ch );
						if( index < 0 )
						{
							x += nFontWidth;
						}
						else
						{
							if( this.txフォント8x16[ (int) ( (int) font / (int) EFontType.WhiteThin ) ] != null )
							{
								this.txフォント8x16[ (int) ( (int) font / (int) EFontType.WhiteThin ) ].tDraw2D( CDTXMania.app.Device, x, y, this.rc文字の矩形領域[ (int) ( (int) font % (int) EFontType.WhiteThin ), index ] );
							}
							x += nFontWidth;
						}
					}
				}
			}
		}


		// CActivity 実装

		public override void OnActivate()
		{
			this.rc文字の矩形領域 = new Rectangle[3, str表記可能文字.Length ];
			for( int i = 0; i < 3; i++ )
			{
				for (int j = 0; j < str表記可能文字.Length; j++)
				{
					const int regionX = 128, regionY = 16;
					this.rc文字の矩形領域[ i, j ].X = ( ( i / 2 ) * regionX ) + ( ( j % regionY ) * nFontWidth );
					this.rc文字の矩形領域[ i, j ].Y = ( ( i % 2 ) * regionX ) + ( ( j / regionY ) * nFontHeight );
					this.rc文字の矩形領域[ i, j ].Width = nFontWidth;
					this.rc文字の矩形領域[ i, j ].Height = nFontHeight;
				}
			}
			base.OnActivate();
		}
		public override void OnDeactivate()
		{
			if( this.rc文字の矩形領域 != null )
				this.rc文字の矩形領域 = null;

			base.OnDeactivate();
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
				this.txフォント8x16[ 0 ] = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\Console font 8x16.png" ) );
				this.txフォント8x16[ 1 ] = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\Console font 2 8x16.png" ) );
				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if( !base.bNotActivated )
			{
				for( int i = 0; i < 2; i++ )
				{
					if( this.txフォント8x16[ i ] != null )
					{
						this.txフォント8x16[ i ].Dispose();
						this.txフォント8x16[ i ] = null;
					}
				}
				base.OnManagedReleaseResources();
			}
		}


		// Other

		#region [ private ]
		//-----------------
		private Rectangle[,] rc文字の矩形領域;
		private const string str表記可能文字 = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~ ";
		private const int nFontWidth = 8, nFontHeight = 16;
		private CTexture[] txフォント8x16 = new CTexture[ 2 ];
		//-----------------
		#endregion
	}
}
