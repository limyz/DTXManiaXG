using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using SharpDX;
using SharpDX.Direct3D9;
using FDK;

using Rectangle = System.Drawing.Rectangle;

namespace DTXMania
{
	internal class CActSelectPreimagePanel : CActivity
	{
		// メソッド

		public CActSelectPreimagePanel()
		{
            base.listChildActivities.Add( this.actステータスパネル = new CActSelectStatusPanel() );
            base.bNotActivated = true;
		}
		public void t選択曲が変更された()
		{
			this.ct遅延表示 = new CCounter( -CDTXMania.ConfigIni.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms, 100, 1, CDTXMania.Timer );
			this.b新しいプレビューファイルを読み込んだ = false;
		}

		public bool bIsPlayingPremovie		// #27060
		{
			get
			{
				return (this.rAVI != null);
			}
		}

		// CActivity 実装

		public override void OnActivate()
		{
			this.n本体X = 8;
			this.n本体Y = 57;
			this.r表示するプレビュー画像 = this.txプレビュー画像がないときの画像;
			this.str現在のファイル名 = "";
			this.b新しいプレビューファイルを読み込んだ = false;
			base.OnActivate();
		}
		public override void OnDeactivate()
		{
			this.ct登場アニメ用 = null;
			this.ct遅延表示 = null;
			if( this.rAVI != null )
			{
				this.rAVI.Dispose();
				this.rAVI = null;
			}
			base.OnDeactivate();
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
				this.txパネル本体 = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\5_preimage panel.png" ), false );
				this.txプレビュー画像 = null;
				this.txプレビュー画像がないときの画像 = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\5_preimage default.png" ), false );
                this.sfAVI画像 = Surface.CreateOffscreenPlain( CDTXMania.app.Device, 0xcc, 0x10d, CDTXMania.app.GraphicsDeviceManager.CurrentSettings.BackBufferFormat, Pool.SystemMemory );
				this.nAVI再生開始時刻 = -1L;
				this.n前回描画したフレーム番号 = -1;
				this.b動画フレームを作成した = false;
				this.pAVIBmp = IntPtr.Zero;
				this.tプレビュー画像_動画の変更();
				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if( !base.bNotActivated )
			{
				CDTXMania.tReleaseTexture( ref this.txパネル本体 );
				CDTXMania.tReleaseTexture( ref this.txプレビュー画像 );
				CDTXMania.tReleaseTexture( ref this.txプレビュー画像がないときの画像 );
                CDTXMania.tReleaseTexture( ref this.r表示するプレビュー画像 );
                if ( this.sfAVI画像 != null )
				{
					this.sfAVI画像.Dispose();
					this.sfAVI画像 = null;
				}
				base.OnManagedReleaseResources();
			}
		}
		public override int OnUpdateAndDraw()
		{
			if( !base.bNotActivated )
			{
				if( base.bJustStartedUpdate )
				{
					this.ct登場アニメ用 = new CCounter( 0, 100, 5, CDTXMania.Timer );
					base.bJustStartedUpdate = false;
				}
				this.ct登場アニメ用.tUpdate();
				if( ( !CDTXMania.stageSongSelection.bScrolling && ( this.ct遅延表示 != null ) ) && this.ct遅延表示.bInProgress )
				{
					this.ct遅延表示.tUpdate();
					if( this.ct遅延表示.bReachedEndValue )
					{
						this.ct遅延表示.tStop();
					}
					else if( ( this.ct遅延表示.nCurrentValue >= 0 ) && this.b新しいプレビューファイルをまだ読み込んでいない )
					{
						this.tプレビュー画像_動画の変更();
						CDTXMania.Timer.tUpdate();
						this.ct遅延表示.nCurrentElapsedTimeMs = CDTXMania.Timer.nCurrentTime;
						this.b新しいプレビューファイルを読み込んだ = true;
					}
				}
				//else if( ( ( this.rAVI != null ) && ( this.sfAVI画像 != null ) ) && ( this.nAVI再生開始時刻 != -1 ) )
				//{
				//	int time = (int) ( ( CDTXMania.Timer.nCurrentTime - this.nAVI再生開始時刻 ) * ( ( (double) CDTXMania.ConfigIni.nPlaySpeed ) / 20.0 ) );
				//	int frameNoFromTime = this.rAVI.GetFrameNoFromTime( time );
				//	if( frameNoFromTime >= this.rAVI.GetMaxFrameCount() )
				//	{
				//		this.nAVI再生開始時刻 = CDTXMania.Timer.nCurrentTime;
				//	}
				//	else if( ( this.n前回描画したフレーム番号 != frameNoFromTime ) && !this.b動画フレームを作成した )
				//	{
				//		this.b動画フレームを作成した = true;
				//		this.n前回描画したフレーム番号 = frameNoFromTime;
				//		this.pAVIBmp = this.rAVI.GetFramePtr( frameNoFromTime );
				//	}
				//}
                else
                {
                    if( this.b新しいプレビューファイルをまだ読み込んでいない )
                    {
						this.tプレビュー画像_動画の変更();
						CDTXMania.Timer.tUpdate();
						this.ct遅延表示.nCurrentElapsedTimeMs = CDTXMania.Timer.nCurrentTime;
						this.b新しいプレビューファイルを読み込んだ = true;
                    }
                }
				this.t描画処理_パネル本体();
//				this.t描画処理_ジャンル文字列();
				this.t描画処理_プレビュー画像();
			}
			return 0;
		}


