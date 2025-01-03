using System;
using System.Collections.Generic;
using System.Text;

namespace DTXMania
{
	/// <summary>
	/// 「List」（複数の固定値からの１つを選択可能）を表すアイテム。
	///  List item (select one from multiple fixed values).
	/// </summary>
	internal class CItemList : CItemBase
	{
		// プロパティ

		public List<string> list項目値;
		public int n現在選択されている項目番号;


		// コンストラクタ

		public CItemList()
		{
			base.eType = CItemBase.EType.List;
			this.n現在選択されている項目番号 = 0;
			this.list項目値 = new List<string>();
		}
		public CItemList(string str項目名, CItemBase.EPanelType eパネル種別, int n初期インデックス値, string str説明文jp, string str説明文en, params string[] arg項目リスト)
			: this() {
			this.tInitialize(str項目名, eパネル種別, n初期インデックス値, str説明文jp, str説明文en, arg項目リスト);
		}


		// CItemBase 実装

		protected override void tEnter押下()
		{
			this.tMoveItemValueToNext();
		}
		public override void tMoveItemValueToNext()
		{
			if( ++this.n現在選択されている項目番号 >= this.list項目値.Count )
			{
				this.n現在選択されている項目番号 = 0;
			}
		}
		public override void tMoveItemValueToPrevious()
		{
			if( --this.n現在選択されている項目番号 < 0 )
			{
				this.n現在選択されている項目番号 = this.list項目値.Count - 1;
			}
		}

		public void tInitialize(string str項目名, CItemBase.EPanelType eパネル種別, int n初期インデックス値, string str説明文jp, string str説明文en, params string[] arg項目リスト) {
			base.tInitialize(str項目名, eパネル種別, str説明文jp, str説明文en);
			this.n現在選択されている項目番号 = n初期インデックス値;
			foreach (string str in arg項目リスト) {
				this.list項目値.Add(str);
			}
		}
		public override object obj現在値()
		{
			return this.list項目値[ n現在選択されている項目番号 ];
		}
		public override int GetIndex()
		{
			return n現在選択されている項目番号;
		}
		public override void SetIndex( int index )
		{
			n現在選択されている項目番号 = index;
		}
	}




	/// <summary>
	/// 簡易コンフィグの「切り替え」に使用する、「List」（複数の固定値からの１つを選択可能）を表すアイテム。
	/// e種別が違うのと、tPressEnter()で何もしない以外は、「List」そのまま。
	/// </summary>
	internal class CSwitchItemList : CItemList
	{
		// コンストラクタ

		public CSwitchItemList()
		{
			base.eType = CItemBase.EType.切替リスト;
			this.n現在選択されている項目番号 = 0;
			this.list項目値 = new List<string>();
		}
		public CSwitchItemList( string str項目名, CItemBase.EPanelType eパネル種別, int n初期インデックス値, string str説明文jp, string str説明文en, params string[] arg項目リスト )
			: this()
		{
			this.tInitialize( str項目名, eパネル種別, n初期インデックス値, str説明文jp, str説明文en, arg項目リスト );
		}

		protected override void tEnter押下()
		{
			// this.tMoveItemValueToNext();	// DoNothing
		}
	}

}
