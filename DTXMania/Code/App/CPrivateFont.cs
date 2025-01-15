using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace DTXMania
{
	/// <summary>
	/// プライベートフォントでの描画を扱うクラス。
	/// </summary>
	/// <exception cref="FileNotFoundException">フォントファイルが見つからない時に例外発生</exception>
	/// <exception cref="ArgumentException">スタイル指定不正時に例外発生</exception>
	/// <remarks>
	/// 簡単な使い方
	/// CPrivateFont prvFont = new CPrivateFont( CSkin.Path( @"Graphics\fonts\mplus-1p-bold.ttf" ), 36 );	// プライベートフォント
	/// とか
	/// CPrivateFont prvFont = new CPrivateFont( new FontFamily("MS UI Gothic"), 36, FontStyle.Bold );		// システムフォント
	/// とかした上で、
	/// Bitmap bmp = prvFont.DrawPrivateFont( "ABCDE", Color.White, Color.Black );							// フォント色＝白、縁の色＝黒の例。縁の色は省略可能
	/// とか
	/// Bitmap bmp = prvFont.DrawPrivateFont( "ABCDE", Color.White, Color.Black, Color.Yellow, Color.OrangeRed ); // 上下グラデーション(Yellow→OrangeRed)
	/// とかして、
	/// CTexture ctBmp = CDTXMania.tGenerateTexture( bmp, false );
	/// ctBMP.tDraw2D( ～～～ );
	/// で表示してください。
	/// 
	/// 注意点
	/// 任意のフォントでのレンダリングは結構負荷が大きいので、なるべｋなら描画フレーム毎にフォントを再レンダリングするようなことはせず、
	/// 一旦レンダリングしたものを描画に使い回すようにしてください。
	/// また、長い文字列を与えると、返されるBitmapも横長になります。この横長画像をそのままテクスチャとして使うと、
	/// 古いPCで問題を発生させやすいです。これを回避するには、一旦Bitmapとして取得したのち、256pixや512pixで分割して
	/// テクスチャに定義するようにしてください。
	/// </remarks>
	public class CPrivateFont : IDisposable
	{
		#region [ コンストラクタ ]
		public CPrivateFont(FontFamily fontfamily, int pt, FontStyle style)
		{
			Initialize(null, fontfamily, pt, style);
		}
		public CPrivateFont(FontFamily fontfamily, int pt)
		{
			Initialize(null, fontfamily, pt, FontStyle.Regular);
		}
		public CPrivateFont(string fontpath, int pt, FontStyle style)
		{
			Initialize(fontpath, null, pt, style);
		}
		public CPrivateFont(string fontpath, int pt)
		{
			Initialize(fontpath, null, pt, FontStyle.Regular);
		}
		public CPrivateFont()
		{
			//throw new ArgumentException("CPrivateFont: 引数があるコンストラクタを使用してください。");
		}
		#endregion

		protected void Initialize(string fontpath, FontFamily fontfamily, int pt, FontStyle style)
		{
			this._pfc = null;
			this._fontfamily = null;
			this._font = null;
			this._pt = pt;
			this._rectStrings = new Rectangle(0, 0, 0, 0);
			this._ptOrigin = new Point(0, 0);
			this.bDispose完了済み = false;

			if (fontfamily != null)
			{
				this._fontfamily = fontfamily;
			}
			else
			{
				try
				{
					this._pfc = new System.Drawing.Text.PrivateFontCollection();    //PrivateFontCollectionオブジェクトを作成する
					this._pfc.AddFontFile(fontpath);                                //PrivateFontCollectionにフォントを追加する
				}
				catch (System.IO.FileNotFoundException)
				{
					Trace.TraceError("プライベートフォントの追加に失敗しました。({0})", fontpath);
					throw new FileNotFoundException("プライベートフォントの追加に失敗しました。({0})", Path.GetFileName(fontpath));
					//return;
				}

				//foreach ( FontFamily ff in pfc.Families )
				//{
				//    Debug.WriteLine( "fontname=" + ff.Name );
				//    if ( ff.Name == Path.GetFileNameWithoutExtension( fontpath ) )
				//    {
				//        _fontfamily = ff;
				//        break;
				//    }
				//}
				//if ( _fontfamily == null )
				//{
				//    Trace.TraceError( "プライベートフォントの追加後、検索に失敗しました。({0})", fontpath );
				//    return;
				//}
				_fontfamily = _pfc.Families[0];
			}

			// 指定されたフォントスタイルが適用できない場合は、フォント内で定義されているスタイルから候補を選んで使用する
			// 何もスタイルが使えないようなフォントなら、例外を出す。
			if (_fontfamily != null)
			{
				if (!_fontfamily.IsStyleAvailable(style))
				{
					FontStyle[] FS = { FontStyle.Regular, FontStyle.Bold, FontStyle.Italic, FontStyle.Underline, FontStyle.Strikeout };
					style = FontStyle.Regular | FontStyle.Bold | FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout;  // null非許容型なので、代わりに全盛をNGワードに設定
					foreach (FontStyle ff in FS)
					{
						if (this._fontfamily.IsStyleAvailable(ff))
						{
							style = ff;
							Trace.TraceWarning("フォント{0}へのスタイル指定を、{1}に変更しました。", Path.GetFileName(fontpath), style.ToString());
							break;
						}
					}
					if (style == (FontStyle.Regular | FontStyle.Bold | FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout))
					{
						Trace.TraceWarning("フォント{0}は適切なスタイル{1}を選択できませんでした。", Path.GetFileName(fontpath), style.ToString());
					}
				}
				//this._font = new Font(this._fontfamily, pt, style);			//PrivateFontCollectionの先頭のフォントのFontオブジェクトを作成する
				float emSize = pt * 96.0f / 72.0f;
				this._font = new Font(this._fontfamily, emSize, style, GraphicsUnit.Pixel); //PrivateFontCollectionの先頭のフォントのFontオブジェクトを作成する
																							//HighDPI対応のため、pxサイズで指定
			}
			else
			// フォントファイルが見つからなかった場合 (MS PGothicを代わりに指定する)
			{
				float emSize = pt * 96.0f / 72.0f;
				this._font = new Font("MS PGothic", emSize, style, GraphicsUnit.Pixel); //MS PGothicのFontオブジェクトを作成する
				FontFamily[] ffs = new System.Drawing.Text.InstalledFontCollection().Families;
				int lcid = System.Globalization.CultureInfo.GetCultureInfo("en-us").LCID;
				foreach (FontFamily ff in ffs)
				{
					// Trace.WriteLine( lcid ) );
					if (ff.GetName(lcid) == "MS PGothic")
					{
						this._fontfamily = ff;
						Trace.TraceInformation("MS PGothicを代わりに指定しました。");
						return;
					}
				}
				throw new FileNotFoundException("プライベートフォントの追加に失敗し、MS PGothicでの代替処理にも失敗しました。({0})", Path.GetFileName(fontpath));
			}
		}

		[Flags]
		public enum DrawMode
		{
			Normal,
			Edge,
			Gradation
		}

		#region [ DrawPrivateFontのオーバーロード群 ]
		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <returns>描画済テクスチャ</returns>
		public Bitmap DrawPrivateFont(string drawstr, Color fontColor)
		{
			return DrawPrivateFont(drawstr, DrawMode.Normal, fontColor, Color.White, Color.White, Color.White);
		}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <returns>描画済テクスチャ</returns>
		public Bitmap DrawPrivateFont(string drawstr, Color fontColor, Color edgeColor)
		{
			return DrawPrivateFont(drawstr, DrawMode.Edge, fontColor, edgeColor, Color.White, Color.White);
		}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="gradationTopColor">グラデーション 上側の色</param>
		/// <param name="gradationBottomColor">グラデーション 下側の色</param>
		/// <returns>描画済テクスチャ</returns>
		//public Bitmap DrawPrivateFont( string drawstr, Color fontColor, Color gradationTopColor, Color gradataionBottomColor )
		//{
		//    return DrawPrivateFont( drawstr, DrawMode.Gradation, fontColor, Color.White, gradationTopColor, gradataionBottomColor );
		//}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <param name="gradationTopColor">グラデーション 上側の色</param>
		/// <param name="gradationBottomColor">グラデーション 下側の色</param>
		/// <returns>描画済テクスチャ</returns>
		public Bitmap DrawPrivateFont(string drawstr, Color fontColor, Color edgeColor, Color gradationTopColor, Color gradataionBottomColor)
		{
			return DrawPrivateFont(drawstr, DrawMode.Edge | DrawMode.Gradation, fontColor, edgeColor, gradationTopColor, gradataionBottomColor);
		}

#if こちらは使わない // (Bitmapではなく、CTextureを返す版)
		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <returns>描画済テクスチャ</returns>
		public CTexture DrawPrivateFont( string drawstr, Color fontColor )
		{
			Bitmap bmp = DrawPrivateFont( drawstr, DrawMode.Normal, fontColor, Color.White, Color.White, Color.White );
			return CDTXMania.tテクスチャの生成( bmp, false );
		}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <returns>描画済テクスチャ</returns>
		public CTexture DrawPrivateFont( string drawstr, Color fontColor, Color edgeColor )
		{
			Bitmap bmp = DrawPrivateFont( drawstr, DrawMode.Edge, fontColor, edgeColor, Color.White, Color.White );
			return CDTXMania.tテクスチャの生成( bmp, false );
		}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="gradationTopColor">グラデーション 上側の色</param>
		/// <param name="gradationBottomColor">グラデーション 下側の色</param>
		/// <returns>描画済テクスチャ</returns>
		//public CTexture DrawPrivateFont( string drawstr, Color fontColor, Color gradationTopColor, Color gradataionBottomColor )
		//{
		//    Bitmap bmp = DrawPrivateFont( drawstr, DrawMode.Gradation, fontColor, Color.White, gradationTopColor, gradataionBottomColor );
		//	  return CDTXMania.tテクスチャの生成( bmp, false );
		//}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <param name="gradationTopColor">グラデーション 上側の色</param>
		/// <param name="gradationBottomColor">グラデーション 下側の色</param>
		/// <returns>描画済テクスチャ</returns>
		public CTexture DrawPrivateFont( string drawstr, Color fontColor, Color edgeColor,  Color gradationTopColor, Color gradataionBottomColor )
		{
			Bitmap bmp = DrawPrivateFont( drawstr, DrawMode.Edge | DrawMode.Gradation, fontColor, edgeColor, gradationTopColor, gradataionBottomColor );
			return CDTXMania.tテクスチャの生成( bmp, false );
		}
#endif
		#endregion


		/// <summary>
		/// 文字列を描画したテクスチャを返す(メイン処理)
		/// </summary>
		/// <param name="rectDrawn">描画された領域</param>
		/// <param name="ptOrigin">描画文字列</param>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="drawmode">描画モード</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <param name="gradationTopColor">グラデーション 上側の色</param>
		/// <param name="gradationBottomColor">グラデーション 下側の色</param>
		/// <returns>描画済テクスチャ</returns>
		public Bitmap DrawPrivateFont(string drawstr, DrawMode drawmode, Color fontColor, Color edgeColor, Color gradationTopColor, Color gradationBottomColor)
		{
			if (this._fontfamily == null || drawstr == null || drawstr == "")
			{
				// nullを返すと、その後bmp→texture処理や、textureのサイズを見て__の処理で全部例外が発生することになる。
				// それは非常に面倒なので、最小限のbitmapを返してしまう。
				// まずはこの仕様で進めますが、問題有れば(上位側からエラー検出が必要であれば)例外を出したりエラー状態であるプロパティを定義するなり検討します。
				Trace.TraceError("DrawPrivateFont()の入力不正。最小値のbitmapを返します。");
				_rectStrings = new Rectangle(0, 0, 0, 0);
				_ptOrigin = new Point(0, 0);
				return new Bitmap(1, 1);
			}
			bool bEdge = ((drawmode & DrawMode.Edge) == DrawMode.Edge);
			bool bGradation = ((drawmode & DrawMode.Gradation) == DrawMode.Gradation);

			// 縁取りの縁のサイズは、とりあえずフォントの大きさの1/4とする
			int nEdgePt = (bEdge) ? _pt / 4 : 0;

			// 描画サイズを測定する
			Size stringSize = System.Windows.Forms.TextRenderer.MeasureText(drawstr, this._font, new Size(int.MaxValue, int.MaxValue),
				System.Windows.Forms.TextFormatFlags.NoPrefix |
				System.Windows.Forms.TextFormatFlags.NoPadding
				);

			//取得した描画サイズを基に、描画先のbitmapを作成する
			Bitmap bmp = new Bitmap(stringSize.Width + nEdgePt * 2, stringSize.Height + nEdgePt * 2);
			bmp.MakeTransparent();
			Graphics g = Graphics.FromImage(bmp);
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			StringFormat sf = new StringFormat();
			sf.LineAlignment = StringAlignment.Far; // 画面下部（垂直方向位置）
			sf.Alignment = StringAlignment.Center;  // 画面中央（水平方向位置) //To fix

			// レイアウト枠
			//Rectangle r = new Rectangle( 0, 0, stringSize.Width + nEdgePt * 2, stringSize.Height + nEdgePt * 2 );
			Rectangle r = new Rectangle(0, 0, stringSize.Width + nEdgePt * 2, stringSize.Height + nEdgePt * 2); //#34638 2014.11.24 kairera0467 とりあえず文字の横サイズを1.5倍に変更。

			if (bEdge)  // 縁取り有りの描画
			{
				// DrawPathで、ポイントサイズを使って描画するために、DPIを使って単位変換する
				// (これをしないと、単位が違うために、小さめに描画されてしまう)
				float sizeInPixels = _font.SizeInPoints * g.DpiY / 72;  // 1 inch = 72 points

				System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
				gp.AddString(drawstr, this._fontfamily, (int)this._font.Style, sizeInPixels, r, sf);

				// 縁取りを描画する
				Pen p = new Pen(edgeColor, nEdgePt);
				p.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
				g.DrawPath(p, gp);

				// 塗りつぶす
				Brush br;
				if (bGradation)
				{
					br = new LinearGradientBrush(r, gradationTopColor, gradationBottomColor, LinearGradientMode.Vertical);
				}
				else
				{
					br = new SolidBrush(fontColor);
				}
				g.FillPath(br, gp);

				if (br != null) br.Dispose(); br = null;
				if (p != null) p.Dispose(); p = null;
				if (gp != null) gp.Dispose(); gp = null;
			}
			else
			{
				// 縁取りなしの描画
				System.Windows.Forms.TextRenderer.DrawText(g, drawstr, _font, new Point(0, 0), fontColor);
			}
#if debug表示
			g.DrawRectangle( new Pen( Color.White, 1 ), new Rectangle( 1, 1, stringSize.Width-1, stringSize.Height-1 ) );
			g.DrawRectangle( new Pen( Color.Green, 1 ), new Rectangle( 0, 0, bmp.Width - 1, bmp.Height - 1 ) );
#endif
			_rectStrings = new Rectangle(0, 0, stringSize.Width, stringSize.Height);
			_ptOrigin = new Point(nEdgePt * 2, nEdgePt * 2);


			#region [ リソースを解放する ]
			if (sf != null) sf.Dispose(); sf = null;
			if (g != null) g.Dispose(); g = null;
			#endregion

			return bmp;
		}

		/// <summary>
		/// 文字列を描画したテクスチャを返す(メイン処理)
		/// </summary>
		/// <param name="rectDrawn">描画された領域</param>
		/// <param name="ptOrigin">描画文字列</param>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="drawmode">描画モード</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <param name="gradationTopColor">グラデーション 上側の色</param>
		/// <param name="gradationBottomColor">グラデーション 下側の色</param>
		/// <returns>描画済テクスチャ</returns>
		public Bitmap DrawPrivateFont(string drawstr, DrawMode drawmode, Color fontColor, Color edgeColor, Color gradationTopColor, Color gradationBottomColor, bool bEdgeGradation)
		{
			if (this._fontfamily == null || drawstr == null || drawstr == "")
			{
				// nullを返すと、その後bmp→texture処理や、textureのサイズを見て__の処理で全部例外が発生することになる。
				// それは非常に面倒なので、最小限のbitmapを返してしまう。
				// まずはこの仕様で進めますが、問題有れば(上位側からエラー検出が必要であれば)例外を出したりエラー状態であるプロパティを定義するなり検討します。
				Trace.TraceError("DrawPrivateFont()の入力不正。最小値のbitmapを返します。");
				_rectStrings = new Rectangle(0, 0, 0, 0);
				_ptOrigin = new Point(0, 0);
				return new Bitmap(1, 1);
			}
			bool bEdge = ((drawmode & DrawMode.Edge) == DrawMode.Edge);
			bool bGradation = ((drawmode & DrawMode.Gradation) == DrawMode.Gradation);

			// 縁取りの縁のサイズは、とりあえずフォントの大きさの1/4とする
			// Changed to 1/6 as 1/4 is too thick for new Black-White Style
			int nEdgePt = (bEdge) ? _pt / 6 : 0;

			// 描画サイズを測定する
			Size stringSize = System.Windows.Forms.TextRenderer.MeasureText(drawstr, this._font, new Size(int.MaxValue, int.MaxValue),
				System.Windows.Forms.TextFormatFlags.NoPrefix |
				System.Windows.Forms.TextFormatFlags.NoPadding
				);

			//取得した描画サイズを基に、描画先のbitmapを作成する
			int l_width = (int)(stringSize.Width * 1.05f); //A constant proportion of 5% buffer should avoid the issue of text truncation
			Bitmap bmp = new Bitmap(l_width, stringSize.Height + nEdgePt * 2);
			bmp.MakeTransparent();
			Graphics g = Graphics.FromImage(bmp);
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			StringFormat sf = new StringFormat();
			sf.LineAlignment = StringAlignment.Far; // 画面下部（垂直方向位置）
			sf.Alignment = StringAlignment.Near;    // 画面中央（水平方向位置）//Changed to Left (Near) of Texture rect

			// レイアウト枠
			Rectangle r = new Rectangle(0, 0, l_width, stringSize.Height + nEdgePt * 2);
			//r = new Rectangle( 0, 0, l_width + nEdgePt*2, stringSize.Height + nEdgePt * 2 ); // 2016.06.12 kairera0467 改行防止

			if (bEdge && bEdgeGradation)    // 縁取り有りの描画
			{
				// DrawPathで、ポイントサイズを使って描画するために、DPIを使って単位変換する
				// (これをしないと、単位が違うために、小さめに描画されてしまう)
				float sizeInPixels = _font.SizeInPoints * g.DpiY / 72;  // 1 inch = 72 points

				System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
				gp.AddString(drawstr, this._fontfamily, (int)this._font.Style, sizeInPixels, r, sf);

				// 縁取りを描画する
				Brush br縁;
				br縁 = new LinearGradientBrush(r, gradationTopColor, gradationBottomColor, LinearGradientMode.Vertical);
				Pen p = new Pen(br縁, nEdgePt);
				p.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
				g.DrawPath(p, gp);

				// 塗りつぶす
				Brush br;
				br = new SolidBrush(fontColor);
				g.FillPath(br, gp);

				if (br != null) br.Dispose(); br = null;
				if (p != null) p.Dispose(); p = null;
				if (gp != null) gp.Dispose(); gp = null;
			}
			else
			{
				// 縁取りなしの描画
				System.Windows.Forms.TextRenderer.DrawText(g, drawstr, _font, new Point(0, 0), fontColor);
			}
#if debug表示
			g.DrawRectangle( new Pen( Color.White, 1 ), new Rectangle( 1, 1, stringSize.Width-1, stringSize.Height-1 ) );
			g.DrawRectangle( new Pen( Color.Green, 1 ), new Rectangle( 0, 0, bmp.Width - 1, bmp.Height - 1 ) );
#endif
			_rectStrings = new Rectangle(0, 0, stringSize.Width, stringSize.Height);
			_ptOrigin = new Point(nEdgePt * 2, nEdgePt * 2);


			#region [ リソースを解放する ]
			if (sf != null) sf.Dispose(); sf = null;
			if (g != null) g.Dispose(); g = null;
			#endregion

			return bmp;
		}

		/// <summary>
		/// 最後にDrawPrivateFont()した文字列の描画領域を取得します。
		/// </summary>
		public Rectangle RectStrings
		{
			get
			{
				return _rectStrings;
			}
			protected set
			{
				_rectStrings = value;
			}
		}
		public Point PtOrigin
		{
			get
			{
				return _ptOrigin;
			}
			protected set
			{
				_ptOrigin = value;
			}
		}

		#region [ IDisposable 実装 ]
		//-----------------
		public void Dispose()
		{
			if (!this.bDispose完了済み)
			{
				if (this._font != null)
				{
					this._font.Dispose();
					this._font = null;
				}
				if (this._pfc != null)
				{
					this._pfc.Dispose();
					this._pfc = null;
				}

				this.bDispose完了済み = true;
			}
		}
		//-----------------
		#endregion

		#region [ private ]
		//-----------------
		protected bool bDispose完了済み;
		protected Font _font;

		private System.Drawing.Text.PrivateFontCollection _pfc;
		private FontFamily _fontfamily;
		private int _pt;
		private Rectangle _rectStrings;
		private Point _ptOrigin;
		//-----------------
		#endregion
	}
}
