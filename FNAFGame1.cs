using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using static Sandbox.PhysicsContact;
namespace FNAF
{
	public struct FNAFGameState
	{
		public Scene LocalScene;
		public bool DEV;
		public FNAFFreddy Freddy;
		public FNAFBonnie Bonnie;
		public FNAFChica Chica;
		public FNAFFoxy Foxy;
		public FNAFEndo Endo;
		public FNAFGoldenFreddy GoldenFreddy;
		public int Power;
		public int PowerConsumption;
		public TimeSince LastDrain;
		public FNAFDoor LeftDoor;
		public FNAFDoor RightDoor;
		public TimeSince GameTimer;
		public int Hour;
		public bool PowerOutage;
		public SpotLight LeftLight;
		public SpotLight RightLight;
		public SpotLight LeftDoorLight;
		public SpotLight RightDoorLight;
		public SpotLight OfficeLight;
		public SpotLight OfficePoweroutLight;
		public FNAFButton LeftDoorButton;
		public FNAFButton RightDoorButton;
		public FNAFButton LeftLightButton;
		public FNAFButton RightLightButton;
		public CameraComponent OfficeCamera;
		public float OfficeCamOffset;
		public float CamsSwivel;
		public float CamSwivelDir;
		public Dictionary<string, CameraComponent> Cams;
		public Dictionary<string, Angles> CamRotations;
		public Dictionary<string, FilmGrain> CamStatic;
		public Dictionary<string, ColorAdjustments> CamColor;
		public Dictionary<string, SpotLight> CamLights;
		public FNAFPlushie FreddyPlush;
		public FNAFPlushie BonniePlush;
		public FNAFPlushie ChicaPlush;
		public FNAFPlushie FoxyPlush;
		public FNAFPlushie GFreddyPlush;
		public bool InCams;
		public string CurCam;
		public string DesiredCam;
		public bool CamChanged;
		public SkinnedModelRenderer FreddyPoster;
		public FNAFTablet Tablet;
		public TimeSince LastCam;
		public bool InJumpscare;
		public TimeSince JumpscareBootTimer;
		public bool PostJumpscareCheck;
		public TimeSince HallucinationTimer;
		static public RootPanel panel;
		static public bool UISetup;
		public SpotLight CamFiveLight;
		public TimeSince CameraInterference;
		public FNAFFan Fan;
		public FNAFPhone Phone;
		public ScreenPanel RPanel;
		public CameraUI CamUI;
		public Power PowerUI;
		public NightTime TimeUI;
		public GameOver GameOverUI;
		public GameWin GameWinUI;
		public SkinnedModelRenderer GFreddyVideoPanel;
		public bool VideoPlaying;
		public VideoPlayer GFreddyVideo;
		public Material GFreddyVideoMat;
		public SoundEvent GFreddyVidSound;
		public SoundHandle GFreddyVidSoundHandle;
		public int GFreddyVidStatus;
		public bool JumpscareDoShake;
		public TimeSince ScaryEventTimer;
		public SpotLight JumpscareLight;
		public bool DidPowerout;
		public SoundEvent hallucinationsound;
		public SoundHandle hallucinationhandle;
		public SoundEvent camswivelsound;
		public SoundHandle camswivelhandle;
		public SoundEvent camblipsound;
		public SoundHandle cambliphandle;
		public SoundEvent caminterferencesound;
		public SoundHandle caminterferencehandle;
		public SoundEvent camusesound;
		public SoundHandle camusehandle;
		public SoundEvent favorsound;
		public SoundHandle favorhandle;
		public SoundEvent stingersound;
		public SoundEvent scarysounds;
		public SoundEvent breathingsound;
		public SoundEvent stepsound;
		public SoundEvent tweakingsounds;
		public SoundHandle tweakingsound;
		public FNAFGameState( Scene scene, int night, bool dev = false)
		{
			LocalScene = scene;
			DidPowerout = false;
			DEV = dev;
			PostJumpscareCheck = false;
			var poster = LocalScene.CreateObject();
			FreddyPoster = poster.Components.Create<SkinnedModelRenderer>();
			FreddyPoster.Model = Sandbox.Model.Load( "models/fnaf/freddyposter.vmdl" );
			poster.WorldPosition = new Vector3(-253, -1032.45f, 130);
			poster.WorldRotation = new Angles(0, -90, 0);
			Freddy = new FNAFFreddy( LocalScene, night );
			Bonnie = new FNAFBonnie( LocalScene, night );
			Chica = new FNAFChica( LocalScene, night );
			Foxy = new FNAFFoxy( LocalScene, night );
			GoldenFreddy = new FNAFGoldenFreddy( LocalScene );
			JumpscareDoShake = true;
			if (night == 7)
			{
				Freddy.CurrentAI = FNAFGameManager.FreddyLevel;
				Freddy.Overridden = true;
				Bonnie.CurrentAI = FNAFGameManager.BonnieLevel;
				Bonnie.Overridden = true;
				Chica.CurrentAI = FNAFGameManager.ChicaLevel;
				Chica.Overridden = true;
				Foxy.CurrentAI = FNAFGameManager.FoxyLevel;
				Foxy.Overridden = true;
			}
			ScaryEventTimer = -7;
			Endo = new FNAFEndo( LocalScene );
			Tablet = new FNAFTablet( LocalScene, new Vector3( 3, 0, 0 ) ); //new Vector3( -148, -1152, 128 )
			Fan = new FNAFFan( LocalScene );
			Phone = new FNAFPhone( LocalScene, night );
			InCams = false;
			DesiredCam = "";
			CamChanged = false;
			VideoPlaying = false;
			FreddyPlush = new FNAFPlushie(new Vector3(7, -1165, 136), new Angles(0, 76.8801f, 15), LocalScene, "models/plush/freddy.vmdl" );
			BonniePlush = new FNAFPlushie(new Vector3(6, -1151, 136), new Angles(0, 97.355f, 15), LocalScene, "models/plush/bonnie.vmdl" );
			ChicaPlush = new FNAFPlushie(new Vector3(-4, -1180, 123), new Angles(0, 56.3262f, 15), LocalScene, "models/plush/chica.vmdl");
			FoxyPlush = new FNAFPlushie(new Vector3(0, -1114, 139.36f), new Angles(0, 120, 15), LocalScene, "models/plush/foxy.vmdl");
			GFreddyPlush = new FNAFPlushie(new Vector3(-15, -1121, 104.773f), new Angles(0, 106.88f, 15), LocalScene, "models/plush/freddy.vmdl");
			GFreddyPlush.Model.MaterialGroup = "golden";
			LastCam = 0;
			InJumpscare = false;
			JumpscareBootTimer = -999999999; //hack, works so whatever
			PowerConsumption = 0;
			HallucinationTimer = 10;
			CameraInterference = 3;
			var save = ProgressTracker.Fetch();
			if(save.bestfreddy == 20 | DEV)
				FreddyPlush.Enable();
			if(save.bestbonnie == 20 | DEV )
				BonniePlush.Enable();
			if(save.bestchica == 20 | DEV )
				ChicaPlush.Enable();
			if(save.bestfoxy == 20 | DEV )
				FoxyPlush.Enable();
			if(save.fourtwenty | DEV )
				GFreddyPlush.Enable();

			//in case a facepunch dev looks at this:
			//why the hell would i ever place objects in the scene editor?
			//if i do, i have to do a million finds to get a hold of my objects,
			//when i could instead make them procedurally.
			//the scene editor is useful for figuring out coordinates, but then
			//they are swiftly deleted and put here.
			//the finds are ugly as shit, uglier than this.
			//its dumb.
			//it would be a lot better if i could index a big table,
			//eg. i have an object named "fuck", i should be able to get it
			//something like Scene.Objects["fuck"]. the FindByName method is stupid.

			CamFiveLight = LocalScene.CreateObject().Components.Create<SpotLight>();
			CamFiveLight.Enabled = false;
			CamFiveLight.WorldPosition = new Vector3( 1064, -863, 152 );
			CamFiveLight.WorldRotation = new Angles( 24.4361f, 136.385f, 4.86709f );
			CamFiveLight.ConeInner = 10;
			CamFiveLight.ConeOuter = 35;
			CamFiveLight.LightColor = new Color( 0.89411764705f, 0.89411764705f, 0.89411764705f );
			CamFiveLight.Shadows = true;
			CamFiveLight.Attenuation = 4;
			CamFiveLight.Enabled = true;

			LeftDoor = new FNAFDoor( new Vector3( -96, -1097, 64 ), new Angles( 0, -90, 0 ), LocalScene );
			RightDoor = new FNAFDoor( new Vector3( -96, -1207, 64 ), new Angles( 0, 90, 0 ), LocalScene );
			LeftLight = LocalScene.CreateObject().Components.Create<SpotLight>();
			RightLight = LocalScene.CreateObject().Components.Create<SpotLight>();
			LeftLight.WorldPosition = new Vector3( -193, -1089, 113 );
			LeftLight.WorldRotation = new Angles( 0, 22.3519f, 0 );
			LeftLight.ConeInner = 15;
			LeftLight.ConeOuter = 35;
			LeftLight.LightColor = new Color( 0.5686274509f, 0.5686274509f, 0.9372549019f );
			LeftLight.Shadows = true;
			LeftLight.Attenuation = 2;
			LeftLight.Enabled = false;
			RightLight.WorldPosition = new Vector3( -193, -1214.25f, 113 );
			RightLight.WorldRotation = new Angles( 0, 337.648f, 0 );
			RightLight.ConeInner = 15;
			RightLight.ConeOuter = 35;
			RightLight.LightColor = new Color( 0.5686274509f, 0.5686274509f, 0.9372549019f );
			RightLight.Shadows = true;
			RightLight.Attenuation = 2;
			RightLight.Enabled = false;
			LeftDoorLight = LocalScene.CreateObject().Components.Create<SpotLight>();
			LeftDoorLight.Enabled = false;
			LeftDoorLight.WorldPosition = new Vector3( -96, -1108, 162 );
			LeftDoorLight.WorldRotation = new Angles( 90, 270, 0 );
			LeftDoorLight.ConeInner = 5;
			LeftDoorLight.ConeOuter = 25;
			LeftDoorLight.LightColor = new Color( 1, 1, 1 );
			LeftDoorLight.Shadows = false;
			LeftDoorLight.Attenuation = 100;
			LeftDoorLight.Enabled = true;
			RightDoorLight = LocalScene.CreateObject().Components.Create<SpotLight>();
			RightDoorLight.Enabled = false;
			RightDoorLight.WorldPosition = new Vector3( -96, -1197, 162 );
			RightDoorLight.WorldRotation = new Angles( 90, 270, 0 );
			RightDoorLight.ConeInner = 5;
			RightDoorLight.ConeOuter = 25;
			RightDoorLight.LightColor = new Color( 1, 1, 1 );
			RightDoorLight.Shadows = false;
			RightDoorLight.Attenuation = 100;
			RightDoorLight.Enabled = true;
			OfficeLight = LocalScene.CreateObject().Components.Create<SpotLight>();
			OfficeLight.Enabled = false;
			OfficeLight.WorldPosition = new Vector3( -37.624f, -1152, 175.5f );
			OfficeLight.WorldRotation = new Angles( 60, 0, 90 );
			OfficeLight.ConeInner = 15;
			OfficeLight.ConeOuter = 66;
			OfficeLight.Radius = 103.7f;
			OfficeLight.LightColor = new Color( 0.83921568627f, 0.6196078431f, 0.4549019607f );
			OfficeLight.Shadows = false;
			OfficeLight.Attenuation = 3.07f;
			OfficeLight.Enabled = true;
			OfficePoweroutLight = LocalScene.CreateObject().Components.Create<SpotLight>();
			OfficePoweroutLight.WorldPosition = new Vector3( -32.5f, -1152, 175.5f );
			OfficePoweroutLight.WorldRotation = new Angles( 90, 270, 0 );
			OfficePoweroutLight.ConeInner = 45;
			OfficePoweroutLight.ConeOuter = 85;
			OfficePoweroutLight.LightColor = new Color( 0.505882352f, 0.631372549019f, 0.8509803921f );
			OfficePoweroutLight.Shadows = false;
			OfficePoweroutLight.Attenuation = 60;
			OfficePoweroutLight.Enabled = false;
			OfficePoweroutLight.Radius = 100;
			LeftDoorButton = new FNAFButton( new Vector3( -55.5f, -1102.5f, 130 ), 
				new Angles( 0, 90, 0 ), 
				new Color( 0, 1, 0 ), 
				new Color( 1, 0, 0 ),
				0,
				true );
			RightDoorButton = new FNAFButton( new Vector3( -55.5f, -1201.5f, 130 ), 
				new Angles( 0, 90, 0 ),
				new Color( 0, 1, 0 ),
				new Color( 1, 0, 0 ),
				0,
				true );
			LeftLightButton = new FNAFButton( new Vector3( -55.5f, -1102.5f, 117.75f ), 
				new Angles( 0, 90, 0 ), 
				new Color( 1, 1, 1 ),
				new Color( 1, 1, 1 ),
				1,
				false, 
				true );
			RightLightButton = new FNAFButton( new Vector3( -55.5f, -1201.5f, 117.75f ), 
				new Angles( 0, 90, 0 ), 
				new Color( 1, 1, 1 ),
				new Color( 1, 1, 1 ),
				1,
				false, 
				true );
			OfficeCamera = LocalScene.CreateObject().Components.Create<CameraComponent>();
			OfficeCamera.FieldOfView = 90;
			OfficeCamera.WorldPosition = new Vector3(-151, -1152, 128);
			OfficeCamera.WorldRotation = new Angles();
			OfficeCamera.ZNear = 0.1f;
			OfficeCamera.IsMainCamera = true;
			OfficeCamera.Priority = 1;
			OfficeCamOffset = 0;
			OfficeCamera.BackgroundColor = Color.Black;
			CamsSwivel = 0;
			CamSwivelDir = 1;
			Tablet.Object.Parent = OfficeCamera.GameObject;
			Tablet.Model.RenderType = ModelRenderer.ShadowRenderType.Off;
			JumpscareLight = LocalScene.CreateObject().Components.Create<SpotLight>();
			JumpscareLight.WorldPosition = new Vector3(-170, -1152, 128);
			JumpscareLight.WorldRotation = new Angles(0, 0, 90);
			JumpscareLight.ConeInner = 33;
			JumpscareLight.ConeOuter = 33;
			JumpscareLight.Attenuation = 35;
			JumpscareLight.Radius = 110;
			JumpscareLight.Enabled = false;
			Cams = new Dictionary<string, CameraComponent>
			{
				{ "onea", LocalScene.CreateObject().Components.Create<CameraComponent>() },
				{ "oneb", LocalScene.CreateObject().Components.Create<CameraComponent>() },
				{ "onec", LocalScene.CreateObject().Components.Create<CameraComponent>() },
				{ "twoa", LocalScene.CreateObject().Components.Create<CameraComponent>() },
				{ "twob", LocalScene.CreateObject().Components.Create<CameraComponent>() },
				{ "three", LocalScene.CreateObject().Components.Create<CameraComponent>() },
				{ "foura", LocalScene.CreateObject().Components.Create<CameraComponent>() },
				{ "fourb", LocalScene.CreateObject().Components.Create<CameraComponent>() },
				{ "five", LocalScene.CreateObject().Components.Create<CameraComponent>() },
				{ "six", LocalScene.CreateObject().Components.Create<CameraComponent>() },
				{ "seven", LocalScene.CreateObject().Components.Create<CameraComponent>() }
			};
			CamLights = new Dictionary<string, SpotLight>
			{
				{ "onea", LocalScene.CreateObject().Components.Create<SpotLight>() },
				{ "oneb", LocalScene.CreateObject().Components.Create<SpotLight>() },
				{ "onec", LocalScene.CreateObject().Components.Create<SpotLight>() },
				{ "twoa", LocalScene.CreateObject().Components.Create<SpotLight>() },
				{ "twoa2", LocalScene.CreateObject().Components.Create<SpotLight>() },
				{ "twob", LocalScene.CreateObject().Components.Create<SpotLight>() },
				{ "three", LocalScene.CreateObject().Components.Create<SpotLight>() },
				{ "foura", LocalScene.CreateObject().Components.Create<SpotLight>() },
				{ "fourb", LocalScene.CreateObject().Components.Create<SpotLight>() },
				{ "five", LocalScene.CreateObject().Components.Create<SpotLight>() },
				{ "six", LocalScene.CreateObject().Components.Create<SpotLight>() },
				{ "seven", LocalScene.CreateObject().Components.Create<SpotLight>() }
			};

			CamStatic = new Dictionary<string, FilmGrain>
			{
				{ "onea", Cams["onea"].Components.Create<FilmGrain>() },
				{ "oneb", Cams["oneb"].Components.Create<FilmGrain>() },
				{ "onec", Cams["onec"].Components.Create<FilmGrain>() },
				{ "twoa", Cams["twoa"].Components.Create<FilmGrain>() },
				{ "twob", Cams["twob"].Components.Create<FilmGrain>() },
				{ "three", Cams["three"].Components.Create<FilmGrain>() },
				{ "foura", Cams["foura"].Components.Create<FilmGrain>() },
				{ "fourb", Cams["fourb"].Components.Create<FilmGrain>() },
				{ "five", Cams["five"].Components.Create<FilmGrain>() },
				{ "six", Cams["six"].Components.Create<FilmGrain>() },
				{ "seven", Cams["seven"].Components.Create<FilmGrain>() }
			};
			CamColor = new Dictionary<string, ColorAdjustments>
			{
				{ "onea", Cams["onea"].Components.Create<ColorAdjustments>() },
				{ "oneb", Cams["oneb"].Components.Create<ColorAdjustments>() },
				{ "onec", Cams["onec"].Components.Create<ColorAdjustments>() },
				{ "twoa", Cams["twoa"].Components.Create<ColorAdjustments>() },
				{ "twob", Cams["twob"].Components.Create<ColorAdjustments>() },
				{ "three", Cams["three"].Components.Create<ColorAdjustments>() },
				{ "foura", Cams["foura"].Components.Create<ColorAdjustments>() },
				{ "fourb", Cams["fourb"].Components.Create<ColorAdjustments>() },
				{ "five", Cams["five"].Components.Create<ColorAdjustments>() },
				{ "six", Cams["six"].Components.Create<ColorAdjustments>() },
				{ "seven", Cams["seven"].Components.Create<ColorAdjustments>() }
			};
			CamRotations = new Dictionary<string, Angles>
			{
				{ "onea", new Angles(17.6917f, -38, 0) },
				{ "oneb", new Angles( 27, -150, 0 ) },
				{ "onec", new Angles( 14, 90, 0 ) },
				{ "twoa", new Angles( 9, 1, 0 ) },
				{ "twob", new Angles( 45.5f, 132.583f, 0 ) },
				{ "three", new Angles( 56.2593f, 36.077f, 0 ) },
				{ "foura", new Angles( 11.0083f, 328.169f, 0 ) },
				{ "fourb", new Angles( 35.4f, 217, 0 ) },
				{ "five", new Angles( 21, 357.215f, 0 ) },
				{ "six", new Angles( -90, 270, 0 ) },
				{ "seven", new Angles( 26.75f, 206.695f, 0 ) }
			};
			Cams["onea"].WorldPosition = new Vector3(1179, -1123, 197);
			Cams["onea"].WorldRotation = CamRotations["onea"];
			Cams["onea"].FieldOfView = 90;
			Cams["onea"].Enabled = false;
			Cams["onea"].Priority = 2;
			CamStatic["onea"].Intensity = 0.07f;
			CamStatic["onea"].Response = 0.3f;
			CamColor["onea"].Saturation = 0;
			CamLights["onea"].LightColor = new Color( 0.89411764705f, 0.89411764705f, 0.89411764705f );
			CamLights["onea"].Attenuation = 6;//8.68f
			CamLights["onea"].Enabled = false;
			CamLights["onea"].ConeInner = 1;
			CamLights["onea"].ConeOuter = 38;
			CamLights["onea"].Shadows = true;
			CamLights["onea"].WorldPosition = new Vector3(1136.53f, -1121, 176);
			CamLights["onea"].WorldRotation = new Angles(17.1786f, 317.319f, 11.2339f);
			CamLights["onea"].Radius = 400;

			Cams["oneb"].WorldPosition = new Vector3( 1063.02f, -1070.16f, 196 );
			Cams["oneb"].WorldRotation = CamRotations["oneb"];
			Cams["oneb"].FieldOfView = 90;
			Cams["oneb"].Enabled = false;
			Cams["oneb"].Priority = 2;
			Cams["oneb"].BackgroundColor = Color.Black;
			Cams["oneb"].ZFar = 952;
			CamStatic["oneb"].Intensity = 0.07f;
			CamStatic["oneb"].Response = 0.3f;
			CamColor["oneb"].Saturation = 0;
			CamLights["oneb"].LightColor = new Color( 0.89411764705f, 0.89411764705f, 0.89411764705f );
			CamLights["oneb"].Attenuation = 1.96f;
			CamLights["oneb"].Enabled = false;
			CamLights["oneb"].ConeInner = 1;
			CamLights["oneb"].ConeOuter = 47.5f;
			CamLights["oneb"].Shadows = true;
			CamLights["oneb"].WorldPosition = new Vector3( 1076.35f, -1066.01f, 200.954f );
			CamLights["oneb"].WorldRotation = new Angles( 21.4717f, 211.162f, 6.68114f );
			CamLights["oneb"].Radius = 400;

			Cams["onec"].WorldPosition = new Vector3( 715, -947, 160 );
			Cams["onec"].WorldRotation = CamRotations["onec"];
			Cams["onec"].FieldOfView = 90;
			Cams["onec"].Enabled = false;
			Cams["onec"].Priority = 2;
			CamStatic["onec"].Intensity = 0.07f;
			CamStatic["onec"].Response = 0.3f;
			CamColor["onec"].Saturation = 0;
			CamLights["onec"].LightColor = new Color( 0.89411764705f, 0.89411764705f, 0.89411764705f );
			CamLights["onec"].Attenuation = 9;
			CamLights["onec"].Enabled = false;
			CamLights["onec"].ConeInner = 1;
			CamLights["onec"].ConeOuter = 50;
			CamLights["onec"].Shadows = true;
			CamLights["onec"].WorldPosition = new Vector3( 715, -933.302f, 160.893f );
			CamLights["onec"].WorldRotation = new Angles( 14, 90, 0 );
			CamLights["onec"].Radius = 210;

			Cams["twoa"].WorldPosition = new Vector3( 80, -1065, 167 );
			Cams["twoa"].WorldRotation = CamRotations["twoa"];
			Cams["twoa"].FieldOfView = 90;
			Cams["twoa"].Enabled = false;
			Cams["twoa"].Priority = 2;
			CamStatic["twoa"].Intensity = 0.07f;
			CamStatic["twoa"].Response = 0.3f;
			CamColor["twoa"].Saturation = 0;
			Cams["twoa"].ZFar = 500;
			Cams["twoa"].BackgroundColor = Color.Black;
			CamLights["twoa"].LightColor = new Color( 0.89411764705f, 0.89411764705f, 0.89411764705f );
			CamLights["twoa"].Attenuation = 25;
			CamLights["twoa"].Enabled = false;
			CamLights["twoa"].ConeInner = 1;
			CamLights["twoa"].ConeOuter = 45;
			CamLights["twoa"].Shadows = true;
			CamLights["twoa"].WorldPosition = new Vector3( 43, -1077, 174 );
			CamLights["twoa"].WorldRotation = new Angles( 17.6801f, 12.9592f, 4.1676f );
			CamLights["twoa"].Radius = 500;
			CamLights["twoa2"].LightColor = new Color( 0.89411764705f, 0.71372549019f, 0.54901960784f );
			CamLights["twoa2"].Attenuation = 120;
			CamLights["twoa2"].Enabled = false;
			CamLights["twoa2"].ConeInner = 45;
			CamLights["twoa2"].ConeOuter = 55;
			CamLights["twoa2"].Shadows = true;
			CamLights["twoa2"].WorldPosition = new Vector3( 439, -1041, 173 );
			CamLights["twoa2"].WorldRotation = new Angles( 90, 270, 0 );
			CamLights["twoa2"].Radius = 500;

			Cams["twob"].WorldPosition = new Vector3( -209, -1083, 175 );
			Cams["twob"].WorldRotation = CamRotations["twob"];
			Cams["twob"].FieldOfView = 90;
			Cams["twob"].Enabled = false;
			Cams["twob"].Priority = 2;
			CamStatic["twob"].Intensity = 0.07f;
			CamStatic["twob"].Response = 0.3f;
			CamColor["twob"].Saturation = 0;
			CamLights["twob"].LightColor = new Color( 0.89411764705f, 0.89411764705f, 0.89411764705f );
			CamLights["twob"].Attenuation = 40;
			CamLights["twob"].Enabled = false;
			CamLights["twob"].ConeInner = 1;
			CamLights["twob"].ConeOuter = 45;
			CamLights["twob"].Shadows = true;
			CamLights["twob"].WorldPosition = new Vector3( -153, -1087, 178 );
			CamLights["twob"].WorldRotation = new Angles( 30.8998f, 154.484f, 7.56981f );
			CamLights["twob"].Radius = 400;

			Cams["three"].WorldPosition = new Vector3( 332, -915, 167 );
			Cams["three"].WorldRotation = CamRotations["three"];
			Cams["three"].FieldOfView = 90;
			Cams["three"].Enabled = false;
			Cams["three"].Priority = 2;
			CamStatic["three"].Intensity = 0.07f;
			CamStatic["three"].Response = 0.3f;
			CamColor["three"].Saturation = 0;
			CamLights["three"].LightColor = new Color( 0.898039215686f, 0.79607843137f, 0.70588235294f );
			CamLights["three"].Attenuation = 220;
			CamLights["three"].Enabled = false;
			CamLights["three"].ConeInner = 45;
			CamLights["three"].ConeOuter = 55;
			CamLights["three"].Shadows = true;
			CamLights["three"].WorldPosition = new Vector3( 332.8f, -906, 175 );
			CamLights["three"].WorldRotation = new Angles( 90, 270, 0 );
			CamLights["three"].Radius = 400;

			Cams["foura"].WorldPosition = new Vector3( 132, -1227, 149.172f );
			Cams["foura"].WorldRotation = CamRotations["foura"];
			Cams["foura"].FieldOfView = 90;
			Cams["foura"].Enabled = false;
			Cams["foura"].Priority = 2;
			CamStatic["foura"].Intensity = 0.07f;
			CamStatic["foura"].Response = 0.3f;
			CamColor["foura"].Saturation = 0;
			Cams["foura"].ZFar = 285;
			Cams["foura"].BackgroundColor = Color.Black;
			CamLights["foura"].LightColor = new Color( 0.89411764705f, 0.89411764705f, 0.89411764705f );
			CamLights["foura"].Attenuation = 25;
			CamLights["foura"].Enabled = false;
			CamLights["foura"].ConeInner = 1;
			CamLights["foura"].ConeOuter = 42.5f;
			CamLights["foura"].Shadows = true;
			CamLights["foura"].WorldPosition = new Vector3( 117.628f, -1224.7f, 155 );
			CamLights["foura"].WorldRotation = new Angles( 13.8491f, 344.359f, 9.35187f );
			CamLights["foura"].Radius = 500;

			Cams["fourb"].WorldPosition = new Vector3( -190, -1225.34f, 161 );
			Cams["fourb"].WorldRotation = CamRotations["fourb"];
			Cams["fourb"].FieldOfView = 90;
			Cams["fourb"].Enabled = false;
			Cams["fourb"].Priority = 2;
			CamStatic["fourb"].Intensity = 0.07f;
			CamStatic["fourb"].Response = 0.3f;
			CamColor["fourb"].Saturation = 0;
			CamLights["fourb"].LightColor = new Color( 0.89411764705f, 0.89411764705f, 0.89411764705f );
			CamLights["fourb"].Attenuation = 60;
			CamLights["fourb"].Enabled = false;
			CamLights["fourb"].ConeInner = 1;
			CamLights["fourb"].ConeOuter = 35;
			CamLights["fourb"].Shadows = true;
			CamLights["fourb"].WorldPosition = new Vector3( -178, -1218, 167 );
			CamLights["fourb"].WorldRotation = new Angles( 38.1724f, 215.037f, 8.2461f );
			CamLights["fourb"].Radius = 400;

			Cams["five"].WorldPosition = new Vector3( 866.268f, -791, 168.774f );
			Cams["five"].WorldRotation = CamRotations["five"];
			Cams["five"].FieldOfView = 90;
			Cams["five"].Enabled = false;
			Cams["five"].Priority = 2;
			CamStatic["five"].Intensity = 0.07f;
			CamStatic["five"].Response = 0.3f;
			CamColor["five"].Saturation = 0;
			CamLights["five"].LightColor = new Color( 0.4117647058f, 0.4117647058f, 0.4117647058f );
			CamLights["five"].Attenuation = 125;
			CamLights["five"].Enabled = false;
			CamLights["five"].ConeInner = 15;
			CamLights["five"].ConeOuter = 35;
			CamLights["five"].Shadows = true;
			CamLights["five"].WorldPosition = new Vector3( 866, -791, 168.774f );
			CamLights["five"].WorldRotation = new Angles( 21, 357.215f, 0 );
			CamLights["five"].Radius = 27.5f;

			Cams["six"].WorldPosition = new Vector3( 301, -1406, 164 );
			Cams["six"].WorldRotation = CamRotations["six"];
			Cams["six"].FieldOfView = 90;
			Cams["six"].Enabled = false;
			Cams["six"].Priority = 2;
			CamStatic["six"].Intensity = 0.07f;
			CamStatic["six"].Response = 0.3f;
			CamColor["six"].Saturation = 0;

			Cams["seven"].WorldPosition = new Vector3( 1030.29f, -1707.83f, 170.657f );
			Cams["seven"].WorldRotation = CamRotations["seven"];
			Cams["seven"].FieldOfView = 90;
			Cams["seven"].Enabled = false;
			Cams["seven"].Priority = 2;
			CamStatic["seven"].Intensity = 0.07f;
			CamStatic["seven"].Response = 0.3f;
			CamColor["seven"].Saturation = 0;
			CamLights["seven"].LightColor = new Color( 0.89411764705f, 0.89411764705f, 0.89411764705f );
			CamLights["seven"].Attenuation = 2.6f;
			CamLights["seven"].Enabled = false;
			CamLights["seven"].ConeInner = 1;
			CamLights["seven"].ConeOuter = 38;
			CamLights["seven"].Shadows = true;
			CamLights["seven"].WorldPosition = new Vector3( 1035, -1708, 173.017f );
			CamLights["seven"].WorldRotation = new Angles( 14.362f, 196.39f, 2.94552f );
			CamLights["seven"].Radius = 238.3f;

			PowerOutage = false;
			Power = 1000;
			LastDrain = 0;
			CurCam = "onea";

			RPanel = LocalScene.CreateObject().Components.Create<ScreenPanel>( true );
			RPanel.GetPanel().StyleSheet.Load("/Styles/CameraUI.razor.scss");
			RPanel.GetPanel().StyleSheet.Load("/Styles/common.razor.scss");
			CamUI = RPanel.Components.Create<CameraUI>( true );
			PowerUI = RPanel.Components.Create<Power>( true );
			TimeUI = RPanel.Components.Create<NightTime>( true );
			GameOverUI = RPanel.Components.Create<GameOver>( true );
			GameWinUI = RPanel.Components.Create<GameWin>( true );

			hallucinationsound = new SoundEvent();
			hallucinationsound.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/hallucination.sound" ) };
			hallucinationsound.Volume = 0.5f;
			camswivelsound = new SoundEvent();
			camswivelsound.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/camswivel.sound" ) };
			camswivelsound.Volume = 0.8f;
			camblipsound = new SoundEvent();
			camblipsound.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/camnoise.sound" ) };
			camblipsound.Volume = 0.7f;
			caminterferencesound = new SoundEvent();
			caminterferencesound.Sounds = new List<SoundFile> {
				SoundFile.Load( "sounds/caminterference.sound" ),
				SoundFile.Load( "sounds/garble1.sound" ),
				SoundFile.Load( "sounds/garble2.sound" ),
				SoundFile.Load( "sounds/garble3.sound" )
			};
			caminterferencesound.SelectionMode = SoundEvent.SoundSelectionMode.RandomExclusive;
			caminterferencesound.Volume = 0.7f;
			camusesound = new SoundEvent();
			camusesound.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/cams.sound" ) };
			camusesound.Volume = 0.6f;
			favorsound = new SoundEvent();
			favorsound.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/partyfavor.sound" ) };
			stingersound = new SoundEvent();
			stingersound.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/stinger.sound" ) };

			tweakingsounds = new SoundEvent();
			tweakingsounds.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/robotvoice.sound" ) };
			tweakingsounds.Volume = 0.75f;

			scarysounds = new SoundEvent();
			scarysounds.Sounds = new List<SoundFile> 
			{ 
				SoundFile.Load( "sounds/ambiance1.sound" ),
				SoundFile.Load( "sounds/ambiance2.sound" ),
				SoundFile.Load( "sounds/ambiance3.sound" ),
				SoundFile.Load( "sounds/circus.sound" ),
				SoundFile.Load( "sounds/piratesong2.sound" ),
			};
			scarysounds.Volume = 0.35f;
			scarysounds.SelectionMode = SoundEvent.SoundSelectionMode.RandomExclusive;

			breathingsound = new SoundEvent();
			breathingsound.Sounds = new List<SoundFile> 
			{
				SoundFile.Load( "sounds/breathing1.sound" ),
				SoundFile.Load( "sounds/breathing2.sound" ),
				SoundFile.Load( "sounds/breathing3.sound" ),
				SoundFile.Load( "sounds/breathing4.sound" ),
			};

			stepsound = new SoundEvent();
			stepsound.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/steps.sound" ) };

			Hour = 12;
			GameTimer = 0;
		}
		public void CloseCams()
		{
			if(InCams)
			{
				Tablet.Flip();
			}
			InCams = false;
			CameraUI.CamLatch = false;
			Cams[CurCam].Enabled = false;
		}
	}
	public class FNAFGameManager : GameObjectSystem
	{
		public static FNAFGameState GameState;
		public static int night;
		public static int FreddyLevel;
		public static int BonnieLevel;
		public static int ChicaLevel;
		public static int FoxyLevel;
		public static bool setup;
		public FNAFGameManager( Scene scene ) : base( scene )
		{
			Listen(Stage.PhysicsStep, 10, GameTick, "Master-tick");
		}
		void GameTick()
		{
			if ( Scene.Title != "game" ) { return; }
			if ( !setup )
			{
				if ( night == 0 ) { night = 1; }
				GameState = new FNAFGameState( Scene, night, false );
				NightTime.night = night;
				NightTime.time = 12;
				NightTime.powerout = false;
				Power.powerout = false;
				GameOver.GFreddyJumpscare = 20;
				GameOver.DoStuff = false;
				CameraUI.disable = false;
				GameWin.DoStuff = false;
				GameWin.MenuTimer = -99999999;
				GameOver.GFreddyJumpscare = 20;
				GameState.DesiredCam = "onea";
				if ( GameState.DEV )
				{
					GameState.Phone.Silence();
					//GameState.FreddyPoster.SceneModel.SetMaterialGroup("golden");
				}
				setup = true;
			} else
			{
				if ( GameState.JumpscareBootTimer >= 1f )
				{
					GameState.Freddy.Lobotomize = true;
					GameState.Bonnie.Lobotomize = true;
					GameState.Chica.Lobotomize = true;
					GameState.Foxy.Lobotomize = true;
					if ( GameState.PostJumpscareCheck == false )
					{
						GameState.PostJumpscareCheck = true;
						var tempstatic = GameState.OfficeCamera.Components.Create<FilmGrain>();
						tempstatic.Response = 0.5f;
						Sound.StopAll(0);
						tempstatic.Intensity = 0.9f;
						Sandbox.Services.Stats.Increment( "nightslost", 1 );
						if ( GameOver.GFreddyJumpscare > 19 )
						{
							GameOver.DoStuff = true;
							GameOver.StaticDelay = 0;
							var soundfx = new SoundEvent();
							soundfx.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/death.sound" ) };
							soundfx.Volume = 0.6f;
							Sound.Play( soundfx, new Vector3( 0, 0, 2002 ) );
						}
						else
						{
							var soundfx = new SoundEvent();
							soundfx.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/xscream2.sound" ) };
							Sound.Play( soundfx, new Vector3( 0, 0, 2002 ) );
							Sound.Play( soundfx, GameState.OfficeCamera.WorldPosition );
						}
						GameState.OfficeCamera.WorldPosition = new Vector3( 0, 0, 2000 );
						Power.powerout = true;
						NightTime.powerout = true;
						CameraUI.disable = true;
					}
					return;
				} else if ( GameState.JumpscareBootTimer >= 0 & GameState.JumpscareDoShake ) 
				{ 
					GameState.OfficeCamera.WorldRotation = new Angles( 0, 0, (float)Math.Sin( Time.Now * 30 ) * 12 );
					return;
				}
				if(GameState.Power < 0 & !GameState.DidPowerout )
				{
					GameState.DidPowerout = true;
					Sandbox.Services.Stats.Increment( "powerouts", 1 );
					GameState.PowerOutage = true;
					if ( GameState.LeftDoorButton.State ) { GameState.LeftDoorButton.Use(); }
					if ( GameState.RightDoorButton.State ) { GameState.RightDoorButton.Use(); }
					if ( GameState.LeftLightButton.State ) { GameState.LeftLightButton.Use(); }
					if ( GameState.RightLightButton.State ) { GameState.RightLightButton.Use(); }
					GameState.LeftDoorButton.KillLight();
					GameState.RightDoorButton.KillLight();
					GameState.OfficeLight.Enabled = false;
					GameState.OfficePoweroutLight.Enabled = true;
					Power.powerout = true;
					NightTime.powerout = true;
					GameState.CloseCams();
					if (!GameState.Phone.Done)
						GameState.Phone.Silence();
					GameState.Fan.Off();
					GameState.Freddy.PoweroutForcer = 0;
					GameState.Freddy.DoPoweroutSequence = true;
					GameState.Bonnie.Lobotomize = true;
					GameState.Bonnie.Model.Enabled = false;
					GameState.Chica.Lobotomize = true;
					GameState.Chica.Model.Enabled = false;
					GameState.Foxy.Lobotomize = true;
					GameState.Foxy.Model.Enabled = false;
					var soundfx = new SoundEvent();
					soundfx.Sounds = new List<SoundFile> { SoundFile.Load( "sounds/powerdown.sound" ) };
					var f = Sound.Play( soundfx, GameState.OfficeCamera.WorldPosition );
					return;
				}
				
				GameState.PowerConsumption = (GameState.InCams ? 1 : 0) 
					+ (GameState.LeftDoorButton.TrueState ? 1 : 0) 
					+ (GameState.RightDoorButton.TrueState ? 1 : 0)
					+ (GameState.LeftLightButton.TrueState ? 1: 0)
					+ (GameState.RightLightButton.TrueState ? 1 : 0);
				if(GameState.LastDrain >= 1 & !GameState.DEV)
				{
					GameState.Power -= GameState.PowerConsumption + 1;
					GameState.LastDrain = 0;
				}
				Power.power = GameState.Power;
				Power.usage = GameState.PowerConsumption;
				Power.powerout = GameState.PowerOutage;
				
				if ( GameState.GameTimer > 90 )
				{
					Sandbox.Services.Stats.Increment( "fnafhours", 1 );
					GameState.GameTimer = 0;
					GameState.Hour = GameState.Hour == 12 ? 1 : GameState.Hour + 1;
					GameState.Bonnie.NewHour( GameState.Hour );
					GameState.Chica.NewHour( GameState.Hour );
					GameState.Foxy.NewHour( GameState.Hour );
					//Log.Info("New Hour: "+GameState.Hour);
					//Log.Info( "Freddy AI: " + GameState.Freddy.CurrentAI );
					//Log.Info( "Bonnie AI: " + GameState.Bonnie.CurrentAI );
					//Log.Info( "Chica AI: " + GameState.Chica.CurrentAI );
					//Log.Info( "Foxy AI: " + GameState.Foxy.CurrentAI );
					NightTime.time = GameState.Hour;
					if ( GameState.Hour == 6 )
					{
						ProgressTracker.Validate();
						var save = ProgressTracker.Fetch();
						if(night == 7)
						{
							save.bestfreddy = FreddyLevel > save.bestfreddy ? FreddyLevel : save.bestfreddy;
							save.bestbonnie = BonnieLevel > save.bestbonnie ? BonnieLevel : save.bestbonnie;
							save.bestchica = ChicaLevel > save.bestchica ? ChicaLevel : save.bestchica;
							save.bestfoxy = FoxyLevel > save.bestfoxy ? FoxyLevel : save.bestfoxy;
							if ( FreddyLevel == 20 & BonnieLevel == 20 & ChicaLevel == 20 & FoxyLevel == 20 ) 
							{
								Sandbox.Services.Stats.Increment( "fourtwentys", 1 );
								save.fourtwenty = true;
							}
						}
						else if (night == 6)
						{
							save.bestnight = 6;
							save.night = 5;
						}
						else
						{
							save.bestnight = save.bestnight + 1 > 6 ? 6 : save.bestnight + 1;
							save.night = save.night + 1 > 5 ? 5 : save.night + 1;
						}
						ProgressTracker.Save(save);
						Sandbox.Services.Stats.Increment( "nightswon", 1 );
						Sound.StopAll( 0 );
						GameState.OfficeCamera.WorldPosition = new Vector3( 0, 0, 0 );
						GameState.Power = 1000;
						GameState.Freddy.Lobotomize = true;
						GameState.Bonnie.Lobotomize = true;
						GameState.Chica.Lobotomize = true;
						GameState.Foxy.Lobotomize = true;
						Power.powerout = true;
						NightTime.powerout = true;
						GameState.PowerOutage = true;
						CameraUI.disable = true;
						GameWin.DoStuff = true;
						GameWin.night = night;
						return;
					}
				}
				if ( GameState.InCams )
				{
					if ( GameState.LeftLightButton.State ) { GameState.LeftLightButton.Use(); }
					if ( GameState.RightLightButton.State ) { GameState.RightLightButton.Use(); }
				}
				//GameState.OfficeCamOffset -= Input.MouseDelta.x;
				//GameState.OfficeCamOffset = Math.Clamp( GameState.OfficeCamOffset, -18, 18);
				if ( GameState.OfficeCamera != null )
					GameState.OfficeCamera.WorldRotation = new Angles( 0, GameState.OfficeCamOffset, 0 );
				if (!GameState.InCams & CameraUI.lmb & !GameState.PowerOutage)
				{
					var tr = Scene.Trace.Ray( GameState.OfficeCamera.ScreenPixelToRay( Mouse.Position ), 1000f ).Run();
					CameraUI.lmb = false;
					if ( tr.HitPosition.Distance( GameState.LeftDoorButton.Object.WorldPosition ) <= 5 )
					{
						GameState.LeftDoorButton.Use();
					}
					else if ( tr.HitPosition.Distance( GameState.LeftLightButton.Object.WorldPosition ) <= 4.5f )
					{
						GameState.LeftLightButton.Use();
					}
					if ( tr.HitPosition.Distance( GameState.RightDoorButton.Object.WorldPosition ) <= 5 )
					{
						GameState.RightDoorButton.Use();
					}
					else if ( tr.HitPosition.Distance( GameState.RightLightButton.Object.WorldPosition ) <= 4.5f )
					{
						GameState.RightLightButton.Use();
					}
					if ( tr.HitPosition.Distance( GameState.Phone.Object.WorldPosition ) <= 10 )
					{
						GameState.Phone.Silence();
					}
					if( tr.HitPosition.Distance(new Vector3(25, -1130, 149)) <= 2 )
					{
						GameState.favorhandle = Sound.Play(GameState.favorsound, new Vector3( 25, -1130, 149 ) );
					}
					if ( tr.HitPosition.Distance( new Vector3( -16, -1187, 119.553f ) ) <= 1 )
					{
						if ( GameState.GFreddyVideoPanel == null )
						{
							GameState.GFreddyVideoPanel = Scene.CreateObject().Components.Create<SkinnedModelRenderer>( true );
							GameState.GFreddyVideoPanel.WorldPosition = new Vector3( -10.0483f, -1177.97f, 116.435f );
							GameState.GFreddyVideoPanel.WorldRotation = new Angles( 0, 326.935f, 180 );
							GameState.GFreddyVideoPanel.Model = Sandbox.Model.Load( "models/tvoverlay.vmdl" );
							GameState.GFreddyVideo = new VideoPlayer();
							GameState.GFreddyVideo.Play( FileSystem.Mounted, "ui/gfreddyvideo.mp4" );
							GameState.GFreddyVideoMat = Material.Load( "materials/ui/videotex.vmat" );
							GameState.GFreddyVideo.Audio.ListenLocal = false;
							GameState.GFreddyVideo.Audio.Position = GameState.GFreddyVideoPanel.WorldPosition;
							GameState.GFreddyVideo.Audio.Volume = 0.6f;
							//GameState.GFreddyVideo.
						}
						GameState.VideoPlaying = true;//!GameState.VideoPlaying;
						/*if( GameState.VideoPlaying )
						{
							GameState.GFreddyVideo.Seek(0);
							GameState.GFreddyVideo.Resume();
						} else
						{
							GameState.GFreddyVideo.Pause();
						}*/
					}
				}
				
				if(GameState.VideoPlaying)
				{
					if ( GameState.GFreddyVidStatus != 2 )
					{
						//if ( GameState.GFreddyVideo.SampleRate == 0 ) { return; }
						//if ( GameState.GFreddyVideo.PlaybackTime == 0 ) 
						//{ 
						//	GameState.GFreddyVideo.Resume();
						//	GameState.GFreddyVideo.Seek(1);
						//}
						GameState.GFreddyVideo.Present();
						if ( GameState.GFreddyVideo.Texture.IsLoaded )
						{
							if ( GameState.GFreddyVidStatus == 0 )
							{
								//GameState.GFreddyVidSound = new SoundEvent();
								//GameState.GFreddyVidSound.Sounds = new List<SoundFile> { SoundFile.Load( "ui/gfreddyvideo.sound" ) };
								//GameState.GFreddyVidSoundHandle = Sound.Play( GameState.GFreddyVidSound, GameState.GFreddyVideoPanel.WorldPosition );
								GameState.GFreddyVidStatus = 1;
							}
							//Log.Info( GameState.GFreddyVideo.Texture );
							GameState.GFreddyVideoMat.Set( "Color", GameState.GFreddyVideo.Texture );
							GameState.GFreddyVideoPanel.MaterialOverride = GameState.GFreddyVideoMat;
							//Log.Info( GameState.GFreddyVideo.PlaybackTime );
							if ( GameState.GFreddyVideo.PlaybackTime >= GameState.GFreddyVideo.Duration )
							{
								GameState.GFreddyVidStatus = 2;
								GameState.GFreddyVideoPanel.Destroy();
								GameState.GFreddyVideo.Dispose();
								//GameState.GFreddyVidSoundHandle.Dispose();
							}
						}
					}
				}
				GameState.CamsSwivel += GameState.CamSwivelDir * 0.035f;
				if ( GameState.CamsSwivel > 6 | GameState.CamsSwivel < -6 )
				{
					GameState.CamSwivelDir = -GameState.CamSwivelDir;
				}
				if( GameState.InCams )
				{
					GameState.Cams[GameState.CurCam].WorldRotation = GameState.CamRotations[GameState.CurCam] + new Angles( 0, Math.Clamp( GameState.CamsSwivel, -3, 3 ), 0 );
				}
				if ( GameState.DesiredCam != "" )
				{
					GameState.Cams[GameState.CurCam].Enabled = false;
					GameState.CamLights[GameState.CurCam].Enabled = false;
					GameState.CurCam = GameState.DesiredCam;
					GameState.Cams[GameState.CurCam].Enabled = true;
					GameState.CamLights[GameState.CurCam].Enabled = true;
					GameState.DesiredCam = "";
					CameraUI.blip = 0;
					CameraUI.bliptog = true;
				}
				
				if ( GameState.CurCam == null ) { GameState.CurCam = "onea"; }
				if( GameState.CurCam == "twoa" & GameState.CamLights["twoa2"] != null)
				{
					GameState.CamLights["twoa2"].Enabled = true;
				} else
				{
					GameState.CamLights["twoa2"].Enabled = false;
				}
				if ( !GameState.InCams )
				{
					GameState.Cams[GameState.CurCam].Enabled = false;
					GameState.CamLights[GameState.CurCam].Enabled = false;
					GameState.Fan.VolumeChange( false );
				}
				else if ( CameraUI.displaydelay > 0.3f )
				{
					GameState.Cams[GameState.CurCam].Enabled = true;
					GameState.CamLights[GameState.CurCam].Enabled = true;
					GameState.Fan.VolumeChange( true );
				}
				if ( GameState.LeftDoorButton.First )
				{
					if ( GameState.LeftDoorButton.State ) { GameState.LeftDoor.Close(); }
					else { GameState.LeftDoor.Open(); }

				}
				if ( GameState.RightDoorButton.First )
				{
					if ( GameState.RightDoorButton.State ) { GameState.RightDoor.Close(); }
					else { GameState.RightDoor.Open(); }

				}
				if ( GameState.LeftLightButton.First )
				{
					if ( GameState.RightLightButton.State ) { GameState.RightLightButton.Use(false); }
				}
				if ( GameState.RightLightButton.First )
				{
					if ( GameState.LeftLightButton.State ) { GameState.LeftLightButton.Use(false); }
				}
				GameState.RightLight.Enabled = GameState.RightLightButton.State;
				GameState.LeftLight.Enabled = GameState.LeftLightButton.State;
				
				/*if ( CameraUI.usetog & !GameState.PowerOutage )
				{
					CameraUI.usetog = false;
					GameState.Tablet.Flip();
					GameState.camusehandle = Sound.Play( GameState.camusesound, GameState.OfficeCamera.WorldPosition );
				}*/
				if( GameState.ScaryEventTimer >= 25 & !GameState.Bonnie.Lobotomize & !GameState.PowerOutage )
				{
					int odds = new Random().Next( 1, 10 );
					if ( odds >= 8 )
					{
						var e = Sound.Play(GameState.scarysounds, GameState.OfficeCamera.WorldPosition );
						e.Volume = 0.85f;
					}
					odds = new Random().Next( 1, 400 );
					if ( odds <= 5 )
					{
						Sandbox.Services.Stats.Increment( "hallucinations", 1 );
						CameraUI.hallucinationtimer = 0;
					}
					GameState.ScaryEventTimer = -new Random().Next(25, 68);
				}

				if( GameState.InCams )
				{
					if( GameState.camswivelhandle == null)
					{
						GameState.camswivelhandle = Sound.Play( GameState.camswivelsound, GameState.OfficeCamera.WorldPosition );
						GameState.camswivelhandle.Volume = 0.4f;
					} else if ( !GameState.camswivelhandle.IsPlaying )
					{
						GameState.camswivelhandle = Sound.Play( GameState.camswivelsound, GameState.OfficeCamera.WorldPosition );
						GameState.camswivelhandle.Volume = 0.4f;
					}
					if ( CameraUI.bliptog )
					{
						GameState.cambliphandle = Sound.Play( GameState.camblipsound, GameState.OfficeCamera.WorldPosition );
						CameraUI.bliptog = false;
					}
					if ( GameState.caminterferencehandle == null )
					{
						GameState.caminterferencehandle = Sound.Play( GameState.caminterferencesound, GameState.OfficeCamera.WorldPosition );
						GameState.caminterferencehandle.Stop();
					}
					if ( GameState.CameraInterference < 2 & !GameState.caminterferencehandle.IsPlaying )
					{
						GameState.caminterferencehandle = Sound.Play( GameState.caminterferencesound, GameState.OfficeCamera.WorldPosition );
					}
					GameState.CamStatic[GameState.CurCam].Intensity = GameState.CameraInterference < 2 ? 1 : 0.07f;
				}
				else
				{
					if( GameState.camswivelhandle != null )
					{
						GameState.camswivelhandle.Stop();
					}
					if( GameState.caminterferencehandle != null )
					{
						GameState.caminterferencehandle.Stop();
					}
				}
				if ( CameraUI.hallucinationtimer < 1.5 )
				{
					if ( GameState.hallucinationhandle == null)
					{
						GameState.hallucinationhandle = Sound.Play( GameState.hallucinationsound, GameState.OfficeCamera.WorldPosition );
						GameState.hallucinationhandle.Volume = 0.6f;
					}
					else if ( !GameState.hallucinationhandle.IsPlaying )
					{
						GameState.hallucinationhandle = Sound.Play( GameState.hallucinationsound, GameState.OfficeCamera.WorldPosition );
						GameState.hallucinationhandle.Volume = 0.6f;
					}
				} else if ( CameraUI.hallucinationtimer > 1.5)
				{
					if( GameState.hallucinationhandle != null)
					{
						GameState.hallucinationhandle.Stop();
					}
				}

				GameState.Phone.Think();

				GameState.LeftDoor.Tick();
				GameState.RightDoor.Tick();
				GameState.LeftDoorButton.Tick();
				GameState.RightDoorButton.Tick();
				GameState.LeftLightButton.Tick();
				GameState.RightLightButton.Tick();

				GameState.Freddy.Think();
				GameState.Bonnie.Think();
				GameState.Chica.Think();
				GameState.Foxy.Think();
				GameState.GoldenFreddy.Think();
				GameState.Endo.Think();
				//if ( Input.EscapePressed ) { ReturnToMenu(); }
			}
		}

