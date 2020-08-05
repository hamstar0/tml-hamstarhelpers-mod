using HamstarHelpers.Classes.UI.ModConfig;
using System;
using Terraria.ModLoader.Config;


namespace HamstarHelpers.Helpers.TModLoader.Configs {
	/// <summary>
	/// Allows defining a "full" int value (furthest positive and negative values). Used for array/list/etc. entries.
	/// </summary>
	public class FullIntDefinition {
		/// <summary></summary>
		[Range( Int32.MinValue, Int32.MaxValue )]
		public int Amount;
	}


	/// <summary>
	/// Allows defining a positive int value (furthest positive values only). Used for array/list/etc. entries.
	/// </summary>
	public class PositiveIntDefinition {
		/// <summary></summary>
		[Range( 0, Int32.MaxValue )]
		public int Amount;
	}


	/// <summary>
	/// Allows defining a bi-directional "percent" float value (between -1 and 1). Used for array/list/etc. entries.
	/// </summary>
	public class FullPercentFloatDefinition {
		/// <summary></summary>
		[Range( -1f, 1f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float Amount;
	}


	/// <summary>
	/// Allows defining a "percent" float value (between 0 and 1). Used for array/list/etc. entries.
	/// </summary>
	public class PercentFloatDefinition {
		/// <summary></summary>
		[Range( 0f, 1f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float Amount;
	}


	/// <summary>
	/// Allows defining a "full" float value (furthest negative and positive ranges). Used for array/list/etc. entries.
	/// </summary>
	public class FullSingleDefinition {
		/// <summary></summary>
		[Range( Single.MinValue, Single.MaxValue )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float Amount;
	}


	/// <summary>
	/// Allows defining a positive float value (furthest positive range only). Used for array/list/etc. entries.
	/// </summary>
	public class PositiveSingleDefinition {
		/// <summary></summary>
		[Range( 0, Single.MaxValue )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float Amount;
	}


	/*public class FullDoubleDefinition {
		[Range( Double.MinValue, Double.MaxValue )]
		[CustomModConfigItem( typeof( DoubleInputElement ) )]
		public double Amount;
	}

	public class PositiveDoubleDefinition {
		[Range( 0, Double.MaxValue )]
		[CustomModConfigItem( typeof( DoubleInputElement ) )]
		public double Amount;
	}*/


	////////////////

	/// <summary>
	/// Allows defining a coordinate value representing a possible screen position (as a percent). Negative values are
	/// meant to map from right-to-left, bottom-to-top, instead. Used for array/list/etc. entries.
	/// </summary>
	public class FullScreenPercentPositionDefinition {
		/// <summary></summary>
		[Range( -1f, 1f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float X;
		/// <summary></summary>
		[Range( -1f, 1f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float Y;
	}


	/// <summary>
	/// Allows defining a coordinate value representing a possible screen position (as a percent).
	/// Used for array/list/etc. entries.
	/// </summary>
	public class ScreenPercentPositionDefinition {
		/// <summary></summary>
		[Range( 0f, 1f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float X;
		/// <summary></summary>
		[Range( 0f, 1f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float Y;
	}


	////

	/// <summary>
	/// Allows defining a coordinate value representing a possible screen position (up to the theoretical largest
	/// resolution Terraria supports). Negative values are meant to map from right-to-left, bottom-to-top, instead.
	/// Used for array/list/etc. entries.
	/// </summary>
	public class FullScreenPositionDefinition {
		/// <summary></summary>
		[Range( -4096, 4096 )]
		public int X;
		/// <summary></summary>
		[Range( -2160, 2160 )]
		public int Y;
	}

	/// <summary>
	/// Allows defining a coordinate value representing a possible screen position (up to the theoretical largest
	/// resolution Terraria supports). Used for array/list/etc. entries.
	/// </summary>
	public class ScreenPositionDefinition {
		/// <summary></summary>
		[Range( 0, 4096 )]
		public int X;
		/// <summary></summary>
		[Range( 0, 2160 )]
		public int Y;
	}
}
