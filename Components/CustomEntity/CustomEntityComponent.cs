using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using System.IO;


namespace HamstarHelpers.Components.CustomEntity {
	public abstract class CustomEntityComponent : PacketProtocolData {
		protected abstract class ComponentFactory<T> : Factory<T> where T : CustomEntityComponent {
			public ComponentFactory( out T ent ) : base( out ent ) { }
		}


		////////////////

		protected abstract class CustomEntityComponentFactory<T> : Factory<T> where T : CustomEntityComponent {
			public CustomEntityComponentFactory( string player_uid, out T comp ) : base( out comp ) { }
		}

		

		public class StaticInitializer {
			protected virtual void StaticInitialize() { }
			internal void StaticInitializationWrapper() {
				this.StaticInitialize();
			}
		}



		////////////////

		protected CustomEntityComponent( PacketProtocolDataConstructorLock ctor_lock ) : base( ctor_lock ) { }


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
