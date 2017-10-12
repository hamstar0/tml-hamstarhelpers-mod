using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers {
	class MyModWorld : ModWorld {
		public string ID { get; private set; }
		public bool HasCorrectID { get; internal set; }  // Workaround for tml bug?

		public MyLogic Logic { get; private set; }



		////////////////

		public override void Initialize() {
			var mymod = (HamstarHelpersMod)this.mod;

			this.Logic = new MyLogic( mymod );

			this.ID = Guid.NewGuid().ToString( "D" );
			this.HasCorrectID = false;	// 'Load()' decides if no pre-existing one is found
		}

		////////////////

		public override void Load( TagCompound tag ) {
			var mymod = (HamstarHelpersMod)this.mod;
			int half_days = 0;

			if( tag.ContainsKey( "world_id" ) ) {
				this.ID = tag.GetString( "world_id" );
				half_days = tag.GetInt( "half_days_elapsed_" + this.ID );
			}

			this.HasCorrectID = true;

			this.Logic.LoadOnce( half_days );
		}

		public override TagCompound Save() {
			var tag = new TagCompound {
				{ "world_id", this.ID },
				{ "half_days_elapsed_" + this.ID, (int)this.Logic.HalfDaysElapsed },
			};

			return tag;
		}

		
		////////////////

		public override void NetSend( BinaryWriter writer ) {
			writer.Write( this.HasCorrectID );
			writer.Write( this.ID );
		}

		public override void NetReceive( BinaryReader reader ) {
			bool has_correct_id = reader.ReadBoolean();
			string id = reader.ReadString();

			if( has_correct_id ) {
				this.ID = id;
				this.HasCorrectID = true;
			}
		}

		////////////////

		public override void PreUpdate() {
			var mymod = (HamstarHelpersMod)this.mod;

			if( Main.netMode == 2 ) { // Server only
				if( this.Logic != null && mymod.HasSetupContent ) {
					this.Logic.Update();
				}
			}
		}
	}
}
