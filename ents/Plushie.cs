using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF
{
	public class FNAFPlushie
	{
		public GameObject Object;
		public SkinnedModelRenderer Model;
		public FNAFPlushie( Vector3 pos, Angles ang, Scene scene, string model )
		{
			Object = scene.CreateObject( true );
			Model = Object.Components.Create<SkinnedModelRenderer>( true );
			Model.Model = Sandbox.Model.Load( model );
			Object.WorldPosition = pos;
			Object.WorldRotation = ang;
			Object.Enabled = false;
		}
		public void Enable()
		{
			Object.Enabled = true;
		}
		public void Disable()
		{
			Object.Enabled = false;
		}
	}
}
