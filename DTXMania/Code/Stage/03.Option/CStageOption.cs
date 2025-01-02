// using System;
// using System.Collections.Generic;
// using System.Text;
// using System.Globalization;
// using System.Runtime.InteropServices;
// using System.Drawing;
// using System.Diagnostics;
// using FDK;
//
// using SlimDXKey = SlimDX.DirectInput.Key;
//
// namespace DTXMania
// {
// 	internal class CStageOption : CStage
// 	{
// 		// プロパティ
//
// 		public CActDFPFont actFont { get; private set; }
//
//
// 		// コンストラクタ
//
// 		public CStageOption()
// 		{
// 			CActDFPFont font;
// 			base.eStageID = CStage.EStage.Option;
// 			base.ePhaseID = CStage.EPhase.Common_DefaultState;
// 			this.actFont = font = new CActDFPFont();
// 			base.listChildActivities.Add( font );
// 			base.listChildActivities.Add( this.actFIFO = new CActFIFOWhite() );
// 			base.listChildActivities.Add( this.actList = new CActConfigList() );
// 			base.listChildActivities.Add( this.actKeyAssign = new CActConfigKeyAssign() );
// 			//base.listChildActivities.Add( this.actオプションパネル = new CActOptionPanel() );
// 			base.bNotActivated = true;
// 		}
// 		
// 		
// 		// メソッド
//
// 		public void tアサイン完了通知()															// CONFIGにのみ存在
// 		{																						//
// 			this.eItemPanelMove = EItemPanelMode.PadList;								//
// 		}																						//
// 		public void tパッド選択通知( EKeyConfigPart part, EKeyConfigPad pad )							//
// 		{																						//
// 			this.actKeyAssign.tStart( part, pad, this.actList.ibCurrentSelection.strItemName );		//
// 			this.eItemPanelMove = EItemPanelMode.KeyCodeList;							//
// 		}																						//
// 		public void t項目変更通知()																// OPTIONと共通
// 		{																						//
// 			this.tDrawSelectedItemDescriptionInDescriptionPanel();						//
// 		}																						//
//
// 		
// 		// CStage 実装
//
// 		public override void OnActivate()
// 		{
// 			Trace.TraceInformation( "オプションステージを活性化します。" );
// 			Trace.Indent();
// 			try
// 			{
// 				this.nCurrentMenuNumber = 0;													//
// 				this.ftFont = new Font( "MS PGothic", 26f, GraphicsUnit.Pixel );			//
// 				for( int i = 0; i < 4; i++ )													//
// 				{																				//
// 					this.ctKeyRepetition[ i ] = new CCounter( 0, 0, 0, CDTXMania.Timer );			//
// 				}																				//
// 				this.bFocusIsOnMenu = true;											// ここまでOPTIONと共通
// 				this.eItemPanelMove = EItemPanelMode.PadList;
// 			}
// 			finally
// 			{
// 				Trace.TraceInformation( "オプションステージの活性化を完了しました。" );
// 				Trace.Unindent();
// 			}
// 			base.OnActivate();		// 2011.3.14 yyagi: OnActivate()をtryの中から外に移動
// 		}
// 		public override void OnDeactivate()
// 		{
// 			Trace.TraceInformation( "オプションステージを非活性化します。" );
// 			Trace.Indent();
// 			try
// 			{
// 				CDTXMania.ConfigIni.tWrite( CDTXMania.strEXEのあるフォルダ + "Config.ini" );	// CONFIGだけ
// 				if( this.ftFont != null )													// 以下OPTIONと共通
// 				{
// 					this.ftFont.Dispose();
// 					this.ftFont = null;
// 				}
// 				for( int i = 0; i < 4; i++ )
// 				{
// 					this.ctKeyRepetition[ i ] = null;
// 				}
// 				base.OnDeactivate();
// 			}
// 			finally
// 			{
// 				Trace.TraceInformation( "オプションステージの非活性化を完了しました。" );
// 				Trace.Unindent();
// 			}
// 		}
// 		public override void OnManagedCreateResources()											// OPTIONと画像以外共通
// 		{
// 			if( !base.bNotActivated )
// 			{
// 				this.txBackground = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\4_background.jpg" ), false );
// 				this.txTopPanel = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\4_header panel.png" ), true );
// 				this.txBottomPanel = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\4_footer panel.png" ), true );
// 				this.txMenuCursor = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\4_menu cursor.png" ), false );
// 				if( this.bFocusIsOnMenu )
// 				{
// 					this.tDrawSelectedMenuDescriptionInDescriptionPanel();
// 				}
// 				else
// 				{
// 					this.tDrawSelectedItemDescriptionInDescriptionPanel();
// 				}
// 				base.OnManagedCreateResources();
// 			}
// 		}
// 		public override void OnManagedReleaseResources()											// OPTIONと同じ(COnfig.iniの書き出しタイミングのみ異なるが、無視して良い)
// 		{
// 			if( !base.bNotActivated )
// 			{
// 				CDTXMania.tReleaseTexture( ref this.txBackground );
// 				CDTXMania.tReleaseTexture( ref this.txTopPanel );
// 				CDTXMania.tReleaseTexture( ref this.txBottomPanel );
// 				CDTXMania.tReleaseTexture( ref this.txMenuCursor );
// 				CDTXMania.tReleaseTexture( ref this.txDescriptionPanel );
// 				base.OnManagedReleaseResources();
// 			}
// 		}
// 		public override int OnUpdateAndDraw()
// 		{
// 			if( base.bNotActivated )
// 				return 0;
//
// 			if( base.bJustStartedUpdate )
// 			{
// 				base.ePhaseID = CStage.EPhase.Common_FadeIn;
// 				this.actFIFO.tフェードイン開始();
// 				base.bJustStartedUpdate = false;
// 			}
//
// 			// 描画
//
// 			#region [ 背景 ]
// 			//---------------------
// 			if( this.txBackground != null )
// 				this.txBackground.tDraw2D( CDTXMania.app.Device, 0, 0 );
// 			//---------------------
// 			#endregion
// 			#region [ メニューカーソル ]
// 			//---------------------
// 			if( this.txMenuCursor != null )
// 			{
// 				Rectangle rectangle;
// 				this.txMenuCursor.nTransparency = this.bFocusIsOnMenu ? 0xff : 0x80;
// 				int x = 111;
// 				int y = 144 + ( this.nCurrentMenuNumber * 38 );
// 				int num3 = 340;
// 				this.txMenuCursor.tDraw2D( CDTXMania.app.Device, x, y, new Rectangle( 0, 0, 0x20, 0x30 ) );
// 				this.txMenuCursor.tDraw2D( CDTXMania.app.Device, ( x + num3 ) - 0x20, y, new Rectangle( 20, 0, 0x20, 0x30 ) );
// 				x += 0x20;
// 				for( num3 -= 0x40; num3 > 0; num3 -= rectangle.Width )
// 				{
// 					rectangle = new Rectangle( 0x10, 0, 0x20, 0x30 );
// 					if( num3 < 0x20 )
// 					{
// 						rectangle.Width -= 0x20 - num3;
// 					}
// 					this.txMenuCursor.tDraw2D( CDTXMania.app.Device, x, y, rectangle );
// 					x += rectangle.Width;
// 				}
// 			}
// 			//---------------------
// 			#endregion
// 			#region [ メニュー ]
// 			//---------------------
// 			string str = "System";
// 			int num4 = this.actFont.n文字列長dot( str );
// 			bool flag = this.nCurrentMenuNumber == 0;
// 			this.actFont.t文字列描画( 0x11a - ( num4 / 2 ), 0x9b, str, flag );
// 			str = "Drums";
// 			num4 = this.actFont.n文字列長dot( str );
// 			flag = this.nCurrentMenuNumber == 1;
// 			this.actFont.t文字列描画( 0x11a - ( num4 / 2 ), 0xc0, str, flag );
// 			str = "Guitar";
// 			num4 = this.actFont.n文字列長dot( str );
// 			flag = this.nCurrentMenuNumber == 2;
// 			this.actFont.t文字列描画( 0x11a - ( num4 / 2 ), 230, str, flag );
// 			str = "Bass";
// 			num4 = this.actFont.n文字列長dot( str );
// 			flag = this.nCurrentMenuNumber == 3;
// 			this.actFont.t文字列描画( 0x11a - ( num4 / 2 ), 0x10b, str, flag );
// 			str = "Exit";
// 			num4 = this.actFont.n文字列長dot( str );
// 			flag = this.nCurrentMenuNumber == 4;
// 			this.actFont.t文字列描画( 0x11a - ( num4 / 2 ), 0x130, str, flag );
//
// 			//---------------------
// 			#endregion
// 			#region [ 説明文パネル ]
// 			//---------------------
// 			if( this.txDescriptionPanel != null )
// 				this.txDescriptionPanel.tDraw2D( CDTXMania.app.Device, 0x43, 0x17e );
// 			//---------------------
// 			#endregion
// 			#region [ アイテム ]
// 			//---------------------
// 			switch( this.eItemPanelMove )
// 			{
// 				case EItemPanelMode.PadList:
// 					this.actList.tUpdateAndDraw( !this.bFocusIsOnMenu );
// 					break;
//
// 				case EItemPanelMode.KeyCodeList:
// 					this.actKeyAssign.OnUpdateAndDraw();
// 					break;
// 			}
// 			//---------------------
// 			#endregion
// 			#region [ 上部パネル ]
// 			//---------------------
// 			if( this.txTopPanel != null )
// 				this.txTopPanel.tDraw2D( CDTXMania.app.Device, 0, 0 );
// 			//---------------------
// 			#endregion
// 			#region [ 下部パネル ]
// 			//---------------------
// 			if( this.txBottomPanel != null )
// 				this.txBottomPanel.tDraw2D( CDTXMania.app.Device, 0, 720 - this.txBottomPanel.szTextureSize.Height );
// 			//---------------------
// 			#endregion
// 			#region [ オプションパネル ]
// 			//---------------------
// 			//this.actオプションパネル.OnUpdateAndDraw();
// 			//---------------------
// 			#endregion
// 			#region [ フェードイン_アウト ]
// 			//---------------------
// 			switch( base.ePhaseID )
// 			{
// 				case CStage.EPhase.Common_FadeIn:
// 					if( this.actFIFO.OnUpdateAndDraw() != 0 )
// 					{
// 						CDTXMania.Skin.bgmコンフィグ画面.tPlay();
// 						base.ePhaseID = CStage.EPhase.Common_DefaultState;
// 					}
// 					break;
//
// 				case CStage.EPhase.Common_FadeOut:
// 					if( this.actFIFO.OnUpdateAndDraw() == 0 )
// 					{
// 						break;
// 					}
// 					return 1;
// 			}
// 			//---------------------
// 			#endregion
//
// 			// キー入力
//
// 			if( ( base.ePhaseID != CStage.EPhase.Common_DefaultState )
// 				|| this.actKeyAssign.bキー入力待ちの最中である
// 				|| CDTXMania.actPluginOccupyingInput != null )
// 				return 0;
//
// 			if( ( CDTXMania.InputManager.Keyboard.bKeyPressed( (int)SlimDXKey.Escape ) || CDTXMania.Pad.bPressed( EInstrumentPart.DRUMS, EPad.LC ) ) || CDTXMania.Pad.bPressedGB( EPad.Cancel) )
// 			{
// 				CDTXMania.Skin.soundCancel.tPlay();
// 				if( !this.bFocusIsOnMenu )
// 				{
// 					if( this.eItemPanelMove == EItemPanelMode.KeyCodeList )
// 					{
// 						CDTXMania.stageConfig.tNotifyAssignmentComplete();
// 						return 0;
// 					}
// 					if ( this.actList.bIsKeyAssignSelected == false )		// #24525 2011.3.15 yyagi
// 					{
// 						this.bFocusIsOnMenu = true;
// 					}
// 					this.tDrawSelectedMenuDescriptionInDescriptionPanel();
// 					this.actList.tPressEsc();								// #24525 2011.3.15 yyagi ESC押下時の右メニュー描画用
// 				}
// 				else
// 				{
// 					this.actFIFO.tStartFadeOut();
// 					base.ePhaseID = CStage.EPhase.Common_FadeOut;
// 				}
// 			}
// 			else if( ( CDTXMania.Pad.bPressedDGB( EPad.CY ) || CDTXMania.Pad.bPressed( EInstrumentPart.DRUMS, EPad.RD ) ) || ( CDTXMania.Pad.bPressed( EInstrumentPart.DRUMS, EPad.LC ) || ( CDTXMania.ConfigIni.bEnterがキー割り当てのどこにも使用されていない && CDTXMania.InputManager.Keyboard.bKeyPressed( (int)SlimDXKey.Return) ) ) )
// 			{
// 				if( this.nCurrentMenuNumber == 4 )
// 				{
// 					CDTXMania.Skin.soundDecideSong.tPlay();
// 					this.actFIFO.tStartFadeOut();
// 					base.ePhaseID = CStage.EPhase.Common_FadeOut;
// 				}
// 				else if( this.bFocusIsOnMenu )
// 				{
// 					CDTXMania.Skin.soundDecide.tPlay();
// 					this.bFocusIsOnMenu = false;
// 					this.tDrawSelectedItemDescriptionInDescriptionPanel();
// 				}
// 				else
// 				{
// 					switch( this.eItemPanelMove )
// 					{
// 						case EItemPanelMode.PadList:
// 							bool bIsKeyAssignSelectedBeforeHitEnter = this.actList.bIsKeyAssignSelected;	// #24525 2011.3.15 yyagi
// 							this.actList.tPressEnter();
// 							if( this.actList.b現在選択されている項目はReturnToMenuである )
// 							{
// 								this.tDrawSelectedMenuDescriptionInDescriptionPanel();
// 								if ( bIsKeyAssignSelectedBeforeHitEnter == false )							// #24525 2011.3.15 yyagi
// 								{
// 									this.bFocusIsOnMenu = true;
// 								}
// 							}
// 							break;
//
// 						case EItemPanelMode.KeyCodeList:
// 							this.actKeyAssign.tPressEnter();
// 							break;
// 					}
// 				}
// 			}
// 			this.ctKeyRepetition.Up.tRepeatKey( CDTXMania.InputManager.Keyboard.bKeyPressing( (int) SlimDXKey.UpArrow ), new CCounter.DGキー処理( this.tMoveCursorUp ) );
// 			this.ctKeyRepetition.R.tRepeatKey( CDTXMania.Pad.b押されているGB( EPad.HH ), new CCounter.DGキー処理( this.tMoveCursorUp ) );
// 			//Change to HT
// 			if( CDTXMania.Pad.bPressed( EInstrumentPart.DRUMS, EPad.HT ) )
// 			{
// 				this.tMoveCursorUp();
// 			}
// 			this.ctKeyRepetition.Down.tRepeatKey( CDTXMania.InputManager.Keyboard.bKeyPressing( (int) SlimDXKey.DownArrow ), new CCounter.DGキー処理( this.tMoveCursourDown ) );
// 			this.ctKeyRepetition.B.tRepeatKey( CDTXMania.Pad.b押されているGB( EPad.BD ), new CCounter.DGキー処理( this.tMoveCursourDown ) );
// 			//Change to LT
// 			if( CDTXMania.Pad.bPressed( EInstrumentPart.DRUMS, EPad.LT ) )
// 			{
// 				this.tMoveCursourDown();
// 			}
// 			return 0;
// 		}
//
//
// 		// Other
//
// 		#region [ private ]
// 		//-----------------
// 		private enum EItemPanelMode
// 		{
// 			PadList,
// 			KeyCodeList
// 		}
//
// 		[StructLayout( LayoutKind.Sequential )]
// 		private struct STKeyRepetitionCounter
// 		{
// 			public CCounter Up;
// 			public CCounter Down;
// 			public CCounter R;
// 			public CCounter B;
// 			public CCounter this[ int index ]
// 			{
// 				get
// 				{
// 					switch( index )
// 					{
// 						case 0:
// 							return this.Up;
//
// 						case 1:
// 							return this.Down;
//
// 						case 2:
// 							return this.R;
//
// 						case 3:
// 							return this.B;
// 					}
// 					throw new IndexOutOfRangeException();
// 				}
// 				set
// 				{
// 					switch( index )
// 					{
// 						case 0:
// 							this.Up = value;
// 							return;
//
// 						case 1:
// 							this.Down = value;
// 							return;
//
// 						case 2:
// 							this.R = value;
// 							return;
//
// 						case 3:
// 							this.B = value;
// 							return;
// 					}
// 					throw new IndexOutOfRangeException();
// 				}
// 			}
// 		}
//
// 		private CActFIFOWhite actFIFO;
// 		private CActConfigKeyAssign actKeyAssign;
// 		private CActConfigList actList;
// 		private bool bFocusIsOnMenu;
// 		private STKeyRepetitionCounter ctKeyRepetition;
// 		private const int DESC_H = 0x80;
// 		private const int DESC_W = 220;
// 		private EItemPanelMode eItemPanelMove;
// 		private Font ftFont;
// 		private int nCurrentMenuNumber;
// 		private CTexture txMenuCursor;
// 		private CTexture txBottomPanel;
// 		private CTexture txTopPanel;
// 		private CTexture txDescriptionPanel;
// 		private CTexture txBackground;
//
// 		private void tMoveCursourDown()
// 		{
// 			if( !this.bFocusIsOnMenu )
// 			{
// 				switch( this.eItemPanelMove )
// 				{
// 					case EItemPanelMode.PadList:
// 						this.actList.tMoveToPrevious();
// 						return;
//
// 					case EItemPanelMode.KeyCodeList:
// 						this.actKeyAssign.tMoveToNext();
// 						return;
// 				}
// 			}
// 			else
// 			{
// 				CDTXMania.Skin.soundCursorMovement.tPlay();
// 				this.nCurrentMenuNumber = ( this.nCurrentMenuNumber + 1 ) % 5;
// 				switch( this.nCurrentMenuNumber )
// 				{
// 					case 0:
// 						this.actList.tSetupItemList_System();
// 						break;
//
// 					//case 1:
// 					//    this.actList.t項目リストの設定_KeyAssignDrums();
// 					//    break;
//
// 					//case 2:
// 					//    this.actList.t項目リストの設定_KeyAssignGuitar();
// 					//    break;
//
// 					//case 3:
// 					//    this.actList.t項目リストの設定_KeyAssignBass();
// 					//    break;
//
// 					case 1:
// 						this.actList.tSetupItemList_Drums();
// 						break;
//
// 					case 2:
// 						this.actList.tSetupItemList_Guitar();
// 						break;
//
// 					case 3:
// 						this.actList.tSetupItemList_Bass();
// 						break;
//
// 					case 4:
// 						this.actList.tSetupItemList_Exit();
// 						break;
// 				}
// 				this.tDrawSelectedMenuDescriptionInDescriptionPanel();
// 			}
// 		}
// 		private void tMoveCursorUp()
// 		{
// 			if( !this.bFocusIsOnMenu )
// 			{
// 				switch( this.eItemPanelMove )
// 				{
// 					case EItemPanelMode.PadList:
// 						this.actList.tMoveToNext();
// 						return;
//
// 					case EItemPanelMode.KeyCodeList:
// 						this.actKeyAssign.tMoveToPrevious();
// 						return;
// 				}
// 			}
// 			else
// 			{
// 				CDTXMania.Skin.soundCursorMovement.tPlay();
// 				this.nCurrentMenuNumber = ( ( this.nCurrentMenuNumber - 1 ) + 5 ) % 5;
// 				switch( this.nCurrentMenuNumber )
// 				{
// 					case 0:
// 						this.actList.tSetupItemList_System();
// 						break;
//
// 					//case 1:
// 					//    this.actList.t項目リストの設定_KeyAssignDrums();
// 					//    break;
//
// 					//case 2:
// 					//    this.actList.t項目リストの設定_KeyAssignGuitar();
// 					//    break;
//
// 					//case 3:
// 					//    this.actList.t項目リストの設定_KeyAssignBass();
// 					//    break;
// 					case 1:
// 						this.actList.tSetupItemList_Drums();
// 						break;
//
// 					case 2:
// 						this.actList.tSetupItemList_Guitar();
// 						break;
//
// 					case 3:
// 						this.actList.tSetupItemList_Bass();
// 						break;
//
// 					case 4:
// 						this.actList.tSetupItemList_Exit();
// 						break;
// 				}
// 				this.tDrawSelectedMenuDescriptionInDescriptionPanel();
// 			}
// 		}
// 		private void tDrawSelectedMenuDescriptionInDescriptionPanel()
// 		{
// 			try
// 			{
// 				var image = new Bitmap( 220 * 2, 192 * 2 );		// 説明文領域サイズの縦横 2 倍。（描画時に 0.5 倍で表示する。）
// 				var graphics = Graphics.FromImage( image );
// 				graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
// 				
// 				string[,] str = new string[ 2, 2 ];
// 				switch( this.nCurrentMenuNumber )
// 				{
// 					case 0:
// 						str[ 0, 0 ] = "システムに関係する項目を設定しま";
// 						str[ 0, 1 ] = "す。";
// 						str[ 1, 0 ] = "Settings for an overall systems.";
// 						break;
//
// 					//case 1:
// 					//    str[0, 0] = "ドラムのキー入力に関する項目を設";
// 					//    str[0, 1] = "定します。";
// 					//    str[1, 0] = "Settings for the drums key/pad inputs.";
// 					//    str[1, 1] = "";
// 					//    break;
//
// 					//case 2:
// 					//    str[0, 0] = "ギターのキー入力に関する項目を設";
// 					//    str[0, 1] = "定します。";
// 					//    str[1, 0] = "Settings for the guitar key/pad inputs.";
// 					//    str[1, 1] = "";
// 					//    break;
//
// 					//case 3:
// 					//    str[0, 0] = "ベースのキー入力に関する項目を設";
// 					//    str[0, 1] = "定します。";
// 					//    str[1, 0] = "Settings for the bass key/pad inputs.";
// 					//    str[1, 1] = "";
// 					//    break;
// 					case 1:
// 						str[ 0, 0 ] = "ドラムの演奏に関する項目を設定し";
// 						str[ 0, 1 ] = "ます。";
// 						str[ 1, 0 ] = "Settings to play the drums.";
// 						str[ 1, 1 ] = "";
// 						break;
//
// 					case 2:
// 						str[ 0, 0 ] = "ギターの演奏に関する項目を設定し";
// 						str[ 0, 1 ] = "ます。";
// 						str[ 1, 0 ] = "Settings to play the guitar.";
// 						str[ 1, 1 ] = "";
// 						break;
//
// 					case 3:
// 						str[ 0, 0 ] = "ベースの演奏に関する項目を設定し";
// 						str[ 0, 1 ] = "ます。";
// 						str[ 1, 0 ] = "Settings to play the bass.";
// 						str[ 1, 1 ] = "";
// 						break;
//
// 					case 4:
// 						str[ 0, 0 ] = "設定を保存し、コンフィグ画面を終了";
// 						str[ 0, 1 ] = "します。";
// 						str[ 1, 0 ] = "Save the settings and exit from";
// 						str[ 1, 1 ] = "CONFIGURATION menu.";
// 						break;
// 				}
// 				
// 				int c = CDTXMania.isJapanese ? 0 : 1;
// 				for (int i = 0; i < 2; i++)
// 				{
// 					graphics.DrawString( str[ c, i ], this.ftFont, Brushes.White, new PointF( 4f, ( i * 30 ) ) );
// 				}
// 				graphics.Dispose();
// 				if( this.txDescriptionPanel != null )
// 				{
// 					this.txDescriptionPanel.Dispose();
// 				}
// 				this.txDescriptionPanel = new CTexture( CDTXMania.app.Device, image, CDTXMania.TextureFormat );
// 				this.txDescriptionPanel.vcScaleRatio.X = 0.8f;
// 				this.txDescriptionPanel.vcScaleRatio.Y = 0.8f;
// 				image.Dispose();
// 			}
// 			catch( CTextureCreateFailedException )
// 			{
// 				Trace.TraceError( "説明文テクスチャの作成に失敗しました。" );
// 				this.txDescriptionPanel = null;
// 			}
// 		}
// 		private void tDrawSelectedItemDescriptionInDescriptionPanel()
// 		{
// 			try
// 			{
// 				var image = new Bitmap( 440, 0x100 );		// 説明文領域サイズの縦横 2 倍。（描画時に 0.5 倍で表示する。）
// 				var graphics = Graphics.FromImage( image );
// 				graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
//
// 				CItemBase item = this.actList.ibCurrentSelection;
// 				if( ( item.str説明文 != null ) && ( item.str説明文.Length > 0 ) )
// 				{
// 					int num = 0;
// 					foreach( string str in item.str説明文.Split( new char[] { '\n' } ) )
// 					{
// 						graphics.DrawString( str, this.ftFont, Brushes.White, new PointF( 4f, (float) num ) );
// 						num += 30;
// 					}
// 				}
// 				graphics.Dispose();
// 				if( this.txDescriptionPanel != null )
// 				{
// 					this.txDescriptionPanel.Dispose();
// 				}
// 				this.txDescriptionPanel = new CTexture( CDTXMania.app.Device, image, CDTXMania.TextureFormat );
// 				this.txDescriptionPanel.vcScaleRatio.X = 0.8f;
// 				this.txDescriptionPanel.vcScaleRatio.Y = 0.8f;
// 				image.Dispose();
// 			}
// 			catch( CTextureCreateFailedException )
// 			{
// 				Trace.TraceError( "説明文パネルテクスチャの作成に失敗しました。" );
// 				this.txDescriptionPanel = null;
// 			}
// 		}
// 		//-----------------
// 		#endregion
// 	}
// }
