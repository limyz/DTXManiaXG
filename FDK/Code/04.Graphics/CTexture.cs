using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
using SharpDX;
using SharpDX.Direct3D9;

using Rectangle = System.Drawing.Rectangle;
using RectangleF = System.Drawing.RectangleF;

namespace FDK
{
	/// <summary>
	/// テクスチャを扱うクラス。
	/// 使用終了時は必ずDispose()してください。Finalize時の自動Disposeはありません。
	/// Disposeを忘れた場合は、メモリリークに直結します。
	/// Finalize時にDisposeしない代わりに、Finalize時にテクスチャのDispose漏れを検出し、
	/// Trace.TraceWarning()でログを出力します。
	/// see also:
	/// https://osdn.net/projects/dtxmania/ticket/38036
	/// https://github.com/sharpdx/SharpDX/pull/192?w=1
	/// </summary>
	public class CTexture : IDisposable
	{
		// プロパティ
		public bool bAdditiveBlending { get; set; }
		public float fZAxisRotation { get; set; }

		public int nTransparency
		{
			get { return this._Transparency; }
			set
			{
				if (value < 0)
				{
					this._Transparency = 0;
				}
				else if (value > 0xff)
				{
					this._Transparency = 0xff;
				}
				else
				{
					this._Transparency = value;
				}
			}
		}

		public Size szTextureSize { get; private set; }
		public Size szImageSize { get; protected set; }
		public Texture texture { get; private set; }
		public Format Format { get; protected set; }
		public Vector3 vcScaleRatio;
		public string filename;

		// 画面が変わるたび以下のプロパティを設定し治すこと。

		public static Size szLogicalScreen = Size.Empty;
		public static Size szPhysicalScreen = Size.Empty;
		public static Rectangle rcPhysicalScreenDrawingArea = Rectangle.Empty;

		/// <summary>
		/// <para>論理画面を1とする場合の物理画面の倍率。</para>
		/// <para>論理値×画面比率＝物理値。</para>
		/// </summary>
		public static float fScreenRatio = 1.0f;

		// コンストラクタ

		public CTexture()
		{
			this.szImageSize = new Size(0, 0);
			this.szTextureSize = new Size(0, 0);
			this._Transparency = 0xff;
			this.texture = null;
			this.bSharpDXTextureDispose完了済み = true;
			this.cvPositionColoredVertexies = null;
			this.bAdditiveBlending = false;
			this.fZAxisRotation = 0f;
			this.vcScaleRatio = new Vector3(1f, 1f, 1f);
			this.filename = ""; // DTXMania rev:693bf14b0d83efc770235c788117190d08a4e531
//			this._txData = null;
		}

		/// <summary>
		/// <para>指定されたビットマップオブジェクトから Managed テクスチャを作成する。</para>
		/// <para>テクスチャのサイズは、BITMAP画像のサイズ以上、かつ、D3D9デバイスで生成可能な最小のサイズに自動的に調節される。
		/// その際、テクスチャの調節後のサイズにあわせた画像の拡大縮小は行わない。</para>
		/// <para>その他、ミップマップ数は 1、Usage は None、Pool は Managed、イメージフィルタは Point、ミップマップフィルタは
		/// None、カラーキーは 0xFFFFFFFF（完全なる黒を透過）になる。</para>
		/// </summary>
		/// <param name="device">Direct3D9 Device デバイス。</param>
		/// <param name="bitmap">Source bitmap 作成元のビットマップ。</param>
		/// <param name="format">Texture format テクスチャのフォーマット。</param>
		/// <exception cref="CTextureCreateFailedException">テクスチャの作成に失敗しました。</exception>
		public CTexture(Device device, Bitmap bitmap, Format format)
			: this()
		{
			try
			{
				this.Format = format;
				this.szImageSize = new Size(bitmap.Width, bitmap.Height);
				this.szTextureSize = this.tGetOptimalTextureSizeNotExceedingSpecifiedSize(device, this.szImageSize);
				this.rcFullImage = new Rectangle(0, 0, this.szImageSize.Width, this.szImageSize.Height);

				using (var stream = new MemoryStream())
				{
					bitmap.Save(stream, ImageFormat.Bmp);
					stream.Seek(0L, SeekOrigin.Begin);
					int colorKey = unchecked((int)0xFF000000);
					this.texture = Texture.FromStream(device, stream, this.szTextureSize.Width,
						this.szTextureSize.Height, 1, Usage.None, format, poolvar, Filter.Point, Filter.None, colorKey);
					this.bSharpDXTextureDispose完了済み = false;
				}
			}
			catch (Exception e)
			{
				this.Dispose();
				throw new CTextureCreateFailedException("ビットマップからのテクスチャの生成に失敗しました。(" + e.Message + ")");
			}
		}

		/// <summary>
		/// <para>空の Managed テクスチャを作成する。</para>
		/// <para>テクスチャのサイズは、指定された希望サイズ以上、かつ、D3D9デバイスで生成可能な最小のサイズに自動的に調節される。
		/// その際、テクスチャの調節後のサイズにあわせた画像の拡大縮小は行わない。</para>
		/// <para>テクスチャのテクセルデータは未初期化。（おそらくゴミデータが入ったまま。）</para>
		/// <para>その他、ミップマップ数は 1、Usage は None、イメージフィルタは Point、ミップマップフィルタは None、
		/// カラーキーは 0x00000000（透過しない）になる。</para>
		/// </summary>
		/// <param name="device">Direct3D9 デバイス。</param>
		/// <param name="n幅">テクスチャの幅（希望値）。</param>
		/// <param name="n高さ">テクスチャの高さ（希望値）。</param>
		/// <param name="format">テクスチャのフォーマット。</param>
		/// <exception cref="CTextureCreateFailedException">テクスチャの作成に失敗しました。</exception>
		public CTexture(Device device, int n幅, int n高さ, Format format)
			: this(device, n幅, n高さ, format, Pool.Managed)
		{
		}

