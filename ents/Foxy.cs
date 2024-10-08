using FNAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF
{
	public class FNAFFoxy : FNAFAnimatronic
	{
		public GameObject CurtainObject;
		public SkinnedModelRenderer CurtainModel;
		public Dictionary<string, int> CurtainAnimIndex;
		public TimeSince Cooldown;
		public float ThinkDelay;
		public bool ReadyToRun;
		public bool Running;
		public TimeSince RunDelay;
		public int RandomDelay;
		public int PowerStack;
		public SoundEvent RunSound;
		public SoundHandle RunHandle;
		public SoundEvent PoundSound;
		public SoundHandle PoundHandle;
		new public Dictionary<string, string> AIIndex;
		public FNAFFoxy( Scene scene, int night = 1, int diffoverride = -1 )
		{
			Setup( scene, night, diffoverride );
			AnimIndex = new Dictionary<string, int>
			{
				{ "spawn", 0 },
				{ "one", 1 },
				{ "two", 2 },
				{ "runstart", 3 },
				{ "transition", 3 },
				{ "office", 4 }
			};
			CurtainAnimIndex = new Dictionary<string, int>
			{
				{ "spawn", 0 },
				{ "one", 1 },
				{ "two", 2 },
				{ "runstart", 2 },
				{ "transition", 2 },
				{ "office", 2 }
			};
			AIIndex = new Dictionary<string, string> 
			{
				{ "spawn", "one" },
				{ "one", "two" },
				{ "two", "runstart" },
				{ "runstart", "transition" },
				{ "transition", "office" },
				{ "office", "spawn" }
			};
			Difficulty = new Dictionary<int, int>
			{
				{ 1, 0 },
				{ 2, 1 },
				{ 3, 2 },
				{ 4, 6 },
				{ 5, 5 },
				{ 6, 16 },
				{ 7, 0 }
			};
			AIPositions = new Dictionary<string, Transform>
			{
				{ "spawn", new Transform(new Vector3(709, -760, 75), new Angles(0, 270, 0).ToRotation()) },
				{ "one", new Transform(new Vector3(711.75f, -810.25f, 75), new Angles(0, 287.682f, 0)) },
				{ "two", new Transform(new Vector3(682.75f, -868.25f, 63.965f), new Angles(0, 225, 0)) },
				{ "runstart", new Transform(new Vector3(592, -1043, 63.965f), new Angles(0, 180, 0)) },
				{ "transition", new Transform(new Vector3(-28, -1037, 63.965f), new Angles(0, 215, 0)) },
				{ "office", new Transform(new Vector3(-72.75f, -1076.5f, 63.965f), new Angles(0, 270, 0)) }
			};
			Model.Model = Sandbox.Model.Load( "models/alejenus/fnafvr/foxy/foxyvr.vmdl" );
			CurtainObject = LocalScene.CreateObject( true );
			CurtainObject.WorldPosition = new Vector3( 708, -754.25f, 79.75f );
			CurtainObject.WorldRotation = new Angles( 0, -90, 0 );
			CurtainModel = CurtainObject.Components.Create<SkinnedModelRenderer>( true );
			CurtainModel.Model = Sandbox.Model.Load( "models/alejenus/fnafvr/stage/curtain.vmdl" );
			ChangePos( "spawn", false );
			MovementDelay = 5.01f;
			MovementOpportunity = -5;
			Cooldown = -5;
			ThinkDelay = 1;
			Running = false;
			ReadyToRun = false;
			RandomDelay = 5;
			PowerStack = 1;
			RunSound = new SoundEvent();
			RunSound.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/running.sound" ) };
			PoundSound = new SoundEvent();
			PoundSound.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/pounding.sound" ) };
		}
		public override void ChangePos( string pos, bool hideitem = true )
		{
			base.ChangePos( pos, hideitem );
			CurtainModel.SceneModel.SetAnimParameter( "state", CurtainAnimIndex[pos] );
		}
		public override void Jumpscare()
		{
			FNAFGameManager.GameState.JumpscareLight.Enabled = true;
			Sandbox.Services.Stats.Increment( "foxykills", 1 );
			var sd = new SoundEvent();
			CameraUI.disable = true;
			sd.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/xscream.sound" ) };
			Sound.Play( sd, FNAFGameManager.GameState.OfficeCamera.WorldPosition );
			FNAFGameManager.GameState.JumpscareDoShake = false;
			FNAFGameManager.GameState.InJumpscare = true;
			//Object.WorldPosition = new Vector3( -75f, -1069, 64f );
			//Object.WorldRotation = new Angles( 0, 270, 0 );
			Model.SceneModel.SetMaterialGroup( "default" );
			Model.SceneModel.SetAnimParameter( "pose", 4 );
			FNAFGameManager.GameState.JumpscareBootTimer = 0;
			if ( FNAFGameManager.GameState.LeftLightButton.TrueState )
				FNAFGameManager.GameState.LeftLightButton.Use();
			if ( FNAFGameManager.GameState.RightLightButton.TrueState )
				FNAFGameManager.GameState.RightLightButton.Use();
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
			if ( Lobotomize ) { return; }
			if ( FNAFGameManager.GameState.PowerOutage | CurrentAI == -1 | FNAFGameManager.GameState.PowerOutage ) { return; }
			if ( ReadyToRun )
			{
				if ( (FNAFGameManager.GameState.CurCam == "twoa") )
				{
					Running = true;
					ReadyToRun = false;
					RunHandle = Sound.Play( RunSound, new Vector3( 160, -1045, 112 ) );
				}
				else if ( RunDelay >= RandomDelay )
				{
					Running = true;
					ReadyToRun = false;
					//RunHandle = Sound.Play( RunSound, new Vector3( 160, -1045, 112 ) );
				}
				return;
			}
			if ( Running )
			{
				var t = Object.WorldPosition.Distance( AIPositions["transition"].Position );
				if ( t < 16 )
				{
					Running = false;
					if ( RunHandle != null )
						RunHandle.Stop();
					if ( !FNAFGameManager.GameState.LeftDoor.IsClosed() )
					{
						ChangePos( "office" );
						//todo : jumpscare stuff
						Jumpscare();
						Lobotomize = true;
						return;
					}
					else
					{
						ChangePos( "spawn" );
						FNAFGameManager.GameState.Power -= PowerStack * 10;
						PowerStack += 5;
						PoundHandle = Sound.Play( PoundSound, FNAFGameManager.GameState.LeftDoor.Object.WorldPosition );
						return;
					}
				}
				else
				{
					Object.WorldPosition += new Vector3(
						AIPositions["transition"].Position.x - Object.WorldPosition.x,
						AIPositions["transition"].Position.y - Object.WorldPosition.y,
						AIPositions["transition"].Position.z - Object.WorldPosition.z ).Normal * 5;
					return;
				}
			}
			if ( FNAFGameManager.GameState.InCams )
			{
				Cooldown = -new Random().Next( 1, 4 );
			}
			if ( Cooldown < ThinkDelay ) { return; }
			int c = new Random().Next( 0, 21 );
			Cooldown = 0;
			ThinkDelay = new Random().Next( 1, 15 );
			if ( (forcemove | c < CurrentAI) & !Running & !ReadyToRun )
			{
				if ( FNAFGameManager.GameState.InCams ) { return; }
				string move = AIIndex[CurrentPos];
				ChangePos( move );
				if ( move == "runstart" )
				{
					ReadyToRun = true;
					RandomDelay = new Random().Next( 4, 26 );
					RunDelay = 0;
				}
			}
			return;
		}
	}
}
