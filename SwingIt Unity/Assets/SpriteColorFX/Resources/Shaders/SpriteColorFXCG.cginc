///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Sprite Color FX.
// Copyright (c) Ibuprogames. All rights reserved.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Luminance.
inline float Luminance601(float3 pixel)
{
  return dot(float3(0.299f, 0.587f, 0.114f), pixel);
}

// 1D rand.
inline float Rand1(float value)
{
  return frac(sin(value) * 43758.5453123);
}

// 2D rand.
inline float Rand2(float2 value)
{
  return frac(sin(dot(value * 0.123, float2(12.9898, 78.233))) * 43758.5453);
}

// 3D rand.
inline float Rand3(float3 value)
{
  return frac(sin(dot(value, float3(12.9898, 78.233, 45.5432))) * 43758.5453);
}