		// Other

		#region [ private ]
		//-----------------
		private CDTX.CAVI rAVI;
		private bool b動画フレームを作成した;
		private CCounter ct遅延表示;
		private CCounter ct登場アニメ用;
		private long nAVI再生開始時刻;
		private int n前回描画したフレーム番号;
		private int n本体X;
		private int n本体Y;
		private IntPtr pAVIBmp;
		private Surface sfAVI画像;
		private string str現在のファイル名;
		private CTexture txパネル本体;
		private CTexture txプレビュー画像;
		private CTexture txプレビュー画像がないときの画像;
		private CTexture r表示するプレビュー画像;
        private CActSelectStatusPanel actステータスパネル;
        private bool b新しいプレビューファイルを読み込んだ;
		private bool b新しいプレビューファイルをまだ読み込んでいない
		{
			get
			{
				return !this.b新しいプレビューファイルを読み込んだ;
			}
			set
			{
				this.b新しいプレビューファイルを読み込んだ = !value;
			}
		}

		private unsafe void tサーフェイスをクリアする( Surface sf )
		{
			DataRectangle rectangle = sf.LockRectangle( LockFlags.None );
			IntPtr dataPointer = rectangle.DataPointer;
			switch( ( rectangle.Pitch / sf.Description.Width ) )
			{
				case 4:
					{
						uint* numPtr = (uint*) dataPointer.ToPointer();
						for( int i = 0; i < sf.Description.Height; i++ )
						{
							for( int j = 0; j < sf.Description.Width; j++ )
							{
								( numPtr + ( i * sf.Description.Width ) )[ j ] = 0;
							}
						}
						break;
					}
				case 2:
					{
						ushort* numPtr2 = (ushort*) dataPointer.ToPointer();
						for( int k = 0; k < sf.Description.Height; k++ )
						{
							for( int m = 0; m < sf.Description.Width; m++ )
							{
								( numPtr2 + ( k * sf.Description.Width ) )[ m ] = 0;
							}
						}
						break;
					}
			}
			sf.UnlockRectangle();
		}
		private void tプレビュー画像_動画の変更()
		{
			if( this.rAVI != null )
			{
				this.rAVI.Dispose();
				this.rAVI = null;
			}
			this.pAVIBmp = IntPtr.Zero;
			this.nAVI再生開始時刻 = -1;
			if( !CDTXMania.ConfigIni.bストイックモード )
			{
				if( this.tプレビュー動画の指定があれば構築する() )
				{
					return;
				}
				if( this.tプレビュー画像の指定があれば構築する() )
				{
					return;
				}
				if( this.t背景画像があればその一部からプレビュー画像を構築する() )
				{
					return;
				}
			}
			this.r表示するプレビュー画像 = this.txプレビュー画像がないときの画像;
			this.str現在のファイル名 = "";
		}
		private bool tプレビュー画像の指定があれば構築する()
		{
			CScore cスコア = CDTXMania.stageSongSelection.rSelectedScore;
			if( ( cスコア == null ) || string.IsNullOrEmpty( cスコア.SongInformation.Preimage ) )
			{
				return false;
			}
			string str = cスコア.FileInformation.AbsoluteFolderPath + cスコア.SongInformation.Preimage;
			if( !str.Equals( this.str現在のファイル名 ) )
			{
				CDTXMania.tReleaseTexture( ref this.txプレビュー画像 );
				this.str現在のファイル名 = str;
				if( !File.Exists( this.str現在のファイル名 ) )
				{
					Trace.TraceWarning( "ファイルが存在しません。({0})", new object[] { this.str現在のファイル名 } );
					return false;
				}
				this.txプレビュー画像 = CDTXMania.tGenerateTexture( this.str現在のファイル名, false );
				if( this.txプレビュー画像 != null )
				{
					this.r表示するプレビュー画像 = this.txプレビュー画像;
				}
				else
				{
					this.r表示するプレビュー画像 = this.txプレビュー画像がないときの画像;
				}
			}
			return true;
		}
		private bool tプレビュー動画の指定があれば構築する()
		{
			CScore cスコア = CDTXMania.stageSongSelection.rSelectedScore;
			if( ( CDTXMania.ConfigIni.bAVIEnabled && ( cスコア != null ) ) && !string.IsNullOrEmpty( cスコア.SongInformation.Premovie ) )
			{
				string filename = cスコア.FileInformation.AbsoluteFolderPath + cスコア.SongInformation.Premovie;
				if( filename.Equals( this.str現在のファイル名 ) )
				{
					return true;
				}
				if( this.rAVI != null )
				{
					this.rAVI.Dispose();
					this.rAVI = null;
				}
				this.str現在のファイル名 = filename;
				if( !File.Exists( this.str現在のファイル名 ) )
				{
					Trace.TraceWarning( "ファイルが存在しません。({0})", new object[] { this.str現在のファイル名 } );
					return false;
				}
				try
				{
					CDTX.CAVI cAVI = new CDTX.CAVI(0, filename, "", (int)CDTXMania.ConfigIni.nPlaySpeed);
					cAVI.OnDeviceCreated();

					this.rAVI = cAVI;
					this.nAVI再生開始時刻 = CDTXMania.Timer.nCurrentTime;
					this.n前回描画したフレーム番号 = -1;
					this.b動画フレームを作成した = false;
					this.tサーフェイスをクリアする( this.sfAVI画像 );
					Trace.TraceInformation( "動画を生成しました。({0})", new object[] { filename } );
				}
				catch
				{
					Trace.TraceError( "動画の生成に失敗しました。({0})", new object[] { filename } );
					this.rAVI = null;
					this.nAVI再生開始時刻 = -1;
				}
			}
			return false;
		}
		private bool t背景画像があればその一部からプレビュー画像を構築する()
		{
			CScore cスコア = CDTXMania.stageSongSelection.rSelectedScore;
			if( ( cスコア == null ) || string.IsNullOrEmpty( cスコア.SongInformation.Backgound ) )
			{
				return false;
			}
			string path = cスコア.FileInformation.AbsoluteFolderPath + cスコア.SongInformation.Backgound;
			if( !path.Equals( this.str現在のファイル名 ) )
			{
				if( !File.Exists( path ) )
				{
					Trace.TraceWarning( "ファイルが存在しません。({0})", new object[] { path } );
					return false;
				}
				CDTXMania.tReleaseTexture( ref this.txプレビュー画像 );
				this.str現在のファイル名 = path;
				Bitmap image = null;
				Bitmap bitmap2 = null;
				Bitmap bitmap3 = null;
				try
				{
					image = new Bitmap( this.str現在のファイル名 );
					bitmap2 = new Bitmap(SampleFramework.GameWindowSize.Width, SampleFramework.GameWindowSize.Height);
					Graphics graphics = Graphics.FromImage( bitmap2 );
					int x = 0;
					for (int i = 0; i < SampleFramework.GameWindowSize.Height; i += image.Height)
					{
						for (x = 0; x < SampleFramework.GameWindowSize.Width; x += image.Width)
						{
							graphics.DrawImage( image, x, i, image.Width, image.Height );
						}
					}
					graphics.Dispose();
					bitmap3 = new Bitmap( 0xcc, 0x10d );
					graphics = Graphics.FromImage( bitmap3 );
					graphics.DrawImage( bitmap2, 5, 5, new Rectangle( 0x157, 0x6d, 204, 269 ), GraphicsUnit.Pixel );
					graphics.Dispose();
					this.txプレビュー画像 = new CTexture( CDTXMania.app.Device, bitmap3, CDTXMania.TextureFormat );
					this.r表示するプレビュー画像 = this.txプレビュー画像;
				}
				catch
				{
					Trace.TraceError( "背景画像の読み込みに失敗しました。({0})", new object[] { this.str現在のファイル名 } );
					this.r表示するプレビュー画像 = this.txプレビュー画像がないときの画像;
					return false;
				}
				finally
				{
					if( image != null )
					{
						image.Dispose();
					}
					if( bitmap2 != null )
					{
						bitmap2.Dispose();
					}
					if( bitmap3 != null )
					{
						bitmap3.Dispose();
					}
				}
			}
			return true;
		}
		private void t描画処理_ジャンル文字列()
		{
			CSongListNode c曲リストノード = CDTXMania.stageSongSelection.r現在選択中の曲;
			CScore cスコア = CDTXMania.stageSongSelection.rSelectedScore;
			if( ( c曲リストノード != null ) && ( cスコア != null ) )
			{
				string str = "";
				switch( c曲リストノード.eNodeType )
				{
					case CSongListNode.ENodeType.SCORE:
						if( ( c曲リストノード.strジャンル == null ) || ( c曲リストノード.strジャンル.Length <= 0 ) )
						{
							if( ( cスコア.SongInformation.Genre != null ) && ( cスコア.SongInformation.Genre.Length > 0 ) )
							{
								str = cスコア.SongInformation.Genre;
							}
							else
							{
								switch( cスコア.SongInformation.SongType )
								{
									case CDTX.EType.DTX:
										str = "DTX";
										break;

									case CDTX.EType.GDA:
										str = "GDA";
										break;

									case CDTX.EType.G2D:
										str = "G2D";
										break;

									case CDTX.EType.BMS:
										str = "BMS";
										break;

									case CDTX.EType.BME:
										str = "BME";
										break;
								}
								str = "Unknown";
							}
							break;
						}
						str = c曲リストノード.strジャンル;
						break;

					case CSongListNode.ENodeType.SCORE_MIDI:
						str = "MIDI";
						break;

					case CSongListNode.ENodeType.BOX:
						str = "MusicBox";
						break;

					case CSongListNode.ENodeType.BACKBOX:
						str = "BackBox";
						break;

					case CSongListNode.ENodeType.RANDOM:
						str = "Random";
						break;

					default:
						str = "Unknown";
						break;
				}
				CDTXMania.actDisplayString.tPrint( this.n本体X + 0x12, this.n本体Y - 30, CCharacterConsole.EFontType.RedThin, str );
			}
		}
		private void t描画処理_パネル本体()
		{
            int n基X = 0x12;
            int n基Y = 0x58;

            if (this.actステータスパネル.txパネル本体 != null)
            {
                n基X = 250;
                n基Y = 34;
            }

            if ( this.ct登場アニメ用.bReachedEndValue || ( this.txパネル本体 != null ) )
			{
				this.n本体X = n基X;
				this.n本体Y = n基Y;
			}
			else
			{
				double num = ( (double) this.ct登場アニメ用.nCurrentValue ) / 100.0;
				double num2 = Math.Cos( ( 1.5 + ( 0.5 * num ) ) * Math.PI );
				this.n本体X = n基X;
				this.n本体Y = n基Y - ( (int) ( this.txパネル本体.szImageSize.Height * ( 1.0 - ( num2 * num2 ) ) ) );
			}
			if( this.txパネル本体 != null )
			{
				this.txパネル本体.tDraw2D( CDTXMania.app.Device, this.n本体X, this.n本体Y );
			}
		}
		private unsafe void t描画処理_プレビュー画像()
		{
			if( !CDTXMania.stageSongSelection.bScrolling && ( ( ( this.ct遅延表示 != null ) && ( this.ct遅延表示.nCurrentValue > 0 ) ) && !this.b新しいプレビューファイルをまだ読み込んでいない ) )
			{
                int n差X = 0x25;
                int n差Y = 0x18;
                int n表示ジャケットサイズ = 368;

                if (this.actステータスパネル.txパネル本体 != null)
                {
                    n差X = 8;
                    n差Y = 8;
                    n表示ジャケットサイズ = 292;
                }

                int x = this.n本体X + n差X;
                int y = this.n本体Y + n差Y;
                int z = n表示ジャケットサイズ;
				float num3 = ( (float) this.ct遅延表示.nCurrentValue ) / 100f;
				float num4 = 0.9f + ( 0.1f * num3 );
				if( ( this.nAVI再生開始時刻 != -1 ) && ( this.sfAVI画像 != null ) )
				{
					if( this.b動画フレームを作成した && ( this.pAVIBmp != IntPtr.Zero ) )
					{
						DataRectangle rectangle = this.sfAVI画像.LockRectangle( LockFlags.None );
						IntPtr dataPointer = rectangle.DataPointer;
						int num5 = rectangle.Pitch / this.sfAVI画像.Description.Width;
						BitmapUtil.BITMAPINFOHEADER* pBITMAPINFOHEADER = (BitmapUtil.BITMAPINFOHEADER*) this.pAVIBmp.ToPointer();
						if( pBITMAPINFOHEADER->biBitCount == 0x18 )
						{
							//switch( num5 )
							//{
							//	case 2:
							//		this.rAVI.tBitmap24ToGraphicsStreamR5G6B5( pBITMAPINFOHEADER, dataPointer, this.sfAVI画像.Description.Width, this.sfAVI画像.Description.Height );
							//		break;

							//	case 4:
							//		this.rAVI.tBitmap24ToGraphicsStreamX8R8G8B8( pBITMAPINFOHEADER, dataPointer, this.sfAVI画像.Description.Width, this.sfAVI画像.Description.Height );
							//		break;
							//}
						}
						this.sfAVI画像.UnlockRectangle();
						this.b動画フレームを作成した = false;
					}
                    x += (z - this.sfAVI画像.Description.Width) / 2;
                    y += (z - this.sfAVI画像.Description.Height) / 2;
                    using (Surface surface = CDTXMania.app.Device.GetBackBuffer(0, 0))
                    {
						CDTXMania.app.Device.UpdateSurface( this.sfAVI画像, new SharpDX.Rectangle( 0, 0, this.sfAVI画像.Description.Width, this.sfAVI画像.Description.Height ), surface, new SharpDX.Point( x, y ) );
						return;
					}
				}
                if (this.r表示するプレビュー画像 != null)
                {
                    float width = this.r表示するプレビュー画像.szImageSize.Width;
                    float height = this.r表示するプレビュー画像.szImageSize.Height;
                    float f倍率;
                    if (((float) z / width) > ((float) z / height))
                    {
                        f倍率 = (float) z / height;
                    }
                    else
                    {
                        f倍率 = (float) z / width;
                    }
                    x += (z - ((int)(width * num4 * f倍率))) / 2;
                    y += (z - ((int)(height * num4 * f倍率))) / 2;
                    this.r表示するプレビュー画像.nTransparency = (int)(255f * num3);
                    this.r表示するプレビュー画像.vcScaleRatio.X = num4 * f倍率;
                    this.r表示するプレビュー画像.vcScaleRatio.Y = num4 * f倍率;
                    this.r表示するプレビュー画像.tDraw2D(CDTXMania.app.Device, x, y);
                }
			}
		}
		//-----------------
		#endregion
	}
}
