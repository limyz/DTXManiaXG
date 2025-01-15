using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using SharpDX;
using FDK;
using SampleFramework;

namespace DTXMania
{
    internal class CActEnumSongs : CActivity
    {
        public bool bコマンドでの曲データ取得;


        /// <summary>
        /// Constructor
        /// </summary>
        public CActEnumSongs()
        {
            Init(false);
        }

        public CActEnumSongs(bool _bコマンドでの曲データ取得)
        {
            Init(_bコマンドでの曲データ取得);
        }
        private void Init(bool _bコマンドでの曲データ取得)
        {
            base.bNotActivated = true;
            bコマンドでの曲データ取得 = _bコマンドでの曲データ取得;
        }

        // CActivity 実装

        public override void OnActivate()
        {
            if (this.bActivated)
                return;
            base.OnActivate();

            try
            {
                this.ctNowEnumeratingSongs = new CCounter();	// 0, 1000, 17, CDTXMania.Timer );
                this.ctNowEnumeratingSongs.tStart(0, 100, 17, CDTXMania.Timer);
            }
            finally
            {
            }
        }
        public override void OnDeactivate()
        {
            if (this.bNotActivated)
                return;
            base.OnDeactivate();
            this.ctNowEnumeratingSongs = null;
        }
        public override void OnManagedCreateResources()
        {
            if (this.bNotActivated)
                return;
            string pathNowEnumeratingSongs = CSkin.Path(@"Graphics\ScreenTitle NowEnumeratingSongs.png");
            if (File.Exists(pathNowEnumeratingSongs))
            {
                this.txNowEnumeratingSongs = CDTXMania.tGenerateTexture(pathNowEnumeratingSongs, false);
            }
            else
            {
                this.txNowEnumeratingSongs = null;
            }
            string pathDialogNowEnumeratingSongs = CSkin.Path(@"Graphics\ScreenConfig NowEnumeratingSongs.png");
            if (File.Exists(pathDialogNowEnumeratingSongs))
            {
                this.txDialogNowEnumeratingSongs = CDTXMania.tGenerateTexture(pathDialogNowEnumeratingSongs, false);
            }
            else
            {
                this.txDialogNowEnumeratingSongs = null;
            }

            try
            {
                System.Drawing.Font ftMessage = new System.Drawing.Font("MS PGothic", 60f, FontStyle.Bold, GraphicsUnit.Pixel);
                string[] strMessage = 
				{
					"     曲データの一覧を\n       取得しています。\n   しばらくお待ちください。",
					" Now enumerating songs.\n         Please wait..."
				};
                int ci = (CDTXMania.isJapanese) ? 0 : 1;
                if ((strMessage != null) && (strMessage.Length > 0))
                {
                    Bitmap image = new Bitmap(1, 1);
                    Graphics graphics = Graphics.FromImage(image);
                    SizeF ef = graphics.MeasureString(strMessage[ci], ftMessage);
                    Size size = new Size((int)Math.Ceiling((double)ef.Width), (int)Math.Ceiling((double)ef.Height));
                    graphics.Dispose();
                    image.Dispose();
                    image = new Bitmap(size.Width, size.Height);
                    graphics = Graphics.FromImage(image);
                    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    graphics.DrawString(strMessage[ci], ftMessage, Brushes.White, (float)0f, (float)0f);
                    graphics.Dispose();
                    this.txMessage = new CTexture(CDTXMania.app.Device, image, CDTXMania.TextureFormat);
                    this.txMessage.vcScaleRatio = new Vector3(0.5f, 0.5f, 1f);
                    image.Dispose();
                    CDTXMania.tDisposeSafely(ref ftMessage);
                }
                else
                {
                    this.txMessage = null;
                }
            }
            catch (CTextureCreateFailedException)
            {
                Trace.TraceError("テクスチャの生成に失敗しました。(txMessage)");
                this.txMessage = null;
            }

            base.OnManagedCreateResources();
        }
        public override void OnManagedReleaseResources()
        {
            if (this.bNotActivated)
                return;

            CDTXMania.tDisposeSafely(ref this.txDialogNowEnumeratingSongs);
            CDTXMania.tDisposeSafely(ref this.txNowEnumeratingSongs);
            CDTXMania.tDisposeSafely(ref this.txMessage);
            base.OnManagedReleaseResources();
        }

        public override int OnUpdateAndDraw()
        {
            if (this.bNotActivated)
            {
                return 0;
            }
            this.ctNowEnumeratingSongs.tUpdateLoop();
            if (this.txNowEnumeratingSongs != null)
            {
                this.txNowEnumeratingSongs.nTransparency = (int)(176.0 + 80.0 * Math.Sin((double)(2 * Math.PI * this.ctNowEnumeratingSongs.nCurrentValue * 2 / 100.0)));
                this.txNowEnumeratingSongs.tDraw2D(CDTXMania.app.Device, 18, 7);
            }
            if (bコマンドでの曲データ取得 && this.txDialogNowEnumeratingSongs != null)
            {
                this.txDialogNowEnumeratingSongs.tDraw2D(CDTXMania.app.Device, 360, 177);
                this.txMessage.tDraw2D(CDTXMania.app.Device, 450, 240);
            }

            return 0;
        }


        private CCounter ctNowEnumeratingSongs;
        private CTexture txNowEnumeratingSongs = null;
        private CTexture txDialogNowEnumeratingSongs = null;
        private CTexture txMessage;
    }
}
