using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using DTXMania.Code.UI;
using FDK;
using SharpDX;
using Rectangle = System.Drawing.Rectangle;
using RectangleF = System.Drawing.RectangleF;
using SlimDXKey = SlimDX.DirectInput.Key;

namespace DTXMania
{
    internal class CStageConfig : CStage
    {
        // UI
        public UIGroup uiGroup;

        // プロパティ

        public CActDFPFont actFont { get; private set; }

        // コンストラクタ

        public CStageConfig()
        {
            CActDFPFont font;
            base.eStageID = CStage.EStage.Config;
            base.ePhaseID = CStage.EPhase.Common_DefaultState;
            this.actFont = font = new CActDFPFont();
            base.listChildActivities.Add(font);
            base.listChildActivities.Add(this.actFIFO = new CActFIFOWhite());
            base.listChildActivities.Add(this.actList = new CActConfigList());
            base.listChildActivities.Add(this.actKeyAssign = new CActConfigKeyAssign());
            base.bNotActivated = true;

            uiGroup = new UIGroup();
        }


        // メソッド

        public void tNotifyAssignmentComplete()
        {
            this.eItemPanelMode = EItemPanelMode.PadList;
        }
        public void tNotifyPadSelection(EKeyConfigPart part, EKeyConfigPad pad)
        {
            this.actKeyAssign.tStart(part, pad, this.actList.ibCurrentSelection.strItemName);
            this.eItemPanelMode = EItemPanelMode.KeyCodeList;
        }
        public void tNotifyItemChange()
        {
            this.tDrawSelectedItemDescriptionInDescriptionPanel();
        }
        
        // CStage 実装

        public override void OnActivate()
        {
            Trace.TraceInformation("コンフィグステージを活性化します。");
            Trace.Indent();
            try
            {
                configLeftOptionsMenu?.SetSelectedIndex(0);
                this.ftFont = new Font("MS PGothic", 17f, FontStyle.Regular, GraphicsUnit.Pixel);
                for (int i = 0; i < 4; i++)
                {
                    this.ctKeyRepetition[i] = new CCounter(0, 0, 0, CDTXMania.Timer);
                }
                this.bFocusIsOnMenu = true;
                this.eItemPanelMode = EItemPanelMode.PadList;
                this.ctDisplayWait = new CCounter( 0, 350, 1, CDTXMania.Timer );
            }
            finally
            {
                Trace.TraceInformation("コンフィグステージの活性化を完了しました。");
                Trace.Unindent();
            }
            base.OnActivate();		// 2011.3.14 yyagi: OnActivate()をtryの中から外に移動
        }
        public override void OnDeactivate()
        {
            Trace.TraceInformation("コンフィグステージを非活性化します。");
            Trace.Indent();
            try
            {
                CDTXMania.ConfigIni.tWrite(CDTXMania.strEXEのあるフォルダ + "Config.ini");	// CONFIGだけ
                if (this.ftFont != null)													// 以下OPTIONと共通
                {
                    this.ftFont.Dispose();
                    this.ftFont = null;
                }
                for (int i = 0; i < 4; i++)
                {
                    this.ctKeyRepetition[i] = null;
                }
                this.ctDisplayWait = null;
                base.OnDeactivate();
            }
            catch (UnauthorizedAccessException e)
            {
                Trace.TraceError(e.Message + "ファイルが読み取り専用になっていないか、管理者権限がないと書き込めなくなっていないか等を確認して下さい");
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
            }
            finally
            {
                Trace.TraceInformation("コンフィグステージの非活性化を完了しました。");
                Trace.Unindent();
            }
        }

