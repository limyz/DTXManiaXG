using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal class CActSelectInformation : CActivity
	{
		// コンストラクタ

		public CActSelectInformation()
		{
			base.bNotActivated = true;
		}


		// CActivity 実装

		public override void OnActivate()
		{
			this.n画像Index上 = -1;
			this.n画像Index下 = 0;
			base.OnActivate();
		}
		public override void OnDeactivate()
		{
			this.ctスクロール用 = null;
			base.OnDeactivate();
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
				string[] infofiles = {		// #25381 2011.6.4 yyagi
				   @"Graphics\5_information.png" ,
				   @"Graphics\5_informatione.png"
				};
				int c = CDTXMania.isJapanese ? 0 : 1; 
				this.txInfo = CDTXMania.tGenerateTexture( CSkin.Path( infofiles[ c ] ), false );
				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if( !base.bNotActivated )
			{
				CDTXMania.tReleaseTexture( ref this.txInfo );
				base.OnManagedReleaseResources();
			}
		}
		public override int OnUpdateAndDraw()
		{
			if( !base.bNotActivated )
			{
				if( base.bJustStartedUpdate )
				{
					this.ctスクロール用 = new CCounter( 0, 6000, 1, CDTXMania.Timer );
					base.bJustStartedUpdate = false;
				}
				this.ctスクロール用.tUpdate();
				if( this.ctスクロール用.bReachedEndValue )
				{
					this.n画像Index上 = this.n画像Index下;
					this.n画像Index下 = ( this.n画像Index下 + 1 ) % stInfo.GetLength( 0 );		//8;
					this.ctスクロール用.nCurrentValue = 0;
				}
				int n現在の値 = this.ctスクロール用.nCurrentValue;
				if( n現在の値 <= 250 )
				{
					double n現在の割合 = ( (double) n現在の値 ) / 250.0;
					if( this.n画像Index上 >= 0 )
					{
						STINFO stinfo = this.stInfo[ this.n画像Index上 ];
						Rectangle rectangle = new Rectangle( stinfo.pt左上座標.X, stinfo.pt左上座標.Y + ( (int) ( 42.0 * n現在の割合 ) ), 240, Convert.ToInt32(42.0 * (1.0 - n現在の割合)) );
						if( this.txInfo != null )
						{
							this.txInfo.tDraw2D( CDTXMania.app.Device, 4, 0, rectangle );
						}
					}
					if( this.n画像Index下 >= 0 )
					{
						STINFO stinfo = this.stInfo[ this.n画像Index下 ];
						Rectangle rectangle = new Rectangle( stinfo.pt左上座標.X, stinfo.pt左上座標.Y, 240, (int) ( 42.0 * n現在の割合 ) );
						if( this.txInfo != null )
						{
							this.txInfo.tDraw2D( CDTXMania.app.Device, 4, 0 + ( (int) ( 42.0 * ( 1.0 - n現在の割合 ) ) ), rectangle );
						}
					}
				}
				else
				{
					STINFO stinfo = this.stInfo[ this.n画像Index下 ];
					Rectangle rectangle = new Rectangle( stinfo.pt左上座標.X, stinfo.pt左上座標.Y, 240, 42 );
					if( this.txInfo != null )
					{
						this.txInfo.tDraw2D( CDTXMania.app.Device, 4, 0, rectangle );
					}
				}
			}
			return 0;
		}


		// Other

		#region [ private ]
		//-----------------
		[StructLayout( LayoutKind.Sequential )]
		private struct STINFO
		{
			public Point pt左上座標;
			public STINFO( int x, int y )
			{
				this.pt左上座標 = new Point( x, y );
			}
		}

		private CCounter ctスクロール用;
		private int n画像Index下;
		private int n画像Index上;
        private readonly STINFO[] stInfo = new STINFO[] {
            new STINFO(0, 0 * 42),
            new STINFO(0, 1 * 42),
            new STINFO(0, 2 * 42),
            new STINFO(0, 3 * 42),
            new STINFO(0, 4 * 42),
            new STINFO(0, 5 * 42),
            new STINFO(0, 6 * 42),
            new STINFO(0, 7 * 42)
        };
        private CTexture txInfo;
		//-----------------
		#endregion
	}
}
