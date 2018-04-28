using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace gipo.util
{
	public class PosInfos {
		public string className;
		public string fileName;
		public int lineNumber;
		public string methodName;

		public PosInfos(int callerFrameIndex = 1) {
			var sf = new StackFrame(callerFrameIndex, true);
			this.className  = sf.GetMethod().ReflectedType.FullName;
			this.fileName   = sf.GetFileName();
			this.lineNumber = sf.GetFileLineNumber();
			this.methodName = sf.GetMethod().Name;
		}

		public override string ToString() {
			return string.Format("F:{0} L:{1} / {2}.{3}", fileName, lineNumber.ToString(), className, methodName);
		}
	}
}
