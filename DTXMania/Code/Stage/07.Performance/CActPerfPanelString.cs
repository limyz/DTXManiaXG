using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using SharpDX;
using FDK;

using Color = System.Drawing.Color;

namespace DTXMania
{
    internal class CActPerfPanelString : CActivity
    {
        // メソッド

        /*
        public void SetPanelString(string str)
        {
            this.strパネル文字列 = str;
            if (base.bActivated)
            {
                CDTXMania.tReleaseTexture(ref this.txPanel);
                if ((this.strパネル文字列 != null) && (this.strパネル文字列.Length > 0))
                {
                    Bitmap image = new Bitmap(1, 1);
                    Graphics graphics = Graphics.FromImage(image);
                    graphics.PageUnit = GraphicsUnit.Pixel;
                    this.n文字列の長さdot = (int)graphics.MeasureString(this.strパネル文字列, this.ft表示用フォント).Width;
                    graphics.Dispose();
                    try
                    {
                        Bitmap bitmap2 = new Bitmap(this.n文字列の長さdot, (int)this.ft表示用フォント.Size);
                        graphics = Graphics.FromImage(bitmap2);
                        graphics.DrawString(this.strパネル文字列, this.ft表示用フォント, Brushes.White, (float)0f, (float)0f);
                        graphics.Dispose();
                        this.txPanel = new CTexture(CDTXMania.app.Device, bitmap2, CDTXMania.TextureFormat);
                        this.txPanel.vcScaleRatio = new Vector3(0.5f, 0.5f, 1f);
                        bitmap2.Dispose();
                    }
                    catch (CTextureCreateFailedException)
                    {
                        Trace.TraceError("パネル文字列テクスチャの生成に失敗しました。");
                        this.txPanel = null;
                    }
                    this.ct進行用 = new CCounter(-278, this.n文字列の長さdot / 2, 8, CDTXMania.Timer);
                }
            }
        }
         */

        // CActivity 実装

        public override void OnActivate()
        {

            if (CDTXMania.ConfigIni.bDrumsEnabled)
            {
                this.n曲名X = 950;
                this.n曲名Y = 630;
            }
            else if (CDTXMania.ConfigIni.bGuitarEnabled)
            {
                this.n曲名X = 500;
                this.n曲名Y = 630;
            }

//          this.n文字列の長さdot = 0;
//          this.txPanel = null;
            this.ct進行用 = new CCounter();
            base.OnActivate();
        }
        public override void OnDeactivate()
        {
//          CDTXMania.tReleaseTexture(ref this.txPanel);
            this.ct進行用 = null;
            base.OnDeactivate();
        }
        public override void OnManagedCreateResources()
        {
            if (!base.bNotActivated)
            {
                this.txジャケットパネル = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_JacketPanel.png"));
                string path = CDTXMania.DTX.strFolderName + CDTXMania.DTX.PREIMAGE;
                if (!File.Exists(path))
                {
                    this.txジャケット画像 = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\5_preimage default.png"));
                }
                else
                {
                    this.txジャケット画像 = CDTXMania.tGenerateTexture(path);
                }

//              this.SetPanelString(this.strパネル文字列);

                #region[ 曲名、アーティスト名テクスチャの生成 ]
                if (string.IsNullOrEmpty(CDTXMania.DTX.TITLE) || (!CDTXMania.bCompactMode && CDTXMania.ConfigIni.b曲名表示をdefのものにする))
                    this.strSongName = CDTXMania.stageSongSelection.r現在選択中の曲.strタイトル;
                else
                    this.strSongName = CDTXMania.DTX.TITLE;

                this.pfタイトル = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.str選曲リストフォント), 20, FontStyle.Regular);
                Bitmap bmpSongName = new Bitmap(1, 1);
                bmpSongName = this.pfタイトル.DrawPrivateFont(this.strSongName, CPrivateFont.DrawMode.Edge, Color.Black, Color.Black, this.clGITADORAgradationTopColor, this.clGITADORAgradationBottomColor, true);
                this.txSongName = CDTXMania.tGenerateTexture(bmpSongName, false);
                bmpSongName.Dispose();

