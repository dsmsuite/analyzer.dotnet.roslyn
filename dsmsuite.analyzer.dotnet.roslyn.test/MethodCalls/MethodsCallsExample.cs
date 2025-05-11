using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Methods
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

    class ParameterConsumer
    {
        public void MethodWithIntParameter(int intParameter)
        {
            intParameter = 123;
        }

        public void MethodWithEnumParameter(ProviderEnum enumParameter)
        {
            enumParameter = ProviderEnum.enum_val1;
        }

        public void MethodWithStructParameter(ProviderStruct structParameter)
        {
            structParameter.member1 = 1;
            structParameter.member2 = "test";
        }

        public void MethodWithClassParameter(ProviderClass classParameter)
        {
            classParameter.PublicMethodA();
            classParameter.PublicMethodB();
        }

        public void MethodWithNullableClassParameter(ProviderClass? classParameter)
        {
            classParameter?.PublicMethodA();
            classParameter?.PublicMethodB();
        }

        public void MethodWithStdListParameter(List<ProviderListTemplateArgument> listParameter)
        {
            ProviderListTemplateArgument firstElement = listParameter.First<ProviderListTemplateArgument>();
            firstElement.PublicMethodA();
            firstElement.PublicMethodB();

            // Use implicit type
            listParameter[0].PublicMethodC();
            listParameter[0].PublicMethodD();
        }

        public void MethodWithGenericClassParameter(ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2> genericClassParameter)
        {
            // Use explicit type
            ProviderTemplateArgument1? t = genericClassParameter.GetFirstTemplateArgument();
            t?.PublicMethodA();
            t?.PublicMethodB();

            ProviderTemplateArgument2? u = genericClassParameter.GetSecondTemplateArgument();
            u?.PublicMethodA();
            u?.PublicMethodB();

            // Use implicit type
            genericClassParameter.GetFirstTemplateArgument()?.PublicMethodC();
            genericClassParameter.GetFirstTemplateArgument()?.PublicMethodD();

            genericClassParameter.GetSecondTemplateArgument()?.PublicMethodC();
            genericClassParameter.GetSecondTemplateArgument()?.PublicMethodD();
        }
    };
}
