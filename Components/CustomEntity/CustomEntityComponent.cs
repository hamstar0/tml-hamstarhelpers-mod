using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using System.IO;


namespace HamstarHelpers.Components.CustomEntity {
	public abstract class CustomEntityComponent : PacketProtocolData {
		public class StaticInitializer {
			protected virtual void StaticInitialize() { }
			internal void StaticInitializationWrapper() {
				this.StaticInitialize();
			}
		}



		////////////////

		internal CustomEntityComponent InternalClone() {
			return this.Clone();
		}

		protected virtual CustomEntityComponent Clone() {
			return (CustomEntityComponent)this.MemberwiseClone();
		}


		////////////////

		public virtual void UpdateSingle( CustomEntity ent ) { }
		public virtual void UpdateClient( CustomEntity ent ) { }
		public virtual void UpdateServer( CustomEntity ent ) { }


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
