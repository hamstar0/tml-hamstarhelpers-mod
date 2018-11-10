using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;


namespace HamstarHelpers.Components.CustomEntity {
	class CustomEntityOverlay : Overlay {
		public Vector2 TargetPosition = Vector2.Zero;



		////////////////

		public CustomEntityOverlay( EffectPriority priority = EffectPriority.VeryHigh, RenderLayers layer = RenderLayers.Walls )
			: base( priority, layer ) {
		}


		////////////////

		public override void Activate( Vector2 position, params object[] args ) {
			this.TargetPosition = position;
			this.Mode = OverlayMode.FadeIn;
		}

		public override void Deactivate( params object[] args ) {
			this.Mode = OverlayMode.FadeOut;
		}

		public override bool IsVisible() {
			return true;
		}


		////////////////

		public override void Draw( SpriteBatch sb ) {
			ModHelpersMod.Instance?.CustomEntMngr?.PreDrawAll( sb );
			//sb.Draw( this._texture.Value, new Rectangle( 0, 0, Main.screenWidth, Main.screenHeight ), Main.bgColor );
		}

		public override void Update( GameTime _ ) { }
	}
}
