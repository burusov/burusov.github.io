using UnityEngine;

namespace Liquid {

    public class Main : MonoBehaviour
    {
        [SerializeField, Range(100, 300)] private int gridWidth = 200;
        [SerializeField, Range(100, 300)] private int gridHeight = 200;
        [SerializeField] private ParticleSystem liquidParticleSystem;
        private LiquidSimulation liquidSimulation;

        private void Start()
        {
            liquidSimulation = new LiquidSimulation(liquidParticleSystem);
            liquidSimulation.Initialize(gridWidth, gridHeight);
        }

        private void FixedUpdate()
        {
            liquidSimulation.Update();
            liquidSimulation.Update();
        }

        private void Update()
        {
            liquidSimulation.Render();
        }

        public void Reset()
        {
            liquidSimulation.Reset();
        }
    }
}
