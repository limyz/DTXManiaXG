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
	internal class CActSelectArtistComment : CActivity
	{
		// メソッド

		public CActSelectArtistComment()
		{
			base.bNotActivated = true;
		}
		public void t選択曲が変更された()
		{
			CScore cスコア = CDTXMania.stageSongSelection.rSelectedScore;
			if( cスコア != null )
			{
				Bitmap image = new Bitmap( 1, 1 );
				CDTXMania.tReleaseTexture( ref this.txArtist );
				this.strArtist = cスコア.SongInformation.ArtistName;
				if( ( this.strArtist != null ) && ( this.strArtist.Length > 0 ) )
				{
					Graphics graphics = Graphics.FromImage( image );
					graphics.PageUnit = GraphicsUnit.Pixel;
					SizeF ef = graphics.MeasureString( this.strArtist, this.ft描画用フォント );
					graphics.Dispose();
					//if (ef.Width > SampleFramework.GameWindowSize.Width)
					//{
					//	ef.Width = SampleFramework.GameWindowSize.Width;
					//}
					try
					{
						//Fix length issue of Artist using the same method used for Song Title
						int nLargestLengthPx = 510;//510px is the available space for artist in the bar
						int widthAfterScaling = (int)((ef.Width + 2) * 0.5f);//+2 buffer
						if (widthAfterScaling > (CDTXMania.app.Device.Capabilities.MaxTextureWidth / 2))
							widthAfterScaling = CDTXMania.app.Device.Capabilities.MaxTextureWidth / 2;  // 右端断ち切れ仕方ないよね
						//Compute horizontal scaling factor
						float f拡大率X = (widthAfterScaling <= nLargestLengthPx) ? 0.5f : (((float)nLargestLengthPx / (float)widthAfterScaling) * 0.5f);   // 長い文字列は横方向に圧縮。
																																						//ef.Width
						Bitmap bitmap2 = new Bitmap( (int) Math.Ceiling( (double)widthAfterScaling * 2), (int) Math.Ceiling( (double) this.ft描画用フォント.Size ) );
						graphics = Graphics.FromImage( bitmap2 );
						graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
						graphics.DrawString( this.strArtist, this.ft描画用フォント, Brushes.White, ( float ) 0f, ( float ) 0f );
						graphics.Dispose();
						this.txArtist = new CTexture( CDTXMania.app.Device, bitmap2, CDTXMania.TextureFormat );
						this.txArtist.vcScaleRatio = new Vector3(f拡大率X, 0.5f, 1f );
						bitmap2.Dispose();
					}
					catch( CTextureCreateFailedException )
					{
						Trace.TraceError( "ARTISTテクスチャの生成に失敗しました。" );
						this.txArtist = null;
					}
				}
				CDTXMania.tReleaseTexture( ref this.txComment );
				this.strComment = cスコア.SongInformation.Comment;
				if( ( this.strComment != null ) && ( this.strComment.Length > 0 ) )
				{
					Graphics graphics2 = Graphics.FromImage( image );
					graphics2.PageUnit = GraphicsUnit.Pixel;
					SizeF ef2 = graphics2.MeasureString( this.strComment, this.ft描画用フォント );
					Size size = new Size( (int) Math.Ceiling( (double) ef2.Width ), (int) Math.Ceiling( (double) ef2.Height ) );
					graphics2.Dispose();
					this.nテクスチャの最大幅 = CDTXMania.app.Device.Capabilities.MaxTextureWidth;
					int maxTextureHeight = CDTXMania.app.Device.Capabilities.MaxTextureHeight;
					Bitmap bitmap3 = new Bitmap( size.Width, (int) Math.Ceiling( (double) this.ft描画用フォント.Size ) );
					graphics2 = Graphics.FromImage( bitmap3 );
					graphics2.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
					graphics2.DrawString( this.strComment, this.ft描画用フォント, Brushes.White, ( float ) 0f, ( float ) 0f );
					graphics2.Dispose();
					this.nComment行数 = 1;
					this.nComment最終行の幅 = size.Width;
					while( this.nComment最終行の幅 > this.nテクスチャの最大幅 )
					{
						this.nComment行数++;
						this.nComment最終行の幅 -= this.nテクスチャの最大幅;
					}
					while( ( this.nComment行数 * ( (int) Math.Ceiling( (double) this.ft描画用フォント.Size ) ) ) > maxTextureHeight )
					{
						this.nComment行数--;
						this.nComment最終行の幅 = this.nテクスチャの最大幅;
					}
					Bitmap bitmap4 = new Bitmap( ( this.nComment行数 > 1 ) ? this.nテクスチャの最大幅 : this.nComment最終行の幅, this.nComment行数 * ( (int) Math.Ceiling( (double) this.ft描画用フォント.Size ) ) );
					graphics2 = Graphics.FromImage( bitmap4 );
					Rectangle srcRect = new Rectangle();
					Rectangle destRect = new Rectangle();
					for( int i = 0; i < this.nComment行数; i++ )
					{
						srcRect.X = i * this.nテクスチャの最大幅;
						srcRect.Y = 0;
						srcRect.Width = ( ( i + 1 ) == this.nComment行数 ) ? this.nComment最終行の幅 : this.nテクスチャの最大幅;
						srcRect.Height = bitmap3.Height;
						destRect.X = 0;
						destRect.Y = i * bitmap3.Height;
						destRect.Width = srcRect.Width;
						destRect.Height = srcRect.Height;
						graphics2.DrawImage( bitmap3, destRect, srcRect, GraphicsUnit.Pixel );
					}
					graphics2.Dispose();
					try
					{
						this.txComment = new CTexture( CDTXMania.app.Device, bitmap4, CDTXMania.TextureFormat );
						this.txComment.vcScaleRatio = new Vector3( 0.5f, 0.5f, 1f );
					}
					catch( CTextureCreateFailedException )
					{
						Trace.TraceError( "COMMENTテクスチャの生成に失敗しました。" );
						this.txComment = null;
					}
					bitmap4.Dispose();
					bitmap3.Dispose();
				}
				image.Dispose();
				if( this.txComment != null )
				{
					this.ctComment = new CCounter( -740, (int) ( ( ( ( this.nComment行数 - 1 ) * this.nテクスチャの最大幅 ) + this.nComment最終行の幅 ) * this.txComment.vcScaleRatio.X ), 10, CDTXMania.Timer );
				}
			}
		}


		// CActivity 実装

		public override void OnActivate()
		{
			this.ft描画用フォント = new Font( "MS PGothic", 40f, GraphicsUnit.Pixel );
			this.txArtist = null;
			this.txComment = null;
			this.strArtist = "";
			this.strComment = "";
			this.nComment最終行の幅 = 0;
			this.nComment行数 = 0;
			this.nテクスチャの最大幅 = 0;
			this.ctComment = new CCounter();
			base.OnActivate();
		}
		public override void OnDeactivate()
		{
			CDTXMania.tReleaseTexture( ref this.txArtist );
			CDTXMania.tReleaseTexture( ref this.txComment );
			if( this.ft描画用フォント != null )
			{
				this.ft描画用フォント.Dispose();
				this.ft描画用フォント = null;
			}
			this.ctComment = null;
			base.OnDeactivate();
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
                this.txコメントバー = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\5_comment bar.png"), true);
                this.t選択曲が変更された();
				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if( !base.bNotActivated )
			{
				CDTXMania.tReleaseTexture( ref this.txArtist );
				CDTXMania.tReleaseTexture( ref this.txComment );
                CDTXMania.tReleaseTexture( ref this.txコメントバー );
                base.OnManagedReleaseResources();
			}
		}
		public override int OnUpdateAndDraw()
		{
			if( !base.bNotActivated )
			{
                if (this.txコメントバー != null)
                    this.txコメントバー.tDraw2D(CDTXMania.app.Device, 560, 257);

                if (this.ctComment.bInProgress)
				{
					this.ctComment.tUpdateLoop();
				}
				if( this.txArtist != null )
				{
					int x = 1260 - 25 - ( (int) ( this.txArtist.szTextureSize.Width * this.txArtist.vcScaleRatio.X ) );		// #27648 2012.3.14 yyagi: -12 for scrollbar
					int y = 320;
					this.txArtist.tDraw2D( CDTXMania.app.Device, x, y );
                    //this.txArtist.tDraw2D(CDTXMania.app.Device, 64, 570);
                }

                    int num3 = 683;
                    int num4 = 339;

                if ((this.txComment != null) && ((this.txComment.szTextureSize.Width * this.txComment.vcScaleRatio.X) < (1250 - num3)))
                {
                    this.txComment.tDraw2D(CDTXMania.app.Device, num3, num4);
                }
                else if (this.txComment != null)
                {
                    Rectangle rectangle = new Rectangle(this.ctComment.nCurrentValue, 0, 750, (int)this.ft描画用フォント.Size);
                    if (rectangle.X < 0)
                    {
                        num3 += -rectangle.X;
                        rectangle.Width -= -rectangle.X;
                        rectangle.X = 0;
                    }
                    int num5 = ((int)(((float)rectangle.X) / this.txComment.vcScaleRatio.X)) / this.nテクスチャの最大幅;
                    Rectangle rectangle2 = new Rectangle();
                    while (rectangle.Width > 0)
                    {
                        rectangle2.X = ((int)(((float)rectangle.X) / this.txComment.vcScaleRatio.X)) % this.nテクスチャの最大幅;
                        rectangle2.Y = num5 * ((int)this.ft描画用フォント.Size);
                        int num6 = ((num5 + 1) == this.nComment行数) ? this.nComment最終行の幅 : this.nテクスチャの最大幅;
                        int num7 = num6 - rectangle2.X;
                        rectangle2.Width = num7;
                        rectangle2.Height = (int)this.ft描画用フォント.Size;
                        this.txComment.tDraw2D(CDTXMania.app.Device, num3, num4, rectangle2);
                        if (++num5 == this.nComment行数)
                        {
                            break;
                        }
                        int num8 = (int)(rectangle2.Width * this.txComment.vcScaleRatio.X);
                        rectangle.X += num8;
                        rectangle.Width -= num8;
                        num3 += num8;
                    }
                }
			}
			return 0;
		}


		// Other

		#region [ private ]
		//-----------------
        private CTexture txコメントバー;
        private CCounter ctComment;
		private Font ft描画用フォント;
		private int nComment行数;
		private int nComment最終行の幅;
		private const int nComment表示幅 = 510;
		private int nテクスチャの最大幅;
		private string strArtist;
		private string strComment;
		private CTexture txArtist;
		private CTexture txComment;
		//-----------------
		#endregion
	}
}
