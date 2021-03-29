using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace HamstarHelpers.Classes.Tiles.TilePattern {
	/// <summary>
	/// Identifies a type of tile by its attributes.
	/// </summary>
	public partial class TilePattern {
		/// <summary></summary>
		/// <returns></returns>
		public override string ToString() {
			return this.ToString( "" );
		}

		private string ToString( string prefix ) {
			var output = new List<string>();
			if( this.IsActive.HasValue ) {
				output.Add( this.IsActive.Value ? "active" : "!active" );
			}
			if( this.HasWall.HasValue ) {
				output.Add( this.HasWall.Value ? "wall" : "!wall" );
			}
			if( (this.IsAnyOfType?.Count ?? 0) > 0 ) {
				output.Add( "types: "+string.Join(",", this.IsAnyOfType) );
			}
			if( (this.IsAnyOfWallType?.Count ?? 0) > 0 ) {
				output.Add( "wall types: "+string.Join(",", this.IsAnyOfWallType) );
			}
			if( (this.IsNotAnyOfType?.Count ?? 0) > 0 ) {
				output.Add( "!types: "+string.Join(",", this.IsNotAnyOfType) );
			}
			if( (this.IsNotAnyOfWallType?.Count ?? 0) > 0 ) {
				output.Add( "!wall types: "+string.Join(",", this.IsNotAnyOfWallType) );
			}
			if( this.AreaFromCenter.HasValue ) {
				output.Add( "adjacent: "+this.AreaFromCenter.Value.ToString() );
			}
			if( this.HasWire1.HasValue ) {
				output.Add( this.HasWire1.Value ? "wire1" : "!wire1" );
			}
			if( this.HasWire2.HasValue ) {
				output.Add( this.HasWire2.Value ? "wire2" : "!wire2" );
			}
			if( this.HasWire3.HasValue ) {
				output.Add( this.HasWire3.Value ? "wire3" : "!wire3" );
			}
			if( this.HasWire4.HasValue ) {
				output.Add( this.HasWire4.Value ? "wire4" : "!wire4" );
			}
			if( this.HasSolidProperties.HasValue ) {
				output.Add( this.HasSolidProperties.Value ? "solid" : "!solid" );
			}
			if( this.IsPlatform.HasValue ) {
				output.Add( this.IsPlatform.Value ? "platform" : "!platform" );
			}
			if( this.IsActuated.HasValue ) {
				output.Add( this.IsActuated.Value ? "actuated" : "!actuated" );
			}
			if( this.IsVanillaBombable.HasValue ) {
				output.Add( this.IsVanillaBombable.Value ? "bombable" : "!bombable" );
			}
			if( this.HasWater.HasValue ) {
				output.Add( this.HasWater.Value ? "water" : "!water" );
			}
			if( this.HasHoney.HasValue ) {
				output.Add( this.HasHoney.Value ? "honey" : "!honey" );
			}
			if( this.HasLava.HasValue ) {
				output.Add( this.HasLava.Value ? "lava" : "!lava" );
			}
			if( this.Shape.HasValue ) {
				output.Add( "shape:"+this.Shape.Value.ToString() );
			}
			if( this.MinimumBrightness.HasValue ) {
				output.Add( "min bright%:"+this.MinimumBrightness.Value.ToString("N2") );
			}
			if( this.MaximumBrightness.HasValue ) {
				output.Add( "max bright%:"+this.MaximumBrightness.Value.ToString("N2") );
			}
			if( this.IsModded.HasValue ) {
				output.Add( this.IsModded.Value ? "modded" : "!modded" );
			}
			if( this.CustomCheck != null ) {
				output.Add( "custom" );
			}
			if( this.Invert ) {
				output.Add( "inverted" );
			}
			if( (this.AnyPattern?.Count ?? 0) > 0 ) {
				IEnumerable<string> patterns = this.AnyPattern.Select( p => p.ToString(prefix+"  ") );
				output.Add( "also:\n  "+prefix+string.Join("\n  "+prefix, patterns) );
			}

			return prefix + string.Join( ", ", output );
		}
	}
}
