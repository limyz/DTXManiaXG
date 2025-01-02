using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using FDK;

namespace DTXMania
{
	internal class CActSelectPresound : CActivity
	{
		// メソッド

		public CActSelectPresound()
		{
			base.bNotActivated = true;
		}
		public void tサウンド停止()
		{
			if( this.sound != null )
			{
				this.sound.tStopPlayback();
				CDTXMania.SoundManager.tDiscard( this.sound );
				this.sound = null;
			}
		}
		public void t選択曲が変更された()
		{
			CScore cスコア = CDTXMania.stageSongSelection.rSelectedScore;
			if( ( cスコア != null ) && ( ( !( cスコア.FileInformation.AbsoluteFolderPath + cスコア.SongInformation.Presound ).Equals( this.str現在のファイル名 ) || ( this.sound == null ) ) || !this.sound.b再生中 ) )
			{
				this.tサウンド停止();
				this.tBGMフェードイン開始();
				if( ( cスコア.SongInformation.Presound != null ) && ( cスコア.SongInformation.Presound.Length > 0 ) )
				{
					this.ct再生待ちウェイト = new CCounter( 0, CDTXMania.ConfigIni.n曲が選択されてからプレビュー音が鳴るまでのウェイトms, 1, CDTXMania.Timer );
				}
			}
		}


		// CActivity 実装

		public override void OnActivate()
		{
			this.sound = null;
			this.str現在のファイル名 = "";
			this.ct再生待ちウェイト = null;
			this.ctBGMフェードアウト用 = null;
			this.ctBGMフェードイン用 = null;
			base.OnActivate();
		}
		public override void OnDeactivate()
		{
			this.tサウンド停止();
			this.ct再生待ちウェイト = null;
			this.ctBGMフェードイン用 = null;
			this.ctBGMフェードアウト用 = null;
			base.OnDeactivate();
		}
		public override int OnUpdateAndDraw()
		{
			if( !base.bNotActivated )
			{
				if( ( this.ctBGMフェードイン用 != null ) && this.ctBGMフェードイン用.bInProgress )
				{
					this.ctBGMフェードイン用.tUpdate();
					CDTXMania.Skin.bgm選曲画面.n音量_現在のサウンド = this.ctBGMフェードイン用.nCurrentValue;
					if( this.ctBGMフェードイン用.bReachedEndValue )
					{
						this.ctBGMフェードイン用.tStop();
					}
				}
				if( ( this.ctBGMフェードアウト用 != null ) && this.ctBGMフェードアウト用.bInProgress )
				{
					this.ctBGMフェードアウト用.tUpdate();
					CDTXMania.Skin.bgm選曲画面.n音量_現在のサウンド = 100 - this.ctBGMフェードアウト用.nCurrentValue;
					if( this.ctBGMフェードアウト用.bReachedEndValue )
					{
						this.ctBGMフェードアウト用.tStop();
					}
				}
				this.t進行処理_プレビューサウンド();
			}
			return 0;
		}


		// Other

		#region [ private ]
		//-----------------
		private CCounter ctBGMフェードアウト用;
		private CCounter ctBGMフェードイン用;
		private CCounter ct再生待ちウェイト;
		private CSound sound;
		private string str現在のファイル名;
		
		private void tBGMフェードアウト開始()
		{
			if( this.ctBGMフェードイン用 != null )
			{
				this.ctBGMフェードイン用.tStop();
			}
			this.ctBGMフェードアウト用 = new CCounter( 0, 100, 10, CDTXMania.Timer );
			this.ctBGMフェードアウト用.nCurrentValue = 100 - CDTXMania.Skin.bgm選曲画面.n音量_現在のサウンド;
		}
		private void tBGMフェードイン開始()
		{
			if( this.ctBGMフェードアウト用 != null )
			{
				this.ctBGMフェードアウト用.tStop();
			}
			this.ctBGMフェードイン用 = new CCounter( 0, 100, 20, CDTXMania.Timer );
			this.ctBGMフェードイン用.nCurrentValue = CDTXMania.Skin.bgm選曲画面.n音量_現在のサウンド;
		}
		private void tプレビューサウンドの作成()
		{
			CScore cスコア = CDTXMania.stageSongSelection.rSelectedScore;
			if( ( cスコア != null ) && !string.IsNullOrEmpty( cスコア.SongInformation.Presound ) )
			{
				string strPreviewFilename = cスコア.FileInformation.AbsoluteFolderPath + cスコア.SongInformation.Presound;
				try
				{
					this.sound = CDTXMania.SoundManager.tGenerateSound( strPreviewFilename );
					this.sound.nVolume = 80;	// CDTXMania.ConfigIni.n自動再生音量;			// #25217 changed preview volume from AutoVolume
					this.sound.tStartPlaying( true );
					this.str現在のファイル名 = strPreviewFilename;
					this.tBGMフェードアウト開始();
					Trace.TraceInformation( "プレビューサウンドを生成しました。({0})", strPreviewFilename );
				}
				catch
				{
					Trace.TraceError( "プレビューサウンドの生成に失敗しました。({0})", strPreviewFilename );
					if( this.sound != null )
					{
						this.sound.Dispose();
					}
					this.sound = null;
				}
			}
		}
		private void t進行処理_プレビューサウンド()
		{
			if( ( this.ct再生待ちウェイト != null ) && !this.ct再生待ちウェイト.bStopped )
			{
				this.ct再生待ちウェイト.tUpdate();
				if( !this.ct再生待ちウェイト.b終了値に達してない )
				{
					this.ct再生待ちウェイト.tStop();
					if( !CDTXMania.stageSongSelection.bScrolling )
					{
						this.tプレビューサウンドの作成();
					}
				}
			}
		}
		//-----------------
		#endregion
	}
}