        private UISelectList configLeftOptionsMenu;
        private UIImage menuCursor;
        public override void OnManagedCreateResources()											// OPTIONと画像以外共通
        {
            if (!base.bNotActivated)
            {
                //create resources for menu elements
                var bg = uiGroup.AddChild(new UIImage(CSkin.Path(@"Graphics\4_background.png")));
                bg.renderOrder = -100;
                bg.position = new Vector2(0, 0);
                
                var itemBar = uiGroup.AddChild(new UIImage(CSkin.Path(@"Graphics\4_item bar.png")));
                itemBar.position = new Vector2(400, 0);
                itemBar.renderOrder = 50;
                
                var headerPanel = uiGroup.AddChild(new UIImage(CSkin.Path(@"Graphics\4_header panel.png")));
                headerPanel.position = new Vector2(0, 0);
                headerPanel.renderOrder = 52;
                
                var footerPanel = uiGroup.AddChild(new UIImage(CSkin.Path(@"Graphics\4_footer panel.png")));
                footerPanel.position = new Vector2(0, 720 - footerPanel.Texture.szTextureSize.Height);
                footerPanel.renderOrder = 53;
                
                var cursor = uiGroup.AddChild(new UIImage(CSkin.Path(@"Graphics\4_menu cursor.png")));
                cursor.renderOrder = 100;
                cursor.isVisible = false;

                //left menu
                var leftMenu = uiGroup.AddChild(new UIGroup());
                leftMenu.position = new Vector2(245, 140);
                leftMenu.renderOrder = 50;
                
                var menuPanel = leftMenu.AddChild(new UIImage(CSkin.Path(@"Graphics\4_menu panel.png")));
                menuPanel.position = new Vector2(0, 0);
                
                //menu items
                configLeftOptionsMenu = leftMenu.AddChild(new UISelectList());
                //340 - size/2, so this becomes 340-245= 95
                configLeftOptionsMenu.position = new Vector2(95, 4);
                
                menuCursor = configLeftOptionsMenu.AddChild(new UIImage(CSkin.Path(@"Graphics\4_menu cursor.png")));
                menuCursor.drawMode = DrawMode.TiledLeftRight;
                menuCursor.position = new Vector2(-5, 2);
                menuCursor.size = new Rectangle(0, 0, 170, 32);
                menuCursor.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Top);
                
                var font = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.str選曲リストフォント), 18);
                configLeftOptionsMenu.AddSelectableChild(new UIBasicButton(font, "System", () => { this.actList.tSetupItemList_System(); }));
                configLeftOptionsMenu.AddSelectableChild(new UIBasicButton(font, "Drums", () => { this.actList.tSetupItemList_Drums(); }));
                configLeftOptionsMenu.AddSelectableChild(new UIBasicButton(font, "Guitar", () => { this.actList.tSetupItemList_Guitar(); }));
                configLeftOptionsMenu.AddSelectableChild(new UIBasicButton(font, "Bass", () => { this.actList.tSetupItemList_Bass(); }));
                configLeftOptionsMenu.AddSelectableChild(new UIBasicButton(font, "Exit", () => { this.actList.tSetupItemList_Exit(); }));
                configLeftOptionsMenu.UpdateLayout();
                configLeftOptionsMenu.SetSelectedIndex(0);
                
                if (this.bFocusIsOnMenu)
                {
                    this.tDrawSelectedMenuDescriptionInDescriptionPanel();
                }
                else
                {
                    this.tDrawSelectedItemDescriptionInDescriptionPanel();
                }

