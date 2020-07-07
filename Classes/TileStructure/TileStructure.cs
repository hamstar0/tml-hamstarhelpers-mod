using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.DotNET.Extensions;


namespace HamstarHelpers.Classes.TileStructure {
	/// <summary>
	/// Represents an arbitrary arrangement of Tile data. No bounding size or contiguity expected.
	/// </summary>
	public class TileStructure {
		/// <summary>
		/// Loads tile data from within a mod.
		/// </summary>
		/// <param name="mod"></param>
		/// <param name="pathOfModFile"></param>
		/// <returns></returns>
		public static TileStructure Load( Mod mod, string pathOfModFile ) {
			var loader = ModContent.GetInstance<TileStructureLoader>();
			byte[] rawData = mod.GetFileBytes( pathOfModFile );

			return loader.Load( rawData );
		}

		/// <summary>
		/// Loads tile data from a file.
		/// </summary>
		/// <param name="pathRelativeToModLoaderFolder"></param>
		/// <returns></returns>
		public static TileStructure Load( string pathRelativeToModLoaderFolder ) {
			var loader = ModContent.GetInstance<TileStructureLoader>();
			byte[] rawData = FileHelpers.LoadBinaryFile( pathRelativeToModLoaderFolder, false );

			return loader.Load( rawData );
		}



		////////////////

		/// <summary>
		/// 2D collection of Tile data.
		/// </summary>
		public IDictionary<int, IDictionary<int, Tile>> Structure { get; }
			= new Dictionary<int, IDictionary<int, Tile>>();



		////////////////
		
		/// <summary></summary>
		public TileStructure() { }

		/// <summary></summary>
		/// <param name="left"></param>
		/// <param name="top"></param>
		/// <param name="right"></param>
		/// <param name="bottom"></param>
		/// <param name="pattern"></param>
		public TileStructure( int left, int top, int right, int bottom, TilePattern pattern ) {
			if( left < 0 || right >= Main.maxTilesX || top < 0 || bottom >= Main.maxTilesY ) {
				throw new ArgumentException( "Ranges exceed map boundaries" );
			}
			if( left >= right || top >= bottom ) {
				throw new ArgumentException( "Invalid ranges" );
			}

			for( int x=left; x<right; x++ ) {
				for( int y=top; y<bottom; y++ ) {
					if( pattern.Check(x, y) ) {
						this.Structure.Set2D( x, y, Main.tile[x, y] );
					}
				}
			}
		}


		////////////////

		/// <summary>
		/// Saves tile data to a file.
		/// </summary>
		/// <param name="pathRelativeToModLoaderFolder">Note: Use `Path.DirectorySeparatorChar` for subfolders.</param>
		/// <returns>Returns `true` if file saved successfully.</returns>
		public bool Save( string pathRelativeToModLoaderFolder ) {
			var loader = ModContent.GetInstance<TileStructureLoader>();
			byte[] rawData = loader.Save( this );
			string fullPath = Main.SavePath + Path.DirectorySeparatorChar + pathRelativeToModLoaderFolder;

			return FileHelpers.SaveBinaryFile( rawData, fullPath, false, false );
		}
	}
}
