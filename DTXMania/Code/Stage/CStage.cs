using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Text;
using FDK;

namespace DTXMania
{
	public abstract class CStage : CActivity
	{
		// プロパティ

		/// <summary>
		/// The presence used to indicate the user's activity within this stage, or <see langword="null"/> if there is none.
		/// </summary>
		protected virtual RichPresence Presence => new CDTXRichPresence
		{
			State = "In Menu",
			Details = "Idle",
		};

		internal enum EHitJudgement
		{
			PERFECT,
			GREAT,
			GOOD,
			POOR,
			MISS,
			BAD,
			AUTO
		}

		internal EStage eStageID;
		public enum EStage
		{
			DoNothing,
			Startup,
			Title,
			Config,
			SongSelection,
			SongLoading,
			Playing,
			Result,
			ChangeSkin,						// #28195 2011.5.4 yyagi
			End
		}
		
		internal EPhase ePhaseID;
		public enum EPhase
		{
			Common_DefaultState,
			Common_FadeIn,
			Common_FadeOut,
			Common_EndStatus,
			起動0_システムサウンドを構築,
			起動00_songlistから曲リストを作成する,
			起動1_SongsDBからスコアキャッシュを構築,
			起動2_曲を検索してリストを作成する,
			起動3_スコアキャッシュをリストに反映する,
			起動4_スコアキャッシュになかった曲をファイルから読み込んで反映する,
			起動5_曲リストへ後処理を適用する,
			起動6_スコアキャッシュをSongsDBに出力する,
			起動7_完了,
			タイトル_起動画面からのフェードイン,
			選曲_結果画面からのフェードイン,
			選曲_NowLoading画面へのフェードアウト,
			NOWLOADING_DTX_FILE_READING,
			NOWLOADING_WAV_FILE_READING,
			NOWLOADING_BMP_FILE_READING,
			NOWLOADING_WAIT_BGM_SOUND_COMPLETION,
			演奏_STAGE_FAILED,
			演奏_STAGE_FAILED_フェードアウト,
            演奏_STAGE_CLEAR,
			演奏_STAGE_CLEAR_フェードアウト,
			演奏_STAGE_RESTART
		}
        public bool bIsStageFailed;

		public override void OnActivate()
		{
			base.OnActivate();
			tDisplayPresence();
		}

		/// <summary>
		/// Display the current <see cref="Presence"/> of this stage.
		/// </summary>
		protected void tDisplayPresence()
		{
			if (Presence is var presence && presence != null)
				CDTXMania.DiscordRichPresence?.tSetPresence(presence);
		}
	}
}
