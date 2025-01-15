using System;
using System.Collections.Generic;
using System.Drawing;
using FDK;

using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;
using Point = System.Drawing.Point;

namespace DTXMania
{
    internal partial class CActConfigList : CActivity
    {
        // プロパティ

        public bool bIsKeyAssignSelected		// #24525 2011.3.15 yyagi
        {
            get
            {
                EMenuType e = this.eMenuType;
                if (e == EMenuType.KeyAssignBass || e == EMenuType.KeyAssignDrums ||
                    e == EMenuType.KeyAssignGuitar || e == EMenuType.KeyAssignSystem)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool bIsFocusingParameter		// #32059 2013.9.17 yyagi
        {
            get
            {
                return bFocusIsOnElementValue;
            }
        }
        
        //Keep these temporarily
        private CItemBase iSystemReturnToMenu;
        private CItemBase iDrumsReturnToMenu;
        private CItemBase iGuitarReturnToMenu;
        private CItemBase iBassReturnToMenu;
        
        public bool b現在選択されている項目はReturnToMenuである
        {
            get
            {
                CItemBase currentItem = this.listItems[this.nCurrentSelection];
                if (currentItem == this.iSystemReturnToMenu || currentItem == this.iDrumsReturnToMenu ||
                    currentItem == this.iGuitarReturnToMenu || currentItem == this.iBassReturnToMenu)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public CItemBase ibCurrentSelection => this.listItems[this.nCurrentSelection];
        public int nCurrentSelection;

        /// <summary>
        /// ESC押下時の右メニュー描画
        /// </summary>
        public void tPressEsc()
        {
            switch (this.eMenuType)
            {
                case EMenuType.KeyAssignSystem:
                    tSetupItemList_System();
                    break;
                case EMenuType.KeyAssignDrums:
                    tSetupItemList_Drums();
                    break;
                case EMenuType.KeyAssignGuitar:
                    tSetupItemList_Guitar();
                    break;
                case EMenuType.KeyAssignBass:
                    tSetupItemList_Bass();
                    break;
            }
        }
        
        public void tPressEnter()
        {
            CDTXMania.Skin.soundDecide.tPlay();
            
            if (this.bFocusIsOnElementValue)
            {
                this.bFocusIsOnElementValue = false;
            }
            else if (this.listItems[this.nCurrentSelection].eType == CItemBase.EType.Integer)
            {
                this.bFocusIsOnElementValue = true;
            }
            else
            {
                // Enter押下後の後処理
                this.listItems[this.nCurrentSelection].RunAction();
            }
        }   

        private void tGenerateSkinSample()
        {
            nSkinIndex = ((CItemList)this.listItems[this.nCurrentSelection]).n現在選択されている項目番号;
            if (nSkinSampleIndex != nSkinIndex)
            {
                string path = skinSubFolders[nSkinIndex];
                path = System.IO.Path.Combine(path, @"Graphics\2_background.jpg");
                Bitmap bmSrc = new Bitmap(path);
                Bitmap bmDest = new Bitmap(1280, 720);
                Graphics g = Graphics.FromImage(bmDest);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmSrc, new Rectangle(60, 106, (int)(1280 * 0.1984), (int)(720 * 0.1984)),
                    0, 0, 1280, 720, GraphicsUnit.Pixel);
                if (txSkinSample1 != null)
                {
                    CDTXMania.tDisposeSafely(ref txSkinSample1);
                }
                txSkinSample1 = CDTXMania.tGenerateTexture(bmDest, false);
                g.Dispose();
                bmDest.Dispose();
                bmSrc.Dispose();
                nSkinSampleIndex = nSkinIndex;
            }
        }

        public void tSetupItemList_Exit()
        {
            this.tRecordToConfigIni();
            this.eMenuType = EMenuType.Unknown;
        }
        
        public void tMoveToPrevious()
        {
            CDTXMania.Skin.soundCursorMovement.tPlay();
            if (this.bFocusIsOnElementValue)
            {
                this.listItems[this.nCurrentSelection].tMoveItemValueToPrevious();
                tPostProcessMoveUpDown();
            }
            else
            {
                this.nTargetScrollCounter += 100;
                CDTXMania.stageConfig.ctDisplayWait.nCurrentValue = 0;
            }
        }
        public void tMoveToNext()
        {
            CDTXMania.Skin.soundCursorMovement.tPlay();
            if (this.bFocusIsOnElementValue)
            {
                this.listItems[this.nCurrentSelection].tMoveItemValueToNext();
                tPostProcessMoveUpDown();
            }
            else
            {
                this.nTargetScrollCounter -= 100;
                CDTXMania.stageConfig.ctDisplayWait.nCurrentValue = 0;
            }
        }
        private void tPostProcessMoveUpDown()  // t要素値を上下に変更中の処理
        {
            if (this.listItems[this.nCurrentSelection] == this.iSystemMasterVolume)              // #33700 2014.4.26 yyagi
            {
                CDTXMania.SoundManager.nMasterVolume = this.iSystemMasterVolume.nCurrentValue;
            }
        }


        // CActivity 実装

        public override void OnActivate()
        {
            if (this.bActivated)
                return;

            this.listItems = new List<CItemBase>();
            this.eMenuType = EMenuType.Unknown;

            ScanSkinFolders();

            this.prvFont = new CPrivateFastFont( new FontFamily( CDTXMania.ConfigIni.str選曲リストフォント ), 15 );	// t項目リストの設定 の前に必要

            this.tSetupItemList_Bass();		// #27795 2012.3.11 yyagi; System設定の中でDrumsの設定を参照しているため、
            this.tSetupItemList_Guitar();	// 活性化の時点でDrumsの設定も入れ込んでおかないと、System設定中に例外発生することがある。
            this.tSetupItemList_Drums();	// 
            this.tSetupItemList_System();	// 順番として、最後にSystemを持ってくること。設定一覧の初期位置がSystemのため。
            
            this.bFocusIsOnElementValue = false;
            this.nTargetScrollCounter = 0;
            this.currentScrollCounter = 0;
            this.scrollTimerValue = -1;
            this.ctTriangleArrowAnimation = new CCounter();
            this.ctToastMessageCounter = new CCounter(0, 1, 10000, CDTXMania.Timer);

            CacheCurrentSoundDevices();
            base.OnActivate();
        }

        public override void OnDeactivate()
        {
            if (this.bNotActivated)
                return;

            this.tRecordToConfigIni();
            this.listItems.Clear();
            this.ctTriangleArrowAnimation = null;

            OnListMenuの解放();
            prvFont.Dispose();

            base.OnDeactivate();

            #region [ Skin変更 ]
            if (CDTXMania.Skin.GetCurrentSkinSubfolderFullName(true) != this.skinSubFolder_org)
            {
                CDTXMania.stageChangeSkin.tChangeSkinMain();	// #28195 2012.6.11 yyagi CONFIG脱出時にSkin更新
            }
            #endregion

            HandleSoundDeviceChanges();
            
            #region [ サウンドのタイムストレッチモード変更 ]
            FDK.CSoundManager.bIsTimeStretch = this.iSystemTimeStretch.bON;
            #endregion
        }

        public override void OnManagedCreateResources()
        {
            if (this.bNotActivated)
                return;

            this.txItemBoxNormal = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\4_itembox.png"), false);
            this.txItemBoxOther = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\4_itembox other.png"), false);
            this.txTriangleArrow = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\4_triangle arrow.png"), false);
            this.txDescriptionPanel = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\4_Description Panel.png" ) );
            this.txArrow = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\4_Arrow.png" ) );
            this.txItemBoxCursor = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\4_itembox cursor.png" ) );
            this.txSkinSample1 = null;		// スキン選択時に動的に設定するため、ここでは初期化しない
            this.prvFontForToastMessage = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.str選曲リストフォント), 14, FontStyle.Regular);
            base.OnManagedCreateResources();
        }
        public override void OnManagedReleaseResources()
        {
            if (this.bNotActivated)
                return;

            CDTXMania.tReleaseTexture(ref this.txSkinSample1);
            CDTXMania.tReleaseTexture(ref this.txItemBoxNormal);
            CDTXMania.tReleaseTexture(ref this.txItemBoxOther);
            CDTXMania.tReleaseTexture(ref this.txTriangleArrow);
            CDTXMania.tReleaseTexture( ref this.txDescriptionPanel );
            CDTXMania.tReleaseTexture( ref this.txArrow );
            CDTXMania.tReleaseTexture( ref this.txItemBoxCursor );
            CDTXMania.tReleaseTexture(ref this.txToastMessage);
            CDTXMania.tDisposeSafely(ref this.prvFontForToastMessage);
            base.OnManagedReleaseResources();
        }

		private void OnListMenuの初期化()
		{
			OnListMenuの解放();
			this.listMenu = new stMenuItemRight[ this.listItems.Count ];
		}

		/// <summary>
		/// 事前にレンダリングしておいたテクスチャを解放する。
		/// </summary>
		private void OnListMenuの解放()
		{
			if ( listMenu != null )
			{
				for ( int i = 0; i < listMenu.Length; i++ )
				{
					if ( listMenu[ i ].txParam != null )
					{
						listMenu[ i ].txParam.Dispose();
					}
					if ( listMenu[ i ].txMenuItemRight != null )
					{
						listMenu[ i ].txMenuItemRight.Dispose();
					}
				}
				this.listMenu = null;
			}
		}
        public override int OnUpdateAndDraw()
        {
            throw new InvalidOperationException("tUpdateAndDraw(bool)のほうを使用してください。");
        }
        public int tUpdateAndDraw(bool isFocusOnItemList)  // t進行描画 bool b項目リスト側にフォーカスがある
        {
            if (this.bNotActivated)
                return 0;

            // 進行

            #region [ First update and draw ] //初めての進行描画
            //-----------------
            if (base.bJustStartedUpdate)
            {
                this.scrollTimerValue = CSoundManager.rcPerformanceTimer.nCurrentTime;
                this.ctTriangleArrowAnimation.tStart(0, 9, 50, CDTXMania.Timer);

                base.bJustStartedUpdate = false;
            }
            //-----------------
            #endregion

            this.bFocusIsOnItemList = isFocusOnItemList;		// 記憶

            #region [ 項目スクロールの進行 ]
            //-----------------
            long currentTime = CDTXMania.Timer.nCurrentTime;
            if (currentTime < this.scrollTimerValue) this.scrollTimerValue = currentTime;

            const int INTERVAL = 2;	// [ms]
            while ((currentTime - this.scrollTimerValue) >= INTERVAL)
            {
                int scrollDistanceToTarget = Math.Abs((int)(this.nTargetScrollCounter - this.currentScrollCounter));
                int scrollAcceleration = 0;

                #region [ n加速度の決定；目標まで遠いほど加速する。]
                //-----------------
                if (scrollDistanceToTarget <= 100)
                {
                    scrollAcceleration = 2;
                }
                else if (scrollDistanceToTarget <= 300)
                {
                    scrollAcceleration = 3;
                }
                else if (scrollDistanceToTarget <= 500)
                {
                    scrollAcceleration = 4;
                }
                else
                {
                    scrollAcceleration = 8;
                }
                //-----------------
                #endregion
                #region [ this.n現在のスクロールカウンタに n加速度 を加減算。]
                //-----------------
                if (this.currentScrollCounter < this.nTargetScrollCounter)
                {
                    this.currentScrollCounter += scrollAcceleration;
                    if (this.currentScrollCounter > this.nTargetScrollCounter)
                    {
                        // 目標を超えたら目標値で停止。
                        this.currentScrollCounter = this.nTargetScrollCounter;
                    }
                }
                else if (this.currentScrollCounter > this.nTargetScrollCounter)
                {
                    this.currentScrollCounter -= scrollAcceleration;
                    if (this.currentScrollCounter < this.nTargetScrollCounter)
                    {
                        // 目標を超えたら目標値で停止。
                        this.currentScrollCounter = this.nTargetScrollCounter;
                    }
                }
                //-----------------
                #endregion
                #region [ 行超え処理、ならびに目標位置に到達したらスクロールを停止して項目変更通知を発行。]
                //-----------------
                if (this.currentScrollCounter >= 100)
                {
                    this.nCurrentSelection = this.tNextItem(this.nCurrentSelection);
                    this.currentScrollCounter -= 100;
                    this.nTargetScrollCounter -= 100;
                    if (this.nTargetScrollCounter == 0)
                    {
                        CDTXMania.stageConfig.tNotifyItemChange();
                    }
                }
                else if (this.currentScrollCounter <= -100)
                {
                    this.nCurrentSelection = this.tPreviousItem(this.nCurrentSelection);
                    this.currentScrollCounter += 100;
                    this.nTargetScrollCounter += 100;
                    if (this.nTargetScrollCounter == 0)
                    {
                        CDTXMania.stageConfig.tNotifyItemChange();
                    }
                }
                //-----------------
                #endregion

                this.scrollTimerValue += INTERVAL;
            }
            //-----------------
            #endregion

            #region [ Triangle arrow animation ]
            //-----------------
            if (this.bFocusIsOnItemList && (this.nTargetScrollCounter == 0))
                this.ctTriangleArrowAnimation.tUpdateLoop();
            //-----------------
            #endregion

            #region [ Update Toast Message Counter] 
            this.ctToastMessageCounter.tUpdate();
            if (this.ctToastMessageCounter.bReachedEndValue)
            {
                this.tUpdateToastMessage("");
            }
            #endregion

            // 描画

            this.basePanelCoordinates[4].X = this.bFocusIsOnItemList ? 0x228 : 0x25a;		// メニューにフォーカスがあるなら、項目リストの中央は頭を出さない。

            //2014.04.25 kairera0467 GITADORAでは項目パネルが11個だが、選択中のカーソルは中央に無いので両方を同じにすると7×2+1=15個パネルが必要になる。
            //　　　　　　　　　　　 さらに画面に映らないがアニメーション中に見える箇所を含めると17個は必要とされる。
            //　　　　　　　　　　　 ただ、画面に表示させる分には上のほうを考慮しなくてもよさそうなので、上4個は必要なさげ。
            #region [ Draw item panels ]
            //-----------------
            int nItem = this.nCurrentSelection;
            for (int i = 0; i < 4; i++)
                nItem = this.tPreviousItem(nItem);

            for (int rowIndex = -4; rowIndex < 10; rowIndex++)		// n行番号 == 0 がフォーカスされている項目パネル。
            {
                #region [ Skip Offscreen Item Panels ]
                //-----------------
                if (((rowIndex == -4) && (this.currentScrollCounter > 0)) ||		// 上に飛び出そうとしている
                    ((rowIndex == +9) && (this.currentScrollCounter < 0)))		// 下に飛び出そうとしている
                {
                    nItem = this.tNextItem(nItem);
                    continue;
                }
                //-----------------
                #endregion
                int n移動元の行の基本位置 = rowIndex + 4;
                int n移動先の行の基本位置 = (this.currentScrollCounter <= 0) ? ((n移動元の行の基本位置 + 1) % 14) : (((n移動元の行の基本位置 - 1) + 14) % 14);
                int x = this.pt新パネルの基本座標[n移動元の行の基本位置].X + ((int)((this.pt新パネルの基本座標[n移動先の行の基本位置].X - this.pt新パネルの基本座標[n移動元の行の基本位置].X) * (((double)Math.Abs(this.currentScrollCounter)) / 100.0)));
                int y = this.pt新パネルの基本座標[n移動元の行の基本位置].Y + ((int)((this.pt新パネルの基本座標[n移動先の行の基本位置].Y - this.pt新パネルの基本座標[n移動元の行の基本位置].Y) * (((double)Math.Abs(this.currentScrollCounter)) / 100.0)));
                int n新項目パネルX = 420;

                #region [ Render Row Panel Frame ]
                //-----------------
                switch (this.listItems[nItem].ePanelType)
                {
                    case CItemBase.EPanelType.Normal:
                        if (this.txItemBoxNormal != null)
                            this.txItemBoxNormal.tDraw2D(CDTXMania.app.Device, n新項目パネルX, y);
                        break;

                    case CItemBase.EPanelType.Other:
                        if (this.txItemBoxOther != null)
                            this.txItemBoxOther.tDraw2D(CDTXMania.app.Device, n新項目パネルX, y);
                        break;
                }
                //-----------------
                #endregion
                #region [ Render Item Name ]
                //-----------------
				if ( listMenu[ nItem ].txMenuItemRight != null )	// 自前のキャッシュに含まれているようなら、再レンダリングせずキャッシュを使用
				{
					listMenu[ nItem ].txMenuItemRight.tDraw2D( CDTXMania.app.Device, ( n新項目パネルX + 20 ), ( y + 24 ) );
				}
				else
				{
					Bitmap bmpItem = prvFont.DrawPrivateFont( this.listItems[ nItem ].strItemName, Color.White, Color.Transparent );
					listMenu[ nItem ].txMenuItemRight = CDTXMania.tGenerateTexture( bmpItem );
//					ctItem.tDraw2D( CDTXMania.app.Device, ( x + 0x12 ) * Scale.X, ( y + 12 ) * Scale.Y - 20 );
//					CDTXMania.tReleaseTexture( ref ctItem );
					CDTXMania.tDisposeSafely( ref bmpItem );
				}
				//CDTXMania.stageConfig.actFont.tDrawString( x + 0x12, y + 12, this.listItems[ nItem ].strItemName );
                //-----------------
                #endregion
                #region [ Render Item Elements ]
				//-----------------
				string strParam = null;
				bool isHighlighted = false;
				switch( this.listItems[ nItem ].eType )
				{
					case CItemBase.EType.ONorOFFToggle:
						#region [ *** ]
						//-----------------
						//CDTXMania.stageConfig.actFont.tDrawString( x + 210, y + 12, ( (CItemToggle) this.listItems[ nItem ] ).bON ? "ON" : "OFF" );
						strParam = ( (CItemToggle) this.listItems[ nItem ] ).bON ? "ON" : "OFF";
						break;
						//-----------------
						#endregion

					case CItemBase.EType.ONorOFForUndefined3State:
						#region [ *** ]
						//-----------------
						switch( ( (CItemThreeState) this.listItems[ nItem ] ).e現在の状態 )
						{
							case CItemThreeState.E状態.ON:
								strParam = "ON";
								break;

							case CItemThreeState.E状態.不定:
								strParam = "- -";
								break;

							default:
								strParam = "OFF";
								break;
						}
						//CDTXMania.stageConfig.actFont.tDrawString( x + 210, y + 12, "ON" );
						break;
						//-----------------
						#endregion

					case CItemBase.EType.Integer:		// #24789 2011.4.8 yyagi: add PlaySpeed supports (copied them from OPTION)
						#region [ *** ]
						//-----------------
						if( this.listItems[ nItem ] == this.iCommonPlaySpeed )
						{
							double d = ( (double) ( (CItemInteger) this.listItems[ nItem ] ).nCurrentValue ) / 20.0;
							//CDTXMania.stageConfig.actFont.tDrawString( x + 210, y + 12, d.ToString( "0.000" ), ( n行番号 == 0 ) && this.bFocusIsOnElementValue );
							strParam = d.ToString( "0.000" );
						}
						else if( this.listItems[ nItem ] == this.iDrumsScrollSpeed || this.listItems[ nItem ] == this.iGuitarScrollSpeed || this.listItems[ nItem ] == this.iBassScrollSpeed )
						{
							float f = ( ( (CItemInteger) this.listItems[ nItem ] ).nCurrentValue + 1 ) * 0.5f;
							//CDTXMania.stageConfig.actFont.tDrawString( x + 210, y + 12, f.ToString( "x0.0" ), ( n行番号 == 0 ) && this.bFocusIsOnElementValue );
							strParam = f.ToString( "x0.0" );
						}
						else
						{
							//CDTXMania.stageConfig.actFont.tDrawString( x + 210, y + 12, ( (CItemInteger) this.listItems[ nItem ] ).nCurrentValue.ToString(), ( n行番号 == 0 ) && this.bFocusIsOnElementValue );
							strParam = ( (CItemInteger) this.listItems[ nItem ] ).nCurrentValue.ToString();
						}
						isHighlighted = ( rowIndex == 0 ) && this.bFocusIsOnElementValue;
						break;
						//-----------------
						#endregion

					case CItemBase.EType.List:	// #28195 2012.5.2 yyagi: add Skin supports
						#region [ *** ]
						//-----------------
						{
							CItemList list = (CItemList) this.listItems[ nItem ];
							//CDTXMania.stageConfig.actFont.tDrawString( x + 210, y + 12, list.list項目値[ list.n現在選択されている項目番号 ] );
							strParam = list.list項目値[ list.n現在選択されている項目番号 ];

							#region [ 必要な場合に、Skinのサンプルを生成_描画する。#28195 2012.5.2 yyagi ]
							if ( this.listItems[ this.nCurrentSelection ] == this.iSystemSkinSubfolder )
							{
								tGenerateSkinSample();		// 最初にSkinの選択肢にきたとき(Enterを押す前)に限り、サンプル生成が発生する。

							}
							#endregion
							break;
						}
						//-----------------
						#endregion
				}
				if ( isHighlighted )
				{
					Bitmap bmpStr = isHighlighted ?
						prvFont.DrawPrivateFont( strParam, Color.White, Color.Black, Color.Yellow, Color.OrangeRed ) :
						prvFont.DrawPrivateFont( strParam, Color.Black, Color.Transparent );
					CTexture txStr = CDTXMania.tGenerateTexture( bmpStr, false );
					txStr.tDraw2D( CDTXMania.app.Device, ( n新項目パネルX + 260 ) , ( y + 20 ) );
					CDTXMania.tReleaseTexture( ref txStr );
					CDTXMania.tDisposeSafely( ref bmpStr );
				}
				else
				{
					int nIndex = this.listItems[ nItem ].GetIndex();
					if ( listMenu[ nItem ].nParam != nIndex || listMenu[ nItem ].txParam == null )
					{
						stMenuItemRight stm = listMenu[ nItem ];
						stm.nParam = nIndex;
						object o = this.listItems[ nItem ].obj現在値();
						stm.strParam = ( o == null ) ? "" : o.ToString();

				        Bitmap bmpStr =
				            prvFont.DrawPrivateFont( strParam, Color.Black, Color.Transparent );
				        stm.txParam = CDTXMania.tGenerateTexture( bmpStr, false );
				        CDTXMania.tDisposeSafely( ref bmpStr );

				        listMenu[ nItem ] = stm;
				    }
				    listMenu[ nItem ].txParam.tDraw2D( CDTXMania.app.Device, ( n新項目パネルX + 260 ) , ( y + 24 ) );
				}
				//-----------------
                #endregion

                nItem = this.tNextItem(nItem);
            }
            //-----------------
            #endregion

            #region[ Cursor ]
            if( this.bFocusIsOnItemList )
            {
                this.txItemBoxCursor.tDraw2D( CDTXMania.app.Device, 413, 193 );
            }
            #endregion

            #region[ Explanation Panel ]
            if( this.bFocusIsOnItemList && this.nTargetScrollCounter == 0 && CDTXMania.stageConfig.ctDisplayWait.bReachedEndValue )
            {
                // 15SEP20 Increasing x position by 180 pixels (was 601)
                this.txDescriptionPanel.tDraw2D( CDTXMania.app.Device, 781, 252 );
                if ( txSkinSample1 != null && this.nTargetScrollCounter == 0 && this.listItems[ this.nCurrentSelection ] == this.iSystemSkinSubfolder )
				{
                    // 15SEP20 Increasing x position by 180 pixels (was 615 - 60)
                    txSkinSample1.tDraw2D( CDTXMania.app.Device, 735, 442 - 106 );
				}
            }
            #endregion

            //項目リストにフォーカスがあって、かつスクロールが停止しているなら、パネルの上下に▲印を描画する。
            #region [ Draw a ▲ symbol at the top and bottom when scrolling finishes and the focus is on the item list ]
            //-----------------
            if( this.bFocusIsOnItemList )//&& (this.nTargetScrollCounter == 0))
            {
                int x;
                int y_upper;
                int y_lower;

                int n新カーソルX = 394;
                int n新カーソル上Y = 174;
                int n新カーソル下Y = 240;

                // 位置決定。

                if (this.bFocusIsOnElementValue)
                {
                    x = 552;	// 要素値の上下あたり。
                    y_upper = 0x117 - this.ctTriangleArrowAnimation.nCurrentValue;
                    y_lower = 0x17d + this.ctTriangleArrowAnimation.nCurrentValue;
                }
                else
                {
                    x = 552;	// 項目名の上下あたり。
                    y_upper = 0x129 - this.ctTriangleArrowAnimation.nCurrentValue;
                    y_lower = 0x16b + this.ctTriangleArrowAnimation.nCurrentValue;
                }

                //新矢印
                if( this.txArrow != null )
                {
                    this.txArrow.tDraw2D(CDTXMania.app.Device, n新カーソルX, n新カーソル上Y, new Rectangle(0, 0, 40, 40));
                    this.txArrow.tDraw2D(CDTXMania.app.Device, n新カーソルX, n新カーソル下Y, new Rectangle(0, 40, 40, 40));
                }
            }
            //-----------------
            #endregion

            #region [ Draw Toast Message ]

            if (this.txToastMessage != null)
            {
                this.txToastMessage.tDraw2D(CDTXMania.app.Device, 15, 325);
            }
            #endregion

            return 0;
        }


        // Other

        #region [ private ]
        //-----------------
        private enum EMenuType
        {
            System,
            Drums,
            Guitar,
            Bass,
            KeyAssignSystem,		// #24609 2011.4.12 yyagi: 画面キャプチャキーのアサイン
            KeyAssignDrums,
            KeyAssignGuitar,
            KeyAssignBass,
            Unknown
        }

        private bool bFocusIsOnItemList;
        private bool bFocusIsOnElementValue;
        private CCounter ctTriangleArrowAnimation;
        private EMenuType eMenuType;

        private List<CItemBase> listItems;
        private long scrollTimerValue;
        private int currentScrollCounter;
        public int nTargetScrollCounter;
        private Point[] basePanelCoordinates = new Point[] { new Point(0x25a, 4), new Point(0x25a, 0x4f), new Point(0x25a, 0x9a), new Point(0x25a, 0xe5), new Point(0x228, 0x130), new Point(0x25a, 0x17b), new Point(0x25a, 0x1c6), new Point(0x25a, 0x211), new Point(0x25a, 0x25c), new Point(0x25a, 0x2a7), new Point(0x25a, 0x2d0) };
        private Point[] pt新パネルの基本座標 = new Point[] { new Point(0x25a, -79), new Point(0x25a, -12), new Point(0x25a, 55), new Point(0x25a, 122), new Point(0x228, 189), new Point(0x25a, 256), new Point(0x25a, 323), new Point(0x25a, 390), new Point(0x25a, 457), new Point(0x25a, 524), new Point(0x25a, 591), new Point(0x25a, 658), new Point(0x25a, 725), new Point(0x25a, 792) };
        private CTexture txItemBoxOther;
        private CTexture txTriangleArrow;
        private CTexture txArrow;
        private CTexture txItemBoxNormal;
        private CTexture txItemBoxCursor;
        private CTexture txDescriptionPanel;
        private CTexture txToastMessage;
        private CPrivateFastFont prvFontForToastMessage;
        private CCounter ctToastMessageCounter;

        private CPrivateFastFont prvFont;
        //private List<string> list項目リスト_str最終描画名;
        private struct stMenuItemRight
        {
            //	public string strMenuItem;
            public CTexture txMenuItemRight;
            public int nParam;
            public string strParam;
            public CTexture txParam;
        }
        private stMenuItemRight[] listMenu;
        
        private CItemList iSystemGRmode;
        private CItemInteger iCommonPlaySpeed;
        
        private int tPreviousItem(int nItem)
        {
            if (--nItem < 0)
            {
                nItem = this.listItems.Count - 1;
            }
            return nItem;
        }
        private int tNextItem(int nItem)
        {
            if (++nItem >= this.listItems.Count)
            {
                nItem = 0;
            }
            return nItem;
        }

        private void tUpdateDisplayValuesFromConfigIni()
        {
            foreach (var item in listItems)
            {
                item.ReadFromConfig();
            }
        }

        private void tRecordToConfigIni()
        {
            foreach (var item in listItems)
            {
                item.WriteToConfig();
            }

            if (eMenuType == EMenuType.System)
            {
                CDTXMania.ConfigIni.bGuitarEnabled = (((this.iSystemGRmode.n現在選択されている項目番号 + 1) / 2) == 1);
                CDTXMania.ConfigIni.bDrumsEnabled = (((this.iSystemGRmode.n現在選択されている項目番号 + 1) % 2) == 1);

                CDTXMania.ConfigIni.strSystemSkinSubfolderFullName = skinSubFolders[nSkinIndex];				// #28195 2012.5.2 yyagi
                CDTXMania.Skin.SetCurrentSkinSubfolderFullName(CDTXMania.ConfigIni.strSystemSkinSubfolderFullName, true);
            }
        }

        private void tUpdateToastMessage(string strMessage) {
            CDTXMania.tDisposeSafely(ref this.txToastMessage);

            if (strMessage != "" && this.prvFontForToastMessage != null)
            {                
                Bitmap bmpItem = this.prvFontForToastMessage.DrawPrivateFont(strMessage, Color.White, Color.Black);
                this.txToastMessage = CDTXMania.tGenerateTexture(bmpItem);                
                CDTXMania.tDisposeSafely(ref bmpItem);
            }
            else 
            {
                this.txToastMessage = null;
            }

        }
        #endregion
    }
}
