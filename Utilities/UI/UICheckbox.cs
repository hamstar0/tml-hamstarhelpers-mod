using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;
using Terraria;


namespace HamstarHelpers.Utilities.UI {
	[System.Obsolete( "use UIHelpers.UI.UICheckbox", true )]
	public class UICheckbox : UIText {
		[System.Obsolete( "use UIHelpers.UI.UICheckbox.CheckboxTexture", true )]
		public static Texture2D CheckboxTexture { get { return UIHelpers.Elements.UICheckbox.CheckboxTexture; } }

		[System.Obsolete( "use UIHelpers.UI.UICheckbox.CheckmarkTexture", true )]
		public static Texture2D CheckmarkTexture { get { return UIHelpers.Elements.UICheckbox.CheckmarkTexture; } }


		////////////////

		private UIHelpers.Elements.UICheckbox TrueElement;
		private object JuryriggedMutex = new Object();

		public event Action OnSelectedChanged {
			add {
				lock( this.JuryriggedMutex ) {
					this.TrueElement.OnSelectedChanged += value;
				}
			}
			remove {
				lock( this.JuryriggedMutex ) {
					this.TrueElement.OnSelectedChanged -= value;
				}
			}
		}
		public float Order {
			get { return this.TrueElement.Order; }
			set { this.TrueElement.Order = value; }
		}
		public bool IsClickable {
			get { return this.TrueElement.IsClickable; }
			set { this.TrueElement.IsClickable = value; }
		}
		public string Title {
			get { return this.TrueElement.Title; }
			set { this.TrueElement.Title = value; }
		}
		
		public bool Selected {
			get { return this.TrueElement.Selected; }
			set { this.TrueElement.Selected = value; }
		}



		public UICheckbox( string label, string title, bool is_clickable = true, float text_scale = 1, bool large = false ) : base( label, text_scale, large ) {
			if( Main.netMode != 2 && UICheckbox.CheckboxTexture == null || UICheckbox.CheckmarkTexture == null ) {
				UIHelpers.Elements.UICheckbox.LoadTextures();
			}

			this.TrueElement = new UIHelpers.Elements.UICheckbox( label, title, is_clickable, text_scale, large );
			this.Append( this.TrueElement );

			CalculatedStyle dim = this.TrueElement.GetDimensions();
			this.Width.Set( dim.Width, 0f );
			this.Height.Set( dim.Height, 0f );
		}

		public override int CompareTo( object obj ) {
			return this.TrueElement.CompareTo( obj );
		}
	}
}

