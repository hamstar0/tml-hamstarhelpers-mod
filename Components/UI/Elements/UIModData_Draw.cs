using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using HamstarHelpers.Services.AnimatedColor;
using HamstarHelpers.Services.Promises;
using HamstarHelpers.Internals.WebRequests;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Diagnostics;
using System.IO;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;


namespace HamstarHelpers.Components.UI.Elements {
	public partial class UIModData : UIPanel {
		public override void Draw( SpriteBatch sb ) {
			base.Draw( sb );

			if( this.IsMouseHovering && this.WillDrawOwnHoverElements ) {
				this.DrawHoverEffects( sb );
			}
		}
		
		protected override void DrawSelf( SpriteBatch sb ) {
			base.DrawSelf( sb );

			if( this.LatestAvailableVersion > this.Mod.Version ) {
				Color color = AnimatedColors.Fire.CurrentColor;
				CalculatedStyle innerDim = base.GetInnerDimensions();
				Vector2 pos = innerDim.Position();
				pos.X += 128f;
			
				sb.DrawString( Main.fontDeathText, this.LatestAvailableVersion.ToString()+" Available", pos, color, 0f, default( Vector2 ), 1f, SpriteEffects.None, 1f );
			}

			//Promises.AddValidatedPromise<ModTagsPromiseArguments>( GetModTags.TagsReceivedPromiseValidator, ( args ) => {
			//	args.ModTags
			//	return false;
			//} );
		}


		public void DrawHoverEffects( SpriteBatch sb ) {
			if( this.TitleElem.IsMouseHovering ) {
				if( this.TitleElem is UIWebUrl ) {
					var titleUrl = (UIWebUrl)this.TitleElem;

					if( !titleUrl.WillDrawOwnHoverUrl ) {
						titleUrl.DrawHoverEffects( sb );
					}
				}
			}
		}
	}
}
