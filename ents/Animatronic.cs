using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAF
{
	public class FNAFAnimatronic
	{
		public Scene LocalScene;
		public GameObject Object;
		public GameObject HeldItem;
		public SkinnedModelRenderer Model;
		public ModelRenderer HeldItemModel;
		public Dictionary<string, int> AnimIndex;
		public Dictionary<string, List<string>> AIIndex;
		public Dictionary<int, int> Difficulty;
		public Dictionary<string, Transform> AIPositions;
		public SoundEvent MoveSound;
		public SoundHandle MoveSoundHandle;
		public int CurrentAI;
		public bool Overridden;
		public TimeSince MovementOpportunity;
		public string CurrentPos;
		public bool ReadyToScare;
		public bool CamBuffer;
		public bool Lobotomize;
		public float MovementDelay;
		public void Setup( Scene scene, int night = 1, int diffoverride = -1 )//used to be public FNAFAnimatronic() but it was being annoying
		{
			LocalScene = scene;
			AnimIndex = new Dictionary<string, int> { };
			AIIndex = new Dictionary<string, List<string>> { };
			Difficulty = new Dictionary<int, int> { };
			AIPositions = new Dictionary<string, Transform> { };
			Object = LocalScene.CreateObject( true );
			Model = Object.Components.Create<SkinnedModelRenderer>( true );
			HeldItem = LocalScene.CreateObject( true );
			HeldItem.SetParent( Object, true );
			HeldItemModel = HeldItem.Components.Create<SkinnedModelRenderer>( true );
			CurrentAI = 0;//Difficulty[night];
			MovementOpportunity = -5;
		}
		public virtual void ChangePos( string pos, bool hideitem = true )
		{
			Model.Enabled = false;
			Object.WorldPosition = AIPositions[pos].Position;
			Object.WorldRotation = AIPositions[pos].Rotation;
			if ( hideitem )
			{
				HeldItem.Destroy();
				MoveSoundHandle = Sound.Play( MoveSound, FNAFGameManager.GameState.OfficeCamera.WorldPosition );
				if( MoveSoundHandle != null )//hack, i dont think this was actually a problem
					MoveSoundHandle.Volume = 0.5f;
			}
			CurrentPos = pos;
			Model.Enabled = true;
			Model.SceneModel.SetAnimParameter( "pose", AnimIndex[pos] );
		}
		public virtual void NewHour( int hour ){}
		public virtual void Jumpscare()
		{
			FNAFGameManager.GameState.JumpscareLight.Enabled = true;
			var sd = new SoundEvent();
			CameraUI.disable = true;
			sd.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/xscream.sound" ) };
			Sound.Play( sd, FNAFGameManager.GameState.OfficeCamera.WorldPosition );
			FNAFGameManager.GameState.InJumpscare = true;
			Object.WorldPosition = new Vector3( -120f, -1152, 54f );
			Object.WorldRotation = new Angles( 0, 180, 0 );
			Model.SceneModel.SetMaterialGroup( "default" );
			Model.SceneModel.SetAnimParameter( "pose", 8 );
			FNAFGameManager.GameState.JumpscareBootTimer = 0;
			if ( FNAFGameManager.GameState.LeftLightButton.TrueState )
				FNAFGameManager.GameState.LeftLightButton.Use();
			if ( FNAFGameManager.GameState.RightLightButton.TrueState )
				FNAFGameManager.GameState.RightLightButton.Use();
		}
		public virtual void Think( bool forcemove = false, string cam = "" )
		{
			if ( Lobotomize ) { return; }
			if ( FNAFGameManager.GameState.InJumpscare | FNAFGameManager.GameState.PowerOutage )
				return;
		}
	}
}
