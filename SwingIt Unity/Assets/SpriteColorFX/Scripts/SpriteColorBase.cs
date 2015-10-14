///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Color FX.
// Copyright (c) Ibuprogames. All rights reserved.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace SpriteColorFX
{
  /// <summary>
  /// Light mode.
  /// </summary>
  public enum LightMode
  {
    UnLit,
    BumpLit,
  }

  /// <summary>
  /// 
  /// </summary>
  public abstract class SpriteColorBase : MonoBehaviour
  {
    [SerializeField]
    protected LightMode lightMode = LightMode.UnLit;

    protected SpriteRenderer spriteRenderer;

    /// <summary>
    /// Shader path.
    /// </summary>
    protected abstract string ShaderPath { get; }

#region BumpLit.
    /// <summary>
    /// Bump intensity [0..5].
    /// </summary>
    public float bumpIntensity = 1.0f;

    /// <summary>
    /// Shininess [0.3..1].
    /// </summary>
    public float shininess = 0.078125f;

    /// <summary>
    /// Specular color.
    /// </summary>
    public Color specularColor = Color.gray;

    /// <summary>
    /// Bump texture.
    /// </summary>
    public Texture normalTex;

    /// <summary>
    /// Inverts the red channel of the normal texture.
    /// </summary>
    public bool invertRedNormalChannel = false;

    /// <summary>
    /// Inverts the green channel of the normal texture.
    /// </summary>
    public bool invertGreenNormalChannel = false;
#endregion

    /// <summary>
    /// Changes the light mode.
    /// </summary>
    public LightMode LightMode
    {
      get { return lightMode; }
      set
      {
        if (lightMode != value)
        {
          lightMode = value;

          CreateMaterial();

          Initialize();
        }
      }
    }

    private void OnEnable()
    {
      spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
      if (spriteRenderer != null)
      {
        CreateMaterial();

        Initialize();
      }
      else
      {
        Debug.LogWarning(string.Format("'{0}' without SpriteRenderer, disabled.", this.GetType().ToString()));

        this.enabled = false;
      }
    }

    private void OnDisable()
    {
      if (spriteRenderer != null && spriteRenderer.sharedMaterial != null && string.CompareOrdinal(spriteRenderer.sharedMaterial.name, @"Sprites/Default") != 0)
      {
        spriteRenderer.sharedMaterial = new Material(Shader.Find(@"Sprites/Default"));
        spriteRenderer.sharedMaterial.name = @"Sprites/Default";
      }
    }

    private void Update()
    {
      if (spriteRenderer == null)
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

      if (spriteRenderer != null && spriteRenderer.sharedMaterial != null)
      {
        if (lightMode == LightMode.BumpLit)
        {
          spriteRenderer.sharedMaterial.SetTexture(SpriteColorHelper.ShaderBumpMapParam, normalTex);
          spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderBumpIntensityParam, bumpIntensity);
          spriteRenderer.sharedMaterial.SetFloat(SpriteColorHelper.ShaderShininessParam, shininess);
          spriteRenderer.sharedMaterial.SetColor(SpriteColorHelper.ShaderSpecularColorParam, specularColor);

          spriteRenderer.sharedMaterial.SetVector(SpriteColorHelper.ShaderBumpFactorChannelsParam,
                                                  (invertRedNormalChannel == true || invertGreenNormalChannel == true) ?
                                                    new Vector3(invertRedNormalChannel ? -1.0f : 1.0f, invertGreenNormalChannel ? -1.0f : 1.0f) :
                                                    Vector3.one);
        }

        UpdateShader();
      }
    }

    /// <summary>
    /// Create the material.
    /// </summary>
    protected void CreateMaterial()
    {
      string effectName = this.GetType().ToString().Replace(@"SpriteColorFX.", string.Empty);

      string shaderFile = ShaderPath;

      Shader shader = Resources.Load<Shader>(shaderFile);
      if (shader == null)
      {
        Debug.LogWarning(string.Format("Failed to load '{0}', {1} disabled.", shaderFile, effectName));

        this.enabled = false;
      }
      else if (shader.isSupported == false)
      {
        Debug.LogWarning(string.Format("Shader '{0}' not supported, {1} disabled.", shaderFile, effectName));

        this.enabled = false;
      }
      else
      {
        spriteRenderer.sharedMaterial = new Material(shader);
        spriteRenderer.sharedMaterial.name = string.Format("Sprite/{0}", effectName);
      }
    }

    /// <summary>
    /// Initialize the effect.
    /// </summary>
    protected virtual void Initialize()
    {
    }

    /// <summary>
    /// Send values to shader.
    /// </summary>
    protected abstract void UpdateShader();
  }
}