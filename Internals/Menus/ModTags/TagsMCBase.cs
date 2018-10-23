using HamstarHelpers.Components.UI.Menu;
using HamstarHelpers.Components.UI.Menus;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Internals.Menus.ModTags.UI;
using HamstarHelpers.Services.Menus;
using System.Collections.Generic;


namespace HamstarHelpers.Internals.Menus.ModTags {
	abstract partial class TagsMenuContextBase : SessionMenuContext {
		internal readonly IDictionary<string, UITagButton> TagButtons = new Dictionary<string, UITagButton>();

		private readonly bool CanDisableTags;



		////////////////

		protected TagsMenuContextBase( bool can_disable_tags ) : base( true, true ) {
			this.CanDisableTags = can_disable_tags;

			int i = 0;
			
			foreach( var kv in TagsMenuContextBase.Tags ) {
				string tag_text = kv.Key;
				string tag_desc = kv.Value;
				
				this.TagButtons[tag_text] = new UITagButton( this, i, tag_text, tag_desc, this.CanDisableTags );
				i++;
			}
		}

		public override void OnContexualize( string ui_class_name, string context_name ) {
			base.OnContexualize( ui_class_name, context_name );

			int i = 0;
			
			foreach( UITagButton button in this.TagButtons.Values ) {
				var button_widget_ctx = new WidgetMenuContext( button, false );

				MenuContextService.AddMenuContext( ui_class_name, context_name + " Tag " + i, button_widget_ctx );
				i++;
			}
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
