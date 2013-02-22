using System;
using Artemis;
using ArtemisTest.Components;
using Artemis.System;
namespace ArtemisTest.System
{
	public class DummySystem : EntityProcessingSystem {
		private ComponentMapper<Health> healthMapper;

        public DummySystem() : base(typeof(Health)) { }
	
		public override void Initialize() {
            healthMapper = new ComponentMapper<Health>(this.EntityWorld);
		}
	
		public override void Process(Entity e) {
			
            for (int i = 0; i < 10; i++)
            {
                double x = Math.Log(i) * Math.Cos(i);
            }
		}
	
	}
}
