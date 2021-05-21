using System;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;


namespace HamstarHelpers.Helpers.XNA {
	/// @private
	public partial class XNAHelpers {
		private FieldInfo SpriteBatchBegunField = null;



		////////////////

		internal XNAHelpers() {
			if( Main.dedServ || Main.netMode == NetmodeID.Server ) { return; }

			Type sbType = typeof(SpriteBatch);
			this.SpriteBatchBegunField = sbType.GetField( "inBeginEndPair", ReflectionHelpers.MostAccess );

			if( this.SpriteBatchBegunField == null ) {
				this.SpriteBatchBegunField = sbType.GetField( "_beginCalled", ReflectionHelpers.MostAccess );
			}
			if( this.SpriteBatchBegunField == null ) {
				this.SpriteBatchBegunField = sbType.GetField( "beginCalled", ReflectionHelpers.MostAccess );
			}
		}
	}
}
