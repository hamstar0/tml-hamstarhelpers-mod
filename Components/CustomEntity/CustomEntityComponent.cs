using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;


namespace HamstarHelpers.Components.CustomEntity {
	abstract public class CustomEntityComponent : PacketProtocolData {
		public virtual void Update( CustomEntity ent ) { }

		////////////////
		
		public class StaticInitializer {
			protected virtual void StaticInitialize() { }
			internal void StaticInitializationWrapper() {
				this.StaticInitialize();
			}
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
