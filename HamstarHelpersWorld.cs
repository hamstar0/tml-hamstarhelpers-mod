using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace HamstarHelpers {
	class HamstarHelpersWorld : ModWorld {
		public string UID { get; private set; }
		
		internal string ObsoleteID;
		public bool HasCorrectID { get; internal set; }  // Workaround for tml bug?

		internal HamstarHelpersLogic Logic { get; private set; }


		////////////////
		
		public override void Initialize() {
			var mymod = (HamstarHelpersMod)this.mod;

			this.UID = WorldHelpers.WorldHelpers.GetUniqueId();
			this.ObsoleteID = Guid.NewGuid().ToString( "D" );
			this.HasCorrectID = false;  // 'Load()' decides if no pre-existing one is found

			this.Logic = new HamstarHelpersLogic( mymod );

			if( String.IsNullOrEmpty(this.UID) ) {
				throw new Exception( "UID not defined." );
			}
		}

		////////////////

		public override void Load( TagCompound tags ) {
			var mymod = (HamstarHelpersMod)this.mod;

			if( tags.ContainsKey( "world_id" ) ) {
				this.ObsoleteID = tags.GetString( "world_id" );
			}
			
			this.Logic.Load( mymod, tags );

			this.HasCorrectID = true;
		}

		public override TagCompound Save() {
			var mymod = (HamstarHelpersMod)this.mod;
			TagCompound tags = new TagCompound();

			tags.Set( "world_id", this.ObsoleteID );
			this.Logic.Save( mymod, tags );

			return tags;
		}

		
		////////////////

		public override void NetSend( BinaryWriter writer ) {
			var mymod = (HamstarHelpersMod)this.mod;

			try {
				writer.Write( this.HasCorrectID );
				writer.Write( this.ObsoleteID );
				this.Logic.NetSend( writer );
			} catch( Exception e ) {
				ErrorLogger.Log( e.ToString() );
			}
		}

		public override void NetReceive( BinaryReader reader ) {
			var mymod = (HamstarHelpersMod)this.mod;

			try {
				bool has_correct_id = reader.ReadBoolean();
				string id = reader.ReadString();

				if( has_correct_id ) {
					this.ObsoleteID = id;
					this.HasCorrectID = true;
					this.Logic.NetReceive( reader );
				}
			} catch( Exception e ) {
				ErrorLogger.Log( e.ToString() );
			}
		}

		////////////////

		public override void PreUpdate() {
			var mymod = (HamstarHelpersMod)this.mod;
			
			if( Main.netMode != 1 ) { // Not client
				if( this.Logic != null ) {
					this.Logic.Update( mymod );
				}
			}
		}
	}
}
