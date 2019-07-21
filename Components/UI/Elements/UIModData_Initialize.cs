using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.ModHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.IO;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.UI;


namespace HamstarHelpers.Components.UI.Elements {
	/// <summary>
	/// Defines a UI panel element specialized for rendering and displaying a mod's data (especially as a list item).
	/// </summary>
	public partial class UIModData : UIPanel {
		private bool InitializeMe( UITheme theme, int? idx, Mod mod, bool willDrawOwnHoverElements = true ) {
			var self = this;
			TmodFile modfile;
			if( !ReflectionHelpers.Get(mod, "File", out modfile) || modfile == null ) {
				LogHelpers.Warn( "Could not find mod "+mod.Name+"'s File" );
				return false;
			}

			this.Mod = mod;
			this.WillDrawOwnHoverElements = willDrawOwnHoverElements;

			this.Author = null;
			this.HomepageUrl = null;
			this.HasIconLoaded = false;
			this.LatestAvailableVersion = default( Version );

			if( ModHelpersMod.Instance.Config.IsCheckingModVersions ) {
				Services.Tml.BuildPropertiesViewer props = modfile != null ?
					Services.Tml.BuildPropertiesViewer.GetBuildPropertiesForModFile( modfile ) :
					(Services.Tml.BuildPropertiesViewer)null;
				if( props != null ) {
					this.Author = (string)props.GetField( "author" );
					this.HomepageUrl = (string)props.GetField( "homepage" );
				}
			}
			
			// Container

			this.SetPadding( 4f );
			this.Width.Set( 0f, 1f );
			this.Height.Set( 64, 0f );

			float titleOffset = 72f;

			// Mod index

			if( idx != null ) {
				var modIdxElem = new UIText( (int)idx + "" );
				modIdxElem.Left.Set( titleOffset, 0f );
				this.Append( (UIElement)modIdxElem );

				titleOffset += 16f;
			}

			// Mod title

			string modTitle = this.Mod.DisplayName + " " + this.Mod.Version.ToString();
			
			if( !String.IsNullOrEmpty(this.HomepageUrl) ) {
				this.TitleElem = new UIWebUrl( theme, modTitle, this.HomepageUrl, false );
			} else {
				this.TitleElem = new UIText( modTitle );
			}
			this.TitleElem.Left.Set( 88f, 0f );
			this.Append( (UIElement)this.TitleElem );

			// Mod author

			if( this.Author != null ) {
				this.AuthorElem = new UIText( "By: "+this.Author, 0.7f );
				this.AuthorElem.Top.Set( 20f, 0f );
				this.AuthorElem.Left.Set( titleOffset, 0f );
				this.Append( (UIElement)this.AuthorElem );
			}

			// Mod icon

			if( modfile != null && modfile.HasFile( "icon.png" ) ) {
				if( !Main.dedServ ) {   //...?
					try {
						var stream = new MemoryStream( modfile.GetBytes( "icon.png" ) );
						var iconTex = Texture2D.FromStream( Main.graphics.GraphicsDevice, stream );

						if( iconTex.Width == 80 && iconTex.Height == 80 ) {
							this.IconElem = new UIImage( iconTex );
							this.IconElem.Top.Set( -4f, 0f );
							this.IconElem.Left.Set( -4f, 0f );
							this.IconElem.MarginTop = -8f;
							this.IconElem.MarginLeft = -4f;
							this.IconElem.ImageScale = 0.7f;
							this.Append( this.IconElem );
						}
					} catch( Exception e ) {
						LogHelpers.Warn( e.ToString() );
					}
				}
			}

			return true;
		}
	}
}
