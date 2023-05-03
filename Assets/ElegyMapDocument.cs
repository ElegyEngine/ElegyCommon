// SPDX-FileCopyrightText: 2023 Admer Šuko
// SPDX-License-Identifier: MIT

using Elegy.Assets.ElegyMapData;
using Elegy.Text;
using Elegy.Utilities;

namespace Elegy.Assets
{
	/// <summary>
	/// Standard Elegy level format.
	/// </summary>
	public class ElegyMapDocument
	{
		//// <summary>
		//// Visibility data.
		//// </summary>
		//public List<MapLeaf> VisibilityLeaves { get; set; } = new();

		/// <summary>
		/// IDs of world meshes.
		/// </summary>
		public List<int> WorldMeshIds { get; set; } = new();

		/// <summary>
		/// Map entities.
		/// </summary>
		public List<Entity> Entities { get; set; } = new();

		/// <summary>
		/// Collision meshes for collision detection.
		/// </summary>
		public List<CollisionMesh> CollisionMeshes { get; set; } = new();

		/// <summary>
		/// Occluder meshes for real-time dynamic occlusion culling.
		/// </summary>
		public List<OccluderMesh> OccluderMeshes { get; set; } = new();

		/// <summary>
		/// Visual renderable meshes.
		/// </summary>
		public List<RenderMesh> RenderMeshes { get; set; } = new();

