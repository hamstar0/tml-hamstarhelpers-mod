﻿using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.UserHelpers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	public class ModCallCommand : ModCommand {
		public override CommandType Type {
			get {
				if( Main.netMode == 0 && !Main.dedServ ) {
					return CommandType.World;
				}
				return CommandType.Console | CommandType.World;
			}
		}
		public override string Command { get { return "mh-mod-call"; } }
		public override string Usage { get { return "/" + this.Command + " MyModName ModAPIFunctionName unquotedstringparam 42 \"quote-wrapped strings needs spaces\" anotherparametc"; } }
		public override string Description {
			get {
				return "Runs Mod.Call(). Use with care!" + "\n   Parameters: <mod name> <parameter 1> <parameter 2> etc...";
			}
		}


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( Main.netMode == 1 ) {
				LogHelpers.Log( "ModCallCommand - Not supposed to run on client." );
				return;
			}

			if( Main.netMode == 2 && caller.CommandType != CommandType.Console ) {
				if( !UserHelpers.HasBasicServerPrivilege( caller.Player ) ) {
					caller.Reply( "Access denied.", Color.Red );
					return;
				}
			}

			ModHelpersMod mymod = ModHelpersMod.Instance;
			if( !mymod.Config.ModCallCommandEnabled ) {
				throw new UsageException( "Mod.Call() command disabled by settings." );
			}

			if( args.Length < 2 ) {
				if( args.Length == 0 ) {
					throw new UsageException( "No arguments given." );
				} else {
					throw new UsageException( "More arguments needed." );
				}
			}

			Mod callmod = null;
			try {
				callmod = ModLoader.GetMod( args[0] );
				if( callmod == null ) { throw new Exception(); }
			} catch( Exception ) {
				throw new UsageException( "Invald mod name " + args[0] );
			}

			try {
				object[] callArgs = new object[args.Length - 1];

				for( int i = 1; i < args.Length; i++ ) {
					callArgs[i - 1] = DotNetHelpers.ParseToInferredPrimitiveType( args[i] );
				}

				callmod.Call( callArgs );
			} catch( Exception e ) {
				caller.Reply( e.Message, Color.Red );
			}
		}
	}
}
