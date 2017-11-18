namespace BitStrap
{
	public static class TypeExtensions
	{
		public static string GetCecilFullname( this System.Type self )
		{
			return self.FullName.Replace( '+', '/' );
		}
	}
}