		/// <summary>
		/// Writes the contents of this <see cref="ElegyMapDocument"/> to a file.
		/// </summary>
		public void WriteToFile( string path )
		{
			using var file = File.CreateText( path );

			// TODO: Utility to write & parse in this format
			// Preferably JSON5 so it's human-friendly and all
			// Refer to: https://github.com/ElegyEngine/ElegyCommon/issues/1

			file.WriteLine( "ElegyLevelFile" );
			file.WriteLine( "{" );

			file.WriteLine( "	WorldMeshIds" );
			file.WriteLine( "	{" );
			file.Write( "		" );
			for ( int i = 0; i < WorldMeshIds.Count; i++ )
			{
				file.Write( $" {WorldMeshIds[i]}" );
			}
			file.WriteLine();
			file.WriteLine( "	}" );

			// Entities
			file.WriteLine( "	Entities" );
			file.WriteLine( "	{" );
			foreach ( var entity in Entities )
			{
				file.WriteLine( "		Entity" );
				file.WriteLine( "		{" );
				file.WriteLine( $"			RenderMeshId {entity.RenderMeshId}" );
				file.WriteLine( $"			CollisionMeshId {entity.CollisionMeshId}" );
				file.WriteLine( $"			OccluderMeshId {entity.OccluderMeshId}" );
				file.WriteLine( "			Attributes" );
				file.WriteLine( "			{" );
				foreach ( var attribute in entity.Attributes )
				{
					file.WriteLine( $"				{attribute.Key} \"{attribute.Value}\"" );
				}
				file.WriteLine( "			}" );
				file.WriteLine( "		}" );
			}
			file.WriteLine( "	}" );

			// Collision meshes
			file.WriteLine( "	CollisionMeshes" );
			file.WriteLine( "	{" );
			foreach ( var mesh in CollisionMeshes )
			{
				file.WriteLine( "		CollisionMesh" );
				file.WriteLine( "		{" );
				file.WriteLine( "			Meshlets" );
				file.WriteLine( "			{" );
				foreach ( var meshlet in mesh.Meshlets )
				{
					file.WriteLine( "				CollisionMeshlet" );
					file.WriteLine( "				{" );
					file.WriteLine( $"					MaterialName \"{meshlet.MaterialName}\"" );
					file.WriteLine( "					Positions" );
					file.WriteLine( "					{" );
					for ( int i = 0; i < meshlet.Positions.Count; i++ )
					{
						file.WriteLine( $"						( {meshlet.Positions[i].X} {meshlet.Positions[i].Y} {meshlet.Positions[i].Z} )" );
					}
					file.WriteLine( "					}" );
					file.WriteLine( "				}" );
				}
				file.WriteLine( "			}" );
				file.WriteLine( "		}" );
			}
			file.WriteLine( "	}" );

			// Occluder meshes
			file.WriteLine( "	OccluderMeshes" );
			file.WriteLine( "	{" );
			foreach ( var mesh in OccluderMeshes )
			{
				file.WriteLine( "		OccluderMesh" );
				file.WriteLine( "		{" );
				file.WriteLine( "			Positions" );
				file.WriteLine( "			{" );
				for ( int i = 0; i < mesh.Positions.Count; i++ )
				{
					file.WriteLine( $"				( {mesh.Positions[i].X} {mesh.Positions[i].Y} {mesh.Positions[i].Z} )" );
				}
				file.WriteLine( "			}" );
				file.WriteLine( "			Indices" );
				file.WriteLine( "			{" );
				file.Write( "				" );
				for ( int i = 0; i < mesh.Indices.Count; i++ )
				{
					file.Write( $" {mesh.Indices[i]}" );
				}
				file.WriteLine();
				file.WriteLine( "			}" );
				file.WriteLine( "		}" );
			}
			file.WriteLine( "	}" );

			// Render meshes
			file.WriteLine( "	RenderMeshes" );
			file.WriteLine( "	{" );
			foreach ( var mesh in RenderMeshes )
			{
				file.WriteLine( "		RenderMesh" );
				file.WriteLine( "		{" );
				file.WriteLine( "			Surfaces" );
				file.WriteLine( "			{" );
				foreach ( var surface in mesh.Surfaces )
				{
					Vector3 boxPosition = surface.BoundingBox.Position;
					Vector3 boxSize = surface.BoundingBox.Size;

					file.WriteLine( "				RenderSurface" );
					file.WriteLine( "				{" );
					file.WriteLine( $"					BoundingBox ( {boxPosition.X} {boxPosition.Y} {boxPosition.Z} ) ( {boxSize.X} {boxSize.Y} {boxSize.Z} )" );
					file.WriteLine( $"					VertexCount {surface.VertexCount}" );
					file.WriteLine( $"					Material \"{surface.Material}\"" );
					file.WriteLine( $"					LightmapTexture \"{surface.LightmapTexture}\"" );
					file.WriteLine( "					Positions" );
					file.WriteLine( "					{" );
					foreach ( var p in surface.Positions )
					{
						file.WriteLine( $"						( {p.X} {p.Y} {p.Z} )" );
					}
					file.WriteLine( "					}" );
					file.WriteLine( "					Normals" );
					file.WriteLine( "					{" );
					foreach ( var p in surface.Normals )
					{
						file.WriteLine( $"						( {p.X} {p.Y} {p.Z} )" );
					}
					file.WriteLine( "					}" );
					file.WriteLine( "					Uvs" );
					file.WriteLine( "					{" );
					foreach ( var p in surface.Uvs )
					{
						file.WriteLine( $"						( {p.X} {p.Y} )" );
					}
					file.WriteLine( "					}" );
					file.WriteLine( "					LightmapUvs" );
					file.WriteLine( "					{" );
					foreach ( var p in surface.LightmapUvs )
					{
						file.WriteLine( $"						( {p.X} {p.Y} )" );
					}
					file.WriteLine( "					}" );
					file.WriteLine( "					Colours" );
					file.WriteLine( "					{" );
					foreach ( var p in surface.Colours )
					{
						file.WriteLine( $"						( {p.X} {p.Y} {p.Z} {p.W} )" );
					}
					file.WriteLine( "					}" );
					file.WriteLine( "					Indices" );
					file.WriteLine( "					{" );
					file.Write( "						" );
					foreach ( var i in surface.Indices )
					{
						file.Write( $" {i}" );
					}
					file.WriteLine();
					file.WriteLine( "					}" );
					file.WriteLine( "				}" );
				}
				file.WriteLine( "			}" );
				file.WriteLine( "		}" );
			}
			file.WriteLine( "	}" );

			file.WriteLine( "}" );
		}

		// Everything below is throwaway code, hopefully will be replaced in June

