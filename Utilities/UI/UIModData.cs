using HamstarHelpers.UIHelpers;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;


namespace HamstarHelpers.Utilities.UI {
	[System.Obsolete( "use UIHelpers.UI.UIModData", true )]
	public class UIModData : UIElement {
		private UIHelpers.Elements.UIModData TrueElement;

		public Mod Mod { get { return this.TrueElement.Mod; } }
		public string Author { get { return this.TrueElement.Author; } }
		public string HomepageUrl { get { return this.TrueElement.HomepageUrl; } }

		public UIImage IconElem { get { return this.TrueElement.IconElem; } }
		public UIElement TitleElem { get { return this.TrueElement.TitleElem; } }
		public UIElement AuthorElem { get { return this.TrueElement.AuthorElem; } }
		public UITextPanel<string> ConfigButton { get { return this.TrueElement.ConfigButton; } }

		public bool HasIconLoaded { get { return this.TrueElement.HasIconLoaded; } }
		public bool WillDrawOwnHoverElements { get { return this.TrueElement.WillDrawOwnHoverElements; } }


		////////////////

		public UIModData( UITheme theme, Mod mod, bool will_draw_own_hover_elements=true ) : base() {
			this.TrueElement = new UIHelpers.Elements.UIModData( theme, mod, will_draw_own_hover_elements );
			this.Append( this.TrueElement );

			CalculatedStyle dim = this.TrueElement.GetDimensions();
			this.Width.Set( dim.Width, 0f );
			this.Height.Set( dim.Height, 0f );
		}
	}
}
