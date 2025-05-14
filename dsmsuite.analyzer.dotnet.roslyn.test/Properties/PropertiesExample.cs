namespace dsmsuite.analyzer.dotnet.roslyn.test.Properties
{
    public struct ProviderStruct
    {
        public ProviderStruct(int val1, string val2)
        {
            structMember1 = val1;
            structMember2 = val2;
        }

        public int structMember1;
        public string structMember2;
    };

    public enum ProviderEnum
    {
        enumVal1,
        enumVal2
    };

    public class ProviderClass
    {
        public void ProviderClassMethod() { }
    }

    public class ProviderListTemplateArgument
    {
        public void ProviderListTemplateArgumentMethod() { }
    };

    public class ProviderTemplateArgument1
    {
        public void ProviderTemplateArgument1Method() { }
    };

    public class ProviderTemplateArgument2
    {
        public void ProviderTemplateArgument2Method() { }
    };

    public class ProviderGenericClass<T, U> where T : new() where U : new()
    {
        public T GetFirstTemplateArgument() { return new T(); }
        public U GetSecondTemplateArgument() { return new U(); }
    };

    class PropertyConsumer
    {
        public PropertyConsumer()
        {
            IntPropertyWithBackingField = 456;
            IntProperty = 123;
            EnumProperty = ProviderEnum.enumVal1;
            StructProperty = new ProviderStruct(1, "test");
            ClassProperty = new ProviderClass();
            ListClassProperty = new List<ProviderListTemplateArgument>();
            GenericClassProperty = new ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2>();
        }


        public void MethodUsingIntMember()
        {
            IntProperty = 123;
        }

        public void MethodUsingEnumMember()
        {
            switch (EnumProperty)
            {
                case ProviderEnum.enumVal1:
                    break;
                case ProviderEnum.enumVal2:
                    break;
                default:
                    break;
            }
        }

        public void MethodUsingStructMember()
        {
            if (StructProperty.structMember1 == 1 && StructProperty.structMember2 == "test") { }
        }

        public void MethodUsingClassMember()
        {
            ClassProperty.ProviderClassMethod();
        }

        public void MethodUsingStdListMember()
        {
            // Use explicit type
            ProviderListTemplateArgument firstElement = ListClassProperty.First<ProviderListTemplateArgument>();
            firstElement.ProviderListTemplateArgumentMethod();
        }

        public void MethodUsingGenericClassMember()
        {
            // Use explicit type
            ProviderTemplateArgument1? t = GenericClassProperty.GetFirstTemplateArgument();
            t?.ProviderTemplateArgument1Method();

            ProviderTemplateArgument2? u = GenericClassProperty.GetSecondTemplateArgument();
            u?.ProviderTemplateArgument2Method();
        }

        private int _intProtertyBackingField;

        public int IntPropertyWithBackingField
        {
            get { return _intProtertyBackingField; }
            set { _intProtertyBackingField = value; }
        }

        public int IntProperty { get; set; }
        public ProviderEnum EnumProperty { get; set; }
        public ProviderStruct StructProperty { get; set; }
        public ProviderClass ClassProperty { get; set; }
        public List<ProviderListTemplateArgument> ListClassProperty { get; set; }
        public ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2> GenericClassProperty { get; set; }
    };
}
