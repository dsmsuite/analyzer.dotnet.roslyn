#pragma once

#include <list>

#include "../Providers/ProviderGenericClass.h"
#include "../Providers/ProviderClass.h"
#include "../Providers/ProviderStruct.h"
#include "../Providers/ProviderUnion.h"
#include "../Providers/ProviderEnum.h"

class ParameterConsumer
{
public:
	ParameterConsumer();
	~ParameterConsumer();

	void MethodWithIntParameter(int intParameter);
	void MethodWithEnumParameter(ProviderEnum enumParameter);
	void MethodWithUnionParameter(ProviderUnion unionParameter);
	void MethodWithStructParameter(ProviderStruct structParameter);
	void MethodWithClassParameter(ProviderClass classParameter);
	void MethodWithStdListParameter(std::list<ProviderStdListTemplateArgument> stdListParameter);
	void MethodWithGenericClassParameter(ProviderGenericClass< ProviderTemplateArgument1, ProviderTemplateArgument2> genericClassParameter);
};


#include "ParameterConsumer.h"

#include <memory>

ParameterConsumer::ParameterConsumer()
{
}

ParameterConsumer::~ParameterConsumer()
{

}


void ParameterConsumer::MethodWithIntParameter(int intParameter)
{
	intParameter = 123;
}

void ParameterConsumer::MethodWithEnumParameter(ProviderEnum enumParameter) 
{
	enumParameter = ProviderEnum::enum_val1;
}

void ParameterConsumer::MethodWithUnionParameter(ProviderUnion unionParameter)
{
	unionParameter.member1 = 1;
	unionParameter.member2 = 2.2;
}

void ParameterConsumer::MethodWithStructParameter(ProviderStruct structParameter)
{
	structParameter.member1 = 1;
	structParameter.member2 = "test";
}

void ParameterConsumer::MethodWithClassParameter(ProviderClass classParameter)
{
	classParameter.PublicMethodA();
	classParameter.PublicMethodB();
}

void ParameterConsumer::MethodWithStdListParameter(std::list<ProviderStdListTemplateArgument> stdListParameter)
{
	// Use explicit type
	ProviderStdListTemplateArgument firstElement = stdListParameter.front();
	firstElement.PublicMethodA();
	firstElement.PublicMethodB();

	// Use implicit type
	stdListParameter.front().PublicMethodC();
	stdListParameter.front().PublicMethodD();
}

void ParameterConsumer::MethodWithGenericClassParameter(ProviderGenericClass< ProviderTemplateArgument1, ProviderTemplateArgument2> genericClassParameter)
{
	// Use explicit type
	ProviderTemplateArgument1* t = genericClassParameter.GetFirstTemplateArgument();
	t->PublicMethodA();
	t->PublicMethodB();

	ProviderTemplateArgument2* u = genericClassParameter.GetSecondTemplateArgument();
	u->PublicMethodA();
	u->PublicMethodB();

	// Use implicit type
	genericClassParameter.GetFirstTemplateArgument()->PublicMethodC();
	genericClassParameter.GetFirstTemplateArgument()->PublicMethodD();

	genericClassParameter.GetSecondTemplateArgument()->PublicMethodC();
	genericClassParameter.GetSecondTemplateArgument()->PublicMethodD();
}




