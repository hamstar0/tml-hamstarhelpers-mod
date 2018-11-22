using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Reflection;


namespace HamstarHelpers.Components.Network.Data {
	public class PacketProtocolDataConstructorLock {
		internal Type FactoryType { get; private set; }


		internal PacketProtocolDataConstructorLock( Type factory_type ) {
			this.FactoryType = factory_type;
		}
	}




	public abstract partial class PacketProtocolData {
		protected abstract class Factory<T> where T : PacketProtocolData {
			public abstract void Initialize( T data );

			public T Create() {
				Type data_type = typeof( T );
				Type factory_type = this.GetType();
				Type factory_container_type = factory_type.DeclaringType;
				
				if( data_type != factory_container_type ) {
					throw new NotImplementedException( "Invalid PacketProtocolData factory for class " + data_type.Name + "; expected " + factory_container_type.Name );
				}

				T data = (T)Activator.CreateInstance( factory_container_type,
					BindingFlags.Instance | BindingFlags.NonPublic,
					null,
					new object[] { new PacketProtocolDataConstructorLock( factory_type ) },
					null
				);
				this.Initialize( data );

				return data;
			}
		}



		////////////////

		/*private Type GetMainFactoryType<T>() where T : PacketProtocolData {
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
		}*/


		////////////////
		
		internal static PacketProtocolData CreateRaw( Type data_type ) {
			if( !data_type.IsSubclassOf(typeof(PacketProtocolData)) ) {
				throw new NotImplementedException("Not a PacketProtocolData subclass.");
			}

			var data = (PacketProtocolData)Activator.CreateInstance( data_type,
				BindingFlags.Instance | BindingFlags.NonPublic,
				null,
				new object[] { new PacketProtocolDataConstructorLock( typeof( PacketProtocolData ) ) },
				null
			);

			return data;
		}


		////////////////

		protected PacketProtocolData( PacketProtocolDataConstructorLock ctor_lock ) {
			if( ctor_lock == null ) {
				throw new NotImplementedException( "Invalid " + this.GetType().Name + ": Must be factory generated or cloned." );
			}
		}
	}
}