		/// <summary>
		/// <para>指定された画像ファイルから Managed テクスチャを作成する。</para>
		/// <para>利用可能な画像形式は、BMP, JPG, PNG, TGA, DDS, PPM, DIB, HDR, PFM のいずれか。</para>
		/// </summary>
		/// <param name="device">Direct3D9 デバイス。</param>
		/// <param name="strファイル名">画像ファイル名。</param>
		/// <param name="format">テクスチャのフォーマット。</param>
		/// <param name="bBlackIsTransparent">画像の黒（0xFFFFFFFF）を透過させるなら true。</param>
		/// <exception cref="CTextureCreateFailedException">テクスチャの作成に失敗しました。</exception>
		public CTexture(Device device, string strファイル名, Format format, bool bBlackIsTransparent)
			: this(device, strファイル名, format, bBlackIsTransparent, Pool.Managed)
		{
		}

		public CTexture(Device device, byte[] txData, Format format, bool bBlackIsTransparent)
			: this(device, txData, format, bBlackIsTransparent, Pool.Managed)
		{
		}

		public CTexture(Device device, Bitmap bitmap, Format format, bool bBlackIsTransparent)
			: this(device, bitmap, format, bBlackIsTransparent, Pool.Managed)
		{
		}

		/// <summary>
		/// <para>空のテクスチャを作成する。</para>
		/// <para>テクスチャのサイズは、指定された希望サイズ以上、かつ、D3D9デバイスで生成可能な最小のサイズに自動的に調節される。
		/// その際、テクスチャの調節後のサイズにあわせた画像の拡大縮小は行わない。</para>
		/// <para>テクスチャのテクセルデータは未初期化。（おそらくゴミデータが入ったまま。）</para>
		/// <para>その他、ミップマップ数は 1、Usage は None、イメージフィルタは Point、ミップマップフィルタは None、
		/// カラーキーは 0x00000000（透過しない）になる。</para>
		/// </summary>
		/// <param name="device">Direct3D9 デバイス。</param>
		/// <param name="n幅">テクスチャの幅（希望値）。</param>
		/// <param name="n高さ">テクスチャの高さ（希望値）。</param>
		/// <param name="format">テクスチャのフォーマット。</param>
		/// <param name="pool">テクスチャの管理方法。</param>
		/// <exception cref="CTextureCreateFailedException">テクスチャの作成に失敗しました。</exception>
		public CTexture(Device device, int n幅, int n高さ, Format format, Pool pool)
			: this(device, n幅, n高さ, format, pool, Usage.None)
		{
		}

		public CTexture(Device device, int n幅, int n高さ, Format format, Pool pool, Usage usage)
			: this()
		{
			try
			{
				this.Format = format;
				this.szImageSize = new Size(n幅, n高さ);
				this.szTextureSize = this.tGetOptimalTextureSizeNotExceedingSpecifiedSize(device, this.szImageSize);
				this.rcFullImage = new Rectangle(0, 0, this.szImageSize.Width, this.szImageSize.Height);

				using (var bitmap = new Bitmap(1, 1))
				{
					using (var graphics = Graphics.FromImage(bitmap))
					{
						graphics.FillRectangle(Brushes.Black, 0, 0, 1, 1);
					}

					using (var stream = new MemoryStream())
					{
						bitmap.Save(stream, ImageFormat.Bmp);
						stream.Seek(0L, SeekOrigin.Begin);
#if TEST_Direct3D9Ex
						pool = poolvar;
#endif
						// 中で更にメモリ読み込みし直していて無駄なので、Streamを使うのは止めたいところ
						this.texture = Texture.FromStream(device, stream, n幅, n高さ, 1, usage, format, pool, Filter.Point,
							Filter.None, 0);
						this.bSharpDXTextureDispose完了済み = false;
					}
				}
			}
			catch
			{
				this.Dispose();
				throw new CTextureCreateFailedException(string.Format("テクスチャの生成に失敗しました。\n({0}x{1}, {2})", n幅, n高さ,
					format));
			}
		}

		/// <summary>
		/// <para>画像ファイルからテクスチャを生成する。</para>
		/// <para>利用可能な画像形式は、BMP, JPG, PNG, TGA, DDS, PPM, DIB, HDR, PFM のいずれか。</para>
		/// <para>テクスチャのサイズは、画像のサイズ以上、かつ、D3D9デバイスで生成可能な最小のサイズに自動的に調節される。
		/// その際、テクスチャの調節後のサイズにあわせた画像の拡大縮小は行わない。</para>
		/// <para>その他、ミップマップ数は 1、Usage は None、イメージフィルタは Point、ミップマップフィルタは None になる。</para>
		/// </summary>
		/// <param name="device">Direct3D9 デバイス。</param>
		/// <param name="strファイル名">画像ファイル名。</param>
		/// <param name="format">テクスチャのフォーマット。</param>
		/// <param name="bBlackIsTransparent">画像の黒（0xFFFFFFFF）を透過させるなら true。</param>
		/// <param name="pool">テクスチャの管理方法。</param>
		/// <exception cref="CTextureCreateFailedException">テクスチャの作成に失敗しました。</exception>
		public CTexture(Device device, string strファイル名, Format format, bool bBlackIsTransparent, Pool pool)
			: this()
		{
			MakeTexture(device, strファイル名, format, bBlackIsTransparent, pool);
		}

		public void MakeTexture(Device device, string strファイル名, Format format, bool b黒を透過する, Pool pool)
		{
			if (!File.Exists(
				    strファイル名)) // #27122 2012.1.13 from: ImageInformation では FileNotFound 例外は返ってこないので、ここで自分でチェックする。わかりやすいログのために。
				throw new FileNotFoundException(string.Format("ファイルが存在しません。\n[{0}]", strファイル名));

			Byte[] _txData = File.ReadAllBytes(strファイル名);
			this.filename = Path.GetFileName(strファイル名);
			MakeTexture(device, _txData, format, b黒を透過する, pool);
		}

		public CTexture(Device device, byte[] txData, Format format, bool bBlackIsTransparent, Pool pool)
			: this()
		{
			MakeTexture(device, txData, format, bBlackIsTransparent, pool);
		}