                this.pfアーティスト = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.str選曲リストフォント), 15, FontStyle.Regular);
                Bitmap bmpArtistName = new Bitmap(1, 1);
                bmpArtistName = this.pfアーティスト.DrawPrivateFont(CDTXMania.DTX.ARTIST, CPrivateFont.DrawMode.Edge, Color.Black, Color.Black, this.clGITADORAgradationTopColor, this.clGITADORAgradationBottomColor, true);
                this.txArtistName = CDTXMania.tGenerateTexture(bmpArtistName, false);
                bmpArtistName.Dispose();
                #endregion

                base.OnManagedCreateResources();
            }
        }
        public override void OnManagedReleaseResources()
        {
            if ( !base.bNotActivated )
            {
//              CDTXMania.tReleaseTexture( ref this.txPanel );
                CDTXMania.tReleaseTexture( ref this.txSongName );
                CDTXMania.tReleaseTexture( ref this.txArtistName );
                CDTXMania.tReleaseTexture( ref this.txジャケットパネル );
                CDTXMania.tReleaseTexture( ref this.txジャケット画像 );
                CDTXMania.tDisposeSafely( ref this.pfタイトル );
                CDTXMania.tDisposeSafely( ref this.pfアーティスト );
                base.OnManagedReleaseResources();
            }
        }
        public override int OnUpdateAndDraw()
        {
            throw new InvalidOperationException("tUpdateAndDraw(x,y)のほうを使用してください。");
        }
        public int tUpdateAndDraw()  // t進行描画
        {
            if (!base.bNotActivated)
            {
                /*
                //this.ct進行用.tUpdateLoop();
                if ((string.IsNullOrEmpty(this.strパネル文字列) || (this.txPanel == null)) || (this.ct進行用 == null))
                {
                    return 0;
                }
                float num = this.txPanel.vcScaleRatio.X;
                Rectangle rectangle = new Rectangle((int)(num), 0, (int)(360f / num), (int)this.ft表示用フォント.Size);
                if (rectangle.X < 0)
                {
                    x -= (int)(rectangle.X * num);
                    rectangle.Width += rectangle.X;
                    rectangle.X = 0;
                }
                if (rectangle.Right >= this.n文字列の長さdot)
                {
                    rectangle.Width -= rectangle.Right - this.n文字列の長さdot;
                }
                 */

                SharpDX.Matrix mat = SharpDX.Matrix.Identity;

                //
                float fScalingFactor;
                float jacketOnScreenSize = 245.0f;
                //Maintain aspect ratio by scaling only to the smaller scalingFactor
                if (jacketOnScreenSize / this.txジャケット画像.szImageSize.Width > jacketOnScreenSize / this.txジャケット画像.szImageSize.Height)
                {
                    fScalingFactor = jacketOnScreenSize / this.txジャケット画像.szImageSize.Height;
                }
                else
                {
                    fScalingFactor = jacketOnScreenSize / this.txジャケット画像.szImageSize.Width;
                }

                if (CDTXMania.ConfigIni.bDrumsEnabled)
                {
                    this.nジャケットX = 915;
                    this.nジャケットY = 287;
                   
                    mat *= SharpDX.Matrix.Scaling(fScalingFactor, fScalingFactor, 1f);
                    mat *= SharpDX.Matrix.Translation(400f, -227f, 0f);
                    mat *= SharpDX.Matrix.RotationZ(0.3f);
                }

                if (CDTXMania.ConfigIni.bGuitarEnabled)
                {
                    this.nジャケットX = 467;
                    this.nジャケットY = 287;

                    mat *= SharpDX.Matrix.Scaling(fScalingFactor, fScalingFactor, 1f);
                    mat *= SharpDX.Matrix.Translation(-28f, -94.5f, 0f);
                    mat *= SharpDX.Matrix.RotationZ(0.3f);
                }

                if (this.txジャケットパネル != null)
                    this.txジャケットパネル.tDraw2D(CDTXMania.app.Device, this.nジャケットX, this.nジャケットY);

                if (this.txジャケット画像 != null)
                    this.txジャケット画像.tDraw3D(CDTXMania.app.Device, mat);

                if (this.txSongName.szImageSize.Width > 320)
                    this.txSongName.vcScaleRatio.X = 320f / this.txSongName.szImageSize.Width;

                if (this.txArtistName.szImageSize.Width > 320)
                    this.txArtistName.vcScaleRatio.X = 320f / this.txArtistName.szImageSize.Width;

                this.txSongName.tDraw2D(CDTXMania.app.Device, this.n曲名X, this.n曲名Y);
                this.txArtistName.tDraw2D(CDTXMania.app.Device, this.n曲名X, this.n曲名Y + 35);
            }
            return 0;
        }


        // Other

        #region [ private ]
        //-----------------
        private CCounter ct進行用;
//      private int n文字列の長さdot;
        private int n曲名X;
        private int n曲名Y;
        private int nジャケットX;
        private int nジャケットY;
//      private string strパネル文字列;
        private string strSongName;
//      private CTexture txPanel;
        private CTexture txジャケットパネル;
        private CTexture txジャケット画像;
        private CTexture txSongName;
        private CTexture txArtistName;

        private CPrivateFastFont pfタイトル;
        private CPrivateFastFont pfアーティスト;

        //2014.04.05.kairera0467 GITADORAグラデーションの色。
        //本当は共通のクラスに設置してそれを参照する形にしたかったが、なかなかいいメソッドが無いため、とりあえず個別に設置。
        //private Color clGITADORAgradationTopColor = Color.FromArgb(0, 220, 200);
        //private Color clGITADORAgradationBottomColor = Color.FromArgb(255, 250, 40);
        private Color clGITADORAgradationTopColor = Color.FromArgb(255, 255, 255);
        private Color clGITADORAgradationBottomColor = Color.FromArgb(255, 255, 255);
        //-----------------
        #endregion
    }
}