		/// <summary>
		/// Loads an <see cref="ElegyMapDocument"/> from a file.
		/// </summary>
		/// <exception cref="Exception">File isn't an ELF</exception>
		public static ElegyMapDocument LoadFromFile( string path )
		{
			Lexer lexer = new( File.ReadAllText( path ) );

			if ( !lexer.Expect( "ElegyLevelFile", true ) )
			{
				throw new Exception( $"'s{path}' is not a valid Elegy level file" );
			}

			if ( !lexer.Expect( "{", true ) )
			{
				throw new Exception( $"Expected '{{' on the 2nd line of '{path}'" );
			}

			ElegyMapDocument map = new();

			while ( true )
			{
				string token = lexer.Next();

				switch ( token )
				{
					case "}": break;
					case "WorldMeshIds": map.WorldMeshIds = ParseWorldMeshIds( lexer ); break;
					case "Entities": map.Entities = ParseEntities( lexer ); break;
					case "CollisionMeshes": map.CollisionMeshes = ParseCollisionMeshes( lexer ); break;
					case "OccluderMeshes": break; // Occluder meshes can be unsupported for now
					case "RenderMeshes": map.RenderMeshes = ParseRenderMeshes( lexer ); break;
					default: throw new Exception( $"Unknown token '{token}' {lexer.GetLineInfo()}" );
				}

				if ( token == "}" || lexer.IsEnd() )
				{
					break;
				}
			}

			return map;
		}

		private static List<int> ParseWorldMeshIds( Lexer lexer )
		{
			if ( !lexer.Expect( "{", true ) )
			{
				throw new Exception( $"Expected '{{' at {lexer.GetLineInfo()}" );
			}

			List<int> meshIds = new();

			while ( true )
			{
				string token = lexer.Next();

				if ( token == "}" )
				{
					break;
				}
				else if ( int.TryParse( token, out int result ) )
				{
					meshIds.Add( result );
				}
				else
				{
					throw new Exception( $"Unknown token '{token}' {lexer.GetLineInfo()}" );
				}
			}

			return meshIds;
		}

		private static List<Entity> ParseEntities( Lexer lexer )
		{
			if ( !lexer.Expect( "{", true ) )
			{
				throw new Exception( $"Expected '{{' at {lexer.GetLineInfo()}" );
			}

			List<Entity> entities = new();

			while ( true )
			{
				string token = lexer.Next();

				if ( token == "}" )
				{
					break;
				}
				else if ( token == "Entity" )
				{
					if ( !lexer.Expect( "{", true ) )
					{
						throw new Exception( $"Expected '{{' at {lexer.GetLineInfo()}" );
					}

					Entity entity = new();
					bool parseEntity = true;
					while ( parseEntity )
					{
						token = lexer.Next();

						switch ( token )
						{
							case "}": parseEntity = false; break;
							case "RenderMeshId": entity.RenderMeshId = int.Parse( lexer.Next() ); break;
							case "CollisionMeshId": entity.CollisionMeshId = int.Parse( lexer.Next() ); break;
							case "OccluderMeshId": entity.OccluderMeshId = int.Parse( lexer.Next() ); break;
							case "Attributes":
								if ( !lexer.Expect( "{", true ) )
								{
									throw new Exception( $"Expected '{{' at {lexer.GetLineInfo()}" );
								}

								bool parseAttributes = true;
								while ( parseAttributes )
								{
									token = lexer.Next();
									entity.Attributes.Add( token, lexer.Next() );
								}
								break;
							default: throw new Exception( $"Unknown token '{token}' {lexer.GetLineInfo()}" );
						}
					}

					entities.Add( entity );
				}
				else
				{
					throw new Exception( $"Unknown token '{token}' {lexer.GetLineInfo()}" );
				}
			}

			return entities;
		}

