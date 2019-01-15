using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;
using System.Reflection;


namespace HamstarHelpers.Components.Network.Data {
	public class PacketProtocolDataConstructorLock {
		internal PacketProtocolDataConstructorLock() { }
	}




	public abstract partial class PacketProtocolData {
		[Obsolete("use PacketDataProtocol.CreateDefault instead.", true)]
		protected abstract class Factory<T> where T : PacketProtocolData {
			protected abstract void Initialize( T data );

			public T Create() {
				Type dataType = typeof( T );
				Type factoryType = this.GetType();
				Type factoryContainerType = factoryType.DeclaringType;
				
				if( dataType != factoryContainerType ) {
					throw new NotImplementedException( "Invalid PacketProtocolData factory for class " + dataType.Name + "; expected a " + factoryContainerType.Name );
				}

				T data = (T)Activator.CreateInstance( factoryContainerType,
					BindingFlags.Instance | BindingFlags.NonPublic,
					null,
					new object[] { new PacketProtocolDataConstructorLock() },
					null
				);
				this.Initialize( data );

				data.OnInitialize();

				return data;
			}
		}



		////////////////

		public static T CreateDefault<T>( object factory ) where T : PacketProtocolData {
			return (T)PacketProtocolData.CreateDefault( typeof(T), factory );
		}

		public static PacketProtocolData CreateDefault( Type productType, object factory ) {
			PacketProtocolData data = PacketProtocolData.CreateRawUninitialized( productType );

			if( data.MyFactoryType != factory.GetType() ) {
				throw new NotImplementedException( "Incorrect factory type: Found: "+factory.GetType().Name+", expected "+data.MyFactoryType.Name );
			}

			FieldInfo[] fields = factory.GetType().GetFields( BindingFlags.Public );

			foreach( FieldInfo field in fields ) {
				var value = field.GetValue( factory );
				string fieldName = field.Name;

				FieldInfo dataField = data.GetType().GetField( fieldName );
				dataField.SetValue( data, value, ReflectionHelpers.MostAccess, null, null );
			}

			data.OnFactoryCreate( factory );
			data.OnInitialize();

			return data;
		}

		////////////////

		internal static PacketProtocolData CreateRawUninitialized( Type dataType ) {
			if( !dataType.IsSubclassOf(typeof(PacketProtocolData)) ) {
				throw new HamstarException("Not a PacketProtocolData subclass.");
			}

			var data = (PacketProtocolData)Activator.CreateInstance( dataType,
				BindingFlags.Instance | BindingFlags.NonPublic,
				null,
				new object[] { new PacketProtocolDataConstructorLock() },
				null
			);

			return data;
		}



		////////////////

		protected virtual Tuple<object, Type> _MyFactoryType { get; }
		protected Type MyFactoryType {
			get {
				var myobj = this._MyFactoryType.Item1;

				if( this._MyFactoryType != null && myobj.GetType() != this.GetType() ) {
					throw new NotImplementedException(
						"Incorrect factory product type: Found " + myobj.GetType().Name + ", expected " + this.GetType().Name
					);
				}
				return this._MyFactoryType.Item2;
			}
		}


		////////////////

		protected virtual void OnFactoryCreate( object factory ) { }

		protected abstract void OnInitialize();

		internal void InternalOnInitialize() { this.OnInitialize(); }
	}
}
