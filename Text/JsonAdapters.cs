
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Elegy.Text.JsonAdapters
{
	internal class GodotVector4Converter : JsonConverter<Vector4>
	{
		public override Vector4 Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		{
			Vector4 value = Vector4.Zero;

			reader.Read();
			value.X = reader.GetSingle();
			value.Y = reader.GetSingle();
			value.Z = reader.GetSingle();
			value.W = reader.GetSingle();
			reader.Read();

			return value;
		}

		public override void Write( Utf8JsonWriter writer, Vector4 value, JsonSerializerOptions options )
		{
			writer.WriteStartArray();
			writer.WriteNumberValue( value.X );
			writer.WriteNumberValue( value.Y );
			writer.WriteNumberValue( value.Z );
			writer.WriteNumberValue( value.W );
			writer.WriteEndArray();
		}
	}

	internal class GodotVector3Converter : JsonConverter<Vector3>
	{
		public override Vector3 Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		{
			Vector3 value = Vector3.Zero;

			reader.Read();
			value.X = reader.GetSingle();
			value.Y = reader.GetSingle();
			value.Z = reader.GetSingle();
			reader.Read();

			return value;
		}

		public override void Write( Utf8JsonWriter writer, Vector3 value, JsonSerializerOptions options )
		{
			writer.WriteStartArray();
			writer.WriteNumberValue( value.X );
			writer.WriteNumberValue( value.Y );
			writer.WriteNumberValue( value.Z );
			writer.WriteEndArray();
		}
	}

	internal class GodotVector2Converter : JsonConverter<Vector2>
	{
		public override Vector2 Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		{
			Vector2 value = Vector2.Zero;

			reader.Read();
			value.X = reader.GetSingle();
			value.Y = reader.GetSingle();
			reader.Read();

			return value;
		}

		public override void Write( Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options )
		{
			writer.WriteStartArray();
			writer.WriteNumberValue( value.X );
			writer.WriteNumberValue( value.Y );
			writer.WriteEndArray();
		}
	}

	internal class GodotAabbConverter : JsonConverter<Aabb>
	{
		public override Aabb Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		{
			Vector3 position = Vector3.Zero;
			Vector3 size = Vector3.Zero;

			reader.Read();
			position.X = reader.GetSingle();
			position.Y = reader.GetSingle();
			position.Z = reader.GetSingle();
			size.X = reader.GetSingle();
			size.Y = reader.GetSingle();
			size.Z = reader.GetSingle();
			reader.Read();

			Aabb value = new( position, size );
			return value;
		}

		public override void Write( Utf8JsonWriter writer, Aabb value, JsonSerializerOptions options )
		{
			writer.WriteStartArray();
			writer.WriteNumberValue( value.Position.X );
			writer.WriteNumberValue( value.Position.Y );
			writer.WriteNumberValue( value.Position.Z );
			writer.WriteNumberValue( value.Size.X );
			writer.WriteNumberValue( value.Size.Y );
			writer.WriteNumberValue( value.Size.Z );
			writer.WriteEndArray();
		}
	}

	internal class GodotRect2Converter : JsonConverter<Rect2>
	{
		public override Rect2 Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		{
			Vector2 position = Vector2.Zero;
			Vector2 size = Vector2.Zero;

			reader.Read();
			position.X = reader.GetSingle();
			position.Y = reader.GetSingle();
			size.X = reader.GetSingle();
			size.Y = reader.GetSingle();
			reader.Read();

			Rect2 value = new( position, size );
			return value;
		}

		public override void Write( Utf8JsonWriter writer, Rect2 value, JsonSerializerOptions options )
		{
			writer.WriteStartArray();
			writer.WriteNumberValue( value.Position.X );
			writer.WriteNumberValue( value.Position.Y );
			writer.WriteNumberValue( value.Size.X );
			writer.WriteNumberValue( value.Size.Y );
			writer.WriteEndArray();
		}
	}
}
