using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Services.ModTagDefinitions;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.ModTags.Base.Manager {
	abstract partial class ModTagsManager {
		public ISet<string> GetTagsForcingGivenTag( string tagName ) {
			var forcingTags = new HashSet<String>();

			foreach( ModTagDefinition tagDef in this.MyTags ) {
				if( tagDef.Tag == tagName ) { continue; }

				if( tagDef.ForcesTags.Contains(tagName) ) {
					forcingTags.Add( tagDef.Tag );
				}
			}

			return forcingTags;
		}
	}
}
