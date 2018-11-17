using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Reflection;


namespace HamstarHelpers.Components.Network.Data {
	public class PacketProtocolDataConstructorLock {
		internal Type Context { get; private set; }


		internal PacketProtocolDataConstructorLock( Type context ) {
			this.Context = context;
		}
	}




	public abstract partial class PacketProtocolData {
		protected abstract class Factory<T> where T : PacketProtocolData {
			public Factory( out T data ) {
				Type init_type = this.GetType();
				Type my_type = init_type.DeclaringType;

				data = (T)Activator.CreateInstance( my_type,
					BindingFlags.Instance | BindingFlags.NonPublic,
					null,
					new object[] { new PacketProtocolDataConstructorLock( init_type ) },
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

		internal static T CreateRawAsContext<T>() where T : PacketProtocolData {
			Type mytype = typeof(T);
			return (T)PacketProtocolData.CreateRaw( mytype, mytype );
		}

		internal static PacketProtocolData CreateRaw( Type data_type ) {
			return PacketProtocolData.CreateRaw( typeof(PacketProtocolData), data_type );
		}

		private static PacketProtocolData CreateRaw( Type context_type, Type data_type ) {
			return (PacketProtocolData)Activator.CreateInstance( data_type,
				BindingFlags.Instance | BindingFlags.NonPublic,
				null,
				new object[] { new PacketProtocolDataConstructorLock( context_type ) },
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
