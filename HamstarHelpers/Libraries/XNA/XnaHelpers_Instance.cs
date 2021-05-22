using System;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET.Reflection;


namespace HamstarHelpers.Libraries.XNA {
	/// @private
	public partial class XNALibraries {
		private FieldInfo SpriteBatchBegunField = null;



		////////////////

		internal XNALibraries() {
			if( Main.dedServ || Main.netMode == NetmodeID.Server ) { return; }

			Type sbType = typeof(SpriteBatch);
			this.SpriteBatchBegunField = sbType.GetField( "inBeginEndPair", ReflectionLibraries.MostAccess );

			if( this.SpriteBatchBegunField == null ) {
				this.SpriteBatchBegunField = sbType.GetField( "_beginCalled", ReflectionLibraries.MostAccess );
			}
			if( this.SpriteBatchBegunField == null ) {
				this.SpriteBatchBegunField = sbType.GetField( "beginCalled", ReflectionLibraries.MostAccess );
			}
		}
	}
}
