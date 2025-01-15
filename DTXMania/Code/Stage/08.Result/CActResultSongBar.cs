using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using SharpDX;
using FDK;

using Rectangle = System.Drawing.Rectangle;

namespace DTXMania
{
	internal class CActResultSongBar : CActivity
	{
		// コンストラクタ

		public CActResultSongBar()
		{
			base.bNotActivated = true;
		}


		// メソッド

		public void tアニメを完了させる()
		{
			this.ct登場用.nCurrentValue = this.ct登場用.nEndValue;
		}


		// CActivity 実装

		public override void OnActivate()
		{
			this.n本体X = 0;
			this.n本体Y = 0x18b;
			this.ft曲名用フォント = new Font( "MS PGothic", 44f, FontStyle.Regular, GraphicsUnit.Pixel );
			base.OnActivate();
		}
		public override void OnDeactivate()
		{
			if( this.ft曲名用フォント != null )
			{
				this.ft曲名用フォント.Dispose();
				this.ft曲名用フォント = null;
			}
			if( this.ct登場用 != null )
			{
				this.ct登場用 = null;
			}
			base.OnDeactivate();
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
				//this.txバー = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenResult song bar.png" ) );
				try
				{
					Bitmap image = new Bitmap( 0x3a8, 0x36 );
					Graphics graphics = Graphics.FromImage( image );
					graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
					graphics.DrawString( CDTXMania.DTX.TITLE, this.ft曲名用フォント, Brushes.White, ( float ) 8f, ( float ) 0f );
					this.tx曲名 = new CTexture( CDTXMania.app.Device, image, CDTXMania.TextureFormat );
					this.tx曲名.vcScaleRatio = new Vector3( 0.5f, 0.5f, 1f );
					graphics.Dispose();
					image.Dispose();
				}
				catch( CTextureCreateFailedException )
				{
					Trace.TraceError( "曲名テクスチャの生成に失敗しました。" );
					this.tx曲名 = null;
				}
				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if( !base.bNotActivated )
			{
				CDTXMania.tReleaseTexture( ref this.txバー );
				CDTXMania.tReleaseTexture( ref this.tx曲名 );
				base.OnManagedReleaseResources();
			}
		}
		public override int OnUpdateAndDraw()
		{
			if( base.bNotActivated )
			{
				return 0;
			}
			if( base.bJustStartedUpdate )
			{
				this.ct登場用 = new CCounter( 0, 270, 4, CDTXMania.Timer );
				base.bJustStartedUpdate = false;
			}
			this.ct登場用.tUpdate();
			int num = 0x1d4;
			int num2 = num - 0x40;
			if( this.ct登場用.bInProgress )
			{
				if( this.ct登場用.nCurrentValue <= 100 )
				{
					double num3 = 1.0 - ( ( (double) this.ct登場用.nCurrentValue ) / 100.0 );
					this.n本体X = -( (int) ( num * Math.Sin( Math.PI / 2 * num3 ) ) );
					this.n本体Y = 0x18b;
				}
				else if( this.ct登場用.nCurrentValue <= 200 )
				{
					double num4 = ( (double) ( this.ct登場用.nCurrentValue - 100 ) ) / 100.0;
					this.n本体X = -( (int) ( ( ( (double) num ) / 6.0 ) * Math.Sin( Math.PI * num4 ) ) );
					this.n本体Y = 0x18b;
				}
				else if( this.ct登場用.nCurrentValue <= 270 )
				{
					double num5 = ( (double) ( this.ct登場用.nCurrentValue - 200 ) ) / 70.0;
					this.n本体X = -( (int) ( ( ( (double) num ) / 18.0 ) * Math.Sin( Math.PI * num5 ) ) );
					this.n本体Y = 0x18b;
				}
			}
			else
			{
				this.n本体X = 0;
				this.n本体Y = 0x18b;
			}
			int num6 = this.n本体X;
			int y = this.n本体Y;
			int num8 = 0;
			while( num8 < num2 )
			{
				Rectangle rectangle = new Rectangle( 0, 0, 0x40, 0x40 );
				if( ( num8 + rectangle.Width ) >= num2 )
				{
					rectangle.Width -= ( num8 + rectangle.Width ) - num2;
				}
				num8 += rectangle.Width;
			}
			if( this.tx曲名 != null )
			{
				this.tx曲名.tDraw2D( CDTXMania.app.Device, this.n本体X, this.n本体Y + 20 );
			}
			if( !this.ct登場用.bReachedEndValue )
			{
				return 0;
			}
			return 1;
		}


		// Other

		#region [ private ]
		//-----------------
		private CCounter ct登場用;
		private Font ft曲名用フォント;
		private int n本体X;
		private int n本体Y;
		private CTexture txバー;
		private CTexture tx曲名;
		//-----------------
		#endregion
	}
}
