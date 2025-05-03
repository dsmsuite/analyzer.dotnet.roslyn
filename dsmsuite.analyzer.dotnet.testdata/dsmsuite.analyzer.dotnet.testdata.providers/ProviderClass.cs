namespace dsmsuite.analyzer.dotnet.testdata.providers
{

    class ProviderClassParameter1
    {
    };

    class ProviderClassParameter2
    {
    };

    class ProviderClass
    {
        class NestedClass
        {
            public NestedClass(ProviderClass provider) { }
            ~NestedClass() { }

            public void NestedPublicMethodA() { }
            public void NestedPublicMethodB() { }
            protected void NestedProtectedMethodA() { }
            protected void NestedProtectedMethodB() { }
            private void NestedPrivateMethodA() { }
            private void NestedPrivateMethodB() { }

            public int _nestedPublicMemberA;
            public int _nestedPublicMemberB;
            protected int _nestedProtectedMemberA;
            protected int _nestedProtectedMemberB;
            private int _nestedPrivateMemberA;
            private int _nestedPrivateMemberB;
            private ProviderClass _provider;
        };

        public ProviderClass(ProviderClassParameter1 par1, ProviderClassParameter2 par2) { }
        public ProviderClass() { }
        ~ProviderClass() { }
        public void PublicMethodA() { }
        public void PublicMethodB() { }
        protected void ProtectedMethodA() { }
        protected void ProtectedMethodB() { }
        private void PrivateMethodA() { }
        private void PrivateMethodB() { }
        public int _publicMemberA;
        public int _publicMemberB;
        protected int _protectedMemberA;
        protected int _protectedMemberB;
        int _privateMemberA;
        int _privateMemberB;
        private NestedClass _nestedClassMember;
    };

}
