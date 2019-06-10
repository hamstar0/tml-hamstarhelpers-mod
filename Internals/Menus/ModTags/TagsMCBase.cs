using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Components.UI.Menus;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Internals.Menus.ModTags.UI;
using HamstarHelpers.Services.Menus;
using System;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.Menus.ModTags {
	abstract partial class TagsMenuContextBase : SessionMenuContext {
		internal readonly IDictionary<string, UITagButton> TagButtons = new Dictionary<string, UITagButton>();

		private readonly bool CanDisableTags;



		////////////////

		protected TagsMenuContextBase( bool canDisableTags ) : base( true, true ) {
			this.CanDisableTags = canDisableTags;

			for( int i=0; i<TagsMenuContextBase.Tags.Length; i++ ) {
				string tagText = TagsMenuContextBase.Tags[i].Item1;
				string tagDesc = TagsMenuContextBase.Tags[i].Item2;
				
				this.TagButtons[ tagText ] = new UITagButton( this, i, tagText, tagDesc, this.CanDisableTags );
			}
		}

		public override void OnContexualize( string uiClassName, string contextName ) {
			base.OnContexualize( uiClassName, contextName );

			int i = 0;
			
			foreach( UITagButton button in this.TagButtons.Values ) {
				var buttonWidgetCtx = new WidgetMenuContext( button, false );

				MenuContextService.AddMenuContext( uiClassName, contextName + " Tag " + i, buttonWidgetCtx );
				i++;
			}
		}


		////////////////

		public abstract void OnTagStateChange( UITagButton tagButton );


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
