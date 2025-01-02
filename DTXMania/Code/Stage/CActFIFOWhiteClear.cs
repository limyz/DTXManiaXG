using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;
using SharpDX;
using SharpDX.Direct3D9;
using FDK;

namespace DTXMania
{
	internal class CActFIFOWhiteClear : CActivity
	{
		// メソッド

		public void tStartFadeOut()
		{
			this.mode = EFIFOMode.FadeOut;
			this.counter = new CCounter( 0, 400, 5, CDTXMania.Timer );
		}
		public void tフェードイン開始()
		{
			this.mode = EFIFOMode.FadeIn;
			this.counter = new CCounter( 0, 400, 5, CDTXMania.Timer );
		}
		public void tフェードイン完了()		// #25406 2011.6.9 yyagi
		{
			this.counter.nCurrentValue = this.counter.nEndValue;
		}

		// CActivity 実装
		public override void OnDeactivate()
		{
			if( !base.bNotActivated )
			{
                for (int i = 0; i < 16; i++)
                {
                    this.st青い星[i].ct進行 = null;
                }
				base.OnDeactivate();
			}
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
                
				this.tx白タイル64x64 = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\Tile white 64x64.png" ), false );
                this.txリザルト画像 = CDTXMania.tGenerateTexture( CSkin.Path(@"Graphics\8_background.jpg"), false );
                this.txFullCombo = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\\7_FullCombo.png"));
                this.txExcellent = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\\7_Excellent.png"));
                this.tx黒幕 = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\\7_Drums_black.png"));

                this.txボーナス花火 = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\ScreenPlayDrums chip star.png"));
                if (this.txボーナス花火 != null)
                {
                    this.txボーナス花火.bAdditiveBlending = true;
                }

                for (int i = 0; i < 16; i++)
                {
                    this.st青い星[i] = new ST青い星();
                    this.st青い星[i].b使用中 = false;
                    this.st青い星[i].ct進行 = new CCounter();
                }

				base.OnManagedCreateResources();
			}
		}
        
        
        public override void OnManagedReleaseResources()
        {
            if (this.bNotActivated)
                return;

            CDTXMania.tReleaseTexture( ref this.txボーナス花火 );
            CDTXMania.tReleaseTexture( ref this.tx白タイル64x64 );
            CDTXMania.tReleaseTexture( ref this.txリザルト画像 );
            CDTXMania.tReleaseTexture( ref this.txFullCombo );
            CDTXMania.tReleaseTexture( ref this.txExcellent );
            CDTXMania.tReleaseTexture( ref this.tx黒幕 );

            base.OnManagedReleaseResources();
        }

        /*
        public override unsafe int OnUpdateAndDraw(Device D3D9Device)
		{
            if (base.bNotActivated || (this.counter == null))
				return 0;

			// 進行。
            this.counter.tUpdate();
			#region [ 初めての進行処理。]
			//-----------------
			if( this.bJustStartedUpdate )
			{
                if (this.ds背景動画 != null)
                {
                    this.ds背景動画.bループ再生 = false;
                    this.ds背景動画.t再生開始();
                    Trace.TraceInformation("DShow動画を再生開始しました。");
                }
                else
                {
                    //Trace.TraceError("DShow動画がnullになっています。");
                }

				this.bJustStartedUpdate = false;
			}
			//-----------------
			#endregion

			

			#region [ 背景動画 ]
			//-----------------
			if( this.ds背景動画 != null &&
				this.tx描画用 != null )
			{

				this.ds背景動画.t現時点における最新のスナップイメージをTextureに転写する( this.tx描画用 );
                Trace.TraceInformation("テクスチャにスナップイメージを転写しました。");
				this.tx描画用.tDraw2D( CDTXMania.app.Device, 0, 0 );
			}
			//-----------------
			#endregion

            if( this.ds背景動画 != null && !this.ds背景動画.b再生中 )			// 再生完了したらステージ終了。
				return 0;

			return 1;
		}
        */
		public override unsafe int OnUpdateAndDraw()
        {
            if (base.bNotActivated || (this.counter == null))
            {
                return 0;
            }
            this.counter.tUpdate();
            if (this.counter.nCurrentValue != 400)
            {
                return 0;
            }
            return 1;
        }
        


		// Other

		#region [ private ]
		//-----------------
		public CCounter counter;
		private EFIFOMode mode;
		private CTexture tx白タイル64x64;
        private CTexture txFullCombo;
        private CTexture txExcellent;
        private CTexture tx黒幕;
        private CTexture txボーナス花火;
        private CTexture txリザルト画像;

        [StructLayout(LayoutKind.Sequential)]
        private struct ST青い星
        {
            public int nLane;
            public bool b使用中;
            public CCounter ct進行;
            public int n前回のValue;
            public float fX;
            public float fY;
            public float f加速度X;
            public float f加速度Y;
            public float f加速度の加速度X;
            public float f加速度の加速度Y;
            public float f重力加速度;
            public float f半径;
        }
        private ST青い星[] st青い星 = new ST青い星[240];
        //-----------------
        #endregion
	}
}
