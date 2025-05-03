#pragma once

#include <list>

#include "../Providers/ProviderGenericClass.h"
#include "../Providers/ProviderClass.h"
#include "../Providers/ProviderStruct.h"
#include "../Providers/ProviderUnion.h"
#include "../Providers/ProviderEnum.h"

class MemberConsumer
{
public:
	MemberConsumer();
	~MemberConsumer();

	void MethodUsingIntMember();
	void MethodUsingEnumMember();
	void MethodUsingStructMember();
	void MethodUsingUnionMember();
	void MethodUsingClassMember();
	void MethodUsingStdListMember();
	void MethodUsingGenericClassMember();
private:
	int _intMember;
	ProviderEnum _enumMember;
	ProviderStruct _structMember;
	ProviderUnion _unionMember;
	ProviderClass _classMember;
	std::list<ProviderStdListTemplateArgument> _stdListClassMember;
	ProviderGenericClass<ProviderTemplateArgument1, ProviderTemplateArgument2> _genericClassMember;
};



#include "MemberConsumer.h"

MemberConsumer::MemberConsumer() :
	_intMember(123),
	_enumMember(ProviderEnum::enum_val1),
	_structMember(1, "test"),
	_classMember(),
	_stdListClassMember(),
	_genericClassMember()
{
}

MemberConsumer::~MemberConsumer()
{

}

void MemberConsumer::MethodUsingIntMember()
{
	_intMember = 123;
}

void MemberConsumer::MethodUsingEnumMember()
{
	switch (_enumMember)
	{
	case ProviderEnum::enum_val1:
		break;
	case ProviderEnum::enum_val2:
		break;
	case ProviderEnum::enum_val3:
		break;
	default:
		break;
	}
}

void MemberConsumer::MethodUsingStructMember()
{
	_structMember.member1 = 1;
	_structMember.member2 = "test";
}

void MemberConsumer::MethodUsingUnionMember()
{
	_unionMember.member1 = 1;
	_unionMember.member2 = 2.2;
}

void MemberConsumer::MethodUsingClassMember()
{
	_classMember.PublicMethodA();
	_classMember.PublicMethodB();
}

void MemberConsumer::MethodUsingStdListMember()
{
	// Use explicit type
	ProviderStdListTemplateArgument firstElement = _stdListClassMember.front();
	firstElement.PublicMethodA();
	firstElement.PublicMethodB();

	// Use implicit type
	_stdListClassMember.front().PublicMethodC();
	_stdListClassMember.front().PublicMethodD();
}

void MemberConsumer::MethodUsingGenericClassMember()
{
	// Use explicit type
	ProviderTemplateArgument1* t = _genericClassMember.GetFirstTemplateArgument();
	t->PublicMethodA();
	t->PublicMethodB();

	ProviderTemplateArgument2* u = _genericClassMember.GetSecondTemplateArgument();
	u->PublicMethodA();
	u->PublicMethodB();

	// Use implicit type
	_genericClassMember.GetFirstTemplateArgument()->PublicMethodC();
	_genericClassMember.GetFirstTemplateArgument()->PublicMethodD();

	_genericClassMember.GetSecondTemplateArgument()->PublicMethodC();
	_genericClassMember.GetSecondTemplateArgument()->PublicMethodD();
}


