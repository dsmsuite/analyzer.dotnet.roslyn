namespace dsmsuite.analyzer.dotnet.testdata.providers
{
	public struct ProviderStruct
	{
		public ProviderStruct()
		{
			member1 = 0;
			member2 = "val2";
		}

		public ProviderStruct(int val1, string val2)
		{
			member1 = val1;
			member2 = val2;
		}

		public int member1;
		public string member2;
	};
}
