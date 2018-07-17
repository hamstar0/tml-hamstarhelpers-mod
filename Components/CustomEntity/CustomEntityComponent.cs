using HamstarHelpers.Components.Network;


namespace HamstarHelpers.Components.CustomEntity {
	abstract public class CustomEntityComponentData : PacketProtocolData { }




	abstract public class CustomEntityComponent {
		protected virtual CustomEntityComponentData CreateData() { return null; }
		public abstract void Update( CustomEntity ent );

		internal CustomEntityComponentData CreateDataInternalWrapper() {
			return this.CreateData();
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
