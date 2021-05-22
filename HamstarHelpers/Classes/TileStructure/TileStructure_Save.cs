using System;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Libraries.DotNET;
using HamstarHelpers.Libraries.TModLoader;


namespace HamstarHelpers.Classes.TileStructure {
	/// <summary>
	/// Represents an arbitrary arrangement of Tile data. No bounding size or contiguity expected.
	/// </summary>
	public partial class TileStructure {
		/// <summary>
		/// Loads tile data from within a mod.
		/// </summary>
		/// <param name="mod"></param>
		/// <param name="pathOfModFile"></param>
		/// <returns></returns>
		public static TileStructure Load( Mod mod, string pathOfModFile ) {
			if( !mod.FileExists(pathOfModFile) ) {
				throw new ModHelpersException( "Mod file not found ("+pathOfModFile+")." );
			}

			var loader = ModContent.GetInstance<TileStructureLoader>();
			byte[] rawCompressedData = mod.GetFileBytes( pathOfModFile );
			byte[] rawData = FileLibraries.DecompressFileData( rawCompressedData );

			TileStructure tiles = loader.Load( rawData );
			tiles.RecountTiles();

			return tiles;
		}

		/// <summary>
		/// Loads tile data from a file.
		/// </summary>
		/// <param name="systemPath"></param>
		/// <returns></returns>
		public static TileStructure Load( string systemPath ) {
			var loader = ModContent.GetInstance<TileStructureLoader>();
			byte[] rawData = FileLibraries.LoadBinaryFile( systemPath, false );
			if( rawData == null ) {
				throw new ModHelpersException( "System file not found ("+systemPath+")." );
			}

			TileStructure tiles = loader.Load( rawData );
			tiles.RecountTiles();

			return tiles;
		}



		////////////////

		/// <summary>
		/// Saves tile data to a file.
		/// </summary>
		/// <param name="systemPath">Note: Use `Path.DirectorySeparatorChar` for folders.</param>
		/// <returns>Returns `true` if file saved successfully.</returns>
		public bool Save( string systemPath ) {
			var tileStructLoader = TmlLibraries.SafelyGetInstance<TileStructureLoader>();
			byte[] rawData = tileStructLoader.Save( this );

			return FileLibraries.SaveBinaryFile( rawData, systemPath, false, false );
		}
	}
}
