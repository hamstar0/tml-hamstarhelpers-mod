using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;


namespace HamstarHelpers.Utilities.UI {
	[System.Obsolete( "use Components.UI.Elements.UIWebUrl.UIWebUrl", true )]
	public class UIWebUrl : UIElement {
		[System.Obsolete( "use Components.UI.Elements.UIWebUrl.DefaultColor", true )]
		public static Color DefaultColor {
			get { return Components.UI.Elements.UIWebUrl.DefaultColor; }
			set { Components.UI.Elements.UIWebUrl.DefaultColor = value; }
		}

		[System.Obsolete( "use Components.UI.Elements.UIWebUrl.DefaultLitColor", true )]
		public static Color DefaultLitColor {
			get { return Components.UI.Elements.UIWebUrl.DefaultLitColor; }
			set { Components.UI.Elements.UIWebUrl.DefaultLitColor = value; }
		}

		[System.Obsolete( "use Components.UI.Elements.UIWebUrl.DefaultVisitColor", true )]
		public static Color DefaultVisitColor {
			get { return Components.UI.Elements.UIWebUrl.DefaultVisitColor; }
			set { Components.UI.Elements.UIWebUrl.DefaultVisitColor = value; }
		}



		////////////////

		private UIHelpers.Elements.UIWebUrl TrueElement;

		public UIText TextElem { get { return this.TrueElement.TextElem; } }
		public UIText LineElem { get { return this.TrueElement.LineElem; } }

		public string Url { get { return this.TrueElement.Url; } }
		public bool WillDrawOwnHoverUrl { get { return this.TrueElement.WillDrawOwnHoverUrl; } }


		////////////////

		public UIWebUrl( string label, string url, bool hover_url=true, float scale=0.85f, bool large=false ) : base() {
			this.TrueElement = new UIHelpers.Elements.UIWebUrl( label, url, hover_url, scale, large );
		}


		////////////////
		
		public void DrawHoverEffects( SpriteBatch sb ) {
			this.TrueElement.DrawHoverEffects( sb );
		}
	}
}
