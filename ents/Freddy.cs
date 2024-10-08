using FNAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF
{
	public class FNAFFreddy : FNAFAnimatronic
	{
		public TimeSince Cooldown;
		public bool OnCooldown;
		public string TempPos;
		public bool DoPoweroutSequence;
		public int PoweroutState;
		public TimeSince PoweroutForcer;
		public int PoweroutRoll;
		public SoundHandle powerouthandle;
		public SoundEvent poweroutsoundEvent;
		public TimeSince EyeGlowTimer;
		public FNAFFreddy( Scene scene, int night = 1, int diffoverride = -1 )
		{
			Setup( scene, night, diffoverride );
			AnimIndex = new Dictionary<string, int>
			{
				{ "spawn", 1 },
				{ "oneb", 2 },
				{ "seven", 3 },
				{ "six", 4 },
				{ "foura", 5 },
				{ "office", 6 },
				{ "inoffice", 7 }
			};
			AIIndex = new Dictionary<string, List<string>>
			{
				{ "spawn", new List<string> { "oneb" } },
				{ "oneb", new List<string> { "seven" } },
				{ "seven", new List<string> { "six" } },
				{ "six", new List<string> { "foura" } },
				{ "foura", new List<string> { "office" } },
				{ "office", new List<string> { "foura" } }
			};
			Difficulty = new Dictionary<int, int>
			{
				{ 1, 0 },
				{ 2, 0 },
				{ 3, 1 },
				{ 4, new Random().Next( 1, 2 ) },
				{ 5, 3 },
				{ 6, 4 },
				{ 7, 0 }
			};
			AIPositions = new Dictionary<string, Transform>
			{
				{ "spawn", new Transform(new Vector3(1248, -1232, 96), new Angles(0, 180, 0).ToRotation()) },
				{ "oneb", new Transform(new Vector3(856, -1281, 64), new Angles(0, 42, 0).ToRotation()) },
				{ "seven", new Transform(new Vector3(956.217f, -1857.37f, 63.3115f), new Angles(0, 55.2244f, -15).ToRotation()) },
				{ "six", new Transform(new Vector3(32, -1475, 64), new Angles(0, 345, 0).ToRotation()) },
				{ "foura", new Transform(new Vector3(267, -1244, 64), new Angles(0, 180, 0).ToRotation()) },
				{ "office", new Transform(new Vector3(-206, -1238, 64), new Angles(0, 20.579f, 0).ToRotation()) },
				{ "inoffice", new Transform(new Vector3(-180, -1156, 64), new Angles(0, 0, 0).ToRotation()) },
				{ "powerout", new Transform(new Vector3(-48, -1056, 64), new Angles(0, 225, 0)) }
			};
			Model.Model = Sandbox.Model.Load( "models/alejenus/fnafvr/freddy/freddyvr.vmdl" );
			HeldItemModel.Model = Sandbox.Model.Load( "models/alejenus/fnafvr/freddy/freddyvr_mic.vmdl" );
			HeldItem.WorldPosition = new Vector3( 16, -11.7f, 47 );
			CurrentAI = Difficulty[night];
			ChangePos( "spawn", false );
		}
		public override void ChangePos( string pos, bool hideitem = true )
		{
			base.ChangePos(pos, hideitem);
			if ( pos == "spawn" | pos == "inoffice" )
			{
				Model.SceneModel.SetMaterialGroup( "default" );
			}
			else
			{
				Model.SceneModel.SetMaterialGroup( "creepy" );
			}
		}
		public override void Jumpscare()
		{
			Sandbox.Services.Stats.Increment( "freddykills", 1 );
			base.Jumpscare();
		}
		public override void Think( bool forcemove = false, string cam = "" )
		{
			base.Think( forcemove, cam );
			if ( DoPoweroutSequence )
			{
				
				if( PoweroutState == 0 )
				{
					PoweroutForcer = 0;
					PoweroutState = 1;
					PoweroutRoll = new Random().Next( 3, 6 );
					poweroutsoundEvent = new SoundEvent();
					poweroutsoundEvent.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/lightsout.sound" ) };
				}
				else if ( PoweroutState == 1 )
				{
					if( PoweroutForcer >= PoweroutRoll )
					{
						PoweroutForcer = 0;
						PoweroutState = 2;
						PoweroutRoll = new Random().Next( 8, 18 );
						Sound.Play( MoveSound, FNAFGameManager.GameState.OfficeCamera.WorldPosition );
					}
				}
				else if ( PoweroutState == 2 )
				{
					if ( PoweroutForcer >= PoweroutRoll )
					{
						PoweroutForcer = 0;
						PoweroutState = 3;
						PoweroutRoll = new Random().Next( 9, 30 );
						Object.WorldPosition = new Vector3( -65, -1068, 64 );
						Object.WorldRotation = new Angles( 0, -135, 0 );
						Model.SceneModel.SetMaterialGroup( "glow" );
						powerouthandle = Sound.Play( poweroutsoundEvent, FNAFGameManager.GameState.OfficeCamera.WorldPosition );
					}
				}
				else if ( PoweroutState == 3 )
				{
					if ( EyeGlowTimer > 0.1f )
					{
						EyeGlowTimer = 0;
						Model.SceneModel.SetMaterialGroup( new Random().Next( 0, 5 ) >= 2 ? "glow" : "default" );
					}
					if ( PoweroutForcer >= PoweroutRoll )
					{
						PoweroutForcer = 0;
						PoweroutState = 4;
						PoweroutRoll = new Random().Next( 5, 10 );
						Model.SceneModel.SetMaterialGroup( "default" );
						FNAFGameManager.GameState.OfficePoweroutLight.Enabled = false;
						FNAFGameManager.GameState.RightDoorLight.Enabled = false;
						FNAFGameManager.GameState.LeftDoorLight.Enabled = false;
						powerouthandle.Stop();
						var sound = new SoundEvent();
						sound.Sounds = new List<SoundFile> { SoundFile.Load("sounds/freddypoweroutbuzz.sound") };
						Sound.Play(sound, FNAFGameManager.GameState.OfficeCamera.WorldPosition);
						FNAFGameManager.GameState.OfficeCamera.ZFar = 65;
					}
				} else
				{
					if ( PoweroutForcer >= PoweroutRoll )
					{
						PoweroutForcer = -99;
						Jumpscare();
					}
				}
			}
			if ( cam != "" )
			{
				ChangePos( cam );
				return;
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
				} else
				{
					if ( !FNAFGameManager.GameState.InCams )
					{
						Jumpscare();
					}
				}
				return;
			}
			else if ( (MovementOpportunity >= MovementDelay | forcemove) 
				& FNAFGameManager.GameState.Bonnie.CurrentPos != "spawn"
				& FNAFGameManager.GameState.Chica.CurrentPos != "spawn")
			{
				//Log.Info( MovementOpportunity );
				string spawncheck = CurrentPos == "spawn" ? "onea" : CurrentPos;
				MovementOpportunity = 0;
				int Roll = new Random().Next( 0, 21 );
				var Moves = AIIndex[CurrentPos];
				int RandomMove = new Random().Next( 0, Moves.Count() );
				var Target = Moves[RandomMove];
				string spawncheck2 = CurrentPos == "office" ? "fourb" : CurrentPos;
				//Log.Info( Roll );
				//Log.Info( CurrentAI );
				if ( (FNAFGameManager.GameState.CurCam == Target | 
					FNAFGameManager.GameState.CurCam == spawncheck | 
					FNAFGameManager.GameState.CurCam == CurrentPos |
					FNAFGameManager.GameState.CurCam == spawncheck2 ) & FNAFGameManager.GameState.InCams )
				{
					return;
				}
				if ( OnCooldown )
				{
					if ( Cooldown > 0 )
					{
						if ( (FNAFGameManager.GameState.CurCam == Target |
						FNAFGameManager.GameState.CurCam == spawncheck |
						FNAFGameManager.GameState.CurCam == CurrentPos |
						FNAFGameManager.GameState.CurCam == spawncheck2) & FNAFGameManager.GameState.InCams )
						{
							return;
						}
						ChangePos( TempPos );
						OnCooldown = false;
						return;
					}
				}
				else if ( (Roll < CurrentAI | forcemove) ) //& !FNAFGameManager.GameState.DEV 
				{
					if ( FNAFGameManager.GameState.InCams & (FNAFGameManager.GameState.CurCam != CurrentPos) )
						return;
					if ( CurrentPos == "office" )
					{
						if ( FNAFGameManager.GameState.InCams & FNAFGameManager.GameState.CurCam != "fourb" & !FNAFGameManager.GameState.RightDoor.IsClosed() )
						{
							FNAFGameManager.GameState.RightDoorButton.Locked = true;
							ChangePos( "inoffice" );
							ReadyToScare = true;
							return;
						}
					}
					Cooldown = -(1000 - CurrentAI * 100) / 60;
					TempPos = Target;
					OnCooldown = true;
					return;
				}
			}
		}
	}
}
