using HamstarHelpers.Components.Network;


namespace HamstarHelpers.Components.CustomEntity {
	abstract public class CustomEntityPropertyData : PacketProtocolData { }




	abstract public class CustomEntityProperty {
		protected virtual CustomEntityPropertyData CreateData() { return null; }
		public abstract void Update( CustomEntity ent );

		internal CustomEntityPropertyData CreateDataInternalWrapper() {
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
