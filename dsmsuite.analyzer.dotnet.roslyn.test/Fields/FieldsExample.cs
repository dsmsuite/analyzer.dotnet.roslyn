using dsmsuite.analyzer.dotnet.roslyn.test.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsmsuite.analyzer.dotnet.roslyn.test.Fields
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

    public class FieldConsumer
    {
        public FieldConsumer()
        {
            _intMember = 123;
            _enumMember = ProviderEnum.enum_val1;
            _structMember = new ProviderStruct(1, "test");
            _classMember = new ProviderClass();
            _listClassMember = new List<ProviderListTemplateArgument>();
            _genericClassMember = new ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2>();
        }

        public void MethodUsingIntMember()
        {
            _intMember = 123;
        }

        public void MethodUsingEnumMember()
        {
            switch (_enumMember)
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
            _structMember.member1 = 1;
            _structMember.member2 = "test";
        }

        public void MethodUsingClassMember()
        {
            _classMember.PublicMethodA();
            _classMember.PublicMethodB();
        }

        public void MethodUsingStdListMember()
        {
            // Use explicit type
            ProviderListTemplateArgument firstElement = _listClassMember.First<ProviderListTemplateArgument>();
            firstElement.PublicMethodA();
            firstElement.PublicMethodB();

            // Use implicit type
            _listClassMember[0].PublicMethodC();
            _listClassMember[0].PublicMethodD();
        }

        public void MethodUsingGenericClassMember()
        {
            // Use explicit type
            ProviderTemplateArgument1? t = _genericClassMember.GetFirstTemplateArgument();
            t?.PublicMethodA();
            t?.PublicMethodB();

            ProviderTemplateArgument2? u = _genericClassMember.GetSecondTemplateArgument();
            u?.PublicMethodA();
            u?.PublicMethodB();

            // Use implicit type
            _genericClassMember.GetFirstTemplateArgument()?.PublicMethodC();
            _genericClassMember.GetFirstTemplateArgument()?.PublicMethodD();

            _genericClassMember.GetSecondTemplateArgument()?.PublicMethodC();
            _genericClassMember.GetSecondTemplateArgument()?.PublicMethodD();
        }

        private int _intMember;
        private ProviderEnum _enumMember;
        private ProviderStruct _structMember;
        private ProviderClass _classMember;
        private List<ProviderListTemplateArgument> _listClassMember;
        private ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2> _genericClassMember;
    };
}
