﻿using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[BurstCompile]
public struct NoiseJob : IJobParallelFor {
   [WriteOnly] public NativeArray<float> levels;
   [ReadOnly] public int size;
   [ReadOnly] public float noiseScale;
   [ReadOnly] public float noiseOffset;

   public void Execute(int index) {
      int z = index % size;
      int y = index / size % size;
      int x = index / (size * size);
      float fx = x / noiseScale + noiseOffset;
      float fy = y / noiseScale;
      float fz = z / noiseScale;
      levels[index] = PerlinNoise3D(fx, fy, fz);
   }

   private static float PerlinNoise3D(float x, float y, float z) {
      y += 1;
      z += 2;
      float xy = _perlin3DFixed(x, y);
      float xz = _perlin3DFixed(x, z);
      float yz = _perlin3DFixed(y, z);
      float yx = _perlin3DFixed(y, x);
      float zx = _perlin3DFixed(z, x);
      float zy = _perlin3DFixed(z, y);
      return xy * xz * yz * yx * zx * zy;
   }

   private static float _perlin3DFixed(float a, float b) {
      return Mathf.Sin(Mathf.PI * Mathf.PerlinNoise(a, b));
   }
}
