using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers.Reflection;
using System;
using System.Reflection;
using Terraria;


namespace HamstarHelpers.Helpers.XnaHelpers {
	public partial class XnaHelpers {
		private FieldInfo SpriteBatchBegunField = null;



		////////////////

		internal XnaHelpers() {
			if( Main.dedServ || Main.netMode == 2 ) { return; }

			Type sbType = Main.spriteBatch.GetType();
			this.SpriteBatchBegunField = sbType.GetField( "inBeginEndPair", ReflectionHelpers.MostAccess );

			if( this.SpriteBatchBegunField == null ) {
				this.SpriteBatchBegunField = sbType.GetField( "_beginCalled", ReflectionHelpers.MostAccess );
			}
		}
	}
}
