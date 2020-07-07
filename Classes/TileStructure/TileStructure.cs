using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.DotNET;


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
		public static TileStructure LoadTileStructure( Mod mod, string pathOfModFile ) {
			var loader = ModContent.GetInstance<TileStructureLoader>();
			byte[] rawData = mod.GetFileBytes( pathOfModFile );

			return loader.Load( rawData );
		}



		////////////////

		/// <summary>
		/// 2D collection of Tile data.
		/// </summary>
		public IDictionary<int, IDictionary<int, Tile>> Structure { get; }
			= new Dictionary<int, IDictionary<int, Tile>>();



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
