using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace DTXMania
{
	[Serializable]
	internal class CSongManager
	{
		// Properties

		public int nNbScoresFromSongsDB
		{
			get; 
			set; 
		}
		public int nNbScoresForSongsDB
		{
			get;
			set;
		}
		public int nNbScoresFromScoreCache 
		{
			get;
			set; 
		}
		public int nNbScoresFromFile
		{
			get;
			set;
		}
		public int nNbScoresFound 
		{ 
			get;
			set;
		}
		public int nNbSongNodesFound
		{
			get; 
			set;
		}
		[NonSerialized]
		public List<CScore> listSongsDB;					// songs.dbから構築されるlist
		public List<CSongListNode> listSongRoot;            // 起動時にフォルダ検索して構築されるlist
		public List<CSongListNode> listSongBeforeSearch = null;    // The SongListNode before a search is performed
		public bool bIsSuspending							// 外部スレッドから、内部スレッドのsuspendを指示する時にtrueにする
		{													// 再開時は、これをfalseにしてから、次のautoReset.Set()を実行する
			get;
			set;
		}
		public bool bIsSlowdown								// #PREMOVIE再生時に曲検索を遅くする
		{
			get;
			set;
		}
		[NonSerialized]
		private AutoResetEvent autoReset;
		public AutoResetEvent AutoReset
		{
			get
			{
				return autoReset;
			}
			private set
			{
				autoReset = value;
			}
		}

		private int searchCount;							// #PREMOVIE中は検索n回実行したら少しスリープする

		// コンストラクタ

		public CSongManager()
		{
			this.listSongsDB = new List<CScore>();
			this.listSongRoot = new List<CSongListNode>();
			this.nNbSongNodesFound = 0;
			this.nNbScoresFound = 0;
			this.bIsSuspending = false;						// #27060
			this.autoReset = new AutoResetEvent( true );	// #27060
			this.searchCount = 0;
		}


		// Methods

		#region [ Read SongsDB(songs.db) ]
		//-----------------
		public void tReadSongsDB( string SongsDBFilename )
		{
			this.nNbScoresFromSongsDB = 0;
			if( File.Exists( SongsDBFilename ) )
			{
				BinaryReader br = null;
				try
				{
					br = new BinaryReader( File.OpenRead( SongsDBFilename ) );
					if ( !br.ReadString().Equals( SONGSDB_VERSION ) )
					{
						throw new InvalidDataException( "ヘッダが異なります。" );
					}
					this.listSongsDB = new List<CScore>();

					while( true )
					{
						try
						{
							CScore item = this.tReadOneScoreFromSongsDB( br );
							this.listSongsDB.Add( item );
							this.nNbScoresFromSongsDB++;
						}
						catch( EndOfStreamException )
						{
							break;
						}
					}
				}
				finally
				{
					if( br != null )
						br.Close();
				}
			}
		}
		//-----------------
		#endregion

		#region [ Search songs and create a list ]
		//-----------------
		public void tSearchSongsAndCreateList( string str基点フォルダ, bool b子BOXへ再帰する )
		{
			this.tSearchSongsAndCreateList( str基点フォルダ, b子BOXへ再帰する, this.listSongRoot, null );
		}
		private void tSearchSongsAndCreateList( string str基点フォルダ, bool b子BOXへ再帰する, List<CSongListNode> listノードリスト, CSongListNode node親 )
		{
			if( !str基点フォルダ.EndsWith( @"\" ) )
				str基点フォルダ = str基点フォルダ + @"\";

			DirectoryInfo info = new DirectoryInfo( str基点フォルダ );

			if( CDTXMania.ConfigIni.bLogSongSearch )
				Trace.TraceInformation( "基点フォルダ: " + str基点フォルダ );

			#region [ a.フォルダ内に set.def が存在する場合 → set.def からノード作成]
			//-----------------------------
			string path = str基点フォルダ + "set.def";
			if( File.Exists( path ) )
			{
				CSetDef def = new CSetDef( path );
				new FileInfo( path );
				if( CDTXMania.ConfigIni.bLogSongSearch )
				{
					Trace.TraceInformation( "set.def検出 : {0}", path );
					Trace.Indent();
				}
				try
				{
					SlowOrSuspendSearchTask();		// #27060 中断要求があったら、解除要求が来るまで待機, #PREMOVIE再生中は検索負荷を落とす
					for( int i = 0; i < def.blocks.Count; i++ )
					{
						CSetDef.CBlock block = def.blocks[ i ];
						CSongListNode item = new CSongListNode();
						item.eNodeType = CSongListNode.ENodeType.SCORE;
						item.strタイトル = block.Title;
						item.strジャンル = block.Genre;
						item.nスコア数 = 0;
						item.col文字色 = block.FontColor;
						item.SetDefのブロック番号 = i;
						item.pathSetDefの絶対パス = path;
						item.r親ノード = node親;

						item.strBreadcrumbs = ( item.r親ノード == null ) ?
							path + i : item.r親ノード.strBreadcrumbs + " > " + path + i;

						for( int j = 0; j < 5; j++ )
						{
							if( !string.IsNullOrEmpty( block.File[ j ] ) )
							{
								string str2 = str基点フォルダ + block.File[ j ];
								if( File.Exists( str2 ) )
								{
									item.arDifficultyLabel[ j ] = block.Label[ j ];
									item.arScore[ j ] = new CScore();
									item.arScore[ j ].FileInformation.AbsoluteFilePath = str2;
									item.arScore[ j ].FileInformation.AbsoluteFolderPath = Path.GetFullPath( Path.GetDirectoryName( str2 ) ) + @"\";
									FileInfo info2 = new FileInfo( str2 );
									item.arScore[ j ].FileInformation.FileSize = info2.Length;
									item.arScore[ j ].FileInformation.LastModified = info2.LastWriteTime;
									string str3 = str2 + ".score.ini";
									if( File.Exists( str3 ) )
									{
										FileInfo info3 = new FileInfo( str3 );
										item.arScore[ j ].ScoreIniInformation.FileSize = info3.Length;
										item.arScore[ j ].ScoreIniInformation.LastModified = info3.LastWriteTime;
									}
									item.nスコア数++;
									this.nNbScoresFound++;
								}
								else
								{
									item.arScore[ j ] = null;
								}
							}
						}
						if( item.nスコア数 > 0 )
						{
							listノードリスト.Add( item );
							this.nNbSongNodesFound++;
							if( CDTXMania.ConfigIni.bLogSongSearch )
							{
								StringBuilder builder = new StringBuilder( 0x200 );
								builder.Append( string.Format( "nID#{0:D3}", item.nID ) );
								if( item.r親ノード != null )
								{
									builder.Append( string.Format( "(in#{0:D3}):", item.r親ノード.nID ) );
								}
								else
								{
									builder.Append( "(onRoot):" );
								}
								if( ( item.strタイトル != null ) && ( item.strタイトル.Length > 0 ) )
								{
									builder.Append( " SONG, Title=" + item.strタイトル );
								}
								if( ( item.strジャンル != null ) && ( item.strジャンル.Length > 0 ) )
								{
									builder.Append( ", Genre=" + item.strジャンル );
								}
								if( item.col文字色 != Color.White )
								{
									builder.Append( ", FontColor=" + item.col文字色 );
								}
								Trace.TraceInformation( builder.ToString() );
								Trace.Indent();
								try
								{
									for( int k = 0; k < 5; k++ )
									{
										if( item.arScore[ k ] != null )
										{
											CScore cスコア = item.arScore[ k ];
											builder.Remove( 0, builder.Length );
											builder.Append( string.Format( "ブロック{0}-{1}:", item.SetDefのブロック番号 + 1, k + 1 ) );
											builder.Append( " Label=" + item.arDifficultyLabel[ k ] );
											builder.Append( ", File=" + cスコア.FileInformation.AbsoluteFilePath );
											builder.Append( ", Size=" + cスコア.FileInformation.FileSize );
											builder.Append( ", LastUpdate=" + cスコア.FileInformation.LastModified );
											Trace.TraceInformation( builder.ToString() );
										}
									}
								}
								finally
								{
									Trace.Unindent();
								}
							}
						}
					}
				}
				finally
				{
					if( CDTXMania.ConfigIni.bLogSongSearch )
					{
						Trace.Unindent();
					}
				}
			}
			//-----------------------------
			#endregion

			#region [ b.フォルダ内に set.def が存在しない場合 → 個別ファイルからノード作成 ]
			//-----------------------------
			else
			{
				foreach( FileInfo fileinfo in info.GetFiles() )
				{
					SlowOrSuspendSearchTask();		// #27060 中断要求があったら、解除要求が来るまで待機, #PREMOVIE再生中は検索負荷を落とす
					string strExt = fileinfo.Extension.ToLower();
					if( ( strExt.Equals( ".dtx" ) || strExt.Equals( ".gda" ) ) || ( ( strExt.Equals( ".g2d" ) || strExt.Equals( ".bms" ) ) || strExt.Equals( ".bme" ) ) )
					{
						CSongListNode c曲リストノード = new CSongListNode();
						c曲リストノード.eNodeType = CSongListNode.ENodeType.SCORE;
						c曲リストノード.nスコア数 = 1;
						c曲リストノード.r親ノード = node親;

						c曲リストノード.strBreadcrumbs = ( c曲リストノード.r親ノード == null ) ?
							str基点フォルダ + fileinfo.Name : c曲リストノード.r親ノード.strBreadcrumbs + " > " + str基点フォルダ + fileinfo.Name;

						c曲リストノード.arScore[ 0 ] = new CScore();
						c曲リストノード.arScore[ 0 ].FileInformation.AbsoluteFilePath = str基点フォルダ + fileinfo.Name;
						c曲リストノード.arScore[ 0 ].FileInformation.AbsoluteFolderPath = str基点フォルダ;
						c曲リストノード.arScore[ 0 ].FileInformation.FileSize = fileinfo.Length;
						c曲リストノード.arScore[ 0 ].FileInformation.LastModified = fileinfo.LastWriteTime;
						string strFileNameScoreIni = c曲リストノード.arScore[ 0 ].FileInformation.AbsoluteFilePath + ".score.ini";
						if( File.Exists( strFileNameScoreIni ) )
						{
							FileInfo infoScoreIni = new FileInfo( strFileNameScoreIni );
							c曲リストノード.arScore[ 0 ].ScoreIniInformation.FileSize = infoScoreIni.Length;
							c曲リストノード.arScore[ 0 ].ScoreIniInformation.LastModified = infoScoreIni.LastWriteTime;
						}
						this.nNbScoresFound++;
						listノードリスト.Add( c曲リストノード );
						this.nNbSongNodesFound++;
						if( CDTXMania.ConfigIni.bLogSongSearch )
						{
							Trace.Indent();
							try
							{
								StringBuilder sb = new StringBuilder( 0x100 );
								sb.Append( string.Format( "nID#{0:D3}", c曲リストノード.nID ) );
								if( c曲リストノード.r親ノード != null )
								{
									sb.Append( string.Format( "(in#{0:D3}):", c曲リストノード.r親ノード.nID ) );
								}
								else
								{
									sb.Append( "(onRoot):" );
								}
								sb.Append( " SONG, File=" + c曲リストノード.arScore[ 0 ].FileInformation.AbsoluteFilePath );
								sb.Append( ", Size=" + c曲リストノード.arScore[ 0 ].FileInformation.FileSize );
								sb.Append( ", LastUpdate=" + c曲リストノード.arScore[ 0 ].FileInformation.LastModified );
								Trace.TraceInformation( sb.ToString() );
							}
							finally
							{
								Trace.Unindent();
							}
						}
					}
					else if( strExt.Equals( ".mid" ) || strExt.Equals( ".smf" ))
					{
						// DoNothing
					}
				}
			}
			//-----------------------------
			#endregion

			foreach( DirectoryInfo infoDir in info.GetDirectories() )
			{
				SlowOrSuspendSearchTask();		// #27060 中断要求があったら、解除要求が来るまで待機, #PREMOVIE再生中は検索負荷を落とす

				#region [ a. "dtxfiles." で始まるフォルダの場合 ]
				//-----------------------------
				if( infoDir.Name.ToLower().StartsWith( "dtxfiles." ) )
				{
					CSongListNode c曲リストノード = new CSongListNode();
					c曲リストノード.eNodeType = CSongListNode.ENodeType.BOX;
					c曲リストノード.bDTXFilesで始まるフォルダ名のBOXである = true;
					c曲リストノード.strタイトル = infoDir.Name.Substring( 9 );
					c曲リストノード.nスコア数 = 1;
					c曲リストノード.r親ノード = node親;

					// 一旦、上位BOXのスキン情報をコピー (後でbox.defの記載にて上書きされる場合がある)
					c曲リストノード.strSkinPath = ( c曲リストノード.r親ノード == null ) ?
						"" : c曲リストノード.r親ノード.strSkinPath;

					c曲リストノード.strBreadcrumbs = ( c曲リストノード.r親ノード == null ) ?
						c曲リストノード.strタイトル : c曲リストノード.r親ノード.strBreadcrumbs + " > " + c曲リストノード.strタイトル;

		
					c曲リストノード.list子リスト = new List<CSongListNode>();
					c曲リストノード.arScore[ 0 ] = new CScore();
					c曲リストノード.arScore[ 0 ].FileInformation.AbsoluteFolderPath = infoDir.FullName + @"\";
					c曲リストノード.arScore[ 0 ].SongInformation.Title = c曲リストノード.strタイトル;
					c曲リストノード.arScore[ 0 ].SongInformation.Comment =
						(CDTXMania.isJapanese) ?
						"BOX に移動します。" :
						"Enter into the BOX.";
					listノードリスト.Add(c曲リストノード);
					if( File.Exists( infoDir.FullName + @"\box.def" ) )
					{
						CBoxDef boxdef = new CBoxDef( infoDir.FullName + @"\box.def" );
						if( ( boxdef.Title != null ) && ( boxdef.Title.Length > 0 ) )
						{
							c曲リストノード.strタイトル = boxdef.Title;
						}
						if( ( boxdef.Genre != null ) && ( boxdef.Genre.Length > 0 ) )
						{
							c曲リストノード.strジャンル = boxdef.Genre;
						}
						if( boxdef.Color != Color.White )
						{
							c曲リストノード.col文字色 = boxdef.Color;
						}
						if( ( boxdef.Artist != null ) && ( boxdef.Artist.Length > 0 ) )
						{
							c曲リストノード.arScore[ 0 ].SongInformation.ArtistName = boxdef.Artist;
						}
						if( ( boxdef.Comment != null ) && ( boxdef.Comment.Length > 0 ) )
						{
							c曲リストノード.arScore[ 0 ].SongInformation.Comment = boxdef.Comment;
						}
						if( ( boxdef.Preimage != null ) && ( boxdef.Preimage.Length > 0 ) )
						{
							c曲リストノード.arScore[ 0 ].SongInformation.Preimage = boxdef.Preimage;
						}
						if( ( boxdef.Premovie != null ) && ( boxdef.Premovie.Length > 0 ) )
						{
							c曲リストノード.arScore[ 0 ].SongInformation.Premovie = boxdef.Premovie;
						}
						if( ( boxdef.Presound != null ) && ( boxdef.Presound.Length > 0 ) )
						{
							c曲リストノード.arScore[ 0 ].SongInformation.Presound = boxdef.Presound;
						}
						if ( boxdef.SkinPath != null )
						{
							if ( boxdef.SkinPath == "" )
							{
								// box.defにスキン情報が記載されていないなら、上位BOXのスキン情報をコピー
								c曲リストノード.strSkinPath = ( c曲リストノード.r親ノード == null ) ?
									"" : c曲リストノード.r親ノード.strSkinPath;
							}
							else
							{
								// box.defに記載されているスキン情報をコピー。末尾に必ず\をつけておくこと。
								string s = System.IO.Path.Combine( infoDir.FullName, boxdef.SkinPath );
								if ( s[ s.Length - 1 ] != System.IO.Path.DirectorySeparatorChar )	// フォルダ名末尾に\を必ずつけて、CSkin側と表記を統一する
								{
									s += System.IO.Path.DirectorySeparatorChar;
								}
								if ( CDTXMania.Skin.bIsValid( s ) )
								{
									c曲リストノード.strSkinPath = s;
								}
								else
								{
									c曲リストノード.strSkinPath = ( c曲リストノード.r親ノード == null ) ?
										"" : c曲リストノード.r親ノード.strSkinPath;
								}
							}
						}

						// copy hit ranges from the box.def
						// these can always be copied regardless of being set,
						// as song list nodes and boxdefs use the same method to indicate an unset range
						c曲リストノード.stDrumHitRanges = boxdef.stDrumHitRanges;
						c曲リストノード.stDrumPedalHitRanges = boxdef.stDrumPedalHitRanges;
						c曲リストノード.stGuitarHitRanges = boxdef.stGuitarHitRanges;
						c曲リストノード.stBassHitRanges = boxdef.stBassHitRanges;
					}
					if( CDTXMania.ConfigIni.bLogSongSearch )
					{
						Trace.Indent();
						try
						{
							StringBuilder sb = new StringBuilder( 0x100 );
							sb.Append( string.Format( "nID#{0:D3}", c曲リストノード.nID ) );
							if( c曲リストノード.r親ノード != null )
							{
								sb.Append( string.Format( "(in#{0:D3}):", c曲リストノード.r親ノード.nID ) );
							}
							else
							{
								sb.Append( "(onRoot):" );
							}
							sb.Append( " BOX, Title=" + c曲リストノード.strタイトル );
							sb.Append( ", Folder=" + c曲リストノード.arScore[ 0 ].FileInformation.AbsoluteFolderPath );
							sb.Append( ", Comment=" + c曲リストノード.arScore[ 0 ].SongInformation.Comment );
							sb.Append( ", SkinPath=" + c曲リストノード.strSkinPath );
							Trace.TraceInformation( sb.ToString() );
						}
						finally
						{
							Trace.Unindent();
						}
					}
					if( b子BOXへ再帰する )
					{
						this.tSearchSongsAndCreateList( infoDir.FullName + @"\", b子BOXへ再帰する, c曲リストノード.list子リスト, c曲リストノード );
					}
				}
				//-----------------------------
				#endregion

				#region [ b.box.def を含むフォルダの場合  ]
				//-----------------------------
				else if( File.Exists( infoDir.FullName + @"\box.def" ) )
				{
					CBoxDef boxdef = new CBoxDef( infoDir.FullName + @"\box.def" );
					CSongListNode c曲リストノード = new CSongListNode();
					c曲リストノード.eNodeType = CSongListNode.ENodeType.BOX;
					c曲リストノード.bDTXFilesで始まるフォルダ名のBOXである = false;
					c曲リストノード.strタイトル = boxdef.Title;
					c曲リストノード.strジャンル = boxdef.Genre;
					c曲リストノード.col文字色 = boxdef.Color;
					c曲リストノード.nスコア数 = 1;
					c曲リストノード.arScore[ 0 ] = new CScore();
					c曲リストノード.arScore[ 0 ].FileInformation.AbsoluteFolderPath = infoDir.FullName + @"\";
					c曲リストノード.arScore[ 0 ].SongInformation.Title = boxdef.Title;
					c曲リストノード.arScore[ 0 ].SongInformation.Genre = boxdef.Genre;
					c曲リストノード.arScore[ 0 ].SongInformation.ArtistName = boxdef.Artist;
					c曲リストノード.arScore[ 0 ].SongInformation.Comment = boxdef.Comment;
					c曲リストノード.arScore[ 0 ].SongInformation.Preimage = boxdef.Preimage;
					c曲リストノード.arScore[ 0 ].SongInformation.Premovie = boxdef.Premovie;
					c曲リストノード.arScore[ 0 ].SongInformation.Presound = boxdef.Presound;
					c曲リストノード.r親ノード = node親;

					if ( boxdef.SkinPath == "" )
					{
						// box.defにスキン情報が記載されていないなら、上位BOXのスキン情報をコピー
						c曲リストノード.strSkinPath = ( c曲リストノード.r親ノード == null ) ?
							"" : c曲リストノード.r親ノード.strSkinPath;
					}
					else
					{
						// box.defに記載されているスキン情報をコピー。末尾に必ず\をつけておくこと。
						string s = System.IO.Path.Combine( infoDir.FullName, boxdef.SkinPath );
						if ( s[ s.Length - 1 ] != System.IO.Path.DirectorySeparatorChar )	// フォルダ名末尾に\を必ずつけて、CSkin側と表記を統一する
						{
							s += System.IO.Path.DirectorySeparatorChar;
						}
						if ( CDTXMania.Skin.bIsValid( s ) )
						{
							c曲リストノード.strSkinPath = s;
						}
						else
						{
							c曲リストノード.strSkinPath = ( c曲リストノード.r親ノード == null ) ?
								"" : c曲リストノード.r親ノード.strSkinPath;
						}
					}
					c曲リストノード.strBreadcrumbs = ( c曲リストノード.r親ノード == null ) ?
						c曲リストノード.strタイトル : c曲リストノード.r親ノード.strBreadcrumbs + " > " + c曲リストノード.strタイトル;
	
					
					c曲リストノード.list子リスト = new List<CSongListNode>();
					c曲リストノード.stDrumHitRanges = boxdef.stDrumHitRanges;
					c曲リストノード.stDrumPedalHitRanges = boxdef.stDrumPedalHitRanges;
					c曲リストノード.stGuitarHitRanges = boxdef.stGuitarHitRanges;
					c曲リストノード.stBassHitRanges = boxdef.stBassHitRanges;
					listノードリスト.Add( c曲リストノード );
					if( CDTXMania.ConfigIni.bLogSongSearch )
					{
						Trace.TraceInformation( "box.def検出 : {0}", infoDir.FullName + @"\box.def" );
						Trace.Indent();
						try
						{
							StringBuilder sb = new StringBuilder( 0x400 );
							sb.Append( string.Format( "nID#{0:D3}", c曲リストノード.nID ) );
							if( c曲リストノード.r親ノード != null )
							{
								sb.Append( string.Format( "(in#{0:D3}):", c曲リストノード.r親ノード.nID ) );
							}
							else
							{
								sb.Append( "(onRoot):" );
							}
							sb.Append( "BOX, Title=" + c曲リストノード.strタイトル );
							if( ( c曲リストノード.strジャンル != null ) && ( c曲リストノード.strジャンル.Length > 0 ) )
							{
								sb.Append( ", Genre=" + c曲リストノード.strジャンル );
							}
							if( ( c曲リストノード.arScore[ 0 ].SongInformation.ArtistName != null ) && ( c曲リストノード.arScore[ 0 ].SongInformation.ArtistName.Length > 0 ) )
							{
								sb.Append( ", Artist=" + c曲リストノード.arScore[ 0 ].SongInformation.ArtistName );
							}
							if( ( c曲リストノード.arScore[ 0 ].SongInformation.Comment != null ) && ( c曲リストノード.arScore[ 0 ].SongInformation.Comment.Length > 0 ) )
							{
								sb.Append( ", Comment=" + c曲リストノード.arScore[ 0 ].SongInformation.Comment );
							}
							if( ( c曲リストノード.arScore[ 0 ].SongInformation.Preimage != null ) && ( c曲リストノード.arScore[ 0 ].SongInformation.Preimage.Length > 0 ) )
							{
								sb.Append( ", Preimage=" + c曲リストノード.arScore[ 0 ].SongInformation.Preimage );
							}
							if( ( c曲リストノード.arScore[ 0 ].SongInformation.Premovie != null ) && ( c曲リストノード.arScore[ 0 ].SongInformation.Premovie.Length > 0 ) )
							{
								sb.Append( ", Premovie=" + c曲リストノード.arScore[ 0 ].SongInformation.Premovie );
							}
							if( ( c曲リストノード.arScore[ 0 ].SongInformation.Presound != null ) && ( c曲リストノード.arScore[ 0 ].SongInformation.Presound.Length > 0 ) )
							{
								sb.Append( ", Presound=" + c曲リストノード.arScore[ 0 ].SongInformation.Presound );
							}
							if( c曲リストノード.col文字色 != ColorTranslator.FromHtml( "White" ) )
							{
								sb.Append( ", FontColor=" + c曲リストノード.col文字色 );
							}

							// hit ranges
							tTryAppendHitRanges(c曲リストノード.stDrumHitRanges, @"Drum", sb);
							tTryAppendHitRanges(c曲リストノード.stDrumPedalHitRanges, @"DrumPedal", sb);
							tTryAppendHitRanges(c曲リストノード.stGuitarHitRanges, @"Guitar", sb);
							tTryAppendHitRanges(c曲リストノード.stBassHitRanges, @"Bass", sb);

							if ( ( c曲リストノード.strSkinPath != null ) && ( c曲リストノード.strSkinPath.Length > 0 ) )
							{
								sb.Append( ", SkinPath=" + c曲リストノード.strSkinPath );
							}

							Trace.TraceInformation( sb.ToString() );
						}
						finally
						{
							Trace.Unindent();
						}
					}
					if( b子BOXへ再帰する )
					{
						this.tSearchSongsAndCreateList( infoDir.FullName + @"\", b子BOXへ再帰する, c曲リストノード.list子リスト, c曲リストノード );
					}
				}
				//-----------------------------
				#endregion

				#region [ c.通常フォルダの場合 ]
				//-----------------------------
				else
				{
					this.tSearchSongsAndCreateList( infoDir.FullName + @"\", b子BOXへ再帰する, listノードリスト, node親 );
				}
				//-----------------------------
				#endregion
			}
		}

		/// <summary>
		/// Append all the set values, if any, of the given <see cref="STHitRanges"/> to the given <see cref="StringBuilder"/>.
		/// </summary>
		/// <param name="stHitRanges">The <see cref="STHitRanges"/> to append the values of.</param>
		/// <param name="strName">The unique identifier of <paramref name="stHitRanges"/>.</param>
		/// <param name="builder">The <see cref="StringBuilder"/> to append to.</param>
		private void tTryAppendHitRanges(STHitRanges stHitRanges, string strName, StringBuilder builder)
		{
			if (stHitRanges.nPerfectSizeMs >= 0)
				builder.Append($@", {strName}Perfect={stHitRanges.nPerfectSizeMs}ms");

			if (stHitRanges.nGreatSizeMs >= 0)
				builder.Append($@", {strName}Great={stHitRanges.nGreatSizeMs}ms");

			if (stHitRanges.nGoodSizeMs >= 0)
				builder.Append($@", {strName}Good={stHitRanges.nGoodSizeMs}ms");

			if (stHitRanges.nPoorSizeMs >= 0)
				builder.Append($@", {strName}Poor={stHitRanges.nPoorSizeMs}ms");
		}

		//-----------------
		#endregion
		#region [ Reflect score cache in song list ]
		//-----------------
		public void tReflectScoreCacheInSongList()
		{
			this.nNbScoresFromScoreCache = 0;
			this.tReflectScoreCacheInSongList( this.listSongRoot );
		}
		private void tReflectScoreCacheInSongList( List<CSongListNode> ノードリスト )
		{
			using( List<CSongListNode>.Enumerator enumerator = ノードリスト.GetEnumerator() )
			{
				while( enumerator.MoveNext() )
				{
					SlowOrSuspendSearchTask();		// #27060 中断要求があったら、解除要求が来るまで待機, #PREMOVIE再生中は検索負荷を落とす

					CSongListNode node = enumerator.Current;
					if( node.eNodeType == CSongListNode.ENodeType.BOX )
					{
						this.tReflectScoreCacheInSongList( node.list子リスト );
					}
					else if( ( node.eNodeType == CSongListNode.ENodeType.SCORE ) || ( node.eNodeType == CSongListNode.ENodeType.SCORE_MIDI ) )
					{
						Predicate<CScore> match = null;
						for( int lv = 0; lv < 5; lv++ )
						{
							if( node.arScore[ lv ] != null )
							{
								if( match == null )
								{
									match = delegate( CScore sc )
									{
										return
											(
											( sc.FileInformation.AbsoluteFilePath.Equals( node.arScore[ lv ].FileInformation.AbsoluteFilePath )
											&& sc.FileInformation.FileSize.Equals( node.arScore[ lv ].FileInformation.FileSize ) )
											&& ( sc.FileInformation.LastModified.Equals( node.arScore[ lv ].FileInformation.LastModified )
											&& sc.ScoreIniInformation.FileSize.Equals( node.arScore[ lv ].ScoreIniInformation.FileSize ) ) )
											&& sc.ScoreIniInformation.LastModified.Equals( node.arScore[ lv ].ScoreIniInformation.LastModified );
									};
								}
								int nMatched = this.listSongsDB.FindIndex( match );
								if( nMatched == -1 )
								{
//Trace.TraceInformation( "songs.db に存在しません。({0})", node.arScore[ lv ].FileInformation.AbsoluteFilePath );
									if ( CDTXMania.ConfigIni.bLogSongSearch )
									{
										Trace.TraceInformation( "Not found in songs.db. ({0})", node.arScore[ lv ].FileInformation.AbsoluteFilePath );
									}
								}
								else
								{
									node.arScore[ lv ].SongInformation = this.listSongsDB[ nMatched ].SongInformation;
									node.arScore[ lv ].bHadACacheInSongDB = true;
									if( CDTXMania.ConfigIni.bLogSongSearch )
									{
										Trace.TraceInformation( "Transcribing data from songs.db. ({0})", node.arScore[ lv ].FileInformation.AbsoluteFilePath );
									}
									this.nNbScoresFromScoreCache++;
									if( node.arScore[ lv ].ScoreIniInformation.LastModified != this.listSongsDB[ nMatched ].ScoreIniInformation.LastModified )
									{
										string strFileNameScoreIni = node.arScore[ lv ].FileInformation.AbsoluteFilePath + ".score.ini";
										try
										{
											CScoreIni scoreIni = new CScoreIni( strFileNameScoreIni );
											scoreIni.tCheckIntegrity();
											for( int i = 0; i < 3; i++ )
											{
												int nSectionHiSkill = ( i * 2 ) + 1;
                                                int nSectionHiScore = i * 2;
												if(    scoreIni.stSection[ nSectionHiSkill ].bMIDIUsed
													|| scoreIni.stSection[ nSectionHiSkill ].bKeyboardUsed
													|| scoreIni.stSection[ nSectionHiSkill ].bJoypadUsed
													|| scoreIni.stSection[ nSectionHiSkill ].bMouseUsed )
												{
                                                    if (CDTXMania.ConfigIni.nSkillMode == 0)
                                                    {
                                                        node.arScore[lv].SongInformation.BestRank[i] =
                                                            (scoreIni.stFile.BestRank[i] != (int)CScoreIni.ERANK.UNKNOWN) ?
                                                            (int)scoreIni.stFile.BestRank[i] : CScoreIni.tCalculateRankOld(scoreIni.stSection[nSectionHiSkill]);
                                                    }
                                                    else
                                                    {
                                                        node.arScore[lv].SongInformation.BestRank[i] =
                                                            (scoreIni.stFile.BestRank[i] != (int)CScoreIni.ERANK.UNKNOWN) ?
                                                            (int)scoreIni.stFile.BestRank[i] : CScoreIni.tCalculateRank(scoreIni.stSection[nSectionHiSkill]);
                                                    }
												}
												else
												{
													node.arScore[ lv ].SongInformation.BestRank[ i ] = (int)CScoreIni.ERANK.UNKNOWN;
												}
												node.arScore[ lv ].SongInformation.HighSkill[ i ] = scoreIni.stSection[ nSectionHiSkill ].dbPerformanceSkill;
												node.arScore[ lv ].SongInformation.FullCombo[ i ] = scoreIni.stSection[ nSectionHiSkill ].bIsFullCombo | scoreIni.stSection[ nSectionHiScore ].bIsFullCombo;
											}
											node.arScore[ lv ].SongInformation.NbPerformances.Drums = scoreIni.stFile.PlayCountDrums;
											node.arScore[ lv ].SongInformation.NbPerformances.Guitar = scoreIni.stFile.PlayCountGuitar;
											node.arScore[ lv ].SongInformation.NbPerformances.Bass = scoreIni.stFile.PlayCountBass;
											for( int j = 0; j < 5; j++ )
											{
												node.arScore[ lv ].SongInformation.PerformanceHistory[ j ] = scoreIni.stFile.History[ j ];
											}
											if( CDTXMania.ConfigIni.bLogSongSearch )
											{
												Trace.TraceInformation("HiSkill information and performance history were retrieved from the performance record file. ({0})", strFileNameScoreIni );
											}
										}
										catch
										{
											Trace.TraceError("Failed to read the performance record file. ({0})", strFileNameScoreIni );
										}
									}
								}
							}
						}
					}
				}
			}
		}
		private CScore tReadOneScoreFromSongsDB( BinaryReader br )
		{
			CScore cスコア = new CScore();
			cスコア.FileInformation.AbsoluteFilePath = br.ReadString();
			cスコア.FileInformation.AbsoluteFolderPath = br.ReadString();
			cスコア.FileInformation.LastModified = new DateTime( br.ReadInt64() );
			cスコア.FileInformation.FileSize = br.ReadInt64();
			cスコア.ScoreIniInformation.LastModified = new DateTime( br.ReadInt64() );
			cスコア.ScoreIniInformation.FileSize = br.ReadInt64();
			cスコア.SongInformation.Title = br.ReadString();
			cスコア.SongInformation.ArtistName = br.ReadString();
			cスコア.SongInformation.Comment = br.ReadString();
			cスコア.SongInformation.Genre = br.ReadString();
			cスコア.SongInformation.Preimage = br.ReadString();
			cスコア.SongInformation.Premovie = br.ReadString();
			cスコア.SongInformation.Presound = br.ReadString();
			cスコア.SongInformation.Backgound = br.ReadString();
			cスコア.SongInformation.Level.Drums = br.ReadInt32();
			cスコア.SongInformation.Level.Guitar = br.ReadInt32();
			cスコア.SongInformation.Level.Bass = br.ReadInt32();
            cスコア.SongInformation.LevelDec.Drums = br.ReadInt32();
            cスコア.SongInformation.LevelDec.Guitar = br.ReadInt32();
            cスコア.SongInformation.LevelDec.Bass = br.ReadInt32();
			cスコア.SongInformation.BestRank.Drums = br.ReadInt32();
			cスコア.SongInformation.BestRank.Guitar = br.ReadInt32();
			cスコア.SongInformation.BestRank.Bass = br.ReadInt32();
			cスコア.SongInformation.HighSkill.Drums = br.ReadDouble();
			cスコア.SongInformation.HighSkill.Guitar = br.ReadDouble();
			cスコア.SongInformation.HighSkill.Bass = br.ReadDouble();
			cスコア.SongInformation.FullCombo.Drums = br.ReadBoolean();
			cスコア.SongInformation.FullCombo.Guitar = br.ReadBoolean();
			cスコア.SongInformation.FullCombo.Bass = br.ReadBoolean();
			cスコア.SongInformation.NbPerformances.Drums = br.ReadInt32();
			cスコア.SongInformation.NbPerformances.Guitar = br.ReadInt32();
			cスコア.SongInformation.NbPerformances.Bass = br.ReadInt32();
			cスコア.SongInformation.PerformanceHistory.行1 = br.ReadString();
			cスコア.SongInformation.PerformanceHistory.行2 = br.ReadString();
			cスコア.SongInformation.PerformanceHistory.行3 = br.ReadString();
			cスコア.SongInformation.PerformanceHistory.行4 = br.ReadString();
			cスコア.SongInformation.PerformanceHistory.行5 = br.ReadString();
			cスコア.SongInformation.bHiddenLevel = br.ReadBoolean();
            cスコア.SongInformation.b完全にCLASSIC譜面である.Drums = br.ReadBoolean();
            cスコア.SongInformation.b完全にCLASSIC譜面である.Guitar = br.ReadBoolean();
            cスコア.SongInformation.b完全にCLASSIC譜面である.Bass = br.ReadBoolean();
            cスコア.SongInformation.bScoreExists.Drums = br.ReadBoolean();
            cスコア.SongInformation.bScoreExists.Guitar = br.ReadBoolean();
            cスコア.SongInformation.bScoreExists.Bass = br.ReadBoolean();
			cスコア.SongInformation.SongType = (CDTX.EType) br.ReadInt32();
			cスコア.SongInformation.Bpm = br.ReadDouble();
			cスコア.SongInformation.Duration = br.ReadInt32();
			//Read Progress after Duration
			cスコア.SongInformation.progress.Drums = br.ReadString();
			cスコア.SongInformation.progress.Guitar = br.ReadString();
			cスコア.SongInformation.progress.Bass = br.ReadString();
			
			//
			cスコア.SongInformation.chipCountByInstrument.Drums = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.LC] = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.HH] = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.SD] = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.LP] = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.HT] = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.BD] = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.LT] = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.FT] = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.CY] = br.ReadInt32();

			cスコア.SongInformation.chipCountByInstrument.Guitar = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.GtR] = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.GtG] = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.GtB] = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.GtY] = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.GtP] = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.GtPick] = br.ReadInt32();

			cスコア.SongInformation.chipCountByInstrument.Bass = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.BsR] = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.BsG] = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.BsB] = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.BsY] = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.BsP] = br.ReadInt32();
			cスコア.SongInformation.chipCountByLane[ELane.BsPick] = br.ReadInt32();

			//Debug.WriteLine( "songs.db: " + cスコア.FileInformation.AbsoluteFilePath );
			return cスコア;
		}
		//-----------------
		#endregion
		#region [ SongsDBになかった曲をファイルから読み込んで反映する ]
		//-----------------
		public void tSongsDBになかった曲をファイルから読み込んで反映する()
		{
			this.nNbScoresFromFile = 0;
			this.tSongsDBになかった曲をファイルから読み込んで反映する( this.listSongRoot );
		}
		private void tSongsDBになかった曲をファイルから読み込んで反映する( List<CSongListNode> ノードリスト )
		{
            List<Task> taskList = new List<Task>();
            foreach ( CSongListNode c曲リストノード in ノードリスト )
			{
				SlowOrSuspendSearchTask();		// #27060 中断要求があったら、解除要求が来るまで待機, #PREMOVIE再生中は検索負荷を落とす

				if( c曲リストノード.eNodeType == CSongListNode.ENodeType.BOX )
				{
					this.tSongsDBになかった曲をファイルから読み込んで反映する( c曲リストノード.list子リスト );
				}
				else if( ( c曲リストノード.eNodeType == CSongListNode.ENodeType.SCORE )
					  || ( c曲リストノード.eNodeType == CSongListNode.ENodeType.SCORE_MIDI ) )
				{
					Task task = Task.Run(delegate {
                        for (int i = 0; i < 5; i++)
                        {
                            if ((c曲リストノード.arScore[i] != null) && !c曲リストノード.arScore[i].bHadACacheInSongDB)
                            {
                                #region [ DTX ファイルのヘッダだけ読み込み、Cスコア.譜面情報 を設定する ]
                                //-----------------
                                string path = c曲リストノード.arScore[i].FileInformation.AbsoluteFilePath;
                                if (File.Exists(path))
                                {
                                    try
                                    {
                                        CDTX cdtx = new CDTX(c曲リストノード.arScore[i].FileInformation.AbsoluteFilePath, false);    //2013.06.04 kairera0467 ここの「ヘッダのみ読み込む」をfalseにすると、選曲画面のBPM表示が狂う場合があるので注意。
                                                                                                                              //CDTX cdtx2 = new CDTX( c曲リストノード.arScore[ i ].FileInformation.AbsoluteFilePath, false );
                                        c曲リストノード.arScore[i].SongInformation.Title = cdtx.TITLE;
                                        c曲リストノード.arScore[i].SongInformation.ArtistName = cdtx.ARTIST;
                                        c曲リストノード.arScore[i].SongInformation.Comment = cdtx.COMMENT;
                                        c曲リストノード.arScore[i].SongInformation.Genre = cdtx.GENRE;
                                        c曲リストノード.arScore[i].SongInformation.Preimage = cdtx.PREIMAGE;
                                        c曲リストノード.arScore[i].SongInformation.Premovie = cdtx.PREMOVIE;
                                        c曲リストノード.arScore[i].SongInformation.Presound = cdtx.PREVIEW;
                                        c曲リストノード.arScore[i].SongInformation.Backgound = ((cdtx.BACKGROUND != null) && (cdtx.BACKGROUND.Length > 0)) ? cdtx.BACKGROUND : cdtx.BACKGROUND_GR;
                                        c曲リストノード.arScore[i].SongInformation.Level.Drums = cdtx.LEVEL.Drums;
                                        c曲リストノード.arScore[i].SongInformation.Level.Guitar = cdtx.LEVEL.Guitar;
                                        c曲リストノード.arScore[i].SongInformation.Level.Bass = cdtx.LEVEL.Bass;
                                        c曲リストノード.arScore[i].SongInformation.LevelDec.Drums = cdtx.LEVELDEC.Drums;
                                        c曲リストノード.arScore[i].SongInformation.LevelDec.Guitar = cdtx.LEVELDEC.Guitar;
                                        c曲リストノード.arScore[i].SongInformation.LevelDec.Bass = cdtx.LEVELDEC.Bass;
                                        c曲リストノード.arScore[i].SongInformation.bHiddenLevel = cdtx.HIDDENLEVEL;
                                        c曲リストノード.arScore[i].SongInformation.b完全にCLASSIC譜面である.Drums = (cdtx.bチップがある.LeftCymbal == false && cdtx.bチップがある.LP == false && cdtx.bチップがある.LBD == false && cdtx.bチップがある.FT == false && cdtx.bチップがある.Ride == false) ? true : false;
                                        c曲リストノード.arScore[i].SongInformation.b完全にCLASSIC譜面である.Guitar = !cdtx.bチップがある.YPGuitar ? true : false;
                                        c曲リストノード.arScore[i].SongInformation.b完全にCLASSIC譜面である.Bass = !cdtx.bチップがある.YPBass ? true : false;
                                        c曲リストノード.arScore[i].SongInformation.bScoreExists.Drums = cdtx.bチップがある.Drums;
                                        c曲リストノード.arScore[i].SongInformation.bScoreExists.Guitar = cdtx.bチップがある.Guitar;
                                        c曲リストノード.arScore[i].SongInformation.bScoreExists.Bass = cdtx.bチップがある.Bass;
                                        c曲リストノード.arScore[i].SongInformation.SongType = cdtx.e種別;
                                        c曲リストノード.arScore[i].SongInformation.Bpm = cdtx.BPM;
                                        c曲リストノード.arScore[i].SongInformation.Duration = (cdtx.listChip == null) ? 0 : cdtx.listChip[cdtx.listChip.Count - 1].nPlaybackTimeMs;

                                        //
                                        c曲リストノード.arScore[i].SongInformation.chipCountByInstrument.Drums = cdtx.nVisibleChipsCount.Drums;
                                        {
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.LC] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.LC);
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.HH] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.HH);
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.SD] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.SD);
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.LP] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.LP);
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.HT] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.HT);
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.BD] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.BD);
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.LT] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.LT);
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.FT] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.FT);
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.CY] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.CY);
                                        }

                                        c曲リストノード.arScore[i].SongInformation.chipCountByInstrument.Guitar = cdtx.nVisibleChipsCount.Guitar;
                                        {
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.GtR] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.GtR);
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.GtG] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.GtG);
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.GtB] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.GtB);
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.GtY] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.GtY);
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.GtP] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.GtP);
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.GtPick] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.GtPick);
                                        }

                                        c曲リストノード.arScore[i].SongInformation.chipCountByInstrument.Bass = cdtx.nVisibleChipsCount.Bass;
                                        {
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.BsR] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.BsR);
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.BsG] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.BsG);
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.BsB] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.BsB);
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.BsY] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.BsY);
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.BsP] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.BsP);
                                            c曲リストノード.arScore[i].SongInformation.chipCountByLane[ELane.BsPick] = cdtx.nVisibleChipsCount.chipCountInLane(ELane.BsPick);
                                        }

                                        this.nNbScoresFromFile++;
                                        cdtx.OnDeactivate();
                                        //Debug.WriteLine( "★" + this.nNbScoresFromFile + " " + c曲リストノード.arScore[ i ].SongInformation.Title );
                                        #region [ 曲検索ログ出力 ]
                                        //-----------------
                                        if (CDTXMania.ConfigIni.bLogSongSearch)
                                        {
                                            StringBuilder sb = new StringBuilder(0x400);
                                            sb.Append(string.Format("曲データファイルから譜面情報を転記しました。({0})", path));
                                            sb.Append("(title=" + c曲リストノード.arScore[i].SongInformation.Title);
                                            sb.Append(", artist=" + c曲リストノード.arScore[i].SongInformation.ArtistName);
                                            sb.Append(", comment=" + c曲リストノード.arScore[i].SongInformation.Comment);
                                            sb.Append(", genre=" + c曲リストノード.arScore[i].SongInformation.Genre);
                                            sb.Append(", preimage=" + c曲リストノード.arScore[i].SongInformation.Preimage);
                                            sb.Append(", premovie=" + c曲リストノード.arScore[i].SongInformation.Premovie);
                                            sb.Append(", presound=" + c曲リストノード.arScore[i].SongInformation.Presound);
                                            sb.Append(", background=" + c曲リストノード.arScore[i].SongInformation.Backgound);
                                            sb.Append(", lvDr=" + c曲リストノード.arScore[i].SongInformation.Level.Drums);
                                            sb.Append(", lvGt=" + c曲リストノード.arScore[i].SongInformation.Level.Guitar);
                                            sb.Append(", lvBs=" + c曲リストノード.arScore[i].SongInformation.Level.Bass);
                                            sb.Append(", lvHide=" + c曲リストノード.arScore[i].SongInformation.bHiddenLevel);
                                            sb.Append(", classic=" + c曲リストノード.arScore[i].SongInformation.b完全にCLASSIC譜面である);
                                            sb.Append(", type=" + c曲リストノード.arScore[i].SongInformation.SongType);
                                            sb.Append(", bpm=" + c曲リストノード.arScore[i].SongInformation.Bpm);
                                            //	sb.Append( ", duration=" + c曲リストノード.arScore[ i ].SongInformation.Duration );
                                            Trace.TraceInformation(sb.ToString());
                                        }
                                        //-----------------
                                        #endregion
                                    }
                                    catch (Exception exception)
                                    {
                                        Trace.TraceError(exception.Message);
                                        c曲リストノード.arScore[i] = null;
                                        c曲リストノード.nスコア数--;
                                        this.nNbScoresFound--;
                                        Trace.TraceError("曲データファイルの読み込みに失敗しました。({0})", path);
                                    }
                                }
                                //-----------------
                                #endregion

                                #region [ 対応する .score.ini が存在していれば読み込み、Cスコア.譜面情報 に追加設定する ]
                                //-----------------
                                this.tReadScoreIniAndSetScoreInformation(c曲リストノード.arScore[i].FileInformation.AbsoluteFilePath + ".score.ini", ref c曲リストノード.arScore[i]);
                                //-----------------
                                #endregion
                            }
                        }
                    });

					taskList.Add(task);
                }
			}

            Task.WaitAll(taskList.ToArray());
        }
		//-----------------
		#endregion
		#region [ 曲リストへ後処理を適用する ]
		//-----------------
		public void t曲リストへ後処理を適用する()
		{
			listStrBoxDefSkinSubfolderFullName = new List<string>();
			if ( CDTXMania.Skin.strBoxDefSkinSubfolders != null )
			{
				foreach ( string b in CDTXMania.Skin.strBoxDefSkinSubfolders )
				{
					listStrBoxDefSkinSubfolderFullName.Add( b );
				}
			}

			this.t曲リストへ後処理を適用する( this.listSongRoot );

			#region [ skin名で比較して、systemスキンとboxdefスキンに重複があれば、boxdefスキン側を削除する ]
			string[] systemSkinNames = CSkin.GetSkinName( CDTXMania.Skin.strSystemSkinSubfolders );
			List<string> l = new List<string>( listStrBoxDefSkinSubfolderFullName );
			foreach ( string boxdefSkinSubfolderFullName in l )
			{
				if ( Array.BinarySearch( systemSkinNames,
					CSkin.GetSkinName( boxdefSkinSubfolderFullName ),
					StringComparer.InvariantCultureIgnoreCase ) >= 0 )
				{
					listStrBoxDefSkinSubfolderFullName.Remove( boxdefSkinSubfolderFullName );
				}
			}
			#endregion
			string[] ba = listStrBoxDefSkinSubfolderFullName.ToArray();
			Array.Sort( ba );
			CDTXMania.Skin.strBoxDefSkinSubfolders = ba;
		}
		private void t曲リストへ後処理を適用する( List<CSongListNode> ノードリスト )
		{
			#region [ リストに１つ以上の曲があるなら RANDOM BOX を入れる ]
			//-----------------------------
			if( ノードリスト.Count > 0 )
			{
				CSongListNode itemRandom = new CSongListNode();
				itemRandom.eNodeType = CSongListNode.ENodeType.RANDOM;
				itemRandom.strタイトル = "< RANDOM SELECT >";
				itemRandom.nスコア数 = 5;
				itemRandom.r親ノード = ノードリスト[ 0 ].r親ノード;

				itemRandom.strBreadcrumbs = ( itemRandom.r親ノード == null ) ?
					itemRandom.strタイトル :  itemRandom.r親ノード.strBreadcrumbs + " > " + itemRandom.strタイトル;

				for( int i = 0; i < 5; i++ )
				{
					itemRandom.arScore[ i ] = new CScore();
					itemRandom.arScore[ i ].SongInformation.Title = string.Format( "< RANDOM SELECT Lv.{0} >", i + 1 );
                    itemRandom.arScore[ i ].SongInformation.Preimage = CSkin.Path(@"Graphics\5_preimage random.png");
                    itemRandom.arScore[ i ].SongInformation.Comment =
	                    CDTXMania.isJapanese ? 
		                    string.Format("難易度レベル {0} 付近の曲をランダムに選択します。難易度レベルを持たない曲も選択候補となります。", i + 1) : 
		                    string.Format("Random select from the songs which has the level about L{0}. Non-leveled songs may also selected.", i + 1);
					itemRandom.arDifficultyLabel[ i ] = string.Format( "L{0}", i + 1 );
				}
				ノードリスト.Add( itemRandom );

				#region [ ログ出力 ]
				//-----------------------------
				if( CDTXMania.ConfigIni.bLogSongSearch )
				{
					StringBuilder sb = new StringBuilder( 0x100 );
					sb.Append( string.Format( "nID#{0:D3}", itemRandom.nID ) );
					if( itemRandom.r親ノード != null )
					{
						sb.Append( string.Format( "(in#{0:D3}):", itemRandom.r親ノード.nID ) );
					}
					else
					{
						sb.Append( "(onRoot):" );
					}
					sb.Append( " RANDOM" );
					Trace.TraceInformation( sb.ToString() );
				}
				//-----------------------------
				#endregion
			}
			//-----------------------------
			#endregion

			// すべてのノードについて…
			foreach( CSongListNode c曲リストノード in ノードリスト )
			{
				SlowOrSuspendSearchTask();		// #27060 中断要求があったら、解除要求が来るまで待機, #PREMOVIE再生中は検索負荷を落とす

				#region [ BOXノードなら子リストに <<BACK を入れ、子リストに後処理を適用する ]
				//-----------------------------
				if( c曲リストノード.eNodeType == CSongListNode.ENodeType.BOX )
				{
					if (c曲リストノード.strSkinPath != "" && !listStrBoxDefSkinSubfolderFullName.Contains(c曲リストノード.strSkinPath))
					{
						listStrBoxDefSkinSubfolderFullName.Add(c曲リストノード.strSkinPath);
					}

					CSongListNode itemBack = new CSongListNode();
					itemBack.eNodeType = CSongListNode.ENodeType.BACKBOX;
					itemBack.strタイトル = "<< BACK";
					itemBack.nスコア数 = 1;
					itemBack.r親ノード = c曲リストノード;

					itemBack.strSkinPath = ( c曲リストノード.r親ノード == null ) ?
						"" : c曲リストノード.r親ノード.strSkinPath;

					itemBack.strBreadcrumbs = ( itemBack.r親ノード == null ) ?
						itemBack.strタイトル : itemBack.r親ノード.strBreadcrumbs + " > " + itemBack.strタイトル;

					itemBack.arScore[ 0 ] = new CScore();
					itemBack.arScore[ 0 ].FileInformation.AbsoluteFolderPath = "";
					itemBack.arScore[ 0 ].SongInformation.Title = itemBack.strタイトル;
                    itemBack.arScore[ 0 ].SongInformation.Preimage = CSkin.Path(@"Graphics\5_preimage backbox.png");
					itemBack.arScore[ 0 ].SongInformation.Comment =
						CDTXMania.isJapanese ?
						"BOX を出ます。" :
						"Exit from the BOX.";
					c曲リストノード.list子リスト.Insert( 0, itemBack );

					#region [ ログ出力 ]
					//-----------------------------
					if( CDTXMania.ConfigIni.bLogSongSearch )
					{
						StringBuilder sb = new StringBuilder( 0x100 );
						sb.Append( string.Format( "nID#{0:D3}", itemBack.nID ) );
						if( itemBack.r親ノード != null )
						{
							sb.Append( string.Format( "(in#{0:D3}):", itemBack.r親ノード.nID ) );
						}
						else
						{
							sb.Append( "(onRoot):" );
						}
						sb.Append( " BACKBOX" );
						Trace.TraceInformation( sb.ToString() );
					}
					//-----------------------------
					#endregion

					this.t曲リストへ後処理を適用する( c曲リストノード.list子リスト );
					continue;
				}
				//-----------------------------
				#endregion

				#region [ ノードにタイトルがないなら、最初に見つけたスコアのタイトルを設定する ]
				//-----------------------------
				if( string.IsNullOrEmpty( c曲リストノード.strタイトル ) )
				{
					for( int j = 0; j < 5; j++ )
					{
						if( ( c曲リストノード.arScore[ j ] != null ) && !string.IsNullOrEmpty( c曲リストノード.arScore[ j ].SongInformation.Title ) )
						{
							c曲リストノード.strタイトル = c曲リストノード.arScore[ j ].SongInformation.Title;

							if( CDTXMania.ConfigIni.bLogSongSearch )
								Trace.TraceInformation( "タイトルを設定しました。(nID#{0:D3}, title={1})", c曲リストノード.nID, c曲リストノード.strタイトル );

							break;
						}
					}
				}
				//-----------------------------
				#endregion
			}

			#region [ ノードをソートする ]
			//-----------------------------
			this.t曲リストのソート1_絶対パス順( ノードリスト );
			//-----------------------------
			#endregion
		}
		//-----------------
		#endregion
		#region [ スコアキャッシュをSongsDBに出力する ]
		//-----------------
		public void tスコアキャッシュをSongsDBに出力する( string SongsDBファイル名 )
		{
			this.nNbScoresForSongsDB = 0;
			try
			{
				BinaryWriter bw = new BinaryWriter( new FileStream( SongsDBファイル名, FileMode.Create, FileAccess.Write ) );
				bw.Write( SONGSDB_VERSION );
				this.tSongsDBにリストを１つ出力する( bw, this.listSongRoot );
				bw.Close();
			}
			catch
			{
				Trace.TraceError( "songs.dbの出力に失敗しました。" );
			}
		}
		private void tSongsDBにノードを１つ出力する( BinaryWriter bw, CSongListNode node )
		{
			for( int i = 0; i < 5; i++ )
			{
				// ここではsuspendに応じないようにしておく(深い意味はない。ファイルの書き込みオープン状態を長時間維持したくないだけ)
				//if ( this.bIsSuspending )		// #27060 中断要求があったら、解除要求が来るまで待機
				//{
				//	autoReset.WaitOne();
				//}

				if( node.arScore[ i ] != null )
				{
					bw.Write( node.arScore[ i ].FileInformation.AbsoluteFilePath );
					bw.Write( node.arScore[ i ].FileInformation.AbsoluteFolderPath );
					bw.Write( node.arScore[ i ].FileInformation.LastModified.Ticks );
					bw.Write( node.arScore[ i ].FileInformation.FileSize );
					bw.Write( node.arScore[ i ].ScoreIniInformation.LastModified.Ticks );
					bw.Write( node.arScore[ i ].ScoreIniInformation.FileSize );
					bw.Write( node.arScore[ i ].SongInformation.Title );
					bw.Write( node.arScore[ i ].SongInformation.ArtistName );
					bw.Write( node.arScore[ i ].SongInformation.Comment );
					bw.Write( node.arScore[ i ].SongInformation.Genre );
					bw.Write( node.arScore[ i ].SongInformation.Preimage );
					bw.Write( node.arScore[ i ].SongInformation.Premovie );
					bw.Write( node.arScore[ i ].SongInformation.Presound );
					bw.Write( node.arScore[ i ].SongInformation.Backgound );
					bw.Write( node.arScore[ i ].SongInformation.Level.Drums );
					bw.Write( node.arScore[ i ].SongInformation.Level.Guitar );
					bw.Write( node.arScore[ i ].SongInformation.Level.Bass );
                    bw.Write( node.arScore[ i ].SongInformation.LevelDec.Drums );
                    bw.Write( node.arScore[ i ].SongInformation.LevelDec.Guitar );
                    bw.Write( node.arScore[ i ].SongInformation.LevelDec.Bass );
					bw.Write( node.arScore[ i ].SongInformation.BestRank.Drums );
					bw.Write( node.arScore[ i ].SongInformation.BestRank.Guitar );
					bw.Write( node.arScore[ i ].SongInformation.BestRank.Bass );
					bw.Write( node.arScore[ i ].SongInformation.HighSkill.Drums );
					bw.Write( node.arScore[ i ].SongInformation.HighSkill.Guitar );
					bw.Write( node.arScore[ i ].SongInformation.HighSkill.Bass );
					bw.Write( node.arScore[ i ].SongInformation.FullCombo.Drums );
					bw.Write( node.arScore[ i ].SongInformation.FullCombo.Guitar );
					bw.Write( node.arScore[ i ].SongInformation.FullCombo.Bass );
					bw.Write( node.arScore[ i ].SongInformation.NbPerformances.Drums );
					bw.Write( node.arScore[ i ].SongInformation.NbPerformances.Guitar );
					bw.Write( node.arScore[ i ].SongInformation.NbPerformances.Bass );
					bw.Write( node.arScore[ i ].SongInformation.PerformanceHistory.行1 );
					bw.Write( node.arScore[ i ].SongInformation.PerformanceHistory.行2 );
					bw.Write( node.arScore[ i ].SongInformation.PerformanceHistory.行3 );
					bw.Write( node.arScore[ i ].SongInformation.PerformanceHistory.行4 );
					bw.Write( node.arScore[ i ].SongInformation.PerformanceHistory.行5 );
					bw.Write( node.arScore[ i ].SongInformation.bHiddenLevel );
                    bw.Write( node.arScore[ i ].SongInformation.b完全にCLASSIC譜面である.Drums );
                    bw.Write( node.arScore[ i ].SongInformation.b完全にCLASSIC譜面である.Guitar );
                    bw.Write( node.arScore[ i ].SongInformation.b完全にCLASSIC譜面である.Bass );
                    bw.Write( node.arScore[ i ].SongInformation.bScoreExists.Drums );
                    bw.Write( node.arScore[ i ].SongInformation.bScoreExists.Guitar );
                    bw.Write( node.arScore[ i ].SongInformation.bScoreExists.Bass );
					bw.Write( (int) node.arScore[ i ].SongInformation.SongType );
					bw.Write( node.arScore[ i ].SongInformation.Bpm );
					bw.Write( node.arScore[ i ].SongInformation.Duration );
					//Write to Progress here
					bw.Write(node.arScore[i].SongInformation.progress.Drums);
					bw.Write(node.arScore[i].SongInformation.progress.Guitar);
					bw.Write(node.arScore[i].SongInformation.progress.Bass);

					//New Data: Chip counts for Instrument and Lanes
					bw.Write(node.arScore[i].SongInformation.chipCountByInstrument.Drums);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.LC]);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.HH]);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.SD]);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.LP]);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.HT]);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.BD]);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.LT]);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.FT]);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.CY]);

					bw.Write(node.arScore[i].SongInformation.chipCountByInstrument.Guitar);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.GtR]);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.GtG]);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.GtB]);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.GtY]);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.GtP]);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.GtPick]);

					bw.Write(node.arScore[i].SongInformation.chipCountByInstrument.Bass);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.BsR]);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.BsG]);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.BsB]);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.BsY]);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.BsP]);
					bw.Write(node.arScore[i].SongInformation.chipCountByLane[ELane.BsPick]);

					this.nNbScoresForSongsDB++;
				}
			}
		}
		private void tSongsDBにリストを１つ出力する( BinaryWriter bw, List<CSongListNode> list )
		{
			foreach( CSongListNode c曲リストノード in list )
			{
				if(    ( c曲リストノード.eNodeType == CSongListNode.ENodeType.SCORE )
					|| ( c曲リストノード.eNodeType == CSongListNode.ENodeType.SCORE_MIDI ) )
				{
					this.tSongsDBにノードを１つ出力する( bw, c曲リストノード );
				}
				if( c曲リストノード.list子リスト != null )
				{
					this.tSongsDBにリストを１つ出力する( bw, c曲リストノード.list子リスト );
				}
			}
		}
		//-----------------
		#endregion
		
		#region [ 曲リストソート ]
		//-----------------
		public void t曲リストのソート1_絶対パス順( List<CSongListNode> ノードリスト )
		{
			ノードリスト.Sort( delegate( CSongListNode n1, CSongListNode n2 )
			{
				#region [ 共通処理 ]
				if ( n1 == n2 )
				{
					return 0;
				}
				int num = this.t比較0_共通( n1, n2 );
				if( num != 0 )
				{
					return num;
				}
				if( ( n1.eNodeType == CSongListNode.ENodeType.BOX ) && ( n2.eNodeType == CSongListNode.ENodeType.BOX ) )
				{
					return n1.arScore[ 0 ].FileInformation.AbsoluteFolderPath.CompareTo( n2.arScore[ 0 ].FileInformation.AbsoluteFolderPath );
				}
				#endregion
				string str = "";
				if( string.IsNullOrEmpty( n1.pathSetDefの絶対パス ) )
				{
					for( int i = 0; i < 5; i++ )
					{
						if( n1.arScore[ i ] != null )
						{
							str = n1.arScore[ i ].FileInformation.AbsoluteFilePath;
							if( str == null )
							{
								str = "";
							}
							break;
						}
					}
				}
				else
				{
					str = n1.pathSetDefの絶対パス + n1.SetDefのブロック番号.ToString( "00" );
				}
				string strB = "";
				if( string.IsNullOrEmpty( n2.pathSetDefの絶対パス ) )
				{
					for( int j = 0; j < 5; j++ )
					{
						if( n2.arScore[ j ] != null )
						{
							strB = n2.arScore[ j ].FileInformation.AbsoluteFilePath;
							if( strB == null )
							{
								strB = "";
							}
							break;
						}
					}
				}
				else
				{
					strB = n2.pathSetDefの絶対パス + n2.SetDefのブロック番号.ToString( "00" );
				}
				return str.CompareTo( strB );
			} );
			foreach( CSongListNode c曲リストノード in ノードリスト )
			{
				if( ( c曲リストノード.list子リスト != null ) && ( c曲リストノード.list子リスト.Count > 1 ) )
				{
					this.t曲リストのソート1_絶対パス順( c曲リストノード.list子リスト );
				}
			}
		}
		public void t曲リストのソート2_タイトル順( List<CSongListNode> ノードリスト, EInstrumentPart part, int order, params object[] p )
		{
			ノードリスト.Sort( delegate( CSongListNode n1, CSongListNode n2 )
			{
				if( n1 == n2 )
				{
					return 0;
				}
				int num = this.t比較0_共通( n1, n2 );
				if( num != 0 )
				{
					return order * num;
				}
				return order * n1.strタイトル.CompareTo( n2.strタイトル );
			} );
//			foreach( CSongListNode c曲リストノード in ノードリスト )
//			{
//				if( ( c曲リストノード.list子リスト != null ) && ( c曲リストノード.list子リスト.Count > 1 ) )
//				{
//					this.t曲リストのソート2_タイトル順( c曲リストノード.list子リスト, part, order );
//				}
//			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ノードリスト"></param>
		/// <param name="part"></param>
		/// <param name="order">1=Ascend -1=Descend</param>
		public void t曲リストのソート3_演奏回数の多い順( List<CSongListNode> ノードリスト, EInstrumentPart part, int order, params object[] p )
		{
			order = -order;
			int nL12345 = (int) p[ 0 ];
			if ( part != EInstrumentPart.UNKNOWN )
			{
				ノードリスト.Sort( delegate( CSongListNode n1, CSongListNode n2 )
				{
					#region [ 共通処理 ]
					if ( n1 == n2 )
					{
						return 0;
					}
					int num = this.t比較0_共通( n1, n2 );
					if( num != 0 )
					{
						return order * num;
					}
					if( ( n1.eNodeType == CSongListNode.ENodeType.BOX ) && ( n2.eNodeType == CSongListNode.ENodeType.BOX ) )
					{
						return order * n1.arScore[ 0 ].FileInformation.AbsoluteFolderPath.CompareTo( n2.arScore[ 0 ].FileInformation.AbsoluteFolderPath );
					}
					#endregion
					int nSumPlayCountN1 = 0, nSumPlayCountN2 = 0;
//					for( int i = 0; i < 5; i++ )
//					{
						if( n1.arScore[ nL12345 ] != null )
						{
							nSumPlayCountN1 += n1.arScore[ nL12345 ].SongInformation.NbPerformances[ (int) part ];
						}
						if( n2.arScore[ nL12345 ] != null )
						{
							nSumPlayCountN2 += n2.arScore[ nL12345 ].SongInformation.NbPerformances[ (int) part ];
						}
//					}
					num = nSumPlayCountN2 - nSumPlayCountN1;
					if( num != 0 )
					{
						return order * num;
					}
					return order * n1.strタイトル.CompareTo( n2.strタイトル );
				} );
				foreach ( CSongListNode c曲リストノード in ノードリスト )
				{
					int nSumPlayCountN1 = 0;
//					for ( int i = 0; i < 5; i++ )
//					{
						if ( c曲リストノード.arScore[ nL12345 ] != null )
						{
							nSumPlayCountN1 += c曲リストノード.arScore[ nL12345 ].SongInformation.NbPerformances[ (int) part ];
						}
//					}
// Debug.WriteLine( nSumPlayCountN1 + ":" + c曲リストノード.strタイトル );
				}

//				foreach( CSongListNode c曲リストノード in ノードリスト )
//				{
//					if( ( c曲リストノード.list子リスト != null ) && ( c曲リストノード.list子リスト.Count > 1 ) )
//					{
//						this.t曲リストのソート3_演奏回数の多い順( c曲リストノード.list子リスト, part );
//					}
//				}
			}
		}
		public void t曲リストのソート4_LEVEL順( List<CSongListNode> ノードリスト, EInstrumentPart part, int order, params object[] p )
		{
			order = -order;
			int nL12345 = (int)p[ 0 ];
			if ( part != EInstrumentPart.UNKNOWN )
			{
                Trace.WriteLine( "----------ソート開始------------" );
				ノードリスト.Sort( delegate( CSongListNode n1, CSongListNode n2 ) //2016.03.12 kairera0467 少数第2位も考慮するようにするテスト。
				{
					#region [ 共通処理 ]
					if ( n1 == n2 )
					{
						return 0;
					}
					float num = this.t比較0_共通( n1, n2 ); //2016.06.17 kairera0467 ソートが正確に行われるよう修正。(int→float)
					if ( num != 0 )
					{
						return (int)(order * num);
					}
					if ( ( n1.eNodeType == CSongListNode.ENodeType.BOX ) && ( n2.eNodeType == CSongListNode.ENodeType.BOX ) )
					{
						return order * n1.arScore[ 0 ].FileInformation.AbsoluteFolderPath.CompareTo( n2.arScore[ 0 ].FileInformation.AbsoluteFolderPath );
					}
					#endregion
					float nSumPlayCountN1 = 0, nSumPlayCountN2 = 0;
					if ( n1.arScore[ nL12345 ] != null )
					{
						nSumPlayCountN1 = ( n1.arScore[ nL12345 ].SongInformation.Level[ (int) part ] / 10.0f ) + ( n1.arScore[ nL12345 ].SongInformation.LevelDec[ (int) part ] / 100.0f );
					}
					if ( n2.arScore[ nL12345 ] != null )
					{
						nSumPlayCountN2 = ( n2.arScore[ nL12345 ].SongInformation.Level[ (int) part ] / 10.0f ) + ( n2.arScore[ nL12345 ].SongInformation.LevelDec[ (int) part ] / 100.0f );
					}
					num = nSumPlayCountN2 - nSumPlayCountN1;
					if ( num != 0 )
					{
						return (int)( (order * num) * 100 );
					}
					return order * n1.strタイトル.CompareTo( n2.strタイトル );
				} );
				foreach ( CSongListNode c曲リストノード in ノードリスト )
				{
					int nSumPlayCountN1 = 0;
					if ( c曲リストノード.arScore[ nL12345 ] != null )
					{
						nSumPlayCountN1 = c曲リストノード.arScore[ nL12345 ].SongInformation.Level[ (int) part ] + c曲リストノード.arScore[ nL12345 ].SongInformation.LevelDec[ (int) part ];
					}
// Debug.WriteLine( nSumPlayCountN1 + ":" + c曲リストノード.strタイトル );
				}
			}
		}
		public void t曲リストのソート5_BestRank順( List<CSongListNode> ノードリスト, EInstrumentPart part, int order, params object[] p )
		{
			order = -order;
			int nL12345 = (int) p[ 0 ];
			if ( part != EInstrumentPart.UNKNOWN )
			{
				ノードリスト.Sort( delegate( CSongListNode n1, CSongListNode n2 )
				{
					#region [ 共通処理 ]
					if ( n1 == n2 )
					{
						return 0;
					}
					int num = this.t比較0_共通( n1, n2 );
					if ( num != 0 )
					{
						return order * num;
					}
					if ( ( n1.eNodeType == CSongListNode.ENodeType.BOX ) && ( n2.eNodeType == CSongListNode.ENodeType.BOX ) )
					{
						return order * n1.arScore[ 0 ].FileInformation.AbsoluteFolderPath.CompareTo( n2.arScore[ 0 ].FileInformation.AbsoluteFolderPath );
					}
					#endregion
					int nSumPlayCountN1 = 0, nSumPlayCountN2 = 0;
					bool isFullCombo1 = false, isFullCombo2 = false;
					if ( n1.arScore[ nL12345 ] != null )
					{
						isFullCombo1 = n1.arScore[ nL12345 ].SongInformation.FullCombo[ (int) part ];
						nSumPlayCountN1 = n1.arScore[ nL12345 ].SongInformation.BestRank[ (int) part ];
					}
					if ( n2.arScore[ nL12345 ] != null )
					{
						isFullCombo2 = n2.arScore[ nL12345 ].SongInformation.FullCombo[ (int) part ];
						nSumPlayCountN2 = n2.arScore[ nL12345 ].SongInformation.BestRank[ (int) part ];
					}
					if ( isFullCombo1 ^ isFullCombo2 )
					{
						if ( isFullCombo1 ) return order; else return -order;
					}
					num = nSumPlayCountN2 - nSumPlayCountN1;
					if ( num != 0 )
					{
						return order * num;
					}
					return order * n1.strタイトル.CompareTo( n2.strタイトル );
				} );
				foreach ( CSongListNode c曲リストノード in ノードリスト )
				{
					int nSumPlayCountN1 = 0;
					if ( c曲リストノード.arScore[ nL12345 ] != null )
					{
						nSumPlayCountN1 = c曲リストノード.arScore[ nL12345 ].SongInformation.BestRank[ (int) part ];
					}
// Debug.WriteLine( nSumPlayCountN1 + ":" + c曲リストノード.strタイトル );
				}
			}
		}
		public void t曲リストのソート6_SkillPoint順( List<CSongListNode> ノードリスト, EInstrumentPart part, int order, params object[] p )
		{
			order = -order;
			int nL12345 = (int) p[ 0 ];
			if ( part != EInstrumentPart.UNKNOWN )
			{
				ノードリスト.Sort( delegate( CSongListNode n1, CSongListNode n2 )
				{
					#region [ 共通処理 ]
					if ( n1 == n2 )
					{
						return 0;
					}
					int num = this.t比較0_共通( n1, n2 );
					if ( num != 0 )
					{
						return order * num;
					}
					if ( ( n1.eNodeType == CSongListNode.ENodeType.BOX ) && ( n2.eNodeType == CSongListNode.ENodeType.BOX ) )
					{
						return order * n1.arScore[ 0 ].FileInformation.AbsoluteFolderPath.CompareTo( n2.arScore[ 0 ].FileInformation.AbsoluteFolderPath );
					}
					#endregion
					double nSumPlayCountN1 = 0, nSumPlayCountN2 = 0;
					if ( n1.arScore[ nL12345 ] != null )
					{
						nSumPlayCountN1 = n1.arScore[ nL12345 ].SongInformation.HighSkill[ (int) part ];
					}
					if ( n2.arScore[ nL12345 ] != null )
					{
						nSumPlayCountN2 = n2.arScore[ nL12345 ].SongInformation.HighSkill[ (int) part ];
					}
					double d = nSumPlayCountN2 - nSumPlayCountN1;
					if ( d != 0 )
					{
						return order * System.Math.Sign(d);
					}
					return order * n1.strタイトル.CompareTo( n2.strタイトル );
				} );
				foreach ( CSongListNode c曲リストノード in ノードリスト )
				{
					double nSumPlayCountN1 = 0;
					if ( c曲リストノード.arScore[ nL12345 ] != null )
					{
						nSumPlayCountN1 = c曲リストノード.arScore[ nL12345 ].SongInformation.HighSkill[ (int) part ];
					}
// Debug.WriteLine( nSumPlayCountN1 + ":" + c曲リストノード.strタイトル );
				}
			}
		}
		public void t曲リストのソート7_更新日時順( List<CSongListNode> ノードリスト, EInstrumentPart part, int order, params object[] p )
		{
			int nL12345 = (int) p[ 0 ];
			if ( part != EInstrumentPart.UNKNOWN )
			{
				ノードリスト.Sort( delegate( CSongListNode n1, CSongListNode n2 )
				{
					#region [ 共通処理 ]
					if ( n1 == n2 )
					{
						return 0;
					}
					int num = this.t比較0_共通( n1, n2 );
					if ( num != 0 )
					{
						return order * num;
					}
					if ( ( n1.eNodeType == CSongListNode.ENodeType.BOX ) && ( n2.eNodeType == CSongListNode.ENodeType.BOX ) )
					{
						return order * n1.arScore[ 0 ].FileInformation.AbsoluteFolderPath.CompareTo( n2.arScore[ 0 ].FileInformation.AbsoluteFolderPath );
					}
					#endregion
					DateTime nSumPlayCountN1 = DateTime.Parse("0001/01/01 12:00:01.000");
					DateTime nSumPlayCountN2 = DateTime.Parse("0001/01/01 12:00:01.000");
					if ( n1.arScore[ nL12345 ] != null )
					{
						nSumPlayCountN1 = n1.arScore[ nL12345 ].FileInformation.LastModified;
					}
					if ( n2.arScore[ nL12345 ] != null )
					{
						nSumPlayCountN2 = n2.arScore[ nL12345 ].FileInformation.LastModified;
					}
					int d = nSumPlayCountN1.CompareTo(nSumPlayCountN2);
					if ( d != 0 )
					{
						return order * System.Math.Sign( d );
					}
					return order * n1.strタイトル.CompareTo( n2.strタイトル );
				} );
				foreach ( CSongListNode c曲リストノード in ノードリスト )
				{
					DateTime nSumPlayCountN1 = DateTime.Parse( "0001/01/01 12:00:01.000" );
					if ( c曲リストノード.arScore[ nL12345 ] != null )
					{
						nSumPlayCountN1 = c曲リストノード.arScore[ nL12345 ].FileInformation.LastModified;
					}
// Debug.WriteLine( nSumPlayCountN1 + ":" + c曲リストノード.strタイトル );
				}
			}
		}
		public void t曲リストのソート8_アーティスト名順( List<CSongListNode> ノードリスト, EInstrumentPart part, int order, params object[] p )
		{
			int nL12345 = (int) p[ 0 ]; 
			ノードリスト.Sort( delegate( CSongListNode n1, CSongListNode n2 )
			{
				if ( n1 == n2 )
				{
					return 0;
				}
				int num = this.t比較0_共通( n1, n2 );
				if ( num != 0 )
				{
					return order * System.Math.Sign( num );
				}
				string strAuthorN1 = "";
				string strAuthorN2 = "";
				if (n1.arScore[ nL12345 ] != null )
                {
					strAuthorN1 = n1.arScore[ nL12345 ].SongInformation.ArtistName;
				}
				if ( n2.arScore[ nL12345 ] != null )
				{
					strAuthorN2 = n2.arScore[ nL12345 ].SongInformation.ArtistName;
				}

				return order * strAuthorN1.CompareTo( strAuthorN2 );
			} );
			foreach ( CSongListNode c曲リストノード in ノードリスト )
			{
				string s = "";
				if ( c曲リストノード.arScore[ nL12345 ] != null )
				{
					s = c曲リストノード.arScore[ nL12345 ].SongInformation.ArtistName;
				}
Debug.WriteLine( s + ":" + c曲リストノード.strタイトル );
			}
		}
#if TEST_SORTBGM
		public void t曲リストのソート9_BPM順( List<C曲リストノード> ノードリスト, E楽器パート part, int order, params object[] p )
		{
			order = -order;
			int nL12345 = (int) p[ 0 ];
			if ( part != E楽器パート.UNKNOWN )
			{
				ノードリスト.Sort( delegate( C曲リストノード n1, C曲リストノード n2 )
				{
					#region [ 共通処理 ]
					if ( n1 == n2 )
					{
						return 0;
					}
					int num = this.t比較0_共通( n1, n2 );
					if ( num != 0 )
					{
						return order * num;
					}
					if ( ( n1.eノード種別 == C曲リストノード.Eノード種別.BOX ) && ( n2.eノード種別 == C曲リストノード.Eノード種別.BOX ) )
					{
						return order * n1.arスコア[ 0 ].ファイル情報.フォルダの絶対パス.CompareTo( n2.arスコア[ 0 ].ファイル情報.フォルダの絶対パス );
					}
					#endregion
					double dBPMn1 = 0.0, dBPMn2 = 0.0;
					if ( n1.arスコア[ nL12345 ] != null )
					{
						dBPMn1 = n1.arスコア[ nL12345 ].譜面情報.bpm;
					}
					if ( n2.arスコア[ nL12345 ] != null )
					{
						dBPMn2 = n2.arスコア[ nL12345 ].譜面情報.bpm;
					}
					double d = dBPMn1- dBPMn2;
					if ( d != 0 )
					{
						return order * System.Math.Sign( d );
					}
					return order * n1.strタイトル.CompareTo( n2.strタイトル );
				} );
				foreach ( C曲リストノード c曲リストノード in ノードリスト )
				{
					double dBPM = 0;
					if ( c曲リストノード.arスコア[ nL12345 ] != null )
					{
						dBPM = c曲リストノード.arスコア[ nL12345 ].譜面情報.bpm;
					}
Debug.WriteLine( dBPM + ":" + c曲リストノード.strタイトル );
				}
			}
		}
#endif
		//-----------------
		#endregion
		#region [ .score.ini を読み込んで Cスコア.譜面情報に設定する ]
		//-----------------
		public void tReadScoreIniAndSetScoreInformation( string strScoreIniファイルパス, ref CScore score )
		{
			if( !File.Exists( strScoreIniファイルパス ) )
				return;

			try
			{
				var ini = new CScoreIni( strScoreIniファイルパス );
				ini.tCheckIntegrity();

				for( int n楽器番号 = 0; n楽器番号 < 3; n楽器番号++ )
				{
					int n = ( n楽器番号 * 2 ) + 1;	// n = 0～5

					#region socre.譜面情報.最大ランク[ n楽器番号 ] = ... 
					//-----------------
					if( ini.stSection[ n ].bMIDIUsed ||
						ini.stSection[ n ].bKeyboardUsed ||
						ini.stSection[ n ].bJoypadUsed ||
						ini.stSection[ n ].bMouseUsed )
                    {
                        // (A) 全オートじゃないようなので、演奏結果情報を有効としてランクを算出する。
                        if ( CDTXMania.ConfigIni.nSkillMode == 0 )
                        {
                            score.SongInformation.BestRank[ n楽器番号 ] =
                            CScoreIni.tCalculateRankOld(
                                ini.stSection[n].nTotalChipsCount,
                                ini.stSection[n].nPerfectCount,
                                ini.stSection[n].nGreatCount,
                                ini.stSection[n].nGoodCount,
                                ini.stSection[n].nPoorCount,
                                ini.stSection[n].nMissCount
                                );
                        }
                        else if( CDTXMania.ConfigIni.nSkillMode == 1 )
                        {
                            score.SongInformation.BestRank[ n楽器番号 ] =
                            CScoreIni.tCalculateRank(
                                ini.stSection[ n ].nTotalChipsCount,
                                ini.stSection[ n ].nPerfectCount,
                                ini.stSection[ n ].nGreatCount,
                                ini.stSection[ n ].nGoodCount,
                                ini.stSection[ n ].nPoorCount,
                                ini.stSection[ n ].nMissCount,
                                ini.stSection[ n ].nMaxCombo
                                );
                        }
                    }
					else
					{
						// (B) 全オートらしいので、ランクは無効とする。
						score.SongInformation.BestRank[ n楽器番号 ] = (int) CScoreIni.ERANK.UNKNOWN;
					}
					//-----------------
					#endregion
					score.SongInformation.HighSkill[ n楽器番号 ] = ini.stSection[ n ].dbPerformanceSkill;
                    score.SongInformation.HighSongSkill[ n楽器番号 ] = ini.stSection[ n ].dbGameSkill;
					score.SongInformation.FullCombo[ n楽器番号 ] = ini.stSection[ n ].bIsFullCombo | ini.stSection[ n楽器番号 * 2 ].bIsFullCombo;
					//New for Progress
					score.SongInformation.progress[n楽器番号] = ini.stSection[n].strProgress;					
					if(score.SongInformation.progress[n楽器番号] == "")
                    {
						//TODO: Read from another file if progress string is empty
						//Set a hard-coded 64 char string for now
						score.SongInformation.progress[n楽器番号] = "0000000000000000000000000000000000000000000000000000000000000000";
					}
				}
				score.SongInformation.NbPerformances.Drums = ini.stFile.PlayCountDrums;
				score.SongInformation.NbPerformances.Guitar = ini.stFile.PlayCountGuitar;
				score.SongInformation.NbPerformances.Bass = ini.stFile.PlayCountBass;
				for( int i = 0; i < 5; i++ )
					score.SongInformation.PerformanceHistory[ i ] = ini.stFile.History[ i ];
			}
			catch
			{
				Trace.TraceError( "演奏記録ファイルの読み込みに失敗しました。[{0}]", strScoreIniファイルパス );
			}
		}
		//-----------------
		#endregion


		// Other

		#region [ private ]
		//-----------------
		private const string SONGSDB_VERSION = "SongsDB3(ver.K)rev2";
		private List<string> listStrBoxDefSkinSubfolderFullName;

		private int t比較0_共通( CSongListNode n1, CSongListNode n2 )
		{
			if( n1.eNodeType == CSongListNode.ENodeType.BACKBOX )
			{
				return -1;
			}
			if( n2.eNodeType == CSongListNode.ENodeType.BACKBOX )
			{
				return 1;
			}
			if( n1.eNodeType == CSongListNode.ENodeType.RANDOM )
			{
				return 1;
			}
			if( n2.eNodeType == CSongListNode.ENodeType.RANDOM )
			{
				return -1;
			}
			if( ( n1.eNodeType == CSongListNode.ENodeType.BOX ) && ( n2.eNodeType != CSongListNode.ENodeType.BOX ) )
			{
				return -1;
			}
			if( ( n1.eNodeType != CSongListNode.ENodeType.BOX ) && ( n2.eNodeType == CSongListNode.ENodeType.BOX ) )
			{
				return 1;
			}
			return 0;
		}

		/// <summary>
		/// 検索を中断_スローダウンする
		/// </summary>
		private void SlowOrSuspendSearchTask()
		{
			if ( this.bIsSuspending )		// #27060 中断要求があったら、解除要求が来るまで待機
			{
				autoReset.WaitOne();
			}
			if ( this.bIsSlowdown && ++this.searchCount > 10 )			// #27060 #PREMOVIE再生中は検索負荷を下げる
			{
				Thread.Sleep( 100 );
				this.searchCount = 0;
			}
		}

		//-----------------
		#endregion
	}
}