		[ConCmd("bonnie")]
		public static void FNAFCommand(string cam)
		{
			if ( !GameState.DEV ) {
				Log.Info("Nope.");
				return; 
			}
			GameState.Bonnie.Think(true, cam);
		}
		[ConCmd( "foxy" )]
		public static void FNAFFoxyTest()
		{
			if ( !GameState.DEV )
			{
				Log.Info( "Nope." );
				return;
			}
			GameState.Foxy.Think( true );
		}
		[ConCmd( "freddy" )]
		public static void FNAFFreddyTest( string cam )
		{
			if ( !GameState.DEV )
			{
				Log.Info( "Nope." );
				return;
			}
			GameState.Freddy.Think( true, cam );
		}
		[ConCmd( "chica" )]
		public static void FNAFChicaTest( string cam )
		{
			if ( !GameState.DEV )
			{
				Log.Info( "Nope." );
				return;
			}
			GameState.Chica.Think( true, cam );
		}
		[ConCmd("quitsmoking")]
		public static void FNAFSmoke()
		{
			//ProgressTracker.Cheater();
			if ( !GameState.DEV )
			{
				Log.Info( "Nope. You may not quit smoking." );
				return;
			}
			//Log.Info("Freddy: "+GameState.Freddy.CurrentAI);
			//Log.Info("Bonnie: "+GameState.Bonnie.CurrentAI);
			//Log.Info("Chica: "+GameState.Chica.CurrentAI);
			//Log.Info("Foxy: "+GameState.Foxy.CurrentAI);
			GameState.GameTimer = 90;
		}
		public static void ReturnToMenu()
		{
			Sound.StopAll( 0 );
			foreach ( GameObject GO in GameState.LocalScene.Children )
				if ( GO.IsValid )
					GO.Clear();
			setup = false;
			GameState.LocalScene.LoadFromFile( "scenes/menu.scene" );
		}
	}
}
