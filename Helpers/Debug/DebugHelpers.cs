using System;
using System.Diagnostics;
using System.Reflection;


namespace HamstarHelpers.Helpers.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to debugging and debug outputs.
	/// </summary>
	public static partial class DebugHelpers {
		/// <summary>
		/// Gets the current method call (context) of a stack trace at the specified frame (stack depth).
		/// </summary>
		/// <param name="stackFrameIdx">Depth within the current call stack.</param>
		/// <returns>Method, class, and namespace of the given frame of the current call stack.</returns>
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
