using UnityEngine;
using gipo.core;

namespace Ark.Core
{
	public class FrameManager : GearHolder
	{
		// 時間倍率
		private float _speedRate = 1.0f;
		// Frame per sencod (1秒に回るFrame数)
		public readonly int FPS = 60;
		// Sencond per frame (１Frameかかる時間（秒）)
		public readonly float SPF = 1.0f;
		// 前回のupdate()の最後のタイミングの、開始時刻からの秒数
		private float _lastUpdateSeconds = 0;
		// 経過時間を蓄積する変数
		private float _elapsedSeconds = 0.0f;

		public FrameManager(int fps) : base(false)
		{
			FPS = fps;
			SPF = 1.0f / FPS;
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 最後に更新した時間（秒）を記録する
		public void RecordLastUpdateSeconds()
		{
			_lastUpdateSeconds = Time.time;
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! Frameを計算する
		public int CalcDeltaFrame()
		{
			int deltaFrame = 0;
			// 経過時間を蓄積しておく
			_elapsedSeconds += ((Time.time - _lastUpdateSeconds) * _speedRate);
			// Frameを数える
			while (_elapsedSeconds >= SPF)
			{
				_elapsedSeconds -= SPF;
				++deltaFrame;
			}

			return deltaFrame;
		}
	}

}