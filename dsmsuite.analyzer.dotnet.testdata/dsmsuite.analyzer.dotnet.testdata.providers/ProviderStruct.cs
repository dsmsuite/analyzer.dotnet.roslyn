namespace dsmsuite.analyzer.dotnet.testdata.providers
{
	struct ProviderStruct
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

		int member1;
		string member2;
	};
}
