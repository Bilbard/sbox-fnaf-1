using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF
{
	public class FNAFButton
	{
		public GameObject Object;
		public SkinnedModelRenderer Model;
		public bool State;
		public Color OnColor;
		public Color OffColor;
		public bool First;
		private static SoundEvent ButtonSound;
		private static SoundEvent SoundLocked;
		private static bool SoundInit;
		private bool DoSound;
		private SoundHandle LoopSound;
		private Vector3 position;
		public bool Locked;
		private bool AlwaysLit;
		private bool Flicker;
		private TimeSince FlickerTimer;
		public bool TrueState;
		public static void InitSounds()
		{
			ButtonSound = new SoundEvent();
			ButtonSound.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/freddylights.sound" ) };
			SoundLocked = new SoundEvent();
			SoundLocked.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/doorlocked.wav" ) };
			SoundInit = true;
		}
		public FNAFButton( Vector3 pos, Angles ang, Color oncolor, Color offcolor, int flicker = 0, bool alwayslit = false, bool sound = false )
		{
			if ( !SoundInit ) { InitSounds(); }
			Object = new GameObject();
			Model = Object.Components.Create<SkinnedModelRenderer>();
			Model.Model = Model.Model = Sandbox.Model.Load( "models/fnaf/doorbutton.vmdl" );
			Object.WorldPosition = pos;
			Object.WorldRotation = ang;
			position = pos;
			OnColor = oncolor;
			OffColor = offcolor;
			State = false;
			First = false;
			TrueState = false;
			FlickerTimer = 0;
			DoSound = sound;
			AlwaysLit = alwayslit;
			if ( AlwaysLit )
			{
				Model.MaterialGroup = "on";
			}
			Model.Tint = OffColor;
			if ( flicker == 1 ) { Flicker = true; }
		}

		public void Use( bool first = true )
		{
			if ( Locked )
			{
				Sound.Play( SoundLocked, position );
				return;
			}
			TrueState = !TrueState;
			State = TrueState;
			if ( State )
			{
				if ( !AlwaysLit )
				{
					Model.MaterialGroup = "on";
				}
				Model.Tint = OnColor;
				if ( DoSound )
				{
					LoopSound = Sound.Play( ButtonSound, position );
				}
			}
			else
			{
				if ( !AlwaysLit )
				{
					Model.MaterialGroup = "off";
				}
				Model.Tint = OffColor;
				if ( DoSound )
				{
					LoopSound.Stop();
				}
			}
			First = first;
		}

		public void KillLight()
		{
			Model.MaterialGroup = "off";
			Model.Tint = Color.White;
			TrueState = false;
			State = false;
		}
		public void Tick()
		{
			if ( First ) { First = false; }
			if ( Flicker & TrueState ) //ugly. oops!
			{
				if ( FlickerTimer >= 0 )
				{
					State = false;
					if ( DoSound ) { LoopSound.Volume = 1; }
					FlickerTimer = -(float)(new Random().NextDouble() + 0.2);
					State = true;
				}
				else if ( FlickerTimer >= -0.075 )
				{
					if ( DoSound ) { LoopSound.Volume = 0; }
					State = false;
				}
			}
		}
	}
}
