using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using FDK;

using SlimDXKey = SlimDX.DirectInput.Key;

namespace DTXMania
{
    internal class CActSelectPopupMenu : CActivity
    {

        // プロパティ
        public int GetIndex(int pos)
        {
            return lciMenuItems[pos].GetIndex();
        }
        public object GetObj現在値(int pos)
        {
            return lciMenuItems[pos].obj現在値();
        }
        public bool bGotoDetailConfig
        {
            get;
            internal set;
        }

        /// <summary>
        /// ソートメニュー機能を使用中かどうか。外部からこれをtrueにすると、ソートメニューが出現する。falseにすると消える。
        /// </summary>
        public bool bIsActivePopupMenu
        {
            get;
            private set;
        }
        public virtual void tActivatePopupMenu(EInstrumentPart einst)
        {
            nItemSelecting = -1;		// #24757 2011.4.1 yyagi: Clear sorting status in each stating menu.
            this.eInst = einst;
            this.bIsActivePopupMenu = true;
            this.bIsSelectingIntItem = false;
            this.bGotoDetailConfig = false;
        }
        public virtual void tDeativatePopupMenu()
        {
            this.bIsActivePopupMenu = false;
        }


        public void Initialize(List<CItemBase> menulist, bool showAllItems, string title)
        {
            Initialize(menulist, showAllItems, title, 0);
        }

        public void Initialize(List<CItemBase> menulist, bool showAllItems, string title, int defaultPos)
        {
            strMenuTitle = title;
            lciMenuItems = menulist;
            bShowAllItems = showAllItems;
            n現在の選択行 = defaultPos;
        }


