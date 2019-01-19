using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using System.IO;


namespace HamstarHelpers.Components.CustomEntity {
	public abstract partial class CustomEntityComponent : PacketProtocolData {
		internal CustomEntityComponent InternalClone() {
			return this.Clone();
		}

		protected virtual CustomEntityComponent Clone() {
			var clone = (CustomEntityComponent)this.MemberwiseClone();
			//clone.InternalOnInitialize();
			return clone;
		}

		////

		internal void InternalOnEntityInitialize( CustomEntity ent ) {
			this.OnEntityInitialize( ent );
		}
		internal void InternalOnAddToWorld( CustomEntity ent ) {
			this.OnAddToWorld( ent );
		}
		
		////////////////

		internal void ReadStreamForwarded( BinaryReader reader ) {
			base.ReadStream( reader );
		}
		internal void WriteStreamForwarded( BinaryWriter writer ) {
			base.WriteStream( writer );
		}
	}


	//IsItem,
	//IsPlayerHostile,
	//IsFriendlyNpcHostile,
	//IsPvpHostile,
	//IsPlayerTarget,
	//IsPvpTarget,
	//IsFriendlyNpcTarget,
	//IsHostileNpcTarget,
	//IsCapturable,
	//TakesKnockback,
	
	//IsRopeBound,
	//Floats,
	//Flies,
	//Crawls,
	//Swims
	
	//SeeksTarget,
	//AvoidsTarget,
	//Wanders,
	//AlwaysAimsAtTarget
}
