using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.Menus.ModTags.UI;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.Menus.ModTags {
	abstract partial class TagsMenuContextBase : MenuContextBase {
		internal IDictionary<string, UITagButton> TagButtons = new Dictionary<string, UITagButton>();

		private bool CanDisableTags;



		////////////////

		protected TagsMenuContextBase( bool can_disable_tags ) : base( true ) {
			this.CanDisableTags = can_disable_tags;

			this.InitializeTagButtons();
		}


		////////////////
		
		public abstract void OnTagStateChange( UITagButton tag_button );


		public ISet<string> GetTagsOfState( int state ) {
			ISet<string> tags = new HashSet<string>();

			foreach( var kv in this.TagButtons ) {
				if( kv.Value.TagState == state ) {
					tags.Add( kv.Key );
				}
			}
			return tags;
		}


		////////////////

		public void EnableTagButtons() {
			foreach( var kv in this.TagButtons ) {
				kv.Value.Enable();
			}
		}

		public void DisableTagButtons() {
			foreach( var kv in this.TagButtons ) {
				kv.Value.Disable();
			}
		}

		////////////////

		public void ResetTagButtons() {
			foreach( var kv in this.TagButtons ) {
				kv.Value.SetTagState( 0 );
			}
		}
	}
}
