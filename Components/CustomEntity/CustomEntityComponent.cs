using HamstarHelpers.Components.Network;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace HamstarHelpers.Components.CustomEntity {
	abstract public class CustomEntityComponentData : PacketProtocolData { }




	abstract public class CustomEntityComponent {
		protected virtual void StaticModInitialize() { }

		protected virtual CustomEntityComponentData CreateData() { return null; }
		public virtual void Update( CustomEntity ent ) { }

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
