namespace HorCup.Games.Options
{
	public class SqlDbOptions
	{
		public const string SqlDb = "SqlDb";

		public string ConnectionString { get; set; }
		
		public string DatabaseName { get; set; }
	}
}