using HamstarHelpers.Components.Errors;
using HamstarHelpers.Components.Network.Data;
using HamstarHelpers.Helpers.DebugHelpers;


namespace HamstarHelpers.Components.CustomEntity {
	public abstract partial class CustomEntity : PacketProtocolData {
		protected void ClearComponentCache() {
			this.ComponentsByTypeName.Clear();
			this.AllComponentsByTypeName.Clear();
		}


		////////////////

		public T GetComponentByType<T>() where T : CustomEntityComponent {
			if( this.ComponentsByTypeName.Count != this.Components.Count ) {
				this.RefreshComponentTypeNames();
			}

			int idx;

			if( !this.AllComponentsByTypeName.TryGetValue( typeof(T).Name, out idx ) ) {
				return null;
			}
			return (T)this.Components[ idx ];
		}

		public CustomEntityComponent GetComponentByName( string name ) {
			if( this.ComponentsByTypeName.Count != this.Components.Count ) {
				this.RefreshComponentTypeNames();
			}

			int idx;

			if( !this.AllComponentsByTypeName.TryGetValue( name, out idx ) ) {
				return null;
			}
			return this.Components[idx];
		}
	}
}
