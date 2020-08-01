using System;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.TModLoader;
using HamstarHelpers.Classes.Errors;


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
			byte[] rawData = FileHelpers.DecompressFileData( rawCompressedData );

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
			byte[] rawData = FileHelpers.LoadBinaryFile( systemPath, false );
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
			var tileStructLoader = TmlHelpers.SafelyGetInstance<TileStructureLoader>();
			byte[] rawData = tileStructLoader.Save( this );

			return FileHelpers.SaveBinaryFile( rawData, systemPath, false, false );
		}
	}
}
