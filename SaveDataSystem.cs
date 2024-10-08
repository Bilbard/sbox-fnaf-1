using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace FNAF
{
	public struct ProgressCookie
	{
		public int night { get; set; }
		public int bestnight { get; set; }
		public bool fourtwenty { get; set; }
		public int bestfreddy { get; set; }
		public int bestbonnie { get; set; }
		public int bestchica { get; set; }
		public int bestfoxy { get; set; }
		public ProgressCookie( int night, int bestnight, bool fourtwenty, int bestfreddy,
			int bestbonnie, int bestchica, int bestfoxy )
		{
			this.night = night;
			this.bestnight = bestnight;
			this.fourtwenty = fourtwenty;
			this.bestfreddy = bestfreddy;
			this.bestbonnie = bestbonnie;
			this.bestchica = bestchica;
			this.bestfoxy = bestfoxy;
		}
	}
	public class ProgressTracker
	{
		public static string FileName = "dagabey_fnaf1_progress_postscene.json";
		public ProgressTracker()
		{
			Validate();
		}
		public static void Validate()
		{
			if ( FileSystem.Data == null )
			{
				return;
			}
			if ( !FileSystem.Data.FileExists( FileName ) )
			{
				var newClient = new ProgressCookie( 1, 0, false, 0, 0, 0, 0 );
				FileSystem.Data.WriteJson<ProgressCookie>( FileName, newClient );
			}
		}
		public static void NewGame()
		{
			var newClient = new ProgressCookie( 1, 0, false, 0, 0, 0, 0 );
			if ( FileSystem.Data.FileExists( FileName ) )
			{
				newClient = FileSystem.Data.ReadJson<ProgressCookie>( FileName );
				newClient.night = 1;
			}
			FileSystem.Data.WriteJson<ProgressCookie>( FileName, newClient );
		}
		public static void Save( ProgressCookie cookie )
		{
			FileSystem.Data.WriteJson<ProgressCookie>( FileName, cookie );
		}
		public static void Cheater()
		{
			var newClient = new ProgressCookie( 5, 6, true, 20, 20, 20, 20 );
			FileSystem.Data.WriteJson<ProgressCookie>( FileName, newClient );
		}
		public static ProgressCookie Fetch()
		{
			return FileSystem.Data.ReadJson<ProgressCookie>( FileName );
		}
	}
}
