using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using FDK;

using SlimDXKey = SlimDX.DirectInput.Key;

namespace DTXMania
{
	internal class CStageTitle : CStage
	{		
		// コンストラクタ

		public CStageTitle()
		{
			base.eStageID = CStage.EStage.Title;
			base.bNotActivated = true;
			base.listChildActivities.Add( this.actFIfromSetup = new CActFIFOWhite() );
			base.listChildActivities.Add( this.actFI = new CActFIFOWhite() );
			base.listChildActivities.Add( this.actFO = new CActFIFOWhite() );
		}


		// CStage 実装

		public override void OnActivate()
		{
			Trace.TraceInformation( "タイトルステージを活性化します。" );
			Trace.Indent();
			try
			{
				for( int i = 0; i < 4; i++ )
				{
					this.ctキー反復用[ i ] = new CCounter( 0, 0, 0, CDTXMania.Timer );
				}
				this.ct上移動用 = new CCounter();
				this.ct下移動用 = new CCounter();
				this.ctカーソルフラッシュ用 = new CCounter();
				base.OnActivate();
			}
			finally
			{
				Trace.TraceInformation( "タイトルステージの活性化を完了しました。" );
				Trace.Unindent();
			}
		}
		public override void OnDeactivate()
		{
			Trace.TraceInformation( "タイトルステージを非活性化します。" );
			Trace.Indent();
			try
			{
				for( int i = 0; i < 4; i++ )
				{
					this.ctキー反復用[ i ] = null;
				}
				this.ct上移動用 = null;
				this.ct下移動用 = null;
				this.ctカーソルフラッシュ用 = null;
			}
			finally
			{
				Trace.TraceInformation( "タイトルステージの非活性化を完了しました。" );
				Trace.Unindent();
			}
			base.OnDeactivate();
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
				this.txBackground = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\2_background.jpg" ), false );
				this.txMenu = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\2_menu.png" ), false );
				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if( !base.bNotActivated )
			{
				CDTXMania.tReleaseTexture( ref this.txBackground );
				CDTXMania.tReleaseTexture( ref this.txMenu );
				base.OnManagedReleaseResources();
			}
		}
		public override int OnUpdateAndDraw()
		{
			if( !base.bNotActivated )
			{
				#region [ 初めての進行描画 ]
				//---------------------
				if( base.bJustStartedUpdate )
				{
					if( CDTXMania.rPreviousStage == CDTXMania.stageStartup )
					{
						this.actFIfromSetup.tStartFadeIn();
						base.ePhaseID = CStage.EPhase.タイトル_起動画面からのフェードイン;
					}
					else
					{
						this.actFI.tStartFadeIn();
						base.ePhaseID = CStage.EPhase.Common_FadeIn;
					}
					this.ctカーソルフラッシュ用.tStart( 0, 700, 5, CDTXMania.Timer );
					this.ctカーソルフラッシュ用.nCurrentValue = 100;
					base.bJustStartedUpdate = false;
				}
				//---------------------
				#endregion

				// 進行

				#region [ カーソル上移動 ]
				//---------------------
				if( this.ct上移動用.bInProgress )
				{
					this.ct上移動用.tUpdate();
					if( this.ct上移動用.bReachedEndValue )
					{
						this.ct上移動用.tStop();
					}
				}
				//---------------------
				#endregion
				#region [ カーソル下移動 ]
				//---------------------
				if( this.ct下移動用.bInProgress )
				{
					this.ct下移動用.tUpdate();
					if( this.ct下移動用.bReachedEndValue )
					{
						this.ct下移動用.tStop();
					}
				}
				//---------------------
				#endregion
				#region [ カーソルフラッシュ ]
				//---------------------
				this.ctカーソルフラッシュ用.tUpdateLoop();
				//---------------------
				#endregion

				// キー入力

				if( base.ePhaseID == CStage.EPhase.Common_DefaultState		// 通常状態、かつ
					&& CDTXMania.actPluginOccupyingInput == null )	// プラグインの入力占有がない
				{
					if( CDTXMania.InputManager.Keyboard.bKeyPressed( (int) SlimDXKey.Escape ) )
						return (int) E戻り値.EXIT;

					this.ctキー反復用.Up.tRepeatKey( CDTXMania.InputManager.Keyboard.bKeyPressing( (int)SlimDXKey.UpArrow ), new CCounter.DGキー処理( this.tMoveCursorUp ) );
					this.ctキー反復用.R.tRepeatKey( CDTXMania.Pad.b押されているGB( EPad.HH ), new CCounter.DGキー処理( this.tMoveCursorUp ) );
					//Change to HT
					if( CDTXMania.Pad.bPressed( EInstrumentPart.DRUMS, EPad.HT ) )
						this.tMoveCursorUp();

					this.ctキー反復用.Down.tRepeatKey( CDTXMania.InputManager.Keyboard.bKeyPressing( (int)SlimDXKey.DownArrow ), new CCounter.DGキー処理( this.tMoveCursorDown ) );
					this.ctキー反復用.B.tRepeatKey( CDTXMania.Pad.b押されているGB( EPad.SD ), new CCounter.DGキー処理( this.tMoveCursorDown ) );
					//Change to LT
					if ( CDTXMania.Pad.bPressed( EInstrumentPart.DRUMS, EPad.LT ) )
						this.tMoveCursorDown();

					if( ( CDTXMania.Pad.bPressedDGB( EPad.CY ) || CDTXMania.Pad.bPressed( EInstrumentPart.DRUMS, EPad.RD ) ) || ( CDTXMania.ConfigIni.bEnterがキー割り当てのどこにも使用されていない && CDTXMania.InputManager.Keyboard.bKeyPressed( (int)SlimDXKey.Return ) ) ) 
					{
						if ( ( this.n現在のカーソル行 == (int) E戻り値.GAMESTART - 1 ) && CDTXMania.Skin.soundGameStart.b読み込み成功 )
						{
							CDTXMania.Skin.soundGameStart.tPlay();
						}
						else
						{
							CDTXMania.Skin.soundDecide.tPlay();
						}
						if( this.n現在のカーソル行 == (int)E戻り値.EXIT - 1 )
						{
							return (int)E戻り値.EXIT;
						}
						this.actFO.tStartFadeOut();
						base.ePhaseID = CStage.EPhase.Common_FadeOut;
					}
				}

				// DrawBackground

				if( this.txBackground != null )
					this.txBackground.tDraw2D( CDTXMania.app.Device, 0, 0 );

                CDTXMania.actDisplayString.tPrint( 2, 2, CCharacterConsole.EFontType.White, CDTXMania.VERSION_DISPLAY);

				if( this.txMenu != null )
				{
					int x = MENU_X;
					int y = MENU_Y + ( this.n現在のカーソル行 * MENU_H );
					if( this.ct上移動用.bInProgress )
					{
						y += (int) ( (double)MENU_H / 2 * ( Math.Cos( Math.PI * ( ( (double) this.ct上移動用.nCurrentValue ) / 100.0 ) ) + 1.0 ) );
					}
					else if( this.ct下移動用.bInProgress )
					{
						y -= (int) ( (double)MENU_H / 2 * ( Math.Cos( Math.PI * ( ( (double) this.ct下移動用.nCurrentValue ) / 100.0 ) ) + 1.0 ) );
					}
					if( this.ctカーソルフラッシュ用.nCurrentValue <= 100 )
					{
						float nMag = (float) ( 1.0 + ( ( ( (double) this.ctカーソルフラッシュ用.nCurrentValue ) / 100.0 ) * 0.5 ) );
						this.txMenu.vcScaleRatio.X = nMag;
						this.txMenu.vcScaleRatio.Y = nMag;
						this.txMenu.nTransparency = (int) ( 255.0 * ( 1.0 - ( ( (double) this.ctカーソルフラッシュ用.nCurrentValue ) / 100.0 ) ) );
						int x_magnified = x + ( (int) ( ( MENU_W * ( 1.0 - nMag ) ) / 2.0 ) );
						int y_magnified = y + ( (int) ( ( MENU_H * ( 1.0 - nMag ) ) / 2.0 ) );
						this.txMenu.tDraw2D( CDTXMania.app.Device, x_magnified, y_magnified, new Rectangle( 0, MENU_H * 5, MENU_W, MENU_H ) );
					}
					this.txMenu.vcScaleRatio.X = 1f;
					this.txMenu.vcScaleRatio.Y = 1f;
					this.txMenu.nTransparency = 0xff;
					this.txMenu.tDraw2D( CDTXMania.app.Device, x, y, new Rectangle( 0, MENU_H * 4, MENU_W, MENU_H ) );
				}
				if( this.txMenu != null )
				{
					this.txMenu.tDraw2D( CDTXMania.app.Device, MENU_X, MENU_Y, new Rectangle( 0, 0, MENU_W, MENU_H ) );
					this.txMenu.tDraw2D( CDTXMania.app.Device, MENU_X, MENU_Y + MENU_H, new Rectangle( 0, MENU_H * 2, MENU_W, MENU_H * 2 ) );
				}
				CStage.EPhase eフェーズid = base.ePhaseID;
				switch( eフェーズid )
				{
					case CStage.EPhase.Common_FadeIn:
						if( this.actFI.OnUpdateAndDraw() != 0 )
						{
							CDTXMania.Skin.soundTitle.tPlay();
							base.ePhaseID = CStage.EPhase.Common_DefaultState;
						}
						break;

					case CStage.EPhase.Common_FadeOut:
						if( this.actFO.OnUpdateAndDraw() == 0 )
						{
							break;
						}
						base.ePhaseID = CStage.EPhase.Common_EndStatus;
						switch ( this.n現在のカーソル行 )
						{
							case (int)E戻り値.GAMESTART - 1:
								return (int)E戻り値.GAMESTART;

							case (int) E戻り値.CONFIG - 1:
								return (int) E戻り値.CONFIG;

							case (int)E戻り値.EXIT - 1:
								return (int) E戻り値.EXIT;
								//return ( this.n現在のカーソル行 + 1 );
						}
						break;

					case CStage.EPhase.タイトル_起動画面からのフェードイン:
						if( this.actFIfromSetup.OnUpdateAndDraw() != 0 )
						{
							CDTXMania.Skin.soundTitle.tPlay();
							base.ePhaseID = CStage.EPhase.Common_DefaultState;
						}
						break;
				}
			}
			return 0;
		}
		public enum E戻り値
		{
			継続 = 0,
			GAMESTART,
//			OPTION,
			CONFIG,
			EXIT
		}