		public void MakeTexture(Device device, byte[] txData, Format format, bool b黒を透過する, Pool pool)
		{
			try
			{
				var information = ImageInformation.FromMemory(txData);
				this.Format = format;
				this.szImageSize = new Size(information.Width, information.Height);
				this.rcFullImage = new Rectangle(0, 0, this.szImageSize.Width, this.szImageSize.Height);
				int colorKey = (b黒を透過する) ? unchecked((int)0xFF000000) : 0;
				this.szTextureSize = this.tGetOptimalTextureSizeNotExceedingSpecifiedSize(device, this.szImageSize);
#if TEST_Direct3D9Ex
				pool = poolvar;
#endif
				//				lock ( lockobj )
				//				{
				//Trace.TraceInformation( "CTexture() start: " );
				this.texture = Texture.FromMemory(device, txData, this.szImageSize.Width, this.szImageSize.Height, 1,
					Usage.None, format, pool, Filter.Point, Filter.None, colorKey);
				this.bSharpDXTextureDispose完了済み = false;
				//Trace.TraceInformation( "CTexture() end:   " );
				//				}
			}
			catch
			{
				this.Dispose();
				// throw new CTextureCreateFailedException( string.Format( "テクスチャの生成に失敗しました。\n{0}", strファイル名 ) );
				throw new CTextureCreateFailedException(string.Format("テクスチャの生成に失敗しました。\n"));
			}
		}

		public CTexture(Device device, Bitmap bitmap, Format format, bool bBlackIsTransparent, Pool pool)
			: this()
		{
			MakeTexture(device, bitmap, format, bBlackIsTransparent, pool);
		}

