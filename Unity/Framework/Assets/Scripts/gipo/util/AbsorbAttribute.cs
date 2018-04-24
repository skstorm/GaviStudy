using System;

namespace gipo.util
{
	[AttributeUsage(AttributeTargets.Field)]
	public class AbsorbAttribute : Attribute
	{
		public AbsorbAttribute() {}
	}
}
