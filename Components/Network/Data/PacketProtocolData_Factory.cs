using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.Reflection;


namespace HamstarHelpers.Components.Network.Data {
	public class PacketProtocolDataConstructorLock {
		internal Type FactoryType { get; private set; }


		internal PacketProtocolDataConstructorLock( Type factoryType ) {
			this.FactoryType = factoryType;
		}
	}




	public abstract partial class PacketProtocolData {
		protected abstract class Factory<T> where T : PacketProtocolData {
			protected abstract void Initialize( T data );

			public T Create() {
				Type dataType = typeof( T );
				Type factoryType = this.GetType();
				Type factoryContainerType = factoryType.DeclaringType;
				
				if( dataType != factoryContainerType ) {
					throw new NotImplementedException( "Invalid PacketProtocolData factory for class " + dataType.Name + "; expected " + factoryContainerType.Name );
				}

				T data = (T)Activator.CreateInstance( factoryContainerType,
					BindingFlags.Instance | BindingFlags.NonPublic,
					null,
					new object[] { new PacketProtocolDataConstructorLock( factoryType ) },
					null
				);
				this.Initialize( data );

				return data;
			}
		}



		////////////////

		/*private Type GetMainFactoryType<T>() where T : PacketProtocolData {
			Type factoryType = typeof( Factory<T> );
			Type[] nesteds = this.GetType().GetNestedTypes( BindingFlags.Static | BindingFlags.NonPublic );

			foreach( var nested in nesteds ) {
				if( nested.IsSubclassOf( factoryType ) && !nested.IsAbstract ) {
					return nested;
				}
			}

			return null;
		}

		protected Type GetMainFactoryType() {
			return this.GetMainFactoryType<PacketProtocolData>();
		}*/


		////////////////
		
		internal static PacketProtocolData CreateRaw( Type dataType ) {
			if( !dataType.IsSubclassOf(typeof(PacketProtocolData)) ) {
				throw new NotImplementedException("Not a PacketProtocolData subclass.");
			}

			var data = (PacketProtocolData)Activator.CreateInstance( dataType,
				BindingFlags.Instance | BindingFlags.NonPublic,
				null,
				new object[] { new PacketProtocolDataConstructorLock( typeof( PacketProtocolData ) ) },
				null
			);

			return data;
		}
	}
}
