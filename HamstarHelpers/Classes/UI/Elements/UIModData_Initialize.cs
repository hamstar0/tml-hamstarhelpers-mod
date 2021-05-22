using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET.Reflection;
using HamstarHelpers.Services.TML;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.UI;


namespace HamstarHelpers.Classes.UI.Elements {
	/// <summary>
	/// Defines a UI panel element specialized for rendering and displaying a mod's data (especially as a list item).
	/// </summary>
	public partial class UIModData : UIThemedPanel {
		private bool InitializeMe( int? idx, Mod mod, bool willDrawOwnHoverElements = true ) {
			var self = this;
			TmodFile modfile;
			if( !ReflectionLibraries.Get(mod, "File", out modfile) || modfile == null ) {
				LogLibraries.Warn( "Could not find mod "+mod.Name+"'s File" );
				return false;
			}

			this.Mod = mod;
			this.WillDrawOwnHoverElements = willDrawOwnHoverElements;

			this.Author = null;
			this.HomepageUrl = null;
			this.HasIconLoaded = false;
			this.LatestAvailableVersion = default( Version );

			if( !ModHelpersConfig.Instance.DisableModMenuUpdates ) {
				BuildPropertiesViewer props = modfile != null ?
					BuildPropertiesViewer.GetBuildPropertiesForModFile( modfile ) :
					(BuildPropertiesViewer)null;
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

			if( idx.HasValue ) {
				this.DisplayIndex = new UIText( (int)idx + "" );
				this.DisplayIndex.Left.Set( titleOffset, 0f );
				this.Append( (UIElement)this.DisplayIndex );

				titleOffset += 16f;
			}

			// Mod title

			string modTitle = this.Mod.DisplayName + " " + this.Mod.Version.ToString();
			
			if( !String.IsNullOrEmpty(this.HomepageUrl) ) {
				this.TitleElem = new UIWebUrl( this.Theme, modTitle, this.HomepageUrl, false );
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
						LogLibraries.Warn( e.ToString() );
					}
				}
			}

			return true;
		}
	}
}
