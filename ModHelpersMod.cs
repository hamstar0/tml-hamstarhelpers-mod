using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	partial class ModHelpersMod : Mod {
		public static ModHelpersMod Instance;



		////////////////

		private static void UnhandledLogger( object sender, UnhandledExceptionEventArgs e ) {
			LogHelpers.Log( "UNHANDLED crash? " + e.IsTerminating + " \nSender: " + sender.ToString() + " \nMessage: " + e.ExceptionObject.ToString() );
		}



		////////////////

		public bool HasSetupContent { get; private set; }
		public bool HasAddedRecipeGroups { get; private set; }
		public bool HasAddedRecipes { get; private set; }



		////////////////

		public ModHelpersMod() {
			ModHelpersMod.Instance = this;

			this.HasSetupContent = false;
			this.HasAddedRecipeGroups = false;
			this.HasAddedRecipes = false;

			this.InitializeInner();
		}


		public override void Load() {
			this.LoadInner();
		}

		////

		public override void Unload() {
			this.UnloadInner();

			ModHelpersMod.Instance = null;
		}


		////////////////

		public override void PostSetupContent() {
			this.PostSetupContentInner();

			this.HasSetupContent = true;
			this.CheckAndProcessLoadFinish();
		}

		////////////////

		public override void AddRecipes() {
			this.AddRecipesInner();
		}

		public override void AddRecipeGroups() {
			this.AddRecipeGroupsInner();

			this.HasAddedRecipeGroups = true;
			this.CheckAndProcessLoadFinish();
		}

		public override void PostAddRecipes() {
			this.PostAddRecipesInner();

			this.HasAddedRecipes = true;
			this.CheckAndProcessLoadFinish();
		}


		////////////////

		private void CheckAndProcessLoadFinish() {
			if( !this.HasSetupContent ) { return; }
			if( !this.HasAddedRecipeGroups ) { return; }
			if( !this.HasAddedRecipes ) { return; }

			this.PostLoadAll();
		}


		////////////////

		public override void PreSaveAndQuit() {
			this.Promises.PreSaveAndExit();
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int playerWho ) {
			try {
				int protocolCode = reader.ReadInt32();
				
				if( Main.netMode == 1 ) {
					PacketProtocol.HandlePacketOnClient( protocolCode, reader, playerWho );
				} else if( Main.netMode == 2 ) {
					PacketProtocol.HandlePacketOnServer( protocolCode, reader, playerWho );
				}
			} catch( Exception e ) {
				LogHelpers.Log( "ModHelpersMod.HandlePacket - " + e.ToString() );
			}
		}


		////////////////

		//public override void UpdateMusic( ref int music ) { //, ref MusicPriority priority
		//	this.MusicHelpers.UpdateMusic();
		//}
	}
}
