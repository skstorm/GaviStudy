using System;
using System.Collections.Generic;

namespace gipo.util
{
	class PosWrapper<T>
	{
		public T value;
		public PosInfos pos;

		public Dictionary<String, PosInfos> keyPos;
		
		public PosWrapper(T value, PosInfos pos)
		{
			this.value = value;
			this.pos = pos;
		}

		public void addPos(string key, PosInfos pos)
		{
			if (keyPos == null) {
				keyPos = new Dictionary<String, PosInfos>();
			}
			keyPos[key] = pos;
		}
	}
}