		public void MakeTexture(Device device, Bitmap bitmap, Format format, bool b黒を透過する, Pool pool)
		{
			try
			{
				this.Format = format;
				this.szImageSize = new Size(bitmap.Width, bitmap.Height);
				this.rcFullImage = new Rectangle(0, 0, this.szImageSize.Width, this.szImageSize.Height);
				int colorKey = (b黒を透過する) ? unchecked((int)0xFF000000) : 0;
				this.szTextureSize = this.tGetOptimalTextureSizeNotExceedingSpecifiedSize(device, this.szImageSize);
#if TEST_Direct3D9Ex
				pool = poolvar;
#endif
				//Trace.TraceInformation( "CTExture() start: " );
				unsafe // Bitmapの内部データ(a8r8g8b8)を自前でゴリゴリコピーする
				{
					int tw =
#if TEST_Direct3D9Ex
					288;		// 32の倍数にする(グラフによっては2のべき乗にしないとダメかも)
#else
						this.szImageSize.Width;
#endif
#if TEST_Direct3D9Ex
					this.texture =
 new Texture( device, tw, this.sz画像サイズ.Height, 1, Usage.Dynamic, format, Pool.Default );
#else
					this.texture = new Texture(device, this.szImageSize.Width, this.szImageSize.Height, 1, Usage.None,
						format, pool);
#endif
					BitmapData srcBufData =
						bitmap.LockBits(new Rectangle(0, 0, this.szImageSize.Width, this.szImageSize.Height),
							ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
					DataRectangle destDataRectangle = texture.LockRectangle(0, LockFlags.Discard); // None
#if TEST_Direct3D9Ex
					byte[] filldata = null;
					if ( tw > this.sz画像サイズ.Width )
					{
						filldata = new byte[ (tw - this.sz画像サイズ.Width) * 4 ];
					}
					for ( int y = 0; y < this.sz画像サイズ.Height; y++ )
					{
						IntPtr src_scan0 = (IntPtr) ( (Int64) srcBufData.Scan0 + y * srcBufData.Stride );
						destDataRectangle.Data.WriteRange( src_scan0, this.sz画像サイズ.Width * 4  );
						if ( tw > this.sz画像サイズ.Width )
						{
							destDataRectangle.Data.WriteRange( filldata );
						}
					}
#else
					IntPtr src_scan0 = (IntPtr)((Int64)srcBufData.Scan0);
					//destDataRectangle.Data.WriteRange( src_scan0, this.szImageSize.Width * 4 * this.szImageSize.Height );
					CopyMemory(destDataRectangle.DataPointer.ToPointer(), src_scan0.ToPointer(),
						this.szImageSize.Width * 4 * this.szImageSize.Height);
#endif
					texture.UnlockRectangle(0);
					bitmap.UnlockBits(srcBufData);
					this.bSharpDXTextureDispose完了済み = false;
				}
				//Trace.TraceInformation( "CTExture() End: " );
			}
			catch
			{
				this.Dispose();
				// throw new CTextureCreateFailedException( string.Format( "テクスチャの生成に失敗しました。\n{0}", strファイル名 ) );
				throw new CTextureCreateFailedException(string.Format("テクスチャの生成に失敗しました。\n"));
			}
		}
		// メソッド

		/// <summary>
		/// テクスチャを 2D 画像と見なして描画する。
		/// </summary>
		/// <param name="device">Direct3D9 デバイス。</param>
		/// <param name="x">描画位置（テクスチャの左上位置の X 座標[dot]）。</param>
		/// <param name="y">描画位置（テクスチャの左上位置の Y 座標[dot]）。</param>
		public void tDraw2D(Device device, int x, int y)
		{
			this.tDraw2D(device, x, y, 1f, this.rcFullImage);
		}

		public void tDraw2D(Device device, int x, int y, Rectangle rcClipRect)
		{
			this.tDraw2D(device, x, y, 1f, rcClipRect);
		}

		public void tDraw2DFloat(Device device, float x, float y)
		{
			this.tDraw2D(device, x, y, 1f, this.rcFullImage);
		}

		public void tDraw2DFloat(Device device, float x, float y, RectangleF rcClipRect)
		{
			this.tDraw2D(device, x, y, 1f, rcClipRect);
		}

		public void tDraw2D(Device device, float x, float y, float depth, RectangleF rcClipRect)
		{
			if (this.texture == null)
				return;

			this.tRenderStateSettings(device);

			if (this.fZAxisRotation == 0f)
			{
				#region [ (A) 回転なし ]

				//-----------------
				float f補正値X = -0.5f; // -0.5 は座標とピクセルの誤差を吸収するための座標補正値。(MSDN参照)
				float f補正値Y = -0.5f; //
				float w = rcClipRect.Width;
				float h = rcClipRect.Height;
				float fULeft = ((float)rcClipRect.Left) / ((float)this.szTextureSize.Width);
				float fURight = ((float)rcClipRect.Right) / ((float)this.szTextureSize.Width);
				float fVTop = ((float)rcClipRect.Top) / ((float)this.szTextureSize.Height);
				float fVBottom = ((float)rcClipRect.Bottom) / ((float)this.szTextureSize.Height);
				this.color4.Alpha = ((float)this._Transparency) / 255f;
				int color = this.color4.ToRgba();

				if (this.cvTransformedColoredVertexies == null)
					this.cvTransformedColoredVertexies = new TransformedColoredTexturedVertex[4];

				// #27122 2012.1.13 from: 以下、マネージドオブジェクト（＝ガベージ）の量産を抑えるため、new は使わず、メンバに値を１つずつ直接上書きする。

				this.cvTransformedColoredVertexies[0].Position.X = x + f補正値X;
				this.cvTransformedColoredVertexies[0].Position.Y = y + f補正値Y;
				this.cvTransformedColoredVertexies[0].Position.Z = depth;
				this.cvTransformedColoredVertexies[0].Position.W = 1.0f;
				this.cvTransformedColoredVertexies[0].Color = color;
				this.cvTransformedColoredVertexies[0].TextureCoordinates.X = fULeft;
				this.cvTransformedColoredVertexies[0].TextureCoordinates.Y = fVTop;

				this.cvTransformedColoredVertexies[1].Position.X = (x + (w * this.vcScaleRatio.X)) + f補正値X;
				this.cvTransformedColoredVertexies[1].Position.Y = y + f補正値Y;
				this.cvTransformedColoredVertexies[1].Position.Z = depth;
				this.cvTransformedColoredVertexies[1].Position.W = 1.0f;
				this.cvTransformedColoredVertexies[1].Color = color;
				this.cvTransformedColoredVertexies[1].TextureCoordinates.X = fURight;
				this.cvTransformedColoredVertexies[1].TextureCoordinates.Y = fVTop;

				this.cvTransformedColoredVertexies[2].Position.X = x + f補正値X;
				this.cvTransformedColoredVertexies[2].Position.Y = (y + (h * this.vcScaleRatio.Y)) + f補正値Y;
				this.cvTransformedColoredVertexies[2].Position.Z = depth;
				this.cvTransformedColoredVertexies[2].Position.W = 1.0f;
				this.cvTransformedColoredVertexies[2].Color = color;
				this.cvTransformedColoredVertexies[2].TextureCoordinates.X = fULeft;
				this.cvTransformedColoredVertexies[2].TextureCoordinates.Y = fVBottom;

				this.cvTransformedColoredVertexies[3].Position.X = (x + (w * this.vcScaleRatio.X)) + f補正値X;
				this.cvTransformedColoredVertexies[3].Position.Y = (y + (h * this.vcScaleRatio.Y)) + f補正値Y;
				this.cvTransformedColoredVertexies[3].Position.Z = depth;
				this.cvTransformedColoredVertexies[3].Position.W = 1.0f;
				this.cvTransformedColoredVertexies[3].Color = color;
				this.cvTransformedColoredVertexies[3].TextureCoordinates.X = fURight;
				this.cvTransformedColoredVertexies[3].TextureCoordinates.Y = fVBottom;

				device.SetTexture(0, this.texture);
				device.VertexFormat = TransformedColoredTexturedVertex.Format;
				device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 0, 2, this.cvTransformedColoredVertexies);
				//-----------------

				#endregion
			}
			else
			{
				#region [ (B) 回転あり ]

				//-----------------
				float f補正値X = ((rcClipRect.Width % 2) == 0) ? -0.5f : 0f; // -0.5 は座標とピクセルの誤差を吸収するための座標補正値。(MSDN参照)
				float f補正値Y = ((rcClipRect.Height % 2) == 0) ? -0.5f : 0f; // 3D（回転する）なら補正はいらない。
				float f中央X = ((float)rcClipRect.Width) / 2f;
				float f中央Y = ((float)rcClipRect.Height) / 2f;
				float fULeft = ((float)rcClipRect.Left) / ((float)this.szTextureSize.Width);
				float fURight = ((float)rcClipRect.Right) / ((float)this.szTextureSize.Width);
				float fVTop = ((float)rcClipRect.Top) / ((float)this.szTextureSize.Height);
				float fVBottom = ((float)rcClipRect.Bottom) / ((float)this.szTextureSize.Height);
				this.color4.Alpha = ((float)this._Transparency) / 255f;
				int color = this.color4.ToRgba();

				if (this.cvPositionColoredVertexies == null)
					this.cvPositionColoredVertexies = new PositionColoredTexturedVertex[4];

				// #27122 2012.1.13 from: 以下、マネージドオブジェクト（＝ガベージ）の量産を抑えるため、new は使わず、メンバに値を１つずつ直接上書きする。

				this.cvPositionColoredVertexies[0].Position.X = -f中央X + f補正値X;
				this.cvPositionColoredVertexies[0].Position.Y = f中央Y + f補正値Y;
				this.cvPositionColoredVertexies[0].Position.Z = depth;
				this.cvPositionColoredVertexies[0].Color = color;
				this.cvPositionColoredVertexies[0].TextureCoordinates.X = fULeft;
				this.cvPositionColoredVertexies[0].TextureCoordinates.Y = fVTop;

				this.cvPositionColoredVertexies[1].Position.X = f中央X + f補正値X;
				this.cvPositionColoredVertexies[1].Position.Y = f中央Y + f補正値Y;
				this.cvPositionColoredVertexies[1].Position.Z = depth;
				this.cvPositionColoredVertexies[1].Color = color;
				this.cvPositionColoredVertexies[1].TextureCoordinates.X = fURight;
				this.cvPositionColoredVertexies[1].TextureCoordinates.Y = fVTop;

				this.cvPositionColoredVertexies[2].Position.X = -f中央X + f補正値X;
				this.cvPositionColoredVertexies[2].Position.Y = -f中央Y + f補正値Y;
				this.cvPositionColoredVertexies[2].Position.Z = depth;
				this.cvPositionColoredVertexies[2].Color = color;
				this.cvPositionColoredVertexies[2].TextureCoordinates.X = fULeft;
				this.cvPositionColoredVertexies[2].TextureCoordinates.Y = fVBottom;

				this.cvPositionColoredVertexies[3].Position.X = f中央X + f補正値X;
				this.cvPositionColoredVertexies[3].Position.Y = -f中央Y + f補正値Y;
				this.cvPositionColoredVertexies[3].Position.Z = depth;
				this.cvPositionColoredVertexies[3].Color = color;
				this.cvPositionColoredVertexies[3].TextureCoordinates.X = fURight;
				this.cvPositionColoredVertexies[3].TextureCoordinates.Y = fVBottom;

				float n描画領域内X = x + (rcClipRect.Width / 2.0f);
				float n描画領域内Y = y + (rcClipRect.Height / 2.0f);
				var vc3移動量 = new Vector3(n描画領域内X - (((float)device.Viewport.Width) / 2f),
					-(n描画領域内Y - (((float)device.Viewport.Height) / 2f)), 0f);

				var matrix = Matrix.Identity * Matrix.Scaling(this.vcScaleRatio);
				matrix *= Matrix.RotationZ(this.fZAxisRotation);
				matrix *= Matrix.Translation(vc3移動量);
				device.SetTransform(TransformState.World, matrix);

				device.SetTexture(0, this.texture);
				device.VertexFormat = TransformedColoredTexturedVertex.Format;
				device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, this.cvPositionColoredVertexies);
				//-----------------

				#endregion
			}
		}

