using System;
using System.Collections.Generic;
using System.Text;

namespace FDK
{
	/// <summary>
	/// 一定間隔で単純増加する整数（カウント値）を扱う。
	/// </summary>
	public class CCounter
	{
		// 値プロパティ

		public int nStartValue //n開始値
		{
			get;
			private set;
		}
		public int nEndValue //n終了値
		{
			get;
			private set;
		}
		public int nCurrentValue  // n現在の値
		{
			get;
			set;
		}
		public long nCurrentElapsedTimeMs  // n現在の経過時間ms
		{
			get;
			set;
		}


		// 状態プロパティ

		public bool bInProgress //b進行中
		{
			get { return (this.nCurrentElapsedTimeMs != -1); }
		}
		public bool bStopped //b停止中
		{
			get { return !this.bInProgress; }
		}
		public bool bReachedEndValue  // n現在の経過時間ms
		{
			get { return (this.nCurrentValue >= this.nEndValue); }
		}
		public bool b終了値に達してない
		{
			get { return !this.bReachedEndValue; }
		}


		// コンストラクタ
		
		public CCounter()
		{
			this.timer = null;
			this.nStartValue = 0;
			this.nEndValue = 0;
			this.nInterval = 0;
			this.nCurrentValue = 0;
			this.nCurrentElapsedTimeMs = CTimer.nUnused;
		}

		/// <summary>
		/// 生成と同時に開始する。
		/// Starts the counter immediately upon creation.
		/// </summary>
		public CCounter(int n開始値, int n終了値, int n間隔ms, CTimer timer)
			: this()
		{
			this.tStart(n開始値, n終了値, n間隔ms, timer);
		}


		// 状態操作メソッド

		/// <summary>
		/// カウントを開始する。
		/// </summary>
		/// <param name="nStartValue">最初のカウント値。</param>
		/// <param name="nEndValue">最後のカウント値。</param>
		/// <param name="nInterval">Time in milliseconds between each timer increment カウント値を１増加させるのにかける時間（ミリ秒単位）。</param>
		/// <param name="timer">Timer used for counting カウントに使用するタイマ。</param>
		public void tStart(int nStartValue, int nEndValue, int nInterval, CTimer timer)  // t開始
		{
			this.nStartValue = nStartValue;
			this.nEndValue = nEndValue;
			this.nInterval = nInterval;
			this.timer = timer;
			this.nCurrentElapsedTimeMs = this.timer.nCurrentTime;
			this.nCurrentValue = nStartValue;
		}

		/// <summary>
		/// 前回の tUpdate() の呼び出しからの経過時間をもとに、必要なだけカウント値を増加させる。
		/// カウント値が終了値に達している場合は、それ以上増加しない（終了値を維持する）。
		/// Increments the CurrentValue as necessary based on the time elapsed since the last call to tUpdate().
		/// CurrentValue will be clamped to EndValue once EndValue is reached.
		/// </summary>
		public void tUpdate()  // t進行
		{
			if ((this.timer != null) && (this.nCurrentElapsedTimeMs != CTimer.nUnused))
			{
				long num = this.timer.nCurrentTime;
				if (num < this.nCurrentElapsedTimeMs)
					this.nCurrentElapsedTimeMs = num;

				while ((num - this.nCurrentElapsedTimeMs) >= this.nInterval)
				{
					if (++this.nCurrentValue > this.nEndValue)
						this.nCurrentValue = this.nEndValue;

					this.nCurrentElapsedTimeMs += this.nInterval;
				}
			}
		}

		/// <summary>
		/// 前回の tUpdateLoop() の呼び出しからの経過時間をもとに、必要なだけカウント値を増加させる。
		/// カウント値が終了値に達している場合は、次の増加タイミングで開始値に戻る（値がループする）。
		/// Increments the CurrentValue as necessary based on the time elapsed since the last call to tUpdateLoop().
		/// CurrentValue will reset to StartValue once EndValue is reached. Otherwise Identical to tUpdate().
		/// </summary>
		public void tUpdateLoop()  // t進行Loop
		{
			if ((this.timer != null) && (this.nCurrentElapsedTimeMs != CTimer.nUnused))
			{
				long num = this.timer.nCurrentTime;
				if (num < this.nCurrentElapsedTimeMs)
					this.nCurrentElapsedTimeMs = num;

				while ((num - this.nCurrentElapsedTimeMs) >= this.nInterval)
				{
					if (++this.nCurrentValue > this.nEndValue)
						this.nCurrentValue = this.nStartValue;

					this.nCurrentElapsedTimeMs += this.nInterval;
				}
			}
		}

		/// <summary>
		/// カウントを停止する。
		/// これ以降に t進行() や t進行Loop() を呼び出しても何も処理されない。
		/// Stop the counter. No further processing will be done by tUpdate() or tUpdateLoop().
		/// </summary>
		public void tStop()  // t停止
		{
			this.nCurrentElapsedTimeMs = CTimer.nUnused;
		}


		// その他

		#region [ 応用：キーの反復入力をエミュレーションする ]
		//-----------------

		/// <summary>
		/// <para>「bキー押下」引数が true の間中、「tキー処理」デリゲート引数を呼び出す。</para>
		/// <para>ただし、2回目の呼び出しは1回目から 200ms の間を開けてから行い、3回目以降の呼び出しはそれぞれ 30ms の間隔で呼び出す。</para>
		/// <para>「bキー押下」が false の場合は何もせず、呼び出し回数を 0 にリセットする。</para>
		/// </summary>
		/// <param name="bキー押下">キーが押下されている場合は true。</param>
		/// <param name="tキー処理">キーが押下されている場合に実行する処理。</param>
		public void tRepeatKey(bool bキー押下, DGキー処理 tキー処理)  // tキー反復
		{
			const int n1回目 = 0;
			const int n2回目 = 1;
			const int n3回目以降 = 2;

			if (bキー押下)
			{
				switch (this.nCurrentValue)
				{
					case n1回目:

						tキー処理();
						this.nCurrentValue = n2回目;
						this.nCurrentElapsedTimeMs = this.timer.nCurrentTime;
						return;

					case n2回目:

						if ((this.timer.nCurrentTime - this.nCurrentElapsedTimeMs) > 200)
						{
							tキー処理();
							this.nCurrentElapsedTimeMs = this.timer.nCurrentTime;
							this.nCurrentValue = n3回目以降;
						}
						return;

					case n3回目以降:

						if ((this.timer.nCurrentTime - this.nCurrentElapsedTimeMs) > 30)
						{
							tキー処理();
							this.nCurrentElapsedTimeMs = this.timer.nCurrentTime;
						}
						return;
				}
			}
			else
			{
				this.nCurrentValue = n1回目;
			}
		}
		public delegate void DGキー処理();

		//-----------------
		#endregion

		#region [ private ]
		//-----------------
		private CTimer timer;
		private int nInterval;
		//-----------------
		#endregion
	}
}