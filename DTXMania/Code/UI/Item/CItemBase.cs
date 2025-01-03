using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace DTXMania
{
	/// <summary>
	/// すべてのアイテムの基本クラス。
	/// Base class for all items.
	/// </summary>
	internal class CItemBase
	{
		// プロパティ

		public EPanelType ePanelType;
		public enum EPanelType
		{
			Normal,
			Other
		}

		public EType eType;
		public enum EType
		{
			基本形,
			ONorOFFToggle,
			ONorOFForUndefined3State,
			Integer,
			List,
			切替リスト
		}

		public string strItemName;
		public string str説明文;


		// コンストラクタ

		public CItemBase()
		{
			this.strItemName = "";
			this.str説明文 = "";
		}
		
		public CItemBase(string str項目名,  string str説明文jp, string str説明文en)
			: this() {
			this.tInitialize(str項目名, str説明文jp, str説明文en);
		}

		public CItemBase(string str項目名, EPanelType eパネル種別, string str説明文jp, string str説明文en)
			: this() {
			this.tInitialize(str項目名, eパネル種別, str説明文jp, str説明文en);
		}
		
		// メソッド；子クラスで実装する
		
		//This will allow simplifying the code inside CActConfigList.cs
		public Action action;

		public void RunAction()
		{
			tEnter押下();

			action?.Invoke();
		}

		//existing method which gets inherited by CItemInteger, CItemList, etc
		protected virtual void tEnter押下()
		{
		}
		public virtual void tMoveItemValueToNext()
		{
		}
		public virtual void tMoveItemValueToPrevious()
		{
		}

		public virtual void tInitialize(string str項目名, string str説明文jp, string str説明文en) {
			this.tInitialize(str項目名, EPanelType.Normal, str説明文jp, str説明文en);
		}
		
		public virtual void tInitialize(string str項目名, EPanelType eパネル種別, string str説明文jp, string str説明文en) {
			this.strItemName = str項目名;
			this.ePanelType = eパネル種別;
			this.str説明文 = CDTXMania.isJapanese ? str説明文jp : str説明文en;
		}
		public virtual object obj現在値()
		{
			return null;
		}
		public virtual int GetIndex()
		{
			return 0;
		}
		public virtual void SetIndex( int index )
		{
		}

		private Action _readFromConfig;
		public virtual void ReadFromConfig()
		{
			_readFromConfig?.Invoke();
		}

		private Action _writeToConfig;
		public virtual void WriteToConfig()
		{
			_writeToConfig?.Invoke();
		}
		
		public void BindConfig(Action readFromConfig, Action writeToConfig)
		{
			this._readFromConfig = readFromConfig;
			this._writeToConfig = writeToConfig;
		}
	}
}
