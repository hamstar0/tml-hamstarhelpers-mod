using HamstarHelpers.Helpers.DebugHelpers;
using System.Collections.Generic;
using System.Linq;


namespace HamstarHelpers.Components.CustomEntity.Templates {
	class CustomEntityTemplate {
		public string DisplayName;
		public int Width;
		public int Height;
		public IList<CustomEntityComponent> Components;


		////////////////

		internal CustomEntityTemplate( string name, int width, int height, IList<CustomEntityComponent> components ) {
			this.DisplayName = name;
			this.Width = width;
			this.Height = height;
			this.Components = components;
		}

		internal CustomEntityTemplate( CustomEntity ent )
			: this( ent.Core.DisplayName, ent.Core.width, ent.Core.height, ent.Components.Select( c => c.InternalClone() ).ToList() ) { }
	}
}
