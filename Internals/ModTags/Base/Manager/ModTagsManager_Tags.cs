using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.ModTags.Base.Manager {
	abstract partial class ModTagsManager {
		public ISet<string> GetTagsForcingGivenTag( string tagName ) {
			var forcingTags = new HashSet<String>();

			foreach( TagDefinition tagDef in this.MyTags ) {
				if( tagDef.Tag == tagName ) { continue; }

				if( tagDef.ForcesTags.Contains(tagName) ) {
					forcingTags.Add( tagDef.Tag );
				}
			}

			return forcingTags;
		}
	}
}