		private static List<CollisionMesh> ParseCollisionMeshes( Lexer lexer )
		{
			CollisionMeshlet ParseMeshlet()
			{
				if ( !lexer.Expect( "{", true ) )
				{
					throw new Exception( $"Expected '{{' at {lexer.GetLineInfo()}" );
				}

				CollisionMeshlet meshlet = new();

				while ( true )
				{
					string token = lexer.Next();

					if ( token == "}" )
					{
						break;
					}
					else if ( token == "MaterialName" )
					{
						meshlet.MaterialName = lexer.Next();
					}
					else if ( token == "Positions" )
					{
						if ( !lexer.Expect( "{", true ) )
						{
							throw new Exception( $"Expected '{{' at {lexer.GetLineInfo()}" );
						}

						while ( true )
						{
							token = lexer.Next();

							if ( token == "}" )
							{
								break;
							}
							else if ( token == "(" )
							{
								Vector3 position = new();
								position.X = Parse.Float( lexer.Next() );
								position.Y = Parse.Float( lexer.Next() );
								position.Z = Parse.Float( lexer.Next() );

								if ( !lexer.Expect( "}", true ) )
								{
									throw new Exception( $"Expected '}}' at {lexer.GetLineInfo()}" );
								}

								meshlet.Positions.Add( position );
							}
							else
							{
								throw new Exception( $"Unknown token '{token}' {lexer.GetLineInfo()}" );
							}
						}
					}
					else
					{
						throw new Exception( $"Unknown token '{token}' {lexer.GetLineInfo()}" );
					}
				}

				return meshlet;
			}

			if ( !lexer.Expect( "{", true ) )
			{
				throw new Exception( $"Expected '{{' at {lexer.GetLineInfo()}" );
			}

			List<CollisionMesh> collisionMeshes = new();

			while ( true )
			{
				string token = lexer.Next();

				if ( token == "}" )
				{
					break;
				}
				else if ( token == "CollisionMesh" )
				{
					if ( !lexer.Expect( "{", true ) )
					{
						throw new Exception( $"Expected '{{' at {lexer.GetLineInfo()}" );
					}

					CollisionMesh mesh = new();

					while ( true )
					{
						token = lexer.Next();

						if ( token == "}" )
						{
							break;
						}
						else if ( token == "Meshlets" )
						{
							if ( !lexer.Expect( "{", true ) )
							{
								throw new Exception( $"Expected '{{' at {lexer.GetLineInfo()}" );
							}

							while ( true )
							{
								token = lexer.Next();

								if ( token == "}" )
								{
									break;
								}
								else if ( token == "CollisionMeshlet" )
								{
									mesh.Meshlets.Add( ParseMeshlet() );
								}
								else
								{
									throw new Exception( $"Unknown token '{token}' {lexer.GetLineInfo()}" );
								}
							}
						}
						else
						{
							throw new Exception( $"Unknown token '{token}' {lexer.GetLineInfo()}" );
						}
					}

					collisionMeshes.Add( mesh );
				}
				else
				{
					throw new Exception( $"Unknown token '{token}' {lexer.GetLineInfo()}" );
				}
			}

			return collisionMeshes;
		}

