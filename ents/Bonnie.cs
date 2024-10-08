using FNAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF
{
	public class FNAFBonnie : FNAFAnimatronic
	{
		public TimeSince StingerTimer;
		public int Tweaking;
		public FNAFBonnie( Scene scene, int night = 1, int diffoverride = -1 )
		{
			Setup( scene, night, diffoverride );
			AnimIndex = new Dictionary<string, int>
			{
				{ "spawn", 1 },
				{ "oneb", 2 },
				{ "five", 3 },
				{ "fivealt", 3 },
				{ "three", 4 },
				{ "twoa", 5 },
				{ "twob", 6 },
				{ "office", 7 },
				{ "inoffice", 8 }
			};
			AIIndex = new Dictionary<string, List<string>> {
				{ "spawn", new List<string> { "oneb", "five", "oneb" } },
				{ "oneb", new List<string> { "five", "twoa", "twoa" } },
				{ "fivealt", new List<string> { "oneb", "twoa" } },
				{ "five", new List<string> { "oneb", "twoa" } },
				{ "three", new List<string> { "twob", "office" } },
				{ "twoa", new List<string> { "three", "twob", "twob" } },
				{ "twob", new List<string> { "office", "three", "office" } },
				{ "office", new List<string> { "oneb" } }
			};
			Difficulty = new Dictionary<int, int>
			{
				{ 1, 0 },
				{ 2, 3 },
				{ 3, 0 },
				{ 4, 2 },
				{ 5, 5 },
				{ 6, 10 },
				{ 7, 0 }
			};
			AIPositions = new Dictionary<string, Transform>
			{
				{ "spawn", new Transform(new Vector3(1264, -1152, 96), new Angles(0, 180, 0).ToRotation()) },
				{ "oneb", new Transform(new Vector3(848, -1069, 64), new Angles(0, 347.016f, 0))},
				{ "five", new Transform(new Vector3(983, -800, 64.25f), new Angles(0, 181.032f, 0))},
				{ "three", new Transform(new Vector3(349, -881, 64), new Angles(0, 270, 0)) },
				{ "twob", new Transform(new Vector3(-229, -1058, 64), new Angles(0, 0, 0)) },
				{ "twoa", new Transform(new Vector3(464, -1041, 64), new Angles(0, 180, 0)) },
				{ "office", new Transform(new Vector3(31, -1073, 64), new Angles(0, -146, 0)) },
				{ "inoffice", new Transform(new Vector3(-180, -1115, 64), new Angles(0, 330, 0)) },
				{ "fivealt", new Transform(new Vector3(891.393f, -792, 83), new Angles(0, -180, 0)) }
			};
			Model.Model = Sandbox.Model.Load( "models/alejenus/fnafvr/bonnie/bonnievr.vmdl" );
			HeldItemModel.Model = Sandbox.Model.Load( "models/alejenus/fnafvr/bonnie/bonnievr_guitar.vmdl" );
			HeldItemModel.WorldPosition = new Vector3( 13.5f, 11, 49 );
			HeldItemModel.WorldRotation = new Angles( 0, 0, 32.5f );
			CurrentAI = Difficulty[night];
			MoveSound = FNAFGameManager.GameState.stepsound;
			Tweaking = 0;
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
			Sandbox.Services.Stats.Increment( "bonniekills", 1 );
			base.Jumpscare();
		}
		public override void NewHour( int hour )
		{
			if ( hour > 1 & hour < 5 )
			{
				CurrentAI += 1;
			}
		}
		public override void Think( bool forcemove = false, string cam = "" )
		{
			base.Think(forcemove, cam);
			if ( CurrentPos == "office" & FNAFGameManager.GameState.LeftLightButton.State & StingerTimer > 10 )
			{
				Sound.Play( FNAFGameManager.GameState.stingersound, FNAFGameManager.GameState.LeftLightButton.Object.WorldPosition );
				StingerTimer = 0;
			}
			if ( cam != "" )
			{
				ChangePos( cam );
				return;
			}
			if ( FNAFGameManager.GameState.InCams & FNAFGameManager.GameState.CurCam != "twob" & Tweaking == 0 & FNAFGameManager.night > 4 & CurrentPos == "twob" )
			{
				var rand = new Random().Next( 0, 1000 );
				if ( rand == 42 )
				{
					Tweaking = 1;
					Model.SceneModel.SetAnimParameter( "pose", 10 );
				}
			}
			else if ( FNAFGameManager.GameState.InCams & FNAFGameManager.GameState.CurCam == "twob" & Tweaking == 1 )
			{
				Tweaking = 2;
				FNAFGameManager.GameState.tweakingsound = Sound.Play( FNAFGameManager.GameState.tweakingsounds, FNAFGameManager.GameState.OfficeCamera.WorldPosition );
			}
			else if ( Tweaking == 2 & (!FNAFGameManager.GameState.InCams | FNAFGameManager.GameState.CurCam != "twob" | CurrentPos != "twob") )
			{
				Model.SceneModel.SetAnimParameter( "pose", AnimIndex[CurrentPos] );
				Tweaking = 0;
				FNAFGameManager.GameState.tweakingsound.Stop();
			}
			else if ( CurrentPos != "twob" )
			{
				Model.SceneModel.SetAnimParameter( "pose", AnimIndex[CurrentPos] );
				Tweaking = 0;
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
				//Log.Info(MovementOpportunity);
				MovementOpportunity = 0;
				int Roll = new Random().Next( 0, 21 );
				//Log.Info(Roll);
				//Log.Info(CurrentAI);
				if ( (Roll < CurrentAI | forcemove) ) //& !FNAFGameManager.GameState.DEV 
				{
					var Moves = AIIndex[CurrentPos];
					int RandomMove = new Random().Next( 0, Moves.Count() );
					var Target = Moves[RandomMove];
					if ( CurrentPos == "office" )
					{
						if ( FNAFGameManager.GameState.LeftDoor.IsClosed() )
						{
							Target = AIIndex[Target][0];
						}
						else
						{
							FNAFGameManager.GameState.LeftDoorButton.Locked = true;
							ChangePos( "inoffice" );
							ReadyToScare = true;
							Sound.Play( FNAFGameManager.GameState.breathingsound, Object.WorldPosition );
							return;
						}
					}
					string spawncheck = CurrentPos == "spawn" ? "onea" : CurrentPos;
					string spawncheck2 = CurrentPos == "fivealt" ? "five" : CurrentPos;
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
