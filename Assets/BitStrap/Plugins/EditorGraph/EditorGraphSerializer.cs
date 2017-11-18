using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters;

namespace BitStrap
{
	/// <summary>
	/// Handles serialization of a graph object.
	/// </summary>
	public static class EditorGraphSerializer
	{
		private static JsonSerializerSettings serializerSettings = new JsonSerializerSettings {
			TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
			TypeNameHandling = TypeNameHandling.Auto,
			DefaultValueHandling = DefaultValueHandling.Populate,
			Formatting = Formatting.None,
			ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
			PreserveReferencesHandling = PreserveReferencesHandling.Objects
		};

		/// <summary>
		/// Add a converter to serialize/deserialize nodes properties correctly.
		/// NOTE: only call this once for each converter!
		/// </summary>
		/// <param name="converter"></param>
		public static void AddSerializerConverter( JsonConverter converter )
		{
			serializerSettings.Converters.Add( converter );
		}

		/// <summary>
		/// Serialize graph like data.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string Serialize( object data )
		{
			return JsonConvert.SerializeObject( data, serializerSettings );
		}

		/// <summary>
		/// Deserialize graph like json.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="json"></param>
		/// <returns></returns>
		public static T Deserialize<T>( string json )
		{
			return JsonConvert.DeserializeObject<T>( json, serializerSettings );
		}
	}
}
