using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;
using Newtonsoft.Json;
using System.IO;


namespace HamstarHelpers.Components.CustomEntity {
	abstract public class CustomEntityComponent : PacketProtocolData {
		public class StaticInitializer {
			protected virtual void StaticInitialize() { }
			internal void StaticInitializationWrapper() {
				this.StaticInitialize();
			}
		}

		////////////////

		[JsonIgnore]
		[PacketProtocolIgnore]
		public bool IsInitialized { get; protected set; }


		////////////////

		protected virtual void ConfirmLoad() {
			this.IsInitialized = true;
		}

		public virtual CustomEntityComponent Clone() {
			return (CustomEntityComponent)null;
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