		public void tDraw2DUpsideDown(Device device, int x, int y)
		{
			this.tDraw2DUpsideDown(device, x, y, 1f, this.rcFullImage);
		}

		public void tDraw2DUpsideDown(Device device, int x, int y, Rectangle rcClipRect)
		{
			this.tDraw2DUpsideDown(device, x, y, 1f, rcClipRect);
		}

		public void tDraw2DUpsideDown(Device device, int x, int y, float depth, Rectangle rcClipRect)
		{
			if (this.texture == null)
				throw new InvalidOperationException("テクスチャは生成されていません。");

			this.tRenderStateSettings(device);

			float fx = x * CTexture.fScreenRatio + CTexture.rcPhysicalScreenDrawingArea.X - 0.5f; // -0.5 は座標とピクセルの誤差を吸収するための座標補正値。(MSDN参照)
			float fy = y * CTexture.fScreenRatio + CTexture.rcPhysicalScreenDrawingArea.Y - 0.5f; //
			float w = rcClipRect.Width * this.vcScaleRatio.X * CTexture.fScreenRatio;
			float h = rcClipRect.Height * this.vcScaleRatio.Y * CTexture.fScreenRatio;
			float fULeft = ((float)rcClipRect.Left) / ((float)this.szTextureSize.Width);
			float fURight = ((float)rcClipRect.Right) / ((float)this.szTextureSize.Width);
			float fVTop = ((float)rcClipRect.Top) / ((float)this.szTextureSize.Height);
			float fVBottom = ((float)rcClipRect.Bottom) / ((float)this.szTextureSize.Height);
			this.color4.Alpha = ((float)this._Transparency) / 255f;
			int color = this.color4.ToRgba();

			if (this.cvTransformedColoredVertexies == null)
				this.cvTransformedColoredVertexies = new TransformedColoredTexturedVertex[4];

			// 以下、マネージドオブジェクトの量産を抑えるため new は使わない。

			this.cvTransformedColoredVertexies[0].TextureCoordinates.X = fULeft; // 左上	→ 左下
			this.cvTransformedColoredVertexies[0].TextureCoordinates.Y = fVBottom;
			this.cvTransformedColoredVertexies[0].Position.X = fx;
			this.cvTransformedColoredVertexies[0].Position.Y = fy;
			this.cvTransformedColoredVertexies[0].Position.Z = depth;
			this.cvTransformedColoredVertexies[0].Position.W = 1.0f;
			this.cvTransformedColoredVertexies[0].Color = color;

			this.cvTransformedColoredVertexies[1].TextureCoordinates.X = fURight; // 右上 → 右下
			this.cvTransformedColoredVertexies[1].TextureCoordinates.Y = fVBottom;
			this.cvTransformedColoredVertexies[1].Position.X = fx + w;
			this.cvTransformedColoredVertexies[1].Position.Y = fy;
			this.cvTransformedColoredVertexies[1].Position.Z = depth;
			this.cvTransformedColoredVertexies[1].Position.W = 1.0f;
			this.cvTransformedColoredVertexies[1].Color = color;

			this.cvTransformedColoredVertexies[2].TextureCoordinates.X = fULeft; // 左下 → 左上
			this.cvTransformedColoredVertexies[2].TextureCoordinates.Y = fVTop;
			this.cvTransformedColoredVertexies[2].Position.X = fx;
			this.cvTransformedColoredVertexies[2].Position.Y = fy + h;
			this.cvTransformedColoredVertexies[2].Position.Z = depth;
			this.cvTransformedColoredVertexies[2].Position.W = 1.0f;
			this.cvTransformedColoredVertexies[2].Color = color;

			this.cvTransformedColoredVertexies[3].TextureCoordinates.X = fURight; // 右下 → 右上
			this.cvTransformedColoredVertexies[3].TextureCoordinates.Y = fVTop;
			this.cvTransformedColoredVertexies[3].Position.X = fx + w;
			this.cvTransformedColoredVertexies[3].Position.Y = fy + h;
			this.cvTransformedColoredVertexies[3].Position.Z = depth;
			this.cvTransformedColoredVertexies[3].Position.W = 1.0f;
			this.cvTransformedColoredVertexies[3].Color = color;

			device.SetTexture(0, this.texture);
			device.VertexFormat = TransformedColoredTexturedVertex.Format;
			device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, this.cvTransformedColoredVertexies);
		}

		public void tDraw2DMatrix(Device device, Matrix transformMatrix, Vector2 size, RectangleF clipRect)
		{
			if (this.texture == null) return;

			//texture dimensions
			float texWidth = this.szTextureSize.Width;
			float texHeight = this.szTextureSize.Height;

			//calculate UV coordinates
			float uLeft = clipRect.Left / texWidth;
			float uRight = clipRect.Right / texWidth;
			float vTop = clipRect.Top / texHeight;
			float vBottom = clipRect.Bottom / texHeight;

			//vertices
			var vertices = new TransformedColoredTexturedVertex[4];
			Vector3[] corners =
			{
				new Vector3(0 - 0.5f, 0 - 0.5f, 0), // TL
				new Vector3(size.X - 0.5f, 0 - 0.5f, 0), // TR
				new Vector3(0 - 0.5f, size.Y - 0.5f, 0), // BL
				new Vector3(size.X - 0.5f, size.Y - 0.5f, 0) // BR
			};

			for (int i = 0; i < corners.Length; i++)
			{
				//transform corner
				Vector3 transformed = Vector3.TransformCoordinate(corners[i], transformMatrix);

				vertices[i] = new TransformedColoredTexturedVertex
				{
					Position = new Vector4(transformed.X, transformed.Y, transformed.Z, 1f),
					TextureCoordinates = i == 0 ? new Vector2(uLeft, vTop) :
						i == 1 ? new Vector2(uRight, vTop) :
						i == 2 ? new Vector2(uLeft, vBottom) :
						new Vector2(uRight, vBottom),
					Color = color4.ToRgba()
				};
			}

			//render texture
			device.SetTexture(0, this.texture);
			device.VertexFormat = TransformedColoredTexturedVertex.Format;
			device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, vertices);
		}

		//todo: cache the vertices on the UI side so this can be more efficient
		//todo: render 9 slices in one draw call
		public void tDraw2DMatrixSliced(Device device, Matrix transformMatrix, Vector2 size, RectangleF clipRect,
			RectangleF sliceRect)
		{
			if (this.texture == null) return;
			
			float texWidth = this.szTextureSize.Width;
			float texHeight = this.szTextureSize.Height;

			//calculate general UV coordinates (defined by cliprect)
			float uLeft = clipRect.Left / texWidth;
			float uRight = clipRect.Right / texWidth;
			float vTop = clipRect.Top / texHeight;
			float vBottom = clipRect.Bottom / texHeight;

			//get the offset sliced region in UV coordinates
			float uSliceLeft = (clipRect.Left + sliceRect.Left) / texWidth;
			float uSliceRight = (clipRect.Left + sliceRect.Right) / texWidth;
			float vSliceTop = (clipRect.Top + sliceRect.Top) / texHeight;
			float vSliceBottom = (clipRect.Top + sliceRect.Bottom) / texHeight;

			//object space dimensions for the 9 regions
			float leftWidth = sliceRect.Left; // Left side width in pixels
			float rightWidth = clipRect.Width - sliceRect.Right; // Right side width in pixels
			float topHeight = sliceRect.Top; // Top side height in pixels
			float bottomHeight = clipRect.Height - sliceRect.Bottom; // Bottom side height in pixels

			float centerWidth = size.X - leftWidth - rightWidth; // Center width in object space
			float centerHeight = size.Y - topHeight - bottomHeight; // Center height in object space

			//define the 9 regions in object space
			var regions = new[]
			{
				//top row
				new RectangleF(0, 0, leftWidth, topHeight), // TL
				new RectangleF(leftWidth, 0, centerWidth, topHeight), // TC
				new RectangleF(leftWidth + centerWidth, 0, rightWidth, topHeight), // TR

				//middle row
				new RectangleF(0, topHeight, leftWidth, centerHeight), // ML
				new RectangleF(leftWidth, topHeight, centerWidth, centerHeight), // MC
				new RectangleF(leftWidth + centerWidth, topHeight, rightWidth, centerHeight), // MR

				//bottom row
				new RectangleF(0, topHeight + centerHeight, leftWidth, bottomHeight), // BL
				new RectangleF(leftWidth, topHeight + centerHeight, centerWidth, bottomHeight), // BC
				new RectangleF(leftWidth + centerWidth, topHeight + centerHeight, rightWidth, bottomHeight), // BR
			};

			//uv coordinates
			var uvRegions = new[]
			{
				//top row
				new RectangleF(uLeft, vTop, uSliceLeft - uLeft, vSliceTop - vTop), // TL
				new RectangleF(uSliceLeft, vTop, uSliceRight - uSliceLeft, vSliceTop - vTop), // TC
				new RectangleF(uSliceRight, vTop, uRight - uSliceRight, vSliceTop - vTop), // TR

				//middle row
				new RectangleF(uLeft, vSliceTop, uSliceLeft - uLeft, vSliceBottom - vSliceTop), // ML
				new RectangleF(uSliceLeft, vSliceTop, uSliceRight - uSliceLeft, vSliceBottom - vSliceTop), // MC
				new RectangleF(uSliceRight, vSliceTop, uRight - uSliceRight, vSliceBottom - vSliceTop), // MR

				//bottom row
				new RectangleF(uLeft, vSliceBottom, uSliceLeft - uLeft, vBottom - vSliceBottom), // BL
				new RectangleF(uSliceLeft, vSliceBottom, uSliceRight - uSliceLeft, vBottom - vSliceBottom), // BC
				new RectangleF(uSliceRight, vSliceBottom, uRight - uSliceRight, vBottom - vSliceBottom), // BR
			};

			//render
			for (int i = 0; i < regions.Length; i++)
			{
				var region = regions[i];
				var uvRegion = uvRegions[i];
				
				Vector3[] corners =
				{
					new Vector3(region.Left - 0.5f, region.Top - 0.5f, 0), // TL
					new Vector3(region.Right - 0.5f, region.Top - 0.5f, 0), // TR
					new Vector3(region.Left - 0.5f, region.Bottom - 0.5f, 0), // BL
					new Vector3(region.Right - 0.5f, region.Bottom - 0.5f, 0), // BR
				};
				
				var vertices = new TransformedColoredTexturedVertex[4];
				for (int j = 0; j < corners.Length; j++)
				{
					//transform the corner
					Vector3 transformed = Vector3.TransformCoordinate(corners[j], transformMatrix);
					vertices[j] = new TransformedColoredTexturedVertex
					{
						Position = new Vector4(transformed.X, transformed.Y, transformed.Z, 1f),
						TextureCoordinates = j == 0 ? new Vector2(uvRegion.Left, uvRegion.Top) :
							j == 1 ? new Vector2(uvRegion.Right, uvRegion.Top) :
							j == 2 ? new Vector2(uvRegion.Left, uvRegion.Bottom) :
							new Vector2(uvRegion.Right, uvRegion.Bottom),
						Color = color4.ToRgba()
					};
				}

				//render the section
				device.SetTexture(0, this.texture);
				device.VertexFormat = TransformedColoredTexturedVertex.Format;
				device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, vertices);
			}
		}


		/// <summary>
		/// テクスチャを 3D 画像と見なして描画する。
		/// </summary>
		public void tDraw3D(Device device, Matrix mat)
		{
			this.tDraw3D(device, mat, this.rcFullImage);
		}

		public void tDraw3D(Device device, Matrix mat, Rectangle rcClipRect)
		{
			if (this.texture == null)
				return;

			float x = ((float)rcClipRect.Width) / 2f;
			float y = ((float)rcClipRect.Height) / 2f;
			float z = 0.0f;
			float fULeft = ((float)rcClipRect.Left) / ((float)this.szTextureSize.Width);
			float fURight = ((float)rcClipRect.Right) / ((float)this.szTextureSize.Width);
			float fVTop = ((float)rcClipRect.Top) / ((float)this.szTextureSize.Height);
			float fVBottom = ((float)rcClipRect.Bottom) / ((float)this.szTextureSize.Height);
			this.color4.Alpha = ((float)this._Transparency) / 255f;
			int color = this.color4.ToRgba();

			if (this.cvPositionColoredVertexies == null)
				this.cvPositionColoredVertexies = new PositionColoredTexturedVertex[4];

			// #27122 2012.1.13 from: 以下、マネージドオブジェクト（＝ガベージ）の量産を抑えるため、new は使わず、メンバに値を１つずつ直接上書きする。

			this.cvPositionColoredVertexies[0].Position.X = -x;
			this.cvPositionColoredVertexies[0].Position.Y = y;
			this.cvPositionColoredVertexies[0].Position.Z = z;
			this.cvPositionColoredVertexies[0].Color = color;
			this.cvPositionColoredVertexies[0].TextureCoordinates.X = fULeft;
			this.cvPositionColoredVertexies[0].TextureCoordinates.Y = fVTop;

			this.cvPositionColoredVertexies[1].Position.X = x;
			this.cvPositionColoredVertexies[1].Position.Y = y;
			this.cvPositionColoredVertexies[1].Position.Z = z;
			this.cvPositionColoredVertexies[1].Color = color;
			this.cvPositionColoredVertexies[1].TextureCoordinates.X = fURight;
			this.cvPositionColoredVertexies[1].TextureCoordinates.Y = fVTop;

			this.cvPositionColoredVertexies[2].Position.X = -x;
			this.cvPositionColoredVertexies[2].Position.Y = -y;
			this.cvPositionColoredVertexies[2].Position.Z = z;
			this.cvPositionColoredVertexies[2].Color = color;
			this.cvPositionColoredVertexies[2].TextureCoordinates.X = fULeft;
			this.cvPositionColoredVertexies[2].TextureCoordinates.Y = fVBottom;

			this.cvPositionColoredVertexies[3].Position.X = x;
			this.cvPositionColoredVertexies[3].Position.Y = -y;
			this.cvPositionColoredVertexies[3].Position.Z = z;
			this.cvPositionColoredVertexies[3].Color = color;
			this.cvPositionColoredVertexies[3].TextureCoordinates.X = fURight;
			this.cvPositionColoredVertexies[3].TextureCoordinates.Y = fVBottom;

			this.tRenderStateSettings(device);

			device.SetTransform(TransformState.World, mat);
			device.SetTexture(0, this.texture);
			device.VertexFormat = PositionColoredTexturedVertex.Format;
			device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, this.cvPositionColoredVertexies);
		}

		public void tDraw3DTopLeftReference(Device device, Matrix mat)
		{
			this.tDraw3DTopLeftReference(device, mat, this.rcFullImage);
		}

		/// <summary>
		/// ○覚書
		///   SharpDX.Matrix mat = SharpDX.Matrix.Identity;
		///   mat *= SharpDX.Matrix.Translation( x, y, z );
		/// 「mat =」ではなく「mat *=」であることを忘れないこと。
		/// </summary>
		public void tDraw3DTopLeftReference(Device device, Matrix mat, Rectangle rcClipRect)
		{
			//とりあえず補正値などは無し。にしても使う機会少なさそうだなー____
			if (this.texture == null)
				return;

			float x = 0.0f;
			float y = 0.0f;
			float z = 0.0f;
			float fULeft = ((float)rcClipRect.Left) / ((float)this.szTextureSize.Width);
			float fURight = ((float)rcClipRect.Right) / ((float)this.szTextureSize.Width);
			float fVTop = ((float)rcClipRect.Top) / ((float)this.szTextureSize.Height);
			float fVBottom = ((float)rcClipRect.Bottom) / ((float)this.szTextureSize.Height);
			this.color4.Alpha = ((float)this._Transparency) / 255f;
			int color = this.color4.ToRgba();

			if (this.cvPositionColoredVertexies == null)
				this.cvPositionColoredVertexies = new PositionColoredTexturedVertex[4];

			// #27122 2012.1.13 from: 以下、マネージドオブジェクト（＝ガベージ）の量産を抑えるため、new は使わず、メンバに値を１つずつ直接上書きする。

			this.cvPositionColoredVertexies[0].Position.X = -x;
			this.cvPositionColoredVertexies[0].Position.Y = y;
			this.cvPositionColoredVertexies[0].Position.Z = z;
			this.cvPositionColoredVertexies[0].Color = color;
			this.cvPositionColoredVertexies[0].TextureCoordinates.X = fULeft;
			this.cvPositionColoredVertexies[0].TextureCoordinates.Y = fVTop;

			this.cvPositionColoredVertexies[1].Position.X = x;
			this.cvPositionColoredVertexies[1].Position.Y = y;
			this.cvPositionColoredVertexies[1].Position.Z = z;
			this.cvPositionColoredVertexies[1].Color = color;
			this.cvPositionColoredVertexies[1].TextureCoordinates.X = fURight;
			this.cvPositionColoredVertexies[1].TextureCoordinates.Y = fVTop;

			this.cvPositionColoredVertexies[2].Position.X = -x;
			this.cvPositionColoredVertexies[2].Position.Y = -y;
			this.cvPositionColoredVertexies[2].Position.Z = z;
			this.cvPositionColoredVertexies[2].Color = color;
			this.cvPositionColoredVertexies[2].TextureCoordinates.X = fULeft;
			this.cvPositionColoredVertexies[2].TextureCoordinates.Y = fVBottom;

			this.cvPositionColoredVertexies[3].Position.X = x;
			this.cvPositionColoredVertexies[3].Position.Y = -y;
			this.cvPositionColoredVertexies[3].Position.Z = z;
			this.cvPositionColoredVertexies[3].Color = color;
			this.cvPositionColoredVertexies[3].TextureCoordinates.X = fURight;
			this.cvPositionColoredVertexies[3].TextureCoordinates.Y = fVBottom;

			this.tRenderStateSettings(device);

			device.SetTransform(TransformState.World, mat);
			device.SetTexture(0, this.texture);
			device.VertexFormat = PositionColoredTexturedVertex.Format;
			device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, this.cvPositionColoredVertexies);
		}

		#region [ IDisposable 実装 ]

		//-----------------
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposeManagedObjects)
		{
			if (this.bDispose完了済み)
				return;

			if (disposeManagedObjects)
			{
				// (A) Managed リソースの解放
				// テクスチャの破棄 (SharpDXのテクスチャは、SharpDX側で管理されるため、FDKからはmanagedリソースと見做す)
				if (this.texture != null)
				{
					this.texture.Dispose();
					this.texture = null;
					this.bSharpDXTextureDispose完了済み = true;
				}
			}

			// (B) Unamanaged リソースの解放


			this.bDispose完了済み = true;
		}

		~CTexture()
		{
			// ファイナライザの動作時にtextureのDisposeがされていない場合は、
			// CTextureのDispose漏れと見做して警告をログ出力する
			if (!this.bSharpDXTextureDispose完了済み)
			{
				Trace.TraceWarning("CTexture: Dispose漏れを検出しました。(Size=({0}, {1}), filename={2})", szImageSize.Width,
					szImageSize.Height, filename);
			}

			this.Dispose(false);
		}

		//-----------------

		#endregion


		// その他

		#region [ private ]

		//-----------------
		private int _Transparency;
		private bool bDispose完了済み, bSharpDXTextureDispose完了済み;
		protected PositionColoredTexturedVertex[] cvPositionColoredVertexies;
		protected TransformedColoredTexturedVertex[] cvTransformedColoredVertexies;
		private const Pool poolvar = // 2011.4.25 yyagi
