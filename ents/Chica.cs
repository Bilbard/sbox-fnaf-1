using FNAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF
{
	public class FNAFChica : FNAFAnimatronic
	{
		public TimeSince StingerTimer;
		public TimeSince KitchenTimer;
		public SoundEvent KitchenSounds;
		public int Tweaking;
		public FNAFChica( Scene scene, int night = 1, int diffoverride = -1 )
		{
			Setup( scene, night, diffoverride );
			AnimIndex = new Dictionary<string, int>
			{
				{ "spawn", 1 },
				{ "oneb", 2 },
				{ "seven", 3 },
				{ "sevenalt", 3 },
				{ "six", 4 },
				{ "foura", 5 },
				{ "fourb", 6 },
				{ "office", 7 },
				{ "inoffice", 8 }
			};
			AIIndex = new Dictionary<string, List<string>> {
				{ "spawn", new List<string> { "oneb" } },
				{ "oneb", new List<string> { "seven", "six" } },
				{ "six", new List<string> { "foura", "seven", "foura" } },
				{ "seven", new List<string> { "foura", "six", "foura" } },//hacks to make her more aggressive
				{ "sevenalt", new List<string> { "foura", "six", "foura" } },
				{ "foura", new List<string> { "fourb", "oneb", "fourb" } },
				{ "fourb", new List<string> { "office", "foura", "office" } },
				{ "office", new List<string> { "oneb" } }
			};
			Difficulty = new Dictionary<int, int>
			{
				{ 1, 0 },
				{ 2, 1 },
				{ 3, 5 },
				{ 4, 4 },
				{ 5, 7 },
				{ 6, 12 },
				{ 7, 0 }
			};
			AIPositions = new Dictionary<string, Transform>
			{
				{ "spawn", new Transform(new Vector3(1264, -1312, 96), new Angles(0, 180, 0).ToRotation()) },
				{ "oneb", new Transform(new Vector3(875, -1213, 64), new Angles(0, 105, 0)) },
				{ "seven", new Transform(new Vector3(832, -1744, 64), new Angles(0, 0, 0)) },
				{ "sevenalt", new Transform(new Vector3(942, -1750, 64), new Angles(0, 27, 0)) },
				{ "six", new Transform(new Vector3(212, -1449, 64), new Angles(0, 165, 0)) },
				{ "foura", new Transform(new Vector3(191, -1253, 64), new Angles(0, 180, 0)) },
				{ "fourb", new Transform(new Vector3(-247, -1254, 64), new Angles(0, 0, 0)) },
				{ "office", new Transform(new Vector3(26.69f, -1237.8f, 64), new Angles(0, 118.069f, 0)) },
				{ "inoffice", new Transform(new Vector3(-176, -1180, 64), new Angles(0, 30, 0)) }
			};
			Model.Model = Sandbox.Model.Load( "models/alejenus/fnafvr/chica/chicavr.vmdl" );
			HeldItemModel.Model = Sandbox.Model.Load( "models/alejenus/fnafvr/chica/chicavr_cupcakeplate.vmdl" );
			HeldItemModel.WorldPosition = new Vector3( 18.5f, 17, 50 );
			CurrentAI = Difficulty[night];
			MoveSound = FNAFGameManager.GameState.stepsound;
			KitchenTimer = 17;
			KitchenSounds = new SoundEvent();
			Tweaking = 0;
			KitchenSounds.Sounds = new List<SoundFile> {
				SoundFile.Load( "sounds/kitchensounds1.wav" ),
				SoundFile.Load( "sounds/kitchensounds2.wav" ),
				SoundFile.Load( "sounds/kitchensounds3.wav" ),
				SoundFile.Load( "sounds/kitchensounds4.wav" )
			};
			ChangePos( "spawn", false );
		}
		public override void ChangePos( string pos, bool hideitem = true )
		{
			if ( pos == "five" )
			{
				if ( new Random().Next( 0, 6 ) == 3 )
				{
					pos = "fivealt";
				}
			}
			base.ChangePos( pos, hideitem );
			if ( hideitem )
			{
				HeldItem.Destroy();
			}
			if ( pos == "fivealt" )
			{
				Model.SceneModel.SetMaterialGroup( "creepy" );
			}
			else
			{
				Model.SceneModel.SetMaterialGroup( "default" );
			}
			//Sound.Play( FNAFGameManager.GameState.stepsound, Object.WorldPosition );
		}
		public override void Jumpscare()
		{
			Sandbox.Services.Stats.Increment( "chicakills", 1 );
			base.Jumpscare();
		}
		public override void NewHour( int hour )
		{
			if ( hour > 2 & hour < 5 )
			{
				CurrentAI += 1;
			}
		}
		public override void Think( bool forcemove = false, string cam = "" )
		{
			base.Think( forcemove, cam );
			if ( CurrentPos == "office" & FNAFGameManager.GameState.RightLightButton.State & StingerTimer > 10 )
			{
				Sound.Play( FNAFGameManager.GameState.stingersound, FNAFGameManager.GameState.RightLightButton.Object.WorldPosition );
				StingerTimer = 0;
			}
			if ( cam != "" )
			{
				ChangePos( cam );
				return;
			}
			if ( FNAFGameManager.GameState.InCams & FNAFGameManager.GameState.CurCam != "fourb" & Tweaking == 0 & FNAFGameManager.night > 4 & CurrentPos == "fourb" )
			{
				var rand = new Random().Next( 0, 1000 );
				if ( rand == 42 )
				{
					Tweaking = 1;
					Model.SceneModel.SetAnimParameter( "pose", 9 );
				}
			}
			else if ( FNAFGameManager.GameState.InCams & FNAFGameManager.GameState.CurCam == "fourb" & Tweaking == 1 )
			{
				Tweaking = 2;
				FNAFGameManager.GameState.tweakingsound = Sound.Play( FNAFGameManager.GameState.tweakingsounds, FNAFGameManager.GameState.OfficeCamera.WorldPosition );
			}
			else if ( Tweaking == 2 & (!FNAFGameManager.GameState.InCams | FNAFGameManager.GameState.CurCam != "fourb" | CurrentPos != "fourb") )
			{
				Model.SceneModel.SetAnimParameter( "pose", AnimIndex[CurrentPos] );
				Tweaking = 0;
				FNAFGameManager.GameState.tweakingsound.Stop();
			}
			else if ( CurrentPos != "fourb" )
			{
				Model.SceneModel.SetAnimParameter( "pose", AnimIndex[CurrentPos] );
				Tweaking = 0;
			}
			if ( KitchenTimer > 13 & CurrentPos == "six" )
			{
				var e = Sound.Play( KitchenSounds, new Vector3( 66, -1280, 93 ) );
				e.Volume = 1.3f;
				KitchenTimer = 0;
			}
			if ( ReadyToScare )
			{
				if ( !CamBuffer )
				{
					if ( FNAFGameManager.GameState.InCams )
					{
						CamBuffer = true;
						return;
					}
				}
				else if ( CamBuffer & !FNAFGameManager.GameState.InCams & !FNAFGameManager.GameState.InJumpscare )
				{
					FNAFGameManager.GameState.CloseCams();
					FNAFGameManager.GameState.InJumpscare = true;
					Jumpscare();
				}
				else
				{
					if ( MovementOpportunity > 25 & !FNAFGameManager.GameState.InJumpscare & FNAFGameManager.GameState.InCams )
					{
						FNAFGameManager.GameState.CloseCams();
						FNAFGameManager.GameState.InJumpscare = true;
						Jumpscare();
					}
				}
			}
			else if ( MovementOpportunity >= MovementDelay )
			{
				//Log.Info( MovementOpportunity );
				MovementOpportunity = 0;
				int Roll = new Random().Next( 0, 21 );
				//Log.Info( Roll );
				//Log.Info( CurrentAI );
				if ( (Roll < CurrentAI | forcemove) ) //& !FNAFGameManager.GameState.DEV 
				{
					var Moves = AIIndex[CurrentPos];
					int RandomMove = new Random().Next( 0, Moves.Count() );
					var Target = Moves[RandomMove];
					if ( CurrentPos == "office" )
					{
						if ( FNAFGameManager.GameState.RightDoor.IsClosed() )
						{
							Target = AIIndex[Target][0];
						}
						else
						{
							FNAFGameManager.GameState.RightDoorButton.Locked = true;
							ChangePos( "inoffice" );
							ReadyToScare = true;
							Sound.Play( FNAFGameManager.GameState.breathingsound, Object.WorldPosition );
							return;
						}
					}
					string spawncheck = CurrentPos == "spawn" ? "onea" : CurrentPos;
					string spawncheck2 = CurrentPos == "sevenalt" ? "seven" : CurrentPos;
					if ( FNAFGameManager.GameState.CurCam == Target | FNAFGameManager.GameState.CurCam == spawncheck | FNAFGameManager.GameState.CurCam == spawncheck2 )
					{
						FNAFGameManager.GameState.CameraInterference = 0;
					}
					ChangePos( Target );
					return;
				}
			}
			return;
		}
	}
}
