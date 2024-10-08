using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF
{
	public class FNAFPhone
	{
		public GameObject Object;
		public SkinnedModelRenderer Model;
		private Scene LocalScene;
		private static SoundEvent PhoneSound;
		private SoundHandle SoundHandle;
		private TimeSince SpawnETA;
		public bool Done;
		private int night;
		public void InitSounds( int night )
		{
			this.night = night;
			PhoneSound = new SoundEvent();
			if ( night < 6 )
			{
				PhoneSound.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/voiceover" + night + ".wav" ) };
			}
		}
		public FNAFPhone( Scene scene, int night = 1 )
		{
			LocalScene = scene;
			InitSounds( night );
			Object = LocalScene.CreateObject( true );
			Model = Object.Components.Create<SkinnedModelRenderer>( true );
			Model.Model = Sandbox.Model.Load( "models/techmisc/phone/phone.vmdl" );
			Object.WorldPosition = new Vector3( -15, -1164, 106.071f );
			Object.WorldRotation = new Angles( 0, 170.196f, 0 );
			Model.SceneModel.SetAnimParameter( "on", true );
			SpawnETA = 0;
		}
		public void Silence()
		{
			if ( SoundHandle != null )
				SoundHandle.Stop();
			Model.SceneModel.SetAnimParameter( "on", false );
			Done = true;
		}
		public void Think()
		{
			if ( !Done & SpawnETA > 5 )
			{
				if ( night < 6 )
				{
					Model.SceneModel.SetMaterialGroup( "playing" );
					SoundHandle = Sound.Play( PhoneSound, Object.WorldPosition );
				}
				Done = true;
				return;
			}
			if ( SoundHandle == null ) { return; }
			if ( Done & !SoundHandle.IsPlaying )
			{
				Model.SceneModel.SetMaterialGroup( "default" );
			}
		}
	}
}
