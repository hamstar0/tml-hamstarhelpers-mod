using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.TileStructure;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.User;
using HamstarHelpers.Helpers.Misc;


namespace HamstarHelpers.Commands {
	/// @private
	public class SaveTilesCommand : ModCommand {
		/// @private
		public override CommandType Type {
			get {
				if( Main.netMode == NetmodeID.SinglePlayer && !Main.dedServ ) {
					return CommandType.World;
				}
				return CommandType.Console | CommandType.World;
			}
		}
		/// @private
		public override string Command => "mh-tiles-save";
		/// @private
		public override string Usage => "/" + this.Command + " 1024 348 32 16 true true";
		/// @private
		public override string Description => "Saves a sample of terrain to file (see ModLoader\\Mod Specific Data\\HamstarHelpers)."
			+ "\n   Parameters: <left> <top> <width> <height> <exclude air> <include walls/walls only>";



		////////////////

		/// @private
		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				LogHelpers.Warn( "Not supposed to run on client." );
				return;
			}

			if( Main.netMode == NetmodeID.Server && caller.CommandType != CommandType.Console ) {
				if( !UserHelpers.HasBasicServerPrivilege( caller.Player ) ) {
					caller.Reply( "Access denied.", Color.Red );
					return;
				}
			}

			if( args.Length < 6 ) {
				if( args.Length == 0 ) {
					caller.Reply( "No arguments given.", Color.Red );
					return;
				} else {
					caller.Reply( "More arguments needed.", Color.Red );
					return;
				}
			}

			int tileX, tileY, width, height;
			bool active, walls;
			string output;

			if( this.CheckArguments( args, out tileX, out tileY, out width, out height, out active, out walls, out output ) ) {
				if( this.RunCommand( tileX, tileY, width, height, active, walls, out output ) ) {
					caller.Reply( "Success. "+output+" tiles saved.", Color.Lime );
				} else {
					caller.Reply( "Could not save terrain sample. "+output+" tiles found.", Color.Yellow );
				}
			} else {
				caller.Reply( output, Color.Red );
			}
		}


		private bool CheckArguments(
					string[] args,
					out int tileX,
					out int tileY,
					out int width,
					out int height,
					out bool activeOnly,
					out bool includesWalls,
					out string result ) {
			tileX = tileY = width = height = 0;
			activeOnly = includesWalls = false;

			if( !Int32.TryParse( args[0], out tileX ) ) {
				result = "Invalid 'tileX' argument supplied (must be int).";
				return false;
			}

			if( !Int32.TryParse( args[1], out tileY ) ) {
				result = "Invalid 'tileY' argument supplied (must be int).";
				return false;
			}

			if( !Int32.TryParse( args[2], out width ) ) {
				result = "Invalid 'width' argument supplied (must be int).";
				return false;
			}

			if( !Int32.TryParse( args[3], out height ) ) {
				result = "Invalid 'height' argument supplied (must be int).";
				return false;
			}

			if( !bool.TryParse( args[4], out activeOnly ) ) {
				result = "Invalid 'activeOnly' argument supplied (must be boolean).";
				return false;
			}

			if( !bool.TryParse( args[5], out includesWalls ) ) {
				result = "Invalid 'includesWalls' argument supplied (must be boolean).";
				return false;
			}

			if( width <= 0 || width >= Main.maxTilesX ) {
				result = "Invalid 'width' value.";
				return false;
			}
			if( height <= 0 || height >= Main.maxTilesY ) {
				result = "Invalid 'height' value.";
				return false;
			}
			if( tileX < 0 || (tileX+width) >= Main.maxTilesX ) {
				result = "Invalid 'tileX' value.";
				return false;
			}
			if( tileY < 0 || (tileY+height) >= Main.maxTilesY ) {
				result = "Invalid 'tileY' value.";
				return false;
			}

			result = "Success";
			return true;
		}


		private bool RunCommand(
					int tileX,
					int tileY,
					int width,
					int height,
					bool excludeAir,
					bool includeWalls,
					out string output ) {
			TilePattern pattern;

			if( excludeAir && includeWalls ) {
				pattern = new TilePattern( new TilePatternBuilder {
					AnyPattern = new HashSet<TilePattern> { TilePattern.AnyForeground, TilePattern.AnyWall }
				} );
			} else if( excludeAir ) {
				pattern = TilePattern.AnyForeground;
			} else if( includeWalls ) {
				pattern = TilePattern.AnyWall;
			} else {
				pattern = TilePattern.Any;
			}

			var tiles = new TileStructure( tileX, tileY, tileX + width, tileY + height, pattern );

			ModCustomDataFileHelpers.PrepareDir( ModHelpersMod.Instance );
			string fileNameWithExt = "Tile Sample "+DateTime.UtcNow.ToFileTime()+".dat";
			string fullPath = ModCustomDataFileHelpers.GetFullPath( mod, fileNameWithExt );

			output = tiles.TileCount+" tiles (from "+tiles.Bounds.Width+"x"+tiles.Bounds.Height+" area)";
			return tiles.Save( fullPath );
		}
	}
}