		private static List<RenderMesh> ParseRenderMeshes( Lexer lexer )
		{
			RenderSurface ParseSurface()
			{
				if ( !lexer.Expect( "{", true ) )
				{
					throw new Exception( $"Expected '{{' at {lexer.GetLineInfo()}" );
				}

				RenderSurface surface = new();

				while ( true )
				{
					string token = lexer.Next();

					if ( token == "}" )
					{
						break;
					}
					else if ( token == "BoundingBox" )
					{
						Vector3 position = new();
						Vector3 size = new();

						if ( !lexer.Expect( "(", true ) )
						{
							throw new Exception( $"Expected '(' at {lexer.GetLineInfo()}" );
						}

						position.X = Parse.Float( lexer.Next() );
						position.Y = Parse.Float( lexer.Next() );
						position.Z = Parse.Float( lexer.Next() );

						if ( !lexer.Expect( ")", true ) )
						{
							throw new Exception( $"Expected ')' at {lexer.GetLineInfo()}" );
						}

						if ( !lexer.Expect( "(", true ) )
						{
							throw new Exception( $"Expected '(' at {lexer.GetLineInfo()}" );
						}

						size.X = Parse.Float( lexer.Next() );
						size.Y = Parse.Float( lexer.Next() );
						size.Z = Parse.Float( lexer.Next() );

						if ( !lexer.Expect( ")", true ) )
						{
							throw new Exception( $"Expected ')' at {lexer.GetLineInfo()}" );
						}

						surface.BoundingBox = new Aabb( position, size );
					}
					else if ( token == "VertexCount" )
					{
						surface.VertexCount = Parse.Int( lexer.Next() );
					}
					else if ( token == "Material" )
					{
						surface.Material = lexer.Next();
					}
					else if ( token == "LightmapTexture" )
					{
						surface.LightmapTexture = lexer.Next();
					}
					else if ( token == "Positions" || token == "Normals" )
					{
						if ( !lexer.Expect( "{", true ) )
						{
							throw new Exception( $"Expected '{{' at {lexer.GetLineInfo()}" );
						}

						List<Vector3> list = token == "Positions" ? surface.Positions : surface.Normals;

						while ( true )
						{
							token = lexer.Next();

							if ( token == "}" )
							{
								break;
							}
							else if ( token == "(" )
							{
								Vector3 vec = new();
								vec.X = Parse.Float( lexer.Next() );
								vec.Y = Parse.Float( lexer.Next() );
								vec.Z = Parse.Float( lexer.Next() );

								if ( !lexer.Expect( ")", true ) )
								{
									throw new Exception( $"Expected ')' at {lexer.GetLineInfo()}" );
								}

								list.Add( vec );
							}
							else
							{
								throw new Exception( $"Unknown token '{token}' {lexer.GetLineInfo()}" );
							}
						}
					}
					else if ( token == "Uvs" || token == "LightmapUvs" )
					{
						if ( !lexer.Expect( "{", true ) )
						{
							throw new Exception( $"Expected '{{' at {lexer.GetLineInfo()}" );
						}

						List<Vector2> list = token == "Uvs" ? surface.Uvs : surface.LightmapUvs;

						while ( true )
						{
							token = lexer.Next();

							if ( token == "}" )
							{
								break;
							}
							else if ( token == "(" )
							{
								Vector2 vec = new();
								vec.X = Parse.Float( lexer.Next() );
								vec.Y = Parse.Float( lexer.Next() );

								if ( !lexer.Expect( ")", true ) )
								{
									throw new Exception( $"Expected ')' at {lexer.GetLineInfo()}" );
								}

								list.Add( vec );
							}
							else
							{
								throw new Exception( $"Unknown token '{token}' {lexer.GetLineInfo()}" );
							}
						}
					}
					else if ( token == "Colours" )
					{
						if ( !lexer.Expect( "{", true ) )
						{
							throw new Exception( $"Expected '{{' at {lexer.GetLineInfo()}" );
						}

						while ( true )
						{
							token = lexer.Next();

							if ( token == "}" )
							{
								break;
							}
							else if ( token == "(" )
							{
								Vector4 vec = new();
								vec.X = Parse.Float( lexer.Next() );
								vec.Y = Parse.Float( lexer.Next() );
								vec.Z = Parse.Float( lexer.Next() );
								vec.W = Parse.Float( lexer.Next() );

								if ( !lexer.Expect( ")", true ) )
								{
									throw new Exception( $"Expected ')' at {lexer.GetLineInfo()}" );
								}

								surface.Colours.Add( vec );
							}
							else
							{
								throw new Exception( $"Unknown token '{token}' {lexer.GetLineInfo()}" );
							}
						}
					}
					else if ( token == "Indices" )
					{
						if ( !lexer.Expect( "{", true ) )
						{
							throw new Exception( $"Expected '{{' at {lexer.GetLineInfo()}" );
						}

						while ( true )
						{
							token = lexer.Next();

							if ( token == "}" )
							{
								break;
							}
							else if ( int.TryParse( token, out int result ) )
							{
								surface.Indices.Add( result );
							}
							else
							{
								throw new Exception( $"Unknown token '{token}' {lexer.GetLineInfo()}" );
							}
						}
					}
					else
					{
						throw new Exception( $"Unknown token '{token}' {lexer.GetLineInfo()}" );
					}
				}

				return surface;
			}

			if ( !lexer.Expect( "{", true ) )
			{
				throw new Exception( $"Expected '{{' at {lexer.GetLineInfo()}" );
			}

			List<RenderMesh> renderMeshes = new();

			while ( true )
			{
				string token = lexer.Next();

				if ( token == "}" )
				{
					break;
				}
				else if ( token == "RenderMesh" )
				{
					if ( !lexer.Expect( "{", true ) )
					{
						throw new Exception( $"Expected '{{' at {lexer.GetLineInfo()}" );
					}

					RenderMesh mesh = new();

					while ( true )
					{
						token = lexer.Next();

						if ( token == "}" )
						{
							break;
						}
						else if ( token == "Surfaces" )
						{
							mesh.Surfaces.Add( ParseSurface() );
						}
						else
						{
							throw new Exception( $"Unknown token '{token}' {lexer.GetLineInfo()}" );
						}
					}

					renderMeshes.Add( mesh );
				}
				else
				{
					throw new Exception( $"Unknown token '{token}' {lexer.GetLineInfo()}" );
				}
			}

			return renderMeshes;
		}
	}
}
