using System;
using System.Diagnostics;
using System.Threading;
using FDK;

namespace DTXMania
{
    public class CDTX2WAVmode
    {
        public enum ECommand
        {
            Record,
            Cancel,
            Other
        }
        /// <summary>
        /// DTXWAVからのコマンド
        /// </summary>
        public ECommand Command
        {
            get;
            set;
        }
        public enum FormatType
        {
            WAV,
            OGG,
            MP3
        }

        /// <summary>
        /// DTX2WAVモードかどうか
        /// </summary>
        public bool Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// プレビューサウンドの再生が発生した
        /// </summary>
        public FormatType Format
        {
            get;
            set;
        }

        public int freq;
        public int bitrate;
        public string outfilename;
        public string dtxfilename;

        public bool VSyncWait
        {
            get;
            set;
        }

        public int[] nMixerVolume = { 100, 100, 100, 100, 100, 100 };

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CDTX2WAVmode()
        {
            this.Enabled = false;
            this.Command = ECommand.Other;
            this.Format = FormatType.WAV;
            this.VSyncWait = false;         // とりあえず VSyncWait=OFF固定で考える
            this.outfilename = "";
            this.dtxfilename = "";
        }

        /// <summary>
        /// DTX2WAV関連の設定のみを更新して、Config.iniに書き出す
        /// </summary>
        /*public void tUpdateConfigIni()
        {
            /// DTX2WAV関連の設定のみを更新するために、
            /// 1. 現在のconfig.ini相当の情報を、別変数にコピーしておく
            /// 2. config.iniを読み込みなおす
            /// 3. 別変数のコピーから、Viewer関連の設定を、configに入れ込む
            /// 4. Config.iniを保存する

            CConfigXml ConfigIni_backup = (CConfigXml)CDTXMania.Instance.ConfigIni.Clone();     // #36612 2016.9.12 yyagi
            CDTXMania.Instance.LoadConfig();

            CDTXMania.Instance.ConfigIni.rcViewerWindow.W = ConfigIni_backup.rcWindow.W;
            CDTXMania.Instance.ConfigIni.rcViewerWindow.H = ConfigIni_backup.rcWindow.H;
            CDTXMania.Instance.ConfigIni.rcViewerWindow.X = ConfigIni_backup.rcWindow.X;
            CDTXMania.Instance.ConfigIni.rcViewerWindow.Y = ConfigIni_backup.rcWindow.Y;

            CDTXMania.Instance.SaveConfig();

            ConfigIni_backup = null;
        }*/


        private System.IntPtr hTargetMainWindowHandle = IntPtr.Zero;
        private System.IntPtr hCurrentMainWindowHandle;

        /// <summary>
        /// DTX2WAVにメッセージを送信する
        /// </summary>
        /// <param name="strSend">送信するテキスト</param>
        public void SendMessage2DTX2WAV(string strSend)
        {
            for (int i = 0; i < 5; i++)   // 検索結果のハンドルがZeroになることがあるので、200ms間隔で5回リトライする
            {
                hCurrentMainWindowHandle = Process.GetCurrentProcess().MainWindowHandle;

                if (hTargetMainWindowHandle == IntPtr.Zero)
                {
                    //Trace.TraceInformation("ハンドル創作");
                    #region [ 既に起動中のDTX2WAV(の録音中ダイアログ)プロセスを検索する。]

                    Process[] running = Process.GetProcesses();
                    foreach (Process p in running)
                    {
                        //Trace.TraceInformation("WindowTitle: " + p.MainWindowTitle);
                        if (p.MainWindowHandle != IntPtr.Zero && p.MainWindowTitle.Contains("DTX2WAV Rel"))
                        {
                            //Trace.TraceInformation("WindowTitle: " + p.MainWindowTitle);
                            hTargetMainWindowHandle = p.MainWindowHandle;
                            break;
                        }
                    }
                    #endregion

                }

                #region [ 起動中のDTXManiaがいれば、そのプロセスにコマンドラインを投げる ]
                if (hTargetMainWindowHandle != null && strSend != null)
                {
                    CSendMessage.sendmessage(hTargetMainWindowHandle, hCurrentMainWindowHandle, strSend);
                    //Trace.TraceInformation("SendToDTX2WAV: " + strSend + ", " + hTargetMainWindowHandle + ", " + hCurrentMainWindowHandle);
                    return;
                }
                #endregion
                else
                {
                    Trace.TraceInformation("メッセージ送信先のプロセスが見つからず。5回リトライします。");
                    Thread.Sleep(200);
                }
            }
        }
    }
}
