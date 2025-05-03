#pragma once

#include <list>


class VariableConsumer
{
public:
	VariableConsumer();
	~VariableConsumer();

	void MethodUsingIntVariable();
	void MethodUsingEnumVariable();
	void MethodUsingStructVariable();
	void MethodUsingUnionVariable();
	void MethodUsingClassVariable();
	void MethodUsingStdListVariable();
	void MethodUsingGenericClassVariable();
};


#include "VariableConsumer.h"

#include <memory>

#include "../Providers/ProviderGenericClass.h"
#include "../Providers/ProviderClass.h"
#include "../Providers/ProviderStruct.h"
#include "../Providers/ProviderUnion.h"
#include "../Providers/ProviderEnum.h"

VariableConsumer::VariableConsumer()
{
}

VariableConsumer::~VariableConsumer()
{

}
void VariableConsumer::MethodUsingIntVariable()
{
	int intVariable = 123;
}

void VariableConsumer::MethodUsingEnumVariable()
{
	ProviderEnum enumVariable = ProviderEnum::enum_val1;

	switch (enumVariable)
	{
	case  ProviderEnum::enum_val1:
		break;
	case  ProviderEnum::enum_val2:
		break;
	case ProviderEnum::enum_val3:
		break;
	default:
		break;
	}
}
void VariableConsumer::MethodUsingStructVariable()
{
	ProviderStruct structVariable;
	structVariable.member1 = 1;
	structVariable.member2 = "test";

}
void VariableConsumer::MethodUsingUnionVariable()
{
	ProviderUnion unionVariable;
	unionVariable.member1 = 1;
	unionVariable.member2 = 2.2;

}
void VariableConsumer::MethodUsingClassVariable()
{
	ProviderClass classVariable;
	classVariable.PublicMethodA();
	classVariable.PublicMethodB();
}

void VariableConsumer::MethodUsingStdListVariable()
{
	std::list<ProviderStdListTemplateArgument> stdListVariable1;

	// Use explicit type
	ProviderStdListTemplateArgument firstElement = stdListVariable1.front();
	firstElement.PublicMethodA();
	firstElement.PublicMethodB();

	// Use implicit type
	stdListVariable1.front().PublicMethodC();
	stdListVariable1.front().PublicMethodD();
}

void VariableConsumer::MethodUsingGenericClassVariable()
{
	ProviderGenericClass< ProviderTemplateArgument1, ProviderTemplateArgument2> genericClassVariable;

	// Use explicit type
	ProviderTemplateArgument1* t = genericClassVariable.GetFirstTemplateArgument();
	t->PublicMethodA();
	t->PublicMethodB();

	ProviderTemplateArgument2* u = genericClassVariable.GetSecondTemplateArgument();
	u->PublicMethodA();
	u->PublicMethodB();

	// Use implicit type
	genericClassVariable.GetFirstTemplateArgument()->PublicMethodC();
	genericClassVariable.GetFirstTemplateArgument()->PublicMethodD();

	genericClassVariable.GetSecondTemplateArgument()->PublicMethodC();
	genericClassVariable.GetSecondTemplateArgument()->PublicMethodD();
}



