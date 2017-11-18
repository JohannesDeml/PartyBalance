// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextureCreator.cs" company="Supyrb">
//   Copyright (c) 2017 Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   send@johannesdeml.com
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BitStrap;
using UnityEngine;
using UnityEditor;
using Random = System.Random;

namespace Supyrb
{
    [Serializable]
    public class QuadData
    {
        public string Name;
        public Vector2 UvLowerLeft;
        public Vector2 UvUpperRight;
        public Color VertexColor;

        public QuadData(string name, Vector2 uvLowerLeft, Vector2 uvUpperRight, Color vertexColor)
        {
            Name = name;
            UvLowerLeft = uvLowerLeft;
            UvUpperRight = uvUpperRight;
            VertexColor = vertexColor;
        }
    }


    public class QuadCreator : ScriptableObject
    {
        public Bounds MeshBounds = new Bounds(new Vector3(0f, 0.35f, 0f), new Vector3(1f, 1.4f, 1f));
        public int AtlasColumns = 4;
        public int AtlasRows = 4;
        public int NumElements = 16;
        public int VariantsForEachAtlasItem = 2;
        public int Seed = 100;
		public MinMaxRange Hue = new MinMaxRange(0f ,1f ,0f , 1f);
		public MinMaxRange Saturation = new MinMaxRange(0f ,1f ,0.7f , 1f);
		public MinMaxRange Value = new MinMaxRange(0f ,1f ,0.6f , 1f);
		[FolderPath(true)]
		public string DataPath = "Assets/";
        public List<QuadData> DataSet = new List<QuadData>();


        [Button]
        public void FillDataSet()
        {
            DataSet.Clear();
            var random = new Random(Seed);
            float uStepSize = 1f/AtlasColumns;
            float vStepSize = 1f/AtlasRows;

            for (int row = 0; row < AtlasRows; row++)
            {
                for (int column = 0; column < AtlasColumns; column++)
                {
                    var index = column + row * AtlasColumns;
                    if (index >= NumElements)
                    {
                        return;
                    }
                    var lowerLeft = new Vector2((float)column/AtlasColumns, 1-(float)(row+1)/AtlasRows);
                    var upperRight = new Vector2(lowerLeft.x + uStepSize, lowerLeft.y + vStepSize);
                    

                    for (int variant = 0; variant < VariantsForEachAtlasItem; variant++)
                    {
                        var QuadName = "AtlasQuad" + index + "-" + variant;
	                    var color = GetNextRandomColor(random);
                        DataSet.Add(new QuadData(QuadName, lowerLeft, upperRight, color));
                    }
                    
                }
            }
        }

	    private Color GetNextRandomColor(Random random)
	    {
			var randomHue = Hue.MinValue + ((float)random.Next(2048) / 2048) * Hue.ValueRange;
			var randomSaturation = Saturation.MinValue + ((float)random.Next(2048) / 2048) * Saturation.ValueRange;
			var randomValue = Value.MinValue + ((float)random.Next(2048) / 2048) * Value.ValueRange;
			return Color.HSVToRGB(randomHue, randomSaturation, randomValue);
		}

	    [Button]
        public void SaveQuads()
        {
            var vertices = new Vector3[4];
            var triangles = new int[6];
            var normals = new Vector3[4];
            var uvs = new Vector2[4];
            var colors = new Color[4];
            

            vertices[0] = new Vector3(-0.5f, -0.5f);
            vertices[1] = new Vector3(0.5f, -0.5f);
            vertices[2] = new Vector3(-0.5f, 0.5f);
            vertices[3] = new Vector3(0.5f, 0.5f);

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            triangles[3] = 1;
            triangles[4] = 3;
            triangles[5] = 2;

            normals[0] = Vector3.forward;
            normals[1] = Vector3.forward;
            normals[2] = Vector3.forward;
            normals[3] = Vector3.forward;

            for (int i = 0; i < DataSet.Count; i++)
            {
                var data = DataSet[i];
                var mesh = new Mesh();
                var lowerLeft = data.UvLowerLeft;
                var upperRight = data.UvUpperRight;
                uvs[0] = lowerLeft;
                uvs[1] = new Vector2(upperRight.x, lowerLeft.y);
                uvs[2] = new Vector2(lowerLeft.x, upperRight.y);
                uvs[3] = upperRight;

                colors[0] = data.VertexColor;
                colors[1] = data.VertexColor;
                colors[2] = data.VertexColor;
                colors[3] = data.VertexColor;

                mesh.name = data.Name;
                mesh.vertices = vertices;
                mesh.triangles = triangles;
                mesh.normals = normals;
                mesh.bounds = MeshBounds;
                mesh.uv = uvs;
                mesh.colors = colors;
                AssetDatabase.CreateAsset(mesh, DataPath + data.Name + ".asset");
            }
            AssetDatabase.Refresh();
        }
    }
}
