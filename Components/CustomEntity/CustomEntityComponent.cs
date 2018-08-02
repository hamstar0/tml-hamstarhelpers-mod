using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using System.IO;


namespace HamstarHelpers.Components.CustomEntity {
	abstract public class CustomEntityComponent : PacketProtocolData {
		public virtual CustomEntityComponent Clone() {
			return (CustomEntityComponent)null;
		}

		public virtual void UpdateSingle( CustomEntity ent ) { }
		public virtual void UpdateClient( CustomEntity ent ) { }
		public virtual void UpdateServer( CustomEntity ent ) { }

		////////////////

		public class StaticInitializer {
			protected virtual void StaticInitialize() { }
			internal void StaticInitializationWrapper() {
				this.StaticInitialize();
			}
		}

		////////////////

		internal void ReadStreamForwarded( BinaryReader reader ) {
			this.ReadStream( reader );
		}
		internal void WriteStreamForwarded( BinaryWriter writer ) {
			this.WriteStream( writer );
		}
	}


	//IsItem,
	//IsPlayerHostile,
	//IsFriendlyNpcHostile,
	//IsPvpHostile,
	//IsPlayerTarget,
	//IsPvpTarget,
	//IsFiendlyNpcTarget,
	//IsHostileNpcTarget,
	//IsCapturable,
	//TakesHits,
	//TakesDamage,
	//TakesKnockback,
	//RespectsTerrain

	//SeeksTarget,
	//IsGravityBound,
	//IsRailBound,
	//IsRopeBound,
	//Floats,
	//Flies,
	//Crawls,
	//Swims

	//abstract public class CustomEntityAttributeBehavior { }
	//SeeksTarget,
	//AvoidsTarget,
	//Wanders,
	//AlwaysAimsAtTarget
}
