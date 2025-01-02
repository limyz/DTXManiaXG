using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using FDK;

namespace DTXMania
{
    internal class CActPerfStageFailure : CActivity
    {
        // コンストラクタ

        public CActPerfStageFailure()
        {
            base.bNotActivated = true;
        }


        // メソッド

        public void Start()
        {
            this.ct進行 = new CCounter(0, 0x3e8, 2, CDTXMania.Timer);
        }


        // CActivity 実装

        public override void OnActivate()
        {
            this.sd効果音 = null;
            this.b効果音再生済み = false;
            this.ct進行 = new CCounter();
            base.OnActivate();
        }
        public override void OnDeactivate()
        {
            this.ct進行 = null;
            if (this.sd効果音 != null)
            {
                CDTXMania.SoundManager.tDiscard(this.sd効果音);
                this.sd効果音 = null;
            }
            base.OnDeactivate();
        }
        public override void OnManagedCreateResources()
        {
            if (!base.bNotActivated)
            {
                this.txStageFailed = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_stage_failed.jpg"));
                base.OnManagedCreateResources();
            }
        }
        public override void OnManagedReleaseResources()
        {
            if (!base.bNotActivated)
            {
                CDTXMania.tReleaseTexture(ref this.txStageFailed);
                base.OnManagedReleaseResources();
            }
        }
        public override int OnUpdateAndDraw()
        {
            if (base.bNotActivated)
            {
                return 0;
            }
            if ((this.ct進行 == null) || this.ct進行.bStopped)
            {
                return 0;
            }
            this.ct進行.tUpdate();
            if (this.ct進行.nCurrentValue < 100)
            {
                int x = (int)(640.0 * Math.Cos((Math.PI / 2 * this.ct進行.nCurrentValue) / 100.0));
                if ((x != 1280) && (this.txStageFailed != null))
                {
                    this.txStageFailed.tDraw2D(CDTXMania.app.Device, 0, 0, new Rectangle(x, 0, 640 - x, 720));
                    this.txStageFailed.tDraw2D(CDTXMania.app.Device, 640 + x, 0, new Rectangle(640, 0, 640 - x, 720));
                }
            }
            else
            {
                if (this.txStageFailed != null)
                {
                    this.txStageFailed.tDraw2D(CDTXMania.app.Device, 0, 0);
                }
                if (!this.b効果音再生済み)
                {
                    if (((CDTXMania.DTX.SOUND_STAGEFAILED != null) && (CDTXMania.DTX.SOUND_STAGEFAILED.Length > 0)) && File.Exists(CDTXMania.DTX.strFolderName + CDTXMania.DTX.SOUND_STAGEFAILED))
                    {
                        try
                        {
                            if (this.sd効果音 != null)
                            {
                                CDTXMania.SoundManager.tDiscard(this.sd効果音);
                                this.sd効果音 = null;
                            }
                            this.sd効果音 = CDTXMania.SoundManager.tGenerateSound(CDTXMania.DTX.strFolderName + CDTXMania.DTX.SOUND_STAGEFAILED);
                            this.sd効果音.tStartPlaying();
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        CDTXMania.Skin.soundSTAGEFAILED音.tPlay();
                    }
                    this.b効果音再生済み = true;
                }
            }
            if (!this.ct進行.bReachedEndValue)
            {
                return 0;
            }
            return 1;
        }


        // Other

        #region [ private ]
        //-----------------
        private bool b効果音再生済み;
        private CCounter ct進行;
        private CSound sd効果音;
        private CTexture txStageFailed;
        //-----------------
        #endregion
    }
}
