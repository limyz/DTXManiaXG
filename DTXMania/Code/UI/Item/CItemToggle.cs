using System;
using System.Collections.Generic;
using System.Text;

namespace DTXMania
{
	/// <summary>
	/// 「トグル」（ON, OFF の2状態）を表すアイテム。
	///  Toggle (2 states: ON or OFF) item.
	/// </summary>
	internal class CItemToggle : CItemBase
	{
		// プロパティ

		public bool bON;

		
		// コンストラクタ

		public CItemToggle()
		{
			base.eType = CItemBase.EType.ONorOFFToggle;
			this.bON = false;
		}
		public CItemToggle(string str項目名, bool b初期状態, string str説明文jp, string str説明文en)
			: this() {
			this.tInitialize(str項目名, b初期状態, str説明文jp, str説明文en);
		}

		// CItemBase 実装

		protected override void tEnter押下()
		{
			this.tMoveItemValueToNext();
		}
		public override void tMoveItemValueToNext()
		{
			this.bON = !this.bON;
		}
		public override void tMoveItemValueToPrevious()
		{
			this.tMoveItemValueToNext();
		}
		public void tInitialize(string str項目名, bool b初期状態, string str説明文jp, string str説明文en) {
			this.tInitialize(str項目名, b初期状態, CItemBase.EPanelType.Normal, str説明文jp, str説明文en);
		}
		public void tInitialize(string str項目名, bool b初期状態, CItemBase.EPanelType eパネル種別, string str説明文jp, string str説明文en) {
			base.tInitialize(str項目名, eパネル種別, str説明文jp, str説明文en);
			this.bON = b初期状態;
		}
		public override object obj現在値()
		{
			return ( this.bON ) ? "ON" : "OFF";
		}
		public override int GetIndex()
		{
			return ( this.bON ) ? 1 : 0;
		}
		public override void SetIndex( int index )
		{
			switch ( index )
			{
				case 0:
					this.bON = false;
					break;
				case 1:
					this.bON = true;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
