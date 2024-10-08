using FNAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF
{
	public class FNAFEndo
	{
		public GameObject Object;
		public SkinnedModelRenderer Model;
		private Scene LocalScene;
		public bool Scare;
		private bool latch;
		private bool testvar;
		public FNAFEndo( Scene scene )
		{
			LocalScene = scene;
			Object = LocalScene.CreateObject( true );
			Model = Object.Components.Create<SkinnedModelRenderer>( true );
			Model.Model = Sandbox.Model.Load( "models/alejenus/fnafvr/endo.vmdl" );
			Object.WorldPosition = new Vector3( 941.25f, -738.5f, 68.25f );
			Object.WorldRotation = new Angles( 0, 180, 0 );
			Model.SceneModel.SetAnimParameter( "pose", 0 );
			Scare = false;
			latch = false;
			testvar = false;
		}
		public void Think()
		{
			int roll = new Random().Next( 0, 90001 );
			if ( (roll == 69696 | testvar) & (FNAFGameManager.GameState.CurCam != "five" | !FNAFGameManager.GameState.InCams) )
			{
				DoScare();
				testvar = false;
			}
			if ( Scare )
			{
				if ( FNAFGameManager.GameState.CurCam == "five" & FNAFGameManager.GameState.InCams )
				{
					latch = true;
				}
				if ( latch & (FNAFGameManager.GameState.CurCam != "five" | !FNAFGameManager.GameState.InCams) )
				{
					StopScare();
					latch = false;
				}
			}
		}
		public void DoScare()
		{
			Scare = true;
			Model.SceneModel.SetAnimParameter( "pose", 1 );
		}
		public void StopScare()
		{
			Scare = false;
			Model.SceneModel.SetAnimParameter( "pose", 0 );
		}
	}
}
