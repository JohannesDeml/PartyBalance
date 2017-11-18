using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitStrap
{
    /// <summary>
    /// A simple tween class for shader param interpolation.
    /// It uses the MaterialPropertyBlock class to modify the renderer's material properties.
    /// It is better to use it than directly setting a material property because it does not
    /// create a new Material instance and, thus, does not generates a new drawcall.
    /// </summary>
    public class TweenShader : MonoBehaviour
    {
        /// <summary>
        /// A shader property that can be interpolated.
        /// </summary>
        [System.Serializable]
        public class ShaderProperty
        {
            /// <summary>
            /// The shader property type.
            /// </summary>
            public enum Type
            {
                Color,
                Float
            }

            /// <summary>
            /// The property's name.
            /// </summary>
            public string name;

            /// <summary>
            /// The interpolation origin.
            /// </summary>
            public Color from = Color.white;

            /// <summary>
            /// The interpolation target.
            /// </summary>
            public Color to = Color.white;

            /// <summary>
            /// The property's type.
            /// </summary>
            public Type type = Type.Float;

            private int? id = null;

            /// <summary>
            /// The cached property Id for faster access.
            /// </summary>
            public int Id
            {
                get
                {
                    if( !id.HasValue )
                    {
                        id = Shader.PropertyToID( name );
                    }

                    return id.Value;
                }
            }

            /// <summary>
            /// Evaluates the property at "t" and stores its value in the MaterialPropertyBlock.
            /// </summary>
            /// <param name="block"></param>
            /// <param name="t"></param>
            public void Evaluate( MaterialPropertyBlock block, float t )
            {
                if( type == Type.Color )
                {
                    block.SetColor( Id, Color.Lerp( from, to, t ) );
                }
                else if( type == Type.Float )
                {
                    block.SetFloat( Id, Mathf.Lerp( from.a, to.a, t ) );
                }
            }
        }

        /// <summary>
        /// The tween curve.
        /// </summary>
        public AnimationCurve curve = AnimationCurve.Linear( 0.0f, 0.0f, 1.0f, 1.0f );

        /// <summary>
        /// The tween duration.
        /// </summary>
        public float duration = 1.0f;

        /// <summary>
        /// List of shader properties to be interpolated.
        /// </summary>
        public List<ShaderProperty> shaderProperties;

        /// <summary>
        /// The current renderer to apply the MaterialPropertyBlock.
        /// </summary>
        public Renderer targetRenderer;

        private MaterialPropertyBlock propertyBlock;

        /// <summary>
        /// Clear the cached MaterialPropertyBlock.
        /// </summary>
        public void Clear()
        {
            propertyBlock.Clear();
            targetRenderer.SetPropertyBlock( propertyBlock );
        }

        /// <summary>
        /// Evaluates the tween and, consequently, all the shader properties at "t".
        /// </summary>
        /// <param name="t"></param>
        public void Evaluate( float t )
        {
            propertyBlock.Clear();
            t = curve.Evaluate( t );

            foreach( ShaderProperty p in shaderProperties )
            {
                p.Evaluate( propertyBlock, t );
            }

            targetRenderer.SetPropertyBlock( propertyBlock );
        }

        /// <summary>
        /// Play the tween backward interpolating all the shader properties.
        /// </summary>
        public void PlayBackward()
        {
            Stop();
            StartCoroutine( PlayAsync( 1.0f, 0.0f ) );
        }

        /// <summary>
        /// Play the tween forward interpolating all the shader properties.
        /// </summary>
        public void PlayForward()
        {
            Stop();
            StartCoroutine( PlayAsync( 0.0f, 1.0f ) );
        }

        /// <summary>
        /// Stop the tween.
        /// Use Clear() to reset to the beginning.
        /// </summary>
        public void Stop()
        {
            StopAllCoroutines();
        }

        private void Awake()
        {
            propertyBlock = new MaterialPropertyBlock();

            if( targetRenderer == null )
            {
                targetRenderer = GetComponent<Renderer>();
            }
        }

        private IEnumerator PlayAsync( float from, float to )
        {
            for( float time = 0.0f; time < duration; time += Time.deltaTime )
            {
                float t = Mathf.Lerp( from, to, Mathf.InverseLerp( 0.0f, duration, time ) );
                Evaluate( t );

                yield return null;
            }

            Evaluate( to );
        }

        private void Reset()
        {
            targetRenderer = GetComponent<Renderer>();
        }
    }
}
