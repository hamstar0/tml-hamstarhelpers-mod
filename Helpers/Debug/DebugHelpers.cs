using System;
using System.Diagnostics;
using System.Reflection;


namespace HamstarHelpers.Helpers.Debug {
	public static partial class DebugHelpers {
		public static string GetCurrentContext( int stackFrameIdx=1 ) {
			try {
				var stack = new StackTrace();
				StackFrame frame = stack.GetFrame( stackFrameIdx );
				MethodBase method = frame.GetMethod();
				string namespaceBase = method.DeclaringType.Namespace.Split('.')[0];

				return namespaceBase + "." + method.DeclaringType.Name + "." + method.Name;
			} catch {
				return "Unknown Context";
			}
		}
	}
}