		// Other

		#region [ private ]
		//-----------------
		[StructLayout( LayoutKind.Sequential )]
		private struct STキー反復用カウンタ
		{
			public CCounter Up;
			public CCounter Down;
			public CCounter R;
			public CCounter B;
			public CCounter this[ int index ]
			{
				get
				{
					switch( index )
					{
						case 0:
							return this.Up;

						case 1:
							return this.Down;

						case 2:
							return this.R;

						case 3:
							return this.B;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch( index )
					{
						case 0:
							this.Up = value;
							return;

						case 1:
							this.Down = value;
							return;

						case 2:
							this.R = value;
							return;

						case 3:
							this.B = value;
							return;
					}
					throw new IndexOutOfRangeException();
				}
			}
		}

		private CActFIFOWhite actFI;
		private CActFIFOWhite actFIfromSetup;
		private CActFIFOWhite actFO;
		private CCounter ctカーソルフラッシュ用;
		private STキー反復用カウンタ ctキー反復用;
		private CCounter ct下移動用;
		private CCounter ct上移動用;
        private const int MENU_H = 0x27;
        private const int MENU_W = 0xe3;
        private const int MENU_X = 0x1fa;
        private const int MENU_Y = 0x201;
		private int n現在のカーソル行;
		private CTexture txMenu;
		private CTexture txBackground;
	
		private void tMoveCursorDown()
		{
			if ( this.n現在のカーソル行 != (int) E戻り値.EXIT - 1 )
			{
				CDTXMania.Skin.soundCursorMovement.tPlay();
				this.n現在のカーソル行++;
				this.ct下移動用.tStart( 0, 100, 1, CDTXMania.Timer );
				if( this.ct上移動用.bInProgress )
				{
					this.ct下移動用.nCurrentValue = 100 - this.ct上移動用.nCurrentValue;
					this.ct上移動用.tStop();
				}
			}
		}
		private void tMoveCursorUp()
		{
			if ( this.n現在のカーソル行 != (int) E戻り値.GAMESTART - 1 )
			{
				CDTXMania.Skin.soundCursorMovement.tPlay();
				this.n現在のカーソル行--;
				this.ct上移動用.tStart( 0, 100, 1, CDTXMania.Timer );
				if( this.ct下移動用.bInProgress )
				{
					this.ct上移動用.nCurrentValue = 100 - this.ct下移動用.nCurrentValue;
					this.ct下移動用.tStop();
				}
			}
		}
		//-----------------
		#endregion
	}
}
