using System.IO;


namespace HamstarHelpers.Components.CustomEntity {
	abstract public class CustomEntityData { }




	abstract public class CustomEntityProperty {
		public abstract CustomEntityData CreateData();
		public abstract void Update( CustomEntity ent );
		internal virtual void Serialize( BinaryWriter writer, CustomEntity ent ) { }
		internal virtual void Deserialize( BinaryReader reader, CustomEntity ent ) { }
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
