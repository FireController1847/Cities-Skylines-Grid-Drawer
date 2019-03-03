using ColossalFramework.Math;
using GridDrawer.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GridDrawer {
    public class GridRenderManager : SimulationManagerBase<RenderManager, RenderProperties>, ISimulationManager, IRenderableManager {
        private static float GRID_CELL_SIZE = 60f;
        // private static Vector3 GridCenter;

        /*
        private static float gridGap = ZoneManager.ZONEGRID_CELL_SIZE / 8;
        private static float gridSize = 1000f;
        private static float gridAlpha = 0.2f;
        */

        public static GridRenderManager Instance = new GridRenderManager();
        static GridRenderManager() {

        }

        /*
        protected override void BeginOverlayImpl(RenderManager.CameraInfo cameraInfo) {
            TerrainPatch clampedPatch = TerrainManager.instance.m_patches[40];
            GridCenter = clampedPatch.m_bounds.center;
        }
        */

        protected override void EndOverlayImpl(RenderManager.CameraInfo cameraInfo) {
            if (GlobalConfig.Instance.EnableVehicleManagerGrid) {
                RenderGrid(cameraInfo, new Vector3(0f, 0f, 0f), 8000f, 100f);
            }
        }

        private void RenderGrid(RenderManager.CameraInfo cameraInfo, Vector3 center, float size, float height) {
            Vector3 sizeVec = new Vector3(size, height, size);
            Bounds gridBounds = new Bounds(center, sizeVec);

            if (cameraInfo.Intersect(gridBounds)) {
                Vector3 xVec = new Vector3(1f, 0f, 0f);
                Vector3 zVec = new Vector3(0f, 0f, 1f);

                // Currently only draws a square - but could easily change shape by changing size in each direction. 
                int xLineCount = (int)Math.Floor(size / GRID_CELL_SIZE);
                int zLineCount = (int)Math.Floor(size / GRID_CELL_SIZE);

                for (int i = 0; i < xLineCount; i++) {
                    Quad3 quad = default(Quad3);
                    quad.a = center - xVec * (size / 2) + zVec * (i * GRID_CELL_SIZE - (size / 2));
                    quad.b = center - xVec * (size / 2) + zVec * (i * GRID_CELL_SIZE - (size / 2));
                    quad.c = center + xVec * (size / 2) + zVec * (i * GRID_CELL_SIZE - (size / 2));
                    quad.d = center + xVec * (size / 2) + zVec * (i * GRID_CELL_SIZE - (size / 2));
                    Color color = (i % 5 == 0 ? Color.red : Color.white);
                    // color.a = 0.2f;
                    RenderManager.instance.OverlayEffect.DrawQuad(cameraInfo, color, quad, -1f, 1025f, false, true);
                }

                for (int i = 0; i < zLineCount; i++) {
                    Quad3 quad = default(Quad3);
                    quad.a = center - zVec * (size / 2) + xVec * (i * GRID_CELL_SIZE - (size / 2));
                    quad.b = center - zVec * (size / 2) + xVec * (i * GRID_CELL_SIZE - (size / 2));
                    quad.c = center + zVec * (size / 2) + xVec * (i * GRID_CELL_SIZE - (size / 2));
                    quad.d = center + zVec * (size / 2) + xVec * (i * GRID_CELL_SIZE - (size / 2));
                    Color color = (i % 5 == 0 ? Color.red : Color.white);
                    // color.a = 0.2f;
                    RenderManager.instance.OverlayEffect.DrawQuad(cameraInfo, color, quad, -1f, 1025f, false, true);
                }
            }
        }

        public new string GetName() {
            return "GridRenderManager";
        }
    }
}