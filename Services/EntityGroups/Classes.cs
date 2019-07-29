using HamstarHelpers.Services.Errors;
using System;
using System.Collections.Generic;
using Terraria;


namespace HamstarHelpers.Services.EntityGroups {
	/// <summary>
	/// Defines a set of entity groups.
	/// </summary>
	public class EntityGroupDependencies : Dictionary<string, ISet<int>> { }




	/// <summary>
	/// Wraps a function for matching entities for a given group.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EntityGroupMatcher<T> where T : Entity {
		/// <summary>
		/// Delegate for matching entity group entities. Receives an entity and the group's dependencies as parameters.
		/// Returns `true` if a match is found.
		/// </summary>
		public Func<T, EntityGroupDependencies, bool> MatcherFunc;
	}

	/// <summary>
	/// An `EntityGroupMatcher` specifically for `Item`s.
	/// </summary>
	public class ItemGroupMatcher : EntityGroupMatcher<Item> {
		/// <summary></summary>
		/// <param name="matcherFun"></param>
		public ItemGroupMatcher( Func<Item, EntityGroupDependencies, bool> matcherFun ) {
			this.MatcherFunc = matcherFun;
		}
	}

	/// <summary>
	/// An `EntityGroupMatcher` specifically for `NPC`s.
	/// </summary>
	public class NPCGroupMatcher : EntityGroupMatcher<NPC> {
		/// <summary></summary>
		/// <param name="matcherFun"></param>
		public NPCGroupMatcher( Func<NPC, EntityGroupDependencies, bool> matcherFun ) {
			this.MatcherFunc = matcherFun;
		}
	}

	/// An `EntityGroupMatcher` specifically for `Projectile`s.
	public class ProjectileGroupMatcher : EntityGroupMatcher<Projectile> {
		/// <summary></summary>
		/// <param name="matcherFun"></param>
		public ProjectileGroupMatcher( Func<Projectile, EntityGroupDependencies, bool> matcherFun ) {
			this.MatcherFunc = matcherFun;
		}
	}




	class EntityGroupMatcherDefinition<T> where T : Entity {
		public string GroupName;
		public string[] GroupDependencies;
		public EntityGroupMatcher<T> Matcher;

		public EntityGroupMatcherDefinition( string grpName, string[] grpDeps, EntityGroupMatcher<T> matcher ) {
			this.GroupName = grpName;
			this.GroupDependencies = grpDeps;
			this.Matcher = matcher;
		}
	}
}
