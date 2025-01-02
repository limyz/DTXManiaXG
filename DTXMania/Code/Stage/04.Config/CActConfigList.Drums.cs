namespace DTXMania
{
    internal partial class CActConfigList
    {
        #region [ t項目リストの設定_Drums() ]
        private CItemInteger iDrumsScrollSpeed;
        private CItemToggle iDrumsGraph;
        
        public void tSetupItemList_Drums()
        {
            this.tRecordToConfigIni();
            this.listItems.Clear();
            
            this.iDrumsReturnToMenu = new CItemBase("<< Return To Menu", CItemBase.EPanelType.Other,
                "左側のメニューに戻ります。",
                "Return to left menu.");
            this.listItems.Add(this.iDrumsReturnToMenu);
            
            //----------AutoPlay----------
            var iDrumsAutoPlayAll = new CItemThreeState("AutoPlay (All)", CItemThreeState.E状態.不定,
                "全パッドの自動演奏のON/OFFを\n" +
                "まとめて切り替えます。",
                "Activate/deactivate Auto for all drum lanes at once.");
            this.listItems.Add(iDrumsAutoPlayAll);

            var iDrumsLeftCymbal = new CItemToggle("    LeftCymbal", CDTXMania.ConfigIni.bAutoPlay.LC,
                "左シンバルを自動で演奏します。",
                "Play Left Cymbal automatically.");
            iDrumsLeftCymbal.BindConfig(
                () => iDrumsLeftCymbal.bON = CDTXMania.ConfigIni.bAutoPlay.LC,
                () => CDTXMania.ConfigIni.bAutoPlay.LC = iDrumsLeftCymbal.bON);
            this.listItems.Add(iDrumsLeftCymbal);

            var iDrumsHiHat = new CItemToggle("    HiHat", CDTXMania.ConfigIni.bAutoPlay.HH,
                "ハイハットを自動で演奏します。\n" +
                "（クローズ、オープンとも）",
                "Play HiHat automatically.\n" +
                "(It affects both HH-close and HH-open)");
            iDrumsHiHat.BindConfig(
                () => iDrumsHiHat.bON = CDTXMania.ConfigIni.bAutoPlay.HH,
                () => CDTXMania.ConfigIni.bAutoPlay.HH = iDrumsHiHat.bON);
            this.listItems.Add(iDrumsHiHat);

            var iDrumsLeftPedal = new CItemToggle("    LeftPedal", CDTXMania.ConfigIni.bAutoPlay.LP,
                "左ペダルを自動で演奏します。",
                "Play Left Pedal automatically.");
            iDrumsLeftPedal.BindConfig(
                () => iDrumsLeftPedal.bON = CDTXMania.ConfigIni.bAutoPlay.LP,
                () => CDTXMania.ConfigIni.bAutoPlay.LP = iDrumsLeftPedal.bON);
            this.listItems.Add(iDrumsLeftPedal);

            var iDrumsLeftBassDrum = new CItemToggle("    LBassDrum", CDTXMania.ConfigIni.bAutoPlay.LBD,
                "左バスドラムを自動で演奏します。",
                "Play Left Bass Drum automatically.");
            iDrumsLeftBassDrum.BindConfig(
                () => iDrumsLeftBassDrum.bON = CDTXMania.ConfigIni.bAutoPlay.LBD,
                () => CDTXMania.ConfigIni.bAutoPlay.LBD = iDrumsLeftBassDrum.bON);
            this.listItems.Add(iDrumsLeftBassDrum);

            var iDrumsSnare = new CItemToggle("    Snare", CDTXMania.ConfigIni.bAutoPlay.SD,
                "スネアを自動で演奏します。",
                "Play Snare automatically.");
            iDrumsSnare.BindConfig(
                () => iDrumsSnare.bON = CDTXMania.ConfigIni.bAutoPlay.SD,
                () => CDTXMania.ConfigIni.bAutoPlay.SD = iDrumsSnare.bON);
            this.listItems.Add(iDrumsSnare);

            var iDrumsBass = new CItemToggle("    BassDrum", CDTXMania.ConfigIni.bAutoPlay.BD,
                "バスドラムを自動で演奏します。",
                "Play Bass Drum automatically.");
            iDrumsBass.BindConfig(
                () => iDrumsBass.bON = CDTXMania.ConfigIni.bAutoPlay.BD,
                () => CDTXMania.ConfigIni.bAutoPlay.BD = iDrumsBass.bON);
            this.listItems.Add(iDrumsBass);

            var iDrumsHighTom = new CItemToggle("    HighTom", CDTXMania.ConfigIni.bAutoPlay.HT,
                "ハイタムを自動で演奏します。",
                "Play High Tom automatically.");
            iDrumsHighTom.BindConfig(
                () => iDrumsHighTom.bON = CDTXMania.ConfigIni.bAutoPlay.HT,
                () => CDTXMania.ConfigIni.bAutoPlay.HT = iDrumsHighTom.bON);
            this.listItems.Add(iDrumsHighTom);

            var iDrumsLowTom = new CItemToggle("    LowTom", CDTXMania.ConfigIni.bAutoPlay.LT,
                "ロータムを自動で演奏します。",
                "Play Low Tom automatically.");
            iDrumsLowTom.BindConfig(
                () => iDrumsLowTom.bON = CDTXMania.ConfigIni.bAutoPlay.LT,
                () => CDTXMania.ConfigIni.bAutoPlay.LT = iDrumsLowTom.bON);
            this.listItems.Add(iDrumsLowTom);

            var iDrumsFloorTom = new CItemToggle("    FloorTom", CDTXMania.ConfigIni.bAutoPlay.FT,
                "フロアタムを自動で演奏します。",
                "Play Floor Tom automatically.");
            iDrumsFloorTom.BindConfig(
                () => iDrumsFloorTom.bON = CDTXMania.ConfigIni.bAutoPlay.FT,
                () => CDTXMania.ConfigIni.bAutoPlay.FT = iDrumsFloorTom.bON);
            this.listItems.Add(iDrumsFloorTom);

            var iDrumsCymbal = new CItemToggle("    Cymbal", CDTXMania.ConfigIni.bAutoPlay.CY,
                "右シンバルを自動で演奏します。",
                "Play Right Cymbal automatically.");
            iDrumsCymbal.BindConfig(
                () => iDrumsCymbal.bON = CDTXMania.ConfigIni.bAutoPlay.CY,
                () => CDTXMania.ConfigIni.bAutoPlay.CY = iDrumsCymbal.bON);
            this.listItems.Add(iDrumsCymbal);

            var iDrumsRide = new CItemToggle("    Ride", CDTXMania.ConfigIni.bAutoPlay.RD,
                "ライドシンバルを自動で演奏します。",
                "Play Ride Cymbal automatically.");
            iDrumsRide.BindConfig(
                () => iDrumsRide.bON = CDTXMania.ConfigIni.bAutoPlay.RD,
                () => CDTXMania.ConfigIni.bAutoPlay.RD = iDrumsRide.bON);
            this.listItems.Add(iDrumsRide);
            
            //add the action for this later, as it needs to be able to change all of the above buttons
            iDrumsAutoPlayAll.action = () =>
            {
                bool bAutoOn = iDrumsAutoPlayAll.e現在の状態 == CItemThreeState.E状態.ON;
                
                iDrumsLeftCymbal.bON = bAutoOn;
                iDrumsHiHat.bON = bAutoOn;
                iDrumsSnare.bON = bAutoOn;
                iDrumsBass.bON = bAutoOn;
                iDrumsHighTom.bON = bAutoOn;
                iDrumsLowTom.bON = bAutoOn;
                iDrumsFloorTom.bON = bAutoOn;
                iDrumsCymbal.bON = bAutoOn;
                iDrumsRide.bON = bAutoOn;
                iDrumsLeftPedal.bON = bAutoOn;
                iDrumsLeftBassDrum.bON = bAutoOn;
            };

            //----------StandardOption----------

            this.iDrumsScrollSpeed = new CItemInteger("ScrollSpeed", 0, 0x7cf, CDTXMania.ConfigIni.nScrollSpeed.Drums,
                "ノーツの流れるスピードを\n"+
                "変更します。\n" +
                "数字が大きくなるほど\n" +
                "スピードが速くなり、\n"+
                "ノーツの間隔が広がります。",
                "Change the scroll speed for drums lanes.\n" +
                "You can set it from x0.5 to x1000.0.\n" +
                "(ScrollSpeed=x0.5 means half speed)");
            iDrumsScrollSpeed.BindConfig(
                () => iDrumsScrollSpeed.nCurrentValue = CDTXMania.ConfigIni.nScrollSpeed.Drums,
                () => CDTXMania.ConfigIni.nScrollSpeed.Drums = iDrumsScrollSpeed.nCurrentValue);
            this.listItems.Add(this.iDrumsScrollSpeed);

            var iDrumsHIDSUD = new CItemList("HID-SUD", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.nHidSud.Drums,
                "HIDDEN:チップが途中から見えなくなります。\n" +
                "SUDDEN:チップが途中まで見えません。\n" +
                "HID-SUD:HIDDEN、SUDDEN\n" +
                "の両方が適用されます。\n" +
                "STEALTH:チップがずっと表示されません。",
                "Hidden: Chips disappear at mid-screen\n" +
                "Sudden: Chips appear from mid-screen\n" +
                "HidSud: Chips appear only at mid-screen\n" +
                "Stealth: Chips are transparent",
                new string[] { "OFF", "Hidden", "Sudden", "HidSud", "Stealth" });
            iDrumsHIDSUD.BindConfig(
                () => iDrumsHIDSUD.n現在選択されている項目番号 = CDTXMania.ConfigIni.nHidSud.Drums,
                () => CDTXMania.ConfigIni.nHidSud.Drums = iDrumsHIDSUD.n現在選択されている項目番号);
            this.listItems.Add(iDrumsHIDSUD);

            //----------DisplayOption----------
            var iDrumsDark = new CItemList("       Dark", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eDark,
                "レーン表示のオプションを\n"+
                "まとめて切り替えます。\n" +
                "HALF:レーンが表示されなく\n"+
                "なります。\n" +
                "FULL:さらに小節線、拍線、\n"+
                "判定ラインも表示されなくなります。",
                "OFF: all display parts are shown.\n"+
                "HALF: lanes and gauge are hidden.\n"+
                "FULL: additionaly to HALF, bar/beat lines, hit bar are hidden.",
                new string[] { "OFF", "HALF", "FULL" });
            this.listItems.Add(iDrumsDark);
            
            var iDrumsLaneDisp = new CItemList("LaneDisp", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.nLaneDisp.Drums,
                "レーンの縦線と小節線の表示を切り替えます。\n" +
                "ALL  ON :全て表示します。\n" +
                "LANE OFF:レーン背景を表示しません。\n" +
                "LINE OFF:小節線を表示しません。\n" +
                "ALL  OFF:全て表示しません。",
                "Display of vertical lines and bar lines in the lane.\n" +
                "ALL ON: Show all\n" +
                "LANE OFF: Do not display lane background\n" +
                "LINE OFF: Do not display bar lines\n" +
                "ALL OFF: Do not display any",
                new string[] { "ALL ON", "LANE OFF", "LINE OFF", "ALL OFF" });
            iDrumsLaneDisp.BindConfig(
                () => iDrumsLaneDisp.n現在選択されている項目番号 = CDTXMania.ConfigIni.nLaneDisp.Drums,
                () => CDTXMania.ConfigIni.nLaneDisp.Drums = iDrumsLaneDisp.n現在選択されている項目番号);
            this.listItems.Add(iDrumsLaneDisp);

            var iDrumsJudgeLineDisp = new CItemToggle("JudgeLineDisp", CDTXMania.ConfigIni.bJudgeLineDisp.Drums,
                "判定ラインの表示 / 非表示を切り替えます。",
                "Toggle JudgeLine");
            iDrumsJudgeLineDisp.BindConfig(
                () => iDrumsJudgeLineDisp.bON = CDTXMania.ConfigIni.bJudgeLineDisp.Drums,
                () => CDTXMania.ConfigIni.bJudgeLineDisp.Drums = iDrumsJudgeLineDisp.bON);
            this.listItems.Add(iDrumsJudgeLineDisp);

            var iDrumsLaneFlush = new CItemToggle("LaneFlush", CDTXMania.ConfigIni.bLaneFlush.Drums,
                "レーンフラッシュの表示 / 非表示を\n" +
                 "切り替えます。",
                "Toggle LaneFlush");
            iDrumsLaneFlush.BindConfig(
                () => iDrumsLaneFlush.bON = CDTXMania.ConfigIni.bLaneFlush.Drums,
                () => CDTXMania.ConfigIni.bLaneFlush.Drums = iDrumsLaneFlush.bON);
            this.listItems.Add(iDrumsLaneFlush);
            
            //add the action for this later, as it needs to be able to change all of the above buttons
            iDrumsDark.action = () =>
            {
                if (iDrumsDark.n現在選択されている項目番号 == (int)EDarkMode.FULL)
                {
                    iDrumsLaneDisp.n現在選択されている項目番号 = 3;
                    iDrumsJudgeLineDisp.bON = false;
                    iDrumsLaneFlush.bON = false;
                }
                else if (iDrumsDark.n現在選択されている項目番号 == (int)EDarkMode.HALF)
                {
                    iDrumsLaneDisp.n現在選択されている項目番号 = 1;
                    iDrumsJudgeLineDisp.bON = true;
                    iDrumsLaneFlush.bON = true;
                }
                else
                {
                    iDrumsLaneDisp.n現在選択されている項目番号 = 0;
                    iDrumsJudgeLineDisp.bON = true;
                    iDrumsLaneFlush.bON = true;
                }
            };

            var iDrumsAttackEffect = new CItemList("AttackEffect", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eAttackEffect.Drums,
                "アタックエフェクトの表示方法を設定します。\n" +
                "ALL ON: すべて表示\n" +
                "ChipOFF: チップエフェクトのみ消す\n" +
                "EffectOnly: エフェクト画像以外消す\n" +
                "ALL OFF: すべて消す",
                "",
                new string[] { "ALL ON", "ChipOFF", "EffectOnly", "ALL OFF" });
            iDrumsAttackEffect.BindConfig(
                () => iDrumsAttackEffect.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eAttackEffect.Drums,
                () => CDTXMania.ConfigIni.eAttackEffect.Drums = (EType)iDrumsAttackEffect.n現在選択されている項目番号);
            this.listItems.Add(iDrumsAttackEffect);

            var iDrumsReverse = new CItemToggle("Reverse", CDTXMania.ConfigIni.bReverse.Drums,
                "ONにすると\n"+
                "判定ラインが上になり、\n" +
                "ノーツが下から上へ\n"+
                "流れます。",
                "The scroll way is reversed. Drums chips flow from the bottom to the top.");
            iDrumsReverse.BindConfig(
                () => iDrumsReverse.bON = CDTXMania.ConfigIni.bReverse.Drums,
                () => CDTXMania.ConfigIni.bReverse.Drums = iDrumsReverse.bON);
            this.listItems.Add(iDrumsReverse);

            var iDrumsPosition = new CItemList("JudgePosition", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.JudgementStringPosition.Drums,
                "ゲーム中に表示される\n"+
                "判定文字の位置を変更します。\n" +
                "  P-A: OnTheLane\n" +
                "  P-B: 判定ライン下\n" +
                "  OFF: 表示しない",
                "The position to show judgement mark.\n" +
                "(Perfect, Great, ...)\n" +
                "\n" +
                " P-A: on the lanes.\n" +
                " P-B: under the hit bar.\n" +
                " OFF: no judgement mark.",
                new string[] { "P-A", "P-B", "OFF" });
            iDrumsPosition.BindConfig(
                () => iDrumsPosition.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.JudgementStringPosition.Drums,
                () => CDTXMania.ConfigIni.JudgementStringPosition.Drums = (EType)iDrumsPosition.n現在選択されている項目番号);
            this.listItems.Add(iDrumsPosition);

            var iDrumsComboDisp = new CItemToggle("Combo", CDTXMania.ConfigIni.bドラムコンボ文字の表示,
                "OFFにするとコンボが表示されなくなります。",
                "Turn ON the Drums Combo Display");
            iDrumsComboDisp.BindConfig(
                () => iDrumsComboDisp.bON = CDTXMania.ConfigIni.bドラムコンボ文字の表示,
                () => CDTXMania.ConfigIni.bドラムコンボ文字の表示 = iDrumsComboDisp.bON);
            this.listItems.Add( iDrumsComboDisp );

            var iDrumsLaneType = new CItemList("LaneType", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eLaneType.Drums,
                "ドラムのレーンの配置を変更します。\n" +
                "Type-A 通常の設定です。\n" +
                "Type-B 2ペダルとタムをそれぞれま\n" +
                "とめた表示です。\n" +
                "Type-C 3タムのみをまとめた表示です。\n" +
                "Type-D 左右完全対象の表示です。",
                "To change the displaying position of\n" +
                "Drum Lanes.\n" +
                "Type-A default\n" +
                "Type-B Summarized 2 pedals and Toms.\n" +
                "Type-C Summarized 3 Toms only.\n" +
                "Type-D Work In Progress....",
                new string[] { "Type-A", "Type-B", "Type-C", "Type-D" });
            iDrumsLaneType.BindConfig(
                () => iDrumsLaneType.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eLaneType.Drums,
                () => CDTXMania.ConfigIni.eLaneType.Drums = (EType)iDrumsLaneType.n現在選択されている項目番号);
            this.listItems.Add(iDrumsLaneType);

            var iDrumsRDPosition = new CItemList("RDPosition", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eRDPosition,
                "ライドシンバルレーンの表示\n" +
                "位置を変更します。\n"+
                "RD RC:最右端はRCレーンになります\n"+
                "RC RD: 最右端はRDレーンになります",
                "Change the display position of the ride cymbal.\n"+
                "RD RC: Rightmost lane is RC\n" +
                "RC RD: Rightmost lane is RD",
                new string[] { "RD RC", "RC RD" });
            iDrumsRDPosition.BindConfig(
                () => iDrumsRDPosition.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eRDPosition,
                () => CDTXMania.ConfigIni.eRDPosition = (ERDPosition)iDrumsRDPosition.n現在選択されている項目番号);
            this.listItems.Add(iDrumsRDPosition);
            
            //----------SpecialOption----------

            var iDrumsHAZARD = new CItemToggle("HAZARD", CDTXMania.ConfigIni.bHAZARD,
                "ドSハザードモード\n" +
                "GREAT以下の判定でも回数が減ります。",
                "SuperHazardMode\n" +
                "");
            iDrumsHAZARD.BindConfig(
                () => iDrumsHAZARD.bON = CDTXMania.ConfigIni.bHAZARD,
                () => CDTXMania.ConfigIni.bHAZARD = iDrumsHAZARD.bON);
            this.listItems.Add(iDrumsHAZARD);

            var iDrumsTight = new CItemToggle("Tight", CDTXMania.ConfigIni.bTight,
                "ドラムチップのないところでパッドを\n" +
                "叩くとミスになります。",
                "Hitting pad without chip is a MISS.");
            iDrumsTight.BindConfig(
                () => iDrumsTight.bON = CDTXMania.ConfigIni.bTight,
                () => CDTXMania.ConfigIni.bTight = iDrumsTight.bON);
            this.listItems.Add(iDrumsTight);
            
            var iSystemHHGroup = new CItemList("HH Group", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eHHGroup,
                "ハイハットレーン打ち分け設定：\n" +
                "左シンバル、ハイハットオープン、ハ\n" +
                "イハットクローズの打ち分け方法を指\n" +
                "定します。\n" +
                "  HH-0 ... LC | HHC | HHO\n" +
                "  HH-1 ... LC & ( HHC | HHO )\n" +
                "  HH-2 ... LC | ( HHC & HHO )\n" +
                "  HH-3 ... LC & HHC & HHO\n" +
                "\n",
                "HH-0: LC|HC|HO; all are separated.\n" +
                "HH-1: LC&(HC|HO);\n" +
                " HC and HO are separted.\n" +
                " LC is grouped with HC and HHO.\n" +
                "HH-2: LC|(HC&HO);\n" +
                " LC and HHs are separated.\n" +
                " HC and HO are grouped.\n" +
                "HH-3: LC&HC&HO; all are grouped.\n" +
                "\n",
                new string[] { "HH-0", "HH-1", "HH-2", "HH-3" });
            iSystemHHGroup.BindConfig(
                () => iSystemHHGroup.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eHHGroup,
                () => CDTXMania.ConfigIni.eHHGroup = (EHHGroup)iSystemHHGroup.n現在選択されている項目番号);
            this.listItems.Add(iSystemHHGroup);

            var iSystemFTGroup = new CItemList("FT Group", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eFTGroup,
                "フロアタム打ち分け設定：\n" +
                "ロータムとフロアタムの打ち分け方法\n" +
                "を指定します。\n" +
                "  FT-0 ... LT | FT\n" +
                "  FT-1 ... LT & FT\n",
                "FT-0: LT|FT\n" +
                " LT and FT are separated.\n" +
                "FT-1: LT&FT\n" +
                " LT and FT are grouped.",
                new string[] { "FT-0", "FT-1" });
            iSystemFTGroup.BindConfig(
                () => iSystemFTGroup.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eFTGroup,
                () => CDTXMania.ConfigIni.eFTGroup = (EFTGroup)iSystemFTGroup.n現在選択されている項目番号);
            this.listItems.Add(iSystemFTGroup);

            var iSystemCYGroup = new CItemList("CY Group", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eCYGroup,
                "シンバルレーン打ち分け設定：\n" +
                "右シンバルとライドシンバルの打ち分\n" +
                "け方法を指定します。\n" +
                "  CY-0 ... CY | RD\n" +
                "  CY-1 ... CY & RD\n",
                "CY-0: CY|RD\n" +
                " CY and RD are separated.\n" +
                "CY-1: CY&RD\n" +
                " CY and RD are grouped.",
                new string[] { "CY-0", "CY-1" });
            iSystemCYGroup.BindConfig(
                () => iSystemCYGroup.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eCYGroup,
                () => CDTXMania.ConfigIni.eCYGroup = (ECYGroup)iSystemCYGroup.n現在選択されている項目番号);
            this.listItems.Add(iSystemCYGroup);

            var iSystemBDGroup = new CItemList("BD Group", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eBDGroup,		// #27029 2012.1.4 from
                "フットペダル打ち分け設定：\n" +
                "左ペダル、左バスドラ、右バスドラの打ち分け\n" +
                "方法を指定します。\n" +
                "  BD-0 ... LP | LBD | BD\n" +
                "  BD-1 ... LP | LBD & BD\n" +
                "  BD-2 ... LP & LBD | BD\n" +
                "  BD-3 ... LP & LBD & BD\n",
                "Foot pedal grouping settings:\n" +
                "Specifies how the left pedal, " +
                "left bass drum, and right bass " +
                "drum are grouped.\n" +
                "  BD-0 ... LP | LBD | BD\n" +
                "  BD-1 ... LP | LBD & BD\n" +
                "  BD-2 ... LP & LBD | BD\n" +
                "  BD-3 ... LP & LBD & BD\n",
                new string[] { "BD-0", "BD-1", "BD-2", "BD-3" });
            iSystemBDGroup.BindConfig(
                () => iSystemBDGroup.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eBDGroup,
                () => CDTXMania.ConfigIni.eBDGroup = (EBDGroup)iSystemBDGroup.n現在選択されている項目番号);
            this.listItems.Add(iSystemBDGroup);

            var iSystemCymbalFree = new CItemToggle("CymbalFree", CDTXMania.ConfigIni.bシンバルフリー,
                "シンバルフリーモード：\n" +
                "左シンバル_右シンバルの区別をなく\n" +
                "します。ライドシンバルまで区別をな\n" +
                "くすか否かは、CYGroup に従います。\n",
                "Turn ON to group LC (left cymbal) and\n" +
                " CY (right cymbal).\n" +
                "Whether RD (ride cymbal) is also\n" +
                " grouped or not depends on the\n" +
                "'CY Group' setting.");
            iSystemCymbalFree.BindConfig(
                () => iSystemCymbalFree.bON = CDTXMania.ConfigIni.bシンバルフリー,
                () => CDTXMania.ConfigIni.bシンバルフリー = iSystemCymbalFree.bON);
            this.listItems.Add(iSystemCymbalFree);

            var iSystemHitSoundPriorityHH = new CItemList("HH Priority", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eHitSoundPriorityHH,
                "発声音決定の優先順位：\n" +
                "ハイハットレーン打ち分け有効時に、\n" +
                "チップの発声音をどのように決定する\n" +
                "かを指定します。\n" +
                "  C > P ... チップの音が優先\n" +
                "  P > C ... 叩いたパッドの音が優先\n" +
                "\n",
                "To specify playing sound in case you're\n" +
                " using HH-0,1 and 2.\n" +
                "\n" +
                "C>P:\n" +
                " Chip sound is prior to the pad sound.\n" +
                "P>C:\n" +
                " Pad sound is prior to the chip sound.\n" +
                "\n" +
                "* This value cannot be changed while \n" +
                "  BD Group is set as BD-1.",
                new string[] { "C>P", "P>C" });
            iSystemHitSoundPriorityHH.BindConfig(
                () => iSystemHitSoundPriorityHH.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eHitSoundPriorityHH,
                () => CDTXMania.ConfigIni.eHitSoundPriorityHH = (EPlaybackPriority)iSystemHitSoundPriorityHH.n現在選択されている項目番号);
            this.listItems.Add(iSystemHitSoundPriorityHH);

            var iSystemHitSoundPriorityFT = new CItemList("FT Priority", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eHitSoundPriorityFT,
                "発声音決定の優先順位：\n" +
                "フロアタム打ち分け有効時に、チップ\n" +
                "の発声音をどのように決定するかを\n" +
                "指定します。\n" +
                "  C > P ... チップの音が優先\n" +
                "  P > C ... 叩いたパッドの音が優先",
                "To specify playing sound in case you're\n" +
                " using FT-0.\n" +
                "\n" +
                "C>P:\n" +
                " Chip sound is prior to the pad sound.\n" +
                "P>C:\n" +
                " Pad sound is prior to the chip sound.",
                new string[] { "C>P", "P>C" });
            iSystemHitSoundPriorityFT.BindConfig(
                () => iSystemHitSoundPriorityFT.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eHitSoundPriorityFT,
                () => CDTXMania.ConfigIni.eHitSoundPriorityFT = (EPlaybackPriority)iSystemHitSoundPriorityFT.n現在選択されている項目番号);
            this.listItems.Add(iSystemHitSoundPriorityFT);

            var iSystemHitSoundPriorityCY = new CItemList("CY Priority", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eHitSoundPriorityCY,
                "発声音決定の優先順位：\n" +
                "シンバルレーン打ち分け有効時に、\n" +
                "チップの発声音をどのように決定する\n" +
                "かを指定します。\n" +
                "  C > P ... チップの音が優先\n" +
                "  P > C ... 叩いたパッドの音が優先",
                "To specify playing sound in case you're\n" +
                " using CY-0.\n" +
                "\n" +
                "C>P:\n" +
                " Chip sound is prior to the pad sound.\n" +
                "P>C:\n" +
                " Pad sound is prior to the chip sound.",
                new string[] { "C>P", "P>C" });
            iSystemHitSoundPriorityCY.BindConfig(
                () => iSystemHitSoundPriorityCY.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eHitSoundPriorityCY,
                () => CDTXMania.ConfigIni.eHitSoundPriorityCY = (EPlaybackPriority)iSystemHitSoundPriorityCY.n現在選択されている項目番号);
            this.listItems.Add(iSystemHitSoundPriorityCY);

            var iSystemFillIn = new CItemToggle("FillIn", CDTXMania.ConfigIni.bFillInEnabled,
                "フィルインエフェクトの使用：\n" +
                "フィルイン区間の爆発パターンに特別" +
                "のエフェクトを使用します。\n" +
                "フィルインエフェクトの描画にはそれな" +
                "りのマシンパワーが必要とされます。",
                "To show bursting effects at the fill-in\n" +
                " zone or not.");
            iSystemFillIn.BindConfig(
                () => iSystemFillIn.bON = CDTXMania.ConfigIni.bFillInEnabled,
                () => CDTXMania.ConfigIni.bFillInEnabled = iSystemFillIn.bON);
            this.listItems.Add(iSystemFillIn); 
            
            var iSystemHitSound = new CItemToggle("HitSound", CDTXMania.ConfigIni.bドラム打音を発声する,
                "打撃音の再生：\n" +
                "これをOFFにすると、パッドを叩いた\n" +
                "ときの音を再生しなくなります（ドラム\n" +
                "のみ）。\n" +
                "DTX の音色で演奏したい場合などに\n" +
                "OFF にします。\n" +
                "\n",
                "Turn OFF if you don't want to play\n" +
                " hitting chip sound.\n" +
                "It is useful to play with real/electric\n" +
                " drums kit.\n");
            iSystemHitSound.BindConfig(
                () => iSystemHitSound.bON = CDTXMania.ConfigIni.bドラム打音を発声する,
                () => CDTXMania.ConfigIni.bドラム打音を発声する = iSystemHitSound.bON);
            this.listItems.Add(iSystemHitSound);

            var iSystemSoundMonitorDrums = new CItemToggle("DrumsMonitor", CDTXMania.ConfigIni.b演奏音を強調する.Drums,
                "ドラム音モニタ：\n" +
                "ドラム音を他の音より大きめの音量で\n" +
                "発声します。\n" +
                "ただし、オートプレイの場合は通常音\n" +
                "量で発声されます。",
                "To enhance the drums chip sound\n" +
                "(except autoplay).");
            iSystemSoundMonitorDrums.BindConfig(
                () => iSystemSoundMonitorDrums.bON = CDTXMania.ConfigIni.b演奏音を強調する.Drums,
                () => CDTXMania.ConfigIni.b演奏音を強調する.Drums = iSystemSoundMonitorDrums.bON);
            this.listItems.Add(iSystemSoundMonitorDrums);
            
            var iSystemMinComboDrums = new CItemInteger("D-MinCombo", 1, 0x1869f, CDTXMania.ConfigIni.n表示可能な最小コンボ数.Drums,
                "表示可能な最小コンボ数（ドラム）：\n" +
                "画面に表示されるコンボの最小の数\n" +
                "を指定します。\n" +
                "1 ～ 99999 の値が指定可能です。",
                "Initial number to show the combo\n" +
                " for the drums.\n" +
                "You can specify from 1 to 99999.");
            iSystemMinComboDrums.BindConfig(
                () => iSystemMinComboDrums.nCurrentValue = CDTXMania.ConfigIni.n表示可能な最小コンボ数.Drums,
                () => CDTXMania.ConfigIni.n表示可能な最小コンボ数.Drums = iSystemMinComboDrums.nCurrentValue);
            this.listItems.Add(iSystemMinComboDrums);
            
            var iDrumsHHOGraphics = new CItemList("HHOGraphics", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eHHOGraphics.Drums,
                "オープンハイハットの表示画像を変更します。\n" +
                "A: DTXMania元仕様\n" +
                "B: ○なし\n" +
                "C: クローズハットと同じ",
                "To change the graphics of open hihat.\n" +
                "A: default graphics of DTXMania\n" +
                "B: A without a circle\n" +
                "C: same as closed hihat",
                new string[] { "Type A", "Type B", "Type C" });
            iDrumsHHOGraphics.BindConfig(
                () => iDrumsHHOGraphics.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eHHOGraphics.Drums,
                () => CDTXMania.ConfigIni.eHHOGraphics.Drums = (EType)iDrumsHHOGraphics.n現在選択されている項目番号);
            this.listItems.Add(iDrumsHHOGraphics);

            var iDrumsLBDGraphics = new CItemList("LBDGraphics", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eLBDGraphics.Drums,
                "LBDチップの表示画像を変更します。\n" +
                "A: LPと同じ画像を使う\n" +
                "B: LBDとLPで色分けをする",
                "To change the graphics of left bass.\n" +
                "A: same as LP chips\n" +
                "B: In LP and LBD Color-coded.",
                new string[] { "Type A", "Type B" });
            iDrumsLBDGraphics.BindConfig(
                () => iDrumsLBDGraphics.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eLBDGraphics.Drums,
                () => CDTXMania.ConfigIni.eLBDGraphics.Drums = (EType)iDrumsLBDGraphics.n現在選択されている項目番号);
            this.listItems.Add(iDrumsLBDGraphics);

            var iDrumsJudgeLinePos = new CItemInteger("JudgeLinePos", 0, 100, CDTXMania.ConfigIni.nJudgeLine.Drums,
                "判定ラインの位置を\n" +
                "調整できます。\n" +
                "0～100の間で指定できます。",
                "To change the judgeLinePosition for the\n" +
                "You can set it from 0 to 100.");
            iDrumsJudgeLinePos.BindConfig(
                () => iDrumsJudgeLinePos.nCurrentValue = CDTXMania.ConfigIni.nJudgeLine.Drums,
                () => CDTXMania.ConfigIni.nJudgeLine.Drums = iDrumsJudgeLinePos.nCurrentValue);
            this.listItems.Add(iDrumsJudgeLinePos);

            var iDrumsShutterInPos = new CItemInteger("ShutterInPos", 0, 100, CDTXMania.ConfigIni.nShutterInSide.Drums,
                "ノーツ出現側の\n"+
                "シャッター位置を調整し\n" +
                "ノーツの見える位置を\n"+
                "制限します。",
                "\n" +
                "\n" +
                "");
            iDrumsShutterInPos.BindConfig(
                () => iDrumsShutterInPos.nCurrentValue = CDTXMania.ConfigIni.nShutterInSide.Drums,
                () => CDTXMania.ConfigIni.nShutterInSide.Drums = iDrumsShutterInPos.nCurrentValue);
            this.listItems.Add(iDrumsShutterInPos);

            var iDrumsShutterOutPos = new CItemInteger("ShutterOutPos", 0, 100, CDTXMania.ConfigIni.nShutterOutSide.Drums,
                "判定ライン側の\n" +
                "シャッター位置を調整し\n" +
                "ノーツの見える位置を\n" +
                "制限します。",
                "\n" +
                "\n" +
                "");
            iDrumsShutterOutPos.BindConfig(
                () => iDrumsShutterOutPos.nCurrentValue = CDTXMania.ConfigIni.nShutterOutSide.Drums,
                () => CDTXMania.ConfigIni.nShutterOutSide.Drums = iDrumsShutterOutPos.nCurrentValue);
            this.listItems.Add(iDrumsShutterOutPos);

            var iMutingLP = new CItemToggle("Muting LP", CDTXMania.ConfigIni.bMutingLP,
                "LPの入力で発声中のHHを\n消音します。",
                "Turn ON to let HH chips be muted\n" +
                "by LP chips.");
            iMutingLP.BindConfig(
                () => iMutingLP.bON = CDTXMania.ConfigIni.bMutingLP,
                () => CDTXMania.ConfigIni.bMutingLP = iMutingLP.bON);
            this.listItems.Add(iMutingLP);

            var iDrumsAssignToLBD = new CItemToggle("AssignToLBD", CDTXMania.ConfigIni.bAssignToLBD.Drums,
                "旧仕様のドコドコチップを\n" +
                "LBDレーンに振り分けます。\n" +
                "LP、LBDがある譜面では\n"+
                "無効になります。",
                "To move some of BassDrum chips to\n" +
                "LBD lane moderately.\n" +
                "(for old-style 2-bass DTX scores\n" +
                "without LP & LBD chips)");
            iDrumsAssignToLBD.BindConfig(
                () => iDrumsAssignToLBD.bON = CDTXMania.ConfigIni.bAssignToLBD.Drums,
                () => CDTXMania.ConfigIni.bAssignToLBD.Drums = iDrumsAssignToLBD.bON);
            this.listItems.Add(iDrumsAssignToLBD);

            var iDrumsDkdkType = new CItemList("DkdkType", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eDkdkType.Drums,
                "ツーバス譜面の仕様を変更する。\n" +
                "L R: デフォルト\n" +
                "R L: 始動足変更\n" +
                "R Only: dkdk1レーン化",
                "To change the style of double-bass-\n" +
                "concerned chips.\n" +
                "L R: default\n" +
                "R L: changes the beginning foot\n" +
                "R Only: puts bass chips into single\n" +
                "lane",
                new string[] { "L R", "R L", "R Only" });
            iDrumsDkdkType.BindConfig(
                () => iDrumsDkdkType.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eDkdkType.Drums,
                () => CDTXMania.ConfigIni.eDkdkType.Drums = (EType)iDrumsDkdkType.n現在選択されている項目番号);
            this.listItems.Add(iDrumsDkdkType);

            var iDrumsNumOfLanes = new CItemList("NumOfLanes", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eNumOfLanes.Drums,
                "10レーン譜面の仕様を変更する。\n" +
                "A: デフォルト10レーン\n" +
                "B: XG仕様9レーン\n" +
                "C: CLASSIC仕様6レーン",

                "To change the number of lanes.\n" +
                "10: default 10 lanes\n" +
                "9: XG style 9 lanes\n" +
                "6: classic style 6 lanes",
                new string[] { "10", "9", "6" });
            iDrumsNumOfLanes.BindConfig(
                () => iDrumsNumOfLanes.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eNumOfLanes.Drums,
                () => CDTXMania.ConfigIni.eNumOfLanes.Drums = (EType)iDrumsNumOfLanes.n現在選択されている項目番号);
            this.listItems.Add(iDrumsNumOfLanes);

            var iDrumsRandomPad = new CItemList("RandomPad", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eRandom.Drums,
                "ドラムのパッドチップが\n" +
                "ランダムに降ってきます。\n" +
                "Mirror:ミラーをかけます\n" +
                "Part:レーン単位で交換\n" +
                "Super:小節単位で交換\n" +
                "Hyper:1拍ごとに交換\n" +
                "Master:死ぬがよい\n" +
                "Another:丁度よくバラける",
                "Drums chips (pads) come randomly.\n" +
                "Mirror: \n" +
                "Part: swapping lanes randomly\n" +
                "Super: swapping for each measure\n" +
                "Hyper: swapping for each 1/4 measure\n" +
                "Master: game over...\n" +
                "Another: moderately swapping each\n" +
                "chip randomly",
                new string[] { "OFF", "Mirror", "Part", "Super", "Hyper", "Master", "Another" });
            iDrumsRandomPad.BindConfig(
                () => iDrumsRandomPad.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eRandom.Drums,
                () => CDTXMania.ConfigIni.eRandom.Drums = (ERandomMode)iDrumsRandomPad.n現在選択されている項目番号);
            this.listItems.Add(iDrumsRandomPad);

            var iDrumsRandomPedal = new CItemList("RandomPedal", CItemBase.EPanelType.Normal, (int)CDTXMania.ConfigIni.eRandomPedal.Drums,
                "ドラムの足チップがランダムに\n降ってきます。\n" +
                "Mirror:ミラーをかけます\n" +
                "Part:レーン単位で交換\n" +
                "Super:小節単位で交換\n" +
                "Hyper:1拍ごとに交換\n" +
                "Master:死ぬがよい\n" +
                "Another:丁度よくバラける",
                "Drums chips (pedals) come randomly.\n" +
                "Part: swapping lanes randomly\n" +
                "Super: swapping for each measure\n" +
                "Hyper: swapping for each 1/4 measure\n" +
                "Master: game over...\n" +
                "Another: moderately swapping each\n" +
                "chip randomly",
                new string[] { "OFF", "Mirror", "Part", "Super", "Hyper", "Master", "Another" });
            iDrumsRandomPedal.BindConfig(
                () => iDrumsRandomPedal.n現在選択されている項目番号 = (int)CDTXMania.ConfigIni.eRandomPedal.Drums,
                () => CDTXMania.ConfigIni.eRandomPedal.Drums = (ERandomMode)iDrumsRandomPedal.n現在選択されている項目番号);
            this.listItems.Add(iDrumsRandomPedal);

			this.iDrumsGraph = new CItemToggle( "Graph", CDTXMania.ConfigIni.bGraph有効.Drums,
				"最高スキルと比較できるグラフを表示します。\n" +
				"オートプレイだと表示されません。",
				"To draw Graph or not." );
            this.iDrumsGraph.BindConfig(
                () => this.iDrumsGraph.bON = CDTXMania.ConfigIni.bGraph有効.Drums,
                () => CDTXMania.ConfigIni.bGraph有効.Drums = this.iDrumsGraph.bON );
			this.listItems.Add( this.iDrumsGraph );
            
            // #23580 2011.1.3 yyagi
            var iDrumsInputAdjustTimeMs = new CItemInteger("InputAdjust", -99, 99, CDTXMania.ConfigIni.nInputAdjustTimeMs.Drums,
                "ドラムの入力タイミングの微調整を行います。\n" +
                "-99 ～ 99ms まで指定可能です。\n" +
                "値を指定してください。\n",
                "To adjust the drums input timing.\n" +
                "You can set from -99 to 0ms.\n" +
                "To decrease input lag, set minus value.");
            iDrumsInputAdjustTimeMs.BindConfig(
                () => iDrumsInputAdjustTimeMs.nCurrentValue = CDTXMania.ConfigIni.nInputAdjustTimeMs.Drums,
                () => CDTXMania.ConfigIni.nInputAdjustTimeMs.Drums = iDrumsInputAdjustTimeMs.nCurrentValue);
            this.listItems.Add(iDrumsInputAdjustTimeMs);

            var iDrumsGoToKeyAssign = new CItemBase("Drums Keys", CItemBase.EPanelType.Normal,
                "ドラムのキー入力に関する項目を設定します。",
                "Settings for the drums key/pad inputs.");
            iDrumsGoToKeyAssign.action = tSetupItemList_KeyAssignDrums;
            this.listItems.Add(iDrumsGoToKeyAssign);

            OnListMenuの初期化();
            this.nCurrentSelection = 0;
            this.eMenuType = EMenuType.Drums;
        }
        
        public void tSetupItemList_KeyAssignDrums()
        {
            this.listItems.Clear();

            var iKeyAssignDrumsReturnToMenu = new CItemBase("<< ReturnTo Menu", CItemBase.EPanelType.Other,
                "左側のメニューに戻ります。",
                "Return to left menu.");
            iKeyAssignDrumsReturnToMenu.action = tSetupItemList_Drums;
            this.listItems.Add(iKeyAssignDrumsReturnToMenu);

            var iKeyAssignDrumsLC = new CItemBase("LeftCymbal",
                "ドラムのキー設定：\n左シンバルへのキーの割り当てを設\n定します。",
                "Drums key assign:\nTo assign key/pads for LeftCymbal\n button.");
            iKeyAssignDrumsLC.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.LC);
            this.listItems.Add(iKeyAssignDrumsLC);

            var iKeyAssignDrumsHHC = new CItemBase("HiHat(Close)",
                "ドラムのキー設定：\nハイハット（クローズ）へのキーの割り\n当てを設定します。",
                "Drums key assign:\nTo assign key/pads for HiHat(Close)\n button.");
            iKeyAssignDrumsHHC.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.HH);
            this.listItems.Add(iKeyAssignDrumsHHC);

            var iKeyAssignDrumsHHO = new CItemBase("HiHat(Open)",
                "ドラムのキー設定：\nハイハット（オープン）へのキーの割り\n当てを設定します。",
                "Drums key assign:\nTo assign key/pads for HiHat(Open)\n button.");
            iKeyAssignDrumsHHO.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.HHO);
            this.listItems.Add(iKeyAssignDrumsHHO);

            var iKeyAssignDrumsSD = new CItemBase("Snare",
                "ドラムのキー設定：\nスネアへのキーの割り当てを設定し\nます。",
                "Drums key assign:\nTo assign key/pads for Snare button.");
            iKeyAssignDrumsSD.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.SD);
            this.listItems.Add(iKeyAssignDrumsSD);

            var iKeyAssignDrumsBD = new CItemBase("Bass",
                "ドラムのキー設定：\nバスドラムへのキーの割り当てを設定\nします。",
                "Drums key assign:\nTo assign key/pads for Bass button.");
            iKeyAssignDrumsBD.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.BD);
            this.listItems.Add(iKeyAssignDrumsBD);

            var iKeyAssignDrumsHT = new CItemBase("HighTom",
                "ドラムのキー設定：\nハイタムへのキーの割り当てを設定\nします。",
                "Drums key assign:\nTo assign key/pads for HighTom\n button.");
            iKeyAssignDrumsHT.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.HT);
            this.listItems.Add(iKeyAssignDrumsHT);

            var iKeyAssignDrumsLT = new CItemBase("LowTom",
                "ドラムのキー設定：\nロータムへのキーの割り当てを設定\nします。",
                "Drums key assign:\nTo assign key/pads for LowTom button.");
            iKeyAssignDrumsLT.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.LT);
            this.listItems.Add(iKeyAssignDrumsLT);

            var iKeyAssignDrumsFT = new CItemBase("FloorTom",
                "ドラムのキー設定：\nフロアタムへのキーの割り当てを設\n定します。",
                "Drums key assign:\nTo assign key/pads for FloorTom\n button.");
            iKeyAssignDrumsFT.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.FT);
            this.listItems.Add(iKeyAssignDrumsFT);

            var iKeyAssignDrumsCY = new CItemBase("RightCymbal",
                "ドラムのキー設定：\n右シンバルへのキーの割り当てを設\n定します。",
                "Drums key assign:\nTo assign key/pads for RightCymbal\n button.");
            iKeyAssignDrumsCY.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.CY);
            this.listItems.Add(iKeyAssignDrumsCY);

            var iKeyAssignDrumsRD = new CItemBase("RideCymbal",
                "ドラムのキー設定：\nライドシンバルへのキーの割り当て\nを設定します。",
                "Drums key assign:\nTo assign key/pads for RideCymbal\n button.");
            iKeyAssignDrumsRD.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.RD);
            this.listItems.Add(iKeyAssignDrumsRD);

            var iKeyAssignDrumsLP = new CItemBase("LeftPedal",									// #27029 2012.1.4 from
                "ドラムのキー設定：\n左ペダルへのキーの\n割り当てを設定します。",
                "Drums key assign:\nTo assign key/pads for HiHatPedal\n button.");
            iKeyAssignDrumsLP.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.LP);
            this.listItems.Add(iKeyAssignDrumsLP);

            var iKeyAssignDrumsLBD = new CItemBase("LeftBassDrum",
                "ドラムのキー設定：\n左バスドラムへのキーの割り当てを設\n定します。",
                "Drums key assign:\nTo assign key/pads for RightCymbal\n button.");
            iKeyAssignDrumsLBD.action = () => CDTXMania.stageConfig.tNotifyPadSelection(EKeyConfigPart.DRUMS, EKeyConfigPad.LBD);
            this.listItems.Add(iKeyAssignDrumsLBD);

            OnListMenuの初期化();
            this.nCurrentSelection = 0;
            this.eMenuType = EMenuType.KeyAssignDrums;
        }
        #endregion
    }
}