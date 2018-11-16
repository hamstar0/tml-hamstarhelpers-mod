using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Reflection;


namespace HamstarHelpers.Components.Network.Data {
	public class PacketProtocolDataConstructorLock {
		internal PacketProtocolDataConstructorLock() { }
	}




	public abstract partial class PacketProtocolData {
		protected abstract class Factory<T> where T : PacketProtocolData {
			public Factory( out T data ) {
				Type inittype = this.GetType();
				Type mytype = inittype.DeclaringType;
				
				data = (T)Activator.CreateInstance( mytype,
					BindingFlags.Instance | BindingFlags.NonPublic,
					null,
					new object[] { ModHelpersMod.Instance.PacketProtocolCtorLock },
					null
				);
			}
		}

		////////////////

		private Type GetMainFactoryType<T>() where T : PacketProtocolData {
			Type factory_type = typeof( Factory<T> );
			Type[] nesteds = this.GetType().GetNestedTypes( BindingFlags.Static | BindingFlags.NonPublic );

			foreach( var nested in nesteds ) {
				if( nested.IsSubclassOf( factory_type ) && !nested.IsAbstract ) {
					return nested;
				}
			}

			return null;
		}
		protected Type GetMainFactoryType() {
			return this.GetMainFactoryType<PacketProtocolData>();
		}


		////////////////

		internal static PacketProtocolData CreateRaw( Type mytype ) {
			return (PacketProtocolData)Activator.CreateInstance( mytype,
				BindingFlags.Instance | BindingFlags.NonPublic,
				null,
				new object[] { ModHelpersMod.Instance.PacketProtocolCtorLock },
				null
			);
		}


		////////////////

		protected PacketProtocolData( PacketProtocolDataConstructorLock ctor_lock ) {
			if( ctor_lock == null ) {
				throw new NotImplementedException();
			}
		}
	}
}
