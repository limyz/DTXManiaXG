using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DTXMania
{
	internal class CActSortSongs : CActSelectPopupMenu
	{

		// コンストラクタ

		public CActSortSongs()
		{
			List<CItemBase> lci = new List<CItemBase>();
			lci.Add( new CItemList( "Title",		CItemBase.EPanelType.Normal, 0, "", "", new string[] { "Z,Y,X,...",		"A,B,C,..." } ) );
			lci.Add( new CItemList( "Level",		CItemBase.EPanelType.Normal, 0, "", "", new string[] { "99,98,97,...",	"1,2,3,..." } ) );
			lci.Add( new CItemList( "Best Rank",	CItemBase.EPanelType.Normal, 0, "", "", new string[] { "E,D,C,...",		"SS,S,A,..." } ) );
			lci.Add( new CItemList( "PlayCount",	CItemBase.EPanelType.Normal, 0, "", "", new string[] { "10,9,8,...",		"1,2,3,..." } ) );
			lci.Add( new CItemList( "Author",		CItemBase.EPanelType.Normal, 0, "", "", new string[] { "Z,Y,X,...",		"A,B,C,..." } ) );
			lci.Add( new CItemList( "SkillPoint",	CItemBase.EPanelType.Normal, 0, "", "", new string[] { "100,99,98,...",	"1,2,3,..." } ) );
#if TEST_SORTBGM
			lci.Add( new CItemList( "BPM",			CItemBase.Eパネル種別.通常, 0, "", "", new string[] { "300,200,...",	"70,80,90,..." } ) );
#endif
			lci.Add( new CItemList( "Date",			CItemBase.EPanelType.Normal, 0, "", "", new string[] { "Dec.31,30,...",	"Jan.1,2,..." } ) );
			lci.Add( new CItemList( "Return",		CItemBase.EPanelType.Normal, 0, "", "", new string[] { "", 				"" } ) );
			
			base.Initialize( lci, "SORT MENU" );
		}


		// メソッド
		public void tActivatePopupMenu( EInstrumentPart einst, ref CActSelectSongList ca )
		{
		    this.actSongList = ca;
			base.tActivatePopupMenu( einst );
		}
		//public void tDeativatePopupMenu()
		//{
		//	base.tDeativatePopupMenu();
		//}


		public override void tPressEnterMain( int nSortOrder)  // tEnter押下Main
		{
			nSortOrder *= 2;	// 0,1  => -1, 1
			nSortOrder -= 1;
			switch ( nCurrentSelection )
			{
				case (int) EOrder.Title:
					this.actSongList.tSortSongList(
						CDTXMania.SongManager.t曲リストのソート2_タイトル順, eInst, nSortOrder
					);
					this.actSongList.t選択曲が変更された(true);
					break;
				case (int) EOrder.Level:
					this.actSongList.tSortSongList(
						CDTXMania.SongManager.t曲リストのソート4_LEVEL順, eInst, nSortOrder,
						this.actSongList.n現在のアンカ難易度レベル
					);
					this.actSongList.t選択曲が変更された( true );
					break;
				case (int) EOrder.BestRank:
					this.actSongList.tSortSongList(
						CDTXMania.SongManager.t曲リストのソート5_BestRank順, eInst, nSortOrder,
						this.actSongList.n現在のアンカ難易度レベル
					);
					break;
				case (int) EOrder.PlayCount:
					// this.actSongList.t曲リストのソート3_演奏回数の多い順( eInst, nSortOrder );
					this.actSongList.tSortSongList(
						CDTXMania.SongManager.t曲リストのソート3_演奏回数の多い順, eInst, nSortOrder,
						this.actSongList.n現在のアンカ難易度レベル
					);
					this.actSongList.t選択曲が変更された( true );
					break;
				case (int) EOrder.Author:
					this.actSongList.tSortSongList(
						CDTXMania.SongManager.t曲リストのソート8_アーティスト名順, eInst, nSortOrder,
						this.actSongList.n現在のアンカ難易度レベル
					);
					this.actSongList.t選択曲が変更された( true );
					break;
				case (int) EOrder.SkillPoint:
					this.actSongList.tSortSongList(
						CDTXMania.SongManager.t曲リストのソート6_SkillPoint順, eInst, nSortOrder,
						this.actSongList.n現在のアンカ難易度レベル
					);
					this.actSongList.t選択曲が変更された( true );
					break;
#if TEST_SORTBGM
						case (int) ESortItem.BPM:
						this.act曲リスト.t曲リストのソート(
							CDTXMania.Songs管理.t曲リストのソート9_BPM順, eInst, nSortOrder,
							this.act曲リスト.n現在のアンカ難易度レベル
						);
					this.act曲リスト.t選択曲が変更された(true);
						break;
#endif
				case (int) EOrder.Date:
					this.actSongList.tSortSongList(
						CDTXMania.SongManager.t曲リストのソート7_更新日時順, eInst, nSortOrder,
						this.actSongList.n現在のアンカ難易度レベル
					);
					this.actSongList.t選択曲が変更された( true );
					break;
				case (int) EOrder.Return:
					this.tDeativatePopupMenu();
					break;
				default:
					break;
			}
		}
		
		// CActivity 実装

		public override void OnActivate()
		{
			base.OnActivate();
		}
		public override void OnDeactivate()
		{
			if( !base.bNotActivated )
			{
				base.OnDeactivate();
			}
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			base.OnManagedReleaseResources();
		}

		#region [ private ]
		//-----------------

		private CActSelectSongList actSongList;  // act曲リスト

		private enum EOrder : int
		{
			Title = 0, Level, BestRank, PlayCount,
			Author,
			SkillPoint,
#if TEST_SORTBGM
			BPM,
#endif
			Date,
			Return, END,
			Default = 99
		};
		//-----------------
		#endregion
	}


}
