using System.Collections.Generic;
using System.Diagnostics;
using FDK;

namespace DTXMania
{
	internal class CStageStartup : CStage
	{
		// コンストラクタ

		public CStageStartup()
		{
			base.eStageID = CStage.EStage.Startup;
			base.bNotActivated = true;
		}

		public List<string> list進行文字列;

		// CStage 実装

		public override void OnActivate()
		{
			Trace.TraceInformation( "起動ステージを活性化します。" );
			Trace.Indent();
			try
			{
				this.list進行文字列 = new List<string>();
				base.ePhaseID = CStage.EPhase.Common_DefaultState;
				base.OnActivate();
				Trace.TraceInformation( "起動ステージの活性化を完了しました。" );
			}
			finally
			{
				Trace.Unindent();
			}
		}
		public override void OnDeactivate()
		{
			Trace.TraceInformation( "起動ステージを非活性化します。" );
			Trace.Indent();
			try
			{
				this.list進行文字列 = null;
				if ( es != null )
				{
					if ( ( es.thDTXFileEnumerate != null ) && es.thDTXFileEnumerate.IsAlive )
					{
						Trace.TraceWarning( "リスト構築スレッドを強制停止します。" );
						es.thDTXFileEnumerate.Abort();
						es.thDTXFileEnumerate.Join();
					}
				}
				base.OnDeactivate();
				Trace.TraceInformation( "起動ステージの非活性化を完了しました。" );
			}
			finally
			{
				Trace.Unindent();
			}
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
                this.tx背景 = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\1_background.jpg"), false);
				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if( !base.bNotActivated )
			{
				CDTXMania.tReleaseTexture( ref this.tx背景 );
				base.OnManagedReleaseResources();
			}
		}
		public override int OnUpdateAndDraw()
		{
			if( !base.bNotActivated )
			{
				if( base.bJustStartedUpdate )
				{
					this.list進行文字列.Add( "DTXMania powered by YAMAHA Silent Session Drums\n" );
					this.list進行文字列.Add( "Release: " + CDTXMania.VERSION + " [" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "]" );

					es = new CEnumSongs();
					es.StartEnumFromCache();										// 曲リスト取得(別スレッドで実行される)
					base.bJustStartedUpdate = false;
					return 0;
				}

				// CSongManager s管理 = CDTXMania.SongManager;

				if( this.tx背景 != null )
					this.tx背景.tDraw2D( CDTXMania.app.Device, 0, 0 );

				#region [ this.str現在進行中 の決定 ]
				//-----------------
				switch( base.ePhaseID )
				{
					case CStage.EPhase.起動0_システムサウンドを構築:
						this.str現在進行中 = "Loading system sounds ... ";
						break;

					case CStage.EPhase.起動00_songlistから曲リストを作成する:
						this.str現在進行中 = "Loading songlist.db ... ";
						break;

					case CStage.EPhase.起動1_SongsDBからスコアキャッシュを構築:
						this.str現在進行中 = "Loading songs.db ... ";
						break;

					case CStage.EPhase.起動2_曲を検索してリストを作成する:
						this.str現在進行中 = string.Format( "{0} ... {1}", "Enumerating songs", es.Songs管理.nNbScoresFound );
						break;

					case CStage.EPhase.起動3_スコアキャッシュをリストに反映する:
						this.str現在進行中 = string.Format( "{0} ... {1}/{2}", "Loading score properties from songs.db", es.Songs管理.nNbScoresFromScoreCache, es.Songs管理.nNbScoresFound );
						break;

					case CStage.EPhase.起動4_スコアキャッシュになかった曲をファイルから読み込んで反映する:
						this.str現在進行中 = string.Format( "{0} ... {1}/{2}", "Loading score properties from files", es.Songs管理.nNbScoresFromFile, es.Songs管理.nNbScoresFound - es.Songs管理.nNbScoresFromScoreCache );
						break;

					case CStage.EPhase.起動5_曲リストへ後処理を適用する:
						this.str現在進行中 = string.Format( "{0} ... ", "Building songlists" );
						break;

					case CStage.EPhase.起動6_スコアキャッシュをSongsDBに出力する:
						this.str現在進行中 = string.Format( "{0} ... ", "Saving songs.db" );
						break;

					case CStage.EPhase.起動7_完了:
						this.str現在進行中 = "Setup done.";
						break;
				}
				//-----------------
				#endregion
				#region [ this.list進行文字列＋this.現在進行中 の表示 ]
				//-----------------
				lock( this.list進行文字列 )
				{
					int x = 0;
					int y = 0;
					foreach( string str in this.list進行文字列 )
					{
						CDTXMania.actDisplayString.tPrint( x, y, CCharacterConsole.EFontType.AshThin, str );
						y += 14;
					}
					CDTXMania.actDisplayString.tPrint( x, y, CCharacterConsole.EFontType.AshThin, this.str現在進行中 );
				}
				//-----------------
				#endregion

				if( es != null && es.IsSongListEnumCompletelyDone )							// 曲リスト作成が終わったら
				{
					CDTXMania.SongManager = ( es != null ) ? es.Songs管理 : null;		// 最後に、曲リストを拾い上げる
					return 1;
				}
			}
			return 0;
		}


		// Other

		#region [ private ]
		//-----------------
		private string str現在進行中 = "";
		private CTexture tx背景;
		private CEnumSongs es;
		
		#endregion
	}
}
