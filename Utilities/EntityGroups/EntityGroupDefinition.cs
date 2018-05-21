using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Terraria;


namespace HamstarHelpers.Utilities.EntityGroups {
	abstract public class EntityGroupDefinition<T> where T : Entity {
		public IDictionary<string, bool> BoolFields { get; private set; }
		public IDictionary<string, IDictionary<double, double>> NumFields { get; private set; }
		public IDictionary<string, Regex> StringFields { get; private set; }


		////////////////

		abstract public T[] GetPool();


		////////////////

		private object GetTypeNameAndValue( string member_name, T entity, out string type_name ) {
			Type mytype = typeof( T );
			FieldInfo field = mytype.GetField( member_name );
			object value = null;
			
			if( field != null ) {
				type_name = field.GetType().Name;
				value = field.GetValue( entity );
			} else {
				PropertyInfo prop = mytype.GetProperty( member_name );
				if( prop == null ) {
					throw new Exception( "Invalid field or property " + member_name );
				}

				type_name = prop.GetType().Name;
				value = prop.GetValue( entity );
			}

			return value;
		}


		////////////////

		public bool ValidateBoolFields( T entity ) {
			Type mytype = typeof( T );

			foreach( var kv in this.BoolFields ) {
				string field_type_name;
				object raw_field_value = this.GetTypeNameAndValue( kv.Key, entity, out field_type_name );

				if( field_type_name != "Boolean" ) {
					throw new InvalidCastException();
				}

				if( (bool)raw_field_value != kv.Value ) {
					return false;
				}
			}

			return true;
		}

		public bool ValidateNumFields( T entity ) {
			Type mytype = typeof( T );

			foreach( var kv in this.NumFields ) {
				string field_type_name;
				object raw_field_value = this.GetTypeNameAndValue( kv.Key, entity, out field_type_name );
				double field_val;

				switch( field_type_name ) {
				case "Byte":
					field_val = (double)( (byte)raw_field_value );
					break;
				case "SByte":
					field_val = (double)( (sbyte)raw_field_value );
					break;
				case "Int16":
					field_val = (double)( (short)raw_field_value );
					break;
				case "UInt16":
					field_val = (double)( (ushort)raw_field_value );
					break;
				case "Int32":
					field_val = (double)( (int)raw_field_value );
					break;
				case "UInt32":
					field_val = (double)( (uint)raw_field_value );
					break;
				case "Int64":
					field_val = (double)( (long)raw_field_value );
					break;
				case "UInt64":
					field_val = (double)( (ulong)raw_field_value );
					break;
				case "Single":
					field_val = (double)( (float)raw_field_value );
					break;
				case "Double":
					field_val = (double)raw_field_value;
					break;
				default:
					throw new InvalidCastException();
				}

				foreach( KeyValuePair<double, double> match_val in kv.Value ) {
					if( field_val < match_val.Key || field_val >= match_val.Value ) {
						return false;
					}
				}
			}

			return true;
		}

		public bool ValidateStringFields( T entity ) {
			Type mytype = typeof( T );

			foreach( var kv in this.StringFields ) {
				string field_type_name;
				object raw_field_value = this.GetTypeNameAndValue( kv.Key, entity, out field_type_name );

				if( field_type_name != "String" ) {
					throw new InvalidCastException();
				}

				if( !kv.Value.IsMatch( (string)raw_field_value ) ) {
					return false;
				}
			}

			return true;
		}


		////////////////

		public virtual IList<T> GetGroup() {
			Type mytype = typeof( T );
			T[] pool = this.GetPool();
			IList<T> group = new List<T>();

			foreach( T ent in pool ) {
				if( !this.ValidateBoolFields( ent ) ) { continue; }
				if( !this.ValidateNumFields( ent ) ) { continue; }
				if( !this.ValidateStringFields( ent ) ) { continue; }

				group.Add( ent );
			}

			return group;
		}
	}
}
