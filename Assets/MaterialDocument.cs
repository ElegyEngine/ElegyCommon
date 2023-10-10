// SPDX-FileCopyrightText: 2023 Admer Šuko
// SPDX-License-Identifier: MIT

using Elegy.Text;
using System.Collections.Specialized;

namespace Elegy.Assets
{
	/// <summary>
	/// A material defined inside a material document.
	/// </summary>
	public class MaterialDefinition
	{
		/// <summary>
		/// Material name.
		/// </summary>
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Name of the material template.
		/// </summary>
		public string TemplateName { get; set; } = string.Empty;

		/// <summary>
		/// Shader parameters.
		/// </summary>
		public StringDictionary Parameters { get; set; } = new();

		/// <summary>
		/// Safely obtain a parameter string.
		/// </summary>
		public string? GetParameterString( string name )
		{
			if ( Parameters.ContainsKey( name ) )
			{
				return Parameters[name];
			}

			return null;
		}

		/// <summary>
		/// Diffuse map.
		/// </summary>
		public string? DiffuseMap => GetParameterString( "map" );
	}

	/// <summary>
	/// Elegy material document.
	/// </summary>
	public class MaterialDocument
	{
		private void ParseMaterialParameters( Lexer lex, MaterialDefinition def )
		{
			if ( !lex.Expect( "{", true ) )
			{
				return;
			}

			while ( !lex.IsEnd() )
			{
				string token = lex.Next();
				
				if ( token == "}" )
				{
					return;
				}
				else
				{
					def.Parameters.Add( token, lex.TokensBeforeNewline() );
				}
			}
		}

		private MaterialDefinition? ParseMaterialDefinition( Lexer lex )
		{
			MaterialDefinition materialDefinition = new();

			string token = lex.Next();
			if ( token == "{" || token == "}" )
			{
				// TODO: add a way to jam errors into Lexer, or give me a logger!
				return null;
			}
			materialDefinition.Name = token;
			
			if ( !lex.Expect( "{", true ) )
			{
				return null;
			}

			while ( !lex.IsEnd() )
			{
				token = lex.Next();
				if ( token == "}" )
				{
					break;
				}
				else if ( token == "materialTemplate" )
				{
					materialDefinition.TemplateName = lex.Next();
					ParseMaterialParameters( lex, materialDefinition );
				}
				else
				{
					// Ignore everything else
					lex.SkipUntil( "}", true );
				}
			}

			return materialDefinition;
		}

		/// <summary>
		/// Parses a material document from given text,
		/// loaded from a file or generated elsewhere.
		/// </summary>
		public MaterialDocument( string content )
		{
			Lexer lex = new( content, "{}" );

			while ( !lex.IsEnd() )
			{
				MaterialDefinition? materialDef = ParseMaterialDefinition( lex );

				if ( materialDef is not null )
				{
					Materials.Add( materialDef );
				}
			}
		}

		/// <summary>
		/// The materials defined in this document.
		/// </summary>
		public List<MaterialDefinition> Materials { get; } = new();
	}
}
