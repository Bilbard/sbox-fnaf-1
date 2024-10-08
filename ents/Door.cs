using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF
{
	public class FNAFDoor
	{
		public GameObject Object;
		public SkinnedModelRenderer Model;
		private int State;
		private Vector3 Closed;
		private static SoundEvent DoorSound;
		private static bool SoundInit;
		public static void InitSounds()
		{
			DoorSound = new SoundEvent();
			DoorSound.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/freddydoor.wav" ) };
			SoundInit = true;
		}
		public FNAFDoor( Vector3 pos, Angles ang, Scene scene )
		{
			if ( !SoundInit ) { InitSounds(); }
			Object = scene.CreateObject( true );
			Model = Object.Components.Create<SkinnedModelRenderer>( true );
			Model.Model = Sandbox.Model.Load( "models/fnaf/officedoor.vmdl" );
			Object.WorldPosition = pos + new Vector3( 0, 0, 88 );
			Closed = pos;
			Object.WorldRotation = ang;
			State = 0;
		}
		public void Open( bool sound = true )
		{
			State = 1;
			if ( sound ) { Sound.Play( DoorSound, Closed ); }

		}
		public void Close( bool sound = true )
		{
			State = 3;
			if ( sound ) { Sound.Play( DoorSound, Closed ); }
		}
		public void Tick()
		{
			if ( State == 1 )
			{
				if ( Object.WorldPosition.z == (Closed.z + 88) )
				{
					State = 0;
					return;
				}
				Object.WorldPosition += new Vector3( 0, 0, 8 );
			}
			else if ( State == 3 )
			{
				if ( Object.WorldPosition.z == (Closed.z) )
				{
					State = 2;
					return;
				}
				Object.WorldPosition -= new Vector3( 0, 0, 8 );
			}
		}
		public bool IsClosed()
		{
			if ( State > 1 )
			{
				return true;
			}
			return false;
		}
	}
}
