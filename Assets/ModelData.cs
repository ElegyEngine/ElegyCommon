// SPDX-FileCopyrightText: 2022-2023 Admer Šuko
// SPDX-License-Identifier: MIT

namespace Elegy.Assets.ModelData
{
	/// <summary>
	/// All possible vertex data that can be stored in EMFs.
	/// </summary>
	public struct Vertex
	{
		public Vertex()
		{

		}

		public Vector3 Position { get; set; } = Vector3.Zero;
		public Vector3 Normal { get; set; } = Vector3.Up;
		public Vector3 Tangent { get; set; } = Vector3.Right;
		public Vector3 Bitangent { get; set; } = Vector3.Forward;
		public Vector2 Uv1 { get; set; } = Vector2.Zero;
		public Vector2 Uv2 { get; set; } = Vector2.Zero;
		public Vector4 Color1 { get; set; } = Vector4.One;
		public Vector4 Color2 { get; set; } = Vector4.One;
		public Vector4I BoneIndices { get; set; } = Vector4I.One;
		public Vector4 BoneWeights { get; set; } = Vector4.One;
	}

	/// <summary>
	/// Represents a particular part of a model with a material assigned to it.
	/// </summary>
	public class Mesh
	{
		public string Name { get; set; } = string.Empty;
		public List<Vertex> Vertices { get; set; } = new();

	}
}
