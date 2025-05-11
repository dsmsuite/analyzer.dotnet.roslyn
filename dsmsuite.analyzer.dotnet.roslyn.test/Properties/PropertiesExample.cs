namespace dsmsuite.analyzer.dotnet.roslyn.test.Properties
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

    public enum ProviderEnum
    {
        enum_val1,
        enum_val2,
        enum_val3
    };

    public class ProviderClass
    {
        public void PublicMethodA() { }
        public void PublicMethodB() { }
    }

    public class ProviderListTemplateArgument
    {
        public void PublicMethodA() { }
        public void PublicMethodB() { }
    };

    public class ProviderTemplateArgument1
    {
        public void PublicMethodA() { }
        public void PublicMethodB() { }
    };

    public class ProviderTemplateArgument2
    {
        public void PublicMethodA() { }
        public void PublicMethodB() { }
    };

    public class ProviderGenericClass<T, U> where T : new() where U : new()
    {
        public ProviderGenericClass() { }
        ~ProviderGenericClass() { }

        public T GetFirstTemplateArgument() { return new T(); }
        public U GetSecondTemplateArgument() { return new U(); }
    };

    class PropertyConsumer
    {
        public PropertyConsumer()
        {
            IntPropertyWithBackingField = 456;
            IntProperty = 123;
            EnumProperty = ProviderEnum.enum_val1;
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
                case ProviderEnum.enum_val1:
                    break;
                case ProviderEnum.enum_val2:
                    break;
                case ProviderEnum.enum_val3:
                    break;
                default:
                    break;
            }
        }

        public void MethodUsingStructMember()
        {
            if (StructProperty.member1 == 1 && StructProperty.member2 == "test") { }
        }

        public void MethodUsingClassMember()
        {
            ClassProperty.PublicMethodA();
            ClassProperty.PublicMethodB();
        }

        public void MethodUsingStdListMember()
        {
            // Use explicit type
            ProviderListTemplateArgument firstElement = ListClassProperty.First<ProviderListTemplateArgument>();
            firstElement.PublicMethodA();
            firstElement.PublicMethodB();

            // Use implicit type
            //ListClassProperty[0].PublicMethodC();
            //ListClassProperty[0].PublicMethodD();
        }

        public void MethodUsingGenericClassMember()
        {
            // Use explicit type
            ProviderTemplateArgument1? t = GenericClassProperty.GetFirstTemplateArgument();
            t?.PublicMethodA();
            t?.PublicMethodB();

            ProviderTemplateArgument2? u = GenericClassProperty.GetSecondTemplateArgument();
            u?.PublicMethodA();
            u?.PublicMethodB();

            // Use implicit type
            //GenericClassProperty.GetFirstTemplateArgument()?.PublicMethodC();
            //GenericClassProperty.GetFirstTemplateArgument()?.PublicMethodD();

            //GenericClassProperty.GetSecondTemplateArgument()?.PublicMethodC();
            //GenericClassProperty.GetSecondTemplateArgument()?.PublicMethodD();
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
