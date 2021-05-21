using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;


namespace HamstarHelpers.Helpers.Debug {
	/// <summary>
	/// Assorted static "helper" functions pertaining to debugging and debug outputs.
	/// </summary>
	public partial class DebugHelpers {
		/// <summary>
		/// Gets a slice of the current method call (context) stack trace beginning at the specified frame (stack
		/// depth).
		/// </summary>
		/// <param name="stackFrameIdx">Depth within the current call stack.</param>
		/// <param name="omitNamespace"></param>
		/// <param name="max"></param>
		/// <param name="until"></param>
		/// <returns></returns>
		public static IList<string> GetContextSlice(
				int stackFrameIdx=1,
				bool omitNamespace=false,
				int max=-1,
				string until="Unknown Context" ) {
			var stackList = new List<string>();

			try {
				StackFrame[] frames = new StackTrace().GetFrames();

				for( int i = stackFrameIdx; i < frames.Length; i++ ) {
					if( max != -1 ) {
						if( (i - stackFrameIdx) >= max ) {
							break;
						}
					}

					StackFrame frame = frames[i];
					MethodBase method = frame.GetMethod();

					string context = (method?.DeclaringType?.Name ?? "<UnknownCTX>") + "." + method.Name;
					if( !omitNamespace ) {
						string namespaceBase = method?.DeclaringType?.Namespace?.Split( '.' )[0] ?? "<UnknownNS>";
						context = namespaceBase + "." + context;
					}

					if( context == until ) {
						break;
					}

					stackList.Add( context );
				}
			} catch {
				stackList.Add( "Unknown Context" );
			}

			return stackList;
		}


		/// <summary>
		/// Gets the current method call (context) of a stack trace at the specified frame (stack depth).
		/// </summary>
		/// <param name="stackFrameIdx">Depth within the current call stack.</param>
		/// <returns>Method, class, and namespace of the given frame of the current call stack.</returns>
		public static string GetCurrentContext( int stackFrameIdx = 1 ) {
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
