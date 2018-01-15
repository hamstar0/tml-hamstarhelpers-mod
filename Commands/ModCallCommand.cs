using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers.Commands {
	public class ModCallCommand : ModCommand {
		public override CommandType Type {
			get {
				if( Main.netMode == 0 ) {
					return CommandType.World;
				}
				return CommandType.Console;
			}
		}
		public override string Command { get { return "hhmodcall"; } }
		public override string Usage { get { return "/hhmodcall MyModName ModAPIFunctionName unquotedstringparam anotherparam nextparamisint 42 \"quote-wrapped string w/ spaces etc\""; } }
		public override string Description { get { return "Runs Mod.Call(). Use with care!"; } }


		////////////////
		
		public override void Action( CommandCaller caller, string input, string[] args ) {
			var mymod = HamstarHelpersMod.Instance;
			if( !mymod.Config.ModCallCommandEnabled ) {
				throw new UsageException( "Mod.Call() command disabled by settings." );
			}

			if( args.Length <= 2 ) { throw new UsageException( "Invald arguments." ); }
			
			Mod callmod = ModLoader.GetMod( args[0] );
			if( callmod == null ) {
				throw new UsageException( "Invald mod name argument." );
			}

			try {
				object[] call_args = new object[ args.Length - 1 ];
				Array.Copy( args, 1, call_args, 0, args.Length - 1 );

				callmod.Call( call_args );
			} catch( Exception e ) {
				caller.Reply( e.Message, Color.Red );
			}
		}
	}
}
