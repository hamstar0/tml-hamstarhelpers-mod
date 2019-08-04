﻿using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using System;
using System.Reflection;


namespace HamstarHelpers.Helpers.DotNET.Reflection {
	/// <summary>
	/// Attribute for indicating reference parameters that must be expected to sometimes occur as null
	/// </summary>
	[AttributeUsage( AttributeTargets.All, AllowMultiple = false, Inherited = true )]
	public class NullableAttribute : Attribute { }




	/// <summary>
	/// Assorted static "helper" functions pertaining to reflection
	/// </summary>
	public partial class ReflectionHelpers {
		/// <summary>
		/// Handy preset for accessing any member of the given name (regardless of access restrictions).
		/// </summary>
		public readonly static BindingFlags MostAccess = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
	}
}