#if TEST_Direct3D9Ex
			Pool.Default;
#else
			Pool.Managed;
#endif
//		byte[] _txData;
		static object lockobj = new object();

		private void tRenderStateSettings(Device device)
		{
			if (this.bAdditiveBlending)
			{
				device.SetRenderState(RenderState.AlphaBlendEnable, true);
				device.SetRenderState(RenderState.SourceBlend, SharpDX.Direct3D9.Blend.SourceAlpha); // 5
				device.SetRenderState(RenderState.DestinationBlend, SharpDX.Direct3D9.Blend.One); // 2
			}
			else
			{
				device.SetRenderState(RenderState.AlphaBlendEnable, true);
				device.SetRenderState(RenderState.SourceBlend, SharpDX.Direct3D9.Blend.SourceAlpha); // 5
				device.SetRenderState(RenderState.DestinationBlend, SharpDX.Direct3D9.Blend.InverseSourceAlpha); // 6
			}
		}

		private Size tGetOptimalTextureSizeNotExceedingSpecifiedSize(Device device, Size sz指定サイズ)
		{
			bool b条件付きでサイズは２の累乗でなくてもOK = (device.Capabilities.TextureCaps & TextureCaps.NonPow2Conditional) != 0;
			bool bサイズは２の累乗でなければならない = (device.Capabilities.TextureCaps & TextureCaps.Pow2) != 0;
			bool b正方形でなければならない = (device.Capabilities.TextureCaps & TextureCaps.SquareOnly) != 0;
			int n最大幅 = device.Capabilities.MaxTextureWidth;
			int n最大高 = device.Capabilities.MaxTextureHeight;
			var szSize = new Size(sz指定サイズ.Width, sz指定サイズ.Height);

			if (bサイズは２の累乗でなければならない && !b条件付きでサイズは２の累乗でなくてもOK)
			{
				// 幅を２の累乗にする
				int n = 1;
				do
				{
					n *= 2;
				} while (n <= sz指定サイズ.Width);

				sz指定サイズ.Width = n;

				// 高さを２の累乗にする
				n = 1;
				do
				{
					n *= 2;
				} while (n <= sz指定サイズ.Height);

				sz指定サイズ.Height = n;
			}

			if (sz指定サイズ.Width > n最大幅)
				sz指定サイズ.Width = n最大幅;

			if (sz指定サイズ.Height > n最大高)
				sz指定サイズ.Height = n最大高;

			if (b正方形でなければならない)
			{
				if (szSize.Width > szSize.Height)
				{
					szSize.Height = szSize.Width;
				}
				else if (szSize.Width < szSize.Height)
				{
					szSize.Width = szSize.Height;
				}
			}

			return szSize;
		}


		// 2012.3.21 さらなる new の省略作戦

		protected Rectangle rcFullImage; // テクスチャ作ったらあとは不変

		protected Color4 color4 = new Color4(1f, 1f, 1f, 1f); // アルファ以外は不変
		//-----------------

		#endregion

		#region " Win32 API "

		//-----------------
		[System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
		private static extern unsafe void CopyMemory(void* dst, void* src, int size);

		//-----------------

		#endregion
	}
}
