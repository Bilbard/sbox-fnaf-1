using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF
{
	public class FNAFTablet
	{
		public GameObject Object;
		public SkinnedModelRenderer Model;
		public bool State;
		private Scene LocalScene;
		public FNAFTablet( Scene scene, Vector3 pos )
		{
			LocalScene = scene;
			Object = LocalScene.CreateObject( true );
			Model = Object.Components.Create<SkinnedModelRenderer>( true );
			Model.Model = Sandbox.Model.Load( "models/tablet/tablet.vmdl" );
			Object.WorldPosition = pos;
			State = false;
		}
		public bool Flip()
		{
			State = !State;
			Model.SceneModel.SetAnimParameter( "flip", State );
			return State;
		}
	}
}