                base.OnManagedCreateResources();
            }
        }
        public override void OnManagedReleaseResources()											// OPTIONと同じ(COnfig.iniの書き出しタイミングのみ異なるが、無視して良い)
        {
            if (!base.bNotActivated)
            {
                CDTXMania.tReleaseTexture(ref this.txDescriptionPanel);
                
                uiGroup.Dispose();
                
                base.OnManagedReleaseResources();
            }
        }
        public override int OnUpdateAndDraw()
        {
            if (base.bNotActivated)
                return 0;

            if (base.bJustStartedUpdate)
            {
                base.ePhaseID = CStage.EPhase.Common_FadeIn;
                this.actFIFO.tStartFadeIn();
                base.bJustStartedUpdate = false;
            }
            this.ctDisplayWait.tUpdate();
            
            //update menu cursor position
            menuCursor.Texture.nTransparency = bFocusIsOnMenu ? 255 : 128;
            menuCursor.position.Y = 2 + configLeftOptionsMenu.currentlySelectedIndex * 32;
            
            uiGroup.Draw(CDTXMania.app.Device, Vector2.Zero);
            
            #region [ アイテム ]
            //---------------------
            switch (this.eItemPanelMode)
            {
                case EItemPanelMode.PadList:
                    this.actList.tUpdateAndDraw(!this.bFocusIsOnMenu);
                    break;

                case EItemPanelMode.KeyCodeList:
                    this.actKeyAssign.OnUpdateAndDraw();
                    break;
            }
            //---------------------
            #endregion
            #region [ Description panel ]
            //---------------------
            if( this.txDescriptionPanel != null && !this.bFocusIsOnMenu && this.actList.nTargetScrollCounter == 0 && this.ctDisplayWait.bReachedEndValue )
                // 15SEP20 Increasing x position by 180 pixels (was 620)
                this.txDescriptionPanel.tDraw2D(CDTXMania.app.Device, 800, 270);
            //---------------------
            #endregion

            #region [ Fade in and out ]
            //---------------------
            switch (base.ePhaseID)
            {
                case CStage.EPhase.Common_FadeIn:
                    if (this.actFIFO.OnUpdateAndDraw() != 0)
                    {
                        CDTXMania.Skin.bgmコンフィグ画面.tPlay();
                        base.ePhaseID = CStage.EPhase.Common_DefaultState;
                    }
                    break;

                case CStage.EPhase.Common_FadeOut:
                    if (this.actFIFO.OnUpdateAndDraw() == 0)
                    {
                        break;
                    }
                    return 1;
            }
            //---------------------
            #endregion
            
            // キー入力

            if ((base.ePhaseID != CStage.EPhase.Common_DefaultState)
                || this.actKeyAssign.bキー入力待ちの最中である
                || CDTXMania.actPluginOccupyingInput != null)
                return 0;

            // 曲データの一覧取得中は、キー入力を無効化する
            if (!CDTXMania.EnumSongs.IsEnumerating || CDTXMania.actEnumSongs.bコマンドでの曲データ取得 != true)
            {
                if ((CDTXMania.InputManager.Keyboard.bKeyPressed((int)SlimDXKey.Escape) || CDTXMania.Pad.bPressed(EInstrumentPart.DRUMS, EPad.LC)) || CDTXMania.Pad.bPressedGB(EPad.Cancel))
                {
                    CDTXMania.Skin.soundCancel.tPlay();
                    if (!this.bFocusIsOnMenu)
                    {
                        if (this.eItemPanelMode == EItemPanelMode.KeyCodeList)
                        {
                            CDTXMania.stageConfig.tNotifyAssignmentComplete();
                            return 0;
                        }
                        if (!this.actList.bIsKeyAssignSelected && !this.actList.bIsFocusingParameter)	// #24525 2011.3.15 yyagi, #32059 2013.9.17 yyagi
                        {
                            this.bFocusIsOnMenu = true;
                        }
                        this.tDrawSelectedMenuDescriptionInDescriptionPanel();
                        this.actList.tPressEsc();								// #24525 2011.3.15 yyagi ESC押下時の右メニュー描画用
                    }
                    else
                    {
                        this.actFIFO.tStartFadeOut();
                        base.ePhaseID = CStage.EPhase.Common_FadeOut;
                    }
                }
                else if ((CDTXMania.Pad.bPressedDGB(EPad.CY) || CDTXMania.Pad.bPressed(EInstrumentPart.DRUMS, EPad.RD) || (CDTXMania.ConfigIni.bEnterがキー割り当てのどこにも使用されていない && CDTXMania.InputManager.Keyboard.bKeyPressed((int)SlimDXKey.Return))))
                {
                    if (configLeftOptionsMenu.currentlySelectedIndex == 4)
                    {
                        CDTXMania.Skin.soundDecide.tPlay();
                        this.actFIFO.tStartFadeOut();
                        base.ePhaseID = CStage.EPhase.Common_FadeOut;
                    }
                    else if (this.bFocusIsOnMenu)
                    {
                        CDTXMania.Skin.soundDecide.tPlay();
                        this.bFocusIsOnMenu = false;
                        this.tDrawSelectedItemDescriptionInDescriptionPanel();
                    }
                    else
                    {
                        switch (this.eItemPanelMode)
                        {
                            case EItemPanelMode.PadList:
                                bool bIsKeyAssignSelectedBeforeHitEnter = this.actList.bIsKeyAssignSelected;	// #24525 2011.3.15 yyagi
                                this.actList.tPressEnter();
                                if (this.actList.b現在選択されている項目はReturnToMenuである)
                                {
                                    this.tDrawSelectedMenuDescriptionInDescriptionPanel();
                                    if (bIsKeyAssignSelectedBeforeHitEnter == false)							// #24525 2011.3.15 yyagi
                                    {
                                        this.bFocusIsOnMenu = true;
                                    }
                                }
                                break;

                            case EItemPanelMode.KeyCodeList:
                                this.actKeyAssign.tPressEnter();
                                break;
                        }
                    }
                }
                this.ctKeyRepetition.Up.tRepeatKey(CDTXMania.InputManager.Keyboard.bKeyPressing((int)SlimDXKey.UpArrow), new CCounter.DGキー処理(this.tMoveCursorUp));
                this.ctKeyRepetition.R.tRepeatKey(CDTXMania.Pad.bPressingGB(EPad.HH), new CCounter.DGキー処理(this.tMoveCursorUp));
                //Change to HT
                if (CDTXMania.Pad.bPressed(EInstrumentPart.DRUMS, EPad.HT))
                {
                    this.tMoveCursorUp();
                }
                this.ctKeyRepetition.Down.tRepeatKey(CDTXMania.InputManager.Keyboard.bKeyPressing((int)SlimDXKey.DownArrow), new CCounter.DGキー処理(this.tMoveCursorDown));
                this.ctKeyRepetition.B.tRepeatKey(CDTXMania.Pad.bPressingGB(EPad.SD), new CCounter.DGキー処理(this.tMoveCursorDown));
                //Change to LT
                if (CDTXMania.Pad.bPressed(EInstrumentPart.DRUMS, EPad.LT))
                {
                    this.tMoveCursorDown();
                }
            }
            
            return 0;
        }


        // Other

        #region [ private ]
        //-----------------
        private enum EItemPanelMode
        {
            PadList,
            KeyCodeList
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct STKeyRepetitionCounter
        {
            public CCounter Up;
            public CCounter Down;
            public CCounter R;
            public CCounter B;
            public CCounter this[int index]
            {
                get
                {
                    switch (index)
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
                    switch (index)
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

        private CActFIFOWhite actFIFO;
        private CActConfigKeyAssign actKeyAssign;
        private CActConfigList actList;
        //private CActOptionPanel actオプションパネル;
        private bool bFocusIsOnMenu;
        private STKeyRepetitionCounter ctKeyRepetition;
        private const int DESC_H = 0x80;
        private const int DESC_W = 220;
        private EItemPanelMode eItemPanelMode;
        private Font ftFont;
        
        private CTexture txDescriptionPanel;
        public CCounter ctDisplayWait;
        
        private void tMoveCursorDown()
        {
            if (!this.bFocusIsOnMenu)
            {
                switch (this.eItemPanelMode)
                {
                    case EItemPanelMode.PadList:
                        this.actList.tMoveToPrevious();
                        return;

                    case EItemPanelMode.KeyCodeList:
                        this.actKeyAssign.tMoveToNext();
                        return;
                }
            }
            else
            {
                CDTXMania.Skin.soundCursorMovement.tPlay();
                this.ctDisplayWait.nCurrentValue = 0;
                
                configLeftOptionsMenu.SelectNext();
                configLeftOptionsMenu.RunAction();
                
                this.tDrawSelectedMenuDescriptionInDescriptionPanel();
            }
        }
        private void tMoveCursorUp()
        {
            if (!this.bFocusIsOnMenu)
            {
                switch (this.eItemPanelMode)
                {
                    case EItemPanelMode.PadList:
                        this.actList.tMoveToNext();
                        return;

                    case EItemPanelMode.KeyCodeList:
                        this.actKeyAssign.tMoveToPrevious();
                        return;
                }
            }
            else
            {
                CDTXMania.Skin.soundCursorMovement.tPlay();
                this.ctDisplayWait.nCurrentValue = 0;
                
                configLeftOptionsMenu.SelectPrevious();
                configLeftOptionsMenu.RunAction();
                
                this.tDrawSelectedMenuDescriptionInDescriptionPanel();
            }
        }
		private void tDrawSelectedMenuDescriptionInDescriptionPanel()
		{
			try
			{
				var image = new Bitmap( (int)(220 * 2 ), (int)(192 * 2 ) );		// 説明文領域サイズの縦横 2 倍。（描画時に 0.5 倍で表示する。）
				var graphics = Graphics.FromImage( image );
				graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
				
				string[,] str = new string[ 2, 2 ];
				switch( configLeftOptionsMenu.currentlySelectedIndex )
				{
					case 0: //system
						str[ 0, 0 ] = "システムに関係する項目を設定します。";
						str[ 0, 1 ] = "";
						str[ 1, 0 ] = "Settings for an overall systems.";
						break;
                    
					case 1: //drums
						str[ 0, 0 ] = "ドラムの演奏に関する項目を設定します。";
						str[ 0, 1 ] = "";
						str[ 1, 0 ] = "Settings to play the drums.";
						str[ 1, 1 ] = "";
						break;

					case 2: //guitar
						str[ 0, 0 ] = "ギターの演奏に関する項目を設定します。";
						str[ 0, 1 ] = "";
						str[ 1, 0 ] = "Settings to play the guitar.";
						str[ 1, 1 ] = "";
						break;

					case 3: //bass
						str[ 0, 0 ] = "ベースの演奏に関する項目を設定します。";
						str[ 0, 1 ] = "";
						str[ 1, 0 ] = "Settings to play the bass.";
						str[ 1, 1 ] = "";
						break;

					case 4: //exit
						str[ 0, 0 ] = "設定を保存し、コンフィグ画面を終了します。";
						str[ 0, 1 ] = "";
						str[ 1, 0 ] = "Save the settings and exit from\nCONFIGURATION menu.";
						str[ 1, 1 ] = "";
						break;
				}
				
				int c = CDTXMania.isJapanese ? 0 : 1;
				for (int i = 0; i < 2; i++)
				{
					graphics.DrawString( str[ c, i ], this.ftFont, Brushes.Black, new PointF( 4f , ( i * 30 ) ) );
				}
				graphics.Dispose();
				if( this.txDescriptionPanel != null )
				{
					this.txDescriptionPanel.Dispose();
				}
				//this.txDescriptionPanel = new CTexture( CDTXMania.app.Device, image, CDTXMania.TextureFormat );
				// this.txDescriptionPanel.vcScaleRatio.X = 0.5f;
				// this.txDescriptionPanel.vcScaleRatio.Y = 0.5f;
				image.Dispose();
			}
			catch( CTextureCreateFailedException )
			{
				Trace.TraceError( "説明文テクスチャの作成に失敗しました。" );
				this.txDescriptionPanel = null;
			}
		}
		private void tDrawSelectedItemDescriptionInDescriptionPanel()
		{
			try
			{
				var image = new Bitmap( (int)(400), (int)(192) );		// 説明文領域サイズの縦横 2 倍。（描画時に 0.5 倍で表示する___のは中止。処理速度向上のため。）
				var graphics = Graphics.FromImage( image );
				graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

				CItemBase item = this.actList.ibCurrentSelection;
				if( ( item.str説明文 != null ) && ( item.str説明文.Length > 0 ) )
				{
					//int num = 0;
					//foreach( string str in item.str説明文.Split( new char[] { '\n' } ) )
					//{
					//    graphics.DrawString( str, this.ftFont, Brushes.White, new PointF( 4f * Scale.X, (float) num * Scale.Y ) );
					//    num += 30;
					//}
					graphics.DrawString( item.str説明文, this.ftFont, Brushes.Black, new RectangleF( 4f, (float) 0, 230, 430 ) );
				}
				graphics.Dispose();
				if( this.txDescriptionPanel != null )
				{
					this.txDescriptionPanel.Dispose();
				}
				this.txDescriptionPanel = new CTexture( CDTXMania.app.Device, image, CDTXMania.TextureFormat, false );
				//this.txDescriptionPanel.vcScaleRatio.X = 0.58f;
				//this.txDescriptionPanel.vcScaleRatio.Y = 0.58f;
				image.Dispose();
			}
			catch( CTextureCreateFailedException )
			{
				Trace.TraceError( "説明文パネルテクスチャの作成に失敗しました。" );
				this.txDescriptionPanel = null;
			}
		}
        //-----------------
        #endregion
    }
}
