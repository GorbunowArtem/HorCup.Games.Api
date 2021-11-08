namespace HorCup.Games.Options
{
	public class MongoDbOptions
	{
		public const string MongoDb = "MongoDb";

		public string ConnectionString { get; set; }
		
		public string DatabaseName { get; set; }
		

		public SqlDbOptions Type { get; set; }
	}
}