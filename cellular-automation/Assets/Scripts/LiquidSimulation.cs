using UnityEngine;
using static Liquid.LiquidCellularAutomation;

namespace Liquid
{
    public class LiquidSimulation
    {
        private LiquidCellularAutomation cellularAutomation;
        private readonly ParticleSystem particleSystem;
        private readonly ParticleSystem.Particle[] particles = new ParticleSystem.Particle[2048];
        private readonly ParticleSystem.EmitParams emitParams = new();

        public LiquidSimulation(ParticleSystem particleSystem)
        {
            this.particleSystem = particleSystem;
        }

        public void Initialize(int gridWidth, int gridHeight)
        {
            cellularAutomation = new LiquidCellularAutomation(gridWidth, gridHeight);
            cellularAutomation.BlockCells(x: 50, y: 125, width: 100, height: 13);
            cellularAutomation.BlockCells(x: 25, y: 100, width: 50, height: 13);
            cellularAutomation.BlockCells(x: 50, y: 75, width: 50, height: 13);
            cellularAutomation.BlockCells(x: 75, y: 50, width: 75, height: 13);

            Reset();
        }

        public void Reset()
        {
            cellularAutomation.EmptyCells();

            cellularAutomation.FillCells(x: 70, y: 160, width: 20, height: 20);
            cellularAutomation.FillCells(x: 90, y: 190, width: 15, height: 10);
            cellularAutomation.FillCells(x: 110, y: 150, width: 30, height: 20);
        }

        public void Update()
        {
            cellularAutomation.Automate();
        }

        public void Render()
        {
            UpdateParticles(ref cellularAutomation.cells);
        }

        private void UpdateParticles(ref sbyte[,] cells)
        {
            if (particleSystem.particleCount < cells.Length)
            {
                particleSystem.Emit(emitParams, cells.Length - particleSystem.particleCount);
                particleSystem.GetParticles(particles);
            }

            Vector3 position = Vector3.zero;
            int i = 0;
            int width = cells.GetLength(0);
            int height = cells.GetLength(1);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (cells[y, x] != FILLED)
                    {
                        continue;
                    }

                    position.x = x;
                    position.y = y;
                    if (i < particles.Length)
                    {
                        particles[i].position = position;
                    }

                    i++;
                }
            }

            for (; i < particles.Length; i++)
            {
                particles[i].remainingLifetime = 0f;
            }

            particleSystem.SetParticles(particles);
        }
    }
}
