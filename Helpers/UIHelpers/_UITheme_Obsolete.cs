using HamstarHelpers.UIHelpers.Elements;
using Terraria.GameContent.UI.Elements;


namespace HamstarHelpers.UIHelpers {
	public partial class OldUITheme {
		[System.Obsolete( "use UITheme.ApplyListContainer", true )]
		public virtual void ApplyList( UIPanel panel ) {
			this.ApplyListContainer( panel );
		}
		[System.Obsolete( "use UITheme.ApplyList", true )]
		public void ApplyModList( UIPanel panel ) {
			this.ApplyList( panel );
		}
		[System.Obsolete( "use UITheme.ApplyListItem", true )]
		public void ApplyModListItem( UIModData panel ) {
			this.ApplyListItem( panel );
		}
		[System.Obsolete( "use UITheme.ApplyListItemLit", true )]
		public void ApplyModListItemLit( UIModData panel ) {
			this.ApplyListItemLit( panel );
		}
		[System.Obsolete( "use UITheme.ApplyListItemSelected", true )]
		public void ApplyModListItemSelected( UIModData panel ) {
			this.ApplyListItemSelected( panel );
		}
	}
}