        public void tPressEnter()
        {
            if (this.bキー入力待ち)
            {
                CDTXMania.Skin.soundDecide.tPlay();

                if (this.n現在の選択行 != lciMenuItems.Count - 1)
                {
                    if (lciMenuItems[n現在の選択行].eType == CItemBase.EType.List ||
                         lciMenuItems[n現在の選択行].eType == CItemBase.EType.ONorOFFToggle ||
                         lciMenuItems[n現在の選択行].eType == CItemBase.EType.ONorOFForUndefined3State)
                    {
                        lciMenuItems[n現在の選択行].tMoveItemValueToNext();
                    }
                    else if (lciMenuItems[n現在の選択行].eType == CItemBase.EType.Integer)
                    {
                        bIsSelectingIntItem = !bIsSelectingIntItem;		// 選択状態/選択解除状態を反転する
                    }
                    else if (lciMenuItems[n現在の選択行].eType == CItemBase.EType.切替リスト)
                    {
                        // 特に何もしない
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                    nItemSelecting = n現在の選択行;
                }
                tPressEnterMain((int)lciMenuItems[n現在の選択行].GetIndex());

                this.bキー入力待ち = true;
            }
        }

        /// <summary>
        /// Decide押下時の処理を、継承先で記述する。
        /// </summary>
        /// <param name="val">CItemBaseの現在の設定値のindex</param>
        public virtual void tPressEnterMain(int val)
        {
        }
        /// <summary>
        /// Cancel押下時の追加処理があれば、継承先で記述する。
        /// </summary>
        public virtual void tCancel()
        {
        }
        /// <summary>
        /// BD二回入力時の追加処理があれば、継承先で記述する。
        /// </summary>
        public virtual void tBDContinuity()
        {
        }
        /// <summary>
        /// 追加の描画処理。必要に応じて、継承先で記述する。
        /// </summary>
        public virtual void tDrawSub()
        {
        }


        public void tMoveToNext()
        {
            if (this.bキー入力待ち)
            {
                CDTXMania.Skin.soundCursorMovement.tPlay();
                if (bIsSelectingIntItem)
                {
                    lciMenuItems[n現在の選択行].tMoveItemValueToPrevious();		// 項目移動と数値上下は方向が逆になるので注意
                }
                else
                {
                    if (++this.n現在の選択行 >= this.lciMenuItems.Count)
                    {
                        this.n現在の選択行 = 0;
                    }
                }
            }
        }
        public void tMoveToPrevious()
        {
            if (this.bキー入力待ち)
            {
                CDTXMania.Skin.soundCursorMovement.tPlay();
                if (bIsSelectingIntItem)
                {
                    lciMenuItems[n現在の選択行].tMoveItemValueToNext();		// 項目移動と数値上下は方向が逆になるので注意
                }
                else
                {
                    if (--this.n現在の選択行 < 0)
                    {
                        this.n現在の選択行 = this.lciMenuItems.Count - 1;
                    }
                }
            }
        }

        // CActivity 実装

        public override void OnActivate()
        {
            //		this.nSelectedRow = 0;
            this.bキー入力待ち = true;
            for (int i = 0; i < 4; i++)
            {
                this.ctキー反復用[i] = new CCounter(0, 0, 0, CDTXMania.Timer);
            }
            base.bNotActivated = true;

            this.bIsActivePopupMenu = false;
            this.font = new CActDFPFont();
            base.listChildActivities.Add(this.font);
            nItemSelecting = -1;

            this.CommandHistory = new DTXMania.CStageSongSelection.CCommandHistory();
            base.OnActivate();
        }
        public override void OnDeactivate()
        {
            if (!base.bNotActivated)
            {
                base.listChildActivities.Remove(this.font);
                this.font.OnDeactivate();
                this.font = null;

                CDTXMania.tReleaseTexture(ref this.txCursor);
                CDTXMania.tReleaseTexture(ref this.txPopupMenuBackground);
                for (int i = 0; i < 4; i++)
                {
                    this.ctキー反復用[i] = null;
                }
                base.OnDeactivate();
            }
        }
        public override void OnManagedCreateResources()
        {
            if (!base.bNotActivated)
            {
                string pathCursor = CSkin.Path(@"Graphics\ScreenConfig menu cursor.png"); ;
                string pathPopupMenuBackground = CSkin.Path(@"Graphics\ScreenSelect sort menu background.png");
                if (File.Exists(pathCursor))
                {
                    this.txCursor = CDTXMania.tGenerateTexture(pathCursor, false);
                }
                if (File.Exists(pathPopupMenuBackground))
                {
                    this.txPopupMenuBackground = CDTXMania.tGenerateTexture(pathPopupMenuBackground, false);
                }
                base.OnManagedCreateResources();
            }
        }
        public override void OnManagedReleaseResources()
        {
            if (!base.bNotActivated)
            {
                CDTXMania.tReleaseTexture(ref this.txPopupMenuBackground);
                CDTXMania.tReleaseTexture(ref this.txCursor);
            }
            base.OnManagedReleaseResources();
        }

        public override int OnUpdateAndDraw()
        {
            throw new InvalidOperationException("tUpdateAndDraw(bool)のほうを使用してください。");
        }

        public int tUpdateAndDraw()  // t進行描画
        {
            if (!base.bNotActivated && this.bIsActivePopupMenu)
            {
                nBodyX = 460; //XG選曲画面の中心点はX=646 Y=358
                nBodyY = 150;

                if (this.bキー入力待ち)
                {
                    #region [ CONFIG画面 ]
                    if (CDTXMania.Pad.bPressed(EInstrumentPart.GUITAR, EPad.Help))
                    {	// [SHIFT] + [F1] CONFIG
                        CDTXMania.Skin.soundCancel.tPlay();
                        tCancel();
                        this.bGotoDetailConfig = true;
                    }
                    #endregion
                    #region [ キー入力: キャンセル ]
                    else if (CDTXMania.InputManager.Keyboard.bKeyPressed((int)SlimDXKey.Escape)
                        || CDTXMania.Pad.bPressed(EInstrumentPart.DRUMS, EPad.LC)
                        || CDTXMania.Pad.bPressedGB(EPad.Cancel))
                    {	// キャンセル
                        CDTXMania.Skin.soundCancel.tPlay();
                        tCancel();
                        this.bIsActivePopupMenu = false;
                    }
                    #endregion
                    #region [ BD二回: キャンセル ]
                    else if (CDTXMania.Pad.bPressed(EInstrumentPart.DRUMS, EPad.BD))
                    {	// キャンセル
                        this.CommandHistory.Add(EInstrumentPart.DRUMS, EPadFlag.BD);
                        EPadFlag[] comChangeScrollSpeed = new EPadFlag[] { EPadFlag.BD, EPadFlag.BD };
                        if (this.CommandHistory.CheckCommand(comChangeScrollSpeed, EInstrumentPart.DRUMS))
                        {
                            CDTXMania.Skin.soundChange.tPlay();
                            tBDContinuity();
                            this.bIsActivePopupMenu = false;
                        }
                    }
                    #endregion
                    #region [ Px2 Guitar: 簡易CONFIG ]
                    if (CDTXMania.Pad.bPressed(EInstrumentPart.GUITAR, EPad.P))
                    {	// [BD]x2 スクロール速度変更
                        CommandHistory.Add(EInstrumentPart.GUITAR, EPadFlag.P);
                        EPadFlag[] comChangeScrollSpeed = new EPadFlag[] { EPadFlag.P, EPadFlag.P };
                        if (CommandHistory.CheckCommand(comChangeScrollSpeed, EInstrumentPart.GUITAR))
                        {
                            CDTXMania.Skin.soundChange.tPlay();
                            tBDContinuity();
                            this.bIsActivePopupMenu = false;
                        }
                    }
                    #endregion
                    #region [ Px2 Bass: 簡易CONFIG ]
                    if (CDTXMania.Pad.bPressed(EInstrumentPart.BASS, EPad.P))
                    {	// [BD]x2 スクロール速度変更
                        CommandHistory.Add(EInstrumentPart.BASS, EPadFlag.P);
                        EPadFlag[] comChangeScrollSpeed = new EPadFlag[] { EPadFlag.P, EPadFlag.P };
                        if (CommandHistory.CheckCommand(comChangeScrollSpeed, EInstrumentPart.BASS))
                        {
                            CDTXMania.Skin.soundChange.tPlay();
                            tBDContinuity();
                            this.bIsActivePopupMenu = false;
                        }
                    }
                    #endregion

                    #region [ キー入力: 決定 ]
                    // EInstrumentPart eInst = EInstrumentPart.UNKNOWN;
                    ESortAction eAction = ESortAction.END;
                    if (CDTXMania.Pad.bPressed(EInstrumentPart.GUITAR, EPad.Decide))
                    {
                        eInst = EInstrumentPart.GUITAR;
                        eAction = ESortAction.Decide;
                    }
                    else if (CDTXMania.Pad.bPressed(EInstrumentPart.BASS, EPad.Decide))
                    {
                        eInst = EInstrumentPart.BASS;
                        eAction = ESortAction.Decide;
                    }
                    else if (
                        CDTXMania.Pad.bPressed(EInstrumentPart.DRUMS, EPad.Decide)	// #24756 2011.4.1 yyagi: Add condition "Drum-Decide" to enable CY in Sort Menu.
                        || CDTXMania.Pad.bPressed(EInstrumentPart.DRUMS, EPad.RD)
                        || (CDTXMania.ConfigIni.bEnterがキー割り当てのどこにも使用されていない && CDTXMania.InputManager.Keyboard.bKeyPressed((int)SlimDXKey.Return)))
                    {
                        eInst = EInstrumentPart.DRUMS;
                        eAction = ESortAction.Decide;
                    }
                    if (eAction == ESortAction.Decide)	// 決定
                    {
                        this.tPressEnter();
                    }
                    #endregion
                    #region [ キー入力: 前に移動 ]
                    this.ctキー反復用.Up.tRepeatKey(CDTXMania.InputManager.Keyboard.bKeyPressing((int)SlimDXKey.UpArrow), new CCounter.DGキー処理(this.tMoveToPrevious));
                    this.ctキー反復用.R.tRepeatKey(CDTXMania.Pad.bPressingGB(EPad.R), new CCounter.DGキー処理(this.tMoveToPrevious));
                    //Change to HT
                    if (CDTXMania.Pad.bPressed(EInstrumentPart.DRUMS, EPad.HT))
                    {
                        this.tMoveToPrevious();
                    }
                    #endregion
                    #region [ キー入力: 次に移動 ]
                    this.ctキー反復用.Down.tRepeatKey(CDTXMania.InputManager.Keyboard.bKeyPressing((int)SlimDXKey.DownArrow), new CCounter.DGキー処理(this.tMoveToNext));
                    this.ctキー反復用.B.tRepeatKey(CDTXMania.Pad.bPressingGB(EPad.G), new CCounter.DGキー処理(this.tMoveToNext));
                    //Change to LT
                    if (CDTXMania.Pad.bPressed(EInstrumentPart.DRUMS, EPad.LT))
                    {
                        this.tMoveToNext();
                    }
                    #endregion
                }
                #region [ ポップアップメニュー 背景描画 ]
                if (this.txPopupMenuBackground != null)
                {
                    this.txPopupMenuBackground.tDraw2D(CDTXMania.app.Device, nBodyX, nBodyY);
                }
                #endregion
                #region [ ソートメニュータイトル描画 ]
                int x = nBodyX + 96, y = nBodyY + 4;
                font.t文字列描画(x, y, strMenuTitle, false, 1.0f);
                #endregion
                #region [ カーソル描画 ]
				if ( this.txCursor != null )
				{
					int height = 32;
                    int curX = nBodyX + 12;
                    int curY = nBodyY + 6 + (height * (this.n現在の選択行 + 1));
					this.txCursor.tDraw2D( CDTXMania.app.Device, curX, curY, new Rectangle( 0, 0, 16, 32 ) );
					curX += 0x10;
					Rectangle rectangle = new Rectangle( 8, 0, 0x10, 0x20 );
					for ( int j = 0; j < 19; j++ )
					{
						this.txCursor.tDraw2D( CDTXMania.app.Device, curX, curY, rectangle );
						curX += 16;
					}
					this.txCursor.tDraw2D( CDTXMania.app.Device, curX, curY, new Rectangle( 0x10, 0, 16, 32 ) );
				}
                #endregion
                #region [ ソート候補文字列描画 ]
                for (int i = 0; i < lciMenuItems.Count; i++)
                {
                    bool bItemBold = (i == nItemSelecting && !bShowAllItems) ? true : false;
                    font.t文字列描画(nBodyX + 18, nBodyY + 40 + i * 32, lciMenuItems[i].strItemName, bItemBold, 1.0f);

                    bool bValueBold = (bItemBold || (i == nItemSelecting && bIsSelectingIntItem)) ? true : false;
                    if (bItemBold || bShowAllItems)
                    {
                        string s;
                        switch (lciMenuItems[i].strItemName)
                        {
                            case "PlaySpeed":
                                {
                                    double d = (double)((int)lciMenuItems[i].obj現在値() / 20.0);
                                    s = "x" + d.ToString("0.000");
                                }
                                break;
                            case "ScrollSpeed":
                                {
                                    double d = (double)((((int)lciMenuItems[i].obj現在値()) + 1) / 2.0);
                                    s = "x" + d.ToString("0.0");
                                }
                                break;

                            default:
                                s = lciMenuItems[i].obj現在値().ToString();
                                break;
                        }
                        font.t文字列描画(nBodyX + 200, nBodyY + 40 + i * 32, s, bValueBold, 1.0f);
                    }
                }
                #endregion
                tDrawSub();
            }
            return 0;
        }


        // Other

        #region [ private ]
        //-----------------

        private bool bキー入力待ち;

        internal int n現在の選択行;
        internal EInstrumentPart eInst = EInstrumentPart.UNKNOWN;

        private CTexture txPopupMenuBackground;
        private CTexture txCursor;
        private CActDFPFont font;

        private int nBodyX;
        private int nBodyY;

        private string strMenuTitle;
        private List<CItemBase> lciMenuItems;
        private bool bShowAllItems;
        private bool bIsSelectingIntItem;
        public DTXMania.CStageSongSelection.CCommandHistory CommandHistory;

        [StructLayout(LayoutKind.Sequential)]
        private struct STキー反復用カウンタ
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
        private STキー反復用カウンタ ctキー反復用;

        private enum ESortAction : int
        {
            Cancel, Decide, Previous, Next, END
        }
        private int nItemSelecting;		// 「nSelectedRow」とは別に設ける。sortでメニュー表示直後にアイテムの中身を表示しないようにするため
        //-----------------
        #endregion
    }
}
