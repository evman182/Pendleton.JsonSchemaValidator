using Manatee.Json;
using Manatee.Json.Serialization;

namespace Pendleton.JsonSchemaValidator.Tests.TestSuite
{
	public class SchemaTest : IJsonSerializable
	{
		public JsonValue Data { get; set; }
		public bool Valid { get; set; }

		public void FromJson(JsonValue json, JsonSerializer serializer)
		{
			var obj = json.Object;
			Data = obj["data"];
			Valid = obj["valid"].Boolean;
		}
		public JsonValue ToJson(JsonSerializer serializer)
		{
			var obj = new JsonObject
				{
					["data"] = Data,
					["valid"] = Valid
				};

			return obj;
		}
	}
}