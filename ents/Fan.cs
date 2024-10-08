using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF
{
	public class FNAFFan
	{
		public GameObject Object;
		public SkinnedModelRenderer Model;
		private Scene LocalScene;
		private static SoundEvent FanSound;
		private SoundHandle SoundHandle;
		private static bool SoundInit;
		public static void InitSounds()
		{
			FanSound = new SoundEvent();
			FanSound.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/buzz_fan_florescent2.sound" ) };
			SoundInit = true;
		}
		public FNAFFan( Scene scene )
		{
			if ( !SoundInit )
				InitSounds();
			LocalScene = scene;
			Object = LocalScene.CreateObject( true );
			Model = Object.Components.Create<SkinnedModelRenderer>( true );
			Model.Model = Sandbox.Model.Load( "models/junk/fnaffan.vmdl" );
			Object.WorldPosition = new Vector3( -12, -1148, 106 );
			Object.WorldRotation = new Angles( 0, 82.8755f, 0 );
			Model.SceneModel.SetAnimParameter( "on", true );
			SoundHandle = Sound.Play( FanSound, Object.WorldPosition );
		}
		public void VolumeChange( bool cams = false )
		{
			SoundHandle.Volume = cams ? 0.1f : 0.5f;
		}
		public void Off()
		{
			if ( SoundHandle != null )
				SoundHandle.Stop();
			Model.SceneModel.SetAnimParameter( "on", false );
		}
	}
}
