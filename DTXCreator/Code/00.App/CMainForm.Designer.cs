namespace DTXCreator
{
	partial class CMainForm
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CMainForm));
            this.splitContainerタブと譜面を分割 = new System.Windows.Forms.SplitContainer();
            this.tabControl情報パネル = new System.Windows.Forms.TabControl();
            this.tabPage基本情報 = new System.Windows.Forms.TabPage();
            this.buttonRESULTIMAGE参照 = new System.Windows.Forms.Button();
            this.buttonBACKGROUND参照 = new System.Windows.Forms.Button();
            this.buttonSTAGEFILE参照 = new System.Windows.Forms.Button();
            this.buttonPREIMAGE参照 = new System.Windows.Forms.Button();
            this.buttonPREVIEW参照 = new System.Windows.Forms.Button();
            this.labeRESULTIMAGE = new System.Windows.Forms.Label();
            this.labelBACKGROUND = new System.Windows.Forms.Label();
            this.labelSTAGEFILE = new System.Windows.Forms.Label();
            this.labelPREIMAGE = new System.Windows.Forms.Label();
            this.labelPREVIEW = new System.Windows.Forms.Label();
            this.textBoxRESULTIMAGE = new System.Windows.Forms.TextBox();
            this.textBoxBACKGROUND = new System.Windows.Forms.TextBox();
            this.textBoxSTAGEFILE = new System.Windows.Forms.TextBox();
            this.textBoxPREIMAGE = new System.Windows.Forms.TextBox();
            this.textBoxPREVIEW = new System.Windows.Forms.TextBox();
            this.textBoxパネル = new System.Windows.Forms.TextBox();
            this.labelパネル = new System.Windows.Forms.Label();
            this.labelBLEVEL = new System.Windows.Forms.Label();
            this.textBoxBLEVEL = new System.Windows.Forms.TextBox();
            this.hScrollBarBLEVEL = new System.Windows.Forms.HScrollBar();
            this.labelGLEVEL = new System.Windows.Forms.Label();
            this.textBoxGLEVEL = new System.Windows.Forms.TextBox();
            this.hScrollBarGLEVEL = new System.Windows.Forms.HScrollBar();
            this.labelDLEVEL = new System.Windows.Forms.Label();
            this.textBoxDLEVEL = new System.Windows.Forms.TextBox();
            this.hScrollBarDLEVEL = new System.Windows.Forms.HScrollBar();
            this.labelBPM = new System.Windows.Forms.Label();
            this.labelコメント = new System.Windows.Forms.Label();
            this.label製作者 = new System.Windows.Forms.Label();
            this.label曲名 = new System.Windows.Forms.Label();
            this.numericUpDownBPM = new System.Windows.Forms.NumericUpDown();
            this.textBoxコメント = new System.Windows.Forms.TextBox();
            this.textBox製作者 = new System.Windows.Forms.TextBox();
            this.textBox曲名 = new System.Windows.Forms.TextBox();
            this.tabPageWAV = new System.Windows.Forms.TabPage();
            this.listViewWAVリスト = new System.Windows.Forms.ListView();
            this.columnHeaderWAV_ラベル = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderWAV_番号 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderWAV_ファイル名 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderWAV_音量 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderWAV_位置 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderWAV_BGM = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStripWAVツールバー = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonWAVリスト上移動 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonWAVリスト下移動 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonWAVリストプレビュー再生開始 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonWAVリストプレビュー再生停止 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonWAVリストプレビュースイッチ = new System.Windows.Forms.ToolStripButton();
            this.tabPageBMP = new System.Windows.Forms.TabPage();
            this.listViewBMPリスト = new System.Windows.Forms.ListView();
            this.columnHeaderBMP_TEX = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBMP_ラベル = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBMP_BMP番号 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBMP_ファイル名 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStripBMPツールバー = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonBMPリスト上移動 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonBMPリスト下移動 = new System.Windows.Forms.ToolStripButton();
            this.tabPageAVI = new System.Windows.Forms.TabPage();
            this.listViewAVIリスト = new System.Windows.Forms.ListView();
            this.columnHeaderAVI_ラベル = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAVI_AVI番号 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAVI_ファイル名 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStripAVIツールバー = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAVIリスト上移動 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAVIリスト下移動 = new System.Windows.Forms.ToolStripButton();
            this.tabPage自由入力 = new System.Windows.Forms.TabPage();
            this.textBox自由入力欄 = new System.Windows.Forms.TextBox();
            this.pictureBox譜面パネル = new System.Windows.Forms.PictureBox();
            this.hScrollBar譜面用水平スクロールバー = new System.Windows.Forms.HScrollBar();
            this.statusStripステータスバー = new System.Windows.Forms.StatusStrip();
            this.menuStripメニューバー = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemファイル = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem新規 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem開く = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem上書き保存 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem名前を付けて保存 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem終了 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem編集 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemアンドゥ = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemリドゥ = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem切り取り = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemコピー = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem貼り付け = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem削除 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemすべて選択 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem選択モード = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem編集モード = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemモード切替 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem検索 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem置換 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem表示 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemチップパレット = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemガイド間隔 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemガイド間隔4分 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemガイド間隔8分 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemガイド間隔12分 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemガイド間隔16分 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemガイド間隔24分 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemガイド間隔32分 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemガイド間隔48分 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemガイド間隔64分 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemガイド間隔フリー = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemガイド間隔拡大 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemガイド間隔縮小 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem再生 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem先頭から再生 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem現在位置から再生 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem現在位置からBGMのみ再生 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem再生停止 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemツール = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemオプション = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemヘルプ = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDTXCreaterマニュアル = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemバージョン = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripツールバー = new System.Windows.Forms.ToolStrip();
            this.toolStripButton新規作成 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton開く = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton上書き保存 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton切り取り = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonコピー = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton貼り付け = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton削除 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonアンドゥ = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonリドゥ = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonチップパレット = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripComboBox譜面拡大率 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripComboBoxガイド間隔 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButton選択モード = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton編集モード = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton先頭から再生 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton現在位置から再生 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton現在位置からBGMのみ再生 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton再生停止 = new System.Windows.Forms.ToolStripButton();
            this.toolStripComboBox演奏速度 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.vScrollBar譜面用垂直スクロールバー = new System.Windows.Forms.VScrollBar();
            this.toolTipツールチップ = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip譜面右メニュー = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem選択チップの切り取り = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem選択チップのコピー = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem選択チップの貼り付け = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem選択チップの削除 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemすべてのチップの選択 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemレーン内のすべてのチップの選択 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem小節内のすべてのチップの選択 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem小節長変更 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem小節の挿入 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem小節の削除 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerタブと譜面を分割)).BeginInit();
            this.splitContainerタブと譜面を分割.Panel1.SuspendLayout();
            this.splitContainerタブと譜面を分割.Panel2.SuspendLayout();
            this.splitContainerタブと譜面を分割.SuspendLayout();
            this.tabControl情報パネル.SuspendLayout();
            this.tabPage基本情報.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBPM)).BeginInit();
            this.tabPageWAV.SuspendLayout();
            this.toolStripWAVツールバー.SuspendLayout();
            this.tabPageBMP.SuspendLayout();
            this.toolStripBMPツールバー.SuspendLayout();
            this.tabPageAVI.SuspendLayout();
            this.toolStripAVIツールバー.SuspendLayout();
            this.tabPage自由入力.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox譜面パネル)).BeginInit();
            this.menuStripメニューバー.SuspendLayout();
            this.toolStripツールバー.SuspendLayout();
            this.contextMenuStrip譜面右メニュー.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerタブと譜面を分割
            // 
            resources.ApplyResources(this.splitContainerタブと譜面を分割, "splitContainerタブと譜面を分割");
            this.splitContainerタブと譜面を分割.Name = "splitContainerタブと譜面を分割";
            // 
            // splitContainerタブと譜面を分割.Panel1
            // 
            this.splitContainerタブと譜面を分割.Panel1.Controls.Add(this.tabControl情報パネル);
            // 
            // splitContainerタブと譜面を分割.Panel2
            // 
            this.splitContainerタブと譜面を分割.Panel2.Controls.Add(this.pictureBox譜面パネル);
            this.splitContainerタブと譜面を分割.Panel2.Controls.Add(this.hScrollBar譜面用水平スクロールバー);
            resources.ApplyResources(this.splitContainerタブと譜面を分割.Panel2, "splitContainerタブと譜面を分割.Panel2");
            this.splitContainerタブと譜面を分割.Panel2.SizeChanged += new System.EventHandler(this.splitContainerタブと譜面を分割_Panel2_SizeChanged);
            // 
            // tabControl情報パネル
            // 
            this.tabControl情報パネル.Controls.Add(this.tabPage基本情報);
            this.tabControl情報パネル.Controls.Add(this.tabPageWAV);
            this.tabControl情報パネル.Controls.Add(this.tabPageBMP);
            this.tabControl情報パネル.Controls.Add(this.tabPageAVI);
            this.tabControl情報パネル.Controls.Add(this.tabPage自由入力);
            resources.ApplyResources(this.tabControl情報パネル, "tabControl情報パネル");
            this.tabControl情報パネル.Name = "tabControl情報パネル";
            this.tabControl情報パネル.SelectedIndex = 0;
            // 
            // tabPage基本情報
            // 
            this.tabPage基本情報.BackColor = System.Drawing.SystemColors.Window;
            this.tabPage基本情報.Controls.Add(this.buttonRESULTIMAGE参照);
            this.tabPage基本情報.Controls.Add(this.buttonBACKGROUND参照);
            this.tabPage基本情報.Controls.Add(this.buttonSTAGEFILE参照);
            this.tabPage基本情報.Controls.Add(this.buttonPREIMAGE参照);
            this.tabPage基本情報.Controls.Add(this.buttonPREVIEW参照);
            this.tabPage基本情報.Controls.Add(this.labeRESULTIMAGE);
            this.tabPage基本情報.Controls.Add(this.labelBACKGROUND);
            this.tabPage基本情報.Controls.Add(this.labelSTAGEFILE);
            this.tabPage基本情報.Controls.Add(this.labelPREIMAGE);
            this.tabPage基本情報.Controls.Add(this.labelPREVIEW);
            this.tabPage基本情報.Controls.Add(this.textBoxRESULTIMAGE);
            this.tabPage基本情報.Controls.Add(this.textBoxBACKGROUND);
            this.tabPage基本情報.Controls.Add(this.textBoxSTAGEFILE);
            this.tabPage基本情報.Controls.Add(this.textBoxPREIMAGE);
            this.tabPage基本情報.Controls.Add(this.textBoxPREVIEW);
            this.tabPage基本情報.Controls.Add(this.textBoxパネル);
            this.tabPage基本情報.Controls.Add(this.labelパネル);
            this.tabPage基本情報.Controls.Add(this.labelBLEVEL);
            this.tabPage基本情報.Controls.Add(this.textBoxBLEVEL);
            this.tabPage基本情報.Controls.Add(this.hScrollBarBLEVEL);
            this.tabPage基本情報.Controls.Add(this.labelGLEVEL);
            this.tabPage基本情報.Controls.Add(this.textBoxGLEVEL);
            this.tabPage基本情報.Controls.Add(this.hScrollBarGLEVEL);
            this.tabPage基本情報.Controls.Add(this.labelDLEVEL);
            this.tabPage基本情報.Controls.Add(this.textBoxDLEVEL);
            this.tabPage基本情報.Controls.Add(this.hScrollBarDLEVEL);
            this.tabPage基本情報.Controls.Add(this.labelBPM);
            this.tabPage基本情報.Controls.Add(this.labelコメント);
            this.tabPage基本情報.Controls.Add(this.label製作者);
            this.tabPage基本情報.Controls.Add(this.label曲名);
            this.tabPage基本情報.Controls.Add(this.numericUpDownBPM);
            this.tabPage基本情報.Controls.Add(this.textBoxコメント);
            this.tabPage基本情報.Controls.Add(this.textBox製作者);
            this.tabPage基本情報.Controls.Add(this.textBox曲名);
            resources.ApplyResources(this.tabPage基本情報, "tabPage基本情報");
            this.tabPage基本情報.Name = "tabPage基本情報";
            // 
            // buttonRESULTIMAGE参照
            // 
            resources.ApplyResources(this.buttonRESULTIMAGE参照, "buttonRESULTIMAGE参照");
            this.buttonRESULTIMAGE参照.Name = "buttonRESULTIMAGE参照";
            this.toolTipツールチップ.SetToolTip(this.buttonRESULTIMAGE参照, resources.GetString("buttonRESULTIMAGE参照.ToolTip"));
            this.buttonRESULTIMAGE参照.UseVisualStyleBackColor = true;
            this.buttonRESULTIMAGE参照.Click += new System.EventHandler(this.buttonRESULTIMAGE参照_Click);
            // 
            // buttonBACKGROUND参照
            // 
            resources.ApplyResources(this.buttonBACKGROUND参照, "buttonBACKGROUND参照");
            this.buttonBACKGROUND参照.Name = "buttonBACKGROUND参照";
            this.toolTipツールチップ.SetToolTip(this.buttonBACKGROUND参照, resources.GetString("buttonBACKGROUND参照.ToolTip"));
            this.buttonBACKGROUND参照.UseVisualStyleBackColor = true;
            this.buttonBACKGROUND参照.Click += new System.EventHandler(this.buttonBACKGROUND参照_Click);
            // 
            // buttonSTAGEFILE参照
            // 
            resources.ApplyResources(this.buttonSTAGEFILE参照, "buttonSTAGEFILE参照");
            this.buttonSTAGEFILE参照.Name = "buttonSTAGEFILE参照";
            this.toolTipツールチップ.SetToolTip(this.buttonSTAGEFILE参照, resources.GetString("buttonSTAGEFILE参照.ToolTip"));
            this.buttonSTAGEFILE参照.UseVisualStyleBackColor = true;
            this.buttonSTAGEFILE参照.Click += new System.EventHandler(this.buttonSTAGEFILE参照_Click);
            // 
            // buttonPREIMAGE参照
            // 
            resources.ApplyResources(this.buttonPREIMAGE参照, "buttonPREIMAGE参照");
            this.buttonPREIMAGE参照.Name = "buttonPREIMAGE参照";
            this.toolTipツールチップ.SetToolTip(this.buttonPREIMAGE参照, resources.GetString("buttonPREIMAGE参照.ToolTip"));
            this.buttonPREIMAGE参照.UseVisualStyleBackColor = true;
            this.buttonPREIMAGE参照.Click += new System.EventHandler(this.buttonPREIMAGE参照_Click);
            // 
            // buttonPREVIEW参照
            // 
            resources.ApplyResources(this.buttonPREVIEW参照, "buttonPREVIEW参照");
            this.buttonPREVIEW参照.Name = "buttonPREVIEW参照";
            this.toolTipツールチップ.SetToolTip(this.buttonPREVIEW参照, resources.GetString("buttonPREVIEW参照.ToolTip"));
            this.buttonPREVIEW参照.UseVisualStyleBackColor = true;
            this.buttonPREVIEW参照.Click += new System.EventHandler(this.buttonPREVIEW参照_Click);
            // 
            // labeRESULTIMAGE
            // 
            resources.ApplyResources(this.labeRESULTIMAGE, "labeRESULTIMAGE");
            this.labeRESULTIMAGE.Name = "labeRESULTIMAGE";
            this.toolTipツールチップ.SetToolTip(this.labeRESULTIMAGE, resources.GetString("labeRESULTIMAGE.ToolTip"));
            // 
            // labelBACKGROUND
            // 
            resources.ApplyResources(this.labelBACKGROUND, "labelBACKGROUND");
            this.labelBACKGROUND.Name = "labelBACKGROUND";
            this.toolTipツールチップ.SetToolTip(this.labelBACKGROUND, resources.GetString("labelBACKGROUND.ToolTip"));
            // 
            // labelSTAGEFILE
            // 
            resources.ApplyResources(this.labelSTAGEFILE, "labelSTAGEFILE");
            this.labelSTAGEFILE.Name = "labelSTAGEFILE";
            this.toolTipツールチップ.SetToolTip(this.labelSTAGEFILE, resources.GetString("labelSTAGEFILE.ToolTip"));
            // 
            // labelPREIMAGE
            // 
            resources.ApplyResources(this.labelPREIMAGE, "labelPREIMAGE");
            this.labelPREIMAGE.Name = "labelPREIMAGE";
            this.toolTipツールチップ.SetToolTip(this.labelPREIMAGE, resources.GetString("labelPREIMAGE.ToolTip"));
            // 
            // labelPREVIEW
            // 
            resources.ApplyResources(this.labelPREVIEW, "labelPREVIEW");
            this.labelPREVIEW.Name = "labelPREVIEW";
            this.toolTipツールチップ.SetToolTip(this.labelPREVIEW, resources.GetString("labelPREVIEW.ToolTip"));
            // 
            // textBoxRESULTIMAGE
            // 
            resources.ApplyResources(this.textBoxRESULTIMAGE, "textBoxRESULTIMAGE");
            this.textBoxRESULTIMAGE.Name = "textBoxRESULTIMAGE";
            this.toolTipツールチップ.SetToolTip(this.textBoxRESULTIMAGE, resources.GetString("textBoxRESULTIMAGE.ToolTip"));
            this.textBoxRESULTIMAGE.TextChanged += new System.EventHandler(this.textBoxRESULTIMAGE_TextChanged);
            this.textBoxRESULTIMAGE.Leave += new System.EventHandler(this.textBoxRESULTIMAGE_Leave);
            // 
            // textBoxBACKGROUND
            // 
            resources.ApplyResources(this.textBoxBACKGROUND, "textBoxBACKGROUND");
            this.textBoxBACKGROUND.Name = "textBoxBACKGROUND";
            this.toolTipツールチップ.SetToolTip(this.textBoxBACKGROUND, resources.GetString("textBoxBACKGROUND.ToolTip"));
            this.textBoxBACKGROUND.TextChanged += new System.EventHandler(this.textBoxBACKGROUND_TextChanged);
            this.textBoxBACKGROUND.Leave += new System.EventHandler(this.textBoxBACKGROUND_Leave);
            // 
            // textBoxSTAGEFILE
            // 
            resources.ApplyResources(this.textBoxSTAGEFILE, "textBoxSTAGEFILE");
            this.textBoxSTAGEFILE.Name = "textBoxSTAGEFILE";
            this.toolTipツールチップ.SetToolTip(this.textBoxSTAGEFILE, resources.GetString("textBoxSTAGEFILE.ToolTip"));
            this.textBoxSTAGEFILE.TextChanged += new System.EventHandler(this.textBoxSTAGEFILE_TextChanged);
            this.textBoxSTAGEFILE.Leave += new System.EventHandler(this.textBoxSTAGEFILE_Leave);
            // 
            // textBoxPREIMAGE
            // 
            resources.ApplyResources(this.textBoxPREIMAGE, "textBoxPREIMAGE");
            this.textBoxPREIMAGE.Name = "textBoxPREIMAGE";
            this.toolTipツールチップ.SetToolTip(this.textBoxPREIMAGE, resources.GetString("textBoxPREIMAGE.ToolTip"));
            this.textBoxPREIMAGE.TextChanged += new System.EventHandler(this.textBoxPREIMAGE_TextChanged);
            this.textBoxPREIMAGE.Leave += new System.EventHandler(this.textBoxPREIMAGE_Leave);
            // 
            // textBoxPREVIEW
            // 
            resources.ApplyResources(this.textBoxPREVIEW, "textBoxPREVIEW");
            this.textBoxPREVIEW.Name = "textBoxPREVIEW";
            this.toolTipツールチップ.SetToolTip(this.textBoxPREVIEW, resources.GetString("textBoxPREVIEW.ToolTip"));
            this.textBoxPREVIEW.TextChanged += new System.EventHandler(this.textBoxPREVIEW_TextChanged);
            this.textBoxPREVIEW.Leave += new System.EventHandler(this.textBoxPREVIEW_Leave);
            // 
            // textBoxパネル
            // 
            resources.ApplyResources(this.textBoxパネル, "textBoxパネル");
            this.textBoxパネル.Name = "textBoxパネル";
            this.toolTipツールチップ.SetToolTip(this.textBoxパネル, resources.GetString("textBoxパネル.ToolTip"));
            this.textBoxパネル.TextChanged += new System.EventHandler(this.textBoxパネル_TextChanged);
            this.textBoxパネル.Leave += new System.EventHandler(this.textBoxパネル_Leave);
            // 
            // labelパネル
            // 
            resources.ApplyResources(this.labelパネル, "labelパネル");
            this.labelパネル.Name = "labelパネル";
            this.toolTipツールチップ.SetToolTip(this.labelパネル, resources.GetString("labelパネル.ToolTip"));
            // 
            // labelBLEVEL
            // 
            resources.ApplyResources(this.labelBLEVEL, "labelBLEVEL");
            this.labelBLEVEL.Name = "labelBLEVEL";
            this.toolTipツールチップ.SetToolTip(this.labelBLEVEL, resources.GetString("labelBLEVEL.ToolTip"));
            // 
            // textBoxBLEVEL
            // 
            resources.ApplyResources(this.textBoxBLEVEL, "textBoxBLEVEL");
            this.textBoxBLEVEL.Name = "textBoxBLEVEL";
            this.toolTipツールチップ.SetToolTip(this.textBoxBLEVEL, resources.GetString("textBoxBLEVEL.ToolTip"));
            this.textBoxBLEVEL.TextChanged += new System.EventHandler(this.textBoxBLEVEL_TextChanged);
            this.textBoxBLEVEL.Leave += new System.EventHandler(this.textBoxBLEVEL_Leave);
            // 
            // hScrollBarBLEVEL
            // 
            resources.ApplyResources(this.hScrollBarBLEVEL, "hScrollBarBLEVEL");
            this.hScrollBarBLEVEL.Maximum = 999;
            this.hScrollBarBLEVEL.Name = "hScrollBarBLEVEL";
            this.toolTipツールチップ.SetToolTip(this.hScrollBarBLEVEL, resources.GetString("hScrollBarBLEVEL.ToolTip"));
            this.hScrollBarBLEVEL.ValueChanged += new System.EventHandler(this.hScrollBarBLEVEL_ValueChanged);
            // 
            // labelGLEVEL
            // 
            resources.ApplyResources(this.labelGLEVEL, "labelGLEVEL");
            this.labelGLEVEL.Name = "labelGLEVEL";
            this.toolTipツールチップ.SetToolTip(this.labelGLEVEL, resources.GetString("labelGLEVEL.ToolTip"));
            // 
            // textBoxGLEVEL
            // 
            resources.ApplyResources(this.textBoxGLEVEL, "textBoxGLEVEL");
            this.textBoxGLEVEL.Name = "textBoxGLEVEL";
            this.toolTipツールチップ.SetToolTip(this.textBoxGLEVEL, resources.GetString("textBoxGLEVEL.ToolTip"));
            this.textBoxGLEVEL.TextChanged += new System.EventHandler(this.textBoxGLEVEL_TextChanged);
            this.textBoxGLEVEL.Leave += new System.EventHandler(this.textBoxGLEVEL_Leave);
            // 
            // hScrollBarGLEVEL
            // 
            resources.ApplyResources(this.hScrollBarGLEVEL, "hScrollBarGLEVEL");
            this.hScrollBarGLEVEL.Maximum = 999;
            this.hScrollBarGLEVEL.Name = "hScrollBarGLEVEL";
            this.toolTipツールチップ.SetToolTip(this.hScrollBarGLEVEL, resources.GetString("hScrollBarGLEVEL.ToolTip"));
            this.hScrollBarGLEVEL.ValueChanged += new System.EventHandler(this.hScrollBarGLEVEL_ValueChanged);
            // 
            // labelDLEVEL
            // 
            resources.ApplyResources(this.labelDLEVEL, "labelDLEVEL");
            this.labelDLEVEL.Name = "labelDLEVEL";
            this.toolTipツールチップ.SetToolTip(this.labelDLEVEL, resources.GetString("labelDLEVEL.ToolTip"));
            // 
            // textBoxDLEVEL
            // 
            resources.ApplyResources(this.textBoxDLEVEL, "textBoxDLEVEL");
            this.textBoxDLEVEL.Name = "textBoxDLEVEL";
            this.toolTipツールチップ.SetToolTip(this.textBoxDLEVEL, resources.GetString("textBoxDLEVEL.ToolTip"));
            this.textBoxDLEVEL.TextChanged += new System.EventHandler(this.textBoxDLEVEL_TextChanged);
            this.textBoxDLEVEL.Leave += new System.EventHandler(this.textBoxDLEVEL_Leave);
            // 
            // hScrollBarDLEVEL
            // 
            resources.ApplyResources(this.hScrollBarDLEVEL, "hScrollBarDLEVEL");
            this.hScrollBarDLEVEL.Maximum = 999;
            this.hScrollBarDLEVEL.Name = "hScrollBarDLEVEL";
            this.toolTipツールチップ.SetToolTip(this.hScrollBarDLEVEL, resources.GetString("hScrollBarDLEVEL.ToolTip"));
            this.hScrollBarDLEVEL.ValueChanged += new System.EventHandler(this.hScrollBarDLEVEL_ValueChanged);
            // 
            // labelBPM
            // 
            resources.ApplyResources(this.labelBPM, "labelBPM");
            this.labelBPM.Name = "labelBPM";
            this.toolTipツールチップ.SetToolTip(this.labelBPM, resources.GetString("labelBPM.ToolTip"));
            // 
            // labelコメント
            // 
            resources.ApplyResources(this.labelコメント, "labelコメント");
            this.labelコメント.Name = "labelコメント";
            this.toolTipツールチップ.SetToolTip(this.labelコメント, resources.GetString("labelコメント.ToolTip"));
            // 
            // label製作者
            // 
            resources.ApplyResources(this.label製作者, "label製作者");
            this.label製作者.Name = "label製作者";
            this.toolTipツールチップ.SetToolTip(this.label製作者, resources.GetString("label製作者.ToolTip"));
            // 
            // label曲名
            // 
            resources.ApplyResources(this.label曲名, "label曲名");
            this.label曲名.Name = "label曲名";
            this.toolTipツールチップ.SetToolTip(this.label曲名, resources.GetString("label曲名.ToolTip"));
            // 
            // numericUpDownBPM
            // 
            this.numericUpDownBPM.DecimalPlaces = 2;
            resources.ApplyResources(this.numericUpDownBPM, "numericUpDownBPM");
            this.numericUpDownBPM.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownBPM.Name = "numericUpDownBPM";
            this.toolTipツールチップ.SetToolTip(this.numericUpDownBPM, resources.GetString("numericUpDownBPM.ToolTip"));
            this.numericUpDownBPM.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numericUpDownBPM.ValueChanged += new System.EventHandler(this.numericUpDownBPM_ValueChanged);
            this.numericUpDownBPM.Leave += new System.EventHandler(this.numericUpDownBPM_Leave);
            // 
            // textBoxコメント
            // 
            resources.ApplyResources(this.textBoxコメント, "textBoxコメント");
            this.textBoxコメント.Name = "textBoxコメント";
            this.toolTipツールチップ.SetToolTip(this.textBoxコメント, resources.GetString("textBoxコメント.ToolTip"));
            this.textBoxコメント.TextChanged += new System.EventHandler(this.textBoxコメント_TextChanged);
            this.textBoxコメント.Leave += new System.EventHandler(this.textBoxコメント_Leave);
            // 
            // textBox製作者
            // 
            resources.ApplyResources(this.textBox製作者, "textBox製作者");
            this.textBox製作者.Name = "textBox製作者";
            this.toolTipツールチップ.SetToolTip(this.textBox製作者, resources.GetString("textBox製作者.ToolTip"));
            this.textBox製作者.TextChanged += new System.EventHandler(this.textBox製作者_TextChanged);
            this.textBox製作者.Leave += new System.EventHandler(this.textBox製作者_Leave);
            // 
            // textBox曲名
            // 
            resources.ApplyResources(this.textBox曲名, "textBox曲名");
            this.textBox曲名.Name = "textBox曲名";
            this.toolTipツールチップ.SetToolTip(this.textBox曲名, resources.GetString("textBox曲名.ToolTip"));
            this.textBox曲名.TextChanged += new System.EventHandler(this.textBox曲名_TextChanged);
            this.textBox曲名.Leave += new System.EventHandler(this.textBox曲名_Leave);
            // 
            // tabPageWAV
            // 
            this.tabPageWAV.Controls.Add(this.listViewWAVリスト);
            this.tabPageWAV.Controls.Add(this.toolStripWAVツールバー);
            resources.ApplyResources(this.tabPageWAV, "tabPageWAV");
            this.tabPageWAV.Name = "tabPageWAV";
            this.tabPageWAV.UseVisualStyleBackColor = true;
            // 
            // listViewWAVリスト
            // 
            this.listViewWAVリスト.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderWAV_ラベル,
            this.columnHeaderWAV_番号,
            this.columnHeaderWAV_ファイル名,
            this.columnHeaderWAV_音量,
            this.columnHeaderWAV_位置,
            this.columnHeaderWAV_BGM});
            resources.ApplyResources(this.listViewWAVリスト, "listViewWAVリスト");
            this.listViewWAVリスト.FullRowSelect = true;
            this.listViewWAVリスト.GridLines = true;
            this.listViewWAVリスト.HideSelection = false;
            this.listViewWAVリスト.MultiSelect = false;
            this.listViewWAVリスト.Name = "listViewWAVリスト";
            this.listViewWAVリスト.UseCompatibleStateImageBehavior = false;
            this.listViewWAVリスト.View = System.Windows.Forms.View.Details;
            this.listViewWAVリスト.VirtualListSize = 1295;
            this.listViewWAVリスト.VirtualMode = true;
            this.listViewWAVリスト.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewWAVリスト_ItemDrag);
            this.listViewWAVリスト.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.listViewWAVリスト_RetrieveVirtualItem);
            this.listViewWAVリスト.SelectedIndexChanged += new System.EventHandler(this.listViewWAVリスト_SelectedIndexChanged);
            this.listViewWAVリスト.Click += new System.EventHandler(this.listViewWAVリスト_Click);
            this.listViewWAVリスト.DoubleClick += new System.EventHandler(this.listViewWAVリスト_DoubleClick);
            this.listViewWAVリスト.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listViewWAVリスト_KeyPress);
            this.listViewWAVリスト.MouseEnter += new System.EventHandler(this.listViewWAVリスト_MouseEnter);
            // 
            // columnHeaderWAV_ラベル
            // 
            resources.ApplyResources(this.columnHeaderWAV_ラベル, "columnHeaderWAV_ラベル");
            // 
            // columnHeaderWAV_番号
            // 
            resources.ApplyResources(this.columnHeaderWAV_番号, "columnHeaderWAV_番号");
            // 
            // columnHeaderWAV_ファイル名
            // 
            resources.ApplyResources(this.columnHeaderWAV_ファイル名, "columnHeaderWAV_ファイル名");
            // 
            // columnHeaderWAV_音量
            // 
            resources.ApplyResources(this.columnHeaderWAV_音量, "columnHeaderWAV_音量");
            // 
            // columnHeaderWAV_位置
            // 
            resources.ApplyResources(this.columnHeaderWAV_位置, "columnHeaderWAV_位置");
            // 
            // columnHeaderWAV_BGM
            // 
            resources.ApplyResources(this.columnHeaderWAV_BGM, "columnHeaderWAV_BGM");
            // 
            // toolStripWAVツールバー
            // 
            resources.ApplyResources(this.toolStripWAVツールバー, "toolStripWAVツールバー");
            this.toolStripWAVツールバー.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripWAVツールバー.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonWAVリスト上移動,
            this.toolStripButtonWAVリスト下移動,
            this.toolStripSeparator13,
            this.toolStripButtonWAVリストプレビュー再生開始,
            this.toolStripButtonWAVリストプレビュー再生停止,
            this.toolStripSeparator14,
            this.toolStripButtonWAVリストプレビュースイッチ});
            this.toolStripWAVツールバー.Name = "toolStripWAVツールバー";
            // 
            // toolStripButtonWAVリスト上移動
            // 
            this.toolStripButtonWAVリスト上移動.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonWAVリスト上移動.Image = global::DTXCreator.Properties.Resources.上移動;
            resources.ApplyResources(this.toolStripButtonWAVリスト上移動, "toolStripButtonWAVリスト上移動");
            this.toolStripButtonWAVリスト上移動.Name = "toolStripButtonWAVリスト上移動";
            this.toolStripButtonWAVリスト上移動.Click += new System.EventHandler(this.toolStripButtonWAVリスト上移動_Click);
            // 
            // toolStripButtonWAVリスト下移動
            // 
            this.toolStripButtonWAVリスト下移動.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonWAVリスト下移動.Image = global::DTXCreator.Properties.Resources.下移動;
            resources.ApplyResources(this.toolStripButtonWAVリスト下移動, "toolStripButtonWAVリスト下移動");
            this.toolStripButtonWAVリスト下移動.Name = "toolStripButtonWAVリスト下移動";
            this.toolStripButtonWAVリスト下移動.Click += new System.EventHandler(this.toolStripButtonWAVリスト下移動_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            resources.ApplyResources(this.toolStripSeparator13, "toolStripSeparator13");
            // 
            // toolStripButtonWAVリストプレビュー再生開始
            // 
            this.toolStripButtonWAVリストプレビュー再生開始.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonWAVリストプレビュー再生開始.Image = global::DTXCreator.Properties.Resources.再生;
            resources.ApplyResources(this.toolStripButtonWAVリストプレビュー再生開始, "toolStripButtonWAVリストプレビュー再生開始");
            this.toolStripButtonWAVリストプレビュー再生開始.Name = "toolStripButtonWAVリストプレビュー再生開始";
            this.toolStripButtonWAVリストプレビュー再生開始.Click += new System.EventHandler(this.toolStripButtonWAVリストプレビュー再生開始_Click);
            // 
            // toolStripButtonWAVリストプレビュー再生停止
            // 
            this.toolStripButtonWAVリストプレビュー再生停止.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonWAVリストプレビュー再生停止.Image = global::DTXCreator.Properties.Resources.再生停止;
            resources.ApplyResources(this.toolStripButtonWAVリストプレビュー再生停止, "toolStripButtonWAVリストプレビュー再生停止");
            this.toolStripButtonWAVリストプレビュー再生停止.Name = "toolStripButtonWAVリストプレビュー再生停止";
            this.toolStripButtonWAVリストプレビュー再生停止.Click += new System.EventHandler(this.toolStripButtonWAVリストプレビュー再生停止_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            resources.ApplyResources(this.toolStripSeparator14, "toolStripSeparator14");
            // 
            // toolStripButtonWAVリストプレビュースイッチ
            // 
            this.toolStripButtonWAVリストプレビュースイッチ.Checked = true;
            this.toolStripButtonWAVリストプレビュースイッチ.CheckOnClick = true;
            this.toolStripButtonWAVリストプレビュースイッチ.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonWAVリストプレビュースイッチ.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonWAVリストプレビュースイッチ.Image = global::DTXCreator.Properties.Resources.スピーカー;
            resources.ApplyResources(this.toolStripButtonWAVリストプレビュースイッチ, "toolStripButtonWAVリストプレビュースイッチ");
            this.toolStripButtonWAVリストプレビュースイッチ.Name = "toolStripButtonWAVリストプレビュースイッチ";
            this.toolStripButtonWAVリストプレビュースイッチ.CheckStateChanged += new System.EventHandler(this.toolStripButtonWAVリストプレビュースイッチ_CheckStateChanged);
            // 
            // tabPageBMP
            // 
            this.tabPageBMP.Controls.Add(this.listViewBMPリスト);
            this.tabPageBMP.Controls.Add(this.toolStripBMPツールバー);
            resources.ApplyResources(this.tabPageBMP, "tabPageBMP");
            this.tabPageBMP.Name = "tabPageBMP";
            this.tabPageBMP.UseVisualStyleBackColor = true;
            // 
            // listViewBMPリスト
            // 
            this.listViewBMPリスト.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderBMP_TEX,
            this.columnHeaderBMP_ラベル,
            this.columnHeaderBMP_BMP番号,
            this.columnHeaderBMP_ファイル名});
            resources.ApplyResources(this.listViewBMPリスト, "listViewBMPリスト");
            this.listViewBMPリスト.FullRowSelect = true;
            this.listViewBMPリスト.GridLines = true;
            this.listViewBMPリスト.HideSelection = false;
            this.listViewBMPリスト.MultiSelect = false;
            this.listViewBMPリスト.Name = "listViewBMPリスト";
            this.listViewBMPリスト.UseCompatibleStateImageBehavior = false;
            this.listViewBMPリスト.View = System.Windows.Forms.View.Details;
            this.listViewBMPリスト.VirtualListSize = 1295;
            this.listViewBMPリスト.VirtualMode = true;
            this.listViewBMPリスト.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewBMPリスト_ItemDrag);
            this.listViewBMPリスト.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.listViewBMPリスト_RetrieveVirtualItem);
            this.listViewBMPリスト.SelectedIndexChanged += new System.EventHandler(this.listViewBMPリスト_SelectedIndexChanged);
            this.listViewBMPリスト.Click += new System.EventHandler(this.listViewBMPリスト_Click);
            this.listViewBMPリスト.DoubleClick += new System.EventHandler(this.listViewBMPリスト_DoubleClick);
            this.listViewBMPリスト.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listViewBMPリスト_KeyPress);
            this.listViewBMPリスト.MouseEnter += new System.EventHandler(this.listViewBMPリスト_MouseEnter);
            // 
            // columnHeaderBMP_TEX
            // 
            resources.ApplyResources(this.columnHeaderBMP_TEX, "columnHeaderBMP_TEX");
            // 
            // columnHeaderBMP_ラベル
            // 
            resources.ApplyResources(this.columnHeaderBMP_ラベル, "columnHeaderBMP_ラベル");
            // 
            // columnHeaderBMP_BMP番号
            // 
            resources.ApplyResources(this.columnHeaderBMP_BMP番号, "columnHeaderBMP_BMP番号");
            // 
            // columnHeaderBMP_ファイル名
            // 
            resources.ApplyResources(this.columnHeaderBMP_ファイル名, "columnHeaderBMP_ファイル名");
            // 
            // toolStripBMPツールバー
            // 
            resources.ApplyResources(this.toolStripBMPツールバー, "toolStripBMPツールバー");
            this.toolStripBMPツールバー.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripBMPツールバー.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonBMPリスト上移動,
            this.toolStripButtonBMPリスト下移動});
            this.toolStripBMPツールバー.Name = "toolStripBMPツールバー";
            this.toolStripBMPツールバー.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // toolStripButtonBMPリスト上移動
            // 
            this.toolStripButtonBMPリスト上移動.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonBMPリスト上移動.Image = global::DTXCreator.Properties.Resources.上移動;
            resources.ApplyResources(this.toolStripButtonBMPリスト上移動, "toolStripButtonBMPリスト上移動");
            this.toolStripButtonBMPリスト上移動.Name = "toolStripButtonBMPリスト上移動";
            this.toolStripButtonBMPリスト上移動.Click += new System.EventHandler(this.toolStripButtonBMPリスト上移動_Click);
            // 
            // toolStripButtonBMPリスト下移動
            // 
            this.toolStripButtonBMPリスト下移動.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonBMPリスト下移動.Image = global::DTXCreator.Properties.Resources.下移動;
            resources.ApplyResources(this.toolStripButtonBMPリスト下移動, "toolStripButtonBMPリスト下移動");
            this.toolStripButtonBMPリスト下移動.Name = "toolStripButtonBMPリスト下移動";
            this.toolStripButtonBMPリスト下移動.Click += new System.EventHandler(this.toolStripButtonBMPリスト下移動_Click);
            // 
            // tabPageAVI
            // 
            this.tabPageAVI.Controls.Add(this.listViewAVIリスト);
            this.tabPageAVI.Controls.Add(this.toolStripAVIツールバー);
            resources.ApplyResources(this.tabPageAVI, "tabPageAVI");
            this.tabPageAVI.Name = "tabPageAVI";
            this.tabPageAVI.UseVisualStyleBackColor = true;
            // 
            // listViewAVIリスト
            // 
            this.listViewAVIリスト.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderAVI_ラベル,
            this.columnHeaderAVI_AVI番号,
            this.columnHeaderAVI_ファイル名});
            resources.ApplyResources(this.listViewAVIリスト, "listViewAVIリスト");
            this.listViewAVIリスト.FullRowSelect = true;
            this.listViewAVIリスト.GridLines = true;
            this.listViewAVIリスト.HideSelection = false;
            this.listViewAVIリスト.MultiSelect = false;
            this.listViewAVIリスト.Name = "listViewAVIリスト";
            this.listViewAVIリスト.UseCompatibleStateImageBehavior = false;
            this.listViewAVIリスト.View = System.Windows.Forms.View.Details;
            this.listViewAVIリスト.VirtualListSize = 1295;
            this.listViewAVIリスト.VirtualMode = true;
            this.listViewAVIリスト.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewAVIリスト_ItemDrag);
            this.listViewAVIリスト.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.listViewAVIリスト_RetrieveVirtualItem);
            this.listViewAVIリスト.SelectedIndexChanged += new System.EventHandler(this.listViewAVIリスト_SelectedIndexChanged);
            this.listViewAVIリスト.Click += new System.EventHandler(this.listViewAVIリスト_Click);
            this.listViewAVIリスト.DoubleClick += new System.EventHandler(this.listViewAVIリスト_DoubleClick);
            this.listViewAVIリスト.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listViewAVIリスト_KeyPress);
            this.listViewAVIリスト.MouseEnter += new System.EventHandler(this.listViewAVIリスト_MouseEnter);
            // 
            // columnHeaderAVI_ラベル
            // 
            resources.ApplyResources(this.columnHeaderAVI_ラベル, "columnHeaderAVI_ラベル");
            // 
            // columnHeaderAVI_AVI番号
            // 
            resources.ApplyResources(this.columnHeaderAVI_AVI番号, "columnHeaderAVI_AVI番号");
            // 
            // columnHeaderAVI_ファイル名
            // 
            resources.ApplyResources(this.columnHeaderAVI_ファイル名, "columnHeaderAVI_ファイル名");
            // 
            // toolStripAVIツールバー
            // 
            resources.ApplyResources(this.toolStripAVIツールバー, "toolStripAVIツールバー");
            this.toolStripAVIツールバー.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripAVIツールバー.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAVIリスト上移動,
            this.toolStripButtonAVIリスト下移動});
            this.toolStripAVIツールバー.Name = "toolStripAVIツールバー";
            // 
            // toolStripButtonAVIリスト上移動
            // 
            this.toolStripButtonAVIリスト上移動.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAVIリスト上移動.Image = global::DTXCreator.Properties.Resources.上移動;
            resources.ApplyResources(this.toolStripButtonAVIリスト上移動, "toolStripButtonAVIリスト上移動");
            this.toolStripButtonAVIリスト上移動.Name = "toolStripButtonAVIリスト上移動";
            this.toolStripButtonAVIリスト上移動.Click += new System.EventHandler(this.toolStripButtonAVIリスト上移動_Click);
            // 
            // toolStripButtonAVIリスト下移動
            // 
            this.toolStripButtonAVIリスト下移動.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAVIリスト下移動.Image = global::DTXCreator.Properties.Resources.下移動;
            resources.ApplyResources(this.toolStripButtonAVIリスト下移動, "toolStripButtonAVIリスト下移動");
            this.toolStripButtonAVIリスト下移動.Name = "toolStripButtonAVIリスト下移動";
            this.toolStripButtonAVIリスト下移動.Click += new System.EventHandler(this.toolStripButtonAVIリスト下移動_Click);
            // 
            // tabPage自由入力
            // 
            this.tabPage自由入力.Controls.Add(this.textBox自由入力欄);
            resources.ApplyResources(this.tabPage自由入力, "tabPage自由入力");
            this.tabPage自由入力.Name = "tabPage自由入力";
            this.tabPage自由入力.UseVisualStyleBackColor = true;
            // 
            // textBox自由入力欄
            // 
            this.textBox自由入力欄.AcceptsReturn = true;
            this.textBox自由入力欄.AcceptsTab = true;
            resources.ApplyResources(this.textBox自由入力欄, "textBox自由入力欄");
            this.textBox自由入力欄.Name = "textBox自由入力欄";
            this.textBox自由入力欄.TextChanged += new System.EventHandler(this.textBox自由入力欄_TextChanged);
            this.textBox自由入力欄.Leave += new System.EventHandler(this.textBox自由入力欄_Leave);
            // 
            // pictureBox譜面パネル
            // 
            resources.ApplyResources(this.pictureBox譜面パネル, "pictureBox譜面パネル");
            this.pictureBox譜面パネル.BackColor = System.Drawing.Color.Black;
            this.pictureBox譜面パネル.Name = "pictureBox譜面パネル";
            this.pictureBox譜面パネル.TabStop = false;
            this.pictureBox譜面パネル.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox譜面パネル_Paint);
            this.pictureBox譜面パネル.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox譜面パネル_MouseClick);
            this.pictureBox譜面パネル.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox譜面パネル_MouseDown);
            this.pictureBox譜面パネル.MouseEnter += new System.EventHandler(this.pictureBox譜面パネル_MouseEnter);
            this.pictureBox譜面パネル.MouseLeave += new System.EventHandler(this.pictureBox譜面パネル_MouseLeave);
            this.pictureBox譜面パネル.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox譜面パネル_MouseMove);
            this.pictureBox譜面パネル.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.pictureBox譜面パネル_PreviewKeyDown);
            // 
            // hScrollBar譜面用水平スクロールバー
            // 
            resources.ApplyResources(this.hScrollBar譜面用水平スクロールバー, "hScrollBar譜面用水平スクロールバー");
            this.hScrollBar譜面用水平スクロールバー.Name = "hScrollBar譜面用水平スクロールバー";
            this.hScrollBar譜面用水平スクロールバー.SmallChange = 5;
            this.hScrollBar譜面用水平スクロールバー.ValueChanged += new System.EventHandler(this.hScrollBar譜面用水平スクロールバー_ValueChanged);
            // 
            // statusStripステータスバー
            // 
            resources.ApplyResources(this.statusStripステータスバー, "statusStripステータスバー");
            this.statusStripステータスバー.Name = "statusStripステータスバー";
            // 
            // menuStripメニューバー
            // 
            this.menuStripメニューバー.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemファイル,
            this.toolStripMenuItem編集,
            this.toolStripMenuItem表示,
            this.toolStripMenuItem再生,
            this.toolStripMenuItemツール,
            this.toolStripMenuItemヘルプ});
            resources.ApplyResources(this.menuStripメニューバー, "menuStripメニューバー");
            this.menuStripメニューバー.Name = "menuStripメニューバー";
            // 
            // toolStripMenuItemファイル
            // 
            this.toolStripMenuItemファイル.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem新規,
            this.toolStripMenuItem開く,
            this.toolStripMenuItem上書き保存,
            this.toolStripMenuItem名前を付けて保存,
            this.toolStripSeparator1,
            this.toolStripMenuItem終了});
            this.toolStripMenuItemファイル.Name = "toolStripMenuItemファイル";
            resources.ApplyResources(this.toolStripMenuItemファイル, "toolStripMenuItemファイル");
            // 
            // toolStripMenuItem新規
            // 
            this.toolStripMenuItem新規.Image = global::DTXCreator.Properties.Resources.新規作成;
            this.toolStripMenuItem新規.Name = "toolStripMenuItem新規";
            resources.ApplyResources(this.toolStripMenuItem新規, "toolStripMenuItem新規");
            this.toolStripMenuItem新規.Click += new System.EventHandler(this.toolStripMenuItem新規_Click);
            // 
            // toolStripMenuItem開く
            // 
            this.toolStripMenuItem開く.Image = global::DTXCreator.Properties.Resources.開く;
            this.toolStripMenuItem開く.Name = "toolStripMenuItem開く";
            resources.ApplyResources(this.toolStripMenuItem開く, "toolStripMenuItem開く");
            this.toolStripMenuItem開く.Click += new System.EventHandler(this.toolStripMenuItem開く_Click);
            // 
            // toolStripMenuItem上書き保存
            // 
            this.toolStripMenuItem上書き保存.Image = global::DTXCreator.Properties.Resources.保存;
            this.toolStripMenuItem上書き保存.Name = "toolStripMenuItem上書き保存";
            resources.ApplyResources(this.toolStripMenuItem上書き保存, "toolStripMenuItem上書き保存");
            this.toolStripMenuItem上書き保存.Click += new System.EventHandler(this.toolStripMenuItem上書き保存_Click);
            // 
            // toolStripMenuItem名前を付けて保存
            // 
            this.toolStripMenuItem名前を付けて保存.Name = "toolStripMenuItem名前を付けて保存";
            resources.ApplyResources(this.toolStripMenuItem名前を付けて保存, "toolStripMenuItem名前を付けて保存");
            this.toolStripMenuItem名前を付けて保存.Click += new System.EventHandler(this.toolStripMenuItem名前を付けて保存_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripMenuItem終了
            // 
            this.toolStripMenuItem終了.Name = "toolStripMenuItem終了";
            resources.ApplyResources(this.toolStripMenuItem終了, "toolStripMenuItem終了");
            this.toolStripMenuItem終了.Click += new System.EventHandler(this.toolStripMenuItem終了_Click);
            // 
            // toolStripMenuItem編集
            // 
            this.toolStripMenuItem編集.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemアンドゥ,
            this.toolStripMenuItemリドゥ,
            this.toolStripSeparator2,
            this.toolStripMenuItem切り取り,
            this.toolStripMenuItemコピー,
            this.toolStripMenuItem貼り付け,
            this.toolStripMenuItem削除,
            this.toolStripSeparator3,
            this.toolStripMenuItemすべて選択,
            this.toolStripSeparator4,
            this.toolStripMenuItem選択モード,
            this.toolStripMenuItem編集モード,
            this.toolStripMenuItemモード切替,
            this.toolStripSeparator5,
            this.toolStripMenuItem検索,
            this.toolStripMenuItem置換});
            this.toolStripMenuItem編集.Name = "toolStripMenuItem編集";
            resources.ApplyResources(this.toolStripMenuItem編集, "toolStripMenuItem編集");
            // 
            // toolStripMenuItemアンドゥ
            // 
            this.toolStripMenuItemアンドゥ.Image = global::DTXCreator.Properties.Resources.Undo;
            this.toolStripMenuItemアンドゥ.Name = "toolStripMenuItemアンドゥ";
            resources.ApplyResources(this.toolStripMenuItemアンドゥ, "toolStripMenuItemアンドゥ");
            this.toolStripMenuItemアンドゥ.Click += new System.EventHandler(this.toolStripMenuItemアンドゥ_Click);
            // 
            // toolStripMenuItemリドゥ
            // 
            this.toolStripMenuItemリドゥ.Image = global::DTXCreator.Properties.Resources.Redo;
            this.toolStripMenuItemリドゥ.Name = "toolStripMenuItemリドゥ";
            resources.ApplyResources(this.toolStripMenuItemリドゥ, "toolStripMenuItemリドゥ");
            this.toolStripMenuItemリドゥ.Click += new System.EventHandler(this.toolStripMenuItemリドゥ_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // toolStripMenuItem切り取り
            // 
            resources.ApplyResources(this.toolStripMenuItem切り取り, "toolStripMenuItem切り取り");
            this.toolStripMenuItem切り取り.Image = global::DTXCreator.Properties.Resources.切り取り;
            this.toolStripMenuItem切り取り.Name = "toolStripMenuItem切り取り";
            this.toolStripMenuItem切り取り.Click += new System.EventHandler(this.toolStripMenuItem切り取り_Click);
            // 
            // toolStripMenuItemコピー
            // 
            resources.ApplyResources(this.toolStripMenuItemコピー, "toolStripMenuItemコピー");
            this.toolStripMenuItemコピー.Image = global::DTXCreator.Properties.Resources.コピー;
            this.toolStripMenuItemコピー.Name = "toolStripMenuItemコピー";
            this.toolStripMenuItemコピー.Click += new System.EventHandler(this.toolStripMenuItemコピー_Click);
            // 
            // toolStripMenuItem貼り付け
            // 
            resources.ApplyResources(this.toolStripMenuItem貼り付け, "toolStripMenuItem貼り付け");
            this.toolStripMenuItem貼り付け.Image = global::DTXCreator.Properties.Resources.貼り付け;
            this.toolStripMenuItem貼り付け.Name = "toolStripMenuItem貼り付け";
            this.toolStripMenuItem貼り付け.Click += new System.EventHandler(this.toolStripMenuItem貼り付け_Click);
            // 
            // toolStripMenuItem削除
            // 
            resources.ApplyResources(this.toolStripMenuItem削除, "toolStripMenuItem削除");
            this.toolStripMenuItem削除.Image = global::DTXCreator.Properties.Resources.削除;
            this.toolStripMenuItem削除.Name = "toolStripMenuItem削除";
            this.toolStripMenuItem削除.Click += new System.EventHandler(this.toolStripMenuItem削除_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // toolStripMenuItemすべて選択
            // 
            this.toolStripMenuItemすべて選択.Name = "toolStripMenuItemすべて選択";
            resources.ApplyResources(this.toolStripMenuItemすべて選択, "toolStripMenuItemすべて選択");
            this.toolStripMenuItemすべて選択.Click += new System.EventHandler(this.toolStripMenuItemすべて選択_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // toolStripMenuItem選択モード
            // 
            this.toolStripMenuItem選択モード.Image = global::DTXCreator.Properties.Resources.矢印ポインタ;
            this.toolStripMenuItem選択モード.Name = "toolStripMenuItem選択モード";
            resources.ApplyResources(this.toolStripMenuItem選択モード, "toolStripMenuItem選択モード");
            this.toolStripMenuItem選択モード.Click += new System.EventHandler(this.toolStripMenuItem選択モード_Click);
            // 
            // toolStripMenuItem編集モード
            // 
            this.toolStripMenuItem編集モード.Image = global::DTXCreator.Properties.Resources.鉛筆;
            this.toolStripMenuItem編集モード.Name = "toolStripMenuItem編集モード";
            resources.ApplyResources(this.toolStripMenuItem編集モード, "toolStripMenuItem編集モード");
            this.toolStripMenuItem編集モード.Click += new System.EventHandler(this.toolStripMenuItem編集モード_Click);
            // 
            // toolStripMenuItemモード切替
            // 
            this.toolStripMenuItemモード切替.Name = "toolStripMenuItemモード切替";
            resources.ApplyResources(this.toolStripMenuItemモード切替, "toolStripMenuItemモード切替");
            this.toolStripMenuItemモード切替.Click += new System.EventHandler(this.toolStripMenuItemモード切替_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // toolStripMenuItem検索
            // 
            this.toolStripMenuItem検索.Name = "toolStripMenuItem検索";
            resources.ApplyResources(this.toolStripMenuItem検索, "toolStripMenuItem検索");
            this.toolStripMenuItem検索.Click += new System.EventHandler(this.toolStripMenuItem検索_Click);
            // 
            // toolStripMenuItem置換
            // 
            this.toolStripMenuItem置換.Name = "toolStripMenuItem置換";
            resources.ApplyResources(this.toolStripMenuItem置換, "toolStripMenuItem置換");
            this.toolStripMenuItem置換.Click += new System.EventHandler(this.toolStripMenuItem置換_Click);
            // 
            // toolStripMenuItem表示
            // 
            this.toolStripMenuItem表示.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemチップパレット,
            this.toolStripMenuItemガイド間隔});
            this.toolStripMenuItem表示.Name = "toolStripMenuItem表示";
            resources.ApplyResources(this.toolStripMenuItem表示, "toolStripMenuItem表示");
            // 
            // toolStripMenuItemチップパレット
            // 
            this.toolStripMenuItemチップパレット.CheckOnClick = true;
            this.toolStripMenuItemチップパレット.Image = global::DTXCreator.Properties.Resources.チップパレット;
            this.toolStripMenuItemチップパレット.Name = "toolStripMenuItemチップパレット";
            resources.ApplyResources(this.toolStripMenuItemチップパレット, "toolStripMenuItemチップパレット");
            this.toolStripMenuItemチップパレット.Click += new System.EventHandler(this.toolStripMenuItemチップパレット_Click);
            // 
            // toolStripMenuItemガイド間隔
            // 
            this.toolStripMenuItemガイド間隔.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemガイド間隔4分,
            this.toolStripMenuItemガイド間隔8分,
            this.toolStripMenuItemガイド間隔12分,
            this.toolStripMenuItemガイド間隔16分,
            this.toolStripMenuItemガイド間隔24分,
            this.toolStripMenuItemガイド間隔32分,
            this.toolStripMenuItemガイド間隔48分,
            this.toolStripMenuItemガイド間隔64分,
            this.toolStripMenuItemガイド間隔フリー,
            this.toolStripSeparator6,
            this.toolStripMenuItemガイド間隔拡大,
            this.toolStripMenuItemガイド間隔縮小});
            this.toolStripMenuItemガイド間隔.Name = "toolStripMenuItemガイド間隔";
            resources.ApplyResources(this.toolStripMenuItemガイド間隔, "toolStripMenuItemガイド間隔");
            // 
            // toolStripMenuItemガイド間隔4分
            // 
            this.toolStripMenuItemガイド間隔4分.Name = "toolStripMenuItemガイド間隔4分";
            resources.ApplyResources(this.toolStripMenuItemガイド間隔4分, "toolStripMenuItemガイド間隔4分");
            this.toolStripMenuItemガイド間隔4分.Click += new System.EventHandler(this.toolStripMenuItemガイド間隔4分_Click);
            // 
            // toolStripMenuItemガイド間隔8分
            // 
            this.toolStripMenuItemガイド間隔8分.Name = "toolStripMenuItemガイド間隔8分";
            resources.ApplyResources(this.toolStripMenuItemガイド間隔8分, "toolStripMenuItemガイド間隔8分");
            this.toolStripMenuItemガイド間隔8分.Click += new System.EventHandler(this.toolStripMenuItemガイド間隔8分_Click);
            // 
            // toolStripMenuItemガイド間隔12分
            // 
            this.toolStripMenuItemガイド間隔12分.Name = "toolStripMenuItemガイド間隔12分";
            resources.ApplyResources(this.toolStripMenuItemガイド間隔12分, "toolStripMenuItemガイド間隔12分");
            this.toolStripMenuItemガイド間隔12分.Click += new System.EventHandler(this.toolStripMenuItemガイド間隔12分_Click);
            // 
            // toolStripMenuItemガイド間隔16分
            // 
            this.toolStripMenuItemガイド間隔16分.Name = "toolStripMenuItemガイド間隔16分";
            resources.ApplyResources(this.toolStripMenuItemガイド間隔16分, "toolStripMenuItemガイド間隔16分");
            this.toolStripMenuItemガイド間隔16分.Click += new System.EventHandler(this.toolStripMenuItemガイド間隔16分_Click);
            // 
            // toolStripMenuItemガイド間隔24分
            // 
            this.toolStripMenuItemガイド間隔24分.Name = "toolStripMenuItemガイド間隔24分";
            resources.ApplyResources(this.toolStripMenuItemガイド間隔24分, "toolStripMenuItemガイド間隔24分");
            this.toolStripMenuItemガイド間隔24分.Click += new System.EventHandler(this.toolStripMenuItemガイド間隔24分_Click);
            // 
            // toolStripMenuItemガイド間隔32分
            // 
            this.toolStripMenuItemガイド間隔32分.Name = "toolStripMenuItemガイド間隔32分";
            resources.ApplyResources(this.toolStripMenuItemガイド間隔32分, "toolStripMenuItemガイド間隔32分");
            this.toolStripMenuItemガイド間隔32分.Click += new System.EventHandler(this.toolStripMenuItemガイド間隔32分_Click);
            // 
            // toolStripMenuItemガイド間隔48分
            // 
            this.toolStripMenuItemガイド間隔48分.Name = "toolStripMenuItemガイド間隔48分";
            resources.ApplyResources(this.toolStripMenuItemガイド間隔48分, "toolStripMenuItemガイド間隔48分");
            this.toolStripMenuItemガイド間隔48分.Click += new System.EventHandler(this.toolStripMenuItemガイド間隔48分_Click);
            // 
            // toolStripMenuItemガイド間隔64分
            // 
            this.toolStripMenuItemガイド間隔64分.Name = "toolStripMenuItemガイド間隔64分";
            resources.ApplyResources(this.toolStripMenuItemガイド間隔64分, "toolStripMenuItemガイド間隔64分");
            this.toolStripMenuItemガイド間隔64分.Click += new System.EventHandler(this.toolStripMenuItemガイド間隔64分_Click);
            // 
            // toolStripMenuItemガイド間隔フリー
            // 
            this.toolStripMenuItemガイド間隔フリー.Name = "toolStripMenuItemガイド間隔フリー";
            resources.ApplyResources(this.toolStripMenuItemガイド間隔フリー, "toolStripMenuItemガイド間隔フリー");
            this.toolStripMenuItemガイド間隔フリー.Click += new System.EventHandler(this.toolStripMenuItemガイド間隔フリー_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // toolStripMenuItemガイド間隔拡大
            // 
            this.toolStripMenuItemガイド間隔拡大.Name = "toolStripMenuItemガイド間隔拡大";
            resources.ApplyResources(this.toolStripMenuItemガイド間隔拡大, "toolStripMenuItemガイド間隔拡大");
            this.toolStripMenuItemガイド間隔拡大.Click += new System.EventHandler(this.toolStripMenuItemガイド間隔拡大_Click);
            // 
            // toolStripMenuItemガイド間隔縮小
            // 
            this.toolStripMenuItemガイド間隔縮小.Name = "toolStripMenuItemガイド間隔縮小";
            resources.ApplyResources(this.toolStripMenuItemガイド間隔縮小, "toolStripMenuItemガイド間隔縮小");
            this.toolStripMenuItemガイド間隔縮小.Click += new System.EventHandler(this.toolStripMenuItemガイド間隔縮小_Click);
            // 
            // toolStripMenuItem再生
            // 
            this.toolStripMenuItem再生.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem先頭から再生,
            this.toolStripMenuItem現在位置から再生,
            this.toolStripMenuItem現在位置からBGMのみ再生,
            this.toolStripMenuItem再生停止});
            this.toolStripMenuItem再生.Name = "toolStripMenuItem再生";
            resources.ApplyResources(this.toolStripMenuItem再生, "toolStripMenuItem再生");
            // 
            // toolStripMenuItem先頭から再生
            // 
            this.toolStripMenuItem先頭から再生.Image = global::DTXCreator.Properties.Resources.最初から再生;
            this.toolStripMenuItem先頭から再生.Name = "toolStripMenuItem先頭から再生";
            resources.ApplyResources(this.toolStripMenuItem先頭から再生, "toolStripMenuItem先頭から再生");
            this.toolStripMenuItem先頭から再生.Click += new System.EventHandler(this.toolStripMenuItem先頭から再生_Click);
            // 
            // toolStripMenuItem現在位置から再生
            // 
            this.toolStripMenuItem現在位置から再生.Image = global::DTXCreator.Properties.Resources.再生;
            this.toolStripMenuItem現在位置から再生.Name = "toolStripMenuItem現在位置から再生";
            resources.ApplyResources(this.toolStripMenuItem現在位置から再生, "toolStripMenuItem現在位置から再生");
            this.toolStripMenuItem現在位置から再生.Click += new System.EventHandler(this.toolStripMenuItem現在位置から再生_Click);
            // 
            // toolStripMenuItem現在位置からBGMのみ再生
            // 
            this.toolStripMenuItem現在位置からBGMのみ再生.Image = global::DTXCreator.Properties.Resources.BGMのみ再生;
            this.toolStripMenuItem現在位置からBGMのみ再生.Name = "toolStripMenuItem現在位置からBGMのみ再生";
            resources.ApplyResources(this.toolStripMenuItem現在位置からBGMのみ再生, "toolStripMenuItem現在位置からBGMのみ再生");
            this.toolStripMenuItem現在位置からBGMのみ再生.Click += new System.EventHandler(this.toolStripMenuItem現在位置からBGMのみ再生_Click);
            // 
            // toolStripMenuItem再生停止
            // 
            this.toolStripMenuItem再生停止.Image = global::DTXCreator.Properties.Resources.再生停止;
            this.toolStripMenuItem再生停止.Name = "toolStripMenuItem再生停止";
            resources.ApplyResources(this.toolStripMenuItem再生停止, "toolStripMenuItem再生停止");
            this.toolStripMenuItem再生停止.Click += new System.EventHandler(this.toolStripMenuItem再生停止_Click);
            // 
            // toolStripMenuItemツール
            // 
            this.toolStripMenuItemツール.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemオプション});
            this.toolStripMenuItemツール.Name = "toolStripMenuItemツール";
            resources.ApplyResources(this.toolStripMenuItemツール, "toolStripMenuItemツール");
            // 
            // toolStripMenuItemオプション
            // 
            this.toolStripMenuItemオプション.Name = "toolStripMenuItemオプション";
            resources.ApplyResources(this.toolStripMenuItemオプション, "toolStripMenuItemオプション");
            this.toolStripMenuItemオプション.Click += new System.EventHandler(this.toolStripMenuItemオプション_Click);
            // 
            // toolStripMenuItemヘルプ
            // 
            this.toolStripMenuItemヘルプ.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemDTXCreaterマニュアル,
            this.toolStripMenuItemバージョン});
            this.toolStripMenuItemヘルプ.Name = "toolStripMenuItemヘルプ";
            resources.ApplyResources(this.toolStripMenuItemヘルプ, "toolStripMenuItemヘルプ");
            // 
            // toolStripMenuItemDTXCreaterマニュアル
            // 
            this.toolStripMenuItemDTXCreaterマニュアル.Image = global::DTXCreator.Properties.Resources.ヘルプ;
            this.toolStripMenuItemDTXCreaterマニュアル.Name = "toolStripMenuItemDTXCreaterマニュアル";
            resources.ApplyResources(this.toolStripMenuItemDTXCreaterマニュアル, "toolStripMenuItemDTXCreaterマニュアル");
            this.toolStripMenuItemDTXCreaterマニュアル.Click += new System.EventHandler(this.toolStripMenuItemDTXCreaterマニュアル_Click);
            // 
            // toolStripMenuItemバージョン
            // 
            this.toolStripMenuItemバージョン.Name = "toolStripMenuItemバージョン";
            resources.ApplyResources(this.toolStripMenuItemバージョン, "toolStripMenuItemバージョン");
            this.toolStripMenuItemバージョン.Click += new System.EventHandler(this.toolStripMenuItemバージョン_Click);
            // 
            // toolStripツールバー
            // 
            this.toolStripツールバー.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton新規作成,
            this.toolStripButton開く,
            this.toolStripButton上書き保存,
            this.toolStripSeparator7,
            this.toolStripButton切り取り,
            this.toolStripButtonコピー,
            this.toolStripButton貼り付け,
            this.toolStripButton削除,
            this.toolStripSeparator8,
            this.toolStripButtonアンドゥ,
            this.toolStripButtonリドゥ,
            this.toolStripSeparator9,
            this.toolStripButtonチップパレット,
            this.toolStripSeparator10,
            this.toolStripComboBox譜面拡大率,
            this.toolStripComboBoxガイド間隔,
            this.toolStripButton選択モード,
            this.toolStripButton編集モード,
            this.toolStripSeparator11,
            this.toolStripButton先頭から再生,
            this.toolStripButton現在位置から再生,
            this.toolStripButton現在位置からBGMのみ再生,
            this.toolStripButton再生停止,
            this.toolStripComboBox演奏速度,
            this.toolStripSeparator12});
            resources.ApplyResources(this.toolStripツールバー, "toolStripツールバー");
            this.toolStripツールバー.Name = "toolStripツールバー";
            // 
            // toolStripButton新規作成
            // 
            this.toolStripButton新規作成.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton新規作成.Image = global::DTXCreator.Properties.Resources.新規作成;
            resources.ApplyResources(this.toolStripButton新規作成, "toolStripButton新規作成");
            this.toolStripButton新規作成.Name = "toolStripButton新規作成";
            this.toolStripButton新規作成.Click += new System.EventHandler(this.toolStripButton新規作成_Click);
            // 
            // toolStripButton開く
            // 
            this.toolStripButton開く.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton開く.Image = global::DTXCreator.Properties.Resources.開く;
            resources.ApplyResources(this.toolStripButton開く, "toolStripButton開く");
            this.toolStripButton開く.Name = "toolStripButton開く";
            this.toolStripButton開く.Click += new System.EventHandler(this.toolStripButton開く_Click);
            // 
            // toolStripButton上書き保存
            // 
            this.toolStripButton上書き保存.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton上書き保存.Image = global::DTXCreator.Properties.Resources.保存;
            resources.ApplyResources(this.toolStripButton上書き保存, "toolStripButton上書き保存");
            this.toolStripButton上書き保存.Name = "toolStripButton上書き保存";
            this.toolStripButton上書き保存.Click += new System.EventHandler(this.toolStripButton上書き保存_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // toolStripButton切り取り
            // 
            this.toolStripButton切り取り.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton切り取り, "toolStripButton切り取り");
            this.toolStripButton切り取り.Image = global::DTXCreator.Properties.Resources.切り取り;
            this.toolStripButton切り取り.Name = "toolStripButton切り取り";
            this.toolStripButton切り取り.Click += new System.EventHandler(this.toolStripButton切り取り_Click);
            // 
            // toolStripButtonコピー
            // 
            this.toolStripButtonコピー.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonコピー, "toolStripButtonコピー");
            this.toolStripButtonコピー.Image = global::DTXCreator.Properties.Resources.コピー;
            this.toolStripButtonコピー.Name = "toolStripButtonコピー";
            this.toolStripButtonコピー.Click += new System.EventHandler(this.toolStripButtonコピー_Click);
            // 
            // toolStripButton貼り付け
            // 
            this.toolStripButton貼り付け.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton貼り付け, "toolStripButton貼り付け");
            this.toolStripButton貼り付け.Image = global::DTXCreator.Properties.Resources.貼り付け;
            this.toolStripButton貼り付け.Name = "toolStripButton貼り付け";
            this.toolStripButton貼り付け.Click += new System.EventHandler(this.toolStripButton貼り付け_Click);
            // 
            // toolStripButton削除
            // 
            this.toolStripButton削除.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton削除, "toolStripButton削除");
            this.toolStripButton削除.Image = global::DTXCreator.Properties.Resources.削除;
            this.toolStripButton削除.Name = "toolStripButton削除";
            this.toolStripButton削除.Click += new System.EventHandler(this.toolStripButton削除_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // toolStripButtonアンドゥ
            // 
            this.toolStripButtonアンドゥ.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonアンドゥ, "toolStripButtonアンドゥ");
            this.toolStripButtonアンドゥ.Image = global::DTXCreator.Properties.Resources.Undo;
            this.toolStripButtonアンドゥ.Name = "toolStripButtonアンドゥ";
            this.toolStripButtonアンドゥ.Click += new System.EventHandler(this.toolStripButtonアンドゥ_Click);
            // 
            // toolStripButtonリドゥ
            // 
            this.toolStripButtonリドゥ.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonリドゥ, "toolStripButtonリドゥ");
            this.toolStripButtonリドゥ.Image = global::DTXCreator.Properties.Resources.Redo;
            this.toolStripButtonリドゥ.Name = "toolStripButtonリドゥ";
            this.toolStripButtonリドゥ.Click += new System.EventHandler(this.toolStripButtonリドゥ_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            // 
            // toolStripButtonチップパレット
            // 
            this.toolStripButtonチップパレット.CheckOnClick = true;
            this.toolStripButtonチップパレット.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonチップパレット.Image = global::DTXCreator.Properties.Resources.チップパレット;
            resources.ApplyResources(this.toolStripButtonチップパレット, "toolStripButtonチップパレット");
            this.toolStripButtonチップパレット.Name = "toolStripButtonチップパレット";
            this.toolStripButtonチップパレット.Click += new System.EventHandler(this.toolStripButtonチップパレット_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
            // 
            // toolStripComboBox譜面拡大率
            // 
            this.toolStripComboBox譜面拡大率.DropDownHeight = 200;
            this.toolStripComboBox譜面拡大率.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.toolStripComboBox譜面拡大率, "toolStripComboBox譜面拡大率");
            this.toolStripComboBox譜面拡大率.Items.AddRange(new object[] {
            resources.GetString("toolStripComboBox譜面拡大率.Items"),
            resources.GetString("toolStripComboBox譜面拡大率.Items1"),
            resources.GetString("toolStripComboBox譜面拡大率.Items2"),
            resources.GetString("toolStripComboBox譜面拡大率.Items3"),
            resources.GetString("toolStripComboBox譜面拡大率.Items4"),
            resources.GetString("toolStripComboBox譜面拡大率.Items5"),
            resources.GetString("toolStripComboBox譜面拡大率.Items6"),
            resources.GetString("toolStripComboBox譜面拡大率.Items7"),
            resources.GetString("toolStripComboBox譜面拡大率.Items8"),
            resources.GetString("toolStripComboBox譜面拡大率.Items9")});
            this.toolStripComboBox譜面拡大率.Name = "toolStripComboBox譜面拡大率";
            this.toolStripComboBox譜面拡大率.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox譜面拡大率_SelectedIndexChanged);
            // 
            // toolStripComboBoxガイド間隔
            // 
            this.toolStripComboBoxガイド間隔.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxガイド間隔.Items.AddRange(new object[] {
            resources.GetString("toolStripComboBoxガイド間隔.Items"),
            resources.GetString("toolStripComboBoxガイド間隔.Items1"),
            resources.GetString("toolStripComboBoxガイド間隔.Items2"),
            resources.GetString("toolStripComboBoxガイド間隔.Items3"),
            resources.GetString("toolStripComboBoxガイド間隔.Items4"),
            resources.GetString("toolStripComboBoxガイド間隔.Items5"),
            resources.GetString("toolStripComboBoxガイド間隔.Items6"),
            resources.GetString("toolStripComboBoxガイド間隔.Items7"),
            resources.GetString("toolStripComboBoxガイド間隔.Items8")});
            resources.ApplyResources(this.toolStripComboBoxガイド間隔, "toolStripComboBoxガイド間隔");
            this.toolStripComboBoxガイド間隔.Name = "toolStripComboBoxガイド間隔";
            this.toolStripComboBoxガイド間隔.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxガイド間隔_SelectedIndexChanged);
            // 
            // toolStripButton選択モード
            // 
            this.toolStripButton選択モード.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton選択モード.Image = global::DTXCreator.Properties.Resources.矢印ポインタ;
            resources.ApplyResources(this.toolStripButton選択モード, "toolStripButton選択モード");
            this.toolStripButton選択モード.Name = "toolStripButton選択モード";
            this.toolStripButton選択モード.Click += new System.EventHandler(this.toolStripButton選択モード_Click);
            // 
            // toolStripButton編集モード
            // 
            this.toolStripButton編集モード.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton編集モード.Image = global::DTXCreator.Properties.Resources.鉛筆;
            resources.ApplyResources(this.toolStripButton編集モード, "toolStripButton編集モード");
            this.toolStripButton編集モード.Name = "toolStripButton編集モード";
            this.toolStripButton編集モード.Click += new System.EventHandler(this.toolStripButton編集モード_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            resources.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
            // 
            // toolStripButton先頭から再生
            // 
            this.toolStripButton先頭から再生.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton先頭から再生.Image = global::DTXCreator.Properties.Resources.最初から再生;
            resources.ApplyResources(this.toolStripButton先頭から再生, "toolStripButton先頭から再生");
            this.toolStripButton先頭から再生.Name = "toolStripButton先頭から再生";
            this.toolStripButton先頭から再生.Click += new System.EventHandler(this.toolStripButton先頭から再生_Click);
            // 
            // toolStripButton現在位置から再生
            // 
            this.toolStripButton現在位置から再生.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton現在位置から再生.Image = global::DTXCreator.Properties.Resources.再生;
            resources.ApplyResources(this.toolStripButton現在位置から再生, "toolStripButton現在位置から再生");
            this.toolStripButton現在位置から再生.Name = "toolStripButton現在位置から再生";
            this.toolStripButton現在位置から再生.Click += new System.EventHandler(this.toolStripButton現在位置から再生_Click);
            // 
            // toolStripButton現在位置からBGMのみ再生
            // 
            this.toolStripButton現在位置からBGMのみ再生.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton現在位置からBGMのみ再生.Image = global::DTXCreator.Properties.Resources.BGMのみ再生;
            resources.ApplyResources(this.toolStripButton現在位置からBGMのみ再生, "toolStripButton現在位置からBGMのみ再生");
            this.toolStripButton現在位置からBGMのみ再生.Name = "toolStripButton現在位置からBGMのみ再生";
            this.toolStripButton現在位置からBGMのみ再生.Click += new System.EventHandler(this.toolStripButton現在位置からBGMのみ再生_Click);
            // 
            // toolStripButton再生停止
            // 
            this.toolStripButton再生停止.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton再生停止.Image = global::DTXCreator.Properties.Resources.再生停止;
            resources.ApplyResources(this.toolStripButton再生停止, "toolStripButton再生停止");
            this.toolStripButton再生停止.Name = "toolStripButton再生停止";
            this.toolStripButton再生停止.Click += new System.EventHandler(this.toolStripButton再生停止_Click);
            // 
            // toolStripComboBox演奏速度
            // 
            this.toolStripComboBox演奏速度.DropDownHeight = 150;
            this.toolStripComboBox演奏速度.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox演奏速度.DropDownWidth = 35;
            resources.ApplyResources(this.toolStripComboBox演奏速度, "toolStripComboBox演奏速度");
            this.toolStripComboBox演奏速度.Items.AddRange(new object[] {
            resources.GetString("toolStripComboBox演奏速度.Items"),
            resources.GetString("toolStripComboBox演奏速度.Items1"),
            resources.GetString("toolStripComboBox演奏速度.Items2"),
            resources.GetString("toolStripComboBox演奏速度.Items3"),
            resources.GetString("toolStripComboBox演奏速度.Items4"),
            resources.GetString("toolStripComboBox演奏速度.Items5"),
            resources.GetString("toolStripComboBox演奏速度.Items6"),
            resources.GetString("toolStripComboBox演奏速度.Items7"),
            resources.GetString("toolStripComboBox演奏速度.Items8"),
            resources.GetString("toolStripComboBox演奏速度.Items9"),
            resources.GetString("toolStripComboBox演奏速度.Items10")});
            this.toolStripComboBox演奏速度.Name = "toolStripComboBox演奏速度";
            this.toolStripComboBox演奏速度.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox演奏速度_SelectedIndexChanged);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            resources.ApplyResources(this.toolStripSeparator12, "toolStripSeparator12");
            // 
            // vScrollBar譜面用垂直スクロールバー
            // 
            resources.ApplyResources(this.vScrollBar譜面用垂直スクロールバー, "vScrollBar譜面用垂直スクロールバー");
            this.vScrollBar譜面用垂直スクロールバー.LargeChange = 64;
            this.vScrollBar譜面用垂直スクロールバー.Name = "vScrollBar譜面用垂直スクロールバー";
            this.vScrollBar譜面用垂直スクロールバー.SmallChange = 4;
            this.vScrollBar譜面用垂直スクロールバー.ValueChanged += new System.EventHandler(this.vScrollBar譜面用垂直スクロールバー_ValueChanged);
            // 
            // contextMenuStrip譜面右メニュー
            // 
            this.contextMenuStrip譜面右メニュー.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem選択チップの切り取り,
            this.toolStripMenuItem選択チップのコピー,
            this.toolStripMenuItem選択チップの貼り付け,
            this.toolStripMenuItem選択チップの削除,
            this.toolStripSeparator15,
            this.toolStripMenuItemすべてのチップの選択,
            this.toolStripMenuItemレーン内のすべてのチップの選択,
            this.toolStripMenuItem小節内のすべてのチップの選択,
            this.toolStripSeparator16,
            this.toolStripMenuItem小節長変更,
            this.toolStripSeparator17,
            this.toolStripMenuItem小節の挿入,
            this.toolStripMenuItem小節の削除});
            this.contextMenuStrip譜面右メニュー.Name = "contextMenuStrip譜面右メニュー";
            resources.ApplyResources(this.contextMenuStrip譜面右メニュー, "contextMenuStrip譜面右メニュー");
            // 
            // toolStripMenuItem選択チップの切り取り
            // 
            this.toolStripMenuItem選択チップの切り取り.Image = global::DTXCreator.Properties.Resources.切り取り;
            this.toolStripMenuItem選択チップの切り取り.Name = "toolStripMenuItem選択チップの切り取り";
            resources.ApplyResources(this.toolStripMenuItem選択チップの切り取り, "toolStripMenuItem選択チップの切り取り");
            this.toolStripMenuItem選択チップの切り取り.Click += new System.EventHandler(this.toolStripMenuItem選択チップの切り取り_Click);
            // 
            // toolStripMenuItem選択チップのコピー
            // 
            this.toolStripMenuItem選択チップのコピー.Image = global::DTXCreator.Properties.Resources.コピー;
            this.toolStripMenuItem選択チップのコピー.Name = "toolStripMenuItem選択チップのコピー";
            resources.ApplyResources(this.toolStripMenuItem選択チップのコピー, "toolStripMenuItem選択チップのコピー");
            this.toolStripMenuItem選択チップのコピー.Click += new System.EventHandler(this.toolStripMenuItem選択チップのコピー_Click);
            // 
            // toolStripMenuItem選択チップの貼り付け
            // 
            this.toolStripMenuItem選択チップの貼り付け.Image = global::DTXCreator.Properties.Resources.貼り付け;
            this.toolStripMenuItem選択チップの貼り付け.Name = "toolStripMenuItem選択チップの貼り付け";
            resources.ApplyResources(this.toolStripMenuItem選択チップの貼り付け, "toolStripMenuItem選択チップの貼り付け");
            this.toolStripMenuItem選択チップの貼り付け.Click += new System.EventHandler(this.toolStripMenuItem選択チップの貼り付け_Click);
            // 
            // toolStripMenuItem選択チップの削除
            // 
            this.toolStripMenuItem選択チップの削除.Image = global::DTXCreator.Properties.Resources.削除;
            this.toolStripMenuItem選択チップの削除.Name = "toolStripMenuItem選択チップの削除";
            resources.ApplyResources(this.toolStripMenuItem選択チップの削除, "toolStripMenuItem選択チップの削除");
            this.toolStripMenuItem選択チップの削除.Click += new System.EventHandler(this.toolStripMenuItem選択チップの削除_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            resources.ApplyResources(this.toolStripSeparator15, "toolStripSeparator15");
            // 
            // toolStripMenuItemすべてのチップの選択
            // 
            this.toolStripMenuItemすべてのチップの選択.Name = "toolStripMenuItemすべてのチップの選択";
            resources.ApplyResources(this.toolStripMenuItemすべてのチップの選択, "toolStripMenuItemすべてのチップの選択");
            this.toolStripMenuItemすべてのチップの選択.Click += new System.EventHandler(this.toolStripMenuItemすべてのチップの選択_Click);
            // 
            // toolStripMenuItemレーン内のすべてのチップの選択
            // 
            this.toolStripMenuItemレーン内のすべてのチップの選択.Name = "toolStripMenuItemレーン内のすべてのチップの選択";
            resources.ApplyResources(this.toolStripMenuItemレーン内のすべてのチップの選択, "toolStripMenuItemレーン内のすべてのチップの選択");
            this.toolStripMenuItemレーン内のすべてのチップの選択.Click += new System.EventHandler(this.toolStripMenuItemレーン内のすべてのチップの選択_Click);
            // 
            // toolStripMenuItem小節内のすべてのチップの選択
            // 
            this.toolStripMenuItem小節内のすべてのチップの選択.Name = "toolStripMenuItem小節内のすべてのチップの選択";
            resources.ApplyResources(this.toolStripMenuItem小節内のすべてのチップの選択, "toolStripMenuItem小節内のすべてのチップの選択");
            this.toolStripMenuItem小節内のすべてのチップの選択.Click += new System.EventHandler(this.toolStripMenuItem小節内のすべてのチップの選択_Click);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            resources.ApplyResources(this.toolStripSeparator16, "toolStripSeparator16");
            // 
            // toolStripMenuItem小節長変更
            // 
            this.toolStripMenuItem小節長変更.Name = "toolStripMenuItem小節長変更";
            resources.ApplyResources(this.toolStripMenuItem小節長変更, "toolStripMenuItem小節長変更");
            this.toolStripMenuItem小節長変更.Click += new System.EventHandler(this.toolStripMenuItem小節長変更_Click);
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            resources.ApplyResources(this.toolStripSeparator17, "toolStripSeparator17");
            // 
            // toolStripMenuItem小節の挿入
            // 
            this.toolStripMenuItem小節の挿入.Name = "toolStripMenuItem小節の挿入";
            resources.ApplyResources(this.toolStripMenuItem小節の挿入, "toolStripMenuItem小節の挿入");
            this.toolStripMenuItem小節の挿入.Click += new System.EventHandler(this.toolStripMenuItem小節の挿入_Click);
            // 
            // toolStripMenuItem小節の削除
            // 
            this.toolStripMenuItem小節の削除.Name = "toolStripMenuItem小節の削除";
            resources.ApplyResources(this.toolStripMenuItem小節の削除, "toolStripMenuItem小節の削除");
            this.toolStripMenuItem小節の削除.Click += new System.EventHandler(this.toolStripMenuItem小節の削除_Click);
            // 
            // CMainForm
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerタブと譜面を分割);
            this.Controls.Add(this.vScrollBar譜面用垂直スクロールバー);
            this.Controls.Add(this.toolStripツールバー);
            this.Controls.Add(this.statusStripステータスバー);
            this.Controls.Add(this.menuStripメニューバー);
            this.MainMenuStrip = this.menuStripメニューバー;
            this.Name = "CMainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Cメインフォーム_FormClosing);
            this.Load += new System.EventHandler(this.Cメインフォーム_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Cメインフォーム_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Cメインフォーム_DragEnter);
            this.splitContainerタブと譜面を分割.Panel1.ResumeLayout(false);
            this.splitContainerタブと譜面を分割.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerタブと譜面を分割)).EndInit();
            this.splitContainerタブと譜面を分割.ResumeLayout(false);
            this.tabControl情報パネル.ResumeLayout(false);
            this.tabPage基本情報.ResumeLayout(false);
            this.tabPage基本情報.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBPM)).EndInit();
            this.tabPageWAV.ResumeLayout(false);
            this.tabPageWAV.PerformLayout();
            this.toolStripWAVツールバー.ResumeLayout(false);
            this.toolStripWAVツールバー.PerformLayout();
            this.tabPageBMP.ResumeLayout(false);
            this.tabPageBMP.PerformLayout();
            this.toolStripBMPツールバー.ResumeLayout(false);
            this.toolStripBMPツールバー.PerformLayout();
            this.tabPageAVI.ResumeLayout(false);
            this.tabPageAVI.PerformLayout();
            this.toolStripAVIツールバー.ResumeLayout(false);
            this.toolStripAVIツールバー.PerformLayout();
            this.tabPage自由入力.ResumeLayout(false);
            this.tabPage自由入力.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox譜面パネル)).EndInit();
            this.menuStripメニューバー.ResumeLayout(false);
            this.menuStripメニューバー.PerformLayout();
            this.toolStripツールバー.ResumeLayout(false);
            this.toolStripツールバー.PerformLayout();
            this.contextMenuStrip譜面右メニュー.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStripステータスバー;
		private System.Windows.Forms.MenuStrip menuStripメニューバー;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemファイル;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem編集;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem表示;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem再生;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemツール;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemヘルプ;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem新規;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem開く;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem上書き保存;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem名前を付けて保存;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem終了;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemアンドゥ;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemリドゥ;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem切り取り;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemコピー;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem貼り付け;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem削除;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemすべて選択;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem選択モード;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem編集モード;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemモード切替;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem検索;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem置換;
		internal System.Windows.Forms.ToolStripMenuItem toolStripMenuItemチップパレット;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemガイド間隔;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemガイド間隔4分;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemガイド間隔8分;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemガイド間隔12分;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemガイド間隔16分;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemガイド間隔24分;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemガイド間隔32分;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemガイド間隔48分;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemガイド間隔64分;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemガイド間隔フリー;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemガイド間隔拡大;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemガイド間隔縮小;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem先頭から再生;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem現在位置から再生;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem現在位置からBGMのみ再生;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem再生停止;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemオプション;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDTXCreaterマニュアル;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemバージョン;
		private System.Windows.Forms.ToolStrip toolStripツールバー;
		private System.Windows.Forms.ToolStripButton toolStripButton新規作成;
		private System.Windows.Forms.ToolStripButton toolStripButton開く;
		private System.Windows.Forms.ToolStripButton toolStripButton上書き保存;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripButton toolStripButton切り取り;
		private System.Windows.Forms.ToolStripButton toolStripButtonコピー;
		private System.Windows.Forms.ToolStripButton toolStripButton貼り付け;
		private System.Windows.Forms.ToolStripButton toolStripButton削除;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
		private System.Windows.Forms.ToolStripButton toolStripButtonアンドゥ;
		private System.Windows.Forms.ToolStripButton toolStripButtonリドゥ;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
		internal System.Windows.Forms.ToolStripButton toolStripButtonチップパレット;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
		private System.Windows.Forms.ToolStripComboBox toolStripComboBox譜面拡大率;
		private System.Windows.Forms.ToolStripComboBox toolStripComboBoxガイド間隔;
		private System.Windows.Forms.ToolStripButton toolStripButton選択モード;
		private System.Windows.Forms.ToolStripButton toolStripButton編集モード;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
		private System.Windows.Forms.ToolStripButton toolStripButton先頭から再生;
		private System.Windows.Forms.ToolStripButton toolStripButton現在位置から再生;
		private System.Windows.Forms.ToolStripButton toolStripButton現在位置からBGMのみ再生;
		private System.Windows.Forms.ToolStripButton toolStripButton再生停止;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
		internal System.Windows.Forms.ToolStripComboBox toolStripComboBox演奏速度;
		internal System.Windows.Forms.VScrollBar vScrollBar譜面用垂直スクロールバー;
		internal System.Windows.Forms.SplitContainer splitContainerタブと譜面を分割;
		private System.Windows.Forms.TabControl tabControl情報パネル;
		private System.Windows.Forms.TabPage tabPage基本情報;
		private System.Windows.Forms.TabPage tabPageWAV;
		internal System.Windows.Forms.HScrollBar hScrollBar譜面用水平スクロールバー;
		internal System.Windows.Forms.PictureBox pictureBox譜面パネル;
		private System.Windows.Forms.TabPage tabPageBMP;
		private System.Windows.Forms.TabPage tabPageAVI;
		private System.Windows.Forms.TabPage tabPage自由入力;
		internal System.Windows.Forms.TextBox textBox曲名;
		private System.Windows.Forms.ToolTip toolTipツールチップ;
		internal System.Windows.Forms.TextBox textBox製作者;
		internal System.Windows.Forms.TextBox textBoxコメント;
		internal System.Windows.Forms.NumericUpDown numericUpDownBPM;
		private System.Windows.Forms.Label label曲名;
		private System.Windows.Forms.Label label製作者;
		private System.Windows.Forms.Label labelコメント;
		private System.Windows.Forms.Label labelBPM;
		internal System.Windows.Forms.HScrollBar hScrollBarDLEVEL;
		internal System.Windows.Forms.TextBox textBoxDLEVEL;
		private System.Windows.Forms.Label labelDLEVEL;
		internal System.Windows.Forms.HScrollBar hScrollBarGLEVEL;
		internal System.Windows.Forms.TextBox textBoxGLEVEL;
		private System.Windows.Forms.Label labelGLEVEL;
		internal System.Windows.Forms.HScrollBar hScrollBarBLEVEL;
		internal System.Windows.Forms.TextBox textBoxBLEVEL;
		private System.Windows.Forms.Label labelBLEVEL;
		private System.Windows.Forms.Label labelパネル;
		internal System.Windows.Forms.TextBox textBoxパネル;
		internal System.Windows.Forms.TextBox textBoxPREVIEW;
		internal System.Windows.Forms.TextBox textBoxPREIMAGE;
		internal System.Windows.Forms.TextBox textBoxSTAGEFILE;
		internal System.Windows.Forms.TextBox textBoxBACKGROUND;
		internal System.Windows.Forms.TextBox textBoxRESULTIMAGE;
		private System.Windows.Forms.Label labelPREVIEW;
		private System.Windows.Forms.Label labelPREIMAGE;
		private System.Windows.Forms.Label labelSTAGEFILE;
		private System.Windows.Forms.Label labelBACKGROUND;
		private System.Windows.Forms.Label labeRESULTIMAGE;
		private System.Windows.Forms.Button buttonPREVIEW参照;
		private System.Windows.Forms.Button buttonPREIMAGE参照;
		private System.Windows.Forms.Button buttonSTAGEFILE参照;
		private System.Windows.Forms.Button buttonBACKGROUND参照;
		private System.Windows.Forms.Button buttonRESULTIMAGE参照;
		private System.Windows.Forms.ToolStrip toolStripWAVツールバー;
		private System.Windows.Forms.ToolStripButton toolStripButtonWAVリスト上移動;
		private System.Windows.Forms.ToolStripButton toolStripButtonWAVリスト下移動;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
		private System.Windows.Forms.ToolStripButton toolStripButtonWAVリストプレビュー再生開始;
		private System.Windows.Forms.ToolStripButton toolStripButtonWAVリストプレビュー再生停止;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
		private System.Windows.Forms.ToolStripButton toolStripButtonWAVリストプレビュースイッチ;
		internal System.Windows.Forms.ListView listViewWAVリスト;
		private System.Windows.Forms.ColumnHeader columnHeaderWAV_ラベル;
		private System.Windows.Forms.ColumnHeader columnHeaderWAV_番号;
		private System.Windows.Forms.ColumnHeader columnHeaderWAV_ファイル名;
		private System.Windows.Forms.ColumnHeader columnHeaderWAV_音量;
		private System.Windows.Forms.ColumnHeader columnHeaderWAV_位置;
		private System.Windows.Forms.ColumnHeader columnHeaderWAV_BGM;
		private System.Windows.Forms.ToolStrip toolStripBMPツールバー;
		private System.Windows.Forms.ToolStripButton toolStripButtonBMPリスト上移動;
		private System.Windows.Forms.ToolStripButton toolStripButtonBMPリスト下移動;
		internal System.Windows.Forms.ListView listViewBMPリスト;
		private System.Windows.Forms.ColumnHeader columnHeaderBMP_TEX;
		private System.Windows.Forms.ColumnHeader columnHeaderBMP_ラベル;
		private System.Windows.Forms.ColumnHeader columnHeaderBMP_BMP番号;
		private System.Windows.Forms.ColumnHeader columnHeaderBMP_ファイル名;
		private System.Windows.Forms.ToolStrip toolStripAVIツールバー;
		private System.Windows.Forms.ToolStripButton toolStripButtonAVIリスト上移動;
		private System.Windows.Forms.ToolStripButton toolStripButtonAVIリスト下移動;
		internal System.Windows.Forms.ListView listViewAVIリスト;
		private System.Windows.Forms.ColumnHeader columnHeaderAVI_ラベル;
		private System.Windows.Forms.ColumnHeader columnHeaderAVI_AVI番号;
		private System.Windows.Forms.ColumnHeader columnHeaderAVI_ファイル名;
		internal System.Windows.Forms.TextBox textBox自由入力欄;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip譜面右メニュー;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem選択チップの切り取り;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem選択チップのコピー;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem選択チップの貼り付け;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem選択チップの削除;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemすべてのチップの選択;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem小節長変更;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem小節の挿入;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem小節の削除;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemレーン内のすべてのチップの選択;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem小節内のすべてのチップの選択;
	}
}