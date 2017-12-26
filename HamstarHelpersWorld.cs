using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers {
	class HamstarHelpersWorld : ModWorld {
		public string ID { get; private set; }
		public bool HasCorrectID { get; internal set; }  // Workaround for tml bug?

		public HamstarHelpersLogic Logic { get; private set; }



		////////////////

		public override void Initialize() {
			var mymod = (HamstarHelpersMod)this.mod;

			//mymod.WorldEvents.OnInitialize();

			this.Logic = new HamstarHelpersLogic( mymod );

			this.ID = Guid.NewGuid().ToString( "D" );
			this.HasCorrectID = false;	// 'Load()' decides if no pre-existing one is found
		}

		////////////////

		public override void Load( TagCompound tags ) {
			var mymod = (HamstarHelpersMod)this.mod;
			int half_days = 0;

			//mymod.WorldEvents.OnLoad( tags );

			if( tags.ContainsKey( "world_id" ) ) {
				this.ID = tags.GetString( "world_id" );
				half_days = tags.GetInt( "half_days_elapsed_" + this.ID );
			}

			this.HasCorrectID = true;

			this.Logic.LoadOnce( half_days );
		}

		public override TagCompound Save() {
			var mymod = (HamstarHelpersMod)this.mod;
			var tags = new TagCompound {
				{ "world_id", this.ID },
				{ "half_days_elapsed_" + this.ID, (int)this.Logic.HalfDaysElapsed },
			};

			//mymod.WorldEvents.OnSave( tags );

			return tags;
		}

		
		////////////////

		public override void NetSend( BinaryWriter writer ) {
			var mymod = (HamstarHelpersMod)this.mod;

			//mymod.WorldEvents.OnNetSend( writer );

			try {
				writer.Write( this.HasCorrectID );
				writer.Write( this.ID );
			} catch( Exception e ) {
				ErrorLogger.Log( e.ToString() );
			}
		}

		public override void NetReceive( BinaryReader reader ) {
			var mymod = (HamstarHelpersMod)this.mod;

			//mymod.WorldEvents.OnNetReceive( reader );

			try {
				bool has_correct_id = reader.ReadBoolean();
				string id = reader.ReadString();

				if( has_correct_id ) {
					this.ID = id;
					this.HasCorrectID = true;
				}
			} catch( Exception e ) {
				ErrorLogger.Log( e.ToString() );
			}
		}

		////////////////

		public override void PreUpdate() {
			var mymod = (HamstarHelpersMod)this.mod;

			//mymod.WorldEvents.OnPreUpdate();
			
			if( Main.netMode == 2 ) { // Server only
				if( this.Logic != null && mymod.HasSetupContent ) {
					this.Logic.Update( mymod );
				}
			}
		}


		////////////////

		/*public override void ChooseWaterStyle( ref int style ) {
			base.ChooseWaterStyle( ref style );
		}
		public override void LoadLegacy( BinaryReader reader ) {
			base.LoadLegacy( reader );
		}
		public override void ModifyWorldGenTasks( List<GenPass> tasks, ref float totalWeight ) {
			base.ModifyWorldGenTasks( tasks, ref totalWeight );
		}
		public override void PostDrawTiles() {
			base.PostDrawTiles();
		}
		public override void PostUpdate() {
			base.PostUpdate();
		}
		public override void PostWorldGen() {
			base.PostWorldGen();
		}
		public override void PreWorldGen() {
			base.PreWorldGen();
		}
		public override void ResetNearbyTileEffects() {
			base.ResetNearbyTileEffects();
		}
		public override void TileCountsAvailable( int[] tileCounts ) {
			base.TileCountsAvailable( tileCounts );
		}*/
	}
}
