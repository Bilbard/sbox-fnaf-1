using FNAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF
{
	public class FNAFGoldenFreddy //no inherit, easier
	{
		public GameObject Object;
		public SkinnedModelRenderer Model;
		private Scene LocalScene;
		private static SoundEvent Giggle;
		private SoundHandle SoundHandle;
		private static bool SoundInit;
		private TimeSince PosterCooldown;
		private bool GigglePlayed;
		private bool Waiting;
		private bool Assumed;
		private bool testvar;
		private TimeSince Attack;
		public static void InitSounds()
		{
			Giggle = new SoundEvent();
			Giggle.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/gfreddygiggle.sound" ) };
			SoundInit = true;
		}
		public FNAFGoldenFreddy( Scene scene )
		{
			if ( !SoundInit )
				InitSounds();
			LocalScene = scene;
			Object = LocalScene.CreateObject( true );
			Model = Object.Components.Create<SkinnedModelRenderer>( true );
			Model.Model = Sandbox.Model.Load( "models/alejenus/fnafvr/goldenfreddy/gfreddyvr.vmdl" );
			PosterCooldown = 0;
			Waiting = false;
			GigglePlayed = false;
			Assumed = false;
			testvar = false;
			AssumePosition( false );
		}
		public void Think()
		{
			if ( FNAFGameManager.GameState.InCams & PosterCooldown >= 1 & !Waiting & FNAFGameManager.GameState.CurCam != "twob" )
			{
				PosterCooldown = 0;
				int roll = new Random().Next( 0, 10000 );
				if ( roll == 7891 | testvar )
				{
					Waiting = true;
					GigglePlayed = false;
					FNAFGameManager.GameState.FreddyPoster.MaterialGroup = "golden";
					testvar = false;
				}
			}
			if ( Waiting )
			{
				if ( !GigglePlayed & FNAFGameManager.GameState.CurCam == "twob" & FNAFGameManager.GameState.InCams )
				{
					SoundHandle = Sound.Play( Giggle, FNAFGameManager.GameState.OfficeCamera.WorldPosition );
					GigglePlayed = true;
					AssumePosition( true );
					Sandbox.Services.Stats.Increment( "encounters", 1 );
				}
				else if ( GigglePlayed & !FNAFGameManager.GameState.InCams & !Assumed )
				{
					CameraUI.hallucinationtimer = -2.5f;
					FNAFGameManager.GameState.FreddyPoster.MaterialGroup = "default";
					Waiting = false;
					Assumed = true;
					Attack = 0;
				}
				else if ( GigglePlayed & FNAFGameManager.GameState.CurCam != "twob" & FNAFGameManager.GameState.InCams )
				{
					CameraUI.hallucinationtimer = 5;
					FNAFGameManager.GameState.FreddyPoster.MaterialGroup = "default";
					Waiting = false;
					Assumed = false;
					AssumePosition( false );
					Attack = 0;
				}
				else if ( FNAFGameManager.GameState.CurCam != "twob" & FNAFGameManager.GameState.InCams )
				{
					Assumed = false;
					GigglePlayed = false;
					AssumePosition( false );
					CameraUI.hallucinationtimer = 5;
				}
			}
			if ( Assumed )
			{
				if ( FNAFGameManager.GameState.InCams )
				{
					Assumed = false;
					GigglePlayed = false;
					AssumePosition( false );
					CameraUI.hallucinationtimer = 5;
				}
				else if ( Attack >= 4 )
				{
					CameraUI.hallucinationtimer = 5;
					Jumpscare();
				}
			}
		}
		public void Jumpscare()
		{
			GameOver.GFreddyJumpscare = 0;
			FNAFGameManager.GameState.JumpscareBootTimer = 0;
		}
		public void AssumePosition( bool pos )
		{
			if ( pos )
			{
				Object.WorldPosition = new Vector3( -39, -1133, 64 );
				Object.WorldRotation = new Angles( 0, -168, 0 );
			}
			else
			{
				Object.WorldPosition = new Vector3( 127, -1484, 111 );
				Object.WorldRotation = new Angles( 0, 161, 0 );
			}
		}
	}
}
