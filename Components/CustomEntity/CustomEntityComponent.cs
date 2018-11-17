using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.IO;


namespace HamstarHelpers.Components.CustomEntity {
	public abstract class CustomEntityComponent : PacketProtocolData {
		public class StaticInitializer {
			internal StaticInitializer() { }
			internal void StaticInitializationWrapper() {
				this.StaticInitialize();
			}

			protected virtual void StaticInitialize() { }
		}



		////////////////

		protected CustomEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }

		////

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
