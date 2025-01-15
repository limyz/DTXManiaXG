using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using DTXMania.Code.UI;
using FDK;
using SharpDX;
using Rectangle = System.Drawing.Rectangle;
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
            this.eInst = einst;
            this.bIsActivePopupMenu = true;
            this.bIsSelectingIntItem = false;
            this.bGotoDetailConfig = false;
        }
        public virtual void tDeativatePopupMenu()
        {
            this.bIsActivePopupMenu = false;
        }


        public void Initialize(List<CItemBase> menulist, string title)
        {
            Initialize(menulist, title, 0);
        }

        struct ItemPair
        {
            public UIDFPText name;
            public UIDFPText value;
        }

        private List<ItemPair> listItems = new List<ItemPair>();
        
        public void Initialize(List<CItemBase> menulist, string title, int defaultPos)
        {
            strMenuTitle = title;
            lciMenuItems = menulist;
            nCurrentSelection = defaultPos;

            if (menuItems == null) return;
            
            //remove current lci items
            menuItems.ClearChildren();

            listItems.Clear();
            listItems = new List<ItemPair>();
            
            //add new lci items
            for (int i = 0; i < lciMenuItems.Count; i++)
            {
                var item = menuItems.AddChild(new UIDFPText(font, lciMenuItems[i].strItemName));
                item.position = new Vector3(18, 40 + i * 32, 0);
                item.renderOrder = 1;
                
                var value = menuItems.AddChild(new UIDFPText(font, ""));
                value.position = new Vector3(200, 40 + i * 32, 0);
                value.renderOrder = 1;

                listItems.Add(new ItemPair() {name = item, value = value});
            }
        }


        public void tPressEnter()
        {
            if (this.bキー入力待ち)
            {
                CDTXMania.Skin.soundDecide.tPlay();

                if (this.nCurrentSelection != lciMenuItems.Count - 1)
                {
                    lciMenuItems[nCurrentSelection].RunAction();
                    
                    if (lciMenuItems[nCurrentSelection].eType == CItemBase.EType.Integer)
                    {
                        bIsSelectingIntItem = !bIsSelectingIntItem;		// 選択状態/選択解除状態を反転する
                    }
                }
                tPressEnterMain(lciMenuItems[nCurrentSelection].GetIndex());

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
                    lciMenuItems[nCurrentSelection].tMoveItemValueToPrevious();		// 項目移動と数値上下は方向が逆になるので注意
                }
                else
                {
                    if (++this.nCurrentSelection >= this.lciMenuItems.Count)
                    {
                        this.nCurrentSelection = 0;
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
                    lciMenuItems[nCurrentSelection].tMoveItemValueToNext();		// 項目移動と数値上下は方向が逆になるので注意
                }
                else
                {
                    if (--this.nCurrentSelection < 0)
                    {
                        this.nCurrentSelection = this.lciMenuItems.Count - 1;
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

                ui.Dispose();
                
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
                ui = new UIGroup();
                ui.position = new Vector3(1280.0f/2.0f, 720.0f/2.0f + 20.0f, 0); 
                ui.anchor = new Vector2(0.5f, 0.5f);
                
                var bg = ui.AddChild(new UIImage(CSkin.Path(@"Graphics\ScreenSelect sort menu background.png")));
                ui.size = bg.size;
                
                cursor = ui.AddChild(new UIImage(CSkin.Path(@"Graphics\ScreenConfig menu cursor.png")));
                cursor.position = new Vector3(12, 32 + 6, 0);
                cursor.size = new Vector2(336, 32);
                cursor.renderMode = ERenderMode.Sliced;
                cursor.sliceRect = new System.Drawing.RectangleF(8, 0, 16, 32);
                
                menuItems = ui.AddChild(new UIGroup());
                
                var menuText = ui.AddChild(new UIDFPText(font, strMenuTitle));
                menuText.position = new Vector3(96.0f, 4.0f, 0);
                
                base.OnManagedCreateResources();
            }
        }
        public override void OnManagedReleaseResources()
        {
            if (!base.bNotActivated)
            {
                ui.Dispose();
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
                
                cursor.position.Y = 6 + (32 * (this.nCurrentSelection + 1));
                
                //draw value items
                for (int i = 0; i < lciMenuItems.Count; i++)
                {
                    var pair = listItems[i];
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

                    bool bValueBold = i == nCurrentSelection && bIsSelectingIntItem;
                    pair.value.SetText(s);
                    pair.value.isHighlighted = bValueBold;
                }
                
                tDrawSub();
                
                ui.Draw(CDTXMania.app.Device, Matrix.Identity);
            }
            return 0;
        }


        // Other

        #region [ private ]
        //-----------------
        protected UIGroup ui;
        private UIGroup menuItems;
        private UIImage cursor;

        private bool bキー入力待ち;

        internal int nCurrentSelection;
        internal EInstrumentPart eInst = EInstrumentPart.UNKNOWN;
        
        private CActDFPFont font;

        private string strMenuTitle;
        private List<CItemBase> lciMenuItems;
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
            Decide, END
        }
        //-----------------
        #endregion
    }
}
